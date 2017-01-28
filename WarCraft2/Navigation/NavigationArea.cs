using C3.XNA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarCraft2.Common;

namespace WarCraft2.Navigation
{
    public enum NavigationCell : byte
    {
        /// <summary>
        /// Free to move.
        /// </summary>
        Free,

        /// <summary>
        /// A river, a rock
        /// </summary>
        PermanentObstacle,

        /// <summary>
        /// An obstacle which can be destroyed - a wall,
        /// a building, forest.
        /// </summary>
        TemporaryObstacle,

        /// <summary>
        /// Does not matter whether friendly, enemy or neutral.
        /// </summary>
        Unit,
    }

    public struct NavigationQuad
    {
        
    }

    public class NavigationArea : DrawableGameComponent
    {
        public Vector2 TopLeft { get; set; }

        public int Width { get { return Map != null ? Map.GetLength(0) : 0; } }

        public int Height { get { return Map != null ? Map.GetLength(1) : 0; } }

        public NavigationCell[,] Map { get; set; }

        public int CellSize { get; set; }

#if DEBUG
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private IList<Point> _currentRoute;
        private short[,] _currentField;
        private int _pointsTraversed;
        private Stopwatch _genTime = new Stopwatch();
#endif

        public NavigationArea(Game game) : base(game)
        {
            CellSize = 32;
        }

        public IList<Point> FindRoute(Point start, Point destination)
        {
            _genTime.Restart();
            _currentRoute = FindRoute(start, destination, ref _currentField);
            _genTime.Stop();
            return _currentRoute;
        }

        public IList<Point> FindRoute(Point start, Point destination, ref short[,] field)
        {
#if DEBUG
            _pointsTraversed = 0;
#endif
            var q = new Queue<Point>();
            field = new short[Width, Height];
            field[start.X, start.Y] = 1;
            q.Enqueue(start);
            while (q.Any())
            {
                var p = q.Dequeue();
#if DEBUG
                _pointsTraversed++;
#endif
                if (p == destination)
                {
                    var result = new List<Point>();
                    result.Add(p);
                    while (field[p.X, p.Y] != 1)
                    {
                        short minVal = field[p.X, p.Y];
                        Point nextP = p;
                        for (int x = Math.Max(0, p.X - 1); x <= Math.Min(Width - 1, p.X + 1); x++)
                        {
                            for (int y = Math.Max(0, p.Y - 1); y <= Math.Min(Height - 1, p.Y + 1); y++)
                            {
                                if (field[x, y] != 0)
                                {
                                    if (minVal > field[x, y])
                                    {
                                        minVal = field[x, y];
                                        nextP = new Point(x, y);
                                    }
                                }
                            }
                        }
                        result.Add(nextP);
                        p = nextP;
                    }
                    return result;
                }
                short range = field[p.X, p.Y];
                for (int x = Math.Max(0, p.X - 1); x <= Math.Min(Width - 1, p.X + 1); x++)
                {
                    for (int y = Math.Max(0, p.Y - 1); y <= Math.Min(Height - 1, p.Y + 1); y++)
                    {
                        if (Map[x, y] == NavigationCell.Free)
                        {
                            var val = range + 1;
                            if (x != p.X && y != p.Y)
                            {
                                val++;
                            }

                            if (field[x, y] == 0 || field[x, y] > val)
                            {
                                field[x, y] = (short)val;
                                q.Enqueue(new Point(x, y));
                            }
                        }
                    }
                }
            }
            return new Point[0];
        }

#if DEBUG

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _font = Game.Content.Load<SpriteFont>("DefaultFont");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            DrawArea(this);
            _spriteBatch.End();

            Game.Services.GetService<IDiagnostics>().SetParam("Navigation", "Points traversed: " + _pointsTraversed + 
                " Rt Len: " + (_currentRoute != null ? _currentRoute.Count : 0) +
                " Gen Time: " + _genTime.ElapsedMilliseconds);
            base.Draw(gameTime);
        }


        private void DrawArea(NavigationArea area)
        {
            var topLeft = area.TopLeft;
            var width = area.Width * CellSize;
            var height = area.Height * CellSize;

            _spriteBatch.FillRectangle(topLeft, new Vector2(width, height), new Color(Color.Green, 0.01f));

            for (int i = 0; i < area.Width; i++)
            {
                for (int j = 0; j < area.Height; j++)
                {
                    if (area.Map != null)
                    {
                        var cell = area.Map[i, j];
                        if (cell != NavigationCell.Free)
                        {
                            Color color = Color.Red;
                            switch (cell)
                            {
                                case NavigationCell.PermanentObstacle:
                                    color = Color.DarkBlue;
                                    break;
                                case NavigationCell.TemporaryObstacle:
                                    color = Color.Purple;
                                    break;
                                case NavigationCell.Unit:
                                    color = Color.Red;
                                    break;
                            }

                            _spriteBatch.FillRectangle(topLeft + new Vector2(i * CellSize, j * CellSize), new Vector2(CellSize, CellSize), new Color(color, 0.2f));
                        }
                    }
                }
            }


            if (_currentRoute != null)
            {
                foreach (var point in _currentRoute)
                {
                    _spriteBatch.FillRectangle(topLeft + new Vector2(point.X * CellSize, point.Y * CellSize), new Vector2(CellSize, CellSize), new Color(Color.BlueViolet, 0.5f));
                }
            }

            if (_currentField != null)
            {
                for (int i = 0; i < _currentField.GetLength(0); i++)
                {
                    for (int j = 0; j < _currentField.GetLength(1); j++)
                    {
                        if (_currentField[i, j] != 0)
                            _spriteBatch.DrawString(_font, _currentField[i, j] + "", new Vector2(i * CellSize, j * CellSize) + new Vector2(10), Color.Black);
                    }
                }
            }

            for (int i = 0; i < area.Width; i++)
            {
                _spriteBatch.DrawLine(new Vector2(topLeft.X + i * CellSize, topLeft.Y), new Vector2(topLeft.X + i * CellSize, topLeft.Y + height), Color.Black);
            }
            for (int i = 0; i < area.Width; i++)
            {
                _spriteBatch.DrawLine(new Vector2(topLeft.X, topLeft.Y + i * CellSize), new Vector2(topLeft.X + width, topLeft.Y + i * CellSize), Color.Black);
            }
        }
#endif
    }
}

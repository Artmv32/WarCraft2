using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C3.XNA;

namespace WarCraft2.Navigation
{
    public class NavigationSheet : DrawableGameComponent
    {
        public NavigationArea[,] Areas { get; set; }
        private IList<Point> _currentRoute;
        private short[,] _currentField;
        private SpriteFont _font;


        public int CellSize { get; set; }

        public Vector2 TopLeft { get; set; }

#if DEBUG
        private SpriteBatch _spriteBatch;
#endif

        public NavigationSheet(Game game) : base(game)
        {
            CellSize = 32;
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
            for (int i = 0; i < Areas.GetLength(0); i++)
            {
                for (int j = 0; j < Areas.GetLength(1); j++)
                {
                    DrawArea(Areas[i, j]);
                }
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void FindPath(Vector2 start, Vector2 end)
        {
            var pStart = new Point((int)(start.X / CellSize), (int)(start.Y / CellSize));
            var pEnd = new Point((int)(end.X / CellSize), (int)(end.Y / CellSize));

            _currentRoute = Areas[0, 0].FindRoute(pStart, pEnd, ref _currentField);
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

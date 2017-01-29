using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using EpPathFinding.cs;
using Microsoft.Xna.Framework.Graphics;
using C3.XNA;

namespace WarCraft2.PathFinder
{
    public class PathFinderImplementation : DrawableGameComponent, IPathFinder
    {
        private JumpPointParam jumpParam;
        private bool _initialized;

#if DEBUG
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private List<Point> _lastPath;

        public Vector2 CellSize = new Vector2(16, 16);
#endif

        public int Height
        {
            get
            {
                if (jumpParam?.SearchGrid != null)
                    return jumpParam.SearchGrid.height;
                return 0;
            }
        }

        public int Width
        {
            get
            {
                if (jumpParam?.SearchGrid != null)
                    return jumpParam.SearchGrid.width;
                return 0;
            }
        }

        public PathFinderImplementation(Game game) : base(game)
        {

        }

        public List<Point> FindRoute(Point a, Point b)
        {
            if (!_initialized)
                return new List<Point>();

            jumpParam.Reset(a, b);
            List<Point> resultList = JumpPointFinder.FindPath(jumpParam);
#if DEBUG
            _lastPath = resultList;
#endif
            return resultList;
        }

        public List<Point> FindRoute(Point a, Point b, IEnumerable<Point> nearUnits)
        {
            if (!_initialized)
                return new List<Point>();

            if (nearUnits != null)
            {
                var grid = jumpParam.SearchGrid;
                foreach (var unit in nearUnits)
                {
                    grid.SetWalkableAt(unit, false);
                }
            }

            List<Point> resultList = FindRoute(a, b);

            if (nearUnits != null)
            {
                var grid = jumpParam.SearchGrid;
                foreach (var unit in nearUnits)
                {
                    grid.SetWalkableAt(unit, true);
                }
            }

            return resultList;
        }

        public void Initialize(MapCell[,] mapInfo)
        {
            _initialized = false;

            int width = mapInfo.GetLength(0);
            int height = mapInfo.GetLength(1);
            var searchGrid = new StaticGrid(width, height);
            jumpParam = new JumpPointParam(searchGrid, true, true, true, HeuristicMode.EUCLIDEAN);//new JumpPointParam(searchGrid, startPos, endPos, cbCrossCorners.Checked, HeuristicMode.EUCLIDEANSQR);
            jumpParam.UseRecursive = false;
            for (int i = 0; i < mapInfo.GetLength(0); i++)
            {
                for (int j = 0; j < mapInfo.GetLength(1); j++)
                {
                    searchGrid.SetWalkableAt(i, j, mapInfo[i, j] == MapCell.Free);
                }
            }

            _initialized = true;
        }

        public bool IsFree(Point p)
        {
            if (!_initialized)
                return false;
            return jumpParam.SearchGrid.IsWalkableAt(p);
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

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var isWalkable = jumpParam.SearchGrid.IsWalkableAt(i, j);
                    var color = Color.Green;
                    if (!isWalkable)
                        color = Color.Red;

                    _spriteBatch.FillRectangle(CellSize * new Vector2(i, j), CellSize, new Color(color, 0.01f));
                    _spriteBatch.DrawLine(new Vector2(0, j * CellSize.Y), new Vector2(Width * CellSize.X, j * CellSize.Y), Color.Black);
                }
                _spriteBatch.DrawLine(new Vector2(i * CellSize.X, 0), new Vector2(i * CellSize.X, Height * CellSize.Y), Color.Black);
            }

            if (_lastPath != null)
            {
                for (int i = 1; i < _lastPath.Count; i++)
                {
                    var a = _lastPath[i - 1];
                    var b = _lastPath[i];
                    _spriteBatch.DrawLine(new Vector2(a.X, a.Y) * CellSize + new Vector2(16), new Vector2(b.X, b.Y) * CellSize + new Vector2(16), Color.Blue);
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
#endif
    }
}

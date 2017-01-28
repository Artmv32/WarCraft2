using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WarCraft2.PathFinder
{
    public enum MapCell : byte
    {
        Free,
        Obstacle,
    }

    public interface IPathFinder
    {
        int Width { get; }

        int Height { get; }

        List<Point> FindRoute(Point a, Point b);

        void Initialize(MapCell[,] mapInfo);

        bool IsFree(Point p);
    }
}

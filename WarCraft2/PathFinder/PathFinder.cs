using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using EpPathFinding.cs;

namespace WarCraft2.PathFinder
{
    public class PathFinder : IPathFinder
    {
        JumpPointParam jumpParam;



        public int Height
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Width
        {
            get
            {
                throw new NotImplementedException();
            }
        }
       

        public List<Point> FindRoute(Point a, Point b)
        {
            jumpParam.Reset(a, b);
            List<Point> resultList = JumpPointFinder.FindPath(jumpParam);
            return resultList;
        }

        public void Initialize(MapCell[,] mapInfo)
        {
            int width = mapInfo.GetLength(0);
            int height = mapInfo.GetLength(1);
            var searchGrid = new StaticGrid(width, height);
            jumpParam = new JumpPointParam(searchGrid, true, true, true, HeuristicMode.EUCLIDEAN);//new JumpPointParam(searchGrid, startPos, endPos, cbCrossCorners.Checked, HeuristicMode.EUCLIDEANSQR);
            jumpParam.UseRecursive = false;
        }

        public bool IsFree(Microsoft.Xna.Framework.Point p)
        {
            throw new NotImplementedException();
        }
    }
}

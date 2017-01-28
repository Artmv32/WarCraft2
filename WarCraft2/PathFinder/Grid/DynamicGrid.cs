/*! 
@file DynamicGrid.cs
@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
		<http://github.com/juhgiyo/eppathfinding.cs>
@date July 16, 2013
@brief DynamicGrid Interface
@version 2.0

@section LICENSE

The MIT License (MIT)

Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

@section DESCRIPTION

An Interface for the DynamicGrid Class.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;

namespace EpPathFinding.cs
{
    public class DynamicGrid : BaseGrid
    {
        protected Dictionary<Point, Node> m_nodes;
        private bool m_notSet;


        public override int width
        {
            get
            {
                if (m_notSet)
                    setBoundingBox();
                return m_gridRect.maxX - m_gridRect.minX;
            }
            protected set
            {

            }
        }

        public override int height
        {
            get
            {
                if (m_notSet)
                    setBoundingBox();
                return m_gridRect.maxY - m_gridRect.minY;
            }
            protected set
            {

            }
        }

        public DynamicGrid(List<Point> iWalkableGridList = null)
            : base()
        {
            m_gridRect = new GridRect();
            m_gridRect.minX = 0;
            m_gridRect.minY = 0;
            m_gridRect.maxX = 0;
            m_gridRect.maxY = 0;
            m_notSet = true;
            buildNodes(iWalkableGridList);
        }

        public DynamicGrid(DynamicGrid b)
            : base(b)
        {
            m_notSet = b.m_notSet;
            m_nodes = new Dictionary<Point, Node>(b.m_nodes);
        }

        protected void buildNodes(List<Point> iWalkableGridList)
        {

            m_nodes = new Dictionary<Point, Node>();
            if (iWalkableGridList == null)
                return;
            foreach (Point gridPos in iWalkableGridList)
            {
                SetWalkableAt(gridPos.X, gridPos.Y, true);
            }
        }


        public override Node GetNodeAt(int iX, int iY)
        {
            Point pos = new Point(iX, iY);
            return GetNodeAt(pos);
        }

        public override bool IsWalkableAt(int iX, int iY)
        {
            Point pos = new Point(iX, iY);
            return IsWalkableAt(pos);
        }

        private void setBoundingBox()
        {
            m_notSet = true;
            foreach (KeyValuePair<Point, Node> pair in m_nodes)
            {
                if (pair.Key.X < m_gridRect.minX || m_notSet)
                    m_gridRect.minX = pair.Key.X;
                if (pair.Key.X > m_gridRect.maxX || m_notSet)
                    m_gridRect.maxX = pair.Key.X;
                if (pair.Key.Y < m_gridRect.minY || m_notSet)
                    m_gridRect.minY = pair.Key.Y;
                if (pair.Key.Y > m_gridRect.maxY || m_notSet)
                    m_gridRect.maxY = pair.Key.Y;
                m_notSet = false;
            }
            m_notSet = false;
        }

        public override bool SetWalkableAt(int iX, int iY, bool iWalkable)
        {
            Point pos = new Point(iX, iY);

            if (iWalkable)
            {
                if (m_nodes.ContainsKey(pos))
                {
                   // this.m_nodes[pos].walkable = iWalkable;
                    return true;
                }
                else
                {
                    if (iX < m_gridRect.minX || m_notSet)
                        m_gridRect.minX = iX;
                    if (iX > m_gridRect.maxX || m_notSet)
                        m_gridRect.maxX = iX;
                    if (iY < m_gridRect.minY || m_notSet)
                        m_gridRect.minY = iY;
                    if (iY > m_gridRect.maxY || m_notSet)
                        m_gridRect.maxY = iY;
                    m_nodes.Add(new Point(pos.X, pos.Y), new Node(pos.X, pos.Y, iWalkable));
                    m_notSet = false;
                }
            }
            else
            {
                if (m_nodes.ContainsKey(pos))
                {
                    m_nodes.Remove(pos);
                    if (iX == m_gridRect.minX || iX == m_gridRect.maxX || iY == m_gridRect.minY || iY == m_gridRect.maxY)
                        m_notSet = true;
                }
            }
            return true;
        }

        public override Node GetNodeAt(Point iPos)
        {
            if (m_nodes.ContainsKey(iPos))
            {
                return m_nodes[iPos];
            }
            return null;
        }

        public override bool IsWalkableAt(Point iPos)
        {
            return m_nodes.ContainsKey(iPos);
        }

        public override bool SetWalkableAt(Point iPos, bool iWalkable)
        {
            return SetWalkableAt(iPos.X, iPos.Y, iWalkable);
        }

        public override void Reset()
        {
            Reset(null);
        }

        public void Reset(List<Point> iWalkableGridList)
        {

            foreach (KeyValuePair<Point, Node> keyValue in m_nodes)
            {
                keyValue.Value.Reset();
            }

            if (iWalkableGridList == null)
                return;
            foreach (KeyValuePair<Point, Node> keyValue in m_nodes)
            {
                if (iWalkableGridList.Contains(keyValue.Key))
                    SetWalkableAt(keyValue.Key, true);
                else
                    SetWalkableAt(keyValue.Key, false);
            }
        }

        public override BaseGrid Clone()
        {
            DynamicGrid tNewGrid = new DynamicGrid();

            foreach (KeyValuePair<Point, Node> keyValue in m_nodes)
            {
                tNewGrid.SetWalkableAt(keyValue.Key.X, keyValue.Key.Y, true);

            }

            return tNewGrid;
        }
    }

}

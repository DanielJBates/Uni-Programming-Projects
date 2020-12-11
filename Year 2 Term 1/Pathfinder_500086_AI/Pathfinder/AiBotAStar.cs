﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class AiBotAStar : AiBotBase
    {
        Coord2 mStartPos;
        Coord2 mTargetPos;
        int mGridSize;
        double[,] mGraph;
        List<Node> mNodes;
        List<Coord2> mPath;
        IDictionary<string, Node> mPathTracking;

        public AiBotAStar(int x, int y, double[,] pGraphMatrix, int pGridSize) : base(x, y)
        {
            mStartPos = new Coord2(x, y);
            mGraph = pGraphMatrix;
            mGridSize = pGridSize;
            mPath = new List<Coord2>();
            
        }
        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            mNodes = new List<Node>();
            mPathTracking = new Dictionary<string, Node>();
            mTargetPos = plr.GridPosition;

            Node startNode = new Node();
            startNode.parent = -1;
            startNode.level = 0;
            startNode.g_n = 0;
            startNode.position = mStartPos;
            startNode.index = PositionToVertex(startNode.position);
            startNode.h_n = Calculate_h_n(startNode.position, mTargetPos);
            startNode.f_n = startNode.g_n + startNode.h_n;

            mNodes.Add(startNode);
            mPathTracking.Add(startNode.index + "-" + startNode.level, startNode);

            Node currentNode = startNode;

            while(mNodes.Count > 0)
            {
                for (int i = 0; i < mGraph.GetLength(1); i++)
                {
                    if (mGraph[currentNode.index, i] >= 1)
                    {
                        Node newNode = new Node();

                        newNode.parent = currentNode.index;
                        newNode.g_n = currentNode.g_n + mGraph[currentNode.index, i];
                        newNode.level = currentNode.level + 1;
                        newNode.index = i;
                        newNode.position = VertexToPosition(i);
                        newNode.h_n = Calculate_h_n(newNode.position, mTargetPos);
                        newNode.f_n = newNode.g_n + newNode.h_n;

                        mNodes.Add(newNode);

                        if(mPathTracking.ContainsKey(newNode.index + "-" + newNode.level))
                        {
                            continue;
                        }
                        else
                        {
                            mPathTracking.Add(newNode.index + "-" + newNode.level, newNode);
                        }
                        
                    }
                }

                mNodes.Remove(currentNode);
                currentNode = getMinVertex(level);
                //SetNextGridPosition(currentNode.position, level);
                if (currentNode.position == mTargetPos)
                {
                    break;
                }                
            }

            while(currentNode.parent != -1)
            {
                mPath.Add(currentNode.position);
                Node temp;
                mPathTracking.TryGetValue(currentNode.parent + "-" + (currentNode.level - 1), out temp);
                currentNode = temp;
                              
            }

            for (int i = mPath.Count - 1; i >= 0; i--)
            {
                SetNextGridPosition(mPath[i], level);
            }

        }
            private Node getMinVertex(Level level)
            {
                double min = int.MaxValue;
                Node v = null;
                foreach (Node n in mNodes)
                {
                    if (n.f_n < min && level.ValidPosition(n.position))
                    {
                        min = n.f_n;
                        v = n;
                    }
                }
                return v;
            }
            private int PositionToVertex(Coord2 pPos)
            {
                return pPos.Y * mGridSize + pPos.X;
            }
            private Coord2 VertexToPosition(int pIndex)
            {
                Coord2 Pos;
                Pos.X = pIndex % mGridSize;
                Pos.Y = pIndex / mGridSize;

                return Pos;
            }
            private double Calculate_h_n(Coord2 pStart, Coord2 pTarget)
            {
                return Math.Sqrt(Convert.ToDouble(((pTarget.X - pStart.X) * (pTarget.X - pStart.X)) + ((pTarget.Y - pStart.Y) * (pTarget.Y - pStart.Y))));
            }
    }
}

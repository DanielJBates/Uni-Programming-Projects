using System;
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
        List<NodeAStar> mNodes;
        List<Coord2> mPath;
        IDictionary<string, NodeAStar> mPathTracking;

        public AiBotAStar(int x, int y, Coord2 pTarget, Level pLevel, double[,] pGraphMatrix) : base(x, y)
        {
            mStartPos = new Coord2(x, y);
            mTargetPos = pTarget;
            mGraph = pGraphMatrix;
            mGridSize = pLevel.GridSize;
            mNodes = new List<NodeAStar>();
            mPathTracking = new Dictionary<string, NodeAStar>();
            mPath = new List<Coord2>();
            build(pLevel);
        }
        private void build(Level pLevel)
        {
            NodeAStar startNode = new NodeAStar();
            startNode.parent = -1;
            startNode.level = 0;
            startNode.g_n = 0;
            startNode.position = mStartPos;
            startNode.index = PositionToVertex(startNode.position, mGridSize);
            startNode.h_n = Calculate_h_n(startNode.position, mTargetPos);
            startNode.f_n = startNode.g_n + startNode.h_n;

            mNodes.Add(startNode);
            mPathTracking.Add(startNode.index + "-" + startNode.level, startNode);

            NodeAStar currentNode = startNode;

            while(mNodes.Count > 0)
            {
                for (int i = 0; i < mGraph.GetLength(1); i++)
                {
                    if (mGraph[currentNode.index, i] >= 1)
                    {
                        NodeAStar newNode = new NodeAStar();

                        newNode.parent = currentNode.index;
                        newNode.g_n = currentNode.g_n + mGraph[currentNode.index, i];
                        newNode.level = currentNode.level + 1;
                        newNode.index = i;
                        newNode.position = VertexToPosition(i, mGridSize);
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
                currentNode = getMinVertex(pLevel);
                if (currentNode.position == mTargetPos)
                {
                    break;
                }                
            }

            while(currentNode.parent != -1)
            {
                mPath.Add(currentNode.position);
                NodeAStar temp;
                mPathTracking.TryGetValue(currentNode.parent + "-" + (currentNode.level - 1), out temp);
                currentNode = temp;
                              
            }

        }
        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            for (int i = mPath.Count - 1; i >= 0; i--)
            {
                SetNextGridPosition(mPath[i], level);
            }

        }
            private NodeAStar getMinVertex(Level pLevel)
            {
                double min = int.MaxValue;
                NodeAStar v = null;
                foreach (NodeAStar n in mNodes)
                {
                    if (n.f_n < min && pLevel.ValidPosition(n.position))
                    {
                        min = n.f_n;
                        v = n;
                    }
                }
                return v;
            }
            private double Calculate_h_n(Coord2 pStart, Coord2 pTarget)
            {
                return Math.Sqrt(Convert.ToDouble(((pTarget.X - pStart.X) * (pTarget.X - pStart.X)) + ((pTarget.Y - pStart.Y) * (pTarget.Y - pStart.Y))));
            }
    }
}

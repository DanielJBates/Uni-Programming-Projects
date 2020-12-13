using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class AiBotLRTAStar: AiBotBase
    {
        Coord2 mStartPos;
        int mCurrentVertex;
        Coord2 mTargetPos;
        int mTargetVertex;
        int mGridSize;
        double[,] mGraph;

        public AiBotLRTAStar(int x, int y, Coord2 pTarget, double[,] pGraphMatrix, int pGridSize) : base(x,y)
        {
            mStartPos = new Coord2(x, y);           
            mTargetPos = pTarget;
            mGraph = pGraphMatrix;
            mGridSize = pGridSize;
            nodesLRTA = new Dictionary<int, NodeLRTAStar>();

            Initalise();
        }
        private void Initalise()
        {
            NodeLRTAStar newNode;
            for (int i = 0; i < mGraph.GetLength(0); i++)
            {
                newNode = new NodeLRTAStar();
                newNode.index = i;
                newNode.position = VertexToPosition(newNode.index, mGridSize);
                newNode.stateCost = 0;

                nodesLRTA.Add(newNode.index, newNode);
            }
            mCurrentVertex = PositionToVertex(mStartPos, mGridSize);
            mTargetVertex = PositionToVertex(mTargetPos, mGridSize);
        }
        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            if (mCurrentVertex != mTargetVertex)
            {
                int nextVertex = LookAtNextMove(mCurrentVertex);
                Coord2 nextPosition = VertexToPosition(nextVertex, mGridSize);

                SetNextGridPosition(nextPosition, level);
                mCurrentVertex = nextVertex;
            }
        }
        private int LookAtNextMove(int pIndex)
        {
            double min = int.MaxValue;
            List<int> minVertex = new List<int>();
            NodeLRTAStar temp;

            for (int i = 0; i < mGraph.GetLength(0); i++)
            {
                nodesLRTA.TryGetValue(i, out temp);
                if (mGraph[pIndex, i] >= 1)
                {
                    if (min >= mGraph[pIndex, i] + temp.stateCost)
                    {
                        if (min > mGraph[pIndex, i] + temp.stateCost)
                        {
                            minVertex.Clear();
                        }
                        minVertex.Add(i);
                        min = mGraph[pIndex, i] + temp.stateCost;
                    }
                }
            }

            int nextVertex;
            if (minVertex.Count > 1)
            {
                Random rnd = new Random();
                int rndVertex = rnd.Next(minVertex.Count());

                nextVertex = minVertex[rndVertex];
            }
            else
            {
                nextVertex = minVertex[0];
            }
            StateCostUpdate(pIndex, nextVertex);
            return nextVertex;
        }
        private void StateCostUpdate(int pCurrentVertex, int pNextVertex)
        {
            NodeLRTAStar temp;
            nodesLRTA.TryGetValue(pCurrentVertex, out temp);
            temp.stateCost += mGraph[pNextVertex, pCurrentVertex];

            nodesLRTA[pCurrentVertex] = temp;
        }
    }
}

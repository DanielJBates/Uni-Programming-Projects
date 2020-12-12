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
        Coord2 mTargetPos;
        int mGridSize;
        double[,] mGraph;
        public AiBotLRTAStar(int x, int y, Coord2 pTarget, double[,] pGraphMatrix, int pGridSize) : base(x,y)
        {
            mStartPos = new Coord2(x, y);
            mTargetPos = pTarget;
            mGraph = pGraphMatrix;
            mGridSize = pGridSize;
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            throw new NotImplementedException();
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class AiBotSimple : AiBotBase
    {
        Random rnd;
        public AiBotSimple(int x, int y) : base(x, y)
        {
            rnd = new Random();
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            bool ok = false;
            Coord2 pos;// = new Coord2();
            while (!ok)
            {
                pos = GridPosition;
                int x = rnd.Next(0, 4);
                switch (x)
                {
                    case (0):
                        pos.X += 1;
                        break;
                    case (1):
                        pos.X -= 1;
                        break;
                    case (2):
                        pos.Y += 1;
                        break;
                    case (3):
                        pos.Y -= 1;
                        break;
                }
                ok = SetNextGridPosition(pos, level);
            }
        }
    }
}

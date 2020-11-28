using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class AiBotLRTAStar: AiBotBase
    {
        public AiBotLRTAStar(int x, int y): base(x,y)
        {
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            throw new NotImplementedException();
        }
    }
}

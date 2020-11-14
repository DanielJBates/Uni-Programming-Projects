using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno
{
    abstract class  GameType
    {
        protected enum mDirectionOptions {Clockwise, CounterClockwise};    
    }
    class Match : GameType
    {
        mDirectionOptions mDirection;
    }
    class SingleGame : GameType
    {
        mDirectionOptions mDirection;
    }
}

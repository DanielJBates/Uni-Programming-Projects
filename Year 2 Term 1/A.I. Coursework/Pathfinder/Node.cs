using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class Node
    {
        public int parent;
        public Coord2 position;
        public int index;
        public double f_n;
        public double g_n;
        public double h_n;
        public int level;
    }
}

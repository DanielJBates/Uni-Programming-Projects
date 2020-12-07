using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class AiBotAStar: AiBotBase
    {
        int startVertex;
        int targetVertex;
        double[,] graph;
        Dictionary<int, double> g_n;
        Dictionary<int, double> f_n;
        public AiBotAStar(int x, int y, double [,] graphMatrix): base(x,y)
        {
            startVertex = x;
            targetVertex = y;
            graph = graphMatrix;
            g_n = new Dictionary<int, double>();
            f_n = new Dictionary<int, double>();
        }
        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            
        }
        public void Build()
        {
            int startX = 0, startY = 0;
            int targetX = 2, targetY = 2;
            g_n.Add(startVertex, 0);

            double h_n = Math.Sqrt(Convert.ToDouble(((targetX - startX) * (targetX - startX)) + ((targetY - startY) * (targetY - startY))));

            f_n.Add(startVertex, 0 + h_n);

            int currentVetrex = startVertex;

            while (f_n.Count > 0)
            {

                for (int i = 0; i < graph.GetLength(1); i++)
                {
                    if (graph[currentVetrex, i] <= 1)
                    {
                        g_n.Add(i, graph[currentVetrex, i]);

                        int i_x = i % graph.GetLength(1);
                        int i_y = i / graph.GetLength(1);

                        h_n = Math.Sqrt(Convert.ToDouble(((targetX - i_x) * (targetX - i_x)) + ((targetY - i_y) * (targetY - i_y))));

                        f_n.Add(i, g_n[i] + h_n);
                    }
                }
                currentVetrex = getMinVertex();
            }
        }
        private int getMinVertex()
        {
            double min = int.MaxValue;
            int v = -1;
            foreach (KeyValuePair<int, double> f in f_n)
            {
                if (f.Value < min)
                {
                    min = f.Value;
                    v = f.Key;
                }
            }
            return v;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Relay
    {
        public Relay(List<int> t, List<int> delta)
        {
            this.t = t;
            this.delta = delta;
        }

        public List<int> t = new List<int>();
        public List<int> delta = new List<int>();
        public double r => delta.Where(x => x == 1).Count();
        public double n => t.Count();

        public double FirstSum() { 
            double sum = 0;

            for (int i = 0; i < n; i++)
            {
                sum += Math.Pow(t[i], 2);
            }

            return sum;
        }

        public double lambda() {
            return Math.Pow(1 / (2 * r) * FirstSum(), -2);
        }

    }
}

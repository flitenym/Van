using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Exponential
    {
        public Exponential(List<int> t, List<int> delta)
        {
            this.t = t;
            this.delta = delta;
        }

        public List<int> t = new List<int>();
        public List<int> delta = new List<int>();
        public double r => delta.Where(x => x == 1).Count();
        public double n => t.Count();

        public double lambda() {
            return r * Math.Pow(t.Sum(), -1);
        }

    }
}

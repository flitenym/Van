using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Exponential
    {
        public Exponential(List<double> t, List<double> delta)
        {
            this.t = t;
            this.delta = delta;
        }

        public List<double> t = new List<double>();
        public List<double> delta = new List<double>();
        public double r => delta.Where(x => x == 1).Count();
        public double n => t.Count();

        public double lambda() {
            return r * Math.Pow(t.Sum(), -1);
        }

    }
}

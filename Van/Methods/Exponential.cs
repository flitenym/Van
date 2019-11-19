using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Exponential
    {
        public Exponential(List<int> t, List<int> delta, double r)
        {
            LambdaCalculation(t, r);
        }
        public double lambda { get; set; }

        public void LambdaCalculation(List<int> t, double r)
        {
            this.lambda = r * Math.Pow(t.Sum(), -1);
        }

    }
}

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
            LCalculation(t, r);
        }
        public double lambda { get; set; }
        public double LValue { get; set; }

        public void LambdaCalculation(List<int> t, double r)
        {
            this.lambda = r * Math.Pow(t.Sum(), -1);
        }

        public void LCalculation(List<int> t, double r) {
            LValue = r * Math.Log(this.lambda) - this.lambda * t.Sum();
        }

    }
}

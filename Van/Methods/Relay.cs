using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Relay
    {
        public Relay(List<int> t, double r)
        {
            LambdaCalculation(t, r);
        } 

        public double lambda { get; set; }

        public double FirstSum(List<int> t) { 
            double sum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                sum += Math.Pow(t[i], 2);
            }

            return sum;
        }

        public void LambdaCalculation(List<int> t, double r) {
            lambda = Math.Pow(1 / (2 * r) * FirstSum(t), -2);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Relay
    {
        public Relay(List<int> t, List<int> delta, double r)
        {
            ParamterCalculation(t, delta, r);
        }

        public double lambda { get; set; }

        public double LValue { get; set; }

        public double FirstSum(List<int> t)
        {
            double sum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                sum += t[i];
            }

            return sum;
        }

        public void LambdaCalculation(List<int> t, double r)
        {
            lambda = Math.Sqrt(FirstSum(t) / (4 * Math.Sqrt(r)));
        }

        public void LCalculation(List<int> t, List<int> delta, double r) {
            double firstSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                firstSum += delta[i] * Math.Log(t[i] / Math.Pow(lambda, 2));
            }

            double secondSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                secondSum += Math.Pow(t[i], 2);
            }

            LValue = firstSum - 0.5 * Math.Pow(lambda, -2) * secondSum;
        }

        public void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            LambdaCalculation(t, r);

            LCalculation(t, delta, r);
        }

    }
}

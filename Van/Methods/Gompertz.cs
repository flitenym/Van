using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Gompertz
    {
        public Gompertz(List<int> t, List<int> delta, double r, double b, double epsilon, double? a = null)
        { 
            this.b = b;
            this.epsilon = epsilon;
            this.a = a != null ? (double)a : epsilon;

            LambdaAlphaCalculation(t, delta, r);
        }

        public double b { get; set; }
        public double epsilon { get; set; }
        public double a { get; set; }

        public double alpha { get; set; }
        public double lambda { get; set; }

        public double FirstSum(List<int> t, List<int> delta, double x) {
            double firstSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                firstSum += t[i] * delta[i];
            }

            return x * firstSum;
        }

        public double SecondSum(List<int> t, double r, double x)
        {
            double secondSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                secondSum += Math.Exp(t[i] * x);
            }

            return r * Math.Pow(secondSum - t.Count(), -1);
        }

        public double ThirdSum(List<int> t, double x)
        {
            double thirdSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                thirdSum += Math.Exp(t[i] * x);
            }

            return t.Count() + (1 - x * x) * thirdSum;
        }

        public double function(List<int> t, List<int> delta, double r, double x)
        {
            return FirstSum(t, delta, x) - SecondSum(t, r, x) * ThirdSum(t, x);
        }

        public double dichotomy(List<int> t, List<int> delta, double r)
        {
            double x;
            while (this.b - this.a > this.epsilon)
            {
                x = (this.a + this.b) / 2;
                if (this.function(t,delta,r,this.b) * this.function(t, delta, r, x) < 0)
                    this.a = x;
                else
                    this.b = x;
            }
            return (this.a + this.b) / 2;
        }

        public void LambdaAlphaCalculation(List<int> t, List<int> delta, double r)
        {
            alpha = dichotomy(t, delta, r);

            double sum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                sum += Math.Exp(t[i] * alpha);
            }

            lambda = alpha * r * Math.Pow(sum - t.Count(), -1);
        }

    }
}

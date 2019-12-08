using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Weibull
    {
        public Weibull(List<int> t, List<int> delta, double r, double b, double epsilon, double? a = null)
        {
            this.b = b;
            this.epsilon = epsilon;
            this.a = a != null ? (double)a : epsilon;

            ParamterCalculation(t, delta, r);
        }
        public double b { get; set; }
        public double epsilon { get; set; }
        public double a { get; set; }

        public double lambda { get; set; }
        public double gamma { get; set; }

        public double LValue { get; set; }

        public double FirstSum(List<int> t, List<int> delta, double r, double x)
        {
            double firstSum = 0;
            double secondSum = 0;
            double thirdSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                double tValue = t[i] == 0 ? 0.1 : t[i];

                firstSum += Math.Log(tValue) * delta[i];

                secondSum += Math.Pow(t[i], x);

                thirdSum += Math.Pow(t[i], x) * Math.Log(tValue);
            }

            secondSum = r * Math.Pow(secondSum, -1);

            return firstSum - secondSum * thirdSum;
        }

        public double function(List<int> t, List<int> delta, double r, double x)
        {
            return r / x + FirstSum(t, delta, r, x);
        }

        public double dichotomy(List<int> t, List<int> delta, double r)
        {
            double x;
            while (this.b - this.a > this.epsilon)
            {
                x = (this.a + this.b) / 2;
                if (this.function(t, delta, r, this.b) * this.function(t, delta, r, x) < 0)
                    this.a = x;
                else
                    this.b = x;
            }
            return (this.a + this.b) / 2;
        }

        public void LambdaGammaCalculate(List<int> t, List<int> delta, double r)
        {
            this.gamma = dichotomy(t, delta, r);

            double sum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                sum += Math.Pow(t[i], this.gamma);
            }

            this.lambda = r * Math.Pow(sum, -1);
        }

        public void LCalculation(List<int> t, List<int> delta, double r)
        {
            double firstSum = 0;
            double secondSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                firstSum += Math.Log(t[i] == 0 ? 0.1 : t[i]) * delta[i];

                secondSum += Math.Pow(t[i], gamma);
            }

            double paramsPow = lambda * gamma == 0 ? 0.1 : lambda * gamma;

            LValue = r * Math.Log(paramsPow) + (gamma - 1) * firstSum - lambda * secondSum;
        }

        public void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            LambdaGammaCalculate(t, delta, r);

            LCalculation(t, delta, r);
        }

    }
}

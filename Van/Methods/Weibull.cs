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

            LambdaGammaCalculate(t, delta, r);
        }
        public double b { get; set; }
        public double epsilon { get; set; }
        public double a { get; set; }

        public double lambda { get; set; }
        public double gamma { get; set; } 

        public double FirstSum(List<int> t, List<int> delta) {
            double firstSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                firstSum += Math.Log(t[i]) * delta[i];
            }

            return firstSum;
        }

        public double SecondSum(List<int> t, double x)
        {
            double secondSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                secondSum += Math.Pow(t[i], x);
            }

            return Math.Pow(secondSum, -1);
        }

        public double ThirdSum(List<int> t, double x)
        {
            double thirdSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                thirdSum += Math.Pow(t[i], x) * Math.Log(t[i]);
            }

            return thirdSum;
        }

        public double function(List<int> t, List<int> delta, double r, double x) {
            return r / x + FirstSum(t, delta) - r * SecondSum(t, x) * ThirdSum(t, x);
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

    }
}

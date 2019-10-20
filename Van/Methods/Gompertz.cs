using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Gompertz
    {
        public Gompertz(List<double> t, List<double> delta, double b, double epsilon, double? a = null)
        {
            this.t = t;
            this.delta = delta;
            this.b = b;
            this.epsilon = epsilon;
            this.a = a != null ? (double)a : epsilon;
        }

        public List<double> t = new List<double>();
        public List<double> delta = new List<double>();
        public double r => delta.Where(x => x == 1).Count();
        public double n => t.Count();
        public double b { get; set; }
        public double epsilon { get; set; }
        public double a { get; set; }


        public double FirstSum(double x) {
            double firstSum = 0;

            for (int i = 0; i < n; i++)
            {
                firstSum += t[i] * delta[i];
            }

            return x * firstSum;
        }

        public double SecondSum(double x)
        {
            double secondSum = 0;

            for (int i = 0; i < n; i++)
            {
                secondSum += Math.Exp(t[i] * x);
            }

            return r * Math.Pow(secondSum - n, -1);
        }

        public double ThirdSum(double x)
        {
            double thirdSum = 0;

            for (int i = 0; i < n; i++)
            {
                thirdSum += Math.Exp(t[i] * x);
            }

            return n + (1-x*x)*thirdSum;
        }

        public double function(double x) {
            return FirstSum(x) - SecondSum(x) * ThirdSum(x);
        }

        public double dichotomy()
        {
            double x;
            while (this.b - this.a > this.epsilon)
            {
                x = (this.a + this.b) / 2;
                if (this.function(this.b) * this.function(x) < 0)
                    this.a = x;
                else
                    this.b = x;
            }
            return (this.a + this.b) / 2;
        }

        public double lambda()
        {
            var alpha = dichotomy();

            double sum = 0;

            for (int i = 0; i < n; i++)
            {
                sum += Math.Exp(t[i] * alpha);
            }

            return alpha * r * Math.Pow(sum - n, -1);
        }

    }
}

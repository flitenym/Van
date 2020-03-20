using System;
using System.Collections.Generic;
using System.Linq;
using Van.Helper.Classes;
using Van.Helper.StaticInfo;
using Van.Methods.Helper;

namespace Van.Methods
{
    public class Weibull : MethodAbstractClass
    {
        public Weibull(List<double> tValue, List<int> t, double r, List<int> delta = null)
            : base(tValue, t, r, delta) { }

        public override void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            double a = SettingsDictionary.a;
            double b = SettingsDictionary.b;

            double FirstSum(double x)
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

            double function(double x)
            {
                return r / x + FirstSum(x);
            }

            double dichotomy()
            {
                double x;
                while (b - a > SettingsDictionary.epsilon)
                {
                    x = (a + b) / 2;
                    if (function(b) * function(x) < 0)
                        a = x;
                    else
                        b = x;
                }
                return (a + b) / 2;
            }

            alpha = dichotomy();

            double sum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                sum += Math.Pow(t[i], alpha);
            }

            lambda = r * Math.Pow(sum, -1);


            double fiveSum = 0;
            double sixSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                fiveSum += Math.Log(t[i] == 0 ? 0.1 : t[i]) * delta[i];

                sixSum += Math.Pow(t[i], alpha);
            }

            double paramsPow = lambda * alpha == 0 ? 0.1 : lambda * alpha;

            LValue = r * Math.Log(paramsPow) + (alpha - 1) * fiveSum - lambda * sixSum;
        }

        public override double SurvivalFunction(double tValue)
        {
            return Math.Round(
                    Math.Exp(-lambda * Math.Pow(tValue, alpha))
                    , SettingsDictionary.round);
        }

        public override double GetDensity(double tValue)
        {
            return Math.Round(
                lambda * alpha * Math.Pow(tValue, alpha - 1) * Math.Exp(-lambda * Math.Pow(tValue, alpha))
                    , SettingsDictionary.round);
        }
    }
}
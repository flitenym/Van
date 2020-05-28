using System;
using System.Collections.Generic;
using System.Linq;
using Van.DataBase.Models;
using Van.Helper.Classes;
using Van.Helper.StaticInfo;
using Van.Methods.Helper;

namespace Van.Methods
{
    public class Gompertz : MethodAbstractClass
    {
        public override int ParametrCount { get; set; } = 1;

        public Gompertz(List<double> tValue, List<int> t, double r, List<int> delta = null)
            : base(tValue, t, r, delta) { }

        public override void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            double a = SettingsDictionary.a;
            double b = SettingsDictionary.b;

            double FirstSum(double x)
            {
                double firstSum = 0;

                for (int i = 0; i < t.Count(); i++)
                {
                    firstSum += t[i] * delta[i];
                }

                return x * firstSum;
            }

            double SecondSum(double x)
            {
                double secondSum = 0;

                for (int i = 0; i < t.Count(); i++)
                {
                    secondSum += Math.Exp(t[i] * x);
                }

                return r * Math.Pow(secondSum - t.Count(), -1);
            }

            double ThirdSum(double x)
            {
                double thirdSum = 0;

                for (int i = 0; i < t.Count(); i++)
                {
                    thirdSum += Math.Exp(t[i] * x);
                }

                return t.Count() + (1 - x * x) * thirdSum;
            }

            double function(double x)
            {
                return FirstSum(x) - SecondSum(x) * ThirdSum(x);
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
                sum += Math.Exp(t[i] * alpha);
            }

            lambda = alpha * r * Math.Pow(sum - t.Count(), -1);

            LValue = r * lambda.GetLn() + FirstSum(alpha) + (lambda / alpha) * (t.Count() - sum);
        }

        public override double SurvivalFunction(double tValue)
        {
            return Math.Exp(lambda / alpha * (1 - Math.Exp(alpha * tValue)));
        }

        public override double GetDensity(double tValue)
        {
            return lambda *
                    Math.Exp(alpha * tValue) *
                    Math.Exp(lambda / alpha * (1 - Math.Exp(alpha * tValue)));
        }
    }
}
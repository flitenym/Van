using System;
using System.Collections.Generic;
using System.Linq;
using Van.Helper.Classes;
using Van.Helper.StaticInfo;
using Van.Methods.Helper;

namespace Van.Methods
{
    public class Gompertz : MethodAbstractClass
    {
        public Gompertz(List<double> StandartValues, List<double> tValue, List<int> t, double r, RangeData FirstAgeX, RangeData SecondAgeX, int parametrCount, List<int> delta = null)
            : base(StandartValues, tValue, t, r, FirstAgeX, SecondAgeX, parametrCount, delta) { }

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

            LValue = r * Math.Log(lambda) + FirstSum(alpha) + (lambda / alpha) * (t.Count() - sum);
        }

        public override double SurvivalFunction(double tValue)
        {
            return Math.Round(
                    Math.Exp(lambda / alpha * (1 - Math.Exp(alpha * tValue)))
                    , SettingsDictionary.round);
        }

        public override double GetDensity(double tValue)
        {
            return Math.Round(
                    lambda *
                    Math.Exp(alpha * tValue) *
                    Math.Exp(lambda / alpha * (1 - Math.Exp(alpha * tValue)))
                    , SettingsDictionary.round);
        }
    }
}
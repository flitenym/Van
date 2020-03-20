using System;
using System.Collections.Generic;
using System.Linq;
using Van.Helper.Classes;
using Van.Helper.StaticInfo;
using Van.Methods.Helper;

namespace Van.Methods
{
    public class Relay : MethodAbstractClass
    {
        public Relay(List<double> tValue, List<int> t, double r, List<int> delta = null)
            : base(tValue, t, r, delta) { }

        public override void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            lambda = Math.Sqrt(t.Sum(x => x * x) / (2.0 * r));

            double firstSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                firstSum += delta[i] * Math.Log((t[i] == 0 ? 0.1 : t[i]) / Math.Pow(lambda, 2));
            }

            double secondSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                secondSum += Math.Pow(t[i], 2);
            }

            LValue = firstSum - 0.5 * Math.Pow(lambda, -2) * secondSum;
        }

        public override double SurvivalFunction(double tValue)
        {
            return Math.Round(
                    Math.Exp(-Math.Pow(tValue, 2) / (2 * Math.Pow(lambda, 2)))
                    , SettingsDictionary.round);
        }

        public override double GetDensity(double tValue)
        {
            return Math.Round(
                    tValue * Math.Exp(-Math.Pow(tValue, 2) / (2 * Math.Pow(lambda, 2))) / Math.Pow(lambda, 2)
                    , SettingsDictionary.round);
        }
    }
}
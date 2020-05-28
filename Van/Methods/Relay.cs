using System;
using System.Collections.Generic;
using System.Linq;
using Van.DataBase.Models;
using Van.Helper.Classes;
using Van.Helper.StaticInfo;
using Van.Methods.Helper;

namespace Van.Methods
{
    public class Relay : MethodAbstractClass
    {
        public override int ParametrCount { get; set; } = 1;

        public Relay(List<double> tValue, List<int> t, double r, List<int> delta = null)
            : base(tValue, t, r, delta) { }

        public override void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            lambda = Math.Sqrt(t.Sum(x => x * x) / (2.0 * r));

            double firstSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                firstSum += delta[i] * (t[i]/ Math.Pow(lambda, 2)).GetLn();
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
            return Math.Exp(-Math.Pow(tValue, 2) / (2 * Math.Pow(lambda, 2)));
        }

        public override double GetDensity(double tValue)
        {
            return tValue * Math.Exp(-Math.Pow(tValue, 2) / (2 * Math.Pow(lambda, 2))) / Math.Pow(lambda, 2);
        }
    }
}
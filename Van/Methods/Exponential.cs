using System;
using System.Collections.Generic;
using System.Linq;
using Van.Helper.Classes;
using Van.Helper.StaticInfo;
using Van.Methods.Helper;

namespace Van.Methods
{
    public class Exponential : MethodAbstractClass
    {
        public Exponential(List<double> StandartValues, List<double> tValue, List<int> t, double r, RangeData FirstAgeX, RangeData SecondAgeX, int parametrCount, List<int> delta = null)
            : base(StandartValues, tValue, t, r, FirstAgeX, SecondAgeX, parametrCount, delta) { }

        public override void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            double tSum = t.Sum();

            //Вычисление параметра
            lambda = r / tSum;

            //Вычисление ФМП
            LValue = r * Math.Log(lambda == 0 ? 0.1 : lambda) - lambda * tSum;
        }

        public override double SurvivalFunction(double tValue)
        {
            return Math.Round(
                    Math.Exp(-lambda * tValue)
                    , SettingsDictionary.round);
        }

        public override double GetDensity(double tValue)
        {
            return Math.Round(
                    lambda * Math.Exp(-lambda * tValue)
                    , SettingsDictionary.round);
        }
    }
}
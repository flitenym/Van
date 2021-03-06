﻿using System;
using System.Collections.Generic;
using System.Linq;
using Van.DataBase.Models;
using Van.Methods.Helper;

namespace Van.Methods
{
    public class Exponential : MethodAbstractClass
    {
        public override int ParametrCount { get; set; } = 1;

        public Exponential(List<double> tValue, List<int> t, double r, List<int> delta = null)
            : base(tValue, t, r, delta) { }

        public override void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            double tSum = t.Sum();

            //Вычисление параметра
            lambda = r / tSum;

            //Вычисление ФМП
            LValue = r * lambda.GetLn() - lambda * tSum;
        }

        public override double SurvivalFunction(double tValue)
        {
            return Math.Exp(-lambda * tValue);
        }

        public override double GetDensity(double tValue)
        {
            return lambda * Math.Exp(-lambda * tValue);
        }
    }
}
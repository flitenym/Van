﻿using System;
using System.Collections.Generic;
using System.Linq;
using Van.Helper.Classes;
using Van.Helper.StaticInfo;

namespace Van.Methods.Helper
{
    public abstract class MethodAbstractClass
    {
        public MethodAbstractClass(List<double> StandartValues, List<double> tValue, List<int> t, double r, RangeData FirstAgeX, RangeData SecondAgeX, int parametrCount, List<int> delta = null)
        {
            ParamterCalculation(t, delta, r);
            GetSurvivalFunctions(tValue);
            GetQuality(StandartValues, FirstAgeX, SecondAgeX, LValue, t.Count(), parametrCount);
        }

        public List<double> SurvivalFunctions { get; set; }

        public List<double> Densitys { get; set; }

        /// <summary>
        /// Значение, которое используется для Acaici
        /// </summary>
        public double LValue { get; set; }

        public double alpha { get; set; }

        public double lambda { get; set; }

        public IDictionary<string, object> Quality = new Dictionary<string, object>();

        public abstract double SurvivalFunction(double tValue);

        public abstract double GetDensity(double tValue);

        public abstract void ParamterCalculation(List<int> t, List<int> delta, double r); 

        public void GetQuality(List<double> standartValues, RangeData FirstAgeX, RangeData SecondAgeX, double LValue, int tCount, int parametrCount)
        {
            if (!standartValues.Any()) return;

            double sumFirst = 0;
            double sumSecond = 0;


            for (int i = FirstAgeX.AgeX; i < SecondAgeX.AgeX; i++)
            {
                sumFirst += Shared.GetDistanceFirst(standartValues[i], SurvivalFunctions[i]);
                sumSecond += Shared.GetDistanceSecond(standartValues[i], SurvivalFunctions[i]);
            }

            Quality.Clear();
            Quality.Add(InfoKeys.AcaiciKey, Shared.GetQuality(LValue, parametrCount, tCount));
            Quality.Add(InfoKeys.DistanceFirstMethodKey, sumFirst);
            Quality.Add(InfoKeys.DistanceSecondMethodKey, Math.Sqrt(sumSecond));
        }

        public void GetSurvivalFunctions(List<double> tValue)
        {
            SurvivalFunctions = new List<double>();
            Densitys = new List<double>();

            for (int i = 0; i < tValue.Count; i++)
            {
                SurvivalFunctions.Add(SurvivalFunction(tValue[i]));
                Densitys.Add(GetDensity(tValue[i]));
            }
        }
    }
}
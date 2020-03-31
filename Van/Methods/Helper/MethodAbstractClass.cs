using System;
using System.Collections.Generic;
using System.Linq;
using Van.Helper.Classes;
using Van.Helper.StaticInfo;

namespace Van.Methods.Helper
{
    public abstract class MethodAbstractClass
    {
        public MethodAbstractClass()
        { }

        public MethodAbstractClass(List<double> tValue, List<int> t, double r, List<int> delta = null)
        {
            ParamterCalculation(t, delta, r);
            GetSurvivalFunctions(tValue);
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

        public void GetQuality(List<double> standartValues, List<double> survivalFunctions, RangeData FirstAgeX, RangeData SecondAgeX, double LValue, int tCount, int parametrCount)
        {
            List<double> distanceFirst = new List<double>();
            List<double> distanceSecond = new List<double>();

            //по каждому значению вычисляем оценку качества
            for (int i = FirstAgeX.AgeX; i <= SecondAgeX.AgeX; i++)
            {
                int valuesCount = SecondAgeX.AgeX - i;

                //вычислим оценку качества для них
                double sumFirst = 0;
                double sumSecond = 0;

                for (int j = 0; j < valuesCount; j++)
                {
                    double survivalFunctionsValue = survivalFunctions[i + j] / survivalFunctions[i];
                    double standartValue = standartValues[i + j] / standartValues[i];
                    sumFirst += Shared.GetDistanceFirst(standartValue, survivalFunctionsValue);
                    sumSecond += Shared.GetDistanceSecond(standartValue, survivalFunctionsValue);
                }

                distanceFirst.Add(sumFirst);
                distanceSecond.Add(Math.Sqrt(sumSecond));
            }

            Quality.Clear();
            Quality.Add(InfoKeys.AcaiciKey, Shared.GetQuality(LValue, parametrCount, tCount));
            Quality.Add(InfoKeys.DistanceFirstMethodKey, distanceFirst);
            Quality.Add(InfoKeys.DistanceSecondMethodKey, distanceSecond);
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
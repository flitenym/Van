using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.Helper.HelperClasses;

namespace Van.Methods
{
    public class Exponential
    {
        public Exponential(List<double> StandartValues, List<double> tValue, List<int> t, List<int> delta, int round, double r, RangeData FirstAgeX, RangeData SecondAgeX)
        {
            this.StandartValues = StandartValues;
            this.round = round;
            ParamterCalculation(t, r);
            this.Quality = Helper.Shared.GetQuality(this.LValue, 1, t.Count);
            GetSurvivalFunctions(tValue);
            this.FirstAgeX = FirstAgeX;
            this.SecondAgeX = SecondAgeX;
            GetDistances(); 
        }

        public RangeData FirstAgeX;

        public RangeData SecondAgeX;

        public List<double> StandartValues { get; set; }

        public List<double> SurvivalFunctions { get; set; }

        public List<double> Densitys { get; set; }

        /// <summary>
        /// Параметр округления
        /// </summary>
        public int round;

        public double lambda { get; set; }

        public double LValue { get; set; }

        public double DistanceFirstMethod { get; set; }

        public double DistanceSecondMethod { get; set; }

        public double Quality { get; set; }

        public void ParamterCalculation(List<int> t, double r)
        {
            double tSum = t.Sum();

            //Вычисление параметра
            this.lambda = r / tSum;

            //Вычисление ФМП
            LValue = r * Math.Log(this.lambda == 0 ? 0.1 : this.lambda) - this.lambda * tSum;
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

        public double SurvivalFunction(double tValue)
        {
            return Math.Round(
                    Math.Exp(-this.lambda * tValue)
                    , round);
        }

        public double GetDensity(double tValue)
        {
            return Math.Round(
                    this.lambda * Math.Exp(-this.lambda * tValue)
                    , round);
        }

        public void GetDistances()
        {
            if (!StandartValues.Any()) return;

            double sumFirst = 0;
            double sumSecond = 0;


            for (int i = FirstAgeX.AgeX; i < SecondAgeX.AgeX; i++)
            {
                sumFirst += Helper.Shared.GetDistanceFirst(StandartValues[i], SurvivalFunctions[i]);
                sumSecond += Helper.Shared.GetDistanceSecond(StandartValues[i], SurvivalFunctions[i]);
            }

            DistanceFirstMethod = sumFirst;
            DistanceSecondMethod = Math.Sqrt(sumSecond);
        }

    }
}

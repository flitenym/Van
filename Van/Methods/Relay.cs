using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.Helper.HelperClasses;

namespace Van.Methods
{
    public class Relay
    {
        public Relay(List<double> StandartValues, List<double> tValue, List<int> t, List<int> delta, int round, double r, RangeData FirstAgeX, RangeData SecondAgeX)
        {
            this.StandartValues = StandartValues;
            this.round = round;
            ParamterCalculation(t, delta, r);
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

        public double FirstSum(List<int> t)
        {
            double sum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                sum += t[i] * t[i];
            }

            return sum;
        }

        public void LambdaCalculation(List<int> t, double r)
        {
            lambda = Math.Sqrt(FirstSum(t) / (2.0 * r));
        }

        public void LCalculation(List<int> t, List<int> delta, double r)
        {
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

        public void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            LambdaCalculation(t, r);

            LCalculation(t, delta, r);
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
                    Math.Exp(-Math.Pow(tValue, 2) / (2 * Math.Pow(this.lambda, 2)))
                    , round);
        }

        public double GetDensity(double tValue)
        {
            return Math.Round(
                    tValue * Math.Exp(-Math.Pow(tValue, 2) / (2 * Math.Pow(this.lambda, 2))) / Math.Pow(this.lambda, 2)
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

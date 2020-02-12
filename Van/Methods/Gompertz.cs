using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.Helper.HelperClasses;

namespace Van.Methods
{
    public class Gompertz
    {
        public Gompertz(List<double> StandartValues, List<double> tValue, List<int> t, List<int> delta, int round, double r, double b, double epsilon, RangeData FirstAgeX, RangeData SecondAgeX, double? a = null)
        {
            this.StandartValues = StandartValues;
            this.round = round;
            this.b = b;
            this.epsilon = epsilon;
            this.a = a != null ? (double)a : epsilon;
            ParamterCalculation(t, delta, r);
            this.Quality = Helper.Shared.GetQuality(this.LValue, 2, t.Count);
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

        public double b { get; set; }

        public double epsilon { get; set; }

        public double a { get; set; }

        public double alpha { get; set; }

        public double lambda { get; set; }

        public double LValue { get; set; }

        public double DistanceFirstMethod { get; set; }

        public double DistanceSecondMethod { get; set; }

        public double Quality { get; set; }

        public double FirstSum(List<int> t, List<int> delta, double x)
        {
            double firstSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                firstSum += t[i] * delta[i];
            }

            return x * firstSum;
        }

        public double SecondSum(List<int> t, double r, double x)
        {
            double secondSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                secondSum += Math.Exp(t[i] * x);
            }

            return r * Math.Pow(secondSum - t.Count(), -1);
        }

        public double ThirdSum(List<int> t, double x)
        {
            double thirdSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                thirdSum += Math.Exp(t[i] * x);
            }

            return t.Count() + (1 - x * x) * thirdSum;
        }

        public double function(List<int> t, List<int> delta, double r, double x)
        {
            return FirstSum(t, delta, x) - SecondSum(t, r, x) * ThirdSum(t, x);
        }

        public double dichotomy(List<int> t, List<int> delta, double r)
        {
            double x;
            while (this.b - this.a > this.epsilon)
            {
                x = (this.a + this.b) / 2;
                if (this.function(t, delta, r, this.b) * this.function(t, delta, r, x) < 0)
                    this.a = x;
                else
                    this.b = x;
            }
            return (this.a + this.b) / 2;
        }

        public void LambdaAlphaCalculation(List<int> t, List<int> delta, double r)
        {
            alpha = dichotomy(t, delta, r);

            double sum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                sum += Math.Exp(t[i] * alpha);
            }

            lambda = alpha * r * Math.Pow(sum - t.Count(), -1);
        }

        public void LCalculation(List<int> t, List<int> delta, double r, double sum)
        {
            LValue = r * Math.Log(lambda) + FirstSum(t, delta, alpha) + (lambda / alpha) * (t.Count() - sum);
        }

        public void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            alpha = dichotomy(t, delta, r);

            double sum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                sum += Math.Exp(t[i] * alpha);
            }

            lambda = alpha * r * Math.Pow(sum - t.Count(), -1);

            LCalculation(t, delta, r, sum);
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
                    Math.Exp(this.lambda / this.alpha * (1 - Math.Exp(this.alpha * tValue)))
                    , round);
        }

        public double GetDensity(double tValue)
        {
            return Math.Round(
                    this.lambda *
                    Math.Exp(this.alpha * tValue) *
                    Math.Exp(this.lambda / this.alpha * (1 - Math.Exp(this.alpha * tValue)))
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

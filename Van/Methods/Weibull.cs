using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Weibull
    {
        public Weibull(List<double> StandartValues, List<double> tValue, List<int> t, List<int> delta, int round, double r, double b, double epsilon, double? a = null)
        {
            this.StandartValues = StandartValues;
            this.b = b;
            this.epsilon = epsilon;
            this.a = a != null ? (double)a : epsilon;
            this.round = round;
            ParamterCalculation(t, delta, r);
            this.Quality = Helper.Shared.GetQuality(this.LValue, 2, t.Count);
            GetSurvivalFunctions(tValue);
            GetDistances();
        }

        public List<double> StandartValues { get; set; }

        public List<double> SurvivalFunctions { get; set; }

        public List<double> Densitys { get; set; }

        /// <summary>
        /// Параметр округления
        /// </summary>
        public int round;

        /// <summary>
        /// Максимальное возможное (используется для выч. корней)
        /// </summary>
        public double b { get; set; }

        /// <summary>
        /// Параметр точности (используется для выч. корней)
        /// </summary>
        public double epsilon { get; set; }

        /// <summary>
        /// Минимальное возможное (используется для выч. корней)
        /// </summary>
        public double a { get; set; }

        /// <summary>
        /// Первый параметр
        /// </summary>
        public double lambda { get; set; }

        /// <summary>
        /// Второй параметр
        /// </summary>
        public double gamma { get; set; }

        /// <summary>
        /// Значение, которое используется для Acaici
        /// </summary>
        public double LValue { get; set; }

        public double DistanceFirstMethod { get; set; }

        public double DistanceSecondMethod { get; set; }

        public double Quality { get; set; }

        public double FirstSum(List<int> t, List<int> delta, double r, double x)
        {
            double firstSum = 0;
            double secondSum = 0;
            double thirdSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                double tValue = t[i] == 0 ? 0.1 : t[i];

                firstSum += Math.Log(tValue) * delta[i];

                secondSum += Math.Pow(t[i], x);

                thirdSum += Math.Pow(t[i], x) * Math.Log(tValue);
            }

            secondSum = r * Math.Pow(secondSum, -1);

            return firstSum - secondSum * thirdSum;
        }

        public double function(List<int> t, List<int> delta, double r, double x)
        {
            return r / x + FirstSum(t, delta, r, x);
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

        public void LambdaGammaCalculate(List<int> t, List<int> delta, double r)
        {
            this.gamma = dichotomy(t, delta, r);

            double sum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                sum += Math.Pow(t[i], this.gamma);
            }

            this.lambda = r * Math.Pow(sum, -1);
        }

        public void LCalculation(List<int> t, List<int> delta, double r)
        {
            double firstSum = 0;
            double secondSum = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                firstSum += Math.Log(t[i] == 0 ? 0.1 : t[i]) * delta[i];

                secondSum += Math.Pow(t[i], gamma);
            }

            double paramsPow = lambda * gamma == 0 ? 0.1 : lambda * gamma;

            LValue = r * Math.Log(paramsPow) + (gamma - 1) * firstSum - lambda * secondSum;
        }

        public void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            LambdaGammaCalculate(t, delta, r);

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
                    Math.Exp(-this.lambda * Math.Pow(tValue, this.gamma))
                    , round);
        }

        public double GetDensity(double tValue)
        {
            return Math.Round(
                this.lambda * this.gamma * Math.Pow(tValue, this.gamma - 1) * Math.Exp(-this.lambda * Math.Pow(tValue, this.gamma))
                    , round);
        }

        public void GetDistances() {
            if (!StandartValues.Any()) return;

            double sumFirst = 0;
            double sumSecond = 0;

            for (int i = 0; i < StandartValues.Count(); i++)
            {
                sumFirst += Helper.Shared.GetDistanceFirst(StandartValues[i], SurvivalFunctions[i]);
                sumSecond += Helper.Shared.GetDistanceSecond(StandartValues[i], SurvivalFunctions[i]);
            }

            DistanceFirstMethod = sumFirst;
            DistanceSecondMethod = Math.Sqrt(sumSecond);
        }

    }
}

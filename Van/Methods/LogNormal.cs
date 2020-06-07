using MahApps.Metro.Controls;
using SharedLibrary.LocalDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using Van.DataBase.Models;
using Van.Helper.Classes;
using Van.Helper.StaticInfo;
using Van.Methods.Helper;
using static Van.Methods.Helper.Shared;

namespace Van.Methods
{
    public class LogNormal : MethodAbstractClass
    {
        public override int ParametrCount { get; set; } = 2;

        public LogNormal(List<double> tValue, List<int> t, double r, List<int> delta = null)
            : base(tValue, t, r, delta) { }

        public double mu { get; set; }

        public double sigma { get; set; }
        
        private double GetZValue(double tValue)
        {
            return (tValue.GetLn() - mu) / sigma;
        }

        private double Get_first_first(double z)
        {
            return -z;
        }

        private double Get_first_second(double z)
        {
            return -GetDensityByZ(z) / GetSurvivalByZ(z);
        }

        private double Get_second_first(double z)
        {
            return -1;
        }

        private double Get_second_second(double z)
        {
            return z * GetDensityByZ(z) / GetSurvivalByZ(z) - Math.Pow(GetDensityByZ(z) / GetSurvivalByZ(z), 2);
        }

        private double[] Get_Q_Matrix(List<int> t, List<int> delta, double r)
        {
            double[] G = new double[2];
            double[,] H = new double[2, 2];

            double G_firstSum = 0;
            double G_secondSum = 0;

            double H_first_first_Sum = 0;
            double H_second_first_Sum = 0;
            double H_second_second_Sum_first = 0;

            for (int i = 0; i < t.Count(); i++)
            {
                double zValue = GetZValue(t[i]);

                var first = delta[i] * Get_first_first(zValue);
                var second = (1.0 - delta[i]) * Get_first_second(zValue);

                var third = delta[i] * Get_second_first(zValue);
                var fourth = (1.0 - delta[i]) * Get_second_second(zValue);

                G_firstSum += first + second;

                G_secondSum += zValue * first + second * zValue;

                H_first_first_Sum = third + fourth;

                H_second_first_Sum = third * zValue + fourth * zValue;

                H_second_second_Sum_first = third * zValue * zValue + fourth * zValue * zValue;
            }

            G[0] = -1.0 / sigma * G_firstSum;
            G[1] = -r / sigma - 1.0 / sigma * G_secondSum;

            H[0, 0] = 1.0 / (sigma * sigma) * H_first_first_Sum;
            H[0, 1] = 1.0 / (sigma * sigma) * G_firstSum + 1.0 / (sigma * sigma) * H_second_first_Sum;
            H[1, 0] = H[0, 1];
            H[1, 1] = r / (sigma * sigma) + 2.0 / (sigma * sigma) * G_secondSum + 1.0 / (sigma * sigma) * H_second_second_Sum_first;

            var inverseH = GetInverseHMatrix(H);

            return GetMultiplyMatrix(inverseH, G);
        }

        public override void ParamterCalculation(List<int> t, List<int> delta, double r)
        {
            //Вычислим мат ожидание и дисперсию
            double dispSum = 0;
            double matOj = 0;

            var probability = 1.0 / t.Count;

            for (int i = 0; i < t.Count; i++)
            {
                matOj += t[i].GetLn() * probability;
            }

            for (int i = 0; i < t.Count; i++)
            {
                dispSum += Math.Pow(t[i].GetLn(), 2) * probability;
            } 

            mu = matOj;
            sigma = Math.Sqrt(dispSum - matOj * matOj);

            var Q = Get_Q_Matrix(t, delta, r);

            for (int i = 0; ; i++)
            {
                mu = Q[0];
                sigma = Q[1];

                var nextQ = Get_Q_Matrix(t, delta, r);

                if (GetDifferenceMatrix(nextQ, Q))
                {
                    mu = nextQ[0];
                    sigma = nextQ[1];

                    break;
                }
                else
                {
                    Q = nextQ.Clone() as double[];
                }
            }
        }

        public override double SurvivalFunction(double tValue)
        {
            double z = GetZValue(tValue);
            return GetSurvivalByZ(z); 
        }

        public double GetSurvivalByZ(double zValue)
        {
            return 1.0 - GetDensityByZ(zValue);
        }

        public override double GetDensity(double tValue)
        {
            double z = GetZValue(tValue);
            return GetDensityByZ(z);
        }

        public double GetDensityByZ(double zValue)
        {
            return Math.Exp(-zValue * zValue / 2.0) / Math.Sqrt(2.0 * Math.PI);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using Van.Helper.StaticInfo;

namespace Van.Methods.Helper
{
    public static class Shared
    {
        public static double GetQuality(double LValue, int k, int n)
        {
            return -2.0 * LValue / n + 2.0 * k / n;
        }

        public static double GetDistanceFirst(double first, double second)
        {
            return Math.Abs(first - second);
        }

        public static double GetDistanceSecond(double first, double second)
        {
            return Math.Pow(first - second, 2);
        }

        /// <summary>
        /// Получение определителя двумерной матрицы
        /// </summary>
        public static double GetDetermine(double[,] matrix)
        {
            return matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
        }

        public static double[,] GetInverseHMatrix(double[,] H)
        {
            double[,] inverseH = new double[2, 2];

            var determine = GetDetermine(H);

            if (determine == 0)
            {
                inverseH[0, 0] = 0;
                inverseH[0, 1] = 0;
                inverseH[1, 0] = 0;
                inverseH[1, 1] = 0;
            }
            else
            {
                inverseH[0, 0] = H[1, 1] / determine;
                inverseH[0, 1] = H[1, 0] / -determine;
                inverseH[1, 0] = H[0, 1] / -determine;
                inverseH[1, 1] = H[0, 0] / determine;
            }

            return inverseH;
        }

        public static double[] GetMultiplyMatrix(double[,] H, double[] G)
        {
            return new double[] { Math.Abs(H[0, 0] * G[0] + H[0, 1] * G[1]), Math.Abs(H[1, 0] * G[0] + H[1, 1] * G[1]) };
        }

        public static bool GetDifferenceMatrix(double[] Q_first, double[] Q_second)
        {
            if (Math.Abs(Q_first[0] - Q_second[0]) < SettingsDictionary.epsilon && Math.Abs(Q_first[1] - Q_second[1]) < SettingsDictionary.epsilon) return true;

            return false;
        }

        public static double GetLn(this double num)
        {
            return num <= 0 ? Math.Log(SettingsDictionary.lnzero) : Math.Log(num);
        }

        public static double GetLn(this int num)
        {
            return num <= 0 ? Math.Log(SettingsDictionary.lnzero) : Math.Log(num);
        }

    }
}

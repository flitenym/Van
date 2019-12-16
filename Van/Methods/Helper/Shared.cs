using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}

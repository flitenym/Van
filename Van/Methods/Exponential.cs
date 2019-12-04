using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Methods
{
    public class Exponential
    {
        public Exponential(List<int> t, List<int> delta, double r)
        {
            ParamterCalculation(t, r);
        }
        public double lambda { get; set; }
        public double LValue { get; set; }

        public void ParamterCalculation(List<int> t, double r)
        {
            int tSum = t.Sum();

            //Вычисление параметра
            this.lambda = r / tSum;

            //Вычисление ФМП
            LValue = r * Math.Log(this.lambda) - this.lambda * tSum;
        }

    }
}

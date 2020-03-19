using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.LocalDataBase.ModelsHelper
{
    public interface IMethod
    {
        double? Standart { get; set; }
        double? Weibull { get; set; }
        double? Relay { get; set; }
        double? Gompertz { get; set; }
        double? Exponential { get; set; }
    }
}

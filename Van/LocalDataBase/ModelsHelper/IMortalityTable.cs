using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.LocalDataBase.ModelsHelper
{
    public interface IMortalityTable
    {
        int MortalityTableID { get; set; }
        string MortalityTableAgeX { get; set; }
    }
}

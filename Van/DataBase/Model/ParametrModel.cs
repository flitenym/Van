using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.DataBase.Model
{
    [Database(nameof(ParametrModel))]
    public class ParametrModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}

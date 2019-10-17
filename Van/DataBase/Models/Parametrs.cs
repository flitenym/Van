using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.DataBase.Models
{
    public class Parametrs : ModelClass
    {
        [Write(false)]
        [Computed]
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        [Write(false)]
        [Computed]
        public string TableName => nameof(Parametrs);

        [Write(false)]
        [Computed]
        public string TableTitle => "Параметры"; 
    }
}

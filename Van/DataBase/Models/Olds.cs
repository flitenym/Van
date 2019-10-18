using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.DataBase.Models
{
    [ModelTitle(TableTitle = "Возраста")]
    public class Olds : ModelClass
    {
        [Write(false)]
        [Computed]
        [Key]
        public int ID { get; set; }
        public int? Old { get; set; }
    }
}

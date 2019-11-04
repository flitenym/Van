using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.DataBase
{
    /// <summary>
    /// Если отсутствует TableTitle, то будет дефолтное название модели, если флаги отсутствуют, то все будет разрешено
    /// </summary>
    public class ModelClassAttribute : Attribute
    {
        public string TableTitle { get; set; }
        public bool CanInsert { get; set; } = true;
        public bool CanDelete { get; set; } = true;
        public bool CanUpdate { get; set; } = true;
    }
}

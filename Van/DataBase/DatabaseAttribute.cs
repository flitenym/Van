using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.DataBase
{
    public class DatabaseAttribute : Attribute
    {
        public string Name { get; set; }
        public int Code { get; set; }

        public DatabaseAttribute()
        { }

        public DatabaseAttribute(string name)
        {
            Name = name;
        }
    }
}

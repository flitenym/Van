using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Core.DataBase
{
    public abstract class ModelClass
    { 
        public string Title { get; set; }

        public bool CanInsert { get; set; }

        public bool CanDelete { get; set; }

        public bool CanUpdate { get; set; } 
    }
}

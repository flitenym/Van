﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.DataBase.Model
{
    [Database(nameof(OldModel))]
    public class OldModel
    {
        public int ID { get; set; }
        public int Old { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.DataBase
{ 
    public class ModelTitleAttribute : Attribute
    {
        public string TableTitle { get; set; }
    }
}
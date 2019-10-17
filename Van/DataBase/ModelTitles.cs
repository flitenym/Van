using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.DataBase.Models;

namespace Van.DataBase
{
    public static class ModelTitles
    {
        public static string GetModelTitle(string ModelName) { 
            switch (ModelName)
            {
                case nameof(Olds): return "Возраста";
                case nameof(Parametrs): return "Параметры";
                default: throw new Exception("Не верная таблица");
            } 
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Van.Helper
{
    static class StaticReflectionHelper
    {
        public static IEnumerable<T> CreateAllInstancesOf<T>()
        {
            return typeof(StaticReflectionHelper).Assembly.GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract && t.IsClass)
                .Select(t => (T)Activator.CreateInstance(t));
        }
    }
}
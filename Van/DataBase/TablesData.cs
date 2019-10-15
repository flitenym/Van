using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Van.DataBase
{
    public static class TablesData
    {
        private static string GetDescription(FieldInfo field)
        {  
            object[] attrs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs != null && attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            } 

            return field.ToString();
        }

        public static IEnumerable<string> GetDescriptions()
        {
            var assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes().Where(x => x.Name == nameof(TablesData)))
            {
                foreach (var field in type.GetFields())
                {
                    yield return GetDescription(field);
                }
            }
        }

        public static string GetValue(string description)
        {
            var assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes().Where(x => x.Name == nameof(TablesData)))
            {
                foreach (var field in type.GetFields())
                {
                    if (GetDescription(field) == description) {
                        return field.Name;
                    }
                }
            }
            return string.Empty;
        }


        [Description("Параметры")]
        public static string Parametrs = nameof(Parametrs);

        [Description("Возраста")]
        public static string Olds = nameof(Olds);


    } 
}

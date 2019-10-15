using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.DataBase.Model;
using Converter = Van.Helper.Helper; 

namespace Van.DataBase
{
    public static class Helper
    {
        public static DataTable GetDataByName(string typeName) {
            switch (typeName)
            {
                case nameof(OldModel):
                    return Converter.ToDataTable(SQLExecutor.Get<OldModel>(SelectQuery(typeName)));
                case nameof(ParametrModel):
                    return Converter.ToDataTable(SQLExecutor.Get<ParametrModel>(SelectQuery(typeName)));
                default: throw new Exception("Не найдена модель");
            }
        }

        private static string SelectQuery(string typeName) {
            return $"select * from {GetTableName(typeName)}";
        }

        private static string GetTableName(string typeName) {
            return typeName.Replace("Model", "");
        } 
    }
}

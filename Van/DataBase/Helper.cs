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
        public static DataTable ConvertToDataTable(IEnumerable<dynamic> items)
        {
            var t = new DataTable();
            var first = (IDictionary<string, object>)items.First();
            foreach (var k in first.Keys)
            {
                var c = t.Columns.Add(k);
                var val = first[k];
                if (val != null) c.DataType = val.GetType();
            }

            foreach (var item in items)
            {
                var r = t.NewRow();
                var i = (IDictionary<string, object>)item;
                foreach (var k in i.Keys)
                {
                    var val = i[k];
                    if (val == null) val = DBNull.Value;
                    r[k] = val;
                }
                t.Rows.Add(r);
            }
            return t;
        }

        private static string GetTableName(string typeName)
        {
            return typeName.Replace("Model", "");
        }

        #region Выбор

        private static string SelectQuery(string typeName) {
            return $"select * from {GetTableName(typeName)}";
        }

        public static DataTable GetData(string typeName)
        {
            return ConvertToDataTable(SQLExecutor.Get(SelectQuery(typeName)));
        }

        #endregion

        #region Удаление

        private static string DeleteQuery(string typeName, string conditions)
        {
            return $"delete from {GetTableName(typeName)} where {conditions}";
        }

        public static void DeleteData(string typeName, string conditions)
        {
            SQLExecutor.Delete(DeleteQuery(typeName, conditions));
        }

        #endregion

        #region Добавление

        private static string InsertQuery(string typeName)
        {
            return $@"INSERT INTO {GetTableName(typeName)}
                      SELECT CAST(last_insert_rowid() as int)";
        }

        public static void InsertData(string typeName)
        {
            SQLExecutor.Insert(InsertQuery(typeName));
        }

        #endregion

    }
}

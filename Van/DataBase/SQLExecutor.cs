using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Van.DataBase.Models;

namespace Van.DataBase
{
    public static class SQLExecutor
    {
        private static string LoadConnectionString(string id = "Default") {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
        
        #region Select

        public static string SelectQuery(string tableName)
        {
            return $@"SELECT * FROM {tableName}";
        }

        public static string SelectQuery(string tableName, int ID)
        {
            switch (tableName)
            {
                case nameof(Olds): return $@"SELECT * FROM {tableName} WHERE ID = {ID}";
                case nameof(Parametrs): return $@"SELECT * FROM {tableName} WHERE ID = {ID}";

                default: throw new Exception("Не верная таблица");
            }
        }

        public static DataTable SelectExecutor(string tableName) {
            switch (tableName) {
                case nameof(Olds): return Select<Olds>(SelectQuery(tableName)).ToDataTable();
                case nameof(Parametrs): return Select<Parametrs>(SelectQuery(tableName)).ToDataTable();
                default: throw new Exception("Не верная таблица");
            }
        }

        public static IEnumerable<dynamic> SelectExecutor(string tableName, int ID)
        {
            switch (tableName)
            {
                case nameof(Olds): return Select<Olds>(SelectQuery(tableName, ID));
                case nameof(Parametrs): return Select<Parametrs>(SelectQuery(tableName, ID));
                default: throw new Exception("Не верная таблица");
            }
        }

        public static IList<T> Select<T>(string query) {
            using (IDbConnection db = new SQLiteConnection(LoadConnectionString()))
            { 
                return db.Query<T>(query).ToList();
            }
        }

        #endregion

        #region Delete

        public static string DeleteQuery(string tableName, int ID)
        {
            return $@"DELETE FROM {tableName} WHERE ID = @ID";
        }

        public static void DeleteExecutor(string tableName, int ID)
        {
            switch (tableName)
            {
                case nameof(Olds): Delete(DeleteQuery(tableName, ID), ID); break;
                case nameof(Parametrs): Delete(DeleteQuery(tableName, ID), ID); break;
                default: throw new Exception("Не верная таблица");
            }
        }

        public static void Delete(string query, int ID)
        {
            using (IDbConnection db = new SQLiteConnection(LoadConnectionString()))
            {
                db.Execute(query, new { ID });
            }
        }

        #endregion 

        #region Insert

        public static string InsertQuery(string tableName)
        {
            switch (tableName)
            {
                case nameof(Olds): return $@"INSERT INTO {tableName}(Old) VALUES (@Old);  select last_insert_rowid()";
                case nameof(Parametrs): return $@"INSERT INTO {tableName}(Name) VALUES (@Name);  select last_insert_rowid()";

                default: throw new Exception("Не верная таблица");
            }
        }

        public static int InsertExecutor(string tableName, object item)
        {
            switch (tableName)
            {
                case nameof(Olds): return Insert(InsertQuery(tableName), item);
                case nameof(Parametrs): return Insert(InsertQuery(tableName), item);
                default: throw new Exception("Не верная таблица");
            }
        }

        public static int Insert(string query, object item)
        {
            using (IDbConnection db = new SQLiteConnection(LoadConnectionString()))
            {
                return db.ExecuteScalar<int>(query, item);
            }
        }

        #endregion

        #region Update

        public static string UpdateQuery(string tableName, int ID)
        {
            switch (tableName)
            {
                case nameof(Olds): return $@"UPDATE {tableName} SET Old = @Old WHERE ID = {ID}";
                case nameof(Parametrs): return $@"UPDATE {tableName} SET Name = @Name WHERE ID = {ID}";

                default: throw new Exception("Не верная таблица");
            }
        }

        public static void UpdateExecutor(string tableName, DataRow row, int ID)
        {
            switch (tableName)
            {
                case nameof(Olds): Update(UpdateQuery(tableName, ID), row.ToObject<Olds>()); break;
                case nameof(Parametrs): Update(UpdateQuery(tableName, ID), row.ToObject<Parametrs>()); break;
                default: throw new Exception("Не верная таблица");
            }
        } 

        public static void Update(string query, object item)
        {
            using (IDbConnection db = new SQLiteConnection(LoadConnectionString()))
            {
                db.Execute(query, item);
            }
        }

        #endregion

        #region Helper Methods

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static T ToObject<T>(this DataRow row) where T : new()
        {
            T obj = new T();
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                if (row.Table.Columns.Contains(property.Name) && property.CanWrite)
                {
                    property.SetValue(obj, row[property.Name]);
                }
            }

            return obj;
        }

        #endregion

    }
}

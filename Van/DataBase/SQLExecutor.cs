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
        private static string LoadConnectionString => ConfigurationManager.ConnectionStrings["Default"].ConnectionString; 
        
        #region Select 

        public static DataTable SelectExecutor(string tableName) {
            switch (tableName) {
                case nameof(MortalityTable): return Select<MortalityTable>($"SELECT * FROM {tableName}").ToDataTable();
                default: throw new Exception("Не верная таблица");
            }
        }

        public static IList<T> Select<T>(string query) {
            using (IDbConnection db = new SQLiteConnection(LoadConnectionString))
            { 
                return db.Query<T>(query).ToList();
            }
        }

        #endregion

        #region Delete

        public static void DeleteExecutor(string tableName, List<int> IDs)
        {
            switch (tableName)
            {
                case nameof(MortalityTable): Delete(tableName, IDs); break;
                default: throw new Exception("Не верная таблица");
            }
        }

        public static void Delete(string tableName, List<int> IDs)
        {
            using (IDbConnection db = new SQLiteConnection(LoadConnectionString))
            { 
                db.Execute($"DELETE FROM {tableName} WHERE ID = @ID", IDs.Select(x => new { Id = x }).ToArray()); 
            }
        }

        #endregion 

        #region Insert

        public static string InsertQuery(string tableName)
        {
            switch (tableName)
            {
                case nameof(MortalityTable): return MortalityTableQuery.InsertQuery;
                default: throw new Exception("Не верная таблица");
            }
        }

        public static int InsertExecutor(string tableName, object item)
        {
            switch (tableName)
            {
                case nameof(MortalityTable): return Insert(InsertQuery(tableName), item);
                default: throw new Exception("Не верная таблица");
            }
        }

        public static int Insert(string query, object item)
        {
            using (IDbConnection db = new SQLiteConnection(LoadConnectionString))
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
                case nameof(MortalityTable): return MortalityTableQuery.UpdateQuery(ID);
                default: throw new Exception("Не верная таблица");
            }
        }

        public static void UpdateExecutor(string tableName, DataRow row, int ID)
        {
            switch (tableName)
            {
                case nameof(MortalityTable): Update(UpdateQuery(tableName, ID), row.ToObject<MortalityTable>()); break; 
                default: throw new Exception("Не верная таблица");
            }
        } 

        public static void Update(string query, object item)
        {
            using (IDbConnection db = new SQLiteConnection(LoadConnectionString))
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
            {
                DataColumn column = new DataColumn(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType)
                {
                    Caption = string.IsNullOrEmpty(prop.Description) ? prop.Name : prop.Description
                };

                table.Columns.Add(column); 
            } 

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
                if (row.Table.Columns.Contains(property.Name) && property.CanWrite && property.DeclaringType != typeof(ModelClass))
                {
                    property.SetValue(obj, row[property.Name] == DBNull.Value ? null : row[property.Name]);
                }
            }

            return obj;
        }

        #endregion

    }
}

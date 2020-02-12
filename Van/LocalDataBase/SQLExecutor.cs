using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Van.AbstractClasses;
using Van.Helper;

namespace Van.LocalDataBase
{
    public static class SQLExecutor
    {
        public static string LoadConnectionString => ConfigurationManager.ConnectionStrings["LocalDataBase"].ConnectionString;

        public static DataTable SelectExecutor(Type type, string tableName, int round)
        {
            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    slc.Open();
                    return slc.Query(type, $"SELECT * FROM {tableName}").ToList().ToDataTable(type, round) ?? new DataTable();
                }
            }
            catch (Exception ex)
            {
                HelperMethods.Message(ex.ToString());
                return new DataTable();
            }
        }

        public static void DeleteExecutor(string tableName, List<int> IDs)
        {
            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    slc.Open();
                    slc.Execute($"DELETE FROM {tableName} WHERE ID = @ID", IDs.Select(x => new { Id = x }).ToArray());
                }
            }
            catch (Exception ex)
            {
                HelperMethods.Message(ex.ToString());
            }
        }

        public static int InsertExecutor(ModelClass item, object objData)
        {
            if (string.IsNullOrEmpty(item.InsertQuery)) return -1;

            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    slc.Open();
                    return slc.ExecuteScalar<int>(item.InsertQuery, objData);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.Message(ex.ToString());
                return -1;
            }
        }

        public static void UpdateExecutor(ModelClass item, Type type, DataRow row, int ID)
        {
            if (string.IsNullOrEmpty(item.UpdateQuery(ID))) return;

            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    slc.Open();
                    slc.Execute(item.UpdateQuery(ID), row.ToObject(type));
                }
            }
            catch (Exception ex)
            {
                HelperMethods.Message(ex.ToString());
            }
        }

        public static void UpdateExecutor(ModelClass item, object obj, int ID)
        {
            if (string.IsNullOrEmpty(item.UpdateQuery(ID))) return;
            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    slc.Open();
                    slc.Execute(item.UpdateQuery(ID), obj);
                }
            }
            catch (Exception ex)
            {
                HelperMethods.Message(ex.ToString());
            }
        }
    }
}

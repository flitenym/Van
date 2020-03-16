using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using Van.AbstractClasses;
using Van.Helper;
using System.Threading.Tasks;

namespace Van.LocalDataBase
{
    public static class SQLExecutor
    {
        public static string LoadConnectionString => ConfigurationManager.ConnectionStrings["LocalDataBase"].ConnectionString;

        public static async Task<DataTable> SelectExecutorAsync(Type type, string tableName)
        {
            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    await slc.OpenAsync(); 

                    var task = await Task.Run(async () =>
                    {
                        return await slc.QueryAsync(type, $"SELECT * FROM {tableName}");
                    });

                    return task.ToList().ToDataTable(type) ?? new DataTable();
                }
            }
            catch (Exception ex)
            {
                await HelperMethods.Message(ex.ToString());
                return new DataTable();
            }
        }

        public static async Task<List<T>> SelectExecutorAsync<T>(string tableName)
        {
            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    await slc.OpenAsync();

                    var task = await Task.Run(async () =>
                    {
                        return await slc.QueryAsync<T>($"SELECT * FROM {tableName}");
                    });

                    return task.ToList();
                }
            }
            catch (Exception ex)
            {
                await HelperMethods.Message(ex.ToString());
                return new List<T>();
            }
        }

        public static async Task DeleteExecutor(string tableName, List<int> IDs)
        {
            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    await slc.OpenAsync();
                    await Task.Run(() => slc.ExecuteAsync($"DELETE FROM {tableName} WHERE ID = @ID", IDs.Select(x => new { Id = x }).ToArray())); 
                }
            }
            catch (Exception ex)
            {
                await HelperMethods.Message(ex.ToString());
            }
        }

        public static async Task<int> InsertExecutorAsync(ModelClass item, object objData)
        {
            if (string.IsNullOrEmpty(item.InsertQuery)) return -1;

            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    await slc.OpenAsync(); 

                    var task = await Task.Run(async () =>
                    {
                        return await slc.ExecuteScalarAsync<int>(item.InsertQuery, objData);
                    });

                    return task;
                }
            }
            catch (Exception ex)
            {
                await HelperMethods.Message(ex.ToString());
                return -1;
            }
        }

        public static async Task UpdateExecutorAsync(ModelClass item, Type type, DataRow row, int ID)
        {
            if (string.IsNullOrEmpty(item.UpdateQuery(ID))) return;

            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    await slc.OpenAsync();
                    await Task.Run(() => slc.ExecuteAsync(item.UpdateQuery(ID), row.ToObject(type))); 
                }
            }
            catch (Exception ex)
            {
                await HelperMethods.Message(ex.ToString());
            }
        }

        public static async Task UpdateExecutorAsync(ModelClass item, object obj, int ID)
        {
            if (string.IsNullOrEmpty(item.UpdateQuery(ID))) return;
            try
            {
                using (var slc = new SQLiteConnection(LoadConnectionString))
                {
                    await slc.OpenAsync();
                    var result = slc.ExecuteAsync(item.UpdateQuery(ID), obj).Result;
                    await Task.Run(() => slc.ExecuteAsync(item.UpdateQuery(ID), obj));
                }
            }
            catch (Exception ex)
            {
                await HelperMethods.Message(ex.ToString());
            }
        }
    }
}

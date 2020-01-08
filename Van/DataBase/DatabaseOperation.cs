using Dapper;
using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using Van.Helper.StaticInfo;
using Van.LocalDataBase;
using Van.LocalDataBase.Models;
using static Van.Helper.HelperMethods;

namespace Van.DataBase
{
    public static class DatabaseOperation
    {
        public static string connectionString = string.Empty;
        public static bool canGetData = false;

        public static string ConnectionString()
        {
            if (!canGetData)
            {
                using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
                {
                    slc.Open();
                    var connectionStringData = slc.Query<Settings>($"SELECT * FROM {nameof(Settings)} Where Name = '{InfoKeys.ConnectionStringKey}'").FirstOrDefault();
                    if (connectionStringData != null && !string.IsNullOrEmpty(connectionStringData.Value))
                    {
                        TryConnection(connectionStringData.Value, connectionStringData);
                        if (canGetData)
                        {
                            connectionString = connectionStringData.Value;
                        }
                    }
                }
            }

            return connectionString;
        }

        public static void TryConnection(string ConnectionString, Settings connectionStringData = null)
        {
            Loading(true);
            try
            {
                connectionString = string.Empty;
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    connection.Close();
                    connectionString = ConnectionString;
                    canGetData = true;
                    Message("ConnectionString верный");

                    if (connectionStringData == null)
                    {
                        using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
                        {
                            slc.Open();
                            connectionStringData = slc.Query<Settings>($"SELECT * FROM {nameof(Settings)} Where Name = '{InfoKeys.ConnectionStringKey}'").FirstOrDefault();
                        }

                        if (connectionStringData == null)
                        {
                            connectionStringData = new Settings() { Name = InfoKeys.ConnectionStringKey, Value = connectionString };
                            SQLExecutor.InsertExecutor(connectionStringData, connectionStringData);
                        }
                    }

                    if (connectionStringData != null && connectionStringData.Value != connectionString)
                    {
                        connectionStringData.Value = connectionString;
                        SQLExecutor.UpdateExecutor(connectionStringData, connectionStringData, connectionStringData.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Message($"ConnectionString неверный, проверьте адрес сервера {ex.Message}");
                canGetData = false;
            }
            finally
            {
                Loading(false);
            }
        }
    }
}
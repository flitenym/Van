using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.DataBase.Model;

namespace Van.DataBase
{
    public static class SQLExecutor
    {
        private static string LoadConnectionString(string id = "Default") {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        //public static DataTable GetTableInfo(string sqlCommand)
        //{  
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        using (SQLiteConnection connection = new SQLiteConnection(DatabaseSource))
        //        {
        //            connection.Open();
        //            using (SQLiteCommand cmd = new SQLiteCommand(sqlCommand, connection))
        //            {
        //                var dataAdapter = new SQLiteDataAdapter(cmd);
        //                dataAdapter.Fill(dt);
        //                connection.Close();
        //                return dt;
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        Helper.Helper.Message("Выполнить невозможно" + Environment.NewLine + ex.Message);
        //        return null;
        //    }
        //    finally
        //    {

        //    }
        //}

        public static List<T> Get<T>(string sqlCommand)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<T>(sqlCommand);
                return output.ToList();
            }
        }

    }
}

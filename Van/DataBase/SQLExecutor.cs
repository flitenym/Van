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

        public static IEnumerable<dynamic> Get(string sqlCommand)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query(sqlCommand);
                return output;
            }
        }

        public static void Delete(string sqlCommand)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query(sqlCommand); 
            }
        }

    }
}

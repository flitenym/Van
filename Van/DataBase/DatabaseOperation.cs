using System;
using System.Data.SqlClient;
using static Van.Helper.HelperMethods;

namespace Van.DataBase
{
    public static class DatabaseOperation
    {
        public static string connectionString = string.Empty;
        public static bool canGetData = false;

        public static void TryConnection(string ConnectionString)
        {
            Loading(true);
            try
            {
                connectionString = string.Empty;
                canGetData = false;
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    connection.Close();
                    connectionString = ConnectionString;
                    canGetData = true;
                    Message("ConnectionString верный");
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

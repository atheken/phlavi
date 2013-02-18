using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PropertyLookup
{
    /// <summary>
    /// Data Access Class
    /// </summary>
    public class DataAccess : IDisposable
    {
        private SqlConnection conn;

        public DataAccess()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnStringPHLAVI"].ConnectionString;
            conn = new SqlConnection(connStr);
            conn.Open();
        }

        public SqlDataReader RunQuery(String ProcedureName, String ParameterValue, String ParameterName)
        {
            SqlCommand command = new SqlCommand(ProcedureName, conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue(ParameterName, ParameterValue);      
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        
        public void Dispose()
        {
            conn.Close();
        }
    }
}
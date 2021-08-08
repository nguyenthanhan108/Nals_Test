using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
namespace DBConnection
{
   public class SqlServerDB
    {
       public string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionSQL"].ConnectionString;
        public DbProviderFactory factory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.SqlClient");


        private static void AttachParameters(SqlCommand command, params SqlParameter[] commandParameters)
        {
            if ((command == null))
                throw new ArgumentNullException("command");
            if (((commandParameters != null))) 
            foreach (var p in commandParameters.Where(p => ((p != null))))
            {
                if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
                command.Parameters.Add(p);
            }
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction,
                                   CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            if ((command == null))
                throw new ArgumentNullException("command");
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentNullException("commandText");
            command.Connection = connection;
            command.CommandText = commandText;
            if ((transaction != null))
            {
                if (transaction.Connection == null)
                    throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            command.CommandType = commandType;
            if ((commandParameters != null))
            {
                AttachParameters(command, commandParameters);
            }
        }
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection connection;
            using (connection = (SqlConnection)factory.CreateConnection())
            {
                if (connection == null) return null;
                connection.ConnectionString = connectionString;
                try
                {
                    connection.Open();
                }
                catch
                {
                    connection.Open();
                }
                SqlCommand command;
                var dt = new DataTable();
                using (command = (SqlCommand)factory.CreateCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    var dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(dt);
                    dataAdapter.Dispose();
                    return dt;
                }
            }
        }
        public int ExecuteNonQuery( CommandType commandType, string commandText,params SqlParameter[] commandParameters)
        {
           SqlConnection connection;
            using (connection = (SqlConnection)factory.CreateConnection())
            {
                if (connection == null) return -1;
                connection.ConnectionString = connectionString;
                try
                {
                    connection.Open();
                }
                catch
                {
                    connection.Open();
                }
                SqlCommand command;
                int retval;
                using (command = (SqlCommand)factory.CreateCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    retval= command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    return retval;
                }
            }
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection connection;
            using (connection = (SqlConnection)factory.CreateConnection())
            {
                if (connection == null) return null;
                connection.ConnectionString = connectionString;
                try
                {
                    connection.Open();
                }
                catch
                {
                    connection.Open();
                }
                SqlCommand command;
                var ds = new DataSet();
                using (command = (SqlCommand)factory.CreateCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    var dataAdapter = new SqlDataAdapter(command);
                    dataAdapter.Fill(ds);
                    dataAdapter.Dispose();
                    return ds;
                }
            }
        }

        public Object ExecuteScalar(CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlConnection connection;
            using (connection = (SqlConnection)factory.CreateConnection())
            {
                if (connection == null) return null;
                connection.ConnectionString = connectionString;
                try
                {
                    connection.Open();
                }
                catch
                {
                    connection.Open();
                }
                SqlCommand command;
                using (command = (SqlCommand)factory.CreateCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    return command.ExecuteScalar();
                }
            }
        }
        public static string GetConnectionString( string sConnectionString)
        {
           string nodeContent ="";
            try
            {
                string sPath = System.Configuration.ConfigurationManager.AppSettings.Get("PathConfigFile") + "\\Configs.xml";
                XmlDocument doc = new XmlDocument ();
                doc.Load(sPath);
                nodeContent=doc.SelectSingleNode("/Configs/" + sConnectionString).InnerText;
                return nodeContent;
            }
            catch (Exception ex)
            {
                return"";
            }
        }       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.Xml;

namespace DBConnection
{
   public class OracleDB
    {
       public string connectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        public DbProviderFactory factory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.OracleClient");


        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
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

        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction,
        CommandType commandType, string commandText, OracleParameter[] commandParameters)
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

        public DataTable ExecuteDataTable(CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleConnection connection;
            using (connection = (OracleConnection) factory.CreateConnection())
            {
                if (connection == null) return null;
                connection.ConnectionString = connectionString;
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    connection.Open();
                }
                OracleCommand command;
                var dt = new DataTable();
                using (command = (OracleCommand) factory.CreateCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    var dataAdapter = new OracleDataAdapter(command);
                    dataAdapter.Fill(dt);
                    dataAdapter.Dispose();
                    return dt;
                }
            }
        }
        public int ExecuteNonQuery(CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleConnection connection;
            using (connection = (OracleConnection)factory.CreateConnection())
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
                OracleCommand command;
                int retval;
                using (command = (OracleCommand)factory.CreateCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    retval = command.ExecuteNonQuery();
                    command.Parameters.Clear();
                    return retval;
                }
            }
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleConnection connection;
            using (connection = (OracleConnection)factory.CreateConnection())
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
                OracleCommand command;
                var ds = new DataSet();
                using (command = (OracleCommand)factory.CreateCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    var dataAdapter = new OracleDataAdapter(command);
                    dataAdapter.Fill(ds);
                    dataAdapter.Dispose();
                    return ds;
                }
            }
        }

        public Object ExecuteScalar(CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleConnection connection;
            using (connection = (OracleConnection)factory.CreateConnection())
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
                OracleCommand command;
                using (command = (OracleCommand)factory.CreateCommand())
                {
                    PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                    return command.ExecuteScalar();
                }
            }
        }
        public static string GetConnectionString(string sConnectionString)
        {
            string nodeContent = "";
            try
            {
                string sPath = System.Configuration.ConfigurationManager.AppSettings.Get("PathConfigFile") + "\\Configs.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(sPath);
                nodeContent = doc.SelectSingleNode("/Configs/" + sConnectionString).InnerText;
                return nodeContent;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public int InsertMutil(DataTable dt, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleConnection connection;
            using (connection = (OracleConnection)factory.CreateConnection())
            {
                if (connection != null)
                {
                    connection.ConnectionString = connectionString;
                    try
                    {
                        connection.Open();
                    }
                    catch
                    {
                        connection.Open();
                    }
                    int retval = 0;
                    OracleCommand command;
                    using (command = (OracleCommand)factory.CreateCommand())
                    {
                        PrepareCommand(command, connection, null, commandType, commandText, commandParameters);
                        if (command != null)
                        {
                            OracleDataAdapter adpt = new OracleDataAdapter
                            {
                                InsertCommand = command,
                                UpdateBatchSize = 2
                            };
                            // Specify the number of records to be Inserted/Updated in one go. Default is 1.
                            command.UpdatedRowSource = UpdateRowSource.None;
                            retval = adpt.Update(dt);
                            command.Parameters.Clear();
                        }
                        return retval;
                    }
                }
            }

            return 0;
        }
    }
}

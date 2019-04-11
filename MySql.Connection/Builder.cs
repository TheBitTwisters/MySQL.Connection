using MySql.Data.MySqlClient;
using System.Data;

namespace MySql.Connection
{
    public abstract class Builder
    {

        public static string ConnectionString
        {
            get 
            {
                var csb = new MySqlConnectionStringBuilder();
                csb.UserID = UserID;
                csb.Password = Password;
                csb.Server = Server;
                csb.Port = Port;
                csb.Database = Database;
                csb.SqlServerMode = SqlServerMode;
                return csb.ConnectionString;
            }
        }

        public static string UserID { get; set; }
        public static string Password { get; set; }
        public static string Server { get; set; }
        public static uint Port { get; set; }
        public static string Database { get; set; }
        public static bool SqlServerMode { get; set; }

        public static ConnectionState ConnectionState
        {
            get { return State(LogOptions.LogToFile); }
        }

        public static bool Connected
        {
            get { return (State(LogOptions.PromptTheUser) == ConnectionState.Open); }
        }

        private static ConnectionState State(LogOptions logOption)
        {
            using (var con = new MySqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                }
                catch (MySqlException ex)
                {
                    ErrorTrapper.Log(ex, logOption);
                }
                return con.State;
            }
        }

    }
}

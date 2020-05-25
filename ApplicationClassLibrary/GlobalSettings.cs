using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationClassLibrary.Connections;
using System.Configuration;

namespace ApplicationClassLibrary
{
    public class GlobalSettings
    {

        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnection(DataBaseType db)
        {

            if (db == DataBaseType.SQLServer)
            {
                SQLServerConnection SQLServer = new SQLServerConnection();
                Connection = SQLServer;
            }
            else if (db == DataBaseType.MySQL)
            {

                MySQLConnection mySQL = new MySQLConnection();
                Connection = mySQL;
            }
        }

        public static string ConnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}

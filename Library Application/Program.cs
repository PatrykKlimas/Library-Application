using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Application
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Initialize the database connections
            ApplicationClassLibrary.GlobalSettings.InitializeConnection(ApplicationClassLibrary.DataBaseType.SQLServer); 
            Application.Run(new LogInForm());
        }
    }
}

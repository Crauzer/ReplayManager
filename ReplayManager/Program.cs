using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReplayManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            String ConfigFolder = Application.UserAppDataPath;
            //if (File.Exists(ConfigFolder + "\\config.json"))
            {
                Application.Run(new ReplayManager());
            }
            //else
            {
                //First Run:
            }
        }
    }
}

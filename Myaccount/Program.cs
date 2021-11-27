using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;


namespace Myaccount
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        private static bool Filecheck()
        {
            string path = @"Config.bat";
            if (!File.Exists(path))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Filecheck() == true)
            {
                Application.Run(new Form3());
            }
            else if(Filecheck() == false)
            {
                Application.Run(new Form5());
            }
            
        }
    }
}

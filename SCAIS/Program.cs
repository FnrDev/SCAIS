using System;
using System.Windows.Forms;

namespace SCAIS
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Database is now in the project root folder, not using DataDirectory
            // All changes will persist in C:\Users\ahmed-pc\Documents\SCAIS\SCAIS\SCAISDB.mdf

            Application.Run(new login());
        }
    }
}

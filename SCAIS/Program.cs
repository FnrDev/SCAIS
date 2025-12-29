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

            // Set DataDirectory to the executable's directory
            // The database file should be in the same folder as the .exe
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

            Application.Run(new login());
        }
    }
}

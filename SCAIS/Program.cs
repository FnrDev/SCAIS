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

            // Resolve |DataDirectory| to the executable folder so AttachDbFilename=|DataDirectory|\SCAISDB.mdf uses your repo copy at runtime
            AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

            Application.Run(new login());
        }
    }
}

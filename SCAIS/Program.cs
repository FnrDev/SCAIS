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

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = System.IO.Path.GetFullPath(System.IO.Path.Combine(baseDirectory, @"..\..\"));
            AppDomain.CurrentDomain.SetData("DataDirectory", projectRoot);

            Application.Run(new login());
        }
    }
}

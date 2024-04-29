using System;
using System.Windows.Forms;

namespace GraficWindowsFormsApp48
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var form = new SplashForm();
            form.ShowDialog();

            Application.Run(new Form1());
        }
    }
}

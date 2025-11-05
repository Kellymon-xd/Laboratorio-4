using System;
using System.Windows.Forms;

namespace Laboratorio_4
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AppContexto());
        }
    }
}

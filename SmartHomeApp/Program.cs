using static System.Net.Mime.MediaTypeNames;

using System;
using System.Windows.Forms;

namespace SmartHomeApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SmartHomeDashboard());
        }
    }
}
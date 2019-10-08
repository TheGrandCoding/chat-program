using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram
{
    public static class Program
    {
        public static bool IsServer { get; set; } = false;

        public const int Port = 6098;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());
            while(Menu.Client != null || Menu.Server != null)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}

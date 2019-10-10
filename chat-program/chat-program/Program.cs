using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatProgram
{
    public static class Program
    {
        const string REGKEY_FOLDER = "HKEY_CURRENT_USER\\CheAle14";
        const string REGKEY_MAIN = "ChatProgram";

        public static void SetRegistry(string key, string value)
        {
            Microsoft.Win32.Registry.SetValue($"{REGKEY_FOLDER}\\{REGKEY_MAIN}", key, value);
        }
        
        public static string GetRegistry(string key, string defaultValue)
        {
            var item = Microsoft.Win32.Registry.GetValue($"{REGKEY_FOLDER}\\{REGKEY_MAIN}", key, defaultValue);
            if (item == null)
                return defaultValue;
            return (string)item;
        }

        public static bool IsServer { get; set; } = false;

        public const int Port = 6098;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Menu());
            while(Menu.Client != null || Menu.Server != null)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if(args.Name.StartsWith("Newtonsoft.Json"))
            {
                Byte[] rawAssembly;
                try
                {
                    rawAssembly = System.IO.File.ReadAllBytes("Newtonsoft.Json.dll");

                } catch (Exception ex)
                {
                    try
                    {
                        Logger.LogMsg($"Failed to get NewtonsoftDLL, defaulting to stored: {ex}", LogSeverity.Error);
                    }
                    catch { }
                    rawAssembly = Properties.Resources.Newtonsoft_Json;
                }
                return System.Reflection.Assembly.Load(rawAssembly);
            }
            return null;
        }

    }
}

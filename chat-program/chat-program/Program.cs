using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
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

        public static string GetIPAddress()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string DefaultIP = "";

        public const int Port = 8889;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var th = new Thread(getServerDefaltIp);
            th.Start();
            Application.Run(new Menu());
            while(Menu.Client != null || Menu.Server != null)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        public const string APIBASE = "https://ml-api.uk.ms";

        static void getServerDefaltIp()
        {
            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{APIBASE}/chat/ip");
                var response = client.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    string text = response.Content.ReadAsStringAsync().Result;
                    if(IPAddress.TryParse(text, out var addr))
                    {
                        DefaultIP = addr.ToString();
                    }
                } else
                {
                    var str = response.Content.ReadAsStringAsync().Result;
                }
            }
            if (string.IsNullOrWhiteSpace(DefaultIP))
                DefaultIP = "127.0.0.1";
            Menu.INSTANCE.Invoke(new Action(() => { Menu.INSTANCE.ButtonRefresh(); }));
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

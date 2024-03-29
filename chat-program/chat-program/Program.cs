﻿using ChatProgram.Classes;
using Newtonsoft.Json;
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

        public static IPAddress SelfExternalIP { get; set; }

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

        public static string GetExternalIPAddress()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://icanhazip.com");
            using(HttpClient client = new HttpClient())
            {
                var response = client.SendAsync(request).Result;
                if(response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result.Trim();
                }
            }
            return "0.0.0.0";
        }

        public static string DefaultIP = "";

        public const int Port = 8888;

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
            th.SetApartmentState(ApartmentState.STA);
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
			System.Threading.Thread.Sleep(500); // wait for menu to load
            try
            {
                SelfExternalIP = IPAddress.Parse(GetExternalIPAddress());
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "ChromeNotChromeLmao");
                    var request = new HttpRequestMessage(HttpMethod.Get, $"{APIBASE}/masterlist/list?type=chatprogram");
                    var response = client.SendAsync(request).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string text = response.Content.ReadAsStringAsync().Result;
                        var servers = JsonConvert.DeserializeObject<List<MLServer>>(text);
                        foreach(var server in servers)
                        {
                            if (server.ExternalIP != SelfExternalIP.ToString())
                                continue;
                            if (server.InternalIP == GetIPAddress())
                                server.InternalIP = "127.0.0.1";
                            var row = new string[] { server.Name, server.CurrentPlayers.ToString(), $"{server.InternalIP}" };
                            Menu.INSTANCE.Invoke(new Action(() =>
                            {
                                Menu.INSTANCE.dgvServers.Rows.Add(row);
                            }));
                        }
                    } else
                    {
                        var str = response.Content.ReadAsStringAsync().Result;
                        Logger.LogMsg(str, LogSeverity.Error);
                    }
                }
            } catch (Exception ex)
            {
                Logger.LogMsg(ex.ToString(), LogSeverity.Error);
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

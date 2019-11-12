using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Server
{
    public static class MLHandler
    {
        public static int SavedNonce { get; private set; }
        public static Guid SavedGuid { get; private set; }
        public static void LoadPriorSave()
        {
            try
            {
                string CONTENT = "";
                CONTENT = File.ReadAllText("masterlist.info");
                var split = CONTENT.Trim().Split('#');
                SavedGuid = Guid.Parse(split[0]);
                SavedNonce = int.Parse(split[1]);
            }
            catch
            {
            }
        }
        public static void SavePriorInfo()
        {
            File.WriteAllText("masterlist.info", $"{SavedGuid}#{SavedNonce}");
        }

        public static string SetPlayerCount(int amount)
        {
            LoadPriorSave();
            string URI = $"/masterlist/players?id={SavedGuid}&nonce={SavedNonce}&value={amount}";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://ml-api.uk.ms/");
                var response = client.SendAsync(new HttpRequestMessage(HttpMethod.Post, URI)).Result;
                if (response.IsSuccessStatusCode)
                {
                    return $"Set player count";
                }
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public static string SendServerStart(string name)
        {
            LoadPriorSave();
            string URI = "";
            var METHOD = HttpMethod.Put;
            if(SavedNonce == 0 || SavedGuid == null)
            {
                var internalIP = Program.GetIPAddress();
                var externalIP = Program.GetExternalIPAddress();
                URI = $"/masterlist/create?name={Uri.EscapeDataString(name)}&extn={externalIP}&intl={internalIP}&type=chatprogram&pass=false";
            } else
            {
                METHOD = HttpMethod.Post;
                URI = $"/masterlist/continue?id={SavedGuid}&nonce={SavedNonce}";
            }
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://ml-api.uk.ms/");
                var response = client.SendAsync(new HttpRequestMessage(METHOD, URI)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    File.WriteAllText("masterlist.info", content);
                    LoadPriorSave();
                    return $"Masterlist is aware of Server";
                }
                if(response.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect)
                {
                    SavedNonce = 0;
                    SavePriorInfo();
                    return SendServerStart(name);
                }
                return "Error: " + response.Content.ReadAsStringAsync().Result;
            }
        }


    }
}

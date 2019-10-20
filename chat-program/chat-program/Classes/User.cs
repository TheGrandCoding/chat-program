using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Classes
{
    public class User : JsonEntity
    {
        public uint Id { get; set; }
        public string UserName { get; set; }

        public string NickName { get; set; }

        public string DisplayName {  get
            {
                if (string.IsNullOrWhiteSpace(NickName))
                    return UserName;
                else
                    return "~" + NickName;
            } }

        /// <summary>
        /// Server Only
        /// </summary>
        public Dictionary<string, string> SavedValues { get; set; } = new Dictionary<string, string>();

        public override void FromJson(JObject json)
        {
            Id = json["id"].ToObject<uint>();
            UserName = json["name"].ToObject<string>();
            if(json.ContainsKey("dict"))
            {
                SavedValues = json["dict"].ToObject<Dictionary<string, string>>();
            }
            if(json.ContainsKey("nick"))
            {
                NickName = json["nick"].ToObject<string>();
            }
        }

        public override JObject ToJson()
        {
            JObject jobj = new JObject();
            jobj["id"] = Id;
            jobj["name"] = UserName;
            if(SavedValues.Count > 0)
                jobj["dict"] = JObject.FromObject(SavedValues);
            if (!string.IsNullOrWhiteSpace(NickName))
                jobj["nick"] = NickName;
            return jobj;
        }
    }
}

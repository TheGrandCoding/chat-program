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
        public string Name { get; set; }

        /// <summary>
        /// Server Only
        /// </summary>
        public Dictionary<string, string> SavedValues { get; set; } = new Dictionary<string, string>();

        public override void FromJson(JObject json)
        {
            Id = json["id"].ToObject<uint>();
            Name = json["name"].ToObject<string>();
            if(json.ContainsKey("dict"))
            {
                SavedValues = json["dict"].ToObject<Dictionary<string, string>>();
            }
        }

        public override JObject ToJson()
        {
            JObject jobj = new JObject();
            jobj["id"] = Id;
            jobj["name"] = Name;
            if(SavedValues.Count > 0)
                jobj["dict"] = JObject.FromObject(SavedValues);
            return jobj;
        }
    }
}

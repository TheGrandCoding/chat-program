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

        public override void FromJson(JObject json)
        {
            Id = json["id"].ToObject<uint>();
            Name = json["name"].ToObject<string>();
        }

        public override JObject ToJson()
        {
            JObject jobj = new JObject();
            jobj["id"] = Id;
            jobj["name"] = Name;
            return jobj;
        }
    }
}

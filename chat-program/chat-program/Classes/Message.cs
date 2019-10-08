using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatProgram.Classes
{
    public class Message : JsonEntity
    {
        public uint Id { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }

        public override void FromJson(JObject json)
        {
            Content = json["content"].ToObject<string>();
            Id = json["id"].ToObject<uint>();
            Author = Common.GetUser(json["author"].ToObject<uint>());
        }

        public override JObject ToJson()
        {
            var jobj = new JObject();
            jobj["author"] = Author.Id;
            jobj["content"] = Content;
            jobj["id"] = Id.ToString();
            return jobj;
        }
    }
}

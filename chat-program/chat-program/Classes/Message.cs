using System;
using System.Collections.Generic;
using System.Drawing;
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

        public Color Colour { get; set; } = Color.Black;

        public override void FromJson(JObject json)
        {
            Content = json["content"].ToObject<string>();
            Id = json["id"].ToObject<uint>();
            Author = Common.GetUser(json["author"].ToObject<uint>());
            if(json.ContainsKey("color"))
                Colour = Color.FromName(json["color"].ToObject<string>());
        }

        public override JObject ToJson()
        {
            var jobj = new JObject();
            jobj["author"] = Author.Id;
            jobj["content"] = Content;
            jobj["id"] = Id.ToString();
            if(Colour != Color.Black)
                jobj["color"] = Colour.Name;
            return jobj;
        }
    }
}

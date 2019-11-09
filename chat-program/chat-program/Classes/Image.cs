using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ChatProgram.Classes
{
    public class Image : JsonEntity
    {
        public const int SliceLength = 1024;
        public uint Id { get; set; }
        public string Name { get; set; }

        public int MaximumSlices { get; set; }
        public User UploadedBy { get; set; }

        public string Path {  get
            {
                return $"Images/{UploadedBy.Id}/{Id}/{Name}";
            } }

        public Image(string name, User author)
        {
            Id = 0;
            Name = name;
            UploadedBy = author;
        }
        public Image() { }

        public void LoadImageIntoString()
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(Path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    ImageB64String = Convert.ToBase64String(imageBytes);
                }
            }
            Slices = new Dictionary<int, string>();
            string temp = "";
            for(int i = 0; i < ImageB64String.Length; i++)
            {
                temp += ImageB64String[i];

                if(i % SliceLength == 0 && i > 0)
                {
                    Slices[Slices.Count] = temp;
                    temp = "";
                }
            }
            if(string.IsNullOrWhiteSpace(temp) == false)
            {
                Slices[Slices.Count] = temp;
            }
            MaximumSlices = Slices.Count;
        }

        public string ImageB64String { get; set; }

        public MemoryStream GetStream()
        {
            var bytes = Convert.FromBase64String(ImageB64String);
            return new MemoryStream(bytes);
        }

        public Dictionary<int, string> Slices { get; private set; }

        public override void FromJson(JObject json)
        {
            Id = json["id"].ToObject<uint>();
            UploadedBy = Common.GetUser(json["author"].ToObject<uint>());
            Name = json["name"].ToObject<string>();
            if (json.TryGetValue("maximum", out var val))
                MaximumSlices = val.ToObject<int>();
            else
                MaximumSlices = -1;
        }

        public override JObject ToJson()
        {
            var jobj = new JObject();
            jobj["id"] = Id;
            jobj["author"] = UploadedBy.Id;
            jobj["name"] = Name;
            return jobj;
        }
        public JObject ToJson(bool includeMaximum)
        {
            var obj = ToJson();
            obj["maximum"] = MaximumSlices;
            return obj;
        }

        public void SetSlice(int index, string content)
        {
            Slices[index] = content;
            string temp = string.Join("", Slices.Values);
            if (temp.Length > ImageB64String.Length)
                ImageB64String = temp;
        }

    }
}

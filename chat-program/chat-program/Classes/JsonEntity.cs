using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Classes
{
    public abstract class JsonEntity
    {
        public abstract JObject ToJson();
        
        public abstract void FromJson(JObject json);
    }
}

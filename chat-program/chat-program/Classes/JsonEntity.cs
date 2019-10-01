using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Interfaces
{
    public abstract class JsonEntity
    {
        public abstract JObject ToJson();
        
        public abstract JsonEntity FromJson(JObject json);
    }
}

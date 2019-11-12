using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Classes
{
    public class MLServer
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("extIP")]
        public string ExternalIP { get; set; }
        [JsonProperty("intIP")]
        public string InternalIP { get; set; }
        [JsonProperty("type")]
        public string Game { get; set; }
        [JsonProperty("hasPw")]
        public bool Password { get; set; }
        [JsonProperty("count", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(0)]
        public int CurrentPlayers { get; set; }
        [JsonProperty("max", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [DefaultValue(0)]
        public int MaximumPlayers { get; set; }
    }
}

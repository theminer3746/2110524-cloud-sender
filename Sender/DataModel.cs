using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    public class DataModel
    {
        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("likes")]
        public int Likes { get; set; }

        [JsonProperty("imageUpdate")]
        public bool ImageUpdate { get; set; }

        [JsonProperty("base64Image")]
        public string Base64Image { get; set; }
    }
}

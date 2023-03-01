using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    public class RequestModel : DataModel
    {
        [JsonProperty("action")]
        public ActionEnum ActionEnum { get; set; }
    }
}

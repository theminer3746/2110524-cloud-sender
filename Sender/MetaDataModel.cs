using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sender
{
    public class MetaDataModel
    {
        public int totalFiles { get; set; }
        public List<int> orders { get; set; } = new List<int>();
        public List<string> hashes { get; set; } = new List<string>();
    }
}

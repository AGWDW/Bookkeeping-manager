using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.Scripts
{
    public class Document : MongoObject
    {
        public string FileName { get; set; }
        public string FileName_Cloud { get; set; }
        public string FilePath { get; set; }
        public string GetClientName()
        {
            return FileName_Cloud.Substring(0, FileName_Cloud.IndexOf('|'));
        }
    }
}

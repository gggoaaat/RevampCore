using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class SMTPModel
    {
        public string SMTPServerConfigPassword { get; set; }
        public string SMTPServerUseLocalDir { get; set; }
        public string SMTPServerConfigPickupDirectoryLocation { get; set; }
        public string SMTPServerConfigHost { get; set; }
        public string SMTPServerConfigPort { get; set; }
        public string SMTPServerConfigUseDefaultCredentials { get; set; }
        public string SMTPServerConfigEnableSsl { get; set; }
        public string SMTPServerConfigUsername { get; set; }
    }
}

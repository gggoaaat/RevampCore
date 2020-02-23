using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{
    public class RevampCoreSettings : IRevampCoreSettings
    {   
        public string DbConnect { get; set; }
        public string SystemDBName { get; set; }
        public string Platform { get; set; }     
        public bool EnableEventLogging { get; set; }
    }
}

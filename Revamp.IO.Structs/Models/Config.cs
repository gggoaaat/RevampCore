using Revamp.IO.Structs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class DBAuthStruct : IDBAuthStruct
    {
        public string connServer { get; set; }
        public string User_ID { get; set; }
        public string connOwnerPassword { get; set; }
        public string Database { get; set; }
        public bool IntegratedSecurity { get; set; }
    }

    [Serializable]
    public class AppSettingsKeyPair : IAppSettingsKeyPair
    {
        public string _value { get; set; }
        public string key { get; set; }
    }

    [Serializable]
    public class WebConfigEdit : IWebConfigEdit
    {
        public string ConnectionString { get; set; }
        public string SectionName { get; set; }
    }

    [Serializable]
    public class DBStruct
    {
        public string DBServer { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

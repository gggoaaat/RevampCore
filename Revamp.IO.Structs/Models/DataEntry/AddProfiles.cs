using System;
using System.Collections.Generic;

namespace Revamp.IO.Structs.Models.DataEntry
{
    public partial class Values
    {
        [Serializable]
        public class AddProfiles
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_PROFILES";
            public long? I_BASE_PROFILES_ID { get; set; } = 0;
            public long? I_PREV_PROFILES_ID { get; set; } = 0;
            public Guid? I_BASE_PROFILES_UUID { get; set; }
            public Guid? I_PREV_PROFILES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public long? I_IDENTITIES__ID { get; set; } = 1000;
            public string I_FIRST_NAME { get; set; }
            public string I_MIDDLE_NAME { get; set; }
            public string I_LAST_NAME { get; set; }
            public string I_OCCUPATION { get; set; }
            public string I_STATE { get; set; }
            public string I_ZIPCODE { get; set; }
            public string I_PHONE { get; set; }
            public string I_COUNTRY { get; set; }
            public string I_CITY { get; set; }
            public string I_ABOUT { get; set; }
            public string I_THIS_CALLER { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_PROFILES_ID { get; set; }
            public Guid? O_PROFILES_UUID { get; set; }
        }
    }
}

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {
        
    }
}
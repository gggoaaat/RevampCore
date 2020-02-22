using System;
using System.Collections.Generic;

namespace Revamp.IO.Structs.Models.DataEntry
{
    public partial class Values
    {
        [Serializable]
        public class AddProfilesSecPriv
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "PROFILES_SEC_PRIV";
            public string V_ATTEMPTED_SQL { get; set; }
            public string I_THIS_CALLER { get; set; } = "";


            public long? I_BASE_PROFILES_SEC_PRIV_ID { get; set; } = 0;
            public long? I_PREV_PROFILES_SEC_PRIV_ID { get; set; } = 0;
            public Guid? I_BASE_PROFILES_SEC_PRIV_UUID { get; set; }
            public Guid? I_PREV_PROFILES_SEC_PRIV_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Permission";
            public long? I_PROFILES_ID { get; set; }
            public long? I_PRIVILEGES_ID { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_PROFILES_SEC_PRIV_ID { get; set; }
            public Guid? O_PROFILES_SEC_PRIV_UUID { get; set; }
        }
    }
}

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {
        
    }
}
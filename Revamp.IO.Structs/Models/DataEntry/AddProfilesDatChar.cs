using System;
using System.Collections.Generic;

namespace Revamp.IO.Structs.Models.DataEntry
{
    public partial class Values
    {
        [Serializable]
        public class AddProfilesDatChar
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_PROFILES_DAT_CHAR";
            public long? I_BASE_PROFILES_DAT_CHAR_ID { get; set; } = 0;
            public long? I_PREV_PROFILES_DAT_CHAR_ID { get; set; } = 0;
            public Guid? I_BASE_PROFILES_DAT_CHAR_UUID { get; set; }
            public Guid? I_PREV_PROFILES_DAT_CHAR_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public long? I_RENDITION { get; set; }
            public long? I_CORES_ID { get; set; } = 1000;
            public long? I_APPLICATIONS_ID { get; set; } = 1000;
            public long? I_STAGES_ID { get; set; }
            public long? I_GRIPS_ID { get; set; }
            public long? I_OBJECT_SETS_ID { get; set; }
            public long? I_OBJ_PROP_SETS_ID { get; set; }
            public long? I_PROFILES_ID { get; set; }
            public string I_PROPERTY_VALUE { get; set; }
            public long? I_OBJ_PROP_OPT_SETS_ID { get; set; }
            public string I_VALUE { get; set; }
            public string I_THIS_CALLER { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_PROFILES_DAT_CHAR_ID { get; set; }
            public Guid? O_PROFILES_DAT_CHAR_UUID { get; set; }
        }
    }
}

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {
        
    }
}
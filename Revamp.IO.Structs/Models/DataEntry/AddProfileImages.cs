using System;
using System.Collections.Generic;

namespace Revamp.IO.Structs.Models.DataEntry
{
    public partial class Values
    {
        [Serializable]
        public class AddProfileImages
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_PROFILE_IMAGES";
            public long? I_BASE_PROFILE_IMAGES_ID { get; set; } = 0;
            public long? I_PREV_PROFILE_IMAGES_ID { get; set; } = 0;
            public Guid? I_BASE_PROFILE_IMAGES_UUID { get; set; }
            public Guid? I_PREV_PROFILE_IMAGES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public Guid? I_PROFILES_UUID { get; set; }
            public string I_FILE_NAME { get; set; }
            public long? I_FILE_SIZE { get; set; }
            public string I_CONTENT_TYPE { get; set; }
            public byte[] I_VALUE { get; set; }
            public string I_THIS_CALLER { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_PROFILE_IMAGES_ID { get; set; }
            public Guid? O_PROFILE_IMAGES_UUID { get; set; }
        }
    }
}

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {
        
    }
}
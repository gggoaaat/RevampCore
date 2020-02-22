using System;
using System.Collections.Generic;

namespace Revamp.IO.Structs.Models.DataEntry
{
    public partial class Values
    {
        [Serializable]
        public class UpdateProfileImages
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_U_PROFILE_IMAGES";
            public long? I_PROFILE_IMAGES_ID { get; set; }
            public long? I_PROFILES_ID { get; set; }
            public string I_FILE_NAME { get; set; }
            public long? I_FILE_SIZE { get; set; }
            public string I_CONTENT_TYPE { get; set; }
            public byte[] I_VALUE { get; set; }
            public long? O_PROFILE_IMAGES_ID { get; set; }
        }
    }
}

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models.RevampSystem
{
    public class RevampNucleus
    {
        public class AddOwnerSeed
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "ER_OWNER_SEEDS";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public long? I_BASE_ER_OWNER_SEEDS_ID { get; set; } = 0;
            public long? I_PREV_ER_OWNER_SEEDS_ID { get; set; } = 0;
            public Guid? I_BASE_ER_OWNER_SEEDS_UUID { get; set; }
            public Guid? I_PREV_ER_OWNER_SEEDS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_SYSTEM_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_OWNER_SEEDS_ID { get; set; }
            public Guid? O_ER_OWNER_SEEDS_UUID { get; set; }
        }
    }
}

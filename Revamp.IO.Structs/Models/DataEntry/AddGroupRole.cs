using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models.DataEntry
{
    public partial class Values
    {
        [Serializable]
        public class AddGroupRole
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string I_THIS_CALLER { get; set; } = "";

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_GROUPS_ROLES";
            public long? I_BASE_GROUPS_ROLES_ID { get; set; } = 0;
            public long? I_PREV_GROUPS_ROLES_ID { get; set; } = 0;
            public Guid? I_BASE_GROUPS_ROLES_UUID { get; set; }
            public Guid? I_PREV_GROUPS_ROLES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Grant";
            public Guid? I_ROLES_UUID { get; set; }
            public Guid? I_CORES_UUID { get; set; }
            public Guid? I_GROUPS_UUID { get; set; }
            public string I_GROUP_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_GROUPS_ROLES_ID { get; set; }
            public Guid? O_GROUPS_ROLES_UUID { get; set; }
        }
    }
}

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {

    }
}
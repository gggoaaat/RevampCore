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
        public class AddSubmissionAudit
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_SUBMISSIONS_AUDIT";
            public long? I_BASE_SUBMISSIONS_AUDIT_ID { get; set; } = 0;
            public long? I_PREV_SUBMISSIONS_AUDIT_ID { get; set; } = 0;
            public Guid? I_BASE_SUBMISSIONS_AUDIT_UUID { get; set; }
            public Guid? I_PREV_SUBMISSIONS_AUDIT_UUID { get; set; }
            public Guid? I_IDENTITIES_UUID { get; set; }
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public Guid? I_CORES_UUID { get; set; }
            public Guid? I_APPLICATIONS_UUID { get; set; }
            public Guid? I_BASE_APPLICATIONS_UUID { get; set; }
            public Guid? I_SOURCE_UUID { get; set; }
            public Guid? I_BASE_SOURCE_UUID { get; set; }
            public Guid? I_PREV_SOURCE_UUID { get; set; }
            public byte[] I_CONTENT { get; set; }
            public string I_THIS_CALLER { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_SUBMISSIONS_AUDIT_ID { get; set; }
            public Guid? O_SUBMISSIONS_AUDIT_UUID { get; set; }
        }
    }
}

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {
        [Serializable]
        public class AddSubmissionAudit
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_SUBMISSIONS_AUDIT";
            public object I_BASE_SUBMISSIONS_AUDIT_ID { get; set; } = 0;
            public object I_PREV_SUBMISSIONS_AUDIT_ID { get; set; } = 0;
            public object I_BASE_SUBMISSIONS_AUDIT_UUID { get; set; }
            public object I_PREV_SUBMISSIONS_AUDIT_UUID { get; set; }
            public object I_IDENTITIES_UUID { get; set; }
            public object I_ENABLED { get; set; } = 'Y';
            public object I_DT_AVAILABLE { get; set; }
            public object I_DT_END { get; set; }
            public object I_CORES_UUID { get; set; }
            public object I_APPLICATIONS_UUID { get; set; }
            public object I_BASE_APPLICATIONS_UUID { get; set; }
            public object I_SOURCE_UUID { get; set; }
            public object I_BASE_SOURCE_UUID { get; set; }
            public object I_PREV_SOURCE_UUID { get; set; }
            public object I_CONTENT { get; set; }
            public string I_THIS_CALLER { get; set; }
            public object O_ERR_NUMB { get; set; }
            public object O_ERR_MESS { get; set; }
            public object O_SUBMISSIONS_AUDIT_ID { get; set; }
            public object O_SUBMISSIONS_AUDIT_UUID { get; set; }
        }
    }
}
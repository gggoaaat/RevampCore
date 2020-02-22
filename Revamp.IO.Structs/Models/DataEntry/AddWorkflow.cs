using System;

namespace Revamp.IO.Structs.Models.DataEntry
{
    public partial class Values
    {
        [Serializable]
        public class AddWorkflow
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_WORKFLOWS";
            public long? I_BASE_WORKFLOWS_ID { get; set; } = 0;
            public long? I_PREV_WORKFLOWS_ID { get; set; } = 0;
            public Guid? I_BASE_WORKFLOWS_UUID { get; set; }
            public Guid? I_PREV_WORKFLOWS_UUID { get; set; }
            public Guid? I_IDENTITIES_UUID { get; set; }
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public Guid? I_CORES_UUID { get; set; }
            public Guid? I_BASE_APPLICATIONS_UUID { get; set; }
            public Guid? I_APPLICATIONS_UUID { get; set; }
            public byte[] I_WORKFLOW { get; set; }
            public string I_THIS_CALLER { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_WORKFLOWS_ID { get; set; }
            public Guid? O_WORKFLOWS_UUID { get; set; }
        }
    }
}

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {
        [Serializable]
        public class AddWorkflow
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_WORKFLOWS";
            public object I_BASE_APPLICATIONS_UUID { get; set; }
            public object I_BASE_WORKFLOWS_ID { get; set; }
            public object I_PREV_WORKFLOWS_ID { get; set; }
            public object I_BASE_WORKFLOWS_UUID { get; set; }
            public object I_PREV_WORKFLOWS_UUID { get; set; }
            public object I_IDENTITIES_UUID { get; set; }
            public object I_ENABLED { get; set; }
            public object I_DT_AVAILABLE { get; set; }
            public object I_DT_END { get; set; }
            public object I_CORES_UUID { get; set; }
            public object I_APPLICATIONS_UUID { get; set; }
            public object I_WORKFLOW { get; set; }
            public object I_THIS_CALLER { get; set; }
            public object O_ERR_NUMB { get; set; }
            public object O_ERR_MESS { get; set; }
            public object O_WORKFLOWS_ID { get; set; }
            public object O_WORKFLOWS_UUID { get; set; }
        }
    }
}
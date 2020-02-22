using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{
    class FormDataModels
    {
    }

    [Serializable]
    public class ReceiveData
    {
        public bool success { get; set; }
        public Guid? cores_uuid { get; set; }
        public Guid? applications_uuid { get; set; }
        public Guid? base_applications_uuid { get; set; }
        public whoCalled who { get; set; }
        public List<ReceiveStageData> formStages { get; set; }

        public List<VirtualProcedureCall> VPC { get; set; } = new List<VirtualProcedureCall>();

        public List<VirtualProcedureCall> VPCTrans { get; set; } = new List<VirtualProcedureCall>();

        public List<Dictionary<string, object>> TransactionResults { get; set; }

        public Dictionary<string, string> navigate { get; set; } = new Dictionary<string, string>();
    }

    [Serializable]
    public class whoCalled
    {
        public string callerID { get; set; }
        public Guid? callerUUID { get; set; }
        public Guid? callerStage { get; set; }
        public string stageName { get; set; }
    }

    [Serializable]
    public class ReceiveStageData
    {
        public Guid? stages_uuid { get; set; }

        public List<DataObject> data_objects { get; set; }

        public VirtualProcedureCall VPC { get; set; } = new VirtualProcedureCall();

    }

    [Serializable]
    public class DataObject
    {
        public string form_type { get; set; } = "new";
        public string name { get; set; }
        public string i_caller_id { get; set; }
        public string value { get; set; }
        public Guid? uuid { get; set; }

        public Guid? c_uuid { get; set; }
        public Guid? a_uuid { get; set; }
        public Guid? s_uuid { get; set; }

        public string destination { get; set; }

        public VirtualProcedureCall VPC { get; set; } = new VirtualProcedureCall();

        public ioDataType ioType { get; set; } = ioDataType.OneToOne;

        public DataObject Copy()
        {
            return (DataObject)Clone();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }


    public enum ioDataType
    {
        OneToOne, OneToMany
    }
}

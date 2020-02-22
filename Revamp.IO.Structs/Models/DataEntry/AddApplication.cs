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
        public class AddApplication
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "APPLICATIONS";
            public string V_ATTEMPTED_SQL { get; set; }
            public string I_THIS_CALLER { get; set; } = "";



            public long? I_BASE_APPLICATIONS_ID { get; set; } = 0;
            public long? I_PREV_APPLICATIONS_ID { get; set; } = 0;
            public Guid? I_BASE_APPLICATIONS_UUID { get; set; }
            public Guid? I_PREV_APPLICATIONS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Application";
            public long? I_CORES_ID { get; set; }
            public Guid? I_CORES_UUID { get; set; }
            public string I_APPLICATION_NAME { get; set; }
            public long? I_RENDITION { get; set; }
            public string I_ROOT_APPLICATION { get; set; }
            public string I_APPLICATION_LINK { get; set; }
            public string I_APPLICATION_DESCRIPTION { get; set; }
            //public string I_BACKGROUND_COLOR { get; set; }
            public Guid? I_CONTAINERS_UUID { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_APPLICATIONS_ID { get; set; }
            public Guid? O_APPLICATIONS_UUID { get; set; }
        }
    }
}

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {
        [Serializable]
        public class Application
        {
            public List<Stage> Stages { get; set; }
            /*, string cores_id, string Identities_ID, string stages_id, string grips_id, string ObjectSetId, string ObjectPropertySetID, string ApplicationName, string _rendition, string _parentname, string _pretty_link, string _desc*/

            public string V_ATTEMPTED_SQL { get; set; }
            public string I_THIS_CALLER { get; set; } = "";

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_APPLICATIONS";
            public object I_BASE_APPLICATIONS_ID { get; set; } = 0;
            public object I_PREV_APPLICATIONS_ID { get; set; } = 0;
            public object I_BASE_APPLICATIONS_UUID { get; set; }
            public object I_PREV_APPLICATIONS_UUID { get; set; }
            public object I_IDENTITIES_ID { get; set; } = 1000;
            public object I_ENABLED { get; set; } = 'Y';
            public object I_DT_AVAILABLE { get; set; }
            public object I_DT_END { get; set; }
            public object I_OBJECT_TYPE { get; set; } = "Application";
            public object I_CORES_ID { get; set; }
            public object I_CORES_UUID { get; set; }
            public object I_APPLICATION_NAME { get; set; }
            public object I_RENDITION { get; set; }
            public object I_ROOT_APPLICATION { get; set; }
            public object I_APPLICATION_LINK { get; set; }
            //public object I_BACKGROUND_COLOR { get; set; }
            public object I_APPLICATION_DESCRIPTION { get; set; }
            public object I_CONTAINERS_UUID { get; set; }
            public object O_ERR_NUMB { get; set; }
            public object O_ERR_MESS { get; set; }
            public object O_APPLICATIONS_ID { get; set; }
            public object O_APPLICATIONS_UUID { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{
    public partial class CASTGOOP
    {
        [Serializable]
        public class Core
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_CORES";

            public long? I_BASE_CORES_ID { get; set; } = 0;
            public long? I_PREV_CORES_ID { get; set; } = 0;
            public Guid? I_BASE_CORES_UUID { get; set; }
            public Guid? I_PREV_CORES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Core";
            public string I_CORE_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_CORES_ID { get; set; }
            public Guid? O_CORES_UUID { get; set; }

        }





        

        [Serializable]
        public class ObjectOptionData
        {
            public long? _IdentityId { get; set; }
            public long? _Rendition { get; set; }
            public long? _ObjectPropSetID { get; set; }
            public string _Destination { get; set; }
            public long? _Destination_ID { get; set; }
            public long? _PropOptionID { get; set; }
        }



        

        [Serializable]
        public class Activity
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_ACTIVITY";
            public object I_BASE_ACTIVITY_ID { get; set; } = 0;
            public object I_PREV_ACTIVITY_ID { get; set; } = 0;
            public object I_BASE_ACTIVITY_UUID { get; set; }
            public object I_PREV_ACTIVITY_UUID { get; set; }
            public object I_IDENTITIES_ID { get; set; } = 1000;
            public object I_ENABLED { get; set; } = 'Y';
            public object I_DT_AVAILABLE { get; set; }
            public object I_DT_END { get; set; }
            public object I_OBJECT_TYPE { get; set; }
            public object I_CORES_ID { get; set; }
            public object I_APPLICATIONS_ID { get; set; }
            public object I_TABLE_SOURCE { get; set; }
            public object I_TABLE_ID { get; set; }
            public object I_VARIANTS_ID { get; set; }
            public object I_SYMBOLS_ID { get; set; }
            public object I_DESC_TEXT { get; set; }
            public object I_DESC_VARIANTS_ID { get; set; }
            public object I_DESC_SYMBOLS_ID { get; set; }
            public object I_DESC_META_TEXT { get; set; }
            public object O_ERR_NUMB { get; set; }
            public object O_ERR_MESS { get; set; }
            public object O_ACTIVITY_ID { get; set; }
            public object O_ACTIVITY_UUID { get; set; }
        }

        [Serializable]
        public class ObjectDML
        {
            public string _Action { get; set; }
            public string _Source { get; set; }
            public string _SourceField { get; set; }
            public long? _SourceID { get; set; }
            public Object_Value _Value { get; set; }
        }

        [Serializable]
        public class Enter
        {
            public string EntrySource { get; set; }
            public List<DBParameters> EntryProcedureParameters { get; set; }
        }

        [Serializable]
        public class EnterFile
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_FILES";
            public object I_BASE_FILES_ID { get; set; }
            public object I_PREV_FILES_ID { get; set; }
            public object I_BASE_FILES_UUID { get; set; }
            public object I_PREV_FILES_UUID { get; set; }
            public object I_IDENTITIES_ID { get; set; }
            public object I_ENABLED { get; set; }
            public object I_DT_AVAILABLE { get; set; }
            public object I_DT_END { get; set; }
            public object I_OBJECT_TYPE { get; set; } = "File";
            public object I_FILE_NAME { get; set; }
            public object I_FILE_SIZE { get; set; }
            public object I_CONTENT_TYPE { get; set; }
            public object I_FILE_DATA { get; set; }
            public object O_ERR_NUMB { get; set; }
            public object O_ERR_MESS { get; set; }
            public object O_FILES_ID { get; set; }
            public object O_FILES_UUID { get; set; }

        }

        [Serializable]
        public class EnterFilePoint
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_OBJ_PROP_FILE";
            public object I_BASE_OBJ_PROP_FILE_ID { get; set; } = 0;
            public object I_PREV_OBJ_PROP_FILE_ID { get; set; } = 0;
            public object I_BASE_OBJ_PROP_FILE_UUID { get; set; }
            public object I_PREV_OBJ_PROP_FILE_UUID { get; set; }
            public object I_IDENTITIES_ID { get; set; } = 1000;
            public object I_ENABLED { get; set; } = 'Y';
            public object I_DT_AVAILABLE { get; set; }
            public object I_DT_END { get; set; }
            public object I_OBJ_PROP_SETS_ID { get; set; }
            public object I_FILES_ID { get; set; }
            public object O_ERR_NUMB { get; set; }
            public object O_ERR_MESS { get; set; }
            public object O_OBJ_PROP_FILE_ID { get; set; }
            public object O_OBJ_PROP_FILE_UUID { get; set; }
        }

        [Serializable]
        public class EnterRoleToObject
        {
            public long? Roles_id { get; set; }
            public object object_sets_id { get; set; }
        }

        [Serializable]
        public class EnterSecurityRole
        {
            public string sourcetable { get; set; }
            public string _id { get; set; }
            public long? _roles_id { get; set; }
        }

        [Serializable]
        public class EnterSecurityPermission
        {
            public string sourcetable { get; set; }
            public string _id { get; set; }
            public long? roles_id { get; set; }
            public long? identities_id { get; set; }
            public long? _rendition { get; set; }
        }

        [Serializable]
        public class AddStageContainer
        {
            //public long? I_CONTAINERS_ID { get; set; }
            //public string I_STAGES_ID { get; set; }

            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_STAGES_CONTAINERS";
            public object I_BASE_STAGES_CONTAINERS_ID { get; set; } = 0;
            public object I_PREV_STAGES_CONTAINERS_ID { get; set; } = 0;
            public object I_BASE_STAGES_CONTAINERS_UUID { get; set; }
            public object I_PREV_STAGES_CONTAINERS_UUID { get; set; }
            public object I_IDENTITIES_ID { get; set; } = 1000;
            public object I_ENABLED { get; set; } = 'Y';
            public object I_DT_AVAILABLE { get; set; }
            public object I_DT_END { get; set; }
            public object I_OBJECT_TYPE { get; set; }
            public object I_CONTAINERS_ID { get; set; }
            public object I_STAGES_ID { get; set; }
            public object O_ERR_NUMB { get; set; }
            public object O_ERR_MESS { get; set; }
            public object O_STAGES_CONTAINERS_ID { get; set; }
            public object O_STAGES_CONTAINERS_UUID { get; set; }
        }

        [Serializable]
        public class EnterStageRelationship
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_STAGES_REL";
            public object I_BASE_STAGES_REL_ID { get; set; } = 0;
            public object I_PREV_STAGES_REL_ID { get; set; } = 0;
            public object I_BASE_STAGES_REL_UUID { get; set; }
            public object I_PREV_STAGES_REL_UUID { get; set; }
            public object I_IDENTITIES_ID { get; set; } = 1000;
            public object I_ENABLED { get; set; } = 'Y';
            public object I_DT_AVAILABLE { get; set; }
            public object I_DT_END { get; set; }
            public object I_OBJECT_TYPE { get; set; }
            public object I_RELATED_STAGES_ID { get; set; }
            public object I_STAGES_ID { get; set; }
            public object O_ERR_NUMB { get; set; }
            public object O_ERR_MESS { get; set; }
            public object O_STAGES_REL_ID { get; set; }
            public object O_STAGES_REL_UUID { get; set; }
        }

        [Serializable]
        public class EnterObjectData
        {
            public string I_THIS_CALLER { get; set; } = "";
            public object _IdentityId { get; set; }
            public object _CoreId { get; set; }
            public object _StageId { get; set; }
            public object _GripId { get; set; }
            public object _ObjectSetId { get; set; }
            public object _ObjectsPropertySetsID { get; set; }
            public object _ObjectsPropertyOptionSetsID { get; set; }
            public string _Destination { get; set; }
            public object _Destination_ID { get; set; }
            public long? _Rendition { get; set; }
            public string _ValueDataType { get; set; }
            public Object_Value _Value { get; set; }
            public bool checkAllowAccess { get; set; }
            public bool V_AVOID_ANTIXSS { get; set; } = false;
        }

        [Serializable]
        public class EnterPrivilegeToObject
        {
            public string SourceName { get; set; }
            public object _SourceID { get; set; }
            public string _Privilege { get; set; }
            public object _IdentityID { get; set; }
        }

        [Serializable]
        public class StageBuilderGrid
        {
            public object i_this_caller { get; set; }
            public StageBuilderStagesModel stageBuilderModel { get; set; }
            public string Stage_Type { get; set; }
            public object Stages_ID { get; set; }
            public object Stages_UUID { get; set; }
            public SessionObjects SO { get; set; }
        }

        [Serializable]
        public class EnterIdentityProperty
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_IDENTITY_PROPERTIES";
            public long? I_BASE_IDENTITY_PROPERTIES_ID { get; set; } = 0;
            public long? I_PREV_IDENTITY_PROPERTIES_ID { get; set; } = 0;
            public Guid? I_BASE_IDENTITY_PROPERTIES_UUID { get; set; }
            public Guid? I_PREV_IDENTITY_PROPERTIES_UUID { get; set; }
            public object I_IDENTITIES_ID { get; set; } = 1000;
            public object I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_PROPERTY_TYPE { get; set; }
            public string I_PROPERTY_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_IDENTITY_PROPERTIES_ID { get; set; }
            public Guid? O_IDENTITY_PROPERTIES_UUID { get; set; }
        }






    }
}

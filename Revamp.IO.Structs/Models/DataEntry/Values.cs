using System;

namespace Revamp.IO.Structs.Models.DataEntry
{
    [Serializable]
    public partial class Values
    {
        [Serializable]
        public class AddDataType
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "VALUE_DATATYPES";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_VALUE_DATATYPES_ID { get; set; } = 0;
            public long? I_PREV_VALUE_DATATYPES_ID { get; set; } = 0;
            public Guid? I_BASE_VALUE_DATATYPES_UUID { get; set; }
            public Guid? I_PREV_VALUE_DATATYPES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_VALUE_DATATYPE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_VALUE_DATATYPES_ID { get; set; }
            public Guid? O_VALUE_DATATYPES_UUID { get; set; }

        }

        [Serializable]
        public class AddObject
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "OBJECTS";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_OBJECTS_ID { get; set; } = 0;
            public long? I_PREV_OBJECTS_ID { get; set; } = 0;
            public Guid? I_BASE_OBJECTS_UUID { get; set; }
            public Guid? I_PREV_OBJECTS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; }
            public string I_OBJECT_LAYER { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_OBJECTS_ID { get; set; }
            public Guid? O_OBJECTS_UUID { get; set; }
        }

        [Serializable]
        public class AddVariant
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "VARIANTS";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_VARIANTS_ID { get; set; } = 0;
            public long? I_PREV_VARIANTS_ID { get; set; } = 0;
            public Guid? I_BASE_VARIANTS_UUID { get; set; }
            public Guid? I_PREV_VARIANTS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Variant";
            public string I_VARIANT_NAME { get; set; }
            public string I_COLOR { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_VARIANTS_ID { get; set; }
            public Guid? O_VARIANTS_UUID { get; set; }

        }

        [Serializable]
        public class AddSymbols
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "SYMBOLS";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_SYMBOLS_ID { get; set; } = 0;
            public long? I_PREV_SYMBOLS_ID { get; set; } = 0;
            public Guid? I_BASE_SYMBOLS_UUID { get; set; }
            public Guid? I_PREV_SYMBOLS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Symbol";
            public string I_SYMBOL_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_SYMBOLS_ID { get; set; }
            public Guid? O_SYMBOLS_UUID { get; set; }
        }

        [Serializable]
        public class AddContentType
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "CONTENT_TYPES";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_CONTENT_TYPES_ID { get; set; } = 0;
            public long? I_PREV_CONTENT_TYPES_ID { get; set; } = 0;
            public Guid? I_BASE_CONTENT_TYPES_UUID { get; set; }
            public Guid? I_PREV_CONTENT_TYPES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Content Type";
            public string I_CONTENT_TYPE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_CONTENT_TYPES_ID { get; set; }
            public Guid? O_CONTENT_TYPES_UUID { get; set; }
        }

        [Serializable]
        public class AddIdentity
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "IDENTITIES";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_IDENTITIES_ID { get; set; } = 0;
            public long? I_PREV_IDENTITIES_ID { get; set; } = 0;
            public Guid? I_BASE_IDENTITIES_UUID { get; set; }
            public Guid? I_PREV_IDENTITIES_UUID { get; set; }
            public long? I_IDENTITIES__ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Identity";
            public string I_USER_NAME { get; set; }
            public string I_EDIPI { get; set; }
            public string I_EMAIL { get; set; }
            public string I_ACTIVE { get; set; } = "Y";
            public string I_VERIFIED { get; set; } = "Y";
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_IDENTITIES_ID { get; set; }
            public Guid? O_IDENTITIES_UUID { get; set; }
        }

        [Serializable]
        public class UpdateIdentity
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_U_" + "IDENTITIES";
            public string V_ATTEMPTED_SQL { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Identity";
            public string I_USER_NAME { get; set; }
            public string I_EDIPI { get; set; }
            public string I_EMAIL { get; set; }
            public string I_ACTIVE { get; set; } = "Y";
            public string I_VERIFIED { get; set; } = "Y";
            public long? O_IDENTITIES_ID { get; set; }
        }               



        [Serializable]
        public class AddPrivilege
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "PRIVILEGES";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_PRIVILEGES_ID { get; set; } = 0;
            public long? I_PREV_PRIVILEGES_ID { get; set; } = 0;
            public Guid? I_BASE_PRIVILEGES_UUID { get; set; }
            public Guid? I_PREV_PRIVILEGES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Privilege";
            public string I_PRIVILEGE_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_PRIVILEGES_ID { get; set; }
            public Guid? O_PRIVILEGES_UUID { get; set; }
        }

       

        [Serializable]
        public class AddObjectLayer
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "OBJECT_LAYERS";

            public long? I_BASE_OBJECT_LAYERS_ID { get; set; } = 0;
            public long? I_PREV_OBJECT_LAYERS_ID { get; set; } = 0;
            public Guid? I_BASE_OBJECT_LAYERS_UUID { get; set; }
            public Guid? I_PREV_OBJECT_LAYERS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_LAYER { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_OBJECT_LAYERS_ID { get; set; }
            public Guid? O_OBJECT_LAYERS_UUID { get; set; }


        }

        [Serializable]
        public class AddPatch
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_ER_PATCHES";
            public long? I_BASE_ER_PATCHES_ID { get; set; } = 0;
            public long? I_PREV_ER_PATCHES_ID { get; set; } = 0;
            public Guid? I_BASE_ER_PATCHES_UUID { get; set; }
            public Guid? I_PREV_ER_PATCHES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_VERSION { get; set; }
            public string I_PUBLISHER { get; set; }
            public long? I_PATCH_LEVEL { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ER_PATCHES_ID { get; set; }
            public Guid? O_ER_PATCHES_UUID { get; set; }
        }



        [Serializable]
        public class AddForms
        {
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "FORMS";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 


            public long? I_BASE_FORMS_ID { get; set; } = 0;
            public long? I_PREV_FORMS_ID { get; set; } = 0;
            public Guid? I_BASE_FORMS_UUID { get; set; }
            public Guid? I_PREV_FORMS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Form";
            public long? I_CONTAINERS_ID { get; set; }
            //public long? I_IDENTITIES_ID { get; set; }
            public long? I_APPLICATIONS_ID { get; set; }
            public long? I_CORES_ID { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_FORMS_ID { get; set; }
            public Guid? O_FORMS_UUID { get; set; }
        }

        [Serializable]
        public class AddFormNotes
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "FORM_NOTES";
            public long? I_BASE_FORM_NOTES_ID { get; set; } = 0;
            public long? I_PREV_FORM_NOTES_ID { get; set; } = 0;
            public Guid? I_BASE_FORM_NOTES_UUID { get; set; }
            public Guid? I_PREV_FORM_NOTES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Notes";
            public long? I_FORMS_ID { get; set; }
            public string I_NOTE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_FORM_NOTES_ID { get; set; }
            public Guid? O_FORM_NOTES_UUID { get; set; }
        }

        [Serializable]
        public class AddVerify
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "VERIFY";
            public long? I_BASE_VERIFY_ID { get; set; } = 0;
            public long? I_PREV_VERIFY_ID { get; set; } = 0;
            public Guid? I_BASE_VERIFY_UUID { get; set; }
            public Guid? I_PREV_VERIFY_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_UUID { get; set; }
            public string I_VERIFIED { get; set; } = "N";
            public string I_VALIDATION_TYPE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_VERIFY_ID { get; set; }
            public Guid? O_VERIFY_UUID { get; set; }
        }

        [Serializable]
        public class UpdateVerify
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_U_" + "VERIFY";
            public long? I_VERIFY_ID { get; set; }
            public string I_UUID { get; set; }
            public string I_VERIFIED { get; set; }
            public string I_VALIDATION_TYPE { get; set; }
            public long? O_VERIFY_ID { get; set; }
        }

        [Serializable]
        public class AddIDPassword
        {
            public string V_PASSWORD;
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_" + "ID_PASSWORD";
            public long? I_BASE_ID_PASSWORD_ID { get; set; } = 0;
            public long? I_PREV_ID_PASSWORD_ID { get; set; } = 0;
            public Guid? I_BASE_ID_PASSWORD_UUID { get; set; }
            public Guid? I_PREV_ID_PASSWORD_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Password";
            //public long? I_IDENTITIES__ID { get; set; }
            public long? I_RENDITION { get; set; }
            public byte[] I_PASSWORD { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ID_PASSWORD_ID { get; set; }
            public Guid? O_ID_PASSWORD_UUID { get; set; }
        }

        [Serializable]
        public class UpdateIDPassword
        {
            public string V_ATTEMPTED_SQL { get; set; }

            public string V_PROCEDURE_NAME { get; set; } = "SP_U_ID_PASSWORD";
            public long? I_ID_PASSWORD_ID { get; set; }
            public string I_OBJECT_TYPE { get; set; }
            public long? I_RENDITION { get; set; }
            public byte[] I_PASSWORD { get; set; }
            public long? O_ID_PASSWORD_ID { get; set; }
        }



        [Serializable]
        public class UpdateProfiles
        {
            public string V_ATTEMPTED_SQL { get; set; }
            public string V_PROCEDURE_NAME { get; set; } = "SP_U_PROFILES";
            public long? I_PROFILES_ID { get; set; }
            public long? I_IDENTITIES__ID { get; set; }
            public string I_FIRST_NAME { get; set; }
            public string I_MIDDLE_NAME { get; set; }
            public string I_LAST_NAME { get; set; }
            public string I_OCCUPATION { get; set; }
            public string I_STATE { get; set; }
            public string I_ZIPCODE { get; set; }
            public string I_PHONE { get; set; }
            public string I_COUNTRY { get; set; }
            public string I_CITY { get; set; }
            public string I_ABOUT { get; set; }
            public long? O_PROFILES_ID { get; set; }
        }

        [Serializable]
        public class AddCryptTicket
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 
            public string V_PROCEDURE_NAME { get; set; } = "SP_I_CRYPT_TICKETS";
            public long? I_BASE_CRYPT_TICKETS_ID { get; set; } = 0;
            public long? I_PREV_CRYPT_TICKETS_ID { get; set; } = 0;
            public Guid? I_BASE_CRYPT_TICKETS_UUID { get; set; }
            public Guid? I_PREV_CRYPT_TICKETS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Cryptology Ticket";
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_CRYPT_TICKETS_ID { get; set; }
            public Guid? O_CRYPT_TICKETS_UUID { get; set; }
        }

        [Serializable]
        public class AddAESPair
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_AES_PAIR";
            public long? I_BASE_AES_PAIR_ID { get; set; } = 0;
            public long? I_PREV_AES_PAIR_ID { get; set; } = 0;
            public Guid? I_BASE_AES_PAIR_UUID { get; set; }
            public Guid? I_PREV_AES_PAIR_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "AES Key";
            public long? I_CRYPT_TICKETS_ID { get; set; }
            public byte[] I_AES_KEY { get; set; }
            public byte[] I_AES_IV { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_AES_PAIR_ID { get; set; }
            public Guid? O_AES_PAIR_UUID { get; set; }
        }

        [Serializable]
        public class AddStageContainer
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_STAGES_CONTAINERS";
            public long? I_BASE_STAGES_CONTAINERS_ID { get; set; } = 0;
            public long? I_PREV_STAGES_CONTAINERS_ID { get; set; } = 0;
            public Guid? I_BASE_STAGES_CONTAINERS_UUID { get; set; }
            public Guid? I_PREV_STAGES_CONTAINERS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Container";
            public long? I_CONTAINERS_ID { get; set; }
            public long? I_STAGES_ID { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_STAGES_CONTAINERS_ID { get; set; }
            public Guid? O_STAGES_CONTAINERS_UUID { get; set; }
        }

        [Serializable]
        public class AddMessage
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_MESSAGES";
            public long? I_BASE_MESSAGES_ID { get; set; } = 0;
            public long? I_PREV_MESSAGES_ID { get; set; } = 0;
            public Guid? I_BASE_MESSAGES_UUID { get; set; }
            public Guid? I_PREV_MESSAGES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_SUBJECT { get; set; }
            public string I_BODY { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_MESSAGES_ID { get; set; }
            public Guid? O_MESSAGES_UUID { get; set; }
        }

        [Serializable]
        public class AddActivity
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_ACTIVITY";
            public long? I_BASE_ACTIVITY_ID { get; set; } = 0;
            public long? I_PREV_ACTIVITY_ID { get; set; } = 0;
            public Guid? I_BASE_ACTIVITY_UUID { get; set; }
            public Guid? I_PREV_ACTIVITY_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Activity";
            public long? I_CORES_ID { get; set; }
            public long? I_APPLICATIONS_ID { get; set; }
            public string I_TABLE_SOURCE { get; set; }
            public long? I_TABLE_ID { get; set; }
            public long? I_VARIANTS_ID { get; set; }
            public long? I_SYMBOLS_ID { get; set; }
            public string I_DESC_TEXT { get; set; }
            public long? I_DESC_VARIANTS_ID { get; set; }
            public long? I_DESC_SYMBOLS_ID { get; set; }
            public string I_DESC_META_TEXT { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ACTIVITY_ID { get; set; }
            public Guid? O_ACTIVITY_UUID { get; set; }
        }
       
        [Serializable]
        public class AddStageRelationship
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_STAGES_REL";
            public long? I_BASE_STAGES_REL_ID { get; set; } = 0;
            public long? I_PREV_STAGES_REL_ID { get; set; } = 0;
            public Guid? I_BASE_STAGES_REL_UUID { get; set; }
            public Guid? I_PREV_STAGES_REL_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; }
            public long? I_RELATED_STAGES_ID { get; set; }
            public long? I_STAGES_ID { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_STAGES_REL_ID { get; set; }
            public Guid? O_STAGES_REL_UUID { get; set; }
        }

        [Serializable]
        public class AddFormRelationship
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_FORMS_REL";
            public long? I_BASE_FORMS_REL_ID { get; set; } = 0;
            public long? I_PREV_FORMS_REL_ID { get; set; } = 0;
            public Guid? I_BASE_FORMS_REL_UUID { get; set; }
            public Guid? I_PREV_FORMS_REL_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "";
            public long? I_RELATED_FORMS_ID { get; set; }
            public long? I_FORMS_ID { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_FORMS_REL_ID { get; set; }
            public Guid? O_FORMS_REL_UUID { get; set; }
        }

        [Serializable]
        public class AddGrid
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_GRIDS";
            public long? I_BASE_GRIDS_ID { get; set; } = 0;
            public long? I_PREV_GRIDS_ID { get; set; } = 0;
            public Guid? I_BASE_GRIDS_UUID { get; set; }
            public Guid? I_PREV_GRIDS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Grid";
            public long? I_CONTAINERS_ID { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_GRIDS_ID { get; set; }
            public Guid? O_GRIDS_UUID { get; set; }
        }

        [Serializable]
        public class AddSession
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_SESSIONS";
            public long? I_BASE_SESSIONS_ID { get; set; } = 0;
            public long? I_PREV_SESSIONS_ID { get; set; } = 0;
            public Guid? I_BASE_SESSIONS_UUID { get; set; }
            public Guid? I_PREV_SESSIONS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_USERNAME { get; set; } = "Anonymous";
            public string I_SESSIONID { get; set; }           
            public string I_TIMEOUT { get; set; }
            public string I_ANONYMOUSID { get; set; }
            public string I_USERAGENT { get; set; }
            public string I_USERHOSTADDRESS { get; set; }
            public string I_ISAUTHENTICATED { get; set; }
            public string I_LOGONUSERIDENTITY { get; set; }
            public string I_BROWSER { get; set; }
            public string I_MAJORVERSION { get; set; }
            public string I_VERSION { get; set; }
            public string I_CRAWLER { get; set; }
            public string I_CLRVERSION { get; set; }
            public string I_COOKIES { get; set; }
            public string I_ISMOBILEDEVICE { get; set; }
            public string I_PLATFORM { get; set; }
            public string I_URL { get; set; }
            public string I_URLREFERRER { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_SESSIONS_ID { get; set; }
            public Guid? O_SESSIONS_UUID { get; set; }
        }

        [Serializable]
        public class AddFile
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_FILES";
            public long? I_BASE_FILES_ID { get; set; } = 0;
            public long? I_PREV_FILES_ID { get; set; } = 0;
            public Guid? I_BASE_FILES_UUID { get; set; }
            public Guid? I_PREV_FILES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "File";
            public string I_FILE_NAME { get; set; }
            public long? I_FILE_SIZE { get; set; }
            public string I_CONTENT_TYPE { get; set; }
            public byte[] I_FILE_DATA { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_FILES_ID { get; set; }
            public Guid? O_FILES_UUID { get; set; }
        }

        [Serializable]
        public class AddFilePoint
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_OBJ_PROP_FILE";
            public long? I_BASE_OBJ_PROP_FILE_ID { get; set; } = 0;
            public long? I_PREV_OBJ_PROP_FILE_ID { get; set; } = 0;
            public Guid? I_BASE_OBJ_PROP_FILE_UUID { get; set; }
            public Guid? I_PREV_OBJ_PROP_FILE_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public long? I_OBJ_PROP_SETS_ID { get; set; }
            public long? I_FILES_ID { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_OBJ_PROP_FILE_ID { get; set; }
            public Guid? O_OBJ_PROP_FILE_UUID { get; set; }
        }

        [Serializable]
        public class AddIdentityProperties
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_IDENTITY_PROPERTIES";
            public long? I_BASE_IDENTITY_PROPERTIES_ID { get; set; } = 0;
            public long? I_PREV_IDENTITY_PROPERTIES_ID { get; set; } = 0;
            public Guid? I_BASE_IDENTITY_PROPERTIES_UUID { get; set; }
            public Guid? I_PREV_IDENTITY_PROPERTIES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_PROPERTY_TYPE { get; set; }
            public string I_PROPERTY_NAME { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_IDENTITY_PROPERTIES_ID { get; set; }
            public Guid? O_IDENTITY_PROPERTIES_UUID { get; set; }
        }              

        [Serializable]
        public class AddToMessage
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_TO_MESSAGES";
            public long? I_BASE_TO_MESSAGES_ID { get; set; } = 0;
            public long? I_PREV_TO_MESSAGES_ID { get; set; } = 0;
            public Guid? I_BASE_TO_MESSAGES_UUID { get; set; }
            public Guid? I_PREV_TO_MESSAGES_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public long? I_MESSAGES_ID { get; set; }
            public string I_MARK_READ { get; set; }
            public string I_MARK_FAVORITE { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_TO_MESSAGES_ID { get; set; }
            public Guid? O_TO_MESSAGES_UUID { get; set; }
        }

        [Serializable]
        public class AddApplicationAccess
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_APP_ACCESS";
            public long? I_BASE_APP_ACCESS_ID { get; set; } = 0;
            public long? I_PREV_APP_ACCESS_ID { get; set; } = 0;
            public Guid? I_BASE_APP_ACCESS_UUID { get; set; }
            public Guid? I_PREV_APP_ACCESS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; }
            public char? I_ENABLED { get; set; }
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Membership";
            public long? I_APPLICATIONS_ID { get; set; }
            public long? I_ROLES_ID { get; set; }
            public long? I_RENDITION { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_APP_ACCESS_ID { get; set; }
            public Guid? O_APP_ACCESS_UUID { get; set; }
        }

        [Serializable]
        public class AddFlatIcon
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "SP_I_FLAT_ICONS";
            public long? I_BASE_FLAT_ICONS_ID { get; set; } = 0;
            public long? I_PREV_FLAT_ICONS_ID { get; set; } = 0;
            public Guid? I_BASE_FLAT_ICONS_UUID { get; set; }
            public Guid? I_PREV_FLAT_ICONS_UUID { get; set; }
            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_ICON_PARENT { get; set; }
            public string I_ICON_NAME { get; set; }
            public string I_FILE_TYPE { get; set; }
            public long? I_FILE_SIZE { get; set; }
            public byte[] I_ICON_BYTES { get; set; }
            public long? O_ERR_NUMB { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_FLAT_ICONS_ID { get; set; }
            public Guid? O_FLAT_ICONS_UUID { get; set; }
        }

        [Serializable]
        public class AddDynamicObjectRole
        {
            public string V_PROCEDURE_NAME { get; set; } = "";
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public long? I_IDENTITIES_ID { get; set; } = 1000;
            public char? I_ENABLED { get; set; } = 'Y';
            public DateTime? I_DT_AVAILABLE { get; set; }
            public DateTime? I_DT_END { get; set; }
            public string I_OBJECT_TYPE { get; set; } = "Permission";
            public string SOURCE { get; set; }
            public long? I_SOURCE_OBJECT_ID { get; set; }
            public long? I_BASE_SOURCE_OBJECT_ID { get; set; } = 0;
            public long? I_PREV_SOURCE_OBJECT_ID { get; set; } = 0;
            public Guid? I_BASE_SOURCE_OBJECT_UUID { get; set; }
            public Guid? I_PREV_SOURCE_OBJECT_UUID { get; set; }
            public long? I_ROLES_ID { get; set; }
            public long? O_SOURCE_SEC_ROLE_ID { get; set; }
            public Guid? O_SOURCE_SEC_ROLE_UUID { get; set; }
            public string O_ERR_MESS { get; set; }
            public long? O_ERR_NUMB { get; set; }
        }

        [Serializable]
        public class AddDynamicPermission
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "";
            public object I_BASE_SOURCE_SEC_PERM_ID { get; set; } = 0;
            public object I_PREV_SOURCE_SEC_PERM_ID { get; set; } = 0;
            public object I_BASE_SOURCE_SEC_PERM_UUID { get; set; } 
            public object I_PREV_SOURCE_SEC_PERM_UUID { get; set; }
            public object I_IDENTITIES_ID { get; set; } = 1000;
            public object I_ENABLED { get; set; } = 'Y';
            public object I_DT_AVAILABLE { get; set; }
            public object I_DT_END { get; set; }
            public object I_OBJECT_TYPE { get; set; }
            public string SOURCE { get; set; }
            public object I_SOURCE_ID { get; set; }
            public object I_ROLES_ID { get; set; }
            public object I_RENDITION { get; set; }
            public object O_ERR_NUMB { get; set; }
            public object O_ERR_MESS { get; set; }
            public object O_SOURCE_SEC_PERM_ID { get; set; }
            public object O_SOURCE_SEC_PERM_UUID { get; set; }
        }

        [Serializable]
        public class ToggleFormObject
        {
            public string V_ATTEMPTED_SQL { get; set; } 
            public string I_THIS_CALLER { get; set; } = ""; 

            public string V_PROCEDURE_NAME { get; set; } = "TOGGLE_FORM_OBJECTS__RW";
            public int I_FORMS_ID { get; set; }
            public int I_STAGES_ID { get; set; }
            public string I_ENABLED { get; set; }
            public int O_ID { get; set; }
        }

    }
}

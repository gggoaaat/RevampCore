using Revamp.IO.Structs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{

    [Serializable]
    public class SqlDbTypeModel
    {
        public int id { get; set; }
        public string dbType { get; set; }
    }

    [Serializable]
    public class sqlCreateObject
    {
        public string objectName { get; set; }
        public string SqlFilePath { get; set; }
        public bool registerObjectToDictionary { get; set; }
        public string DBOBJTypeID { get; set; }
        public string _selectedTemplateID { get; set; }
    }

    [Serializable]
    public class AuditDB
    {

        [Serializable]
        public class SqlDbTypeModel
        {
            public int id { get; set; }
            public string dbType { get; set; }
        }

        public AuditDB()
        {

        }

        /// <summary>
        /// constructor that accepts the most common required values for audit command
        /// </summary>
        /// <param name="sessionModel">ensure ._IdentityModel.Identity_ID and ._ActivityLog.SESSION_ID are present, as these values are use to populate the corresponding fields</param>
        /// <param name="applicationId"></param>
        /// <param name="eventType"></param>
        /// <param name="description"></param>
        public AuditDB(SessionObjects sessionModel, long applicationId, AuditEventType eventType, string description)
        {
            this.IDENTITY_ID = sessionModel._IdentityModel.identities_id;
            this.SESSION_ID = sessionModel._ActivityLog.SESSION_ID;
            this.EVENT_TYPE = eventType;
            this.APPLICATION_ID = applicationId;
            this.DESCRIPTION = description;
        }

        public long? APPLICATION_ID { get; set; }
        public long? IDENTITY_ID { get; set; }
        public long? SESSION_ID { get; set; }
        public byte[] EVENT_BYTES { get; set; }

        public AuditEventType EVENT_TYPE { get; set; }
        public string EVENT_DATA { get; set; }

        public string PROCEDURE_CALLED { get; set; }
        public string PREPARED_SQL { get; set; }
        public string DESCRIPTION { get; set; }
    }

}

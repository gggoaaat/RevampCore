using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class NotificationsModel
    {
        public List<APPLICATIONS_NTFY> Apps { get; set; }
        public List<VW__APPLICATIONS_NTFY> Apps_View { get; set; }
        public List<CORES_NTFY> Cores { get; set; }
        public List<VW__CORES_NTFY> Cores_View { get; set; }
        public List<FORMS_NTFY> Forms { get; set; }
        public List<VW__FORMS_NTFY> Forms_View { get; set; }

    }

    [Serializable]
    public class APPLICATIONS_NTFY
    {
        public long? applications_ntfy_id { get; set; }
        public string enable { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public string object_type { get; set; }
        public long? applications_id { get; set; }
    }
    
    [Serializable]
    public class VW__APPLICATIONS_NTFY
    {
        public long? applications_ntfy_id { get; set; }
        public string enable { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public string object_type { get; set; }
        public long? applications_id { get; set; }
        public long? cores_id { get; set; }
        public string application_name { get; set; }
        
    }

    [Serializable]
    public class CORES_NTFY
    {
        public long? cores_ntfy_id { get; set; }
        public string enable { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public string object_type { get; set; }
        public long? cores_id { get; set; }
    }

    [Serializable]
    public class VW__CORES_NTFY
    {
        public long? cores_ntfy_id { get; set; }
        public string enable { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public string object_type { get; set; }
        public long? cores_id { get; set; }
        public string core_name { get; set; }
    }

    [Serializable]
    public class FORMS_NTFY
    {
        public long? forms_ntfy_id { get; set; }
        public string enable { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public string object_type { get; set; }
        public long? forms_id { get; set; }
    }

    [Serializable]
    public class VW__FORMS_NTFY
    {
        public long? forms_ntfy_id { get; set; }
        public string enable { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public string object_type { get; set; }
        public long? applications_id { get; set; }
        public long? forms_id { get; set; }
        public long? containers_id { get; set; }
        public long? identities_id { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class ProfileModel
    {
        public long? profiles_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
    }

    [Serializable]
    public class ViewProfileModel
    {
        public long? profiles_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public string object_type { get; set; }
        public string user_name { get; set; }

        public byte[] profile_image { get; set; }
    }

}
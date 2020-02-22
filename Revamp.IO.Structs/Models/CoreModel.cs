using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Globalization;
//using System.Web.Security;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class CoreModels
    {

        public List<ApplicationModels> applications { get; set; }

        public CoreStruct Core { get; set; }
        public ViewCoreModel ViewCore { get; set; }
    }

    [Serializable]
    public class CoreStruct
    {
        public CoreTableModel Core { get; set; }
        public CoreCharModel coreCharMeta { get; set; }
        public CoreNumbModel coreNumbMeta { get; set; }
        public CoreDateModel coreDateMeta { get; set; }
        public StageModel corestage { get; set; }
        public GripModel coregrips { get; set; }
        public ObjectSetModels coreObjectSets { get; set; }
        public ViewCoreModel CoreView { get; set; }
    }

    [Serializable]
    public class CreateCoreModel
    {
        [Required]
        public string DB_PLATFORM { get; set; }
        public string connOwner { get; set; }
        public string core_name { get; set; }
    }

    [Serializable]
    public class CoreTableModel
    {
        public long? cores_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public string core_name { get; set; }

    }

    [Serializable]
    public class CoreCharModel
    {
        public long? Cores_dat_char_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? applications_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? identities_id { get; set; }
        public string value { get; set; }
    }

    [Serializable]
    public class CoreNumbModel
    {
        public long? Cores_dat_char_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? applications_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? identities_id { get; set; }
        public long? value { get; set; }
    }

    [Serializable]
    public class CoreDateModel
    {
        public long? Cores_dat_char_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public long? applications_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? identities_id { get; set; }
        public DateTime value { get; set; }

    }

    [Serializable]
    public class ViewCoreModel
    {
        public long? cores_id { get; set; }
        public Guid? cores_uuid { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public string core_name { get; set; }
        public string object_layer { get; set; }
        public long? applications { get; set; }
        public long? members { get; set; }

    }
}
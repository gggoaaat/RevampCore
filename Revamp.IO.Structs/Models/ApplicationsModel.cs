using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Globalization;
//using System.Web.Security;
using System.Data;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class SaveStruct
    {
        public bool successful { get; set; }
        public List<string> messages { get; set; } = new List<string>();
        public string applications_uuid { get; set; }
        public ArrayList trans { get; set; } = new ArrayList();
        public List<CommandResult> StructeGenerator = new List<CommandResult>(); 
    }

    [Serializable]
    public class ApplicationActivity
    {
        public DateTime DT_created { get; set; }
        public long? count { get; set; }

    }

    [Serializable]
    public class ApplicationActivityDates
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int app_id { get; set; }
    }

    [Serializable]
    public class DataTablesDotNetJsonStruct
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<object> data { get; set; }
       
    }

    [Serializable]
    public class ApplicationImage
    {
        public string src { get; set; }
        public long? app_id { get; set; }
       

    }

    [Serializable]
    public class ApplicationModels
    {
        //public int applications_id { get; set; }
        //public DateTime? dt_created { get; set; }
        //public DateTime? dt_available { get; set; }
        //public DateTime? dt_end { get; set; }
        //public string object_type { get; set; }
        //public string object_layer { get; set; }
        //public int cores_id { get; set; }
        //public string application_name { get; set; }
        public int applications_id { get; set; }

        public ApplicationTableModel AppTable { get; set; }
        public ViewApplicationModel AppView { get; set; }

        public List<ApplicationActivity> ActivityCounts { get; set; }

        public virtual List<StageModels> stages { get; set; }
        public NotificationsModel SessionNotification { get; set; }
        
    }

    [Serializable]
    public class ApplicationTableModel
    {
        public int Applications_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public int cores_id { get; set; }
        public string application_name { get; set; }
        public string rendition { get; set; }
        public string root_application { get; set; }
        public string application_link { get; set; }

    }

    [Serializable]
    public class CreateApplicationModel
    {
        [Required]
        public string db_platform { get; set; }
        public string connOwner { get; set; }
        [Required]
        public string _core_id { get; set; }
        public string application_name { get; set; }
        public string rendition { get; set; }
        public string root_application { get; set; }
        public string application_link { get; set; }
    }

    [Serializable]
    public class ViewApplicationModel
    {
        public List<SecurityApplicationRoles> roles = new List<SecurityApplicationRoles>();
        public List<ViewStageModel> stages { get; set; } = new List<ViewStageModel>();
        public long? applications_id { get; set; }
        public long? base_applications_id { get; set; }
        public long? prev_applications_id { get; set; }
        public Guid? applications_uuid { get; set; }
        public Guid? base_applications_uuid { get; set; }
        public Guid? prev_applications_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? cores_id { get; set; }
        public Guid? cores_uuid { get; set; }
        public string application_name { get; set; }
        public string rendition { get; set; }
        public string root_application { get; set; }
        public string application_link { get; set; }
        public string application_description { get; set; }
        public string background_color { get; set; }
        public Guid? containers_uuid { get; set; }
        public long? cores_id_f_cores { get; set; }
        public long? base_cores_id { get; set; }
        public long? prev_cores_id { get; set; }
        public Guid? cores_uuid_f_cores { get; set; }
        public Guid? base_cores_uuid { get; set; }
        public Guid? prev_cores_uuid { get; set; }
        public long? identities_id_f_cores { get; set; }
        public string enabled_f_cores { get; set; }
        public DateTime? dt_created_f_cores { get; set; }
        public DateTime? dt_updated_f_cores { get; set; }
        public DateTime? dt_available_f_cores { get; set; }
        public DateTime? dt_end_f_cores { get; set; }
        public string object_type_f_cores { get; set; }
        public string core_name { get; set; }
        public long? objects_id { get; set; }
        public long? base_objects_id { get; set; }
        public long? prev_objects_id { get; set; }
        public Guid? objects_uuid { get; set; }
        public Guid? base_objects_uuid { get; set; }
        public Guid? prev_objects_uuid { get; set; }
        public long? identities_id_f_objects { get; set; }
        public string enabled_f_objects { get; set; }
        public DateTime? dt_created_f_objects { get; set; }
        public DateTime? dt_updated_f_objects { get; set; }
        public DateTime? dt_available_f_objects { get; set; }
        public DateTime? dt_end_f_objects { get; set; }
        public string object_type_f_objects { get; set; }
        public string object_layer { get; set; }
        public long? containers_id { get; set; }
        public long? base_containers_id { get; set; }
        public long? prev_containers_id { get; set; }
        public Guid? containers_uuid_f_containers { get; set; }
        public Guid? base_containers_uuid { get; set; }
        public Guid? prev_containers_uuid { get; set; }
        public long? identities_id_f_containers { get; set; }
        public string enabled_f_containers { get; set; }
        public DateTime? dt_created_f_containers { get; set; }
        public DateTime? dt_updated_f_containers { get; set; }
        public DateTime? dt_available_f_containers { get; set; }
        public DateTime? dt_end_f_containers { get; set; }
        public string object_type_f_containers { get; set; }
        public string container_name { get; set; }
        public string application_creator { get; set; }
       
    }

    [Serializable]
    public class ApplicationCharModel
    {
        public int Applications_dat_char_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public int applications_id { get; set; }
        public int stages_id { get; set; }
        public int grips_id { get; set; }
        public int obj_prop_sets_id { get; set; }
        public int identities_id { get; set; }
        public string value { get; set; }
    }

    [Serializable]
    public class ApplicationNumbModel
    {
        public int Applications_dat_char_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public int applications_id { get; set; }
        public int stages_id { get; set; }
        public int grips_id { get; set; }
        public int obj_prop_sets_id { get; set; }
        public int identities_id { get; set; }
        public int value { get; set; }
    }

    [Serializable]
    public class ApplicationDateModel
    {
        public int Applications_dat_char_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime dt_available { get; set; }
        public DateTime dt_end { get; set; }
        public int applications_id { get; set; }
        public int stages_id { get; set; }
        public int grips_id { get; set; }
        public int obj_prop_sets_id { get; set; }
        public int identities_id { get; set; }
        public DateTime value { get; set; }
    }

    [Serializable]
    public class ApplicationStruct
    {

        public ApplicationTableModel app { get; set; }
        public ApplicationCharModel appCharMeta { get; set; }
        public ApplicationNumbModel appNumbMeta { get; set; }
        public ApplicationDateModel appDateMeta { get; set; }
        //public StageModel appstage { get; set; }
        //public GripModel appgrips { get; set; }
        public ObjectSetModels appObjectSets { get; set; }

        //public IEnumerable<ApplicationTableModel> app { get; set; }
        //public IEnumerable<ApplicationCharModel> appCharMeta { get; set; }
        //public IEnumerable<ApplicationNumbModel> appNumbMeta { get; set; }
        //public IEnumerable<ApplicationDateModel> appDateMeta { get; set; }
        //public IEnumerable<StageModel> appstage { get; set; }
        //public IEnumerable<GripsModel> appgrips { get; set; }
        //public IEnumerable<ObjectSetsModel> appObjectSets { get; set; }

    }

}
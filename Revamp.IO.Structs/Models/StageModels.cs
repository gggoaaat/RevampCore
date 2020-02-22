using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
using System.Globalization;
//using System.Web.Security;
using System.Web;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class StageModels
    {
        /// <summary>
        /// Information and Data for the current Stage
        /// </summary>
        /// 
        public long? Stages_id { get; set; }
        public StageModel Stage { get; set; }
        public ViewStageModel StageView { get; set; }

        /// <summary>
        /// All Grips belonging to a Stage
        /// </summary>
        public virtual List<GripModels> Grips { get; set; }
    }

    [Serializable]
    public class StageModel
    {
        public long? stages_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public string application_name { get; set; }
        public long? containers_id { get; set; }
        public long? identities_id { get; set; }
        public string object_layer { get; set; }
        public long? cores_id { get; set; }
        public string stage_link { get; set; }

        public virtual List<GripModel> Grips { get; set; }
    }

    [Serializable]
    public class ViewStageModel
    {
        #region VW__STAGES columns
        public long? stages_id { get; set; }
        public long? base_stages_id { get; set; }
        public long? prev_stages_id { get; set; }
        public Guid? stages_uuid { get; set; }
        public Guid? base_stages_uuid { get; set; }
        public Guid? prev_stages_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public long? containers_id { get; set; }
        public string stage_link { get; set; }
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
        public string object_type { get; set; }
        public string object_layer { get; set; }
        public long? containers_id_f_containers { get; set; }
        public long? base_containers_id { get; set; }
        public long? prev_containers_id { get; set; }
        public Guid? containers_uuid { get; set; }
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
        public long? identities_id_f_identities { get; set; }
        public long? base_identities_id { get; set; }
        public long? prev_identities_id { get; set; }
        public Guid? identities_uuid { get; set; }
        public Guid? base_identities_uuid { get; set; }
        public Guid? prev_identities_uuid { get; set; }
        public long? identities__id { get; set; }
        public string enabled_f_identities { get; set; }
        public DateTime? dt_created_f_identities { get; set; }
        public DateTime? dt_updated_f_identities { get; set; }
        public DateTime? dt_available_f_identities { get; set; }
        public DateTime? dt_end_f_identities { get; set; }
        public string object_type_f_identities { get; set; }
        public string user_name { get; set; }
        public string edipi { get; set; }
        public string email { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        public long? applications_id_f_applications { get; set; }
        public long? base_applications_id { get; set; }
        public long? prev_applications_id { get; set; }
        public Guid? applications_uuid { get; set; }
        public Guid? base_applications_uuid { get; set; }
        public Guid? prev_applications_uuid { get; set; }
        public long? identities_id_f_applications { get; set; }
        public string enabled_f_applications { get; set; }
        public DateTime? dt_created_f_applications { get; set; }
        public DateTime? dt_updated_f_applications { get; set; }
        public DateTime? dt_available_f_applications { get; set; }
        public DateTime? dt_end_f_applications { get; set; }
        public string object_type_f_applications { get; set; }
        public long? cores_id { get; set; }
        public string application_name { get; set; }
        public string rendition { get; set; }
        public string root_application { get; set; }
        public string application_link { get; set; }
        public string application_description { get; set; }
        public string background_color { get; set; }
        #endregion

        public Guid? cores_uuid { get; set; }       
        public string formbodySpan { get; set; }

        public List<SecurityStageRoles> roles = new List<SecurityStageRoles>();

        public virtual List<ViewGripModel> Grips { get; set; } = new List<ViewGripModel>();

        public virtual List<ViewObjectSetModel> Gridster { get; set; } = new List<ViewObjectSetModel>();

        public List<Dictionary<string, object>> GridsterGrids = new List<Dictionary<string, object>>();
    }

    [Serializable]
    public class ViewGripModel
    {       
        #region VW__GRIPS     
        public long? grips_id { get; set; }
        public long? base_grips_id { get; set; }
        public long? prev_grips_id { get; set; }
        public Guid? grips_uuid { get; set; }
        public Guid? base_grips_uuid { get; set; }
        public Guid? prev_grips_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public Guid? stages_uuid { get; set; }
        public long? stages_id { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public long? stages_id_f_stages { get; set; }
        public long? base_stages_id { get; set; }
        public long? prev_stages_id { get; set; }
        public Guid? stages_uuid_f_stages { get; set; }
        public Guid? base_stages_uuid { get; set; }
        public Guid? prev_stages_uuid { get; set; }
        public long? identities_id_f_stages { get; set; }
        public string enabled_f_stages { get; set; }
        public DateTime? dt_created_f_stages { get; set; }
        public DateTime? dt_updated_f_stages { get; set; }
        public DateTime? dt_available_f_stages { get; set; }
        public DateTime? dt_end_f_stages { get; set; }
        public Guid? applications_uuid { get; set; }
        public long? applications_id { get; set; }
        public string stage_type_f_stages { get; set; }
        public string stage_name_f_stages { get; set; }
        public string stage_link { get; set; }
        public long? identities_id_f_identities { get; set; }
        public long? base_identities_id { get; set; }
        public long? prev_identities_id { get; set; }
        public Guid? identities_uuid { get; set; }
        public Guid? base_identities_uuid { get; set; }
        public Guid? prev_identities_uuid { get; set; }
        public long? identities__id { get; set; }
        public string enabled_f_identities { get; set; }
        public DateTime? dt_created_f_identities { get; set; }
        public DateTime? dt_updated_f_identities { get; set; }
        public DateTime? dt_available_f_identities { get; set; }
        public DateTime? dt_end_f_identities { get; set; }
        public string object_type { get; set; }
        public string user_name { get; set; }
        public string edipi { get; set; }
        public string email { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        #endregion

        public virtual List<ViewObjectSetModel> ObjectSets { get; set; } = new List<ViewObjectSetModel>();
    }

    [Serializable]
    public class ViewObjectSetModel
    {
        #region VW__OBJECT_SETS       
        public long? object_sets_id { get; set; }
        public long? base_object_sets_id { get; set; }
        public long? prev_object_sets_id { get; set; }
        public Guid? object_sets_uuid { get; set; }
        public Guid? base_object_sets_uuid { get; set; }
        public Guid? prev_object_sets_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public Guid? grips_uuid { get; set; }
        public long? grips_id { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public string object_type { get; set; }
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
        public long? identities_id_f_identities { get; set; }
        public long? base_identities_id { get; set; }
        public long? prev_identities_id { get; set; }
        public Guid? identities_uuid { get; set; }
        public Guid? base_identities_uuid { get; set; }
        public Guid? prev_identities_uuid { get; set; }
        public long? identities__id { get; set; }
        public string enabled_f_identities { get; set; }
        public DateTime? dt_created_f_identities { get; set; }
        public DateTime? dt_updated_f_identities { get; set; }
        public DateTime? dt_available_f_identities { get; set; }
        public DateTime? dt_end_f_identities { get; set; }
        public string object_type_f_identities { get; set; }
        public string user_name { get; set; }
        public string edipi { get; set; }
        public string email { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        public long? grips_id_f_grips { get; set; }
        public long? base_grips_id { get; set; }
        public long? prev_grips_id { get; set; }
        public Guid? grips_uuid_f_grips { get; set; }
        public Guid? base_grips_uuid { get; set; }
        public Guid? prev_grips_uuid { get; set; }
        public long? identities_id_f_grips { get; set; }
        public string enabled_f_grips { get; set; }
        public DateTime? dt_created_f_grips { get; set; }
        public DateTime? dt_updated_f_grips { get; set; }
        public DateTime? dt_available_f_grips { get; set; }
        public DateTime? dt_end_f_grips { get; set; }
        public Guid? stages_uuid { get; set; }
        public long? stages_id { get; set; }
        public string stage_type_f_grips { get; set; }
        public string stage_name_f_grips { get; set; }
        public string grip_type_f_grips { get; set; }
        public string grip_name_f_grips { get; set; }
        #endregion

        public virtual List<ViewObjectPropSetsModel> ObjectPropSets { get; set; } = new List<ViewObjectPropSetsModel>();
    }

    [Serializable]
    public class ViewObjectPropSetsModel
    {
        #region VW__OBJ_PROP_SETS
        public long? obj_prop_sets_id { get; set; }
        public long? base_obj_prop_sets_id { get; set; }
        public long? prev_obj_prop_sets_id { get; set; }
        public Guid? obj_prop_sets_uuid { get; set; }
        public Guid? base_obj_prop_sets_uuid { get; set; }
        public Guid? prev_obj_prop_sets_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public Guid? object_sets_uuid { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        public long? object_sets_id_f_object_sets { get; set; }
        public long? base_object_sets_id { get; set; }
        public long? prev_object_sets_id { get; set; }
        public Guid? object_sets_uuid_f_object_sets { get; set; }
        public Guid? base_object_sets_uuid { get; set; }
        public Guid? prev_object_sets_uuid { get; set; }
        public long? identities_id_f_object_sets { get; set; }
        public string enabled_f_object_sets { get; set; }
        public DateTime? dt_created_f_object_sets { get; set; }
        public DateTime? dt_updated_f_object_sets { get; set; }
        public DateTime? dt_available_f_object_sets { get; set; }
        public DateTime? dt_end_f_object_sets { get; set; }
        public Guid? grips_uuid { get; set; }
        public long? grips_id { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public string object_type { get; set; }
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
        public long? value_datatypes_id { get; set; }
        public long? base_value_datatypes_id { get; set; }
        public long? prev_value_datatypes_id { get; set; }
        public Guid? value_datatypes_uuid { get; set; }
        public Guid? base_value_datatypes_uuid { get; set; }
        public Guid? prev_value_datatypes_uuid { get; set; }
        public long? identities_id_f_value_datatypes { get; set; }
        public string enabled_f_value_datatypes { get; set; }
        public DateTime? dt_created_f_value_datatypes { get; set; }
        public DateTime? dt_updated_f_value_datatypes { get; set; }
        public DateTime? dt_available_f_value_datatypes { get; set; }
        public DateTime? dt_end_f_value_datatypes { get; set; }
        public string value_datatype_f_value_datatypes { get; set; }
        #endregion        

        public virtual List<ViewObjectPropOptSetsModel> ObjectPropOptSets { get; set; } = new List<ViewObjectPropOptSetsModel>();
        public virtual List<ViewObjectPropSetFile> PropFiles { get; set; }
        public virtual List<ViewObjectPropSetString> PropStrings { get; set; }
        public virtual List<ViewObjectPropSetNumb> PropNumb { get; set; }
        public virtual List<ViewObjectPropSetDate> PropDate { get; set; }
        public virtual List<ViewObjectPropFile> Files { get; set; }

    }

    [Serializable]
    public class ViewObjectPropSetString
    {
        public long? obj_prop_sets_dat_char_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? identities_id { get; set; }
        public string value { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public long? containers_id { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public string object_type { get; set; }
    }

    [Serializable]
    public class ViewObjectPropSetNumb
    {
        public long? obj_prop_sets_dat_numb_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? identities_id { get; set; }
        public long? value { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public long? containers_id { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public string object_type { get; set; }
    }

    [Serializable]
    public class ViewObjectPropSetDate
    {
        public long? obj_prop_sets_dat_date_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? identities_id { get; set; }
        public DateTime value { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public long? containers_id { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public string object_type { get; set; }
    }

    [Serializable]
    public class ViewObjectPropSetFile
    {
        public long? obj_prop_sets_dat_file_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? identities_id { get; set; }
        public byte[] value { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public long? containers_id { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public string object_type { get; set;}
    }

    [Serializable]
    public class ViewObjectPropFile
    {
        public long? obj_prop_sets_dat_file_id { get; set; }
        public long? base_obj_prop_sets_dat_file_id { get; set; }
        public long? prev_obj_prop_sets_dat_file_id { get; set; }
        public Guid? obj_prop_sets_dat_file_uuid { get; set; }
        public Guid? base_obj_prop_sets_dat_file_uuid { get; set; }
        public Guid? prev_obj_prop_sets_dat_file_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public Guid? obj_prop_sets_uuid { get; set; }
        public string file_name { get; set; }
        public long? file_size { get; set; }
        public string content_type { get; set; }
        public byte[] value { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? base_obj_prop_sets_id { get; set; }
        public long? prev_obj_prop_sets_id { get; set; }
        public Guid? obj_prop_sets_uuid_f_obj_prop_sets { get; set; }
        public Guid? base_obj_prop_sets_uuid { get; set; }
        public Guid? prev_obj_prop_sets_uuid { get; set; }
        public long? identities_id_f_obj_prop_sets { get; set; }
        public string enabled_f_obj_prop_sets { get; set; }
        public DateTime? dt_created_f_obj_prop_sets { get; set; }
        public DateTime? dt_updated_f_obj_prop_sets { get; set; }
        public DateTime? dt_available_f_obj_prop_sets { get; set; }
        public DateTime? dt_end_f_obj_prop_sets { get; set; }
        public Guid? object_sets_uuid { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
    }

    [Serializable]
    public class ViewObjectPropOptSetsModel
    {
        #region VW__OBJ_PROP_OPT_SETS
        public long? obj_prop_opt_sets_id { get; set; }
        public long? base_obj_prop_opt_sets_id { get; set; }
        public long? prev_obj_prop_opt_sets_id { get; set; }
        public Guid? obj_prop_opt_sets_uuid { get; set; }
        public Guid? base_obj_prop_opt_sets_uuid { get; set; }
        public Guid? prev_obj_prop_opt_sets_uuid { get; set; }
        public long? identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime? dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public Guid? obj_prop_sets_uuid { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public string option_value { get; set; }
        public string option_name { get; set; }
        public long? obj_prop_sets_id_f_obj_prop_sets { get; set; }
        public long? base_obj_prop_sets_id { get; set; }
        public long? prev_obj_prop_sets_id { get; set; }
        public Guid? obj_prop_sets_uuid_f_obj_prop_sets { get; set; }
        public Guid? base_obj_prop_sets_uuid { get; set; }
        public Guid? prev_obj_prop_sets_uuid { get; set; }
        public long? identities_id_f_obj_prop_sets { get; set; }
        public string enabled_f_obj_prop_sets { get; set; }
        public DateTime? dt_created_f_obj_prop_sets { get; set; }
        public DateTime? dt_updated_f_obj_prop_sets { get; set; }
        public DateTime? dt_available_f_obj_prop_sets { get; set; }
        public DateTime? dt_end_f_obj_prop_sets { get; set; }
        public Guid? object_sets_uuid { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        #endregion
    }


    //public class StageModel
    //{
    //    [Required]
    //    public string DB_PLATFORM { get; set; }
    //    public string connOwner { get; set; }
    //    [Required]
    //    public string _Core_ID { get; set; }
    //}
    [Serializable]
    public class GripModels
    {
        /// <summary>
        /// List of Object Sets Belonging to Grips
        /// </summary>
        public virtual List<ObjectSetModels> ObjectSets { get; set; }


        public ViewGripModel GripView { get; set; }
        public GripModel gripinfo { get; set; }

    }

    [Serializable]
    public class GripModel
    {
        public long? grips_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? stages_id { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public long? containers_id { get; set; }
        public long? identities_id { get; set; }

        public virtual List<ObjectSetModels> ObjectSets { get; set; }
    }

    [Serializable]
    public class ObjectSetModel
    {
        public long? object_sets_id { get; set; }
        public long? grips_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public string object_type { get; set; }
        public long? containers_id { get; set; }
        public long? identities_id { get; set; }
    }

    [Serializable]
    public class ObjectSetModels
    {
        public ObjectSetModel SetModel { get; set; }

        public ViewObjectSetModel SetView { get; set; }

        public virtual List<ObjectPropSetModels> ObjectPropSets { get; set; }
    }

    [Serializable]
    public class ObjectPropSetModels
    {
        public PropSetView PropSetView { get; set; }

        public PropSet PropSet { get; set; }

        public virtual List<ObjectPropOptSetModels> ObjectPropOptSets { get; set; }
    }

    [Serializable]
    public class PropSet
    {
        public long? obj_prop_sets_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
    }

    [Serializable]
    public class PropSetView
    {
        public long? obj_prop_sets_id { get; set; }
        public string enabled { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime dt_created { get; set; }
        public DateTime? dt_available  { get; set; }
        public DateTime? dt_end { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        public long? grips_id { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public string grip_type { get; set; }
        public string grip_name { get; set; }
        public string object_type { get; set; }
        public long? containers_id { get; set; }
        public long? identities_id { get; set; }
        public string object_layer { get; set; }
        
        public virtual List<ViewObjectPropOptSetsModel> ObjectPropOptSets { get; set; }
        public virtual List<ViewObjectPropSetFile> PropFiles { get; set; }
        public virtual List<ViewObjectPropSetString> PropStrings { get; set; }
        public virtual List<ViewObjectPropSetNumb> PropNumb { get; set; }
        public virtual List<ViewObjectPropSetDate> PropDate { get; set; }
    }

    [Serializable]
    public class ObjectPropOptSetModels
    {
        public long? obj_prop_opt_sets_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public string option_value { get; set; }
    }

    [Serializable]
    public class ObjectDefaultStatusModel
    {
        public long? obj_prop_opt_sets_id { get; set; }       
        public long? obj_prop_sets_id { get; set; }
        public long? object_sets_id { get; set; }
        public string property_value { get; set; }
        public string option_value { get; set; }
    }

    [Serializable]
    public class  ApplicationObjects
    {
        public string controller { get; set; }
        public string actionname { get; set; }
        public long? application_id { get; set; }
        public string application_name { get; set; }
        public List<ViewStageModel> stage { get; set; }
        //public List<ViewGripModel> grip { get; set; }
        //public List<ViewObjectSetModel> objectset { get; set; }
        //public List<ViewObjectPropSetsModel> propset { get; set; }
    }

    [Serializable]
    public class StageBuilderModel
    {
        public object i_this_caller { get; set; }
        public List<StageBuilderStagesModel> stage { get; set; }
        public List<StageBuilderGridSectionsModel> flow { get; set; }
        public string settings { get; set; }
        public string app_name { get; set; }
        public string app_core { get; set; }
        public Guid? applications_uuid { get; set; }
        public Guid? base_aplications_uuid { get; set; }
        
        public Guid? cores_uuid { get; set; }
        public Guid? base_cores_uuid { get; set; }
        public string app_container { get; set; }
        public List<string> app_subcontainers { get; set; }
        public string app_description { get; set; }
        public string app_email_confirm_msg { get; set; }
        public List<string> app_roles { get; set; }
        public string app_logo { get; set; }
        public string app_background_color { get; set; }
        public string app_hide_tool_bar { get; set; }
        public string app_send_email_confirm { get; set; }
        public Guid? parent_id { get; set; }
        public long? rendition { get; set; } = 0;
        public string stage_link { get; set; }
    }

    [Serializable]
    public class AppStatusField {
        public string name { get; set; }
        public List<Dictionary<string, string>> statusOption { get; set; }
        public List<string> stage_roles { get; set; }
    }

    [Serializable]
    public class StageBuilderStagesModel
    {
        public List<AppStatusField> AppStatusCollection { get; set; }

        public object i_this_caller { get; set; }
        public object stages_uuid { get; set; }
        public object base_stages_uuid { get; set; }
        public object prev_stages_uuid { get; set; }

        public List<StageBuilderSectionsModel> grid_items { get; set; }
        public List<StageBuilderGridSectionsModel> grid_sections { get; set; }
        public List<string> stage_roles { get; set; }
        public List<string> stage_parents { get; set; }
        public List<string> stage_children { get; set; }
        public string stage_name { get; set; }
        public string stage_link { get; set; }
        public string stage_background { get; set; }

    }

    [Serializable]
    public class StageBuilderSectionsModel
    {
        public object i_this_caller { get; set; }
        public object object_sets_uuid { get; set; }
        public object base_object_sets_uuid { get; set; }
        public object prev_object_sets_uuid { get; set; }

        public string section { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string objectdata { get; set; }
        public string label { get; set; }
        public string required { get; set; }
        public string show_label { get; set; }
        public string req_message { get; set; }
        public List<StageBuilderFieldOptions> options { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public string minlength { get; set; }
        public string maxlength { get; set; }
        public string format { get; set; }
        public string reg_exp { get; set; }
        public string pattern { get; set; }
        public string pattern_message { get; set; }
        public string message { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string digits { get; set; }
        public List<string> num_objects { get; set; }
        public string number_check { get; set; }
        public string currency_check { get; set; }
        public string dropdown_check { get; set; }
        public string imageData { get; set; }
        public string imageSource { get; set; }
        public string imageId { get; set; }
        public string currencyType { get; set; }
        public string tabindex { get; set; }
        public string columnorder { get; set; }
        public string email { get; set; }
        public string url { get; set; }
        public string date { get; set; }
        public string phone { get; set; }
        public string time { get; set; }
        public string button_border { get; set; }
        public string button_fontcolor { get; set; }
        public string button_bgcolor { get; set; }
        public string button_type { get; set; }
        public string button_stage { get; set; }
        public string button_submit { get; set; }
        public string button_send { get; set; }
        public string link_type { get; set; }
        public string link_stage { get; set; }
        public string link_target { get; set; }
        public string link_url { get; set; }
        public string field_size { get; set; }
        public string filesize { get; set; }
        public string accept { get; set; }
        public string field_height { get; set; }
        public string con_action { get; set; }
        public string con_logic { get; set; }
        public List<StageBuilderFieldConditions> conditions { get; set; }
        public List<string> section_roles { get; set; }
        public string status_check { get; set; }
        public string status_type { get; set; }
        public string status_identity { get; set; }
        public string bind_check { get; set; }
        public string values_check { get; set; }
        public string roles_check { get; set; }
        public string roles_collections { get; set; }
        public string roles_user { get; set; }
        public string bind_stage { get; set; }
        public List<string> bind_objects { get; set; }
        public string status_message { get; set; }
        public string style { get; set; }
        public string view_table_stage { get; set; }
        public string view_table_filter { get; set; }
        public List<string> view_table_columns { get; set; }



    }

    [Serializable]
    public class StageBuilderFieldConditions
    {
        public object i_this_caller { get; set; }
        public string con_id { get; set; }
        public string con_field { get; set; }
        public string con_operator { get; set; }
        public string con_option { get; set; }
    }

    [Serializable]
    public class StageBuilderFieldOptions
    {
        public object i_this_caller { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    [Serializable]
    public class StageBuilderGridSectionsModel
    {
        public object i_this_caller { get; set; }
        public string id { get; set; }
        public string data_col { get; set; }
        public string data_row { get; set; }
        public string data_sizex { get; set; }
        public string data_sizey { get; set; }
        public string section_top_border { get; set; }
        public string section_bottom_border { get; set; }
        public string section_left_border { get; set; }
        public string section_right_border { get; set; }
        public string section_border_style { get; set; }
        public string background_color { get; set; }
        public string background_image { get; set; }
        public string color { get; set; }
        public List<string> section_roles { get; set; }
      

    }  
}
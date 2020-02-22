using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class IdentityModels
    {
        public string username { get; set; }
        public string password { get; set; }

        public virtual List<CoreModels> cores { get; set; }

        public virtual List<ContainersModel> containers { get; set; }
        public IdentityModel Identity  { get; set; }

        public ViewProfileModel Profile { get; set; }

        public ViewIdentityModel ViewIdentity { get; set; }

        public IdentityObjects Identities { get; set; }

        public List<ViewSourceSecPerm> ApplicationPermissions { get; set; }

        public List<ViewSourceSecPerm> StagePermissions { get; set; }

        public List<IdentityAppStatus> IDStatuses { get; set; }

        public List<IdentityProperties> IDProperties { get; set; }

        //public long? identities_id { get; set; }
        //public string enabled { get; set; }
        //public DateTime dt_created { get; set; }
        //public DateTime? dt_updated { get; set; }
        //public DateTime? dt_available { get; set; }
        //public DateTime? dt_end { get; set; }
        //public string object_type { get; set; }

        //public virtual ICollection<IdentityCharDataModel> chardata { get; set; }
        //public virtual ICollection<IdentityNumbDataModel> numbdata { get; set; }
        //public virtual ICollection<IdentityDateDataModel> datedata { get; set; }

        //public virtual ICollection<ViewIdentityCharDataModel> viewchardata { get; set; }
        //public virtual ICollection<ViewIdentityNumbDataModel> viewnumbdata { get; set; }
        //public virtual ICollection<ViewIdentityDateDataModel> viewdatedata { get; set; }

        //public virtual 


    }

    [Serializable]
    public class IdentityAppStatus
    {
        public long? forms_id { get; set; }
        public long? identities_id { get; set; }
        public DateTime datesubmitted { get; set; }
        public DateTime lastupdated { get; set; }
        public long? applications_id { get; set; }
        public string application_name { get; set; }
        public string StatusName { get; set; }
        public string StatusValue { get; set; }
    }

    [Serializable]
    public class IdentityProperties
    {
        public long? identity_properties_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string property_type { get; set; }
        public string property_name { get; set; }
        public long? identities_id { get; set; }
    }

    [Serializable]
    public class IdentityObjects
    {
        public List<ViewIdentityModel>Identity { get; set; }
    }

    [Serializable]
    public class IdentityModel
    {
        //public long? identities_id { get; set; }
        public string username { get; set; } = "Anonymous@Unathenticated.User.COM";
        public long? identities_id { get; set; }
        public Guid? identities_uuid { get; set; }
        public string enabled { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public string user_name { get; set; }

        public virtual List<IdentityCharDataModel> chardata { get; set; }
        public virtual List<IdentityNumbDataModel> numbdata { get; set; }
        public virtual List<IdentityDateDataModel> datedata { get; set; }

        
    }

    [Serializable]
    public class IdentityCharDataModel
    {
        public long? identities_dat_char_id { get; set; }
        public string enabled { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public string value { get; set; }
    }

    [Serializable]
    public class IdentityNumbDataModel
    {
        public long? identities_dat_numb_id { get; set; }
        public string enabled { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? value { get; set; }

    }

    [Serializable]
    public class IdentityDateDataModel
    {
        public long? identities_dat_date_id { get; set; }
        public string enabled { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public DateTime value { get; set; }
    }

    [Serializable]
    public class ViewIdentityModel
    {

        public long? identities_id { get; set; }
        public string enabled { get; set; }
        public string active { get; set; }
        public string verified { get; set; }

        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public string user_name { get; set; }
        public string object_layer { get; set; }
        public string edipi { get; set; }
        public string email { get; set; }
        public virtual ICollection<ViewIdentityCharDataModel> chardata { get; set; }
        public virtual ICollection<ViewIdentityNumbDataModel> numbdata { get; set; }
        public virtual ICollection<ViewIdentityDateDataModel> datedata { get; set; }
    }

    [Serializable]
    public class ViewIdentityCharDataModel
    {
        public long? identities_dat_char_id { get; set; }
        public string enabled { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public string value { get; set; }
        public string object_type { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public long? containers_id { get; set; }
        public string grip_name { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        public string user_name { get; set; }
    }

    [Serializable]
    public class ViewIdentityNumbDataModel
    {
        public string active { get; set; }
        public string verified { get; set; }
        public long? identities_dat_numb_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public long? value { get; set; }
        public string object_type { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public long? containers_id { get; set; }
        public string grip_name { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        public string user_name { get; set; }
    }

    [Serializable]
    public class ViewIdentityDateDataModel
    {
        public long? identities_dat_date_id { get; set; }
        public string enabled { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? stages_id { get; set; }
        public long? grips_id { get; set; }
        public long? obj_prop_sets_id { get; set; }
        public DateTime? value { get; set; }
        public string object_type { get; set; }
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public long? containers_id { get; set; }
        public string grip_name { get; set; }
        public long? object_sets_id { get; set; }
        public string object_prop_type { get; set; }
        public string value_datatype { get; set; }
        public string property_name { get; set; }
        public string has_parent { get; set; }
        public string has_child { get; set; }
        public long? parent_obj_prop_sets_id { get; set; }
        public string property_value { get; set; }
        public string user_name { get; set; }
    }

   
}
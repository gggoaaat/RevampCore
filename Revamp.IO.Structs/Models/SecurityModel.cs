using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class SecurityModels
    {
        public List<NotificationModel> LoginNotifications { get; set; }

        public object PermissionNotifications { get; set; }
        public object SystemAccountRequestsNotifications { get; set; }
    }

    [Serializable]
    public class NotificationModel
    {
        public string Indicator { get; set; }
        public string Symbol { get; set; }
        public string Link { get; set; }
        public string Message { get; set; }
        public string TimeAgo { get; set; }
    }

    [Serializable]
    public class Activity
    {
        public string ActivityViewTitle { get; set; }
        public string spanLength { get; set; }
        public List<ActivityModelView> ActivityView { get; set; }
        public string newActivityCount { get; set; }

    }

    [Serializable]
    public class currentMemory
    {
        public string _num { get; set; }
        public string _result { get; set; }
    }

    [Serializable]
    public class ActivityModel
    {
        public long? activity_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? cores_id { get; set; }
        public long? applications_id { get; set; }
        public string table_source { get; set; }
        public long? table_id { get; set; }
        public long? identities_id { get; set; }
        public long? variants_id { get; set; }
        public long? symbols_id { get; set; }
        public string desc_text { get; set; }
        public long? desc_variants_id { get; set; }
        public long? desc_symbols_id { get; set; }
        public string desc_meta_text { get; set; }

    }

    [Serializable]
    public class ActivityModelView
    {
        public long? activity_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? cores_id { get; set; }
        public long? applications_id { get; set; }
        public string table_source { get; set; }
        public long? table_id { get; set; }
        public long? identities_id { get; set; }
        public long? variants_id { get; set; }
        public long? symbols_id { get; set; }
        public string desc_text { get; set; }
        public long? desc_variants_id { get; set; }
        public long? desc_symbols_id { get; set; }
        public string desc_meta_text { get; set; }
        public string object_layer { get; set; }
        public string core_name { get; set; }
        public string application_name { get; set; }
        public long? rendition { get; set; }
        public string root_application { get; set; }
        public string application_link { get; set; }
        public string variant_name { get; set; }
        public string color { get; set; }
        public string symbol_name { get; set; }
    }

    [Serializable]
    public class SystemSessions
    {
        public long? sessions_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public string sessionid { get; set; }
        public string timeout { get; set; }
        public string anonymousid { get; set; }
        public string useragent { get; set; }
        public string userhostaddress { get; set; }
        public string isauthenticated { get; set; }
        public string logonuseridentity { get; set; }
        public string browser { get; set; }
        public string majorversion { get; set; }
        public string version { get; set; }
        public string crawler { get; set; }
        public string clrversion { get; set; }
        public string cookies { get; set; }
        public string ismobiledevice { get; set; }
        public string platform { get; set; }
        public string url { get; set; }
        public string urlreferrer { get; set; }
    }

    [Serializable]
    public class AppRoles
    {
        public long? app_id { get; set; }
        public long?[] roles { get; set; }        
    }

    [Serializable]
    public class SourceSecRole
    {
        public long? source_sec_role_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? source_id { get; set; }
        public long? roles_id { get; set; }
    }

    [Serializable]
    public class SourceSecPriv
    {
        public long? source_sec_priv_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? source_id { get; set; }
        [Display(Name = "privileges_id")]
        public long? privileges_id { get; set; }
        public long? identities_id { get; set; }
    }

    [Serializable]
    public class SourceSecPerm
    {
        public long? source_sec_perm_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? source_id { get; set; }
        public long? roles_id { get; set; }
        public long? identities_id { get; set; }
        public long? rendition { get; set; }
    }

    [Serializable]
    public class ViewSourceSecRole
    {
        public long? source_sec_role_id { get; set; }
        public string enabled { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? source_id { get; set; }
        public long? roles_id { get; set; }
        public string role_name { get; set; }
        public string object_layer { get; set; }

        //application specific
        public long? cores_id { get; set; }
        public string application_name { get; set; }
        public string rendition { get; set; }
        public string root_application { get; set; }
        public string application_link { get; set; }

        //Stages Specific
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public string containers_id { get; set; }
        public string stage_link { get; set; }
    }

    [Serializable]
    public class ViewSourceSecPriv
    {
        public long? source_sec_priv_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? source_id { get; set; }
        [Display(Name = "privileges_id")]
        public long? privileges_id { get; set; }
        public long? identities_id { get; set; }
        [Display(Name = "privileges_name")]
        public string privileges_name { get; set; }
        public string user_name { get; set; }
        public string object_layer { get; set; }
        public long? cores_id { get; set; }
        public string application_name { get; set; }
        public string rendition { get; set; }
        public string root_application { get; set; }
        public string application_link { get; set; }
    }

    [Serializable]
    public class ViewSourceSecPerm
    {
        public long? source_sec_perm_id { get; set; }
        public string enabled { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public long? source_id { get; set; }
        public long? roles_id { get; set; }
        public long? identities_id { get; set; }
        public string role_name { get; set; }
        public string user_name { get; set; }
        public string object_layer { get; set; }
        public long? rendition { get; set; }

        //Applications specific
        public long? cores_id { get; set; }
        public string application_name { get; set; }      
        public string root_application { get; set; }
        public string application_link { get; set; }

        //Stages Specific
        public string stage_type { get; set; }
        public string stage_name { get; set; }
        public long? applications_id { get; set; }
        public string containers_id { get; set; }
        public string stage_link { get; set; }
    }
    
    [Serializable]
    public class AppAccessModel
    {
        public long? app_access_id { get; set; }

        public string enabled { get; set; }

        public DateTime dt_created { get; set; }
        
        public DateTime? dt_updated { get; set; }

        public DateTime? dt_available { get; set; }

        public DateTime? dt_end { get; set; }

        public string object_type { get; set; }

        public long? applications_id { get; set; }

        public long? roles_id { get; set; }

        public long? identities_id { get; set; }

        public long? rendition { get; set; }
    }

    [Serializable]
    public class ViewAppAccessModel
    {
        public long? app_access_id { get; set; }

        public string enabled { get; set; }

        public DateTime dt_created { get; set; }

        public DateTime? dt_available { get; set; }

        public DateTime? dt_end { get; set; }

        public string object_type { get; set; }

        public long? applications_id { get; set; }

        public long? roles_id { get; set; }

        public long? identities_id { get; set; }

        public long? rendition { get; set; }

        public string object_layer { get; set; }

        public long? cores_id { get; set; }

        public string application_name { get; set; }

        public string root_application { get; set; }

        public string application_link { get; set; }

        public string user_name { get; set; }

        public string role_name { get; set; }
    }

    [Serializable]
    public class SecurityModel
    {
    }

    [Serializable]
    public class RolesModel
    {
        public long? roles_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? DT_UPDATED { get; set; }
        public DateTime? DT_AVAILABLE { get; set; }
        public DateTime? DT_END { get; set; }
        public string OBJECT_TYPE { get; set; }
        public string ROLE_NAME { get; set; }
    }

    //public class ViewRolesModel
    //{
    //    public long? roles_id { get; set; }
    //    public string enabled { get; set; }
    //    public DateTime dt_created { get; set; }
    //    public DateTime? DT_AVAILABLE { get; set; }
    //    public DateTime? DT_END { get; set; }
    //    public string OBJECT_TYPE { get; set; }
    //    public string ROLE_NAME { get; set; }
    //    public string OBJECT_LAYER { get; set; }
    //}

    //public class PrivilegesModel
    //{
    //    public long? privileges_id { get; set; }
    //    public string enabled { get; set; }
    //    public DateTime dt_created { get; set; }
    //    public DateTime? dt_updated { get; set; }
    //    public DateTime? dt_available { get; set; }
    //    public DateTime? dt_end { get; set; }
    //    public string object_type { get; set; }
    //    public string privilege_name { get; set; }
    //}

    //public class ViewPrivilegesModel
    //{
    //    public long? privileges_id { get; set; }
    //    public string enabled { get; set; }
    //    public DateTime dt_created { get; set; }
    //    public DateTime? dt_available { get; set; }
    //    public DateTime? dt_end { get; set; }
    //    public string object_type { get; set; }
    //    public string privilege_name { get; set; }
    //    public string OBJECT_LAYER { get; set; }
    //}

    [Serializable]
    public class PermissionModels
    {
        public PrievelegesModel PermissionTable { get; set; }
        public ViewPrivilegesModel PermissionView { get; set; }
    }

    [Serializable]
    public class PrievelegesModel
    {
        [Display(Name = "privileges_id")]
        public long? privileges_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        [Display(Name = "privilege_name")]
        public string privilege_name { get; set; }

    }

    [Serializable]
    public class ViewPrivilegesModel
    {
        [Display(Name = "privileges_id")]
        public long? privileges_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        [Display(Name = "privilege_name")]
        public string privilege_name { get; set; }
        public string object_layer { get; set; }
    }
    
    [Serializable]
    public class RoleModels
    {
        public RoleModel RoleTable { get; set; }
        public ViewRoleModel ViewRole { get; set; }

        public List<ViewRolesPrivilegesModel> ViewRolePrivs { get; set; }
        public List<RolesPrivilegesModel> RolePrivs { get; set; }
        public List<PermissionModels> Permissions { get; set; }

    }
    
    [Serializable]
    public class RoleModel
    {
        public long? roles_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public string role_name { get; set; }
    }

    [Serializable]
    public class ViewRoleModel
    {
        public long? roles_id { get; set; }
        public string enabled { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public string role_name { get; set; }
        public string object_layer { get; set; }
        public long? cores_id { get; set; }
        public string core_name { get; set; }   
    }

    [Serializable]
    public class RolesPrivilegesModel
    {
        [Display(Name = "roles_privileges_id")]
        public long? roles_privileges_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string role_name { get; set; }
        [Display(Name = "privilege_name")]
        public string privilege_name { get; set; }
    }

    [Serializable]
    public class ViewRolesPrivilegesModel
    {
        [Display(Name = "roles_privileges_id")]
        public long? roles_privileges_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string role_name { get; set; }
        [Display(Name = "privilege_name")]
        public string privilege_name { get; set; }
        public string object_type { get; set; }
        public string core_Name { get; set; }
    }

    [Serializable]
    public class GroupModels
    {
        public GroupModel GroupTable { get; set; }
        public ViewGroupModel ViewGroup { get; set; }

        public List<ViewGroupsRolesModel> ViewGroupRoles { get; set; }
        public List<GroupRolesModel> GroupRoles { get; set; }
        public List<ViewGroupMembersModel> GroupIdentities { get; set; }
        public List<PermissionModels> Permissions { get; set; }

    }

    [Serializable]
    public class GroupModel
    {
        public long? groups_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public string object_layer { get; set; }
        public string group_name { get; set; }
    }

    [Serializable]
    public class ViewGroupModel
    {
        public long? groups_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public string object_type { get; set; }
        public string group_name { get; set; }
        public long? cores_id { get; set; }
        public string core_name { get; set; }
        public string object_layer { get; set; }


    }

    [Serializable]
    public class GroupRolesModel
    {
        public long? groups_roles_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? roles_id { get; set; }
        public long? groups_id { get; set; }
        public string role_name { get; set; }
        public string group_name { get; set; }
        public string object_layer { get; set; }
    }

    [Serializable]
    public class ViewGroupsRolesModel
    {
        public long? groups_roles_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? roles_id { get; set; }
        public long? groups_id { get; set; }
        public long? cores_id { get; set; }
        public string role_name { get; set; }
        public string group_name { get; set; }
        public string object_type { get; set; }
        public string object_layer { get; set; }
    }

    [Serializable]
    public class GroupMembersModel
    {
        public long? groups_identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? groups_id { get; set; }
    }

    [Serializable]
    public class ViewGroupMembersModel
    {
        public long? group_members_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? groups_id { get; set; }
        public string user_name { get; set; }
        public string edipi { get; set; }
        public string email { get; set; }
        public string group_name { get; set; }
        public string object_type { get; set; }
        public string object_layer { get; set; }
    }

    [Serializable]
    public class IdentityRolesModel
    {
        public long? identities_roles_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? roles_id { get; set; }
        public string object_type { get; set; }
    }

    [Serializable]
    public class ViewIdentityRolesModel
    {
        public long? identities_roles_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? roles_id { get; set; }
        public string user_name { get; set; }
        public string edipi { get; set; }
        public string email { get; set; }
        public string role_name { get; set; }
        public string object_type { get; set; }
        public string object_layer { get; set; }
        public string core_name { get; set; }
        public long? cores_id { get; set; }
    }

    [Serializable]
    public class CoresIdentitiesModel
    {
        public long? cores_identities_id { get; set; }
        public string enabled { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_updated { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? cores_id { get; set; }
        public string object_type { get; set; }
    }



    [Serializable]
    public class ViewCoresIdentitiesModel
    {
        public long? cores_identities_id { get; set; }
        public string enabled { get; set; }
        public string active { get; set; }
        public string verified { get; set; }
        public DateTime dt_created { get; set; }
        public DateTime? dt_available { get; set; }
        public DateTime? dt_end { get; set; }
        public long? identities_id { get; set; }
        public long? cores_id { get; set; }
        public string user_name { get; set; }
        public string edipi { get; set; }
        public string email { get; set; }
        public string core_name { get; set; }
        public string object_type { get; set; }
        public string object_layer { get; set; }
    }

    [Serializable]
    public class appPermissions
    {
        public inheritedFromIdentityModel inheritedFromIdentity { get; set; }

        public inheritedFromGroupModel inheritedFromGroup { get; set; }

        public inheritedFromRoleModel inheritedFromRole { get; set; }
    }

    [Serializable]
    public class inheritedFromIdentityModel
    {
        public object PRIVILEGES { get; set; }
        public List<IdentityPRIVILEGES> PRIVILEGESs { get; set; }
    }

    [Serializable]
    public class inheritedFromGroupModel
    {
        public object PRIVILEGES { get; set; }
        public List<GroupPrivileges> PRIVILEGESs { get; set; }
    }

    [Serializable]
    public class inheritedFromRoleModel
    {
        public object PRIVILEGES { get; set; }
        public List<RolePrivileges> PRIVILEGESs { get; set; }
    }

    [Serializable]
    public class IdentityPRIVILEGES
    {
        public long identity_id { get; set; }
        public string username { get; set; }
        public long application_id { get; set; }
        public string application_name { get; set; }
        public long identity_role_id { get; set; }
        public long identity_group_id { get; set; }
        public long group_id { get; set; }
        public string group_name { get; set; }
        public string assigned_to_group { get; set; }
        public long role_id { get; set; }
        public string role_name { get; set; }
        public string role_code { get; set; }
        public string assigned_to_role { get; set; }
        public long privilege_id { get; set; }
        public string privilege_name { get; set; }
        public string assigned_to_privilege { get; set; }
    }

    [Serializable]
    public class GroupPrivileges
    {
        public long identity_id { get; set; }
        public string username { get; set; }
        public long application_id { get; set; }
        public string application_name { get; set; }
        public long identity_role_id { get; set; }
        public long identity_group_id { get; set; }
        public long group_id { get; set; }
        public string group_name { get; set; }
        public string assigned_to_group { get; set; }
        public long role_id { get; set; }
        public string role_name { get; set; }
        public string role_code { get; set; }
        public string assigned_to_role { get; set; }
        public long privilege_id { get; set; }
        public string privilege_name { get; set; }
        public string assigned_to_privilege { get; set; }
    }


    [Serializable]
    public class RolePrivileges
    {
        public long identity_id { get; set; }
        public string username { get; set; }
        public long application_id { get; set; }
        public string application_name { get; set; }
        public long identity_role_id { get; set; }
        public long identity_group_id { get; set; }
        public long group_id { get; set; }
        public string group_name { get; set; }
        public string assigned_to_group { get; set; }
        public long role_id { get; set; }
        public string role_name { get; set; }
        public string role_code { get; set; }
        public string assigned_to_role { get; set; }
        public long privilege_id { get; set; }
        public string privilege_name { get; set; }
        public string assigned_to_privilege { get; set; }
    }
}

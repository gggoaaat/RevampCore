using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Revamp.IO.Structs.Models
{

    /// <summary>
    /// This Model can hold all session Information
    /// </summary>
    [Serializable]
    public class SessionObjects : ISessionObjects
    {
        /// <summary>
        /// Memory Related Stuff
        /// </summary>        
        public RevampCoreSettings appSettings { get; set; }
        public SessionObjects SOPricing { get; set; }
        public MemoryUsage ERMemory { get; set; }

        public DateTime? SessionStartTime { get; set; }
        public DateTime? SessionEndTime { get; set; }
        public DateTime? LastLoad { get; set; }
        public DateTime? NextReload { get; set; }

        public IdentityModels SessionIdentity { get; set; }

        public List<NotificationsModel> Notification { get; set; }

        public Activity ActivityList { get; set; }
        public Activity LoginActivity { get; set; }
        public Activity AllActivity { get; set; }
        public Activity Notifications { get; set; }

        //Imported from ApplicationObjects Model.
        public string controller { get; set; }
        public string actionname { get; set; }
        public long? application_id { get; set; }
        public string application_name { get; set; }
        public string application_lastsaved { get; set; }
        public DataTable application_properties { get; set; }
        public long? cores_id { get; set; }
        public Guid? cores_uuid { get; set; }

        public List<ViewStageModel> stage { get; set; }

        public NotificationsModel SessionNotification { get; set; }

        public InstallerModels Installer { get; set; }

        public List<ViewObjectModel> Objects { get; set; }

        public ERDictionaryModels ERDictionary { get; set; }

        public List<RoleModels> ERRoles { get; set; }

        public List<GroupModels> ERGroups { get; set; }
        public DataTable ERGroupTable { get; set; }

        public DataTable ERIdentityRolesTable { get; set; }

        public DataTable ERCoreIdentityTable { get; set; }

        public List<ViewGroupsRolesModel> ERGroupRoles { get; set; }

        public List<ViewGroupMembersModel> ERGroupMembers { get; set; }

        public List<ViewIdentityRolesModel> ERIdentityRoles { get; set; }

        public List<ViewCoresIdentitiesModel> ERCoreIdentities { get; set; }

        public List<ViewSourceSecRole> AppRoles { get; set; }

        public List<ViewSourceSecPerm> AppPerms { get; set; }

        public RegisterModel RegisterValues { get; set; }

        public List<ViewSourceSecRole> StageRoles { get; set; }

        public List<ViewSourceSecPerm> StagePerms { get; set; }

        public List<ViewPrivilegesModel> ERPrivileges { get; set; }

        public ERChartsModels ERCharts { get; set; }

        public List<SystemSessions> ERSessions { get; set; }

        public PageFeatures _PageFeatures { get; set; }

        public List<FormattedIdentityApp> FormattedIdentityApps { get; set; }

        /// <summary>
        /// used to indicate we should reload apps and permissions from the database instead of relying on cache/session
        /// </summary>
        public bool ForceRefresh { get; set; }

        public LoadedPermissions LoadedPermissions { get; set; }

        /// <summary>
        /// used to determine whether this user's password was set by an admin, and thus must be changed before user is allowed to continue
        /// (null when we haven't checked yet)
        /// </summary>
        public bool? ForcePasswordChange { get; set; }

        public IdentityModel _IdentityModel { get; set; }

        public ActivityLog _ActivityLog { get; set; }

    }

    [Serializable]
    public class FormattedIdentityApp
    {
        public int application_id { get; set; }
        public string application_name { get; set; }
        public string template { get; set; }
        public string href { get; set; }
        public string icon { get; set; }
    }

    [Serializable]
    public class PageFeatures
    {
        public bool DatablesDotNet { get; set; }
        public bool SignalR { get; set; }
        public bool MultiSelect { get; set; }
        public bool CalendarDatePicker { get; set; }
        public bool FormWizard { get; set; }
        public bool UITabs { get; set; }
    }

    [Serializable]
    public class MemoryUsage
    {
        public long GCTotalMemory { get; set; }
        public long GCTotalMemoryMB { get; set; }
        public int SOGetGenerationID { get; set; }
        public long SOTotalMemory { get; set; }
        public long SOTotalMemoryMB { get; set; }
    }

    [Serializable]
    public class SessionModel
    {

    }

    [Serializable]
    public class SessionRequest
    {
        public long? sessions_id { get; set; }
        public string username { get; set; } = "Anonymous";
        public long? identities_id { get; set; }
        public string sessionid { get; set; }
        public string timeout { get; set; }
        public string anonymousid { get; set; }
        public string useragent { get; set; }
        public string userhostaddress { get; set; }
        public string userhostname { get; set; }
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
    public class LoadedPermissions
    {
        public List<AppPermissionTracker> LoadedAppPermissions { get; set; }

        public appPermissions AppPermissions { get; set; }
    }

    [Serializable]
    public class AppPermissionTracker
    {
        public string AppName { get; set; }
        public long AppID { get; set; }
        public DateTime TimeLoaded { get; set; }
    }

    [Serializable]
    public class SideBarModel
    {
        public string ActiveLevel1 { get; set; }
        public string ActiveLevel2 { get; set; }
        public string ActiveLevel3 { get; set; }
    }

    [Serializable]
    public class UserInterface
    {
        public bool collapseSideBar { get; set; }
    }

    [Serializable]
    public class DownloadClass
    {
        public List<string> Download_ID_List { get; set; }
    }

    [Serializable]
    public class ActivityLog
    {
        public Int64 IDENTITY_ID { get; set; }
        public Int64 LOGIN_TYPE_ID { get; set; }
        public Int64 BROWSER_ID { get; set; }
        public Int64 APP_ID { get; set; }
        public string MESSAGE { get; set; }
        public string URL { get; set; }
        public string BROWSER_SESSION_ID { get; set; }
        public string IP_ADDRESS { get; set; }
        public string SYMBOL { get; set; }
        public Int64 SYSTEM_ACTIVITY_ID { get; set; }
        public Int64 LOG_LOGON_ID { get; set; }
        public Int64 SESSION_ID { get; set; }
    }
}

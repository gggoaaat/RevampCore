using System;
using System.Collections.Generic;
using System.Data;

namespace Revamp.IO.Structs.Models
{
    public interface ISessionObjects
    {
        RevampCoreSettings appSettings { get; set; }
        string actionname { get; set; }
        Activity ActivityList { get; set; }
        Activity AllActivity { get; set; }
        long? application_id { get; set; }
        string application_lastsaved { get; set; }
        string application_name { get; set; }
        DataTable application_properties { get; set; }
        List<ViewSourceSecPerm> AppPerms { get; set; }
        List<ViewSourceSecRole> AppRoles { get; set; }
        string controller { get; set; }
        long? cores_id { get; set; }
        ERChartsModels ERCharts { get; set; }
        List<ViewCoresIdentitiesModel> ERCoreIdentities { get; set; }
        DataTable ERCoreIdentityTable { get; set; }
        ERDictionaryModels ERDictionary { get; set; }
        List<ViewGroupMembersModel> ERGroupMembers { get; set; }
        List<ViewGroupsRolesModel> ERGroupRoles { get; set; }
        List<GroupModels> ERGroups { get; set; }
        DataTable ERGroupTable { get; set; }
        List<ViewIdentityRolesModel> ERIdentityRoles { get; set; }
        DataTable ERIdentityRolesTable { get; set; }
        MemoryUsage ERMemory { get; set; }
        List<ViewPrivilegesModel> ERPrivileges { get; set; }
        List<RoleModels> ERRoles { get; set; }
        List<SystemSessions> ERSessions { get; set; }
        bool? ForcePasswordChange { get; set; }
        bool ForceRefresh { get; set; }
        List<FormattedIdentityApp> FormattedIdentityApps { get; set; }
        InstallerModels Installer { get; set; }
        DateTime? LastLoad { get; set; }
        LoadedPermissions LoadedPermissions { get; set; }
        Activity LoginActivity { get; set; }
        DateTime? NextReload { get; set; }
        List<NotificationsModel> Notification { get; set; }
        Activity Notifications { get; set; }
        List<ViewObjectModel> Objects { get; set; }
        RegisterModel RegisterValues { get; set; }
        DateTime? SessionEndTime { get; set; }
        IdentityModels SessionIdentity { get; set; }
        NotificationsModel SessionNotification { get; set; }
        DateTime? SessionStartTime { get; set; }
        SessionObjects SOPricing { get; set; }
        List<ViewStageModel> stage { get; set; }
        List<ViewSourceSecPerm> StagePerms { get; set; }
        List<ViewSourceSecRole> StageRoles { get; set; }
        ActivityLog _ActivityLog { get; set; }
        IdentityModel _IdentityModel { get; set; }
        PageFeatures _PageFeatures { get; set; }
    }
}
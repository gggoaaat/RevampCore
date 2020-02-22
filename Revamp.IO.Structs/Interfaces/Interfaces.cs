using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Interfaces
{


    public interface IDBAuthStruct
    {
        string connOwnerPassword { get; set; }
        string connServer { get; set; }
        string Database { get; set; }
        string User_ID { get; set; }
        bool IntegratedSecurity { get; set; }
    }

    public interface IWebConfigEdit
    {
        string ConnectionString { get; set; }
        string SectionName { get; set; }
    }

    public interface IAppSettingsKeyPair
    {
        string key { get; set; }
        string _value { get; set; }
    }

    public interface I_IRISSessionModels
    {
        DateTime? LastLoad { get; set; }
        DateTime? NextReload { get; set; }
        DateTime? SessionEndTime { get; set; }
        DateTime? SessionStartTime { get; set; }
        ManagementModel SystemManagement { get; set; }
        SecurityModels SystemSecurity { get; set; }
        ActivityLog _ActivityLog { get; set; }
        DownloadClass _DownloadData { get; set; }
        PageFeatures _Features { get; set; }
        IdentityModel _IdentityModel { get; set; }
        _FullReleasesModel _ReleaseNotesFull { get; set; }
        SideBarModel _SideBarModel { get; set; }
        UserInterface _UI { get; set; }
        LoadedPermissions LoadedPermissions { get; set; }

    }

    public interface IDBLoginObject
    {
        string DBLogin { get; set; }
        string DBLoginPwd { get; set; }
    }


}

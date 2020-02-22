using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Revamp.IO.Structs.Models
{

    [Serializable]
    public class ManagementModel
    {
        public List<_ApplicationModel> Applications { get; set; }
        public List<_ApplicationModel> DisabledApplications { get; set; }
        public List<_UsernameModel> UserNames { get; set; }     
    }

    [Serializable]
    public class _ApplicationModel
    {
        public Int64 Application_ID { get; set; }
        public bool Enabled { get; set; }
        public DateTime DT_Created { get; set; }
        public DateTime DT_Updated { get; set; }

        [Display(Name = "Application")]
        public string Application_Name { get; set; }
        public Int64 Template_ID { get; set; }
        public string Template { get; set; }

        public List<_GroupModel> Application_Groups { get; set; }
        public List<_RoleModel> Application_Roles  { get; set; }
        public List<_PrivilegeModel> Application_Privileges { get; set; }
        public List<_MSC> Application_MSCs { get; set; }
        public List<_UIC> Application_UICs { get; set; }
        public List<_AppProperties> Application_Properties { get; set; }
    }

    public class _AppProperties
    {
        public string Property { get; set; }
        public string Value { get; set; }
    }

    [Serializable]
    public class _GroupModel
    {
        public Int64 Group_ID { get; set; }
        public bool Enabled { get; set; }
        public DateTime DT_Created { get; set; }
        public DateTime DT_Updated { get; set; }

        [Display(Name = "Application ID")]
        public Int64 Application_ID { get; set; }

        [Display(Name = "Group Name")]
        public string Group_Name { get; set; }

    }

    [Serializable]
    public class _RoleModel
    {
        public Int64 Role_ID { get; set; }
        public bool Enabled { get; set; }
        public DateTime DT_Created { get; set; }
        public DateTime DT_Updated { get; set; }

        [Display(Name = "Application ID")]
        public Int64 Application_ID { get; set; }
        
        [Display(Name = "Role Name")]
        public string Role_Name { get; set; }

        public string Role_Code { get; set; }

        public List<_PrivilegeModel> Role_Privileges { get; set; }
    }

    [Serializable]
    public class _PrivilegeModel
    {
        public Int64 Privilege_ID { get; set; }
        public bool Enabled { get; set; }
        public DateTime DT_Created { get; set; }
        public DateTime DT_Updated { get; set; }
        public Int64 Application_ID { get; set; }
        public string Privilege_Name { get; set; }
    }

    [Serializable]
    public class _RolePrivilegeModel
    {
        public Int64 Role_Privilege_ID { get; set; }
        public bool Enabled { get; set; }
        public DateTime DT_Created { get; set; }
        public DateTime DT_Updated { get; set; }
        public Int64 Application_ID { get; set; }
        public Int64 Role_ID { get; set; }
        public Int64 Privilege_ID { get; set; }
    }

    [Serializable]
    public class _MSC
    {
        public Int64 MSC_ID { get; set; }
        public string MSC_CODE { get; set; }
        public string ASN { get; set; }
        public string MSC_Name { get; set; }
        public string FAD_MSC_Code { get; set; }
        public string Future_MSC { get; set; }
        public string Future_MSC_Name { get; set; }
        public string Rollup_MSC_Code { get; set; }
        public string MSC_Shortname { get; set; }

        public List<_UIC> MSC_UICs { get; set; }
    }

    [Serializable]
    public class _UIC
    {
        public Int64 UIC_ID { get; set; }
        public string UIC { get; set; }
        public string Unit_Name { get; set; }
        public string Rollup_MSC_Code { get; set; }
        public string Rollup_MSC_Name { get; set; }
        public Int64? seq { get; set; }
        public string MSC_Rollup_Shortname { get; set; }
        public string Doc_Type { get; set; }
        public string Compo_Cd { get; set; }
    }

    [Serializable]
    public class _UsernameModel
    {
        public Int64 Username_ID { get; set; }
        public bool Enabled { get; set; }
        public DateTime DT_Created { get; set; }
        public DateTime DT_Updated { get; set; }
        public Int64 Identity_ID { get; set; }
        public string Username { get; set; }
        public DateTime? DT_Last_Logged_In { get; set; }
    }

    [Serializable]
    public class __DocType
    {
        public string DOC_TYPE { get; set; }
    }

    [Serializable]
    public class _TemplateModel
    {
        public Int64 Templates_ID { get; set; }
        public bool Enabled { get; set; }
        public DateTime DT_Created { get; set; }
        public DateTime DT_Updated { get; set; }
        public string Template { get; set; }
    }
    
    [Serializable]
    public class Users
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public bool isMapped { get; set; }
        public Int32 UserAccountControl { get; set; }
        public bool isUserEnabled { get; set; }
        public string Message { get; set; }
    }

    [Serializable]
    public class WindowsAuthStruct
    {
        public string Username { get; set; }
        public string Domain { get; set; }
        public string FullUserName { get; set; }
    }

    [Serializable]
    public class DocumentationModel
    {
        public Int64 IDENTITY_ID { get; set; }
        public Int64 BASE_CSA_DOCUMENTATION_ID { get; set; }
        public Int64 PREV_CSA_DOCUMENTATION_ID { get; set; }
        public Int64 CSA_DOCUMENTATION_ID { get; set; }
        public char ENABLED { get; set; }
        public Int64 TEMPLATE_ID { get; set; }
        public string DOCUMENT_DESCRIPTION { get; set; }
        public string DOCUMENT_TITLE { get; set; }

        public string DOCUMENT_EXTENSION { get; set; }

        public Int64 DOCUMENT_SIZE { get; set; }

        public byte[] DOCUMENT_ATTACHMENT { get; set; }

        public byte[] DOCUMENT_PDF_ATTACHMENT { get; set; }
    }

    [Serializable]
    public class _BrowserSessionModel
    {
        public string Session_ID { get; set; }
        public string BrowserName { get; set; }
        public string Browser_ID { get; set; }
            
        public Int64 LogLogon_ID { get; set; }
        public Int64 IDENTITY_ID { get; set; }
        public string IPAddress { get; set; }
        public string Version { get; set; }
        public string UserAgent { get; set; }
    }


    [Serializable]
    public class _LogonActModel
    {
        public Int64 LoginType { get; set; }
        public _BrowserSessionModel BrwSessModel { get; set; }
        public string UserName { get; set; }
        //Todo: Fix with VS Upgrade
        public string Successful { get; set; } // = "No";
        public string SourceHost { get; set; }        
    }


}
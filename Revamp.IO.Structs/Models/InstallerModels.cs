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
    public class InstallerModels
    {
        public bool IntegratedSecurity { get; set; }

        //[Display(Name = "My property title")]
        [Required]
        public string DB_PLATFORM { get; set; }
        public string connRoot
        {
            get;
            set;
        }
        public string connAuth { get; set; }
        public string connOwner { get; set; }

        [Required(ErrorMessage = "System Name is required")]
        public string SystemName { get; set; }
        
            
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)] 
        public string Password { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")] 
        public string compare_Password { get; set; }

        [Required(ErrorMessage = "Database Name is required")]
        public string DBServer { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string DBOwnerUsername { get; set; }

        [Required(ErrorMessage = "DB Password is required")]
        [DataType(DataType.Password)] 
        public string DBOwnerPassword { get; set; }
        public string UseExistingLogin { get; set; }
        //public string DB_PLATFORM { get; set; }
        //public string connRoot { get{ return constrSQLServer;} set 
        //{
        //    connRoot = value;
        //} }
        //public string connAuth { get { return constrSQLServer1; } set { connAuth = value;} }
        //public string connOwner { get { return constrSQLServer2; } set { connOwner = value; } }
        //public string SystemName { get; set; }
        //public string Password { get; set; }

        public InstallExampleDataModels InstallExample { get; set; }

    }

    [Serializable]
    public class InstallExampleDataModels
    {
        [Required]
        public string DB_PLATFORM { get; set; }
        public string connOwner { get; set; }
        [Required]
        public string SystemName { get; set; }
    }

}
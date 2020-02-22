using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revamp.IO.Structs.Models;
using System.Configuration;
using Revamp.IO.Structs.Enums;
using Revamp.IO.Structs.Interfaces;

namespace Revamp.IO.DB.Bridge
{
    [Serializable]
    public class ConnectToDB : IConnectToDB
    {
        public string Platform { get; set; }
        public string DBConnString { get; set; }
       // public DBLoginObject DBLoginObj { get; set; }
        public string SourceDBOwner { get; set; }
        public string ConnServer { get; set; }
        public string Password { get; set; }
        public string Schema { get; set; } = "CSA";
        public string Schema2 { get; set; }
        public string authType { get; set; }
        public int? TimeOutTime { get; set; }
        public string windowsUsername { get; set; }
        public SessionObjects SessionModel { get; set; }
        public string RevampSystemName { get; set; }

        public ConnectToDB Copy()
        {
            return (ConnectToDB)Clone();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public interface IConnectToDB
    {
        string authType { get; set; }
        string ConnServer { get; set; }
        //string DBConnString { get; set; }
        //IDBLoginObject DBLoginObj { get; set; }
        //string DBRole { get; set; }
       // string DBUser { get; set; }
        string Password { get; set; }
        string Platform { get; set; }
        string Schema { get; set; }
        string Schema2 { get; set; }
        //string SourceDBName { get; set; }
        string DBConnString { get; set; }
        string SourceDBOwner { get; set; }
        int? TimeOutTime { get; set; }
        string windowsUsername { get; set; }
        SessionObjects SessionModel { get; set; }
        string RevampSystemName { get; set; }
        object Clone();
        ConnectToDB Copy();
    }


    [Serializable]
    public class SQLTrasaction
    {
        public StringBuilder SQLBlock { get; set;}

        public string _SQLBlock { get; set; }

        public List<DBParameters> EntryProcedureParameters { get; set; }

        public string ProcedureName { get; set; }
    }

    [Serializable]
    public class sqlTransBlocks
    {
        public List<DBParameters> UniversalParameters { get; set; }
        public List<TSQLCommands> Commands { get; set; } = new List<TSQLCommands>();
        public List<SQLTrasaction> Series { get; set; } = new List<SQLTrasaction>();
        public string SQLBlock { get; set; }
    }

    [Serializable]
    public class TSQLCommands
    {
        public string Syntax { get; set; }
    }

    [Serializable]
    public class SQLContent
    {
        public string ProcedureName { get; set; }

        public List<DBParameters> Parameters { get; set; }

        //public DynamicSQLContent _SQL { get; set; }

        public List<ParameterSet> AllParams { get; set; }
        public List<ParameterSet> OutParams { get; set; }  
    }
   
    [Serializable]
    public class DynamicSQLContent
    {
        public StringBuilder _SQLOut { get; set; }

              
    }

    [Serializable]
    public class ParameterSet
    {
        public string ParamName { get; set; }
        public string ParamType { get; set; }
        public string ParamValue { get; set; }
    }

    [Serializable]
    public class DropDBStruct
    {
        public string DB_PLATFORM { get; set; }
        public string connAuth { get; set; }
        public string SystemName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs
{
    public class ER_Models
    {
        
    }
    [Serializable]
    public class TableServerSide
    {
        public string query_type { get; set; }
        public string stage_id { get; set; }
        public string app_id { get; set; }
        public string cores_id { get; set; }
        public string start { get; set; }
        public string length { get; set; }
        public string search_value { get; set; }
        public string where_clause { get; set; }
        public string order_by { get; set; }
        public string order_dir { get; set; }
        public bool verify { get; set; }
        public bool run_rows { get; set; }

    }

    [Serializable]
    public class ProcedureParameterStruct
    {
        public string ParamName { get; set; }
        public string ParamDirection { get; set; }
        public string MSSqlParamDataType { get; set; }
        public string ParamSize { get; set; }
        public object ParamValue { get; set; }
    }

    [Serializable]
    public class CommandResult : ICommandResult
    {
        public string _CommandName;
        public DateTime _StartTime { get; set; } = DateTime.Now;
        public DateTime _EndTime { get;set; } = DateTime.Now;
        public bool _Successful { get; set; }
        public string _Response { get; set; }
        public CommandResultReturn _Return { get; set; }
        public long originalOrderNumber { get; set; }
        public string attemptedCommand { get; set; }
    }

    [Serializable]
    public class CommandResultReturn
    {
        public object value { get; set; }
        public CommandResultReturnTypes type { get; set; } = CommandResultReturnTypes._string;
    }

    public enum CommandResultReturnTypes
    {
        _string, _int, _long, _date, _bool

    }

    [Serializable]
    public class VirtualProcedureCall : IVirtualProcedureCall
    {
        public string ProcedureName { get; set; }
        public string ProcedureReturnType { get; set; }
        public List<ProcedureParameterStruct> ProcedureParams { get; set; }
    }

    [Serializable]
    public class PatchStruct
    {
        public string PatchName { get; set; }
        public bool installPatch { get; set; }
        public string PatchMethod { get; set; }

        public bool commitPatch { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }
    }

    [Serializable]
    public class TemplateStruct
    {
        public string PatchName { get; set; }
        public bool installTemplates { get; set; }
        public string PatchMethod { get; set; }
    }

    [Serializable]
    public class IdentitySearch
    {
        public string Search { get; set; }
        public string Where { get; set; }
        public Int64 StartingRow { get; set; }
        public Int64 LengthOfSet { get; set; }
        public string OrderBy { get; set; }
        public string Username { get; set; }
        public string Edipi { get; set; }
        public string Email { get; set; }
        public string Active { get; set; }
        public string Verified { get; set; }
        public string ObjectLayer { get; set; }
        public string Verify { get; set; }
    }

    [Serializable]
    public class IdentitySearchCount
    {
        public string Search { get; set; }
        public string Where { get; set; }
        public string Username { get; set; }
        public string Edipi { get; set; }
        public string Email { get; set; }
        public string Active { get; set; }
        public string Verified { get; set; }
        public string ObjectLayer { get; set; }
        public string Verify { get; set; }
    }
}

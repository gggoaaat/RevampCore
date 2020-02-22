using System.Collections.Generic;

namespace Revamp.IO.Structs
{
    public interface IVirtualProcedureCall
    {
        string ProcedureName { get; set; }
        List<ProcedureParameterStruct> ProcedureParams { get; set; }
        string ProcedureReturnType { get; set; }
    }
}
using System.Collections.Generic;
using System.Data;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;

namespace Revamp.IO.DB.Binds.IO.Dynamic
{
    public interface I_DynamicOutputProcedures
    {
        DataTable DynoProcSearch(IConnectToDB _Connect, string QueryType, string ProcedureName, DataTableDotNetModelMetaData Meta, List<DynamicModels.RootReportFilter> _Filters);
        long DynoProcSearchCount(IConnectToDB _Connect, string ProcedureName, DataTableDotNetModelMetaData Meta, List<DynamicModels.RootReportFilter> _Filters);        
        DataTable GetBaseReportsSearch(IConnectToDB _Connect, DynamicModels.Output.Root_Reports_Search thisModel);
        
        DataTable GetPortletsSearch(IConnectToDB _Connect, DynamicModels.Output.Portlet_Search thisModel);
        DataTable GetPortletsSearchContainers(IConnectToDB _Connect, DynamicModels.Output.Portlet_Containers_Search thisModel);                
        DataTable GetTableStruct(IConnectToDB _Connect, string ProcedureName);
        DataTable GetTableStruct(IConnectToDB _Connect, string ProcedureName, string DynoCol);        
        DataTable GetPortletPrivilegesSearch(IConnectToDB _Connect, long? base_portlet_id);
    }
}
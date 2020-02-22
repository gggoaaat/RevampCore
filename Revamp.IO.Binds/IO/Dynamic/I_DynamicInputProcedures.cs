using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using System.Collections.Generic;

namespace Revamp.IO.DB.Binds.IO.Dynamic
{
    public interface I_DynamicInputProcedures
    {

        DynamicModels.Input.Delete_Portlet_Container_Custom_Report DeleteContainerReport(IConnectToDB _Connect, DynamicModels.Input.Delete_Portlet_Container_Custom_Report thisModel);                
        DynamicModels.Input.Root_Report_Filter InsertRootReportFilter(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Filter thisModel, bool GetSQL);
        DynamicModels.Input.Root_Report InsertRootReport(IConnectToDB _Connect, DynamicModels.Input.Root_Report thisModel);
        DynamicModels.Input.Root_Report_Column InsertRootReportColumns(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Column thisModel);
        DynamicModels.Input.Portlet_Container_Custom_Reports InsertPortletContainerCustomReport(IConnectToDB _Connect, DynamicModels.Input.Portlet_Container_Custom_Reports thisModel);
        DynamicModels.Input.Portlet_ InsertPortlet(IConnectToDB _Connect, DynamicModels.Input.Portlet_ thisModel);
        DynamicModels.Input.Portlet_Container InsertPortletContainer(IConnectToDB _Connect, DynamicModels.Input.Portlet_Container thisModel);
        DynamicModels.Input.Custom_Reports InsertCustomReport(IConnectToDB _Connect, DynamicModels.Input.Custom_Reports thisModel, AuditDB AuditCommand);
        DynamicModels.Input.Custom_Report_Column InsertCustomReportColumns(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Column thisModel);
        DynamicModels.Input.Custom_Report_Filter InsertCustomReportFilter(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Filter thisModel);

       // List<DynamicModels.Input.Custom_Reports> LoadCustomReports(IConnectToDB _Connect, _IRISSessionModels _SessionModel, List<DynamicModels.Input.Custom_Reports> Reports, AuditDB AuditCommand = null);
      //  DynamicModels.Input.Custom_Reports LoadCustomReport(IConnectToDB _Connect, _IRISSessionModels _SessionModel, DynamicModels.Input.Custom_Reports Report, AuditDB AuditCommand = null);
        bool SetPortletPrivileges(IConnectToDB _Connect, DynamicModels.Output.PortletPrivileges thisModel);

    }
}
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Helpers.Helpers
{
    public class WorkflowHelper
    {
        public static flowObject GetWorkflowIfExists(IConnectToDB _Connect, Guid? applications_uuid, string Column)
        {
            flowObject theWorkflow = null;
            List<DynamicModels.RootReportFilter> thisTempFilters = new List<DynamicModels.RootReportFilter>();

            thisTempFilters.Add(new DynamicModels.RootReportFilter { FilterName = "GET_LATEST", ParamValue = "T" });
            thisTempFilters.Add(new DynamicModels.RootReportFilter { FilterName = Column + "_", ParamValue = applications_uuid });

            DataTable AppWorkflow = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__WORKFLOWS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 desc", start = 0, verify = "T" }, thisTempFilters);

            if (AppWorkflow.Rows.Count > 0)
            {
                byte[] thisWorkflowBytes = AppWorkflow.Rows[0].Field<Byte[]>("WORKFLOW");

                theWorkflow = Revamp.IO.Tools.Box.FromByteArray<flowObject>(thisWorkflowBytes);
                theWorkflow.workflows_uuid = AppWorkflow.Rows[0].Field<Guid?>("workflows_uuid");
                theWorkflow.base_workflows_uuid = AppWorkflow.Rows[0].Field<Guid?>("base_workflows_uuid");
            }

            return theWorkflow;
        }
    }
}

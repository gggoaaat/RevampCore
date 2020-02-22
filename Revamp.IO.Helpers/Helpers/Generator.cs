using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.Structs.Models;

namespace Revamp.IO.Helpers.Helpers
{
    public class Generator
    {
        public DataTable GetAllStageObjects(IConnectToDB _Connect, string Stage)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GRIP_TYPE_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Stage });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

    }


}
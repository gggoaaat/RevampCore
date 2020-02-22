using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.Structs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Revamp.IO.Helpers.Helpers
{
    public class ActivityHelper
    {

        public DataTable FindAllbyID(IConnectToDB _Connect, string _typeofActivity, int? _forIdentityID)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            switch (_typeofActivity.ToLower())
            {
                case "all":
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _forIdentityID });
                    break;
                default:
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _forIdentityID });
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = _typeofActivity });
                    break;
            }

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ACTIVITY_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED desc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindAllbyIDforDays(IConnectToDB _Connect, string _typeofActivity, long? _forIdentityID, int DaysBack)
        {
            ER_Query er_query = new ER_Query();
            DataTable TempDataTable;
            List<DBParameters> _dbParameters = new List<DBParameters>();
            string query = "";
            long? Identity = _forIdentityID != null ? _forIdentityID : 0;

            _dbParameters.Add(new DBParameters { ParamName = "DAYS_BACK", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = DaysBack });
            _dbParameters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = Identity });

            switch (_typeofActivity.ToLower())
            {
                case "all":
                    query = "Select * from CSA.VW__ACTIVITY where DT_CREATED > DATEADD(DAY,  -@DAYS_BACK, GETDATE()) and identities_id = @IDENTITIES_ID Order by DT_CREATED desc";
                    break;
                default:
                    _dbParameters.Add(new DBParameters { ParamName = "OBJECT_TYPE", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _typeofActivity.ToLower() });
                    query = "Select * from CSA.VW__ACTIVITY where DT_CREATED > DATEADD(DAY,  -@DAYS_BACK, GETDATE()) and Lower(object_type) = @OBJECT_TYPE and identities_id = @IDENTITIES_ID Order by DT_CREATED desc";
                    break;
            }

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = query,
                _dbParameters = _dbParameters
            };

            TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            return TempDataTable;
        }

        public DataTable FindAll(IConnectToDB _Connect, string _typeofActivity)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            switch (_typeofActivity.ToLower())
            {
                case "all":
                    break;
                default:
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = _typeofActivity });
                    break;
            }

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ACTIVITY_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED desc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable Find(IConnectToDB _Connect, string _id, string type)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            switch (type.ToLower())
            {
                case "cores":
                case "core":
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = _id });
                    break;
                case "coreactivity":
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "add object" });
                    //Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = type });
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "TABLE_SOURCE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "applications" });
                    break;
            }

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ACTIVITY_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindNotifications(IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_ACTIVITY_NOTIFICATIONS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED desc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindMemberNotifications(IConnectToDB _Connect, string identity)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = identity }
            };

            SQlin.sqlIn = "Select top 10 * from CSA.VW__ACTIVITY a where Lower(object_type) in ('add object','delete object') and Lower(table_source) in ('identities', 'applications') and a.CORES_ID in (Select c.CORES_ID from CSA.CORES_IDENTITIES c where c.IDENTITIES_ID = @IDENTITIES_ID) Order by DT_CREATED desc";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            return TempDataTable;
        }

        public DataTable FindNotifications(IConnectToDB _Connect, string start)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "ACTIVITY_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = start }
            };

            SQlin.sqlIn = " Select * " +
         " from (Select ROW_NUMBER() OVER(ORDER BY ACTIVITY_ID ) AS rownumb, * from CSA.VW__ACTIVITY where Lower(object_type) in ('add object','delete object') and " +
         "Lower(table_source) in ('identities', 'applications')) ok " +
         " where  rownumb between CONVERT(int, (select rownumb1 from (Select ROW_NUMBER() OVER(ORDER BY ACTIVITY_ID ) AS rownumb1, * from CSA.VW__ACTIVITY where Lower(object_type) in ('add object','delete object') and " +
         " Lower(table_source) in ('identities', 'applications')) ok1 where activity_id = @ACTIVITY_ID)) + 1 and CONVERT(int, (select rownumb1 from (Select ROW_NUMBER() OVER(ORDER BY ACTIVITY_ID ) AS rownumb1, * from CSA.VW__ACTIVITY where Lower(object_type) in ('add object','delete object') and " +
         " Lower(table_source) in ('identities', 'applications')) ok1 where activity_id = @ACTIVITY_ID)) + 10 and Lower(object_type) in ('add object','delete object') and " +
         " Lower(table_source) in ('identities', 'applications')  Order by DT_CREATED desc";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            return TempDataTable;
        }

        //Returns the count of the new notifications
        public string NewNotificationCount(IConnectToDB _Connect, string _identity)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();
            string count = "";

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _identity },
            };

            SQlin.sqlIn = " Select COUNT(*) total from CSA.VW__ACTIVITY a where a.APPLICATION_NAME != 'Core Settings' and a.DT_CREATED >= (select max(ip.DT_CREATED) FROM CSA.IDENTITY_PROPERTIES ip where ip.IDENTITIES_ID = @IDENTITIES_ID) and a.OBJECT_TYPE not in ('Login','Access Log')";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            if (TempDataTable.Rows.Count > 0)
            {
                count = TempDataTable.Rows[0][0].ToString();
            }

            return count;
        }

        public DataTable FindPastWeek(IConnectToDB _Connect, string _typeofActivity)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            switch (_typeofActivity.ToLower())
            {
                case "all":
                    break;
                default:
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = _typeofActivity });
                    break;
            }

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_ACTIVITY_PAST_WEEK_SEARCH",
                                   new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED desc", start = 0, verify = "T" },
                                   Filters);

            return TempDataTable;
        }

        public ActivityModelView ActivityRow(ActivityModelView AMV, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            AMV = JsonConvert.DeserializeObject<ActivityModelView>(thisAppRow);

            return AMV;
        }

        public List<ActivityModelView> ActivityList(List<ActivityModelView> AMVL, DataTable ActivityDT)
        {
            foreach (DataRow _ActivityRow in ActivityDT.Rows)
            {
                AMVL.Add(ActivityRow(new ActivityModelView(), _ActivityRow));
            }

            return AMVL;
        }

        public ApplicationActivity AppActivity(ApplicationActivity AMV, DataRow _ActivityRow)
        {
            AMV.count = _ActivityRow.Field<int?>("count");
            AMV.DT_created = _ActivityRow.Field<DateTime>("DT_created");

            return AMV;
        }

        public List<ApplicationActivity> ActivityCounts(List<ApplicationActivity> AA, DataTable ActivityDT)
        {
            foreach (DataRow _ActivityRow in ActivityDT.Rows)
            {
                AA.Add(AppActivity(new ApplicationActivity(), _ActivityRow));
            }

            return AA;
        }

    }
}
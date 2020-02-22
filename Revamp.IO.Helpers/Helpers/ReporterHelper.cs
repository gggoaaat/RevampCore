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
using Revamp.IO.Structs;

namespace Revamp.IO.Helpers.Helpers
{
    public class ReporterHelper
    {

        public DataTable GetActivityCounts(IConnectToDB _Connect, DataTable _DataTable, long? applications_id, DateTime? dateFrom, DateTime? dateTo)
        {
            ER_Query er_query = new ER_Query();
            ActivityHelper AH = new ActivityHelper();

            if (applications_id != null)
            {
                string query = "";

                List<DBParameters> _dbParameters = new List<DBParameters>();
                _dbParameters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = applications_id });

                if (dateFrom == null && dateTo == null)
                {
                    query = "SELECT count(*) as count, dateadd(DAY, 0, datediff(DAY, 0, a.dt_created)) as dt_created from CSA.ACTIVITY a inner join CSA.Applications aa ON (a.applications_id = aa.applications_id) inner join CSA.Applications aaa ON (aa.ROOT_APPLICATION = aaa.ROOT_APPLICATION and aaa.applications_id = @APPLICATIONS_ID)  where a.DT_CREATED > DATEADD(DAY, -51, GETDATE()) group by dateadd(DAY, 0, datediff(DAY, 0, a.dt_created))";
                }
                else if (dateFrom != null && dateTo == null)
                {
                    query = "SELECT count(*) as count, dateadd(DAY, 0, datediff(DAY, 0, a.dt_created)) as dt_created from CSA.ACTIVITY a inner join CSA.Applications aa ON (a.applications_id = aa.applications_id) inner join CSA.Applications aaa ON (aa.ROOT_APPLICATION = aaa.ROOT_APPLICATION and aaa.applications_id = @APPLICATIONS_ID) where a.DT_CREATED > DATEADD(DAY, -30, GETDATE()) group by dateadd(DAY, 0, datediff(DAY, 0, a.dt_created))";
                }
                else if (dateFrom == null && dateTo != null)
                {
                    query = "SELECT count(*) as count, dateadd(DAY, 0, datediff(DAY, 0, a.dt_created)) as dt_created from CSA.ACTIVITY a inner join CSA.Applications aa ON (a.applications_id = aa.applications_id) inner join CSA.Applications aaa ON (aa.ROOT_APPLICATION = aaa.ROOT_APPLICATION and aaa.applications_id = @APPLICATIONS_ID) where a.DT_CREATED > DATEADD(DAY, -30, GETDATE()) group by  dateadd(DAY, 0, datediff(DAY, 0, a.dt_created))";
                }
                else if (dateFrom != null && dateTo != null)
                {
                    query = "SELECT count(*) as count, dateadd(DAY, 0, datediff(DAY, 0, a.dt_created)) as dt_created from CSA.ACTIVITY a inner join CSA.Applications aa ON (a.applications_id = aa.applications_id) inner join CSA.Applications aaa ON (aa.ROOT_APPLICATION = aaa.ROOT_APPLICATION and aaa.applications_id = @APPLICATIONS_ID) where a.DT_CREATED >=  dateadd(DAY, -1, @START_DATE) and a.DT_CREATED <= dateadd(DAY, 1, @END_DATE) group by dateadd(DAY, 0, datediff(DAY, 0, a.dt_created))";
                    _dbParameters.Add(new DBParameters { ParamName = "START_DATE", MSSqlParamDataType = SqlDbType.DateTime, ParamValue = dateFrom.Value.ToShortDateString() });
                    _dbParameters.Add(new DBParameters { ParamName = "END_DATE", MSSqlParamDataType = SqlDbType.DateTime, ParamValue = dateTo.Value.ToShortDateString() });
                }

                ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                {
                    sqlIn = query,
                    _dbParameters = _dbParameters
                };

                _DataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);
            }

            return _DataTable;
        }
    }
}
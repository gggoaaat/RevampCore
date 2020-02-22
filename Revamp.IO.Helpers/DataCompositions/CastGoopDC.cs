using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using System.Collections.Generic;
using System.Data;

namespace Revamp.IO.Helpers.DataCompositions
{
    public class CastGoopDC : DataCompositionHelper
    {
        public override int TemplateOrdinal { get { return 3; } }

        public override DynamicModels.ReportDefinitions ReportMapper(IConnectToDB _Connection, DynamicModels.ReportDefinitions thisDefinition)
        {
            _DynamicOutputProcedures CoreOut = new _DynamicOutputProcedures();

            DynamicModels.RootReport CurrentReportSelected = new DynamicModels.RootReport();

            CurrentReportSelected.ReportName = thisDefinition.SearchReport;

            DataTable Result_Set = ER_Query._RUN_PARAMETER_QUERY(_Connection, new ER_Query.Parameter_Run
            {
                _dbParameters = new List<DBParameters>
                    {
                        new DBParameters {  ParamName = "P_TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = thisDefinition.SearchReport },
                        new DBParameters {ParamName = "P_SCHEMA", MSSqlParamDataType = SqlDbType.VarChar,  ParamValue = _Connection.Schema },
                        new DBParameters {ParamName = "P_PROC", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = "SP_S" + thisDefinition.SearchReport + "_SEARCH"},
                        new DBParameters {ParamName = "P_REPORT_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = thisDefinition.SearchReport}
                    },
                sqlIn = "select 0 rownumb," +
                        "0 ROOT_REPORT_ID," +
                        "0 BASE_ROOT_REPORT_ID," +
                        "0 PREV_ROOT_REPORT_ID," +
                        "'Y' ENABLED," +
                        "getDate() DT_CREATED," +
                        "getDate() DT_UPDATE," +
                        " CAST(0 AS bigint) TEMPLATE_ID," +
                        "'Tempt' TEMPLATE," +
                        "@P_REPORT_NAME REPORT_NAME," +
                        "@P_PROC PROCEDURE_NAME," +
                        " CAST(0 AS bigint) ROOT_REPORT_FILTER_ID," +
                        " CAST(0 AS bigint) BASE_ROOT_REPORT_FILTER_ID," +
                        " CAST(0 AS bigint) PREV_ROOT_REPORT_FILTER_ID," +
                        "'N' REQUIRED," +
                        "COLUMN_NAME FILTER_NAME," +
                        "COLUMN_NAME PRETTY_NAME," +
                        "'MultiSelect' FILTER_TYPE," +
                        "'T' IN_PICK_LIST," +
                        "CASE " +
                        "   WHEN a.DATA_TYPE = 'int' then 'Int' " +
                        "   WHEN a.DATA_TYPE = 'bigint' then 'BigInt' " +
                        "   WHEN a.DATA_TYPE = 'decimal' then 'Decimal'" +
                        "   WHEN a.DATA_TYPE = 'varchar' then 'VarChar' " +
                        "   WHEN a.DATA_TYPE = 'char' then 'Char' " +
                        "   WHEN a.DATA_TYPE = 'varbinary' then 'VarBinary' " +
                        "   WHEN a.DATA_TYPE in ('datetime') then 'DateTime'" +
                        "   WHEN a.DATA_TYPE in ('datetime2') then 'DateTime2'" +
                        "   WHEN a.DATA_TYPE in ('smalldatetime') then 'SmallDateTime'" +
                        "   WHEN a.DATA_TYPE in ('text') then 'SmallDateTime'" +
                        "   WHEN a.DATA_TYPE in ('bit') then 'Bit'" +
                        "   WHEN a.DATA_TYPE = 'NVarChar' then 'VarChar'" +
                        "   WHEN a.DATA_TYPE in ('date') then 'Date'" +
                        "   WHEN a.DATA_TYPE in ('money', 'numeric') then 'Decimal'" +
                        "   WHEN a.DATA_TYPE in ('uniqueidentifier') then 'UniqueIdentifier'" +
                        "   else a.DATA_TYPE END  DB_TYPE," +
                        "CAST(isNull(a.CHARACTER_MAXIMUM_LENGTH,0) AS bigint) PARAM_SIZE," +
                        "'VarChar' SEARCH_DB_TYPE," +
                        "CAST(-1 AS bigint)  SEARCH_DB_SIZE," +
                        "'' ParamValue," +
                        "'DISTINCT ' + a.COLUMN_NAME  FILTER_SELECT," +
                        "'' ALTERNATE_REPORT_NAME," +
                        "'' ALTERNATE_SCHEMA," +
                        "''  ALTERNATE_SOURCE," +
                        "''  ALTERNATE_TEMPLATE from INFORMATION_SCHEMA.COLUMNS a " +
                        "where TABLE_SCHEMA = @P_SCHEMA and upper(TABLE_NAME) = upper(@P_TABLE_NAME)"
            });

            // DynamicModels.CoreReport thisReport = thisDef.theReport;
            DynamicModels.RootReport thisReport = UniversalHelper.PopulateReport(_Connection, new DynamicModels.RootReport(), CurrentReportSelected.ReportName, Result_Set);

            thisDefinition.theReport = thisReport;
            thisDefinition.theReport.ProcedureName = "SP_S_" + thisDefinition.SearchReport + "_SEARCH";

            Revamp.IO.Helpers.DataCompositions.UniversalHelper.GetTableStructure(_Connection, thisDefinition, CoreOut, thisDefinition.theReport);

            return thisDefinition;
        }

        public override List<DynamicModels.RootReport> Reports
        {
            get
            {
                List<DynamicModels.RootReport> CoreReports = new List<DynamicModels.RootReport>();

                return CoreReports;
            }
        }

    }
}

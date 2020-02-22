using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using Revamp_IO_DB_Bridge;
using Revamp_IO_DB_Proc_Binds.IO.Dynamic;
using Revamp_IO_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Revamp_IO_Helpers.Helpers
{
    public class DynamicHelper
    {
        public static DynamicModels.ReportDefinitions ReportMapper(IConnectToDB _Connection, DynamicModels.ReportDefinitions thisDefinition)
        {
            _DynamicOutputProcedures CoreOut = new _DynamicOutputProcedures();

            DynamicModels.RootReport CurrentReportSelected = new DynamicModels.RootReport();

            switch (thisDefinition.SearchReport.ToLower())
            {
                default:
                    CurrentReportSelected = new DynamicModels.RootReport();
                    break;
                case "custom reports":
                    CurrentReportSelected = DynamicHelper.CustomReports(new DynamicModels.RootReport());
                    break;
                case "custom report order":
                    CurrentReportSelected = DynamicHelper.CustomReportOrder(new DynamicModels.RootReport());
                    break;
                case "custom report columns":
                    CurrentReportSelected = DynamicHelper.CustomReportColumns(new DynamicModels.RootReport());
                    break;
                case "custom report filters":
                    CurrentReportSelected = DynamicHelper.CustomReportFilters(new DynamicModels.RootReport());
                    break;
                case "custom reports utilized":
                    CurrentReportSelected = DynamicHelper.CustomReportUtilized(new DynamicModels.RootReport());
                    break;
                case "root reports":
                    CurrentReportSelected = DynamicHelper.RootReportsSearch(new DynamicModels.RootReport());
                    break;
                case "root report column order":
                    CurrentReportSelected = DynamicHelper.RootReportColumnOrder(new DynamicModels.RootReport());
                    break;
                case "root reports and filters":
                    CurrentReportSelected = DynamicHelper.RootReportsAndFilters(new DynamicModels.RootReport());
                    break;
                case "root report columns":
                    CurrentReportSelected = DynamicHelper.RootReportColumns(new DynamicModels.RootReport());
                    break;
                case "custom report permissions":
                    CurrentReportSelected = DynamicHelper.CustomReportPermissions(new DynamicModels.RootReport());
                    break;
            }

            UniversalHelper.GetTableStructure(_Connection, thisDefinition, CoreOut, CurrentReportSelected);

            return thisDefinition;
        }

        public static void CustomReportLogic(DynamicModels.ReportLogicModel logicModel, HttpRequestBase Request)
        {
            //Combo Conditions
            switch (logicModel.CurrentReportSelected.ReportName.ToLower())
            {
                default:
                    break;
            }

            //Iterative Conditions
            foreach (DynamicModels.RootReportFilter _thisReport in logicModel.CurrentReportSelected.ReportFilters)
            {
                bool runElseLogic = false;

                switch (logicModel.CurrentReportSelected.ReportName.ToLower())
                {

                    default:
                        runElseLogic = true;

                        break;


                }

                if (runElseLogic)
                {
                    var P_THIS_FILTERNAME = logicModel.useQsCol ? logicModel.qscol.Get("P_" + _thisReport.FilterName) : Request.Params["P_" + _thisReport.FilterName];

                    if (P_THIS_FILTERNAME != null)
                    {
                        _thisReport.DBType = _thisReport.SearchDBType;
                        _thisReport.ParamSize = _thisReport.SearchParamSize;
                        _thisReport.ParamValue = P_THIS_FILTERNAME.ToString();
                        logicModel.UsedFilters.Add(_thisReport);
                    }

                    var P_THIS_EXCLUDE_FILTERNAME = logicModel.useQsCol ? logicModel.qscol.Get("P_EXCLUDE_" + _thisReport.FilterName) : Request.Params["P_EXCLUDE_" + _thisReport.FilterName];

                    if (P_THIS_EXCLUDE_FILTERNAME != null)
                    {
                        _thisReport.DBType = _thisReport.SearchDBType;
                        _thisReport.ParamSize = _thisReport.SearchParamSize;
                        _thisReport.ParamValue = P_THIS_EXCLUDE_FILTERNAME.ToString();
                        _thisReport.FilterName = "EXCLUDE_" + _thisReport.FilterName;
                        logicModel.UsedFilters.Add(_thisReport);
                    }
                }

            }
        }

        public static List<DynamicModels.RootReport> Dynamic_Reports()
        {
            List<DynamicModels.RootReport> CoreReports = new List<DynamicModels.RootReport>();

            CoreReports.Add(CustomReports(new DynamicModels.RootReport()));
            CoreReports.Add(CustomReportOrder(new DynamicModels.RootReport()));
            CoreReports.Add(CustomReportColumns(new DynamicModels.RootReport()));
            CoreReports.Add(CustomReportFilters(new DynamicModels.RootReport()));
            CoreReports.Add(CustomReportUtilized(new DynamicModels.RootReport()));
            CoreReports.Add(CustomReportPermissions(new DynamicModels.RootReport()));
            CoreReports.Add(RootReportsSearch(new DynamicModels.RootReport()));
            CoreReports.Add(RootReportColumnOrder(new DynamicModels.RootReport()));
            CoreReports.Add(RootReportsAndFilters(new DynamicModels.RootReport()));
            CoreReports.Add(RootReportColumns(new DynamicModels.RootReport()));


            return CoreReports;
        }


        public static DynamicModels.RootReport CustomReports(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Custom Reports";
            thisCoreReport.Source = "VIEW_ROOT_REPORTS_AND_CUSTOM_REPORTS";
            thisCoreReport.ProcedureName = "SP_S_VIEW_ROOT_REPORTS_AND_CUSTOM_REPORTS_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();

            #region Columns for Filters
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CUSTOM_REPORT_ID", PrettyName = "CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_CUSTOM_REPORT_ID", PrettyName = "BASE_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_CUSTOM_REPORT_ID", PrettyName = "PREV_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", PrettyName = "ENABLED", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ENABLED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_UPDATED", PrettyName = "DT_UPDATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_UPDATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITY_ID", PrettyName = "IDENTITY_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT IDENTITY_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_ID", PrettyName = "ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_ROOT_REPORT_ID", PrettyName = "BASE_ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "REPORT_NAME", PrettyName = "REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DESCRIPTION", PrettyName = "DESCRIPTION", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DESCRIPTION" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_REPORT_NAME", PrettyName = "BASE_REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATE_ID", PrettyName = "TEMPLATE_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATE_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PROCEDURE_NAME", PrettyName = "PROCEDURE_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PROCEDURE_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATE", PrettyName = "TEMPLATE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATE" });
            #endregion
            #endregion
            return thisCoreReport;
        }

        public static DynamicModels.RootReport CustomReportColumns(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Custom Report Columns";
            thisCoreReport.Source = "CUSTOM_REPORT_COLUMNS";
            thisCoreReport.ProcedureName = "SP_S_CUSTOM_REPORT_COLUMNS_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();


            #region Columns for Custom Report
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CUSTOM_REPORT_COLUMN_ID", PrettyName = "CUSTOM_REPORT_COLUMN_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CUSTOM_REPORT_COLUMN_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_CUSTOM_REPORT_COLUMN_ID", PrettyName = "BASE_CUSTOM_REPORT_COLUMN_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_CUSTOM_REPORT_COLUMN_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_CUSTOM_REPORT_COLUMN_ID", PrettyName = "PREV_CUSTOM_REPORT_COLUMN_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_CUSTOM_REPORT_COLUMN_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", PrettyName = "ENABLED", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ENABLED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_UPDATED", PrettyName = "DT_UPDATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_UPDATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_ID", PrettyName = "ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_REPORT_NAME", PrettyName = "BASE_REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CUSTOM_REPORT_ID", PrettyName = "CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "COLUMN_AREA", PrettyName = "COLUMN_AREA", DBType = System.Data.SqlDbType.VarChar, ParamSize = -1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT COLUMN_AREA" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALIAS_AREA", PrettyName = "ALIAS_AREA", DBType = System.Data.SqlDbType.VarChar, ParamSize = -1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALIAS_AREA" });
            #endregion
            #endregion
            return thisCoreReport;
        }

        public static DynamicModels.RootReport CustomReportOrder(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Custom Report Order";
            thisCoreReport.Source = "VIEW_CUSTOM_REPORT_ORDER_SETS";
            thisCoreReport.ProcedureName = "SP_S_VIEW_CUSTOM_REPORT_ORDER_SETS_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();


            #region Columns for Filters
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CUSTOM_REPORTS_ORDER_SET_ID", PrettyName = "CUSTOM_REPORTS_ORDER_SET_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CUSTOM_REPORTS_ORDER_SET_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_CUSTOM_REPORTS_ORDER_SET_ID", PrettyName = "BASE_CUSTOM_REPORTS_ORDER_SET_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_CUSTOM_REPORTS_ORDER_SET_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_CUSTOM_REPORTS_ORDER_SET_ID", PrettyName = "PREV_CUSTOM_REPORTS_ORDER_SET_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_CUSTOM_REPORTS_ORDER_SET_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CUSTOM_REPORT_ID", PrettyName = "CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SORT_COLUMN", PrettyName = "SORT_COLUMN", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SORT_COLUMN" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SORT_DIRECTION", PrettyName = "SORT_DIRECTION", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SORT_DIRECTION" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SORT_ORDER", PrettyName = "SORT_ORDER", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SORT_ORDER" });
            #endregion
            #endregion
            return thisCoreReport;
        }

        public static DynamicModels.RootReport CustomReportFilters(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Custom Report Filters";
            thisCoreReport.Source = "VIEW_CUSTOM_REPORTS_AND_FILTERS";
            thisCoreReport.ProcedureName = "SP_S_VIEW_CUSTOM_REPORTS_AND_FILTERS_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();


            #region Columns for Filters
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "FILTER_ID", PrettyName = "FILTER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT FILTER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_FILTER_ID", PrettyName = "BASE_FILTER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_FILTER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_FILTER_ID", PrettyName = "PREV_FILTER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_FILTER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", PrettyName = "ENABLED", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ENABLED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_UPDATED", PrettyName = "DT_UPDATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_UPDATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_REPORT_NAME", PrettyName = "BASE_REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "REPORT_NAME", PrettyName = "REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CUSTOM_REPORT_ID", PrettyName = "CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_ID", PrettyName = "ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "FILTER_NAME", PrettyName = "FILTER_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT FILTER_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "FILTER_ALIAS", PrettyName = "FILTER_ALIAS", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT FILTER_ALIAS" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PRETTY_NAME", PrettyName = "PRETTY_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PRETTY_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "FILTER_TYPE", PrettyName = "FILTER_TYPE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT FILTER_TYPE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IN_PICK_LIST", PrettyName = "IN_PICK_LIST", DBType = System.Data.SqlDbType.VarChar, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT IN_PICK_LIST" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DB_TYPE", PrettyName = "DB_TYPE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DB_TYPE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PARAM_SIZE", PrettyName = "PARAM_SIZE", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PARAM_SIZE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SEARCH_DB_TYPE", PrettyName = "SEARCH_DB_TYPE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SEARCH_DB_TYPE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SEARCH_DB_SIZE", PrettyName = "SEARCH_DB_SIZE", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SEARCH_DB_SIZE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ParamValue", PrettyName = "ParamValue", DBType = System.Data.SqlDbType.VarChar, ParamSize = -1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ParamValue" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "FILTER_SELECT", PrettyName = "FILTER_SELECT", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT FILTER_SELECT" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "MANDATORY", PrettyName = "MANDATORY", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT MANDATORY" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "FILTER_ORDER", PrettyName = "FILTER_ORDER", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT FILTER_ORDER" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "REQUIRED", PrettyName = "REQUIRED", DBType = System.Data.SqlDbType.VarChar, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT REQUIRED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALTERNATE_REPORT_NAME", PrettyName = "ALTERNATE_REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALTERNATE_REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALTERNATE_SCHEMA", PrettyName = "ALTERNATE_SCHEMA", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALTERNATE_SCHEMA" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALTERNATE_SOURCE", PrettyName = "ALTERNATE_SOURCE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALTERNATE_SOURCE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALTERNATE_TEMPLATE", PrettyName = "ALTERNATE_TEMPLATE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALTERNATE_TEMPLATE" });
            #endregion
            #endregion
            return thisCoreReport;
        }

        public static DynamicModels.RootReport CustomReportUtilized(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Custom Reports Utilized";
            thisCoreReport.Source = "VIEW_CUSTOM_REPORTS_UTILIZED";
            thisCoreReport.ProcedureName = "SP_S_VIEW_CUSTOM_REPORTS_UTILIZED_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();


            #region Columns for Filters
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PORTLET_CONTAINER_CUSTOM_REPORT_ID", PrettyName = "PORTLET_CONTAINER_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PORTLET_CONTAINER_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID", PrettyName = "BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID", PrettyName = "PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", PrettyName = "ENABLED", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ENABLED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_UPDATED", PrettyName = "DT_UPDATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_UPDATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITY_ID", PrettyName = "IDENTITY_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT IDENTITY_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATION_ID", PrettyName = "APPLICATION_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT APPLICATION_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PORTLET_ID", PrettyName = "PORTLET_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PORTLET_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_PORTLET_ID", PrettyName = "BASE_PORTLET_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_PORTLET_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_PORTLET_ID", PrettyName = "PREV_PORTLET_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_PORTLET_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PORTLET_TITLE", PrettyName = "PORTLET_TITLE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PORTLET_TITLE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CONTAINER_ID", PrettyName = "CONTAINER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CONTAINER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_CONTAINER_ID", PrettyName = "BASE_CONTAINER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_CONTAINER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_CONTAINER_ID", PrettyName = "PREV_CONTAINER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_CONTAINER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CONTAINER_TITLE", PrettyName = "CONTAINER_TITLE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CONTAINER_TITLE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_ID", PrettyName = "ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_ROOT_REPORT_ID", PrettyName = "BASE_ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_ROOT_REPORT_ID", PrettyName = "PREV_ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_REPORT_NAME", PrettyName = "BASE_REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATE_ID", PrettyName = "TEMPLATE_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATE_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATE_NAME", PrettyName = "TEMPLATE_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATE_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CUSTOM_REPORT_ID", PrettyName = "CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_CUSTOM_REPORT_ID", PrettyName = "BASE_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_CUSTOM_REPORT_ID", PrettyName = "PREV_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CUSTOM_REPORT_NAME", PrettyName = "CUSTOM_REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CUSTOM_REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TITLE", PrettyName = "TITLE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 255, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TITLE" });
            #endregion
            #endregion
            return thisCoreReport;
        }

        public static DynamicModels.RootReport RootReportsAndFilters(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Root Reports and Filters";
            thisCoreReport.Source = "VIEW_ROOT_REPORTS_AND_FILTERS";
            thisCoreReport.ProcedureName = "SP_S_VIEW_ROOT_REPORTS_AND_FILTERS_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();


            #region Columns for Filters
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_ID", PrettyName = "ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_ROOT_REPORT_ID", PrettyName = "BASE_ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_ROOT_REPORT_ID", PrettyName = "PREV_ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", PrettyName = "ENABLED", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ENABLED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_UPDATED", PrettyName = "DT_UPDATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_UPDATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATE_ID", PrettyName = "TEMPLATE_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATE_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATE", PrettyName = "TEMPLATE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "REPORT_NAME", PrettyName = "REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PROCEDURE_NAME", PrettyName = "PROCEDURE_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PROCEDURE_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_FILTER_ID", PrettyName = "ROOT_REPORT_FILTER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_FILTER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_ROOT_REPORT_FILTER_ID", PrettyName = "BASE_ROOT_REPORT_FILTER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_ROOT_REPORT_FILTER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_ROOT_REPORT_FILTER_ID", PrettyName = "PREV_ROOT_REPORT_FILTER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_ROOT_REPORT_FILTER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "REQUIRED", PrettyName = "REQUIRED", DBType = System.Data.SqlDbType.VarChar, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT REQUIRED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "FILTER_NAME", PrettyName = "FILTER_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT FILTER_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PRETTY_NAME", PrettyName = "PRETTY_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PRETTY_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "FILTER_TYPE", PrettyName = "FILTER_TYPE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT FILTER_TYPE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IN_PICK_LIST", PrettyName = "IN_PICK_LIST", DBType = System.Data.SqlDbType.VarChar, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT IN_PICK_LIST" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DB_TYPE", PrettyName = "DB_TYPE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DB_TYPE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PARAM_SIZE", PrettyName = "PARAM_SIZE", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PARAM_SIZE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SEARCH_DB_TYPE", PrettyName = "SEARCH_DB_TYPE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SEARCH_DB_TYPE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SEARCH_DB_SIZE", PrettyName = "SEARCH_DB_SIZE", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SEARCH_DB_SIZE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ParamValue", PrettyName = "ParamValue", DBType = System.Data.SqlDbType.VarChar, ParamSize = -1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ParamValue" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "FILTER_SELECT", PrettyName = "FILTER_SELECT", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT FILTER_SELECT" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALTERNATE_REPORT_NAME", PrettyName = "ALTERNATE_REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALTERNATE_REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALTERNATE_SCHEMA", PrettyName = "ALTERNATE_SCHEMA", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALTERNATE_SCHEMA" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALTERNATE_SOURCE", PrettyName = "ALTERNATE_SOURCE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALTERNATE_SOURCE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALTERNATE_TEMPLATE", PrettyName = "ALTERNATE_TEMPLATE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALTERNATE_TEMPLATE" });
            #endregion
            #endregion
            return thisCoreReport;
        }

        public static DynamicModels.RootReport RootReportsSearch(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Root Reports";
            thisCoreReport.Source = "VIEW_ROOT_REPORTS";
            thisCoreReport.ProcedureName = "SP_S_ROOT_REPORTS_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();

            #region Columns for Filters
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_ID", PrettyName = "ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_ROOT_REPORT_ID", PrettyName = "BASE_ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_ROOT_REPORT_ID", PrettyName = "PREV_ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", PrettyName = "ENABLED", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ENABLED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_UPDATED", PrettyName = "DT_UPDATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_UPDATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITY_ID", PrettyName = "IDENTITY_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT IDENTITY_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATE_ID", PrettyName = "TEMPLATE_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATE_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATE_NAME", PrettyName = "TEMPLATE_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATE_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "REPORT_NAME", PrettyName = "REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PROCEDURE_NAME", PrettyName = "PROCEDURE_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PROCEDURE_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "INSTALLED", PrettyName = "INSTALLED", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT INSTALLED" });

            #endregion
            #endregion
            return thisCoreReport;
        }

        public static DynamicModels.RootReport RootReportColumnOrder(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Root Report Column Order";
            thisCoreReport.Source = "VIEW_ROOT_REPORT_ORDER_SETS";
            thisCoreReport.ProcedureName = "SP_S_VIEW_ROOT_REPORT_ORDER_SETS_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();


            #region Columns for Filters
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORTS_ORDER_SET_ID", PrettyName = "ROOT_REPORTS_ORDER_SET_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORTS_ORDER_SET_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_ROOT_REPORTS_ORDER_SET_ID", PrettyName = "BASE_ROOT_REPORTS_ORDER_SET_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_ROOT_REPORTS_ORDER_SET_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_ROOT_REPORTS_ORDER_SET_ID", PrettyName = "PREV_ROOT_REPORTS_ORDER_SET_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_ROOT_REPORTS_ORDER_SET_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_ID", PrettyName = "ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SORT_COLUMN", PrettyName = "SORT_COLUMN", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SORT_COLUMN" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SORT_DIRECTION", PrettyName = "SORT_DIRECTION", DBType = System.Data.SqlDbType.VarChar, ParamSize = 250, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SORT_DIRECTION" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "SORT_ORDER", PrettyName = "SORT_ORDER", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT SORT_ORDER" });
            #endregion
            #endregion
            return thisCoreReport;
        }

        public static DynamicModels.RootReport RootReportColumns(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Root Report Columns";
            thisCoreReport.Source = "ROOT_REPORT_COLUMNS";
            thisCoreReport.ProcedureName = "SP_S_ROOT_REPORT_COLUMNS_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();


            #region Columns for Filters
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_COLUMN_ID", PrettyName = "ROOT_REPORT_COLUMN_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_COLUMN_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_ROOT_REPORT_COLUMN_ID", PrettyName = "BASE_ROOT_REPORT_COLUMN_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_ROOT_REPORT_COLUMN_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_ROOT_REPORT_COLUMN_ID", PrettyName = "PREV_ROOT_REPORT_COLUMN_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_ROOT_REPORT_COLUMN_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", PrettyName = "ENABLED", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ENABLED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_UPDATED", PrettyName = "DT_UPDATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_UPDATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITY_ID", PrettyName = "IDENTITY_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT IDENTITY_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROOT_REPORT_ID", PrettyName = "ROOT_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROOT_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ORIGINAL_COLUMN", PrettyName = "ORIGINAL_COLUMN", DBType = System.Data.SqlDbType.VarChar, ParamSize = -1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ORIGINAL_COLUMN" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "COLUMN_AREA", PrettyName = "COLUMN_AREA", DBType = System.Data.SqlDbType.VarChar, ParamSize = -1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT COLUMN_AREA" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ALIAS_AREA", PrettyName = "ALIAS_AREA", DBType = System.Data.SqlDbType.VarChar, ParamSize = -1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ALIAS_AREA" });
            #endregion
            #endregion
            return thisCoreReport;
        }


        public static DynamicModels.RootReport CustomReportPermissions(DynamicModels.RootReport thisCoreReport)
        {
            #region Report Root Reports and Filters
            thisCoreReport = new DynamicModels.RootReport();
            thisCoreReport.ReportName = "Custom Report Permissions";
            thisCoreReport.Source = "VIEW_CUSTOM_REPORT_PERMISSIONS";
            thisCoreReport.ProcedureName = "SP_S_VIEW_CUSTOM_REPORT_PERMISSIONS_SEARCH";
            thisCoreReport.ReportFilters = new List<DynamicModels.RootReportFilter>();


            #region Columns for Filters
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PORTLET_CONTAINER_CUSTOM_REPORT_ID", PrettyName = "PORTLET_CONTAINER_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PORTLET_CONTAINER_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID", PrettyName = "BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID", PrettyName = "PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", PrettyName = "ENABLED", DBType = System.Data.SqlDbType.Char, ParamSize = 1, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ENABLED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_CREATED", PrettyName = "DT_CREATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_CREATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "DT_UPDATED", PrettyName = "DT_UPDATED", DBType = System.Data.SqlDbType.DateTime, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT DT_UPDATED" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CONTAINER_ID", PrettyName = "CONTAINER_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CONTAINER_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TITLE", PrettyName = "TITLE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 255, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TITLE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATE", PrettyName = "TEMPLATE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "TEMPLATES_ID", PrettyName = "TEMPLATES_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT TEMPLATES_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATION_NAME", PrettyName = "APPLICATION_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT APPLICATION_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CUSTOM_REPORT_ID", PrettyName = "CUSTOM_REPORT_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT CUSTOM_REPORT_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "REPORT_NAME", PrettyName = "REPORT_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 100, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT REPORT_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "USERNAME", PrettyName = "USERNAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 75, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT USERNAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITY_ID", PrettyName = "IDENTITY_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT IDENTITY_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME", PrettyName = "PRIVILEGE_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 50, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT PRIVILEGE_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ASSIGNED_TO_PRIVILEGE", PrettyName = "ASSIGNED_TO_PRIVILEGE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 30, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ASSIGNED_TO_PRIVILEGE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ASSIGNED_TO_ROLE", PrettyName = "ASSIGNED_TO_ROLE", DBType = System.Data.SqlDbType.VarChar, ParamSize = 30, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ASSIGNED_TO_ROLE" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_ID", PrettyName = "ROLE_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROLE_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_NAME", PrettyName = "ROLE_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 25, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ROLE_NAME" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ASSIGNED_TO_GROUP", PrettyName = "ASSIGNED_TO_GROUP", DBType = System.Data.SqlDbType.VarChar, ParamSize = 30, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT ASSIGNED_TO_GROUP" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUP_ID", PrettyName = "GROUP_ID", DBType = System.Data.SqlDbType.BigInt, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT GROUP_ID" });
            thisCoreReport.ReportFilters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUP_NAME", PrettyName = "GROUP_NAME", DBType = System.Data.SqlDbType.VarChar, ParamSize = 50, SearchDBType = System.Data.SqlDbType.VarChar, SearchParamSize = -1, inPickList = true, FilterType = DynamicModels.FilterType.MultiSelect, FilterSelect = "DISTINCT GROUP_NAME" });
            #endregion
            #endregion
            return thisCoreReport;
        }
    }
}

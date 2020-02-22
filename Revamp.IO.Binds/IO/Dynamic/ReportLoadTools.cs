using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Revamp.IO.Structs;

namespace Revamp.IO.DB.Binds.IO.Dynamic
{
    public static class ReportLoadTools
    {
        public static List<DynamicModels.Input.Custom_Reports> LoadCustomReports(IConnectToDB _Connect, SessionObjects _SessionModel, List<DynamicModels.Input.Custom_Reports> Reports, AuditDB AuditCommand = null)
        {
            for (int i = 0; i < Reports.Count; i++)
            {
                //try
                //{
                Reports[i] = LoadCustomReport(_Connect, _SessionModel, Reports[i], AuditCommand);
                // }
                //catch (Exception e)
                //{
                //    Reports[i] = Reports[i];
                //}
            }

            return Reports;
        }

        public static DynamicModels.Input.Custom_Reports LoadCustomReport(IConnectToDB _Connect, SessionObjects _SessionModel, DynamicModels.Input.Custom_Reports Report, AuditDB AuditCommand = null)
        {
            _DynamicInputProcedures CoreIH = new _DynamicInputProcedures();

            DynamicModels.Input.Custom_Reports thisModel = Report;

            if (thisModel != null)
            {
                thisModel.I_BASE_CUSTOM_REPORT_ID = thisModel.I_BASE_CUSTOM_REPORT_ID == null ? 0 : thisModel.I_BASE_CUSTOM_REPORT_ID;
                thisModel.I_PREV_CUSTOM_REPORT_ID = thisModel.I_PREV_CUSTOM_REPORT_ID == null ? 0 : thisModel.I_PREV_CUSTOM_REPORT_ID;
                thisModel.I_ENABLED = thisModel.I_ENABLED == null ? 'Y' : thisModel.I_ENABLED;
                thisModel.I_IDENTITIES_ID = _SessionModel._IdentityModel.identities_id;
                thisModel = CoreIH.InsertCustomReport(_Connect, thisModel, AuditCommand);

                if (thisModel.Filters != null && thisModel.O_CUSTOM_REPORT_ID > 0)
                {
                    for (int i = 0; i < thisModel.Filters.Count; i++)
                    {
                        thisModel.Filters[i].I_CUSTOM_REPORT_ID = thisModel.O_CUSTOM_REPORT_ID;
                        thisModel.Filters[i].I_BASE_FILTER_ID = 0;
                        thisModel.Filters[i].I_PREV_FILTER_ID = 0;
                        thisModel.Filters[i].I_ENABLED = 'Y';
                        thisModel.Filters[i].I_IDENTITIES_ID = _SessionModel._IdentityModel.identities_id;
                        thisModel.Filters[i].I_FILTER_ORDER = i;
                        thisModel.Filters[i].I_FILTER_ALIAS = String.IsNullOrWhiteSpace(thisModel.Filters[i].I_FILTER_ALIAS) ? thisModel.Filters[i].I_FILTER : thisModel.Filters[i].I_FILTER_ALIAS;
                        thisModel.Filters[i] = CoreIH.InsertCustomReportFilter(_Connect, thisModel.Filters[i]);
                    }
                }

                if (thisModel.Columns != null && thisModel.O_CUSTOM_REPORT_ID > 0)
                {
                    for (int i = 0; i < thisModel.Columns.Count; i++)
                    {
                        thisModel.Columns[i].I_CUSTOM_REPORT_ID = thisModel.O_CUSTOM_REPORT_ID;
                        thisModel.Columns[i].I_BASE_CUSTOM_REPORT_COLUMN_ID = 0;
                        thisModel.Columns[i].I_PREV_CUSTOM_REPORT_COLUMN_ID = 0;
                        thisModel.Columns[i].I_ENABLED = 'Y';
                        thisModel.Columns[i].I_IDENTITIES_ID = _SessionModel._IdentityModel.identities_id;
                        thisModel.Columns[i] = CoreIH.InsertCustomReportColumns(_Connect, thisModel.Columns[i]);
                    }
                }
                else
                {
                    StringBuilder _sqlin = new StringBuilder();
                    //TODO: Get Columns from DB
                    if (thisModel.O_CUSTOM_REPORT_ID > 0)
                    {
                        _sqlin.AppendLine("BEGIN");
                        //string _Schema = TemplateName == "SYSTEM" ? "CSA" : TemplateName;

                        _sqlin.AppendLine("DECLARE @P_ROOT_REPORT_ID bigint = '" + thisModel.I_ROOT_REPORT_ID + "'");

                        _sqlin.AppendLine("SELECT ORIGINAL_COLUMN, ROOT_REPORT_COLUMN_ID");
                        _sqlin.AppendLine("FROM [DYNAMIC].[ROOT_REPORTS] a");
                        _sqlin.AppendLine("INNER JOIN [DYNAMIC].ROOT_REPORT_COLUMNS b on a.ROOT_REPORT_ID = b.ROOT_REPORT_ID");
                        _sqlin.AppendLine("WHERE a.ROOT_REPORT_ID = @P_ROOT_REPORT_ID");
                        _sqlin.AppendLine("ORDER BY ROOT_REPORT_COLUMN_ID ASC");
                        _sqlin.AppendLine("END ");

                        ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = _sqlin.ToString(),
                            _dbParameters = new List<DBParameters>()
                        };

                        DataTable ReportColumnns = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
                        DataColumnCollection DCC = ReportColumnns.Columns;

                        for (int i = 0; i < ReportColumnns.Rows.Count; i++)
                        {
                            DynamicModels.Input.Custom_Report_Column thisColumn = new DynamicModels.Input.Custom_Report_Column();

                            thisColumn.I_CUSTOM_REPORT_ID = thisModel.O_CUSTOM_REPORT_ID;
                            thisColumn.I_BASE_CUSTOM_REPORT_COLUMN_ID = 0;
                            thisColumn.I_PREV_CUSTOM_REPORT_COLUMN_ID = 0;
                            thisColumn.I_ENABLED = 'Y';
                            thisColumn.I_IDENTITIES_ID = _SessionModel._IdentityModel.identities_id;
                            thisColumn.I_ORIGINAL_COLUMN = ReportColumnns.Rows[i].Field<string>("ORIGINAL_COLUMN");
                            thisColumn.I_ALIAS_AREA = ReportColumnns.Rows[i].Field<string>("ORIGINAL_COLUMN");
                            thisColumn.I_COLUMN_AREA = ReportColumnns.Rows[i].Field<string>("ORIGINAL_COLUMN");
                            thisColumn = CoreIH.InsertCustomReportColumns(_Connect, thisColumn);
                        }
                    }
                }

                if (Report.V_ROOT_REPORT_NAME != null)
                {

                    //Report.RootReport = CoreHelper.ReportMapper(_Connect, new DynamicModels.ReportDefinitions
                    //{
                    //    GetStructure = false,
                    //    SearchReport = Report.V_ROOT_REPORT_NAME
                    //}).theReport;

                    DynamicModels.Input.Custom_Report_Order_Sets thisOrderSet = new DynamicModels.Input.Custom_Report_Order_Sets();

                    if (Report.RootReport.ReportColumnOrderSet != null && Report.RootReport.ReportColumnOrderSet.Count > 0)
                    {
                        thisOrderSet = new DynamicModels.Input.Custom_Report_Order_Sets
                        {
                            I_BASE_CUSTOM_REPORTS_ORDER_SET_ID = 0,
                            I_PREV_CUSTOM_REPORTS_ORDER_SET_ID = 0,
                            I_CUSTOM_REPORT_ID = thisModel.O_CUSTOM_REPORT_ID,
                            I_ENABLED = 'Y',
                            I_IDENTITIES_ID = _SessionModel._IdentityModel.identities_id
                        };

                        thisOrderSet = _DynamicInputProcedures._InsertCustomReportOrderSet(_Connect, thisOrderSet);

                        if (Report.RootReport.ReportColumnOrderSet != null && thisOrderSet.O_CUSTOM_REPORTS_ORDER_SET_ID > 0)
                        {
                            for (int j = 0; j < Report.RootReport.ReportColumnOrderSet.Count; j++)
                            {
                                DynamicModels.ReportOrder thisOrder = Report.RootReport.ReportColumnOrderSet[j];

                                _DynamicInputProcedures._InsertCustomReportOrder(_Connect, new DynamicModels.Input.Custom_Report_Order
                                {
                                    I_BASE_CUSTOM_REPORTS_ORDER_ID = 0,
                                    I_PREV_CUSTOM_REPORTS_ORDER_ID = 0,
                                    I_ENABLED = 'Y',
                                    I_IDENTITIES_ID = _SessionModel._IdentityModel.identities_id,
                                    I_CUSTOM_REPORTS_ORDER_SET_ID = thisOrderSet.O_CUSTOM_REPORTS_ORDER_SET_ID,
                                    I_SORT_COLUMN = thisOrder.sort_column,
                                    I_SORT_DIRECTION = thisOrder.sort_direction,
                                    I_SORT_ORDER = thisOrder.sort_order
                                });
                            }
                        }
                    }
                }
            }


            return Report;
        }

    }
}

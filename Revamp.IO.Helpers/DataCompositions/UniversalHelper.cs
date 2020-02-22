using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revamp.IO.Structs;

namespace Revamp.IO.Helpers.DataCompositions
{
    public class UniversalHelper
    {
        public static void GetTableStructure(IConnectToDB _Connect, DynamicModels.ReportDefinitions thisDefinition, _DynamicOutputProcedures CoreOut, DynamicModels.RootReport CurrentReportSelected)
        {
            thisDefinition.theReport = CurrentReportSelected;

            if (thisDefinition.GetStructure)
            {
                if (string.IsNullOrEmpty(thisDefinition.theReport.Source))
                {
                    thisDefinition.theStructure = CoreOut.GetTableStruct(_Connect, CurrentReportSelected.ProcedureName, thisDefinition.DynamicColumns);
                }
                else
                {
                    if (!string.IsNullOrEmpty(thisDefinition.DynamicColumns))
                    {
                        //TODO: Convert to Procedure
                        ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = "select  top 0 '-' as rownumb, " + thisDefinition.DynamicColumns + " from " + _Connect.Schema + "." + thisDefinition.theReport.Source + " where 1 = 2",
                            _dbParameters = new List<DBParameters>()
                        };

                        DataTable thisTableColumns = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
                        DataColumnCollection DCC = thisTableColumns.Columns;

                        thisDefinition.theStructure = thisTableColumns;
                    }
                    else
                    {
                        //TODO: Convert to Procedure
                        ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = "select ORDINAL_POSITION, COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = @TABLE_SCHEMA and upper(TABLE_NAME) = upper(@TABLE_NAME) order by ORDINAL_POSITION ASC",
                            _dbParameters = new List<DBParameters> {
                                new DBParameters { ParamName = "TABLE_SCHEMA", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = _Connect.Schema } ,
                                new DBParameters { ParamName = "TABLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = thisDefinition.theReport.Source }
                            }
                        };

                        DataTable thisTableColumns = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
                        DataColumnCollection DCC = thisTableColumns.Columns;

                        System.Data.DataTable table = new System.Data.DataTable();

                        DataColumn column;
                        column = new DataColumn();
                        column.ColumnName = "rownumb";
                        column.Caption = "rownumb";
                        table.Columns.Add(column);

                        for (int i = 0; i < thisTableColumns.Rows.Count; i++)
                        {
                            column = new DataColumn();
                            column.ColumnName = thisTableColumns.Rows[i]["COLUMN_NAME"].ToString();
                            column.Caption = thisTableColumns.Rows[i]["COLUMN_NAME"].ToString();
                            table.Columns.Add(column);
                        }

                        thisDefinition.theStructure = table;
                    }
                }
            }
        }

        public static SqlDbType GetSqlDbType(string DBType)
        {
            return (SqlDbType)System.Enum.Parse(typeof(SqlDbType), DBType);
        }

        public static DynamicModels.FilterType GetFilterType(string _FilterType)
        {
            return (DynamicModels.FilterType)System.Enum.Parse(typeof(DynamicModels.FilterType), _FilterType);
        }

        public static DynamicModels.RootReport PopulateReport(IConnectToDB _Connect1, DynamicModels.RootReport thisModel, string ReportName, DataTable thisDT)
        {
            ConnectToDB _Connect = _Connect1.Copy();
            _Connect.Schema = "DYNAMIC";

            thisModel.ReportName = ReportName;

            if (thisDT.Rows.Count > 0)
            {
                thisModel.ReportFilters = new List<DynamicModels.RootReportFilter>();

                // thisModel.ProcedureName = thisDT.Rows[0].Field<string>("PROCEDURE_NAME");
                thisModel.Template_ID = thisDT.Rows[0].Field<long>("TEMPLATE_ID");
                thisModel.Template = thisDT.Rows[0].Field<string>("TEMPLATE");
                thisModel.ReportName = thisDT.Rows[0].Field<string>("REPORT_NAME");

                thisModel.ReportFilters = new List<DynamicModels.RootReportFilter>();

                for (int i = 0; i < thisDT.Rows.Count; i++)
                {
                    DynamicModels.RootReportFilter thisReport = new DynamicModels.RootReportFilter();

                    thisReport.FilterName = thisDT.Rows[i].Field<string>("FILTER_NAME");
                    thisReport.PrettyName = thisDT.Rows[i].Field<string>("PRETTY_NAME");
                    thisReport.ParamSize = Convert.ToInt32(thisDT.Rows[i].Field<long>("PARAM_SIZE"));
                    thisReport.SearchParamSize = Convert.ToInt32(thisDT.Rows[i].Field<long>("SEARCH_DB_SIZE"));
                    thisReport.DBType = GetSqlDbType(thisDT.Rows[i].Field<string>("DB_TYPE"));
                    thisReport.SearchDBType = GetSqlDbType(thisDT.Rows[i].Field<string>("SEARCH_DB_TYPE"));
                    thisReport.FilterSelect = thisDT.Rows[i].Field<string>("FILTER_SELECT");
                    thisReport.FilterType = GetFilterType(thisDT.Rows[i].Field<string>("FILTER_TYPE"));
                    thisReport.inPickList = thisDT.Rows[i].Field<string>("IN_PICK_LIST") == "T" ? true : false;
                    thisReport.ParamValue = thisDT.Rows[i].Field<string>("ParamValue");
                    thisReport.Required = thisDT.Rows[i].Field<string>("REQUIRED") == "T" ? true : false;
                    thisReport.AlternateTemplate = thisDT.Rows[i].Field<string>("ALTERNATE_TEMPLATE");
                    thisReport.AlternateReportName = thisDT.Rows[i].Field<string>("ALTERNATE_REPORT_NAME");
                    thisReport.AlternateSchema = thisDT.Rows[i].Field<string>("ALTERNATE_SCHEMA");
                    thisReport.AlternateSource = thisDT.Rows[i].Field<string>("ALTERNATE_SOURCE");

                    thisModel.ReportFilters.Add(thisReport);
                }
            }
            else
            {
                thisModel.ReportFilters = new List<DynamicModels.RootReportFilter>();
            }

            return thisModel;
        }

       

    }
}

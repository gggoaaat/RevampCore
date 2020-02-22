using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Foundation;
using Revamp.IO.SQL.Generators;
using Revamp.IO.Structs;
using Revamp.IO.Structs.Models;
using Revamp.IO.Structs.Models.DataEntry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Revamp.IO.Helpers.Helpers
{
    public class StagesHelper
    {
        public HttpContext Current => new HttpContextAccessor().HttpContext;

        public DataTable StageLimitByIdentityId(IConnectToDB _Connect, string Identities_id)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = Identities_id },
            };

            SQlin.sqlIn = "select isnull((select property_Name 'UserAllowed' from CSA.IDENTITY_PROPERTIES " +
            "where IDENTITIES_ID= @IDENTITIES_ID and property_Type='member_stagelimit'),0) UserAllowed";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            return TempDataTable;
        }

        public string GetControlType(IConnectToDB _Connect, string _propertyId)
        {
            int n;
            bool isNumeric = int.TryParse(_propertyId, out n);

            if (isNumeric)
            {
                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJ_PROP_SETS_ID_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = _propertyId });

                DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_STAGE_CONTROL_TYPE_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                return TempDataTable.Rows[0]["OBJECT_TYPE"].ToString() != "" ? TempDataTable.Rows[0]["OBJECT_TYPE"].ToString() : "";
            }
            else
            {
                return "";
            }
        }

        public string GetObjectTypeViaObjSetID(IConnectToDB _Connect, long? objectsetid)
        {
            string Object_type = "No Matching ID";

            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_SETS_ID_", DBType = SqlDbType.BigInt, ParamValue = objectsetid });

            DataTable _DT = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJECT_SETS_SEARCH",
                new DataTableDotNetModelMetaData { columns = "OBJECT_TYPE", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            foreach (DataRow _DR in _DT.Rows)
            {
                Object_type = _DR.Field<string>("OBJECT_TYPE").ToString();
                break;
            }

            return Object_type;
        }

        public string GetStatus(IConnectToDB _Connect, string status_object_prop_sets_id, string app_id, string forms_id, string rendition)
        {
            string value = "";

            int n;
            bool isNumeric1 = int.TryParse(status_object_prop_sets_id, out n);
            bool isNumeric2 = int.TryParse(app_id, out n);
            bool isNumeric3 = int.TryParse(forms_id, out n);

            if (isNumeric1 && isNumeric2 && isNumeric3)
            {
                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJ_PROP_SETS_ID_", DBType = SqlDbType.BigInt, ParamValue = status_object_prop_sets_id });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", DBType = SqlDbType.BigInt, ParamValue = app_id });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "FORMS_ID_", DBType = SqlDbType.BigInt, ParamValue = forms_id });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "Y" });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "RENDITION_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = rendition });

                DataTable _DTCurrent = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__FORMS_DAT_OPT_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "VALUE", length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                if (_DTCurrent.Rows.Count == 1)
                {
                    value = _DTCurrent.Rows[0][0].ToString();
                }
            }

            return value;
        }

        public bool HasStatusChanged(IConnectToDB _Connect, long? status_object_prop_sets_id, long? app_id, long? forms_id, long? rendition)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlinPrevious = new ER_Query.Parameter_Run();
            ER_Query.Parameter_Run SQlinCurrent = new ER_Query.Parameter_Run();

            //DataTable _DTPrevious = er_query.RUN_QUERY(_Connect, "Select OBJ_PROP_OPT_SETS_ID From " + _Connect.Schema + ".FORMS_DAT_OPT WHERE OBJ_PROP_SETS_ID = '" + status_object_prop_sets_id + 
            //    "' and APPLICATIONS_ID = '" + app_id + "'" + " and FORMS_ID = '" + forms_id + "' and ENABLED = 'Y' and rendition = " + (ER_Tools.ConvertToInt64(rendition) - 1).ToString());
            //int n;

            bool isNumeric1 = status_object_prop_sets_id > 0;
            bool isNumeric2 = app_id > 0;
            bool isNumeric3 = forms_id > 0;
            bool isNumeric4 = rendition > 0;

            if (isNumeric1 && isNumeric2 && isNumeric3 && isNumeric4)
            {
                SQlinPrevious._dbParameters = new List<DBParameters>
                {
                    new DBParameters { ParamName = "OBJ_PROP_SETS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = status_object_prop_sets_id },
                    new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = app_id },
                    new DBParameters { ParamName = "FORMS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = forms_id },
                    new DBParameters { ParamName = "RENDITION", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = ER_Tools.ConvertToInt64(rendition) - 1 },
                };

                SQlinPrevious.sqlIn = " Select OBJ_PROP_OPT_SETS_ID from CSA.FORMS_DAT_OPT d " +
                    "where d.OBJ_PROP_SETS_ID in ( " +
                    "Select OBJ_PROP_SETS_ID from CSA.OBJ_PROP_SETS c " +
                    "where c.OBJECT_PROP_TYPE = 'Items' and c.OBJECT_SETS_ID in ( " +
                    "Select OBJECT_SETS_ID from CSA.OBJECT_SETS b " +
                    "where b.OBJECT_SETS_ID in  " +
                    "(SELECT OBJECT_SETS_ID " +
                    "  FROM CSA.OBJ_PROP_SETS a " +
                    "  where a.OBJ_PROP_SETS_ID = @OBJ_PROP_SETS_ID))) " +
                    "   and APPLICATIONS_ID = @APPLICATIONS_ID " +
                    "and FORMS_ID = @FORMS_ID " +
                    "and ENABLED = 'Y'  " +
                    "and RENDITION = @RENDITION";

                DataTable _DTPrevious = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinPrevious);

                DataColumnCollection DCCPrevious = _DTPrevious.Columns;
                //DataTable _DTCurrent = er_query.RUN_QUERY(_Connect, "Select OBJ_PROP_OPT_SETS_ID From " + _Connect.Schema + ".FORMS_DAT_OPT WHERE OBJ_PROP_SETS_ID = '" + status_object_prop_sets_id + 
                //    "' and APPLICATIONS_ID = '" + app_id + "'" + " and FORMS_ID = '" + forms_id + "' and ENABLED = 'Y' and rendition = " + rendition);


                long? prevrendition = 0;

                try
                {
                    prevrendition = ER_Tools.ConvertToInt64(rendition) - 1;
                }
                catch (Exception)
                {
                    prevrendition = ER_Tools.ConvertToInt64(rendition);
                }

                SQlinCurrent._dbParameters = new List<DBParameters>
                {
                    new DBParameters { ParamName = "OBJ_PROP_SETS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = status_object_prop_sets_id },
                    new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = app_id },
                    new DBParameters { ParamName = "FORMS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = forms_id },
                    new DBParameters { ParamName = "RENDITION", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = rendition },
                };

                SQlinCurrent.sqlIn = " Select OBJ_PROP_OPT_SETS_ID from CSA.FORMS_DAT_OPT d " +
                    "where d.OBJ_PROP_SETS_ID in ( " +
                    "Select OBJ_PROP_SETS_ID from CSA.OBJ_PROP_SETS c " +
                    "where c.OBJECT_PROP_TYPE = 'Items' and c.OBJECT_SETS_ID in ( " +
                    "Select OBJECT_SETS_ID from CSA.OBJECT_SETS b " +
                    "where b.OBJECT_SETS_ID in  " +
                    "(SELECT OBJECT_SETS_ID " +
                    "  FROM CSA.OBJ_PROP_SETS a " +
                    "  where a.OBJ_PROP_SETS_ID = @OBJ_PROP_SETS_ID)))  " +
                    "   and APPLICATIONS_ID = @APPLICATIONS_ID " +
                    "and FORMS_ID = @FORMS_ID " +
                    "and ENABLED = 'Y'  " +
                    "and RENDITION = @RENDITION";

                DataTable _DTCurrent = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinCurrent);

                DataColumnCollection DCCCurrent = _DTCurrent.Columns;

                if (_DTPrevious.Rows.Count == 1 &&
                    DCCPrevious.Contains("OBJ_PROP_OPT_SETS_ID") &&
                    _DTCurrent.Rows.Count == 1 &&
                    DCCCurrent.Contains("OBJ_PROP_OPT_SETS_ID"))
                {

                    string previousValue = _DTPrevious.Rows[0]["OBJ_PROP_OPT_SETS_ID"].ToString();
                    string currentValue = _DTCurrent.Rows[0]["OBJ_PROP_OPT_SETS_ID"].ToString();

                    if (previousValue != currentValue)
                        return true;
                }
            }

            return false;
        }

        public string GetStatusFirstOption(IConnectToDB _Connect, string stages_id)
        {

            string value = "";
            int n;
            bool isNumeric = int.TryParse(stages_id, out n);

            if (isNumeric)
            {
                //check to see if this stage has a status object
                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

                List<DynamicModels.RootReportFilter> statusObjectFilters = new List<DynamicModels.RootReportFilter>();

                statusObjectFilters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGES_ID_", DBType = SqlDbType.BigInt, ParamValue = stages_id });

                DataTable _DTPrevious = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_STAGE_STATUS_OBJECT_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "OBJECT_SETS_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                    statusObjectFilters);

                if (_DTPrevious.Rows.Count > 0)
                {
                    List<DynamicModels.RootReportFilter> statusFirstOptionFilters = new List<DynamicModels.RootReportFilter>();

                    statusFirstOptionFilters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJECT_SETS_ID_", DBType = SqlDbType.BigInt, ParamValue = ER_Tools.ConvertToInt64(_DTPrevious.Rows[0][0]) });

                    DataTable _DTObjectSet = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_OPT_SETS_SEARCH",
                        new DataTableDotNetModelMetaData { columns = "OBJ_PROP_OPT_SETS_ID", length = -1, order = "OBJ_PROP_OPT_SETS_ID asc", start = 0, verify = "T" },
                        statusFirstOptionFilters);

                    if (_DTObjectSet.Rows.Count > 0)
                    {
                        value = _DTObjectSet.Rows[0][0].ToString();
                    }

                }
                //If has a status object Get the status dropdown's first option of the stage_id
            }
            return value;
        }

        public DataTable GetStatusesFirstOption(IConnectToDB _Connect, string stages_id)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            int n;
            bool isNumeric = int.TryParse(stages_id, out n);

            if (isNumeric)
            {
                SQlin._dbParameters = new List<DBParameters>
                {
                    new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = stages_id },
                };

                SQlin.sqlIn = "select a10.OBJECT_SETS_ID, a10.PROPERTY_VALUE, a00.* from CSA.OBJ_PROP_OPT_SETS a00 " +
                    "inner join CSA.VW__OBJ_PROP_SETS a10 ON (a00.OBJ_PROP_SETS_ID = a10.OBJ_PROP_SETS_ID) " +
                    "where a00.OBJ_PROP_OPT_SETS_ID in ( " +
                    "Select MIN(a0.OBJ_PROP_OPT_SETS_ID)  from CSA.OBJ_PROP_OPT_SETS a0 " +
                    "where a0.OBJ_PROP_SETS_ID in ( " +
                    "select a1.OBJ_PROP_SETS_ID from CSA.VW__OBJ_PROP_SETS a1 " +
                    "where a1.OBJECT_SETS_ID in ( " +
                    "select a.OBJECT_SETS_ID from CSA.OBJECT_SETS a where a.OBJECT_SETS_ID in ( " +
                    "Select b.OBJECT_SETS_ID from CSA.VW__OBJECT_SETS b   " +
                    "inner join CSA.[OBJ_PROP_SETS] c on (b.OBJECT_SETS_ID = c.OBJECT_SETS_ID and b.STAGES_ID = @STAGES_ID) " +
                    "where c.PROPERTY_NAME in ('Status Check', 'Status Type', 'Status Message'))) " +
                    "and OBJECT_PROP_TYPE = 'Items') " +
                    "group by OBJ_PROP_SETS_ID)";

                return er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);
            }
            else
            {
                return new DataTable();
            }
        }


        [Serializable]
        public class FormValueSearch
        {
            public string type { get; set; }
            public string forms_id { get; set; }
            public string stages_id { get; set; }
            public string grips_id { get; set; }

            public string object_sets_id { get; set; }
            public string object_prop_sets_id { get; set; }
            public string rendition { get; set; }
        }


        public bool IsTotalField(IConnectToDB _Connect, string object_prop_sets_id)
        {
            bool is_total = false;

            int n;
            bool isNumeric1 = int.TryParse(object_prop_sets_id, out n);

            if (isNumeric1)
            {
                //ER_Query er_query = new ER_Query();
                //DataTable TempDataTable = new DataTable();

                //TempDataTable = er_query.RUN_QUERY(_Connect, "Select value, grips_id from " + _Connect.Schema + ".VW__FORMS_DAT_CHAR WHERE OBJ_PROP_SETS_ID = '" + object_prop_sets_id + "'");
            }
            return is_total;
        }

        public string GetFormEntryTotalValue(IConnectToDB _Connect, string object_prop_sets_id)
        {
            string value = "";

            int n;
            bool isNumeric1 = int.TryParse(object_prop_sets_id, out n);

            if (isNumeric1)
            {
                //string grips_id = "";
                //string[] values = null;
                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJ_PROP_SETS_ID_", DBType = SqlDbType.BigInt, ParamValue = object_prop_sets_id });

                DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "PROPERTY_VALUE", length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                if (TempDataTable.Rows.Count > 0)
                {
                    value = TempDataTable.Rows[0][0].ToString();
                }
            }
            return value;
        }


        public string GetAppRenditionViaUUID(IConnectToDB _Connect, string app_id)
        {
            string value = "";

            Guid n;
            bool isNumeric1 = Guid.TryParse(app_id, out n);

            if (isNumeric1)
            {
                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

                List<DynamicModels.RootReportFilter> rootAppFilters = new List<DynamicModels.RootReportFilter>();

                rootAppFilters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUP_BY", ParamValue = "BASE_APPLICATIONS_UUID" });
                rootAppFilters.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_APPLICATIONS_UUID_", ParamValue = app_id });

                DataTable RootAppByAppIDTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__APPLICATIONS_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "BASE_APPLICATIONS_UUID, MAX(CAST(RENDITION AS bigint)) RENDITION", length = -1, order = "1 asc", start = 0, verify = "T" },
                    rootAppFilters);

                if (RootAppByAppIDTable.Rows.Count > 0)
                {
                    value = ((ER_Tools.ConvertToInt64(RootAppByAppIDTable.Rows[0]["RENDITION"]) + 1)).ToString();
                }
            }

            return value;
        }

        public DataTable FindAll(IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "Form" });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__STAGES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "STAGES_ID asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable Find(IConnectToDB _Connect, string _id)
        {
            int n;
            bool isNumeric1 = int.TryParse(_id, out n);

            if (isNumeric1)
            {
                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGES_ID_", DBType = SqlDbType.BigInt, ParamValue = _id });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "Form" });

                DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__STAGES_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                return TempDataTable;
            }
            else
            {
                return new DataTable();
            }
        }

        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "Form" });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__STAGES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "STAGES_ID asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }
        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value, string stage_type)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = stage_type });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__STAGES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "STAGES_ID asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, List<string> _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column, ParamValue = String.Join(",", _value) });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "Form" });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__STAGES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "STAGES_ID asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }
        public DataTable FindbyURLInfo(IConnectToDB _Connect, string _column, string _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_BY_URL_INFO_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "STAGES_ID asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public string GetStageURL(IConnectToDB _Connect, string[] stages_info)
        {
            string url = "";

            int n;
            if (stages_info[0] != "" || stages_info[0] != null)
            {
                bool isNumeric1 = int.TryParse(stages_info[0], out n);

                if (isNumeric1)
                {
                    DataTable tempDataTable = FindbyURLInfo(_Connect, "stages_id", stages_info[0]);
                    if (tempDataTable.Rows.Count > 0)
                    {

                        url = "/form/" + tempDataTable.Rows[0]["Core_Name"].ToString() + "/" + tempDataTable.Rows[0]["APPLICATION_LINK"].ToString() + "/" + tempDataTable.Rows[0]["STAGE_LINK"].ToString();
                    }
                }
            }

            return url;
        }

        public string GetAppIDByStageID(IConnectToDB _Connect, string id)
        {
            string value = "";

            int n;
            bool isNumeric1 = int.TryParse(id, out n);

            if (isNumeric1)
            {
                DataTable tempDataTable = FindbyColumnID(_Connect, "stages_id", id);

                if (tempDataTable.Rows.Count > 0)
                {
                    value = tempDataTable.Rows[0]["APPLICATIONS_ID"].ToString();
                }
            }

            return value;
        }

        public string Get_APPID_VIA_STAGEID(IConnectToDB _Connect, long? StageID)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>(); DataTable TempDataTable = new DataTable();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = StageID });

            DataTable _DT = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_STAGES_SEARCH", new DataTableDotNetModelMetaData { columns = "APPLICATIONS_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            string AppID = "No Stages matches ID";

            foreach (DataRow _DR in _DT.Rows)
            {
                AppID = _DR.Field<long?>("APPLICATIONS_ID").ToString();
                break;
            }

            return AppID;
        }


        public List<ViewGripModel> GetGripsViaStage(IConnectToDB _Connect, string stages_id)
        {
            //ApplicationObjects appObjects = new ApplicationObjects();
            List<ViewGripModel> GripsList = new List<ViewGripModel>();

            int n;
            bool isNumeric1 = int.TryParse(stages_id, out n);

            if (isNumeric1)
            {
                GripsHelper grips = new GripsHelper();

                DataTable gripsdt;

                if (stages_id.ToLower() == "all")
                {
                    gripsdt = grips.FindAll(_Connect);
                }
                else
                {
                    gripsdt = grips.FindbyColumnID(_Connect, "stages_id", stages_id);
                }

                ViewGripModel[] Grips = new ViewGripModel[gripsdt.Rows.Count];

                for (int i = 0; i < gripsdt.Rows.Count; i++)
                {
                    string thisGripRow = new JObject(gripsdt.Columns.Cast<DataColumn>()
                                        .Select(c => new JProperty(c.ColumnName, JToken.FromObject(gripsdt.Rows[i][c])))
                                  ).ToString(Formatting.None);

                    Grips[i] = JsonConvert.DeserializeObject<ViewGripModel>(thisGripRow);

                    List<ViewObjectSetModel> VWOSM = grips.GetObjectSetsViaGrip(_Connect, gripsdt.Rows[i].Field<long?>("grips_id").ToString());

                    Grips[i].ObjectSets = VWOSM;

                    if (Grips[i].grip_type != "Cache")
                    {
                        GripsList.Add(Grips[i]);
                    }
                }
            }
            return GripsList;
        }

        public DataTable FindbyPK(IConnectToDB _Connect, string applications_id, string stageType, string stageName)
        {
            ER_Query er_query = new ER_Query();
            DataTable TempDataTable = new DataTable();

            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", DBType = SqlDbType.BigInt, ParamValue = applications_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_NAME_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = stageName });

            if (stageType == "Identity")
            {

                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = stageType });
            }
            else
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "Form" });
            }

            TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__STAGES_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

            return TempDataTable;
        }

        public List<ViewStageModel> GetStage(IConnectToDB _Connect, string applications_id, string stageType, string stageName)
        {
            //ApplicationObjects appObjects = new ApplicationObjects();

            List<ViewStageModel> StageList = new List<ViewStageModel>();
            ViewStageModel CachedStage = null;
            try
            {
                ER_DML er_dml = new ER_DML();
                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", DBType = SqlDbType.BigInt, ParamValue = applications_id });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGE_NAME_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = stageName });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GRIP_TYPE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "Cache" });

                DataTable FileCache = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__OBJ_PROP_SETS_DAT_FILE_SEARCH",
                        new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                        Filters);

                if (FileCache.Rows.Count == 1)
                {
                    ER_Tools _tools = new ER_Tools();

                    byte[] tempbytes = FileCache.Rows[0].Field<byte[]>("VALUE");

                    CachedStage = (ViewStageModel)_tools.ByteArrayToObject(tempbytes);
                    StageList.Add(CachedStage);
                }
            }
            catch (Exception e)
            {
                // StageList = null;
                e.ToString();
            }

            if (CachedStage == null || stageName == "Login")
            {

                StagesHelper stages = new StagesHelper();

                DataTable stagesdt;

                stagesdt = stages.FindbyPK(_Connect, applications_id, stageType, stageName);



                ViewStageModel[] Stages = new ViewStageModel[stagesdt.Rows.Count];

                int i = 0;
                foreach (DataRow _DR in stagesdt.Rows)
                {
                    Stages[i] = new ViewStageModel
                    {
                        applications_id = _DR.Field<long?>("applications_id"),
                        containers_id = _DR.Field<long?>("containers_id"),
                        dt_available = _DR.Field<DateTime?>("dt_available"),
                        dt_created = _DR.Field<DateTime>("dt_created"),
                        dt_end = _DR.Field<DateTime?>("dt_end"),
                        stage_name = _DR.Field<string>("stage_name"),
                        stage_type = _DR.Field<string>("stage_type"),
                        stages_id = _DR.Field<long?>("stages_id"),
                        application_name = _DR.Field<string>("application_name"),
                        cores_id = _DR.Field<long?>("cores_id"),
                        object_layer = _DR.Field<string>("object_layer"),
                        enabled = _DR.Field<string>("enabled"),
                        object_type = _DR.Field<string>("object_type"),
                        stage_link = _DR.Field<string>("stage_link"),
                        formbodySpan = "12"
                    };

                    Stages[i].Grips = stages.GetGripsViaStage(_Connect, _DR.Field<long?>("stages_id").ToString());

                    StageList.Add(Stages[i]);

                    i++;
                }

            }

            return StageList;
        }


        #region Old Way of Saving CASTGOOP
        //public string SaveStageBuilder(IConnectToDB _Connect, StageBuilderModel stageBuilderModel, String JSON, string parent_id, long? identities_id, SessionObjects SO)
        //{
        //    var whichWay = "new";

        //    switch (whichWay.ToLower())
        //    {
        //        case "old":
        //            return SaveStageBuilderOLDWayOfBusiness(_Connect, stageBuilderModel, JSON, parent_id, identities_id, SO);

        //        default:
        //        case "new":
        //            return ""; // SaveStageBuilderNew(_Connect, stageBuilderModel, JSON, parent_id, identities_id, SO);
        //    }
        //}

        //public string SaveStageBuilderOLDWayOfBusiness(IConnectToDB _Connect, StageBuilderModel stageBuilderModel, String JSON, string parent_id, long? identities_id, SessionObjects SO)
        //{
        //    string errmsg = "";
        //    if (stageBuilderModel != null)
        //    {
        //        #region VARIABLES
        //        add addHelp = new add();
        //        int stages_count = stageBuilderModel.stage.Count;
        //        AppHelper appHelper = new AppHelper();
        //        ER_Tools er_tools = new ER_Tools();
        //        SecurityHelper SECH = new SecurityHelper();
        //        //string appid = "";
        //        long? coreid = 1000;
        //        long? containersid = 1000;
        //        string description = "";
        //        ER_DML er_dml = new ER_DML();
        //        Values.AddObjectPropertySet thisPropSet = new Values.AddObjectPropertySet();

        //        long? _identity_id = identities_id;


        //        System.Xml.Linq.XNode node = Newtonsoft.Json.JsonConvert.DeserializeXNode(@JSON, "Root");

        //        string app_background_color = "#000;";

        //        if (stageBuilderModel.app_core != null)
        //            coreid = ER_Tools.ConvertToInt64(stageBuilderModel.app_core);

        //        if (stageBuilderModel.app_description != null)
        //            description = stageBuilderModel.app_description;

        //        if (stageBuilderModel.app_background_color != null)
        //            app_background_color = stageBuilderModel.app_background_color;

        //        if (stageBuilderModel.app_container != null)
        //            containersid = ER_Tools.ConvertToInt64(stageBuilderModel.app_container);
        //        #endregion

        //        #region SAVE APPLICATION

        //        Values.AddApplication thisApplication = new Values.AddApplication
        //        {
        //            I_CORES_ID = coreid,
        //            I_CORES_UUID = null,
        //            I_APPLICATION_NAME = stageBuilderModel.app_name,
        //            I_APPLICATION_LINK = stageBuilderModel.app_name.Trim().Replace(" ", "-"),
        //            I_APPLICATION_DESCRIPTION = stageBuilderModel.app_description
        //        };

        //        if (parent_id != "")
        //        {
        //            string parent_name = appHelper.GetRootAppName(_Connect, parent_id);
        //            string rendition = GetAppRendition(_Connect, parent_id);

        //            thisApplication.I_ROOT_APPLICATION = parent_name;
        //            thisApplication.I_RENDITION = rendition;

        //            thisApplication = addHelp.ADD_ENTRY_Application(_Connect, thisApplication);

        //            SECH.LogActivity(_Connect, "Add Object", coreid.ToString(), thisApplication.O_APPLICATIONS_ID.ToString(), "APPLICATIONS", thisApplication.O_APPLICATIONS_ID.ToString(), identities_id, "1005", "1007", (stageBuilderModel.app_name + " updated by " + System.Web.HttpContext.Current.Session["UserName"].ToString().ToUpper()), "1000", "1000", "");
        //        }
        //        else
        //        {

        //            if (stageBuilderModel.rendition == null)
        //            {
        //                try
        //                {
        //                    thisApplication.I_RENDITION = "0";
        //                    thisApplication.I_ROOT_APPLICATION = stageBuilderModel.app_name;

        //                    thisApplication = addHelp.ADD_ENTRY_Application(_Connect, thisApplication);

        //                    //appid = thisApplication.application_id.ToString();

        //                    if (!thisApplication.O_ERR_MESS.Contains("Error")) //If the app name is already taken.
        //                        SECH.LogActivity(_Connect, "Add Object", coreid.ToString(), thisApplication.O_APPLICATIONS_ID.ToString(), "APPLICATIONS", thisApplication.O_APPLICATIONS_ID.ToString(), identities_id, "1006", "1007", (stageBuilderModel.app_name + " created by " + System.Web.HttpContext.Current.Session["UserName"].ToString().ToUpper()), "1000", "1000", "");
        //                    else
        //                        throw (new Exception());

        //                }
        //                catch (Exception)
        //                {
        //                    errmsg = "The application name " + stageBuilderModel.app_name + " already exist. Please choose a different application name or a different core to save this application to.";
        //                    return errmsg;

        //                }
        //            }
        //            else
        //            {
        //                thisApplication.I_RENDITION = stageBuilderModel.rendition;
        //                thisApplication.I_ROOT_APPLICATION = stageBuilderModel.app_name;

        //                thisApplication = addHelp.ADD_ENTRY_Application(_Connect, thisApplication);

        //                if (stageBuilderModel.rendition == "0")
        //                {
        //                    SECH.LogActivity(_Connect, "Add Object", coreid.ToString(), thisApplication.O_APPLICATIONS_ID.ToString(), "APPLICATIONS", thisApplication.O_APPLICATIONS_ID.ToString(), identities_id, "1006", "1007", (stageBuilderModel.app_name + " created by " + System.Web.HttpContext.Current.Session["UserName"].ToString().ToUpper()), "1000", "1000", "");
        //                }
        //                else
        //                {
        //                    SECH.LogActivity(_Connect, "Add Object", coreid.ToString(), thisApplication.O_APPLICATIONS_ID.ToString(), "APPLICATIONS", thisApplication.O_APPLICATIONS_ID.ToString(), identities_id, "1005", "1007", (stageBuilderModel.app_name + " updated by " + System.Web.HttpContext.Current.Session["UserName"].ToString().ToUpper()), "1000", "1000", "");
        //                }
        //            }
        //        }
        //        #endregion

        //        if (thisApplication.O_ERR_MESS != null && thisApplication.O_ERR_MESS.ToLower().Contains("error"))
        //        {
        //            errmsg = thisApplication.O_ERR_MESS;
        //        }
        //        else
        //        {
        //            #region APPLICATION LOGO
        //            //app logo image data
        //            if (stageBuilderModel.app_logo != null)
        //            {
        //                ER_DML dml = new ER_DML();
        //                IOHelper io = new IOHelper();
        //                //ObjectPropertySetID = add.ObjectSetProperty(_Connect, ObjectSetsID, "Text", "File", "ID", "false", "false", "", "Image");
        //                // DMLH.ADD_ENTRY_OBJECT_DATA(_Connect, Identities_ID, Stages_ID, GripsID, ObjectPropertySetID, "OBJ_PROP_SETS", ObjectPropertySetID, "0", "file", new Object_Value { _File = io.getBytes(stageBuilderStagesModel.grid_items[i].imageData) });

        //                Values.AddFile thisFile = addHelp.ADD_ENTRY_FILE(_Connect, new Values.AddFile
        //                {
        //                    I_FILE_NAME = "logo",
        //                    I_CONTENT_TYPE = ".jpg",
        //                    I_FILE_SIZE = io.getBytes(stageBuilderModel.app_logo).Length,
        //                    I_FILE_DATA = io.getBytes(stageBuilderModel.app_logo),
        //                    I_IDENTITIES_ID = SO._IdentityModel.identities_id
        //                });

        //                addHelp.ADD_FILE_POINT(_Connect, new Values.AddFilePoint
        //                {
        //                    I_FILES_ID = thisFile.O_FILES_ID,
        //                    I_IDENTITIES_ID = SO._IdentityModel.identities_id,
        //                    I_OBJ_PROP_SETS_ID = thisApplication.O_APPLICATIONS_ID
        //                });
        //            }
        //            #endregion

        //            #region APPLICATION ROLES
        //            //app roles
        //            if (stageBuilderModel.app_roles != null)
        //            {
        //                foreach (string role in stageBuilderModel.app_roles)
        //                {
        //                    //SECH.AddRoletoApp(_Connect, ER_Tools.ConvertToInt64(SECH.GetRoleID(_Connect, role, ER_Tools.ConvertToInt64(coreid))), ER_Tools.ConvertToInt64(appid));
        //                    SECH.AddRoletoApp(_Connect, ER_Tools.ConvertToInt64(role), thisApplication.O_APPLICATIONS_ID);
        //                }
        //            }
        //            #endregion

        //            #region APPLICATION SETTINGS
        //            //app settings
        //            if (stageBuilderModel.app_background_color != null || stageBuilderModel.app_hide_tool_bar != null || stageBuilderModel.app_send_email_confirm != null || stageBuilderModel.app_email_confirm_msg != null || stageBuilderModel.app_subcontainers != null)
        //            {
        //                string /*Stages_ID = "",*/ GripsID = "", ObjectSetsID = "", ObjectPropertySetID = "";
        //                long? Organizations_ID = containersid;

        //                #region Stage Application Properties
        //                Values.AddStage thisStageAppProps = new Values.AddStage
        //                {
        //                    I_STAGE_TYPE = "Settings",
        //                    I_STAGE_NAME = "Application Properties",
        //                    I_APPLICATIONS_ID = thisApplication.O_APPLICATIONS_ID,
        //                    I_APPLICATIONS_UUID = thisApplication.O_APPLICATIONS_UUID,
        //                    I_CONTAINERS_ID = containersid,
        //                    I_IDENTITIES_ID = _identity_id,
        //                    PrettyLink = "Application_Properties"
        //                };

        //                /* string _stagetype, string _stagename, string _application_id, string _containers_id, string _identities_id, string _pretty_link*/
        //                thisStageAppProps = addHelp.ADD_ENTRY_Stage(_Connect, thisStageAppProps);
        //                #endregion

        //                #region Grip Application Properties
        //                Values.AddGrip gripApplicationSettings = new Values.AddGrip
        //                {
        //                    I_STAGES_UUID = thisStageAppProps.O_STAGES_UUID,
        //                    I_STAGES_ID = thisStageAppProps.O_STAGES_ID,
        //                    I_STAGE_TYPE = "Settings",
        //                    I_STAGE_NAME = "Application Properties",
        //                    I_GRIP_TYPE = "Settings",
        //                    I_GRIP_NAME = "Application Properties",
        //                    I_CONTAINERS_ID = Organizations_ID,
        //                    I_IDENTITIES_ID = _identity_id
        //                };

        //                gripApplicationSettings = addHelp.GripWithPermissions(_Connect, gripApplicationSettings);
        //                GripsID = gripApplicationSettings.O_GRIPS_ID.ToString();
        //                #endregion

        //                #region Object Application Properties
        //                Values.AddObjectSet objectSetCoreSettings = new Values.AddObjectSet
        //                {
        //                    I_GRIPS_ID = gripApplicationSettings.O_GRIPS_ID,
        //                    I_STAGE_TYPE = "Settings",
        //                    I_STAGE_NAME = "Application Properties",
        //                    I_GRIP_TYPE = "Settings",
        //                    I_GRIP_NAME = "Application Properties",
        //                    I_OBJECT_TYPE = "Settings",
        //                    I_CONTAINERS_ID = Organizations_ID,
        //                    I_IDENTITIES_ID = _identity_id
        //                };

        //                objectSetCoreSettings = addHelp.ObjectSetwithPermission(_Connect, objectSetCoreSettings);
        //                ObjectSetsID = objectSetCoreSettings.O_OBJECT_SETS_ID.ToString();
        //                #endregion
        //                //save app background color

        //                #region Property Background Color
        //                if (stageBuilderModel.app_background_color != null)
        //                {
        //                    thisPropSet = new Values.AddObjectPropertySet
        //                    {
        //                        I_OBJECT_SETS_ID = objectSetCoreSettings.O_OBJECT_SETS_ID,
        //                        I_OBJECT_PROP_TYPE = "Text",
        //                        I_VALUE_DATATYPE = "Characters",
        //                        I_PROPERTY_NAME = "Background Color",
        //                        I_HAS_PARENT = "false",
        //                        I_HAS_CHILD = "false",
        //                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                        I_PROPERTY_VALUE = stageBuilderModel.app_background_color
        //                    };

        //                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //                    ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID.ToString();
        //                }
        //                #endregion

        //                #region Property Hide Toolbar
        //                //save hide app toolbar
        //                if (stageBuilderModel.app_hide_tool_bar != null)
        //                {
        //                    thisPropSet = new Values.AddObjectPropertySet
        //                    {
        //                        I_OBJECT_SETS_ID = objectSetCoreSettings.O_OBJECT_SETS_ID,
        //                        I_OBJECT_PROP_TYPE = "Text",
        //                        I_VALUE_DATATYPE = "Characters",
        //                        I_PROPERTY_NAME = "Hide ToolBar",
        //                        I_HAS_PARENT = "false",
        //                        I_HAS_CHILD = "false",
        //                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                        I_PROPERTY_VALUE = stageBuilderModel.app_hide_tool_bar
        //                    };

        //                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //                    ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID.ToString();
        //                }
        //                #endregion

        //                #region Property Email Confirm
        //                //save email confirmation
        //                if (stageBuilderModel.app_send_email_confirm != null)
        //                {
        //                    thisPropSet = new Values.AddObjectPropertySet
        //                    {
        //                        I_OBJECT_SETS_ID = objectSetCoreSettings.O_OBJECT_SETS_ID,
        //                        I_OBJECT_PROP_TYPE = "Text",
        //                        I_VALUE_DATATYPE = "Characters",
        //                        I_PROPERTY_NAME = "Email Confirm",
        //                        I_HAS_PARENT = "false",
        //                        I_HAS_CHILD = "false",
        //                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                        I_PROPERTY_VALUE = stageBuilderModel.app_send_email_confirm
        //                    };

        //                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //                    ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID.ToString();
        //                }
        //                #endregion

        //                #region Email Confirm Message
        //                //save email confirmation message
        //                if (stageBuilderModel.app_email_confirm_msg != null)
        //                {
        //                    thisPropSet = new Values.AddObjectPropertySet
        //                    {
        //                        I_OBJECT_SETS_ID = objectSetCoreSettings.O_OBJECT_SETS_ID,
        //                        I_OBJECT_PROP_TYPE = "Text",
        //                        I_VALUE_DATATYPE = "Characters",
        //                        I_PROPERTY_NAME = "Email Confirm Message",
        //                        I_HAS_PARENT = "false",
        //                        I_HAS_CHILD = "false",
        //                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                        I_PROPERTY_VALUE = stageBuilderModel.app_email_confirm_msg
        //                    };

        //                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //                    ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID.ToString();
        //                }
        //                #endregion

        //                //Save subcontainers for the app in stages containers
        //                if (stageBuilderModel.app_subcontainers != null)
        //                {
        //                    if (stageBuilderModel.app_subcontainers.Count > 0)
        //                    {
        //                        for (int i = 0; i < stageBuilderModel.app_subcontainers.Count; i++)
        //                        {
        //                            Values.AddStageContainer thisStageContainer = addHelp.ADD_ENTRY_Stage_Containers(_Connect, new Values.AddStageContainer
        //                            {
        //                                I_CONTAINERS_ID = ER_Tools.ConvertToInt64(stageBuilderModel.app_subcontainers[i]),
        //                                I_STAGES_ID = thisStageAppProps.O_STAGES_ID
        //                            });
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion

        //            List<ViewStageModel> _StagesToBytes = new List<ViewStageModel>();

        //            #region LOOP THROUGH STAGES
        //            for (int s = 0; s < stages_count; s++)
        //            {
        //                StageBuilderStagesModel stageBuilderStagesModel = stageBuilderModel.stage[s];

        //                //Instantiate Stage Model for Byte Save.

        //                string /*Stages_ID = "",*/ Stages_Type = "Form";
        //                long? GripsID;
        //                long? ObjectSetsID;
        //                long? ObjectPropertySetID = null;
        //                long? Organizations_ID = containersid;
        //                long? Identities_ID = identities_id;
        //                string stageName = stageBuilderStagesModel.stage_name;
        //                Values.AddObjectPropertyOption ObjectPropertyOptionSetID = new Values.AddObjectPropertyOption();
        //                int count = stageBuilderStagesModel.grid_items.Count;

        //                #region STAGE FROM BUILDER
        //                Values.AddStage thisStage = new Values.AddStage
        //                {
        //                    I_STAGE_TYPE = Stages_Type,
        //                    I_STAGE_NAME = stageBuilderStagesModel.stage_name,
        //                    I_APPLICATIONS_ID = thisApplication.O_APPLICATIONS_ID,
        //                    I_APPLICATIONS_UUID = thisApplication.O_APPLICATIONS_UUID,
        //                    I_CONTAINERS_ID = Organizations_ID,
        //                    I_IDENTITIES_ID = _identity_id,
        //                    PrettyLink = stageBuilderStagesModel.stage_name.Replace(" ", "-")
        //                };

        //                thisStage = addHelp.ADD_ENTRY_Stage(_Connect, thisStage);

        //                //This saves the stage builder grid to the database.
        //                ViewGripModel GridGripToBytes = SaveStageBuilderGrid(_Connect, stageBuilderStagesModel, Stages_Type, thisStage.O_STAGES_ID, containersid, SO);

        //                ViewStageModel _StageToBytes = new ViewStageModel();

        //                _StageToBytes.Grips = new List<ViewGripModel>();
        //                _StageToBytes.applications_id = thisApplication.O_APPLICATIONS_ID;
        //                _StageToBytes.application_name = stageBuilderModel.app_name;
        //                _StageToBytes.stage_link = stageBuilderStagesModel.stage_name.Replace(" ", "-");
        //                _StageToBytes.stage_name = stageName;
        //                _StageToBytes.cores_id = ER_Tools.ConvertToInt64(coreid);
        //                _StageToBytes.stage_type = Stages_Type;
        //                _StageToBytes.stages_id = thisStage.O_STAGES_ID;
        //                _StageToBytes.identities_id = _identity_id;
        //                _StageToBytes.object_type = Stages_Type;
        //                #endregion

        //                #region THINGS RELATED TO STAGE
        //                if (s == 0)
        //                {
        //                    SaveStageBuilderJSONOldWayOfBusiness(_Connect, stageBuilderStagesModel, stageBuilderModel.app_name, Stages_Type, thisStage.O_STAGES_ID, Identities_ID, JSON);
        //                }

        //                if (stageBuilderStagesModel.stage_roles != null)
        //                {
        //                    foreach (string role in stageBuilderStagesModel.stage_roles)
        //                    {
        //                        SECH.AddRoletoStage(_Connect, ER_Tools.ConvertToInt64(SECH.GetRoleID(_Connect, role, ER_Tools.ConvertToInt64(coreid))), thisStage.O_STAGES_ID);
        //                    }
        //                }

        //                if (stageBuilderStagesModel.stage_parents != null)
        //                {
        //                    for (int i = 0; i < stageBuilderStagesModel.stage_parents.Count; i++)
        //                    {
        //                        addHelp.ADD_ENTRY_Stage_Relationships(_Connect, new Values.AddStageRelationship
        //                        {
        //                            I_OBJECT_TYPE = "Parent_Relationship",
        //                            I_RELATED_STAGES_ID = ER_Tools.ConvertToInt64(stageBuilderStagesModel.stage_parents[i]),
        //                            I_STAGES_ID = thisStage.O_STAGES_ID
        //                        });
        //                    }
        //                }

        //                if (stageBuilderStagesModel.stage_children != null)
        //                {
        //                    for (int i = 0; i < stageBuilderStagesModel.stage_children.Count; i++)
        //                    {
        //                        addHelp.ADD_ENTRY_Stage_Relationships(_Connect, new Values.AddStageRelationship
        //                        {
        //                            I_OBJECT_TYPE = "Child_Relationship",
        //                            I_RELATED_STAGES_ID = ER_Tools.ConvertToInt64(stageBuilderStagesModel.stage_children[i]),
        //                            I_STAGES_ID = thisStage.O_STAGES_ID
        //                        });
        //                    }
        //                }
        //                #endregion

        //                #region Grip Form Fields
        //                //_StageToBytes.gripList = new List<ViewGripModel>();
        //                ViewGripModel GripToBytes = new ViewGripModel();

        //                Values.AddGrip thisGrip = new Values.AddGrip
        //                {
        //                    I_STAGES_UUID = thisStage.O_STAGES_UUID,
        //                    I_STAGES_ID = thisStage.O_STAGES_ID,
        //                    I_STAGE_TYPE = "Form",
        //                    I_STAGE_NAME = stageBuilderStagesModel.stage_name,
        //                    I_GRIP_TYPE = "Form",
        //                    I_GRIP_NAME = "Form Fields",
        //                    I_CONTAINERS_ID = Organizations_ID,
        //                    I_IDENTITIES_ID = ER_Tools.ConvertToInt64(identities_id)
        //                };

        //                thisGrip = addHelp.GripWithPermissions(_Connect, thisGrip);
        //                GripsID = thisGrip.O_GRIPS_ID;

        //                GripToBytes.stages_id = thisStage.O_STAGES_ID;
        //                GripToBytes.stage_type = "Form";
        //                GripToBytes.stage_name = stageBuilderStagesModel.stage_name;
        //                GripToBytes.grip_type = "Form";
        //                GripToBytes.grip_name = "Form Fields";
        //                GripToBytes.containers_id = Organizations_ID;
        //                GripToBytes.identities_id = _identity_id;
        //                GripToBytes.ObjectSets = new List<ViewObjectSetModel>();
        //                #endregion

        //                if (count > 0)
        //                {
        //                    int i;
        //                    #region LOOP through Objects
        //                    for (i = 0; i < count; i++)
        //                    {
        //                        string fieldType = stageBuilderStagesModel.grid_items[i].type;
        //                        string fieldID = stageBuilderStagesModel.grid_items[i].id;//stageBuilderStagesModel.grid_items[i].name;
        //                        int conCount = 0;
        //                        string fieldObjecttype = "";
        //                        string objecttype = "Text";
        //                        string propertyValue = stageBuilderStagesModel.grid_items[i].label;
        //                        bool saveindb = true;

        //                        #region SWITCH Field Object Type
        //                        switch (fieldType)
        //                        {
        //                            case "addImage":
        //                                fieldObjecttype = "File";

        //                                break;
        //                            case "textField":
        //                                fieldObjecttype = "Text_Box";
        //                                break;
        //                            case "dropDown":
        //                                fieldObjecttype = "Drop_Down";
        //                                break;
        //                            case "richText":
        //                                fieldObjecttype = "Rich_Text";
        //                                break;
        //                            case "textArea":
        //                                fieldObjecttype = "Paragraph_Text";
        //                                break;
        //                            case "radioButton":
        //                                fieldObjecttype = "Radio_Button";
        //                                break;
        //                            case "checkBoxGroup":
        //                                fieldObjecttype = "Check_Box";
        //                                break;
        //                            case "button":
        //                                fieldObjecttype = "Button";
        //                                break;
        //                            case "link":
        //                                fieldObjecttype = "Link";
        //                                break;
        //                            case "textLabel":
        //                                fieldObjecttype = "Text";
        //                                //fieldID = stageBuilderStagesModel.grid_items[i].label;
        //                                objecttype = "HTML";
        //                                propertyValue = stageBuilderStagesModel.grid_items[i].name;
        //                                break;
        //                            case "number":
        //                                fieldObjecttype = "Number";
        //                                break;
        //                            case "currency":
        //                                fieldObjecttype = "Decimal";
        //                                stageBuilderStagesModel.grid_items[i].currencyType = "True";
        //                                break;
        //                            case "total":
        //                                fieldObjecttype = "Total";
        //                                break;
        //                            case "email":
        //                                fieldObjecttype = "Email";
        //                                break;
        //                            case "url":
        //                                fieldObjecttype = "Url";
        //                                break;
        //                            case "date":
        //                                fieldObjecttype = "Date";
        //                                break;
        //                            case "time":
        //                                fieldObjecttype = "Time";
        //                                break;
        //                            case "phone":
        //                                fieldObjecttype = "Phone";
        //                                break;
        //                            case "creditCard":
        //                                fieldObjecttype = "Credit_Card";
        //                                break;
        //                            case "fileUpload":
        //                                fieldObjecttype = "File_Upload";
        //                                break;
        //                            case "signature":
        //                                fieldObjecttype = "signature";
        //                                break;
        //                            case "viewTable":
        //                                fieldObjecttype = "View_Table";
        //                                break;
        //                            default:
        //                                saveindb = false;
        //                                break;
        //                        }
        //                        #endregion

        //                        if (saveindb)
        //                        {
        //                            #region Object Form Fields
        //                            Values.AddObjectSet objectSetFormField = new Values.AddObjectSet
        //                            {
        //                                I_GRIPS_ID = thisGrip.O_GRIPS_ID,
        //                                I_STAGE_TYPE = "Form",
        //                                I_STAGE_NAME = stageName,
        //                                I_GRIP_TYPE = "Form",
        //                                I_GRIP_NAME = "Form Fields",
        //                                I_OBJECT_TYPE = fieldObjecttype,
        //                                I_CONTAINERS_ID = Organizations_ID,
        //                                I_IDENTITIES_ID = _identity_id
        //                            };

        //                            objectSetFormField = addHelp.ObjectSetwithPermission(_Connect, objectSetFormField);
        //                            ObjectSetsID = objectSetFormField.O_OBJECT_SETS_ID;

        //                            ViewObjectSetModel _ObjectSetToBytes = new ViewObjectSetModel();

        //                            _ObjectSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                            _ObjectSetToBytes.grips_id = ER_Tools.ConvertToInt64(GripsID);
        //                            _ObjectSetToBytes.grip_type = "Form";
        //                            _ObjectSetToBytes.stage_name = stageName;
        //                            _ObjectSetToBytes.stage_type = "Form";
        //                            _ObjectSetToBytes.grip_name = "Form Fields";
        //                            _ObjectSetToBytes.object_type = fieldObjecttype;
        //                            _ObjectSetToBytes.containers_id = Organizations_ID;
        //                            _ObjectSetToBytes.identities_id = _identity_id;
        //                            #endregion

        //                            _ObjectSetToBytes.ObjectPropSets = new List<ViewObjectPropSetsModel>();

        //                            ViewObjectPropSetsModel PropSetToBytes = new ViewObjectPropSetsModel();

        //                            if (fieldObjecttype != "Text")
        //                            {
        //                                #region Property ID
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "ID",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "ID",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = fieldID
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ObjectPropertySetID;
        //                                PropSetToBytes.object_sets_id = objectSetFormField.O_OBJECT_SETS_ID;
        //                                PropSetToBytes.object_type = "ID";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "ID";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = fieldID;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                #endregion

        //                                #region Property Label
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = objecttype,
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Label",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = propertyValue
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = objecttype;
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Label";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = propertyValue;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                #endregion

        //                            }
        //                            else
        //                            {
        //                                //save label as byte 
        //                                ER_DML dml = new ER_DML();
        //                                IOHelper io = new IOHelper();

        //                                #region Property ID
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "ID",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = "Label"
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "ID";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = "Label";
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                #endregion

        //                                byte[] labelbyte = io.getBytes(propertyValue);

        //                                Values.AddFile thisFile = addHelp.ADD_ENTRY_FILE(_Connect, new Values.AddFile
        //                                {
        //                                    I_FILE_NAME = "",
        //                                    I_CONTENT_TYPE = ".ER",
        //                                    I_FILE_SIZE = labelbyte.Length,
        //                                    I_FILE_DATA = labelbyte,
        //                                    I_IDENTITIES_ID = SO._IdentityModel.identities_id
        //                                });

        //                                addHelp.ADD_FILE_POINT(_Connect, new Values.AddFilePoint
        //                                {
        //                                    I_FILES_ID = thisFile.O_FILES_ID,
        //                                    I_IDENTITIES_ID = SO._IdentityModel.identities_id,
        //                                    I_OBJ_PROP_SETS_ID = ObjectPropertySetID
        //                                });
        //                            }

        //                            #region Property Widget Section
        //                            thisPropSet = new Values.AddObjectPropertySet
        //                            {
        //                                I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                I_OBJECT_PROP_TYPE = "Text",
        //                                I_VALUE_DATATYPE = "Characters",
        //                                I_PROPERTY_NAME = "Widget",
        //                                I_HAS_PARENT = "false",
        //                                I_HAS_CHILD = "false",
        //                                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].section
        //                            };

        //                            thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                            PropSetToBytes = new ViewObjectPropSetsModel();
        //                            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                            PropSetToBytes.object_type = "Text";
        //                            PropSetToBytes.object_prop_type = "Text";
        //                            PropSetToBytes.value_datatype = "Characters";
        //                            PropSetToBytes.property_name = "Widget";
        //                            PropSetToBytes.has_parent = "false";
        //                            PropSetToBytes.has_child = "false";
        //                            PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                            PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].section;
        //                            _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            #endregion

        //                            //Add Roles to Object Sets
        //                            if (stageBuilderStagesModel.grid_items[i].section_roles != null)
        //                            {
        //                                foreach (string role in stageBuilderStagesModel.grid_items[i].section_roles)
        //                                {
        //                                    SECH.AddRoletoObjectSet(_Connect, ER_Tools.ConvertToInt64(SECH.GetRoleID(_Connect, role, ER_Tools.ConvertToInt64(coreid))), ER_Tools.ConvertToInt64(ObjectSetsID));
        //                                }
        //                            }

        //                            #region Property Column Order
        //                            //Column order
        //                            if (stageBuilderStagesModel.grid_items[i].columnorder != null)
        //                            {

        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Column_Order",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].columnorder
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Column_Order";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].columnorder;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property Currency Type
        //                            //Currency Type                                    
        //                            if (stageBuilderStagesModel.grid_items[i].currencyType != null)
        //                            {

        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Decimal",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Currency_Type",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].currencyType
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Decimal";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Currency_Type";
        //                                PropSetToBytes.property_name = "Currency_Type";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].currencyType;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            //Currency Type
        //                            /*if (stageBuilderStagesModel.grid_items[i].currencyType != null)
        //                            {
        //                                ObjectPropertySetID = add.ObjectSetProperty(_Connect, ObjectSetsID, "Check_Box", "Characters", "Currency_Type", "false", "false", "", stageBuilderStagesModel.grid_items[i].currencyType);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Currency_Type";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].currencyType;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }*/

        //                            #region Property Tab Index
        //                            //tab order
        //                            if (stageBuilderStagesModel.grid_items[i].tabindex != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Tab_Index",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].tabindex
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Tab_Index";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].tabindex;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property File
        //                            //image data
        //                            if (stageBuilderStagesModel.grid_items[i].imageData != null)
        //                            {
        //                                ER_DML dml = new ER_DML();
        //                                IOHelper io = new IOHelper();
        //                                string imagename = System.IO.Path.GetFileName(stageBuilderStagesModel.grid_items[i].name);


        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "File",
        //                                    I_PROPERTY_NAME = "ID",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = "Image"
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "File";
        //                                PropSetToBytes.property_name = "ID";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = "Image";
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);


        //                                // DMLH.ADD_ENTRY_OBJECT_DATA(_Connect, Identities_ID, Stages_ID, GripsID, ObjectPropertySetID, "OBJ_PROP_SETS", ObjectPropertySetID, "0", "file", new Object_Value { _File = io.getBytes(stageBuilderStagesModel.grid_items[i].imageData) });
        //                                long? FileID = 0;

        //                                string extension = System.IO.Path.GetExtension(System.IO.Path.GetFileName(imagename));
        //                                if (stageBuilderStagesModel.grid_items[i].imageId == "0")
        //                                {
        //                                    Values.AddFile thisFile = addHelp.ADD_ENTRY_FILE(_Connect, new Values.AddFile
        //                                    {
        //                                        I_FILE_NAME = imagename,
        //                                        I_CONTENT_TYPE = extension,
        //                                        I_FILE_SIZE = io.getBytes(stageBuilderStagesModel.grid_items[i].imageData).Length,
        //                                        I_FILE_DATA = io.getBytes(stageBuilderStagesModel.grid_items[i].imageData)
        //                                    });

        //                                    FileID = thisFile.O_FILES_ID;
        //                                }
        //                                else
        //                                {
        //                                    FileID = ER_Tools.ConvertToInt64(stageBuilderStagesModel.grid_items[i].imageId);
        //                                }
        //                                try
        //                                {
        //                                    addHelp.ADD_FILE_POINT(_Connect, new Values.AddFilePoint
        //                                    {
        //                                        I_FILES_ID = FileID,
        //                                        I_IDENTITIES_ID = SO._IdentityModel.identities_id,
        //                                        I_OBJ_PROP_SETS_ID = ObjectPropertySetID
        //                                    });
        //                                }
        //                                catch
        //                                {
        //                                    //Do Nothing for now.
        //                                }
        //                            }
        //                            #endregion

        //                            #region Property Digits Number
        //                            //digits (Number)
        //                            if (stageBuilderStagesModel.grid_items[i].digits != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Digits",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].digits
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Digits";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].digits;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Status Check Dropdown
        //                            //Status Check (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].status_check != null)
        //                            {

        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Status Check",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].status_check
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Status Check";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].status_check;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property Status Type Dropdown
        //                            //Status Type (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].status_type != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Status Type",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].status_type
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Status Type";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].status_type;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property Status Identity Dropdown
        //                            //Status Identity (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].status_identity != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Status Identity",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].status_identity
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Status Identity";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].status_identity;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property Status Message Dropdown
        //                            //Status Message (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].status_check != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Status Message",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].status_message
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Status Message";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].status_message;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Bind Check
        //                            //Bind Check (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].bind_check != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Bind Check",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].bind_check
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Bind Check";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].bind_check;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property Roles Check
        //                            //Roles Check (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].roles_check != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Roles Check",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].roles_check
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Roles Check";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].roles_check;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property Roles Collections
        //                            //Roles Collections (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].roles_collections != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Roles Collections",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].roles_collections
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Roles Collections";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].roles_collections;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property Roles User
        //                            //Roles User (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].roles_user != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Roles User",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].roles_user
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Roles User";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].roles_user;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property Bind Stage Dropdown
        //                            //Bind Stage (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].bind_stage != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Bind Stage",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].bind_stage
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Bind Stage";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].bind_stage;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property Bind Objects Dropdown
        //                            //Bind Objects (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].bind_objects != null)
        //                            {
        //                                for (int objects_i = 0; objects_i < stageBuilderStagesModel.grid_items[i].bind_objects.Count; objects_i++)
        //                                {
        //                                    thisPropSet = new Values.AddObjectPropertySet
        //                                    {
        //                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                        I_OBJECT_PROP_TYPE = "Text",
        //                                        I_VALUE_DATATYPE = "Characters",
        //                                        I_PROPERTY_NAME = "Bind Objects",
        //                                        I_HAS_PARENT = "false",
        //                                        I_HAS_CHILD = "false",
        //                                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].bind_objects[objects_i]
        //                                    };

        //                                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                    PropSetToBytes = new ViewObjectPropSetsModel();
        //                                    PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                    PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                    PropSetToBytes.object_type = "Text";
        //                                    PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                    PropSetToBytes.value_datatype = "Characters";
        //                                    PropSetToBytes.property_name = "Bind Objects";
        //                                    PropSetToBytes.has_parent = "false";
        //                                    PropSetToBytes.has_child = "false";
        //                                    PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                    PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].bind_objects[objects_i];
        //                                    _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                }
        //                            }
        //                            #endregion

        //                            #region Property View Table Stage
        //                            //View Table Stage (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].view_table_stage != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "View_Table_Stage",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].view_table_stage
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "View_Table_Stage";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].view_table_stage;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property View Table Filter Dropdown
        //                            //View Table Filter (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].view_table_filter != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "View_Table_Filter",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].view_table_filter
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "View_Table_Filter";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].view_table_filter;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

        //                            }
        //                            #endregion

        //                            #region Property View Table Columns Dropdown
        //                            //View Table Columns (Dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].view_table_columns != null)
        //                            {
        //                                for (int objects_i = 0; objects_i < stageBuilderStagesModel.grid_items[i].view_table_columns.Count; objects_i++)
        //                                {
        //                                    thisPropSet = new Values.AddObjectPropertySet
        //                                    {
        //                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                        I_OBJECT_PROP_TYPE = "Text",
        //                                        I_VALUE_DATATYPE = "Characters",
        //                                        I_PROPERTY_NAME = "View_Table_Columns",
        //                                        I_HAS_PARENT = "false",
        //                                        I_HAS_CHILD = "false",
        //                                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].view_table_columns[objects_i]
        //                                    };

        //                                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                    PropSetToBytes = new ViewObjectPropSetsModel();
        //                                    PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                    PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                    PropSetToBytes.object_type = "Text";
        //                                    PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                    PropSetToBytes.value_datatype = "Characters";
        //                                    PropSetToBytes.property_name = "View_Table_Columns";
        //                                    PropSetToBytes.has_parent = "false";
        //                                    PropSetToBytes.has_child = "false";
        //                                    PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                    PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].view_table_columns[objects_i];
        //                                    _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                }
        //                            }
        //                            #endregion

        //                            #region Property Total Object
        //                            //total (Total Object)
        //                            if (stageBuilderStagesModel.grid_items[i].num_objects != null)
        //                            {
        //                                string objects = "";
        //                                for (int _count = 0; _count < stageBuilderStagesModel.grid_items[i].num_objects.Count; _count++)
        //                                {
        //                                    objects += stageBuilderStagesModel.grid_items[i].num_objects[_count].Trim();
        //                                    if (_count != (stageBuilderStagesModel.grid_items[i].num_objects.Count - 1))
        //                                        objects += ",";
        //                                }

        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Total",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Total",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = objects
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = thisPropSet.O_OBJ_PROP_SETS_ID;
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Total";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Total";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = objects;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property email
        //                            //email 
        //                            if (stageBuilderStagesModel.grid_items[i].email != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Email",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].email
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Email";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].email;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property URL
        //                            //URL 
        //                            if (stageBuilderStagesModel.grid_items[i].url != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "URL",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].url
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "URL";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].url;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Date
        //                            //date 
        //                            if (stageBuilderStagesModel.grid_items[i].date != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Date",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].date
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Date";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].date;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Time
        //                            //time 
        //                            if (stageBuilderStagesModel.grid_items[i].time != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Time",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].time
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Time";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].time;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Phone
        //                            //phone 
        //                            if (stageBuilderStagesModel.grid_items[i].phone != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Phone",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].phone
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Phone";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].phone;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Height
        //                            //height
        //                            if (stageBuilderStagesModel.grid_items[i].height != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Height",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].height
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Height";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].height;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Width
        //                            //width
        //                            if (stageBuilderStagesModel.grid_items[i].width != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Width",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].width
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Width";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].width;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Button Border
        //                            //button border
        //                            if (stageBuilderStagesModel.grid_items[i].button_border != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Button Border",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].button_border
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Button Border";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_border;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Button BG Color
        //                            //button bgcolor
        //                            if (stageBuilderStagesModel.grid_items[i].button_bgcolor != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Button BG Color",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].button_bgcolor
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Button BG Color";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_bgcolor;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Button Font Color
        //                            //button fontcolor
        //                            if (stageBuilderStagesModel.grid_items[i].button_fontcolor != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Button Font Color",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].button_fontcolor
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Button Font Color";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_fontcolor;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Button Type
        //                            //button type
        //                            if (stageBuilderStagesModel.grid_items[i].button_type != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Button Type",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].button_type
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Button Type";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_type;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Button Stage
        //                            //button stage
        //                            if (stageBuilderStagesModel.grid_items[i].button_stage != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Button Stage",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].button_stage
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Button Stage";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_stage;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Button Submit
        //                            //button submit
        //                            if (stageBuilderStagesModel.grid_items[i].button_submit != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Button Submit",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].button_submit
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Button Submit";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_submit;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Button Send
        //                            //button send
        //                            if (stageBuilderStagesModel.grid_items[i].button_send != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Button Send",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].button_send
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Button Send";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_send;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Style
        //                            //style
        //                            if (stageBuilderStagesModel.grid_items[i].button_border != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Style",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].style
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Style";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].style;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Link Type
        //                            //link type
        //                            if (stageBuilderStagesModel.grid_items[i].link_type != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Link Type",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].link_type
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Link Type";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].link_type;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Link Stage
        //                            //link stage
        //                            if (stageBuilderStagesModel.grid_items[i].link_stage != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Link Stage",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].link_stage
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Link Stage";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].link_stage;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Link Target
        //                            //link target
        //                            if (stageBuilderStagesModel.grid_items[i].link_target != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Link Target",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].link_target
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Link Target";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].link_target;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Link URL
        //                            //link url
        //                            if (stageBuilderStagesModel.grid_items[i].link_url != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Link Url",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].link_url
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Link Url";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].link_url;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Show Label
        //                            //show label
        //                            if (stageBuilderStagesModel.grid_items[i].show_label != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "ShowLabel",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].show_label
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "ShowLabel";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].show_label;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            //field required 
        //                            if (stageBuilderStagesModel.grid_items[i].required != null && stageBuilderStagesModel.grid_items[i].required.ToString().ToLower() == "true")
        //                            {
        //                                #region Property Required
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Required",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = "true"
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Required";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = "true";
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                #endregion

        //                                #region Property Required Message
        //                                if (stageBuilderStagesModel.grid_items[i].req_message != null)
        //                                {
        //                                    thisPropSet = new Values.AddObjectPropertySet
        //                                    {
        //                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                        I_OBJECT_PROP_TYPE = "Text_Box",
        //                                        I_VALUE_DATATYPE = "Characters",
        //                                        I_PROPERTY_NAME = "Required Message",
        //                                        I_HAS_PARENT = "false",
        //                                        I_HAS_CHILD = "false",
        //                                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].req_message
        //                                    };

        //                                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                    PropSetToBytes = new ViewObjectPropSetsModel();
        //                                    PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                    PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                    PropSetToBytes.object_type = "Text_Box";
        //                                    PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                    PropSetToBytes.value_datatype = "Characters";
        //                                    PropSetToBytes.property_name = "Required Message";
        //                                    PropSetToBytes.has_parent = "false";
        //                                    PropSetToBytes.has_child = "false";
        //                                    PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                    PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].req_message;
        //                                    _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                }
        //                                #endregion
        //                            }

        //                            //reg expression
        //                            if (stageBuilderStagesModel.grid_items[i].reg_exp != null && stageBuilderStagesModel.grid_items[i].reg_exp.ToString().ToLower() == "true")
        //                            {
        //                                #region Property Regular Expression
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Check_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Regular Expression",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].reg_exp
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Check_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Regular Expression";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].reg_exp;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                #endregion

        //                                #region Property Regular Expression Pattern
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Regular Expression Pattern",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].pattern
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Regular Expression Pattern";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].pattern;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                #endregion

        //                                #region Pattern Regular Expression Message
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Regular Expression Message",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].pattern_message
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Regular Expression Message";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].pattern_message;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                #endregion

        //                            }

        //                            #region Property Min
        //                            //min 
        //                            if (stageBuilderStagesModel.grid_items[i].min != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Min",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].min
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Min";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].min;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Max
        //                            //max
        //                            if (stageBuilderStagesModel.grid_items[i].max != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Max",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].max
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Max";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].max;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Format
        //                            //format
        //                            if (stageBuilderStagesModel.grid_items[i].format != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "Format",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].format
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "Format";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].format;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Field Size
        //                            //field size
        //                            if (stageBuilderStagesModel.grid_items[i].field_size != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Drop_Down",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "FieldSize",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].field_size
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Drop_Down";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "FieldSize";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].field_size;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            #region Property Field Height
        //                            //field height
        //                            if (stageBuilderStagesModel.grid_items[i].field_height != null)
        //                            {
        //                                thisPropSet = new Values.AddObjectPropertySet
        //                                {
        //                                    I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                    I_OBJECT_PROP_TYPE = "Text_Box",
        //                                    I_VALUE_DATATYPE = "Characters",
        //                                    I_PROPERTY_NAME = "FieldHeight",
        //                                    I_HAS_PARENT = "false",
        //                                    I_HAS_CHILD = "false",
        //                                    I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].field_height
        //                                };

        //                                thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                PropSetToBytes = new ViewObjectPropSetsModel();
        //                                PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                PropSetToBytes.object_type = "Text_Box";
        //                                PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                PropSetToBytes.value_datatype = "Characters";
        //                                PropSetToBytes.property_name = "FieldHeight";
        //                                PropSetToBytes.has_parent = "false";
        //                                PropSetToBytes.has_child = "false";
        //                                PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].field_height;
        //                                _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                            }
        //                            #endregion

        //                            //conditions rules
        //                            if (stageBuilderStagesModel.grid_items[i].conditions != null)
        //                            {
        //                                conCount = stageBuilderStagesModel.grid_items[i].conditions.Count;
        //                                if (conCount > 0)
        //                                {
        //                                    #region Property Condition Action
        //                                    thisPropSet = new Values.AddObjectPropertySet
        //                                    {
        //                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                        I_OBJECT_PROP_TYPE = "Drop_Down",
        //                                        I_VALUE_DATATYPE = "Characters",
        //                                        I_PROPERTY_NAME = "Condition Action",
        //                                        I_HAS_PARENT = "false",
        //                                        I_HAS_CHILD = "false",
        //                                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].con_action
        //                                    };

        //                                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                    PropSetToBytes = new ViewObjectPropSetsModel();
        //                                    PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                    PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                    PropSetToBytes.object_type = "Drop_Down";
        //                                    PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                    PropSetToBytes.value_datatype = "Characters";
        //                                    PropSetToBytes.property_name = "Condition Action";
        //                                    PropSetToBytes.has_parent = "false";
        //                                    PropSetToBytes.has_child = "false";
        //                                    PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                    PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].con_action;
        //                                    _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                    #endregion

        //                                    #region Property Condition Logic
        //                                    thisPropSet = new Values.AddObjectPropertySet
        //                                    {
        //                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                        I_OBJECT_PROP_TYPE = "Drop_Down",
        //                                        I_VALUE_DATATYPE = "Characters",
        //                                        I_PROPERTY_NAME = "Condition Logic",
        //                                        I_HAS_PARENT = "false",
        //                                        I_HAS_CHILD = "false",
        //                                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].con_logic
        //                                    };

        //                                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                    PropSetToBytes = new ViewObjectPropSetsModel();
        //                                    PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                    PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                    PropSetToBytes.object_type = "Drop_Down";
        //                                    PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                    PropSetToBytes.value_datatype = "Characters";
        //                                    PropSetToBytes.property_name = "Condition Logic";
        //                                    PropSetToBytes.has_parent = "false";
        //                                    PropSetToBytes.has_child = "false";
        //                                    PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                    PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].con_logic;
        //                                    _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                    #endregion

        //                                    for (int b = 0; b < conCount; b++)
        //                                    {
        //                                        #region Property Condition ID #
        //                                        thisPropSet = new Values.AddObjectPropertySet
        //                                        {
        //                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                            I_OBJECT_PROP_TYPE = "Text",
        //                                            I_VALUE_DATATYPE = "Characters",
        //                                            I_PROPERTY_NAME = "Condition ID " + b,
        //                                            I_HAS_PARENT = "false",
        //                                            I_HAS_CHILD = "false",
        //                                            I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                            I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].conditions[b].con_id
        //                                        };

        //                                        thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                        PropSetToBytes = new ViewObjectPropSetsModel();
        //                                        PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                        PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                        PropSetToBytes.object_type = "Text";
        //                                        PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                        PropSetToBytes.value_datatype = "Characters";
        //                                        PropSetToBytes.property_name = "Condition ID " + b;
        //                                        PropSetToBytes.has_parent = "false";
        //                                        PropSetToBytes.has_child = "false";
        //                                        PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                        PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].conditions[b].con_id;
        //                                        _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                        #endregion

        //                                        #region Property Condition Field #
        //                                        thisPropSet = new Values.AddObjectPropertySet
        //                                        {
        //                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                            I_OBJECT_PROP_TYPE = "Text_Box",
        //                                            I_VALUE_DATATYPE = "Characters",
        //                                            I_PROPERTY_NAME = "Condition Field " + b,
        //                                            I_HAS_PARENT = "false",
        //                                            I_HAS_CHILD = "false",
        //                                            I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                            I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].conditions[b].con_field
        //                                        };

        //                                        thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                        PropSetToBytes = new ViewObjectPropSetsModel();
        //                                        PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                        PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                        PropSetToBytes.object_type = "Text_Box";
        //                                        PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                        PropSetToBytes.value_datatype = "Characters";
        //                                        PropSetToBytes.property_name = "Condition Field " + b;
        //                                        PropSetToBytes.has_parent = "false";
        //                                        PropSetToBytes.has_child = "false";
        //                                        PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                        PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].conditions[b].con_field;
        //                                        _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                        #endregion

        //                                        #region Property Condition Operator #
        //                                        thisPropSet = new Values.AddObjectPropertySet
        //                                        {
        //                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                            I_OBJECT_PROP_TYPE = "Drop_Down",
        //                                            I_VALUE_DATATYPE = "Characters",
        //                                            I_PROPERTY_NAME = "Condition Operator " + b,
        //                                            I_HAS_PARENT = "false",
        //                                            I_HAS_CHILD = "false",
        //                                            I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                            I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].conditions[b].con_operator
        //                                        };

        //                                        thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                        PropSetToBytes = new ViewObjectPropSetsModel();
        //                                        PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                        PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                        PropSetToBytes.object_type = "Drop_Down";
        //                                        PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                        PropSetToBytes.value_datatype = "Characters";
        //                                        PropSetToBytes.property_name = "Condition Operator " + b;
        //                                        PropSetToBytes.has_parent = "false";
        //                                        PropSetToBytes.has_child = "false";
        //                                        PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                        PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].conditions[b].con_operator;
        //                                        _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                        #endregion

        //                                        #region Property Condition Option #
        //                                        thisPropSet = new Values.AddObjectPropertySet
        //                                        {
        //                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                            I_OBJECT_PROP_TYPE = "Text",
        //                                            I_VALUE_DATATYPE = "Characters",
        //                                            I_PROPERTY_NAME = "Condition Option " + b,
        //                                            I_HAS_PARENT = "false",
        //                                            I_HAS_CHILD = "false",
        //                                            I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                            I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[i].conditions[b].con_option
        //                                        };

        //                                        thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                        PropSetToBytes = new ViewObjectPropSetsModel();
        //                                        PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                        PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                        PropSetToBytes.object_type = "Text";
        //                                        PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
        //                                        PropSetToBytes.value_datatype = "Characters";
        //                                        PropSetToBytes.property_name = "Condition Option " + b;
        //                                        PropSetToBytes.has_parent = "false";
        //                                        PropSetToBytes.has_child = "false";
        //                                        PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                        PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].conditions[b].con_option;
        //                                        _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                        #endregion
        //                                    }
        //                                }
        //                            }

        //                            //options (for checkboxgroup, radiobuttons, dropdown)
        //                            if (stageBuilderStagesModel.grid_items[i].options != null)
        //                            {
        //                                conCount = stageBuilderStagesModel.grid_items[i].options.Count;

        //                                if (conCount > 0)
        //                                {
        //                                    #region Property Value
        //                                    thisPropSet = new Values.AddObjectPropertySet
        //                                    {
        //                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
        //                                        I_OBJECT_PROP_TYPE = "Items",
        //                                        I_VALUE_DATATYPE = "Characters",
        //                                        I_PROPERTY_NAME = "Value",
        //                                        I_HAS_PARENT = "false",
        //                                        I_HAS_CHILD = "false",
        //                                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                                        I_PROPERTY_VALUE = fieldID //stageBuilderStagesModel.grid_items[i].options[0].name);
        //                                    };

        //                                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);

        //                                    PropSetToBytes = new ViewObjectPropSetsModel();
        //                                    PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //                                    PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //                                    PropSetToBytes.object_type = "Items";
        //                                    PropSetToBytes.object_prop_type = "Items";
        //                                    PropSetToBytes.value_datatype = "Characters";
        //                                    PropSetToBytes.property_name = "Value";
        //                                    PropSetToBytes.has_parent = "false";
        //                                    PropSetToBytes.has_child = "false";
        //                                    PropSetToBytes.parent_obj_prop_sets_id = 0;
        //                                    PropSetToBytes.property_value = fieldID;
        //                                    #endregion

        //                                    PropSetToBytes.ObjectPropOptSets = new List<ViewObjectPropOptSetsModel>();
        //                                    ViewObjectPropOptSetsModel _PropertyOptionToBytes = new ViewObjectPropOptSetsModel();
        //                                    for (int b = 0; b < conCount; b++)
        //                                    {
        //                                        #region Property Option
        //                                        ObjectPropertyOptionSetID = addHelp.addObjectSetPropertyOption(_Connect, new Values.AddObjectPropertyOption { I_OBJ_PROP_SETS_ID = ObjectPropertySetID, I_OPTION_VALUE = stageBuilderStagesModel.grid_items[i].options[b].name });

        //                                        _PropertyOptionToBytes = new ViewObjectPropOptSetsModel();
        //                                        _PropertyOptionToBytes.obj_prop_opt_sets_id = ObjectPropertyOptionSetID.O_OBJ_PROP_OPT_SETS_ID;
        //                                        _PropertyOptionToBytes.obj_prop_sets_id = ObjectPropertySetID;
        //                                        _PropertyOptionToBytes.option_value = stageBuilderStagesModel.grid_items[i].options[b].name;
        //                                        _PropertyOptionToBytes.option_name = stageBuilderStagesModel.grid_items[i].options[b].value;


        //                                        PropSetToBytes.ObjectPropOptSets.Add(_PropertyOptionToBytes);

        //                                        #endregion
        //                                    }

        //                                    _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //                                }
        //                            }

        //                            GripToBytes.ObjectSets.Add(_ObjectSetToBytes);
        //                        }
        //                    }
        //                    #endregion
        //                }

        //                //Add Grip to GripsList
        //                _StageToBytes.Grips.Add(GripToBytes);

        //                // Adds the Grid Grip to the stage
        //                _StageToBytes.Grips.Add(GridGripToBytes);
        //                #region SAVE CACHE Array
        //                try
        //                {

        //                    var listOfArrays = _StagesToBytes;

        //                    ER_Tools _tools = new ER_Tools();

        //                    List<ViewStageModel> temp = new List<ViewStageModel>();

        //                    temp.Add(_StageToBytes);

        //                    byte[] StageBytes = _tools.ListToBytes(_StageToBytes);
        //                    //ViewStageModel reverse = (ViewStageModel)_tools.ByteArrayToObject(StageBytes);

        //                    //Array TempArray = _StagesToBytes.ToArray();

        //                    //byte[] StageBytes = ER_Tools._ListToBytes(TempArray);
        //                    #region Grip Stage Cache
        //                    Values.AddGrip thisCacheGrip = new Values.AddGrip
        //                    {
        //                        I_STAGES_UUID = thisStage.O_STAGES_UUID,
        //                        I_STAGES_ID = thisStage.O_STAGES_ID,
        //                        I_STAGE_TYPE = "Form",
        //                        I_STAGE_NAME = stageBuilderStagesModel.stage_name,
        //                        I_GRIP_TYPE = "Cache",
        //                        I_GRIP_NAME = "Stage Cache",
        //                        I_CONTAINERS_ID = Organizations_ID,
        //                        I_IDENTITIES_ID = _identity_id
        //                    };

        //                    thisCacheGrip = addHelp.GripWithPermissions(_Connect, thisCacheGrip);
        //                    GripsID = thisCacheGrip.O_GRIPS_ID;

        //                    GripToBytes.stages_id = thisStage.O_STAGES_ID;
        //                    GripToBytes.stage_type = "Form";
        //                    GripToBytes.stage_name = stageBuilderStagesModel.stage_name;
        //                    GripToBytes.grip_type = "Form";
        //                    GripToBytes.grip_name = "Stage Cache";
        //                    GripToBytes.containers_id = Organizations_ID;
        //                    GripToBytes.identities_id = _identity_id;
        //                    GripToBytes.ObjectSets = new List<ViewObjectSetModel>();
        //                    #endregion

        //                    #region Object Stage Cache
        //                    //Run Patch for this object
        //                    Values.AddObjectSet objectSetCache = new Values.AddObjectSet
        //                    {
        //                        I_GRIPS_ID = thisCacheGrip.O_GRIPS_ID,
        //                        I_STAGE_TYPE = "Cache",
        //                        I_STAGE_NAME = stageBuilderStagesModel.stage_name,
        //                        I_GRIP_TYPE = "Cache",
        //                        I_GRIP_NAME = "Stage Cache",
        //                        I_OBJECT_TYPE = "Cache",
        //                        I_CONTAINERS_ID = Organizations_ID,
        //                        I_IDENTITIES_ID = _identity_id
        //                    };
        //                    objectSetCache = addHelp.ObjectSetwithPermission(_Connect, objectSetCache);
        //                    ObjectSetsID = objectSetCache.O_OBJECT_SETS_ID;
        //                    #endregion

        //                    #region Property Stage Cache
        //                    thisPropSet = new Values.AddObjectPropertySet
        //                    {
        //                        I_OBJECT_SETS_ID = objectSetCache.O_OBJECT_SETS_ID,
        //                        I_OBJECT_PROP_TYPE = "Cache",
        //                        I_VALUE_DATATYPE = "File",
        //                        I_PROPERTY_NAME = "Stage Cache",
        //                        I_HAS_PARENT = "false",
        //                        I_HAS_CHILD = "false",
        //                        I_PARENT_OBJ_PROP_SETS_ID = null,
        //                        I_PROPERTY_VALUE = "Cache"
        //                    };

        //                    thisPropSet = addHelp.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //                    #endregion

        //                    DMLHelper DMLH = new DMLHelper();
        //                    string CacheResult = DMLH.ADD_ENTRY_OBJECT_DATA(_Connect,
        //                        Identities_ID,
        //                        thisStage.O_STAGES_ID,
        //                        thisCacheGrip.O_GRIPS_ID,
        //                        objectSetCache.O_OBJECT_SETS_ID,
        //                        thisPropSet.O_OBJ_PROP_SETS_ID,
        //                        "OBJ_PROP_SETS",
        //                        ObjectPropertySetID,
        //                        0,
        //                        "file",
        //                        new Object_Value
        //                        {
        //                            _File = new File_Object
        //                            {
        //                                _FileBytes = StageBytes,
        //                                FILE_NAME = stageBuilderStagesModel.stage_name + "_CACHE",
        //                                FILE_SIZE = StageBytes.Length,
        //                                CONTENT_TYPE = ".ER"
        //                            }
        //                        },
        //                        true
        //                    );

        //                    //byte[] array = listOfArrays.SelectMany(a => a).ToArray();

        //                    //byte[] StageBytes = _StagesToBytes.SelectMany(s => Encoding.UTF8.GetBytes(s)).ToArray();
        //                }
        //                catch (Exception e)
        //                {
        //                    e.ToString();
        //                }
        //                #endregion

        //                ER_Generate er_gen = new ER_Generate();

        //                er_gen.GENERATE_STAGEPROP_VIEW(_Connect, thisStage.O_STAGES_ID.ToString());
        //            }
        //            #endregion
        //        }
        //    }
        //    return errmsg;
        //}

        #endregion
        //TODO: Review and Revisit
        public SaveStruct SaveStageBuilderNew(IConnectToDB _Connect, StageBuilderModel stageBuilderModel, String JSON, Guid? parent_id, long? identities_id, SessionObjects SO)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            SaveStruct thisResult = new SaveStruct();
            ER_Generate gen = new ER_Generate();

            if (stageBuilderModel != null)
            {
                #region VARIABLES
                add add = new add();
                long? stages_count = stageBuilderModel.stage.Count;
                AppHelper appHelper = new AppHelper();
                ER_Tools er_tools = new ER_Tools();
                SecurityHelper SECH = new SecurityHelper();
                add addHelp = new add();
                //string appid = "";
                long? coreid = -1;
                bool containerIsValid = false;
                Guid? thisContainerUUID = ER_Tools.ConvertToGuid(stageBuilderModel.app_container);
                string description = "";
                ER_DML er_dml = new ER_DML();
                ER_DDL er_ddl = new ER_DDL();
                CASTGOOP.ObjectPropSets thisPropSet = new CASTGOOP.ObjectPropSets();

                long? _identity_id = identities_id;

                System.Xml.Linq.XNode node = Newtonsoft.Json.JsonConvert.DeserializeXNode(@JSON, "Root");

                string app_background_color = "#FFF;";

                stageBuilderModel.app_name = stageBuilderModel.app_name.Trim();

                bool CanUseAppName = appHelper.CanUseAppName(_Connect, stageBuilderModel.app_name, stageBuilderModel.cores_uuid, parent_id);

                if (thisContainerUUID != null)
                {
                    #region Run Query to Convert UUID to ID
                    List<DynamicModels.RootReportFilter> rootAppFilters = new List<DynamicModels.RootReportFilter>();
                    rootAppFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CONTAINERS_UUID_", ParamValue = thisContainerUUID });
                    rootAppFilters.Add(new DynamicModels.RootReportFilter { FilterName = "GET_LATEST", ParamValue = "T" });

                    bool runContainerQuery = thisContainerUUID == null ? false : true;
                    DataTable GetContainerData = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CONTAINERS_SEARCH",
                        new DataTableDotNetModelMetaData { columns = "", length = -1, order = "1 asc", start = 0, verify = (runContainerQuery ? "T" : "F") },
                        rootAppFilters);

                    DataColumnCollection containerDCC = GetContainerData.Columns;
                    #endregion

                    if (containerDCC.Contains("CONTAINER_NAME") && GetContainerData.Rows.Count > 0)
                    {
                        containerIsValid = true;
                    }
                }

                if (containerIsValid)
                {
                    List<DynamicModels.RootReportFilter> coreFilters = new List<DynamicModels.RootReportFilter>();

                    coreFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.UniqueIdentifier, ParamValue = stageBuilderModel.cores_uuid });

                    DataTable CoreFetch = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_SEARCH",
                            new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                            coreFilters);

                    DataColumnCollection coreDCC = CoreFetch.Columns;

                    if (coreDCC.Contains("CORES_ID") && CoreFetch.Rows.Count > 0)
                    {
                        coreid = CoreFetch.Rows[0].Field<long?>("CORES_ID");
                    }

                    if (coreid > 0 && CanUseAppName)
                    {
                        if (stageBuilderModel.app_description != null)
                        {
                            description = stageBuilderModel.app_description;
                        }

                        if (stageBuilderModel.app_background_color != null)
                        {
                            app_background_color = stageBuilderModel.app_background_color;
                        }

                        sqlTransBlocks _Transaction = new sqlTransBlocks();
                        _Transaction.Series = new List<SQLTrasaction>();
                        #endregion

                        #region SAVE APPLICATION

                        CASTGOOP.Application thisApplication = new CASTGOOP.Application
                        {
                            I_CORES_ID = coreid,
                            I_CORES_UUID = stageBuilderModel.cores_uuid,
                            I_IDENTITIES_ID = identities_id,
                            I_APPLICATION_NAME = stageBuilderModel.app_name,
                            I_THIS_CALLER = stageBuilderModel.i_this_caller != null ? stageBuilderModel.i_this_caller.ToString() : "",
                            I_APPLICATION_LINK = stageBuilderModel.app_name.Trim().Replace(" ", "-"),
                            I_APPLICATION_DESCRIPTION = stageBuilderModel.app_description,
                            //I_BACKGROUND_COLOR = app_background_color,
                            I_BASE_APPLICATIONS_UUID = parent_id == null ? null : parent_id,
                            I_RENDITION = "0",
                            I_CONTAINERS_UUID = thisContainerUUID
                        };

                        if (parent_id != null)
                        {
                            string parent_name = appHelper.GetRootAppNameViaBaseUUID(_Connect, parent_id.ToString());
                            string rendition = GetAppRenditionViaUUID(_Connect, parent_id.ToString());

                            thisApplication.I_ROOT_APPLICATION = parent_name;
                            thisApplication.I_RENDITION = rendition;

                            //thisApplication = add.Application(_Connect, thisApplication);
                            _Transaction = add.ADD_ENTRY_Application(_Connect, thisApplication, _Transaction);

                            //SECH.LogActivity(_Connect, "Add Object", coreid.ToString(), thisApplication.application_id.ToString(), "APPLICATIONS", thisApplication.application_id.ToString(), identities_id, "1005", "1007", (stageBuilderModel.app_name + " updated by " + System.Web.HttpContext.Current.Session["UserName"].ToString().ToUpper()), "1000", "1000", "");
                            CASTGOOP.Activity thisActivity = new CASTGOOP.Activity
                            {
                                I_OBJECT_TYPE = "Add Object",
                                I_CORES_ID = coreid,
                                I_APPLICATIONS_ID = "@T_APPLICATIONS_ID",
                                I_TABLE_SOURCE = "APPLICATIONS",
                                I_TABLE_ID = "@T_APPLICATIONS_ID",
                                I_IDENTITIES_ID = identities_id,
                                I_VARIANTS_ID = 1005,
                                I_SYMBOLS_ID = 1007,
                                I_DESC_TEXT = (stageBuilderModel.app_name + " updated by " + Current.Session.GetString("UserName").ToUpper()),
                                I_DESC_VARIANTS_ID = 1000,
                                I_DESC_SYMBOLS_ID = 1000,
                                I_DESC_META_TEXT = "",
                                I_THIS_CALLER = Guid.NewGuid().ToString()
                            };

                            _Transaction = add.Activity(_Connect, thisActivity, _Transaction);
                        }
                        else
                        {

                            if (stageBuilderModel.rendition == null)
                            {
                                thisApplication.I_RENDITION = 0;
                                thisApplication.I_ROOT_APPLICATION = stageBuilderModel.app_name;

                                //thisApplication = add.Application(_Connect, thisApplication);
                                _Transaction = add.ADD_ENTRY_Application(_Connect, thisApplication, _Transaction);

                                CASTGOOP.Activity thisActivity = new CASTGOOP.Activity
                                {
                                    I_OBJECT_TYPE = "Add Object",
                                    I_CORES_ID = coreid,
                                    I_APPLICATIONS_ID = "@T_APPLICATIONS_ID",
                                    I_TABLE_SOURCE = "APPLICATIONS",
                                    I_TABLE_ID = "@T_APPLICATIONS_ID",
                                    I_IDENTITIES_ID = identities_id,
                                    I_VARIANTS_ID = 1006,
                                    I_SYMBOLS_ID = 1007,
                                    I_DESC_TEXT = (stageBuilderModel.app_name + " created by " + Current.Session.GetString("UserName").ToUpper()),
                                    I_DESC_VARIANTS_ID = 1000,
                                    I_DESC_SYMBOLS_ID = 1000,
                                    I_DESC_META_TEXT = "",
                                    I_THIS_CALLER = Guid.NewGuid().ToString()
                                };

                                _Transaction = add.Activity(_Connect, thisActivity, _Transaction);

                            }
                            else
                            {
                                thisApplication.I_RENDITION = stageBuilderModel.rendition;
                                thisApplication.I_ROOT_APPLICATION = stageBuilderModel.app_name;

                                //thisApplication = add.Application(_Connect, thisApplication);
                                _Transaction = add.ADD_ENTRY_Application(_Connect, thisApplication, _Transaction);

                                if (stageBuilderModel.rendition == 0)
                                {
                                    //SECH.LogActivity(_Connect, "Add Object", coreid.ToString(), thisApplication.application_id.ToString(), "APPLICATIONS", thisApplication.application_id.ToString(), identities_id, "1006", "1007", (stageBuilderModel.app_name + " created by " + System.Web.HttpContext.Current.Session["UserName"].ToString().ToUpper()), "1000", "1000", "");
                                    CASTGOOP.Activity thisActivity = new CASTGOOP.Activity
                                    {
                                        I_OBJECT_TYPE = "Add Object",
                                        I_CORES_ID = coreid,
                                        I_APPLICATIONS_ID = "@T_APPLICATIONS_ID",
                                        I_TABLE_SOURCE = "APPLICATIONS",
                                        I_TABLE_ID = "@T_APPLICATIONS_ID",
                                        I_IDENTITIES_ID = identities_id,
                                        I_VARIANTS_ID = 1006,
                                        I_SYMBOLS_ID = 1007,
                                        I_DESC_TEXT = (stageBuilderModel.app_name + " created by " + Current.Session.GetString("UserName").ToUpper()),
                                        I_DESC_VARIANTS_ID = 1000,
                                        I_DESC_SYMBOLS_ID = 1000,
                                        I_DESC_META_TEXT = "",
                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                    };

                                    _Transaction = add.Activity(_Connect, thisActivity, _Transaction);
                                }
                                else
                                {
                                    //SECH.LogActivity(_Connect, "Add Object", coreid.ToString(), thisApplication.application_id.ToString(), "APPLICATIONS", thisApplication.application_id.ToString(), identities_id, "1005", "1007", (stageBuilderModel.app_name + " updated by " + System.Web.HttpContext.Current.Session["UserName"].ToString().ToUpper()), "1000", "1000", "");
                                    CASTGOOP.Activity thisActivity = new CASTGOOP.Activity
                                    {
                                        I_OBJECT_TYPE = "Add Object",
                                        I_CORES_ID = coreid,
                                        I_APPLICATIONS_ID = "@T_APPLICATIONS_ID",
                                        I_TABLE_SOURCE = "APPLICATIONS",
                                        I_TABLE_ID = "@T_APPLICATIONS_ID",
                                        I_IDENTITIES_ID = identities_id,
                                        I_VARIANTS_ID = 1005,
                                        I_SYMBOLS_ID = 1007,
                                        I_DESC_TEXT = (stageBuilderModel.app_name + " updated by " + Current.Session.GetString("UserName").ToUpper()),
                                        I_DESC_VARIANTS_ID = 1000,
                                        I_DESC_SYMBOLS_ID = 1000,
                                        I_DESC_META_TEXT = "",
                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                    };

                                    _Transaction = add.Activity(_Connect, thisActivity, _Transaction);
                                }
                            }
                        }

                    if (stageBuilderModel.app_roles != null)
                        {

                            foreach (string returnedRole in stageBuilderModel.app_roles)
                            {
                                CASTGOOP.AddApplicationRole thisAppRole = new CASTGOOP.AddApplicationRole
                                {
                                    I_THIS_CALLER = Guid.NewGuid().ToString(),
                                    I_APPLICATIONS_UUID = "@T_APPLICATIONS_UUID",
                                    I_ROLES_UUID = returnedRole
                                };

                                _Transaction = add.ADD_APPLICATION_ROLE(_Connect, thisAppRole, _Transaction);
                            }
                        }

                        #endregion

                        List<string> findTheseUUIDsLate = new List<string>();
                        StageBuilderStagesModel stageBuilderStagesModelPost = null;

                        List<ViewStageModel> _StagesToBytes = new List<ViewStageModel>();

                        #region LOOP THROUGH STAGES
                        for (int s = 0; s < stages_count; s++)
                        {
                            StageBuilderStagesModel stageBuilderStagesModel = stageBuilderModel.stage[s];

                            //Instantiate Stage Model for Byte Save.

                            string /*Stages_ID = "",*/ Stages_Type = "Form";
                            long? ObjectPropertySetID = null;
                            Guid? ObjectPropertySetUUID = null;
                            long? Identities_ID = identities_id;
                            string stageName = stageBuilderStagesModel.stage_name;
                            int numberOfObjectsinGrid = stageBuilderStagesModel.grid_items.Count;

                            string i_this_caller = stageBuilderStagesModel.i_this_caller.ToString();
                            var thisStageBaseUUID = stageBuilderStagesModel.base_stages_uuid;
                            bool isNewStageObject = thisStageBaseUUID.ToString().Contains("NEW-BASE-") ? true : false;
                            if (!string.IsNullOrWhiteSpace(i_this_caller))
                            {
                                findTheseUUIDsLate.Add(i_this_caller);
                            }

                            #region STAGE FROM BUILDER
                            CASTGOOP.Stage thisStage = new CASTGOOP.Stage
                            {
                                I_BASE_STAGES_UUID = isNewStageObject ? null : stageBuilderStagesModel.base_stages_uuid,
                                I_PREV_STAGES_UUID = isNewStageObject ? null : stageBuilderStagesModel.prev_stages_uuid,
                                I_STAGE_TYPE = Stages_Type,
                                I_STAGE_NAME = stageBuilderStagesModel.stage_name,
                                I_APPLICATIONS_ID = "@T_APPLICATIONS_ID",
                                I_APPLICATIONS_UUID = "@T_APPLICATIONS_UUID",
                                I_IDENTITIES_ID = _identity_id,
                                PrettyLink = stageBuilderStagesModel.stage_name.Replace(" ", "-"),
                                I_THIS_CALLER = i_this_caller
                            };

                            _Transaction = addHelp.ADD_ENTRY_Stage(_Connect, thisStage, _Transaction);
                            //thisStage = add.Stage(_Connect, thisStage);
                            #endregion


                            #region MyRegion
                            if (true)
                            {
                                if (true)
                                {
                                    //This saves the stage builder grid to the database.
                                    //ViewGripModel GridGripToBytes = SaveStageBuilderGrid(_Connect, stageBuilderStagesModel, Stages_Type, thisStage.stages_id.ToString(), containersid.ToString(), SO);
                                    _Transaction = SaveStageBuilderGrid(_Connect, new CASTGOOP.StageBuilderGrid
                                    {
                                        // stageBuilderStagesModel, Stages_Type, thisStage.stages_id.ToString(), containersid.ToString(), SO, 
                                        stageBuilderModel = stageBuilderStagesModel,
                                        Stage_Type = Stages_Type,
                                        Stages_ID = "@T_STAGES_ID",
                                        Stages_UUID = "@T_STAGES_UUID",
                                        SO = SO,
                                        i_this_caller = Guid.NewGuid().ToString()
                                    }, _Transaction);
                                }

                                //ViewStageModel _StageToBytes = new ViewStageModel();

                                //_StageToBytes.gripList = new List<ViewGripModel>();
                                //_StageToBytes.applications_id = "@T_APPLICATIONS_ID";
                                //_StageToBytes.application_name = stageBuilderModel.app_name;
                                //_StageToBytes.stage_link = stageBuilderStagesModel.stage_name.Replace(" ", "-");
                                //_StageToBytes.stage_name = stageName;
                                //_StageToBytes.cores_id = ER_Tools.ConvertToInt64(coreid);
                                //_StageToBytes.stage_type = Stages_Type;
                                //_StageToBytes.stages_id = "@T_STAGES_ID";
                                //_StageToBytes.identities_id = _identity_id;
                                //_StageToBytes.object_type = Stages_Type;


                                #region THINGS RELATED TO STAGE
                                if (true)
                                {
                                    if (s == 0)
                                    {
                                        stageBuilderStagesModelPost = stageBuilderStagesModel; //For Use Later
                                                                                               //  _Transaction = SaveStageBuilderJSON(_Connect, stageBuilderStagesModel, coreid, stageBuilderModel.app_name, Stages_Type, ER_Tools.ConvertToInt64(thisStage.O_STAGES_ID), Identities_ID, JSON, _Transaction);
                                    }

                                    //if (stageBuilderStagesModel.background != null)
                                    //{
                                //        CASTGOOP.AddStageRole thisStageRole = new CASTGOOP.AddStageRole
                                //        {
                                //            I_THIS_CALLER = Guid.NewGuid().ToString(),
                                //            I_STAGES_UUID = "@T_STAGES_UUID",
                                //            I_ROLES_UUID = returnedRole,
                                //            I_APPLICATIONS_UUID = "@T_APPLICATIONS_UUID"
                                //        };

                                //        _Transaction = add.ADD_STAGE_ROLE(_Connect, thisStageRole, _Transaction);
                                    //}

                                    if (stageBuilderStagesModel.stage_roles != null)
                                    {
                                        foreach (string returnedRole in stageBuilderStagesModel.stage_roles)
                                        {
                                            CASTGOOP.AddStageRole thisStageRole = new CASTGOOP.AddStageRole
                                            {
                                                I_THIS_CALLER = Guid.NewGuid().ToString(),
                                                I_STAGES_UUID = "@T_STAGES_UUID",
                                                I_ROLES_UUID = returnedRole,
                                                I_APPLICATIONS_UUID = "@T_APPLICATIONS_UUID"
                                            };

                                            _Transaction = add.ADD_STAGE_ROLE(_Connect, thisStageRole, _Transaction);
                                        }
                                    }

                                    if (stageBuilderStagesModel.stage_parents != null)
                                    {
                                        for (int i = 0; i < stageBuilderStagesModel.stage_parents.Count; i++)
                                        {
                                            //er_dml.ADD_ENTRY_Stage_Relationships(_Connect, 
                                            //    thisStage.stages_id, 
                                            //    ER_Tools.ConvertToInt64(stageBuilderStagesModel.stage_parents[i].ToString()), 
                                            //    "Parent_Relationship");
                                            _Transaction = addHelp.ADD_ENTRY_Stage_Relationships(_Connect,
                                                 new CASTGOOP.EnterStageRelationship
                                                 {
                                                     I_STAGES_ID = thisStage.O_STAGES_ID,
                                                     I_RELATED_STAGES_ID = ER_Tools.ConvertToInt64(stageBuilderStagesModel.stage_parents[i].ToString()),
                                                     I_OBJECT_TYPE = "Parent_Relationship",
                                                     I_THIS_CALLER = Guid.NewGuid().ToString()
                                                 }, _Transaction);
                                        }
                                    }

                                    if (stageBuilderStagesModel.stage_children != null)
                                    {
                                        for (int i = 0; i < stageBuilderStagesModel.stage_children.Count; i++)
                                        {
                                            //er_dml.ADD_ENTRY_Stage_Relationships(_Connect, thisStage.stages_id, ER_Tools.ConvertToInt64(stageBuilderStagesModel.stage_children[i].ToString()), " Relationship");
                                            _Transaction = addHelp.ADD_ENTRY_Stage_Relationships(_Connect,
                                                 new CASTGOOP.EnterStageRelationship
                                                 {
                                                     I_STAGES_ID = thisStage.O_STAGES_ID,
                                                     I_RELATED_STAGES_ID = ER_Tools.ConvertToInt64(stageBuilderStagesModel.stage_children[i].ToString()),
                                                     I_OBJECT_TYPE = "Child_Relationship",
                                                     I_THIS_CALLER = Guid.NewGuid().ToString()
                                                 }, _Transaction);
                                        }
                                    }
                                }
                                #endregion

                                #region Grip Form Fields
                                //_StageToBytes.gripList = new List<ViewGripModel>();
                                ViewGripModel GripToBytes = new ViewGripModel();

                                CASTGOOP.Grip thisGrip = new CASTGOOP.Grip
                                {
                                    I_STAGES_UUID = thisStage.O_STAGES_UUID,
                                    I_STAGES_ID = thisStage.O_STAGES_ID,
                                    I_STAGE_TYPE = "Form",
                                    I_STAGE_NAME = stageBuilderStagesModel.stage_name,
                                    I_GRIP_TYPE = "Form",
                                    I_GRIP_NAME = "Form Fields",
                                    I_IDENTITIES_ID = ER_Tools.ConvertToInt64(identities_id),
                                    I_THIS_CALLER = Guid.NewGuid().ToString()
                                };

                                _Transaction = add.Grip(_Connect, thisGrip, _Transaction);
                                #endregion

                                #region Object Loading into DB
                                if (numberOfObjectsinGrid > 0)
                                {
                                    int currentObjectIndex;
                                    #region LOOP through Objects
                                    for (currentObjectIndex = 0; currentObjectIndex < numberOfObjectsinGrid; currentObjectIndex++)
                                    {
                                        stageBuilderStagesModel.grid_items[currentObjectIndex].type = stageBuilderStagesModel.grid_items[currentObjectIndex].type.Replace("formObject ", "");
                                        string fieldType = stageBuilderStagesModel.grid_items[currentObjectIndex].type;
                                        string fieldID = stageBuilderStagesModel.grid_items[currentObjectIndex].id;//stageBuilderStagesModel.grid_items[i].name;
                                        i_this_caller = stageBuilderStagesModel.grid_items[currentObjectIndex].i_this_caller.ToString();

                                        if (!string.IsNullOrWhiteSpace(i_this_caller))
                                        {
                                            findTheseUUIDsLate.Add(i_this_caller);
                                        }

                                        string fieldData = stageBuilderStagesModel.grid_items[currentObjectIndex].objectdata;
                                        int conCount = 0;
                                        string fieldObjecttype = "";
                                        string objecttype = "Text";
                                        string propertyValue = stageBuilderStagesModel.grid_items[currentObjectIndex].label;
                                        bool saveindb = true;

                                        #region SWITCH Field Object Type
                                        switch (fieldType)
                                        {
                                            case "addImage":
                                                //fieldObjecttype = "File";
                                                fieldObjecttype = "Image";
                                                break;
                                            case "textField":
                                                fieldObjecttype = "Text_Box";
                                                break;
                                            case "dropDown":
                                                fieldObjecttype = "Drop_Down";
                                                break;
                                            case "richText":
                                                fieldObjecttype = "Rich_Text";
                                                break;
                                            case "textArea":
                                                fieldObjecttype = "Paragraph_Text";
                                                break;
                                            case "radioButton":
                                                fieldObjecttype = "Radio_Button";
                                                break;
                                            case "checkBoxGroup":
                                                fieldObjecttype = "Check_Box";
                                                break;
                                            case "button":
                                                fieldObjecttype = "Button";
                                                break;
                                            case "link":
                                                fieldObjecttype = "Link";
                                                break;
                                            case "textLabel":
                                                fieldObjecttype = "Text";
                                                //fieldID = stageBuilderStagesModel.grid_items[i].label;
                                                objecttype = "HTML";
                                                propertyValue = HttpUtility.HtmlEncode(stageBuilderStagesModel.grid_items[currentObjectIndex].name);
                                                break;
                                            case "number":
                                                fieldObjecttype = "Number";
                                                break;
                                            case "currency":
                                                fieldObjecttype = "Currency";
                                                stageBuilderStagesModel.grid_items[currentObjectIndex].currencyType = "True";
                                                break;
                                            case "decimal":
                                                fieldObjecttype = "Decimal";
                                                break;
                                            case "total":
                                                fieldObjecttype = "Total";
                                                break;
                                            case "email":
                                                fieldObjecttype = "Email";
                                                break;
                                            case "url":
                                                fieldObjecttype = "Url";
                                                break;
                                            case "date":
                                                fieldObjecttype = "Date";
                                                break;
                                            case "time":
                                                fieldObjecttype = "Time";
                                                break;
                                            case "phone":
                                                fieldObjecttype = "Phone";
                                                break;
                                            case "creditCard":
                                                fieldObjecttype = "Credit_Card";
                                                break;
                                            case "fileUpload":
                                                fieldObjecttype = "File_Upload";
                                                break;
                                            case "signature":
                                                fieldObjecttype = "signature";
                                                break;
                                            case "viewTable":
                                                fieldObjecttype = "View_Table";
                                                break;
                                            case "sharedObject":
                                                fieldObjecttype = "SharedObject";
                                                break;
                                            default:
                                                saveindb = false;
                                                break;
                                        }
                                        #endregion

                                        if (saveindb)
                                        {
                                            #region Object Form Fields
                                            var thisBaseUUID = stageBuilderStagesModel.grid_items[currentObjectIndex].base_object_sets_uuid;
                                            bool isNewObject = thisBaseUUID.ToString().Contains("NEW-BASE-") ? true : false;
                                            CASTGOOP.ObjectSet objectSetFormField = new CASTGOOP.ObjectSet
                                            {
                                                I_BASE_OBJECT_SETS_UUID = isNewObject ? null : stageBuilderStagesModel.grid_items[currentObjectIndex].base_object_sets_uuid,
                                                I_PREV_OBJECT_SETS_UUID = isNewObject ? null : stageBuilderStagesModel.grid_items[currentObjectIndex].prev_object_sets_uuid,
                                                I_GRIPS_UUID = thisGrip.O_GRIPS_UUID,
                                                I_GRIPS_ID = thisGrip.O_GRIPS_ID,
                                                I_STAGE_TYPE = "Form",
                                                I_STAGE_NAME = stageName,
                                                I_GRIP_TYPE = "Form",
                                                I_GRIP_NAME = "Form Fields",
                                                I_OBJECT_TYPE = fieldObjecttype,
                                                I_IDENTITIES_ID = _identity_id,
                                                I_THIS_CALLER = i_this_caller
                                            };

                                            _Transaction = add.ObjectSet(_Connect, objectSetFormField, _Transaction);
                                            //objectSetFormField = add.ObjectSet(_Connect, objectSetFormField);

                                            ViewObjectSetModel _ObjectSetToBytes = new ViewObjectSetModel(); //Delete this later
                                            if (false)
                                            {
                                                //ObjectSetsID = objectSetFormField.object_sets_id.ToString();

                                                //ViewObjectSetModel _ObjectSetToBytes = new ViewObjectSetModel();

                                                //_ObjectSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                //_ObjectSetToBytes.grips_id = ER_Tools.ConvertToInt64(GripsID);
                                                //_ObjectSetToBytes.grip_type = "Form";
                                                //_ObjectSetToBytes.stage_name = stageName;
                                                //_ObjectSetToBytes.stage_type = "Form";
                                                //_ObjectSetToBytes.grip_name = "Form Fields";
                                                //_ObjectSetToBytes.object_type = fieldObjecttype;
                                                //_ObjectSetToBytes.containers_id = Organizations_ID;
                                                //_ObjectSetToBytes.identities_id = _identity_id; 
                                            }


                                            #endregion

                                            #region each Object Logic
                                            if (true)
                                            {
                                                _ObjectSetToBytes.ObjectPropSets = new List<ViewObjectPropSetsModel>();

                                                ViewObjectPropSetsModel PropSetToBytes = new ViewObjectPropSetsModel();

                                                // if (fieldObjecttype != "Text")
                                                // {
                                                #region Property ID
                                                thisPropSet = new CASTGOOP.ObjectPropSets
                                                {
                                                    I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                    I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                    I_OBJECT_PROP_TYPE = "ID",
                                                    I_VALUE_DATATYPE = "Characters",
                                                    I_PROPERTY_NAME = "ID",
                                                    I_HAS_PARENT = "false",
                                                    I_HAS_CHILD = "false",
                                                    I_PARENT_OBJ_PROP_SETS_ID = null,
                                                    I_PROPERTY_VALUE = fieldID,
                                                    I_THIS_CALLER = Guid.NewGuid().ToString()
                                                };

                                                _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                #endregion

                                                #region Property DATA                                        

                                                thisPropSet = new CASTGOOP.ObjectPropSets
                                                {
                                                    I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                    I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                    I_OBJECT_PROP_TYPE = "JSON",
                                                    I_VALUE_DATATYPE = "Characters",
                                                    I_PROPERTY_NAME = "ObjectData",
                                                    I_HAS_PARENT = "false",
                                                    I_HAS_CHILD = "false",
                                                    I_PARENT_OBJ_PROP_SETS_ID = null,
                                                    I_PROPERTY_VALUE = fieldData,
                                                    I_THIS_CALLER = Guid.NewGuid().ToString(),
                                                    V_AVOID_ANTIXSS = true
                                                };

                                                _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                #endregion

                                                #region Property Label
                                                thisPropSet = new CASTGOOP.ObjectPropSets
                                                {
                                                    I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                    I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                    I_OBJECT_PROP_TYPE = objecttype,
                                                    I_VALUE_DATATYPE = "Characters",
                                                    I_PROPERTY_NAME = "Label",
                                                    I_HAS_PARENT = "false",
                                                    I_HAS_CHILD = "false",
                                                    I_PARENT_OBJ_PROP_SETS_ID = null,
                                                    I_PROPERTY_VALUE = propertyValue,
                                                    I_THIS_CALLER = Guid.NewGuid().ToString()
                                                };

                                                _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                #endregion

                                                // }
                                                // else
                                                // {
                                                if (fieldObjecttype == "Text")
                                                {
                                                    //save label as byte 
                                                    ER_DML dml = new ER_DML();
                                                    IOHelper io = new IOHelper();

                                                    byte[] labelbyte = io.getBytes(propertyValue);

                                                    //_Transaction = addHelp.ADD_ENTRY_FILE(_Connect, new CASTGOOP.EnterFile
                                                    //{
                                                    //    I_FILE_NAME = "",
                                                    //    I_CONTENT_TYPE = ".ER",
                                                    //    I_FILE_SIZE = labelbyte.Length,
                                                    //    I_FILE_DATA = labelbyte,
                                                    //    I_IDENTITIES_ID = SO._IdentityModel.identities_id,
                                                    //    I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    //}, _Transaction);

                                                    ////dml.ADD_FILE_POINT(_Connect, LabelId, ObjectPropertySetID);
                                                    ////TODO: Make this use UUID instead
                                                    //_Transaction = addHelp.ADD_FILE_POINT(_Connect, new CASTGOOP.EnterFilePoint
                                                    //{
                                                    //    I_OBJ_PROP_SETS_ID = "@T_OBJ_PROP_SETS_ID",
                                                    //    I_FILES_ID = "@T_FILES_ID",
                                                    //    I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    //}, _Transaction);

                                                    CASTGOOP.AddObjectPropSetDataFile thisPropSetFile = new CASTGOOP.AddObjectPropSetDataFile
                                                    {
                                                        I_CONTENT_TYPE = ".ER",
                                                        I_FILE_NAME = "",
                                                        I_FILE_SIZE = labelbyte.Length,
                                                        I_VALUE = labelbyte,
                                                        I_OBJ_PROP_SETS_UUID = "@T_OBJ_PROP_SETS_UUID",
                                                        V_AVOID_ANTIXSS = true
                                                    };

                                                    _Transaction = add.ADD_PROP_SET_DATA_FILE(_Connect, thisPropSetFile, _Transaction);
                                                }

                                                #region Property Widget Section
                                                thisPropSet = new CASTGOOP.ObjectPropSets
                                                {
                                                    I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                    I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                    I_OBJECT_PROP_TYPE = "Text",
                                                    I_VALUE_DATATYPE = "Characters",
                                                    I_PROPERTY_NAME = "Widget",
                                                    I_HAS_PARENT = "false",
                                                    I_HAS_CHILD = "false",
                                                    I_PARENT_OBJ_PROP_SETS_ID = null,
                                                    I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].section,
                                                    I_THIS_CALLER = Guid.NewGuid().ToString()
                                                };

                                                _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                #endregion

                                                #region Add Roles to Object Sets

                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].section_roles != null)
                                                {
                                                    foreach (string returnedRole in stageBuilderStagesModel.grid_items[currentObjectIndex].section_roles)
                                                    {
                                                        CASTGOOP.AddObjectSetRole thisGridObjectSetRole = new CASTGOOP.AddObjectSetRole
                                                        {
                                                            I_THIS_CALLER = Guid.NewGuid().ToString(),
                                                            I_APPLICATIONS_UUID = "@T_APPLICATIONS_UUID",
                                                            I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                            I_ROLES_UUID = returnedRole
                                                        };

                                                        _Transaction = add.ADD_OBJECTSET_ROLE(_Connect, thisGridObjectSetRole, _Transaction);
                                                    }
                                                }
                                                #endregion


                                                #region Property Column Order
                                                //Column order
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].columnorder != null)
                                                {

                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Column_Order",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].columnorder,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = thisPropSet.obj_prop_sets_id;
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Column_Order";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].columnorder;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

                                                }
                                                #endregion

                                                #region Property Currency Type
                                                //Currency Type                                    
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].currencyType != null)
                                                {

                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                        I_OBJECT_PROP_TYPE = "Currency",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Currency_Type",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].currencyType,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = thisPropSet.obj_prop_sets_id;
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Decimal";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Currency_Type";
                                                    //PropSetToBytes.property_name = "Currency_Type";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].currencyType;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Tab Index
                                                //tab order
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].tabindex != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Tab_Index",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].tabindex,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property File Size
                                                //File Size
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].filesize != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "File_Size",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].filesize,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property File Accept
                                                //File Accept
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].accept != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "File_Accept",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].accept,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property File
                                                //image data
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].imageData != null)
                                                {
                                                    ER_DML dml = new ER_DML();
                                                    IOHelper io = new IOHelper();
                                                    string imagename = System.IO.Path.GetFileName(stageBuilderStagesModel.grid_items[currentObjectIndex].name);


                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "File",
                                                        I_PROPERTY_NAME = "ID",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = "Image",
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                    string extension = System.IO.Path.GetExtension(System.IO.Path.GetFileName(imagename));
                                                    //if (stageBuilderStagesModel.grid_items[currentObjectIndex].imageId == "0")
                                                    //{
                                                    byte[] thisFile = io.getBytes(stageBuilderStagesModel.grid_items[currentObjectIndex].imageData);
                                                    ////FileID = dml.ADD_ENTRY_FILE(_Connect, imagename, extension, io.getBytes(stageBuilderStagesModel.grid_items[i].imageData).Length.ToString(), io.getBytes(stageBuilderStagesModel.grid_items[i].imageData));
                                                    //_Transaction = addHelp.ADD_ENTRY_FILE(_Connect, new CASTGOOP.EnterFile
                                                    //{
                                                    //    I_FILE_NAME = imagename,
                                                    //    I_CONTENT_TYPE = extension,s
                                                    //    I_FILE_SIZE = thisFile.Length.ToString(),
                                                    //    I_FILE_DATA = thisFile,
                                                    //    I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    //}, _Transaction);

                                                    CASTGOOP.AddObjectPropSetDataFile thisPropSetFile = new CASTGOOP.AddObjectPropSetDataFile
                                                    {
                                                        I_CONTENT_TYPE = "File",
                                                        I_FILE_NAME = imagename,
                                                        I_FILE_SIZE = thisFile.Length.ToString(),
                                                        I_VALUE = thisFile,
                                                        I_OBJ_PROP_SETS_UUID = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ADD_PROP_SET_DATA_FILE(_Connect, thisPropSetFile, _Transaction);
                                                    //}
                                                    //else
                                                    //{
                                                    //    FileID = ER_Tools.ConvertToInt64(stageBuilderStagesModel.grid_items[currentObjectIndex].imageId);
                                                    //}
                                                    //try
                                                    //{
                                                    //    //dml.ADD_FILE_POINT(_Connect, FileID, ObjectPropertySetID);
                                                    //    if (FileID > 0)
                                                    //    {
                                                    //        _Transaction = addHelp.ADD_FILE_POINT(_Connect, new CASTGOOP.EnterFilePoint
                                                    //        {
                                                    //            I_OBJ_PROP_SETS_ID = "@T_OBJECT_SETS_ID",
                                                    //            I_FILES_ID = FileID,
                                                    //            I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    //        }, _Transaction);
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        _Transaction = addHelp.ADD_FILE_POINT(_Connect, new CASTGOOP.EnterFilePoint
                                                    //        {
                                                    //            I_OBJ_PROP_SETS_ID = "@T_OBJECT_SETS_ID",
                                                    //            I_FILES_ID = "@T_FILES_ID",
                                                    //            I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    //        }, _Transaction);
                                                    //    }

                                                    //}
                                                    //catch
                                                    //{
                                                    //    //Do Nothing for now.
                                                    //}
                                                }
                                                #endregion

                                                #region Property Digits Number
                                                //digits (Number)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].digits != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Digits",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].digits,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = thisPropSet.obj_prop_sets_id;
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Check_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Digits";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].digits;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Status Check Dropdown
                                                //Status Check (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].status_check != null)
                                                {

                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                        I_OBJECT_PROP_TYPE = "Check_Box", //PropSetToBytes.object_type,
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Status Check",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].status_check,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = thisPropSet.obj_prop_sets_id;
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Check_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Status Check";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].status_check;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

                                                }
                                                #endregion

                                                #region Property Status Type Dropdown
                                                //Status Type (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].status_type != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Status Type",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].status_type,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = thisPropSet.obj_prop_sets_id;
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Status Type";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].status_type;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

                                                }
                                                #endregion

                                                #region Property Status Identity Dropdown
                                                //Status Identity (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].status_identity != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Status Identity",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].status_identity,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = thisPropSet.obj_prop_sets_id;
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Status Identity";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].status_identity;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

                                                }
                                                #endregion

                                                #region Property Status Message Dropdown
                                                //Status Message (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].status_check != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Status Message",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].status_message,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = thisPropSet.obj_prop_sets_id;
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Status Message";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].status_message;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Bind Check
                                                //Bind Check (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].bind_check != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Bind_Check",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].bind_check,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                }
                                                #endregion

                                                #region Property Values Check
                                                //Values Check (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].values_check != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Values Check",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].values_check,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property Roles Check
                                                //Roles Check (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].roles_check != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Roles Check",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].roles_check,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                }
                                                #endregion

                                                #region Property Roles Collections
                                                //Roles Collections (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].roles_collections != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Roles Collections",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].roles_collections,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                }
                                                #endregion

                                                #region Property Roles User
                                                //Roles User (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].roles_user != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Drop_Down",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Roles User",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].roles_user,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                }
                                                #endregion

                                                #region Property Bind Stage Dropdown
                                                //Bind Stage (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].bind_stage != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Drop_Down",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Bind Stage",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].bind_stage,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Bind Stage";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].bind_stage;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);

                                                }
                                                #endregion

                                                #region Property Bind Objects Dropdown
                                                //Bind Objects (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].bind_objects != null)
                                                {
                                                    for (int objects_i = 0; objects_i < stageBuilderStagesModel.grid_items[currentObjectIndex].bind_objects.Count; objects_i++)
                                                    {
                                                        thisPropSet = new CASTGOOP.ObjectPropSets
                                                        {
                                                            I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                            I_OBJECT_PROP_TYPE = "Drop_Down",
                                                            I_VALUE_DATATYPE = "Characters",
                                                            I_PROPERTY_NAME = "Bind Objects",
                                                            I_HAS_PARENT = "false",
                                                            I_HAS_CHILD = "false",
                                                            I_PARENT_OBJ_PROP_SETS_ID = null,
                                                            I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].bind_objects[objects_i],
                                                            I_THIS_CALLER = Guid.NewGuid().ToString()
                                                        };

                                                        _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    }
                                                }
                                                #endregion

                                                #region Property View Table Stage
                                                //View Table Stage (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].view_table_stage != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Drop_Down",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "View_Table_Stage",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].view_table_stage,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                }
                                                #endregion

                                                #region Property View Table Filter Dropdown
                                                //View Table Filter (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].view_table_filter != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Drop_Down",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "View_Table_Filter",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].view_table_filter,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                }
                                                #endregion

                                                #region Property View Table Columns Dropdown
                                                //View Table Columns (Dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].view_table_columns != null)
                                                {
                                                    for (int objects_i = 0; objects_i < stageBuilderStagesModel.grid_items[currentObjectIndex].view_table_columns.Count; objects_i++)
                                                    {
                                                        thisPropSet = new CASTGOOP.ObjectPropSets
                                                        {
                                                            I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                            I_OBJECT_PROP_TYPE = "Text",
                                                            I_VALUE_DATATYPE = "Characters",
                                                            I_PROPERTY_NAME = "View_Table_Columns",
                                                            I_HAS_PARENT = "false",
                                                            I_HAS_CHILD = "false",
                                                            I_PARENT_OBJ_PROP_SETS_ID = null,
                                                            I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].view_table_columns[objects_i],
                                                            I_THIS_CALLER = Guid.NewGuid().ToString()
                                                        };

                                                        _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    }
                                                }
                                                #endregion

                                                #region Property Total Object
                                                //total (Total Object)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].num_objects != null)
                                                {
                                                    string objects = "";
                                                    for (int _count = 0; _count < stageBuilderStagesModel.grid_items[currentObjectIndex].num_objects.Count; _count++)
                                                    {
                                                        objects += stageBuilderStagesModel.grid_items[currentObjectIndex].num_objects[_count].Trim();
                                                        if (_count != (stageBuilderStagesModel.grid_items[currentObjectIndex].num_objects.Count - 1))
                                                            objects += ",";
                                                    }


                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Total",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Total",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = objects,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property Total Number Check
                                                //numbers check (Total Object)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].number_check != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Number Check",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].number_check,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property Total Currency Check
                                                //currency check (Total Object)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].currency_check != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Currency Check",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].currency_check,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property Total Dropdown Check
                                                //dropdown check (Total Object)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].dropdown_check != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Dropdown Check",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].dropdown_check,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property email
                                                //email 
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].email != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Email",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Email",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].email,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Check_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Email";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].email;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property URL
                                                //URL 
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].url != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Url",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "URL",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].url,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property Date
                                                //date 
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].date != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Date",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Date",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].date,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property Time
                                                //time 
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].time != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Time", //PropSetToBytes.object_type,
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Time",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].time,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                }
                                                #endregion

                                                #region Property Phone
                                                //phone 
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].phone != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Phone",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Phone",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].phone,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Check_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Phone";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].phone;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Height
                                                //height
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].height != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Height",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].height,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Height";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].height;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Width
                                                //width
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].width != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Width",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].width,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Width";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].width;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Button Border
                                                //button border
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].button_border != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Button_Border",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].button_border,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Button Border";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_border;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Button BG Color
                                                //button bgcolor
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].button_bgcolor != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Button_BG_Color",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].button_bgcolor,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Button BG Color";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_bgcolor;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Button Font Color
                                                //button fontcolor
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].button_fontcolor != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Button Font Color",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].button_fontcolor,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Button Font Color";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_fontcolor;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Button Type
                                                //button type
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].button_type != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Button Type",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].button_type,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Button Type";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_type;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Button Stage
                                                //button stage
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].button_stage != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Button Stage",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].button_stage,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Button Stage";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_stage;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Button Submit
                                                //button submit
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].button_submit != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Button Submit",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].button_submit,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Button Submit";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_submit;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Button Send
                                                //button send
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].button_send != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Button Send",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].button_send,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Button Send";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].button_send;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Style
                                                //style
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].button_border != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Style",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].style,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Style";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].style;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Link Type
                                                //link type
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].link_type != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Link Type",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].link_type,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Link Type";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].link_type;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Link Stage
                                                //link stage
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].link_stage != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Link Stage",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].link_stage,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Link Stage";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].link_stage;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Link Target
                                                //link target
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].link_target != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Link Target",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].link_target,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Link Target";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].link_target;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Link URL
                                                //link url
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].link_url != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Link Url",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].link_url,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Link Url";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].link_url;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Show Label
                                                //show label
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].show_label != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "ShowLabel",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].show_label,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Check_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "ShowLabel";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].show_label;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                //field required 
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].required != null && stageBuilderStagesModel.grid_items[currentObjectIndex].required.ToString().ToLower() == "true")
                                                {
                                                    #region Property Required
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Required",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = "true",
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Check_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Required";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = "true";
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                    #endregion

                                                    #region Property Required Message
                                                    if (stageBuilderStagesModel.grid_items[currentObjectIndex].req_message != null)
                                                    {
                                                        thisPropSet = new CASTGOOP.ObjectPropSets
                                                        {
                                                            I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                            I_OBJECT_PROP_TYPE = "Text_Box",
                                                            I_VALUE_DATATYPE = "Characters",
                                                            I_PROPERTY_NAME = "Required_Message",
                                                            I_HAS_PARENT = "false",
                                                            I_HAS_CHILD = "false",
                                                            I_PARENT_OBJ_PROP_SETS_ID = null,
                                                            I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].req_message,
                                                            I_THIS_CALLER = Guid.NewGuid().ToString()
                                                        };

                                                        _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                        //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                        //PropSetToBytes = new ViewObjectPropSetsModel();
                                                        //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                        //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                        //PropSetToBytes.object_type = "Text_Box";
                                                        //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                        //PropSetToBytes.value_datatype = "Characters";
                                                        //PropSetToBytes.property_name = "Required Message";
                                                        //PropSetToBytes.has_parent = "false";
                                                        //PropSetToBytes.has_child = "false";
                                                        //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                        //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].req_message;
                                                        //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                    }
                                                    #endregion
                                                }

                                                //reg expression
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].reg_exp != null && stageBuilderStagesModel.grid_items[currentObjectIndex].reg_exp.ToString().ToLower() == "true")
                                                {
                                                    #region Property Regular Expression
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Check_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Regular_Expression",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].reg_exp,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Check_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Regular Expression";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].reg_exp;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                    #endregion

                                                    #region Property Regular Expression Pattern
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Regular_Expression_Pattern",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].pattern,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString(),
                                                        V_AVOID_ANTIXSS = true
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Regular Expression Pattern";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].pattern;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                    #endregion

                                                    #region Pattern Regular Expression Message
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Regular_Expression_Message",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].pattern_message,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Regular Expression Message";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].pattern_message;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                    #endregion

                                                }

                                                #region Property MinLength
                                                //min 
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].minlength != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "MinLength",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].minlength,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Min";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].min;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property MaxLength
                                                //max
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].maxlength != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "MaxLength",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].maxlength,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Max";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].max;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Min
                                                //min 
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].min != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Min",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].min,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Min";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].min;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Max
                                                //max
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].max != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Max",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].max,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Max";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].max;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Format
                                                //format
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].format != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Drop_Down",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "Format",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].format,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "Format";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].format;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Field Size
                                                //field size
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].field_size != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Drop_Down",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "FieldSize",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].field_size,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Drop_Down";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "FieldSize";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].field_size;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                #region Property Field Height
                                                //field height
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].field_height != null)
                                                {
                                                    thisPropSet = new CASTGOOP.ObjectPropSets
                                                    {
                                                        I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                        I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                        I_OBJECT_PROP_TYPE = "Text_Box",
                                                        I_VALUE_DATATYPE = "Characters",
                                                        I_PROPERTY_NAME = "FieldHeight",
                                                        I_HAS_PARENT = "false",
                                                        I_HAS_CHILD = "false",
                                                        I_PARENT_OBJ_PROP_SETS_ID = null,
                                                        I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].field_height,
                                                        I_THIS_CALLER = Guid.NewGuid().ToString()
                                                    };

                                                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                    //PropSetToBytes = new ViewObjectPropSetsModel();
                                                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                    //PropSetToBytes.object_type = "Text_Box";
                                                    //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                    //PropSetToBytes.value_datatype = "Characters";
                                                    //PropSetToBytes.property_name = "FieldHeight";
                                                    //PropSetToBytes.has_parent = "false";
                                                    //PropSetToBytes.has_child = "false";
                                                    //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                    //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].field_height;
                                                    //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                }
                                                #endregion

                                                //conditions rules
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].conditions != null)
                                                {
                                                    conCount = stageBuilderStagesModel.grid_items[currentObjectIndex].conditions.Count;
                                                    if (conCount > 0)
                                                    {
                                                        #region Property Condition Action
                                                        thisPropSet = new CASTGOOP.ObjectPropSets
                                                        {
                                                            I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                            I_OBJECT_PROP_TYPE = "Drop_Down",
                                                            I_VALUE_DATATYPE = "Characters",
                                                            I_PROPERTY_NAME = "Condition_Action",
                                                            I_HAS_PARENT = "false",
                                                            I_HAS_CHILD = "false",
                                                            I_PARENT_OBJ_PROP_SETS_ID = null,
                                                            I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].con_action,
                                                            I_THIS_CALLER = Guid.NewGuid().ToString()
                                                        };

                                                        _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                        //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                        //PropSetToBytes = new ViewObjectPropSetsModel();
                                                        //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                        //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                        //PropSetToBytes.object_type = "Drop_Down";
                                                        //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                        //PropSetToBytes.value_datatype = "Characters";
                                                        //PropSetToBytes.property_name = "Condition Action";
                                                        //PropSetToBytes.has_parent = "false";
                                                        //PropSetToBytes.has_child = "false";
                                                        //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                        //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].con_action;
                                                        //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                        #endregion

                                                        #region Property Condition Logic
                                                        thisPropSet = new CASTGOOP.ObjectPropSets
                                                        {
                                                            I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                            I_OBJECT_PROP_TYPE = "Drop_Down",
                                                            I_VALUE_DATATYPE = "Characters",
                                                            I_PROPERTY_NAME = "Condition_Logic",
                                                            I_HAS_PARENT = "false",
                                                            I_HAS_CHILD = "false",
                                                            I_PARENT_OBJ_PROP_SETS_ID = null,
                                                            I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].con_logic,
                                                            I_THIS_CALLER = Guid.NewGuid().ToString()
                                                        };

                                                        _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                        //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                        //PropSetToBytes = new ViewObjectPropSetsModel();
                                                        //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                        //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                        //PropSetToBytes.object_type = "Drop_Down";
                                                        //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                        //PropSetToBytes.value_datatype = "Characters";
                                                        //PropSetToBytes.property_name = "Condition Logic";
                                                        //PropSetToBytes.has_parent = "false";
                                                        //PropSetToBytes.has_child = "false";
                                                        //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                        //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].con_logic;
                                                        //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                        #endregion

                                                        for (int b = 0; b < conCount; b++)
                                                        {
                                                            #region Property Condition ID #
                                                            thisPropSet = new CASTGOOP.ObjectPropSets
                                                            {
                                                                I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                                I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                                I_OBJECT_PROP_TYPE = "Text",
                                                                I_VALUE_DATATYPE = "Characters",
                                                                I_PROPERTY_NAME = "Condition_ID" + b,
                                                                I_HAS_PARENT = "false",
                                                                I_HAS_CHILD = "false",
                                                                I_PARENT_OBJ_PROP_SETS_ID = null,
                                                                I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].conditions[b].con_id,
                                                                I_THIS_CALLER = Guid.NewGuid().ToString()
                                                            };

                                                            _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                            //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                            //PropSetToBytes = new ViewObjectPropSetsModel();
                                                            //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                            //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                            //PropSetToBytes.object_type = "Text";
                                                            //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                            //PropSetToBytes.value_datatype = "Characters";
                                                            //PropSetToBytes.property_name = "Condition ID " + b;
                                                            //PropSetToBytes.has_parent = "false";
                                                            //PropSetToBytes.has_child = "false";
                                                            //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                            //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].conditions[b].con_id;
                                                            //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                            #endregion

                                                            #region Property Condition Field #
                                                            thisPropSet = new CASTGOOP.ObjectPropSets
                                                            {
                                                                I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                                I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                                I_OBJECT_PROP_TYPE = "Text_Box",
                                                                I_VALUE_DATATYPE = "Characters",
                                                                I_PROPERTY_NAME = "Condition_Field" + b,
                                                                I_HAS_PARENT = "false",
                                                                I_HAS_CHILD = "false",
                                                                I_PARENT_OBJ_PROP_SETS_ID = null,
                                                                I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].conditions[b].con_field,
                                                                I_THIS_CALLER = Guid.NewGuid().ToString()
                                                            };

                                                            _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                            //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                            //PropSetToBytes = new ViewObjectPropSetsModel();
                                                            //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                            //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                            //PropSetToBytes.object_type = "Text_Box";
                                                            //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                            //PropSetToBytes.value_datatype = "Characters";
                                                            //PropSetToBytes.property_name = "Condition Field " + b;
                                                            //PropSetToBytes.has_parent = "false";
                                                            //PropSetToBytes.has_child = "false";
                                                            //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                            //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].conditions[b].con_field;
                                                            //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                            #endregion

                                                            #region Property Condition Operator #
                                                            thisPropSet = new CASTGOOP.ObjectPropSets
                                                            {
                                                                I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                                I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                                I_OBJECT_PROP_TYPE = "Drop_Down",
                                                                I_VALUE_DATATYPE = "Characters",
                                                                I_PROPERTY_NAME = "Condition_Operator" + b,
                                                                I_HAS_PARENT = "false",
                                                                I_HAS_CHILD = "false",
                                                                I_PARENT_OBJ_PROP_SETS_ID = null,
                                                                I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].conditions[b].con_operator,
                                                                I_THIS_CALLER = Guid.NewGuid().ToString()
                                                            };

                                                            _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                            //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                            //PropSetToBytes = new ViewObjectPropSetsModel();
                                                            //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                            //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                            //PropSetToBytes.object_type = "Drop_Down";
                                                            //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                            //PropSetToBytes.value_datatype = "Characters";
                                                            //PropSetToBytes.property_name = "Condition Operator " + b;
                                                            //PropSetToBytes.has_parent = "false";
                                                            //PropSetToBytes.has_child = "false";
                                                            //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                            //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].conditions[b].con_operator;
                                                            //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                            #endregion

                                                            #region Property Condition Option #
                                                            thisPropSet = new CASTGOOP.ObjectPropSets
                                                            {
                                                                I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                                I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                                I_OBJECT_PROP_TYPE = "Text",
                                                                I_VALUE_DATATYPE = "Characters",
                                                                I_PROPERTY_NAME = "Condition_Option" + b,
                                                                I_HAS_PARENT = "false",
                                                                I_HAS_CHILD = "false",
                                                                I_PARENT_OBJ_PROP_SETS_ID = null,
                                                                I_PROPERTY_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].conditions[b].con_option,
                                                                I_THIS_CALLER = Guid.NewGuid().ToString()
                                                            };

                                                            _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                                            //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);

                                                            //PropSetToBytes = new ViewObjectPropSetsModel();
                                                            //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                            //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                            //PropSetToBytes.object_type = "Text";
                                                            //PropSetToBytes.object_prop_type = PropSetToBytes.object_type;
                                                            //PropSetToBytes.value_datatype = "Characters";
                                                            //PropSetToBytes.property_name = "Condition Option " + b;
                                                            //PropSetToBytes.has_parent = "false";
                                                            //PropSetToBytes.has_child = "false";
                                                            //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                            //PropSetToBytes.property_value = stageBuilderStagesModel.grid_items[i].conditions[b].con_option;
                                                            //_ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                            #endregion
                                                        }
                                                    }
                                                }

                                                //options (for checkboxgroup, radiobuttons, dropdown)
                                                if (stageBuilderStagesModel.grid_items[currentObjectIndex].options != null)
                                                {
                                                    conCount = stageBuilderStagesModel.grid_items[currentObjectIndex].options.Count;

                                                    if (conCount > 0)
                                                    {
                                                        #region Property Value
                                                        thisPropSet = new CASTGOOP.ObjectPropSets
                                                        {
                                                            I_OBJECT_SETS_UUID = objectSetFormField.O_OBJECT_SETS_UUID,
                                                            I_OBJECT_SETS_ID = objectSetFormField.O_OBJECT_SETS_ID,
                                                            I_OBJECT_PROP_TYPE = "Items",
                                                            I_VALUE_DATATYPE = "Characters",
                                                            I_PROPERTY_NAME = "Value",
                                                            I_HAS_PARENT = "false",
                                                            I_HAS_CHILD = "false",
                                                            I_PARENT_OBJ_PROP_SETS_ID = null,
                                                            I_PROPERTY_VALUE = fieldID, //stageBuilderStagesModel.grid_items[i].options[0].name);
                                                            I_THIS_CALLER = Guid.NewGuid().ToString()
                                                        };

                                                        //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                                                        _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                                                        //PropSetToBytes = new ViewObjectPropSetsModel();
                                                        //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                        //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                                                        //PropSetToBytes.object_type = "Items";
                                                        //PropSetToBytes.object_prop_type = "Items";
                                                        //PropSetToBytes.value_datatype = "Characters";
                                                        //PropSetToBytes.property_name = "Value";
                                                        //PropSetToBytes.has_parent = "false";
                                                        //PropSetToBytes.has_child = "false";
                                                        //PropSetToBytes.parent_obj_prop_sets_id = 0;
                                                        //PropSetToBytes.property_value = fieldID;
                                                        #endregion

                                                        PropSetToBytes.ObjectPropOptSets = new List<ViewObjectPropOptSetsModel>();
                                                        ViewObjectPropOptSetsModel _PropertyOptionToBytes = new ViewObjectPropOptSetsModel();
                                                        for (int b = 0; b < conCount; b++)
                                                        {
                                                            #region Property Option
                                                            //ObjectPropertyOptionSetID = add.ObjectSetPropertyOption(_Connect, ObjectPropertySetID, stageBuilderStagesModel.grid_items[i].options[b].name);
                                                            _Transaction = add.ObjectSetPropertyOption(_Connect, new CASTGOOP.ObjectPropSetOption
                                                            {
                                                                I_OBJ_PROP_SETS_UUID = ObjectPropertySetUUID,
                                                                I_OBJ_PROP_SETS_ID = ObjectPropertySetID,
                                                                I_OPTION_VALUE = stageBuilderStagesModel.grid_items[currentObjectIndex].options[b].value == null ? "" : stageBuilderStagesModel.grid_items[currentObjectIndex].options[b].value,
                                                                I_OPTION_NAME = stageBuilderStagesModel.grid_items[currentObjectIndex].options[b].name == null ? "" : stageBuilderStagesModel.grid_items[currentObjectIndex].options[b].name,
                                                                I_THIS_CALLER = Guid.NewGuid().ToString()
                                                            }, _Transaction);

                                                            //_PropertyOptionToBytes = new ViewObjectPropOptSetsModel();
                                                            //_PropertyOptionToBytes.obj_prop_opt_sets_id = ER_Tools.ConvertToInt64(ObjectPropertyOptionSetID);
                                                            //_PropertyOptionToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                                                            //_PropertyOptionToBytes.option_value = stageBuilderStagesModel.grid_items[i].options[b].name;


                                                            //PropSetToBytes.ObjectPropOptSets.Add(_PropertyOptionToBytes);

                                                            #endregion
                                                        }

                                                        _ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                                                    }
                                                }
                                            }
                                            #endregion

                                        }
                                    }
                                    #endregion
                                }
                                #endregion

                                #region Old Code that may be reused later. This is for caching.
                                //if (false)
                                //{
                                //    //Add Grip to GripsList
                                //    _StageToBytes.gripList.Add(GripToBytes);

                                //    // Adds the Grid Grip to the stage
                                //    //_StageToBytes.gripList.Add(GridGripToBytes);
                                //    #region SAVE CACHE Array
                                //    try
                                //    {

                                //        var listOfArrays = _StagesToBytes;

                                //        ER_Tools _tools = new ER_Tools();

                                //        List<ViewStageModel> temp = new List<ViewStageModel>();

                                //        temp.Add(_StageToBytes);

                                //        byte[] StageBytes = _tools.ListToBytes(_StageToBytes);
                                //        //ViewStageModel reverse = (ViewStageModel)_tools.ByteArrayToObject(StageBytes);

                                //        //Array TempArray = _StagesToBytes.ToArray();

                                //        //byte[] StageBytes = ER_Tools._ListToBytes(TempArray);
                                //        #region Grip Stage Cache
                                //        CASTGOOP.Grip thisCacheGrip = new CASTGOOP.Grip
                                //        {
                                //            I_STAGES_ID = thisStage.O_STAGES_ID,
                                //            I_STAGE_TYPE = "Form",
                                //            I_STAGE_NAME = stageBuilderStagesModel.stage_name,
                                //            I_GRIP_TYPE = "Cache",
                                //            I_GRIP_NAME = "Stage Cache",
                                //            I_CONTAINERS_ID = Organizations_ID,
                                //            I_IDENTITIES_ID = _identity_id
                                //        };

                                //        thisCacheGrip = add.GripWithPermissions(_Connect, thisCacheGrip);
                                //        GripsID = thisCacheGrip.O_GRIPS_ID;

                                //        GripToBytes.stages_id = thisStage.O_STAGES_ID;
                                //        GripToBytes.stage_type = "Form";
                                //        GripToBytes.stage_name = stageBuilderStagesModel.stage_name;
                                //        GripToBytes.grip_type = "Form";
                                //        GripToBytes.grip_name = "Stage Cache";
                                //        GripToBytes.containers_id = Organizations_ID;
                                //        GripToBytes.identities_id = _identity_id;
                                //        GripToBytes.ObjectSets = new List<ViewObjectSetModel>();
                                //        #endregion

                                //        #region Object Stage Cache
                                //        //Run Patch for this object
                                //        CASTGOOP.ObjectSet objectSetCache = new CASTGOOP.ObjectSet
                                //        {
                                //            I_GRIPS_ID = thisCacheGrip.O_GRIPS_ID,
                                //            I_STAGE_TYPE = "Cache",
                                //            I_STAGE_NAME = stageBuilderStagesModel.stage_name,
                                //            I_GRIP_TYPE = "Cache",
                                //            I_GRIP_NAME = "Stage Cache",
                                //            I_OBJECT_TYPE = "Cache",
                                //            I_CONTAINERS_ID = Organizations_ID,
                                //            I_IDENTITIES_ID = _identity_id
                                //        };
                                //        objectSetCache = add.ObjectSetwithPermission(_Connect, objectSetCache);
                                //        ObjectSetsID = objectSetCache.O_OBJECT_SETS_ID;
                                //        #endregion

                                //        #region Property Stage Cache
                                //        thisPropSet = new CASTGOOP.ObjectPropSets
                                //        {
                                //            I_OBJECT_SETS_ID = objectSetCache.O_OBJECT_SETS_ID,
                                //            I_OBJECT_PROP_TYPE = "Cache",
                                //            I_VALUE_DATATYPE = "File",
                                //            I_PROPERTY_NAME = "Stage Cache",
                                //            I_HAS_PARENT = "false",
                                //            I_HAS_CHILD = "false",
                                //            I_PARENT_OBJ_PROP_SETS_ID = null,
                                //            I_PROPERTY_VALUE = "Cache"
                                //        };

                                //        _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                                //        #endregion

                                //        DMLHelper DMLH = new DMLHelper();
                                //        string CacheResult = DMLH.ADD_ENTRY_OBJECT_DATA(_Connect,
                                //            Identities_ID,
                                //            thisStage.O_STAGES_ID,
                                //            thisCacheGrip.O_GRIPS_ID,
                                //            objectSetCache.O_OBJECT_SETS_ID,
                                //            thisPropSet.O_OBJ_PROP_SETS_ID,
                                //            "OBJ_PROP_SETS",
                                //            ObjectPropertySetID,
                                //            0,
                                //            "file",
                                //            new Object_Value
                                //            {
                                //                _File = new File_Object
                                //                {
                                //                    _FileBytes = StageBytes,
                                //                    FILE_NAME = stageBuilderStagesModel.stage_name + "_CACHE",
                                //                    FILE_SIZE = StageBytes.Length,
                                //                    CONTENT_TYPE = ".ER"
                                //                }
                                //            },
                                //            true
                                //        );

                                //        //byte[] array = listOfArrays.SelectMany(a => a).ToArray();

                                //        //byte[] StageBytes = _StagesToBytes.SelectMany(s => Encoding.UTF8.GetBytes(s)).ToArray();
                                //    }
                                //    catch (Exception e)
                                //    {
                                //        e.ToString();
                                //    }
                                //    #endregion

                                //    ER_Generate er_gen = new ER_Generate();

                                //    er_gen.GENERATE_STAGEPROP_VIEW(_Connect, thisStage.O_STAGES_ID.ToString());
                                //} 
                                #endregion
                            }
                            #endregion
                        }
                        #endregion

                        string TransactionName = ER_Procedure.TransactionNameGenerator(SO, "DESIGNER_SAVE_I");
                        _Transaction = ER_Procedure.SQL_BUILD_BLOCK(_Connect, TransactionName, _Transaction);
                        ER_Query CMD = new ER_Query();

                        List<DataTable> AllResults = CMD.RUN_NON_QUERY(_Connect, _Transaction.SQLBlock.ToString(), "Success", new List<DataTable>());

                        #region Generate Tables
                        DataTable AppResult = AllResults[0];
                        DataColumnCollection DCC = AppResult.Columns;
                        List<CommandResult> HoldResult = new List<CommandResult>();
                        string temp = "";
                        if (DCC.Contains("T_APPLICATIONS_UUID") && AppResult.Rows.Count > 0)
                        {
                            temp = AppResult.Rows[0]["T_APPLICATIONS_UUID"].ToString();

                            ConnectToDB DynamicDB = _Connect.Copy();
                            DynamicDB.Schema = "DYNAMIC";
                            DynamicDB.Schema2 = "CSA";


                            ConnectToDB connectMV = _Connect.Copy();
                            connectMV.Schema = "MV";
                            connectMV.Schema2 = "CSA";

                            var TableName = "T_" + temp.Replace("-", "_").ToUpper();
                            //Structs.Models.RevampSystem.Dictionary.CreatTable Temp2 = er_dml.CreateTableFromCastGoop(DynamicDB,
                            //    new Structs.Models.RevampSystem.Dictionary.CreatTable
                            //    {
                            //        I_APPLICATIONS_UUID = temp,
                            //        I_SCHEMA = "MV",
                            //        I_TABLENAME = TableName
                            //    });




                            thisResult.StructeGenerator = GenerateApplicationTables.Create(_Connect, new GenerateApplicationTables.GenerateApplication
                            {
                                I_APPLICATIONS_UUID = temp,
                                I_SCHEMA = "CSA",
                                I_TABLENAME = TableName
                            });

                        }
                        #endregion

                        ArrayList AL2 = new ArrayList();

                        #region Load Designer Transactions into thisResult
                        foreach (DataTable thisReturnSet in AllResults)
                        {
                            thisResult.trans = ER_Tools._GetObjectListFromDataTable(AL2, thisReturnSet);
                        }
                        #endregion

                        #region Convert Transactions Array to Json String and Then Deserialize into Objects
                        string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(thisResult.trans);
                        List<returnObject> thisJSON = Newtonsoft.Json.JsonConvert.DeserializeObject<List<returnObject>>(jsonString);

                        JArray rss = JArray.Parse(jsonString);
                        #endregion

                        #region Update JSON with UUID's that came from DB
                        foreach (string item in findTheseUUIDsLate)
                        {
                            returnObject currentFind = thisJSON.Find(p => p.i_this_caller == item);

                            var useThis = string.IsNullOrWhiteSpace(currentFind.t_stages_uuid) ? currentFind.t_object_sets_uuid : currentFind.t_stages_uuid;

                            JSON = currentFind != null ? JSON.Replace("NEW-BASE-" + item, useThis) : JSON;
                            JSON = currentFind != null ? JSON.Replace(item, useThis) : JSON;

                        }
                        #endregion

                        #region Get StageID that was first saved to DB.
                        returnObject firstStage = thisJSON.Find(p => p.t_stages_id != null);
                        #endregion

                        #region Load JSON into DB
                        sqlTransBlocks _Transaction2 = new sqlTransBlocks();
                        _Transaction2 = SaveStageBuilderJSON(_Connect, stageBuilderStagesModelPost, coreid, stageBuilderModel.app_name, "Form", firstStage.t_stages_id, firstStage.t_stages_uuid, identities_id, JSON, _Transaction2);

                        TransactionName = ER_Procedure.TransactionNameGenerator(SO, "DESIGNER_SAVE_II");
                        _Transaction2 = ER_Procedure.SQL_BUILD_BLOCK(_Connect, TransactionName, _Transaction2);
                        List<DataTable> AllResults2 = CMD.RUN_NON_QUERY(_Connect, _Transaction2.SQLBlock.ToString(), "Success", new List<DataTable>());
                        #endregion

                        #region Load JSON Transactions into thisResult
                        foreach (DataTable thisReturnSet in AllResults2)
                        {
                            thisResult.trans = ER_Tools._GetObjectListFromDataTable(AL2, thisReturnSet);
                        }
                        #endregion

                        thisResult.applications_uuid = temp;
                        thisResult.successful = true;
                    }
                    else
                    {
                        thisResult.successful = false;
                        if (coreid > 0)
                        {
                            thisResult.messages.Add("Valid Core Not Provided.");
                        }

                        if (CanUseAppName)
                        {
                            thisResult.messages.Add("Application Name Can not be used.");
                        }
                    }

                }
            }
            return thisResult;
        }

        public class returnObject
        {
            public string i_this_caller { get; set; }

            public string t_object_sets_uuid { get; set; }
            public long? t_stages_id { get; set; }

            public string t_stages_uuid { get; set; }
        }


        ////TODO: Review and Revisit
        //public void SaveStageBuilderJSONOldWayOfBusiness(IConnectToDB _Connect, StageBuilderStagesModel stageBuilderModel, string AppName, string Stage_Type, long? Stages_ID, long? Identity_ID, string JSON)
        //{
        //    bool isNumeric1 = Stages_ID > 0;
        //    bool isNumeric2 = Identity_ID > 0;
        //    long? _identity_id = Identity_ID;

        //    if (isNumeric1 && isNumeric2)
        //    {
        //        add add = new add();
        //        DMLHelper DMLH = new DMLHelper();
        //        IOHelper io = new IOHelper();
        //        string stageName = stageBuilderModel.stage_name;
        //        int Organizations_ID = 1000;

        //        Values.AddGrip thisGrip = new Values.AddGrip
        //        {
        //            I_STAGES_ID = Stages_ID,
        //            I_STAGE_TYPE = Stage_Type,
        //            I_STAGE_NAME = stageBuilderModel.stage_name,
        //            I_GRIP_TYPE = "JSON",
        //            I_GRIP_NAME = "JSON",
        //            I_CONTAINERS_ID = Organizations_ID,
        //            I_IDENTITIES_ID = _identity_id
        //        };

        //        thisGrip = add.GripWithPermissions(_Connect, thisGrip);

        //        Values.AddObjectSet objectSetCoreSettings = new Values.AddObjectSet
        //        {
        //            I_GRIPS_ID = thisGrip.O_GRIPS_ID,
        //            I_STAGE_TYPE = Stage_Type,
        //            I_STAGE_NAME = stageName,
        //            I_GRIP_TYPE = "JSON",
        //            I_GRIP_NAME = "JSON",
        //            I_OBJECT_TYPE = "JSON Text",
        //            I_CONTAINERS_ID = Organizations_ID,
        //            I_IDENTITIES_ID = _identity_id
        //        };

        //        objectSetCoreSettings = add.ObjectSetwithPermission(_Connect, objectSetCoreSettings);

        //        #region Property ID
        //        Values.AddObjectPropertySet thisPropSet = new Values.AddObjectPropertySet
        //        {
        //            I_OBJECT_SETS_ID = objectSetCoreSettings.O_OBJECT_SETS_ID,
        //            I_OBJECT_PROP_TYPE = "ID",
        //            I_VALUE_DATATYPE = "File",
        //            I_PROPERTY_NAME = "ID",
        //            I_HAS_PARENT = "false",
        //            I_HAS_CHILD = "false",
        //            I_PARENT_OBJ_PROP_SETS_ID = null,
        //            I_PROPERTY_VALUE = "JSON Prop Set"
        //        };

        //        thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //        #endregion

        //        byte[] _bytearray = io.getBytes(JSON);

        //        DMLH.ADD_ENTRY_OBJECT_DATA(_Connect, Identity_ID, Stages_ID, thisGrip.O_GRIPS_ID, objectSetCoreSettings.O_OBJECT_SETS_ID, thisPropSet.O_OBJ_PROP_SETS_ID, "OBJ_PROP_SETS", thisPropSet.O_OBJ_PROP_SETS_ID, 0, "file", new Object_Value { _File = new File_Object { _FileBytes = _bytearray, FILE_NAME = AppName + "_JSON", FILE_SIZE = _bytearray.Length, CONTENT_TYPE = ".json" } }, true);

        //    }
        //}

        //TODO:Remap to use UUID
        public sqlTransBlocks SaveStageBuilderJSON(IConnectToDB _Connect, StageBuilderStagesModel stageBuilderModel,
                long? coreid,
                string AppName,
                string Stage_Type,
                object Stages_ID,
                object Stages_UUID,
                long? Identity_ID,
                string JSON, sqlTransBlocks _Transaction)
        {
            long? _identity_id = Identity_ID;

            if (true)
            {
                add add = new add();
                DMLHelper DMLH = new DMLHelper();
                IOHelper io = new IOHelper();
                string stageName = stageBuilderModel.stage_name;

                CASTGOOP.Grip thisGrip = new CASTGOOP.Grip
                {
                    I_STAGES_ID = ER_Tools.ConvertToInt64(Stages_ID) > 0 ? Stages_ID : "@T_STAGES_ID",
                    I_STAGES_UUID = ER_Tools.ConvertToGuid(Stages_UUID.ToString()) != null ? Stages_UUID : "@T_STAGES_UUID",
                    I_STAGE_TYPE = Stage_Type,
                    I_STAGE_NAME = stageBuilderModel.stage_name,
                    I_GRIP_TYPE = "JSON",
                    I_GRIP_NAME = "JSON",
                    I_IDENTITIES_ID = _identity_id
                };

                //thisGrip = add.Grip(_Connect, thisGrip);
                _Transaction = add.Grip(_Connect, thisGrip, _Transaction);

                CASTGOOP.ObjectSet objectSetCoreSettings = new CASTGOOP.ObjectSet
                {
                    I_GRIPS_ID = "@T_GRIPS_ID",
                    I_GRIPS_UUID = "@T_GRIPS_UUID",
                    I_STAGE_TYPE = Stage_Type,
                    I_STAGE_NAME = stageName,
                    I_GRIP_TYPE = "JSON",
                    I_GRIP_NAME = "JSON",
                    I_OBJECT_TYPE = "JSON Text",
                    I_IDENTITIES_ID = _identity_id
                };

                //objectSetCoreSettings = add.ObjectSet(_Connect, objectSetCoreSettings);
                _Transaction = add.ObjectSet(_Connect, objectSetCoreSettings, _Transaction);

                #region Property ID
                CASTGOOP.ObjectPropSets thisPropSet = new CASTGOOP.ObjectPropSets
                {
                    I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                    I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                    I_OBJECT_PROP_TYPE = "ID",
                    I_VALUE_DATATYPE = "File",
                    I_PROPERTY_NAME = "ID",
                    I_HAS_PARENT = "false",
                    I_HAS_CHILD = "false",
                    I_PARENT_OBJ_PROP_SETS_ID = null,
                    I_PROPERTY_VALUE = "JSON Prop Set"
                };

                //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                #endregion

                byte[] _bytearray = io.getBytes(JSON);

                CASTGOOP.AddObjectPropSetDataFile thisPropSetFile = new CASTGOOP.AddObjectPropSetDataFile
                {
                    I_CONTENT_TYPE = ".json",
                    I_FILE_NAME = AppName + "_JSON",
                    I_FILE_SIZE = _bytearray.Length,
                    I_VALUE = _bytearray,
                    I_OBJ_PROP_SETS_UUID = "@T_OBJ_PROP_SETS_UUID"
                };

                _Transaction = add.ADD_PROP_SET_DATA_FILE(_Connect, thisPropSetFile, _Transaction);
            }

            return _Transaction;
        }

        ///// <summary>
        ///// Save Stage Builder Grid
        ///// </summary>
        ///// <param name="db_platform">Database type</param>
        ///// <param name="connAuth">Connection string</param>
        ///// <param name="stageBuilderModel">Stage builder object that holds stage builder data.</param>
        //public ViewGripModel SaveStageBuilderGrid(IConnectToDB _Connect, StageBuilderStagesModel stageBuilderModel, string Stage_Type, long? Stages_ID, long? Containers_ID, SessionObjects SO)
        //{
        //    long? n = Stages_ID;
        //    bool isNumeric1 = n > 0;
        //    ViewGripModel GridGriptoBytes = new ViewGripModel();

        //    long? _stages_id = Stages_ID;

        //    if (isNumeric1)
        //    {
        //        add add = new add();
        //        long? GripsID, ObjectSetsID, ObjectPropertySetID, Organizations_ID = Containers_ID;
        //        string stageName = stageBuilderModel.stage_name;

        //        long? Identities_ID = SO.SessionIdentity.Identity.identities_id;

        //        long? _organizations_id = Organizations_ID;

        //        SecurityHelper SECH = new SecurityHelper();

        //        //Stages_ID = add.Stage(_Connect,"Grid", stageBuilderModel.stage_name, "1000", Organizations_ID, Identities_ID);

        //        Values.AddGrip thisGridGrip = new Values.AddGrip
        //        {
        //            I_STAGES_ID = _stages_id,
        //            I_STAGE_TYPE = Stage_Type,
        //            I_STAGE_NAME = stageBuilderModel.stage_name,
        //            I_GRIP_TYPE = "Grid",
        //            I_GRIP_NAME = "Grid",
        //            I_CONTAINERS_ID = _organizations_id,
        //            I_IDENTITIES_ID = Identities_ID
        //        };

        //        thisGridGrip = add.GripWithPermissions(_Connect, thisGridGrip);
        //        GripsID = thisGridGrip.O_GRIPS_ID;

        //        GridGriptoBytes.grip_name = "Grid";
        //        GridGriptoBytes.grip_type = "Grid";
        //        GridGriptoBytes.identities_id = SO.SessionIdentity.Identity.identities_id;
        //        GridGriptoBytes.stages_id = Stages_ID;
        //        GridGriptoBytes.stage_type = Stage_Type;
        //        GridGriptoBytes.stage_name = stageBuilderModel.stage_name;

        //        int count = stageBuilderModel.grid_sections.Count;

        //        int i;

        //        GridGriptoBytes.ObjectSets = new List<ViewObjectSetModel>();

        //        for (i = 0; i < count; i++)
        //        {
        //            ViewObjectSetModel ObjectSetToBytes = new ViewObjectSetModel();
        //            ViewObjectPropSetsModel PropSetToBytes = new ViewObjectPropSetsModel();
        //            ObjectSetToBytes.ObjectPropSets = new List<ViewObjectPropSetsModel>();

        //            Values.AddObjectSet objectSetGridWidget = new Values.AddObjectSet
        //            {
        //                I_GRIPS_ID = thisGridGrip.O_GRIPS_ID,
        //                I_STAGE_TYPE = Stage_Type,
        //                I_STAGE_NAME = stageName,
        //                I_GRIP_TYPE = "Grid",
        //                I_GRIP_NAME = "Grid",
        //                I_OBJECT_TYPE = "Grid Widget",
        //                I_CONTAINERS_ID = _organizations_id,
        //                I_IDENTITIES_ID = Identities_ID
        //            };

        //            objectSetGridWidget = add.ObjectSetwithPermission(_Connect, objectSetGridWidget);
        //            ObjectSetsID = objectSetGridWidget.O_OBJECT_SETS_ID;


        //            ObjectSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            ObjectSetToBytes.dt_created = DateTime.Now;
        //            ObjectSetToBytes.grip_name = "Grid";
        //            ObjectSetToBytes.grip_type = "Grid";
        //            ObjectSetToBytes.object_type = "Grid Widget";
        //            ObjectSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            ObjectSetToBytes.stage_name = stageName;
        //            ObjectSetToBytes.stage_type = Stage_Type;
        //            ObjectSetToBytes.grips_id = ER_Tools.ConvertToInt64(GripsID);
        //            ObjectSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);


        //            #region Property ID
        //            Values.AddObjectPropertySet thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "ID",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "ID",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].id

        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "ID";
        //            PropSetToBytes.object_type = "ID";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].id;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property data-col
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "data-col",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].data_col
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = Identities_ID;
        //            PropSetToBytes.obj_prop_sets_id = ObjectPropertySetID;
        //            PropSetToBytes.object_sets_id = ObjectSetsID;
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "data-col";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].data_col;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property data-row
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "data-row",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].data_row
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "data-row";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].data_row;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property data-sizex
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "data-sizex",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].data_sizex
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "data-sizex";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].data_sizex;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property data-sizey
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "data-sizey",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].data_sizey
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "data-sizey";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].data_sizey;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property Color
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "color",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].color
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "color";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].color;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property background-color
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "background-color",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].background_color
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "background-color";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].background_color;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            //section background image data
        //            if (stageBuilderModel.grid_sections[i].background_image != null)
        //            {
        //                add addHelp = new add();
        //                IOHelper io = new IOHelper();
        //                //ObjectPropertySetID = add.ObjectSetProperty(_Connect, ObjectSetsID, "Text", "File", "ID", "false", "false", "", "Image");
        //                // DMLH.ADD_ENTRY_OBJECT_DATA(_Connect, Identities_ID, Stages_ID, GripsID, ObjectPropertySetID, "OBJ_PROP_SETS", ObjectPropertySetID, "0", "file", new Object_Value { _File = io.getBytes(stageBuilderStagesModel.grid_items[i].imageData) });

        //                Values.AddFile thisFile = addHelp.ADD_ENTRY_FILE(_Connect, new Values.AddFile
        //                {
        //                    I_FILE_NAME = "section_background_image",
        //                    I_CONTENT_TYPE = ".jpg",
        //                    I_FILE_SIZE = io.getBytes(stageBuilderModel.grid_sections[i].background_image).Length,
        //                    I_FILE_DATA = io.getBytes(stageBuilderModel.grid_sections[i].background_image),
        //                    I_IDENTITIES_ID = SO._IdentityModel.identities_id
        //                });

        //                addHelp.ADD_FILE_POINT(_Connect, new Values.AddFilePoint
        //                {
        //                    I_FILES_ID = thisFile.O_FILES_ID,
        //                    I_IDENTITIES_ID = SO._IdentityModel.identities_id,
        //                    I_OBJ_PROP_SETS_ID = ObjectSetsID
        //                });
        //            }

        //            #region Property section-border-style
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "section-border-style",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].section_border_style
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "section-border-style";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].section_border_style;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property section-top-border
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "section-top-border",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].section_top_border
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "section-top-border";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].section_top_border;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property section-right-border
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "section-right-border",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].section_right_border
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "section-right-border";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].section_right_border;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property section-bottom-border
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "section-bottom-border",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].section_bottom_border
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "section-bottom-border";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].section_bottom_border;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            #region Property section-left-border
        //            thisPropSet = new Values.AddObjectPropertySet
        //            {
        //                I_OBJECT_SETS_ID = objectSetGridWidget.O_OBJECT_SETS_ID,
        //                I_OBJECT_PROP_TYPE = "Text",
        //                I_VALUE_DATATYPE = "Characters",
        //                I_PROPERTY_NAME = "section-left-border",
        //                I_HAS_PARENT = "false",
        //                I_HAS_CHILD = "false",
        //                I_PARENT_OBJ_PROP_SETS_ID = null,
        //                I_PROPERTY_VALUE = stageBuilderModel.grid_sections[i].section_left_border
        //            };

        //            thisPropSet = add.ADD_ENTRY_Object_Property_Set(_Connect, thisPropSet);
        //            ObjectPropertySetID = thisPropSet.O_OBJ_PROP_SETS_ID;

        //            PropSetToBytes = new ViewObjectPropSetsModel();
        //            PropSetToBytes.grip_name = "Grid";
        //            PropSetToBytes.grip_type = "Grid";
        //            PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
        //            PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
        //            PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
        //            PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
        //            PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
        //            PropSetToBytes.property_name = "section-left-border";
        //            PropSetToBytes.object_type = "Text";
        //            PropSetToBytes.object_prop_type = "Characters";
        //            PropSetToBytes.property_value = stageBuilderModel.grid_sections[i].section_left_border;
        //            PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
        //            PropSetToBytes.has_parent = "false";
        //            PropSetToBytes.has_child = "false";
        //            ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
        //            #endregion

        //            GridGriptoBytes.ObjectSets.Add(ObjectSetToBytes);

        //            //Add Roles to Object Sets
        //            if (stageBuilderModel.grid_sections[i].section_roles != null)
        //            {
        //                foreach (string role in stageBuilderModel.grid_sections[i].section_roles)
        //                {
        //                    SECH.AddRoletoObjectSet(_Connect, ER_Tools.ConvertToInt64(SECH.GetRoleID(_Connect, role)), ER_Tools.ConvertToInt64(ObjectSetsID));
        //                }
        //            }
        //        }

        //    }
        //    return GridGriptoBytes;
        //}

        public sqlTransBlocks SaveStageBuilderGrid(IConnectToDB _Connect, CASTGOOP.StageBuilderGrid thisStageBuilderGrid, sqlTransBlocks _Transaction)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            long? n = ER_Tools.ConvertToInt64(thisStageBuilderGrid.Stages_ID);

            ViewGripModel GridGriptoBytes = new ViewGripModel();

            var _stages_id = thisStageBuilderGrid.Stages_ID;

            if (true)
            {
                add add = new add();
                long? ObjectSetsID = 0;
                string stageName = thisStageBuilderGrid.stageBuilderModel.stage_name;

                long? Identities_ID = thisStageBuilderGrid.SO.SessionIdentity.Identity.identities_id;

                SecurityHelper SECH = new SecurityHelper();

                //Stages_ID = add.Stage(_Connect,"Grid", stageBuilderModel.stage_name, "1000", Organizations_ID, Identities_ID);

                CASTGOOP.Grip thisGridGrip = new CASTGOOP.Grip
                {
                    I_STAGES_ID = _stages_id,
                    I_STAGES_UUID = thisStageBuilderGrid.Stages_UUID,
                    I_STAGE_TYPE = thisStageBuilderGrid.Stage_Type,
                    I_STAGE_NAME = thisStageBuilderGrid.stageBuilderModel.stage_name,
                    I_GRIP_TYPE = "Grid",
                    I_GRIP_NAME = "Grid",
                    I_IDENTITIES_ID = Identities_ID,
                    I_THIS_CALLER = Guid.NewGuid().ToString()
                };

                //thisGridGrip = add.Grip(_Connect, thisGridGrip);
                _Transaction = add.Grip(_Connect, thisGridGrip, _Transaction);

                //GripsID = thisGridGrip.grips_id.ToString();

                //GridGriptoBytes.grip_name = "Grid";
                //GridGriptoBytes.grip_type = "Grid";
                //GridGriptoBytes.identities_id = ER_Tools.ConvertToInt64(SO.SessionIdentity.Identity.identities_id);
                //GridGriptoBytes.stages_id = ER_Tools.ConvertToInt64(thisStageBuilderGrid.Stages_ID);
                //GridGriptoBytes.stage_type = thisStageBuilderGrid.Stage_Type;
                //GridGriptoBytes.stage_name = thisStageBuilderGrid.stageBuilderModel.stage_name;

                int count = thisStageBuilderGrid.stageBuilderModel.grid_sections.Count;

                int gridSectionIndex;

                GridGriptoBytes.ObjectSets = new List<ViewObjectSetModel>();

                for (gridSectionIndex = 0; gridSectionIndex < count; gridSectionIndex++)
                {
                    ViewObjectSetModel ObjectSetToBytes = new ViewObjectSetModel();
                    ViewObjectPropSetsModel PropSetToBytes = new ViewObjectPropSetsModel();
                    ObjectSetToBytes.ObjectPropSets = new List<ViewObjectPropSetsModel>();

                    CASTGOOP.ObjectSet objectSetGridWidget = new CASTGOOP.ObjectSet
                    {
                        I_GRIPS_UUID = "@T_GRIPS_UUID",
                        I_GRIPS_ID = "@T_GRIPS_ID",
                        I_STAGE_TYPE = thisStageBuilderGrid.Stage_Type,
                        I_STAGE_NAME = stageName,
                        I_GRIP_TYPE = "Grid",
                        I_GRIP_NAME = "Grid",
                        I_OBJECT_TYPE = "Grid Widget",
                        I_IDENTITIES_ID = Identities_ID,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //objectSetGridWidget = add.ObjectSet(_Connect, objectSetGridWidget);
                    _Transaction = add.ObjectSet(_Connect, objectSetGridWidget, _Transaction);

                    //ObjectSetsID = objectSetGridWidget.object_sets_id.ToString();


                    //ObjectSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //ObjectSetToBytes.dt_created = DateTime.Now;
                    //ObjectSetToBytes.grip_name = "Grid";
                    //ObjectSetToBytes.grip_type = "Grid";
                    //ObjectSetToBytes.object_type = "Grid Widget";
                    //ObjectSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //ObjectSetToBytes.stage_name = stageName;
                    //ObjectSetToBytes.stage_type = thisStageBuilderGrid.Stage_Type;
                    //ObjectSetToBytes.grips_id = ER_Tools.ConvertToInt64(GripsID);
                    //ObjectSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);


                    #region Property ID
                    CASTGOOP.ObjectPropSets thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "ID",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "ID",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].id,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };


                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "ID";
                    //PropSetToBytes.object_type = "ID";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].id;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property data-col
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "data-col",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].data_col,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "data-col";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].data_col;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property data-row
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "data-row",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].data_row,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);


                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "data-row";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].data_row;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property data-sizex
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "data-sizex",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].data_sizex,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "data-sizex";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].data_sizex;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property data-sizey
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "data-sizey",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].data_sizey,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "data-sizey";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].data_sizey;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property Color
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "color",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].color,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "color";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].color;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property background-color
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "background-color",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].background_color,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "background-color";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].background_color;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    //section background image data
                    if (thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].background_image != null)
                    {
                        add addHelp = new add();
                        IOHelper io = new IOHelper();
                        //ObjectPropertySetID = add.ObjectSetProperty(_Connect, ObjectSetsID, "Text", "File", "ID", "false", "false", "", "Image");
                        // DMLH.ADD_ENTRY_OBJECT_DATA(_Connect, Identities_ID, Stages_ID, GripsID, ObjectPropertySetID, "OBJ_PROP_SETS", ObjectPropertySetID, "0", "file", new Object_Value { _File = io.getBytes(stageBuilderStagesModel.grid_items[i].imageData) });

                        //string FileID = dml.ADD_ENTRY_FILE(_Connect, "section_background_image", ".jpg", io.getBytes(thisStageBuilderGrid.stageBuilderModel.grid_sections[i].background_image).Length.ToString(), io.getBytes(stageBuilderModel.grid_sections[i].background_image));
                        //string FileID = dml.ADD_ENTRY_FILE(_Connect, "section_background_image", ".jpg", 
                        //    io.getBytes(thisStageBuilderGrid.stageBuilderModel.grid_sections[i].background_image).Length.ToString(), 
                        //    io.getBytes(thisStageBuilderGrid.stageBuilderModel.grid_sections[i].background_image));
                        thisPropSet = new CASTGOOP.ObjectPropSets
                        {
                            I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                            I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                            I_OBJECT_PROP_TYPE = "Text",
                            I_VALUE_DATATYPE = "Characters",
                            I_PROPERTY_NAME = "section_background_image",
                            I_HAS_PARENT = "false",
                            I_HAS_CHILD = "false",
                            I_PARENT_OBJ_PROP_SETS_ID = null,
                            I_PROPERTY_VALUE = "Get-From-Files",
                            I_THIS_CALLER = Guid.NewGuid().ToString()
                        };
                        _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                        byte[] thisFileBytes = io.getBytes(thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].background_image);

                        CASTGOOP.AddObjectPropSetDataFile thisPropSetFile = new CASTGOOP.AddObjectPropSetDataFile
                        {
                            I_CONTENT_TYPE = ".jpg",
                            I_FILE_NAME = "section_background_image",
                            I_FILE_SIZE = thisFileBytes.Length.ToString(),
                            I_VALUE = thisFileBytes,
                            I_OBJ_PROP_SETS_UUID = Guid.NewGuid().ToString()
                        };

                        _Transaction = add.ADD_PROP_SET_DATA_FILE(_Connect, thisPropSetFile, _Transaction);

                        //_Transaction = addHelp.ADD_ENTRY_FILE(_Connect,
                        //    new CASTGOOP.EnterFile
                        //    {
                        //        I_FILE_NAME = "section_background_image",
                        //        I_CONTENT_TYPE = ".jpg",
                        //        I_FILE_SIZE = io.getBytes(thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].background_image).Length.ToString(),
                        //        I_FILE_DATA = io.getBytes(thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].background_image),
                        //        I_THIS_CALLER = Guid.NewGuid().ToString()
                        //    }, _Transaction);

                        ////dml.ADD_FILE_POINT(_Connect, FileID, thisApplication.application_id.ToString()
                        ////_Transaction = dml.ADD_FILE_POINT(_Connect, new CASTGOOP.EnterFilePoint { _Obj_Prop_Sets_ID = "@T_APPLICATIONS_ID" }, _Transaction);
                        //_Transaction = addHelp.ADD_FILE_POINT(_Connect, new CASTGOOP.EnterFilePoint { I_OBJ_PROP_SETS_ID = "@T_OBJECT_SETS_ID", I_FILES_ID = "@T_FILES_ID" }, _Transaction);
                        ////dml.ADD_FILE_POINT(_Connect, FileID, ObjectSetsID);
                    }

                    #region Property section-border-style
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "section-border-style",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].section_border_style,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "section-border-style";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].section_border_style;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property section-top-border
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "section-top-border",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].section_top_border,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "section-top-border";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].section_top_border;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property section-right-border
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "section-right-border",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].section_right_border,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);
                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "section-right-border";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].section_right_border;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property section-bottom-border
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "section-bottom-border",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].section_bottom_border,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "section-bottom-border";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].section_bottom_border;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    #region Property section-left-border
                    thisPropSet = new CASTGOOP.ObjectPropSets
                    {
                        I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                        I_OBJECT_SETS_ID = "@T_OBJECT_SETS_ID",
                        I_OBJECT_PROP_TYPE = "Text",
                        I_VALUE_DATATYPE = "Characters",
                        I_PROPERTY_NAME = "section-left-border",
                        I_HAS_PARENT = "false",
                        I_HAS_CHILD = "false",
                        I_PARENT_OBJ_PROP_SETS_ID = null,
                        I_PROPERTY_VALUE = thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].section_left_border,
                        I_THIS_CALLER = Guid.NewGuid().ToString()
                    };

                    //thisPropSet = add.ObjectSetProperty(_Connect, thisPropSet);
                    _Transaction = add.ObjectSetProperty(_Connect, thisPropSet, _Transaction);

                    //ObjectPropertySetID = thisPropSet.obj_prop_sets_id.ToString();

                    //PropSetToBytes = new ViewObjectPropSetsModel();
                    //PropSetToBytes.grip_name = "Grid";
                    //PropSetToBytes.grip_type = "Grid";
                    //PropSetToBytes.identities_id = ER_Tools.ConvertToInt64(Identities_ID);
                    //PropSetToBytes.obj_prop_sets_id = ER_Tools.ConvertToInt64(ObjectPropertySetID);
                    //PropSetToBytes.object_sets_id = ER_Tools.ConvertToInt64(ObjectSetsID);
                    //PropSetToBytes.stage_name = GridGriptoBytes.stage_name;
                    //PropSetToBytes.stage_type = GridGriptoBytes.stage_type;
                    //PropSetToBytes.property_name = "section-left-border";
                    //PropSetToBytes.object_type = "Text";
                    //PropSetToBytes.object_prop_type = "Characters";
                    //PropSetToBytes.property_value = thisStageBuilderGrid.stageBuilderModel.grid_sections[i].section_left_border;
                    //PropSetToBytes.containers_id = ER_Tools.ConvertToInt64(Organizations_ID);
                    //PropSetToBytes.has_parent = "false";
                    //PropSetToBytes.has_child = "false";
                    //ObjectSetToBytes.ObjectPropSets.Add(PropSetToBytes);
                    #endregion

                    //GridGriptoBytes.ObjectSets.Add(ObjectSetToBytes);
                    ObjectSetsID = ObjectSetsID < 0 || ObjectSetsID == null ? 0 : ObjectSetsID;

                    #region Add Roles to Grid Section Object Sets
                    //Add Roles to Object Sets
                    //if (thisStageBuilderGrid.stageBuilderModel.grid_sections[i].section_roles != null)
                    //{

                    //    foreach (string role in thisStageBuilderGrid.stageBuilderModel.grid_sections[i].section_roles)
                    //    {
                    //        _Transaction = SECH.AddRoletoObjectSet(_Connect, new CASTGOOP.EnterRoleToObject
                    //        {
                    //            object_sets_id = "@T_OBJECT_SETS_ID",
                    //            Roles_id = ER_Tools.ConvertToInt64(SECH.GetRoleID(_Connect, role)),
                    //        }, _Transaction);
                    //    }
                    //}

                    if (thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].section_roles != null)
                    {
                        foreach (string returnedRole in thisStageBuilderGrid.stageBuilderModel.grid_sections[gridSectionIndex].section_roles)
                        {
                            CASTGOOP.AddObjectSetRole thisAppRole = new CASTGOOP.AddObjectSetRole
                            {
                                I_THIS_CALLER = Guid.NewGuid().ToString(),
                                I_OBJECT_SETS_UUID = "@T_OBJECT_SETS_UUID",
                                I_APPLICATIONS_UUID = "@T_APPLICATIONS_UUID",
                                I_ROLES_UUID = returnedRole
                            };

                            _Transaction = add.ADD_OBJECTSET_ROLE(_Connect, thisAppRole, _Transaction);
                        }
                    }
                    #endregion

                }

            }
            //return GridGriptoBytes;
            return _Transaction;
        }


        public StageModels SingleStageModel(StageModels Stage, DataRow _DR)
        {
            StageModel StageModel = new StageModel
            {
                applications_id = _DR.Field<long?>("applications_id"),
                containers_id = _DR.Field<long?>("containers_id"),
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                stage_name = _DR.Field<string>("stage_men"),
                stage_type = _DR.Field<string>("stage_type"),
                stages_id = _DR.Field<long?>("stages_id"),
                application_name = _DR.Field<string>("application_name"),
                cores_id = _DR.Field<long?>("cores_id"),
                object_layer = _DR.Field<string>("object_layer"),
                enabled = _DR.Field<string>("enabled")
            };

            Stage = new StageModels { Stage = StageModel };

            return Stage;

        }

        public StageModels SingleStageView(ViewStageModel Stage, DataRow _DR)
        {
            ViewStageModel StageModel = new ViewStageModel
            {
                applications_id = _DR.Field<long?>("applications_id"),
                containers_id = _DR.Field<long?>("containers_id"),
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                stage_name = _DR.Field<string>("stage_name"),
                stage_type = _DR.Field<string>("stage_type"),
                stages_id = _DR.Field<long?>("stages_id"),
                application_name = _DR.Field<string>("application_name"),
                cores_id = _DR.Field<long?>("cores_id"),
                object_layer = _DR.Field<string>("object_layer"),
                enabled = _DR.Field<string>("enabled"),
                object_type = _DR.Field<string>("object_type"),
                stage_link = _DR.Field<string>("stage_link")

            };

            StageModels SM = new StageModels { StageView = StageModel };

            return SM;

        }

        public List<StageModels> GetStageModels(IConnectToDB _Connect, ApplicationModels App, Boolean _GetGrips, Boolean _GetObjectSets, Boolean _GetPropertySets, Boolean _GetOptionProperties)
        {
            List<StageModels> StageModels = new List<StageModels>();
            StageModels StageModel1 = new StageModels();

            DataTable _DT = FindbyColumnID(_Connect, "applications_id", App.AppView.applications_id.ToString());

            foreach (DataRow AppStageRow in _DT.Rows)
            {
                StageModel1 = SingleStageView(new ViewStageModel(), AppStageRow);

                if (_GetGrips)
                {
                    GripsHelper GH = new GripsHelper();
                    StageModel1.Grips = GH.GetGrips(_Connect, StageModel1, _GetObjectSets, _GetPropertySets, _GetOptionProperties);
                }
                StageModels.Add(StageModel1);
            }

            return StageModels;
        }

    }
}

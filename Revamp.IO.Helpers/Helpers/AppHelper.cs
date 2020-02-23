using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Foundation;
using Revamp.IO.Structs.Models;
using Revamp.IO.Structs.Models.DataEntry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Revamp.IO.Helpers.Helpers
{
    public class AppHelper
    {
        public static ViewApplicationModel Get(IConnectToDB _Connect, SessionObjects SO, Guid? applications_uuid)
        {
            AppHelper app = new AppHelper();
            ViewApplicationModel thisApp = new ViewApplicationModel();

            bool hasAccessToApplication = true;
            List<IdentityRoles> thisPersonRoles = SecurityHelper.GetIdentityRoles(_Connect, SO.SessionIdentity.Identity.identities_uuid,"Y");
            if (applications_uuid != null)
            {
                thisApp = app.GetApplicationStruct(_Connect, SO, applications_uuid, true);

                List<SecurityApplicationRoles> thisAppRoles = thisApp.roles;

                IEnumerable<Guid?> thisAppRoles2 = thisAppRoles.Select(x => x.roles_uuid).ToArray();
                if (thisAppRoles2.Count() > 0)
                {
                    IEnumerable<Guid?> thisPersonRoles2 = thisPersonRoles.Select(x => x.roles_uuid).ToArray();
                    hasAccessToApplication = thisAppRoles2.Intersect(thisPersonRoles2).Any();
                }

                //Give SYSTEM ADMIN access
                if(thisPersonRoles.Exists(x => x.role_name == "SYSTEM ADMIN"))
                    hasAccessToApplication = true;
            }

            if (hasAccessToApplication)
            {
                for (int i = 0; i < thisApp.stages.Count; i++)
                {
                    var HasAccessToThisStage = true;
                    IEnumerable<Guid?> thisStageRoles = thisApp.stages[i].roles.Select(x => x.roles_uuid).ToArray();
                    if (thisStageRoles.Count() > 0)
                    {
                        IEnumerable<Guid?> thisPersonRoles2 = thisPersonRoles.Select(x => x.roles_uuid).ToArray();
                        HasAccessToThisStage = thisStageRoles.Intersect(thisPersonRoles2).Any();
                    }

                    if (!HasAccessToThisStage)
                    {
                        thisApp.stages.RemoveAt(i);
                        i--;
                    }
                }

                thisApp.stages = AppHelper.CreateRenderFromStages(_Connect, true, thisApp.stages);
            }

            return thisApp;
        }

        public static StageBuilderModel GetAppJSON(IConnectToDB _Connect, SessionObjects SO, ViewApplicationModel thisApp)
        {
            List<ViewGripModel> grips = new List<ViewGripModel>();
            IOHelper io = new IOHelper();
            string jsonString = "";

            StageBuilderModel stageBuilder = new StageBuilderModel();

            if (thisApp != null)
            {
                List<ViewStageModel> stage = thisApp.stages;
                if (stage.Count > 0)
                {
                    Guid? obj_prop_sets_uuid = null;

                    if (stage.Find(x => x.stage_name == "Application Properties") == null)
                    {
                        grips = stage.ElementAt(0).Grips;
                        obj_prop_sets_uuid = grips.Find(x => x.grip_name == "JSON").ObjectSets.ElementAt(0).ObjectPropSets.ElementAt(0).obj_prop_sets_uuid;
                    }
                    else
                    {
                        grips = stage.ElementAt(1).Grips;
                        obj_prop_sets_uuid = grips.Find(x => x.grip_name == "JSON").ObjectSets.ElementAt(0).ObjectPropSets.ElementAt(0).obj_prop_sets_uuid;
                    }

                    jsonString = io.getStringFromBytesViaObjectProSetid(_Connect, obj_prop_sets_uuid);
                    stageBuilder = JsonConvert.DeserializeObject<StageBuilderModel>(HttpUtility.HtmlDecode(jsonString));
                }
            }

            return stageBuilder;
        }

        public DataTable FindApplicationsByCore(IConnectToDB _Connect, string coreid)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = coreid });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_RENDITION", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "0" });

            string viewName = "VW_APPLICATION_LATEST_RENDITION";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }

        public DataTable FindAll(IConnectToDB _Connect)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_APPLICATION_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Core Settings" });

            string viewName = "VW_APPLICATION_LATEST_RENDITION_AND_CREATOR";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }

        public DataTable FindByCoreOrUSer(IConnectToDB _Connect, string coreid, string id, string column)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = coreid });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_APPLICATION_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Core Settings,Revamp Settings" });

            string viewName = "VW_APPLICATION_LATEST_RENDITION_AND_CREATOR";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }
        public DataTable FindByColumnAndValue(IConnectToDB _Connect, string id, string column)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_APPLICATION_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Core Settings,Revamp Settings" });

            string viewName = "VW_APPLICATION_LATEST_RENDITION_AND_CREATOR";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }

        public DataTable FindByApplicationID(IConnectToDB _Connect, string _id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _id });

            string viewName = "VW_APPLICATION_LATEST_RENDITION_AND_CREATOR";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }
        public DataTable SubmissionLimitByIdentityId(IConnectToDB _Connect, string Identities_id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Identities_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PROPERTY_TYPE_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "member_submissionlimit" });

            string viewName = "IDENTITY_PROPERTIES";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { columns = "isnull(property_Name, 0) UserAllowed", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }
        public DataTable MembershipTypeByIdentityId(IConnectToDB _Connect, string Identities_id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Identities_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PROPERTY_TYPE_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "member_type" });

            string viewName = "IDENTITY_PROPERTIES";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { columns = "isnull(property_Name, 0) UserAllowed", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }

        public DataTable ApplicationLimitReachedByIdentityId(IConnectToDB _Connect, string Identities_id)
        {
            ER_Query er_query = new ER_Query();
            string coreid = "";
            string ownerIdentityId = "";
            string ownerAppsAllowed = "";
            string appsRemaining = "";
            long? ownerAppsUtilised = 0;
            long? OwnerAppsRemaining = 0;
            #region App Util Query
            List<DynamicModels.RootReportFilter> UserAppUtilizationFilters = new List<DynamicModels.RootReportFilter>();
            UserAppUtilizationFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Identities_id });

            string viewName = "VW_USERS_APPLICATION_UTILIZATION";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, UserAppUtilizationFilters);

            DataColumnCollection DCC = TempDataTable.Columns;

            #endregion

            #region User Cores
            //Get all cores associated with identity
            List<DynamicModels.RootReportFilter> AllIdentityCoresFilters = new List<DynamicModels.RootReportFilter>();
            AllIdentityCoresFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Identities_id });
            AllIdentityCoresFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CREATOR_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "N" });
            viewName = "VW__CORES_IDENTITIES";

            DataTable AllIdentityCores = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { columns = "Cores_ID 'Identity_Cores'", length = -1, order = "1 asc", start = 0, verify = "T" }, AllIdentityCoresFilters);
            #endregion

            DataColumnCollection DCC_AllIdentityCores = AllIdentityCores.Columns;

            if (DCC_AllIdentityCores.Contains("IDENTITIES_ID"))
            {
                foreach (DataRow row in AllIdentityCores.Rows)
                {
                    coreid = row["Identity_Cores"].ToString();

                    //Get max number of applications allowed by Owner
                    #region Get max number of applications allowed by Owner
                    List<DynamicModels.RootReportFilter> thresholdFilters = new List<DynamicModels.RootReportFilter>();
                    thresholdFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = coreid });

                    viewName = "VW_USERS_CORE_APP_THRESHOLDS";

                    DataTable OwnerLimits = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                        new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, thresholdFilters);
                    #endregion

                    DataColumnCollection DCC_OwnerLimits = OwnerLimits.Columns;

                    ownerIdentityId = OwnerLimits.Rows[0]["OwnerIdentity"].ToString().ToLower();

                    if (coreid == "1000")
                        ownerAppsAllowed = DCC.Contains("UserAllowed") && TempDataTable.Rows.Count > 0 ? TempDataTable.Rows[0]["UserAllowed"].ToString().ToLower() : "-1";
                    else
                        ownerAppsAllowed = DCC_OwnerLimits.Contains("OwnerIdentity") && OwnerLimits.Rows.Count > 0 ? OwnerLimits.Rows[0]["OwnerAppsAllowed"].ToString().ToLower() : "-1";

                    if (ownerIdentityId == "1000")
                        ownerAppsAllowed = "unlimited";

                    if (ownerAppsAllowed == "unlimited")
                        break;

                    //Get all applications utilised by owner within the core
                    List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "OWNER_IDENTITIES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = ownerIdentityId });
                    Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = coreid });

                    viewName = "VW_USERS_CORE_OWNER_USAGE";

                    DataTable OwnerUtilised = _DynamicOutputProcedures._DynoProcSearch(_Connect, "count Query", "SP_S_" + viewName + "_SEARCH",
                        new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

                    DataColumnCollection DCC_OwnerUtilised = OwnerUtilised.Columns;

                    ownerAppsUtilised = DCC_OwnerUtilised.Contains("count") && OwnerUtilised.Rows.Count > 0 ? ER_Tools.ConvertToInt64(OwnerUtilised.Rows[0]["count"].ToString()) : -1;

                    OwnerAppsRemaining = OwnerAppsRemaining + (ER_Tools.ConvertToInt64(ownerAppsAllowed) - ownerAppsUtilised);
                }
            }

            if (ownerAppsAllowed == "unlimited")
                appsRemaining = "unlimited";
            else
                appsRemaining = DCC.Contains("UserAllowed") && TempDataTable.Rows.Count > 0 ? Convert.ToString(OwnerAppsRemaining + ER_Tools.ConvertToInt64(TempDataTable.Rows[0]["UserAllowed"].ToString())) : "-1";

            //Get new total of applications allowed by Identity
            TempDataTable.Columns["UserAllowed"].ReadOnly = false;
            TempDataTable.Rows[0]["UserAllowed"] = appsRemaining.ToString();
            TempDataTable.Columns["UserAllowed"].ReadOnly = true;

            return TempDataTable;
        }

        public bool SetAppName(string name)
        {
            HttpContext.Current.Session["NewFormName"] = name;
            return true;
        }

        public bool CanUseAppName(IConnectToDB _Connect, string name, Guid? cores_id, Guid? baseapp)
        {
            ER_Query er_query = new ER_Query();

            bool canUseName = false;

            if (er_query.CanIdentityConnect(_Connect))
            {
                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATION_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = name });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.UniqueIdentifier, ParamValue = cores_id });

                //string viewName = "VW__ER_APP_NAMES";
                string viewName = "VW__APPLICATIONS";

                DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "APPLICATION_NAME", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

                DataColumnCollection DCC = TempDataTable.Columns;

                if (DCC.Contains("APPLICATION_NAME"))
                {
                    if (TempDataTable.Rows.Count > 0)
                    {
                        if (baseapp == null)
                        {
                            canUseName = false;
                        }
                        else
                        {
                            if (baseapp != null)
                            {
                                List<DynamicModels.RootReportFilter> Filters2 = new List<DynamicModels.RootReportFilter>();
                                Filters2.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATION_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = name });
                                Filters2.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.UniqueIdentifier, ParamValue = cores_id });
                                Filters2.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_APPLICATIONS_UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = baseapp });

                                DataTable TempDataTable2 = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                                    new DataTableDotNetModelMetaData { columns = "APPLICATION_NAME", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters2);

                                DataColumnCollection DCC2 = TempDataTable2.Columns;

                                if (TempDataTable2.Rows.Count > 0)
                                {
                                    canUseName = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        canUseName = true;
                    }
                }
                else
                {
                    canUseName = false; //Error Querying Do Not Grant App Name
                }
            }

            return canUseName;
        }

        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });

            string viewName = "VW_APPLICATION_LATEST_RENDITION_AND_CREATOR";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "APPLICATIONS_ID DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }

        public DataTable FindbyBaseColumnID(IConnectToDB _Connect, string _column, string _value)
        {
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "APPLICATIONS_UUID", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = _value },
            };

            SQlin.sqlIn = "SELECT * FROM CSA.VW_APPLICATION_LATEST_RENDITION_AND_CREATOR WHERE APPLICATIONS_UUID = @APPLICATIONS_UUID OR BASE_APPLICATIONS_UUID = @APPLICATIONS_UUID";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            return TempDataTable;
        }

        public DataTable FindbyColumnIDAndCore(IConnectToDB _Connect, string _column, string _value, string core)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = core });

            string viewName = "VW_APPLICATION_LATEST_RENDITION_AND_CREATOR";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "APPLICATIONS_ID DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }

        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, List<string> _value)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column, DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = String.Join(",", _value) });

            string viewName = "VW_APPLICATION_LATEST_RENDITION_AND_CREATOR";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "APPLICATIONS_ID DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }

        public DataTable FindAllDetails(IConnectToDB _Connect, string detailtype)
        {
            string viewName = "";

            switch (detailtype.ToLower())
            {
                case "char": viewName = "VW__APPLICATIONS_DAT_CHAR"; break;
                case "numb": viewName = "VW__APPLICATIONS_DAT_NUMB"; break;
                case "date": viewName = "VW__APPLICATIONS_DAT_DATE"; break;
            }

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            DataTable TempDataTable = new DataTable();
            if (!string.IsNullOrWhiteSpace(viewName))
            {
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                             new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                             Filters);
            }

            return TempDataTable;
        }

        public string GetAppID(IConnectToDB _Connect, string name)
        {

            DataTable tempDataTable = FindbyColumnID(_Connect, "application_link", name);
            string value = "";

            if (tempDataTable.Rows.Count > 0)
            {
                value = tempDataTable.Rows[0][0].ToString();
            }

            return value;
        }

        public string GetAppCoreID(IConnectToDB _Connect, string name)
        {

            DataTable tempDataTable = FindbyColumnID(_Connect, "application_link", name);
            string value = "";

            if (tempDataTable.Rows.Count > 0)
            {
                value = tempDataTable.Rows[0]["CORES_ID"].ToString();
            }

            return value;
        }

        public string GetAppCoreID(IConnectToDB _Connect, int id)
        {

            DataTable tempDataTable = FindbyColumnID(_Connect, "applications_id", id.ToString());
            string value = "";

            if (tempDataTable.Rows.Count > 0)
            {
                value = tempDataTable.Rows[0]["CORES_ID"].ToString();
            }

            return value;
        }

        public Guid? GetAppCoreUUID(IConnectToDB _Connect, Guid? id)
        {
            DataTable tempDataTable = FindbyBaseColumnID(_Connect, "applications_uuid", id.ToString());

            Guid? value = null;

            if (tempDataTable.Rows.Count > 0)
            {
                value = ER_Tools.ConvertToGuid(tempDataTable.Rows[0]["CORES_UUID"].ToString());
            }

            return value;
        }

        public string GetAppIDByCore(IConnectToDB _Connect, string name, string core)
        {

            DataTable tempDataTable = FindbyColumnIDAndCore(_Connect, "application_link", name, core);
            string value = "";

            if (tempDataTable.Rows.Count > 0)
            {
                value = tempDataTable.Rows[0]["APPLICATIONS_ID"].ToString();
            }

            return value;
        }

        public string GetRootAppName(IConnectToDB _Connect, string id)
        {

            DataTable tempDataTable = FindbyColumnID(_Connect, "applications_id", id);
            string value = "";

            if (tempDataTable.Rows.Count > 0)
            {
                value = tempDataTable.Rows[0]["ROOT_APPLICATION"].ToString();
            }

            return value;
        }

        public string GetRootAppNameViaBaseUUID(IConnectToDB _Connect, string uuid)
        {

            DataTable tempDataTable = FindbyColumnID(_Connect, "base_applications_uuid", uuid);
            string value = "";

            if (tempDataTable.Rows.Count > 0)
            {
                value = tempDataTable.Rows[0]["ROOT_APPLICATION"].ToString();
            }

            return value;
        }
        public DataTable GetAppLastSaveTime(IConnectToDB _Connect, string uuid)
        {
            DataTable tempDataTable = FindbyColumnID(_Connect, "applications_uuid", uuid);

            return tempDataTable;
        }
        public string GetAppName(IConnectToDB _Connect, long? id)
        {

            DataTable tempDataTable = FindbyColumnID(_Connect, "applications_id", id.ToString());
            string value = "";

            if (tempDataTable.Rows.Count > 0)
            {
                value = tempDataTable.Rows[0]["APPLICATION_NAME"].ToString();
            }

            return value;
        }
        public string GetAppOwnerId(IConnectToDB _Connect, string applicationid)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = applicationid });

            string viewName = "VW_APPLICATION_LATEST_RENDITION_AND_CREATOR";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { length = 1, order = "APPLICATIONS_ID DESC", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("IDENTITIES_ID"))
                return TempDataTable.Rows[0]["IDENTITIES_ID"].ToString();

            return "";
        }
        public DataTable GetAppProperties(IConnectToDB _Connect, string id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = id });

            string viewName = "VW_OBJECT_SETTINGS";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { columns = "PROPERTY_NAME, PROPERTY_VALUE", length = -11, order = "1 DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }

        public SessionObjects GetApplicationObjects(IConnectToDB _Connect, SessionObjects SO, long? applications_id, string Identities_ID)
        {
            //ApplicationObjects appObjects = new ApplicationObjects();
            //SessionObjects viewModel = new SessionObjects();

            SO.application_id = applications_id;
            SO.application_name = "Test Name";
            SO.cores_id = 1000;

            SO.application_name = GetAppName(_Connect, applications_id);
            //SO.stage = GetApplicationStages(_Connect, applications_id, "Form");
            SO = GetAllApplicationObjects(_Connect, SO, applications_id);

            //Notifications NotificationsHelper = new Notifications();

            //SO.SessionNotification = NotificationsHelper.Get(_Connect, Int32.Parse(Identities_ID));

            return SO;
        }

        public SessionObjects GetApplicationObjectsByAppName(IConnectToDB _Connect, SessionObjects SO, string appName, string Identities_ID)
        {
            //ApplicationObjects appObjects = new ApplicationObjects();
            //SessionObjects viewModel = new SessionObjects();

            //SO.application_id = Convert.ToInt32(applications_id);
            SO.application_name = "Test Name";
            SO.cores_id = 1000;

            SO.application_name = appName;//GetAppName(_Connect, applications_id);
            SO.stage = GetApplicationStagesByAppName(_Connect, appName, "Form");
            //SO = GetAllApplicationObjects(_Connect,SO,  applications_id, "Form");


            //Notifications NotificationsHelper = new Notifications();

            //SO.SessionNotification = NotificationsHelper.Get(_Connect, Int32.Parse(Identities_ID));

            return SO;
        }

        public SessionObjects GetAllApplicationObjects(IConnectToDB _Connect, SessionObjects SO, long? applications_id)
        {
            SO.application_id = applications_id;

            ViewApplicationModel thisNewApp = new ViewApplicationModel();
            List<DynamicModels.RootReportFilter> thisAppFilter = new List<DynamicModels.RootReportFilter>();

            thisAppFilter.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", ParamValue = applications_id });

            DataTable thisAppDT = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__APPLICATIONS_SEARCH",
                   new DataTableDotNetModelMetaData { length = -1, order = "", start = 0, verify = "T" },
                   thisAppFilter);

            DataColumnCollection appDCC = thisAppDT.Columns;

            if (appDCC.Contains("APPLICATIONS_ID") && thisAppDT.Rows.Count > 0)
            {
                string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                      .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                                ).ToString(Formatting.None);

                thisNewApp = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewApplicationModel>(thisAppRow);

                List<DynamicModels.RootReportFilter> AppObjectQueryFilters = new List<DynamicModels.RootReportFilter>();

                AppObjectQueryFilters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", ParamValue = applications_id });

                List<ViewStageModel> AppObjects = GetAllApplicationStageObjects(_Connect, AppObjectQueryFilters, false);

                AppObjects = CreateRenderFromStages(_Connect, false, AppObjects);

                SO.application_name = thisNewApp.application_name;
                SO.cores_id = thisNewApp.cores_id;
                SO.cores_uuid = thisNewApp.cores_uuid;
                SO.stage = AppObjects;
            }

            return SO;
        }

        public ViewApplicationModel GetApplicationStruct(IConnectToDB _Connect, SessionObjects SO, Guid? uuid, bool withRoles)
        {
            ViewApplicationModel thisNewApp = new ViewApplicationModel();
            List<DynamicModels.RootReportFilter> thisAppFilter = new List<DynamicModels.RootReportFilter>();

            thisAppFilter.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_UUID_", ParamValue = uuid });

            thisNewApp = FetchApplication(_Connect, SO, uuid, withRoles, thisNewApp, thisAppFilter);

            return thisNewApp;
        }

        public ViewApplicationModel GetBaseApplicationStruct(IConnectToDB _Connect, SessionObjects SO, Guid? uuid, bool withRoles)
        {
            ViewApplicationModel thisNewApp = new ViewApplicationModel();
            List<DynamicModels.RootReportFilter> thisAppFilter = new List<DynamicModels.RootReportFilter>();

            thisAppFilter.Add(new DynamicModels.RootReportFilter { FilterName = "GET_LATEST", ParamValue = 'T' });
            thisAppFilter.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_APPLICATIONS_UUID_", ParamValue = uuid });

            #region Update App UUID to latest from Base UUID
            List<DynamicModels.RootReportFilter> GetLatestApplicationsUUID = new List<DynamicModels.RootReportFilter>();

            GetLatestApplicationsUUID.Add(new DynamicModels.RootReportFilter { FilterName = "GET_LATEST", ParamValue = "T" });
            GetLatestApplicationsUUID.Add(new DynamicModels.RootReportFilter { FilterName = "BASE_APPLICATIONS_UUID_", ParamValue = uuid });

            DataTable GetLatestAppData = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__APPLICATIONS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, GetLatestApplicationsUUID);

            DataColumnCollection dcc = GetLatestAppData.Columns;
            if (dcc.Contains("APPLICATIONS_UUID") && GetLatestAppData.Rows.Count > 0)
            {
                uuid = GetLatestAppData.Rows[0].Field<Guid?>("APPLICATIONS_UUID");
            }
            #endregion

            thisNewApp = FetchApplication(_Connect, SO, uuid, withRoles, thisNewApp, thisAppFilter);

            return thisNewApp;
        }

        private ViewApplicationModel FetchApplication(IConnectToDB _Connect, SessionObjects SO, Guid? uuid, bool withRoles, ViewApplicationModel thisNewApp, List<DynamicModels.RootReportFilter> thisAppFilter)
        {
            DataTable thisAppDT = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__APPLICATIONS_SEARCH",
                   new DataTableDotNetModelMetaData { length = -1, order = "", start = 0, verify = "T" },
                   thisAppFilter);

            DataColumnCollection appDCC = thisAppDT.Columns;

            if (appDCC.Contains("APPLICATIONS_ID") && thisAppDT.Rows.Count > 0)
            {
                string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                      .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                                ).ToString(Formatting.None);

                thisNewApp = JsonConvert.DeserializeObject<ViewApplicationModel>(thisAppRow);

                List<DynamicModels.RootReportFilter> AppObjectQueryFilters = new List<DynamicModels.RootReportFilter>();

                AppObjectQueryFilters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_UUID_", ParamValue = uuid });

                List<ViewStageModel> AppObjects = GetAllApplicationStageObjects(_Connect, AppObjectQueryFilters, withRoles);

                List<IdentityRoles> thisPersonRoles = SecurityHelper.GetIdentityRoles(_Connect, SO.SessionIdentity.Identity.identities_uuid, "Y");

                thisNewApp.stages = AppObjects;

                if (withRoles)
                {
                    List<DynamicModels.RootReportFilter> AppRoles = new List<DynamicModels.RootReportFilter>();

                    AppRoles.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_UUID_", ParamValue = uuid });

                    DataTable thisAppRolesDT = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__APPLICATIONS_SEC_ROLE_SEARCH",
                        new DataTableDotNetModelMetaData { length = -1, order = "", start = 0, verify = "T" },
                        AppRoles);

                    DataColumnCollection appRoleDCC = thisAppRolesDT.Columns;

                    if (appRoleDCC.Contains("APPLICATIONS_UUID") && thisAppRolesDT.Rows.Count > 0)
                    {
                        string thisAppRoleJSON = JsonConvert.SerializeObject(thisAppRolesDT, Formatting.Indented);

                        thisNewApp.roles = JsonConvert.DeserializeObject<List<SecurityApplicationRoles>>(thisAppRoleJSON);
                    }
                }

            }

            return thisNewApp;
        }

        /// <summary>
        /// This Method is very important in converting the SQL that returns us the Application CastGoop Data into a list of Stages.
        /// </summary>
        /// <param name="_Connect"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public List<ViewStageModel> GetAllApplicationStageObjects(IConnectToDB _Connect, List<DynamicModels.RootReportFilter> filters, bool withRoles)
        {
            //TOLEARN: this is very important to the overall system.
            #region Load App Stages
            DataTable AppObjectsTableResult = DB.Binds.IO.Dynamic._DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_APPLICATION_OBJECTS_SEARCH",
                   new DataTableDotNetModelMetaData { length = -1, order = "APPLICATIONS_ID asc, STAGES_ID asc, GRIPS_ID asc, OBJECT_SETS_ID asc, OBJ_PROP_SETS_ID asc ", start = 0, verify = "T" },
                   filters);

            List<ViewStageModel> Stages = new List<ViewStageModel>();

            long? currentStageID = 0;
            int currentStageIteration = 0;
            long? currentGripID = 0;
            int currentGripIteration = 0;
            long? currentObjSetsID = 0;
            int currentObjSetsIteration = 0;
            long? currentObjPropSetsID = 0;
            int currentObjPropSetsIteration = 0;
            long? currentObjPropOptSetsID = 0;
            int currentObjPropOptSetsIteration = 0;

            if (AppObjectsTableResult.Rows.Count > 0 && AppObjectsTableResult.Columns.Contains("CORES_ID") && AppObjectsTableResult.Columns.Contains("APPLICATIONS_ID"))
            {
                #region Major Work For Populating Models
                for (int i = 0; i < AppObjectsTableResult.Rows.Count; i++)
                {
                    //TOLEARN: People need to learn how to do this basic function. Its take an DataRow and converts it to a Json string;
                    string thisCASTGOOPRow = new JObject(AppObjectsTableResult.Columns.Cast<DataColumn>()
                              .Select(c => new JProperty(c.ColumnName, JToken.FromObject(AppObjectsTableResult.Rows[i][c])))
                        ).ToString(Formatting.None);

                    #region Stages
                    if (currentStageID != AppObjectsTableResult.Rows[i].Field<long?>("stages_id"))
                    {
                        Stages.Add(new ViewStageModel());

                        thisCASTGOOPRow = thisCASTGOOPRow.Replace("stage_dt_available", "dt_available");
                        thisCASTGOOPRow = thisCASTGOOPRow.Replace("stage_date_created", "dt_created");
                        thisCASTGOOPRow = thisCASTGOOPRow.Replace("stage_dt_end", "dt_end");
                        thisCASTGOOPRow = thisCASTGOOPRow.Replace("stage_object_type", "object_type");
                        thisCASTGOOPRow = thisCASTGOOPRow.Replace("stage_object_layer", "object_layer");

                        //TOLEARN: People need to learn how to do this basic function. Its take An string of JSON and Converts it to a c# class model.
                        ViewStageModel thisNewStage = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewStageModel>(thisCASTGOOPRow);
                        Stages[currentStageIteration] = thisNewStage;
                        currentStageIteration++;
                        currentStageID = AppObjectsTableResult.Rows[i].Field<long?>("stages_id");
                        currentGripIteration = 0;
                    }
                    #endregion

                    #region Grips
                    if (currentGripID != AppObjectsTableResult.Rows[i].Field<long?>("grips_id"))
                    {
                        ViewGripModel thisNewGrip = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewGripModel>(thisCASTGOOPRow);
                        Stages[currentStageIteration - 1].Grips.Add(thisNewGrip);
                        currentGripIteration++;
                        currentGripID = AppObjectsTableResult.Rows[i].Field<long?>("grips_id");
                        currentObjSetsIteration = 0;
                    }
                    #endregion

                    #region Object Sets
                    if (currentObjSetsID != AppObjectsTableResult.Rows[i].Field<long?>("object_sets_id"))
                    {
                        ViewObjectSetModel thisNewObjectSet = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewObjectSetModel>(thisCASTGOOPRow);
                        Stages[currentStageIteration - 1].Grips[currentGripIteration - 1].ObjectSets.Add(thisNewObjectSet);
                        currentObjSetsIteration++;
                        currentObjSetsID = AppObjectsTableResult.Rows[i].Field<long?>("Object_sets_id");
                        currentObjPropSetsIteration = 0;
                    }
                    #endregion

                    #region Properties
                    if (currentObjPropSetsID != AppObjectsTableResult.Rows[i].Field<long?>("obj_prop_sets_id"))
                    {
                        ViewObjectPropSetsModel thisNewViewObjectProp = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewObjectPropSetsModel>(thisCASTGOOPRow);
                        Stages[currentStageIteration - 1].Grips[currentGripIteration - 1].ObjectSets[currentObjSetsIteration - 1].ObjectPropSets.Add(thisNewViewObjectProp);
                        currentObjPropSetsIteration++;
                        currentObjPropSetsID = AppObjectsTableResult.Rows[i].Field<long?>("obj_prop_sets_id");
                        currentObjPropOptSetsIteration = 0;
                    }
                    #endregion

                    #region Options
                    if (currentObjPropOptSetsID != AppObjectsTableResult.Rows[i].Field<long?>("obj_prop_opt_sets_id"))
                    {
                        if (AppObjectsTableResult.Rows[i].Field<long?>("obj_prop_opt_sets_id") != null)
                        {
                            ViewObjectPropOptSetsModel thisNewOptionSet = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewObjectPropOptSetsModel>(thisCASTGOOPRow);
                            Stages[currentStageIteration - 1].Grips[currentGripIteration - 1].ObjectSets[currentObjSetsIteration - 1].ObjectPropSets[currentObjPropSetsIteration - 1].ObjectPropOptSets.Add(thisNewOptionSet);
                            currentObjPropOptSetsID = AppObjectsTableResult.Rows[i].Field<long?>("obj_prop_opt_sets_id");
                        }

                        currentObjPropOptSetsIteration++;
                    }
                    #endregion

                }
                #endregion
            }

            #endregion
            if (withRoles)
            {
                foreach (var item in Stages)
                {
                    List<DynamicModels.RootReportFilter> StageRoles = new List<DynamicModels.RootReportFilter>();

                    StageRoles.Add(new DynamicModels.RootReportFilter { FilterName = "STAGES_UUID_", ParamValue = item.stages_uuid });

                    DataTable thisAppRolesDT = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__STAGES_SEC_ROLE_SEARCH",
                        new DataTableDotNetModelMetaData { length = -1, order = "", start = 0, verify = "T" },
                        StageRoles);

                    DataColumnCollection appRoleDCC = thisAppRolesDT.Columns;

                    if (appRoleDCC.Contains("STAGES_UUID") && thisAppRolesDT.Rows.Count > 0)
                    {
                        //TOLEARN: Serialize Datatable to Object
                        string thisStageRoleJSON = JsonConvert.SerializeObject(thisAppRolesDT, Formatting.Indented);

                        item.roles = JsonConvert.DeserializeObject<List<SecurityStageRoles>>(thisStageRoleJSON);
                    }
                }
            }

            return Stages;
        }

        //TOLEARN: this is very important to the overall system.
        public static List<ViewStageModel> CreateRenderFromStages(IConnectToDB _Connect, bool withRoles, List<ViewStageModel> Stages)
        {
            if (Stages != null)
            {
                #region Load All Grids Objects
                foreach (ViewStageModel thisStage in Stages)
                {
                    foreach (ViewGripModel thisGrip in thisStage.Grips)
                    {
                        foreach (ViewObjectSetModel item in thisGrip.ObjectSets)
                        {
                            bool thisHasGridWidget = item.ObjectPropSets.Exists(i => i.object_type == "Grid Widget");
                            if (thisHasGridWidget)
                            {
                                thisStage.Gridster.Add(item);
                            }
                        }
                    }
                }
                #endregion

                #region Load Add Grid Properties
                foreach (ViewStageModel thisStage in Stages)
                {
                    foreach (ViewObjectSetModel thisGrid in thisStage.Gridster)
                    {
                        Dictionary<string, object> thisSet = new Dictionary<string, object>();
                        foreach (ViewObjectPropSetsModel item in thisGrid.ObjectPropSets)
                        {
                            //Get section image
                            if (item.property_name.ToLower() == "section_background_image")
                            {
                                ObjectPropSetsHelper objectPropSetsHelper = new ObjectPropSetsHelper();

                                thisSet[item.property_name.ToLower()] = objectPropSetsHelper.GetObjPropFile(_Connect, item.object_sets_uuid);
                            }
                            else
                            {
                                thisSet[item.property_name.ToLower()] = item.property_value;
                            }
                        }

                        thisStage.GridsterGrids.Add(thisSet);
                    }
                }
                #endregion

                #region Load all Objects In Grid Container
                foreach (ViewStageModel thisStage in Stages)
                {
                    foreach (ViewGripModel thisGrip in thisStage.Grips)
                    {
                        List<ViewObjectSetModel> thisObject = thisGrip.ObjectSets;

                        foreach (var thisGrid in thisStage.GridsterGrids)
                        {
                            List<ViewObjectSetModel> childObjects = thisObject.Where(o => o.ObjectPropSets.Any(p => p.property_name == "Widget" && p.property_value == thisGrid["id"].ToString())).ToList();

                            if (childObjects.Count > 0)
                            {
                                thisGrid["childObjects"] = childObjects;
                            }
                            else
                            {
                                thisGrid["childObjects"] = thisGrid.ContainsKey("childObjects") ? thisGrid["childObjects"] : new List<ViewObjectSetModel>();
                            }
                        }
                    }
                }
                #endregion

                #region Parse All Property objects and Load in Child Containers
                foreach (ViewStageModel thisStage in Stages)
                {
                    foreach (var thisGrid in thisStage.GridsterGrids)
                    {
                        thisGrid["childObjectsParsed"] = new List<Dictionary<string, object>>();

                        if (thisGrid["childObjects"] != null)
                        {
                            List<ViewObjectSetModel> childObjects = (List<ViewObjectSetModel>)thisGrid["childObjects"];

                            if (childObjects.Count > 0)
                            {
                                foreach (ViewObjectSetModel thisObject in childObjects)
                                {
                                    Dictionary<string, object> thisSet = new Dictionary<string, object>();

                                    foreach (ViewObjectPropSetsModel thisProp in thisObject.ObjectPropSets)
                                    {
                                        thisGrid["childObjectOptsParsed"] = new List<Dictionary<string, object>>();

                                        thisSet["uuid"] = thisProp.object_sets_uuid;
                                        thisSet["object_type"] = thisProp.object_type;
                                        thisSet["partial"] = "partial_" + thisProp.object_type.ToLower();

                                        if (thisProp.property_name.ToLower() == "id")
                                        {
                                            thisSet["name"] = thisProp.property_value;
                                        }

                                        //Images
                                        if (thisProp.object_type == "Image")
                                        {
                                            ObjectPropSetsHelper objectPropSetsHelper = new ObjectPropSetsHelper();
                                            thisSet["imageurl"] = objectPropSetsHelper.GetObjPropFile(_Connect, thisProp.object_sets_uuid);
                                        }

                                        //File upload
                                        //if (thisProp.object_type.ToLower() == "file_upload" && thisProp.property_name.ToLower() == "file_accept")
                                        //{
                                        //    thisSet["fileTypes"] = thisProp.property_value.Replace("|",",");
                                        //}                                        

                                        if (thisProp.object_prop_type == "HTML")
                                        {
                                            thisSet[thisProp.property_name.ToLower()] = HttpUtility.HtmlDecode(thisProp.property_value);
                                        }
                                        else
                                        {
                                            thisSet[thisProp.property_name.ToLower()] = thisProp.property_value;
                                        }

                                        foreach (ViewObjectPropOptSetsModel thisPropOpt in thisProp.ObjectPropOptSets)
                                        {
                                            Dictionary<string, object> thisOptSet = new Dictionary<string, object>();

                                            if (!String.IsNullOrEmpty(thisPropOpt.option_value))
                                            {
                                                thisOptSet["name"] = thisPropOpt.option_name;
                                                thisOptSet["value"] = thisPropOpt.option_value;
                                            }
                                            else
                                            {
                                                thisOptSet["name"] = thisPropOpt.option_name;
                                                thisOptSet["value"] = thisPropOpt.option_name;
                                            }

                                            ((List<Dictionary<string, object>>)thisGrid["childObjectOptsParsed"]).Add(thisOptSet);
                                        }

                                        thisSet["options"] = thisGrid["childObjectOptsParsed"];
                                    }

                                    ((List<Dictionary<string, object>>)thisGrid["childObjectsParsed"]).Add(thisSet);
                                }
                            }
                        }
                    }
                }
                #endregion


            }

            return Stages;
        }

        public SessionObjects GetApplicationObjectViaStage(SessionObjects SO, IConnectToDB _Connect, string actionname, string controller, string applications_id, string _stageType, string _stageName)
        {
            //SessionObjects viewModel = new SessionObjects();

            StagesHelper stages = new StagesHelper();

            SO.application_id = Convert.ToInt32(applications_id);
            // SO.application_name = "Test Name";

            SO.actionname = actionname;
            SO.controller = controller;

            //SO.application_name = "Change Name in Fetcher";
            SO.stage = stages.GetStage(_Connect, applications_id, _stageType, _stageName);
            SO.application_name = SO.stage.First().application_name;


            SO.application_properties = GetAppProperties(_Connect, applications_id);

            return SO;
        }

        public List<ViewStageModel> GetApplicationStages(IConnectToDB _Connect, string applications_id, string stage_type)
        {
            SessionObjects appObjects = new SessionObjects();

            StagesHelper stages = new StagesHelper();

            DataTable stagesdt;

            if (applications_id.ToLower() == "all")
            {
                stagesdt = stages.FindAll(_Connect);
            }
            else
            {
                stagesdt = stages.FindbyColumnID(_Connect, "applications_id", applications_id, stage_type);
            }
            //TODO: Verify .Net Core Code
            stagesdt.DefaultView.Sort = "stages_id asc";
            stagesdt = stagesdt.DefaultView.ToTable();

            List<ViewStageModel> StageList = new List<ViewStageModel>();

            ViewStageModel[] Stages = new ViewStageModel[stagesdt.Rows.Count];

            int i = 0;
            foreach (DataRow datarowdc in stagesdt.Rows)
            {

                Stages[i] = new ViewStageModel
                {
                    applications_id = datarowdc.Field<Int32>("applications_id"),
                    containers_id = datarowdc.Field<Int32>("containers_id"),
                    cores_id = datarowdc.Field<Int32>("cores_id"),
                    dt_available = datarowdc.Field<DateTime?>("dt_available"),
                    dt_created = datarowdc.Field<DateTime>("dt_created"),
                    dt_end = datarowdc.Field<DateTime?>("dt_end"),
                    enabled = datarowdc.Field<string>("enabled"),
                    identities_id = datarowdc.Field<Int32>("identities_id"),
                    object_layer = datarowdc.Field<string>("object_layer"),
                    object_type = datarowdc.Field<string>("object_type"),
                    stage_name = datarowdc.Field<string>("stage_name"),
                    stage_link = datarowdc.Field<string>("stage_link"),
                    stage_type = datarowdc.Field<string>("stage_type"),
                    stages_id = datarowdc.Field<Int32>("stages_id"),
                    Grips = stages.GetGripsViaStage(_Connect, datarowdc.Field<Int32>("stages_id").ToString())
                };

                StageList.Add(Stages[i]);
                i++;
            }

            return StageList;
        }

        public List<ViewStageModel> GetApplicationStagesByAppName(IConnectToDB _Connect, string appName, string stage_type)
        {
            SessionObjects appObjects = new SessionObjects();

            StagesHelper stages = new StagesHelper();

            DataTable stagesdt;

            if (appName.ToLower() == "all")
            {
                stagesdt = stages.FindAll(_Connect);
            }
            else
            {
                stagesdt = stages.FindbyColumnID(_Connect, "application_name", appName, stage_type);
            }
            //TODO: Verify .Net Core Code
            stagesdt.DefaultView.Sort = "stages_id asc";
            stagesdt = stagesdt.DefaultView.ToTable();

            List<ViewStageModel> StageList = new List<ViewStageModel>();

            ViewStageModel[] Stages = new ViewStageModel[stagesdt.Rows.Count];

            int i = 0;
            foreach (DataRow datarowdc in stagesdt.Rows)
            {

                Stages[i] = new ViewStageModel
                {
                    applications_id = datarowdc.Field<Int32>("applications_id"),
                    containers_id = datarowdc.Field<Int32>("containers_id"),
                    cores_id = datarowdc.Field<Int32>("cores_id"),
                    dt_available = datarowdc.Field<DateTime?>("dt_available"),
                    dt_created = datarowdc.Field<DateTime>("dt_created"),
                    dt_end = datarowdc.Field<DateTime?>("dt_end"),
                    enabled = datarowdc.Field<string>("enabled"),
                    identities_id = datarowdc.Field<Int32>("identities_id"),
                    object_layer = datarowdc.Field<string>("object_layer"),
                    object_type = datarowdc.Field<string>("object_type"),
                    stage_name = datarowdc.Field<string>("stage_name"),
                    stage_link = datarowdc.Field<string>("stage_link"),
                    stage_type = datarowdc.Field<string>("stage_type"),
                    stages_id = datarowdc.Field<Int32>("stages_id"),
                    Grips = stages.GetGripsViaStage(_Connect, datarowdc.Field<Int32>("stages_id").ToString())
                };

                StageList.Add(Stages[i]);
                i++;
            }

            return StageList;
        }

        public List<ViewStageModel> GetApplicationStage(IConnectToDB _Connect, string stages_id, string stage_type)
        {
            SessionObjects appObjects = new SessionObjects();

            StagesHelper stages = new StagesHelper();

            DataTable stagesdt;

            if (stages_id.ToLower() == "all")
            {
                stagesdt = stages.FindAll(_Connect);
            }
            else
            {
                stagesdt = stages.FindbyColumnID(_Connect, "stages_id", stages_id, stage_type);
            }

            List<ViewStageModel> StageList = new List<ViewStageModel>();

            ViewStageModel[] Stages = new ViewStageModel[stagesdt.Rows.Count];

            int i = 0;
            foreach (DataRow datarowdc in stagesdt.Rows)
            {

                Stages[i] = new ViewStageModel
                {
                    applications_id = datarowdc.Field<long>("applications_id"),
                    containers_id = datarowdc.Field<long>("containers_id"),
                    cores_id = datarowdc.Field<long>("cores_id"),
                    dt_available = datarowdc.Field<DateTime?>("dt_available"),
                    dt_created = datarowdc.Field<DateTime>("dt_created"),
                    dt_end = datarowdc.Field<DateTime?>("dt_end"),
                    enabled = datarowdc.Field<string>("enabled"),
                    identities_id = datarowdc.Field<long>("identities_id"),
                    object_layer = datarowdc.Field<string>("object_layer"),
                    object_type = datarowdc.Field<string>("object_type"),
                    stage_name = datarowdc.Field<string>("stage_name"),
                    stage_link = datarowdc.Field<string>("stage_link"),
                    stage_type = datarowdc.Field<string>("stage_type"),
                    stages_id = datarowdc.Field<long>("stages_id"),
                    Grips = stages.GetGripsViaStage(_Connect, datarowdc.Field<long>("stages_id").ToString())
                };

                StageList.Add(Stages[i]);
                i++;
            }

            return StageList;
        }

        private ViewApplicationModel AppView(ViewApplicationModel AppView, DataRow _DR)
        {
            //TOLEARN
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            AppView = JsonConvert.DeserializeObject<ViewApplicationModel>(thisAppRow);

            return AppView;
        }

        public List<ApplicationModels> Apps(IConnectToDB _Connect, CoreModels CoreModels, DataTable _DT, string ModelType, Boolean _GetStages, Boolean _GetGrips, Boolean _GetObjectSets, Boolean _GetPropertySets, Boolean _GetOptionProperties)
        {
            List<ApplicationModels> Apps = new List<ApplicationModels>();

            switch (ModelType.ToLower())
            {
                case "view":
                    foreach (DataRow AppViewRow in _DT.Rows)
                    {
                        ApplicationModels App = new ApplicationModels();
                        App.AppView = AppView(new ViewApplicationModel(), AppViewRow);

                        //ApplicationModels.Application AppHelper = new Application();
                        //App.AppView = AppHelper.Apps(CoreModels, AppHelper.FindbyColumnID("Microsoft", constrSQLServer2, owner, "cores_id", CoreModels.Core.Core.cores_id.ToString()), "View", _GetStages, _GetGrips);

                        //Counts
                        App.ActivityCounts = new List<ApplicationActivity>();
                        ActivityHelper AH = new ActivityHelper();
                        ReporterHelper RH = new ReporterHelper();

                        App.ActivityCounts = AH.ActivityCounts(new List<ApplicationActivity>(), RH.GetActivityCounts(_Connect, new DataTable(), App.AppView.applications_id, null, null));


                        if (_GetStages)
                        {
                            StagesHelper StagesHelper = new StagesHelper();
                            App.stages = StagesHelper.GetStageModels(_Connect, App, _GetGrips, _GetObjectSets, _GetPropertySets, _GetOptionProperties);

                        };
                        Apps.Add(App);
                    }
                    break;
                case "table":
                    break;
            }

            return Apps;
        }

        public DataTable GetApplicationEntryFileById(IConnectToDB _Connect, long? file_id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "FORMS_DAT_FILE_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = file_id });

            string viewName = "VW__FORMS_DAT_FILE";

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH",
                new DataTableDotNetModelMetaData { columns = "VALUE,FILE_NAME,CONTENT_TYPE", length = -1, order = "1 DESC", start = 0, verify = "T" }, Filters);

            return TempDataTable;


        }

        //TOLEARN: Refactor this to use CASTGOOP
        public string AddAppImage(IConnectToDB _Connect, ApplicationImage _appPic)
        {
            add addHelp = new add();
            DMLHelper DMLH = new DMLHelper();
            IOHelper io = new IOHelper();

            Values.AddFile thisFile = addHelp.ADD_ENTRY_FILE(_Connect, new Values.AddFile
            {
                I_FILE_NAME = "appimage",
                I_CONTENT_TYPE = ".jpg",
                I_FILE_SIZE = io.getBytes(_appPic.src).Length,
                I_FILE_DATA = io.getBytes(_appPic.src)
            });

            Values.AddFilePoint thisFilePoint = new Values.AddFilePoint();

            if (thisFile.O_FILES_ID > 0)
            {
                thisFilePoint = addHelp.ADD_FILE_POINT(_Connect, new Values.AddFilePoint
                {
                    I_FILES_ID = thisFile.O_FILES_ID,
                    I_OBJ_PROP_SETS_ID = _appPic.app_id //TODO: Risi Unfuck this App ID should not be passed to Prop set
                });
            }

            return thisFile.O_FILES_ID > 0 && thisFilePoint.O_OBJ_PROP_FILE_ID > 0 ? "file saved" : "error";
        }

        public List<ViewApplicationModel> GetPublicApps(IConnectToDB _Connect, SessionObjects SO)
        {
            //string SQLin = "SELECT a.DT_CREATED, a.APPLICATION_NAME, a.APPLICATIONS_UUID from " +
            //     "  CSA.VW_APPLICATION_LATEST_RENDITION_AND_CREATOR a " +
            //     "  INNER JOIN[CSA].VW__APPLICATIONS_SEC_ROLE b on(a.APPLICATIONS_UUID = b.APPLICATIONS_UUID) " +
            //     "  WHERE ROLE_NAME = @PRIV";

            //string SQLin = "select DT_CREATED, APPLICATION_NAME, APPLICATIONS_UUID, CORE_NAME from (SELECT a.DT_CREATED, a.APPLICATION_NAME, a.APPLICATIONS_UUID, a.CORE_NAME, b.ROLE_NAME, r.USER_NAME " +
            //"from CSA.VW_APPLICATION_LATEST_RENDITION_AND_CREATOR a " +
            //"LEFT JOIN [CSA].VW__APPLICATIONS_SEC_ROLE b on(a.APPLICATIONS_UUID = b.APPLICATIONS_UUID) " +
            //"LEFT JOIN [CSA].VW__IDENTITIES_ROLES r on(a.CORES_UUID = r.CORES_UUID and r.ROLE_NAME = b.ROLE_NAME) " +
            //"where a.APPLICATION_NAME not in ('Revamp System', 'Core Settings') and r.user_name is not null and r.IDENTITIES_UUID = @IDENTITIES_UUID) a";

            string SQLin = "SELECT DT_CREATED, APPLICATION_NAME, APPLICATIONS_UUID, CORE_NAME " +
            "FROM CSA.VW_APPLICATION_LATEST_RENDITION_AND_CREATOR a " +
            "WHERE APPLICATION_NAME NOT IN ('Revamp System', 'Core Settings') " +
            "AND IDENTITIES_UUID = @IDENTITIES_UUID ";

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                _dbParameters = new List<DBParameters>
                {
                    new DBParameters { ParamName = "IDENTITIES_UUID", ParamValue = SO._IdentityModel.identities_uuid }
                },
                sqlIn = SQLin
            };

            DataTable theseApps = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

            string theseAppsJson = JsonConvert.SerializeObject(theseApps, Formatting.Indented);

            List<ViewApplicationModel> AppsJSON = JsonConvert.DeserializeObject<List<ViewApplicationModel>>(theseAppsJson);

            return AppsJSON;
        }
    }
}
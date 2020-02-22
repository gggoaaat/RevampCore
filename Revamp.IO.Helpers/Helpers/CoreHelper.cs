using Microsoft.Security.Application;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Foundation;
using Revamp.IO.Structs;
using Revamp.IO.Structs.Models;
using Revamp.IO.Structs.Models.DataEntry;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace Revamp.IO.Helpers.Helpers
{
    public class CoreHelper
    {        
        public HttpContext Current => new HttpContextAccessor().HttpContext;
        SecurityHelper SECH = new SecurityHelper();

        public static bool CanUseCoreName(IConnectToDB _Connect, string Corename)
        {
            string[] NamesNotAllowed = CommonHelper.UnAllowedWords();

            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#"); //Used to Make Sure 

            bool canUseAttemptedCoreName = false;
            Corename = Sanitizer.GetSafeHtmlFragment(Corename.Trim());

            var a = Array.IndexOf(NamesNotAllowed, Corename.ToLower());
            var b = !string.IsNullOrWhiteSpace(Corename);
            var c = Corename.Length;
            var d = provider.IsValidIdentifier(Corename);

            if (Array.IndexOf(NamesNotAllowed, Corename.ToLower()) == -1
                && !string.IsNullOrWhiteSpace(Corename)
                && Corename.Length >= 3)
                //&& !provider.IsValidIdentifier(Corename)) ??Validates variable names
            {
                CoreHelper coreHelper = new CoreHelper();

                DataTable dt = coreHelper.FindbyColumnID(_Connect,
                    "CORE_NAME", Corename);
                canUseAttemptedCoreName = !(dt.Rows.Count > 0);
            }

            return canUseAttemptedCoreName;
        }

        

        public DataTable FindCoreRoles(IConnectToDB _Connect, Guid? uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.VarChar, ParamValue = uuid });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ROLES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "cores_id asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindAll(IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED desc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable Find(IConnectToDB _Connect, string id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "cores_id asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindCoreInfo(IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_CORE_INFO_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindCoreIdentityID(IConnectToDB _Connect, long? cores_id, long? identities_id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = cores_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = identities_id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindCoreInvitedByInviteKey(IConnectToDB _Connect, string InviteKey)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PROPERTY_VALUE_", DBType = SqlDbType.VarChar, ParamValue = InviteKey });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_CORE_INVITED_BY_INVITEKEY_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }


        /// <summary>
        /// Created this method to follow the current paradigm on the identities index page. -- Isaac Barnes 5/27/2017
        /// </summary>
        /// <param name="_Connect"></param>
        /// <param name="coreId"></param>
        /// <returns>List<ViewIdentityModel></returns>
        public List<ViewIdentityModel> GetAllInvitesForCore(IConnectToDB _Connect, string coreId)
        {
            ViewIdentityModel invite = new ViewIdentityModel();
            List<ViewIdentityModel> invites = new List<ViewIdentityModel>();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = coreId });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "S_S_VW_ALL_INVITES_FOR_CORE_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (DCC.Contains("CORES_ID"))
            {
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    invite = new ViewIdentityModel
                    {
                        email = TempDataTable.Rows[i].Field<string>("EMAIL"),
                        identities_id = i,
                        verified = TempDataTable.Rows[i].Field<string>("STATUS"),
                        dt_created = TempDataTable.Rows[i].Field<DateTime>("DT_CREATED"),
                        active = "",
                        object_type = "",
                        chardata = null,
                        datedata = null,
                        dt_available = null,
                        dt_end = null,
                        edipi = "",
                        enabled = "Y",
                        numbdata = null,
                        object_layer = "Invites",
                        user_name = ""
                    };

                    invites.Add(invite);
                }
            }

            return invites;

        }

        public string FindGripIdByCoreIdForInvitation(IConnectToDB _Connect, string CoreId)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> invitationFilters = new List<DynamicModels.RootReportFilter>();

            invitationFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = CoreId });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_GRIP_ID_BY_CORE_ID_FOR_INVITATION_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                invitationFilters);

            if (TempDataTable != null && TempDataTable.Rows.Count > 0)
            {
                return TempDataTable.Rows[0]["GRIPS_ID"].ToString();
            }
            else
            {
                List<DynamicModels.RootReportFilter> formFilters = new List<DynamicModels.RootReportFilter>();

                formFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = CoreId });

                TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_GRIP_ID_BY_CORE_ID_FOR_FORM_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    invitationFilters);

                if (TempDataTable != null && TempDataTable.Rows.Count > 0)
                    return TempDataTable.Rows[0]["GRIPS_ID"].ToString();
                else
                    return "1005";
            }
        }

        public int IsEmailAlreadyInvitedToCore(IConnectToDB _Connect, string CoreId, string email)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = CoreId });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PROPERTY_VALUE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = email });

            DataTable emailInvitedToCoreDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_EMAIL_ALREADY_INVITED_TO_CORE_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            if (emailInvitedToCoreDataTable != null && emailInvitedToCoreDataTable.Rows.Count > 0)
                return 1;
            else
            {
                List<DynamicModels.RootReportFilter> coreIdentitiesFilters = new List<DynamicModels.RootReportFilter>();

                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = CoreId });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EMAIL_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = email });

                DataTable coreIdentitiesDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_IDENTITIES_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "cores_id asc", start = 0, verify = "T" },
                    coreIdentitiesFilters);

                if (coreIdentitiesDataTable != null && coreIdentitiesDataTable.Rows.Count > 0)
                    return 2;
                else
                    return 0;
            }
        }

        public string CoreIdentityEnabled(IConnectToDB _Connect, long? cores_id, long? identities_id)
        {
            DataTable dt = FindCoreIdentityID(_Connect, cores_id, identities_id);

            string isEnabled = "N";
            if (dt.Rows.Count > 0)
            {
                isEnabled = dt.Rows[0]["enabled"].ToString();
            }

            return isEnabled;
        }

        public bool HasActiveCoreRoles(IConnectToDB _Connect, Guid? core_uuid, Guid? identities_uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            DataTable TempDataTable;
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_UUID_", DBType = SqlDbType.VarChar, ParamValue = identities_uuid });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = core_uuid });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });

            TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_ROLES_SEARCH",
                            new DataTableDotNetModelMetaData { columns = "IDENTITIES_UUID", length = -1, order = "1 asc", start = 0, verify = "T" },
                            Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("IDENTITIES_UUID"))
                return true;
            else
                return false;
        }

        public long? GetCoreIdentityId(IConnectToDB _Connect, Guid? cores_identities_uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            DataTable TempDataTable;
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_IDENTITIES_UUID_", DBType = SqlDbType.VarChar, ParamValue = cores_identities_uuid });

            TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_IDENTITIES_SEARCH",
                            new DataTableDotNetModelMetaData { columns = "CORES_IDENTITIES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                            Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("CORES_IDENTITIES_ID"))
                return TempDataTable.Rows[0].Field<long?>("CORES_IDENTITIES_ID");
            else
                return 0;
        }

        public long? GetCoreIdentityId(IConnectToDB _Connect, Guid? cores_uuid, Guid? identities_uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            DataTable TempDataTable;
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.VarChar, ParamValue = cores_uuid });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_UUID_", DBType = SqlDbType.VarChar, ParamValue = identities_uuid });

            TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_IDENTITIES_SEARCH",
                            new DataTableDotNetModelMetaData { columns = "CORES_IDENTITIES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                            Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("CORES_IDENTITIES_ID"))
                return TempDataTable.Rows[0].Field<long?>("CORES_IDENTITIES_ID");
            else
                return 0;
        }

        public DataTable CoreLimitReachedByIdentityId(IConnectToDB _Connect, string Identities_id)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = Identities_id }
            };

            SQlin.sqlIn = "select isnull((select property_Name 'UserAllowed' from CSA.IDENTITY_PROPERTIES where IDENTITIES_ID= @IDENTITIES_ID and property_Type='member_corelimit'),0)  UserAllowed,isnull((select COUNT(*) 'CoreUtilised' from CSA.VW__CORES_IDENTITIES where CREATOR='Y' and IDENTITIES_ID= @IDENTITIES_ID),0) CoreUtilised";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            return TempDataTable;
        }

        public DataTable GetAvailableCores(IConnectToDB _Connect, string Identities_id)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlinIdentityApps = new ER_Query.Parameter_Run();
            ER_Query.Parameter_Run SQlinAllIdentityCores = new ER_Query.Parameter_Run();
            ER_Query.Parameter_Run SQlinOwnerLimits = new ER_Query.Parameter_Run();
            ER_Query.Parameter_Run SQlinOwnerUtilised = new ER_Query.Parameter_Run();
            ER_Query.Parameter_Run SQlinAppsCreatedByIdentity = new ER_Query.Parameter_Run();
            string coreid = "";
            string ownerIdentityId = "";
            string ownerAppsAllowed = "";
            int ownerAppsUtilised = 0;
            int identityAppsUtilised = 0;
            int totalAppsUtilised = 0;

            SQlinIdentityApps._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = Identities_id }
            };

            SQlinAppsCreatedByIdentity._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = Identities_id }
            };

            SQlinIdentityApps.sqlIn = "select isnull((select property_Name 'UserAllowed' from " + _Connect.Schema + ".IDENTITY_PROPERTIES " +
            "where IDENTITIES_ID= @IDENTITIES_ID and property_Type='member_applimit'),0) UserAllowed," +
            "isnull((select COUNT(*) AppsUtilised from (Select a0.*,(select top(1) a.IDENTITIES_ID from " +
            "CSA.VW__ACTIVITY  a INNEr join CSA.VW__IDENTITIES i ON a.IDENTITIES_ID=i.IDENTITIES_ID " +
            "where a.APPLICATIONS_ID=a0.APPLICATIONS_ID and a.OBJECT_TYPE='add object' and Table_Source='APPLICATIONS')IDENTITIES_ID " +
            "from CSA.VW__APPLICATIONS a0 INNER JOIN CSA.VW__APPLICATIONS a1 ON a0.APPLICATIONS_ID = a1.APPLICATIONS_ID " +
            "and a1.RENDITION in (Select  MAX(Convert(int,RENDITION)) from CSA.VW__APPLICATIONS a1a where a1.ROOT_APPLICATION = a1a.ROOT_APPLICATION and a1.CORES_ID=a1a.CORES_ID))b " +
            "where APPLICATION_NAME not in('Core Settings','Revamp System') and IDENTITIES_ID = @IDENTITIES_ID),0)AppsUtilised";

            //Get allowed number of apps, Gets number of utilized apps
            DataTable IdentityApps = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinIdentityApps);

            SQlinAllIdentityCores.sqlIn = "select * from CSA.VW__CORES_IDENTITIES a where a.IDENTITIES_ID= @IDENTITIES_ID and a.enabled='Y' and a.core_name in " +
                        "(select Core_name from CSA.VW__IDENTITIES_ROLES b inner join CSA.[VW__ROLES] c ON (b.ROLES_ID = c.ROLES_ID)" +
                        "inner join CSA.[VW__ROLES_PRIVILEGES] d on (d.ROLE_NAME = b.ROLE_NAME and d.CORES_ID=c.CORES_ID) " +
                        "where b.IDENTITIES_ID = @IDENTITIES_ID and UPPER(d.PRIVILEGE_NAME) = 'CREATE FORM' )";

            //Get all cores associated with identity that has create right
            DataTable AllIdentityCores = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinAllIdentityCores);

            DataTable NewDataTable;
            NewDataTable = AllIdentityCores.Clone();

            foreach (DataRow row in AllIdentityCores.Rows)
            {
                coreid = row["Cores_ID"].ToString();

                SQlinOwnerLimits.sqlIn = "Select isnull((Select property_Name 'OwnerAppsAllowed' FROM CSA.IDENTITY_PROPERTIES ip Inner join CSA.VW__CORES_IDENTITIES ci on ip.Identities_ID = ci.Identities_ID " +
                "where ci.cores_ID = " + coreid + " And Creator = 'Y' and property_Type='member_applimit'),0) OwnerAppsAllowed, isnull((Select Top 1 ip.IDENTITIES_ID 'OwnerIdentity' " +
                "FROM CSA.IDENTITY_PROPERTIES ip Inner join CSA.VW__CORES_IDENTITIES ci on ip.Identities_ID = ci.Identities_ID where ci.cores_ID = " + coreid + " And ci.Creator = 'Y'),0) OwnerIdentity";

                //Get max number of applications allowed by Owner
                DataTable OwnerLimits = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinOwnerLimits);

                ownerAppsAllowed = OwnerLimits.Rows[0]["OwnerAppsAllowed"].ToString();
                ownerIdentityId = OwnerLimits.Rows[0]["OwnerIdentity"].ToString();

                //Get all applications utilised by owner within the core
                SQlinOwnerUtilised.sqlIn = "Select isnull((select COUNT(*) OwnerAppsUtilised from (Select a0.*,(select top(1) a.IDENTITIES_ID from CSA.VW__ACTIVITY a INNEr join CSA.VW__IDENTITIES i ON a.IDENTITIES_ID=i.IDENTITIES_ID where a.APPLICATIONS_ID=a0.APPLICATIONS_ID " +
                "and a.OBJECT_TYPE='add object' and Table_Source='APPLICATIONS')IDENTITIES_ID from CSA.VW__APPLICATIONS a0 INNER JOIN CSA.VW__APPLICATIONS a1 ON a0.APPLICATIONS_ID = a1.APPLICATIONS_ID and a1.RENDITION in (Select MAX(Convert(int,RENDITION)) " +
                "FROM CSA.VW__APPLICATIONS a1a where a1.ROOT_APPLICATION = a1a.ROOT_APPLICATION and a1.CORES_ID=a1a.CORES_ID))b where APPLICATION_NAME not in('Core Settings','Revamp System') and IDENTITIES_ID = " + ownerIdentityId + " AND CORES_ID = " + coreid + "),0) OwnerAppsUtilised";

                DataTable OwnerUtilised = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinOwnerUtilised);

                //Get all applications utilised by Identity within the core
                SQlinAppsCreatedByIdentity.sqlIn = "Select isnull((select COUNT(*) AppsUtilised from (Select a0.*,(select top(1) a.IDENTITIES_ID from CSA.VW__ACTIVITY a INNEr join CSA.VW__IDENTITIES i ON a.IDENTITIES_ID=i.IDENTITIES_ID where a.APPLICATIONS_ID=a0.APPLICATIONS_ID " +
                "and a.OBJECT_TYPE='add object' and Table_Source='APPLICATIONS')IDENTITIES_ID from CSA.VW__APPLICATIONS a0 INNER JOIN CSA.VW__APPLICATIONS a1 ON a0.APPLICATIONS_ID = a1.APPLICATIONS_ID and a1.RENDITION in (Select MAX(Convert(int,RENDITION)) " +
                "FROM CSA.VW__APPLICATIONS a1a where a1.ROOT_APPLICATION = a1a.ROOT_APPLICATION and a1.CORES_ID=a1a.CORES_ID))b where APPLICATION_NAME not in('Core Settings','Revamp System') and IDENTITIES_ID = @IDENTITIES_ID AND CORES_ID = " + coreid + "),0) AppsUtilised";

                DataTable AppsCreatedByIdentity = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinOwnerUtilised);

                ownerAppsUtilised = Convert.ToInt32(OwnerUtilised.Rows[0]["OwnerAppsUtilised"]);
                identityAppsUtilised = Convert.ToInt32(AppsCreatedByIdentity.Rows[0]["AppsUtilised"]);

                if (Identities_id == ownerIdentityId)
                    totalAppsUtilised = ownerAppsUtilised;
                else
                    totalAppsUtilised = ownerAppsUtilised + identityAppsUtilised;

                if (coreid == "1000")
                    ownerAppsAllowed = IdentityApps.Rows[0]["UserAllowed"].ToString();

                if (ownerAppsAllowed.ToLower() == "unlimited" || (Convert.ToInt32(ownerAppsAllowed) > totalAppsUtilised))
                    //Get allowable cores for identity
                    NewDataTable.ImportRow(row);
            }

            return NewDataTable;
        }

        public string GetCoreCreator(IConnectToDB _Connect, string Cores_Id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = Convert.ToInt32(Cores_Id) });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CREATOR_", DBType = SqlDbType.VarChar, ParamValue = "Y" });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            if (TempDataTable.Rows.Count > 0)
                return TempDataTable.Rows[0]["User_Name"].ToString();
            else
                return "";
        }

        public string GetCoreName(IConnectToDB _Connect, long? id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = Convert.ToInt32(id) });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "CORE_NAME", length = -1, order = "cores_id asc", start = 0, verify = "T" },
                Filters);

            if (TempDataTable.Rows.Count > 0)
                return TempDataTable.Rows[0]["core_name"].ToString();
            else
                return "";
        }

        public string GetCoreName(IConnectToDB _Connect, Guid? uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.VarChar, ParamValue = uuid });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "CORE_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);


            DataColumnCollection _dccColumnID = TempDataTable.Columns;

            if (_dccColumnID.Contains("CORE_NAME") && TempDataTable.Rows.Count > 0)
                return TempDataTable.Rows[0]["CORE_NAME"].ToString();
            else
                return "";
        }

        public long? GetCoreID(IConnectToDB _Connect, Guid? uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable;
            long? id = 0;

            if (uuid != null)
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", ParamValue = uuid });

                TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "CORES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                DataColumnCollection _dccColumnID = TempDataTable.Columns;

                if (_dccColumnID.Contains("CORES_ID") && TempDataTable.Rows.Count > 0)
                {
                    id = TempDataTable.Rows[0].Field<long?>("CORES_ID");
                }
            }

            return id;
        }

        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, List<string> _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", ParamValue = String.Join(",", _value) });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        private ViewCoreModel CoreView(IConnectToDB _Connect, ViewCoreModel CoreView, DataRow _DR)
        {
            AppHelper AH = new AppHelper();
            IdentityHelper IH = new IdentityHelper();
            CoreView = new ViewCoreModel();

            CoreView.cores_uuid = _DR.Field<Guid?>("cores_uuid");
            CoreView.core_name = _DR.Field<string>("core_name");
            CoreView.cores_id = _DR.Field<long?>("cores_id");
            CoreView.dt_available = _DR.Field<DateTime?>("dt_available");
            CoreView.dt_created = _DR.Field<DateTime>("dt_created");
            CoreView.dt_end = _DR.Field<DateTime?>("dt_end");
            CoreView.object_layer = _DR.Field<string>("object_layer");
            CoreView.object_type = _DR.Field<string>("object_type");
            CoreView.applications = (AH.FindByColumnAndValue(_Connect, _DR.Field<long?>("cores_id").ToString(), "cores_id")).Rows.Count;
            //applications = (AH.FindApplicationsByCore(_Connect, _DR.Field<long?>("cores_id").ToString())).Rows.Count,
            //members = _DR.Field<long?>("members")
            CoreView.members = IH.GetCoreMembersCount(_Connect, _DR.Field<long?>("cores_id").ToString());



            return CoreView;
        }

        public CoreModels GetCore(CoreModels Core, DataRow _DR)
        {
            Core = new CoreModels
            {
                ViewCore = new ViewCoreModel
                {
                    core_name = _DR.Field<string>("core_name"),
                    enabled = _DR.Field<string>("enabled"),
                    cores_id = _DR.Field<long?>("cores_id"),
                    cores_uuid = _DR.Field<Guid>("cores_uuid"),
                    object_layer = _DR.Field<string>("object_layer"),
                    object_type = _DR.Field<string>("object_type"),
                    dt_available = _DR.Field<DateTime?>("dt_available"),
                    dt_created = _DR.Field<DateTime>("dt_created"),
                    dt_end = _DR.Field<DateTime?>("dt_end"),
                }
            };
            return Core;
        }

        public List<CoreModels> GetCores(List<CoreModels> Cores, DataTable _DT)
        {
            CoreModels _Core = new CoreModels();

            foreach (DataRow CoreRow in _DT.Rows)
            {
                Cores.Add(GetCore(new CoreModels(), CoreRow));
            }

            return Cores;
        }

        public List<CoreModels> Cores(IConnectToDB _Connect, List<CoreModels> CoreModels, DataTable _DT, string ModelType, Boolean _GetApp, Boolean _GetStages, Boolean _GetGrips, Boolean _GetSets, Boolean _PropertySets, Boolean _OptionProperties)
        {
            List<CoreModels> Cores = new List<CoreModels>();

            switch (ModelType.ToLower())
            {
                case "view":
                    foreach (DataRow CoreViewRow in _DT.Rows)
                    {
                        CoreModels Core = new CoreModels();
                        Core.Core = new CoreStruct();
                        Core.Core.CoreView = new ViewCoreModel();

                        if (_GetApp)
                        {
                            AppHelper AppHelper = new AppHelper();
                            Core.applications = AppHelper.Apps(_Connect, Core, AppHelper.FindbyColumnID(_Connect, "cores_id", CoreViewRow.Field<long?>("cores_id").ToString()), "View", _GetStages, _GetGrips, _GetSets, _PropertySets, _OptionProperties);
                        };
                        Core.Core.CoreView = CoreView(_Connect, new ViewCoreModel(), CoreViewRow);
                        Cores.Add(Core);
                    }
                    break;
                case "table":
                    break;
            }

            return Cores;
        }

        
        public string AddCore(IConnectToDB _Connect, SessionObjects SO, FormCollection collection, out long? coreid, out Guid? coreUUID)
        {                       
            string status = "";
            ER_DML er_dml = new ER_DML();
            SecurityHelper SH = new SecurityHelper();
            
            add addHelp = new add();

            Values.AddCore thisCore = new Values.AddCore();
            string corename = "", corepath = "";
            long? identities_id = SO.SessionIdentity.Identity.identities_id;
            long? _identities_id = SO.SessionIdentity.Identity.identities_id;
            string appid = "";
            coreid = -1;
            coreUUID = null;
            //IValueProvider valueProvider = collection.ToValueProvider();
            if (collection.Keys.Count > 0)
            {
                corename = Sanitizer.GetSafeHtmlFragment(collection["core-name"].ToString().Trim());
                corepath = Sanitizer.GetSafeHtmlFragment(collection["core-path"].ToString().Trim());

                //Create Core
                if (CoreHelper.CanUseCoreName(_Connect, corename))
                {
                    thisCore = addHelp.ADD_ENTRY_Cores(_Connect, new Values.AddCore { I_CORE_NAME = collection["core-name"].ToString().ToString(), I_IDENTITIES_ID = SO.SessionIdentity.Identity.identities_id });

                    if (thisCore.O_CORES_ID > 0)
                    {
                        coreid = thisCore.O_CORES_ID != null ? thisCore.O_CORES_ID : 0;
                        coreUUID = thisCore.O_CORES_UUID;
                        //status = status + corename + " collection created, <a href='/core/collections?id=" + thisCore.O_CORES_UUID.ToString() + "'>click here to view it</a>   ";


                        //Add loggedin user to core identity
                        if (SO.SessionIdentity.Identity.identities_id != null)
                            addHelp.ADD_ENTRY_Cores_Identity(_Connect, new Values.AddCoreIdentity
                            {
                                I_IDENTITIES_ID = SO.SessionIdentity.Identity.identities_id,
                                I_CORES_UUID = coreUUID,
                                I_CREATOR = "Y"
                            });

                        //Save Members
                        if (collection["core-members"].ToString() != "" && SO.SessionIdentity.Identity.identities_id != null)
                        {
                            string[] users = collection["core-members"].ToString().Split(',');
                            for (int i = 0; i < users.Length; i++)
                            {
                                long? userid = SH.GetIdentityID(_Connect, users[i]);
                                if (userid != null && userid > 0)
                                {
                                    if (SO.SessionIdentity.Identity.identities_id != userid)
                                        addHelp.ADD_ENTRY_Cores_Identity(_Connect, new Values.AddCoreIdentity { I_IDENTITIES_ID = userid, I_CORES_UUID = coreUUID, I_CREATOR = "N" });
                                    //status = status + users[i] + " user is added to " + corename + ", ";
                                }
                                else
                                {
                                    //status = status + users[i] + " user is not present in system. Please use 'Invite' option to add new a member, ";
                                }
                            }
                        }

                        //Save Privacy settings to a default app for each core
                        ER_DB er_DB = new ER_DB();
                        er_DB.ADD_MSSQL_SCHEMA(_Connect, thisCore.I_CORE_NAME.Replace(" ", "_"), "");

                        List<DynamicModels.RootReportFilter> rootAppFilters = new List<DynamicModels.RootReportFilter>();
                        rootAppFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CONTAINER_NAME", ParamValue = "Other" });
                        rootAppFilters.Add(new DynamicModels.RootReportFilter { FilterName = "GET_LATEST", ParamValue = "T" });

                        DataTable GetContainerData = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CONTAINERS_SEARCH",
                            new DataTableDotNetModelMetaData { columns = "", length = -1, order = "1 asc", start = 0, verify = "T" },
                            rootAppFilters);

                        DataColumnCollection containerDCC = GetContainerData.Columns;
                        Guid? _containersUUid = null;

                        if (containerDCC.Contains("CONTAINERS_UUID") && GetContainerData.Rows.Count > 0)
                        {
                            _containersUUid = GetContainerData.Rows[0].Field<Guid?>("CONTAINERS_UUID");
                        }
                        
                        Values.AddApplication thisApplication = new Values.AddApplication
                        {
                            I_CORES_ID = coreid,
                            I_CORES_UUID = coreUUID,
                            I_APPLICATION_NAME = "Core Settings",
                            I_RENDITION = 0,
                            I_ROOT_APPLICATION = corename,
                            I_APPLICATION_LINK = "",
                            I_APPLICATION_DESCRIPTION = "Application For " + corename + " Settings",
                            I_CONTAINERS_UUID = _containersUUid
                        };

                        thisApplication = addHelp.ADD_ENTRY_Application(_Connect, thisApplication);
                        appid = thisApplication.O_APPLICATIONS_ID.ToString();
                        
                        SH.LogActivity(_Connect, "Add Object", thisCore.O_CORES_ID.ToString(), appid.ToString(), "APPLICATIONS", appid.ToString(), identities_id, "1005", "1007", ("Collection Settings" + " updated by " + Current.Session.GetString("UserName").ToUpper()), "1000", "1000", "");

                        Values.AddStage thisStageCoreSettings = new Values.AddStage
                        {
                            I_STAGE_TYPE = "Settings",
                            I_STAGE_NAME = "Core Settings",
                            I_APPLICATIONS_ID = thisApplication.O_APPLICATIONS_ID,
                            I_APPLICATIONS_UUID = thisApplication.O_APPLICATIONS_UUID,
                            I_IDENTITIES_ID = _identities_id,
                            PrettyLink = "Core_Settings"
                        };

                        thisStageCoreSettings = addHelp.ADD_ENTRY_Stage(_Connect, thisStageCoreSettings);

                        var gripCoreSettings = new Values.AddGrip
                        {
                            //applications_id = thisStageCoreSettings.I_APPLICATIONS_ID,
                            I_STAGES_ID = thisStageCoreSettings.O_STAGES_ID,
                            I_STAGES_UUID = thisStageCoreSettings.O_STAGES_UUID,
                            I_STAGE_TYPE = "Settings",
                            I_STAGE_NAME = "Core Settings",
                            I_GRIP_TYPE = "Settings",
                            I_GRIP_NAME = "Core Settings",
                            I_IDENTITIES_ID = _identities_id
                        };

                        gripCoreSettings = addHelp.ADD_ENTRY_Grip(_Connect, gripCoreSettings);
                        //GripsID = gripCoreSettings.grips_id.ToString();

                        var objectSetCoreSettings = new Values.AddObjectSet
                        {
                            I_GRIPS_UUID = gripCoreSettings.O_GRIPS_UUID,
                            I_GRIPS_ID = gripCoreSettings.O_GRIPS_ID,
                            I_STAGE_TYPE = "Settings",
                            I_STAGE_NAME = "Core Settings",
                            I_GRIP_TYPE = "Settings",
                            I_GRIP_NAME = "Core Settings",
                            I_OBJECT_TYPE = "Settings",
                            I_IDENTITIES_ID = _identities_id
                        };

                        objectSetCoreSettings = addHelp.ADD_ENTRY_Object_Set(_Connect, objectSetCoreSettings);
                        //ObjectSetsID = objectSetCoreSettings.object_sets_id.ToString();

                        if (collection.Keys.Contains("core-privacy") && collection.Keys.Contains("core-privacy"))
                        {
                            #region Property Privacy
                            Values.AddObjectPropertySet thisPropSet = new Values.AddObjectPropertySet
                            {
                                I_OBJECT_SETS_UUID = objectSetCoreSettings.O_OBJECT_SETS_UUID,
                                I_OBJECT_SETS_ID = objectSetCoreSettings.O_OBJECT_SETS_ID,
                                I_OBJECT_PROP_TYPE = "Text",
                                I_VALUE_DATATYPE = "Characters",
                                I_PROPERTY_NAME = "Privacy",
                                I_HAS_PARENT = "false",
                                I_HAS_CHILD = "false",
                                I_PARENT_OBJ_PROP_SETS_ID = null,
                                I_PROPERTY_VALUE = collection["core-privacy"].ToString()
                            };

                            thisPropSet = addHelp.addObjectSetProperty(_Connect, thisPropSet);
                            #endregion
                        }

                        //this is the for the URL to the core
                        if (corepath != null)
                        {
                            #region Property Core Path
                            Values.AddObjectPropertySet thisPropSet = new Values.AddObjectPropertySet
                            {
                                I_OBJECT_SETS_UUID = objectSetCoreSettings.O_OBJECT_SETS_UUID,
                                I_OBJECT_SETS_ID = objectSetCoreSettings.O_OBJECT_SETS_ID,
                                I_OBJECT_PROP_TYPE = "Text",
                                I_VALUE_DATATYPE = "Characters",
                                I_PROPERTY_NAME = "Core_Path",
                                I_HAS_PARENT = "false",
                                I_HAS_CHILD = "false",
                                I_PARENT_OBJ_PROP_SETS_ID = null,
                                I_PROPERTY_VALUE = corepath
                            };

                            thisPropSet = addHelp.addObjectSetProperty(_Connect, thisPropSet);
                            #endregion
                        }

                        //Add new stage, grip for invitations to this core.

                        Values.AddStage thisStageInvitations = new Values.AddStage
                        {
                            I_STAGE_TYPE = "Invitations",
                            I_STAGE_NAME = "Core Invitations",
                            I_APPLICATIONS_ID = thisApplication.O_APPLICATIONS_ID,
                            I_APPLICATIONS_UUID = thisApplication.O_APPLICATIONS_UUID,
                            I_IDENTITIES_ID = _identities_id,
                            PrettyLink = "Core_Invitations"
                        };

                        //string _stagetype, string _stagename, string _application_id, string _containers_id, string _identities_id, string _pretty_link
                        thisStageInvitations = addHelp.ADD_ENTRY_Stage(_Connect, thisStageInvitations);

                        //GripsID = gripCoreInvitations.grips_id.ToString();

                        //Add default roles to core
                        AddCoreRoles(_Connect, SO, thisCore);

                        //Add OWNERS core roles to LoggedIn user.
                        DataTable dtRoles = FindCoreRoles(_Connect, thisCore.O_CORES_UUID);
                        foreach (DataRow row in dtRoles.Select("Role_Name='OWNERS'"))
                        {
                            addHelp.ADD_ENTRY_Identity_Role(_Connect, new Values.AddIdentityRole
                            {
                                I_IDENTITIES_ID = SO.SessionIdentity.Identity.identities_id,
                                I_ROLES_UUID = row.Field<Guid?>("ROLES_UUID")
                            });
                        }

                        status = "Core Created";
                        return status;
                    }
                    else
                    {
                        status = "Core Not Created";
                    }
                }
                else
                {
                    status = "Core Not Created";
                }
            }
            return status;
        }


        public List<CommandResult> AddCoreRoles(IConnectToDB _Connect, SessionObjects SO, Values.AddCore thisCore)
        {
            ER_DML er_dml = new ER_DML();
            SecurityHelper SH = new SecurityHelper();
            List<CommandResult> Logger = new List<CommandResult>();

            List<Values.AddRole> rolesList = new List<Values.AddRole>();

            rolesList.Add(new Values.AddRole { I_ROLE_NAME = "OWNERS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            //rolesList.Add(new Values.AddRole { I_ROLE_NAME = "CREATOR", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolesList.Add(new Values.AddRole { I_ROLE_NAME = "AUDITOR", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolesList.Add(new Values.AddRole { I_ROLE_NAME = "PUBLIC ACCESS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolesList.Add(new Values.AddRole { I_ROLE_NAME = "OBJECT OWNER", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });

            Logger.AddRange(add.AddListOfRoles(_Connect, rolesList));

            List<Values.AddRolePrivilege> rolePriviligeList = new List<Values.AddRolePrivilege>();
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE FOREIGN KEY", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE UNIQUE KEY", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE PRIMARY KEY", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE PRIVILEGE", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE GRIP", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE CORE", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE APPLICATION", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE STAGE", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE GRIP", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE OBJECT", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE IDENTITY", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE CONTAINER", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE GROUPS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE ROLE", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE LOG", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "CREATE FORM", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "EDIT APPLICATIONS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "ACCESS OBJECT", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "ACCESS VIEWS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "ACCESS TABLES", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "ACCESS LOGS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "ACCESS OBJECT", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "EDIT APPLICATIONS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME =  "OWNERS", P_PRIVILEGE_NAME = "EDIT OTHERS APPLICATIONS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "EDIT ROLES", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME =  "OWNERS", P_PRIVILEGE_NAME = "EDIT PRIVILEGES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "EDIT GROUPS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME =  "OWNERS", P_PRIVILEGE_NAME = "EDIT IDENTITIES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW APPLICATIONS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW ROLES", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW IDENTITIES", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW GROUPS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW GRIPS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW OBJECT SETS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW OBJECT PROP SETS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW OBJECTS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME =  "OWNERS", P_PRIVILEGE_NAME = "VIEW INSTALLER", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW CORES", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW STAGES", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME =  "OWNERS", P_PRIVILEGE_NAME = "VIEW DICTIONARY", P_CORES_ID = thisCore.O_CORES_ID});
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME =  "OWNERS", P_PRIVILEGE_NAME = "VIEW SECURITY", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW PRIVILEGES", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW APPLICATION DETAILS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW FLOWS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "VIEW PROFILES", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "DUPLICATE APPLICATIONS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "ADD FORM", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OWNERS", I_PRIVILEGE_NAME = "ADD PROFILE", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });

            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE CORE", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE APPLICATION", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE STAGE", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE GRIP", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE OBJECT", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE IDENTITY", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE CONTAINER", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "ACCESS OBJECT", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE ROLE", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE LOG", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE PRIVILEGE", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE FORM", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "VIEW OBJECTS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "VIEW STAGES", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "VIEW APPLICATIONS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "VIEW GROUPS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "VIEW GRIPS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "VIEW OBJECT SETS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "EDIT APPLICATIONS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "CREATE GROUPS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "VIEW APPLICATION DETAILS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "VIEW PRIVILEGES", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "VIEW PROFILES", I_CORES_ID = thisCore.O_CORES_ID });
            ////rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME =  "CREATOR", P_PRIVILEGE_NAME = "VIEW DICTIONARY", P_CORES_ID = thisCore.O_CORES_ID});
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "ADD FORM", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "CREATOR", I_PRIVILEGE_NAME = "ADD PROFILE", I_CORES_ID = thisCore.O_CORES_ID });


            /*rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE FOREIGN KEY", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE UNIQUE KEY", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE PRIMARY KEY", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE PRIVILEGE", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE GRIP", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE CORE", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE APPLICATION", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE STAGE", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE GRIP", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE OBJECT", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE IDENTITY", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE CONTAINER", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE GROUPS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE ROLE", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE LOG", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "CREATE FORM", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "EDIT APPLICATIONS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "ACCESS OBJECT", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "ACCESS VIEWS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "ACCESS TABLES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "ACCESS LOGS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "ACCESS OBJECT", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "EDIT APPLICATIONS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "EDIT OTHERS APPLICATIONS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "EDIT ROLES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "EDIT PRIVILEGES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "EDIT GROUPS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "EDIT IDENTITIES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW APPLICATIONS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW ROLES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW IDENTITIES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW GROUPS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW GRIPS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW OBJECT SETS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW OBJECT PROP SETS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW OBJECTS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW INSTALLER", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW CORES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW STAGES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW DICTIONARY", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW SECURITY", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW PRIVILEGES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW APPLICATION DETAILS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW FLOWS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "VIEW PROFILES", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "DUPLICATE APPLICATIONS", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "ADD FORM", P_CORES_ID = thisCore.O_CORES_ID});
            rolePriviligeList.Add(new Values.AddRolesPrivileges { P_ROLE_NAME = "SYSTEM ADMIN", P_PRIVILEGE_NAME = "ADD PROFILE", P_CORES_ID = thisCore.O_CORES_ID});*/

            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "AUDITOR", I_PRIVILEGE_NAME = "ACCESS VIEWS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "AUDITOR", I_PRIVILEGE_NAME = "ACCESS TABLES", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "AUDITOR", I_PRIVILEGE_NAME = "ACCESS LOGS", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "AUDITOR", I_PRIVILEGE_NAME = "ACCESS OBJECT", I_CORES_ID = thisCore.O_CORES_ID });
            //rolePriviligeList.Add(new Values.AddRolesPrivileges { I_ROLE_NAME = "AUDITOR", I_PRIVILEGE_NAME = "ADD PROFILE", I_CORES_ID = thisCore.O_CORES_ID });

            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OBJECT OWNER", I_PRIVILEGE_NAME = "CREATE FORM", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OBJECT OWNER", I_PRIVILEGE_NAME = "CREATE STAGE", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "OBJECT OWNER", I_PRIVILEGE_NAME = "ADD FORM", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });

            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "PUBLIC ACCESS", I_PRIVILEGE_NAME = "ALLOW CONNECTION", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });
            rolePriviligeList.Add(new Values.AddRolePrivilege { I_ROLE_NAME = "PUBLIC ACCESS", I_PRIVILEGE_NAME = "VIEW APPLICATIONS", I_CORES_ID = thisCore.O_CORES_ID, I_CORES_UUID = thisCore.O_CORES_UUID });

            Logger.AddRange(add.AddListOfRoleAndPrivs(_Connect, rolePriviligeList));

            return Logger;
        }
    }
}
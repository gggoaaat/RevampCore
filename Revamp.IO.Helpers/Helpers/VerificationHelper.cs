using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.Structs.Models.DataEntry;

namespace Revamp.IO.Helpers.Helpers
{
    public class VerificationHelper
    {
        public bool ValidVerification(IConnectToDB _Connect, string VerificationString, string validation_type)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = VerificationString });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "VALIDATION_TYPE_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = validation_type });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "WHERE", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "DT_CREATED > DATEADD(minute, -15, GETDATE())" });
            DataTable DT = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__VERIFY_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            string UUID = "";
            
            try
            {
                foreach (DataRow DR in DT.Rows)
                {
                    UUID = DR.Field<string>("UUID");

                    if (UUID == VerificationString)
                    {
                        return true;
                    }
                }                
            }
            catch
            {
                return false;
            }

            return false;
        }

        public string DisableVerificationsForID(IConnectToDB _Connect, long? identities_id,string Validation_Type)
        {
            ER_DML er_dml = new ER_DML();
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
       
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            try
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = identities_id });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "VALIDATION_TYPE_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Validation_Type });

                DataTable _ResultSet = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__VERIFY_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                foreach (DataRow _Row in _ResultSet.Rows)
                {
                    er_dml.TOGGLE_OBJECT(_Connect, "VERIFY", _Row.Field<long?>("VERIFY_ID"), "N");
                }

                return "All Validations Disabled";
            }
            catch
            {
                return "Error Disabling Verifications";
            }
        }
        public string MarkVerificationsForID(IConnectToDB _Connect, long identities_id, string Validation_Type)
        {
            //ER_DML er_dml = new ER_DML();
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            add addHelp = new add();
            long? verifyId = null;

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            try
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = identities_id });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "VALIDATION_TYPE_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Validation_Type });

                DataTable _ResultSet = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__VERIFY_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                foreach (DataRow _Row in _ResultSet.Rows)
                {
                    Values.UpdateVerify VerifyModel = null;
                    VerifyModel = addHelp.UPDATE_ENTRY_Verify(_Connect, new Values.UpdateVerify
                    {
                        I_VERIFY_ID = _Row.Field<long?>("VERIFY_ID"),
                        I_UUID = _Row.Field<string>("UUID"),
                        I_VERIFIED = "Y",
                        I_VALIDATION_TYPE = _Row.Field<string>("VALIDATION_TYPE")
                    });
                    verifyId = VerifyModel.O_VERIFY_ID;
                    //er_dml.OBJECT_DML(_Connect, "Update", "VERIFY", "VERIFIED", _Row.Field<Int32>("VERIFY_ID"), new Object_Value { _String = "Y" });
                }

                return "All Validations Disabled";
            }
            catch
            {
                return "Error Disabling Verifications";
            }
        }
        public DataTable getUserByUUID(string UUID, IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = UUID });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_VERIFY_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }
        public DataTable getUUIDByUsername(string Username, IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "USER_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Username });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_VERIFY_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }
    }
}
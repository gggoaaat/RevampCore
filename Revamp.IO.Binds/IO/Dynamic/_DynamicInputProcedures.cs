using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Enums;
using Revamp.IO.Structs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Revamp.IO.Structs.Models.SQLProcedureModels;

namespace Revamp.IO.DB.Binds.IO.Dynamic
{
    public class _DynamicInputProcedures : I_DynamicInputProcedures
    {
        #region UI Stuff
        public static List<DynamicModels.Input.Portlet_> CorePortlets(IConnectToDB _Connect, long AppID, SessionObjects _SessionModel, List<DynamicModels.Input.Portlet_> thesePortlets, long INITIAL_CSA_PRIVILEGE_ID)
        {

            for (int i = 0; i < thesePortlets.Count; i++)
            {
                thesePortlets[i] = _DynamicInputProcedures._InsertPortlet(_Connect, thesePortlets[i]);

                for (int j = 0; j < thesePortlets[i].Containers.Count; j++)
                {
                    thesePortlets[i].Containers[j].I_PORTLET_ID = thesePortlets[i].O_PORTLET_ID;
                    thesePortlets[i].Containers[j] = _DynamicInputProcedures._InsertPortletContainer(_Connect, thesePortlets[i].Containers[j]);
                }
            }

            return thesePortlets;
        }


        public DynamicModels.Input.Custom_Reports InsertCustomReport(IConnectToDB _Connect, DynamicModels.Input.Custom_Reports thisModel, AuditDB AuditCommand = null)
        {
       

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_CUSTOM_REPORTS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_CUSTOM_REPORT_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_CUSTOM_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_CUSTOM_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_ROOT_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_ROOT_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_REPORT_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 100, ParamValue = thisModel.I_REPORT_NAME });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_DESCRIPTION", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_DESCRIPTION });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_CUSTOM_REPORT_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_CUSTOM_REPORT_ID = Convert.ToInt64(O_CUSTOM_REPORT_ID);
            }
            catch (Exception e)
            {
                thisModel.O_CUSTOM_REPORT_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Portlet_ _InsertPortlet(IConnectToDB _Connect, DynamicModels.Input.Portlet_ thisModel)
        {
            _DynamicInputProcedures COREIP = new _DynamicInputProcedures();

            return COREIP.InsertPortlet(_Connect, thisModel);
        }

        public DynamicModels.Input.Portlet_ InsertPortlet(IConnectToDB _Connect, DynamicModels.Input.Portlet_ thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_PORTLETS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_PORTLET_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_PORTLET_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_PORTLET_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_APPLICATION_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_APPLICATION_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_TITLE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 100, ParamValue = thisModel.I_TITLE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_CSA_PRIVILEGE_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.INITIAL_CSA_PRIVILEGE_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_PORTLET_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_PORTLET_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_PORTLET_ID = Convert.ToInt64(O_PORTLET_ID);
            }
            catch (Exception e)
            {
                thisModel.O_PORTLET_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Portlet_Container _InsertPortletContainer(IConnectToDB _Connect, DynamicModels.Input.Portlet_Container thisModel)
        {
            _DynamicInputProcedures COREIP = new _DynamicInputProcedures();

            return COREIP.InsertPortletContainer(_Connect, thisModel);
        }

        public DynamicModels.Input.Portlet_Container InsertPortletContainer(IConnectToDB _Connect, DynamicModels.Input.Portlet_Container thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_PORTLET_CONTAINERS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_CONTAINER_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_CONTAINER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_CONTAINER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_CONTAINER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_CONTAINER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PORTLET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PORTLET_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_TITLE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 100, ParamValue = thisModel.I_TITLE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SIZE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 20, ParamValue = thisModel.I_SIZE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_CONTAINER_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_CONTAINER_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_CONTAINER_ID = Convert.ToInt64(O_CONTAINER_ID);
            }
            catch (Exception e)
            {
                thisModel.O_CONTAINER_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Root_Report _InsertRootReport(IConnectToDB _Connect, DynamicModels.Input.Root_Report thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertRootReport(_Connect, thisModel);
        }

        public DynamicModels.Input.Root_Report InsertRootReport(IConnectToDB _Connect, DynamicModels.Input.Root_Report thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_ROOT_REPORTS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_ROOT_REPORT_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_ROOT_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_ROOT_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_TEMPLATE_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_TEMPLATE_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_TEMPLATE_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_TEMPLATE_NAME });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_REPORT_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_REPORT_NAME });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PROCEDURE_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_PROCEDURE_NAME });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SOURCE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_SOURCE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_ROOT_REPORT_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_ROOT_REPORT_ID = Convert.ToInt64(O_ROOT_REPORT_ID);
            }
            catch (Exception e)
            {
                thisModel.O_ROOT_REPORT_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Root_Report_Filter _InsertRootReportFilter(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Filter thisModel, bool GetSQL)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertRootReportFilter(_Connect, thisModel, GetSQL);
        }

        public DynamicModels.Input.Root_Report_Filter InsertRootReportFilter(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Filter thisModel, bool GetSQL)
        {
            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_ROOT_REPORT_FILTERS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_ROOT_REPORT_FILTER_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_ROOT_REPORT_FILTER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_ROOT_REPORT_FILTER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_ROOT_REPORT_FILTER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_ROOT_REPORT_FILTER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_ROOT_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_REQUIRED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 1, ParamValue = thisModel.I_REQUIRED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_FILTER_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_FILTER_NAME });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PRETTY_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_PRETTY_NAME });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_FILTER_TYPE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_FILTER_TYPE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IN_PICK_LIST", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 1, ParamValue = thisModel.I_IN_PICK_LIST });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_DB_TYPE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_DB_TYPE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PARAM_SIZE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PARAM_SIZE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SEARCH_DB_TYPE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_SEARCH_DB_TYPE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SEARCH_DB_SIZE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_SEARCH_DB_SIZE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ParamValue", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.I_ParamValue });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_FILTER_SELECT", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_FILTER_SELECT });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ALTERNATE_SOURCE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_ALTERNATE_SOURCE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ALTERNATE_SCHEMA", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_ALTERNATE_SCHEMA });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ALTERNATE_TEMPLATE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 100, ParamValue = thisModel.I_ALTERNATE_TEMPLATE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ALTERNATE_REPORT_NAME", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 100, ParamValue = thisModel.I_ALTERNATE_REPORT_NAME });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ROOT_REPORT_FILTER_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            if (GetSQL)
            {
                thisModel.Generated_SQL = returnSet.Rows.Count > 0 ? returnSet.Rows[0]["PROCEDURE_SQL"].ToString() : "";
            }
            else
            {
                string O_ROOT_REPORT_FILTER_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
                string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
                string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

                try
                {
                    thisModel.O_ROOT_REPORT_FILTER_ID = Convert.ToInt64(O_ROOT_REPORT_FILTER_ID);
                }
                catch (Exception e)
                {
                    thisModel.O_ROOT_REPORT_FILTER_ID = -1;
                    string error = e.ToString();
                }

                try
                {
                    thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
                }
                catch (Exception e)
                {
                    thisModel.O_ERR_NUMB = -1;
                    string error = e.ToString();
                }

                thisModel.O_ERR_MESS = O_ERR_MESS; 
            }

            return thisModel;
        }

        public static DynamicModels.Input.Custom_Report_Filter _InsertCustomReportFilter(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Filter thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertCustomReportFilter(_Connect, thisModel);
        }

        public DynamicModels.Input.Custom_Report_Filter InsertCustomReportFilter(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Filter thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_CUSTOM_REPORT_FILTERS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_FILTER_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_FILTER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_FILTER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_FILTER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_FILTER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_CUSTOM_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_MANDATORY", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_MANDATORY });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_FILTER", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_FILTER });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_FILTER_ALIAS", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_FILTER_ALIAS });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_FILTER_ORDER", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_FILTER_ORDER });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_REQUIRED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 1, ParamValue = thisModel.I_REQUIRED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_FILTER_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_FILTER_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_FILTER_ID = Convert.ToInt64(O_FILTER_ID);
            }
            catch (Exception e)
            {
                thisModel.O_FILTER_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Custom_Report_Column _InsertCustomReportColumns(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Column thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertCustomReportColumns(_Connect, thisModel);
        }

        public DynamicModels.Input.Custom_Report_Column InsertCustomReportColumns(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Column thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_CUSTOM_REPORT_COLUMNS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_CUSTOM_REPORT_COLUMN_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_CUSTOM_REPORT_COLUMN_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_CUSTOM_REPORT_COLUMN_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_CUSTOM_REPORT_COLUMN_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_CUSTOM_REPORT_COLUMN_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_CUSTOM_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ORIGINAL_COLUMN", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.I_ORIGINAL_COLUMN });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_COLUMN_AREA", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.I_COLUMN_AREA == null ? thisModel.I_ORIGINAL_COLUMN : thisModel.I_COLUMN_AREA });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ALIAS_AREA", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.I_ALIAS_AREA == null ? thisModel.I_ORIGINAL_COLUMN : thisModel.I_ALIAS_AREA });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_CUSTOM_REPORT_COLUMN_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_CUSTOM_REPORT_COLUMN_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_CUSTOM_REPORT_COLUMN_ID = Convert.ToInt64(O_CUSTOM_REPORT_COLUMN_ID);
            }
            catch (Exception e)
            {
                thisModel.O_CUSTOM_REPORT_COLUMN_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Root_Report_Column _InsertRootReportColumns(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Column thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertRootReportColumns(_Connect, thisModel);
        }

        public DynamicModels.Input.Root_Report_Column InsertRootReportColumns(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Column thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_ROOT_REPORT_COLUMNS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_ROOT_REPORT_COLUMN_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_ROOT_REPORT_COLUMN_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_ROOT_REPORT_COLUMN_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_ROOT_REPORT_COLUMN_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_ROOT_REPORT_COLUMN_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_ROOT_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ORIGINAL_COLUMN", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.I_ORIGINAL_COLUMN });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_COLUMN_AREA", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.I_COLUMN_AREA });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ALIAS_AREA", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1, ParamValue = thisModel.I_ALIAS_AREA });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ROOT_REPORT_COLUMN_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_CUSTOM_REPORT_COLUMN_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_ROOT_REPORT_COLUMN_ID = Convert.ToInt64(O_CUSTOM_REPORT_COLUMN_ID);
            }
            catch (Exception e)
            {
                thisModel.O_ROOT_REPORT_COLUMN_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }


        public static DynamicModels.Input.Portlet_Container_Custom_Reports _InsertPortletContainerCustomReport(IConnectToDB _Connect, DynamicModels.Input.Portlet_Container_Custom_Reports thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertPortletContainerCustomReport(_Connect, thisModel);
        }

        public DynamicModels.Input.Portlet_Container_Custom_Reports InsertPortletContainerCustomReport(IConnectToDB _Connect, DynamicModels.Input.Portlet_Container_Custom_Reports thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_PORTLET_CONTAINERS_CUSTOM_REPORTS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_PORTLET_CONTAINER_CUSTOM_REPORT_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";
            
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_CONTAINER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_CONTAINER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_CUSTOM_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_TITLE", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 255, ParamValue = thisModel.I_TITLE });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_PORTLET_CONTAINER_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_PORTLET_CONTAINER_CUSTOM_REPORT_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_PORTLET_CONTAINER_CUSTOM_REPORT_ID = Convert.ToInt64(O_PORTLET_CONTAINER_CUSTOM_REPORT_ID);
            }
            catch (Exception e)
            {
                thisModel.O_PORTLET_CONTAINER_CUSTOM_REPORT_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Custom_Report_Order _InsertCustomReportOrder(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Order thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertCustomReportOrder(_Connect, thisModel);
        }

        public DynamicModels.Input.Custom_Report_Order InsertCustomReportOrder(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Order thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_CUSTOM_REPORT_ORDER";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_CUSTOM_REPORTS_ORDER_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_CUSTOM_REPORTS_ORDER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_CUSTOM_REPORTS_ORDER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_CUSTOM_REPORTS_ORDER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_CUSTOM_REPORTS_ORDER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_CUSTOM_REPORTS_ORDER_SET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_CUSTOM_REPORTS_ORDER_SET_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SORT_COLUMN", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_SORT_COLUMN });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SORT_DIRECTION", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_SORT_DIRECTION });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SORT_ORDER", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_SORT_ORDER });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_CUSTOM_REPORTS_ORDER_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_CUSTOM_REPORTS_ORDER_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_CUSTOM_REPORTS_ORDER_ID = Convert.ToInt64(O_CUSTOM_REPORTS_ORDER_ID);
            }
            catch (Exception e)
            {
                thisModel.O_CUSTOM_REPORTS_ORDER_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Root_Report_Order _InsertRootReportOrder(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Order thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertRootReportOrder(_Connect, thisModel);
        }

        public DynamicModels.Input.Root_Report_Order InsertRootReportOrder(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Order thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_ROOT_REPORT_ORDER";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_CUSTOM_REPORTS_ORDER_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_ROOT_REPORTS_ORDER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_ROOT_REPORTS_ORDER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_ROOT_REPORTS_ORDER_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_ROOT_REPORTS_ORDER_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ROOT_REPORTS_ORDER_SET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_ROOT_REPORTS_ORDER_SET_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SORT_COLUMN", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_SORT_COLUMN });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SORT_DIRECTION", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_SORT_DIRECTION });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_SORT_ORDER", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 250, ParamValue = thisModel.I_SORT_ORDER });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ROOT_REPORTS_ORDER_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_ROOT_REPORTS_ORDER_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_ROOT_REPORTS_ORDER_ID = Convert.ToInt64(O_ROOT_REPORTS_ORDER_ID);
            }
            catch (Exception e)
            {
                thisModel.O_ROOT_REPORTS_ORDER_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Root_Report_Order_Sets _InsertRootReportOrderSet(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Order_Sets thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertRootReportOrderSet(_Connect, thisModel);
        }

        public DynamicModels.Input.Root_Report_Order_Sets InsertRootReportOrderSet(IConnectToDB _Connect, DynamicModels.Input.Root_Report_Order_Sets thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_ROOT_REPORT_ORDER_SETS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_ROOT_REPORTS_ORDER_SET_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_ROOT_REPORTS_ORDER_SET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_ROOT_REPORTS_ORDER_SET_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_ROOT_REPORTS_ORDER_SET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_ROOT_REPORTS_ORDER_SET_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ROOT_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_ROOT_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ROOT_REPORTS_ORDER_SET_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_ROOT_REPORTS_ORDER_SET_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_ROOT_REPORTS_ORDER_SET_ID = Convert.ToInt64(O_ROOT_REPORTS_ORDER_SET_ID);
            }
            catch (Exception e)
            {
                thisModel.O_ROOT_REPORTS_ORDER_SET_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Custom_Report_Order_Sets _InsertCustomReportOrderSet(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Order_Sets thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.InsertCustomReportOrderSet(_Connect, thisModel);
        }


        public DynamicModels.Input.Custom_Report_Order_Sets InsertCustomReportOrderSet(IConnectToDB _Connect, DynamicModels.Input.Custom_Report_Order_Sets thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_CUSTOM_REPORT_ORDER_SETS";
            string ProcedureReturnType = "object";
            string ReturnValueFor = "@O_CUSTOM_REPORTS_ORDER_SET_ID";
            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_BASE_CUSTOM_REPORTS_ORDER_SET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_BASE_CUSTOM_REPORTS_ORDER_SET_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_PREV_CUSTOM_REPORTS_ORDER_SET_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_PREV_CUSTOM_REPORTS_ORDER_SET_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_ENABLED", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Char, ParamSize = 1, ParamValue = thisModel.I_ENABLED });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_CUSTOM_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_CUSTOM_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = -1 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_CUSTOM_REPORTS_ORDER_SET_ID", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_CUSTOM_REPORTS_ORDER_SET_ID = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueFor, returnSet);
            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            try
            {
                thisModel.O_CUSTOM_REPORTS_ORDER_SET_ID = Convert.ToInt64(O_CUSTOM_REPORTS_ORDER_SET_ID);
            }
            catch (Exception e)
            {
                thisModel.O_CUSTOM_REPORTS_ORDER_SET_ID = -1;
                string error = e.ToString();
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        #endregion


        public static bool _SetPortletPrivileges(IConnectToDB _Connect, DynamicModels.Output.PortletPrivileges thisModel)
        {
            _DynamicInputProcedures CIP = new _DynamicInputProcedures();

            return CIP.SetPortletPrivileges(_Connect, thisModel);
        }
        public bool SetPortletPrivileges(IConnectToDB _Connect, DynamicModels.Output.PortletPrivileges thisModel, RevampCoreSettings appSettings = null)
        {
            try
            { 

                List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

                string ProcedureName = "SP_I_PORTLET_PRIVILEGES_BULK";
                string ProcedureReturnType = "object";

                EntryProcedureParameters.Add(new DBParameters { ParamName = "I_base_portlet_id", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Int, ParamValue = thisModel.base_portlet_id });
                EntryProcedureParameters.Add(new DBParameters { ParamName = "I_portlet_privilege_type_id", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.Int, ParamValue = thisModel.portlet_privilege_type_id });
                EntryProcedureParameters.Add(new DBParameters { ParamName = "I_privilege_id_list", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 2000, ParamValue = string.Join(",", thisModel.privilege_ids) });

               // DataTable ReturnSet = _dml.CALL_DB_PROCEDURE(_Connect, ProcedureName, ProcedureReturnType, EntryProcedureParameters);

                BIG_CALL RUN = new BIG_CALL();

                RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

                RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

                DataTable ReturnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

                return true;
            }
            catch (Exception ex)
            {
                ER_Tools._WriteEventLog(string.Format("Caught exception: {0} \r\n Stack Trace: {1}", ex.Message, ex.StackTrace), EventLogType.exception, appSettings);
                return false;
            }
        }

        /// <summary>
        /// make the call to delete container report from the db
        /// </summary>
        /// <param name="_Connect"></param>
        /// <param name="thisModel">model containing id of the record to be deleted</param>
        /// <returns></returns>
        public DynamicModels.Input.Delete_Portlet_Container_Custom_Report DeleteContainerReport(IConnectToDB _Connect, DynamicModels.Input.Delete_Portlet_Container_Custom_Report thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_D_CONTAINER_REPORT";
            string ProcedureReturnType = "object";

            string ReturnValueForErrMess = "@O_ERR_MESS";
            string ReturnValueForErrNumb = "@O_ERR_NUMB";

            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_LOGIN", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 30, ParamValue = thisModel.I_LOGIN });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_CONTAINER_REPORT_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_CONTAINER_REPORT_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_MESS", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.VarChar, ParamSize = 30 });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "O_ERR_NUMB", ParamDirection = ParameterDirection.Output, MSSqlParamDataType = SqlDbType.BigInt });

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable returnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            string O_ERR_MESS = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrMess, returnSet);
            string O_ERR_NUMB = ER_Procedure.SQL_PROCEDURE_GET_VALUE(ReturnValueForErrNumb, returnSet);

            //couldn't even make the call successfully
            if (returnSet.Rows.Count == 1 &&
                returnSet.Rows[0].ItemArray.Count() == 4
                && returnSet.Rows[0][1].ToString() == "Error")
            {
                thisModel.O_ERR_NUMB = 1;
                thisModel.O_ERR_MESS = "db call failed";
            }

            try
            {
                thisModel.O_ERR_NUMB = Convert.ToInt64(O_ERR_NUMB);
            }
            catch (Exception e)
            {
                thisModel.O_ERR_NUMB = -1;
                string error = e.ToString();
            }

            thisModel.O_ERR_MESS = O_ERR_MESS;

            return thisModel;
        }

        public static DynamicModels.Input.Load_App_Custom_Reports _LoadCustomReportsToAppContainer(IConnectToDB _Connect, DynamicModels.Input.Load_App_Custom_Reports thisModel)
        {
            _DynamicInputProcedures DIP = new _DynamicInputProcedures();

            return DIP.LoadCustomReportsToAppContainer(_Connect, thisModel);
        }
        public DynamicModels.Input.Load_App_Custom_Reports LoadCustomReportsToAppContainer(IConnectToDB _Connect, DynamicModels.Input.Load_App_Custom_Reports thisModel)
        {

            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            string ProcedureName = "SP_I_LOAD_CUSTOM_REPORTS_TO_PORTLETS";
            string ProcedureReturnType = "object";


            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_APP_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_APP_ID });
            EntryProcedureParameters.Add(new DBParameters { ParamName = "I_IDENTITIES_ID", ParamDirection = ParameterDirection.Input, MSSqlParamDataType = SqlDbType.BigInt, ParamValue = thisModel.I_IDENTITIES_ID });

           // DataTable ReturnSet = _dml.CALL_DB_PROCEDURE(_Connect, ProcedureName, ProcedureReturnType, EntryProcedureParameters);

            BIG_CALL RUN = new BIG_CALL();

            RUN.COMMANDS = new List<SQL_PROCEDURE_CALL>();

            RUN.COMMANDS.Add(new SQL_PROCEDURE_CALL { ProcedureType = ProcedureReturnType, ProcedureName = ProcedureName, _dbParameters = EntryProcedureParameters });

            DataTable ReturnSet = ER_Procedure.SQL_PROCEDURE_PARAMS(_Connect, RUN).COMMANDS.FirstOrDefault().result._DataTable;

            return thisModel;
        }
    }
}

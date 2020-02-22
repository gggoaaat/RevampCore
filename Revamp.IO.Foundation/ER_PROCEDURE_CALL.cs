using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs;

namespace Revamp.IO.Foundation
{
    public class ER_Procedure_Call
    {
        public DataTable GetIdentities(IConnectToDB _Connect, IdentitySearch Model)
        {
            ER_DML er_dml = new ER_DML();

            VirtualProcedureCall ProcedureModel = new VirtualProcedureCall();

            ProcedureModel.ProcedureName = "VW__IDENTITIES_SEARCH";
            ProcedureModel.ProcedureReturnType = "query";

            ProcedureModel.ProcedureParams = new List<ProcedureParameterStruct>();

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_SEARCH",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "MAX",
                ParamValue = Model.Search
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_WHERE",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "MAX",
                ParamValue = Model.Where
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_STARTING_ROW",
                MSSqlParamDataType = "bigint",
                ParamDirection = "input",
                ParamValue = Model.StartingRow
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_LENGTH_OF_SET",
                MSSqlParamDataType = "bigint",
                ParamDirection = "input",
                ParamValue = Model.LengthOfSet
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_ORDER_BY",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "MAX",
                ParamValue = Model.OrderBy
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_USER_NAME",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "50",
                ParamValue = Model.Username
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_EDIPI",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "10",
                ParamValue = Model.Edipi
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_EMAIL",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "254",
                ParamValue = Model.Email
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_ACTIVE",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "1",
                ParamValue = Model.Active
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_VERIFIED",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "1",
                ParamValue = Model.Verified
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_OBJECT_LAYER",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "30",
                ParamValue = Model.ObjectLayer
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_VERIFY",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "1",
                ParamValue = Model.Verify
            });

            DataTable _Result = ER_Procedure.VIRTUAL_PROCEDURE_CALL(_Connect, ProcedureModel);

            return _Result;
        }

        public DataTable GetIdentitiesCount(IConnectToDB _Connect, IdentitySearchCount Model)
        {
            ER_DML er_dml = new ER_DML();

            VirtualProcedureCall ProcedureModel = new VirtualProcedureCall();

            ProcedureModel.ProcedureName = "IDENTITIES_SEARCH_COUNT";
            ProcedureModel.ProcedureReturnType = "value";

            ProcedureModel.ProcedureParams = new List<ProcedureParameterStruct>();

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_SEARCH",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "MAX",
                ParamValue = Model.Search
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_WHERE",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "MAX",
                ParamValue = Model.Where
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_USER_NAME",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "50",
                ParamValue = Model.Username
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_EDIPI",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "10",
                ParamValue = Model.Edipi
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_EMAIL",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "254",
                ParamValue = Model.Email
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_ACTIVE",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "1",
                ParamValue = Model.Active
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_VERIFIED",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "1",
                ParamValue = Model.Verified
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_OBJECT_LAYER",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "30",
                ParamValue = Model.ObjectLayer
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "P_VERIFY",
                MSSqlParamDataType = "varchar",
                ParamDirection = "input",
                ParamSize = "1",
                ParamValue = Model.Verify
            });

            ProcedureModel.ProcedureParams.Add(new ProcedureParameterStruct
            {
                ParamName = "O_ATTEMPTED_RESULTS",
                MSSqlParamDataType = "varchar",
                ParamDirection = "output",
                ParamSize = "MAX"
            });

            DataTable _Result = ER_Procedure.VIRTUAL_PROCEDURE_CALL(_Connect, ProcedureModel);

            return _Result;
        }
    }
}

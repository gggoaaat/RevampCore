using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs;

namespace Revamp.IO.Foundation
{

    public class ER_CRYPT_PAIR
    {
        public byte[] _KEY { get; set; }
        public byte[] _IV { get; set; }
        public string _Message { get; set; }
    }

    public class ER_Sec
    {

        //START SECURITY TOOL
        //
        public Boolean DoesIdentityHaveAccess(IConnectToDB _Connect, string _ObjectLogicLayer, long? _ObjectLogicLayerId, long? _IdentityID, string _AccessType, DataTable GroupResult, DataTable Identity_Roles, DataTable Core_Identities)
        {
            List<string> Logger = new List<string>();

            DataTable _DT = new DataTable();

            StringBuilder _sqlin = new StringBuilder();

            List<string> Steps = new List<string>();

            Steps.Add("Check Roles");
            Steps.Add("Check Permissions");
            Steps.Add("Check Privileges");
            Steps.Add("Check Group Roles");

            bool HasRole = false;
            bool HasPriv = false;
            bool HasPerm = false;
            bool HasGroupRole = false;

            bool HasOwnerRole = false;
            bool HasSysAdminRole = false;


            DataColumnCollection DCC = Identity_Roles.Columns;

            foreach (DataRow _IDRole in Identity_Roles.Rows)
            {
                if (DCC.Contains("ROLE_NAME"))
                {
                    switch (_IDRole.Field<string>("ROLE_NAME").ToUpper())
                    {
                        case "OWNERS":
                            HasOwnerRole = true;
                            break;
                        case "SYSTEM ADMIN":
                            HasSysAdminRole = true;
                            break;
                    }
                }
            }


            DataColumnCollection dci = Core_Identities.Columns;

            foreach (DataRow _CoreID in Core_Identities.Rows)
            {
                if (DCC.Contains("CORE_NAME"))
                {
                    switch (_CoreID.Field<string>("CORE_NAME").ToUpper())
                    {
                        case "OWNERS":
                            HasOwnerRole = true;
                            break;
                        case "SYSTEM ADMIN":
                            HasSysAdminRole = true;
                            break;
                    }
                }
            }

            //Check if has sysadmin or owner via group.

            DataColumnCollection DCC2 = GroupResult.Columns;

            foreach (DataRow _IDGroupRole in GroupResult.Rows)
            {
                if (DCC.Contains("ROLE_NAME"))
                {
                    switch (_IDGroupRole.Field<string>("ROLE_NAME").ToUpper())
                    {
                        case "OWNERS":
                            HasOwnerRole = true;
                            break;
                        case "SYSTEM ADMIN":
                            HasSysAdminRole = true;
                            break;
                    }
                }
            }

            if (HasOwnerRole == false && HasSysAdminRole == false)
            {
                foreach (string _step in Steps)
                {
                    //_sqlin.Append("Select a.APPLICATIONS_ID, a.CONTAINERS_ID, a.IDENTITIES_ID, a.STAGES_ID, a.ROLES_ID FROM ");
                    _sqlin.Append("Select * FROM ");
                    List<DBParameters> Filters = new List<DBParameters>();
                    if (_step == "Check Roles")
                    {
                        switch (_ObjectLogicLayer.ToLower())
                        {
                            case "application":
                            case "applications":
                                _sqlin.Append("CSA.VW__APPLICATIONS_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.APPLICATIONS_ID = @APPLICATIONS_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "stage":
                            case "stages":
                                _sqlin.Append("CSA.VW__STAGES_SEC_ROLE  a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.STAGES_ID = @STAGES_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "grip":
                            case "grips":
                                _sqlin.Append("CSA.VW__GRIPS_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.GRIPS_ID = @GRIPS_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "GRIPS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "object_set":
                            case "object_sets":
                                _sqlin.Append("CSA.VW__OBJECT_SETS_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.OBJECT_SETS_ID = @OBJECT_SETS_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "OBJECT_SETS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "profile":
                            case "profiles":
                                _sqlin.Append("CSA.VW__PROFILES_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.PROFILES_ID = @PROFILES_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "PROFILES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "identitiy":
                            case "identities":
                                _sqlin.Append("CSA.VW__IDENTITIES_ROLES a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            default:
                                _sqlin.Append("CSA.VW__STAGES_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where  ");
                                break;
                        }
                        _sqlin.Append(" b.ROLE_NAME in (select c.ROLE_NAME from CSA.VW__ROLES_PRIVILEGES c where a.ROLE_NAME = c.ROLE_NAME and c.PRIVILEGE_NAME = @PRIVILEGE_NAME )");
                        Filters.Add(new DBParameters { ParamName = "PRIVILEGE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = _AccessType });

                        ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = _sqlin.ToString(),
                            _dbParameters = Filters
                        };

                        _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                        if (_DT.Rows.Count > 0)
                            HasRole = true;
                        else
                            HasRole = false;
                    }

                    if (_step == "Check Permissions")
                    {
                        switch (_ObjectLogicLayer.ToLower())
                        {
                            case "application":
                            case "applications":
                                _sqlin.Append("CSA.VW__APPLICATIONS_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.APPLICATIONS_ID = @APPLICATIONS_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "stage":
                            case "stages":
                                _sqlin.Append("CSA.VW__STAGES_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b  ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.STAGES_ID = @STAGES_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "grip":
                            case "grips":
                                _sqlin.Append("CSA.VW__GRIPS_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.GRIPS_ID = @GRIPS_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "GRIPS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "object_set":
                            case "object_sets":
                                _sqlin.Append("CSA.VW__OBJECT_SETS_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.OBJECT_SETS_ID = @OBJECT_SETS_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "OBJECT_SETS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "profile":
                            case "profiles":
                                _sqlin.Append("CSA.VW__PROFILES_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.PROFILES_ID = @PROFILES_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "PROFILES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "identity":
                            case "identities":
                                _sqlin.Append("CSA.VW__IDENTITIES_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.IDENTITIES_ID = @IDENTITIES_ID and b.IDENTITIES_ID = @IDENTITIES_ID2");
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID2", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            default:
                                _sqlin.Append("CSA.VW__STAGES_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b  ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.stages_id is not null ");
                                break;

                        }
                        _sqlin.Append(" and a.ROLE_NAME in (select c.ROLE_NAME from CSA.VW__ROLES_PRIVILEGES c where a.ROLE_NAME = c.ROLE_NAME and c.PRIVILEGE_NAME = @PRIVILEGE_NAME )");
                        Filters.Add(new DBParameters { ParamName = "PRIVILEGE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = _AccessType });

                        ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = _sqlin.ToString(),
                            _dbParameters = Filters
                        };

                        _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                        if (_DT.Rows.Count > 0)
                            HasPerm = true;
                        else
                            HasPerm = false;
                    }

                    if (_step == "Check Privileges")
                    {
                        switch (_ObjectLogicLayer.ToLower())
                        {
                            case "application":
                            case "applications":
                                _sqlin.Append("CSA.VW__APPLICATIONS_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.APPLICATIONS_ID = @APPLICATIONS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "stage":
                            case "stages":
                                _sqlin.Append("CSA.VW__STAGES_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.STAGES_ID = @STAGES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "grip":
                            case "grips":
                                _sqlin.Append("CSA.VW__GRIPS_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.GRIPS_ID = @GRIPS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "GRIPS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "object_set":
                            case "object_sets":
                                _sqlin.Append("CSA.VW__OBJECT_SETS_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.OBJECT_SETS_ID = @OBJECT_SETS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "OBJECT_SETS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "profile":
                            case "profiles":
                                _sqlin.Append("CSA.VW__PROFILES_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.PROFILES_ID = @PROFILES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "PROFILES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "identitiy":
                            case "identities":
                                _sqlin.Append("CSA.VW__IDENTITIES_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.IDENTITIES_ID  = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            default:
                                _sqlin.Append("CSA.VW__STAGES_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where ");
                                break;
                        }
                        _sqlin.Append("b.IDENTITIES_ID = @IDENTITIES_ID2 and a.PRIVILEGE_NAME = @PRIVILEGE_NAME ");
                        Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID2", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                        Filters.Add(new DBParameters { ParamName = "PRIVILEGE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = _AccessType });

                        ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = _sqlin.ToString(),
                            _dbParameters = Filters
                        };

                        _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                        if (_DT.Rows.Count > 0)
                            HasPriv = true;
                        else
                            HasPriv = false;
                    }

                    if (_step == "Check Group Roles")
                    {
                        switch (_ObjectLogicLayer.ToLower())
                        {
                            case "application":
                            case "applications":
                                _sqlin.Append("CSA.VW__APPLICATIONS_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.APPLICATIONS_ID = @APPLICATIONS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "stage":
                            case "stages":
                                _sqlin.Append("CSA.VW__STAGES_SEC_ROLE  a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.STAGES_ID = @STAGES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "grip":
                            case "grips":
                                _sqlin.Append("CSA.VW__GRIPS_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.GRIPS_ID = @GRIPS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "GRIPS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "object_set":
                            case "object_sets":
                                _sqlin.Append("CSA.VW__OBJECT_SETS_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.OBJECT_SETS_ID = @OBJECT_SETS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "OBJECT_SETS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "profile":
                            case "profiles":
                                _sqlin.Append("CSA.VW__PROFILES_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.PROFILES_ID = @PROFILES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "PROFILES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "identitiy":
                            case "identities":
                                _sqlin.Append("CSA.VW__IDENTITIES_ROLES a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            default:
                                _sqlin.Append("CSA.VW__STAGES_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where ");
                                break;
                        }
                        _sqlin.Append("b.GROUP_NAME in (select c.GROUP_NAME from CSA.VW__GROUP_MEMBERS c where c.GROUP_NAME = b.GROUP_NAME and c.IDENTITIES_ID = @IDENTITIES_ID2 ) and ");
                        _sqlin.Append("b.ROLE_NAME in (select d.ROLE_NAME from CSA.VW__ROLES_PRIVILEGES d where d.ROLE_NAME = b.ROLE_NAME and d.PRIVILEGE_NAME = @PRIVILEGE_NAME ) ");
                        Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID2", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                        Filters.Add(new DBParameters { ParamName = "PRIVILEGE_NAME", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _AccessType });

                        ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = _sqlin.ToString(),
                            _dbParameters = Filters
                        };

                        _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                        if (_DT.Rows.Count > 0)
                            HasGroupRole = true;
                        else
                            HasGroupRole = false;
                    }

                    _sqlin.Clear();
                }
            }

            if (HasPriv == false &&
                HasRole == false &&
                HasPerm == false &&
                HasGroupRole == false &&
                HasSysAdminRole == false &&
                HasOwnerRole == false)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public Boolean DoesIdentityHaveAccess(IConnectToDB _Connect, string _ObjectLogicLayer, long? _ObjectLogicLayerId, long? _IdentityID, string _AccessType)
        {
            List<string> Logger = new List<string>();

            DataTable _DT = new DataTable();

            StringBuilder _sqlin = new StringBuilder();

            List<string> Steps = new List<string>();

            Steps.Add("Check Roles");
            Steps.Add("Check Permissions");
            Steps.Add("Check Privileges");
            Steps.Add("Check Group Roles");

            bool HasRole = false;
            bool HasPriv = false;
            bool HasPerm = false;
            bool HasGroupRole = false;

            bool HasOwnerRole = false;
            bool HasSysAdminRole = false;

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = "Select * from CSA.VW__IDENTITIES_ROLES where IDENTITIES_ID = @Identities_id",
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID } }
            };

            DataTable Identity_Roles = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

            DataColumnCollection DCC = Identity_Roles.Columns;

            if (DCC.Contains("ROLE_NAME"))
            {
                foreach (DataRow _IDRole in Identity_Roles.Rows)
                {
                    switch (_IDRole.Field<string>("ROLE_NAME").ToUpper())
                    {
                        case "OWNERS":
                            HasOwnerRole = true;
                            break;
                        case "SYSTEM ADMIN":
                            HasSysAdminRole = true;
                            break;
                    }
                }
            }

            SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = "Select * from CSA.VW__CORES_IDENTITIES where IDENTITIES_ID = @IDENTITIES_ID",
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID } }
            };

            DataTable Core_Identities = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC2 = Core_Identities.Columns;

            foreach (DataRow _CoreID in Core_Identities.Rows)
            {
                if (DCC.Contains("CORE_NAME"))
                {
                    switch (_CoreID.Field<string>("CORE_NAME").ToUpper())
                    {
                        case "OWNERS":
                            HasOwnerRole = true;
                            break;
                        case "SYSTEM ADMIN":
                            HasSysAdminRole = true;
                            break;
                    }
                }
            }

            //Check if has sysadmin or owner via group.

            SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = "SELECT distinct aa.*, b.GROUP_NAME, c.IDENTITIES_ID, c.USER_NAME " +
                   "FROM  CSA.[VW__ROLES_PRIVILEGES] aa " +
                   "inner join CSA.[VW__ROLES] a ON (aa.ROLE_NAME = a.ROLE_NAME) " +
                   "inner join CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) " +
                   "inner join CSA.VW__GROUP_MEMBERS c ON (b.GROUPS_ID = c.GROUPS_ID) " +
                   "where c.IDENTITIES_ID = @IDENTITIES_ID",
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID } }
            };

            DataTable GroupResult = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

            DataColumnCollection DCC3 = GroupResult.Columns;

            foreach (DataRow _IDGroupRole in GroupResult.Rows)
            {
                if (DCC.Contains("ROLE_NAME"))
                {
                    switch (_IDGroupRole.Field<string>("ROLE_NAME").ToUpper())
                    {
                        case "OWNERS":
                            HasOwnerRole = true;
                            break;
                        case "SYSTEM ADMIN":
                            HasSysAdminRole = true;
                            break;
                    }
                }
            }

            if (HasOwnerRole == false && HasSysAdminRole == false)
            {
                List<DBParameters> Filters = new List<DBParameters>();

                foreach (string _step in Steps)
                {
                    //_sqlin.Append("Select a.APPLICATIONS_ID, a.CONTAINERS_ID, a.IDENTITIES_ID, a.STAGES_ID, a.ROLES_ID FROM ");
                    _sqlin.Append("Select * FROM ");

                    if (_step == "Check Roles")
                    {
                        switch (_ObjectLogicLayer.ToLower())
                        {
                            case "application":
                            case "applications":
                                _sqlin.Append("CSA.VW__APPLICATIONS_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.APPLICATIONS_ID = @APPLICATIONS_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "stage":
                            case "stages":
                                _sqlin.Append("CSA.VW__STAGES_SEC_ROLE  a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.STAGES_ID = @STAGES_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "grip":
                            case "grips":
                                _sqlin.Append("CSA.VW__GRIPS_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.GRIPS_ID = @GRIPS_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "object_set":
                            case "object_sets":
                                _sqlin.Append("CSA.VW__OBJECT_SETS_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.OBJECT_SETS_ID = @OBJECT_SETS_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "profile":
                            case "profiles":
                                _sqlin.Append("CSA.VW__PROFILES_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.PROFILES_ID = @PROFILES_ID and b.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "identitiy":
                            case "identity":
                            case "identities":
                                _sqlin.Append("CSA.VW__IDENTITIES_ROLES a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where a.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            default:
                                _sqlin.Append( "CSA.VW__STAGES_SEC_ROLE a INNER JOIN CSA.VW__IDENTITIES_ROLES b ON (a.ROLE_NAME = b.ROLE_NAME) where  ");
                                break;
                        }
                        _sqlin.Append(" b.ROLE_NAME in (select c.ROLE_NAME from CSA.VW__ROLES_PRIVILEGES c where a.ROLE_NAME = c.ROLE_NAME and c.PRIVILEGE_NAME = @PRIVILEGE_NAME )");
                        Filters.Add(new DBParameters { ParamName = "PRIVILEGE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = _AccessType });

                        SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = _sqlin.ToString(),
                            _dbParameters = Filters
                        };

                        _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                        if (_DT.Rows.Count > 0)
                            HasRole = true;
                        else
                            HasRole = false;
                    }

                    if (_step == "Check Permissions")
                    {
                        switch (_ObjectLogicLayer.ToLower())
                        {
                            case "application":
                            case "applications":
                                _sqlin.Append("CSA.VW__APPLICATIONS_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.APPLICATIONS_ID = @APPLICATIONS_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "stage":
                            case "stages":
                                _sqlin.Append("CSA.VW__STAGES_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b  ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.STAGES_ID = @STAGES_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "grip":
                            case "grips":
                                _sqlin.Append("CSA.VW__GRIPS_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.GRIPS_ID = @GRIPS_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "GRIPS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "object_set":
                            case "object_sets":
                                _sqlin.Append("CSA.VW__OBJECT_SETS_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.OBJECT_SETS_ID = @OBJECT_SETS_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "OBJECT_SETS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "profile":
                            case "profiles":
                                _sqlin.Append("CSA.VW__PROFILES_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.PROFILES_ID = @PROFILES_ID and b.IDENTITIES_ID = @IDENTITIES_ID");
                                Filters.Add(new DBParameters { ParamName = "PROFILES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            case "identitiy":
                            case "identity":
                            case "identities":
                                _sqlin.Append("CSA.VW__IDENTITIES_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.IDENTITIES_ID = @IDENTITIES_ID and b.IDENTITIES_ID = @IDENTITIES_ID2");
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID2", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                                break;
                            default:
                                _sqlin.Append( "CSA.VW__STAGES_SEC_PERM a INNER JOIN CSA.VW__IDENTITIES b  ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.STAGES_ID is not null ");
                                break;

                        }
                        _sqlin.Append(" and a.ROLE_NAME in (select c.ROLE_NAME from CSA.VW__ROLES_PRIVILEGES c where a.ROLE_NAME = c.ROLE_NAME and c.PRIVILEGE_NAME = @PRIVILEGE_NAME )");
                        Filters.Add(new DBParameters { ParamName = "PRIVILEGE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = _AccessType });

                        SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = _sqlin.ToString(),
                            _dbParameters = Filters
                        };

                        _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                        if (_DT.Rows.Count > 0)
                            HasPerm = true;
                        else
                            HasPerm = false;
                    }

                    if (_step == "Check Privileges")
                    {
                        switch (_ObjectLogicLayer.ToLower())
                        {
                            case "application":
                            case "applications":
                                _sqlin.Append("CSA.VW__APPLICATIONS_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.APPLICATIONS_ID = @APPLICATIONS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "stage":
                            case "stages":
                                _sqlin.Append("CSA.VW__STAGES_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.STAGES_ID = @STAGES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "grip":
                            case "grips":
                                _sqlin.Append("CSA.VW__GRIPS_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.GRIPS_ID = @GRIPS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "GRIPS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "object_set":
                            case "object_sets":
                                _sqlin.Append("CSA.VW__OBJECT_SETS_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.OBJECT_SETS_ID = @OBJECT_SETS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "OBJECT_SETS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "profile":
                            case "profiles":
                                _sqlin.Append("CSA.VW__PROFILES_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.PROFILES_ID = @PROFILES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "PROFILES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "identitiy":
                            case "identity":
                            case "identities":
                                _sqlin.Append("CSA.VW__IDENTITIES_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where a.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            default:
                                _sqlin.Append( "CSA.VW__STAGES_SEC_PRIV a INNER JOIN CSA.VW__IDENTITIES b ON (a.IDENTITIES_ID = b.IDENTITIES_ID) where ");
                                break;
                        }
                        _sqlin.Append("b.IDENTITIES_ID = @IDENTITIES_ID2 and a.PRIVILEGE_NAME = @PRIVILEGE_NAME ");
                        Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID2", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                        Filters.Add(new DBParameters { ParamName = "PRIVILEGE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = _AccessType });

                        SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = _sqlin.ToString(),
                            _dbParameters = Filters
                        };

                        _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                        if (_DT.Rows.Count > 0)
                            HasPriv = true;
                        else
                            HasPriv = false;
                    }

                    if (_step == "Check Group Roles")
                    {
                        switch (_ObjectLogicLayer.ToLower())
                        {
                            case "application":
                            case "applications":
                                _sqlin.Append("CSA.VW__APPLICATIONS_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.APPLICATIONS_ID = @APPLICATIONS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "APPLICATIONS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "stage":
                            case "stages":
                                _sqlin.Append("CSA.VW__STAGES_SEC_ROLE  a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.STAGES_ID = @STAGES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "STAGES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "grip":
                            case "grips":
                                _sqlin.Append("CSA.VW__GRIPS_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.GRIPS_ID = @GRIPS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "GRIPS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "object_set":
                            case "object_sets":
                                _sqlin.Append("CSA.VW__OBJECT_SETS_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.OBJECT_SETS_ID = @OBJECT_SETS_ID and ");
                                Filters.Add(new DBParameters { ParamName = "OBJECT_SETS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "profile":
                            case "profiles":
                                _sqlin.Append("CSA.VW__PROFILES_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.PROFILES_ID = @PROFILES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "PROFILES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            case "identitiy":
                            case "identities":
                                _sqlin.Append("CSA.VW__IDENTITIES_ROLES a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where a.IDENTITIES_ID = @IDENTITIES_ID and ");
                                Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _ObjectLogicLayerId });
                                break;
                            default:
                                _sqlin.Append( "CSA.VW__STAGES_SEC_ROLE a INNER JOIN CSA.VW__GROUPS_ROLES b ON (a.ROLES_ID = b.ROLES_ID) where ");
                                break;
                        }
                        _sqlin.Append("b.GROUP_NAME in (select c.GROUP_NAME from CSA.VW__GROUP_MEMBERS c where c.GROUP_NAME = b.GROUP_NAME and c.IDENTITIES_ID = @IDENTITIES_ID2 ) and ");
                        _sqlin.Append("b.ROLE_NAME in (select d.ROLE_NAME from CSA.VW__ROLES_PRIVILEGES d where d.ROLE_NAME = b.ROLE_NAME and d.PRIVILEGE_NAME = @PRIVILEGE_NAME ) ");
                        Filters.Add(new DBParameters { ParamName = "IDENTITIES_ID2", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID });
                        Filters.Add(new DBParameters { ParamName = "PRIVILEGE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = _AccessType });

                        SQlin = new ER_Query.Parameter_Run
                        {
                            sqlIn = _sqlin.ToString(),
                            _dbParameters = Filters
                        };

                        _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

                        if (_DT.Rows.Count > 0)
                            HasGroupRole = true;
                        else
                            HasGroupRole = false;
                    }

                    _sqlin.Clear();
                }
            }

            if (HasPriv == false &&
                HasRole == false &&
                HasPerm == false &&
                HasGroupRole == false &&
                HasSysAdminRole == false &&
                HasOwnerRole == false)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public List<CommandResult> PurgeIdentityFromSystem(IConnectToDB _Connect, Int32 identity_id, List<CommandResult> _result)
        {

            ER_Query er_query = new ER_Query();

            StringBuilder _sqlIn = new StringBuilder();

            _sqlIn.Append("");
            _sqlIn.AppendLine("DECLARE @IDENTITY_ID int = " + identity_id + " ");
            _sqlIn.AppendLine("DECLARE @OUTCOME varchar(1) = 'F' ");
            _sqlIn.AppendLine("");
            _sqlIn.AppendLine("BEGIN");

            _sqlIn.AppendLine("");
            _sqlIn.AppendLine("BEGIN TRY");
            _sqlIn.AppendLine("     delete CSA.[IDENTITIES_ROLES]");
            _sqlIn.AppendLine("     where IDENTITIES_ID = @IDENTITY_ID");
            _sqlIn.AppendLine("");
            _sqlIn.AppendLine("     delete CSA.IDENTITY_PROPERTIES");
            _sqlIn.AppendLine("     where IDENTITIES_ID = @IDENTITY_ID");
            _sqlIn.AppendLine("");
            _sqlIn.AppendLine("     delete CSA.ID_PASSWORD");
            _sqlIn.AppendLine("     where IDENTITIES_ID = @IDENTITY_ID");
            _sqlIn.AppendLine("     ");
            _sqlIn.AppendLine("     delete CSA.AES_PAIR ");
            _sqlIn.AppendLine("     where  CRYPT_TICKETS_ID in (select CRYPT_TICKETS_ID from NOMINATIONS.NOMINATIONS.CRYPT_TICKETS");
            _sqlIn.AppendLine("     where IDENTITIES_ID = @IDENTITY_ID)");
            _sqlIn.AppendLine("     ");
            _sqlIn.AppendLine("     delete CSA.CRYPT_TICKETS");
            _sqlIn.AppendLine("     where IDENTITIES_ID = @IDENTITY_ID");
            _sqlIn.AppendLine("");
            //_sqlIn.AppendLine("     delete NOMINATIONS.NOMINATIONS.IDENTITIES");
            //_sqlIn.AppendLine("     where IDENTITIES_ID = @IDENTITY_ID");
            _sqlIn.AppendLine("");
            _sqlIn.AppendLine("     delete CSA.PROFILES");
            _sqlIn.AppendLine("     where IDENTITIES_ID = @IDENTITY_ID");
            _sqlIn.AppendLine("");
            _sqlIn.AppendLine("     delete CSA.IDENTITY_PROPERTIES");
            _sqlIn.AppendLine("     where IDENTITIES_ID = @IDENTITY_ID");
            _sqlIn.AppendLine("");
            _sqlIn.AppendLine("     delete CSA.IDENTITIES");
            _sqlIn.AppendLine("     where IDENTITIES_ID = @IDENTITY_ID");
            _sqlIn.AppendLine("   SET @OUTCOME = 'T'");
            _sqlIn.AppendLine("");
            _sqlIn.AppendLine("END TRY");
            _sqlIn.AppendLine("BEGIN CATCH");
            _sqlIn.AppendLine("");
            _sqlIn.AppendLine("   SET @OUTCOME = 'F'");
            _sqlIn.AppendLine("END CATCH");
            _sqlIn.AppendLine("   SELECT @OUTCOME");
            _sqlIn.AppendLine("END");

            _result.Add(new CommandResult { _StartTime = DateTime.Now, _EndTime = DateTime.Now, _Response = er_query.RUN_NON_QUERY(_Connect, _sqlIn.ToString(), "Identity and all its related content deleted."), _Successful = true });

            return _result;
        }

        public ER_CRYPT_PAIR Generate_Crypt_Pair(ER_CRYPT_PAIR _Container)
        {
            List<byte[]> _returnDT = new List<byte[]>();
            try
            {

                //string original = _inputstring;//"Here is some data to encrypt!";
                //byte[] _key;
                //byte[] _iv;

                // Create a new instance of the Aes 
                // class.  This generates a new key and initialization  
                // vector (IV). 
                using (Aes myAes = Aes.Create())
                {

                    //myAes.Key.GetValue(

                    _Container._KEY = myAes.Key;
                    _Container._IV = myAes.IV;
                    _Container._Message = "Crypt Pair Generated";
                    // Encrypt the string to an array of bytes. 
                    //byte[] encrypted = EncryptStringToBytes_Aes(original, _key, _iv);

                    // Decrypt the bytes to a string. 
                    //string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

                    //Display the original data and the decrypted data.
                    //Console.WriteLine("Original:   {0}", original);
                    //_returnDT.Add(_key);
                    //_returnDT.Add(_iv);
                    //_returnDT.Add("Original Text: " + original);
                    //_returnDT.Add("Encrypted Text: " + System.Text.Encoding.UTF8.GetString(encrypted));
                    //_returnDT.Add("Round Trip Text: " + roundtrip);
                    //Console.WriteLine("Round Trip: {0}", roundtrip);
                }

            }
            catch (Exception e)
            {
                //_returnDT.Add("Error: {0}" + e.Message);
                return new ER_CRYPT_PAIR { _Message = e.Message };
            }

            return _Container;
        }

        public byte[] EncryptStringToBytes_Aes(string plainText, ER_CRYPT_PAIR Crypt_Container)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Crypt_Container._KEY == null || Crypt_Container._KEY.Length <= 0)
                throw new ArgumentNullException("Key");
            if (Crypt_Container._IV == null || Crypt_Container._IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Crypt_Container._KEY;
                aesAlg.IV = Crypt_Container._IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);

                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        public string DecryptStringFromBytes_Aes(byte[] cipherText, ER_CRYPT_PAIR Crypt_Container)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Crypt_Container._KEY == null || Crypt_Container._KEY.Length <= 0)
                throw new ArgumentNullException("Key");
            if (Crypt_Container._IV == null || Crypt_Container._IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Crypt_Container._KEY;
                aesAlg.IV = Crypt_Container._IV;
                //aesAlg.Padding = PaddingMode.None;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        public static byte[] ConvertStringToByte(string Input)
        {

            return System.Text.Encoding.UTF8.GetBytes(Input);

        }

        private static string ConvertByteArrayToString(Byte[] ByteOutput)
        {

            string StringOutput = System.Text.Encoding.UTF8.GetString(ByteOutput);

            return StringOutput;

        }

        public ER_CRYPT_PAIR GetCryptPairforID(IConnectToDB _Connect, long? _IdentityID, ER_CRYPT_PAIR AESPair)
        {
            ER_DML er_dml = new ER_DML();

            string sqlstatement = "Select top(1) a.IDENTITIES_ID, b.* from CSA.CRYPT_TICKETS a inner join CSA.AES_PAIR b on (a.IDENTITIES_ID = @IDENTITIES_ID and a.CRYPT_TICKETS_ID = b.CRYPT_TICKETS_ID) order by CRYPT_TICKETS_ID desc ";

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sqlstatement,
                _dbParameters = new List<DBParameters> { new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _IdentityID } }
            };

            DataTable _DT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
            DataColumnCollection DCC = _DT.Columns;

            if (DCC.Contains("IDENTITIES_ID"))
            {
                foreach (DataRow AESDR in _DT.Rows)
                {
                    AESPair._KEY = AESDR.Field<byte[]>("AES_KEY");
                    AESPair._IV = AESDR.Field<byte[]>("AES_IV");
                } 
            }

            return AESPair;
        }


        ///////////////////////////////////////////////////////////////////////////////
        // SAMPLE: Hashing data with salt using MD5 and several SHA algorithms.
        //
        // To run this sample, create a new Visual C# project using the Console
        // Application template and replace the contents of the Class1.cs file with
        // the code below.
        //
        // THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
        // EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
        // WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
        // 
        // Copyright (C) 2002 Obviex(TM). All rights reserved.

        /// <summary>
        /// This class generates and compares hashes using MD5, SHA1, SHA256, SHA384, 
        /// and SHA512 hashing algorithms. Before computing a hash, it appends a
        /// randomly generated salt to the plain text, and stores this salt appended
        /// to the result. To verify another plain text value against the given hash,
        /// this class will retrieve the salt value from the hash string and use it
        /// when computing a new hash of the plain text. Appending a salt value to
        /// the hash may not be the most efficient approach, so when using hashes in
        /// a real-life application, you may choose to store them separately. You may
        /// also opt to keep results as byte arrays instead of converting them into
        /// base64-encoded strings.
        /// </summary>

        /// <summary>
        /// Generates a hash for the given plain text value and returns a
        /// base64-encoded result. Before the hash is computed, a random salt
        /// is generated and appended to the plain text. This salt is stored at
        /// the end of the hash value, so it can be used later for hash
        /// verification.
        /// </summary>
        /// <param name="plainText">
        /// Plaintext value to be hashed. The function does not check whether
        /// this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1",
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="saltBytes">
        /// Salt bytes. This parameter can be null, in which case a random salt
        /// value will be generated.
        /// </param>
        /// <returns>
        /// Hash value formatted as a base64-encoded string.
        /// </returns>
        public static string ComputeHash(string plainText,
                                         string hashAlgorithm,
                                         byte[] saltBytes)
        {
            // If salt is not specified, generate it on the fly.
            if (saltBytes == null)
            {
                // Define min and max salt sizes.
                int minSaltSize = 4;
                int maxSaltSize = 8;

                // Generate a random number for the size of the salt.
                Random random = new Random();
                int saltSize = random.Next(minSaltSize, maxSaltSize);

                // Allocate a byte array, which will hold the salt.
                saltBytes = new byte[saltSize];

                // Initialize a random number generator.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

                // Fill the salt with cryptographically strong byte values.
                rng.GetNonZeroBytes(saltBytes);
            }

            // Convert plain text into a byte array.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Allocate array, which will hold plain text and salt.
            byte[] plainTextWithSaltBytes =
                    new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy plain text bytes into resulting array.
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextWithSaltBytes[i] = plainTextBytes[i];

            // Append salt bytes to the resulting array.
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            // Because we support multiple hashing algorithms, we must define
            // hash object as a common (abstract) base class. We will specify the
            // actual hashing algorithm class later during object creation.
            HashAlgorithm hash;

            // Make sure hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Initialize appropriate hashing algorithm class.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    //hash = new SHA1Managed();
                    hash = new SHA1CryptoServiceProvider();
                    break;

                case "SHA256":
                    //hash = new SHA256Managed();
                    hash = new SHA256CryptoServiceProvider();
                    break;

                case "SHA384":
                    //hash = new SHA384Managed();
                    hash = new SHA384CryptoServiceProvider();
                    break;

                case "SHA512":
                    //hash = new SHA512Managed();
                    hash = new SHA512CryptoServiceProvider();
                    break;

                default:
                    //hash = new MD5CryptoServiceProvider();
                    hash = new SHA512CryptoServiceProvider();
                    break;
            }

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            // Create array which will hold hash and original salt bytes.
            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                saltBytes.Length];

            // Copy hash bytes into resulting array.
            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            // Append salt bytes to the result.
            for (int i = 0; i < saltBytes.Length; i++)
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashWithSaltBytes);

            // Return the result.
            return hashValue;
        }

        /// <summary>
        /// Compares a hash of the specified plain text value to a given hash
        /// value. Plain text is hashed with the same salt value as the original
        /// hash.
        /// </summary>
        /// <param name="plainText">
        /// Plain text to be verified against the specified hash. The function
        /// does not check whether this parameter is null.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
        /// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
        /// MD5 hashing algorithm will be used). This value is case-insensitive.
        /// </param>
        /// <param name="hashValue">
        /// Base64-encoded hash value produced by ComputeHash function. This value
        /// includes the original salt appended to it.
        /// </param>
        /// <returns>
        /// If computed hash mathes the specified hash the function the return
        /// value is true; otherwise, the function returns false.
        /// </returns>
        public static bool VerifyHash(string plainText,
                                      string hashAlgorithm,
                                      string hashValue)
        {
            // Convert base64-encoded hash value into a byte array.
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // We must know size of hash (without salt).
            int hashSizeInBits, hashSizeInBytes;

            // Make sure that hashing algorithm name is specified.
            if (hashAlgorithm == null)
                hashAlgorithm = "";

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hashSizeInBits = 160;
                    break;

                case "SHA256":
                    hashSizeInBits = 256;
                    break;

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // Must be MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert size of hash from bits to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Make sure that the specified hash value is long enough.
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Allocate array to hold original salt bytes retrieved from hash.
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            // Copy salt from the end of the hash to the new array.
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            // Compute a new hash string.
            string expectedHashString =
                        ComputeHash(plainText, hashAlgorithm, saltBytes);

            // If the computed hash matches the specified hash,
            // the plain text value must be correct.
            return (hashValue == expectedHashString);
        }

        public class HashResult
        {
            public byte[] SaltArray { get; set; }
            public List<string> Output { get; set; }
        }

        /// <summary>
        /// Illustrates the use of the SimpleHash class.
        /// </summary>

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static HashResult TesttheHash()
        {
            HashResult _ResultReturn = new HashResult();

            string password = "myP@5sw0rd";  // original password
            string wrongPassword = "password";    // wrong password

            string passwordHashMD5 =
                   ComputeHash(password, "MD5", null);
            string passwordHashSha1 =
                   ComputeHash(password, "SHA1", null);
            string passwordHashSha256 =
                   ComputeHash(password, "SHA256", null);
            string passwordHashSha384 =
                   ComputeHash(password, "SHA384", null);
            string passwordHashSha512 =
                   ComputeHash(password, "SHA512", null);


            _ResultReturn.Output = new List<string>();
            _ResultReturn.Output.Add("COMPUTING HASH VALUES\r\n");
            _ResultReturn.Output.Add("MD5   : " + passwordHashMD5);
            _ResultReturn.Output.Add("SHA1  : " + passwordHashSha1);
            _ResultReturn.Output.Add("SHA256: " + passwordHashSha256);
            _ResultReturn.Output.Add("SHA384: " + passwordHashSha384);
            _ResultReturn.Output.Add("SHA512: " + passwordHashSha512);
            _ResultReturn.Output.Add("");

            _ResultReturn.Output.Add("COMPARING PASSWORD HASHES\r\n");
            _ResultReturn.Output.Add("MD5    (good): " +
                                VerifyHash(
                                password, "MD5",
                                passwordHashMD5).ToString());
            _ResultReturn.Output.Add("MD5    (bad) : " +
                                VerifyHash(
                                wrongPassword, "MD5",
                                passwordHashMD5).ToString());
            _ResultReturn.Output.Add("SHA1   (good): " +
                                VerifyHash(
                                password, "SHA1",
                                passwordHashSha1).ToString());
            _ResultReturn.Output.Add("SHA1   (bad) : " +
                                VerifyHash(
                                wrongPassword, "SHA1",
                                passwordHashSha1).ToString());
            _ResultReturn.Output.Add("SHA256 (good): " +
                                VerifyHash(
                                password, "SHA256",
                                passwordHashSha256).ToString());
            _ResultReturn.Output.Add("SHA256 (bad) : " +
                                VerifyHash(
                                wrongPassword, "SHA256",
                                passwordHashSha256).ToString());
            _ResultReturn.Output.Add("SHA384 (good): " +
                                VerifyHash(
                                password, "SHA384",
                                passwordHashSha384).ToString());
            _ResultReturn.Output.Add("SHA384 (bad) : " +
                                VerifyHash(
                                wrongPassword, "SHA384",
                                passwordHashSha384).ToString());
            _ResultReturn.Output.Add("SHA512 (good): " +
                                VerifyHash(
                                password, "SHA512",
                                passwordHashSha512).ToString());
            _ResultReturn.Output.Add("SHA512 (bad) : " +
                                VerifyHash(
                                wrongPassword, "SHA512",
                                passwordHashSha512).ToString());

            return _ResultReturn;
        }

        //
        // END OF FILE
        ///////////////////////////////////////////////////////////////////////////////

    }
}
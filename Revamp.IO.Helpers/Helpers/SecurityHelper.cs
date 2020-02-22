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
using Microsoft.AspNetCore.Mvc;

namespace Revamp.IO.Helpers.Helpers
{
    public class SecurityHelper
    {

        public List<SystemSessions> GetSessionsforDays(IConnectToDB _Connect, List<SystemSessions> _Sessions, long? DaysBack)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "WHERE", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "DT_CREATED > DATEADD(DAY,  -" + DaysBack + ", GETDATE())" });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_SESSIONS_SEARCH", new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            _Sessions = GetSessions(new List<SystemSessions>(), TempDataTable);

            return _Sessions;
        }

        public List<SystemSessions> GetSessions(List<SystemSessions> _Sessions, DataTable _DT)
        {
            foreach (DataRow SessionRow in _DT.Rows)
            {
                _Sessions.Add(GetSession(new SystemSessions(), SessionRow));
            }

            return _Sessions;
        }

        public SystemSessions GetSession(SystemSessions _Session, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            _Session = JsonConvert.DeserializeObject<SystemSessions>(thisAppRow);

            return _Session;
        }

        public RoleModels GetRole(IConnectToDB _Connect, RoleModels Role, DataRow _DR, bool _includePrivs)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            ViewRoleModel role = JsonConvert.DeserializeObject<ViewRoleModel>(thisAppRow);

            Role = new RoleModels
            {
                ViewRole = role
            };

            if (_includePrivs)
            {
                //Role.ViewRolePrivs = GetRolesPrivs(new List<ViewRolesPrivilegesModel>(), FindbyColumnID(_Connect, "role_name", Role.ViewRole.role_name.ToString(), "rolesprivs"));              
                Role.ViewRolePrivs = GetRolesPrivs(new List<ViewRolesPrivilegesModel>(), GetRoleprivilegeByCore(_Connect, Role.ViewRole.role_name.ToString(), _DR.Field<long?>("cores_id").ToString()));
            }

            //if (Role.ViewRole.role_name.ToUpper() == "OWNERS")
            //    Role.ViewRole.role_name = "COLLECTION ADMIN";

            //if (Role.ViewRole.role_name.ToUpper() == "OBJECT OWNER")
            //    Role.ViewRole.role_name = "DESIGNER";

            return Role;
        }

        public ViewIdentityRolesModel GetUserRole(IConnectToDB _Connect, ViewIdentityRolesModel UserRole, DataRow _DR, bool _includePrivs)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            UserRole = JsonConvert.DeserializeObject<ViewIdentityRolesModel>(thisAppRow);

            return UserRole;
        }

        public List<RoleModels> GetRoles(IConnectToDB _Connect, List<RoleModels> Roles, DataTable _DT, bool _includePrivs)
        {
            foreach (DataRow RoleRow in _DT.Rows)
            {
                //if (new[] { "AUDITOR", "CREATOR" }.Contains(RoleRow.Field<string>("role_name").ToUpper()))
                //    continue;

                Roles.Add(GetRole(_Connect, new RoleModels(), RoleRow, _includePrivs));
            }

            return Roles;
        }

        public List<ViewIdentityRolesModel> GetUserRoles(IConnectToDB _Connect, List<ViewIdentityRolesModel> UserRoles, DataTable _DT, bool _includePrivs)
        {
            foreach (DataRow RoleRow in _DT.Rows)
            {
                UserRoles.Add(GetUserRole(_Connect, new ViewIdentityRolesModel(), RoleRow, _includePrivs));
            }

            return UserRoles;
        }

        public GroupModels GetGroup(IConnectToDB _Connect, GroupModels Group, DataRow _DR, bool _includePrivs)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            ViewGroupModel thisGroup = JsonConvert.DeserializeObject<ViewGroupModel>(thisAppRow);

            Group = new GroupModels
            {
                ViewGroup = thisGroup

            };

            if (_includePrivs)
            {
                Group.ViewGroupRoles = GetGroupRoles(new List<ViewGroupsRolesModel>(), FindbyColumnID(_Connect, "group_name", Group.ViewGroup.group_name.ToString(), "groupsroles"));
            }

            return Group;
        }

        public List<GroupModels> GetGroups(IConnectToDB _Connect, List<GroupModels> Groups, DataTable _DT, bool _includePrivs)
        {
            GroupModels _Group = new GroupModels();

            foreach (DataRow GroupRow in _DT.Rows)
            {
                Groups.Add(GetGroup(_Connect, new GroupModels(), GroupRow, _includePrivs));
            }

            return Groups;
        }


        public ViewPrivilegesModel GetPriv(ViewPrivilegesModel Priv, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            Priv = JsonConvert.DeserializeObject<ViewPrivilegesModel>(thisAppRow);

            return Priv;
        }

        public List<ViewPrivilegesModel> GetPrivs(List<ViewPrivilegesModel> Privs, DataTable _DT)
        {
            ViewPrivilegesModel _Priv = new ViewPrivilegesModel();

            foreach (DataRow RoleRow in _DT.Rows)
            {
                Privs.Add(GetPriv(new ViewPrivilegesModel(), RoleRow));
            }

            return Privs;
        }

        public string[] GetPrivsNotAssigned(IConnectToDB _Connect, string role, long coresId)
        {
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "ROLE_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = role },
                new DBParameters { ParamName = "CORES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = coresId }
            };

            SQlin.sqlIn = "SELECT t1.PRIVILEGE_NAME FROM CSA.VW__PRIVILEGES t1 LEFT JOIN CSA.VW__ROLES_PRIVILEGES t2 ON t2.PRIVILEGE_NAME = t1.PRIVILEGE_NAME and t2.ROLE_NAME = @ROLE_NAME and t2.CORES_ID = @CORES_ID WHERE t2.PRIVILEGE_NAME IS NULL";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            string[] privileges = null;

            if (TempDataTable.Rows.Count >= 1)
            {
                privileges = new string[TempDataTable.Rows.Count];
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    privileges[i] = "";
                    privileges[i] = TempDataTable.Rows[i][0].ToString();
                }
            }

            return privileges;
        }


        public string[] GetGroupRolesNotAssigned(IConnectToDB _Connect, long? groupsid, long? coresid)
        {
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "GROUPS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = groupsid },
                new DBParameters { ParamName = "CORES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = coresid }
            };

            SQlin.sqlIn = "SELECT t1.ROLE_NAME FROM CSA.VW__ROLES t1 WHERE t1.cores_id = @CORES_ID and enabled = 'Y' and role_name not in (select role_name from CSA.VW__GROUPS_ROLES where GROUPS_ID = @GROUPS_ID AND ENABLED = 'Y') ";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            string[] roles = new string[] { };

            if (TempDataTable.Rows.Count >= 1)
            {
                roles = new string[TempDataTable.Rows.Count];
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    roles[i] = "";
                    roles[i] = TempDataTable.Rows[i][0].ToString();
                }

                roles = roles.Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
            }

            return roles;
        }

        public string[] GetGroupRolesAssigned(IConnectToDB _Connect, long groupsid, long coresid)
        {
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "GROUPS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = groupsid },
                new DBParameters { ParamName = "CORES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = coresid }
            };

            SQlin.sqlIn = "SELECT t1.ROLE_NAME FROM CSA.VW__ROLES t1 WHERE t1.cores_id = @CORES_ID and ENABLED = 'Y' and role_name in (select role_name from CSA.VW__GROUPS_ROLES where GROUPS_ID = @GROUPS_ID AND ENABLED = 'Y') ";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            string[] roles = new string[] { };

            if (TempDataTable.Rows.Count >= 1)
            {
                roles = new string[TempDataTable.Rows.Count];
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    roles[i] = "";
                    roles[i] = TempDataTable.Rows[i][0].ToString();
                }

                roles = roles.Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
            }

            return roles;
        }

        public string[] EnableGroupRoles(IConnectToDB _Connect, Guid? groups_uuid, string[] roles)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();
            List<string> enabledRoles = new List<string>();
            List<string> roleNames = new List<string>();
            DataTable TempDataTable;

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_UUID_", ParamValue = groups_uuid });

            TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_" + "VW__GROUPS_ROLES" + "_SEARCH",
                new DataTableDotNetModelMetaData { columns = "GROUPS_ROLES_UUID, ROLE_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection _dccGroupRolesColumnID = TempDataTable.Columns;

            if (_dccGroupRolesColumnID.Contains("GROUPS_ROLES_UUID") && TempDataTable.Rows.Count > 0)
            {
                foreach (DataRow row in TempDataTable.Rows)
                {
                    var groupRoles_uuid = row["GROUPS_ROLES_UUID"].ToString();
                    var roleName = row["ROLE_NAME"].ToString();
                    bool duplicate = Array.Exists(roleNames.ToArray(), element => element == roleName);

                    if (Array.Exists(roles, element => element == roleName) && !duplicate)
                    {
                        enabledRoles.Add(groupRoles_uuid);
                        roleNames.Add(roleName);
                    }
                }

                foreach (string groupRoleUUID in enabledRoles)
                {
                    SQlin._dbParameters = new List<DBParameters>
                    {
                        new DBParameters { ParamName = "GROUPS_ROLES_UUID", MSSqlParamDataType = SqlDbType.UniqueIdentifier, ParamValue = groupRoleUUID }
                    };

                    SQlin.sqlIn = "UPDATE CSA.GROUPS_ROLES SET ENABLED = 'Y' WHERE GROUPS_ROLES_UUID = @GROUPS_ROLES_UUID";

                    DataTable _GroupDT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
                }
            }

            return enabledRoles.ToArray();
        }

        public string DisableAllGroupRoles(IConnectToDB _Connect, Guid? groups_uuid)
        {
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "GROUPS_UUID", MSSqlParamDataType = SqlDbType.UniqueIdentifier, ParamValue = groups_uuid },
            };

            SQlin.sqlIn = "UPDATE CSA.GROUPS_ROLES SET ENABLED = 'N' WHERE GROUPS_UUID = @GROUPS_UUID";

            DataTable _GroupDT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

            return "";
        }

        public string[] EnableGroupIdentities(IConnectToDB _Connect, Guid? groups_uuid, string[] identities)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();
            List<string> enabledIdentities = new List<string>();
            List<string> userNames = new List<string>();
            DataTable TempDataTable;

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_UUID_", ParamValue = groups_uuid });

            TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_" + "VW__GROUP_MEMBERS" + "_SEARCH",
                new DataTableDotNetModelMetaData { columns = "GROUP_MEMBERS_UUID, USER_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection _dccGroupRolesColumnID = TempDataTable.Columns;

            if (_dccGroupRolesColumnID.Contains("GROUP_MEMBERS_UUID") && TempDataTable.Rows.Count > 0)
            {
                foreach (DataRow row in TempDataTable.Rows)
                {
                    var groupIdentities_uuid = row["GROUP_MEMBERS_UUID"].ToString();
                    var userName = row["USER_NAME"].ToString();
                    bool duplicate = Array.Exists(userNames.ToArray(), element => element == userName);

                    if (Array.Exists(identities, element => element == userName) && !duplicate)
                    {
                        enabledIdentities.Add(groupIdentities_uuid);
                        userNames.Add(userName);
                    }
                }

                foreach (string groupIdentitiesUUID in enabledIdentities)
                {
                    SQlin._dbParameters = new List<DBParameters>
                    {
                        new DBParameters { ParamName = "GROUP_MEMBERS_UUID", MSSqlParamDataType = SqlDbType.UniqueIdentifier, ParamValue = groupIdentitiesUUID }
                    };

                    SQlin.sqlIn = "UPDATE CSA.GROUP_MEMBERS SET ENABLED = 'Y' WHERE GROUP_MEMBERS_UUID = @GROUP_MEMBERS_UUID";

                    DataTable _GroupDT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);
                }
            }

            return enabledIdentities.ToArray();
        }

        public string DisableAllGroupIdentities(IConnectToDB _Connect, Guid? groups_uuid)
        {
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "GROUPS_UUID", MSSqlParamDataType = SqlDbType.UniqueIdentifier, ParamValue = groups_uuid },
            };

            SQlin.sqlIn = "UPDATE CSA.GROUP_MEMBERS SET ENABLED = 'N' WHERE GROUPS_UUID = @GROUPS_UUID";

            DataTable _GroupDT = ER_Query._RUN_PARAMETER_QUERY(_Connect, SQlin);

            return "";
        }

        public string[] GetGroupIdentitiesNotAssigned(IConnectToDB _Connect, string group)
        {
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "GROUP_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = group }
            };

            SQlin.sqlIn = "SELECT t1.USER_NAME FROM CSA.VW__IDENTITIES t1 LEFT JOIN CSA.VW__GROUP_MEMBERS t2 ON t2.IDENTITIES_ID = t1.IDENTITIES_ID and t2.GROUP_NAME = @GROUP_NAME and t2.enabled = 'Y' WHERE t2.USER_NAME IS NULL and t1.IDENTITIES_ID != '1000' and t1.ENABLED = 'Y'";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            string[] roles = new string[] { };

            if (TempDataTable.Rows.Count >= 1)
            {
                roles = new string[TempDataTable.Rows.Count];
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    roles[i] = "";
                    roles[i] = TempDataTable.Rows[i][0].ToString();
                }
            }

            return roles;
        }

        public string[] GetGroupIdentitiesAssigned(IConnectToDB _Connect, string groupName)
        {
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "GROUP_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = groupName }
            };

            SQlin.sqlIn = "SELECT t1.USER_NAME FROM CSA.VW__IDENTITIES t1 LEFT JOIN CSA.VW__GROUP_MEMBERS t2 ON t2.IDENTITIES_ID = t1.IDENTITIES_ID and t2.GROUP_NAME = @GROUP_NAME and t2.enabled = 'Y' WHERE t2.USER_NAME IS NOT NULL and t1.IDENTITIES_ID != '1000 and t1.ENABLED = 'Y'";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            string[] identities = new string[] { };

            if (TempDataTable.Rows.Count >= 1)
            {
                identities = new string[TempDataTable.Rows.Count];
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    identities[i] = "";
                    identities[i] = TempDataTable.Rows[i][0].ToString();
                }
            }

            return identities;
        }

        public string GetPrivID(IConnectToDB _Connect, string privilege)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();

            if (!string.IsNullOrWhiteSpace(privilege))
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = privilege });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__PRIVILEGES_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "PRIVILEGES_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            }

            string privilege_id = "";

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("PRIVILEGES_ID"))
            {
                privilege_id = TempDataTable.Rows[0].Field<long?>("PRIVILEGES_ID").ToString();
            }

            return privilege_id;
        }

        public long? GetRoleID(IConnectToDB _Connect, Guid? uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable;
            long? id = 0;

            if (uuid != null)
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLES_UUID_", ParamValue = uuid });

                TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ROLES_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "ROLES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                DataColumnCollection _dccColumnID = TempDataTable.Columns;

                if (_dccColumnID.Contains("ROLES_ID") && TempDataTable.Rows.Count > 0)
                {
                    id = TempDataTable.Rows[0].Field<long?>("ROLES_ID");
                }
            }

            return id;
        }

        //public string GetRoleID(IConnectToDB _Connect, string role, long? cores_id)
        //{

        //    //switch (role)
        //    //{
        //    //    case "COLLECTION ADMIN":
        //    //        role = "OWNERS";
        //    //        break;
        //    //    case "DESIGNER":
        //    //        role = "OBJECT OWNER";
        //    //        break;
        //    //}

        //    List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
        //    DataTable TempDataTable = new DataTable();

        //    if (!string.IsNullOrWhiteSpace(role) && cores_id != null)
        //    {
        //        Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = role });
        //        Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = cores_id });
        //        TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ROLES_SEARCH",
        //            new DataTableDotNetModelMetaData { columns = "ROLES_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

        //    }

        //    string role_id = "";

        //    DataColumnCollection DCC = TempDataTable.Columns;

        //    if (TempDataTable.Rows.Count > 0 && DCC.Contains("ROLES_ID"))
        //    {
        //        role_id = TempDataTable.Rows[0].Field<long?>("ROLES_ID").ToString();
        //    }

        //    return role_id;
        //}


        //public string GetRoleID(IConnectToDB _Connect, string role)
        //{
        //    //switch (role)
        //    //{
        //    //    case "COLLECTION ADMIN":
        //    //        role = "OWNERS";
        //    //        break;
        //    //    case "DESIGNER":
        //    //        role = "OBJECT OWNER";
        //    //        break;
        //    //}

        //    List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
        //    DataTable TempDataTable = new DataTable();

        //    if (!string.IsNullOrWhiteSpace(role))
        //    {
        //        Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = role });
        //        TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ROLES_SEARCH",
        //            new DataTableDotNetModelMetaData { columns = "ROLES_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
        //    }

        //    string role_id = "";

        //    DataColumnCollection DCC = TempDataTable.Columns;

        //    if (TempDataTable.Rows.Count > 0 && DCC.Contains("ROLES_ID"))
        //    {
        //        role_id = TempDataTable.Rows[0].Field<long?>("ROLES_ID").ToString();
        //    }

        //    return role_id;
        //}

        public string GetRoleName(IConnectToDB _Connect, Guid? roles_uuid)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();
            string roleName = "";

            if (roles_uuid != null)
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLES_UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = roles_uuid });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ROLES_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "ROLE_NAME", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
            }

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("ROLE_NAME"))
            {
                roleName = TempDataTable.Rows[0].Field<string>("ROLE_NAME");
            }

            return roleName;
        }

        public Guid? GetRoleUUID(IConnectToDB _Connect, string role, Guid? cores_uuid)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();
            Guid? role_id = null;

            if (!string.IsNullOrWhiteSpace(role))
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = role });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.UniqueIdentifier, ParamValue = cores_uuid });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ROLES_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "ROLES_UUID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
            }

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("ROLES_UUID"))
            {
                role_id = TempDataTable.Rows[0].Field<Guid?>("ROLES_UUID");
            }

            return role_id;
        }

        public Guid? GetIdentitiesUUID(IConnectToDB _Connect, string username)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();
            Guid? identities_uuid = null;

            if (!string.IsNullOrWhiteSpace(username))
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "USER_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = username });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "IDENTITIES_UUID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
            }

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("IDENTITIES_UUID"))
            {
                identities_uuid = TempDataTable.Rows[0].Field<Guid?>("IDENTITIES_UUID");
            }

            return identities_uuid;
        }

        public Guid? GetRoleCoreUUID(IConnectToDB _Connect, Guid? roles_uuid)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable = new DataTable();
            Guid? cores_uuid = null;

            if (roles_uuid != null)
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLES_UUID_", DBType = SqlDbType.UniqueIdentifier, ParamValue = roles_uuid });

                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ROLES_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "CORES_UUID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
            }

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("CORES_UUID"))
            {
                cores_uuid = TempDataTable.Rows[0].Field<Guid?>("CORES_UUID");
            }

            return cores_uuid;
        }

        public long? GetIdentityID(IConnectToDB _Connect, string identity)
        {
            IdentityHelper ih = new IdentityHelper();

            DataTable TempDataTable = new DataTable();

            TempDataTable = ih.FindIdentity(_Connect, identity);

            long? identity_id = null;

            if (TempDataTable.Rows.Count >= 1)
            {
                identity_id = ER_Tools.ConvertToInt64(TempDataTable.Rows[0]["IDENTITIES_ID"].ToString());
            }

            return identity_id;
        }


        public ViewRolesPrivilegesModel GetRolePriv(ViewRolesPrivilegesModel RolePriv, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            RolePriv = JsonConvert.DeserializeObject<ViewRolesPrivilegesModel>(thisAppRow);

            return RolePriv;
        }



        public List<ViewRolesPrivilegesModel> GetRolesPrivs(List<ViewRolesPrivilegesModel> RolePrivs, DataTable _DT)
        {
            ViewRolesPrivilegesModel _RolePriv = new ViewRolesPrivilegesModel();

            foreach (DataRow RolePrivRow in _DT.Rows)
            {
                RolePrivs.Add(GetRolePriv(new ViewRolesPrivilegesModel(), RolePrivRow));
            }

            return RolePrivs;
        }

        public ViewGroupsRolesModel GetGroupRole(ViewGroupsRolesModel GroupRole, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            GroupRole = JsonConvert.DeserializeObject<ViewGroupsRolesModel>(thisAppRow);

            return GroupRole;
        }

        public List<ViewGroupsRolesModel> GetGroupRoles(List<ViewGroupsRolesModel> GroupRoles, DataTable _DT)
        {
            ViewGroupsRolesModel _GroupRole = new ViewGroupsRolesModel();

            foreach (DataRow GroupRoleRow in _DT.Rows)
            {
                //if (new[] { "AUDITOR", "CREATOR" }.Contains(GroupRoleRow.Field<string>("role_name").ToUpper()))
                //    continue;

                GroupRoles.Add(GetGroupRole(new ViewGroupsRolesModel(), GroupRoleRow));
            }

            return GroupRoles;
        }

        public ViewGroupMembersModel GetGroupIdentity(ViewGroupMembersModel GroupIdentity, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            GroupIdentity = JsonConvert.DeserializeObject<ViewGroupMembersModel>(thisAppRow);

            return GroupIdentity;
        }

        public List<ViewGroupMembersModel> GetGroupIdentities(List<ViewGroupMembersModel> GroupIdentities, DataTable _DT)
        {
            ViewGroupMembersModel _GroupIdentity = new ViewGroupMembersModel();

            foreach (DataRow GroupIdentityRow in _DT.Rows)
            {
                GroupIdentities.Add(GetGroupIdentity(new ViewGroupMembersModel(), GroupIdentityRow));
            }

            return GroupIdentities;
        }

        public ViewAppAccessModel GetAppAccessRow(ViewAppAccessModel _AccessRow, DataRow _DR)
        {
            DataTable thisAppDT = new DataRow[] { _DR }.CopyToDataTable();
            string thisAppRow = new JObject(thisAppDT.Columns.Cast<DataColumn>()
                                     .Select(c => new JProperty(c.ColumnName, JToken.FromObject(thisAppDT.Rows[0][c])))
                               ).ToString(Formatting.None);

            _AccessRow = JsonConvert.DeserializeObject<ViewAppAccessModel>(thisAppRow);

            return new ViewAppAccessModel();
        }

        public List<ViewAppAccessModel> GetAppAccessList(List<ViewAppAccessModel> _AccessList, DataTable _DT)
        {
            foreach (DataRow AccessRow in _DT.Rows)
            {
                //if (new[] { "AUDITOR", "CREATOR" }.Contains(AccessRow.Field<string>("role_name").ToUpper()))
                //    continue;

                _AccessList.Add(GetAppAccessRow(new ViewAppAccessModel(), AccessRow));
            }

            return _AccessList;
        }

        public DataTable FindAll(IConnectToDB _Connect, string type)
        {
            string viewName = "";

            switch (type.ToLower())
            {
                case "roles":
                case "role":
                    viewName = "VW__ROLES";
                    break;
                case "identities":
                case "identity":
                    viewName = "VW__IDENTITIES";
                    break;
                case "privileges":
                case "privilege":
                    viewName = "VW__PRIVILEGES";
                    break;
                case "rolesprivs":
                    viewName = "VW__ROLES_PRIVILEGES";
                    break;
                case "rolesidentities":
                case "identityroles":
                case "identitiesroles":
                    viewName = "VW__IDENTITIES_ROLES";
                    break;
                case "groups":
                case "group":
                    viewName = "VW__GROUPS";
                    break;
                case "groupsroles":
                    viewName = "VW__GROUPS_ROLES";
                    break;
                case "groupidentities":
                case "groupmembers":
                    viewName = "VW__GROUP_MEMBERS";
                    break;
                case "approles":
                    viewName = "VW__APPLICATIONS_SEC_ROLE";
                    break;
                case "appaccess":
                    viewName = "VW__APPLICATIONS_SEC_PERM";
                    break;
                case "stageroles":
                    viewName = "VW__STAGES_SEC_ROLE";
                    break;
                case "stageaccess":
                    viewName = "VW__STAGES_SEC_PERM";
                    break;
                case "objectsetsroles":
                    viewName = "VW__OBJECT_SETS_SEC_ROLE";
                    break;
                case "coresidentity":
                    viewName = "VW__CORES_IDENTITIES";
                    break;
            }

            DataTable TempDataTable = new DataTable();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH", new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            return TempDataTable;
        }

        public DataTable Find(IConnectToDB _Connect, string _id, string type)
        {
            string viewName = "", columnName = "";

            switch (type.ToLower())
            {
                case "roles":
                case "role":
                    viewName = "VW__ROLES";
                    columnName = "ROLES_ID";
                    break;
                case "privileges":
                case "privilege":
                    viewName = "VW__PRIVILEGES";
                    columnName = "PRIVILEGES_ID";
                    break;
                case "rolesprivs":
                    viewName = "VW__ROLES_PRIVILEGES";
                    columnName = "ROLES_PRIVILEGES_ID";
                    break;
                case "rolesidentities":
                case "identityroles":
                case "identitiesroles":
                    viewName = "VW__IDENTITIES_ROLES";
                    columnName = "IDENTITIES_ROLES_ID";
                    break;
                case "coresidentities":
                case "identitycores":
                case "identitiescores":
                    viewName = "VW__CORES_IDENTITIES";
                    columnName = "CORES_IDENTITIES_ID";
                    break;
                case "groups":
                case "group":
                    viewName = "VW__GROUPS";
                    columnName = "GROUPS_ID";
                    break;
                case "groupsroles":
                    viewName = "VW__GROUPS_ROLES";
                    columnName = "GROUPS_ROLES_ID";
                    break;
                case "groupidentities":
                case "groupmembers":
                    viewName = "VW__GROUP_MEMBERS";
                    columnName = "GROUP_MEMBERS_ID";
                    break;
                case "approles":
                    viewName = "VW__APPLICATIONS_SEC_ROLE";
                    columnName = "APPLICATIONS_SEC_ROLE_ID";
                    break;
                case "appaccess":
                    viewName = "VW__APPLICATIONS_SEC_PERM";
                    columnName = "APPLICATIONS_SEC_PERM_ID";
                    break;
                case "stageroles":
                    viewName = "VW__STAGES_SEC_ROLE";
                    columnName = "STAGES_SEC_ROLE_ID";
                    break;
                case "stageaccess":
                    viewName = "VW__STAGES_SEC_PERM";
                    columnName = "STAGES_SEC_PERM_ID";
                    break;
                case "objectsetsroles":
                    viewName = "VW__OBJECT_SETS_SEC_ROLE";
                    columnName = "OBJECT_SETS_SEC_ROLE_ID";
                    break;
            }

            DataTable TempDataTable = new DataTable();
            if (!string.IsNullOrWhiteSpace(viewName))
            {
                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = columnName + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _id });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + viewName + "_SEARCH", new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
            }

            return TempDataTable;
        }
        public DataTable GetRoleprivilegeByCore(IConnectToDB _Connect, string Role_Name, string Cores_ID)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = Cores_ID });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Role_Name });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_ROLE_PRIVILEGE_CORE_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }
        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value, string type)
        {
            CoreHelper CH = new CoreHelper();
            DataTable TempDataTable = new DataTable();

            string Procedure = "";

            switch (type.ToLower())
            {
                case "roles":
                case "role":
                    Procedure = "SP_S_VW__ROLES_SEARCH";
                    break;
                case "privileges":
                case "privilege":
                    Procedure = "SP_S_VW__PRIVILEGES_SEARCH";
                    break;
                case "rolesprivs":
                    Procedure = "SP_S_VW__ROLES_PRIVILEGES_SEARCH";
                    break;
                case "rolesidentities":
                case "identityroles":
                case "identitiesroles":
                    Procedure = "SP_S_VW_VERIFIED_USERS_AND_ROLES_SEARCH";
                    break;
                case "availablecores":
                    //TODO: Revisit this Code Inside Method Below Looks complex.
                    TempDataTable = CH.GetAvailableCores(_Connect, _value);
                    break;
                case "coresidentities":
                case "identitycores":
                case "identitiescores":
                    Procedure = "SP_S_VW_CORE_MEMBERS_SEARCH";
                    break;
                case "coresidentitieswithcreateright":
                    Procedure = "SP_S_VW_USERS_WITH_CREATE_FORM_SEARCH";
                    break;
                case "coresidentitiescreated":
                    Procedure = "SP_S_VW_CORES_IDENTITIES_CREATORS_SEARCH";
                    //TODO: Review why there is an admin check
                    //if (_value != "1000")
                    //    TempDataTable = er_query.RUN_QUERY(_Connect, "Select * from " + _Connect.Schema + ".VW__CORES_IDENTITIES c inner join " + _Connect.Schema + ".VW__STAGES s on c.CORES_ID = s.CORES_ID where c." + _column + "  = '" + _value + "' and CREATOR='Y'  and stage_type='Invitations'");
                    //else
                    //    TempDataTable = er_query.RUN_QUERY(_Connect, "Select * from " + _Connect.Schema + ".VW__CORES_IDENTITIES c inner join " + _Connect.Schema + ".VW__STAGES s on c.CORES_ID = s.CORES_ID where stage_type='Invitations'");

                    break;
                case "corescreated":
                    Procedure = "SP_S_VW__CORES_IDENTITIES_SEARCH";
                    //TODO: Review why there is an admin check
                    //if (_value != "1000")
                    //    TempDataTable = er_query.RUN_QUERY(_Connect, "Select * from " + _Connect.Schema + ".VW__CORES_IDENTITIES where " + _column + "  = '" + _value + "' and CREATOR='Y'");
                    //else
                    //    TempDataTable = er_query.RUN_QUERY(_Connect, "Select * from " + _Connect.Schema + ".VW__CORES c inner join " + _Connect.Schema + ".VW__STAGES s on c.CORES_ID = s.CORES_ID where stage_type='Invitations'");
                    break;
                case "groups":
                case "group":
                    Procedure = "SP_S_VW__GROUPS_SEARCH";
                    break;
                case "groupsroles":
                    Procedure = "SP_S_VW__GROUPS_ROLES_SEARCH";
                    break;
                case "groupidentities":
                case "groupmembers":
                    Procedure = "SP_S_VW__GROUP_MEMBERS_SEARCH";
                    break;
                case "approles":
                    Procedure = "SP_S_VW__APPLICATIONS_SEC_ROLE_SEARCH";
                    break;
                case "appaccess":
                    Procedure = "SP_S_VW__APPLICATIONS_SEC_PERM_SEARCH";
                    break;
                case "stageroles":
                    Procedure = "SP_S_VW__STAGES_SEC_ROLE_SEARCH";
                    break;
                case "stageaccess":
                    Procedure = "SP_S_VW__STAGES_SEC_PERM_SEARCH";
                    break;
                case "objectsetsroles":
                    Procedure = "SP_S_VW__OBJECT_SETS_SEC_ROLE_SEARCH";
                    break;
            }


            if (!string.IsNullOrWhiteSpace(Procedure))
            {
                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", Procedure, new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
            }

            return TempDataTable;
        }
        public DataTable FindbyColumnsID(IConnectToDB _Connect, List<string> _column, List<string> _value, string type)
        {
            ER_Query er_query = new ER_Query();
            List<DBParameters> _dbParameters = new List<DBParameters>();
            string sql = "";
            string condition = "";
            int i = 0;
            foreach (var col in _column)
            {
                if (condition != null && condition != "")
                    condition = condition + " and ";
                condition = condition + col + "='" + _value[i] + "'";
                i += 1;
            }

            switch (type.ToLower())
            {
                case "identityrolesbycore":
                    _dbParameters.Add(new DBParameters { ParamName = "CONDITION", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = condition });
                    sql = "Select ir.enabled,ir.role_name,ir.user_name,ir.email,ir.edipi,ir.identities_roles_id,ir.roles_id,r.core_name,ir.object_type,ir.object_layer,ir.dt_available,ir.dt_created,ir.dt_end from " +
                        "CSA.VW__IDENTITIES_ROLES ir, " +
                        "CSA.VW__ROLES r where @CONDITION and ir.ROLES_ID = r.ROLES_ID";
                    break;
            }

            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run
            {
                sqlIn = sql,
                _dbParameters = _dbParameters
            };

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            return TempDataTable;
        }
        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, List<string> _value, string type)
        {
            string Procedure = "";

            switch (type.ToLower())
            {
                case "roles":
                case "role":
                    Procedure = "SP_S_VW__ROLES_SEARCH";
                    break;
                case "privileges":
                case "privilege":
                    Procedure = "SP_S_VW__PRIVILEGES_SEARCH";
                    break;
                case "rolesprivs":
                    Procedure = "SP_S_VW__ROLES_PRIVILEGES_SEARCH";
                    break;
                case "rolesidentities":
                case "identityroles":
                case "identitiesroles":
                    Procedure = "SP_S_VW_VERIFIED_USERS_AND_ROLES_SEARCH";
                    break;
                case "coreidentities":
                case "identitycores":
                case "identitiescores":
                    Procedure = "SP_S_VW_CORE_MEMBERS_SEARCH";
                    break;
                case "groups":
                case "group":
                    Procedure = "SP_S_VW__GROUPS_SEARCH";
                    break;
                case "groupsroles":
                    Procedure = "SP_S_VW__GROUPS_ROLES_SEARCH";
                    break;
                case "groupidentities":
                case "groupmembers":
                    Procedure = "SP_S_VW__GROUP_MEMBERS_SEARCH";
                    break;
                case "approles":
                    Procedure = "SP_S_VW__APPLICATIONS_SEC_ROLE_SEARCH";
                    break;
                case "appaccess":
                    Procedure = "SP_S_VW__APPLICATIONS_SEC_PERM_SEARCH";
                    break;
                case "stageroles":
                    Procedure = "SP_S_VW__STAGES_SEC_ROLE_SEARCH";
                    break;
                case "stageaccess":
                    Procedure = "SP_S_VW__STAGES_SEC_PERM_SEARCH";
                    break;
                case "objectsetsroles":
                    Procedure = "SP_S_VW__OBJECT_SETS_SEC_ROLE_SEARCH";
                    break;
            }
            DataTable TempDataTable = new DataTable();

            if (!string.IsNullOrWhiteSpace(Procedure))
            {
                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column, DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = String.Join(",", _value) });
                TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", Procedure, new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);
            }

            return TempDataTable;
        }

        public string[] GetAppRoles(IConnectToDB _Connect, string applications_id)
        {
            string[] app_roles = null;

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_UUID_", DBType = SqlDbType.VarChar, ParamValue = applications_id });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__APPLICATIONS_SEC_ROLE_SEARCH",
                new DataTableDotNetModelMetaData { columns = "ROLES_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("ROLES_ID"))
            {
                app_roles = new string[TempDataTable.Rows.Count];
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    app_roles[i] = "";
                    app_roles[i] = TempDataTable.Rows[i]["ROLES_ID"].ToString();
                }
            }

            return app_roles;

        }

        public long? GetGroupCoreID(IConnectToDB _Connect, string groups_id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = groups_id });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GROUPS_SEARCH",
                new DataTableDotNetModelMetaData { columns = "CORES_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("CORES_ID"))
                return TempDataTable.Rows[0].Field<long?>("CORES_ID");
            else
                return 0;
        }

        public Guid? GetGroupCoreUUID(IConnectToDB _Connect, Guid? groups_uuid)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = groups_uuid });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GROUPS_SEARCH",
                new DataTableDotNetModelMetaData { columns = "CORES_UUID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("CORES_UUID"))
                return TempDataTable.Rows[0].Field<Guid?>("CORES_UUID");
            else
                return null;
        }

        public string GetGroupName(IConnectToDB _Connect, Guid? groups_uuid)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = groups_uuid });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GROUPS_SEARCH",
                new DataTableDotNetModelMetaData { columns = "GROUP_NAME", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("GROUP_NAME"))
                return TempDataTable.Rows[0]["GROUP_NAME"].ToString();
            else
                return "";
        }

        public long? GetGroupId(IConnectToDB _Connect, Guid? groups_uuid)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = groups_uuid });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GROUPS_SEARCH",
                new DataTableDotNetModelMetaData {columns = "GROUPS_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("GROUPS_ID"))
                return TempDataTable.Rows[0].Field<long?>("GROUPS_ID");
            else
                return 0;
        }

        public string[] GetGroupRoles(IConnectToDB _Connect, long? groups_id)
        {
            string[] roles = null;

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_ID_", DBType = SqlDbType.BigInt, SearchParamSize = -1, ParamValue = groups_id });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GROUPS_SEARCH",
                new DataTableDotNetModelMetaData { columns = "ROLES_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("ROLES_ID"))
            {
                roles = new string[TempDataTable.Rows.Count];
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    roles[i] = "";
                    roles[i] = TempDataTable.Rows[i]["ROLES_ID"].ToString();
                }
            }

            return roles;
        }

        public string[] GetGroupIdentities(IConnectToDB _Connect, long? groups_id)
        {
            string[] identities = null;

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_ID_", DBType = SqlDbType.BigInt, SearchParamSize = -1, ParamValue = groups_id });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GROUP_MEMBERS_SEARCH",
                new DataTableDotNetModelMetaData { columns = "IDENTITIES_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("IDENTITIES_ID"))
            {
                identities = new string[TempDataTable.Rows.Count];
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    identities[i] = "";
                    identities[i] = TempDataTable.Rows[i]["IDENTITIES_ID"].ToString();
                }
            }

            return identities;

        }

        public List<ViewAppAccessModel> GetAccessbyIdentity(IConnectToDB _Connect, long? identity_id)
        {
            return GetAppAccessList(new List<ViewAppAccessModel>(), FindbyColumnID(_Connect, "identities_id", identity_id.ToString(), "appaccess"));
        }

        public List<ViewAppAccessModel> GetAccessbyAppid(IConnectToDB _Connect, long? applications_id)
        {
            return GetAppAccessList(new List<ViewAppAccessModel>(), FindbyColumnID(_Connect, "applications_id", applications_id.ToString(), "appaccess"));
        }

        public List<ViewAppAccessModel> GetAccessbyRolesid(IConnectToDB _Connect, long? roles_id)
        {
            return GetAppAccessList(new List<ViewAppAccessModel>(), FindbyColumnID(_Connect, "roles_id", roles_id.ToString(), "appaccess"));
        }

        public List<ViewAppAccessModel> GetAccessbyUserName(IConnectToDB _Connect, string user_name)
        {
            return GetAppAccessList(new List<ViewAppAccessModel>(), FindbyColumnID(_Connect, "user_name", user_name, "appaccess"));
        }

        public Values.AddRole AddRole(IConnectToDB _Connect, Guid? coresUUID, long? coresId, string roleName)
        {
            add addHelp = new add();

            //TODO: Verify this Logic. Something looks suspect about the logic. RISI 20180103
            return addHelp.ADD_ENTRY_Role(_Connect, new Values.AddRole
            {
                I_ROLE_NAME = roleName,
                I_CORES_ID = coresId,
                I_CORES_UUID = coresUUID
            });
        }

        public Values.AddRolePrivilege SavePrivToRole(IConnectToDB _Connect, string _RoleName, string _PrivilegeName)
        {
            add addHelp = new add();

            Values.AddRolePrivilege thisRolePriv = new Values.AddRolePrivilege();

            thisRolePriv = addHelp.ADD_ENTRY_Priv_to_Role(_Connect, new Values.AddRolePrivilege { I_ROLE_NAME = _RoleName, I_PRIVILEGE_NAME = _PrivilegeName });

            return thisRolePriv;
        }

        public Values.AddRolePrivilege SavePrivToCoreRole(IConnectToDB _Connect, string _RoleName, string _PrivilegeName, Guid? Cores_UUID, long? Cores_ID, long? identitiesId)
        {
            add er_dml = new add();

            return er_dml.ADD_ENTRY_Priv_to_Core_Role(_Connect, new Values.AddRolePrivilege
            {
                I_ROLE_NAME = _RoleName,
                I_PRIVILEGE_NAME = _PrivilegeName,
                I_CORES_UUID = Cores_UUID,
                I_CORES_ID = Cores_ID,
                I_IDENTITIES_ID = identitiesId
            });
        }

        public Values.AddPrivilege SavePrivilege(IConnectToDB _Connect, string _PrivilegeName)
        {
            add addHelp = new add();
            Values.AddPrivilege thisPriv = new Values.AddPrivilege();
            thisPriv = addHelp.ADD_ENTRY_Privilege(_Connect, new Values.AddPrivilege { I_PRIVILEGE_NAME = _PrivilegeName });

            return thisPriv;
        }

        public Values.AddIdentityRole AddIdentitytoRole(IConnectToDB _Connect, Guid? roles_id, long? Identities_id)
        {
            add addHelp = new add();

            return addHelp.ADD_ENTRY_Identity_Role(_Connect, new Values.AddIdentityRole { I_IDENTITIES_ID = Identities_id, I_ROLES_UUID = roles_id });
        }

        public Values.AddGroup AddGroup(IConnectToDB _Connect, string Group_Name, Guid? Cores_UUID, long? Cores_Id, long? Identities_Id)
        {
            add addHelp = new add();

            return addHelp.ADD_ENTRY_GROUP(_Connect, new Values.AddGroup
            {
                I_GROUP_NAME = Group_Name,
                I_CORES_UUID = Cores_UUID,
                I_CORES_ID = Cores_Id,
                I_IDENTITIES_ID = Identities_Id,
                I_ENABLED = 'Y'
            });
        }

        public Values.AddGroupMember AddIdentitytoGroup(IConnectToDB _Connect, long? Identities_Id, Guid? Groups_UUID, long? Groups_id)
        {
            add addHelp = new add();

            return addHelp.ADD_ENTRY_GROUP_MEMBER(_Connect, new Values.AddGroupMember
            {
                I_GROUPS_UUID = Groups_UUID,
                I_GROUPS_ID = Groups_id,
                I_IDENTITIES_ID = Identities_Id
            });
        }

        public Values.AddGroupRole AddRoletoGroup(IConnectToDB _Connect, Guid? Roles_id, Guid? Groups_id, Guid? Cores_id, long? Identities_Id, string Group_name)
        {
            add addHelp = new add();

            return addHelp.ADD_ENTRY_Group_Role(_Connect, new Values.AddGroupRole
            {
                I_GROUPS_UUID = Groups_id,
                I_ROLES_UUID = Roles_id,
                I_CORES_UUID = Cores_id,
                I_GROUP_NAME = Group_name,
                I_IDENTITIES_ID = Identities_Id
            });
        }

        public bool DoesIdentityHaveRole(IConnectToDB _Connect, string RoleName, long? Identities_id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = RoleName });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "Identities_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Identities_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.Char, SearchParamSize = -1, ParamValue = 'Y' });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_ROLES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "ROLE_NAME", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("ROLE_NAME"))
                return true;
            else
                return false;
        }

        public bool DoesIdentityHaveRoles(IConnectToDB _Connect, string[] RoleNames, long? Identities_id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = string.Join(",", RoleNames) });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "Identities_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Identities_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.Char, SearchParamSize = -1, ParamValue = 'Y' });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_ROLES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "ROLE_NAME", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("ROLE_NAME"))
                return true;
            else
                return false;
        }

        public bool DoesIdentityHaveRole(IConnectToDB _Connect, long? Roles_id, long? Identities_id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Roles_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "Identities_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Identities_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.Char, SearchParamSize = -1, ParamValue = 'Y' });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "S_S_VW__IDENTITIES_ROLES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "ROLES_ID", length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("ROLES_ID"))
                return true;
            else
                return false;
        }

        public bool DoesIdentityBelongToGroup(IConnectToDB _Connect, string GroupName, long? Identities_id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUP_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = GroupName });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "Identities_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Identities_id });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "S_S_VW__GROUP_MEMBERS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("GROUP_NAME"))
                return true;
            else
                return false;
        }

        public bool DoesGroupExist(IConnectToDB _Connect, string group, string cores)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            DataTable TempDataTable;
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUP_NAME_", DBType = SqlDbType.VarChar, ParamValue = group });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = cores });

            TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__GROUPS_SEARCH",
                            new DataTableDotNetModelMetaData { columns = "GROUP_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                            Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && DCC.Contains("GROUP_NAME"))
                return true;
            else
                return false;
        }

        public bool DoesRoleExist(IConnectToDB _Connect, string roleName, Guid? coresUUID)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", DBType = SqlDbType.VarChar, ParamValue = coresUUID });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = roleName });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_ROLES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "ROLE_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection _dccColumnID = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && _dccColumnID.Contains("ROLE_NAME"))
                return true;
            else
                return false;
        }

        public bool DoesIdentityBelongToGroup(IConnectToDB _Connect, long? Groups_id, long? Identities_id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Groups_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "Identities_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = Identities_id });
            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "S_S_VW__GROUP_MEMBERS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            if (TempDataTable.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public bool DoesIdentityHavePrivilege(IConnectToDB _Connect, string _layer, long? _layerid, long? identities_id, string privilege, DataTable group_result, DataTable identity_roles, DataTable core_identities)
        {
            ER_Sec er_sec = new ER_Sec();

            return er_sec.DoesIdentityHaveAccess(_Connect, _layer, _layerid, identities_id, privilege, group_result, identity_roles, core_identities);

        }

        //Todo: Point this to Memory and then convert calls to PROCS.
        public bool DoesIdentityHavePrivilege(IConnectToDB _Connect, string _IdentityID, string privilege)
        {
            if (_IdentityID == "1000")
                return true;

            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _IdentityID });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = privilege.ToUpper() });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });

            DataTable ResultDT = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_IDENITITY_PRIVILEGES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection DCC = ResultDT.Columns;

            if (ResultDT.Rows.Count > 0 & DCC.Contains("ROLES_PRIVILEGES_ID"))
            {
                return true;
            }
            else
            {
                List<DynamicModels.RootReportFilter> groupFilters = new List<DynamicModels.RootReportFilter>();

                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _IdentityID });
                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = privilege.ToUpper() });
                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });

                //Check if Identity has privilege via group.
                DataTable GroupResult = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_IDENITITY_PRIVILEGES_GROUPS_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    groupFilters);

                DataColumnCollection DCC2 = GroupResult.Columns;

                if (GroupResult.Rows.Count > 0 & DCC2.Contains("ROLES_PRIVILEGES_ID"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DoesIdentityHavePrivileges(IConnectToDB _Connect, long? _IdentityID, string[] privileges)
        {
            if (_IdentityID == 1000)
                return true;

            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _IdentityID });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = string.Join(",", privileges) });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });

            DataTable ResultDT = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_IDENITITY_PRIVILEGES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection DCC = ResultDT.Columns;

            if (ResultDT.Rows.Count > 0 & DCC.Contains("ROLES_PRIVILEGES_ID"))
            {
                return true;
            }
            else
            {
                List<DynamicModels.RootReportFilter> groupFilters = new List<DynamicModels.RootReportFilter>();

                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _IdentityID });
                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = string.Join(",", privileges) });

                //Check if Identity has privilege via group.
                DataTable GroupResult = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_IDENITITY_PRIVILEGES_GROUPS_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    groupFilters);

                DataColumnCollection DCC2 = GroupResult.Columns;

                if (GroupResult.Rows.Count > 0 & DCC2.Contains("ROLES_PRIVILEGES_ID"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DoesIdentityHavePrivilegeOnCore(IConnectToDB _Connect, long? _IdentityID, string[] privileges, Guid? cores_id)
        {
            if (_IdentityID == 1000)
                return true;

            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            CoreHelper coreHelper = new CoreHelper();
            long? coresId = coreHelper.GetCoreID(_Connect, cores_id);

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _IdentityID });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = coresId });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = string.Join(",", privileges) });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });

            DataTable ResultDT = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_IDENITITY_PRIVILEGES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection DCC = ResultDT.Columns;

            if (ResultDT.Rows.Count > 0 & DCC.Contains("ROLES_PRIVILEGES_ID"))
            {
                return true;
            }
            else
            {
                List<DynamicModels.RootReportFilter> groupFilters = new List<DynamicModels.RootReportFilter>();

                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _IdentityID });
                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = coresId });
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = string.Join(",", privileges) });
                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "Y" });

                //Check if Identity has privilege via group.
                DataTable GroupResult = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_IDENITITY_PRIVILEGES_GROUPS_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    groupFilters);

                DataColumnCollection DCC2 = GroupResult.Columns;

                if (GroupResult.Rows.Count > 0 & DCC2.Contains("ROLES_PRIVILEGES_ID"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DoesIdentityHavePrivilegeOnApp(IConnectToDB _Connect, long? _IdentityID, string[] privileges, Guid? app_uuid)
        {
            if (_IdentityID == 1000)
                return true;

            AppHelper appHelper = new AppHelper();
            Guid? coreUUID = appHelper.GetAppCoreUUID(_Connect, app_uuid);
            bool hasAccess = false;

            if (coreUUID != null)
            {
                hasAccess = DoesIdentityHavePrivilegeOnCore(_Connect, _IdentityID, privileges, coreUUID);
            }

            return hasAccess;
        }

        public bool DoesIdentityHaveApplicationCreatePrivilege(IConnectToDB _Connect, string _IdentityID)
        {
            if (_IdentityID == "1000")
                return true;

            ER_Query er_query = new ER_Query();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "CREATE APPLICATION" });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "Identities_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _IdentityID });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_CORES_ID", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "1000" });

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ROLES_PRIVILEGES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, Filters);

            DataColumnCollection DCC = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 & DCC.Contains("ROLES_PRIVILEGES_ID"))
            {
                return true;
            }
            else
            {
                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
                List<DynamicModels.RootReportFilter> groupFilters = new List<DynamicModels.RootReportFilter>();

                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _IdentityID });
                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_CORES_ID", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "1000" });
                groupFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PRIVILEGE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "CREATE APPLICATION" });

                //Check if Identity has privilege via group.
                DataTable GroupResult = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_IDENITITY_PRIVILEGES_GROUPS_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    groupFilters);

                DataColumnCollection DCC2 = GroupResult.Columns;

                if (GroupResult.Rows.Count > 0 & DCC2.Contains("ROLES_PRIVILEGES_ID"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DoesIdentityHaveAppAccess(IConnectToDB _Connect, long? _Identities_ID, long? _Applications_id, long? _Roles_id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "APPLICATIONS_ID_", DBType = SqlDbType.BigInt, ParamValue = _Applications_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLES_ID_", DBType = SqlDbType.BigInt, ParamValue = _Roles_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _Identities_ID });

            DataTable ResultDT = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__APP_ACCESS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            if (ResultDT.Rows.Count > 0)
                return true;
            else
                return false;
        }

        public string LogActivity(IConnectToDB _Connect, string _activitytype, string _coresid, string _applicationsid, string _tableSource, string _tableid, long? _identitiesId, string _variantsID, string _symbolsID, string _descText, string _descvariantID, string _descsymbolsID, string _descmetatext)
        {
            add addHelp = new add();

            Values.AddActivity thisActivity = addHelp.AddActivity(_Connect, new Values.AddActivity
            {
                I_OBJECT_TYPE = _activitytype,
                I_CORES_ID = ER_Tools.ConvertToInt64(_coresid),
                I_APPLICATIONS_ID = ER_Tools.ConvertToInt64(_applicationsid),
                I_TABLE_SOURCE = _tableSource,
                I_TABLE_ID = ER_Tools.ConvertToInt64(_tableid),
                I_IDENTITIES_ID = _identitiesId,
                I_VARIANTS_ID = ER_Tools.ConvertToInt64(_variantsID),
                I_SYMBOLS_ID = ER_Tools.ConvertToInt64(_symbolsID),
                I_DESC_TEXT = _descText,
                I_DESC_VARIANTS_ID = ER_Tools.ConvertToInt64(_descvariantID),
                I_DESC_SYMBOLS_ID = ER_Tools.ConvertToInt64(_descsymbolsID),
                I_DESC_META_TEXT = _descmetatext
            });

            return thisActivity.O_ERR_MESS;

        }

        public static List<IdentityRoles> GetIdentityRoles(IConnectToDB _Connect, Guid? id, string enabled)
        {
            List<DynamicModels.RootReportFilter> StageRoles = new List<DynamicModels.RootReportFilter>();

            StageRoles.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_UUID", ParamValue = id });
            StageRoles.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED", ParamValue = enabled });

            DataTable thisAppRolesDT = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_ROLES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "", start = 0, verify = "T" },
                StageRoles);

            DataColumnCollection appRoleDCC = thisAppRolesDT.Columns;

            string thisIdentityRoles = JsonConvert.SerializeObject(thisAppRolesDT, Formatting.Indented);

            List<IdentityRoles> CurrentPersonRoles = JsonConvert.DeserializeObject<List<IdentityRoles>>(thisIdentityRoles);

            return CurrentPersonRoles;
        }
    }
}
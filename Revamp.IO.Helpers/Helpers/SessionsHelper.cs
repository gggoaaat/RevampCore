using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Web.Security;
//using DotNetOpenAuth.AspNet;
//using Microsoft.Web.WebPages.OAuth;
//using WebMatrix.WebData;
using System.Data;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using Revamp.IO.Helpers.Helpers;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.Structs;
using Revamp.IO.Structs.Models.DataEntry;
using Microsoft.AspNetCore.Http;

namespace Revamp.IO.Helpers.Helpers
{
    public class SessionsHelper : Controller
    {
        public HttpContext Current => new HttpContextAccessor().HttpContext;
        public string isUserValid(IConnectToDB _Connect, string Username, string Password, out string uname)
        {
            //Logic comes later make everyone true for now.
            ER_Query er_query = new ER_Query();
            uname = "";
            DataTable TempDataTable = GetUser(_Connect, new DataTable(), Username, Password);

            if (TempDataTable.Rows.Count == 0)
                return "The user name or password is incorrect.";
            else
            {
                if (TempDataTable.Columns.Contains("VERIFIED"))
                    if (TempDataTable.Rows[0]["VERIFIED"].ToString() == "N" && TempDataTable.Rows[0]["ACTIVE"].ToString() == "N")
                    {
                        return "This account has not been verified yet. <a href='/account/ResendVerfiyEmail?username=" + Username + "'> Click here to resend verification email</a>.";
                    }
                    else if (TempDataTable.Rows[0]["Active"].ToString() == "N")
                    {
                        return "This account has been deactivated.";
                    }
                    else if (TempDataTable.Rows[0]["ENABLED"].ToString() == "N")
                    {
                        return "This account has been deleted.";
                    }
                    else
                    {
                        if (TempDataTable.Rows[0]["Email"].ToString().ToLower() == Username.ToLower())
                            uname = GetUsernameByEmail(_Connect, Username);
                        else
                            uname = Username;
                        return "Yes";
                    }
                else if (TempDataTable.Rows[0]["ENABLED"].ToString() == "N")
                {
                    return "This account has been deleted.";
                }
                else if (TempDataTable.Rows[0]["Active"].ToString() == "N")
                {
                    return "This account has been deactivated.";
                }
                else
                {
                    if (TempDataTable.Rows[0]["Email"].ToString().ToLower() == Username)
                        uname = GetUsernameByEmail(_Connect, Username);
                    else
                        uname = Username;
                    return "Yes";
                }
            }
        }
        public string VerifyUserByRegCode(IConnectToDB _Connect, string VerifyUUID)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlinUserName = new ER_Query.Parameter_Run();
            ER_Query.Parameter_Run SQlinPassword = new ER_Query.Parameter_Run();
            string DecryptedPassword = "";

            SQlinUserName._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "UUID", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = VerifyUUID }
            };

            SQlinUserName.sqlIn = "Select a.* from CSA.IDENTITIES a inner join CSA.VW__VERIFY b on (a.IDENTITIES_ID = b.IDENTITIES_ID and b.UUID = @UUID) where b.UUID = @UUID";

            DataTable usernamedt = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinUserName);

            if (usernamedt.Rows.Count == 1)
            {
                foreach (DataRow _DR in usernamedt.Rows)
                {
                    SQlinPassword._dbParameters = new List<DBParameters>
                    {
                        new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = _DR.Field<long?>("identities_id").ToString() }
                    };

                    SQlinPassword.sqlIn = "Select a.* from CSA.ID_PASSWORD a inner join CSA.ID_PASSWORD b on a.IDENTITIES_ID = b.IDENTITIES_ID and a.RENDITION in (select max(c.RENDITION) from " +
                    "CSA.ID_PASSWORD c where c.Identities_id = b.Identities_ID) where a.identities_id = @IDENTITIES_ID";

                    DataTable passdt = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinPassword);

                    byte[] EncryptedPassword = (byte[])passdt.Rows[0]["Password"];
                    long IDENTITIES_ID = (long)passdt.Rows[0]["IDENTITIES_ID"];

                    ER_Sec er_sec = new ER_Sec();

                    DecryptedPassword = er_sec.DecryptStringFromBytes_Aes(EncryptedPassword, er_sec.GetCryptPairforID(_Connect, IDENTITIES_ID, new ER_CRYPT_PAIR()));

                    if (passdt.Rows.Count != 0)
                    {
                        //ER_DML er_dml = new ER_DML();
                        //er_dml.OBJECT_DML(_Connect, "Update", "Identities", "ENABLED", IDENTITIES_ID, new Object_Value { _String = "Y" }); Commented out because this is already being set to yes when the identity is created.
                        add addHelp = new add();
                        long? IdentitiesId = null;

                        //er_dml.OBJECT_DML(_Connect, "Update", "Identities", "ACTIVE", IDENTITIES_ID, new Object_Value { _String = "Y" });
                        //er_dml.OBJECT_DML(_Connect, "Update", "Identities", "VERIFIED", IDENTITIES_ID, new Object_Value { _String = "Y" });

                        Values.UpdateIdentity IdentitiesModel = null;
                        IdentitiesModel = addHelp.UPDATE_ENTRY_Identities(_Connect, new Values.UpdateIdentity
                        {
                            I_IDENTITIES_ID = _DR.Field<long?>("IDENTITIES_ID"),
                            I_OBJECT_TYPE = _DR.Field<string>("OBJECT_TYPE"),
                            I_USER_NAME = _DR.Field<string>("USER_NAME"),
                            I_EDIPI = _DR.Field<string>("EDIPI"),
                            I_EMAIL = _DR.Field<string>("EMAIL"),
                            I_ACTIVE = "Y",
                            I_VERIFIED = "Y",
                        });
                        IdentitiesId = IdentitiesModel.O_IDENTITIES_ID;

                        VerificationHelper VH = new VerificationHelper();
                        VH.MarkVerificationsForID(_Connect, IDENTITIES_ID, "CreateUser");
                        VH.DisableVerificationsForID(_Connect, IDENTITIES_ID, "CreateUser");
                    }
                    break;
                }
            }
            return DecryptedPassword;
        }

        public string isPendingUserValid(IConnectToDB _Connect, string Username, string Password, string VerifyUUID)
        {
            //Logic comes later make everyone true for now.
            ER_Query er_query = new ER_Query();
            string uname = "";
            DataTable TempDataTable = GetPendingUser(_Connect, new DataTable(), Username, Password, VerifyUUID);

            if (TempDataTable.Rows.Count > 0)
            {
                if (TempDataTable.Rows[0]["Email"].ToString().ToLower() == Username.ToLower())
                {
                    //get username by emailid
                    uname = GetUsernameByEmail(_Connect, Username);

                    /*Session["UserName"] = uname;
                    Session["User"] = uname;*/
                }
                else if (TempDataTable.Rows[0]["User_Name"].ToString().ToLower() == Username.ToLower())
                {
                    uname = Username;
                }
            }
            return uname;
        }

        public Boolean isUserValid(IConnectToDB _Connect, string EDIPI)
        {
            ER_Query er_query = new ER_Query();

            DataRow TempDR = GetUser(_Connect, EDIPI);

            try
            {
                if (!string.IsNullOrWhiteSpace(EDIPI) && TempDR.Field<string>("EDIPI") == EDIPI )
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public Boolean CreateSession(string username)
        {
            try
            {
                Session["User"] = username;
                FormsAuthentication.SetAuthCookie(username, false);
                Session["UserName"] = username;


                return true;
            }

            catch
            {
                return false;
            }

        }

        public DataTable GetUser(IConnectToDB _Connect, DataTable _DT, string Username, string SubmittedPassword)
        {
            ER_Query er_query = new ER_Query();
            IdentityHelper ih = new IdentityHelper();

            _DT = new DataTable();

            DataTable usernamedt = ih.FindIdentity(_Connect, Username);

            if (usernamedt.Rows.Count == 1)
            {
                foreach (DataRow _DR in usernamedt.Rows)
                {
                    if (_DR.Field<string>("User_name").ToLower() == Username.ToLower() || _DR.Field<string>("Email").ToLower() == Username.ToLower())
                    {
                        List<DynamicModels.RootReportFilter> passwordFilters = new List<DynamicModels.RootReportFilter>();

                        passwordFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", ParamValue = _DR.Field<long?>("identities_id") });

                        DataTable passdt = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_" + "VW__ID_PASSWORD" + "_SEARCH",
                            new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                            passwordFilters);

                        if (passdt.Rows.Count != 0)
                        {
                            byte[] EncryptedPassword = (byte[])passdt.Rows[0]["Password"];
                            long? IDENTITIES_ID = (long?)passdt.Rows[0]["IDENTITIES_ID"];

                            ER_Sec er_sec = new ER_Sec();

                            string DecryptedPasswordHash = er_sec.DecryptStringFromBytes_Aes(EncryptedPassword, er_sec.GetCryptPairforID(_Connect, IDENTITIES_ID, new ER_CRYPT_PAIR()));                            
                            if (passdt.Rows.Count != 0 && ER_Sec.VerifyHash(SubmittedPassword, "SHA512", DecryptedPasswordHash))
                            {
                                _DT = usernamedt;
                            }
                        }
                        else { _DT = usernamedt; }

                        break;
                    }
                }
            }

            return _DT;
        }
        public string GetUsernameForVerification(IConnectToDB _Connect, string VerifyUUID)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "UUID", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = VerifyUUID }
            };

            SQlin.sqlIn = "Select a.* from CSA.IDENTITIES a inner join CSA.VW__VERIFY b on (a.IDENTITIES_ID = b.IDENTITIES_ID and b.UUID = @UUID)";

            DataTable usernamedt = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            if (usernamedt.Rows.Count == 1)
            {
                return usernamedt.Rows[0]["User_Name"].ToString();
            }
            return "";
        }

        public string GetUsernameByEmail(IConnectToDB _Connect, string email)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EMAIL_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = email });

            DataTable usernamedt = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            if (usernamedt.Rows.Count == 1)
            {
                return usernamedt.Rows[0]["User_Name"].ToString();
            }
            return "";
        }

        public DataTable GetPendingUser(IConnectToDB _Connect, DataTable _DT, string Username, string SubmittedPassword, string VerifyUUID)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlinUserName = new ER_Query.Parameter_Run();
            ER_Query.Parameter_Run SQlinPassword = new ER_Query.Parameter_Run();

            SQlinUserName._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "UUID", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = VerifyUUID },
                new DBParameters { ParamName = "USER_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = Username.ToLower() },
            };

            SQlinUserName.sqlIn = "Select a.* from CSA.IDENTITIES a inner join CSA.VW__VERIFY b on (a.IDENTITIES_ID = b.IDENTITIES_ID and b.UUID = @UUID) where LOWER(a.User_name) = @USER_NAME or LOWER(a.Email) = @USER_NAME";

            DataTable usernamedt = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinUserName);

            if (usernamedt.Rows.Count == 1)
            {
                foreach (DataRow _DR in usernamedt.Rows)
                {
                    if (_DR.Field<string>("User_name").ToLower() == Username.ToLower() || _DR.Field<string>("Email").ToLower() == Username.ToLower())
                    {
                        SQlinPassword._dbParameters = new List<DBParameters>
                        {
                            new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue =  _DR.Field<long?>("identities_id") }
                        };

                        SQlinPassword.sqlIn = "Select a.* from CSA.ID_PASSWORD a inner join CSA.ID_PASSWORD b on a.IDENTITIES_ID = b.IDENTITIES_ID and a.RENDITION in (select max(c.RENDITION) from CSA.ID_PASSWORD c where c.Identities_id = b.Identities_ID) where a.identities_id = @IDENTITIES_ID";

                        DataTable passdt = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinPassword);

                        byte[] EncryptedPassword = (byte[])passdt.Rows[0]["Password"];
                        long? IDENTITIES_ID = (long?)passdt.Rows[0]["IDENTITIES_ID"];

                        ER_Sec er_sec = new ER_Sec();

                        string DecryptedPasswordHash = er_sec.DecryptStringFromBytes_Aes(EncryptedPassword, er_sec.GetCryptPairforID(_Connect, IDENTITIES_ID, new ER_CRYPT_PAIR()));

                        if (passdt.Rows.Count != 0 && ER_Sec.VerifyHash(SubmittedPassword, "SHA512", DecryptedPasswordHash)
)
                        {
                            _DT = usernamedt;

                            ER_DML er_dml = new ER_DML();

                            //er_dml.OBJECT_DML(_Connect, "Update", "Identities", "ENABLED", IDENTITIES_ID, new Object_Value { _String = "Y" }); Commented out because this is already being set to yes when the identity is created.
                            er_dml.OBJECT_DML(_Connect, "Update", "Identities", "ACTIVE", IDENTITIES_ID, new Object_Value { _String = "Y" });
                            er_dml.OBJECT_DML(_Connect, "Update", "Identities", "VERIFIED", IDENTITIES_ID, new Object_Value { _String = "Y" });

                            VerificationHelper VH = new VerificationHelper();
                            VH.DisableVerificationsForID(_Connect, IDENTITIES_ID, "CreateUser");


                        }

                        break;
                    }
                }
            }

            return _DT;
        }

        public DataRow GetUser(IConnectToDB _Connect, string EDIPI)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EDIPI_", ParamValue = EDIPI });

            DataTable edipidt = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataRow _DR = edipidt.NewRow();

            if (edipidt.Rows.Count == 1)
            {
                foreach (DataRow _DR1 in edipidt.Rows)
                {
                    try
                    {
                        if (_DR1.Field<string>("EDIPI") == EDIPI)
                        {
                            _DR = _DR1;

                            break;
                        }
                    }
                    catch
                    {
                        //Do Nothing
                    }
                }
            }

            return _DR;
        }

        public DataTable GetIDviaUserName(IConnectToDB _Connect, string Username)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "USER_NAME_", ParamValue = Username });

            DataTable usernamedt = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return usernamedt;
        }

        /*public string LogRequest(IConnectToDB _Connect, SessionRequest _ERSession)
        {
            ER_DML er_dml = new ER_DML();
            List<DBParameters> EntryProcedureParameters = new List<DBParameters>();

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_IDENTITIES_ID",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.Int,
                ParamValue = _ERSession.identities_id
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_SESSIONID",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.sessionid
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_TIMEOUT",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.timeout
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_ANONYMOUSID",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.anonymousid
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_USERAGENT",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.useragent
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_USERHOSTADDRESS",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.userhostaddress
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_ISAUTHENTICATED",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.isauthenticated
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_LOGONUSERIDENTITY",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.logonuseridentity
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_BROWSER",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.browser
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_MAJORVERSION",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.majorversion
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_VERSION",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.version
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_CRAWLER",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.crawler
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_CLRVERSION",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.clrversion
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_COOKIES",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.cookies
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_ISMOBILEDEVICE",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.ismobiledevice
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_PLATFORM",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.platform
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_URL",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.url
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "P_URLREFERRER",
                ParamDirection = ParameterDirection.Input,
                MSSqlParamDataType = SqlDbType.VarChar,
                ParamSize = 4000,
                ParamValue = _ERSession.urlreferrer
            });

            EntryProcedureParameters.Add(new DBParameters
            {
                ParamName = "R_SESSIONS_ID",
                ParamDirection = ParameterDirection.Output,
                MSSqlParamDataType = SqlDbType.Int
            });

            return er_dml.ADD_ENTRY(_Connect, "SESSIONS", EntryProcedureParameters);
        }*/

        public string LogRequest(IConnectToDB _Connect, SessionRequest _ERSession)
        {
            add addHelp = new add();
            long? sessionsId = null;

            Values.AddSession SessionsModel = null;
            SessionsModel = addHelp.ADD_ENTRY_SESSION(_Connect, new Values.AddSession
            {
                I_BASE_SESSIONS_ID = 0,
                I_PREV_SESSIONS_ID = 0,
                //I_BASE_SESSIONS_UUID,
                //I_PREV_SESSIONS_UUID,
                I_USERNAME = _ERSession.username,
                I_IDENTITIES_ID = _ERSession.identities_id,
                I_ENABLED = 'Y',
                //I_DT_AVAILABLE = "",
                //I_DT_END = "",
                I_SESSIONID = _ERSession.sessionid,
                I_TIMEOUT = _ERSession.timeout,
                I_ANONYMOUSID = _ERSession.anonymousid,
                I_USERAGENT = _ERSession.useragent,
                I_USERHOSTADDRESS = _ERSession.userhostaddress,
                I_ISAUTHENTICATED = _ERSession.isauthenticated,
                I_LOGONUSERIDENTITY = _ERSession.logonuseridentity,
                I_BROWSER = _ERSession.browser,
                I_MAJORVERSION = _ERSession.majorversion,
                I_VERSION = _ERSession.version,
                I_CRAWLER = _ERSession.crawler,
                I_CLRVERSION = _ERSession.clrversion,
                I_COOKIES = _ERSession.cookies,
                I_ISMOBILEDEVICE = _ERSession.ismobiledevice,
                I_PLATFORM = _ERSession.platform,
                I_URL = _ERSession.url,
                I_URLREFERRER = _ERSession.urlreferrer
                //O_ERR_NUMB = "",
                //O_ERR_MESS = "",
                //O_SESSIONS_ID = ""
            });
            sessionsId = SessionsModel.O_SESSIONS_ID;

            return sessionsId.ToString();
            //return er_dml.ADD_ENTRY(_Connect, "SESSIONS", EntryProcedureParameters);
        }

        public string GetSessionAndRequest(IConnectToDB _Connect, long? _identities_id, string uname)
        {

            SessionRequest SR = new SessionRequest();

            SR.identities_id = _identities_id > 0 ? _identities_id : 0;
            SR.username = uname;
            //TODO: Refactor for .Net Core
           /* SR.anonymousid = System.Web.HttpContext.Current.Request.AnonymousID != null ? System.Web.HttpContext.Current.Request.AnonymousID : "0";
            SR.sessionid = !Current.Session.Keys.Contains("SessionID") ? "" : Current.Session.GetString("SessionID");
            SR.timeout = !Current.Session.Keys.Contains("Timeout") ? "" : Current.Session.GetString("Timeout");
            SR.useragent = !Current.Request.Headers.Keys.Contains("User-Agent") ? "" : Current.Request.Headers["User-Agent"].ToString();
            SR.userhostaddress = !Current.Request.Headers.Keys.Contains("UserHostAddress") ? "" : Current.Request.Headers["UserAgent"].ToString();
            SR.userhostaddress = System.Web.HttpContext.Current.Request.UserHostAddress == null ? "" : System.Web.HttpContext.Current.Request.UserHostAddress;
            SR.userhostname = System.Web.HttpContext.Current.Request.UserHostName == null ? "" : System.Web.HttpContext.Current.Request.UserHostName;
            SR.isauthenticated = System.Web.HttpContext.Current.Request.IsAuthenticated.ToString() == null ? "" : System.Web.HttpContext.Current.Request.IsAuthenticated.ToString();
            SR.logonuseridentity = System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.ToString() == null ? "" : System.Web.HttpContext.Current.Request.LogonUserIdentity.Name.ToString();
            SR.browser = System.Web.HttpContext.Current.Request.Browser.Browser.ToString() == null ? "" : System.Web.HttpContext.Current.Request.Browser.Browser.ToString();
            SR.majorversion = System.Web.HttpContext.Current.Request.Browser.MajorVersion.ToString() == null ? "" : System.Web.HttpContext.Current.Request.Browser.MajorVersion.ToString();
            SR.version = System.Web.HttpContext.Current.Request.Browser.Version.ToString() == null ? "" : System.Web.HttpContext.Current.Request.Browser.Version.ToString();
            SR.crawler = System.Web.HttpContext.Current.Request.Browser.Crawler.ToString() == null ? "" : System.Web.HttpContext.Current.Request.Browser.Crawler.ToString();
            SR.clrversion = System.Web.HttpContext.Current.Request.Browser.ClrVersion.ToString() == null ? "" : System.Web.HttpContext.Current.Request.Browser.ClrVersion.ToString();
            SR.cookies = "";
            SR.ismobiledevice = System.Web.HttpContext.Current.Request.Browser.IsMobileDevice.ToString() == null ? "" : System.Web.HttpContext.Current.Request.Browser.IsMobileDevice.ToString();
            SR.platform = System.Web.HttpContext.Current.Request.Browser.Platform == null ? "" : System.Web.HttpContext.Current.Request.Browser.Platform;
            SR.url = System.Web.HttpContext.Current.Request.Url.ToString() == null ? "" : System.Web.HttpContext.Current.Request.Url.ToString();
            SR.urlreferrer = (System.Web.HttpContext.Current.Request.UrlReferrer == null) ? "" : System.Web.HttpContext.Current.Request.UrlReferrer.ToString();*/

            return LogRequest(_Connect, SR);
        }


    }
}
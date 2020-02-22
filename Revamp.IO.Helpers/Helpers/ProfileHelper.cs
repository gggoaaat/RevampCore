using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using Revamp.IO.Structs.Models.DataEntry;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.Structs;
using static Revamp.IO.Structs.Models.UIModels;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Revamp.IO.Helpers.Helpers
{
    public class ProfileHelper
    {
        public HttpContext Current => new HttpContextAccessor().HttpContext;
        public SessionObjects AddProfilePic(IConnectToDB _Connect, SessionObjects SO, FormCollection _formCollection)
        {
            try
            {
                if (_formCollection.Keys.Count > 1)
                {
                    SecurityHelper securityHelper = new SecurityHelper();
                    IdentityHelper identityHelper = new IdentityHelper();
                    ER_DML er_dml = new ER_DML();
                    add addHelp = new add();
                    long? identitiesId = ER_Tools.ConvertToInt64(identityHelper.GetIdentityID(_Connect, _formCollection["identitiesUUID"].ToString()));
                    string fileName = _formCollection["fileName"].ToString();
                    string fileType = _formCollection["fileType"].ToString();
                    long? fileSize = ER_Tools.ConvertToInt64(_formCollection["fileSize"].ToString());
                    byte[] fileContent = Encoding.UTF8.GetBytes(_formCollection["profileImage"].ToString());
                    Guid? ProfilesUUID = ER_Tools.ConvertToGuid(_formCollection["profilesUUID"].ToString());
                    long? Profiles_ID = GetProfileId(_Connect, ProfilesUUID.ToString());
                    long? profileImagesId = 0;
                    var profileImagesDT = GetImageIdByProfile(_Connect, Profiles_ID);
                    DataColumnCollection _dccColumnID = profileImagesDT.Columns;

                    if (_dccColumnID.Contains("PROFILE_IMAGES_ID") && profileImagesDT.Rows.Count > 0)
                    {
                        Values.UpdateProfileImages ProfileImagesModel = null;
                        ProfileImagesModel = addHelp.UPDATE_ENTRY_Profile_Images(_Connect, new Values.UpdateProfileImages
                        {
                            I_PROFILE_IMAGES_ID = profileImagesDT.Rows[0].Field<long?>("PROFILE_IMAGES_ID"),
                            I_PROFILES_ID = Profiles_ID,
                            I_FILE_NAME = fileName,
                            I_FILE_SIZE = fileSize,
                            I_CONTENT_TYPE = fileType,
                            I_VALUE = fileContent
                        });

                        profileImagesId = ProfileImagesModel.O_PROFILE_IMAGES_ID;

                        //Enable Profile Image
                        er_dml.TOGGLE_OBJECT(_Connect, "PROFILE_IMAGES", profileImagesId, "Y");
                    }
                    else
                    {
                        //Values.AddProfilesSecPriv ProfilesSecPrivModel = null;
                        //ProfilesSecPrivModel = addHelp.ADD_ENTRY_Profiles_Sec_Priv(_Connect, new Values.AddProfilesSecPriv
                        //{
                        //    I_OBJECT_TYPE = "Permission",
                        //    I_PROFILES_ID = Profiles_ID,
                        //    I_PRIVILEGES_ID = ER_Tools.ConvertToInt64(securityHelper.GetPrivID(_Connect, "CREATE OBJECT")),
                        //    I_ENABLED = 'Y',
                        //    I_IDENTITIES_ID = identitiesId
                        //});

                        Values.AddProfileImages ProfileImagesModel = null;
                        ProfileImagesModel = addHelp.ADD_ENTRY_Profile_Images(_Connect, new Values.AddProfileImages
                        {
                            I_IDENTITIES_ID = identitiesId,
                            I_PROFILES_UUID = ProfilesUUID,
                            I_ENABLED = 'Y',
                            I_FILE_NAME = fileName,
                            I_FILE_SIZE = fileSize,
                            I_CONTENT_TYPE = fileType,
                            I_VALUE = fileContent
                        });
                    }

                    //Set Profile Image
                    HttpContext.Current.Session["ProfileImage"] = GetProfileImage(_Connect, identitiesId);
                }
            }
            catch
            {

            }

            return SO;
        }
        public bool RecoverUsername(IConnectToDB _Connect, string _emailaddress)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EMAIL_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = _emailaddress });

            DataTable Username = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ID_PASSWORD_SEARCH",
                new DataTableDotNetModelMetaData { columns = "USER_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            if (Username.Rows.Count > 0)
            {
                string Host = Current.Request.Scheme + "://" + Current.Request.Host.ToString().ToLower();
                if (Current.Request.Host.Port != 80 &&
                    Current.Request.Host.Port != 443)
                { Host = Host + ":" + Current.Request.Host.Port; }
                string VerifyURL = Host + "/Login/";
                string EmailBody = "This email was sent by an automated administrator. Please do not reply to this message." +
                  "<br /><br />" +
                  "Your Revamp username was requested at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " via the forgot username link. Your username for your Revamp account is: <b>" + Username.Rows[0]["USER_NAME"] + "</b>" +
                "<br /><br />Please click <a href='" + VerifyURL + "'>here</a> to login"
                + "<br/><br/> If you did not request your username no further action is needed on your part."
                + "<br /><br /><br /><br />"
                + "Thanks for using Revamp!";
                string tempemail = _emailaddress;
                IOHelper ioHelper = new IOHelper();
                try
                {
                    ioHelper.SendEmail(tempemail, "NO REPLY: Username for  " + _Connect.SourceDBOwner.ToUpper() + " ACCOUNT", EmailBody);
                }
                catch
                {
                    //Couldn't send email.
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool RecoverPassword(IConnectToDB _Connect, string _emailaddress)
        {
            add addHelp = new add();

            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EMAIL_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = _emailaddress });

            DataTable PASSWORD = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ID_PASSWORD_SEARCH",
                new DataTableDotNetModelMetaData { columns = "PASSWORD, IDENTITIES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection _dccColumnID = PASSWORD.Columns;

            if (_dccColumnID.Contains("IDENTITIES_ID") && PASSWORD.Rows.Count > 0)
            {
                long? IdentityId = PASSWORD.Rows[0].Field<long?>("IDENTITIES_ID");
                Values.AddVerify thisVerify = addHelp.ADD_ENTRY_Verify(_Connect, new Values.AddVerify { I_IDENTITIES_ID = IdentityId, I_VALIDATION_TYPE = "ResetPassword" });
                string VerifyUUID = thisVerify.I_UUID;

                string Host = Current.Request.Scheme + "://" + Current.Request.Host.ToString().ToLower();
                if (Current.Request.Host.Port != 80 &&
                    Current.Request.Host.Port != 443)
                { Host = Host + ":" + Current.Request.Host.Port; }

                string VerifyURL = Host + "/ResetPassword/Index/" + VerifyUUID;
                string EmailBody = HttpUtility.HtmlEncode("This email was sent by an automated administrator. Please do not reply to this message.<br><br>This link will be active for 1 hour!" +
                    "<br /><br />" +
                    "Please click <a href='" + VerifyURL + "'>here</a> to reset your password.");

                //"<br /><br />" +
                //_Connect.SourceDBOwner + " Verification URL: " +
                //"<br /><br />" +
                //VerifyURL +
                //"<br /><br /><br /><br />" +
                //"Thanks for using Revamp!"
                string tempemail = _emailaddress;
                IOHelper ioHelper = new IOHelper();
                try
                {
                    string subject = "NO REPLY: Password reset link for " + _Connect.SourceDBOwner.ToUpper() + " ACCOUNT";
                    ioHelper.SendEmail(tempemail, subject, EmailBody);
                }
                catch (Exception err)
                {
                    //Couldn't send email.
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ResetPassword(IConnectToDB _Connect, string VerifyUUID, string password)
        {
            ER_Sec er_sec = new ER_Sec();
            ER_DML er_dml = new ER_DML();

            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> verifyIdentityFilters = new List<DynamicModels.RootReportFilter>();
            List<DynamicModels.RootReportFilter> idPasswordFilters = new List<DynamicModels.RootReportFilter>();

            verifyIdentityFilters.Add(new DynamicModels.RootReportFilter { FilterName = "UUID_", DBType = SqlDbType.VarChar, ParamValue = VerifyUUID });

            DataTable verifyIdentityTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__VERIFY_SEARCH",
                new DataTableDotNetModelMetaData { columns = "IDENTITIES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                verifyIdentityFilters);

            idPasswordFilters.Add(new DynamicModels.RootReportFilter { FilterName = "Identities_ID_", DBType = SqlDbType.BigInt, ParamValue = verifyIdentityTable.Rows[0].Field<long?>("IDENTITIES_ID") });

            DataTable idPasswordTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ID_PASSWORD_SEARCH",
                new DataTableDotNetModelMetaData { columns = "ID_PASSWORD_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                idPasswordFilters);

            //Update Password
            string hash = ER_Sec.ComputeHash(password, "SHA512", null);
            er_dml.OBJECT_DML(_Connect, "Update", "ID_PASSWORD", "PASSWORD", idPasswordTable.Rows[0].Field<long?>("ID_PASSWORD_ID"),
                new Object_Value
                {
                    _File = new File_Object
                    {
                        _FileBytes = er_sec.EncryptStringToBytes_Aes(hash,
                        er_sec.GetCryptPairforID(_Connect, verifyIdentityTable.Rows[0].Field<long?>("IDENTITIES_ID"), new ER_CRYPT_PAIR()))
                    }
                });

            //disable verification
            VerificationHelper VH = new VerificationHelper();
            VH.DisableVerificationsForID(_Connect, verifyIdentityTable.Rows[0].Field<long?>("IDENTITIES_ID"), "ResetPassword");
        }
        public void ChangePassword(IConnectToDB _Connect, Guid? uuid, string password)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            ER_Sec er_sec = new ER_Sec();
            add addHelp = new add();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_UUID_", DBType = SqlDbType.UniqueIdentifier, ParamValue = uuid });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__ID_PASSWORD_SEARCH",
                new DataTableDotNetModelMetaData { columns = "ID_PASSWORD_ID,RENDITION,IDENTITIES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection _dccColumnID = TempDataTable.Columns;

            if (_dccColumnID.Contains("ID_PASSWORD_ID") && TempDataTable.Rows.Count > 0)
            {
                //Update Password
                Values.UpdateIDPassword IDPasswordModel = null;
                string hash = ER_Sec.ComputeHash(password, "SHA512", null);
                IDPasswordModel = addHelp.UPDATE_ENTRY_Identities_Password(_Connect, new Values.UpdateIDPassword
                {
                    I_ID_PASSWORD_ID = TempDataTable.Rows[0].Field<long?>("ID_PASSWORD_ID"),
                    I_OBJECT_TYPE = "Password",
                    I_RENDITION = TempDataTable.Rows[0].Field<long?>("RENDITION"),
                    I_PASSWORD = er_sec.EncryptStringToBytes_Aes(hash, er_sec.GetCryptPairforID(_Connect, TempDataTable.Rows[0].Field<long?>("IDENTITIES_ID"), new ER_CRYPT_PAIR()))
                });
            }
        }

        public long? AddProfileEntry(IConnectToDB _Connect, long? identitiesId)
        {
            SecurityHelper securityHelper = new SecurityHelper();
            add addHelp = new add();
            long? profilesId = null;
            long? privilegesId = ER_Tools.ConvertToInt64(securityHelper.GetPrivID(_Connect, "ADD PROFILE"));

            //Enter profile information
            Values.AddProfiles ProfilesModel = null;
            ProfilesModel = addHelp.ADD_ENTRY_Profiles(_Connect, new Values.AddProfiles
            {
                I_IDENTITIES_ID = identitiesId,
                I_ENABLED = 'Y',
            });
            profilesId = ProfilesModel.O_PROFILES_ID;

            //Enter profile security information
            //Values.AddProfilesSecPriv ProfilesSecPrivModel = null;
            //ProfilesSecPrivModel = addHelp.ADD_ENTRY_Profiles_Sec_Priv(_Connect, new Values.AddProfilesSecPriv
            //{
            //    I_OBJECT_TYPE = "Permission",
            //    I_PROFILES_ID = profilesId,
            //    I_PRIVILEGES_ID = privilegesId,
            //    I_ENABLED = 'Y',
            //    I_IDENTITIES_ID = identitiesId
            //});
            //profilesSecPrivId = ProfilesSecPrivModel.O_PROFILES_SEC_PRIV_ID;

            return profilesId;
        }

        public string UpdateProfileEntry(IConnectToDB _Connect, SessionObjects SO, FormCollection _formCollection)
        {
            SecurityHelper securityHelper = new SecurityHelper();
            IdentityHelper identityHelper = new IdentityHelper();
            ProfileHelper profileHelper = new ProfileHelper();
            add addHelp = new add();

            if (_formCollection.Keys.Count > 1)
            {
                long? identitiesId = ER_Tools.ConvertToInt64(identityHelper.GetIdentityID(_Connect, _formCollection["identitiesUUID"].ToString()));
                long? identitiesIdOwner = SO.SessionIdentity.Identity.identities_id;
                long? profilesId = ER_Tools.ConvertToInt64(profileHelper.GetProfileId(_Connect, _formCollection["profilesUUID"].ToString()));
                string newemail = _formCollection["email"].ToString();

                //Update profile information
                UpdateProfileEmail(_Connect, identitiesId, newemail);

                Values.UpdateProfiles ProfilesModel = null;
                ProfilesModel = addHelp.Update_ENTRY_Profiles(_Connect, new Values.UpdateProfiles
                {
                    I_PROFILES_ID = profilesId,
                    I_IDENTITIES__ID = identitiesIdOwner,
                    I_FIRST_NAME = _formCollection["firstName"].ToString(),
                    I_MIDDLE_NAME = _formCollection["middleName"].ToString(),
                    I_LAST_NAME = _formCollection["lastName"].ToString(),
                    I_OCCUPATION = _formCollection["occupation"].ToString(),
                    I_STATE = _formCollection["state"].ToString(),
                    I_ZIPCODE = _formCollection["zipCode"].ToString(),
                    I_PHONE = _formCollection["phone"].ToString(),
                    I_COUNTRY = _formCollection["country"].ToString(),
                    I_CITY = _formCollection["city"].ToString(),
                    I_ABOUT = _formCollection["about"].ToString(),
                });
            }
            return "";
        }

        //TOLEARN: This is important and has to be refactored for new system.
        public DataTable FindIDStatuses(IConnectToDB _Connect, long? identities_id)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = identities_id }
            };

            SQlin.sqlIn = "Select distinct e.FORMS_ID, a.DT_CREATED 'Date_Submitted', a.DT_CREATED 'Date_Updated', a.IDENTITIES_ID, a.FORMS_DAT_OPT_ID, a.applications_id, d.application_name, a.PROPERTY_VALUE, a.VALUE from " + _Connect.Schema + ".FORMS_DAT_OPT a " +
                "inner join CSA.Object_Sets b ON " +
                "(a.OBJECT_SETS_ID = b.OBJECT_SETS_ID and a.Identities_id = @IDENTITIES_ID) " +
                "inner join  CSA.[OBJ_PROP_SETS] c on (b.OBJECT_SETS_ID = c.OBJECT_SETS_ID) " +
                "inner join CSA.[APPLICATIONS] d on (d.APPLICATIONS_ID = a.APPLICATIONS_ID) " +
                "inner join CSA.FORMS e on (e.FORMS_ID = a.FORMS_ID) " +
                "where c.PROPERTY_NAME in ('Status Check', 'Status Type', 'Status Message') " +
                "and a.RENDITION in (Select MAX(RENDITION) from  CSA.VW__forms_dat_opt fdo2 where fdo2.FORMS_ID = a.FORMS_ID) " +
                "ORDER BY a.FORMS_DAT_OPT_ID desc ";

            DataTable Result_DT = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            return Result_DT;
        }

        public List<IdentityAppStatus> GETIDStatuses(List<IdentityAppStatus> Statuses, DataTable _DT)
        {
            foreach (DataRow GetIdentityStatusRow in _DT.Rows)
            {
                Statuses.Add(GETIDStatus(new IdentityAppStatus(), GetIdentityStatusRow));
            }

            return Statuses;
        }

        public IdentityAppStatus GETIDStatus(IdentityAppStatus _Status, DataRow _DR)
        {
            _Status.forms_id = _DR.Field<long?>("forms_id");
            _Status.application_name = _DR.Field<string>("application_name");
            _Status.applications_id = _DR.Field<long?>("applications_id");
            _Status.identities_id = _DR.Field<long?>("identities_id");
            _Status.StatusName = _DR.Field<string>("PROPERTY_VALUE");
            _Status.StatusValue = _DR.Field<string>("VALUE");
            _Status.datesubmitted = _DR.Field<DateTime>("Date_Submitted");
            _Status.lastupdated = _DR.Field<DateTime>("Date_Updated");

            return _Status;
        }

        public string GetProfileData(IConnectToDB _Connect, long? identity_id, string property)
        {
            string value = "", rendition = "";

            rendition = GetProfileRendition(_Connect, identity_id).ToString();

            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = identity_id });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "RENDITION_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = rendition });
            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PROPERTY_VALUE_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = property });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__PROFILES_DAT_CHAR_SEARCH",
                new DataTableDotNetModelMetaData { columns = "VALUE", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            if (TempDataTable.Rows.Count > 0)
                value = TempDataTable.Rows[0][0].ToString();

            return value;
        }

        public long? GetProfileId(IConnectToDB _Connect, string uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            long? value = 0;

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PROFILES_UUID_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = uuid });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__PROFILES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "PROFILES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection _dccColumnID = TempDataTable.Columns;

            if (_dccColumnID.Contains("PROFILES_ID") && TempDataTable.Rows.Count > 0)
            {
                value = TempDataTable.Rows[0].Field<long?>("PROFILES_ID");
            }

            return value;
        }

        public void UpdateProfileEmail(IConnectToDB _Connect, long? _Identities_ID, string email)
        {
            IdentityHelper identityHelper = new IdentityHelper();

            if (!string.IsNullOrWhiteSpace(email) && _Identities_ID > 0)
            {
                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = _Identities_ID });

                DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                DataColumnCollection _dccColumnID = TempDataTable.Columns;

                if (_dccColumnID.Contains("IDENTITIES_ID") && TempDataTable.Rows.Count > 0)
                {
                    long? identitiesId = TempDataTable.Rows[0].Field<long?>("IDENTITIES_ID");
                    var objectType = TempDataTable.Rows[0].Field<string>("OBJECT_TYPE");
                    var userName = TempDataTable.Rows[0].Field<string>("USER_NAME");
                    var edipi = TempDataTable.Rows[0].Field<string>("EDIPI");
                    var active = TempDataTable.Rows[0].Field<string>("ACTIVE");
                    var verified = TempDataTable.Rows[0].Field<string>("VERIFIED");

                    //Update user
                    string result = identityHelper.UpdateIdentity(_Connect, identitiesId, objectType, userName, edipi, email, active, verified);

                    //SH.LogActivity(_Connect, "Deactivate Identity", "1000", "1000", "IDENTITIES", id.ToString(), SO.SessionIdentity.Identity.identities_id, "1004", "1005", Session["UserName"].ToString().ToUpper() + " deactivated user " + user_name, "1000", "1000", "");
                }
            }
        }

        public byte[] GetProfileImageBytes(IConnectToDB _Connect, long? identity_id)
        {
            if (identity_id != null)
            {
                byte[] value = new byte[0];

                _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

                List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

                DataTable SetInfo = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_SET_INFO_SEARCH",
                    new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                long? _stages_id = 0;
                long? _grips_id = 0;
                long? _objectsets_id = 0;
                long? _propset_id = 0;

                foreach (DataRow _DR in SetInfo.Rows)
                {
                    _stages_id = _DR.Field<long?>("stages_id");
                    _grips_id = _DR.Field<long?>("grips_id");
                    _objectsets_id = _DR.Field<long?>("object_sets_id");
                    _propset_id = _DR.Field<long?>("obj_prop_sets_id");

                    break;
                }

                List<DynamicModels.RootReportFilter> profileDatFileFilters = new List<DynamicModels.RootReportFilter>();

                profileDatFileFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = identity_id });
                //profileDatFileFilters.Add(new DynamicModels.RootReportFilter { FilterName = "STAGES_ID_", DBType = SqlDbType.BigInt, ParamValue = _stages_id });
                //profileDatFileFilters.Add(new DynamicModels.RootReportFilter { FilterName = "GRIPS_ID_", DBType = SqlDbType.BigInt, ParamValue = _grips_id });
                //profileDatFileFilters.Add(new DynamicModels.RootReportFilter { FilterName = "OBJ_PROP_SETS_ID_", DBType = SqlDbType.BigInt, ParamValue = _propset_id });
                profileDatFileFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ENABLED_", DBType = SqlDbType.VarChar, ParamSize = -1, ParamValue = "Y" });

                DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__PROFILE_IMAGES_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "PROFILE_IMAGES_ID, VALUE", length = -1, order = "PROFILE_IMAGES_ID desc", start = 0, verify = "T" },
                    profileDatFileFilters);

                DataColumnCollection _dccColumnID = TempDataTable.Columns;

                if (_dccColumnID.Contains("VALUE") && TempDataTable.Rows.Count > 0)
                {
                    value = TempDataTable.Rows.Count > 0 ? TempDataTable.Rows[0].Field<byte[]>("VALUE") : new byte[0];
                }

                return value;
            }
            else
            {
                return new byte[0];
            }
        }

        public string GetProfileImage(IConnectToDB _Connect, long? id)
        {
            IdentityHelper identityHelper = new IdentityHelper();
            SecurityHelper securityHelper = new SecurityHelper();
            FilePathResult imageURL;

            byte[] profileimage = GetProfileImageBytes(_Connect, id);

            if (profileimage.Length > 0)
            {
                imageURL = new FilePathResult(Encoding.UTF8.GetString(profileimage), "image/jpeg");
            }
            else
            {
                imageURL = new FilePathResult("/assets/img/profile/profile-img.png", "image/jpeg");
            }

            return imageURL.FileName.ToString().Replace(" ","+");
        }

        public DataTable GetImageIdByProfile(IConnectToDB _Connect, long? id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "PROFILES_ID_", DBType = SqlDbType.BigInt, ParamValue = id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__PROFILE_IMAGES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "PROFILE_IMAGES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public long? GetProfileRendition(IConnectToDB _Connect, long? identity_id)
        {
            long? value = 0;

            ER_DML er_dml = new ER_DML();

            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.BigInt, ParamValue = identity_id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW_PROFILES_DAT_CHAR_RENDITION_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            foreach (DataRow row in TempDataTable.Rows)
            {
                if (row["rendition"].ToString() != "")
                {
                    value = ER_Tools.ConvertToInt64(row["rendition"].ToString());
                }
            }

            return value;
        }
    }
}
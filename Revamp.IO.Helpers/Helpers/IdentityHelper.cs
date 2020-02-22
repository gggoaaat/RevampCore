using Revamp.IO.DB.Binds.IO.Dynamic;
//using Stripe;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Foundation;
using Revamp.IO.Structs.Models;
using Revamp.IO.Structs.Models.DataEntry;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Revamp.IO.Helpers.Helpers
{
    public class IdentityHelper
    {
        public HttpContext Current => new HttpContextAccessor().HttpContext;
        //System.Configuration.ConfigurationManager.AppSettings["InstallType"].ToLower() 
        public String Create(IConnectToDB _Connect, FormCollection _formCollection, string InstallType)
        {

            ER_DML er_dml = new ER_DML();
            add addHelp = new add();
            DMLHelper DMLH = new DMLHelper();
            SecurityHelper SECH = new SecurityHelper();
            string usernameReturn = "";
            FormCollection fc = _formCollection;

            //IValueProvider valueProvider = _formCollection.ToValueProvider();

            long? Identities_ID = null;
            Boolean UserNameSaved = false;
            Boolean PasswordSaved = false;
            var fullname = "";

            if (_formCollection.Keys.Contains("first_name") && _formCollection.Keys.Contains("last_name"))
            {
                fullname = _formCollection["first_name"].ToString() + " " + _formCollection["last_name"].ToString();
            }

            string gripid = _formCollection.Keys.Contains("plan") ? _formCollection["plan"].ToString() : "0";
            string objectsetid = _formCollection.Keys.Contains("objectset")? _formCollection["objectset"].ToString() : "0";
            ObjectPropSetsHelper obj = new ObjectPropSetsHelper();
            DataTable plandt = obj.FindbyGripId(_Connect, gripid, objectsetid);
            var planId = "Developer";
            var coreLimit = "1";
            var applicationLimit = "1";
            var stageLimit = "10";
            var formsubmissionLimit = "3000";
            var fileHostingLimit = "1 GB";
            string price = "0";
            #region SAAS Stuff Remove from this base install.
            if (_formCollection.Keys.Contains("pricing"))
            {
                price = _formCollection["pricing"].ToString();

                foreach (DataRow row in plandt.Rows)
                {
                    if (row["Property_Name"].ToString() == "Cores")
                        coreLimit = row["Property_Value"].ToString();
                    else if (row["Property_Name"].ToString() == "Applications")
                        applicationLimit = row["Property_Value"].ToString();
                    else if (row["Property_Name"].ToString() == "Stages")
                        stageLimit = row["Property_Value"].ToString();
                    else if (row["Property_Name"].ToString() == "Form Submissions")
                        formsubmissionLimit = row["Property_Value"].ToString();
                    else if (row["Property_Name"].ToString() == "File Hosting")
                        fileHostingLimit = row["Property_Value"].ToString();
                    else if (row["Property_Name"].ToString() == "Monthly Price" && row["Property_Value"].ToString() == price)
                        planId = row["GRIP_NAME"].ToString();
                    else if (row["Property_Name"].ToString() == "Yearly Price" && row["Property_Value"].ToString() == price)
                        planId = row["GRIP_NAME"].ToString() + "Annual";
                }
            }

            //Add Customer to Stripe for payment processing
            if (InstallType.ToLower() == "saas" && _formCollection.Keys.Contains("number"))
            {
                //Add Credit Card to Stripe
                //var myToken = new StripeTokenCreateOptions();


                //if (planId != "Developer")
                //{

                //    // if you need this...
                //    myToken.Card = new StripeCreditCardOptions()
                //    {
                //        // set these properties if passing full card details (do not
                //        // set these properties if you set TokenId)
                //        Number = _formCollection["number"].ToString(),
                //        ExpirationYear = _formCollection["card_year"].ToString(),
                //        ExpirationMonth = _formCollection["card_month"].ToString(),
                //        Name = fullname,               // optional
                //        Cvc = _formCollection["cvc"].ToString()                       // optional
                //    };

                //    // set this property if using a customer (stripe connect only)
                //    // myToken.CustomerId = *customerId*;
                //    try
                //    {
                //        var tokenService = new StripeTokenService();
                //        StripeToken stripeToken = tokenService.Create(myToken);

                //        //Add token to customertoken for saving to DB
                //        customertoken = stripeToken.Id;

                //        var myCustomer = new StripeCustomerCreateOptions();

                //        // set these properties if it makes you happy
                //        myCustomer.Email = tempemail;
                //        myCustomer.Description = "Subscription for " + fullname + " on the " + planId + " plan.";

                //        // setting up the card
                //        myCustomer.Source = new StripeSourceOptions()
                //        {
                //            // set this property if using a token
                //            TokenId = stripeToken.Id
                //        };

                //        //myCustomer.PlanId = planId.Replace("Collection", "Core"); // only if you have a plan
                //        //myCustomer.PlanId = planId.Replace("UnlimitedCollection", "UnlimitedCores"); // only if you have a plan
                //        myCustomer.PlanId = planId; // only if you have a plan

                //        var customerService = new StripeCustomerService();
                //        StripeCustomer stripeCustomer = customerService.Create(myCustomer);

                //        //Add customer id to customerid for saving to DB
                //        customerid = stripeCustomer.Id;
                //    }
                //    catch (StripeException se)
                //    {
                //        throw se;
                //    }
                //}
            }
            #endregion
            var EmailAddressValue = _formCollection.Keys.Contains("EmailAddress") ? _formCollection["EmailAddress"].ToString() : "";
            var userNameValue = EmailAddressValue;
            var PasswordValue = _formCollection.Keys.Contains("Password") ? _formCollection["Password"].ToString() : "";
            var ConfirmPasswordValue = _formCollection.Keys.Contains("ConfirmPassword") ? _formCollection["ConfirmPassword"].ToString() : "";
            var FirstNameValue = _formCollection.Keys.Contains("FirstName") ? _formCollection["FirstName"].ToString() : "";
            var MiddleNameValue = _formCollection.Keys.Contains("MiddleName") ? _formCollection["MiddleName"].ToString() : "";
            var LastNameValue = _formCollection.Keys.Contains("LastName") ? _formCollection["LastName"].ToString() : "";

            if (!string.IsNullOrWhiteSpace(EmailAddressValue) && !string.IsNullOrWhiteSpace(PasswordValue) && !string.IsNullOrWhiteSpace(ConfirmPasswordValue))
            {
                if (PasswordValue == ConfirmPasswordValue)
                {
                    string username = "";
                    Values.AddIdentity IdentityModel = null;
                    Values.AddIDPassword password = null;
                    try
                    {
                        IdentityModel = addHelp.ADD_ENTRY_Identities(_Connect, new Values.AddIdentity
                        {
                            I_OBJECT_TYPE = "Application User",
                            I_USER_NAME = userNameValue.Trim(),
                            I_EMAIL = EmailAddressValue,
                            I_EDIPI = null,
                            I_ENABLED = 'Y',  //Set all new accounts enabled, with the active field set to N and the verified field set to N.
                            I_VERIFIED = "N",
                            I_ACTIVE = "N"
                        });

                        Identities_ID = IdentityModel.O_IDENTITIES_ID;

                        Values.AddProfiles ProfilesModel = null;
                        ProfilesModel = addHelp.ADD_ENTRY_Profiles(_Connect, new Values.AddProfiles
                        {
                            I_IDENTITIES_ID = Identities_ID,
                            I_ENABLED = 'Y',
                            I_FIRST_NAME = FirstNameValue,
                            I_MIDDLE_NAME = MiddleNameValue,
                            I_LAST_NAME = LastNameValue
                        });

                        UserNameSaved = true;
                    }
                    catch (Exception)
                    {

                        UserNameSaved = false;
                    }

                    if (Identities_ID > 0)
                    {
                        #region Get Core Information
                        List<DynamicModels.RootReportFilter> coreFilters = new List<DynamicModels.RootReportFilter>();
                        coreFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CORE_NAME_", DBType = SqlDbType.VarChar, ParamValue = "Revamp System" });
                        DataTable CoreFetch = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_CORES_SEARCH",
                                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                                coreFilters);

                        DataColumnCollection coreDCC = CoreFetch.Columns;

                        Guid? Cores_UUID = null;
                        if (coreDCC.Contains("CORES_ID") && CoreFetch.Rows.Count > 0)
                        {
                            Cores_UUID = CoreFetch.Rows[0].Field<Guid?>("CORES_UUID");
                        }
                        #endregion
                        //save roles for identity
                        #region Create Basic Permissions
                        Values.AddIdentityRole PublicAccessModel = addHelp.ADD_ENTRY_Identity_Role(_Connect, new Values.AddIdentityRole { I_IDENTITIES_ID = Identities_ID, I_ROLES_UUID = SECH.GetRoleUUID(_Connect, "PUBLIC ACCESS", Cores_UUID) });

                        add Add = new add();
                        sqlTransBlocks _thisSubmissionSeries = new sqlTransBlocks();

                        List<DynamicModels.RootReportFilter> rolesSearchFilters = new List<DynamicModels.RootReportFilter>();

                        rolesSearchFilters.Add(new DynamicModels.RootReportFilter { FilterName = "GET_LATEST", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "T" });
                        rolesSearchFilters.Add(new DynamicModels.RootReportFilter { FilterName = "ROLE_NAME_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = "PUBLIC ACCESS" });

                        DataTable availableRoles = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_ROLES_SEARCH",
                            new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" }, rolesSearchFilters);
                        DataColumnCollection tempDCC = availableRoles.Columns;

                        if (tempDCC.Contains("ROLES_ID") && availableRoles.Rows.Count > 0)
                        {
                            foreach (DataRow item in availableRoles.Rows)
                            {
                                CASTGOOP.AddIdentityRole submission = new CASTGOOP.AddIdentityRole { I_IDENTITIES_ID = Identities_ID, I_ROLES_UUID = item.Field<Guid?>("ROLES_UUID") };
                                Values.AddIdentityRole thisEntry = new Values.AddIdentityRole();
                                _thisSubmissionSeries = Add.ADD_ENTRY_Generic(_Connect, submission, thisEntry, _thisSubmissionSeries);
                            }

                            string TransactionName = ER_Procedure.TransactionNameGenerator(EmailAddressValue, "USER_REGISTER_ADD_IDENTITY_ROLES");
                            _thisSubmissionSeries = ER_Procedure.SQL_BUILD_BLOCK(_Connect, TransactionName, _thisSubmissionSeries);
                            ER_Query CMD = new ER_Query();
                            List<DataTable> AllResults2 = CMD.RUN_NON_QUERY(_Connect, _thisSubmissionSeries.SQLBlock.ToString(), "Success", new List<DataTable>());

                            // addHelp.ADD_ENTRY_Cores_Identity(IConnect, new Values.AddCoreIdentity { I_IDENTITIES_ID = identityId, I_CORES_UUID = coreId, I_CREATOR = "N" });
                        }


                        #endregion
                        // save default core for identity
                        Values.AddCoreIdentity coreIdentityModel = addHelp.ADD_ENTRY_Cores_Identity(_Connect, new Values.AddCoreIdentity { I_IDENTITIES_ID = Identities_ID, I_CORES_UUID = Cores_UUID, I_CREATOR = "N" });

                        username = userNameValue.Trim();
                        usernameReturn = username;
                        UserNameSaved = true;

                        #region Create Password
                        try
                        {
                            addHelp.GEN_CRYPT_TICKET(_Connect, Identities_ID);

                            password = addHelp.ADD_ENTRY_Identities_Password(_Connect, new Values.AddIDPassword { I_IDENTITIES_ID = Identities_ID, I_RENDITION = 0, V_PASSWORD = PasswordValue });

                            SECH.LogActivity(_Connect, "Register", "1000", "1000", "IDENTITIES", Identities_ID.ToString(), Identities_ID, "1004", "1001", (usernameReturn + " has registered."), "1000", "1000", "");

                            PasswordSaved = true;
                        }
                        catch (Exception)
                        {

                            PasswordSaved = false;
                        }
                        #endregion
                    }

                }
            }

            //Create Identity and Create Profile

            if (UserNameSaved && PasswordSaved)
            {

                SendVerificationEmailtoIdentity(_Connect, Identities_ID, EmailAddressValue);

            }
            return usernameReturn;
        }

        public string AddIdentity(IConnectToDB _Connect, FormCollection _formCollection)
        {
            IValueProvider valueProvider = _formCollection.ToValueProvider();
            ER_DML er_dml = new ER_DML();
            add addHelp = new add();
            DMLHelper DMLH = new DMLHelper();
            SecurityHelper SECH = new SecurityHelper();

            var verificationToken = _formCollection.Keys.Contains("currentRequestVerificationToken") ? _formCollection["currentRequestVerificationToken"].ToString() : "";
            var username = _formCollection.Keys.Contains("username") ? _formCollection["username"].ToString() : "";
            var email = _formCollection.Keys.Contains("emailaddress") ? _formCollection["emailaddress"].ToString() : "";
            var password = _formCollection.Keys.Contains("password") ? _formCollection["password"].ToString() : "";

            //Add Identity
            Values.AddIdentity thisIdentity = addHelp.ADD_ENTRY_Identities(_Connect, new Values.AddIdentity { I_OBJECT_TYPE = "Application User", I_USER_NAME = username, I_EDIPI = null, I_EMAIL = email });

            long? Identities_ID = thisIdentity.O_IDENTITIES_ID;

            //Add profile
            Values.AddProfiles ProfilesModel = null;
            ProfilesModel = addHelp.ADD_ENTRY_Profiles(_Connect, new Values.AddProfiles
            {
                I_IDENTITIES_ID = Identities_ID,
                I_ENABLED = 'Y',
                I_FIRST_NAME = "",
                I_MIDDLE_NAME = "",
                I_LAST_NAME = ""
            });

            //Generate Crypt Ticket before saving password
            addHelp.GEN_CRYPT_TICKET(_Connect, Identities_ID);
            //Add Password
            addHelp.ADD_ENTRY_Identities_Password(_Connect, new Values.AddIDPassword { I_IDENTITIES_ID = Identities_ID, I_RENDITION = 0, V_PASSWORD = password });

            return password;
        }

        public bool DoesEDIPIExist(IConnectToDB _Connect, string edipi)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "EDIPI_", DBType = SqlDbType.VarChar, ParamValue = edipi });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "EDIPI", length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            DataColumnCollection _dccColumnID = TempDataTable.Columns;

            if (TempDataTable.Rows.Count > 0 && _dccColumnID.Contains("EDIPI"))
                return true;
            else
                return false;
        }

        public DataTable FindAll(IConnectToDB _Connect)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();

            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_USER_NAME", ParamValue = "seed" });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public string validateCreditCard(string CCNumber)
        {
            CreditCardInfoModel CCIM = new CreditCardInfoModel();
            CreditCardInfoModel.CardType CT = CCIM.GetCardType(CCNumber);
            return CT.ToString();
        }

        public DataTable Find(IConnectToDB _Connect, string _id)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", ParamValue = _id });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public int GetCoreMembersCount(IConnectToDB _Connect, string _id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_ID_", DBType = SqlDbType.BigInt, ParamValue = _id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Count Query", "SP_S_VW_CORE_MEMBERS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable != null && TempDataTable.Rows.Count > 0 ? Convert.ToInt32(TempDataTable.Rows[0][0].ToString()) : 0;
        }

        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", ParamValue = _value });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, List<string> _value)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = _column, ParamValue = String.Join(",", _value) });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public DataTable FindIDProperties(IConnectToDB _Connect, long? identities_id)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "identities_id_", ParamValue = identities_id });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_IDENTITY_PROPERTIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public List<IdentityProperties> GETIDProperties(List<IdentityProperties> IDProperties, DataTable _DT)
        {
            foreach (DataRow GetIdentityPropertyRow in _DT.Rows)
            {
                IDProperties.Add(GETIDProperty(new IdentityProperties(), GetIdentityPropertyRow));
            }

            return IDProperties;
        }

        public IdentityProperties GETIDProperty(IdentityProperties _Property, DataRow _DR)
        {
            _Property.identity_properties_id = _DR.Field<long?>("identity_properties_id");
            _Property.enabled = _DR.Field<string>("enabled");
            _Property.dt_created = _DR.Field<DateTime>("dt_created");
            _Property.dt_updated = _DR.Field<DateTime>("dt_updated");
            _Property.dt_available = _DR.Field<DateTime?>("dt_available");
            _Property.dt_end = _DR.Field<DateTime?>("dt_end");
            _Property.property_type = _DR.Field<string>("property_type");
            _Property.property_name = _DR.Field<string>("property_name");

            return _Property;
        }

        private ViewIdentityModel GetID(ViewIdentityModel ViewID, DataRow _DR)
        {
            return ViewID = new ViewIdentityModel
            {
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                enabled = _DR.Field<string>("enabled"),
                active = _DR.Field<string>("active"),
                verified = _DR.Field<string>("verified"),
                identities_id = _DR.Field<long?>("identities_id"),
                user_name = _DR.Field<string>("user_name"),
                edipi = _DR.Field<string>("edipi"),
                email = _DR.Field<string>("email"),
                //object_layer = _DR.Field<string>("object_layer"),
                object_type = _DR.Field<string>("object_type"),
                // chardata = GetIdentityCharDataViaID((IConnectToDB)new ConnectToDB { Platform = platform, DBConnString = constrSQLServer2, SourceDBOwner = owner }, _DR.Field<long?>("identities_id").ToString()),
                // datedata = GetIdentityDateDataViaID((IConnectToDB)new ConnectToDB { Platform = platform, DBConnString = constrSQLServer2, SourceDBOwner = owner }, _DR.Field<long?>("identities_id").ToString()),
                // numbdata = GetIdentityNumbDataViaID((IConnectToDB)new ConnectToDB { Platform = platform, DBConnString = constrSQLServer2, SourceDBOwner = owner }, _DR.Field<long?>("identities_id").ToString())
            };
        }

        public IdentityObjects GetALLIDs(IdentityObjects IDs, IConnectToDB _Connect, DataTable _DT)
        {
            //ViewIdentityModel appObjects = new ViewIdentityModel();

            IdentityHelper identity = new IdentityHelper();

            IDs.Identity = new List<ViewIdentityModel>();

            //DataTable identitydt;

            //if (_id.ToLower() == "all")
            //{
            //    identitydt = identity.FindAll(_Connect);
            //}
            //else
            //{
            //    identitydt = identity.FindbyColumnID(_Connect, "identities_id", _id);
            //}



            //List<ViewIdentityModel> IdentitiesList = new List<ViewIdentityModel>();

            //ViewIdentityModel[] Identities = new ViewIdentityModel[identitydt.Rows.Count];

            int i = 0;
            foreach (DataRow idRow in _DT.Rows)
            {

                ViewIdentityModel tempID = GetID(new ViewIdentityModel(), idRow);

                //Identities[i] = new ViewIdentityModel
                //{

                //    identities_id = datarowdc.Field<Int32>("identities_id"),
                //    enabled = datarowdc.Field<string>("enabled"),
                //    dt_created = datarowdc.Field<DateTime>("dt_created"),
                //    dt_available = datarowdc.Field<DateTime?>("dt_available"),
                //    dt_end = datarowdc.Field<DateTime?>("dt_end"),
                //    object_type = datarowdc.Field<string>("object_type"),
                //    object_layer = datarowdc.Field<string>("object_layer"),
                //    chardata = GetIdentityCharDataViaID(_Connect, datarowdc.Field<Int32>("identities_id").ToString()), //Still need to build out Helpers
                //    datedata = GetIdentityDateDataViaID(_Connect, datarowdc.Field<Int32>("identities_id").ToString()), //Still need to build out Helpers
                //    numbdata = GetIdentityNumbDataViaID(_Connect, datarowdc.Field<Int32>("identities_id").ToString()) //Still need to build out Helpers

                //};
                //Grips[i].stage_name = datarowdc["stage_name"].ToString();


                IDs.Identity.Add(tempID);
                i++;
            }


            return IDs;
        }

        public List<ViewIdentityCharDataModel> GetIdentityCharDataViaID(IConnectToDB _Connect, string _id)
        {
            //ViewIdentityModel appObjects = new ViewIdentityModel();

            //Identity identity = new Identity();

            DataTable identitydt;

            if (_id.ToLower() == "all")
            {
                identitydt = GetAllCharData(_Connect);
            }
            else
            {
                identitydt = GetCharDataViaID(_Connect, _id);
            }

            List<ViewIdentityCharDataModel> IdentitiesList = new List<ViewIdentityCharDataModel>();

            ViewIdentityCharDataModel[] Identities = new ViewIdentityCharDataModel[identitydt.Rows.Count];

            int i = 0;
            foreach (DataRow datarowdc in identitydt.Rows)
            {

                Identities[i] = new ViewIdentityCharDataModel
                {

                    identities_id = datarowdc.Field<Int32>("identities_id"),
                    enabled = datarowdc.Field<string>("enabled"),
                    active = datarowdc.Field<string>("active"),
                    verified = datarowdc.Field<string>("verified"),
                    dt_created = datarowdc.Field<DateTime>("dt_created"),
                    dt_available = datarowdc.Field<DateTime?>("dt_available"),
                    dt_end = datarowdc.Field<DateTime?>("dt_end"),
                    object_type = datarowdc.Field<string>("object_type"),
                    applications_id = datarowdc.Field<Int32>("applications_id"),
                    containers_id = datarowdc.Field<Int32>("containers_id"),
                    grip_name = datarowdc.Field<string>("grip_name"),
                    grips_id = datarowdc.Field<Int32>("grips_id"),
                    has_child = datarowdc.Field<string>("has_child"),
                    has_parent = datarowdc.Field<string>("has_parent"),
                    identities_dat_char_id = datarowdc.Field<Int32>("identities_dat_char_id"),
                    obj_prop_sets_id = datarowdc.Field<Int32>("obj_prop_sets_id"),
                    object_prop_type = datarowdc.Field<string>("object_prop_type"),
                    object_sets_id = datarowdc.Field<Int32>("object_sets_id"),
                    parent_obj_prop_sets_id = datarowdc.Field<Int32>("parent_obj_prop_sets_id"),
                    property_name = datarowdc.Field<string>("property_name"),
                    property_value = datarowdc.Field<string>("property_value"),
                    stage_name = datarowdc.Field<string>("stage_name"),
                    stage_type = datarowdc.Field<string>("stage_type"),
                    stages_id = datarowdc.Field<Int32>("stages_id"),
                    value = datarowdc.Field<string>("value"),
                    value_datatype = datarowdc.Field<string>("value_datatype")


                };
                //Grips[i].stage_name = datarowdc["stage_name"].ToString();
                IdentitiesList.Add(Identities[i]);
                i++;
            }

            return IdentitiesList;
        }

        public List<ViewIdentityNumbDataModel> GetIdentityNumbDataViaID(IConnectToDB _Connect, string _id)
        {
            //ViewIdentityModel appObjects = new ViewIdentityModel();

            //Identity identity = new Identity();

            DataTable identitydt;

            if (_id.ToLower() == "all")
            {
                identitydt = GetAllNumbData(_Connect);
            }
            else
            {
                identitydt = GetNumbDataViaID(_Connect, _id);
            }

            List<ViewIdentityNumbDataModel> IdentitiesList = new List<ViewIdentityNumbDataModel>();

            ViewIdentityNumbDataModel[] Identities = new ViewIdentityNumbDataModel[identitydt.Rows.Count];

            int i = 0;
            foreach (DataRow datarowdc in identitydt.Rows)
            {

                Identities[i] = new ViewIdentityNumbDataModel
                {

                    identities_id = datarowdc.Field<Int32>("identities_id"),
                    enabled = datarowdc.Field<string>("enabled"),
                    active = datarowdc.Field<string>("active"),
                    verified = datarowdc.Field<string>("verified"),
                    dt_created = datarowdc.Field<DateTime>("dt_created"),
                    dt_available = datarowdc.Field<DateTime?>("dt_available"),
                    dt_end = datarowdc.Field<DateTime?>("dt_end"),
                    object_type = datarowdc.Field<string>("object_type"),
                    applications_id = datarowdc.Field<Int32>("applications_id"),
                    containers_id = datarowdc.Field<Int32>("containers_id"),
                    grip_name = datarowdc.Field<string>("grip_name"),
                    grips_id = datarowdc.Field<Int32>("grips_id"),
                    has_child = datarowdc.Field<string>("has_child"),
                    has_parent = datarowdc.Field<string>("has_prent"),
                    identities_dat_numb_id = datarowdc.Field<Int32>("identities_dat_numb_id"),
                    obj_prop_sets_id = datarowdc.Field<Int32>("obj_prop_sets_id"),
                    object_prop_type = datarowdc.Field<string>("object_prop_type"),
                    object_sets_id = datarowdc.Field<Int32>("object_sets_id"),
                    parent_obj_prop_sets_id = datarowdc.Field<Int32>("parent_obj_prop_sets_id"),
                    property_name = datarowdc.Field<string>("property_name"),
                    property_value = datarowdc.Field<string>("property_value"),
                    stage_name = datarowdc.Field<string>("stage_name"),
                    stage_type = datarowdc.Field<string>("stage_type"),
                    stages_id = datarowdc.Field<Int32>("stages_id"),
                    value = datarowdc.Field<Int32>("value"),
                    value_datatype = datarowdc.Field<string>("string")


                };
                //Grips[i].stage_name = datarowdc["stage_name"].ToString();
                IdentitiesList.Add(Identities[i]);
                i++;
            }

            return IdentitiesList;
        }

        public List<ViewIdentityDateDataModel> GetIdentityDateDataViaID(IConnectToDB _Connect, string _id)
        {
            //ViewIdentityModel appObjects = new ViewIdentityModel();

            // Identity identity = new Identity();

            DataTable identitydt;

            if (_id.ToLower() == "all")
            {
                identitydt = GetAllDateData(_Connect);
            }
            else
            {
                identitydt = GetDateDataViaID(_Connect, _id);
            }

            List<ViewIdentityDateDataModel> IdentitiesList = new List<ViewIdentityDateDataModel>();

            ViewIdentityDateDataModel[] Identities = new ViewIdentityDateDataModel[identitydt.Rows.Count];

            int i = 0;
            foreach (DataRow datarowdc in identitydt.Rows)
            {

                Identities[i] = new ViewIdentityDateDataModel
                {

                    identities_id = datarowdc.Field<Int32>("identities_id"),
                    enabled = datarowdc.Field<string>("enabled"),
                    active = datarowdc.Field<string>("active"),
                    verified = datarowdc.Field<string>("verified"),
                    dt_created = datarowdc.Field<DateTime>("dt_created"),
                    dt_available = datarowdc.Field<DateTime?>("dt_available"),
                    dt_end = datarowdc.Field<DateTime?>("dt_end"),
                    object_type = datarowdc.Field<string>("object_type"),
                    applications_id = datarowdc.Field<Int32>("applications_id"),
                    containers_id = datarowdc.Field<Int32>("containers_id"),
                    grip_name = datarowdc.Field<string>("grip_name"),
                    grips_id = datarowdc.Field<Int32>("grips_id"),
                    has_child = datarowdc.Field<string>("has_child"),
                    has_parent = datarowdc.Field<string>("has_prent"),
                    identities_dat_date_id = datarowdc.Field<Int32>("identities_dat_date_id"),
                    obj_prop_sets_id = datarowdc.Field<Int32>("obj_prop_sets_id"),
                    object_prop_type = datarowdc.Field<string>("object_prop_type"),
                    object_sets_id = datarowdc.Field<Int32>("object_sets_id"),
                    parent_obj_prop_sets_id = datarowdc.Field<Int32>("parent_obj_prop_sets_id"),
                    property_name = datarowdc.Field<string>("property_name"),
                    property_value = datarowdc.Field<string>("property_value"),
                    stage_name = datarowdc.Field<string>("stage_name"),
                    stage_type = datarowdc.Field<string>("stage_type"),
                    stages_id = datarowdc.Field<Int32>("stages_id"),
                    value = datarowdc.Field<DateTime?>("value"),
                    value_datatype = datarowdc.Field<string>("string")


                };
                //Grips[i].stage_name = datarowdc["stage_name"].ToString();
                IdentitiesList.Add(Identities[i]);
                i++;
            }

            return IdentitiesList;
        }

        public DataTable GetCharDataViaID(IConnectToDB _Connect, string _id)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "identities_id_", ParamValue = _id });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_DAT_CHAR_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public DataTable GetAllCharData(IConnectToDB _Connect)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_DAT_CHAR_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public DataTable GetNumbDataViaID(IConnectToDB _Connect, string _id)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "identities_id_", ParamValue = _id });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_DAT_NUMB_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public DataTable GetAllNumbData(IConnectToDB _Connect)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_DAT_NUMB_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public DataTable GetDateDataViaID(IConnectToDB _Connect, string _id)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "identities_id_", ParamValue = _id });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_DAT_DATE_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public DataTable GetAllDateData(IConnectToDB _Connect)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_DAT_DATE_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public DataTable GetAllUserNames(IConnectToDB _Connect)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_USER_NAME", ParamValue = "seed" });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return result;
        }

        public DataTable GetCoreUserNames(IConnectToDB _Connect, string cores_id)
        {
            List<DynamicModels.RootReportFilter> queryFilters = new List<DynamicModels.RootReportFilter>();
            queryFilters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_USER_NAME", ParamValue = "seed" });
            queryFilters.Add(new DynamicModels.RootReportFilter { FilterName = "CORES_UUID_", ParamValue = cores_id });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "USER_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                queryFilters);

            return result;
        }


        public string GetUserName(IConnectToDB _Connect, long? identities_id)
        {
            string value = "";

            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_USER_NAME", ParamValue = "seed" });
            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", ParamValue = identities_id });

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "USER_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            if (TempDataTable.Rows.Count > 0)
                value = TempDataTable.Rows[0]["USER_NAME"].ToString();

            return value;
        }

        public string GetIdentityFullName(IConnectToDB _Connect, long? identities_id)
        {
            ProfileHelper ph = new ProfileHelper();
            string name = ph.GetProfileData(_Connect, identities_id, "First Name") + ph.GetProfileData(_Connect, identities_id, "Last Name");

            return name;
        }

        public DataTable GetIdentitiesCores(IConnectToDB _Connect, string id)
        {
            List<DynamicModels.RootReportFilter> queryFilters = new List<DynamicModels.RootReportFilter>();
            queryFilters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", ParamValue = id });
            queryFilters.Add(new DynamicModels.RootReportFilter { FilterName = "EXCLUDE_CORES_ID", ParamValue = "1000" });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CORES_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { columns = "CORE_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                queryFilters);

            return result;
        }

        public DataTable FindUsersRecentlyAccessedCores(IConnectToDB _Connect, string identities_id)
        {
            List<DynamicModels.RootReportFilter> queryFilters = new List<DynamicModels.RootReportFilter>();
            queryFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PROPERTY_TYPE", ParamValue = "coreaccessed" });

            DataTable result = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_IDENTITY_PROPERTIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED desc", start = 0, verify = "T" },
                queryFilters);

            return result;
        }

        public void SetObjectAccessedByUser(IConnectToDB _Connect, string type, string identities_id, string object_id)
        {
            add addHelp = new add();
            switch (type.ToLower())
            {
                case "core":
                    //Store the fact that the user accessed a core
                    Values.AddIdentityProperties thisUserProp = addHelp.ADD_ENTRY_Identity_Properties(_Connect,
                    new Values.AddIdentityProperties
                    {
                        I_IDENTITIES_ID = ER_Tools.ConvertToInt64(identities_id),
                        I_PROPERTY_NAME = "coreaccessed",
                        I_PROPERTY_TYPE = object_id
                    });

                    break;
                case "forms":
                    //Store the fact that the user accessed a form
                    Values.AddIdentityProperties thisUserProp2 = addHelp.ADD_ENTRY_Identity_Properties(_Connect,
                    new Values.AddIdentityProperties
                    {
                        I_IDENTITIES_ID = ER_Tools.ConvertToInt64(identities_id),
                        I_PROPERTY_NAME = "formaccessed",
                        I_PROPERTY_TYPE = object_id
                    });
                    break;
            }



        }

        public ViewIdentityRolesModel GetIdentityRole(ViewIdentityRolesModel IdentityRoles, DataRow _DR)
        {
            IdentityRoles = new ViewIdentityRolesModel
            {
                enabled = _DR.Field<string>("enabled"),
                role_name = _DR.Field<string>("role_name"),
                user_name = _DR.Field<string>("user_name"),
                email = _DR.Field<string>("email"),
                edipi = _DR.Field<string>("edipi"),
                identities_roles_id = _DR.Field<long?>("identities_roles_id"),
                roles_id = _DR.Field<long?>("roles_id"),
                core_name = _DR.Field<string>("core_name"),
                object_type = _DR.Field<string>("object_type"),
                object_layer = _DR.Field<string>("object_layer"),
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end")
            };

            //if (IdentityRoles.role_name.ToUpper() == "OWNERS")
            //    IdentityRoles.role_name = "COLLECTION ADMIN";

            //if (IdentityRoles.role_name.ToUpper() == "OBJECT OWNER")
            //    IdentityRoles.role_name = "DESIGNER";

            if (IdentityRoles.core_name.ToUpper() == "REVAMP SYSTEM")
                IdentityRoles.core_name = "Templates";

            return IdentityRoles;
        }

        public List<ViewIdentityRolesModel> GetIdentityRoles(List<ViewIdentityRolesModel> IdentityRoles, DataTable _DT)
        {
            foreach (DataRow GetIdentityRow in _DT.Rows)
            {
                //if (new[] { "AUDITOR", "CREATOR" }.Contains(GetIdentityRow.Field<string>("role_name").ToUpper()))
                //    continue;

                IdentityRoles.Add(GetIdentityRole(new ViewIdentityRolesModel(), GetIdentityRow));
            }

            return IdentityRoles;
        }

        public ArrayList GetRolesNotAssigned(IConnectToDB _Connect, Guid? identitiesUUID, Guid? coresUUID)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();
            ER_DML er_dml = new ER_DML();
            CoreHelper coreHelper = new CoreHelper();
            IdentityHelper identityHelper = new IdentityHelper();

            //long? identitiesId = identityHelper.GetIdentityID(_Connect, identitiesUUID.ToString());
            //long? coresId = coreHelper.GetCoreID(_Connect, coresUUID.ToString());

            //SQlin._dbParameters = new List<DBParameters>
            //    {
            //        new DBParameters { ParamName = "CORES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = coresId },
            //        new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = identitiesId },
            //    };
            //SQlin.sqlIn = "SELECT ROLE_NAME, ROLES_UUID FROM CSA.VW__ROLES t1 WHERE t1.cores_id = @CORES_ID and enabled = 'Y' and roles_id not in (select roles_id from " +
            //   "CSA.VW__IDENTITIES_ROLES where IDENTITIES_ID = @IDENTITIES_ID) ";

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "CORES_UUID", ParamValue = coresUUID },
                new DBParameters { ParamName = "IDENTITIES_UUID", ParamValue = identitiesUUID },
            };

            SQlin.sqlIn = "SELECT ROLE_NAME, ROLES_UUID, CORES_UUID FROM CSA.VW__ROLES t1 WHERE t1.cores_uuid = @CORES_UUID and t1.enabled = 'Y' and roles_id not in (select roles_id from " +
                "CSA.VW__IDENTITIES_ROLES where IDENTITIES_UUID = @IDENTITIES_UUID AND ENABLED = 'Y') ";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            ArrayList list = new ArrayList();
            Int64 numRows = TempDataTable.Rows.Count;

            for (int current = 0; current < numRows; current++)
            {
                Dictionary<string, string> myDict = new Dictionary<string, string>();
                bool skip = false;
                Int64 numCols = TempDataTable.Columns.Count;

                for (int currentCol = 0; currentCol < numCols; currentCol++)
                {
                    //if (new[] { "AUDITOR", "CREATOR", "", null }.Contains(TempDataTable.Rows[current][TempDataTable.Columns[0]].ToString().ToUpper()))
                    //{
                    //    skip = true;
                    //    break;
                    //}

                    var columnValue = TempDataTable.Columns[currentCol].ToString().ToLower();
                    var rowValue = TempDataTable.Rows[current][TempDataTable.Columns[currentCol]].ToString().ToUpper();

                    //switch (rowValue)
                    //{
                    //    case "OWNERS":
                    //        rowValue = "COLLECTION ADMIN";
                    //        break;
                    //    case "OBJECT OWNER":
                    //        rowValue = "DESIGNER";
                    //        break;
                    //}

                    myDict.Add(columnValue, rowValue);
                }
                if (!skip)
                {
                    list.Add(myDict);
                }
            }

            return list;
        }

        public string[] GetRolesNotAssigned(IConnectToDB _Connect, string identitiesId, string coresid)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();
            ER_DML er_dml = new ER_DML();

            //SQlin._dbParameters = new List<DBParameters>
            //    {
            //        new DBParameters { ParamName = "CORES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = coresid },
            //        new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = identitiesId },
            //    };
            //SQlin.sqlIn = "SELECT ROLE_NAME FROM CSA.VW__ROLES t1 WHERE t1.cores_id = @CORES_ID and enabled = 'Y' and roles_id not in (select roles_id from " +
            //   "CSA.VW__IDENTITIES_ROLES where IDENTITIES_ID = @IDENTITIES_ID) ";

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "CORES_UUID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = coresid },
                new DBParameters { ParamName = "IDENTITIES_UUID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = identitiesId },
            };

            SQlin.sqlIn = "SELECT ROLE_NAME, ROLES_UUID FROM CSA.VW__ROLES t1 WHERE t1.cores_uuid = @CORES_UUID and enabled = 'Y' and roles_id not in (select roles_id from " +
                "CSA.VW__IDENTITIES_ROLES where IDENTITIES_UUID = @IDENTITIES_UUID) ";

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            string[] roles = null;

            if (TempDataTable.Rows.Count >= 1)
            {
                //int s = 0;
                roles = new string[TempDataTable.Rows.Count];
                for (int i = 0; i < TempDataTable.Rows.Count; i++)
                {
                    //if (new[] { "AUDITOR", "CREATOR", "", null }.Contains(TempDataTable.Rows[i][0].ToString().ToUpper()))
                    //{
                    //    if (i > 0)
                    //        s = i - 1;
                    //    continue;
                    //}

                    roles[i] = "";
                    roles[i] = TempDataTable.Rows[i][0].ToString();

                    //if (TempDataTable.Rows[i][0].ToString().ToUpper() == "OWNERS")
                    //    roles[s] = "COLLECTION ADMIN";

                    //if (TempDataTable.Rows[i][0].ToString().ToUpper() == "OBJECT OWNER")
                    //    roles[s] = "DESIGNER";

                    //s++;
                }
            }
            else
            {
                roles = new string[1] { "Roles not available" };
            }

            roles = roles.Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();

            return roles;
        }

        public ViewCoresIdentitiesModel GetCoreIdentity(ViewCoresIdentitiesModel CoreIdentities, DataRow _DR)
        {
            CoreIdentities = new ViewCoresIdentitiesModel
            {
                enabled = _DR.Field<string>("enabled"),

                core_name = _DR.Field<string>("core_name"),
                user_name = _DR.Field<string>("user_name"),
                email = _DR.Field<string>("email"),
                edipi = _DR.Field<string>("edipi"),
                cores_id = _DR.Field<long?>("cores_id"),
                object_type = _DR.Field<string>("object_type"),
                object_layer = _DR.Field<string>("object_layer"),
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end")
            };

            try
            {
                CoreIdentities.active = _DR.Field<string>("active");
                CoreIdentities.verified = _DR.Field<string>("verified");
                CoreIdentities.cores_identities_id = _DR.Field<long?>("cores_identities_id");
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }

            return CoreIdentities;
        }

        public List<ViewCoresIdentitiesModel> GetCoreIdentities(List<ViewCoresIdentitiesModel> CoreIdentities, DataTable _DT)
        {
            foreach (DataRow GetCoreIdentityRow in _DT.Rows)
            {
                CoreIdentities.Add(GetCoreIdentity(new ViewCoresIdentitiesModel(), GetCoreIdentityRow));
            }

            return CoreIdentities;
        }

        public bool SendFormConfirmEmailtoIdentity(IConnectToDB _Connect, long? appid, string appname, long? identities_id, string sendConfirm, string confirmMsg, SMTPModel _configSettings)
        {
            ER_Query er_query = new ER_Query();
            IOHelper ioHelper = new IOHelper();
            string user_email = "";
            string EmailBody = "";
            string EmailSubject = "";
            
            string Host = Current.Request.Scheme + "://" + Current.Request.Host.ToString().ToLower();
            if (Current.Request.Host.Port != 80 &&
                Current.Request.Host.Port != 443)
            { Host = Host + ":" + Current.Request.Host.Port; }

            if (sendConfirm.ToUpper() == "ON")
            {
                //Check if user belongs to application                
                DataTable _Users;
                //foreach (DataRow _AppRow in _AppRows.Rows)
                //{
                //Get users email
                _Users = Find(_Connect, identities_id.ToString());
                foreach (DataRow _User in _Users.Rows)
                {
                    user_email = _User.Field<String>("Email");
                }

                EmailSubject = "Revamp Form Submission Confirmation from " + appname;

                if (confirmMsg == "")
                {
                    EmailBody = "Your form has been submitted from " + appname + "." + "<a href='" + Host + "/forms/dashboard/" + appid + "'>Click here</a> to view it.";
                }
                else
                {
                    EmailBody = confirmMsg + "<br /><br /><a href='" + Host + "/forms/dashboard/" + appid + "'>Click here</a> to view it.";
                }

                try
                {
                    //ioHelper.SendEmail(user_email, "NO REPLY: Revamp invite message", EmailBody);
                    //return true;

                    System.Net.Mail.MailMessage emailStruct = new System.Net.Mail.MailMessage();

                    emailStruct.From = new System.Net.Mail.MailAddress("noreply@revamp.io", "Revamp IO");
                    emailStruct.To.Add(new System.Net.Mail.MailAddress(user_email, _Connect.SourceDBOwner.ToUpper()));
                    emailStruct.Subject = EmailSubject;
                    emailStruct.IsBodyHtml = true;
                    emailStruct.Body = EmailBody;

                    System.Net.Mail.SmtpClient SmtpServerConfig = ioHelper.SMTPConfigSettings(new System.Net.Mail.SmtpClient(), _configSettings);

                    //Send email
                    ER_Mail.MailSendStatus EmailResult = ER_Mail.SendEmail(emailStruct, SmtpServerConfig);

                    switch (EmailResult.ToString())
                    {
                        case "Sent":
                            return true;
                        case "None":
                        case "ErrorCannotSend":
                        case "TryAgain":
                        case "SentMaybe":
                        default:
                            return false;
                    }


                }
                catch
                {
                    //Could not send email
                    return false;
                }
                // }                  
            }
            else
            {
                return false;
            }
        }

        public bool SendFormSubmitEmailtoIdentities(IConnectToDB _Connect, long? appid, string appname, SMTPModel _configSettings)
        {
            ER_Query er_query = new ER_Query();
            IOHelper ioHelper = new IOHelper();
            string enabled = "";
            string user_email = "";
            string EmailBody = "";
            string EmailSubject = "";
            string identities_id = "";

            string Host = Current.Request.Scheme + "://" + Current.Request.Host.ToString().ToLower();
            if (Current.Request.Host.Port != 80 &&
                Current.Request.Host.Port != 443)
            { Host = Host + ":" + Current.Request.Host.Port; }

            //Check if user is subscribed to application
            List<DynamicModels.RootReportFilter> queryFilters = new List<DynamicModels.RootReportFilter>();
            queryFilters.Add(new DynamicModels.RootReportFilter { FilterName = "PROPERTY_TYPE", ParamValue = appid });
            queryFilters.Add(new DynamicModels.RootReportFilter { FilterName = "property_name", ParamValue = "subscribetoapp" });

            DataTable _AppRows = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_IDENTITY_PROPERTIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "DT_CREATED desc", start = 0, verify = "T" },
                queryFilters);
            DataTable _Users;
            foreach (DataRow _AppRow in _AppRows.Rows)
            {
                enabled = _AppRow.Field<String>("Enabled");
                identities_id = Convert.ToString(_AppRow.Field<Int32>("IDENTITIES_ID"));

                //If the user is subscribed to this application. Send an email
                if (enabled == "Y")
                {
                    //Get users email
                    _Users = Find(_Connect, identities_id.ToString());
                    foreach (DataRow _User in _Users.Rows)
                    {
                        user_email = _User.Field<String>("Email");
                    }

                    //Send email
                    EmailBody = "A form has been submitted from " + appname + "." + "<a href='" + Host + "/forms/dashboard/" + appid + "'>Click here</a> to view it.";
                    EmailSubject = "Revamp Form Submission from " + appname;

                    try
                    {
                        System.Net.Mail.MailMessage emailStruct = new System.Net.Mail.MailMessage();

                        emailStruct.From = new System.Net.Mail.MailAddress("noreply@revamp.io", "Revamp IO");
                        emailStruct.To.Add(new System.Net.Mail.MailAddress(user_email, _Connect.SourceDBOwner.ToUpper()));
                        emailStruct.Subject = EmailSubject;
                        emailStruct.IsBodyHtml = true;
                        emailStruct.Body = EmailBody;

                        System.Net.Mail.SmtpClient SmtpServerConfig = ioHelper.SMTPConfigSettings(new System.Net.Mail.SmtpClient(), _configSettings);

                        ER_Mail.MailSendStatus EmailResult = ER_Mail.SendEmail(emailStruct, SmtpServerConfig);

                        switch (EmailResult.ToString())
                        {
                            case "Sent":
                                return true;
                            case "None":
                            case "ErrorCannotSend":
                            case "TryAgain":
                            case "SentMaybe":
                            default:
                                return false;
                        }

                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public bool SendAlreadyVerifiedEmail(IConnectToDB _Connect, long? identities_id, string tempemail)
        {
            string Host = Current.Request.Scheme + "://" + Current.Request.Host.ToString().ToLower();
            if (Current.Request.Host.Port != 80 &&
               Current.Request.Host.Port != 443)
            { Host = Host + ":" + Current.Request.Host.Port; }

            string EmailBody = "A request to activate your account was recently made.<br />" +
                "If this was accidental please disregard this message.<br />" +
                "If this was you and you've forgotten your password go to " +
                "<a href='" + Host + "/ForgotPassword'>Forgot Password</a><br /><br />" +
                "Thanks for using our System!<br />";

            string EmailSubject = "No Reply: Activate your " + _Connect.SourceDBOwner.ToUpper() + " account ";
            IOHelper ioHelper = new IOHelper();
            ioHelper.SendEmail(tempemail, EmailSubject, EmailBody);

            return true;
        }

        public bool SendVerificationEmailtoIdentity(IConnectToDB _Connect, long? identities_id, string tempemail)
        {
            IdentityHelper IH = new IdentityHelper();
            string VerifyUUID = "";
            add addHelp = new add();
            DataTable verifyTable = IH.GetIdentityUUID(_Connect, identities_id);

            VerificationHelper VH = new VerificationHelper();
            VH.DisableVerificationsForID(_Connect, identities_id, "CreateUser");
            Values.AddVerify thisVerify = addHelp.ADD_ENTRY_Verify(_Connect, new Values.AddVerify { I_IDENTITIES_ID = identities_id, I_VALIDATION_TYPE = "CreateUser" });
            VerifyUUID = thisVerify.I_UUID;

            string Host = Current.Request.Scheme + "://" + Current.Request.Host.Host.ToString().ToLower();
            if (Current.Request.Host.Port != 80 &&
                Current.Request.Host.Port != 443)
            { Host = Host + ":" + Current.Request.Host.Port; }

            string VerifyURL = Host + "/Verify/Account/" + VerifyUUID;

            var codechar = VerifyUUID.ToCharArray(5, 3);
            string verifycode = "";
            for (int i = 0; i < codechar.Length; i++)
            {
                verifycode = verifycode + Convert.ToInt32(codechar[i]);
            }

            string EmailBody = "Action required: Please verify your email address. You will not be able to access Revamp until verification is complete." +
                "<br><br>This link will be active for the next 15 minutes!<br /><br />" +
                "<br><br><b>Your Registration Code: " + verifycode + "</b><br /><br />" +
                "All you need to do is click the link below and enter the registration code shown above (it only takes a few seconds). You will then be able to access your Revamp account – we're simply verifying ownership of this email address." +
                "<br /><br />" +
                "<br /><br />" +
                "Please click <a href='" + VerifyURL + "'>here</a> to activate your account." +
                "<br /><br />" +
                "<br /><br />" +
                // owner + " Verification URL: " +
                // "<br /><br />" +
                // VerifyURL +
                // "<br /><br />"+
                "Please do not reply to this message." +
                "<br /><br /><br /><br />" +
                "Thanks for using Revamp!";

            string EmailSubject = "No Reply: Activate your " + _Connect.SourceDBOwner.ToUpper() + " account ";

            IOHelper ioHelper = new IOHelper();
            //try
            //{
            //    result = ioHelper.SendEmail(tempemail, EmailSubject, EmailBody);
            //    return true;
            //}
            //catch (Exception e)
            //{
            //    return false;
            //}

            if (true)
            {
                try
                {
                    ioHelper.SendEmail(tempemail, EmailSubject, EmailBody);

                    return true;
                    /*System.Net.Mail.MailMessage emailStruct = new System.Net.Mail.MailMessage();

                    emailStruct.From = new System.Net.Mail.MailAddress("noreply@revamp.io", "Revamp IO");
                    emailStruct.To.Add(new System.Net.Mail.MailAddress(tempemail, _Connect.SourceDBOwner.ToUpper()));
                    //emailStruct.To.Add(new System.Net.Mail.MailAddress("risi@emin-it.com", "CEO Risi"));
                    //emailStruct.To.Add(new System.Net.Mail.MailAddress("isaac@emin-it.com", "COO Isaac"));
                    emailStruct.Subject = EmailSubject;
                    emailStruct.IsBodyHtml = true;
                    emailStruct.Body = EmailBody;

                    System.Net.Mail.SmtpClient SmtpServerConfig = ioHelper.SMTPConfigSettings(new System.Net.Mail.SmtpClient(), _configSettings);


                    ER_Mail.MailSendStatus EmailResult = ER_Mail.SendEmail(emailStruct, SmtpServerConfig);

                    switch (EmailResult.ToString())
                    {
                        case "Sent":
                            return true;
                        case "None":
                        case "ErrorCannotSend":
                        case "TryAgain":
                        case "SentMaybe":
                        default:
                            return false;
                    }*/
                }
                catch
                {
                    return false;
                }
            }
        }

        public DataTable FindIdentity(IConnectToDB _Connect, string _username)
        {
            List<DynamicModels.RootReportFilter> usernamesFilters = new List<DynamicModels.RootReportFilter>();

            usernamesFilters.Add(new DynamicModels.RootReportFilter { FilterName = "WHERE", ParamValue = "LOWER(User_name) = '" + _username.ToLower() + "' or LOWER(Email) = '" + _username.ToLower() + "'" });

            DataTable usernamedt = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_IDENTITIES_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                usernamesFilters);

            return usernamedt;
        }

        public DataTable GetIdentityUUID(IConnectToDB _Connect, long? _id)
        {
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_ID_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _id });

            DataTable TempDataTable = _DynamicOutputProcedures._DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__VERIFY_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }
        public long? GetIdentityID(IConnectToDB _Connect, string uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable;
            long? id = 0;

            if (!string.IsNullOrWhiteSpace(uuid))
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "IDENTITIES_UUID_", ParamValue = uuid });

                TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_" + "IDENTITIES" + "_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "IDENTITIES_ID", length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                DataColumnCollection _dccColumnID = TempDataTable.Columns;

                if (_dccColumnID.Contains("IDENTITIES_ID") && TempDataTable.Rows.Count > 0)
                {
                    id = TempDataTable.Rows[0].Field<long?>("IDENTITIES_ID");
                }
            }

            return id;
        }
        public string GetGroupName(IConnectToDB _Connect, string uuid)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();
            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();
            DataTable TempDataTable;
            string name = "";

            if (!string.IsNullOrWhiteSpace(uuid))
            {
                Filters.Add(new DynamicModels.RootReportFilter { FilterName = "GROUPS_UUID_", ParamValue = uuid });

                TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_" + "GROUPS" + "_SEARCH",
                    new DataTableDotNetModelMetaData { columns = "GROUP_NAME", length = -1, order = "1 asc", start = 0, verify = "T" },
                    Filters);

                DataColumnCollection _dccColumnID = TempDataTable.Columns;

                if (_dccColumnID.Contains("GROUP_NAME") && TempDataTable.Rows.Count > 0)
                {
                    name = TempDataTable.Rows[0].Field<string>("GROUP_NAME");
                }
            }

            return name;
        }
        public string UpdateIdentity(IConnectToDB _Connect, long? identitiesId, string objectType, string userName, string edipi, string email, string active, string verified)
        {
            add addHelp = new add();
            long? result = 0;

            Values.UpdateIdentity IdentitiesModel = null;
            IdentitiesModel = addHelp.UPDATE_ENTRY_Identities(_Connect, new Values.UpdateIdentity
            {
                I_IDENTITIES_ID = identitiesId,
                I_OBJECT_TYPE = objectType,
                I_USER_NAME = userName,
                I_EDIPI = edipi,
                I_EMAIL = email,
                I_ACTIVE = active,
                I_VERIFIED = verified,
            });
            result = IdentitiesModel.O_IDENTITIES_ID;

            return result.ToString();
        }
    }
}

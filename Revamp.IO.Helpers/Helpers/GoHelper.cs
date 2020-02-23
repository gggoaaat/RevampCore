using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;

namespace Revamp.IO.Helpers.Helpers
{
    public class GoHelper
    {
        public SessionObjects handleSession(IConnectToDB _Connect, SessionObjects SO, string _desiredstate, string username, bool ValidSession, string _JsonTimeoutInterval, string _MemoryTracking, string _InstallType)
        {

            //_SessionObjects.SessionIdentity.username = username;
            //_SessionObjects.SessionIdentity.password = password;
            Boolean _userIsValid = ValidSession; //Create an is user valid method.

            Int64 JsonTimeoutInterval = 15;

            try
            {
                JsonTimeoutInterval = Convert.ToInt64(_JsonTimeoutInterval);

                JsonTimeoutInterval = JsonTimeoutInterval > 0 ? JsonTimeoutInterval : 15;
            }
            catch
            {
                JsonTimeoutInterval = 15;
            }

            switch (_desiredstate.ToLower())
            {
                case "new":
                case "preserve":
                    if (SO != null || SO == null)
                    {
                        try
                        {

                            if (SO.SessionStartTime != null)
                            { //No need to reset Starttime, just check last load time.                        
                                SO.LastLoad = DateTime.Now;
                                SO.SessionEndTime = DateTime.Now.AddMinutes(JsonTimeoutInterval);
                            }
                            else
                            {
                                SO = new SessionObjects();
                                SO.LastLoad = DateTime.Now;
                                SO.SessionStartTime = DateTime.Now;
                                SO.SessionEndTime = DateTime.Now.AddMinutes(JsonTimeoutInterval);
                            }
                        }
                        catch
                        {
                            SO = new SessionObjects();
                            SO.SessionStartTime = DateTime.Now;
                            SO.LastLoad = DateTime.Now;
                            SO.SessionEndTime = DateTime.Now.AddMinutes(JsonTimeoutInterval);
                        }

                        long? tempUserID = 0;
                        //If user is valid then proceed with loading the first core.
                        if (_userIsValid)
                        {
                            //Hardcoded user for now

                            //Instantiate Sub Model of Session.
                            SO.SessionStartTime = DateTime.Now;

                            if (SO.NextReload == null || SO.NextReload < DateTime.Now || _desiredstate.ToLower() == "new")
                            {
                                SO.SessionIdentity = new IdentityModels();

                                SO.SessionIdentity.Identity = new IdentityModel();

                                SessionsHelper SH = new SessionsHelper();
                                SecurityHelper SECH = new SecurityHelper();

                                
                                DataTable thisUser = SH.GetIDviaUserName(_Connect, username);

                                SO.SessionIdentity.Identity.username = username;
                                SO.SessionIdentity.Identity.user_name = username;
                                SO.SessionIdentity.Identity.identities_id = thisUser.Rows[0].Field<long?>("IDENTITIES_ID");
                                SO.SessionIdentity.Identity.identities_uuid = thisUser.Rows[0].Field<Guid?>("BASE_Identities_UUID");
                                SO._IdentityModel = new IdentityModel();
                                SO._IdentityModel.username = username;
                                SO._IdentityModel.user_name = username;
                                SO._IdentityModel.identities_id = SO.SessionIdentity.Identity.identities_id;
                                SO._IdentityModel.identities_uuid = SO.SessionIdentity.Identity.identities_uuid;

                                tempUserID = SO.SessionIdentity.Identity.identities_id;

                             //   SO.SessionIdentity.ApplicationPermissions = SECH.SourceSecPermList(new List<ViewSourceSecPerm>(), SECH.FindbyColumnID(_Connect, "identities_id", SO.SessionIdentity.Identity.identities_id.ToString(), "appaccess"), "Applications");

                                //ProfileHelper PH = new ProfileHelper();
                                // SO.SessionIdentity.Profile = new ViewProfileModel();
                                // SO.SessionIdentity.Profile.profile_image = PH.GetProfileImage(SO.SessionIdentity.Identity.identities_id);
                            }

                            DictionaryHelper DH = new DictionaryHelper();
                            ActivityHelper _AH = new ActivityHelper();

                            if ((SO.NextReload == null || SO.NextReload < DateTime.Now || _desiredstate.ToLower() == "new")
                                && new SecurityHelper().DoesIdentityHavePrivilege(_Connect, tempUserID.ToString(), "VIEW DICTIONARY"))
                            {
                                SO.ERDictionary = new ERDictionaryModels();

                                //SO.ERDictionary.tablesView = DH.GetTables(new List<ViewTablesDictionary>(), DH.FindAll(_Connect, "table"));
                                //SO.ERDictionary.ukeysViews = DH.GetUKeys(new List<ViewUniqueKeysDictionary>(), DH.FindAll(_Connect, "ukeys"));
                                //SO.ERDictionary.fkeysViews = DH.GetFKeys(new List<ViewForeignKeysDictionary>(), DH.FindAll(_Connect, "fkeys"));
                                //SO.ERDictionary.viewsView = DH.GetViews(new List<ViewViewsDictionary>(), DH.FindAll(_Connect, "views"));
                                //SO.ERDictionary.pkeysViews = DH.GetPKeys(new List<ViewPrimaryKeysDictionary>(), DH.FindAll(_Connect, "pkeys"));
                                //SO.ERDictionary.indexes = DH.GetIndexes(new List<ViewIndexesDictionary>(), DH.FindAll(_Connect, "indexes"));
                            }

                            if (SO.NextReload == null || SO.NextReload < DateTime.Now || _desiredstate.ToLower() == "new")
                            {
                                SO.ActivityList = new Activity();

                                SO.ActivityList.ActivityView = _AH.ActivityList(new List<ActivityModelView>(), _AH.FindAllbyIDforDays(_Connect, "All", SO.SessionIdentity.Identity.identities_id, 7));
                                SO.ActivityList.ActivityViewTitle = "My Personal Activity";
                            }

                            //_SessionObjects.SessionIdentity.cores = GetsCoreForUserID(new List<CoreModels>(), _SessionObjects.SessionIdentity.identities_id);



                            if (SO.NextReload == null || SO.NextReload < DateTime.Now || _desiredstate.ToLower() == "new")
                            {
                                //  SO.SessionNotification = GetNotifications(_Connect, new NotificationsModel(), SO.SessionIdentity.Identity.identities_id);
                                SO.SessionNotification = new NotificationsModel();
                            }

                            SO.Notifications = new Activity();
                            SO.Notifications.newActivityCount = "";

                            if (_InstallType != null)
                            {
                                if (_InstallType.ToLower() == "saas")
                                {
                                    SO.Notifications.ActivityView = _AH.ActivityList(new List<ActivityModelView>(), _AH.FindMemberNotifications(_Connect, SO.SessionIdentity.Identity.identities_id.ToString()));
                                }
                                else
                                {
                                    SO.Notifications.ActivityView = _AH.ActivityList(new List<ActivityModelView>(), _AH.FindNotifications(_Connect));
                                }
                            }
                            else { SO.Notifications.ActivityView = _AH.ActivityList(new List<ActivityModelView>(), _AH.FindNotifications(_Connect)); }
                            SO.Notifications.newActivityCount = _AH.NewNotificationCount(_Connect, SO.SessionIdentity.Identity.identities_id.ToString());

                            if (SO.NextReload == null || SO.NextReload < DateTime.Now.AddMinutes(-5))
                                SO.NextReload = DateTime.Now.AddMinutes(5);

                            SO.ERCharts = new ERChartsModels();
                            SO.ERCharts.Charts = new List<DynamicChartModel>();

                            SO._PageFeatures = new PageFeatures();

                        }
                    }

                    //Only run this if you need to know Session Memory Related Stuff

                    if (_MemoryTracking.ToUpper() == "ON")
                    {
                        SO.ERMemory = new MemoryUsage();
                        SO.ERMemory.GCTotalMemory = GC.GetTotalMemory(false);
                        SO.ERMemory.GCTotalMemoryMB = (SO.ERMemory.GCTotalMemory / 1024) / 1024;
                        SO.ERMemory.SOGetGenerationID = GC.GetGeneration(SO);
                        GC.Collect(SO.ERMemory.SOGetGenerationID);
                        SO.ERMemory.SOTotalMemory = GC.GetTotalMemory(false);
                        SO.ERMemory.SOTotalMemoryMB = (SO.ERMemory.SOTotalMemory / 1024) / 1024;
                    }
                    break;
                case "clear":
                    SO = new SessionObjects();
                    SO.SessionIdentity = new IdentityModels();
                    //SO.SessionIdentity.Profile = new ViewProfileModel();
                    SO.SessionNotification = new NotificationsModel();
                    SO.SessionNotification.Apps_View = new List<VW__APPLICATIONS_NTFY>();
                    SO.SessionNotification.Cores_View = new List<VW__CORES_NTFY>();
                    SO.SessionNotification.Forms_View = new List<VW__FORMS_NTFY>();
                    SO.ERDictionary = new ERDictionaryModels();
                    SO.ERDictionary.fkeysViews = new List<ViewForeignKeysDictionary>();
                    SO.ERDictionary.pkeysViews = new List<ViewPrimaryKeysDictionary>();
                    SO.ERDictionary.tablesView = new List<ViewTablesDictionary>();
                    SO.ERDictionary.ukeysViews = new List<ViewUniqueKeysDictionary>();
                    SO.ERDictionary.viewsView = new List<ViewViewsDictionary>();
                    SO.SessionEndTime = DateTime.Now;

                    break;

            }
            return SO;
        }

        public NotificationsModel GetNotifications(IConnectToDB _Connect, NotificationsModel Model, long? ID)
        {
            Model.Apps_View = new List<VW__APPLICATIONS_NTFY>();
            Model.Cores_View = new List<VW__CORES_NTFY>();
            Model.Forms_View = new List<VW__FORMS_NTFY>();

            Notifications NotificationHelper = new Notifications();

            //NotificationsModel _Notification = new NotificationsModel();

            Model = NotificationHelper.Get(_Connect, ID);

            //Model.Apps_View.Add(new VW__APPLICATIONS_NTFY { application_name =  "", applications_id = 0, applications_ntfy_id=0, cores_id=0, dt_available=DateTime.Today, enable = "Y" });

            //Model.Cores_View.Add(new VW__CORES_NTFY { cores_id=0, cores_name="", cores_ntfy_id=0, dt_created=DateTime.Today, enable="Y", object_type=""  });

            //Model.Forms_View.Add(new VW__FORMS_NTFY {  applications_id=0, forms_id=0, forms_ntfy_id=0, dt_created = DateTime.Today, enable = "Y", object_type = "" });

            return Model;
        }

        public List<CoreModels> GetsCoreForUserID(IConnectToDB _Connect, List<CoreModels> Cores, int identity_id)
        {

            //Get Cores for user.

            CoreHelper CoreHelper = new CoreHelper();

            DataTable CoreTable = CoreHelper.FindAll(_Connect);


            foreach (DataRow CoreRow in CoreTable.Rows)
            {
                //Instantiate New Models
                CoreModels CoreModel = new CoreModels();
                CoreModel.Core = new CoreStruct();
                CoreModel.Core.Core = new CoreTableModel();

                CoreModel.Core.Core.core_name = CoreRow.Field<string>("core_name");
                CoreModel.Core.Core.dt_created = CoreRow.Field<DateTime>("dt_created");
                CoreModel.Core.Core.cores_id = CoreRow.Field<long?>("cores_id");
                CoreModel.Core.Core.object_type = CoreRow.Field<string>("object_type");
                CoreModel.Core.Core.dt_available = CoreRow.Field<DateTime?>("dt_available");
                CoreModel.Core.Core.dt_end = CoreRow.Field<DateTime?>("dt_end");
                CoreModel.Core.Core.enabled = CoreRow.Field<string>("enabled");

                //Get Applications belonging to Each Core.

                CoreModel.applications = GetApplicationsForUserID(_Connect, CoreModel, new List<ApplicationModels>(), identity_id);

                Cores.Add(CoreModel);
            }

            return Cores;
        }

        public List<ApplicationModels> GetApplicationsForUserID(IConnectToDB _Connect, CoreModels CoreModel, List<ApplicationModels> Applications, int identity_id)
        {

            AppHelper applicationhelper = new AppHelper();

            DataTable CoreApplications = applicationhelper.FindbyColumnID(_Connect, "Cores_ID", CoreModel.Core.Core.cores_id.ToString());

            ApplicationModels applicationModel = new ApplicationModels();

            foreach (DataRow AppRows in CoreApplications.Rows)
            {
                applicationModel = new ApplicationModels();
                applicationModel.AppView = new ViewApplicationModel();

                applicationModel.AppView.application_name = AppRows.Field<string>("application_name");
                applicationModel.AppView.applications_id = AppRows.Field<long?>("applications_id");
                applicationModel.AppView.cores_id = AppRows.Field<long?>("cores_id");

                applicationModel.stages = GetStagesForAppandUserID(_Connect, new List<StageModels>(), applicationModel, identity_id);

                Applications.Add(applicationModel);
            }

            return Applications;
        }

        public List<StageModels> GetStagesForAppandUserID(IConnectToDB _Connect, List<StageModels> Stages, ApplicationModels application, int identity_id)
        {
            StagesHelper stageshelper = new StagesHelper();

            DataTable ApplicationStages = stageshelper.FindbyColumnID(_Connect, "Applications_ID", application.AppView.applications_id.ToString());

            foreach (DataRow StageRows in ApplicationStages.Rows)
            {
                StageModels tempStageModel = new StageModels();
                tempStageModel.Stage = new StageModel();
                tempStageModel.Grips = new List<GripModels>();

                tempStageModel.Stage.stages_id = StageRows.Field<long?>("stages_id");
                tempStageModel.Stage.stage_name = StageRows.Field<string>("stage_name");
                tempStageModel.Stage.stage_type = StageRows.Field<string>("stage_type");
                tempStageModel.Stage.dt_created = StageRows.Field<DateTime>("dt_created");
                tempStageModel.Stage.dt_available = StageRows.Field<DateTime?>("dt_available");
                tempStageModel.Stage.dt_end = StageRows.Field<DateTime?>("dt_end");
                tempStageModel.Stage.enabled = StageRows.Field<string>("enabled");

                tempStageModel.Grips = GetgripsforStage(_Connect, new List<GripModels>(), tempStageModel, identity_id);

                Stages.Add(tempStageModel);
            }

            return Stages;
        }

        public List<GripModels> GetgripsforStage(IConnectToDB _Connect, List<GripModels> Grips, StageModels Stage, int identity_id)
        {
            GripsHelper griphelper = new GripsHelper();

            DataTable StageGrips = griphelper.FindbyColumnID(_Connect, "Stages_ID", Stage.Stage.stages_id.ToString());

            foreach (DataRow GripRows in StageGrips.Rows)
            {

                GripModels grip = new GripModels();
                grip.gripinfo = new GripModel();

                grip.gripinfo.grips_id = GripRows.Field<long?>("grips_id");
                grip.gripinfo.grip_name = GripRows.Field<string>("grip_name");
                grip.gripinfo.grip_type = GripRows.Field<string>("grip_type");
                grip.gripinfo.stages_id = GripRows.Field<long?>("stages_id");
                grip.gripinfo.stage_name = GripRows.Field<string>("stage_name");
                grip.gripinfo.stage_type = GripRows.Field<string>("stage_type");
                grip.gripinfo.identities_id = GripRows.Field<long?>("identities_id");
                grip.gripinfo.enabled = GripRows.Field<string>("enabled");
                grip.gripinfo.dt_created = GripRows.Field<DateTime>("dt_created");
                grip.gripinfo.dt_available = GripRows.Field<DateTime?>("dt_available");

                grip.ObjectSets = GetSetsbyGripNID(_Connect, new List<ObjectSetModels>(), grip, identity_id);

                Grips.Add(grip);
            }

            return Grips;
        }

        public List<ObjectSetModels> GetSetsbyGripNID(IConnectToDB _Connect, List<ObjectSetModels> Sets, GripModels Grip, int identity_id)
        {

            ObjectSetsHelper SetsHelper = new ObjectSetsHelper();

            DataTable ObjectSetsDT = SetsHelper.FindbyColumnID(_Connect, "Grips_ID", Grip.gripinfo.grips_id.ToString());

            foreach (DataRow SetRows in ObjectSetsDT.Rows)
            {
                ObjectSetModels Set = new ObjectSetModels();
                Set.SetView = new ViewObjectSetModel();

                Set.SetView.object_sets_id = SetRows.Field<long?>("object_sets_id");
                Set.SetView.dt_created = SetRows.Field<DateTime>("dt_created");
                Set.SetView.dt_available = SetRows.Field<DateTime?>("dt_available");
                Set.SetView.dt_end = SetRows.Field<DateTime?>("dt_end");

                Set.ObjectPropSets = GetPropSetsbySetNID(_Connect, new List<ObjectPropSetModels>(), Set, identity_id);

                Sets.Add(Set);
            }

            return Sets;
        }

        public List<ObjectPropSetModels> GetPropSetsbySetNID(IConnectToDB _Connect, List<ObjectPropSetModels> PropSets, ObjectSetModels ObjSet, int identity_id)
        {
            ObjectPropSetsHelper PropSetHelper = new ObjectPropSetsHelper();

            DataTable PropSetDT = PropSetHelper.FindbyColumnID(_Connect, "Object_Sets_ID", ObjSet.SetView.object_sets_id.ToString());

            foreach (DataRow PropRows in PropSetDT.Rows)
            {
                ObjectPropSetModels PropSet = new ObjectPropSetModels();
                PropSet.PropSetView = new PropSetView();

                PropSet.PropSetView.obj_prop_sets_id = PropRows.Field<long?>("obj_prop_sets_id");
                PropSet.PropSetView.object_prop_type = PropRows.Field<string>("object_prop_type");
                PropSet.PropSetView.property_name = PropRows.Field<string>("property_name");
                PropSet.PropSetView.property_value = PropRows.Field<string>("property_value");
                PropSet.PropSetView.value_datatype = PropRows.Field<string>("value_datatype");
                PropSet.PropSetView.has_parent = PropRows.Field<string>("has_parent");
                PropSet.PropSetView.has_child = PropRows.Field<string>("has_child");
                PropSet.PropSetView.parent_obj_prop_sets_id = PropRows.Field<long?>("parent_obj_prop_sets_id");

                PropSet.ObjectPropOptSets = GetPropOptSetsbyPropSetNID(_Connect, new List<ObjectPropOptSetModels>(), PropSet, identity_id);

                PropSets.Add(PropSet);
            }
            return PropSets;
        }

        public List<ObjectPropOptSetModels> GetPropOptSetsbyPropSetNID(IConnectToDB _Connect, List<ObjectPropOptSetModels> PropOptSets, ObjectPropSetModels PropSet, int identity_id)
        {
            ObjectPropOptSets PropOptHelper = new ObjectPropOptSets();

            DataTable PropOpt = PropOptHelper.FindbyColumnID(_Connect, "obj_prop_sets_id", PropSet.PropSetView.obj_prop_sets_id.ToString());

            foreach (DataRow PropOptRows in PropOpt.Rows)
            {
                ObjectPropOptSetModels PropOptSet = new ObjectPropOptSetModels();

                PropOptSet.obj_prop_opt_sets_id = PropOptRows.Field<long?>("obj_prop_opt_sets_id");
                PropOptSet.option_value = PropOptRows.Field<string>("option_value");
                PropOptSet.enabled = PropOptRows.Field<string>("enabled");

                PropOptSets.Add(PropOptSet);
            }

            return PropOptSets;
        }
    }
}

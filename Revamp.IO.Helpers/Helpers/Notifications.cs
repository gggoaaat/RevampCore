using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.Structs;

namespace Revamp.IO.Helpers.Helpers
{
    public class Notifications
    {
        /// <summary>
        /// Populates Model Necessary to Present Notifications.
        /// </summary>
        /// <param name="DB_PLATFORM"></param>
        /// <param name="connstring"></param>
        /// <param name="connownername"></param>
        /// <param name="NotificationTable"></param>
        /// <returns></returns>
        public NotificationsModel Get(IConnectToDB _Connect, long? Identity_ID)
        {
            List<NotificationsModel> Notifications = new List<NotificationsModel>();

            List<string> NotificationList = new List<string>();

            NotificationList.Add("FORMS");
            NotificationList.Add("CORES");
            NotificationList.Add("APPLICATIONS");

            NotificationsModel _Notification = new NotificationsModel();

            _Notification.Forms_View = new List<VW__FORMS_NTFY>();
            _Notification.Cores_View = new List<VW__CORES_NTFY>();
            _Notification.Apps_View = new List<VW__APPLICATIONS_NTFY>();

            foreach (string Name in NotificationList)
            {

                DataTable DT_Notifications = FindAll(_Connect, Name);

                foreach (DataRow datarowdc in DT_Notifications.Rows)
                {
                    switch (Name.ToLower())
                    {
                        case "form":
                        case "forms":
                            
                            _Notification.Forms_View.Add(new VW__FORMS_NTFY
                            {
                                applications_id = datarowdc.Field<long?>("applications_id"),
                                forms_id = datarowdc.Field<long?>("forms_id"),
                                identities_id = datarowdc.Field<long?>("identities_id"),
                                dt_created = datarowdc.Field<DateTime>("dt_created"),
                                object_type = datarowdc.Field<string>("object_type")
                            });
                            break;
                        case "cores":
                            

                            _Notification.Cores_View.Add(new VW__CORES_NTFY
                            {
                                cores_id = datarowdc.Field<long?>("cores_id"),
                                core_name = datarowdc.Field<string>("core_name"),
                                object_type = datarowdc.Field<string>("object_type"),
                                cores_ntfy_id = datarowdc.Field<long?>("cores_ntfy_id"),
                                dt_created = datarowdc.Field<DateTime>("dt_created")
                            });
                            break;
                        case "applications":

                            
                            _Notification.Apps_View.Add(new VW__APPLICATIONS_NTFY
                            {
                                application_name = datarowdc.Field<string>("application_name"),
                                applications_ntfy_id = datarowdc.Field<long?>("applications_ntfy_id"),
                                object_type = datarowdc.Field<string>("object_type"),
                                applications_id = datarowdc.Field<long?>("applications_id"),
                                dt_created = datarowdc.Field<DateTime>("dt_created")
                            });
                            break;
                        default:
                            break;
                    }
                }


            }

            //Notifications.Add(_Notification);

            return _Notification;
        }

        public DataTable FindAll(IConnectToDB _Connect, string NotificationTable)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__" + NotificationTable + "_NTFY_SEARCH",
                new DataTableDotNetModelMetaData { length = 50, order = "DT_CREATED desc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public string GetNotificationSettingValue(IConnectToDB _Connect, long? identity, string type)
        {
            ER_Query er_query = new ER_Query();
            ER_Query.Parameter_Run SQlin = new ER_Query.Parameter_Run();
            string settingValue = "";

            SQlin.sqlIn = "Select property_type from CSA.IDENTITY_PROPERTIES a where a.DT_CREATED >= (select max(ip.DT_CREATED) FROM CSA.IDENTITY_PROPERTIES ip where ip.IDENTITIES_ID = @IDENTITIES_ID and ip.PROPERTY_NAME = @PROPERTY_NAME) and a.PROPERTY_NAME = @PROPERTY_NAME";

            SQlin._dbParameters = new List<DBParameters>
            {
                new DBParameters { ParamName = "IDENTITIES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = identity },
                new DBParameters { ParamName = "PROPERTY_NAME", MSSqlParamDataType = SqlDbType.VarChar, ParamValue = type }
            };

            DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlin);

            if(TempDataTable.Rows.Count > 0)
            {
                settingValue = TempDataTable.Rows[0][0].ToString();

            }

            return settingValue;
        }
 
    }
}
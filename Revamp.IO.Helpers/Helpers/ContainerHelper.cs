using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Revamp.IO.Foundation;
using Microsoft.AspNetCore.Mvc;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs.Models;
using Revamp.IO.Structs.Models.DataEntry;
using Revamp.IO.DB.Binds.IO.Dynamic;
using Revamp.IO.Structs;

namespace Revamp.IO.Helpers.Helpers
{
    public class ContainerHelper
    {
        public DataTable FindAll(IConnectToDB _Connect)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CONTAINERS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "CONTAINER_NAME asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable Find(IConnectToDB _Connect, string _id)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = "CONTAINERS_ID_", DBType = SqlDbType.BigInt, ParamValue = _id });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CONTAINERS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }


        public DataTable FindbyColumnID(IConnectToDB _Connect, string _column, string _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", DBType = SqlDbType.VarChar, SearchParamSize = -1, ParamValue = _value });

            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CONTAINERS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "CONTAINERS_ID desc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable FindbyColumnIDs(IConnectToDB _Connect, string _column, List<string> _value)
        {
            _DynamicOutputProcedures DynamicOutput = new _DynamicOutputProcedures();

            List<DynamicModels.RootReportFilter> Filters = new List<DynamicModels.RootReportFilter>();

            Filters.Add(new DynamicModels.RootReportFilter { FilterName = _column + "_", ParamValue = String.Join(",", _value) });
            
            DataTable TempDataTable = DynamicOutput.DynoProcSearch(_Connect, "Custom Query", "SP_S_VW__CONTAINERS_SEARCH",
                new DataTableDotNetModelMetaData { length = -1, order = "1 asc", start = 0, verify = "T" },
                Filters);

            return TempDataTable;
        }

        public DataTable GetContainerApplications(IConnectToDB _Connect, string coreid, string containers_id)
        {
           AppHelper appHelper = new AppHelper();
           List<string> appids = new List<string>();
           
           ER_Query er_query = new ER_Query();
           ER_Query.Parameter_Run SQlinAppId = new ER_Query.Parameter_Run();
           ER_Query.Parameter_Run SQlinApps = new ER_Query.Parameter_Run();

            SQlinAppId._dbParameters = new List<DBParameters>
           {
                new DBParameters { ParamName = "CORES_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = coreid },
                new DBParameters { ParamName = "CONTAINERS_ID", MSSqlParamDataType = SqlDbType.BigInt, ParamValue = containers_id }
           };

           SQlinAppId.sqlIn = "Select DISTINCT(APPLICATIONS_ID)  from CSA.VW__STAGES s where CONTAINERS_ID  = @CONTAINERS_ID and cores_id = @CORES_ID and RENDITION in (Select  MAX(Convert(int,RENDITION)) from CSA.VW__STAGES s1 where s.ROOT_APPLICATION = s1.ROOT_APPLICATION) order by APPLICATIONS_ID desc";

           DataTable TempDataTable = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinAppId);

           for (int i = 0; i < TempDataTable.Rows.Count; i++)
           {
               appids.Add(TempDataTable.Rows[i]["applications_id"].ToString());            
           }

            string query = "Select a0.*,(select top(1) [USER_NAME] from CSA.VW__ACTIVITY  a INNEr join CSA.VW__IDENTITIES i ON a.IDENTITIES_ID=i.IDENTITIES_ID " +
          " where a.APPLICATIONS_ID=a0.APPLICATIONS_ID and a.OBJECT_TYPE='add object' and Table_Source='APPLICATIONS')USER_NAME from CSA.VW__APPLICATIONS a0 " +
          " INNER JOIN CSA.VW__APPLICATIONS a1 ON a0.APPLICATIONS_ID = a1.APPLICATIONS_ID and a0.APPLICATION_NAME not in('Core Settings','Revamp System') and a1.RENDITION in (Select  MAX(Convert(int,RENDITION))  from CSA.VW__APPLICATIONS a1a where a1.ROOT_APPLICATION = a1a.ROOT_APPLICATION)";

            if (TempDataTable.Rows.Count > 0)
            {
                query += " where a0.APPLICATIONS_ID in (";

                for (int i = 0; i < appids.Count; i++)
                {

                    // apps = appHelper.FindbyColumnID(_Connect, "applications_id", appids[i]);
                    if (i != (appids.Count - 1))
                    {
                        query += appids[i] + ",";
                    }
                    else
                    {
                        query += appids[i];
                    }
                }

                query += ")  and a0.Enabled='Y' Order By DT_CREATED DESC";
            }
            else
            {
                query += " where a0.APPLICATIONS_ID in (1)"; //used to return nothing if no apps belong to the container passed in
            }

            SQlinApps.sqlIn = query;

            DataTable appsDT = er_query.RUN_PARAMETER_QUERY(_Connect, SQlinApps);

            return appsDT;
        }

        public List<ContainersModel> Containers(IConnectToDB _Connect, List<ContainersModel> ContainerModel, DataTable _DT, string ModelType)
        {
            List<ContainersModel> Containers = new List<ContainersModel>();

            switch (ModelType.ToLower())
            {
                case "view":
                    foreach (DataRow ContainerViewRow in _DT.Rows)
                    {
                        ContainersModel Container = new ContainersModel();
                        Container.Container = new ContainersModel.ContainerStruct();
                        Container.Container.ContainerView = new ContainersModel.ViewContainerModel();

                        
                        Container.Container.ContainerView = ContainerView(new ContainersModel.ViewContainerModel(), ContainerViewRow);
                        Containers.Add(Container);
                    }
                    break;
                case "table":
                    break;
            }

            return Containers;
        }

        private ContainersModel.ViewContainerModel ContainerView(ContainersModel.ViewContainerModel ContainerView, DataRow _DR)
        {
            ContainerView = new ContainersModel.ViewContainerModel
            {
                container_name = _DR.Field<string>("container_name"),
                containers_id = _DR.Field<long?>("containers_id"),
                dt_available = _DR.Field<DateTime?>("dt_available"),
                dt_created = _DR.Field<DateTime>("dt_created"),
                dt_end = _DR.Field<DateTime?>("dt_end"),
                object_layer = _DR.Field<string>("object_layer")
            };


            return ContainerView;
        }

        public ContainersModel GetContainer(ContainersModel Containers, DataRow _DR)
        {
            Containers = new ContainersModel
            {
                ViewContainer = new ContainersModel.ViewContainerModel
                {
                    
                    enabled = _DR.Field<string>("enabled"),
                    containers_id = _DR.Field<long?>("containers_id"),
                    container_name = _DR.Field<string>("container_name"),
                    object_layer = _DR.Field<string>("object_layer"),
                    object_type = _DR.Field<string>("object_type"),
                    dt_available = _DR.Field<DateTime?>("dt_available"),
                    dt_created = _DR.Field<DateTime>("dt_created"),
                    dt_end = _DR.Field<DateTime?>("dt_end"),
                }
            };
            return Containers;
        }
        

        public List<ContainersModel> GetContainers(List<ContainersModel> Containers, DataTable _DT)
        {
            ContainersModel _Containers = new ContainersModel();

            foreach (DataRow ContainerRow in _DT.Rows)
            {
                Containers.Add(GetContainer(new ContainersModel(), ContainerRow));
            }

            return Containers;
        }

        public Values.AddContainer AddContainer(IConnectToDB _Connect, FormCollection collection)
        {
            add addHelp = new add();

            Values.AddContainer _result = new Values.AddContainer();
            IValueProvider valueProvider = collection.ToValueProvider();
            if (collection.Keys.Count > 0)
            {

                ValueProviderResult result = valueProvider.GetValue(collection.Keys[0].ToString());

                _result = addHelp.ADD_ENTRY_Containers(_Connect, new Values.AddContainer { I_CONTAINER_NAME = result.AttemptedValue.ToString() });

                if (_result.O_CONTAINERS_ID > 0)
                {
                    _result.O_ERR_MESS = "Container Saved!";
                }
            }
            else
            {
                _result.O_ERR_MESS = "This container already exist";
            }

            return _result;
        }
    }
}
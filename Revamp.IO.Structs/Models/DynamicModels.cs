using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class DynamicModels
    {
        [Serializable]
        public class ReportLogicModel
        {
            public string template { get; set; }
            public List<clause> _clause { get; set; }
            public bool useQsCol { get; set; }
            public NameValueCollection qscol { get; set; }
            public List<RootReportFilter> UsedFilters { get; set; }
            public RootReport CurrentReportSelected { get; set; }
            public SessionObjects _SessionModel { get; set; }
            public bool isFilterQuery { get; set; }
        }

        [Serializable]
        public class ReportDefinitions
        {
            public RootReport theReport { get; set; }
            public DataTable theStructure { get; set; }
            public string DynamicColumns { get; set; }
            public bool GetStructure { get; set; }
            public string SearchReport { get; set; }
        }

        [Serializable]
        public class Core
        {
            public List<RootReport> RootReports { get; set; }
        }

        [Serializable]
        public class RootReport
        {
            public long Template_ID { get; set; }
            public string Template { get; set; }
            public string ReportName { get; set; }
            public string ProcedureName { get; set; } = "";
            public List<RootReportFilter> ReportFilters { get; set; }
            public List<ReportOrder> ReportColumnOrderSet { get; set; }
            public List<CrystalReportStruct> CrystalReports { get; set; }
            public string Source { get; set; }
        }

        [Serializable]
        public class ReportOrder
        {
            public string sort_column { get; set; }
            public string sort_direction { get; set; }
            public long sort_order { get; set; }
        }

        [Serializable]
        public class RootReportFilter
        {
            public string FilterName { get; set; }

            public string PrettyName { get; set; }

            public FilterType FilterType { get; set; }

            public bool inPickList { get; set; }

            public SqlDbType DBType { get; set; } = SqlDbType.VarChar;

            public int ParamSize { get; set; }

            public SqlDbType SearchDBType { get; set; }

            public int SearchParamSize { get; set; } = -1;

            public object ParamValue { get; set; }

            public string FilterSelect { get; set; }

            public bool Required { get; set; }

            public string AlternateSource { get; set; }

            public string AlternateSchema { get; set; }

            public string AlternateTemplate { get; set; }

            public string AlternateReportName { get; set; }
        }

        [Serializable]
        public enum FilterType
        {
            //DO NOT CHANGE ORDER!!!!!
            MultiSelect, DatePicker, Toggle, Columns, Hidden
        }

        [Serializable]
        public class CrystalReportStruct
        {
            public string Name { get; set; }
            public string FilePath { get; set; }
            public string OrderBY { get; set; }
            public string ExportType { get; set; }
            public string Columns { get; set; }
            public List<CCParameters> Parameters { get; set; }
        }

        [Serializable]
        public class CCParameters
        {
            public string parametersName { get; set; }
            public string Value { get; set; }
            public string ValueSource { get; set; }
        }

        public class Input
        {
            [Serializable]
            public class Root_Report
            {
                public long? I_BASE_ROOT_REPORT_ID { get; set; }
                public long? I_PREV_ROOT_REPORT_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_TEMPLATE_ID { get; set; }
                public string I_TEMPLATE_NAME { get; set; }
                public string I_REPORT_NAME { get; set; }
                public string I_PROCEDURE_NAME { get; set; }
                public string I_SOURCE { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_ROOT_REPORT_ID { get; set; }

                public List<Root_Report_Column> Columns { get; set; }

                public List<Root_Report_Filter> Filters { get; set; }
            }

            [Serializable]
            public class Root_Report_Filter
            {
                public string Generated_SQL { get; set; }
                public long? I_BASE_ROOT_REPORT_FILTER_ID { get; set; }
                public long? I_PREV_ROOT_REPORT_FILTER_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_ROOT_REPORT_ID { get; set; }
                public string I_REQUIRED { get; set; }
                public string I_FILTER_NAME { get; set; }
                public string I_PRETTY_NAME { get; set; }
                public string I_FILTER_TYPE { get; set; }
                public string I_IN_PICK_LIST { get; set; }
                public string I_DB_TYPE { get; set; }
                public long? I_PARAM_SIZE { get; set; }
                public string I_SEARCH_DB_TYPE { get; set; }
                public long? I_SEARCH_DB_SIZE { get; set; }
                public string I_ParamValue { get; set; }
                public string I_FILTER_SELECT { get; set; }
                public string I_ALTERNATE_SOURCE { get; set; }
                public string I_ALTERNATE_SCHEMA { get; set; }
                public string I_ALTERNATE_TEMPLATE { get; set; }
                public string I_ALTERNATE_REPORT_NAME { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_ROOT_REPORT_FILTER_ID { get; set; }
            }

            [Serializable]
            public class Custom_Reports
            {
                public string V_ROOT_REPORT_NAME { get; set; }
                public long? I_BASE_CUSTOM_REPORT_ID { get; set; }
                public long? I_PREV_CUSTOM_REPORT_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_ROOT_REPORT_ID { get; set; }
                public long? I_BASE_ROOT_REPORT_ID { get; set; }
                public string I_REPORT_NAME { get; set; }
                public string I_DESCRIPTION { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_CUSTOM_REPORT_ID { get; set; }
                public List<Custom_Report_Filter> Filters { get; set; }
                public List<Custom_Report_Column> Columns { get; set; }
                public RootReport RootReport { get; set; }
                public string OrderBy { get; set; }
            }

            [Serializable]
            public class Portlet_
            {
                public Int64 I_BASE_PORTLET_ID { get; set; }
                public Int64 I_PREV_PORTLET_ID { get; set; }
                public char I_ENABLED { get; set; }
                public Int64? I_IDENTITIES_ID { get; set; }
                public Int64 I_APPLICATION_ID { get; set; }
                public string I_TITLE { get; set; }
                public Int64? INITIAL_CSA_PRIVILEGE_ID { get; set; }
                public Int64 O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public Int64 O_PORTLET_ID { get; set; }

                public List<Portlet_Container> Containers { get; set; }
            }

            [Serializable]
            public class Portlet_Container
            {
                public long? I_BASE_CONTAINER_ID { get; set; }
                public long? I_PREV_CONTAINER_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_PORTLET_ID { get; set; }
                public string I_TITLE { get; set; }
                public string I_SIZE { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_CONTAINER_ID { get; set; }
            }

            [Serializable]
            public class Custom_Report_Filter
            {
                public long? I_BASE_FILTER_ID { get; set; }
                public long? I_PREV_FILTER_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_CUSTOM_REPORT_ID { get; set; }
                public char? I_MANDATORY { get; set; }
                public string I_FILTER { get; set; }
                public string I_FILTER_ALIAS { get; set; }
                public string I_REQUIRED { get; set; }
                public long? I_FILTER_ORDER { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_FILTER_ID { get; set; }
            }

            [Serializable]
            public class Root_Report_Column
            {
                public long? I_BASE_ROOT_REPORT_COLUMN_ID { get; set; }
                public long? I_PREV_ROOT_REPORT_COLUMN_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_ROOT_REPORT_ID { get; set; }
                public string I_ORIGINAL_COLUMN { get; set; }
                public string I_COLUMN_AREA { get; set; }
                public string I_ALIAS_AREA { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_ROOT_REPORT_COLUMN_ID { get; set; }
            }

            [Serializable]
            public class Custom_Report_Column
            {
                public long? I_BASE_CUSTOM_REPORT_COLUMN_ID { get; set; }
                public long? I_PREV_CUSTOM_REPORT_COLUMN_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_CUSTOM_REPORT_ID { get; set; }
                public string I_ORIGINAL_COLUMN { get; set; }
                public string I_COLUMN_AREA { get; set; }
                public string I_ALIAS_AREA { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_CUSTOM_REPORT_COLUMN_ID { get; set; }
            }

            [Serializable]
            public class Portlet_Container_Custom_Reports
            {
                public long? I_BASE_PORTLET_CONTAINER_CUSTOM_REPORT_ID { get; set; }
                public long? I_PREV_PORTLET_CONTAINER_CUSTOM_REPORT_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_CONTAINER_ID { get; set; }
                public long? I_CUSTOM_REPORT_ID { get; set; }
                public string I_TITLE { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_PORTLET_CONTAINER_CUSTOM_REPORT_ID { get; set; }
            }

            [Serializable]
            /// <summary>
            /// model used to delete a container_report record (by id)
            /// </summary>
            public class Delete_Portlet_Container_Custom_Report
            {
                public string I_LOGIN { get; set; }
                public long? I_CONTAINER_REPORT_ID { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
            }

            public class Load_App_Custom_Reports
            {
                public long? I_APP_ID { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
            }

            public class Custom_Report_Order
            {
                public long? I_BASE_CUSTOM_REPORTS_ORDER_ID { get; set; }
                public long? I_PREV_CUSTOM_REPORTS_ORDER_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_CUSTOM_REPORTS_ORDER_SET_ID { get; set; }
                public string I_SORT_COLUMN { get; set; }
                public string I_SORT_DIRECTION { get; set; }
                public long? I_SORT_ORDER { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_CUSTOM_REPORTS_ORDER_ID { get; set; }
            }

            public class Root_Report_Order
            {
                public long? I_BASE_ROOT_REPORTS_ORDER_ID { get; set; }
                public long? I_PREV_ROOT_REPORTS_ORDER_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_ROOT_REPORTS_ORDER_SET_ID { get; set; }
                public string I_SORT_COLUMN { get; set; }
                public string I_SORT_DIRECTION { get; set; }
                public long? I_SORT_ORDER { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_ROOT_REPORTS_ORDER_ID { get; set; }
            }

            public class Root_Report_Order_Sets
            {
                public long? I_BASE_ROOT_REPORTS_ORDER_SET_ID { get; set; }
                public long? I_PREV_ROOT_REPORTS_ORDER_SET_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_ROOT_REPORT_ID { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_ROOT_REPORTS_ORDER_SET_ID { get; set; }
            }

            public class Custom_Report_Order_Sets
            {
                public long? I_BASE_CUSTOM_REPORTS_ORDER_SET_ID { get; set; }
                public long? I_PREV_CUSTOM_REPORTS_ORDER_SET_ID { get; set; }
                public char? I_ENABLED { get; set; }
                public long? I_IDENTITIES_ID { get; set; }
                public long? I_CUSTOM_REPORT_ID { get; set; }
                public long? O_ERR_NUMB { get; set; }
                public string O_ERR_MESS { get; set; }
                public long? O_CUSTOM_REPORTS_ORDER_SET_ID { get; set; }
            }
        }

        public class Output
        {
            [Serializable]
            public class Portlet_Search
            {
                public string P_TableorCount { get; set; }
                public string P_DYNO_COL { get; set; }
                public string P_SEARCH { get; set; }
                public string P_WHERE { get; set; }
                public long? P_STARTING_ROW { get; set; }
                public long? P_LENGTH_OF_SET { get; set; }
                public string P_ORDER_BY { get; set; }
                public char? P_GET_LATEST { get; set; }
                public string P_PORTLET_ID { get; set; }
                public string P_EXCLUDE_PORTLET_ID { get; set; }
                public string P_BASE_PORTLET_ID { get; set; }
                public string P_EXCLUDE_BASE_PORTLET_ID { get; set; }
                public string P_PREV_PORTLET_ID { get; set; }
                public string P_EXCLUDE_PREV_PORTLET_ID { get; set; }
                public string P_ENABLED { get; set; }
                public string P_EXCLUDE_ENABLED { get; set; }
                public string P_DT_CREATED { get; set; }
                public string P_EXCLUDE_DT_CREATED { get; set; }
                public string P_DT_UPDATED { get; set; }
                public string P_EXCLUDE_DT_UPDATED { get; set; }
                public string P_IDENTITY_ID { get; set; }
                public string P_EXCLUDE_IDENTITY_ID { get; set; }
                public string P_APPLICATION_ID { get; set; }
                public string P_EXCLUDE_APPLICATION_ID { get; set; }
                public string P_TITLE { get; set; }
                public string P_EXCLUDE_TITLE { get; set; }
                public char? P_VERIFY { get; set; }
            }

            [Serializable]
            public class Portlet_Containers_Search
            {
                public string P_TableorCount { get; set; }
                public string P_DYNO_COL { get; set; }
                public string P_SEARCH { get; set; }
                public string P_WHERE { get; set; }
                public long? P_STARTING_ROW { get; set; }
                public long? P_LENGTH_OF_SET { get; set; }
                public string P_ORDER_BY { get; set; }
                public char? P_GET_LATEST { get; set; }
                public string P_CONTAINER_ID { get; set; }
                public string P_EXCLUDE_CONTAINER_ID { get; set; }
                public string P_BASE_CONTAINER_ID { get; set; }
                public string P_EXCLUDE_BASE_CONTAINER_ID { get; set; }
                public string P_PREV_CONTAINER_ID { get; set; }
                public string P_EXCLUDE_PREV_CONTAINER_ID { get; set; }
                public string P_ENABLED { get; set; }
                public string P_EXCLUDE_ENABLED { get; set; }
                public string P_DT_CREATED { get; set; }
                public string P_EXCLUDE_DT_CREATED { get; set; }
                public string P_DT_UPDATED { get; set; }
                public string P_EXCLUDE_DT_UPDATED { get; set; }
                public string P_IDENTITY_ID { get; set; }
                public string P_EXCLUDE_IDENTITY_ID { get; set; }
                public string P_BASE_PORTLET_ID { get; set; }
                public string P_EXCLUDE_BASE_PORTLET_ID { get; set; }
                public string P_PORTLET_ID { get; set; }
                public string P_EXCLUDE_PORTLET_ID { get; set; }
                public string P_TITLE { get; set; }
                public string P_EXCLUDE_TITLE { get; set; }
                public string P_SIZE { get; set; }
                public string P_EXCLUDE_SIZE { get; set; }
                public char? P_VERIFY { get; set; }
            }

            [Serializable]
            public class Root_Reports_Search
            {
                public string P_TableorCount { get; set; }
                public string P_DYNO_COL { get; set; }
                public string P_SEARCH { get; set; }
                public string P_WHERE { get; set; }
                public long? P_STARTING_ROW { get; set; }
                public long? P_LENGTH_OF_SET { get; set; }
                public string P_ORDER_BY { get; set; }
                public char? P_GET_LATEST { get; set; }
                public string P_ROOT_REPORT_ID { get; set; }
                public string P_EXCLUDE_ROOT_REPORT_ID { get; set; }
                public string P_BASE_ROOT_REPORT_ID { get; set; }
                public string P_EXCLUDE_BASE_ROOT_REPORT_ID { get; set; }
                public string P_PREV_ROOT_REPORT_ID { get; set; }
                public string P_EXCLUDE_PREV_ROOT_REPORT_ID { get; set; }
                public string P_ENABLED { get; set; }
                public string P_EXCLUDE_ENABLED { get; set; }
                public string P_DT_CREATED { get; set; }
                public string P_EXCLUDE_DT_CREATED { get; set; }
                public string P_DT_UPDATED { get; set; }
                public string P_EXCLUDE_DT_UPDATED { get; set; }
                public string P_IDENTITY_ID { get; set; }
                public string P_EXCLUDE_IDENTITY_ID { get; set; }
                public string P_TEMPLATE_ID { get; set; }
                public string P_EXCLUDE_TEMPLATE_ID { get; set; }
                public string P_TEMPLATE_NAME { get; set; }
                public string P_EXCLUDE_TEMPLATE_NAME { get; set; }
                public string P_REPORT_NAME { get; set; }
                public string P_EXCLUDE_REPORT_NAME { get; set; }
                public string P_PROCEDURE_NAME { get; set; }
                public string P_EXCLUDE_PROCEDURE_NAME { get; set; }
                public char? P_VERIFY { get; set; }
            }

            [Serializable]
            public class PortletPrivileges
            {
                public PortletPrivileges()
                {
                    privilege_ids = new List<long>();
                }

                public long base_portlet_id { get; set; }
                public long portlet_privilege_type_id { get; set; }
                public List<long> privilege_ids { get; set; }
            }
        }

    }
}

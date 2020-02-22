using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class CommonModels
    {
        [Serializable]
        public class SearchProcedureStruct
        {
            public string ProcedurePrefix { get; set; }
            public string ProcedureName { get; set; }
            public bool GetLatestVersion { get; set; }
            public string SourceName { get; set; }
            public bool OnlyEnabled { get; set; }
            public List<string> ParamList { get; set; }
            public List<string> ColumnOverride { get; set; }
            public List<MoreParam> MoreParams { get; set; }
            public bool EnableDistinct { get; set; }
            public string OrderBy { get; set; }

            public class MoreParam
            {
                public string ParamName { get; set; }
                public string ParamDataType { get; set; }
                public string ParamClause { get; set; }
                public string ParamDefault { get; set; }

            }

        }

        [Serializable]
        public class MVCGetPartial
        {
            public ControllerContext _thisController { get; set; }

            public ViewDataDictionary _ViewData { get; set; }

            public TempDataDictionary _TempData { get; set; }

            public string ViewName { get; set; }

            public object model { get; set; }

            public object model2 { get; set; }
        }

        [Serializable]
        public class DynoDataTable
        {
            public string ID { get; set; }

            public DataTable Result { get; set; }
        }

        [Serializable]
        public class CommonDataTablePartialModel
        {
            public DataTable _Datatable { get; set; }
            public string _TableName { get; set; }
        }
    }
}

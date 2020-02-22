using System;
using System.Collections;
using System.Collections.Generic;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class DataTableModels
    {
    }

    [Serializable]
    public class DataTableDotNetModel
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public ArrayList data { get; set; }
        //public string data { get; set; }

    }

    [Serializable]
    public class DataTableDotNetModel1
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public object data { get; set; }
    }

    [Serializable]
    public class DataTableDotNetModelMetaData
    {
        public Int64? start { get; set; }
        public Int64? length { get; set; }
        public string order { get; set; }

        public string search { get; set; }

        public string columns { get; set; }

        public string verify { get; set; }

        public List<clause> _clause { get; set; }

    }

    [Serializable]
    public class clause
    {
        public string leftSideOfClause { get; set; }
        public string ClauseConditions { get; set; }
        public string rightSideOfClause { get; set; }
        public string preConditions { get; set; }


        public string AppName { get; set; }
        public string QueryName { get; set; }
        //public string leftSideOfClause { get; set; }
        //public string ClauseConditions { get; set; }
        // public string rightSideOfClause { get; set; }
        public string rightSideOfClause2 { get; set; }
        public string QueryType { get; set; }
        //public string preConditions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Structs.Models
{
    [Serializable]
    public class SQLProcedureModels
    {
        [Serializable]
        public class SQL_PROCEDURE_CALL
        {
            public string ProcedureType { get; set; }
            public string ProcedureName { get; set; }
            public List<DBParameters> _dbParameters { get; set; }
            public Results result { get; set; }
            public long ID { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        [Serializable]
        public class Results
        {
            public DataTable _DataTable { get; set; }
            public string _string { get; set; }
            public long _long { get; set; }
            public ResultsType resulttype { get; set; }
        }

        public enum ResultsType
        {
            DataTable, String, Long
        }

        [Serializable]
        public class PARAMATER_VALUE
        {
            public string PARAMETER { get; set; }
            public string VALUE { get; set; }
        }

        [Serializable]
        public class BIG_CALL
        {
            public bool CHAIN_COMMANDS { get; set; }
            public List<SQL_PROCEDURE_CALL> COMMANDS { get; set; }

            public List<PARAMATER_VALUE> PARAMETER_CONTAINER { get; set; }
        }
    }
}

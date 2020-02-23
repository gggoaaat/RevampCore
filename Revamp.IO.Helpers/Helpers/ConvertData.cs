using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;

namespace Revamp.IO.Helpers.Helpers
{
    public class ConvertData
    {
        public string ConvertDataTabletoString(DataTable dt)
        {            
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            string tempstring = JsonConvert.SerializeObject(rows);

            return tempstring;
        }
    }
}
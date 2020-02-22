using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Revamp.IO.Helpers.Helpers
{
    public class ConvertData
    {
        public string ConvertDataTabletoString(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
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

            string tempstring = serializer.Serialize(rows);

            return tempstring;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace Asiatek.Common
{
    public class JsonHelper
    {
        public static string DataTableToJson(DataTable table)
        {
            List<Dictionary<string, object>> listDataRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dataRow;
            foreach (DataRow row in table.Rows)
            {
                dataRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    dataRow.Add(col.ColumnName, row[col]);
                }
                listDataRows.Add(dataRow);
            }
            return new JavaScriptSerializer().Serialize(listDataRows);
        }

        public static List<T> ConvertToList<T>(string jsonStr)
        {
            return new JavaScriptSerializer().Deserialize<List<T>>(jsonStr);
        }

        public static string ConvertToJson<T>(IEnumerable<T> list)
        {
            var result = new JavaScriptSerializer().Serialize(list);
            //注意，这里将UTC的时间戳转换为可读的 本地时间  ，
            //因为是本地时间，所以如果遇到需要跨时区的情况，请修改这部分
            //要么转成可读的UTC时间，要么直接保留原时间戳，但是在转回去的时候注意转换
            result = Regex.Replace(result, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            });
            return result;
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Utils;

namespace Common
{
    public static class Extensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
        public static int ToInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return -1;
            return int.Parse(str);
        }
        public static bool ToBoolean(this JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return false;
            return bool.Parse(token.ToString());
        }
        public static int ToInt(this JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return -1;
            return int.Parse(token.ToString());
        }
        public static long ToLong(this JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return 0;
            return long.Parse(token.ToString());
        }
        public static float ToFloat(this JToken token)
        {
            if (token == null || token.Type == JTokenType.Null)
                return -1;
            return float.Parse(token.ToString());
        }

        public static float ToFloat(this string token)
        {
            if (token == null)
                return -1;
            return float.Parse(token);
        }

        public static bool EqualsIgnoreCase(this string str, string str2)
        {
            return str.ToUpper().Equals(str2.ToUpper());
        }

        public static string Format(this string str, params string[] parameters)
        {
            return str.Format(parameters);
        }

        public static DateTime ToDateTime(this long millis)
        {
            return new DateTime(1970, 1, 1, 3, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(millis);
        }


        public static string ToJsonString(this DataTable table)
        {
            return JsonConvert.SerializeObject(table, Formatting.Indented);
        }

        public static JArray ToJArray(this DataTable table)
        {
            var str = JsonConvert.SerializeObject(table, Formatting.Indented);
            return JArray.Parse(str);
        }

        public static List<JObject> ConvertToJObjectList(DataTable dataTable)
        {
            var list = new List<JObject>();

            foreach (DataRow row in dataTable.Rows)
            {
                var item = new JObject();

                foreach (DataColumn column in dataTable.Columns)
                {
                    item.Add(column.ColumnName, JToken.FromObject(row[column.ColumnName]));
                }

                list.Add(item);
            }

            return list;
        }

        public static string[] ConvertTostringArray(this DataTable dataTable)
        {
            int count = dataTable.Rows.Count;
            string[] list = new string[count];
            int i = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    list[i++] = JToken.FromObject(row[column.ColumnName]).ToString();
                }
            }

            return list;
        }


        public static JObject ToJObject(this DataRow row)
        {
            var str = JsonConvert.SerializeObject(row, Formatting.Indented);
            return JObject.Parse(str);
        }

        public static Dictionary<string, object> ToDictionary(this DataTable dt)
        {
            var kvpsMap = new Dictionary<string, object>();
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in dt.Columns)
                    {
                        kvpsMap.Add(col.ColumnName, row[col.ColumnName]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return kvpsMap;
        }

        public static string ToJObjectString(this DataTable table)
        {
            var str = JsonConvert.SerializeObject(table, Formatting.Indented);
            return JArray.Parse(str).Children<JObject>().FirstOrDefault().ToString();
        }

        public static JObject ToJObject(this DataTable table)
        {
            var str = JsonConvert.SerializeObject(table, Formatting.None);
            return JArray.Parse(str).Children<JObject>().FirstOrDefault();
        }


        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public static bool AreAllColumnsEmpty(DataRow dr)
        {
            if (dr == null)
            {
                return true;
            }
            else
            {
                foreach (var value in dr.ItemArray)
                {
                    if (value != null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static DataTable Pivot(DataTable table, string pivotColumnName)
        {
            // TODO make sure the table contains at least two columns

            // get the data type of the first value column
            var dataType = table.Columns[1].DataType;

            // create a pivoted table, and add the first column
            var pivotedTable = new DataTable();
            pivotedTable.Columns.Add("Row Name", typeof(string));

            // determine the names of the remaining columns of the pivoted table
            var additionalColumnNames = table.AsEnumerable().Select(x => x[pivotColumnName].ToString());

            // add the remaining columns to the pivoted table
            foreach (var columnName in additionalColumnNames)
                pivotedTable.Columns.Add(columnName, dataType);

            // determine the row names for the pivoted table
            var rowNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Where(x => x != pivotColumnName);

            // fill in the pivoted data
            foreach (var rowName in rowNames)
            {
                // get the value data from the appropriate column of the input table
                var pivotedData = table.AsEnumerable().Select(x => x[rowName]);

                // make the rowName the first value
                var data = new object[] { rowName }.Concat(pivotedData).ToArray();

                // add the row
                pivotedTable.Rows.Add(data);
            }
            return pivotedTable;
        }
    }

}

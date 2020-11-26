using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace DbTools.Extensions
{
    public static class DataTableExtension
    {
        /// <summary>
        /// DataTable 轉換為List 集合
        /// </summary>
        /// <typeparam name="TResult">類型</typeparam>
        /// <param name="d​​t">DataTable</param>
        /// <returns></returns>
        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class, new()
        {
            //創建一個屬性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //獲取TResult的類型實例  反射的入口 
            Type t = typeof(TResult);
            //獲得TResult 的所有的Public 屬性並找出TResult屬性和DataTable的列名稱相同的屬性(PropertyInfo) 並加入到屬性列表
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => { if (dt.Columns.IndexOf(p.Name) != -1) prlist.Add(p); });
            //創建返回的集合
            List<TResult> oblist = new List<TResult>();

            foreach (DataRow row in dt.Rows)
            {
                //創建TResult的實例
                TResult ob = new TResult();
                //找到對應的數據  並賦值
                prlist.ForEach(p => {
                    if (row[p.Name] != DBNull.Value)
                    {
                        var type = p.PropertyType.Name;
                        var value = type == "Boolean" ? TranBoolean(row[p.Name].ToString()) : row[p.Name];
                        p.SetValue(ob, value, null);
                    }
                });
                //放入到返回的集合中.
                oblist.Add(ob);
            }
            return oblist;
        }
        /// <summary>
        /// DataTable 轉換為List 集合
        /// </summary>
        /// <typeparam name="TResult">類型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <param name="keyValuePairs">key:model屬性,value:Execl欄位</param>
        /// <returns></returns>
        public static List<TResult> ToList<TResult>(this DataTable dt,Dictionary<string,string> keyValuePairs) where TResult : class, new()
        {
            //創建一個屬性的列表
            List<PropertyInfo> prlist = new List<PropertyInfo>();
            //獲取TResult的類型實例  反射的入口 
            Type t = typeof(TResult);
            //獲得TResult 的所有的Public 屬性並找出TResult屬性和DataTable的列名稱相同的屬性(PropertyInfo) 並加入到屬性列表
            Array.ForEach<PropertyInfo>(t.GetProperties(), p => 
                                            {
                                                if (keyValuePairs.ContainsKey(p.Name))
                                                {
                                                    if (dt.Columns.IndexOf(keyValuePairs[p.Name]) != -1)
                                                    {
                                                        prlist.Add(p);
                                                    }
                                                }
                                            }
                                            );
            if (prlist?.Count != 0)
            {
                //創建返回的集合
                List<TResult> oblist = new List<TResult>();
                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        //創建TResult的實例
                        TResult ob = new TResult();
                        //找到對應的數據  並賦值
                        prlist.ForEach(p => {
                            if (row[keyValuePairs[p.Name]] != DBNull.Value)
                            {
                                var type = p.PropertyType.Name;

                                var value = new object();

                                switch (type)
                                {

                                    case "Nullable`1":
                                        //value = type == "Nullable`1" ? Convert.ToInt32(row[keyValuePairs[p.Name]]) : row[keyValuePairs[p.Name]];
                                        int intResult = 0;
                                        DateTime datetimeResult = new DateTime();
                                        if (int.TryParse(row[keyValuePairs[p.Name]].ToString(), out intResult))
                                        {
                                            value = intResult;
                                        }
                                        else if (DateTime.TryParse(row[keyValuePairs[p.Name]].ToString(), out datetimeResult))
                                        {
                                            value = datetimeResult;
                                        }
                                        else
                                        {
                                            value = row[keyValuePairs[p.Name]];
                                        }
                                        break;
                                    case "Boolean":
                                        value = type == "Boolean" ? TranBoolean(row[keyValuePairs[p.Name]].ToString()) : row[keyValuePairs[p.Name]];
                                        break;
                                    case "String":
                                        value = row[keyValuePairs[p.Name]];
                                        break;
                                    case "DateTime":
                                        value = type == "DateTime" ? row[keyValuePairs[p.Name]].ToString() is "" ? new Nullable<DateTime>() : DateTime.Parse(row[keyValuePairs[p.Name]].ToString()) : row[keyValuePairs[p.Name]];
                                        break;
                                    default:
                                        break;
                                }


                                p.SetValue(ob, value, null);
                            }
                        });
                        //放入到返回的集合中.
                        oblist.Add(ob);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                }

                return oblist;
            }
            else
            {
                return null;
            }
            
        }
        /// <summary>
        /// 字串 Y,null,0,"" 真值判斷
        /// </summary>
        /// <param name="str"></param>
        /// <returns>Boolean</returns>
        private static Boolean TranBoolean(String str)
        {
            var result = true;
            if (!Boolean.TryParse(str ,out result)) {
                if (str == "Y" || str.ToLower()=="null" || str== String.Empty || str == "0")
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// 使用 Newtonsoft.Json 將 DataTable轉Json
        /// </summary>
        /// <param name="table">DataTable物件</param>
        /// <returns>string</returns>
        public static string ToJson(this DataTable table)
        {
            string JsonString = string.Empty;
            JsonString = JsonConvert.SerializeObject(table);
            return JsonString;
        }

        /// <summary>
        /// 清理欄位（無法轉入Excel的資料）
        /// </summary>
        /// <param name="data"></param>
        /// <param name="leagth">字串最大值</param>
        /// <returns></returns>
        public static DataTable ClearField(this DataTable data, int leagth)
        {
            foreach (DataRow row in data.Rows)
            {
                foreach (DataColumn column in data.Columns)
                {
                    if (row[column].GetType().Name == "String" && row[column].ToString().Length > leagth)
                    {
                        row[column] = row[column].ToString().Substring(0,200)+".....";
                    }
                    if (row[column].GetType().Name == "Byte[]")
                    {
                        row[column] = null;
                    }
                }
            }
            return data;
        }

        public static List<Dictionary<string,object>> TranDictionarys(this DataTable data)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (DataRow row in data.Rows)
            {
                var dir = new Dictionary<string, object>();

                foreach (DataColumn column in data.Columns)
                {
                    dir.Add($"[{column}]", row[column]);
                }
                result.Add(dir);
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Data.SqlClient;
using Asiatek.Common;
using System.Data.Common;

namespace Asiatek.DBUtility
{
    /// <summary>
    /// 将Datatable、DataSet、SqlDataReader转换为指定类型的List
    /// 作者：戴天辰
    /// </summary>
    public class ConvertToList<T>
    {
        ///// <summary>
        ///// 将DataTable转换为指定类型的List
        ///// </summary>
        //public static List<T> Convert(DataTable dt)
        //{
        //    if (dt == null)
        //        return null;
        //    List<T> tList = new List<T>();
        //    try
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            T t = Activator.CreateInstance<T>();
        //            SetPropValue(dr, t);
        //            tList.Add(t);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
        //        LogHelper.DoOtherErrorLog(msg);
        //        return null;
        //    }
        //    return tList;
        //}
        ///// <summary>
        ///// 将DataSet转换为指定类型的List
        ///// </summary>
        //public static List<T> Convert(DataSet ds, int tableIndex)
        //{
        //    if (ds == null || ds.Tables.Count == 0 || ds.Tables.Count <= tableIndex)
        //        return null;
        //    List<T> tList = new List<T>();
        //    try
        //    {
        //        foreach (DataRow dr in ds.Tables[tableIndex].Rows)
        //        {
        //            T t = Activator.CreateInstance<T>();
        //            SetPropValue(dr, t);
        //            tList.Add(t);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
        //        LogHelper.DoOtherErrorLog(msg);
        //        return null;
        //    }
        //    return tList;
        //}
        ///// <summary>
        ///// 将SqlDataReader转换为指定类型的List
        ///// </summary>
        //public static List<T> Convert(SqlDataReader dr)
        //{
        //    if (dr == null || !dr.HasRows)
        //        return null;
        //    List<T> tList = new List<T>();
        //    try
        //    {
        //        while (dr.Read())
        //        {
        //            T t = Activator.CreateInstance<T>();
        //            SetPropValue(dr, t);
        //            tList.Add(t);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
        //        LogHelper.DoOtherErrorLog(msg);
        //        return null;
        //    }
        //    finally
        //    {
        //        dr.Close();
        //    }
        //    return tList;
        //}
        ///// <summary>
        ///// 将IList接口转换为DataSet
        ///// </summary>
        //public static DataSet ToDataSet(IList list)
        //{
        //    DataSet result = new DataSet();
        //    DataTable _DataTable = new DataTable();
        //    try
        //    {
        //        if (list.Count > 0)
        //        {
        //            PropertyInfo[] propertys = list[0].GetType().GetProperties();
        //            foreach (PropertyInfo pi in propertys)
        //            {
        //                _DataTable.Columns.Add(pi.Name, pi.PropertyType);
        //            }

        //            for (int i = 0; i < list.Count; i++)
        //            {
        //                ArrayList tempList = new ArrayList();
        //                foreach (PropertyInfo pi in propertys)
        //                {
        //                    object obj = pi.GetValue(list[i], null);
        //                    tempList.Add(obj);
        //                }
        //                object[] array = tempList.ToArray();
        //                _DataTable.LoadDataRow(array, true);
        //            }
        //        }
        //        result.Tables.Add(_DataTable);
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
        //        LogHelper.DoOtherErrorLog(msg);
        //        return null;
        //    }
        //    return result;
        //}


        /// <summary>
        /// 将DataTable转换为指定类型的List
        /// </summary>
        public static List<T> Convert(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }
            List<T> tList = new List<T>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    T t = Activator.CreateInstance<T>();
                    SetPropValue(dr, t);
                    tList.Add(t);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
                return null;
            }
            return tList;
        }
        /// <summary>
        /// 将DataSet转换为指定类型的List
        /// </summary>
        public static List<T> Convert(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            List<T> tList = new List<T>();
            try
            {

                foreach (DataRow dr in ds.Tables[tableIndex].Rows)
                {
                    T t = Activator.CreateInstance<T>();
                    SetPropValue(dr, t);
                    tList.Add(t);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
                return null;
            }
            return tList;
        }
        /// <summary>
        /// 将SqlDataReader转换为指定类型的List
        /// 发生异常返回空集合
        /// </summary>
        public static List<T> Convert(DbDataReader dr)
        {
            List<T> tList = new List<T>();
            try
            {
                while (dr.Read())
                {
                    T t = Activator.CreateInstance<T>();
                    SetPropValue(dr, t);
                    tList.Add(t);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
                return null;
            }
            finally
            {
                dr.Close();
            }
            return tList;
        }



        /// <summary>
        /// 检查SqlDataReader中是否包含指定的列名
        /// </summary>
        private static bool CheckColumnNameExist(DataTable dt, string columnName)
        {

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ColumnName"].ToString() == columnName)
                {
                    return true;
                }
            }
            return false;
        }


        static void SetPropValue(DataRow dr, Object t)
        {
            IEnumerable<PropertyInfo> pary = t.GetType().GetProperties().Where(p => p.CanWrite);

            foreach (PropertyInfo p in pary)
            {
                //某些属性为Nullable，这种要获取实际的类型
                //否则可能出现一种情况
                //比如属性是 int? ，但是数据库中是tinyint，这时如果有值则无法转换 
                Type safeType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                var ienumerableType = safeType.GetInterface("IEnumerable", false);

                if ((safeType != typeof(string) && safeType != typeof(byte[])) && ienumerableType != null)//不是字符串且不是字节数组但却是实现了IEnumerable接口的类型
                {
                    continue;
                }

                if (safeType.IsClass || safeType.IsValueType)
                {
                    var vtProps = safeType.GetProperties().Where(pi => pi.CanWrite);
                    if (vtProps.Count() > 0)//包含可写属性的复杂类型
                    {
                        //这种值类型要特殊对待
                        Object obj = Assembly.GetAssembly(safeType).CreateInstance(safeType.FullName);
                        SetPropValue(dr, obj);
                        p.SetValue(t, obj, null);
                        continue;
                    }
                }

                //判断是否包含当前列
                if (dr.Table.Columns.Contains(p.Name))
                {
                    if (dr[p.Name] is DBNull)
                    {
                        p.SetValue(t, null, null);
                        continue;
                    }
                    if (safeType.IsEnum)//枚举类型
                    {
                        p.SetValue(t, Enum.ToObject(safeType, dr[p.Name]), null);
                    }
                    else//其他简单类型
                    {
                        var val = dr[p.Name];
                        val = System.Convert.ChangeType(val, safeType);//注意这里转换为safeType
                        p.SetValue(t, val, null);
                    }
                }
            }


        }


        static void SetPropValue(DbDataReader dr, Object t)
        {
            DataTable dt = dr.GetSchemaTable();//描述datareader列元数据的DataTable
            IEnumerable<PropertyInfo> pary = t.GetType().GetProperties().Where(p => p.CanWrite);
            foreach (PropertyInfo p in pary)
            {
                //某些属性为Nullable，这种要获取实际的类型
                //否则可能出现一种情况
                //比如属性是 int? ，但是数据库中是tinyint，这时如果有值则无法转换 
                Type safeType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                var ienumerableType = safeType.GetInterface("IEnumerable", false);


                if ((safeType != typeof(string) && safeType != typeof(byte[])) && ienumerableType != null)//不是字符串且不是字节数组但却是实现了IEnumerable接口的类型
                {
                    continue;
                }

                if (safeType.IsClass || safeType.IsValueType)
                {
                    var vtProps = safeType.GetProperties().Where(pi => pi.CanWrite);
                    if (vtProps.Count() > 0)//包含可写属性的复杂类型
                    {
                        //这种值类型要特殊对待
                        Object obj = Assembly.GetAssembly(safeType).CreateInstance(safeType.FullName);
                        SetPropValue(dr, obj);
                        p.SetValue(t, obj, null);
                        continue;
                    }
                }

                //判断是否包含当前列
                if (CheckColumnNameExist(dt, p.Name))
                {
                    if (dr[p.Name] is DBNull)
                    {
                        p.SetValue(t, null, null);
                        continue;
                    }
                    if (safeType.IsEnum)//枚举类型
                    {
                        var val = dr.GetValue(dr.GetOrdinal(p.Name));
                        p.SetValue(t, Enum.ToObject(safeType, val), null);
                    }
                    else//其他简单类型
                    {
                        var val = dr.GetValue(dr.GetOrdinal(p.Name));
                        val = System.Convert.ChangeType(val, safeType);//注意这里转换为safeType
                        p.SetValue(t, val, null);
                    }
                }
            }



        }


    }
}

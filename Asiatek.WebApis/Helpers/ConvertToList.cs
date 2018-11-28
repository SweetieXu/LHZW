using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Asiatek.WebApis.Helpers
{
    /// <summary>
    /// 将DataTable或DbDataReader转换为类型为T的List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConvertToList<T>
    {

        /// <summary>
        /// 将DataTable转换为指定类型的List
        /// </summary>
        /// <param name="dt">待转换的包含数据的DataTable</param>
        /// <param name="ct">取消标记</param>
        /// <returns></returns>
        public static List<T> Convert(DataTable dt)
        {
            if (dt == null)
            {
                throw new ArgumentNullException("dt");
            }
            List<T> tList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = Activator.CreateInstance<T>();
                SetPropValue(dr, t);
                tList.Add(t);
            }
            return tList;
        }

        /// <summary>
        /// 将DbDataReader转换为指定类型的List
        /// </summary>
        /// <param name="dr">待转换的包含数据的DbDataReader</param>
        /// <param name="ct">取消标记</param>
        /// <returns></returns>
        public async static Task<List<T>> ConvertAsync(DbDataReader dr, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (dr == null)
            {
                throw new ArgumentNullException("dr");
            }
            List<T> tList = new List<T>();
            try
            {
                while (await dr.ReadAsync(ct).ConfigureAwait(false))
                {
                    ct.ThrowIfCancellationRequested();
                    T t = Activator.CreateInstance<T>();
                    SetPropValue(dr, t);
                    tList.Add(t);
                }
            }
            finally
            {
                dr.Close();
            }
            return tList;
        }

        /// <summary>
        /// 将DbDataReader转换为指定类型的List
        /// </summary>
        /// <param name="dr">待转换的包含数据的DbDataReader</param>
        /// <returns></returns>
        public static Task<List<T>> ConvertAsync(DbDataReader dr)
        {
            return ConvertAsync(dr, CancellationToken.None);
        }

        /// <summary>
        /// 检查是否包含指定的列名
        /// </summary>
        static bool CheckColumnNameExist(DataTable dt, string columnName)
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

                if (safeType != typeof(string) && ienumerableType != null)//不是字符串但却是实现了IEnumerable接口的类型
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

                if (safeType != typeof(string) && ienumerableType != null)//不是字符串但却是实现了IEnumerable接口的类型
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
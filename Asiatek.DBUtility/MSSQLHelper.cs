using Asiatek.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Asiatek.DBUtility
{


    public abstract class MSSQLHelper
    {
        private static readonly string ConnStr = ConfigurationManager.ConnectionStrings["SqlConnStr"].ConnectionString;




        /// <summary>
        /// 将Datatable批量插入指定表
        /// </summary>
        /// <param name="dt">待插入的包含数据的Datable</param>
        /// <param name="destinationTatbleName">目标表名</param>
        /// <returns>true：成功；false：失败</returns>
        public static bool ImportToDataBase(DataTable dt, string destinationTatbleName, params SqlBulkCopyColumnMapping[] columnMappingList)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                using (SqlBulkCopy bkc = new SqlBulkCopy(ConnStr, SqlBulkCopyOptions.UseInternalTransaction | SqlBulkCopyOptions.CheckConstraints))
                {
                    if (columnMappingList != null)
                    {
                        foreach (var item in columnMappingList)
                        {
                            bkc.ColumnMappings.Add(item);
                        }
                    }
                    bkc.DestinationTableName = destinationTatbleName;
                    bkc.WriteToServer(dt);
                    return true;
                }
            }
        }
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters, 60);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception ex)
            {
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return -1;
            }
        }
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters, 60);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return null;
            }
        }
        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters, 60);
                    SqlDataAdapter adp = new SqlDataAdapter();
                    adp.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return null;
            }
        }
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(ConnStr);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters, 180);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return dr;
            }
            catch (Exception ex)
            {
                conn.Close();
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return null;
            }
        }
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnStr))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters, 60);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception ex)
            {
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return null;
            }
        }
        public static bool ExecuteTransaction(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection myConnection = new SqlConnection(ConnStr);
            myConnection.Open();
            SqlTransaction myTrans = myConnection.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = myConnection;
            cmd.Transaction = myTrans;

            try
            {
                if (commandParameters == null)
                {
                    PrepareCommand(cmd, myConnection, null, cmdType, cmdText, null, 60);
                }
                else
                {
                    PrepareCommand(cmd, myConnection, null, cmdType, cmdText, commandParameters, 60);
                }

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                myTrans.Commit();
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return false;
            }
            finally
            {
                myConnection.Close();
            }

            return true;
        }
        public static bool ExecuteTransaction(CommandType cmdType, string[] cmdTexts, params SqlParameter[][] commandParameters)
        {
            SqlConnection myConnection = new SqlConnection(ConnStr);
            myConnection.Open();
            SqlTransaction myTrans = myConnection.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = myConnection;
            cmd.Transaction = myTrans;

            try
            {
                for (int i = 0; i < cmdTexts.Length; i++)
                {
                    if (commandParameters == null)
                    {
                        PrepareCommand(cmd, myConnection, null, cmdType, cmdTexts[i], null, 60);
                    }
                    else
                    {
                        PrepareCommand(cmd, myConnection, null, cmdType, cmdTexts[i], commandParameters[i], 60);
                    }

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return false;
            }
            finally
            {
                myConnection.Close();
            }

            return true;
        }
        public static bool ExecuteTransaction(CommandType cmdType, string[] cmdTexts, out int count, params SqlParameter[][] commandParameters)
        {
            count = 0;
            SqlConnection myConnection = new SqlConnection(ConnStr);
            myConnection.Open();
            SqlTransaction myTrans = myConnection.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = myConnection;
            cmd.Transaction = myTrans;

            try
            {
                for (int i = 0; i < cmdTexts.Length; i++)
                {
                    PrepareCommand(cmd, myConnection, null, cmdType, cmdTexts[i], commandParameters[i], 60);
                    count += cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return false;
            }
            finally
            {
                myConnection.Close();
            }

            return true;
        }
        public static int ExecuteIdentityTransaction(CommandType cmdType, string[] cmdTexts, params SqlParameter[][] commandParameters)
        {
            int id = 0;
            SqlConnection myConnection = new SqlConnection(ConnStr);
            myConnection.Open();
            SqlTransaction myTrans = myConnection.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = myConnection;
            cmd.Transaction = myTrans;

            try
            {
                PrepareCommand(cmd, myConnection, null, cmdType, cmdTexts[0], commandParameters[0], 60);
                id = Convert.ToInt32(cmd.ExecuteScalar());

                for (int i = 1; i < cmdTexts.Length; i++)
                {
                    PrepareCommand(cmd, myConnection, null, cmdType, cmdTexts[i], commandParameters[i], 60);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return 0;
            }
            finally
            {
                myConnection.Close();
            }

            return id;
        }
        public static int ExecuteIdentityIncludeTransaction(CommandType cmdType, string[] cmdTexts, params SqlParameter[][] commandParameters)
        {
            int id = 0;
            SqlConnection myConnection = new SqlConnection(ConnStr);
            myConnection.Open();
            SqlTransaction myTrans = myConnection.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = myConnection;
            cmd.Transaction = myTrans;

            try
            {
                PrepareCommand(cmd, myConnection, null, cmdType, cmdTexts[0], commandParameters[0], 60);
                id = Convert.ToInt32(cmd.ExecuteScalar());

                for (int i = 1; i < cmdTexts.Length; i++)
                {
                    commandParameters[i][0].Value = id;
                    PrepareCommand(cmd, myConnection, null, cmdType, cmdTexts[i], commandParameters[i], 60);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                myTrans.Commit();
            }
            catch (Exception ex)
            {
                myTrans.Rollback();
                string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoDataBaseErrorLog(errorInfo);
                return 0;
            }
            finally
            {
                myConnection.Close();
            }

            return id;
        }
        public static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms, int timeout)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandTimeout = timeout;
            cmd.CommandText = cmdText;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
        /// <summary>
        /// 批量出入信号
        /// </summary>
        /// <param name="dtSource">批量插入的数据</param>
        /// <param name="desTbName">表名</param>
        /// <param name="columnMappingList"></param>
        /// <returns></returns>
        public  static void BulkCopyAsync(DataTable dtSource, string desTbName,  params SqlBulkCopyColumnMapping[] columnMappingList)
        {
            if (dtSource == null)
            {
                throw new ArgumentNullException("dtSource");
            }
            using (SqlBulkCopy bkc = new SqlBulkCopy(ConnStr, SqlBulkCopyOptions.UseInternalTransaction | SqlBulkCopyOptions.CheckConstraints))
            {
                bkc.DestinationTableName = desTbName;
                if (columnMappingList != null)
                {
                    foreach (var item in columnMappingList)
                    {
                        bkc.ColumnMappings.Add(item);
                    }
                }
                     bkc.WriteToServer(dtSource);
            }
        }
    }
}

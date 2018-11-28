using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Asiatek.WebApis.Helpers
{
    public static class MSDBHelper
    {
        #region 变量
        /// <summary>
        /// 连接字符串
        /// </summary>
        public readonly static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["SqlConnStr"].ConnectionString;
        #endregion

        #region 方法

        #region 返回受影响的行数
        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParas)
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder();
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SetCmd(cmd, conn, null, cmdText, cmdParas);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public async static Task<int> ExecuteNonQueryAsync(string cmdText, CommandType cmdType, CancellationToken ct, params SqlParameter[] cmdParas)
        {
            ct.ThrowIfCancellationRequested();
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    await SetCmdAsync(cmd, conn, null, cmdType, cmdText, cmdParas, ct).ConfigureAwait(false);
                    return await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
                }
            }
        }

        public async static Task<int> ExecuteNonQueryAsync(string cmdText, CommandType cmdType, params SqlParameter[] cmdParas)
        {
            return await ExecuteNonQueryAsync(cmdText, cmdType, CancellationToken.None, cmdParas).ConfigureAwait(false);
        }
        #endregion


        #region 返回查询结果的第一行第一列
        public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParas)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    SetCmd(cmd, conn, null, cmdText, cmdParas);
                    return cmd.ExecuteScalar();
                }
            }
        }

        public async static Task<object> ExecuteScalarAsync(string cmdText, CommandType cmdType, CancellationToken ct, params SqlParameter[] cmdParas)
        {
            ct.ThrowIfCancellationRequested();
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    await SetCmdAsync(cmd, conn, null, cmdType, cmdText, cmdParas, ct).ConfigureAwait(false);
                    return await cmd.ExecuteScalarAsync(ct).ConfigureAwait(false);
                }
            }
        }

        public async static Task<object> ExecuteScalarAsync(string cmdText, CommandType cmdType, params SqlParameter[] cmdParas)
        {
            return await ExecuteScalarAsync(cmdText, cmdType, CancellationToken.None, cmdParas).ConfigureAwait(false);
        }
        #endregion


        #region 返回DataReader
        public async static Task<SqlDataReader> ExecuteReaderAsync(string cmdText, CommandType cmdType, CancellationToken ct, params SqlParameter[] cmdParas)
        {
            ct.ThrowIfCancellationRequested();
            SqlConnection conn = new SqlConnection(CONNECTIONSTRING);
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    await SetCmdAsync(cmd, conn, null, cmdType, cmdText, cmdParas, ct).ConfigureAwait(false);
                    return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection, ct).ConfigureAwait(false);
                }
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public async static Task<SqlDataReader> ExecuteReaderAsync(string cmdText, CommandType cmdType, params SqlParameter[] cmdParameters)
        {
            return await ExecuteReaderAsync(cmdText, cmdType, CancellationToken.None, cmdParameters).ConfigureAwait(false);
        }
        #endregion


        #region 返回DataTable
        public async static Task<DataTable> ExecuteDataTableAsync(string cmdText, CommandType cmdType, CancellationToken ct, params SqlParameter[] cmdParas)
        {
            ct.ThrowIfCancellationRequested();
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    await SetCmdAsync(cmd, conn, null, cmdType, cmdText, cmdParas, ct).ConfigureAwait(false);
                    var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection, ct).ConfigureAwait(false);
                    ct.ThrowIfCancellationRequested();
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    cmd.Parameters.Clear();
                    return dt;
                }
            }
        }

        public async static Task<DataTable> ExecuteDataTableAsync(string cmdText, CommandType cmdType, params SqlParameter[] cmdParas)
        {
            return await ExecuteDataTableAsync(cmdText, cmdType, CancellationToken.None, cmdParas).ConfigureAwait(false);
        }
        #endregion


        #region 事务
        public static bool ExecuteTransaction(string[] cmdTexts, params SqlParameter[][] cmdParas)
        {
            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i != cmdTexts.Length; i++)
                        {
                            using (SqlCommand cmd = new SqlCommand())
                            {
                                SetCmd(cmd, conn, trans, cmdTexts[i], cmdParas[i]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public async static Task ExecuteTransactionAsync(CommandType cmdType, CancellationToken ct, string[] cmdTexts, params SqlParameter[][] cmdParas)
        {
            ct.ThrowIfCancellationRequested();
            SqlConnection conn = new SqlConnection(CONNECTIONSTRING);
            SqlTransaction trans = null;
            bool commitFlag = false;
            try
            {
                await conn.OpenAsync(ct);
                trans = conn.BeginTransaction(); ;
                for (int i = 0; i < cmdTexts.Length; i++)
                {
                    ct.ThrowIfCancellationRequested();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        await SetCmdAsync(cmd, conn, trans, cmdType, cmdTexts[i], cmdParas[i], ct).ConfigureAwait(false);
                        await cmd.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
                    }
                }
                ct.ThrowIfCancellationRequested();
                commitFlag = true;
                trans.Commit();
            }
            catch
            {
                if (commitFlag && trans != null)
                {
                    trans.Rollback();
                }
                throw;
            }
            finally
            {
                if (trans != null)
                {
                    trans.Dispose();
                }
                conn.Close();
            }
        }

        public async static Task ExecuteTransactionAsync(CommandType cmdType, string[] cmdTexts, params SqlParameter[][] cmdParas)
        {
            await ExecuteTransactionAsync(cmdType, CancellationToken.None, cmdTexts, cmdParas).ConfigureAwait(false);
        }
        #endregion


        #region 批量插入
        public static void BulkCopy(DataTable dtSource, string desTbName, params SqlBulkCopyColumnMapping[] columnMappingList)
        {
            if (dtSource == null)
            {
                throw new ArgumentNullException("dtSource");
            }
            using (SqlBulkCopy bkc = new SqlBulkCopy(CONNECTIONSTRING, SqlBulkCopyOptions.UseInternalTransaction | SqlBulkCopyOptions.CheckConstraints))
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

        public async static Task BulkCopyAsync(DataTable dtSource, string desTbName, CancellationToken ct, params SqlBulkCopyColumnMapping[] columnMappingList)
        {
            ct.ThrowIfCancellationRequested();
            if (dtSource == null)
            {
                throw new ArgumentNullException("dtSource");
            }
            using (SqlBulkCopy bkc = new SqlBulkCopy(CONNECTIONSTRING, SqlBulkCopyOptions.UseInternalTransaction | SqlBulkCopyOptions.CheckConstraints))
            {
                bkc.DestinationTableName = desTbName;
                if (columnMappingList != null)
                {
                    foreach (var item in columnMappingList)
                    {
                        ct.ThrowIfCancellationRequested();
                        bkc.ColumnMappings.Add(item);
                    }
                }
                await bkc.WriteToServerAsync(dtSource, ct).ConfigureAwait(false);
            }
        }


        public async static Task BulkCopyAsync(DataTable dtSource, string desTbName, params SqlBulkCopyColumnMapping[] columnMappingList)
        {
            await BulkCopyAsync(dtSource, desTbName, CancellationToken.None, columnMappingList).ConfigureAwait(false);
        }
        #endregion


        #region 其他
        /// <summary>
        /// 打开连接并设置Command对象
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <param name="ct"></param>
        /// <param name="cmdTimeout"></param>
        /// <returns></returns>
        async static Task SetCmdAsync(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms, CancellationToken ct, int cmdTimeout = 180)
        {
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync(ct).ConfigureAwait(false);
            }
            cmd.Connection = conn;
            cmd.CommandTimeout = cmdTimeout;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    ct.ThrowIfCancellationRequested();
                    cmd.Parameters.Add(parm);
                }
            }
        }

        static void SetCmd(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms, int cmdTimeout = 180)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandTimeout = cmdTimeout;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion
        #endregion
    }
}
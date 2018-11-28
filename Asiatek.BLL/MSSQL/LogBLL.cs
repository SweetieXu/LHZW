using Asiatek.DBUtility;
using Asiatek.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;

namespace Asiatek.BLL.MSSQL
{
    public class LogBLL
    {

        /// <summary>
        /// 记录新增、删除、修改操作的日志到数据库
        /// </summary>
        /// <param name="log"></param>
        public static void DoOperationLog(LogModel log)
        {
            string sql = @"INSERT INTO dbo.OperationLogs(OperationInfo ,UserID ,OperationType,AreaName,ControllerName,ActionName,FunctionName) VALUES ( @operationInfo,@userID,@operationType,@areaName,@controllerName,@actionName,@functionName)";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@operationInfo",SqlDbType.NVarChar,-1),
                new SqlParameter("@userID",SqlDbType.Char,36),
                new SqlParameter("@operationType",SqlDbType.NVarChar,20),
                new SqlParameter("@areaName",SqlDbType.NVarChar,20),
                new SqlParameter("@controllerName",SqlDbType.NVarChar,50),
                new SqlParameter("@actionName",SqlDbType.NVarChar,50),
                new SqlParameter("@functionName",SqlDbType.NVarChar,50),
            };
            paras[0].Value = log.OperationInfo;
            paras[1].Value = log.UserID;
            paras[2].Value = log.OperationType;
            paras[3].Value = log.AreaName;
            paras[4].Value = log.ControllerName;
            paras[5].Value = log.ActionName;
            paras[6].Value = log.FunctionName;


            MSSQLHelper.ExecuteNonQuery(System.Data.CommandType.Text, sql, paras.ToArray());
        }

    }


}

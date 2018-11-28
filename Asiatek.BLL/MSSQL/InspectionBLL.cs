using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Asiatek.BLL.MSSQL
{
    /// <summary>
    /// 查岗相关
    /// </summary>
    public class InspectionBLL
    {
        /// <summary>
        /// 根据单位ID获取24小时内未回复的针对该单位的查岗信息
        /// </summary>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static List<InspectionListModel> GetUserInspectionInfos(int strucID)
        {
            //用单位的“业户经营许可证号”去匹配该公司的查岗信息
            string sql = @"SELECT  CAST(MSG_ID AS BIGINT) AS ID ,
        MSG_CONTENT AS Content ,
         CONVERT(VARCHAR,CREATE_TIME,121) AS CheckDateTime,
       PLATNAMR AS PlatformName
FROM    InspectionServer.TMS.dbo.TB_PLATFORM_CHECKMSG tpc
INNER JOIN dbo.Structures s ON tpc.OBJECT_ID=s.LicenseNo
WHERE   FLAG = 0
        AND DATEDIFF(hour, CREATE_TIME, GETDATE()) <= 24
       AND s.ID= @SID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(){
                    ParameterName="@SID",
                    Value=strucID,
                    SqlDbType=SqlDbType.Int,
                }
            };
            return ConvertToList<InspectionListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

        /// <summary>
        /// 获取24小时内未回复的针对平台的查岗信息
        /// </summary>
        /// <returns></returns>
        public static List<InspectionListModel> GetPlatformInspectionInfos()
        {
            string sql = @"SELECT  CAST(MSG_ID AS BIGINT) AS ID ,
                    MSG_CONTENT AS Content ,
                      CONVERT(VARCHAR,CREATE_TIME,121) AS CheckDateTime,
                   PLATNAMR AS PlatformName
            FROM    InspectionServer.TMS.dbo.TB_PLATFORM_CHECKMSG tpc
            WHERE   FLAG = 0 AND OBJECT_TYPE=1 AND DATEDIFF(hour, CREATE_TIME, GETDATE()) <= 24";
            //            string sql = @"SELECT  CAST(MSG_ID AS BIGINT) AS ID ,
            //        MSG_CONTENT AS Content ,
            //          CONVERT(VARCHAR,CREATE_TIME,121) AS CheckDateTime,
            //       PLATNAMR AS PlatformName
            //FROM    InspectionServer.TMS.dbo.TB_PLATFORM_CHECKMSG tpc
            //WHERE   OBJECT_TYPE=1";
            return ConvertToList<InspectionListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }


        public static SelectResult<InspectionModel> GetInspectionInfoByID(long ID)
        {
            string sql = @"SELECT  OBJECT_TYPE AS ObjType ,
        OBJECT_ID AS ObjID ,
        AccessCode ,
        Version ,
       CAST(M1 AS INT) M1 ,
        CAST(IA1 AS INT) IA1 ,
        CAST(IC1 AS INT) IC1 ,
        FLAG,
       PLATNAMR AS PlatformName 
FROM    InspectionServer.TMS.dbo.TB_PLATFORM_CHECKMSG
WHERE   MSG_ID = @ID";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter(){
                    ParameterName="@ID",
                    Value=ID.ToString(),
                },
            };

            List<InspectionModel> list = ConvertToList<InspectionModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            InspectionModel data = null;
            string msg = string.Empty;

            if (list == null)
            {
                msg = PromptInformation.DBError;
            }
            else if (list.Count == 0)
            {
                msg = PromptInformation.NotExists;
            }
            else
            {
                data = list[0];
            }
            return new SelectResult<InspectionModel>()
            {
                DataResult = data,
                Message = msg
            };
        }


        /// <summary>
        /// 更新查岗信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static OperationResult UpdateInspectionState(long ID, string content)
        {
            string sql = @"UPDATE  InspectionServer.TMS.dbo.TB_PLATFORM_CHECKMSG
SET     FLAG = 1 ,
        RE_MSG_CONTENT = @content ,
        RE_TIME = GETDATE()
WHERE   MSG_ID = @ID";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter()
            {
                ParameterName = "@content",
                Value = content.Trim()
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@ID",
                Value = ID.ToString()
            });

            int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
            string msg = string.Empty;
            switch (result)
            {
                case 1:
                    msg = PromptInformation.OperationSuccess;
                    break;
                case 0:
                    msg = PromptInformation.NotExists;
                    break;
                case -1:
                    msg = PromptInformation.DBError;
                    break;
            }
            return new OperationResult()
            {
                Success = result > 0,
                Message = msg
            };
        }




        public static OperationResult DoReplyLog(long msgID, int userID, string resultMsg, bool success)
        {
            //TODO:待更新回复人
            string sql = @"INSERT INTO dbo.InspectionReplyLogs( MSGID ,ReplyUserID ,ResultMsg ,Result) VALUES  ( @MSGID ,@ReplyUserID ,@ResultMsg ,@Result )";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter()
            {
                ParameterName = "@MSGID",
                Value = msgID
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@ReplyUserID",
                Value = userID
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@ResultMsg",
                Value = resultMsg
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@Result",
                Value = success
            });

            int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
            string msg = string.Empty;
            switch (result)
            {
                case 1:
                    msg = PromptInformation.OperationSuccess;
                    break;
                case 0:
                    msg = PromptInformation.NotExists;
                    break;
                case -1:
                    msg = PromptInformation.DBError;
                    break;
            }
            return new OperationResult()
            {
                Success = result > 0,
                Message = msg
            };
        }
    }
}

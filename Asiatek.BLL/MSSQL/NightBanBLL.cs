using Asiatek.AjaxPager;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Asiatek.BLL.MSSQL
{
    public class NightBanBLL
    {
        #region   查询
        public static AsiatekPagedList<NightBanListModel> GetPagedNightBan(NightBanSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","[dbo].[NightBan]"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","ID DESC"),
                new SqlParameter("@showColumns",@"[ID],[NightBanName],[NightBanAddress] ,[StartTime]
                                                                                ,[EndTime],[IsEnabled]"),
            };

            string conditionStr = " Status = 0 ";
            if (!string.IsNullOrWhiteSpace(model.NightBanName))
            {
                conditionStr += " AND NightBanName LIKE '%" + model.NightBanName + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.NightBanAddress))
            {
                conditionStr += " AND NightBanAddress LIKE '%" + model.NightBanAddress + "%'";
            }

            if (model.IsEnabled != -1)
            {
                conditionStr += " AND IsEnabled =" + model.IsEnabled;
            }

            if (!string.IsNullOrWhiteSpace(conditionStr))
            {
                paras.Add(new SqlParameter("@conditionStr", conditionStr));
            }

            paras.Add(new SqlParameter()
            {
                ParameterName = "@totalItemCount",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Int
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@newCurrentPage",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Int
            });
            List<NightBanListModel> list = ConvertToList<NightBanListModel>.
                                      Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region   新增
        public static OperationResult AddNightBan(NightBanEditModel model)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@NightBanName",SqlDbType.NVarChar),
                new SqlParameter("@NightBanAddress",SqlDbType.NVarChar),
                new SqlParameter("@StartTime",SqlDbType.Time),
                new SqlParameter("@EndTime",SqlDbType.Time),
                new SqlParameter("@IsEnabled",SqlDbType.Bit),
                new SqlParameter("@IsDefault",SqlDbType.Bit),
                new SqlParameter("@CreateUserID",SqlDbType.Int)
            };

            paras[0].Value = model.NightBanName.Trim();
            paras[1].Value = model.NightBanAddress.Trim();
            paras[2].Value = model.StartTime;
            paras[3].Value = model.EndTime;
            paras[4].Value = model.IsEnabled;
            paras[5].Value = false;
            paras[6].Value = model.CreateUserID;

            #region  SQL
            string sql = @"INSERT INTO [dbo].[NightBan]([NightBanName] ,[NightBanAddress] ,[StartTime] ,[EndTime]
                                    ,[IsEnabled] ,[IsDefault],[CreateUserID],[CreateTime] ,[Status]) VALUES 
                                   (@NightBanName,@NightBanAddress,@StartTime,@EndTime,@IsEnabled,@IsDefault,@CreateUserID,
                                   GETDATE(),0)";
            #endregion

            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region   编辑
        public static SelectResult<NightBanEditModel> GetNightBanByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @" SELECT [ID],[NightBanName],[NightBanAddress],[StartTime],[EndTime]
                                     ,[IsEnabled] FROM NightBan WHERE ID=@ID";
            List<NightBanEditModel> list = ConvertToList<NightBanEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            NightBanEditModel data = null;
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
            return new SelectResult<NightBanEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }


        public static OperationResult EditNightBan(NightBanEditModel model)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@NightBanName",SqlDbType.NVarChar),
                new SqlParameter("@NightBanAddress",SqlDbType.NVarChar),
                new SqlParameter("@StartTime",SqlDbType.Time),
                new SqlParameter("@EndTime",SqlDbType.Time),
                new SqlParameter("@IsEnabled",SqlDbType.Bit),
                new SqlParameter("@UpdateUserID",SqlDbType.Int)
            };

            paras[0].Value = model.ID;
            paras[1].Value = model.NightBanName.Trim();
            paras[2].Value = model.NightBanAddress.Trim();
            paras[3].Value = model.StartTime;
            paras[4].Value = model.EndTime;
            paras[5].Value = model.IsEnabled;
            paras[6].Value = model.UpdateUserID;
            #region  SQL
            string sql;
            sql = @"UPDATE  [dbo].[NightBan] SET [NightBanName] = @NightBanName ,[NightBanAddress] = @NightBanAddress
                          ,[StartTime] = @StartTime,[EndTime] = @EndTime ,[IsEnabled] = @IsEnabled ,[UpdateUserID] = @UpdateUserID
                         ,[UpdateTime] = GETDATE()  WHERE ID = @ID";
            #endregion
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
        #endregion

        #region   删除
        /// <summary>
        /// 物理删除
        /// </summary>
        public static OperationResult DeleteNightBan(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.NightBan WHERE ID=@ID";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i];
                paras[i][0] = temp;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion

        #region 唯一性校验
        /// <summary>
        /// 检查名称是否重复
        /// </summary>
        public static bool CheckNightBanExists(string nightBanName, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@NightBanName",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = nightBanName.Trim();
            paras[1].Value = id;

            string sql = "SELECT COUNT(0) FROM dbo.NightBan WHERE NightBanName = @NightBanName";
            if (id > 0)
            {
                sql += " AND ID <> @ID";
            }
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion
    }
}

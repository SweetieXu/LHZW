using Asiatek.AjaxPager;
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
    public class CLCS_ReturnOrderManageBLL
    {
        public static AsiatekPagedList<CLCS_ReturnOrderListModel> GetPagedCLCS_ReturnOrder(CLCS_ReturnOrderSearchModel model, int searchPage, int pageSize, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName"," [dbo].[CLCS_MileageManage] "),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy"," CreateTime DESC "),
                new SqlParameter("@showColumns",@"[ID]
      ,[PlateNum]
      ,[StartTime]
      ,[EndTime]
      ,[ReturnMileage]
      ,[Remark]
      ,[CreateTime]
      ,[CreateUserID] "), 
            };

            string conditionStr = "";

            #region 筛选条件
            if (!string.IsNullOrWhiteSpace(model.PlateNum))
            {
                conditionStr += " PlateNum LIKE '%" + model.PlateNum + "%'";
            }
            if (!string.IsNullOrWhiteSpace(conditionStr))
            {
                paras.Add(new SqlParameter("@conditionStr", conditionStr));
            }
            #endregion

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
            var rs = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray());
            List<CLCS_ReturnOrderListModel> list = ConvertToList<CLCS_ReturnOrderListModel>.Convert(rs);
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        public static OperationResult AddCLCS_ReturnOrder(AddCLCS_ReturnOrderModel model, int currentUserID)
        {
            #region 参数
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar),
                new SqlParameter("@StartTime",SqlDbType.DateTime),
                new SqlParameter("@EndTime",SqlDbType.DateTime),
                new SqlParameter("@ReturnMileage",SqlDbType.Float),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
            };

            paras[0].Value = model.PlateNum;
            paras[1].Value = Convert.ToDateTime(model.StartTime);
            paras[2].Value = Convert.ToDateTime(model.EndTime);
            paras[3].Value = model.ReturnMileage;
            if (string.IsNullOrEmpty(model.Remark))
            {
                paras[4].Value = DBNull.Value;
            }
            else
            {
                paras[4].Value = model.Remark;
            }
            paras[5].Value = DateTime.Now;
            paras[6].Value = currentUserID;
            #endregion


            #region  SQL
            string sql = @"INSERT INTO dbo.CLCS_MileageManage
        ( PlateNum ,
          StartTime ,
          EndTime ,
          ReturnMileage ,
          Remark ,
          CreateTime ,
          CreateUserID
        )
VALUES  ( @PlateNum , 
          @StartTime , 
          @EndTime , 
          @ReturnMileage , 
          @Remark , 
          @CreateTime , 
          @CreateUserID 
        )";
            #endregion

            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

    }
}

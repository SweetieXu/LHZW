using Asiatek.AjaxPager;
using Asiatek.DBUtility;
using Asiatek.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Asiatek.BLL.MSSQL
{
    public class TransportManagementBLL
    {
        #region   查询
        /// <summary>
        /// 查询运管所分页信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="searchPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static AsiatekPagedList<TransportManagementListModel> GetPagedTransportManagement
                                                        (TransportManagementSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","[dbo].[TransportManagement]"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","ID DESC"),
                new SqlParameter("@showColumns",@"[ID],[Code],[Name]")
            };

            string conditionStr = " Status = 0 ";
            if (!string.IsNullOrWhiteSpace(model.Code))
            {
                conditionStr += " AND Code LIKE '%" + model.Code + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                conditionStr += " AND Name LIKE '%" + model.Name + "%'";
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
            List<TransportManagementListModel> list = ConvertToList<TransportManagementListModel>.
                                      Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region 运管所下拉列表
        /// <summary>
        /// 运管所下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<TransportManagementDDLModel> GetTransportManagementDDL()
        {
            string sql = @"SELECT [ID],[Name] FROM  [dbo].[TransportManagement] WHERE Status <> 9";
            return ConvertToList<TransportManagementDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion
     
    }
}

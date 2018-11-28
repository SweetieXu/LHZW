using Asiatek.AjaxPager;
using Asiatek.Common;
using Asiatek.DBUtility;
using Asiatek.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Asiatek.BLL.MSSQL
{
    public class GateSentryRecordBLL
    {
        #region 查询
        /// <summary>
        /// 获取当前单位及其子单位下的保养记录
        /// </summary>
        /// <returns></returns>
        public static AsiatekPagedList<GateSentryRecordListModel> GetPagedGateSentryRecord(GateSentryRecordSearchModel model, int searchPage, int pageSize, int currentStrucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.GateSentryRecord "),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","InOrOutTime ASC "),
                new SqlParameter("@showColumns",@" [ID] 
      ,[CarNumber] AS PlateNum
      ,[PassGate]
      ,[InOrOut]
      ,[InOrOutTime] "),
            };

            #region 筛选条件
            string conditionStr = " ";
            if (!string.IsNullOrWhiteSpace(model.PlateNum))
            {
                conditionStr += " 1=1 AND CarNumber LIKE '%" + FontHelper.StringConvert(model.PlateNum,"1") + "%'";
            }
            else 
            {
                conditionStr += " 1!=1 ";
            }
            if (!string.IsNullOrWhiteSpace(model.StartTime))
            {
                conditionStr += " AND InOrOutTime >= '" + Convert.ToDateTime(model.StartTime) +"' ";
            }
            if (!string.IsNullOrWhiteSpace(model.EndTime))
            {
                conditionStr += " AND InOrOutTime <='" + Convert.ToDateTime(model.EndTime) + "' ";
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
            List<GateSentryRecordListModel> list = ConvertToList<GateSentryRecordListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        #endregion

        /// <summary>
        /// 根据车牌号查询车辆
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<GateSentryVehicleSearchModel> GetVehicleByName(string name)
        {
            string sql = "SELECT ID,PlateNum FROM dbo.Vehicles WHERE Status<>9 AND PlateNum LIKE @name";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NVarChar)
            };
            paras[0].Value = "%" + name + "%";
            return ConvertToList<GateSentryVehicleSearchModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }


    }
}

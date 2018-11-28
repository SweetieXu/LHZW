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
    public class MaintenanceRecordBLL
    {
        #region 查询
        /// <summary>
        /// 获取当前单位及其子单位下的保养记录
        /// </summary>
        /// <returns></returns>
        public static AsiatekPagedList<MaintenanceRecordListModel> GetPagedMaintenanceRecord(MaintenanceRecordSearchModel model, int searchPage, int pageSize, int currentStrucID)
        {
            string joinStr = string.Format(@"  INNER JOIN dbo.Vehicles ve ON ve.ID=mr.VehicleID 
  INNER JOIN dbo.Structures st ON ve.StrucID = st.ID 
  INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID({0})  AS vt ON ve.ID=vt.VID 
  INNER JOIN dbo.Users us ON us.ID = mr.CreateUser  ", currentStrucID);

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.MaintenanceRecord mr "),
                new SqlParameter("@joinStr",joinStr),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","mr.ID DESC"),
                new SqlParameter("@showColumns",@" mr.ID,mr.RecordDetails,mr.CreateTime,ve.VehicleName,ve.VIN,st.StrucName,us.UserName AS CreateUser "),
            };

            #region 筛选条件
            string conditionStr = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(model.VehicleName))
            {
                conditionStr += " AND ve.VehicleName LIKE '%" + model.VehicleName + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.VIN))
            {
                conditionStr += " AND ve.VIN LIKE '%" + model.VIN + "%'";
            }
            if (model.SearchStrucID != -1)
            {
                conditionStr += " AND ve.StrucID = " + model.SearchStrucID + "";
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
            List<MaintenanceRecordListModel> list = ConvertToList<MaintenanceRecordListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        #endregion

        #region 新增
        public static OperationResult AddMaintenanceRecord(AddMaintenanceRecordModel model, int currentUserID)
        {
           List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleID",SqlDbType.BigInt),
                new SqlParameter("@RecordDetails",SqlDbType.NVarChar),
                new SqlParameter("@CreateUser",SqlDbType.Int),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
            };

           paras[0].Value = model.VehicleID;
           if (model.RecordDetails == null)
           {
               paras[1].Value = DBNull.Value;
           }
           else
           {
               paras[1].Value = model.RecordDetails;
           }
           paras[2].Value = currentUserID;
           paras[3].Value = DateTime.Now;

           string sql = @"INSERT  INTO dbo.MaintenanceRecord
                ( [VehicleID]
      ,[RecordDetails]
      ,[CreateUser]
      ,[CreateTime] )
        VALUES  ( @VehicleID , 
                    @RecordDetails, 
                    @CreateUser, 
                    @CreateTime )";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        public static List<VehicleNameDropListModel> GetVehicleIDByVehicleName(string vehicleName, int currentStrucID)
        {
            string sql = string.Format(@"SELECT v.ID,v.VehicleName FROM dbo.Vehicles v
                INNER JOIN Func_GetStrucAndSubStrucByUserAffiliatedStrucID({0}) s ON v.StrucID = s.ID
WHERE Status<>9 and VehicleName LIKE @VehicleName", currentStrucID);
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleName",SqlDbType.NVarChar)
            };
            paras[0].Value = "%" + vehicleName + "%";
            return ConvertToList<VehicleNameDropListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

        #endregion

        #region 修改
        public static SelectResult<EditMaintenanceRecordModel> GetMaintenanceRecordByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@ID",
                    SqlDbType=SqlDbType.Int,
                },
            };
            paras[0].Value = id;
            string sql = @"SELECT mr.ID,mr.RecordDetails,ve.VehicleName
  FROM dbo.MaintenanceRecord mr 
  INNER JOIN dbo.Vehicles ve ON ve.ID=mr.VehicleID  
      WHERE mr.ID=@ID";

            List<EditMaintenanceRecordModel> list = ConvertToList<EditMaintenanceRecordModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
            EditMaintenanceRecordModel data = null;
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
            return new SelectResult<EditMaintenanceRecordModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditMaintenanceRecord(EditMaintenanceRecordModel model, int currentUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@RecordDetails",SqlDbType.NVarChar),
                new SqlParameter("@UpdateUser",SqlDbType.Int),
                new SqlParameter("@UpdateTime",SqlDbType.DateTime),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };

            paras[0].Value = model.RecordDetails;
            paras[1].Value = currentUserID;
            paras[2].Value = DateTime.Now;
            paras[3].Value = model.ID;

            #region  SQL
            string sql = @"UPDATE   dbo.MaintenanceRecord
       SET      RecordDetails = @RecordDetails ,
                UpdateUser = @UpdateUser ,
                UpdateTime = @UpdateTime 
       WHERE    ID = @ID";
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
    }
}

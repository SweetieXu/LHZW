using Asiatek.AjaxPager;
using Asiatek.Common;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Asiatek.BLL.MSSQL
{
    public class RepairRecordBLL
    {
        private static readonly string ConnStr = ConfigurationManager.ConnectionStrings["SqlConnStr"].ConnectionString;
        #region   查询
        public static AsiatekPagedList<RepairRecordModel> GetPagedRepairRecord(RepairRecordSearchModel model, int searchPage, int pageSize, int currentStrucID)
        {
            string joinStr = string.Format(@"
            INNER JOIN dbo.Vehicles v ON v.ID = r.LinkedVehicleID
            INNER JOIN Func_GetStrucAndSubStrucByUserAffiliatedStrucID({0}) s ON v.StrucID = s.ID", currentStrucID);
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","RepairRecord r"),
                new SqlParameter("@joinStr",joinStr),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","r.ID"),
                new SqlParameter("@showColumns",@"r.ID,r.PlateNum,r.DriverName,r.DriverPhone,r.RepairStartTime,r.RepairEndTime ,r.RepairType ,r.TotalPrice,r.Status,(select NickName from Users where ID=r.OperateUserID) AS OperateUserName,(select NickName from Users where ID=r.AuditUserID) AS AuditUserName"),
            };
            string conditionStr = " 1=1 ";//不查询删除和报废的
            if (!string.IsNullOrWhiteSpace(model.PlateNum))
            {
                conditionStr += " AND r.PlateNum LIKE '%" + model.PlateNum + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.Status) && model.Status != "-1")
            {
                conditionStr += " AND r.Status='" + model.Status + "'";
            }
            if (model.RepairStartTimestart != DateTime.MinValue && model.RepairStartTimeend != DateTime.MinValue && model.RepairStartTimeend >= model.RepairStartTimestart)
            {
                conditionStr += " AND r.RepairStartTime Between  '" + model.RepairStartTimestart + "' and  '" + model.RepairStartTimeend.AddDays(1) + "'";
            }
            else if (model.RepairStartTimestart != DateTime.MinValue && model.RepairStartTimeend != DateTime.MinValue && model.RepairStartTimeend < model.RepairStartTimestart)
            {
                return new AsiatekPagedList<RepairRecordModel>(new List<RepairRecordModel>(), 1, 10, 0);
            }
            else if (model.RepairStartTimestart == DateTime.MinValue && model.RepairStartTimeend != DateTime.MinValue)
            {
                conditionStr += " AND r.RepairStartTime< '" + model.RepairStartTimeend.AddDays(1) + "'";
            }
            else if (model.RepairStartTimestart != DateTime.MinValue && model.RepairStartTimeend == DateTime.MinValue)
            {
                conditionStr += " AND r.RepairStartTime> '" + model.RepairStartTimestart + "'";
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

            List<RepairRecordModel> list = ConvertToList<RepairRecordModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            foreach (var recordmodel in list)
            {
                recordmodel.RepairStartTime = Convert.ToDateTime(recordmodel.RepairStartTime).ToString("yyyy-MM-dd");
                recordmodel.RepairEndTime = Convert.ToDateTime(recordmodel.RepairEndTime).ToString("yyyy-MM-dd");
            }
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion
        #region 新增
        public static OperationResult AddRepairRecord(RepairRecordModel model, int ID)
        {
            #region  SQL
            string sql = @"
Declare @id BIGINT;
INSERT INTO [TMS2016].[dbo].[RepairRecord]
           ([PlateNum]
           ,[DriverName]
           ,[DriverPhone]
           ,[RepairStartTime]
           ,[RepairEndTime]
           ,[RepairType]
           ,[TotalPrice]
           ,[Status],[LinkedVehicleID],[Remark],OperateUserID)
     VALUES
           (@PlateNum
           ,@DriverName 
           ,@DriverPhone 
           ,@RepairStartTime 
           ,@RepairEndTime 
           ,@RepairType 
           ,@TotalPrice
           ,@Status,@LinkedVehicleID,@Remark,@OperateUserID);
set @id=SCOPE_IDENTITY();
select @id as ID;";
            #endregion
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar,10),
                new SqlParameter("@DriverName",SqlDbType.NVarChar,20),
                new SqlParameter("@DriverPhone",SqlDbType.NVarChar,20),
                new SqlParameter("@RepairType",SqlDbType.NVarChar,20),
                new SqlParameter("@RepairStartTime",SqlDbType.Date),
                 new SqlParameter("@RepairEndTime",SqlDbType.Date),
                 new SqlParameter("@TotalPrice",SqlDbType.Float),
                  new SqlParameter("@Status",SqlDbType.Int),
                    new SqlParameter("@LinkedVehicleID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar,2000),
             new SqlParameter("@OperateUserID",SqlDbType.Int)
            };
            paras[0].Value = model.PlateNum.Trim();
            paras[1].Value = string.IsNullOrWhiteSpace(model.DriverName) ? DBNull.Value : (object)model.DriverName.Trim();
            paras[2].Value = string.IsNullOrWhiteSpace(model.DriverPhone) ? DBNull.Value : (object)model.DriverPhone.Trim();
            paras[3].Value = string.IsNullOrWhiteSpace(model.RepairType) ? DBNull.Value : (object)model.RepairType.Trim();
            paras[4].Value = model.RepairStartTime;
            paras[5].Value = model.RepairEndTime;
            paras[6].Value = string.IsNullOrWhiteSpace(model.TotalPrice) ? DBNull.Value : (object)model.TotalPrice;
            paras[7].Value = 0;
            paras[8].Value = model.LinkedVehicleID;
            paras[9].Value = string.IsNullOrWhiteSpace(model.Remark) ? DBNull.Value : (object)model.Remark;
            paras[10].Value = ID;
            bool result = false;
            List<RecordID> recordid = ConvertToList<RecordID>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
            try
            {
                int id = recordid[0].ID;
                foreach (var reecorddetail in model.RepairRecordDetail)
                {
                    reecorddetail.LinkedRecordID = id;
                }
                SaveRepairRecordDetail(model.RepairRecordDetail, "RepairRecordDetail");
                result = true;
            }
            catch (Exception e)
            {
                LogHelper.DoDataBaseErrorLog("维修单详情错误记录:" + e.ToString());
                result = false;
            }
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }


        public static void SaveRepairRecordDetail(List<RepairRecordDetail> details, string tableName, bool needBackup = true)
        {
            var dt = details.ToDataTable();
            List<SqlBulkCopyColumnMapping> mapping = new List<SqlBulkCopyColumnMapping>();
            #region 数据
            mapping.Add(new SqlBulkCopyColumnMapping("ProjectName", "ProjectName"));
            mapping.Add(new SqlBulkCopyColumnMapping("PartsName", "PartsName"));
            mapping.Add(new SqlBulkCopyColumnMapping("PartsNum", "PartsNum"));
            mapping.Add(new SqlBulkCopyColumnMapping("PartsVersion", "PartsVersion"));
            mapping.Add(new SqlBulkCopyColumnMapping("Unit", "Unit"));
            mapping.Add(new SqlBulkCopyColumnMapping("Price", "Price"));
            mapping.Add(new SqlBulkCopyColumnMapping("MaterialCost", "MaterialCost"));
            mapping.Add(new SqlBulkCopyColumnMapping("TimeFee", "TimeFee"));
            mapping.Add(new SqlBulkCopyColumnMapping("LinkedRecordID", "LinkedRecordID"));
            #endregion
            MSSQLHelper.BulkCopyAsync(dt, tableName, mapping.ToArray());
        }
        #endregion
        #region 修改
        public static OperationResult EditRepairRecord(RepairRecordModel model, int ID)
        {
            #region  SQL
            string sql = @"IF exists (SELECT * FROM RepairRecord WHERE ID=@ID and Status<>1)
begin
update RepairRecord
set [PlateNum]=@PlateNum,[DriverName]=@DriverName,[DriverPhone]=@DriverPhone ,[RepairStartTime]=@RepairStartTime,
[RepairEndTime]=@RepairEndTime ,[RepairType]=@RepairType,[TotalPrice]=@TotalPrice,[Status]=@Status,LinkedVehicleID=@LinkedVehicleID,Remark=@Remark,OperateTime=getdate(),OperateUserID=@OperateUserID where ID=@ID;
delete from RepairRecordDetail where LinkedRecordID=@ID
end
";
            #endregion
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar,10),
                new SqlParameter("@DriverName",SqlDbType.NVarChar,20),
                new SqlParameter("@DriverPhone",SqlDbType.NVarChar,20),
                new SqlParameter("@RepairType",SqlDbType.NVarChar,20),
                new SqlParameter("@RepairStartTime",SqlDbType.Date),
               new SqlParameter("@RepairEndTime",SqlDbType.Date),
               new SqlParameter("@TotalPrice",SqlDbType.Float),
               new SqlParameter("@Status",SqlDbType.Int),
               new SqlParameter("@ID",SqlDbType.BigInt),
               new SqlParameter("@LinkedVehicleID",SqlDbType.Int),
               new SqlParameter("@Remark",SqlDbType.NVarChar,2000),
               new SqlParameter("@OperateUserID",SqlDbType.Int)
            };

            paras[0].Value = model.PlateNum.Trim();
            paras[1].Value = string.IsNullOrWhiteSpace(model.DriverName) ? DBNull.Value : (object)model.DriverName.Trim();
            paras[2].Value = string.IsNullOrWhiteSpace(model.DriverPhone) ? DBNull.Value : (object)model.DriverPhone.Trim();
            paras[3].Value = string.IsNullOrWhiteSpace(model.RepairType) ? DBNull.Value : (object)model.RepairType.Trim();
            paras[4].Value = model.RepairStartTime;
            paras[5].Value = model.RepairEndTime;
            paras[6].Value = string.IsNullOrWhiteSpace(model.TotalPrice) ? DBNull.Value : (object)model.TotalPrice;
            paras[7].Value = 0;
            paras[8].Value = model.ID;
            paras[9].Value = model.LinkedVehicleID;
            paras[10].Value = string.IsNullOrWhiteSpace(model.Remark) ? DBNull.Value : (object)model.Remark;
            paras[11].Value = ID;
            //bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, paras.ToArray());
            bool result = false;
            SqlConnection myConnection = new SqlConnection(ConnStr);
            myConnection.Open();
            SqlTransaction myTrans = myConnection.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = myConnection;
            cmd.Transaction = myTrans;
            SqlParameter[] commandParameters = paras.ToArray();
            try
            {
                if (commandParameters == null)
                {
                    MSSQLHelper.PrepareCommand(cmd, myConnection, null, CommandType.Text, sql, null, 60);
                }
                else
                {
                    MSSQLHelper.PrepareCommand(cmd, myConnection, null, CommandType.Text, sql, commandParameters, 60);
                }
              int i=  cmd.ExecuteNonQuery();
              cmd.Parameters.Clear();
                //内容被审核
              if (i <= 0)
              {
                  myTrans.Rollback();
                  return new OperationResult()
                  {
                      Success = false,
                      Message = PromptInformation.StatusChange
                  };
              }
              else {
                  //插入项目明细
                  foreach (var reecorddetail in model.RepairRecordDetail)
                  {
                      reecorddetail.LinkedRecordID = model.ID;
                  }
                  SaveRepairRecordDetail(model.RepairRecordDetail, "RepairRecordDetail");
                  result = true;
                  myTrans.Commit();
                
              }
            }
            catch (Exception e)
            {
                myTrans.Rollback();
                LogHelper.DoDataBaseErrorLog("维修单修改:" + e.ToString());
                result = false;
            }
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }




        public static SelectResult<RepairRecordModel> GetRecordByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@id",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT * FROM dbo.RepairRecord WHERE ID=@id";
            List<RepairRecordModel> list = ConvertToList<RepairRecordModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            RepairRecordModel data = null;
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
            string recordsql = @"select * from dbo.RepairRecordDetail where LinkedRecordID=@id";
            List<SqlParameter> paras1 = new List<SqlParameter>()
            {
                new SqlParameter("@id",SqlDbType.Int),
            };
            paras1[0].Value = id;
            List<RepairRecordDetail> detaillist = ConvertToList<RepairRecordDetail>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, recordsql, paras1.ToArray()));
            data.RepairRecordDetail = detaillist;
            data.RepairStartTime = Convert.ToDateTime(data.RepairStartTime).ToString("yyyy-MM-dd");
            data.RepairEndTime = Convert.ToDateTime(data.RepairEndTime).ToString("yyyy-MM-dd");
            return new SelectResult<RepairRecordModel>()
            {
                DataResult = data,
                Message = msg
            };
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        public static OperationResult DeleteRecord(string id)
        {
            string sqls = string.Empty;
            sqls = @"DELETE FROM dbo.RepairRecord WHERE ID=@ID;
                Delete from dbo.RepairRecordDetail where LinkedRecordID=@ID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras.ToArray());
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion


        #region 审核
        public static OperationResult AuditeRecord(string[] ids, int ID)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                //sqls[i] = @"UPDATE dbo.SensorList SET [Status]=9 WHERE TypeID=@ID";
                sqls[i] = @"IF exists (SELECT * FROM RepairRecord WHERE ID=@ID and Status<>1)
update dbo.RepairRecord set Status=1,AuditTime=getdate(),AuditUserID=@AuditUserID WHERE ID=@ID and Status<>1";
                paras[i] = new SqlParameter[2];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = Convert.ToInt32(ids[i] == "" ? "0" : ids[i]);
                SqlParameter temp1 = new SqlParameter("@AuditUserID", SqlDbType.Int);
                temp1.Value = ID;
                paras[i][0] = temp;
                paras[i][1] = temp1;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.AuditeSuccess : PromptInformation.AuditeFailed
            };
        }
        #endregion

    }

    public static class ListExtension
    {
        public static DataTable ToDataTable(this IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    Type safeType = Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType;
                    result.Columns.Add(pi.Name, safeType);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
    }
}

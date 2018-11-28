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
    public class VehicleExceptionBLL
    {
        #region 查询
        public static List<GetVehicleExceptionInfoModel> GetVehicleExceptions(string vehicleID)
        {
            string sql = @" SELECT e.ID,e.ExName,e.Remark,ve.VehicleID FROM dbo.ExceptionType e 
LEFT JOIN (SELECT * FROM dbo.VehicleCheckExList WHERE VehicleID=@vehicleID) ve ON 
e.ID=ve.ExTypeID";

            SqlParameter para = new SqlParameter("@vehicleID", int.Parse(vehicleID));

            return ConvertToList<GetVehicleExceptionInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, para));

        }
        #endregion

        #region 编辑
        public static OperationResult EditVehicleException(string checkedExIDs, string vehicleID)
        {
            var exIDs = checkedExIDs.Split(',');
            
            //删除数据库中该车辆下的异常信息
            string sqlDel = @"DELETE FROM dbo.VehicleCheckExList WHERE VehicleID=@VehicleID";

            List<SqlParameter> parasDel = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleID",SqlDbType.Int),
            };
            parasDel[0].Value = int.Parse(vehicleID);

            bool resultDel = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sqlDel, parasDel.ToArray()) > 0;
            for (int i = 0; i < exIDs.Length; i++)
            {
                if (exIDs[i] != "") {
                    //添加该车辆下的异常信息
                    string sqlAdd = @"INSERT  INTO dbo.VehicleCheckExList
        ( VehicleID , ExTypeID ) VALUES  ( @vehicleID , @ExTypeID )";
                    List<SqlParameter> parasAdd = new List<SqlParameter>()
                    {
                        new SqlParameter("@vehicleID",SqlDbType.Int),
                        new SqlParameter("@ExTypeID",SqlDbType.Int),
                    };

                    parasAdd[0].Value = int.Parse(vehicleID);
                    parasAdd[1].Value = int.Parse(exIDs[i]);

                    bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sqlAdd, parasAdd.ToArray()) > 0;
                }
            }
            
            return new OperationResult()
            {
                Success = resultDel,
                Message = resultDel ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };

        }
        #endregion
    }
}

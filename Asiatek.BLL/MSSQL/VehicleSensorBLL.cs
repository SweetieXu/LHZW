using Asiatek.AjaxPager;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Asiatek.BLL.MSSQL
{
    public class VehicleSensorBLL
    {
        #region 查询
        public static List<VehicleSensorListModel> GetVehicleSensors(long vehicleID)
        {
            //保固日期为null时  直接显示当前日期的下一个月的时间，因为前台设置了  一开始还是显示“1-01-01” 点击后才会显示当前日期下一个月
            string sql = @" SELECT s.TypeID, s.SensorName,vs.SensorType,vs.Value1,vs.Value2,DATEADD(MONTH,1,GETDATE()) AS WarrantyDate,s.Value1 AS IsUsed1,s.Value2 AS IsUsed2,0 AS IsHave 
  FROM dbo.SensorList s 
  LEFT JOIN 
  (SELECT * FROM dbo.VehicleSensor WHERE VehicleID=@vehicleID) vs ON s.TypeID=vs.SensorType 
  WHERE s.Status<>9 AND vs.WarrantyDate IS NULL 
  UNION 
   SELECT s.TypeID, s.SensorName,vs.SensorType,vs.Value1,vs.Value2,vs.WarrantyDate,s.Value1 AS IsUsed1,s.Value2 AS IsUsed2,1 AS IsHave
  FROM dbo.SensorList s 
  LEFT JOIN 
  (SELECT * FROM dbo.VehicleSensor WHERE VehicleID=@vehicleID) vs ON s.TypeID=vs.SensorType 
  WHERE s.Status<>9 AND vs.WarrantyDate IS NOT NULL";

            SqlParameter para = new SqlParameter("@vehicleID", vehicleID);

            return ConvertToList<VehicleSensorListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, para));
        }
        #endregion


        #region 删除
        /// <summary>
        /// 根据车辆编号和传感器类型批量删除（物理删除）
        /// </summary>
        public static OperationResult DeleteVehicleSensor(long vehicleID, int TypeID)
        {
            string sql = @"DELETE FROM dbo.VehicleSensor WHERE VehicleID=@VehicleID AND SensorType=@SensorType";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleID",SqlDbType.BigInt),
                new SqlParameter("@SensorType",SqlDbType.Int),
            };
            paras[0].Value = vehicleID;
            paras[1].Value = TypeID;

            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion


        #region 新增、编辑
        public static OperationResult EditVehicleSensors(VehicleSensorEditModel model)
        {
            string sqlSel = "SELECT COUNT(0) FROM dbo.VehicleSensor WHERE VehicleID=@vehicleID AND SensorType=@sensorType";
            List<SqlParameter> parasSel = new List<SqlParameter>()
            {
                new SqlParameter("@vehicleID",SqlDbType.BigInt),
                new SqlParameter("@sensorType",SqlDbType.Int),
            };
            parasSel[0].Value = model.VehicleID;
            parasSel[1].Value = model.TypeID;
            var resultSel = MSSQLHelper.ExecuteScalar(CommandType.Text, sqlSel, parasSel.ToArray());
            int count = int.Parse(resultSel.ToString());
            if (count > 0)
            {
                string sqlEdit = @"UPDATE  dbo.VehicleSensor
SET  Value1 = @value1 ,
        Value2 = @value2 ,
        WarrantyDate = @warrantyDate 
WHERE   VehicleID=@vehicleID AND SensorType=@sensorType";
                List<SqlParameter> parasEdit = new List<SqlParameter>()
            {
                new SqlParameter("@value1",SqlDbType.Float),
                new SqlParameter("@value2",SqlDbType.Float),
                new SqlParameter("@warrantyDate",SqlDbType.DateTime),
                new SqlParameter("@vehicleID",SqlDbType.BigInt),
                new SqlParameter("@sensorType",SqlDbType.Int),
            };
                if (model.Value1 == null)
                { parasEdit[0].Value = DBNull.Value; }
                else
                {
                    parasEdit[0].Value = double.Parse(model.Value1.ToString());
                }
                if (model.Value2 == null)
                { parasEdit[1].Value = DBNull.Value; }
                else
                {
                    parasEdit[1].Value = double.Parse(model.Value2.ToString());
                }
                //parasEdit[0].Value = double.Parse(model.Value1.ToString());
                //parasEdit[1].Value = double.Parse(model.Value2.ToString());
                parasEdit[2].Value = model.WarrantyDate;
                parasEdit[3].Value = model.VehicleID;
                parasEdit[4].Value = model.TypeID;

                int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sqlEdit, parasEdit.ToArray());
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
            else
            {
                string sqlAdd = @"INSERT  INTO dbo.VehicleSensor
        ( VehicleID , SensorType , Value1 , Value2 , WarrantyDate )
VALUES  ( @vehicleID , @sensorType, @value1, @value2, @warrantyDate )";
                List<SqlParameter> parasAdd = new List<SqlParameter>()
            {
                new SqlParameter("@vehicleID",SqlDbType.BigInt),
                new SqlParameter("@sensorType",SqlDbType.Int),
                new SqlParameter("@value1",SqlDbType.Float),
                new SqlParameter("@value2",SqlDbType.Float),
                new SqlParameter("@warrantyDate",SqlDbType.DateTime),
            };

                parasAdd[0].Value = model.VehicleID;
                parasAdd[1].Value = model.TypeID;
                if (model.Value1 == null)
                { parasAdd[2].Value = DBNull.Value; }
                else {
                    parasAdd[2].Value = double.Parse(model.Value1.ToString());
                }
                if (model.Value2 == null)
                { parasAdd[3].Value = DBNull.Value; }
                else
                {
                    parasAdd[3].Value = double.Parse(model.Value2.ToString());
                }
                parasAdd[4].Value = model.WarrantyDate;

                bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sqlAdd, parasAdd.ToArray()) > 0;
                return new OperationResult()
                {
                    Success = result,
                    Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
                };
            }

        }
        #endregion
    }
}

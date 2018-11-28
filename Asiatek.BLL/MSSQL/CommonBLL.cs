using Asiatek.DBUtility;
using Asiatek.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Asiatek.BLL.MSSQL
{
    public class CommonBLL
    {
        #region 获取用户分配的车辆信息（车代号、车辆使用单位名）
        #region 自由模式
        /// <summary>
        /// 获取用户分配的车辆信息（车代号、车辆使用单位名） 自由模式
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="vehicleName"></param>
        /// <returns></returns>
        public static List<UserVehicles> GetVehiclesAndStrucName(int userID, string vehicleName)
        {
            string sql = @"SELECT vt.VID,vt.VehicleName,vt.VIN,s.StrucName FROM dbo.Structures s 
                                    INNER JOIN Func_GetVehiclesListByUserID_New(@userID) AS vt ON s.ID=vt.StrucID WHERE vt.VehicleName LIKE @vehicleName";
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@userID",
                Value = userID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@vehicleName",
                Value = "%" + vehicleName + "%",
            };
            return ConvertToList<UserVehicles>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 默认模式
        /// <summary>
        /// 获取用户分配的车辆信息（车代号、车辆使用单位名） 默认模式
        /// </summary>
        /// <param name="strucID"></param>
        /// <param name="vehicleName"></param>
        /// <returns></returns>
        public static List<UserVehicles> GetDefaultVehiclesAndStrucName(int strucID, string vehicleName)
        {
            string sql = @"SELECT vt.VID,vt.VehicleName,vt.VIN,s.StrucName FROM dbo.Structures s 
                                    INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID(@StrucID) AS vt ON s.ID=vt.StrucID WHERE vt.VehicleName LIKE @vehicleName";
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@vehicleName",
                Value = "%" + vehicleName + "%",
            };
            return ConvertToList<UserVehicles>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion
        #endregion
    }
}

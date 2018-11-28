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
    public class VehicleTypeBLL
    {
        public static List<VehicleTypeDDLModel> GetVehicleTypes()
        {
            string sql = "SELECT Code,NAME FROM dbo.VehicleTypes";
            return ConvertToList<VehicleTypeDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        #region 导入车辆资料
        /// <summary>
        /// 根据车辆类型名获取车辆类型编码
        /// </summary>
        public static bool TryGetCodeByName(string name, out int code)
        {
            code = -1;
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NVarChar,30),
            };
            paras[0].Value = name;
            string sql = "SELECT CODE FROM VehicleTypes WHERE Name=@name";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)
            {
                return false;
            }
            code = Convert.ToInt32(result);
            return true;
        }
        #endregion
    }
}

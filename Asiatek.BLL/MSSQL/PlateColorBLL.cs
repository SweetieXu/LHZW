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
    public class PlateColorBLL
    {
        public static List<PlateColorsDDLModel> GetPlateColors()
        {
            string sql = "SELECT * FROM dbo.PlateColors";
            return ConvertToList<PlateColorsDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        #region 导入车辆资料
        /// <summary>
        /// 根据车牌颜色名获取车牌颜色编码
        /// </summary>
        public static bool TryGetCodeByName(string name, out int code)
        {
            code = -1;
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NChar,2),
            };
            paras[0].Value = name;
            string sql = "SELECT CODE FROM PlateColors WHERE Name=@name";
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

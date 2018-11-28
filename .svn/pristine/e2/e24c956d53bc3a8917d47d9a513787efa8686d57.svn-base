using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Asiatek.BLL.MSSQL
{
    /// <summary>
    /// 单位与车辆分配业务
    /// </summary>
    public class StrucVehicleDistributionBLL
    {
        /// <summary>
        /// 分配单位、车辆到用户
        /// </summary>
        /// <param name="vehicleIDs">待分配的车辆ID</param>
        /// <param name="strucIDs">待分配的单位ID</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static OperationResult AllotStrucAndVehicleToUser(int[] vehicleIDs, int[] strucIDs, int userID)
        {
            //先删除之前分配的车辆与单位信息
            //重新插入分配的信息到车辆分配表与单位分配表
            string[] sqls = new string[1];
            sqls[0] = @"DELETE FROM dbo.VehicleDistributionInfo WHERE UserID=" + userID + ";" +
"DELETE FROM dbo.StructureDistributionInfo WHERE UserID=" + userID + ";";



            if (vehicleIDs.Length != 0)
            {
                sqls[0] += @"INSERT INTO dbo.VehicleDistributionInfo( UserID, VehicleID )";
                for (int i = 0; i < vehicleIDs.Length; i++)
                {
                    if (i == 0)
                    {
                        sqls[0] += @"SELECT " + userID + "," + vehicleIDs[i] + "";
                    }
                    else
                    {
                        sqls[0] += @"UNION ALL SELECT " + userID + "," + vehicleIDs[i] + "";
                    }
                }
            }

            if (strucIDs.Length != 0)
            {

                sqls[0] += ";INSERT INTO dbo.StructureDistributionInfo( UserID, StrucID )";
                for (int i = 0; i < strucIDs.Length; i++)
                {
                    if (i == 0)
                    {
                        sqls[0] += @"SELECT " + userID + "," + strucIDs[i] + "";
                    }
                    else
                    {
                        sqls[0] += @"UNION ALL SELECT " + userID + "," + strucIDs[i] + "";
                    }
                }
            }








            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, null);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
    }
}

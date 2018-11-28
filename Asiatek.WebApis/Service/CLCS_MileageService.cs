using Asiatek.WebApis.Helpers;
using Asiatek.WebApis.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace Asiatek.WebApis.Service
{
    public class CLCS_MileageService
    {
        //1、获取CLCS_MileageManage表中的回单里程数据
        public static async Task<ReturnData> GetMilieageAsync(CLCS_MileageModel model)
        {
            //以下方法生成的时间格式是2018/6/7 0:00这样的，与数据库中2018-06-07 00:00不符
            //string sTime = model.StartTime.ToShortDateString() + " " + model.StartTime.ToShortTimeString();
            //string eTime = model.EndTime.ToShortDateString() + " " + model.EndTime.ToShortTimeString();
            string sTime = model.StartTime.ToString("yyyy-MM-dd HH:mm");
            string eTime = model.EndTime.ToString("yyyy-MM-dd HH:mm");

            //查询的时间段需要和CLCS回单里程查询的数据开始时间和结束时间完全匹配，时间精度读取到分钟（两边匹配数据都精确到分钟）
            //开始里程为0，结束里程和这段时间段内的行驶的里程一样

            string sql = string.Format(@" SELECT TOP 1 cmm.[PlateNum] AS VehicleCode,ve.VehicleName,sc.StrucName,(SELECT StrucName FROM dbo.Structures WHERE ID=sc.ParentID) AS PStrucName,
   cmm.[StartTime] AS BeginTime,cmm.[EndTime] AS EndTime,cmm.[ReturnMileage],0 AS BeginMilieage,cmm.[ReturnMileage] AS EndMilieage 
  FROM [dbo].[CLCS_MileageManage] cmm 
  INNER JOIN dbo.Vehicles ve ON ve.PlateNum = cmm.PlateNum  
  INNER JOIN dbo.Structures sc ON sc.ID = ve.StrucID 
  WHERE cmm.PlateNum='{0}' AND CONVERT(CHAR(16),cmm.StartTime,120)='{1}' AND CONVERT(CHAR(16),cmm.EndTime,120)='{2}' 
  ORDER BY cmm.CreateTime DESC ", model.PlateNum, sTime, eTime);

            var dt = await MSDBHelper.ExecuteDataTableAsync(sql, System.Data.CommandType.Text).ConfigureAwait(false);
            var list = ConvertToList<ReturnData>.Convert(dt);
            if (list == null)
            {
                return null;
            }
            if (list.Count == 0)
            {
                return null;
            }
            return list[0];
        }


        //2、获取Web.config设置中的车牌号对应的里程数据
        public static async Task<ReturnData> GetSettingMilieageAsync(CLCS_MileageModel model)
        {
            //返回的里程即为配置的里程
            //开始里程为0，结束里程和这段时间段内的行驶的里程一样
            //先查询除了里程、时间外的其他信息，然后再赋值
            string sql = string.Format(@"   SELECT TOP 1 ve.[PlateNum] AS VehicleCode,ve.VehicleName,sc.StrucName,
(SELECT StrucName FROM dbo.Structures WHERE ID=sc.ParentID) AS PStrucName, 0 AS BeginMilieage 
  FROM dbo.Vehicles ve  
  INNER JOIN dbo.Structures sc ON sc.ID = ve.StrucID 
  WHERE ve.PlateNum='{0}' ", model.PlateNum);

            var dt = await MSDBHelper.ExecuteDataTableAsync(sql, System.Data.CommandType.Text).ConfigureAwait(false);
            var list = ConvertToList<ReturnData>.Convert(dt);
            if (list == null)
            {
                return null;
            }
            if (list.Count == 0)
            {
                return null;
            }
            list[0].EndMilieage = model.ReturnMileage;
            list[0].ReturnMileage = model.ReturnMileage;
            list[0].BeginTime = model.StartTime;
            list[0].EndTime = model.EndTime;
            return list[0];
        }


        //3、通过车机信息正常计算里程
        #region 链接服务器写在配置文件中
        //        public static async Task<ReturnData> GetMilieageByHistoryAsync(CLCS_MileageModel model)
        //        {
        //            //根据车牌号和开始、结束时间，分别去历史轨迹表中查询对应的数据，计算车机里程
        //            var linkedServer = WebConfigurationManager.AppSettings["linkedServer"];

        //            #region 根据车牌号查询历史轨迹数据
        //            //判断查询月表，分开查询时间点里程数据然后计算
        //            string st = model.StartTime.ToString("yyyyMM");
        //            string et = model.EndTime.ToString("yyyyMM");
        //            //考虑到数据库月表数据量比较大，这边直接查询出所有满足条件的数据，然后再计算里程数据
        //            //查询同一张月表中的数据，只要关联一张月表就可以了
        //            string sql = "";
        //            if (st == et)
        //            {
        //                sql = string.Format(@"  SELECT ve.[PlateNum] AS VehicleCode,ve.VehicleName,sc.StrucName,
        //(SELECT StrucName FROM dbo.Structures WHERE ID=sc.ParentID) AS PStrucName,gn.Mileage ,gn.SignalDateTime 
        //  FROM dbo.Vehicles ve  
        //  INNER JOIN {0}.GNSS.dbo.[{1}] gn ON gn.VIN = ve.VIN 
        //  INNER JOIN dbo.Structures sc ON sc.ID = ve.StrucID 
        //  WHERE ve.PlateNum='{2}' AND gn.SignalDateTime>='{3}' AND gn.SignalDateTime<='{4}'
        //  ORDER BY gn.SignalDateTime  ", linkedServer, st, model.PlateNum, model.StartTime, model.EndTime);
        //            }
        //            else {
        //                sql = string.Format(@" SELECT * FROM (
        //SELECT ve.[PlateNum] AS VehicleCode,ve.VehicleName,sc.StrucName,
        //(SELECT StrucName FROM dbo.Structures WHERE ID=sc.ParentID) AS PStrucName,gn.Mileage ,gn.SignalDateTime
        //  FROM dbo.Vehicles ve  
        //  INNER JOIN {0}.GNSS.dbo.[{1}] gn ON gn.VIN = ve.VIN
        //  INNER JOIN dbo.Structures sc ON sc.ID = ve.StrucID 
        //  WHERE ve.PlateNum='{2}' AND gn.SignalDateTime>='{3}' 
        // UNION  ALL 
        //SELECT ve.[PlateNum] AS VehicleCode,ve.VehicleName,sc.StrucName,
        //(SELECT StrucName FROM dbo.Structures WHERE ID=sc.ParentID) AS PStrucName,gn.Mileage,gn.SignalDateTime 
        //  FROM dbo.Vehicles ve  
        //  INNER JOIN {0}.GNSS.dbo.[{4}] gn ON gn.VIN = ve.VIN
        //  INNER JOIN dbo.Structures sc ON sc.ID = ve.StrucID 
        //  WHERE ve.PlateNum='{2}' AND gn.SignalDateTime<='{5}' ) AS rs 
        //ORDER BY rs.SignalDateTime", linkedServer, st, model.PlateNum, model.StartTime, et, model.EndTime);
        //            }


        //            var dt = await MSDBHelper.ExecuteDataTableAsync(sql, System.Data.CommandType.Text).ConfigureAwait(false);
        //            var list = ConvertToList<SearchResultData>.Convert(dt);
        //            if (list == null)
        //            {
        //                return null;
        //            }
        //            if (list.Count == 0)
        //            {
        //                return null;
        //            }
        //            var rs = new ReturnData();
        //            var count = list.Count;
        //            rs.VehicleCode = list[0].VehicleCode;
        //            rs.VehicleName = list[0].VehicleName;
        //            rs.StrucName = list[0].StrucName;
        //            rs.PStrucName = list[0].PStrucName;
        //            rs.BeginTime = model.StartTime;
        //            rs.EndTime = model.EndTime;
        //            rs.BeginMilieage = list[0].Mileage;
        //            rs.EndMilieage = list[count - 1].Mileage;
        //            rs.ReturnMileage = rs.EndMilieage - rs.BeginMilieage;
        //            return rs;
        //            #endregion
        //        } 
        #endregion


        #region 读取终端表中的链接服务器
        public static async Task<ReturnData> GetMilieageByHistoryAsync(CLCS_MileageModel model)
        {
            //根据车牌号和开始、结束时间，分别去历史轨迹表中查询对应的数据，计算车机里程
            List<ServerInfoModel> slist = GetServerInfoByPlateNum(model.PlateNum);
            if (slist == null || slist.Count == 0)
            {
                return null;
            }
            var linkedServer = slist[0].LinkedServerName;

            #region 根据车牌号查询历史轨迹数据
            //判断查询月表，分开查询时间点里程数据然后计算
            string st = model.StartTime.ToString("yyyyMM");
            string et = model.EndTime.ToString("yyyyMM");
            //考虑到数据库月表数据量比较大，这边直接查询出所有满足条件的数据，然后再计算里程数据
            //查询同一张月表中的数据，只要关联一张月表就可以了
            string sql = "";
            if (st == et)
            {
                sql = string.Format(@"  SELECT ve.[PlateNum] AS VehicleCode,ve.VehicleName,sc.StrucName,
(SELECT StrucName FROM dbo.Structures WHERE ID=sc.ParentID) AS PStrucName,gn.Mileage ,gn.SignalDateTime 
  FROM dbo.Vehicles ve  
  INNER JOIN {0}.GNSS.dbo.[{1}] gn ON gn.VIN = ve.VIN 
  INNER JOIN dbo.Structures sc ON sc.ID = ve.StrucID 
  WHERE ve.PlateNum='{2}' AND gn.SignalDateTime>='{3}' AND gn.SignalDateTime<='{4}'
  ORDER BY gn.SignalDateTime  ", linkedServer, st, model.PlateNum, model.StartTime, model.EndTime);
            }
            else {
                sql = string.Format(@" SELECT * FROM (
SELECT ve.[PlateNum] AS VehicleCode,ve.VehicleName,sc.StrucName,
(SELECT StrucName FROM dbo.Structures WHERE ID=sc.ParentID) AS PStrucName,gn.Mileage ,gn.SignalDateTime
  FROM dbo.Vehicles ve  
  INNER JOIN {0}.GNSS.dbo.[{1}] gn ON gn.VIN = ve.VIN
  INNER JOIN dbo.Structures sc ON sc.ID = ve.StrucID 
  WHERE ve.PlateNum='{2}' AND gn.SignalDateTime>='{3}' 
 UNION  ALL 
SELECT ve.[PlateNum] AS VehicleCode,ve.VehicleName,sc.StrucName,
(SELECT StrucName FROM dbo.Structures WHERE ID=sc.ParentID) AS PStrucName,gn.Mileage,gn.SignalDateTime 
  FROM dbo.Vehicles ve  
  INNER JOIN {0}.GNSS.dbo.[{4}] gn ON gn.VIN = ve.VIN
  INNER JOIN dbo.Structures sc ON sc.ID = ve.StrucID 
  WHERE ve.PlateNum='{2}' AND gn.SignalDateTime<='{5}' ) AS rs 
ORDER BY rs.SignalDateTime", linkedServer, st, model.PlateNum, model.StartTime, et, model.EndTime);
            }
            

            var dt = await MSDBHelper.ExecuteDataTableAsync(sql, System.Data.CommandType.Text).ConfigureAwait(false);
            var list = ConvertToList<SearchResultData>.Convert(dt);
            if (list == null)
            {
                return null;
            }
            if (list.Count == 0)
            {
                return null;
            }
            var rs = new ReturnData();
            var count = list.Count;
            rs.VehicleCode = list[0].VehicleCode;
            rs.VehicleName = list[0].VehicleName;
            rs.StrucName = list[0].StrucName;
            rs.PStrucName = list[0].PStrucName;
            rs.BeginTime = model.StartTime;
            rs.EndTime = model.EndTime;
            rs.BeginMilieage = list[0].Mileage;
            rs.EndMilieage = list[count - 1].Mileage;
            rs.ReturnMileage = rs.EndMilieage - rs.BeginMilieage;
            return rs;
            #endregion
        }
        #endregion


        #region  根据车辆ID查询连接服务器
        public static List<ServerInfoModel> GetServerInfoByPlateNum(string PlateNum)
        {
            string sql = @"   SELECT v.PlateNum,s.LinkedServerName 
  FROM dbo.Vehicles v 
  INNER JOIN dbo.Terminals t ON v.ID=t.LinkedVehicleID 
  INNER JOIN dbo.ServerInfo s ON s.ID = t.ServerInfoID 
  WHERE v.PlateNum=@PlateNum ";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar)
            };
            paras[0].Value = PlateNum;
            var list = ConvertToList<ServerInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
            return list;
        }
        #endregion
    }
}
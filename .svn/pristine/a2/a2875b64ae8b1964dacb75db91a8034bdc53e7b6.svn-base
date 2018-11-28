using Asiatek.WebApis.Helpers;
using Asiatek.WebApis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Asiatek.WebApis.Service
{
    public class GLZWService
    {
        //获取实时信号表中的相关数据
        public static async Task<GLZW_RealTimeModel> SyncGetRealTimeInfoByPlateNum(string PlateNum)
        {
            List<ServerInfoModel> slist = CLCS_MileageService.GetServerInfoByPlateNum(PlateNum);
            if (slist == null || slist.Count == 0)
            {
                return null;
            }
            var linkedServer = slist[0].LinkedServerName;

            string sql = string.Format(@" SELECT PlateNumber AS PlateNum,Longitude,Latitude,[Address],[TerminalCode] 
FROM {0}.[GNSS].[dbo].[TerminalAddress] WHERE PlateNumber='{1}' ", linkedServer, PlateNum);

            var dt = await MSDBHelper.ExecuteDataTableAsync(sql, System.Data.CommandType.Text).ConfigureAwait(false);
            var list = ConvertToList<GLZW_RealTimeModel>.Convert(dt);
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


        //获取进出电子围栏相关数据
        public static async Task<List<GLZW_EFModel>> SyncGetEFInfoByPlateNum(GLZW_EFSearchModel model)
        {
            #region 查询绑定的电子围栏异常数据
            //链接服务器
            List<ServerInfoModel> slist = CLCS_MileageService.GetServerInfoByPlateNum(model.PlateNum);
            if (slist == null || slist.Count == 0)
            {
                return null;
            }
            var linkedServer = slist[0].LinkedServerName;
            string sql = string.Format(@"  SELECT ve.PlateNum AS VehicleCode,ve.VehicleName,ef.FenceName,efe.TerminalCode,
efe.SignalStartTime AS ExceptionStartTime,efe.SignalEndTime AS ExceptionEndTime  
  FROM dbo.Vehicles ve 
  INNER  JOIN  dbo.VehicleElectricFence vef ON vef.VehicleID = ve.ID 
  INNER JOIN dbo.ElectricFence ef ON ef.ID = vef.FenceID 
  INNER JOIN {0}.GNSS.dbo.ElectricFenceException efe ON efe.FenceID = vef.FenceID 
  WHERE ve.PlateNum='{1}' AND ('{2}' BETWEEN efe.SignalStartTime AND efe.SignalEndTime OR '{3}' BETWEEN efe.SignalStartTime AND efe.SignalEndTime)
  ", linkedServer, model.PlateNum, model.StartTime, model.EndTime);
            #endregion

            var dt = await MSDBHelper.ExecuteDataTableAsync(sql, System.Data.CommandType.Text).ConfigureAwait(false);
            var list = ConvertToList<GLZW_EFModel>.Convert(dt);
            if (list == null)
            {
                return null;
            }
            if (list.Count == 0)
            {
                return null;
            }
            //赋值查询开始时间、结束时间
            foreach (var item in list) {
                item.StartTime = model.StartTime;
                item.EndTime = model.EndTime;
            }
            return list;
        }

    }
}
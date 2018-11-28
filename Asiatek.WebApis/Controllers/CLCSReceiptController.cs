using Asiatek.Common;
using Asiatek.WebApis.Models;
using Asiatek.WebApis.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace Asiatek.WebApis.Controllers
{
    /// <summary>
    /// CLCS回单接口V1版本
    /// </summary>
    [RoutePrefix("CLCSreceipt")]
    public class CLCSReceiptController : BaseApiController
    {
        //
        // GET: /CLCSReceipt/

        #region 返回指定时间段内的里程信息
        /// <summary>
        /// 判断不同条件，返回相对应条件下的里程信息
        /// </summary>
        [Route("SyncGetMilieage"), HttpGet]
        public async Task<IHttpActionResult> SyncGetMilieage([FromUri]CLCS_MileageModel model)
        {
            //1.根据车牌号获取回单里程中相应车牌号创建时间最新的回单里程信息：
            //a.查无数据
            //b.成功查询，返回里程数据
            //2.上1查无数据，读取配置文件，查看相应的车牌号是否配置了回单里程：
            //a.查无数据
            //b.成功查询，返回里程数据
            //3.上1、2皆查无数据，通过车机信息正常计算里程数据：
            //a.车机信息有误，无法计算
            //b.成功

            #region 请求参数验证
            if (model.EndTime <= model.StartTime)
            {
                return AsiatekJson(ResponseCode.开始时间不能大于结束时间);
            }
            TimeSpan ts = model.EndTime - model.StartTime;
            if (ts.Days > 7)
            {  //里程查询天数超过7天
                return AsiatekJson(ResponseCode.查询天数不能超过7天);
            }
            //将传过来的车牌号转换成简体
            model.PlateNum = FontHelper.StringConvert(model.PlateNum, "2");
            #endregion

            #region 查询里程数据
            //返回数据有：vehicleCode、vehicleName、strucName、pStrucName、beginTime、endTime、returnMileage、beginMilieage、endMilieage
            //1、查询CLCS回单表中是否有数据
            var result = await CLCS_MileageService.GetMilieageAsync(model).ConfigureAwait(false);
            if (result == null)
            {
                //2、查询配置文件
                Dictionary<string, double> m_datas = new Dictionary<string, double>();
                //暂时采用配置文件的方式存取 apikey
                var settingMilieageStr = WebConfigurationManager.AppSettings["SettingVehicleMilieages"];
                if (!string.IsNullOrWhiteSpace(settingMilieageStr))
                {
                    try
                    {
                        var apiDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingVehicleMilieage>(settingMilieageStr);
                        foreach (var item in apiDatas.Datas)
                        {
                            m_datas.Add(item.PlateNum.Trim(), item.Mileage);
                        }
                    }
                    catch
                    {
                        return AsiatekJson(ResponseCode.配置数据有误);
                    }
                    if (m_datas.ContainsKey(model.PlateNum))
                    {
                        double milieage;
                        m_datas.TryGetValue(model.PlateNum, out milieage);
                        model.ReturnMileage = milieage;
                        var settingRs = await CLCS_MileageService.GetSettingMilieageAsync(model).ConfigureAwait(false);
                        return AsiatekJson(settingRs);
                    }
                    else
                    {
                        //3、计算里程
                        var historyRs = await CLCS_MileageService.GetMilieageByHistoryAsync(model).ConfigureAwait(false);
                        return AsiatekJson(historyRs);
                    }
                }
                else
                {
                    //3、计算里程
                    var historyRs = await CLCS_MileageService.GetMilieageByHistoryAsync(model).ConfigureAwait(false);
                    return AsiatekJson(historyRs);
                }
            }
            else
            {
                return AsiatekJson(result);
            }
            #endregion

        }
        #endregion

    }
}
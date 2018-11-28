using Asiatek.WebApis.Models;
using Asiatek.WebApis.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Asiatek.WebApis.Controllers
{
    [RoutePrefix("GLZW")]
    public class GLZWController : BaseApiController
    {
        #region 返回指定车牌号的实时位置相关信息
        [Route("SyncGetRealTimeInfoByPlateNum"), HttpGet]
        public async Task<IHttpActionResult> SyncGetRealTimeInfoByPlateNum(string PlateNum)
        {
            if (string.IsNullOrWhiteSpace(PlateNum)) 
            {
                return AsiatekJson(ResponseCode.请求参数有误);
            }
            var result = await GLZWService.SyncGetRealTimeInfoByPlateNum(PlateNum).ConfigureAwait(false);
            return AsiatekJson(result);
        }

        #endregion


        #region 返回指定车牌号指定时间段内进出电子围栏信息
        [Route("SyncGetEFInfoByPlateNum"), HttpGet]
        public async Task<IHttpActionResult> SyncGetEFInfoByPlateNum([FromUri]GLZW_EFSearchModel model)
        {
            if (model.EndTime <= model.StartTime)
            {
                return AsiatekJson(ResponseCode.开始时间不能大于结束时间);
            }
            TimeSpan ts = model.EndTime - model.StartTime;
            if (ts.Days > 7)
            {  //里程查询天数超过7天
                return AsiatekJson(ResponseCode.查询天数不能超过7天);
            }
            var result = await GLZWService.SyncGetEFInfoByPlateNum(model).ConfigureAwait(false);
            return AsiatekJson(result);
        }

        #endregion

    }
}

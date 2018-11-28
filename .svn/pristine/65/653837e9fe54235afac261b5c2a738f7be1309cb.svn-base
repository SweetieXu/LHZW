using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiatek.WebApis.Models
{
    public class ResponseData<T>
    {
        public bool State { get; set; }
        public ResponseCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class ResponseData
    {
        public bool State { get; set; }
        public ResponseCode Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public enum ResponseCode
    {
        //4位应答码   前两位为模块码从10开始，后两位为结果码 从00开始

        #region 10xx 通用返回码
        成功 = 1000,
        未知错误 = 1099,
        缺失接入码 = 1001,
        缺失签名 = 1002,
        缺失时间 = 1003,
        时间格式错误 = 1004,
        请求过期 = 1005,
        接入码不存在 = 1006,
        签名无效 = 1007,
        请求参数有误 = 1008,
        #endregion


        #region 参数判断
        配置数据有误 = 1101,
        查询天数不能超过7天 = 2000,
        开始时间不能大于结束时间 = 2001,
        #endregion

    }
}
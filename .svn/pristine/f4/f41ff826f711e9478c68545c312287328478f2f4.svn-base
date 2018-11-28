using Asiatek.WebApis.Models;
using Asiatek.WebApis.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Asiatek.WebApis.Controllers
{
    public class BaseApiController : ApiController
    {
        protected virtual AsiatekJsonResult<T> AsiatekJson<T>(T content, HttpStatusCode httpStatusCode)
        {
            return new AsiatekJsonResult<T>(content, this.Request, httpStatusCode);
        }


        protected AsiatekJsonResult<ResponseData> AsiatekJson(ResponseCode code, string message, object data = null, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            return this.AsiatekJson<ResponseData>(new ResponseData()
            {
                State = true,
                Code = code,
                Message = message,
                Data = data
            }, httpStatusCode);
        }

        protected AsiatekJsonResult<ResponseData> AsiatekJson(ResponseCode code, HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            return this.AsiatekJson(code, code.ToString(), null, httpStatusCode);
        }

        /// <summary>
        /// 返回成功附带Data数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected AsiatekJsonResult<ResponseData> AsiatekJson(object data)
        {
            return this.AsiatekJson(ResponseCode.成功, ResponseCode.成功.ToString(), data, HttpStatusCode.OK);
        }

    }
}
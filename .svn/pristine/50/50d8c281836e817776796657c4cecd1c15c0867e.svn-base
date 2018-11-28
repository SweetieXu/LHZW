using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;

namespace Asiatek.WebApis.ActionResults
{
    /// <summary>
    /// 亚士德Json结果
    /// UTF-8编码
    /// 时间格式为yyyy-MM-dd HH:mm:ss:ms
    /// 时间为本地时间
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsiatekJsonResult<T> : JsonResult<T>
    {
        public HttpStatusCode HttpStatusCode { get; set; }

        public AsiatekJsonResult(T content, HttpRequestMessage request, HttpStatusCode code = System.Net.HttpStatusCode.OK)
            : base(content, new Newtonsoft.Json.JsonSerializerSettings()
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss:ms",
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local,
                Formatting = Newtonsoft.Json.Formatting.Indented
            }, Encoding.UTF8, request)
        {
            this.HttpStatusCode = code;
        }

        public override Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var resp = this.Request.CreateResponse(this.HttpStatusCode, this.Content);
            return Task.FromResult(resp);
        }
    }
}
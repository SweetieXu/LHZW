using Asiatek.WebApis.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace Asiatek.WebApis
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);


            //移除XML格式化器
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            //配置返回均为JSON
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss:ms";//json中Date类型格式
            json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;//本地时间
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //只返回utf-8字符编码的json
            json.SupportedEncodings.Clear();
            json.SupportedEncodings.Add(Encoding.UTF8);
            //启用特性路由
            config.MapHttpAttributeRoutes();
            //默认路由
            config.Routes.MapHttpRoute
            (
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //全局启用Action过滤器
            config.Filters.Add(new CLCSReceiptActionFilterAttribute());
            //全局启用异常过滤器
            config.Filters.Add(new CLCSReceiptExceptionHandlingAttribute());
        }
    }
}

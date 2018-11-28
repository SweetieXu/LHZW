using Asiatek.TMS.Filters;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //全局注册自定义异常处理过滤器
            filters.Add(new AsiatekHandleErrorAttribute());
            //全局注册授权过滤器
            filters.Add(new AsiatekAuthorizeAttribute());
        }
    }
}
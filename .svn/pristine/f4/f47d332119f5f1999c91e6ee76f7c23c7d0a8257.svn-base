using Asiatek.TMS.ModelBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Asiatek.TMS
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //由于MVC寻找视图的时候默认是从WebForm视图开始，所以当我们只用Razor视图时，去掉前者可以增加效率
            ViewEngines.Engines.Clear(); //移除所有视图引擎
            ViewEngines.Engines.Add(new RazorViewEngine());//只添加Razor视图引擎

            //为了安全性所有响应中不包含MVC响应头
            MvcHandler.DisableMvcResponseHeader = false;

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleTable.EnableOptimizations = true;//调试下也启用js与css压缩
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //添加自定义的模型绑定器
            ModelBinders.Binders.DefaultBinder = new AsiatekModelBinder();

            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnStr"].ConnectionString;
            ////启用指定数据库的更改通知
            //System.Web.Caching.SqlCacheDependencyAdmin.EnableNotifications(connectionString);
            ////启用针对指定数据库的指定表的更改通知
            //System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(connectionString, "Structures");

        }
    }
}
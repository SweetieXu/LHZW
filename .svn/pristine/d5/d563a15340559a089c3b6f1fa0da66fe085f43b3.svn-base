using Asiatek.BLL.MSSQL;
using Asiatek.Common;
using Asiatek.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Asiatek.TMS.Filters
{
    /// <summary>
    /// 表示一个特性，该特性用于标记需要进过授权验证的控制器或操作
    /// <para>作者：戴天辰</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AsiatekAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            #region 允许匿名
            //如果当前控制器或操作上拥有AllowAnonymousAttribute特性，那么跳过身份验证
            //比如登录操作
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length > 0 |
                 filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Length > 0)
            {
                return;
            }
            #endregion


            #region 是否登录
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)//未登录情况
            {
                //当没有登录时，默认返回未授权结果
                ActionResult result = new HttpUnauthorizedResult();
                //但是如果是Ajax操作，那么需要返回一个状态值，通过脚本返回指定页面
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    string url = FormsAuthentication.LoginUrl;
                    //string url = UrlHelper.GenerateUrl("Default", "Login", "Account", filterContext.RouteData.Values, RouteTable.Routes, filterContext.RequestContext, true);
                    result = new JsonResult() { Data = new { AsiatekError = true, Url = url, Message = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                filterContext.Result = result;
                return;
            }
            #endregion


            #region Session是否过期
            //通过登录界面登录的时候，会创建一个临时cookie，如果有这个cookie，并且session为null，就代表session过期了，那么就可以直接返回登录页
            //如果是通过免登录并且关闭过浏览器上来的用户，肯定没有这个cookie值，所以直接对session赋值，并且创建该cookie
            //因此不管是哪种情况，只要有该cookie并且session为null就代表session过期，可以返回登录页


            //当前登录凭证信息  用户编号|用户名|角色编号|角色名称|角色等级|用户昵称|单位ID|车辆查看模式
            int userID = 0;
            string userName = string.Empty;
            int roleID = 0;
            string roleName = string.Empty;
            int roleLevel = 0;
            string nickName = string.Empty;
            int strucID = 0;
            bool vehicleViewMode = true;
            try
            {
                string identity = filterContext.HttpContext.User.Identity.Name;
                string[] identities = identity.Split('|');
                userID = Convert.ToInt32(identities[0]);//用户编号
                userName = identities[1];//用户名
                roleID = Convert.ToInt32(identities[2]);//角色编号
                roleName = identities[3];
                roleLevel = Convert.ToInt32(identities[4]);//角色等级
                nickName = identities[5];//用户昵称
                strucID = Convert.ToInt32(identities[6]);//单位ID
                vehicleViewMode = Convert.ToBoolean(identities[7]);//车辆查看模式
            }
            catch
            {
                //解析Cookie出错
                FormsAuthentication.SignOut();
                ActionResult result = new HttpUnauthorizedResult();
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    string url = FormsAuthentication.LoginUrl;
                    result = new JsonResult() { Data = new { AsiatekError = true, Url = url, Message = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                filterContext.Result = result;
                return;
            }
            HttpCookie ck = filterContext.HttpContext.Request.Cookies["loginflag"];//登录标识cookie
            Asiatek.Model.UserSessionModel currentUser = filterContext.HttpContext.Session["currentUser"] as Asiatek.Model.UserSessionModel;//存储在session中的用户信息
            if (ck != null && currentUser == null)//session过期,清除cookie信息，返回登录页
            {
                ck.Expires = DateTime.Now.AddYears(-1);
                filterContext.HttpContext.Response.Cookies.Add(ck);
                FormsAuthentication.SignOut();

                ActionResult result = new HttpUnauthorizedResult();
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    string url = FormsAuthentication.LoginUrl;
                    //string url = UrlHelper.GenerateUrl("Default", "Login", "Account", filterContext.RouteData.Values, RouteTable.Routes, filterContext.RequestContext, true);
                    result = new JsonResult() { Data = new { AsiatekError = true, Url = url, Message = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                filterContext.Result = result;
                return;
            }
            //关闭浏览器后免登录上来的用户无登录标识cookie，并且session为null，此时对session赋值，并且发送登录标识cookie
            else if (ck == null && currentUser == null)
            {
                //临时的登录标识cookie，关闭浏览器后消失
                ck = new HttpCookie("loginflag");
                ck.Expires = DateTime.MinValue;
                filterContext.HttpContext.Response.Cookies.Add(ck);

                //进行当前用户的信息绑定
                currentUser = new Asiatek.Model.UserSessionModel()
                {
                    UserName = userName,
                    UserId = userID,
                    NickName = nickName,
                    StrucID = strucID,
                    RoleInfo = new Asiatek.Model.RoleInfoModel()
                    {
                        RoleID = roleID,
                        RoleName = roleName,
                        RoleLevel = (Asiatek.Model.RoleLevelEnum)roleLevel
                    },
                    VehicleViewMode = vehicleViewMode
                };

                //这里查询出该用户的权限信息，赋值给currentUser对象
                if (currentUser.RoleInfo.RoleLevel == Model.RoleLevelEnum.SuperAdmin)
                {
                    currentUser.Functions = FunctionBLL.GetAllFunctions();
                }
                else
                {
                    currentUser.Functions = FunctionBLL.GetFunctionsByUserID(currentUser.UserId);
                }

                //重新添加回session中
                filterContext.HttpContext.Session["currentUser"] = currentUser;
            }
            #endregion


            #region 是否是超级管理员（超级管理员拥有全部权限，可以不用验证）
            if (currentUser.RoleInfo.RoleLevel == RoleLevelEnum.SuperAdmin)
            {
                return;
            }
            #endregion



            #region 权限

            #region 跳过权限
            //拥有PassPremissionAttribute的可以跳过数据库权限验证
            //比如有些操作中的ajax操作，这些操作完全可以跳过权限验证，比如动态刷新时间，否则权限设定处还需要将这些操作列出来赋给用户，毫无意义
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AsiatekPassPremissionAttribute), true).Length > 0 |
                 filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AsiatekPassPremissionAttribute), true).Length > 0)
            {
                return;
            }
            #endregion



            #region 是否具有权限
            //当前访问控制器名称
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //当前访问的操作名称
            string actionName = filterContext.ActionDescriptor.ActionName;
            //当前访问的区域名称  没有指定区域 结果为null
            var routeData = filterContext.HttpContext.Request.RequestContext.RouteData;
            var area = routeData.DataTokens["area"];
            string areaName = "DefaultArea";//默认的区域是空 我们系统默认为DefaultArea
            if (area != null)
            {
                areaName = area.ToString();
            }

            #region 欢迎页无需权限
            if (areaName == "DefaultArea" && controllerName == "Home" && actionName == "Welcome")
            {
                return;
            }
            #endregion



            #region 是否是从属功能
            //当前行为具有从属特性，检查上级内容是否在用户权限中
            var subAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AsiatekSubordinateFunctionAttribute), true);
            if (subAttributes.Length > 0)
            {
                foreach (var item in subAttributes)
                {
                    AsiatekSubordinateFunctionAttribute temp = item as AsiatekSubordinateFunctionAttribute;
                    string superiorAreaName = temp.SuperiorAreaName;
                    string superiorControllerName = temp.SuperiorControllerName;
                    string superiorActionName = temp.SuperiorActionName;
                    //如果superiorControllerName或superiorControllerName是NULL，则默认为当前值
                    var tempQuery = from c in currentUser.Functions
                                    where
                                       c.AreaName == (superiorAreaName == null ? areaName : superiorAreaName) &&
                                       c.ControllerName == (superiorControllerName == null ? controllerName : superiorControllerName) &&
                                       c.ActionName == superiorActionName
                                    select c;
                    if (tempQuery.Count() != 0)//拥有权限
                    {
                        return;
                    }
                }
            }
            #endregion



            //查询当前登录用户的权限中是否包含目前访问的区域、控制器与操作
            //只有三个条件均包含才算拥有权限
            var query = from c in currentUser.Functions
                        where c.ControllerName == controllerName &&
                        c.ActionName == actionName &&
                        c.AreaName == areaName
                        select c;
            if (query.Count() != 0)//拥有权限
            {
                return;
            }
            #endregion




            #region 不包含权限的逻辑
            //不包含权限的处理
            //通过Html.Action或Html.RenderAction方式执行的操作算作子操作
            //子操作不能执行重定向操作
            //这里返回无权限的文字显示
            if (filterContext.IsChildAction)
            {
                filterContext.Result = new ContentResult() { Content = Resource.UIText.NoPermission };
                return;
            }


            //处理操作为Ajax操作
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                //没有权限的话，对于Ajax方法，返回一个json值，前端通过该值决定返回到哪个URL
                //这里返回无权限页面
                string url = UrlHelper.GenerateUrl("Default", "UnAuth", "Account", filterContext.RouteData.Values, RouteTable.Routes, filterContext.RequestContext, true);
                filterContext.Result = new JsonResult() { Data = new { AsiatekError = true, Url = url, Message = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                return;
            }
            //普通操作,将用户重定向到无权限页面
            RouteValueDictionary rvd = new RouteValueDictionary();
            rvd.Add("controller", "Account");
            rvd.Add("action", "UnAuth");
            filterContext.Result = new RedirectToRouteResult("Default", rvd);
            #endregion
            #endregion
        }
    }




    /// <summary>
    /// 表示一个特性，该特性用于处理错误
    /// <para>作者：戴天辰</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AsiatekHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            //如果异常已经被处理了或者没有在配置文件里启用自定义错误
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }
            //如果异常不是500服务器端错误
            Exception exception = filterContext.Exception;
            if (new HttpException((string)null, exception).GetHttpCode() != 500 || !this.ExceptionType.IsInstanceOfType((object)exception))
            {
                return;
            }
            ////引发异常的控制器名称
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            ////引发异常的操作名称
            var actionName = (string)filterContext.RouteData.Values["action"];
            //HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);


            if (filterContext.IsChildAction)
            {
                filterContext.Result = new ContentResult() { Content = Resource.UIText.ErrorOccurred };
            }
            else if (filterContext.HttpContext.Request.IsAjaxRequest())//Ajax请求需要返回json对象
            {
                string url = UrlHelper.GenerateUrl("Default", "OtherError", "Errors", filterContext.RouteData.Values, RouteTable.Routes, filterContext.RequestContext, true);
                //注意一定要允许get请求获取json数据，否则对于ajax的get请求，永远会应答500
                filterContext.Result = new JsonResult() { Data = new { AsiatekError = true, Url = url, Message = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else//普通请求
            {
                RouteValueDictionary rvd = new RouteValueDictionary();
                rvd.Add("controller", "Errors");
                rvd.Add("action", "OtherError");
                filterContext.Result = new RedirectToRouteResult("Default", rvd);
            }
            string errorInfo = string.Format("异常消息：{0}\r\n堆栈信息：{1}", exception.Message, exception.StackTrace);
            LogHelper.DoOtherErrorLog(errorInfo);//记录日志
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            //如果是ajax操作，那么响应500的话就会进入脚本的错误逻辑而不会走200的回调，所以这里不能响应500
            //为了让session过期与发生错误进行统一处理，均算success
            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;//程序中处理了错误，告诉IIS别再处理

        }
    }



    /// <summary>
    /// 表示一个特性，该特性用于标记在授权期间要跳过数据库权限验证的控制器或操作
    /// <para>作者：戴天辰</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AsiatekPassPremissionAttribute : Attribute { }



    /// <summary>
    /// 表示一个特性，该特性用于标记从属于某个功能的功能
    /// <para>说明：若B功能从属于A功能，则当具有A功能的权限时，则相当于具有B功能的权限</para>
    /// <para>比如：新增区域信息，其中具有检查区域名称是否重复的操作，该操作隶属于新增区域信息操作</para>
    /// <para>此时对验证名称是否重复的Action加上该特性，则无需再对该操作配置权限</para>
    /// <para>作者：戴天辰</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AsiatekSubordinateFunctionAttribute : Attribute
    {
        /// <summary>
        /// 上级功能所属区域
        /// </summary>
        public string SuperiorAreaName { get; set; }
        /// <summary>
        /// 上级功能所属控制器
        /// </summary>
        public string SuperiorControllerName { get; set; }
        /// <summary>
        /// 上级功能所属动作
        /// </summary>
        public string SuperiorActionName { get; set; }



        /// <summary>
        /// 构建从属特性
        /// </summary>
        ///  <param name="superiorActionName">上级功能所属动作</param>
        /// <param name="superiorControllerName">上级功能所属控制器**保持NULL为当前请求的控制器**</param>
        /// <param name="superiorAreaName">上级功能所属区域**保持NULL为当前请求的区域**</param>
        public AsiatekSubordinateFunctionAttribute(string superiorActionName, string superiorControllerName = null, string superiorAreaName = null)
        {
            this.SuperiorActionName = superiorActionName;
            this.SuperiorControllerName = superiorControllerName;
            this.SuperiorAreaName = superiorAreaName;
        }
    }

}
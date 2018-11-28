using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using Asiatek.Model;
using Asiatek.BLL.MSSQL;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Asiatek.TMS.TerminalOperation;
using System.ServiceModel;

namespace Asiatek.TMS.Controllers
{
    /// <summary>
    /// 基础控制器
    /// 这里定义一些控制器可能通用的操作
    /// </summary>
    public class BaseController : AsyncController
    {



        #region 属性
        /// <summary>
        /// 当前用户是否是普通用户
        /// </summary>
        protected bool IsUser
        {
            get
            {
                return (Session["currentUser"] as UserSessionModel).RoleInfo.RoleLevel == RoleLevelEnum.User;
            }
        }

        /// <summary>
        /// 当前用户是否是管理员
        /// </summary>
        protected bool IsAdmin
        {
            get
            {
                return (Session["currentUser"] as UserSessionModel).RoleInfo.RoleLevel == RoleLevelEnum.Admin;
            }
        }
        /// <summary>
        /// 当前用户是否是超级管理员
        /// </summary>
        protected bool IsSuperAdmin
        {
            get
            {
                return (Session["currentUser"] as UserSessionModel).RoleInfo.RoleLevel == RoleLevelEnum.SuperAdmin;
            }
        }
        /// <summary>
        /// 当前区域名
        /// </summary>
        protected string AreaName
        {
            get
            {
                try
                {
                    return Request.RequestContext.RouteData.DataTokens["area"].ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 当前控制器名
        /// </summary>
        protected string ControllerName
        {
            get
            {
                return Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }
        /// <summary>
        /// 当前动作名
        /// </summary>
        protected string ActionName
        {
            get
            {
                return Request.RequestContext.RouteData.Values["action"].ToString();
            }
        }
        /// <summary>
        /// 分页大小
        /// 默认25
        /// </summary>
        protected virtual int PageSize
        {
            get
            {
                return 10;
            }
        }
        /// <summary>
        /// 当前登录用户的UserID
        /// </summary>
        protected int CurrentUserID
        {
            get
            {
                return GetUserSession().UserId;
            }
        }

        /// <summary>
        /// 当前登录用户的所属单位ID
        /// </summary>
        protected int CurrentStrucID
        {
            get
            {
                return GetUserSession().StrucID;
            }
        }

        /// <summary>
        /// 车辆查看模式 (true 默认模式 能看到自己和自己子单位的所有车辆 false 自由模式 查看在车辆与单位分配功能处分配的车辆)
        /// </summary>
        public bool VehicleViewMode
        {
            get
            {
                return GetUserSession().VehicleViewMode;
            }
        }

        /// <summary>
        /// 用于各种记录的用户ID
        /// 如果切换过用户，那么为原ID
        /// </summary>
        protected int UserIDForLog
        {
            get
            {
                int userID = CurrentUserID;
                var originalUserID = GetSession("OriginalUserID");
                if (originalUserID != null)
                {
                    userID = Convert.ToInt32(originalUserID);
                }
                return userID;
            }
        }
        /// <summary>
        /// 终端操作WCF的contract
        /// </summary>
        protected const string WCFTerminalOperationContract = "/ITerminalOperation";
        #endregion

        #region 方法


        #region 资源文件获取

        protected string GetDisplayText(string name)
        {
            return GetResourceText(name, Asiatek.Resource.DisplayText.ResourceManager);
        }

        protected string GetDataAnnotations(string name)
        {
            return GetResourceText(name, Asiatek.Resource.DataAnnotations.ResourceManager);
        }

        protected string GetPromptInformation(string name)
        {
            return GetResourceText(name, Asiatek.Resource.PromptInformation.ResourceManager);
        }

        protected string GetUIText(string name)
        {
            return GetResourceText(name, Asiatek.Resource.UIText.ResourceManager);
        }


        protected string GetResourceText(string name, System.Resources.ResourceManager rm)
        {
            var temp = rm.GetString(name, System.Threading.Thread.CurrentThread.CurrentUICulture);
            if (string.IsNullOrWhiteSpace(temp))
            {
                return name;
            }
            else
            {
                return temp;
            }
        }
        #endregion



        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //国际化处理
            HttpCookie ck = requestContext.HttpContext.Request.Cookies["lang"];
            if (ck != null && !string.IsNullOrWhiteSpace(ck.Value))
            {
                string lang = ck.Value;
                System.Threading.Thread.CurrentThread.CurrentCulture =
System.Globalization.CultureInfo.CreateSpecificCulture(lang);
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(lang);
            }
            base.Initialize(requestContext);
        }

        /// <summary>
        /// 将视图结果转换为字符串
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <returns></returns>
        [NonAction]
        protected string RenderViewToString(Controller controller, string viewName, string masterName)
        {
            IView view = ViewEngines.Engines.FindView(controller.ControllerContext, viewName, masterName).View;
            using (StringWriter writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer);
                viewContext.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }

        /// <summary>
        /// 将分部视图转换为字符串
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="partialViewName"></param>
        /// <returns></returns>
        [NonAction]
        protected string RenderPartialViewToString(Controller controller, string partialViewName)
        {
            IView view = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialViewName).View;
            using (StringWriter writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, view, controller.ViewData, controller.TempData, writer);
                viewContext.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }


        /// <summary>
        /// 创建SelectListItem对象
        /// </summary>
        /// <param name="valueField">选定的值</param>
        /// <param name="textField">数据文本字段</param>
        /// <returns></returns>
        protected SelectListItem GetSelectListItem(object valueField, object textField, bool selected = false)
        {
            return new SelectListItem() { Text = textField.ToString(), Value = valueField.ToString(), Selected = selected };
        }

        /// <summary>
        /// 设置Session信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        protected void SetSession(string key, object value)
        {
            Session[key] = value;
        }

        /// <summary>
        /// 根据键获取Session值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>Session中对应Key的Value</returns>
        protected object GetSession(string key)
        {
            return Session[key];
        }

        /// <summary>
        /// 获取当前Session中用户信息
        /// </summary>
        protected UserSessionModel GetUserSession()
        {
            return Session["currentUser"] as UserSessionModel;
        }






        /// <summary>
        /// 根据 AreaName、ControllerName、ActionName获取功能信息
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="actionName">动作名</param>
        /// <returns>功能信息</returns>
        protected FunctionsInfoModel GetFunctionsInfo(string areaName, string controllerName, string actionName)
        {

            if (string.IsNullOrWhiteSpace(areaName))
            {
                areaName = "DefaultArea";
            }
            return
                   (from f in GetUserSession().Functions
                    where f.AreaName == areaName && f.ControllerName == controllerName && f.ActionName == actionName
                    select f).SingleOrDefault();
        }

        /// <summary>
        /// 获取当前功能信息
        /// </summary>
        /// <returns>当前功能信息</returns>
        protected FunctionsInfoModel GetCurrentFunctionsInfo()
        {
            return GetFunctionsInfo(this.AreaName, this.ControllerName, this.ActionName);
        }



        /// <summary>
        /// 根据 AreaName、ControllerName、ActionName获取功能的子功能信息
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="actionName">动作名</param>
        /// <returns>功能的子功能信息</returns>
        protected List<FunctionsInfoModel> GetSubFunctionsInfo(string areaName, string controllerName, string actionName)
        {
            return GetUserSession().Functions.Where(f => f.ParentID == GetFunctionsInfo(areaName, controllerName, actionName).ID).ToList();
        }


        /// <summary>
        /// 获取当前功能的子功能信息
        /// </summary>
        /// <returns>当前功能的子功能信息</returns>
        protected List<FunctionsInfoModel> GetCurrentSubFunctionsInfo()
        {
            return GetUserSession().Functions.Where(f => f.ParentID == GetFunctionsInfo(this.AreaName, this.ControllerName, this.ActionName).ID).OrderBy(f => f.OrderIndex).ToList();
        }


        /// <summary>
        /// 根据 AreaName、ControllerName、ActionName获取当前功能对应视图的标题
        /// </summary>
        /// <param name="areaName">区域名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="actionName">动作名</param>
        /// <returns>当前功能信息</returns>
        protected string GetViewTitle(string areaName, string controllerName, string actionName)
        {
            return GetFunctionsInfo(areaName, controllerName, actionName).FunctionName;
        }

        /// <summary>
        /// 获取当前功能名
        /// </summary>
        /// <returns></returns>
        protected string GetCurrentViewName()
        {
            return GetViewTitle(this.AreaName, this.ControllerName, this.ActionName);
        }

        /// <summary>
        /// 修复当使用OutputCache缓存时，ASP.NET 会在第二次的304应答时，响应头中附加Vary:"*"
        /// 导致第三次浏览器的请求并不带有If-Modified-Since头内容
        /// 造成浏览器直接去请求原始服务器要求下载最新内容的情况
        /// 虽然当服务器端也同时具有缓存且没有过期时也依然会去应答缓存的内容给客户端，
        /// 可是 这违背了HTTP的规范，导致中间缓存服务器会失去它的作用,浏览器收到的应答也不是304而是200
        /// 这是从ASP.NET 1.0到ASP.NET 4.0都有一个BUG
        /// 解决方法是对于缓存响应不再附加Vary:"*"
        /// [目前发现只有火狐严格按照HTTP规范，所以也只有火狐会让这个BUG起效；IE9和谷歌都忽略掉应答中的Vary："*"]
        /// </summary>
        protected void FixVaryBug()
        {
            Response.Cache.SetOmitVaryStar(true);
        }

        /// <summary>
        /// 记录增删改日志
        /// </summary>
        /// <param name="opType">操作类型</param>
        /// <param name="operationResult">操作结果</param>
        protected void DoLog(OperationTypeEnum opType, OperationResult operationResult)
        {
            DoCommonLog(opType, operationResult.Message);
        }

        /// <summary>
        /// 记录增删改日志
        /// </summary>
        /// <param name="opType">操作类型</param>
        /// <param name="operationResult">操作结果</param>
        /// <param name="content">操作内容：比如新增时的名称等等</param>
        protected void DoLog(OperationTypeEnum opType, OperationResult operationResult, string content)
        {
            DoCommonLog(opType, operationResult.Message + "[" + content + "]");
        }



        /// <summary>
        /// 记录增删改日志
        /// </summary>
        /// <param name="opType">操作类型</param>
        /// <param name="operationInfo">操作信息</param>
        protected void DoCommonLog(OperationTypeEnum opType, string operationInfo)
        {
            var fun = this.GetCurrentFunctionsInfo();

            LogModel log = new LogModel()
            {
                UserID = UserIDForLog.ToString(),
                AreaName = this.AreaName,
                ControllerName = this.ControllerName,
                ActionName = this.ActionName,
                FunctionName = fun != null ? fun.FunctionName : string.Empty,
                OperationType = opType,
                OperationInfo = operationInfo
            };
            LogBLL.DoOperationLog(log);
        }


        /// <summary>
        /// 根据指定的地址获取TerminalOperationClient
        /// 注意地址只包含IP和端口，不包含Contract
        /// </summary>
        protected virtual TerminalOperationClient GetTerminalOperationClient(string endPoint)
        {
            WSHttpBinding wshb = new WSHttpBinding();//使用协议与服务端相同
            wshb.Security.Mode = SecurityMode.None; //安全级别
            EndpointAddress addr = new EndpointAddress(endPoint + WCFTerminalOperationContract);
            return new TerminalOperationClient(wshb, addr);
        }
        #endregion
        #region 获取WEB客户端IP地址
        /// <summary>
        /// 获取WEB客户端IP地址
        /// </summary>
        protected string GetRemoteAddress()
        {
            string userIP = "未获取用户IP";
            var Context = this.HttpContext;
            try
            {
                if (Context == null || Context.Request == null || Context.Request.ServerVariables == null)
                {
                    return "";
                }

                string CustomerIP = "";

                //CDN加速后取到的IP
                CustomerIP = Context.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                if (Context.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (CustomerIP == null)
                    {
                        CustomerIP = Context.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    CustomerIP = Context.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
                {
                    return Context.Request.UserHostAddress;
                }
                return CustomerIP;
            }
            catch { }

            return userIP;
        }
        #endregion
        #region 实体类反射
        /// <summary>
        /// 获取属性与属性值字符串(返回字符串为:key1:value1,key2:value,key3:value.....)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns>key1:value1,key2:value,key3:value.....</returns>
        protected string GetProperties<T>(T t)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string setInfo = string.Empty;

            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return null;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value != null)
                    {
                        ret.Add(name, value.ToString());
                    }

                }
            }

            if (ret.Count > 0)
            {
                foreach (var item in ret)
                {
                    setInfo += item.Key + ":" + item.Value + ",";
                }
            }
            return setInfo.TrimEnd(',');
        }
        #endregion
    
    }


}

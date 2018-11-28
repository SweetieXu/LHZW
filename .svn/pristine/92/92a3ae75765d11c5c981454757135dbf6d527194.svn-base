using Asiatek.Common;
using Asiatek.Model;
using Asiatek.Resource;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asiatek.TMS.ReportFiles.reports
{
    public partial class RptBaseForm : System.Web.UI.Page
    {
        #region 方法
        protected override void InitializeCulture()
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies["lang"];
            if (ck != null && !string.IsNullOrWhiteSpace(ck.Value))
            {
                string lang = ck.Value;
                this.Culture = lang;
                this.UICulture = lang;
            }
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
        /// 检查报表访问权限
        /// 无权限时重定向到欢迎页
        /// </summary>
        protected void CheckReportPermission()
        {
            var rptFlag = GetSession("rptFlag");
            var url = this.Request.UrlReferrer.AbsoluteUri;
            //如果点击了报表中的查询，再点其他页面，再点浏览器后退回到报表，就会因为rptFlag为null，导致无权限提示。。。
            //因此只能通过URL来判断了，如果UrlReferrer中包含/Reports/。。。那么就算合法。。。
            if (!url.Contains("/Reports/"))
            {
                //经由控制器到Action最后正常访问到报表一定会有该session值
                //如果没有就代表是非正常访问
                if (rptFlag == null)
                {
                    this.RedirectToUnAuth();
                }
            }
            SetSession("rptFlag", null);


            //var rptName = Request["rptName"];//获取报表名称rptName
            //if (string.IsNullOrWhiteSpace(rptName))//如果是正常方式访问，那么一定会有该查询变量
            //{
            //    this.RedirectToUnAuth();
            //}

            //var userSession = this.GetUserSession();
            //if (userSession.Functions.SingleOrDefault(f => f.ActionName == rptName) == null)
            //{
            //    this.RedirectToUnAuth();
            //}
        }

        /// <summary>
        /// 检查查询结果
        /// </summary>
        /// <param name="jsonResult"></param>
        /// <param name="ltCheckInfo"></param>
        /// <returns></returns>
        protected bool CheckResult(string jsonResult, Literal ltCheckInfo)
        {
            if (string.IsNullOrWhiteSpace(jsonResult))
            {
                string script = string.Format("$.showPromptDialog('{0}', '{1}', '{2}');", UIText.NoDatas, UIText.InformationTitle, UIText.Close);
                System.Web.UI.ScriptManager.RegisterStartupScript(ltCheckInfo, this.GetType(), "tempScript", script, true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="ltCheckInfo"></param>
        /// <returns></returns>
        protected bool CheckResult<T>(List<T> list, Literal ltCheckInfo)
        {
            if (list == null || list.Count <= 0)
            {
                string script = string.Format("$.showPromptDialog('{0}', '{1}', '{2}');", UIText.NoDatas, UIText.InformationTitle, UIText.Close);
                System.Web.UI.ScriptManager.RegisterStartupScript(ltCheckInfo, this.GetType(), "tempScript", script, true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查查询输入条件
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ltCheckInfo"></param>
        /// <returns></returns>
        protected bool CheckInput(DateTime startTime, DateTime endTime, Literal ltCheckInfo)
        {
            if (endTime < startTime)
            {
                string script = string.Format("$.showPromptDialog('{0}', '{1}', '{2}');", DataAnnotations.StartTimeMoreThanEndTimeError, UIText.InformationTitle, UIText.Close);
                System.Web.UI.ScriptManager.RegisterStartupScript(ltCheckInfo, this.GetType(), "tempScript", script, true);
                return false;
            }

            int monthDiff = (endTime.Year - startTime.Year) * 12 + (endTime.Month - startTime.Month);
            if (monthDiff >= 2)
            {
                string script = string.Format("$.showPromptDialog('{0}', '{1}', '{2}');", DataAnnotations.TimeRangeOver2Months, UIText.InformationTitle, UIText.Close);
                System.Web.UI.ScriptManager.RegisterStartupScript(ltCheckInfo, this.GetType(), "tempScript", script, true);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查查询输入条件
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="ltCheckInfo"></param>
        /// <returns></returns>
        protected bool CheckDateInput(DateTime startTime, DateTime endTime, Literal ltCheckInfo)
        {
            if (endTime < startTime)
            {
                string script = string.Format("$.showPromptDialog('{0}', '{1}', '{2}');", DataAnnotations.StartTimeMoreThanEndTimeError, UIText.InformationTitle, UIText.Close);
                System.Web.UI.ScriptManager.RegisterStartupScript(ltCheckInfo, this.GetType(), "tempScript", script, true);
                return false;
            }

            if (DateTime.Parse(endTime.ToString("yyyy-MM-dd")) > DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")))
            {
                string script = string.Format("$.showPromptDialog('{0}', '{1}', '{2}');", DataAnnotations.EndTimeRangeOverNow, UIText.InformationTitle, UIText.Close);
                System.Web.UI.ScriptManager.RegisterStartupScript(ltCheckInfo, this.GetType(), "tempScript", script, true);
                return false;
            }

            if (DateTime.Parse(endTime.AddMonths(-6).ToString("yyyy-MM-dd")) > DateTime.Parse(startTime.ToString("yyyy-MM-dd")))
            {
                string script = string.Format("$.showPromptDialog('{0}', '{1}', '{2}');", DataAnnotations.StartTimeRangeOver6Months, UIText.InformationTitle, UIText.Close);
                System.Web.UI.ScriptManager.RegisterStartupScript(ltCheckInfo, this.GetType(), "tempScript", script, true);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 设置ReportViewer
        /// 隐藏PDF导出
        /// </summary>
        /// <param name="rv"></param>
        protected void SetReportViewer(ReportViewer rv)
        {
            //隐藏PDF导出。。。排版有问题。。
            foreach (RenderingExtension re in rv.LocalReport.ListRenderingExtensions())
            {
                if (re.Name.ToUpper() == "PDF")
                {
                    FieldInfo fi = re.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                    fi.SetValue(re, false);
                    break;
                }
            }
        }

        /// <summary>
        /// 重定向到未授权页面
        /// </summary>
        private void RedirectToUnAuth()
        {
            RouteValueDictionary rvd = new RouteValueDictionary();
            rvd.Add("controller", "Account");
            rvd.Add("action", "UnAuth");
            Response.RedirectToRoute("Default", rvd);
            Response.End();
        }


        protected string GetRealNameFromRptName(string rptName)
        {
            var obj = GetUserSession().Functions.SingleOrDefault(f => f.AreaName == "Reports" && f.ControllerName == "Home" && f.ActionName == rptName);
            return obj.FunctionName;
        }

        protected void DoWebServiceLog(string webFuncName, Exception ex, Literal ltCheckInfo)
        {
            LogHelper.DoWebServiceErrorLog(string.Format("{0}{1}[{2}]", this.CurrentRptRealName, webFuncName, ex.Message));
            string script = string.Format("$.showPromptDialog('{0}', '{1}', '{2}');", UIText.ErrorOccurred, UIText.InformationTitle, UIText.Close);
            System.Web.UI.ScriptManager.RegisterStartupScript(ltCheckInfo, this.GetType(), "tempScript", script, true);
        }

        protected void DoReportLog(string errorMessage, Literal ltCheckInfo)
        {
            LogHelper.DoOtherErrorLog(string.Format("{0}{1}]", this.CurrentRptRealName, errorMessage));
            string script = string.Format("$.showPromptDialog('{0}', '{1}', '{2}');", UIText.ErrorOccurred, UIText.InformationTitle, UIText.Close);
            System.Web.UI.ScriptManager.RegisterStartupScript(ltCheckInfo, this.GetType(), "tempScript", script, true);
        }
        #endregion



        #region 属性



        /// <summary>
        /// 当前报表名
        /// </summary>
        protected string CurrentRptName
        {
            get
            {
                return Request["rptName"].ToString();
            }
        }

        /// <summary>
        /// 当前报表实际的功能名称
        /// </summary>
        protected string CurrentRptRealName
        {
            get
            {
                var obj = GetUserSession().Functions.SingleOrDefault(f =>
                    f.AreaName == "Reports" &&
                    f.ControllerName == "Home" &&
                    f.ActionName == this.CurrentRptName);
                return obj.FunctionName;
            }
        }

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
        #endregion

    }
}
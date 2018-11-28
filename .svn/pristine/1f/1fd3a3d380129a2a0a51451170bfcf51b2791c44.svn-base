using Asiatek.BLL;
using Asiatek.BLL.MSSQL;
using Asiatek.Common;
using Asiatek.Model;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asiatek.TMS.Helpers;
using System.Configuration;

namespace Asiatek.TMS.Controllers
{
    public class AccountController : BaseController
    {

        /// <summary>
        /// 修改系统语言
        /// </summary>
        /// <param name="lang"></param>
        [AllowAnonymous, HttpPost]
        public void ChangeLang(string lang)
        {
            SetCultureInfoCookie(lang);
        }

        /// <summary>
        /// 设置文化（语言信息）到Cookie，用于BaseController中Initialize的语言设置
        /// </summary>
        /// <param name="lang"></param>
        private void SetCultureInfoCookie(string lang)
        {
            HttpCookie ck = new HttpCookie("lang");
            ck.Value = lang;
            ck.Expires = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
            Response.Cookies.Add(ck);
        }


        /// <summary>
        /// 登录
        /// </summary>
        [AllowAnonymous]
        public ActionResult Login()
        {
            UserLoginModel model = new UserLoginModel();

            SetLanguagesSelectList(model);

            return View(model);
        }

        private void SetLanguagesSelectList(UserLoginModel model)
        {
            string cultures = ConfigurationManager.AppSettings["CultureInfo"].ToString();
            var temps = cultures.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            List<dynamic> list = new List<dynamic>();
            temps.ToList().ForEach(cultureInfo =>
            {
                string[] datas = cultureInfo.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                list.Add(new { Name = datas[0], Value = datas[1] });
            });

            HttpCookie ck = Request.Cookies["lang"];
            string currentLang = string.Empty;
            if (ck != null && !string.IsNullOrWhiteSpace(ck.Value))
            {
                currentLang = ck.Value;
            }
            model.LanguagesSelectList = list.ToSelectList(item =>
            {
                if (item.Value == currentLang)
                {
                    return GetSelectListItem(item.Value, item.Name, true);
                }
                return GetSelectListItem(item.Value, item.Name);
            });
        }

        [HttpPost, AllowAnonymous]
        public ActionResult CheckValidateCode(string validateCode)
        {
            string vc = GetSession("validateCode").ToString();
            return Json(vc.ToUpper() == validateCode.ToUpper());
        }

        /// <summary>
        /// 登录
        /// </summary>
        [AllowAnonymous, HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            var vcObj = GetSession("validateCode");
            if (vcObj == null)
            {
                ModelState.AddModelError("ValidateCode", Asiatek.Resource.DataAnnotations.ValidateCodeError);
                SetLanguagesSelectList(model);
                return View(model);
            }
            string vc = vcObj.ToString();
            if (model.ValidateCode.ToUpper() != vc.ToUpper())
            {
                ModelState.AddModelError("ValidateCode", Asiatek.Resource.DataAnnotations.ValidateCodeError);
                SetLanguagesSelectList(model);
                return View(model);
            }
            Session.Clear();

            UserSessionModel userSession;
            var logingResult = UserBLL.Login(model, out userSession);
            if (!logingResult.Success)
            {
                SetLanguagesSelectList(model);
                ViewBag.Message = logingResult.Message;
                return View(model);
            }


            //登录成功
            SetSession("currentUser", userSession);
            //身份凭证cookie  保存 用户编号|用户名|角色编号|角色名称|角色等级|用户昵称|单位ID|车辆查看模式
            string cookieInfo = userSession.UserId + "|" +
                userSession.UserName + "|" +
                userSession.RoleInfo.RoleID + "|" +
                userSession.RoleInfo.RoleName + "|" +
                (int)userSession.RoleInfo.RoleLevel + "|" +
                userSession.NickName + "|" +
                userSession.StrucID + "|" +
                userSession.VehicleViewMode;
            FormsAuthentication.SetAuthCookie(cookieInfo, model.SavePassword);

            //临时的cookie，用于标识用户是通过登录界面登录的还是保存密码后免登录上来的
            HttpCookie ck = new HttpCookie("loginflag");
            ck.Expires = DateTime.MinValue;
            Response.Cookies.Add(ck);

            SetCultureInfoCookie(model.CultureName);



            //防止开放重定向攻击
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }



            //string vc = GetSession("validateCode").ToString();
            //if (model.ValidateCode.ToUpper() != vc.ToUpper())
            //{
            //    ModelState.AddModelError("ValidateCode", Resources.DataAnnotations.ValidateCodeError);
            //    return View();
            //}
            //Session.Clear();
            //string msg = string.Empty;//登录结果
            //string userID = string.Empty;//存储用户编号
            //int roleID = -1;//角色编号
            //string roleName = string.Empty;//角色名称
            //int roleLevel = -1;//角色等级
            //if (!model.Login(ref msg, out userID, out roleID, out roleName, out roleLevel))//登录失败
            //{
            //    ViewBag.Message = msg;
            //    return View();
            //}

            ////登录成功
            ////这里根据用户信息获取需要放入session中的信息
            //UserSessionModel sessionModel = new UserSessionModel() { UserId = userID, UserName = model.UserName, RoleInfo = new RoleInfoModel() { RoleID = roleID, RoleLevel = (RoleLevelEnum)roleLevel, RoleName = roleName } };
            //sessionModel.BindFunctionsInfo();
            //Session["currentUser"] = sessionModel;

            ////身份凭证cookie  保存 用户编号|用户名|角色编号|角色名称|角色等级
            //FormsAuthentication.SetAuthCookie(userID + "|" + model.UserName + "|" + roleID + "|" + roleName + "|" + roleLevel, model.SavePassword);

            ////临时的cookie，用于标识用户是通过登录界面登录的还是保存密码后免登录上来的
            //HttpCookie ck = new HttpCookie("loginflag");
            //ck.Expires = DateTime.MinValue;
            //Response.Cookies.Add(ck);

            //if (!string.IsNullOrEmpty(model.ReturnUrl))
            //{
            //    return Redirect(model.ReturnUrl);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}
        }


        /// <summary>
        /// 注销
        /// </summary>
        [AllowAnonymous]
        public ActionResult Logout()
        {
            //移除所有cookie与session信息
            HttpCookie ck = Request.Cookies["loginflag"];
            if (ck != null)
            {
                ck.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(ck);
            }
            FormsAuthentication.SignOut();
            Session.Clear();
            return Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// 未授权
        /// </summary>
        [AllowAnonymous]
        public ActionResult UnAuth()
        {
            return View();
        }


        /// <summary>
        /// 获取验证码
        /// </summary>
        [AllowAnonymous]
        public ActionResult GetValidateCode()
        {
            ValidateCode vc = new ValidateCode();
            string code = string.Empty;
            byte[] bts = vc.CreateValidateCode(out code);
            SetSession("validateCode", code);
            return File(bts, "image/jpeg");
        }

    }
}

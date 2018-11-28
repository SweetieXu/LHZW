using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.PersonalSetting.Controllers
{
    public class UserController : BaseController
    {
        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPassword()
        {
            var rptName = GetSession("rptName");
            return PartialView("_ModifyPassword");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ModifyPassword(UserModifyPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.ID = base.CurrentUserID;
                var result = UserBLL.ModifyPassword(model);
                if (result.Success)//修改成功需要重定向到注销页面
                {
                    result.Url = Url.Content("~/Account/Logout");
                }
                return Json(result);
            }
            else
            {
                return PartialView("_ModifyPassword", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("ModifyPassword")]
        public ActionResult CheckOriginalPassword(string originalPassword)
        {
            return Json(UserBLL.CheckOriginalPassword(originalPassword, base.CurrentUserID));
        }
        #endregion



    }
}

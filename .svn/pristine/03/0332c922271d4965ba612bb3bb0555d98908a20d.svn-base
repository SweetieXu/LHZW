using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Web;
using System.Web.Mvc;
using Asiatek.TMS.Helpers;
using System.Web.Security;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        #region 查询
        public ActionResult UserSetting()
        {
            SearchDataWithPagedDatas<UserSearchModel, UserListModel> model = new SearchDataWithPagedDatas<UserSearchModel, UserListModel>();
            model.SearchModel = new UserSearchModel();
            //model.SearchModel.SubordinateStrucSelectList = StructureBLL.GetStructures().ToSelectListWithAll(m => GetSelectListItem(m.ID, m.StrucName));
            model.SearchModel.StrucID = -1;
            model.SearchModel.VehicleViewMode = -1;


            model.PagedDatas = UserBLL.GetPagedUserInfo(model.SearchModel, 1, this.PageSize, base.IsSuperAdmin);



            return PartialView("_UserSetting", model);
        }


        [AsiatekSubordinateFunction("UserSetting")]
        public ActionResult GetUserInfo(UserSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<UserSearchModel, UserListModel> result = new SearchDataWithPagedDatas<UserSearchModel, UserListModel>();
            result.SearchModel = model;

            result.PagedDatas = UserBLL.GetPagedUserInfo(model, searchPage, this.PageSize, base.IsSuperAdmin);

            return PartialView("_UserPagedGrid", result);
        }

        [AsiatekSubordinateFunction("DistributionSetting", "StrucVehicleDistribution", "Admin")]
        public ActionResult GetUsers()
        {
            return Json(UserBLL.GetUsers(base.IsSuperAdmin), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 新增
        //public ActionResult AddUserInfo()
        //{
        //    UserAddModel model = new UserAddModel();
        //    int roleLevel = (int)GetUserSession().RoleInfo.RoleLevel;
        //    model.RoleSelectList = RoleBLL.GetRolesByCurrentUserRoleLevel(roleLevel).ToSelectList(m => GetSelectListItem(m.ID, m.RoleName));
        //    //model.StrucSelectList = StructureBLL.GetStructures().ToSelectList(m => GetSelectListItem(m.ID, m.StrucName));
        //    //model.StrucSelectList.First().Selected = true;
        //    return PartialView("_AddUserInfo", model);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult AddUserInfo(UserAddModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = UserBLL.Adduser(model,base.UserIDForLog);
        //        base.DoLog(OperationTypeEnum.Add, result, "UserName:" + model.UserName);
        //        return Json(result);
        //    }
        //    else
        //    {
        //        int roleLevel = (int)GetUserSession().RoleInfo.RoleLevel;
        //        model.RoleSelectList = RoleBLL.GetRolesByCurrentUserRoleLevel(roleLevel).ToSelectList(m => GetSelectListItem(m.ID, m.RoleName));
        //        //model.StrucSelectList = StructureBLL.GetStructures().ToSelectList(m => GetSelectListItem(m.ID, m.StrucName));
        //        return PartialView("_AddUserInfo", model);
        //    }
        //}


        //[HttpPost, AsiatekSubordinateFunction("AddUserInfo")]
        //public ActionResult CheckAddUserNameExists(string userName)
        //{
        //    return Json(!UserBLL.CheckUserNameExists(userName));
        //}
        #endregion

        #region 编辑
        //public ActionResult EditUserInfo(int id)
        //{
        //    var obj = UserBLL.GetUserByID(id);
        //    if (obj.DataResult == null)
        //    {
        //        return Content(obj.Message);
        //    }
        //    var model = obj.DataResult;
        //    int roleLevel = (int)GetUserSession().RoleInfo.RoleLevel;
        //    model.RoleSelectList = RoleBLL.GetRolesByCurrentUserRoleLevel(roleLevel).ToSelectList(m => GetSelectListItem(m.ID, m.RoleName));
        //    //model.StrucSelectList = StructureBLL.GetStructures().ToSelectList(m => GetSelectListItem(m.ID, m.StrucName));
        //    return PartialView("_EditUserInfo", model);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult EditUserInfo(UserEditModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = UserBLL.EditUser(model,base.UserIDForLog);
        //        base.DoLog(OperationTypeEnum.Edit, result, "UserID:" + model.ID);
        //        return Json(result);
        //    }
        //    else
        //    {
        //        int roleLevel = (int)GetUserSession().RoleInfo.RoleLevel;
        //        model.RoleSelectList = RoleBLL.GetRolesByCurrentUserRoleLevel(roleLevel).ToSelectList(m => GetSelectListItem(m.ID, m.RoleName));
        //        //model.StrucSelectList = StructureBLL.GetStructures().ToSelectList(m => GetSelectListItem(m.ID, m.StrucName));
        //        return PartialView("_EditUserInfo", model);
        //    }
        //}


        //[HttpPost, AsiatekSubordinateFunction("EditUserInfo")]
        //public ActionResult CheckEditUserNameExists(string userName, int id)
        //{
        //    return Json(!UserBLL.CheckUserNameExists(userName, id));
        //}
        #endregion

        #region 新增编辑用户新版
        #region 新增
        public ActionResult AddUserInfo()
        {
            UserAddModel model = new UserAddModel();
            int roleLevel = (int)GetUserSession().RoleInfo.RoleLevel;
            model.RoleSelectList = RoleBLL.GetRolesByCurrentUserRoleLevel(roleLevel).ToSelectList(m => GetSelectListItem(m.ID, m.RoleName));
            model.VehicleViewMode = true;
            return PartialView("_AddUserInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddUserInfo(UserAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = UserBLL.Adduser(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "UserName:" + model.UserName);
                return Json(result);
            }
            else
            {
                int roleLevel = (int)GetUserSession().RoleInfo.RoleLevel;
                model.RoleSelectList = RoleBLL.GetRolesByCurrentUserRoleLevel(roleLevel).ToSelectList(m => GetSelectListItem(m.ID, m.RoleName));
                //model.StrucSelectList = StructureBLL.GetStructures().ToSelectList(m => GetSelectListItem(m.ID, m.StrucName));
                return PartialView("_AddUserInfo", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("AddUserInfo")]
        public ActionResult CheckAddUserNameExists(string userName)
        {
            return Json(!UserBLL.CheckUserNameExists(userName));
        }
        #endregion

        #region 编辑
        public ActionResult EditUserInfo(int id)
        {
            var obj = UserBLL.GetUserByID(id);
            if (obj.DataResult == null)
            {
                return Content(obj.Message);
            }
            var model = obj.DataResult;
            int roleLevel = (int)GetUserSession().RoleInfo.RoleLevel;
            model.RoleSelectList = RoleBLL.GetRolesByCurrentUserRoleLevel(roleLevel).ToSelectList(m => GetSelectListItem(m.ID, m.RoleName));
            //model.StrucSelectList = StructureBLL.GetStructures().ToSelectList(m => GetSelectListItem(m.ID, m.StrucName));
            return PartialView("_EditUserInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditUserInfo(UserEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = UserBLL.EditUser(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "UserID:" + model.ID);
                return Json(result);
            }
            else
            {
                int roleLevel = (int)GetUserSession().RoleInfo.RoleLevel;
                model.RoleSelectList = RoleBLL.GetRolesByCurrentUserRoleLevel(roleLevel).ToSelectList(m => GetSelectListItem(m.ID, m.RoleName));
                //model.StrucSelectList = StructureBLL.GetStructures().ToSelectList(m => GetSelectListItem(m.ID, m.StrucName));
                return PartialView("_EditUserInfo", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("EditUserInfo")]
        public ActionResult CheckEditUserNameExists(string userName, int id)
        {
            return Json(!UserBLL.CheckUserNameExists(userName, id));
        }
        #endregion
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteUser(FormCollection fc)
        {
            string[] ids = fc["userid"].Split(',');

            //var result = UserBLL.DeleteUser(ids);
            var result = UserBLL.DeleteUserPhysical(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["userid"]);
            return Json(result);
        }
        #endregion

        #region 重置密码
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetUserPassword(string userids)
        {
            string[] ids = userids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var result = UserBLL.ResetUserPwd(ids);

            base.DoLog(OperationTypeEnum.Edit, result, "重置密码：" + userids);
            return Json(result);
        }


        #endregion

        #region 切换用户
        /// <summary>
        /// 切换到指定ID的用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SwitchUser(int id)
        {
            //切换用户
            //1。清除当前登录用户的cookie与session信息，相当于注销
            //2。根据用户ID查询相关信息 ，相当于登录
            //3。添加一个特殊标志，标示用户是切换过去的，要在原用户的基础上添加上 “返回之前用户”的权限

            int currentUserID = base.CurrentUserID;

            #region 重新获取用户信息

            UserSessionModel userSession;
            var result = UserBLL.GetUserForSwitch(id, out userSession);
            if (!result.Success)
            {
                return Json(result);
            }
            #endregion

            #region 清理
            HttpCookie ck = Request.Cookies["loginflag"];
            if (ck != null)
            {
                ck.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(ck);
            }
            FormsAuthentication.SignOut();
            Session.Clear();
            #endregion

            #region 重新设置用户信息

            SetSession("currentUser", userSession);

            //身份凭证cookie  保存    用户编号|用户名|角色编号|角色名称|角色等级|用户昵称|单位ID|车辆查看模式
            string temp = userSession.UserId + "|" + userSession.UserName + "|" + userSession.RoleInfo.RoleID + "|" + userSession.RoleInfo.RoleName + "|" + (int)userSession.RoleInfo.RoleLevel + "|" + userSession.NickName + "|" + userSession.StrucID + "|" + userSession.VehicleViewMode; 
            FormsAuthentication.SetAuthCookie(temp, false);
            //临时的cookie，用于标识用户是通过登录界面登录的还是保存密码后免登录上来的
            ck = new HttpCookie("loginflag");
            ck.Expires = DateTime.MinValue;
            Response.Cookies.Add(ck);
            #endregion


            #region 添加标识
            SetSession("OriginalUserID", currentUserID);
            #endregion

            return Json(new { Url = FormsAuthentication.DefaultUrl });
        }

        /// <summary>
        ///  恢复之前用户
        /// </summary>
        /// <returns></returns>
        [AsiatekPassPremission, HttpPost]
        public ActionResult SwitchToOriginalUser()
        {
            int id = Convert.ToInt32(Session["OriginalUserID"]);//获取切换之前的用户ID

            #region 重新获取用户信息

            UserSessionModel userSession;
            var result = UserBLL.GetUserForSwitch(id, out userSession);
            if (!result.Success)
            {
                return Json(result);
            }
            #endregion

            #region 清理
            HttpCookie ck = Request.Cookies["loginflag"];
            if (ck != null)
            {
                ck.Expires = DateTime.Now.AddYears(-1);
                Response.Cookies.Add(ck);
            }
            FormsAuthentication.SignOut();
            Session.Clear();
            #endregion

            #region 重新赋值
            SetSession("currentUser", userSession);
            //身份凭证cookie  保存    用户编号|用户名|角色编号|角色名称|角色等级|用户昵称|单位ID
            string temp = userSession.UserId + "|" + userSession.UserName + "|" + userSession.RoleInfo.RoleID + "|" + userSession.RoleInfo.RoleName + "|" + (int)userSession.RoleInfo.RoleLevel + "|" + userSession.NickName + "|" + userSession.StrucID + "|" + userSession.VehicleViewMode; ;
            FormsAuthentication.SetAuthCookie(temp, true);

            //临时的cookie，用于标识用户是通过登录界面登录的还是保存密码后免登录上来的
            ck = new HttpCookie("loginflag");
            ck.Expires = DateTime.MinValue;
            Response.Cookies.Add(ck);
            #endregion


            return Json(new { Url = FormsAuthentication.DefaultUrl });
        }
        #endregion
    }
}

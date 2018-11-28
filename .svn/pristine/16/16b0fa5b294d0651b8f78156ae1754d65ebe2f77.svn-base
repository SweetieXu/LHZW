using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {


        #region 查询


        #region 旧代码
        //public ActionResult RoleSetting()
        //{
        //    var data = RoleBLL.GetPagedRoleInfo(1, this.PageSize, base.IsSuperAdmin);
        //    return PartialView("_RoleSetting", data);
        //}



        //public ActionResult RolePagedGrid(string roleName, int currentPage = 1)
        //{
        //    return GetControllerPagedGridPV(roleName, currentPage);
        //}




        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult GetRoleInfoByRoleName(string roleName)
        //{
        //    return GetControllerPagedGridPV(roleName, 1);
        //}


        //private ActionResult GetControllerPagedGridPV(string roleName, int currentPage)
        //{
        //    ViewBag.RoleName = roleName;
        //    var obj = RoleBLL.GetPagedRoleInfo(currentPage, this.PageSize, base.IsSuperAdmin, roleName);
        //    return PartialView("_RolePagedGrid", obj);
        //}
        #endregion

        public ActionResult RoleSetting()
        {
            RoleSettingModel model = new RoleSettingModel();
            model.SearchPage = 1;
            model.PagedDatas = RoleBLL.GetPagedRoleInfo(model, this.PageSize, base.IsSuperAdmin);
            return PartialView("_RoleSetting", model);
        }



        [AsiatekSubordinateFunction("RoleSetting")]
        public ActionResult GetRoleInfo(RoleSettingModel model)
        {
            return GetControllerPagedGridPV(model);
        }


        private ActionResult GetControllerPagedGridPV(RoleSettingModel model)
        {
            model.PagedDatas = RoleBLL.GetPagedRoleInfo(model, this.PageSize, base.IsSuperAdmin);
            return PartialView("_RolePagedGrid", model);
        }
        #endregion


        #region 删除

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult DeleteRole(FormCollection fc)
        //{
        //    int currentPage = Convert.ToInt32(fc["currentPage"]);
        //    string roleName = fc["roleName"];
        //    string[] ids = fc["roleid"].Split(',');


        //    int[] idValues = new int[ids.Length];
        //    for (int i = 0; i < idValues.Length; i++)
        //    {
        //        idValues[i] = Convert.ToInt32(ids[i]);
        //    }

        //    var result = RoleBLL.DeleteRoles(idValues);
        //    base.DoLog(Model.OperationTypeEnum.Delete, result, fc["roleid"]);
        //    ViewBag.Message = result.Message;
        //    return GetControllerPagedGridPV(roleName, currentPage);
        //}

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteRole(FormCollection fc)
        {
            string[] ids = fc["roleid"].Split(',');


            int[] idValues = new int[ids.Length];
            for (int i = 0; i < idValues.Length; i++)
            {
                idValues[i] = Convert.ToInt32(ids[i]);
            }

            var result = RoleBLL.DeleteRoles(idValues);
            base.DoLog(Model.OperationTypeEnum.Delete, result, fc["roleid"]);
            return Json(result);
        }
        #endregion


        #region 增加

        public ActionResult AddRole()
        {
            RoleAddModel model = new RoleAddModel();
            if (IsSuperAdmin)
            {
                model.FunctionTreeNodes = FunctionBLL.GetAllFunctionsForTree();
            }
            else
            {
                model.FunctionTreeNodes = FunctionBLL.GetNormalFunctionsForTree();
            }
            return PartialView("_AddRole", model);

        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddRole(RoleAddModel model)
        {
            if (model.FunctionIDs.Count == 0)
            {
                ModelState.AddModelError("FunctionIDs", Asiatek.Resource.DataAnnotations.RoleMustHaveOneFunction);
            }
            if (ModelState.IsValid)
            {
                var result = RoleBLL.AddRoleInfo(model,base.UserIDForLog);
                base.DoLog(Model.OperationTypeEnum.Add, result, "RoleName:" + model.RoleName);
                return Json(result);
            }
            else
            {
                if (IsSuperAdmin)
                {
                    model.FunctionTreeNodes = FunctionBLL.GetAllFunctionsForTree();
                }
                else
                {
                    model.FunctionTreeNodes = FunctionBLL.GetNormalFunctionsForTree();
                }
                model.FunctionTreeNodes.ForEach(node =>
                {
                    if (model.FunctionIDs.Contains(node.ID))
                    {
                        node.Checked = true;
                    }
                });
                return PartialView("_AddRole", model);
            }
        }






        [HttpPost, AsiatekSubordinateFunction("AddRole")]
        public ActionResult CheckAddRoleNameExists(string roleName)
        {
            bool result = RoleBLL.CheckRoleNameExists(roleName);
            return Json(!result);
        }
        #endregion

        #region 修改
        [HttpPost, AsiatekSubordinateFunction("EditRole")]
        public ActionResult CheckEditRoleNameExists(string roleName, int ID)
        {
            bool result = RoleBLL.CheckRoleNameExists(roleName, ID);
            return Json(!result);
        }







        public ActionResult EditRole(int id)
        {
            var obj = RoleBLL.GetRoleByID(id);
            if (obj.DataResult == null)
            {
                return Content(obj.Message);
            }
            var role = obj.DataResult;
            if (IsSuperAdmin)
            {
                role.FunctionTreeNodes = FunctionBLL.GetAllFunctionsForTree();
            }
            else
            {
                role.FunctionTreeNodes = FunctionBLL.GetNormalFunctionsForTree();
            }
            role.FunctionTreeNodes.ForEach(node =>
            {
                if (role.FunctionIDs.Contains(node.ID))
                {
                    node.Checked = true;
                }
            });
            return PartialView("_EditRole", role);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditRole(RoleEditModel model)
        {
            if (model.FunctionIDs.Count == 0)
            {
                ModelState.AddModelError("FunctionIDs", Asiatek.Resource.DataAnnotations.RoleMustHaveOneFunction);
            }
            if (ModelState.IsValid)
            {
                var result = RoleBLL.ModifyRoleInfo(model,base.UserIDForLog);
                base.DoLog(Model.OperationTypeEnum.Edit, result, "RoleID:" + model.ID);
                return Json(result);
            }
            else
            {
                if (IsSuperAdmin)
                {
                    model.FunctionTreeNodes = FunctionBLL.GetAllFunctionsForTree();
                }
                else
                {
                    model.FunctionTreeNodes = FunctionBLL.GetNormalFunctionsForTree();
                }
                model.FunctionTreeNodes.ForEach(node =>
                {
                    if (model.FunctionIDs.Contains(node.ID))
                    {
                        node.Checked = true;
                    }
                });

                return PartialView("_EditRole", model);
            }
        }
        #endregion






        //[AsiatekPassPremission]
        //public ActionResult GetControllerDDLWithDefaultByAreaID(int areaID)
        //{
        //    List<Asiatek.Model.ControllerDDLModel> list;
        //    if (areaID == -1)
        //    {
        //        list = ControllerBLL.GetControllers();
        //    }
        //    else
        //    {
        //        list = ControllerBLL.GetControllersByAreaID(areaID);
        //    }
        //    list.Insert(0, new Model.ControllerDDLModel() { ID = -1, ControllerName = Asiatek.Resource.UIText.All });
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        //[AsiatekPassPremission]
        //public ActionResult GetControllerDDLByAreaID(int areaID)
        //{
        //    var list = ControllerBLL.GetControllersByAreaID(areaID);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}


    }
}

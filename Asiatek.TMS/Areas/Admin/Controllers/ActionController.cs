using Asiatek.BLL.MSSQL;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.TMS.Helpers;
using Asiatek.Model;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class ActionController : BaseController
    {



        #region 查询
        /// <summary>
        /// 动作设定
        /// </summary>
        public ActionResult ActionSetting()
        {
            ActionSettingModel model = new ActionSettingModel();
            var areaList = AreaBLL.GetAreas();
            model.AreasSelectList = areaList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.AreaName));

            var ctrList = ControllerBLL.GetControllers();
            model.ControllersSelectList = ctrList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.ControllerName));

            model.SearchPage = 1;
            model.AreaID = -1;
            model.ControllerID = -1;
            model.PagedDatas = ActionBLL.GetPagedActions(model, PageSize);

            return PartialView("_ActionSetting", model);
        }

        [AsiatekSubordinateFunction("ActionSetting")]
        public ActionResult GetActionInfo(ActionSettingModel model)
        {
            return GetActionPagedGridPV(model);
        }



        private ActionResult GetActionPagedGridPV(ActionSettingModel model)
        {
            model.PagedDatas = ActionBLL.GetPagedActions(model, PageSize);
            return PartialView("_ActionPagedGrid", model);
        }


        #region 旧代码
        ///// <summary>
        ///// 动作设定
        ///// </summary>
        //public ActionResult ActionSetting()
        //{
        //    ViewBag.AreaID = -1;
        //    ViewBag.ControllerID = -1;
        //    ViewBag.ActionName = string.Empty;

        //    Asiatek.Model.ActionSettingModel model = new Model.ActionSettingModel();
        //    model.PagedActions = ActionBLL.GetPagedActions(PageSize);
        //    var areaList = AreaBLL.GetAreas();
        //    model.AreasSelectList = areaList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.AreaName));
        //    var ctrList = ControllerBLL.GetControllers();
        //    model.ControllersSelectList = ctrList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.ControllerName));
        //    return PartialView("_ActionSetting", model);
        //}


        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult GetActionInfo(string actionName, int areaID, int controllerID)
        //{
        //    return GetActionPagedGridPV(actionName, areaID, controllerID, 1);
        //}

        //private ActionResult GetActionPagedGridPV(string actionName, int areaID, int controllerID, int currentPage)
        //{
        //    ViewBag.AreaID = areaID;
        //    ViewBag.ControllerID = controllerID;
        //    ViewBag.ActionName = actionName;
        //    return PartialView("_ActionPagedGrid", ActionBLL.GetPagedActions(PageSize, currentPage, actionName, controllerID, areaID));
        //}

        //public ActionResult ActionPagedGrid(string actionName, int areaID = -1, int controllerID = -1, int currentPage = 1)
        //{
        //    return GetActionPagedGridPV(actionName, areaID, controllerID, currentPage);
        //}
        #endregion








        [HttpPost, AsiatekSubordinateFunction("AddFunctionInfo", "Function", "Admin")]
        public ActionResult CheckActionIDWhenAddFunction(int? actionID, bool functionIsMenu)
        {
            if (functionIsMenu)
            {
                return Json(true);
            }
            return Json(!ActionBLL.CheckActionIDHadBeenLinkedWithFunction(actionID.Value));
        }



        [HttpPost, AsiatekSubordinateFunction("EditFunctionInfo", "Function", "Admin")]
        public ActionResult CheckActionIDWhenEditFunction(int? actionID, int ID, bool functionIsMenu)
        {
            if (functionIsMenu)
            {
                return Json(true);
            }
            return Json(!ActionBLL.CheckActionIDHadBeenLinkedWithFunction(actionID.Value, ID));
        }

        [AsiatekSubordinateFunction("AddFunctionInfo", "Function", "Admin")]
        [AsiatekSubordinateFunction("EditFunctionInfo", "Function", "Admin")]
        public ActionResult GetActionDDLByControllerID(int controllerID)
        {
            var list = ActionBLL.GetActionsByControllerID(controllerID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region 修改
        public ActionResult EditActionInfo(int id)
        {
            var obj = ActionBLL.GetActionByID(id);
            if (obj.DataResult == null)
            {
                return Content(obj.Message);
            }

            int areaID = obj.DataResult.AreaID;
            obj.DataResult.AreasSelectList = new SelectList(AreaBLL.GetAreas(), "ID", "AreaName", areaID);
            obj.DataResult.ControllersSelectList = new SelectList(ControllerBLL.GetControllersByAreaID(areaID), "ID", "ControllerName");
            return PartialView("_EditActionInfo", obj.DataResult);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditActionInfo(Asiatek.Model.ActionEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ActionBLL.ModifyAction(model,base.UserIDForLog);
                base.DoLog(Model.OperationTypeEnum.Edit, result, "ActionID:" + model.ID);
                return Json(result);
            }
            else
            {
                int areaID = model.AreaID;
                model.AreasSelectList = new SelectList(AreaBLL.GetAreas(), "ID", "AreaName", areaID);
                model.ControllersSelectList = new SelectList(ControllerBLL.GetControllersByAreaID(areaID), "ID", "ControllerName", model.ControllerID);
                return PartialView("_EditActionInfo", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("EditActionInfo")]
        public ActionResult CheckEditActionNameExists(string actionName, int controllerID, int ID)
        {
            return Json(!ActionBLL.CheckActionNameExists(actionName, controllerID, ID));
        }
        #endregion

        #region 新增
        public ActionResult AddActionInfo()
        {
            Asiatek.Model.ActionAddModel model = new Asiatek.Model.ActionAddModel();
            var aList = AreaBLL.GetAreas();
            model.AreasSelectList = new SelectList(aList, "ID", "AreaName");
            var cList = ControllerBLL.GetControllersByAreaID(aList.First().ID);
            model.ControllersSelectList = new SelectList(cList, "ID", "ControllerName");
            return PartialView("_AddActionInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddActionInfo(ActionAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ActionBLL.AddAction(model, base.UserIDForLog);
                base.DoLog(Model.OperationTypeEnum.Add, result, "ActionName:" + model.ActionName);
                return Json(result);
            }
            else
            {
                int areaID = model.AreaID;
                var aList = AreaBLL.GetAreas();
                model.AreasSelectList = new SelectList(aList, "ID", "AreaName", areaID);
                var cList = ControllerBLL.GetControllersByAreaID(areaID);
                model.ControllersSelectList = new SelectList(cList, "ID", "ControllerName", model.ControllerID);
                return PartialView("_AddActionInfo", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("AddActionInfo")]
        public ActionResult CheckAddActionNameExists(string actionName, int controllerID)
        {
            return Json(!ActionBLL.CheckActionNameExists(actionName, controllerID));
        }
        #endregion




        #region 删除

        #region 旧代码
        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult DeleteAction(FormCollection fc)
        //{
        //    int currentPage = Convert.ToInt32(fc["currentPage"]);
        //    string actionName = fc["actionName"];
        //    int areaID = Convert.ToInt32(fc["areaID"]);
        //    int controllerID = Convert.ToInt32(fc["controllerID"]);
        //    string[] ids = fc["actid"].Split(',');

        //    var result = ActionBLL.DeleteActions(ids);

        //    base.DoLog(Model.OperationTypeEnum.Delete, result, fc["actid"]);
        //    ViewBag.Message = result.Message;
        //    return GetActionPagedGridPV(actionName, areaID, controllerID, currentPage);
        //}
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteAction(FormCollection fc)
        {
            string[] ids = fc["actid"].Split(',');

            var result = ActionBLL.DeleteActions(ids);
            base.DoLog(Model.OperationTypeEnum.Delete, result, fc["actid"]);
            return Json(result);
        }
        #endregion




    }
}

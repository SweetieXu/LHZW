using Asiatek.BLL.MSSQL;
using Asiatek.TMS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.TMS.Helpers;
using Asiatek.Model;
using Asiatek.TMS.Filters;
using Asiatek.Resource;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class FunctionController : BaseController
    {


        #region 查询


        #region 旧代码
        //public ActionResult FunctionSetting()
        //{

        //    ViewBag.AreaID = -1;
        //    ViewBag.ControllerID = -1;
        //    ViewBag.ParentFunctionID = -1;
        //    ViewBag.FunctionName = string.Empty;

        //    FunctionSettingModel model = new FunctionSettingModel();
        //    model.PagedFunctions = FunctionBLL.GetPagedFunctions(PageSize);
        //    var areaList = AreaBLL.GetAreas();
        //    model.AreasSelectList = areaList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.AreaName));
        //    var ctrList = ControllerBLL.GetControllers();
        //    model.ControllersSelectList = ctrList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.ControllerName));

        //    var parentFuncList = FunctionBLL.GetFunctions();
        //    model.ParentFunctionsSelectList = parentFuncList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.FunctionName));

        //    return PartialView("_FunctionSetting", model);
        //}

        //private ActionResult GetFunctionPagedGridPV(string functionName, int areaID, int controllerID, int parentFunctionID, int currentPage)
        //{
        //    ViewBag.AreaID = areaID;
        //    ViewBag.ControllerID = controllerID;
        //    ViewBag.FunctionName = functionName;
        //    ViewBag.ParentFunctionID = parentFunctionID;
        //    return PartialView("_FunctionPagedGrid", FunctionBLL.GetPagedFunctions(PageSize, currentPage, functionName, controllerID, areaID, parentFunctionID));
        //}

        //public ActionResult FunctionPagedGrid(string functionName, int areaID = -1, int controllerID = -1, int currentPage = 1, int parentFunctionID = -1)
        //{
        //    return GetFunctionPagedGridPV(functionName, areaID, controllerID, parentFunctionID, currentPage);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult GetFunctionInfo(string functionName, int areaID, int controllerID, int parentFunctionID)
        //{
        //    return GetFunctionPagedGridPV(functionName, areaID, controllerID, parentFunctionID, 1);
        //}
        #endregion

        public ActionResult FunctionSetting()
        {
            FunctionSettingModel model = new FunctionSettingModel();

            var areaList = AreaBLL.GetAreas();
            model.AreasSelectList = areaList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.AreaName));

            var ctrList = ControllerBLL.GetControllers();
            model.ControllersSelectList = ctrList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.ControllerName));

            var parentFuncList = FunctionBLL.GetFunctions();
            model.ParentFunctionsSelectList = parentFuncList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.FunctionName));


            model.SearchPage = 1;
            model.AreaID = -1;
            model.ControllerID = -1;
            model.ParentFunctionID = -1;
            model.PagedDatas = FunctionBLL.GetPagedFunctions(model, this.PageSize);
            return PartialView("_FunctionSetting", model);
        }


        private ActionResult GetFunctionPagedGridPV(FunctionSettingModel model)
        {
            model.PagedDatas = FunctionBLL.GetPagedFunctions(model, this.PageSize);
            return PartialView("_FunctionPagedGrid", model);
        }


        [AsiatekSubordinateFunction("FunctionSetting")]
        public ActionResult GetFunctionInfo(FunctionSettingModel model)
        {
            return GetFunctionPagedGridPV(model);
        }


        [AsiatekSubordinateFunction("FunctionSetting")]
        public ActionResult GetFunctionDDLWithDefault(int controllerID, int areaID)
        {
            List<FunctionDDLModel> list;
            if (areaID == -1 && controllerID == -1)//区域与控制器均为全部
            {
                list = FunctionBLL.GetFunctions();
            }
            else if (areaID != -1 && controllerID == -1)//选了区域，但没选控制器
            {
                list = FunctionBLL.GetFunctionsByAreaID(areaID);
            }
            else//选了具体的控制器
            {
                list = FunctionBLL.GetFunctionsByControllerID(controllerID);
            }
            list.Insert(0, new Model.FunctionDDLModel() { ID = -1, FunctionName = UIText.All });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AsiatekSubordinateFunction("AddFunctionInfo")]
        [AsiatekSubordinateFunction("EditFunctionInfo")]
        public ActionResult GetFunctionDDLByControllerID(int controllerID)
        {
            var list = FunctionBLL.GetFunctionsByControllerID(controllerID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }




        [AsiatekSubordinateFunction("AddFunctionInfo")]
        public ActionResult GetParentFunctionByName(string functionName)
        {
            var list = FunctionBLL.GetFunctionsByName(functionName);
            var query = from f in list select new { label = f.FunctionName, value = f.ID };

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region 新增
        //public ActionResult AddFunctionInfo()
        //{
        //    var aList = AreaBLL.GetAreas();
        //    FunctionAddModel model = new FunctionAddModel();
        //    model.AreasSelectList = new SelectList(aList, "ID", "AreaName");

        //    var cList = ControllerBLL.GetControllersByAreaID(aList.First().ID);
        //    model.ControllersSelectList = new SelectList(cList, "ID", "ControllerName");

        //    var actionList = ActionBLL.GetActionsByControllerID(cList.First().ID);
        //    model.ActionsSelectList = new SelectList(actionList, "ID", "ActionName");


        //    var parentFuncList = FunctionBLL.GetFunctions();
        //    model.ParentFunctionsSelectList = new SelectList(parentFuncList, "ID", "FunctionName");


        //    return PartialView("_AddFunctionInfo", model);
        //}

        public ActionResult AddFunctionInfo()
        {
            var aList = AreaBLL.GetAreas();
            FunctionAddModel model = new FunctionAddModel();
            model.OrderIndex = Int32.MaxValue;//默认新增的处于最后的位置

            model.AreasSelectList = new SelectList(aList, "ID", "AreaName");

            var cList = ControllerBLL.GetControllersByAreaID(aList.First().ID);
            model.ControllersSelectList = new SelectList(cList, "ID", "ControllerName");

            var actionList = ActionBLL.GetActionsByControllerID(cList.First().ID);

            //model.ActionsSelectList = actionList.ToSelectListWithEmpty(m => GetSelectListItem(m.ID, m.ActionName));
            model.ActionsSelectList = actionList.ToSelectList(m => GetSelectListItem(m.ID, m.ActionName));
            var parentFuncList = FunctionBLL.GetFunctions();
            model.ParentFunctionsSelectList = new SelectList(parentFuncList, "ID", "FunctionName", parentFuncList.First().ID);

            return PartialView("_AddFunctionInfo", model);
        }



        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddFunctionInfo(FunctionAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = FunctionBLL.AddFunction(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "FunctionName:" + model.FunctionName);
                return Json(result);
            }
            else
            {

                if (model.FunctionIsMenu)
                {
                    var aList = AreaBLL.GetAreas();
                    model.AreasSelectList = new SelectList(aList, "ID", "AreaName");
                    var cList = ControllerBLL.GetControllersByAreaID(aList.First().ID);
                    model.ControllersSelectList = new SelectList(cList, "ID", "ControllerName");

                    var actionList = ActionBLL.GetActionsByControllerID(cList.First().ID);
                    model.ActionsSelectList = actionList.ToSelectList(m => GetSelectListItem(m.ID, m.ActionName));

                    var parentFuncList = FunctionBLL.GetFunctions();
                    model.ParentFunctionsSelectList = new SelectList(parentFuncList, "ID", "FunctionName");
                    return PartialView("_AddFunctionInfo", model);
                }
                else
                {
                    int areaID = model.AreaID.Value;
                    int controllerID = model.ControllerID.Value;
                    var aList = AreaBLL.GetAreas();
                    model.AreasSelectList = new SelectList(aList, "ID", "AreaName");

                    var cList = ControllerBLL.GetControllersByAreaID(areaID);
                    model.ControllersSelectList = new SelectList(cList, "ID", "ControllerName");

                    var actionList = ActionBLL.GetActionsByControllerID(controllerID);
                    model.ActionsSelectList = actionList.ToSelectList(m => GetSelectListItem(m.ID, m.ActionName));

                    var parentFuncList = FunctionBLL.GetFunctions();
                    model.ParentFunctionsSelectList = new SelectList(parentFuncList, "ID", "FunctionName");
                    return PartialView("_AddFunctionInfo", model);
                }
            }
        }


        [HttpPost, AsiatekSubordinateFunction("AddFunctionInfo")]
        public ActionResult CheckAddFunctionNameExists(string functionName)
        {
            return Json(!FunctionBLL.CheckFunctionNameExists(functionName));
        }

        [HttpPost, AsiatekSubordinateFunction("AddFunctionInfo")]
        public ActionResult CheckAddFeaturesCodeExists(string featuresCode)
        {
            return Json(!FunctionBLL.CheckAddFunctionNameExists(featuresCode));
        }

        #endregion



        #region 编辑
        //public ActionResult EditFunctionInfo(int id)
        //{
        //    string message = string.Empty;
        //    var obj = FunctionBLL.GetFunctionByID(id);
        //    if (obj.DataResult == null)
        //    {
        //        return Content(obj.Message);
        //    }


        //    var model = obj.DataResult;

        //    model.IsTopFunction = model.ParentID == null;

        //    model.AreasSelectList = new SelectList(AreaBLL.GetAreas(), "ID", "AreaName");

        //    model.ControllersSelectList = new SelectList(ControllerBLL.GetControllersByAreaID(model.AreaID), "ID", "ControllerName");

        //    model.ActionsSelectList = new SelectList(ActionBLL.GetActionsByControllerID(model.ControllerID), "ID", "ActionName");

        //    model.ParentFunctionsSelectList = new SelectList(FunctionBLL.GetFunctions(), "ID", "FunctionName");

        //    return PartialView("_EditFunctionInfo", model);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult EditFunctionInfo(FunctionEditModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = FunctionBLL.EditFunction(model);
        //        base.DoLog(OperationTypeEnum.Edit, result, "FunctionID:" + model.ID);
        //        return Json(result);
        //    }
        //    else
        //    {
        //        int areaID = model.AreaID;
        //        int controllerID = model.ControllerID;
        //        int? parentID = model.ParentID;
        //        int actionID = model.ActionID;
        //        model.IsTopFunction = model.ParentID == null;


        //        model.AreasSelectList = new SelectList(AreaBLL.GetAreas(), "ID", "AreaName", areaID);

        //        model.ControllersSelectList = new SelectList(ControllerBLL.GetControllersByAreaID(areaID), "ID", "ControllerName", controllerID);

        //        model.ActionsSelectList = new SelectList(ActionBLL.GetActionsByControllerID(controllerID), "ID", "ActionName", actionID);

        //        model.ParentFunctionsSelectList = new SelectList(FunctionBLL.GetFunctions(), "ID", "FunctionName", parentID);

        //        return PartialView("_EditFunctionInfo", model);
        //    }
        //}




        public ActionResult EditFunctionInfo(int id)
        {
            var obj = FunctionBLL.GetFunctionByID(id);
            if (obj.DataResult == null)
            {
                return Content(obj.Message);
            }


            var model = obj.DataResult;

            model.IsTopFunction = model.ParentID == null;
            model.FunctionIsMenu = model.ActionID == null;

            var areas = AreaBLL.GetAreas();
            model.AreasSelectList = new SelectList(areas, "ID", "AreaName");


            List<ControllerDDLModel> controllers;
            if (!model.AreaID.HasValue)//没有与Action绑定过的功能
            {
                controllers = ControllerBLL.GetControllersByAreaID(areas.First().ID);
            }
            else
            {
                controllers = ControllerBLL.GetControllersByAreaID(model.AreaID.Value);
            }
            model.ControllersSelectList = new SelectList(controllers, "ID", "ControllerName");

            List<ActionDDLModel> actions;
            if (!model.ControllerID.HasValue)//没有与Action绑定过的功能
            {
                actions = ActionBLL.GetActionsByControllerID(controllers.First().ID);
            }
            else
            {
                actions = ActionBLL.GetActionsByControllerID(model.ControllerID.Value);
            }

            model.ActionsSelectList = actions.ToSelectList(m => GetSelectListItem(m.ID, m.ActionName));

            var parentFunctions = FunctionBLL.GetFunctions(model.ID);

            //如果是顶级功能，则没有上级功能，所以默认选一个
            if (model.IsTopFunction)
            {
                var tempPid = parentFunctions.First().ID;
                model.ParentFunctionsSelectList = new SelectList(parentFunctions, "ID", "FunctionName", tempPid);
                model.ParentID = tempPid;
            }
            else
            {
                model.ParentFunctionsSelectList = new SelectList(parentFunctions, "ID", "FunctionName", model.ParentID);
            }

            return PartialView("_EditFunctionInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditFunctionInfo(FunctionEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = FunctionBLL.EditFunction(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "FunctionID:" + model.ID);
                return Json(result);
            }
            else
            {

                if (model.FunctionIsMenu)
                {
                    var aList = AreaBLL.GetAreas();
                    model.AreasSelectList = new SelectList(aList, "ID", "AreaName");
                    var cList = ControllerBLL.GetControllersByAreaID(aList.First().ID);
                    model.ControllersSelectList = new SelectList(cList, "ID", "ControllerName");

                    var actionList = ActionBLL.GetActionsByControllerID(cList.First().ID);
                    model.ActionsSelectList = actionList.ToSelectList(m => GetSelectListItem(m.ID, m.ActionName));

                    var parentFuncList = FunctionBLL.GetFunctions();
                    model.ParentFunctionsSelectList = new SelectList(FunctionBLL.GetFunctions(model.ID), "ID", "FunctionName");
                    return PartialView("_EditFunctionInfo", model);
                }
                else
                {
                    int areaID = model.AreaID.Value;
                    int controllerID = model.ControllerID.Value;
                    int? parentID = model.ParentID;
                    int actionID = model.ActionID.Value;
                    model.IsTopFunction = model.ParentID == null;


                    model.AreasSelectList = new SelectList(AreaBLL.GetAreas(), "ID", "AreaName", areaID);

                    model.ControllersSelectList = new SelectList(ControllerBLL.GetControllersByAreaID(areaID), "ID", "ControllerName", controllerID);

                    model.ActionsSelectList = ActionBLL.GetActionsByControllerID(controllerID).ToSelectList(m => GetSelectListItem(m.ID, m.ActionName));

                    model.ParentFunctionsSelectList = new SelectList(FunctionBLL.GetFunctions(model.ID), "ID", "FunctionName");

                    return PartialView("_EditFunctionInfo", model);
                }




            }
        }


        [HttpPost, AsiatekSubordinateFunction("EditFunctionInfo")]
        public ActionResult CheckEditFunctionNameExists(string functionName, int ID)
        {
            return Json(!FunctionBLL.CheckFunctionNameExists(functionName, ID));
        }

        [HttpPost, AsiatekSubordinateFunction("EditFunctionInfo")]
        public ActionResult CheckEditFeaturesCodeExists(string featuresCode, int ID)
        {
            return Json(!FunctionBLL.CheckEditFeaturesCodeExists(featuresCode, ID));
        }
        #endregion



        #region 删除

        #region 旧代码
        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult DeleteFunction(FormCollection fc)
        //{
        //    int currentPage = Convert.ToInt32(fc["currentPage"]);
        //    string functionName = fc["functionName"];
        //    int areaID = Convert.ToInt32(fc["areaID"]);
        //    int controllerID = Convert.ToInt32(fc["controllerID"]);
        //    int parentFunctionID = Convert.ToInt32(fc["parentFunctionID"]);
        //    string[] ids = fc["funid"].Split(',');


        //    var result = FunctionBLL.DeleteFunctions(ids);
        //    base.DoLog(OperationTypeEnum.Delete, result, fc["funid"]);

        //    ViewBag.Message = result.Message;
        //    return GetFunctionPagedGridPV(functionName, areaID, controllerID, parentFunctionID, currentPage);
        //} 
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteFunction(FormCollection fc)
        {
            string[] ids = fc["funid"].Split(',');

            var result = FunctionBLL.DeleteFunctions(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["funid"]);
            return Json(result);
        }
        #endregion


    }
}

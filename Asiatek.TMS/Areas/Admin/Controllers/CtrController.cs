using Asiatek.BLL.MSSQL;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Asiatek.TMS.Helpers;
using Asiatek.Model;
using Asiatek.AjaxPager;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class CtrController : BaseController
    {


        #region 查询
        public ActionResult ControllerInfo()
        {
            SearchDataWithPagedDatas<ControllerSearchModel, ControllerListModel> model = new SearchDataWithPagedDatas<ControllerSearchModel, ControllerListModel>();
            model.SearchPage = 1;

            model.SearchModel = new ControllerSearchModel();
            model.SearchModel.AreaID = -1;
            model.SearchModel.ControllerName = string.Empty;
            var areaList = AreaBLL.GetAreas();
            model.SearchModel.AreasSelectList = areaList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.AreaName));

            model.PagedDatas = ControllerBLL.GetPagedControllerInfo(model.SearchModel, model.SearchPage, this.PageSize);
            return PartialView("_ControllerInfo", model);
        }


        [AsiatekSubordinateFunction("ControllerInfo")]
        public ActionResult GetControllerInfo(ControllerSearchModel model, int searchPage)
        {
            return GetControllerPagedGridPV(model, searchPage);
        }



   

        private ActionResult GetControllerPagedGridPV(ControllerSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<ControllerSearchModel, ControllerListModel> result = new SearchDataWithPagedDatas<ControllerSearchModel, ControllerListModel>();
            result.SearchModel = model;
            result.SearchPage = searchPage;
            result.PagedDatas = ControllerBLL.GetPagedControllerInfo(result.SearchModel, result.SearchPage, this.PageSize);
            return PartialView("_ControllerPagedGrid", result);
        }



        #region NEW
        //public ActionResult ControllerInfo()
        //{
        //    ControllerSettingModel model = new ControllerSettingModel();
        //    var areaList = AreaBLL.GetAreas();
        //    model.AreasSelectList = areaList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.AreaName));


        //    model.SearchPage = 1;
        //    model.AreaID = -1;
        //    model.PagedDatas = ControllerBLL.GetPagedControllerInfo(model, this.PageSize);
        //    return PartialView("_ControllerInfo", model);
        //}


        //[HttpPost, ValidateAntiForgeryToken, AsiatekSubordinateFunction("ControllerInfo")]
        //public ActionResult GetControllerInfo(ControllerSettingModel model)
        //{
        //    return GetControllerPagedGridPV(model);
        //}

        //private ActionResult GetControllerPagedGridPV(ControllerSettingModel model)
        //{
        //    model.PagedDatas = ControllerBLL.GetPagedControllerInfo(model, this.PageSize);
        //    return PartialView("_ControllerPagedGrid", model);
        //}

        //[AsiatekSubordinateFunction("ControllerInfo")]
        //public ActionResult ControllerPagedGrid(ControllerSettingModel model)
        //{
        //    return GetControllerPagedGridPV(model);
        //} 
        #endregion

        #region 旧代码
        //public ActionResult ControllerInfo()
        //{
        //    ViewBag.AreaID = -1;
        //    ViewBag.ControllerName = string.Empty;

        //    var areaList = AreaBLL.GetAreas();
        //    var cList = ControllerBLL.GetPagedControllerInfo(PageSize);
        //    Asiatek.Model.ControllerSettingModel model = new Asiatek.Model.ControllerSettingModel();
        //    model.AreasSelectList = areaList.ToSelectListWithAll(m => GetSelectListItem(m.ID, m.AreaName));
        //    model.PagedControllers = cList;
        //    return PartialView("_ControllerInfo", model);
        //} 
        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult GetControllerInfo(string controllerName, int areaID)
        //{
        //    return GetControllerPagedGridPV(controllerName, areaID, 1);
        //}
        //private ActionResult GetControllerPagedGridPV(string controllerName, int areaID, int currentPage)
        //{
        //    ViewBag.ControllerName = controllerName;
        //    ViewBag.AreaID = areaID;

        //    var obj = ControllerBLL.GetPagedControllerInfo(PageSize, currentPage, areaID, controllerName);
        //    return PartialView("_ControllerPagedGrid", obj);
        //}
        //public ActionResult ControllerPagedGrid(string controllerName, int areaID = -1, int currentPage = 1)
        //{
        //    return GetControllerPagedGridPV(controllerName, areaID, currentPage);
        //}
        #endregion










        [AsiatekSubordinateFunction("ActionSetting", "Action", "Admin")]
        public ActionResult GetControllerDDLWithDefaultByAreaID(int areaID)
        {
            List<Asiatek.Model.ControllerDDLModel> list;
            if (areaID == -1)
            {
                list = ControllerBLL.GetControllers();
            }
            else
            {
                list = ControllerBLL.GetControllersByAreaID(areaID);
            }
            list.Insert(0, new Model.ControllerDDLModel() { ID = -1, ControllerName = Asiatek.Resource.UIText.All });
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [AsiatekSubordinateFunction("AddActionInfo", "Action", "Admin")]
        [AsiatekSubordinateFunction("EditActionInfo", "Action", "Admin")]
        public ActionResult GetControllerDDLByAreaID(int areaID)
        {
            var list = ControllerBLL.GetControllersByAreaID(areaID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region 删除
        #region 旧代码
        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult DeleteCtr(FormCollection fc)
        //{
        //    int currentPage = Convert.ToInt32(fc["currentPage"]);
        //    string controllerName = fc["controllerName"];
        //    int areaID = Convert.ToInt32(fc["areaID"]);
        //    string[] ids = fc["ctrid"].Split(',');

        //    var result = ControllerBLL.DeleteControllers(ids);

        //    base.DoLog(OperationTypeEnum.Delete, result, fc["ctrid"]);
        //    ViewBag.Message = result.Message;
        //    return GetControllerPagedGridPV(controllerName, areaID, currentPage);
        //} 
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteCtr(FormCollection fc)
        {
            string[] ids = fc["ctrid"].Split(',');
            var result = ControllerBLL.DeleteControllers(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["ctrid"]);
            return Json(result);
        }
        #endregion



        #region 修改

        public ActionResult EditControllerInfo(int id)
        {
            var obj = ControllerBLL.GetControllerInfoByID(id);
            if (obj == null)
            {
                return Content(obj.Message);
            }
            obj.DataResult.AreasSelectList = new SelectList(AreaBLL.GetAreas(), "ID", "AreaName");
            return PartialView("_EditControllerInfo", obj.DataResult);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditControllerInfo(ControllerEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ControllerBLL.ModifyController(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "ControllerID:" + model.ID);
                return Json(result);
            }
            else
            {
                model.AreasSelectList = new SelectList(AreaBLL.GetAreas(), "ID", "AreaName");
                return PartialView("_EditControllerInfo", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("EditControllerInfo")]
        public ActionResult CheckEditControllerNameExists(string controllerName, int areaID, int ID)
        {
            bool result = ControllerBLL.CheckControllerNameExists(controllerName, areaID, ID);
            return Json(!result);
        }
        #endregion


        #region 新增
        public ActionResult AddControllerInfo()
        {
            ControllerAddModel model = new ControllerAddModel();
            model.AreasSelectList = new SelectList(AreaBLL.GetAreas(), "ID", "AreaName");
            return PartialView("_AddControllerInfo", model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddControllerInfo(ControllerAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ControllerBLL.AddController(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "ControllerName:" + model.ControllerName);
                return Json(result);
            }
            else
            {
                model.AreasSelectList = new SelectList(AreaBLL.GetAreas(), "ID", "AreaName");
                return PartialView("_AddControllerInfo", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("AddControllerInfo")]
        public ActionResult CheckAddControllerNameExists(string controllerName, int areaID)
        {
            bool result = ControllerBLL.CheckControllerNameExists(controllerName, areaID);
            return Json(!result);
        }
        #endregion



    }
}

using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class AreaController : BaseController
    {



        #region 查询
        public ActionResult AreaInfo()
        {
            SearchDataWithPagedDatas<AreaSearchModel, AreaModel> model
                = new SearchDataWithPagedDatas<AreaSearchModel, AreaModel>();
            model.SearchModel = new AreaSearchModel();
            model.SearchModel.AreaName = string.Empty;
            model.SearchPage = 1;
            model.PagedDatas = AreaBLL.GetPagedAreaInfo(model.SearchModel, model.SearchPage, this.PageSize);
            return PartialView("_AreaInfo", model);
        }

        [AsiatekSubordinateFunction("AreaInfo")]
        public ActionResult GetAreaInfo(AreaSearchModel model, int searchPage)
        {
            return GetAreaPagedGridPV(model, searchPage);
        }


        private ActionResult GetAreaPagedGridPV(AreaSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<AreaSearchModel, AreaModel> result = new SearchDataWithPagedDatas<AreaSearchModel, AreaModel>();
            result.SearchPage = searchPage;
            result.SearchModel = model;
            result.PagedDatas = AreaBLL.GetPagedAreaInfo(result.SearchModel, result.SearchPage, this.PageSize);
            return PartialView("_AreaPagedGrid", result);
        }


        #region NEW注释
        //public ActionResult AreaInfo()
        //{
        //    AreaSettingModel model = new AreaSettingModel();
        //    model.SearchPage = 1;
        //    model.PagedDatas = AreaBLL.GetPagedAreaInfo(model, this.PageSize);
        //    return PartialView("_AreaInfo", model);
        //}

        //[AsiatekSubordinateFunction("AreaInfo")]
        //public ActionResult AreaPagedGrid(AreaSettingModel model)
        //{
        //    return GetAreaPagedGridPV(model);
        //}



        //[HttpPost, ValidateAntiForgeryToken, AsiatekSubordinateFunction("AreaInfo")]
        //public ActionResult GetAreaInfoByAreaName(AreaSettingModel model)
        //{
        //    return GetAreaPagedGridPV(model);
        //}


        //private ActionResult GetAreaPagedGridPV(AreaSettingModel model)
        //{
        //    model.PagedDatas = AreaBLL.GetPagedAreaInfo(model, this.PageSize);
        //    return PartialView("_AreaPagedGrid", model);
        //} 
        #endregion

        #region 旧代码


        //public ActionResult AreaInfo()
        //{
        //    return PartialView("_AreaInfo", AreaBLL.GetPagedAreaInfo(1, this.PageSize));
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult GetAreaInfoByAreaName(string areaName)
        //{
        //    return GetAreaPagedGridPV(areaName, 1);
        //}


        //[AsiatekSubordinateFunction("AreaInfo")]
        //public ActionResult AreaPagedGrid(string areaName, int currentPage = 1)
        //{
        //    return GetAreaPagedGridPV(areaName, currentPage);
        //}


        //private ActionResult GetAreaPagedGridPV(string areaName, int currentPage)
        //{
        //    ViewBag.AreaName = areaName;
        //    return PartialView("_AreaPagedGrid", AreaBLL.GetPagedAreaInfo(currentPage, PageSize, areaName));
        //} 
        #endregion



        #endregion

        #region 删除
        #region 旧代码
        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult DeleteArea(FormCollection fc)
        //{
        //    int currentPage = Convert.ToInt32(fc["currentPage"]);
        //    string areaName = fc["areaName"];
        //    string[] ids = fc["areaid"].Split(',');

        //    var result = AreaBLL.DeleteAreas(ids);
        //    base.DoLog(OperationTypeEnum.Delete, result, fc["areaid"]);
        //    ViewBag.Message = result.Message;
        //    return GetAreaPagedGridPV(areaName, currentPage);
        //} 
        #endregion


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteArea(FormCollection fc)
        {
            string[] ids = fc["areaid"].Split(',');
            var result = AreaBLL.DeleteAreas(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["areaid"]);
            return Json(result);
        }
        #endregion

        #region 修改
        public ActionResult EditAreaInfo(int id)
        {
            var model = AreaBLL.GetAreaByID(id);
            if (model.DataResult == null)
            {
                return Content(model.Message);
            }
            return PartialView("_EditAreaInfo", model.DataResult);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditAreaInfo(AreaEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = AreaBLL.ModifyAreaInfo(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "AreaID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditAreaInfo", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("EditAreaInfo")]
        public ActionResult CheckEditAreaNameExists(string areaName, int id)
        {
            return Json(!AreaBLL.CheckAreaNameExists(areaName, id));
        }
        #endregion


        #region 新增
        public ActionResult AddAreaInfo()
        {
            return PartialView("_AddAreaInfo");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddAreaInfo(AreaAddModel model)
        {
            if (ModelState.IsValid)
            {
                
                var result = AreaBLL.AddAreaInfo(model,base.UserIDForLog);

                base.DoLog(OperationTypeEnum.Add, result, "AreaName:" + model.AreaName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddAreaInfo", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("AddAreaInfo")]
        public ActionResult CheckAddAreaNameExists(string areaName)
        {
            return Json(!AreaBLL.CheckAreaNameExists(areaName));
        }

        #endregion



    }
}

using Asiatek.AjaxPager;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.BLL.MSSQL;
using Asiatek.TMS.Filters;
using Asiatek.Resource;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    public class MapRegionsController : BaseController
    {
        #region 查询
        public ActionResult MapRegionsSetting()
        {
            SearchDataWithPagedDatas<MapRegionsSearchModel, MapRegionsListModel> model = new SearchDataWithPagedDatas<MapRegionsSearchModel, MapRegionsListModel>();
            model.SearchModel = new MapRegionsSearchModel();

            List<SelectListItem> liRegionsType = new List<SelectListItem>();
            liRegionsType.Add(new SelectListItem { Text = "全部", Value = "0", Selected = true });
            liRegionsType.Add(new SelectListItem { Text = "圆", Value = "1" });
            liRegionsType.Add(new SelectListItem { Text = "矩形", Value = "2" });
            liRegionsType.Add(new SelectListItem { Text = "多边形", Value = "3" });
            model.SearchModel.RegionsTypeSelectList = liRegionsType;

            model.PagedDatas = MapRegionsBLL.GetPagedMapRegions(model.SearchModel, 1, this.PageSize);
            return PartialView("_MapRegionsSetting", model);
        }

        [AsiatekSubordinateFunction("MapRegionsSetting")]
        public ActionResult GetMapRegions(MapRegionsSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<MapRegionsSearchModel, MapRegionsListModel> result = new SearchDataWithPagedDatas<MapRegionsSearchModel, MapRegionsListModel>();
            result.SearchModel = model;
            result.PagedDatas = MapRegionsBLL.GetPagedMapRegions(result.SearchModel, searchPage, this.PageSize);
            return PartialView("_MapRegionsPagedGrid", result);
        }

        #endregion


        #region 新增
        public ActionResult AddMapRegions()
        {
            MapRegionsAddModel model = new MapRegionsAddModel();

            List<SelectListItem> liRegionsType = new List<SelectListItem>();
            liRegionsType.Add(new SelectListItem { Text = "圆", Value = "1", Selected = true });
            liRegionsType.Add(new SelectListItem { Text = "矩形", Value = "2" });
            liRegionsType.Add(new SelectListItem { Text = "多边形", Value = "3" });
            model.RegionsTypeSelectList = liRegionsType;

            return PartialView("_AddMapRegions", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddMapRegions(MapRegionsAddModel model)
        {
            if (model.Radius == 0.0 && model.LeftUpperLatitude == 0.0 && string.IsNullOrWhiteSpace(model.PolygonList[0]))
            {
                ModelState.AddModelError("RegionsType", DataAnnotations.NeedDrawError);
            }
            if (ModelState.IsValid)
            {
                var result = MapRegionsBLL.AddMapRegions(model, base.CurrentUserID, base.CurrentStrucID);
                base.DoLog(OperationTypeEnum.Add, result, "RegionsName:" + model.RegionsName);
                return Json(result);
            }
            else
            {
                List<SelectListItem> liRegionsType = new List<SelectListItem>();
                liRegionsType.Add(new SelectListItem { Text = "圆", Value = "1", Selected = true });
                liRegionsType.Add(new SelectListItem { Text = "矩形", Value = "2" });
                liRegionsType.Add(new SelectListItem { Text = "多边形", Value = "3" });
                model.RegionsTypeSelectList = liRegionsType;

                return PartialView("_AddMapRegions", model);
            }
        }

        /// <summary>
        /// 添加时 验证用户所属单位下是否有重复的区域名称
        /// </summary>
        /// <param name="regionsName"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("AddMapRegions")]
        public ActionResult CheckAddRegionsNameExists(string regionsName)
        {
            return Json(!MapRegionsBLL.CheckAddRegionsNameExists(regionsName, base.CurrentStrucID));
        }

        #endregion


        #region 修改
        public ActionResult EditMapRegions(int id, int regionsType)
        {
            var result = MapRegionsBLL.GetMapRegionsByID(id, regionsType);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            List<SelectListItem> liRegionsType = new List<SelectListItem>();
            liRegionsType.Add(new SelectListItem { Text = "圆", Value = "1", Selected = true });
            liRegionsType.Add(new SelectListItem { Text = "矩形", Value = "2" });
            liRegionsType.Add(new SelectListItem { Text = "多边形", Value = "3" });
            model.RegionsTypeSelectList = liRegionsType;

            return PartialView("_EditMapRegions", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditMapRegions(MapRegionsEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MapRegionsBLL.EditMapRegions(model, base.CurrentUserID, base.CurrentStrucID);
                base.DoLog(OperationTypeEnum.Add, result, "RegionsName:" + model.RegionsName);
                return Json(result);
            }
            else
            {
                List<SelectListItem> liRegionsType = new List<SelectListItem>();
                liRegionsType.Add(new SelectListItem { Text = "圆", Value = "1", Selected = true });
                liRegionsType.Add(new SelectListItem { Text = "矩形", Value = "2" });
                liRegionsType.Add(new SelectListItem { Text = "多边形", Value = "3" });
                model.RegionsTypeSelectList = liRegionsType;
                return PartialView("_EditMapRegions", model);
            }
        }

        /// <summary>
        /// 编辑时 验证用户所属单位下是否有重复的区域名称
        /// </summary>
        /// <param name="regionsName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EditMapRegions")]
        public ActionResult CheckEditRegionsNameExists(string regionsName, int id)
        {
            return Json(!MapRegionsBLL.CheckEditRegionsNameExists(regionsName, id, base.CurrentStrucID));
        }
        #endregion


        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteMapRegions(FormCollection fc)
        {
            string[] ids = fc["mapRgid"].Split(',');

            var result = MapRegionsBLL.DeleteMapRegions(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["mapRgid"]);
            return Json(result);
        }
        #endregion

        #region 下拉列表
        [AsiatekSubordinateFunction("RegionAbnormalRpt", "Home", "Reports")]
        [AsiatekSubordinateFunction("RouteAbnormalRpt", "Home", "Reports")]
        public ActionResult GetStrucMapRegions(string searchName)
        {
            var list = MapRegionsBLL.GetStrucMapRegions(base.CurrentStrucID, searchName);
            var resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.RegionsName + "[" + item.StrucName + "]", value = item.RegionsName, ID = item.RegionID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

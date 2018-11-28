using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    public class MapLinesController : BaseController
    {
        #region 查询
        public ActionResult MapLinesSetting()
        {
        
            SearchDataWithPagedDatas<MapLinesSearchModel, MapLinesListModel> model = new SearchDataWithPagedDatas<MapLinesSearchModel, MapLinesListModel>();
            model.SearchModel = new MapLinesSearchModel();

            model.PagedDatas = MapLinesBLL.GetPagedMapLines(model.SearchModel, 1, this.PageSize);
            return PartialView("_MapLinesSetting", model);
        }

        [AsiatekSubordinateFunction("MapLinesSetting")]
        public ActionResult GetMapLines(MapLinesSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<MapLinesSearchModel, MapLinesListModel> result = new SearchDataWithPagedDatas<MapLinesSearchModel, MapLinesListModel>();
            result.SearchModel = model;
            result.PagedDatas = MapLinesBLL.GetPagedMapLines(result.SearchModel, searchPage, this.PageSize);
            return PartialView("_MapLinesPagedGrid", result);
        }
        #endregion


        #region 新增
        public ActionResult AddMapLines()
        {
            MapLinesAddModel model = new MapLinesAddModel();

            return PartialView("_AddMapLines", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddMapLines(MapLinesAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MapLinesBLL.AddMapLines(model, base.CurrentUserID, base.CurrentStrucID);
                base.DoLog(OperationTypeEnum.Add, result, "LinesName:" + model.LinesName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddMapLines", model);
            }
        }

        /// <summary>
        /// 添加时 验证用户所属单位下是否有重复的路线名称
        /// </summary>
        /// <param name="linesName"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("AddMapLines")]
        public ActionResult CheckAddLinesNameExists(string linesName)
        {
            return Json(!MapLinesBLL.CheckAddLinesNameExists(linesName, base.CurrentStrucID));
        }
        #endregion



        #region 编辑
        public ActionResult EditMapLines(int id)
        {
            var result = MapLinesBLL.GetMapLinesByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditMapLines", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditMapLines(MapLinesEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MapLinesBLL.EditMapLines(model, base.CurrentUserID, base.CurrentStrucID);
                base.DoLog(OperationTypeEnum.Edit, result, "LinesName:" + model.LinesName);
                return Json(result);
            }
            else
            {
                return PartialView("_EditMapLines", model);
            }
        }

        /// <summary>
        /// 编辑时 验证用户所属单位下是否有重复的路线名称
        /// </summary>
        /// <param name="linesName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EditMapRegions")]
        public ActionResult CheckEditLinesNameExists(string linesName, int id)
        {
            return Json(!MapLinesBLL.CheckEditLinesNameExists(linesName, id, base.CurrentStrucID));
        }
        #endregion


        #region  删除
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteMapLines(FormCollection fc)
        {
            string[] ids = fc["mapLnid"].Split(',');

            var result = MapLinesBLL.DeleteMapLines(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["mapLnid"]);
            return Json(result);
        }
        #endregion


        #region 下拉列表
        [AsiatekSubordinateFunction("RegionAbnormalRpt", "Home", "Reports")]
        [AsiatekSubordinateFunction("RouteAbnormalRpt", "Home", "Reports")]
        public ActionResult GetStrucMapLines(string searchName)
        {
            var list = MapLinesBLL.GetStrucMapLines(base.CurrentStrucID, searchName);
            var resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.LinesName + "[" + item.StrucName + "]", value = item.LinesName, ID = item.LineID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}

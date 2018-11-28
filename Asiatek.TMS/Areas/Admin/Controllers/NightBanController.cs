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

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class NightBanController : BaseController
    {
        #region 查询
        public ActionResult NightBanSetting()
        {
            SearchDataWithPagedDatas<NightBanSearchModel, NightBanListModel> model =
                new SearchDataWithPagedDatas<NightBanSearchModel, NightBanListModel>();
            model.SearchModel = new NightBanSearchModel();
            model.SearchModel.IsEnabled = -1;

            model.PagedDatas = NightBanBLL.GetPagedNightBan(model.SearchModel, 1, this.PageSize);
            return PartialView("_NightBanSetting", model);
        }

        [AsiatekSubordinateFunction("NightBanSetting")]
        public ActionResult GetNightBanInfo(NightBanSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<NightBanSearchModel, NightBanListModel> result =
                          new SearchDataWithPagedDatas<NightBanSearchModel, NightBanListModel>();
            result.SearchModel = model;
            result.PagedDatas = NightBanBLL.GetPagedNightBan(model, searchPage, this.PageSize);
            return PartialView("_NightBanPagedGrid", result);
        }

        #endregion

        #region 新增
        [AsiatekSubordinateFunction("NightBanSetting")]
        public ActionResult AddNightBan()
        {
            NightBanEditModel model = new NightBanEditModel();
            model.IsEnabled = true;
            model.StartTime = "02:00:00";
            model.EndTime = "05:00:00";
            return PartialView("_AddNightBan", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("NightBanSetting")]
        public ActionResult AddNightBan(NightBanEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreateUserID = base.UserIDForLog;
                var result = NightBanBLL.AddNightBan(model);
                base.DoLog(OperationTypeEnum.Add, result, "NightBanName:" + model.NightBanName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddNightBan", model);
            }
        }

        #endregion

        #region 编辑
        [AsiatekSubordinateFunction("NightBanSetting")]
        public ActionResult EditNightBan(int id)
        {
            var result = NightBanBLL.GetNightBanByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            return PartialView("_EditNightBan", result.DataResult);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("NightBanSetting")]
        public ActionResult EditNightBan(NightBanEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserID = base.UserIDForLog;
                var result = NightBanBLL.EditNightBan(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditNightBan", model);
            }
        }
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("TerminalSetting")]
        public ActionResult DeleteNightBan(FormCollection fc)
        {
            string[] ids = fc["nbid"].Split(',');
            var result = NightBanBLL.DeleteNightBan(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["nbid"]);
            return Json(result);
        }
        #endregion

        #region 唯一性校验
        [HttpPost, AsiatekSubordinateFunction("NightBanSetting")]
        public ActionResult CheckNightBanExists(string nightBanName, int id = 0)
        {
            return Json(!NightBanBLL.CheckNightBanExists(nightBanName, id));
        }
        #endregion

    }
}

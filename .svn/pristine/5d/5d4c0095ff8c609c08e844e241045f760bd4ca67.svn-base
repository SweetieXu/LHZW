using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.TMS.Helpers;
using Asiatek.AjaxPager;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class TerminalManufacturerController : BaseController
    {

        #region 查询
        public ActionResult TerminalManufacturerSetting()
        {
            SearchDataWithPagedDatas<TerminalManufacturerSearchModel, TerminalManufacturerListModel> model = new SearchDataWithPagedDatas<TerminalManufacturerSearchModel, TerminalManufacturerListModel>();
            model.SearchModel = new TerminalManufacturerSearchModel();
            model.PagedDatas = TerminalManufacturerBLL.GetPagedTerminalManufacturers(model.SearchModel, 1, this.PageSize);
            return PartialView("_TerminalManufacturerSetting", model);
        }


        [AsiatekSubordinateFunction("TerminalManufacturerSetting")]
        public ActionResult GetTerminalManufacturerInfo(TerminalManufacturerSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<TerminalManufacturerSearchModel, TerminalManufacturerListModel> result = new SearchDataWithPagedDatas<TerminalManufacturerSearchModel, TerminalManufacturerListModel>();
            result.SearchModel = model;
            result.PagedDatas = TerminalManufacturerBLL.GetPagedTerminalManufacturers(result.SearchModel, searchPage, this.PageSize);
            return PartialView("_TerminalManufacturerPagedGrid", result);
        }

        #endregion

        #region 新增

        public ActionResult AddTerminalManufacturer()
        {
            TerminalManufacturerAddModel model = new TerminalManufacturerAddModel();
            return PartialView("_AddTerminalManufacturer", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddTerminalManufacturer(TerminalManufacturerAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = TerminalManufacturerBLL.AddTerminalManufacturer(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "TerminalManufacturerName:" + model.ManufacturerName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddTerminalManufacturer", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("AddTerminalManufacturer")]
        public ActionResult CheckAddTerminalManufacturerNameExists(string manufacturerName)
        {
            return Json(!TerminalManufacturerBLL.CheckTerminalManufacturerNameExists(manufacturerName));
        }


        [HttpPost, AsiatekSubordinateFunction("AddTerminalManufacturer")]
        public ActionResult CheckAddTerminalManufacturerCodeExists(string manufacturerCode)
        {
            return Json(!TerminalManufacturerBLL.CheckTerminalManufacturerCodeExists(manufacturerCode));
        }
        #endregion

        #region 编辑
        public ActionResult EditTerminalManufacturer(int id)
        {
            var result = TerminalManufacturerBLL.GetTerminalManufacturerByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            return PartialView("_EditTerminalManufacturer", result.DataResult);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditTerminalManufacturer(TerminalManufacturerEditModel model)
        {
            if (ModelState.IsValid)
            {
                //base.UserIDForLog为修改人的ID
                var result = TerminalManufacturerBLL.EditTerminalManufacturer(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "TerminalManufacturerName:" + model.ManufacturerName);
                return Json(result);
            }
            else
            {
                return PartialView("_EditTerminalManufacturer", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("EditTerminalManufacturer")]
        public ActionResult CheckEditTerminalManufacturerNameExists(string manufacturerName, int id)
        {
            return Json(!TerminalManufacturerBLL.CheckTerminalManufacturerNameExists(manufacturerName, id));
        }


        [HttpPost, AsiatekSubordinateFunction("EditTerminalManufacturer")]
        public ActionResult CheckEditTerminalManufacturerCodeExists(string manufacturerCode, int id)
        {
            return Json(!TerminalManufacturerBLL.CheckTerminalManufacturerCodeExists(manufacturerCode, id));
        }
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteTerminalManufacturer(FormCollection fc)
        {
            string[] ids = fc["tmid"].Split(',');

            var result = TerminalManufacturerBLL.DeleteTerminalManufacturer(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["tmid"]);
            return Json(result);
        }
        #endregion






    }
}

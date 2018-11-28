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
using Asiatek.TMS.Helpers;
using Asiatek.Resource;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class TerminalTypeController : BaseController
    {
        #region 查询
        public ActionResult TerminalTypeSetting()
        {
            SearchDataWithPagedDatas<TerminalTypeSearchModel, TerminalTypeListModel> model = new SearchDataWithPagedDatas<TerminalTypeSearchModel, TerminalTypeListModel>();
            model.SearchModel = new TerminalTypeSearchModel();
            model.SearchModel.TerminalManufacturerID = -1;
            model.SearchModel.TerminalManufacturerSelectList = TerminalManufacturerBLL.GetTerminalManufacturers().ToSelectListWithAll(m => GetSelectListItem(m.ID, m.ManufacturerName));
            model.PagedDatas = TerminalTypeBLL.GetPagedTerminalTypes(model.SearchModel, 1, this.PageSize);
            return PartialView("_TerminalTypeSetting", model);
        }


        [AsiatekSubordinateFunction("TerminalTypeSetting")]
        public ActionResult GetTerminalTypeInfo(TerminalTypeSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<TerminalTypeSearchModel, TerminalTypeListModel> result = new SearchDataWithPagedDatas<TerminalTypeSearchModel, TerminalTypeListModel>();
            result.SearchModel = model;
            result.PagedDatas = TerminalTypeBLL.GetPagedTerminalTypes(model, searchPage, this.PageSize);
            return PartialView("_TerminalTypePagedGrid", result);
        }



        [AsiatekSubordinateFunction("TerminalSetting", "Terminal", "Admin")]
        public ActionResult GetTerminalDDLWithDefaultByTMID(int tmID)
        {
            List<TerminalTypeDDLModel> list;
            if (tmID == -1)
            {
                list = TerminalTypeBLL.GetTerminalTypes();
            }
            else
            {
                list = TerminalTypeBLL.GetTerminalTypesByTMID(tmID);
            }
            list.Insert(0, new Model.TerminalTypeDDLModel() { ID = -1, TerminalName = UIText.All });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [AsiatekSubordinateFunction("AddTerminal", "Terminal", "Admin")]
        [AsiatekSubordinateFunction("EditTerminal", "Terminal", "Admin")]
        public ActionResult GetTerminalDDLByTMID(int tmID)
        {
            List<TerminalTypeDDLModel> list = TerminalTypeBLL.GetTerminalTypesByTMID(tmID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新增

        public ActionResult AddTerminalType()
        {
            TerminalTypeAddModel model = new TerminalTypeAddModel();
            model.Filter = true;//默认过滤
            model.ACCOFF_Frequency = 180;//默认3分钟
            model.ACCON_Frequency = 15;//默认15秒
            model.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
            return PartialView("_AddTerminalType", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddTerminalType(TerminalTypeAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = TerminalTypeBLL.AddTerminalType(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "TerminalName:" + model.TerminalName);
                return Json(result);
            }
            else
            {
                model.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
                return PartialView("_AddTerminalType", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("AddTerminalType")]
        public ActionResult CheckAddTerminalNameExists(string terminalName)
        {
            return Json(!TerminalTypeBLL.CheckTerminalNameExists(terminalName));
        }

        #endregion

        #region 编辑
        public ActionResult EditTerminalType(int id)
        {
            var result = TerminalTypeBLL.GetTerminalTypeByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            result.DataResult.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
            return PartialView("_EditTerminalType", result.DataResult);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditTerminalType(TerminalTypeEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = TerminalTypeBLL.EditTerminalType(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "TerminalName:" + model.TerminalName);
                return Json(result);
            }
            else
            {
                model.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
                return PartialView("_EditTerminalType", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("EditTerminalType")]
        public ActionResult CheckEditTerminalNameExists(string terminalName, int id)
        {
            return Json(!TerminalTypeBLL.CheckTerminalNameExists(terminalName, id));
        }
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteTerminalType(FormCollection fc)
        {
            string[] ids = fc["ttid"].Split(',');

            var result = TerminalTypeBLL.DeleteTerminalType(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["ttid"]);
            return Json(result);
        }
        #endregion


    }
}

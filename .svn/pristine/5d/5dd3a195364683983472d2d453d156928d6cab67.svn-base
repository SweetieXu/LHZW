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
    public class MaintenanceRecordController : BaseController
    {
        //
        // GET: /Admin/MaintenanceRecord/

        #region  查询
        public ActionResult MaintenanceRecordSetting()
        {
            SearchDataWithPagedDatas<MaintenanceRecordSearchModel, MaintenanceRecordListModel> model = new SearchDataWithPagedDatas<MaintenanceRecordSearchModel, MaintenanceRecordListModel>();
            model.SearchModel = new MaintenanceRecordSearchModel();
            model.SearchModel.SearchStrucID = -1;
            model.PagedDatas = MaintenanceRecordBLL.GetPagedMaintenanceRecord(model.SearchModel, 1, this.PageSize, CurrentStrucID);
            return PartialView("_MaintenanceRecordSetting", model);
        }

        [AsiatekSubordinateFunction("MaintenanceRecordSetting")]
        public ActionResult GetMaintenanceRecord(MaintenanceRecordSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<MaintenanceRecordSearchModel, MaintenanceRecordListModel> result = new SearchDataWithPagedDatas<MaintenanceRecordSearchModel, MaintenanceRecordListModel>();
            result.SearchModel = model;
            result.PagedDatas = MaintenanceRecordBLL.GetPagedMaintenanceRecord(result.SearchModel, searchPage, this.PageSize, CurrentStrucID);
            return PartialView("_MaintenanceRecordPagedGrid", result);
        }
        #endregion

        #region 新增
        [AsiatekSubordinateFunction("MaintenanceRecordSetting")]
        public ActionResult AddMaintenanceRecord()
        {
            AddMaintenanceRecordModel model = new AddMaintenanceRecordModel();
            return PartialView("_AddMaintenanceRecord", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("MaintenanceRecordSetting")]
        public ActionResult AddMaintenanceRecord(AddMaintenanceRecordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MaintenanceRecordBLL.AddMaintenanceRecord(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Add, result, "VehicleName:" + model.VehicleName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddMaintenanceRecord", model);
            }
        }

        /// <summary>
        /// 根据车辆代号查找当前单位及其子单位车辆
        /// </summary>
        [AsiatekSubordinateFunction("MaintenanceRecordSetting")]
        public ActionResult GetVehicleIDByVehicleName(string vehicleName)
        {
            var list = MaintenanceRecordBLL.GetVehicleIDByVehicleName(vehicleName, this.CurrentStrucID);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { value = item.VehicleName, label = item.VehicleName, ID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 编辑
        [AsiatekSubordinateFunction("MaintenanceRecordSetting")]
        public ActionResult EditMaintenanceRecord(int id)
        {
            var result = MaintenanceRecordBLL.GetMaintenanceRecordByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditMaintenanceRecord", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("MaintenanceRecordSetting")]
        public ActionResult EditMaintenanceRecord(EditMaintenanceRecordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MaintenanceRecordBLL.EditMaintenanceRecord(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "MaintenanceRecordID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditMaintenanceRecord", model);
            }
        }
        #endregion

    }
}

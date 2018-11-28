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
    public class MaintenanceScheduleController : BaseController
    {
        //
        // GET: /Admin/MaintenanceSchedule/

        #region  查询
        public ActionResult MaintenanceScheduleSetting()
        {
            SearchDataWithPagedDatas<MaintenanceScheduleSearchModel, MaintenanceScheduleListModel> model = new SearchDataWithPagedDatas<MaintenanceScheduleSearchModel, MaintenanceScheduleListModel>();
            model.SearchModel = new MaintenanceScheduleSearchModel();
            model.SearchModel.SearchStrucID = -1;
            model.PagedDatas = MaintenanceScheduleBLL.GetPagedMaintenanceSchedule(model.SearchModel, 1, this.PageSize, CurrentStrucID);
            return PartialView("_MaintenanceScheduleSetting", model);
        }

        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult GetMaintenanceSchedule(MaintenanceScheduleSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<MaintenanceScheduleSearchModel, MaintenanceScheduleListModel> result = new SearchDataWithPagedDatas<MaintenanceScheduleSearchModel, MaintenanceScheduleListModel>();
            result.SearchModel = model;
            result.PagedDatas = MaintenanceScheduleBLL.GetPagedMaintenanceSchedule(result.SearchModel, searchPage, this.PageSize, CurrentStrucID);
            return PartialView("_MaintenanceSchedulePagedGrid", result);
        }
        #endregion

        #region 新增
        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult AddMaintenanceSchedule()
        {
            AddMaintenanceScheduleModel model = new AddMaintenanceScheduleModel();
            return PartialView("_AddMaintenanceSchedule", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult AddMaintenanceSchedule(AddMaintenanceScheduleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MaintenanceScheduleBLL.AddMaintenanceSchedule(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Add, result, "ScheduleName:" + model.ScheduleName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddMaintenanceSchedule", model);
            }
        }

        /// <summary>
        /// 添加时 验证保养方案名称
        /// </summary>
        /// <param name="scheduleName"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult CheckAddScheduleNameExists(string scheduleName)
        {
            return Json(!MaintenanceScheduleBLL.CheckAddScheduleNameExists(scheduleName));
        }
        #endregion

        #region  物理删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult DeleteMaintenanceSchedule(FormCollection fc)
        {
            string[] ids = fc["pid"].Split(',');

            var result = MaintenanceScheduleBLL.DeleteMaintenanceSchedule(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["pid"]);
            return Json(result);
        }
        #endregion

        #region 编辑
        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult EditMaintenanceSchedule(int id)
        {
            var result = MaintenanceScheduleBLL.GetMaintenanceScheduleByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditMaintenanceSchedule", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult EditMaintenanceSchedule(EditMaintenanceScheduleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MaintenanceScheduleBLL.EditMaintenanceScheduleModel(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "ScheduleID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditMaintenanceSchedule", model);
            }
        }

        /// <summary>
        /// 修改时 验证保养方案
        /// </summary>
        /// <param name="scheduleName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult CheckEditScheduleNameExists(string scheduleName, int id)
        {
            return Json(!MaintenanceScheduleBLL.CheckEditScheduleNameExists(scheduleName, id, base.CurrentStrucID));
        }
        #endregion

        #region 绑车
        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult VehicleMaintenanceSetting(int id, string scheduleName)
        {
            SearchDataWithPagedDatas<MSBindVehicleSearchModel, MSBindVehicleListModel> model = new SearchDataWithPagedDatas<MSBindVehicleSearchModel, MSBindVehicleListModel>();
            model.SearchModel = new MSBindVehicleSearchModel();
            model.SearchModel.id = id;
            model.SearchModel.ScheduleName = scheduleName;
            ViewData["ScheduleID"] = id;
            ViewData["ScheduleName"] = scheduleName;
            model.PagedDatas = MaintenanceScheduleBLL.GetPagedCustomerVehicle(id, model.SearchModel, 1, this.PageSize, base.CurrentStrucID);
            return PartialView("_BindVehicleSetting", model);
        }

        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult GetVehicleMaintenanceSchedule(MSBindVehicleSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<MSBindVehicleSearchModel, MSBindVehicleListModel> result = new SearchDataWithPagedDatas<MSBindVehicleSearchModel, MSBindVehicleListModel>();
            result.SearchModel = model;
            ViewData["ScheduleID"] = model.id;
            result.PagedDatas = MaintenanceScheduleBLL.GetPagedCustomerVehicle(model.id, result.SearchModel, searchPage, this.PageSize, base.CurrentStrucID);
            return PartialView("_BindVehiclePagedGrid", result);
        }

        /// <summary>
        /// 批量绑车
        /// </summary>
        /// <param name="fc"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult AddVehicleToMaintenanceSchedule(FormCollection fc)
        {
            string[] vids = fc["vid"].Split(',');  //选中几条数据，这边就有几个值
            string[] sids = fc["sid"].Split(',');  //页面所有数据条数，值都是一样的
            string[] miles = fc["fmm"].Split(',');  //包括未选中的空值，前几条分别对应vid的fmm值，前台赋值的，其余为空
            string[] times = fc["fmt"].Split(',');  //包括未选中的空值，前几条分别对应vid的fmt值，前台赋值的，其余为空

            miles = miles.Where(s => !string.IsNullOrEmpty(s)).ToArray(); //去掉为空的值
            times = times.Where(s=>!string.IsNullOrEmpty(s)).ToArray();

            int len = vids.Length;
            string[,] datas = new string[len,4];  //将值整合成一个二维数组
            for (int i = 0; i < len; i++) {
                datas[i, 0] = vids[i];
                datas[i, 1] = sids[0];
                datas[i, 2] = miles[i];
                datas[i, 3] = times[i];
            }

            var result = MaintenanceScheduleBLL.AddVehicleToMaintenanceSchedule(datas, base.CurrentUserID);
            base.DoLog(OperationTypeEnum.Add, result, fc["vid"]);
            return Json(result);
        }
        //public ActionResult AddVehicleToMaintenanceSchedule(int scheduleID, long vehicleID, string firstMile, string firstTime)
        //{
        //    var result = MaintenanceScheduleBLL.AddVehicleToMaintenanceSchedule(scheduleID, vehicleID, firstMile, firstTime, base.CurrentUserID);
        //    return Json(result);
        //}
        #endregion

        #region 解绑
        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult GetVehicleByScheduleID(int id)
        {
            var list = MaintenanceScheduleBLL.GetVehicleByScheduleID(id);
            return PartialView("_BindVehicleList", list);
        }

        [AsiatekSubordinateFunction("MaintenanceScheduleSetting")]
        public ActionResult DelVehicleFromMaintenanceSchedule(long vehicleID, int scheduleID)
        {
            var result = MaintenanceScheduleBLL.DelVehicleFromMaintenanceSchedule(vehicleID, scheduleID);
            base.DoLog(OperationTypeEnum.Delete, result, "VehicleID:" + vehicleID + ",ScheduleID:" + scheduleID);
            return Json(result);
        }
        #endregion
    }
}

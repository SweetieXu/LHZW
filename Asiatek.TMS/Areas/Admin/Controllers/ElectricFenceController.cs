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
    public class ElectricFenceController : BaseController
    {
        //
        // GET: /Admin/ElectricFence/
        #region  查询
        public ActionResult ElectricFenceSetting()
        {
            SearchDataWithPagedDatas<ElectricFenceSearchModel, ElectricFenceListModel> model = new SearchDataWithPagedDatas<ElectricFenceSearchModel, ElectricFenceListModel>();
            model.SearchModel = new ElectricFenceSearchModel();
            //model.SearchModel.AlarmType = -1;
            model.SearchModel.FenceType = -1;
            model.SearchModel.SearchStrucID = -1;
            model.PagedDatas = ElectricFenceBLL.GetPagedElectricFences(model.SearchModel, 1, this.PageSize, base.CurrentStrucID);
            return PartialView("_ElectricFenceSetting", model);
        }

        [AsiatekSubordinateFunction("ElectricFenceSetting")]
        public ActionResult GetElectricFence(ElectricFenceSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<ElectricFenceSearchModel, ElectricFenceListModel> result = new SearchDataWithPagedDatas<ElectricFenceSearchModel, ElectricFenceListModel>();
            result.SearchModel = model;
            result.PagedDatas = ElectricFenceBLL.GetPagedElectricFences(result.SearchModel, searchPage, this.PageSize, base.CurrentStrucID);
            return PartialView("_ElectricFencePagedGrid", result);
        }

        /// <summary>
        ///  用户所属单位以及除亚士德以外的父单位和子单位
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("ElectricFenceSetting")]
        public ActionResult GetStructuresByStrucID(string structName)
        {
            var list = ElectricFenceBLL.GetStructuresByStrucID(base.CurrentStrucID, structName);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { value = item.StrucName, label = item.StrucName, ID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  新增
        public ActionResult AddElectricFence(string drawFlag = "", string drawFlagStr = "")
        {
            AddElectricFenceModel model = new AddElectricFenceModel();
            //model.StartTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            //model.EndTime = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd 23:59:59");
            //model.FenceState = true;
            model.PropertyNamesSelectList = new SelectList(ElectricFenceBLL.GetPropertyNames(base.CurrentStrucID), "PropertyID", "PropertyName");
            if (!string.IsNullOrEmpty(drawFlag) && !string.IsNullOrEmpty(drawFlagStr))
            {
                model.FenceType = int.Parse(drawFlag);
                model.FenceTypeInfo = drawFlagStr;
            }

            return PartialView("_AddElectricFence", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddElectricFence(AddElectricFenceModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ElectricFenceBLL.AddElectricFence(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Add, result, "FenceName:" + model.FenceName);
                return Json(result);
            }
            else
            {
                model.PropertyNamesSelectList = new SelectList(ElectricFenceBLL.GetPropertyNames(base.CurrentStrucID), "PropertyID", "PropertyName");
                return PartialView("_AddElectricFence", model);
            }
        }

        /// <summary>
        /// 电子围栏名称验证：前用户所属单位的非亚士德上级以及所有其子单位中不允许重复，亚士德时则验证所有
        /// 名称相关验证改为验证所有，因为用上述权限验证时，会存在名称重复无法通过修改验证的问题
        /// </summary>
        /// <param name="fenceName"></param>
        /// <returns></returns>
        [HttpPost]
        [AsiatekSubordinateFunction("ElectricFenceSetting")]
        public ActionResult CheckAddFenceNameExists(string fenceName)
        {
            return Json(!ElectricFenceBLL.CheckAddFenceNameExists(fenceName, base.CurrentStrucID));
        }

        //验证所有
        [HttpPost]
        [AsiatekSubordinateFunction("ElectricFenceSetting")]
        public ActionResult CheckAddFenceCodeExists(string fenceCode)
        {
            return Json(!ElectricFenceBLL.CheckAddFenceCodeExists(fenceCode));
        }

        //南钢嘉华
        public ActionResult NGJH_AddElectricFence(string drawFlag = "", string drawFlagStr = "")
        {
            NGJH_AddElectricFenceModel model = new NGJH_AddElectricFenceModel();

            if (!string.IsNullOrEmpty(drawFlag) && !string.IsNullOrEmpty(drawFlagStr))
            {
                model.FenceType = int.Parse(drawFlag);
                model.FenceTypeInfo = drawFlagStr;
            }

            return PartialView("NGJH_AddElectricFence", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NGJH_AddElectricFence(NGJH_AddElectricFenceModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ElectricFenceBLL.NGJH_AddElectricFence(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Add, result, "NGJH_FenceName:" + model.FenceName);
                return Json(result);
            }
            else
            {
                return PartialView("NGJH_AddElectricFence", model);
            }
        }

        #endregion


        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ElectricFenceSetting")]
        public ActionResult DeleteElectricFence(FormCollection fc)
        {
            string[] ids = fc["Fnid"].Split(',');

            var result = ElectricFenceBLL.DeleteElectricFence(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["Fnid"]);
            return Json(result);
        }
        #endregion


        #region  修改
        public ActionResult EditElectricFence(int id)
        {
            var result = ElectricFenceBLL.GetElectricFenceByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            model.PropertyNamesSelectList = new SelectList(ElectricFenceBLL.GetPropertyNames(base.CurrentStrucID), "PropertyID", "PropertyName");
            return PartialView("_EditElectricFence", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditElectricFence(EditElectricFenceModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ElectricFenceBLL.EditElectricFence(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "FenceID:" + model.ID);
                return Json(result);
            }
            else
            {
                model.PropertyNamesSelectList = new SelectList(ElectricFenceBLL.GetPropertyNames(base.CurrentStrucID), "PropertyID", "PropertyName");
                return PartialView("_EditElectricFence", model);
            }
        }

        /// <summary>
        /// 电子围栏名称验证：验证所有
        /// </summary>
        [HttpPost]
        [AsiatekSubordinateFunction("ElectricFenceSetting")]
        public ActionResult CheckEditFenceNameExists(string fenceName, int id)
        {
            return Json(!ElectricFenceBLL.CheckEditFenceNameExists(fenceName, id, base.CurrentStrucID));
        }

        [HttpPost]
        [AsiatekSubordinateFunction("ElectricFenceSetting")]
        public ActionResult CheckEditFenceCodeExists(string fenceCode, int id)
        {
            return Json(!ElectricFenceBLL.CheckEditFenceCodeExists(fenceCode, id));
        }

        //南钢嘉华
        public ActionResult NGJH_EditElectricFence(int id)
        {
            var result = ElectricFenceBLL.GetNGJH_ElectricFenceByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("NGJH_EditElectricFence", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NGJH_EditElectricFence(NGJH_EditElectricFenceModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ElectricFenceBLL.NGJH_EditElectricFence(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "NGJH_FenceID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("NGJH_EditElectricFence", model);
            }
        }
        #endregion


        //电子围栏绑定车辆
        #region  绑定车辆
        /// <summary>
        /// 加载车辆，带分页，附带电子围栏信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fenceName"></param>
        /// <returns></returns>
        public ActionResult VehicleElectricFenceSetting(int id, string fenceName)
        {
            SearchDataWithPagedDatas<EFVehicleSearchModel, EFVehicleListModel> model = new SearchDataWithPagedDatas<EFVehicleSearchModel, EFVehicleListModel>();
            model.SearchModel = new EFVehicleSearchModel();
            model.SearchModel.id = id;
            model.SearchModel.FenceName = fenceName;
            ViewData["fenceID"] = id;
            ViewData["FenceName"] = fenceName;
            model.PagedDatas = ElectricFenceBLL.GetPagedCustomerVehicle(id, model.SearchModel, 1, this.PageSize, base.CurrentStrucID);
            return PartialView("_ElectricFenceVehicleSetting", model);
        }

        [AsiatekSubordinateFunction("VehicleElectricFenceSetting")]
        public ActionResult GetVehicleElectricFence(EFVehicleSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<EFVehicleSearchModel, EFVehicleListModel> result = new SearchDataWithPagedDatas<EFVehicleSearchModel, EFVehicleListModel>();
            result.SearchModel = model;
            ViewData["fenceID"] = model.id;
            result.PagedDatas = ElectricFenceBLL.GetPagedCustomerVehicle(model.id, result.SearchModel, searchPage, this.PageSize, base.CurrentStrucID);
            return PartialView("_ElectricFenceVehiclePagedGrid", result);
        }

        /// <summary>
        /// 电子围栏绑定车辆
        /// </summary>
        /// <param name="fenceID"></param>
        /// <param name="vehicleID"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("VehicleElectricFenceSetting")]
        public ActionResult AddVehicleToElectricFence(int fenceID, long vehicleID)
        {
            var result = ElectricFenceBLL.AddVehicleToElectricFence(fenceID, vehicleID, base.CurrentUserID);
            base.DoLog(OperationTypeEnum.Add, result, "FenceID:" + fenceID + ",VehicleID" + vehicleID);
            return Json(result);
        }

        #endregion

        #region  解绑终端
        /// <summary>
        /// 获取电子围栏下绑定的所有终端信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetVehicleByFenceID(int id)
        {
            var list = ElectricFenceBLL.GetVehicleByFenceID(id);
            return PartialView("_ElectricFenceVehicleBindList", list);
        }

        /// <summary>
        /// 解绑指定终端
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="fenceID"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("GetVehicleByFenceID")]
        public ActionResult DelVehicleFromElectricFence(long vehicleID, int fenceID)
        {
            var result = ElectricFenceBLL.DelVehicleFromElectricFence(vehicleID, fenceID);
            base.DoLog(OperationTypeEnum.Delete, result, "FenceID:" + fenceID + ",VehicleID" + vehicleID);
            return Json(result);
        }

        #endregion

    }
}

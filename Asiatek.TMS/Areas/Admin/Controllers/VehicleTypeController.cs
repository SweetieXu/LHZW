using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class VehicleTypeController : BaseController
    {
        #region   查询车辆类型
        public ActionResult SelectVehicleType()
        {
            SearchDataWithPagedDatas<VehicleTypeSearchModel, VehicleTypeListModel> model =
                new SearchDataWithPagedDatas<VehicleTypeSearchModel, VehicleTypeListModel>();

            model.SearchModel = new VehicleTypeSearchModel();
            model.PagedDatas = VehicleMaintainBLL.GetPagedVehicleType(model.SearchModel, 1, this.PageSize);
            return PartialView("_SelectVehicleType", model);
        }

        [AsiatekSubordinateFunction("SelectVehicleType")]
        public ActionResult GetVehicleTypeInfo(VehicleTypeSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<VehicleTypeSearchModel, VehicleTypeListModel> result =
                new SearchDataWithPagedDatas<VehicleTypeSearchModel, VehicleTypeListModel>();
            result.SearchModel = model;
            result.PagedDatas = VehicleMaintainBLL.GetPagedVehicleType(model, searchPage, this.PageSize);
            return PartialView("_VehicleTypesGrid", result);
        }
        #endregion

        #region   新增车辆类型
         [AsiatekSubordinateFunction("SelectVehicleType")]
        public ActionResult AddVehicleTypes()
        {
            return PartialView("_AddVehicleTypeGrid");
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SelectVehicleType")]
        public ActionResult AddVehicleTypes(VehicleTypeAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = VehicleMaintainBLL.AddVehicleType(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "VehicleTCode:" + model.Code);
                return Json(result);
            }
            else
            {
                return PartialView("_AddVehicleTypeGrid", model);
            }
        }

        #region   验证是否重复
        /// <summary>
        /// 编号
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("SelectVehicleType")]
        public ActionResult CheckAddTypeCodeExists(string Code)
        {
            return Json(!VehicleMaintainBLL.CheckTypeCodeExists(Code));
        }

        /// <summary>
        /// 名称
        /// </summary>
        /// <param name="Colors"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("SelectVehicleType")]
        public ActionResult CheckAddTypeNameExists(string Name)
        {
            return Json(!VehicleMaintainBLL.CheckTypeNameExists(Name));
        }
        #endregion

        //联想数据
        [AsiatekSubordinateFunction("SelectVehicleType")]
        public ActionResult GetParentCode(int vehicleName)
        {
            var list = VehicleMaintainBLL.GetParentCode(vehicleName);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = "[" + item.Code + "]", value = item.Code, VID = item.Code });
                //resultList.Add(new { label = item.Code + "[" + item.Name + "]", value = item.Name, VID = item.Code });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region   编辑车辆类型
          [AsiatekSubordinateFunction("SelectVehicleType")]
        public ActionResult EditVehicleTypes(int id)
        {
            var result = VehicleMaintainBLL.GetVehicleTypeID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditVehicleTypes", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SelectVehicleType")]
        public ActionResult EditVehicleTypes(VehicleTypeEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = VehicleMaintainBLL.EditVehicleType(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "Code:" + model.Code);
                return Json(result);
            }
            else
            {
                return PartialView("_EditVehicleTypes", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("SelectVehicleType")]
        public ActionResult CheckEditTypeNameExists(string name, long id)
        {
            return Json(!VehicleMaintainBLL.CheckCodeNameExists(name, id));
        }
        #endregion
    }
}

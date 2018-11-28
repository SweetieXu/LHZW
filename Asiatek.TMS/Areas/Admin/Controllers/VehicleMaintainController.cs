using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class VehicleMaintainController : BaseController
    {
        #region   查询车辆颜色
        public ActionResult SelectPlateColor()
        {
            SearchDataWithPagedDatas<VehicleMaintainSearchModels, VehicleMaintainListModels> model =
                new SearchDataWithPagedDatas<VehicleMaintainSearchModels, VehicleMaintainListModels>();

            model.SearchModel = new VehicleMaintainSearchModels();
            model.PagedDatas = VehicleMaintainBLL.GetPagedPlateColors(model.SearchModel, 1, this.PageSize);
            return PartialView("_SelectPlateColors", model);
        }

        [AsiatekSubordinateFunction("SelectPlateColor")]
        public ActionResult GetPlateColorsInfo(VehicleMaintainSearchModels model, int searchPage)
        {
            SearchDataWithPagedDatas<VehicleMaintainSearchModels, VehicleMaintainListModels> result =
                new SearchDataWithPagedDatas<VehicleMaintainSearchModels, VehicleMaintainListModels>();
            result.SearchModel = model;
            result.PagedDatas = VehicleMaintainBLL.GetPagedPlateColors(model, searchPage, this.PageSize);
            return PartialView("_PlateColorGrid", result);
        }
        #endregion

        #region   新增车辆颜色
         [AsiatekSubordinateFunction("SelectPlateColor")]
        public ActionResult AddPlateColors()
        {
            return PartialView("_AddPlateColorGrid");
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SelectPlateColor")]
        public ActionResult AddPlateColors(VehicleMaintainAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = VehicleMaintainBLL.AddPlateColors(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "PlateColors:" + model.PlateCode);
                return Json(result);
            }
            else
            {
                return PartialView("_AddPlateColorGrid", model);
            }
        }
        #endregion

        #region   验证是否重复
        /// <summary>
        /// 编号
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("SelectPlateColor")]
        public ActionResult CheckAddPlateCodeExists(string PlateCode)
        {
            return Json(!VehicleMaintainBLL.CheckPlateCodeExists(PlateCode));
        }

        /// <summary>
        /// 颜色
        /// </summary>
        /// <param name="Colors"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("SelectPlateColor")]
        public ActionResult CheckAddPlateColorsExists(string PlateColor)
        {
            return Json(!VehicleMaintainBLL.CheckPlateColorsExists(PlateColor));
        }
        #endregion
    }
}

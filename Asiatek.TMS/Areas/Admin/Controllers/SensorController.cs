using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class SensorController : BaseController
    {
        // #region   查询
        // public ActionResult SeachSensorType()
        // {
        //     SearchDataWithPagedDatas<SensorSearchModel, SensorListModels> model =
        //         new SearchDataWithPagedDatas<SensorSearchModel, SensorListModels>();

        //     model.SearchModel = new SensorSearchModel();
        //     model.PagedDatas = SensorBLL.GetPagedSensor(model.SearchModel, 1, this.PageSize);
        //     return PartialView("_SeachSensorType", model);
        // }

        // [AsiatekSubordinateFunction("SeachSensorType")]
        // public ActionResult GetSensorInfo(SensorSearchModel model, int searchPage)
        // {
        //     SearchDataWithPagedDatas<SensorSearchModel, SensorListModels> result =
        //new SearchDataWithPagedDatas<SensorSearchModel, SensorListModels>();
        //     result.SearchModel = model;
        //     result.PagedDatas = SensorBLL.GetPagedSensor(model, searchPage, this.PageSize);
        //     return PartialView("_SensorPagedGrid", result);
        // }
        // #endregion

        // #region   新增
        // [AsiatekSubordinateFunction("SeachSensorType")]
        // public ActionResult AddSensorTypes()
        // {
        //     return PartialView("_AddSensorType");
        // }

        // [HttpPost, ValidateAntiForgeryToken]
        // [AsiatekSubordinateFunction("SeachSensorType")]
        // public ActionResult AddSensorTypes(SensorAddModel model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var result = SensorBLL.AddSensorType(model,base.UserIDForLog);
        //         base.DoLog(OperationTypeEnum.Add, result, "SensorCode:" + model.SensorCode);
        //         return Json(result);
        //     }
        //     else
        //     {
        //         return PartialView("_AddSensorType", model);
        //     }
        // }

        // [HttpPost, AsiatekSubordinateFunction("SeachSensorType")]
        // public ActionResult CheckAddTypeNameExists(string SensorName)
        // {
        //     return Json(!SensorBLL.CheckTypeNameExists(SensorName));
        // }

        // [HttpPost, AsiatekSubordinateFunction("SeachSensorType")]
        // public ActionResult CheckAddSensorCodeExists(string sensorCode)
        // {
        //     return Json(!SensorBLL.CheckAddSensorCodeExists(sensorCode));
        // }
        // #endregion

        // #region   编辑
        //  [AsiatekSubordinateFunction("SeachSensorType")]
        // public ActionResult EditSensorTypes(int id)
        // {
        //     var result = SensorBLL.GetSensorTypeID(id);
        //     if (result.DataResult == null)
        //     {
        //         return Content(result.Message);
        //     }
        //     var model = result.DataResult;
        //     return PartialView("_EditSensorType", model);
        // }

        // [HttpPost, ValidateAntiForgeryToken]
        // [AsiatekSubordinateFunction("SeachSensorType")]
        // public ActionResult EditSensorTypes(SensorEditModel model)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var result = SensorBLL.EditSensorType(model,base.UserIDForLog);
        //         base.DoLog(OperationTypeEnum.Edit, result, "TypeID:" + model.TypeID);
        //         return Json(result);
        //     }
        //     else
        //     {
        //         return PartialView("_EditSensorType", model);
        //     }
        // }

        // [HttpPost, AsiatekSubordinateFunction("SeachSensorType")]
        // public ActionResult CheckEditTypeNameExists(string SensorName)
        // {
        //     return Json(!SensorBLL.CheckTypeNameExists(SensorName));
        // }
        // #endregion

        // #region   删除
        // [HttpPost, ValidateAntiForgeryToken]
        // [AsiatekSubordinateFunction("SeachSensorType")]
        // public ActionResult DeleteSensor(FormCollection fc)
        // {
        //     string[] ids = fc["tid"].Split(',');

        //     var result = SensorBLL.DeleteSensor(ids);

        //     base.DoLog(OperationTypeEnum.Delete, result, fc["tid"]);
        //     return Json(result);
        // }
        // #endregion




        #region   查询
        public ActionResult SeachSensorType()
        {
            SearchDataWithPagedDatas<SensorSearchModel, SensorListModels> model =
                new SearchDataWithPagedDatas<SensorSearchModel, SensorListModels>();

            model.SearchModel = new SensorSearchModel();
            model.PagedDatas = SensorBLL.GetPagedSensor(model.SearchModel, 1, this.PageSize);
            return PartialView("_SeachSensorType", model);
        }

        [AsiatekSubordinateFunction("SeachSensorType")]
        public ActionResult GetSensorInfo(SensorSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<SensorSearchModel, SensorListModels> result =
       new SearchDataWithPagedDatas<SensorSearchModel, SensorListModels>();
            result.SearchModel = model;
            result.PagedDatas = SensorBLL.GetPagedSensor(model, searchPage, this.PageSize);
            return PartialView("_SensorPagedGrid", result);
        }
        #endregion

        #region   新增
        [AsiatekSubordinateFunction("SeachSensorType")]
        public ActionResult AddSensorTypes()
        {
            return PartialView("_AddSensorType");
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachSensorType")]
        public ActionResult AddSensorTypes(SensorAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = SensorBLL.AddSensorType(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "SensorCode:" + model.SensorCode);
                return Json(result);
            }
            else
            {
                return PartialView("_AddSensorType", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("SeachSensorType")]
        public ActionResult CheckAddTypeNameExists(string SensorName)
        {
            return Json(!SensorBLL.CheckTypeNameExists(SensorName));
        }

        [HttpPost, AsiatekSubordinateFunction("SeachSensorType")]
        public ActionResult CheckAddSensorCodeExists(string sensorCode)
        {
            return Json(!SensorBLL.CheckAddSensorCodeExists(sensorCode));
        }
        #endregion

        #region   编辑
        [AsiatekSubordinateFunction("SeachSensorType")]
        public ActionResult EditSensorTypes(int id)
        {
            var result = SensorBLL.GetSensorTypeID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditSensorType", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachSensorType")]
        public ActionResult EditSensorTypes(SensorEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = SensorBLL.EditSensorType(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "SensorName:" + model.SensorName);
                return Json(result);
            }
            else
            {
                return PartialView("_EditSensorType", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("SeachSensorType")]
        public ActionResult CheckEditTypeNameExists(string SensorName)
        {
            return Json(!SensorBLL.CheckTypeNameExists(SensorName));
        }
        #endregion

        #region   删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachSensorType")]
        public ActionResult DeleteSensor(FormCollection fc)
        {
            string[] ids = fc["tid"].Split(',');

            var result = SensorBLL.DeleteSensor(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["tid"]);
            return Json(result);
        }
        #endregion
    }
}

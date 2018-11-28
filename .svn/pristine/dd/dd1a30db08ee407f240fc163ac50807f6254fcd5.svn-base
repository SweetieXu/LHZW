using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class VehicleSensorController : BaseController
    {
        #region 查询
        [AsiatekSubordinateFunction("NewEditVehicle", "Vehicle", "Admin")]
        public ActionResult GetVehicleSensor(long id, string plateNum)
        {
            VehicleSensorInfoParaModel model = new VehicleSensorInfoParaModel();
            model.ID = id;
            model.PlateNum = plateNum;
            return PartialView("VehicleSensor", model);
        }

        [AsiatekSubordinateFunction("NewEditVehicle", "Vehicle", "Admin")]
        public ActionResult GetVehicleSensorInfo(long vehicleID)
        {
            return Json(VehicleSensorBLL.GetVehicleSensors(vehicleID), JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 编辑
        [AsiatekSubordinateFunction("NewEditVehicle", "Vehicle", "Admin")]
        public ActionResult EditVehicleSensor(VehicleSensorEditModel model)
        {
           return Json(VehicleSensorBLL.EditVehicleSensors(model), JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 删除
        [AsiatekSubordinateFunction("NewEditVehicle", "Vehicle", "Admin")]
        public ActionResult DeleteVehicleSensor(long vehicleID, int TypeID)
        {
            var result = VehicleSensorBLL.DeleteVehicleSensor(vehicleID, TypeID);
            base.DoLog(OperationTypeEnum.Delete, result, "VehicleID:" + vehicleID + "TypeID:" + TypeID);
            return Json(result);
        }
        #endregion
    }
}

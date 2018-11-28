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
    public class VehicleExceptionController : BaseController
    {
        //
        // GET: /Admin/VehicleException/

        #region 查询
        public ActionResult GetVehicleException(string id, string plateNum)
        {
            VehicleExceptionInfoParaModel model = new VehicleExceptionInfoParaModel();
            model.ID = int.Parse(id);
            model.PlateNum = plateNum;
            return PartialView("VehicleException", model);
        }

        public ActionResult GetVehicleExceptionInfo()
        {
            string id = Request.Params["vehicleID"];
            return Json(VehicleExceptionBLL.GetVehicleExceptions(id), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 编辑
        public ActionResult EditVehicleException(string checkedExIDs, string vehicleID)
        {
            return Json(VehicleExceptionBLL.EditVehicleException(checkedExIDs,vehicleID), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

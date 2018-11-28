using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Controllers
{
    public class HomeElectricFenceController : BaseController
    {
        //
        // GET: /HomeElectricFence/

        public ActionResult Index()
        {
            ViewBag.Title = base.GetCurrentViewName();
            return View();
        }


        [AsiatekSubordinateFunction("Index")]
        public ActionResult GetElectricFenceByVehicleID(long vehicleID)
        {
            var list = ElectricFenceBLL.GetElectricFenceByVehicleID(vehicleID);
            if (list == null)
            {
                list = new List<ElectricFenceListModel>();
            }
            foreach (var item in list) 
            {
                item.FenceTypeInfo = MGJH_TransportPointBLL.ChangeCoordinateSystem(item.FenceType, item.FenceTypeInfo, 2);  //将取出的车机坐标转成地图坐标显示
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [AsiatekSubordinateFunction("Index")]
        public ActionResult GetElectricFenceExInfo(long vehicleID, int fenceID)
        {
            List<ElectricFenceExceptionModel> list;
            if (base.VehicleViewMode)
            {
                list = ElectricFenceBLL.GetDefaultElectricFenceExInfo(vehicleID, fenceID, base.CurrentStrucID);
            }
            else
            {
                list = ElectricFenceBLL.GetElectricFenceExInfo(vehicleID, fenceID, base.CurrentUserID);
            }
            if (list == null)
            {
                list = new List<ElectricFenceExceptionModel>();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }



    }
}

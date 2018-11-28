using Asiatek.TMS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.TMS.Filters;
using Asiatek.Common;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using System.Threading;


namespace Asiatek.TMS.Areas.ReportManage.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 功能权限
        /// </summary>
        public ActionResult Index()
        {
            ViewBag.Title = GetCurrentViewName();
            return View(GetCurrentSubFunctionsInfo());
        }

        [AsiatekSubordinateFunction("Index")]
        public ActionResult GetUserVehiclesByVehicleName(string vehicleName)
        {
            var list = VehicleBLL.GetVehiclesAndStrucName(base.CurrentUserID, vehicleName);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.VehicleName + "[" + item.StrucName + "]", value = item.VehicleName, VID = item.VID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
    }
}

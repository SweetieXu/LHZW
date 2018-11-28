using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.BLL;
using Asiatek.BLL.MSSQL;
using Asiatek.TMS.Filters;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    /// <summary>
    /// 单位与车辆分配
    /// </summary>
    public class StrucVehicleDistributionController : Controller
    {
        /// <summary>
        /// 单位与车辆分配设定
        /// </summary>
        /// <returns></returns>
        public ActionResult DistributionSetting()
        {
            return PartialView("_DistributionSetting");
        }

        /// <summary>
        /// 分配车辆
        /// </summary>
        [AsiatekSubordinateFunction("DistributionSetting")]
        public ActionResult AllotVehicles()
        {
            return PartialView("_AllotVehicles");
        }

        /// <summary>
        /// 分配单位与车辆到用户
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("DistributionSetting")]
        public ActionResult AllotStrucAndVehicle(string sids, string vids, int uid)
        {
            var temp = sids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            int[] strucIDs = new int[temp.Length];
            for (int i = 0; i < strucIDs.Length; i++)
            {
                strucIDs[i] = Convert.ToInt32(temp[i]);
            }

            temp = vids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            int[] vehicleIDs = new int[temp.Length];
            for (int i = 0; i < vehicleIDs.Length; i++)
            {
                vehicleIDs[i] = Convert.ToInt32(temp[i]);
            }
            return Json(StrucVehicleDistributionBLL.AllotStrucAndVehicleToUser(vehicleIDs, strucIDs, uid));
        }



    }
}

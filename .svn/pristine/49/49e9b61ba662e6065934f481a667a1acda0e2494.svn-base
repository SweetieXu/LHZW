using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Filters;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Asiatek.TMS.Controllers
{
    /// <summary>
    /// 包含公共方法的控制器
    /// </summary>
    public class CommonController : BaseController
    {
        #region 获取用户分配的车辆信息（车代号、车辆使用单位名）
        /// <summary>
        /// 获取用户分配的车辆信息（车代号、车辆使用单位名）
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <returns></returns>
        //实时监控
        [AsiatekSubordinateFunction("Index", "Home")]
        //历史轨迹
        [AsiatekSubordinateFunction("Index", "Home", "HistoricalRoute")]
        // 报表
        [AsiatekSubordinateFunction("Index", "Home", "Reports")]
        // 电子围栏
        [AsiatekSubordinateFunction("Index", "HomeElectricFence")]
        public ActionResult GetUserVehiclesByVehicleName(string vehicleName)
        {
            List<UserVehicles> list = new List<UserVehicles>();
            // 默认模式
            if (base.VehicleViewMode)
            {
                list = VehicleBLL.GetDefaultVehiclesAndStrucName(base.CurrentStrucID, vehicleName);
            }
            else
            {
                list = VehicleBLL.GetVehiclesAndStrucName(base.CurrentUserID, vehicleName);
            }
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new
                {
                    label = item.VehicleName + "[" + item.StrucName + "]",
                    value = item.VehicleName,
                    VID = item.VID,
                    strucName = item.StrucName,
                    VIN = item.VIN
                });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 获取用户分配的车辆信息（车牌号、车辆使用单位名）
        /// <summary>
        /// 获取用户分配的车辆信息（车牌号、车辆使用单位名）
        /// </summary>
        /// <param name="plateNum"></param>
        /// <returns></returns>
        // 报表
        [AsiatekSubordinateFunction("Index", "Home", "Reports")]
        public ActionResult GetUserVehiclesByPlateNum(string plateNum)
        {
            List<UserVehiclesModel> list = new List<UserVehiclesModel>();
            // 默认模式
            if (base.VehicleViewMode)
            {
                list = VehicleBLL.GetDefaultVehiclesAndStrucNameByPlateNum(base.CurrentStrucID, plateNum);
            }
            else
            {
                list = VehicleBLL.GetVehiclesAndStrucNameByPlateNum(base.CurrentUserID, plateNum);
            }
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new
                {
                    label = item.PlateNum + "[" + item.StrucName + "]",
                    value = item.PlateNum,
                    VID = item.VID,
                    strucName = item.StrucName,
                    VIN = item.VIN
                });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 获取用户所属单位以及子单位的下拉列表数据
        [AsiatekSubordinateFunction("TemperatureAlarmRuleSetting", "TemperatureAlarmRule", "Admin")]//1.温度报警规则设置
        [AsiatekSubordinateFunction("TemperatureAlarmRuleDistribution", "TemperatureAlarmRule", "Admin")]//2.温度报警规则分配
        [AsiatekSubordinateFunction("EmployeeInfoSetting", "EmployeeInfo", "Admin")]//3.员工信息设定
        [AsiatekSubordinateFunction("MaintenanceScheduleSetting", "MaintenanceSchedule", "Admin")]//4.保养方案设定
        public ActionResult GetStructuresByName(string name)
        {
            var list = StructureBLL.GetUsersStructuresByName(name, base.CurrentStrucID);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.StrucName, value = item.StrucName, ID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}

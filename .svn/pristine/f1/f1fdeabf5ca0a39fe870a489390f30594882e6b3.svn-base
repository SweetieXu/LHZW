using Asiatek.BLL.MSSQL;
using Asiatek.Model.TerminalSetting;
using Asiatek.TMS.TerminalOperation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    partial class TerminalSetupController
    {
        /// <summary>
        /// GET: /TerminalSetting/TerminalSetup/TerminalFlags/
        /// 下发终端参数设置页面
        /// </summary>
        public ActionResult TerminalFlags()
        {
            ViewBag.SubView = "TerminalFlags";
            ViewBag.SubViewData = null;
            ViewBag.TreeViewShowCheckBox = true;
            return PartialView("Main");
        }

        /// <summary>
        /// POST: /TerminalSetting/TerminalSetup/UpdateTerminalSetting/
        /// 下发终端设置
        /// </summary>
        /// <param name="Vehicles">PlateNum#TerminalCode数组</param>
        /// <param name="date">终端参数</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTerminalSetting(
            [ModelBinder(typeof(BasicArrayBinder<string>))] string[] Vehicles,
            [ModelBinder(typeof(ObjectBinder<TerminalSettingsModel>))] TerminalSettingsModel Settings)
        {
            int userid = GetUserSession().UserId;

            var client = new TerminalOperationClient();
            var objResult = new List<dynamic>();// 记录每次下发设置的返回结果
            foreach (var v in Vehicles)
            {
                dynamic result = new ExpandoObject();

                // Vehicles当中的记录为 PlateNum#TerminalCode
                string[] PlateNumAndTerminalCode = v.Split('#');
                string PlateNum = PlateNumAndTerminalCode[0];
                string TerminalCode = PlateNumAndTerminalCode[1];

                var response = client.SetTerminalParas(TerminalCode, new TerminalParasSettingData()
                {
                    ParaItems = new TerminalParasData()
                    {
                        终端心跳发送间隔 = Settings.HeartbeatInterval,
                        位置汇报策略 = (LocationReportingStrategy)Settings.LocationReportPolicy,
                        位置汇报方案 = (LocationReportingProgram)Settings.LocationReportTrigger,
                        驾驶员未登录汇报时间间隔 = Settings.StateRepotrInterval_NotLoggedIn,
                        休眠时汇报时间间隔 = Settings.StateRepotrInterval_Sleep,
                        紧急报警时汇报时间间隔 = Settings.StateRepotrInterval_Urgent,
                        缺省时间汇报间隔 = Settings.StateRepotrInterval_General,
                        最高速度 = Settings.MaximumSpeed,
                        超速持续时间 = Settings.OverspeedCheckTime,
                        超速报警预警差值 = Settings.DifferenceOfOverspeedValues,
                        连续驾驶时间门限 = Settings.MaximumDrivingTime,
                        当天累计驾驶时间门限 = Settings.MaximumDrivingTimeToday,
                        疲劳驾驶预警差值 = Settings.DifferenceOfFatigueDrivingTime,
                        最小休息时间 = Settings.MinimumBreakTime,
                        最长停车时间 = Settings.MaximumStoppingTime,
                        车辆里程表读数 = Settings.TotalODO,
                        监听电话号码 = new string[] { Settings.TelphoneNO }
                    }
                });

                // 记录日志
                TerminalSettingsBLL.InsertTerminalOperationsLog(
                    userid, TerminalSettingType.TerminalSetup_Update, PlateNum,
                    TerminalCode, response.State, JsonConvert.SerializeObject(Settings),
                    null, GetRemoteAddress()
                );

                //保存下发结果以便后面转换为json返回给页面显示
                result.state = response.State;
                result.message = response.Message;
                objResult.Add(result);
            }
            return Content(JsonConvert.SerializeObject(objResult));
        }
    }
}

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
        /// GET: /TerminalSetting/TerminalSetup/TerminalFlagsStatic/
        /// 终端参数设置查看页面
        /// </summary>
        public ActionResult TerminalFlagsViewer()
        {
            ViewBag.SubView = "TerminalFlagsViewer";
            ViewBag.SubViewData = null;
            ViewBag.TreeViewShowCheckBox = false;
            return PartialView("Main");
        }

        /// <summary>
        /// POST: /TerminalSetting/TerminalSetup/QueryTerminalFlags/
        /// 查询中断参数设置的值
        /// </summary>
        /// <param name="Vehicle">PlateNum#TerminalCode</param>
        /// <param name="Settings">指定查询的设置ID</param>
        [HttpPost]
        public ActionResult QueryTerminalFlags(
            string Vehicle,
            int[] Settings
        )
        {
            int userid = GetUserSession().UserId;

            var client = new TerminalOperationClient();
            // Vehicle当中的记录为 PlateNum#TerminalCode
            string[] PlateNumAndTerminalCode = Vehicle.Split('#');
            string PlateNum = PlateNumAndTerminalCode[0];
            string TerminalCode = PlateNumAndTerminalCode[1];

            TerminalParaID[] ParamIDs = new TerminalParaID[Settings.Length];
            for (int i = 0; i != Settings.Length; i++)
                ParamIDs[i] = (TerminalParaID)Settings[i];

            var response = client.QuerySpecifiedTerminalParas(TerminalCode, new QuerySpecifiedTerminalParasData()
                {
                    TerminalParaIDs = ParamIDs
                });

            var result = new TerminalSettingsModel()
            {
                HeartbeatInterval = response.ResultData.ParaItems.终端心跳发送间隔,
                LocationReportPolicy = (int)response.ResultData.ParaItems.位置汇报策略,
                LocationReportTrigger = (int)response.ResultData.ParaItems.位置汇报方案,
                StateRepotrInterval_NotLoggedIn = response.ResultData.ParaItems.驾驶员未登录汇报时间间隔,
                StateRepotrInterval_Sleep = response.ResultData.ParaItems.休眠时汇报时间间隔,
                StateRepotrInterval_Urgent = response.ResultData.ParaItems.紧急报警时汇报时间间隔,
                StateRepotrInterval_General = response.ResultData.ParaItems.缺省时间汇报间隔,
                MaximumSpeed = response.ResultData.ParaItems.最高速度,
                OverspeedCheckTime = response.ResultData.ParaItems.超速持续时间,
                DifferenceOfOverspeedValues = response.ResultData.ParaItems.超速报警预警差值,
                MaximumDrivingTime = response.ResultData.ParaItems.连续驾驶时间门限,
                MaximumDrivingTimeToday = response.ResultData.ParaItems.当天累计驾驶时间门限,
                DifferenceOfFatigueDrivingTime = response.ResultData.ParaItems.疲劳驾驶预警差值,
                MinimumBreakTime = response.ResultData.ParaItems.最小休息时间,
                MaximumStoppingTime = response.ResultData.ParaItems.最长停车时间,
                TotalODO = response.ResultData.ParaItems.车辆里程表读数,
                TelphoneNO = response.ResultData.ParaItems.监听电话号码.FirstOrDefault()
            };

            TerminalSettingsBLL.InsertTerminalOperationsLog(
                    userid, TerminalSettingType.TerminalSetup_Query, PlateNum,
                    TerminalCode, response.State, JsonConvert.SerializeObject(new { Vehicle = Vehicle, Settings = Settings }),
                    null, GetRemoteAddress()
                );

            return Content(JsonConvert.SerializeObject(result));
        }
    }
}

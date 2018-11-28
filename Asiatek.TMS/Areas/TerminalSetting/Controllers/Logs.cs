using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model.TerminalSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    partial class TerminalSetupController
    {
        /// <summary>
        /// GET: /TerminalSetting/TerminalSetup/Logs/
        /// 终端参数设置操作日志
        /// </summary>
        public ActionResult Logs()
        {

            var model = new SearchDataWithPagedDatas<TerminalSetupLogs_QueryModel, TerminalSetupLogs_DataModel>();
            model.PagedDatas = new AsiatekPagedList<TerminalSetupLogs_DataModel>(
                new List<TerminalSetupLogs_DataModel>(), 1, 10, 0
            );

            return PartialView("Logs", model);
        }

        [HttpPost]
        public ActionResult LoadLogs(TerminalSetupLogs_QueryModel conditions, int? searchPage)
        {
            var model = new SearchDataWithPagedDatas<TerminalSetupLogs_QueryModel, TerminalSetupLogs_DataModel>();
            model.SearchModel = conditions;

            if (searchPage == null)
            {
                searchPage = 1;
            }
            
            var result = TerminalSettingsBLL.QueryTerminalSetupLogs(
                conditions.CompanyName,
                conditions.PlateNumber,
                conditions.TerminalCode,
                conditions.SettingType,
                conditions.StartTime,
                conditions.EndTime,
                searchPage.Value
            );

            model.PagedDatas = result.Item1.ToPagedList(searchPage.Value, 10, result.Item2);

            ViewBag.Settings = "";

            return PartialView("Logs_DataTable", model);
        }
    }
}
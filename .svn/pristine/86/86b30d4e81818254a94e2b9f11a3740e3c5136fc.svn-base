using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Resource;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Controllers
{
    public class LHZW_HistoricalRouteController : BaseController
    {
        //
        // GET: /LHZW_HistoricalRoute/

        public ActionResult Index()
        {
            //ViewBag.Title = base.GetCurrentViewName();
            var model = new HistorySignalAllInfoModels();
            //model.StartTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            //model.EndTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            return View(model);
        }

        #region  历史轨迹
        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("Index")]
        public ActionResult Search(long vid, string vName, string VIN, string startTime, string endTime, string invTime, string isSpeed, bool showStopPoint)
        {
            HistorySignalAllInfoModels model = new HistorySignalAllInfoModels();
            model.VehicleID = vid;
            model.VehicleName = vName;
            model.VIN = VIN;
            model.StartTime = startTime;
            model.EndTime = endTime;
            model.InvTime = invTime;
            model.IsSpeed = isSpeed;
            model.showStopPoint = showStopPoint;
            var list = HistoricalBLL.GetHistoricalInfo(model);
            //理论上来说如果查出来的数据量大于int32.maxvalue，那么list也会返回null，所以又可能不是查不出数据而是数据太多，不过实际上不太可能这么多数据，所以这里暂时不处理
            if (list == null)
            {
                list = new List<HistorySignalShowListModel>();
            }
            //最好的做法是发现信号点太多直接给出提示要求缩小查询范围，否则地图也会很卡,这里就先算了
            //修正当信号过多时超出json长度的问题，理论上来说
            return new JsonResult()
            {
                Data = list,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        #endregion


        #region  导出查询所有轨迹信号
        [AsiatekSubordinateFunction("Index")]
        public ActionResult ExportAllToExcel(long vid, string vName, string VIN, string strucName, string startTime, string endTime, string invTime, string isSpeed)
        {
            #region 求导出的数据结果
            HistorySignalAllInfoModels model = new HistorySignalAllInfoModels();
            model.VehicleID = vid;
            model.VehicleName = vName;
            model.VIN = VIN;
            model.StrucName = strucName;
            model.StartTime = startTime;
            model.EndTime = endTime;
            model.InvTime = invTime;
            model.IsSpeed = isSpeed;
            var list = HistoricalBLL.GetHistoricalInfo(model);
            model.Datas = list;
            #endregion

            ViewBag.ExportFlag = true;
            ViewData.Model = model;
            string viewHtml = RenderPartialViewToString(this, "_ExportAllSignal");
            var reportName = vName + "（" + strucName + "）" + @DisplayText.HistoryRoute + DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(System.Text.Encoding.UTF8.GetBytes(viewHtml), "application/ms-excel", string.Format("{0}.xls", reportName + "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")"));
        }
        #endregion



    }
}

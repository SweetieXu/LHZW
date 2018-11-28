/*
 模块：报表管理
 编写：蒋正波
 时间：2016-10-20
 功能：超速报表的数据展示和导出
 */
using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using Asiatek.TMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Asiatek.DBUtility;


namespace Asiatek.TMS.Areas.ReportManage.Controllers
{
    public class ReportsController : BaseController
    {
        #region   超速报表
        public ActionResult SpeedReport()
        {
            #region  老版本
            //SearchDataWithPagedDatas<ReportSearchModels, ReportListModel> model =
            //    new SearchDataWithPagedDatas<ReportSearchModels, ReportListModel>();
            //model.SearchModel = new ReportSearchModels();
            //model.PagedDatas = ReportBLL.GetPagedSpeed(model.SearchModel, 1, this.PageSize);
            //return PartialView("_SpeedReportGrid", model);
            #endregion

            var model = new ReportSearchModels();
            model.SearchPage = 1;
            //model.GPSStartTime = DateTime.Now.AddDays(-1).ToString("yyyy-M-dd 00:00:00");
            //model.GPSEndTime = DateTime.Now.AddDays(-1).ToString("yyyy-M-dd 23:59:59");
            //model.GPSStartTime = "2016-10-10 00:00:00";
            //model.GPSEndTime = "2016-10-10 23:59:59";
            //model.PagedDatas = ReportBLL.GetPagedSpeed(model, 1, this.PageSize, base.CurrentUserID);
            model.GPSStartTime = DateTime.Now.AddDays(-1).ToString("yyyy-M-dd 00:00:00");
            model.GPSEndTime = DateTime.Now.AddDays(-1).ToString("yyyy-M-dd 23:59:59");

            DataTable dt = new DataTable();
            //dt.Columns.Add("StrucName", typeof(string));
            //dt.Columns.Add("VehicleName", typeof(string));
            //dt.Columns.Add("GPSStartTime", typeof(string));
            //dt.Columns.Add("GPSEndTime", typeof(string));
            //dt.Columns.Add("Time", typeof(string));

            //DataRow dr = dt.NewRow();
            //dr["StrucName"] = "南京亚士德";
            //dr["VehicleName"] = "京TEX008";
            //dr["GPSStartTime"] = "2016-10-10 15:04:20";
            //dr["GPSEndTime"] = "2016-10-10 15:05:32";
            //dr["Time"] = "1分12秒";
            //dt.Rows.Add(dr);

            //DataRow dr1 = dt.NewRow();
            //dr1["StrucName"] = "南京亚士德";
            //dr1["VehicleName"] = "亚A00001";
            //dr1["GPSStartTime"] = "2016-10-10  17:06:52";
            //dr1["GPSEndTime"] = "2016-10-10  17:08:01";
            //dr1["Time"] = "1分09秒";
            //dt.Rows.Add(dr1);

            //DataRow dr2 = dt.NewRow();
            //dr2["StrucName"] = "南京亚士德";
            //dr2["VehicleName"] = "京TEX201";
            //dr2["GPSStartTime"] = "2016-10-10  17:06:52";
            //dr2["GPSEndTime"] = "2016-10-10  17:08:01";
            //dr2["Time"] = "1分09秒";
            //dt.Rows.Add(dr2);


            List<ReportListModel> list = ConvertToList<ReportListModel>.Convert(dt);
            model.PagedDatas = list.ToPagedList(1, 1, 1);


            return PartialView("_SpeedReportGrid", model);
        }

        [AsiatekSubordinateFunction("SpeedReport")]
        public ActionResult GetOverSpeedInfo(ReportSearchModels model)
        {
            #region  老版本
            //     SearchDataWithPagedDatas<ReportSearchModels, ReportListModel> result =
            //new SearchDataWithPagedDatas<ReportSearchModels, ReportListModel>();
            //     result.SearchModel = model;
            //     result.PagedDatas = ReportBLL.GetPagedSpeed(model, searchPage, this.PageSize);
            //     return PartialView("_SpeedReportDetailGrid", result);
            #endregion

            //model.GPSStartTime = "2016-10-10 00:00:00";
            //model.GPSEndTime = "2016-10-10 23:59:59";
            //model.PagedDatas = ReportBLL.GetPagedSpeed(model, model.SearchPage, this.PageSize, base.CurrentUserID);

            DataTable dt = new DataTable();
            dt.Columns.Add("StrucName", typeof(string));
            dt.Columns.Add("VehicleName", typeof(string));
            dt.Columns.Add("GPSStartTime", typeof(string));
            dt.Columns.Add("GPSEndTime", typeof(string));
            dt.Columns.Add("Time", typeof(string));

            DataRow dr = dt.NewRow();
            dr["StrucName"] = "南京亚士德";
            dr["VehicleName"] = "京TEX008";
            dr["GPSStartTime"] = "2016-10-10 15:04:20";
            dr["GPSEndTime"] = "2016-10-10 15:05:32";
            dr["Time"] = "1分12秒";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["StrucName"] = "南京亚士德";
            dr1["VehicleName"] = "亚A00001";
            dr1["GPSStartTime"] = "2016-10-10  17:06:52";
            dr1["GPSEndTime"] = "2016-10-10  17:08:01";
            dr1["Time"] = "1分09秒";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["StrucName"] = "南京亚士德";
            dr2["VehicleName"] = "京TEX201";
            dr2["GPSStartTime"] = "2016-10-10  17:06:52";
            dr2["GPSEndTime"] = "2016-10-10  17:08:01";
            dr2["Time"] = "1分09秒";
            dt.Rows.Add(dr2);


            DataRow dr3 = dt.NewRow();
            dr3["StrucName"] = "南京亚士德";
            dr3["VehicleName"] = "京TEX008";
            dr3["GPSStartTime"] = "2016-10-10 15:04:20";
            dr3["GPSEndTime"] = "2016-10-10 15:05:32";
            dr3["Time"] = "1分12秒";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["StrucName"] = "南京亚士德";
            dr4["VehicleName"] = "亚A00001";
            dr4["GPSStartTime"] = "2016-10-10  17:06:52";
            dr4["GPSEndTime"] = "2016-10-10  17:08:01";
            dr4["Time"] = "1分09秒";
            dt.Rows.Add(dr4);

            DataRow dr5 = dt.NewRow();
            dr5["StrucName"] = "南京亚士德";
            dr5["VehicleName"] = "京TEX201";
            dr5["GPSStartTime"] = "2016-10-10  17:06:52";
            dr5["GPSEndTime"] = "2016-10-10  17:08:01";
            dr5["Time"] = "1分09秒";
            dt.Rows.Add(dr5);

            DataRow dr6 = dt.NewRow();
            dr6["StrucName"] = "南京亚士德";
            dr6["VehicleName"] = "京TEX008";
            dr6["GPSStartTime"] = "2016-10-10 15:04:20";
            dr6["GPSEndTime"] = "2016-10-10 15:05:32";
            dr6["Time"] = "1分12秒";
            dt.Rows.Add(dr6);

            //DataRow dr7 = dt.NewRow();
            //dr7["StrucName"] = "南京亚士德";
            //dr7["VehicleName"] = "亚A00001";
            //dr7["GPSStartTime"] = "2016-10-10  17:06:52";
            //dr7["GPSEndTime"] = "2016-10-10  17:08:01";
            //dr7["Time"] = "1分09秒";
            //dt.Rows.Add(dr7);

            //DataRow dr8 = dt.NewRow();
            //dr8["StrucName"] = "南京亚士德";
            //dr8["VehicleName"] = "京TEX201";
            //dr8["GPSStartTime"] = "2016-10-10  17:06:52";
            //dr8["GPSEndTime"] = "2016-10-10  17:08:01";
            //dr8["Time"] = "1分09秒";
            //dt.Rows.Add(dr8);

            //DataRow dr9 = dt.NewRow();
            //dr9["StrucName"] = "南京亚士德";
            //dr9["VehicleName"] = "京TEX008";
            //dr9["GPSStartTime"] = "2016-10-10 15:04:20";
            //dr9["GPSEndTime"] = "2016-10-10 15:05:32";
            //dr9["Time"] = "1分12秒";
            //dt.Rows.Add(dr9);

            //DataRow dr10 = dt.NewRow();
            //dr10["StrucName"] = "南京亚士德";
            //dr10["VehicleName"] = "亚A00001";
            //dr10["GPSStartTime"] = "2016-10-10  17:06:52";
            //dr10["GPSEndTime"] = "2016-10-10  17:08:01";
            //dr10["Time"] = "1分09秒";
            //dt.Rows.Add(dr10);

            //DataRow dr11 = dt.NewRow();
            //dr11["StrucName"] = "南京亚士德";
            //dr11["VehicleName"] = "京TEX201";
            //dr11["GPSStartTime"] = "2016-10-10  17:06:52";
            //dr11["GPSEndTime"] = "2016-10-10  17:08:01";
            //dr11["Time"] = "1分09秒";
            //dt.Rows.Add(dr11);
            List<ReportListModel> list = ConvertToList<ReportListModel>.Convert(dt);
            model.PagedDatas = list.ToPagedList(1, 1, 1);


            return PartialView("_SpeedReportDetailGrid", model);
        }

        [HttpPost]
        [AsiatekSubordinateFunction("SpeedReport")]
        public ActionResult ExportToExcel(ReportSearchModels model)
        {
            #region  老版本
            //        SearchDataWithPagedDatas<ReportSearchModels, ReportListModel> result =
            //new SearchDataWithPagedDatas<ReportSearchModels, ReportListModel>();
            //        result.SearchModel = model;
            //        result.PagedDatas = ReportBLL.GetPagedSpeed(model, 1, this.PageSize);

            //        ViewBag.ExportFlag = true;
            //        ViewData.Model = result;
            //        string viewHtml = RenderPartialViewToString(this, "_SpeedReportDetailGrid");
            //        return File(System.Text.Encoding.UTF8.GetBytes(viewHtml), "application/ms-excel", string.Format("资料{0}.xls", Guid.NewGuid()));
            #endregion

            //model.PagedDatas = ReportBLL.GetPagedSpeed(model, 1, Int32.MaxValue, base.CurrentUserID);
            DataTable dt = new DataTable();
            dt.Columns.Add("StrucName", typeof(string));
            dt.Columns.Add("VehicleName", typeof(string));
            dt.Columns.Add("GPSStartTime", typeof(string));
            dt.Columns.Add("GPSEndTime", typeof(string));
            dt.Columns.Add("Time", typeof(string));

            DataRow dr = dt.NewRow();
            dr["StrucName"] = "南京亚士德";
            dr["VehicleName"] = "京TEX008";
            dr["GPSStartTime"] = "2016-10-10 15:04:20";
            dr["GPSEndTime"] = "2016-10-10 15:05:32";
            dr["Time"] = "1分12秒";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["StrucName"] = "南京亚士德";
            dr1["VehicleName"] = "亚A00001";
            dr1["GPSStartTime"] = "2016-10-10  17:06:52";
            dr1["GPSEndTime"] = "2016-10-10  17:08:01";
            dr1["Time"] = "1分09秒";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["StrucName"] = "南京亚士德";
            dr2["VehicleName"] = "京TEX201";
            dr2["GPSStartTime"] = "2016-10-10  17:06:52";
            dr2["GPSEndTime"] = "2016-10-10  17:08:01";
            dr2["Time"] = "1分09秒";
            dt.Rows.Add(dr2);


            List<ReportListModel> list = ConvertToList<ReportListModel>.Convert(dt);

            model.PagedDatas = list.ToPagedList(1, 1, 1);
            ViewBag.ExportFlag = true;
            ViewData.Model = model;
            string viewHtml = RenderPartialViewToString(this, "_SpeedReportDetailGrid");
            var reportName = this.GetUIText(base.GetFunctionsInfo("ReportManage", "Reports", "SpeedReport").FunctionName); ;   //功能名称
            return File(System.Text.Encoding.UTF8.GetBytes(viewHtml), "application/ms-excel", string.Format("{0}.xls", reportName + "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")"));
        }
        #endregion
    }
}

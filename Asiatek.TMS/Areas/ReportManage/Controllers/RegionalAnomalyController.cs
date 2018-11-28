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
    public class RegionalAnomalyController : BaseController
    {
        #region   区域异常
        public ActionResult SeachRegionalAnomaly()
        {
            var model = new RegionalAnomalySearchModel();
            model.SearchPage = 1;
            model.GPSStartTime = DateTime.Now.AddDays(-1).ToString("yyyy-M-dd 00:00:00");
            model.GPSEndTime = DateTime.Now.AddDays(-1).ToString("yyyy-M-dd 23:59:59");

            DataTable dt = new DataTable();
            //dt.Columns.Add("StrucName", typeof(string));
            //dt.Columns.Add("VehicleName", typeof(string));
            //dt.Columns.Add("AreaName", typeof(string));
            //dt.Columns.Add("ExceptionTypeName", typeof(string));
            //dt.Columns.Add("GPSStartTime", typeof(string));
            //dt.Columns.Add("GPSEndTime", typeof(string));
            //dt.Columns.Add("Time", typeof(string));

            //DataRow dr = dt.NewRow();
            //dr["StrucName"] = "南京亚士德";
            //dr["VehicleName"] = "京TEX008";
            //dr["AreaName"] = "软件大道";
            //dr["ExceptionTypeName"] = "路段超速";
            //dr["GPSStartTime"] = "2016-10-10 15:04:20";
            //dr["GPSEndTime"] = "2016-10-10 15:05:32";
            //dr["Time"] = "1分12秒";
            //dt.Rows.Add(dr);

            //DataRow dr1 = dt.NewRow();
            //dr1["StrucName"] = "南京亚士德";
            //dr1["VehicleName"] = "亚A00001";
            //dr1["AreaName"] = "中华门";
            //dr1["ExceptionTypeName"] = "进路线";
            //dr1["GPSStartTime"] = "2016-10-10  17:06:52";
            //dr1["GPSEndTime"] = "2016-10-10  17:08:01";
            //dr1["Time"] = "1分09秒";
            //dt.Rows.Add(dr1);

            //DataRow dr2 = dt.NewRow();
            //dr2["StrucName"] = "南京亚士德";
            //dr2["VehicleName"] = "京TEX201";
            //dr2["AreaName"] = "将军山";
            //dr2["ExceptionTypeName"] = "出路线";
            //dr2["GPSStartTime"] = "2016-10-10  17:06:52";
            //dr2["GPSEndTime"] = "2016-10-10  17:08:01";
            //dr2["Time"] = "1分12秒";
            //dt.Rows.Add(dr2);


            List<RegionalAnomalyListModel> list = ConvertToList<RegionalAnomalyListModel>.Convert(dt);
            model.PagedDatas = list.ToPagedList(1, 1, 1);

            return PartialView("_SeachRegionalAnomaly", model);
        }


        [AsiatekSubordinateFunction("SeachRegionalAnomaly")]
        public ActionResult GetRegionalAnomalyInfo(RegionalAnomalySearchModel model)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StrucName", typeof(string));
            dt.Columns.Add("VehicleName", typeof(string));
            dt.Columns.Add("AreaName", typeof(string));
            dt.Columns.Add("ExceptionTypeName", typeof(string));
            dt.Columns.Add("GPSStartTime", typeof(string));
            dt.Columns.Add("GPSEndTime", typeof(string));
            dt.Columns.Add("Time", typeof(string));

            DataRow dr = dt.NewRow();
            dr["StrucName"] = "南京亚士德";
            dr["VehicleName"] = "京TEX008";
            dr["AreaName"] = "软件大道";
            dr["ExceptionTypeName"] = "出区域";
            dr["GPSStartTime"] = "2016-10-10 15:04:20";
            dr["GPSEndTime"] = "2016-10-10 15:05:32";
            dr["Time"] = "1分12秒";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["StrucName"] = "南京亚士德";
            dr1["VehicleName"] = "亚A00001";
            dr1["AreaName"] = "中华门";
            dr1["ExceptionTypeName"] = "进区域";
            dr1["GPSStartTime"] = "2016-10-10  17:06:52";
            dr1["GPSEndTime"] = "2016-10-10  17:08:01";
            dr1["Time"] = "1分12秒";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["StrucName"] = "南京亚士德";
            dr2["VehicleName"] = "京TEX201";
            dr2["AreaName"] = "将军山";
            dr2["ExceptionTypeName"] = "进区域";
            dr2["GPSStartTime"] = "2016-10-10  17:06:52";
            dr2["GPSEndTime"] = "2016-10-10  17:08:01";
            dr2["Time"] = "1分12秒";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["StrucName"] = "南京亚士德";
            dr3["VehicleName"] = "钢A00021";
            dr3["AreaName"] = "奥体大街";
            dr3["ExceptionTypeName"] = "出区域";
            dr3["GPSStartTime"] = "2016-10-10  17:06:52";
            dr3["GPSEndTime"] = "2016-10-10  17:08:01";
            dr3["Time"] = "1分09秒";
            dt.Rows.Add(dr3);

            List<RegionalAnomalyListModel> list = ConvertToList<RegionalAnomalyListModel>.Convert(dt);
            model.PagedDatas = list.ToPagedList(1, 1, 1);

            return PartialView("_RegionalAnomalyGrid", model);
        }


        [HttpPost]
        [AsiatekSubordinateFunction("SeachRegionalAnomaly")]
        public ActionResult RegionalAnomalyToExcel(RegionalAnomalySearchModel model)
        {
            //model.PagedDatas = ReportBLL.GetPagedSpeed(model, 1, Int32.MaxValue, base.CurrentUserID);
            DataTable dt = new DataTable();
            dt.Columns.Add("StrucName", typeof(string));
            dt.Columns.Add("VehicleName", typeof(string));
            dt.Columns.Add("AreaName", typeof(string));
            dt.Columns.Add("ExceptionTypeName", typeof(string));
            dt.Columns.Add("GPSStartTime", typeof(string));
            dt.Columns.Add("GPSEndTime", typeof(string));
            dt.Columns.Add("Time", typeof(string));

            DataRow dr = dt.NewRow();
            dr["StrucName"] = "南京亚士德";
            dr["VehicleName"] = "京TEX008";
            dr["AreaName"] = "软件大道";
            dr["ExceptionTypeName"] = "出区域";
            dr["GPSStartTime"] = "2016-10-10 15:04:20";
            dr["GPSEndTime"] = "2016-10-10 15:05:32";
            dr["Time"] = "1分12秒";
            dt.Rows.Add(dr);

            DataRow dr1 = dt.NewRow();
            dr1["StrucName"] = "南京亚士德";
            dr1["VehicleName"] = "亚A00001";
            dr1["AreaName"] = "中华门";
            dr1["ExceptionTypeName"] = "进区域";
            dr1["GPSStartTime"] = "2016-10-10  17:06:52";
            dr1["GPSEndTime"] = "2016-10-10  17:08:01";
            dr1["Time"] = "1分12秒";
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["StrucName"] = "南京亚士德";
            dr2["VehicleName"] = "京TEX201";
            dr2["AreaName"] = "将军山";
            dr2["ExceptionTypeName"] = "出区域";
            dr2["GPSStartTime"] = "2016-10-10  17:06:52";
            dr2["GPSEndTime"] = "2016-10-10  17:08:01";
            dr2["Time"] = "1分12秒";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["StrucName"] = "南京亚士德";
            dr3["VehicleName"] = "钢A00021";
            dr3["AreaName"] = "奥体大街";
            dr3["ExceptionTypeName"] = "进区域";
            dr3["GPSStartTime"] = "2016-10-10  17:06:52";
            dr3["GPSEndTime"] = "2016-10-10  17:08:01";
            dr3["Time"] = "1分09秒";
            dt.Rows.Add(dr3);

            List<RegionalAnomalyListModel> list = ConvertToList<RegionalAnomalyListModel>.Convert(dt);
            model.PagedDatas = list.ToPagedList(1, 1, 1);
            ViewBag.ExportFlag = true;
            ViewData.Model = model;
            string viewHtml = RenderPartialViewToString(this, "_RegionalAnomalyGrid");
            var reportName = this.GetUIText(base.GetFunctionsInfo("ReportManage", "RegionalAnomaly", "SeachRegionalAnomaly").FunctionName);   //功能名称
            return File(System.Text.Encoding.UTF8.GetBytes(viewHtml), "application/ms-excel", string.Format("{0}.xls", reportName + "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")"));
        }
        #endregion
    }
}

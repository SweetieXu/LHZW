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
    public class FatigueDriveController : BaseController
    {
        #region   疲劳驾驶
        public ActionResult SeachFatigueDrive()
        {
            var model = new FatigueDriveSeachModel();
            model.SearchPage = 1;
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
            

            List<FatigueDriveListModel> list = ConvertToList<FatigueDriveListModel>.Convert(dt);
            model.PagedDatas = list.ToPagedList(1, 1, 1);

            return PartialView("_SeachFatigueDrive", model);
        }


        [AsiatekSubordinateFunction("SeachFatigueDrive")]
        public ActionResult GetFatigueDriveInfo(FatigueDriveSeachModel model)
        {
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
            dr3["VehicleName"] = "汇A00002";
            dr3["GPSStartTime"] = "2016-10-10 15:04:20";
            dr3["GPSEndTime"] = "2016-10-10 15:05:32";
            dr3["Time"] = "1分12秒";
            dt.Rows.Add(dr3);
            List<FatigueDriveListModel> list = ConvertToList<FatigueDriveListModel>.Convert(dt);
            model.PagedDatas = list.ToPagedList(1, 1, 1);

            return PartialView("_FatigueDriveGrid", model);
        }


        [HttpPost]
        [AsiatekSubordinateFunction("SeachFatigueDrive")]
        public ActionResult FatigueDriveToExcel(FatigueDriveSeachModel model)
        {
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

            DataRow dr3 = dt.NewRow();
            dr3["StrucName"] = "南京亚士德";
            dr3["VehicleName"] = "汇A00002";
            dr3["GPSStartTime"] = "2016-10-10 15:04:20";
            dr3["GPSEndTime"] = "2016-10-10 15:05:32";
            dr3["Time"] = "1分12秒";
            dt.Rows.Add(dr3);
            List<FatigueDriveListModel> list = ConvertToList<FatigueDriveListModel>.Convert(dt);
            model.PagedDatas = list.ToPagedList(1, 1, 1);

            ViewBag.ExportFlag = true;
            ViewData.Model = model;
            string viewHtml = RenderPartialViewToString(this, "_FatigueDriveGrid");
            var reportName = this.GetUIText(base.GetFunctionsInfo("ReportManage", "FatigueDrive", "SeachFatigueDrive").FunctionName);   //功能名称
            return File(System.Text.Encoding.UTF8.GetBytes(viewHtml), "application/ms-excel", string.Format("{0}.xls", reportName + "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")"));
        }
        #endregion
    }
}

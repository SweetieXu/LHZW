using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Resource;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asiatek.TMS.ReportFiles.reports
{
    public partial class ExceptionHandleRpt : RptBaseForm
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; private set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CheckReportPermission();
                btnSearch.Text = UIText.Search;
                btnSearch.Attributes.Add("style", "display:none;");
                var oneweekago = DateTime.Now.AddDays(-7);
                var yesterday = DateTime.Now.AddDays(-1);
                this.StartTime = oneweekago.ToString("yyyy-MM-dd 00:00:00");
                this.EndTime = yesterday.ToString("yyyy-MM-dd 23:59:59");
                hidStartTime.Value = this.StartTime;
                hidEndTime.Value = this.EndTime;
                base.SetReportViewer(this.rvResult);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var startTime = DateTime.Parse(hidStartTime.Value);
            var endTime = DateTime.Parse(hidEndTime.Value);
            if (!base.CheckDateInput(startTime, endTime, ltCheckInfo))
            {
                ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                return;
            }
            // 车辆ID
            long vehiclesID = 0;
            if (!string.IsNullOrWhiteSpace(this.hidVehicleID.Value))
            {
                vehiclesID = Convert.ToInt64(hidVehicleID.Value);
            }
            var rptName = base.CurrentRptName;

            try
            {
                ExceptionSearchModel model = new ExceptionSearchModel();
                model.UserID = base.CurrentUserID;
                model.SartTime = startTime;
                model.EndTime = endTime;
                model.VehiclesID = vehiclesID;

                List<ServerInfoModel> listServerInfo;
                // 默认模式
                if (base.VehicleViewMode)
                {
                    // 获取链接服务器信息
                    listServerInfo = ReportBLL.GetDefaultServerInfo(base.CurrentStrucID, vehiclesID);
                }
                // 自由模式
                else
                {
                    // 获取链接服务器信息
                    listServerInfo = ReportBLL.GetServerInfo(base.CurrentUserID, vehiclesID);
                }

                List<ExceptionHandleModel> list = new List<ExceptionHandleModel>();
                foreach (var item in listServerInfo)
                {
                    List<ExceptionHandleModel> result;
                    // 默认模式
                    if (base.VehicleViewMode)
                    {
                        result = ReportBLL.GetDefaultExceptionHandle(model, item.LinkedServerName, base.CurrentStrucID);
                    }
                    else
                    {//自由模式
                        result = ReportBLL.GetExceptionHandle(model, item.LinkedServerName);
                    }
                    if (result != null && result.Count > 0)
                    {
                        list.AddRange(result);
                    }
                }

                if (!base.CheckResult<ExceptionHandleModel>(list, ltCheckInfo))
                {
                    ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                    return;
                }
                this.ShowReport(list);
                ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                base.DoReportLog(ex.Message, ltCheckInfo);
            }
        }
        private void ShowReport(List<ExceptionHandleModel> datas)
        {
            this.rvResult.ZoomMode = ZoomMode.PageWidth;
            var rpt = this.rvResult.LocalReport;
            rpt.DataSources.Clear();


            var rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = datas;
            rpt.DataSources.Add(rds);


            string queryConditions = string.Empty;
            string vn = hidVehicleName.Value.Trim();
            if (string.IsNullOrWhiteSpace(vn))
            {
                queryConditions = string.Format("{0}：{1} ~ {2}", DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
            }
            else
            {
                queryConditions = string.Format("{0}：{1}\t\t{2}：{3} ~ {4}", DisplayText.VehicleName, vn,
                                              DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
            }
            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/ExceptionHandle.rdlc");
            List<ReportParameter> rps = new List<ReportParameter>()
            {
                new ReportParameter("RowNo",DisplayText.RowNo),

                new ReportParameter("StrucName",DisplayText.SubordinateStrucName),
                new ReportParameter("VehicleName",DisplayText.VehicleName),
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("ExceptionType",DisplayText.Exception),
                new ReportParameter("ExceptionBeginDateTime",DisplayText.ExSignalStartTime),
                new ReportParameter("ExceptionBeginAddress",DisplayText.ExStartAddress),
                new ReportParameter("ExceptionEndDateTime",DisplayText.ExSignalEndTime),
                new ReportParameter("ExceptionEndAddress",DisplayText.ExEndAddress),
                new ReportParameter("DealUserName",DisplayText.DealUser),
                new ReportParameter("DealDateTime",DisplayText.DealTime),
                new ReportParameter("DealInfo",DisplayText.DealInfo),
                new ReportParameter("QueryConditions",queryConditions)
            };
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }

    }
}
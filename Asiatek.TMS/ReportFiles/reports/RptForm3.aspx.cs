using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Resource;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Asiatek.TMS.ReportFiles.reports
{
    public partial class RptForm3 : RptBaseForm
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
                var yesterday = DateTime.Now.AddDays(-1);
                this.StartTime = yesterday.ToString("yyyy-MM-dd 00:00:00");
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
            if (!base.CheckInput(startTime, endTime, ltCheckInfo))
            {
                ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                return;
            }
            int exceptionTypeID = -1;
            try
            {
                exceptionTypeID = (int)Enum.Parse(typeof(ExceptionTypeEnum), base.CurrentRptName);
            }
            catch
            {
                exceptionTypeID = -1;
            }

            // 车辆ID
            long vehiclesID = 0;
            if (!string.IsNullOrWhiteSpace(this.hidVehicleID.Value))
            {
                vehiclesID = Convert.ToInt64(hidVehicleID.Value);
            }
            try
            {
                ExceptionSearchModel model = new ExceptionSearchModel();
                model.UserID = base.CurrentUserID;
                model.SartTime = startTime;
                model.EndTime = endTime;
                model.VehiclesID = vehiclesID;
                model.ExceptionTypeID = exceptionTypeID;
                model.DealUserName = hidDealUser.Value;
                List<ExceptionsAndDealInfoModel> list;
                if (base.VehicleViewMode)
                {
                    list = ReportBLL.GetDefaultExceptionsAndDealInfo(model,base.CurrentStrucID);
                }
                else
                {
                    list = ReportBLL.GetExceptionsAndDealInfo(model);
                }
                if (!base.CheckResult<ExceptionsAndDealInfoModel>(list, ltCheckInfo))
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

        private void ShowReport(List<ExceptionsAndDealInfoModel> datas)
        {
            this.rvResult.ZoomMode = ZoomMode.PageWidth;
            var rpt = this.rvResult.LocalReport;
            rpt.DataSources.Clear();

            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/EmergencyAlarmRpt.rdlc");

            var rds = new ReportDataSource();
            rds.Name = "Datas";
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

            var dealUser = hidDealUser.Value.Trim();
            if (!string.IsNullOrWhiteSpace(dealUser))
            {
                queryConditions = string.Format("{0}\t\t{1}：{2}", queryConditions, DisplayText.DealUser, dealUser);
            }

            List<ReportParameter> rps = new List<ReportParameter>()
            {
                new ReportParameter("RowNo",DisplayText.RowNo),

                new ReportParameter("StrucName",DisplayText.StrucWhichUseVehicle),
                new ReportParameter("VehicleName",DisplayText.VehicleName),
                new ReportParameter("StartDateTime",DisplayText.StartTime),
                new ReportParameter("EndDateTime",DisplayText.EndTime),
                 new ReportParameter("ActualDuration",DisplayText.ActualDuration),
                new ReportParameter("StartAddress",DisplayText.StartAddress),
                new ReportParameter("EndAddress",DisplayText.EndAddress),
                new ReportParameter("DealUser",DisplayText.DealUser),
                new ReportParameter("UserName",DisplayText.UserName),
                new ReportParameter("DealTime",DisplayText.DealTime),
                new ReportParameter("DealInfo",DisplayText.DealInfo),
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("QueryConditions",queryConditions)
            };
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }
    }

}
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Resource;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Asiatek.TMS.ReportFiles.reports
{
    public partial class RptForm1 : RptBaseForm
    {
        public string StartTime { get; private set; }
        public string EndTime { get; private set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CheckReportPermission();
                btnSearch.Text = UIText.Search;
                btnSearch.Attributes.Add("style","display:none;");
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
                List<ExceptionModel> list;
                // 默认模式
                if (base.VehicleViewMode)
                {
                   list = ReportBLL.GetDefaultExceptions(model,base.CurrentStrucID);
                }
                // 自由模式
                else
                {
                    list = ReportBLL.GetExceptions(model);
                }
               
                if (!base.CheckResult<ExceptionModel>(list, ltCheckInfo))
                {
                    ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                    return;
                }

               this.ShowReport(list, exceptionTypeID);
               ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
             
       
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                base.DoReportLog(ex.Message, ltCheckInfo);
            }
        }

        private void ShowReport(List<ExceptionModel> datas, int exceptionTypeID)
        {
            this.rvResult.ZoomMode = ZoomMode.PageWidth;
            var rpt = this.rvResult.LocalReport;
            rpt.DataSources.Clear();

            var rds = new ReportDataSource();
            //.rdlc中数据集的名称
            rds.Name = "Datas";
            rds.Value = datas;
            rpt.DataSources.Add(rds);

            string queryConditions = string.Empty;
            string vehicleName = hidVehicleName.Value.Trim();
            if (string.IsNullOrWhiteSpace(vehicleName))
            {
                queryConditions = string.Format("{0}：{1} ~ {2}", DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
            }
            else
            {
                queryConditions = string.Format("{0}：{1}\t\t{2}：{3} ~ {4}", DisplayText.VehicleName, vehicleName,
                                                DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
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
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("QueryConditions",queryConditions)
            };
            if (exceptionTypeID == (int)ExceptionTypeEnum.OverSpeedRpt)
            {
                rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/OverSpeed.rdlc");
                rps.Add(new ReportParameter("MaxSpeed", DisplayText.MaxSpeed));
                rps.Add(new ReportParameter("OverspeedThreshold", DisplayText.OverspeedThreshold));
                rps.Add(new ReportParameter("MinimumDuration", DisplayText.MinimumDuration));
            }
            else if (exceptionTypeID == (int)ExceptionTypeEnum.FatigueDrivingRpt)
            {
                rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/FatigueDriving.rdlc");
                rps.Add(new ReportParameter("ContinuousDrivingThreshold", DisplayText.ContinuousDrivingThreshold));
                rps.Add(new ReportParameter("MinimumBreakTime", DisplayText.MinimumBreakTime));
            }
            else if (exceptionTypeID == (int)ExceptionTypeEnum.AccumulatedDrivingOvertime)
            {
                rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/AccumulatedDrivingOvertime.rdlc");
                rps.Add(new ReportParameter("DrivingTimeThreshold", DisplayText.DrivingTimeThreshold));
            }
            else if (exceptionTypeID == (int)ExceptionTypeEnum.OvertimeParking)
            {
                rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/OvertimeParking.rdlc");
                rps.Add(new ReportParameter("MaximumParkingTime", DisplayText.MaximumParkingTime));
                rps.Add(new ReportParameter("ParkingTime", DisplayText.ParkingTime));
            }
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }
    }
}
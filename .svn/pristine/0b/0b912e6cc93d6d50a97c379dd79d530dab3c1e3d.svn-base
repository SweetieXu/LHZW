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
    public partial class NGAreaOverSpeedRpt : RptBaseForm
    {
        public string StartTime { get; private set; }
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

            // 车辆ID
            long vehiclesID = 0;
            if (!string.IsNullOrWhiteSpace(this.hidVehicleID.Value))
            {
                vehiclesID = Convert.ToInt64(hidVehicleID.Value);
            }
            int averageOfOverSpeed = 0;
            int actualDuration = 0;
            int.TryParse(this.hidAverageOfOverSpeed.Value, out averageOfOverSpeed);
            int.TryParse(this.hidActualDuration.Value, out actualDuration);
            try
            {
                ExceptionSearchModel model = new ExceptionSearchModel();
                model.UserID = base.CurrentUserID;
                model.SartTime = startTime;
                model.EndTime = endTime;
                model.VehiclesID = vehiclesID;
                List<NGAreaAverageOverSpeedModel> list;
                

                // 默认模式
                if (base.VehicleViewMode)
                {
                    list = ReportBLL.GetDefaultNGAreaAverageOverSpeed(model, base.CurrentStrucID, averageOfOverSpeed, actualDuration);
                }
                // 自由模式
                else
                {
                    list = ReportBLL.GetDefaultNGAreaAverageOverSpeed(model, averageOfOverSpeed, actualDuration);
                }

                if (!base.CheckResult<NGAreaAverageOverSpeedModel>(list, ltCheckInfo))
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

        private void ShowReport(List<NGAreaAverageOverSpeedModel> datas)
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
                 new ReportParameter("FenceName",DisplayText.OverSpeedArea),
                new ReportParameter("VehicleName",DisplayText.VehicleName),
                new ReportParameter("AverageOverSpeed",DisplayText.PerHourSpeed),
                new ReportParameter("MaxSpeed",DisplayText.IsCheckSpeed),
                new ReportParameter("OverSpeedPercent",DisplayText.OverSpeedPercent),
                new ReportParameter("StartDateTime",DisplayText.StartTime),
                new ReportParameter("EndDateTime",DisplayText.EndTime),
                new ReportParameter("ActualDuration",DisplayText.ActualDuration1),
                new ReportParameter("OverSpeedTimes",DisplayText.OverSpeed+DisplayText.OverSpeedTimes),
                new ReportParameter("Penalty",DisplayText.Penalty),
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("QueryConditions",queryConditions)
            };
            // rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/Report1.rdlc");
            //rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/NGAreaOverSpeed.rdlc");
            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/NGAreaAverageOverSpeed.rdlc");
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }
    }
}
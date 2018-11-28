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
    public partial class LHZWACCONStatisticRpt : RptBaseForm
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
                var onedayago = DateTime.Now.AddDays(-1);
                var yesterday = DateTime.Now.AddDays(-1);
                this.StartTime = onedayago.ToString("yyyy-MM-dd 00:00:00");
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
            // 车架号VIN
            string vin = string.Empty;
            if (!string.IsNullOrEmpty(this.hidVIN.Value))
            {
                vin = hidVIN.Value;
            }
            var rptName = base.CurrentRptName;

            try
            {
                ACCONStatisticSearchModel model = new ACCONStatisticSearchModel();
                model.SartTime = startTime;
                model.EndTime = endTime;
                model.VIN = vin;
                model.PlateNum = this.hidPlateNum.Value;
                List<ACCONStatisticDataModel> list;

                // 默认模式
                if (base.VehicleViewMode)
                {
                    list = ReportBLL.GetDefaultACCONStatisticData(model, base.CurrentStrucID);
                }
                // 自由模式
                else
                {
                    list = ReportBLL.GetACCONStatisticData(model, base.CurrentUserID);
                }

                if (!base.CheckResult<ACCONStatisticDataModel>(list, ltCheckInfo))
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


        private void ShowReport(List<ACCONStatisticDataModel> datas)
        {
            this.rvResult.ZoomMode = ZoomMode.PageWidth;
            var rpt = this.rvResult.LocalReport;
            rpt.DataSources.Clear();

            var rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = datas;
            rpt.DataSources.Add(rds);

            string queryConditions = string.Empty;
            string vn = hidPlateNum.Value.Trim();
            if (string.IsNullOrWhiteSpace(vn))
            {
                queryConditions = string.Format("{0}：{1} ~ {2}", DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
            }
            else
            {
                queryConditions = string.Format("{0}：{1}\t\t{2}：{3} ~ {4}", DisplayText.PlateNum, vn,
                                              DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
            }
            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/LHZWACCONStatistic.rdlc");
            List<ReportParameter> rps;
            rps = new List<ReportParameter>()
            {
                new ReportParameter("RowNo",DisplayText.RowNo),
                new ReportParameter("StrucName",DisplayText.StrucWhichUseVehicle),
                new ReportParameter("VehicleName",DisplayText.VehicleName),
                new ReportParameter("StartDateTime",DisplayText.StartTime),
                new ReportParameter("EndDateTime",DisplayText.EndTime),
                new ReportParameter("TotalTime","统计时长"),
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("QueryConditions",queryConditions)
            };
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }

    }
}
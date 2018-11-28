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
    public partial class MGJHExpForIllegalDischarg : RptBaseForm
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
                this.StartTime = DateTime.Now.ToString("yyyy-MM-01 00:00:00");
                this.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
            try
            {
                ExceptionSearchModel model = new ExceptionSearchModel();
                model.UserID = base.CurrentUserID;
                model.SartTime = startTime;
                model.EndTime = endTime;
                model.VehiclesID = vehiclesID;
                List<MGJHExpForIllegalDischargModel> list;
                // 默认模式
                if (base.VehicleViewMode)
                {
                    list = ReportBLL.GetMGJHExpForIllegalDischarg(model, base.CurrentStrucID);
                }
                // 自由模式
                else
                {
                    list = ReportBLL.GetMGJHExpForIllegalDischarg(model);
                }

                if (!base.CheckResult<MGJHExpForIllegalDischargModel>(list, ltCheckInfo))
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

        private void ShowReport(List<MGJHExpForIllegalDischargModel> datas)
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

                new ReportParameter("PoundSheetCode",DisplayText.PoundSheetCode),
                 new ReportParameter("CustomerName",DisplayText.CustomerName),
                new ReportParameter("ShippingAddress",DisplayText.ReceiveAddressName),
                new ReportParameter("VehicleName",DisplayText.VehicleName),
                new ReportParameter("StrucName",DisplayText.StrucName),
                new ReportParameter("ServerStartTime",DisplayText.Exception+DisplayText.StartTime),
                new ReportParameter("ServerEndTime",DisplayText.Exception+DisplayText.EndTime),
                new ReportParameter("ActualDuration",DisplayText.ActualDuration),
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("QueryConditions",queryConditions)
            };
            // rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/Report1.rdlc");
            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/MGJHExpForIllegalDischarg.rdlc");
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }
    }
}
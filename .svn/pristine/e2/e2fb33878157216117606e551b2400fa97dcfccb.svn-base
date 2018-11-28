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
    public partial class LHZWGateExceptionRpt : RptBaseForm
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; private set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; private set; }
        //判断门岗异常和电子围栏异常的时间误差范围  设置十分钟
        public int timeInterval = 10;

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
            // 车辆ID
            long vehicleID = 0;
            if (!string.IsNullOrWhiteSpace(this.hidVehicleID.Value))
            {
                vehicleID = Convert.ToInt64(hidVehicleID.Value);
            }
            //异常类型  0-门岗异常  1-电子围栏异常
            int exceptionTypeID = Convert.ToInt32(this.ddlExTypes.SelectedValue);

            var rptName = base.CurrentRptName;

            try
            {
                GateExceptionSearchModel model = new GateExceptionSearchModel();
                model.SartTime = startTime;
                model.EndTime = endTime;
                model.VehicleID = vehicleID;
                model.ExceptionType = exceptionTypeID;
                model.PlateNum = this.hidPlateNum.Value;

                //查询数据并且对比
                List<GateExceptionModel> list;
                if(exceptionTypeID==0)
                {
                    list = ReportBLL.GetGateException(model, timeInterval);
                }
                else
                {
                    list = ReportBLL.GetEFException(model, timeInterval);
                }
                

                if (!base.CheckResult<GateExceptionModel>(list, ltCheckInfo))
                {
                    ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                    return;
                }
                this.ShowReport(list,model.ExceptionType);
                ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                base.DoReportLog(ex.Message, ltCheckInfo);
            }
        }
        private void ShowReport(List<GateExceptionModel> datas, int exceptionType)
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
            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/LHZWGateException.rdlc");
            List<ReportParameter> rps;
            //查询门岗异常
            if (exceptionType == 0)  
            { 
                rps = new List<ReportParameter>()
                {
                    new ReportParameter("RowNo",DisplayText.RowNo),
                    new ReportParameter("PlateNum",DisplayText.PlateNum),
                    new ReportParameter("PassGate","门岗名称"),
                    new ReportParameter("InOrOut","进出门岗类型"),
                    new ReportParameter("InOrOutTime","进出门岗时间"),
                    new ReportParameter("ExceptionInfo","电子围栏异常信息"),
                    new ReportParameter("SignalStartTime",DisplayText.ExSignalStartTime),
                    new ReportParameter("SignalEndTime",DisplayText.ExSignalEndTime),
                    new ReportParameter("IsShowGate","false"),//true为启用隐藏, false为显示
                    new ReportParameter("IsShowEf","true"),
                    new ReportParameter("RealReportName",base.CurrentRptRealName),
                    new ReportParameter("QueryConditions",queryConditions)
                };
            }
            else
            {
                rps = new List<ReportParameter>()
                {
                    new ReportParameter("RowNo",DisplayText.RowNo),
                    new ReportParameter("PlateNum",DisplayText.PlateNum),
                    new ReportParameter("PassGate","门岗名称"),
                    new ReportParameter("InOrOut","进出门岗类型"),
                    new ReportParameter("InOrOutTime","进出门岗时间"),
                    new ReportParameter("ExceptionInfo","电子围栏异常信息"),
                    new ReportParameter("SignalStartTime",DisplayText.ExSignalStartTime),
                    new ReportParameter("SignalEndTime",DisplayText.ExSignalEndTime),
                    new ReportParameter("IsShowGate","true"),//true为启用隐藏, false为显示
                    new ReportParameter("IsShowEf","false"),
                    new ReportParameter("RealReportName",base.CurrentRptRealName),
                    new ReportParameter("QueryConditions",queryConditions)
                };
            }
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }
    }
}
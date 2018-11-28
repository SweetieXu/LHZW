using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Resource;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Asiatek.TMS.ReportFiles.reports
{
    public partial class RptForm2 : RptBaseForm
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

                //var list = ExceptionBLL.GetVehicleDistance(model);
                List<VehicleDistanceModel> list = new List<VehicleDistanceModel>();
                foreach (var item in listServerInfo)
                {
                    List<VehicleDistanceModel> result;
                    // 默认模式
                    if (base.VehicleViewMode)
                    {
                        //result = ReportBLL.GetDefaultVehicleDistance(model, item.LinkedServerName, base.CurrentStrucID);
                        result = ReportBLL.GetDefaultVehicleDistanceNew(model, item.LinkedServerName, base.CurrentStrucID);
                    }
                    else
                    {
                        //result = ReportBLL.GetVehicleDistance(model, item.LinkedServerName);
                        result = ReportBLL.GetVehicleDistanceNew(model, item.LinkedServerName);
                    }
                    if (result != null && result.Count > 0)
                    {
                        list.AddRange(result);
                    }
                }

                if (!base.CheckResult<VehicleDistanceModel>(list, ltCheckInfo))
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
        private void ShowReport(List<VehicleDistanceModel> datas)
        {
            this.rvResult.ZoomMode = ZoomMode.PageWidth;
            var rpt = this.rvResult.LocalReport;
            rpt.DataSources.Clear();
            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/Mileage.rdlc");

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

            List<ReportParameter> rps = new List<ReportParameter>()
            {
                new ReportParameter("RowNo",DisplayText.RowNo),

                new ReportParameter("StrucName",DisplayText.StrucWhichUseVehicle),
                new ReportParameter("VehicleName",DisplayText.VehicleName),
                new ReportParameter("StartDateTime",DisplayText.StartTime),
                new ReportParameter("EndDateTime",DisplayText.EndTime),
                new ReportParameter("Distance",DisplayText.Mileage),
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("QueryConditions",queryConditions),
            };
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }


        //private Task<List<VehicleDistanceModel>> SyncVehicleDistance(ExceptionSearchModel model)
        //{
        //    //这里启用异步任务仅仅只是为了不同步顺序一个一个的去调用接口，但是由于是同步IO，所以依然会阻塞后台线程,这点暂时不优化，除非自己写调用
        //    return Task<List<VehicleDistanceModel>>.Run(() =>
        //    {
        //        return ExceptionBLL.GetVehicleDistance(model);
        //    });
        //}
    }
}
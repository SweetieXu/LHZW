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
    public partial class TemperForJSYY : RptBaseForm
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

                List<TemperModel> list = new List<TemperModel>();
                foreach (var item in listServerInfo)
                {
                    List<TemperModel> result;
                    // 默认模式
                    if (base.VehicleViewMode)
                    {
                        result = ReportBLL.GetDefaultTemper(model, item.LinkedServerName, base.CurrentStrucID);
                    }
                    else
                    {//自由模式
                        result = ReportBLL.GetTemper(model, item.LinkedServerName);
                    }
                    if (result != null && result.Count > 0)
                    {
                        list.AddRange(result);
                    }
                }
                List<TemperModel> resultlist = new List<TemperModel>();
                if (list != null && list.Count > 0)
                {
                    #region 时间间隔的计算
                    //计算第一笔信号与最后一笔信号的时间间隔然后看与设置的时间周期除有几个时间周期
                    double timedifference = (list.OrderByDescending(a => a.SignalDateTime).First().SignalDateTime - list.OrderBy(a => a.SignalDateTime).First().SignalDateTime).TotalMinutes;
                    int settingtimedifference = Convert.ToInt32(txtTimespan.Value);
                    int cycle = (int)(timedifference / settingtimedifference);
                    DateTime firstsignals = list.OrderBy(a => a.SignalDateTime).First().SignalDateTime;
                    TemperModel firsttm = list.OrderBy(a => a.SignalDateTime).First();
                    resultlist.Add(firsttm);
                    Random rd = new Random();
                    //以查到的第一个信号为基准，循环添加设置的周期分钟数，取分钟数前后较理论时间间隔小的数据添加为模拟信号时间信号信息
                    for (int i = 1; i <= cycle; i++)
                    {
                        DateTime cycletime = firstsignals.AddMinutes(settingtimedifference * i);
                        int addseconds = rd.Next(-2, 2);
                        TemperModel previoussignal = list.Where(a => a.SignalDateTime <= cycletime).OrderByDescending(a => a.SignalDateTime).First();
                        double previousspan = (cycletime - previoussignal.SignalDateTime).TotalMinutes;
                        TemperModel lastsignal = list.Where(a => a.SignalDateTime >= cycletime).OrderBy(a => a.SignalDateTime).First();
                        double lastspan = (lastsignal.SignalDateTime - cycletime).TotalMinutes;
                        if (previousspan >= lastspan)
                        {
                            TemperModel tm = new TemperModel()
                            {
                                VIN = lastsignal.VIN,
                                StrucName = lastsignal.StrucName,
                                VehicleName = lastsignal.VehicleName,
                                SignalDateTime = cycletime.AddSeconds(addseconds),
                                Temperature = lastsignal.Temperature,
                                ACCState = lastsignal.ACCState,
                                Speed = lastsignal.Speed,
                            };
                            resultlist.Add(tm);
                        }
                        else
                        {
                            TemperModel tm = new TemperModel()
                            {
                                VIN = previoussignal.VIN,
                                StrucName = previoussignal.StrucName,
                                VehicleName = previoussignal.VehicleName,
                                SignalDateTime = cycletime.AddSeconds(addseconds),
                                Temperature = previoussignal.Temperature,
                                ACCState = previoussignal.ACCState,
                                Speed = previoussignal.Speed,
                            };
                            resultlist.Add(tm);
                        }
                    }
                    #endregion
                }

                if (!base.CheckResult<TemperModel>(resultlist, ltCheckInfo))
                {
                    ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                    return;
                }
                this.ShowReport(resultlist.OrderByDescending(a => a.SignalDateTime).ToList());
                ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(upRpt, this.Page.GetType(), "", "canClick()", true);
                base.DoReportLog(ex.Message, ltCheckInfo);
            }
        }
        private void ShowReport(List<TemperModel> datas)
        {
            this.rvResult.ZoomMode = ZoomMode.PageWidth;
            var rpt = this.rvResult.LocalReport;
            rpt.DataSources.Clear();
            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/Temper.rdlc");

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

                new ReportParameter("StrucName",DisplayText.SubordinateStrucName),
                new ReportParameter("VehicleName",DisplayText.VehicleName),
                new ReportParameter("SignalDateTime",DisplayText.SignalDateTime),
                new ReportParameter("Temperature",DisplayText.Temperature),
                new ReportParameter("ACCState",DisplayText.ACCState),
                new ReportParameter("Speed",DisplayText.Speed),
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("QueryConditions",queryConditions),
            };
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }

    }
}
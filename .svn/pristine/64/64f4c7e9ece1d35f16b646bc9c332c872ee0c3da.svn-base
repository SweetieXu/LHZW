using Asiatek.BLL.MSSQL;
using Asiatek.Common;
using Asiatek.Model;
using Asiatek.Resource;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asiatek.TMS.ReportFiles.reports
{
    public partial class RptForm5 : RptBaseForm
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; private set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; private set; }

        /// <summary>
        /// 获取区域或路线名称的URL
        /// </summary>
        public string LineRegionNameGetUrl { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CheckReportPermission();
                btnSearch.Text = UIText.Search;
                var yesterday = DateTime.Now.AddDays(-1);
                this.StartTime = yesterday.ToString("yyyy-MM-dd 00:00:00");
                this.EndTime = yesterday.ToString("yyyy-MM-dd 23:59:59");
                hidStartTime.Value = this.StartTime;
                hidEndTime.Value = this.EndTime;
                lblExTypeName.Text = DisplayText.ExceptionTypeName;
                var rptName = base.CurrentRptName;


                switch (rptName)
                {
                    case "RegionAbnormalRpt"://区域异常
                        lblLineRegionName.Text = DisplayText.RegionsName;
                        LineRegionNameGetUrl = new UrlHelper(this.Request.RequestContext).Content("~/TerminalSetting/MapRegions/GetStrucMapRegions");
                        break;
                    case "RouteAbnormalRpt"://路线异常
                        lblLineRegionName.Text = DisplayText.LinesName;
                        LineRegionNameGetUrl = new UrlHelper(this.Request.RequestContext).Content("~/TerminalSetting/MapLines/GetStrucMapLines");
                        break;
                    default:
                        break;
                }

                BindCombox(rptName);
                base.SetReportViewer(this.rvResult);

            }
        }

        private void BindCombox(string rptName)
        {
            List<ExceptionTypeDDLModel> list = null;
            switch (rptName)
            {
                case "RegionAbnormalRpt"://区域异常
                    list = ExceptionTypeBLL.GetExTypes(3);
                    break;
                case "RouteAbnormalRpt"://路线异常
                    list = ExceptionTypeBLL.GetExTypes(4);
                    break;
                default:
                    break;
            }

            var allType = new ExceptionTypeDDLModel()
            {
                ID = -1,
                ExName = UIText.All
            };

            if (list == null)
            {
                list = new List<ExceptionTypeDDLModel>()
                {
                    allType
                };
            }
            else
            {
                list.Insert(0, allType);
            }

            ddlExTypes.DataSource = list;
            ddlExTypes.DataValueField = "ID";
            ddlExTypes.DataTextField = "ExName";
            ddlExTypes.DataBind();


        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            var startTime = DateTime.Parse(hidStartTime.Value);
            var endTime = DateTime.Parse(hidEndTime.Value);
            if (!base.CheckInput(startTime, endTime, ltCheckInfo))
            {
                return;
            }

            int vid = 0;
            if (!string.IsNullOrWhiteSpace(this.hidVehicleID.Value))
            {
                vid = Convert.ToInt32(hidVehicleID.Value);
            }

            int exTypeID = Convert.ToInt32(this.ddlExTypes.SelectedValue);


            try
            {
                var rptName = base.CurrentRptName;
                string jsonResult = string.Empty;
                switch (rptName)
                {
                    case "RegionAbnormalRpt"://区域异常
                        jsonResult = new HistoricalDataNS.HistoricalDataSoapClient().GetExceptionsForMapRegion(vid, startTime, endTime, exTypeID, "", base.CurrentUserID);
                        break;
                    case "RouteAbnormalRpt"://路线异常
                        jsonResult = new HistoricalDataNS.HistoricalDataSoapClient().GetExceptionsForMapLine(vid, startTime, endTime, exTypeID, 1, base.CurrentUserID);
                        break;
                }

                if (!base.CheckResult(jsonResult, ltCheckInfo))
                {
                    return;
                }
                var datas = JsonHelper.ConvertToList<RptForm5Model>(jsonResult);
                this.ShowReport(datas);
            }
            catch (Exception ex)
            {
                base.DoWebServiceLog("RptForm5", ex, ltCheckInfo);
            }

        }

        private void ShowReport(List<RptForm5Model> datas)
        {
            this.rvResult.ZoomMode = ZoomMode.PageWidth;
            var rpt = this.rvResult.LocalReport;
            rpt.DataSources.Clear();


            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/Rpt5.rdlc");

            var rds = new ReportDataSource();
            rds.Name = "Datas";
            rds.Value = datas;
            rpt.DataSources.Add(rds);


            string queryConditions = string.Empty;
            string vn = hidVehicleName.Value.Trim();
            if (string.IsNullOrWhiteSpace(vn))
            {
                queryConditions =
string.Format("{0}：{1} ~ {2}",
DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
            }
            else
            {
                queryConditions =
    string.Format("{0}：{1}\t\t{2}：{3} ~ {4}",
    DisplayText.VehicleName, vn,
    DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
            }


            queryConditions = string.Format("{0}\t\t{1}：{2}", queryConditions, DisplayText.ExceptionTypeName, ddlExTypes.SelectedItem.Text);


            if (!string.IsNullOrWhiteSpace(hidLineRegionName.Value))
            {
                queryConditions = string.Format("{0}\t\t{1}：{2}", queryConditions, lblLineRegionName.Text, hidLineRegionName.Value);
            }




            var lineRegionName = string.Empty;
            var rptName = base.CurrentRptName;
            switch (rptName)
            {
                case "RegionAbnormalRpt"://区域异常
                    lineRegionName = DisplayText.RegionsName;
                    break;
                case "RouteAbnormalRpt"://路线异常
                    lineRegionName = DisplayText.LinesName;
                    break;
                default:
                    break;
            }



            List<ReportParameter> rps = new List<ReportParameter>()
            {
                new ReportParameter("RowNo",DisplayText.RowNo),

                new ReportParameter("StrucName",DisplayText.StrucWhichUseVehicle),
                new ReportParameter("VehicleName",DisplayText.VehicleName),
                new ReportParameter("StartDateTime",DisplayText.StartTime),
                new ReportParameter("EndDateTime",DisplayText.EndTime),
                new ReportParameter("Duration",DisplayText.Duration),
                new ReportParameter("ExTypeName",DisplayText.ExceptionTypeName),
                new ReportParameter("LineRegionName",lineRegionName),
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("QueryConditions",queryConditions),
            };
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }


        private List<RptForm5Model> GetDatas()
        {
            List<RptForm5Model> list = new List<RptForm5Model>();
            list.Add(new RptForm5Model()
            {
                StrucName = "单位1",
                VehicleName = "代号3",
                StartDateTime = DateTime.Now.AddMinutes(-10),
                EndDateTime = DateTime.Now,
                ExTypeName = "GNSS天线未接或被剪断",
                LineRegionName = "路线1"
            });
            list.Add(new RptForm5Model()
            {
                StrucName = "单位1",
                VehicleName = "代号3",
                StartDateTime = DateTime.Now.AddDays(-2),
                EndDateTime = DateTime.Now,
                ExTypeName = "TTS模块故障",
                LineRegionName = "路线1"
            });
            list.Add(new RptForm5Model()
            {
                StrucName = "单位2",
                VehicleName = "代号2",
                StartDateTime = DateTime.Now.AddMinutes(-16),
                EndDateTime = DateTime.Now,
                ExTypeName = "道路运输证IC卡模块故障",
                LineRegionName = "路线1"
            });
            list.Add(new RptForm5Model()
            {
                StrucName = "单位1",
                VehicleName = "代号1",
                StartDateTime = DateTime.Now.AddMinutes(-50),
                EndDateTime = DateTime.Now,
                ExTypeName = "TTS模块故障",
                LineRegionName = "路线1"
            });
            return list;
        }

        private List<RptForm5Model> GetDatas2()
        {
            List<RptForm5Model> list = new List<RptForm5Model>();
            list.Add(new RptForm5Model()
            {
                StrucName = "单位1",
                VehicleName = "代号3",
                StartDateTime = DateTime.Now.AddMinutes(-10),
                EndDateTime = DateTime.Now,
                ExTypeName = "GNSS天线未接或被剪断",
                LineRegionName = "区域1"
            });
            list.Add(new RptForm5Model()
            {
                StrucName = "单位1",
                VehicleName = "代号3",
                StartDateTime = DateTime.Now.AddDays(-2),
                EndDateTime = DateTime.Now,
                ExTypeName = "TTS模块故障",
                LineRegionName = "区域1"
            });
            list.Add(new RptForm5Model()
            {
                StrucName = "单位2",
                VehicleName = "代号2",
                StartDateTime = DateTime.Now.AddMinutes(-16),
                EndDateTime = DateTime.Now,
                ExTypeName = "道路运输证IC卡模块故障",
                LineRegionName = "区域1"
            });
            list.Add(new RptForm5Model()
            {
                StrucName = "单位1",
                VehicleName = "代号1",
                StartDateTime = DateTime.Now.AddMinutes(-50),
                EndDateTime = DateTime.Now,
                ExTypeName = "TTS模块故障",
                LineRegionName = "区域1"
            });
            return list;
        }


    }


    /// <summary>
    /// 类型4报表数据模型
    /// </summary>
    public class RptForm5Model
    {
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExTypeName { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// 路线区域名称
        /// </summary>
        public string LineRegionName { get; set; }
    }


}
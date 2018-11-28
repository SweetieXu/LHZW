using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Resource;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Asiatek.TMS.ReportFiles.reports
{
    public partial class RptForm4 : RptBaseForm
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
                if (this.CurrentRptName == "ExDetailsSummaryRpt")//异常明细汇总报表不需要选择异常类型
                {
                    lblExTypeName.Visible = false;
                    ddlExTypes.Visible = false;
                }
                else
                {
                    lblExTypeName.Text = DisplayText.ExceptionTypeName + "：";
                    // 设置样式
                    lblExTypeName.Attributes.Add("style", " display: inline-block;max-width: 100%;margin-bottom: 5px; font-weight: 700;");
                    BindCombox();
                }
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
                vehiclesID = Convert.ToInt32(hidVehicleID.Value);
            }
            //这里的id是指异常类型表里的ID列而不是IndexID
            //因为对于设备以及电源异常都属于硬件异常，肯定是车机上报
            int exceptionTypeID = -1;
            if (ddlExTypes.Visible)
            {
                exceptionTypeID = Convert.ToInt32(this.ddlExTypes.SelectedValue);
            }

            try
            {
                int collectID = -1;
                ExceptionSearchModel model = new ExceptionSearchModel();
                List<ExceptionsEquipmentModel> list = null;
                model.UserID = base.CurrentUserID;
                model.SartTime = startTime;
                model.EndTime = endTime;
                model.VehiclesID = vehiclesID;

                // 异常明细汇总
                if (this.CurrentRptName == ExceptionCollectEnum.ExDetailsSummaryRpt.ToString())
                {
                    if (base.VehicleViewMode)
                    {
                        list = ReportBLL.GetDefaultAllExceptions(model,base.CurrentStrucID);
                    }
                    else
                    {
                        list = ReportBLL.GetAllExceptions(model);
                    }                 
                }
                else
                {
                    collectID = (int)Enum.Parse(typeof(ExceptionCollectEnum), base.CurrentRptName);
                    model.ExceptionTypeID = exceptionTypeID;
                    model.CollectID = collectID;
                     if (base.VehicleViewMode)
                    {
                        list = ReportBLL.GetDefaultExceptionsForEquipment(model,base.CurrentStrucID);
                    }
                    else
                    {
                       list = ReportBLL.GetExceptionsForEquipment(model);
                    }                 
                }

                if (!base.CheckResult<ExceptionsEquipmentModel>(list, ltCheckInfo))
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

        private void ShowReport(List<ExceptionsEquipmentModel> datas)
        {
            this.rvResult.ZoomMode = ZoomMode.PageWidth;
            var rpt = this.rvResult.LocalReport;
            rpt.DataSources.Clear();


            rpt.ReportPath = Server.MapPath("~/ReportFiles/rdlcs/Rpt4.rdlc");

            var rds = new ReportDataSource();
            rds.Name = "Datas";
            rds.Value = datas;
            rpt.DataSources.Add(rds);


            string queryConditions = string.Empty;
            string vn = hidVehicleName.Value.Trim();
            if (string.IsNullOrWhiteSpace(vn))
            {
                queryConditions =string.Format("{0}：{1} ~ {2}",DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
            }
            else
            {
                queryConditions =string.Format("{0}：{1}\t\t{2}：{3} ~ {4}",DisplayText.VehicleName, vn,
                                      DisplayText.TimeInterval, hidStartTime.Value.Trim(), hidEndTime.Value.Trim());
            }

            if (this.CurrentRptName != "ExDetailsSummaryRpt")//非异常明细汇总报表需要异常类型
            {
                queryConditions = string.Format("{0}\t\t{1}：{2}", queryConditions, DisplayText.ExceptionTypeName, ddlExTypes.SelectedItem.Text);
            }

            List<ReportParameter> rps = new List<ReportParameter>()
            {
                new ReportParameter("RowNo",DisplayText.RowNo),

                new ReportParameter("StrucName",DisplayText.StrucWhichUseVehicle),
                new ReportParameter("VehicleName",DisplayText.VehicleName),
                new ReportParameter("StartDateTime",DisplayText.StartTime),
                new ReportParameter("EndDateTime",DisplayText.EndTime),
                new ReportParameter("ExTypeName",DisplayText.ExceptionTypeName),
                new ReportParameter("RealReportName",base.CurrentRptRealName),
                new ReportParameter("QueryConditions",queryConditions),
                new ReportParameter("ActualDuration",DisplayText.ActualDuration),
                new ReportParameter("StartAddress",DisplayText.StartAddress),
                new ReportParameter("EndAddress",DisplayText.EndAddress),
            };
            rpt.SetParameters(rps);
            rpt.DisplayName = string.Format("{0}_{1}", base.CurrentRptRealName, DateTime.Now.ToString("yyyyMMddHHmmss"));

            rpt.Refresh();
        }

        #region 获取异常类型
        private void BindCombox()
        {
            var rptName = base.CurrentRptName;
            List<ExceptionTypeDDLModel> list = null;

            int collectID = (int)Enum.Parse(typeof(ExceptionCollectEnum), base.CurrentRptName);
            list = ExceptionTypeBLL.GetExTypes(collectID);

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
        #endregion
    }
}
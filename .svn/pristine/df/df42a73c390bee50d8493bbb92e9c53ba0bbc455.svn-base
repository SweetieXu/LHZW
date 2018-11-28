<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptForm5.aspx.cs" Inherits="Asiatek.TMS.ReportFiles.reports.RptForm5" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Content/jqueryui/jquery-ui.css" rel="stylesheet" />
    <link href="../../Content/jqueryui/jquery-ui.structure.css" rel="stylesheet" />
    <link href="../../Content/jqueryui/jquery-ui.theme.css" rel="stylesheet" />
    <link href="http://cdn.bootcss.com/bootstrap/3.3.5/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../../Scripts/jquery/jquery-2.2.3.js"></script>
    <script src="../../Scripts/jqueryui/jquery-ui-1.12.0.js"></script>
    <script src="../../Scripts/asiatekExtend/AsiatekjQueryExtend.js"></script>
    <script src="../../Scripts/jqueryuitimepicker/jquery-ui-timepicker-addon.js"></script>
    <script src="../../Scripts/initplugin/InitPlugin.js"></script>
    <script>

        var $hidVehicleName;
        var $hidVehicleID;
        var $hidStartTime;
        var $hidEndTime;


        var $txtVehicleName;
        var $txtStartTime;
        var $txtEndTime;


        var checkDateRange = function ()
        {
            var _startTime = new Date($txtStartTime.val());  //开始时间
            var _endTime = new Date($txtEndTime.val());    //结束时间

            if (_startTime > _endTime)
            {
                var _title = '<%=Asiatek.Resource.UIText.InformationTitle%>';
                var _close = '<%=Asiatek.Resource.UIText.Close%>';
                $.showPromptDialog('<%=Asiatek.Resource.DataAnnotations.StartTimeMoreThanEndTimeError%>', _title, _close);
                return false;
            }

            var _stYear = _startTime.getFullYear();
            var _stMonth = _startTime.getMonth();
            var _endYear = _endTime.getFullYear();
            var _endMonth = _endTime.getMonth();

            var _MMDiff = (_endYear - _stYear) * 12 + (_endMonth - _stMonth);
            if (_MMDiff >= 2)
            {
                var _title = '<%=Asiatek.Resource.UIText.InformationTitle%>';
                var _close = '<%=Asiatek.Resource.UIText.Close%>';
                $.showPromptDialog('<%=Asiatek.Resource.DataAnnotations.TimeRangeOver2Months%>', _title, _close);
                return false;
            }
            return true;
        }


        $(function ()
        {


            $hidVehicleName = $("#hidVehicleName");
            $hidVehicleID = $("#hidVehicleID");
            $hidStartTime = $("#hidStartTime");
            $hidEndTime = $("#hidEndTime");
            $hidDealUser = $("#hidDealUser");
            $hidLineRegionName = $("#hidLineRegionName");
            $hidLineRegionID = $("#hidLineRegionID");

            $txtVehicleName = $("#txtVehicleName");
            $txtStartTime = $("#txtStartTime");
            $txtEndTime = $("#txtEndTime");
            $txtDealUser = $("#txtDealUser");
            $txtLineRegionName = $("#txtLineRegionName");




            $txtStartTime.datetimepicker({
                changeMonth: true,
                changeYear: true,
                showSecond: true,
                timeText: '<%=Asiatek.Resource.UIText.Time%>',
                hourText: '<%=Asiatek.Resource.UIText.Hour%>',
                minuteText: '<%=Asiatek.Resource.UIText.Minute%>',
                secondText: '<%=Asiatek.Resource.UIText.Second%>',
                currentText: '<%=Asiatek.Resource.UIText.Now%>',
                closeText: '<%=Asiatek.Resource.UIText.Complete%>',
                showSecond: true, //显示秒  
                timeFormat: 'HH:mm:ss'//格式化时间
            });

            $txtEndTime.datetimepicker({
                changeMonth: true,
                changeYear: true,
                showSecond: true,
                timeText: '<%=Asiatek.Resource.UIText.Time%>',
                hourText: '<%=Asiatek.Resource.UIText.Hour%>',
                minuteText: '<%=Asiatek.Resource.UIText.Minute%>',
                secondText: '<%=Asiatek.Resource.UIText.Second%>',
                currentText: '<%=Asiatek.Resource.UIText.Now%>',
                closeText: '<%=Asiatek.Resource.UIText.Complete%>',
                showSecond: true, //显示秒  
                timeFormat: 'HH:mm:ss'//格式化时间
            });

            $txtStartTime.on("change", function ()
            {
                $hidStartTime.val($txtStartTime.val());
            });


            $txtEndTime.on("change", function ()
            {
                $hidEndTime.val($txtEndTime.val());
            });

            $txtDealUser.on("change", function ()
            {
                $hidDealUser.val($txtDealUser.val());
            });


            $txtVehicleName.on("blur", function ()
            {
                var val = $txtVehicleName.val();
                if (val == "" || val == undefined)
                {
                    $hidVehicleName.val("");
                    $hidVehicleID.val("");
                }
            });

            $txtLineRegionName.on("blur", function ()
            {
                var val = $txtLineRegionName.val();
                if (val == "" || val == undefined)
                {
                    $hidLineRegionID.val("");
                    $hidLineRegionName.val("");
                }
            });


            $txtVehicleName.autocomplete({
                delay: 500,
                minLength: 1,
                select: function (event, ui)
                {
                    $hidVehicleName.val(ui.item.value);
                    $hidVehicleID.val(ui.item.VID);
                },
                source: function (request, response)
                {
                    var realUrl = '<%=new UrlHelper(this.Request.RequestContext).Content("~/Home/GetUserVehiclesByVehicleName")%>';
                    $.get(realUrl, { vehicleName: request.term }, function (data, status, xhr)
                    {
                        if ($.handleAjaxError(data))
                        {
                            return;
                        }
                        response(data);
                    });
                }
            });


            $txtLineRegionName.autocomplete({
                delay: 500,
                minLength: 1,
                select: function (event, ui)
                {
                    $hidLineRegionName.val(ui.item.value);
                    $hidLineRegionID.val(ui.item.ID);
                },
                source: function (request, response)
                {
                    var realUrl = '<%=this.LineRegionNameGetUrl%>';
                    $.get(realUrl, { searchName: request.term }, function (data, status, xhr)
                    {
                        if ($.handleAjaxError(data))
                        {
                            return;
                        }
                        response(data);
                    });
                }
            });


        });
    </script>
</head>
<body>
    <form id="frmRpt" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        <label><%=Asiatek.Resource.DisplayText.VehicleName %>：</label>
                    </td>
                    <td>
                        <input id="txtVehicleName" type="text" name="txtVehicleName" value="" class="form-control" placeholder="<%=Asiatek.Resource.UIText.PleaseInput + Asiatek.Resource.DisplayText.VehicleName%>" />
                        <asp:HiddenField ID="hidVehicleName" runat="server" Value="" ClientIDMode="Static" />
                        <asp:HiddenField ID="hidVehicleID" runat="server" Value="" ClientIDMode="Static" />
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <asp:Label Text="" runat="server" ID="lblExTypeName" ClientIDMode="Static" />：
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlExTypes" ClientIDMode="Static" CssClass="form-control">
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <asp:Label Text="" runat="server" ID="lblLineRegionName" ClientIDMode="Static" />：
                    </td>
                    <td>
                        <input id="txtLineRegionName" type="text" name="txtLineRegionName" value="" class="form-control" placeholder="<%=Asiatek.Resource.UIText.PleaseInput + lblLineRegionName.Text%>" />
                        <asp:HiddenField ID="hidLineRegionName" runat="server" Value="" ClientIDMode="Static" />
                        <asp:HiddenField ID="hidLineRegionID" runat="server" Value="" ClientIDMode="Static" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label><%=Asiatek.Resource.DisplayText.TimeInterval %>：</label>
                    </td>
                    <td>
                        <input type="text" id="txtStartTime" name="txtStartTime" value="<%=this.StartTime %>" class="form-control" placeholder="<%=Asiatek.Resource.DisplayText.StartTime%>" />
                        <asp:HiddenField ID="hidStartTime" runat="server" Value="" ClientIDMode="Static" />
                    </td>
                    <td>~</td>
                    <td>
                        <input type="text" id="txtEndTime" name="txtEndTime" value="<%=this.EndTime %>" class="form-control" placeholder="<%=Asiatek.Resource.DisplayText.EndTime%>" />
                        <asp:HiddenField ID="hidEndTime" runat="server" Value="" ClientIDMode="Static" />
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" OnClientClick="return checkDateRange()" runat="server" OnClick="btnSearch_Click" ClientIDMode="Static" CssClass="btn btn-primary" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:ScriptManager ID="smRpt" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
            <asp:UpdatePanel ID="upRpt" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Literal ID="ltCheckInfo" runat="server"></asp:Literal>
                    <rsweb:ReportViewer Width="100%" Height="720px" ID="rvResult" runat="server" BackColor="Silver" ZoomMode="PageWidth" Font-Bold="True" Font-Italic="False" Font-Names="Verdana" Font-Size="Medium" ShowPrintButton="False" ShowRefreshButton="False"></rsweb:ReportViewer>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>

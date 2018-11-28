<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LHZWGateExceptionRpt.aspx.cs" Inherits="Asiatek.TMS.ReportFiles.reports.LHZWGateExceptionRpt" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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

        var $hidPlateNum;
        var $hidVehicleID;
        var $hidStartTime;
        var $hidEndTime;


        var $txtPlateNum;
        var $txtStartTime;
        var $txtEndTime;

        var canClick = function () {
            $("#btnClientSearch").prop("disabled", false);
            $("#btnClientSearch").text("<%=Asiatek.Resource.UIText.Search%>");
        }

        //按钮不可点击
        var notClick = function () {
            $("#btnClientSearch").text("<%=Asiatek.Resource.UIText.SearchLoding%>");
            $("#btnClientSearch").prop("disabled", true);
        }

        var checkDateRange = function () {
            var _startTime = new Date($txtStartTime.val());  //开始时间
            var _endTime = new Date($txtEndTime.val());    //结束时间

            if (_startTime > _endTime) {
                var _title = '<%=Asiatek.Resource.UIText.InformationTitle%>';
                var _close = '<%=Asiatek.Resource.UIText.Close%>';
                $.showPromptDialog('<%=Asiatek.Resource.DataAnnotations.StartTimeMoreThanEndTimeError%>', _title, _close);
                return false;
            }

            var startTime = _startTime.getTime();
            var endTime = _endTime.getTime();
            var dates = Math.abs((startTime - endTime)) / (1000 * 60 * 60 * 24);

            if (dates >= 15) {
                var _title = '<%=Asiatek.Resource.UIText.InformationTitle%>';
                var _close = '<%=Asiatek.Resource.UIText.Close%>';
                $.showPromptDialog('时间跨度不可以超过半个月', _title, _close);
                return false;
            }

            return true;
        }

        $(function () {

            $("#btnClientSearch").text("<%=Asiatek.Resource.UIText.Search%>");

            $("#btnClientSearch").click(function () {
                var result = checkDateRange();
                if (result) {
                    $("#btnSearch").click();
                    // 这个一定放在后面 否则直接无法触发  $("#btnSearch").click()事件
                    notClick();
                }
                else {
                    canClick();
                }
                return false;
            });


            $hidPlateNum = $("#hidPlateNum");
            $hidVehicleID = $("#hidVehicleID");
            $hidStartTime = $("#hidStartTime");
            $hidEndTime = $("#hidEndTime");


            $txtPlateNum = $("#txtPlateNum");
            $txtStartTime = $("#txtStartTime");
            $txtEndTime = $("#txtEndTime");


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
                minDate: '<%=DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd HH:mm:ss")%>',
                maxDate: '<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>',//最大日期为当前时间
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
                minDate: '<%=DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd HH:mm:ss")%>',
                maxDate: '<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>',//最大日期为当前时间
                showSecond: true, //显示秒  
                timeFormat: 'HH:mm:ss'//格式化时间
            });




            $txtStartTime.on("change", function () {
                $hidStartTime.val($txtStartTime.val());
            });


            $txtEndTime.on("change", function () {
                $hidEndTime.val($txtEndTime.val());
            });


            $txtPlateNum.on("blur", function () {
                var val = $txtPlateNum.val();
                if (val == "" || val == undefined) {
                    $hidPlateNum.val("");
                    $hidVehicleID.val("");
                }
            });


            $txtPlateNum.autocomplete({
                delay: 500,
                minLength: 1,
                select: function (event, ui) {
                    $hidPlateNum.val(ui.item.value);
                    $hidVehicleID.val(ui.item.VID);
                },
                source: function (request, response) {
                    <%
        //提取到公共控制器
        var realUrl = new UrlHelper(this.Request.RequestContext).Content("~/Common/GetUserVehiclesByPlateNum");
                    %>
                    var realUrl = '<%=realUrl%>';
                    $.get(realUrl, { plateNum: request.term }, function (data, status, xhr) {
                        if ($.handleAjaxError(data)) {
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
                        <label>车牌号：</label>
                    </td>
                    <td>
                        <input id="txtPlateNum" type="text" name="plateNum" value="" class="form-control" placeholder="<%=Asiatek.Resource.UIText.PleaseInput + Asiatek.Resource.DisplayText.PlateNum%>" />
                        <asp:HiddenField ID="hidPlateNum" runat="server" Value="" ClientIDMode="Static" />
                        <asp:HiddenField ID="hidVehicleID" runat="server" Value="" ClientIDMode="Static" />
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <label>异常类型：</label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlExTypes"  CssClass="form-control">
                            <asp:ListItem Selected="True" Value="0">门岗异常</asp:ListItem>
                            <asp:ListItem Value="1">电子围栏异常</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;</td>
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
                    <td>&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <button id="btnClientSearch" class="btn btn-primary"></button>
                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" ClientIDMode="Static" CssClass="btn btn-primary" />
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

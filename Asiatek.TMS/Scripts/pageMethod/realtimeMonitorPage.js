 ///----调整页面窗口大小----
var setTreeContentSize = function () {
    var tdLeftContent = document.getElementById("tdLeftInfoContent");//左侧边栏内容
    var container = document.getElementById("container");//地图
    var tdRightContent = document.getElementById("tdRightContent");
    var tdBottom = document.getElementById("tdBottomInfo").clientHeight;//右侧底部高度 
    //布局页面div
    var mainDiv = document.getElementById("mainDiv");
    var headDivHt = document.getElementById("headDiv").clientHeight;
    var menusDiv = document.getElementById("menusDiv");
    var menusDivHt = document.getElementById("menusDiv").clientHeight;
    var bottomDivHt = document.getElementById("bottomDiv").clientHeight;

    tdRightContent.style.height = window.innerHeight - headDivHt - menusDivHt - bottomDivHt + "px";
    container.style.height = window.innerHeight - headDivHt - menusDivHt - bottomDivHt - 6 - tdBottom + "px";
    tdLeftContent.style.height = container.clientHeight + tdBottom - 100 + "px";
    mainDiv.style.width = window.innerWidth + "px";
    menusDiv.style.width = window.innerWidth + "px";
}


var ruler;      //测距
var markList = new Object();     //存储Mark对象(map.js中new Mark(....)) 对应vid
var showInfoTag = 0;     //标记当前打开的infowindow显示的tag页，目前是：1-第一页，2-第二页
var mousetool;     //鼠标工具，用于在地图上绘制图形
var rulerBtnFlag = 0;     //标识地图上测距按钮 0关闭 1开启
var rectBtnFlag = 0;        //标识地图上矩形区域查车按钮 0关闭 1开启
var circleBtnFlag = 0;        //标识地图上圆形区域查车按钮 0关闭 1开启
var polygonBtnFlag = 0;      //标识地图上圆形区域查车按钮 0关闭 1开启
var hasAreaRtFlag = 0;         //地图上是否有区域查车结果
var showEFMarkerBtnFlag = 0;     //标识查询电子围栏按钮

//定义map对象
var mapObj = initMap('container', true, 12, null);
//添加标尺、鹰眼、比例尺
addControl(true, true, false, true, false, mapObj);
//图层切换控件
showLayerSwitcher(true, mapObj);


//******地址搜索【start】******
//输入提示
var autoOptions = {
    input: "txtAddress"
};
var auto = new AMap.Autocomplete(autoOptions);
var placeSearch = new AMap.PlaceSearch({
    map: mapObj
});  //构造地点查询类
AMap.event.addListener(auto, "select", select);//注册监听，当选中某条记录时会触发
function select(e) {
    placeSearch.setCity(e.poi.adcode);
    placeSearch.search(e.poi.name);  //关键字查询查询
}
//******地址搜索【end】******


////--------显示图标marker、信息窗体infowindow相关【start】-------- 
////（信息窗体内容加载在Index页面）
//在地图上显示指定的车辆图标，加载前先判断，初次加载时添加，刷新时更新车辆信息
function showMarkers(result) {
    var Marker;
    var image = getMarkerIcon(result.Icon, result.Angle, result.IsOnline, result.IsRunning);
    var infowindow;

    //如果已经选中就更新状态信息
    if (markList[result.VID]) {
        Marker = markList[result.VID];
        infowindow = Marker.getInfoWindow();
        //根据当前显示的信息加载内容
        if (showInfoTag == 1) {
            infowindow.setContent(getPositionInfo(result).join("<br/>"));
        }
        if (showInfoTag == 2) {
            infowindow.setContent(getBaseInfo(result).join("<br/>"));
        }
        //更新carMarker信息时，需要判断当前infowindow是否打开的状态，将其作为参数传递到后台
        markList[result.VID] = updateMarker(mapObj, Marker, image, infowindow, result);
    }
        //添加新的marker
    else {
        infowindow = SetInfoWindow(result);         //初始化信息窗体
        markList[result.VID] = addMarker(mapObj, image, new AMap.Size(30, 30), infowindow, result);
        //将点击事件放到map.js外面来,是为了改变 showInfoTag值和显示tag页面
        //有一种情况：一个marker显示在showInfoTag=2的页面，然后点击显示另一个marker，刷新时需要显示第一个tag页面，
        //所以要在marker的点击事件中修改showInfoTag值并调用显示第一个tag页面
        if (infowindow != null) {
            //鼠标点击marker弹出自定义的信息窗体
            AMap.event.addListener(markList[result.VID].getMarker(), 'click', function () {
                infowindow.open(mapObj, markList[result.VID].getMarker().getPosition());
                showInfoTag = 1;
            });
        }
        mapObj.setFitView();
    }
}

//构建信息窗体（初始化时）  包含定位信息
function SetInfoWindow(result) {
    var info = getPositionInfo(result);
    //自定义信息窗体
    var infoWindow = new AMap.InfoWindow({
        content: info.join("<br/>"),  //使用默认信息窗体框样式，显示信息内容
        offset: new AMap.Pixel(1, -10)
        //isCustom: true,
        //showShadow: true    
    });
    return infoWindow;
}

//点击切换信息窗体的内容，显示定位信息
function ptSpanFun(result) {
    showInfoTag = 1;
    changeInfoWindow(result);
}

//点击切换信息窗体的内容，显示基本信息
function baseSpanFun(result) {
    showInfoTag = 2;
    changeInfoWindow(result);
}

//点击切换infoWindow内容时，加载显示不同内容
function changeInfoWindow(result) {
    if (markList[result.VID]) {
        var Marker = markList[result.VID];
        var infowindow = Marker.getInfoWindow();
        var image = getMarkerIcon(result.Icon, result.Angle, result.IsOnline, result.IsRunning);
        if (showInfoTag == 1) {
            infowindow.setContent(getPositionInfo(result).join("<br/>"));
        }
        if (showInfoTag == 2) {
            infowindow.setContent(getBaseInfo(result).join("<br/>"));
        }
        //更新carMarker信息时，需要判断当前infowindow是否打开的状态，将其作为参数传递到后台
        markList[result.VID] = updateMarker(mapObj, Marker, image, infowindow, result);
    }
}

//获取不同状态（行驶、停车、掉线），不同方向的车辆图标
function getMarkerIcon(icon, angle, isOnline, isRunning) {
    var image = "../../Content/vehicleIcons/" + icon;
    //掉线
    if (isOnline != true) {
        if (angle > 22 && angle <= 67) {
            image += "/Offline45.png";
        }
        else if (angle > 67 && angle <= 112) {
            image += "/Offline90.png";
        }
        else if (angle > 112 && angle <= 157) {
            image += "/Offline135.png";
        }
        else if (angle > 157 && angle <= 202) {
            image += "/Offline180.png";
        }
        else if (angle > 202 && angle <= 247) {
            image += "/Offline225.png";
        }
        else if (angle > 247 && angle <= 292) {
            image += "/Offline270.png";
        }
        else if (angle > 292 && angle <= 337) {
            image += "/Offline315.png";
        }
        else {
            image += "/Offline0.png";
        }
    }
    //行驶
    if (isOnline == true && isRunning == true) {
        if (angle > 22 && angle <= 67) {
            image += "/Running45.png";
        }
        else if (angle > 67 && angle <= 112) {
            image += "/Running90.png";
        }
        else if (angle > 112 && angle <= 157) {
            image += "/Running135.png";
        }
        else if (angle > 157 && angle <= 202) {
            image += "/Running180.png";
        }
        else if (angle > 202 && angle <= 247) {
            image += "/Running225.png";
        }
        else if (angle > 247 && angle <= 292) {
            image += "/Running270.png";
        }
        else if (angle > 292 && angle <= 337) {
            image += "/Running315.png";
        }
        else {
            image += "/Running0.png";
        }
    }
    //停车
    if (isOnline == true && isRunning != true) {
        if (angle > 22 && angle <= 67) {
            image += "/Stop45.png";
        }
        else if (angle > 67 && angle <= 112) {
            image += "/Stop90.png";
        }
        else if (angle > 112 && angle <= 157) {
            image += "/Stop135.png";
        }
        else if (angle > 157 && angle <= 202) {
            image += "/Stop180.png";
        }
        else if (angle > 202 && angle <= 247) {
            image += "/Stop225.png";
        }
        else if (angle > 247 && angle <= 292) {
            image += "/Stop270.png";
        }
        else if (angle > 292 && angle <= 337) {
            image += "/Stop315.png";
        }
        else {
            image += "/Stop0.png";
        }
    }
    return image;
}

//取消选中点marker
var removeMarker = function (vid) {
    var data = markList[vid];
    if (data != null) {
        mapObj.clearInfoWindow();//关闭打开的infowindow
        data.marker.setMap(null);
        //修改地图的缩放级别
        mapObj.setZoom(12);
        delete markList[vid];
    }
    //自适应
    mapObj.setFitView();
}

//取消所有marker
var removeAllMarker = function () {
    for (markerId in markList) {
        if (markList.hasOwnProperty(markerId)) {
            //从下方监控车辆表格移除车辆信息
            removeVehicleInfoFromBottomTable(markerId);
            markList[markerId] = null;
            delete markList[markerId];
        }
    }
    //清空左侧车辆信息
    initLeftInfo();
    //自适应
    mapObj.setFitView();
}
////--------显示图标marker、信息窗体infowindow相关【end】--------


////--------距离测量相关【start】--------
// 距离测量   测距事件完成后自动显示未选中的测距图标
var displayObj = document.getElementById("btnGrouprulerImg");
var hideObj = document.getElementById("btnGrouprulerSelImg");
addRangingTool(mapObj, displayObj, hideObj);

//启用默认样式测距   绘制完成后恢复到待绘制状态
function startRuler() {
    claerMapBeforeMouse();     //清理地图
    removeBtnGroupImgsClass();       //清除右上角图标选中样式
    rectBtnFlag = 0;
    polygonBtnFlag = 0;
    circleBtnFlag = 0;
    showEFMarkerBtnFlag = 0;
    if (rulerBtnFlag == 0) {
        document.getElementById("btnGrouprulerSelImg").style.display = "";
        document.getElementById("btnGrouprulerImg").style.display = "none";
        rulerBtnFlag = 1;
    }
    else {
        document.getElementById("btnGrouprulerSelImg").style.display = "none";
        document.getElementById("btnGrouprulerImg").style.display = "";
        rulerBtnFlag = 0;
    }
    if (rulerBtnFlag == 1) {
        if (mousetool) {
            mousetool.close(true);
        }
        mapObj.clearMap();
        ruler.turnOn();
    }
}
////--------距离测量相关【end】--------


////--------区域查车（矩形、圆形、多边形）相关【start】--------
////包括区域绘制，区域查车，区域查询车辆结果在右下方显示
//在鼠标绘制矩形、圆形、多边形之前清理
function claerMapBeforeMouse() {
    mapObj.clearMap();
    ruler.turnOff();
    removeAllMarker();
    $tbIntraScopeVehicles.find("tbody").empty();
}

//有区域查车结果，先清除
function clearAreaResult(){
    if (hasAreaRtFlag == 1) {             //有区域查车结果，先清除，再显示车辆
        mapObj.clearMap();
        clearBtmVehiclesSearchInfo();
        hasAreaRtFlag = 0;
    }
}

//鼠标绘制矩形
function mouseRect() {
    //清理
    claerMapBeforeMouse();
    removeBtnGroupImgsClass();
    rulerBtnFlag = 0;
    polygonBtnFlag = 0;
    circleBtnFlag = 0;
    showEFMarkerBtnFlag = 0;

    if (rectBtnFlag == 0) {
        document.getElementById("btnGroupRectSelImg").style.display = "";
        document.getElementById("btnGroupRectImg").style.display = "none";
        rectBtnFlag = 1;
    }
    else {
        document.getElementById("btnGroupRectSelImg").style.display = "none";
        document.getElementById("btnGroupRectImg").style.display = "";
        rectBtnFlag = 0;
    }

    if (mousetool) {
        mousetool.close(true);
    }
    if (rectBtnFlag == 1) {
        mapObj.plugin(["AMap.MouseTool"], function () {
            mousetool = new AMap.MouseTool(mapObj);
            mousetool.rectangle(); //使用鼠标工具，在地图上画矩形
            AMap.event.addListener(mousetool, "draw", function (e) {
                var point = e.obj.getPath();
                getRealTimeRectangleVehicles(point);
                mousetool.close(false);//false:鼠标操作关闭,保留所绘制的覆盖物对象;true:鼠标操作关闭,清除覆盖物对象
                hasAreaRtFlag = 1;
            });
        });
    }
}

//鼠标绘制圆形
function mouseCircle() {
    //清理
    claerMapBeforeMouse();
    removeBtnGroupImgsClass();
    rulerBtnFlag = 0;
    rectBtnFlag = 0;
    polygonBtnFlag = 0;
    showEFMarkerBtnFlag = 0;

    if (circleBtnFlag == 0) {
        document.getElementById("btnGroupCircleSelImg").style.display = "";
        document.getElementById("btnGroupCircleImg").style.display = "none";
        circleBtnFlag = 1;
    }
    else {
        document.getElementById("btnGroupCircleSelImg").style.display = "none";
        document.getElementById("btnGroupCircleImg").style.display = "";
        circleBtnFlag = 0;
    }

    if (mousetool) {
        mousetool.close(true);
    }
    if (circleBtnFlag == 1) {
        mapObj.plugin(["AMap.MouseTool"], function () {
            mousetool = new AMap.MouseTool(mapObj);
            mousetool.circle(); //圆形
            AMap.event.addListener(mousetool, "draw", function (e) {
                var radius = e.obj.getRadius();
                var center = e.obj.getCenter();
                getRealTimeCircleVehicles(radius, center);
                mousetool.close(false);
                hasAreaRtFlag = 1;
            });
        });
        circleBtnFlag == 0;
    }
}

//鼠标绘制多边形
function mousePolygon() {
    //清理
    claerMapBeforeMouse();
    removeBtnGroupImgsClass();
    rulerBtnFlag = 0;
    rectBtnFlag = 0;
    circleBtnFlag = 0;
    showEFMarkerBtnFlag = 0;

    if (polygonBtnFlag == 0) {
        document.getElementById("btnGroupPolygonSelImg").style.display = "";
        document.getElementById("btnGroupPolygonImg").style.display = "none";
        polygonBtnFlag = 1;
    }
    else {
        document.getElementById("btnGroupPolygonSelImg").style.display = "none";
        document.getElementById("btnGroupPolygonImg").style.display = "";
        polygonBtnFlag = 0;
    }

    if (mousetool) {
        mousetool.close(true);
    }
    if (polygonBtnFlag == 1) {
        mapObj.plugin(["AMap.MouseTool"], function () {
            mousetool = new AMap.MouseTool(mapObj);
            mousetool.polygon(); //多边形
            AMap.event.addListener(mousetool, "draw", function (e) {
                var point = e.obj.getPath();
                getRealTimePolygonVehicles(point);
                mousetool.close(false);
                hasAreaRtFlag = 1;
            });
        });
    }
}

//清理下方区域查车信息
function clearBtmVehiclesSearchInfo() {
    $tbIntraScopeVehicles.find("tbody").empty();
    $tbIntraScopeVehicles.find("span").text("");
}

//矩形区域查车
var getRealTimeRectangleVehicles = function (point) {
    var data = { Lngmax: point[1].lng, Latmax: point[1].lat, Lngmin: point[3].lng, Latmin: point[3].lat };
    $.post("../../Home/GetRealTimeRectangleVehicles", data, function (result) {
        if ($.handleAjaxError(result)) {
            return;
        }
        //清理
        clearBtmVehiclesSearchInfo();
        $.each(result, function (i, v) {
            buildRealTimeScopeVehicles(v);
            showMarkers(v);
        });
    });
};

//圆形区域查车
var getRealTimeCircleVehicles = function (radius, center) {
    var data = { radius: radius, lng: center.lng, lat: center.lat };
    $.post("../../Home/GetRealTimeCircleVehicles", data, function (result) {
        if ($.handleAjaxError(result)) {
            return;
        }
        //清理
        clearBtmVehiclesSearchInfo();
        $.each(result, function (i, v) {
            buildRealTimeScopeVehicles(v);
            showMarkers(v);
        });
    })
}

//多边形区域查车
var getRealTimePolygonVehicles = function (point) {
    var data = JSON.stringify(point);
    //用高德自带的判断点是否在多边形内的方法
    var polygon = new AMap.Polygon({
        map: mapObj,
        path: point,
        fillOpacity: 0.2,
        fillColor: "lightblue", //填充色
        strokeWeight: 1,
        strokeColor: "lightblue" //线颜色
    });
    $.post("../../Home/GetRealTimePolygonVehicles", { points: data }, function (result) {
        if ($.handleAjaxError(result)) {
            return;
        }
        //清理
        clearBtmVehiclesSearchInfo();
        $.each(result, function (i, v) {
            if (polygon.contains([v.Longitude, v.Latitude])) {
                buildRealTimeScopeVehicles(v);
                showMarkers(v);
            }
        });
    })
}

//添加区域车辆信息
var buildRealTimeScopeVehicles = function (result) {
    //调用index界面获取的资源文件信息
    var text = getDisplayText();
    var _$tbody = $tbIntraScopeVehicles.find("tbody");
    var _$tr = $("<tr></tr>");
    _$tr.append($("<td></td>").text(result.VN));
    _$tr.append($("<td></td>").text(result.SN));
    _$tr.append($("<td></td>").text(result.Address));
    _$tr.append($("<td></td>").text(result.Speed));
    _$tr.append($("<td></td>").text(result.Direction));
    if (result.IsOnline == 1) {
        _$tr.append($("<td></td>").text(text[0]));
    }
    else {
        _$tr.append($("<td></td>").text(text[1]));
    }
    _$tr.append($("<td></td>").text(result.SignalTime));
    _$tr.append($("<td></td>").text(result.Temperature));
    _$tbody.append(_$tr);
    $tbIntraScopeVehicles.find("span").text("(" + $tbIntraScopeVehicles.find("tbody>tr").length + ")");
}
////--------区域查车（矩形、圆形、多边形）相关【end】--------


////--------电子围栏信息获取、绘制加载相关【start】--------
//加载权限范围内的电子围栏
function ShowEFMarker() {
    mapObj.remove(mapObj.getAllOverlays('SvgMarker'));  //移除地图上的marker
    //显示电子围栏时会清除地图上的区域查车结果，所以这边修改区域查车结果为0，同时要清除右下方区域查车结果
    //这边不修改hasAreaRtFlag值的话，这种情况：先选择区域查车，然后显示电子围栏，再选中车辆。这时清除区域查车结果时候会将电子围栏数据连带清除掉
    hasAreaRtFlag = 0;
    $tbIntraScopeVehicles.find("tbody").empty();     //清除右下方区域查车结果
    refreshCheckedVehicleInfo();   //刷新车辆，显示车辆图标
    removeBtnGroupImgsClass();
    rulerBtnFlag = 0;
    rectBtnFlag = 0;
    polygonBtnFlag = 0;
    circleBtnFlag = 0;

    if (showEFMarkerBtnFlag == 0) {
        document.getElementById("btnGroupMarkerSelImg").style.display = "";
        document.getElementById("btnGroupMarkerImg").style.display = "none";
        showEFMarkerBtnFlag = 1;
    }
    else {
        document.getElementById("btnGroupMarkerSelImg").style.display = "none";
        document.getElementById("btnGroupMarkerImg").style.display = "";
        showEFMarkerBtnFlag = 0;
    }
    if (mousetool) {
        mousetool.close(true);
    }
    if (showEFMarkerBtnFlag == 1) {
        $.ajax({
            type: "GET",
            //url: "@Url.Content("~/Home/GetEFMarkersInfo")",
            url: "../../Home/GetEFMarkersInfo",
            success: function (result) {
                if ($.handleAjaxError(result)) { return; }
                if (result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        createElectricFence(result[i], mapObj);
                    }
                }
                else {
                    var _message = getDataAnnotations();    //获取Index页面定义的资源文件
                    $.showPromptDialog(_message, null, null, null, false);
                }
            }
        });
    }
}

//根据电子围栏信息在地图上绘制不同形状、不同颜色的电子围栏
function createElectricFence(result, mapObj) {
    //存放图形中间点坐标，显示电子围栏名称
    var lngLat;
    //填充色、边框默认蓝色
    var fillColor = "#0D9BF2";
    var strokeColor = "#0D9BF2";
    var fontColor = "#000";     //字体颜色默认黑色
    if (result.CustomerStatus == 1)        //在用客户  绿色显示
    {
        fillColor = "#00ff00";
        strokeColor = "#0bc30b";
        fontColor = "#0bc30b";
    }
    if (result.CustomerStatus == 2)        //潜在客户  红色显示
    {
        fillColor = "#F33";
        strokeColor = "#ee2200";
        fontColor = "#ee2200";
    }
    if (result.CustomerStatus == 3)        //不在用客户  灰色显示
    {
        fillColor = "#666666";
        strokeColor = "#434343";
        fontColor = "#434343";
    }
    //圆形
    if (result.FenceType == 1) {
        var circleInfo = result.FenceTypeInfo.split(';');
        var center = circleInfo[0].split(',');
        var centerlng = center[0];
        var centerlat = center[1];
        var circle = new AMap.Circle({
            map: mapObj,
            center: [centerlng, centerlat],
            radius: circleInfo[1],
            fillOpacity: 0.3,
            fillColor: fillColor,
            strokeWeight: 2,
            strokeColor: strokeColor
        });
        lngLat = [centerlng, centerlat];       //显示在圆心
    }
    //矩形
    if (result.FenceType == 2) {
        var rectInfo = result.FenceTypeInfo.split(';');
        var rectArr = new Array();//多边形覆盖物节点坐标数组
        for (var i = 0; i < rectInfo.length; i++) {
            var point = rectInfo[i].split(',');
            var lng = point[0];
            var lat = point[1];
            rectArr.push([lng, lat]);
        }
        var rect = new AMap.Polygon({
            map: mapObj,
            path: rectArr,
            fillOpacity: 0.3,
            fillColor: fillColor,
            strokeWeight: 2,
            strokeColor: strokeColor
        });
        lngLat = rect.getBounds().getCenter();       //显示在矩形中间
    }
    //多边形
    if (result.FenceType == 3) {
        var polygonInfo = result.FenceTypeInfo.split(';');
        var polygonArr = new Array();//多边形覆盖物节点坐标数组
        for (var i = 0; i < polygonInfo.length; i++) {
            var point = polygonInfo[i].split(',');
            var lng = point[0];
            var lat = point[1];
            polygonArr.push([lng, lat]);
        }
        var polygon = new AMap.Polygon({
            map: mapObj,
            path: polygonArr,
            fillOpacity: 0.3,
            fillColor: fillColor,
            strokeWeight: 2,
            strokeColor: strokeColor
        });
        lngLat = polygon.getBounds().getCenter();       //显示在多边形外切矩形中间
    }
    mapObj.setFitView();

    //加载SvgMarker  显示电子围栏图标和名称
    AMapUI.loadUI(['overlay/SvgMarker'], function (SvgMarker) {
        if (!SvgMarker.supportSvg) {
            //当前环境并不支持SVG，此时SvgMarker会回退到父类，即SimpleMarker
            alert('当前环境不支持SVG');
        }
        //SvgMarker.Shape下的Shape 
        var shapeKeys = ['TriangleFlagPin'];
        var markers = [];
        //创建shape
        var shape = new SvgMarker.Shape[shapeKeys[0]]({
            height: 24 * (1 + 0.3),
            strokeWidth: 1,
            strokeColor: '#d62728',
            fillColor: '#d62728'
        });
        var labelCenter = shape.getCenter();
        markers.push(new SvgMarker(
            shape, {
                map: mapObj,
                position: lngLat,
                containerClassNames: 'shape-' + shapeKeys[0],
                iconLabel: {
                    innerHTML: '<div style="white-space:nowrap; border:1px solid black; display:inline-block; background:#FFF;">' + result.FenceName + '</div>',           //用style="white-space:nowrap;"样式设置中文文字不换行
                    style: {
                        top: labelCenter[0] - 9 + 'px',
                        left: labelCenter[0] + 2 + 'px',
                        color: fontColor, //设置字体颜色
                        fontSize: '14px', //设置字号
                        fontWeight: '500'
                    }
                },
                showPositionPoint: false
            }));
    });
}
////--------电子围栏信息获取、绘制加载相关【end】--------


////--------页面中图片按钮加载切换相关【start】--------
//左侧按钮图片状态切换：左侧车辆树状图上方显示的 全部、行驶、停车、掉线 车辆图片按钮之间的选中切换
function vehicleChangeImg(e) {
    removeLeftVehicleBtnImgsClass();        //先全部去掉选中状态，再根据点击的按钮添加选中状态
    if (e.name == "btnAllVehicles") {
        document.getElementById("allImg").style.display = "none";
        document.getElementById("allSelImg").style.display = "";
    }
    if (e.name == "btnRunningVehicles") {
        document.getElementById("runningImg").style.display = "none";
        document.getElementById("runningSelImg").style.display = "";
    }
    if (e.name == "btnStopVehicles") {
        document.getElementById("stopImg").style.display = "none";
        document.getElementById("stopSelImg").style.display = "";
    }
    if (e.name == "btnOfflineVehicles") {
        document.getElementById("offlineImg").style.display = "none";
        document.getElementById("offlineSelImg").style.display = "";
    }
}

//左侧全部、行驶、停车、掉线图片按钮  全部去掉选中状态
function removeLeftVehicleBtnImgsClass() {
    var btnVehicleImgs = document.getElementsByName("btnVehicleImg");
    for (var i = 0; i < btnVehicleImgs.length; i++) {
        btnVehicleImgs[i].style.display = "";
    }
    var btnVehicleSelImgs = document.getElementsByName("btnVehicleSelImg");
    for (var i = 0; i < btnVehicleSelImgs.length; i++) {
        btnVehicleSelImgs[i].style.display = "none";
    }
}

//地图右上角各形状的区域查车、电子围栏、测距图片按钮  全部去掉选中样式
function removeBtnGroupImgsClass() {
    var btnGroupImgs = document.getElementsByName("btnGroupImg");
    for (var i = 0; i < btnGroupImgs.length; i++) {
        btnGroupImgs[i].style.display = "";
    }
    var btnGroupSelImgs = document.getElementsByName("btnGroupSelImg");
    for (var i = 0; i < btnGroupSelImgs.length; i++) {
        btnGroupSelImgs[i].style.display = "none";
    }
}
////--------页面中图片按钮加载切换相关【end】--------
////----调整页面窗口的大小----
///参数，IsAlarmShowOrSignalsList：显示信号列表按钮状态；IsShowPressureList：显示气压卸料信号列表按钮状态；IsShowStopContentList：显示停车点列表
///根据右下方显示不同信息列表获取元素高度
function setContentSize(IsAlarmShowOrSignalsList, IsShowPressureList, IsShowStopContentList) {
    var tdBottom;
    var height = document.documentElement.scrollHeight;
    var container = document.getElementById("container");//地图
    var tdRightContent = document.getElementById("tdRightContent");
    var tdLeftDivInfo = document.getElementById("tdLeftDivInfo");//左侧边栏div
    //var tdBottom = document.getElementById("tdBottomInfo").clientHeight;//右侧底部高度 
    if (IsAlarmShowOrSignalsList == true) {
        tdBottom = document.getElementById("tdBottomSignalsInfo").clientHeight;//右侧底部高度 
    } else if (IsShowPressureList == true) {
        tdBottom = document.getElementById("tdBottomPressureInfo").clientHeight;
    } else if (IsShowStopContentList == true) {
        tdBottom = document.getElementById("tdBottomStopContentInfo").clientHeight;
    } else {
        tdBottom = document.getElementById("tdBottomInfo").clientHeight;
    }
    //布局页面div
    var mainDiv = document.getElementById("mainDiv");
    var headDivHt = document.getElementById("headDiv").clientHeight;
    var menusDiv = document.getElementById("menusDiv");
    var menusDivHt = document.getElementById("menusDiv").clientHeight;
    var bottomDivHt = document.getElementById("bottomDiv").clientHeight;

    tdRightContent.style.height = window.innerHeight - headDivHt - menusDivHt - bottomDivHt + "px";
    if (window.innerHeight < 550) {
        container.style.height = height - headDivHt - menusDivHt - bottomDivHt - 6 - tdBottom + "px";
    } else {
        container.style.height = window.innerHeight - headDivHt - menusDivHt - bottomDivHt - 6 - tdBottom + "px";
    }
    tdLeftDivInfo.style.height = tdRightContent.style.height;

    mainDiv.style.width = window.innerWidth + "px";
    menusDiv.style.width = window.innerWidth + "px";
}


////----存储解析的地址，显示信号列表时，一方面滚动加载信号信息，同时对于相同的地址信息采用一次填充方式----
//存储经纬度对应的地址
var  lngandlataddress = new Map();
var tempMap = new Map();
var m1 = new Map();
var a="0||0";
var b;
//buildSignalsContentWithDiv方法中要用到的参数，buildContentTextArray：右下方显示信号列表时候@DisplayText要显示的所有文字数组
function buildSearchResultSignalsContentWithDiv(begin, end, searchResult, buildContentTextArray) {
    AMap.service('AMap.Geocoder', function () {//回调函数
        //实例化Geocoder
        var geocoder = new AMap.Geocoder({
            city: "010"//城市，默认：“全国”
        });
        if (end > searchResult.length) {
            end = searchResult.length;
        }
        for (var i = begin; i < end; i++) {
            var currentItem = searchResult[i];
            //先加载除了地址以外的模块
            buildSignalsContentWithDiv(currentItem, i, buildContentTextArray);
            //最后一笔信号速度为0的时候要去检查
            if (i == searchResult.length - 1 && searchResult[i - 1].Speed == 0) {
                m1.forEach(function (value, Index) {
                    if (Index > a.split("||")[0]) {
                        a = Index + "||" + value;
                    }
                });
                //只有2个信号并且都是速度为0 第二个信号直接给高德解析
                if (i == 1) {
                    var lng = [currentItem.Longitude, currentItem.Latitude];
                    getAddress(i, currentItem, lng, geocoder, null);
                } else {
                    tempMap.set(i, currentItem);
                    tempMap.forEach(function (prevItem, prevPointIndex) {
                        prevItem.Address = a.split(a.split("||")[1]);
                        $("#ul" + prevPointIndex + "").append(" <li class='table-cell2'>" + prevItem.Address + " </li>");
                    });
                }
                continue;
            }
            //过滤掉连续2个速度都是0的点
            if (i > 0 && currentItem.Speed == 0 && searchResult[i - 1].Speed == 0) {
                tempMap.set(i, currentItem);
                continue;
            }
            var lnglatXY = [currentItem.Longitude, currentItem.Latitude];
            var currentTempMap = new Map(tempMap);
            tempMap.clear();
            if (currentTempMap.size > 0) {
                if (lngandlataddress.get(currentItem.Longitude + "||" + currentItem.Latitude)) {
                    currentItem.Address = lngandlataddress.get(currentItem.Longitude + "||" + currentItem.Latitude);
                    $("#ul" + i + "").append(" <li class='table-cell2'>" + currentItem.Address + " </li>");
                    currentTempMap.forEach(function (prevItem, prevPointIndex) {
                        prevItem.Address = currentItem.Address;
                        $("#ul" + prevPointIndex + "").append(" <li class='table-cell2'>" + prevItem.Address + " </li>");
                    });
                    //console.log("经纬度解析的直接获取到了的索引_" + i);
                    continue;
                } else {
                    //console.log("当前索引："+i+"_需要补的索引数量："+currentTempMap.size);
                    getAddress(i, currentItem, lnglatXY, geocoder, currentTempMap);
                }
            }
            else {
                if (lngandlataddress.get(currentItem.Longitude + "||" + currentItem.Latitude)) {
                    currentItem.Address = lngandlataddress.get(currentItem.Longitude + "||" + currentItem.Latitude);
                    $("#ul" + i + "").append(" <li class='table-cell2'>" + currentItem.Address + " </li>");
                } else {
                    getAddress(i, currentItem, lnglatXY, geocoder, null);
                }
            }
        }
    });
};

var getAddress = function (index, currentItem, lnglatXY, geocoder, prevPoints) {
    geocoder.getAddress(lnglatXY, function (status, result) {
        if (status === 'complete' && result.info === 'OK') {
            currentItem.Address = result.regeocode.formattedAddress;
            a = result.regeocode.formattedAddress;
            $("#ul" + index + "").append(" <li class='table-cell2'>" + currentItem.Address + " </li>");
            if (!lngandlataddress.get(currentItem.Longitude + "||" + currentItem.Latitude)) {
                lngandlataddress.set(currentItem.Longitude + "||" + currentItem.Latitude, result.regeocode.formattedAddress);
            }
            m1.set(index, result.regeocode.formattedAddress);
            if (prevPoints==null) {
                return;
            }
            prevPoints.forEach(function (prevItem,prevPointIndex) {
                //console.log("对索引：" + prevPointIndex + "_补充地址，地址来源索引：" + index);
                prevItem.Address = currentItem.Address;
                $("#ul" + prevPointIndex + "").append(" <li class='table-cell2'>" + prevItem.Address + " </li>");
            });
        } else {
            $("#ul" + index + "").append("<li class='table-cell2' id='li" + index + "' ><button class='btn btn-primary' onclick='addressresolution(" + index + ")'>地址解析</button></li>");
            //获取地址失败
        }
    });
}

//解析失败后增加的按钮解析，传递进来的是信号的排名
function addressresolution(num) {
    AMap.service('AMap.Geocoder', function () {//回调函数
        //实例化Geocoder
        var geocoder = new AMap.Geocoder({
            city: "010"//城市，默认：“全国”
        });
        var lnglatXY = [searchResult[num].Longitude, searchResult[num].Latitude];
        geocoder.getAddress(lnglatXY, function (status, result) {
            if (status === 'complete' && result.info === 'OK') {
                $("#li" + num + "")[0].innerHTML = result.regeocode.formattedAddress;
            } else {
            }
        })
    })
};


////----右下方显示信号列表，先加载除了地址以外的其他信号信息列表----
function buildSignalsContentWithDiv(result, i, buildContentTextArray) {
    $("#divData1").append(" <ul id='ul" + i + "' class='table-row' style='cursor:pointer;' onclick='changeActiveRow(this)'> </ul>");
    $("#ul" + i + "").append(" <li class='table-cell2'>" + formatNumToDate(result.Time) + " </li>");
    $("#ul" + i + "").append(" <li class='table-cell1'>" + result.Speed + " </li>");
    $("#ul" + i + "").append(" <li class='table-cell1'>" + result.Mileage + " </li>");
    if (result.ACCState == false) {
        $("#ul" + i + "").append(" <li class='table-cell1'>" + buildContentTextArray[0] + " </li>");
    }
    else {
        $("#ul" + i + "").append(" <li class='table-cell1'> " + buildContentTextArray[1] + " </li>");
    }
    if (result.Temperature != null) {
        $("#ul" + i + "").append(" <li class='table-cell1'>" + result.Temperature + " </li>");
    }
    else {
        $("#ul" + i + "").append(" <li class='table-cell1'> </li>");
    }
    //if (result.Address != null) { $("#ul" + i + "").append(" <li class='table-cell2'>" + result.Address + " </li>"); } else {
    //    $("#ul" + i + "").append(" <li class='table-cell1'> </li>");
    //}

    //$("#ul" + i + "").append(" <li class='table-cell2'>" + address + " </li>");
    //创建右下方显示信息 div

    $("#ul" + i + "").click(function () {
        //Index页面定义的判断清空marker方法
        removeMarker();
        var lng = result.Longitude;
        var lat = result.Latitude;
        var angle = result.Angle;
        MapMarkerMove(lng, lat, angle, result.Speed, result.Time, result.Mileage, result);
    });
}


////----创建右下方显示信息div    显示报警信息----  
function buildAlarmContentWithDiv(result, i, buildContentTextArray) {
    //添加ul,li,存放生成数据  
    $("#divData").append(" <ul id='ul" + i + "' class='table-row' style='cursor:pointer;' onclick='changeActiveRow(this)'> </ul>");
    $("#ul" + i + "").append(" <li class='table-cell2'>" + formatNumToDate(result.Time) + " </li>");
    $("#ul" + i + "").append(" <li class='table-cell1'>" + result.Speed + " </li>");
    $("#ul" + i + "").append(" <li class='table-cell1'>" + result.Mileage + " </li>");
    if (result.ACCState == false) {
        $("#ul" + i + "").append(" <li class='table-cell1'>" + buildContentTextArray[0] + "</li>");
    }
    else {
        $("#ul" + i + "").append(" <li class='table-cell1'> " + buildContentTextArray[1] + "</li>");
    }
    if (result.Temperature != null) {
        $("#ul" + i + "").append(" <li class='table-cell1'>" + result.Temperature + " </li>");
    }
    else {
        $("#ul" + i + "").append(" <li class='table-cell1'> </li>");
    }
    $("#ul" + i + "").append(" <li class='table-cell2'>" + result.ExName + " </li>");
    //行点中显示在地图上
    $("#ul" + i + "").click(function () {
        //Index页面定义的判断清空marker方法
        removeMarker();
        var lng = result.Longitude;
        var lat = result.Latitude;
        var angle = result.Angle;
        MapMarkerMove(lng, lat, angle, result.Speed, result.Time, result.Mileage, result);
    });
}


////----创建右下方显示信息 div    显示气压卸料信息列表----  
function buildPressureListContentWithDiv(result, i, buildContentTextArray) {
    //添加ul,li,存放生成数据  
    $("#divPressureData").append(" <ul id='ul" + i + "' class='table-row' style='cursor:pointer;' onclick='changeActiveRow(this)'> </ul>");
    $("#ul" + i + "").append(" <li class='table-cell2'>" + formatNumToDate(result.Time) + " </li>");
    $("#ul" + i + "").append(" <li class='table-cell2'>" + result.Speed + " </li>");
    $("#ul" + i + "").append(" <li class='table-cell2'>" + result.Mileage + " </li>");
    if (result.ACCState == false) {
        $("#ul" + i + "").append(" <li class='table-cell2'>" + buildContentTextArray[0] + " </li>");
    }
    else {
        $("#ul" + i + "").append(" <li class='table-cell2'> " + buildContentTextArray[1] + " </li>");
    }
    $("#ul" + i + "").append(" <li class='table-cell2'> </li>");
    //行点中显示在地图上
    $("#ul" + i + "").click(function () {
        //Index页面定义的判断清空marker方法
        removeMarker();
        var lng = result.Longitude;
        var lat = result.Latitude;
        var angle = result.Angle;
        MapMarkerMove(lng, lat, angle, result.Speed, result.Time, result.Mileage, result);
    });
}

var currentActiveRow; //当前活动行 右下方显示信息
////----改变选中行的颜色----
function changeActiveRow(obj) {
    if (currentActiveRow) {
        currentActiveRow.style.backgroundColor = "";
    }
    currentActiveRow = obj;
    currentActiveRow.style.backgroundColor = "#479AC7";
}


////----点击右下方列表行时，根据角度回传车辆图片----
function funGetVehicleImage(ang) {
    var strFile;
    strFile = "../../Content/images/pic/N.PNG";
    if (ang > 22 && ang <= 68) {
        strFile = "../../Content/images/pic/NE.PNG";
    }
    else if (ang > 68 && ang <= 112) {
        strFile = "../../Content/images/pic/E.PNG";
    }
    else if (ang > 112 && ang <= 158) {
        strFile = "../../Content/images/pic/SE.PNG";
    }
    else if (ang > 158 && ang <= 202) {
        strFile = "../../Content/images/pic/S.PNG";
    }
    else if (ang > 202 && ang <= 248) {
        strFile = "../../Content/images/pic/SW.PNG";
    }
    else if (ang > 248 && ang <= 292) {
        strFile = "../../Content/images/pic/W.PNG";
    }
    else if (ang > 292 && ang <= 338) {
        strFile = "../../Content/images/pic/NW.PNG";
    }
    return strFile;
}


////----地图工具框 去掉选中，地图右上方显示电子围栏图片按钮更换图片表示不同状态----
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


////----根据电子围栏信息在地图上绘制不同形状、不同颜色的电子围栏----
function createElectricFence(result, map) {
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
            map: map,
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
            map: map,
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
            map: map,
            path: polygonArr,
            fillOpacity: 0.3,
            fillColor: fillColor,
            strokeWeight: 2,
            strokeColor: strokeColor
        });
        lngLat = polygon.getBounds().getCenter();       //显示在多边形外切矩形中间
    }
    map.setFitView();

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
                map: map,
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
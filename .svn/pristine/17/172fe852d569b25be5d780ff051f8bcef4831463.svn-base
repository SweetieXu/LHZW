/**
 * 包含以下功能:
 *   * 地图显示
 *   * 地图测距
 *   * 自定义信息窗体
 *   * 自定义控件
 *   * 鼠标工具插件
 */


/**
 * 创建地图对象
 * resizeEnable默认true
 * mapDivId   页面调用地图div的id
 * zoom  Number  地图显示的缩放级别，若center与level未赋值，地图初始化默认显示用户所在城市范围
 */
function initMap(mapDivId, resizeEnable, zoom, center) {
    if (mapDivId == null) {
        return false;
    }
    if (resizeEnable == null) {
        resizeEnable = true;
    }
    var mapObj = new AMap.Map(mapDivId, {
        resizeEnable: resizeEnable,
        zoom: zoom,
        center: center
    });
    return mapObj;
}


/**
 *  添加地图控件 
 *  enableToolBar       Boolean      是否显示地图工具条插件
 *  enableScale         Boolean      是否显示地图比例尺插件
 *  enableGeolocation   Boolean      是否显示定位插件
 *  enableOverView      Boolean      是否显示地图鹰眼插件
 *  enableMapType       Boolean      是否显示地图类型切换插件
 */
function addControl(enableToolBar, enableScale, enableGeolocation, enableOverView, enableMapType, mapObj) {
    if (mapObj == null) {
        return false;
    }
    if (enableToolBar == true) {
        AMap.plugin(['AMap.ToolBar'], function () {
            mapObj.addControl(new AMap.ToolBar());
        });
    }
    if (enableScale == true) {
        AMap.plugin(['AMap.Scale'], function () {
            mapObj.addControl(new AMap.Scale());
        });
    }
    if (enableGeolocation == true) {
        AMap.plugin(['AMap.Geolocation'], function () {
            mapObj.addControl(new AMap.Geolocation());
        });
    }
    if (enableOverView == true) {
        AMap.plugin(['AMap.OverView'], function () {
            mapObj.addControl(new AMap.OverView({ isOpen: false }));
        });
    }
    if (enableMapType == true) {
        AMap.plugin(['AMap.MapType'], function () {
            mapObj.addControl(new AMap.MapType());
        });
    }
    
}


///显示地图图层切换控件
function showLayerSwitcher(showflag, mapObj)
{
    if (mapObj == null) {
        return false;
    }
    if (showflag == true)
    {
        AMapUI.loadUI(['control/BasicControl'], function (BasicControl) {
            //图层切换控件
            mapObj.addControl(new BasicControl.LayerSwitcher({
                position: 'rb'
            }));
        });
    }
}


/**
 * 鼠标点击事件 
 */
function clickOnMap(mapObj) {
    AMap.event.addListener(mapObj, 'click', getLnglat); 	
}


/**
 * 鼠标点击，获取经纬度坐标  
 */
function getLnglat(e) {
    var x = e.lnglat.getLng();
    var y = e.lnglat.getLat();
    return x + "," + y;
}


/**
 * 清空地图 
 */
function clearMap(mapObj) {
    mapObj.clearMap();
}

/**
 *地图自适应显示到合适的范围
 */
function setMapFitView(mapObj) {
    mapObj.setFitView();
}


/**
 * 需在点标记中显示的图标
 * size   图标尺寸，默认值(36,36)
 * image  图标的取图地址。默认为蓝色图钉图片
 */
function initIcon(image, imageSize) {
    var icon = new AMap.Icon({
        image: image,
        //icon可缺省，缺省时为默认的蓝色水滴图标，
        size: imageSize
    });
    return icon;
}


/**
 * 关闭信息窗体
 */
function closeInfoWindow(mapObj) {
    mapObj.clearInfoWindow();
}


function initMarker(mapObj, icon, position, labelText, infowindow) {
    if (mapObj == null) {
        return false;
    }
    var marker = new AMap.Marker({
        mapObj: mapObj,
        position: position,
        icon: icon,
        offset: new AMap.Pixel(-12, -12)
    });
    if (labelText != null) {
        marker.setLabel({//label默认蓝框白底左上角显示，样式className为：amap-marker-label
            offset: new AMap.Pixel(20, 20),//修改label相对于maker的位置
            content: labelText
        });
    }
    //if (infowindow != null) {
    //    //鼠标点击marker弹出自定义的信息窗体
    //    AMap.event.addListener(marker, 'click', function () {
    //        infowindow.open(mapObj, marker.getPosition());
    //    });
    //}
    return marker;
}


/**
 * 实例化marker
 *  result              传参过来的结果集
 *  labelText      String       自带标签
 *  infoWindow     String       信息框
 *  marker     marker对象
 *将labelText, infoWindow, marker绑定到一起，是为了后面刷新marker时能够更新他们的信息，而不是重新加载，可解决infowindow刷新关闭的问题
 */
function Mark(result, labelText, infoWindow, marker) {
    this.result = result;
    this.labelText = labelText;
    this.infoWindow = infoWindow;
    this.marker = marker;
}

Mark.prototype.getResult = function () {
    return this.result;
}
Mark.prototype.getLabelText = function () {
    return this.labelText;
}
Mark.prototype.getInfoWindow = function () {
    return this.infoWindow;
}
Mark.prototype.getMarker = function () {
    return this.marker;
}

Mark.prototype.setResult = function (result) {
    this.result = result;
}
Mark.prototype.setLabelText = function (labelText) {
    this.labelText = labelText;
}
Mark.prototype.setInfoWindow = function (infoWindow) {
    this.infoWindow = infoWindow;
}
Mark.prototype.setMarker = function (marker) {
    this.marker = marker;
}



/**
 * 添加marker
 */
function addMarker(mapObj, image, imageSize, infowindow, result) {
    //设置缩放级别和中心点
    mapObj.setZoomAndCenter(18, [result.Longitude, result.Latitude]);
    var icon = initIcon(image, imageSize);
    var position = [result.Longitude, result.Latitude];
    //创建新的marker
    var marker = initMarker(mapObj, icon, position, result.VN, infowindow);
    //将信息绑定到一起
    var mark = new Mark(result, result.VN, infowindow, marker);
    mark.marker.setMap(mapObj);  //在地图上添加点  
    return mark;
}


/**
 * 修改marker
 */
function updateMarker(mapObj, mark, image, infowindow, result) {
    var marker = mark.getMarker();
    var lngLat = new AMap.LngLat(result.Longitude, result.Latitude);

    marker.setPosition(lngLat);
    if (image != null) {
        marker.setIcon(image);
    }
    marker.setLabel({//label默认蓝框白底左上角显示，样式className为：amap-marker-label
        offset: new AMap.Pixel(20, 20),//修改label相对于maker的位置
        content: result.VN
    });

    //修改infowindow位置
    infowindow.setPosition(lngLat);
    marker.setMap(mapObj);  //在地图上添加点  
    return mark;
}

/**
 * 更换markerContent内容
 */
function changeMarkerContent(mapObj, marker, infowindow, result)
{
    mapObj.clearInfoWindow();//关闭打开的infowindow
    infowindow.open(mapObj, [result.Longitude, result.Latitude]);

    marker.setMap(mapObj);  //在地图上添加点  
    return marker;
}



/**
 * 移除点标记
 */
function removeMarker(mapObj, marker) {
    if (mapObj == null) {
        return false;
    }
    mapObj.clearInfoWindow();//关闭打开的infowindow
    marker.setMap(null);
    //自适应
    mapObj.setFitView();
}


/**
 * 距离测量
 */
function addRangingTool(mapObj) {
    if (mapObj == null) {
        return false;
    }
    mapObj.plugin(["AMap.RangingTool"], function () {
        ruler = new AMap.RangingTool(mapObj);
        AMap.event.addListener(ruler, "end", function (e) {
            ruler.turnOff();
        });
        var sMarker = {
            icon: new AMap.Icon({
                size: new AMap.Size(19, 31),//图标大小
                image: "../../Content/images/mark_b1.png"
            })
        };
        var eMarker = {
            icon: new AMap.Icon({
                size: new AMap.Size(19, 31),//图标大小
                image: "../../Content/images/mark_b2.png"
            }),
            offset: new AMap.Pixel(-9, -31)
        };
        var lOptions = {
            strokeStyle: "solid",
            strokeColor: "#FF33FF",
            strokeOpacity: 1,
            strokeWeight: 2
        };
        var rulerOptions = { startMarkerOptions: sMarker, endMarkerOptions: eMarker, lineOptions: lOptions };
    });
}

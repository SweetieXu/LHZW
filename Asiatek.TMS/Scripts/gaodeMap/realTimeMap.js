
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
 * 实例化marker
 *  position       LngLat       点标记在地图上显示的位置
 *  offset         Pixel        位置偏移量
 *  icon           String/Icon  需在点标记中显示的图标
 *  angle          Number       点标记的旋转角度支持IE9及以上版本
 *  labelText      String       自带标签
 *  infoWindow     String       是否显示信息框，为null时不显示
 */
function Mark(mapObj, position, offset, icon, angle, labelText, labelOffset, infoWindow, result) {
    this.mapObj = mapObj;
    this.position = position;
    this.offset = offset;
    this.icon = icon;
    this.angle = angle;
    this.labelText = labelText;
    this.labelOffset = labelOffset;
    this.infoWindow = infoWindow;
    this.result = result;
}

Mark.prototype.getMapObj = function () {
    return this.mapObj;
}
Mark.prototype.getPosition = function () {
    return this.position;
}
Mark.prototype.getOffset = function () {
    return this.offset;
}
Mark.prototype.getIcon = function () {
    return this.icon;
}
Mark.prototype.getAngle = function () {
    return this.angle;
}
Mark.prototype.getLabelText = function () {
    return this.labelText;
}
Mark.prototype.getLabelOffset = function () {
    return this.labelOffset;
}
Mark.prototype.getInfoWindow = function () {
    return this.infoWindow;
}

Mark.prototype.setMapObj = function (mapObj) {
    this.mapObj = mapObj;
}
Mark.prototype.setPosition = function (position) {
    this.position = position;
}
Mark.prototype.setOffset = function (offset) {
    this.offset = offset;
}
Mark.prototype.setIcon = function (icon) {
    this.icon = icon;
}
Mark.prototype.setAngle = function (angle) {
    this.angle = angle;
}
Mark.prototype.setLabelText = function (labelText) {
    this.labelText = labelText;
}
Mark.prototype.setLabelOffset = function (labelOffset) {
    this.labelOffset = labelOffset;
}
Mark.prototype.setInfoWindow = function (infoWindow) {
    this.infoWindow = infoWindow;
}

Mark.prototype.init = function () {
    if (this.mapObj == null) {
        return false;
    }
    var marker = new AMap.Marker({
        mapObj: this.mapObj,
        position: this.position,
        icon: this.icon,
        offset: this.offset
    });
    if (this.labelText != null) {
        marker.setLabel({//label默认蓝框白底左上角显示，样式className为：amap-marker-label
            //offset: new AMap.Pixel(20, 20),//修改label相对于maker的位置
            offset: this.labelOffset,//修改label相对于maker的位置
            content: this.labelText
        });
    }
    if (this.infoWindow != null) {
        //自定义信息窗体
        //var infoW = SetInfoWindow(this.result);
        var infoW = this.infoWindow;

        //鼠标点击marker弹出自定义的信息窗体
        AMap.event.addListener(marker, 'click', function () {
            infoW.open(mapObj, marker.getPosition());
        });
    }
    marker.setMap(this.mapObj);
    return marker;
}






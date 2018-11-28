function Mark(mapObj, position, offset, icon, visible, angle, labelText, infoWindow)
{
    this.mapObj = mapObj;
    this.position = position;
    this.offset = offset;
    this.icon = icon;
    this.visible = visible;
    this.angle = angle;
    this.labelText = labelText;
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
            offset: new AMap.Pixel(20, 20),//修改label相对于maker的位置
            content: this.labelText
        });
    }
    if (this.infoWindow != null) {
        //构建信息窗体中显示的内容
        var info = [];
        info.push("<div><b>" + result.VN + "</b>");
        info.push("定位信息");
        info.push("速度：" + result.Speed + "");
        info.push("方向：" + result.Direction + "");
        info.push("经度：" + result.Longitude + "");
        info.push("纬度：" + result.Latitude + "");
        info.push("定位时间：" + result.SignalTime + "");
        info.push("位置：" + result.Address + "</div>");
        info.join("<br/>");

        //自定义信息窗体
        marker.setInfoWindow = new AMap.InfoWindow({
            content: info.join("<br/>"),  //使用默认信息窗体框样式，显示信息内容
            offset: new AMap.Pixel(1, -10)
        });
    }
    return marker;
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
Mark.prototype.getVisible = function () {
    return this.visible;
}
Mark.prototype.getAngle = function () {
    return this.angle;
}
Mark.prototype.getLabelText = function () {
    return this.labelText;
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
Mark.prototype.setIcon = function (icon) {
    this.icon = icon;
}
Mark.prototype.setVisible = function (visible) {
    this.visible = visible;
}
Mark.prototype.setAngle = function (angle) {
    this.angle = angle;
}
Mark.prototype.setLabelText = function (labelText) {
    this.labelText = labelText;
}
Mark.prototype.setInfoWindow = function (infoWindow) {
    this.infoWindow = infoWindow;
}

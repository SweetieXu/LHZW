///简单路径+巡航器，历史轨迹页面的轨迹动态展示功能
var colors = [ "#3366cc", "#00ff33", "#000000", "#ff0000", "#cc00ff" ];
var pathNavigs = [];
var flag;
var currencyspeed;
var pathSimplifierIns;
var points;

function showSignalsByPathSimplifier(value, routeItemTextArray, pathSimplifierTextArray, navigBtnsConf, map) {
    var allPoints = [];
    var blindPoints = [];
    var blindLabel = [];
    var alarmPoints = [];
    var alarmLabel = [];
    var fixedPoints = [];
    var fixedLabel = [];
    var normalPoints = [];
    var normalLabel = [];
    var acc;
    var temper;
    var label;
    var lastPointType;//1:正常；2：盲区；3：报警；4：盲区+报警
    var currentPointType = 1;
    var tempPoints = new Array();
    var tempLabels = new Array();
    for (var i = 0; i < value.length; i++) {
        if (value[i].ACCState == false) {
            acc = "<br/> " + pathSimplifierTextArray[0] + ":" + pathSimplifierTextArray[1];
        }
        else {
            acc = "<br/> " + pathSimplifierTextArray[2] + ":" + pathSimplifierTextArray[3];
        }
        if (value[i].Temperature != null) {
            temper = "<br/> " + pathSimplifierTextArray[4] + " ( ℃ ):" + value[i].Temperature + "";
        }
        else {
            temper = "";
        }
        label = pathSimplifierTextArray[5] + ":" + value[i].Speed + "<br/>" + pathSimplifierTextArray[6] + ":" + formatNumToDate(value[i].Time) + "<br/>" + pathSimplifierTextArray[7] + ":" + value[i].Mileage + acc + temper + "<br/>" + pathSimplifierTextArray[8] + ":" + value[i].ExName;
        if (value[i].IsBlind == "1" && value[i].ExName != "") {//盲区+报警
            currentPointType = 4;
        }
        else if (value[i].ExName != "") {//报警
            currentPointType = 3;
        }
        else if (value[i].IsBlind == "1") {//盲区
            currentPointType = 2;
        }
        else {//正常
            currentPointType = 1;
        }

        //第一个点不用比较是否与之前的类型是否相同
        //从第二个点开始，与之前一个点类型不同，就截断一条轨迹出来，并且前一段轨迹的结束点是后一段的开始点，这样才能看上去是连续的
        if ((i != 0 && lastPointType != currentPointType) || i == value.length - 1) {
            if (i == value.length - 1) {
                //console.log("最后一个");
            } else {
                //console.log("不同");
            }
            //加入当前的点，当作轨迹的结尾
            tempPoints.push([value[i].Longitude, value[i].Latitude]);
            tempLabels.push(label);
            //添加一条带显示的轨迹
            //var lastPointType = 1;//1:正常；2：盲区；3：报警；4：盲区+报警
            var pathName;
            var pointTypeFlag = lastPointType;
            if (pointTypeFlag == undefined) {//如果只查询出一个点，那么轨迹的名称就是这个点的类型名称
                pointTypeFlag = currentPointType;
            }
            if (pointTypeFlag == 1) {
                pathName = pathSimplifierTextArray[9];
            } else if (pointTypeFlag == 2) {
                pathName = pathSimplifierTextArray[10];
            } else if (pointTypeFlag == 3) {
                pathName = pathSimplifierTextArray[11];
            } else {
                pathName = pathSimplifierTextArray[12];
            }
            allPoints.push({ name: pathName, path: tempPoints, label: tempLabels });
            //console.log("添加一条" + pathName + "共" + tempPoints.length + "个点");
            //因为一条轨迹结束了，那么需要清空之前的点，后面新加进去的就是下一段的开始点，正好就是上一段的结尾
            tempPoints = new Array();
            tempLabels = new Array();
        }
        tempPoints.push([value[i].Longitude, value[i].Latitude]);
        tempLabels.push(label);
        lastPointType = currentPointType;
    }
    AMapUI.load(['ui/misc/PathSimplifier', 'lib/$'], function (PathSimplifier, $) {
        if (!PathSimplifier.supportCanvas) {
            $.showPromptDialog(pathSimplifierTextArray[13], null, null, null, true);
            return;
        }
        var colors = [
            "#3366cc", "#109618", "#000000", "#ff0000", "#cc00ff"
        ];
        pathSimplifierIns = new PathSimplifier({
            zIndex: 100,
            map: map, //所属的地图实例
            getPath: function (pathData, pathIndex) {
                return pathData.path;
            },
            getHoverTitle: function (pathData, pathIndex, pointIndex) {
                //console.log(pointIndex);
                if (pointIndex >= 0) {
                    return pathData.name + '<br/>' + pathSimplifierTextArray[14] + ':' + (pointIndex + 1) + '/' + pathData.path.length + '<br/>' + pathData.label[pointIndex];
                }
                return pathData.name + '，' + pathSimplifierTextArray[15] + pathSimplifierTextArray[16] + pathData.path.length;
            },
            renderOptions: {
                keyPointTolerance: 1,
                renderAllPointsIfNumberBelow: 1000,
                //pathLineHoverStyle: {
                //    strokeStyle: "#3366cc"
                //},
                getPathStyle: function (pathItem, zoom) {
                    var pathName = pathItem.pathData.name;
                    var pathIndex = pathItem.pathIndex;
                    if (!pathName) {
                        return null;
                    }
                    var i = 0;
                    if (pathName == pathSimplifierTextArray[17]) {
                        i = 1;
                    } else if (pathName == pathSimplifierTextArray[18]) {
                        i = 2;
                    } else if (pathName == pathSimplifierTextArray[19]) {
                        i = 3;
                    } else if (pathName == pathSimplifierTextArray[20]) {
                        i = 4;
                    }
                    //只有第一个轨迹能看到开始点和最后一条轨迹能看到结束点
                    var color = colors[i];
                    var returnObj = {
                        pathLineStyle: {
                            strokeStyle: color,
                            lineWidth: 2.8
                        },
                        endPointStyle: {//默认没结束点
                            radius: 0,
                            lineWidth: 0,
                        },
                        startPointStyle: {//默认没开始点
                            radius: 0,
                            lineWidth: 0,
                        },
                    };
                    if (pathIndex == 0) {//第一个轨迹有开始点
                        returnObj.startPointStyle = {
                            radius: 10,
                            fillStyle: "blue",
                            lineWidth: 1,
                            strokeStyle: "#eeeeee"
                        };
                    }
                    if (pathIndex == allPoints.length - 1) {//最后一个轨迹有结束点
                        returnObj.endPointStyle = {
                            radius: 10,
                            fillStyle: "red",
                            lineWidth: 1,
                            strokeStyle: "#eeeeee"
                        };
                    }
                    return returnObj;
                }
            }
        });
        pathSimplifierIns.setData(allPoints);
        ////设置最后一个点zindex大于100，就显示在最上层
        pathSimplifierIns.setZIndexOfPath(0, 101);
        //initRoutesContainer(allPoints, 0);
        initRoutesContainer(allPoints, 0, routeItemTextArray, navigBtnsConf, map);
        points = allPoints;
    });
}

///参数，data：要显示的所有点数据；routeItemTextArray：initRouteItem中@DisplayText要显示的所有文字数组；
function initRoutesContainer(data, index, routeItemTextArray, navigBtnsConf, map) {
    initRouteItem(data[index], index, routeItemTextArray, navigBtnsConf);
    refreshNavgButtons(navigBtnsConf);
    $('#routes-container .navigBtn').click(function () {
        var pathIndex = parseInt($(this).closest('.route-item').attr('data-idx'), 0);
        var navg = getNavg(pathIndex, routeItemTextArray, navigBtnsConf, map);
        navg[$(this).attr('data-act')]();
        refreshNavgButtons(navigBtnsConf);
    });
}


function initRouteItem(pathData, idx, routeItemTextArray, navigBtnsConf) {
    var pathName = pathData.name;
    var i = 0;
    if (pathName == routeItemTextArray[0]) {
        i = 1;
    } else if (pathName == routeItemTextArray[1]) {
        i = 2;
    } else if (pathName == routeItemTextArray[2]) {
        i = 3;
    } else if (pathName == routeItemTextArray[3]) {
        i = 4;
    }

    var $routeItem = $('<div class="route-item"></div>');
    $routeItem.attr('data-idx', idx);
    var pointnumber = routeItemTextArray[4] + routeItemTextArray[5]
    $('<h3/>').css({
        color: colors[i]
    }).html(pathData.name + '(' + pointnumber + '： ' + pathData.path.length + ')').appendTo($routeItem).on('click', function () {
        pathSimplifierIns.setSelectedPathIndex(idx);
    });
    for (var i = 0, len = navigBtnsConf.length; i < len; i++) {
        $('<button class="navigBtn btn btn-large btn-primary" data-btnIdx="' + i + '" data-act="' + navigBtnsConf[i].action + '"></button>').html(navigBtnsConf[i].name).appendTo($routeItem);
    }
    $speedBox = $('<div class="speedBox"></div>').appendTo($routeItem);
    var speedTxt = $('<span><span>').appendTo($speedBox);
    var speedRangeInput;
    if (currencyspeed == null) {
        speedRangeInput = $('<input id="speedInp_' + idx +
            '" class="speedRange" type="range" min="1000" max="10000" step="1000" value="1000" />').appendTo($speedBox);
    } else {
        speedRangeInput = $('<input id="speedInp_' + idx +
           '" class="speedRange" type="range" min="1000" max="10000" step="1000" value="' + currencyspeed + '" />').appendTo($speedBox);
    }
    var v_speed = routeItemTextArray[6];
    function updateSpeedTxt() {
        var speed = parseInt(speedRangeInput.val(), 10);
        speedTxt.html(v_speed + '：' + speed + ' km/h');
        currencyspeed = speed;
        if (pathNavigs[idx]) {
            pathNavigs[idx].setSpeed(speed);
        }
    }
    speedRangeInput.on('change', updateSpeedTxt);
    updateSpeedTxt();
    $speedBox.appendTo($routeItem);
    //$('<div class="msg"></div>').appendTo($routeItem);
    $('#routes-container').html($routeItem);
}


function refreshNavgButtons(navigBtnsConf) {
    $('#routes-container').find('div.route-item').each(function () {
        var pathIndex = parseInt($(this).attr('data-idx'), 0);
        if (pathIndex < 0) {
            return;
        }
        var navgStatus = 'stop',
            navgExists = !!pathNavigs[pathIndex];
        if (navgExists) {
            navgStatus = pathNavigs[pathIndex].getNaviStatus();
        }
        $(this).find('.navigBtn').each(function () {
            var btnIdx = parseInt($(this).attr('data-btnIdx'));
            $(this).prop('disabled', !eval(navigBtnsConf[btnIdx].enableExp));
        });
    });
}


function getNavg(pathIndex, routeItemTextArray, navigBtnsConf, map) {
    if (!pathNavigs[pathIndex]) {
        //创建一个轨迹巡航器
        var navgtr = pathSimplifierIns.createPathNavigator(pathIndex, {
            loop: true,
            speed: parseInt($('#speedInp_' + pathIndex).val()),
            pathNavigatorStyle: {
                width: 24,
                height: 24,
                //经过路径的样式
                pathLinePassedStyle: {
                    lineWidth: 6,
                    strokeStyle: 'black',
                    dirArrowStyle: {
                        stepSpace: 15,
                        strokeStyle: 'red'
                    }
                }
            }
        });
        var $markerContent = $('<div class="markerInfo"></div>');
        $markerContent.html(pathSimplifierIns.getPathData(pathIndex).name);
        navgtr.marker = new AMap.Marker({
            offset: new AMap.Pixel(12, -10),
            content: $markerContent.get(0),
            map: map
        });
        var $msg = $('#routes-container').find('div.route-item[data-idx="' +
            pathIndex + '"]').find('.msg');
        navgtr.on('move', function () {
            navgtr.marker.setPosition(navgtr.getPosition());
        });
        navgtr.onDestroy(function () {
            pathNavigs[pathIndex] = null;
            navgtr.marker.setMap(null);
            $msg.html('');
        });
        navgtr.on('start', function () {
            navgtr._startTime = Date.now();
            navgtr._startDist = this.getMovedDistance();
            if (flag == 1) {
                flag = 0;
                navgtr["destroy"]();
                initRoutesContainer(points, 0, routeItemTextArray, navigBtnsConf, map);
                $("#routes-container button:first").click();
            }
        });
        navgtr.on('resume', function () {
            navgtr._startTime = Date.now();
            navgtr._startDist = this.getMovedDistance();
            flag = 0;
        });
        navgtr.on('stop', function () {
        });
        navgtr.on('pause', function () {
            navgtr._movedTime = Date.now() - navgtr._startTime;
            navgtr._movedDist = this.getMovedDistance() - navgtr._startDist;
            navgtr._realSpeed = (navgtr._movedDist / navgtr._movedTime * 3600);
            flag = 1;
            refreshNavgButtons(navigBtnsConf);
        });
        navgtr.on('move', function () {
            if (navgtr.isCursorAtPathEnd()) {
                var index = navgtr.getPathIndex() + 1;
                if (index < points.length) {
                    var navg = getNavg(index, routeItemTextArray, navigBtnsConf, map);
                    initRoutesContainer(points, index, routeItemTextArray, navigBtnsConf, map);
                    $("#routes-container button:first").click();
                    navgtr["destroy"]();
                }
                else {
                    initRoutesContainer(points, 0, routeItemTextArray, navigBtnsConf, map);
                    navgtr["destroy"]();
                    $('#routes-container').find('button').eq(1).click();
                    $('#routes-container').find('button').eq(2).attr("disabled", true);;
                }
            }
        });
        pathNavigs[pathIndex] = navgtr;
    }
    return pathNavigs[pathIndex];
}


//如果地图存在历史轨迹线路，清除地图历史轨迹线路
function clearPathSimplifierIns(){
    if (pathSimplifierIns != null) {
        pathSimplifierIns.setData(null);
    }
}

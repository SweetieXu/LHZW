(function ($) {
    $.TpOptionLine = function (datas,intelLabel) {
        //获取到的数据中温度为5.9|5.3格式，于是先求出温度字段的最多温度数据，即有几条温度曲线
        var tnum;   //温度数据'|'分割后的最大量
        for (var i = 0; i < datas.length; i++) {
            if (i == 0) {
                if (datas[i].Temperature != null) {
                    tnum = datas[i].Temperature.split('|').length;
                } else {
                    tnum = 0;
                }
            }
            else {
                if (datas[i].Temperature != null) {
                    var num = datas[i].Temperature.split('|').length;
                    if (tnum < num) { tnum = num; }
                }
            }
        }

        //处理温度数据，需要分割温度数据
        var tempDatas = new Array(tnum);
        for (var i = 0; i < tnum; i++) {
            tempDatas[i] = [];
            for (var j = 0; j < datas.length; j++) {
                if (datas[j].Temperature != null) {
                    var td = datas[j].Temperature.split('|');
                    //温度数据
                    tempDatas[i].push(parseFloat(td[i]));
                }
            }
        }

        //处理时间数据（X轴数据）、门磁 、速度、方向、地理坐标数据
        var timeDatas = [];
        var sensorDoorDatas = [];
        var speedDatas = [];
        var angelDatas = [];
        var lnglatDatas = [];
        for (var j = 0; j < datas.length; j++) {
            if (datas[j].Temperature != null) {
                var time = formatNumToDate(datas[j].Time);
                timeDatas.push(time);
                sensorDoorDatas.push(datas[j].DoorSensor);
                speedDatas.push(datas[j].Speed);
                angelDatas.push(datas[j].Angle);
                lnglatDatas.push(new AMap.LngLat(datas[j].Longitude, datas[j].Latitude));
            }
        }

        //折线类别
        var legendData = [];
        for (var n = 1; n <= tnum; n++) {
            legendData.push('T' + n);
        }
        //折线颜色设置，目前设置6种颜色，可扩展
        var colorList = ['#27727B', '#E87C25', '#FCCE10', '#B5C334', '#FE8463', '#9BCA63'];
        //折线类别设置
        var series = [];
        //警戒温度设置
        //var  alarmTemper = [10,8];
        for (var n = 0; n < tnum; n++) {
            var m = n % 6;    //用colorList中的颜色显示折线的颜色
            series.push({
                name: legendData[n],
                type: 'line',
                data: tempDatas[n],
                itemStyle: {
                    normal: {
                        color: colorList[m],  //圈圈的颜色
                        lineStyle: {            //线的颜色 
                            color: colorList[m]  
                        }
                    }
                },
                markLine: {         //设置标识线
                    //data: [
                    //    [
                    //        {
                    //            name: legendData[n] + '警戒线',
                    //            coord: [timeDatas[0], alarmTemper[n%2]]        //坐标[time[0],警戒温度]
                    //        }, {
                    //            coord: [timeDatas[timeDatas.length-1], alarmTemper[n%2]]     //坐标[time[length-1],警戒温度]
                    //        }
                    //    ]
                    //],
                    //itemStyle: {
                    //    normal: {
                    //        lineStyle: {            //线的颜色 
                    //            color: 'red',
                    //            type: 'solid'
                    //        },
                    //        label: {
                    //            show: true,
                    //            textStyle: {
                    //                fontSize: 10,
                    //                color: 'red'          // 文字颜色
                    //            }
                    //        }
                    //    }
                    //}
                }

            });

        }
        var option = {
            tooltip: {     //提示框
                trigger: 'axis',       //提示框触发方式，'item':数据项图形触发，'axis':坐标轴触发
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'line',         // 默认为直线，可选为：'line' | 'shadow'
                    lineStyle: {          // 直线指示器样式设置
                        color: '#48b',
                        width: 2,
                        type: 'solid'
                    },
                    shadowStyle: {                       // 阴影指示器样式设置
                        width: 'auto',                   // 阴影大小
                        color: 'rgba(150,150,150,0.3)'  // 阴影颜色
                    },
                    textStyle: {
                        color: '#fff'
                    }
                },
                formatter: function (params) {
                    //如果有汽车轨迹，那么把点移除（上一个点）
                    if (marker != null) {
                        marker.setMap(null);
                    }
                    var icon = funGetVehicleImage(angelDatas[params[0].dataIndex]);           //根据角度获取车辆图标
                    marker = new AMap.Marker({                        //显示车辆图标
                        position: lnglatDatas[params[0].dataIndex],      //经纬度
                        map: map,
                        icon: icon,
                        offset: new AMap.Pixel(-10, -10),
                        title: ""
                    });
                    var res = params[0].name + '<br/>';     //时间
                    for (var i = 0; i < params.length; i++) {
                        res += params[i].seriesName + '(℃): ' + params[i].data + '</br>';
                    }
                    res += intelLabel[0] + ' (KM/H): ' + speedDatas[params[0].dataIndex] + '</br>';     //温度
                    res += sensorDoorDatas[params[0].dataIndex] + '</br>';            //门磁开关
                    return res;
                }
            },
            legend: {        //图例
                data: legendData,
                orient: 'vertical',
                x: 'right',
                y: 'top'
            },
            grid: {        //网格，left和right值过小会导致x轴左右两端时间显示不全
                left: '6%',
                right: '8%',
                bottom: '15%',
                top: '5%',
                containLabel: true
            },
            dataZoom: {
                type: 'slider',     //图表下方的伸缩条
                show: true,
                realtime: true,     //缩放变化是否实时显示，在不支持Canvas的浏览器中该值自动强制置为false
                start: 0,
                end: 100,
                textStyle: {
                    fontSize: 10,
                    fontWeight: 'bolder'
                },
            },
            xAxis: {
                data: timeDatas
            },
            yAxis: {},
            series: series
        };
        return option;
    };

    $.DsOptionLine = function (datas,intelLabel2) {
        //处理时间数据（X轴数据）、门磁数据
        var timeDatas = [];
        var sensorDoorFlageDatas = [];
        for (var j = 0; j < datas.length; j++) {
            if (datas[j].Temperature != null) {
                var time = formatNumToDate(datas[j].Time);
                timeDatas.push(time);
                if (datas[j].DoorSensorFlag == true) 
                    sensorDoorFlageDatas.push(1);
                if (datas[j].DoorSensorFlag == false)
                    sensorDoorFlageDatas.push(0);
            }
        }

        var option = {
            tooltip: {     //提示框
                trigger: 'axis',      //提示框触发方式，'item':数据项图形触发，'axis':坐标轴触发
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'line',         // 默认为直线，可选为：'line' | 'shadow'
                    lineStyle: {          // 直线指示器样式设置
                        color: '#48b',
                        width: 2,
                        type: 'solid'
                    }
                },
                formatter: function (params) {    //设置阴影指示器不显示内容
                    return '';
                }
            },
            grid: {        //网格，left和right值过小会导致x轴左右两端时间显示不全
                left: '6%',
                right: '8%',
                bottom: '1%',
                top: '5%',
                containLabel: true
            },
            xAxis: {
                data: timeDatas,
                show: false
            },
            yAxis: {
                type: 'category',        //类目轴
                data: [intelLabel2[0], intelLabel2[1]] ,    //关、开
                axisLabel:{
                    show:true,
                    interval: 0,
                    axisTick: { show: false }
                }
            },
            dataZoom: {
                type: 'slider',     //图表下方的伸缩条
                show: false,
                realtime: true,     //缩放变化是否实时显示，在不支持Canvas的浏览器中该值自动强制置为false
                start: 0,
                end: 100
            },
            series: [{
                type: 'line',
                data: sensorDoorFlageDatas,
                itemStyle: {
                    normal: {
                        areaStyle: {
                            // 区域图
                            color: '#60C0DD'
                        },
                        color: '#60C0DD',
                        label: {
                            show: false
                        },
                        lineStyle: {
                            width: 1,
                            type: 'solid',
                            shadowColor: 'rgba(0,0,0,0)', //默认透明
                            shadowBlur: 5,
                            shadowOffsetX: 3,
                            shadowOffsetY: 3
                        }
                    },
                    emphasis: {
                        label: {
                            show: false
                        }
                    }
                }
            }]
        };
        return option;
    };

})(jQuery);
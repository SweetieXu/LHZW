
/*某些属性设置使用前要确认已经引用相关js文件*/
(function ($) {
    //温度图表显示
    //TargetId 目标位置(目标元素ID)
    $.ajaxDataRenderer = function (TargetId, dataValue, titleName, xaxisName, yaxisName, num) {
        var seriesArray = new Array(num);
        for (var i = 0; i < num; i++) {
            seriesArray[i] = "T" + (i + 1);
        }
        //seriesArray[num] = "速度";
        var plot = $.jqplot(TargetId, dataValue, OptionLine(titleName, xaxisName, yaxisName, seriesArray));
        plot.replot();//释放绘画对象
    };

    //门磁图表显示
    $.showDoorsensorTB = function (TargetId, dataValue, xaxisName, yaxisName) {
        var plot1 = $.jqplot(TargetId, dataValue, OptionLine1(xaxisName, yaxisName));
        plot1.replot();//释放绘画对象
    };

    //折线参数设置  温度图表
    var OptionLine = function (titleName, xaxisName, yaxisName, seriesArray) {

        return {
            //图标标题
            //title: {
            //    text: titleName,
            //    show: false,
            //    fontFamily: "Microsoft Yahei",
            //    fontSize: "15pt",
            //    textColor: "#000"
            //},
            //提示栏显示配置，通常在右上角显示
            legend: {
                show: true,
                location: 'ne',// 提示栏信息显示位置（英文方向的首写字母）: n, ne, e, se, s, sw, w, nw.
                placement: 'outside'
            },
                //提示栏显示信息配置，多个数据分类需配置多个
            series: [{ 
                lineWidth: 1,
                    label: seriesArray[0],//按data先后顺序显示每种分类名称  
                    showMarker: false          //只显示折线不显示点
                },
            {
                lineWidth: 1,
                label: seriesArray[1],
                showMarker: false
            },
            {
                lineWidth: 1,
                label: seriesArray[2],
                showMarker: false
            },
            {
                lineWidth: 1,
                label: seriesArray[3],
                showMarker: false
            }],
            //节点数值提示，需要引用jqplot.pointLabels.js
            seriesDefaults: {
                pointLabels: { show: false, ypadding: -1 } //数据点标签
            },
            //鼠标放在节点时突出显示当前结点,需引用jqplot.highlighter.js
            highlighter: {
                show: true,
                showMarker: true,
                sizeAdjust: 3,//当鼠标移动到数据点上时，数据点扩大的增量
                fadeTooltip: true,//设置提示信息栏出现和消失的方式（是否淡入淡出）
                //lineWidthAdjust: 2.5,   //当鼠标移动到放大的数据点上时，设置增大的数据点的宽度
                tooltipLocation: 'nw',// 提示信息显示位置（英文方向的首写字母）: n, ne, e, se, s, sw, w, nw.
                tooltipAxes: 'x',          // 提示信息框显示数据点那个坐标轴上的值，目前有横/纵/横纵三种方式，值分别为 x, y, xy or both
                tooltipFormatString: '%s',              
                tooltipContentEditor: function (str, seriesIndex, pointIndex, plot) {
                    //显示获取到x点的竖线
                    var co = plot.plugins.canvasOverlay;
                    var line = co.get('vline');
                    line.options.x = new Date(str).getTime();
                    line.OptionLine;
                    co.draw(plot);

                    //地图上画点
                    //map.clearMap();
                    //如果有汽车轨迹，那么把移除（上一个点）
                    if (marker != null) {
                        marker.setMap(null);
                    }
                    var lng = plot.data[seriesIndex][pointIndex][2];       //经度
                    var lat = plot.data[seriesIndex][pointIndex][3];            //纬度
                    var angel = plot.data[seriesIndex][pointIndex][4];        //方向
                    var icon = funGetVehicleImage(angel);
                    var latLng = new AMap.LngLat(lng, lat);
                    marker = new AMap.Marker({
                        position: latLng,
                        map: map,
                        icon: icon,
                        offset: new AMap.Pixel(-10, -10),
                        title: ""
                    });
                    //显示该x点的所有x、y数据
                    var len = seriesArray.length;
                    var text = "<table id='highlighterTB' class='jqplot-highlighter'>";
                    for (var i = 0; i < len; i++) {
                        if (i == 0) { text += "<tr><td>"+ plot.data[i][pointIndex][0] +"</td></tr> "; }
                        if (!isNaN(plot.data[i][pointIndex][1])) {
                            text += "<tr><td>" + plot.series[i]["label"] + ": " + plot.data[i][pointIndex][1] + "(℃)</td></tr>";
                        }
                        if (i == len - 1) {
                            text += "<tr><td>速度: " + plot.data[i][pointIndex][6] + "(KM/H)</td></tr>";
                            text += "<tr><td>" + plot.data[i][pointIndex][5] + "</td></tr>";          //门磁状态信息
                        }
                    }
                    text += "</table>";
                    return text;
                }
            },
            //鼠标在图标中的提示位置信息，需引用jqplot.cursor.js
            cursor: {
                show: false,                 //是否显示光标
                style: 'crosshair'
                //zoom: true              //地图缩放功能
                //showTooltip: true,  // 是否显示提示信息栏
            },
            canvasOverlay: {        //竖线显示设置
                show: true,
                objects:
                        [
                           {
                               verticalLine: {
                                   name: 'vline',
                                   x: [],
                                   lineWidth: 2,
                                   color: 'black',
                                   shadow: false
                               }
                           }
                        ]
            },
            //设置X,Y轴默认加载方式
            axesDefaults: {
                tickRenderer: $.jqplot.CanvasAxisTickRenderer,// 设置横（纵）轴上数据加载的渲染器，需引用jqplot.canvasAxisTickRenderer.js
                tickOptions: {
                    //fontSize: '10pt',
                    showMark: true,     //设置是否显示刻度  
                    showGridLine: true, // 是否在图表区域显示刻度值方向的网格线 
                    show: true,         // 是否显示刻度线，与刻度线同方向的网格线，以及坐标轴上的刻度值  
                    showLabel: true,    // 是否显示刻度线以及坐标轴上的刻度值  
                    //angle: 40   // 刻度值与坐标轴夹角，角度为坐标轴正向顺时针方向，需引用jqplot.canvasTextRenderer.js，同时添加canvasAxisTickRenderer.js，否则无效 
                },
                tickDistribution: 'even',// 坐标轴显示方式：'even' or 'power'.  'even' 产生的是均匀分布于坐标轴上的坐标刻度值 。而'power' 则是根据不断增大的增数来确定坐标轴上的刻度
                autoscale: true,
                numberTicks: 4 //一个相除因子，用于设置横（纵）坐标刻度间隔    //横（纵）坐标刻度间隔值=横（纵）坐标区间长度/(numberTicks-1)  //需要autoscale设为true
            },
            axes: {
                xaxis: {
                    //label: xaxisName,  //x轴显示标题
                    renderer: $.jqplot.DateAxisRenderer, //x轴绘制方式
                    //ticks: ticks,
                    tickOptions: {
                        fontSize: '8pt',
                        //angle: -25,  //倾斜角度
                        labelPosition: 'middle',//start，middle，auto，end
                        formatString: '%Y-%m-%d %H:%M:%S'
                    }
                },
                yaxis: {
                    label: yaxisName, // y轴显示标题
                    //min: 0,//Y轴最小值
                    tickOptions: {
                        fontSize: '8pt',
                        labelPosition: 'middle',//start，middle，auto，end
                        formatString: '%.2f'
                    }
                }
            }
        }
    };

    //折线参数设置  门磁图表
    var OptionLine1 = function (xaxisName, yaxisName) {
        return {
            series: [{
                showLine: true,
                // showLine为true，设置fill样式，y轴为1时向下填充
                fill: true,        // 是否填充图表中折线下面的区域（填充颜色同折线颜色）以及legend  
                //设置的分类名称框中分类的颜色，此处注意的是如果fill为true，  
                //那么showLine必须为true，否则不会显示效果   
                fillColor: '#0066ff',      // 设置填充区域的颜色  
                //设置showLine为false，添加以下markerOptions样式，只显示点，不连线，门关在y=0添加点，门开在y=1添加点
                //markerOptions: {       //
                //    size: 7,
                //    style: "filledCircle",
                //    color: '#113DEE'                                                                   
                //}
            }],
            //鼠标放在节点时突出显示当前结点,需引用jqplot.highlighter.js
            highlighter: {
                show: false,
                showMarker: false,
                sizeAdjust: 3//当鼠标移动到数据点上时，数据点扩大的增量
            },
            //以下设置与 OptionLine相同，控制x轴时间范围相同
            //设置X,Y轴默认加载方式
            axesDefaults: {
                tickRenderer: $.jqplot.CanvasAxisTickRenderer,// 设置横（纵）轴上数据加载的渲染器，需引用jqplot.canvasAxisTickRenderer.js
                tickOptions: {
                    //fontSize: '10pt',
                    showMark: false,     //设置是否显示刻度  
                    showGridLine: false, // 是否在图表区域显示刻度值方向的网格线 
                    show: false,         // 是否显示刻度线，与刻度线同方向的网格线，以及坐标轴上的刻度值  
                    showLabel: false,    // 是否显示刻度线以及坐标轴上的刻度值  
                    //angle: 40   // 刻度值与坐标轴夹角，角度为坐标轴正向顺时针方向，需引用jqplot.canvasTextRenderer.js，同时添加canvasAxisTickRenderer.js，否则无效 
                },
                tickDistribution: 'even',// 坐标轴显示方式：'even' or 'power'.  'even' 产生的是均匀分布于坐标轴上的坐标刻度值 。而'power' 则是根据不断增大的增数来确定坐标轴上的刻度
                autoscale: true,
                numberTicks: 4 //一个相除因子，用于设置横（纵）坐标刻度间隔    //横（纵）坐标刻度间隔值=横（纵）坐标区间长度/(numberTicks-1)  //需要autoscale设为true
            },
            axes: {
                xaxis: {
                    //label: xaxisName,  //x轴显示标题
                    renderer: $.jqplot.DateAxisRenderer, //x轴绘制方式
                    //ticks: ticks,
                    tickOptions: {
                        fontSize: '8pt',
                        //angle: -25,  //倾斜角度
                        labelPosition: 'middle',//start，middle，auto，end
                        formatString: '%Y-%m-%d %H:%M:%S'
                    }
                },
                yaxis: {
                    label: yaxisName, // y轴显示标题
                    //门磁取值0、1
                    min: 0,      // 纵坐标显示的最小值  
                    max: 1,      // 纵坐标显示的最大值（保留最大值1，将门开值设置很大，保证竖线绘制向上填满，不会出现绘制一部分的情况）  
                    tickOptions: {
                        fontSize: '8pt',
                        labelPosition: 'middle',//start，middle，auto，end
                        formatString: '%.2f'
                    }
                }
            }
        }
    };


})(jQuery);

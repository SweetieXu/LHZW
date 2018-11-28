// JavaScript Document
// 这个版本js相对于jquery-PlayBar.min.js版本，删除了部分方法，原因是原版本在历史轨迹页面使用时候不能选中文字
(function($) {
    var isAction = true;
    var width = 0;
    var thewidth = 0;
    var CurrTime = 0;    //记录当前进度条表示时间
    var t;
    var addHour = 0,
        addMinute = 0,
        addSecond = 0,
        TheHour = 0,
        TheMinute = 0,
        TheSecond = 0;
    var flag = 0;//标志位
    var alltime = 0;//总时间毫秒
    var addwidth = 0;//每次增加的长度
    var offsetW = 0;//偏移量
    var times = 0;
    var rwidth = 0;
    jQuery.playBar = {
        addBar: function(DOM, allTime) {
            CleanAll();
            alltime = allTime;
            DOM.empty();
            DOM.append("<div class='BarControl'><div id='BarBeginTime' style='visibility:hidden;' class='BarBeginTime'>00:00</div></div>");
            $(".BarControl").append('<div class="TheBar"><div id="TimeBall" class="TimeBall"></div><div class="TheColorBar"></div></div><div id="BarFinishTime" style="visibility:hidden;" class="BarFinishTime">10:35</div>');
            width = $('.TheBar').width();
            times = allTime / 1000;
            rwidth = width - 8;
            addwidth = (width - 10) / times;
            var t = TransitionTime(allTime);
            $('.BarFinishTime').html(t.StringTime);
            OpenBar()
        },
        restTime: function (allTime) { //从新开始
            CleanAll();
            StopBar();
            alltime = allTime;
            width = $('.TheBar').width();
            times = allTime / 1000;
            rwidth = width - 8;
            addwidth = (width - 10) / times;
            var t = TransitionTime(allTime);
            $('.BarFinishTime').html(t.StringTime);
            $('.TheColorBar').css("width", 0);
            $('.TimeBall').css("left", 0);
            OpenBar()
        },
        Stop: function() {
            StopBar()
        },
        Begin: function() {
            OpenBar()
        },  //多次暂停后，点击播放时计算当前进度条位置，因为进度条是每秒改变一次，所以会存在一秒内点击多次，进度条不变的状态，此时会记录显示进度条应该显示的位置
        changeThWidth: function (str) {
            thewidth = str;
        },
        changeBarColor: function(COLOR) {
            var color = typeof(COLOR) != "undefined" ? COLOR : '#3498DB';
            $('.TheColorBar').css("background", color);
            $('.TimeBall').css("background", color)
        },
        changeFontColor: function(COLOR) {
            var color = typeof(COLOR) != "undefined" ? COLOR : '#3498DB';
            $('.BarBeginTime,.BarFinishTime').css("color", color)
        },
        getTheTime: function() {
            return CurrTime
        }
    };
    //定义
    /*
	*****全部清零
	*/
    function CleanAll() {
        isAction = true;
        thewidth = 0;
        CurrTime = 0;
        addHour = 0;
        addMinute = 0;
        addSecond = 0;
        TheHour = 0;
        TheMinute = 0;
        TheSecond = 0;
        offsetW = 0;
        flag = 0
    }
    var down = false;

    //时间拖动时改变时间
    function changeBar() {
        var second, minute, hour;
        thewidth = thewidth * 1 + addwidth - offsetW;
        if (offsetW > 0) {
            offsetW = 0
        }
        if (thewidth < rwidth && CurrTime < alltime) {
            CurrTime = CurrTime + 1 * 1000;
            addSecond = addSecond + 1;
            if (addSecond > 59) {
                addSecond = 0;
                addMinute = addMinute + 1;
                if (addMinute > 59) {
                    addMinute = 0;
                    addHour = addHour + 1
                }
            }  //时间累加判断
            if (addSecond > 9) {
                second = "" + addSecond
            } else {
                second = "0" + addSecond
            } if (addMinute > 9) {
                minute = "" + addMinute
            } else {
                minute = "0" + addMinute
            } if (addHour > 9) {
                hour = "" + addHour
            } else {
                hour = "0" + addHour
            } if (addHour > 0) {
                flag = 1
            }
            if (flag == 0) {
                $('.BarBeginTime').html(minute + ":" + second)
            } else {
                $('.BarBeginTime').html(hour + ":" + minute + ":" + second)
            }
        } else {
            thewidth = rwidth;
            StopBar()
        }
        $('.TheColorBar').css("width", thewidth + 1);
        $('.TimeBall').css("left", thewidth)
    }
    //改变进度条
    function TransitionTime(str) {
        var m = parseFloat(str) / 1000;
        var time = "";
        var second, minute, hour;
        var haveHour = false;
        var ch = 0,
            csx = 0,
            cm = 0;
        if (m >= 60 && m < 60 * 60) {
            if (parseInt(m / 60.0) < 10) {
                minute = "0" + parseInt(m / 60.0)
            } else {
                minute = parseInt(m / 60.0)
            }
            var cs = parseInt(m - parseInt(m / 60.0) * 60);
            if (cs < 10) {
                second = "0" + cs
            } else {
                second = "" + cs
            }
            TheMinute = parseInt(m / 60.0);
            TheSecond = cs;
            cm = TheMinute;
            TheHour = 0;
            csx = cs;
            time = minute + ":" + second;
            $('.BarBeginTime').html("00:00")
        } else if (m >= 60 * 60) {  //到达小时
            flag = 1;
            haveHour = true;
            ch = parseInt(m / 3600.0);
            cm = parseInt((parseFloat(m / 3600.0) - parseInt(m / 3600.0)) * 60);
            csx = parseInt((parseFloat((parseFloat(m / 3600.0) - parseInt(m / 3600.0)) * 60) - parseInt((parseFloat(m / 3600.0) - parseInt(m / 3600.0)) * 60)) * 60);
            if (ch < 10) {
                hour = "0" + ch
            } else {
                hour = "" + ch
            } if (cm < 10) {
                minute = "0" + cm
            } else {
                minute = "" + cm
            } if (csx < 10) {
                second = "0" + csx
            } else {
                second = "" + csx
            }
            TheHour = ch;
            TheMinute = cm;
            TheSecond = csx;
            time = hour + ":" + minute + ":" + second;
            $('.BarBeginTime').html("00:00:00")
        } else {  //秒
            $('.BarBeginTime').html("00:00");
            csx = parseInt(m);
            if (parseInt(m) > 9) {
                second = "" + parseInt(m)
            } else {
                second = "0" + parseInt(m)
            }
            TheMinute = 0;
            TheSecond = parseInt(m);
            TheHour = 0;
            time = "00:" + second
        }
        var tt = {
            hHour: haveHour,
            Thour: ch,
            Tmin: cm,
            Tsec: csx,
            StringTime: time
        };
        return tt
    }
    //毫秒转换成分钟小时格式
    function StopBar() {
        if (!down) {
            isAction = false
        }
        clearInterval(t)
    }
    //进度停止
    function OpenBar() {
        isAction = true;
        t = setInterval(changeBar, 1000)
    }
    //进度条开始
})(jQuery);
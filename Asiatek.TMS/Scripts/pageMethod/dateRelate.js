//日期格式转化   Json传递的日期/Date(数字)/解析
function formatNumToDate(value) {
    var now = eval(value.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));///.../gi是用来标记正则开始和结束；\是转义符；()标注了正则匹配分组1，$1 
    var year = now.getYear() + 1900;//或者 now.getFullYear();
    var month = now.getMonth() + 1;
    var date = now.getDate();
    var hour = now.getHours();
    var minute = now.getMinutes();
    var second = now.getSeconds();
    return year + "-" + compareNine(month) + "-" + compareNine(date) + " " + compareNine(hour) + ":" + compareNine(minute) + ":" + compareNine(second);
}

function compareNine(value) {
    return value > 9 ? value : '0' + value;
}


//时间间隔计算（间隔月份）
function GetDateDiff(startDate, endDate) {
    var startTime = new Date(Date.parse(startDate.replace(/-/g, "/")));
    var startY = startTime.getYear();
    var startM = startTime.getMonth();
    var endTime = new Date(Date.parse(endDate.replace(/-/g, "/")));
    var endY = endTime.getYear();
    var endM = endTime.getMonth();
    var dates;
    if (startY == endY) {
        dates = Math.abs((endM - startM));
    }
    else if (endY - startY == 1) {        //时间跨度一年时，计算前一年的月份+后一年的月份
        dates = 12 - startM + endM;
    }
    else {             //时间跨度超过一年时，提示时间跨越不能超过两个月
        dates = 2;
    }
    return dates;
}


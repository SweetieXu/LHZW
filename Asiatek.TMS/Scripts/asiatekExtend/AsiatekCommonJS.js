


//统一的编辑结果显示
var showCommonEditResult = function (result)
{
    if ($.handleAjaxError(result))
    {
        return;
    }
    var $editWin = $("#divAsiatekEditWin");
    if (result.Success === true)//操作成功
    {
        if ($editWin)
        {
            $editWin.dialog("close");
        }
        $.showPromptDialog(result.Message, null, null, function ()
        {
            if (result.Url)
            {
                location.href = result.Url;
            }
        }, null, null);
    } else if (result.Success === false)//操作失败
    {
        $.showPromptDialog(result.Message, null, null, function ()
        {
            if (result.Url)
            {
                location.href = result.Url;
            }
        }, null, null);
    } else//服务器端验证错误
    {
        if ($editWin)
        {
            $editWin.html(result);
        }
    }
}

//统一的编辑结果显示（不关闭）
var showCommonEditResultNotClose = function (result)
{
    if ($.handleAjaxError(result))
    {
        return;
    }

    if (result.Success === true || result.Success === false)
    {
        $.showPromptDialog(result.Message);
    }
    else//服务器端验证错误
    {
        var $editWin = $("#divAsiatekEditWin");
        if ($editWin)
        {
            $editWin.html(result);
        }
    }
}

//统一的删除结果显示
var showCommonDeleteResult = function (result)
{
    if ($.handleAjaxError(result))
    {
        return;
    }
    $.showPromptDialog(result.Message);
}

//统一的Ajax操作成功结果显示
var commonAjaxSuccess = function (result)
{
    $.handleAjaxError(result);
}

//检查字符串是否是NULL、undefined或空字符串
var isNullOrUndefinedOrEmpty = function (str)
{

    return str == null || str == undefined || trim(str) == '';
}


//为字符串添加format方法
String.prototype.format = function (args)
{
    var result = this;
    if (arguments.length > 0)
    {
        if (arguments.length == 1 && typeof (args) == "object")
        {
            for (var key in args)
            {
                if (args[key] != undefined)
                {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else
        {
            for (var i = 0; i < arguments.length; i++)
            {
                if (arguments[i] != undefined)
                {
                    //var reg = new RegExp("({[" + i + "]})", "g");//这个在索引大于9时会有问题，谢谢何以笙箫的指出


                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}

// 数组转为对象
Array.prototype.serializeObject = function (lName) {
    var o = {};
    $t = this;

    for (var i = 0; i < $t.length; i++) {
        for (var item in $t[i]) {
            o[lName + '[' + i + '].' + item.toString()] = $t[i][item].toString();
        }
    }
    return o;
};

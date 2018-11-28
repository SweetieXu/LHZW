
$.extend({
    /**
返回Cookie的json格式对象
作者：戴天辰
调用示例：
var temp=$.cookie(document.cookie).你的Cookie中的键;
*/
    cookie: function (ck)
    {
        if (ck == "")
        {
            return undefined;
        }
        //obj用于最终转换为JSON对象
        var obj = "{";
        //对如code=Wmzd;.info=vc=验证码&userName=dtc这样的多键值cookie做处理
        while (ck.indexOf(";") > 0)
        {
            ck = ck.replace(";", "&"); //得到code=Wmzd&.info=vc=验证码&userName=dtc
        }
        //再按照&分割成数组
        var arry = ck.split("&");
        //循环数组
        for (var i = 0; i < arry.length; i++)
        {
            if ($.trim(arry[i]).split("=").length == 2)
            {
                //则此时obj={"键:值",
                obj += "\"" + $.trim(arry[i]).split("=")[0] + "\":\"" + $.trim(arry[i]).split("=")[1] + "\",";
            }
            //如果格式为cookie名=键=值
            if ($.trim(arry[i]).split("=").length == 3)
            {
                obj += "\"" + $.trim(arry[i]).split("=")[1] + "\":\"" + $.trim(arry[i]).split("=")[2] + "\",";
            }
        }
        //将obj最后的一个","去掉,并且补上"}",最终格式为{"code":"Wmzd","vc":"验证码","userName":"dtc"}
        obj = obj.substring(0, obj.length - 1) + "}";
        //最后返回json格式的obj对象
        return $.parseJSON(obj);
    }
});




$.extend({
    /**
    针对TMS的Ajax操作可能的异常结果，定义一个统一的处理逻辑
    作者：戴天辰
    调用示例：
    $.post("", {}, function (result)
    {
        if ($.handleAjaxError(result))
        {
            return;
        }
        //回调处理逻辑
    });
*/
    handleAjaxError: function (result)
    {
        if (result.AsiatekError)//如果AsiatekError为true
        {
            if (result.Message)//如果有Message提示消息
            {
                $.showPromptDialog(result.Message, null, null, function ()
                {
                    if (result.Url)
                    {
                        location.href = result.Url;
                    }
                }, null, null);
            } else if (result.Url)
            {
                location.href = result.Url;
            }
            return true;
        }
    }
});



$.extend({
    /**
    获取32位大写MD5摘要
    调用示例：
    $(function () {
                $("#btn").click(function () {
                    $("#txt").val($.md5($("#txt").val()));
                });
            });
    */
    md5: function (t)
    {
        var hexcase = 0;
        var b64pad = "";
        var chrsz = 8;
        function hex_md5(s) { return binl2hex(core_md5(str2binl(s), s.length * chrsz)); };
        function b64_md5(s) { return binl2b64(core_md5(str2binl(s), s.length * chrsz)); };
        function hex_hmac_md5(key, data) { return binl2hex(core_hmac_md5(key, data)); };
        function b64_hmac_md5(key, data) { return binl2b64(core_hmac_md5(key, data)); };
        function calcMD5(s) { return binl2hex(core_md5(str2binl(s), s.length * chrsz)); };
        function md5_vm_test()
        {
            return hex_md5("abc") == "900150983cd24fb0d6963f7d28e17f72";
        };
        function core_md5(x, len)
        {
            x[len >> 5] |= 0x80 << ((len) % 32);
            x[(((len + 64) >>> 9) << 4) + 14] = len;
            var a = 1732584193;
            var b = -271733879;
            var c = -1732584194;
            var d = 271733878;
            for (var i = 0; i < x.length; i += 16)
            {
                var olda = a;
                var oldb = b;
                var oldc = c;
                var oldd = d;
                a = md5_ff(a, b, c, d, x[i + 0], 7, -680876936);
                d = md5_ff(d, a, b, c, x[i + 1], 12, -389564586);
                c = md5_ff(c, d, a, b, x[i + 2], 17, 606105819);
                b = md5_ff(b, c, d, a, x[i + 3], 22, -1044525330);
                a = md5_ff(a, b, c, d, x[i + 4], 7, -176418897);
                d = md5_ff(d, a, b, c, x[i + 5], 12, 1200080426);
                c = md5_ff(c, d, a, b, x[i + 6], 17, -1473231341);
                b = md5_ff(b, c, d, a, x[i + 7], 22, -45705983);
                a = md5_ff(a, b, c, d, x[i + 8], 7, 1770035416);
                d = md5_ff(d, a, b, c, x[i + 9], 12, -1958414417);
                c = md5_ff(c, d, a, b, x[i + 10], 17, -42063);
                b = md5_ff(b, c, d, a, x[i + 11], 22, -1990404162);
                a = md5_ff(a, b, c, d, x[i + 12], 7, 1804603682);
                d = md5_ff(d, a, b, c, x[i + 13], 12, -40341101);
                c = md5_ff(c, d, a, b, x[i + 14], 17, -1502002290);
                b = md5_ff(b, c, d, a, x[i + 15], 22, 1236535329);
                a = md5_gg(a, b, c, d, x[i + 1], 5, -165796510);
                d = md5_gg(d, a, b, c, x[i + 6], 9, -1069501632);
                c = md5_gg(c, d, a, b, x[i + 11], 14, 643717713);
                b = md5_gg(b, c, d, a, x[i + 0], 20, -373897302);
                a = md5_gg(a, b, c, d, x[i + 5], 5, -701558691);
                d = md5_gg(d, a, b, c, x[i + 10], 9, 38016083);
                c = md5_gg(c, d, a, b, x[i + 15], 14, -660478335);
                b = md5_gg(b, c, d, a, x[i + 4], 20, -405537848);
                a = md5_gg(a, b, c, d, x[i + 9], 5, 568446438);
                d = md5_gg(d, a, b, c, x[i + 14], 9, -1019803690);
                c = md5_gg(c, d, a, b, x[i + 3], 14, -187363961);
                b = md5_gg(b, c, d, a, x[i + 8], 20, 1163531501);
                a = md5_gg(a, b, c, d, x[i + 13], 5, -1444681467);
                d = md5_gg(d, a, b, c, x[i + 2], 9, -51403784);
                c = md5_gg(c, d, a, b, x[i + 7], 14, 1735328473);
                b = md5_gg(b, c, d, a, x[i + 12], 20, -1926607734);
                a = md5_hh(a, b, c, d, x[i + 5], 4, -378558);
                d = md5_hh(d, a, b, c, x[i + 8], 11, -2022574463);
                c = md5_hh(c, d, a, b, x[i + 11], 16, 1839030562);
                b = md5_hh(b, c, d, a, x[i + 14], 23, -35309556);
                a = md5_hh(a, b, c, d, x[i + 1], 4, -1530992060);
                d = md5_hh(d, a, b, c, x[i + 4], 11, 1272893353);
                c = md5_hh(c, d, a, b, x[i + 7], 16, -155497632);
                b = md5_hh(b, c, d, a, x[i + 10], 23, -1094730640);
                a = md5_hh(a, b, c, d, x[i + 13], 4, 681279174);
                d = md5_hh(d, a, b, c, x[i + 0], 11, -358537222);
                c = md5_hh(c, d, a, b, x[i + 3], 16, -722521979);
                b = md5_hh(b, c, d, a, x[i + 6], 23, 76029189);
                a = md5_hh(a, b, c, d, x[i + 9], 4, -640364487);
                d = md5_hh(d, a, b, c, x[i + 12], 11, -421815835);
                c = md5_hh(c, d, a, b, x[i + 15], 16, 530742520);
                b = md5_hh(b, c, d, a, x[i + 2], 23, -995338651);
                a = md5_ii(a, b, c, d, x[i + 0], 6, -198630844);
                d = md5_ii(d, a, b, c, x[i + 7], 10, 1126891415);
                c = md5_ii(c, d, a, b, x[i + 14], 15, -1416354905);
                b = md5_ii(b, c, d, a, x[i + 5], 21, -57434055);
                a = md5_ii(a, b, c, d, x[i + 12], 6, 1700485571);
                d = md5_ii(d, a, b, c, x[i + 3], 10, -1894986606);
                c = md5_ii(c, d, a, b, x[i + 10], 15, -1051523);
                b = md5_ii(b, c, d, a, x[i + 1], 21, -2054922799);
                a = md5_ii(a, b, c, d, x[i + 8], 6, 1873313359);
                d = md5_ii(d, a, b, c, x[i + 15], 10, -30611744);
                c = md5_ii(c, d, a, b, x[i + 6], 15, -1560198380);
                b = md5_ii(b, c, d, a, x[i + 13], 21, 1309151649);
                a = md5_ii(a, b, c, d, x[i + 4], 6, -145523070);
                d = md5_ii(d, a, b, c, x[i + 11], 10, -1120210379);
                c = md5_ii(c, d, a, b, x[i + 2], 15, 718787259);
                b = md5_ii(b, c, d, a, x[i + 9], 21, -343485551);
                a = safe_add(a, olda);
                b = safe_add(b, oldb);
                c = safe_add(c, oldc);
                d = safe_add(d, oldd);
            }
            return Array(a, b, c, d);
        };
        function md5_cmn(q, a, b, x, s, t)
        {
            return safe_add(bit_rol(safe_add(safe_add(a, q), safe_add(x, t)), s), b);
        };
        function md5_ff(a, b, c, d, x, s, t)
        {
            return md5_cmn((b & c) | ((~b) & d), a, b, x, s, t);
        };
        function md5_gg(a, b, c, d, x, s, t)
        {
            return md5_cmn((b & d) | (c & (~d)), a, b, x, s, t);
        };
        function md5_hh(a, b, c, d, x, s, t)
        {
            return md5_cmn(b ^ c ^ d, a, b, x, s, t);
        };
        function md5_ii(a, b, c, d, x, s, t)
        {
            return md5_cmn(c ^ (b | (~d)), a, b, x, s, t);
        };
        function core_hmac_md5(key, data)
        {
            var bkey = str2binl(key);
            if (bkey.length > 16) bkey = core_md5(bkey, key.length * chrsz);

            var ipad = Array(16), opad = Array(16);
            for (var i = 0; i < 16; i++)
            {
                ipad[i] = bkey[i] ^ 0x36363636;
                opad[i] = bkey[i] ^ 0x5C5C5C5C;
            }
            var hash = core_md5(ipad.concat(str2binl(data)), 512 + data.length * chrsz);
            return core_md5(opad.concat(hash), 512 + 128);
        };
        function safe_add(x, y)
        {
            var lsw = (x & 0xFFFF) + (y & 0xFFFF);
            var msw = (x >> 16) + (y >> 16) + (lsw >> 16);
            return (msw << 16) | (lsw & 0xFFFF);
        };
        function bit_rol(num, cnt)
        {
            return (num << cnt) | (num >>> (32 - cnt));
        };
        function str2binl(str)
        {
            var bin = Array();
            var mask = (1 << chrsz) - 1;
            for (var i = 0; i < str.length * chrsz; i += chrsz)
                bin[i >> 5] |= (str.charCodeAt(i / chrsz) & mask) << (i % 32);
            return bin;
        };
        function binl2hex(binarray)
        {
            var hex_tab = hexcase ? "0123456789ABCDEF" : "0123456789abcdef";
            var str = "";
            for (var i = 0; i < binarray.length * 4; i++)
            {
                str += hex_tab.charAt((binarray[i >> 2] >> ((i % 4) * 8 + 4)) & 0xF) +
   hex_tab.charAt((binarray[i >> 2] >> ((i % 4) * 8)) & 0xF);
            }
            return str;
        };
        function binl2b64(binarray)
        {
            var tab = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var str = "";
            for (var i = 0; i < binarray.length * 4; i += 3)
            {
                var triplet = (((binarray[i >> 2] >> 8 * (i % 4)) & 0xFF) << 16)
        | (((binarray[i + 1 >> 2] >> 8 * ((i + 1) % 4)) & 0xFF) << 8)
        | ((binarray[i + 2 >> 2] >> 8 * ((i + 2) % 4)) & 0xFF);
                for (var j = 0; j < 4; j++)
                {
                    if (i * 8 + j * 6 > binarray.length * 32) str += b64pad;
                    else str += tab.charAt((triplet >> 6 * (3 - j)) & 0x3F);
                }
            }
            return str;
        };
        return hex_md5(t).toUpperCase();
    }
});




$.extend({
    /**
        统一的全选框设置
        作者：戴天辰
        调用示例：
        $(function(){
                 var $chkAllObj = $("你的全选框对象");
                var $chkObjs = $("你的复选框对象");
                $.setCheckBox($chkAllObj,$chkObjs);
        });
    */
    setCheckBox: function ($chkAllObj, $chkObjs)
    {
        $chkAllObj.on("click", function ()
        {
            $chkObjs.prop("checked", $(this).prop("checked"));
        });

        $chkObjs.on("click", function ()
        {
            $chkAllObj.prop("checked", $chkObjs.not(":checked").length === 0);
        });
    },
});





$.extend({
    /**
    统一的编辑弹出对话框
    作者：戴天辰
    调用示例：
     $.showEditDialog("对话框内的界面url",参数,"对话框标题",function(){
                    关闭对话框后的回调
            },宽度,是否是模式对话框,http方法);
*/
    showEditDialog: function (url, data, dialogTitle, closeCallback, width, modal, httpMethod)
    {

        var $editWin = $("#divAsiatekEditWin");
        if ($editWin.length == 0)
        {
            $editWin = $('<div id="divAsiatekEditWin" style="display:none;"></div>').appendTo("body");
        }
        if (httpMethod == undefined)
        {
            httpMethod = "GET";
        }

        $.ajax({
            type: httpMethod,
            url: url,
            data: data,
            success: function (result)
            {
                if ($.handleAjaxError(result))
                {
                    return;
                }
                $editWin.html(result);
                if (modal == undefined || modal == null)
                {
                    modal = true;
                }
                if (width == undefined)
                {
                    width = "Auto";
                }
                $editWin.dialog({
                    appendTo: "body",
                    title: dialogTitle,
                    modal: modal,
                    width: width,
                    resizable: false,
                    close: function ()
                    {
                        $editWin.remove();
                        if (typeof (closeCallback) == "function")
                        {
                            closeCallback();
                        }
                    }
                });
            }
        });
        return false;
    }
});









/*
通用的对话框
作者：戴天辰
调用示例：
$.showCommonDialog("显示内容","标题",{按钮})
如：
$.showCommonDialog("输入有误","提示",{
                按钮1: function ()
                {
                    按钮1操作
                }
            })
*/
$.extend({
    showCommonDialog: function (message, title, buttons, modal, $appendElement)
    {
        if (message)
        {
            if ($appendElement)
            {
                $appendElement.append($("<div id='dvMessage'></div>"));

            } else
            {
                $("body").append($("<div id='dvMessage'></div>"));
            }
            if (modal == undefined || modal == null)
            {
                modal = true;
            }
            $("#dvMessage").html(message).dialog({
                modal: modal,
                resizable: false,
                title: title,
                close: function () { $(this).remove(); },
                buttons: buttons
            });
        }
    }
});



//显示提示对话框
$.extend({
    showPromptDialog: function (message, title, closeText, closeCallBack, modal, $appendElement, dialogDivID)
    {
        if (message)
        {
            var _divID = dialogDivID ? dialogDivID : "dvMessage";
            if ($("#" + _divID + ""))
            {
                $("#" + _divID + "").dialog("close");
            }
            if ($appendElement)
            {
                $appendElement.append($("<div id='" + _divID + "'></div>"));
            } else
            {
                $("body").append($("<div id='" + _divID + "'></div>"));
            }
            if (modal == undefined || modal == null)
            {
                modal = true;
            }
            var options = {
                modal: modal,
                resizable: false,
                title: title,
                close: function ()
                {
                    $(this).remove();
                    if (typeof (closeCallBack) == "function")
                    {
                        closeCallBack();
                    }
                },
            };
            if (closeText)
            {
                options.buttons = [{
                    text: closeText,
                    click: function ()
                    {
                        $(this).dialog("close");
                        $(this).remove();
                        if (typeof (closeCallBack) == "function")
                        {
                            closeCallBack();
                        }
                    }
                }];
            }
            $("#" + _divID + "").html(message).dialog(options);
        }
    }
});






//显示通用的模式确认对话框
$.extend({
    showConfirmDialog: function (message, title, confirmText, cancelText, confirmCallBack, cancelCallBack, $appendElement)
    {
        if (message)
        {
            if ($("#dvMessage"))
            {
                $("#dvMessage").dialog("close");
            }
            if ($appendElement)
            {
                $appendElement.append($("<div id='dvMessage'></div>"));
            } else
            {
                $("body").append($("<div id='dvMessage'></div>"));
            }
            $("#dvMessage").html(message).dialog({
                modal: true,
                resizable: false,
                title: title,
                close: function () { $(this).remove(); },
                buttons: [
                    {
                        text: cancelText,
                        click: function ()
                        {
                            $(this).dialog("close");
                            //$(this).remove();
                            if (typeof (cancelCallBack) == "function")
                            {
                                cancelCallBack();
                            }
                        }
                    },
                    {
                        text: confirmText,
                        click: function ()
                        {
                            $(this).dialog("close");
                            $(this).remove();
                            if (typeof (confirmCallBack) == "function")
                            {
                                confirmCallBack();
                            }
                        }
                    },
                ]
            });
        }
    }
});





//修正动态表单无法使用非介入验证的问题
$.fn.extend({
    fixUnobtrusiveValidation: function (submitHandler)
    {
        this.removeData("validator").removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse(this);
        if (submitHandler)
        {
            this.data("validator").settings.submitHandler = submitHandler;
        }
    }
});











$.extend({
    //删除左边所有空格
    ltrim: function (str)
    {
        return str.replace(/(^\s*)/g, "");
    },
    //删除右边所有空格
    rtrim: function (str)
    {
        return str.replace(/(\s*$)/g, "");
    }
});





$.fn.extend({
    /**
    统一的带复选框的可展开与收缩的树状图
    作者：戴天辰
    调用示例：
    $(包含树状图的DOM元素).setTreeStyle();
*/
    setCheckBoxTreeStyle: function (hideChildren)
    {
        var $chkBoxes = this.find(":checkbox");

        $chkBoxes.on("change", function ()
        {
            var $obj = $(this);
            var value = $obj.prop("checked");
            //当前复选框子节点复选框的选中状态等于当前复选框的选中状态，并且继续触发change事件
            $obj.siblings("ul").children("li").children(":checkbox").prop("checked", value).trigger("change", value);
            //setParentStatus($obj);//注释该行代码，原本的设计是，如果一个大的功能下，子功能一个都没有分配，那么父功能也无法分配
            //但是比如实时监控的查岗，紧急报警之类的功能，即使不分配，实时监控也必须能够分配给用户
            //所有这里取消这个限制
        });


        var $chksiblingLabel = $chkBoxes.siblings("label");
        $chksiblingLabel.each(function ()
        {
            var $currentObj = $(this);
            var $ul = $currentObj.siblings("ul");
            if ($ul.length != 0)
            {
                $currentObj.addClass("colorBlue");
                $currentObj.hover(function ()
                {
                    $currentObj.toggleClass("colorOrange");
                });
                if (hideChildren == undefined || hideChildren === true)
                {
                    $ul.hide();
                }
            }
        });

        $chksiblingLabel.on("click", function ()
        {
            var $currentObj = $(this);
            var $siblingUL = $currentObj.siblings("ul");
            var len = $siblingUL.length;
            if (len === 0)
            {
                return;
            }
            $siblingUL.slideToggle();
        });
    }
});


//设置父节点状态
var setParentStatus = function ($obj)
{
    //获取上级节点的复选框
    var $parentChk = $obj.parent("li").parent("ul").siblings(":checkbox");
    if ($parentChk.length == 0)
    {
        return;
    }
    //当前节点所在父节点下的所有子节点的复选框（包括当前节点自身）
    var $sibChks = $parentChk.siblings("ul").find(":checkbox");
    //当前节点所在父节点下的所有未选中的复选框
    var $sibNotCheckedChks = $sibChks.not(":checked");
    //如果子节点的数目不等于未选中的数目，则父节点复选框保持选中
    var result = $sibChks.length != $sibNotCheckedChks.length;
    $parentChk.prop("checked", result);
    setParentStatus($parentChk);
}


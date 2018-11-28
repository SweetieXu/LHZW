$(function ()
{
    var $conObj = $("#asiatekPagerContent");
    var $allLinkObj = $("#asiatekPager a");
    //$allLinkObj.addClass("asiatekPagerDefault");
    var $currentPageObj = $("a.asiatekPagerSelected");
    if ($currentPageObj && $currentPageObj.text() != "1")
    {
        if ($currentPageObj.length != 0)
        {
            var left = $currentPageObj[0].offsetLeft - 35;
            var tmp = left - $conObj.width();
            $conObj.scrollLeft(((left + tmp) / 2));
        }
    }
});


$(document).ajaxSend(function (event, jqxhr, settings)
{
    //由于非介入式ajax的get请求会在末尾附加如X-Requested-With=XMLHttpRequest&_=1476756293265
    //的参数，由于1476756293265数字每次都变化，导致如果是get请求计算设置了浏览器缓存也无法缓存的情况
    //因此这里如果发现请求的url中包含X-Requested-With=XMLHttpRequest&_=  
    //就截取掉最后的&_=1476756293265
    var _url = settings.url;
    var _reg = new RegExp("X-Requested-With=XMLHttpRequest&_=");
    if (_reg.test(_url))
    {
        var _index = _url.lastIndexOf('&');
        var _newUrl = _url.substring(0, _index);
        settings.url = _newUrl;
    }
});

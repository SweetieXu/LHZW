//亚士德DOVE新增的验证规则 包含非注入式代码


//**********后台新增了自定义的RequiredIfTrueAttribute**********
//以下代码为该验证特性的客户端逻辑

//添加新的requirediftrue验证方法到validator对象
$.validator.addMethod('requirediftrue',
    //value：被验证元素的当前值；
    //element：被验证的当前元素；
    //parameters：为该方法指定的参数，
    //其属性名正好是 非注入式data-val-requirediftrue 标签属性的data-val-requirediftrue-dependentproperty
    function (value, element, parameters)
    {
        var name = parameters.dependentproperty;//依赖属性所在HTML标签的name


        var _target = $("input[name='" + name + "']");
        var actualvalue = _target.prop("checked");

        var _this = this;

        if (_target.not(".validate-requirediftrue-change").length)
        {
            _target.addClass("validate-requirediftrue-change").on("change.validate-requirediftrue-change", function ()
            {
                $(element).valid();
            });
        }

        if (true === actualvalue)//如果选中复选框
        {
            //调用默认的required验证
            return $.validator.methods.required.call(this, value, element, parameters);
        }
        return true;
    }
);
//requirediftrue验证方法的非注入式适配器
$.validator.unobtrusive.adapters.add('requirediftrue', ['dependentproperty'], function (options)
{
    options.rules['requirediftrue'] = {
        dependentproperty: options.params['dependentproperty']
    };
    options.messages['requirediftrue'] = options.message;
});
//################################################################



//添加新的requiredifnottrue验证方法到validator对象
$.validator.addMethod('requiredifnottrue',
    //value：被验证元素的当前值；
    //element：被验证的当前元素；
    //parameters：为该方法指定的参数，
    //其属性名正好是 非注入式data-val-requiredifnottrue 标签属性的data-val-requiredifnottrue-dependentproperty
    function (value, element, parameters)
    {
        var name = parameters.dependentproperty;//依赖属性所在HTML标签的name


        var _target = $("input[name='" + name + "']");
        var actualvalue = _target.prop("checked");

        var _this = this;

        if (_target.not(".validate-requiredifnottrue-change").length)
        {
            _target.addClass("validate-requiredifnottrue-change").on("change.validate-requiredifnottrue-change", function ()
            {
                $(element).valid();
            });
        }

        if (false === actualvalue)//如果未选中复选框
        {
            //调用默认的required验证
            return $.validator.methods.required.call(this, value, element, parameters);
        }
        return true;
    }
);
//requiredifnottrue验证方法的非注入式适配器
$.validator.unobtrusive.adapters.add('requiredifnottrue', ['dependentproperty'], function (options)
{
    options.rules['requiredifnottrue'] = {
        dependentproperty: options.params['dependentproperty']
    };
    options.messages['requiredifnottrue'] = options.message;
});
//################################################################




//**********后台新增了自定义的ChinesePhoneAttribute**********
//以下代码为该验证特性的客户端逻辑

//添加新的chinesePhone验证方法到validator对象
$.validator.addMethod('chinesephone',
    //value：被验证元素的当前值；
    //element：被验证的当前元素；
    function (value, element)
    {
        if (this.optional(element))
        {
            return true;
        }
        var telReg = /^(0\d{2,3}-)?\d{7,8}(-\d{3,4})?$/;//座机
        var mobilieReg = /^1[0-9]{10}$/;//手机
        return telReg.test(value) || mobilieReg.test(value);
    }
);
//chinesephone验证方法的非注入式适配器
$.validator.unobtrusive.adapters.addBool("chinesephone");

//################################################################



//**********后台新增了自定义的AsiatekRegularExpressionAttribute**********
//以下代码为该验证特性的客户端逻辑

//添加新的asiatekregex验证方法到validator对象
$.validator.addMethod('asiatekregex',
    //value：被验证元素的当前值；
    //element：被验证的当前元素；
    function (value, element, params)
    {
        var match;
        match = new RegExp(params).exec(value);
        return (match && (match.index === 0) && (match[0].length === value.length));
    }
);
//asiatekregex验证方法的非注入式适配器
$.validator.unobtrusive.adapters.addSingleVal("asiatekregex", "pattern");

//################################################################






//**********后台新增了自定义的LegalDateAttribute**********
//以下代码为该验证特性的客户端逻辑

//添加新的legaldate验证方法到validator对象
$.validator.addMethod('legaldate',
    //value：被验证元素的当前值；
    //element：被验证的当前元素；
    function (value, element)
    {
        if (this.optional(element))
        {
            return true;
        }
        var reg = /^((?:19|20)\d\d)-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$/;
        var r = value.match(reg);
        if (r == null)
        {
            return false;
        }
        var tempDate = new Date(r[1], r[2] - 1, r[3]);
        var result = (tempDate.getFullYear() == r[1] && (tempDate.getMonth() + 1) == r[2] && tempDate.getDate() == r[3]);
        return result;
    }
);
//chinesephone验证方法的非注入式适配器
$.validator.unobtrusive.adapters.addBool("legaldate");

//################################################################





//**********后台新增了自定义的DateRangeAttribute**********
//以下代码为该验证特性的客户端逻辑

//添加新的daterange验证方法到validator对象
$.validator.addMethod('daterange',
    //value：被验证元素的当前值；
    //element：被验证的当前元素；
    //parameters：为该方法指定的参数，
    //其属性名正好是 非注入式data-val-requirediftrue 标签属性的data-val-requirediftrue-dependentproperty
    function (value, element, parameters)
    {
        if (this.optional(element))
        {
            return true;
        }

        var currentDate = new Date(value);
        currentDate = new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate());

        var min = parseInt(parameters.min);
        var max = parseInt(parameters.max);

        var now = new Date();
        var date = now.getDate();

        if (!isNaN(min) && !isNaN(max))
        {
            var minDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            minDate.setDate(date + min);

            var maxDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            maxDate.setDate(date + max);

            return currentDate >= minDate && currentDate <= maxDate;
        } else if (!isNaN(min))
        {
            var minDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            minDate.setDate(date + min);
            return currentDate >= minDate;
        } else
        {
            var maxDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
            maxDate.setDate(date + max);
            return currentDate <= maxDate;
        }
        return true;


    }
);
//daterange验证方法的非注入式适配器
$.validator.unobtrusive.adapters.add('daterange', ['min', 'max'], function (options)
{
    options.rules['daterange'] = {
        min: options.params['min'],
        max: options.params['max']
    };
    options.messages['daterange'] = options.message;
});
//################################################################


//**********后台新增了自定义的UnMatchedAttribute**********
//以下代码为该验证特性的客户端逻辑

//添加新的unmatched验证方法到validator对象
$.validator.addMethod('unmatched',
    //value：被验证元素的当前值；
    //element：被验证的当前元素；
    //parameters：为该方法指定的参数，
    //其属性名正好是 非注入式data-val-unmatched 标签属性的data-val-requirediftrue-otherproperty
    function (value, element, parameters)
    {
        var id = '#' + parameters.otherproperty;//比较属性所在HTML标签的ID
        var actualvalue = $.trim($(id).val());
        if (value == actualvalue)
        {
            return false;
        }
        return true;
    }
);
//unmatched验证方法的非注入式适配器
$.validator.unobtrusive.adapters.add('unmatched', ['otherproperty'], function (options)
{
    options.rules['unmatched'] = {
        otherproperty: options.params['otherproperty']
    };
    options.messages['unmatched'] = options.message;
});
//################################################################


//添加新的legaldatetime验证方法到validator对象
$.validator.addMethod('legaldatetime',
    //value：被验证元素的当前值；
    //element：被验证的当前元素；
    function (value, element)
    {
        debugger;
        if (this.optional(element))
        {
            return true;
        }
        var reg = "^([2][0][0-9][0-9])(\-)([1-9]|[1][0-2])(\-)([0-2][0-9]|[3][0-1])( )([0-1][0-9]|[2][0-3])(:)([0-5][0-9])(:)([0-5][0-9])$";
        var r = value.match(reg);
        if (r == null)
        {
            return false;
        }
        var tempDate = new Date(r[1], r[3] - 1, r[5], r[7], r[9], r[11]);
        var result = (tempDate.getFullYear() == r[1] && (tempDate.getMonth() + 1) == r[3] && tempDate.getDate() == r[5] && tempDate.getHours() == r[7] && tempDate.getMinutes() == r[9] && tempDate.getSeconds() == r[11]);
        return result;
    }
);
//chinesephone验证方法的非注入式适配器
$.validator.unobtrusive.adapters.addBool("legaldatetime");
//################################################################


//添加新的startenddatetime验证方法到validator对象
$.validator.addMethod('startenddatetime',
    //value：被验证元素的当前值；
    //element：被验证的当前元素；
    //parameters：为该方法指定的参数，
    //其属性名正好是 非注入式data-val-startenddatetime 标签属性的data-val-startenddatetime-dependentproperty
    function (value, element, parameters)
    {

        var name = parameters.dependentproperty;//依赖属性所在HTML标签的ID

        var target = $("input[name='" + name + "']");
        //console.log(target);
        if (target.not(".validate-startenddatetime-change").length)
        {
            //console.log(value);
            target.addClass("validate-startenddatetime-change").on("change.validate-startenddatetime-change", function ()
            {
                $(element).valid();
            });
        }

        var actualvalue = $("input[name='" + name + "']").val();
        var reg = "^([2][0][0-9][0-9])(\-)([1-9]|[1][0-2])(\-)([0-2][0-9]|[3][0-1])( )([0-1][0-9]|[2][0-3])(:)([0-5][0-9])(:)([0-5][0-9])$";
        if (!actualvalue.match(reg))
        {
            return false;
        }
        if (!value.match(reg))
        {
            return false;
        }
        var startTime = new Date(Date.parse(actualvalue.replace(/-/g, "/"))).getTime();
        var endTime = new Date(Date.parse(value.replace(/-/g, "/"))).getTime();
        var dates = Math.abs((startTime - endTime)) / (1000 * 60 * 60 * 24);
        if (startTime > endTime)
        {
            return false;
        }
        if (Math.round(dates) > 30)
        {
            return false;
        }
        return true;
    }
);
//startenddatetime验证方法的非注入式适配器
$.validator.unobtrusive.adapters.add('startenddatetime', ['dependentproperty'], function (options)
{
    options.rules['startenddatetime'] = {
        dependentproperty: options.params['dependentproperty']
    };
    options.messages['startenddatetime'] = options.message;
});
//################################################################

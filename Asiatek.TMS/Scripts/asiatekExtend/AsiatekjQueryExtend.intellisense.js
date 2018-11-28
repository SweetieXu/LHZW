intellisense.annotate(jQuery, {
    'cookie': function ()
    {
        /// <signature>
        ///   <summary>返回cookie的json格式对象</summary>
        ///   <param name="ck" type="Object">Cookie对象</param>
        ///   <returns type="Object" />
        /// </signature>
    },
    'handleAjaxError': function ()
    {
        /// <signature>
        ///   <summary>
        ///    针对TMS的Ajax操作可能的异常结果，定义一个统一的处理逻辑 [True：表示发生错误]
        ///</summary>
        ///   <param name="result" type="Object">Ajax操作返回结果</param>
        ///   <returns type="Boolean" />
        /// </signature>
    },
    'setCheckBox': function ()
    {
        /// <signature>
        ///   <summary>
        ///    统一的复选框设置
        ///</summary>
        ///   <param name="$chkAllObj" type="jQuery">全选框对象</param>
        ///   <param name="$chkObjs" type="jQuery">复选框对象</param>
        ///   <returns type="Boolean" />
        /// </signature>
    },
    'showCommonDialog': function ()
    {
        /// <signature>
        ///   <summary>
        ///    显示通用模式对话框
        ///</summary>
        ///   <param name="message" type="String">对话框内容</param>
        ///   <param name="title" type="String">对话框标题</param>
        ///   <param name="buttons" type="Array">对话框按钮</param>
        ///   <param name="modal" type="Boolean">是否模式对话框(默认为true)</param>
        ///   <param name="$appendElement" type="jQuery">追加对话框的元素(默认为body)</param>
        /// </signature>
    },
    'showPromptDialog': function ()
    {
        /// <signature>
        ///   <summary>
        ///    显示通用提示对话框
        ///</summary>
        ///   <param name="message" type="String">对话框内容</param>
        ///   <param name="title" type="String">对话框标题</param>
        ///   <param name="closeText" type="String">关闭按钮文字</param>
        ///   <param name="closeCallBack" type="Function">关闭后执行的回调函数</param>
        ///   <param name="modal" type="Boolean">是否模式对话框(默认为true)</param>
        ///   <param name="$appendElement" type="jQuery">追加对话框的元素(默认为body)</param>
        ///   <param name="dialogDivID" type="String">用于创建对话框的DIV的ID</param>
        /// </signature>
    },
    'showConfirmDialog': function ()
    {
        /// <signature>
        ///   <summary>
        ///   显示通用模式确认对话框
        ///</summary>
        ///   <param name="message" type="String">对话框内容</param>
        ///   <param name="title" type="String">对话框标题</param>
        ///   <param name="confirmText" type="String">确认按钮文字</param>
        ///   <param name="cancelText" type="String">取消按钮文字</param>
        ///   <param name="confirmCallBack" type="Function">确认后执行的回调函数</param>
        ///   <param name="cancelCallBack" type="Function">取消后执行的回调函数</param>
        ///   <param name="$appendElement" type="jQuery">追加对话框的元素(默认为body)</param>
        /// </signature>
    },
    'showEditDialog': function ()
    {
        /// <signature>
        ///   <summary>
        ///  显示通用编辑对话框
        ///</summary>
        ///   <param name="url" type="String">对话框内容地址</param>
        ///   <param name="data" type="Object">内容URL需要的参数</param>
        ///   <param name="dialogTitle" type="String">对话框标题</param>
        ///   <param name="closeCallback" type="Function">关闭对话框后的回调函数</param>
        ///   <param name="width" type="Number">对话框宽度</param>
        ///   <param name="modal" type="Boolean">是否是模式对话框（默认是）</param>
        ///   <param name="httpMethod" type="String">打开内容地址的HTTP方法（Get或POST）</param>
        /// </signature>
    },
    'ltrim': function ()
    {
        /// <signature>
        ///   <summary>移除字符串左边空格</summary>
        ///   <param name="str" type="String">待移除左边空格的字符串</param>
        ///   <returns type="String" />
        /// </signature>
    },
    'rtrim': function ()
    {
        /// <signature>
        ///   <summary>移除字符串右边空格</summary>
        ///   <param name="str" type="String">待移除右边空格的字符串</param>
        ///   <returns type="String" />
        /// </signature>
    }
});



intellisense.annotate(jQuery.fn, {
    'fixUnobtrusiveValidation': function ()
    {
        /// <signature>
        ///   <summary>修正通过Ajax动态获取表单后，无法执行非介入式验证的问题</summary>
        ///   <param name="submitHandler" type="Function">验证成功后的提交函数</param>
        /// </signature>
    },
});








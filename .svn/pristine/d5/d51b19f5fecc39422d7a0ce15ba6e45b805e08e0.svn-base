using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    /// <summary>
    /// 操作结果
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// 操作结果
        /// true：成功
        /// false：失败
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 操作结果对应的国际化提示内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 操作后需要重定向的URL
        /// </summary>
        public string Url { get; set; }
    }


    /// <summary>
    /// 查询操作结果
    /// </summary>
    /// <typeparam name="T">查询结果类型</typeparam>
    public class SelectResult<T>
    {
        /// <summary>
        /// 查询结果
        /// 发生错误时为NULL
        /// </summary>
        public T DataResult { get; set; }
        /// <summary>
        /// 查询结果对应的国际化提示内容
        /// </summary>
        public string Message { get; set; }
    }
}

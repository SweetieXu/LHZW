using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiatek.Model
{
    public class LogModel
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationTypeEnum OperationType { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperationInfo { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string FunctionName { get; set; }

    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationTypeEnum
    {
        /// <summary>
        /// 新增
        /// </summary>
        Add,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 修改
        /// </summary>
        Edit,
        /// <summary>
        /// 导入
        /// </summary>
        Import
    }
}
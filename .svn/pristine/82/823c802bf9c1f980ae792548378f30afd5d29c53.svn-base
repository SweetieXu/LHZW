using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Ajax;

namespace Asiatek.AjaxPager
{
    #region 旧版本
    public class AsiatekAjaxPagerOptions : AjaxOptions
    {
        /// <summary>
        /// 分页请求区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 分页请求控制器名称
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 分页请求动作名称
        /// </summary>
        public string ActionName { get; set; }

        public AsiatekAjaxPagerOptions() { }

        public AsiatekAjaxPagerOptions(string actionName, string controllerName, string areaName, string onSuccess, string updateTargetId)
        {
            this.ActionName = actionName;
            this.ControllerName = controllerName;
            this.AreaName = areaName;
            this.OnSuccess = OnSuccess;
            this.UpdateTargetId = updateTargetId;
        }
    }


    public class AsiatekAjaxPagerOptions2 : AsiatekAjaxPagerOptions
    {
        /// <summary>
        /// 标识搜索页的字段名（不设置则为SearchPage）
        /// 如果SearchDatasObj中包含该字段名
        /// 请设置成一样
        /// </summary>
        public string SearchPageFieldName { get; set; }


        public AsiatekAjaxPagerOptions2() { }

        public AsiatekAjaxPagerOptions2(string actionName, string controllerName, string areaName, string onSuccess, string updateTargetId, string searchPageFieldName)
            : base(actionName, controllerName, areaName, onSuccess, updateTargetId)
        {
            this.SearchPageFieldName = searchPageFieldName;
        }
    }


    public class AsiatekAjaxPagerOptions3 : AsiatekAjaxPagerOptions2
    {
        /// <summary>
        /// 分页时包含查询参数的对象
        /// </summary>
        public object SearchDatasObj { get; set; }


        public AsiatekAjaxPagerOptions3() { }

        public AsiatekAjaxPagerOptions3(string actionName, string controllerName, string areaName, string onSuccess, string updateTargetId, string searchPageFieldName, object searchDatasObj)
            : base(actionName, controllerName, areaName, onSuccess, updateTargetId, searchPageFieldName)
        {
            this.SearchDatasObj = searchDatasObj;
        }
    }
    #endregion


    #region Bootstrap版
    /// <summary>
    /// Ajax分页样式选项
    /// </summary>
    public class AjaxPagerStyleOptions
    {
        /// <summary>
        /// Bootstrap分页导航条样式枚举
        /// 默认为Middle
        /// </summary>
        public BootstrapPaginationStyleEnum PaginationStyle { get; set; }

        /// <summary>
        /// 分页导航条最多显示页码数
        /// 默认10
        /// </summary>
        public int NavShowCount { get; set; }
    }
    /// <summary>
    /// Ajax分页选项
    /// </summary>
    public class AjaxPagerOptions : AjaxOptions
    {
        /// <summary>
        /// 分页请求区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 分页请求控制器名称
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 分页请求动作名称
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 标识搜索页的字段名（默认为SearchPage）
        /// </summary>
        public string SearchPageFieldName { get; set; }
    }
    /// <summary>
    /// Bootstrap分页导航样式
    /// </summary>
    public enum BootstrapPaginationStyleEnum
    {
        Middle,
        Small,
        Large
    }
    #endregion


}

using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Asiatek.Model
{
    #region 查询
    public class ElectricFencePropertyListModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        [Display(Name = "PropertyName", ResourceType = typeof(DisplayText))]
        public string PropertyName { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
        public bool FenceState { get; set; }
        /// <summary>
        /// 有效期开始时间
        /// </summary>
        [Display(Name = "ValidStartTime", ResourceType = typeof(DisplayText))]
        public DateTime ValidStartTime { get; set; }
        /// <summary>
        /// 有效期结束时间
        /// </summary>
        [Display(Name = "ValidEndTime", ResourceType = typeof(DisplayText))]
        public DateTime ValidEndTime { get; set; }
        /// <summary>
        /// 报警类型
        /// </summary>
        [Display(Name = "AlarmType", ResourceType = typeof(DisplayText))]
        public bool? AlarmType { get; set; }
        /// <summary>
        /// 是否限速
        /// </summary>
        [Display(Name = "IsSpeed", ResourceType = typeof(DisplayText))]
        public bool IsSpeed { get; set; }
        /// <summary>
        /// 限速值
        /// </summary>
        [Display(Name = "SpeedLimit", ResourceType = typeof(DisplayText))]
        public double? MaxSpeed { get; set; }
        /// <summary>
        /// 是否周期
        /// </summary>
        [Display(Name = "IsPeriodic", ResourceType = typeof(DisplayText))]
        public bool IsPeriod { get; set; }
        /// <summary>
        /// 周几
        /// </summary>
        [Display(Name = "Week", ResourceType = typeof(DisplayText))]
        public int? Week { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        public string EndTime { get; set; }
    }

    public class ElectricFencePropertySearchModel
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        [Display(Name = "PropertyName", ResourceType = typeof(DisplayText))]
        public string PropertyName { get; set; }
    }
    #endregion

    #region  新增
    public class AddEFPropertyModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        [Display(Name = "PropertyName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddEFPropertyNameExists", "ElectricFenceProperty", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string PropertyName { get; set; }
        /// <summary>
        /// 报警类型
        /// </summary>
        [Display(Name = "AlarmType", ResourceType = typeof(DisplayText))]
        public bool? AlarmType { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool FenceState { get; set; }
        /// <summary>
        /// 有效期开始时间
        /// </summary>
        [Display(Name = "ValidStartTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string ValidStartTime { get; set; }
        /// <summary>
        /// 有效期结束时间
        /// </summary>
        [Display(Name = "ValidEndTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string ValidEndTime { get; set; }
        /// <summary>
        /// 是否限速
        /// </summary>
        [Display(Name = "IsSpeed", ResourceType = typeof(DisplayText))]
        public bool IsSpeed { get; set; }
        /// <summary>
        /// 限速值
        /// </summary>
        [Display(Name = "SpeedLimit", ResourceType = typeof(DisplayText))]
        public double? MaxSpeed { get; set; }
        /// <summary>
        /// 是否周期
        /// </summary>
        [Display(Name = "IsPeriodic", ResourceType = typeof(DisplayText))]
        public bool IsPeriod { get; set; }

        public List<EFPropertyPeriodModel> EFPropertyPeriod { get; set; }
    }

    public class EFPropertyPeriodModel
    {
        /// <summary>
        /// 电子围栏配置周期表ID
        /// </summary>
        public int PeriodID { get; set; }
        /// <summary>
        /// 电子围栏配置表ID
        /// </summary>
        public int EFPropertyID { get; set; }
        /// <summary>
        /// 周几
        /// </summary>
        [Display(Name = "Week", ResourceType = typeof(DisplayText))]
        public int? Week { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        public string EndTime { get; set; }
    }
    #endregion

    #region 修改
    public class EditEFPropertyModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        [Display(Name = "PropertyName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditEFPropertyNameExists", "ElectricFenceProperty", "Admin",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string PropertyName { get; set; }
        /// <summary>
        /// 报警类型
        /// </summary>
        [Display(Name = "AlarmType", ResourceType = typeof(DisplayText))]
        public bool? AlarmType { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool FenceState { get; set; }
        /// <summary>
        /// 有效期开始时间
        /// </summary>
        [Display(Name = "ValidStartTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public DateTime ValidStartTime { get; set; }
        /// <summary>
        /// 有效期结束时间
        /// </summary>
        [Display(Name = "ValidEndTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public DateTime ValidEndTime { get; set; }
        /// <summary>
        /// 是否限速
        /// </summary>
        [Display(Name = "IsSpeed", ResourceType = typeof(DisplayText))]
        public bool IsSpeed { get; set; }
        /// <summary>
        /// 限速值
        /// </summary>
        [Display(Name = "SpeedLimit", ResourceType = typeof(DisplayText))]
        public double? MaxSpeed { get; set; }
        /// <summary>
        /// 是否周期
        /// </summary>
        [Display(Name = "IsPeriodic", ResourceType = typeof(DisplayText))]
        public bool IsPeriod { get; set; }

        public List<EFPropertyPeriodModel> EFPropertyPeriod { get; set; }
    }

    #endregion 
}

using Asiatek.CustomDataAnnotations;
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
    public class MapRegionsListModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 区域类型（1:圆形区域、2：矩形区域、3：多边形区域）
        /// </summary>
        [Display(Name = "RegionsType", ResourceType = typeof(DisplayText))]
        public int RegionsType { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        [Display(Name = "RegionsName", ResourceType = typeof(DisplayText))]
        public string RegionsName { get; set; }

        /// <summary>
        /// 是否周期
        /// </summary>
        [Display(Name = "IsPeriodic", ResourceType = typeof(DisplayText))]
        public bool Periodic { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        public string EndTime { get; set; }

        /// <summary>
        /// 最高速度
        /// </summary>
        [Display(Name = "SpeedLimit1", ResourceType = typeof(DisplayText))]
        public double SpeedLimit { get; set; }

        /// <summary>
        /// 超速持续时间
        /// </summary>
        [Display(Name = "OverSpeedDuration", ResourceType = typeof(DisplayText))]
        public int OverSpeedDuration { get; set; }
    }

    public class MapRegionsSearchModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 区域类型（1:圆形区域、2：矩形区域、3：多边形区域）
        /// </summary>
        [Display(Name = "RegionsType", ResourceType = typeof(DisplayText))]
        public int SearchRegionsType { get; set; }

        public IEnumerable<SelectListItem> RegionsTypeSelectList { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        [Display(Name = "RegionsName", ResourceType = typeof(DisplayText))]
        public string SearchRegionsName { get; set; }

        /// <summary>
        /// 最高速度
        /// </summary>
        [Display(Name = "SpeedLimit1", ResourceType = typeof(DisplayText))]
        public float SearchSpeedLimit { get; set; }
    }

    /// <summary>
    /// 单位及其子单位拥有的区域信息模型
    /// </summary>
    public class StrucMapRegionsModel
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public long RegionID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string RegionsName { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string StrucName { get; set; }

    }
    #endregion


    #region 新增
    public class MapRegionsAddModel
    {
        /// <summary>
        /// 区域类型（1:圆形区域、2：矩形区域、3：多边形区域）
        /// </summary>
        [Display(Name = "RegionsType", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int RegionsType { get; set; }

        public IEnumerable<SelectListItem> RegionsTypeSelectList { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        [Display(Name = "RegionsName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddRegionsNameExists", "MapRegions", "TerminalSetting", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string RegionsName { get; set; }

        /// <summary>
        /// 中心点纬度
        /// </summary>
        public float CenterLatitude { get; set; }

        /// <summary>
        /// 中心点经度
        /// </summary>
        public float CenterLongitude { get; set; }

        /// <summary>
        /// 左上点纬度
        /// </summary>
        public float LeftUpperLatitude { get; set; }

        /// <summary>
        /// 左上点经度
        /// </summary>
        public float LeftUpperLongitude { get; set; }

        /// <summary>
        /// 右下点纬度
        /// </summary>
        public float RightLowerLatitude { get; set; }

        /// <summary>
        /// 右下点经度
        /// </summary>
        public float RightLowerLongitude { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// 多边形点
        /// </summary>
        public List<string> PolygonList { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        public string EndTime { get; set; }

        /// <summary>
        /// 是否周期
        /// </summary>
        [Display(Name = "IsPeriodic", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool Periodic { get; set; }

        /// <summary>
        /// 最高速度
        /// </summary>
        [Display(Name = "SpeedLimit1", ResourceType = typeof(DisplayText))]
        public float SpeedLimit { get; set; }

        /// <summary>
        /// 超速持续时间
        /// </summary>
        [Display(Name = "OverSpeedDuration", ResourceType = typeof(DisplayText))]
        public int OverSpeedDuration { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(1000,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }
    #endregion


    #region 修改
    public class MapRegionsEditModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 区域类型（1:圆形区域、2：矩形区域、3：多边形区域）
        /// </summary>
        [Display(Name = "RegionsType", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int RegionsType { get; set; }

        public IEnumerable<SelectListItem> RegionsTypeSelectList { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        [Display(Name = "RegionsName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditRegionsNameExists", "MapRegions", "TerminalSetting",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string RegionsName { get; set; }

        /// <summary>
        /// 中心点纬度
        /// </summary>
        public float CenterLatitude { get; set; }

        /// <summary>
        /// 中心点经度
        /// </summary>
        public float CenterLongitude { get; set; }

        /// <summary>
        /// 左上点纬度
        /// </summary>
        public float LeftUpperLatitude { get; set; }

        /// <summary>
        /// 左上点经度
        /// </summary>
        public float LeftUpperLongitude { get; set; }

        /// <summary>
        /// 右下点纬度
        /// </summary>
        public float RightLowerLatitude { get; set; }

        /// <summary>
        /// 右下点经度
        /// </summary>
        public float RightLowerLongitude { get; set; }

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// 多边形点
        /// </summary>
        public List<string> PolygonList { get; set; }

        /// <summary>
        /// 多边形点
        /// </summary>
        public string RePolygonList { get; set; }

        /// <summary>
        /// 定位点序号—地图区域明细表
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// 纬度—地图区域明细表
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// 经度—地图区域明细表
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
        public string StartTime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        public string EndTime { get; set; }

        /// <summary>
        /// 是否周期
        /// </summary>
        [Display(Name = "IsPeriodic", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool Periodic { get; set; }

        /// <summary>
        /// 最高速度
        /// </summary>
        [Display(Name = "SpeedLimit1", ResourceType = typeof(DisplayText))]
        public float SpeedLimit { get; set; }

        /// <summary>
        /// 超速持续时间
        /// </summary>
        [Display(Name = "OverSpeedDuration", ResourceType = typeof(DisplayText))]
        public int OverSpeedDuration { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

    }
    #endregion
}

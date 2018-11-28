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
    public class MapLinesListModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 路线名称
        /// </summary>
        [Display(Name = "LinesName", ResourceType = typeof(DisplayText))]
        public string LinesName { get; set; }

        /// <summary>
        /// 时间起
        /// </summary>
        [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 时间迄
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        public DateTime EndTime { get; set; }
    }

    public class MapLinesSearchModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 路线名称
        /// </summary>
        [Display(Name = "LinesName", ResourceType = typeof(DisplayText))]
        public string LinesName { get; set; }
    }

    public class MapLinesDetailModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 路线名称
        /// </summary>
        [Display(Name = "LinesName", ResourceType = typeof(DisplayText))]
        public string LinesName { get; set; }

        /// <summary>
        /// 路线类型（4:路线）
        /// </summary>
        public int LinesType { get; set; }

        /// <summary>
        /// 时间起
        /// </summary>
        [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
        public string StartTime { get; set; }

        /// <summary>
        /// 时间迄
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        public string EndTime { get; set; }

        public int OrderID { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [Display(Name = "Latitude", ResourceType = typeof(DisplayText))]
        public float Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [Display(Name = "Longitude", ResourceType = typeof(DisplayText))]
        public float Longitude { get; set; }

        /// <summary>
        /// 路段宽度
        /// </summary>
        [Display(Name = "RoadWidth", ResourceType = typeof(DisplayText))]
        public float? RoadWidth { get; set; }

        /// <summary>
        /// 是否判断行驶时间
        /// </summary>
        [Display(Name = "IsCheckTime", ResourceType = typeof(DisplayText))]
        public bool? IsCheckTime { get; set; }

        /// <summary>
        /// 行驶过长阈值
        /// </summary>
        [Display(Name = "MaxSecond", ResourceType = typeof(DisplayText))]
        public int? MaxSecond { get; set; }

        /// <summary>
        /// 行驶不足阈值
        /// </summary>
        [Display(Name = "MinSecond", ResourceType = typeof(DisplayText))]
        public int? MinSecond { get; set; }

        /// <summary>
        /// 是否判断行驶速度
        /// </summary>
        [Display(Name = "IsCheckSpeed", ResourceType = typeof(DisplayText))]
        public bool? IsCheckSpeed { get; set; }

        /// <summary>
        /// 最高速度
        /// </summary>
        [Display(Name = "SpeedLimit1", ResourceType = typeof(DisplayText))]
        public float? SpeedLimit { get; set; }

        /// <summary>
        /// 超速持续时间
        /// </summary>
        [Display(Name = "OverSpeedDuration", ResourceType = typeof(DisplayText))]
        public int? OverSpeedDuration { get; set; }
    }

    /// <summary>
    /// 单位及其子单位拥有的路线信息模型
    /// </summary>
    public class StrucMapLinesModel
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public long LineID { get; set; }
        /// <summary>
        /// 路线名称
        /// </summary>
        public string LinesName { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string StrucName { get; set; }

    }

    #endregion


    #region 新增
    public class MapLinesAddModel
    {
        /// <summary>
        /// 路线类型（4:路线）
        /// </summary>
        public int LinesType { get; set; }

        /// <summary>
        /// 路线名称
        /// </summary>
        [Display(Name = "LinesName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddLinesNameExists", "MapLines", "TerminalSetting", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string LinesName { get; set; }

        /// <summary>
        /// 时间起
        /// </summary>
        [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string StartTime { get; set; }

        /// <summary>
        /// 时间迄
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string EndTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(1000,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

        public List<MapLinesDetailsAddModel> MapLinesDetails { get; set; }

        //public MapLinesDetailsAddModel[] MapLinesDetails { get; set; }
    }

    public class MapLinesDetailsAddModel
    {
        /// <summary>
        /// 纬度
        /// </summary>
        [Display(Name = "Latitude", ResourceType = typeof(DisplayText))]
        public float Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [Display(Name = "Longitude", ResourceType = typeof(DisplayText))]
        public float Longitude { get; set; }

        /// <summary>
        /// 路段宽度
        /// </summary>
        [Display(Name = "RoadWidth", ResourceType = typeof(DisplayText))]
        public float? RoadWidth { get; set; }

        /// <summary>
        /// 是否判断行驶时间
        /// </summary>
        [Display(Name = "IsCheckTime", ResourceType = typeof(DisplayText))]
        public bool? IsCheckTime { get; set; }

        /// <summary>
        /// 行驶过长阈值
        /// </summary>
        [Display(Name = "MaxSecond", ResourceType = typeof(DisplayText))]
        public int? MaxSecond { get; set; }

        /// <summary>
        /// 行驶不足阈值
        /// </summary>
        [Display(Name = "MinSecond", ResourceType = typeof(DisplayText))]
        public int? MinSecond { get; set; }

        /// <summary>
        /// 是否判断行驶速度
        /// </summary>
        [Display(Name = "IsCheckSpeed", ResourceType = typeof(DisplayText))]
        public bool? IsCheckSpeed { get; set; }

        /// <summary>
        /// 最高速度
        /// </summary>
        [Display(Name = "SpeedLimit1", ResourceType = typeof(DisplayText))]
        public float? SpeedLimit { get; set; }

        /// <summary>
        /// 超速持续时间
        /// </summary>
        [Display(Name = "OverSpeedDuration", ResourceType = typeof(DisplayText))]
        public int? OverSpeedDuration { get; set; }
    }
    #endregion


    #region 修改
    public class MapLinesEditModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 路线类型（4:路线）
        /// </summary>
        public int LinesType { get; set; }

        /// <summary>
        /// 路线名称
        /// </summary>
        [Display(Name = "LinesName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditLinesNameExists", "MapLines", "TerminalSetting",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string LinesName { get; set; }

        /// <summary>
        /// 时间起
        /// </summary>
        [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string StartTime { get; set; }

        /// <summary>
        /// 时间迄
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string EndTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(1000,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }


        public List<MapLinesDetailsEditModel> MapLinesDetails { get; set; }

        public string MapLinesPoints { get; set; }
    }

    public class MapLinesDetailsEditModel
    {
        public int OrderID { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [Display(Name = "Latitude", ResourceType = typeof(DisplayText))]
        public float Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [Display(Name = "Longitude", ResourceType = typeof(DisplayText))]
        public float Longitude { get; set; }

        /// <summary>
        /// 路段宽度
        /// </summary>
        [Display(Name = "RoadWidth", ResourceType = typeof(DisplayText))]
        public float? RoadWidth { get; set; }

        /// <summary>
        /// 是否判断行驶时间
        /// </summary>
        [Display(Name = "IsCheckTime", ResourceType = typeof(DisplayText))]
        public bool? IsCheckTime { get; set; }

        /// <summary>
        /// 行驶过长阈值
        /// </summary>
        [Display(Name = "MaxSecond", ResourceType = typeof(DisplayText))]
        public int? MaxSecond { get; set; }

        /// <summary>
        /// 行驶不足阈值
        /// </summary>
        [Display(Name = "MinSecond", ResourceType = typeof(DisplayText))]
        public int? MinSecond { get; set; }

        /// <summary>
        /// 是否判断行驶速度
        /// </summary>
        [Display(Name = "IsCheckSpeed", ResourceType = typeof(DisplayText))]
        public bool? IsCheckSpeed { get; set; }

        /// <summary>
        /// 最高速度
        /// </summary>
        [Display(Name = "SpeedLimit1", ResourceType = typeof(DisplayText))]
        public float? SpeedLimit { get; set; }

        /// <summary>
        /// 超速持续时间
        /// </summary>
        [Display(Name = "OverSpeedDuration", ResourceType = typeof(DisplayText))]
        public int? OverSpeedDuration { get; set; }
    }
    #endregion
}

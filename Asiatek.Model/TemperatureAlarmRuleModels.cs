using Asiatek.CustomDataAnnotations;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    #region 查询
    public class TemperatureAlarmRuleListModel
    {
        public int ID { get; set; }
        [Display(Name = "RuleName", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public string AffiliatedStrucName { get; set; }
    }

    public class TemperatureAlarmRuleSearchModel
    {
        [Display(Name = "RuleName", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public int AffiliatedStrucID { get; set; }
    }
    #endregion

    #region 新增
    public class TemperatureAlarmRuleAddModel
    {
        /// <summary>
        /// 温度报警规则名称
        /// </summary>
        [Display(Name = "RuleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddTemperatureAlarmRuleNameExists", "TemperatureAlarmRule", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string Name { get; set; }
        /// <summary>
        /// 报警规则所属单位ID
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? AffiliatedStrucID { get; set; }
        /// <summary>
        /// 创建用户ID
        /// </summary>
        public int CreateUserID { get; set; }
        /// <summary>
        /// 启用报警的开始时间
        /// </summary>
        [Display(Name = "AlarmEnableStartTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public string StartTime { get; set; }
        /// <summary>
        /// 启用报警的结束时间
        /// </summary>
        [Display(Name = "AlarmEnableEndTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public string EndTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<TemperatureAlarmRuleDetailModel> TemperatureAlarmRuleDetails { get; set; }

    }
    /// <summary>
    /// 温度报警规则明细模型
    /// </summary>
    public class TemperatureAlarmRuleDetailModel
    {
        public string SensorName { get; set; }
        public int SensorCode { get; set; }
        [Display(Name = "InstallationPosition", ResourceType = typeof(DisplayText))]
        public string InstallationPosition { get; set; }
        [Display(Name = "LowestTemperature", ResourceType = typeof(DisplayText))]
        public double? LowestTemperature { get; set; }
        [Display(Name = "HighestTemperature", ResourceType = typeof(DisplayText))]
        public double? HighestTemperature { get; set; }
        [Display(Name = "EnableAlarm", ResourceType = typeof(DisplayText))]
        public bool Enabled { get; set; }
    }

    #endregion

    #region 完全编辑
    public class TemperatureAlarmRuleEditFlatModel
    {
        /// <summary>
        /// 温度报警规则ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 温度报警规则名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 报警规则所属单位ID
        /// </summary>
        public int AffiliatedStrucID { get; set; }
        /// <summary>
        /// 报警规则所属单位名称
        /// </summary>
        public string AffiliatedStrucName { get; set; }
        /// <summary>
        /// 启用报警的开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 启用报警的结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 温度传感器编号
        /// </summary>
        public int SensorCode { get; set; }
        /// <summary>
        /// 温度传感器名称
        /// </summary>
        public string SensorName { get; set; }
        /// <summary>
        /// 温度传感器安装位置
        /// </summary>
        public string InstallationPosition { get; set; }
        /// <summary>
        /// 最低温度
        /// </summary>
        public double LowestTemperature { get; set; }
        /// <summary>
        /// 最高温度
        /// </summary>
        public double HighestTemperature { get; set; }
        /// <summary>
        /// 是否启用报警
        /// </summary>
        public bool Enabled { get; set; }

    }


    public class TemperatureAlarmRuleEditModel
    {
        /// <summary>
        /// 温度报警规则ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 温度报警规则名称
        /// </summary>
        [Display(Name = "RuleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditTemperatureAlarmRuleNameExists", "TemperatureAlarmRule", "Admin", HttpMethod = "POST",
            AdditionalFields = "ID",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string Name { get; set; }
        /// <summary>
        /// 报警规则所属单位ID
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? AffiliatedStrucID { get; set; }
        /// <summary>
        /// 报警规则所属单位名称
        /// </summary>
        public string AffiliatedStrucName { get; set; }
        /// <summary>
        /// 启用报警的开始时间
        /// </summary>
        [Display(Name = "AlarmEnableStartTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public string StartTime { get; set; }
        /// <summary>
        /// 启用报警的结束时间
        /// </summary>
        [Display(Name = "AlarmEnableEndTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public string EndTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }

        /// <summary>
        /// 明细
        /// </summary>
        public List<TemperatureAlarmRuleDetailModel> TemperatureAlarmRuleDetails { get; set; }


        /// <summary>
        /// 修改用户ID
        /// </summary>
        public int EditUserID { get; set; }

    }
    #endregion

    #region 温度传感器
    public class TemperatureSensorModel
    {
        public string SensorName { get; set; }
        public int SensorCode { get; set; }
    }
    #endregion



    #region 温度报警规则分配
    public class VehicleTemperatureAlarmRulesSearchModel
    {
        /// <summary>
        /// 车代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        public string PlateNum { get; set; }

        /// <summary>
        /// 车辆分配的报警规则ID
        /// </summary>
        [Display(Name = "RuleName", ResourceType = typeof(DisplayText))]
        public int TemperatureAlarmRuleID { get; set; }

        /// <summary>
        /// 车辆所属单位ID
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public int StrucID { get; set; }
    }


    public class VehicleTemperatureAlarmRulesListModel
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        public long VID { get; set; }
        /// <summary>
        /// 车代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        public string PlateNum { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        public string VIN { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        [Display(Name = "RuleName", ResourceType = typeof(DisplayText))]
        public string RuleName { get; set; }
        /// <summary>
        /// 隶属单位名称
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public string StrucName { get; set; }
    }

    public class TemperatureAlarmRulesModel
    {
        /// <summary>
        /// 规则ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 规则名称
        /// </summary>
        [Display(Name = "RuleName", ResourceType = typeof(DisplayText))]
        public string RuleName { get; set; }
        /// <summary>
        /// 规则所属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public string StrucName { get; set; }
    }
    #endregion
}

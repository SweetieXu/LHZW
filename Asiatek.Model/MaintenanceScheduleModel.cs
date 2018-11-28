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
    public class MaintenanceScheduleSearchModel
    {
        /// <summary>
        /// 保养方案名称
        /// </summary>
        [Display(Name = "ScheduleName", ResourceType = (typeof(DisplayText)))]
        public string SearchScheduleName { get; set; }

        /// <summary>
        /// 隶属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = (typeof(DisplayText)))]
        public int SearchStrucID { get; set; }
    }

    public class MaintenanceScheduleListModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 保养方案名称
        /// </summary>
        [Display(Name = "ScheduleName", ResourceType = (typeof(DisplayText)))]
        public string ScheduleName { get; set; }

        /// <summary>
        /// 到期规则
        /// </summary>
        [Display(Name = "RulesType", ResourceType = (typeof(DisplayText)))]
        public int RulesType { get; set; }

        /// <summary>
        /// 绑定车辆数  
        /// </summary>
        [Display(Name = "BindVehicleNum", ResourceType = (typeof(DisplayText)))]
        public int BindVehicleNum { get; set; }

        /// <summary>
        /// 隶属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = (typeof(DisplayText)))]
        public string StrucName { get; set; }

        //public int StrucID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "CreateUser", ResourceType = (typeof(DisplayText)))]
        public string CreateUser { get; set; }

        //public int CreateUser { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = (typeof(DisplayText)))]
        public string Remark { get; set; }

    }

    public class MaintenanceProjectModel
    {
        public int ProjectID { get; set; }

        /// <summary>
        /// 保养项目
        /// </summary>
        [Display(Name = "MaintenanceProject", ResourceType = (typeof(DisplayText)))]
        public string ProjectName { get; set; }

        /// <summary>
        /// 配件名称
        /// </summary>
        [Display(Name = "PartsName", ResourceType = (typeof(DisplayText)))]
        public string PartsName { get; set; }

        /// <summary>
        /// 配件品牌
        /// </summary>
        [Display(Name = "PartsBrand", ResourceType = (typeof(DisplayText)))]
        public string PartsBrand { get; set; }

        /// <summary>
        /// 配件型号
        /// </summary>
        [Display(Name = "PartsVersion", ResourceType = (typeof(DisplayText)))]
        public string PartsVersion { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "Number", ResourceType = (typeof(DisplayText)))]
        public int? Num { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "Unit", ResourceType = (typeof(DisplayText)))]
        public string Unit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "UnitPrice", ResourceType = (typeof(DisplayText)))]
        public double? UnitPrice { get; set; }
    }
    #endregion

    #region 新增
    public class AddMaintenanceScheduleModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 保养方案名称
        /// </summary>
        [Display(Name = "ScheduleName", ResourceType = (typeof(DisplayText)))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddScheduleNameExists", "MaintenanceSchedule", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string ScheduleName { get; set; }

        /// <summary>
        /// 到期规则
        /// </summary>
        [Display(Name = "RulesType", ResourceType = (typeof(DisplayText)))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int RulesType { get; set; }

        /// <summary>
        /// 设定公里数
        /// </summary>
        public int? SettingMile { get; set; }

        /// <summary>
        /// 提前公里数
        /// </summary>
        public int? PreMile { get; set; }

        /// <summary>
        /// 设定时间数（天）
        /// </summary>
        public int? SettingDay { get; set; }

        /// <summary>
        /// 提前时间数（天）
        /// </summary>
        public int? PreDay { get; set; }

        [Display(Name = "MaintenanceContent", ResourceType = (typeof(DisplayText)))]
        public List<MaintenanceProjectModel> MaintenanceProjectDetails { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = (typeof(DisplayText)))]
        public string Remark { get; set; }
    }
    #endregion

    #region 编辑
    public class EditMaintenanceScheduleModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 保养方案名称
        /// </summary>
        [Display(Name = "ScheduleName", ResourceType = (typeof(DisplayText)))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditScheduleNameExists", "MaintenanceSchedule", "Admin",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string ScheduleName { get; set; }

        /// <summary>
        /// 到期规则
        /// </summary>
        [Display(Name = "RulesType", ResourceType = (typeof(DisplayText)))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int RulesType { get; set; }

        /// <summary>
        /// 设定公里数
        /// </summary>
        //[Display(Name = "SettingMile", ResourceType = (typeof(DisplayText)))]
        public int? SettingMile { get; set; }

        /// <summary>
        /// 提前公里数
        /// </summary>
        //[Display(Name = "PreMile", ResourceType = (typeof(DisplayText)))]
        public int? PreMile { get; set; }

        /// <summary>
        /// 设定时间数（天）
        /// </summary>
        //[Display(Name = "SettingDay", ResourceType = (typeof(DisplayText)))]
        public int? SettingDay { get; set; }

        /// <summary>
        /// 提前时间数（天）
        /// </summary>
        //[Display(Name = "PreDay", ResourceType = (typeof(DisplayText)))]
        public int? PreDay { get; set; }

        [Display(Name = "MaintenanceContent", ResourceType = (typeof(DisplayText)))]
        public List<MaintenanceProjectModel> MaintenanceProjectDetails { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = (typeof(DisplayText)))]
        public string Remark { get; set; }

        /// <summary>
        /// 绑定车辆数  已经绑定车辆的保养方案不可以修改到期规则
        /// </summary>
        public int BindVehicleNum { get; set; }
    }
    #endregion

    #region 车辆绑定
    public class MSBindVehicleSearchModel
    {
        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }

        /// <summary>
        /// ScheduleID 记录当前保养方案
        /// </summary>
        public int id { get; set; }
        public string ScheduleName { get; set; }
    }

    public class MSBindVehicleListModel
    {
        public int ScheduleID { get; set; }

        /// <summary>
        /// 保养方案名称
        /// </summary>
        [Display(Name = "ScheduleName", ResourceType = (typeof(DisplayText)))]
        public string ScheduleName { get; set; }

        [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
        public long VehicleID { get; set; }

        /// <summary>
        /// 车辆代号
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

        public int StructuresID { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        [Display(Name = "StrucName", ResourceType = typeof(DisplayText))]
        public string StrucName { get; set; }

        /// <summary>
        /// 初次保养里程
        /// </summary>
        [Display(Name = "FirstMaintenanceMile", ResourceType = typeof(DisplayText))]
        public double FirstMaintenanceMile { get; set; }

        /// <summary>
        /// 初次保养时间
        /// </summary>
        [Display(Name = "FirstMaintenanceTime", ResourceType = typeof(DisplayText))]
        public DateTime FirstMaintenanceTime { get; set; }

        [Display(Name = "CreateTime", ResourceType = typeof(DisplayText))]
        public DateTime CreateTime { get; set; }
    }
    #endregion
}

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
    public class MaintenanceRecordSearchModel
    {
        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }

        /// <summary>
        ///车辆隶属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public int SearchStrucID { get; set; }

        /// <summary>
        /// 隶属单位下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> SubordinateStrucSelectList { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        public string VIN { get; set; }
    }

    public class MaintenanceRecordListModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        public string VIN { get; set; }

        /// <summary>
        /// 隶属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = (typeof(DisplayText)))]
        public string StrucName { get; set; }

        /// <summary>
        /// 保养时间
        /// </summary>
        [Display(Name = "MaintenanceTime2", ResourceType = (typeof(DisplayText)))]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 保养详情
        /// </summary>
        [Display(Name = "MaintenanceDetail", ResourceType = (typeof(DisplayText)))]
        public string RecordDetails { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "CreateUser", ResourceType = (typeof(DisplayText)))]
        public string CreateUser { get; set; }

    }
    #endregion

    #region 新增
    public class AddMaintenanceRecordModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }

        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? VehicleID { get; set; }

        /// <summary>
        /// 保养详情
        /// </summary>
        [Display(Name = "MaintenanceDetail", ResourceType = (typeof(DisplayText)))]
        public string RecordDetails { get; set; }
    }
    #endregion

    #region 编辑
    public class EditMaintenanceRecordModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }

        /// <summary>
        /// 保养详情
        /// </summary>
        [Display(Name = "MaintenanceDetail", ResourceType = (typeof(DisplayText)))]
        public string RecordDetails { get; set; }
    }
    #endregion
}

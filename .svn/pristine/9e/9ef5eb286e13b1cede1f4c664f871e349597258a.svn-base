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
    /// <summary>
    /// 车辆传感器信息
    /// </summary>
    public class VehicleSensorListModel
    {
        public int TypeID { get; set; }
        /// <summary>
        /// 传感器名称
        /// </summary>
        [Display(Name = "SensorName", ResourceType = typeof(DisplayText))]
        public string SensorName { get; set; }
        public int SensorType { get; set; }
        /// <summary>
        /// 设备值1
        /// </summary>
        [Display(Name = "Value1", ResourceType = typeof(DisplayText))]
        public double? Value1 { get; set; }
        /// <summary>
        /// 设备值2
        /// </summary>
        [Display(Name = "Value2", ResourceType = typeof(DisplayText))]
        public double? Value2 { get; set; }
        /// <summary>
        /// 保固期
        /// </summary>
        [Display(Name = "WarrantyDate", ResourceType = typeof(DisplayText))]
        public DateTime WarrantyDate { get; set; }

        public bool IsUsed1 { get; set; }
        public bool IsUsed2 { get; set; }
        /// <summary>
        /// 是否已经添加传感器
        /// </summary>
        public bool IsHave { get; set; }
    }

    /// <summary>
    /// 给车辆传感器页面传递的参数
    /// </summary>
    public class VehicleSensorInfoParaModel
    {
        public long ID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
    }
    #endregion


    #region 新增、编辑
    public class VehicleSensorEditModel
    {
        /// <summary>
        /// 车辆编号
        /// </summary>
        [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
        public long VehicleID { get; set; }
        /// <summary>
        /// 车辆名称
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }
        /// <summary>
        /// 设备值1
        /// </summary>
        [Display(Name = "Value1", ResourceType = typeof(DisplayText))]
        public double? Value1 { get; set; }
        /// <summary>
        /// 设备值2
        /// </summary>
        [Display(Name = "Value2", ResourceType = typeof(DisplayText))]
        public double? Value2 { get; set; }
        /// <summary>
        /// 保固期
        /// </summary>
        [Display(Name = "WarrantyDate", ResourceType = typeof(DisplayText))]
        public DateTime WarrantyDate { get; set; }

        /// <summary>
        /// 传感器类型
        /// </summary>
        [Display(Name = "SensorType", ResourceType = typeof(DisplayText))]
        public int TypeID { get; set; }
        /// <summary>
        /// 传感器名称
        /// </summary>
        [Display(Name = "SensorName", ResourceType = typeof(DisplayText))]
        public string SensorName { get; set; }

    }
    #endregion

}

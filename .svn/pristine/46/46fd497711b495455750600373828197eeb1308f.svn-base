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
    public class SensorModels
    {
        public int TypeID { get; set; }
        public string SensorName { get; set; }
        public bool Value1 { get; set; }
        public bool Value2 { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
    }

    public class SensorSearchModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "VehicleTName", ResourceType = typeof(DisplayText))]
        public string SensorName { get; set; }
    }

    public class SensorListModels
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        public int TypeID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "SensorName", ResourceType = typeof(DisplayText))]
        public string SensorName { get; set; }

        /// <summary>
        /// 传感器编码
        /// </summary>
        [Display(Name = "SensorCode", ResourceType = typeof(DisplayText))]
        public string SensorCode { get; set; }

        /// <summary>
        /// 值1
        /// </summary>
        [Display(Name = "Value1", ResourceType = typeof(DisplayText))]
        public bool Value1 { get; set; }

        /// <summary>
        /// 值2
        /// </summary>
        [Display(Name = "Value2", ResourceType = typeof(DisplayText))]
        public bool Value2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "Status", ResourceType = typeof(DisplayText))]
        public string Status { get; set; }

        /// <summary>
        /// 传感器大类型编码（如WD表示温度传感器，YL表示油料传感器）
        /// </summary>
        [Display(Name = "SensorTypeCode", ResourceType = typeof(DisplayText))]
        public string TypeCode { get; set; }
    }


    #region   新增
    public class SensorAddModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        public int TypeID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "SensorName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [AsiatekRemote("CheckAddTypeNameExists", "Sensor", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string SensorName { get; set; }

        /// <summary>
        /// 传感器编码
        /// </summary>
        [Display(Name = "SensorCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [AsiatekRemote("CheckAddSensorCodeExists", "Sensor", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int? SensorCode { get; set; }

        /// <summary>
        /// 值1
        /// </summary>
        [Display(Name = "TValue", ResourceType = typeof(DisplayText))]
        public bool Value1 { get; set; }

        /// <summary>
        /// 值2
        /// </summary>
        [Display(Name = "FValue", ResourceType = typeof(DisplayText))]
        public bool Value2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "Status", ResourceType = typeof(DisplayText))]
        public string Status { get; set; }

        [Display(Name = "SensorTypeCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string TypeCode { get; set; }
    }
    #endregion

    #region   编辑
    public class SensorEditModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        public int TypeID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "SensorName", ResourceType = typeof(DisplayText))]
        //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        //[AsiatekRemote("CheckEditTypeNameExists", "Sensor", "Admin", HttpMethod = "POST",
        //    ErrorMessageResourceType = typeof(DataAnnotations),
        //    ErrorMessageResourceName = "FieldExists")]
        public string SensorName { get; set; }

        /// <summary>
        /// 传感器编码
        /// </summary>
        [Display(Name = "SensorCode", ResourceType = typeof(DisplayText))]
        public int SensorCode { get; set; }

        /// <summary>
        /// 值1
        /// </summary>
        [Display(Name = "TValue", ResourceType = typeof(DisplayText))]
        public bool Value1 { get; set; }

        /// <summary>
        /// 值2
        /// </summary>
        [Display(Name = "FValue", ResourceType = typeof(DisplayText))]
        public bool Value2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "Status", ResourceType = typeof(DisplayText))]
        public string Status { get; set; }

        [Display(Name = "SensorTypeCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string TypeCode { get; set; }
    }
    #endregion

}

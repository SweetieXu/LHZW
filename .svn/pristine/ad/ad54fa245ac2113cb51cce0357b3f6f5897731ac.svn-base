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
    /// 终端下拉数据
    /// 包含终端ID、终端号
    /// </summary>
    public class TerminalDDLModel
    {
        public long ID { get; set; }
        public string TerminalCode { get; set; }
    }

    /// <summary>
    /// 为了编辑车辆信息所用的终端下拉模型
    /// 其中IsPrimary为了区分主辅终端
    /// 如果是辅助终端那么多选的下拉列表中要默认选中
    /// </summary>
    public class TerminalDDLForEditVehicleModel
    {
        public long ID { get; set; }
        public string TerminalCode { get; set; }
        public bool IsPrimary { get; set; }
    }

    #region 注释代码
    //public class TerminalListModel
    //{
    //    public long ID { get; set; }
    //    /// <summary>
    //    /// 终端号
    //    /// </summary>
    //    [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
    //    public string TerminalCode { get; set; }
    //    /// <summary>
    //    /// 终端类型名称
    //    /// </summary>
    //    [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
    //    public string TerminalName { get; set; }
    //    /// <summary>
    //    /// 终端厂商名称
    //    /// </summary>
    //    [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
    //    public string ManufacturerName { get; set; }

    //    /// <summary>
    //    /// 终端通信所用SIM卡号
    //    /// </summary>
    //    [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
    //    public string SIMCode { get; set; }


    //    /// <summary>
    //    /// 车辆标识（终端内置车牌号）
    //    /// </summary>
    //    [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
    //    public string VehicleID { get; set; }

    //    /// <summary>
    //    /// 状态 ：默认0、删除9、报废8、使用中7
    //    /// </summary>
    //    public int Status { get; set; }

    //}


    //public class TerminalSearchModel
    //{
    //    /// <summary>
    //    /// 终端号
    //    /// </summary>
    //    [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
    //    public string TerminalCode { get; set; }

    //    /// <summary>
    //    /// 终端制造厂商ID
    //    /// </summary>
    //    [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
    //    public int TerminalManufacturerID { get; set; }


    //    /// <summary>
    //    /// 制造厂商下拉数据
    //    /// </summary>
    //    public IEnumerable<SelectListItem> TerminalManufacturerSelectList { get; set; }


    //    /// <summary>
    //    /// 终端类型ID
    //    /// </summary>
    //    [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
    //    public int TerminalTypeID { get; set; }


    //    /// <summary>
    //    /// 制造厂商下拉数据
    //    /// </summary>
    //    public IEnumerable<SelectListItem> TerminalTypeSelectList { get; set; }
    //}
    #endregion

    public class TerminalListModel
    {
        public long ID { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
        public string TerminalCode { get; set; }
        /// <summary>
        /// 终端类型名称
        /// </summary>
        [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
        public string TerminalName { get; set; }
        /// <summary>
        /// 终端厂商名称
        /// </summary>
        [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
        public string ManufacturerName { get; set; }

        /// <summary>
        /// 终端通信所用SIM卡号
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        public string SIMCode { get; set; }


        /// <summary>
        /// 车辆标识（终端内置车牌号）
        /// </summary>
        [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
        public string VehicleID { get; set; }

        /// <summary>
        /// 状态 ：默认0、删除9、报废8、使用中7
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        public string VIN { get; set; }
        /// <summary>
        /// 关联车辆ID
        /// </summary>
        public int LinkedVehicleID { get; set; }

    }


    public class TerminalSearchModel
    {
        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
        public string TerminalCode { get; set; }

        /// <summary>
        /// 终端制造厂商ID
        /// </summary>
        [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
        public int TerminalManufacturerID { get; set; }

        /// <summary>
        /// 车辆标识（终端内置车牌号）
        /// </summary>
        [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
        public string VehicleID { get; set; }

        /// <summary>
        /// 制造厂商下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> TerminalManufacturerSelectList { get; set; }


        /// <summary>
        /// 终端类型ID
        /// </summary>
        [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
        public int TerminalTypeID { get; set; }


        /// <summary>
        /// 制造厂商下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> TerminalTypeSelectList { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        public string VIN { get; set; }
        /// <summary>
        /// 终端通信所用SIM卡号
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        public string SIMCode { get; set; }
    }

    #endregion


    #region 新增
    public class TerminalAddModel
    {
        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(30, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddTerminalCodeExists", "Terminal", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string TerminalCode { get; set; }

        ///// <summary>
        ///// 终端通信Sim卡号
        ///// </summary>
        //[Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        //[RegularExpression(@"^([0-9]{11}|[0-9]{13})$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "SIMCodeError")]
        //[AsiatekRemote("CheckAddSIMCodeExists", "Terminal", "Admin", HttpMethod = "POST",
        //    ErrorMessageResourceType = typeof(DataAnnotations),
        //    ErrorMessageResourceName = "FieldExists")]
        //public string SIMCode { get; set; }

        ///// <summary>
        ///// 终端通信Sim卡号
        ///// </summary>
        public string SIMCode { get; set; }
        /// <summary>
        /// 终端通信Sim卡ID
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? SIMCodeID { get; set; }


        /// <summary>
        /// 车辆标识（终端内置车牌号）
        /// </summary>
        [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(30, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddVehicleIDExists", "Terminal", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string VehicleID { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }



        /// <summary>
        /// 终端类型ID
        /// </summary>
        [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int TerminalTypeID { get; set; }


        /// <summary>
        /// 制造厂商下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> TerminalTypeSelectList { get; set; }


        /// <summary>
        /// 终端制造厂商ID
        /// </summary>
        [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int TerminalManufacturerID { get; set; }


        /// <summary>
        /// 制造厂商下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> TerminalManufacturerSelectList { get; set; }

        /// <summary>
        /// 服务器ID
        /// </summary>
        [Display(Name = "ServerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? ServerInfoID { get; set; }
        /// <summary>
        /// 服务器下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> ServerInfoSelectList { get; set; }
        /// <summary>
        ///  超速阈值(单位km/h)
        /// </summary>
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        [Display(Name = "OverspeedThreshold", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public uint? OverspeedThreshold { get; set; }
        /// <summary>
        /// 引发超速报警所需的最小持续时间 （默认5s) 
        /// </summary>
        [Display(Name = "MinimumDuration", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint MinimumDuration { get; set; }
        /// <summary>
        /// 连续驾驶时间门限(默认14400s)
        /// </summary>
        [Display(Name = "ContinuousDrivingThreshold", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint ContinuousDrivingThreshold { get; set; }
        /// <summary>
        /// 最小休息时间（默认值1200s）
        /// </summary>
        [Display(Name = "MinimumBreakTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint MinimumBreakTime { get; set; }
        /// <summary>
        /// 当天累计驾驶时间门限(默认值57600秒)
        /// </summary>
        [Display(Name = "DrivingTimeThreshold", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint DrivingTimeThreshold { get; set; }
        /// <summary>
        /// 最长停车时间阈值(默认值3600秒)
        /// </summary>
        [Display(Name = "MaximumParkingTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint MaximumParkingTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserID { get; set; }
    }
    #endregion

    #region 编辑
    public class TerminalEditModel
    {

        public long ID { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(30, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditTerminalCodeExists", "Terminal", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string TerminalCode { get; set; }


        ///// <summary>
        ///// 终端通信Sim卡号
        ///// </summary>
        //[Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        //[RegularExpression(@"^([0-9]{11}|[0-9]{13})$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "SIMCodeError")]
        //[AsiatekRemote("CheckEditSIMCodeExists", "Terminal", "Admin", HttpMethod = "POST",
        //    ErrorMessageResourceType = typeof(DataAnnotations),
        //    ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        //public string SIMCode { get; set; }

        /// <summary>
        /// 原终端通信Sim卡ID
        /// </summary>
        public int? SIMCodeIDOld { get; set; }
        ///// <summary>
        ///// 终端通信Sim卡号
        ///// </summary>
        public string SIMCode { get; set; }
        /// <summary>
        /// 终端通信Sim卡ID
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? SIMCodeID { get; set; }

        /// <summary>
        /// 车辆标识（终端内置车牌号）
        /// </summary>
        [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(30, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditVehicleIDExists", "Terminal", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string VehicleID { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }



        /// <summary>
        /// 终端类型ID
        /// </summary>
        [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int TerminalTypeID { get; set; }


        /// <summary>
        /// 制造厂商下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> TerminalTypeSelectList { get; set; }


        /// <summary>
        /// 终端制造厂商ID
        /// </summary>
        [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int TerminalManufacturerID { get; set; }


        /// <summary>
        /// 制造厂商下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> TerminalManufacturerSelectList { get; set; }

        /// <summary>
        /// 服务器ID
        /// </summary>
        [Display(Name = "ServerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? ServerInfoID { get; set; }
        /// <summary>
        /// 服务器下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> ServerInfoSelectList { get; set; }

        /// <summary>
        ///  超速阈值(单位km/h)
        /// </summary>
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        [Display(Name = "OverspeedThreshold", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public uint? OverspeedThreshold { get; set; }
        /// <summary>
        /// 引发超速报警所需的最小持续时间 （默认5s) 
        /// </summary>
        [Display(Name = "MinimumDuration", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint MinimumDuration { get; set; }
        /// <summary>
        /// 连续驾驶时间门限(默认14400s)
        /// </summary>
        [Display(Name = "ContinuousDrivingThreshold", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint ContinuousDrivingThreshold { get; set; }
        /// <summary>
        /// 最小休息时间（默认值1200s）
        /// </summary>
        [Display(Name = "MinimumBreakTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint MinimumBreakTime { get; set; }
        /// <summary>
        /// 当天累计驾驶时间门限(默认值57600秒)
        /// </summary>
        [Display(Name = "DrivingTimeThreshold", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint DrivingTimeThreshold { get; set; }
        /// <summary>
        /// 最长停车时间阈值(默认值3600秒)
        /// </summary>
        [Display(Name = "MaximumParkingTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]\d*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PositiveIntegerError")]
        [Range(uint.MinValue, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public uint MaximumParkingTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public int EditUserID { get; set; }
    }
    #endregion

    #region 使用记录实体
    public class TerminalUsageLogs
    {
        public string TerminalCode { get; set; }
        public int VehicleID { get; set; }
        public int TerminalID { get; set; }
        public string PlateNum { get; set; }
        public string VIN { get; set; }
        public string SimCode { get; set; }
    }

    #endregion
}

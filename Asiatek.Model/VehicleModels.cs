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
    public class VehicleListModel
    {
        public long ID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        public string PlateNum { get; set; }
        /// <summary>
        /// 主定位终端号
        /// </summary>
        [Display(Name = "PrimaryTerminalCode", ResourceType = typeof(DisplayText))]
        public string TerminalCode { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }
        /// <summary>
        /// 主定位终端SIM卡号
        /// </summary>
        [Display(Name = "PrimarySIMCode", ResourceType = typeof(DisplayText))]
        public string SIMCode { get; set; }
        /// <summary>
        /// 主定位终端类型
        /// </summary>
        [Display(Name = "PrimaryTerminalType", ResourceType = typeof(DisplayText))]
        public string TerminalName { get; set; }
        /// <summary>
        /// 使用单位
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        public string StrucName { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        [Display(Name = "PlateColor", ResourceType = typeof(DisplayText))]
        public string PlateColor { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        [Display(Name = "VehicleType", ResourceType = typeof(DisplayText))]
        public string VehicleType { get; set; }
        ///// <summary>
        ///// 限速值（公里/小时）
        ///// </summary>
        //[Display(Name = "SpeedLimit", ResourceType = typeof(DisplayText))]
        //public int SpeedLimit { get; set; }
        /// <summary>
        /// 是否转发
        /// </summary>
        [Display(Name = "IsTransmit", ResourceType = typeof(DisplayText))]
        public bool IsTransmit { get; set; }

        public bool IsReceived { get; set; }

        /// <summary>
        /// 保固日期
        /// </summary>
        [Display(Name = "HardwareDate", ResourceType = typeof(DisplayText))]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public DateTime WarrantyDate { get; set; }
        /// <summary>
        /// 软件服务到期日期
        /// </summary>
        [Display(Name = "SoftwareDate", ResourceType = typeof(DisplayText))]
        public DateTime? SoftwareDate { get; set; }

        //状态情况
        public int Status { get; set; }
    }

    public class OLDVehicleSearchModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        public string PlateNum { get; set; }

        /// <summary>
        /// SIM卡号
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        public string SIMCode { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
        public string TerminalCode { get; set; }

        /// <summary>
        /// 初始车牌号
        /// </summary>
        [Display(Name = "VehicleCode", ResourceType = typeof(DisplayText))]
        public string VehicleCode { get; set; }

        /// <summary>
        /// 终端类型编号
        /// </summary>
        public int TerminalTypeID { get; set; }

        /// <summary>
        /// 终端类型下拉
        /// </summary>
        [Display(Name = "TerminalType", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> TerminalTypeSelectList { get; set; }

        /// <summary>
        /// 使用单位下拉数据
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> SubordinateStrucSelectList { get; set; }

        /// <summary>
        ///车辆使用单位ID
        /// </summary>
        public int StrucID { get; set; }

    }

    public class VehicleSearchModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        public string PlateNum { get; set; }

        /// <summary>
        /// 主定位终端SIM卡号
        /// </summary>
        [Display(Name = "PrimarySIMCode", ResourceType = typeof(DisplayText))]
        public string PrimarySIMCode { get; set; }

        /// <summary>
        /// 主定位终端号
        /// </summary>
        [Display(Name = "PrimaryTerminalCode", ResourceType = typeof(DisplayText))]
        public string PrimaryTerminalCode { get; set; }

        /// <summary>
        /// 辅助定位终端终端号
        /// </summary>
        [Display(Name = "AuxiliaryTerminalCode", ResourceType = typeof(DisplayText))]
        public string AuxiliaryTerminalCode { get; set; }



        /// <summary>
        /// 终端类型编号
        /// </summary>
        [Display(Name = "PrimaryTerminalType", ResourceType = typeof(DisplayText))]
        public int PrimaryTerminalTypeID { get; set; }


        /// <summary>
        /// 终端类型下拉
        /// </summary>
        public IEnumerable<SelectListItem> PrimaryTerminalTypeSelectList { get; set; }

        /// <summary>
        ///车辆使用单位ID
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        public int SearchStrucID { get; set; }

        /// <summary>
        /// 使用单位下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> SubordinateStrucSelectList { get; set; }

        /// <summary>
        /// 车主姓名
        /// </summary>
        [Display(Name = "OwnersName", ResourceType = typeof(DisplayText))]
        public string OwnersName { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        public string VIN { get; set; }

    }

    /// <summary>
    /// 车辆分配
    /// </summary>
    public class VehicleDistributionInfoModel
    {
        public long ID { get; set; }
        public string StrucName { get; set; }
        public string PlateNum { get; set; }
    }
    #endregion


    #region 编辑
    /// <summary>
    /// 修改车辆保固期模型
    /// </summary>
    public class VehicleModifyWarrantyDateModel
    {
        /// <summary>
        /// 待修改保固期的车辆ID（以|分割）
        /// </summary>
        public string VehicleIDs { get; set; }
        /// <summary>
        /// 保固期
        /// </summary>
        [Display(Name = "WarrantyDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [LegalDate(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateError")]
        [DateRange(0, null, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "WarrantyDateError")]
        public string WarrantyDate { get; set; }
    }

    /// <summary>
    /// 修改车辆限速值模型
    /// </summary>
    public class VehicleModifySpeedLimitModel
    {
        /// <summary>
        /// 待修改限速值的车辆ID（以|分割）
        /// </summary>
        public string VehicleIDs { get; set; }

        /// <summary>
        /// 限速值(KM/H）
        /// </summary>
        [Display(Name = "SpeedLimit", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Range(30, 100, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int SpeedLimit { get; set; }
    }

    public class OLDVehicleEditModel
    {
        /// <summary>
        /// 车辆资料修改用户
        /// </summary>
        public int UpdateUserID { get; set; }

        public long ID { get; set; }


        /// <summary>
        /// 保固期
        /// </summary>
        [Display(Name = "WarrantyDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [LegalDate(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateError")]
        public string WarrantyDate { get; set; }

        /// <summary>
        /// 初始车牌
        /// </summary>
        [Display(Name = "VehicleCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditVehicleCodeExists", "Vehicle", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string VehicleCode { get; set; }

        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditVehicleNameExists", "Vehicle", "Admin", AdditionalFields = "StrucID,ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "VehicleNameExists")]
        public string VehicleName { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditTerminalCodeExists", "Vehicle", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string TerminalCode { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(7, MinimumLength = 7, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PlateNumLengthError")]
        [AsiatekRemote("CheckEditPlateNumExists", "Vehicle", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string PlateNum { get; set; }


        /// <summary>
        /// SIM卡号
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^([0-9]{11}|[0-9]{13})$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "SIMCodeError")]
        [AsiatekRemote("CheckEditSIMCodeExists", "Vehicle", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string SIMCode { get; set; }


        /// <summary>
        /// 车牌颜色编码
        /// </summary>
        public int PlateColorCode { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        [Display(Name = "PlateColors", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> PlateColorsSelectList { get; set; }


        /// <summary>
        /// 终端型号ID
        /// </summary>
        public int TerminalTypeID { get; set; }


        /// <summary>
        /// 终端型号
        /// </summary>
        [Display(Name = "TerminalType", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> TerminalTypesSelectList { get; set; }

        /// <summary>
        /// 车辆类型编码
        /// </summary>
        public int VehicleTypeCode { get; set; }



        /// <summary>
        /// 车辆类型
        /// </summary>
        [Display(Name = "VehicleTypes", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> VehicleTypesSelectList { get; set; }


        /// <summary>
        /// 车辆使用单位ID
        /// </summary>
        public int StrucID { get; set; }



        /// <summary>
        /// 使用单位
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> StructuresSelectList { get; set; }


        /// <summary>
        /// 车辆产权所属单位ID
        /// </summary>
        public int Ownership { get; set; }



        /// <summary>
        /// 产权所属单位
        /// </summary>
        [Display(Name = "Ownership", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> OwnershipSelectList { get; set; }


        /// <summary>
        /// 平台信号接收
        /// </summary>
        [Display(Name = "IsReceived", ResourceType = typeof(DisplayText))]
        public bool IsReceived { get; set; }


        /// <summary>
        /// 货运平台接入
        /// </summary>
        [Display(Name = "IsAccess", ResourceType = typeof(DisplayText))]
        public bool IsAccess { get; set; }


        /// <summary>
        /// 是否转发到运管
        /// </summary>
        [Display(Name = "IsTransmit", ResourceType = typeof(DisplayText))]
        public bool IsTransmit { get; set; }

        /// <summary>
        /// 是否是危险品车
        /// </summary>
        [Display(Name = "IsDangerous", ResourceType = typeof(DisplayText))]
        public bool IsDangerous { get; set; }



        /// <summary>
        /// 限速值（公里/小时）
        /// </summary>
        [Display(Name = "SpeedLimit", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Range(30, 100, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int SpeedLimit { get; set; }




        /// <summary>
        /// 疲劳驾驶时间（分钟）
        /// </summary>
        [Display(Name = "DrivingTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Range(1, 300, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int DrivingTime { get; set; }


        /// <summary>
        /// 车辆图标
        /// </summary>
        [Display(Name = "Icon", ResourceType = typeof(DisplayText))]
        public string Icon { get; set; }

        public List<string> Icons { get; set; }



        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }

    public class VehicleEditModel
    {
        /// <summary>
        /// 车辆资料修改用户
        /// </summary>
        public int UpdateUserID { get; set; }

        public long ID { get; set; }


        /// <summary>
        /// 主定位终端ID
        /// </summary>
        [Display(Name = "PrimaryTerminal", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelectPrimaryTerminal")]
        public long PrimaryTerminalID { get; set; }

        public string TerminalCode { get; set; }

        /// <summary>
        /// 辅助定位终端ID
        /// 多个终端ID用,分割
        /// </summary>
        [Display(Name = "AuxiliaryTerminal", ResourceType = typeof(DisplayText))]
        public string AuxiliaryTerminalIDs { get; set; }

        /// <summary>
        ///  终端列表
        ///  用于主定位终端的选择
        /// </summary>
        public IEnumerable<SelectListItem> PrimaryTerminalsSelectList { get; set; }

        /// <summary>
        ///  终端列表
        ///  用于辅助定位终端的选择
        /// </summary>
        public IEnumerable<SelectListItem> AuxiliaryTerminalsSelectList { get; set; }


        /// <summary>
        /// 保固期
        /// </summary>
        [Display(Name = "WarrantyDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [LegalDate(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateError")]
        public string WarrantyDate { get; set; }


        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditVehicleNameExists", "Vehicle", "Admin", AdditionalFields = "StrucID,ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "VehicleNameExists")]
        public string VehicleName { get; set; }


        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(8, MinimumLength = 7, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PlateNumLengthError")]
        [AsiatekRemote("CheckEditPlateNumExists", "Vehicle", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string PlateNum { get; set; }


        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[a-zA-Z0-9]{17}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "VINError")]
        [AsiatekRemote("CheckEditVINExists", "Vehicle", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
           ErrorMessageResourceType = typeof(DataAnnotations),
           ErrorMessageResourceName = "FieldExists")]
        public string VIN { get; set; }


        /// <summary>
        /// 车牌颜色编码
        /// </summary>
        [Display(Name = "PlateColors", ResourceType = typeof(DisplayText))]
        public int PlateColorCode { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        public IEnumerable<SelectListItem> PlateColorsSelectList { get; set; }


        /// <summary>
        /// 终端型号ID
        /// </summary>
        [Display(Name = "TerminalType", ResourceType = typeof(DisplayText))]
        public int TerminalTypeID { get; set; }


        /// <summary>
        /// 终端型号
        /// </summary>
        public IEnumerable<SelectListItem> TerminalTypesSelectList { get; set; }


        /// <summary>
        /// 车辆类型编码
        /// </summary>
        [Display(Name = "VehicleTypes", ResourceType = typeof(DisplayText))]
        public int VehicleTypeCode { get; set; }


        /// <summary>
        /// 车辆类型
        /// </summary>
        public IEnumerable<SelectListItem> VehicleTypesSelectList { get; set; }


        /// <summary>
        /// 车辆使用单位ID（用于CheckEditVehicleNameExists验证时StrucID不能为空，用int?验证会报错）
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        public int StrucID { get; set; }

        public string StrucName { get; set; }

        /// <summary>
        /// 车辆使用单位ID  （添加时要验证EditStrucID字段不能为空，所以使用int?，用int型会初始默认0值，无法验证）
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? EditStrucID { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        public IEnumerable<SelectListItem> StructuresSelectList { get; set; }

        /// <summary>
        /// 车辆产权所属单位ID
        /// </summary>
        [Display(Name = "Ownership", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? Ownership { get; set; }

        public string OwnershipName { get; set; }


        /// <summary>
        /// 产权所属单位
        /// </summary>
        public IEnumerable<SelectListItem> OwnershipSelectList { get; set; }


        /// <summary>
        /// 平台信号接收
        /// </summary>
        [Display(Name = "IsReceived", ResourceType = typeof(DisplayText))]
        public bool IsReceived { get; set; }


        /// <summary>
        /// 货运平台接入
        /// </summary>
        [Display(Name = "IsAccess", ResourceType = typeof(DisplayText))]
        public bool IsAccess { get; set; }


        /// <summary>
        /// 是否转发到运管
        /// </summary>
        [Display(Name = "IsTransmit", ResourceType = typeof(DisplayText))]
        public bool IsTransmit { get; set; }

        /// <summary>
        /// 是否是危险品车
        /// </summary>
        [Display(Name = "IsDangerous", ResourceType = typeof(DisplayText))]
        public bool IsDangerous { get; set; }

        /// <summary>
        /// 车辆图标
        /// </summary>
        [Display(Name = "Icon", ResourceType = typeof(DisplayText))]
        public string Icon { get; set; }

        public List<string> Icons { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }
    #endregion


    #region 新增
    public class OLDVehicleAddModel
    {
        /// <summary>
        /// 车辆资料创建用户
        /// </summary>
        public int CreateUserID { get; set; }

        /// <summary>
        /// 初始车牌
        /// </summary>
        [Display(Name = "VehicleCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddVehicleCodeExists", "Vehicle", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string VehicleCode { get; set; }

        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddVehicleNameExists", "Vehicle", "Admin", AdditionalFields = "StrucID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "VehicleNameExists")]
        public string VehicleName { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddTerminalCodeExists", "Vehicle", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string TerminalCode { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(7, MinimumLength = 7, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PlateNumLengthError")]
        [AsiatekRemote("CheckAddPlateNumExists", "Vehicle", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string PlateNum { get; set; }


        /// <summary>
        /// SIM卡号
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^([0-9]{11}|[0-9]{13})$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "SIMCodeError")]
        [AsiatekRemote("CheckAddSIMCodeExists", "Vehicle", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string SIMCode { get; set; }


        /// <summary>
        /// 车牌颜色编码
        /// </summary>
        public int PlateColorCode { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        [Display(Name = "PlateColors", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> PlateColorsSelectList { get; set; }


        /// <summary>
        /// 终端型号ID
        /// </summary>
        public int TerminalTypeID { get; set; }


        /// <summary>
        /// 终端型号
        /// </summary>
        [Display(Name = "TerminalType", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> TerminalTypesSelectList { get; set; }

        /// <summary>
        /// 车辆类型编码
        /// </summary>
        public int VehicleTypeCode { get; set; }



        /// <summary>
        /// 车辆类型
        /// </summary>
        [Display(Name = "VehicleTypes", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> VehicleTypesSelectList { get; set; }


        /// <summary>
        /// 车辆使用单位ID
        /// </summary>
        public int StrucID { get; set; }



        /// <summary>
        /// 使用单位
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> StructuresSelectList { get; set; }


        /// <summary>
        /// 车辆产权所属单位ID
        /// </summary>
        public int Ownership { get; set; }



        /// <summary>
        /// 产权所属单位
        /// </summary>
        [Display(Name = "Ownership", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> OwnershipSelectList { get; set; }


        /// <summary>
        /// 平台信号接收
        /// </summary>
        [Display(Name = "IsReceived", ResourceType = typeof(DisplayText))]
        public bool IsReceived { get; set; }


        /// <summary>
        /// 货运平台接入
        /// </summary>
        [Display(Name = "IsAccess", ResourceType = typeof(DisplayText))]
        public bool IsAccess { get; set; }


        /// <summary>
        /// 是否转发到运管
        /// </summary>
        [Display(Name = "IsTransmit", ResourceType = typeof(DisplayText))]
        public bool IsTransmit { get; set; }

        /// <summary>
        /// 是否是危险品车
        /// </summary>
        [Display(Name = "IsDangerous", ResourceType = typeof(DisplayText))]
        public bool IsDangerous { get; set; }



        /// <summary>
        /// 限速值（公里/小时）
        /// </summary>
        [Display(Name = "SpeedLimit", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Range(30, 100, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int SpeedLimit { get; set; }




        /// <summary>
        /// 疲劳驾驶时间（分钟）
        /// </summary>
        [Display(Name = "DrivingTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Range(1, 300, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int DrivingTime { get; set; }


        /// <summary>
        /// 保固期
        /// </summary>
        [Display(Name = "WarrantyDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [LegalDate(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateError")]
        [DateRange(0, null, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "WarrantyDateError")]
        public string WarrantyDate { get; set; }

        /// <summary>
        /// 车辆图标
        /// </summary>
        [Display(Name = "Icon", ResourceType = typeof(DisplayText))]
        public string Icon { get; set; }

        public List<string> Icons { get; set; }



        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }

    public class VehicleAddModel
    {

        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(8, MinimumLength = 7, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PlateNumLengthError")]
        [AsiatekRemote("CheckAddPlateNumExists", "Vehicle", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string PlateNum { get; set; }


        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddVehicleNameExists", "Vehicle", "Admin", AdditionalFields = "StrucID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "VehicleNameExists")]
        public string VehicleName { get; set; }


        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[a-zA-Z0-9]{17}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "VINError")]
        [AsiatekRemote("CheckAddVINExists", "Vehicle", "Admin", HttpMethod = "POST",
           ErrorMessageResourceType = typeof(DataAnnotations),
           ErrorMessageResourceName = "FieldExists")]
        public string VIN { get; set; }


        /// <summary>
        /// 主定位终端ID
        /// </summary>
        [Display(Name = "PrimaryTerminal", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelectPrimaryTerminal")]
        public long PrimaryTerminalID { get; set; }


        /// <summary>
        /// 辅助定位终端ID
        /// 多个终端ID用,分割
        /// </summary>
        [Display(Name = "AuxiliaryTerminal", ResourceType = typeof(DisplayText))]
        public string AuxiliaryTerminalIDs { get; set; }

        /// <summary>
        ///  终端列表
        ///  用于主定位终端与辅助定位终端的选择
        /// </summary>
        public IEnumerable<SelectListItem> TerminalsSelectList { get; set; }


        /// <summary>
        /// 车牌颜色编码
        /// </summary>
        [Display(Name = "PlateColors", ResourceType = typeof(DisplayText))]
        public int PlateColorCode { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        public IEnumerable<SelectListItem> PlateColorsSelectList { get; set; }



        /// <summary>
        /// 车辆类型编码
        /// </summary>
        [Display(Name = "VehicleTypes", ResourceType = typeof(DisplayText))]
        public int VehicleTypeCode { get; set; }



        /// <summary>
        /// 车辆类型
        /// </summary>
        public IEnumerable<SelectListItem> VehicleTypesSelectList { get; set; }


        /// <summary>
        /// 车辆使用单位ID（用于CheckAddVehicleNameExists验证时StrucID不能为空）
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        public int StrucID { get; set; }


        /// <summary>
        /// 车辆使用单位ID（验证使用单位不能为空，所以用int?，没有选择时值为空）
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? AddStrucID { get; set; }



        /// <summary>
        /// 使用单位
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> StructuresSelectList { get; set; }


        /// <summary>
        /// 车辆产权所属单位ID
        /// </summary>
        [Display(Name = "Ownership", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? Ownership { get; set; }



        /// <summary>
        /// 产权所属单位
        /// </summary>
        [Display(Name = "Ownership", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> OwnershipSelectList { get; set; }


        /// <summary>
        /// 平台信号接收
        /// </summary>
        [Display(Name = "IsReceived", ResourceType = typeof(DisplayText))]
        public bool IsReceived { get; set; }


        /// <summary>
        /// 货运平台接入
        /// </summary>
        [Display(Name = "IsAccess", ResourceType = typeof(DisplayText))]
        public bool IsAccess { get; set; }


        /// <summary>
        /// 是否转发到运管
        /// </summary>
        [Display(Name = "IsTransmit", ResourceType = typeof(DisplayText))]
        public bool IsTransmit { get; set; }

        /// <summary>
        /// 是否是危险品车
        /// </summary>
        [Display(Name = "IsDangerous", ResourceType = typeof(DisplayText))]
        public bool IsDangerous { get; set; }



        ///// <summary>
        ///// 限速值（公里/小时）
        ///// </summary>
        //[Display(Name = "SpeedLimit", ResourceType = typeof(DisplayText))]
        //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        //[Range(30, 120, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        //public int SpeedLimit { get; set; }




        ///// <summary>
        ///// 疲劳驾驶时间（分钟）
        ///// </summary>
        //[Display(Name = "DrivingTime", ResourceType = typeof(DisplayText))]
        //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        //[Range(1, 300, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        //public int DrivingTime { get; set; }


        /// <summary>
        /// 保固期
        /// </summary>
        [Display(Name = "WarrantyDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [LegalDate(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateError")]
        [DateRange(0, null, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "WarrantyDateError")]
        public string WarrantyDate { get; set; }

        /// <summary>
        /// 车辆图标
        /// </summary>
        [Display(Name = "Icon", ResourceType = typeof(DisplayText))]
        public string Icon { get; set; }

        public List<string> Icons { get; set; }


        /// <summary>
        /// 车辆资料创建用户
        /// </summary>
        public int CreateUserID { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }
    #endregion


    #region 其他
    /// <summary>
    /// 用户分配的有效车辆信息（车代号、车辆使用单位名、单位ID、车架号）(有效是指：Status为0，IsReceived为1)
    /// </summary>
    public class UserVehicles
    {
        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 车辆使用单位名
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆ID
        /// </summary>
        public long VID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }
    }

    /// <summary>
    /// 用户分配的有效车辆信息（车牌号、车辆使用单位名、单位ID、车架号）
    /// </summary>
    public class UserVehiclesModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 车辆使用单位名
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆ID
        /// </summary>
        public long VID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }
    }
    #endregion


    #region 终端设置模块涉及到的终端车辆单位实体
    public class VehiclesTerminalModel
    {
        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
    }
    #endregion


    #region 终端所属服务器的wcf地址实体
    public class TerminalWCfModel
    {
        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// wcf地址
        /// </summary>
        public string WCfAddress { get; set; }
    }
    #endregion

    #region 新增编辑实体 新版

    public class EditVehicleModel
    {
        public long ID { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(8, MinimumLength = 7, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PlateNumLengthError")]
        [AsiatekRemote("CheckNewEditPlateNumExists", "Vehicle", "Admin", HttpMethod = "POST",
         ErrorMessageResourceType = typeof(DataAnnotations),
         ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string PlateNum { get; set; }

        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckNewEditVehicleNameExists", "Vehicle", "Admin", AdditionalFields = "StrucID,ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "VehicleNameExists")]
        public string VehicleName { get; set; }

        /// <summary>
        /// 车辆使用单位ID（用于CheckAddVehicleNameExists验证时StrucID不能为空）
        /// </summary>
        public int StrucID { get; set; }

        /// <summary>
        /// 车辆使用单位ID（验证使用单位不能为空，所以用int?，没有选择时值为空）
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? EditStrucID { get; set; }

        /// <summary>
        /// 使用单位名称
        /// </summary>
        public string UserStrucName { get; set; }

        /// <summary>
        /// 车辆产权所属单位ID
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        [Display(Name = "Ownership", ResourceType = typeof(DisplayText))]
        public int? Ownership { get; set; }

        /// <summary>
        /// 产权所属单位
        /// </summary>
        public string OwnerStrucName { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[a-zA-Z0-9]{17}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "VINError")]
        [AsiatekRemote("CheckNewEditVINExists", "Vehicle", "Admin", HttpMethod = "POST",
           ErrorMessageResourceType = typeof(DataAnnotations),
           ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string VIN { get; set; }

        /// <summary>
        /// 定位终端
        /// </summary>
        [Display(Name = "PrimaryTerminal", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelectPrimaryTerminal")]
        public long PrimaryTerminalID { get; set; }

        /// <summary>
        /// 原终端 用于车辆编辑的时候使用
        /// </summary>
        public long OldTerminalID { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }

        /// <summary>
        ///  终端列表
        /// </summary>
        public IEnumerable<SelectListItem> TerminalsSelectList { get; set; }


        /// <summary>
        /// 车牌颜色编码
        /// </summary>
        [Display(Name = "PlateColors", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int PlateColorCode { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        public IEnumerable<SelectListItem> PlateColorsSelectList { get; set; }

        /// <summary>
        /// 车辆类型编码
        /// </summary>
        [Display(Name = "VehicleTypes", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int VehicleTypeCode { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public IEnumerable<SelectListItem> VehicleTypesSelectList { get; set; }

        /// <summary>
        /// 平台信号接收
        /// </summary>
        [Display(Name = "IsReceived", ResourceType = typeof(DisplayText))]
        public bool IsReceived { get; set; }


        /// <summary>
        /// 货运平台接入
        /// </summary>
        [Display(Name = "IsAccess", ResourceType = typeof(DisplayText))]
        public bool IsAccess { get; set; }


        /// <summary>
        /// 是否转发到运管
        /// </summary>
        [Display(Name = "IsTransmit", ResourceType = typeof(DisplayText))]
        public bool IsTransmit { get; set; }

        /// <summary>
        /// 是否是危险品车
        /// </summary>
        [Display(Name = "IsDangerous", ResourceType = typeof(DisplayText))]
        public bool IsDangerous { get; set; }

        [Display(Name = "Driver", ResourceType = typeof(UIText))]
        public int? DriverID { get; set; }
        /// <summary>
        /// 驾驶员列表
        /// </summary>
        public IEnumerable<SelectListItem> DriverSelectList { get; set; }

        [Display(Name = "Escort", ResourceType = typeof(UIText))]
        public int? CarrierID { get; set; }
        /// <summary>
        /// 押运员列表
        /// </summary>
        public IEnumerable<SelectListItem> CarrierSelectList { get; set; }

        /// <summary>
        /// 硬件服务到期日期
        /// </summary>
        [Display(Name = "HardwareDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string WarrantyDate { get; set; }
        /// <summary>
        /// 软件服务到期日期
        /// </summary>
        [Display(Name = "SoftwareDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string SoftwareDate { get; set; }

        /// <summary>
        /// 车辆图标
        /// </summary>
        [Display(Name = "Icon", ResourceType = typeof(DisplayText))]
        public string Icon { get; set; }

        public List<string> Icons { get; set; }

        /// <summary>
        /// 车辆资料创建用户
        /// </summary>
        public int CreateUserID { get; set; }

        public int UpdateUserID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

        /// <summary>
        /// 经营范围列表
        /// </summary>
        public string BusinessScopeList { get; set; }
        /// <summary>
        /// 关联运管所列表
        /// </summary>
        public string TransportManagementList { get; set; }
        /// <summary>
        /// 车籍地
        /// </summary>
        [Display(Name = "CarToLand", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "CarToLandError")]
        public string CarToLand { get; set; }
        /// <summary>
        /// 道路运输证号
        /// </summary>
        [Display(Name = "RoadransportNo", ResourceType = typeof(DisplayText))]
        public string RoadransportNo { get; set; }
        /// <summary>
        /// 运输行业编码
        /// </summary>
        [Display(Name = "TransportIndustry", ResourceType = typeof(DisplayText))]
        public string TransportIndustryCode { get; set; }

    }

    #endregion

    #region 驾驶员押运员信息
    public class VehicleEmployeeInfoDDLModel
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public int EmployeeInfoID { get; set; }
        /// <summary>
        /// 类型 1 驾驶员 2押运员
        /// </summary>
        public int Type { get; set; }
    }
    #endregion

    #region 新增编辑实体 新版  2017-12-06

    public class NewEditVehicleModel
    {
        public long ID { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(8, MinimumLength = 7, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PlateNumLengthError")]
        [AsiatekRemote("CheckNewEditPlateNumExists", "Vehicle", "Admin", HttpMethod = "POST",
         ErrorMessageResourceType = typeof(DataAnnotations),
         ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string PlateNum { get; set; }

        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckNewEditVehicleNameExists", "Vehicle", "Admin", AdditionalFields = "StrucID,ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "VehicleNameExists")]
        public string VehicleName { get; set; }

        /// <summary>
        /// 车辆使用单位ID（用于CheckAddVehicleNameExists验证时StrucID不能为空）
        /// </summary>
        public int StrucID { get; set; }

        /// <summary>
        /// 车辆使用单位ID（验证使用单位不能为空，所以用int?，没有选择时值为空）
        /// </summary>
        [Display(Name = "StrucWhichUseVehicle", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? EditStrucID { get; set; }

        /// <summary>
        /// 使用单位名称
        /// </summary>
        public string UserStrucName { get; set; }

        /// <summary>
        /// 车辆产权所属单位ID
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        [Display(Name = "Ownership", ResourceType = typeof(DisplayText))]
        public int? Ownership { get; set; }

        /// <summary>
        /// 产权所属单位
        /// </summary>
        public string OwnerStrucName { get; set; }

        /// <summary>
        /// 车架号
        /// </summary>
        [Display(Name = "VIN", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[a-zA-Z0-9]{17}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "VINError")]

        [AsiatekRemote("CheckNewEditVINExists", "Vehicle", "Admin", HttpMethod = "POST",
           ErrorMessageResourceType = typeof(DataAnnotations),
           ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string VIN { get; set; }

        /// <summary>
        /// 定位终端
        /// </summary>
        [Display(Name = "PrimaryTerminal", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelectPrimaryTerminal")]
        public long PrimaryTerminalID { get; set; }

        /// <summary>
        /// 原终端 用于车辆编辑的时候使用
        /// </summary>
        public long OldTerminalID { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }

        /// <summary>
        ///  终端列表
        /// </summary>
        public IEnumerable<SelectListItem> TerminalsSelectList { get; set; }


        /// <summary>
        /// 车牌颜色编码
        /// </summary>
        [Display(Name = "PlateColors", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int PlateColorCode { get; set; }

        /// <summary>
        /// 车牌颜色
        /// </summary>
        public IEnumerable<SelectListItem> PlateColorsSelectList { get; set; }

        /// <summary>
        /// 车辆类型编码
        /// </summary>
        [Display(Name = "VehicleTypes", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int VehicleTypeCode { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        public IEnumerable<SelectListItem> VehicleTypesSelectList { get; set; }

        /// <summary>
        /// 平台信号接收
        /// </summary>
        [Display(Name = "IsReceived", ResourceType = typeof(DisplayText))]
        public bool IsReceived { get; set; }


        /// <summary>
        /// 货运平台接入
        /// </summary>
        [Display(Name = "IsAccess", ResourceType = typeof(DisplayText))]
        public bool IsAccess { get; set; }


        /// <summary>
        /// 是否转发到运管
        /// </summary>
        [Display(Name = "IsTransmit", ResourceType = typeof(DisplayText))]
        public bool IsTransmit { get; set; }

        /// <summary>
        /// 是否是危险品车
        /// </summary>
        [Display(Name = "IsDangerous", ResourceType = typeof(DisplayText))]
        public bool IsDangerous { get; set; }

        [Display(Name = "Driver", ResourceType = typeof(UIText))]
        public string Driver { get; set; }

        public int? DriverID { get; set; }


        [Display(Name = "Escort", ResourceType = typeof(UIText))]
        public string Carrier { get; set; }

        public int? CarrierID { get; set; }


        /// <summary>
        /// 硬件服务到期日期
        /// </summary>
        [Display(Name = "HardwareDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string WarrantyDate { get; set; }
        /// <summary>
        /// 软件服务到期日期
        /// </summary>
        [Display(Name = "SoftwareDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string SoftwareDate { get; set; }
        /// <summary>
        /// 年检时间
        /// </summary>
        [Display(Name = "AnnualInspectionTime", ResourceType = typeof(DisplayText))]
        public string AnnualInspectionTime { get; set; }
        /// <summary>
        /// 年检提前提醒时间(单位:天)
        /// </summary>
        [Display(Name = "RemindTimeSpan", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^$|^\d+$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "TimeSpanError")]
        public string RemindTimeSpan { get; set; }
        /// <summary>
        /// 年检时间
        /// </summary>
        [Display(Name = "AnnualInspectionTime1", ResourceType = typeof(DisplayText))]
        public string AnnualInspectionTime1 { get; set; }
        /// <summary>
        /// 年检提前提醒时间(单位:天)
        /// </summary>
        [Display(Name = "RemindTimeSpan1", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^$|^\d+$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "TimeSpanError")]
        public string RemindTimeSpan1 { get; set; }
        /// <summary>
        /// 车辆图标
        /// </summary>
        [Display(Name = "Icon", ResourceType = typeof(DisplayText))]
        public string Icon { get; set; }

        public List<string> Icons { get; set; }

        /// <summary>
        /// 车辆资料创建用户
        /// </summary>
        public int CreateUserID { get; set; }

        public int UpdateUserID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

        /// <summary>
        /// 经营范围列表
        /// </summary>
        public string BusinessScopeList { get; set; }
        /// <summary>
        /// 关联运管所列表
        /// </summary>
        public string TransportManagementList { get; set; }
        /// <summary>
        /// 车籍地
        /// </summary>
        [Display(Name = "CarToLand", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^[0-9]{6}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "CarToLandError")]
        public string CarToLand { get; set; }
        /// <summary>
        /// 道路运输证号
        /// </summary>
        [Display(Name = "RoadransportNo", ResourceType = typeof(DisplayText))]
        public string RoadransportNo { get; set; }
        /// <summary>
        /// 运输行业编码
        /// </summary>
        [Display(Name = "TransportIndustry", ResourceType = typeof(DisplayText))]
        public string TransportIndustryCode { get; set; }

        /// <summary>
        /// 车主姓名
        /// </summary>
        [Display(Name = "IsOwners", ResourceType = typeof(DisplayText))]
        public string OwnersName { get; set; }

        public int? OwnersID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }

    #endregion

    #region 维修单搜索下拉列表
    public class VehicleDropListModel
    {
        public int ID { get; set; }
        public string PlateNum { get; set; }
    }

    public class VehicleNameDropListModel
    {
        public int ID { get; set; }
        public string VehicleName { get; set; }
    }
    #endregion

    #region 下发消息实体
    public class SendMessageModel
    {
        /// <summary>
        /// 超速阈值
        /// </summary>
        public uint? OverspeedThreshold { get; set; }
        /// <summary>
        /// 引发超速报警所需的最小持续时间 （默认5s) 
        /// </summary>
        public uint MinimumDuration { get; set; }
        /// <summary>
        /// 连续驾驶时间门限(默认14400s)
        /// </summary>
        public uint ContinuousDrivingThreshold { get; set; }
        /// <summary>
        /// 最小休息时间（默认值1200s）
        /// </summary>
        public uint MinimumBreakTime { get; set; }
        /// <summary>
        /// 当天累计驾驶时间门限(默认值57600秒)
        /// </summary>
        public uint DrivingTimeThreshold { get; set; }
        /// <summary>
        /// 最长停车时间阈值(默认值3600秒)
        /// </summary>
        public uint MaximumParkingTime { get; set; }
        /// <summary>
        /// 车机最后一次里程数
        /// </summary>
        public double LastMiles { get; set; }
    }
    #endregion

    #region 车辆信息（车牌、终端号、SIM卡号、单位）
    /// <summary>
    /// 车辆信息（车牌、终端号、SIM卡号、单位）
    /// </summary>
    public class VerhicleInfo
    {
        /// <summary>
        /// 车牌
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string SimCode { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string StrucName { get; set; }
    }
    #endregion

}

using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Asiatek.Model
{
    #region 提货点
    #region  查询
    public class MGJH_PickUpTransportPointListModel
    {
        public int ID { get; set; }

        /// <summary>
        ///  设定类别（提货点、收货点）
        /// </summary>
        [Display(Name = "SettingType", ResourceType = typeof(DisplayText))]
        public int SettingType { get; set; }

        /// <summary>
        /// 提货地址名称
        /// </summary>
        [Display(Name = "PickUpAddressName", ResourceType = typeof(DisplayText))]
        public string AddressName { get; set; }

        /// <summary>
        /// 提货地址编码
        /// </summary>
        [Display(Name = "PickUpAddressCode", ResourceType = typeof(DisplayText))]
        public string AddressCode { get; set; }

        /// <summary>
        /// 提货点电子围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        public int EFType { get; set; }

        /// <summary>
        /// 提货点电子围栏信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        public string EFInfo { get; set; }

    }

    public class MGJH_PickUpTransportPointSearchModel
    {
        /// <summary>
        ///  设定类别（提货点、收货点）
        /// </summary>
        [Display(Name = "SettingType", ResourceType = typeof(DisplayText))]
        public int SettingType { get; set; }

        /// <summary>
        /// 提货地址名称
        /// </summary>
        [Display(Name = "PickUpAddressName", ResourceType = typeof(DisplayText))]
        public string AddressName { get; set; }

        /// <summary>
        /// 提货地址编码
        /// </summary>
        [Display(Name = "PickUpAddressCode", ResourceType = typeof(DisplayText))]
        public string AddressCode { get; set; }

    }
    #endregion

    #region 新增
    public class AddPickUpTransportPointModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 提货地址名称
        /// </summary>
        [Display(Name = "PickUpAddressName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddPickUpAddressNameExists", "MGJH_PickUpTransportPoint", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressName { get; set; }

        /// <summary>
        /// 提货地址编码
        /// </summary>
        [Display(Name = "PickUpAddressCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddPickUpAddressCodeExists", "MGJH_PickUpTransportPoint", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressCode { get; set; }

        /// <summary>
        /// 提货点电子围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int EFType { get; set; }

        /// <summary>
        /// 提货点电子围栏信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PleaseDrawElectricFenceFirst")]
        public string EFInfo { get; set; }
    }
    #endregion

    #region 修改
    public class EditPickUpTransportPointModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 提货地址名称
        /// </summary>
        [Display(Name = "PickUpAddressName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditPickUpAddressNameExists", "MGJH_PickUpTransportPoint", "Admin",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressName { get; set; }

        /// <summary>
        /// 提货地址编码
        /// </summary>
        [Display(Name = "PickUpAddressCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditPickUpAddressCodeExists", "MGJH_PickUpTransportPoint", "Admin",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressCode { get; set; }

        /// <summary>
        /// 提货点电子围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int EFType { get; set; }

        /// <summary>
        /// 提货点电子围栏信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PleaseDrawElectricFenceFirst")]
        public string EFInfo { get; set; }
    }
    #endregion
    #endregion


    #region 收货点
    #region  查询
    public class MGJH_ReceiveTransportPointListModel
    {
        public int ID { get; set; }

        /// <summary>
        ///  设定类别（提货点、收货点）
        /// </summary>
        [Display(Name = "SettingType", ResourceType = typeof(DisplayText))]
        public int SettingType { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "CustomerName", ResourceType = typeof(DisplayText))]
        public string CustomerName { get; set; }

        /// <summary>
        /// 收货点地址名称
        /// </summary>
        [Display(Name = "ReceiveAddressName", ResourceType = typeof(DisplayText))]
        public string AddressName { get; set; }

        /// <summary>
        /// 收货点地址编码
        /// </summary>
        [Display(Name = "ReceiveAddressCode", ResourceType = typeof(DisplayText))]
        public string AddressCode { get; set; }

        /// <summary>
        /// 收货地址所属区域
        /// </summary>
        [Display(Name = "ReceiveAddressArea", ResourceType = typeof(DisplayText))]
        public string AddressArea { get; set; }

        public int? SuperiorAddressID { get; set; }
        /// <summary>
        /// 上级收货地址
        /// </summary>
        [Display(Name = "SuperiorAddressName", ResourceType = typeof(DisplayText))]
        public string SuperiorAddressName { get; set; }

        /// <summary>
        /// 是否卸货点
        /// </summary>
        [Display(Name = "IsUnloadPoint", ResourceType = typeof(DisplayText))]
        public bool IsUnloadPoint { get; set; }

        /// <summary>
        /// 卸货点电子围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        public int? EFType { get; set; }

        /// <summary>
        /// 卸货点电子围栏信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        public string EFInfo { get; set; }

        /// <summary>
        /// 预计卸货时长
        /// </summary>
        [Display(Name = "UnloadTime", ResourceType = typeof(DisplayText))]
        public int? UnloadTime { get; set; }

        /// <summary>
        /// 误差
        /// </summary>
        [Display(Name = "UnloadTimeError", ResourceType = typeof(DisplayText))]
        public int? UnloadTimeError { get; set; }
    }

    public class MGJH_ReceiveTransportPointSearchModel
    {
        /// <summary>
        ///  设定类别（提货点、收货点）
        /// </summary>
        [Display(Name = "SettingType", ResourceType = typeof(DisplayText))]
        public int SettingType { get; set; }

        /// <summary>
        /// 收货地址名称
        /// </summary>
        [Display(Name = "ReceiveAddressName", ResourceType = typeof(DisplayText))]
        public string AddressName { get; set; }

        /// <summary>
        /// 提货地址编码
        /// </summary>
        [Display(Name = "ReceiveAddressCode", ResourceType = typeof(DisplayText))]
        public string AddressCode { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "CustomerName", ResourceType = typeof(DisplayText))]
        public string CustomerName { get; set; }

        /// <summary>
        /// 收货地址所属区域
        /// </summary>
        [Display(Name = "ReceiveAddressArea", ResourceType = typeof(DisplayText))]
        public string AddressArea { get; set; }
    }
    #endregion

    #region 新增
    public class AddReceiveTransportPointModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "CustomerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 收货点地址名称
        /// </summary>
        [Display(Name = "ReceiveAddressName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddReceiveAddressNameExists", "MGJH_ReceiveTransportPoint", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressName { get; set; }

        /// <summary>
        /// 收货点地址编码
        /// </summary>
        [Display(Name = "ReceiveAddressCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddReceiveAddressCodeExists", "MGJH_ReceiveTransportPoint", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressCode { get; set; }

        /// <summary>
        /// 收货地址所属区域
        /// </summary>
        [Display(Name = "ReceiveAddressArea", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string AddressArea { get; set; }

        /// <summary>
        /// 上级收货地址
        /// </summary>
        [Display(Name = "SuperiorAddressName", ResourceType = typeof(DisplayText))]
        public int? SuperiorAddressID { get; set; }
        public IEnumerable<SelectListItem> SuperiorAddressSelectList { get; set; }

        /// <summary>
        /// 是否卸货点
        /// </summary>
        [Display(Name = "IsUnloadPoint", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool IsUnloadPoint { get; set; }

        /// <summary>
        /// 卸货点电子围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        public int? EFType { get; set; }

        /// <summary>
        /// 卸货点电子围栏信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        public string EFInfo { get; set; }

        /// <summary>
        /// 预计卸货时长
        /// </summary>
        [Display(Name = "UnloadTime", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ValueIsNum")]
        public int? UnloadTime { get; set; }

        /// <summary>
        /// 误差
        /// </summary>
        [Display(Name = "UnloadTimeError", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ValueIsNum")]
        public int? UnloadTimeError { get; set; }

        /// <summary>
        /// 同步收货地址的记录ID
        /// </summary>
        public int? SourceID { get; set; }
    }

    public class SuperiorAddressModel
    {
        public int ID { get; set; }
        public string AddressName { get; set; }
    }
    #endregion

    #region 修改
    public class EditReceiveTransportPointModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "CustomerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 收货点地址名称
        /// </summary>
        [Display(Name = "ReceiveAddressName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditReceiveAddressNameExists", "MGJH_ReceiveTransportPoint", "Admin",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressName { get; set; }

        /// <summary>
        /// 收货点地址编码
        /// </summary>
        [Display(Name = "ReceiveAddressCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditReceiveAddressCodeExists", "MGJH_ReceiveTransportPoint", "Admin",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressCode { get; set; }

        /// <summary>
        /// 收货地址所属区域
        /// </summary>
        [Display(Name = "ReceiveAddressArea", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string AddressArea { get; set; }

        /// <summary>
        /// 上级收货地址
        /// </summary>
        [Display(Name = "SuperiorAddressName", ResourceType = typeof(DisplayText))]
        public int? SuperiorAddressID { get; set; }
        public IEnumerable<SelectListItem> SuperiorAddressSelectList { get; set; }

        /// <summary>
        /// 是否卸货点
        /// </summary>
        [Display(Name = "IsUnloadPoint", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool IsUnloadPoint { get; set; }

        /// <summary>
        /// 卸货点电子围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        public int? EFType { get; set; }

        /// <summary>
        /// 卸货点电子围栏信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        public string EFInfo { get; set; }

        /// <summary>
        /// 预计卸货时长
        /// </summary>
        [Display(Name = "UnloadTime", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ValueIsNum")]
        public int? UnloadTime { get; set; }

        /// <summary>
        /// 误差
        /// </summary>
        [Display(Name = "UnloadTimeError", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ValueIsNum")]
        public int? UnloadTimeError { get; set; }
    }
    #endregion
    #endregion


    #region 同步收货点
    public class MGJH_SynchroReceivePointListModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "CustomerName", ResourceType = typeof(DisplayText))]
        public string CustomerName { get; set; }

        /// <summary>
        /// 收货点地址名称
        /// </summary>
        [Display(Name = "ReceiveAddressName", ResourceType = typeof(DisplayText))]
        public string AddressName { get; set; }

        /// <summary>
        /// 收货点地址编码
        /// </summary>
        [Display(Name = "ReceiveAddressCode", ResourceType = typeof(DisplayText))]
        public string AddressCode { get; set; }

        /// <summary>
        /// 收货地址所属区域
        /// </summary>
        [Display(Name = "ReceiveAddressArea", ResourceType = typeof(DisplayText))]
        public string AddressArea { get; set; }

        //dbo.MGJH_TransportPointSetting中字段，关联dbo.MGJH_SynchroReceiveAddress中ID字段
        public int? SourceID { get; set; }

    }

    public class MGJH_SynchroReceivePointSearchModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "CustomerName", ResourceType = typeof(DisplayText))]
        public string CustomerName { get; set; }

        /// <summary>
        /// 收货点地址名称
        /// </summary>
        [Display(Name = "ReceiveAddressName", ResourceType = typeof(DisplayText))]
        public string AddressName { get; set; }

        /// <summary>
        /// 收货点地址编码
        /// </summary>
        [Display(Name = "ReceiveAddressCode", ResourceType = typeof(DisplayText))]
        public string AddressCode { get; set; }

        /// <summary>
        /// 收货地址所属区域
        /// </summary>
        [Display(Name = "ReceiveAddressArea", ResourceType = typeof(DisplayText))]
        public string AddressArea { get; set; }

        /// <summary>
        /// 是否已经同步
        /// </summary>
        [Display(Name = "IsSynchro", ResourceType = typeof(DisplayText))]
        public int IsSynchro { get; set; }

    }

    public class EditSynchroReceivePointModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [Display(Name = "CustomerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 收货点地址名称
        /// </summary>
        [Display(Name = "ReceiveAddressName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddReceiveAddressNameExists", "MGJH_ReceiveTransportPoint", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressName { get; set; }

        /// <summary>
        /// 收货点地址编码
        /// </summary>
        [Display(Name = "ReceiveAddressCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddReceiveAddressCodeExists", "MGJH_ReceiveTransportPoint", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string AddressCode { get; set; }

        /// <summary>
        /// 收货地址所属区域
        /// </summary>
        [Display(Name = "ReceiveAddressArea", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string AddressArea { get; set; }

        /// <summary>
        /// 上级收货地址
        /// </summary>
        [Display(Name = "SuperiorAddressName", ResourceType = typeof(DisplayText))]
        public int? SuperiorAddressID { get; set; }
        public IEnumerable<SelectListItem> SuperiorAddressSelectList { get; set; }

        /// <summary>
        /// 是否卸货点
        /// </summary>
        [Display(Name = "IsUnloadPoint", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool IsUnloadPoint { get; set; }

        /// <summary>
        /// 卸货点电子围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        public int? EFType { get; set; }

        /// <summary>
        /// 卸货点电子围栏信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        public string EFInfo { get; set; }

        /// <summary>
        /// 预计卸货时长
        /// </summary>
        [Display(Name = "UnloadTime", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ValueIsNum")]
        public int? UnloadTime { get; set; }

        /// <summary>
        /// 误差
        /// </summary>
        [Display(Name = "UnloadTimeError", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ValueIsNum")]
        public int? UnloadTimeError { get; set; }
    }
    #endregion
}

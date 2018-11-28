using Asiatek.CustomDataAnnotations;
using Asiatek.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Asiatek.Model
{
    #region 查询
    public class EmployeeInfoListModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Display(Name = "EmployeeID", ResourceType = typeof(DisplayText))]
        public string EmployeeID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "EmployeeName", ResourceType = typeof(DisplayText))]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "EmployeeGender", ResourceType = typeof(DisplayText))]
        public bool EmployeeGender { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        [Display(Name = "CertificateCode", ResourceType = typeof(DisplayText))]
        public string CertificateCode { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "ContactPhone", ResourceType = typeof(DisplayText))]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 紧急联系电话
        /// </summary>
        [Display(Name = "EmergePhone", ResourceType = typeof(DisplayText))]
        public string EmergePhone { get; set; }

        /// <summary>
        /// 是否为驾驶员
        /// </summary>
        [Display(Name = "IsDriver", ResourceType = typeof(DisplayText))]
        public bool IsDriver { get; set; }

        /// <summary>
        /// 是否为押运员
        /// </summary>
        [Display(Name = "IsCarrier", ResourceType = typeof(DisplayText))]
        public bool IsCarrier { get; set; }

        /// <summary>
        /// 准驾车型
        /// </summary>
        [Display(Name = "DriveType", ResourceType = typeof(DisplayText))]
        public string DriveTypeName { get; set; }

        /// <summary>
        /// 驾照状态
        /// </summary>
        [Display(Name = "DriveLicenseState", ResourceType = typeof(DisplayText))]
        public string DriveLicenseStateName { get; set; }
    }


    public class EmployeeInfoSearchModel
    {
        /// <summary>
        /// 工号
        /// </summary>
        [Display(Name = "EmployeeID", ResourceType = typeof(DisplayText))]
        public string EmployeeID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "EmployeeName", ResourceType = typeof(DisplayText))]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 驾照状态
        /// </summary>
        [Display(Name = "DriveLicenseState", ResourceType = typeof(DisplayText))]
        public int DriveLicenseStateID { get; set; }

        /// <summary>
        /// 驾照状态下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> DriveLicenseStateSelectList { get; set; }
    }


    public class DriveLicenseStateSelectModel
    {
        public int DriveLicenseStateID { get; set; }
        public string DriveLicenseStateName { get; set; }
    }
    #endregion

    #region 查询 新版
    public class EmployeeInfoPageModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "EmployeeName", ResourceType = typeof(DisplayText))]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "EmployeeGender", ResourceType = typeof(DisplayText))]
        public bool EmployeeGender { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        [Display(Name = "CertificateCode", ResourceType = typeof(DisplayText))]
        public string CertificateCode { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "ContactPhone", ResourceType = typeof(DisplayText))]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 紧急联系电话
        /// </summary>
        [Display(Name = "EmergePhone", ResourceType = typeof(DisplayText))]
        public string EmergePhone { get; set; }

        /// <summary>
        /// 是否为驾驶员
        /// </summary>
        [Display(Name = "IsDriver", ResourceType = typeof(DisplayText))]
        public bool IsDriver { get; set; }

        /// <summary>
        /// 是否为押运员
        /// </summary>
        [Display(Name = "IsCarrier", ResourceType = typeof(DisplayText))]
        public bool IsCarrier { get; set; }
        /// <summary>
        /// 隶属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public string StrucName { get; set; }

    }


    public class EmployeeInfoFindModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "EmployeeName", ResourceType = typeof(DisplayText))]
        public string EmployeeName { get; set; }
        /// <summary>
        /// 证件号
        /// </summary>
        [Display(Name = "CertificateCode", ResourceType = typeof(DisplayText))]
        public string CertificateCode { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "ContactPhone", ResourceType = typeof(DisplayText))]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 是否为驾驶员
        /// </summary>
        [Display(Name = "IsDriver", ResourceType = typeof(DisplayText))]
        public int IsDrivers { get; set; }

        /// <summary>
        /// 是否为押运员
        /// </summary>
        [Display(Name = "IsCarrier", ResourceType = typeof(DisplayText))]
        public int IsCarriers { get; set; }
        /// <summary>
        /// 隶属单位ID
        /// </summary>
        public int? SearchStrucID { get; set; }
        /// <summary>
        /// 隶属单位
        /// </summary>
       [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public string SearchStrucName { get; set; }
    }
    #endregion

    #region 新增
    public class EmployeeInfoAddModel
    {
        /// <summary>
        /// 工号
        /// </summary>
        [Display(Name = "EmployeeID", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [AsiatekRemote("CheckAddEmployeeIDExists", "EmployeeInfo", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string EmployeeID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "EmployeeName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "EmployeeGender", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool EmployeeGender { get; set; }

        /// <summary>
        /// 出生年月日
        /// </summary>
        [Display(Name = "BirthDate", ResourceType = typeof(DisplayText))]
        [LegalDate(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateError")]
        public string BirthDate { get; set; }

        /// <summary>
        /// 隶属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int StrucID { get; set; }

        /// <summary>
        /// 证件类型编号
        /// </summary>
        [Display(Name = "CertificateType", ResourceType = typeof(DisplayText))]
        public int CertificateTypeID { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public IEnumerable<SelectListItem> CertificateTypeSelectList { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        [Display(Name = "CertificateCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string CertificateCode { get; set; }

        /// <summary>
        /// 发证机关
        /// </summary>
        [Display(Name = "CertificateOffice", ResourceType = typeof(DisplayText))]
        public string CertificateOffice { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        [Display(Name = "ValidPeriod", ResourceType = typeof(DisplayText))]
        public string ValidStartTime { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        [Display(Name = "To", ResourceType = typeof(DisplayText))]
        public string ValidEndTime { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "ContactPhone", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Display(Name = "ContactAddress", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string ContactAddress { get; set; }

        /// <summary>
        /// 是否为驾驶员
        /// </summary>
        [Display(Name = "IsDriver", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool IsDriver { get; set; }

        /// <summary>
        /// 驾驶证号
        /// </summary>
        [Display(Name = "DriveCode", ResourceType = typeof(DisplayText))]
        public string DriveCode { get; set; }

        /// <summary>
        /// 驾驶证有效期
        /// </summary>
        [Display(Name = "DriveCodeValidTime", ResourceType = typeof(DisplayText))]
        public string DriveCodeValidTime { get; set; }

        /// <summary>
        /// 准驾车型编号
        /// </summary>
        [Display(Name = "DriveType", ResourceType = typeof(DisplayText))]
        public string DriveTypeID { get; set; }

        /// <summary>
        /// 准驾车型
        /// </summary>
        public IEnumerable<SelectListItem> DriveTypeSelectList { get; set; }

        /// <summary>
        /// 驾照状态编号
        /// </summary>
        [Display(Name = "DriveLicenseState", ResourceType = typeof(DisplayText))]
        public int DriveLicenseStateID { get; set; }

        /// <summary>
        /// 驾照状态
        /// </summary>
        public IEnumerable<SelectListItem> DriveLicenseStateSelectList { get; set; }

        /// <summary>
        /// 是否为押运员
        /// </summary>
        [Display(Name = "IsCarrier", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool IsCarrier { get; set; }

        /// <summary>
        /// 押运员资格证
        /// </summary>
        [Display(Name = "CarrierCode", ResourceType = typeof(DisplayText))]
        public string CarrierCode { get; set; }


        /// <summary>
        /// 紧急联系电话
        /// </summary>
        [Display(Name = "EmergePhone", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string EmergePhone { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }

    public class CertificateTypeSelectModel
    {
        public int CertificateTypeID { get; set; }
        public string CertificateTypeName { get; set; }
    }

    public class DriveTypeSelectModel
    {
        public string DriveTypeID { get; set; }
        public string DriveTypeName { get; set; }
    }
    #endregion


    #region 修改
    public class EmployeeInfoEditModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Display(Name = "EmployeeID", ResourceType = typeof(DisplayText))]
        public string EmployeeID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "EmployeeName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "EmployeeGender", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool EmployeeGender { get; set; }

        /// <summary>
        /// 出生年月日
        /// </summary>
        [Display(Name = "BirthDate", ResourceType = typeof(DisplayText))]
        [LegalDate(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateError")]
        public string BirthDate { get; set; }

        /// <summary>
        /// 隶属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int StrucID { get; set; }

        /// <summary>
        /// 证件类型编号
        /// </summary>
        [Display(Name = "CertificateType", ResourceType = typeof(DisplayText))]
        public int CertificateTypeID { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public IEnumerable<SelectListItem> CertificateTypeSelectList { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        [Display(Name = "CertificateCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string CertificateCode { get; set; }

        /// <summary>
        /// 发证机关
        /// </summary>
        [Display(Name = "CertificateOffice", ResourceType = typeof(DisplayText))]
        public string CertificateOffice { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        [Display(Name = "ValidPeriod", ResourceType = typeof(DisplayText))]
        public string ValidStartTime { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        [Display(Name = "To", ResourceType = typeof(DisplayText))]
        public string ValidEndTime { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "ContactPhone", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Display(Name = "ContactAddress", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string ContactAddress { get; set; }

        /// <summary>
        /// 是否为驾驶员
        /// </summary>
        [Display(Name = "IsDriver", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool IsDriver { get; set; }

        /// <summary>
        /// 驾驶证号
        /// </summary>
        [Display(Name = "DriveCode", ResourceType = typeof(DisplayText))]
        public string DriveCode { get; set; }

        /// <summary>
        /// 驾驶证有效期
        /// </summary>
        [Display(Name = "DriveCodeValidTime", ResourceType = typeof(DisplayText))]
        public string DriveCodeValidTime { get; set; }

        /// <summary>
        /// 准驾车型编号
        /// </summary>
        [Display(Name = "DriveType", ResourceType = typeof(DisplayText))]
        public string DriveTypeID { get; set; }

        /// <summary>
        /// 准驾车型
        /// </summary>
        public IEnumerable<SelectListItem> DriveTypeSelectList { get; set; }

        /// <summary>
        /// 驾照状态编号
        /// </summary>
        [Display(Name = "DriveLicenseState", ResourceType = typeof(DisplayText))]
        public int DriveLicenseStateID { get; set; }

        /// <summary>
        /// 驾照状态
        /// </summary>
        public IEnumerable<SelectListItem> DriveLicenseStateSelectList { get; set; }

        /// <summary>
        /// 是否为押运员
        /// </summary>
        [Display(Name = "IsCarrier", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool IsCarrier { get; set; }

        /// <summary>
        /// 押运员资格证
        /// </summary>
        [Display(Name = "CarrierCode", ResourceType = typeof(DisplayText))]
        public string CarrierCode { get; set; }

        /// <summary>
        /// 紧急联系电话
        /// </summary>
        [Display(Name = "EmergePhone", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string EmergePhone { get; set; }

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

    #region 员工信息信息新增修改新版本
     #region 注释代码 20171013 证件号 证件类型改成非必填
    //public class EditEmployeeInfoModel
    //{
    //    public int ID { get; set; }

    //    /// <summary>
    //    /// 姓名
    //    /// </summary>
    //    [Display(Name = "EmployeeName", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public string EmployeeName { get; set; }

    //    /// <summary>
    //    /// 性别
    //    /// </summary>
    //    [Display(Name = "EmployeeGender", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public bool EmployeeGender { get; set; }

    //    /// <summary>
    //    /// 隶属单位
    //    /// </summary>
    //    [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
    //    public int? StrucID { get; set; }
    //    /// <summary>
    //    /// 原隶属单位
    //    /// </summary>
    //    public int OldStrucID { get; set; }

    //    public string StrucName { get; set; }

    //    /// <summary>
    //    /// 证件类型编号
    //    /// </summary>
    //    [Display(Name = "CertificateType", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
    //    public int CertificateTypeID { get; set; }

    //    /// <summary>
    //    /// 证件类型
    //    /// </summary>
    //    public IEnumerable<SelectListItem> CertificateTypeSelectList { get; set; }

    //    /// <summary>
    //    /// 证件号
    //    /// </summary>
    //    [Display(Name = "CertificateCode", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    [RegularExpression(@"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "IDcardError")]
    //    [AsiatekRemote("CheckEditCertificateCodeExists", "EmployeeInfo", "Admin", HttpMethod = "POST",
    //ErrorMessageResourceType = typeof(DataAnnotations),
    //ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
    //    public string CertificateCode { get; set; }

    //    /// <summary>
    //    /// 联系电话
    //    /// </summary>
    //    [Display(Name = "ContactPhone", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    [ChinesePhone(
    //        ErrorMessageResourceType = typeof(DataAnnotations),
    //        ErrorMessageResourceName = "ChinesePhoneError")]
    //    public string ContactPhone { get; set; }

    //    /// <summary>
    //    /// 联系地址
    //    /// </summary>
    //    [Display(Name = "ContactAddress", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public string ContactAddress { get; set; }

    //    /// <summary>
    //    /// 是否为驾驶员
    //    /// </summary>
    //    [Display(Name = "IsDriver", ResourceType = typeof(DisplayText))]
    //    public bool IsDriver { get; set; }

    //    /// <summary>
    //    /// 是否为押运员
    //    /// </summary>
    //    [Display(Name = "IsCarrier", ResourceType = typeof(DisplayText))]
    //    public bool IsCarrier { get; set; }

    //    /// <summary>
    //    /// 驾驶证号
    //    /// </summary>
    //    [Display(Name = "DriveCode", ResourceType = typeof(DisplayText))]
    //    public string DriveCode { get; set; }

    //    /// <summary>
    //    /// 驾驶证有效期
    //    /// </summary>
    //    [Display(Name = "DriveCodeValidTime", ResourceType = typeof(DisplayText))]
    //    public string DriveCodeValidTime { get; set; }

    //    /// <summary>
    //    /// 押运员资格证
    //    /// </summary>
    //    [Display(Name = "CarrierCode", ResourceType = typeof(DisplayText))]
    //    public string CarrierCode { get; set; }

    //    /// <summary>
    //    /// 紧急联系电话
    //    /// </summary>
    //    [Display(Name = "EmergePhone", ResourceType = typeof(DisplayText))]
    //    [ChinesePhone(
    //        ErrorMessageResourceType = typeof(DataAnnotations),
    //        ErrorMessageResourceName = "ChinesePhoneError")]
    //    public string EmergePhone { get; set; }

    //    /// <summary>
    //    /// 备注
    //    /// </summary>
    //    [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
    //    [StringLength(500,
    //        ErrorMessageResourceType = typeof(DataAnnotations),
    //        ErrorMessageResourceName = "MaxLength")]
    //    public string Remark { get; set; }
    //    /// <summary>
    //    /// 押运员证有效期
    //    /// </summary>
    //    [Display(Name = "CarrieCodeValidTime", ResourceType = typeof(DisplayText))]
    //    public string CarrieCodeValidTime { get; set; }

    //    #region 屏蔽字段
    //    ///// <summary>
    //    ///// 工号
    //    ///// </summary>
    //    //[Display(Name = "EmployeeID", ResourceType = typeof(DisplayText))]
    //    //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    //[AsiatekRemote("CheckEditEmployeeIDExists", "EmployeeInfo", "Admin", HttpMethod = "POST",
    //    //    ErrorMessageResourceType = typeof(DataAnnotations),
    //    //    ErrorMessageResourceName = "FieldExists",AdditionalFields="ID")]
    //    //public string EmployeeID { get; set; }
    //    ///// <summary>
    //    ///// 出生年月日
    //    ///// </summary>
    //    //[Display(Name = "BirthDate", ResourceType = typeof(DisplayText))]
    //    //[LegalDate(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateError")]
    //    //public string BirthDate { get; set; }

    //    ///// <summary>
    //    ///// 发证机关
    //    ///// </summary>
    //    //[Display(Name = "CertificateOffice", ResourceType = typeof(DisplayText))]
    //    //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    //public string CertificateOffice { get; set; }

    //    ///// <summary>
    //    ///// 有效期开始时间
    //    ///// </summary>
    //    //[Display(Name = "ValidPeriod", ResourceType = typeof(DisplayText))]
    //    //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    //public string ValidStartTime { get; set; }

    //    ///// <summary>
    //    ///// 有效期结束时间
    //    ///// </summary>
    //    //[Display(Name = "To", ResourceType = typeof(DisplayText))]
    //    //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    //public string ValidEndTime { get; set; }

    //    ///// <summary>
    //    ///// 准驾车型编号
    //    ///// </summary>
    //    //[Display(Name = "DriveType", ResourceType = typeof(DisplayText))]
    //    //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
    //    //public string DriveTypeID { get; set; }

    //    ///// <summary>
    //    ///// 准驾车型
    //    ///// </summary>
    //    //public IEnumerable<SelectListItem> DriveTypeSelectList { get; set; }

    //    ///// <summary>
    //    ///// 驾照状态编号
    //    ///// </summary>
    //    //[Display(Name = "DriveLicenseState", ResourceType = typeof(DisplayText))]
    //    //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
    //    //public int DriveLicenseStateID { get; set; }

    //    ///// <summary>
    //    ///// 驾照状态
    //    ///// </summary>
    //    //public IEnumerable<SelectListItem> DriveLicenseStateSelectList { get; set; }
    //    #endregion

    //}
    #endregion

    public class EditEmployeeInfoModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "EmployeeName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "EmployeeGender", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public bool EmployeeGender { get; set; }

        /// <summary>
        /// 隶属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? StrucID { get; set; }
        /// <summary>
        /// 原隶属单位
        /// </summary>
        public int OldStrucID { get; set; }

        public string StrucName { get; set; }

        /// <summary>
        /// 证件类型编号
        /// </summary>
        [Display(Name = "CertificateType", ResourceType = typeof(DisplayText))]
        public int CertificateTypeID { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public IEnumerable<SelectListItem> CertificateTypeSelectList { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        [Display(Name = "CertificateCode", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "IDcardError")]
        [AsiatekRemote("CheckEditCertificateCodeExists", "EmployeeInfo", "Admin", HttpMethod = "POST",
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string CertificateCode { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "ContactPhone", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Display(Name = "ContactAddress", ResourceType = typeof(DisplayText))]
        //[Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string ContactAddress { get; set; }

        /// <summary>
        /// 是否为驾驶员
        /// </summary>
        [Display(Name = "IsDriver", ResourceType = typeof(DisplayText))]
        public bool IsDriver { get; set; }

        /// <summary>
        /// 是否为押运员
        /// </summary>
        [Display(Name = "IsCarrier", ResourceType = typeof(DisplayText))]
        public bool IsCarrier { get; set; }
        /// <summary>
        /// 是否为车主
        /// </summary>
        [Display(Name = "IsOwners", ResourceType = typeof(DisplayText))]
        public bool IsOwners { get; set; }
        /// <summary>
        /// 驾驶证号
        /// </summary>
        [Display(Name = "DriveCode", ResourceType = typeof(DisplayText))]
        public string DriveCode { get; set; }

        /// <summary>
        /// 驾驶证有效期
        /// </summary>
        [Display(Name = "DriveCodeValidTime", ResourceType = typeof(DisplayText))]
        public string DriveCodeValidTime { get; set; }

        /// <summary>
        /// 押运员资格证
        /// </summary>
        [Display(Name = "CarrierCode", ResourceType = typeof(DisplayText))]
        public string CarrierCode { get; set; }

        /// <summary>
        /// 紧急联系电话
        /// </summary>
        [Display(Name = "EmergePhone", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string EmergePhone { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
        /// <summary>
        /// 押运员证有效期
        /// </summary>
        [Display(Name = "CarrieCodeValidTime", ResourceType = typeof(DisplayText))]
        public string CarrieCodeValidTime { get; set; }
    }
  
    #endregion



    #region 员工信息下拉实体
    public class EmployeeInfoDDLModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 是否是驾驶员
        /// </summary>
        public bool IsDriver { get; set; }
        /// <summary>
        /// 是否押运员
        /// </summary>
        public bool IsCarrier { get; set; }
    }
    #endregion

    #region 员工信息下拉实体
    public class NewEmployeeInfoDDLModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName { get; set; }
    }
    #endregion

}

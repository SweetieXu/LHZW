using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Asiatek.Model
{
    //#region  查询
    //public class ElectricFenceListModel
    //{
    //    /// <summary>
    //    /// 电子围栏编号
    //    /// </summary>
    //    public int ID { get; set; }
    //    /// <summary>
    //    /// 围栏名称
    //    /// </summary>
    //    [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
    //    public string FenceName { get; set; }
    //    /// <summary>
    //    /// 围栏类型
    //    /// </summary>
    //    [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
    //    public int FenceType { get; set; }
    //    public string FenceTypeInfo { get; set; }
    //    /// <summary>
    //    /// 报警类型
    //    /// </summary>
    //    [Display(Name = "AlarmType", ResourceType = typeof(DisplayText))]
    //    public int AlarmType { get; set; }
    //    /// <summary>
    //    /// 启用状态
    //    /// </summary>
    //    [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
    //    public bool FenceState { get; set; }
    //    /// <summary>
    //    /// 创建时间
    //    /// </summary>
    //    [Display(Name = "CreateTime", ResourceType = typeof(DisplayText))]
    //    public DateTime CreateTime { get; set; }
    //    /// <summary>
    //    /// 有效期开始日期
    //    /// </summary>
    //    [Display(Name = "ValidStartTime", ResourceType = typeof(DisplayText))]
    //    public DateTime StartTime { get; set; }
    //    /// <summary>
    //    /// 有效期结束日期
    //    /// </summary>
    //    [Display(Name = "ValidEndTime", ResourceType = typeof(DisplayText))]
    //    public DateTime EndTime { get; set; }

    //    public string TerminalCode { get; set; }
    //    public int TerminalID { get; set; }
    //    /// <summary>
    //    /// 车辆编号
    //    /// </summary>
    //    [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
    //    public long VehicleID { get; set; }
    //    /// <summary>
    //    /// 车辆代号
    //    /// </summary>
    //    [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
    //    public string VehicleName { get; set; }
    //    //[Display(Name = "车主姓名")]
    //    //public string ClientName { get; set; }
    //    /// <summary>
    //    /// 所属单位
    //    /// </summary>
    //    [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
    //    public string StrucName { get; set; }
    //    /// <summary>
    //    /// 昵称
    //    /// </summary>
    //    [Display(Name = "CreateUser", ResourceType = typeof(DisplayText))]
    //    public string NickName { get; set; }
    //}

    //public class ElectricFenceSearchModel
    //{
    //    /// <summary>
    //    /// 围栏名称
    //    /// </summary>
    //    [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
    //    public string FenceName { get; set; }
    //    /// <summary>
    //    /// 围栏类型
    //    /// </summary>
    //    [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
    //    public int FenceType { get; set; }
    //    /// <summary>
    //    /// 报警类型
    //    /// </summary>
    //    [Display(Name = "AlarmType", ResourceType = typeof(DisplayText))]
    //    public int AlarmType { get; set; }
    //    /// <summary>
    //    /// 开始时间
    //    /// </summary>
    //    [Display(Name = "StartTime", ResourceType = typeof(DisplayText))]
    //    public string StartTime { get; set; }
    //    /// <summary>
    //    /// 结束时间
    //    /// </summary>
    //    [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
    //    public string EndTime { get; set; }
    //    /// <summary>
    //    /// 所属单位ID
    //    /// </summary>
    //    [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
    //    public int SearchStrucID { get; set; }
    //}
    //#endregion

    //#region  新增
    //public class AddElectricFenceModel
    //{
    //    public int ID { get; set; }
    //    /// <summary>
    //    /// 围栏名称
    //    /// </summary>
    //    [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    [Remote("CheckAddFenceNameExists", "ElectricFence", "Admin", HttpMethod = "POST",
    //        ErrorMessageResourceType = typeof(DataAnnotations),
    //        ErrorMessageResourceName = "FieldExists")]
    //    public string FenceName { get; set; }
    //    /// <summary>
    //    /// 围栏类型
    //    /// </summary>
    //    [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public int FenceType { get; set; }
    //    /// <summary>
    //    /// 围栏类型信息
    //    /// </summary>
    //    [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PleaseDrawElectricFenceFirst")]
    //    public string FenceTypeInfo { get; set; }
    //    /// <summary>
    //    /// 报警类型
    //    /// </summary>
    //    [Display(Name = "AlarmType", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public int AlarmType { get; set; }
    //    /// <summary>
    //    /// 启用状态
    //    /// </summary>
    //    [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public bool FenceState { get; set; }
    //    /// <summary>
    //    /// 有效期开始时间
    //    /// </summary>
    //    [Display(Name = "ValidStartTime", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public string StartTime { get; set; }
    //    /// <summary>
    //    /// 有效期结束时间
    //    /// </summary>
    //    [Display(Name = "ValidEndTime", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public string EndTime { get; set; }
    //}
    //#endregion

    //#region  修改
    //public class EditElectricFenceModel
    //{
    //    public int ID { get; set; }
    //    /// <summary>
    //    /// 围栏名称
    //    /// </summary>
    //    [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    [Remote("CheckEditFenceNameExists", "ElectricFence", "Admin",
    //        AdditionalFields = "ID", HttpMethod = "POST",
    //        ErrorMessageResourceType = typeof(DataAnnotations),
    //        ErrorMessageResourceName = "FieldExists")]
    //    public string FenceName { get; set; }
    //    /// <summary>
    //    /// 围栏类型
    //    /// </summary>
    //    [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public int FenceType { get; set; }
    //    /// <summary>
    //    /// 围栏类型信息
    //    /// </summary>
    //    [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PleaseDrawElectricFenceFirst")]
    //    public string FenceTypeInfo { get; set; }
    //    /// <summary>
    //    /// 报警类型
    //    /// </summary>
    //    [Display(Name = "AlarmType", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public int AlarmType { get; set; }
    //    /// <summary>
    //    /// 启用状态
    //    /// </summary>
    //    [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public bool FenceState { get; set; }
    //    /// <summary>
    //    /// 开始时间
    //    /// </summary>
    //    [Display(Name = "ValidStartTime", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public DateTime StartTime { get; set; }
    //    /// <summary>
    //    /// 结束时间
    //    /// </summary>
    //    [Display(Name = "ValidEndTime", ResourceType = typeof(DisplayText))]
    //    [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
    //    public DateTime EndTime { get; set; }
    //}
    //#endregion

    #region  查询
    public class ElectricFenceListModel
    {
        /// <summary>
        /// 电子围栏编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 围栏名称
        /// </summary>
        [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
        public string FenceName { get; set; }
        /// <summary>
        /// 围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        public int FenceType { get; set; }
        public string FenceTypeInfo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "CreateTime", ResourceType = typeof(DisplayText))]
        public DateTime CreateTime { get; set; }
        public string TerminalCode { get; set; }
        public int TerminalID { get; set; }
        /// <summary>
        /// 车辆编号
        /// </summary>
        [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
        public long VehicleID { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }
        //[Display(Name = "车主姓名")]
        //public string ClientName { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public string StrucName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "CreateUser", ResourceType = typeof(DisplayText))]
        public string NickName { get; set; }
        /// <summary>
        /// 客户状态
        /// </summary>
        [Display(Name = "Status", ResourceType = typeof(DisplayText))]
        public int CustomerStatus { get; set; }

        public int? PropertyID { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        [Display(Name = "PropertyName", ResourceType = typeof(DisplayText))]
        public string PropertyName { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
        public bool FenceState { get; set; }
        /// <summary>
        /// 有效期开始日期
        /// </summary>
        [Display(Name = "ValidStartTime", ResourceType = typeof(DisplayText))]
        public DateTime ValidStartTime { get; set; }
        /// <summary>
        /// 有效期结束日期
        /// </summary>
        [Display(Name = "ValidEndTime", ResourceType = typeof(DisplayText))]
        public DateTime ValidEndTime { get; set; }
        /// <summary>
        /// 电子围栏编码，江苏医药有用
        /// </summary>
        [Display(Name = "FenceCode", ResourceType = typeof(DisplayText))]
        public string FenceCode { get; set; }
    }

    public class ElectricFenceSearchModel
    {
        /// <summary>
        /// 围栏名称
        /// </summary>
        [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
        public string FenceName { get; set; }
        /// <summary>
        /// 围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        public int FenceType { get; set; }
        /// <summary>
        /// 所属单位ID
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public int SearchStrucID { get; set; }
    }
    #endregion

    #region  新增
    public class AddElectricFenceModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 围栏名称
        /// </summary>
        [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddFenceNameExists", "ElectricFence", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FenceName { get; set; }
        /// <summary>
        /// 围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int FenceType { get; set; }
        /// <summary>
        /// 围栏类型信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PleaseDrawElectricFenceFirst")]
        public string FenceTypeInfo { get; set; }
        /// <summary>
        /// 配置ID
        /// </summary>
        [Display(Name = "PropertyName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int PropertyID { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        public IEnumerable<SelectListItem> PropertyNamesSelectList { get; set; }
        /// <summary>
        /// 电子围栏编码，江苏医药有用
        /// </summary>
        [Display(Name = "FenceCode", ResourceType = typeof(DisplayText))]
        [Remote("CheckAddFenceCodeExists", "ElectricFence", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FenceCode { get; set; }
    }

    public class EFPropertyModel 
    {
        public int PropertyID { get; set; }
        public string PropertyName { get; set; }
    }

    //南钢嘉华新增电子围栏  无电子围栏配置信息
    public class NGJH_AddElectricFenceModel 
    {
        public int ID { get; set; }
        /// <summary>
        /// 围栏名称
        /// </summary>
        [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckAddFenceNameExists", "ElectricFence", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FenceName { get; set; }
        /// <summary>
        /// 围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int FenceType { get; set; }
        /// <summary>
        /// 围栏类型信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PleaseDrawElectricFenceFirst")]
        public string FenceTypeInfo { get; set; }

        /// <summary>
        /// 状态----三个值“在用客户”、“潜在客户”、“不在用客户”
        /// </summary>
        [Display(Name = "Status", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int CustomerStatus { get; set; }
    }
    #endregion

    #region  修改
    public class EditElectricFenceModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 围栏名称
        /// </summary>
        [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditFenceNameExists", "ElectricFence", "Admin",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FenceName { get; set; }
        /// <summary>
        /// 围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int FenceType { get; set; }
        /// <summary>
        /// 围栏类型信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PleaseDrawElectricFenceFirst")]
        public string FenceTypeInfo { get; set; }
        /// <summary>
        /// 配置ID
        /// </summary>
        [Display(Name = "PropertyName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int PropertyID { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        public IEnumerable<SelectListItem> PropertyNamesSelectList { get; set; }

        /// <summary>
        /// 电子围栏编码，江苏医药有用
        /// </summary>
        [Display(Name = "FenceCode", ResourceType = typeof(DisplayText))]
        [Remote("CheckEditFenceCodeExists", "ElectricFence", "Admin",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FenceCode { get; set; }
    }

    //南钢嘉华修改电子围栏
    public class NGJH_EditElectricFenceModel 
    {
        public int ID { get; set; }
        /// <summary>
        /// 围栏名称
        /// </summary>
        [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckEditFenceNameExists", "ElectricFence", "Admin",
            AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FenceName { get; set; }
        /// <summary>
        /// 围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int FenceType { get; set; }
        /// <summary>
        /// 围栏类型信息
        /// </summary>
        [Display(Name = "FenceTypeInfo", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PleaseDrawElectricFenceFirst")]
        public string FenceTypeInfo { get; set; }

        /// <summary>
        /// 状态----三个值“在用客户”、“潜在客户”、“不在用客户”
        /// </summary>
        [Display(Name = "Status", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int CustomerStatus { get; set; }
    }
    #endregion

    #region 电子围栏绑定车辆
    #region 绑定车辆
    public class EFVehicleSearchModel
    {
        ///// <summary>
        ///// 车主
        ///// </summary>
        //[Display(Name = "车主名称")]
        //public string ClientName { get; set; }

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
        /// FenceID 记录当前电子围栏
        /// </summary>
        public int id { get; set; }
        public string FenceName { get; set; }
    }

    public class EFVehicleListModel
    {
        [Display(Name = "VehicleID", ResourceType = typeof(DisplayText))]
        public long VehicleID { get; set; }
        ///// <summary>
        ///// 车主
        ///// </summary>
        //[Display(Name = "车主姓名")]
        //public string ClientName { get; set; }

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
    }

    #endregion

    #region  解绑车辆
    public class ElectricFenceVehicleBindListModel
    {
        public long VehicleID { get; set; }
        ///// <summary>
        ///// 车主
        ///// </summary>
        //[Display(Name = "车主名称")]
        //public string ClientName { get; set; }

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
        public int FenceID { get; set; }
        /// <summary>
        /// 围栏名称
        /// </summary>
        [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
        public string FenceName { get; set; }
        /// <summary>
        /// 围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        public int FenceType { get; set; }
        /// <summary>
        /// 报警类型
        /// </summary>
        [Display(Name = "AlarmType", ResourceType = typeof(DisplayText))]
        public int AlarmType { get; set; }
        /// <summary>
        /// 启用状态
        /// </summary>
        [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
        public bool FenceState { get; set; }
        
        [Display(Name = "CreateTime", ResourceType = typeof(DisplayText))]
        public DateTime CreateTime { get; set; }
    }
    #endregion

    #endregion

    #region   围栏异常
    public class ElectricFenceExceptionModel
    {
        public long ExID { get; set; }
        /// <summary>
        /// 电子围栏编号
        /// </summary>
        public int FenceID { get; set; }

        [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
        public string FenceName { get; set; }

        //[Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        //public int FenceType { get; set; }
        //public string FenceTypeInfo { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        [Display(Name = "ExceptionTypeName", ResourceType = typeof(DisplayText))]
        public int ExceptionType { get; set; }

        public string TerminalCode { get; set; }
        public string ServerStartTime { get; set; }
        public string ServerEndTime { get; set; }

        /// <summary>
        /// 异常开始时信号时间
        /// </summary>
        [Display(Name = "SignalStartTime", ResourceType = typeof(DisplayText))]
        public string SignalStartTime { get; set; }

        /// <summary>
        /// 异常结束时信号时间
        /// </summary>
        [Display(Name = "SignalEndTime", ResourceType = typeof(DisplayText))]
        public string SignalEndTime { get; set; }

        public float StartLongitude { get; set; }
        public float StartLatitude { get; set; }

        /// <summary>
        /// 异常开始时位置
        /// </summary>
        [Display(Name = "StartAddress", ResourceType = typeof(DisplayText))]
        public string StartAddress { get; set; }

        public float EndLongitude { get; set; }
        public float EndLatitude { get; set; }

        /// <summary>
        /// 异常结束时位置
        /// </summary>
        [Display(Name = "EndAddress", ResourceType = typeof(DisplayText))]
        public string EndAddress { get; set; }
    }

    #endregion

    #region 地图显示电子围栏
    public class EFMarkerListModel
    {
        /// <summary>
        /// 电子围栏编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 围栏名称
        /// </summary>
        [Display(Name = "FenceName", ResourceType = typeof(DisplayText))]
        public string FenceName { get; set; }
        /// <summary>
        /// 围栏类型
        /// </summary>
        [Display(Name = "FenceType", ResourceType = typeof(DisplayText))]
        public int FenceType { get; set; }
        public string FenceTypeInfo { get; set; }
        /// <summary>
        /// 客户状态
        /// </summary>
        [Display(Name = "Status", ResourceType = typeof(DisplayText))]
        public int? CustomerStatus { get; set; }
    }
    #endregion
}

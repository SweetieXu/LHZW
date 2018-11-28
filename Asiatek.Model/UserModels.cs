using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Asiatek.CustomDataAnnotations;
using System.Web.Mvc;
using Asiatek.AjaxPager;
using Asiatek.Resource;

namespace Asiatek.Model
{
    /// <summary>
    /// 用户下拉列表模型
    /// 包含用户ID、用户名
    /// </summary>
    public class UserDDLModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
    }


    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class UserLoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "UserName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "UserPwd", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// 公司账号
        /// </summary>
        [Display(Name = "StrucAccount", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string StrucAccount { get; set; }

        /// <summary>
        /// 是否保存密码
        /// </summary>
        [Display(Name = "SavePwd", ResourceType = typeof(DisplayText))]
        public bool SavePassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Display(Name = "ValidateCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Remote("CheckValidateCode", "Account", HttpMethod = "POST",
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "ValidateCodeError")]
        public string ValidateCode { get; set; }


        /// <summary>
        /// 登录时选择的地区语言码（如：en-US、zh-CN)
        /// </summary>
        [Display(Name = "CultureName", ResourceType = typeof(DisplayText))]
        public string CultureName { get; set; }

        /// <summary>
        /// 语言选择下拉
        /// </summary>
        public IEnumerable<SelectListItem> LanguagesSelectList { get; set; }

        /// <summary>
        /// 返回URL
        /// </summary>
        public string ReturnUrl { get; set; }
    }


    /// <summary>
    /// 当用户登录后，Session["currentUser"]中存储的对象
    /// </summary>
    public class UserSessionModel
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 单位ID
        /// </summary>
        public int StrucID { get; set; }
        /// <summary>
        /// 车辆查看模式 (true 默认模式 能看到自己和自己子单位的所有车辆 false 自由模式 查看在车辆与单位分配功能处分配的车辆)
        /// </summary>
        public bool VehicleViewMode { get; set; }

        /// <summary>
        /// 菜单功能信息
        /// </summary>
        public List<FunctionsInfoModel> Functions { get; set; }
        /// <summary>
        /// 角色信息
        /// </summary>
        public RoleInfoModel RoleInfo { get; set; }

    }



    /// <summary>
    /// 用户列表信息
    /// </summary>
    public class UserListModel
    {
        public int ID { get; set; }
        [Display(Name = "UserName", ResourceType = typeof(DisplayText))]
        public string UserName { get; set; }
        [Display(Name = "NickName", ResourceType = typeof(DisplayText))]
        public string NickName { get; set; }
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public string SubordinateStrucName { get; set; }
        /// <summary>
        /// 车辆查看模式 (true 默认模式 能看到自己和自己子单位的所有车辆 false 自由模式 查看在车辆与单位分配功能处分配的车辆)
        /// </summary>
        [Display(Name = "VehicleViewMode", ResourceType = typeof(DisplayText))]
        public bool VehicleViewMode { get; set; }

    }

    /// <summary>
    /// 用户查询
    /// </summary>
    public class UserSearchModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "UserName", ResourceType = typeof(DisplayText))]
        public string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "NickName", ResourceType = typeof(DisplayText))]
        public string NickName { get; set; }

        /// <summary>
        /// 用户所属单位下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> SubordinateStrucSelectList { get; set; }

        /// <summary>
        /// 用户所属单位ID
        /// </summary>
        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        public int StrucID { get; set; }
        /// <summary>
        /// 车辆查看模式
        /// </summary>
        [Display(Name = "VehicleViewMode", ResourceType = typeof(DisplayText))]
        public int VehicleViewMode { get; set; }
    }


    /// <summary>
    /// 用户新增
    /// </summary>
    public class UserAddModel
    {
        [Display(Name = "UserName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddUserNameExists", "User", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string UserName { get; set; }

        [Display(Name = "NickName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string NickName { get; set; }

        [Display(Name = "ContactNumber1", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string ContactNumber1 { get; set; }

        [Display(Name = "ContactNumber2", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "ChinesePhoneError")]
        public string ContactNumber2 { get; set; }

        [Display(Name = "ContactAddress", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string ContactAddress { get; set; }

        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

        [Display(Name = "UserRole", ResourceType = typeof(DisplayText))]
        public int RoleID { get; set; }


        public IEnumerable<SelectListItem> RoleSelectList { get; set; }

        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
 ErrorMessageResourceName = "MustSelect")]
        public int? StrucID { get; set; }

        //public IEnumerable<SelectListItem> StrucSelectList { get; set; }
        /// <summary>
        /// 车辆查看模式 (true 默认模式 能看到自己和自己子单位的所有车辆 false 自由模式 查看在车辆与单位分配功能处分配的车辆)
        /// </summary>
        [Display(Name = "VehicleViewMode", ResourceType = typeof(DisplayText))]
        public bool VehicleViewMode { get; set; }

    }



    /// <summary>
    /// 用户编辑
    /// </summary>
    public class UserEditModel
    {

        public int ID { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(DisplayText))]
        public string UserName { get; set; }

        [Display(Name = "NickName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string NickName { get; set; }

        [Display(Name = "ContactNumber1", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string ContactNumber1 { get; set; }

        [Display(Name = "ContactNumber2", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "ChinesePhoneError")]
        public string ContactNumber2 { get; set; }

        [Display(Name = "ContactAddress", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string ContactAddress { get; set; }

        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

        [Display(Name = "UserRole", ResourceType = typeof(DisplayText))]
        public int RoleID { get; set; }


        public IEnumerable<SelectListItem> RoleSelectList { get; set; }

        [Display(Name = "SubordinateStrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
 ErrorMessageResourceName = "MustSelect")]
        public int? StrucID { get; set; }

        public string StrucName { get; set; }

        //public IEnumerable<SelectListItem> StrucSelectList { get; set; }
        /// <summary>
        /// 车辆查看模式 (true 默认模式 能看到自己和自己子单位的所有车辆 false 自由模式 查看在车辆与单位分配功能处分配的车辆)
        /// </summary>
        [Display(Name = "VehicleViewMode", ResourceType = typeof(DisplayText))]
        public bool VehicleViewMode { get; set; }

    }



    /// <summary>
    /// 修改密码
    /// </summary>
    public class UserModifyPasswordModel
    {

        public int ID { get; set; }

        /// <summary>
        /// 原密码
        /// </summary>
        [Display(Name = "OriginalPassword", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MustInput")]
        [Remote("CheckOriginalPassword", "User", "PersonalSetting", HttpMethod = "POST",
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "OriginalPasswordError")]
        public string OriginalPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Display(Name = "NewPassword", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MustInput")]
        [UnMatched("OriginalPassword",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "NewPasswordError")]
        public string NewPassword { get; set; }
        /// <summary>
        /// 确认新密码
        /// </summary>
        [Display(Name = "ConfirmPassword", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MustInput")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ConfirmPasswordError")]
        public string ConfirmPassword { get; set; }
    }
}
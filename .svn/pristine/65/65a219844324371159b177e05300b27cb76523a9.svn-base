using Asiatek.AjaxPager;
using Asiatek.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Asiatek.Resource;

namespace Asiatek.Model
{
    public class RoleInfoModel
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public RoleLevelEnum RoleLevel { get; set; }
    }


    public class RoleListModel
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        [Display(Name = "RoleID", ResourceType = typeof(DisplayText))]
        public int ID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Display(Name = "RoleName", ResourceType = typeof(DisplayText))]
        public string RoleName { get; set; }

        /// <summary>
        /// 是否是默认的角色
        /// 默认角色固定为3个：超管、管理员、普通用户
        /// </summary>
        [Display(Name = "IsDefault", ResourceType = typeof(DisplayText))]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        public string Description { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        [Display(Name = "RoleLevel", ResourceType = typeof(DisplayText))]
        public RoleLevelEnum RoleLevel { get; set; }
    }


    public class RoleAddModel
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Display(Name = "RoleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(200, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddRoleNameExists", "Role", "Admin", HttpMethod = "POST",
       ErrorMessageResourceType = typeof(DataAnnotations),
       ErrorMessageResourceName = "FieldExists")]
        public string RoleName { get; set; }


        /// <summary>
        /// 角色描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelectAtLeastOneFunction")]
        public List<int> FunctionIDs { get; set; }


        public List<FunctionTreeNodeModel> FunctionTreeNodes { get; set; }
    }


    public class RoleEditModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Display(Name = "RoleName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(200, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditRoleNameExists", "Role", "Admin",
            AdditionalFields = "ID",
            HttpMethod = "POST",
       ErrorMessageResourceType = typeof(DataAnnotations),
       ErrorMessageResourceName = "FieldExists")]
        public string RoleName { get; set; }


        /// <summary>
        /// 角色描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelectAtLeastOneFunction")]
        public List<int> FunctionIDs { get; set; }


        public List<FunctionTreeNodeModel> FunctionTreeNodes { get; set; }
    }


    public class RoleSettingModel : ContainsPagedDatas<RoleListModel>
    {
        public string RoleName { get; set; }
    }


    /// <summary>
    /// 角色等级枚举
    /// </summary>
    public enum RoleLevelEnum
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperAdmin = 0,
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 1,
        /// <summary>
        /// 普通用户
        /// </summary>
        User = 2
    }

    /// <summary>
    /// 角色下拉数据模型
    /// </summary>
    public class RoleDDLModel
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
    }
}

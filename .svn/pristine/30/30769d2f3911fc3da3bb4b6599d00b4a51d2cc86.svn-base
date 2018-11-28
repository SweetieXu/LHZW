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
    #region 搜索类实体
    public class SimCodeSearchModels
    {
        /// <summary>
        /// Sim卡号
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        public string SimCode { get; set; }

        /// <summary>
        /// 通信方式
        /// </summary>
        [Display(Name = "CommMode", ResourceType = typeof(DisplayText))]
        public int CommMode { get; set; }

        public IEnumerable<SelectListItem> CommModeSelectList { get; set; }

        /// <summary>
        /// 归属单位
        /// </summary>
        [Display(Name = "OwnerStrucCode", ResourceType = typeof(DisplayText))]
        public int OwnerStrucID { get; set; }

        public IEnumerable<SelectListItem> OwnerStrucCodeSelectList { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        [Display(Name = "UseStrucCode", ResourceType = typeof(DisplayText))]
        public int UseStrucID { get; set; }

        public IEnumerable<SelectListItem> UseStrucCodeSelectList { get; set; }

        /// <summary>
        /// 服务商
        /// </summary>
        [Display(Name = "ServiceProvider", ResourceType = typeof(DisplayText))]
        public int ServiceProviderID { get; set; }

        public IEnumerable<SelectListItem> ServiceProviderSelectList { get; set; }
    }
    #endregion

    #region 列表实体
    public class SimCodeListModels
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        public int ID { get; set; }

        /// <summary>
        /// Sim卡号
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        public string SimCode { get; set; }

        /// <summary>
        /// 通信方式
        /// </summary>
        [Display(Name = "CommMode", ResourceType = typeof(DisplayText))]
        public int CommMode { get; set; }

        public string CName { get; set; }

        /// <summary>
        /// 归属单位
        /// </summary>
        [Display(Name = "OwnerStrucCode", ResourceType = typeof(DisplayText))]
        public string OwnerStrucName { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        [Display(Name = "UseStrucCode", ResourceType = typeof(DisplayText))]
        public string UseStrucName { get; set; }

        /// <summary>
        /// 服务商
        /// </summary>
        [Display(Name = "ServiceProvider", ResourceType = typeof(DisplayText))]
        public string ServiceProvider { get; set; }

        /// <summary>
        /// 购买日期
        /// </summary>
        [Display(Name = "PurchaseDate", ResourceType = typeof(DisplayText))]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// 开通日期
        /// </summary>
        [Display(Name = "OpeningDate", ResourceType = typeof(DisplayText))]
        public DateTime OpeningDate { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
        [Display(Name = "ExpiryDate", ResourceType = typeof(DisplayText))]
        public DateTime ExpiryDate { get; set; }

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
    }

    #endregion
 
    #region   新增
    public class AddSimCodeModels
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int ID { get; set; }

        /// <summary>
        /// Sim卡号
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^([0-9]{11}|[0-9]{13})$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "SIMCodeError")]
        [AsiatekRemote("CheckAddSIMCodeExists", "SimCode", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
       ErrorMessageResourceType = typeof(DataAnnotations),
       ErrorMessageResourceName = "FieldExists")]
        public string SimCode { get; set; }

        /// <summary>
        /// 通信方式
        /// </summary>
        [Display(Name = "CommMode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int CommMode { get; set; }

        public IEnumerable<SelectListItem> CommModeSelectList { get; set; }



        /// <summary>
        /// 归属单位
        /// </summary>
        [Display(Name = "OwnerStrucCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? OwnerStrucID { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        [Display(Name = "UseStrucCode", ResourceType = typeof(DisplayText))]
       [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? UseStrucID { get; set; }

        /// <summary>
        /// 服务商
        /// </summary>
        [Display(Name = "ServiceProvider", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int ServiceProviderID { get; set; }
        
        public IEnumerable<SelectListItem> ServiceProviderSelectList { get; set; }


        /// <summary>
        /// 购买日期
        /// </summary>
        [Display(Name = "PurchaseDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string PurchaseDate { get; set; }

        /// <summary>
        /// 开通日期
        /// </summary>
        [Display(Name = "OpeningDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string OpeningDate { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
        [Display(Name = "ExpiryDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string ExpiryDate { get; set; }

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
    }
    #endregion

    #region   编辑
    public class EditSimCodeModels
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public int ID { get; set; }

        /// <summary>
        /// Sim卡号
        /// </summary>
        [Display(Name = "SIMCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^([0-9]{11}|[0-9]{13})$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "SIMCodeError")]
        [AsiatekRemote("CheckEditSIMCodeExists", "SimCode", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
       ErrorMessageResourceType = typeof(DataAnnotations),
       ErrorMessageResourceName = "FieldExists")]
        public string SimCode { get; set; }

        /// <summary>
        /// 通信方式
        /// </summary>
        [Display(Name = "CommMode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int CommMode { get; set; }

        public IEnumerable<SelectListItem> CommModeSelectList { get; set; }

        /// <summary>
        /// 归属单位
        /// </summary>
        [Display(Name = "OwnerStrucCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? OwnerStrucID { get; set; }

        public string OwnerStrucName { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        [Display(Name = "UseStrucCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int? UseStrucID { get; set; }

        public string UseStrucName { get; set; }

        /// <summary>
        /// 服务商
        /// </summary>
        [Display(Name = "ServiceProvider", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
        public int ServiceProviderID { get; set; }

        public IEnumerable<SelectListItem> ServiceProviderSelectList { get; set; }


        /// <summary>
        /// 购买日期
        /// </summary>
        [Display(Name = "PurchaseDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// 开通日期
        /// </summary>
        [Display(Name = "OpeningDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public DateTime OpeningDate { get; set; }

        /// <summary>
        /// 过期日期
        /// </summary>
        [Display(Name = "ExpiryDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public DateTime ExpiryDate { get; set; }

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
    }
    #endregion

    #region 扩展类
    /// <summary>
    /// SIM卡下拉列表模型
    /// </summary>
    public class SimCodeDDLModel
    {
        /// <summary>
        /// SIM卡ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// SIM卡号
        /// </summary>
        public string SimCode { get; set; }
    }
    #endregion
   
}

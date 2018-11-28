using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Asiatek.CustomDataAnnotations;
using System.Web.Mvc;
using Asiatek.AjaxPager;
using Asiatek.Resource;

namespace Asiatek.Model
{

    /// <summary>
    /// 查询区域模型
    /// </summary>
    public class AreaSearchModel
    {
        //TODO:缺少国际化
        [Required]
        public string AreaName { get; set; }
    }



    /// <summary>
    /// 区域设定模型
    /// </summary>
    public class AreaSettingModel : ContainsPagedDatas<AreaModel>
    {
        public string AreaName { get; set; }
    }


    /// <summary>
    /// 区域模型
    /// </summary>
    public class AreaModel
    {
        /// <summary>
        /// 区域编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 区域名
        /// </summary>
        [Display(Name = "AreaName", ResourceType = typeof(DisplayText))]
        public string AreaName { get; set; }
        /// <summary>
        /// 区域描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        public string Description { get; set; }

    }

    /// <summary>
    /// 区域编辑模型
    /// </summary>
    public class AreaEditModel
    {
        /// <summary>
        /// 区域编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 区域名
        /// </summary>
        [Display(Name = "AreaName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditAreaNameExists", "Area", "Admin", AdditionalFields = "ID", HttpMethod = "POST", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "FieldExists")]
        [RegularExpression(@"^[A-Z][a-zA-Z0-9_]*", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "AreaControllerActionName")]
        public string AreaName { get; set; }
        /// <summary>
        /// 区域描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }

    }




    /// <summary>
    /// 区域新增模型
    /// </summary>
    public class AreaAddModel
    {
        /// <summary>
        /// 区域名
        /// </summary>
        [Display(Name = "AreaName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddAreaNameExists", "Area", "Admin", HttpMethod = "POST", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "FieldExists")]
        [RegularExpression(@"^[A-Z][a-zA-Z0-9_]*", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "AreaControllerActionName")]
        public string AreaName { get; set; }
        /// <summary>
        /// 区域描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }
    }

    /// <summary>
    /// 区域下拉列表模型
    /// </summary>
    public class AreaDDLModel
    {
        /// <summary>
        /// 区域编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
    }




}
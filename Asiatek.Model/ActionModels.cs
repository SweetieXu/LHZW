using Asiatek.AjaxPager;
using Asiatek.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Asiatek.Resource;

namespace Asiatek.Model
{
    /// <summary>
    /// 动作信息集 
    /// </summary>
    public class ActionListModel
    {
        /// <summary>
        /// 动作编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        [Display(Name = "ActionName", ResourceType = typeof(DisplayText))]
        public string ActionName { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        [Display(Name = "SubordinateController", ResourceType = typeof(DisplayText))]
        public string ControllerName { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public string AreaName { get; set; }
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        public string Description { get; set; }

    }
    ///// <summary>
    ///// 动作设定
    ///// </summary>
    //public class ActionSettingModel
    //{
    //    /// <summary>
    //    /// 动作分页数据
    //    /// </summary>
    //    public AsiatekPagedList<ActionListModel> PagedActions { get; set; }
    //    /// <summary>
    //    /// 区域下拉
    //    /// </summary>
    //    [Display(Name = "AreaName", ResourceType = typeof(DataAnnotations))]
    //    public IEnumerable<SelectListItem> AreasSelectList { get; set; }
    //    /// <summary>
    //    /// 控制器下拉
    //    /// </summary>
    //    [Display(Name = "ControllerName", ResourceType = typeof(DataAnnotations))]
    //    public IEnumerable<SelectListItem> ControllersSelectList { get; set; }
    //}





    /// <summary>
    /// 动作设定
    /// </summary>
    public class ActionSettingModel : ContainsPagedDatas<ActionListModel>
    {
        /// <summary>
        /// 区域下拉
        /// </summary>
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }

        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public int AreaID { get; set; }


        /// <summary>
        /// 控制器下拉
        /// </summary>
        public IEnumerable<SelectListItem> ControllersSelectList { get; set; }

        [Display(Name = "SubordinateController", ResourceType = typeof(DisplayText))]
        public int ControllerID { get; set; }

        public string ActionName { get; set; }

    }




    public class ActionAddModel
    {
        /// <summary>
        /// 动作名
        /// </summary>
        [Display(Name = "ActionName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddActionNameExists", "Action", "Admin",
            AdditionalFields = "ControllerID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ActionNameExists")]
        [RegularExpression(@"^[A-Z][a-zA-Z0-9_]*", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "AreaControllerActionName")]
        public string ActionName { get; set; }
        /// <summary>
        /// 动作描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }

        /// <summary>
        /// 所属区域编号
        /// </summary>
        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public int AreaID { get; set; }

        /// <summary>
        /// 区域信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }



        /// <summary>
        /// 所属控制器编号
        /// </summary>
        [Display(Name = "SubordinateController", ResourceType = typeof(DisplayText))]
        public int ControllerID { get; set; }

        /// <summary>
        /// 控制器信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> ControllersSelectList { get; set; }

    }


    public class ActionEditModel
    {
        /// <summary>
        /// 动作编号
        /// </summary>
        public int ID { get; set; }


        /// <summary>
        /// 动作名
        /// </summary>
        [Display(Name = "ActionName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditActionNameExists", "Action", "Admin",
            AdditionalFields = "ControllerID,ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ActionNameExists")]
        [RegularExpression(@"^[A-Z][a-zA-Z0-9_]*", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "AreaControllerActionName")]
        public string ActionName { get; set; }
        /// <summary>
        /// 动作描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }


        /// <summary>
        /// 区域编号
        /// </summary>
        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public int AreaID { get; set; }




        /// <summary>
        /// 区域信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }


        /// <summary>
        /// 控制器编号
        /// </summary>
        [Display(Name = "SubordinateController", ResourceType = typeof(DisplayText))]
        public int ControllerID { get; set; }

        /// <summary>
        /// 控制器信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> ControllersSelectList { get; set; }

    }


    /// <summary>
    /// 动作下拉列表模型
    /// </summary>
    public class ActionDDLModel
    {
        /// <summary>
        /// 动作编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName { get; set; }
    }
}
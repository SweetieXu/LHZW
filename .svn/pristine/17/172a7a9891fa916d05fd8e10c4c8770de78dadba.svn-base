using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using Asiatek.CustomDataAnnotations;
using System.Web.Mvc;
using Asiatek.AjaxPager;
using Asiatek.Resource;

namespace Asiatek.Model
{


    public class ControllerListModel
    {

        public int ID { get; set; }
        [Display(Name = "ControllerName", ResourceType = typeof(DisplayText))]
        public string ControllerName { get; set; }
        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public string AreaName { get; set; }
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        public string Description { get; set; }


    }

    /// <summary>
    /// 控制器下拉列表模型
    /// </summary>
    public class ControllerDDLModel
    {
        /// <summary>
        /// 控制器编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

    }



    /// <summary>
    /// 查询控制器
    /// </summary>
    public class ControllerSearchModel
    {
        /// <summary>
        /// 区域下拉列表
        /// </summary>
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }

        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public int AreaID { get; set; }

        public string ControllerName { get; set; }
    }


    /// <summary>
    /// 控制器设定 
    /// </summary>
    public class ControllerSettingModel : ContainsPagedDatas<ControllerListModel>
    {
        /// <summary>
        /// 区域下拉列表
        /// </summary>
        [Display(Name = "AreaName", ResourceType = typeof(DisplayText))]
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }

        public int AreaID { get; set; }

        public string ControllerName { get; set; }
    }




    ///// <summary>
    ///// 控制器设定 
    ///// </summary>
    //public class ControllerSettingModel
    //{
    //    /// <summary>
    //    /// 控制器分页数据
    //    /// </summary>
    //    public AsiatekPagedList<ControllerListModel> PagedControllers { get; set; }
    //    /// <summary>
    //    /// 区域下拉列表
    //    /// </summary>
    //    [Display(Name = "AreaName", ResourceType = typeof(DataAnnotations))]
    //    public IEnumerable<SelectListItem> AreasSelectList { get; set; }

    //}

    /// <summary>
    /// 控制器编辑
    /// </summary>
    public class ControllerEditModel
    {
        public int ID { get; set; }

        [Display(Name = "ControllerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(30, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditControllerNameExists", "Ctr", "Admin", AdditionalFields = "AreaID,ID", HttpMethod = "POST", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ControllerNameExists")]
        [RegularExpression(@"^[A-Z][a-zA-Z0-9_]*", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "AreaControllerActionName")]
        public string ControllerName { get; set; }


        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }


        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public int AreaID { get; set; }

        /// <summary>
        /// 区域信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }


    }


    /// <summary>
    /// 控制器新增
    /// </summary>
    public class ControllerAddModel
    {

        [Display(Name = "ControllerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(30, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddControllerNameExists", "Ctr", "Admin", AdditionalFields = "AreaID", HttpMethod = "POST", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ControllerNameExists")]
        [RegularExpression(@"^[A-Z][a-zA-Z0-9_]*", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "AreaControllerActionName")]
        public string ControllerName { get; set; }


        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }


        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public int AreaID { get; set; }

        /// <summary>
        /// 区域信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }
    }





}
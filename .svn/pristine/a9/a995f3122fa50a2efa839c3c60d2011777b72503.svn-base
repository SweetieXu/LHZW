using Asiatek.AjaxPager;
using Asiatek.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.Resource;

namespace Asiatek.Model
{
    /// <summary>
    /// 描述用户菜单功能信息的模型
    /// 用户存储在UserSessionModel中，决定了用户的权限
    /// </summary>
    public class FunctionsInfoModel
    {
        /// <summary>
        /// 功能编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 父节点编号
        /// </summary>
        public int? ParentID { get; set; }

        /// <summary>
        /// 功能排列索引
        /// </summary>
        public int OrderIndex { get; set; }

    }


    public class FunctionListModel
    {
        /// <summary>
        /// 功能编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [Display(Name = "FunctionName", ResourceType = typeof(DisplayText))]
        public string FunctionName { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public string AreaName { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        [Display(Name = "SubordinateController", ResourceType = typeof(DisplayText))]
        public string ControllerName { get; set; }

        /// <summary>
        /// 动作名称
        /// </summary>
        [Display(Name = "AssociatedAction", ResourceType = typeof(DisplayText))]
        public string ActionName { get; set; }

        /// <summary>
        /// 上级功能名称
        /// </summary>
        [Display(Name = "ParentFunction", ResourceType = typeof(DisplayText))]
        public string ParentFunctionName { get; set; }

        /// <summary>
        /// 是否后台功能
        /// </summary>
        [Display(Name = "IsBackground", ResourceType = typeof(DisplayText))]
        public bool IsBackground { get; set; }

        /// <summary>
        /// 用于显示层次效果的LevelID
        /// </summary>
        public string LevelID { get; set; }
    }



    public class FunctionDDLModel
    {
        public int ID { get; set; }
        public string FunctionName { get; set; }
    }




    ///// <summary>
    ///// 动作设定
    ///// </summary>
    //public class FunctionSettingModel
    //{
    //    /// <summary>
    //    /// 功能分页数据
    //    /// </summary>
    //    public AsiatekPagedList<FunctionListModel> PagedFunctions { get; set; }
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

    //    /// <summary>
    //    /// 上级功能信息下拉
    //    /// </summary>
    //    [Display(Name = "ParentFunctionName", ResourceType = typeof(DataAnnotations))]
    //    public IEnumerable<SelectListItem> ParentFunctionsSelectList { get; set; }
    //}



    /// <summary>
    /// 动作设定
    /// </summary>
    public class FunctionSettingModel : ContainsPagedDatas<FunctionListModel>
    {
        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public int AreaID { get; set; }
        /// <summary>
        /// 区域下拉
        /// </summary>
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }

        [Display(Name = "SubordinateController", ResourceType = typeof(DisplayText))]
        public int ControllerID { get; set; }
        /// <summary>
        /// 控制器下拉
        /// </summary>
        public IEnumerable<SelectListItem> ControllersSelectList { get; set; }

        [Display(Name = "ParentFunction", ResourceType = typeof(DisplayText))]
        public int ParentFunctionID { get; set; }
        /// <summary>
        /// 上级功能信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> ParentFunctionsSelectList { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        [Display(Name = "FunctionName", ResourceType = typeof(DisplayText))]
        public string FunctionName { get; set; }

        /// <summary>
        /// 菜单（不与具体Action关联的功能仅仅只是个菜单）
        /// </summary>
        [Display(Name = "IsMenu", ResourceType = typeof(DisplayText))]
        public bool IsMenu { get; set; }


        /// <summary>
        /// 是否是最顶级功能
        /// 最顶级功能无父节点
        /// </summary>
        [Display(Name = "IsTopFunction", ResourceType = typeof(DisplayText))]
        public bool IsTopFunction { get; set; }
        /// <summary>
        /// 是否是App功能
        /// </summary>
        [Display(Name = "IsAppFeatures", ResourceType = typeof(DisplayText))]
        public bool IsAppFeatures { get; set; }

    }



    public class FunctionAddModel
    {
        /// <summary>
        /// 动作名
        /// </summary>
        [Display(Name = "FunctionName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddFunctionNameExists", "Function", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FunctionName { get; set; }


        /// <summary>
        /// 动作描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }



        /// <summary>
        /// 是否后台功能
        /// </summary>
        [Display(Name = "IsBackground", ResourceType = typeof(DisplayText))]
        public bool IsBackground { get; set; }

        /// <summary>
        /// 是否是最顶级功能
        /// 最顶级功能无父节点
        /// </summary>
        [Display(Name = "IsTopFunction", ResourceType = typeof(DisplayText))]
        public bool IsTopFunction { get; set; }



        /// <summary>
        /// 所属区域编号
        /// </summary>
        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public int? AreaID { get; set; }

        /// <summary>
        /// 区域信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }



        /// <summary>
        /// 所属控制器编号
        /// </summary>
        [Display(Name = "SubordinateController", ResourceType = typeof(DisplayText))]
        public int? ControllerID { get; set; }

        /// <summary>
        /// 控制器信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> ControllersSelectList { get; set; }



        /// <summary>
        /// 所属动作编号
        /// </summary>
        [AsiatekRemote("CheckActionIDWhenAddFunction", "Action", "Admin"
    , HttpMethod = "POST"
    , ErrorMessageResourceType = typeof(DataAnnotations)
    , ErrorMessageResourceName = "ActionIDHadBeenLinkedWithFunction", AdditionalFields = "FunctionIsMenu")]
        [RequiredIfNotTrue("FunctionIsMenu", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelectAction")]
        [Display(Name = "AssociatedAction", ResourceType = typeof(DisplayText))]
        public int? ActionID { get; set; }


        /// <summary>
        /// 动作信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> ActionsSelectList { get; set; }

        /// <summary>
        /// 上级功能编号
        /// </summary>
        [Display(Name = "ParentFunction", ResourceType = typeof(DisplayText))]
        public int? ParentID { get; set; }

        /// <summary>
        /// 上级功能信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> ParentFunctionsSelectList { get; set; }


        /// <summary>
        /// 排序索引
        /// </summary>
        [Display(Name = "OrderIndex", ResourceType = typeof(DisplayText))]
        [Range(Int32.MinValue, Int32.MaxValue,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "RangeError")]
        public int OrderIndex { get; set; }

        /// <summary>
        /// 是否是菜单（当功能是菜单时，不与任何Action关联）
        /// </summary>
        [Display(Name = "FunctionIsMenu", ResourceType = typeof(DisplayText))]
        public bool FunctionIsMenu { get; set; }
        /// <summary>
        /// 是否App功能
        /// </summary>
        [Display(Name = "IsAppFeatures", ResourceType = typeof(DisplayText))]
        public bool IsAppFeatures { get; set; }
        /// <summary>
        /// 是否是App首页模块
        /// </summary>
        [Display(Name = "IsAppHomeModule", ResourceType = typeof(DisplayText))]
        public bool IsAppHomeModule { get; set; }
        /// <summary>
        /// 是否是App快捷菜单（配合实时监控中的快捷菜单使用）
        /// </summary>
        [Display(Name = "IsAppShortcutMenu", ResourceType = typeof(DisplayText))]
        public bool IsAppShortcutMenu { get; set; }
        /// <summary>
        /// 功能编码
        /// </summary>
        [Display(Name = "FeaturesCode", ResourceType = typeof(DisplayText))]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddFeaturesCodeExists", "Function", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FeaturesCode { get; set; }

    }









    public class FunctionEditModel
    {
        /// <summary>
        /// 功能编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 动作名
        /// </summary>
        [Display(Name = "FunctionName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditFunctionNameExists", "Function", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FunctionName { get; set; }

        /// <summary>
        /// 动作描述
        /// </summary>
        [Display(Name = "Description", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }


        /// <summary>
        /// 是否是最顶级功能
        /// 最顶级功能无父节点
        /// </summary>
        [Display(Name = "IsTopFunction", ResourceType = typeof(DisplayText))]
        public bool IsTopFunction { get; set; }



        /// <summary>
        /// 是否后台功能
        /// </summary>
        [Display(Name = "IsBackground", ResourceType = typeof(DisplayText))]
        public bool IsBackground { get; set; }

        /// <summary>
        /// 所属区域编号
        /// </summary>
        [Display(Name = "SubordinateArea", ResourceType = typeof(DisplayText))]
        public int? AreaID { get; set; }

        /// <summary>
        /// 区域信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> AreasSelectList { get; set; }


        /// <summary>
        /// 所属控制器编号
        /// </summary>
        [Display(Name = "SubordinateController", ResourceType = typeof(DisplayText))]
        public int? ControllerID { get; set; }

        /// <summary>
        /// 控制器信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> ControllersSelectList { get; set; }


        /// <summary>
        /// 所属动作编号
        /// </summary>
        [AsiatekRemote("CheckActionIDWhenEditFunction", "Action", "Admin"
    , HttpMethod = "POST"
    , AdditionalFields = "ID,FunctionIsMenu"
    , ErrorMessageResourceType = typeof(DataAnnotations)
    , ErrorMessageResourceName = "ActionIDHadBeenLinkedWithFunction")]
        [Display(Name = "AssociatedAction", ResourceType = typeof(DisplayText))]
        [RequiredIfNotTrue("FunctionIsMenu", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelectAction")]
        public int? ActionID { get; set; }

        /// <summary>
        /// 动作信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> ActionsSelectList { get; set; }


        /// <summary>
        /// 上级功能编号
        /// </summary>
        [Display(Name = "ParentFunction", ResourceType = typeof(DisplayText))]
        public int? ParentID { get; set; }

        /// <summary>
        /// 上级功能信息下拉
        /// </summary>
        public IEnumerable<SelectListItem> ParentFunctionsSelectList { get; set; }

        /// <summary>
        /// 排序索引
        /// </summary>
        [Display(Name = "OrderIndex", ResourceType = typeof(DisplayText))]
        [Range(Int32.MinValue, Int32.MaxValue,
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "RangeError")]
        public int OrderIndex { get; set; }


        /// <summary>
        /// 是否是菜单（当功能是菜单时，不与任何Action关联）
        /// </summary>
        [Display(Name = "FunctionIsMenu", ResourceType = typeof(DisplayText))]
        public bool FunctionIsMenu { get; set; }
        /// <summary>
        /// 是否App功能
        /// </summary>
        [Display(Name = "IsAppFeatures", ResourceType = typeof(DisplayText))]
        public bool IsAppFeatures { get; set; }
        /// <summary>
        /// 功能编码
        /// </summary>
        [Display(Name = "FeaturesCode", ResourceType = typeof(DisplayText))]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditFeaturesCodeExists", "Function", "Admin", HttpMethod = "POST", AdditionalFields = "ID",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string FeaturesCode { get; set; }
        /// <summary>
        /// 是否是App首页模块
        /// </summary>
        [Display(Name = "IsAppHomeModule", ResourceType = typeof(DisplayText))]
        public bool IsAppHomeModule { get; set; }
        /// <summary>
        /// 是否是App快捷菜单（配合实时监控中的快捷菜单使用）
        /// </summary>
        [Display(Name = "IsAppShortcutMenu", ResourceType = typeof(DisplayText))]
        public bool IsAppShortcutMenu { get; set; }

    }


    public class FunctionTreeNodeModel
    {
        /// <summary>
        /// 功能编号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string FunctionName { get; set; }


        /// <summary>
        /// 父节点编号
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; }
    }

}
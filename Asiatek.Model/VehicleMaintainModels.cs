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
    public class VehicleMaintainListModels
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "PlateCode", ResourceType = typeof(DisplayText))]
        public int Code { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        [Display(Name = "PlateColor", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }
    }

    public class VehicleMaintainSearchModels
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "PlateCode", ResourceType = typeof(DisplayText))]
        public string PlateCode { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [Display(Name = "PlateColor", ResourceType = typeof(DisplayText))]
        public string PlateName { get; set; }
    }

    #region   新增
    public class VehicleMaintainAddModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "PlateCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^\d{1,3}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustBeThreeIntegers")]
        [AsiatekRemote("CheckAddPlateCodeExists", "VehicleMaintain", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public int PlateCode { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [Display(Name = "PlateColor", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[\u4e00-\u9fa5]{1,2}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "PlateColor")]
        [AsiatekRemote("CheckAddPlateColorsExists", "VehicleMaintain", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string PlateColor { get; set; }
    }
    #endregion
}

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
    public class VehicleTypeDDLModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
    }



    public class VehicleTypeListModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "VehicleTCode", ResourceType = typeof(DisplayText))]
        public int Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "VehicleTName", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }

        /// <summary>
        /// 上级编号
        /// </summary>
        [Display(Name = "HigheNumber", ResourceType = typeof(DisplayText))]
        public int ParentCode { get; set; }
    }

    public class VehicleTypeSearchModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "VehicleTCode", ResourceType = typeof(DisplayText))]
        public string VehicleTCode { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "VehicleTName", ResourceType = typeof(DisplayText))]
        public string VehicleTName { get; set; }
    }



    #region   新增
    public class VehicleTypeAddModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "VehicleTCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^\d{1,3}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustBeThreeIntegers")]
        [AsiatekRemote("CheckAddTypeCodeExists", "VehicleType", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public int Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "VehicleTName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[\u4e00-\u9fa5]{1,10}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "VehicleTName")]
        [AsiatekRemote("CheckAddTypeNameExists", "VehicleType", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string Name { get; set; }

        /// <summary>
        /// 上级编号
        /// </summary>
        [Display(Name = "HigheNumber", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^\d{1,3}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustBeThreeIntegers")]
        public int? HigheNumber { get; set; }
    }
    #endregion


    #region   编辑
    public class VehicleTypeEditModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "VehicleTCode", ResourceType = typeof(DisplayText))]
        public int Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "VehicleTName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[\u4e00-\u9fa5]{1,10}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "VehicleTName")]
        [AsiatekRemote("CheckEditTypeNameExists", "VehicleType", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string Name { get; set; }

        /// <summary>
        /// 上级编号
        /// </summary>
        [Display(Name = "HigheNumber", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^\d{1,3}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustBeThreeIntegers")]
        public string ParentCode { get; set; }
    }
    #endregion

    #region 其他
    /// <summary>
    /// 联想查询
    /// </summary>
    public class VehiclesTypeCode
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 上级编号
        /// </summary>
        public int ParentCode { get; set; }
    }
    #endregion
}

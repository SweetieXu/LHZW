using Asiatek.CustomDataAnnotations;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    /// <summary>
    /// 终端厂商信息
    /// </summary>
    public class TerminalManufacturerListModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 车载终端厂商名称
        /// </summary>
        [Display(Name = "ManufacturerName", ResourceType = typeof(DisplayText))]
        public string ManufacturerName { get; set; }
        /// <summary>
        /// 车载终端厂商编码（5位）
        /// </summary>
        [Display(Name = "ManufacturerCode", ResourceType = typeof(DisplayText))]
        public string ManufacturerCode { get; set; }
        /// <summary>
        /// 车载终端厂商所在地行政区划码（6位）
        /// </summary>
        [Display(Name = "Nationality", ResourceType = typeof(DisplayText))]
        public string Nationality { get; set; }

    }

    /// <summary>
    /// 终端厂商搜索
    /// </summary>
    public class TerminalManufacturerSearchModel
    {
        [Display(Name = "ManufacturerName", ResourceType = typeof(DisplayText))]
        public string ManufacturerName { get; set; }
    }

    public class TerminalManufacturerAddModel
    {
        /// <summary>
        /// 车载终端厂商名称
        /// </summary>
        [Display(Name = "ManufacturerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(200, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddTerminalManufacturerNameExists", "TerminalManufacturer", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string ManufacturerName { get; set; }
        /// <summary>
        /// 车载终端厂商编码（5位）
        /// </summary>
        [Display(Name = "ManufacturerCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ManufacturerCodeError")]
        [AsiatekRemote("CheckAddTerminalManufacturerCodeExists", "TerminalManufacturer", "Admin", HttpMethod = "POST",
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "FieldExists")]
        public string ManufacturerCode { get; set; }
        /// <summary>
        /// 车载终端厂商所在地行政区划码（6位）
        /// </summary>
        [Display(Name = "Nationality", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression("^[0-9]{6}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "NationalityCodeError")]
        public string Nationality { get; set; }
    }


    public class TerminalManufacturerEditModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 车载终端厂商名称
        /// </summary>
        [Display(Name = "ManufacturerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(200, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditTerminalManufacturerNameExists", "TerminalManufacturer", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string ManufacturerName { get; set; }
        /// <summary>
        /// 车载终端厂商编码（5位）
        /// </summary>
        [Display(Name = "ManufacturerCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "ManufacturerCodeError")]
        [AsiatekRemote("CheckEditTerminalManufacturerCodeExists", "TerminalManufacturer", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "FieldExists")]
        public string ManufacturerCode { get; set; }
        /// <summary>
        /// 车载终端厂商所在地行政区划码（6位）
        /// </summary>
        [Display(Name = "Nationality", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [RegularExpression("^[0-9]{6}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "NationalityCodeError")]
        public string Nationality { get; set; }
    }

    /// <summary>
    /// 终端厂商下拉列表模型
    /// 包含ID、名称
    /// </summary>
    public class TerminalManufacturerDDLModel
    {
        public int ID { get; set; }
        public string ManufacturerName { get; set; }
    }
}

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
    public class TerminalTypeListModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 终端名称
        /// </summary>
        [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
        public string TerminalName { get; set; }
        /// <summary>
        /// 终端制造厂商
        /// </summary>
        [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
        public string ManufacturerName { get; set; }
        /// <summary>
        /// ACC ON 信号频率（秒）
        /// </summary>
        [Display(Name = "ACCON_Frequency", ResourceType = typeof(DisplayText))]
        public int ACCON_Frequency { get; set; }
        /// <summary>
        /// ACC OFF 信号频率（秒）
        /// </summary>
        [Display(Name = "ACCOFF_Frequency", ResourceType = typeof(DisplayText))]
        public int ACCOFF_Frequency { get; set; }
        /// <summary>
        /// 是否过滤异常信号点
        /// True：过滤 ；False：不过滤
        /// </summary>
        [Display(Name = "Filter", ResourceType = typeof(DisplayText))]
        public bool Filter { get; set; }
        /// <summary>
        /// 通信模式
        /// </summary>
        [Display(Name = "CommunicationMode", ResourceType = typeof(DisplayText))]
        public string CommunicationMode { get; set; }

    }

    public class TerminalTypeSearchModel
    {
        /// <summary>
        /// 终端名称
        /// </summary>
        [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
        public string TerminalName { get; set; }

        /// <summary>
        /// 终端制造厂商ID
        /// </summary>
        [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
        public int TerminalManufacturerID { get; set; }


        /// <summary>
        /// 制造厂商下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> TerminalManufacturerSelectList { get; set; }
    }


    public class TerminalTypeAddModel
    {
        /// <summary>
        /// 终端名称
        /// </summary>
        [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(200, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddTerminalNameExists", "TerminalType", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string TerminalName { get; set; }

        /// <summary>
        /// 终端制造厂商ID
        /// </summary>
        [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
        public int TerminalManufacturerID { get; set; }


        /// <summary>
        /// 制造厂商下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> TerminalManufacturerSelectList { get; set; }


        /// <summary>
        /// ACC ON 信号频率（秒）
        /// </summary>
        [Display(Name = "ACCON_Frequency", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Range(1, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int ACCON_Frequency { get; set; }

        /// <summary>
        /// ACC OFF 信号频率（秒）
        /// </summary>
        [Display(Name = "ACCOFF_Frequency", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Range(1, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int ACCOFF_Frequency { get; set; }

        /// <summary>
        /// 是否过滤异常信号点
        /// True：过滤 ；False：不过滤
        /// </summary>
        [Display(Name = "Filter", ResourceType = typeof(DisplayText))]
        public bool Filter { get; set; }

        /// <summary>
        /// 通信模式
        /// </summary>
        [Display(Name = "CommunicationMode", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string CommunicationMode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }


    public class TerminalTypeEditModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        [Display(Name = "TerminalName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(200, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditTerminalNameExists", "TerminalType", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string TerminalName { get; set; }

        /// <summary>
        /// 终端制造厂商ID
        /// </summary>
        [Display(Name = "Manufacturer", ResourceType = typeof(DisplayText))]
        public int TerminalManufacturerID { get; set; }


        /// <summary>
        /// 制造厂商下拉数据
        /// </summary>
        public IEnumerable<SelectListItem> TerminalManufacturerSelectList { get; set; }


        /// <summary>
        /// ACC ON 信号频率（秒）
        /// </summary>
        [Display(Name = "ACCON_Frequency", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Range(1, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int ACCON_Frequency { get; set; }

        /// <summary>
        /// ACC OFF 信号频率（秒）
        /// </summary>
        [Display(Name = "ACCOFF_Frequency", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [Range(1, uint.MaxValue, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "RangeError")]
        public int ACCOFF_Frequency { get; set; }

        /// <summary>
        /// 是否过滤异常信号点
        /// True：过滤 ；False：不过滤
        /// </summary>
        [Display(Name = "Filter", ResourceType = typeof(DisplayText))]
        public bool Filter { get; set; }

        /// <summary>
        /// 通信模式
        /// </summary>
        [Display(Name = "CommunicationMode", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string CommunicationMode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }



    public class TerminalTypeDDLModel
    {
        public int ID { get; set; }
        public string TerminalName { get; set; }
    }
}

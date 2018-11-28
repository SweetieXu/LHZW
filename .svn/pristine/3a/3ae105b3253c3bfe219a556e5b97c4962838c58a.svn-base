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
    public class ServiceProviderDDLModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }



    public class ServiceProviderSeachModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }
    }


    public class ServiceProviderListModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "Status", ResourceType = typeof(DisplayText))]
        public int Status { get; set; }
    }


    public class AddServiceProviderModel
    {

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(30, ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddSPNameExists", "ServiceProviderMaintain", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

    }


    public class EditServiceProviderModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(30, ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditSPNameExists", "ServiceProviderMaintain", "Admin", HttpMethod = "POST",
            AdditionalFields = "ID",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(50, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }
}

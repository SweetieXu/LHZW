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
    public class CommunicationTypeDDLModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }


    public class CommunicationTypeModel
    {
    }



    public class CommunicationTypeSeachModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }
    }


    public class CommunicationTypeListModel
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
        public string Status { get; set; }
    }


    public class AddCommunicationTypeModel
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
        public string Status { get; set; }
    }


    public class EditCommunicationTypeModel
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
        public string Status { get; set; }
    }
}

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
    public class ExceptionTypeModels
    {
        public int ID { get; set; }
        public string ExName { get; set; }
        public string Remark { get; set; }
    }

    public class ExceptionTypeSeachModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "ExName", ResourceType = typeof(DisplayText))]
        public string ExName { get; set; }
    }

    public class ExceptionTypeListModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "ExName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string ExName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 异常类型下拉列表模型
    /// </summary>
    public class ExceptionTypeDDLModel
    {
        public int ID { get; set; }
        public string ExName { get; set; }
    }


    #region   新增
    public class ExceptionTypeAddModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "ExName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string ExName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        public string Remark { get; set; }
    }
    #endregion

    #region   编辑
    public class ExceptionTypeEditModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "TypeID", ResourceType = typeof(DisplayText))]
        public int ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "ExName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string ExName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        public string Remark { get; set; }
    }
    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Asiatek.Resource;
using Asiatek.CustomDataAnnotations;

namespace Asiatek.Model
{

        #region 搜索类实体
        public class ReceiverMailInfoSearchModel
        {
            /// <summary>
            /// 邮箱
            /// </summary>
            [Display(Name = "ReceiverMail", ResourceType = typeof(DisplayText))]
            public string Email { get; set; }

            /// <summary>
            /// 姓名
            /// </summary>
            [Display(Name = "MailReceiverName", ResourceType = typeof(DisplayText))]
            public string Name { get; set; }
        }

        #endregion

        #region 列表实体
        public class ReceiverMailInfoListModel 
        {
            /// <summary>
            /// 编号
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// 邮箱
            /// </summary>
            [Display(Name = "ReceiverMail", ResourceType = typeof(DisplayText))]
            public string Email { get; set; }

            /// <summary>
            /// 姓名
            /// </summary>
            [Display(Name = "MailReceiverName", ResourceType = typeof(DisplayText))]
            public string Name { get; set; }

            /// <summary>
            /// 状态
            /// </summary>
            [Display(Name = "Status", ResourceType = typeof(DisplayText))]
            public bool Status { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
            public string Remark { get; set; }
        }

        #endregion
   
        #region 新增和编辑实体
        public class ReceiverMailInfoFormModel
        {
            public int ID { get; set; }

            /// <summary>
            /// 邮箱
            /// </summary>
            [Display(Name = "ReceiverMail", ResourceType = typeof(DisplayText))]
            [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
            [RegularExpression(@"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]{2,6}$", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "EmailFormatError")]
            [AsiatekRemote("CheckReceiverMailInfoExists", "ReceiverMailInfo", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
           ErrorMessageResourceType = typeof(DataAnnotations),
           ErrorMessageResourceName = "FieldExists")]
            public string Email { get; set; }

            /// <summary>
            /// 姓名
            /// </summary>
            [Display(Name = "MailReceiverName", ResourceType = typeof(DisplayText))]
            public string Name { get; set; }

            /// <summary>
            /// 状态
            /// </summary>
            [Display(Name = "Status", ResourceType = typeof(DisplayText))]
            public bool Status { get; set; }

            /// <summary>
            /// 备注
            /// </summary>
            [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
            public string Remark { get; set; }


        }
        #endregion

        #region 扩展类

        #endregion
}

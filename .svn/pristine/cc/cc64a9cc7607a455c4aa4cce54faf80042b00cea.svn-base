using Asiatek.Resource;
using System;
using System.ComponentModel.DataAnnotations;

namespace Asiatek.Model
{
    /// <summary>
    /// 服务器信息实体
    /// </summary>
    public class ServerManagerModel
    {
        /// <summary>
        /// 主键ID 自增
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 服务器编码
        /// </summary>
        public string ServerCode { get; set; }
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 链接服务器别名
        /// </summary>
        public string LinkedServerName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

    }


    /// <summary>
    /// 服务器搜索实体类
    /// </summary>
    public class ServerManagerSearchModel
    {
        [Display(Name = "ServerName", ResourceType = typeof(DisplayText))]
        public string ServerName { get; set; }
    }

    /// <summary>
    /// 服务器列表页实体
    /// </summary>
    public class ServerManagerListModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 服务器编码
        /// </summary>
        [Display(Name = "ServerCode", ResourceType = typeof(DisplayText))]
        public string ServerCode { get; set; }
        /// <summary>
        /// 服务器名称
        /// </summary>
        [Display(Name = "ServerName", ResourceType = typeof(DisplayText))]
        public string ServerName { get; set; }
        /// <summary>
        /// 链接服务器别名
        /// </summary>
        [Display(Name = "LinkedServerName", ResourceType = typeof(DisplayText))]
        public string LinkedServerName { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [Display(Name = "IP", ResourceType = typeof(DisplayText))]
        public string IP { get; set; }
        /// <summary>
        /// WCF地址
        /// </summary>
        [Display(Name = "WCFAddress", ResourceType = typeof(DisplayText))]
        public string WCFAddress { get; set; }
    }

    #region 服务器信息增加/编辑实体
    public class ServerManagerEditModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 服务器编码
        /// </summary>
        [Display(Name = "ServerCode", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string ServerCode { get; set; }
        /// <summary>
        /// 服务器名称
        /// </summary>
        [Display(Name = "ServerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(30, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string ServerName { get; set; }
        /// <summary>
        /// 链接服务器别名
        /// </summary>
        [Display(Name = "LinkedServerName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string LinkedServerName { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        [Display(Name = "IP", ResourceType = typeof(DisplayText))]
        public string IP { get; set; }
        /// <summary>
        /// WCF地址
        /// </summary>
        [Display(Name = "WCFAddress", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(150, ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MaxLength")]
        public string WCFAddress { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserID { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public int UpdateUserID { get; set; }
    }
    #endregion

    public class ServerInfoDDLModel
    {
        public int ID { get; set; }
        public string ServerName { get; set; }
    }

    /// <summary>
    /// 终端服务器信息
    /// </summary>
    public class TerminalServerInfoModel
    {
        /// <summary>
        /// 链接服务器名
        /// </summary>
        public string LinkedServerName { get; set; }
        /// <summary>
        /// WCF地址
        /// </summary>
        public string WCFAddress { get; set; }
    }

}

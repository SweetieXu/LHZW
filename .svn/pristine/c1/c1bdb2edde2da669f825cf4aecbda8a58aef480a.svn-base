using Asiatek.CustomDataAnnotations;
using Asiatek.Resource;
using System.ComponentModel.DataAnnotations;

namespace Asiatek.Model
{
    #region 查询实体
    public class NightBanSearchModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name", ResourceType = typeof(DisplayText))]
        public string NightBanName { get; set; }
        /// <summary>
        /// 禁行地点
        /// </summary>
        [Display(Name = "NightBanAddress", ResourceType = typeof(DisplayText))]
        public string NightBanAddress { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
        public int IsEnabled { get; set; }
    }
    #endregion

    #region 列表实体
    public class NightBanListModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name", ResourceType = typeof(DisplayText))]
        public string NightBanName { get; set; }
        /// <summary>
        /// 禁行地点
        /// </summary>
        [Display(Name = "NightBanAddress", ResourceType = typeof(DisplayText))]
        public string NightBanAddress { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "ExceptionStartDateTime", ResourceType = typeof(DisplayText))]
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        public string EndTime { get; set; }
    }
    #endregion

    #region 新增编辑实体
    public class NightBanEditModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "Name", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [AsiatekRemote("CheckNightBanExists", "NightBan", "Admin", AdditionalFields = "ID", HttpMethod = "POST",
        ErrorMessageResourceType = typeof(DataAnnotations),ErrorMessageResourceName = "FieldExists")]
        public string NightBanName { get; set; }
        /// <summary>
        /// 禁行地点
        /// </summary>
        [Display(Name = "NightBanAddress", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string NightBanAddress { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "FenceState", ResourceType = typeof(DisplayText))]
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "ExceptionStartDateTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "EndTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string EndTime { get; set; }
        public int CreateUserID { get; set; }
        public int UpdateUserID { get; set; }
    }
    #endregion

}

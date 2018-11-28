using Asiatek.Resource;
using System.ComponentModel.DataAnnotations;

namespace Asiatek.Model
{
    #region 查询实体
    public class TransportManagementSearchModel
    {
        /// <summary>
        /// 运管所编码
        /// </summary>
        [Display(Name = "TransportManagementCode", ResourceType = typeof(DisplayText))]
        public string Code { get; set; }
        /// <summary>
        /// 运管所名称
        /// </summary>
        [Display(Name = "TransportManagementName", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }
    }
    #endregion

    #region 列表实体
    public class TransportManagementListModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 运管所编码
        /// </summary>
        [Display(Name = "TransportManagementCode", ResourceType = typeof(DisplayText))]
        public string Code { get; set; }
        /// <summary>
        /// 运管所名称
        /// </summary>
        [Display(Name = "TransportManagementName", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }
    }
    #endregion

    #region 运管所下拉框实体
    public class TransportManagementDDLModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 运管所名称
        /// </summary>
        public string Name { get; set; }
        public bool IsHas { get; set; }
    }
    #endregion

}

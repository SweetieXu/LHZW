using Asiatek.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asiatek.Model
{
    public class BusinessScopeModel
    { 

        /// <summary>
        /// 经营范围编号
        /// </summary>
        [Display(Name = "BusinessScopeCode", ResourceType = typeof(DisplayText))]
        public string Code { get; set; }
        /// <summary>
        /// 经营范围名称
        /// </summary>
        [Display(Name = "BusinessScopeName", ResourceType = typeof(DisplayText))]
        public string Name { get; set; }
    }

    public class StructureBussinessScopeDDLModel
    { 
        /// <summary>
        /// 经营范围编号
        /// </summary>
        public string BusinessScopeCode { get; set; }
        /// <summary>
        /// 经营范围名称
        /// </summary>
        public string Name { get; set; }
    }

    public class StrucVehicleBussinessScopeDDLModel
    {
        /// <summary>
        /// 经营范围名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 经营范围编号
        /// </summary>
        public string BusinessScopeCode { get; set; }
        /// <summary>
        /// 车辆是否设定对应的经营范围 1 是 0 否
        /// </summary>
        public bool IsHas { get; set; }
    }
}

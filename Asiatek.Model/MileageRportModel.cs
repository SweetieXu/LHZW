using Asiatek.CustomDataAnnotations;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Asiatek.AjaxPager;

namespace Asiatek.Model
{
    public class MileageRportSeachModel : ContainsPagedDatas<MileageRportListModel>
    {
        public long ID { get; set; }

        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        [Display(Name = "StartDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [LegalDateTime(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateTimeError")]
        public string GPSStartTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        [Display(Name = "ExpirationDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [LegalDateTime(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateTimeError")]
        [StartEndDateTime("GPSStartTime", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "TimeInterval")]
        public string GPSEndTime { get; set; }
    }

    public class MileageRportListModel
    {
        public long ID { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        [Display(Name = "StrucName", ResourceType = typeof(DisplayText))]
        public string StrucName { get; set; }

        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }

        /// <summary>
        /// 起始里程
        /// </summary>
        [Display(Name = "StartDate", ResourceType = typeof(DisplayText))]
        public DateTime GPSStartTime { get; set; }

        /// <summary>
        /// 终止里程
        /// </summary>
        [Display(Name = "ExpirationDate", ResourceType = typeof(DisplayText))]
        public DateTime GPSEndTime { get; set; }

        /// <summary>
        /// 里程
        /// </summary>
        [Display(Name = "Duration", ResourceType = typeof(DisplayText))]
        public string Duration { get; set; }

        /// <summary>
        /// 状态 ：默认0、删除9、报废8、使用中7
        /// </summary>
        //public int Status { get; set; }
    }
}

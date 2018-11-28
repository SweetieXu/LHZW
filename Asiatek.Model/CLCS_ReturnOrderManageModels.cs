using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    public class CLCS_ReturnOrderListModel
    {
        public int ID { get; set; }

        [Display(Name="车牌号")]
        public string PlateNum { get; set; }

        [Display(Name = "开始时间")]
        public DateTime StartTime { get; set; }

        [Display(Name = "截止时间")]
        public DateTime EndTime { get; set; }

        [Display(Name = "回单里程")]
        public double ReturnMileage { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "添加时间")]
        public DateTime CreateTime { get; set; }

    }

    public class CLCS_ReturnOrderSearchModel
    {
        [Display(Name = "车牌号")]
        public string PlateNum { get; set; }
    }

    public class AddCLCS_ReturnOrderModel
    {
        [Display(Name = "车牌号")]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [StringLength(8, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        public string PlateNum { get; set; }

        [Display(Name = "开始时间")]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string StartTime { get; set; }

        [Display(Name = "截止时间")]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public string EndTime { get; set; }

        [Display(Name = "回单里程")]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        public double? ReturnMileage { get; set; }

        [Display(Name = "备注")]
        [StringLength(500, ErrorMessageResourceType = typeof(DataAnnotations),ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }
    }

}

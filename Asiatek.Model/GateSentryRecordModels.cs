using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    
    public class GateSentryRecordSearchModel
    {
        
        [Display(Name = "车牌号")]
        public string PlateNum { get; set; }

        [Display(Name = "开始时间")]
        public string StartTime { get; set; }

        [Display(Name = "结束时间")]
        public string EndTime { get; set; }
    }

    public class GateSentryRecordListModel
    {
        public int ID { get; set; }

        [Display(Name = "车牌号")]
        public string PlateNum { get; set; }

        [Display(Name = "门岗名称")]
        public string PassGate { get; set; }

        [Display(Name = "进出门岗")]
        public string InOrOut { get; set; }

        [Display(Name = "进出门岗时间")]
        public DateTime InOrOutTime { get; set; }

    }

    public class GateSentryVehicleSearchModel
    {
        public int VID { get; set; }

        [Display(Name = "车牌号")]
        public string PlateNum { get; set; }

    }


}

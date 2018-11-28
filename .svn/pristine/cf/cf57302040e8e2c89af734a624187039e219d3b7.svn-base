using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiatek.WebApis.Models
{
    /// <summary>
    ///  请求参数
    /// </summary>
    public class CLCS_MileageModel
    {
        /// <summary>
        ///  车牌号
        /// </summary>
        [Required]
        public string PlateNum { get; set; }
        /// <summary>
        ///  开始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }
        /// <summary>
        ///  结束时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 里程数据  读取在配置文件中设置的车牌号对应的里程数据
        /// </summary>
        public double ReturnMileage { get; set; }

    }


    public class ServerInfoModel
    {
        /// <summary>
        /// 链接服务器名称
        /// </summary>
        public string LinkedServerName { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
    }
    
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asiatek.WebApis.Models
{
    /// <summary>
    /// 观澜总务 返回实时信号位置信息
    /// </summary>
    public class GLZW_RealTimeModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }
    }


    /// <summary>
    /// 观澜总务  查询电子围栏进出信息传递过来的参数
    /// </summary>
    public class GLZW_EFSearchModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        [Required]
        public string PlateNum { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }
    }


    /// <summary>
    /// 观澜总务  返回的电子围栏进行信息
    /// </summary>
    public class GLZW_EFModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string VehicleCode { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }
        /// <summary>
        /// 电子围栏名称
        /// </summary>
        public string FenceName { get; set; }
        /// <summary>
        /// 异常开始时间
        /// </summary>
        public DateTime ExceptionStartTime { get; set; }
        /// <summary>
        /// 异常结束时间
        /// </summary>
        public DateTime ExceptionEndTime { get; set; }
        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
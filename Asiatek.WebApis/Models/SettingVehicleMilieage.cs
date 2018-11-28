using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiatek.WebApis.Models
{
    /// <summary>
    /// 配置文件中设置的key[SettingVehicleMilieage]
    /// </summary>
    public class SettingVehicleMilieage
    {
        public IEnumerable<DataModel> Datas { get; set; }
    }
    /// <summary>
    /// Datas数组中存放的是{'PlateNum':'xxx','Mileage':'xxx'}这样的对象数组
    /// </summary>
    public class DataModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 里程
        /// </summary>
        public double Mileage { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.Model.TerminalSetting
{
    public class MapRegionSettingQueryModel
    {
        /// <summary>
        /// 区域类型
        /// </summary>
        public int RegionType { get; set; }

        /// <summary>
        /// PlateNum#TerminalCode数组
        /// </summary>
        public string[] Vehicles { get; set; }
    }
}

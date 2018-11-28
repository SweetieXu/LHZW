using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.Model.TerminalSetting
{
    public class MapRegionSettingModel
    {
        public long RowNumber { get; set; }
        public long Key { get; set; }
        public string PlateNum { get; set; }
        public long PathID { get; set; }
        public string PathName { get; set; }
        public int RegionsType { get; set; }
        public short Property { get; set; }
        public bool Periodic { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double SpeedLimit { get; set; }
        public int OverSpeedDuration { get; set; }
    }
}

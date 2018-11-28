using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.Model.TerminalSetting
{
    public class MapLineSettingModel
    {
        public long RowNumber { get; set; }
        public long Key { get; set; }
        public string PlateNum { get; set; }
        public long PathID { get; set; }
        public string PathName { get; set; }
        public ushort Property { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}

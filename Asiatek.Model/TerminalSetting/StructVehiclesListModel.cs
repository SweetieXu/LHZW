using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiatek.Model.TerminalSetting
{
    public class StrucVehiclesListModel
    {
        public string CompanyName { get; set; }

        public KeyValuePair<string, string>[] Vehicles { get; set; }
    }
}
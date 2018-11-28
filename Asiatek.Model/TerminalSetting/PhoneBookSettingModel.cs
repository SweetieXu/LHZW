using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.Model.TerminalSetting
{
    public class PhoneBookSettingModel
    {
        /// <summary>
        /// 设置类型
        /// </summary>
        public byte SettingType { get; set; }

        /// <summary>
        /// 呼叫类型
        /// </summary>
        public byte CallType { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
    }
}

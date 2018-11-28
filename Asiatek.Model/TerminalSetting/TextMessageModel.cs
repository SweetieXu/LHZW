using System;
using System.Collections.Generic;

namespace Asiatek.Model.TerminalSetting
{
    public class TextMessageModel
    {
        /// <summary>
        /// TextInformationFlag
        /// </summary>
        public byte Flags { get; set; }

        /// <summary>
        /// 文本消息
        /// </summary>
        public string MessageText { get; set; }
    }
}

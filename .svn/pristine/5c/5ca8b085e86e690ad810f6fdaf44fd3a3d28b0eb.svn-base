using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.Common
{
    public class FontHelper
    {
        //简体和繁体相互转换
        public static string StringConvert(string x, string type)
        {
            String value = String.Empty;
            switch (type)
            {
                case "1"://转繁体
                    value = Microsoft.VisualBasic.Strings.StrConv(x, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
                    break;
                case "2"://转简体
                    value = Microsoft.VisualBasic.Strings.StrConv(x, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
                    break;
                default:
                    break;
            }
            return value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Asiatek.Common
{
    /// <summary>
    /// MD5帮助类
    /// </summary>
    public class MD5Helper
    {
        ///// <summary>
        ///// 根据输入的字符串获取对应的32位大写MD5摘要
        ///// </summary>
        //public static string GetMD5Str(string str)
        //{
        //    using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        //    {
        //        byte[] bytes = Encoding.UTF8.GetBytes(str);
        //        bytes = md5.ComputeHash(bytes);
        //        StringBuilder sb = new StringBuilder();
        //        foreach (byte num in bytes)
        //        {
        //            sb.AppendFormat("{0:X2}", num);
        //        }
        //        return sb.ToString();
        //    }
        //}

        /// <summary>
        /// 根据输入的字符串获取对应MD5摘要
        /// </summary>
        public static string GetMD5Str(string str)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] bytes = Encoding.Unicode.GetBytes(str);
                bytes = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte num in bytes)
                {
                    sb.AppendFormat("{0:x}", num);
                }
                return sb.ToString();
            }
        }
    }
}

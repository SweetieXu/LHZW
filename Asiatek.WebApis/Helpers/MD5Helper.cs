using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Asiatek.WebApis.Helpers
{
    /// <summary>
    /// MD5帮助类
    /// </summary>
    public class MD5Helper
    {
        /// <summary>
        /// 32位小写MD5值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5Str(string str)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                bytes = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte num in bytes)
                {
                    sb.AppendFormat("{0:X2}", num);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 获得16位小写MD5字符串
        /// </summary>
        public static string GetLen16LowerCaseMD5Str(string str)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str)), 4, 8).Replace("-", "").ToLower();
            }
        }

    }
}
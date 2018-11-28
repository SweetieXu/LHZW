using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Asiatek.Common
{
    public static class ImageExtension
    {
        /// <summary>
        /// 根据图形获取图形的扩展名
        /// </summary>
        public static string GetImageExtension(this Image image)
        {
            Type Type = typeof(ImageFormat);
            System.Reflection.PropertyInfo[] props = Type.GetProperties(BindingFlags.Static | BindingFlags.Public);
            for (int i = 0; i != props.Length; i++)
            {
                ImageFormat temp = (ImageFormat)props[i].GetValue(null, null);
                if (temp.Guid.Equals(image.RawFormat.Guid))
                {
                    return props[i].Name;
                }
            }
            return "";
        }
    }
}

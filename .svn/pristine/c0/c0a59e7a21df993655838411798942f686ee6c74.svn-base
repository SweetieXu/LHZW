using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.Common
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple=false)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        public EnumDescriptionAttribute(string desc)
        {
            this.Description = desc;
        }

        public EnumDescriptionAttribute(string TypeName, string PropertyName)
        {
            Assembly This = Assembly.GetAssembly(this.GetType());
            Uri uri = new Uri(Path.Combine(Path.GetDirectoryName(This.CodeBase), "Asiatek.Resource.dll"));
            Assembly res = System.Reflection.Assembly.LoadFrom(uri.LocalPath);
            string fullName = TypeName;
            if (!TypeName.StartsWith("Asiatek.Resource"))
                fullName = "Asiatek.Resource." + TypeName;
            Type uiText = res.GetType(fullName, true);
            this.Description = uiText.GetProperty(PropertyName).GetValue(null, null) as string;
        }

        public string Description { get; private set; }

        public static string GetDescriptionValue(Type type, object value)
        {
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = type.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    var attr = Attribute.GetCustomAttribute(fieldInfo, typeof(EnumDescriptionAttribute), false) as EnumDescriptionAttribute;
                    return attr.Description;
                }
            }
            throw new InvalidOperationException(string.Format("枚举类型 {0} 当中不包含EnumDescriptionAttribute特效。", type.FullName));
        }

        /// <summary>
        /// 获取枚举值+描述 
        /// </summary>
        /// <param name="enumType">Type,该参数的格式为typeof(需要读的枚举类型)</param>
        /// <param name="Isextend">是否需要显示第一行自定义文本 当为false时 后面两个参数无效（extendText，extendValue）</param>
        /// <param name="extendText">自定义第一行文本</param>
        /// <param name="extendValue">自定义第一行值</param>
        /// <returns></returns>
        public static List<SelectListItem> GetEnumDescDropDownList(Type enumType, bool Isextend = true, string extendText = "请选择", string extendValue = "-1")
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (Isextend)
            {
                items.Add(new SelectListItem() { Text = extendText, Value = extendValue });
            }
            if (enumType.BaseType != typeof(Enum))
            {
                //throw new InvalidOperationException("属性不是枚举类型");
                return items;
            }


            Type typeDescription = typeof(DescriptionAttribute);

            FieldInfo[] fields = enumType.GetFields();
            string strText = string.Empty;
            string strValue = string.Empty;

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue = ((byte)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        DescriptionAttribute desc = (DescriptionAttribute)arr[0];
                        strText = desc.Description;
                    }
                    else
                    {
                        strText = field.Name;
                    }
                    items.Add(new SelectListItem() { Text = strText, Value = strValue });
                }
            }
            return items;
        }
    }
}
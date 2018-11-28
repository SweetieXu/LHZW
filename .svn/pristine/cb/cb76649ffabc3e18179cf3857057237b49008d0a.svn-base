/* 
 模块：报表管理
 编写：蒋正波
 时间：2016-10-26
 功能：搜索功能栏中对'日期时间'格式的判断
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Asiatek.CustomDataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LegalDateTimeAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 日期+时间
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public const string DATETIMEPATTERN = @"^([2][0][0-9][0-9])(\-)([1-9]|[1][0-2])(\-)([0-2][0-9]|[3][0-1])( )([0-1][0-9]|[2][0-3])(:)([0-5][0-9])(:)([0-5][0-9])$";

        public LegalDateTimeAttribute() { }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (value is string)
            {
                if (new Regex(DATETIMEPATTERN).IsMatch(value.ToString()))//检查是否是日期+时间
                {
                    DateTime temp;
                    return DateTime.TryParse(value.ToString(), out temp);//是否是有效的时间
                }
            }
            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule()
            {
                //该值必须为全小写,其值与自定义的客户端脚本中验证方法名相同
                //属性data-val-LegalDateAttribute="ErrorMessage"会添加到对应的HTML标签上
                ValidationType = "legaldatetime",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            yield return rule;
        }
    }
}

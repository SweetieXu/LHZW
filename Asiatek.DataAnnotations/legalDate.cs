using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Asiatek.CustomDataAnnotations
{

    /// <summary>
    /// 验证是否是合法的日期
    /// 作者：戴天辰
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LegalDateAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 日期正则
        /// yyyy-MM-dd
        /// </summary>
        public const string DatePATTERN = @"^((?:19|20)\d\d)-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$";



        public LegalDateAttribute() { }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (value is string)
            {
                if (new Regex(DatePATTERN).IsMatch(value.ToString()))//检查是否是日期 不包含时间
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
                ValidationType = "legaldate",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            yield return rule;
        }
    }
}

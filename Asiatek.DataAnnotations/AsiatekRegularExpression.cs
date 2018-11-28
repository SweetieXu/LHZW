using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.CustomDataAnnotations
{

    /// <summary>
    /// 自定义的正则表达式验证特性
    /// MVC自带的RegularExpressionAttribute会跳过非空验证
    /// 即正则表达式中类似{1,}的要求均无效，必须为属性加上Required特性
    /// 该自定义特性不会跳过非空验证、对元素的值进行正则表达式的完全匹配验证
    /// 作者：戴天辰
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AsiatekRegularExpressionAttribute : RegularExpressionAttribute, IClientValidatable
    {
        public AsiatekRegularExpressionAttribute(string pattern)
            : base(pattern)
        {

        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            if (value is string)
            {
                Regex reg = new Regex(this.Pattern);
                return reg.IsMatch(value.ToString());
            }
            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "asiatekregex"
            };
            rule.ValidationParameters.Add("pattern", this.Pattern);
            yield return rule;
        }
    }

}
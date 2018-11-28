using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Asiatek.CustomDataAnnotations
{

    /// <summary>
    /// 用于比较某个模型的两个属性值是否不匹配
    /// 如果匹配则提示错误
    /// 作者：戴天辰
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UnMatchedAttribute : CompareAttribute, IClientValidatable
    {
        public UnMatchedAttribute(string otherProperty) : base(otherProperty) { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(base.ErrorMessage, new string[] { validationContext.MemberName });
            }
            //根据依赖的属性名获取依赖的属性信息
            var field = validationContext.ObjectType.GetProperty(this.OtherProperty);

            //获取依赖的属性的实际值
            var otherPropertyValue = field.GetValue(validationContext.ObjectInstance, null);

            if (otherPropertyValue.ToString() == value.ToString())
            {
                //验证失败，返回错误结果
                return new ValidationResult(base.ErrorMessage, new string[] { validationContext.MemberName });
            }
            //验证成功
            return ValidationResult.Success;
        }

        public new IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule()
            {
                //该值必须为全小写,其值与自定义的客户端脚本中验证方法名相同
                //属性data-val-unmatched="ErrorMessage"会添加到对应的HTML标签上
                ValidationType = "unmatched",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters.Add("otherproperty", this.OtherProperty);
            yield return rule;
        }
    }
}

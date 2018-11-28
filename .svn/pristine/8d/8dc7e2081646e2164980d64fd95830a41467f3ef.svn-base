using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Asiatek.CustomDataAnnotations
{

    ///// <summary>
    ///// 验证日期是否在规定的范围
    ///// 作者：jzb
    ///// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StartEndDateTimeAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 验证依赖属性名
        /// </summary>
        private string _dependentPropertyName;

        /// <summary>
        /// <para>dependentPropertyName：依赖属性名</para>
        /// </summary>
        public StartEndDateTimeAttribute(string dependentPropertyName)
        {
            _dependentPropertyName = dependentPropertyName;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //根据依赖的属性名获取依赖的属性信息
            var field = validationContext.ObjectType.GetProperty(_dependentPropertyName);

            //获取依赖的属性的实际值
            var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);

            DateTime val;
            if (!DateTime.TryParse(dependentvalue.ToString(), out val))
            {
                //验证失败，返回错误结果
                return new ValidationResult(base.ErrorMessage, new string[] { validationContext.MemberName });
            }
            if (!DateTime.TryParse(value.ToString(), out val))
            {
                //验证失败，返回错误结果
                return new ValidationResult(base.ErrorMessage, new string[] { validationContext.MemberName });
            }

            var currentDateTime = DateTime.Parse(value.ToString());
            var dependentDateTime = DateTime.Parse(dependentvalue.ToString());

            if (dependentDateTime > currentDateTime)
            {
                //验证失败，返回错误结果
                return new ValidationResult(base.ErrorMessage, new string[] { validationContext.MemberName });
            }
            if (Math.Round(currentDateTime.Subtract(dependentDateTime).TotalDays) > 30)
            {
                //验证失败，返回错误结果
                return new ValidationResult(base.ErrorMessage, new string[] { validationContext.MemberName });
            }
            //验证成功
            return ValidationResult.Success;
        }

        /// <summary>
        /// 返回客户端验证规则
        /// </summary>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                //该值必须为全小写,其值与自定义的客户端脚本中验证方法名相同
                //属性data-val-requirediftrue="ErrorMessage"会添加到对应的HTML标签上
                ValidationType = "startenddatetime",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            //data-val-requirediftrue-dependentproperty="_dependentPropertyName"
            rule.ValidationParameters.Add("dependentproperty", _dependentPropertyName);
            yield return rule;
        }
    }
}

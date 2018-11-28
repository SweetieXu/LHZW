using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.CustomDataAnnotations
{
    /// <summary>
    /// 指定当依赖的属性值为False时需要数据字段值
    /// 作者：戴天辰
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredIfNotTrueAttribute : RequiredAttribute, IClientValidatable
    {

        /// <summary>
        /// 验证依赖属性名
        /// </summary>
        private string _dependentPropertyName;

        /// <summary>
        /// <para>dependentPropertyName：依赖属性名</para>
        /// </summary>
        public RequiredIfNotTrueAttribute(string dependentPropertyName)
        {
            _dependentPropertyName = dependentPropertyName;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //根据依赖的属性名获取依赖的属性信息
            var field = validationContext.ObjectType.GetProperty(_dependentPropertyName);

            //获取依赖的属性的实际值
            var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);

            if (dependentvalue.Equals(false))//如果依赖的值等于false，那么就需要验证当前属性的有效性
            {
                if (!base.IsValid(value))//进行Required验证
                {
                    //验证失败，返回错误结果
                    return new ValidationResult(base.ErrorMessage, new string[] { validationContext.MemberName });
                }
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
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                //该值必须为全小写,其值与自定义的客户端脚本中验证方法名相同
                //属性data-val-requirediftrue="ErrorMessage"会添加到对应的HTML标签上
                ValidationType = "requiredifnottrue"
            };
            //data-val-requirediftrue-dependentproperty="_dependentPropertyName"
            rule.ValidationParameters.Add("dependentproperty", _dependentPropertyName);
            yield return rule;
        }
    }

}
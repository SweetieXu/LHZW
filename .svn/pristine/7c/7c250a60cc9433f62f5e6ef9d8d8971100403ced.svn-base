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
    /// 验证字段是否是中国大陆电话号码
    ///  包括座机号、手机号
    /// 作者：戴天辰
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ChinesePhoneAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// 座机正则
        /// 可以是
        /// 区号-电话号-分机号
        /// 区号-电话号
        /// 电话号-分机号
        /// 电话号
        /// </summary>
        public const string TelPATTERN = @"^(0\d{2,3}-)?\d{7,8}(-\d{3,4})?$";

        /// <summary>
        /// 手机正则
        /// </summary>
        public const string MobiliePATTERN = @"^1[0-9]{10}$";


        public ChinesePhoneAttribute() { }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (value is string)
            {
                Regex reg1 = new Regex(TelPATTERN);
                Regex reg2 = new Regex(MobiliePATTERN);
                return reg1.IsMatch(value.ToString()) || reg2.IsMatch(value.ToString());
            }
            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule()
            {
                //该值必须为全小写,其值与自定义的客户端脚本中验证方法名相同
                //属性data-val-chinesePhone="ErrorMessage"会添加到对应的HTML标签上
                ValidationType = "chinesephone",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            yield return rule;
        }
    }

}
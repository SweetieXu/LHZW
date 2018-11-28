using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Asiatek.CustomDataAnnotations
{
    /// <summary>
    /// 验证日期是否在指定的范围
    /// 包含端点
    /// 作者：戴天辰
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateRangeAttribute : ValidationAttribute, IClientValidatable
    {

        /// <summary>
        /// 最小时间
        /// </summary>
        object _min;
        /// <summary>
        /// 最大时间
        /// </summary>
        object _max;

        public DateRangeAttribute(object min, object max)
        {
            this._min = min;
            this._max = max;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            if (value is string)
            {
                DateTime valDate;
                if (!DateTime.TryParse(value.ToString(), out valDate))
                {
                    return false;
                }
                if (_min != null && _max != null)
                {
                    return valDate >= DateTime.Today.AddDays(Convert.ToInt32(_min))
                        && valDate <= DateTime.Today.AddDays(Convert.ToInt32(_max));
                }
                else if (_min != null)
                {
                    return valDate >= DateTime.Today.AddDays(Convert.ToInt32(_min));
                }
                else
                {
                    return valDate <= DateTime.Today.AddDays(Convert.ToInt32(_max));
                }
            }
            return false;
        }


        public override string FormatErrorMessage(string name)
        {
            if (_min != null && _max != null)
            {
                var min = DateTime.Today.AddDays(Convert.ToInt32(_min)).ToString("yyyy-MM-dd");
                var max = DateTime.Today.AddDays(Convert.ToInt32(_max)).ToString("yyyy-MM-dd");
                return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, min, max);
            }
            else if (_min != null)
            {
                var min = DateTime.Today.AddDays(Convert.ToInt32(_min)).ToString("yyyy-MM-dd");
                return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, min);
            }
            else
            {
                var max = DateTime.Today.AddDays(Convert.ToInt32(_max)).ToString("yyyy-MM-dd");
                return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, max);
            }

        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule()
            {
                //该值必须为全小写,其值与自定义的客户端脚本中验证方法名相同
                //属性data-val-daterange="ErrorMessage"会添加到对应的HTML标签上
                ValidationType = "daterange",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName())
            };
            rule.ValidationParameters["min"] = _min;
            rule.ValidationParameters["max"] = _max;
            yield return rule;
        }
    }
}

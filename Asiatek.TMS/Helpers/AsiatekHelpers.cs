using Asiatek.AjaxPager;
using Asiatek.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;


namespace Asiatek.TMS.Helpers
{

    public static class AsiatekHelpers
    {
        #region 超链接
        /// <summary>
        /// 返回一个非注入式Ajax链接
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">动作名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="areaName">区域名</param>
        /// <param name="ajaxOptions">Ajax参数</param>
        public static MvcHtmlString AsiatekActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, string areaName, AjaxOptions ajaxOptions)
        {
            return AjaxExtensions.ActionLink(ajaxHelper, linkText, actionName, controllerName, new { area = areaName }, ajaxOptions);
        }
        /// <summary>
        /// 返回一个非注入式Ajax链接
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">动作名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="areaName">区域名</param>
        /// <param name="htmlAttributes">html特性</param>
        /// <param name="ajaxOptions">Ajax参数</param>
        public static MvcHtmlString AsiatekActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, string areaName, object htmlAttributes, AjaxOptions ajaxOptions)
        {
            return AjaxExtensions.ActionLink(ajaxHelper, linkText, actionName, controllerName, new { area = areaName }, ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// 返回一个链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">动作名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="areaName">区域名</param>
        public static MvcHtmlString AsiatekActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string areaName)
        {
            return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, new { controller = controllerName, area = areaName });
        }
        /// <summary>
        /// 返回一个链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">链接文字</param>
        /// <param name="actionName">动作名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="areaName">区域名</param>
        /// <param name="htmlAttributes">html特性</param>
        public static MvcHtmlString AsiatekActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string areaName, object htmlAttributes)
        {
            return LinkExtensions.ActionLink(htmlHelper, linkText, actionName, new { controller = controllerName, area = areaName }, htmlAttributes);
        }
        #endregion




        #region DisplayNameFor扩展

        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<SelectResult<TModel>> html, Expression<Func<TModel, TValue>> expression)
        {

            ModelMetadata mmd = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, new ViewDataDictionary<TModel>());
            string displayName = mmd.DisplayName;
            string modelName = ExpressionHelper.GetExpressionText(expression);
            if (displayName == null)
            {
                string propertyName = mmd.PropertyName;
                if (propertyName == null)
                    displayName = Enumerable.Last<string>((IEnumerable<string>)modelName.Split('.'));
                else
                    displayName = propertyName;
            }
            return new MvcHtmlString(HttpUtility.HtmlEncode(displayName));
        }


        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<SelectResult<AsiatekPagedList<TModel>>> html, Expression<Func<TModel, TValue>> expression)
        {

            ModelMetadata mmd = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, new ViewDataDictionary<TModel>());
            string displayName = mmd.DisplayName;
            string modelName = ExpressionHelper.GetExpressionText(expression);
            if (displayName == null)
            {
                string propertyName = mmd.PropertyName;
                if (propertyName == null)
                    displayName = Enumerable.Last<string>((IEnumerable<string>)modelName.Split('.'));
                else
                    displayName = propertyName;
            }
            return new MvcHtmlString(HttpUtility.HtmlEncode(displayName));
        }


        #endregion

        #region 其他

        /// <summary>
        /// 转换为下拉列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> list, Func<T, SelectListItem> func)
        {
            List<SelectListItem> sli = new List<SelectListItem>();
            foreach (var item in list)
            {
                sli.Add(func(item));
            }
            return sli;
        }


        /// <summary>
        /// 转换IEnumerable为带“全部”的下拉选项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectListWithAll<T>(this IEnumerable<T> list, Func<T, SelectListItem> func)
        {
            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem() { Text = Asiatek.Resource.UIText.All, Value = "-1", Selected = true });
            foreach (var item in list)
            {
                sli.Add(func(item));
            }
            return sli;
        }

        /// <summary>
        /// 转换IEnumerable为带“请选择”的下拉选项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectListWithChoose<T>(this IEnumerable<T> list, Func<T, SelectListItem> func)
        {
            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem() { Text = Asiatek.Resource.DisplayText.PleaseSelect, Value = "-1", Selected = true });
            foreach (var item in list)
            {
                sli.Add(func(item));
            }
            return sli;
        }


        /// <summary>
        /// 转换IEnumerable为带“无”的下拉选项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectListWithEmpty<T>(this IEnumerable<T> list, Func<T, SelectListItem> func, object selectedValue = null)
        {
            List<SelectListItem> sli = new List<SelectListItem>();
            sli.Add(new SelectListItem() { Text = string.Empty, Value = "-1", Selected = selectedValue == null });
            foreach (var item in list)
            {
                sli.Add(func(item));
            }
            if (selectedValue != null)
            {
                var obj = sli.Find(item => item.Value == selectedValue.ToString());
                if (obj != null)
                {
                    obj.Selected = true;
                }
            }
            return sli;
        }
        #endregion


        #region LabelFor扩展
        /// <summary>
        /// 为必填项字段生成*符号
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelForRequired<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            TagBuilder starFlag = new TagBuilder("span");
            starFlag.SetInnerText("*");
            starFlag.AddCssClass("requiredField");
            return MvcHtmlString.Create(starFlag.ToString() + html.LabelFor(expression).ToString());
        }



        /// <summary>
        /// 包含*符号的Label
        /// </summary>
        public static MvcHtmlString LabelForRequired(this HtmlHelper html, string labelText)
        {
            TagBuilder starFlag = new TagBuilder("span");
            starFlag.SetInnerText("*");
            starFlag.AddCssClass("requiredField");

            TagBuilder label = new TagBuilder("label");
            label.SetInnerText(labelText);

            return MvcHtmlString.Create(starFlag.ToString() + label.ToString());
        }
        #endregion



        #region 递归
        public static void Render<T>(this HtmlHelper helper, T model, SelfApplicable<T> f)
        {
            f(f, model);
        }

        public static void Render<T1, T2>(this HtmlHelper helper, T1 arg1, T2 arg2, SelfApplicable2<T1, T2> f)
        {
            f(f, arg1, arg2);
        }

        public delegate void SelfApplicable<T>(SelfApplicable<T> self, T arg);
        public delegate void SelfApplicable2<T1, T2>(SelfApplicable2<T1, T2> self, T1 arg1, T2 arg2);
        #endregion


        #region 枚举下拉扩展
        //public static MvcHtmlString EnumDropDownListFor<TModel, TProperty, TEnum>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression, TEnum selectedValue)
        //{

        //    var propType = typeof(TProperty);//当前属性类型
        //    var baseType = propType.BaseType;//属性基础类型

        //    if (baseType != typeof(Enum))
        //    {
        //        throw new InvalidOperationException("属性不是枚举类型");
        //    }
        //    var underlyingType = propType.GetEnumUnderlyingType();//枚举类型的基础类型

        //    var enumValues = Enum.GetValues(propType).Cast<TProperty>();

        //    IEnumerable<SelectListItem> items = from value in enumValues
        //                                        let valStr = value.ToString()
        //                                        where valStr != "None"
        //                                        select new SelectListItem()
        //                                        {
        //                                            Text = valStr,
        //                                            Value = Convert.ChangeType(value, underlyingType).ToString(),
        //                                            Selected = (value.Equals(selectedValue))
        //                                        };
        //    return htmlHelper.DropDownListFor(expression, items);

        //}


        private static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
    Expression<Func<TModel, TProperty>> expression, string filterVal)
        {
            var propType = typeof(TProperty);//当前属性类型
            var baseType = propType.BaseType;//属性基础类型

            if (baseType != typeof(Enum))
            {
                throw new InvalidOperationException("属性不是枚举类型");
            }
            var underlyingType = propType.GetEnumUnderlyingType();//枚举类型的基础类型
            var enumValues = Enum.GetValues(propType).Cast<TProperty>();//获取所有枚举值

            IEnumerable<SelectListItem> items = null;
            if (string.IsNullOrWhiteSpace(filterVal))
            {
                items = from value in enumValues
                        select new SelectListItem()
                        {
                            Text = value.ToString(),
                            Value = Convert.ChangeType(value, underlyingType).ToString()
                        };
            }
            else
            {
                items = from value in enumValues
                        let valStr = value.ToString()
                        where valStr != filterVal
                        select new SelectListItem()
                        {
                            Text = valStr,
                            Value = Convert.ChangeType(value, underlyingType).ToString()
                        };
            }


            return htmlHelper.DropDownListFor(expression, items);

        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.EnumDropDownListFor(expression, null);
        }

        public static MvcHtmlString EnumOfServiceDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.EnumDropDownListFor(expression, "None");
        }

        #endregion


    }
}
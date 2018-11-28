using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.AjaxPager
{
    public static class DisplayNameExtensions
    {
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<AsiatekPagedList<TModel>> html, Expression<Func<TModel, TValue>> expression)
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

        public static MvcHtmlString DisplayNameFor<T1, T2, TValue>(this HtmlHelper<SearchDataWithPagedDatas<T1, T2>> html, Expression<Func<T2, TValue>> expression)
        {

            ModelMetadata mmd = ModelMetadata.FromLambdaExpression<T2, TValue>(expression, new ViewDataDictionary<T2>());
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

    }
}

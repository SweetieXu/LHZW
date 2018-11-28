using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;


namespace Asiatek.AjaxPager
{
    public static class PagerExtensions
    {
        #region 旧版本
        public static MvcHtmlString AsiatekAjaxPager(this AjaxHelper ajaxHelper, IAsiatekPagedList pageList, Func<int, AjaxOptions> pageAjaxOptions)
        {
            int pageIndex = pageList.CurrentPageIndex;//当前页索引
            int pageCount = pageList.TotalPageCount;//总页数

            string voidStr = "javascript:void(0)";

            TagBuilder asiatekPagerDiv = new TagBuilder("div");//最外层DIV
            asiatekPagerDiv.GenerateId("asiatekPager");


            TagBuilder asiatekPagerBackDiv = new TagBuilder("div");//第一页
            asiatekPagerBackDiv.GenerateId("asiatekPagerBack");
            TagBuilder backLink = new TagBuilder("a");
            backLink.MergeAttribute("href", voidStr);
            backLink.SetInnerText("||<");
            backLink.AddCssClass("asiatekPagerDefault");
            if (pageCount != 1)
            {
                backLink.MergeAttributes(pageAjaxOptions(1).ToUnobtrusiveHtmlAttributes());
            }
            asiatekPagerBackDiv.InnerHtml = backLink.ToString();


            TagBuilder asiatekPagerFrontDiv = new TagBuilder("div");//末页
            asiatekPagerFrontDiv.GenerateId("asiatekPagerFront");
            TagBuilder frontLink = new TagBuilder("a");
            frontLink.MergeAttribute("href", voidStr);
            frontLink.SetInnerText(">||");
            frontLink.AddCssClass("asiatekPagerDefault");
            if (pageCount != 1)
            {
                frontLink.MergeAttributes(pageAjaxOptions(pageCount).ToUnobtrusiveHtmlAttributes());
            }
            asiatekPagerFrontDiv.InnerHtml = frontLink.ToString();


            //内容（页选择）
            TagBuilder asiatekPagerContentDiv = new TagBuilder("div");//页内容
            asiatekPagerContentDiv.GenerateId("asiatekPagerContent");


            for (int i = 1; i <= pageCount; i++)
            {
                TagBuilder numLink = new TagBuilder("a");
                numLink.AddCssClass("asiatekPagerDefault");
                numLink.MergeAttribute("href", voidStr);
                if (i == pageIndex)//当前页添加 css
                {
                    numLink.AddCssClass("asiatekPagerSelected");
                }
                if (pageCount != 1)//如果总页数超过1，那么才对 页链接创建非注入式脚本，否则不添加
                {
                    numLink.MergeAttributes(pageAjaxOptions(i).ToUnobtrusiveHtmlAttributes());
                }
                numLink.SetInnerText(i.ToString());
                asiatekPagerContentDiv.InnerHtml += numLink.ToString();
            }
            int contentWidth = 250;//页数内容容器默认宽度250px
            int pagerWidth = 320;//整个分页导航总宽度默认320px
            if (pageCount < 10)//最多同时显示10页，当少于10页时，动态计算整体宽度，每个页链接占用25px，第一页与末页链接各占用35px
            {
                contentWidth = pageCount * 25;
                pagerWidth = 70 + contentWidth;
            }
            //设置容器宽度
            asiatekPagerContentDiv.MergeAttribute("style", "width:" + contentWidth + "px;");
            asiatekPagerDiv.MergeAttribute("style", "width:" + pagerWidth + "px;");

            //合并全部HTML
            asiatekPagerDiv.InnerHtml = asiatekPagerBackDiv.ToString() + asiatekPagerContentDiv.ToString() + asiatekPagerFrontDiv.ToString();
            return MvcHtmlString.Create(asiatekPagerDiv.ToString());


        }

        public static MvcHtmlString AsiatekAjaxPager(this AjaxHelper ajaxHelper, IAsiatekPagedList pageList, AsiatekAjaxPagerOptions3 asiatekAjaxPagerOptions)
        {
            if (string.IsNullOrWhiteSpace(asiatekAjaxPagerOptions.SearchPageFieldName))
            {
                asiatekAjaxPagerOptions.SearchPageFieldName = "SearchPage";
            }

            //利用请求上下文对象创建url对象，用于分页超链接的创建
            UrlHelper URL = new UrlHelper(ajaxHelper.ViewContext.RequestContext);


            #region 配置路由参数值
            //反射查询参数对象，将系统类型的属性值添加到路由参数值中，属性名是路由参数的key
            //之所以这样做，是因为对于分页查询所需要的参数肯定都是简单类型
            //比如视图采用模型StudentSetting，StudentSetting包含下拉列表Grades年级，以及对应的值GradeID
            //对于我们的查询而言，需要的只是GradeID，而不是Grades下拉列表属性
            Type type = asiatekAjaxPagerOptions.SearchDatasObj.GetType();
            var props = type.GetProperties().Where(p => p.PropertyType.ToString().StartsWith("System.") && !p.PropertyType.IsGenericType);
            RouteValueDictionary rvd = new RouteValueDictionary();
            foreach (PropertyInfo p in props)
            {
                rvd.Add(p.Name, p.GetValue(asiatekAjaxPagerOptions.SearchDatasObj, null));
            }

            if (!rvd.ContainsKey(asiatekAjaxPagerOptions.SearchPageFieldName))
            {
                //允许自定义分页查询  当前页字段名 比如currentPage
                //相当于往请求参数中附加了currentPage=1
                rvd.Add(asiatekAjaxPagerOptions.SearchPageFieldName, 1);
            }
            else
            {
                rvd[asiatekAjaxPagerOptions.SearchPageFieldName] = 1;
            }
            //查询操作所属区域名
            rvd.Add("area", asiatekAjaxPagerOptions.AreaName);
            #endregion




            int pageIndex = pageList.CurrentPageIndex;//当前页索引
            int pageCount = pageList.TotalPageCount;//总页数

            string voidStr = "javascript:void(0)";

            TagBuilder asiatekPagerDiv = new TagBuilder("div");//最外层DIV
            asiatekPagerDiv.GenerateId("asiatekPager");


            TagBuilder asiatekPagerBackDiv = new TagBuilder("div");//第一页
            asiatekPagerBackDiv.GenerateId("asiatekPagerBack");
            TagBuilder backLink = new TagBuilder("a");
            backLink.MergeAttribute("href", voidStr);
            backLink.SetInnerText("||<");
            backLink.AddCssClass("asiatekPagerDefault");
            if (pageCount != 1)//只有当总页数不为1时，才需要为返回第一页的html标签设置地址
            {
                asiatekAjaxPagerOptions.Url = URL.Action(asiatekAjaxPagerOptions.ActionName, asiatekAjaxPagerOptions.ControllerName, rvd);
                backLink.MergeAttributes(asiatekAjaxPagerOptions.ToUnobtrusiveHtmlAttributes());
            }
            asiatekPagerBackDiv.InnerHtml = backLink.ToString();


            TagBuilder asiatekPagerFrontDiv = new TagBuilder("div");//末页
            asiatekPagerFrontDiv.GenerateId("asiatekPagerFront");
            TagBuilder frontLink = new TagBuilder("a");
            frontLink.MergeAttribute("href", voidStr);
            frontLink.SetInnerText(">||");
            frontLink.AddCssClass("asiatekPagerDefault");
            if (pageCount != 1)//只有当总页数不为1时，才需要为末页的html标签设置地址
            {
                rvd[asiatekAjaxPagerOptions.SearchPageFieldName] = pageCount;
                asiatekAjaxPagerOptions.Url = URL.Action(asiatekAjaxPagerOptions.ActionName, asiatekAjaxPagerOptions.ControllerName, rvd);
                frontLink.MergeAttributes(asiatekAjaxPagerOptions.ToUnobtrusiveHtmlAttributes());
            }
            asiatekPagerFrontDiv.InnerHtml = frontLink.ToString();


            //内容（页选择）
            TagBuilder asiatekPagerContentDiv = new TagBuilder("div");//页内容
            asiatekPagerContentDiv.GenerateId("asiatekPagerContent");


            for (int i = 1; i <= pageCount; i++)
            {
                TagBuilder numLink = new TagBuilder("a");
                numLink.AddCssClass("asiatekPagerDefault");
                numLink.MergeAttribute("href", voidStr);
                if (i == pageIndex)//当前页添加 css
                {
                    numLink.AddCssClass("asiatekPagerSelected");
                }
                if (pageCount != 1)//如果总页数超过1，那么才对 页链接创建非注入式脚本，否则不添加
                {
                    rvd[asiatekAjaxPagerOptions.SearchPageFieldName] = i;
                    asiatekAjaxPagerOptions.Url = URL.Action(asiatekAjaxPagerOptions.ActionName, asiatekAjaxPagerOptions.ControllerName, rvd);
                    numLink.MergeAttributes(asiatekAjaxPagerOptions.ToUnobtrusiveHtmlAttributes());
                }
                numLink.SetInnerText(i.ToString());
                asiatekPagerContentDiv.InnerHtml += numLink.ToString();
            }
            int contentWidth = 250;//页数内容容器默认宽度250px
            int pagerWidth = 320;//整个分页导航总宽度默认320px
            if (pageCount < 10)//最多同时显示10页，当少于10页时，动态计算整体宽度，每个页链接占用25px，第一页与末页链接各占用35px
            {
                contentWidth = pageCount * 25;
                pagerWidth = 70 + contentWidth;
            }
            //设置容器宽度
            asiatekPagerContentDiv.MergeAttribute("style", "width:" + contentWidth + "px;");
            asiatekPagerDiv.MergeAttribute("style", "width:" + pagerWidth + "px;");

            //合并全部HTML
            asiatekPagerDiv.InnerHtml = asiatekPagerBackDiv.ToString() + asiatekPagerContentDiv.ToString() + asiatekPagerFrontDiv.ToString();
            return MvcHtmlString.Create(asiatekPagerDiv.ToString());


        }

        public static MvcHtmlString AsiatekAjaxPager<T1, T2>(this AjaxHelper<SearchDataWithPagedDatas<T1, T2>> ajaxHelper, AsiatekAjaxPagerOptions asiatekAjaxPagerOptions)
        {

            var searchDataWithPagedDatas = ajaxHelper.ViewData.Model;
            string searchPageFieldName = "SearchPage";

            #region 配置路由参数值
            //反射查询参数对象，将系统类型的属性值添加到路由参数值中，属性名是路由参数的key
            //之所以这样做，是因为对于分页查询所需要的参数肯定都是简单类型
            //比如视图采用模型StudentSetting，StudentSetting包含下拉列表Grades年级，以及对应的值GradeID
            //对于我们的查询而言，需要的只是GradeID，而不是Grades下拉列表属性
            Type type = searchDataWithPagedDatas.SearchModel.GetType();
            var props = type.GetProperties().Where(p => p.PropertyType.ToString().StartsWith("System.") && !p.PropertyType.IsGenericType);
            RouteValueDictionary rvd = new RouteValueDictionary();
            foreach (PropertyInfo p in props)
            {
                rvd.Add(p.Name, p.GetValue(searchDataWithPagedDatas.SearchModel, null));
            }
            rvd.Add(searchPageFieldName, 1);
            //查询操作所属区域名
            rvd.Add("area", asiatekAjaxPagerOptions.AreaName);
            #endregion



            //利用请求上下文对象创建url对象，用于分页超链接的创建
            UrlHelper URL = new UrlHelper(ajaxHelper.ViewContext.RequestContext);

            int pageIndex = searchDataWithPagedDatas.PagedDatas.CurrentPageIndex;//当前页索引
            int pageCount = searchDataWithPagedDatas.PagedDatas.TotalPageCount;//总页数

            string voidStr = "javascript:void(0)";

            TagBuilder asiatekPagerDiv = new TagBuilder("div");//最外层DIV
            asiatekPagerDiv.GenerateId("asiatekPager");


            TagBuilder asiatekPagerBackDiv = new TagBuilder("div");//第一页
            asiatekPagerBackDiv.GenerateId("asiatekPagerBack");
            TagBuilder backLink = new TagBuilder("a");
            backLink.MergeAttribute("href", voidStr);
            backLink.SetInnerText("||<");
            backLink.AddCssClass("asiatekPagerDefault");
            if (pageCount != 1)//只有当总页数不为1时，才需要为返回第一页的html标签设置地址
            {
                asiatekAjaxPagerOptions.Url = URL.Action(asiatekAjaxPagerOptions.ActionName, asiatekAjaxPagerOptions.ControllerName, rvd);
                backLink.MergeAttributes(asiatekAjaxPagerOptions.ToUnobtrusiveHtmlAttributes());
            }
            asiatekPagerBackDiv.InnerHtml = backLink.ToString();


            TagBuilder asiatekPagerFrontDiv = new TagBuilder("div");//末页
            asiatekPagerFrontDiv.GenerateId("asiatekPagerFront");
            TagBuilder frontLink = new TagBuilder("a");
            frontLink.MergeAttribute("href", voidStr);
            frontLink.SetInnerText(">||");
            frontLink.AddCssClass("asiatekPagerDefault");
            if (pageCount != 1)//只有当总页数不为1时，才需要为末页的html标签设置地址
            {
                rvd[searchPageFieldName] = pageCount;
                asiatekAjaxPagerOptions.Url = URL.Action(asiatekAjaxPagerOptions.ActionName, asiatekAjaxPagerOptions.ControllerName, rvd);
                frontLink.MergeAttributes(asiatekAjaxPagerOptions.ToUnobtrusiveHtmlAttributes());
            }
            asiatekPagerFrontDiv.InnerHtml = frontLink.ToString();


            //内容（页选择）
            TagBuilder asiatekPagerContentDiv = new TagBuilder("div");//页内容
            asiatekPagerContentDiv.GenerateId("asiatekPagerContent");


            for (int i = 1; i <= pageCount; i++)
            {
                TagBuilder numLink = new TagBuilder("a");
                numLink.AddCssClass("asiatekPagerDefault");
                numLink.MergeAttribute("href", voidStr);
                if (i == pageIndex)//当前页添加 css
                {
                    numLink.AddCssClass("asiatekPagerSelected");
                }
                if (pageCount != 1)//如果总页数超过1，那么才对 页链接创建非注入式脚本，否则不添加
                {
                    rvd[searchPageFieldName] = i;
                    asiatekAjaxPagerOptions.Url = URL.Action(asiatekAjaxPagerOptions.ActionName, asiatekAjaxPagerOptions.ControllerName, rvd);
                    numLink.MergeAttributes(asiatekAjaxPagerOptions.ToUnobtrusiveHtmlAttributes());
                }
                numLink.SetInnerText(i.ToString());
                asiatekPagerContentDiv.InnerHtml += numLink.ToString();
            }
            int contentWidth = 250;//页数内容容器默认宽度250px
            int pagerWidth = 320;//整个分页导航总宽度默认320px
            if (pageCount < 10)//最多同时显示10页，当少于10页时，动态计算整体宽度，每个页链接占用25px，第一页与末页链接各占用35px
            {
                contentWidth = pageCount * 25;
                pagerWidth = 70 + contentWidth;
            }
            //设置容器宽度
            asiatekPagerContentDiv.MergeAttribute("style", "width:" + contentWidth + "px;");
            asiatekPagerDiv.MergeAttribute("style", "width:" + pagerWidth + "px;");

            //合并全部HTML
            asiatekPagerDiv.InnerHtml = asiatekPagerBackDiv.ToString() + asiatekPagerContentDiv.ToString() + asiatekPagerFrontDiv.ToString();
            return MvcHtmlString.Create(asiatekPagerDiv.ToString());


        }

        public static MvcHtmlString AsiatekAjaxPager<T>(this AjaxHelper<T> ajaxHelper, AsiatekAjaxPagerOptions2 asiatekAjaxPagerOptions, IAsiatekPagedList pageList)
        {
            if (string.IsNullOrWhiteSpace(asiatekAjaxPagerOptions.SearchPageFieldName))
            {
                asiatekAjaxPagerOptions.SearchPageFieldName = "SearchPage";
            }

            //利用请求上下文对象创建url对象，用于分页超链接的创建
            UrlHelper URL = new UrlHelper(ajaxHelper.ViewContext.RequestContext);


            #region 配置路由参数值
            //反射查询参数对象，将系统类型的属性值添加到路由参数值中，属性名是路由参数的key
            //之所以这样做，是因为对于分页查询所需要的参数肯定都是简单类型
            //比如视图采用模型StudentSetting，StudentSetting包含下拉列表Grades年级，以及对应的值GradeID
            //对于我们的查询而言，需要的只是GradeID，而不是Grades下拉列表属性

            var model = ajaxHelper.ViewData.Model;

            Type type = model.GetType();
            var props = type.GetProperties().Where(p => p.PropertyType.ToString().StartsWith("System.") && !p.PropertyType.IsGenericType);
            RouteValueDictionary rvd = new RouteValueDictionary();
            foreach (PropertyInfo p in props)
            {
                rvd.Add(p.Name, p.GetValue(model, null));
            }

            if (!rvd.ContainsKey(asiatekAjaxPagerOptions.SearchPageFieldName))
            {
                //允许自定义分页查询  当前页字段名 比如currentPage
                //相当于往请求参数中附加了currentPage=1
                rvd.Add(asiatekAjaxPagerOptions.SearchPageFieldName, 1);
            }
            else
            {
                rvd[asiatekAjaxPagerOptions.SearchPageFieldName] = 1;
            }
            //查询操作所属区域名
            rvd.Add("area", asiatekAjaxPagerOptions.AreaName);
            #endregion




            int pageIndex = pageList.CurrentPageIndex;//当前页索引
            int pageCount = pageList.TotalPageCount;//总页数

            string voidStr = "javascript:void(0)";

            TagBuilder asiatekPagerDiv = new TagBuilder("div");//最外层DIV
            asiatekPagerDiv.GenerateId("asiatekPager");


            TagBuilder asiatekPagerBackDiv = new TagBuilder("div");//第一页
            asiatekPagerBackDiv.GenerateId("asiatekPagerBack");
            TagBuilder backLink = new TagBuilder("a");
            backLink.MergeAttribute("href", voidStr);
            backLink.SetInnerText("||<");
            backLink.AddCssClass("asiatekPagerDefault");
            if (pageCount != 1)//只有当总页数不为1时，才需要为返回第一页的html标签设置地址
            {
                asiatekAjaxPagerOptions.Url = URL.Action(asiatekAjaxPagerOptions.ActionName, asiatekAjaxPagerOptions.ControllerName, rvd);
                backLink.MergeAttributes(asiatekAjaxPagerOptions.ToUnobtrusiveHtmlAttributes());
            }
            asiatekPagerBackDiv.InnerHtml = backLink.ToString();


            TagBuilder asiatekPagerFrontDiv = new TagBuilder("div");//末页
            asiatekPagerFrontDiv.GenerateId("asiatekPagerFront");
            TagBuilder frontLink = new TagBuilder("a");
            frontLink.MergeAttribute("href", voidStr);
            frontLink.SetInnerText(">||");
            frontLink.AddCssClass("asiatekPagerDefault");
            if (pageCount != 1)//只有当总页数不为1时，才需要为末页的html标签设置地址
            {
                rvd[asiatekAjaxPagerOptions.SearchPageFieldName] = pageCount;
                asiatekAjaxPagerOptions.Url = URL.Action(asiatekAjaxPagerOptions.ActionName, asiatekAjaxPagerOptions.ControllerName, rvd);
                frontLink.MergeAttributes(asiatekAjaxPagerOptions.ToUnobtrusiveHtmlAttributes());
            }
            asiatekPagerFrontDiv.InnerHtml = frontLink.ToString();


            //内容（页选择）
            TagBuilder asiatekPagerContentDiv = new TagBuilder("div");//页内容
            asiatekPagerContentDiv.GenerateId("asiatekPagerContent");


            for (int i = 1; i <= pageCount; i++)
            {
                TagBuilder numLink = new TagBuilder("a");
                numLink.AddCssClass("asiatekPagerDefault");
                numLink.MergeAttribute("href", voidStr);
                if (i == pageIndex)//当前页添加 css
                {
                    numLink.AddCssClass("asiatekPagerSelected");
                }
                if (pageCount != 1)//如果总页数超过1，那么才对 页链接创建非注入式脚本，否则不添加
                {
                    rvd[asiatekAjaxPagerOptions.SearchPageFieldName] = i;
                    asiatekAjaxPagerOptions.Url = URL.Action(asiatekAjaxPagerOptions.ActionName, asiatekAjaxPagerOptions.ControllerName, rvd);
                    numLink.MergeAttributes(asiatekAjaxPagerOptions.ToUnobtrusiveHtmlAttributes());
                }
                numLink.SetInnerText(i.ToString());
                asiatekPagerContentDiv.InnerHtml += numLink.ToString();
            }
            int contentWidth = 250;//页数内容容器默认宽度250px
            int pagerWidth = 320;//整个分页导航总宽度默认320px
            if (pageCount < 10)//最多同时显示10页，当少于10页时，动态计算整体宽度，每个页链接占用25px，第一页与末页链接各占用35px
            {
                contentWidth = pageCount * 25;
                pagerWidth = 70 + contentWidth;
            }
            //设置容器宽度
            asiatekPagerContentDiv.MergeAttribute("style", "width:" + contentWidth + "px;");
            asiatekPagerDiv.MergeAttribute("style", "width:" + pagerWidth + "px;");

            //合并全部HTML
            asiatekPagerDiv.InnerHtml = asiatekPagerBackDiv.ToString() + asiatekPagerContentDiv.ToString() + asiatekPagerFrontDiv.ToString();
            return MvcHtmlString.Create(asiatekPagerDiv.ToString());


        }
        #endregion



        #region Bootstrap版
        /// <summary>
        /// 生成Ajax分页导航
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="currentPage">当前页码</param>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="ajaxOptionsFunc">返回AjaxOptions类型的方法</param>
        /// <param name="apso">Ajax分页样式选项</param>
        /// <returns></returns>
        public static MvcHtmlString AsiatekAjaxPagerBootstrap(this AjaxHelper ajaxHelper, int currentPage, int totalPageCount, Func<int, AjaxOptions> ajaxOptionsFunc, AjaxPagerStyleOptions apso = null)
        {
            return CreateBootstrapPagerNav(currentPage, totalPageCount, apso, null, ajaxHelper, null, ajaxOptionsFunc);
        }
        /// <summary>
        /// <para>生成Ajax分页导航</para>
        /// <para>用于SearchDataWithPagedDatas模型</para>
        /// </summary>
        /// <typeparam name="TSearchModel">包含查询条件字段的类型</typeparam>
        /// <typeparam name="TPagedModel">分页数据对应的类型</typeparam>
        /// <param name="ajaxHelper"></param>
        /// <param name="apo">Ajax分页选项</param>
        /// <param name="apso">Ajax分页样式选项</param>
        /// <returns></returns>
        public static MvcHtmlString AsiatekAjaxPagerBootstrap<TSearchModel, TPagedModel>(this AjaxHelper<SearchDataWithPagedDatas<TSearchModel, TPagedModel>> ajaxHelper, AjaxPagerOptions apo, AjaxPagerStyleOptions apso = null)
        {

            var searchDataWithPagedDatas = ajaxHelper.ViewData.Model;
            apo.SearchPageFieldName = "SearchPage";

            #region 配置路由参数值
            //反射查询参数对象，将系统类型的属性值添加到路由参数值中，属性名是路由参数的key
            //之所以这样做，是因为对于分页查询所需要的参数肯定都是简单类型
            //比如视图采用模型StudentSetting，StudentSetting包含下拉列表Grades年级，以及对应的值GradeID
            //对于我们的查询而言，需要的只是GradeID，而不是Grades下拉列表属性
            Type type = searchDataWithPagedDatas.SearchModel.GetType();
            //var props = type.GetProperties().Where(p => p.PropertyType.ToString().StartsWith("System.") && !p.PropertyType.IsGenericType);
            //必须使用Nullable.GetUnderlyingType获取可空类型的基础类型，否者如int？类型的IsGenericType是true，因为本质是Nullable<T>类型
            var props = type.GetProperties().Where(p =>
            {
                Type safeType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                return safeType.ToString().StartsWith("System.") && !safeType.IsGenericType;
            });
            RouteValueDictionary rvd = new RouteValueDictionary();
            foreach (PropertyInfo p in props)
            {
                rvd.Add(p.Name, p.GetValue(searchDataWithPagedDatas.SearchModel, null));
            }
            rvd.Add(apo.SearchPageFieldName, 1);
            //查询操作所属区域名
            rvd.Add("area", apo.AreaName);
            #endregion

            int pageIndex = searchDataWithPagedDatas.PagedDatas.CurrentPageIndex;//当前页索引
            int pageCount = searchDataWithPagedDatas.PagedDatas.TotalPageCount;//总页数

            return CreateBootstrapPagerNav(pageIndex, pageCount, apso, apo, ajaxHelper, rvd, null);
        }

        /// <summary>
        /// <para>生成Ajax分页导航</para>
        /// </summary>
        /// <typeparam name="T">包含分页数据与分页查询条件的类型</typeparam>
        /// <param name="ajaxHelper"></param>
        /// <param name="pageList">分页结果</param>
        /// <param name="apo">Ajax分页选项</param>
        /// <param name="apso">Ajax分页样式选项</param>
        /// <returns></returns>
        public static MvcHtmlString AsiatekAjaxPagerBootstrap<T>(this AjaxHelper<T> ajaxHelper, IAsiatekPagedList pageList, AjaxPagerOptions apo, AjaxPagerStyleOptions apso = null)
        {

            #region 配置路由参数值
            //反射查询参数对象，将系统类型的属性值添加到路由参数值中，属性名是路由参数的key
            //之所以这样做，是因为对于分页查询所需要的参数肯定都是简单类型
            //比如视图采用模型StudentSetting，StudentSetting包含下拉列表Grades年级，以及对应的值GradeID
            //对于我们的查询而言，需要的只是GradeID，而不是Grades下拉列表属性
            var model = ajaxHelper.ViewData.Model;

            Type type = model.GetType();
            //必须使用Nullable.GetUnderlyingType获取可空类型的基础类型，否者如int？类型的IsGenericType是true，因为本质是Nullable<T>类型
            var props = type.GetProperties().Where(p =>
            {
                Type safeType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                return safeType.ToString().StartsWith("System.") && !safeType.IsGenericType;
            });
            RouteValueDictionary rvd = new RouteValueDictionary();
            foreach (PropertyInfo p in props)
            {
                rvd.Add(p.Name, p.GetValue(model, null));
            }
            if (string.IsNullOrEmpty(apo.SearchPageFieldName))
            {
                apo.SearchPageFieldName = "SearchPage";
            }

            if (!rvd.ContainsKey(apo.SearchPageFieldName))
            {
                //允许自定义分页查询  当前页字段名 
                //相当于往请求参数中附加了SearchPage=1
                rvd.Add(apo.SearchPageFieldName, 1);
            }
            else
            {
                rvd[apo.SearchPageFieldName] = 1;
            }
            //查询操作所属区域名
            rvd.Add("area", apo.AreaName);
            #endregion


            return CreateBootstrapPagerNav(pageList.CurrentPageIndex, pageList.TotalPageCount, apso, apo, ajaxHelper, rvd, null);


        }




        /// <summary>
        /// 创建分页导航
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="totalPageCount">总页数</param>
        /// <param name="apo">ajax分页参数</param>
        /// <param name="ajaxHelper">ajax帮助类</param>
        /// <param name="rvd">路由参数</param>
        /// <returns>分页导航HTML内容</returns>
        private static MvcHtmlString CreateBootstrapPagerNav(int currentPage, int totalPageCount, AjaxPagerStyleOptions apso, AjaxPagerOptions apo, AjaxHelper ajaxHelper, RouteValueDictionary rvd, Func<int, AjaxOptions> aoFunc)
        {
            UrlHelper urlHelper = new UrlHelper(ajaxHelper.ViewContext.RequestContext);
            string voidStr = "javascript:void(0)";
            if (apso == null)
            {
                apso = new AjaxPagerStyleOptions()
                {
                    PaginationStyle = BootstrapPaginationStyleEnum.Middle,
                    NavShowCount = 10
                };
            }

            #region 外层
            TagBuilder nav = new TagBuilder("nav");
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");
            if (apso.PaginationStyle == BootstrapPaginationStyleEnum.Small)
            {
                ul.AddCssClass("pagination-sm");
            }
            else if (apso.PaginationStyle == BootstrapPaginationStyleEnum.Large)
            {
                ul.AddCssClass("pagination-lg");
            }
            #endregion


            #region 首页
            TagBuilder first = new TagBuilder("li");
            TagBuilder firstLink = new TagBuilder("a");
            firstLink.SetInnerText(HttpUtility.HtmlDecode("&laquo;"));

            firstLink.MergeAttribute("href", voidStr);
            if (totalPageCount == 1 || currentPage == 1)//只有一页数据或者当前就是第一页的话，那么禁用返回第一页的按钮
            {
                first.AddCssClass("disabled");
            }
            else
            {
                if (aoFunc != null)
                {
                    firstLink.MergeAttributes(aoFunc(1).ToUnobtrusiveHtmlAttributes());
                }
                else
                {
                    apo.Url = urlHelper.Action(apo.ActionName, apo.ControllerName, rvd);
                    firstLink.MergeAttributes(apo.ToUnobtrusiveHtmlAttributes());
                }
            }
            first.InnerHtml = firstLink.ToString();
            #endregion

            #region 末页
            TagBuilder last = new TagBuilder("li");
            TagBuilder lastLink = new TagBuilder("a");
            lastLink.SetInnerText(HttpUtility.HtmlDecode("&raquo;"));
            lastLink.MergeAttribute("href", voidStr);
            if (totalPageCount == 1 || currentPage == totalPageCount)//只有一页数据或者当前已经是最后一页的话，那么禁用最后一页的按钮
            {
                last.AddCssClass("disabled");
            }
            else
            {
                if (aoFunc != null)
                {
                    lastLink.MergeAttributes(aoFunc(totalPageCount).ToUnobtrusiveHtmlAttributes());
                }
                else
                {
                    rvd[apo.SearchPageFieldName] = totalPageCount;
                    apo.Url = urlHelper.Action(apo.ActionName, apo.ControllerName, rvd);
                    lastLink.MergeAttributes(apo.ToUnobtrusiveHtmlAttributes());
                }
            }
            last.InnerHtml = lastLink.ToString();
            #endregion





            #region 中间页码
            //假设一共有22页
            //分页导航条一次最多显示多少页
            int navMaxCount = apso.NavShowCount <= 2 ? 10 : apso.NavShowCount;
            if (totalPageCount < navMaxCount)
            {
                navMaxCount = totalPageCount;
            }
            //中间值 （用于生成每个区段的中间值，区段中间值用于计算点击页码后生成的开始页码和结束页码）
            //中间参考值为5
            int midFlag = (int)Math.Ceiling(navMaxCount * 1.0 / 2);
            //计算区段   比如每次10页的话 那么应该是3个区段 1-10 11-20 21-30
            int blockCount = totalPageCount / navMaxCount;
            if (totalPageCount % navMaxCount != 0)//无法被整除，区块增加1个
            {
                blockCount++;
            }
            List<BlockParam> blockParams = new List<BlockParam>();
            for (int i = 1; i <= blockCount; i++)
            {


                /**
                 * 第一个区段中间位置参考值 (1-1)*10+5   5 
                    第二个区段中间位置参考值 (2-1)*10+5   15
                 * 以此类推
                 */
                int middleValue = (i - 1) * navMaxCount + midFlag;
                /**
               * 第一个区段开始位置参考值  10*(1-1)+1   1
                   第二个区段开始位置参考值  10*(2-1)+1   11
               * 以此类推
               */
                int startValue = navMaxCount * (i - 1) + 1;
                /**
                 * 第一个区段结束位置参考值  10
                     第二个区段结束位置参考值  20
                 * 以此类推
                 */
                int endValue = navMaxCount * i;

                blockParams.Add(new BlockParam()
                {
                    EndNum = endValue,
                    MiddleNum = middleValue,
                    StartNum = startValue
                });
            }

            int startPage = 0;//开始页码
            int endPage = 0;//结束页码
            if (currentPage == 1)//当前页是第一页
            {
                //如果当前页是第一页，那么生成的分页导航开始页码就是1，结束页码是总页数或分页导航单次最大页数
                startPage = 1;
                //endPage = totalPageCount < navMaxCount ? totalPageCount : navMaxCount;
                endPage = navMaxCount;
            }
            else if (currentPage == totalPageCount)//最后一页
            {
                //结束页码就是当前页码，开始页码是当前页码-导航最大页码+1
                endPage = currentPage;
                startPage = currentPage - navMaxCount + 1;
            }
            else
            {
                #region 中间某一页
                //中间的某一页
                int blockIndex = currentPage / navMaxCount;//获取区段索引
                var currentBlock = blockParams[blockIndex];
                startPage = currentBlock.StartNum;
                endPage = currentBlock.EndNum;
                int currentBlockMidVal = currentBlock.MiddleNum;

                if (currentPage <= currentBlockMidVal)
                {
                    #region 小于等于中间值
                    int offset = Math.Abs(currentPage - currentBlockMidVal);
                    int newEnd = endPage - offset;
                    if (newEnd > totalPageCount)
                    {
                        endPage = totalPageCount;
                        startPage = totalPageCount - navMaxCount + 1;
                    }
                    //特殊情况：比如导航要显示10页，点了第2页，并且总页数是大于等于导航页数的，当计算出来的结束页码小于应该显示的页数时，
                    //结束页应该是导航页码的最大值，而不是根据偏移量算出来的值
                    else if (newEnd < navMaxCount && totalPageCount >= navMaxCount)
                    {
                        endPage = navMaxCount;
                        startPage = 1;
                    }
                    else
                    {
                        startPage -= offset;
                        endPage -= offset;
                    }
                    #endregion
                }
                else
                {
                    #region 大于中间值
                    int offset = Math.Abs(currentPage - currentBlockMidVal);
                    int newEnd = endPage + offset;
                    if (newEnd > totalPageCount)
                    {
                        endPage = totalPageCount;
                        startPage = totalPageCount - navMaxCount + 1;
                    }
                    else
                    {
                        startPage += offset;
                        endPage += offset;
                    }
                    #endregion
                }
                #endregion
            }


            #region  生成
            //修正页码
            if (startPage <= 0)
            {
                startPage = 1;
            }
            if (endPage > totalPageCount)
            {
                endPage = totalPageCount;
            }
            string contentStr = string.Empty;
            for (int i = startPage; i <= endPage; i++)
            {
                TagBuilder numLI = new TagBuilder("li");
                TagBuilder numLink = new TagBuilder("a");
                numLink.MergeAttribute("href", voidStr);
                if (i == currentPage)//当前页添加 css
                {
                    numLI.AddCssClass("active");
                }
                if (totalPageCount != 1)//如果总页数超过1，那么才对 页链接创建非注入式脚本，否则不添加
                {
                    if (aoFunc != null)
                    {
                        numLink.MergeAttributes(aoFunc(i).ToUnobtrusiveHtmlAttributes());
                    }
                    else
                    {
                        rvd[apo.SearchPageFieldName] = i;
                        apo.Url = urlHelper.Action(apo.ActionName, apo.ControllerName, rvd);
                        numLink.MergeAttributes(apo.ToUnobtrusiveHtmlAttributes());
                    }

                }
                numLink.SetInnerText(i.ToString());
                numLI.InnerHtml = numLink.ToString();
                contentStr += numLI.ToString();
            }
            #endregion
            #endregion



            //合并
            ul.InnerHtml += first.ToString() + contentStr + last.ToString();
            nav.InnerHtml = ul.ToString();
            return MvcHtmlString.Create(nav.ToString());
        }
        #endregion



    }

    /// <summary>
    /// 分页区段参数
    /// </summary>
    class BlockParam
    {
        /// <summary>
        /// 区段开始页码
        /// </summary>
        public int StartNum { get; set; }
        /// <summary>
        /// 区段结束页码
        /// </summary>
        public int EndNum { get; set; }
        /// <summary>
        /// 区段中间页码
        /// </summary>
        public int MiddleNum { get; set; }
    }
}

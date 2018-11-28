using System.Web;
using System.Web.Optimization;

namespace Asiatek.TMS
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //使用CDN，加载bootstrap
            bundles.UseCdn = true;

            #region 脚本
            //var bootstrapScriptCdnPath = "http://cdn.bootcss.com/bootstrap/3.3.0/js/bootstrap.min.js";
            var bootstrapScriptCdnPath = "../../Scripts/bootstrap/bootstrap_3.3.0.min.js";

            bundles.Add(new ScriptBundle("~/asiatek/bootstrap",
                        bootstrapScriptCdnPath));


            // bootstrap组件以及解决bootstrap的js组件与其他组件命名空间冲突的文件
            bundles.Add(new ScriptBundle("~/asiatek/fixbootstrap").Include(
            "~/Scripts/bootstrap/FixBootstrapConflict*"));



            //jquery
            bundles.Add(new ScriptBundle("~/asiatek/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            //jquery-ui
            bundles.Add(new ScriptBundle("~/asiatek/jqueryui").Include(
                        "~/Scripts/jqueryui/jquery-ui-{version}.js"));

            //jquery非介入式ajax脚本
            bundles.Add(new ScriptBundle("~/asiatek/jqueryub").Include(
                        "~/Scripts/jqueryub/jquery.unobtrusive*"));

            //jquery验证、非介入式验证、自定义非介入式验证
            bundles.Add(new ScriptBundle("~/asiatek/jqueryval").Include(
                      "~/Scripts/jqueryval/jquery.validate*"));


            //jquery表单
            bundles.Add(new ScriptBundle("~/asiatek/jqueryform").Include(
                      "~/Scripts/jqueryForm/jquery.form.js"));


            //亚士德jquery扩展、通用js方法
            bundles.Add(new ScriptBundle("~/asiatek/asiatekExtend").Include(
                "~/Scripts/asiatekExtend/AsiatekjQueryExtend*",
                "~/Scripts/asiatekExtend/AsiatekCommonJS*"
                ));

            //亚士德ajax分页组件所需脚本
            bundles.Add(new ScriptBundle("~/asiatek/asiatekAjaxPager").Include(
                "~/Scripts/asiatekAjaxPager/AsiatekAjaxPagerJS*"
                ));

            //可编辑的下拉框
            bundles.Add(new ScriptBundle("~/asiatek/jqueryEditableSelect").Include(
 "~/Scripts/jqueryEditableSelect/jquery-editable-select*"));




            //bootstrap-treeview
            bundles.Add(new ScriptBundle("~/asiatek/bootstraptreeview").Include(
                      "~/Scripts/bootstraptreeview/bootstrap-treeview*"));


            //初始化插件国际化内容
            bundles.Add(new ScriptBundle("~/asiatek/initplugin").Include(
    "~/Scripts/initplugin/InitPlugin*"));

            //ladda插件
            bundles.Add(new ScriptBundle("~/asiatek/ladda").Include("~/Scripts/ladda/spin*", "~/Scripts/ladda/ladda*"));


            //初始化插件日历国际化内容
            bundles.Add(new ScriptBundle("~/asiatek/jqueryuitimepicker").Include(
                        "~/Scripts/jqueryuitimepicker/jquery-ui-timepicker-addon*"));

            #endregion


            #region 样式
            //bootstrap
            //var bootstrapStyleCdnPath = "http://cdn.bootcss.com/bootstrap/3.3.5/css/bootstrap.min.css";
            var bootstrapStyleCdnPath = "../../Content/bootstrap/bootstrap.min.css";

            bundles.Add(new StyleBundle("~/Content/bootstrap",
                        bootstrapStyleCdnPath));


            //站点默认样式表
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            //jquery-ui样式表
            bundles.Add(new StyleBundle("~/Content/jqueryui/css").IncludeDirectory("~/Content/jqueryui/", "*.css"));

            //jqueryeditableselect样式
            bundles.Add(new StyleBundle("~/Content/jqEditableSelect/css").Include(
    "~/Content/jqEditableSelect/jquery-editable-select.min.css"));


            //ladda插件
            bundles.Add(new StyleBundle("~/Content/ladda/css").IncludeDirectory("~/Content/ladda/", "*.css"));
            #endregion




            //bundles.Add(new StyleBundle("~/Content/themes/base/css").IncludeDirectory("~/Content/themes/base", "*.css"));

            //亚士德ajax分页组件样式表
            //bundles.Add(new StyleBundle("~/Content/pagerCss").Include("~/Content/AsiatekAjaxPagerStyles.css"));

            //亚士德ajax分页组件脚本
            //bundles.Add(new ScriptBundle("~/asiatek/pagerJS").Include("~/Scripts/AsiatekAjaxPagerJS.js"));


        }
    }
}
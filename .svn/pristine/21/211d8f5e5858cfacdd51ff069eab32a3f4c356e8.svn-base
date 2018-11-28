using System.Web.Mvc;

namespace Asiatek.TMS.Areas.ReportManage
{
    public class ReportManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ReportManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ReportManage_default",
                "ReportManage/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

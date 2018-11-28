using System.Web.Mvc;

namespace Asiatek.TMS.Areas.HistoricalRoute
{
    public class HistoricalRouteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HistoricalRoute";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HistoricalRoute_default",
                "HistoricalRoute/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

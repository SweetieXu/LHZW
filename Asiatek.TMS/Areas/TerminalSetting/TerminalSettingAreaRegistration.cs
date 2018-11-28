using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting
{
    public class TerminalSettingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TerminalSetting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TerminalSetting_default",
                "TerminalSetting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

using System.Web.Mvc;

namespace Asiatek.TMS.Areas.PersonalSetting
{
    public class PersonalSettingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PersonalSetting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PersonalSetting_default",
                "PersonalSetting/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

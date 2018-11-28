using Asiatek.TMS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = GetCurrentViewName();
            return View(GetCurrentSubFunctionsInfo());
        }
    }
}
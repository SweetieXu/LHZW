using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.Caching;

namespace Asiatek.TMS.Controllers
{
    public class FunctionsController : BaseController
    {
        /// <summary>
        /// 获取导航菜单
        /// </summary>
        [ChildActionOnly, AsiatekPassPremission]
        public ActionResult GetMenus()
        {
            var query = GetUserSession().Functions.Where(p => p.ParentID == null).OrderBy(f => f.OrderIndex);
            ViewBag.CurrentUserStrucID = base.CurrentStrucID;
            return PartialView("_GetMenus", query);
        }
    }
}

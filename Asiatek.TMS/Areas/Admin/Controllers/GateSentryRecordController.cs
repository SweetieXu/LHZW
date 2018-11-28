using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class GateSentryRecordController : BaseController
    {
        //
        // GET: /Admin/GateSentryRecord/

        #region  查询
        public ActionResult GateSentryRecordSetting()
        {
            SearchDataWithPagedDatas<GateSentryRecordSearchModel, GateSentryRecordListModel> model = new SearchDataWithPagedDatas<GateSentryRecordSearchModel, GateSentryRecordListModel>();
            model.SearchModel = new GateSentryRecordSearchModel();
            model.PagedDatas = GateSentryRecordBLL.GetPagedGateSentryRecord(model.SearchModel, 1, this.PageSize, CurrentStrucID); 
            return PartialView("_GateSentryRecordSetting", model);
        }

        [AsiatekSubordinateFunction("GateSentryRecordSetting")]
        public ActionResult GetGateSentryRecord(GateSentryRecordSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<GateSentryRecordSearchModel, GateSentryRecordListModel> result = new SearchDataWithPagedDatas<GateSentryRecordSearchModel, GateSentryRecordListModel>();
            result.SearchModel = model;
            result.PagedDatas = GateSentryRecordBLL.GetPagedGateSentryRecord(result.SearchModel, searchPage, this.PageSize, CurrentStrucID);
            return PartialView("_GateSentryRecordPagedGrid", result);
        }
        #endregion

        [AsiatekSubordinateFunction("GateSentryRecordSetting")]
        public ActionResult GetVehicleByName(string name)
        {
            var list = GateSentryRecordBLL.GetVehicleByName(name);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.PlateNum, value = item.PlateNum, ID = item.VID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        

    }
}

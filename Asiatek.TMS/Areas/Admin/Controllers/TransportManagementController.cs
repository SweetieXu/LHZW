using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class TransportManagementController : BaseController
    {
        #region 查询
        public ActionResult TransportManagementSetting()
        {
            SearchDataWithPagedDatas<TransportManagementSearchModel, TransportManagementListModel> model =
                new SearchDataWithPagedDatas<TransportManagementSearchModel, TransportManagementListModel>();
            model.SearchModel = new TransportManagementSearchModel();

            model.PagedDatas = TransportManagementBLL.GetPagedTransportManagement(model.SearchModel, 1, this.PageSize);
            return PartialView("_TransportManagementSetting", model);
        }

        [AsiatekSubordinateFunction("TransportManagementSetting")]
        public ActionResult GetTransportManagement(TransportManagementSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<TransportManagementSearchModel, TransportManagementListModel> result =
              new SearchDataWithPagedDatas<TransportManagementSearchModel, TransportManagementListModel>();
            result.SearchModel = model;
            result.PagedDatas = TransportManagementBLL.GetPagedTransportManagement(model, searchPage, this.PageSize);
            return PartialView("_TransportManagementPagedGrid", result);
        }

        #endregion
       

    }
}

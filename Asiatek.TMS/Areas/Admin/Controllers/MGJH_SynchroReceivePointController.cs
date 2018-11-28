using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Resource;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class MGJH_SynchroReceivePointController : BaseController
    {
        //
        // GET: /Admin/MGJH_SynchroReceivePoint/

        #region  查询
        public ActionResult SynchroReceivePointSetting()
        {
            SearchDataWithPagedDatas<MGJH_SynchroReceivePointSearchModel, MGJH_SynchroReceivePointListModel> model = new SearchDataWithPagedDatas<MGJH_SynchroReceivePointSearchModel, MGJH_SynchroReceivePointListModel>();
            model.SearchModel = new MGJH_SynchroReceivePointSearchModel();
            model.PagedDatas = MGJH_TransportPointBLL.GetPagedSynchroReceivePoints(model.SearchModel, 1, this.PageSize);
            return PartialView("_SynchroReceivePointSetting", model);
        }

        [AsiatekSubordinateFunction("SynchroReceivePointSetting")]
        public ActionResult GetSynchroReceivePoint(MGJH_SynchroReceivePointSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<MGJH_SynchroReceivePointSearchModel, MGJH_SynchroReceivePointListModel> result = new SearchDataWithPagedDatas<MGJH_SynchroReceivePointSearchModel, MGJH_SynchroReceivePointListModel>();
            result.SearchModel = model;
            result.PagedDatas = MGJH_TransportPointBLL.GetPagedSynchroReceivePoints(result.SearchModel, searchPage, this.PageSize);
            return PartialView("_SynchroReceivePointPagedGrid", result);
        }
        #endregion


        #region 同步收货地址
        [AsiatekSubordinateFunction("SynchroReceivePointSetting")]
        public ActionResult EditSynchroReceivePoint(int id)
        {
            var result = MGJH_TransportPointBLL.GetSynchroReceivePointByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            //在查询出的收货地址前面加上一条记录--表示无上级收货地址
            List<SuperiorAddressModel> superiorAddress = new List<SuperiorAddressModel>();
            superiorAddress.Add(new SuperiorAddressModel() { ID = -1, AddressName = @UIText.Noting });
            superiorAddress.AddRange(MGJH_TransportPointBLL.GetAddSuperiorAddress());
            model.SuperiorAddressSelectList = new SelectList(superiorAddress, "ID", "AddressName");
            return PartialView("_EditSynchroReceivePoint", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SynchroReceivePointSetting")]
        public ActionResult EditSynchroReceivePoint(EditSynchroReceivePointModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MGJH_TransportPointBLL.EditSynchroReceivePoint(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "SynchroReceiveID:" + model.ID);
                return Json(result);
            }
            else
            {
                //在查询出的收货地址前面加上一条记录--表示无上级收货地址
                List<SuperiorAddressModel> superiorAddress = new List<SuperiorAddressModel>();
                superiorAddress.Add(new SuperiorAddressModel() { ID = -1, AddressName = @UIText.Noting });
                superiorAddress.AddRange(MGJH_TransportPointBLL.GetAddSuperiorAddress());
                model.SuperiorAddressSelectList = new SelectList(superiorAddress, "ID", "AddressName");
                return PartialView("_EditSynchroReceivePoint", model);
            }
        }

        #endregion
    }
}

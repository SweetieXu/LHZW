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
    public class CLCS_ReturnOrderManageController : BaseController
    {
        //
        // GET: /Admin/CLCS_ReturnOrderManage/

        #region 查询
        public ActionResult CLCS_ReturnOrderManageSetting()
        {
            SearchDataWithPagedDatas<CLCS_ReturnOrderSearchModel, CLCS_ReturnOrderListModel> model = new SearchDataWithPagedDatas<CLCS_ReturnOrderSearchModel, CLCS_ReturnOrderListModel>();
            model.SearchModel = new CLCS_ReturnOrderSearchModel();
            model.PagedDatas = CLCS_ReturnOrderManageBLL.GetPagedCLCS_ReturnOrder(model.SearchModel, 1, this.PageSize, base.CurrentStrucID);
            return PartialView("CLCS_ReturnOrderSetting", model);
        }

        [AsiatekSubordinateFunction("CLCS_ReturnOrderManageSetting")]
        public ActionResult GetCLCS_ReturnOrder(CLCS_ReturnOrderSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<CLCS_ReturnOrderSearchModel, CLCS_ReturnOrderListModel> result = new SearchDataWithPagedDatas<CLCS_ReturnOrderSearchModel, CLCS_ReturnOrderListModel>();
            result.SearchModel = model;
            result.PagedDatas = CLCS_ReturnOrderManageBLL.GetPagedCLCS_ReturnOrder(result.SearchModel, searchPage, this.PageSize, base.CurrentStrucID);
            return PartialView("CLCS_ReturnOrderPagedGrid", result);
        } 
        #endregion


        #region 新增
        [AsiatekSubordinateFunction("CLCS_ReturnOrderManageSetting")]
        public ActionResult AddCLCS_ReturnOrder()
        {
            AddCLCS_ReturnOrderModel model = new AddCLCS_ReturnOrderModel();
            //model.StartTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
           // model.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return PartialView("AddCLCS_ReturnOrder", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("CLCS_ReturnOrderManageSetting")]
        public ActionResult AddCLCS_ReturnOrder(AddCLCS_ReturnOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var result = CLCS_ReturnOrderManageBLL.AddCLCS_ReturnOrder(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Add, result, "CLCS_ReturnOrderPlateNum:" + model.PlateNum);
                return Json(result);
            }
            else
            {
                model.StartTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                model.EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                return PartialView("AddCLCS_ReturnOrder", model);
            }
        }
        #endregion

    }
}

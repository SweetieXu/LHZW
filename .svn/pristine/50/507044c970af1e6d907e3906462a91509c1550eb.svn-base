using Asiatek.TMS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.AjaxPager;
using Asiatek.Model;
using Asiatek.BLL.MSSQL;
using Asiatek.TMS.Filters;
using Asiatek.Resource;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class ReceiverMailInfoController : BaseController
    {

        #region   查询
        public ActionResult SearchReceiverMailInfo()
        {
            SearchDataWithPagedDatas<ReceiverMailInfoSearchModel, ReceiverMailInfoListModel> model =
    new SearchDataWithPagedDatas<ReceiverMailInfoSearchModel, ReceiverMailInfoListModel>();
            model.SearchModel = new ReceiverMailInfoSearchModel();
            model.PagedDatas = ReceiverMailInfoBLL.GetPagedReceiverMailInfo(model.SearchModel, 1, this.PageSize);
            return PartialView("_SearchReceiverMailInfoList", model);
        }

        [AsiatekSubordinateFunction("SearchReceiverMailInfo")]
        public ActionResult GetReceiverMailInfo(ReceiverMailInfoSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<ReceiverMailInfoSearchModel, ReceiverMailInfoListModel> result =
       new SearchDataWithPagedDatas<ReceiverMailInfoSearchModel, ReceiverMailInfoListModel>();
            result.SearchModel = model;
            result.PagedDatas = ReceiverMailInfoBLL.GetPagedReceiverMailInfo(model, searchPage, this.PageSize);
            return PartialView("_ReceiverMailInfoPagedGrid", result);
        }
        #endregion

        #region   新增
        [AsiatekSubordinateFunction("SearchReceiverMailInfo")]
        public ActionResult AddReceiverMailInfo()
        {
            ReceiverMailInfoFormModel model = new ReceiverMailInfoFormModel();
            model.Status = true;
            return PartialView("_AddReceiverMailInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SearchReceiverMailInfo")]
        public ActionResult AddReceiverMailInfo(ReceiverMailInfoFormModel model)
        {
            OperationResult result;
            if (ModelState.IsValid)
            {
                result = ReceiverMailInfoBLL.AddReceiverMailInfo(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "Email:" + model.Email);
                return Json(result);
            }
            else
            {
                return PartialView("_AddReceiverMailInfo", model);
            }
        }


        #endregion

        #region   编辑
        [AsiatekSubordinateFunction("SearchReceiverMailInfo")]
        public ActionResult EditReceiverMailInfo(int id)
        {
            SelectResult<ReceiverMailInfoFormModel> result = ReceiverMailInfoBLL.GetReceiverMailInfo(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }

            ReceiverMailInfoFormModel model = result.DataResult;
            return PartialView("_EditReceiverMailInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SearchReceiverMailInfo")]
        public ActionResult EditReceiverMailInfo(ReceiverMailInfoFormModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ReceiverMailInfoBLL.EditReceiverMailInfo(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditReceiverMailInfo", model);
            }
        }
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SearchReceiverMailInfo")]
        public ActionResult DeleteReceiverMailInfo(FormCollection fc)
        {
            string[] ids = fc["tid"].Split(',');

            var result = ReceiverMailInfoBLL.DeleteReceiverMailInfo(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["tid"]);
            return Json(result);
        }

        #endregion

        #region 扩展方法
        [AsiatekSubordinateFunction("SearchReceiverMailInfo")]
        public ActionResult CheckReceiverMailInfoExists(string email, int? id)
        {
            return Json(!ReceiverMailInfoBLL.CheckReceiverMailInfoExists(email, id));
        }
        #endregion
    }
}

using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class ExceptionTypeController : BaseController
    {
        #region   查询
        public ActionResult SeachExceptionType()
        {
            SearchDataWithPagedDatas<ExceptionTypeSeachModel, ExceptionTypeListModel> model =
                new SearchDataWithPagedDatas<ExceptionTypeSeachModel, ExceptionTypeListModel>();

            model.SearchModel = new ExceptionTypeSeachModel();
            model.PagedDatas = ExceptionTypeBLL.GetPagedExceptionType(model.SearchModel, 1, this.PageSize);
            return PartialView("_ExceptionType", model);
        }

        [AsiatekSubordinateFunction("SeachExceptionType")]
        public ActionResult GetExceptionTypeInfo(ExceptionTypeSeachModel model, int searchPage)
        {
            SearchDataWithPagedDatas<ExceptionTypeSeachModel, ExceptionTypeListModel> result =
       new SearchDataWithPagedDatas<ExceptionTypeSeachModel, ExceptionTypeListModel>();
            result.SearchModel = model;
            result.PagedDatas = ExceptionTypeBLL.GetPagedExceptionType(model, searchPage, this.PageSize);
            return PartialView("_ExceptionTypePagedGrid", result);
        }
        #endregion

        #region   新增
         [AsiatekSubordinateFunction("SeachExceptionType")]
        public ActionResult AddExceptionType()
        {
            return PartialView("_AddExceptionType");
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachExceptionType")]
        public ActionResult AddExceptionType(ExceptionTypeAddModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ExceptionTypeBLL.AddExceptionType(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_AddExceptionType", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("AddExceptionType")]
        public ActionResult CheckAddExceptionIDExists(int id)
        {
            return Json(!ExceptionTypeBLL.CheckAddExceptionIDExists(id));
        }
        #endregion

        #region   编辑
         [AsiatekSubordinateFunction("SeachExceptionType")]
        public ActionResult EditExceptionType(int id)
        {
            var result = ExceptionTypeBLL.GetExceptionTypeID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditExceptionType", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachExceptionType")]
        public ActionResult EditExceptionType(ExceptionTypeEditModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ExceptionTypeBLL.EditExceptionType(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditExceptionType", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("EditExceptionType")]
        public ActionResult CheckEditExceptionIDExists(int id)
        {
            return Json(!ExceptionTypeBLL.CheckEditExceptionIDExists(id));
        }
        #endregion
    }
}

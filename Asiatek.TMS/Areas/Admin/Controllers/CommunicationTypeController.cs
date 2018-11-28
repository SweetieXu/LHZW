using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class CommunicationTypeController : BaseController
    {
        #region   查询
        public ActionResult SeachCommunicationType()
        {
            SearchDataWithPagedDatas<CommunicationTypeSeachModel, CommunicationTypeListModel> model =
                new SearchDataWithPagedDatas<CommunicationTypeSeachModel, CommunicationTypeListModel>();

            model.SearchModel = new CommunicationTypeSeachModel();
            model.PagedDatas = CommunicationTypeBLL.GetPagedCommunicationType(model.SearchModel, 1, this.PageSize);
            return PartialView("_SeachCommunicationType", model);
        }

        [AsiatekSubordinateFunction("SeachCommunicationType")]
        public ActionResult GetCommunicationTypeInfo(CommunicationTypeSeachModel model, int searchPage)
        {
            SearchDataWithPagedDatas<CommunicationTypeSeachModel, CommunicationTypeListModel> result =
                new SearchDataWithPagedDatas<CommunicationTypeSeachModel, CommunicationTypeListModel>();
            result.SearchModel = model;
            result.PagedDatas = CommunicationTypeBLL.GetPagedCommunicationType(model, searchPage, this.PageSize);
            return PartialView("_CommunicationTypeGrid", result);
        }
        #endregion

        #region   新增
        [AsiatekSubordinateFunction("SeachCommunicationType")]
        public ActionResult AddCommunicationType()
        {
            return PartialView("_AddCommunicationType");
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachCommunicationType")]
        public ActionResult AddCommunicationType(AddCommunicationTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var result = CommunicationTypeBLL.AddCommunicationType(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "Name:" + model.Name);
                return Json(result);
            }
            else
            {
                return PartialView("_AddCommunicationType", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("SeachCommunicationType")]
        public ActionResult CheckAddTypeNameExists(string SensorName)
        {
            return Json(!SensorBLL.CheckTypeNameExists(SensorName));
        }
        #endregion

        #region   编辑
        [AsiatekSubordinateFunction("SeachCommunicationType")]
        public ActionResult EditCommunicationType(int id)
        {
            var result = CommunicationTypeBLL.GetCommunicationTypeID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditCommunicationType", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachCommunicationType")]
        public ActionResult EditCommunicationType(EditCommunicationTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var result = CommunicationTypeBLL.EditCommunicationType(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditCommunicationType", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("SeachCommunicationType")]
        public ActionResult CheckEditTypeNameExists(string SensorName)
        {
            return Json(!SensorBLL.CheckTypeNameExists(SensorName));
        }
        #endregion

        #region   删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachCommunicationType")]
        public ActionResult DeleteCommunicationType(FormCollection fc)
        {
            string[] ids = fc["tid"].Split(',');
            //var result = CommunicationTypeBLL.DeleteCommunicationType(ids);
            var result = CommunicationTypeBLL.DeleteCommunicationTypePhysical(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["tid"]);
            return Json(result);
        }
        #endregion
    }
}

using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class ServiceProviderMaintainController : BaseController
    {
        #region   查询
        public ActionResult SeachServiceProvider()
        {
            var model = new SearchDataWithPagedDatas<ServiceProviderSeachModel, ServiceProviderListModel>();

            model.SearchModel = new ServiceProviderSeachModel();
            model.PagedDatas = ServiceProviderTypeBLL.GetPagedServiceProviderType(model.SearchModel, 1, this.PageSize);
            return PartialView("_SeachServiceProviderType", model);
        }

        [AsiatekSubordinateFunction("SeachServiceProvider")]
        public ActionResult GetServiceProviderInfo(ServiceProviderSeachModel model, int searchPage)
        {
            var result = new SearchDataWithPagedDatas<ServiceProviderSeachModel, ServiceProviderListModel>();
            result.SearchModel = model;
            result.PagedDatas = ServiceProviderTypeBLL.GetPagedServiceProviderType(model, searchPage, this.PageSize);
            return PartialView("_ServiceProviderGrid", result);
        }
        #endregion

        #region   新增
        [AsiatekSubordinateFunction("SeachServiceProvider")]
        public ActionResult AddServiceProvider()
        {
            return PartialView("_AddServiceProvider");
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachServiceProvider")]
        public ActionResult AddServiceProvider(AddServiceProviderModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ServiceProviderTypeBLL.AddServiceProvider(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "Name:" + model.Name);
                return Json(result);
            }
            else
            {
                return PartialView("_AddServiceProvider", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("SeachServiceProvider")]
        public ActionResult CheckAddSPNameExists(string name)
        {
            return Json(!ServiceProviderTypeBLL.CheckSPNameExists(name.Trim()));
        }
        #endregion

        #region   编辑
        [AsiatekSubordinateFunction("SeachServiceProvider")]
        public ActionResult EditServiceProvider(int id)
        {
            var result = ServiceProviderTypeBLL.GetServiceProviderByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditServiceProvider", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachServiceProvider")]
        public ActionResult EditServiceProvider(EditServiceProviderModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ServiceProviderTypeBLL.EditServiceProvider(model,base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditServiceProvider", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("SeachServiceProvider")]
        public ActionResult CheckEditSPNameExists(string name, int id)
        {
            return Json(!ServiceProviderTypeBLL.CheckSPNameExists(name.Trim(), id));
        }
        #endregion

        #region   删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SeachServiceProvider")]
        public ActionResult DeleteServiceProvider(FormCollection fc)
        {
            string[] ids = fc["tid"].Split(',');

           // var result = ServiceProviderTypeBLL.DeleteServiceProvider(ids);
            var result = ServiceProviderTypeBLL.DeleteServiceProviderPhysical(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["tid"]);
            return Json(result);
        }
        #endregion
    }
}

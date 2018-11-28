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
    public class ElectricFencePropertyController : BaseController
    {
        //
        // GET: /Admin/ElectricFenceProperty/
        #region  查询
        public ActionResult ElectricFencePropertySetting()
        {
            SearchDataWithPagedDatas<ElectricFencePropertySearchModel, ElectricFencePropertyListModel> model = new SearchDataWithPagedDatas<ElectricFencePropertySearchModel, ElectricFencePropertyListModel>();
            model.SearchModel = new ElectricFencePropertySearchModel();
            model.PagedDatas = ElectricFencePropertyBLL.GetPagedEFPropertys(model.SearchModel, 1, this.PageSize, base.CurrentStrucID);
            return PartialView("_ElectricFencePropertySetting", model);
        }

        [AsiatekSubordinateFunction("ElectricFencePropertySetting")]
        public ActionResult GetEFPropertys(ElectricFencePropertySearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<ElectricFencePropertySearchModel, ElectricFencePropertyListModel> result = new SearchDataWithPagedDatas<ElectricFencePropertySearchModel, ElectricFencePropertyListModel>();
            result.SearchModel = model;
            result.PagedDatas = ElectricFencePropertyBLL.GetPagedEFPropertys(result.SearchModel, searchPage, this.PageSize, base.CurrentStrucID);
            return PartialView("_ElectricFencePropertyPagedGrid", result);
        }
        #endregion


        #region  新增
        [AsiatekSubordinateFunction("ElectricFencePropertySetting")]
        public ActionResult AddEFProperty()
        {
            AddEFPropertyModel model = new AddEFPropertyModel();
            model.ValidStartTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            model.ValidEndTime = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd 23:59:59");
            model.FenceState = true;
            return PartialView("_AddElectricFenceProperty", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ElectricFencePropertySetting")]
        public ActionResult AddEFProperty(AddEFPropertyModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ElectricFencePropertyBLL.AddEFProperty(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Add, result, "PropertyName:" + model.PropertyName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddElectricFenceProperty", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("ElectricFencePropertySetting")]
        public ActionResult CheckAddEFPropertyNameExists(string propertyName)
        {
            return Json(!ElectricFencePropertyBLL.CheckAddEFPropertyNameExists(propertyName));
        }
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ElectricFencePropertySetting")]
        public ActionResult DeleteEFProperty(FormCollection fc)
        {
            string[] ids = fc["Fnid"].Split(',');

            var result = ElectricFencePropertyBLL.DeleteEFProperty(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["Fnid"]);
            return Json(result);
        }
        #endregion

        #region 修改
        [AsiatekSubordinateFunction("ElectricFencePropertySetting")]
        public ActionResult EditEFProperty(int id)
        {
            var result = ElectricFencePropertyBLL.GetEFPropertyByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            model.EFPropertyPeriod = ElectricFencePropertyBLL.GetEFPropertyPeriodByID(id);
            List<EFPropertyPeriodModel> tmpList = new List<EFPropertyPeriodModel>();//用tmpList将结果集按0-6顺序存储
            if (model.EFPropertyPeriod == null || model.EFPropertyPeriod.Count == 0)
            {
                for (int i = 0; i < 7; i++)
                {
                    tmpList.Add(new EFPropertyPeriodModel() { Week = i });
                }
            }
            else {
                for (int i = 0; i < 7; i++)
                {
                    if (model.EFPropertyPeriod.Where(a => a.Week == i).FirstOrDefault() == null)
                    {
                        tmpList.Add(new EFPropertyPeriodModel() { Week = i });
                    }
                    else {
                        tmpList.Add(model.EFPropertyPeriod.Where(a => a.Week == i).FirstOrDefault());
                    }
                }
            }
            model.EFPropertyPeriod = tmpList;
            return PartialView("_EditElectricFenceProperty", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ElectricFencePropertySetting")]
        public ActionResult EditEFProperty(EditEFPropertyModel model)
        {
            if (ModelState.IsValid)
            {
                var result = ElectricFencePropertyBLL.EditEFProperty(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "PropertyID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditElectricFenceProperty", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("ElectricFencePropertySetting")]
        public ActionResult CheckEditEFPropertyNameExists(string propertyName, int id)
        {
            return Json(!ElectricFencePropertyBLL.CheckEditEFPropertyNameExists(propertyName, id));
        }

        #endregion
    }
}

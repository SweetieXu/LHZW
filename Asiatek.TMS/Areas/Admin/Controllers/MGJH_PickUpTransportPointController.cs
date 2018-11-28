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
    public class MGJH_PickUpTransportPointController : BaseController
    {
        //
        // GET: /Admin/MGJH_TransportPoint/

        #region  查询
        public ActionResult PickUpTransportPointSetting()
        {
            SearchDataWithPagedDatas<MGJH_PickUpTransportPointSearchModel, MGJH_PickUpTransportPointListModel> model = new SearchDataWithPagedDatas<MGJH_PickUpTransportPointSearchModel, MGJH_PickUpTransportPointListModel>();
            model.SearchModel = new MGJH_PickUpTransportPointSearchModel();
            model.PagedDatas = MGJH_TransportPointBLL.GetPagedPickUpTransportPoints(model.SearchModel, 1, this.PageSize);
            return PartialView("_PickUpTransportPointSetting", model);
        }

        [AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult GetPickUpTransportPoint(MGJH_PickUpTransportPointSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<MGJH_PickUpTransportPointSearchModel, MGJH_PickUpTransportPointListModel> result = new SearchDataWithPagedDatas<MGJH_PickUpTransportPointSearchModel, MGJH_PickUpTransportPointListModel>();
            result.SearchModel = model;
            result.PagedDatas = MGJH_TransportPointBLL.GetPagedPickUpTransportPoints(result.SearchModel, searchPage, this.PageSize);
            return PartialView("_PickUpTransportPointPagedGrid", result);
        }
        #endregion


        #region  新增
        [AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult AddPickUpTransportPoint()
        {
            AddPickUpTransportPointModel model = new AddPickUpTransportPointModel();
            return PartialView("_AddPickUpTransportPoint", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult AddPickUpTransportPoint(AddPickUpTransportPointModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MGJH_TransportPointBLL.AddPickUpTransportPoint(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Add, result, "PickUpAddressName:" + model.AddressName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddPickUpTransportPoint", model);
            }
        }

        #region 验证
        /// <summary>
        /// 提货点名称唯一性验证
        /// </summary>
        /// <param name="addressName"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult CheckAddPickUpAddressNameExists(string addressName)
        {
            return Json(!MGJH_TransportPointBLL.CheckAddPickUpAddressNameExists(addressName));
        }

        /// <summary>
        /// 提货点编码唯一性验证
        /// </summary>
        /// <param name="addressCode"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult CheckAddPickUpAddressCodeExists(string addressCode)
        {
            return Json(!MGJH_TransportPointBLL.CheckAddPickUpAddressCodeExists(addressCode));
        }
        #endregion
        #endregion


        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult DeletePickUpTransportPoint(FormCollection fc)
        {
            string[] ids = fc["pid"].Split(',');
            var result = MGJH_TransportPointBLL.DeletePickUpTransportPoint(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["pid"]);
            return Json(result);
        }
        #endregion


        #region 修改
        [AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult EditPickUpTransportPoint(int id)
        {
            var result = MGJH_TransportPointBLL.GetPickUpTransportPointByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditPickUpTransportPoint", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult EditPickUpTransportPoint(EditPickUpTransportPointModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MGJH_TransportPointBLL.EditPickUpTransportPoint(model,base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "PickUpTransportID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditPickUpTransportPoint", model);
            }
        }

        #region 验证
        /// <summary>
        /// 提货点名称唯一性验证
        /// </summary>
        /// <param name="addressName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult CheckEditPickUpAddressNameExists(string addressName, int id)
        {
            return Json(!MGJH_TransportPointBLL.CheckEditPickUpAddressNameExists(addressName, id));
        }

        /// <summary>
        /// 提货点编码唯一性验证
        /// </summary>
        /// <param name="addressCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("PickUpTransportPointSetting")]
        public ActionResult CheckEditPickUpAddressCodeExists(string addressCode, int id)
        {
            return Json(!MGJH_TransportPointBLL.CheckEditPickUpAddressCodeExists(addressCode, id));
        }
        #endregion
        #endregion
    }
}

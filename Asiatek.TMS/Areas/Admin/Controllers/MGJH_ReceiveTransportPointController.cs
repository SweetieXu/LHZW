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
    public class MGJH_ReceiveTransportPointController : BaseController
    {
        //
        // GET: /Admin/MGJH_ReceiveTransportPoint/

        #region  查询
        public ActionResult ReceiveTransportPointSetting()
        {
            SearchDataWithPagedDatas<MGJH_ReceiveTransportPointSearchModel, MGJH_ReceiveTransportPointListModel> model = new SearchDataWithPagedDatas<MGJH_ReceiveTransportPointSearchModel, MGJH_ReceiveTransportPointListModel>();
            model.SearchModel = new MGJH_ReceiveTransportPointSearchModel();
            model.PagedDatas = MGJH_TransportPointBLL.GetPagedReceiveTransportPoints(model.SearchModel, 1, this.PageSize);
            return PartialView("_ReceiveTransportPointSetting", model);
        }

        [AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult GetReceiveTransportPoint(MGJH_ReceiveTransportPointSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<MGJH_ReceiveTransportPointSearchModel, MGJH_ReceiveTransportPointListModel> result = new SearchDataWithPagedDatas<MGJH_ReceiveTransportPointSearchModel, MGJH_ReceiveTransportPointListModel>();
            result.SearchModel = model;
            result.PagedDatas = MGJH_TransportPointBLL.GetPagedReceiveTransportPoints(result.SearchModel, searchPage, this.PageSize);
            return PartialView("_ReceiveTransportPointPagedGrid", result);
        }
        #endregion

        #region 新增
        [AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult AddReceiveTransportPoint(int? synchroReceiveAddressId)
        {
            AddReceiveTransportPointModel model = new AddReceiveTransportPointModel();
            if (synchroReceiveAddressId.HasValue)//如果是通过同步收货地址打开的新增收货地址界面，那么会有通过接口过来的待同步地址ID
            {
                var result = MGJH_TransportPointBLL.GetSynchroReceivePointByID(synchroReceiveAddressId.Value);
                if (result.DataResult == null)
                {
                    return Content(result.Message);
                }
                var tempModel = result.DataResult;
                model.AddressName = tempModel.AddressName;
                model.CustomerName = tempModel.CustomerName;
                model.AddressArea = tempModel.AddressArea;
                model.AddressCode = tempModel.AddressCode;
                model.SourceID = synchroReceiveAddressId;
            }

            //AddReceiveTransportPointModel model = new AddReceiveTransportPointModel();
            //在查询出的收货地址前面加上一条记录--表示无上级收货地址
            List<SuperiorAddressModel> superiorAddress = new List<SuperiorAddressModel>();
            superiorAddress.Add(new SuperiorAddressModel() { ID = -1, AddressName = @UIText.Noting });
            superiorAddress.AddRange(MGJH_TransportPointBLL.GetAddSuperiorAddress());
            model.SuperiorAddressSelectList = new SelectList(superiorAddress, "ID", "AddressName");
            return PartialView("_AddReceiveTransportPoint", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult AddReceiveTransportPoint(AddReceiveTransportPointModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MGJH_TransportPointBLL.AddReceiveTransportPoint(model, base.CurrentUserID);
                string temp = string.Format("ReceiveTransportPointAddressName:{0} SourceID:{1}", model.AddressName, model.SourceID);
                base.DoLog(OperationTypeEnum.Add, result, temp);
                return Json(result);
            }
            else
            {
                //在查询出的收货地址前面加上一条记录--表示无上级收货地址
                List<SuperiorAddressModel> superiorAddress = new List<SuperiorAddressModel>();
                superiorAddress.Add(new SuperiorAddressModel() { ID = -1, AddressName = @UIText.Noting });
                superiorAddress.AddRange(MGJH_TransportPointBLL.GetAddSuperiorAddress());
                model.SuperiorAddressSelectList = new SelectList(superiorAddress, "ID", "AddressName");
                return PartialView("_AddReceiveTransportPoint", model);
            }
        }

        #region 验证
        /// <summary>
        /// 收货点名称唯一性验证
        /// </summary>
        /// <param name="addressName"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult CheckAddReceiveAddressNameExists(string addressName)
        {
            return Json(!MGJH_TransportPointBLL.CheckAddReceiveAddressNameExists(addressName));
        }

        /// <summary>
        /// 收货点编码唯一性验证
        /// </summary>
        /// <param name="addressCode"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult CheckAddReceiveAddressCodeExists(string addressCode)
        {
            return Json(!MGJH_TransportPointBLL.CheckAddReceiveAddressCodeExists(addressCode));
        }
        #endregion
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult DeleteReceiveTransportPoint(FormCollection fc)
        {
            string[] ids = fc["rid"].Split(',');
            var result = MGJH_TransportPointBLL.DeleteReceiveTransportPoint(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["rid"]);
            return Json(result);
        }
        #endregion

        #region 修改
        [AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult EditReceiveTransportPoint(int id)
        {
            var result = MGJH_TransportPointBLL.GetReceiveTransportPointByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            //在查询出的收货地址前面加上一条记录--表示无上级收货地址
            List<SuperiorAddressModel> superiorAddress = new List<SuperiorAddressModel>();
            superiorAddress.Add(new SuperiorAddressModel() { ID = -1, AddressName = @UIText.Noting });
            superiorAddress.AddRange(MGJH_TransportPointBLL.GetEditSuperiorAddress(id));
            model.SuperiorAddressSelectList = new SelectList(superiorAddress, "ID", "AddressName");
            return PartialView("_EditReceiveTransportPoint", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult EditReceiveTransportPoint(EditReceiveTransportPointModel model)
        {
            if (ModelState.IsValid)
            {
                var result = MGJH_TransportPointBLL.EditReceiveTransportPoint(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "ReceiveTransportID:" + model.ID);
                return Json(result);
            }
            else
            {
                //在查询出的收货地址前面加上一条记录--表示无上级收货地址
                List<SuperiorAddressModel> superiorAddress = new List<SuperiorAddressModel>();
                superiorAddress.Add(new SuperiorAddressModel() { ID = -1, AddressName = @UIText.Noting });
                superiorAddress.AddRange(MGJH_TransportPointBLL.GetEditSuperiorAddress(model.ID));
                model.SuperiorAddressSelectList = new SelectList(superiorAddress, "ID", "AddressName");
                return PartialView("_EditReceiveTransportPoint", model);
            }
        }

        #region 验证
        /// <summary>
        /// 提货点名称唯一性验证
        /// </summary>
        /// <param name="addressName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult CheckEditReceiveAddressNameExists(string addressName, int id)
        {
            return Json(!MGJH_TransportPointBLL.CheckEditReceiveAddressNameExists(addressName, id));
        }

        /// <summary>
        /// 提货点编码唯一性验证
        /// </summary>
        /// <param name="addressCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("ReceiveTransportPointSetting")]
        public ActionResult CheckEditReceiveAddressCodeExists(string addressCode, int id)
        {
            return Json(!MGJH_TransportPointBLL.CheckEditReceiveAddressCodeExists(addressCode, id));
        }
        #endregion
        #endregion
    }
}

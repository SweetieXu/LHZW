using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class RepairRecordController : BaseController
    {
        #region   查询
        public ActionResult RepairRecordDetail()
        {
            SearchDataWithPagedDatas<RepairRecordSearchModel, RepairRecordModel> model =
                new SearchDataWithPagedDatas<RepairRecordSearchModel, RepairRecordModel>();

            model.SearchModel = new RepairRecordSearchModel();
            model.PagedDatas = RepairRecordBLL.GetPagedRepairRecord(model.SearchModel, 1, this.PageSize,this.CurrentStrucID);
            return PartialView("_RepairRecordDetail", model);
        }

        [AsiatekSubordinateFunction("RepairRecordDetail")]
        public ActionResult GetRepairRecordInfo(RepairRecordSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<RepairRecordSearchModel, RepairRecordModel> result =
       new SearchDataWithPagedDatas<RepairRecordSearchModel, RepairRecordModel>();
            result.SearchModel = model;
            //时间搜索设置不正确直接返回count为0
                result.PagedDatas = RepairRecordBLL.GetPagedRepairRecord(model, searchPage, this.PageSize, this.CurrentStrucID);
            return PartialView("_RepairRecordPageGrid", result);
        }
        #endregion

        #region   新增
        [AsiatekSubordinateFunction("RepairRecordDetail")]
        public ActionResult AddRepairRecord()
        {
            return PartialView("_RepairRecordAdd");
        }
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult AddRecord(RepairRecordModel model)
        {
                var result = RepairRecordBLL.AddRepairRecord(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "PlateNum:" + model.PlateNum);
                return Json(result);
        }
        #endregion

        #region   编辑
        [AsiatekSubordinateFunction("RepairRecordDetail")]
        public ActionResult EditRepairRecord(int id)
        {
            var result = RepairRecordBLL.GetRecordByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_RepairRecordEdit", model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditRecord(RepairRecordModel model)
        {
            var result = RepairRecordBLL.EditRepairRecord(model, base.UserIDForLog);
            base.DoLog(OperationTypeEnum.Edit, result, "PlateNum:" + model.PlateNum);
            return Json(result);
        }

        #endregion
        #region   删除
        [HttpPost]
        public ActionResult DeleteRepairRecord(RecordID fc)
        {
            var result = RepairRecordBLL.DeleteRecord(fc.ID.ToString());
            base.DoLog(OperationTypeEnum.Delete, result, fc.ID.ToString());
            return Json(result);
        }
        #endregion


        #region 审核
        [HttpPost]
        public ActionResult AuditeRepairRecord(RecordID ids)
        {
            string[] idslist = ids.idList.Split(',');
            var result = RepairRecordBLL.AuditeRecord(idslist,base.UserIDForLog);
            base.DoLog(OperationTypeEnum.Delete, result);
            return Json(result);
        }
        #endregion

        #region 详情页
        [AsiatekSubordinateFunction("RepairRecordDetail")]
        public ActionResult DetailViewRepairRecord(int id)
        {
            var result = RepairRecordBLL.GetRecordByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_RepairRecordReadOnly", model);
        }
        #endregion
        #region 根据车牌号获取车辆ID
        public ActionResult GetVehicleIDByPlateNum(string PlateNum) 
        {
            base.FixVaryBug();
            var list = VehicleBLL.GetVehiclesByPlateNum(PlateNum,this.CurrentStrucID);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { value = item.PlateNum, label = item.PlateNum, ID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

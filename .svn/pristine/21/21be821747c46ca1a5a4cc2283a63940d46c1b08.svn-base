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
    public class TemperatureAlarmRuleController : BaseController
    {

        #region 查询
        public ActionResult TemperatureAlarmRuleSetting()
        {
            var model = new SearchDataWithPagedDatas<TemperatureAlarmRuleSearchModel, TemperatureAlarmRuleListModel>();
            model.SearchModel = new TemperatureAlarmRuleSearchModel();
            model.SearchModel.AffiliatedStrucID = -1;
            model.PagedDatas = TemperatureAlarmRuleBLL.GetPagedTemperatureAlarmRules(model.SearchModel, base.CurrentStrucID, 1, this.PageSize);
            return PartialView("_TemperatureAlarmRuleSetting", model);
        }


        [AsiatekSubordinateFunction("TemperatureAlarmRuleSetting")]
        public ActionResult GetTemperatureAlarmRules(TemperatureAlarmRuleSearchModel model, int searchPage)
        {
            var result = new SearchDataWithPagedDatas<TemperatureAlarmRuleSearchModel, TemperatureAlarmRuleListModel>();
            result.SearchModel = model;
            result.PagedDatas = TemperatureAlarmRuleBLL.GetPagedTemperatureAlarmRules(model, base.CurrentStrucID, searchPage, this.PageSize);
            return PartialView("_TemperatureAlarmRulePagedGrid", result);
        }

        #endregion


        #region 新增
        public ActionResult AddTemperatureAlarmRule()
        {
            var model = new TemperatureAlarmRuleAddModel();
            model.StartTime = "00:00:00";
            model.EndTime = "23:59:59";
            //得到温度传感器
            var list = TemperatureAlarmRuleBLL.GetTemperatureSensors();
            model.TemperatureAlarmRuleDetails = new List<TemperatureAlarmRuleDetailModel>(list.Count);
            list.ForEach(item =>
            {
                model.TemperatureAlarmRuleDetails.Add(new TemperatureAlarmRuleDetailModel()
                {
                    SensorCode = item.SensorCode,
                    SensorName = item.SensorName
                });
            });
            return PartialView("_AddTemperatureAlarmRule", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddTemperatureAlarmRule(TemperatureAlarmRuleAddModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreateUserID = base.UserIDForLog;
                var result = TemperatureAlarmRuleBLL.AddTemperatureAlarmRule(model);
                base.DoLog(OperationTypeEnum.Add, result, "Name:" + model.Name);
                return Json(result);
            }
            else
            {
                return PartialView("_AddTemperatureAlarmRule", model);
            }
        }



        [HttpPost, AsiatekSubordinateFunction("AddTemperatureAlarmRule")]
        public ActionResult CheckAddTemperatureAlarmRuleNameExists(string name)
        {
            return Json(!TemperatureAlarmRuleBLL.CheckTemperatureAlarmRuleNameExists(name));
        }
        #endregion

        #region 完全编辑
        public ActionResult EditTemperatureAlarmRuleEditCompletely(int id)
        {
            var result = TemperatureAlarmRuleBLL.GetTemperatureAlarmRuleByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }

            var obj = result.DataResult;
            //得到温度传感器
            var list = TemperatureAlarmRuleBLL.GetTemperatureSensors();
            if (list.Count != obj.TemperatureAlarmRuleDetails.Count)
            {
                foreach (var item in list)
                {
                    if (obj.TemperatureAlarmRuleDetails.Where(t => t.SensorCode == item.SensorCode).Count() == 0)
                    {
                        obj.TemperatureAlarmRuleDetails.Add(new TemperatureAlarmRuleDetailModel()
                        {
                            SensorName = item.SensorName,
                            SensorCode = item.SensorCode,
                        });
                    }
                }
                obj.TemperatureAlarmRuleDetails = obj.TemperatureAlarmRuleDetails.OrderBy(t => t.SensorCode).ToList();
            }
            return PartialView("_EditTemperatureAlarmRuleEditCompletely", obj);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditTemperatureAlarmRuleEditCompletely(TemperatureAlarmRuleEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.EditUserID = base.UserIDForLog;
                var result = TemperatureAlarmRuleBLL.EditTemperatureAlarmRuleEdit(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditTemperatureAlarmRuleEditCompletely", model);
            }
        }


        [HttpPost]
        [AsiatekSubordinateFunction("EditTemperatureAlarmRuleEditCompletely")]
        [AsiatekSubordinateFunction("EditTemperatureAlarmRuleEditPartially")]
        public ActionResult CheckEditTemperatureAlarmRuleNameExists(string name, int id)
        {
            return Json(!TemperatureAlarmRuleBLL.CheckTemperatureAlarmRuleNameExists(name, id));
        }

        #endregion

        #region 部分编辑(正常情况下客户只能使用部分编辑功能，但是程序不会限制把完全编辑功能开放给客户)：部分编辑的意思是不能编辑传感器安装位置和数量
        public ActionResult EditTemperatureAlarmRuleEditPartially(int id)
        {
            var result = TemperatureAlarmRuleBLL.GetTemperatureAlarmRuleByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            return PartialView("_EditTemperatureAlarmRuleEditPartially", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditTemperatureAlarmRuleEditPartially(TemperatureAlarmRuleEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.EditUserID = base.UserIDForLog;
                var result = TemperatureAlarmRuleBLL.EditTemperatureAlarmRuleEdit(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditTemperatureAlarmRuleEditPartially", model);
            }
        }
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteTemperatureAlarmRulePhysically(int[] tid)
        {
            var result = TemperatureAlarmRuleBLL.DeleteTemperatureAlarmRulePhysically(tid);
            string idListStr = string.Empty;
            foreach (var item in tid)
            {
                idListStr += item + ",";
            }
            base.DoLog(OperationTypeEnum.Delete, result, "idList:" + idListStr);
            return Json(result);
        }
        #endregion



        #region 温度报警规则分配
        public ActionResult TemperatureAlarmRuleDistribution()
        {
            var model = new SearchDataWithPagedDatas<VehicleTemperatureAlarmRulesSearchModel, VehicleTemperatureAlarmRulesListModel>();
            model.SearchModel = new VehicleTemperatureAlarmRulesSearchModel();
            model.SearchModel.StrucID = -1;
            model.SearchModel.TemperatureAlarmRuleID = -1;
            //代码预留在这里，先强制是默认模式
            if (base.VehicleViewMode)//默认模式
            {
                model.PagedDatas = TemperatureAlarmRuleBLL.GetPagedVehicleTemperatureAlarmRules(model.SearchModel, base.CurrentStrucID, 1, this.PageSize);
            }
            else//自由模式
            {
                model.PagedDatas = TemperatureAlarmRuleBLL.GetPagedVehicleTemperatureAlarmRules(model.SearchModel, base.CurrentStrucID, 1, this.PageSize);
            }
            return PartialView("_TemperatureAlarmRuleDistribution", model);
        }

        [AsiatekSubordinateFunction("TemperatureAlarmRuleDistribution")]
        public ActionResult GetTemperatureAlarmRuleDistribution(VehicleTemperatureAlarmRulesSearchModel model, int searchPage)
        {
            var result = new SearchDataWithPagedDatas<VehicleTemperatureAlarmRulesSearchModel, VehicleTemperatureAlarmRulesListModel>();
            result.SearchModel = model;
            //代码预留在这里，先强制是默认模式
            if (base.VehicleViewMode)//默认模式
            {
                result.PagedDatas = TemperatureAlarmRuleBLL.GetPagedVehicleTemperatureAlarmRules(model, base.CurrentStrucID, searchPage, this.PageSize);
            }
            else//自由模式
            {
                result.PagedDatas = TemperatureAlarmRuleBLL.GetPagedVehicleTemperatureAlarmRules(model, base.CurrentStrucID, searchPage, this.PageSize);
            }

            return PartialView("_VehicleTemperatureAlarmRulesPagedGrid", result);
        }


        [AsiatekSubordinateFunction("TemperatureAlarmRuleDistribution")]
        public ActionResult TemperatureAlarmRules(string idList)
        {
            var list = TemperatureAlarmRuleBLL.GetTemperatureAlarmRules(base.CurrentStrucID);
            if (list == null || list.Count == 0)
            {
                return Content(Asiatek.Resource.UIText.NoDatas);
            }
            ViewBag.VehicleIDs = idList;
            return PartialView("_TemperatureAlarmRules", list);
        }
        #endregion

        #region 分配运输行业
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("TemperatureAlarmRuleDistribution")]
        public ActionResult AllotTemperatureAlarmRules(string vehicleIDs, string ruleIDs)
        {
            var result = TemperatureAlarmRuleBLL.AllotTemperatureAlarmRules(vehicleIDs, ruleIDs);
            base.DoLog(OperationTypeEnum.Edit, result, vehicleIDs);
            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("TemperatureAlarmRuleDistribution")]
        public ActionResult CancelAllotingTemperatureAlarmRules(string vids)
        {
            var result = TemperatureAlarmRuleBLL.CancelAllotingTemperatureAlarmRules(vids);
            base.DoLog(OperationTypeEnum.Edit, result, vids);
            return Json(result);
        }
        #endregion


    }
}

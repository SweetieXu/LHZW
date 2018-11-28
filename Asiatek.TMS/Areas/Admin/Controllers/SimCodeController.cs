using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Resource;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using Asiatek.TMS.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class SimCodeController : BaseController
    {
        #region   查询
        public ActionResult SearchSimCode()
        {
            SearchDataWithPagedDatas<SimCodeSearchModels, SimCodeListModels> model =
      new SearchDataWithPagedDatas<SimCodeSearchModels, SimCodeListModels>();

            model.SearchModel = new SimCodeSearchModels();
            model.SearchModel.CommModeSelectList = SimCodeBLL.GetCommMode().ToSelectListWithAll(m => GetSelectListItem(m.ID, m.Name));
            model.SearchModel.CommMode = -1;

            model.SearchModel.ServiceProviderSelectList = SimCodeBLL.GetServiceProvider().ToSelectListWithAll(m => GetSelectListItem(m.ID, m.Name));
            model.SearchModel.ServiceProviderID = -1;

            model.SearchModel.UseStrucID = -1;
            model.SearchModel.OwnerStrucID = -1;
            model.PagedDatas = SimCodeBLL.GetPagedSimCode(model.SearchModel, 1, this.PageSize);
            return PartialView("_SearchSimCodeList", model);
        }

        [AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult GetSimCodeInfo(SimCodeSearchModels model, int searchPage)
        {
            SearchDataWithPagedDatas<SimCodeSearchModels, SimCodeListModels> result =
       new SearchDataWithPagedDatas<SimCodeSearchModels, SimCodeListModels>();
            result.SearchModel = model;
            result.PagedDatas = SimCodeBLL.GetPagedSimCode(model, searchPage, this.PageSize);
            return PartialView("_SimCodePagedGrid", result);
        }


        /// <summary>
        /// 获取JSON格式单位下拉列表数据
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("SearchSimCode")]
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = Int32.MaxValue, SqlDependency = "asiatekCache:Structures")]
        public ActionResult GetStructures()
        {
            base.FixVaryBug();
            return Json(StructureBLL.GetStructures(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost, AsiatekSubordinateFunction("SearchSimCode")]
        [AsiatekSubordinateFunction("WorkOrderProcessingSetting", "WorkOrderProcessing", "Admin")]
        public ActionResult CheckAddSIMCodeExists(string SIMCode)
        {
            return Json(!SimCodeBLL.CheckSIMCodeExists(SIMCode));
        }
        #endregion

        #region   新增
        [AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult AddSimCode()
        {
            AddSimCodeModels model = new AddSimCodeModels();
            model.OpeningDate = DateTime.Today.ToString("yyyy-MM-dd");
            model.PurchaseDate = DateTime.Today.ToString("yyyy-MM-dd");
            model.ExpiryDate = DateTime.Today.ToString("yyyy-MM-dd");
            model.CommModeSelectList = new SelectList(SimCodeBLL.GetCommMode(), "ID", "Name");
            model.ServiceProviderSelectList = new SelectList(SimCodeBLL.GetServiceProvider(), "ID", "Name");
            //var structures = StructureBLL.GetStructures();
            //model.OwnerStrucName = new SelectList(structures, "ID", "StrucName");
            //model.UseStrucName = new SelectList(structures, "ID", "StrucName");
            return PartialView("_AddSimCode", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult AddSimCode(AddSimCodeModels model)
        {
            if (ModelState.IsValid)
            {
                var result = SimCodeBLL.AddSimCode(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_AddSimCode", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult CheckAddTypeNameExists(string SensorName)
        {
            return Json(!SensorBLL.CheckTypeNameExists(SensorName));
        }
        #endregion

        #region   编辑
        [AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult EditSimCode(int id)
        {
            var result = SimCodeBLL.GetSimCodeID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            model.CommModeSelectList = new SelectList(SimCodeBLL.GetCommMode(), "ID", "Name");
            model.ServiceProviderSelectList = new SelectList(SimCodeBLL.GetServiceProvider(), "ID", "Name");
            //var structures = StructureBLL.GetStructures();
            //model.OwnerStrucName = new SelectList(structures, "ID", "StrucName",model.OwnerStrucID);
            //model.UseStrucName = new SelectList(structures, "ID", "StrucName",model.UseStrucID);
            return PartialView("_EditSimCode", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult EditSimCode(EditSimCodeModels model)
        {
            if (ModelState.IsValid)
            {
                var result = SimCodeBLL.EditSimCode(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditSimCode", model);
            }
        }

        [HttpPost, AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult CheckEditTypeNameExists(string SensorName)
        {
            return Json(!SensorBLL.CheckTypeNameExists(SensorName));
        }
        [HttpPost, AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult CheckEditSIMCodeExists(string SIMCode, int id)
        {
            return Json(!SimCodeBLL.CheckEditSIMCodeExists(SIMCode, id));
        }
        #endregion

        #region   删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult DeleteSimCode(FormCollection fc)
        {
            string[] ids = fc["tid"].Split(',');

            var result = SimCodeBLL.DeleteSimCode(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["tid"]);
            return Json(result);
        }
        #endregion

        #region 导入
        [HttpPost, AsiatekSubordinateFunction("SearchSimCode")]
        public ActionResult ImportExcel(HttpPostedFileBase excelFile)
        {
            object importExcelLocker = new object();
            #region 文件合法性检查
            if (excelFile == null)
            {
                return Json(new { Success = false, Message = DataAnnotations.FileDoesNotExist });
            }
            Regex reg = new Regex(@"\.(xls|xlsx|XLS|XLSX)$");
            string fileName = excelFile.FileName;
            string extName = Path.GetExtension(fileName);
            fileName = fileName.Remove(fileName.LastIndexOf('.'));
            if (!reg.IsMatch(extName))
            {
                return Json(new { Success = false, Message = DataAnnotations.WrongFileType });
            }
            #endregion

            #region 保存上传的文件
            string filePath = string.Empty;
            try
            {
                //保存上传的文件到UploadFiles/VehicleExcels/日期/
                string dirPath = Path.Combine("~/UploadFiles/SimExcels/", DateTime.Now.ToShortDateString());
                dirPath = Server.MapPath(dirPath);
                if (!Directory.Exists(dirPath))
                {
                    lock (importExcelLocker)
                    {
                        if (!Directory.Exists(dirPath))
                        {
                            Directory.CreateDirectory(dirPath);
                        }
                    }
                }
                filePath = Path.Combine(dirPath, string.Format("{0}[{1}]{2}", fileName, DateTime.Now.ToString("HHmmss"), extName));
                excelFile.SaveAs(filePath);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = "导入失败：保存文件异常！" });
            }
            #endregion

            return Json(SimCodeBLL.ImportSimCodes(filePath, "SIM", base.CurrentUserID));
        }
        #endregion

    }
}

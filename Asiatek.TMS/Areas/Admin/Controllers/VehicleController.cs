using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Asiatek.TMS.Helpers;
using Asiatek.Resource;
using System.Text.RegularExpressions;
using System.IO;
using Asiatek.TMS.TerminalOperation;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Asiatek.DBUtility;
using Asiatek.Model.TerminalSetting;
using Asiatek.Common;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using System.Text;
using NPOI.SS.Util;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class VehicleController : BaseController
    {
        #region 查询
        public ActionResult VehicleSetting()
        {
            SearchDataWithPagedDatas<VehicleSearchModel, VehicleListModel> model =
                new SearchDataWithPagedDatas<VehicleSearchModel, VehicleListModel>();
            model.SearchModel = new VehicleSearchModel();

            model.SearchModel.PrimaryTerminalTypeSelectList = TerminalTypeBLL.GetTerminalTypes().ToSelectListWithAll(m => GetSelectListItem(m.ID, m.TerminalName));
            model.SearchModel.PrimaryTerminalTypeID = -1;
            model.SearchModel.SearchStrucID = -1;

            model.PagedDatas = VehicleBLL.GetPagedVehicles(model.SearchModel, 1, this.PageSize, CurrentStrucID);
            return PartialView("_VehicleSetting", model);
        }

        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult GetVehicleInfo(VehicleSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<VehicleSearchModel, VehicleListModel> result =
    new SearchDataWithPagedDatas<VehicleSearchModel, VehicleListModel>();
            result.SearchModel = model;
            result.PagedDatas = VehicleBLL.GetPagedVehicles(model, searchPage, this.PageSize, CurrentStrucID);
            return PartialView("_VehiclePagedGrid", result);
        }

        [HttpPost]
        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult ExportToExcel(VehicleSearchModel model)
        {
            SearchDataWithPagedDatas<VehicleSearchModel, VehicleListModel> result =
    new SearchDataWithPagedDatas<VehicleSearchModel, VehicleListModel>();
            result.SearchModel = model;
            result.PagedDatas = VehicleBLL.GetPagedVehicles(model, 1, this.PageSize, CurrentStrucID);

            ViewBag.ExportToExcel = true;
            ViewData.Model = result;
            string viewHtml = RenderPartialViewToString(this, "_VehiclePagedGrid");
            return File(System.Text.Encoding.UTF8.GetBytes(viewHtml), "application/ms-excel", string.Format("资料{0}.xls", Guid.NewGuid()));

        }

        [AsiatekSubordinateFunction("VehicleSetting")]
        [AsiatekSubordinateFunction("AddEmployeeInfoNew", "EmployeeInfo", "Admin")]
        [AsiatekSubordinateFunction("EditEmployeeInfoNew", "EmployeeInfo", "Admin")]
        [AsiatekSubordinateFunction("EmployeeInfoSetting", "EmployeeInfo", "Admin")]
        public ActionResult GetStructuresByName(string name)
        {
            var list = StructureBLL.GetStructuresByName(name);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.StrucName, value = item.StrucName, ID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据单位名称模糊查询当前单位和子单位
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("VehicleSetting")]
        [AsiatekSubordinateFunction("NewAddVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        public ActionResult GetStrucAndSubStrucByName(string name)
        {
            var list = StructureBLL.GetStrucAndSubStrucByName(name, CurrentStrucID);
            if (list == null) { list = new List<StructureDDLModel>(); }
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.StrucName, value = item.StrucName, ID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        [AsiatekSubordinateFunction("VehicleSetting")]
        [AsiatekSubordinateFunction("EditVehicle")]
        public ActionResult GetStructuresByID(int id)
        {
            var list = StructureBLL.GetStructuresByID(id);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.StrucName, value = item.StrucName, ID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改保固期
        public ActionResult ModifyWarrantyDate(string ids)
        {
            VehicleModifyWarrantyDateModel model = new VehicleModifyWarrantyDateModel();
            model.VehicleIDs = ids.Trim(new char[] { '\"' });
            model.WarrantyDate = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd");
            return View("_ModifyWarrantyDate", model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ModifyWarrantyDate(VehicleModifyWarrantyDateModel model)
        {
            if (ModelState.IsValid)
            {
                var result = VehicleBLL.ModifyWarrantyDate(model);
                base.DoLog(OperationTypeEnum.Edit, result, model.VehicleIDs);
                return Json(result);
            }
            else
            {
                return View("_ModifyWarrantyDate", model);
            }
        }
        #endregion

        #region 修改限速值
        public ActionResult ModifySpeedLimit(string ids)
        {
            VehicleModifySpeedLimitModel model = new VehicleModifySpeedLimitModel();
            model.VehicleIDs = ids.Trim(new char[] { '\"' });
            model.SpeedLimit = 100;
            return View("_ModifySpeedLimit", model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ModifySpeedLimit(VehicleModifySpeedLimitModel model)
        {
            if (ModelState.IsValid)
            {
                var result = VehicleBLL.ModifySpeedLimit(model);
                base.DoLog(OperationTypeEnum.Edit, result, model.VehicleIDs);
                return Json(result);
            }
            else
            {
                return View("_ModifySpeedLimit", model);
            }
        }
        #endregion

        #region 接入
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult OpenAccess(FormCollection fc)
        {
            string[] ids = fc["vids"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            var result = VehicleBLL.ModifyAccess(ids, true);

            base.DoLog(OperationTypeEnum.Edit, result, fc["vids"]);
            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CloseAccess(FormCollection fc)
        {
            string[] ids = fc["vids"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            var result = VehicleBLL.ModifyAccess(ids, false);

            base.DoLog(OperationTypeEnum.Edit, result, fc["vids"]);
            return Json(result);
        }
        #endregion

        #region 转发
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult OpenTransmit(FormCollection fc)
        {
            string[] ids = fc["vids"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            var result = VehicleBLL.ModifyTransmit(ids, true);

            base.DoLog(OperationTypeEnum.Edit, result, fc["vids"]);
            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CloseTransmit(FormCollection fc)
        {
            string[] ids = fc["vids"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            var result = VehicleBLL.ModifyTransmit(ids, false);

            base.DoLog(OperationTypeEnum.Edit, result, fc["vids"]);
            return Json(result);
        }
        #endregion

        #region 启用禁用
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EnableReceive(FormCollection fc)
        {
            string[] ids = fc["vids"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            var result = VehicleBLL.ModifyReceive(ids, true);

            base.DoLog(OperationTypeEnum.Edit, result, fc["vids"]);
            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DisableReceive(FormCollection fc)
        {
            string[] ids = fc["vids"].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            var result = VehicleBLL.ModifyReceive(ids, false);

            base.DoLog(OperationTypeEnum.Edit, result, fc["vids"]);
            return Json(result);
        }
        #endregion

        #region 新增
        public ActionResult AddVehicle()
        {
            VehicleAddModel model = new VehicleAddModel();
            model.IsReceived = true;
            model.IsAccess = false;
            model.IsTransmit = false;
            model.IsDangerous = false;
            model.WarrantyDate = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd");
            model.Icon = "Default";//默认一个图标
            model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
            model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
            model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
            model.TerminalsSelectList = new SelectList(TerminalBLL.GetUsefulTerminals(), "ID", "TerminalCode");

            return PartialView("_AddVehicle", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddVehicle(VehicleAddModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                model.CreateUserID = base.UserIDForLog;
                var result = VehicleBLL.AddVehicle(model);
                base.DoLog(OperationTypeEnum.Add, result, "PlateNum:" + model.PlateNum);
                return Json(result);
            }
            else
            {
                model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
                model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
                model.TerminalsSelectList = new SelectList(TerminalBLL.GetUsefulTerminals(), "ID", "TerminalCode");
                model.Icons = new List<string>()
                {
                    "Default","Arrow","MixerTruck","PumpTruck"
                };
                return PartialView("_AddVehicle", model);
            }
        }

        /// <summary>
        /// 检查初始车牌是否重复
        /// </summary>
        /// <param name="vehicleCode"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("AddVehicle")]
        public ActionResult CheckAddVehicleCodeExists(string vehicleCode)
        {
            return Json(!VehicleBLL.CheckVehicleCodeExists(vehicleCode));
        }

        /// <summary>
        /// 检查同一个使用单位下车代号是否重复
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("AddVehicle")]
        public ActionResult CheckAddVehicleNameExists(string vehicleName, int strucID)
        {
            return Json(!VehicleBLL.CheckVehicleNameExists(vehicleName, strucID));
        }

        /// <summary>
        /// 检查终端号是否重复
        /// </summary>
        /// <param name="terminalCode"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("AddVehicle")]
        public ActionResult CheckAddTerminalCodeExists(string terminalCode)
        {
            return Json(!VehicleBLL.CheckTerminalCodeExists(terminalCode));
        }

        /// <summary>
        /// 检查车牌号是否重复
        /// </summary>
        /// <param name="plateNum"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("AddVehicle")]
        public ActionResult CheckAddPlateNumExists(string plateNum)
        {
            return Json(!VehicleBLL.CheckPlateNumExists(plateNum));
        }

        /// <summary>
        /// 检查SIM卡号是否重复
        /// </summary>
        /// <param name="SIMCode"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("AddVehicle")]
        public ActionResult CheckAddSIMCodeExists(string SIMCode)
        {
            return Json(!VehicleBLL.CheckSIMCodeExists(SIMCode));
        }

        /// <summary>
        /// 检查车架号VIN是否重复
        /// </summary>
        /// <param name="VIN"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("AddVehicle")]
        public ActionResult CheckAddVINExists(string VIN)
        {
            return Json(!VehicleBLL.CheckAddVINExists(VIN));
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 检查初始车牌是否重复
        /// </summary>
        /// <param name="vehicleCode"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EditVehicle")]
        public ActionResult CheckEditVehicleCodeExists(string vehicleCode, long id)
        {
            return Json(!VehicleBLL.CheckVehicleCodeExists(vehicleCode, id));
        }

        /// <summary>
        /// 检查同一个使用单位下车代号是否重复
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EditVehicle")]
        public ActionResult CheckEditVehicleNameExists(string vehicleName, int strucID, long id)
        {
            return Json(!VehicleBLL.CheckVehicleNameExists(vehicleName, strucID, id));
        }

        /// <summary>
        /// 检查终端号是否重复
        /// </summary>
        /// <param name="terminalCode"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EditVehicle")]
        public ActionResult CheckEditTerminalCodeExists(string terminalCode, long id)
        {
            return Json(!VehicleBLL.CheckTerminalCodeExists(terminalCode, id));
        }

        /// <summary>
        /// 检查车牌号是否重复
        /// </summary>
        /// <param name="plateNum"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EditVehicle")]
        public ActionResult CheckEditPlateNumExists(string plateNum, long id)
        {
            return Json(!VehicleBLL.CheckPlateNumExists(plateNum, id));
        }

        /// <summary>
        /// 检查SIM卡号是否重复
        /// </summary>
        /// <param name="SIMCode"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EditVehicle")]
        public ActionResult CheckEditSIMCodeExists(string SIMCode, long id)
        {
            return Json(!VehicleBLL.CheckSIMCodeExists(SIMCode, id));
        }

        /// <summary>
        /// 检查车架号VIN是否重复
        /// </summary>
        /// <param name="VIN"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EditVehicle")]
        public ActionResult CheckEditVINExists(string VIN, long id)
        {
            return Json(!VehicleBLL.CheckEditVINExists(VIN, id));
        }

        public ActionResult EditVehicle(long id)
        {
            var result = VehicleBLL.GetEditVehicleByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };

            model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
            model.TerminalTypesSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypes(), "ID", "TerminalName");
            model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
            var terminals = TerminalBLL.GetUsefulTerminals(id, model.TerminalCode);
            model.PrimaryTerminalsSelectList = new SelectList(terminals, "ID", "TerminalCode");

            return PartialView("_EditVehicle", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditVehicle(VehicleEditModel model, FormCollection fc)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserID = base.UserIDForLog;
                var result = VehicleBLL.EditVehicle(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
                model.TerminalTypesSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypes(), "ID", "TerminalName");
                model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
                var terminals = TerminalBLL.GetUsefulTerminals(model.ID, model.TerminalCode);
                model.PrimaryTerminalsSelectList = new SelectList(terminals, "ID", "TerminalCode");
                model.Icons = new List<string>()
                {
                    "Default","Arrow","MixerTrucks","PumpTruck"
                };
                return PartialView("_EditVehicle", model);
            }
        }
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteVehicle(FormCollection fc)
        {
            string[] ids = fc["vid"].Split(',');

            // var result = VehicleBLL.DeleteVehicle(ids);
            var result = VehicleBLL.DeleteVehiclePhysical(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["vid"]);
            return Json(result);
        }
        #endregion

        #region 车辆分配
        [AsiatekSubordinateFunction("DistributionSetting", "StrucVehicleDistribution", "Admin")]
        public ActionResult GetDistributiveVehiclesByUserID(int uid)
        {
            return Json(VehicleBLL.GetDistributiveVehiclesByUserID(uid), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据单位名称和车牌号模糊查询前五条车辆信息记录用于车辆分配
        /// </summary>
        /// <param name="strucName"></param>
        /// <param name="plateNum"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("DistributionSetting", "StrucVehicleDistribution", "Admin")]
        public ActionResult GetTOP5VehiclesByStrucNameAndPlateNum(string strucName, string plateNum)
        {
            return Json(VehicleBLL.GetVehicles(strucName, plateNum), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 导入Excel
        static object importExcelLocker = new object();
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ImportExcel(HttpPostedFileBase excelFile)
        {
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
                string dirPath = Path.Combine("~/UploadFiles/VehicleExcels/", DateTime.Now.ToShortDateString());
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

            return Json(VehicleBLL.ImportVehicles(filePath, "Vehicle", base.CurrentUserID));
        }
        #endregion

        #region 导出Excel
        [HttpGet]
        public ActionResult ExportExcel()
        {
            try
            {
                DataTable dtSource = null; ;
                //默认模式
                if (base.VehicleViewMode)
                {
                    dtSource = VehicleBLL.GetVehicleInfo(base.CurrentStrucID, base.VehicleViewMode);
                }
                else
                {
                    dtSource = VehicleBLL.GetVehicleInfo(base.CurrentUserID, base.VehicleViewMode);
                }
                if (dtSource == null || dtSource.Rows.Count < 1)
                {
                    dtSource = new DataTable();
                    dtSource.Columns.Add(Asiatek.Resource.UIText.PlateNumber);
                    dtSource.Columns.Add(Asiatek.Resource.UIText.TerminalSetting_TerminalCode);
                    dtSource.Columns.Add(Asiatek.Resource.UIText.SIMCode);
                    dtSource.Columns.Add(Asiatek.Resource.UIText.Struct);
                    dtSource.Columns.Add(Asiatek.Resource.DisplayText.VIN);
                }
                else
                {
                    dtSource.Columns[0].ColumnName = Asiatek.Resource.UIText.PlateNumber;
                    dtSource.Columns[1].ColumnName = Asiatek.Resource.UIText.TerminalSetting_TerminalCode;
                    dtSource.Columns[2].ColumnName = Asiatek.Resource.UIText.SIMCode;
                    dtSource.Columns[3].ColumnName = Asiatek.Resource.UIText.Struct;
                    dtSource.Columns[4].ColumnName = Asiatek.Resource.DisplayText.VIN;
                }
                string strHeaderText = "车辆信息";
                string fileName = String.Format("{0}_{1}_{2}.xls", "车辆列表", DateTime.Now.ToString("yyyyMMddHHmmss"), base.User.Identity.Name.Split('|')[5]);
                return File(NPOIHelper.Export(dtSource, strHeaderText), "application/vnd.ms-excel", fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 新增编辑车辆信息新版
        #region 新增车辆信息初始化界面
        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult AddVehicleNew()
        {
            EditVehicleModel model = new EditVehicleModel();
            model.IsReceived = true;
            model.IsAccess = false;
            model.IsTransmit = false;
            model.IsDangerous = false;
            model.WarrantyDate = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd");
            model.SoftwareDate = DateTime.Today.AddYears(3).ToString("yyyy-MM-dd");
            model.Icon = "Default";//默认一个图标
            model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
            model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
            model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
            var terminalsList = TerminalBLL.GetNewUsefulTerminals(0);
            if (terminalsList != null && terminalsList.Count > 0)
            {
                model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode", terminalsList[0].ID);
                model.TerminalCode = terminalsList[0].TerminalCode;
            }
            else
            {
                model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode");
            }

            List<EmployeeInfoDDLModel> list = new List<EmployeeInfoDDLModel>();
            model.CarrierSelectList = new SelectList(list, "ID", "EmployeeName");
            model.DriverSelectList = new SelectList(list, "ID", "EmployeeName");

            return PartialView("_AddVehicleNew", model);
        }
        #endregion

        #region 新增车辆
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult AddVehicleNew(EditVehicleModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreateUserID = base.UserIDForLog;
                var result = VehicleBLL.AddVehicleNew(model);
                base.DoLog(OperationTypeEnum.Add, result, "PlateNum:" + model.PlateNum);
                return Json(result);
            }
            else
            {
                model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
                model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
                model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
                var terminalsList = TerminalBLL.GetUsefulTerminals();
                if (terminalsList != null && terminalsList.Count > 0)
                {
                    model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode", terminalsList[0].ID);
                    model.TerminalCode = terminalsList[0].TerminalCode;
                }
                else
                {
                    model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode");
                }
                List<EmployeeInfoDDLModel> list = new List<EmployeeInfoDDLModel>();
                model.CarrierSelectList = new SelectList(list, "ID", "EmployeeName");
                model.DriverSelectList = new SelectList(list, "ID", "EmployeeName");
                return PartialView("_AddVehicleNew", model);

            }
        }
        #endregion

        #region 编辑车辆初始化页面
        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult EditVehicleNew(long id)
        {
            var result = VehicleBLL.GetVehicleInfoByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
            model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
            model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
            model.TerminalsSelectList = new SelectList(TerminalBLL.GetNewUsefulTerminals(id), "ID", "TerminalCode");

            // 如果是危险品车
            if (model.IsDangerous)
            {
                // 获取驾驶员和押运员信息
                List<VehicleEmployeeInfoDDLModel> listVehicleEmployee = VehicleBLL.GetVehicleEmployeeInfoByID(id);
                if (listVehicleEmployee != null && listVehicleEmployee.Count > 1)
                {
                    model.CarrierID = listVehicleEmployee.Find(o => o.Type == 2).EmployeeInfoID;
                    model.DriverID = listVehicleEmployee.Find(o => o.Type == 1).EmployeeInfoID;
                }

                // 获取当前单位的员工信息（驾驶员和押运员）
                var listEmployeeInfo = VehicleBLL.GetEmployeeInfoByStrucID(model.StrucID);
                if (listEmployeeInfo != null && listEmployeeInfo.Count > 0)
                {
                    model.CarrierSelectList = new SelectList(listEmployeeInfo.FindAll(o => o.IsCarrier == true), "ID", "EmployeeName");
                    model.DriverSelectList = new SelectList(listEmployeeInfo.FindAll(o => o.IsDriver == true), "ID", "EmployeeName");
                }
                else
                {
                    List<EmployeeInfoDDLModel> tempList = new List<EmployeeInfoDDLModel>();
                    model.CarrierSelectList = new SelectList(tempList, "ID", "EmployeeName");
                    model.DriverSelectList = new SelectList(tempList, "ID", "EmployeeName");
                }
                // 获取经营范围
                var list = VehicleBLL.GetBussinessScopeByVehicleIDAndStrucID(model.StrucID, id);
                if (list != null)
                {
                    ViewBag.StrucVehicleBussinessScope = list;
                }
                // 获取运输行业
                var transportIndustryList = StructureBLL.GetTransportIndustryListByStrucID(model.StrucID);
                if (transportIndustryList != null)
                {
                    ViewBag.TransportIndustryList = transportIndustryList;
                }
                // 获取运管所
                if (model.IsTransmit)
                {
                    var transportManagementList = VehicleBLL.GetTransportManagementByVehicleID(id);
                    if (transportManagementList != null)
                    {
                        ViewBag.TransportManagementList = transportManagementList;
                    }
                }
            }
            else
            {
                List<EmployeeInfoDDLModel> list = new List<EmployeeInfoDDLModel>();
                model.CarrierSelectList = new SelectList(list, "ID", "EmployeeName");
                model.DriverSelectList = new SelectList(list, "ID", "EmployeeName");
            }

            return PartialView("_EditVehicleNew", model);
        }
        #endregion

        #region 编辑车辆
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult EditVehicleNew(EditVehicleModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserID = base.UserIDForLog;
                var result = VehicleBLL.EditVehicleNew(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {

                model.Icons = new List<string>()
                {
                    "Default","Arrow","MixerTrucks","PumpTruck"
                };
                model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
                model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
                model.TerminalsSelectList = new SelectList(TerminalBLL.GetNewUsefulTerminals(model.ID), "ID", "TerminalCode");

                // 如果是危险品车
                if (model.IsDangerous)
                {
                    // 获取驾驶员和押运员信息
                    List<VehicleEmployeeInfoDDLModel> listVehicleEmployee = VehicleBLL.GetVehicleEmployeeInfoByID(model.ID);
                    if (listVehicleEmployee != null && listVehicleEmployee.Count > 1)
                    {
                        model.CarrierID = listVehicleEmployee.Find(o => o.Type == 2).EmployeeInfoID;
                        model.DriverID = listVehicleEmployee.Find(o => o.Type == 1).EmployeeInfoID;
                    }

                    // 获取当前单位的员工信息（驾驶员和押运员）
                    var listEmployeeInfo = VehicleBLL.GetEmployeeInfoByStrucID(model.StrucID);
                    if (listEmployeeInfo != null && listEmployeeInfo.Count > 0)
                    {
                        model.CarrierSelectList = new SelectList(listEmployeeInfo.FindAll(o => o.IsCarrier == true), "ID", "EmployeeName");
                        model.DriverSelectList = new SelectList(listEmployeeInfo.FindAll(o => o.IsDriver == true), "ID", "EmployeeName");
                    }
                    else
                    {
                        List<EmployeeInfoDDLModel> tempList = new List<EmployeeInfoDDLModel>();
                        model.CarrierSelectList = new SelectList(tempList, "ID", "EmployeeName");
                        model.DriverSelectList = new SelectList(tempList, "ID", "EmployeeName");
                    }
                    // 获取经营范围
                    var list = VehicleBLL.GetBussinessScopeByVehicleIDAndStrucID(model.StrucID, model.ID);
                    if (list != null)
                    {
                        ViewBag.StrucVehicleBussinessScope = list;
                    }
                    // 获取运输行业
                    var transportIndustryList = StructureBLL.GetTransportIndustryListByStrucID(model.StrucID);
                    if (transportIndustryList != null)
                    {
                        ViewBag.TransportIndustryList = transportIndustryList;
                    }
                    // 获取运管所
                    if (model.IsTransmit)
                    {
                        var transportManagementList = VehicleBLL.GetTransportManagementByVehicleID(model.ID);
                        if (transportManagementList != null)
                        {
                            ViewBag.TransportManagementList = transportManagementList;
                        }
                    }
                }
                else
                {
                    List<EmployeeInfoDDLModel> list = new List<EmployeeInfoDDLModel>();
                    model.CarrierSelectList = new SelectList(list, "ID", "EmployeeName");
                    model.DriverSelectList = new SelectList(list, "ID", "EmployeeName");
                }
                return PartialView("_EditVehicleNew", model);
            }
        }
        #endregion

        #region 根据使用单位获取经营单位列表
        /// <summary>
        /// 根据使用单位获取经营单位列表
        /// </summary>
        /// <param name="strucID">单位ID</param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("NewAddVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        public ActionResult GetBusinessScopeByStrucID(int strucID)
        {
            var list = VehicleBLL.GetBusinessScopeByStrucID(strucID);
            return Json(list);
        }
        #endregion

        #region 获取使用单位对应的运输行业列表
        /// <summary>
        /// 获取使用单位对应的运输行业列表
        /// </summary>
        /// <param name="strucID">单位ID</param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("NewAddVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        public ActionResult GetTransportIndustryByStrucID(int strucID)
        {
            var list = VehicleBLL.GetTransportIndustryByStrucID(strucID);
            return Json(list);
        }
        #endregion

        #region 获取运管所列表
        /// <summary>
        /// 获取运管所列表
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("NewAddVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        public ActionResult GetTransportManagement()
        {
            var list = TransportManagementBLL.GetTransportManagementDDL();
            return Json(list);
        }
        #endregion

        #region 根据使用单位获取员工信息（驾驶员和押运员）
        /// <summary>
        /// 根据使用单位获取员工信息
        /// </summary>
        /// <param name="strucID">单位ID</param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("NewAddVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        public ActionResult GetEmployeeInfoByStrucID(int strucID)
        {
            var list = VehicleBLL.GetEmployeeInfoByStrucID(strucID);
            return Json(list);
        }
        #endregion

        #region 唯一性验证
        /// <summary>
        /// 检查车牌号是否重复
        /// </summary>
        /// <param name="plateNum"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("NewAddVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        public ActionResult CheckNewEditPlateNumExists(string plateNum, long id = 0)
        {
            return Json(!VehicleBLL.CheckNewEditPlateNumExists(plateNum, id));
        }

        /// <summary>
        /// 检查车架号VIN是否重复
        /// </summary>
        /// <param name="VIN"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("NewAddVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        public ActionResult CheckNewEditVINExists(string VIN, long id = 0)
        {
            return Json(!VehicleBLL.CheckNewEditVINExists(VIN, id));
        }

        /// <summary>
        /// 检查同一个使用单位下车代号是否重复
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("NewAddVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        public ActionResult CheckNewEditVehicleNameExists(string vehicleName, int strucID, long id = 0)
        {
            if (strucID > 0)
            {
                return Json(!VehicleBLL.CheckNewEditVehicleNameExists(vehicleName, strucID, id));
            }
            else
            {
                return Json(true);
            }

        }
        #endregion

        #endregion

        #region 新增编辑车辆信息新版 非危险品车也可以选择驾驶员 2017-10-16
        #region 新增车辆信息初始化界面
        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult AddVehicle_New()
        {
            EditVehicleModel model = new EditVehicleModel();
            model.IsReceived = true;
            model.IsAccess = false;
            model.IsTransmit = false;
            model.IsDangerous = false;
            model.WarrantyDate = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd");
            model.SoftwareDate = DateTime.Today.AddYears(3).ToString("yyyy-MM-dd");
            model.Icon = "Default";//默认一个图标
            model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
            model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
            model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
            var terminalsList = TerminalBLL.GetNewUsefulTerminals(0);
            if (terminalsList != null && terminalsList.Count > 0)
            {
                model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode", terminalsList[0].ID);
                model.TerminalCode = terminalsList[0].TerminalCode;
            }
            else
            {
                model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode");
            }

            List<EmployeeInfoDDLModel> list = new List<EmployeeInfoDDLModel>();
            model.CarrierSelectList = new SelectList(list, "ID", "EmployeeName");
            model.DriverSelectList = new SelectList(list, "ID", "EmployeeName");

            return PartialView("_AddVehicle_New", model);
        }
        #endregion

        #region 新增车辆
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult AddVehicle_New(EditVehicleModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreateUserID = base.UserIDForLog;
                var result = VehicleBLL.AddVehicle_New(model);
                base.DoLog(OperationTypeEnum.Add, result, "PlateNum:" + model.PlateNum);
                return Json(result);
            }
            else
            {
                model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
                model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
                model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
                var terminalsList = TerminalBLL.GetUsefulTerminals();
                if (terminalsList != null && terminalsList.Count > 0)
                {
                    model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode", terminalsList[0].ID);
                    model.TerminalCode = terminalsList[0].TerminalCode;
                }
                else
                {
                    model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode");
                }
                List<EmployeeInfoDDLModel> list = new List<EmployeeInfoDDLModel>();
                model.CarrierSelectList = new SelectList(list, "ID", "EmployeeName");
                model.DriverSelectList = new SelectList(list, "ID", "EmployeeName");
                return PartialView("_AddVehicle_New", model);

            }
        }
        #endregion

        #region 编辑车辆初始化页面
        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult EditVehicle_New(long id)
        {
            var result = VehicleBLL.GetVehicleInfoByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
            model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
            model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
            model.TerminalsSelectList = new SelectList(TerminalBLL.GetNewUsefulTerminals(id), "ID", "TerminalCode");

            // 获取当前驾驶员和押运员信息
            List<VehicleEmployeeInfoDDLModel> listVehicleEmployee = VehicleBLL.GetVehicleEmployeeInfoByID(id);
            if (listVehicleEmployee != null && listVehicleEmployee.Count > 0)
            {
                model.DriverID = listVehicleEmployee.Find(o => o.Type == 1).EmployeeInfoID;
            }

            // 获取当前单位的员工信息（驾驶员和押运员）
            var listEmployeeInfo = VehicleBLL.GetEmployeeInfoByStrucID(model.StrucID);
            List<SelectListItem> liDriver = new List<SelectListItem>();
            liDriver.Add(new SelectListItem() { Text = DisplayText.PleaseSelect, Value = "" });

            // 驾驶员信息
            if (listEmployeeInfo != null && listEmployeeInfo.Count > 0)
            {
                liDriver.AddRange(new SelectList(listEmployeeInfo.FindAll(o => o.IsDriver == true), "ID", "EmployeeName"));
            }
            model.DriverSelectList = liDriver;

            // 如果是危险品车
            if (model.IsDangerous)
            {
                if (listVehicleEmployee != null && listVehicleEmployee.Count > 1)
                {
                    model.CarrierID = listVehicleEmployee.Find(o => o.Type == 2).EmployeeInfoID;
                    model.DriverID = listVehicleEmployee.Find(o => o.Type == 1).EmployeeInfoID;
                }

                if (listEmployeeInfo != null && listEmployeeInfo.Count > 0)
                {
                    model.CarrierSelectList = new SelectList(listEmployeeInfo.FindAll(o => o.IsCarrier == true), "ID", "EmployeeName");
                    model.DriverSelectList = new SelectList(listEmployeeInfo.FindAll(o => o.IsDriver == true), "ID", "EmployeeName");
                }
                else
                {
                    List<EmployeeInfoDDLModel> tempList = new List<EmployeeInfoDDLModel>();
                    model.CarrierSelectList = new SelectList(tempList, "ID", "EmployeeName");
                    model.DriverSelectList = new SelectList(tempList, "ID", "EmployeeName");
                }
                // 获取经营范围
                var list = VehicleBLL.GetBussinessScopeByVehicleIDAndStrucID(model.StrucID, id);
                if (list != null)
                {
                    ViewBag.StrucVehicleBussinessScope = list;
                }
                // 获取运输行业
                var transportIndustryList = StructureBLL.GetTransportIndustryListByStrucID(model.StrucID);
                if (transportIndustryList != null)
                {
                    ViewBag.TransportIndustryList = transportIndustryList;
                }
                // 获取运管所
                if (model.IsTransmit)
                {
                    var transportManagementList = VehicleBLL.GetTransportManagementByVehicleID(id);
                    if (transportManagementList != null)
                    {
                        ViewBag.TransportManagementList = transportManagementList;
                    }
                }
            }
            else
            {
                List<EmployeeInfoDDLModel> list = new List<EmployeeInfoDDLModel>();
                model.CarrierSelectList = new SelectList(list, "ID", "EmployeeName");
            }

            return PartialView("_EditVehicle_New", model);
        }
        #endregion

        #region 编辑车辆
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("VehicleSetting")]
        public ActionResult EditVehicle_New(EditVehicleModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserID = base.UserIDForLog;
                var result = VehicleBLL.EditVehicle_New(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                model.Icons = new List<string>()
                {
                    "Default","Arrow","MixerTrucks","PumpTruck"
                };
                model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
                model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
                model.TerminalsSelectList = new SelectList(TerminalBLL.GetNewUsefulTerminals(model.ID), "ID", "TerminalCode");

                // 获取驾驶员和押运员信息
                List<VehicleEmployeeInfoDDLModel> listVehicleEmployee = VehicleBLL.GetVehicleEmployeeInfoByID(model.ID);
                if (listVehicleEmployee != null && listVehicleEmployee.Count > 0)
                {
                    model.DriverID = listVehicleEmployee.Find(o => o.Type == 1).EmployeeInfoID;
                }

                // 获取当前单位的员工信息（驾驶员和押运员）
                var listEmployeeInfo = VehicleBLL.GetEmployeeInfoByStrucID(model.StrucID);
                List<SelectListItem> liDriver = new List<SelectListItem>();
                liDriver.Add(new SelectListItem() { Text = DisplayText.PleaseSelect, Value = "" });

                // 驾驶员信息
                if (listEmployeeInfo != null && listEmployeeInfo.Count > 0)
                {
                    liDriver.AddRange(new SelectList(listEmployeeInfo.FindAll(o => o.IsDriver == true), "ID", "EmployeeName"));
                }
                model.DriverSelectList = liDriver;

                // 如果是危险品车
                if (model.IsDangerous)
                {
                    if (listVehicleEmployee != null && listVehicleEmployee.Count > 1)
                    {
                        model.CarrierID = listVehicleEmployee.Find(o => o.Type == 2).EmployeeInfoID;
                        model.DriverID = listVehicleEmployee.Find(o => o.Type == 1).EmployeeInfoID;
                    }

                    if (listEmployeeInfo != null && listEmployeeInfo.Count > 0)
                    {
                        model.CarrierSelectList = new SelectList(listEmployeeInfo.FindAll(o => o.IsCarrier == true), "ID", "EmployeeName");
                        model.DriverSelectList = new SelectList(listEmployeeInfo.FindAll(o => o.IsDriver == true), "ID", "EmployeeName");
                    }
                    else
                    {
                        List<EmployeeInfoDDLModel> tempList = new List<EmployeeInfoDDLModel>();
                        model.CarrierSelectList = new SelectList(tempList, "ID", "EmployeeName");
                        model.DriverSelectList = new SelectList(tempList, "ID", "EmployeeName");
                    }
                    // 获取经营范围
                    var list = VehicleBLL.GetBussinessScopeByVehicleIDAndStrucID(model.StrucID, model.ID);
                    if (list != null)
                    {
                        ViewBag.StrucVehicleBussinessScope = list;
                    }
                    // 获取运输行业
                    var transportIndustryList = StructureBLL.GetTransportIndustryListByStrucID(model.StrucID);
                    if (transportIndustryList != null)
                    {
                        ViewBag.TransportIndustryList = transportIndustryList;
                    }
                    // 获取运管所
                    if (model.IsTransmit)
                    {
                        var transportManagementList = VehicleBLL.GetTransportManagementByVehicleID(model.ID);
                        if (transportManagementList != null)
                        {
                            ViewBag.TransportManagementList = transportManagementList;
                        }
                    }
                }
                else
                {
                    List<EmployeeInfoDDLModel> list = new List<EmployeeInfoDDLModel>();
                    model.CarrierSelectList = new SelectList(list, "ID", "EmployeeName");
                }
                return PartialView("_EditVehicle_New", model);
            }
        }
        #endregion
        #endregion

        #region 新增编辑车辆信息新版 非危险品车也可以选择驾驶员 驾驶员押运员的下拉框做成autocomplete 2017-12-06
        #region 新增车辆信息初始化界面
        public ActionResult NewAddVehicle()
        {
            NewEditVehicleModel model = new NewEditVehicleModel();
            model.IsReceived = true;
            model.IsAccess = false;
            model.IsTransmit = false;
            model.IsDangerous = false;
            model.WarrantyDate = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd");
            model.SoftwareDate = DateTime.Today.AddYears(3).ToString("yyyy-MM-dd");
            model.Icon = "Default";//默认一个图标
            model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
            model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
            model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
            var terminalsList = TerminalBLL.GetNewUsefulTerminals(0);
            if (terminalsList != null && terminalsList.Count > 0)
            {
                model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode", terminalsList[0].ID);
                model.TerminalCode = terminalsList[0].TerminalCode;
            }
            else
            {
                model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode");
            }

            return PartialView("_NewAddVehicle", model);
        }
        #endregion

        #region 新增车辆
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewAddVehicle(NewEditVehicleModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreateUserID = base.UserIDForLog;
                var result = VehicleBLL.NewAddVehicle(model);
                base.DoLog(OperationTypeEnum.Add, result, "PlateNum:" + model.PlateNum);
                return Json(result);
            }
            else
            {
                model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
                model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
                model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
                var terminalsList = TerminalBLL.GetUsefulTerminals();
                if (terminalsList != null && terminalsList.Count > 0)
                {
                    model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode", terminalsList[0].ID);
                    model.TerminalCode = terminalsList[0].TerminalCode;
                }
                else
                {
                    model.TerminalsSelectList = new SelectList(terminalsList, "ID", "TerminalCode");
                }
                return PartialView("_NewAddVehicle", model);

            }
        }
        #endregion

        #region 编辑车辆初始化页面
        public ActionResult NewEditVehicle(long id)
        {
            var result = VehicleBLL.GetVehicleInfoByID_New(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            model.Icons = new List<string>()
            {
                "Default","Arrow","MixerTruck","PumpTruck"
            };
            model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
            model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
            model.TerminalsSelectList = new SelectList(TerminalBLL.GetNewUsefulTerminals(id), "ID", "TerminalCode");
            // 如果是危险品车
            if (model.IsDangerous)
            {
                // 获取经营范围
                var list = VehicleBLL.GetBussinessScopeByVehicleIDAndStrucID(model.StrucID, id);
                if (list != null)
                {
                    ViewBag.StrucVehicleBussinessScope = list;
                }
                // 获取运输行业
                var transportIndustryList = StructureBLL.GetTransportIndustryListByStrucID(model.StrucID);
                if (transportIndustryList != null)
                {
                    ViewBag.TransportIndustryList = transportIndustryList;
                }
                // 获取运管所
                if (model.IsTransmit)
                {
                    var transportManagementList = VehicleBLL.GetTransportManagementByVehicleID(id);
                    if (transportManagementList != null)
                    {
                        ViewBag.TransportManagementList = transportManagementList;
                    }
                }
            }
            return PartialView("_NewEditVehicle", model);
        }
        #endregion

        #region 编辑车辆
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewEditVehicle(NewEditVehicleModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserID = base.UserIDForLog;
                var result = VehicleBLL.NewEditVehicle(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                model.Icons = new List<string>()
                {
                    "Default","Arrow","MixerTrucks","PumpTruck"
                };
                model.PlateColorsSelectList = new SelectList(PlateColorBLL.GetPlateColors(), "Code", "Name");
                model.VehicleTypesSelectList = new SelectList(VehicleTypeBLL.GetVehicleTypes(), "Code", "Name");
                model.TerminalsSelectList = new SelectList(TerminalBLL.GetNewUsefulTerminals(model.ID), "ID", "TerminalCode");

                // 如果是危险品车
                if (model.IsDangerous)
                {
                    // 获取经营范围
                    var list = VehicleBLL.GetBussinessScopeByVehicleIDAndStrucID(model.StrucID, model.ID);
                    if (list != null)
                    {
                        ViewBag.StrucVehicleBussinessScope = list;
                    }
                    // 获取运输行业
                    var transportIndustryList = StructureBLL.GetTransportIndustryListByStrucID(model.StrucID);
                    if (transportIndustryList != null)
                    {
                        ViewBag.TransportIndustryList = transportIndustryList;
                    }
                    // 获取运管所
                    if (model.IsTransmit)
                    {
                        var transportManagementList = VehicleBLL.GetTransportManagementByVehicleID(model.ID);
                        if (transportManagementList != null)
                        {
                            ViewBag.TransportManagementList = transportManagementList;
                        }
                    }
                }
                return PartialView("_NewEditVehicle", model);
            }
        }
        #endregion
        #endregion

        #region 根据使用单位和 驾驶员或者 押运员或者 车主名称模糊查询员工信息
        [AsiatekSubordinateFunction("NewAddVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle")]
        [AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        public ActionResult GetEmployeeInfoByStrucIDAndName(string name, int strucID, int type)
        {
            var list = EmployeeInfoBLL.GetEmployeeInfoByStrucIDAndName(name, strucID, type);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.EmployeeName, value = item.EmployeeName, ID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 宿州运达编辑车辆
        public ActionResult NewEditVehicle_SZYD(long id)
        {
            var result = VehicleBLL.GetVehicleInfoByID_New_SZYD(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            // 如果是危险品车
            if (model.IsDangerous)
            {
                // 获取经营范围
                var list = VehicleBLL.GetBussinessScopeByVehicleIDAndStrucID(model.StrucID, id);
                if (list != null)
                {
                    ViewBag.StrucVehicleBussinessScope = list;
                }
                // 获取运输行业
                var transportIndustryList = StructureBLL.GetTransportIndustryListByStrucID(model.StrucID);
                if (transportIndustryList != null)
                {
                    ViewBag.TransportIndustryList = transportIndustryList;
                }
                // 获取运管所
                if (model.IsTransmit)
                {
                    var transportManagementList = VehicleBLL.GetTransportManagementByVehicleID(id);
                    if (transportManagementList != null)
                    {
                        ViewBag.TransportManagementList = transportManagementList;
                    }
                }
            }
            return PartialView("_NewEditVehicle_SZYD", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult NewEditVehicle_SZYD(NewEditVehicleModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserID = base.UserIDForLog;
                var result = VehicleBLL.NewEditVehicle_SZYD(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                // 如果是危险品车
                if (model.IsDangerous)
                {
                    // 获取经营范围
                    var list = VehicleBLL.GetBussinessScopeByVehicleIDAndStrucID(model.StrucID, model.ID);
                    if (list != null)
                    {
                        ViewBag.StrucVehicleBussinessScope = list;
                    }
                    // 获取运输行业
                    var transportIndustryList = StructureBLL.GetTransportIndustryListByStrucID(model.StrucID);
                    if (transportIndustryList != null)
                    {
                        ViewBag.TransportIndustryList = transportIndustryList;
                    }
                    // 获取运管所
                    if (model.IsTransmit)
                    {
                        var transportManagementList = VehicleBLL.GetTransportManagementByVehicleID(model.ID);
                        if (transportManagementList != null)
                        {
                            ViewBag.TransportManagementList = transportManagementList;
                        }
                    }
                }
                return PartialView("_NewEditVehicle_SZYD", model);
            }
        }

        ///// <summary>
        ///// 根据车主名称模糊查询  加载当前单位和子单位下的车主信息
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="strucID"></param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //[AsiatekSubordinateFunction("NewEditVehicle_SZYD")]
        //public ActionResult GetEmployeeInfoByOwnersName(string name, int type)
        //{
        //    var list = EmployeeInfoBLL.GetEmployeeInfoByOwnersName(name, type,CurrentStrucID);
        //    if (list == null) {
        //        list = new List<NewEmployeeInfoDDLModel>();
        //    }
        //    List<dynamic> resultList = new List<dynamic>();
        //    foreach (var item in list)
        //    {
        //        resultList.Add(new { label = item.EmployeeName, value = item.EmployeeName, ID = item.ID });
        //    }
        //    return Json(resultList, JsonRequestBehavior.AllowGet);
        //}
        #endregion


        #region 针对更换车机的配置推送
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("TerminalSettingsIssued")]
        public async Task<ActionResult> SendParameter(int VehicleID, string TerminalCode, string PlateNum)
        {
            var sendmodel = VehicleBLL.SendParameter(VehicleID);
            //针对无使用记录的车辆绑定后读取不到上一次的车机的配置，只更新状态，不推送配置
            if (sendmodel == null)
            {
                bool updateresult = VehicleBLL.UpdateSendSuccessStatus(VehicleID);
                if (updateresult)
                {
                    return Json(new OperationResult()
                    {
                        Success = true,
                        Message = PromptInformation.TerminalSetting_UpdateTerminalSettingSuccess
                    });
                }
                else
                {
                    return Json(new OperationResult()
                    {
                        Success = false,
                        Message = PromptInformation.OperationFailure
                    });
                };
            }
            TerminalSettingsIssuedModel model = new TerminalSettingsIssuedModel()
            {
                超速持续时间 = sendmodel.MinimumDuration,
                超速报警预警差值 = sendmodel.OverspeedThreshold,
                当天累计驾驶时间门限 = sendmodel.DrivingTimeThreshold,
                连续驾驶时间门限 = sendmodel.ContinuousDrivingThreshold,
                最小休息时间 = sendmodel.MinimumBreakTime,
                最长停车时间 = sendmodel.MaximumParkingTime,
                车辆里程表读数 = sendmodel.LastMiles,
            };

            string wcfAddress = string.Empty;
            ActionResult result = null;
            if (!this.CheckWCF(TerminalCode, out result, out wcfAddress))
            {
                return result;
            }
            //记录错误信息
            string errorMsg = string.Empty;
            try
            {
                var syncResult = await this.DoSend(wcfAddress, TerminalCode, model).ConfigureAwait(false);
                //var syncResult = new OperationResultGeneralRep()
                //{
                //    Code = OperationResultCode.操作成功,
                //    Message = "测试",
                //    State = true
                //};
                string valuesSql = string.Empty;
                //日志打印是打印出类里面有参数的属性
                string setInfo = GetProperties<TerminalSettingsIssuedModel>(model);
                string wanIP = GetRemoteAddress();
                DateTime setDTime = DateTime.Now;
                string resultResponse = "Code:" + (int)(OperationResultCode)syncResult.Code + ";Message:" + syncResult.Message;
                valuesSql += string.Format("('{0}','{1}',{2},'{3}',{4},'{5}','{6}',{7},'{8}'),", TerminalCode, PlateNum, (byte)TerminalSettingTypeEnum.TerminalSetup_Update, setInfo, syncResult.State ? 1 : 0, resultResponse, wanIP, base.CurrentUserID, setDTime);
                if (!syncResult.State)
                {
                    return Json(new OperationResult()
                    {
                        Success = false,
                        Message = PromptInformation.TerminalSetting_UpdateTerminalSettingFail
                    });
                }
                else
                {
                    TerminalSettingsBLL.BatchInsertTerminalOperationsLog(valuesSql.TrimEnd(','));
                    //更新车辆的状态 status=8 ==》status=0
                    bool updateresult1 = VehicleBLL.UpdateSendSuccessStatus(VehicleID);
                    if (updateresult1)
                    {
                        return Json(new OperationResult()
                        {
                            Success = true,
                            Message = PromptInformation.TerminalSetting_UpdateTerminalSettingSuccess
                        });
                    }
                    else
                    {
                        return Json(new OperationResult()
                       {
                           Success = false,
                           Message = PromptInformation.OperationFailure
                       });
                    };
                }
            }
            catch (Exception e)
            {
                return Json(new OperationResult()
                {
                    Success = false,
                    Message = PromptInformation.RemotingError
                });
            }
        }
        private async Task<OperationResultGeneralRep> DoSend(string wcfaddress, string TerminalCode, TerminalSettingsIssuedModel model)
        {
            var currentWCFAddr = wcfaddress;
            var client = base.GetTerminalOperationClient(currentWCFAddr);
            return await SyncSetTerminalParas(client, TerminalCode, model);
        }

        private bool CheckWCF(string TerminalCode, out ActionResult result, out string wcfaddress)
        {
            result = null;
            wcfaddress = TerminalSettingsBLL.GetTerminalOfWCFAddress(TerminalCode);
            if (wcfaddress == null || wcfaddress == "")
            {
                result = Json(new { Success = false, Message = PromptInformation.TerminalSetting_WCFError });
                return false;
            }
            return true;
        }

        private async Task<OperationResultGeneralRep> SyncSetTerminalParas(TerminalOperationClient client, string terminalCode, TerminalSettingsIssuedModel model)
        {
            TerminalParasData paraItems = new TerminalParasData();
            if (model.车牌颜色.HasValue)
            {
                paraItems.车牌颜色 = (PlateColor)model.车牌颜色.Value;
            }
            paraItems.终端心跳发送间隔 = model.终端心跳发送间隔;
            paraItems.主服务器IP地址或域名 = model.主服务器IP地址或域名;
            paraItems.备份服务器IP地址或域名 = model.备份服务器IP地址或域名;
            paraItems.服务器TCP端口 = model.服务器TCP端口;
            paraItems.休眠时汇报时间间隔 = model.休眠时汇报时间间隔;
            paraItems.紧急报警时汇报时间间隔 = model.紧急报警时汇报时间间隔;
            paraItems.缺省时间汇报间隔 = model.缺省时间汇报间隔;
            paraItems.最高速度 = model.最高速度;
            paraItems.超速持续时间 = model.超速持续时间;
            paraItems.连续驾驶时间门限 = model.连续驾驶时间门限;
            paraItems.当天累计驾驶时间门限 = model.当天累计驾驶时间门限;
            paraItems.最小休息时间 = model.最小休息时间;
            paraItems.最长停车时间 = model.最长停车时间;
            paraItems.超速报警预警差值 = model.超速报警预警差值;
            paraItems.疲劳驾驶预警差值 = model.疲劳驾驶预警差值;
            paraItems.车辆里程表读数 = model.车辆里程表读数;
            paraItems.公安交通管理部门颁发的机动车号牌 = model.公安交通管理部门颁发的机动车号牌;

            return await client.SetTerminalParasAsync(terminalCode, new TerminalParasSettingData()
            {
                ParaItems = paraItems
            });

        }
        #endregion


    }
}

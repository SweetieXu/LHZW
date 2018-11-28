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
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class TerminalController : BaseController
    {


        #region 查询
        public ActionResult TerminalSetting()
        {
            SearchDataWithPagedDatas<TerminalSearchModel, TerminalListModel> model =
                new SearchDataWithPagedDatas<TerminalSearchModel, TerminalListModel>();

            model.SearchModel = new TerminalSearchModel();
            model.SearchModel.TerminalManufacturerID = -1;
            model.SearchModel.TerminalManufacturerSelectList =
                TerminalManufacturerBLL.GetTerminalManufacturers().
                ToSelectListWithAll(m => GetSelectListItem(m.ID, m.ManufacturerName));



            model.SearchModel.TerminalTypeID = -1;
            model.SearchModel.TerminalTypeSelectList =
                TerminalTypeBLL.GetTerminalTypes().
                ToSelectListWithAll(m => GetSelectListItem(m.ID, m.TerminalName));

            model.PagedDatas = TerminalBLL.GetPagedTerminals(model.SearchModel, 1, this.PageSize);
            return PartialView("_TerminalSetting", model);
        }


             [AsiatekSubordinateFunction("TerminalSetting")]
        public ActionResult GetTerminalInfo(TerminalSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<TerminalSearchModel, TerminalListModel> result =
       new SearchDataWithPagedDatas<TerminalSearchModel, TerminalListModel>();
            result.SearchModel = model;
            result.PagedDatas = TerminalBLL.GetPagedTerminals(model, searchPage, this.PageSize);
            return PartialView("_TerminalPagedGrid", result);
        }

        #endregion

        #region 新增

        #region 注释代码
        //public ActionResult AddTerminal()
        //{
        //    TerminalAddModel model = new TerminalAddModel();
        //    model.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
        //    model.TerminalTypeSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypes(), "ID", "TerminalName");
        //    return PartialView("_AddTerminal", model);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult AddTerminal(TerminalAddModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = TerminalBLL.AddTerminal(model);
        //        base.DoLog(OperationTypeEnum.Add, result, "TerminalCode:" + model.TerminalCode);
        //        return Json(result);
        //    }
        //    else
        //    {
        //        model.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
        //        model.TerminalTypeSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypes(), "ID", "TerminalName");
        //        return PartialView("_AddTerminal", model);
        //    }
        //}
        #endregion

        public ActionResult AddTerminal()
        {
            TerminalAddModel model = new TerminalAddModel();
            var list = TerminalManufacturerBLL.GetTerminalManufacturers();
            model.TerminalManufacturerSelectList = new SelectList(list, "ID", "ManufacturerName");
            var tmid = 0;
            if (list != null && list.Count != 0)
            {
                tmid = list.First().ID;
            }
            model.TerminalTypeSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypesByTMID(tmid), "ID", "TerminalName");
            var serverInfoList = ServerManagerBLL.GetServerInfoList();
            if (serverInfoList != null && serverInfoList.Count != 0)
            {
                model.ServerInfoID = serverInfoList.First().ID;
            }
            model.ServerInfoSelectList = new SelectList(serverInfoList, "ID", "ServerName");
            #region 默认值
            model.ContinuousDrivingThreshold = 14400;
            model.DrivingTimeThreshold = 57600;
            model.MaximumParkingTime = 3600;
            model.MinimumBreakTime = 1200;
            model.MinimumDuration = 5;
            #endregion

            return PartialView("_AddTerminal", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddTerminal(TerminalAddModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreateUserID = base.UserIDForLog;
                var result = TerminalBLL.AddTerminal(model);
                base.DoLog(OperationTypeEnum.Add, result, "TerminalCode:" + model.TerminalCode);
                return Json(result);
            }
            else
            {
                var list = TerminalManufacturerBLL.GetTerminalManufacturers();
                model.TerminalManufacturerSelectList = new SelectList(list, "ID", "ManufacturerName");
                var tmid = 0;
                if (list != null && list.Count != 0)
                {
                    tmid = list.First().ID;
                }
                model.TerminalTypeSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypesByTMID(tmid), "ID", "TerminalName");
                var serverInfoList = ServerManagerBLL.GetServerInfoList();
                if (serverInfoList != null && serverInfoList.Count != 0)
                {
                    model.ServerInfoID = serverInfoList.First().ID;
                }
                model.ServerInfoSelectList = new SelectList(serverInfoList, "ID", "ServerName");
                #region 默认值
                model.ContinuousDrivingThreshold = 144000;
                model.DrivingTimeThreshold = 57600;
                model.MaximumParkingTime = 3600;
                model.MinimumBreakTime = 1200;
                model.MinimumDuration = 5;
                #endregion
                return PartialView("_AddTerminal", model);
            }
        }



        [HttpPost, AsiatekSubordinateFunction("AddTerminal")]
        public ActionResult CheckAddTerminalCodeExists(string terminalCode)
        {
            return Json(!TerminalBLL.CheckTerminalCodeExists(terminalCode));
        }

        [HttpPost, AsiatekSubordinateFunction("AddTerminal")]
        public ActionResult CheckAddSIMCodeExists(string SIMCode)
        {
            return Json(!TerminalBLL.CheckSIMCodeExists(SIMCode));
        }



        [HttpPost, AsiatekSubordinateFunction("AddTerminal")]
        public ActionResult CheckAddVehicleIDExists(string vehicleID)
        {
            return Json(!TerminalBLL.CheckVehicleIDExists(vehicleID));
        }

        #endregion

        #region 编辑
        #region 注释代码
        //public ActionResult EditTerminal(long id)
        //{
        //    var result = TerminalBLL.GetTerminalByID(id);
        //    if (result.DataResult == null)
        //    {
        //        return Content(result.Message);
        //    }
        //    var model = result.DataResult;
        //    model.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
        //    model.TerminalTypeSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypes(), "ID", "TerminalName");
        //    return PartialView("_EditTerminal", model);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult EditTerminal(TerminalEditModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = TerminalBLL.EditTerminal(model);
        //        base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
        //        return Json(result);
        //    }
        //    else
        //    {
        //        model.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
        //        model.TerminalTypeSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypes(), "ID", "TerminalName");
        //        return PartialView("_EditTerminal", model);
        //    }
        //}
        #endregion


        public ActionResult EditTerminal(long id)
        {
            var result = TerminalBLL.GetTerminalByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            model.SIMCodeIDOld = model.SIMCodeID == null ? null :(int?) model.SIMCodeID.Value;
            model.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
            model.TerminalTypeSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypesByTMID(model.TerminalManufacturerID), "ID", "TerminalName");
            model.ServerInfoSelectList = new SelectList(ServerManagerBLL.GetServerInfoList(), "ID", "ServerName");
            return PartialView("_EditTerminal", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditTerminal(TerminalEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.EditUserID = base.UserIDForLog;
                var result = TerminalBLL.EditTerminal(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                model.TerminalManufacturerSelectList = new SelectList(TerminalManufacturerBLL.GetTerminalManufacturers(), "ID", "ManufacturerName");
                model.TerminalTypeSelectList = new SelectList(TerminalTypeBLL.GetTerminalTypesByTMID(model.TerminalManufacturerID), "ID", "TerminalName");
                model.ServerInfoSelectList = new SelectList(ServerManagerBLL.GetServerInfoList(), "ID", "ServerName");
                return PartialView("_EditTerminal", model);
            }
        }


        [HttpPost, AsiatekSubordinateFunction("EditTerminal")]
        public ActionResult CheckEditTerminalCodeExists(string terminalCode, long id)
        {
            return Json(!TerminalBLL.CheckTerminalCodeExists(terminalCode, id));
        }

        [HttpPost, AsiatekSubordinateFunction("EditTerminal")]
        public ActionResult CheckEditSIMCodeExists(string SIMCode, long id)
        {
            return Json(!TerminalBLL.CheckSIMCodeExists(SIMCode, id));
        }



        [HttpPost, AsiatekSubordinateFunction("EditTerminal")]
        public ActionResult CheckEditVehicleIDExists(string vehicleID, long id)
        {
            return Json(!TerminalBLL.CheckVehicleIDExists(vehicleID, id));
        }
        #endregion

        #region 报废
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ScrapTerminal(FormCollection fc)
        {
            string[] ids = fc["tid"].Split(',');

            var result = TerminalBLL.ScrapTerminal(ids);

            base.DoLog(OperationTypeEnum.Edit, result, fc["tid"]);
            return Json(result);
        }
        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("TerminalSetting")]
        public ActionResult DeleteTerminal(int[] idList)
        {
            var result = TerminalBLL.DeleteTerminals(idList);
            string idListStr = string.Empty;
            foreach (var item in idList)
            {
                idListStr += item + ",";
            }
            base.DoLog(OperationTypeEnum.Delete, result, "idList:" + idListStr);
            return Json(result);
        }
        #endregion

        #region 解除绑定
        [HttpPost, ValidateAntiForgeryToken]
       [AsiatekSubordinateFunction("TerminalSetting")]
        public ActionResult UnBind(int TerminalID,int VehicleID,string VIN ,string TerminalCode,string SimCode)
        {
            TerminalUsageLogs oldterminal = new TerminalUsageLogs()
            {
                TerminalID = TerminalID,
                VehicleID = VehicleID,
                VIN = VIN,
                TerminalCode = TerminalCode,
                SimCode = SimCode
            };
            var result = TerminalBLL.UnBind(oldterminal,base.UserIDForLog);
            base.DoLog(OperationTypeEnum.Edit, result, "设备ID:"+oldterminal.TerminalID + "||车辆ID:" + oldterminal.VehicleID+"||车辆VIN"+oldterminal.VIN);
            return Json(result);
        }
        #endregion

        #region 获取所有未使用未删除的SIM卡列表
        /// <summary>
        /// 获取所有未使用未删除的SIM卡列表
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("AddTerminal")]
        [AsiatekSubordinateFunction("EditTerminal")]
        public ActionResult GetNotUserdSimCodeList(string simCode, int? simCodeId)
        {
            var list = SimCodeBLL.GetNotUserdSimCodeList(simCode, simCodeId);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.SimCode, value = item.SimCode, VID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 导入
        [HttpPost, AsiatekSubordinateFunction("TerminalSetting")]
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
                string dirPath = Path.Combine("~/UploadFiles/TerminalExcels/", DateTime.Now.ToShortDateString());
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

            return Json(TerminalBLL.ImportTerminals(filePath, "Terminal", base.CurrentUserID));
        }
        #endregion
    }
}

using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System.Collections.Generic;
using System.Web.Mvc;
using Asiatek.TMS.Helpers;
using Asiatek.Resource;
using System.Text.RegularExpressions;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class EmployeeInfoController : BaseController
    {
        #region 查询  已注释
        //public ActionResult EmployeeInfoSetting()
        //{
        //    SearchDataWithPagedDatas<EmployeeInfoSearchModel, EmployeeInfoListModel> model = new SearchDataWithPagedDatas<EmployeeInfoSearchModel, EmployeeInfoListModel>();
        //    model.SearchModel = new EmployeeInfoSearchModel();

        // model.SearchModel.DriveLicenseStateSelectList = EmployeeInfoBLL.GetDriveLicenseStates().ToSelectListWithAll(m => GetSelectListItem(m.DriveLicenseStateID, m.DriveLicenseStateName));
        //    model.SearchModel.DriveLicenseStateID = -1;

        //    model.PagedDatas = EmployeeInfoBLL.GetPagedEmployeeInfos(model.SearchModel, 1, this.PageSize);
        //    return PartialView("_EmployeeInfoSetting", model);
        //}


        //[AsiatekSubordinateFunction("EmployeeInfoSetting")]
        //public ActionResult GetEmployeeInfo(EmployeeInfoSearchModel model, int searchPage)
        //{
        //    SearchDataWithPagedDatas<EmployeeInfoSearchModel, EmployeeInfoListModel> result = new SearchDataWithPagedDatas<EmployeeInfoSearchModel, EmployeeInfoListModel>();
        //    result.SearchModel = model;
        //    result.PagedDatas = EmployeeInfoBLL.GetPagedEmployeeInfos(result.SearchModel, searchPage, this.PageSize);
        //    return PartialView("_EmployeeInfoPagedGrid", result);
        //}
        #endregion

        #region 查询 新版
        public ActionResult EmployeeInfoSetting()
        {
            SearchDataWithPagedDatas<EmployeeInfoFindModel, EmployeeInfoPageModel> model = new SearchDataWithPagedDatas<EmployeeInfoFindModel, EmployeeInfoPageModel>();
            model.SearchModel = new EmployeeInfoFindModel();
            model.SearchModel.IsCarriers = -1;
            model.SearchModel.IsDrivers = -1;
            //model.PagedDatas = EmployeeInfoBLL.GetPagedEmployeeInfo(model.SearchModel, 1, this.PageSize);
            model.PagedDatas = EmployeeInfoBLL.GetPagedEmployeeInfo_New(model.SearchModel, 1, this.PageSize,base.CurrentStrucID);
            return PartialView("_EmployeeInfoSetting", model);
        }


        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult GetEmployeeInfo(EmployeeInfoFindModel model, int searchPage)
        {
            SearchDataWithPagedDatas<EmployeeInfoFindModel, EmployeeInfoPageModel> result = new SearchDataWithPagedDatas<EmployeeInfoFindModel, EmployeeInfoPageModel>();
            result.SearchModel = model;
            //result.PagedDatas = EmployeeInfoBLL.GetPagedEmployeeInfo(result.SearchModel, searchPage, this.PageSize);
            result.PagedDatas = EmployeeInfoBLL.GetPagedEmployeeInfo_New(result.SearchModel, searchPage, this.PageSize,base.CurrentStrucID);
            return PartialView("_EmployeeInfoPagedGrid", result);
        }
        #endregion

        #region 新增
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult AddEmployeeInfo()
        {
            EmployeeInfoAddModel model = new EmployeeInfoAddModel();
            model.CertificateTypeSelectList = new SelectList(EmployeeInfoBLL.GetCertificateTypes(), "CertificateTypeID", "CertificateTypeName");

            SelectList driveLicenseStateSelectList = new SelectList(EmployeeInfoBLL.GetDriveLicenseStates(), "DriveLicenseStateID", "DriveLicenseStateName");
            List<SelectListItem> liDriveLicenseState = new List<SelectListItem>();
            liDriveLicenseState.Add(new SelectListItem { Text = DisplayText.PleaseSelect, Value = "0", Selected = true });
            liDriveLicenseState.AddRange(driveLicenseStateSelectList);
            model.DriveLicenseStateSelectList = liDriveLicenseState;


            SelectList driveTypeSelectList = new SelectList(EmployeeInfoBLL.GetDriveTypes(), "DriveTypeID", "DriveTypeName");
            List<SelectListItem> liDriveType = new List<SelectListItem>();
            liDriveType.Add(new SelectListItem { Text = DisplayText.PleaseSelect, Value = "0", Selected = true });
            liDriveType.AddRange(driveTypeSelectList);
            model.DriveTypeSelectList = liDriveType;

            return PartialView("_AddEmployeeInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult AddEmployeeInfo(EmployeeInfoAddModel model)
        {
            //if (model.CertificateTypeID == 1)
            //{
            //    bool isIDcard = IsIDcard(model.CertificateCode);
            //    if (!isIDcard) {
            //        ModelState.AddModelError("CertificateCode", DataAnnotations.IDcardError);
            //    }
            //}
            //if (model.IsDriver == true && string.IsNullOrEmpty(model.DriveCode))
            //{
            //    ModelState.AddModelError("DriveCode", DataAnnotations.NeedDriveCode);
            //}
            //if (model.IsDriver == true && string.IsNullOrEmpty(model.DriveCodeValidTime))
            //{
            //    ModelState.AddModelError("DriveCodeValidTime", DataAnnotations.MustInputDriveCodeValidTime);
            //}


            if (model.IsDriver == true && model.DriveLicenseStateID == 0)
            {
                ModelState.AddModelError("DriveLicenseStateID", DataAnnotations.MustInputDriveLicenseStateID);
            }
            //if (model.IsDriver == true && model.DriveTypeID.Equals("0"))
            //{
            //    ModelState.AddModelError("DriveTypeID", DataAnnotations.MustInputDriveTypeID);
            //}


            //if (model.IsCarrier == true && string.IsNullOrEmpty(model.EmergePhone))
            //{
            //    ModelState.AddModelError("EmergePhone", DataAnnotations.NeedEmergePhone);
            //}
            //if (model.IsCarrier == true && string.IsNullOrEmpty(model.CarrierCode))
            //{
            //    ModelState.AddModelError("CarrierCode", DataAnnotations.NeedCarrierCode);
            //}
            if (ModelState.IsValid)
            {
                var result = EmployeeInfoBLL.AddEmployeeInfo(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Add, result, "EmployeeName:" + model.EmployeeName);
                return Json(result);
            }
            else
            {
                model.CertificateTypeSelectList = new SelectList(EmployeeInfoBLL.GetCertificateTypes(), "CertificateTypeID", "CertificateTypeName");

                SelectList driveLicenseStateSelectList = new SelectList(EmployeeInfoBLL.GetDriveLicenseStates(), "DriveLicenseStateID", "DriveLicenseStateName");
                List<SelectListItem> liDriveLicenseState = new List<SelectListItem>();
                liDriveLicenseState.Add(new SelectListItem { Text = DisplayText.PleaseSelect, Value = "0", Selected = true });
                liDriveLicenseState.AddRange(driveLicenseStateSelectList);
                model.DriveLicenseStateSelectList = liDriveLicenseState;


                SelectList driveTypeSelectList = new SelectList(EmployeeInfoBLL.GetDriveTypes(), "DriveTypeID", "DriveTypeName");
                List<SelectListItem> liDriveType = new List<SelectListItem>();
                liDriveType.Add(new SelectListItem { Text = DisplayText.PleaseSelect, Value = "0", Selected = true });
                liDriveType.AddRange(driveTypeSelectList);
                model.DriveTypeSelectList = liDriveType;

                return PartialView("_AddEmployeeInfo", model);
            }
        }


        /// <summary>
        /// 验证身份证号
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        public bool IsIDcard(string idcard)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(idcard, @"^(^\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$");
        }


        /// <summary>
        /// 检查工号是否重复
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult CheckAddEmployeeIDExists(string employeeID)
        {
            return Json(!EmployeeInfoBLL.CheckAddEmployeeIDExists(employeeID));
        }

        /// <summary>
        /// 证件类型为身份证时,检查身份证号是否重复
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult CheckAddIDCardIsExists(string idCard)
        {
            return Json(!EmployeeInfoBLL.CheckAddIDCardIsExists(idCard), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 修改
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult EditEmployeeInfo(int id)
        {
            var result = EmployeeInfoBLL.GetEmployeeInfoByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            model.CertificateTypeSelectList = new SelectList(EmployeeInfoBLL.GetCertificateTypes(), "CertificateTypeID", "CertificateTypeName");

            SelectList driveLicenseStateSelectList = new SelectList(EmployeeInfoBLL.GetDriveLicenseStates(), "DriveLicenseStateID", "DriveLicenseStateName");
            List<SelectListItem> liDriveLicenseState = new List<SelectListItem>();
            liDriveLicenseState.Add(new SelectListItem { Text = DisplayText.PleaseSelect, Value = "0" });
            liDriveLicenseState.AddRange(driveLicenseStateSelectList);
            model.DriveLicenseStateSelectList = liDriveLicenseState;


            SelectList driveTypeSelectList = new SelectList(EmployeeInfoBLL.GetDriveTypes(), "DriveTypeID", "DriveTypeName");
            List<SelectListItem> liDriveType = new List<SelectListItem>();
            liDriveType.Add(new SelectListItem { Text = DisplayText.PleaseSelect, Value = "0" });
            liDriveType.AddRange(driveTypeSelectList);
            model.DriveTypeSelectList = liDriveType;

            return PartialView("_EditEmployeeInfo", result.DataResult);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult EditEmployeeInfo(EmployeeInfoEditModel model)
        {
            //if (model.CertificateTypeID == 1)
            //{
            //    bool isIDcard = IsIDcard(model.CertificateCode);
            //    if (!isIDcard)
            //    {
            //        ModelState.AddModelError("CertificateCode", DataAnnotations.IDcardError);
            //    }
            //}
            //if (model.IsDriver == true && string.IsNullOrEmpty(model.DriveCode))
            //{
            //    ModelState.AddModelError("DriveCode", DataAnnotations.NeedDriveCode);
            //}
            //if (model.IsDriver == true && string.IsNullOrEmpty(model.DriveCodeValidTime))
            //{
            //    ModelState.AddModelError("DriveCodeValidTime", DataAnnotations.MustInputDriveCodeValidTime);
            //}


            if (model.IsDriver == true && model.DriveLicenseStateID == 0)
            {
                ModelState.AddModelError("DriveLicenseStateID", DataAnnotations.MustInputDriveLicenseStateID);
            }
            //if (model.IsDriver == true && model.DriveTypeID.Equals("0"))
            //{
            //    ModelState.AddModelError("DriveTypeID", DataAnnotations.MustInputDriveTypeID);
            //}


            //if (model.IsCarrier == true && string.IsNullOrEmpty(model.EmergePhone))
            //{
            //    ModelState.AddModelError("EmergePhone", DataAnnotations.NeedEmergePhone);
            //}
            //if (model.IsCarrier == true && string.IsNullOrEmpty(model.CarrierCode))
            //{
            //    ModelState.AddModelError("CarrierCode", DataAnnotations.NeedCarrierCode);
            //}
            if (ModelState.IsValid)
            {
                var result = EmployeeInfoBLL.EditEmployeeInfo(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "EmployeeID:" + model.EmployeeID);
                return Json(result);
            }
            else
            {
                model.CertificateTypeSelectList = new SelectList(EmployeeInfoBLL.GetCertificateTypes(), "CertificateTypeID", "CertificateTypeName");

                SelectList driveLicenseStateSelectList = new SelectList(EmployeeInfoBLL.GetDriveLicenseStates(), "DriveLicenseStateID", "DriveLicenseStateName");
                List<SelectListItem> liDriveLicenseState = new List<SelectListItem>();
                liDriveLicenseState.Add(new SelectListItem { Text = DisplayText.PleaseSelect, Value = "0" });
                liDriveLicenseState.AddRange(driveLicenseStateSelectList);
                model.DriveLicenseStateSelectList = liDriveLicenseState;


                SelectList driveTypeSelectList = new SelectList(EmployeeInfoBLL.GetDriveTypes(), "DriveTypeID", "DriveTypeName");
                List<SelectListItem> liDriveType = new List<SelectListItem>();
                liDriveType.Add(new SelectListItem { Text = DisplayText.PleaseSelect, Value = "0" });
                liDriveType.AddRange(driveTypeSelectList);
                model.DriveTypeSelectList = liDriveType;

                return PartialView("_EditEmployeeInfo", model);
            }
        }

        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult CheckEditIDCardIsExists(string idCard, string employeeID)
        {
            return Json(!EmployeeInfoBLL.CheckEditIDCardIsExists(idCard, employeeID), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult DeleteEmployeeInfo(FormCollection fc)
        {
            string[] ids = fc["empid"].Split(',');

            //var result = EmployeeInfoBLL.DeleteEmployeeInfo(ids);
            var result = EmployeeInfoBLL.DeleteEmployeeInfoPhysical(ids);

            base.DoLog(OperationTypeEnum.Delete, result, fc["empid"]);
            return Json(result);
        }

        #endregion

        #region 员工信息新增修改新版本

        #region 员工信息新增初始化页面
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult AddEmployeeInfoNew()
        {
            EditEmployeeInfoModel model = new EditEmployeeInfoModel();       
            // 证件类型
            model.CertificateTypeSelectList = new SelectList(EmployeeInfoBLL.GetCertificateTypes(), "CertificateTypeID", "CertificateTypeName");
            // 默认选中驾驶员
            model.IsDriver = true;

            return PartialView("_AddEmployeeInfoNew", model);
        }
        #endregion

        #region 员工信息新增
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult AddEmployeeInfoNew(EditEmployeeInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var result = EmployeeInfoBLL.AddEmployeeInfoNew(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Add, result, "EmployeeName:" + model.EmployeeName);
                return Json(result);
            }
            else
            {
                // 证件类型
                model.CertificateTypeSelectList = new SelectList(EmployeeInfoBLL.GetCertificateTypes(), "CertificateTypeID", "CertificateTypeName");
                return PartialView("_AddEmployeeInfoNew", model);
            }
        }
        #endregion

        #region 员工信息修改初始化页面
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult EditEmployeeInfoNew(int id)
        {
            var result = EmployeeInfoBLL.GetNewEmployeeInfoByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            var model = result.DataResult;
            // 证件类型
            model.CertificateTypeSelectList = new SelectList(EmployeeInfoBLL.GetCertificateTypes(), "CertificateTypeID", "CertificateTypeName");
            //  获取当前员工已经被哪些车使用过 如果对应的员工类型已经关联了相关的车辆 单位信息就不能修改
            List<VehicleEmployeeInfoDDLModel> list = EmployeeInfoBLL.GetEmployeeInfoUsedToVehicle(model.ID);
            model.OldStrucID = model.StrucID.Value;

            ViewBag.IsUpdateStruc = 1;
            if (list != null && list.Count > 0)
            {
                ViewBag.IsUpdateStruc = 0;
            }
            return PartialView("_EditEmployeeInfoNew", result.DataResult);
        }

        #endregion

        #region 员工信息修改
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult EditEmployeeInfoNew(EditEmployeeInfoModel model)
        {
            //  获取当前员工已经被哪些车使用过 如果对应的员工类型已经关联了相关的车辆 则不能取消绑定
            List<VehicleEmployeeInfoDDLModel> list = EmployeeInfoBLL.GetEmployeeInfoUsedToVehicle(model.ID);
            if (list != null && list.Count > 0)
            {
                if (!model.IsDriver)
                {
                    if (list.Find(o => o.Type == 1) != null)
                    {
                        ModelState.AddModelError("DriveCode", DataAnnotations.DriverError);
                    }
                }

                if (!model.IsCarrier && ModelState.IsValid)
                {
                    if (list.Find(o => o.Type == 2) != null)
                    {
                        ModelState.AddModelError("DriveCode", DataAnnotations.CarrierError);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var result = EmployeeInfoBLL.EditEmployeeInfoNew(model, base.CurrentUserID);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                // 证件类型
                model.CertificateTypeSelectList = new SelectList(EmployeeInfoBLL.GetCertificateTypes(), "CertificateTypeID", "CertificateTypeName");
                return PartialView("_EditEmployeeInfoNew", model);
            }
        }
        #endregion


        /// <summary>
        /// 验证证件号是否重复
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        [HttpPost, AsiatekSubordinateFunction("EmployeeInfoSetting")]
        public ActionResult CheckEditCertificateCodeExists(string certificateCode, int id = 0)
        {
            return Json(!EmployeeInfoBLL.CheckEditCertificateCodeExists(certificateCode, id));
        }

        ///// <summary>
        ///// 检查工号是否重复
        ///// </summary>
        ///// <param name="employeeID"></param>
        ///// <returns></returns>
        //[HttpPost, AsiatekSubordinateFunction("EmployeeInfoSetting")]
        //public ActionResult CheckEditEmployeeIDExists(string employeeID, int id = 0)
        //{
        //    return Json(!EmployeeInfoBLL.CheckEditEmployeeIDExists(employeeID,id));       
        //}
        #endregion
    }
}

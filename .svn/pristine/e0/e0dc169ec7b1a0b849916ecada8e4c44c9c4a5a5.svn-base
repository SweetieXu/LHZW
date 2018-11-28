using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.TMS.Helpers;
using Asiatek.TMS.Filters;
using Asiatek.Resource;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Reflection;
using Asiatek.Common;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class StructureController : BaseController
    {


        #region 查询

        #region 旧代码
        //public ActionResult StructureSetting()
        //{
        //    StructureSettingModel model = new StructureSettingModel();
        //    model.IsCascaded = true;
        //    model.InspectTypeAny = true;
        //    model.ExNoticeTypeAny = true;
        //    model.PagedStructures = StructureBLL.GetPagedStructures(model, this.PageSize);
        //    return PartialView("_StructureSetting", model);
        //}

        //private ActionResult GetStructurePagedGridPV(StructureSettingModel model, int currentPage)
        //{
        //    model.PagedStructures = StructureBLL.GetPagedStructures(model, this.PageSize, currentPage);
        //    return PartialView("_StructureSetting", model);
        //}


        //public ActionResult StructurePagedGrid(StructureSettingModel model, int currentPage)
        //{
        //    return GetStructurePagedGridPV(model, currentPage);
        //}


        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult GetStructureInfo(StructureSettingModel model)
        //{
        //    return GetStructurePagedGridPV(model, 1);
        //}
        #endregion
        public ActionResult StructureSetting()
        {
            StructureSettingModel model = new StructureSettingModel();
            model.IsCascaded = false;
            model.InspectTypeAny = true;
            model.ExNoticeTypeAny = true;
            model.SearchPage = 1;
            model.Nightban = -1;
            model.PagedDatas = StructureBLL.GetPagedStructures(model, this.PageSize);
            return PartialView("_StructureSetting", model);
        }

        private ActionResult GetStructurePagedGridPV(StructureSettingModel model)
        {
            model.PagedDatas = StructureBLL.GetPagedStructures(model, this.PageSize);
            return PartialView("_StructurePagedGrid", model);
        }


        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult GetStructureInfo(StructureSettingModel model)
        {
            return GetStructurePagedGridPV(model);
        }

        /// <summary>
        /// 获取JSON格式单位下拉列表数据
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("UserSetting", "User", "Admin")]
        [AsiatekSubordinateFunction("AddUserInfo", "User", "Admin")]
        [AsiatekSubordinateFunction("EditUserInfo", "User", "Admin")]
        [AsiatekSubordinateFunction("VehicleSetting", "Vehicle", "Admin")]
        [AsiatekSubordinateFunction("NewEditVehicle", "Vehicle", "Admin")]
        [AsiatekSubordinateFunction("NewAddVehicle", "Vehicle", "Admin")]
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.Server, Duration = Int32.MaxValue, SqlDependency = "asiatekCache:Structures")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult GetStructures()
        {
            base.FixVaryBug();
            return Json(StructureBLL.GetStructures(), JsonRequestBehavior.AllowGet);
        }

        [AsiatekSubordinateFunction("UserSetting", "User", "Admin")]
        [AsiatekSubordinateFunction("AddUserInfo", "User", "Admin")]
        [AsiatekSubordinateFunction("EditUserInfo", "User", "Admin")]
        [AsiatekSubordinateFunction("VehicleSetting", "Vehicle", "Admin")]
        [AsiatekSubordinateFunction("NewEditVehicle", "Vehicle", "Admin")]
        [AsiatekSubordinateFunction("NewAddVehicle", "Vehicle", "Admin")]
        [AsiatekSubordinateFunction("SearchSimCode", "SimCode", "Admin")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult GetStructuresByStructureName(string structuresName)
        {
            base.FixVaryBug();
            var list = StructureBLL.GetStructuresByStructureName(structuresName);
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { value = item.StrucName, label = item.StrucName, ID = item.ID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        [AsiatekSubordinateFunction("EmployeeInfoSetting", "EmployeeInfo", "Admin")]
        public ActionResult GetStrucListByUserID()
        {
            int uid = base.CurrentUserID;
            return Json(StructureBLL.GetStructuresByUserID(uid), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 新增
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult AddStructureInfo()
        {
            StructureAddModel model = new StructureAddModel();
            model.InspectType1 = true;
            model.ExNoticeType1 = true;
            model.MapType1 = true;
            model.MapType2 = true;
            model.MapType3 = true;
            //model.ParentStructureSelectList = new SelectList(StructureBLL.GetStructures(), "ID", "StrucName");
            //model.ParentStructureSelectList = StructureBLL.GetStructures().ToSelectListWithEmpty(m => GetSelectListItem(m.ID, m.StrucName));
            return PartialView("_AddStructureInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult AddStructureInfo(StructureAddModel model)
        {
            //暂时没有做相应的验证特性，进行手动验证
            //一个地图类型都没有选
            if (!(model.MapType1 || model.MapType2 || model.MapType3))
            {
                ModelState.AddModelError("MapType1", DataAnnotations.StructureMustSelectMapType);
            }
            //上传了LOGO文件则验证logo格式与大小 只能是png或jpg 最大1M
            if (model.LogoFile != null)
            {

                Regex reg = new Regex(@"\.(png|PNG|jpg|JPG|JPEG|jpeg)$");

                if (!reg.IsMatch(model.LogoFile.FileName))
                {
                    ModelState.AddModelError("LogoFile", DataAnnotations.LogoTypeError);
                }
                else if (model.LogoFile.ContentLength > 1024 * 1024)
                {
                    ModelState.AddModelError("LogoFile", DataAnnotations.LogoSizeError);
                }
            }


            if (ModelState.IsValid)
            {
                if (model.LogoFile != null)
                {
                    //将图片转换为字节数组
                    int logoLength = model.LogoFile.ContentLength;
                    byte[] logoBytes = new byte[logoLength];
                    model.LogoFile.InputStream.Read(logoBytes, 0, logoLength);
                    model.Logo = logoBytes;
                }
                var result = StructureBLL.AddStructure(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Add, result, "StrucName:" + model.StrucName);
                return Json(result);
            }
            else
            {
                //model.ParentStructureSelectList = new SelectList(StructureBLL.GetStructures(), "StrucCode", "StrucName");
                return PartialView("_AddStructureInfo", model);
            }
        }

        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult AddSubStrucToCurrentStruc(int parentID, string parentStrucName)
        {

            StructureAddSubModel model = new StructureAddSubModel();
            model.InspectType1 = true;
            model.ExNoticeType1 = true;
            model.MapType1 = true;
            model.MapType2 = true;
            model.MapType3 = true;
            model.ParentID = parentID;
            model.ParentStrucName = parentStrucName;

            return PartialView("_AddSubStructureInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult AddSubStrucToCurrentStruc(StructureAddSubModel model)
        {

            //暂时没有做相应的验证特性，进行手动验证
            //一个地图类型都没有选
            if (!(model.MapType1 || model.MapType2 || model.MapType3))
            {
                ModelState.AddModelError("MapType1", DataAnnotations.StructureMustSelectMapType);
            }
            //上传了LOGO文件则验证logo格式与大小 只能是png或jpg 最大1M
            if (model.LogoFile != null)
            {

                Regex reg = new Regex(@"\.(png|PNG|jpg|JPG|jpeg|jpeg)$");

                if (!reg.IsMatch(model.LogoFile.FileName))
                {
                    ModelState.AddModelError("LogoFile", DataAnnotations.LogoTypeError);
                }
                else if (model.LogoFile.ContentLength > 1024 * 1024)
                {
                    ModelState.AddModelError("LogoFile", DataAnnotations.LogoSizeError);
                }
            }


            if (ModelState.IsValid)
            {
                if (model.LogoFile != null)
                {
                    //将图片转换为字节数组
                    int logoLength = model.LogoFile.ContentLength;
                    byte[] logoBytes = new byte[logoLength];
                    model.LogoFile.InputStream.Read(logoBytes, 0, logoLength);
                    model.Logo = logoBytes;
                }


                var result = StructureBLL.AddStructure(model);
                base.DoLog(OperationTypeEnum.Add, result, "StrucName:" + model.StrucName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddSubStructureInfo", model);
            }
        }


        /// <summary>
        /// 检查添加单位时单位名称是否存在
        /// </summary>
        [HttpPost]
        [AsiatekSubordinateFunction("AddSubStrucToCurrentStruc")]
        [AsiatekSubordinateFunction("AddStructureInfo")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult CheckAddStrucNameExists(string strucName)
        {
            return Json(!StructureBLL.CheckStrucNameExists(strucName));
        }

        /// <summary>
        /// 检查添加单位时单位账号是否存在
        /// </summary>
        [HttpPost]
        [AsiatekSubordinateFunction("AddSubStrucToCurrentStruc")]
        [AsiatekSubordinateFunction("AddStructureInfo")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult CheckAddStrucAccountExists(string strucAccount)
        {
            return Json(!StructureBLL.CheckStrucAccountExists(strucAccount));
        }

        /// <summary>
        /// 检查新增时业户经营许可证号是否重复
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        [HttpPost]
        [AsiatekSubordinateFunction("AddSubStrucToCurrentStruc")]
        [AsiatekSubordinateFunction("AddStructureInfo")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult CheckAddStrucLicenseNoExists(string licenseNo)
        {
            if (string.IsNullOrWhiteSpace(licenseNo))
            {
                return Json(true);
            }
            return Json(!StructureBLL.CheckStrucLicenseNoExists(licenseNo));
        }

        /// <summary>
        /// 检查新增时自定义编码是否存在
        /// </summary>
        /// <param name="customEncoding"></param>
        /// <returns></returns>
        [HttpPost]
        [AsiatekSubordinateFunction("AddSubStrucToCurrentStruc")]
        [AsiatekSubordinateFunction("AddStructureInfo")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult CheckAddCustomEncodingExists(string customEncoding)
        {
            if (string.IsNullOrWhiteSpace(customEncoding))
            {
                return Json(true);
            }
            return Json(!StructureBLL.CheckAddCustomEncodingExists(customEncoding));
        }
        #endregion

        #region 编辑
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult EditStructureInfo(int id)
        {
            var obj = StructureBLL.GetStructureByID(id);
            if (obj.DataResult == null)
            {
                return Content(obj.Message);
            }
            var model = obj.DataResult;
            model.ModifyLogo = false;
            //model.ParentStructureSelectList = StructureBLL.GetStructures(id).ToSelectListWithEmpty(m => GetSelectListItem(m.ID, m.StrucName));
            return PartialView("_EditStructureInfo", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult EditStructureInfo(StructureEditModel model)
        {
            //暂时没有做相应的验证特性，进行手动验证
            //一个地图类型都没有选
            if (!(model.MapType1 || model.MapType2 || model.MapType3))
            {
                ModelState.AddModelError("MapType1", DataAnnotations.StructureMustSelectMapType);
            }
            //上传了LOGO文件则验证logo格式与大小 只能是png或jpg 最大1M
            if (model.LogoFile != null)
            {
                Regex reg = new Regex(@"\.(png|PNG|jpg|JPG|JPEG|jpeg)$");

                if (!reg.IsMatch(model.LogoFile.FileName))
                {
                    ModelState.AddModelError("LogoFile", DataAnnotations.LogoTypeError);
                }
                else if (model.LogoFile.ContentLength > 1024 * 1024)
                {
                    ModelState.AddModelError("LogoFile", DataAnnotations.LogoSizeError);
                }
            }
            if (ModelState.IsValid)
            {
                if (model.LogoFile != null)
                {
                    //将图片转换为字节数组
                    int logoLength = model.LogoFile.ContentLength;
                    byte[] logoBytes = new byte[logoLength];
                    model.LogoFile.InputStream.Read(logoBytes, 0, logoLength);
                    model.Logo = logoBytes;
                }
                var result = StructureBLL.EditStructure(model, base.UserIDForLog);
                base.DoLog(OperationTypeEnum.Edit, result, "StructureID:" + model.ID + "|StrucCode:" + model.StrucAccount);
                return Json(result);
            }
            else
            {
                //model.ParentStructureSelectList = StructureBLL.GetStructures(model.ID).ToSelectListWithEmpty(m => GetSelectListItem(m.ID, m.StrucName));
                model.ModifyLogo = false;
                return PartialView("_EditStructureInfo", model);
            }
        }

        /// <summary>
        /// 检查修改单位时单位名称是否存在
        /// </summary>
        [HttpPost, AsiatekSubordinateFunction("EditStructureInfo")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult CheckEditStrucNameExists(string strucName, int id)
        {
            return Json(!StructureBLL.CheckStrucNameExists(strucName, id));
        }



        /// <summary>
        /// 检查修改时业户经营许可证号是否重复
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        [HttpPost]
        [AsiatekSubordinateFunction("EditStructureInfo")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult CheckEditStrucLicenseNoExists(string licenseNo, int id)
        {
            if (string.IsNullOrWhiteSpace(licenseNo))
            {
                return Json(true);
            }
            return Json(!StructureBLL.CheckStrucLicenseNoExists(licenseNo, id));
        }

        [AsiatekSubordinateFunction("EditStructureInfo")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult GetStructureLogo(int id)
        {
            byte[] bts = StructureBLL.GetLogBytes(id);
            MemoryStream memStream = new MemoryStream(bts);
            Image img = Image.FromStream(memStream, true);
            return File(bts, "image/" + img.GetImageExtension() + "");
        }


        [HttpPost]
        [AsiatekSubordinateFunction("EditStructureInfo")]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult CheckEditCustomEncodingExists(string customEncoding, int id)
        {
            if (string.IsNullOrWhiteSpace(customEncoding))
            {
                return Json(true);
            }
            return Json(!StructureBLL.CheckEditCustomEncodingExists(customEncoding, id));
        }

        #endregion

        #region 删除

        #region 旧代码
        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult DeleteStruc(StructureSettingModel model, FormCollection fc)
        //{
        //    int currentPage = Convert.ToInt32(fc["currentPage"]);
        //    string[] ids = fc["strucid"].Split(',');

        //    var result = StructureBLL.DeleteStrus(ids);

        //    base.DoLog(OperationTypeEnum.Delete, result, fc["ctrid"]);

        //    ViewBag.Message = result.Message;


        //    return GetStructurePagedGridPV(model, currentPage);
        //} 
        #endregion

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult DeleteStruc(FormCollection fc)
        {
            string[] ids = fc["strucid"].Split(',');

            //var result = StructureBLL.DeleteStrus(ids);
            var result = StructureBLL.DeleteStrusPhysical(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["strucid"]);
            return Json(result);
        }
        #endregion

        #region 其他
        //用于界面上LOGO的显示，只做客户端浏览器缓存，缓存8小时
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.Client, Duration = 60 * 60 * 8)]
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.Any, Duration = Int32.MaxValue, VaryByParam = "id", SqlDependency = "asiatekCache:Structures")]
        [AsiatekPassPremission]
        public ActionResult GetCurrentOrParentLogo(int id)
        {
            base.FixVaryBug();
            byte[] bts = StructureBLL.GetCurrentOrParentLogoBytes(id);
            //string defaultPath = Server.MapPath("~/Content/images/login/logo.png");
            string defaultPath = Server.MapPath("~/Content/images/login/AsiatekLOGO.png");
            if (bts == null)
            {
                return File(defaultPath, "image/png");
            }
            else
            {
                try
                {
                    MemoryStream memStream = new MemoryStream(bts);
                    Image img = Image.FromStream(memStream, true);
                    return File(bts, "image/" + img.GetImageExtension() + "");
                }
                catch
                {
                    return File(defaultPath, "image/png");
                }
            }

        }


        /// <summary>
        /// 获取单位信息JSON格式内容
        /// 用于树状图
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("DistributionSetting", "StrucVehicleDistribution", "Admin")]
        public ActionResult GetDistributiveStrucsByUserID(int uid)
        {
            var allStrucs = StructureBLL.GetStructuresForTree();
            var userStrucIDs = StructureBLL.GetDistributiveStrucIDByUserID(uid);
            var parentStruc = allStrucs.Where(s => s.ParentID == null);
            List<BootstrapTreeViewNode> nodes = new List<BootstrapTreeViewNode>();
            foreach (var item in parentStruc)
            {
                var temp = new BootstrapTreeViewNode()
                {
                    text = item.StrucName,
                    tag = item.ID,
                    state = new BootstrapTreeViewNodeState()
                    {
                        expanded = true,
                        @checked = userStrucIDs.SingleOrDefault(id => id == item.ID) != null
                    }
                };
                CreateSubTreeNode(temp, item.ID, allStrucs, userStrucIDs);
                nodes.Add(temp);
            }
            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        private void CreateSubTreeNode(BootstrapTreeViewNode parentNode, int parentID, List<StructureTreeModel> strus, List<int?> ids)
        {
            var subStrucs = strus.Where(s => s.ParentID == parentID);
            if (subStrucs.Count() == 0)
            {
                return;
            }
            parentNode.nodes = new List<BootstrapTreeViewNode>();
            foreach (var item in subStrucs)
            {
                bool flag = ids.SingleOrDefault(id => id == item.ID) != null;
                var temp = new BootstrapTreeViewNode()
                {
                    text = item.StrucName,
                    tag = item.ID,
                    //ParentNode = parentNode,
                    state = new BootstrapTreeViewNodeState()
                    {
                        @checked = flag,
                        expanded = flag
                    }
                };
                //if (temp.state.@checked)//如果节点是分配过的，那么还需要展开父节点，防止分配了子单位未分配父单位，导致节点未展开
                //{
                //    //parentNode.state.expanded = true;
                //    ExpandParentNode(parentNode);
                //}
                parentNode.nodes.Add(temp);
                CreateSubTreeNode(temp, item.ID, strus, ids);
            }
        }

        //private void ExpandParentNode(BootstrapTreeViewNode parentNode)
        //{
        //    if (parentNode != null)
        //    {
        //        parentNode.state.expanded = true;
        //        ExpandParentNode((parentNode.ParentNode as BootstrapTreeViewNode));
        //    }
        //}
        #endregion

        #region 分配经营范围
        #region 初始化经营范围视图
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult BusinessScope(string strucIDList)
        {
            var list = StructureBLL.GetBusinessScopeList();
            ViewBag.StrucIDList = strucIDList;
            return PartialView("_BusinessScope", list);
        }
        #endregion

        #region 分配经营范围
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult DistributionBusinessScope(string strucIDList, string codeList)
        {
            var result = StructureBLL.DistributionBusinessScope(strucIDList, codeList);
            base.DoLog(OperationTypeEnum.Edit, result, strucIDList);
            return Json(result);
        }
        #endregion
        #endregion

        #region 查看经营范围
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult SearchBusinessScope(int id)
        {
            var result = StructureBLL.GetBusinessScopeListByStrucID(id);
            return PartialView("_SearchBusinessScope", result);
        }
        #endregion

        #region 分配运输行业
        #region 初始化运输行业视图
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult TransportIndustry(string strucIDList)
        {
            var list = StructureBLL.GetTransportIndustryList();
            ViewBag.StrucIDList = strucIDList;
            return PartialView("_TransportIndustry", list);
        }
        #endregion

        #region 分配运输行业
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult DistributionTransportIndustry(string strucIDList, string codeList)
        {
            var result = StructureBLL.DistributionTransportIndustry(strucIDList, codeList);
            base.DoLog(OperationTypeEnum.Edit, result, strucIDList);
            return Json(result);
        }
        #endregion
        #endregion

        #region 查看运输行业
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult SearchTransportIndustry(int id)
        {
            var result = StructureBLL.GetTransportIndustryListByStrucID(id);
            return PartialView("_SearchSearchTransportIndustry", result);
        }
        #endregion

        #region 夜间禁行启用/关闭设置
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("StructureSetting")]
        public ActionResult NightBan(string strucIDList, int isNightBan)
        {
            var result = StructureBLL.NightBan(strucIDList, isNightBan);
            base.DoLog(OperationTypeEnum.Edit, result, strucIDList);
            return Json(result);
        }
        #endregion
    }
}

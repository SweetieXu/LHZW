using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.Admin.Controllers
{
    public class ServerManagerController : BaseController
    {
        #region 查询
        /// <summary>
        /// 初始化查询界面
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ServerSetting()
        {
            SearchDataWithPagedDatas<ServerManagerSearchModel, ServerManagerListModel> model = new SearchDataWithPagedDatas<ServerManagerSearchModel, ServerManagerListModel>();
            model.SearchModel = new ServerManagerSearchModel();
            model.PagedDatas = ServerManagerBLL.GetPagedServerManager(model.SearchModel, 1, this.PageSize);
            return PartialView("_ServerSetting", model);
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <param name="model"></param>
        /// <param name="searchPage"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("ServerSetting")]
        public PartialViewResult GetServerInfo(ServerManagerSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<ServerManagerSearchModel, ServerManagerListModel> result = new SearchDataWithPagedDatas<ServerManagerSearchModel, ServerManagerListModel>();
            result.SearchModel = model;
            result.PagedDatas = ServerManagerBLL.GetPagedServerManager(result.SearchModel, searchPage, this.PageSize);
            return PartialView("_ServerPagedGrid", result);
        }

        #endregion

        #region 新增
        /// <summary>
        /// 初始化新增页面
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("ServerSetting")]
        public PartialViewResult AddServer()
        {
            ServerManagerEditModel model = new ServerManagerEditModel();
            return PartialView("_AddServer", model);
        }

        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ServerSetting")]
        public ActionResult AddServer(ServerManagerEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreateUserID = base.CurrentUserID;
                var result = ServerManagerBLL.AddServer(model);
                base.DoLog(OperationTypeEnum.Add, result, "ServerName:" + model.ServerName);
                return Json(result);
            }
            else
            {
                return PartialView("_AddServer", model);
            }
        }
        #endregion

        #region 修改
        /// <summary>
        /// 初始化修改页面
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("ServerSetting")]
        public ActionResult EditServer(int id)
        {
            var result = ServerManagerBLL.GetServerByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            return PartialView("_EditServer", result.DataResult);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ServerSetting")]
        public ActionResult EditServer(ServerManagerEditModel model)
        {
            if (ModelState.IsValid)
            {
                model.UpdateUserID = base.CurrentUserID;
                var result = ServerManagerBLL.EditServer(model);
                base.DoLog(OperationTypeEnum.Edit, result, "ID:" + model.ID);
                return Json(result);
            }
            else
            {
                return PartialView("_EditServer", model);
            }
        }

        #endregion

        #region 删除

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("ServerSetting")]
        public JsonResult DeleteServer(FormCollection fc)
        {
            string[] ids = fc["tmid"].Split(',');
            var result = ServerManagerBLL.DeleteServer(ids);
            base.DoLog(OperationTypeEnum.Delete, result, fc["tmid"]);
            return Json(result);

        }
        #endregion
    }
}

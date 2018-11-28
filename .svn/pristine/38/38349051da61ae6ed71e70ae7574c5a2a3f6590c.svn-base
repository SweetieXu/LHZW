using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using Asiatek.Resource;

namespace Asiatek.TMS.Areas.HistoricalRoute.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = base.GetCurrentViewName();
            var model = new HistorySignalAllInfoModels();
            model.StartTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00");
            model.EndTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
            return View(model);
        }

        #region  历史轨迹
        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("Index")]
        public ActionResult Search(long vid, string vName, string VIN, string startTime, string endTime, string invTime, string isSpeed, bool showStopPoint)
        {
            HistorySignalAllInfoModels model = new HistorySignalAllInfoModels();
            model.VehicleID = vid;
            model.VehicleName = vName;
            model.VIN = VIN;
            model.StartTime = startTime;
            model.EndTime = endTime;
            model.InvTime = invTime;
            model.IsSpeed = isSpeed;
            model.showStopPoint = showStopPoint;
            var list = HistoricalBLL.GetHistoricalInfo(model);
            #region 传给高德地图的分段数据放入后台处理代码(已采用前端的代码处理)
            //List<MapInfo> resultList = new List<MapInfo>();
            //List<double[]> path=new List<double[]>();
            //List<string[]> remark=new List<string[]>();
            //for(int i=0;i<list.Count-1;i++)
            //{
            //    var currentSignal = list[i];
            //    //定位点的数组
            //    double[] d_location = new double[] { Convert.ToDouble(currentSignal.Longitude),Convert.ToDouble(currentSignal.Latitude)};
            //    string[] s_remark = new string[] { currentSignal.Time.ToString("yyyy-MM-dd HH:mm:ss"), currentSignal.Speed, currentSignal.Mileage, currentSignal.Temperature, currentSignal.ACCState.ToString() };
            //    path.Add(d_location);
            //    remark.Add(s_remark);
            //    var nextsignal =list[i+1];
            //    string name = "正常信号";
            //    //因为要判断当前的点与后一个点的类型是否一样所以只能到倒数第二个点
            //    if (i < list.Count - 1)
            //    {
            //        //当前信号为正常信号，下一个信号为盲区或者报警信号
            //        if ((nextsignal.IsBlind ==true || nextsignal.ExName != "") && (currentSignal.IsBlind != true && currentSignal.ExName == ""))
            //        {
            //            name = "正常信号";
            //            //Remark += list[i].SignalDateTime;
            //            //添加到整个的返回集
            //            resultList.Add(new MapInfo()
            //            {
            //                name = name,
            //                path = path,
            //                remark=remark
            //            });
            //            //重置区段的定位集，将当前信号当做下一个区段的起点
            //            path = new List<double[]>();
            //            path.Add(d_location);
            //        }
            //            //当前信号为异常的信号，下一个为正常的
            //        else if ((currentSignal.IsBlind == true || currentSignal.ExName != "") && (nextsignal.IsBlind != true && nextsignal.ExName == ""))
            //        {

            //            if (currentSignal.IsBlind == true && currentSignal.ExName == "")
            //            {
            //                name = "盲区";
            //            }
            //            else if (currentSignal.IsBlind != true && currentSignal.ExName != "")
            //            {
            //                name = "报警";
            //            }
            //            else 
            //            {
            //                name = "混合区域";
            //            }
            //            resultList.Add(new MapInfo()
            //            {
            //                name =  name,
            //                path = path,
            //                remark=remark
            //            });
            //            path = new List<double[]>();
            //            path.Add(d_location);
            //            remark=new List<string[]>();
            //            remark.Add(s_remark);
                        
            //        }
            //    }
            //    else {
            //        resultList.Add(new MapInfo()
            //        {
            //            name = name.ToString(),
            //            path = path,
            //            remark=remark
            //        });
            //    }
            //};
            //if (resultList == null)
            //{
            //    resultList = new List<MapInfo>();
            //}
            //AllInfo allinfo = new AllInfo();
            //allinfo.historySignalShowListModel = list;
            //allinfo.mapInfo = resultList;
            #endregion
            //理论上来说如果查出来的数据量大于int32.maxvalue，那么list也会返回null，所以又可能不是查不出数据而是数据太多，不过实际上不太可能这么多数据，所以这里暂时不处理
            if (list == null)
            {
                list = new List<HistorySignalShowListModel>();
            }
            //最好的做法是发现信号点太多直接给出提示要求缩小查询范围，否则地图也会很卡,这里就先算了
            //修正当信号过多时超出json长度的问题，理论上来说
            return new JsonResult()
            {
                Data = list,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        #endregion


        //#region  查询异常轨迹
        //public ActionResult SearchException(string vin, string startTime, string endTime)
        //{
        //    HistorySignalExportModel model = new HistorySignalExportModel();
        //    model.VIN = vin;
        //    model.StartTime = startTime;
        //    model.EndTime = endTime;
        //    var list = HistoricalBLL.GetHistoryExceptionInfo(model);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region  查询超速异常轨迹
        [AsiatekSubordinateFunction("Index")]
        public ActionResult SearchOverSpeedException(long vid, string vName, string VIN, string startTime, string endTime, string invTime, string isSpeed)
        {
            HistorySignalAllInfoModels model = new HistorySignalAllInfoModels();
            model.VehicleID = vid;
            model.VehicleName = vName;
            model.VIN = VIN;
            model.StartTime = startTime;
            model.EndTime = endTime;
            model.InvTime = invTime;
            model.IsSpeed = isSpeed;
            var list = HistoricalBLL.GetHistoryOverSpeedExceptionInfo(model);
            //理论上来说如果查出来的数据量大于int32.maxvalue，那么list也会返回null，所以又可能不是查不出数据而是数据太多，不过实际上不太可能这么多数据，所以这里暂时不处理
            if (list == null)
            {
                list = new List<HistorySignalShowListModel>();
            }
            //最好的做法是发现信号点太多直接给出提示要求缩小查询范围，否则地图也会很卡,这里就先算了
            //修正当信号过多时超出json长度的问题，理论上来说
            return new JsonResult()
            {
                Data = list,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }
        #endregion


        /// <summary>
        /// 左侧车辆代号查询数据
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("Index")]
        public ActionResult GetUserVehiclesByVehicleName(string vehicleName)
        {
            List<UserVehicles> list = new List<UserVehicles>();
            // 默认模式
            if (base.VehicleViewMode)
            {
                list = VehicleBLL.GetDefaultVehiclesAndStrucName(base.CurrentStrucID, vehicleName);
            }
            else
            {
                list = VehicleBLL.GetVehiclesAndStrucName(base.CurrentUserID, vehicleName);
            }
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.VehicleName + "[" + item.StrucName + "]", value = item.VehicleName, VID = item.VID, strucName = item.StrucName, VIN = item.VIN });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }


        #region  导出查询所有轨迹信号
        [AsiatekSubordinateFunction("Index")]
        public ActionResult ExportAllToExcel(long vid, string vName, string VIN, string strucName, string startTime, string endTime, string invTime, string isSpeed)
        {
            #region 求导出的数据结果
            HistorySignalAllInfoModels model = new HistorySignalAllInfoModels();
            model.VehicleID = vid;
            model.VehicleName = vName;
            model.VIN = VIN;
            model.StrucName = strucName;
            model.StartTime = startTime;
            model.EndTime = endTime;
            model.InvTime = invTime;
            model.IsSpeed = isSpeed;
            var list = HistoricalBLL.GetHistoricalInfo(model);
            model.Datas = list;
            #endregion

            ViewBag.ExportFlag = true;
            ViewData.Model = model;
            string viewHtml = RenderPartialViewToString(this, "_ExportAllSignal");
            var reportName = vName + "（" + strucName + "）" + @DisplayText.HistoryRoute + DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(System.Text.Encoding.UTF8.GetBytes(viewHtml), "application/ms-excel", string.Format("{0}.xls", reportName + "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")"));
        }
        #endregion


        #region  导出异常轨迹信号
        [AsiatekSubordinateFunction("Index")]
        public ActionResult ExportExceptionToExcel(long vid, string vName, string VIN, string strucName, string startTime, string endTime, string invTime, string isSpeed, int exportType)
        {
            #region 求导出的数据结果
            HistorySignalAllInfoModels model = new HistorySignalAllInfoModels();
            model.VehicleID = vid;
            model.VehicleName = vName;
            model.VIN = VIN;
            model.StrucName = strucName;
            model.StartTime = startTime;
            model.EndTime = endTime;
            model.InvTime = invTime;
            model.IsSpeed = isSpeed;
            var list = new List<HistorySignalShowListModel>();
            //if(exportType == 2)
            //{
            //    list = HistoricalBLL.GetHistoryExceptionInfo(model);
            //}
            if (exportType == 3)
            {
                list = HistoricalBLL.GetHistoryOverSpeedExceptionInfo(model);
            }

            model.Datas = list;
            #endregion

            ViewBag.StrucName = strucName;
            ViewBag.ExportFlag = true;
            ViewData.Model = model;
            string viewHtml = RenderPartialViewToString(this, "_ExportAllExceptionSignal");
            var reportName = vName + "（" + strucName + "）" + @DisplayText.HistoryRoute + DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(System.Text.Encoding.UTF8.GetBytes(viewHtml), "application/ms-excel", string.Format("{0}.xls", reportName + "(" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")"));
        }
        #endregion


        #region  显示区域
        [AsiatekSubordinateFunction("Index")]
        public ActionResult ShowArea()
        {
            var list = HistoricalBLL.GetShowAreaDataWithUserID(base.CurrentUserID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

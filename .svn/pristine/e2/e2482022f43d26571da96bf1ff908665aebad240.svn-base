using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Model.TerminalSetting;
using Asiatek.Resource;
using Asiatek.TMS.TerminalOperation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    partial class TerminalSetupController
    {
        /// <summary>
        /// GET: /TerminalSetting/TerminalSetup/Paths/
        /// 下发路径设置页面
        /// </summary>
        public ActionResult Paths()
        {
            ViewBag.SubView = "Paths";
            ViewBag.SubViewData = null;
            ViewBag.TreeViewShowCheckBox = true;
            var model = new SearchDataWithPagedDatas<string[], MapLineSettingModel>();
            model.PagedDatas = new AsiatekPagedList<MapLineSettingModel>(
                new List<MapLineSettingModel>(),
                1, 5, 5
            );
            return PartialView("Main", model);
        }

        /// <summary>
        /// 获取当前选中车辆所有设置的区域
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadLineVehicleTable(string[] Vehicles, int SearchPage)
        {
            var model = new SearchDataWithPagedDatas<string[], MapLineSettingModel>();

            if (Vehicles == null || Vehicles.Length == 0 || SearchPage <= 0)//非法请求
            {
                model.PagedDatas = new AsiatekPagedList<MapLineSettingModel>(
                    new List<MapLineSettingModel>(),
                    1, 5, 5
                );
                return PartialView("Paths_DataTable", model);
            }

            ViewBag.SubView = "Paths";
            ViewBag.SubViewData = null;
            ViewBag.TreeViewShowCheckBox = true;

            var PlateNums = new List<string>();
            var PlateNumAndTerminalCodes = new List<Tuple<string, string>>();
            foreach (var v in Vehicles)
            {
                // Vehicles当中的记录为 PlateNum#TerminalCode
                string[] PlateNumAndTerminalCode = v.Split('#');
                PlateNums.Add(PlateNumAndTerminalCode[0]);
                PlateNumAndTerminalCodes.Add(new Tuple<string, string>(PlateNumAndTerminalCode[0], PlateNumAndTerminalCode[1]));
            }

            var data = TerminalSettingsBLL.QueryMapLineSettingsByUserID(base.CurrentUserID, SearchPage, PlateNums.ToArray());

            model.PagedDatas = new AsiatekPagedList<MapLineSettingModel>(data.Item1, SearchPage, 5, (int)data.Item2);
            return PartialView("Paths_DataTable", model);
        }

        /// <summary>
        /// 从数据库和车机上删除指定的路线设置
        /// </summary>
        /// <param name="RelationIDs"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLineVehicleRelation(int[] RelationIDs)
        {
            if (RelationIDs == null || RelationIDs.Length == 0)
                return Json(new OperationResult() { Success = false, Message = "非法请求。" });

            var tArray = new Task<OperationResult>[RelationIDs.Length];
            TerminalOperationClient client = new TerminalOperationClient();
            for (int i = 0; i != RelationIDs.Length; i++)
            {
                tArray[i] = InternalDeleteLineVehicleRelation(RelationIDs[i], client);
            }

            var result = await Task.WhenAll<OperationResult>(tArray);
            return Json(result);
        }

        private Task<OperationResult> InternalDeleteLineVehicleRelation(int RelationID, TerminalOperationClient client)
        {
            return Task<OperationResult>.Run(() =>
            {
                var LineInfo = TerminalSettingsBLL.GetLineInformation(base.CurrentUserID, RelationID);
                if (LineInfo.Item1)
                {
                    string PlateNum = LineInfo.Item2;
                    string TerminalCode = LineInfo.Item3;
                    int LineType = LineInfo.Item4;
                    long LineID = LineInfo.Item5;
                    var response = client.DeleteLine(TerminalCode, new RegionLineDeletionData()
                        {
                            IDs = new uint[] { (uint)LineID }
                        });
                    OperationResult resultElement = null;
                    if (!response.State)
                    {
                        resultElement = new OperationResult()
                        {
                            Success = false,
                            Message = response.Message
                        };
                    }
                    else
                    {
                        bool success = TerminalSettingsBLL.DeleteLineVehicleRelation(base.CurrentUserID, RelationID);
                        resultElement = new OperationResult()
                        {
                            Success = success,
                            Message = success ? DisplayText.OperationSucceeded : "车机端删除操作成功完成，删除数据库记录出错。"
                        };
                    }
                    TerminalSettingsBLL.InsertTerminalOperationsLog(
                        base.CurrentUserID, TerminalSettingType.RouteSettings_Delete, PlateNum, TerminalCode, false,
                        RelationID.ToString(), resultElement.Message, GetRemoteAddress()
                    );
                    return resultElement;
                }
                else
                {
                    string RecordNotExists = "数据库中找不到对应的设置记录。";
                    TerminalSettingsBLL.InsertTerminalOperationsLog(
                        base.CurrentUserID, TerminalSettingType.RouteSettings_Delete,
                        null, null, false, RelationID.ToString(), RecordNotExists, GetRemoteAddress()
                    );
                    return new OperationResult()
                    {
                        Success = false,
                        Message = RecordNotExists
                    };
                }
            });
        }

        /// <summary>
        /// 查询指定路线数据
        /// </summary>
        /// <param name="RegionID">路线ID</param>
        /// <returns></returns>
        public ActionResult GetLineData(int LineID)
        {
            var lines = TerminalSettingsBLL.GetLinesByLineID(LineID);
            return Json(lines, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取当前用户可以操作的所有指定类型的路线
        /// </summary>
        /// <param name="RegionType">路线类型</param>
        /// <returns>Tuple(RegionID, RegionName)[]</returns>
        public ActionResult GetAllLines(int LineType)
        {
            var regions = TerminalSettingsBLL.GetAllLines(base.CurrentUserID, LineType);
            return Json(new SelectResult<Tuple<long, string>[]>() { DataResult = regions }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下发路线设置
        /// </summary>
        /// <param name="LineProperty"></param>
        /// <param name="Vehicles">PlateNum#TerminalCode数组</param>
        /// <param name="LineID">区域ID</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendRegionSettings(int LineProperty, string[] Vehicles, long LineID)
        {
            OperationResult valid = null;
            var line = TerminalSettingsBLL.GetLinesByLineID((int)LineID);

            if (Vehicles == null || Vehicles.Length == 0)
                valid = new OperationResult() { Success = false, Message = "未指定设置的车辆。" };

            else if (line == null || line.Count == 0)
                valid = new OperationResult() { Success = false, Message = "数据库中未找到指定的路线。" };

            if (valid != null)
            {
                dynamic settingData = new ExpandoObject();
                settingData.LineProperty = LineProperty;
                settingData.Vehicles = Vehicles;
                settingData.LineID = LineID;
                TerminalSettingsBLL.InsertTerminalOperationsLog(
                    base.CurrentUserID,
                    TerminalSettingType.RouteSettings_Add,
                    null,
                    null,
                    false,
                    JsonConvert.SerializeObject(settingData),
                    valid.Message,
                    GetRemoteAddress()
                );
                return Json(new OperationResult[] { valid });
            }

            var tasks = new Task<OperationResult>[Vehicles.Length];

            for (int i = 0; i != Vehicles.Length; i++)
            {
                string[] PlateNumAndTerminalCode = Vehicles[i].Split('#');
                string PlateNum = PlateNumAndTerminalCode[0];
                string TerminalCode = PlateNumAndTerminalCode[1];
                var client = new TerminalOperationClient();
                tasks[i] = InternalSendLineSettings((LineProperty)LineProperty, PlateNum, TerminalCode, line, client);
            }

            var result = await Task.WhenAll<OperationResult>(tasks);
            return Json(result);
        }

        private Task<OperationResult> InternalSendLineSettings(
            LineProperty LineProperty,
            string PlateNum,
            string TerminalCode,
            List<MapLinesDetailModel> LineModel,
            TerminalOperationClient client
        )
        {
            return Task<OperationResult>.Run(() =>
                {
                    dynamic settingData = new ExpandoObject();
                    settingData.LineProperty = LineProperty;
                    settingData.Vehicles = new string[] { PlateNum };
                    settingData.LineID = LineModel[0].ID;

                    var exists = TerminalSettingsBLL.LineVehicleRelationExists(PlateNum, TerminalCode, LineModel[0].ID);
                    if (exists.Item1)
                    {
                        var r = new OperationResult()
                        {
                            Success = false,
                            Message = "数据库中已存在与指定车辆、区域对应的设置记录。"
                        };
                        TerminalSettingsBLL.InsertTerminalOperationsLog(
                            base.CurrentUserID,
                            TerminalSettingType.RouteSettings_Add,
                            PlateNum,
                            TerminalCode,
                            false,
                            JsonConvert.SerializeObject(settingData),
                            r.Message,
                            GetRemoteAddress()
                        );
                        return r;
                    }

                    var LineInflectionPointItems = new LineInflectionPointItem[LineModel.Count];
                    for (int i = 0; i != LineModel.Count; i++)
                    {
                        LineInflectionPointItems[i] = new LineInflectionPointItem()
                        {
                            PointID = (uint)LineModel[i].OrderID,
                            RoadID = (uint)LineModel[i].ID,
                            PointLatitude = LineModel[i].Latitude,
                            PointLongitude = LineModel[i].Longitude,
                            RoadWidth = (byte)LineModel[i].RoadWidth.GetValueOrDefault(),
                            DrivingTooLongThresholding = (ushort)LineModel[i].MaxSecond.GetValueOrDefault(),
                            DrivingTooLackThresholding = (ushort)LineModel[i].MinSecond.GetValueOrDefault(),
                            MaxSpeed = (ushort)LineModel[i].SpeedLimit.GetValueOrDefault(),
                            OverSpeedDuration = (byte)LineModel[i].OverSpeedDuration.GetValueOrDefault()
                        };

                        if (LineModel[i].IsCheckTime.HasValue && LineModel[i].IsCheckTime.Value)
                            LineInflectionPointItems[i].RoadProperty |= RoadProperty.行驶时间;
                        if (LineModel[i].IsCheckSpeed.HasValue && LineModel[i].IsCheckSpeed.Value)
                            LineInflectionPointItems[i].RoadProperty |= RoadProperty.限速;
                    }

                    var response = client.SetLine(TerminalCode, new LineSettingData()
                        {
                            LineID = (uint)LineModel[0].ID,
                            IsCyclical = false,
                            LineProperty = LineProperty,
                            StartDateTime = DateTime.Parse(LineModel[0].StartTime),
                            EndDateTime = DateTime.Parse(LineModel[0].EndTime),
                            LineInflectionPointItems = LineInflectionPointItems
                        });

                    OperationResult resultElement = null;

                    if (!response.State)
                    {
                        //车机端操作失败
                        resultElement = new OperationResult()
                        {
                            Success = false,
                            Message = response.Message
                        };
                        TerminalSettingsBLL.InsertTerminalOperationsLog(
                            base.CurrentUserID,
                            TerminalSettingType.RouteSettings_Add,
                            PlateNum,
                            TerminalCode,
                            false,
                            JsonConvert.SerializeObject(settingData),
                            resultElement.Message,
                            GetRemoteAddress()
                        );
                    }
                    else
                    {
                        bool success = TerminalSettingsBLL.AddLineVehicleRelation(PlateNum, TerminalCode, exists.Item2.Value, (ushort)LineProperty);
                        resultElement = new OperationResult()
                        {
                            Success = success,
                            Message = success ? DisplayText.OperationSucceeded : "车机端更新操作成功完成，更新数据库记录出错。"
                        };
                        TerminalSettingsBLL.InsertTerminalOperationsLog(
                            base.CurrentUserID,
                            TerminalSettingType.RouteSettings_Add,
                            PlateNum,
                            TerminalCode,
                            success,
                            JsonConvert.SerializeObject(settingData),
                            resultElement.Message,
                            GetRemoteAddress()
                        );
                    }
                    return resultElement;
                });
        }
    }
}
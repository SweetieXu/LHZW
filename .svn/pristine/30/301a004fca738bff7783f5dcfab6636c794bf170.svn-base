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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    partial class TerminalSetupController
    {
        /// <summary>
        /// GET: /TerminalSetting/TerminalSetup/Regions/
        /// 下发区域设置页面
        /// </summary>
        public ActionResult Regions()
        {
            ViewBag.SubView = "Regions";
            ViewBag.SubViewData = null;
            ViewBag.TreeViewShowCheckBox = true;
            var model = new SearchDataWithPagedDatas<int, MapRegionSettingModel>();
            model.PagedDatas = new AsiatekPagedList<MapRegionSettingModel>(
                new List<MapRegionSettingModel>(),
                1, 5, 5
            );
            return PartialView("Main", model);
        }

        /// <summary>
        /// 获取当前选中车辆所有设置的区域
        /// </summary>
        /// <param name="RegionType">区域类型。1 为圆形，2 为矩形，3 为多边形</param>
        /// <param name="Vehicles">PlateNum#TerminalCode数组</param>
        /// <param name="SearchPage">当前页码</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadRegionVehicleTable(int RegionType, string[] Vehicles, int SearchPage)
        {
            var model = new SearchDataWithPagedDatas<int, MapRegionSettingModel>();

            if (RegionType < 1 || RegionType > 3 || Vehicles == null || Vehicles.Length == 0 || SearchPage <= 0)//非法请求
            {
                model.PagedDatas = new AsiatekPagedList<MapRegionSettingModel>(
                    new List<MapRegionSettingModel>(),
                    1, 5, 5
                );
                return PartialView("Regions_DataTable", model);
            }

            ViewBag.SubView = "Regions";
            ViewBag.SubViewData = null;
            ViewBag.TreeViewShowCheckBox = true;

            var PlaterNums = new List<string>();
            var PlaterNumAndTerminalCodes = new List<Tuple<string, string>>();
            foreach (var v in Vehicles)
            {
                // Vehicles当中的记录为 PlateNum#TerminalCode
                string[] PlateNumAndTerminalCode = v.Split('#');
                PlaterNums.Add(PlateNumAndTerminalCode[0]);
                PlaterNumAndTerminalCodes.Add(new Tuple<string, string>(PlateNumAndTerminalCode[0], PlateNumAndTerminalCode[1]));
            }

            var data = TerminalSettingsBLL.QueryMapRegionSettingsByUserID(base.CurrentUserID, SearchPage, RegionType, PlaterNums.ToArray());

            model.PagedDatas = new AsiatekPagedList<MapRegionSettingModel>(data.Item1, SearchPage, 5, (int)data.Item2);
            return PartialView("Regions_DataTable", model);
        }

        /// <summary>
        /// 从数据库和车机上删除指定的区域设置
        /// </summary>
        /// <param name="RelationIDs">区域设置表 dbo.MapRegionSettings 主键</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRegionVehicleRelation(int[] RelationIDs)
        {
            if (RelationIDs == null || RelationIDs.Length == 0)
                return Json(new OperationResult() { Success = false, Message = "非法请求。"});

            var tArray = new Task<OperationResult>[RelationIDs.Length];
            TerminalOperationClient client = new TerminalOperationClient();
            for (int i = 0; i != RelationIDs.Length; i++)
            {
                tArray[i] = InternalDeleteRegionVehicleRelation(RelationIDs[i], client);
            }

            var result = await Task.WhenAll<OperationResult>(tArray);
            return Json(result);
        }

        private Task<OperationResult> InternalDeleteRegionVehicleRelation(int RelationID, TerminalOperationClient client)
        {
            return Task<OperationResult>.Run(() =>
                {
                    var RegionInfo = TerminalSettingsBLL.GetRegionInformation(base.CurrentUserID, RelationID);
                    if (RegionInfo.Item1)
                    {
                        string PlateNum = RegionInfo.Item2;
                        string TerminalCode = RegionInfo.Item3;
                        int RegionType = RegionInfo.Item4;
                        long RegionID = RegionInfo.Item5;
                        OperationResultGeneralRep response;
                        switch (RegionType)
                        {
                            case 1:
                                response = client.DeleteCircularRegion(TerminalCode, new RegionLineDeletionData()
                                {
                                    IDs = new uint[] { (uint)RegionID }
                                });
                                break;
                            case 2:
                                response = client.DeleteRectangularRegion(TerminalCode, new RegionLineDeletionData()
                                {
                                    IDs = new uint[] { (uint)RegionID }
                                });
                                break;
                            case 3:
                                response = client.DeletePolygonRegion(TerminalCode, new RegionLineDeletionData()
                                {
                                    IDs = new uint[] { (uint)RegionID }
                                });
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

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
                            bool success = TerminalSettingsBLL.DeleteRegionVehicleRelation(base.CurrentUserID, RelationID);
                            resultElement = new OperationResult()
                            {
                                Success = success,
                                Message = success ? DisplayText.OperationSucceeded : "车机端删除操作成功完成，删除数据库记录出错。"
                            };
                        }
                        TerminalSettingsBLL.InsertTerminalOperationsLog(
                            base.CurrentUserID, TerminalSettingType.RegionSettings_Delete, PlateNum, TerminalCode, false,
                            RelationID.ToString(), resultElement.Message, GetRemoteAddress()
                        );
                        return resultElement;
                    }
                    else
                    {
                        string RecordNotExists = "数据库中找不到对应的设置记录。";
                        TerminalSettingsBLL.InsertTerminalOperationsLog(
                            base.CurrentUserID, TerminalSettingType.RegionSettings_Delete,
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
        /// 查询指定区域数据
        /// </summary>
        /// <param name="RegionID">区域ID</param>
        /// <returns>MapRegionsEditModel[]</returns>
        public ActionResult GetRegionData(int RegionID)
        {
            var regions = TerminalSettingsBLL.GetRegionsByRegionID(RegionID);
            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取当前用户可以操作的所有指定类型的区域
        /// </summary>
        /// <param name="RegionType">区域类型</param>
        /// <returns>Tuple(RegionID, RegionName)[]</returns>
        public ActionResult GetAllRegions(int RegionType)
        {
            if (RegionType < 1 || RegionType > 3)
                return Json(new SelectResult<Tuple<long, string>[]>() { Message = "非法的区域类型。" }, JsonRequestBehavior.AllowGet);

            var regions = TerminalSettingsBLL.GetAllRegions(base.CurrentUserID, RegionType);
            return Json(new SelectResult<Tuple<long, string>[]>() { DataResult = regions }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下发区域设置
        /// </summary>
        /// <param name="OperationType">1 为更新，2 为追加</param>
        /// <param name="RegionProperty">区域属性</param>
        /// <param name="Vehicles">PlateNum#TerminalCode数组</param>
        /// <param name="RegionID">区域ID</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendRegionSettings(int OperationType, int RegionProperty, string[] Vehicles, long RegionID)
        {
            OperationResult valid = null;
            var region = TerminalSettingsBLL.GetRegionsByRegionID((int)RegionID);

            TerminalSettingType SettingType;
            if (OperationType == (int)RegionSettingType.更新区域)
                SettingType = TerminalSettingType.RegionSettings_Update;
            else if (OperationType == (int)RegionSettingType.追加区域)
                SettingType = TerminalSettingType.RegionSettings_Add;
            else
            {
                SettingType = TerminalSettingType.None;
                valid = new OperationResult() { Success = false, Message = "区域设置暂不支持除 更新、追加 之外的其它操作。" };
            }

            if (Vehicles == null || Vehicles.Length == 0)
                valid = new OperationResult() { Success = false, Message = "未指定设置的车辆。" };
            
            else if (region == null || region.Count == 0)
                valid = new OperationResult() { Success = false, Message = "数据库中未找到指定的区域。" };

            else if (region[0].RegionsType < 1 || region[0].RegionsType > 3)
                valid = new OperationResult() { Success = false, Message = "指定的区域类型不受支持。" };

            else if (OperationType == (int)RegionSettingType.更新区域 && region[0].RegionsType == 3)
                return Json(new OperationResult[] { new OperationResult() { Success = false, Message = "多边形区域不支持更新操作。" } });

            if (valid != null)
            {
                dynamic settingData = new ExpandoObject();
                settingData.OperationType = OperationType;
                settingData.RegionProperty = RegionProperty;
                settingData.Vehicles = Vehicles;
                settingData.RegionID = RegionID;
                TerminalSettingsBLL.InsertTerminalOperationsLog(
                    base.CurrentUserID,
                    SettingType,
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
                if (OperationType == (int)RegionSettingType.更新区域)
                {
                    tasks[i] = InternalUpdateRegionSettings((RegionProperty)RegionProperty, PlateNum, TerminalCode, region, client);
                }
                else if (OperationType == (int)RegionSettingType.追加区域)
                {
                    tasks[i] = InternalAddRegionSettings((RegionProperty)RegionProperty, PlateNum, TerminalCode, region, client);
                }
            }
            
            var result = await Task.WhenAll<OperationResult>(tasks);
            return Json(result);
        }

        private Task<OperationResult> InternalUpdateRegionSettings(
            RegionProperty RegionProperty,
            string PlateNum, 
            string TerminalCode, 
            List<MapRegionsEditModel> RegionModel,
            TerminalOperationClient client
        )
        {
            return Task<OperationResult>.Run(() =>
                {
                    dynamic settingData = new ExpandoObject();
                    settingData.OperationType = RegionSettingType.更新区域;
                    settingData.RegionProperty = RegionProperty;
                    settingData.Vehicles = new string[] { PlateNum };
                    settingData.RegionID = RegionModel[0].ID;

                    var exists = TerminalSettingsBLL.RegionVehicleRelationExists(PlateNum, TerminalCode, RegionModel[0].ID);
                    if (!exists.Item1)
                    {
                        var r = new OperationResult()
                        {
                            Success = false,
                            Message = "数据库中不存在与指定车辆、区域对应的设置记录。"
                        };
                        TerminalSettingsBLL.InsertTerminalOperationsLog(
                            base.CurrentUserID,
                            TerminalSettingType.RegionSettings_Update,
                            PlateNum,
                            TerminalCode,
                            false,
                            JsonConvert.SerializeObject(settingData),
                            r.Message,
                            GetRemoteAddress()
                        );
                        return r;
                    }

                    StringBuilder start = new StringBuilder(), end = new StringBuilder();
                    start.Append(RegionModel[0].StartDate == null ? "1601.01.01" : RegionModel[0].StartDate);
                    start.Append(" ");
                    start.Append(RegionModel[0].StartTime == null ? "00:00:00.000" : RegionModel[0].StartTime);

                    end.Append(RegionModel[0].EndDate == null ? "1601.01.01" : RegionModel[0].EndDate);
                    end.Append(" ");
                    end.Append(RegionModel[0].EndTime == null ? "00:00:00.000" : RegionModel[0].EndTime);

                    OperationResultGeneralRep response = null;
                    switch (RegionModel[0].RegionsType)
                    {
                        case 1://圆形
                            response = client.SetCircularRegion(TerminalCode, new CircularRegionSettingData()
                                {
                                    RegionSettingType = RegionSettingType.更新区域,
                                    CircularRegionItems = new CircularRegionItem[]
                                    {
                                        new CircularRegionItem()
                                        {
                                            RegionID = (uint)RegionModel[0].ID,
                                            CentralPointLatitude = RegionModel[0].CenterLatitude,
                                            CentralPointLongitude = RegionModel[0].CenterLongitude,
                                            Radius = Convert.ToUInt32(RegionModel[0].Radius),
                                            MaxSpeed = Convert.ToUInt16(RegionModel[0].SpeedLimit),
                                            OverSpeedDuration = (byte)RegionModel[0].OverSpeedDuration,
                                            IsCyclical = RegionModel[0].Periodic,
                                            StartDateTime = DateTime.Parse(start.ToString()),
                                            EndDateTime = DateTime.Parse(end.ToString()),
                                            RegionProperty = (RegionProperty)RegionProperty,
                                        }
                                    }
                                });
                            break;
                        case 2://矩形
                            response = client.SetRectangularRegion(TerminalCode, new RectangularRegionSettingData()
                                {
                                    RegionSettingType = RegionSettingType.更新区域,
                                    RectangularRegionItems = new RectangularRegionItem[]
                                    {
                                        new RectangularRegionItem()
                                        {
                                            RegionID = (uint)RegionModel[0].ID,
                                            TopLeftPointLatitude = RegionModel[0].LeftUpperLatitude,
                                            TopLeftPointLongitude = RegionModel[0].LeftUpperLongitude,
                                            BottomRightPointLatitude = RegionModel[0].RightLowerLatitude,
                                            BottomRightPointLongitude = RegionModel[0].RightLowerLongitude,
                                            MaxSpeed = Convert.ToUInt16(RegionModel[0].SpeedLimit),
                                            OverSpeedDuration = (byte)RegionModel[0].OverSpeedDuration,
                                            IsCyclical = RegionModel[0].Periodic,
                                            StartDateTime = DateTime.Parse(start.ToString()),
                                            EndDateTime = DateTime.Parse(end.ToString()),
                                            RegionProperty = (RegionProperty)RegionProperty,
                                        }
                                    }
                                });
                            break;
                        default:
                            break;
                    }

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
                            TerminalSettingType.RegionSettings_Update,
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
                        bool success = TerminalSettingsBLL.UpdateRegionVehicleRelation(exists.Item2.Value, (ushort)RegionProperty);
                        resultElement = new OperationResult()
                        {
                            Success = success,
                            Message = success ? DisplayText.OperationSucceeded : "车机端更新操作成功完成，更新数据库记录出错。"
                        };
                        TerminalSettingsBLL.InsertTerminalOperationsLog(
                            base.CurrentUserID,
                            TerminalSettingType.RegionSettings_Update,
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

        private Task<OperationResult> InternalAddRegionSettings(
            RegionProperty RegionProperty,
            string PlateNum,
            string TerminalCode,
            List<MapRegionsEditModel> RegionModel,
            TerminalOperationClient client
        )
        {
            return Task<OperationResult>.Run(() =>
                {
                    dynamic settingData = new ExpandoObject();
                    settingData.OperationType = RegionSettingType.追加区域;
                    settingData.RegionProperty = RegionProperty;
                    settingData.Vehicles = new string[] { PlateNum };
                    settingData.RegionID = RegionModel[0].ID;

                    var exists = TerminalSettingsBLL.RegionVehicleRelationExists(PlateNum, TerminalCode, RegionModel[0].ID);
                    if (exists.Item1)
                    {
                        var r = new OperationResult()
                        {
                            Success = false,
                            Message = "数据库中已存在与指定车辆、区域对应的设置记录。"
                        };
                        TerminalSettingsBLL.InsertTerminalOperationsLog(
                            base.CurrentUserID,
                            TerminalSettingType.RegionSettings_Add,
                            PlateNum,
                            TerminalCode,
                            false,
                            JsonConvert.SerializeObject(settingData),
                            r.Message,
                            GetRemoteAddress()
                        );
                        return r;
                    }

                    StringBuilder start = new StringBuilder(), end = new StringBuilder();
                    start.Append(RegionModel[0].StartDate == null ? "1601.01.01" : RegionModel[0].StartDate);
                    start.Append(" ");
                    start.Append(RegionModel[0].StartTime == null ? "00:00:00.000" : RegionModel[0].StartTime);

                    end.Append(RegionModel[0].EndDate == null ? "1601.01.01" : RegionModel[0].EndDate);
                    end.Append(" ");
                    end.Append(RegionModel[0].EndTime == null ? "00:00:00.000" : RegionModel[0].EndTime);

                    OperationResultGeneralRep response = null;
                    switch (RegionModel[0].RegionsType)
                    {
                        case 1://圆形
                            response = client.SetCircularRegion(TerminalCode, new CircularRegionSettingData()
                            {
                                RegionSettingType = RegionSettingType.追加区域,
                                CircularRegionItems = new CircularRegionItem[]
                                    {
                                        new CircularRegionItem()
                                        {
                                            RegionID = (uint)RegionModel[0].ID,
                                            CentralPointLatitude = RegionModel[0].CenterLatitude,
                                            CentralPointLongitude = RegionModel[0].CenterLongitude,
                                            Radius = Convert.ToUInt32(RegionModel[0].Radius),
                                            MaxSpeed = Convert.ToUInt16(RegionModel[0].SpeedLimit),
                                            OverSpeedDuration = (byte)RegionModel[0].OverSpeedDuration,
                                            IsCyclical = RegionModel[0].Periodic,
                                            StartDateTime = DateTime.Parse(start.ToString()),
                                            EndDateTime = DateTime.Parse(end.ToString()),
                                            RegionProperty = (RegionProperty)RegionProperty,
                                        }
                                    }
                            });
                            break;
                        case 2://矩形
                            response = client.SetRectangularRegion(TerminalCode, new RectangularRegionSettingData()
                            {
                                RegionSettingType = RegionSettingType.追加区域,
                                RectangularRegionItems = new RectangularRegionItem[]
                                    {
                                        new RectangularRegionItem()
                                        {
                                            RegionID = (uint)RegionModel[0].ID,
                                            TopLeftPointLatitude = RegionModel[0].LeftUpperLatitude,
                                            TopLeftPointLongitude = RegionModel[0].LeftUpperLongitude,
                                            BottomRightPointLatitude = RegionModel[0].RightLowerLatitude,
                                            BottomRightPointLongitude = RegionModel[0].RightLowerLongitude,
                                            MaxSpeed = Convert.ToUInt16(RegionModel[0].SpeedLimit),
                                            OverSpeedDuration = (byte)RegionModel[0].OverSpeedDuration,
                                            IsCyclical = RegionModel[0].Periodic,
                                            StartDateTime = DateTime.Parse(start.ToString()),
                                            EndDateTime = DateTime.Parse(end.ToString()),
                                            RegionProperty = (RegionProperty)RegionProperty,
                                        }
                                    }
                            });
                            break;
                        case 3://多边形
                            break;
                        default:
                            break;
                    }

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
                            TerminalSettingType.RegionSettings_Update,
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
                        bool success = TerminalSettingsBLL.AddRegionVehicleRelation(PlateNum, TerminalCode, RegionModel[0].ID, (ushort)RegionProperty);
                        resultElement = new OperationResult()
                        {
                            Success = success,
                            Message = success ? DisplayText.OperationSucceeded : "车机端更新操作成功完成，添加数据库记录出错。"
                        };
                        TerminalSettingsBLL.InsertTerminalOperationsLog(
                            base.CurrentUserID,
                            TerminalSettingType.RegionSettings_Update,
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
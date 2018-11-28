using System.Text;
using Asiatek.AjaxPager;
using Asiatek.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Asiatek.Resource;

namespace Asiatek.Model
{
    /// <summary>
    /// 界面信息显示
    /// </summary>
    public class HistorySignalShowListModel
    {
        public long VehicleID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }
        /// <summary>
        /// 平均时速
        /// </summary>
        [Display(Name = "AvgSpeed", ResourceType = typeof(DisplayText))]
        public string AvgSpeed { get; set; }
        /// <summary>
        /// 最高时速
        /// </summary>
        [Display(Name = "MaxSpeed", ResourceType = typeof(DisplayText))]
        public string MaxSpeed { get; set; }
        /// <summary>
        /// 里程
        /// </summary>
        [Display(Name = "Mileage", ResourceType = typeof(DisplayText))]
        public string Mileage { get; set; }
        /// <summary>
        /// 地图里程
        /// </summary>
        [Display(Name = "MapMileage", ResourceType = typeof(DisplayText))]
        public string MapMileage { get; set; }
        /// <summary>
        /// 油量
        /// </summary>
        [Display(Name = "OilMass", ResourceType = typeof(DisplayText))]
        public string OilMass { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        [Display(Name = "Temperature", ResourceType = typeof(DisplayText))]
        public string Temperature { get; set; }
        /// <summary>
        /// 时间 对应接收数据的SignalDateTime
        /// </summary>
        [Display(Name = "Time", ResourceType = typeof(DisplayText))]
        public DateTime Time { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        [Display(Name = "Speed", ResourceType = typeof(DisplayText))]
        public string Speed { get; set; }
        /// <summary>
        /// 方向 对应接收数据的Direction
        /// </summary>
        public string Angle { get; set; }
        /// <summary>
        /// 盲区补点
        /// </summary>
        public bool IsBlind { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 举升（卸料点）
        /// </summary>
        public string TTStatus { get; set; }
        public string ExName { get; set; }
        public bool ACCState { get; set; }
        /// <summary>
        /// 是否停车点
        /// </summary>
        public bool IsStopPoint { get; set; }
        /// <summary>
        /// 停车点信息（包括开始时间、结束时间、停车时长）
        /// </summary>
        public string StopContent { get; set; }
        /// <summary>
        /// 开始停车时间（显示停车点列表信息用）
        /// </summary>
        public string StartStopVehicleTime { get; set; }
        /// <summary>
        /// 停车时长（显示停车点列表信息用）
        /// </summary>
        public string StopVehicleDuration { get; set; }
        /// <summary>
        /// 门磁状态
        /// </summary>
        public bool DoorSensorFlag { get; set; }
        /// <summary>
        /// 显示门磁信息（温度图表）
        /// </summary>
        public string DoorSensor { get; set; }
        /// <summary>
        /// 是否气压卸料
        /// </summary>
        public bool PressureFlag { get; set; }
    }


    /// <summary>
    /// 查询条件和界面显示信息
    /// </summary>
    public class HistorySignalAllInfoModels : ContainsPagedDatas<HistorySignalShowListModel>
    {
        /// <summary>
        /// 车辆编号
        /// </summary>
        public long VehicleID { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        [Display(Name = "StartDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [LegalDateTime(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateTimeError")]
        public string StartTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        [Display(Name = "ExpirationDate", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustInput")]
        [LegalDateTime(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "DateTimeError")]
        [StartEndDateTime("GPSStartTime", ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "TimeInterval")]
        public string EndTime { get; set; }
        /// <summary>
        /// 时间间隔
        /// </summary>
        [Display(Name = "InvTime", ResourceType = typeof(DisplayText))]
        public string InvTime { get; set; }
        /// <summary>
        /// 过滤零速
        /// </summary>
        [Display(Name = "IsSpeed", ResourceType = typeof(DisplayText))]
        public string IsSpeed { get; set; }
        /// <summary>
        /// 显示停车点
        /// </summary>
        [Display(Name = "showStopPoint", ResourceType = typeof(DisplayText))]
        public bool showStopPoint { get; set; }

        public List<HistorySignalShowListModel> Datas { get; set; }
    }


    /// <summary>
    /// 异常轨迹信息
    /// </summary>
    public class ExceptionSingalModel : ContainsPagedDatas<HistorySignalsModel>
    {
        /// <summary>
        /// 车辆编号
        /// </summary>
        public long VehicleID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        [Display(Name = "StartDate", ResourceType = typeof(DisplayText))]
        public string StartTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        [Display(Name = "ExpirationDate", ResourceType = typeof(DisplayText))]
        public string EndTime { get; set; }

        public int ID { get; set; }
        public int ExceptionTypeID { get; set; }
        public string ExName { get; set; }
        public string TerminalCode { get; set; }
        public int StrucID { get; set; }
        public string PlateNum { get; set; }
        public DateTime SignalStartTime { get; set; }
        public DateTime SignalEndTime { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public float Speed { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }

    }


    /// <summary>
    /// 后台获取数据格式
    /// </summary>
    public class HistorySignalsModel 
    {
        public long VehicleID { get; set; }

        public string VIN { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }

        public DateTime SignalDateTime { get; set; }

        public DateTime Time { get; set; }

        public double Speed { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public int Direction { get; set; }
        /// <summary>
        /// 里程
        /// </summary>
        public double Mileage { get; set; }
        /// <summary>
        /// 地图里程  后台计算得出
        /// </summary>
        public double MapMileage { get; set; }
        /// <summary>
        /// ACC状态
        /// </summary>
        public bool ACCState { get; set; }
        /// <summary>
        /// 是否盲区补点
        /// </summary>
        public bool IsBlind { get; set; }
        /// <summary>
        /// 油位
        /// </summary>
        public double OilHeight { get; set; }
        /// <summary>
        /// 滚筒
        /// </summary>
        public int RollerState { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long AlarmFlag { get; set; }
        /// <summary>
        /// 定位点状态
        /// </summary>
        //public bool PositioningState { get; set; }
        /// <summary>
        /// 匹配异常表时记录位
        /// </summary>
        public string ExceptionTypeID { get; set; }
        /// <summary>
        /// 异常名称
        /// </summary>
        public string ExName { get; set; }
        /// <summary>
        /// 是否停车点
        /// </summary>
        public bool IsStopPoint { get; set; }
        /// <summary>
        /// 停车点信息
        /// </summary>
        public string StopContent { get; set; }
        /// <summary>
        /// 开始停车时间（显示停车点列表信息用）
        /// </summary>
        public string StartStopVehicleTime { get; set; }
        /// <summary>
        /// 停车时长（显示停车点列表信息用）
        /// </summary>
        public string StopVehicleDuration { get; set; }
        /// <summary>
        /// 门磁状态
        /// </summary>
        public bool DoorsensorFlag { get; set; }
        /// <summary>
        /// 是否气压卸料
        /// </summary>
        public bool PressureFlag { get; set; }
    }


    /// <summary>
    /// 轨迹导出
    /// </summary>
    public class HistorySignalExportModel : ContainsPagedDatas<HistorySignalsModel>
    {
        public long VehicleID { get; set; }
        public string VIN { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 车牌代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }
        /// <summary>
        /// 单位编号
        /// </summary>
        public int StrucID { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 里程
        /// </summary>
        public string Mileage { get; set; }
        /// <summary>
        /// 地图里程
        /// </summary>
        public string MapMileage { get; set; }
        /// <summary>
        /// GPS时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public string Speed { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public string Angle { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 异常类型
        /// </summary>
        public int ExceptionTypeID { get; set; }
        /// <summary>
        /// 时间间隔
        /// </summary>
        [Display(Name = "InvTime", ResourceType = typeof(DisplayText))]
        public string InvTime { get; set; }
        /// <summary>
        /// 过滤零速
        /// </summary>
        [Display(Name = "IsSpeed", ResourceType = typeof(DisplayText))]
        public string IsSpeed { get; set; }
    }


    /// <summary>
    /// 链接服务器模型
    /// </summary>
    public class GetLinkedServerName
    {
        public string LinkedServerName { get; set; }
    }

    public class Location
    {
        //经度
        public double Longitude { get; set; }
        //纬度
        public double Latitude { get; set; }
        //定位时间
        public DateTime SignalDateTime { get; set; }
        //盲区
        public bool IsBlind { get; set; }
        //报警位
        public string AlarmFlag { get; set; }
        //速度
        public string Speed { get; set; }
        //温度
        public string Temperature { get; set; }
        //ACC状态
        public bool ACCState { get; set; }
        //里程
        public string Mileage { get; set; }
    }

    public class MapInfo
    {
        public string name { get; set; }

        public List<double[]> path { get; set; }
        public List<string[]> remark { get; set; }
    }


    public class AllInfo {
        public List<HistorySignalShowListModel> historySignalShowListModel;
        public List<MapInfo> mapInfo;
    }
}

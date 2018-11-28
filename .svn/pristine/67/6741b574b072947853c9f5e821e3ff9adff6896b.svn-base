using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    /// <summary>
    /// 实时监控页面 实时信号树模型
    /// </summary>
    public class RealTimeSignalTreeModel
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        public long VID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }
        /// <summary>
        /// 车代号
        /// </summary>
        public string VN { get; set; }
        /// <summary>
        /// 车辆使用单位ID
        /// </summary>
        public int SID { get; set; }
        /// <summary>
        /// 车辆使用单位上级单位ID
        /// </summary>
        public int? SPID { get; set; }
        /// <summary>
        /// 使用单位名称
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 最后一笔信号速度
        /// </summary>
        public double Speed { get; set; }
        ///// <summary>
        ///// 最后一笔信号时间
        ///// </summary>
        //public DateTime PCTime { get; set; }
        /// <summary>
        /// 是否在线 1：在线 0：离线 （10分钟无信号算离线）
        /// </summary>
        public int IsOnline { get; set; }
        /// <summary>
        ///  是否行驶中 1：行驶 0：停车
        /// </summary>
        public int IsRunning { get; set; }


        /// <summary>
        /// 纬度
        /// </summary>
        public double? Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double? Longitude { get; set; }
    }


    public class RealTimeSignalModel
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        public long VID { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }
        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 使用单位名称
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 方向（角度）
        /// </summary>
        public double Direction { get; set; }
        /// <summary>
        /// 速率
        /// </summary>
        public double Speed { get; set; }
        /// <summary>
        /// 信号时间
        /// </summary>
        public DateTime SignalDateTime { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 图标文件夹名称
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 是否在线
        /// </summary>
        public int IsOnline { get; set; }
        /// <summary>
        /// 是否行驶中
        /// </summary>
        public int IsRunning { get; set; }
        /// <summary>
        /// 车辆里程表读数（km）
        /// </summary>
        public long Mileage { get; set; }
        /// <summary>
        /// 油料高度（cm）
        /// </summary>
        public long? OilHeight { get; set; }
        /// <summary>
        /// 温度（摄氏度）
        /// </summary>
        public string Temperature { get; set; }
        /// <summary>
        /// ACC状态 1:开  0:关
        /// </summary>
        public bool ACCState { get; set; }
        /// <summary>
        /// 滚筒状态  0:停转  1:正转  2:反转
        /// </summary>
        public int? RollerState { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public string PlateColor { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        public string VehicleType { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string  Address { get; set; }
        /// <summary>
        ///是否气压卸料
        /// </summary>
        public bool PressureFlag { get; set; }
        /// <summary>
        ///门磁开关
        /// </summary>
        public bool DoorsensorFlag { get; set; }
        /// <summary>
        ///押运员姓名
        /// </summary>
        public string EscortName { get; set; }
          /// <summary>
        ///押运员联系方式
        /// </summary>
        public string EscortPhone { get; set; }
          /// <summary>
        ///驾驶员姓名
        /// </summary>
        public string DriverName { get; set; }
        /// <summary>
        /// 驾驶员联系方式
        /// </summary>
        public string DriverPhone { get; set; }
        /// <summary>
        /// 车主姓名
        /// </summary>
        public string OwnersName { get; set; }
        /// <summary>
        /// 车主联系方式
        /// </summary>
        public string OwnersPhone { get; set; }
    }


    public class RealTimePolygonVehiclesPoints
    {
        public double L { get; set; }
        public double I { get; set; }
        public double lng { get; set; }
        public double lat { get; set; }
    }


    public class GetRectangleRealTimeSingalsModel
    {
        /// <summary>
        /// 最小经度
        /// </summary>
        public double LngMin { get; set; }
        /// <summary>
        /// 最大经度
        /// </summary>
        public double LngMax { get; set; }
        /// <summary>
        /// 最小纬度
        /// </summary>
        public double LatMin { get; set; }
        /// <summary>
        /// 最大纬度
        /// </summary>
        public double LatMax { get; set; }
    }


    public class GetCircleRealTimeSingalsModel
    {
        /// <summary>
        /// 半径
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// 中心点经度
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// 中心点纬度
        /// </summary>
        public double Lat { get; set; }
    }

    public class PointModel 
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
    }
}

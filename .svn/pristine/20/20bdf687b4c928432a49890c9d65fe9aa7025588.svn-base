using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Asiatek.Model
{
    /// <summary>
    /// 紧急告警异常
    /// </summary>
    public class EmergencyAlarmInfoModel
    {
        /// <summary>
        /// 异常消息ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }

        /// <summary>
        /// 车辆使用单位
        /// </summary>
        public string StrucName { get; set; }

        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }

        /// <summary>
        /// 异常开始时间
        /// </summary>
        public string StartDateTime { get; set; }

        /// <summary>
        /// 异常发生地点
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 异常状态
        /// 1：成功 ；2：超时
        /// </summary>
        public int Status { get; set; }
    }


    /// <summary>
    /// 实时异常(不包括紧急报警）
    /// </summary>
    public class RealTimeExceptionModel
    {
        /// <summary>
        /// 车辆使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 异常开始时间
        /// </summary>
        public string StartDateTime { get; set; }
        /// <summary>
        /// 异常发生地点
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 异常类型名称
        /// </summary>
        public string ExTypeName { get; set; }

        /// <summary>
        /// 阈值
        /// </summary>
        public string Threshold { get; set; }

        public string VIN { get; set; }
    }
    /// <summary>
    /// 年检类
    /// </summary>
    public class YearCheckAlarmModel {
        /// <summary>
        /// 年检时间
        /// </summary>
        public DateTime AnnualInspectionTime { get; set; }
       /// <summary>
       /// 设置的要求提前的天数
       /// </summary>
        public int RemindTimeSpan { get; set; }
        /// <summary>
        /// 年检时间2
        /// </summary>
        public DateTime AnnualInspectionTime1 { get; set; }
        /// <summary>
        /// 设置的要求提前的天数2
        /// </summary>
        public int RemindTimeSpan1 { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string StrucName { get; set; }
    }
    /// <summary>
    /// 更新紧急报警异常
    /// </summary>
    public class UpdateEmergencyAlarmModel
    {
        /// <summary>
        /// 异常ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 处理该紧急报警用户ID
        /// </summary>
        public int DealUserID { get; set; }
        /// <summary>
        /// 处理信息
        /// </summary>
        public string DealInfo { get; set; }
        /// <summary>
        /// 异常所属链接服务器
        /// </summary>
        public string LinkedServerName { get; set; }
        /// <summary>
        /// 处理是否超时
        /// </summary>
        public bool Timeout { get; set; }
    }

    /// <summary>
    /// 车辆保养
    /// </summary>
    public class MaintenanceCheckAlarmModel
    {
        /// <summary>
        /// 保养方案名称
        /// </summary>
        public string ScheduleName { get; set; }
        public int ScheduleID { get; set; }

        /// <summary>
        /// 到期规则
        /// </summary>
        public int RulesType { get; set; }

        /// <summary>
        /// 设定公里数
        /// </summary>
        public int? SettingMile { get; set; }

        /// <summary>
        /// 提前公里数
        /// </summary>
        public int? PreMile { get; set; }

        /// <summary>
        /// 设定时间数（天）
        /// </summary>
        public int? SettingDay { get; set; }

        /// <summary>
        /// 提前时间数（天）
        /// </summary>
        public int? PreDay { get; set; }

        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }
        public int VehicleID { get; set; }

        /// <summary>
        /// 初次保养里程
        /// </summary>
        public double FirstMaintenanceMile { get; set; }

        /// <summary>
        /// 初次保养时间
        /// </summary>
        public DateTime FirstMaintenanceTime { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string StrucName { get; set; }

        /// <summary>
        /// sql计算出来的预警里程，用于和设定的提前预警里程进行比较
        /// </summary>
        public int? alarmMile { get; set; }

        /// <summary>
        /// sql计算出来的预警时间，用于和设定的提前预警时间进行比较
        /// </summary>
        public int? alarmTime { get; set; }

        /// <summary>
        /// 实时监控页面右下方显示的到期保养的预警类型，对于里程或时间周期的，显示是其中一个预警还是两者都有预警
        /// </summary>
        public string AlarmInfo { get; set; }
    }

    #region 异常报表实体  含超速报表  疲劳驾驶报表 当天累计驾驶超时报表 超时停车报表
    public class ExceptionModel
    {
        public int ExceptionTypeID { get; set; }
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public double? ActualDuration { get; set; }
        /// <summary>
        /// 开始位置
        /// </summary>
        public string StartAddress { get; set; }
        /// <summary>
        /// 结束位置
        /// </summary>
        public string EndAddress { get; set; }
        /// <summary>
        /// 超速持续时间段内的最大速度值
        /// </summary>
        public double MaxSpeed { get; set; }
        /// <summary>
        /// 超速阈值
        /// </summary>
        public uint OverspeedThreshold { get; set; }
        /// <summary>
        /// 引发超速报警所需的最小持续时间
        /// </summary>
        public uint MinimumDuration { get; set; }
        /// <summary>
        /// 续驾驶时间门限(时间为秒)
        /// </summary>
        public uint ContinuousDrivingThreshold { get; set; }
        /// <summary>
        /// 最小休息时间(针对疲劳驾驶单位秒)
        /// </summary>
        public uint MinimumBreakTime { get; set; }
        /// <summary>
        /// 当天累计驾驶时间门限(当天累计驾驶超时)
        /// </summary>
        public uint DrivingTimeThreshold { get; set; }
        /// <summary>
        /// 最长停车时间阈值
        /// </summary>
        public uint MaximumParkingTime { get; set; }
        /// <summary>
        /// 停车时间
        /// </summary>
        public DateTime? ParkingTime { get; set; }
    }
    #endregion

    #region 异常报表实体  含紧急报警报表
    public class ExceptionsAndDealInfoModel
    {
        public int ExceptionTypeID { get; set; }
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public double? ActualDuration { get; set; }
        /// <summary>
        /// 开始位置
        /// </summary>
        public string StartAddress { get; set; }
        /// <summary>
        /// 结束位置
        /// </summary>
        public string EndAddress { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? DealTime { get; set; }
        /// <summary>
        /// 处理信息
        /// </summary>
        public string DealInfo { get; set; }
        /// <summary>
        ///  处理人
        /// </summary>
        public string DealUser { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

    }
    #endregion

    #region 异常报表实体  设备故障报表 电源异常报表  异常明细汇总报表
    public class ExceptionsEquipmentModel
    {
        public int ExceptionTypeID { get; set; }
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public double? ActualDuration { get; set; }
        /// <summary>
        /// 开始位置
        /// </summary>
        public string StartAddress { get; set; }
        /// <summary>
        /// 结束位置
        /// </summary>
        public string EndAddress { get; set; }
        /// <summary>
        /// 异常类型名称
        /// </summary>
        public string ExTypeName { get; set; }

    }
    #endregion

    #region 里程报表实体
    public class VehicleDistanceModel
    {
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 信号时间
        /// </summary>
        public DateTime SignalDateTime { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndDateTime { get; set; }
        /// <summary>
        /// 里程
        /// </summary>
        public double Distance { get; set; }


    }
    #endregion

    #region  温度相关
    /// <summary>
    /// 温度实体
    /// </summary>
    public class TemperModel
    {
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }
        /// <summary>
        /// 所属单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 信号时间
        /// </summary>
        public DateTime SignalDateTime { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public string Temperature { get; set; }
        /// <summary>
        /// ACC状态
        /// </summary>
        public bool ACCState { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public double Speed { get; set; }
    }

    /// <summary>
    /// 温度异常实体
    /// </summary>
    public class TemperExceptionModel
    {
        /// <summary>
        /// 所属单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 异常类型 ：101代表低温报警  102代表高温报警
        /// </summary>
        public int ExceptionTypeID { get; set; }
        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType { get; set; }
        /// <summary>
        /// 温度传感器报警设置中的安装位置
        /// </summary>
        public string InstallationPosition { get; set; }
        /// <summary>
        /// 报警极限温度
        /// </summary>
        public double? LimitValue { get; set; }
        /// <summary>
        /// 正常温度最大值
        /// </summary>
        public double HighestTemperature { get; set; }
        /// <summary>
        /// 正常温度最小值
        /// </summary>
        public double LowestTemperature { get; set; }
        /// <summary>
        /// 异常开始时间
        /// </summary>
        public DateTime? SignalStartTime { get; set; }
        /// <summary>
        /// 异常结束时间
        /// </summary>
        public DateTime? SignalEndTime { get; set; }
        /// <summary>
        /// 异常实际的持续时间
        /// </summary>
        public double? ActualDuration { get; set; }
    }
    #endregion

    #region 异常报表查询实体
    public class ExceptionSearchModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime SartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 车辆ID
        /// </summary>
        public long VehiclesID { get; set; }
        /// <summary>
        /// 异常汇总编号
        /// </summary>
        public int CollectID { get; set; }
        /// <summary>
        /// 异常类型
        /// </summary>
        public int ExceptionTypeID { get; set; }
        /// <summary>
        /// 昵称或用户名
        /// </summary>
        public string DealUserName { get; set; }
    }
    #endregion

    #region 异常类型枚举
    public enum ExceptionTypeEnum
    {
        /// <summary>
        /// 紧急报警
        /// </summary>
        EmergencyAlarmRpt = 1,
        /// <summary>
        /// 超速报表
        /// </summary>
        OverSpeedRpt = 2,
        /// <summary>
        /// 疲劳驾驶
        /// </summary>
        FatigueDrivingRpt = 5,
        /// <summary>
        /// 当天累计驾驶超时
        /// </summary>
        AccumulatedDrivingOvertime = 18,
        /// <summary>
        /// 超时停车
        /// </summary>
        OvertimeParking = 19
    }
    #endregion

    #region 设备异常大类枚举
    public enum ExceptionCollectEnum
    {
        /// <summary>
        /// 异常明细汇总
        /// </summary>
        ExDetailsSummaryRpt = -1,
        /// <summary>
        /// 设备故障
        /// </summary>
        EquipmentFailureRpt = 7,
        /// <summary>
        /// 电源异常
        /// </summary>
        PowerAbnormalRpt = 10,
    }

    #endregion



    #region 离线报表相关
    public class OfflineModel 
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 掉线车辆最后定位时间
        /// </summary>
        public DateTime LastSignalDateTime { get; set; }
        /// <summary>
        /// 掉线天数
        /// </summary>
        public string OfflineDays { get; set; }
        /// <summary>
        /// 查询的总的车辆数
        /// </summary>
        public string VehicleNumber { get; set; }
    }
    #endregion

    #region 异常处理报表相关
    public class ExceptionHandleModel
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 异常类别
        /// </summary>
        public string ExceptionType { get; set; }
        /// <summary>
        /// 异常发生时间
        /// </summary>
        public DateTime? ExceptionBeginDateTime { get; set; }
        /// <summary>
        /// 异常发生地址
        /// </summary>
        public string ExceptionBeginAddress { get; set; }
        /// <summary>
        /// 异常结束时间
        /// </summary>
        public DateTime? ExceptionEndDateTime { get; set; }
        /// <summary>
        /// 异常结束地址
        /// </summary>
        public string ExceptionEndAddress { get; set; }
        /// <summary>
        /// 处理人姓名
        /// </summary>
        public string DealUserName { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? DealDateTime { get; set; }
        /// <summary>
        /// 处理信息
        /// </summary>
        public string DealInfo { get; set; }
    }
    #endregion
    #region MyRegion
    public class ServerInfoModel
    {
        public string LinkedServerName { get; set; }
    }
    #endregion

    #region 待处理异常
    /// <summary>
    /// 待处理异常查询模型
    /// </summary>
    public class NeedDealExceptionSearchModel
    {
        /// <summary>
        /// 异常开始时间的开始
        /// </summary>
        public DateTime BeginDateTime { get; set; }
        /// <summary>
        /// 异常开始时间的结束
        /// </summary>
        public DateTime EndDateTime { get; set; }
        /// <summary>
        /// 是否已结束
        /// true：结束
        /// false：未结束
        /// NULL：全部
        /// </summary>
        public int EndState { get; set; }

        public IEnumerable<SelectListItem> EndStates { get; set; }
    }


    /// <summary>
    /// 需要处理的异常列表模型
    /// </summary>
    public class NeedDealExceptionListModel
    {
        /// <summary>
        /// 异常ID
        /// </summary>
        public long ExID { get; set; }
        /// <summary>
        /// 异常名称
        /// </summary>
        [Display(Name = "ExName", ResourceType = typeof(DisplayText))]
        public string ExName { get; set; }
        /// <summary>
        /// 车代号
        /// </summary>
        [Display(Name = "VehicleName", ResourceType = typeof(DisplayText))]
        public string VehicleName { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalCode", ResourceType = typeof(DisplayText))]
        public string TerminalCode { get; set; }
        /// <summary>
        /// 异常发生信号时间
        /// </summary>
        [Display(Name = "ExSignalStartTime", ResourceType = typeof(DisplayText))]
        public DateTime SignalStartTime { get; set; }
        /// <summary>
        /// 发生地址
        /// </summary>
        [Display(Name = "ExStartAddress", ResourceType = typeof(DisplayText))]
        public string StartAddress { get; set; }
        /// <summary>
        /// 异常结束信号时间
        /// </summary>
        [Display(Name = "ExSignalEndTime", ResourceType = typeof(DisplayText))]
        public DateTime? SignalEndTime { get; set; }
        /// <summary>
        /// 结束地址
        /// </summary>
        [Display(Name = "ExEndAddress", ResourceType = typeof(DisplayText))]
        public string EndAddress { get; set; }
        /// <summary>
        /// 异常所属链接服务器
        /// </summary>
        public string LinkedServerName { get; set; }
        /// <summary>
        /// 最大速度
        /// </summary>
        public string MaxSpeed { get; set; }
        /// <summary>
        /// 速度阈值
        /// </summary>
        public string OverspeedThreshold { get; set; }
    }


    public class DealAlarmModel
    {
        /// <summary>
        /// 待处理异常ID
        /// </summary>
        public long EID { get; set; }
        /// <summary>
        /// 处理人ID
        /// </summary>
        public int DealUserID { get; set; }
        /// <summary>
        /// 处理信息
        /// </summary>
        public string DealInfo { get; set; }
        /// <summary>
        /// 异常所属链接服务器名
        /// </summary>
        public string LinkedServerName { get; set; }
    }
    #endregion

    #region 夜间行驶报表
    public class NightDrivingModel
    {
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? StartDateTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public double? ActualDuration { get; set; }
        /// <summary>
        /// 开始位置
        /// </summary>
        public string StartAddress { get; set; }
        /// <summary>
        /// 结束位置
        /// </summary>
        public string EndAddress { get; set; }
    }
    #endregion

    #region 南钢区域超速报表

    #region 南钢区域超速报表V 1.0（已暂停使用，不建议删除）
    ///// <summary>
    ///// 南钢区域超速报表实体表V1.0（已暂停使用，不建议删除）
    ///// </summary>
    //public class NGAreaOverSpeedModel
    //{
    //    /// <summary>
    //    /// 使用单位
    //    /// </summary>
    //    public string StrucName { get; set; }
    //    /// <summary>
    //    /// 超速厂区(电子围栏的名称作为超区名称)
    //    /// </summary>
    //    public string FenceName { get; set; }
    //    /// <summary>
    //    /// 车辆代号
    //    /// </summary>
    //    public string VehicleName { get; set; }
    //    /// <summary>
    //    /// 起始时间
    //    /// </summary>
    //    public DateTime StartDateTime { get; set; }
    //    /// <summary>
    //    /// 截止时间
    //    /// </summary>
    //    public DateTime EndDateTime { get; set; }
    //    /// <summary>
    //    /// 持续时间
    //    /// </summary>
    //    public double ActualDuration { get; set; }
    //    /// <summary>
    //    /// 限速值
    //    /// </summary>
    //    public double MaxSpeed { get; set; }
    //    /// <summary>
    //    ///  时速（超速持续时间段内的最大速度值）
    //    /// </summary>
    //    public double MaxOverSpeed { get; set; }
    //    /// <summary>
    //    /// 超速次数
    //    /// </summary>
    //    public int OverSpeedTimes { get; set; }
    //    /// <summary>
    //    /// 罚款金额
    //    /// </summary>
    //    public double Penalty { get; set; }
    //} 
    #endregion

    #region 南钢区域超速报表V 2.0
    /// <summary>
    /// 南钢区域超速报表实体表V2.0
    /// </summary>
    public class NGAreaAverageOverSpeedModel
    {
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 超速厂区(电子围栏的名称作为超区名称)
        /// </summary>
        public string FenceName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        ///  时速（超速持续时间段内的平均速度值）
        /// </summary>
        public double AverageOverSpeed { get; set; }
        /// <summary>
        /// 限速值
        /// </summary>
        public double MaxSpeed { get; set; }
        /// <summary>
        /// 超速比例
        /// </summary>
        public string OverSpeedPercent { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndDateTime { get; set; }
        /// <summary>
        /// 持续时间
        /// </summary>
        public double ActualDuration { get; set; }
        /// <summary>
        /// 超速次数
        /// </summary>
        public int OverSpeedTimes { get; set; }
        /// <summary>
        /// 罚款金额
        /// </summary>
        public double Penalty { get; set; }
    }
    #endregion

    #endregion

    #region 马钢嘉华卸料异常报表
    /// <summary>
    /// 马钢嘉华卸料异常报表Model
    /// </summary>
    public class MGJHExpForIllegalDischargModel
    {
        /// <summary>
        /// 磅单号
        /// </summary>
        public string PoundSheetCode { get; set; }

        /// <summary>
        /// 客商名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string ShippingAddress { get; set; }

        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }

        /// <summary>
        /// 异常开始时间
        /// </summary>
        public DateTime ServerStartTime { get; set; }

        /// <summary>
        /// 异常结束时间
        /// </summary>
        public DateTime ServerEndTime { get; set; }
        
        /// <summary>
        /// 持续时长（秒）
        /// </summary>
        public int ActualDuration { get; set; }
    }
    #endregion

    #region 马钢嘉华营运报表
    /// <summary>
    /// 马钢嘉华营运报表Model
    /// </summary>
    public class MGJHServiceModel
    {
        /// <summary>
        /// 磅单号
        /// </summary>
        public string PoundSheetCode { get; set; }

        /// <summary>
        /// 客商名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public string ShippingAddress { get; set; }
        
        /// <summary>
        /// 实际卸货点
        /// </summary>
        public string RealShippingAddressName { get; set; }

        /// <summary>
        /// 卸货点区域
        /// </summary>
        public string RealShippingAddressArea { get; set; }

        /// <summary>
        /// 车代号
        /// </summary>
        public string VehicleName { get; set; }

        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }

        /// <summary>
        /// 磅单接收时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 车辆出厂时间
        /// </summary>
        public DateTime? LeaveDeliveryPointTime { get; set; }

        /// <summary>
        /// 到达卸货点时间
        /// </summary>
        public DateTime? ReachUploadPointTime { get; set; }

        /// <summary>
        /// 运送时长
        /// </summary>
        public int TransportDuration { get; set; }

        /// <summary>
        /// 离开卸货点时间
        /// </summary>
        public DateTime? LeaveUploadPointTime { get; set; }

        /// <summary>
        /// 停留时长
        /// </summary>
        public int StayDuration { get; set; }

    }
    #endregion

    #region 门岗异常报表
    public class GateExceptionSearchModel
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        public long VehicleID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime SartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 异常类型   0-门岗异常  1-电子围栏异常
        /// </summary>
        public int ExceptionType { get; set; }
    }

    public class GateDataModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 门岗名称
        /// </summary>
        public string PassGate { get; set; }
        /// <summary>
        /// 进出门岗类型
        /// </summary>
        public string InOrOut { get; set; }
        /// <summary>
        /// 进出门岗时间
        /// </summary>
        public DateTime InOrOutTime { get; set; }
    }

    public class EfDataModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 异常类型=0  这边查询禁入的异常  后台管理中心配置禁入的电子围栏
        /// </summary>
        public int ExceptionType { get; set; }
        /// <summary>
        /// 异常开始时间
        /// </summary>
        public DateTime SignalStartTime { get; set; }
        /// <summary>
        /// 异常结束时间
        /// </summary>
        public DateTime? SignalEndTime { get; set; }
    }

    public class GateExceptionModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 门岗名称
        /// </summary>
        public string PassGate { get; set; }
        /// <summary>
        /// 进出门岗类型
        /// </summary>
        public string InOrOut { get; set; }
        /// <summary>
        /// 进出门岗时间
        /// </summary>
        public string InOrOutTime { get; set; }
        /// <summary>
        /// 电子围栏的异常详细信息，表示是开始时间不匹配还是结束时间不匹配还是开始结束时间都不匹配
        /// </summary>
        public string ExceptionInfo { get; set; }
        /// <summary>
        /// 异常开始时间
        /// </summary>
        public string SignalStartTime { get; set; }
        /// <summary>
        /// 异常结束时间
        /// </summary>
        public string SignalEndTime { get; set; }
    }
    #endregion

    #region ACCON时长统计报表
    public class ACCONStatisticSearchModel
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        //public long VehicleID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime SartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 车架号
        /// </summary>
        public string VIN { get; set; }
    }

    public class ACCONStatisticHisgModel
    {
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 信号时间
        /// </summary>
        public DateTime SignalDateTime { get; set; }
        /// <summary>
        /// ACC状态
        /// </summary>
        public bool ACCState { get; set; }
    }

    public class ACCONStatisticDataModel
    {
        /// <summary>
        /// 使用单位
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 车辆代号
        /// </summary>
        public string VehicleName { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndDateTime { get; set; }
        /// <summary>
        /// 统计时长（小时）
        /// </summary>
        public double TotalTime { get; set; }
    }
    #endregion
}

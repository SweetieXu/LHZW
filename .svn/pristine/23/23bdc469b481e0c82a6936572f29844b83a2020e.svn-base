using Asiatek.Common;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asiatek.Model.TerminalSetting
{
    #region 下发文本消息实体
    public class SendTextMessageModel
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public byte Flags { get; set; }
        /// <summary>
        /// 文本消息
        /// </summary>
        public string MessageText { get; set; }
        /// <summary>
        /// 车辆终端集合
        /// </summary>
        public List<VehiclesModel> ListVehicles { get; set; }
    }
    #endregion

    #region 下发终端设置实体
    public class TerminalSettingsIssuedModel
    {
        /// <summary>
        /// 终端心跳间隔 s
        /// </summary>
        public uint? 终端心跳发送间隔 { get; set; }
        /// <summary>
        /// 主服务器IP地址或域名
        /// </summary>
        public string 主服务器IP地址或域名 { get; set; }
        /// <summary>
        /// 备份服务器IP地址或域名
        /// </summary>
        public string 备份服务器IP地址或域名 { get; set; }
        /// <summary>
        /// 服务器TCP端口
        /// </summary>
        public uint? 服务器TCP端口 { get; set; }
        /// <summary>
        /// 休眠时汇报时间间隔 s
        /// </summary>
        public uint? 休眠时汇报时间间隔 { get; set; }
        /// <summary>
        /// 紧急报警时汇报时间间隔 s
        /// </summary>
        public uint? 紧急报警时汇报时间间隔 { get; set; }
        /// <summary>
        /// 缺省时间汇报间隔 s
        /// </summary>
        public uint? 缺省时间汇报间隔 { get; set; }
        /// <summary>
        /// 最高速度 km/h
        /// </summary>
        public uint? 最高速度 { get; set; }
        /// <summary>
        /// 超速持续时间 s
        /// </summary>
        public uint? 超速持续时间 { get; set; }
        /// <summary>
        /// 连续驾驶时间门限 s
        /// </summary>
        public uint? 连续驾驶时间门限 { get; set; }
        /// <summary>
        /// 当天累计驾驶时间门限 s
        /// </summary>
        public uint? 当天累计驾驶时间门限 { get; set; }
        /// <summary>
        /// 最小休息时间 s
        /// </summary>
        public uint? 最小休息时间 { get; set; }
        /// <summary>
        /// 最长停车时间 s
        /// </summary>
        public uint? 最长停车时间 { get; set; }
        /// <summary>
        /// 超速报警预警差值  km/h
        /// 最大6553.5
        /// </summary>
        public double? 超速报警预警差值 { get; set; }
        /// <summary>
        /// 疲劳驾驶预警差值  s
        /// </summary>
        public ushort? 疲劳驾驶预警差值 { get; set; }
        /// <summary>
        /// 车辆里程表读数 km  最大值429496729.5
        /// </summary>
        public double? 车辆里程表读数 { get; set; }
        /// <summary>
        /// 公安交通管理部门颁发的机动车号牌
        /// 7位或8位
        /// </summary>
        public string 公安交通管理部门颁发的机动车号牌 { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public byte? 车牌颜色 { get; set; }
        /// <summary>
        /// 车辆终端集合
        /// </summary>
        public List<VehiclesModel> ListVehicles { get; set; }
    }
    #endregion

    #region 终端查询实体
    public class TerminalSettingsReadModel
    {
        /// <summary>
        /// 车辆终端集合
        /// </summary>
        public List<VehiclesModel> ListVehicles { get; set; }
        /// <summary>
        /// 选中的需要查询的参数
        /// </summary>
        public uint[] CheckedParamArray { get; set; }
    }
    #endregion

    #region 终端日志查询

    #region 终端日志查询搜索实体类
    public class TerminalSettingLogSearchModel
    {
        /// <summary>
        /// 单位名称
        /// </summary>
        [Display(Name = "TerminalSetting_CompanyName", ResourceType = typeof(UIText))]
        public string StrucName { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "TerminalSetting_PlateNumber", ResourceType = typeof(UIText))]
        public string PlateNum { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalSetting_TerminalCode", ResourceType = typeof(UIText))]
        public string TerminalCode { get; set; }
        /// <summary>
        /// 设置类型
        /// </summary>
        [Display(Name = "TerminalSetting_SettingType", ResourceType = typeof(UIText))]
        public TerminalSettingTypeEnum? SettingType { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        [Display(Name = "TerminalSetting_StartTime", ResourceType = typeof(UIText))]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "TerminalSetting_EndTime", ResourceType = typeof(UIText))]
        public DateTime? EndTime { get; set; }
    }

    #endregion

    #region 终端日志查询列表实体
    public class TerminalSettingLogListModel
    {
        public long ID { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        [Display(Name = "TerminalSetting_CompanyName", ResourceType = typeof(UIText))]
        public string StrucName { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalSetting_TerminalCode", ResourceType = typeof(UIText))]
        public string TerminalCode { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "TerminalSetting_PlateNumber", ResourceType = typeof(UIText))]
        public string PlateNum { get; set; }
        /// <summary>
        /// 设置类型
        /// </summary>
        [Display(Name = "TerminalSetting_SettingType", ResourceType = typeof(UIText))]
        public TerminalSettingTypeEnum SetType { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [Display(Name = "SetDTime", ResourceType = typeof(DisplayText))]
        public DateTime SetDTime { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [Display(Name = "SetUser", ResourceType = typeof(DisplayText))]
        public string SetUserName { get; set; }
    }
    #endregion

    #region 终端日志详情实体
    public class TerminalSettingLogDetailModel
    {
        /// <summary>
        /// 终端号
        /// </summary>
        [Display(Name = "TerminalSetting_TerminalCode", ResourceType = typeof(UIText))]
        public string TerminalCode { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "TerminalSetting_PlateNumber", ResourceType = typeof(UIText))]
        public string PlateNum { get; set; }
        /// <summary>
        /// 设置类型
        /// </summary>
        [Display(Name = "TerminalSetting_SettingType", ResourceType = typeof(UIText))]
        public TerminalSettingTypeEnum SetType { get; set; }
        /// <summary>
        /// 设置信息
        /// </summary>
        [Display(Name = "SetInfo", ResourceType = typeof(DisplayText))]
        public string SetInfo { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        [Display(Name = "Succeeded", ResourceType = typeof(DisplayText))]
        public bool Succeeded { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        [Display(Name = "Result", ResourceType = typeof(DisplayText))]
        public string Result { get; set; }
        /// <summary>
        /// 外网IP
        /// </summary>
        [Display(Name = "WanIP", ResourceType = typeof(DisplayText))]
        public string WanIP { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [Display(Name = "SetDTime", ResourceType = typeof(DisplayText))]
        public DateTime SetDTime { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [Display(Name = "SetUser", ResourceType = typeof(DisplayText))]
        public string SetUserName { get; set; }
    }
    #endregion

    #endregion

    #region 车辆信息实体
    public class VehiclesModel
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
        /// <summary>
        /// 终端号
        /// </summary>
        public string TerminalCode { get; set; }
    }
    #endregion

    #region 终端类型枚举
    public enum TerminalSettingTypeEnum : byte
    {
        /// <summary>
        /// 未知操作
        /// </summary>
        [EnumDescription("UIText", "TerminalSetting_Type_None")]
        None = 1,

        /// <summary>
        /// 文本消息
        /// </summary>
        [EnumDescription("UIText", "TerminalSetting_Type_TextMessage")]
        TextMessage = 2,
        /// <summary>
        /// 终端设置下发
        /// </summary>
        [EnumDescription("UIText", "TerminalSetting_Type_TerminalSetup_Update")]
        TerminalSetup_Update = 3,
        /// <summary>
        /// 终端设置查询
        /// </summary>
        [EnumDescription("UIText", "TerminalSetting_Type_TerminalSetup_Query")]
        TerminalSetup_Query = 4
    }
    #endregion

    #region 部分终端信息实体 用于下发成功后修改TMS中终端设置的值的
    public class TerminalSettingsSectionModel
    {
         /// <summary>
        /// 最高速度 km/h
        /// </summary>
        public uint? 最高速度 { get; set; }
        /// <summary>
        /// 超速持续时间 s
        /// </summary>
        public uint? 超速持续时间 { get; set; }
          /// <summary>
        /// 连续驾驶时间门限 s
        /// </summary>
        public uint? 连续驾驶时间门限 { get; set; }
        /// <summary>
        /// 最小休息时间 s
        /// </summary>
        public uint? 最小休息时间 { get; set; }
        /// <summary>
        /// 当天累计驾驶时间门限 s
        /// </summary>
        public uint? 当天累计驾驶时间门限 { get; set; }
        /// <summary>
        /// 最长停车时间 s
        /// </summary>
        public uint? 最长停车时间 { get; set; }
    }
    #endregion
}
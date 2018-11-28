using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    public class RepairRecordSearchModel {
        /// <summary>
        /// 车牌号
        /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
        public string PlateNum { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
       [Display(Name = "Status", ResourceType = typeof(DisplayText))]
        public string Status { get; set; }
       [Display(Name = "RepairStartTime", ResourceType = typeof(DisplayText))]
       public DateTime RepairStartTimestart{ get; set; }
       public DateTime RepairStartTimeend { get; set; } 
    }
   public class RepairRecordModel
    {
       /// <summary>
       /// 车牌号
       /// </summary>
        [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
       public string PlateNum{get;set;}
       /// <summary>
       /// 司机
       /// </summary>
        [Display(Name = "DriverName", ResourceType = typeof(DisplayText))]
       public string DriverName{get;set;}
       /// <summary>
       /// 联系方式
       /// </summary> 
        [Display(Name = "DriverPhone", ResourceType = typeof(DisplayText))]
       public string DriverPhone{get;set;}
       /// <summary>
       /// 维修开始时间
       /// </summary>
        [Display(Name = "RepairStartTime", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
       public string RepairStartTime{get;set;}
       /// <summary>
       /// 维修结束时间
       /// </summary>
       [Display(Name = "RepairEndTime", ResourceType = typeof(DisplayText))]
       [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
       public string RepairEndTime{get;set;}
       /// <summary>
       /// 维修类型
       /// </summary>
       [Display(Name = "RepairType", ResourceType = typeof(DisplayText))]
       public string RepairType{get;set;}
       /// <summary>
       /// 总价格
       /// </summary>
       [Display(Name = "TotalPrice", ResourceType = typeof(DisplayText))]
       public string TotalPrice { get; set; }
       /// <summary>
       /// 明细
       /// </summary>
       public List<RepairRecordDetail> RepairRecordDetail { get; set; }
       /// <summary>
       /// id
       /// </summary>
       public int ID { get; set; }
       /// <summary>
       /// 状态为0为未审核，1为审核
       /// </summary>
        [Display(Name = "Status", ResourceType = typeof(DisplayText))]
       public int Status { get; set; }
       [Display(Name = "PlateNum", ResourceType = typeof(DisplayText))]
       [Required(ErrorMessageResourceType = typeof(DataAnnotations), ErrorMessageResourceName = "MustSelect")]
       public int LinkedVehicleID { get; set; }

       [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
       public string Remark { get; set; }
        [Display(Name = "OperateUserName", ResourceType = typeof(DisplayText))]
       public string OperateUserName { get; set; }
        [Display(Name = "AuditUserName", ResourceType = typeof(DisplayText))]
       public string AuditUserName { get; set; }
    }
    public class RepairRecordDetail{
        /// <summary>
        /// 维修项目
        /// </summary>
  [Display(Name = "ProjectName", ResourceType = typeof(DisplayText))]
    public string ProjectName{get;set;}
        /// <summary>
        /// 使用配件
        /// </summary>
 [Display(Name = "PartsName", ResourceType = typeof(DisplayText))]
        public string PartsName{get;set;}
        /// <summary>
        /// 配件型号
        /// </summary>
         [Display(Name = "PartsVersion", ResourceType = typeof(DisplayText))]
        public string PartsVersion{get;set;}
        /// <summary>
        /// 配件数量
        /// </summary>
             [Display(Name = "PartsNum", ResourceType = typeof(DisplayText))]
        public int PartsNum{get;set;}
        /// <summary>
        /// 单位
        /// </summary>
       [Display(Name = "Unit", ResourceType = typeof(DisplayText))]
        public string Unit{get;set;}
        /// <summary>
        /// 单价
        /// </summary>
       [Display(Name = "Price", ResourceType = typeof(DisplayText))]
        public string Price{get;set;}
        /// <summary>
        /// 材料费
        /// </summary>
       [Display(Name = "MaterialCost", ResourceType = typeof(DisplayText))]
        public string MaterialCost{get;set;}
        /// <summary>
        /// 工时费
        /// </summary>
        [Display(Name = "TimeFee", ResourceType = typeof(DisplayText))]
        public string TimeFee{get;set;}
        public int LinkedRecordID { get; set; }

    }
    public class RecordID {
        public int ID { get; set; }
        public string idList { get; set; }
    }
}

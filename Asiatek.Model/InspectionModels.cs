using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    /// <summary>
    /// 查岗信息
    /// </summary>
    public class InspectionListModel
    {
        /// <summary>
        /// 查岗ID
        /// </summary>
        public UInt32 ID { get; set; }
        /// <summary>
        /// 查岗内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 查岗时间
        /// </summary>
        public string CheckDateTime { get; set; }
        /// <summary>
        /// 上级平台
        /// </summary>
        public string PlatformName { get; set; }
    }


    public class InspectionModel
    {
        /// <summary>
        /// 查岗类型
        /// </summary>
        public string ObjType { get; set; }
        /// <summary>
        /// 查岗对象ID
        /// </summary>
        public string ObjID { get; set; }
        /// <summary>
        /// 接入码
        /// </summary>
        public string AccessCode { get; set; }
        /// <summary>
        /// 协议版本号
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 发送查岗的上级平台名
        /// </summary>
        public string PlatformName { get; set; }
        /// <summary>
        /// 是否已被回复
        /// </summary>
        public bool Flag { get; set; }
        public int M1 { get; set; }
        public int IA1 { get; set; }
        public int IC1 { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Asiatek.CustomDataAnnotations
{
    /// <summary>
    ///  提供使用 jQuery 验证插件远程验证程序的特性。
    ///  由于RemoteAttribute无法对应用了该特性的属性进行模型绑定时的验证
    ///  一旦跳过javascript验证将不安全
    ///  所以继承RemoteAttribute实现AsiatekRemoteAttribute
    ///  为了在重写的ModelBinder中获得需要调用的具体方法来实现验证
    ///  让特性公开了AreaName、ControllerName、ActionName
    ///  作者：戴天辰
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AsiatekRemoteAttribute : RemoteAttribute
    {
        public AsiatekRemoteAttribute(string action, string controller, string areaName)
            : base(action, controller, areaName)
        {
            this.AreaName = areaName;
            this.ControllerName = controller;
            this.ActionName = action;
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName { get; set; }
    }
}

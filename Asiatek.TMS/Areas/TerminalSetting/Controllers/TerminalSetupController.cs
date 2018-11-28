using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using Asiatek.Model.TerminalSetting;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.TerminalOperation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    public partial class TerminalSetupController : BaseController
    {
        public ActionResult GetVehiclesListByCurrentUserID(string CompanyName, string PlateNumOrTerminalCode)
        {
            StrucVehiclesListModel[] list = InternalGetVehiclesListByCurrentUserID(CompanyName, PlateNumOrTerminalCode);
            List<dynamic> jsonObject = new List<dynamic>();
            for (int i = 0; i != list.Length; i++)
            {
                dynamic struc = new ExpandoObject();
                struc.state = new ExpandoObject();
                struc.state.expanded = false;
                struc.text = list[i].CompanyName;
                var vehicles = new List<dynamic>();
                for (int j = 0; j != list[i].Vehicles.Length; j++)
                {
                    dynamic v = new ExpandoObject();
                    v.text = list[i].Vehicles[j].Key;
                    v.tags = list[i].Vehicles[j].Value;
                    vehicles.Add(v);
                }
                struc.nodes = vehicles;
                jsonObject.Add(struc);
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject));
        }

        /// <summary>
        /// 根据条件获取当前用户能查看的所有车辆，参数的两个条件由 AND 组合
        /// </summary>
        /// <param name="CompanyName">公司名称</param>
        /// <param name="PlateNumOrTerminalCode">车牌号或终端号</param>
        private StrucVehiclesListModel[] InternalGetVehiclesListByCurrentUserID(string CompanyName, string PlateNumOrTerminalCode)
        {
            int userid = GetUserSession().UserId;

            var map = new Dictionary<string, List<KeyValuePair<string, string>>>();
            var dt = TerminalSettingsBLL.GetVehicleList(userid, CompanyName, PlateNumOrTerminalCode);
            foreach (DataRow dr in dt.Rows)
            {
                var vehicle = new KeyValuePair<string, string>((string)dr["PlateNum"], (string)dr["TerminalCode"]);
                string StrucName = (string)dr["StrucName"];

                List<KeyValuePair<string, string>> vehiclesList;
                if (!map.ContainsKey(StrucName))
                {
                    vehiclesList = new List<KeyValuePair<string, string>>();
                    map.Add(StrucName, vehiclesList);
                }
                else
                {
                    vehiclesList = map[StrucName];
                }
                vehiclesList.Add(vehicle);
            }

            var ret = new StrucVehiclesListModel[map.Values.Count];
            var Keys = map.Keys.ToArray();
            for (int i = 0; i != Keys.Length; i++)
            {
                ret[i] = new StrucVehiclesListModel();
                ret[i].CompanyName = Keys[i];
                ret[i].Vehicles = map[Keys[i]].ToArray();
            }
            return ret;
        }

        /// <summary>
        /// 获取WEB客户端IP地址
        /// </summary>
        private string GetRemoteAddress()
        {
            string userIP = "未获取用户IP";
            var Context = this.HttpContext;
            try
            {
                if (Context == null || Context.Request == null || Context.Request.ServerVariables == null)
                {
                    return "";
                }

                string CustomerIP = "";

                //CDN加速后取到的IP
                CustomerIP = Context.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                if (Context.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (CustomerIP == null)
                    {
                        CustomerIP = Context.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    CustomerIP = Context.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
                {
                    return Context.Request.UserHostAddress;
                }
                return CustomerIP;
            }
            catch { }

            return userIP;
        }
    }

    /// <summary>
    /// 转换Web请求数据为基元类型数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class BasicArrayBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Type type = typeof(T);

            if (!type.IsPrimitive && type != typeof(string))
                throw new Exception("无法绑定非基元类型。");

            string key = bindingContext.ModelName + "[]";
            //从请求中获取提交的参数数据
            var data = controllerContext.HttpContext.Request.Form[key] as string;
            if (string.IsNullOrEmpty(data))
                return Array.CreateInstance(type, 0);
            string[] strArray = data.Split(',');

            Array array = Array.CreateInstance(typeof(T), strArray.Length);
            for (int i = 0; i != strArray.Length; i++)
            {
                array.SetValue(ConvertSimpleType(strArray[i], type), i);
            }
            return array;
        }

        private static object ConvertSimpleType(object value, Type destinationType)
        {
            object returnValue;

            //string
            if ((value == null) || destinationType.IsInstanceOfType(value))
                return value;

            string str = value as string;
            if (string.IsNullOrEmpty(str))
                return null;

            TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
            {
                converter = TypeDescriptor.GetConverter(value.GetType());
            }
            if (!flag && !converter.CanConvertTo(destinationType))
            {
                throw new InvalidOperationException("无法转换成类型：" + value.ToString() + "==>" + destinationType);
            }
            try
            {
                returnValue = flag ? converter.ConvertFrom(null, null, value) : converter.ConvertTo(null, null, value, destinationType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("类型转换出错：" + value.ToString() + "==>" + destinationType, e);
            }
            return returnValue;
        }
    }

    class ObjectBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string key = bindingContext.ModelName;
            var data = controllerContext.HttpContext.Request.Form[key] as string;

            if (string.IsNullOrEmpty(data))
                return null;

            JObject jsonBody = JObject.Parse(data);
            JsonSerializer js = new JsonSerializer();
            object obj = js.Deserialize(jsonBody.CreateReader(), typeof(T));
            return obj;
        }
    }
}

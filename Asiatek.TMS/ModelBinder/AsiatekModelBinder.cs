using Asiatek.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.ModelBinder
{
    /// <summary>
    /// 新的模型绑定器
    /// 增加了对AsiatekRemoteAttribute的支持
    /// 使其可以进行绑定时验证
    /// 作者：戴天辰
    /// </summary>
    public class AsiatekModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// 使用指定的控制器上下文、绑定上下文和指定的属性描述符来绑定指定的属性。
        /// 当模型中的属性应用了AsiatekRemoteAttribute特性时，需要执行置顶的方法进行后台验证
        /// </summary>
        /// <param name="controllerContext">运行控制器的上下文。上下文信息包括控制器、HTTP 内容、请求上下文和路由数据。</param>
        /// <param name="bindingContext">绑定模型的上下文。上下文包含模型对象、模型名称、模型类型、属性筛选器和值提供程序等信息。</param>
        /// <param name="propertyDescriptor">描述要绑定的属性。该描述符提供组件类型、属性类型和属性值等信息。它还提供用于获取或设置属性值的方法。</param>
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            //查找当前待绑定模型是否有应用了AsiatekRemoteAttribute的属性
            var myAttr = propertyDescriptor.Attributes.OfType<AsiatekRemoteAttribute>().FirstOrDefault();

            if (myAttr != null)//特性不为NULL表示当前模型有属性应用了该特性
            {
                var allControllerTypes = GetControllerTypes();//全部的Controller

                //在所有Controller中寻找名字在AsiatekRemoteAttribute中设定的需要进行验证的方法所在的Controller
                var controllerType = allControllerTypes.FirstOrDefault(t => CheckController(t, myAttr.AreaName, myAttr.ControllerName));
                if (controllerType != null)//如果找到了
                {
                    //反射调用具体的方法

                    //查找Controller中的Action方法
                    var methodInfo = controllerType.GetMethod(myAttr.ActionName);

                    if (methodInfo != null)
                    {
                        //调用方法，得到验证的返回结果
                        dynamic result = CallValidationFunction(controllerContext, bindingContext, propertyDescriptor, controllerType, methodInfo, myAttr.AdditionalFields);

                        //验证失败，向ModelState添加错误信息
                        if (result != null && !result.Data)
                        {
                            string errorMessage = myAttr.ErrorMessage;
                            if (!string.IsNullOrEmpty(myAttr.ErrorMessageResourceName))//如果特性应用的是资源文件中的值
                            {
                                var property = myAttr.ErrorMessageResourceType.GetProperty(myAttr.ErrorMessageResourceName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
                                if (property != null)
                                {
                                    //根据特性中设置的资源文件类型，与属性名，反射获得具体的数据
                                    //注意第二个参数，一定要为true，因为资源文件的构造函数并不是public
                                    //虽然可以改为public，但是一旦修改资源文件，构造函数又会变成internal
                                    var messageObj = Activator.CreateInstance(myAttr.ErrorMessageResourceType, true);
                                    errorMessage = property.GetValue(messageObj, null).ToString();
                                }
                            }
                            //如果错误消息是{0}XX格式，将占位符替换为DisplayName中的值
                            ModelMetadata propertyMetadata = bindingContext.PropertyMetadata[propertyDescriptor.Name];
                            string displayName = propertyMetadata.GetDisplayName();
                            errorMessage = String.Format(errorMessage, displayName);
                            bindingContext.ModelState.AddModelError(propertyDescriptor.Name, errorMessage);
                        }
                    }
                }
            }
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        /// <summary>
        /// 调用验证方法
        /// </summary>
        private object CallValidationFunction(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, Type controllerType, MethodInfo methodInfo, string additionalFields)
        {
            var propValue = controllerContext.RequestContext.HttpContext.Request.Form[bindingContext.ModelName + propertyDescriptor.Name];//请求中指定属性名包含的值
            //反射获得需要调用验证方法的控制器对象
            var ctrObj = (Controller)Activator.CreateInstance(controllerType);
            //待调用的方法的参数
            var paras = methodInfo.GetParameters();




            //方法返回的结果
            object result = null;
            if (paras.Length == 0)//没有参数
            {
                result = methodInfo.Invoke(ctrObj, null);
            }
            else
            {
                var parasList = new List<object>();//参数集合
                Type type = paras[0].ParameterType;


                //ChangeType方法无法转换可空类型，需要获取实际类型进行转换
                // Nullable.GetUnderlyingType返回null表示类型不是nullable
                Type safeType = Nullable.GetUnderlyingType(type) ?? type;
                //布尔类型特殊，当选中checkbox时，会传递两个值，以半角逗号分隔，如"true,false",因此一旦有两个值，那么就是true
                if (typeof(bool) == safeType && propValue != null)
                {
                    var temp = propValue.ToString();
                    if (temp.Contains(","))
                    {
                        parasList.Add(true);
                    }
                    else
                    {
                        parasList.Add(false);
                    }
                }
                else
                {
                    object safeValue = (propValue == null) ? null : Convert.ChangeType(propValue, safeType);
                    parasList.Add(safeValue);
                }


                if (paras.Length == 1)//如果方法只有一个参数，那么直接调用
                {
                    result = methodInfo.Invoke(ctrObj, parasList.ToArray());
                }
                else//如果有多个，那么检查验证特性的 附加参数是否存在
                {
                    if (!string.IsNullOrEmpty(additionalFields))
                    {
                        string[] addFields = additionalFields.Split(',');//分割获得附加参数
                        foreach (var field in addFields)
                        {
                            //获取该属性对应的类型
                            Type t = bindingContext.ModelMetadata.Properties.Where(m => m.PropertyName == field).SingleOrDefault().ModelType;
                            //从请求上下文获得附加参数的内容
                            object value = controllerContext.RequestContext.HttpContext.Request.Form[bindingContext.ModelName + field];
                            //进行可空类型检测
                            safeType = Nullable.GetUnderlyingType(t) ?? t;
                            //布尔类型特殊，当选中checkbox时，会传递两个值，以半角逗号分隔，如"true,false",
                            //因此一旦有两个值，那么就是true
                            if (typeof(bool) == safeType && value != null)
                            {
                                var temp = value.ToString();
                                if (temp.Contains(","))
                                {
                                    parasList.Add(true);
                                }
                                else
                                {
                                    parasList.Add(false);
                                }
                            }
                            else
                            {
                                object safeValue = (value == null) ? null : Convert.ChangeType(value, safeType);
                                parasList.Add(safeValue);
                            }
                        }
                        if (parasList.Count == paras.Length)
                        {
                            result = methodInfo.Invoke(ctrObj, parasList.ToArray());
                        }
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 获取所有继承自Controller类的类型
        /// </summary>
        private IEnumerable<Type> GetControllerTypes()
        {
            return GetSubClasses<Controller>();
        }

        /// <summary>
        /// 根据区域名与控制器名获取具体的控制器
        /// </summary>
        private bool CheckController(Type t, string areaName, string controllerName)
        {
            return t.Name == controllerName + "Controller" && CheckNamespace(t.Namespace, areaName);
        }

        /// <summary>
        /// 检查控制器的命名空间 是否符合要求
        /// 如果Remote特性使用时带入了具体的Area，为了找到不同Area下同名控制器
        /// 进行判断
        /// </summary>
        private bool CheckNamespace(string nameSpace, string areaName)
        {
            if (string.IsNullOrEmpty(areaName))
            {
                return !nameSpace.Contains(".Areas.");
            }
            else
            {
                return nameSpace.Contains(".Areas." + areaName + ".Controllers");
            }
        }

        /// <summary>
        /// 获取调用该方法的方法的所在程序集的所有T类型的子类型
        /// </summary>
        private IEnumerable<Type> GetSubClasses<T>()
        {
            return Assembly.GetCallingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(T)));
        }


    }


}
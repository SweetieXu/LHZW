using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Asiatek.IBLL;
using System.Reflection;
using System.Collections.Concurrent;

namespace Asiatek.BLL
{
    public class BLLFactory
    {
        private readonly static string DBType = ConfigurationManager.AppSettings["DBType"];
        private readonly static string AssemblyName = "Asiatek.BLL";


        public static IUserBLL GetUserBLL()
        {
            string className = AssemblyName + "." + DBType + ".UserBLL";
            return Assembly.Load(AssemblyName).CreateInstance(className) as IUserBLL;
        }

        public static IFunctionBLL GetFunctionBLL()
        {
            string className = AssemblyName + "." + DBType + ".FunctionBLL";
            return Assembly.Load(AssemblyName).CreateInstance(className) as IFunctionBLL;
        }
    }



}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Asiatek.IBLL
{
    public interface IFunctionBLL
    {
        /// <summary>
        /// 根据用户编号获取用户菜单功能信息
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns>权限信息</returns>
        DataTable GetFunctionsByUserID(string userID);

        /// <summary>
        /// 获取全部主功能菜单信息
        /// </summary>
        /// <returns>主动菜单</returns>
        DataTable GetAllFunctions();
    }
}

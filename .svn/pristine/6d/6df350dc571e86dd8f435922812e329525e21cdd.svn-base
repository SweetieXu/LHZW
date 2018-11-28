using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.IBLL
{
    public interface IUserBLL
    {
        /// <summary>
        /// 登录
        /// 1：登录成功；0：用户名或密码错误；-1：发生错误
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="strucAccount">单位账号</param>
        /// <param name="userID">用户编号</param>
        /// <param name="roleID">角色编号</param>
        /// <param name="roleName">角色名称</param>
        /// <param name="roleLevel">角色等级 0：超级管理员 1：:管理员 2：普通用户</param>
        /// <returns>1：登录成功；0：用户名或密码错误；-1：发生错误</returns>
        int Login(string userName, string password, string strucAccount, out string userID, out int roleID, out string roleName, out int roleLevel);
    }
}

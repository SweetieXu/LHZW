using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asiatek.Common;
using Asiatek.DBUtility;
using System.Data.SqlClient;
using System.Data;
using Asiatek.IBLL;
using Asiatek.Model;
using Asiatek.AjaxPager;
using Asiatek.Resource;

namespace Asiatek.BLL.MSSQL
{
    /// <summary>
    /// 用户信息相关的业务
    /// </summary>
    public class UserBLL
    {
        #region 登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="strucAccount">公司账号</param>
        /// <param name="userID">用户编号</param>
        /// <param name="roleID">角色编号</param>
        /// <param name="roleName">角色名称</param>
        /// <param name="roleLevel">角色等级</param>
        /// <returns>-1：发生错误；0：查无记录；1：成功</returns>
        public int Login(string userName, string password, string strucAccount, out string userID, out int roleID, out string roleName, out int roleLevel)
        {
            userID = string.Empty;
            roleID = -1;
            roleName = string.Empty;
            roleLevel = -1;
            string sql = @"SELECT u.ID,RoleID,r.RoleName,r.RoleLevel FROM dbo.Users u
INNER JOIN dbo.Roles r ON u.RoleID=r.ID
INNER JOIN dbo.Structures s ON u.StrucCode=s.StrucCode
WHERE u.UserName=@UserName AND u.UserPassword=@UserPassword AND s.StrucAccount=@StrucAccount";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@UserName",userName),
                new SqlParameter("@UserPassword",MD5Helper.GetMD5Str(password)),
                new SqlParameter("@StrucAccount",strucAccount),
            };
            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray());
            if (dt == null)
            {
                return -1;
            }
            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            userID = dt.Rows[0][0].ToString();
            roleID = Convert.ToInt32(dt.Rows[0][1]);
            roleName = dt.Rows[0][2].ToString();
            roleLevel = Convert.ToInt32(dt.Rows[0][3]);
            return 1;
        }


        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user">用户登录数据</param>
        /// <param name="userSession">登录后返回的用户相关信息</param>
        /// <returns></returns>
        public static OperationResult Login(UserLoginModel user, out UserSessionModel userSession)
        {

            string sql = @"SELECT u.ID,RoleID,r.RoleName,r.RoleLevel,u.NickName,u.StrucID,u.VehicleViewMode FROM dbo.Users u
INNER JOIN dbo.Roles r ON u.RoleID=r.ID
INNER JOIN dbo.Structures s ON u.StrucID=s.ID
WHERE u.UserName=@UserName AND u.UserPassword=@UserPassword AND s.StrucAccount=@StrucAccount";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@UserName",SqlDbType.VarChar,20),
                new SqlParameter("@UserPassword",SqlDbType.Char,32),
                new SqlParameter("@StrucAccount",SqlDbType.VarChar,50),
            };
            paras[0].Value = user.UserName.Trim();
            paras[1].Value = MD5Helper.GetMD5Str(user.Password.Trim());
            paras[2].Value = user.StrucAccount.Trim();


            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray());
            OperationResult result = new OperationResult();
            userSession = null;
            if (dt == null)
            {
                result.Message = Asiatek.Resource.PromptInformation.LoginDBError;
                return result;
            }
            if (dt.Rows.Count == 0)
            {
                result.Message = Asiatek.Resource.PromptInformation.LoginError;
                return result;
            }

            //获取到用户信息，组装session内容
            int userID = Convert.ToInt32(dt.Rows[0][0].ToString());
            int roleID = Convert.ToInt32(dt.Rows[0][1]);
            string roleName = dt.Rows[0][2].ToString();
            int roleLevel = Convert.ToInt32(dt.Rows[0][3]);
            userSession = new UserSessionModel();
            userSession.UserName = user.UserName.Trim();
            userSession.NickName = dt.Rows[0][4].ToString();
            userSession.StrucID = Convert.ToInt32(dt.Rows[0][5]);
            userSession.UserId = Convert.ToInt32(userID);
            userSession.RoleInfo = new RoleInfoModel() { RoleID = roleID, RoleLevel = (RoleLevelEnum)roleLevel, RoleName = roleName };
            userSession.VehicleViewMode = string.IsNullOrEmpty(dt.Rows[0][6].ToString()) ? true : Convert.ToBoolean(dt.Rows[0][6]);


            //获取用户功能信息
            List<FunctionsInfoModel> funcList = null;
            if (userSession.RoleInfo.RoleLevel == RoleLevelEnum.SuperAdmin)
            {
                funcList = FunctionBLL.GetAllFunctions();
            }
            else
            {
                funcList = FunctionBLL.GetFunctionsByUserID(userID);
            }
            if (funcList == null)
            {
                result.Message = Asiatek.Resource.PromptInformation.GetFunctionsError;
                return result;
            }
            if (funcList.Count == 0)
            {
                result.Message = Asiatek.Resource.PromptInformation.NoFunctions;
                return result;
            }

            userSession.Functions = funcList;
            result.Success = true;
            return result;
        }
        #endregion

        #region 查询
        //public static AsiatekPagedList<UserListModel> GetPagedUserInfo(UserSearchModel model, int searchPage, int pageSize, bool isSuperAdmin)
        //{
        //    List<SqlParameter> paras = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@tableName","Users u"),
        //        new SqlParameter("@joinStr","INNER JOIN Structures s ON u.StrucID=s.ID"),
        //        new SqlParameter("@pageSize",pageSize),
        //        new SqlParameter("@currentPage",searchPage),
        //        new SqlParameter("@orderBy","u.ID"),
        //        new SqlParameter("@showColumns","u.ID,u.UserName,u.NickName,s.StrucName AS SubordinateStrucName"),
        //    };

        //    string conditionStr = "u.Status<>9";
        //    if (!isSuperAdmin)//不是超级管理员只查询角色等级大于等于3的用户
        //    {
        //        conditionStr += " AND u.RoleID<>1 AND u.RoleID<>2";
        //    }

        //    bool b1 = !string.IsNullOrWhiteSpace(model.UserName);
        //    bool b2 = !string.IsNullOrWhiteSpace(model.NickName);
        //    if (b1 && b2)//条件同时包含用户名和昵称
        //    {
        //        conditionStr += " AND (u.UserName LIKE '%" + model.UserName + "%' OR u.NickName LIKE '%" + model.NickName + "%')";
        //    }
        //    else if (b1)//只有用户名
        //    {
        //        conditionStr += " AND u.UserName LIKE '%" + model.UserName + "%'";
        //    }
        //    else if (b2)//只有昵称
        //    {
        //        conditionStr += " AND u.NickName LIKE '%" + model.NickName + "%'";
        //    }

        //    if (model.StrucID != -1)//如果选择了具体的隶属单位
        //    {
        //        conditionStr += " AND u.StrucID=" + model.StrucID + "";
        //    }

        //    paras.Add(new SqlParameter("@conditionStr", conditionStr));

        //    paras.Add(new SqlParameter()
        //    {
        //        ParameterName = "@totalItemCount",
        //        Direction = ParameterDirection.Output,
        //        SqlDbType = SqlDbType.Int
        //    });
        //    paras.Add(new SqlParameter()
        //    {
        //        ParameterName = "@newCurrentPage",
        //        Direction = ParameterDirection.Output,
        //        SqlDbType = SqlDbType.Int
        //    });
        //    List<UserListModel> list = ConvertToList<UserListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
        //    int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
        //    int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
        //    return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        //}


        public static List<UserDDLModel> GetUsers(bool isSuperAdmin)
        {
            string sql = @"SELECT u.ID,UserName FROM dbo.Users u INNER JOIN dbo.Roles r ON u.RoleID=r.ID WHERE [Status]<>9";
            if (!isSuperAdmin)//非超级管理员只能查询普通用户级别
            {
                sql += " AND r.RoleLevel>=2";
            }
            return ConvertToList<UserDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, null));
        }
        #endregion

        #region 查询新版
        public static AsiatekPagedList<UserListModel> GetPagedUserInfo(UserSearchModel model, int searchPage, int pageSize, bool isSuperAdmin)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","Users u"),
                new SqlParameter("@joinStr","INNER JOIN Structures s ON u.StrucID=s.ID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","u.ID"),
                new SqlParameter("@showColumns","u.ID,u.UserName,u.NickName,s.StrucName AS SubordinateStrucName,u.VehicleViewMode"),
            };

            string conditionStr = "u.Status<>9";
            if (!isSuperAdmin)//不是超级管理员只查询角色等级大于等于3的用户
            {
                conditionStr += " AND u.RoleID<>1 AND u.RoleID<>2";
            }

            bool b1 = !string.IsNullOrWhiteSpace(model.UserName);
            bool b2 = !string.IsNullOrWhiteSpace(model.NickName);
            if (b1 && b2)//条件同时包含用户名和昵称
            {
                conditionStr += " AND (u.UserName LIKE '%" + model.UserName + "%' OR u.NickName LIKE '%" + model.NickName + "%')";
            }
            else if (b1)//只有用户名
            {
                conditionStr += " AND u.UserName LIKE '%" + model.UserName + "%'";
            }
            else if (b2)//只有昵称
            {
                conditionStr += " AND u.NickName LIKE '%" + model.NickName + "%'";
            }

            if (model.StrucID != -1)//如果选择了具体的隶属单位
            {
                conditionStr += " AND u.StrucID=" + model.StrucID + "";
            }
            if (model.VehicleViewMode != -1)
            {
                conditionStr += " AND u.VehicleViewMode=" + model.VehicleViewMode + "";
            }

            paras.Add(new SqlParameter("@conditionStr", conditionStr));

            paras.Add(new SqlParameter()
            {
                ParameterName = "@totalItemCount",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Int
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@newCurrentPage",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Int
            });
            List<UserListModel> list = ConvertToList<UserListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 根据用户ID批量删除（逻辑删除）
        /// 将Status设置为9表示删除
        /// </summary>
        public static OperationResult DeleteUser(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.Users SET [Status]=9 WHERE ID=@ID";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i];
                paras[i][0] = temp;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion

        #region 删除
        /// <summary>
        /// 根据用户ID批量删除（物理删除）
        /// </summary>
        public static OperationResult DeleteUserPhysical(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.Users WHERE ID=@ID";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i];
                paras[i][0] = temp;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion

        #region 新增
        /// <summary>
        /// 检查用户名是否重复
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool CheckUserNameExists(string userName)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@userName",SqlDbType.NVarChar,20),
            };
            paras[0].Value = userName.Trim();
            string sql = "SELECT COUNT(0) FROM Users WHERE userName=@userName";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        //public static OperationResult Adduser(UserAddModel model,int CreateUserID)
        //{


        //    List<SqlParameter> paras = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@UserName",SqlDbType.NVarChar,20),
        //        new SqlParameter("@RoleID",SqlDbType.Int),
        //        new SqlParameter("@NickName",SqlDbType.NVarChar,20),
        //        new SqlParameter("@StrucID",SqlDbType.Int),

        //        new SqlParameter("@ContactNumber1",SqlDbType.NVarChar,50),
        //        new SqlParameter("@ContactNumber2",SqlDbType.NVarChar,50),
        //        new SqlParameter("@ContactAddress",SqlDbType.NVarChar,50),
        //        new SqlParameter("@Remark",SqlDbType.NVarChar,50),
        //        new SqlParameter("@CreateUserID",SqlDbType.Int)
        //    };

        //    paras[0].Value = model.UserName;
        //    paras[1].Value = model.RoleID;
        //    paras[2].Value = model.NickName;
        //    paras[3].Value = model.StrucID;

        //    #region 可NULL

        //    if (string.IsNullOrWhiteSpace(model.ContactNumber1))
        //    {
        //        paras[4].Value = DBNull.Value;
        //    }
        //    else
        //    {
        //        paras[4].Value = model.ContactNumber1;
        //    }

        //    if (string.IsNullOrWhiteSpace(model.ContactNumber2))
        //    {
        //        paras[5].Value = DBNull.Value;
        //    }
        //    else
        //    {
        //        paras[5].Value = model.ContactNumber2;
        //    }

        //    if (string.IsNullOrWhiteSpace(model.ContactAddress))
        //    {
        //        paras[6].Value = DBNull.Value;
        //    }
        //    else
        //    {
        //        paras[6].Value = model.ContactAddress;
        //    }

        //    if (string.IsNullOrWhiteSpace(model.Remark))
        //    {
        //        paras[7].Value = DBNull.Value;
        //    }
        //    else
        //    {
        //        paras[7].Value = model.Remark;
        //    }
        //    paras[8].Value = CreateUserID;
        //    #endregion



        //    //新增用户密码默认为8个8
        //    string defaultPwd = MD5Helper.GetMD5Str("88888888");
        //    paras.Add(new SqlParameter("@Password", SqlDbType.Char, 32));
        //    paras[paras.Count - 1].Value = defaultPwd;


        //    #region  SQL
        //    string sql = "Proc_AddUser";
        //    //            string sql = @"INSERT INTO dbo.Users
        //    //        ( UserName ,
        //    //          UserPassword ,
        //    //          RoleID ,
        //    //          NickName ,
        //    //          StrucID ,
        //    //          ContactNumber1 ,
        //    //          ContactNumber2 ,
        //    //          ContactAddress ,
        //    //          Remark ,
        //    //          Status ,
        //    //          CreateDateTime ,
        //    //          EditDateTime
        //    //        )
        //    //VALUES  ( @UserName , -- UserName - nvarchar(20)
        //    //          @Pwd, -- UserPassword - char(32)
        //    //          @RoleID , -- RoleID - int
        //    //          @NickName , -- NickName - nvarchar(20)
        //    //          @StrucID , -- StrucID - int
        //    //          @ContactNumber1, -- ContactNumber1 - nvarchar(50)
        //    //          @ContactNumber2 , -- ContactNumber2 - nvarchar(50)
        //    //          @ContactAddress , -- ContactAddress - nvarchar(50)
        //    //          @Remark , -- Remark - nvarchar(50)
        //    //          0 , -- Status - tinyint
        //    //          GETDATE(), -- CreateDateTime - datetime
        //    //          GETDATE()  -- EditDateTime - datetime
        //    //        );";
        //    #endregion


        //    bool result = MSSQLHelper.ExecuteNonQuery(CommandType.StoredProcedure, sql, paras.ToArray()) > 1;
        //    return new OperationResult()
        //    {
        //        Success = result,
        //        Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
        //    };
        //}
        #endregion

        #region 编辑
        //        public static SelectResult<UserEditModel> GetUserByID(int id)
        //        {
        //            List<SqlParameter> paras = new List<SqlParameter>()
        //            {
        //                new SqlParameter("@ID",SqlDbType.Int),
        //            };
        //            paras[0].Value = id;
        //            string sql = @"SELECT   a.ID ,
        //                UserName ,
        //                RoleID ,
        //                NickName ,
        //                StrucID ,
        //                ContactNumber1 ,
        //                ContactNumber2 ,
        //                ContactAddress ,
        //                a.Remark,
        //                b.StrucName
        //       FROM     dbo.Users AS a
        //       LEFT JOIN Structures AS b ON a.StrucID = b.ID
        //       WHERE    a.ID = @ID AND a.[Status] <> 9";
        //            List<UserEditModel> list = ConvertToList<UserEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

        //            UserEditModel data = null;
        //            string msg = string.Empty;

        //            if (list == null)
        //            {
        //                msg = PromptInformation.DBError;
        //            }
        //            else if (list.Count == 0)
        //            {
        //                msg = PromptInformation.NotExists;
        //            }
        //            else
        //            {
        //                data = list[0];
        //            }
        //            return new SelectResult<UserEditModel>()
        //            {
        //                DataResult = data,
        //                Message = msg
        //            };
        //        }

        //        public static OperationResult EditUser(UserEditModel model,int EditUserID)
        //        {
        //            List<SqlParameter> paras = new List<SqlParameter>()
        //            {
        //                new SqlParameter("@ID",SqlDbType.Int),
        //                new SqlParameter("@RoleID",SqlDbType.Int),
        //                new SqlParameter("@NickName",SqlDbType.NVarChar,20),
        //                new SqlParameter("@StrucID",SqlDbType.Int),

        //                new SqlParameter("@ContactNumber1",SqlDbType.NVarChar,50),
        //                new SqlParameter("@ContactNumber2",SqlDbType.NVarChar,50),
        //                new SqlParameter("@ContactAddress",SqlDbType.NVarChar,50),
        //                new SqlParameter("@Remark",SqlDbType.NVarChar,50),
        //                new SqlParameter("@EditUserID",SqlDbType.Int)
        //            };

        //            paras[0].Value = model.ID;
        //            paras[1].Value = model.RoleID;
        //            paras[2].Value = model.NickName;
        //            paras[3].Value = model.StrucID;

        //            #region 可NULL

        //            if (string.IsNullOrWhiteSpace(model.ContactNumber1))
        //            {
        //                paras[4].Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                paras[4].Value = model.ContactNumber1;
        //            }

        //            if (string.IsNullOrWhiteSpace(model.ContactNumber2))
        //            {
        //                paras[5].Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                paras[5].Value = model.ContactNumber2;
        //            }

        //            if (string.IsNullOrWhiteSpace(model.ContactAddress))
        //            {
        //                paras[6].Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                paras[6].Value = model.ContactAddress;
        //            }

        //            if (string.IsNullOrWhiteSpace(model.Remark))
        //            {
        //                paras[7].Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                paras[7].Value = model.Remark;
        //            }
        //            paras[8].Value = EditUserID;
        //            #endregion


        //            #region  SQL
        //            string sql = @"UPDATE  dbo.Users
        //SET     NickName = @NickName ,
        //        RoleID = @RoleID ,
        //        StrucID = @StrucID ,
        //        Remark = @Remark,
        //        ContactAddress = @ContactAddress ,
        //        ContactNumber1 = @ContactNumber1 ,
        //        ContactNumber2 = @ContactNumber2 ,
        //        EditDateTime = GETDATE(),
        //        EditUserID=@EditUserID
        //WHERE   ID = @ID";
        //            #endregion

        //            int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
        //            string msg = string.Empty;
        //            switch (result)
        //            {
        //                case 1:
        //                    msg = PromptInformation.OperationSuccess;
        //                    break;
        //                case 0:
        //                    msg = PromptInformation.NotExists;
        //                    break;
        //                case -1:
        //                    msg = PromptInformation.DBError;
        //                    break;
        //            }
        //            return new OperationResult()
        //            {
        //                Success = result > 0,
        //                Message = msg
        //            };
        //        }


        /// <summary>
        /// 检查用户名是否重复
        /// </summary>
        public static bool CheckUserNameExists(string userName, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@userName",SqlDbType.NVarChar,20),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = userName.Trim();
            paras[1].Value = id;
            string sql = "SELECT COUNT(0) FROM Users WHERE userName=@userName AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        #endregion

        #region 新增编辑用户新版
        #region 新增
        public static OperationResult Adduser(UserAddModel model, int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@UserName",SqlDbType.NVarChar,20),
                new SqlParameter("@RoleID",SqlDbType.Int),
                new SqlParameter("@NickName",SqlDbType.NVarChar,20),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@ContactNumber1",SqlDbType.NVarChar,50),
                new SqlParameter("@ContactNumber2",SqlDbType.NVarChar,50),
                new SqlParameter("@ContactAddress",SqlDbType.NVarChar,50),
                new SqlParameter("@Remark",SqlDbType.NVarChar,50),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                new SqlParameter("@VehicleViewMode",SqlDbType.Bit),
            };

            paras[0].Value = model.UserName.Trim();
            paras[1].Value = model.RoleID;
            paras[2].Value = model.NickName.Trim();
            paras[3].Value = model.StrucID;

            #region 可NULL
            if (string.IsNullOrWhiteSpace(model.ContactNumber1))
            {
                paras[4].Value = DBNull.Value;
            }
            else
            {
                paras[4].Value = model.ContactNumber1.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.ContactNumber2))
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.ContactNumber2.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.ContactAddress))
            {
                paras[6].Value = DBNull.Value;
            }
            else
            {
                paras[6].Value = model.ContactAddress.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[7].Value = DBNull.Value;
            }
            else
            {
                paras[7].Value = model.Remark;
            }
            paras[8].Value = CreateUserID;
            #endregion

            paras[9].Value = model.VehicleViewMode;

            //新增用户密码默认为8个8
            string defaultPwd = MD5Helper.GetMD5Str("88888888");
            paras.Add(new SqlParameter("@Password", SqlDbType.Char, 32));
            paras[paras.Count - 1].Value = defaultPwd;

            #region  SQL
            string sql = @"INSERT INTO dbo.Users
			 ( UserName ,
			  UserPassword ,
			  RoleID ,
			  NickName ,
			  StrucID ,
			  ContactNumber1 ,
			  ContactNumber2 ,
			  ContactAddress ,
			  Remark ,
			  Status ,
			  CreateDateTime ,
			  CreateUserID,
              VehicleViewMode
			)
	VALUES  ( @UserName , -- UserName - nvarchar(20)
			  @Password , -- UserPassword - char(32)
			  @RoleID , -- RoleID - int
			  @NickName , -- NickName - nvarchar(20)
			  @StrucID , -- StrucID - int
			  @ContactNumber1 , -- ContactNumber1 - nvarchar(50)
			  @ContactNumber2 , -- ContactNumber2 - nvarchar(50)
			  @ContactAddress , -- ContactAddress - nvarchar(50)
			  @Remark , -- Remark - nvarchar(50)
			  0 , -- Status - tinyint
			  GETDATE() , -- CreateDateTime - datetime
			  @CreateUserID,
             @VehicleViewMode
			) ;";

            #endregion

            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 编辑
        public static SelectResult<UserEditModel> GetUserByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT   a.ID ,
                UserName ,
                RoleID ,
                NickName ,
                StrucID ,
                ContactNumber1 ,
                ContactNumber2 ,
                ContactAddress ,
                a.Remark,
                b.StrucName,
                a.VehicleViewMode
       FROM     dbo.Users AS a
       LEFT JOIN Structures AS b ON a.StrucID = b.ID
       WHERE    a.ID = @ID AND a.[Status] <> 9";
            List<UserEditModel> list = ConvertToList<UserEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            UserEditModel data = null;
            string msg = string.Empty;

            if (list == null)
            {
                msg = PromptInformation.DBError;
            }
            else if (list.Count == 0)
            {
                msg = PromptInformation.NotExists;
            }
            else
            {
                data = list[0];
            }
            return new SelectResult<UserEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditUser(UserEditModel model, int EditUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@RoleID",SqlDbType.Int),
                new SqlParameter("@NickName",SqlDbType.NVarChar,20),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@ContactNumber1",SqlDbType.NVarChar,50),
                new SqlParameter("@ContactNumber2",SqlDbType.NVarChar,50),
                new SqlParameter("@ContactAddress",SqlDbType.NVarChar,50),
                new SqlParameter("@Remark",SqlDbType.NVarChar,50),
                new SqlParameter("@EditUserID",SqlDbType.Int),
                new SqlParameter("@VehicleViewMode",SqlDbType.Bit),
            };

            paras[0].Value = model.ID;
            paras[1].Value = model.RoleID;
            paras[2].Value = model.NickName.Trim();
            paras[3].Value = model.StrucID;

            #region 可NULL

            if (string.IsNullOrWhiteSpace(model.ContactNumber1))
            {
                paras[4].Value = DBNull.Value;
            }
            else
            {
                paras[4].Value = model.ContactNumber1.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.ContactNumber2))
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.ContactNumber2.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.ContactAddress))
            {
                paras[6].Value = DBNull.Value;
            }
            else
            {
                paras[6].Value = model.ContactAddress.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[7].Value = DBNull.Value;
            }
            else
            {
                paras[7].Value = model.Remark;
            }
            paras[8].Value = EditUserID;
            paras[9].Value = model.VehicleViewMode;
            #endregion


            #region  SQL
            string sql = @"UPDATE  dbo.Users SET     NickName = @NickName ,
        RoleID = @RoleID ,
        StrucID = @StrucID ,
        Remark = @Remark,
        ContactAddress = @ContactAddress ,
        ContactNumber1 = @ContactNumber1 ,
        ContactNumber2 = @ContactNumber2 ,
        EditDateTime = GETDATE(),
        EditUserID=@EditUserID,
        VehicleViewMode =@VehicleViewMode
WHERE   ID = @ID";
            #endregion

            int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
            string msg = string.Empty;
            switch (result)
            {
                case 1:
                    msg = PromptInformation.OperationSuccess;
                    break;
                case 0:
                    msg = PromptInformation.NotExists;
                    break;
                case -1:
                    msg = PromptInformation.DBError;
                    break;
            }
            return new OperationResult()
            {
                Success = result > 0,
                Message = msg
            };
        }
        #endregion
        #endregion

        #region 重置密码
        /// <summary>
        /// 将用户密码重置为88888888   8个8
        /// </summary>
        public static OperationResult ResetUserPwd(string[] ids)
        {
            string defaultPwd = MD5Helper.GetMD5Str("88888888");

            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.Users SET UserPassword='" + defaultPwd + "' WHERE ID=@ID";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i];
                paras[i][0] = temp;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 切换用户
        /// <summary>
        /// 获取待切换用户信息
        /// </summary>
        public static OperationResult GetUserForSwitch(int id, out UserSessionModel userSession)
        {

            string sql = @"SELECT u.ID,RoleID,r.RoleName,r.RoleLevel,u.UserName,u.NickName,u.StrucID,u.VehicleViewMode FROM dbo.Users u
INNER JOIN dbo.Roles r ON u.RoleID=r.ID
INNER JOIN dbo.Structures s ON u.StrucID=s.ID
WHERE u.ID=@ID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;


            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray());
            OperationResult result = new OperationResult();
            userSession = null;
            if (dt == null)
            {
                result.Message = PromptInformation.DBError;
                return result;
            }
            if (dt.Rows.Count == 0)
            {
                result.Message = PromptInformation.NotExists;
                return result;
            }

            //获取到用户信息，组装session内容
            int userID = Convert.ToInt32(dt.Rows[0][0].ToString());
            int roleID = Convert.ToInt32(dt.Rows[0][1]);
            string roleName = dt.Rows[0][2].ToString();
            int roleLevel = Convert.ToInt32(dt.Rows[0][3]);
            userSession = new UserSessionModel();
            userSession.UserName = dt.Rows[0][4].ToString();
            userSession.NickName = dt.Rows[0][5].ToString();
            userSession.StrucID = Convert.ToInt32(dt.Rows[0][6]);
            userSession.VehicleViewMode = string.IsNullOrEmpty(dt.Rows[0][7].ToString()) ? true : Convert.ToBoolean(dt.Rows[0][7]);
            userSession.UserId = Convert.ToInt32(userID);
            userSession.RoleInfo = new RoleInfoModel() { RoleID = roleID, RoleLevel = (RoleLevelEnum)roleLevel, RoleName = roleName };


            //获取用户功能信息
            List<FunctionsInfoModel> funcList = null;
            if (userSession.RoleInfo.RoleLevel == RoleLevelEnum.SuperAdmin)
            {
                funcList = FunctionBLL.GetAllFunctions();
            }
            else
            {
                funcList = FunctionBLL.GetFunctionsByUserID(userID);
            }
            if (funcList == null)
            {
                result.Message = Asiatek.Resource.PromptInformation.GetFunctionsError;
                return result;
            }
            if (funcList.Count == 0)
            {
                result.Message = Asiatek.Resource.PromptInformation.NoFunctions;
                return result;
            }

            userSession.Functions = funcList;
            result.Success = true;
            return result;
        }
        #endregion


        #region 修改密码
        /// <summary>
        /// 检查当前用户的原密码是否正确
        /// </summary>
        /// <param name="originalPwd">原密码</param>
        /// <param name="userID">用户ID</param>
        /// <returns>true：正确；false：错误</returns>
        public static bool CheckOriginalPassword(string originalPwd, int userID)
        {
            string md5Pwd = MD5Helper.GetMD5Str(originalPwd.Trim());
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@UserPassword",SqlDbType.Char,32),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = md5Pwd;
            paras[1].Value = userID;
            string sql = "SELECT COUNT(0) FROM dbo.Users WHERE UserPassword=@UserPassword AND ID=@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)
            {
                return false;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public static OperationResult ModifyPassword(UserModifyPasswordModel model)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@originalPwd",SqlDbType.Char,32),
                new SqlParameter("@newPwd",SqlDbType.Char,32),
            };

            paras[0].Value = model.ID;
            paras[1].Value = MD5Helper.GetMD5Str(model.OriginalPassword);
            paras[2].Value = MD5Helper.GetMD5Str(model.NewPassword);

            #region  SQL
            string sql = @"UPDATE dbo.Users SET UserPassword=@newPwd WHERE ID=@ID AND UserPassword=@originalPwd";
            #endregion

            int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
            string msg = string.Empty;
            switch (result)
            {
                case 1:
                    msg = PromptInformation.OperationSuccess;
                    break;
                case 0:
                    msg = PromptInformation.NotExists;
                    break;
                case -1:
                    msg = PromptInformation.DBError;
                    break;
            }
            return new OperationResult()
            {
                Success = result > 0,
                Message = msg
            };
        }

        #endregion

    }
}

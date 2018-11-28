using Asiatek.AjaxPager;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Asiatek.BLL.MSSQL
{
    public class RoleBLL
    {
        #region 查询

        public static AsiatekPagedList<RoleListModel> GetPagedRoleInfo(RoleSettingModel model, int pageSize, bool isSuperAdmin)
        {
            return GetPagedRoleInfo(model.SearchPage, pageSize, isSuperAdmin, model.RoleName);
        }

        /// <summary>
        /// 获取角色分页数据
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="roleName">角色编号</param>
        /// <returns></returns>
        public static AsiatekPagedList<RoleListModel> GetPagedRoleInfo(int currentPage, int pageSize, bool isSuperAdmin, string roleName = "")
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","Roles"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",currentPage),
                new SqlParameter("@orderBy","ID"),
                
                
              
            };

            if (isSuperAdmin)//超级管理员查询全部角色信息
            {
                paras.Add(new SqlParameter("@conditionStr", "RoleName LIKE '%" + roleName + "%'"));
            }
            else//其他则只查询普通用户
            {
                paras.Add(new SqlParameter("@conditionStr", "RoleName LIKE '%" + roleName + "%' AND RoleLevel='2'"));
            }


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



            List<RoleListModel> list = ConvertToList<RoleListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));

            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }


        /// <summary>
        /// 根据当前登录用户的角色等级获取角色下拉信息
        /// 超级管理员获取全部、其他获取低于当前等级的角色信息
        /// </summary>
        /// <param name="roleLevel"></param>
        /// <returns></returns>
        public static List<RoleDDLModel> GetRolesByCurrentUserRoleLevel(int roleLevel)
        {
            string sql = "SELECT ID,RoleName FROM dbo.Roles";
            if (roleLevel != 0)//超级管理员的角色等级是0,
            {
                sql += " WHERE RoleLevel>@roleLevel";
            }
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@roleLevel", SqlDbType.Int);
            paras[0].Value = roleLevel;
            return ConvertToList<RoleDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }

        #endregion


        #region 删除

        /// <summary>
        /// 删除角色
        /// 1.对应角色的用户设置为普通用户
        /// 2.删除角色与功能关联信息
        /// 3.最后删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult DeleteRoles(int[] ids)
        {

            string idsStr = string.Empty;
            foreach (var item in ids)
            {
                idsStr += "'" + item + "',";
            }
            idsStr = idsStr.TrimEnd(',');



            string[] sqls = new string[3];


            //更新要删除的角色关联的用户的角色为  系统默认的普通用户
            sqls[0] = @"UPDATE  dbo.Users
        SET     RoleID = ( SELECT   ID
                           FROM     dbo.Roles
                           WHERE    IsDefault = 1
                                    AND RoleLevel = 2
                         )
        WHERE   RoleID IN(" + idsStr + ")";

            //删除角色与功能关联信息
            sqls[1] = "DELETE FROM dbo.RolesFunctions WHERE RoleID IN (" + idsStr + ")";

            //删除角色信息
            sqls[2] = "DELETE FROM Roles WHERE ID IN (" + idsStr + ")";

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, null);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion


        #region 修改
        /// <summary>
        /// 检查非当前角色是否同名
        /// <para>True：存在；False：不存在</para>
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="ID">当前角色编号</param>
        /// <returns></returns>
        public static bool CheckRoleNameExists(string roleName, int ID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@roleName",SqlDbType.NVarChar,200),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = roleName.Trim();
            paras[1].Value = ID;

            string sql = "SELECT COUNT(0) FROM Roles WHERE roleName=@roleName AND ID!=@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 修改角色信息
        /// 修改角色名称、角色描述
        /// 同时先删除所有角色与功能关联信息，再重新添加角色与功能关联信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult ModifyRoleInfo(RoleEditModel model,int EditUserID)
        {

            int count = 2 + model.FunctionIDs.Count;
            string[] sqls = new string[count];

            #region 更新角色
            sqls[0] = @"UPDATE dbo.Roles SET RoleName=@RoleName,[Description]=@Description,EditUserID=@EditUserID,EditTime=@EditTime WHERE ID=@ID";

            SqlParameter[][] paras = new SqlParameter[count][];
            paras[0] = new SqlParameter[5];
            paras[0][0] = new SqlParameter()
            {
                ParameterName = "@RoleName",
                SqlDbType = SqlDbType.NVarChar,
                Size = 200,
                Value = model.RoleName.Trim()
            };
            paras[0][1] = new SqlParameter("@Description", SqlDbType.NVarChar, 500);


            if (string.IsNullOrWhiteSpace(model.Description))
            {
                paras[0][1].Value = DBNull.Value;
            }
            else
            {
                paras[0][1].Value = model.Description;
            }

            paras[0][2] = new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = model.ID
            };
            paras[0][3] = new SqlParameter("@EditUserID", SqlDbType.Int);
            paras[0][4] = new SqlParameter("@EditTime", SqlDbType.DateTime);
            paras[0][3].Value = EditUserID;
            paras[0][4].Value = DateTime.Now;
            #endregion


            #region 删除角色功能关联信息
            sqls[1] = @"DELETE FROM dbo.RolesFunctions WHERE RoleID=@RoleID";
            paras[1] = new SqlParameter[1];
            paras[1][0] = new SqlParameter()
            {
                ParameterName = "@RoleID",
                SqlDbType = SqlDbType.Int,
                Value = model.ID
            };
            #endregion







            for (int i = 0; i < model.FunctionIDs.Count; i++)
            {
                int index = i + 2;
                sqls[index] = @"INSERT INTO dbo.RolesFunctions( RoleID, FunctionID ) VALUES  ( @RoleID, @FunctionID )";
                paras[index] = new SqlParameter[2];
                paras[index][0] = new SqlParameter()
                {
                    ParameterName = "@RoleID",
                    SqlDbType = SqlDbType.Int,
                    Value = model.ID
                };
                paras[index][1] = new SqlParameter()
                {
                    ParameterName = "@FunctionID",
                    SqlDbType = SqlDbType.Int,
                    Value = model.FunctionIDs[i]
                };
            }



            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        /// <summary>
        /// 根据角色编号获取角色信息（用于编辑）
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static SelectResult<RoleEditModel> GetRoleByID(int roleID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = roleID;
            string sql = "SELECT ID,RoleName,[Description] FROM Roles WHERE ID=@ID";
            List<RoleEditModel> list = ConvertToList<RoleEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            RoleEditModel data = null;
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
                //如果存在角色，查询出角色的功能信息
                var roleObj = list[0];
                sql = @"SELECT FunctionID FROM dbo.RolesFunctions WHERE RoleID=@RoleID";
                paras = new List<SqlParameter>()
                {
                    new SqlParameter("@RoleID",SqlDbType.Int),
                };
                paras[0].Value = roleObj.ID;
                DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray());
                if (dt == null)
                {
                    msg = PromptInformation.DBError;
                }
                else
                {
                    var rows = dt.Rows;
                    roleObj.FunctionIDs = new List<int>();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        roleObj.FunctionIDs.Add(Convert.ToInt32(rows[i][0]));
                    }
                    data = roleObj;
                }

            }
            return new SelectResult<RoleEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }
        #endregion


        #region 新增
        /// <summary>
        /// 新增角色信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult AddRoleInfo(RoleAddModel model,int CreateUserID)
        {

            int count = 1 + model.FunctionIDs.Count;
            string[] sqls = new string[count];


            #region 新增角色信息到角色表
            sqls[0] = @"INSERT  INTO dbo.Roles
        ( RoleName ,
          [Description],CreateUserID
        )VALUES  ( @RoleName ,@Description ,@CreateUserID);SELECT SCOPE_IDENTITY()";

            SqlParameter[][] paras = new SqlParameter[count][];
            paras[0] = new SqlParameter[3];
            paras[0][0] = new SqlParameter()
            {
                ParameterName = "@RoleName",
                SqlDbType = SqlDbType.NVarChar,
                Size = 200,
                Value = model.RoleName.Trim()
            };

            paras[0][1] = new SqlParameter("@Description", SqlDbType.NVarChar, 500);
            paras[0][2] = new SqlParameter("@CreateUserID", SqlDbType.Int);

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                paras[0][1].Value = DBNull.Value;
            }
            else
            {
                paras[0][1].Value = model.Description;
            }
            paras[0][2].Value = CreateUserID;
            #endregion



            #region 新增角色与功能关联信息
            for (int i = 0; i < model.FunctionIDs.Count; i++)
            {
                int index = i + 1;
                sqls[index] = @"INSERT INTO dbo.RolesFunctions( RoleID, FunctionID ) VALUES  ( @RoleID, @FunctionID )";
                paras[index] = new SqlParameter[2];
                paras[index][0] = new SqlParameter { ParameterName = "@RoleID", SqlDbType = SqlDbType.Int };


                paras[index][1] = new SqlParameter()
                {
                    ParameterName = "@FunctionID",
                    SqlDbType = SqlDbType.Int,
                    Value = model.FunctionIDs[i]
                };
            }
            #endregion




            bool result = MSSQLHelper.ExecuteIdentityIncludeTransaction(CommandType.Text, sqls, paras) != 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        /// <summary>
        /// 检查角色名是否存在
        /// <para>True：存在；False：不存在</para>
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool CheckRoleNameExists(string roleName)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@roleName",SqlDbType.NVarChar,200),
            };
            paras[0].Value = roleName.Trim();
            string sql = "SELECT COUNT(0) FROM Roles WHERE roleName=@roleName";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion


    }
}

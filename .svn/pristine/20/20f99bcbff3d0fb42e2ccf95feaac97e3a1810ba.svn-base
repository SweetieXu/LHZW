using Asiatek.DBUtility;
using Asiatek.IBLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Asiatek.Model;
using Asiatek.Resource;
using Asiatek.AjaxPager;


namespace Asiatek.BLL.MSSQL
{
    /// <summary>
    /// 功能菜单信息相关的业务
    /// </summary>
    public class FunctionBLL
    {

        #region 查询

        /// <summary>
        /// 获取全部功能信息
        /// </summary>
        /// <returns></returns>
        public static List<FunctionsInfoModel> GetAllFunctions()
        {
            string sql = @"SELECT
                f.ID,
                FunctionName ,
                AreaName ,
                ControllerName ,
                ActionName,
                f.ParentID,
                f.OrderIndex
         FROM    dbo.Functions f 
         LEFT JOIN dbo.Actions act ON f.ActionID=act.ID
         LEFT JOIN dbo.Controllers c ON act.ControllerID=c.ID
         LEFT JOIN dbo.Areas a ON c.AreaID=a.ID";
            return ConvertToList<FunctionsInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        /// <summary>
        /// 根据用户编号获取包含功能信息
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns></returns>
        public static List<FunctionsInfoModel> GetFunctionsByUserID(int userID)
        {
            string sql = @"SELECT  f.ID ,
        FunctionName ,
        AreaName ,
        ControllerName ,
        ActionName ,
        f.ParentID,
        f.OrderIndex
FROM    dbo.Functions f
        LEFT JOIN dbo.Actions act ON f.ActionID = act.ID
        LEFT JOIN dbo.Controllers c ON act.ControllerID = c.ID
        LEFT JOIN dbo.Areas a ON c.AreaID = a.ID
        LEFT JOIN dbo.RolesFunctions rf ON f.ID = rf.FunctionID
        LEFT JOIN dbo.Roles r ON r.ID = rf.RoleID
        LEFT JOIN dbo.Users u ON u.RoleID = r.ID
         WHERE  u.ID = @UserID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@UserID",SqlDbType.Char,36),
            };
            paras[0].Value = userID;
            return ConvertToList<FunctionsInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }


        /// <summary>
        /// 获取功能分页信息
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="functionName">功能名称</param>
        /// <param name="controllerID">功能对应的动作所属控制器编号</param>
        /// <param name="areaID">功能对应的动作所属控制器所属区域编号</param>
        /// <param name="parentFunctionID">上级功能编号</param>
        /// <returns></returns>
        public static AsiatekPagedList<FunctionListModel> GetPagedFunctions(FunctionSettingModel model, int pageSize)
        {

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","VW_GetFunctionsWithLevelID f"),
                new SqlParameter("@joinStr",@"LEFT JOIN Actions act ON f.ActionID=act.ID 
LEFT JOIN Controllers c ON act.ControllerID=c.ID 
LEFT JOIN Areas a ON c.AreaID=a.ID
LEFT JOIN Functions f2 ON f.ParentID=f2.ID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",model.SearchPage),
                new SqlParameter("@orderBy","f.LevelID"),
                new SqlParameter("@showColumns",@"f.ID ,f.IsBackground,f.LevelID,
        f.FunctionName ,
        f2.FunctionName AS ParentFunctionName ,
        act.ActionName ,
        c.ControllerName ,
        a.AreaName"),
            };

            string conditionStr = "f.FunctionName LIKE '%" + model.FunctionName + "%'";

            if (model.IsMenu)//搜索菜单，搜索菜单的情况下，只额外考虑功能名、顶级功能、上级功能
            {
                conditionStr += " AND f.ActionID IS NULL";
                if (model.IsTopFunction)//顶级功能
                {
                    conditionStr += " AND f.ParentID IS NULL";
                }
                else if (model.ParentFunctionID != -1)//非顶级功能并且选了具体的上级功能
                {
                    conditionStr += " AND f.ParentID='" + model.ParentFunctionID + "'";
                }
            }
            else//非菜单，正常与Action绑定的功能
            {
                if (model.IsTopFunction)
                {
                    conditionStr += " AND f.ParentID IS NULL";
                }

                //1.选择了所属区域、未选所属控制器与具体上级功能
                if (model.AreaID != -1 && model.ControllerID == -1 && model.ParentFunctionID == -1)
                {
                    conditionStr += " AND c.AreaID='" + model.AreaID + "'";
                }
                //2.选择了具体的控制器，但是没选具体上级功能
                else if (model.ControllerID != -1 && model.ParentFunctionID == -1)
                {
                    conditionStr += " AND act.ControllerID='" + model.ControllerID + "'";
                }
                //3.非顶级功能且选择了具体的上级功能
                else if (model.ParentFunctionID != -1 && !model.IsTopFunction)
                {
                    conditionStr += " AND f.ParentID='" + model.ParentFunctionID + "'";
                }
            }

            if (model.IsAppFeatures)
            {
                conditionStr += " AND f.IsAppFeatures= 1";
            }

            //if (model.IsMenu)
            //{
            //    if (model.ParentFunctionID == -1)//没选上级功能
            //    {
            //        paras.Add(new SqlParameter("@conditionStr", "f.FunctionName LIKE '%" + model.FunctionName + "%' AND f.ActionID IS NULL"));
            //    }
            //    else
            //    {
            //        paras.Add(new SqlParameter("@conditionStr", "f.FunctionName LIKE '%" + model.FunctionName + "%' AND f.ActionID IS NULL AND f.ParentID='" + model.ParentFunctionID + "'"));
            //    }
            //}
            //else if (model.AreaID != -1 && model.ControllerID == -1 && model.ParentFunctionID == -1)//选择了区域，没选控制器与上级功能
            //{
            //    paras.Add(new SqlParameter("@conditionStr", "f.FunctionName LIKE '%" + model.FunctionName + "%' AND c.AreaID='" + model.AreaID + "'"));
            //}
            //else if (model.ControllerID != -1 && model.ParentFunctionID == -1)//选择了具体的控制器，但是没选上级功能
            //{
            //    paras.Add(new SqlParameter("@conditionStr", "f.FunctionName LIKE '%" + model.FunctionName + "%' AND act.ControllerID='" + model.ControllerID + "'"));
            //}
            //else if (model.ParentFunctionID != -1)//选择了上级功能
            //{
            //    paras.Add(new SqlParameter("@conditionStr", "f.FunctionName LIKE '%" + model.FunctionName + "%' AND f.ParentID='" + model.ParentFunctionID + "'"));
            //}
            //else//什么都没选
            //{
            //    paras.Add(new SqlParameter("@conditionStr", "f.FunctionName LIKE '%" + model.FunctionName + "%'"));
            //}



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

            List<FunctionListModel> list = ConvertToList<FunctionListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);


        }


        /// <summary>
        /// 获取全部功能下拉列表数据
        /// <para>包括功能编号、功能名称</para>
        /// </summary>
        /// <returns></returns>
        public static List<FunctionDDLModel> GetFunctions()
        {
            string sql = @"SELECT ID,FunctionName FROM dbo.Functions";
            return ConvertToList<FunctionDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }



        /// <summary>
        /// 获取不包含当前功能的功能下拉列表数据
        /// <para>包括功能编号、功能名称</para>
        /// <para>用于功能修改</para>
        /// </summary>
        /// <param name="currentFunctionID"></param>
        /// <returns></returns>
        public static List<FunctionDDLModel> GetFunctions(int currentFunctionID)
        {
            string sql = @"SELECT ID,FunctionName FROM dbo.Functions WHERE ID<>@currentFunctionID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@currentFunctionID",SqlDbType.Int)
            };
            paras[0].Value = currentFunctionID;
            return ConvertToList<FunctionDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

        /// <summary>
        ///  根据控制器编号获取全部功能下拉列表数据（用于级联）
        /// <para>包括功能编号、功能名称</para>
        /// </summary>
        /// <param name="controllerID">功能对应的动作所属控制器编号</param>
        /// <returns></returns>
        public static List<FunctionDDLModel> GetFunctionsByControllerID(int controllerID)
        {
            string sql = @"SELECT f.ID,f.FunctionName FROM dbo.Functions  f
LEFT JOIN dbo.Actions act ON f.ActionID=act.ID
LEFT JOIN dbo.Controllers c ON act.ControllerID=c.ID
WHERE c.ID=@controllerID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@controllerID",SqlDbType.Int),
            };
            paras[0].Value = controllerID;
            return ConvertToList<FunctionDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

        /// <summary>
        ///  根据区域编号获取全部功能下拉列表数据（用于级联）
        /// <para>包括功能编号、功能名称</para>
        /// </summary>
        /// <param name="areaID">功能对应的动作所属控制器所属区域编号</param>
        /// <returns></returns>
        public static List<FunctionDDLModel> GetFunctionsByAreaID(int areaID)
        {
            string sql = @"SELECT f.ID,f.FunctionName FROM dbo.Functions  f
LEFT JOIN dbo.Actions act ON f.ActionID=act.ID
LEFT JOIN dbo.Controllers c ON act.ControllerID=c.ID
LEFT JOIN dbo.Areas a ON c.AreaID=a.ID
WHERE  a.ID=@areaID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@areaID",SqlDbType.Int),
            };
            paras[0].Value = areaID;
            return ConvertToList<FunctionDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }




        /// <summary>
        /// 获得包括后台功能的全部功能树节点信息
        /// </summary>
        /// <returns></returns>
        public static List<FunctionTreeNodeModel> GetAllFunctionsForTree()
        {
            string sql = @"SELECT ID,FunctionName,ParentID FROM dbo.Functions";
            return ConvertToList<FunctionTreeNodeModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }


        /// <summary>
        /// 获得除后台功能外的功能树节点信息
        /// </summary>
        /// <returns></returns>
        public static List<FunctionTreeNodeModel> GetNormalFunctionsForTree()
        {
            string sql = @"SELECT ID,FunctionName,ParentID FROM dbo.Functions WHERE IsBackground=0 ";
            return ConvertToList<FunctionTreeNodeModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        /// <summary>
        /// 根据功能名称获取功能下拉数据（模糊查询）
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public static List<FunctionDDLModel> GetFunctionsByName(string functionName)
        {
            string sql = @"SELECT ID,FunctionName FROM dbo.Functions WHERE FunctionName LIKE @functionName";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@functionName",SqlDbType.NVarChar,52),
            };
            paras[0].Value = "%" + functionName + "%";
            return ConvertToList<FunctionDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion







        #region 新增
        /// <summary>
        /// 新增功能信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult AddFunction(FunctionAddModel model, int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FunctionName",SqlDbType.NVarChar,20),
                new SqlParameter("@ActionID",SqlDbType.Int),
                new SqlParameter("@IsBackground",SqlDbType.Bit),
                new SqlParameter("@ParentID",SqlDbType.Int),
                new SqlParameter("@Description", SqlDbType.NVarChar,50),
                new SqlParameter("@OrderIndex",SqlDbType.Int),
                new SqlParameter("CreateUserID",SqlDbType.Int),
                new SqlParameter("IsAppFeatures",SqlDbType.Bit),
                new SqlParameter("FeaturesCode",SqlDbType.VarChar),
                new SqlParameter("IsAppHomeModule",SqlDbType.Bit),
                new SqlParameter("IsAppShortcutMenu",SqlDbType.Bit),
            };

            paras[0].Value = model.FunctionName.Trim();


            if (model.FunctionIsMenu)
            {
                paras[1].Value = DBNull.Value;
            }
            else
            {
                paras[1].Value = model.ActionID;
            }


            paras[2].Value = model.IsBackground;


            if (model.IsTopFunction)
            {
                paras[3].Value = DBNull.Value;
            }
            else
            {
                paras[3].Value = model.ParentID;
            }

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                paras[4].Value = DBNull.Value;
            }
            else
            {
                paras[4].Value = model.Description;
            }

            paras[5].Value = model.OrderIndex;
            paras[6].Value = CreateUserID;
            paras[7].Value = model.IsAppFeatures;
            if (string.IsNullOrWhiteSpace(model.FeaturesCode))
            {
                paras[8].Value = DBNull.Value;
            }
            else
            {
                paras[8].Value = model.FeaturesCode.Trim();
            }
            paras[9].Value = model.IsAppHomeModule;
            paras[10].Value = model.IsAppShortcutMenu;
            string sql = @"INSERT  INTO dbo.Functions( FunctionName, ActionID,ParentID,IsBackground,Description,
                                     OrderIndex,CreateUserID, IsAppFeatures,FeaturesCode,IsAppHomeModule,IsAppShortcutMenu)
                                  VALUES  ( @FunctionName, @ActionID,@ParentID,@IsBackground,@Description,
                                  @OrderIndex ,@CreateUserID,@IsAppFeatures,@FeaturesCode,@IsAppHomeModule,@IsAppShortcutMenu)";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        /// <summary>
        /// 检查是否存在同名功能（用于新增 ）
        /// <para>True：存在；False：不存在</para>
        /// </summary>
        /// <param name="functionName">检查的功能名称</param>
        /// <returns></returns>
        public static bool CheckFunctionNameExists(string functionName)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@functionName",SqlDbType.NVarChar,20),
            };
            paras[0].Value = functionName.Trim();
            string sql = "SELECT COUNT(0) FROM Functions WHERE functionName=@functionName";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查是否存在同名的功能编码（用于新增 ）
        /// <para>True：存在；False：不存在</para>
        /// </summary>
        /// <param name="featuresCode">检查的功能编码</param>
        /// <returns></returns>
        public static bool CheckAddFunctionNameExists(string featuresCode)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FeaturesCode",SqlDbType.VarChar),
            };
            paras[0].Value = featuresCode.Trim();
            string sql = "SELECT COUNT(0) FROM Functions WHERE FeaturesCode=@FeaturesCode";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        #endregion




        #region 编辑
        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public static SelectResult<FunctionEditModel> GetFunctionByID(int functionID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = functionID;
            string sql = @"SELECT  fun.ID ,
        fun.FunctionName ,
        fun.ActionID ,
        fun.ParentID ,
        fun.Description,
        fun.IsBackground,
        fun.OrderIndex, 
        act.ControllerID ,
        c.AreaID,
        fun.IsAppFeatures,
        fun.FeaturesCode,fun.IsAppHomeModule,fun.IsAppShortcutMenu 
FROM    dbo.Functions fun
        LEFT JOIN dbo.Actions act ON fun.ActionID = act.ID
        LEFT JOIN dbo.Controllers c ON act.ControllerID = c.ID
WHERE   fun.ID = @ID";
            List<FunctionEditModel> list = ConvertToList<FunctionEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            FunctionEditModel data = null;
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
            return new SelectResult<FunctionEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        /// <summary>
        /// 编辑功能信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult EditFunction(FunctionEditModel model, int EditUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FunctionName",SqlDbType.NVarChar,20),
                new SqlParameter("@ActionID",SqlDbType.Int),
                new SqlParameter("@IsBackground",SqlDbType.Bit),
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@ParentID",SqlDbType.Int),
                new SqlParameter("@Description", SqlDbType.NVarChar,50),
                new SqlParameter("@OrderIndex",SqlDbType.Int),
                new SqlParameter("EditUserID",SqlDbType.Int),
                new SqlParameter("EditTime",SqlDbType.DateTime),
                new SqlParameter("IsAppFeatures",SqlDbType.Bit),
                new SqlParameter("FeaturesCode",SqlDbType.VarChar),
                new SqlParameter("IsAppHomeModule",SqlDbType.Bit),
                new SqlParameter("IsAppShortcutMenu",SqlDbType.Bit),
            };

            paras[0].Value = model.FunctionName.Trim();
            if (model.FunctionIsMenu)
            {
                paras[1].Value = DBNull.Value;
            }
            else
            {
                paras[1].Value = model.ActionID;
            }
            paras[2].Value = model.IsBackground;
            paras[3].Value = model.ID;
            if (model.IsTopFunction)
            {
                paras[4].Value = DBNull.Value;
            }
            else
            {
                paras[4].Value = model.ParentID;
            }
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.Description;
            }

            paras[6].Value = model.OrderIndex;
            paras[7].Value = EditUserID;
            paras[8].Value = DateTime.Now;
            paras[9].Value = model.IsAppFeatures;
            if (string.IsNullOrWhiteSpace(model.FeaturesCode))
            {
                paras[10].Value = DBNull.Value;
            }
            else
            {
                paras[10].Value = model.FeaturesCode.Trim();
            }
            paras[11].Value = model.IsAppHomeModule;
            paras[12].Value = model.IsAppShortcutMenu;

            string sql = @"UPDATE  dbo.Functions
SET     FunctionName = @FunctionName,
        ActionID = @ActionID,
        ParentID =@ParentID,
        IsBackground=@IsBackground,
        [Description] = @Description,
        IsAppFeatures = @IsAppFeatures,FeaturesCode = @FeaturesCode,IsAppHomeModule = @IsAppHomeModule,
        IsAppShortcutMenu = @IsAppShortcutMenu,
        OrderIndex=@OrderIndex,EditTime=@EditTime,EditUserID=@EditUserID
WHERE   ID =@ID";
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

        /// <summary>
        /// 检查当前功能之外是否存在同名功能（用于编辑）
        /// <para>True：存在；False：不存在</para>
        /// </summary>
        /// <param name="functionName">检查的功能名称</param>
        /// <param name="functionID">当前编辑的功能的编号</param>
        /// <returns></returns>
        public static bool CheckFunctionNameExists(string functionName, int functionID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@functionName",SqlDbType.NVarChar,20),
                new SqlParameter("@functionID",SqlDbType.Int),
            };
            paras[0].Value = functionName.Trim();
            paras[1].Value = functionID;

            string sql = "SELECT COUNT(0) FROM Functions WHERE functionName=@functionName AND ID!=@functionID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查当前功能之外是否存在同名功能编码（用于编辑）
        /// <para>True：存在；False：不存在</para>
        /// </summary>
        /// <param name="featuresCode">检查的功能名称</param>
        /// <param name="functionID">当前编辑的功能的编号</param>
        /// <returns></returns>
        public static bool CheckEditFeaturesCodeExists(string featuresCode, int functionID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FeaturesCode",SqlDbType.VarChar),
                new SqlParameter("@FunctionID",SqlDbType.Int),
            };
            paras[0].Value = featuresCode.Trim();
            paras[1].Value = functionID;

            string sql = "SELECT COUNT(0) FROM Functions WHERE FeaturesCode=@FeaturesCode AND ID!=@functionID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        #endregion




        #region 删除
        /// <summary>
        /// 批量删除功能信息（物理删除）
        /// 删除时级联删除直接与间接子节点
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult DeleteFunctions(string[] ids)
        {

            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"WITH CTE
AS
(
	SELECT ID,ParentID FROM dbo.Functions WHERE ID=@ID
	UNION ALL
	SELECT f.ID,f.ParentID FROM dbo.Functions  f
	INNER JOIN CTE ON f.ParentID=CTE.ID
)
DELETE dbo.Functions FROM CTE WHERE dbo.Functions.ID=CTE.ID";
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


            //string[] sqls = new string[ids.Length];
            //SqlParameter[][] paras = new SqlParameter[ids.Length][];
            //for (int i = 0; i < ids.Length; i++)
            //{
            //    sqls[i] = "DELETE FROM Functions WHERE ID=@ID";
            //    paras[i] = new SqlParameter[1];
            //    SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
            //    temp.Value = ids[i];
            //    paras[i][0] = temp;
            //}
            //bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            //return new OperationResult()
            //{
            //    Success = result,
            //    Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            //};
        }
        #endregion




    }
}

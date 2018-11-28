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
    public class ActionBLL
    {

        #region 查询
        /// <summary>
        /// 获取动作信息分页数据
        /// </summary>
        public static AsiatekPagedList<ActionListModel> GetPagedActions(ActionSettingModel model, int pageSize)
        {
            return GetPagedActions(pageSize, model.SearchPage, model.ActionName, model.ControllerID, model.AreaID);
        }



        /// <summary>
        /// 获取动作信息分页数据
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="actionName">动作名称</param>
        /// <param name="controllerID">动作所属控制器ID</param>
        /// <param name="areaID">动作所属控制器所属区域ID</param>
        /// <returns></returns>
        public static AsiatekPagedList<ActionListModel> GetPagedActions(int pageSize, int currentPage = 1, string actionName = "", int controllerID = -1, int areaID = -1)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","Actions act"),
                new SqlParameter("@joinStr","INNER JOIN Controllers c ON act.ControllerID=c.ID INNER JOIN Areas a ON c.AreaID=a.ID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",currentPage),
                new SqlParameter("@orderBy","act.ID"),
                new SqlParameter("@showColumns","act.ID,act.ActionName,act.Description,c.ControllerName,a.AreaName"),
            };


            //如果选择了区域，但是控制器选择了全部
            if (areaID != -1 && controllerID == -1)
            {
                paras.Add(new SqlParameter("@conditionStr", "act.ActionName LIKE '%" + actionName + "%' AND c.AreaID='" + areaID + "'"));
            }
            else if (controllerID != -1)//选择了具体的控制器
            {
                paras.Add(new SqlParameter("@conditionStr", "act.ActionName LIKE '%" + actionName + "%' AND act.ControllerID='" + controllerID + "'"));
            }
            else
            {
                paras.Add(new SqlParameter("@conditionStr", "act.ActionName LIKE '%" + actionName + "%'"));
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

            List<ActionListModel> list = ConvertToList<ActionListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        /// <summary>
        /// 根据控制器编号获取动作下拉列表数据
        /// <para>包含动作编号、动作名称</para>
        /// </summary>
        /// <param name="controllerID">动作所属控制器编号</param>
        /// <returns></returns>
        public static List<ActionDDLModel> GetActionsByControllerID(int controllerID)
        {
            string sql = @"SELECT ID,ActionName FROM Actions WHERE controllerID=@controllerID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@controllerID",SqlDbType.Int),
            };
            paras[0].Value = controllerID;
            return ConvertToList<ActionDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }



        /// <summary>
        /// 根据动作方法编号检查其是否已经与某个功能数据关联（用于新增功能）
        /// <para>True:存在关联；False：无关联</para>
        /// </summary>
        /// <param name="actionID"></param>
        /// <returns></returns>
        public static bool CheckActionIDHadBeenLinkedWithFunction(int actionID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@actionID",SqlDbType.Int),
            };
            paras[0].Value = actionID;
            string sql = "SELECT COUNT(0) FROM dbo.Functions WHERE ActionID=@actionID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 根据动作方法编号与功能编号检查其是否已经与非指定的功能数据关联(用于修改功能）
        /// <para>True:存在关联；False：无关联</para>
        /// </summary>
        /// <param name="actionID"></param>
        /// <returns></returns>
        public static bool CheckActionIDHadBeenLinkedWithFunction(int actionID, int functionID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@actionID",SqlDbType.Int),
                new SqlParameter("@functionID",SqlDbType.Int),
            };
            paras[0].Value = actionID;
            paras[1].Value = functionID;
            string sql = "SELECT COUNT(0) FROM dbo.Functions WHERE ActionID=@actionID AND ID<>@functionID";
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
        /// 根据动作编号获取动作信息（用于编辑）
        /// </summary>
        /// <param name="actionID"></param>
        /// <returns></returns>
        public static SelectResult<ActionEditModel> GetActionByID(int actionID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID", SqlDbType.Int)
            };
            paras[0].Value = actionID;


            string sql = @"SELECT  act.ID ,
                                    act.ActionName ,
                                    act.ControllerID ,
                                    act.Description,
                                    c.AreaID
                            FROM    dbo.Actions act
                                    INNER JOIN dbo.Controllers c ON act.ControllerID = c.ID
                            WHERE   act.ID = @ID";
            List<ActionEditModel> list = ConvertToList<ActionEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            ActionEditModel data = null;
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
            return new SelectResult<ActionEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        /// <summary>
        /// 修改动作信息
        /// <para></para>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult ModifyAction(ActionEditModel model, int EditUserID)
        {

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ControllerID",SqlDbType.Int),
                new SqlParameter("@ActionName",SqlDbType.NVarChar,50),
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@Description",SqlDbType.NVarChar,50),
                new SqlParameter("@EditUserID",SqlDbType.Int),
                new SqlParameter("@EditTime",SqlDbType.DateTime)

            };
            paras[0].Value = model.ControllerID;
            paras[1].Value = model.ActionName.Trim();
            paras[2].Value = model.ID;

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                paras[3].Value = DBNull.Value;
            }
            else
            {
                paras[3].Value = model.Description;
            }
            paras[4].Value = EditUserID;
            paras[5].Value = DateTime.Now;




            string sql = "UPDATE Actions SET ActionName=@ActionName,ControllerID=@ControllerID,Description=@Description,EditUserID=@EditUserID,EditTime=@EditTime WHERE ID=@ID";
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
        /// 检查同控制器下非当前动作是否具有有名动作方法（用于编辑）
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerID"></param>
        /// <param name="actionID"></param>
        /// <returns></returns>
        public static bool CheckActionNameExists(string actionName, int controllerID, int actionID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@actionName",SqlDbType.NVarChar,50),
                new SqlParameter("@controllerID",SqlDbType.Int),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = actionName.Trim();
            paras[1].Value = controllerID;
            paras[2].Value = actionID;


            string sql = "SELECT COUNT(0) FROM Actions WHERE actionName=@actionName AND controllerID=@controllerID AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion



        #region 新增
        /// <summary>
        /// 新增动作信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult AddAction(ActionAddModel model, int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ActionName",SqlDbType.NVarChar,50),
                new SqlParameter("@ControllerID",SqlDbType.Int),
                new SqlParameter("@Description",SqlDbType.NVarChar,50),
                new SqlParameter("@CreateUserID",SqlDbType.Int)
            };
            paras[0].Value = model.ActionName.Trim();
            paras[1].Value = model.ControllerID;

            if (string.IsNullOrWhiteSpace(model.Description))
            {
                paras[2].Value = DBNull.Value;
            }
            else
            {
                paras[2].Value = model.Description;
            }
            paras[3].Value = CreateUserID;



            string sql = @"INSERT  INTO dbo.Actions( ActionName, ControllerID,Description,CreateUserID )VALUES  ( @ActionName, @ControllerID,@Description,@CreateUserID )";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        /// <summary>
        /// 检查同控制器下是否具有同名动作方法
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerID"></param>
        /// <returns></returns>
        public static bool CheckActionNameExists(string actionName, int controllerID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@actionName",SqlDbType.NVarChar,50),
                new SqlParameter("@controllerID",SqlDbType.Int),
            };
            paras[0].Value = actionName.Trim();
            paras[1].Value = controllerID;


            string sql = "SELECT COUNT(0) FROM Actions WHERE actionName=@actionName AND controllerID=@controllerID";
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
        /// 根据动作编号批量删除（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult DeleteActions(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = "DELETE FROM Actions WHERE ID=@ID";
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




    }
}

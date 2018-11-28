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
    public class ControllerBLL
    {


        #region 查询

        public static AsiatekPagedList<ControllerListModel> GetPagedControllerInfo(ControllerSearchModel model, int searchPage, int pageSize)
        {
            return GetPagedControllerInfo(pageSize, searchPage, model.AreaID, model.ControllerName);
        }

        public static AsiatekPagedList<ControllerListModel> GetPagedControllerInfo(ControllerSettingModel model, int pageSize)
        {
            return GetPagedControllerInfo(pageSize, model.SearchPage, model.AreaID, model.ControllerName);
        }



        public static AsiatekPagedList<ControllerListModel> GetPagedControllerInfo(int pageSize, int currentPage = 1, int areaID = -1, string controllerName = "")
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","Controllers c"),
                new SqlParameter("@joinStr","INNER JOIN Areas a ON c.AreaID=a.ID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",currentPage),
                new SqlParameter("@orderBy","c.ID"),
                new SqlParameter("@showColumns","c.ID,c.ControllerName,c.Description,a.AreaName"),
            };

            if (areaID != -1)
            {
                paras.Add(new SqlParameter("@conditionStr", "c.ControllerName LIKE '%" + controllerName + "%' AND AreaID='" + areaID + "'"));
            }
            else
            {
                paras.Add(new SqlParameter("@conditionStr", "c.ControllerName LIKE '%" + controllerName + "%'"));
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
            List<ControllerListModel> list = ConvertToList<ControllerListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        /// <summary>
        /// 获取控制器下拉列表信息
        /// 包含控制器编号、控制器名称
        /// </summary>
        /// <returns></returns>
        public static List<ControllerDDLModel> GetControllers()
        {
            string sql = "SELECT ID,ControllerName FROM Controllers";
            return ConvertToList<ControllerDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        /// <summary>
        /// 根据区域编号获取控制器下拉数据信息
        /// 用于级联
        /// </summary>
        /// <param name="areaID">所属区域编号</param>
        /// <returns></returns>
        public static List<ControllerDDLModel> GetControllersByAreaID(int areaID)
        {
            string sql = "SELECT ID,ControllerName FROM Controllers WHERE areaID=@areaID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@areaID",SqlDbType.Int),
            };
            paras[0].Value = areaID;
            return ConvertToList<ControllerDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增控制器信息
        /// <para>包括 控制名称、所属区域、控制器描述</para>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult AddController(ControllerAddModel model, int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ControllerName",SqlDbType.NVarChar,50),
                new SqlParameter("@AreaID",SqlDbType.Int),
                new SqlParameter("@Description",SqlDbType.NVarChar,50),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
            };
            paras[0].Value = model.ControllerName.Trim();
            paras[1].Value = model.AreaID;



            if (string.IsNullOrWhiteSpace(model.Description))
            {
                paras[2].Value = DBNull.Value;
            }
            else
            {
                paras[2].Value = model.Description;
            }
            paras[3].Value = CreateUserID;
            string sql = @"INSERT INTO dbo.Controllers
        ( ControllerName, AreaID,Description,CreateUserID )
VALUES  ( @ControllerName,@AreaID,@Description,@CreateUserID )";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        /// <summary>
        /// 检查同一个区域下是否存在同名控制器（用于新增）
        /// <para>True:存在；false：不存在</para>
        /// </summary>
        /// <param name="controllerName">控制器名</param>
        /// <param name="areaID">所属区域编号</param>
        /// <returns>True:存在；false：不存在</returns>
        public static bool CheckControllerNameExists(string controllerName, int areaID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ControllerName",SqlDbType.NVarChar,50),
                new SqlParameter("@AreaID",SqlDbType.Int),
            };
            paras[0].Value = controllerName.Trim();
            paras[1].Value = areaID;



            string sql = "SELECT COUNT(0) FROM Controllers WHERE ControllerName=@ControllerName AND AreaID=@AreaID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 根据控制器编号获取待修改的控制器信息
        /// </summary>
        /// <param name="controllerID">控制器编号</param>
        /// <returns></returns>
        public static SelectResult<ControllerEditModel> GetControllerInfoByID(int controllerID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = controllerID;
            string sql = "SELECT * FROM Controllers WHERE ID=@ID";
            List<ControllerEditModel> list = ConvertToList<ControllerEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            ControllerEditModel data = null;
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
            return new SelectResult<ControllerEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }
        /// <summary>
        /// 修改控制器
        /// <para>包括控制器名、控制器所属区域、控制器描述</para>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult ModifyController(ControllerEditModel model, int EditUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AreaID",SqlDbType.Int),
                new SqlParameter("@ControllerName",SqlDbType.NVarChar,50),
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@Description",SqlDbType.NVarChar,50),
                new SqlParameter("EditUserID",SqlDbType.Int),
                new SqlParameter("EditTime",SqlDbType.DateTime)
            };

            paras[0].Value = model.AreaID;
            paras[1].Value = model.ControllerName.Trim();
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
            string sql = "UPDATE Controllers SET ControllerName=@ControllerName,AreaID=@AreaID,Description=@Description,EditTime=@EditTime,EditUserID=@EditUserID WHERE ID=@ID";
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
        /// 检查当前控制器所属的同一区域下是否存在同名控制器（用于修改）
        /// <para>True：存在；False：不存在</para>
        /// </summary>
        /// <param name="controllerName">控制器名称</param>
        /// <param name="areaID">所属区域编号</param>
        /// <param name="controllerID">当前控制器编号</param>
        /// <returns>True：存在；False：不存在</returns>
        public static bool CheckControllerNameExists(string controllerName, int areaID, int controllerID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ControllerName",SqlDbType.NVarChar,50),
                new SqlParameter("@AreaID",SqlDbType.Int),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = controllerName.Trim();
            paras[1].Value = areaID;
            paras[2].Value = controllerID;
            string sql = "SELECT COUNT(0) FROM Controllers WHERE ControllerName=@ControllerName AND AreaID=@AreaID AND ID<>@ID";
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
        /// 根据控制器编号批量删除（物理删除）
        /// </summary>
        /// <param name="ids">要删除的所有控制器编号</param>
        /// <returns></returns>
        public static OperationResult DeleteControllers(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = "DELETE FROM Controllers WHERE ID=@ID";
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

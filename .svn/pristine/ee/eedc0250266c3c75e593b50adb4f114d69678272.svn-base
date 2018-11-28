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
    /// <summary>
    /// 区域信息业务
    /// </summary>
    public class AreaBLL
    {
        #region 查询

        /// <summary>
        /// 获取区域信息分页数据
        /// </summary>
        public static AsiatekPagedList<AreaModel> GetPagedAreaInfo(AreaSearchModel model, int searchPage, int pageSize)
        {
            return GetPagedAreaInfo(searchPage, pageSize, model.AreaName);
        }

        /// <summary>
        /// 获取区域信息分页数据
        /// </summary>
        public static AsiatekPagedList<AreaModel> GetPagedAreaInfo(AreaSettingModel model, int pageSize)
        {
            return GetPagedAreaInfo(model.SearchPage, pageSize, model.AreaName);
        }


        /// <summary>
        /// 获取区域信息分页数据
        /// </summary>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="areaName">区域名称</param>
        /// <returns></returns>
        public static AsiatekPagedList<AreaModel> GetPagedAreaInfo(int currentPage, int pageSize, string areaName = "")
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","Areas"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",currentPage),
                new SqlParameter("@orderBy","ID"),
                new SqlParameter("@conditionStr","AreaName LIKE '%"+areaName+"%'"),
                new SqlParameter()
                {
                    ParameterName="@totalItemCount",
                    Direction=ParameterDirection.Output,
                    SqlDbType=SqlDbType.Int
                },
                new SqlParameter()
                {
                    ParameterName="@newCurrentPage",
                    Direction=ParameterDirection.Output,
                    SqlDbType=SqlDbType.Int
                },
            };
            List<AreaModel> list = ConvertToList<AreaModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));

            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }




        /// <summary>
        /// 获取区域信息下拉列表数据
        /// 包含区域编号、区域名称
        /// </summary>
        /// <returns></returns>
        public static List<AreaDDLModel> GetAreas()
        {
            string sql = "SELECT ID,AreaName FROM Areas";
            return ConvertToList<AreaDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增区域信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult AddAreaInfo(AreaAddModel model, int UserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter()
            {
                ParameterName = "@areaName",
                SqlDbType = SqlDbType.NVarChar,
                Size = 20,
                Value = model.AreaName.Trim(),
            });

            paras.Add(new SqlParameter("@description", SqlDbType.NVarChar, 50));
            paras.Add(new SqlParameter("@CreateUserID", SqlDbType.Int));
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                paras[1].Value = DBNull.Value;
            }
            else
            {
                paras[1].Value = model.Description;
            }
            paras[2].Value = UserID;
            string sql = @"INSERT INTO dbo.Areas
        ( AreaName, Description,CreateUserID )
VALUES  ( @areaName,@description,@CreateUserID);";



            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }


        /// <summary>
        /// 检查是否有同名区域（用于新增）
        /// 新增时只要同名就不可以添加
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <returns>True：存在；false：不存在</returns>
        public static bool CheckAreaNameExists(string areaName)
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter()
            {
                ParameterName = "@areaName",
                SqlDbType = SqlDbType.NVarChar,
                Size = 20,
                Value = areaName.Trim(),
            });

            string sql = "SELECT COUNT(0) FROM Areas WHERE AreaName=@AreaName";
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
        /// 检查不是当前区域编号的其他区域是否与该区域名同名（用于修改）
        /// 修改的时候，如果输入的区域名称被其他区域所使用则不可以修改
        /// </summary>
        /// <param name="areaName">区域名称</param>
        /// <param name="areaID">区域编号</param>
        /// <returns></returns>
        public static bool CheckAreaNameExists(string areaName, int areaID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@areaName",SqlDbType.NVarChar,20),
                new SqlParameter("@areaID",SqlDbType.Int),
            };

            paras[0].Value = areaName.Trim();
            paras[1].Value = areaID;
            string sql = "SELECT COUNT(0) FROM Areas WHERE AreaName=@AreaName AND ID!=@areaID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 修改区域信息
        /// 根据区域编号修改区域名、区域描述
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult ModifyAreaInfo(AreaEditModel model, int UserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@areaName",SqlDbType.NVarChar,20),
                new SqlParameter("@areaID",SqlDbType.Int),
                new SqlParameter("@description", SqlDbType.NVarChar, 50),
                new SqlParameter("@EditTime",SqlDbType.DateTime),
                new SqlParameter("@EditUserID", SqlDbType.Int)
            };

            paras[0].Value = model.AreaName.Trim();
            paras[1].Value = model.ID;


            if (string.IsNullOrWhiteSpace(model.Description))
            {
                paras[2].Value = DBNull.Value;
            }
            else
            {
                paras[2].Value = model.Description;
            }
            paras[3].Value = DateTime.Now;
            paras[4].Value = UserID;
            string sql = "UPDATE Areas SET AreaName=@areaName,Description=@description,EditTime=@EditTime,EditUserID=@EditUserID WHERE ID=@areaID";
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
        /// 根据区域编号获取区域信息
        /// </summary>
        /// <param name="areaID">区域编号</param>
        /// <returns></returns>
        public static SelectResult<AreaEditModel> GetAreaByID(int areaID)
        {

            SqlParameter para = new SqlParameter("@ID", SqlDbType.Int);
            para.Value = areaID;
            SqlParameter[] paras = { para };


            string sql = "SELECT * FROM Areas WHERE ID=@ID";
            List<AreaEditModel> list = ConvertToList<AreaEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));

            AreaEditModel data = null;
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
            return new SelectResult<AreaEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }


        #endregion


        #region 删除
        /// <summary>
        /// 根据区域编号批量删除区域信息（物理删除）
        /// </summary>
        /// <param name="ids">要删除的所有区域编号</param>
        /// <returns></returns>
        public static OperationResult DeleteAreas(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = "DELETE FROM Areas WHERE ID=@ID";
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

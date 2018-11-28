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
    public class ExceptionTypeBLL
    {
        #region   查询
        public static AsiatekPagedList<ExceptionTypeListModel> GetPagedExceptionType(ExceptionTypeSeachModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","ExceptionType"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","ID"),
                new SqlParameter("@showColumns",@"ID,ExName,Remark"),
            };

            string conditionStr = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(model.ExName))
            {
                conditionStr += " AND ExName LIKE '%" + model.ExName + "%'";
            }

            if (!string.IsNullOrWhiteSpace(conditionStr))
            {
                paras.Add(new SqlParameter("@conditionStr", conditionStr));
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
            List<ExceptionTypeListModel> list = ConvertToList<ExceptionTypeListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion


        #region   新增
        public static OperationResult AddExceptionType(ExceptionTypeAddModel model,int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ExName",SqlDbType.NVarChar,10),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                 new SqlParameter("@ID",SqlDbType.NVarChar,10)
            };

            paras[0].Value = model.ExName;

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[1].Value = DBNull.Value;
            }
            else
            {
                paras[1].Value = model.Remark;
            }
            paras[2].Value = CreateUserID;
            paras[3].Value=model.ID;
            #region  SQL
            string sql;
            #endregion

            sql = @"INSERT INTO dbo.ExceptionType(ID,ExName,Remark,CreateUserID) VALUES (@ID,@ExName,@Remark,@CreateUserID)";


            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        public static bool CheckAddExceptionIDExists(int id) {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.NVarChar,20),
            };
            paras[0].Value = id;
            string sql = "SELECT COUNT(0) FROM ExceptionType WHERE ID=@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) == 1;
        
        }
        #endregion

        #region   编辑
        public static SelectResult<ExceptionTypeEditModel> GetExceptionTypeID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@id",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT * FROM dbo.ExceptionType WHERE ID=@id";
            List<ExceptionTypeEditModel> list = ConvertToList<ExceptionTypeEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            ExceptionTypeEditModel data = null;
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
            return new SelectResult<ExceptionTypeEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }


        public static OperationResult EditExceptionType(ExceptionTypeEditModel model, int EditUserID)
        {

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@ExName",SqlDbType.NVarChar,30),
                new SqlParameter("@Remark",SqlDbType.NVarChar,50),
                new SqlParameter("@EditUserID",SqlDbType.Int),
                new SqlParameter("@EditTime",SqlDbType.NVarChar,50)
            };

            paras[0].Value = model.ID;
            paras[1].Value = model.ExName;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[2].Value = DBNull.Value;
            }
            else
            {
                paras[2].Value = model.Remark;
            }
            paras[3].Value = EditUserID;
            paras[4].Value = DateTime.Now;
            #region  SQL
            string sql;

            sql = @"UPDATE  dbo.ExceptionType
SET     ExName = @ExName ,Remark=@Remark,EditTime=@EditTime,EditUserID=@EditUserID
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

        public static bool CheckEditExceptionIDExists(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.NVarChar,20),
            };
            paras[0].Value = id;
            string sql = "SELECT COUNT(0) FROM ExceptionType WHERE ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) == 1;

        }
        #endregion

        #region 下拉列表
        public static List<ExceptionTypeDDLModel> GetExTypes(int collectID)
        {
            string sql = @"SELECT ID,ExName FROM dbo.ExceptionType WHERE CollectID=@CollectID";
            SqlParameter para = new SqlParameter()
            {
                ParameterName = "@CollectID",
                SqlDbType = SqlDbType.Int,
                Value = collectID
            };
            return ConvertToList<ExceptionTypeDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, para));
        }
        #endregion
    }
}

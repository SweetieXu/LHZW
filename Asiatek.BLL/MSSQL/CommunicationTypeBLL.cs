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
    public class CommunicationTypeBLL
    {
        #region   查询
        public static AsiatekPagedList<CommunicationTypeListModel> GetPagedCommunicationType(CommunicationTypeSeachModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","CommunicationType"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","ID"),
                new SqlParameter("@showColumns",@"ID ,
        Name,
        Remark,
        Status"),
            };

            string conditionStr = " Status<>9 ";//不查询删除
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                conditionStr += " AND Name LIKE '%" + model.Name + "%'";
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
            List<CommunicationTypeListModel> list = ConvertToList<CommunicationTypeListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion


        #region   新增
        public static OperationResult AddCommunicationType(AddCommunicationTypeModel model, int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Name",SqlDbType.NVarChar,30),
                new SqlParameter("@Remark",SqlDbType.NVarChar,200),
                new SqlParameter("@CreateUserID",SqlDbType.Int)
            };

            paras[0].Value = model.Name.Trim();

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[1].Value = DBNull.Value;
            }
            else
            {
                paras[1].Value = model.Remark.Trim();
            }
            paras[2].Value = CreateUserID;
            #region  SQL
            string sql;
            #endregion

            sql = @"INSERT INTO dbo.CommunicationType(Name,Remark,Status,CreateUserID)
VALUES (@Name,@Remark,'0',@CreateUserID)";


            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region   编辑
        public static SelectResult<EditCommunicationTypeModel> GetCommunicationTypeID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@id",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT * FROM dbo.CommunicationType WHERE ID=@id";
            List<EditCommunicationTypeModel> list = ConvertToList<EditCommunicationTypeModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditCommunicationTypeModel data = null;
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
            return new SelectResult<EditCommunicationTypeModel>()
            {
                DataResult = data,
                Message = msg
            };
        }


        public static OperationResult EditCommunicationType(EditCommunicationTypeModel model,int EditUserID)
        {

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@Name",SqlDbType.NVarChar,30),
                new SqlParameter("@Remark",SqlDbType.NVarChar,200),
                new SqlParameter("@EditUserID",SqlDbType.Int),
                new SqlParameter("@EditTime",SqlDbType.DateTime),
            };

            paras[0].Value = model.ID;
            paras[1].Value = model.Name.Trim();


            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[2].Value = DBNull.Value;
            }
            else
            {
                paras[2].Value = model.Remark.Trim();
            }
            paras[3].Value = EditUserID;
            paras[4].Value = DateTime.Now;
            #region  SQL
            string sql;

            sql = @"UPDATE  dbo.CommunicationType
SET     Name=@Name,Remark=@Remark,EditUserID=@EditUserID,EditTime=@EditTime
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


        #region   删除
        /// <summary>
        /// 删除（逻辑删除）
        /// </summary>
        public static OperationResult DeleteCommunicationType(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.CommunicationType SET [Status]=9 WHERE ID=@ID";
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

        #region   删除
        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        public static OperationResult DeleteCommunicationTypePhysical(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.CommunicationType WHERE ID=@ID";
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

        #region 其他
        /// <summary>
        /// 根据通信方式名获取通信方式编码
        /// </summary>
        public static bool TryGetCodeByName(string name, out int code)
        {
            code = -1;
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NChar,20),
            };
            paras[0].Value = name;
            string sql = "SELECT ID FROM CommunicationType WHERE Name=@name";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)
            {
                return false;
            }
            code = Convert.ToInt32(result);
            return true;
        }
        #endregion
    }
}

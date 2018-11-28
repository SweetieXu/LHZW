using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asiatek.AjaxPager;
using Asiatek.Model;
using System.Data.SqlClient;
using Asiatek.Model;
using Asiatek.DBUtility;
using System.Data;
using Asiatek.Resource;

namespace Asiatek.BLL.MSSQL
{
    public class ReceiverMailInfoBLL
    {
        #region   查询

        public static AsiatekPagedList<ReceiverMailInfoListModel> GetPagedReceiverMailInfo(ReceiverMailInfoSearchModel model, int searchPage, int pageSize)
        {
            string joinStr = string.Empty;


            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","ReceiverMailInfo rmi"),
                new SqlParameter("@joinStr",joinStr),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","rmi.CreateTime desc"),
                new SqlParameter("@showColumns",@"rmi.ID ,rmi.Email,rmi.Name,rmi.Remark,rmi.Status"),
};
            string conditionStr = " 1=1 ";//不查询删除
            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                conditionStr += (string.Format(" and rmi.Email like '%{0}%'", model.Email.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                conditionStr += (string.Format(" and rmi.Name like '%{0}%'", model.Name.Trim()));
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
            List<ReceiverMailInfoListModel> list = ConvertToList<ReceiverMailInfoListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region 新增
        public static OperationResult AddReceiverMailInfo(ReceiverMailInfoFormModel model, int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Email",SqlDbType.NVarChar,60),
                new SqlParameter("@Name",SqlDbType.NVarChar,20),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@Status",SqlDbType.TinyInt),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
               
            };

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                paras[0].Value = DBNull.Value;
            }
            else
            {
                paras[0].Value = model.Email.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                paras[1].Value = DBNull.Value;
            }
            else
            {
                paras[1].Value = model.Name.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[2].Value = DBNull.Value;
            }
            else
            {
                paras[2].Value = model.Remark;
            }

            paras[3].Value = model.Status;
            paras[4].Value = CreateUserID;
            paras[5].Value = DateTime.Now;

            string sql = @"INSERT INTO dbo.ReceiverMailInfo(Email,Name,Remark,Status,CreateUserID,CreateTime) 
                                                                                       VALUES (@Email,@Name,@Remark,@Status,@CreateUserID,@CreateTime)";


            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 编辑

        public static OperationResult EditReceiverMailInfo(ReceiverMailInfoFormModel model, int EditUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Email",SqlDbType.NVarChar,60),
                new SqlParameter("@Name",SqlDbType.NVarChar,20),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@Status",SqlDbType.TinyInt),
                new SqlParameter("@UpdateUserID",SqlDbType.Int),
                new SqlParameter("@UpdateTime",SqlDbType.DateTime),
                  new SqlParameter("@ID",SqlDbType.Int),
            };

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                paras[0].Value = DBNull.Value;
            }
            else
            {
                paras[0].Value = model.Email.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                paras[1].Value = DBNull.Value;
            }
            else
            {
                paras[1].Value = model.Name.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[2].Value = DBNull.Value;
            }
            else
            {
                paras[2].Value = model.Remark;
            }

            paras[3].Value = model.Status;
            paras[4].Value = EditUserID;
            paras[5].Value = DateTime.Now;
            paras[6].Value = model.ID;

            string sql;

            sql = @"UPDATE  dbo.ReceiverMailInfo
                            SET  Email=@Email,Name = @Name,Remark=@Remark,
                            Status=@Status,UpdateUserID = @UpdateUserID,UpdateTime = @UpdateTime
                            WHERE   ID = @ID";



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

        public static SelectResult<ReceiverMailInfoFormModel> GetReceiverMailInfo(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@id",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @" select ID ,Email,Name,Remark,Status
                                        FROM dbo.ReceiverMailInfo 
                                        WHERE ID=@id";
            List<ReceiverMailInfoFormModel> list = ConvertToList<ReceiverMailInfoFormModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            ReceiverMailInfoFormModel data = null;
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
            return new SelectResult<ReceiverMailInfoFormModel>()
            {
                DataResult = data,
                Message = msg
            };
        }
        #endregion

        #region 删除
        /// <summary>
        /// 物理删除
        /// </summary>
        public static OperationResult DeleteReceiverMailInfo(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.ReceiverMailInfo WHERE ID=@ID";
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

        #region 扩展方法
        /// <summary>
        /// 检查邮箱是否重复
        /// </summary>
        public static bool CheckReceiverMailInfoExists(string email, int? id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Email",SqlDbType.NVarChar,60),
            };

            paras[0].Value = email.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.ReceiverMailInfo WHERE Email=@Email ";
            if (id != null)
            {
                sql += (" AND ID<>@ID");
                SqlParameter sp = new SqlParameter("@ID", SqlDbType.Int);
                sp.Value = id;
                paras.Add(sp);
            }
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

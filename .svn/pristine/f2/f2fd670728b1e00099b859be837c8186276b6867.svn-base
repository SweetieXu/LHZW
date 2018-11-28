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
    public class TerminalTypeBLL
    {
        #region 查询
        public static AsiatekPagedList<TerminalTypeListModel> GetPagedTerminalTypes(TerminalTypeSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","TerminalTypes tt"),
                new SqlParameter("@joinStr","INNER JOIN TerminalManufacturer tm ON tt.TerminalManufacturerID = tm.ID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","tt.ID"),
                new SqlParameter("@showColumns",@"tt.ID ,tt.CommunicationMode,
        tt.TerminalName ,
        tt.ACCOFF_Frequency ,
        tt.ACCON_Frequency ,
        tt.Filter ,
        tm.ManufacturerName"),
            };

            string conditionStr = string.Empty;
            if (!string.IsNullOrWhiteSpace(model.TerminalName))
            {
                conditionStr += "tt.TerminalName LIKE '%" + model.TerminalName + "%'";
            }

            if (model.TerminalManufacturerID != -1)
            {
                if (string.IsNullOrWhiteSpace(conditionStr))
                {
                    conditionStr += "tt.TerminalManufacturerID=" + model.TerminalManufacturerID + "";
                }
                else
                {
                    conditionStr += " AND tt.TerminalManufacturerID=" + model.TerminalManufacturerID + "";
                }
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
            List<TerminalTypeListModel> list = ConvertToList<TerminalTypeListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        /// <summary>
        /// 获取终端类型下拉信息数据
        /// 包含ID、名称
        /// </summary>
        public static List<TerminalTypeDDLModel> GetTerminalTypes()
        {
            string sql = "SELECT ID,TerminalName FROM dbo.TerminalTypes";
            return ConvertToList<TerminalTypeDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        /// <summary>
        /// 根据厂商ID获取终端类型下拉数据
        /// </summary>
        /// <returns></returns>
        public static List<TerminalTypeDDLModel> GetTerminalTypesByTMID(int tmID)
        {
            string sql = "SELECT ID,TerminalName FROM dbo.TerminalTypes WHERE TerminalManufacturerID=@tmID ";
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@tmID", SqlDbType.Int);
            paras[0].Value = tmID;
            return ConvertToList<TerminalTypeDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 删除
        /// <summary>
        /// 根据终端ID批量删除（物理删除）
        /// </summary>
        public static OperationResult DeleteTerminalType(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.TerminalTypes WHERE ID=@ID";
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
        /// 检查终端名是否已存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CheckTerminalNameExists(string name)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NVarChar,200),
            };
            paras[0].Value = name.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.TerminalTypes WHERE TerminalName=@name";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        public static OperationResult AddTerminalType(TerminalTypeAddModel model,int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@TerminalName",SqlDbType.NVarChar,200),
                new SqlParameter("@TerminalManufacturerID",SqlDbType.Int),
                new SqlParameter("@ACCON_Frequency",SqlDbType.Int),
                new SqlParameter("@ACCOFF_Frequency",SqlDbType.Int),
                new SqlParameter("@Filter",SqlDbType.Bit),
                new SqlParameter("@CommunicationMode",SqlDbType.NVarChar,50),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
            };

            paras[0].Value = model.TerminalName.Trim();
            paras[1].Value = model.TerminalManufacturerID;
            paras[2].Value = model.ACCON_Frequency;
            paras[3].Value = model.ACCOFF_Frequency;
            paras[4].Value = model.Filter;

            if (string.IsNullOrWhiteSpace(model.CommunicationMode))
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.CommunicationMode.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[6].Value = DBNull.Value;
            }
            else
            {
                paras[6].Value = model.Remark.Trim();
            }
            paras[7].Value = CreateUserID;
            #region  SQL
            string sql = @"INSERT INTO dbo.TerminalTypes
        ( TerminalName ,
          TerminalManufacturerID ,
          ACCON_Frequency ,
          ACCOFF_Frequency ,
          Filter ,
          CommunicationMode ,
          Remark,
CreateUserID
        )
VALUES  ( @TerminalName , -- TerminalName - nvarchar(20)
          @TerminalManufacturerID , -- TerminalManufacturerID - int
          @ACCON_Frequency , -- ACCON_Frequency - int
          @ACCOFF_Frequency , -- ACCOFF_Frequency - int
          @Filter , -- Filter - bit
          @CommunicationMode , -- CommunicationMode - nvarchar(50)
          @Remark,  -- Remark - nvarchar(50)
          @CreateUserID
        )";
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
        /// <summary>
        /// 检查终端名是否已存在
        /// </summary>
        public static bool CheckTerminalNameExists(string name, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NVarChar,200),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = name.Trim();
            paras[1].Value = id;


            string sql = "SELECT COUNT(0) FROM dbo.TerminalTypes WHERE TerminalName=@name AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        public static SelectResult<TerminalTypeEditModel> GetTerminalTypeByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT * FROM dbo.TerminalTypes WHERE ID=@ID";
            List<TerminalTypeEditModel> list = ConvertToList<TerminalTypeEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            TerminalTypeEditModel data = null;
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
            return new SelectResult<TerminalTypeEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditTerminalType(TerminalTypeEditModel model,int EditUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@TerminalName",SqlDbType.NVarChar,200),
                new SqlParameter("@TerminalManufacturerID",SqlDbType.Int),
                new SqlParameter("@ACCON_Frequency",SqlDbType.Int),
                new SqlParameter("@ACCOFF_Frequency",SqlDbType.Int),
                new SqlParameter("@Filter",SqlDbType.Bit),
                new SqlParameter("@CommunicationMode",SqlDbType.NVarChar,50),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@EditUserID",SqlDbType.Int),
                new SqlParameter("@EditTime",SqlDbType.DateTime)
            };

            paras[0].Value = model.TerminalName.Trim();
            paras[1].Value = model.TerminalManufacturerID;
            paras[2].Value = model.ACCON_Frequency;
            paras[3].Value = model.ACCOFF_Frequency;
            paras[4].Value = model.Filter;

            if (string.IsNullOrWhiteSpace(model.CommunicationMode))
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.CommunicationMode.Trim();
            }

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[6].Value = DBNull.Value;
            }
            else
            {
                paras[6].Value = model.Remark;
            }

            paras[7].Value = model.ID;
            paras[8].Value = EditUserID;
            paras[9].Value = DateTime.Now;
            #region  SQL
            string sql = @"UPDATE  dbo.TerminalTypes
SET     TerminalName = @TerminalName ,
        TerminalManufacturerID = @TerminalManufacturerID ,
        ACCON_Frequency = @ACCON_Frequency ,
        ACCOFF_Frequency = @ACCOFF_Frequency ,
        Filter = @Filter ,
        CommunicationMode = @CommunicationMode ,
        Remark = @Remark,
        EditTime=@EditTime,
EditUserID=@EditUserID
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

        #region 其他
        /// <summary>
        /// 根据终端类型名获取终端类型编码
        /// </summary>
        public static bool TryGetCodeByName(string name, out int code)
        {
            code = -1;
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NVarChar,200),
            };
            paras[0].Value = name;
            string sql = "SELECT ID FROM TerminalTypes WHERE TerminalName=@name";
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

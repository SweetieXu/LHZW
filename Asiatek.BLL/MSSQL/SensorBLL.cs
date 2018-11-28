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
    public class SensorBLL
    {
        #region   查询
        public static AsiatekPagedList<SensorListModels> GetPagedSensor(SensorSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","SensorList s"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","s.TypeID"),
                new SqlParameter("@showColumns",@"s.TypeID ,s.SensorCode,
        s.SensorName,
        s.Value1,
        s.Value2,
        s.Remark,
        s.Status,s.TypeCode"),
            };

            string conditionStr = " Status<>9 ";//不查询删除和报废的
            if (!string.IsNullOrWhiteSpace(model.SensorName))
            {
                conditionStr += " AND s.SensorName LIKE '%" + model.SensorName + "%'";
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
            List<SensorListModels> list = ConvertToList<SensorListModels>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region   新增
        public static OperationResult AddSensorType(SensorAddModel model,int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SensorName",SqlDbType.NVarChar,30),
                new SqlParameter("@TValue",SqlDbType.Bit),
                new SqlParameter("@FValue",SqlDbType.Bit),
                new SqlParameter("@Remark",SqlDbType.NVarChar,200),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                 new SqlParameter("@SensorCode",SqlDbType.Int),
                 new SqlParameter("@TypeCode",SqlDbType.NVarChar)
            };

            paras[0].Value = model.SensorName.Trim();
            paras[1].Value = model.Value1;
            paras[2].Value = model.Value2;

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[3].Value = DBNull.Value;
            }
            else
            {
                paras[3].Value = model.Remark;
            }
            paras[4].Value = CreateUserID;
            paras[5].Value = model.SensorCode;
            paras[6].Value = model.TypeCode.Trim();
            #region  SQL
            string sql;
            #endregion

            sql = @"INSERT INTO dbo.SensorList(SensorName,Value1,Value2,Remark,Status,CreateUserID,SensorCode,TypeCode) VALUES (@SensorName,@TValue,@FValue,@Remark,'0',@CreateUserID,@SensorCode,@TypeCode)";


            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region   编辑
        public static SelectResult<SensorEditModel> GetSensorTypeID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@id",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT * FROM dbo.SensorList WHERE TypeID=@id";
            List<SensorEditModel> list = ConvertToList<SensorEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            SensorEditModel data = null;
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
            return new SelectResult<SensorEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }


        public static OperationResult EditSensorType(SensorEditModel model, int EditUserID)
        {

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@TypeID",SqlDbType.Int),
                //new SqlParameter("@SensorName",SqlDbType.NVarChar,30),
                new SqlParameter("@TValue",SqlDbType.Bit),
                new SqlParameter("@FValue",SqlDbType.Bit),
                new SqlParameter("@Remark",SqlDbType.NVarChar,200),
                new SqlParameter("@EditTime",SqlDbType.DateTime),
                new SqlParameter("@EditUserID",SqlDbType.Int),
            };

            paras[0].Value = model.TypeID;
            paras[1].Value = model.Value1;
            paras[2].Value = model.Value2;


            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[3].Value = DBNull.Value;
            }
            else
            {
                paras[3].Value = model.Remark;
            }
            paras[4].Value = DateTime.Now;
            paras[5].Value = EditUserID;
            #region  SQL
            string sql;

            sql = @"UPDATE  dbo.SensorList
SET     Value1=@TValue,Value2 = @FValue,Remark=@Remark ,EditTime=@EditTime,EditUserID=@EditUserID
WHERE   TypeID = @TypeID";
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

        #region   删除 物理删除
        /// <summary>
        /// 删除（物理删除）
        /// </summary>
        public static OperationResult DeleteSensor(string[] ids)
        { 
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                //sqls[i] = @"UPDATE dbo.SensorList SET [Status]=9 WHERE TypeID=@ID";
                sqls[i] = @"DELETE FROM dbo.SensorList WHERE TypeID=@ID";
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


        #region   验证是否重复
        /// <summary>
        /// 名称 是否重复
        /// </summary>
        public static bool CheckTypeNameExists(string Name)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Name",SqlDbType.NVarChar,20),
            };
            paras[0].Value = Name.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.SensorList WHERE SensorName=@Name";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        public static bool CheckAddSensorCodeExists(string sensorCode)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SensorCode",SqlDbType.NVarChar,20),
            };
            paras[0].Value = sensorCode.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.SensorList WHERE SensorCode=@SensorCode";
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

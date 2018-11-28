using Asiatek.AjaxPager;
using Asiatek.Common;
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
    public class VehicleMaintainBLL
    {
        #region  查询车辆颜色
        public static AsiatekPagedList<VehicleMaintainListModels> GetPagedPlateColors(VehicleMaintainSearchModels model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","PlateColors p"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","p.Code"),
                new SqlParameter("@showColumns",@"p.Code ,p.Name"),
            };

            string conditionStr = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(model.PlateCode))
            {
                conditionStr += " AND p.Code LIKE '%" + model.PlateCode + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.PlateName))
            {
                conditionStr += " AND p.Name LIKE '%" + model.PlateName + "%'";
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
            List<VehicleMaintainListModels> list = ConvertToList<VehicleMaintainListModels>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region   保存颜色数据
        public static OperationResult AddPlateColors(VehicleMaintainAddModel model,int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Code",SqlDbType.TinyInt),
                new SqlParameter("@Name",SqlDbType.NVarChar,30),
                new SqlParameter("@CreateUserID",SqlDbType.Int)
            };

            paras[0].Value = model.PlateCode;
            paras[1].Value = model.PlateColor;
            paras[2].Value = CreateUserID;
            #region  SQL
            string sql = @"INSERT INTO dbo.PlateColors
                           ( Code ,Name,CreateUserID) VALUES  ( @Code , @Name,@CreateUserID)";
            #endregion


            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region   数据验证
        /// <summary>
        /// 编号 是否重复
        /// </summary>
        public static bool CheckPlateCodeExists(string Code)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Code",SqlDbType.VarChar,13),
            };
            paras[0].Value = Code;


            string sql = "SELECT COUNT(0) FROM dbo.PlateColors WHERE Code=@Code";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 颜色 是否重复
        /// </summary>
        public static bool CheckPlateColorsExists(string Colors)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Colors",SqlDbType.NVarChar,30),
            };
            paras[0].Value = Colors;


            string sql = "SELECT COUNT(0) FROM dbo.PlateColors WHERE Name=@Colors";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion






        #region  查询车辆类型
        public static AsiatekPagedList<VehicleTypeListModel> GetPagedVehicleType(VehicleTypeSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","VehicleTypes v"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","v.Code"),
                new SqlParameter("@showColumns",@"v.Code ,v.Name,v.ParentCode"),
            };

            string conditionStr = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(model.VehicleTCode))
            {
                conditionStr += " AND v.Code LIKE '%" + model.VehicleTCode + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.VehicleTName))
            {
                conditionStr += " AND v.Name LIKE '%" + model.VehicleTName + "%'";
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
            List<VehicleTypeListModel> list = ConvertToList<VehicleTypeListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region   保存数据
        public static OperationResult AddVehicleType(VehicleTypeAddModel model,int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Code",SqlDbType.TinyInt),
                new SqlParameter("@Name",SqlDbType.NVarChar,30),
                new SqlParameter("@CreateUserID",SqlDbType.Int)
            };

            paras[0].Value = model.Code;
            paras[1].Value = model.Name.Trim();
            paras[2].Value = CreateUserID;
            #region  SQL
            string sql;
            #endregion

            if (!string.IsNullOrWhiteSpace(model.HigheNumber.ToString()))
            {
                paras.Add(new SqlParameter("@Number", SqlDbType.TinyInt));
                paras[3].Value = model.HigheNumber;

                sql = @"INSERT INTO dbo.VehicleTypes
                           (Code,Name,ParentCode,CreateUserID) VALUES (@Code,@Name,@Number,@CreateUserID)";
            }
            else
            {
                sql = @"INSERT INTO dbo.VehicleTypes
                           (Code,Name,CreateUserID) VALUES (@Code,@Name,@CreateUserID)";
            }


            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region   编辑保存数据
        public static SelectResult<VehicleTypeEditModel> GetVehicleTypeID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Code",SqlDbType.TinyInt),
            };
            paras[0].Value = id;
            string sql = @"SELECT Code ,Name ,CAST(ParentCode AS nvarchar(10))as ParentCode 
FROM    dbo.VehicleTypes 
WHERE   Code=@Code";
            List<VehicleTypeEditModel> list = ConvertToList<VehicleTypeEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            VehicleTypeEditModel data = null;
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
            return new SelectResult<VehicleTypeEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditVehicleType(VehicleTypeEditModel model,int EditUserID)
        {

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Code",SqlDbType.TinyInt),
                new SqlParameter("@Name",SqlDbType.VarChar,13),
                new SqlParameter("@ID",SqlDbType.TinyInt),
                new SqlParameter("@EditTime",SqlDbType.DateTime),
                new SqlParameter("@EditUserID",SqlDbType.Int)
            };

            paras[0].Value = model.Code;
            paras[1].Value = model.Name.Trim();
            paras[2].Value = model.ID;
            paras[3].Value = DateTime.Now;
            paras[4].Value = EditUserID;

            #region  SQL
            string sql;
            #endregion

            if (!string.IsNullOrWhiteSpace(model.ParentCode))
            {
                paras.Add(new SqlParameter("@ParentCode", SqlDbType.TinyInt));
                paras[3].Value = model.ParentCode;

                sql = @"UPDATE  dbo.VehicleTypes
SET     Code = @ID ,Name=@Name,
        ParentCode = @ParentCode,EditUserID=@EditUserID,EditTime=@EditTime
WHERE   Code = @ID";
            }
            else
            {
                sql = @"UPDATE  dbo.VehicleTypes
SET     Code = @ID ,Name=@Name,EditUserID=@EditUserID,EditTime=@EditTime
WHERE   Code = @ID";
            }


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

        #region   联想查询
        public static List<VehiclesTypeCode> GetParentCode(int Code)
        {
            string sql = @"SELECT v.Code,v.Name,v.ParentCode FROM dbo.VehicleTypes v WHERE Code LIKE @Code ";

            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@Code",
                Value = "%" + Code + "%",
            };
            return ConvertToList<VehiclesTypeCode>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region   是否重复
        /// <summary>
        /// 编号 是否重复
        /// </summary>
        public static bool CheckTypeCodeExists(string Code)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Code",SqlDbType.TinyInt),
            };
            paras[0].Value = Code.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.VehicleTypes WHERE Code=@Code";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 名称 是否重复
        /// </summary>
        public static bool CheckTypeNameExists(string Name)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Name",SqlDbType.NVarChar,30),
            };
            paras[0].Value = Name.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.VehicleTypes WHERE Name=@Name";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }


        /// <summary>
        /// 根据编号检查名称是否重复
        /// </summary>
        public static bool CheckCodeNameExists(string Name, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Name",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.TinyInt),
            };
            paras[0].Value = Name.Trim();
            paras[1].Value = id;

            string sql = "SELECT COUNT(0) FROM dbo.VehicleTypes WHERE Name=@Name AND Code<>@ID";
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

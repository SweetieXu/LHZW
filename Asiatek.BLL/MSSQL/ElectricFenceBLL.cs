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
    public class ElectricFenceBLL
    {
//        #region 查询
//        public static AsiatekPagedList<ElectricFenceListModel> GetPagedElectricFences(ElectricFenceSearchModel model, int searchPage, int pageSize,int strucID)
//        {
//            #region 是否属于单位亚士德，是则查所有，否则查询前用户所属单位的非亚士德上级以及所有其子单位
//            string isSql = string.Format(@" SELECT ParentID FROM dbo.Structures WHERE ID ={0} ", strucID);
//            var isResult = MSSQLHelper.ExecuteScalar(CommandType.Text, isSql); 
//            #endregion

//            List<SqlParameter> paras = new List<SqlParameter>()
//            {
//                new SqlParameter("@tableName","dbo.ElectricFence  AS ef "),
//                new SqlParameter("@joinStr",@" INNER JOIN dbo.Users AS us ON us.ID = ef.CreateUser 
//      INNER JOIN dbo.Structures AS st ON st.ID = us.StrucID "),
//                new SqlParameter("@pageSize",pageSize),
//                new SqlParameter("@currentPage",searchPage),
//                new SqlParameter("@orderBy","ef.CreateTime DESC"),
//                new SqlParameter("@showColumns",@"ef.ID
//      ,ef.FenceName
//      ,ef.FenceType
//      ,ef.FenceState
//      ,ef.AlarmType
//,ef.StartTime
//,ef.EndTime
//      ,ef.CreateTime,st.StrucName,st.ID AS StrucID,us.NickName "), 
//            };

//            string conditionStr = "";

//            #region 筛选条件
//            if (isResult != DBNull.Value)
//            {
//                conditionStr = "ef.Status=0 AND StrucID IN (SELECT ID FROM Func_GetAllSubStructsAndParentStructsByStrucID(" + strucID + ") WHERE ParentID IS NOT NULL) ";
//            }
//            else {
//                conditionStr = "ef.Status=0 ";
//            }
            
//            if (!string.IsNullOrWhiteSpace(model.FenceName))
//            {
//                conditionStr += " AND ef.FenceName LIKE '%" + model.FenceName + "%'";
//            }
//            if (model.FenceType != -1)
//            {
//                conditionStr += " AND ef.FenceType=" + model.FenceType + "";
//            }

//            if (model.AlarmType != -1)
//            {
//                conditionStr += " AND ef.AlarmType=" + model.AlarmType + "";
//            }
//            if (!string.IsNullOrEmpty(model.StartTime))
//            {
//                conditionStr += " AND ef.CreateTime>='" + Convert.ToDateTime(model.StartTime) + "'";
//            }
//            if (!string.IsNullOrEmpty(model.EndTime))
//            {
//                conditionStr += " AND ef.CreateTime<'" + Convert.ToDateTime(model.EndTime).AddDays(1) + "'";
//            }
//            if (model.SearchStrucID != -1)
//            {
//                conditionStr += " AND StrucID = " + model.SearchStrucID + "";
//            }

//            if (!string.IsNullOrWhiteSpace(conditionStr))
//            {
//                paras.Add(new SqlParameter("@conditionStr", conditionStr));
//            }
//            #endregion

//            paras.Add(new SqlParameter()
//            {
//                ParameterName = "@totalItemCount",
//                Direction = ParameterDirection.Output,
//                SqlDbType = SqlDbType.Int
//            });
//            paras.Add(new SqlParameter()
//            {
//                ParameterName = "@newCurrentPage",
//                Direction = ParameterDirection.Output,
//                SqlDbType = SqlDbType.Int
//            });
//            var rs = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray());
//            List<ElectricFenceListModel> list = ConvertToList<ElectricFenceListModel>.Convert(rs);
//            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
//            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
//            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
//        }

//        public static List<StructureDDLModel> GetStructuresByStrucID(int strucID, string structName)
//        {
//            string sql = string.Format(@"SELECT ID,StrucName FROM Func_GetAllSubStructsAndParentStructsByStrucID({0}) WHERE ParentID IS NOT NULL AND StrucName LIKE  {1}", strucID, "'%" + structName + "%'");

//            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
//        }
//        #endregion

//        #region 新增
//        public static OperationResult AddElectricFence(AddElectricFenceModel model, int currentUserID)
//        {
//            #region 参数
//            List<SqlParameter> paras = new List<SqlParameter>()
//            {
//                new SqlParameter("@FenceName",SqlDbType.NVarChar),
//                new SqlParameter("@FenceType",SqlDbType.TinyInt),
//                new SqlParameter("@FenceTypeInfo",SqlDbType.VarChar),
//                new SqlParameter("@FenceState",SqlDbType.Bit),
//                new SqlParameter("@AlarmType",SqlDbType.TinyInt),

//                new SqlParameter("@StartTime",SqlDbType.DateTime),
//                new SqlParameter("@EndTime",SqlDbType.DateTime),
//                new SqlParameter("@Status",SqlDbType.TinyInt),
//                new SqlParameter("@CreateUser",SqlDbType.Int),
//                new SqlParameter("@CreateTime",SqlDbType.DateTime),
//            };

//            paras[0].Value = model.FenceName;
//            paras[1].Value = model.FenceType;
//            paras[2].Value = model.FenceTypeInfo;
//            paras[3].Value = model.FenceState;
//            paras[4].Value = model.AlarmType;

//            paras[5].Value = model.StartTime;
//            paras[6].Value = model.EndTime;
//            paras[7].Value = 0;
//            paras[8].Value = currentUserID;
//            paras[9].Value = DateTime.Now;
//            #endregion

//            #region
//            string sql = @"INSERT  INTO dbo.ElectricFence
//                ( [FenceName]
//                ,[FenceType]
//                ,[FenceTypeInfo]
//                ,[FenceState]
//                ,[AlarmType]
//                ,[StartTime]
//                ,[EndTime]
//                ,[Status]
//                ,[CreateUser]
//                ,[CreateTime]
//                )
//        VALUES  ( @FenceName , 
//                    @FenceType, 
//                    @FenceTypeInfo,
//                    @FenceState , 
//                    @AlarmType, 
//                    @StartTime , 
//                    @EndTime, 
//                    @Status , 
//                    @CreateUser, 
//                    @CreateTime 
//                )";
//            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;

//            #endregion

//            return new OperationResult()
//            {
//                Success = result,
//                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
//            };
//        }


//        public static bool CheckAddFenceNameExists(string fenceName, int strucID)
//        {
//            #region 是否属于单位亚士德，是则查所有，否则查询前用户所属单位的非亚士德上级以及所有其子单位
//            string isSql = string.Format(@" SELECT ParentID FROM dbo.Structures WHERE ID ={0} ", strucID);
//            var isResult = MSSQLHelper.ExecuteScalar(CommandType.Text, isSql); 
//            #endregion

//            string sql = "";
//            if (isResult != DBNull.Value)
//            {
//                sql = string.Format(@" SELECT COUNT(0) FROM dbo.ElectricFence AS ef INNER JOIN dbo.Users us ON us.ID = ef.CreateUser  
//  WHERE us.StrucID IN (SELECT ID FROM Func_GetAllSubStructsAndParentStructsByStrucID({0}) WHERE ParentID IS NOT NULL) AND ef.FenceName=@FenceName AND ef.Status<>9 ", strucID);
//            }
//            else
//            {
//                sql = @" SELECT COUNT(0) FROM dbo.ElectricFence WHERE FenceName=@FenceName AND Status<>9 ";
//            }

//            List<SqlParameter> paras = new List<SqlParameter>()
//            {
//                new SqlParameter("@FenceName",SqlDbType.NVarChar,50),
//            };
//            paras[0].Value = fenceName;

//            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
//            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
//            {
//                return true;
//            }
//            return Convert.ToInt32(result) > 0;
//        }
//        #endregion


//        #region  删除
//        public static OperationResult DeleteElectricFence(string[] ids)
//        {
//            int length = ids.Length * 2;
//            string[] sqls = new string[length];
//            SqlParameter[][] paras = new SqlParameter[length][];
//            //删除关联的终端号记录
//            for (int i = 0; i < length; i++)
//            {
//                sqls[i] = @"UPDATE dbo.ElectricFence SET Status=9 WHERE ID=@ID";
//                sqls[i + 1] = @"UPDATE dbo.VehicleElectricFence SET Status=9 WHERE FenceID=@ID";
//                paras[i] = new SqlParameter[1];
//                paras[i + 1] = new SqlParameter[1];
//                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
//                temp.Value = ids[i / 2];
//                paras[i][0] = temp;
//                paras[i + 1][0] = temp;
//                i = i + 1;
//            }
//            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
//            return new OperationResult()
//            {
//                Success = result,
//                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
//            };
//        }
//        #endregion


//        #region  修改
//        public static SelectResult<EditElectricFenceModel> GetElectricFenceByID(int id)
//        {
//            List<SqlParameter> paras = new List<SqlParameter>()
//            {
//                new SqlParameter()
//                {
//                    ParameterName="@ID",
//                    SqlDbType=SqlDbType.Int,
//                },
//            };
//            paras[0].Value = id;
//            string sql = @"SELECT ID
//      ,FenceName
//      ,FenceType
//,FenceTypeInfo
//      ,FenceState
//      ,AlarmType
//,StartTime
//,EndTime
//      ,CreateTime 
//  FROM dbo.ElectricFence
//  WHERE Status=0 AND ID=@ID";

//            List<EditElectricFenceModel> list = ConvertToList<EditElectricFenceModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

//            EditElectricFenceModel data = null;
//            string msg = string.Empty;

//            if (list == null)
//            {
//                msg = PromptInformation.DBError;
//            }
//            else if (list.Count == 0)
//            {
//                msg = PromptInformation.NotExists;
//            }
//            else
//            {
//                data = list[0];
//            }
//            return new SelectResult<EditElectricFenceModel>()
//            {
//                DataResult = data,
//                Message = msg
//            };
//        }

//        public static bool CheckEditFenceNameExists(string fenceName, int id, int strucID)
//        {
//            #region 是否属于单位亚士德，是则查所有，否则查询前用户所属单位的非亚士德上级以及所有其子单位
//            string isSql = string.Format(@" SELECT ParentID FROM dbo.Structures WHERE ID ={0} ", strucID);
//            var isResult = MSSQLHelper.ExecuteScalar(CommandType.Text, isSql);
//            #endregion

//            string sql = "";
//            if (isResult != DBNull.Value)
//            {
//                sql = string.Format(@"SELECT COUNT(0) FROM dbo.ElectricFence AS ef INNER JOIN dbo.Users us ON us.ID = ef.CreateUser  
//  WHERE us.StrucID IN (SELECT ID FROM Func_GetAllSubStructsAndParentStructsByStrucID({0}) WHERE ParentID IS NOT NULL) AND ef.FenceName=@FenceName AND ef.ID!=@ID AND ef.Status<>9", strucID);
//            }
//            else
//            {
//                sql = @" SELECT COUNT(0) FROM dbo.ElectricFence WHERE FenceName=@FenceName AND ID!=@ID AND Status<>9 ";
//            }

//            List<SqlParameter> paras = new List<SqlParameter>()
//            {
//                new SqlParameter("@FenceName",SqlDbType.NVarChar,50),
//                new SqlParameter("@ID",SqlDbType.Int),
//            };
//            paras[0].Value = fenceName;
//            paras[1].Value = id;

//            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
//            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
//            {
//                return true;
//            }
//            return Convert.ToInt32(result) > 0;
//        }

//        public static OperationResult EditElectricFence(EditElectricFenceModel model, int currentUserID)
//        {
//            List<SqlParameter> paras = new List<SqlParameter>()
//            {
//                new SqlParameter("@FenceName",SqlDbType.NVarChar,50),
//                new SqlParameter("@FenceType",SqlDbType.TinyInt),
//                new SqlParameter("@FenceTypeInfo",SqlDbType.VarChar),
//                new SqlParameter("@FenceState",SqlDbType.Bit),
//                new SqlParameter("@AlarmType",SqlDbType.TinyInt),

//                new SqlParameter("@StartTime",SqlDbType.DateTime),
//                new SqlParameter("@EndTime",SqlDbType.DateTime),
//                new SqlParameter("@UpdateUser",SqlDbType.Int),
//                new SqlParameter("@UpdateTime",SqlDbType.DateTime),
//                new SqlParameter("@ID",SqlDbType.Int),
//            };

//            paras[0].Value = model.FenceName;
//            paras[1].Value = model.FenceType;
//            paras[2].Value = model.FenceTypeInfo;
//            paras[3].Value = model.FenceState;
//            paras[4].Value = model.AlarmType;

//            paras[5].Value = model.StartTime;
//            paras[6].Value = model.EndTime;
//            paras[7].Value = currentUserID;
//            paras[8].Value = DateTime.Now;
//            paras[9].Value = model.ID;

//            #region  SQL
//            string sql = @"UPDATE   dbo.ElectricFence
//       SET      FenceName = @FenceName ,
//                FenceType = @FenceType ,
//                FenceTypeInfo = @FenceTypeInfo ,
//                FenceState = @FenceState ,
//                AlarmType = @AlarmType ,
//                StartTime = @StartTime ,
//                EndTime = @EndTime ,
//                UpdateUser = @UpdateUser ,
//                UpdateTime = @UpdateTime
//       WHERE    ID = @ID";
//            #endregion


//            int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
//            string msg = string.Empty;
//            switch (result)
//            {
//                case 1:
//                    msg = PromptInformation.OperationSuccess;
//                    break;
//                case 0:
//                    msg = PromptInformation.NotExists;
//                    break;
//                case -1:
//                    msg = PromptInformation.DBError;
//                    break;
//            }
//            return new OperationResult()
//            {
//                Success = result > 0,
//                Message = msg
//            };
//        }
//        #endregion


        //电子围栏绑定车辆

        #region 查询
        public static AsiatekPagedList<ElectricFenceListModel> GetPagedElectricFences(ElectricFenceSearchModel model, int searchPage, int pageSize, int strucID)
        {
            #region 是否属于单位亚士德，是则查所有，否则查询前用户所属单位的非亚士德上级以及所有其子单位
            string isSql = @" SELECT ParentID FROM dbo.Structures WHERE ID =@StrucID ";
            List<SqlParameter> isParas = new List<SqlParameter>()
            {
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            isParas[0].Value = strucID;
            var isResult = MSSQLHelper.ExecuteScalar(CommandType.Text, isSql, isParas.ToArray());
            #endregion

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.ElectricFence  AS ef "),
                new SqlParameter("@joinStr",@" INNER JOIN dbo.Users AS us ON us.ID = ef.CreateUser 
      INNER JOIN dbo.Structures AS st ON st.ID = us.StrucID  LEFT JOIN dbo.ElectricFenceProperty AS efp ON efp.ID = ef.PropertyID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","ef.CreateTime DESC"),
                new SqlParameter("@showColumns",@"ef.ID ,ef.FenceName ,ef.FenceType,ef.CustomerStatus,ef.FenceCode
       ,ef.CreateTime,st.StrucName,st.ID AS StrucID,us.NickName,efp.FenceState,ef.PropertyID
      ,efp.ValidStartTime ,efp.ValidEndTime,efp.PropertyName "), 
            };

            string conditionStr = "";

            #region 筛选条件
            if (isResult != DBNull.Value)
            {
                conditionStr = "ef.Status=0 AND us.StrucID IN (SELECT ID FROM Func_GetAllSubStructsAndParentStructsByStrucID(" + strucID + ") WHERE ParentID IS NOT NULL) ";
            }
            else
            {
                conditionStr = "ef.Status=0 ";
            }
            if (!string.IsNullOrWhiteSpace(model.FenceName))
            {
                conditionStr += " AND ef.FenceName LIKE '%" + model.FenceName + "%'";
            }
            if (model.FenceType != -1)
            {
                conditionStr += " AND ef.FenceType=" + model.FenceType + "";
            }
            if (model.SearchStrucID != -1)
            {
                conditionStr += " AND StrucID = " + model.SearchStrucID + "";
            }
            if (!string.IsNullOrWhiteSpace(conditionStr))
            {
                paras.Add(new SqlParameter("@conditionStr", conditionStr));
            }
            #endregion

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
            var rs = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray());
            List<ElectricFenceListModel> list = ConvertToList<ElectricFenceListModel>.Convert(rs);
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        public static List<StructureDDLModel> GetStructuresByStrucID(int strucID, string structName)
        {
            //是南京亚士德则显示南京亚士德，否则询前用户所属单位的非亚士德上级以及所有其子单位
            string isSql = string.Format(@" SELECT ParentID FROM dbo.Structures WHERE ID ={0} ", strucID);
            var isResult = MSSQLHelper.ExecuteScalar(CommandType.Text, isSql);
            string sql = "";
            if (isResult == DBNull.Value)
            {
                sql = string.Format(@"SELECT ID,StrucName FROM Func_GetAllSubStructsAndParentStructsByStrucID({0}) WHERE StrucName LIKE  {1}", strucID, "'%" + structName + "%'");
            }
            else {
                sql = string.Format(@"SELECT ID,StrucName FROM Func_GetAllSubStructsAndParentStructsByStrucID({0}) WHERE ParentID IS NOT NULL AND StrucName LIKE  {1}", strucID, "'%" + structName + "%'");
            }
            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion

        #region 新增
        public static OperationResult AddElectricFence(AddElectricFenceModel model, int currentUserID)
        {
            string _FenceTypeInfo = MGJH_TransportPointBLL.ChangeCoordinateSystem(model.FenceType, model.FenceTypeInfo, 1); //地图坐标转车机坐标,存入数据库
            #region 参数
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FenceName",SqlDbType.NVarChar),
                new SqlParameter("@FenceType",SqlDbType.TinyInt),
                new SqlParameter("@FenceTypeInfo",SqlDbType.VarChar),
                new SqlParameter("@Status",SqlDbType.TinyInt),

                new SqlParameter("@CreateUser",SqlDbType.Int),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
                new SqlParameter("@PropertyID",SqlDbType.Int),
                new SqlParameter("@FenceCode",SqlDbType.NVarChar),
            };

            paras[0].Value = model.FenceName.Trim();
            paras[1].Value = model.FenceType;
            paras[2].Value = _FenceTypeInfo;
            paras[3].Value = 0;

            paras[4].Value = currentUserID;
            paras[5].Value = DateTime.Now;
            paras[6].Value = model.PropertyID;
            if (model.FenceCode == null)
            {
                paras[7].Value = DBNull.Value;
            }
            else
            {
                paras[7].Value = model.FenceCode.Trim();
            }
            #endregion

            #region
            string sql = @"INSERT  INTO dbo.ElectricFence
                ( [FenceName]
                ,[FenceType]
                ,[FenceTypeInfo]
                ,[Status]
                ,[CreateUser]
                ,[CreateTime]
                ,[PropertyID]
                ,[FenceCode]
                )
        VALUES  ( @FenceName , 
                    @FenceType, 
                    @FenceTypeInfo,
                    @Status , 
                    @CreateUser, 
                    @CreateTime,
                    @PropertyID,
                    @FenceCode
                )";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;

            #endregion

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }


        public static bool CheckAddFenceNameExists(string fenceName, int strucID)
        {
//            #region 是否属于单位亚士德，是则查所有，否则查询前用户所属单位的非亚士德上级以及所有其子单位
//            string isSql = string.Format(@" SELECT ParentID FROM dbo.Structures WHERE ID ={0} ", strucID);
//            var isResult = MSSQLHelper.ExecuteScalar(CommandType.Text, isSql);
//            #endregion

//            string sql = "";
//            if (isResult != DBNull.Value)
//            {
//                sql = string.Format(@" SELECT COUNT(0) FROM dbo.ElectricFence AS ef INNER JOIN dbo.Users us ON us.ID = ef.CreateUser  
//  WHERE us.StrucID IN (SELECT ID FROM Func_GetAllSubStructsAndParentStructsByStrucID({0}) WHERE ParentID IS NOT NULL) AND ef.FenceName=@FenceName AND ef.Status<>9 ", strucID);
//            }
//            else
//            {
               string sql = @" SELECT COUNT(0) FROM dbo.ElectricFence WHERE FenceName=@FenceName AND Status<>9 ";
            //}

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FenceName",SqlDbType.NVarChar,50),
            };
            paras[0].Value = fenceName.Trim();

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }


        public static bool CheckAddFenceCodeExists(string fenceCode)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.ElectricFence WHERE FenceCode=@FenceCode AND Status<>9 ";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FenceCode",SqlDbType.NVarChar),
            };
            paras[0].Value = fenceCode.Trim();

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        

        public static List<EFPropertyModel> GetPropertyNames(int strucID)
        {
            #region 是否属于单位亚士德，是则查所有，否则查询前用户所属单位的非亚士德上级以及所有其子单位
            string isSql = string.Format(@" SELECT ParentID FROM dbo.Structures WHERE ID ={0} ", strucID);
            var isResult = MSSQLHelper.ExecuteScalar(CommandType.Text, isSql);
            #endregion

            string sql = "";
            if (isResult != DBNull.Value)
            {
                sql = string.Format(@" SELECT efp.ID AS PropertyID,efp.PropertyName FROM dbo.ElectricFenceProperty AS efp INNER JOIN dbo.Users us ON us.ID = efp.CreateUser  
  WHERE us.StrucID IN (SELECT ID FROM Func_GetAllSubStructsAndParentStructsByStrucID({0}) WHERE ParentID IS NOT NULL)  AND efp.Status<>9 ", strucID);
            }
            else
            {
                sql = @" SELECT ID AS PropertyID,PropertyName FROM dbo.ElectricFenceProperty WHERE Status<>9 ";
            }
            return ConvertToList<EFPropertyModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        //南钢嘉华
        public static OperationResult NGJH_AddElectricFence(NGJH_AddElectricFenceModel model, int currentUserID)
        {
            string _FenceTypeInfo = MGJH_TransportPointBLL.ChangeCoordinateSystem(model.FenceType, model.FenceTypeInfo, 1); //地图坐标转车机坐标,存入数据库
            #region 参数
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FenceName",SqlDbType.NVarChar),
                new SqlParameter("@FenceType",SqlDbType.TinyInt),
                new SqlParameter("@FenceTypeInfo",SqlDbType.VarChar),
                new SqlParameter("@Status",SqlDbType.TinyInt),

                new SqlParameter("@CreateUser",SqlDbType.Int),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
                new SqlParameter("@PropertyID",SqlDbType.Int),
                new SqlParameter("@CustomerStatus",SqlDbType.Int),
            };

            paras[0].Value = model.FenceName.Trim();
            paras[1].Value = model.FenceType;
            paras[2].Value = _FenceTypeInfo;
            paras[3].Value = 0;

            paras[4].Value = currentUserID;
            paras[5].Value = DateTime.Now;
            paras[6].Value = DBNull.Value;
            paras[7].Value = model.CustomerStatus;
            #endregion

            #region
            string sql = @"INSERT  INTO dbo.ElectricFence
                ( [FenceName]
                ,[FenceType]
                ,[FenceTypeInfo]
                ,[Status]
                ,[CreateUser]
                ,[CreateTime]
                ,[PropertyID]
                ,[CustomerStatus]
                )
        VALUES  ( @FenceName , 
                    @FenceType, 
                    @FenceTypeInfo,
                    @Status , 
                    @CreateUser, 
                    @CreateTime,
                    @PropertyID,
                    @CustomerStatus
                )";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;

            #endregion

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }


        #endregion


        //#region  删除  逻辑删除
        //public static OperationResult DeleteElectricFence(string[] ids)
        //{
        //    int length = ids.Length * 2;
        //    string[] sqls = new string[length];
        //    SqlParameter[][] paras = new SqlParameter[length][];
        //    //删除关联的终端号记录
        //    for (int i = 0; i < length; i++)
        //    {
        //        sqls[i] = @"UPDATE dbo.ElectricFence SET Status=9 WHERE ID=@ID";
        //        sqls[i + 1] = @"UPDATE dbo.VehicleElectricFence SET Status=9 WHERE FenceID=@ID";
        //        paras[i] = new SqlParameter[1];
        //        paras[i + 1] = new SqlParameter[1];
        //        SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
        //        temp.Value = ids[i / 2];
        //        paras[i][0] = temp;
        //        paras[i + 1][0] = temp;
        //        i = i + 1;
        //    }
        //    bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
        //    return new OperationResult()
        //    {
        //        Success = result,
        //        Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
        //    };
        //}
        //#endregion

        #region  删除  物理删除
        public static OperationResult DeleteElectricFence(string[] ids)
        {
            int length = ids.Length * 2;
            string[] sqls = new string[length];
            SqlParameter[][] paras = new SqlParameter[length][];
            //删除关联的终端号记录
            for (int i = 0; i < length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.VehicleElectricFence WHERE FenceID=@ID";
                sqls[i + 1] = @"DELETE FROM dbo.ElectricFence WHERE ID=@ID";
                paras[i] = new SqlParameter[1];
                paras[i + 1] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i / 2];
                paras[i][0] = temp;
                paras[i + 1][0] = temp;
                i = i + 1;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion

        #region  修改
        public static SelectResult<EditElectricFenceModel> GetElectricFenceByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@ID",
                    SqlDbType=SqlDbType.Int,
                },
            };
            paras[0].Value = id;
            string sql = @"SELECT ID
      ,FenceName
      ,FenceType
,FenceTypeInfo
      ,CreateTime
,PropertyID,FenceCode 
  FROM dbo.ElectricFence
  WHERE Status=0 AND ID=@ID";

            List<EditElectricFenceModel> list = ConvertToList<EditElectricFenceModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditElectricFenceModel data = null;
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
                data.FenceTypeInfo = MGJH_TransportPointBLL.ChangeCoordinateSystem(data.FenceType, data.FenceTypeInfo, 2); //将取出的车机坐标转成地图坐标显示
            }
            return new SelectResult<EditElectricFenceModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static bool CheckEditFenceNameExists(string fenceName, int id, int strucID)
        {
//            #region 是否属于单位亚士德，是则查所有，否则查询前用户所属单位的非亚士德上级以及所有其子单位
//            string isSql = string.Format(@" SELECT ParentID FROM dbo.Structures WHERE ID ={0} ", strucID);
//            var isResult = MSSQLHelper.ExecuteScalar(CommandType.Text, isSql);
//            #endregion

//            string sql = "";
//            if (isResult != DBNull.Value)
//            {
//                sql = string.Format(@"SELECT COUNT(0) FROM dbo.ElectricFence AS ef INNER JOIN dbo.Users us ON us.ID = ef.CreateUser  
//  WHERE us.StrucID IN (SELECT ID FROM Func_GetAllSubStructsAndParentStructsByStrucID({0}) WHERE ParentID IS NOT NULL) AND ef.FenceName=@FenceName AND ef.ID!=@ID AND ef.Status<>9", strucID);
//            }
//            else
//            {
//               sql = @" SELECT COUNT(0) FROM dbo.ElectricFence AS ef INNER JOIN dbo.Users AS us ON us.ID = ef.CreateUser
//  INNER JOIN dbo.Structures AS st ON st.ID = us.StrucID WHERE st.ParentID IS NULL  AND FenceName=@FenceName AND ef.ID!=@ID AND ef.Status<>9 ";
            //}

            string sql = @" SELECT COUNT(0) FROM dbo.ElectricFence  WHERE  FenceName=@FenceName AND ID!=@ID AND Status<>9 ";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FenceName",SqlDbType.NVarChar,50),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = fenceName.Trim();
            paras[1].Value = id;

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        public static bool CheckEditFenceCodeExists(string fenceCode, int id)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.ElectricFence  WHERE  FenceCode=@FenceCode AND ID!=@ID AND Status<>9 ";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FenceCode",SqlDbType.NVarChar,50),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = fenceCode.Trim();
            paras[1].Value = id;

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        public static OperationResult EditElectricFence(EditElectricFenceModel model, int currentUserID)
        {
            string _FenceTypeInfo = MGJH_TransportPointBLL.ChangeCoordinateSystem(model.FenceType, model.FenceTypeInfo, 1); //地图坐标转车机坐标,存入数据库
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FenceName",SqlDbType.NVarChar,50),
                new SqlParameter("@FenceType",SqlDbType.TinyInt),
                new SqlParameter("@FenceTypeInfo",SqlDbType.VarChar),
                new SqlParameter("@UpdateUser",SqlDbType.Int),
                new SqlParameter("@UpdateTime",SqlDbType.DateTime),
                new SqlParameter("@PropertyID",SqlDbType.Int),
                new SqlParameter("@FenceCode",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int),
            };

            paras[0].Value = model.FenceName.Trim();
            paras[1].Value = model.FenceType;
            paras[2].Value = _FenceTypeInfo;
            paras[3].Value = currentUserID;
            paras[4].Value = DateTime.Now;
            paras[5].Value = model.PropertyID;
            if (model.FenceCode == null)
            {
                paras[6].Value = DBNull.Value;
            }
            else
            {
                paras[6].Value = model.FenceCode.Trim();
            }
            paras[7].Value = model.ID;

            #region  SQL
            string sql = @"UPDATE   dbo.ElectricFence
       SET      FenceName = @FenceName ,
                FenceType = @FenceType ,
                FenceTypeInfo = @FenceTypeInfo ,
                UpdateUser = @UpdateUser ,
                UpdateTime = @UpdateTime,
                PropertyID = @PropertyID,
FenceCode = @FenceCode
       WHERE    ID = @ID";
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

        //南钢嘉华
        public static SelectResult<NGJH_EditElectricFenceModel> GetNGJH_ElectricFenceByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@ID",
                    SqlDbType=SqlDbType.Int,
                },
            };
            paras[0].Value = id;
            string sql = @"SELECT ID
      ,FenceName
      ,FenceType
,FenceTypeInfo
      ,CreateTime
,CustomerStatus 
  FROM dbo.ElectricFence
  WHERE Status=0 AND ID=@ID";

            List<NGJH_EditElectricFenceModel> list = ConvertToList<NGJH_EditElectricFenceModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            NGJH_EditElectricFenceModel data = null;
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
                data.FenceTypeInfo = MGJH_TransportPointBLL.ChangeCoordinateSystem(data.FenceType, data.FenceTypeInfo, 2); //将取出的车机坐标转成地图坐标显示
            }
            return new SelectResult<NGJH_EditElectricFenceModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult NGJH_EditElectricFence(NGJH_EditElectricFenceModel model, int currentUserID)
        {
            string _FenceTypeInfo = MGJH_TransportPointBLL.ChangeCoordinateSystem(model.FenceType, model.FenceTypeInfo, 1); //地图坐标转车机坐标,存入数据库
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FenceName",SqlDbType.NVarChar,50),
                new SqlParameter("@FenceType",SqlDbType.TinyInt),
                new SqlParameter("@FenceTypeInfo",SqlDbType.VarChar),
                new SqlParameter("@UpdateUser",SqlDbType.Int),
                new SqlParameter("@UpdateTime",SqlDbType.DateTime),
                new SqlParameter("@CustomerStatus",SqlDbType.Int),
                new SqlParameter("@ID",SqlDbType.Int),
            };

            paras[0].Value = model.FenceName.Trim();
            paras[1].Value = model.FenceType;
            paras[2].Value = _FenceTypeInfo;
            paras[3].Value = currentUserID;
            paras[4].Value = DateTime.Now;
            paras[5].Value = model.CustomerStatus;
            paras[6].Value = model.ID;

            #region  SQL
            string sql = @"UPDATE   dbo.ElectricFence
       SET      FenceName = @FenceName ,
                FenceType = @FenceType ,
                FenceTypeInfo = @FenceTypeInfo ,
                UpdateUser = @UpdateUser ,
                UpdateTime = @UpdateTime,
                CustomerStatus = @CustomerStatus
       WHERE    ID = @ID";
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

        #region  绑定车辆
        /// <summary>
        /// 获取用户分配的车辆信息   获取其自己和自己子单位的所有车辆
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="searchPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static AsiatekPagedList<EFVehicleListModel> GetPagedCustomerVehicle(int id, EFVehicleSearchModel model, int searchPage, int pageSize, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.Vehicles ve "),
                new SqlParameter("@joinStr",@"INNER JOIN dbo.Structures st ON ve.StrucID = st.ID 
INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID(" + strucID + ")  AS vt ON ve.ID=vt.VID "),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","ve.ID DESC"),
                new SqlParameter("@showColumns",@"ve.ID AS VehicleID,ve.PlateNum,ve.VIN,ve.VehicleName,ve.StrucID,st.StrucName "),
            };

            StringBuilder sbWhere = new StringBuilder("ve.Status<>9 AND st.Status<>9 AND ve.ID NOT IN (SELECT VehicleID FROM dbo.VehicleElectricFence WHERE FenceID=" + id + " AND Status<>9)");

            if (!string.IsNullOrWhiteSpace(model.VIN))
            {
                sbWhere.Append(" AND ve.VIN LIKE '%" + model.VIN + "%' ");
            }
            if (!string.IsNullOrWhiteSpace(model.VehicleName))
            {
                sbWhere.Append(" AND ve.VehicleName LIKE '%" + model.VehicleName + "%'");
            }

            if (!string.IsNullOrEmpty(sbWhere.ToString()))
            {
                paras.Add(new SqlParameter("@conditionStr", sbWhere.ToString()));
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
            List<EFVehicleListModel> list = ConvertToList<EFVehicleListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        public static OperationResult AddVehicleToElectricFence(int fenceID, long vehicleID, int uID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@FenceID",SqlDbType.Int),
                new SqlParameter("@VehicleID",SqlDbType.BigInt),
                new SqlParameter("@Status",SqlDbType.Bit),
                new SqlParameter("@CreateUser",SqlDbType.Int),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
            };

            paras[0].Value = fenceID;
            paras[1].Value = vehicleID;
            paras[2].Value = 0;
            paras[3].Value = uID;
            paras[4].Value = DateTime.Now;

            string sql = @"INSERT  INTO dbo.VehicleElectricFence
                ([FenceID]
                ,[VehicleID]
                ,[Status]
                ,[CreateUser]
                ,[CreateTime]
                )
        VALUES  ( @FenceID, 
                    @VehicleID,
                    @Status , 
                    @CreateUser, 
                    @CreateTime  
                )";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region  解绑终端

        public static List<ElectricFenceVehicleBindListModel> GetVehicleByFenceID(int id)
        {
            string sql = string.Format(@" SELECT vef.FenceID,vef.CreateTime,ef.FenceName,ef.FenceType,ef.FenceTypeInfo,
  ve.VIN,ve.VehicleName,ve.ID AS VehicleID,ve.PlateNum 
  FROM dbo.VehicleElectricFence vef 
  INNER JOIN dbo.ElectricFence ef ON vef.FenceID=ef.ID 
  LEFT JOIN dbo.Vehicles ve ON ve.ID = vef.VehicleID
  WHERE vef.Status<>9 AND ef.Status<>9 AND ve.Status<>9 AND vef.FenceID={0} 
  ORDER BY vef.CreateTime DESC ", id);

            return ConvertToList<ElectricFenceVehicleBindListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }


        public static OperationResult DelVehicleFromElectricFence(long vehicleID, int fenceID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleID",SqlDbType.BigInt),
                new SqlParameter("@FenceID",SqlDbType.Int)
            };

            paras[0].Value = vehicleID;
            paras[1].Value = fenceID;

            string sql = @"DELETE FROM dbo.VehicleElectricFence WHERE VehicleID=@VehicleID AND FenceID=@FenceID";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region  查询电子围栏
        //查询车辆绑定的电子围栏
        public static List<ElectricFenceListModel> GetElectricFenceByVehicleID(long vehicleID)
        {
            string sql = @"  SELECT ef.ID,ef.FenceName,ef.FenceType,ef.FenceTypeInfo,efp.FenceState,efp.AlarmType,ef.CreateTime,efp.ValidStartTime,efp.ValidEndTime,vef.VehicleID,ve.VehicleName
  FROM dbo.VehicleElectricFence vef 
  INNER JOIN dbo.ElectricFence ef ON vef.FenceID=ef.ID  
  INNER JOIN dbo.ElectricFenceProperty efp ON efp.ID=ef.PropertyID
INNER JOIN dbo.Vehicles ve ON vef.VehicleID=ve.ID
  WHERE vef.VehicleID=@vehicleID AND vef.Status<>9 AND ef.Status<>9 AND ve.Status<>9";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@vehicleID",SqlDbType.BigInt)
            };
            paras[0].Value = vehicleID;
            return ConvertToList<ElectricFenceListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

        //实时监控  查询用户权限范围内的电子围栏
        public static List<EFMarkerListModel> GetEFMarkersInfoBySID(int strucID)
        {
            #region 是否属于单位亚士德，是则查所有，否则查询前用户所属单位的非亚士德上级以及所有其子单位
            string isSql = @" SELECT ParentID FROM dbo.Structures WHERE ID =@StrucID ";
            List<SqlParameter> isParas = new List<SqlParameter>()
            {
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            isParas[0].Value = strucID;
            var isResult = MSSQLHelper.ExecuteScalar(CommandType.Text, isSql, isParas.ToArray());
            #endregion

            string sql = @" SELECT ef.ID,ef.FenceName,ef.FenceType,ef.FenceTypeInfo,ef.CustomerStatus,us.StrucID
  FROM dbo.ElectricFence ef 
  INNER JOIN dbo.Users AS us ON us.ID = ef.CreateUser 
      INNER JOIN dbo.Structures AS st ON st.ID = us.StrucID  
      LEFT JOIN dbo.ElectricFenceProperty AS efp ON efp.ID = ef.PropertyID
  WHERE ((efp.ID IS NOT NULL AND efp.FenceState=1 AND efp.ValidEndTime>=GETDATE()) OR (efp.ID IS NULL)) ";

            if (isResult != DBNull.Value)
            {
                sql += "AND us.StrucID IN (SELECT ID FROM Func_GetAllSubStructsAndParentStructsByStrucID(" + strucID + ") WHERE ParentID IS NOT NULL) ";
            }

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@StrucID",SqlDbType.Int)
            };
            paras[0].Value = strucID;
            return ConvertToList<EFMarkerListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion

        #region 查询电子围栏异常
        #region 默认模式
        /// <summary>
        /// 电子围栏 异常查询（ 默认模式）
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="fenceID"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static List<ElectricFenceExceptionModel> GetDefaultElectricFenceExInfo(long vehicleID, int fenceID, int strucID)
        {
            #region 获取服务器名
            List<ServerInfoModel> linkName_list = ReportBLL.GetDefaultServerInfo(strucID, vehicleID);
            if (linkName_list == null)
            {
                return null;
            }
            #endregion

            string sql = "";
            string linkedServerName = "";
            for (int i = 0; i < linkName_list.Count; i++)
            {
                linkedServerName = linkName_list[i].LinkedServerName;
                sql += string.Format(@"SELECT ef.ID AS FenceID,ef.FenceName,
efe.ExceptionType,CONVERT(varchar(100), efe.SignalStartTime, 20) AS SignalStartTime,CONVERT(varchar(100), efe.SignalEndTime, 20) AS SignalEndTime,efe.StartAddress,efe.EndAddress,efe.ID AS ExID 
  FROM {0}.GNSS.dbo.ElectricFenceException efe 
  INNER JOIN dbo.Terminals tt ON tt.TerminalCode=efe.TerminalCode 
  INNER JOIN dbo.ServerInfo sv ON sv.ID = tt.ServerInfoID 
  INNER JOIN dbo.ElectricFence ef ON ef.ID=efe.FenceID
  INNER JOIN dbo.VehicleElectricFence vef ON vef.VehicleID=tt.LinkedVehicleID
  WHERE sv.LinkedServerName='{0}' AND vef.VehicleID={1} AND ef.ID={2} AND vef.FenceID={2} AND vef.Status<>9 AND ef.Status<>9 AND tt.Status<>9", linkedServerName, vehicleID, fenceID);
                if (i != linkName_list.Count - 1)
                {
                    sql += "  UNION  ";
                }
            }
            return ConvertToList<ElectricFenceExceptionModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion


        #region 自由模式
        /// <summary>
        /// 电子围栏 异常查询（ 自由模式）
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <param name="fenceID"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static List<ElectricFenceExceptionModel> GetElectricFenceExInfo(long vehicleID, int fenceID, int userID)
        {
            #region 获取服务器名
            List<ServerInfoModel> linkName_list = ReportBLL.GetServerInfo(userID, vehicleID);
            if (linkName_list == null)
            {
                return null;
            }
            #endregion

            string sql = "";
            string linkedServerName = "";
            for (int i = 0; i < linkName_list.Count; i++)
            {
                linkedServerName = linkName_list[i].LinkedServerName;
                sql += string.Format(@"SELECT ef.ID AS FenceID,ef.FenceName,
efe.ExceptionType,CONVERT(varchar(100), efe.SignalStartTime, 20) AS SignalStartTime,CONVERT(varchar(100), efe.SignalEndTime, 20) AS SignalEndTime,efe.StartAddress,efe.EndAddress,efe.ID AS ExID 
  FROM {0}.GNSS.dbo.ElectricFenceException efe 
  INNER JOIN dbo.Terminals tt ON tt.TerminalCode=efe.TerminalCode 
  INNER JOIN dbo.ServerInfo sv ON sv.ID = tt.ServerInfoID 
  INNER JOIN dbo.ElectricFence ef ON ef.ID=efe.FenceID
  INNER JOIN dbo.VehicleElectricFence vef ON vef.VehicleID=tt.LinkedVehicleID
  WHERE sv.LinkedServerName='{0}' AND vef.VehicleID={1} AND ef.ID={2} AND vef.FenceID={2} AND vef.Status<>9 AND ef.Status<>9 AND tt.Status<>9", linkedServerName, vehicleID, fenceID);
                if (i != linkName_list.Count - 1)
                {
                    sql += "  UNION  ";
                }
            }
            var rs = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql);
            var List = ConvertToList<ElectricFenceExceptionModel>.Convert(rs);
            return List;
        }
        #endregion

        //        #region 自由模式
        //        /// <summary>
        //        /// 电子围栏 异常查询（ 自由模式）
        //        /// </summary>
        //        /// <param name="vehicleID"></param>
        //        /// <param name="fenceID"></param>
        //        /// <param name="strucID"></param>
        //        /// <returns></returns>
        //        public static List<ElectricFenceExceptionModel> GetElectricFenceExInfo(long vehicleID, int fenceID, int userID)
        //        {
        //            #region 获取服务器名
        //            List<ServerInfoModel> linkName_list = ReportBLL.GetServerInfo(userID, vehicleID);
        //            //            string linkName_sql = @" SELECT TOP(1) sv.LinkedServerName FROM dbo.Vehicles ve 
        //            //  INNER JOIN dbo.Terminals tt ON tt.LinkedVehicleID = ve.ID 
        //            //  INNER JOIN dbo.ServerInfo sv ON sv.ID = tt.ServerInfoID 
        //            //  WHERE ve.Status<>9 AND ve.ID=@VehicleID ";
        //            //            List<SqlParameter> linkName_paras = new List<SqlParameter>()
        //            //            {
        //            //                new SqlParameter("@VehicleID",SqlDbType.BigInt)
        //            //            };？？？
        //            //            linkName_paras[0].Value = vehicleID;
        //            //            var linkName_list = MSSQLHelper.ExecuteScalar(CommandType.Text, linkName_sql, linkName_paras.ToArray());
        //            if (linkName_list == null)
        //            {
        //                return null;
        //            }
        //            #endregion

        //            string linkedServerName = linkName_list.ToString();
        //            string sql = string.Format(@"SELECT ef.ID AS FenceID,ef.FenceName,ef.FenceType,ef.FenceTypeInfo,
        //efe.ExceptionType,CONVERT(varchar(100), efe.SignalStartTime, 20) AS SignalStartTime,CONVERT(varchar(100), efe.SignalEndTime, 20) AS SignalEndTime,efe.StartAddress,efe.EndAddress,efe.ID AS ExID 
        //  FROM {0}.GNSS.dbo.ElectricFenceException efe 
        //  INNER JOIN dbo.Terminals tt ON tt.TerminalCode=efe.TerminalCode 
        //  INNER JOIN dbo.ServerInfo sv ON sv.ID = tt.ServerInfoID 
        //  INNER JOIN dbo.ElectricFence ef ON ef.ID=efe.FenceID
        //  INNER JOIN dbo.VehicleElectricFence vef ON vef.VehicleID=tt.LinkedVehicleID
        //  WHERE sv.LinkedServerName=@LinkedServerName AND vef.VehicleID=@vehicleID AND ef.ID=@fenceID AND vef.FenceID=@fenceID AND vef.Status<>9 AND ef.Status<>9 AND tt.Status<>9", linkedServerName);

        //            List<SqlParameter> paras = new List<SqlParameter>()
        //            {
        //                new SqlParameter("@LinkedServerName",SqlDbType.VarChar),
        //                new SqlParameter("@vehicleID",SqlDbType.BigInt),
        //                new SqlParameter("@fenceID",SqlDbType.Int)
        //            };
        //            paras[0].Value = linkedServerName;
        //            paras[1].Value = vehicleID;
        //            paras[2].Value = fenceID;
        //            return ConvertToList<ElectricFenceExceptionModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        //        }
        //        #endregion

        #endregion

    }
}

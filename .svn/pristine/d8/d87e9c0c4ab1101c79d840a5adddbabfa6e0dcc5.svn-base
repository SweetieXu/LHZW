using Asiatek.AjaxPager;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Asiatek.BLL.MSSQL
{
    public class MaintenanceScheduleBLL
    {
        #region 查询
        /// <summary>
        /// 获取当前单位及其子单位下的保养方案记录
        /// </summary>
        /// <returns></returns>
        public static AsiatekPagedList<MaintenanceScheduleListModel> GetPagedMaintenanceSchedule(MaintenanceScheduleSearchModel model, int searchPage, int pageSize, int currentStrucID)
        {
            string joinStr = string.Format(@"  INNER JOIN dbo.Users u ON u.ID = ms.CreateUser 
  INNER JOIN Func_GetStrucAndSubStrucByUserAffiliatedStrucID({0}) st ON st.ID=u.StrucID 
  LEFT JOIN (SELECT COUNT(*) AS BindVehicleNum,ScheduleID FROM dbo.VehicleMaintenance GROUP BY ScheduleID) num ON num.ScheduleID=ms.ID", currentStrucID);

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.MaintenanceSchedule AS ms "),
                new SqlParameter("@joinStr",joinStr),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","ms.ID DESC"),
                new SqlParameter("@showColumns",@" ms.ID,ms.ScheduleName,ms.RulesType,ms.Remark,st.StrucName,u.UserName AS CreateUser,num.BindVehicleNum "),
            };

            #region 筛选条件
            string conditionStr = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(model.SearchScheduleName))
            {
                conditionStr += " AND ms.ScheduleName LIKE '%" + model.SearchScheduleName + "%'";
            }
            if (model.SearchStrucID != -1)
            {
                conditionStr += " AND u.StrucID = " + model.SearchStrucID + "";
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
            List<MaintenanceScheduleListModel> list = ConvertToList<MaintenanceScheduleListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        #endregion

        #region 新增
        public static OperationResult AddMaintenanceSchedule(AddMaintenanceScheduleModel model, int currentUserID)
        {
            int len = model.MaintenanceProjectDetails.Count + 1;  //数据表中添加数据=保养方案表一条数据+保养内容表的Count条数据
            string[] sqls = new string[len];
            SqlParameter[][] paras = new SqlParameter[len][];
            int num = 1;

            #region 保养方案表添加
            sqls[0] = @"INSERT  INTO dbo.MaintenanceSchedule
                    ( ScheduleName 
      ,RulesType 
      ,SettingMile 
      ,PreMile 
      ,SettingDay 
      ,PreDay 
      ,Remark 
      ,CreateUser 
      ,CreateTime 
                    )
            VALUES  ( @ScheduleName , 
                      @RulesType, 
                      @SettingMile , 
                      @PreMile , 
                      @SettingDay, 
                      @PreDay , 
                      @Remark, 
                      @CreateUser,
                      @CreateTime
                    );SELECT  SCOPE_IDENTITY();";

            paras[0] = new SqlParameter[9];
            paras[0][0] = new SqlParameter()
            {
                ParameterName = "@ScheduleName",
                SqlDbType = SqlDbType.NVarChar,
                Value = model.ScheduleName.Trim()
            };
            paras[0][1] = new SqlParameter()
            {
                ParameterName = "@RulesType",
                SqlDbType = SqlDbType.Int,
                Value = model.RulesType
            };
            if (model.SettingMile == null)
            {
                paras[0][2] = new SqlParameter()
                {
                    ParameterName = "@SettingMile",
                    SqlDbType = SqlDbType.Int,
                    Value = DBNull.Value
                };
            }
            else
            {
                paras[0][2] = new SqlParameter()
                {
                    ParameterName = "@SettingMile",
                    SqlDbType = SqlDbType.Int,
                    Value = model.SettingMile
                };
            }
            if (model.PreMile == null)
            {
                paras[0][3] = new SqlParameter()
                {
                    ParameterName = "@PreMile",
                    SqlDbType = SqlDbType.Int,
                    Value = DBNull.Value
                };
            }
            else
            {
                paras[0][3] = new SqlParameter()
                {
                    ParameterName = "@PreMile",
                    SqlDbType = SqlDbType.Int,
                    Value = model.PreMile
                };
            }
            if (model.SettingDay == null)
            {
                paras[0][4] = new SqlParameter()
                {
                    ParameterName = "@SettingDay",
                    SqlDbType = SqlDbType.Int,
                    Value = DBNull.Value
                };
            }
            else
            {
                paras[0][4] = new SqlParameter()
                {
                    ParameterName = "@SettingDay",
                    SqlDbType = SqlDbType.Int,
                    Value = model.SettingDay
                };
            }
            if (model.PreDay == null)
            {
                paras[0][5] = new SqlParameter()
                {
                    ParameterName = "@PreDay",
                    SqlDbType = SqlDbType.Int,
                    Value = DBNull.Value
                };
            }
            else
            {
                paras[0][5] = new SqlParameter()
                {
                    ParameterName = "@PreDay",
                    SqlDbType = SqlDbType.Int,
                    Value = model.PreDay
                };
            }
            if (string.IsNullOrEmpty(model.Remark))
            {
                paras[0][6] = new SqlParameter()
                {
                    ParameterName = "@Remark",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = DBNull.Value
                };
            }
            else
            {
                paras[0][6] = new SqlParameter()
                {
                    ParameterName = "@Remark",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.Remark
                };
            }
            paras[0][7] = new SqlParameter()
            {
                ParameterName = "@CreateUser",
                SqlDbType = SqlDbType.Int,
                Value = currentUserID
            };
            paras[0][8] = new SqlParameter()
            {
                ParameterName = "@CreateTime",
                SqlDbType = SqlDbType.DateTime,
                Value = DateTime.Now
            };
            #endregion

            #region 保养内容表添加
            for (int i = 0; i < model.MaintenanceProjectDetails.Count; i++)
            {
                string tempSql = string.Empty;
                tempSql = @" INSERT  INTO dbo.MaintenanceProject( ScheduleID
      , ProjectName
      , PartsName 
      , PartsBrand 
      , PartsVersion 
      , Num 
      , Unit 
      , UnitPrice 
      , CreateUser 
      , CreateTime  ) VALUES  ( @ScheduleID,@ProjectName,@PartsName,@PartsBrand,@PartsVersion,@Num,@Unit,@UnitPrice,@CreateUser1,@CreateTime1)";
                sqls[num] = tempSql;
                paras[num] = new SqlParameter[10];
                //@ScheduleID用的上一条数据自增列ID，此处参数只能放在[num][0]的位置，放在[num][1]无效
                paras[num][0] = new SqlParameter { ParameterName = "@ScheduleID", SqlDbType = SqlDbType.Int };  
                paras[num][1] = new SqlParameter()
                {
                    ParameterName = "@ProjectName",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = model.MaintenanceProjectDetails[i].ProjectName.Trim()
                };
                if (string.IsNullOrEmpty(model.MaintenanceProjectDetails[i].PartsName))
                {
                    paras[num][2] = new SqlParameter()
                    {
                        ParameterName = "@PartsName",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    paras[num][2] = new SqlParameter()
                    {
                        ParameterName = "@PartsName",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.MaintenanceProjectDetails[i].PartsName.Trim()
                    };
                }
                if (string.IsNullOrEmpty(model.MaintenanceProjectDetails[i].PartsBrand))
                {
                    paras[num][3] = new SqlParameter()
                    {
                        ParameterName = "@PartsBrand",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = DBNull.Value
                    };
                }
                else {
                    paras[num][3] = new SqlParameter()
                    {
                        ParameterName = "@PartsBrand",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.MaintenanceProjectDetails[i].PartsBrand.Trim()
                    };
                }
                if (string.IsNullOrEmpty(model.MaintenanceProjectDetails[i].PartsVersion))
                {
                    paras[num][4] = new SqlParameter()
                    {
                        ParameterName = "@PartsVersion",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    paras[num][4] = new SqlParameter()
                    {
                        ParameterName = "@PartsVersion",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.MaintenanceProjectDetails[i].PartsVersion.Trim()
                    };
                }
                if (model.MaintenanceProjectDetails[i].Num == null)
                {
                    paras[num][5] = new SqlParameter()
                    {
                        ParameterName = "@Num",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    paras[num][5] = new SqlParameter()
                    {
                        ParameterName = "@Num",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MaintenanceProjectDetails[i].Num
                    };
                }
                if (string.IsNullOrEmpty(model.MaintenanceProjectDetails[i].Unit))
                {
                    paras[num][6] = new SqlParameter()
                    {
                        ParameterName = "@Unit",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    paras[num][6] = new SqlParameter()
                    {
                        ParameterName = "@Unit",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = model.MaintenanceProjectDetails[i].Unit.Trim()
                    };
                }
                if (model.MaintenanceProjectDetails[i].UnitPrice == null)
                {
                    paras[num][7] = new SqlParameter()
                    {
                        ParameterName = "@UnitPrice",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    paras[num][7] = new SqlParameter()
                    {
                        ParameterName = "@UnitPrice",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MaintenanceProjectDetails[i].UnitPrice
                    };
                }
                paras[num][8] = new SqlParameter()
                {
                    ParameterName = "@CreateUser1",
                    SqlDbType = SqlDbType.Int,
                    Value = currentUserID
                };
                paras[num][9] = new SqlParameter()
                {
                    ParameterName = "@CreateTime1",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DateTime.Now
                };
                num++;
            }
            #endregion

            bool result = MSSQLHelper.ExecuteIdentityIncludeTransaction(CommandType.Text, sqls, paras) != 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        /// <summary>
        /// 添加时 验证保养方案名称
        /// </summary>
        /// <param name="scheduleName"></param>
        /// <returns></returns>
        public static bool CheckAddScheduleNameExists(string scheduleName)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ScheduleName",SqlDbType.NVarChar,50),
            };
            paras[0].Value = scheduleName.Trim();

            string sql = "SELECT COUNT(0) FROM dbo.MaintenanceSchedule WHERE ScheduleName=@ScheduleName ";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion

        #region 物理删除
        public static OperationResult DeleteMaintenanceSchedule(string[] ids)
        {
            int len = ids.Length * 3;  //删除保养方案表，相关联的保养项目表中数据，绑定的车辆数据
            string[] sqls = new string[len];
            SqlParameter[][] paras = new SqlParameter[len][];
            for (int i = 0; i < len; i = i+3)
            {
                sqls[i] = @"DELETE FROM dbo.MaintenanceSchedule WHERE ID=@ID";
                sqls[i + 1] = @"DELETE FROM dbo.MaintenanceProject WHERE ScheduleID=@ID";
                sqls[i + 2] = @"DELETE FROM dbo.VehicleMaintenance WHERE ScheduleID=@ID";
                paras[i] = new SqlParameter[1];
                paras[i + 1] = new SqlParameter[1];
                paras[i + 2] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i / 3];
                paras[i][0] = temp;
                paras[i + 1][0] = temp;
                paras[i + 2][0] = temp;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion

        #region 修改
        public static SelectResult<EditMaintenanceScheduleModel> GetMaintenanceScheduleByID(int id)
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
            string sql = @"SELECT ms.ID 
      , ms.ScheduleName 
      , ms.RulesType 
      , ms.SettingMile 
      , ms.PreMile 
      , ms.SettingDay 
      , ms.PreDay 
      , ms.Remark 
,num.BindVehicleNum
      FROM dbo.MaintenanceSchedule ms 
LEFT JOIN (SELECT COUNT(*) AS BindVehicleNum,ScheduleID FROM dbo.VehicleMaintenance GROUP BY ScheduleID) num ON num.ScheduleID=ms.ID 
      WHERE ms.ID=@ID";

            List<EditMaintenanceScheduleModel> list = ConvertToList<EditMaintenanceScheduleModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditMaintenanceScheduleModel data = null;
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
                //保养方案表有数据的情况下，查询保养内容表，填充Model.MaintenanceProjectDetails数据
                List<SqlParameter> detailParas = new List<SqlParameter>()
                {
                    new SqlParameter()
                    {
                        ParameterName="@ID",
                        SqlDbType=SqlDbType.Int,
                    },
                };
                detailParas[0].Value = id;
                string detailSql = @"SELECT ID AS ProjectID,
ProjectName 
      , ScheduleID 
      , PartsName 
      , PartsBrand 
      , PartsVersion 
      , Num 
      , Unit 
      , UnitPrice 
FROM dbo.MaintenanceProject
WHERE ScheduleID=@ID";
                List<MaintenanceProjectModel> detailList = ConvertToList<MaintenanceProjectModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, detailSql, detailParas.ToArray()));
                list[0].MaintenanceProjectDetails = detailList;
                data = list[0];
            }
            return new SelectResult<EditMaintenanceScheduleModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditMaintenanceScheduleModel(EditMaintenanceScheduleModel model, int currentUserID)
        {
            int result = 0;
            using (TransactionScope transaction = new TransactionScope())//使用事务
            {
                try
                {
                    if (model.BindVehicleNum == 0)  //无绑定车辆，可修改
                    {
                        #region 更新保养方案表
                        var updateSql = @"UPDATE dbo.MaintenanceSchedule
                    SET ScheduleName = @ScheduleName 
      ,RulesType = @RulesType
      ,SettingMile = @SettingMile 
      ,PreMile = @PreMile
      ,SettingDay = @SettingDay
      ,PreDay = @PreDay 
      ,Remark = @Remark 
      ,UpdateTime = GetDate() 
      ,UpdateUser =  @UpdateUser
       WHERE  ID = @ID ";
                        SqlParameter[] updateParas = new SqlParameter[9];
                        updateParas[0] = new SqlParameter()
                        {
                            ParameterName = "@ScheduleName",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = model.ScheduleName.Trim()
                        };
                        updateParas[1] = new SqlParameter()
                        {
                            ParameterName = "@RulesType",
                            SqlDbType = SqlDbType.Int,
                            Value = model.RulesType
                        };
                        if (model.SettingMile == null)
                        {
                            updateParas[2] = new SqlParameter()
                            {
                                ParameterName = "@SettingMile",
                                SqlDbType = SqlDbType.Int,
                                Value = DBNull.Value
                            };
                        }
                        else
                        {
                            updateParas[2] = new SqlParameter()
                            {
                                ParameterName = "@SettingMile",
                                SqlDbType = SqlDbType.Int,
                                Value = model.SettingMile
                            };
                        }
                        if (model.PreMile == null)
                        {
                            updateParas[3] = new SqlParameter()
                            {
                                ParameterName = "@PreMile",
                                SqlDbType = SqlDbType.Int,
                                Value = DBNull.Value
                            };
                        }
                        else
                        {
                            updateParas[3] = new SqlParameter()
                            {
                                ParameterName = "@PreMile",
                                SqlDbType = SqlDbType.Int,
                                Value = model.PreMile
                            };
                        }
                        if (model.SettingDay == null)
                        {
                            updateParas[4] = new SqlParameter()
                            {
                                ParameterName = "@SettingDay",
                                SqlDbType = SqlDbType.Int,
                                Value = DBNull.Value
                            };
                        }
                        else
                        {
                            updateParas[4] = new SqlParameter()
                            {
                                ParameterName = "@SettingDay",
                                SqlDbType = SqlDbType.Int,
                                Value = model.SettingDay
                            };
                        }
                        if (model.PreDay == null)
                        {
                            updateParas[5] = new SqlParameter()
                            {
                                ParameterName = "@PreDay",
                                SqlDbType = SqlDbType.Int,
                                Value = DBNull.Value
                            };
                        }
                        else
                        {
                            updateParas[5] = new SqlParameter()
                            {
                                ParameterName = "@PreDay",
                                SqlDbType = SqlDbType.Int,
                                Value = model.PreDay
                            };
                        }
                        if (string.IsNullOrEmpty(model.Remark))
                        {
                            updateParas[6] = new SqlParameter()
                            {
                                ParameterName = "@Remark",
                                SqlDbType = SqlDbType.NVarChar,
                                Value = DBNull.Value
                            };
                        }
                        else
                        {
                            updateParas[6] = new SqlParameter()
                            {
                                ParameterName = "@Remark",
                                SqlDbType = SqlDbType.NVarChar,
                                Value = model.Remark
                            };
                        }
                        updateParas[7] = new SqlParameter()
                        {
                            ParameterName = "@UpdateUser",
                            SqlDbType = SqlDbType.Int,
                            Value = currentUserID
                        };
                        updateParas[8] = new SqlParameter()
                        {
                            ParameterName = "@ID",
                            SqlDbType = SqlDbType.Int,
                            Value = model.ID
                        };
                        int updateResult = MSSQLHelper.ExecuteNonQuery(CommandType.Text, updateSql, updateParas);
                        #endregion

                        #region 删除保养项目表中关联数据
                        var delSql = @"DELETE FROM dbo.MaintenanceProject WHERE ScheduleID=@ScheduleID";
                        SqlParameter[] delParas = new SqlParameter[1];
                        delParas[0] = new SqlParameter()
                        {
                            ParameterName = "@ScheduleID",
                            SqlDbType = SqlDbType.Int,
                            Value = model.ID
                        };
                        int delResult = MSSQLHelper.ExecuteNonQuery(CommandType.Text, delSql, delParas);
                        #endregion

                        #region 添加保养项目
                        int len = model.MaintenanceProjectDetails.Count;
                        string[] sqls = new string[len];
                        SqlParameter[][] paras = new SqlParameter[len][];
                        int num = 0;
                        for (int i = 0; i < len; i++)
                        {
                            string tempSql = string.Empty;
                            tempSql = @" INSERT  INTO dbo.MaintenanceProject( ScheduleID
              , ProjectName
              , PartsName 
              , PartsBrand 
              , PartsVersion 
              , Num 
              , Unit 
              , UnitPrice 
              , CreateUser 
              , CreateTime  ) VALUES  ( @ScheduleID,@ProjectName,@PartsName,@PartsBrand,@PartsVersion,@Num,@Unit,@UnitPrice,@CreateUser1,@CreateTime1)";
                            sqls[num] = tempSql;
                            paras[num] = new SqlParameter[10];
                            paras[num][0] = new SqlParameter()
                            {
                                ParameterName = "@ScheduleID",
                                SqlDbType = SqlDbType.Int,
                                Value = model.ID
                            };
                            paras[num][1] = new SqlParameter()
                            {
                                ParameterName = "@ProjectName",
                                SqlDbType = SqlDbType.NVarChar,
                                Value = model.MaintenanceProjectDetails[i].ProjectName.Trim()
                            };
                            if (string.IsNullOrEmpty(model.MaintenanceProjectDetails[i].PartsName))
                            {
                                paras[num][2] = new SqlParameter()
                                {
                                    ParameterName = "@PartsName",
                                    SqlDbType = SqlDbType.NVarChar,
                                    Value = DBNull.Value
                                };
                            }
                            else
                            {
                                paras[num][2] = new SqlParameter()
                                {
                                    ParameterName = "@PartsName",
                                    SqlDbType = SqlDbType.NVarChar,
                                    Value = model.MaintenanceProjectDetails[i].PartsName.Trim()
                                };
                            }
                            if (string.IsNullOrEmpty(model.MaintenanceProjectDetails[i].PartsBrand))
                            {
                                paras[num][3] = new SqlParameter()
                                {
                                    ParameterName = "@PartsBrand",
                                    SqlDbType = SqlDbType.NVarChar,
                                    Value = DBNull.Value
                                };
                            }
                            else
                            {
                                paras[num][3] = new SqlParameter()
                                {
                                    ParameterName = "@PartsBrand",
                                    SqlDbType = SqlDbType.NVarChar,
                                    Value = model.MaintenanceProjectDetails[i].PartsBrand.Trim()
                                };
                            }
                            if (string.IsNullOrEmpty(model.MaintenanceProjectDetails[i].PartsVersion))
                            {
                                paras[num][4] = new SqlParameter()
                                {
                                    ParameterName = "@PartsVersion",
                                    SqlDbType = SqlDbType.NVarChar,
                                    Value = DBNull.Value
                                };
                            }
                            else
                            {
                                paras[num][4] = new SqlParameter()
                                {
                                    ParameterName = "@PartsVersion",
                                    SqlDbType = SqlDbType.NVarChar,
                                    Value = model.MaintenanceProjectDetails[i].PartsVersion.Trim()
                                };
                            }
                            if (model.MaintenanceProjectDetails[i].Num == null)
                            {
                                paras[num][5] = new SqlParameter()
                                {
                                    ParameterName = "@Num",
                                    SqlDbType = SqlDbType.Int,
                                    Value = DBNull.Value
                                };
                            }
                            else
                            {
                                paras[num][5] = new SqlParameter()
                                {
                                    ParameterName = "@Num",
                                    SqlDbType = SqlDbType.Int,
                                    Value = model.MaintenanceProjectDetails[i].Num
                                };
                            }
                            if (string.IsNullOrEmpty(model.MaintenanceProjectDetails[i].Unit))
                            {
                                paras[num][6] = new SqlParameter()
                                {
                                    ParameterName = "@Unit",
                                    SqlDbType = SqlDbType.NVarChar,
                                    Value = DBNull.Value
                                };
                            }
                            else
                            {
                                paras[num][6] = new SqlParameter()
                                {
                                    ParameterName = "@Unit",
                                    SqlDbType = SqlDbType.NVarChar,
                                    Value = model.MaintenanceProjectDetails[i].Unit.Trim()
                                };
                            }
                            if (model.MaintenanceProjectDetails[i].UnitPrice == null)
                            {
                                paras[num][7] = new SqlParameter()
                                {
                                    ParameterName = "@UnitPrice",
                                    SqlDbType = SqlDbType.Int,
                                    Value = DBNull.Value
                                };
                            }
                            else
                            {
                                paras[num][7] = new SqlParameter()
                                {
                                    ParameterName = "@UnitPrice",
                                    SqlDbType = SqlDbType.Int,
                                    Value = model.MaintenanceProjectDetails[i].UnitPrice
                                };
                            }
                            paras[num][8] = new SqlParameter()
                            {
                                ParameterName = "@CreateUser1",
                                SqlDbType = SqlDbType.Int,
                                Value = currentUserID
                            };
                            paras[num][9] = new SqlParameter()
                            {
                                ParameterName = "@CreateTime1",
                                SqlDbType = SqlDbType.DateTime,
                                Value = DateTime.Now
                            };
                            num++;
                        }
                        #endregion

                        bool insertResult = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
                        if (updateResult > 0 && delResult != 0 && insertResult == true)
                        {
                            result = 1;
                            transaction.Complete();//成功再执行这句话
                        }
                    }
                    else  //只能修改名称和备注 
                    {
                        var updateSql2 = @"UPDATE dbo.MaintenanceSchedule 
                    SET ScheduleName = @ScheduleName 
      ,Remark = @Remark 
      ,UpdateTime = GetDate() 
      ,UpdateUser =  @UpdateUser 
       WHERE  ID = @ID ";
                        SqlParameter[] updateParas2 = new SqlParameter[4];
                        updateParas2[0] = new SqlParameter()
                        {
                            ParameterName = "@ScheduleName",
                            SqlDbType = SqlDbType.NVarChar,
                            Value = model.ScheduleName.Trim()
                        };
                        if (string.IsNullOrEmpty(model.Remark))
                        {
                            updateParas2[1] = new SqlParameter()
                            {
                                ParameterName = "@Remark",
                                SqlDbType = SqlDbType.NVarChar,
                                Value = DBNull.Value
                            };
                        }
                        else
                        {
                            updateParas2[1] = new SqlParameter()
                            {
                                ParameterName = "@Remark",
                                SqlDbType = SqlDbType.NVarChar,
                                Value = model.Remark
                            };
                        }
                        updateParas2[2] = new SqlParameter()
                        {
                            ParameterName = "@UpdateUser",
                            SqlDbType = SqlDbType.Int,
                            Value = currentUserID
                        };
                        updateParas2[3] = new SqlParameter()
                        {
                            ParameterName = "@ID",
                            SqlDbType = SqlDbType.Int,
                            Value = model.ID
                        };
                        result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, updateSql2, updateParas2);
                        if (result == 1)
                        {
                            transaction.Complete();//成功再执行这句话
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

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

        public static bool CheckEditScheduleNameExists(string scheduleName, int id, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ScheduleName",SqlDbType.NVarChar,50),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = scheduleName.Trim();
            paras[1].Value = id;

            string sql = "SELECT COUNT(0) FROM dbo.MaintenanceSchedule WHERE ID!=@ID AND ScheduleName=@ScheduleName ";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion

        #region  绑定车辆
        /// <summary>
        /// 获取用户分配的车辆信息   获取其自己和自己子单位的所有车辆
        /// </summary>
        public static AsiatekPagedList<MSBindVehicleListModel> GetPagedCustomerVehicle(int id, MSBindVehicleSearchModel model, int searchPage, int pageSize, int strucID)
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

            StringBuilder sbWhere = new StringBuilder("ve.Status<>9 AND st.Status<>9 AND ve.ID NOT IN (SELECT VehicleID FROM dbo.VehicleMaintenance WHERE ScheduleID=" + id + ")");

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
            List<MSBindVehicleListModel> list = ConvertToList<MSBindVehicleListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }


        public static OperationResult AddVehicleToMaintenanceSchedule(string[,] datas, int uID)
        {
            var len = datas.GetLength(0); //取行数
            SqlParameter[][] paras = new SqlParameter[len][];
            string[] sqls = new string[len];

            for (int i = 0; i < len; i++) 
            {
                sqls[i] = @"INSERT  INTO dbo.VehicleMaintenance
                                ([VehicleID]
                      ,[ScheduleID]
                      ,[FirstMaintenanceMile]
                      ,[FirstMaintenanceTime]
                      ,[CreateUser]
                      ,[CreateTime]
                                )
                        VALUES  ( @VehicleID, 
                                    @ScheduleID,
                                    @FirstMaintenanceMile , 
                                    @FirstMaintenanceTime ,
                                    @CreateUser, 
                                    @CreateTime  
                                )";
                paras[i] = new SqlParameter[6];
                paras[i][0] = new SqlParameter()
                {
                    ParameterName = "@ScheduleID",
                    SqlDbType = SqlDbType.Int,
                    Value = datas[i,1]
                };
                paras[i][1] = new SqlParameter()
                {
                    ParameterName = "@VehicleID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = datas[i, 0]
                };
                paras[i][2] = new SqlParameter()
                {
                    ParameterName = "@FirstMaintenanceMile",
                    SqlDbType = SqlDbType.Float,
                    Value = datas[i, 2]
                };
                paras[i][3] = new SqlParameter()
                {
                    ParameterName = "@FirstMaintenanceTime",
                    SqlDbType = SqlDbType.Date,
                    Value = datas[i, 3]
                };
                paras[i][4] = new SqlParameter()
                {
                    ParameterName = "@CreateUser",
                    SqlDbType = SqlDbType.Int,
                    Value = uID
                };
                paras[i][5] = new SqlParameter()
                {
                    ParameterName = "@CreateTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DateTime.Now
                };
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

//        public static OperationResult AddVehicleToMaintenanceSchedule(int scheduleID, long vehicleID, string firstMile, string firstTime, int uID)
//        {
//            List<SqlParameter> paras = new List<SqlParameter>()
//            {
//                new SqlParameter("@ScheduleID",SqlDbType.Int),
//                new SqlParameter("@VehicleID",SqlDbType.BigInt),
//                new SqlParameter("@FirstMaintenanceMile",SqlDbType.Float),
//                new SqlParameter("@FirstMaintenanceTime",SqlDbType.DateTime),
//                new SqlParameter("@CreateUser",SqlDbType.Int),
//                new SqlParameter("@CreateTime",SqlDbType.DateTime),
//            };

//            paras[0].Value = scheduleID;
//            paras[1].Value = vehicleID;
//            if (string.IsNullOrEmpty(firstMile))
//            {
//                paras[2].Value = DBNull.Value;
//            }
//            else {
//                paras[2].Value = Double.Parse(firstMile);
//            }
//            if (string.IsNullOrEmpty(firstTime))
//            {
//                paras[3].Value = DBNull.Value;
//            }
//            else
//            {
//                paras[3].Value = Convert.ToDateTime(firstTime);
//            }
//            paras[4].Value = uID;
//            paras[5].Value = DateTime.Now;

//            string sql = @"INSERT  INTO dbo.VehicleMaintenance
//                ([VehicleID]
//      ,[ScheduleID]
//      ,[FirstMaintenanceMile]
//      ,[FirstMaintenanceTime]
//      ,[CreateUser]
//      ,[CreateTime]
//                )
//        VALUES  ( @VehicleID, 
//                    @ScheduleID,
//                    @FirstMaintenanceMile , 
//                    @FirstMaintenanceTime ,
//                    @CreateUser, 
//                    @CreateTime  
//                )";
//            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
//            return new OperationResult()
//            {
//                Success = result,
//                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
//            };
//        }
        #endregion

        #region  解绑
        public static List<MSBindVehicleListModel> GetVehicleByScheduleID(int id)
        {
            string sql = string.Format(@" SELECT vm.ScheduleID,vm.CreateTime,ms.ScheduleName,vm.FirstMaintenanceMile,vm.FirstMaintenanceTime,
  ve.VIN,ve.VehicleName,ve.ID AS VehicleID,ve.PlateNum 
  FROM dbo.VehicleMaintenance vm 
  INNER JOIN dbo.MaintenanceSchedule ms ON ms.ID=vm.ScheduleID 
  LEFT JOIN dbo.Vehicles ve ON ve.ID = vm.VehicleID
  WHERE ve.Status<>9 AND vm.ScheduleID={0} 
  ORDER BY vm.CreateTime DESC ", id);

            return ConvertToList<MSBindVehicleListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }


        public static OperationResult DelVehicleFromMaintenanceSchedule(long vehicleID, int scheduleID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleID",SqlDbType.BigInt),
                new SqlParameter("@ScheduleID",SqlDbType.Int)
            };

            paras[0].Value = vehicleID;
            paras[1].Value = scheduleID;

            string sql = @"DELETE FROM dbo.VehicleMaintenance WHERE VehicleID=@VehicleID AND ScheduleID=@ScheduleID";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion
    }
}

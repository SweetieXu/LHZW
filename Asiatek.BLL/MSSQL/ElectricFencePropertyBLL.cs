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
    public class ElectricFencePropertyBLL
    {
        #region  查询
        public static AsiatekPagedList<ElectricFencePropertyListModel> GetPagedEFPropertys(ElectricFencePropertySearchModel model, int searchPage, int pageSize, int strucID)
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
                new SqlParameter("@tableName","dbo.ElectricFenceProperty efp "),
                new SqlParameter("@joinStr",@" INNER JOIN dbo.Users AS us ON us.ID = efp.CreateUser "),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","efp.CreateTime DESC"),
                new SqlParameter("@showColumns",@"efp.ID,efp.PropertyName,efp.FenceState,efp.ValidStartTime,efp.ValidEndTime,
                                                                             efp.AlarmType,efp.IsSpeed,efp.MaxSpeed,efp.IsPeriod "), 
            };

            string conditionStr = " ";
            if (isResult != DBNull.Value)
            {
                conditionStr = "efp.Status<>9 AND us.StrucID IN (SELECT ID FROM Func_GetAllSubStructsAndParentStructsByStrucID(" + strucID + ") WHERE ParentID IS NOT NULL) ";
            }
            else
            {
                conditionStr = "efp.Status<>9 ";
            }
            if (!string.IsNullOrWhiteSpace(model.PropertyName))
            {
                conditionStr += " AND efp.PropertyName LIKE '%" + model.PropertyName + "%'";
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
            var rs = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray());
            List<ElectricFencePropertyListModel> list = ConvertToList<ElectricFencePropertyListModel>.Convert(rs);
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion


        #region 新增
        public static OperationResult AddEFProperty(AddEFPropertyModel model, int currentUserID)
        {
            int len=0;
            for (int i = 0; i < model.EFPropertyPeriod.Count; i++)
            {
                if (model.EFPropertyPeriod[i].Week != null)
                {
                    len++;
                }
            }
            //插入主表1条记录+插入关联表len条记录
            len = len + 1;
            string[] sqls = new string[len];
            SqlParameter[][] paras = new SqlParameter[len][];
            int num = 1;

            #region 添加ElectricFenceProperty表
            sqls[0] = @" INSERT  INTO dbo.ElectricFenceProperty
                ( [PropertyName]
                  ,[FenceState]
                  ,[ValidStartTime]
                  ,[ValidEndTime]
                  ,[AlarmType]
                  ,[IsSpeed]
                  ,[MaxSpeed]
                  ,[IsPeriod]
                  ,[Status]
                  ,[CreateUser]
                  ,[CreateTime]
                )
        VALUES  ( @PropertyName , 
                    @FenceState, 
                    @ValidStartTime,
                    @ValidEndTime , 
                    @AlarmType, 
                    @IsSpeed , 
                    @MaxSpeed, 
                    @IsPeriod , 
                    @Status , 
                    @CreateUser, 
                    @CreateTime 
                );SELECT  SCOPE_IDENTITY();";

            paras[0] = new SqlParameter[11];
            paras[0][0] = new SqlParameter()
            {
                ParameterName = "@PropertyName",
                SqlDbType = SqlDbType.NVarChar,
                Value = model.PropertyName.Trim()
            };
            paras[0][1] = new SqlParameter()
            {
                ParameterName = "@FenceState",
                SqlDbType = SqlDbType.Bit,
                Value = model.FenceState
            };
            paras[0][2] = new SqlParameter()
            {
                ParameterName = "@ValidStartTime",
                SqlDbType = SqlDbType.DateTime,
                Value = model.ValidStartTime
            };
            paras[0][3] = new SqlParameter()
            {
                ParameterName = "@ValidEndTime",
                SqlDbType = SqlDbType.DateTime,
                Value = model.ValidEndTime
            };
            if (model.AlarmType == null)
            {
                paras[0][4] = new SqlParameter()
                {
                    ParameterName = "@AlarmType",
                    SqlDbType = SqlDbType.Bit,
                    Value = DBNull.Value
                };
            }
            else {
                paras[0][4] = new SqlParameter()
                {
                    ParameterName = "@AlarmType",
                    SqlDbType = SqlDbType.Bit,
                    Value = model.AlarmType
                };
            }
            paras[0][5] = new SqlParameter()
            {
                ParameterName = "@IsSpeed",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsSpeed
            };
            if (model.MaxSpeed == null)
            {
                paras[0][6] = new SqlParameter()
                {
                    ParameterName = "@MaxSpeed",
                    SqlDbType = SqlDbType.Float,
                    Value = DBNull.Value
                };
            }
            else {
                paras[0][6] = new SqlParameter()
                {
                    ParameterName = "@MaxSpeed",
                    SqlDbType = SqlDbType.Float,
                    Value = model.MaxSpeed
                };
            }
            paras[0][7] = new SqlParameter()
            {
                ParameterName = "@IsPeriod",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsPeriod
            };
            paras[0][8] = new SqlParameter()
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = 0
            };
            paras[0][9] = new SqlParameter()
            {
                ParameterName = "@CreateUser",
                SqlDbType = SqlDbType.Int,
                Value = currentUserID
            };
            paras[0][10] = new SqlParameter()
            {
                ParameterName = "@CreateTime",
                SqlDbType = SqlDbType.DateTime,
                Value = DateTime.Now
            };
            #endregion

            #region 添加ElectricFencePropertyPeriod表
            if (model.IsPeriod == true)
            {
                for (int i = 0; i < model.EFPropertyPeriod.Count; i++)
                {
                    if (model.EFPropertyPeriod[i].Week != null)
                    {
                         string tempSql = string.Empty;
                    tempSql = @"INSERT  INTO dbo.ElectricFencePropertyPeriod
                ( EFPropertyID
                ,[Week]
              ,[StartTime]
              ,[EndTime]
                )
        VALUES  ( @EFPropertyID,
                     @Week , 
                    @StartTime, 
                    @EndTime
                );";
                    sqls[num] = tempSql;
                    paras[num] = new SqlParameter[4];
                    paras[num][0] = new SqlParameter { ParameterName = "@EFPropertyID", SqlDbType = SqlDbType.Int };
                    paras[num][1] = new SqlParameter()
                    {
                        ParameterName = "@Week",
                        SqlDbType = SqlDbType.Int,
                        Value = model.EFPropertyPeriod[i].Week
                    };
                    paras[num][2] = new SqlParameter()
                    {
                        ParameterName = "@StartTime",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.EFPropertyPeriod[i].StartTime
                    };
                    paras[num][3] = new SqlParameter()
                    {
                        ParameterName = "@EndTime",
                        SqlDbType = SqlDbType.DateTime,
                        Value = model.EFPropertyPeriod[i].EndTime
                    };
                    num++;
                    }
                }
            }
           
            #endregion

            bool result = MSSQLHelper.ExecuteIdentityIncludeTransaction(CommandType.Text, sqls,paras) != 0;

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }


        /// <summary>
        /// 目前验证所有
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static bool CheckAddEFPropertyNameExists(string propertyName)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.ElectricFenceProperty WHERE PropertyName=@PropertyName AND Status<>9 ";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@PropertyName",SqlDbType.NVarChar),
            };
            paras[0].Value = propertyName.Trim();

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion


        //#region  删除 逻辑删除
        //public static OperationResult DeleteEFProperty(string[] ids)
        //{
        //    var IsRelateData = false;
        //    for (int i = 0; i < ids.Length; i++) {
        //        string IsSqls = @"SELECT COUNT(0) FROM dbo.ElectricFence WHERE PropertyID=@PropertyID ";
        //        List<SqlParameter> IsParas = new List<SqlParameter>()
        //        {
        //            new SqlParameter("@PropertyID",SqlDbType.Int),
        //        };
        //        IsParas[0].Value = ids[i];
        //        var count = MSSQLHelper.ExecuteScalar(CommandType.Text, IsSqls, IsParas.ToArray());
        //        if (count != null && (int)count > 0) {
        //            IsRelateData = true;
        //        }
        //    }
        //    int length = ids.Length * 2;
        //    string[] sqls = new string[length];
        //    SqlParameter[][] paras = new SqlParameter[length][];
        //    //删除关联的周期记录，物理删除
        //    for (int i = 0; i < length; i++)
        //    {
        //        sqls[i] = @"UPDATE dbo.ElectricFenceProperty SET Status=9 WHERE ID=@ID";
        //        sqls[i + 1] = @"DELETE FROM dbo.ElectricFencePropertyPeriod WHERE EFPropertyID=@ID";
        //        paras[i] = new SqlParameter[1];
        //        paras[i + 1] = new SqlParameter[1];
        //        SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
        //        temp.Value = ids[i / 2];
        //        paras[i][0] = temp;
        //        paras[i + 1][0] = temp;
        //        i = i + 1;
        //    }
        //    bool result;
        //    if (IsRelateData == false)
        //    {
        //        result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
        //        return new OperationResult()
        //        {
        //            Success = result,
        //            Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
        //        };
        //    }
        //    else {
        //        result = false;
        //        return new OperationResult()
        //        {
        //            Success = result,
        //            Message = result ? PromptInformation.DeleteSuccess : PromptInformation.HaveRelateEFPropertyInfo
        //        };
        //    }
            
        //}
        //#endregion


        #region  删除 物理删除
        public static OperationResult DeleteEFProperty(string[] ids)
        {
            var IsRelateData = false;
            for (int i = 0; i < ids.Length; i++)
            {
                string IsSqls = @"SELECT COUNT(0) FROM dbo.ElectricFence WHERE PropertyID=@PropertyID ";
                List<SqlParameter> IsParas = new List<SqlParameter>()
                {
                    new SqlParameter("@PropertyID",SqlDbType.Int),
                };
                IsParas[0].Value = ids[i];
                var count = MSSQLHelper.ExecuteScalar(CommandType.Text, IsSqls, IsParas.ToArray());
                if (count != null && (int)count > 0)
                {
                    IsRelateData = true;
                }
            }
            int length = ids.Length * 2;
            string[] sqls = new string[length];
            SqlParameter[][] paras = new SqlParameter[length][];
            //删除关联的周期记录，物理删除
            for (int i = 0; i < length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.ElectricFenceProperty WHERE ID=@ID";
                sqls[i + 1] = @"DELETE FROM dbo.ElectricFencePropertyPeriod WHERE EFPropertyID=@ID";
                paras[i] = new SqlParameter[1];
                paras[i + 1] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i / 2];
                paras[i][0] = temp;
                paras[i + 1][0] = temp;
                i = i + 1;
            }
            bool result;
            if (IsRelateData == false)
            {
                result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
                return new OperationResult()
                {
                    Success = result,
                    Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
                };
            }
            else
            {
                result = false;
                return new OperationResult()
                {
                    Success = result,
                    Message = result ? PromptInformation.DeleteSuccess : PromptInformation.HaveRelateEFPropertyInfo
                };
            }

        }
        #endregion



        #region 修改
        public static SelectResult<EditEFPropertyModel> GetEFPropertyByID(int id)
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
            string sql = @"SELECT ID,PropertyName,FenceState,ValidStartTime,ValidEndTime,AlarmType,IsSpeed,MaxSpeed,IsPeriod 
FROM dbo.ElectricFenceProperty WHERE Status<>9 AND ID=@ID";
            List<EditEFPropertyModel> list = ConvertToList<EditEFPropertyModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditEFPropertyModel data = null;
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
            return new SelectResult<EditEFPropertyModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditEFProperty(EditEFPropertyModel model, int currentUserID)
        {
            int result = 0;

            #region 更新ElectricFenceProperty表
            var updateSql = @"UPDATE dbo.ElectricFenceProperty
                      SET PropertyName = @PropertyName ,
                      FenceState = @FenceState ,
                      ValidStartTime = @ValidStartTime ,
                      ValidEndTime = @ValidEndTime ,
                      AlarmType = @AlarmType ,
                      IsSpeed = @IsSpeed ,
                      MaxSpeed = @MaxSpeed ,
                      IsPeriod = @IsPeriod ,
                      UpdateTime = GetDate() ,
                      UpdateUser = @UpdateUser 
                     WHERE  ID = @ID ";
            SqlParameter[] updateParas = new SqlParameter[10];
            updateParas[0] = new SqlParameter()
            {
                ParameterName = "@PropertyName",
                SqlDbType = SqlDbType.NVarChar,
                Value = model.PropertyName.Trim()
            };
            updateParas[1] = new SqlParameter()
            {
                ParameterName = "@FenceState",
                SqlDbType = SqlDbType.Bit,
                Value = model.FenceState
            };
            updateParas[2] = new SqlParameter()
            {
                ParameterName = "@ValidStartTime",
                SqlDbType = SqlDbType.DateTime,
                Value = model.ValidStartTime
            };
            updateParas[3] = new SqlParameter()
            {
                ParameterName = "@ValidEndTime",
                SqlDbType = SqlDbType.DateTime,
                Value = model.ValidEndTime
            };
            if (model.AlarmType == null)
            {
                updateParas[4] = new SqlParameter()
                {
                    ParameterName = "@AlarmType",
                    SqlDbType = SqlDbType.Bit,
                    Value = DBNull.Value
                };
            }
            else {
                updateParas[4] = new SqlParameter()
                {
                    ParameterName = "@AlarmType",
                    SqlDbType = SqlDbType.Bit,
                    Value = model.AlarmType
                };
            }
            updateParas[5] = new SqlParameter()
            {
                ParameterName = "@IsSpeed",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsSpeed
            };
            if (model.MaxSpeed == null)
            {
                updateParas[6] = new SqlParameter()
                {
                    ParameterName = "@MaxSpeed",
                    SqlDbType = SqlDbType.Float,
                    Value = DBNull.Value
                };
            }
            else
            {
                updateParas[6] = new SqlParameter()
                {
                    ParameterName = "@MaxSpeed",
                    SqlDbType = SqlDbType.Float,
                    Value = model.MaxSpeed
                };
            }
            updateParas[7] = new SqlParameter()
            {
                ParameterName = "@IsPeriod",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsPeriod
            };
            updateParas[8] = new SqlParameter()
            {
                ParameterName = "@UpdateUser",
                SqlDbType = SqlDbType.Int,
                Value = currentUserID
            };
            updateParas[9] = new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = model.ID
            };
            int updateResult = MSSQLHelper.ExecuteNonQuery(CommandType.Text, updateSql, updateParas);
            #endregion

            #region 删除ElectricFencePropertyPeriod表关联数据
            int delResult = 0;
            string sql = @"SELECT COUNT(0) FROM dbo.ElectricFencePropertyPeriod WHERE EFPropertyID=@EFPropertyID ";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@EFPropertyID",SqlDbType.Int),
            };
            paras[0].Value = model.ID;
            var count = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if ((int)count > 0)
            {
                var delSql = @"DELETE FROM dbo.ElectricFencePropertyPeriod WHERE EFPropertyID=@EFPropertyID";
                SqlParameter[] delParas = new SqlParameter[1];
                delParas[0] = new SqlParameter()
                {
                    ParameterName = "@EFPropertyID",
                    SqlDbType = SqlDbType.Int,
                    Value = model.ID
                };
                delResult = MSSQLHelper.ExecuteNonQuery(CommandType.Text, delSql, delParas);
            }
            else {
                delResult = 1;
            }
            #endregion

            #region 添加ElectricFencePropertyPeriod表关联数据
            bool insertResult = false;
            //插入关联表len条记录
            int len = 0;
            if (model.IsPeriod == true)
            {
                for (int i = 0; i < model.EFPropertyPeriod.Count; i++)
                {
                    if (model.EFPropertyPeriod[i].Week != null && model.EFPropertyPeriod[i].StartTime != null && model.EFPropertyPeriod[i].EndTime != null)
                    {
                        len++;
                    }
                }
            }
            if (model.IsPeriod == false || len == 0)
            {
                insertResult = true;
            }
            else {
                string[] sqls = new string[len];
                SqlParameter[][] addParas = new SqlParameter[len][];
                int num = 0;
                if (model.IsPeriod == true)
                {
                    for (int i = 0; i < model.EFPropertyPeriod.Count; i++)
                    {
                        if (model.EFPropertyPeriod[i].Week != null && model.EFPropertyPeriod[i].StartTime != null && model.EFPropertyPeriod[i].EndTime != null)
                        {
                            string tempSql = string.Empty;
                            tempSql = @"INSERT  INTO dbo.ElectricFencePropertyPeriod
                                    ( EFPropertyID
                                    ,[Week]
                                  ,[StartTime]
                                  ,[EndTime]
                                    )
                            VALUES  ( @PropertyID,
                                         @Week , 
                                        @StartTime, 
                                        @EndTime
                                    );";
                            sqls[num] = tempSql;
                            addParas[num] = new SqlParameter[4];
                            addParas[num][0] = new SqlParameter
                            {
                                ParameterName = "@PropertyID",
                                SqlDbType = SqlDbType.Int,
                                Value = model.ID
                            };
                            addParas[num][1] = new SqlParameter()
                            {
                                ParameterName = "@Week",
                                SqlDbType = SqlDbType.Int,
                                Value = model.EFPropertyPeriod[i].Week
                            };
                            addParas[num][2] = new SqlParameter()
                            {
                                ParameterName = "@StartTime",
                                SqlDbType = SqlDbType.DateTime,
                                Value = model.EFPropertyPeriod[i].StartTime
                            };
                            addParas[num][3] = new SqlParameter()
                            {
                                ParameterName = "@EndTime",
                                SqlDbType = SqlDbType.DateTime,
                                Value = model.EFPropertyPeriod[i].EndTime
                            };
                            num++;
                        }
                    }
                }
                insertResult = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, addParas);
            }
            #endregion

            if (updateResult > 0 && delResult != 0 && insertResult == true)
                result = 1;

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

        public static List<EFPropertyPeriodModel> GetEFPropertyPeriodByID(int id)
        {
            List<EFPropertyPeriodModel> list = new List<EFPropertyPeriodModel>();
            string sql = @" SELECT COUNT(0) FROM dbo.ElectricFencePropertyPeriod WHERE EFPropertyID=@EFPropertyID ";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@EFPropertyID",SqlDbType.Int),
            };
            paras[0].Value = id;
            var count = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (count != null)
            {
                string sqlTemp = string.Format(@" SELECT Week,CONVERT(VARCHAR(12), StartTime, 114) AS StartTime,CONVERT(VARCHAR(12), EndTime, 114) AS EndTime FROM dbo.ElectricFencePropertyPeriod WHERE EFPropertyID={0} ORDER BY Week ASC", id);
                list = ConvertToList<EFPropertyPeriodModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sqlTemp));
            }
            return list;
        }

        /// <summary>
        /// 目前验证所有
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="id"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static bool CheckEditEFPropertyNameExists(string propertyName, int id)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.ElectricFenceProperty WHERE PropertyName=@PropertyName AND ID!=@ID AND Status<>9 ";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@PropertyName",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = propertyName;
            paras[1].Value = id;

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

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
    /// 温度报警规则相关业务逻辑
    /// </summary>
    public class TemperatureAlarmRuleBLL
    {
        #region  查询
        public static AsiatekPagedList<TemperatureAlarmRuleListModel> GetPagedTemperatureAlarmRules(TemperatureAlarmRuleSearchModel model, int userAffiliatedStrucID, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","TemperatureAlarmRules tar"),
                new SqlParameter("@joinStr",@"INNER JOIN Func_GetStrucAndSubStrucByUserAffiliatedStrucID("+userAffiliatedStrucID+") struc ON tar.AffiliatedStrucID=struc.ID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","tar.ID DESC"),
                new SqlParameter("@showColumns",@"tar.ID,tar.Name,struc.StrucName AS AffiliatedStrucName"),
            };

            string conditionStr = "tar.Name LIKE '%" + model.Name + "%'";
            if (model.AffiliatedStrucID != -1)
            {
                conditionStr += " AND struc.ID = " + model.AffiliatedStrucID + "";
            }
            paras.Add(new SqlParameter("@conditionStr", conditionStr));
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
            List<TemperatureAlarmRuleListModel> list = ConvertToList<TemperatureAlarmRuleListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region 新增
        //        public static OperationResult AddTemperatureAlarmRule(TemperatureAlarmRuleAddModel model)
        //        {
        //            //规则基础数据1+时段数据1+明细数据n
        //            int count = 2 + model.TemperatureAlarmRuleDetails.Count;
        //            string[] sqls = new string[count];
        //            SqlParameter[][] paras = new SqlParameter[count][];

        //            #region 新增温度报警规则
        //            sqls[0] = @"INSERT INTO dbo.TemperatureAlarmRules
        //        ( Name ,
        //          AffiliatedStrucID ,
        //          CreateUserID,
        //          Description
        //        )
        //VALUES  ( @Name , -- Name - nvarchar(50)
        //          @AffiliatedStrucID , -- AffiliatedStrucID - int
        //          @CreateUserID ,-- CreateUserID - int
        //          @Description
        //        );SELECT SCOPE_IDENTITY();";


        //            paras[0] = new SqlParameter[4];
        //            paras[0][0] = new SqlParameter()
        //            {
        //                ParameterName = "@Name",
        //                SqlDbType = SqlDbType.NVarChar,
        //                Value = model.Name
        //            };
        //            paras[0][1] = new SqlParameter()
        //            {
        //                ParameterName = "@AffiliatedStrucID",
        //                SqlDbType = SqlDbType.Int,
        //                Value = model.AffiliatedStrucID
        //            };
        //            paras[0][2] = new SqlParameter()
        //            {
        //                ParameterName = "@CreateUserID",
        //                SqlDbType = SqlDbType.Int,
        //                Value = model.CreateUserID
        //            };

        //            if (string.IsNullOrWhiteSpace(model.Description))
        //            {
        //                paras[0][3].Value = DBNull.Value;
        //            }
        //            else
        //            {
        //                paras[0][3].Value = model.Description;
        //            }
        //            #endregion

        //            #region 新增报警规则时段数据
        //            sqls[1] = @"INSERT INTO dbo.TemperatureAlarmRuleTimePeriods
        //        ( TemperatureAlarmRuleID ,
        //          StartTime ,
        //          EndTime
        //        )
        //VALUES  ( @TemperatureAlarmRuleID , -- TemperatureAlarmRuleID - int
        //          @StartTime , -- StartTime - time
        //          @EndTime  -- EndTime - time
        //        ) ";
        //            paras[1] = new SqlParameter[3];
        //            paras[0][0] = new SqlParameter()
        //            {
        //                ParameterName = "@TemperatureAlarmRuleID",
        //                SqlDbType = SqlDbType.Int,
        //            };
        //            paras[0][1] = new SqlParameter()
        //            {
        //                ParameterName = "@StartTime",
        //                SqlDbType = SqlDbType.Time,
        //                Value = model.StartTime
        //            };
        //            paras[0][2] = new SqlParameter()
        //            {
        //                ParameterName = "@EndTime",
        //                SqlDbType = SqlDbType.Time,
        //                Value = model.EndTime
        //            };


        //            #endregion

        //            #region 新增报警规则的明细
        //            for (int i = 0; i < model.TemperatureAlarmRuleDetails.Count; i++)
        //            {
        //                var item = model.TemperatureAlarmRuleDetails[i];
        //                int index = i + 2;//开始是第三条语句
        //                sqls[index] = @"INSERT INTO dbo.TemperatureAlarmRuleDetails
        //        ( TemperatureAlarmRuleID ,
        //          SensorCode ,
        //          InstallationPosition ,
        //          LowestTemperature ,
        //          HighestTemperature
        //        )
        //VALUES  ( @TemperatureAlarmRuleID , -- TemperatureAlarmRuleID - int
        //          @SensorCode , -- SensorCode - int
        //          @InstallationPosition, -- InstallationPosition - nvarchar(50)
        //          @LowestTemperature , -- LowestTemperature - float
        //          @HighestTemperature  -- HighestTemperature - float
        //        )";
        //                paras[index] = new SqlParameter[5];
        //                paras[index][0] = new SqlParameter { ParameterName = "@TemperatureAlarmRuleID", SqlDbType = SqlDbType.Int };


        //                paras[index][1] = new SqlParameter()
        //                {
        //                    ParameterName = "@SensorCode",
        //                    SqlDbType = SqlDbType.Int,
        //                    Value = item.SensorCode
        //                };
        //                paras[index][2] = new SqlParameter()
        //                {
        //                    ParameterName = "@InstallationPosition",
        //                    SqlDbType = SqlDbType.NVarChar,
        //                    Value = item.InstallationPosition
        //                };
        //                paras[index][3] = new SqlParameter()
        //                {
        //                    ParameterName = "@LowestTemperature",
        //                    SqlDbType = SqlDbType.Float,
        //                    Value = item.LowestTemperature
        //                };
        //                paras[index][4] = new SqlParameter()
        //                {
        //                    ParameterName = "@HighestTemperature",
        //                    SqlDbType = SqlDbType.Float,
        //                    Value = item.HighestTemperature
        //                };
        //            }
        //            #endregion


        //            bool result = MSSQLHelper.ExecuteIdentityIncludeTransaction(CommandType.Text, sqls, paras) != 0;
        //            return new OperationResult()
        //            {
        //                Success = result,
        //                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
        //            };
        //        }


        public static OperationResult AddTemperatureAlarmRule(TemperatureAlarmRuleAddModel model)
        {
            //获取有效的报警配置详细数据，安装位置为空的表示没设置
            var effectiveDetails = (from item in model.TemperatureAlarmRuleDetails where !string.IsNullOrWhiteSpace(item.InstallationPosition) select item).ToList();
            //规则基础数据1+时段数据1+有效明细数据n
            int count = 2 + effectiveDetails.Count;
            string[] sqls = new string[count];
            SqlParameter[][] paras = new SqlParameter[count][];

            #region 新增温度报警规则
            sqls[0] = @"INSERT INTO dbo.TemperatureAlarmRules
        ( Name ,
          AffiliatedStrucID ,
          CreateUserID,
          Description
        )
VALUES  ( @Name , 
          @AffiliatedStrucID ,
          @CreateUserID ,
          @Description
        );SELECT SCOPE_IDENTITY();";


            paras[0] = new SqlParameter[4];
            paras[0][0] = new SqlParameter()
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.NVarChar,
                Value = model.Name.Trim()
            };
            paras[0][1] = new SqlParameter()
            {
                ParameterName = "@AffiliatedStrucID",
                SqlDbType = SqlDbType.Int,
                Value = model.AffiliatedStrucID
            };
            paras[0][2] = new SqlParameter()
            {
                ParameterName = "@CreateUserID",
                SqlDbType = SqlDbType.Int,
                Value = model.CreateUserID
            };

            paras[0][3] = new SqlParameter()
            {
                ParameterName = "@Description",
                SqlDbType = SqlDbType.NVarChar,
                Value = string.IsNullOrWhiteSpace(model.Description) ? (object)DBNull.Value : model.Description
            };
            #endregion

            #region 新增报警规则时段数据
            sqls[1] = @"INSERT INTO dbo.TemperatureAlarmRuleTimePeriods
        ( TemperatureAlarmRuleID ,
          StartTime ,
          EndTime
        )
VALUES  ( @TemperatureAlarmRuleID , -- TemperatureAlarmRuleID - int
          @StartTime , -- StartTime - time
          @EndTime  -- EndTime - time
        ) ";
            paras[1] = new SqlParameter[3];
            paras[1][0] = new SqlParameter()
            {
                ParameterName = "@TemperatureAlarmRuleID",
                SqlDbType = SqlDbType.Int,
            };
            paras[1][1] = new SqlParameter()
            {
                ParameterName = "@StartTime",
                SqlDbType = SqlDbType.Time,
                Value = model.StartTime
            };
            paras[1][2] = new SqlParameter()
            {
                ParameterName = "@EndTime",
                SqlDbType = SqlDbType.Time,
                Value = model.EndTime
            };


            #endregion

            #region 新增报警规则的明细
            for (int i = 0; i < effectiveDetails.Count; i++)
            {
                var item = effectiveDetails[i];
                int index = i + 2;//开始是第三条语句
                sqls[index] = @"INSERT INTO dbo.TemperatureAlarmRuleDetails
        ( TemperatureAlarmRuleID ,
          SensorCode ,
          InstallationPosition ,
          LowestTemperature ,
          HighestTemperature,
          Enabled
        )
VALUES  ( @TemperatureAlarmRuleID , -- TemperatureAlarmRuleID - int
          @SensorCode , -- SensorCode - int
          @InstallationPosition, -- InstallationPosition - nvarchar(50)
          @LowestTemperature , -- LowestTemperature - float
          @HighestTemperature,  -- HighestTemperature - float
          @Enabled
        )";
                paras[index] = new SqlParameter[6];
                paras[index][0] = new SqlParameter { ParameterName = "@TemperatureAlarmRuleID", SqlDbType = SqlDbType.Int };


                paras[index][1] = new SqlParameter()
                {
                    ParameterName = "@SensorCode",
                    SqlDbType = SqlDbType.Int,
                    Value = item.SensorCode
                };
                paras[index][2] = new SqlParameter()
                {
                    ParameterName = "@InstallationPosition",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = item.InstallationPosition.Trim()
                };
                paras[index][3] = new SqlParameter()
                {
                    ParameterName = "@LowestTemperature",
                    SqlDbType = SqlDbType.Float,
                    Value = item.LowestTemperature
                };
                paras[index][4] = new SqlParameter()
                {
                    ParameterName = "@HighestTemperature",
                    SqlDbType = SqlDbType.Float,
                    Value = item.HighestTemperature
                };
                paras[index][5] = new SqlParameter()
                {
                    ParameterName = "@Enabled",
                    SqlDbType = SqlDbType.Bit,
                    Value = item.Enabled
                };
            }
            #endregion


            bool result = MSSQLHelper.ExecuteIdentityIncludeTransaction(CommandType.Text, sqls, paras) != 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        public static bool CheckTemperatureAlarmRuleNameExists(string name)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Name",SqlDbType.NVarChar),
            };
            paras[0].Value = name.Trim();
            string sql = "SELECT COUNT(0) FROM TemperatureAlarmRules WHERE Name=@Name";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion

        #region 完全编辑
        public static bool CheckTemperatureAlarmRuleNameExists(string name, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@Name",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = name.Trim();
            paras[1].Value = id;
            string sql = "SELECT COUNT(0) FROM TemperatureAlarmRules WHERE Name=@Name AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;

        }


        public static SelectResult<TemperatureAlarmRuleEditModel> GetTemperatureAlarmRuleByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT tar.ID,tar.Name,tar.[Description],tar.AffiliatedStrucID,struc.StrucName AS AffiliatedStrucName,
CAST(tarp.StartTime AS CHAR(8)) AS StartTime,CAST(tarp.EndTime AS CHAR(8)) AS EndTime,
tard.SensorCode,tard.InstallationPosition,tard.LowestTemperature,tard.HighestTemperature,tard.[Enabled],s.SensorName
FROM dbo.TemperatureAlarmRules tar
INNER JOIN dbo.TemperatureAlarmRuleDetails tard ON tar.ID=tard.TemperatureAlarmRuleID
INNER JOIN dbo.TemperatureAlarmRuleTimePeriods tarp ON tar.ID=tarp.TemperatureAlarmRuleID
INNER JOIN dbo.Structures struc ON tar.AffiliatedStrucID=struc.ID
INNER JOIN dbo.SensorList s ON s.SensorCode=tard.SensorCode
WHERE tar.ID=@ID
ORDER BY tard.SensorCode ASC";
            var list = ConvertToList<TemperatureAlarmRuleEditFlatModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));


            TemperatureAlarmRuleEditModel data = null;
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
                var result = from item in list
                             group item by new { item.ID, item.Name, item.Description, item.AffiliatedStrucID, item.AffiliatedStrucName, item.StartTime, item.EndTime } into groupedResult
                             select new TemperatureAlarmRuleEditModel
                             {
                                 ID = groupedResult.Key.ID,
                                 Name = groupedResult.Key.Name,
                                 Description = groupedResult.Key.Description,
                                 AffiliatedStrucID = groupedResult.Key.AffiliatedStrucID,
                                 AffiliatedStrucName = groupedResult.Key.AffiliatedStrucName,
                                 StartTime = groupedResult.Key.StartTime,
                                 EndTime = groupedResult.Key.EndTime,
                                 TemperatureAlarmRuleDetails = (from subItem in list
                                                                where subItem.ID == groupedResult.Key.ID
                                                                select new TemperatureAlarmRuleDetailModel
                                                                {
                                                                    Enabled = subItem.Enabled,
                                                                    SensorCode = subItem.SensorCode,
                                                                    SensorName = subItem.SensorName,
                                                                    HighestTemperature = subItem.HighestTemperature,
                                                                    LowestTemperature = subItem.LowestTemperature,
                                                                    InstallationPosition = subItem.InstallationPosition
                                                                }).ToList()
                             };
                data = result.FirstOrDefault();
            }
            return new SelectResult<TemperatureAlarmRuleEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditTemperatureAlarmRuleEdit(TemperatureAlarmRuleEditModel model)
        {
            //获取有效的报警配置详细数据，安装位置为空的表示没设置
            var effectiveDetails = (from item in model.TemperatureAlarmRuleDetails where !string.IsNullOrWhiteSpace(item.InstallationPosition) select item).ToList();
            //规则基础数据1+时段数据1+删除明细数据1+有效明细数据n
            int count = 3 + effectiveDetails.Count;
            string[] sqls = new string[count];
            SqlParameter[][] paras = new SqlParameter[count][];

            #region 新增温度报警规则
            sqls[0] = @"UPDATE dbo.TemperatureAlarmRules 
SET Name=@Name,AffiliatedStrucID=@AffiliatedStrucID,EditTime=GETDATE(),Description=@Description,EditUserID=@EditUserID WHERE ID=@ID";


            paras[0] = new SqlParameter[5];
            paras[0][0] = new SqlParameter()
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.NVarChar,
                Value = model.Name.Trim()
            };
            paras[0][1] = new SqlParameter()
            {
                ParameterName = "@AffiliatedStrucID",
                SqlDbType = SqlDbType.Int,
                Value = model.AffiliatedStrucID
            };
            paras[0][2] = new SqlParameter()
            {
                ParameterName = "@EditUserID",
                SqlDbType = SqlDbType.Int,
                Value = model.EditUserID
            };
            paras[0][3] = new SqlParameter()
            {
                ParameterName = "@Description",
                SqlDbType = SqlDbType.NVarChar,
                Value = string.IsNullOrWhiteSpace(model.Description) ? (object)DBNull.Value : model.Description
            };
            paras[0][4] = new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = model.ID
            };
            #endregion

            #region 删除后新增报警规则时段数据
            sqls[1] = @"
DELETE FROM TemperatureAlarmRuleTimePeriods WHERE TemperatureAlarmRuleID=@TemperatureAlarmRuleID
INSERT INTO dbo.TemperatureAlarmRuleTimePeriods
        ( TemperatureAlarmRuleID ,
          StartTime ,
          EndTime
        )
VALUES  ( @TemperatureAlarmRuleID , -- TemperatureAlarmRuleID - int
          @StartTime , -- StartTime - time
          @EndTime  -- EndTime - time
        ) ";
            paras[1] = new SqlParameter[3];
            paras[1][0] = new SqlParameter()
            {
                ParameterName = "@TemperatureAlarmRuleID",
                SqlDbType = SqlDbType.Int,
                Value = model.ID
            };
            paras[1][1] = new SqlParameter()
            {
                ParameterName = "@StartTime",
                SqlDbType = SqlDbType.Time,
                Value = model.StartTime
            };
            paras[1][2] = new SqlParameter()
            {
                ParameterName = "@EndTime",
                SqlDbType = SqlDbType.Time,
                Value = model.EndTime
            };


            #endregion

            #region 删除后新增报警规则的明细
            sqls[2] = "DELETE FROM dbo.TemperatureAlarmRuleDetails WHERE TemperatureAlarmRuleID=" + model.ID + "";
            for (int i = 0; i < effectiveDetails.Count; i++)
            {
                var item = effectiveDetails[i];
                int index = i + 3;//开始是第4️条语句
                sqls[index] = @"INSERT INTO dbo.TemperatureAlarmRuleDetails
        ( TemperatureAlarmRuleID ,
          SensorCode ,
          InstallationPosition ,
          LowestTemperature ,
          HighestTemperature,
          Enabled
        )
VALUES  ( @TemperatureAlarmRuleID , -- TemperatureAlarmRuleID - int
          @SensorCode , -- SensorCode - int
          @InstallationPosition, -- InstallationPosition - nvarchar(50)
          @LowestTemperature , -- LowestTemperature - float
          @HighestTemperature,  -- HighestTemperature - float
          @Enabled
        )";
                paras[index] = new SqlParameter[6];
                paras[index][0] = new SqlParameter { ParameterName = "@TemperatureAlarmRuleID", SqlDbType = SqlDbType.Int, Value = model.ID };


                paras[index][1] = new SqlParameter()
                {
                    ParameterName = "@SensorCode",
                    SqlDbType = SqlDbType.Int,
                    Value = item.SensorCode
                };
                paras[index][2] = new SqlParameter()
                {
                    ParameterName = "@InstallationPosition",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = item.InstallationPosition.Trim()
                };
                paras[index][3] = new SqlParameter()
                {
                    ParameterName = "@LowestTemperature",
                    SqlDbType = SqlDbType.Float,
                    Value = item.LowestTemperature
                };
                paras[index][4] = new SqlParameter()
                {
                    ParameterName = "@HighestTemperature",
                    SqlDbType = SqlDbType.Float,
                    Value = item.HighestTemperature
                };
                paras[index][5] = new SqlParameter()
                {
                    ParameterName = "@Enabled",
                    SqlDbType = SqlDbType.Bit,
                    Value = item.Enabled
                };
            }
            #endregion


            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }


        #endregion




        #region 删除
        /// <summary>
        /// 物理删除温度报警规则
        /// </summary>
        public static OperationResult DeleteTemperatureAlarmRulePhysically(int[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            //删除温度报警规则明细与时间区间后再删除报警规则
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.TemperatureAlarmRuleDetails WHERE TemperatureAlarmRuleID=@ID
DELETE FROM dbo.TemperatureAlarmRuleTimePeriods WHERE TemperatureAlarmRuleID=@ID
DELETE FROM dbo.TemperatureAlarmRules WHERE ID=@ID";
                paras[i] = new SqlParameter[1];
                paras[i][0] = new SqlParameter()
                {
                    ParameterName = "ID",
                    SqlDbType = SqlDbType.Int,
                    Value = ids[i]
                };
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 获取温度传感器
        public static List<TemperatureSensorModel> GetTemperatureSensors()
        {
            string sql = @"SELECT SensorName,SensorCode FROM dbo.SensorList
WHERE TypeCode='WD' ORDER BY SensorCode ASC";
            var dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql);
            var list = ConvertToList<TemperatureSensorModel>.Convert(dt);
            return list;
        }

        #endregion


        #region 温度报警规则分配
        public static AsiatekPagedList<VehicleTemperatureAlarmRulesListModel> GetPagedVehicleTemperatureAlarmRules
            (VehicleTemperatureAlarmRulesSearchModel model, int userAffiliatedStrucID, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.Vehicles v"),
                new SqlParameter("@joinStr",@"INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID("+userAffiliatedStrucID+@") vt ON vt.VID=v.ID
INNER JOIN dbo.Structures struc ON vt.StrucID=struc.ID
LEFT JOIN dbo.VehicleTemperatureAlarmRules vtar ON vt.VID=vtar.VehicleID
LEFT JOIN dbo.TemperatureAlarmRules tar ON tar.ID=vtar.TemperatureAlarmRuleID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","v.ID DESC"),
                new SqlParameter("@showColumns",@"v.ID AS VID,v.VehicleName,v.PlateNum,v.VIN,tar.Name AS RuleName,struc.StrucName "),
            };

            string conditionStr = "1=1";
            if (model.TemperatureAlarmRuleID != -1)
            {
                conditionStr += " AND vtar.TemperatureAlarmRuleID = " + model.TemperatureAlarmRuleID + "";
            }
            if (model.StrucID != -1)
            {
                conditionStr += " AND v.StrucID = " + model.StrucID + "";
            }
            if (!string.IsNullOrWhiteSpace(model.PlateNum))
            {
                conditionStr += " AND v.PlateNum LIKE '%" + model.PlateNum + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.VehicleName))
            {
                conditionStr += " AND v.VehicleName LIKE '%" + model.VehicleName + "%'";
            }

            paras.Add(new SqlParameter("@conditionStr", conditionStr));
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
            List<VehicleTemperatureAlarmRulesListModel> list = ConvertToList<VehicleTemperatureAlarmRulesListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }


        public static List<TemperatureAlarmRulesModel> GetTemperatureAlarmRules(int userStrucID)
        {
            string sql = @"SELECT tar.ID,tar.Name AS RuleName,struc.StrucName FROM dbo.TemperatureAlarmRules tar
INNER JOIN dbo.Func_GetStrucAndSubStrucByUserAffiliatedStrucID(" + userStrucID + ") struc ON tar.AffiliatedStrucID=struc.ID";
            var dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql);
            var list = ConvertToList<TemperatureAlarmRulesModel>.Convert(dt);
            return list;
        }

        public static OperationResult AllotTemperatureAlarmRules(string vehicleIDs, string ruleIDs)
        {
            var tempVehicleIDs = vehicleIDs.TrimEnd(',');
            string deleteSql = string.Format("DELETE FROM dbo.VehicleTemperatureAlarmRules WHERE VehicleID IN ({0});", tempVehicleIDs);
            string[] vehicleIDsArray = tempVehicleIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] ruleIDsArray = ruleIDs.TrimEnd(',').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string sql = @"INSERT INTO dbo.VehicleTemperatureAlarmRules
                            ( VehicleID ,
                              TemperatureAlarmRuleID
                            ) VALUES ";
            foreach (var vid in vehicleIDsArray)
            {
                foreach (var rid in ruleIDsArray)
                {
                    sql += string.Format("({0},'{1}'),", vid, rid);
                }
            }
            sql = sql.TrimEnd(',');
            var result = MSSQLHelper.ExecuteTransaction(CommandType.Text, deleteSql + sql);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.OperationFailure
            };

        }


        public static OperationResult CancelAllotingTemperatureAlarmRules(string vehicleIDs)
        {
            var tempVehicleIDs = vehicleIDs.TrimEnd(',');
            string deleteSql = string.Format("DELETE FROM dbo.VehicleTemperatureAlarmRules WHERE VehicleID IN ({0});", tempVehicleIDs);
            var result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, deleteSql) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.OperationFailure
            };

        }

        #endregion

    }
}

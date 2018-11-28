﻿using Asiatek.AjaxPager;
using Asiatek.Common;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Model.TerminalSetting;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Asiatek.BLL.MSSQL
{
    public class TerminalBLL
    {
        #region 查询
        //        public static AsiatekPagedList<TerminalListModel> GetPagedTerminals(TerminalSearchModel model, int searchPage, int pageSize)
        //        {
        //            List<SqlParameter> paras = new List<SqlParameter>()
        //            {
        //                new SqlParameter("@tableName","Terminals t"),
        //                new SqlParameter("@joinStr",@"INNER JOIN dbo.TerminalTypes tt ON t.TerminalTypeID = tt.ID 
        //INNER JOIN dbo.TerminalManufacturer tm ON tt.TerminalManufacturerID = tm.ID"),
        //                new SqlParameter("@pageSize",pageSize),
        //                new SqlParameter("@currentPage",searchPage),
        //                new SqlParameter("@orderBy","t.ID"),
        //                new SqlParameter("@showColumns",@"t.ID ,
        //        t.TerminalCode ,
        //        t.VehicleID,
        //        t.Status,
        //        t.SIMCode,
        //        tt.TerminalName,
        //        tm.ManufacturerName"),
        //            };

        //            string conditionStr = "t.[Status]<>9 AND t.[Status]<>8";//不查询删除和报废的
        //            if (!string.IsNullOrWhiteSpace(model.TerminalCode))
        //            {
        //                conditionStr += " AND t.TerminalCode LIKE '%" + model.TerminalCode + "%'";
        //            }

        //            if (model.TerminalTypeID != -1)//选择了具体的终端类型
        //            {
        //                conditionStr += " AND t.TerminalTypeID = " + model.TerminalTypeID + "";
        //            }
        //            else if (model.TerminalTypeID == -1 && model.TerminalManufacturerID != -1)//如果选择了具体的厂商，未选具体终端类型
        //            {
        //                conditionStr += " AND tm.ID = " + model.TerminalManufacturerID + "";
        //            }

        //            if (!string.IsNullOrWhiteSpace(conditionStr))
        //            {
        //                paras.Add(new SqlParameter("@conditionStr", conditionStr));
        //            }

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
        //            List<TerminalListModel> list = ConvertToList<TerminalListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
        //            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
        //            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
        //            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        //        }

        /// <summary>
        /// 列表增加车架号列 查询条件增加车架号搜索
        /// </summary>
        /// <param name="model"></param>
        /// <param name="searchPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static AsiatekPagedList<TerminalListModel> GetPagedTerminals(TerminalSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
                    {
                        new SqlParameter("@tableName","Terminals t"),
                        new SqlParameter("@joinStr",@"INNER JOIN dbo.TerminalTypes tt ON t.TerminalTypeID = tt.ID 
                                                                             INNER JOIN dbo.TerminalManufacturer tm ON tt.TerminalManufacturerID = tm.ID 
                                                                             LEFT JOIN Vehicles AS ve  ON t.LinkedVehicleID = ve.ID
                                                                             left JOIN SimCodeList AS si ON si.ID = t.SimCodeID"),
                        new SqlParameter("@pageSize",pageSize),
                        new SqlParameter("@currentPage",searchPage),
                        new SqlParameter("@orderBy","t.ID DESC"),
                        new SqlParameter("@showColumns",@"t.ID , t.TerminalCode ,t.VehicleID, t.Status,si.SimCode,t.LinkedVehicleID,
                                                           tt.TerminalName, tm.ManufacturerName,ve.VIN"),
                    };

            string conditionStr = "t.[Status]<>9 AND t.[Status]<>8";//不查询删除和报废的
            if (!string.IsNullOrWhiteSpace(model.TerminalCode))
            {
                conditionStr += " AND t.TerminalCode LIKE '%" + model.TerminalCode + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.VIN))
            {
                conditionStr += " AND ve.VIN LIKE '%" + model.VIN + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.VehicleID))
            {
                conditionStr += " AND t.VehicleID LIKE '%" + model.VehicleID + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.SIMCode))
            {
                conditionStr += " AND si.SimCode LIKE '%" + model.SIMCode + "%'";
            }

            if (model.TerminalTypeID != -1)//选择了具体的终端类型
            {
                conditionStr += " AND t.TerminalTypeID = " + model.TerminalTypeID + "";
            }
            else if (model.TerminalTypeID == -1 && model.TerminalManufacturerID != -1)//如果选择了具体的厂商，未选具体终端类型
            {
                conditionStr += " AND tm.ID = " + model.TerminalManufacturerID + "";
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
            List<TerminalListModel> list = ConvertToList<TerminalListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }


        /// <summary>
        /// 根据车辆ID获取可用的终端列表
        /// 包含所有状态为0的终端数据
        /// 以及当前车辆的主定位、辅助定位终端
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <returns></returns>
        //        public static List<TerminalDDLForEditVehicleModel> GetUsefulTerminals(long vehicleID)
        //        {
        //            string sql = @"SELECT ID,TerminalCode, CAST(1 AS BIT) AS IsPrimary FROM dbo.Terminals WHERE Status=0
        //  UNION 
        //  SELECT t.ID,t.TerminalCode,vt.IsPrimary FROM dbo.VehiclesTerminals vt 
        //  INNER JOIN dbo.Terminals t ON vt.TerminalID=t.ID
        //  WHERE vt.VehicleID=@vehicleID";
        //            SqlParameter[] paras = new SqlParameter[1];
        //            paras[0] = new SqlParameter()
        //            {
        //                ParameterName = "@vehicleID",
        //                SqlDbType = SqlDbType.BigInt,
        //                Value = vehicleID
        //            };
        //            return ConvertToList<TerminalDDLForEditVehicleModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        //        }


        //车辆编辑时获取终端号信息 （包含原本绑定的终端号）
        public static List<TerminalDDLForEditVehicleModel> GetUsefulTerminals(long vehicleID, string terminalCode)
        {
            string sql = @"  SELECT ID,TerminalCode FROM dbo.Terminals WHERE Status=0  
  UNION
  SELECT tt.ID,tt.TerminalCode FROM dbo.Terminals tt INNER JOIN dbo.Vehicles ve ON ve.ID = tt.LinkedVehicleID WHERE tt.LinkedVehicleID=@LinkedVehicleID AND tt.TerminalCode=@TerminalCode";
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@LinkedVehicleID",
                SqlDbType = SqlDbType.BigInt,
                Value = vehicleID
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@TerminalCode",
                SqlDbType = SqlDbType.NVarChar,
                Value = terminalCode
            };
            return ConvertToList<TerminalDDLForEditVehicleModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }

        /// <summary>
        /// 获取可以使用的终端下拉数据
        /// </summary>
        /// <returns></returns>
        public static List<TerminalDDLModel> GetUsefulTerminals()
        {
            string sql = "SELECT ID,TerminalCode FROM dbo.Terminals WHERE Status=0";
            return ConvertToList<TerminalDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        /// <summary>
        /// 获取可以使用的终端下拉数据
        /// </summary>
        /// <returns></returns>
        public static List<TerminalDDLModel> GetNewUsefulTerminals(long vehicleID)
        {
            string sql = string.Empty;
            if (vehicleID > 0)
            {
                sql = string.Format(@"SELECT ID,TerminalCode FROM dbo.Terminals WHERE Status=0  UNION
                              SELECT tt.ID,tt.TerminalCode FROM dbo.Terminals tt INNER JOIN dbo.Vehicles ve ON ve.ID = tt.LinkedVehicleID 
                              WHERE  tt.LinkedVehicleID = {0}", vehicleID);
                //                sql = string.Format(@"SELECT tt.ID,tt.TerminalCode FROM dbo.Terminals tt INNER JOIN dbo.Vehicles ve ON ve.ID = tt.LinkedVehicleID 
                //                              WHERE  tt.LinkedVehicleID = {0}", vehicleID);
            }
            else
            {
                sql = "SELECT ID,TerminalCode FROM dbo.Terminals WHERE Status=0";
            }

            return ConvertToList<TerminalDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion


        #region 报废
        /// <summary>
        /// 根据终端ID批量报废非使用中的终端
        /// 设置Status为8
        /// </summary>
        public static OperationResult ScrapTerminal(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.Terminals SET Status=8 WHERE Status<>7 AND ID=@ID";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.BigInt);
                temp.Value = ids[i];
                paras[i][0] = temp;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region 新增
        public static bool CheckTerminalCodeExists(string terminalCode)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@terminalCode",SqlDbType.NVarChar,30),
            };
            paras[0].Value = terminalCode.Trim();
            string sql = "SELECT COUNT(0) FROM dbo.Terminals WHERE terminalCode=@terminalCode";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查SIM卡号是否重复
        /// </summary>
        public static bool CheckSIMCodeExists(string SIMCode)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SIMCode",SqlDbType.VarChar,13),
            };
            paras[0].Value = SIMCode.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.Terminals WHERE SIMCode=@SIMCode";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查车辆标识（终端内置车牌号）是否重复
        /// </summary>
        public static bool CheckVehicleIDExists(string vehicleID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleID",SqlDbType.NVarChar,30),
            };
            paras[0].Value = vehicleID.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.Terminals WHERE VehicleID=@VehicleID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        public static OperationResult AddTerminal(TerminalAddModel model)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar,30),
                new SqlParameter("@SimCodeID",SqlDbType.BigInt),
                new SqlParameter("@TerminalTypeID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@VehicleID",SqlDbType.NVarChar,30),
                new SqlParameter("@ServerInfoID",SqlDbType.Int),
                new SqlParameter("@OverspeedThreshold",SqlDbType.BigInt),
                new SqlParameter("@MinimumDuration",SqlDbType.BigInt),
                new SqlParameter("@ContinuousDrivingThreshold",SqlDbType.BigInt),
                new SqlParameter("@MinimumBreakTime",SqlDbType.BigInt),
                new SqlParameter("@MaximumParkingTime",SqlDbType.BigInt),
                new SqlParameter("@DrivingTimeThreshold",SqlDbType.BigInt),
                new SqlParameter("@CreateUserID",SqlDbType.Int)
                
            };

            paras[0].Value = model.TerminalCode.Trim();
            paras[1].Value = model.SIMCodeID;
            paras[2].Value = model.TerminalTypeID;

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[3].Value = DBNull.Value;
            }
            else
            {
                paras[3].Value = model.Remark;
            }

            paras[4].Value = model.VehicleID.Trim();
            paras[5].Value = model.ServerInfoID;
            paras[6].Value = model.OverspeedThreshold;
            paras[7].Value = model.MinimumDuration;
            paras[8].Value = model.ContinuousDrivingThreshold;
            paras[9].Value = model.MinimumBreakTime;
            paras[10].Value = model.MaximumParkingTime;
            paras[11].Value = model.DrivingTimeThreshold;
            paras[12].Value = model.CreateUserID;

            #region  SQL

            //            string sql = @"INSERT INTO dbo.Terminals( TerminalCode ,SimCodeID,VehicleID,ServerInfoID,
            //                                   TerminalTypeID ,Remark ,Status,OverspeedThreshold,MinimumDuration,
            //                                   ContinuousDrivingThreshold,MinimumBreakTime,MaximumParkingTime,DrivingTimeThreshold,CreateUserID)
            //                                 VALUES (@TerminalCode ,@SimCodeID,@VehicleID,@ServerInfoID,@TerminalTypeID ,
            //                                    @Remark ,0,@OverspeedThreshold,@MinimumDuration,@ContinuousDrivingThreshold,@MinimumBreakTime,
            //                                    @MaximumParkingTime,@DrivingTimeThreshold,@CreateUserID);";

            // 关联SIM卡的时候在查询一遍是为了防止同时打开两个页面 添加了一个终端后另外一个终端关联的还是此SIM卡的情况
            // 无法防止并发，但是并发几乎可忽略
            string sql = @"INSERT INTO dbo.Terminals( TerminalCode ,SimCodeID,VehicleID,ServerInfoID,
                                   TerminalTypeID ,Remark ,Status,OverspeedThreshold,MinimumDuration,
                                   ContinuousDrivingThreshold,MinimumBreakTime,MaximumParkingTime,DrivingTimeThreshold,CreateUserID)
                                 VALUES (@TerminalCode ,
                                     (SELECT  [ID] FROM [dbo].[SimCodeList] WHERE ID = @SimCodeID AND Status = 0),@VehicleID,@ServerInfoID,@TerminalTypeID ,
                                    @Remark ,0,@OverspeedThreshold,@MinimumDuration,@ContinuousDrivingThreshold,@MinimumBreakTime,
                                    @MaximumParkingTime,@DrivingTimeThreshold,@CreateUserID);";

            // 修改SIM卡状态为已使用
            sql += "UPDATE [dbo].[SimCodeList] SET [Status] = 1 WHERE [ID] = @SimCodeID";
            #endregion


            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, paras.ToArray());
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region 编辑
        /// <summary>
        /// 检查终端编号是否已存在
        /// </summary>
        public static bool CheckTerminalCodeExists(string terminalCode, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@terminalCode",SqlDbType.NVarChar,30),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = terminalCode.Trim();
            paras[1].Value = id;


            string sql = "SELECT COUNT(0) FROM dbo.Terminals WHERE terminalCode=@terminalCode AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        /// <summary>
        /// 检查SIM卡号是否重复
        /// </summary>
        public static bool CheckSIMCodeExists(string SIMCode, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SIMCode",SqlDbType.VarChar,13),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = SIMCode.Trim();
            paras[1].Value = id;

            string sql = "SELECT COUNT(0) FROM dbo.Terminals WHERE SIMCode=@SIMCode AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }


        /// <summary>
        /// 检查车辆标识（终端内置车牌号）是否重复
        /// </summary>
        public static bool CheckVehicleIDExists(string vehicleID, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleID",SqlDbType.NVarChar,30),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = vehicleID.Trim();
            paras[1].Value = id;

            string sql = "SELECT COUNT(0) FROM dbo.Terminals WHERE VehicleID=@VehicleID AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }



        public static SelectResult<TerminalEditModel> GetTerminalByID(long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = id;
            string sql = @"SELECT t.ID ,t.TerminalCode ,t.VehicleID,t.TerminalTypeID ,t.Remark,tt.TerminalManufacturerID,
                                    t.ServerInfoID,OverspeedThreshold,MinimumDuration,ContinuousDrivingThreshold ,MinimumBreakTime
                                    ,DrivingTimeThreshold ,MaximumParkingTime,t.SimCodeID,si.SimCode FROM   dbo.Terminals t  
                                    INNER JOIN dbo.TerminalTypes tt ON t.TerminalTypeID=tt.ID 
                                    Left JOIN SimCodeList AS si ON si.ID = t.SimCodeID
                                    WHERE   t.[Status] <> 9 AND t.[Status] <> 8 AND t.ID=@ID";
            List<TerminalEditModel> list = ConvertToList<TerminalEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            TerminalEditModel data = null;
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
            return new SelectResult<TerminalEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditTerminal(TerminalEditModel model)
        {

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar,30),
                new SqlParameter("@SIMCodeID",SqlDbType.BigInt),
                new SqlParameter("@TerminalTypeID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@ID",SqlDbType.BigInt),
                new SqlParameter("@VehicleID",SqlDbType.NVarChar,30),
                //new SqlParameter("@OverspeedThreshold",SqlDbType.BigInt),
                //new SqlParameter("@MinimumDuration",SqlDbType.BigInt),
                //new SqlParameter("@ContinuousDrivingThreshold",SqlDbType.BigInt),
                //new SqlParameter("@MinimumBreakTime",SqlDbType.BigInt),
                //new SqlParameter("@MaximumParkingTime",SqlDbType.BigInt),
                //new SqlParameter("@DrivingTimeThreshold",SqlDbType.BigInt),
                new SqlParameter("@EditUserID",SqlDbType.Int),
                new SqlParameter("@ServerInfoID",SqlDbType.Int),
                new SqlParameter("@SIMCodeIDOld",SqlDbType.BigInt)
            };

            paras[0].Value = model.TerminalCode.Trim();
            paras[1].Value = model.SIMCodeID;
            paras[2].Value = model.TerminalTypeID;

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[3].Value = DBNull.Value;
            }
            else
            {
                paras[3].Value = model.Remark;
            }
            paras[4].Value = model.ID;
            paras[5].Value = model.VehicleID.Trim();
            //paras[6].Value = model.OverspeedThreshold;
            //paras[7].Value = model.MinimumDuration;
            //paras[8].Value = model.ContinuousDrivingThreshold;
            //paras[9].Value = model.MinimumBreakTime;
            //paras[10].Value = model.MaximumParkingTime;
            //paras[11].Value = model.DrivingTimeThreshold;
            paras[6].Value = model.EditUserID;
            paras[7].Value = model.ServerInfoID;
            paras[8].Value = model.SIMCodeIDOld == null ? DBNull.Value : (object)model.SIMCodeIDOld;

            #region  SQL
            string sql = @"UPDATE  dbo.Terminals SET  TerminalCode = @TerminalCode ,SimCodeID=@SIMCodeID,TerminalTypeID = @TerminalTypeID ,
                                 Remark = @Remark,VehicleID=@VehicleID,ServerInfoID = @ServerInfoID,EditUserID=@EditUserID,
                                 EditTime=GETDATE() WHERE   ID = @ID;";



            #region 修改成功后 如果更改了sim卡 则原来的卡变成未使用（0） 现在的卡变成已使用（1）

            if (model.SIMCodeIDOld != model.SIMCodeID)
            {
                // 如果修改的时候没改改变SIM卡则只需要使用上面的sql 如果改变了 则使用下面的sql
                // 为什么不能直接把这个sql放在上面呢 因为如果没有改变的时候也查询一遍 那么那个SIM卡的状态肯定不是0，这样修改的时候
                //如果不修改关联的SIM卡就会报错 这显然不合符要求
                // 关联SIM卡的时候在查询一遍是为了防止同时打开两个页面 编辑了一个终端后另外一个终端关联的还是此SIM卡的情况
                // 无法防止并发，但是并发几乎可忽略
                //增加报废车机的判断，报废车机状态为6,如果状态是6则需要将status状态修改为0，如果是7则不需要
                sql = @"UPDATE  dbo.Terminals SET  TerminalCode = @TerminalCode ,
                                 SimCodeID= (SELECT  [ID] FROM [dbo].[SimCodeList] WHERE ID = @SimCodeID AND Status = 0),
                                 TerminalTypeID = @TerminalTypeID,Remark = @Remark,VehicleID=@VehicleID,ServerInfoID = @ServerInfoID,EditUserID=@EditUserID,Status=CASE WHEN Status=6 THEN 0 ELSE Status END,
                                 EditTime=GETDATE() WHERE   ID = @ID;";
                //                sql+=@"UPDATE  dbo.Terminals SET  TerminalCode = @TerminalCode ,
                //                                 SimCodeID= (SELECT  [ID] FROM [dbo].[SimCodeList] WHERE ID = @SimCodeID AND Status = 0),
                //                                 TerminalTypeID = @TerminalTypeID,Remark = @Remark,VehicleID=@VehicleID,ServerInfoID = @ServerInfoID,EditUserID=@EditUserID
                //                                 EditTime=GETDATE() WHERE   ID = @ID and Status=7;";
                sql += @"UPDATE [dbo].[SimCodeList] SET [Status] = 0 WHERE [ID] = @SIMCodeIDOld; 
                               UPDATE [dbo].[SimCodeList] SET [Status] = 1 WHERE [ID] = @SIMCodeID";

            }
            #endregion
            #endregion

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, paras.ToArray());
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region 导入车辆资料
        /// <summary>
        /// 根据车辆类型名获取车辆类型编码
        /// </summary>
        public static bool TryGetIDByTerminalCode(string terminalCode, out long id)
        {
            id = -1;
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@terminalCode",SqlDbType.NVarChar,30),
            };
            paras[0].Value = terminalCode;
            string sql = "SELECT ID FROM dbo.Terminals WHERE TerminalCode=@terminalCode AND Status=0";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)
            {
                return false;
            }
            id = Convert.ToInt64(result);
            return true;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 物理删除
        /// </summary>
        public static OperationResult DeleteTerminals(int[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                // 这里是物理删除 必须先跟新所有的SimCodeID
                // 删除的终端 必须不是在使用中 而且没有关联车辆
                sqls[i] = @"UPDATE dbo.SimCodeList SET Status=0 WHERE Status<>9 AND ID=(
                           SELECT TOP 1 SimCodeID FROM Terminals WHERE ID = @ID AND LinkedVehicleID IS NULL AND Status <> 7);";
                sqls[i] += @"DELETE FROM dbo.Terminals  WHERE LinkedVehicleID IS NULL AND ID=@ID AND Status <> 7";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.BigInt);
                temp.Value = ids[i];
                paras[i][0] = temp;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion
        #region 解除绑定信息(包括绑定车辆与绑定Sim)
        public static OperationResult UnBind(TerminalUsageLogs oldterminal, int userid)
        {
            //插入使用记录表
//            string sql = @"Update TerminalUsageLogs SET EndDateTime=getdate(),OverspeedThreshold=(select OverspeedThreshold from Terminals where ID=@TerminalID), 
//MinimumDuration=(select MinimumDuration from Terminals where ID=@TerminalID),ContinuousDrivingThreshold=(select ContinuousDrivingThreshold from Terminals where ID=@TerminalID),
//MinimumBreakTime=(select MinimumBreakTime from Terminals where ID=@TerminalID),DrivingTimeThreshold=(select DrivingTimeThreshold from Terminals where ID=@TerminalID),
//MaximumParkingTime=(select MaximumParkingTime from Terminals where ID=@TerminalID),LastMiles=(select Mileage from VW_GetRealTimeSignals where TID=@TerminalID)
//where TerminalID=@TerminalID and EndDateTime is null;";
            string sql = @"Update TerminalUsageLogs SET EndDateTime=getdate(),OverspeedThreshold=ter.OverspeedThreshold, 
MinimumDuration=ter.MinimumDuration,ContinuousDrivingThreshold=ter.ContinuousDrivingThreshold,
MinimumBreakTime=ter.MinimumBreakTime,DrivingTimeThreshold=ter.DrivingTimeThreshold,
MaximumParkingTime=ter.MaximumParkingTime,LastMiles=(select Mileage from VW_GetRealTimeSignals where TID=@TerminalID) from TerminalUsageLogs t,Terminals ter
where t.TerminalID=ter.ID and t.TerminalID=@TerminalID and t.EndDateTime is null ;";
            //解绑Sim卡
            sql += @"
 UPDATE dbo.SimCodeList SET Status=0 WHERE Status<>9 AND SimCode=@SimCode;
UPDATE  dbo.Terminals set   LinkedVehicleID = NULL,Status=6,SimCodeID=null,EditTime=getdate(),EditUserID=@UserID,VehicleID=null  where ID=@TerminalID;
 Update dbo.Vehicles set Status=7,UpdateUserID=@UserID,EditTime=getdate() where ID=@VehicleID";
            List<SqlParameter> paras = new List<SqlParameter>(){
                  new SqlParameter("@TerminalCode",SqlDbType.NVarChar),
                  new SqlParameter("@VehicleID",SqlDbType.Int),
                  new SqlParameter("@TerminalID",SqlDbType.Int),
                  new SqlParameter("@PlateNum",SqlDbType.NVarChar),
                  new SqlParameter("@VIN",SqlDbType.NVarChar),
                  new SqlParameter("@SimCode",SqlDbType.NVarChar),
                  new SqlParameter("@UserID",SqlDbType.Int)
            };
            paras[0].Value = oldterminal.TerminalCode;
            paras[1].Value = oldterminal.VehicleID;
            paras[2].Value = oldterminal.TerminalID;
            paras[3].Value = "";
            paras[4].Value = oldterminal.VIN;
            paras[5].Value = oldterminal.SimCode;
            paras[6].Value = userid;
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, paras.ToArray());
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.OperationFailure
            };
        }

        #endregion

        #region 当终端这是模块中下发终端参数成功后 修改对应的终端参数信息
        public static bool UpdateTerminals(List<string> listTerminalCode, TerminalSettingsSectionModel model)
        {

            string updateSql = string.Empty;
            if (model.最高速度.HasValue)
            {
                updateSql += "OverspeedThreshold =" + model.最高速度.Value + ",";
            }
            if (model.超速持续时间.HasValue)
            {
                updateSql += "MinimumDuration =" + model.超速持续时间.Value + ",";
            }
            if (model.连续驾驶时间门限.HasValue)
            {
                updateSql += "ContinuousDrivingThreshold =" + model.连续驾驶时间门限.Value + ",";
            }
            if (model.最小休息时间.HasValue)
            {
                updateSql += "MinimumBreakTime =" + model.最小休息时间.Value + ",";
            }
            if (model.当天累计驾驶时间门限.HasValue)
            {
                updateSql += "DrivingTimeThreshold =" + model.当天累计驾驶时间门限.Value + ",";
            }
            if (model.最长停车时间.HasValue)
            {
                updateSql += "MaximumParkingTime =" + model.最长停车时间.Value + ",";
            }

            if (string.IsNullOrEmpty(updateSql))
            {
                return true;
            }

            var terminalCodeCount = listTerminalCode.Count;
            SqlParameter[][] paras = new SqlParameter[terminalCodeCount][];
            string[] sqls = new string[terminalCodeCount];
            for (int i = 0; i < terminalCodeCount; i++)
            {
                sqls[i] = string.Format("{0}{1}{2}", "UPDATE dbo.Terminals SET ", updateSql.TrimEnd(','), " WHERE TerminalCode= '" + listTerminalCode[i] + "'");
            }
            return MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, null);

        }
        #endregion

        #region 导入
        public static OperationResult ImportTerminals(string excelFilePath, string sheetName, int createUserID)
        {
            bool success = false;
            string message = string.Empty;
            string brStr = "<br/>";
            var datas = ExcelHelper.ExcelToDataTable(excelFilePath, sheetName);
            if (datas == null)
            {
                message = PromptInformation.AccessExcelFailed;
            }
            else if (datas.Rows.Count == 0)
            {
                message = PromptInformation.NoTerminals;
            }
            else
            {
                //存储Sim卡号，用于判断DataTable中本身是否存在重复的Sim卡号
                List<string> SimCodes = new List<string>();
                //存储终端号，用于判断DataTable中本身是否存在重复的终端号
                List<string> TerminalCodes = new List<string>();
                //存储车辆标识，用于判断DataTable中本身是否存在重复的车辆标识
                List<string> VehicleIDs = new List<string>();


                //存储待新增的终端
                Dictionary<string, TerminalAddModel> addDic = new Dictionary<string, TerminalAddModel>();
                int i = 0;
                for (i = 0; i < datas.Rows.Count; i++)
                {
                    int rowNum = i + 2;
                    string rowMessage = string.Format(PromptInformation.RowIndex, rowNum);
                    TerminalAddModel addModel = new TerminalAddModel();
                    addModel.CreateUserID = createUserID;
                    List<string> errorMessage = new List<string>();
                    var currentRow = datas.Rows[i];
                    if (CheckImportTerminalInfo(currentRow, errorMessage, SimCodes, TerminalCodes, VehicleIDs, addModel))
                    {
                        addDic.Add(rowMessage, addModel);
                    }
                    else//只要有一条失败，马上返回提示，不继续检查
                    {
                        message = rowMessage + brStr;
                        errorMessage.ForEach(msg =>
                        {
                            message += string.Format("{0}{1}", msg, brStr);
                        });
                        break;
                    }
                }

                if (i == datas.Rows.Count)//全部都检查无误才对数据进行新增
                {
                    int failedCount = 0;
                    addDic.ToList().ForEach(kv =>
                    {
                        if (!AddTerminal(kv.Value).Success)
                        {
                            message += string.Format("{0}{1}", kv.Key, brStr);
                            failedCount++;
                        }
                    });
                    success = failedCount == 0;
                }
            }
            return new OperationResult()
            {
                Success = success,
                Message = success ? PromptInformation.ImportSuccess : PromptInformation.ImportFailed + brStr + message
            };
        }

        /// <summary>
        /// 检查导入的终端资料合法性
        /// </summary>
        private static bool CheckImportTerminalInfo(DataRow currentRow, List<string> errorMessages, List<string> SimCodes, List<string> TerminalCodes, List<string> VehicleIDs, TerminalAddModel model)
        {
            string fieldName = string.Empty;

            #region 获取当前行数据
            string TerminalCodeStr = currentRow[0].ToString().Trim();//                    终端号                           
            string SIMCodeStr = currentRow[1].ToString().Trim();//                 Sim卡号                     
            string VehicleIDStr = currentRow[2].ToString().Trim();//                  车辆标识                      
            string ServerInfoNameStr = currentRow[3].ToString().Trim();//               服务器名称
            string TerminalManufacturerNameStr = currentRow[4].ToString().Trim();//              车载终端厂商
            string TerminalTypeNameStr = currentRow[5].ToString().Trim();//               终端类型
            string OverspeedThresholdStr = currentRow[6].ToString().Trim();//                 超速阈值（km/h）
            string MinimumDurationStr = currentRow[7].ToString().Trim();//              引发超速报警所需的最小持续时间（s）
            string ContinuousDrivingThresholdStr = currentRow[8].ToString().Trim();//              连续驾驶时间门限（s）
            string MinimumBreakTimeStr = currentRow[9].ToString().Trim();//              最小休息时间（s）
            string DrivingTimeThresholdStr = currentRow[10].ToString().Trim();//              当天累计驾驶时间门限（s）
            string MaximumParkingTimeStr = currentRow[11].ToString().Trim();//              最长停车时间阈值（s）
            string RemarkStr = currentRow[12].ToString().Trim();//              备注
            #endregion

            Regex regInteger = new Regex(@"^[0-9]\d*$");

            #region 检查终端号
            fieldName = DisplayText.TerminalCode;

            if (string.IsNullOrWhiteSpace(TerminalCodeStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查终端号长度
            else if (TerminalCodeStr.Length > 30)
            {
                errorMessages.Add(string.Format(DataAnnotations.MaxLength, fieldName, 30));
            }
            //检查终端号在excel中是否重复
            else if (TerminalCodes.Exists(tc => tc == TerminalCodeStr))
            {
                errorMessages.Add(PromptInformation.SameTerminalInExcel + "：" + TerminalCodeStr);
            }
            //检查终端号在数据库中是否重复
            else if (CheckTerminalCodeExists(TerminalCodeStr))
            {
                errorMessages.Add(string.Format(DataAnnotations.FieldExists, fieldName));
            }
            else
            {
                TerminalCodes.Add(TerminalCodeStr);
            }
            #endregion

            #region 检查Sim卡号
            fieldName = DisplayText.SIMCode;
            Regex reg = new Regex(@"^([0-9]{11}|[0-9]{13})$");
            int simCodeId = -1;
            string errMsg = "";
            if (string.IsNullOrWhiteSpace(SIMCodeStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查Sim卡格式
            else if (!reg.IsMatch(SIMCodeStr))
            {
                errorMessages.Add(DataAnnotations.SIMCodeError);
            }
            //检查Sim卡在excel中是否重复
            else if (SimCodes.Exists(sc => sc == SIMCodeStr))
            {
                errorMessages.Add(PromptInformation.SameSIMInExcel + "：" + SIMCodeStr);
            }
            //检查Sim卡号是否存在和是否被使用验证
            else if (!CheckSIMCodeExistsAndNotUsed(SIMCodeStr, out errMsg))
            {
                errorMessages.Add(errMsg);
            }
            else
            {
                SimCodeBLL.TryGetIDByCode(SIMCodeStr, out simCodeId);
                SimCodes.Add(SIMCodeStr);
            }
            #endregion

            #region 检查车辆标识
            fieldName = DisplayText.VehicleID;

            if (string.IsNullOrWhiteSpace(VehicleIDStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查车辆标识长度
            else if (VehicleIDStr.Length > 30)
            {
                errorMessages.Add(string.Format(DataAnnotations.MaxLength, fieldName, 30));
            }
            //检查车辆标识在excel中是否重复
            else if (VehicleIDs.Exists(tc => tc == VehicleIDStr))
            {
                errorMessages.Add(PromptInformation.SameVehicleIDInExcel + "：" + VehicleIDStr);
            }
            //检查车辆标识在数据库中是否重复
            else if (CheckVehicleIDExists(VehicleIDStr))
            {
                errorMessages.Add(string.Format(DataAnnotations.FieldExists, fieldName));
            }
            else
            {
                VehicleIDs.Add(VehicleIDStr);
            }
            #endregion

            #region 服务器名称
            fieldName = DisplayText.ServerName;
            int ServerInfoValue = -1;
            if (string.IsNullOrWhiteSpace(ServerInfoNameStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查服务器名称合法性
            else if (!ServerManagerBLL.TryGetCodeByName(ServerInfoNameStr, out ServerInfoValue))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, ServerInfoNameStr, PromptInformation.NotExists));
            }
            #endregion

            #region 车载终端厂商
            fieldName = DisplayText.Manufacturer;
            int TerminalManufacturerValue = -1;
            if (string.IsNullOrWhiteSpace(TerminalManufacturerNameStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查车载终端厂商名称合法性
            else if (!TerminalManufacturerBLL.TryGetCodeByName(TerminalManufacturerNameStr, out TerminalManufacturerValue))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, TerminalManufacturerNameStr, PromptInformation.NotExists));
            }
            #endregion

            #region 终端名称
            fieldName = DisplayText.TerminalName;
            int TerminalTypeValue = -1;
            if (string.IsNullOrWhiteSpace(TerminalTypeNameStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查终端名称名称合法性
            else if (!TerminalTypeBLL.TryGetCodeByName(TerminalTypeNameStr, out TerminalTypeValue))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, TerminalTypeNameStr, PromptInformation.NotExists));
            }
            #endregion

            #region 超速阈值（km/h）
            fieldName = DisplayText.OverspeedThreshold;
            uint OverspeedThresholdValue = 0;
            if (string.IsNullOrWhiteSpace(OverspeedThresholdStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //数字验证
            else if (!uint.TryParse(OverspeedThresholdStr, out OverspeedThresholdValue))
            {
                errorMessages.Add(string.Format(PromptInformation.integerValidErr, fieldName));
            }
            //数字范围验证
            else if (!(OverspeedThresholdValue >= uint.MinValue && OverspeedThresholdValue <= uint.MaxValue))
            {
                errorMessages.Add(string.Format(DataAnnotations.RangeError, fieldName, uint.MinValue, uint.MaxValue));
            }
            #endregion

            #region 引发超速报警所需的最小持续时间（s）
            fieldName = DisplayText.MinimumDuration;
            uint MinimumDurationValue = 0;
            if (string.IsNullOrWhiteSpace(MinimumDurationStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //数字验证
            else if (!uint.TryParse(MinimumDurationStr, out MinimumDurationValue))
            {
                errorMessages.Add(string.Format(PromptInformation.integerValidErr, fieldName));
            }
            //数字范围验证
            else if (!(MinimumDurationValue >= uint.MinValue && MinimumDurationValue <= uint.MaxValue))
            {
                errorMessages.Add(string.Format(DataAnnotations.RangeError, fieldName, uint.MinValue, uint.MaxValue));
            }
            #endregion

            #region 连续驾驶时间门限（s）
            fieldName = DisplayText.ContinuousDrivingThreshold;
            uint ContinuousDrivingThresholdValue = 0;
            if (string.IsNullOrWhiteSpace(ContinuousDrivingThresholdStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //数字验证
            else if (!uint.TryParse(ContinuousDrivingThresholdStr, out ContinuousDrivingThresholdValue))
            {
                errorMessages.Add(string.Format(PromptInformation.integerValidErr, fieldName));
            }
            //数字范围验证
            else if (!(ContinuousDrivingThresholdValue >= uint.MinValue && ContinuousDrivingThresholdValue <= uint.MaxValue))
            {
                errorMessages.Add(string.Format(DataAnnotations.RangeError, fieldName, uint.MinValue, uint.MaxValue));
            }
            #endregion

            #region 最小休息时间（s）
            fieldName = DisplayText.MinimumBreakTime;
            uint MinimumBreakTimeValue = 0;
            if (string.IsNullOrWhiteSpace(MinimumBreakTimeStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //数字验证
            else if (!uint.TryParse(MinimumBreakTimeStr, out MinimumBreakTimeValue))
            {
                errorMessages.Add(string.Format(PromptInformation.integerValidErr, fieldName));
            }
            //数字范围验证
            else if (!(MinimumBreakTimeValue >= uint.MinValue && MinimumBreakTimeValue <= uint.MaxValue))
            {
                errorMessages.Add(string.Format(DataAnnotations.RangeError, fieldName, uint.MinValue, uint.MaxValue));
            }
            #endregion

            #region 当天累计驾驶时间门限（s）
            fieldName = DisplayText.DrivingTimeThreshold;
            uint DrivingTimeThresholdValue = 0;
            if (string.IsNullOrWhiteSpace(DrivingTimeThresholdStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //数字验证
            else if (!uint.TryParse(DrivingTimeThresholdStr, out DrivingTimeThresholdValue))
            {
                errorMessages.Add(string.Format(PromptInformation.integerValidErr, fieldName));
            }
            //数字范围验证
            else if (!(DrivingTimeThresholdValue >= uint.MinValue && DrivingTimeThresholdValue <= uint.MaxValue))
            {
                errorMessages.Add(string.Format(DataAnnotations.RangeError, fieldName, uint.MinValue, uint.MaxValue));
            }
            #endregion

            #region 最长停车时间阈值（s）
            fieldName = DisplayText.MaximumParkingTime;
            uint MaximumParkingTimeValue = 0;
            if (string.IsNullOrWhiteSpace(MaximumParkingTimeStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //数字验证
            else if (!uint.TryParse(MaximumParkingTimeStr, out MaximumParkingTimeValue))
            {
                errorMessages.Add(string.Format(PromptInformation.integerValidErr, fieldName));
            }
            //数字范围验证
            else if (!(MaximumParkingTimeValue >= uint.MinValue && MaximumParkingTimeValue <= uint.MaxValue))
            {
                errorMessages.Add(string.Format(DataAnnotations.RangeError, fieldName, uint.MinValue, uint.MaxValue));
            }
            #endregion

            model.TerminalCode = TerminalCodeStr;
            model.SIMCodeID = simCodeId;
            model.VehicleID = VehicleIDStr;
            model.ServerInfoID = ServerInfoValue;
            model.TerminalManufacturerID = TerminalManufacturerValue;
            model.TerminalTypeID = TerminalTypeValue;
            model.OverspeedThreshold = OverspeedThresholdValue;
            model.MinimumDuration = MinimumDurationValue;
            model.ContinuousDrivingThreshold = ContinuousDrivingThresholdValue;
            model.MinimumBreakTime = MinimumBreakTimeValue;
            model.DrivingTimeThreshold = DrivingTimeThresholdValue;
            model.MaximumParkingTime = MaximumParkingTimeValue;

            model.Remark = RemarkStr;

            return errorMessages.Count == 0;
        }

        public static bool CheckSIMCodeExistsAndNotUsed(string SIMCode, out string errMsg)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SIMCode",SqlDbType.VarChar,13),
            };
            paras[0].Value = SIMCode.Trim();


            string sql = @"declare @res int,@sameSimCnt int,@useCnt int
                                        select @res=0
                                        select  @sameSimCnt=count(1) from SimCodeList where SimCode=@SIMCode
                                        select @useCnt=count(1) from SimCodeList where SimCode=@SIMCode and Status=1
                                        if @sameSimCnt=0
                                          select @res=1
                                        else if @useCnt=1
                                          select @res=2
                                        select @res as res";
            int? result = Convert.ToInt32(MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray()));
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                errMsg = PromptInformation.DBError;
                return false;
            }
            else if (result == 0)
            {
                errMsg = "";
                return true;
            }
            else if (result == 1)
            {
                errMsg = PromptInformation.SimNotExist;
                return false;
            }
            else if (result == 2)
            {
                errMsg = PromptInformation.SimHasUsed;
                return false;
            }
            else
            {
                errMsg = PromptInformation.SimUnknownErr;
                return false;
            }
        }
        #endregion
    }
}

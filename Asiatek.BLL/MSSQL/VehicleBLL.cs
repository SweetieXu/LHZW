﻿using Asiatek.AjaxPager;
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
using System.Text.RegularExpressions;

namespace Asiatek.BLL.MSSQL
{
    public class VehicleBLL
    {
        #region 查询
        public static AsiatekPagedList<VehicleListModel> GetPagedVehicles(OLDVehicleSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","Vehicles v"),
                new SqlParameter("@joinStr",@"INNER JOIN dbo.TerminalTypes tt ON v.TerminalTypeID=tt.ID
INNER JOIN dbo.PlateColors pnc ON v.PlateColorCode=pnc.Code
INNER JOIN dbo.VehicleTypes vt ON v.VehicleTypeCode=vt.Code
INNER JOIN dbo.Structures s ON v.StrucID=s.ID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","v.CreateTime DESC"),
                new SqlParameter("@showColumns",@"  v.ID ,
        v.PlateNum ,
        v.TerminalCode ,
        v.VehicleName ,
        v.SIMCode ,
        tt.TerminalName ,
        s.StrucName ,
        pnc.Name AS PlateColor,
        vt.NAME AS VehicleType ,
        v.SpeedLimit ,
        v.IsTransmit ,
        v.WarrantyDate"),
            };

            #region 筛选条件
            string conditionStr = "v.[Status]<>9";
            if (!string.IsNullOrWhiteSpace(model.PlateNum))
            {
                conditionStr += " AND v.PlateNum LIKE '%" + model.PlateNum + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.TerminalCode))
            {

                conditionStr += " AND v.TerminalCode LIKE '%" + model.TerminalCode + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.SIMCode))
            {

                conditionStr += " AND v.SIMCode LIKE '%" + model.SIMCode + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.VehicleCode))
            {
                conditionStr += " AND v.VehicleCode LIKE '%" + model.VehicleCode + "%'";
            }


            if (model.TerminalTypeID != -1)
            {
                conditionStr += " AND v.TerminalTypeID = " + model.TerminalTypeID + "";
            }

            if (model.StrucID != -1)
            {

                conditionStr += " AND v.StrucID = " + model.StrucID + "";
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
            List<VehicleListModel> list = ConvertToList<VehicleListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        public static AsiatekPagedList<VehicleListModel> GetPagedVehicles(VehicleSearchModel model, int searchPage, int pageSize, int currentStrucID)
        {
            //            string joinStr = string.Empty;
            //            if (string.IsNullOrWhiteSpace(model.AuxiliaryTerminalCode))
            //            {
            //                joinStr = @"INNER JOIN dbo.PlateColors pc ON v.PlateColorCode = pc.Code
            //            INNER JOIN dbo.VehicleTypes vtype ON v.VehicleTypeCode = vtype.Code
            //            INNER JOIN dbo.VehiclesTerminals vt ON v.ID = vt.VehicleID
            //            INNER JOIN dbo.Terminals t ON vt.TerminalID = t.ID
            //            INNER JOIN dbo.TerminalTypes tt ON t.TerminalTypeID = tt.ID
            //            INNER JOIN dbo.Structures s ON v.StrucID = s.ID";
            //            }
            //            else
            //            {
            //                joinStr = @" INNER JOIN dbo.PlateColors pc ON v.PlateColorCode = pc.Code
            //            INNER JOIN dbo.VehicleTypes vtype ON v.VehicleTypeCode = vtype.Code
            //            INNER JOIN dbo.VehiclesTerminals vt ON v.ID = vt.VehicleID
            //            INNER JOIN dbo.Terminals t ON vt.TerminalID = t.ID
            //            INNER JOIN dbo.TerminalTypes tt ON t.TerminalTypeID = tt.ID
            //            INNER JOIN dbo.Structures s ON v.StrucID = s.ID
            //            INNER  JOIN ( SELECT    v.ID
            //                          FROM      dbo.VehiclesTerminals vt
            //                                    INNER JOIN dbo.Terminals t ON vt.TerminalID = t.ID
            //                                    INNER JOIN dbo.Vehicles v ON vt.VehicleID = v.ID
            //                          WHERE     vt.IsPrimary = 0
            //                                    AND t.TerminalCode LIKE '%" + model.AuxiliaryTerminalCode + "%' ) temp ON temp.ID = v.ID";
            //            }

            string joinStr = string.Format(@" LEFT JOIN dbo.PlateColors pc ON v.PlateColorCode = pc.Code
            LEFT JOIN dbo.VehicleTypes vtype ON v.VehicleTypeCode = vtype.Code
            LEFT JOIN dbo.Terminals t ON v.ID = t.LinkedVehicleID
            LEFT JOIN dbo.TerminalTypes tt ON t.TerminalTypeID = tt.ID
            INNER JOIN Func_GetStrucAndSubStrucByUserAffiliatedStrucID({0}) s ON v.StrucID = s.ID 
            LEFT JOIN dbo.SimCodeList scl ON t.SimCodeID=scl.ID 
LEFT JOIN (SELECT vei.VehicleID,ei.EmployeeName AS OwnersName FROM dbo.VehicleEmployeeInfo vei INNER JOIN dbo.EmployeeInfo ei ON ei.ID = vei.EmployeeInfoID WHERE vei.Type=3) tmp ON tmp.VehicleID = v.ID  ", currentStrucID);

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","Vehicles v"),
                new SqlParameter("@joinStr",joinStr),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","v.CreateTime DESC"),
                new SqlParameter("@showColumns",@"v.ID ,v.SoftwareDate,v.Status,
            v.PlateNum ,
            v.VehicleName ,
            pc.Name AS PlateColor ,
            vtype.Name AS VehicleType ,
            v.IsTransmit ,
            v.IsReceived,
            v.WarrantyDate ,
            scl.SimCode ,
            t.TerminalCode ,
            tt.TerminalName ,
            s.StrucName "),
            };

            #region 筛选条件
            //string conditionStr = "vt.IsPrimary = 1 AND v.[Status]<>9";
            string conditionStr = "v.[Status]<>9 ";
            if (!string.IsNullOrWhiteSpace(model.PlateNum))
            {
                conditionStr += " AND v.PlateNum LIKE '%" + model.PlateNum + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.PrimaryTerminalCode))
            {
                conditionStr += " AND t.TerminalCode LIKE '%" + model.PrimaryTerminalCode + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.PrimarySIMCode))
            {
                conditionStr += " AND scl.SimCode LIKE '%" + model.PrimarySIMCode + "%'";
            }

            if (model.PrimaryTerminalTypeID != -1)
            {
                conditionStr += " AND t.TerminalTypeID = " + model.PrimaryTerminalTypeID + "";
            }

            if (model.SearchStrucID != -1)
            {

                conditionStr += " AND v.StrucID = " + model.SearchStrucID + "";
            }
            if (!string.IsNullOrWhiteSpace(model.OwnersName))
            {
                conditionStr += " AND tmp.OwnersName LIKE '%" + model.OwnersName + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.VIN))
            {
                conditionStr += " AND v.VIN LIKE '%" + model.VIN + "%'";
            }


            paras.Add(new SqlParameter("@conditionStr", conditionStr));

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
            List<VehicleListModel> list = ConvertToList<VehicleListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion


        #region 修改保固期
        public static OperationResult ModifyWarrantyDate(VehicleModifyWarrantyDateModel model)
        {
            string[] ids = model.VehicleIDs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.Vehicles SET WarrantyDate=@WarrantyDate WHERE ID=@ID";

                paras[i] = new SqlParameter[2];
                SqlParameter sp1 = new SqlParameter("@ID", SqlDbType.Int);
                sp1.Value = Convert.ToInt32(ids[i]);
                paras[i][0] = sp1;

                SqlParameter sp2 = new SqlParameter("@WarrantyDate", SqlDbType.Date);
                sp2.Value = model.WarrantyDate;
                paras[i][1] = sp2;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region 修改限速值
        public static OperationResult ModifySpeedLimit(VehicleModifySpeedLimitModel model)
        {
            string[] ids = model.VehicleIDs.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.Vehicles SET SpeedLimit=@SpeedLimit WHERE ID=@ID";

                paras[i] = new SqlParameter[2];
                SqlParameter sp1 = new SqlParameter("@ID", SqlDbType.Int);
                sp1.Value = Convert.ToInt32(ids[i]);
                paras[i][0] = sp1;

                SqlParameter sp2 = new SqlParameter("@SpeedLimit", SqlDbType.Int);
                sp2.Value = model.SpeedLimit;
                paras[i][1] = sp2;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region 接入
        public static OperationResult ModifyAccess(string[] ids, bool isAccess)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.Vehicles SET IsAccess=" + (isAccess ? "1" : "0") + " WHERE ID=@ID";

                paras[i] = new SqlParameter[1];
                SqlParameter sp1 = new SqlParameter("@ID", SqlDbType.Int);
                sp1.Value = ids[i];
                paras[i][0] = sp1;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region 转发
        public static OperationResult ModifyTransmit(string[] ids, bool isTransmit)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.Vehicles SET IsTransmit=" + (isTransmit ? "1" : "0") + " WHERE ID=@ID";

                paras[i] = new SqlParameter[1];
                SqlParameter sp1 = new SqlParameter("@ID", SqlDbType.Int);
                sp1.Value = ids[i];
                paras[i][0] = sp1;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region 接受信号
        public static OperationResult ModifyReceive(string[] ids, bool isReceived)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.Vehicles SET IsReceived=" + (isReceived ? "1" : "0") + " WHERE ID=@ID";

                paras[i] = new SqlParameter[1];
                SqlParameter sp1 = new SqlParameter("@ID", SqlDbType.Int);
                sp1.Value = ids[i];
                paras[i][0] = sp1;
            }
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
        /// 删除车辆（逻辑删除）
        /// </summary>
        /// 1删除车辆
        /// 2更改原绑定终端的终端状态
        /// 3终端使用表中该绑定记录添加结束时间
        public static OperationResult DeleteVehicle(string[] ids)
        {
            int length = ids.Length * 3;
            string[] sqls = new string[length];
            SqlParameter[][] paras = new SqlParameter[length][];
            for (int i = 0; i < length; i = i + 3)
            {
                //删除车辆
                sqls[i] = @"UPDATE dbo.Vehicles SET [Status]=9 WHERE ID=@ID";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i / 3];
                paras[i][0] = temp;
                //更新终端状态
                sqls[i + 1] = @"UPDATE  dbo.Terminals  SET  LinkedVehicleID = null , Status=0  WHERE   LinkedVehicleID = @LinkedVehicleID";
                paras[i + 1] = new SqlParameter[1];
                paras[i + 1][0] = new SqlParameter()
                {
                    ParameterName = "@LinkedVehicleID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = ids[i / 3]
                };
                //终端使用表添加结束时间
                sqls[i + 2] = @"UPDATE  dbo.TerminalUsageLogs  SET EndDateTime = GETDATE()  WHERE  EndDateTime IS NULL AND VehicleID = @VehicleID";
                paras[i + 2] = new SqlParameter[1];
                paras[i + 2][0] = new SqlParameter()
                {
                    ParameterName = "@VehicleID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = ids[i / 3]
                };
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除车辆（物理删除）
        /// </summary>
        /// 1更改原绑定终端的终端状态
        /// 2 删除车辆   （删除车辆前 必须先解除之前所有与之相关的所有外键表数据）
        /// 3 终端使用表中该绑定记录添加结束时间
        public static OperationResult DeleteVehiclePhysical(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE  dbo.Terminals  SET  LinkedVehicleID = null , Status=0  WHERE   LinkedVehicleID = @ID;
                                    DELETE FROM dbo.Vehicles WHERE ID=@ID;
                                    UPDATE  dbo.TerminalUsageLogs  SET EndDateTime = GETDATE()  WHERE  EndDateTime IS NULL AND VehicleID = @ID";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.BigInt);
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
        /// 检查初始车牌是否重复
        /// </summary>
        public static bool CheckVehicleCodeExists(string vehicleCode)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@vehicleCode",SqlDbType.NVarChar,20),
            };
            paras[0].Value = vehicleCode.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VehicleCode=@vehicleCode";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查同一个使用单位下车代号是否重复
        /// </summary>
        public static bool CheckVehicleNameExists(string vehicleName, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@vehicleName",SqlDbType.NVarChar,20),
                new SqlParameter("@strucID",SqlDbType.Int),
            };
            paras[0].Value = vehicleName.Trim();
            paras[1].Value = strucID;


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VehicleName=@vehicleName AND StrucID=@strucID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }


        /// <summary>
        /// 检查终端号是否重复
        /// </summary>
        public static bool CheckTerminalCodeExists(string terminalCode)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@terminalCode",SqlDbType.NVarChar,20),
            };
            paras[0].Value = terminalCode.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE TerminalCode=@terminalCode";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查车牌号是否重复
        /// </summary>
        public static bool CheckPlateNumExists(string plateNum)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@plateNum",SqlDbType.NVarChar,8),
            };
            paras[0].Value = plateNum.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE PlateNum=@plateNum";
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
                new SqlParameter("@SIMCode",SqlDbType.Char,13),
            };
            paras[0].Value = SIMCode.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE SIMCode=@SIMCode";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查车架号是否重复
        /// </summary>
        public static bool CheckAddVINExists(string VIN)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VIN",SqlDbType.Char,17),
            };
            paras[0].Value = VIN.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VIN=@VIN";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        public static OperationResult AddVehicle(OLDVehicleAddModel model)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleCode",SqlDbType.NVarChar,20),
                new SqlParameter("@PlateNum",SqlDbType.NVarChar,8),
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar,20),
                new SqlParameter("@VehicleName",SqlDbType.NVarChar,20),
                new SqlParameter("@SIMCode",SqlDbType.VarChar,13),

                new SqlParameter("@TerminalTypeID",SqlDbType.Int),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@Ownership",SqlDbType.Int),
                new SqlParameter("@PlateColorCode",SqlDbType.TinyInt),
                new SqlParameter("@VehicleTypeCode",SqlDbType.Char,2),

                new SqlParameter("@SpeedLimit",SqlDbType.Int),
                new SqlParameter("@DrivingTime",SqlDbType.Int),
                new SqlParameter("@IsReceived",SqlDbType.Bit),
                new SqlParameter("@IsAccess",SqlDbType.Bit),
                new SqlParameter("@IsTransmit",SqlDbType.Bit),

                new SqlParameter("@IsDangerous",SqlDbType.Bit),
                new SqlParameter("@WarrantyDate",SqlDbType.Date), 
                new SqlParameter("@Icon",SqlDbType.NVarChar,50),
                new SqlParameter("@Remark",SqlDbType.NVarChar,50),
                new SqlParameter("@CreateUserID",SqlDbType.Int),

            };

            paras[0].Value = model.VehicleCode;
            paras[1].Value = model.PlateNum;
            paras[2].Value = model.TerminalCode;
            paras[3].Value = model.VehicleName;
            paras[4].Value = model.SIMCode;

            paras[5].Value = model.TerminalTypeID;
            paras[6].Value = model.StrucID;
            paras[7].Value = model.Ownership;
            paras[8].Value = model.PlateColorCode;
            paras[9].Value = model.VehicleTypeCode;

            paras[10].Value = model.SpeedLimit;
            paras[11].Value = model.DrivingTime;
            paras[12].Value = model.IsReceived;
            paras[13].Value = model.IsAccess;
            paras[14].Value = model.IsTransmit;

            paras[15].Value = model.IsDangerous;
            paras[16].Value = model.WarrantyDate;
            paras[17].Value = model.Icon;



            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[18].Value = DBNull.Value;
            }
            else
            {
                paras[18].Value = model.Remark;
            }

            paras[19].Value = model.CreateUserID;



            #region  SQL
            string sql = @"INSERT INTO dbo.Vehicles
        ( VehicleCode ,
          PlateNum ,
          TerminalCode ,
          VehicleName ,
          SIMCode ,
          TerminalTypeID ,
          StrucID ,
          Ownership ,
          PlateColorCode ,
          VehicleTypeCode ,
          SpeedLimit ,
          DrivingTime ,
          IsReceived ,
          IsAccess ,
          IsTransmit ,
          IsDangerous ,
          Icon ,
          WarrantyDate ,
          CreateTime ,
          EditTime ,
          Remark ,
          Status,
          CreateUserID
        )
VALUES  ( @VehicleCode , -- VehicleCode - nvarchar(20)
          @PlateNum , -- PlateNum - nvarchar(20)
          @TerminalCode , -- TerminalCode - nvarchar(20)
          @VehicleName , -- VehicleName - nvarchar(20)
          @SIMCode , -- SIMCode - nvarchar(13)
          @TerminalTypeID , -- TerminalTypeID - int
          @StrucID , -- StrucID - int
          @Ownership , -- Ownership - int
          @PlateColorCode, -- PlateColorCode - char(1)
          @VehicleTypeCode , -- VehicleTypeCode - char(2)
          @SpeedLimit , -- SpeedLimit - int
          @DrivingTime , -- DrivingTime - int
          @IsReceived , -- IsReceived - bit
          @IsAccess , -- IsAccess - bit
          @IsTransmit , -- IsTransmit - bit
          @IsDangerous , -- IsDangerous - bit
          @Icon , -- Icon - nvarchar(50)
          @WarrantyDate , -- WarrantyDate - date
          GETDATE() , -- CreateTime - datetime
          GETDATE(), -- EditTime - datetime
          @Remark , -- Remark - nvarchar(50)
          0,  -- Status - tinyint
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

        public static OperationResult AddVehicle(VehicleAddModel model)
        {
            ////--------修改前逻辑
            //        //1.新增车辆信息到车辆表
            //        //2.新增终端与车辆关联信息到车辆终端表
            //        //3.更新终端状态和插入终端使用记录
            //        //      (该操作功能由VehiclesTerminals表的[TRI_ModifyTerminalStatusWhenInsert]触发器实现)
            //        List<long> auxiliaryIDs = new List<long>();
            //        if (!string.IsNullOrWhiteSpace(model.AuxiliaryTerminalIDs))
            //        {
            //            model.AuxiliaryTerminalIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
            //.ForEach(id => auxiliaryIDs.Add(Convert.ToInt64(id)));
            //        }
            //        int sqlCount = 1 + 1 + auxiliaryIDs.Count;//插入车辆资料语句+插入主定位设备语句+插入辅助定位设备语句


            //1.新增车辆信息到车辆表
            //2.定位终端与车辆关联信息，完善终端表LinkedVehicleID信息  /同时修改终端表终端状态 
            //3.终端使用表 添加终端使用记录
            //终端使用表的数据等1/2执行结束返回true后执行  否则查询终端表关联的LinkedVehicleID为null  无法插入值
            int sqlCount = 2;//1/2/3
            string[] sqls = new string[sqlCount];
            SqlParameter[][] paras = new SqlParameter[sqlCount][];

            #region 插入数据到车辆表
            sqls[0] = @"INSERT INTO dbo.Vehicles
        ( PlateNum ,
          VehicleName ,
          StrucID ,
          Ownership ,
          PlateColorCode ,
          VehicleTypeCode ,
          IsReceived ,
          IsAccess ,
          IsTransmit ,
          IsDangerous ,
          Icon ,
          WarrantyDate ,
          CreateTime ,
          EditTime ,
          Remark ,
          Status ,
          CreateUserID ,
          UpdateUserID,
          VIN
        )
VALUES  ( @PlateNum , -- PlateNum - nchar(7)
          @VehicleName, -- VehicleName - nvarchar(20)
          @StrucID , -- StrucID - int
          @Ownership , -- Ownership - int
          @PlateColorCode , -- PlateColorCode - tinyint
          @VehicleTypeCode , -- VehicleTypeCode - tinyint
          @IsReceived , -- IsReceived - bit
          @IsAccess , -- IsAccess - bit
          @IsTransmit , -- IsTransmit - bit
          @IsDangerous , -- IsDangerous - bit
          @Icon, -- Icon - nvarchar(50)
          @WarrantyDate, -- WarrantyDate - date
          GETDATE() , -- CreateTime - datetime
          GETDATE() , -- EditTime - datetime
          @Remark , -- Remark - nvarchar(500)
          0 , -- Status - tinyint
          @CreateUserID , -- CreateUserID - int
          NULL,  -- UpdateUserID - int
          @VIN
        );SELECT  SCOPE_IDENTITY();";
            paras[0] = new SqlParameter[15];
            paras[0][0] = new SqlParameter()
            {
                ParameterName = "@PlateNum",
                SqlDbType = SqlDbType.NVarChar,
                Size = 8,
                Value = model.PlateNum
            };
            paras[0][1] = new SqlParameter()
            {
                ParameterName = "@VehicleName",
                SqlDbType = SqlDbType.NVarChar,
                Size = 20,
                Value = model.VehicleName
            };
            paras[0][2] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                SqlDbType = SqlDbType.Int,
                Value = model.AddStrucID
            };
            paras[0][3] = new SqlParameter()
            {
                ParameterName = "@Ownership",
                SqlDbType = SqlDbType.Int,
                Value = model.Ownership
            };
            paras[0][4] = new SqlParameter()
            {
                ParameterName = "@PlateColorCode",
                SqlDbType = SqlDbType.TinyInt,
                Value = model.PlateColorCode
            };
            paras[0][5] = new SqlParameter()
            {
                ParameterName = "@VehicleTypeCode",
                SqlDbType = SqlDbType.TinyInt,
                Value = model.VehicleTypeCode
            };
            paras[0][6] = new SqlParameter()
            {
                ParameterName = "@IsReceived",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsReceived
            };
            paras[0][7] = new SqlParameter()
            {
                ParameterName = "@IsAccess",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsAccess
            };
            paras[0][8] = new SqlParameter()
            {
                ParameterName = "@IsTransmit",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsTransmit
            };
            paras[0][9] = new SqlParameter()
            {
                ParameterName = "@IsDangerous",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsDangerous
            };
            paras[0][10] = new SqlParameter()
            {
                ParameterName = "@Icon",
                SqlDbType = SqlDbType.NVarChar,
                Size = 50,
                Value = model.Icon
            };
            paras[0][11] = new SqlParameter()
            {
                ParameterName = "@WarrantyDate",
                SqlDbType = SqlDbType.Date,
                Value = model.WarrantyDate
            };
            paras[0][12] = new SqlParameter()
            {
                ParameterName = "@CreateUserID",
                SqlDbType = SqlDbType.Int,
                Value = model.CreateUserID
            };

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[0][13] = new SqlParameter()
                {
                    ParameterName = "@Remark",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 500,
                    Value = DBNull.Value
                };
            }
            else
            {
                paras[0][13] = new SqlParameter()
                {
                    ParameterName = "@Remark",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 500,
                    Value = model.Remark
                };
            }
            paras[0][14] = new SqlParameter()
            {
                ParameterName = "@VIN",
                SqlDbType = SqlDbType.Char,
                Size = 17,
                Value = model.VIN
            };

            #endregion


            #region  完善终端表LinkedVehicleID信息
            sqls[1] = @"UPDATE  dbo.Terminals
SET     LinkedVehicleID = @LinkedVehicleID , Status=7 
WHERE   ID = @ID";
            paras[1] = new SqlParameter[2];
            paras[1][0] = new SqlParameter()
            {
                ParameterName = "@LinkedVehicleID",
                SqlDbType = SqlDbType.BigInt
            };
            paras[1][1] = new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.BigInt,
                Value = model.PrimaryTerminalID
            };
            #endregion



            //#region 插入数据到车辆与终端关联表
            //for (int i = 1; i <= (1 + auxiliaryIDs.Count); i++)
            //{
            //    string tempSql = string.Empty;
            //    long terminalID = 0;
            //    if (i == 1)//第一条先插入主定位终端
            //    {
            //        tempSql = @"INSERT  INTO dbo.VehiclesTerminals( VehicleID, TerminalID, IsPrimary ) VALUES  ( @VehicleID,@TerminalID,1)";
            //        terminalID = model.PrimaryTerminalID;
            //    }
            //    else
            //    {
            //        tempSql = @"INSERT  INTO dbo.VehiclesTerminals( VehicleID, TerminalID, IsPrimary ) VALUES  ( @VehicleID,@TerminalID,0)";
            //        terminalID = auxiliaryIDs[i - 2];
            //    }
            //    sqls[i] = tempSql;
            //    paras[i] = new SqlParameter[2];
            //    paras[i][0] = new SqlParameter { ParameterName = "@VehicleID", SqlDbType = SqlDbType.BigInt };

            //    paras[i][1] = new SqlParameter()
            //    {
            //        ParameterName = "@TerminalID",
            //        SqlDbType = SqlDbType.BigInt,
            //        Value = terminalID
            //    };

            //}
            //#endregion


            bool result1 = MSSQLHelper.ExecuteIdentityIncludeTransaction(CommandType.Text, sqls, paras) != 0;

            string[] sqls2 = new string[1];
            SqlParameter[][] paras2 = new SqlParameter[1][];
            if (result1 == true)
            {
                #region  终端使用表添加记录
                sqls2[0] = @"INSERT INTO dbo.TerminalUsageLogs
        ( TerminalCode ,
          VehicleID ,
          CreateDateTime ,
          CreateUserID ,
          EndDateTime
        )
VALUES  ( (SELECT TerminalCode FROM dbo.Terminals WHERE ID=@ID) , 
          (SELECT LinkedVehicleID FROM dbo.Terminals WHERE ID=@ID), 
          GETDATE() , -- CreateTime - datetime
          @CreateUserID , -- CreateUserID - int
          NULL  -- EndDateTime - datetime
        );";
                paras2[0] = new SqlParameter[2];
                paras2[0][0] = new SqlParameter()
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.PrimaryTerminalID
                };
                paras2[0][1] = new SqlParameter()
                {
                    ParameterName = "@CreateUserID",
                    SqlDbType = SqlDbType.Int,
                    Value = model.CreateUserID
                };
                #endregion
            }

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls2, paras2);

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 编辑

        public static SelectResult<OLDVehicleEditModel> GetVehicleByID(long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = id;
            string sql = @"SELECT  ID ,CAST(WarrantyDate AS CHAR(10)) AS WarrantyDate,
        VehicleCode ,
        PlateNum ,
        TerminalCode ,
        VehicleName ,
        SIMCode ,
        TerminalTypeID ,
        StrucID ,
        Ownership ,
        PlateColorCode ,
        VehicleTypeCode ,
        SpeedLimit ,
        DrivingTime ,
        IsReceived ,
        IsTransmit ,
        IsAccess ,
        IsDangerous ,
        Icon ,
        Remark
FROM    dbo.Vehicles
WHERE   ID = @ID
        AND [Status] <> 9";
            List<OLDVehicleEditModel> list = ConvertToList<OLDVehicleEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            OLDVehicleEditModel data = null;
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
            return new SelectResult<OLDVehicleEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static SelectResult<VehicleEditModel> GetEditVehicleByID(long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = id;
            string sql = @"SELECT    v.ID ,
            v.PlateNum ,
            v.VehicleName ,
            v.PlateColorCode ,
            v.VehicleTypeCode ,
            v.StrucID AS EditStrucID ,
            Ostr.StrucName, 
            v.Ownership ,
            UStr.StrucName AS OwnershipName,
            CAST(v.WarrantyDate AS CHAR(10)) AS WarrantyDate ,
            v.Icon ,
            v.IsAccess ,
            v.IsDangerous ,
            v.IsReceived ,
            v.IsTransmit,
           v.Remark,
           tt.TerminalCode,
            tt.ID AS PrimaryTerminalID,
            v.VIN  
  FROM      dbo.Vehicles v
  INNER JOIN dbo.Terminals tt ON v.ID=tt.LinkedVehicleID 
  LEFT JOIN Structures AS OStr ON v.StrucID = OStr.ID 
  LEFT JOIN Structures AS UStr ON v.Ownership = UStr.ID
  WHERE v.[Status]<>9 AND v.ID=@ID";
            List<VehicleEditModel> list = ConvertToList<VehicleEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            VehicleEditModel data = null;
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
            return new SelectResult<VehicleEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        /// <summary>
        /// 检查初始车牌是否重复
        /// </summary>
        public static bool CheckVehicleCodeExists(string vehicleCode, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@vehicleCode",SqlDbType.NVarChar,20),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = vehicleCode.Trim();
            paras[1].Value = id;


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VehicleCode=@vehicleCode AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查同一个使用单位下车代号是否重复
        /// </summary>
        public static bool CheckVehicleNameExists(string vehicleName, int strucID, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@vehicleName",SqlDbType.NVarChar,20),
                new SqlParameter("@strucID",SqlDbType.Int),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = vehicleName.Trim();
            paras[1].Value = strucID;
            paras[2].Value = id;



            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VehicleName=@vehicleName AND StrucID=@strucID AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }


        /// <summary>
        /// 检查终端号是否重复
        /// </summary>
        public static bool CheckTerminalCodeExists(string terminalCode, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@terminalCode",SqlDbType.NVarChar,20),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = terminalCode.Trim();
            paras[1].Value = id;


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE TerminalCode=@terminalCode AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查车牌号是否重复
        /// </summary>
        public static bool CheckPlateNumExists(string plateNum, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@plateNum",SqlDbType.NVarChar,8),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = plateNum.Trim();
            paras[1].Value = id;


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE PlateNum=@plateNum AND ID<>@ID";
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
                new SqlParameter("@SIMCode",SqlDbType.Char,13),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = SIMCode.Trim();
            paras[1].Value = id;


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE SIMCode=@SIMCode AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }



        public static bool CheckEditVINExists(string VIN, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VIN",SqlDbType.Char,17),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = VIN.Trim();
            paras[1].Value = id;


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VIN=@VIN AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        public static OperationResult EditVehicle(OLDVehicleEditModel model)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
                    {
                        new SqlParameter("@ID",SqlDbType.BigInt),
                        new SqlParameter("@PlateNum",SqlDbType.NVarChar,8),
                        new SqlParameter("@TerminalCode",SqlDbType.NVarChar,20),
                        new SqlParameter("@VehicleName",SqlDbType.NVarChar,20),
                        new SqlParameter("@SIMCode",SqlDbType.VarChar,13),

                        new SqlParameter("@TerminalTypeID",SqlDbType.Int),
                        new SqlParameter("@StrucID",SqlDbType.Int),
                        new SqlParameter("@Ownership",SqlDbType.Int),
                        new SqlParameter("@PlateColorCode",SqlDbType.TinyInt),
                        new SqlParameter("@VehicleTypeCode",SqlDbType.Char,2),

                        new SqlParameter("@SpeedLimit",SqlDbType.Int),
                        new SqlParameter("@DrivingTime",SqlDbType.Int),
                        new SqlParameter("@IsReceived",SqlDbType.Bit),
                        new SqlParameter("@IsAccess",SqlDbType.Bit),
                        new SqlParameter("@IsTransmit",SqlDbType.Bit),

                        new SqlParameter("@IsDangerous",SqlDbType.Bit),
                        new SqlParameter("@WarrantyDate",SqlDbType.Date), 
                        new SqlParameter("@Icon",SqlDbType.NVarChar,50),
                        new SqlParameter("@Remark",SqlDbType.NVarChar,50),
                        new SqlParameter("@UpdateUserID",SqlDbType.Int),
   
                    };

            paras[0].Value = model.ID;
            paras[1].Value = model.PlateNum;
            paras[2].Value = model.TerminalCode;
            paras[3].Value = model.VehicleName;
            paras[4].Value = model.SIMCode;

            paras[5].Value = model.TerminalTypeID;
            paras[6].Value = model.StrucID;
            paras[7].Value = model.Ownership;
            paras[8].Value = model.PlateColorCode;
            paras[9].Value = model.VehicleTypeCode;

            paras[10].Value = model.SpeedLimit;
            paras[11].Value = model.DrivingTime;
            paras[12].Value = model.IsReceived;
            paras[13].Value = model.IsAccess;
            paras[14].Value = model.IsTransmit;

            paras[15].Value = model.IsDangerous;
            paras[16].Value = model.WarrantyDate;
            paras[17].Value = model.Icon;



            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[18].Value = DBNull.Value;
            }
            else
            {
                paras[18].Value = model.Remark;
            }

            paras[19].Value = model.UpdateUserID;




            #region  SQL
            string sql = @"UPDATE  dbo.Vehicles
SET  PlateNum = @PlateNum ,
        TerminalCode = @TerminalCode ,
        VehicleName = @VehicleName ,
        SIMCode = @SIMCode ,
        TerminalTypeID = @TerminalTypeID ,
        StrucID = @StrucID ,
        Ownership = @Ownership ,
        PlateColorCode = @PlateColorCode ,
        VehicleTypeCode = @VehicleTypeCode ,
        SpeedLimit = @SpeedLimit ,
        DrivingTime = @DrivingTime ,
        IsReceived = @IsReceived,
        IsAccess = @IsAccess ,
        IsTransmit = @IsTransmit ,
        IsDangerous = @IsDangerous ,
        WarrantyDate=@WarrantyDate,
        Icon = @Icon ,
        Remark = @Remark,
        UpdateUserID=@UpdateUserID,
        EditTime = GETDATE()
WHERE   ID = @ID";
            #endregion



            int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
            string msg = string.Empty;
            if (result > 0)
            {
                msg = PromptInformation.OperationSuccess;
            }
            else if (result == 0)
            {
                msg = PromptInformation.NotExists;
            }
            else
            {
                msg = PromptInformation.DBError;
            }
            return new OperationResult()
            {
                Success = result > 0,
                Message = msg
            };
        }

        /// <summary>
        /// 编辑车辆
        /// 1.更新资料
        /// 2.根据车辆ID删除终端关联信息
        /// 3.重新插入终端关联信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult EditVehicle(VehicleEditModel model)
        {
            ////1.更新车辆资料表
            ////2.删除终端与车辆关联表
            ////3.更新终端状态与插入终端使用记录
            ////部分功能由VehiclesTerminals表的[TRI_ModifyTerminalStatusWhenInsert]
            ////和[TRI_ModifyTerminalStatusWhenDelete]实现
            //List<long> auxiliaryIDs = new List<long>();
            //if (!string.IsNullOrWhiteSpace(model.AuxiliaryTerminalIDs))
            //{
            //    model.AuxiliaryTerminalIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
            //        .ForEach(id => auxiliaryIDs.Add(Convert.ToInt64(id)));
            //}
            ////更新车辆资料语句+删除终端与车辆关联数据+插入主定位设备语句+插入辅助定位设备语句
            //int sqlCount = 1 + 1 + 1 + auxiliaryIDs.Count;
            //string[] sqls = new string[sqlCount];
            //SqlParameter[][] paras = new SqlParameter[sqlCount][];



            //1.更新车辆资料表
            //2.判断终端有没有更换  没有更换只更新车辆资料表 
            //3.1终端更换 / 原终端LinkedVehicleID置为空，修改状态0（查询原先绑定的终端信息用LinkedVehicleID） / 新终端绑定
            //3.2更新终端使用表，原终端添加结束时间，添加新终端和车辆关联记录  

            int sqlCount = 1;//1/2/3
            string[] sqls = new string[sqlCount];
            SqlParameter[][] paras = new SqlParameter[sqlCount][];

            #region 更新车辆资料
            sqls[0] = @"UPDATE  dbo.Vehicles
SET  PlateNum = @PlateNum ,
        VehicleName = @VehicleName ,
        StrucID = @StrucID ,
        Ownership = @Ownership ,
        PlateColorCode = @PlateColorCode ,
        VehicleTypeCode = @VehicleTypeCode ,
        IsReceived = @IsReceived,
        IsAccess = @IsAccess ,
        IsTransmit = @IsTransmit ,
        IsDangerous = @IsDangerous ,
        WarrantyDate=@WarrantyDate,
        Icon = @Icon ,
        Remark = @Remark,
        UpdateUserID=@UpdateUserID,
        EditTime = GETDATE(),
        VIN = @VIN
WHERE   ID = @ID";
            paras[0] = new SqlParameter[16];
            paras[0][0] = new SqlParameter()
            {
                ParameterName = "@PlateNum",
                SqlDbType = SqlDbType.NVarChar,
                Size = 8,
                Value = model.PlateNum
            };
            paras[0][1] = new SqlParameter()
            {
                ParameterName = "@VehicleName",
                SqlDbType = SqlDbType.NVarChar,
                Size = 20,
                Value = model.VehicleName
            };
            paras[0][2] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                SqlDbType = SqlDbType.Int,
                Value = model.EditStrucID
            };
            paras[0][3] = new SqlParameter()
            {
                ParameterName = "@Ownership",
                SqlDbType = SqlDbType.Int,
                Value = model.Ownership
            };
            paras[0][4] = new SqlParameter()
            {
                ParameterName = "@PlateColorCode",
                SqlDbType = SqlDbType.TinyInt,
                Value = model.PlateColorCode
            };
            paras[0][5] = new SqlParameter()
            {
                ParameterName = "@VehicleTypeCode",
                SqlDbType = SqlDbType.TinyInt,
                Value = model.VehicleTypeCode
            };
            paras[0][6] = new SqlParameter()
            {
                ParameterName = "@IsReceived",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsReceived
            };
            paras[0][7] = new SqlParameter()
            {
                ParameterName = "@IsAccess",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsAccess
            };
            paras[0][8] = new SqlParameter()
            {
                ParameterName = "@IsTransmit",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsTransmit
            };
            paras[0][9] = new SqlParameter()
            {
                ParameterName = "@IsDangerous",
                SqlDbType = SqlDbType.Bit,
                Value = model.IsDangerous
            };
            paras[0][10] = new SqlParameter()
            {
                ParameterName = "@Icon",
                SqlDbType = SqlDbType.NVarChar,
                Size = 50,
                Value = model.Icon
            };
            paras[0][11] = new SqlParameter()
            {
                ParameterName = "@WarrantyDate",
                SqlDbType = SqlDbType.Date,
                Value = model.WarrantyDate
            };
            paras[0][12] = new SqlParameter()
            {
                ParameterName = "@UpdateUserID",
                SqlDbType = SqlDbType.Int,
                Value = model.UpdateUserID
            };

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[0][13] = new SqlParameter()
                {
                    ParameterName = "@Remark",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 500,
                    Value = DBNull.Value
                };
            }
            else
            {
                paras[0][13] = new SqlParameter()
                {
                    ParameterName = "@Remark",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 500,
                    Value = model.Remark
                };
            }
            paras[0][14] = new SqlParameter()
            {
                ParameterName = "@VIN",
                SqlDbType = SqlDbType.Char,
                Size = 17,
                Value = model.VIN
            };
            paras[0][15] = new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.BigInt,
                Value = model.ID
            };

            #endregion

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);

            string sql = "SELECT ID FROM dbo.Terminals WHERE LinkedVehicleID=@ID";
            List<SqlParameter> para = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            para[0].Value = model.ID;
            var dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, para.ToArray());
            if (dt == null)
            {
                return null;
            }

            decimal preTerminalID = Convert.ToDecimal(dt.Rows[0][0].ToString());
            if (preTerminalID != model.PrimaryTerminalID)
            { //更换绑定终端  不更换直接返回result
                string[] sqlChange = new string[4];
                SqlParameter[][] paraChange = new SqlParameter[4][];

                #region  修改终端表原终端信息状态
                sqlChange[0] = @"UPDATE  dbo.Terminals
SET     LinkedVehicleID = null , Status=0 
WHERE   LinkedVehicleID = @LinkedVehicleID";
                paraChange[0] = new SqlParameter[1];
                paraChange[0][0] = new SqlParameter()
                {
                    ParameterName = "@LinkedVehicleID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.ID
                };
                #endregion


                #region  更新终端表新绑定终端
                sqlChange[1] = @"UPDATE  dbo.Terminals
SET     LinkedVehicleID = @LinkedVehicleID , Status=7 
WHERE   ID = @ID";
                paraChange[1] = new SqlParameter[2];
                paraChange[1][0] = new SqlParameter()
                {
                    ParameterName = "@LinkedVehicleID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.ID
                };
                paraChange[1][1] = new SqlParameter()
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.PrimaryTerminalID
                };
                #endregion


                #region  更新终端使用表原纪录结束时间
                sqlChange[2] = @"  UPDATE  dbo.TerminalUsageLogs
SET     EndDateTime = GETDATE()  
WHERE  EndDateTime IS NULL AND VehicleID = @VehicleID AND TerminalCode=(SELECT TerminalCode FROM dbo.Terminals WHERE ID=@ID)";
                paraChange[2] = new SqlParameter[2];
                paraChange[2][0] = new SqlParameter()
                {
                    ParameterName = "@VehicleID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.ID
                };
                paraChange[2][1] = new SqlParameter()
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = preTerminalID
                };
                #endregion


                #region  终端使用表添加新记录
                sqlChange[3] = @"INSERT INTO dbo.TerminalUsageLogs
        ( TerminalCode ,
          VehicleID ,
          CreateDateTime ,
          CreateUserID ,
          EndDateTime
        )
VALUES  ( (SELECT TerminalCode FROM dbo.Terminals WHERE ID=@ID) , 
          @VehicleID, 
          GETDATE() , -- CreateTime - datetime
          @CreateUserID , -- CreateUserID - int
          NULL  -- EndDateTime - datetime
        );";
                paraChange[3] = new SqlParameter[3];
                paraChange[3][0] = new SqlParameter()
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.PrimaryTerminalID
                };
                paraChange[3][1] = new SqlParameter()
                {
                    ParameterName = "@VehicleID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.ID
                };
                paraChange[3][2] = new SqlParameter()
                {
                    ParameterName = "@CreateUserID",
                    SqlDbType = SqlDbType.Int,
                    Value = model.UpdateUserID
                };
                #endregion

                result = result && MSSQLHelper.ExecuteTransaction(CommandType.Text, sqlChange, paraChange);

            }



            //#region 删除车辆与终端关联数据
            //sqls[1] = "DELETE FROM dbo.VehiclesTerminals WHERE VehicleID=@VehicleID";
            //paras[1] = new SqlParameter[1];
            //paras[1][0] = new SqlParameter()
            //{
            //    ParameterName = "@VehicleID",
            //    SqlDbType = SqlDbType.BigInt,
            //    Value = model.ID
            //};
            //#endregion


            //#region 插入数据到车辆与终端关联表
            //for (int i = 2; i <= (2 + auxiliaryIDs.Count); i++)
            //{
            //    string tempSql = string.Empty;
            //    long terminalID = 0;
            //    if (i == 2)//第一条先插入主定位终端
            //    {
            //        tempSql = @"INSERT  INTO dbo.VehiclesTerminals( VehicleID, TerminalID, IsPrimary ) VALUES  ( @VehicleID,@TerminalID,1)";
            //        terminalID = model.PrimaryTerminalID;
            //    }
            //    else
            //    {
            //        tempSql = @"INSERT  INTO dbo.VehiclesTerminals( VehicleID, TerminalID, IsPrimary ) VALUES  ( @VehicleID,@TerminalID,0)";
            //        terminalID = auxiliaryIDs[i - 3];
            //    }
            //    sqls[i] = tempSql;
            //    paras[i] = new SqlParameter[2];
            //    paras[i][0] = new SqlParameter()
            //    {
            //        ParameterName = "@VehicleID",
            //        SqlDbType = SqlDbType.BigInt,
            //        Value = model.ID
            //    };

            //    paras[i][1] = new SqlParameter()
            //    {
            //        ParameterName = "@TerminalID",
            //        SqlDbType = SqlDbType.BigInt,
            //        Value = terminalID
            //    };

            //}
            //#endregion


            //bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region 车辆分配

        public static List<VehicleDistributionInfoModel> GetDistributiveVehiclesByUserID(int userID)
        {
            string sql = @"SELECT v.ID,v.PlateNum,s.StrucName FROM dbo.VehicleDistributionInfo vdi
  INNER JOIN dbo.Vehicles v ON vdi.VehicleID=v.ID
  INNER JOIN dbo.Structures s ON v.StrucID=s.ID
  WHERE vdi.UserID=@userID AND v.[Status]<>9";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@userID",SqlDbType.Int),
            };
            paras[0].Value = userID;

            return ConvertToList<VehicleDistributionInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

        public static List<VehicleDistributionInfoModel> GetVehicles(string strucName, string plateNum)
        {
            string sql = @"SELECT TOP 5 v.ID,v.PlateNum,s.StrucName FROM dbo.Vehicles v 
  INNER JOIN dbo.Structures s ON v.StrucID=s.ID
  WHERE v.PlateNum LIKE @plateNum AND s.StrucName LIKE @strucName AND v.[Status]<>9";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@plateNum",SqlDbType.NVarChar,8),
                new SqlParameter("@strucName",SqlDbType.NVarChar,32),
            };
            paras[0].Value = "%" + plateNum + "%";
            paras[1].Value = "%" + strucName + "%";

            return ConvertToList<VehicleDistributionInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion

        #region 批量导入车辆 旧
        /////// <summary>
        /////// 批量导入车辆资料
        /////// </summary>
        /////// <param name="datas">待带入的车辆资料DataTable</param>
        /////// <returns></returns>
        ////public static OperationResult ImportVehicles(DataTable datas, int createUserID)
        ////{
        ////    bool success = false;
        ////    string message = string.Empty;
        ////    #region 检查数据合法性
        ////    if (datas == null)
        ////    {
        ////        message = "导入失败：无法获取文件内容！";
        ////        goto end;
        ////    }
        ////    if (datas.Rows.Count == 0)
        ////    {
        ////        message = "导入失败：没有找到任何车辆资料！";
        ////        goto end;
        ////    }


        ////    DataTable importDataTable = CreateImportVehicleDataTable();

        ////    string errorMessage;
        ////    string brStr = "</br>";
        ////    //存储车牌号，用于判断DataTable中本身是否存在重复的车牌号
        ////    List<string> plateNums = new List<string>();
        ////    //用于判断同使用单位下是否存在同车代号数据
        ////    List<dynamic> vehiclesNames = new List<dynamic>();

        ////    for (int i = 0; i < datas.Rows.Count; i++)
        ////    {
        ////        errorMessage = string.Empty;
        ////        int rowIndex = i + 1;
        ////        string rowMessage = string.Format(PromptInformation.RowIndex, rowIndex);

        ////        #region 获取当前行数据
        ////        var currentRow = datas.Rows[i];
        ////        string plateNumStr = currentRow[0].ToString().Trim();//                    车牌号                           
        ////        string vehicleNameStr = currentRow[1].ToString().Trim();//                 车代号                      
        ////        string plateColorStr = currentRow[2].ToString().Trim();//                  车牌颜色                      
        ////        string speedLimitStr = currentRow[3].ToString().Trim();//               限速值
        ////        string drivingTimeStr = currentRow[4].ToString().Trim();//              疲劳驾驶时间
        ////        string isReceivedStr = currentRow[5].ToString().Trim();//               接收信号
        ////        string isAccessStr = currentRow[6].ToString().Trim();//                 货运平台接入
        ////        string isTransmitStr = currentRow[7].ToString().Trim();//               转发运管
        ////        string isDangerousStr = currentRow[8].ToString().Trim();//              危险品车
        ////        string warrantyDateStr = currentRow[9].ToString().Trim();//                 保固日期
        ////        string iconStr = currentRow[10].ToString().Trim();//                            车辆图标
        ////        string vehicleTypeStr = currentRow[11].ToString().Trim();//                 车辆类型
        ////        string structureStr = currentRow[12].ToString().Trim();//                      使用单位
        ////        string ownershipStr = currentRow[13].ToString().Trim();//                  产权单位
        ////        string remarkStr = currentRow[14].ToString().Trim();//                      备注 
        ////        #endregion

        ////        string fieldName = string.Empty;


        ////        #region 检查限速值
        ////        fieldName = DisplayText.SpeedLimit;
        ////        int speedLimitValue = -1;
        ////        if (string.IsNullOrWhiteSpace(speedLimitStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!int.TryParse(speedLimitStr, out speedLimitValue) || (speedLimitValue < 30 || speedLimitValue > 120))
        ////        {
        ////            errorMessage += fieldName + "范围不正确（30-12）" + brStr;
        ////        }

        ////        #endregion

        ////        #region 检查疲劳驾驶时间
        ////        fieldName = DisplayText.DrivingTime;
        ////        int drivingTimeValue = -1;
        ////        if (string.IsNullOrWhiteSpace(drivingTimeStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!int.TryParse(drivingTimeStr, out drivingTimeValue) || (drivingTimeValue < 1 || drivingTimeValue > 300))
        ////        {
        ////            errorMessage += fieldName + "范围不正确（1-300）" + brStr;
        ////        }

        ////        #endregion

        ////        #region 检查车辆图标
        ////        fieldName = DisplayText.Icon;
        ////        List<string> icons = new List<string>()
        ////        {
        ////            "Default","Arrow","MixerTruck","PumpTruck"
        ////        };
        ////        if (string.IsNullOrWhiteSpace(iconStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (icons.SingleOrDefault(m => m == iconStr) == null)
        ////        {
        ////            errorMessage += fieldName + "不属于Default、Arrow、MixerTruck、PumpTruck其中之一" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查保固日期
        ////        fieldName = DisplayText.WarrantyDate;
        ////        DateTime warrantyDateValue = DateTime.Today;
        ////        if (string.IsNullOrWhiteSpace(warrantyDateStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!DateTime.TryParse(warrantyDateStr, out warrantyDateValue))
        ////        {
        ////            errorMessage += fieldName + "不是正确的日期格式" + brStr;
        ////        }
        ////        else if (DateTime.Today.Subtract(warrantyDateValue).TotalDays > 0)
        ////        {
        ////            errorMessage += string.Format(DataAnnotations.WarrantyDateError, fieldName, DateTime.Now.ToString("yyyy-MM-dd")) + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查接收信号
        ////        fieldName = DisplayText.IsReceived;
        ////        bool isReceivedValue = false;
        ////        if (string.IsNullOrWhiteSpace(isReceivedStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (bool.TryParse(isReceivedStr, out isReceivedValue))
        ////        {
        ////            errorMessage += fieldName + "只能是1或0" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查货运平台接入
        ////        fieldName = DisplayText.IsAccess;
        ////        bool isAccessValue = false;
        ////        if (string.IsNullOrWhiteSpace(isAccessStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (bool.TryParse(isAccessStr, out isAccessValue))
        ////        {
        ////            errorMessage += fieldName + "只能是1或0" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查转发运管
        ////        fieldName = DisplayText.IsTransmit;
        ////        bool isTransmitValue = false;
        ////        if (string.IsNullOrWhiteSpace(isTransmitStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (bool.TryParse(isTransmitStr, out isTransmitValue))
        ////        {
        ////            errorMessage += fieldName + "只能是1或0" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查危险品车
        ////        fieldName = DisplayText.IsDangerous;
        ////        bool isDangerousValue = false;
        ////        if (string.IsNullOrWhiteSpace(isDangerousStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (bool.TryParse(isDangerousStr, out isDangerousValue))
        ////        {
        ////            errorMessage += fieldName + "只能是1或0" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查备注
        ////        fieldName = DisplayText.Remark;
        ////        if (remarkStr.Length > 500)
        ////        {
        ////            errorMessage += string.Format(DataAnnotations.MaxLength, fieldName, 500) + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查车牌号
        ////        fieldName = DisplayText.PlateNum;
        ////        if (string.IsNullOrWhiteSpace(plateNumStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查长度
        ////        else if (plateNumStr.Length != 7)
        ////        {
        ////            errorMessage += DataAnnotations.PlateNumLengthError + brStr;
        ////        }
        ////        //检查是否重复
        ////        else if (CheckPlateNumExists(plateNumStr))
        ////        {
        ////            errorMessage += string.Format(DataAnnotations.FieldExists, fieldName) + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查使用单位（先检查单位，因为车代号的检查需要用到使用单位ID）
        ////        fieldName = DisplayText.StrucWhichUseVehicle;
        ////        int structureID = -1;
        ////        if (string.IsNullOrWhiteSpace(structureStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查使用单位名是否存在，并且获取真正的使用单位ID
        ////        else if (!StructureBLL.TryGetStructureIDByName(structureStr, out structureID))
        ////        {
        ////            errorMessage += "不存在" + fieldName + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查车代号
        ////        fieldName = DisplayText.VehicleName;
        ////        if (string.IsNullOrWhiteSpace(vehicleNameStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查长度
        ////        else if (vehicleNameStr.Length > 20)
        ////        {
        ////            errorMessage += string.Format(DataAnnotations.MaxLength, fieldName, 20) + brStr;
        ////        }
        ////        //检查是否重复
        ////        else if (CheckVehicleNameExists(vehicleNameStr, structureID))
        ////        {
        ////            errorMessage += DataAnnotations.VehicleNameExists + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查车牌颜色
        ////        fieldName = DisplayText.PlateColor;
        ////        int plateCodeValue = -1;
        ////        if (string.IsNullOrWhiteSpace(plateColorStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查车牌颜色合法性
        ////        else if (!PlateColorBLL.TryGetCodeByName(plateColorStr, out plateCodeValue))
        ////        {
        ////            errorMessage += "不存在" + fieldName + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查产权所属单位
        ////        fieldName = DisplayText.Ownership;
        ////        int ownershipID = -1;
        ////        if (string.IsNullOrWhiteSpace(ownershipStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查产权单位名是否存在，并且获取真正的产权单位ID
        ////        else if (!StructureBLL.TryGetStructureIDByName(ownershipStr, out ownershipID))
        ////        {
        ////            errorMessage += "不存在" + fieldName + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查车辆类型
        ////        fieldName = DisplayText.VehicleType;
        ////        int vehicleTypeCode = -1;
        ////        if (string.IsNullOrWhiteSpace(vehicleTypeStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!VehicleTypeBLL.TryGetCodeByName(vehicleTypeStr, out vehicleTypeCode))
        ////        {
        ////            errorMessage += "不存在" + fieldName + brStr;
        ////        }
        ////        #endregion


        ////        #region 创建新行
        ////        if (!string.IsNullOrWhiteSpace(errorMessage))
        ////        {
        ////            message = "导入失败：" + brStr + rowMessage + brStr + errorMessage;
        ////            goto end;
        ////        }
        ////        else//没有错误则创建新行
        ////        {
        ////            var importRow = importDataTable.NewRow();
        ////            //importRow["ID"] = DBNull.Value;
        ////            importRow["PlateNum"] = plateNumStr;
        ////            importRow["VehicleName"] = vehicleNameStr;
        ////            importRow["StrucID"] = structureID;
        ////            importRow["Ownership"] = ownershipID;
        ////            importRow["PlateColorCode"] = plateCodeValue;

        ////            importRow["VehicleTypeCode"] = vehicleTypeCode;
        ////            importRow["SpeedLimit"] = speedLimitValue;
        ////            importRow["DrivingTime"] = drivingTimeValue;
        ////            importRow["IsReceived"] = isReceivedValue;
        ////            importRow["IsAccess"] = isAccessValue;

        ////            importRow["IsTransmit"] = isTransmitValue;
        ////            importRow["IsDangerous"] = isDangerousValue;
        ////            importRow["Icon"] = iconStr;
        ////            importRow["WarrantyDate"] = warrantyDateValue.ToShortDateString();
        ////            importRow["CreateTime"] = DateTime.Now;
        ////            //importRow["EditTime"] = string.Empty;
        ////            importRow["Remark"] = remarkStr;
        ////            //importRow["Status"] = 0;

        ////            importRow["CreateUserID"] = createUserID;
        ////            //importRow["UpdateUserID"] = string.Empty;

        ////            importDataTable.Rows.Add(importRow);

        ////            if (plateNums.Exists(pn => pn == plateNumStr))
        ////            {
        ////                message = "导入失败：Excel文件中存在重复车牌号：" + plateNumStr;
        ////                goto end;
        ////            }
        ////            if (vehiclesNames.Exists(t => t.SID == structureID && t.VN == vehicleNameStr))
        ////            {
        ////                message = "导入失败：Excel文件中存在同使用单位下的同名车代号：" + vehicleNameStr + "_" + structureStr;
        ////                goto end;
        ////            }

        ////            vehiclesNames.Add(new { SID = structureID, VN = vehicleNameStr });
        ////            plateNums.Add(plateNumStr);
        ////        }
        ////        #endregion
        ////    }
        ////    #endregion


        ////    #region 导入
        ////    List<SqlBulkCopyColumnMapping> mappingItems = new List<SqlBulkCopyColumnMapping>()
        ////    {
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="PlateNum",DestinationColumn="PlateNum"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="VehicleName",DestinationColumn="VehicleName"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="StrucID",DestinationColumn="StrucID"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="Ownership",DestinationColumn="Ownership"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="PlateColorCode",DestinationColumn="PlateColorCode"},

        ////        new SqlBulkCopyColumnMapping(){SourceColumn="VehicleTypeCode",DestinationColumn="VehicleTypeCode"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="SpeedLimit",DestinationColumn="SpeedLimit"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="DrivingTime",DestinationColumn="DrivingTime"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="IsReceived",DestinationColumn="IsReceived"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="IsAccess",DestinationColumn="IsAccess"},

        ////        new SqlBulkCopyColumnMapping(){SourceColumn="IsTransmit",DestinationColumn="IsTransmit"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="IsDangerous",DestinationColumn="IsDangerous"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="Icon",DestinationColumn="Icon"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="WarrantyDate",DestinationColumn="WarrantyDate"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="CreateTime",DestinationColumn="CreateTime"},

        ////        new SqlBulkCopyColumnMapping(){SourceColumn="Remark",DestinationColumn="Remark"},
        ////        new SqlBulkCopyColumnMapping(){SourceColumn="CreateUserID",DestinationColumn="CreateUserID"},
        ////    };
        ////    success = MSSQLHelper.ImportToDataBase(importDataTable, "dbo.Vehicles", mappingItems.ToArray());
        ////    message = success ? PromptInformation.OperationSuccess : PromptInformation.DBError;
        ////    #endregion

        ////end:
        ////    return new OperationResult()
        ////    {
        ////        Success = success,
        ////        Message = message
        ////    };
        ////}

        ////private static DataTable CreateImportVehicleDataTable()
        ////{

        ////    DataTable importDataTable = new DataTable();

        ////    //DataColumn dc = new DataColumn("ID", typeof(long));
        ////    //importDataTable.Columns.Add(dc);

        ////    DataColumn dc = new DataColumn("PlateNum", typeof(string));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("VehicleName", typeof(string));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("StrucID", typeof(Int32));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("Ownership", typeof(Int32));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("PlateColorCode", typeof(Int32));
        ////    importDataTable.Columns.Add(dc);

        ////    dc = new DataColumn("VehicleTypeCode", typeof(Int32));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("SpeedLimit", typeof(Int32));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("DrivingTime", typeof(Int32));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("IsReceived", typeof(bool));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("IsAccess", typeof(bool));
        ////    importDataTable.Columns.Add(dc);

        ////    dc = new DataColumn("IsTransmit", typeof(bool));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("IsDangerous", typeof(bool));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("Icon", typeof(string));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("WarrantyDate", typeof(string));
        ////    importDataTable.Columns.Add(dc);
        ////    dc = new DataColumn("CreateTime", typeof(DateTime));
        ////    importDataTable.Columns.Add(dc);


        ////    //dc = new DataColumn("EditTime", typeof(string));
        ////    //importDataTable.Columns.Add(dc);

        ////    dc = new DataColumn("Remark", typeof(string));
        ////    importDataTable.Columns.Add(dc);

        ////    //dc = new DataColumn("Status", typeof(int));
        ////    //importDataTable.Columns.Add(dc);

        ////    dc = new DataColumn("CreateUserID", typeof(long));
        ////    importDataTable.Columns.Add(dc);

        ////    //dc = new DataColumn("UpdateUserID", typeof(long));
        ////    //importDataTable.Columns.Add(dc);
        ////    return importDataTable;
        ////}

        /////// <summary>
        /////// 批量导入车辆信息
        /////// </summary>
        /////// <param name="excelFilePath">待导入的Excel文件绝对物理路径</param>
        /////// <param name="sheetName">车辆资料对应的SheetName</param>
        /////// <param name="createUserID">导入车辆资料的用户ID</param>
        /////// <returns></returns>
        ////public static OperationResult ImportVehicles(string excelFilePath, string sheetName, int createUserID)
        ////{
        ////    bool success = false;
        ////    string message = string.Empty;

        ////    #region 检查数据合法性
        ////    var datas = ExcelHelper.ExcelToDataTable(excelFilePath, sheetName);
        ////    if (datas == null)
        ////    {
        ////        message = "导入失败：无法获取文件内容！";
        ////        goto end;
        ////    }
        ////    if (datas.Rows.Count == 0)
        ////    {
        ////        message = "导入失败：没有找到任何车辆资料！";
        ////        goto end;
        ////    }

        ////    string errorMessage;
        ////    string brStr = "</br>";
        ////    //存储车牌号，用于判断DataTable中本身是否存在重复的车牌号
        ////    List<string> plateNums = new List<string>();
        ////    //用于判断同使用单位下是否存在同车代号数据
        ////    List<dynamic> vehiclesNames = new List<dynamic>();
        ////    //存储主定位终端号，用于判断DataTable中本身是否存在重复的信息
        ////    List<string> primaryTerminalCodes = new List<string>();
        ////    for (int i = 0; i < datas.Rows.Count; i++)
        ////    {


        ////        errorMessage = string.Empty;
        ////        int rowIndex = i + 1;
        ////        string rowMessage = string.Format(PromptInformation.RowIndex, rowIndex);


        ////        #region 获取当前行数据
        ////        var currentRow = datas.Rows[i];
        ////        string plateNumStr = currentRow[0].ToString().Trim();//                    车牌号                           
        ////        string vehicleNameStr = currentRow[1].ToString().Trim();//                 车代号                      
        ////        string plateColorStr = currentRow[2].ToString().Trim();//                  车牌颜色                      
        ////        string speedLimitStr = currentRow[3].ToString().Trim();//               限速值
        ////        string drivingTimeStr = currentRow[4].ToString().Trim();//              疲劳驾驶时间
        ////        string isReceivedStr = currentRow[5].ToString().Trim();//               接收信号
        ////        string isAccessStr = currentRow[6].ToString().Trim();//                 货运平台接入
        ////        string isTransmitStr = currentRow[7].ToString().Trim();//               转发运管
        ////        string isDangerousStr = currentRow[8].ToString().Trim();//              危险品车
        ////        string warrantyDateStr = currentRow[9].ToString().Trim();//                 保固日期
        ////        string iconStr = currentRow[10].ToString().Trim();//                            车辆图标
        ////        string vehicleTypeStr = currentRow[11].ToString().Trim();//                 车辆类型
        ////        string structureStr = currentRow[12].ToString().Trim();//                      使用单位
        ////        string ownershipStr = currentRow[13].ToString().Trim();//                  产权单位
        ////        string remarkStr = currentRow[14].ToString().Trim();//                      备注 
        ////        string primaryTerminalStr = currentRow[15].ToString().Trim();//主定位终端号
        ////        #endregion

        ////        string fieldName = string.Empty;


        ////        #region 检查限速值
        ////        fieldName = DisplayText.SpeedLimit;
        ////        int speedLimitValue = -1;
        ////        if (string.IsNullOrWhiteSpace(speedLimitStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!int.TryParse(speedLimitStr, out speedLimitValue) || (speedLimitValue < 30 || speedLimitValue > 120))
        ////        {
        ////            errorMessage += fieldName + "范围不正确（30-12）" + brStr;
        ////        }

        ////        #endregion

        ////        #region 检查疲劳驾驶时间
        ////        fieldName = DisplayText.DrivingTime;
        ////        int drivingTimeValue = -1;
        ////        if (string.IsNullOrWhiteSpace(drivingTimeStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!int.TryParse(drivingTimeStr, out drivingTimeValue) || (drivingTimeValue < 1 || drivingTimeValue > 300))
        ////        {
        ////            errorMessage += fieldName + "范围不正确（1-300）" + brStr;
        ////        }

        ////        #endregion

        ////        #region 检查车辆图标
        ////        fieldName = DisplayText.Icon;
        ////        List<string> icons = new List<string>()
        ////        {
        ////            "Default","Arrow","MixerTruck","PumpTruck"
        ////        };
        ////        if (string.IsNullOrWhiteSpace(iconStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (icons.SingleOrDefault(m => m == iconStr) == null)
        ////        {
        ////            errorMessage += fieldName + "不属于Default、Arrow、MixerTruck、PumpTruck其中之一" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查保固日期
        ////        fieldName = DisplayText.WarrantyDate;
        ////        DateTime warrantyDateValue = DateTime.Today;
        ////        if (string.IsNullOrWhiteSpace(warrantyDateStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!DateTime.TryParse(warrantyDateStr, out warrantyDateValue))
        ////        {
        ////            errorMessage += fieldName + "不是正确的日期格式" + brStr;
        ////        }
        ////        else if (DateTime.Today.Subtract(warrantyDateValue).TotalDays > 0)
        ////        {
        ////            errorMessage += string.Format(DataAnnotations.WarrantyDateError, fieldName, DateTime.Now.ToString("yyyy-MM-dd")) + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查接收信号
        ////        fieldName = DisplayText.IsReceived;
        ////        bool isReceivedValue = false;
        ////        if (string.IsNullOrWhiteSpace(isReceivedStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!bool.TryParse(isReceivedStr, out isReceivedValue))
        ////        {
        ////            errorMessage += fieldName + "只能是TRUE或FALSE" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查货运平台接入
        ////        fieldName = DisplayText.IsAccess;
        ////        bool isAccessValue = false;
        ////        if (string.IsNullOrWhiteSpace(isAccessStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!bool.TryParse(isAccessStr, out isAccessValue))
        ////        {
        ////            errorMessage += fieldName + "只能是TRUE或FALSE" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查转发运管
        ////        fieldName = DisplayText.IsTransmit;
        ////        bool isTransmitValue = false;
        ////        if (string.IsNullOrWhiteSpace(isTransmitStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!bool.TryParse(isTransmitStr, out isTransmitValue))
        ////        {
        ////            errorMessage += fieldName + "只能是TRUE或FALSE" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查危险品车
        ////        fieldName = DisplayText.IsDangerous;
        ////        bool isDangerousValue = false;
        ////        if (string.IsNullOrWhiteSpace(isDangerousStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!bool.TryParse(isDangerousStr, out isDangerousValue))
        ////        {
        ////            errorMessage += fieldName + "只能是TRUE或FALSE" + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查备注
        ////        fieldName = DisplayText.Remark;
        ////        if (remarkStr.Length > 500)
        ////        {
        ////            errorMessage += string.Format(DataAnnotations.MaxLength, fieldName, 500) + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查车牌号
        ////        fieldName = DisplayText.PlateNum;
        ////        if (string.IsNullOrWhiteSpace(plateNumStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查长度
        ////        else if (plateNumStr.Length != 7)
        ////        {
        ////            errorMessage += DataAnnotations.PlateNumLengthError + brStr;
        ////        }
        ////        //检查是否重复
        ////        else if (CheckPlateNumExists(plateNumStr))
        ////        {
        ////            errorMessage += string.Format(DataAnnotations.FieldExists, fieldName) + brStr;
        ////        }


        ////        #endregion

        ////        #region 检查使用单位（先检查单位，因为车代号的检查需要用到使用单位ID）
        ////        fieldName = DisplayText.StrucWhichUseVehicle;
        ////        int structureID = -1;
        ////        if (string.IsNullOrWhiteSpace(structureStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查使用单位名是否存在，并且获取真正的使用单位ID
        ////        else if (!StructureBLL.TryGetStructureIDByName(structureStr, out structureID))
        ////        {
        ////            errorMessage += "不存在" + fieldName + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查车代号
        ////        fieldName = DisplayText.VehicleName;
        ////        if (string.IsNullOrWhiteSpace(vehicleNameStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查长度
        ////        else if (vehicleNameStr.Length > 20)
        ////        {
        ////            errorMessage += string.Format(DataAnnotations.MaxLength, fieldName, 20) + brStr;
        ////        }
        ////        //检查是否重复
        ////        else if (CheckVehicleNameExists(vehicleNameStr, structureID))
        ////        {
        ////            errorMessage += DataAnnotations.VehicleNameExists + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查车牌颜色
        ////        fieldName = DisplayText.PlateColor;
        ////        int plateCodeValue = -1;
        ////        if (string.IsNullOrWhiteSpace(plateColorStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查车牌颜色合法性
        ////        else if (!PlateColorBLL.TryGetCodeByName(plateColorStr, out plateCodeValue))
        ////        {
        ////            errorMessage += "不存在" + fieldName + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查产权所属单位
        ////        fieldName = DisplayText.Ownership;
        ////        int ownershipID = -1;
        ////        if (string.IsNullOrWhiteSpace(ownershipStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        //检查产权单位名是否存在，并且获取真正的产权单位ID
        ////        else if (!StructureBLL.TryGetStructureIDByName(ownershipStr, out ownershipID))
        ////        {
        ////            errorMessage += "不存在" + fieldName + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查车辆类型
        ////        fieldName = DisplayText.VehicleType;
        ////        int vehicleTypeCode = -1;
        ////        if (string.IsNullOrWhiteSpace(vehicleTypeStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!VehicleTypeBLL.TryGetCodeByName(vehicleTypeStr, out vehicleTypeCode))
        ////        {
        ////            errorMessage += "不存在" + fieldName + brStr;
        ////        }
        ////        #endregion

        ////        #region 检查主定位终端号
        ////        fieldName = DisplayText.PrimaryTerminalCode;
        ////        long primaryTerminalID = -1;
        ////        if (string.IsNullOrWhiteSpace(primaryTerminalStr))
        ////        {
        ////            errorMessage += "缺失" + fieldName + brStr;
        ////        }
        ////        else if (!TerminalBLL.TryGetIDByTerminalCode(primaryTerminalStr, out primaryTerminalID))
        ////        {
        ////            errorMessage += "不存在" + fieldName + "或已被使用" + brStr;
        ////        }
        ////        #endregion


        ////        #region
        ////        if (!string.IsNullOrWhiteSpace(errorMessage))
        ////        {
        ////            message = "导入失败：" + brStr + rowMessage + brStr + errorMessage;
        ////            goto end;
        ////        }
        ////        else
        ////        {
        ////            if (plateNums.Exists(pn => pn == plateNumStr))
        ////            {
        ////                message = "导入失败：Excel文件中存在重复车牌号：" + plateNumStr;
        ////                goto end;
        ////            }
        ////            if (vehiclesNames.Exists(t => t.SID == structureID && t.VN == vehicleNameStr))
        ////            {
        ////                message = "导入失败：Excel文件中存在同使用单位下的同名车代号：" + vehicleNameStr + "_" + structureStr;
        ////                goto end;
        ////            }
        ////            if (primaryTerminalCodes.Exists(tc => tc == primaryTerminalStr))
        ////            {
        ////                message = "导入失败：Excel文件中存在重复的主定位终端号：" + primaryTerminalStr;
        ////                goto end;
        ////            }
        ////            vehiclesNames.Add(new { SID = structureID, VN = vehicleNameStr });
        ////            plateNums.Add(plateNumStr);
        ////            primaryTerminalCodes.Add(primaryTerminalStr);

        ////            VehicleAddModel model = new VehicleAddModel()
        ////            {
        ////                PlateNum = plateNumStr,
        ////                VehicleName = vehicleNameStr,
        ////                PlateColorCode = plateCodeValue,
        ////                SpeedLimit = speedLimitValue,
        ////                DrivingTime = drivingTimeValue,
        ////                IsReceived = isReceivedValue,
        ////                IsAccess = isAccessValue,
        ////                IsTransmit = isTransmitValue,
        ////                IsDangerous = isDangerousValue,
        ////                WarrantyDate = warrantyDateValue.ToString("yyyy-MM-dd"),
        ////                Icon = iconStr,
        ////                VehicleTypeCode = vehicleTypeCode,
        ////                StrucID = structureID,
        ////                Ownership = ownershipID,
        ////                Remark = remarkStr,
        ////                PrimaryTerminalID = primaryTerminalID
        ////            };

        ////            if (!AddVehicle(model).Success)
        ////            {
        ////                message += string.Format("{0}{1}{2}", rowMessage, "导入失败", brStr);
        ////            }

        ////        }
        ////        #endregion
        ////    }
        ////    #endregion

        ////    if (string.IsNullOrWhiteSpace(message))
        ////    {
        ////        success = true;
        ////        message = PromptInformation.ImportSuccess;
        ////    }

        ////end:
        ////    return new OperationResult()
        ////    {
        ////        Success = success,
        ////        Message = message
        ////    };
        ////}

        ///// <summary>
        ///// 检查导入的车辆资料合法性
        ///// </summary>
        //private static bool CheckImportVehicleInfo(DataRow currentRow, List<string> errorMessages, List<string> plateNums, List<string> primaryTerminalCodes, List<dynamic> vehiclesNames, VehicleAddModel model)
        //{
        //    string fieldName = string.Empty;

        //    #region 获取当前行数据
        //    string plateNumStr = currentRow[0].ToString().Trim();//                    车牌号                           
        //    string vehicleNameStr = currentRow[1].ToString().Trim();//                 车代号                      
        //    string plateColorStr = currentRow[2].ToString().Trim();//                  车牌颜色                      
        //    string speedLimitStr = currentRow[3].ToString().Trim();//               限速值
        //    string drivingTimeStr = currentRow[4].ToString().Trim();//              疲劳驾驶时间
        //    string isReceivedStr = currentRow[5].ToString().Trim();//               接收信号
        //    string isAccessStr = currentRow[6].ToString().Trim();//                 货运平台接入
        //    string isTransmitStr = currentRow[7].ToString().Trim();//               转发运管
        //    string isDangerousStr = currentRow[8].ToString().Trim();//              危险品车
        //    string warrantyDateStr = currentRow[9].ToString().Trim();//                 保固日期
        //    string iconStr = currentRow[10].ToString().Trim();//                            车辆图标
        //    string vehicleTypeStr = currentRow[11].ToString().Trim();//                 车辆类型
        //    string structureStr = currentRow[12].ToString().Trim();//                      使用单位
        //    string ownershipStr = currentRow[13].ToString().Trim();//                  产权单位
        //    string remarkStr = currentRow[14].ToString().Trim();//                      备注 
        //    string primaryTerminalStr = currentRow[15].ToString().Trim();//主定位终端号
        //    #endregion

        //    #region 检查限速值
        //    fieldName = DisplayText.SpeedLimit;
        //    int speedLimitValue = -1;
        //    if (string.IsNullOrWhiteSpace(speedLimitStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (!int.TryParse(speedLimitStr, out speedLimitValue) || (speedLimitValue < 30 || speedLimitValue > 120))
        //    {
        //        errorMessages.Add(string.Format(DataAnnotations.RangeError, fieldName, 30, 120));
        //    }

        //    #endregion

        //    #region 检查疲劳驾驶时间
        //    fieldName = DisplayText.DrivingTime;
        //    int drivingTimeValue = -1;
        //    if (string.IsNullOrWhiteSpace(drivingTimeStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (!int.TryParse(drivingTimeStr, out drivingTimeValue) || (drivingTimeValue < 1 || drivingTimeValue > 300))
        //    {
        //        errorMessages.Add(string.Format(DataAnnotations.RangeError, fieldName, 1, 300));
        //    }

        //    #endregion

        //    #region 检查车辆图标
        //    fieldName = DisplayText.Icon;
        //    List<string> icons = new List<string>()
        //        {
        //            "Default","Arrow","MixerTruck","PumpTruck"
        //        };
        //    if (string.IsNullOrWhiteSpace(iconStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (icons.SingleOrDefault(m => m == iconStr) == null)
        //    {
        //        errorMessages.Add(string.Format("{0}：{1} {2}", fieldName, iconStr, PromptInformation.Invalid));
        //    }
        //    #endregion

        //    #region 检查保固日期
        //    fieldName = DisplayText.WarrantyDate;
        //    DateTime warrantyDateValue = DateTime.Today;
        //    if (string.IsNullOrWhiteSpace(warrantyDateStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (!DateTime.TryParse(warrantyDateStr, out warrantyDateValue))
        //    {
        //        errorMessages.Add(fieldName + " " + DataAnnotations.DateError);
        //    }
        //    else if (DateTime.Today.Subtract(warrantyDateValue).TotalDays > 0)
        //    {
        //        errorMessages.Add(string.Format(DataAnnotations.WarrantyDateError, fieldName, DateTime.Now.ToString("yyyy-MM-dd")));
        //    }
        //    #endregion

        //    #region 检查接收信号
        //    fieldName = DisplayText.IsReceived;
        //    bool isReceivedValue = false;
        //    if (string.IsNullOrWhiteSpace(isReceivedStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (!bool.TryParse(isReceivedStr, out isReceivedValue))
        //    {
        //        errorMessages.Add(fieldName + PromptInformation.OnlyBoolean);
        //    }
        //    #endregion

        //    #region 检查货运平台接入
        //    fieldName = DisplayText.IsAccess;
        //    bool isAccessValue = false;
        //    if (string.IsNullOrWhiteSpace(isAccessStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (!bool.TryParse(isAccessStr, out isAccessValue))
        //    {
        //        errorMessages.Add(fieldName + PromptInformation.OnlyBoolean);
        //    }
        //    #endregion

        //    #region 检查转发运管
        //    fieldName = DisplayText.IsTransmit;
        //    bool isTransmitValue = false;
        //    if (string.IsNullOrWhiteSpace(isTransmitStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (!bool.TryParse(isTransmitStr, out isTransmitValue))
        //    {
        //        errorMessages.Add(fieldName + PromptInformation.OnlyBoolean);
        //    }
        //    #endregion

        //    #region 检查危险品车
        //    fieldName = DisplayText.IsDangerous;
        //    bool isDangerousValue = false;
        //    if (string.IsNullOrWhiteSpace(isDangerousStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (!bool.TryParse(isDangerousStr, out isDangerousValue))
        //    {
        //        errorMessages.Add(fieldName + PromptInformation.OnlyBoolean);
        //    }
        //    #endregion

        //    #region 检查备注
        //    fieldName = DisplayText.Remark;
        //    if (remarkStr.Length > 500)
        //    {
        //        errorMessages.Add(string.Format(DataAnnotations.MaxLength, fieldName, 500));
        //    }
        //    #endregion

        //    #region 检查车牌号
        //    fieldName = DisplayText.PlateNum;
        //    if (string.IsNullOrWhiteSpace(plateNumStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    //检查长度
        //    else if (plateNumStr.Length != 7 && plateNumStr.Length != 8)
        //    {
        //        errorMessages.Add(DataAnnotations.PlateNumLengthError);
        //    }
        //    else if (plateNums.Exists(pn => pn == plateNumStr))
        //    {
        //        errorMessages.Add(PromptInformation.SamePlateNumInExcel + "：" + plateNumStr);
        //    }
        //    //检查是否重复
        //    else if (CheckPlateNumExists(plateNumStr))
        //    {
        //        errorMessages.Add(string.Format(DataAnnotations.FieldExists, fieldName));
        //    }
        //    else
        //    {
        //        plateNums.Add(plateNumStr);
        //    }
        //    #endregion

        //    #region 检查使用单位（先检查单位，因为车代号的检查需要用到使用单位ID）
        //    fieldName = DisplayText.StrucWhichUseVehicle;
        //    int structureID = -1;
        //    if (string.IsNullOrWhiteSpace(structureStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    //检查使用单位名是否存在，并且获取真正的使用单位ID
        //    else if (!StructureBLL.TryGetStructureIDByName(structureStr, out structureID))
        //    {
        //        errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, structureStr, PromptInformation.NotExists));
        //    }
        //    #endregion

        //    #region 检查车代号
        //    fieldName = DisplayText.VehicleName;
        //    if (string.IsNullOrWhiteSpace(vehicleNameStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    //检查长度
        //    else if (vehicleNameStr.Length > 20)
        //    {
        //        errorMessages.Add(string.Format(DataAnnotations.MaxLength, fieldName, 20));
        //    }
        //    else if (vehiclesNames.Exists(t => t.SID == structureID && t.VN == vehicleNameStr))
        //    {
        //        errorMessages.Add(string.Format("{0}：{1}{2}{3}{4}{5}{6}{7}", PromptInformation.InvalidVehicelNameInExcel, DisplayText.VehicleName, "：", vehicleNameStr, "、", DisplayText.StrucWhichUseVehicle, "：", structureStr));
        //    }
        //    //检查是否重复
        //    else if (CheckVehicleNameExists(vehicleNameStr, structureID))
        //    {
        //        errorMessages.Add(DataAnnotations.VehicleNameExists);
        //    }
        //    else
        //    {
        //        vehiclesNames.Add(new { SID = structureID, VN = vehicleNameStr });
        //    }
        //    #endregion

        //    #region 检查车牌颜色
        //    fieldName = DisplayText.PlateColor;
        //    int plateCodeValue = -1;
        //    if (string.IsNullOrWhiteSpace(plateColorStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    //检查车牌颜色合法性
        //    else if (!PlateColorBLL.TryGetCodeByName(plateColorStr, out plateCodeValue))
        //    {
        //        errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, plateColorStr, PromptInformation.NotExists));
        //    }
        //    #endregion

        //    #region 检查产权所属单位
        //    fieldName = DisplayText.Ownership;
        //    int ownershipID = -1;
        //    if (string.IsNullOrWhiteSpace(ownershipStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    //检查产权单位名是否存在，并且获取真正的产权单位ID
        //    else if (!StructureBLL.TryGetStructureIDByName(ownershipStr, out ownershipID))
        //    {
        //        errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, ownershipStr, PromptInformation.NotExists));
        //    }
        //    #endregion

        //    #region 检查车辆类型
        //    fieldName = DisplayText.VehicleType;
        //    int vehicleTypeCode = -1;
        //    if (string.IsNullOrWhiteSpace(vehicleTypeStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (!VehicleTypeBLL.TryGetCodeByName(vehicleTypeStr, out vehicleTypeCode))
        //    {
        //        errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, vehicleTypeStr, PromptInformation.NotExists));
        //    }
        //    #endregion

        //    #region 检查主定位终端号
        //    fieldName = DisplayText.PrimaryTerminalCode;
        //    long primaryTerminalID = -1;
        //    if (string.IsNullOrWhiteSpace(primaryTerminalStr))
        //    {
        //        errorMessages.Add(PromptInformation.MissingField + fieldName);
        //    }
        //    else if (primaryTerminalCodes.Exists(tc => tc == primaryTerminalStr))
        //    {
        //        errorMessages.Add(PromptInformation.SamePrimaryTerminalCodeInExcel + "：" + primaryTerminalStr);
        //    }
        //    else if (!TerminalBLL.TryGetIDByTerminalCode(primaryTerminalStr, out primaryTerminalID))
        //    {
        //        errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, primaryTerminalStr, PromptInformation.NotExistsOrHasBeenUsed));
        //    }
        //    else
        //    {
        //        primaryTerminalCodes.Add(primaryTerminalStr);
        //    }
        //    #endregion

        //    model.PlateNum = plateNumStr;
        //    model.VehicleName = vehicleNameStr;
        //    model.PlateColorCode = plateCodeValue;
        //    model.IsReceived = isReceivedValue;
        //    model.IsAccess = isAccessValue;
        //    model.IsTransmit = isTransmitValue;
        //    model.IsDangerous = isDangerousValue;
        //    model.WarrantyDate = warrantyDateValue.ToString("yyyy-MM-dd");
        //    model.Icon = iconStr;
        //    model.VehicleTypeCode = vehicleTypeCode;
        //    model.AddStrucID = structureID;
        //    model.Ownership = ownershipID;
        //    model.Remark = remarkStr;
        //    model.PrimaryTerminalID = primaryTerminalID;

        //    return errorMessages.Count == 0;
        //}

        ///// <summary>
        ///// 批量导入车辆信息
        ///// </summary>
        ///// <param name="excelFilePath">待导入的Excel文件绝对物理路径</param>
        ///// <param name="sheetName">车辆资料对应的SheetName</param>
        ///// <param name="createUserID">导入车辆资料的用户ID</param>
        ///// <returns></returns>
        //public static OperationResult ImportVehicles(string excelFilePath, string sheetName, int createUserID)
        //{
        //    bool success = false;
        //    string message = string.Empty;
        //    string brStr = "<br/>";
        //    var datas = ExcelHelper.ExcelToDataTable(excelFilePath, sheetName);
        //    if (datas == null)
        //    {
        //        message = PromptInformation.AccessExcelFailed;
        //    }
        //    else if (datas.Rows.Count == 0)
        //    {
        //        message = PromptInformation.NoVehicles;
        //    }
        //    else
        //    {
        //        //存储车牌号，用于判断DataTable中本身是否存在重复的车牌号
        //        List<string> plateNums = new List<string>();
        //        //用于判断同使用单位下是否存在同车代号数据
        //        List<dynamic> vehiclesNames = new List<dynamic>();
        //        //存储主定位终端号，用于判断DataTable中本身是否存在重复的信息
        //        List<string> primaryTerminalCodes = new List<string>();
        //        //存储主车架号，用于判断excel中本身是否存在重复的信息
        //        List<string> VINs = new List<string>();
        //        //存储待新增的车辆数
        //        Dictionary<string, EditVehicleModel> addDic = new Dictionary<string, EditVehicleModel>();
        //        int i = 0;
        //        for (i = 0; i < datas.Rows.Count; i++)
        //        {
        //            int rowNum = i + 2;
        //            string rowMessage = string.Format(PromptInformation.RowIndex, rowNum);
        //            EditVehicleModel addModel = new EditVehicleModel();
        //            List<string> errorMessage = new List<string>();
        //            var currentRow = datas.Rows[i];
        //            if (CheckImportVehicleInfo(currentRow, errorMessage,
        //                plateNums, primaryTerminalCodes, vehiclesNames,VINs, addModel))
        //            {
        //                addDic.Add(rowMessage, addModel);
        //            }
        //            else//只要有一条失败，马上返回提示，不继续检查
        //            {
        //                message = rowMessage + brStr;
        //                errorMessage.ForEach(msg =>
        //                {
        //                    message += string.Format("{0}{1}", msg, brStr);
        //                });
        //                break;
        //            }
        //        }

        //        if (i == datas.Rows.Count)//全部都检查无误才对数据进行新增
        //        {
        //            int failedCount = 0;
        //            addDic.ToList().ForEach(kv =>
        //            {
        //                if (!AddVehicleNew(kv.Value).Success)
        //                {
        //                    message += string.Format("{0}{1}", kv.Key, brStr);
        //                    failedCount++;
        //                }
        //            });
        //            success = failedCount == 0;
        //        }
        //    }
        //    return new OperationResult()
        //    {
        //        Success = success,
        //        Message = success ? PromptInformation.ImportSuccess : PromptInformation.ImportFailed + brStr + message
        //    };
        //}
        #endregion


        #region 批量导入车辆
        /// <summary>
        /// 检查导入的车辆资料合法性
        /// </summary>
        private static bool CheckImportVehicleInfo(DataRow currentRow, List<string> errorMessages, List<string> plateNums, List<string> primaryTerminalCodes, List<dynamic> vehiclesNames, List<string> VINs, EditVehicleModel model)
        {
            string fieldName = string.Empty;

            #region 获取当前行数据
            string plateNumStr = currentRow[0].ToString().Trim();//                    车牌号                           
            string vehicleNameStr = currentRow[1].ToString().Trim();//                 车辆代号    
            string isReceivedStr = currentRow[2].ToString().Trim();//               平台信号接收      
            string isAccessStr = currentRow[3].ToString().Trim();//                 货运平台接入

            string plateColorStr = currentRow[4].ToString().Trim();//                  车牌颜色               
            string vehicleTypeStr = currentRow[5].ToString().Trim();//                 车辆类型
            string primaryTerminalStr = currentRow[6].ToString().Trim();//         定位终端
            string UserStrucName = currentRow[7].ToString().Trim();//                      使用单位
            string OwnerStrucName = currentRow[8].ToString().Trim();//                 产权所属
            string iconStr = currentRow[9].ToString().Trim();//                            车辆图标
            string VINStr = currentRow[10].ToString().Trim();//                           车架号
            string SoftwareDateStr = currentRow[11].ToString().Trim();//                           软件服务到期日期
            string WarrantyDateStr = currentRow[12].ToString().Trim();//                           硬件服务到期日期
            string remarkStr = currentRow[13].ToString().Trim();//                      备注 


            #endregion



            #region 检查车牌号
            fieldName = DisplayText.PlateNum;
            if (string.IsNullOrWhiteSpace(plateNumStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查长度
            else if (plateNumStr.Length != 7 && plateNumStr.Length != 8)
            {
                errorMessages.Add(DataAnnotations.PlateNumLengthError);
            }
            else if (plateNums.Exists(pn => pn == plateNumStr))
            {
                errorMessages.Add(PromptInformation.SamePlateNumInExcel + "：" + plateNumStr);
            }
            //检查是否重复
            else if (CheckPlateNumExists(plateNumStr))
            {
                errorMessages.Add(string.Format(DataAnnotations.FieldExists, fieldName));
            }
            else
            {
                plateNums.Add(plateNumStr);
            }
            #endregion

            #region 检查使用单位（先检查单位，因为车代号的检查需要用到使用单位ID）
            fieldName = DisplayText.StrucWhichUseVehicle;
            int structureID = -1;
            if (string.IsNullOrWhiteSpace(UserStrucName))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查使用单位名是否存在，并且获取真正的使用单位ID
            else if (!StructureBLL.TryGetStructureIDByName(UserStrucName, out structureID))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, UserStrucName, PromptInformation.NotExists));
            }
            #endregion

            #region 检查车辆代号(使用单位有效前提下，再检查车辆代号有效性)
            if (structureID != -1)
            {
                fieldName = DisplayText.VehicleName;
                if (string.IsNullOrWhiteSpace(vehicleNameStr))
                {
                    errorMessages.Add(PromptInformation.MissingField + fieldName);
                }
                //检查长度
                else if (vehicleNameStr.Length > 20)
                {
                    errorMessages.Add(string.Format(DataAnnotations.MaxLength, fieldName, 20));
                }
                else if (vehiclesNames.Exists(t => t.SID == structureID && t.VN == vehicleNameStr))
                {
                    errorMessages.Add(string.Format("{0}：{1}{2}{3}{4}{5}{6}{7}", PromptInformation.InvalidVehicelNameInExcel, DisplayText.VehicleName, "：", vehicleNameStr, "、", DisplayText.StrucWhichUseVehicle, "：", UserStrucName));
                }
                //检查是否重复
                else if (CheckVehicleNameExists(vehicleNameStr, structureID))
                {
                    errorMessages.Add(DataAnnotations.VehicleNameExists);
                }
                else
                {
                    vehiclesNames.Add(new { SID = structureID, VN = vehicleNameStr });
                }
            }
            #endregion

            #region 检查接收信号
            fieldName = DisplayText.IsReceived;

            bool isReceivedValue = false;
            if (string.IsNullOrWhiteSpace(isReceivedStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            else if (!VehicleBLL.TryGetBoolByName(isReceivedStr, out isReceivedValue))
            {
                errorMessages.Add(fieldName + PromptInformation.UnVaildBool);
            }
            #endregion

            #region 检查货运平台接入
            fieldName = DisplayText.IsAccess;
            bool isAccessValue = false;
            if (string.IsNullOrWhiteSpace(isAccessStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            else if (!VehicleBLL.TryGetBoolByName(isAccessStr, out isAccessValue))
            {
                errorMessages.Add(fieldName + PromptInformation.UnVaildBool);
            }
            #endregion

            #region 检查车牌颜色
            fieldName = DisplayText.PlateColor;
            int plateCodeValue = -1;
            if (string.IsNullOrWhiteSpace(plateColorStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查长度
            else if (plateColorStr.Length > 2)
            {
                errorMessages.Add(string.Format(DataAnnotations.MaxLength, fieldName, 2));
            }
            //检查车牌颜色合法性
            else if (!PlateColorBLL.TryGetCodeByName(plateColorStr, out plateCodeValue))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, plateColorStr, PromptInformation.NotExists));
            }
            #endregion

            #region 检查车辆类型
            fieldName = DisplayText.VehicleType;
            int vehicleTypeCode = -1;
            if (string.IsNullOrWhiteSpace(vehicleTypeStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            else if (!VehicleTypeBLL.TryGetCodeByName(vehicleTypeStr, out vehicleTypeCode))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, vehicleTypeStr, PromptInformation.NotExists));
            }
            #endregion

            #region 检查主定位终端号
            fieldName = DisplayText.PrimaryTerminalCode;
            long primaryTerminalID = -1;
            string errMsg = "";
            if (string.IsNullOrWhiteSpace(primaryTerminalStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查主定位终端号长度
            else if (primaryTerminalStr.Length > 30)
            {
                errorMessages.Add(string.Format(DataAnnotations.MaxLength, fieldName, 30));
            }
            else if (primaryTerminalCodes.Exists(tc => tc == primaryTerminalStr))
            {
                errorMessages.Add(PromptInformation.SamePrimaryTerminalCodeInExcel + "：" + primaryTerminalStr);
            }
            else if (!VehicleBLL.CheckTerminalCodeExistsAndNotUsed(primaryTerminalStr, out errMsg, out primaryTerminalID))
            {
                errorMessages.Add(errMsg);
            }
            else
            {
                primaryTerminalCodes.Add(primaryTerminalStr);
            }
            #endregion

            #region 检查产权所属
            fieldName = DisplayText.Ownership;
            int ownershipID = -1;
            if (string.IsNullOrWhiteSpace(OwnerStrucName))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查产权单位名是否存在，并且获取真正的产权单位ID
            else if (!StructureBLL.TryGetStructureIDByName(OwnerStrucName, out ownershipID))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, OwnerStrucName, PromptInformation.NotExists));
            }
            #endregion

            #region 检查车辆图标
            fieldName = DisplayText.Icon;
            string iconValue = "";

            if (string.IsNullOrWhiteSpace(iconStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            else if (!VehicleBLL.TryGetcodeByIconName(iconStr, out iconValue))
            {
                errorMessages.Add(string.Format("{0}：{1} {2}", fieldName, iconStr, PromptInformation.Invalid));
            }
            #endregion

            #region 检查车架号
            fieldName = DisplayText.VIN;
            Regex reg = new Regex(@"^[a-zA-Z0-9]{17}$");
            if (string.IsNullOrWhiteSpace(VINStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查车架号格式
            else if (!reg.IsMatch(VINStr))
            {
                errorMessages.Add(DataAnnotations.VINError);
            }
            //检查车架号在excel中是否重复
            else if (VINs.Exists(sc => sc == VINStr))
            {
                errorMessages.Add(PromptInformation.SameVINInExcel + "：" + VINStr);
            }
            //检查车架号重复验证
            else if (CheckVINExists(VINStr))
            {
                errorMessages.Add(string.Format(DataAnnotations.FieldExists, fieldName));
            }
            else
            {
                VINs.Add(VINStr);
            }
            #endregion

            #region 检查软件服务到期日期
            fieldName = DisplayText.SoftwareDate;
            DateTime SoftwareDateValue = DateTime.Today;
            if (string.IsNullOrWhiteSpace(SoftwareDateStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            else if (!DateTime.TryParse(SoftwareDateStr, out SoftwareDateValue))
            {
                errorMessages.Add(fieldName + " " + DataAnnotations.DateError);
            }
            #endregion

            #region 检查硬件服务到期日期
            fieldName = DisplayText.HardwareDate;
            DateTime WarrantyDateValue = DateTime.Today;
            if (string.IsNullOrWhiteSpace(WarrantyDateStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            else if (!DateTime.TryParse(WarrantyDateStr, out WarrantyDateValue))
            {
                errorMessages.Add(fieldName + " " + DataAnnotations.DateError);
            }
            #endregion

            #region 检查备注
            fieldName = DisplayText.Remark;
            if (remarkStr.Length > 500)
            {
                errorMessages.Add(string.Format(DataAnnotations.MaxLength, fieldName, 500));
            }
            #endregion


            model.PlateNum = plateNumStr;
            model.EditStrucID = structureID;
            model.VehicleName = vehicleNameStr;
            model.IsReceived = isReceivedValue;
            model.IsAccess = isAccessValue;
            model.PlateColorCode = plateCodeValue;
            model.VehicleTypeCode = vehicleTypeCode;
            model.PrimaryTerminalID = primaryTerminalID;
            model.TerminalCode = primaryTerminalStr;
            model.Ownership = ownershipID;
            model.Icon = iconValue;
            model.VIN = VINStr;
            model.SoftwareDate = SoftwareDateValue.ToString("yyyy-MM-dd");
            model.WarrantyDate = WarrantyDateValue.ToString("yyyy-MM-dd");
            model.Remark = remarkStr;

            model.IsDangerous = false;


            return errorMessages.Count == 0;
        }

        /// <summary>
        /// 检查车架号是否重复
        /// </summary>
        public static bool CheckVINExists(string VIN)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VIN",SqlDbType.Char,17),
            };
            paras[0].Value = VIN.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VIN=@VIN";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 是，否验证，并转true，false
        /// </summary>
        /// <param name="isReceivedStr"></param>
        /// <param name="isReceivedValue"></param>
        /// <returns></returns>
        public static bool TryGetBoolByName(string isReceivedStr, out bool isReceivedValue)
        {
            List<string> receivedItems = new List<string> { DisplayText.Yes, DisplayText.No };
            if (receivedItems.SingleOrDefault(value => { return value == isReceivedStr; }) != null)
            {
                if (isReceivedStr == DisplayText.Yes)
                    isReceivedValue = true;
                else
                    isReceivedValue = false;
                return true;
            }
            else
            {
                isReceivedValue = false;
                return false;
            }
        }

        /// <summary>
        /// 图标名称合法验证，并转编码
        /// </summary>
        /// <param name="iconStr"></param>
        /// <param name="iconValue"></param>
        /// <returns></returns>
        public static bool TryGetcodeByIconName(string iconStr, out string iconValue)
        {
            iconValue = "";
            List<string> icons = new List<string>()
                {
                    DisplayText.CarIcon,DisplayText.ArrowIcon,DisplayText.MixerTruckIcon,DisplayText.PumpTruckIcon
                };
            if (icons.SingleOrDefault(value => { return value == iconStr; }) != null)
            {
                if (iconStr == DisplayText.CarIcon)
                    iconValue = "Default";
                else if (iconStr == DisplayText.ArrowIcon)
                    iconValue = "Arrow";
                else if (iconStr == DisplayText.MixerTruckIcon)
                    iconValue = "MixerTruck";
                else if (iconStr == DisplayText.PumpTruckIcon)
                    iconValue = "PumpTruck";
                else
                    iconValue = "";
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查终端是否是存在并且未被使用
        /// </summary>
        /// <param name="TerminalCode"></param>
        /// <param name="errMsg"></param>
        /// <param name="primaryTerminalID"></param>
        /// <returns></returns>
        public static bool CheckTerminalCodeExistsAndNotUsed(string TerminalCode, out string errMsg, out long primaryTerminalID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@TerminalCode",SqlDbType.VarChar,30),
            };
            paras[0].Value = TerminalCode.Trim();


            string sql = @"declare @res int,@sameTerminalCnt int,@useCnt int,@TerminalID bigint
                                        select @res=0,@TerminalID=-1
                                        select  @sameTerminalCnt=count(1) from Terminals where TerminalCode=@TerminalCode
                                        select @useCnt=count(1) from Terminals where TerminalCode=@TerminalCode and Status=7
                                        if @sameTerminalCnt=0
                                          select @res=1
                                        else if @useCnt=1
                                          select @res=2
                                        if @res=0
                                            select  @TerminalID=ID from Terminals where TerminalCode=@TerminalCode
                                        select @res as res,@TerminalID as TerminalID";
            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray());

            if (dt == null || dt.Rows.Count == 0)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                errMsg = PromptInformation.DBError;
                primaryTerminalID = -1;
                return false;
            }
            int? result = Convert.ToInt32(dt.Rows[0]["res"]);
            primaryTerminalID = Convert.ToInt64(dt.Rows[0]["TerminalID"]);
            if (result == 0)
            {
                errMsg = "";
                return true;
            }
            else if (result == 1)
            {
                errMsg = PromptInformation.TerminalCodeNotExist;
                return false;
            }
            else if (result == 2)
            {
                errMsg = PromptInformation.TerminalHasUsed;
                return false;
            }
            else
            {
                errMsg = PromptInformation.TerminalUnknownErr;
                return false;
            }
        }

        /// <summary>
        /// 批量导入车辆信息
        /// </summary>
        /// <param name="excelFilePath">待导入的Excel文件绝对物理路径</param>
        /// <param name="sheetName">车辆资料对应的SheetName</param>
        /// <param name="createUserID">导入车辆资料的用户ID</param>
        /// <returns></returns>
        public static OperationResult ImportVehicles(string excelFilePath, string sheetName, int createUserID)
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
                message = PromptInformation.NoVehicles;
            }
            else
            {
                //存储车牌号，用于判断excel中本身是否存在重复的信息
                List<string> plateNums = new List<string>();
                //存储车辆代号，用于判断excel中本身是否存在重复的信息
                List<dynamic> vehiclesNames = new List<dynamic>();
                //存储主定位终端号，用于判断excel中本身是否存在重复的信息
                List<string> primaryTerminalCodes = new List<string>();
                //存储主车架号，用于判断excel中本身是否存在重复的信息
                List<string> VINs = new List<string>();
                //存储待新增的车辆数
                Dictionary<string, EditVehicleModel> addDic = new Dictionary<string, EditVehicleModel>();
                int i = 0;
                for (i = 0; i < datas.Rows.Count; i++)
                {
                    int rowNum = i + 2;
                    string rowMessage = string.Format(PromptInformation.RowIndex, rowNum);
                    EditVehicleModel addModel = new EditVehicleModel();
                    List<string> errorMessage = new List<string>();
                    var currentRow = datas.Rows[i];
                    if (CheckImportVehicleInfo(currentRow, errorMessage,
                        plateNums, primaryTerminalCodes, vehiclesNames, VINs, addModel))
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
                        if (!AddVehicleNew(kv.Value).Success)
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
        #endregion


        #region 获取用户分配的车辆信息（车代号、车辆使用单位名）
        #region 自由模式
        /// <summary>
        /// 获取用户分配的车辆信息（车代号、车辆使用单位名） 自由模式
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="vehicleName"></param>
        /// <returns></returns>
        public static List<UserVehicles> GetVehiclesAndStrucName(int userID, string vehicleName)
        {
            //            string sql = @"SELECT vt.VID,vt.VehicleName,vt.VIN,s.StrucName FROM dbo.Structures s INNER JOIN
            //            (
            //            SELECT v.VehicleName,v.StrucID,V.ID AS VID,v.VIN FROM dbo.Vehicles v INNER JOIN
            //            (
            //            SELECT StrucID FROM dbo.StructureDistributionInfo
            //            WHERE UserID=@userID) AS temp1 ON v.StrucID=temp1.StrucID
            //            WHERE v.Status=0 AND v.IsReceived=1  AND v.VehicleName LIKE @vehicleName
            //            UNION
            //            SELECT v.VehicleName,v.StrucID,v.ID AS VID,v.VIN FROM dbo.Vehicles v INNER JOIN
            //            (
            //            SELECT VehicleID FROM dbo.VehicleDistributionInfo 
            //            WHERE UserID=@userID) AS temp1 ON v.ID=temp1.VehicleID
            //            WHERE v.Status=0 AND v.IsReceived=1  AND v.VehicleName LIKE @vehicleName
            //            ) AS vt ON s.ID=vt.StrucID";
            string sql = @"SELECT TOP 30 vt.VID,vt.VehicleName,vt.VIN,s.StrucName FROM dbo.Structures s 
                                    INNER JOIN Func_GetVehiclesListByUserID_New(@userID) AS vt ON s.ID=vt.StrucID WHERE vt.VehicleName LIKE @vehicleName";

            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@userID",
                Value = userID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@vehicleName",
                Value = "%" + vehicleName + "%",
            };
            return ConvertToList<UserVehicles>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 默认模式
        /// <summary>
        /// 获取用户分配的车辆信息（车代号、车辆使用单位名） 默认模式
        /// </summary>
        /// <param name="strucID"></param>
        /// <param name="vehicleName"></param>
        /// <returns></returns>
        public static List<UserVehicles> GetDefaultVehiclesAndStrucName(int strucID, string vehicleName)
        {
            string sql = @"SELECT TOP 30 vt.VID,vt.VehicleName,vt.VIN,s.StrucName FROM dbo.Structures s 
                                    INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID(@StrucID) AS vt ON s.ID=vt.StrucID WHERE vt.VehicleName LIKE @vehicleName";
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@vehicleName",
                Value = "%" + vehicleName + "%",
            };
            return ConvertToList<UserVehicles>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion
        #endregion


        #region 获取用户分配的车辆信息（车牌号、车辆使用单位名）
        #region 自由模式
        /// <summary>
        /// 获取用户分配的车辆信息（车牌号、车辆使用单位名） 自由模式
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="plateNum"></param>
        /// <returns></returns>
        public static List<UserVehiclesModel> GetVehiclesAndStrucNameByPlateNum(int userID, string plateNum)
        {
            string sql = @"SELECT TOP 30 vt.VID,vt.VehicleName,vt.VIN,s.StrucName,vt.PlateNum FROM dbo.Structures s 
                                    INNER JOIN Func_GetVehiclesListByUserID_New(@userID) AS vt ON s.ID=vt.StrucID WHERE vt.PlateNum LIKE @plateNum";

            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@userID",
                Value = userID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@plateNum",
                Value = "%" + plateNum + "%",
            };
            return ConvertToList<UserVehiclesModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 默认模式
        /// <summary>
        /// 获取用户分配的车辆信息（车牌号、车辆使用单位名） 默认模式
        /// </summary>
        /// <param name="strucID"></param>
        /// <param name="plateNum"></param>
        /// <returns></returns>
        public static List<UserVehiclesModel> GetDefaultVehiclesAndStrucNameByPlateNum(int strucID, string plateNum)
        {
            string sql = @"SELECT TOP 30 vt.VID,vt.VehicleName,vt.VIN,s.StrucName,vt.PlateNum FROM dbo.Structures s 
                                    INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID(@StrucID) AS vt ON s.ID=vt.StrucID WHERE vt.PlateNum LIKE @plateNum";
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@plateNum",
                Value = "%" + plateNum + "%",
            };
            return ConvertToList<UserVehiclesModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion
        #endregion


        #region  新增编辑车辆信息新版

        #region 根据单位ID获取经营范围类表
        /// <summary>
        /// 获取经营范围类表
        /// </summary>
        /// <param name="strucID">单位ID</param>
        /// <returns></returns>
        public static List<StructureBussinessScopeDDLModel> GetBusinessScopeByStrucID(int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            paras[0].Value = strucID;
            string sql = @"SELECT  a.[BusinessScopeCode],b.name FROM [StructureBussinessScope] AS a
                                    INNER JOIN BusinessScope AS b ON a.BusinessScopeCode = b.code WHERE a.StrucID = @StrucID";
            return ConvertToList<StructureBussinessScopeDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

        }
        #endregion

        #region 根据单位ID对应的运输行业列表
        /// <summary>
        /// 根据单位ID对应的运输行业列表
        /// </summary>
        /// <param name="strucID">单位ID</param>
        /// <returns></returns>
        public static List<StructureTransportIndustryDDLModel> GetTransportIndustryByStrucID(int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            paras[0].Value = strucID;
            string sql = @"SELECT  a.[TransportIndustryCode],b.name FROM [StructureTransportIndustry] AS a
                                    INNER JOIN TransportIndustry AS b ON a.TransportIndustryCode = b.code WHERE a.StrucID = @StrucID";
            return ConvertToList<StructureTransportIndustryDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

        }
        #endregion

        #region 获取员工信息列表信息（驾驶员和押运员）
        /// <summary>
        /// 获取员工信息列表信息（驾驶员和押运员）
        /// </summary>
        /// <param name="strucID">单位ID</param>
        /// <returns></returns>
        public static List<EmployeeInfoDDLModel> GetEmployeeInfoByStrucID(int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            paras[0].Value = strucID;

            string sql = @"SELECT [ID],EmployeeName+'['+DriveCode+']' AS EmployeeName,[IsDriver],0 AS [IsCarrier] FROM [EmployeeInfo] 
                                    WHERE Status = 0 AND IsDriver = 1 AND StrucID = @StrucID  UNION ALL
                                      SELECT [ID],EmployeeName+'['+CarrierCode+']' AS EmployeeName,0 AS [IsDriver],[IsCarrier] FROM [EmployeeInfo] 
                                    WHERE Status = 0 AND IsCarrier = 1 AND StrucID = @StrucID ";
            return ConvertToList<EmployeeInfoDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

        }
        #endregion

        #region 新增车辆信息
        /// <summary>
        /// 新增车辆信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult AddVehicleNew(EditVehicleModel model)
        {
            //1.新增车辆信息到车辆表
            //2.定位终端与车辆关联信息，完善终端表LinkedVehicleID信息  /同时修改终端表终端状态 
            //3.终端使用表 添加终端使用记录
            //4.车辆关联员工表记录  和车辆所属经营范围插入 关联运管所插入

            #region 插入数据到车辆表
            List<SqlParameter> parasList = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar),
                new SqlParameter("@VehicleName",SqlDbType.NVarChar),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@Ownership",SqlDbType.Int),
                new SqlParameter("@PlateColorCode",SqlDbType.TinyInt),
                new SqlParameter("@VehicleTypeCode",SqlDbType.TinyInt),
                new SqlParameter("@IsReceived",SqlDbType.Bit),
                new SqlParameter("@IsAccess",SqlDbType.Bit),
                new SqlParameter("@IsTransmit",SqlDbType.Bit),
                new SqlParameter("@IsDangerous",SqlDbType.Bit),
                new SqlParameter("@Icon",SqlDbType.NVarChar),
                new SqlParameter("@WarrantyDate",SqlDbType.Date),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@VIN",SqlDbType.Char),
                new SqlParameter("@PrimaryTerminalID",SqlDbType.BigInt),
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar),
                new SqlParameter("@DriverID",SqlDbType.Int),
                new SqlParameter("@CarrierID",SqlDbType.Int),
                new SqlParameter("@BusinessScopeList",SqlDbType.VarChar),
                new SqlParameter("@CarToLand",SqlDbType.Char),
                new SqlParameter("@RoadransportNo",SqlDbType.VarChar),
                new SqlParameter("@TransportIndustryCode",SqlDbType.Char),
                new SqlParameter("@SoftwareDate",SqlDbType.Date),
                new SqlParameter("@TransportManagementList",SqlDbType.VarChar),
            };
            parasList[0].Value = model.PlateNum;
            parasList[1].Value = model.VehicleName;
            parasList[2].Value = model.EditStrucID;
            parasList[3].Value = model.Ownership;
            parasList[4].Value = model.PlateColorCode;
            parasList[5].Value = model.VehicleTypeCode;
            parasList[6].Value = model.IsReceived;
            parasList[7].Value = model.IsAccess;
            parasList[8].Value = model.IsTransmit;
            parasList[9].Value = model.IsDangerous;
            parasList[10].Value = model.Icon;
            parasList[11].Value = model.WarrantyDate;
            parasList[12].Value = model.CreateUserID;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                parasList[13].Value = DBNull.Value;
            }
            else
            {
                parasList[13].Value = model.Remark;
            }
            parasList[14].Value = model.VIN;
            parasList[15].Value = model.PrimaryTerminalID;
            parasList[16].Value = model.TerminalCode;
            if (model.IsDangerous)
            {
                parasList[17].Value = model.DriverID;
                parasList[18].Value = model.CarrierID;
                parasList[19].Value = model.BusinessScopeList;
                parasList[20].Value = model.CarToLand;
                parasList[21].Value = model.RoadransportNo;
                parasList[22].Value = model.TransportIndustryCode;
                if (model.IsTransmit)
                {
                    parasList[24].Value = model.TransportManagementList;
                }
                else
                {
                    parasList[24].Value = DBNull.Value;
                }
            }
            else
            {
                parasList[17].Value = DBNull.Value;
                parasList[18].Value = DBNull.Value;
                parasList[19].Value = DBNull.Value;
                parasList[20].Value = DBNull.Value;
                parasList[21].Value = DBNull.Value;
                parasList[22].Value = DBNull.Value;
                parasList[24].Value = DBNull.Value;
            }
            parasList[23].Value = model.SoftwareDate;

            string sql = "DECLARE @IDENTITYID INT;";
            sql += @"INSERT INTO dbo.Vehicles( PlateNum ,VehicleName,StrucID,Ownership,PlateColorCode,VehicleTypeCode,IsReceived ,
                              IsAccess ,IsTransmit ,IsDangerous ,Icon ,WarrantyDate ,CreateTime ,Remark ,Status ,CreateUserID ,
                              VIN,CarToLand,RoadransportNo,TransportIndustryCode,SoftwareDate)
                                VALUES  ( @PlateNum , @VehicleName, @StrucID, @Ownership ,@PlateColorCode , @VehicleTypeCode ,@IsReceived , 
                               @IsAccess , @IsTransmit , @IsDangerous , @Icon, @WarrantyDate, GETDATE() ,@Remark , 0 , @CreateUserID,
                                @VIN,@CarToLand,@RoadransportNo,@TransportIndustryCode,@SoftwareDate);
                               SELECT  @IDENTITYID=SCOPE_IDENTITY();";
            #endregion

            #region  完善终端表LinkedVehicleID信息
            sql += @"UPDATE  dbo.Terminals SET  LinkedVehicleID = @IDENTITYID, Status=7 WHERE   ID = @PrimaryTerminalID;";
            #endregion

            #region  终端使用表添加记录
            sql += @"INSERT INTO dbo.TerminalUsageLogs(TerminalCode ,VehicleID , CreateDateTime ,CreateUserID,TerminalID,PlateNum,VIN)
                             VALUES  ( @TerminalCode , @IDENTITYID, GETDATE() , @CreateUserID,@PrimaryTerminalID,@PlateNum,@VIN);";
            #endregion

            #region 车辆关联驾驶员表记录  和车辆所属经营范围插入
            if (model.IsDangerous)
            {

                // 车辆关联员工表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位

                //sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                //                  (@IDENTITYID,@DriverID,1),(@IDENTITYID,@CarrierID,2);";
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@IDENTITYID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID),
                                1),
                              (@IDENTITYID,  
                                (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @CarrierID),
                               2);";

                // 车辆所属经营范围
                string[] bussinessScopeArray = model.BusinessScopeList.TrimEnd(',').Split(',');
                sql += @"INSERT INTO  [VehicleBussinessScope]([VehicleID],[BusinessScopeCode])VALUES";
                int length = bussinessScopeArray.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i == length - 1)
                    {
                        sql += string.Format("(@IDENTITYID,'{0}');", bussinessScopeArray[i]);
                    }
                    else
                    {
                        sql += string.Format("(@IDENTITYID,'{0}'),", bussinessScopeArray[i]);
                    }
                }

                // 是危险品车 而且选择了转发运管 则进行以下操作
                if (model.IsTransmit)
                {
                    // 车辆关联的运管
                    string[] transportManagementArray = model.TransportManagementList.TrimEnd(',').Split(',');
                    sql += @"INSERT INTO [dbo].[VehicleTransportManagement]([VehicleID],[TransportManagementID])VALUES";
                    int tmLength = transportManagementArray.Length;
                    for (int i = 0; i < tmLength; i++)
                    {
                        if (i == tmLength - 1)
                        {
                            sql += string.Format("(@IDENTITYID,{0});", transportManagementArray[i]);
                        }
                        else
                        {
                            sql += string.Format("(@IDENTITYID,{0}),", transportManagementArray[i]);
                        }
                    }
                }
            }

            #endregion

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, parasList.ToArray());

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 编辑车辆信息
        /// <summary>
        /// 编辑车辆
        /// 1.更新资料
        /// 2.根据车辆ID删除终端关联信息
        /// 3.重新插入终端关联信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult EditVehicleNew(EditVehicleModel model)
        {

            //1.更新车辆资料表
            //2.判断终端有没有更换  没有更换只更新车辆资料表 
            //3.1终端更换 / 原终端LinkedVehicleID置为空，修改状态0（查询原先绑定的终端信息用LinkedVehicleID） / 新终端绑定
            //3.2更新终端使用表，原终端添加结束时间，添加新终端和车辆关联记录  
            //4.车辆关联员工表记录  和车辆所属经营范围插入 

            #region 参数
            List<SqlParameter> parasList = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar),
                new SqlParameter("@VehicleName",SqlDbType.NVarChar),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@Ownership",SqlDbType.Int),
                new SqlParameter("@PlateColorCode",SqlDbType.TinyInt),
                new SqlParameter("@VehicleTypeCode",SqlDbType.TinyInt),
                new SqlParameter("@IsReceived",SqlDbType.Bit),
                new SqlParameter("@IsAccess",SqlDbType.Bit),
                new SqlParameter("@IsTransmit",SqlDbType.Bit),
                new SqlParameter("@IsDangerous",SqlDbType.Bit),
                new SqlParameter("@Icon",SqlDbType.NVarChar),
                new SqlParameter("@WarrantyDate",SqlDbType.Date),
                new SqlParameter("@UpdateUserID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@VIN",SqlDbType.Char),
                new SqlParameter("@PrimaryTerminalID",SqlDbType.BigInt),
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar),
                new SqlParameter("@DriverID",SqlDbType.Int),
                new SqlParameter("@CarrierID",SqlDbType.Int),
                new SqlParameter("@BusinessScopeList",SqlDbType.VarChar),
                new SqlParameter("@OldTerminalID",SqlDbType.BigInt), 
                new SqlParameter("@ID",SqlDbType.BigInt),
                new SqlParameter("@CarToLand",SqlDbType.Char),
                new SqlParameter("@RoadransportNo",SqlDbType.VarChar),
                new SqlParameter("@TransportIndustryCode",SqlDbType.Char),
                new SqlParameter("@SoftwareDate",SqlDbType.Date),
                new SqlParameter("@TransportManagementList",SqlDbType.VarChar),
            
            };
            parasList[0].Value = model.PlateNum;
            parasList[1].Value = model.VehicleName;
            parasList[2].Value = model.EditStrucID;
            parasList[3].Value = model.Ownership;
            parasList[4].Value = model.PlateColorCode;
            parasList[5].Value = model.VehicleTypeCode;
            parasList[6].Value = model.IsReceived;
            parasList[7].Value = model.IsAccess;
            parasList[8].Value = model.IsTransmit;
            parasList[9].Value = model.IsDangerous;
            parasList[10].Value = model.Icon;
            parasList[11].Value = model.WarrantyDate;
            parasList[12].Value = model.UpdateUserID;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                parasList[13].Value = DBNull.Value;
            }
            else
            {
                parasList[13].Value = model.Remark;
            }
            parasList[14].Value = model.VIN;
            parasList[15].Value = model.PrimaryTerminalID;
            parasList[16].Value = model.TerminalCode;
            if (model.IsDangerous)
            {
                parasList[17].Value = model.DriverID;
                parasList[18].Value = model.CarrierID;
                parasList[19].Value = model.BusinessScopeList;
                parasList[22].Value = model.CarToLand;
                parasList[23].Value = model.RoadransportNo;
                parasList[24].Value = model.TransportIndustryCode;
                if (model.IsTransmit)
                {
                    parasList[26].Value = model.TransportManagementList;
                }
                else
                {
                    parasList[26].Value = DBNull.Value;
                }
            }
            else
            {
                parasList[17].Value = DBNull.Value;
                parasList[18].Value = DBNull.Value;
                parasList[19].Value = DBNull.Value;
                parasList[22].Value = DBNull.Value;
                parasList[23].Value = DBNull.Value;
                parasList[24].Value = DBNull.Value;
                parasList[26].Value = DBNull.Value;
            }
            parasList[20].Value = model.OldTerminalID;
            parasList[21].Value = model.ID;
            parasList[25].Value = model.SoftwareDate;
            #endregion

            string sql = string.Empty;

            #region 更新车辆资料
            sql += @"UPDATE  dbo.Vehicles SET  PlateNum = @PlateNum ,VehicleName = @VehicleName ,CarToLand = @CarToLand,
                         StrucID = @StrucID ,Ownership = @Ownership ,PlateColorCode = @PlateColorCode ,VehicleTypeCode = @VehicleTypeCode ,
                         IsReceived = @IsReceived,IsAccess = @IsAccess ,IsTransmit = @IsTransmit ,IsDangerous = @IsDangerous ,
                        RoadransportNo = @RoadransportNo,SoftwareDate = @SoftwareDate,WarrantyDate=@WarrantyDate,
                         Icon = @Icon ,Remark = @Remark,UpdateUserID=@UpdateUserID,EditTime = GETDATE(),
                         TransportIndustryCode = @TransportIndustryCode,
                         VIN = @VIN WHERE   ID = @ID;";
            #endregion

            #region 如果更新了终端 就更新终端信息
            if (model.OldTerminalID != model.PrimaryTerminalID)
            {
                #region  修改终端表原终端信息状态  更新终端表新绑定终端
                sql += @"UPDATE  dbo.Terminals SET  LinkedVehicleID = null , Status=0  WHERE   ID = @OldTerminalID;
                                 UPDATE  dbo.Terminals SET  LinkedVehicleID = @ID, Status=7 WHERE   ID = @PrimaryTerminalID;";
                #endregion

                #region  更新终端使用表原纪录结束时间
                sql += @" UPDATE  dbo.TerminalUsageLogs SET EndDateTime = GETDATE()  
                                  WHERE  EndDateTime IS NULL AND VehicleID = @ID AND  TerminalID =@OldTerminalID;";
                #endregion

                #region  终端使用表添加记录
                sql += @"INSERT INTO dbo.TerminalUsageLogs(TerminalCode ,VehicleID , CreateDateTime ,CreateUserID,TerminalID,PlateNum,VIN)
                             VALUES  ( @TerminalCode , @ID, GETDATE() , @UpdateUserID,@PrimaryTerminalID,@PlateNum,@VIN);";
                #endregion
            }
            #endregion

            #region 不管车辆是不是危险品车 修改的时候先删除原来的数据  车辆关联员工表记录 车辆所属经营范围 车辆所属车管所
            sql += "DELETE FROM VehicleEmployeeInfo WHERE VehicleID = @ID;";
            sql += "DELETE FROM VehicleBussinessScope WHERE VehicleID = @ID;";
            sql += "DELETE FROM VehicleTransportManagement WHERE VehicleID = @ID;";
            #endregion

            #region 车辆关联驾驶员表记录  和车辆所属经营范围插入
            if (model.IsDangerous)
            {
                // 车辆关联驾驶员表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位

                //sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                //                  (@ID,@DriverID,1),(@ID,@CarrierID,2);";
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@ID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID),
                                1),
                              (@ID,  
                                (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @CarrierID),
                               2);";

                // 车辆所属经营范围
                string[] bussinessScopeArray = model.BusinessScopeList.TrimEnd(',').Split(',');
                sql += @"INSERT INTO  [VehicleBussinessScope]([VehicleID],[BusinessScopeCode])VALUES";
                int length = bussinessScopeArray.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i == length - 1)
                    {
                        sql += string.Format("(@ID,'{0}');", bussinessScopeArray[i]);
                    }
                    else
                    {
                        sql += string.Format("(@ID,'{0}'),", bussinessScopeArray[i]);
                    }
                }
                // 是危险品车 而且选择了转发运管 则进行以下操作
                if (model.IsTransmit)
                {
                    // 车辆关联的运管
                    string[] transportManagementArray = model.TransportManagementList.TrimEnd(',').Split(',');
                    sql += @"INSERT INTO [dbo].[VehicleTransportManagement]([VehicleID],[TransportManagementID])VALUES";
                    int tmLength = transportManagementArray.Length;
                    for (int i = 0; i < tmLength; i++)
                    {
                        if (i == tmLength - 1)
                        {
                            sql += string.Format("(@ID,{0});", transportManagementArray[i]);
                        }
                        else
                        {
                            sql += string.Format("(@ID,{0}),", transportManagementArray[i]);
                        }
                    }
                }
            }

            #endregion

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, parasList.ToArray());
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 唯一性验证
        /// <summary>
        /// 检查车牌号是否重复
        /// </summary>
        public static bool CheckNewEditPlateNumExists(string plateNum, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@plateNum",SqlDbType.NVarChar,8),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = plateNum.Trim();
            paras[1].Value = id;

            string sql = string.Empty;
            if (id > 0)
            {
                sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE PlateNum=@plateNum AND ID <> @ID";
            }
            else
            {
                sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE PlateNum=@plateNum";
            }

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }


        /// <summary>
        /// 检查车架号是否重复
        /// </summary>
        public static bool CheckNewEditVINExists(string VIN, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
              new SqlParameter("@VIN",SqlDbType.Char,17),
              new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = VIN.Trim();
            paras[1].Value = id;

            string sql = string.Empty;
            if (id > 0)
            {
                sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VIN=@VIN AND ID <> @ID";
            }
            else
            {
                sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VIN=@VIN";
            }
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查同一个使用单位下车代号是否重复
        /// </summary>
        public static bool CheckNewEditVehicleNameExists(string vehicleName, int strucID, long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleName",SqlDbType.NVarChar,20),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = vehicleName.Trim();
            paras[1].Value = strucID;
            paras[2].Value = id;

            string sql = string.Empty;
            if (id > 0)
            {
                sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VehicleName=@VehicleName AND StrucID=@StrucID AND ID <> @ID";
            }
            else
            {
                sql = "SELECT COUNT(0) FROM dbo.Vehicles WHERE VehicleName=@VehicleName AND StrucID=@StrucID";
            }
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion

        #region 根据车辆id获取车辆信息
        /// <summary>
        /// 根据车辆id获取车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SelectResult<EditVehicleModel> GetVehicleInfoByID(long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = id;
            string sql = @"SELECT    v.ID , v.[CarToLand],v.[RoadransportNo],v.[TransportIndustryCode],v.SoftwareDate,
            v.PlateNum ,
            v.VehicleName ,
            v.PlateColorCode ,
            v.VehicleTypeCode ,
            v.StrucID AS EditStrucID ,
            v.StrucID ,
            Ostr.StrucName AS UserStrucName, 
            v.Ownership ,
            UStr.StrucName AS OwnerStrucName,
            CAST(v.WarrantyDate AS CHAR(10)) AS WarrantyDate ,
            v.Icon ,
            v.IsAccess ,
            v.IsDangerous ,
            v.IsReceived ,
            v.IsTransmit,
           v.Remark,
           tt.TerminalCode,
            tt.ID AS PrimaryTerminalID,
            tt.ID AS OldTerminalID,
            v.VIN  
  FROM      dbo.Vehicles v
  INNER JOIN dbo.Terminals tt ON v.ID=tt.LinkedVehicleID 
  LEFT JOIN Structures AS OStr ON v.StrucID = OStr.ID 
  LEFT JOIN Structures AS UStr ON v.Ownership = UStr.ID
  WHERE v.[Status]<>9 AND v.ID=@ID";
            List<EditVehicleModel> list = ConvertToList<EditVehicleModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditVehicleModel data = null;
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
            return new SelectResult<EditVehicleModel>()
            {
                DataResult = data,
                Message = msg
            };
        }
        #endregion

        #region 根据车辆id获取车辆的驾驶押运员信息
        /// <summary>
        /// 根据车辆id获取车辆的驾驶押运员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<VehicleEmployeeInfoDDLModel> GetVehicleEmployeeInfoByID(long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = id;
            string sql = "SELECT   [EmployeeInfoID],[Type] FROM [VehicleEmployeeInfo] WHERE [VehicleID] =@ID";
            return ConvertToList<VehicleEmployeeInfoDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion

        #region 根据单位id车辆id获取经营范围
        /// <summary>
        /// 根据单位id车辆id获取经营范围
        /// </summary>
        /// <param name="strucID"></param>
        /// <param name="vehicleID"></param>
        /// <returns></returns>
        public static List<StrucVehicleBussinessScopeDDLModel> GetBussinessScopeByVehicleIDAndStrucID(int strucID, long vehicleID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@StrucID",SqlDbType.Int),
                  new SqlParameter("@VehicleID",SqlDbType.BigInt),
            };
            paras[0].Value = strucID;
            paras[1].Value = vehicleID;
            string sql = @"SELECT a.BusinessScopeCode,c.NAME,ISNULL(temp.IsHas, 0) AS IsHas
                                      FROM   StructureBussinessScope AS a
                                    LEFT JOIN ( SELECT  BusinessScopeCode,1 AS IsHas
                                                FROM    VehicleBussinessScope
                                                WHERE   VehicleID = @VehicleID
                                              ) AS temp ON a.BusinessScopeCode = temp.BusinessScopeCode
                                  INNER JOIN BusinessScope AS c ON a.BusinessScopeCode = c.Code
                                    WHERE  a.StrucID = @StrucID;";
            return ConvertToList<StrucVehicleBussinessScopeDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion

        #region 根据车辆id获取运管所信息
        /// <summary>
        /// 根据车辆id获取运管所信息
        /// </summary>
        /// <param name="vehicleID"></param>
        /// <returns></returns>
        public static List<TransportManagementDDLModel> GetTransportManagementByVehicleID(long vehicleID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@VehicleID",SqlDbType.BigInt),
            };
            paras[0].Value = vehicleID;
            string sql = @"SELECT a.ID,a.NAME,ISNULL(temp.IsHas, 0) AS IsHas
                                      FROM   TransportManagement AS a
                                    LEFT JOIN ( SELECT  TransportManagementID,1 AS IsHas
                                                FROM    VehicleTransportManagement
                                                WHERE   VehicleID = @VehicleID
                                              ) AS temp ON a.ID = temp.TransportManagementID
                               ";
            return ConvertToList<TransportManagementDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion

        #endregion

        #region 新增编辑车辆信息新版 非危险品车也可以选择驾驶员 2017-10-16
        #region 新增车辆信息
        /// <summary>
        /// 新增车辆信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult AddVehicle_New(EditVehicleModel model)
        {
            //1.新增车辆信息到车辆表
            //2.定位终端与车辆关联信息，完善终端表LinkedVehicleID信息  /同时修改终端表终端状态 
            //3.终端使用表 添加终端使用记录
            //4.车辆关联员工表记录  和车辆所属经营范围插入 关联运管所插入

            #region 插入数据到车辆表
            List<SqlParameter> parasList = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar),
                new SqlParameter("@VehicleName",SqlDbType.NVarChar),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@Ownership",SqlDbType.Int),
                new SqlParameter("@PlateColorCode",SqlDbType.TinyInt),
                new SqlParameter("@VehicleTypeCode",SqlDbType.TinyInt),
                new SqlParameter("@IsReceived",SqlDbType.Bit),
                new SqlParameter("@IsAccess",SqlDbType.Bit),
                new SqlParameter("@IsTransmit",SqlDbType.Bit),
                new SqlParameter("@IsDangerous",SqlDbType.Bit),
                new SqlParameter("@Icon",SqlDbType.NVarChar),
                new SqlParameter("@WarrantyDate",SqlDbType.Date),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@VIN",SqlDbType.Char),
                new SqlParameter("@PrimaryTerminalID",SqlDbType.BigInt),
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar),
                new SqlParameter("@DriverID",SqlDbType.Int),
                new SqlParameter("@CarrierID",SqlDbType.Int),
                new SqlParameter("@BusinessScopeList",SqlDbType.VarChar),
                new SqlParameter("@CarToLand",SqlDbType.Char),
                new SqlParameter("@RoadransportNo",SqlDbType.VarChar),
                new SqlParameter("@TransportIndustryCode",SqlDbType.Char),
                new SqlParameter("@SoftwareDate",SqlDbType.Date),
                new SqlParameter("@TransportManagementList",SqlDbType.VarChar),
            };
            parasList[0].Value = model.PlateNum;
            parasList[1].Value = model.VehicleName;
            parasList[2].Value = model.EditStrucID;
            parasList[3].Value = model.Ownership;
            parasList[4].Value = model.PlateColorCode;
            parasList[5].Value = model.VehicleTypeCode;
            parasList[6].Value = model.IsReceived;
            parasList[7].Value = model.IsAccess;
            parasList[8].Value = model.IsTransmit;
            parasList[9].Value = model.IsDangerous;
            parasList[10].Value = model.Icon;
            parasList[11].Value = model.WarrantyDate;
            parasList[12].Value = model.CreateUserID;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                parasList[13].Value = DBNull.Value;
            }
            else
            {
                parasList[13].Value = model.Remark;
            }
            parasList[14].Value = model.VIN;
            parasList[15].Value = model.PrimaryTerminalID;
            parasList[16].Value = model.TerminalCode;
            if (model.DriverID.HasValue)
            {
                parasList[17].Value = model.DriverID;
            }
            else
            {
                parasList[17].Value = DBNull.Value;
            }
            if (model.IsDangerous)
            {
                parasList[18].Value = model.CarrierID;
                parasList[19].Value = model.BusinessScopeList;
                parasList[20].Value = model.CarToLand;
                parasList[21].Value = model.RoadransportNo;
                parasList[22].Value = model.TransportIndustryCode;
                if (model.IsTransmit)
                {
                    parasList[24].Value = model.TransportManagementList;
                }
                else
                {
                    parasList[24].Value = DBNull.Value;
                }
            }
            else
            {
                parasList[18].Value = DBNull.Value;
                parasList[19].Value = DBNull.Value;
                parasList[20].Value = DBNull.Value;
                parasList[21].Value = DBNull.Value;
                parasList[22].Value = DBNull.Value;
                parasList[24].Value = DBNull.Value;
            }
            parasList[23].Value = model.SoftwareDate;

            string sql = "DECLARE @IDENTITYID INT;";
            sql += @"INSERT INTO dbo.Vehicles( PlateNum ,VehicleName,StrucID,Ownership,PlateColorCode,VehicleTypeCode,IsReceived ,
                              IsAccess ,IsTransmit ,IsDangerous ,Icon ,WarrantyDate ,CreateTime ,Remark ,Status ,CreateUserID ,
                              VIN,CarToLand,RoadransportNo,TransportIndustryCode,SoftwareDate)
                                VALUES  ( @PlateNum , @VehicleName, @StrucID, @Ownership ,@PlateColorCode , @VehicleTypeCode ,@IsReceived , 
                               @IsAccess , @IsTransmit , @IsDangerous , @Icon, @WarrantyDate, GETDATE() ,@Remark , 0 , @CreateUserID,
                                @VIN,@CarToLand,@RoadransportNo,@TransportIndustryCode,@SoftwareDate);
                               SELECT  @IDENTITYID=SCOPE_IDENTITY();";
            #endregion

            #region  完善终端表LinkedVehicleID信息
            sql += @"UPDATE  dbo.Terminals SET  LinkedVehicleID = @IDENTITYID, Status=7 WHERE   ID = @PrimaryTerminalID;";
            #endregion

            #region  终端使用表添加记录
            sql += @"INSERT INTO dbo.TerminalUsageLogs(TerminalCode ,VehicleID , CreateDateTime ,CreateUserID,TerminalID,PlateNum,VIN)
                             VALUES  ( @TerminalCode , @IDENTITYID, GETDATE() , @CreateUserID,@PrimaryTerminalID,@PlateNum,@VIN);";
            #endregion

            #region 车辆关联驾驶员表记录  和车辆所属经营范围插入
            if (model.IsDangerous)
            {

                // 车辆关联员工表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@IDENTITYID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND  IsDriver = 1),
                                1),
                              (@IDENTITYID,  
                                (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @CarrierID AND IsCarrier = 1),
                               2);";

                // 车辆所属经营范围
                string[] bussinessScopeArray = model.BusinessScopeList.TrimEnd(',').Split(',');
                sql += @"INSERT INTO  [VehicleBussinessScope]([VehicleID],[BusinessScopeCode])VALUES";
                int length = bussinessScopeArray.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i == length - 1)
                    {
                        sql += string.Format("(@IDENTITYID,'{0}');", bussinessScopeArray[i]);
                    }
                    else
                    {
                        sql += string.Format("(@IDENTITYID,'{0}'),", bussinessScopeArray[i]);
                    }
                }

                // 是危险品车 而且选择了转发运管 则进行以下操作
                if (model.IsTransmit)
                {
                    // 车辆关联的运管
                    string[] transportManagementArray = model.TransportManagementList.TrimEnd(',').Split(',');
                    sql += @"INSERT INTO [dbo].[VehicleTransportManagement]([VehicleID],[TransportManagementID])VALUES";
                    int tmLength = transportManagementArray.Length;
                    for (int i = 0; i < tmLength; i++)
                    {
                        if (i == tmLength - 1)
                        {
                            sql += string.Format("(@IDENTITYID,{0});", transportManagementArray[i]);
                        }
                        else
                        {
                            sql += string.Format("(@IDENTITYID,{0}),", transportManagementArray[i]);
                        }
                    }
                }
            }

            #endregion

            #region 不是危险品车但是选择了驾驶员信息
            if (!model.IsDangerous && model.DriverID.HasValue)
            {
                // 车辆关联员工表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@IDENTITYID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND  IsDriver = 1),
                                1);";
            }
            #endregion

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, parasList.ToArray());

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 编辑车辆信息
        /// <summary>
        /// 编辑车辆
        /// 1.更新资料
        /// 2.根据车辆ID删除终端关联信息
        /// 3.重新插入终端关联信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult EditVehicle_New(EditVehicleModel model)
        {

            //1.更新车辆资料表
            //2.判断终端有没有更换  没有更换只更新车辆资料表 
            //3.1终端更换 / 原终端LinkedVehicleID置为空，修改状态0（查询原先绑定的终端信息用LinkedVehicleID） / 新终端绑定
            //3.2更新终端使用表，原终端添加结束时间，添加新终端和车辆关联记录  
            //4.车辆关联员工表记录  和车辆所属经营范围插入 

            #region 参数
            List<SqlParameter> parasList = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar),
                new SqlParameter("@VehicleName",SqlDbType.NVarChar),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@Ownership",SqlDbType.Int),
                new SqlParameter("@PlateColorCode",SqlDbType.TinyInt),
                new SqlParameter("@VehicleTypeCode",SqlDbType.TinyInt),
                new SqlParameter("@IsReceived",SqlDbType.Bit),
                new SqlParameter("@IsAccess",SqlDbType.Bit),
                new SqlParameter("@IsTransmit",SqlDbType.Bit),
                new SqlParameter("@IsDangerous",SqlDbType.Bit),
                new SqlParameter("@Icon",SqlDbType.NVarChar),
                new SqlParameter("@WarrantyDate",SqlDbType.Date),
                new SqlParameter("@UpdateUserID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@VIN",SqlDbType.Char),
                new SqlParameter("@PrimaryTerminalID",SqlDbType.BigInt),
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar),
                new SqlParameter("@DriverID",SqlDbType.Int),
                new SqlParameter("@CarrierID",SqlDbType.Int),
                new SqlParameter("@BusinessScopeList",SqlDbType.VarChar),
                new SqlParameter("@OldTerminalID",SqlDbType.BigInt), 
                new SqlParameter("@ID",SqlDbType.BigInt),
                new SqlParameter("@CarToLand",SqlDbType.Char),
                new SqlParameter("@RoadransportNo",SqlDbType.VarChar),
                new SqlParameter("@TransportIndustryCode",SqlDbType.Char),
                new SqlParameter("@SoftwareDate",SqlDbType.Date),
                new SqlParameter("@TransportManagementList",SqlDbType.VarChar),
            
            };
            parasList[0].Value = model.PlateNum;
            parasList[1].Value = model.VehicleName;
            parasList[2].Value = model.EditStrucID;
            parasList[3].Value = model.Ownership;
            parasList[4].Value = model.PlateColorCode;
            parasList[5].Value = model.VehicleTypeCode;
            parasList[6].Value = model.IsReceived;
            parasList[7].Value = model.IsAccess;
            parasList[8].Value = model.IsTransmit;
            parasList[9].Value = model.IsDangerous;
            parasList[10].Value = model.Icon;
            parasList[11].Value = model.WarrantyDate;
            parasList[12].Value = model.UpdateUserID;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                parasList[13].Value = DBNull.Value;
            }
            else
            {
                parasList[13].Value = model.Remark;
            }
            parasList[14].Value = model.VIN;
            parasList[15].Value = model.PrimaryTerminalID;
            parasList[16].Value = model.TerminalCode;
            if (model.DriverID.HasValue)
            {
                parasList[17].Value = model.DriverID;
            }
            else
            {
                parasList[17].Value = DBNull.Value;
            }
            if (model.IsDangerous)
            {
                parasList[18].Value = model.CarrierID;
                parasList[19].Value = model.BusinessScopeList;
                parasList[22].Value = model.CarToLand;
                parasList[23].Value = model.RoadransportNo;
                parasList[24].Value = model.TransportIndustryCode;
                if (model.IsTransmit)
                {
                    parasList[26].Value = model.TransportManagementList;
                }
                else
                {
                    parasList[26].Value = DBNull.Value;
                }
            }
            else
            {
                parasList[18].Value = DBNull.Value;
                parasList[19].Value = DBNull.Value;
                parasList[22].Value = DBNull.Value;
                parasList[23].Value = DBNull.Value;
                parasList[24].Value = DBNull.Value;
                parasList[26].Value = DBNull.Value;
            }
            parasList[20].Value = model.OldTerminalID;
            parasList[21].Value = model.ID;
            parasList[25].Value = model.SoftwareDate;
            #endregion

            string sql = string.Empty;

            #region 更新车辆资料
            sql += @"UPDATE  dbo.Vehicles SET  PlateNum = @PlateNum ,VehicleName = @VehicleName ,CarToLand = @CarToLand,
                         StrucID = @StrucID ,Ownership = @Ownership ,PlateColorCode = @PlateColorCode ,VehicleTypeCode = @VehicleTypeCode ,
                         IsReceived = @IsReceived,IsAccess = @IsAccess ,IsTransmit = @IsTransmit ,IsDangerous = @IsDangerous ,
                        RoadransportNo = @RoadransportNo,SoftwareDate = @SoftwareDate,WarrantyDate=@WarrantyDate,
                         Icon = @Icon ,Remark = @Remark,UpdateUserID=@UpdateUserID,EditTime = GETDATE(),
                         TransportIndustryCode = @TransportIndustryCode,
                         VIN = @VIN WHERE   ID = @ID;";
            #endregion

            #region 如果更新了终端 就更新终端信息
            if (model.OldTerminalID != model.PrimaryTerminalID)
            {
                #region  修改终端表原终端信息状态  更新终端表新绑定终端
                sql += @"UPDATE  dbo.Terminals SET  LinkedVehicleID = null , Status=0  WHERE   ID = @OldTerminalID;
                                 UPDATE  dbo.Terminals SET  LinkedVehicleID = @ID, Status=7 WHERE   ID = @PrimaryTerminalID;";
                #endregion

                #region  更新终端使用表原纪录结束时间
                //                sql += @" UPDATE  dbo.TerminalUsageLogs SET EndDateTime = GETDATE()  
                //                                  WHERE  EndDateTime IS NULL AND VehicleID = @ID AND  TerminalID =@OldTerminalID;";
                sql += @" UPDATE  dbo.TerminalUsageLogs SET EndDateTime = GETDATE()  
                                                  WHERE  EndDateTime IS NULL AND VehicleID = @ID AND  TerminalID =@OldTerminalID;";
                #endregion

                #region  终端使用表添加记录
                sql += @"INSERT INTO dbo.TerminalUsageLogs(TerminalCode ,VehicleID , CreateDateTime ,CreateUserID,TerminalID,PlateNum,VIN)
                             VALUES  ( @TerminalCode , @ID, GETDATE() , @UpdateUserID,@PrimaryTerminalID,@PlateNum,@VIN);";
                #endregion
            }
            #endregion

            #region 不管车辆是不是危险品车 修改的时候先删除原来的数据  车辆关联员工表记录 车辆所属经营范围 车辆所属车管所
            sql += "DELETE FROM VehicleEmployeeInfo WHERE VehicleID = @ID;";
            sql += "DELETE FROM VehicleBussinessScope WHERE VehicleID = @ID;";
            sql += "DELETE FROM VehicleTransportManagement WHERE VehicleID = @ID;";
            #endregion

            #region 车辆关联驾驶员表记录  和车辆所属经营范围插入
            if (model.IsDangerous)
            {
                // 车辆关联驾驶员表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位

                //sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                //                  (@ID,@DriverID,1),(@ID,@CarrierID,2);";
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@ID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND IsDriver = 1),
                                1),
                              (@ID,  
                                (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @CarrierID AND IsCarrier = 1),
                               2);";

                // 车辆所属经营范围
                string[] bussinessScopeArray = model.BusinessScopeList.TrimEnd(',').Split(',');
                sql += @"INSERT INTO  [VehicleBussinessScope]([VehicleID],[BusinessScopeCode])VALUES";
                int length = bussinessScopeArray.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i == length - 1)
                    {
                        sql += string.Format("(@ID,'{0}');", bussinessScopeArray[i]);
                    }
                    else
                    {
                        sql += string.Format("(@ID,'{0}'),", bussinessScopeArray[i]);
                    }
                }
                // 是危险品车 而且选择了转发运管 则进行以下操作
                if (model.IsTransmit)
                {
                    // 车辆关联的运管
                    string[] transportManagementArray = model.TransportManagementList.TrimEnd(',').Split(',');
                    sql += @"INSERT INTO [dbo].[VehicleTransportManagement]([VehicleID],[TransportManagementID])VALUES";
                    int tmLength = transportManagementArray.Length;
                    for (int i = 0; i < tmLength; i++)
                    {
                        if (i == tmLength - 1)
                        {
                            sql += string.Format("(@ID,{0});", transportManagementArray[i]);
                        }
                        else
                        {
                            sql += string.Format("(@ID,{0}),", transportManagementArray[i]);
                        }
                    }
                }
            }

            #endregion

            #region 不是危险品车但是选择了驾驶员信息
            if (!model.IsDangerous && model.DriverID.HasValue)
            {
                // 车辆关联员工表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@ID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND  IsDriver = 1),
                                1);";
            }
            #endregion

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, parasList.ToArray());
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion
        #endregion

        #region 新增编辑车辆信息新版 非危险品车也可以选择驾驶员 2017-12-06
        #region 新增车辆信息
        /// <summary>
        /// 新增车辆信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult NewAddVehicle(NewEditVehicleModel model)
        {
            //1.新增车辆信息到车辆表
            //2.定位终端与车辆关联信息，完善终端表LinkedVehicleID信息  /同时修改终端表终端状态 
            //3.终端使用表 添加终端使用记录
            //4.车辆关联员工表记录  和车辆所属经营范围插入 关联运管所插入

            #region 插入数据到车辆表
            List<SqlParameter> parasList = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar),
                new SqlParameter("@VehicleName",SqlDbType.NVarChar),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@Ownership",SqlDbType.Int),
                new SqlParameter("@PlateColorCode",SqlDbType.TinyInt),
                new SqlParameter("@VehicleTypeCode",SqlDbType.TinyInt),
                new SqlParameter("@IsReceived",SqlDbType.Bit),
                new SqlParameter("@IsAccess",SqlDbType.Bit),
                new SqlParameter("@IsTransmit",SqlDbType.Bit),
                new SqlParameter("@IsDangerous",SqlDbType.Bit),
                new SqlParameter("@Icon",SqlDbType.NVarChar),
                new SqlParameter("@WarrantyDate",SqlDbType.Date),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@VIN",SqlDbType.Char),
                new SqlParameter("@PrimaryTerminalID",SqlDbType.BigInt),
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar),
                new SqlParameter("@DriverID",SqlDbType.Int),
                new SqlParameter("@CarrierID",SqlDbType.Int),
                new SqlParameter("@BusinessScopeList",SqlDbType.VarChar),
                new SqlParameter("@CarToLand",SqlDbType.Char),
                new SqlParameter("@RoadransportNo",SqlDbType.VarChar),
                new SqlParameter("@TransportIndustryCode",SqlDbType.Char),
                new SqlParameter("@SoftwareDate",SqlDbType.Date),
                new SqlParameter("@TransportManagementList",SqlDbType.VarChar),
                 new SqlParameter("@AnnualInspectionTime",SqlDbType.Date),
                  new SqlParameter("@RemindTimeSpan",SqlDbType.Int),
                  new SqlParameter("@AnnualInspectionTime1",SqlDbType.Date),
                  new SqlParameter("@RemindTimeSpan1",SqlDbType.Int),
            };
            parasList[0].Value = model.PlateNum.Trim();
            parasList[1].Value = model.VehicleName.Trim();
            parasList[2].Value = model.EditStrucID;
            parasList[3].Value = model.Ownership;
            parasList[4].Value = model.PlateColorCode;
            parasList[5].Value = model.VehicleTypeCode;
            parasList[6].Value = model.IsReceived;
            parasList[7].Value = model.IsAccess;
            parasList[8].Value = model.IsTransmit;
            parasList[9].Value = model.IsDangerous;
            parasList[10].Value = model.Icon.Trim();
            parasList[11].Value = model.WarrantyDate;
            parasList[12].Value = model.CreateUserID;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                parasList[13].Value = DBNull.Value;
            }
            else
            {
                parasList[13].Value = model.Remark;
            }
            parasList[14].Value = model.VIN.Trim();
            parasList[15].Value = model.PrimaryTerminalID;
            parasList[16].Value = model.TerminalCode.Trim();
            if (model.DriverID.HasValue)
            {
                parasList[17].Value = model.DriverID;
            }
            else
            {
                parasList[17].Value = DBNull.Value;
            }
            if (model.IsDangerous)
            {
                parasList[18].Value = model.CarrierID;
                parasList[19].Value = model.BusinessScopeList.Trim();
                parasList[20].Value = model.CarToLand.Trim();
                parasList[21].Value = model.RoadransportNo.Trim();
                parasList[22].Value = model.TransportIndustryCode.Trim();
                if (model.IsTransmit)
                {
                    parasList[24].Value = model.TransportManagementList.Trim();
                }
                else
                {
                    parasList[24].Value = DBNull.Value;
                }
            }
            else
            {
                parasList[18].Value = DBNull.Value;
                parasList[19].Value = DBNull.Value;
                parasList[20].Value = DBNull.Value;
                parasList[21].Value = DBNull.Value;
                parasList[22].Value = DBNull.Value;
                parasList[24].Value = DBNull.Value;
            }
            parasList[23].Value = model.SoftwareDate;
            parasList[25].Value = string.IsNullOrWhiteSpace(model.AnnualInspectionTime) ? DBNull.Value : (object)model.AnnualInspectionTime;
            parasList[26].Value = string.IsNullOrWhiteSpace(model.RemindTimeSpan) ? DBNull.Value : (object)model.RemindTimeSpan;
            parasList[27].Value = string.IsNullOrWhiteSpace(model.AnnualInspectionTime1) ? DBNull.Value : (object)model.AnnualInspectionTime1;
            parasList[28].Value = string.IsNullOrWhiteSpace(model.RemindTimeSpan1) ? DBNull.Value : (object)model.RemindTimeSpan1;
            string sql = "DECLARE @IDENTITYID INT;";
            sql += @"INSERT INTO dbo.Vehicles( PlateNum ,VehicleName,StrucID,Ownership,PlateColorCode,VehicleTypeCode,IsReceived ,
                              IsAccess ,IsTransmit ,IsDangerous ,Icon ,WarrantyDate ,CreateTime ,Remark ,Status ,CreateUserID ,
                              VIN,CarToLand,RoadransportNo,TransportIndustryCode,SoftwareDate,AnnualInspectionTime,RemindTimeSpan,AnnualInspectionTime1,RemindTimeSpan1)
                                VALUES  ( @PlateNum , @VehicleName, @StrucID, @Ownership ,@PlateColorCode , @VehicleTypeCode ,@IsReceived , 
                               @IsAccess , @IsTransmit , @IsDangerous , @Icon, @WarrantyDate, GETDATE() ,@Remark , 0 , @CreateUserID,
                                @VIN,@CarToLand,@RoadransportNo,@TransportIndustryCode,@SoftwareDate,@AnnualInspectionTime,@RemindTimeSpan,@AnnualInspectionTime1,@RemindTimeSpan1);
                               SELECT  @IDENTITYID=SCOPE_IDENTITY();";
            #endregion

            #region  完善终端表LinkedVehicleID信息
            sql += @"UPDATE  dbo.Terminals SET  LinkedVehicleID = @IDENTITYID, Status=7 WHERE   ID = @PrimaryTerminalID;";
            #endregion

            #region  终端使用表添加记录
            sql += @"INSERT INTO dbo.TerminalUsageLogs(TerminalCode ,VehicleID , CreateDateTime ,CreateUserID,TerminalID,PlateNum,VIN)
                             VALUES  ( @TerminalCode , @IDENTITYID, GETDATE() , @CreateUserID,@PrimaryTerminalID,@PlateNum,@VIN);";
            #endregion

            #region 车辆关联驾驶员表记录  和车辆所属经营范围插入
            if (model.IsDangerous)
            {

                // 车辆关联员工表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@IDENTITYID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND  IsDriver = 1),
                                1),
                              (@IDENTITYID,  
                                (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @CarrierID AND IsCarrier = 1),
                               2);";

                // 车辆所属经营范围
                string[] bussinessScopeArray = model.BusinessScopeList.TrimEnd(',').Split(',');
                sql += @"INSERT INTO  [VehicleBussinessScope]([VehicleID],[BusinessScopeCode])VALUES";
                int length = bussinessScopeArray.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i == length - 1)
                    {
                        sql += string.Format("(@IDENTITYID,'{0}');", bussinessScopeArray[i]);
                    }
                    else
                    {
                        sql += string.Format("(@IDENTITYID,'{0}'),", bussinessScopeArray[i]);
                    }
                }

                // 是危险品车 而且选择了转发运管 则进行以下操作
                if (model.IsTransmit)
                {
                    // 车辆关联的运管
                    string[] transportManagementArray = model.TransportManagementList.TrimEnd(',').Split(',');
                    sql += @"INSERT INTO [dbo].[VehicleTransportManagement]([VehicleID],[TransportManagementID])VALUES";
                    int tmLength = transportManagementArray.Length;
                    for (int i = 0; i < tmLength; i++)
                    {
                        if (i == tmLength - 1)
                        {
                            sql += string.Format("(@IDENTITYID,{0});", transportManagementArray[i]);
                        }
                        else
                        {
                            sql += string.Format("(@IDENTITYID,{0}),", transportManagementArray[i]);
                        }
                    }
                }
            }

            #endregion

            #region 不是危险品车但是选择了驾驶员信息
            if (!model.IsDangerous && model.DriverID.HasValue)
            {
                // 车辆关联员工表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@IDENTITYID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND  IsDriver = 1),
                                1);";
            }
            #endregion

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, parasList.ToArray());

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 编辑车辆信息
        /// <summary>
        /// 编辑车辆
        /// 1.更新资料
        /// 2.根据车辆ID删除终端关联信息
        /// 3.重新插入终端关联信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult NewEditVehicle(NewEditVehicleModel model)
        {

            //1.更新车辆资料表
            //2.判断终端有没有更换  没有更换只更新车辆资料表 
            //3.1终端更换 / 原终端LinkedVehicleID置为空，修改状态0（查询原先绑定的终端信息用LinkedVehicleID） / 新终端绑定
            //3.2更新终端使用表，原终端添加结束时间，添加新终端和车辆关联记录  
            //4.车辆关联员工表记录  和车辆所属经营范围插入 

            #region 参数
            List<SqlParameter> parasList = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar),
                new SqlParameter("@VehicleName",SqlDbType.NVarChar),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@Ownership",SqlDbType.Int),
                new SqlParameter("@PlateColorCode",SqlDbType.TinyInt),
                new SqlParameter("@VehicleTypeCode",SqlDbType.TinyInt),
                new SqlParameter("@IsReceived",SqlDbType.Bit),
                new SqlParameter("@IsAccess",SqlDbType.Bit),
                new SqlParameter("@IsTransmit",SqlDbType.Bit),
                new SqlParameter("@IsDangerous",SqlDbType.Bit),
                new SqlParameter("@Icon",SqlDbType.NVarChar),
                new SqlParameter("@WarrantyDate",SqlDbType.Date),
                new SqlParameter("@UpdateUserID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@VIN",SqlDbType.Char),
                new SqlParameter("@PrimaryTerminalID",SqlDbType.BigInt),
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar),
                new SqlParameter("@DriverID",SqlDbType.Int),
                new SqlParameter("@CarrierID",SqlDbType.Int),
                new SqlParameter("@BusinessScopeList",SqlDbType.VarChar),
                new SqlParameter("@OldTerminalID",SqlDbType.BigInt), 
                new SqlParameter("@ID",SqlDbType.BigInt),
                new SqlParameter("@CarToLand",SqlDbType.Char),
                new SqlParameter("@RoadransportNo",SqlDbType.VarChar),
                new SqlParameter("@TransportIndustryCode",SqlDbType.Char),
                new SqlParameter("@SoftwareDate",SqlDbType.Date),
                new SqlParameter("@TransportManagementList",SqlDbType.VarChar),
                new SqlParameter("@AnnualInspectionTime",SqlDbType.Date),
                new SqlParameter("@RemindTimeSpan",SqlDbType.Int),
            new SqlParameter("@AnnualInspectionTime1",SqlDbType.Date),
                new SqlParameter("@RemindTimeSpan1",SqlDbType.Int),
            };
            parasList[0].Value = model.PlateNum.Trim();
            parasList[1].Value = model.VehicleName.Trim();
            parasList[2].Value = model.EditStrucID;
            parasList[3].Value = model.Ownership;
            parasList[4].Value = model.PlateColorCode;
            parasList[5].Value = model.VehicleTypeCode;
            parasList[6].Value = model.IsReceived;
            parasList[7].Value = model.IsAccess;
            parasList[8].Value = model.IsTransmit;
            parasList[9].Value = model.IsDangerous;
            parasList[10].Value = model.Icon.Trim();
            parasList[11].Value = model.WarrantyDate;
            parasList[12].Value = model.UpdateUserID;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                parasList[13].Value = DBNull.Value;
            }
            else
            {
                parasList[13].Value = model.Remark;
            }
            parasList[14].Value = model.VIN.Trim();
            parasList[15].Value = model.PrimaryTerminalID;
            parasList[16].Value = model.TerminalCode.Trim();
            if (model.DriverID.HasValue)
            {
                parasList[17].Value = model.DriverID;
            }
            else
            {
                parasList[17].Value = DBNull.Value;
            }
            if (model.IsDangerous)
            {
                parasList[18].Value = model.CarrierID;
                parasList[19].Value = model.BusinessScopeList.Trim();
                parasList[22].Value = model.CarToLand.Trim();
                parasList[23].Value = model.RoadransportNo.Trim();
                parasList[24].Value = model.TransportIndustryCode.Trim();
                if (model.IsTransmit)
                {
                    parasList[26].Value = model.TransportManagementList.Trim();
                }
                else
                {
                    parasList[26].Value = DBNull.Value;
                }
            }
            else
            {
                parasList[18].Value = DBNull.Value;
                parasList[19].Value = DBNull.Value;
                parasList[22].Value = DBNull.Value;
                parasList[23].Value = DBNull.Value;
                parasList[24].Value = DBNull.Value;
                parasList[26].Value = DBNull.Value;
            }
            parasList[20].Value = model.OldTerminalID;
            parasList[21].Value = model.ID;
            parasList[25].Value = model.SoftwareDate;
            parasList[27].Value = string.IsNullOrWhiteSpace(model.AnnualInspectionTime) ? DBNull.Value : (object)model.AnnualInspectionTime;
            parasList[28].Value = string.IsNullOrWhiteSpace(model.RemindTimeSpan) ? DBNull.Value : (object)model.RemindTimeSpan;
            parasList[29].Value = string.IsNullOrWhiteSpace(model.AnnualInspectionTime1) ? DBNull.Value : (object)model.AnnualInspectionTime1;
            parasList[30].Value = string.IsNullOrWhiteSpace(model.RemindTimeSpan1) ? DBNull.Value : (object)model.RemindTimeSpan1;
            #endregion

            string sql = string.Empty;

            #region 更新车辆资料
            sql += @"UPDATE  dbo.Vehicles SET  PlateNum = @PlateNum ,VehicleName = @VehicleName ,CarToLand = @CarToLand,
                         StrucID = @StrucID ,Ownership = @Ownership ,PlateColorCode = @PlateColorCode ,VehicleTypeCode = @VehicleTypeCode ,
                         IsReceived = @IsReceived,IsAccess = @IsAccess ,IsTransmit = @IsTransmit ,IsDangerous = @IsDangerous ,
                        RoadransportNo = @RoadransportNo,SoftwareDate = @SoftwareDate,WarrantyDate=@WarrantyDate,
                         Icon = @Icon ,Remark = @Remark,UpdateUserID=@UpdateUserID,EditTime = GETDATE(),
                         TransportIndustryCode = @TransportIndustryCode,AnnualInspectionTime=@AnnualInspectionTime,RemindTimeSpan=@RemindTimeSpan,AnnualInspectionTime1=@AnnualInspectionTime1,RemindTimeSpan1=@RemindTimeSpan1,
                         VIN = @VIN WHERE   ID = @ID;";
            #endregion

            #region 如果更新了终端 就更新终端信息
            if (model.OldTerminalID != model.PrimaryTerminalID)
            {
                #region  修改终端表原终端信息状态  更新终端表新绑定终端
                sql += @"UPDATE  dbo.Terminals SET  LinkedVehicleID = null , Status=0  WHERE   ID = @OldTerminalID;
                                 UPDATE  dbo.Terminals SET  LinkedVehicleID = @ID, Status=7 WHERE   ID = @PrimaryTerminalID;";
                #endregion

                #region  更新终端使用表原纪录结束时间
                //                sql += @" UPDATE  dbo.TerminalUsageLogs SET EndDateTime = GETDATE()  
                //                                  WHERE  EndDateTime IS NULL AND VehicleID = @ID AND  TerminalID =@OldTerminalID;";

                //                sql += @" UPDATE  dbo.TerminalUsageLogs SET EndDateTime = GETDATE(),OverspeedThreshold=(select OverspeedThreshold from Terminals where ID=@OldTerminalID), 
                //MinimumDuration=(select MinimumDuration from Terminals where ID=@OldTerminalID),ContinuousDrivingThreshold=(select ContinuousDrivingThreshold from Terminals where ID=@OldTerminalID),
                //MinimumBreakTime=(select MinimumBreakTime from Terminals where ID=@OldTerminalID),DrivingTimeThreshold=(select DrivingTimeThreshold from Terminals where ID=@OldTerminalID),
                //MaximumParkingTime=(select MaximumParkingTime from Terminals where ID=@OldTerminalID),LastMiles=(select Mileage from VW_GetRealTimeSignals where TID=@OldTerminalID)
                //                                  WHERE  EndDateTime IS NULL AND VehicleID = @ID AND  TerminalID =@OldTerminalID;";
                //车辆如果还没推送更换设备只会更新结束时间而不会更新配置，避免推送错误数据给客户端
                if (model.Status != 8)
                {
                    sql += @"Update TerminalUsageLogs SET EndDateTime=getdate(),OverspeedThreshold=ter.OverspeedThreshold, 
MinimumDuration=ter.MinimumDuration,ContinuousDrivingThreshold=ter.ContinuousDrivingThreshold,
MinimumBreakTime=ter.MinimumBreakTime,DrivingTimeThreshold=ter.DrivingTimeThreshold,
MaximumParkingTime=ter.MaximumParkingTime,LastMiles=(select Mileage from VW_GetRealTimeSignals where TID=@OldTerminalID) from TerminalUsageLogs t,Terminals ter
where t.TerminalID=ter.ID and t.TerminalID=@OldTerminalID and t.EndDateTime is null ;";
                }
                else
                {
                    sql += @"Update TerminalUsageLogs SET EndDateTime=getdate()
where  TerminalID=@OldTerminalID and EndDateTime is null ;";
                };
                #endregion

                #region  终端使用表添加记录
                sql += @"INSERT INTO dbo.TerminalUsageLogs(TerminalCode ,VehicleID , CreateDateTime ,CreateUserID,TerminalID,PlateNum,VIN)
                             VALUES  ( @TerminalCode , @ID, GETDATE() , @UpdateUserID,@PrimaryTerminalID,@PlateNum,@VIN);";
                #endregion

                #region 更新车辆的状态为更换了车机但是未推送信息
                sql += @"UPDATE  dbo.Vehicles SET Status=8 where   ID = @ID;";
                #endregion
            }
            #endregion

            #region 不管车辆是不是危险品车 修改的时候先删除原来的数据  车辆关联员工表记录 车辆所属经营范围 车辆所属车管所
            sql += "DELETE FROM VehicleEmployeeInfo WHERE VehicleID = @ID;";
            sql += "DELETE FROM VehicleBussinessScope WHERE VehicleID = @ID;";
            sql += "DELETE FROM VehicleTransportManagement WHERE VehicleID = @ID;";
            #endregion

            #region 车辆关联驾驶员表记录  和车辆所属经营范围插入
            if (model.IsDangerous)
            {
                // 车辆关联驾驶员表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位

                //sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                //                  (@ID,@DriverID,1),(@ID,@CarrierID,2);";
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@ID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND IsDriver = 1),
                                1),
                              (@ID,  
                                (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @CarrierID AND IsCarrier = 1),
                               2);";

                // 车辆所属经营范围
                string[] bussinessScopeArray = model.BusinessScopeList.TrimEnd(',').Split(',');
                sql += @"INSERT INTO  [VehicleBussinessScope]([VehicleID],[BusinessScopeCode])VALUES";
                int length = bussinessScopeArray.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i == length - 1)
                    {
                        sql += string.Format("(@ID,'{0}');", bussinessScopeArray[i]);
                    }
                    else
                    {
                        sql += string.Format("(@ID,'{0}'),", bussinessScopeArray[i]);
                    }
                }
                // 是危险品车 而且选择了转发运管 则进行以下操作
                if (model.IsTransmit)
                {
                    // 车辆关联的运管
                    string[] transportManagementArray = model.TransportManagementList.TrimEnd(',').Split(',');
                    sql += @"INSERT INTO [dbo].[VehicleTransportManagement]([VehicleID],[TransportManagementID])VALUES";
                    int tmLength = transportManagementArray.Length;
                    for (int i = 0; i < tmLength; i++)
                    {
                        if (i == tmLength - 1)
                        {
                            sql += string.Format("(@ID,{0});", transportManagementArray[i]);
                        }
                        else
                        {
                            sql += string.Format("(@ID,{0}),", transportManagementArray[i]);
                        }
                    }
                }
            }

            #endregion

            #region 不是危险品车但是选择了驾驶员信息
            if (!model.IsDangerous && model.DriverID.HasValue)
            {
                // 车辆关联员工表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@ID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND  IsDriver = 1),
                                1);";
            }
            #endregion

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, parasList.ToArray());
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion
        #endregion

        #region 根据车辆id获取车辆信息 2017-12-06
        /// <summary>
        /// 根据车辆id获取车辆信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SelectResult<NewEditVehicleModel> GetVehicleInfoByID_New(long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = id;
            string sql = @"SELECT    v.ID,v.Status , v.[CarToLand],v.[RoadransportNo],v.[TransportIndustryCode],v.SoftwareDate,
            v.PlateNum ,
            v.VehicleName ,
            v.PlateColorCode ,
            v.VehicleTypeCode ,
            v.StrucID AS EditStrucID ,
            v.StrucID ,
            Ostr.StrucName AS UserStrucName, 
            v.Ownership ,
            UStr.StrucName AS OwnerStrucName,
            CAST(v.WarrantyDate AS CHAR(10)) AS WarrantyDate ,
            v.Icon ,
            v.IsAccess ,
            v.IsDangerous ,
            v.IsReceived ,
            v.IsTransmit,
           v.Remark,
           tt.TerminalCode,
            tt.ID AS PrimaryTerminalID,
            tt.ID AS OldTerminalID,
            v.VIN,v.AnnualInspectionTime
      ,v.RemindTimeSpan,v.AnnualInspectionTime1,v.RemindTimeSpan1,
            em.EmployeeName AS Driver,
            em.ID AS DriverID,
            em1.EmployeeName AS Carrier,
            em1.ID AS CarrierID
  FROM      dbo.Vehicles v
  LEFT JOIN dbo.Terminals tt ON v.ID=tt.LinkedVehicleID 
  LEFT JOIN Structures AS OStr ON v.StrucID = OStr.ID 
  LEFT JOIN Structures AS UStr ON v.Ownership = UStr.ID
  LEFT JOIN dbo.VehicleEmployeeInfo AS ve ON ve.VehicleID = v.ID AND ve.Type = 1
  LEFT JOIN dbo.EmployeeInfo AS em ON ve.EmployeeInfoID = em.ID
  LEFT JOIN dbo.VehicleEmployeeInfo AS ve1 ON ve1.VehicleID = v.ID AND ve1.Type = 2
  LEFT JOIN dbo.EmployeeInfo AS em1 ON ve1.EmployeeInfoID = em1.ID
  WHERE v.[Status]<>9 AND  v.ID=@ID";
            List<NewEditVehicleModel> list = ConvertToList<NewEditVehicleModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            NewEditVehicleModel data = null;
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
                if (!string.IsNullOrEmpty(data.AnnualInspectionTime))
                {
                    data.AnnualInspectionTime = Convert.ToDateTime(data.AnnualInspectionTime).ToString("yyyy-MM-dd");
                }
                if (!string.IsNullOrEmpty(data.AnnualInspectionTime1))
                {
                    data.AnnualInspectionTime1 = Convert.ToDateTime(data.AnnualInspectionTime1).ToString("yyyy-MM-dd");
                }
            }
            return new SelectResult<NewEditVehicleModel>()
            {
                DataResult = data,
                Message = msg
            };
        }
        #endregion

        #region 宿州运达  编辑车辆信息
        public static SelectResult<NewEditVehicleModel> GetVehicleInfoByID_New_SZYD(long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
            };
            paras[0].Value = id;
            string sql = @"SELECT    v.ID , v.[CarToLand],v.[RoadransportNo],v.[TransportIndustryCode],v.SoftwareDate,
            v.PlateNum ,
            v.VehicleName ,
            v.PlateColorCode ,
            v.VehicleTypeCode ,
            v.StrucID AS EditStrucID ,
            v.StrucID ,
            Ostr.StrucName AS UserStrucName, 
            v.Ownership ,
            UStr.StrucName AS OwnerStrucName,
            CAST(v.WarrantyDate AS CHAR(10)) AS WarrantyDate ,
            v.Icon ,
            v.IsAccess ,
            v.IsDangerous ,
            v.IsReceived ,
            v.IsTransmit,
           v.Remark,
           tt.TerminalCode,
            tt.ID AS PrimaryTerminalID,
            tt.ID AS OldTerminalID,
            v.VIN,
            em.EmployeeName AS Driver,
            em.ID AS DriverID,
            em1.EmployeeName AS Carrier,
            em1.ID AS CarrierID,
            em2.EmployeeName AS OwnersName,
            em2.ID AS OwnersID 
  FROM      dbo.Vehicles v
  LEFT JOIN dbo.Terminals tt ON v.ID=tt.LinkedVehicleID 
  LEFT JOIN Structures AS OStr ON v.StrucID = OStr.ID 
  LEFT JOIN Structures AS UStr ON v.Ownership = UStr.ID
  LEFT JOIN dbo.VehicleEmployeeInfo AS ve ON ve.VehicleID = v.ID AND ve.Type = 1
  LEFT JOIN dbo.EmployeeInfo AS em ON ve.EmployeeInfoID = em.ID
  LEFT JOIN dbo.VehicleEmployeeInfo AS ve1 ON ve1.VehicleID = v.ID AND ve1.Type = 2
  LEFT JOIN dbo.EmployeeInfo AS em1 ON ve1.EmployeeInfoID = em1.ID 
LEFT JOIN dbo.VehicleEmployeeInfo AS ve2 ON ve2.VehicleID = v.ID AND ve2.Type = 3 
  LEFT JOIN dbo.EmployeeInfo AS em2 ON em2.ID = ve2.EmployeeInfoID 
  WHERE v.[Status]<>9 AND  v.ID=@ID";
            List<NewEditVehicleModel> list = ConvertToList<NewEditVehicleModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            NewEditVehicleModel data = null;
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
            return new SelectResult<NewEditVehicleModel>()
            {
                DataResult = data,
                Message = msg
            };
        }
        /// <summary>
        /// 编辑车辆
        /// 1.更新资料（编辑车辆代号，使用单位，是否危险品车、驾驶员、押运员、车主）
        /// 2.根据车辆ID删除终端关联信息
        /// 3.重新插入终端关联信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult NewEditVehicle_SZYD(NewEditVehicleModel model)
        {

            //1.更新车辆资料表
            //2.判断终端有没有更换  没有更换只更新车辆资料表 
            //3.1终端更换 / 原终端LinkedVehicleID置为空，修改状态0（查询原先绑定的终端信息用LinkedVehicleID） / 新终端绑定
            //3.2更新终端使用表，原终端添加结束时间，添加新终端和车辆关联记录  
            //4.车辆关联员工表记录  和车辆所属经营范围插入 

            #region 参数
            List<SqlParameter> parasList = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar),
                new SqlParameter("@VehicleName",SqlDbType.NVarChar),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@IsTransmit",SqlDbType.Bit),
                new SqlParameter("@IsDangerous",SqlDbType.Bit),
                new SqlParameter("@UpdateUserID",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar),
                new SqlParameter("@PrimaryTerminalID",SqlDbType.BigInt),
                new SqlParameter("@TerminalCode",SqlDbType.NVarChar),
                new SqlParameter("@DriverID",SqlDbType.Int),
                new SqlParameter("@CarrierID",SqlDbType.Int),
                new SqlParameter("@BusinessScopeList",SqlDbType.VarChar),
                new SqlParameter("@CarToLand",SqlDbType.Char),
                new SqlParameter("@RoadransportNo",SqlDbType.VarChar),
                new SqlParameter("@TransportIndustryCode",SqlDbType.Char),
                new SqlParameter("@TransportManagementList",SqlDbType.VarChar),
                new SqlParameter("@OldTerminalID",SqlDbType.BigInt), 
                new SqlParameter("@ID",SqlDbType.BigInt),
                new SqlParameter("@OwnersID",SqlDbType.Int),
                new SqlParameter("@VIN",SqlDbType.Char),
            };
            parasList[0].Value = model.PlateNum.Trim();
            parasList[1].Value = model.VehicleName.Trim();
            parasList[2].Value = model.EditStrucID;
            parasList[3].Value = model.IsTransmit;
            parasList[4].Value = model.IsDangerous;
            parasList[5].Value = model.UpdateUserID;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                parasList[6].Value = DBNull.Value;
            }
            else
            {
                parasList[6].Value = model.Remark;
            }
            parasList[7].Value = model.PrimaryTerminalID;
            parasList[8].Value = model.TerminalCode.Trim();
            if (model.DriverID.HasValue)
            {
                parasList[9].Value = model.DriverID;
            }
            else
            {
                parasList[9].Value = DBNull.Value;
            }
            if (model.IsDangerous)
            {
                parasList[10].Value = model.CarrierID;
                parasList[11].Value = model.BusinessScopeList.Trim();
                parasList[12].Value = model.CarToLand.Trim();
                parasList[13].Value = model.RoadransportNo.Trim();
                parasList[14].Value = model.TransportIndustryCode.Trim();
                if (model.IsTransmit)
                {
                    parasList[15].Value = model.TransportManagementList.Trim();
                }
                else
                {
                    parasList[15].Value = DBNull.Value;
                }
            }
            else
            {
                parasList[10].Value = DBNull.Value;
                parasList[11].Value = DBNull.Value;
                parasList[12].Value = DBNull.Value;
                parasList[13].Value = DBNull.Value;
                parasList[14].Value = DBNull.Value;
                parasList[15].Value = DBNull.Value;
            }
            parasList[16].Value = model.OldTerminalID;
            parasList[17].Value = model.ID;
            if (model.OwnersID.HasValue)
            {
                parasList[18].Value = model.OwnersID;
            }
            else
            {
                parasList[18].Value = DBNull.Value;
            }
            parasList[19].Value = model.VIN;
            #endregion

            string sql = string.Empty;

            #region 更新车辆资料 车辆代号，使用单位，是否危险品车、驾驶员、押运员、车主相关
            sql += @"UPDATE  dbo.Vehicles SET  VehicleName = @VehicleName ,CarToLand = @CarToLand,StrucID = @StrucID ,
                         IsTransmit = @IsTransmit ,IsDangerous = @IsDangerous ,RoadransportNo = @RoadransportNo,
                         Remark = @Remark,UpdateUserID=@UpdateUserID,EditTime = GETDATE(),TransportIndustryCode = @TransportIndustryCode 
                         WHERE   ID = @ID;";
            #endregion

            #region 如果更新了终端 就更新终端信息
            if (model.OldTerminalID != model.PrimaryTerminalID)
            {
                #region  修改终端表原终端信息状态  更新终端表新绑定终端
                sql += @"UPDATE  dbo.Terminals SET  LinkedVehicleID = null , Status=0  WHERE   ID = @OldTerminalID;
                                 UPDATE  dbo.Terminals SET  LinkedVehicleID = @ID, Status=7 WHERE   ID = @PrimaryTerminalID;";
                #endregion

                #region  更新终端使用表原纪录结束时间
                sql += @" UPDATE  dbo.TerminalUsageLogs SET EndDateTime = GETDATE()  
                                  WHERE  EndDateTime IS NULL AND VehicleID = @ID AND  TerminalID =@OldTerminalID;";
                #endregion

                #region  终端使用表添加记录
                sql += @"INSERT INTO dbo.TerminalUsageLogs(TerminalCode ,VehicleID , CreateDateTime ,CreateUserID,TerminalID,PlateNum,VIN)
                             VALUES  ( @TerminalCode , @ID, GETDATE() , @UpdateUserID,@PrimaryTerminalID,@PlateNum,@VIN);";
                #endregion
            }
            #endregion

            #region 不管车辆是不是危险品车 修改的时候先删除原来的数据  车辆关联员工表记录 车辆所属经营范围 车辆所属车管所
            sql += "DELETE FROM VehicleEmployeeInfo WHERE VehicleID = @ID;";
            sql += "DELETE FROM VehicleBussinessScope WHERE VehicleID = @ID;";
            sql += "DELETE FROM VehicleTransportManagement WHERE VehicleID = @ID;";
            #endregion

            #region 车辆关联驾驶员表记录  和车辆所属经营范围插入
            if (model.IsDangerous)
            {
                // 车辆关联驾驶员表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位

                //sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                //                  (@ID,@DriverID,1),(@ID,@CarrierID,2);";
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@ID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND IsDriver = 1),
                                1),
                              (@ID,  
                                (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @CarrierID AND IsCarrier = 1),
                               2);";

                // 车辆所属经营范围
                string[] bussinessScopeArray = model.BusinessScopeList.TrimEnd(',').Split(',');
                sql += @"INSERT INTO  [VehicleBussinessScope]([VehicleID],[BusinessScopeCode])VALUES";
                int length = bussinessScopeArray.Length;
                for (int i = 0; i < length; i++)
                {
                    if (i == length - 1)
                    {
                        sql += string.Format("(@ID,'{0}');", bussinessScopeArray[i]);
                    }
                    else
                    {
                        sql += string.Format("(@ID,'{0}'),", bussinessScopeArray[i]);
                    }
                }
                // 是危险品车 而且选择了转发运管 则进行以下操作
                if (model.IsTransmit)
                {
                    // 车辆关联的运管
                    string[] transportManagementArray = model.TransportManagementList.TrimEnd(',').Split(',');
                    sql += @"INSERT INTO [dbo].[VehicleTransportManagement]([VehicleID],[TransportManagementID])VALUES";
                    int tmLength = transportManagementArray.Length;
                    for (int i = 0; i < tmLength; i++)
                    {
                        if (i == tmLength - 1)
                        {
                            sql += string.Format("(@ID,{0});", transportManagementArray[i]);
                        }
                        else
                        {
                            sql += string.Format("(@ID,{0}),", transportManagementArray[i]);
                        }
                    }
                }
            }

            #endregion

            #region 不是危险品车但是选择了驾驶员信息
            if (!model.IsDangerous && model.DriverID.HasValue)
            {
                // 车辆关联员工表记录 类型 1 驾驶员 2押运员
                // 车辆关联员工表  当新增车辆信息勾选危险品车，选择使用单位后 此时会带出员工信息，
                // 但是如果此时新开一个界面在员工设定处把该员工信息的隶属单位修改掉 则就会出现员工已经不属于该单位了 但是还是被关联到该车辆中
                // 为了防止这类情况 添加VehicleEmployeeInfo表数据时 需要验证该员工是否属于使用单位
                // SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID 就是为了验证员工是否属于该使用单位
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@ID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE StrucID = @StrucID AND ID = @DriverID AND  IsDriver = 1),
                                1);";
            }
            #endregion

            #region 修改车主信息  因为前面已经删除了VehicleEmployeeInfo表中VehicleID = @ID的所有数据，所以这边判断是否选择了车主，有数据则直接添加
            if (model.OwnersID != null)
            {
                sql += @"INSERT INTO  [VehicleEmployeeInfo]([VehicleID],[EmployeeInfoID],[Type]) VALUES
                                (@ID,
                                  (SELECT TOP 1 ID FROM dbo.EmployeeInfo WHERE ID = @OwnersID AND  IsOwners = 1),
                                3);";
            }
            #endregion

            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, parasList.ToArray());
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 发送消息时的操作
        /// <summary>
        /// 获取发送消息时的设置内容
        /// </summary>
        /// <param name="VehicleId"></param>
        /// <returns></returns>
        public static SendMessageModel SendParameter(int VehicleId)
        {
            string sql = @"select top 1 OverspeedThreshold ,MinimumDuration,ContinuousDrivingThreshold ,MinimumBreakTime,DrivingTimeThreshold ,MaximumParkingTime
                                ,LastMiles  from TerminalUsageLogs where VehicleID=@VehicleID and EndDateTime is not null and ContinuousDrivingThreshold is not null  order by EndDateTime desc";
            List<SqlParameter> param = new List<SqlParameter>()
            {  
                new SqlParameter("@VehicleID",SqlDbType.NVarChar),
            };
            param[0].Value = VehicleId;
            List<SendMessageModel> list = ConvertToList<SendMessageModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, param.ToArray()));
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public static bool UpdateSendSuccessStatus(int VehicleID)
        {
            string sql = @"update Vehicles set Status=0 where ID=@ID";
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("ID",SqlDbType.BigInt)
            };
            param[0].Value = VehicleID;
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sql, param.ToArray());
            return result;
        }
        #endregion

        #region 下拉
        public static List<VehicleDropListModel> GetVehiclesByPlateNum(string PlateNum, int currentStrucID)
        {
            string sql = string.Format(@"SELECT v.ID,v.PlateNum FROM dbo.Vehicles v
                INNER JOIN Func_GetStrucAndSubStrucByUserAffiliatedStrucID({0}) s ON v.StrucID = s.ID
WHERE Status<>9 and PlateNum LIKE @PlateNum", currentStrucID);
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@PlateNum",SqlDbType.NVarChar)
            };
            paras[0].Value = "%" + PlateNum + "%";
            return ConvertToList<VehicleDropListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion


        #region 获取用户权限范围内的车辆信息
        /// <summary>
        /// 获取用户权限范围内的车辆信息(车牌、终端号、SIM卡号、单位)
        /// </summary>
        /// <param name="id">用户ID或者公司ID</param>
        /// <param name="viewMode">默认模式/自由模式</param>
        /// <returns></returns>
        public static DataTable GetVehicleInfo(int id,bool viewMode=true)
        {

            string sql = String.Empty;
            if (viewMode)
            {
                #region 默认模式
                sql = @"SELECT v.PlateNum,t.TerminalCode,sim.SimCode,s.StrucName,v.VIN FROM dbo.Structures s 
	                            JOIN dbo.Func_GetAllTheSubsetOfVehiclesByStrucID(@Id) AS vt 
		                            ON s.ID=vt.StrucID 
	                            JOIN dbo.Vehicles v 
		                            ON vt.VIN = v.VIN
	                            JOIN dbo.Terminals t 
                                    ON v.ID = t.LinkedVehicleID
	                            JOIN dbo.SimCodeList sim 
                                    ON t.SimCodeID = sim.ID 
                                ORDER BY s.StrucName ASC";
                #endregion
            }
            else
            {
                #region 自由模式
                sql = @"SELECT v.PlateNum,t.TerminalCode,sim.SimCode,s.StrucName,v.VIN FROM dbo.Structures s 
	                            JOIN dbo.Func_GetVehiclesListByUserID_New(@Id) AS vt 
		                            ON s.ID=vt.StrucID 
	                            JOIN dbo.Vehicles v 
		                            ON vt.VIN = v.VIN
	                            JOIN dbo.Terminals t 
                                    ON v.ID = t.LinkedVehicleID
	                            JOIN dbo.SimCodeList sim 
                                    ON t.SimCodeID = sim.ID
                                ORDER BY s.StrucName ASC";
                #endregion
            }
            SqlParameter para = new SqlParameter()
            {
                ParameterName="@Id",
                Value = id,
                SqlDbType= SqlDbType.Int
            };
            return MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, para);
        }
        #endregion

    }
}

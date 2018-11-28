using Asiatek.AjaxPager;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Asiatek.BLL.MSSQL
{
    /// <summary>
    /// 车机异常
    /// 比如：紧急报警
    /// </summary>
    public class ExceptionBLL
    {
        //        /// <summary>
        //        /// 根据用户ID获取用户分配的有效车辆尚未结束的紧急报警异常
        //        /// </summary>
        //        /// <param name="userID"></param>
        //        /// <returns></returns>
        //        public static List<EmergencyAlarmInfoModel> GetEmergencyAlarms(int userID)
        //        {
        //            //考虑到跨月情况，上月紧急报警还是需要查询出来，
        //            //那么规定如果当前是这个月的前三天，需要同时查询上个月的异常表
        //            //之所以是3天，是因为考虑周末2天没人看，所以多留一天，否则第三天不刷出来，就没人知道了
        //            //天数多也没意义
        //            //还有关于上个月异常表可能不存在的情况，这个只针对刚使用的时候
        //            //那个时候没有上个月异常表
        //            //本质上这里需要判断上个月异常表是否存在，存在才去查询
        //            //但是我们这里采用人工添加上个月月表的方式避免这个问题。

        //            var now = DateTime.Now;
        //            string currentMonth = now.ToString("yyyyMM");
        //            string prevMonth = now.AddMonths(-1).ToString("yyyyMM");

        //            string sql = string.Empty;

        //            if (now.Day <= 3)//前三天，需要查询上月异常表
        //            {
        //                #region 语句
        //                sql = @"
        //                DECLARE @tempTable TABLE(VehicleName NVARCHAR(20),StrucName NVARCHAR(200),TerminalCode NVARCHAR(30))
        //                INSERT INTO @tempTable 
        //                SELECT userVehicles.VehicleName,s.StrucName,t.TerminalCode FROM dbo.Structures s
        //                INNER JOIN
        //                (
        //                		SELECT v.ID AS VID,v.StrucID,v.VehicleName FROM dbo.Vehicles v
        //                		INNER JOIN 
        //                		(
        //                			SELECT StrucID FROM dbo.StructureDistributionInfo 
        //                			WHERE UserID=@userID
        //                		) AS temp1
        //                		ON v.StrucID=temp1.StrucID
        //                		WHERE v.IsReceived=1 AND v.Status=0
        //                		UNION
        //                		SELECT v.ID,v.StrucID,v.VehicleName FROM dbo.VehicleDistributionInfo vdi
        //                		INNER JOIN dbo.Vehicles v ON  vdi.VehicleID=v.ID
        //                		WHERE UserID=@userID AND v.IsReceived=1 AND v.Status=0
        //                ) AS userVehicles ON userVehicles.StrucID=s.ID
        //                INNER JOIN dbo.VehiclesTerminals vt ON userVehicles.VID=vt.VehicleID
        //                INNER JOIN dbo.Terminals t ON vt.TerminalID=t.ID
        //                WHERE vt.IsPrimary=1
        //                SELECT e.ID,e.GPSStartTime AS StartDateTime,e.Status,e.Address,[@tempTable].* FROM dbo.Exception_" + currentMonth + @" e
        //                INNER JOIN @tempTable ON [@tempTable].TerminalCode=e.TerminalCode
        //                WHERE ExceptionTypeID=1 AND GPSEndTime IS NULL
        //                UNION ALL
        //                SELECT e.ID,e.GPSStartTime AS StartDateTime,e.Status,e.Address,[@tempTable].* FROM dbo.Exception_" + prevMonth + @" e
        //                INNER JOIN @tempTable ON [@tempTable].TerminalCode=e.TerminalCode
        //                WHERE ExceptionTypeID=1 AND GPSEndTime IS NULL";

        //                #endregion
        //            }
        //            else//只查询当月的
        //            {
        //                #region 语句
        //                sql = @"
        //SELECT e.ID,e.GPSStartTime AS StartDateTime,e.Status,e.Address,tempTable.* FROM dbo.Exception_" + currentMonth + @" e
        //INNER JOIN 
        //(
        //SELECT userVehicles.VehicleName,s.StrucName,t.TerminalCode FROM dbo.Structures s
        //INNER JOIN
        //(
        //		SELECT v.ID AS VID,v.StrucID,v.VehicleName FROM dbo.Vehicles v
        //		INNER JOIN 
        //		(
        //			SELECT StrucID FROM dbo.StructureDistributionInfo 
        //			WHERE UserID=@userID
        //		) AS temp1
        //		ON v.StrucID=temp1.StrucID
        //		WHERE v.IsReceived=1 AND v.Status=0
        //		UNION
        //		SELECT v.ID,v.StrucID,v.VehicleName FROM dbo.VehicleDistributionInfo vdi
        //		INNER JOIN dbo.Vehicles v ON  vdi.VehicleID=v.ID
        //		WHERE UserID=@userID AND v.IsReceived=1 AND v.Status=0
        //) AS userVehicles ON userVehicles.StrucID=s.ID
        //INNER JOIN dbo.VehiclesTerminals vt ON userVehicles.VID=vt.VehicleID
        //INNER JOIN dbo.Terminals t ON vt.TerminalID=t.ID
        //WHERE vt.IsPrimary=1
        //) AS tempTable
        // ON tempTable.TerminalCode=e.TerminalCode
        //WHERE ExceptionTypeID=1 AND GPSEndTime IS NULL";
        //                #endregion
        //            }

        //            SqlParameter[] paras = new SqlParameter[1];
        //            paras[0] = new SqlParameter()
        //            {
        //                ParameterName = "@userID",
        //                Value = userID,
        //                SqlDbType = SqlDbType.Int
        //            };
        //            return ConvertToList<EmergencyAlarmInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        //        }


        //        /// <summary>
        //        /// 根据用户ID获取用户分配的有效车辆尚未结束的紧急报警异常
        //        /// </summary>
        //        /// <param name="userID"></param>
        //        /// <returns></returns>
        //        public static List<EmergencyAlarmInfoModel> GetEmergencyAlarms(int userID)
        //        {

        //            string sql = @"SELECT e.ID,e.GPSStartTime AS StartDateTime,e.Status,e.Address,tempTable.* FROM dbo.Exception e
        //INNER JOIN 
        //(
        //SELECT userVehicles.VehicleName,s.StrucName,t.TerminalCode FROM dbo.Structures s
        //INNER JOIN
        //(
        //		SELECT v.ID AS VID,v.StrucID,v.VehicleName FROM dbo.Vehicles v
        //		INNER JOIN 
        //		(
        //			SELECT StrucID FROM dbo.StructureDistributionInfo 
        //			WHERE UserID=@userID
        //		) AS temp1
        //		ON v.StrucID=temp1.StrucID
        //		WHERE v.IsReceived=1 AND v.Status=0
        //		UNION
        //		SELECT v.ID,v.StrucID,v.VehicleName FROM dbo.VehicleDistributionInfo vdi
        //		INNER JOIN dbo.Vehicles v ON  vdi.VehicleID=v.ID
        //		WHERE UserID=@userID AND v.IsReceived=1 AND v.Status=0
        //) AS userVehicles ON userVehicles.StrucID=s.ID
        //INNER JOIN dbo.VehiclesTerminals vt ON userVehicles.VID=vt.VehicleID
        //INNER JOIN dbo.Terminals t ON vt.TerminalID=t.ID
        //WHERE vt.IsPrimary=1
        //) AS tempTable
        // ON tempTable.TerminalCode=e.TerminalCode
        //WHERE ExceptionTypeID=1 AND GPSEndTime IS NULL";


        //            SqlParameter[] paras = new SqlParameter[1];
        //            paras[0] = new SqlParameter()
        //            {
        //                ParameterName = "@userID",
        //                Value = userID,
        //                SqlDbType = SqlDbType.Int
        //            };
        //            return ConvertToList<EmergencyAlarmInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        //        }

        #region 紧急告警

        #region 自由模式
        /// <summary>
        /// 根据用户ID获取用户分配的有效车辆尚未结束的紧急报警异常 自由模式
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<EmergencyAlarmInfoModel> GetEmergencyAlarms(int userID)
        {
            //            string sql = @"SELECT e.ID,e.TerminalCode,tempTable.StrucName,tempTable.VehicleName,e.SignalStartTime AS StartDateTime,e.StartAddress AS Address,e.Status FROM 
            //dbo.VW_Exceptions e
            //INNER JOIN 
            //(
            //SELECT userVehicles.VehicleName,userVehicles.VIN,s.StrucName FROM dbo.Structures s
            //INNER JOIN
            //(
            //		SELECT v.ID AS VID,v.StrucID,v.VehicleName,v.VIN FROM dbo.Vehicles v
            //		INNER JOIN 
            //		(
            //			SELECT StrucID FROM dbo.StructureDistributionInfo 
            //			WHERE UserID=@userID
            //		) AS temp1
            //		ON v.StrucID=temp1.StrucID
            //		WHERE v.IsReceived=1 AND v.Status=0
            //		UNION
            //		SELECT v.ID,v.StrucID,v.VehicleName,v.VIN FROM dbo.VehicleDistributionInfo vdi
            //		INNER JOIN dbo.Vehicles v ON  vdi.VehicleID=v.ID
            //		WHERE UserID=@userID AND v.IsReceived=1 AND v.Status=0
            //) AS userVehicles ON userVehicles.StrucID=s.ID
            //) AS tempTable ON e.VIN=tempTable.VIN
            //WHERE e.ExceptionTypeID=1 AND e.SignalEndTime IS NULL";
            string sql = @"SELECT e.ID,e.TerminalCode,tempTable.StrucName,tempTable.VehicleName,
                                    e.SignalStartTime AS StartDateTime,e.StartAddress AS Address,e.Status FROM 
                                   dbo.VW_Exceptions e
                                    INNER JOIN 
                                    (
                                        SELECT userVehicles.VehicleName,userVehicles.VIN,s.StrucName FROM dbo.Structures s
                                        INNER JOIN Func_GetVehiclesListByUserID_New(@userID) AS userVehicles ON userVehicles.StrucID=s.ID
                                    ) AS tempTable ON e.VIN=tempTable.VIN
                                    WHERE e.ExceptionTypeID=1 AND e.SignalEndTime IS NULL";

            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@userID",
                Value = userID,
                SqlDbType = SqlDbType.Int
            };
            return ConvertToList<EmergencyAlarmInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 默认模式
        /// <summary>
        /// 根据用户ID获取用户分配的有效车辆尚未结束的紧急报警异常 默认模式
        /// </summary>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static List<EmergencyAlarmInfoModel> GetDefaultEmergencyAlarms(int strucID)
        {
            string sql = @"SELECT e.ID,e.TerminalCode,tempTable.StrucName,tempTable.VehicleName,
                                    e.SignalStartTime AS StartDateTime,e.StartAddress AS Address,e.Status FROM 
                                    dbo.VW_Exceptions e
                                    INNER JOIN 
                                    (
                                        SELECT userVehicles.VehicleName,userVehicles.VIN,s.StrucName FROM dbo.Structures s
                                        INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID(@StrucID) AS userVehicles ON userVehicles.StrucID=s.ID
                                    ) AS tempTable ON e.VIN=tempTable.VIN
                                    WHERE e.ExceptionTypeID=1 AND e.SignalEndTime IS NULL";

            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            return ConvertToList<EmergencyAlarmInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion
        #endregion



        /// <summary>
        /// 处理紧急报警成功后更新报警数据（所谓处理成功：1.应答成功；2.超时）
        /// </summary>
        /// <param name="ID"></param>
        public static OperationResult UpdateEmergencyAlarm(UpdateEmergencyAlarmModel model)
        {
            string sql = @"UPDATE {0}.GNSS.dbo.Exception SET Status=@Status,
DealTime=GETDATE(),DealUserID=@userID,DealInfo=@DealInfo WHERE ID=@ID";
            sql = string.Format(sql, model.LinkedServerName);
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter()
            {
                ParameterName = "@ID",
                Value = model.ID
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@Status",
                Value = model.Timeout ? 2 : 1//1：成功；2：超时
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@userID",
                Value = model.DealUserID
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@DealInfo",
                Value = string.IsNullOrWhiteSpace(model.DealInfo) ? (object)DBNull.Value : model.DealInfo.Trim()
            });


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

        #region 实时异常

        #region 自由模式
        /// <summary>
        /// 实时异常 自由模式
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<RealTimeExceptionModel> GetRealTimeExceptions(int userID)
        {
            //            string sql = @"SELECT  et.ExName AS ExTypeName,
            //         CONVERT(CHAR(19),SignalStartTime,120) AS StartDateTime,
            //        rte.VIN, innerTable.StrucName,innerTable.VehicleName
            //FROM    dbo.VW_GetRealTimeExceptions rte
            //INNER JOIN dbo.ExceptionType et ON rte.ExceptionTypeID=et.ID
            //INNER JOIN 
            //(
            //SELECT t.TerminalCode,userVehicles.StrucName,userVehicles.VehicleName,userVehicles.VIN FROM dbo.Terminals t  
            //INNER JOIN 
            //(
            //		SELECT v.ID,v.VehicleName,v.VIN,s.StrucName FROM dbo.Vehicles v
            //		INNER JOIN dbo.Structures s ON v.StrucID=s.ID
            //		INNER JOIN 
            //		(
            //			SELECT StrucID FROM dbo.StructureDistributionInfo 
            //			WHERE UserID=@userID
            //		) AS temp1
            //		ON v.StrucID=temp1.StrucID
            //		WHERE v.IsReceived=1 AND v.Status=0
            //		UNION
            //		SELECT v.ID,v.VehicleName,v.VIN,s.StrucName FROM dbo.VehicleDistributionInfo vdi
            //		INNER JOIN dbo.Vehicles v ON  vdi.VehicleID=v.ID
            //		INNER JOIN dbo.Structures s ON v.StrucID=s.ID
            //		WHERE vdi.UserID=@userID AND v.IsReceived=1 AND v.Status=0
            //	) 
            //	AS userVehicles ON t.LinkedVehicleID=userVehicles.ID
            //) AS innerTable ON rte.VIN=innerTable.VIN
            //WHERE rte.ExceptionTypeID<>1
            //ORDER BY ServerStartTime DESC";
            string sql = @"SELECT  et.ExName AS ExTypeName,
                                     CONVERT(CHAR(19),SignalStartTime,120) AS StartDateTime,
                                    rte.VIN, innerTable.StrucName,innerTable.VehicleName
                                    FROM    dbo.VW_GetRealTimeExceptions rte
                                    INNER JOIN dbo.ExceptionType et ON rte.ExceptionTypeID=et.ID
                                    INNER JOIN 
                                    (
                                    SELECT t.TerminalCode,userVehicles.StrucName,userVehicles.VehicleName,userVehicles.VIN FROM dbo.Terminals t  
                                    INNER JOIN 
                                    (
		                                     SELECT vt.VID AS ID,vt.VehicleName,vt.VIN,s.StrucName FROM dbo.Structures s 
                                             INNER JOIN Func_GetVehiclesListByUserID_New(@userID) AS vt ON s.ID=vt.StrucID
	                                )  AS userVehicles ON t.LinkedVehicleID=userVehicles.ID
                                    ) AS innerTable ON rte.VIN=innerTable.VIN
                                    WHERE rte.ExceptionTypeID<>1
                                    ORDER BY ServerStartTime DESC";

            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@userID",
                Value = userID,
                SqlDbType = SqlDbType.Int
            };
            return ConvertToList<RealTimeExceptionModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 默认模式
        /// <summary>
        /// 实时异常 默认模式
        /// </summary>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static List<RealTimeExceptionModel> GetDefaultRealTimeExceptions(int strucID)
        {
            string sql = @"SELECT  et.ExName AS ExTypeName,
                                     CONVERT(CHAR(19),SignalStartTime,120) AS StartDateTime,
                                    rte.VIN, innerTable.StrucName,innerTable.VehicleName
                            FROM    dbo.VW_GetRealTimeExceptions rte
                            INNER JOIN dbo.ExceptionType et ON rte.ExceptionTypeID=et.ID
                            INNER JOIN 
                            (
                            SELECT t.TerminalCode,userVehicles.StrucName,userVehicles.VehicleName,userVehicles.VIN FROM dbo.Terminals t  
                            INNER JOIN 
                            (
		                          SELECT vt.VID AS ID,vt.VehicleName,vt.VIN,s.StrucName FROM dbo.Structures s 
                                  INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID(@StrucID) AS vt ON s.ID=vt.StrucID
	                         )  AS userVehicles ON t.LinkedVehicleID=userVehicles.ID
                            ) AS innerTable ON rte.VIN=innerTable.VIN
                            WHERE rte.ExceptionTypeID<>1
                            ORDER BY ServerStartTime DESC";

            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            return ConvertToList<RealTimeExceptionModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion
        #endregion

        #region 年检
        #region 自由模式
        public static List<YearCheckAlarmModel> GetYearCheckAlarm(int userID) {
            string sql = @"SELECT v.AnnualInspectionTime,v.AnnualInspectionTime1,v.PlateNum,v.RemindTimeSpan,v.RemindTimeSpan1,s.StrucName,v.VehicleName FROM dbo.Vehicles v
left join dbo.Structures s on v.StrucID=s.ID
where v.ID in (SELECT vt.VID  FROM dbo.Structures s 
                                             INNER JOIN Func_GetVehiclesListByUserID_New(@userID) AS vt ON s.ID=vt.StrucID) and AnnualInspectionTime is not null and RemindTimeSpan>0";
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@userID",
                Value = userID,
                SqlDbType = SqlDbType.Int
            };
            List<YearCheckAlarmModel> list = ConvertToList<YearCheckAlarmModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
            List<YearCheckAlarmModel> returnmodel = new List<YearCheckAlarmModel>();
            if (list == null || list.Count <= 0) {
                return null;
            }
            foreach (var model in list)
            {
                DateTime dtnow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime check = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + model.AnnualInspectionTime.Month.ToString() + "-" + model.AnnualInspectionTime.Day.ToString());
                int alarmtimespan = (int)(check - dtnow).TotalDays;
                DateTime check1 = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + model.AnnualInspectionTime1.Month.ToString() + "-" + model.AnnualInspectionTime1.Day.ToString());
                int alarmtimespan1 = (int)(check1 - dtnow).TotalDays;
                //年检时间不能为空，设置的提前提醒时间不能为0
                if (alarmtimespan >= 0 && alarmtimespan <= model.RemindTimeSpan && model.RemindTimeSpan >= 0 && model.AnnualInspectionTime > DateTime.MinValue)
                {
                    returnmodel.Add(model);
                }
                else if (alarmtimespan1 >= 0 && alarmtimespan1 <= model.RemindTimeSpan1 && model.RemindTimeSpan1 >= 0 && model.AnnualInspectionTime1 > DateTime.MinValue)
                {
                    returnmodel.Add(model);
                }
            }
            return returnmodel;
        }

        #endregion

        #region 默认模式
        public static List<YearCheckAlarmModel> GetDefaultCheckAlarmModel(int strucID)
        {
            string sql = @"SELECT v.AnnualInspectionTime,v.AnnualInspectionTime1,v.PlateNum,v.RemindTimeSpan,v.RemindTimeSpan1,s.StrucName,v.VehicleName FROM dbo.Vehicles v
left join dbo.Structures s on v.StrucID=s.ID
where v.ID in (SELECT vt.VID FROM dbo.Structures s INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID(@StrucID) AS vt ON s.ID=vt.StrucID) and ((AnnualInspectionTime is not null and RemindTimeSpan>=0) or (AnnualInspectionTime1 is not null and RemindTimeSpan1>=0))";
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            List<YearCheckAlarmModel> list = ConvertToList<YearCheckAlarmModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
            List<YearCheckAlarmModel> returnmodel = new List<YearCheckAlarmModel>();
            if (list == null || list.Count <= 0) {
                return null;
            }
            foreach (var model in list)
            { 
                DateTime dtnow=Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                DateTime check = Convert.ToDateTime(DateTime.Now.Year.ToString() +"-"+ model.AnnualInspectionTime.Month.ToString() +"-"+ model.AnnualInspectionTime.Day.ToString());
                int alarmtimespan =(int) (check-dtnow).TotalDays;
                DateTime check1 = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-" + model.AnnualInspectionTime1.Month.ToString() + "-" + model.AnnualInspectionTime1.Day.ToString());
                int alarmtimespan1 = (int)(check1 - dtnow).TotalDays;
                //年检时间不能为空，设置的提前提醒时间不能为0
                if (alarmtimespan >= 0 && alarmtimespan <= model.RemindTimeSpan&&model.RemindTimeSpan>=0&&model.AnnualInspectionTime>DateTime.MinValue) {
                    returnmodel.Add(model);
                }
                else if (alarmtimespan1 >= 0 && alarmtimespan1 <= model.RemindTimeSpan1 && model.RemindTimeSpan1 >= 0 && model.AnnualInspectionTime1 > DateTime.MinValue)
                {
                    returnmodel.Add(model);
                }
            }
            return returnmodel;
        }
        #endregion 
        #endregion

        #region 车辆保养
        #region 自由模式
        public static List<MaintenanceCheckAlarmModel> GetMaintenanceCheckAlarm(int userID)
        {
            string sql = string.Format(@"SELECT vm.VehicleID,vm.ScheduleID,vm.FirstMaintenanceMile,vm.FirstMaintenanceTime,
  ms.ScheduleName,ms.RulesType,ms.SettingMile,ms.PreMile,ms.SettingDay,ms.PreDay,v.VehicleName,v.VIN,s.StrucName,
	CASE ms.RulesType
            WHEN 1 THEN ms.SettingMile-CAST((sg.Mileage-vm.FirstMaintenanceMile) AS INT)%ms.SettingMile
            WHEN 3 THEN ms.SettingMile-CAST((sg.Mileage-vm.FirstMaintenanceMile) AS INT)%ms.SettingMile
    END AS alarmMile,
    CASE ms.RulesType
            WHEN 2 THEN ms.SettingDay-(SELECT DATEDIFF(DAY,vm.FirstMaintenanceTime,sg.SignalDateTime))%ms.SettingDay
            WHEN 3 THEN ms.SettingDay-(SELECT DATEDIFF(DAY,vm.FirstMaintenanceTime,sg.SignalDateTime))%ms.SettingDay
    END AS alarmTime
  FROM dbo.VehicleMaintenance vm 
  INNER JOIN dbo.MaintenanceSchedule ms ON ms.ID=vm.ScheduleID 
  INNER JOIN dbo.Vehicles v ON v.ID = vm.VehicleID 
  INNER JOIN [dbo].[VW_GetRealTimeSignals] sg ON sg.VIN = v.VIN 
  LEFT JOIN dbo.Structures s on v.StrucID=s.ID
WHERE v.ID in (SELECT vt.VID  FROM dbo.Structures s INNER JOIN Func_GetVehiclesListByUserID_New({0}) AS vt ON s.ID=vt.StrucID)", userID);
            List<MaintenanceCheckAlarmModel> list = ConvertToList<MaintenanceCheckAlarmModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
            List<MaintenanceCheckAlarmModel> result = new List<MaintenanceCheckAlarmModel>();
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            else
            {  //如果既设定了里程周期又设定了时间周期，只要有一个有预警就进行提醒，两者都有预警就都提醒
                foreach (var model in list)
                {
                    //里程预警提醒的
                    if (model.RulesType == 1 && model.alarmMile != null && model.alarmMile <= model.PreMile)
                    {
                        model.AlarmInfo = @DisplayText.Mileage2+@UIText.MaintenanceCheckAlarm;
                        result.Add(model);
                    }
                    //时间预警提醒的
                    if (model.RulesType == 2 && model.alarmTime != null && model.alarmTime <= model.PreDay)
                    {
                        model.AlarmInfo = @DisplayText.Time + @UIText.MaintenanceCheckAlarm;
                        result.Add(model);
                    }
                    //里程或时间预警提醒的
                    if (model.RulesType == 3)
                    {
                        //只有里程预警
                        if (model.alarmMile != null && model.alarmTime == null && model.alarmMile <= model.PreMile)
                        {
                            model.AlarmInfo = @DisplayText.Mileage2 + @UIText.MaintenanceCheckAlarm;
                            result.Add(model);
                        }
                        //只有时间预警
                        if (model.alarmTime != null && model.alarmMile == null && model.alarmTime <= model.PreDay)
                        {
                            model.AlarmInfo = @DisplayText.Time + @UIText.MaintenanceCheckAlarm;
                            result.Add(model);
                        }
                        //两者都有
                        if (model.alarmMile != null && model.alarmTime != null && model.alarmMile <= model.PreMile && model.alarmTime <= model.PreDay)
                        {
                            model.AlarmInfo = @DisplayText.Mileage2 + @DisplayText.And + @DisplayText.Time + @UIText.MaintenanceCheckAlarm;
                            result.Add(model);
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region 默认模式
        public static List<MaintenanceCheckAlarmModel> GetDefaultMaintenanceCheckAlarmModel(int strucID)
        {
            //alarmMile=设置里程数-（当前里程数-初始里程数）%设定里程数
            //alarmTime=设置天数-（初试时间和信号时间的间隔天数）%设置天数
            //alarmMile<=提前提醒里程数，alarmTime<=提前提醒天数，车辆保养预警提醒
            //因为sg.Mileage、vm.FirstMaintenanceMile是浮点数，ms.SettingMile是整数，数据类型 float 和 int 在 modulo 运算符中不兼容，两个float也无法运算，所以将前者转化成int型，四舍五入，会出现一点偏差
            string sql = string.Format(@"  SELECT vm.VehicleID,vm.ScheduleID,vm.FirstMaintenanceMile,vm.FirstMaintenanceTime,
  ms.ScheduleName,ms.RulesType,ms.SettingMile,ms.PreMile,ms.SettingDay,ms.PreDay,userVehicles.VehicleName,userVehicles.VIN,userVehicles.StrucName,
	CASE ms.RulesType
            WHEN 1 THEN ms.SettingMile-CAST((sg.Mileage-vm.FirstMaintenanceMile) AS INT)%ms.SettingMile
            WHEN 3 THEN ms.SettingMile-CAST((sg.Mileage-vm.FirstMaintenanceMile) AS INT)%ms.SettingMile
    END AS alarmMile,
    CASE ms.RulesType
            WHEN 2 THEN ms.SettingDay-(SELECT DATEDIFF(DAY,vm.FirstMaintenanceTime,sg.SignalDateTime))%ms.SettingDay
            WHEN 3 THEN ms.SettingDay-(SELECT DATEDIFF(DAY,vm.FirstMaintenanceTime,sg.SignalDateTime))%ms.SettingDay
    END AS alarmTime
  FROM dbo.VehicleMaintenance vm 
  INNER JOIN dbo.MaintenanceSchedule ms ON ms.ID=vm.ScheduleID 
  INNER JOIN  
  (
      SELECT vt.VID AS ID,vt.VehicleName,vt.VIN,s.StrucName FROM dbo.Structures s 
      INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID({0}) AS vt ON s.ID=vt.StrucID
  )  AS userVehicles ON vm.VehicleID=userVehicles.ID
  INNER JOIN [dbo].[VW_GetRealTimeSignals] sg ON sg.VIN = userVehicles.VIN ", strucID);
            List<MaintenanceCheckAlarmModel> list = ConvertToList<MaintenanceCheckAlarmModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
            List<MaintenanceCheckAlarmModel> result = new List<MaintenanceCheckAlarmModel>();
            if (list == null || list.Count <= 0)
            {
                return null;
            }
            else
            {  //如果既设定了里程周期又设定了时间周期，只要有一个有预警就进行提醒，两者都有预警就都提醒
                foreach (var model in list)
                {
                    //里程预警提醒的
                    if (model.RulesType == 1 && model.alarmMile != null && model.alarmMile <= model.PreMile)
                    {
                        model.AlarmInfo = @DisplayText.Mileage2 + @UIText.MaintenanceCheckAlarm;
                        result.Add(model);
                    }
                    //时间预警提醒的
                    if (model.RulesType == 2 && model.alarmTime != null && model.alarmTime <= model.PreDay)
                    {
                        model.AlarmInfo = @DisplayText.Time + @UIText.MaintenanceCheckAlarm;
                        result.Add(model);
                    }
                    //里程或时间预警提醒的
                    if (model.RulesType == 3)
                    {
                        //两者都有
                        if (model.alarmMile != null && model.alarmTime != null && model.alarmMile <= model.PreMile && model.alarmTime <= model.PreDay)
                        {
                            model.AlarmInfo = @DisplayText.Mileage2 + @DisplayText.And + @DisplayText.Time + @UIText.MaintenanceCheckAlarm;
                            result.Add(model);
                        }
                        //只有里程预警
                        else if (model.alarmMile != null && model.alarmMile <= model.PreMile)
                        {
                            model.AlarmInfo = @DisplayText.Mileage2 + @UIText.MaintenanceCheckAlarm;
                            result.Add(model);
                        }
                        //只有时间预警
                        else if (model.alarmTime != null && model.alarmTime <= model.PreDay)
                        {
                            model.AlarmInfo = @DisplayText.Time + @UIText.MaintenanceCheckAlarm;
                            result.Add(model);
                        }
                       
                    }
                }
            }
            return result;
        }
        #endregion
        #endregion


        #region 异常处理数量与列表
        #region 获取数量
        /// <summary>
        /// 默认模式使用
        /// 获取当前单位及子单位下车辆的未处理异常（目前写死为 超速、疲劳驾驶、夜间禁行，后续改成可配置）
        /// 发生异常返回null
        /// </summary>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static int? GetDefaultNeedDealAlarmCount(int strucID)
        {
            //            string sql = @"SELECT COUNT(0) FROM dbo.VW_Exceptions e
            //INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID(@StrucID) v ON e.VIN=v.VIN
            //WHERE e.ExceptionTypeID IN(2,5,103) AND Status<>1 AND e.SignalStartTime BETWEEN @BeginDateTime AND @EndDateTime";
            //var now = DateTime.Now;
            //SqlParameter[] paras = new SqlParameter[3];
            //paras[0] = new SqlParameter()
            //{
            //    ParameterName = "@StrucID",
            //    Value = strucID,
            //    SqlDbType = SqlDbType.Int
            //};
            //paras[1] = new SqlParameter()
            //{
            //    ParameterName = "@BeginDateTime",
            //    Value = now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00"),
            //    SqlDbType = SqlDbType.DateTime
            //};
            //paras[2] = new SqlParameter()
            //{
            //    ParameterName = "@EndDateTime",
            //    Value = now.AddDays(1).ToString("yyyy-MM-dd 00:00:00"),
            //    SqlDbType = SqlDbType.DateTime
            //};
            //var obj = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras);
            //if (obj == null)
            //{
            //    return null;
            //}
            //return Convert.ToInt32(obj);
            string sql = @"SELECT COUNT(0) FROM dbo.VW_SZYDExceptions e
INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID(@StrucID) v ON e.VIN=v.VIN";
            SqlParameter para = new SqlParameter
            {
                ParameterName = "@StrucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            var obj = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, para);
            if (obj == null)
            {
                return null;
            }
            return Convert.ToInt32(obj);

        }
        /// <summary>
        /// 自由模式使用
        /// 获取用户分配的车辆的未处理异常（目前写死为 超速、疲劳驾驶、夜间禁行，后续改成可配置）
        /// 发生异常返回null
        /// </summary>
        public static int? GetNeedDealAlarmCount(int userID)
        {
            string sql = @"SELECT COUNT(0) FROM dbo.VW_SZYDExceptions e
INNER JOIN Func_GetVehiclesListByUserID_New(@UserID) v ON e.VIN=v.VIN";
            var now = DateTime.Now;
            SqlParameter para = new SqlParameter()
            {
                ParameterName = "@UserID",
                Value = userID,
                SqlDbType = SqlDbType.Int
            };
            var obj = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, para);
            if (obj == null)
            {
                return null;
            }
            return Convert.ToInt32(obj);
            //            string sql = @"SELECT COUNT(0) FROM dbo.VW_Exceptions e
            //INNER JOIN Func_GetVehiclesListByUserID_New(@UserID) v ON e.VIN=v.VIN
            //WHERE e.ExceptionTypeID IN(2,5,103) AND Status<>1 AND e.SignalStartTime BETWEEN @BeginDateTime AND @EndDateTime";
            //            var now = DateTime.Now;
            //            SqlParameter[] paras = new SqlParameter[3];
            //            paras[0] = new SqlParameter()
            //            {
            //                ParameterName = "@UserID",
            //                Value = userID,
            //                SqlDbType = SqlDbType.Int
            //            };
            //            paras[1] = new SqlParameter()
            //            {
            //                ParameterName = "@BeginDateTime",
            //                Value = now.AddDays(-7).ToString("yyyy-MM-dd 00:00:00"),
            //                SqlDbType = SqlDbType.DateTime
            //            };
            //            paras[2] = new SqlParameter()
            //            {
            //                ParameterName = "@EndDateTime",
            //                Value = now.AddDays(1).ToString("yyyy-MM-dd 00:00:00"),
            //                SqlDbType = SqlDbType.DateTime
            //            };
            //            var obj = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras);
            //            if (obj == null)
            //            {
            //                return null;
            //            }
            //            return Convert.ToInt32(obj);
        }
        #endregion

        /// <summary>
        /// 获取所属单位以及子单位车辆的待处理异常
        /// </summary>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static AsiatekPagedList<NeedDealExceptionListModel> GetDefaultNeedDealExceptionList(NeedDealExceptionSearchModel model, int searchPage, int pageSize, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","VW_SZYDExceptions e"),
                new SqlParameter("@joinStr",@" INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID("+strucID+@") v ON e.VIN=v.VIN
INNER JOIN dbo.ExceptionType et ON et.ID=e.ExceptionTypeID
INNER JOIN dbo.Terminals t ON t.LinkedVehicleID=v.VID
INNER JOIN dbo.ServerInfo si ON t.ServerInfoID=si.ID AND si.ServerCode=e.ServerCode"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","e.SignalStartTime DESC"),
                new SqlParameter("@showColumns",@"e.ID AS ExID,et.ExName,v.VehicleName,e.SignalStartTime,e.StartAddress,e.SignalEndTime,e.EndAddress,si.LinkedServerName,e.TerminalCode,e.MaxSpeed,e.OverspeedThreshold"),
            };

            #region 筛选条件
            string conditionStr = "e.ExceptionTypeID IN(2,5,103) AND e.Status<>1 AND e.SignalStartTime BETWEEN '" + model.BeginDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + model.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            if (model.EndState == 1)//结束
            {
                conditionStr += " AND e.SignalEndTime IS NOT NULL";
            }
            else if (model.EndState == 0)//未结束
            {
                conditionStr += " AND e.SignalEndTime IS NULL";
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

            var list = ConvertToList<NeedDealExceptionListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        public static AsiatekPagedList<NeedDealExceptionListModel> GetNeedDealExceptionList(NeedDealExceptionSearchModel model, int searchPage, int pageSize, int userID)
        {

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","VW_SZYDExceptions e"),
                new SqlParameter("@joinStr",@" INNER JOIN Func_GetVehiclesListByUserID_New("+userID+@") v ON e.VIN=v.VIN
INNER JOIN dbo.ExceptionType et ON et.ID=e.ExceptionTypeID
INNER JOIN dbo.Terminals t ON t.LinkedVehicleID=v.VID
INNER JOIN dbo.ServerInfo si ON t.ServerInfoID=si.ID AND si.ServerCode=e.ServerCode"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","e.SignalStartTime DESC"),
                new SqlParameter("@showColumns",@"e.ID AS ExID,et.ExName,v.VehicleName,e.SignalStartTime,e.StartAddress,e.SignalEndTime,e.EndAddress,si.LinkedServerName,e.TerminalCode"),
            };

            #region 筛选条件
            string conditionStr = "e.ExceptionTypeID IN(2,5,103) AND e.Status<>1 AND e.SignalStartTime BETWEEN '" + model.BeginDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + model.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            if (model.EndState == 1)//结束
            {
                conditionStr += " AND e.SignalEndTime IS NOT NULL";
            }
            else if (model.EndState == 0)//未结束
            {
                conditionStr += " AND e.SignalEndTime IS NULL";
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

            var list = ConvertToList<NeedDealExceptionListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }


        /// <summary>
        /// 处理报警
        /// </summary>
        public static OperationResult DealAlarm(DealAlarmModel model)
        {
            string sql = @"UPDATE {0}.GNSS.dbo.Exception SET Status=1,
DealTime=GETDATE(),DealUserID=@userID,DealInfo=@DealInfo WHERE ID=@ID";
            sql = string.Format(sql, model.LinkedServerName);
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter()
            {
                ParameterName = "@ID",
                Value = model.EID,
                SqlDbType = SqlDbType.BigInt
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@userID",
                Value = model.DealUserID,
                SqlDbType = SqlDbType.Int
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@DealInfo",
                Value = string.IsNullOrWhiteSpace(model.DealInfo) ? (object)DBNull.Value : model.DealInfo.Trim(),
                SqlDbType = SqlDbType.NVarChar,
                Size = 200
            });


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

    }
}

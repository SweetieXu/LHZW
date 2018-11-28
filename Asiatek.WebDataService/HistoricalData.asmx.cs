using Asiatek.Common;
using Asiatek.DBUtility;
using Asiatek.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Asiatek.WebDataService
{
    /// <summary>
    /// HistoricalData 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class HistoricalData : System.Web.Services.WebService
    {
        /// <summary>
        /// 根据车辆ID、开始时间、结束时间，查询该车辆期间内的历史信号点
        /// <para>返回结构</para>
        /// <para>车牌号 [PlateNum] [nvarchar](10)</para>
        /// <para>车号 [VehicleName] [nvarchar](20)</para>
        /// <para>时间 [SignalDateTime] [datetime]</para>
        /// <para>速度 [Speed] [float]</para>
        /// <para>方向 [Direction] [smallint]</para>
        /// <para>里程 [Mileage] [float]</para>
        /// <para>ACC状态 [ACCState] [bit]</para>
        /// <para>是否盲区 [IsBlind] [bit]</para>
        /// <para>是否定位 [PositioningState] [bit]</para>
        /// <para>油位 [OilHeight] [float]</para>
        /// <para>滚筒 [RollerState] [tinyint]</para>
        /// <para>温度 [Temperature] [varchar](20)</para>
        /// <para>地址 [Address] [varchar](50)</para>
        /// <para>维度 [Latitude] [float]</para>
        /// <para>经度 [Longitude] [float]</para>
        /// </summary>
        /// <param name="VehicleID">车辆ID</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <returns>json</returns>
        [WebMethod]
        public string GetHistorySignals(int VehicleID, DateTime StartDateTime, DateTime EndDateTime)
        {
            string JsonString = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strProc = "Proc_GetVehicleHisSignal";
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@VehicleID",SqlDbType.Int),
                    new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                    new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                };
                paras[0].Value = VehicleID;
                paras[1].Value = StartDateTime;
                paras[2].Value = EndDateTime;

                dt = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, strProc, paras.ToArray());
                if (dt.Rows.Count > 0)
                {
                    JsonString = JsonHelper.DataTableToJson(dt);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
            }
            return JsonString;
        }

        /// <summary>
        /// 根据车辆ID or 用户ID、开始时间、结束时间，查询该车辆期间内的所有异常列表
        /// <para>返回结构</para>
        /// <para>使用单位 [StrucName] [nvarchar](200)</para>
        /// <para>车号 [VehicleName] [nvarchar](20)</para>
        /// <para>异常类型 [ExTypeName] [nvarchar](50)</para>
        /// <para>起始时间 [StartDateTime] [datetime]</para>
        /// <para>截止时间 [EndDateTime] [datetime]</para>
        /// <para>时长 [Duration] [varchar](20)</para>
        /// </summary>
        /// <param name="VehicleID">车辆ID</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="UserID">用户ID</param>
        /// <returns>json</returns>
        [WebMethod]
        public string GetAllExceptions(int VehicleID, DateTime StartDateTime, DateTime EndDateTime, int UserID)
        {
            string JsonString = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strProc = "Proc_GetAllExceptions";
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@VehicleID",SqlDbType.Int),
                    new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                    new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                    new SqlParameter("@UserID",SqlDbType.Int),
                };
                paras[0].Value = VehicleID;
                paras[1].Value = StartDateTime;
                paras[2].Value = EndDateTime;
                paras[3].Value = UserID;

                dt = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, strProc, paras.ToArray());
                if (dt.Rows.Count > 0)
                {
                    JsonString = JsonHelper.DataTableToJson(dt);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
            }
            return JsonString;
        }

        /// <summary>
        /// 根据车辆ID or 用户ID、开始时间、结束时间，查询期间内的指定类型的异常列表
        /// <para>返回结构</para>
        /// <para>使用单位 [StrucName] [nvarchar](200)</para>
        /// <para>车号 [VehicleName] [nvarchar](20)</para>
        /// <para>起始时间 [StartDateTime] [datetime]</para>
        /// <para>截止时间 [EndDateTime] [datetime]</para>
        /// <para>时长 [Duration] [varchar](20)</para>
        /// </summary>
        /// <param name="VehicleID">车辆ID</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="ExceptionTypeID">异常类型ID</param>
        /// <param name="UserID">异常类型ID</param>
        /// <returns>json</returns>
        [WebMethod]
        public string GetExceptions(int VehicleID, DateTime StartDateTime, DateTime EndDateTime, int ExceptionTypeID, int UserID)
        {
            string JsonString = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strProc = "Proc_GetExceptions";
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@VehicleID",SqlDbType.Int),
                    new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                    new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                    new SqlParameter("@ExceptionTypeID",SqlDbType.Int),
                    new SqlParameter("@UserID",SqlDbType.Int),
                };
                paras[0].Value = VehicleID;
                paras[1].Value = StartDateTime;
                paras[2].Value = EndDateTime;
                paras[3].Value = ExceptionTypeID;
                paras[4].Value = UserID;

                dt = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, strProc, paras.ToArray());
                if (dt.Rows.Count > 0)
                {
                    JsonString = JsonHelper.DataTableToJson(dt);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
            }
            return JsonString;
        }

        /// <summary>
        /// 根据车辆ID or 用户ID、开始时间、结束时间，查询期间内的指定类型的带处理信息的异常列表
        /// <para>返回结构</para>
        /// <para>使用单位 [StrucName] [nvarchar](200)</para>
        /// <para>车号 [VehicleName] [nvarchar](20)</para>
        /// <para>起始时间 [StartDateTime] [datetime]</para>
        /// <para>截止时间 [EndDateTime] [datetime]</para>
        /// <para>时长 [Duration] [varchar](20)</para>
        /// <para>处理账号 [UserName] [nvarchar](20)</para>
        /// <para>处理人 [DealUser] [nvarchar](20)</para>
        /// <para>处理时间 [DealTime] [datetime]</para>
        /// <para>处理信息 [DealInfo] [nvarchar](200)</para>
        /// </summary>
        /// <param name="VehicleID">车辆ID</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="ExceptionTypeID">异常类型ID</param>
        /// <param name="UserID">用户ID</param>
        /// <param name="DealUser">处理人</param>
        /// <returns>json</returns>
        [WebMethod]
        public string GetExceptionsAndDealInfo(int VehicleID, DateTime StartDateTime, DateTime EndDateTime, int ExceptionTypeID, int UserID, string DealUser)
        {
            string JsonString = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strProc = "Proc_GetExceptionsAndDealInfo";
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@VehicleID",SqlDbType.Int),
                    new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                    new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                    new SqlParameter("@ExceptionTypeID",SqlDbType.Int),
                    new SqlParameter("@UserID",SqlDbType.Int),
                    new SqlParameter("@DealUser",SqlDbType.NVarChar,20),
                };
                paras[0].Value = VehicleID;
                paras[1].Value = StartDateTime;
                paras[2].Value = EndDateTime;
                paras[3].Value = ExceptionTypeID;
                paras[4].Value = UserID;
                paras[5].Value = DealUser;

                dt = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, strProc, paras.ToArray());
                if (dt.Rows.Count > 0)
                {
                    JsonString = JsonHelper.DataTableToJson(dt);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
            }
            return JsonString;
        }

        /// <summary>
        /// 根据车辆ID or 用户ID、开始时间、结束时间，查询期间内的所有设备故障
        /// <para>返回结构</para>
        /// <para>使用单位 [StrucName] [nvarchar](200)</para>
        /// <para>车号 [VehicleName] [nvarchar](20)</para>
        /// <para>异常类型 [ExTypeName] [nvarchar](50)</para>
        /// <para>起始时间 [StartDateTime] [datetime]</para>
        /// <para>截止时间 [EndDateTime] [datetime]</para>
        /// <para>时长 [Duration] [varchar](20)</para>
        /// </summary>
        /// <param name="VehicleID">车辆ID</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="ExceptionTypeID">异常类型ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns>json</returns>
        [WebMethod]
        public string GetVehicleExceptionsForEquipmen(int VehicleID, DateTime StartDateTime, DateTime EndDateTime, int ExceptionTypeID, int UserID)
        {
            string JsonString = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strProc = "Proc_GetExceptionsForEquipment";
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@VehicleID",SqlDbType.Int),
                    new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                    new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                    new SqlParameter("@ExceptionTypeID",SqlDbType.Int),
                    new SqlParameter("@UserID",SqlDbType.Int),
                };
                paras[0].Value = VehicleID;
                paras[1].Value = StartDateTime;
                paras[2].Value = EndDateTime;
                paras[3].Value = ExceptionTypeID;
                paras[4].Value = UserID;

                dt = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, strProc, paras.ToArray());
                if (dt.Rows.Count > 0)
                {
                    JsonString = JsonHelper.DataTableToJson(dt);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
            }
            return JsonString;
        }

        /// <summary>
        /// 根据车辆ID or 用户ID、开始时间、结束时间，查询期间内的所有电源相关异常
        /// <para>返回结构</para>
        /// <para>使用单位 [StrucName] [nvarchar](200)</para>
        /// <para>车号 [VehicleName] [nvarchar](20)</para>
        /// <para>异常类型 [ExTypeName] [nvarchar](50)</para>
        /// <para>起始时间 [StartDateTime] [datetime]</para>
        /// <para>截止时间 [EndDateTime] [datetime]</para>
        /// <para>时长 [Duration] [varchar](20)</para>
        /// </summary>
        /// <param name="VehicleID">车辆ID</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="ExceptionTypeID">异常类型ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns>json</returns>
        [WebMethod]
        public string GetExceptionsForMainPower(int VehicleID, DateTime StartDateTime, DateTime EndDateTime, int ExceptionTypeID, int UserID)
        {
            string JsonString = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strProc = "Proc_GetExceptionsForMainPower";
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@VehicleID",SqlDbType.Int),
                    new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                    new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                    new SqlParameter("@ExceptionTypeID",SqlDbType.Int),
                    new SqlParameter("@UserID",SqlDbType.Int),
                };
                paras[0].Value = VehicleID;
                paras[1].Value = StartDateTime;
                paras[2].Value = EndDateTime;
                paras[3].Value = ExceptionTypeID;
                paras[4].Value = UserID;

                dt = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, strProc, paras.ToArray());
                if (dt.Rows.Count > 0)
                {
                    JsonString = JsonHelper.DataTableToJson(dt);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
            }
            return JsonString;
        }

        /// <summary>
        /// 根据车辆ID or 用户ID、开始时间、结束时间，查询期间内的所有路线相关异常
        /// <para>返回结构</para>
        /// <para>使用单位 [StrucName] [nvarchar](200)</para>
        /// <para>车号 [VehicleName] [nvarchar](20)</para>
        /// <para>路线名称 [LineRegionName] [nvarchar](50)</para>
        /// <para>异常类型 [ExTypeName] [nvarchar](50)</para>
        /// <para>起始时间 [StartDateTime] [datetime]</para>
        /// <para>截止时间 [EndDateTime] [datetime]</para>
        /// <para>时长 [Duration] [varchar](20)</para>
        /// </summary>
        /// <param name="VehicleID">车辆ID</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="ExceptionTypeID">异常类型ID</param>
        /// <param name="LineID">路线ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns>json</returns>
        [WebMethod]
        public string GetExceptionsForMapLine(int VehicleID, DateTime StartDateTime, DateTime EndDateTime, int ExceptionTypeID, int LineID, int UserID)
        {
            string JsonString = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strProc = "Proc_GetExceptionsForMapLine";
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@VehicleID",SqlDbType.Int),
                    new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                    new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                    new SqlParameter("@ExceptionTypeID",SqlDbType.Int),
                    new SqlParameter("@LineID",SqlDbType.Int),
                    new SqlParameter("@UserID",SqlDbType.Int),
                };
                paras[0].Value = VehicleID;
                paras[1].Value = StartDateTime;
                paras[2].Value = EndDateTime;
                paras[3].Value = ExceptionTypeID;
                paras[4].Value = LineID;
                paras[5].Value = UserID;

                dt = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, strProc, paras.ToArray());
                if (dt.Rows.Count > 0)
                {
                    JsonString = JsonHelper.DataTableToJson(dt);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
            }
            return JsonString;
        }

        /// <summary>
        /// 根据车辆ID or 用户ID、开始时间、结束时间，查询期间内的所有区域相关异常
        /// <para>返回结构</para>
        /// <para>使用单位 [StrucName] [nvarchar](200)</para>
        /// <para>车号 [VehicleName] [nvarchar](20)</para>
        /// <para>区域名称 [LineRegionName] [nvarchar](50)</para>
        /// <para>异常类型 [ExTypeName] [nvarchar](50)</para>
        /// <para>起始时间 [StartDateTime] [datetime]</para>
        /// <para>截止时间 [EndDateTime] [datetime]</para>
        /// <para>时长 [Duration] [varchar](20)</para>
        /// </summary>
        /// <param name="VehicleID">车辆ID</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="ExceptionTypeID">异常类型ID</param>
        /// <param name="RegionID">区域ID</param>
        /// <param name="UserID">用户ID</param>
        /// <returns>json</returns>
        [WebMethod]
        public string GetExceptionsForMapRegion(int VehicleID, DateTime StartDateTime, DateTime EndDateTime, int ExceptionTypeID, string RegionID, int UserID)
        {
            string JsonString = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strProc = "Proc_GetExceptionsForMapRegion";
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@VehicleID",SqlDbType.Int),
                    new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                    new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                    new SqlParameter("@ExceptionTypeID",SqlDbType.Int),
                    new SqlParameter("@RegionID",SqlDbType.Int),
                    new SqlParameter("@UserID",SqlDbType.Int),
                };
                paras[0].Value = VehicleID;
                paras[1].Value = StartDateTime;
                paras[2].Value = EndDateTime;
                paras[3].Value = ExceptionTypeID;
                paras[4].Value = RegionID;
                paras[5].Value = UserID;

                dt = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, strProc, paras.ToArray());
                if (dt.Rows.Count > 0)
                {
                    JsonString = JsonHelper.DataTableToJson(dt);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
            }
            return JsonString;
        }

        /// <summary>
        /// 根据车辆ID or 用户ID、开始时间、结束时间，查询期间内的GPS里程
        /// <para>返回结构</para>
        /// <para>使用单位 [StrucName] [nvarchar](200)</para>
        /// <para>车号 [VehicleName] [nvarchar](20)</para>
        /// <para>起始时间 [StartDateTime] [datetime]</para>
        /// <para>截止时间 [EndDateTime] [datetime]</para>
        /// <para>里程 [Distance] [float]</para>
        /// </summary>
        /// <param name="VehicleID">车辆ID</param>
        /// <param name="StartDateTime">开始时间</param>
        /// <param name="EndDateTime">结束时间</param>
        /// <param name="UserID">用户ID</param>
        /// <returns>json</returns>
        [WebMethod]
        public string GetVehicleDistance(int VehicleID, DateTime StartDateTime, DateTime EndDateTime, int UserID)
        {
            string JsonString = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                string strProc = "Proc_GetVehicleDistance";
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@VehicleID",SqlDbType.Int),
                    new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                    new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                    new SqlParameter("@UserID",SqlDbType.Int),
                };
                paras[0].Value = VehicleID;
                paras[1].Value = StartDateTime;
                paras[2].Value = EndDateTime;
                paras[3].Value = UserID;

                dt = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, strProc, paras.ToArray());
                if (dt.Rows.Count > 0)
                {
                    JsonString = JsonHelper.DataTableToJson(dt);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("异常信息：{0}\r\n堆栈信息：{1}", ex.Message, ex.StackTrace);
                LogHelper.DoOtherErrorLog(msg);
            }
            return JsonString;
        }

        public void test()
        {
        }
    }
}

using Asiatek.AjaxPager;
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
using System.Threading;
using System.Threading.Tasks;

namespace Asiatek.BLL.MSSQL
{
    public static class TerminalSettingsBLL
    {
        /// <summary>
        /// 获取指定用户可以操作车辆的列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        public static DataTable GetVehicleList(int userid, string CompanyName, string PlateNumOrTerminalCode)
        {
            string sqlTemlpate = @"SELECT  a.ID AS VehicleID ,
                        TerminalCode ,
                        PlateNum ,
                        StrucID ,
                        b.StrucName
                FROM    dbo.Func_Get808VehiclesListByUserID (@UserID) AS a
                        INNER JOIN dbo.Structures AS b ON b.ID = a.StrucID
                WHERE 1=1 {0}
                ORDER BY b.StrucName;";

            StringBuilder sbConditions = new StringBuilder(250);
            if (CompanyName != null)
                sbConditions.Append(string.Format("AND b.StrucName=\'{0}' ", CompanyName));

            if (PlateNumOrTerminalCode != null)
                sbConditions.Append(string.Format("AND (PlateNum='{0}' OR TerminalCode='{0}')", PlateNumOrTerminalCode));

            string sql = string.Format(sqlTemlpate, sbConditions.ToString());
            DataTable dt = MSSQLHelper.ExecuteDataTable(
                CommandType.Text,
                sql,
                new SqlParameter("@UserID", userid.ToString())
            );

            return dt;
        }
        public static void InsertTerminalOperationsLog(
            int UserID,
            TerminalSettingType SettingType,
            string PlateNum,
            string TerminalCode,
            bool Succeeded,
            string SettingData,
            string Result,
            string IPAddress
        )
        {
            string sql = @"INSERT INTO [dbo].[TerminalSetLogs]
                    ([TerminalCode],[PlateNum],[SetType],[SetInfo],[Succeeded],
                    [Result],[WanIP],[SetUserID],[SetDTime])
            VALUES
                    (@TerminalCode,@PlateNum,@SetType,@SetInfo,@Succeeded ,
                      @Result,@WanIP,@SetUserID,@SetDTime)";

            SqlParameter[] @params = new SqlParameter[]
            {
                new SqlParameter("@TerminalCode", TerminalCode == null ? (object)DBNull.Value : TerminalCode),
                new SqlParameter("@PlateNum", PlateNum == null ? (object)DBNull.Value : PlateNum),
                new SqlParameter("@SetType", (byte)SettingType),
                new SqlParameter("@SetInfo", SettingData == null ? (object)DBNull.Value : SettingData),
                new SqlParameter("@Succeeded", Succeeded),
                new SqlParameter("@Result", Result),
                new SqlParameter("@WanIP", IPAddress),
                new SqlParameter("@SetUserID", UserID.ToString()),
                new SqlParameter("@SetDTime", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
            };

            MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, @params);
        }

        public static Tuple<TerminalSetupLogs_DataModel[], int> QueryTerminalSetupLogs(
            string CompanyName,
            string PlateNumber,
            string TerminalCode,
            TerminalSettingType? SettingType,
            DateTime? StartTime,
            DateTime? EndTime,
            int PageIndex
        )
        {
            StringBuilder conditions = new StringBuilder();
            if (!string.IsNullOrEmpty(PlateNumber))
                conditions.Append("AND log.PlateNum=@PlateNumber ");
            if (!string.IsNullOrEmpty(TerminalCode))
                conditions.Append("AND TerminalCode=@TerminalCode ");
            if (SettingType != null)
                conditions.Append("AND log.SetType=@SettingType ");
            if (StartTime != null && EndTime != null)
            {
                conditions.Append("AND SetDTime BETWEEN @StartTime AND @EndTime");
            }
            else if (StartTime != null && EndTime == null)
            {
                conditions.Append("AND SetDTime>=@StartTime");
            }
            else if (StartTime == null && EndTime != null)
            {
                conditions.Append("AND SetDTime<=@EndTime");
            }

            StringBuilder sqlTemplate = new StringBuilder();
            sqlTemplate.AppendFormat(
                @"DECLARE @RowNumMax BIGINT, @RowNumMin BIGINT

                    SELECT @TotalRecords=COUNT(*)
                    FROM    dbo.TerminalSetLogs AS log
                    INNER JOIN dbo.Vehicles AS vehic ON vehic.PlateNum=log.PlateNum
                    INNER JOIN dbo.Structures AS struct ON struct.ID = vehic.StrucID {0}
                    WHERE 1=1 {1};

                    SET @RowNumMin = ( ( @PageIndex - 1 ) * 10 ) + 1;
                    SET @RowNumMax = @RowNumMin -1+ 10;

                    SELECT * FROM (
                        SELECT  ROW_NUMBER() OVER ( ORDER BY SetDTime ) AS RowNumber ,
                                TerminalCode ,
                                log.PlateNum ,
                                SetType ,
                                SetInfo ,
                                Succeeded ,
                                Result ,
                                WanIP ,
                                users.NickName as UserName ,
                                SetDTime,
                                log.Remark
                        FROM    dbo.TerminalSetLogs AS log
                        INNER JOIN dbo.Vehicles AS vehic ON vehic.PlateNum=log.PlateNum
                        INNER JOIN dbo.Users AS users ON users.ID = log.SetUserID
                        INNER JOIN dbo.Structures AS struct ON struct.ID = vehic.StrucID {0}
                        WHERE 1=1 {1}
                    ) AS a
                    WHERE RowNumber BETWEEN @RowNumMin AND @RowNumMax
                    ORDER BY SetDTime;",
                    string.IsNullOrEmpty(CompanyName) ? "" : "AND struct.StrucName = @CompanyName",
                    conditions.ToString()
                );

            SqlParameter[] Params = new SqlParameter[]
            {
                new SqlParameter("@PlateNumber", string.IsNullOrEmpty(PlateNumber) ? "" : PlateNumber),
                new SqlParameter("@TerminalCode", string.IsNullOrEmpty(TerminalCode) ? "" : TerminalCode),
                new SqlParameter("@SettingType", SettingType == null ? 0 : (byte)SettingType),
                new SqlParameter("@StartTime", StartTime == null ? "" : StartTime.Value.ToString("yyyy-MM-dd hh:mm:ss")),
                new SqlParameter("@EndTime", EndTime == null ? "" : EndTime.Value.ToString("yyyy-MM-dd hh:mm:ss")),
                new SqlParameter("@PageIndex", PageIndex),
                new SqlParameter("@CompanyName", string.IsNullOrEmpty(CompanyName) ? "" : CompanyName),
                new SqlParameter("@TotalRecords", SqlDbType.BigInt)
            };
            Params[Params.Length - 1].Direction = ParameterDirection.Output;

            string sql = sqlTemplate.ToString();

            DataTable dt = MSSQLHelper.ExecuteDataTable(
                CommandType.Text,
                sqlTemplate.ToString(),
                Params
            );

            var datalist = new TerminalSetupLogs_DataModel[dt.Rows.Count];
            for (int i = 0; i != dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                datalist[i] = new TerminalSetupLogs_DataModel()
                {
                    RowNumber = (long)dr["RowNumber"],
                    TerminalCode = dr["TerminalCode"] as string,
                    PlateNum = dr["PlateNum"] as string,
                    SetType = (TerminalSettingType)dr["SetType"],
                    SetInfo = dr["SetInfo"] as string,
                    Succeeded = (bool)dr["Succeeded"],
                    Result = dr["Result"] as string,
                    WanIP = dr["WanIP"] as string,
                    SetUserName = dr["UserName"] as string,
                    SetDTime = (DateTime)dr["SetDTime"],
                    Remark = dr["Remark"] as string
                };
            }
            return new Tuple<TerminalSetupLogs_DataModel[], int>(datalist, (int)(long)Params[Params.Length - 1].Value);
        }

        public static Tuple<MapLineSettingModel[], long> QueryMapLineSettingsByUserID(int UserID, int PageIndex, string[] PlateNum)
        {
            StringBuilder sqlCondition = new StringBuilder();
            SqlParameter[] Params = new SqlParameter[PlateNum.Length + 1];// 参数最后一个为数据记录总数
            for (int i = 0; i < PlateNum.Length - 1; i++)
            {
                string name = string.Format("@Param{0}", i);
                sqlCondition.Append(name + ",");
                Params[i] = new SqlParameter(name, PlateNum[i]);
            }
            if (PlateNum.Length > 0)
            {
                //最后一个PlateNum
                string lastname = string.Format("@Param{0}", (PlateNum.Length - 1));
                sqlCondition.Append(lastname);
                Params[PlateNum.Length - 1] = new SqlParameter(lastname, PlateNum[PlateNum.Length - 1]);
            }

            Params[Params.Length - 1] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
            Params[Params.Length - 1].Direction = ParameterDirection.Output;

            StringBuilder sqlTemplate = new StringBuilder();
            sqlTemplate.AppendFormat(@"SELECT @TotalRecord=COUNT(*)
            FROM dbo.MapLineSettings AS [set]
            INNER JOIN dbo.MapLinesList AS list ON list.ID = [set].LineID AND list.Status<>9
            INNER JOIN dbo.Vehicles AS veh ON veh.ID = [set].VehicleID
            INNER JOIN (SELECT id, StrucName FROM dbo.Func_GetStrucListByUserID({1})) AS struc ON struc.id=list.StrucID
            WHERE veh.PlateNum IN({0}) AND [set].IsDeleted=0;

            DECLARE @RowNumMin BIGINT = ( ( {2} - 1 ) * 5 ) + 1;
            DECLARE @RowNumMax BIGINT = @RowNumMin -1+ 5;

            SELECT * FROM (
                SELECT ROW_NUMBER() OVER (ORDER BY veh.PlateNum,struc.StrucName+'_'+list.LinesName) AS RowNumber,
                    [set].ID,
                    veh.PlateNum,
                    [set].LineID,
                    struc.StrucName+'_'+list.LinesName AS PathName,
                    [set].Property,
                    list.StartTime,
                    list.EndTime
                FROM dbo.MapLineSettings AS [set]
                INNER JOIN dbo.MapLinesList AS list ON list.ID = [set].LineID AND list.Status<>9
                INNER JOIN dbo.Vehicles AS veh ON veh.ID = [set].VehicleID
                INNER JOIN (SELECT id, StrucName FROM dbo.Func_GetStrucListByUserID({1})) AS struc ON struc.id=list.StrucID
                WHERE veh.PlateNum IN({0}) AND [set].IsDeleted=0
            ) as a WHERE RowNumber BETWEEN @RowNumMin AND @RowNumMax;", sqlCondition.ToString(), UserID, PageIndex);


            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sqlTemplate.ToString(), Params);
            var result = new MapLineSettingModel[dt.Rows.Count];
            for (int i = 0; i != dt.Rows.Count; i++)
            {
                var r = dt.Rows[i];
                result[i] = new MapLineSettingModel()
                {
                    RowNumber = (long)r["RowNumber"],
                    Key = (long)r["ID"],
                    PlateNum = r["PlateNum"] as string,
                    PathID = (long)r["PathID"],
                    PathName = r["PathName"] as string,
                    Property = (ushort)r["Property"],
                    StartTime = ((DateTime)r["StartTime"]).ToString("yyyy-MM-dd hh:mm:ss"),
                    EndTime = ((DateTime)r["EndTime"]).ToString("yyyy-MM-dd hh:mm:ss")
                };
            }

            return new Tuple<MapLineSettingModel[], long>(result, (long)Params[Params.Length - 1].Value);
        }

        public static Tuple<MapRegionSettingModel[], long> QueryMapRegionSettingsByUserID(int UserID, int PageIndex, int RegionType, string[] PlateNum)
        {
            if (PageIndex <= 0)
                throw new ArgumentOutOfRangeException("PageIndex");
            else if (RegionType < 1 || RegionType > 3)
                throw new ArgumentException("RegionType");
            else if (PlateNum == null)
                throw new ArgumentNullException("PlateNum");
            else if (PlateNum.Length == 0)
                throw new ArgumentException("PlateNum");

            StringBuilder sqlCondition = new StringBuilder();
            SqlParameter[] Params = new SqlParameter[PlateNum.Length + 1];// 参数最后一个为数据记录总数
            for (int i = 0; i < PlateNum.Length - 1; i++)
            {
                string name = string.Format("@Param{0}", i);
                sqlCondition.Append(name + ",");
                Params[i] = new SqlParameter(name, PlateNum[i]);
            }
            if (PlateNum.Length > 0)
            {
                //最后一个PlateNum
                string lastname = string.Format("@Param{0}", (PlateNum.Length - 1));
                sqlCondition.Append(lastname);
                Params[PlateNum.Length - 1] = new SqlParameter(lastname, PlateNum[PlateNum.Length - 1]);
            }

            Params[Params.Length - 1] = new SqlParameter("@TotalRecord", SqlDbType.BigInt);
            Params[Params.Length - 1].Direction = ParameterDirection.Output;

            StringBuilder sqlTemplate = new StringBuilder();
            sqlTemplate.AppendFormat(@"SELECT @TotalRecord=COUNT(*)
                    FROM dbo.MapRegionSettings AS [set]
                    INNER JOIN dbo.MapRegionsList AS list ON list.ID=[set].RegionID AND list.Status<>9
                    INNER JOIN dbo.Vehicles AS veh ON veh.ID=[set].VehicleID
                    INNER JOIN (SELECT id, StrucName FROM dbo.Func_GetStrucListByUserID({1})) AS struc ON struc.id=list.StrucID
            WHERE veh.PlateNum IN({0}) AND list.RegionsType={2} AND [set].IsDeleted=0;

            DECLARE @RowNumMin BIGINT = ( ( {3} - 1 ) * 5 ) + 1;
            DECLARE @RowNumMax BIGINT = @RowNumMin -1+ 5;

            SELECT * FROM (
                SELECT ROW_NUMBER() OVER (ORDER BY veh.PlateNum,struc.StrucName+'_'+list.RegionsName) AS RowNumber,
                        [set].ID,
                        veh.PlateNum,
                        [set].RegionID AS PathID,
                        struc.StrucName+'_'+list.RegionsName AS PathName,
                        list.RegionsType,
                        [set].Property,
                        list.Periodic,
                        CASE WHEN list.[StartDate] is NULL THEN CONVERT(NVARCHAR(8), list.[StartTime], 114)
                            ELSE CONVERT(NVARCHAR(10), list.[StartDate], 111)+' '+CONVERT(NVARCHAR(8), list.[StartTime], 114) END AS StartTime,
                        CASE WHEN list.[EndDate] is NULL THEN CONVERT(NVARCHAR(8), list.[EndTime], 114)
                            ELSE CONVERT(NVARCHAR(10), list.[EndDate], 111)+' '+CONVERT(NVARCHAR(8), list.[EndTime], 114) END AS EndTime,
                        list.SpeedLimit,
                        list.OverSpeedDuration
                FROM dbo.MapRegionSettings AS [set]
                INNER JOIN dbo.MapRegionsList AS list ON list.ID=[set].RegionID AND list.Status<>9
                INNER JOIN dbo.Vehicles AS veh ON veh.ID=[set].VehicleID
                INNER JOIN (SELECT id, StrucName FROM dbo.Func_GetStrucListByUserID({1})) AS struc ON struc.id=list.StrucID
                WHERE veh.PlateNum IN({0}) AND list.RegionsType={2} AND [set].IsDeleted=0
            ) as a WHERE RowNumber BETWEEN @RowNumMin AND @RowNumMax;", sqlCondition.ToString(), UserID, RegionType, PageIndex);

            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sqlTemplate.ToString(), Params);
            var result = new MapRegionSettingModel[dt.Rows.Count];
            for (int i = 0; i != dt.Rows.Count; i++)
            {
                var r = dt.Rows[i];
                result[i] = new MapRegionSettingModel()
                {
                    RowNumber = (long)r["RowNumber"],
                    Key = (long)r["ID"],
                    PlateNum = r["PlateNum"] as string,
                    PathID = (long)r["PathID"],
                    PathName = r["PathName"] as string,
                    Property = (short)r["Property"],
                    StartTime = r["StartTime"] as string,
                    EndTime = r["EndTime"] as string,
                    RegionsType = (int)r["RegionsType"],
                    Periodic = (bool)r["Periodic"],
                    SpeedLimit = (double)r["SpeedLimit"],
                    OverSpeedDuration = (int)r["OverSpeedDuration"],
                };
            }

            return new Tuple<MapRegionSettingModel[], long>(result, result.Length == 0 ? 5 : (long)Params[Params.Length - 1].Value);
        }

        /// <summary>
        /// 删除区域设置数据记录
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="RelationID">区域设置主键</param>
        /// <returns>删除操作是否成功</returns>
        public static bool DeleteRegionVehicleRelation(int UserID, int RelationID)
        {
            string sql = string.Format(@"IF EXISTS(SELECT [set].ID FROM dbo.MapRegionSettings AS [set]
                INNER JOIN dbo.MapRegionsList AS list ON list.ID=[set].RegionID
                WHERE [set].ID={1} AND [set].IsDeleted=0 AND list.StrucID IN (SELECT id FROM dbo.Func_GetStrucListByUserID({0})))
            BEGIN
                UPDATE dbo.MapRegionSettings SET IsDeleted=1 WHERE ID={1}
                SELECT @Deleted=1
            END", UserID, RelationID);

            SqlParameter param = new SqlParameter("@Deleted", SqlDbType.Bit);
            param.Direction = ParameterDirection.InputOutput;
            param.Value = false;

            MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);

            return (bool)param.Value;
        }

        /// <summary>
        /// 获取指定指定区域设置对应的区域信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="RelationID">区域设置主键</param>
        /// <returns>Tuple(查询结果, 车牌号, 终端号, 区域类型, 区域ID)</returns>
        public static Tuple<bool, string, string, int, long> GetRegionInformation(int UserID, int RelationID)
        {
            string sql = string.Format(@"SELECT TerminalCode,VehicleID,RegionsType AS RegionType,[set].RegionID
                FROM dbo.MapRegionSettings AS [set]
                INNER JOIN dbo.Terminals AS term ON term.ID = [set].TerminalID
                INNER JOIN dbo.MapRegionsList AS list ON list.ID = [set].RegionID
                    AND list.StrucID IN (SELECT id FROM dbo.Func_GetStrucListByUserID({0}))
                    AND list.Status<>9
                WHERE [set].ID={1} AND [set].IsDeleted=0;", UserID, RelationID);

            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql);
            if (dt.Rows.Count == 0)
                return new Tuple<bool, string, string, int, long>(false, null, null, 0, 0);

            string PlateNum = dt.Rows[0]["VehicleID"] as string;
            string TerminalCode = dt.Rows[0]["TerminalCode"] as string;
            int RegionType = (int)dt.Rows[0]["RegionType"];
            long RegionID = (long)dt.Rows[0]["RegionID"];

            return new Tuple<bool, string, string, int, long>(true, PlateNum, TerminalCode, RegionType, RegionID);
        }

        /// <summary>
        /// 获取用户可以操作的所有区域
        /// </summary>
        /// <returns>Tuple(区域ID, 区域名称)</returns>
        public static Tuple<long, string>[] GetAllRegions(int UserID, int RegionType)
        {
            string sql = string.Format(@"SELECT list.ID,
                    [struc].StrucName+'_'+list.RegionsName AS PathName
                FROM dbo.MapRegionsList AS list
                INNER JOIN (SELECT id, StrucName FROM dbo.Func_GetStrucListByUserID({0})) AS struc ON struc.id = list.StrucID
                WHERE list.RegionsType={1} AND list.Status<>9", UserID, RegionType);

            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql);

            var array = new Tuple<long, string>[dt.Rows.Count];
            for (int i = 0; i != dt.Rows.Count; i++)
                array[i] = new Tuple<long, string>((long)dt.Rows[i]["ID"], dt.Rows[i]["PathName"] as string);

            return array;
        }

        /// <summary>
        /// 获取指定指定路线设置对应的路线信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="RelationID">路线设置主键</param>
        /// <returns>Tuple(查询结果, 车牌号, 终端号, 路线类型, 路线ID)</returns>
        public static Tuple<bool, string, string, int, long> GetLineInformation(int UserID, int RelationID)
        {
            string sql = string.Format(@"SELECT TerminalCode, LinesType AS LineType,[set].LineID,VehicleID AS PlateNum
                FROM dbo.MapLineSettings AS [set]
                INNER JOIN dbo.Terminals AS term ON term.ID = [set].TerminalID
                INNER JOIN dbo.MapLinesList AS list ON list.ID = [set].RegionID
                    AND list.StrucID IN (SELECT id FROM dbo.Func_GetStrucListByUserID({0}))
                    AND list.Status<>9
                WHERE [set].ID={1} AND [set].IsDeleted=0;", UserID, RelationID);

            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql);
            if (dt == null || dt.Rows.Count == 0)
                return new Tuple<bool, string, string, int, long>(false, null, null, 0, 0);

            string PlateNum = dt.Rows[0]["PlateNum"] as string;
            string TerminalCode = dt.Rows[0]["TerminalCode"] as string;
            int RegionType = (int)dt.Rows[0]["LineType"];
            long RegionID = (long)dt.Rows[0]["LineID"];

            return new Tuple<bool, string, string, int, long>(true, PlateNum, TerminalCode, RegionType, RegionID);
        }

        /// <summary>
        /// 删除路线设置数据记录
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="RelationID">路线设置主键</param>
        /// <returns>删除操作是否成功</returns>
        public static bool DeleteLineVehicleRelation(int UserID, int RelationID)
        {
            string sql = string.Format(@"IF EXISTS(SELECT [set].ID FROM dbo.MapLineSettings AS [set]
                INNER JOIN dbo.MapLinesList AS list ON list.ID = [set].RegionID
                WHERE [set].ID={1} AND list.StrucID IN (SELECT id FROM dbo.Func_GetStrucListByUserID({0})))
            BEGIN
                UPDATE dbo.MapLineSettings SET IsDeleted=1 WHERE ID={1}
                SELECT @Deleted=1
            END", UserID, RelationID);

            SqlParameter param = new SqlParameter("@Deleted", SqlDbType.Bit);
            param.Direction = ParameterDirection.InputOutput;
            param.Value = false;

            MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, param);

            return (bool)param.Value;
        }

        /// <summary>
        /// 获取用户可以操作的所有路线
        /// </summary>
        /// <returns>Tuple(路线ID, 路线名称)</returns>
        public static Tuple<long, string>[] GetAllLines(int UserID, int LineType)
        {
            string sql = string.Format(@"SELECT list.ID,
                    [struc].StrucName+'_'+list.LinesName AS PathName
                FROM dbo.MapLinesList AS list
                INNER JOIN (SELECT id, StrucName FROM dbo.Func_GetStrucListByUserID({0})) AS struc ON struc.id = list.StrucID
                WHERE list.LinesType={1} AND list.Status<>9", UserID, LineType);

            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql);

            var array = new Tuple<long, string>[dt.Rows.Count];
            for (int i = 0; i != dt.Rows.Count; i++)
                array[i] = new Tuple<long, string>((long)dt.Rows[i]["ID"], dt.Rows[i]["PathName"] as string);

            return array;
        }

        /// <summary>
        /// 根据RegionID查找区域信息
        /// </summary>
        /// <param name="RegionID"></param>
        /// <returns></returns>
        public static List<MapRegionsEditModel> GetRegionsByRegionID(int RegionID)
        {
            string sql = string.Format(@"SELECT MPL.ID,MPL.RegionsType,MPL.RegionsName,
MPL.CenterLatitude,MPL.CenterLongitude,MPL.Radius,
MPL.LeftUpperLatitude,MPL.LeftUpperLongitude,MPL.RightLowerLatitude,MPL.RightLowerLongitude,
                CAST(MPL.StartDate AS CHAR(10)) AS StartDate,
                CAST(MPL.StartTime AS CHAR(8)) AS StartTime,
                CAST(MPL.EndDate AS CHAR(10)) AS EndDate,
                CAST(MPL.EndTime AS CHAR(8)) AS EndTime,
                MPL.Periodic,
                MPL.SpeedLimit,
                MPL.OverSpeedDuration,MPD.Latitude,MPD.Longitude,MPD.OrderID
  FROM dbo.MapRegionsList MPL 
  LEFT JOIN dbo.MapRegionsDetails MPD 
  ON MPL.ID=MPD.RegionsID
  WHERE MPL.Status=0 AND MPL.ID={0}", RegionID);

            List<MapRegionsEditModel> list = ConvertToList<MapRegionsEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));

            return list;
        }

        public static List<MapLinesDetailModel> GetLinesByLineID(int LineID)
        {
            string sql = string.Format(@"SELECT MLL.ID ,MLL.LinesType ,MLL.LinesName ,MLL.StartTime ,MLL.EndTime ,
MLD.OrderID,MLD.Latitude,MLD.Longitude,MLD.RoadWidth,MLD.IsCheckTime,MLD.MaxSecond,
MLD.MinSecond,MLD.IsCheckSpeed,MLD.SpeedLimit,MLD.OverSpeedDuration
      FROM dbo.MapLinesList AS MLL 
      LEFT JOIN dbo.MapLinesDetails AS MLD ON MLL.ID=MLD.LinesID
      WHERE Status=0 AND MLL.ID={0}", LineID);

            List<MapLinesDetailModel> list = ConvertToList<MapLinesDetailModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));

            return list;
        }

        /// <summary>
        /// 找到数据库区域设置表dbo.MapRegionSettings当中是否存在与条件匹配的记录并返回记录主键
        /// </summary>
        /// <param name="PlateNum">车牌号</param>
        /// <param name="TerminalCode">终端号</param>
        /// <param name="RegionID">区域ID</param>
        /// <returns>Tuple(记录是否存在, 记录主键)</returns>
        public static Tuple<bool, int?> RegionVehicleRelationExists(string PlateNum, string TerminalCode, int RegionID)
        {
            string sql = string.Format(@"SELECT @Exists=1,
                    @SettingID=[set.ID]
                FROM dbo.MapRegionSettings AS [set]
                INNER JOIN dbo.MapRegionsList AS list ON list.ID = [set].RegionID
                WHERE [set].RegionID={0} AND [set].VehicleID=(SELECT ID FROM dbo.Vehicles WHERE PlateNum=@PlateNum)
                    AND [set].IsDeleted=0 AND [set].TerminalID=(SELECT ID FROM dbo.Terminals WHERE TerminalCode=@TerminalCode);", RegionID);

            SqlParameter[] Params = new SqlParameter[]
            {
                new SqlParameter("@PlateNum", PlateNum),
                new SqlParameter("@TerminalCode", TerminalCode),
                new SqlParameter("@SettingID", SqlDbType.BigInt),
                new SqlParameter("@Exists", SqlDbType.Bit)
            };
            Params[2].Direction = ParameterDirection.Output;
            Params[3].Direction = ParameterDirection.InputOutput;
            Params[3].Value = false;

            MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, Params);
            return new Tuple<bool, int?>((bool)Params[3].Value, Convert.IsDBNull(Params[2].Value) ? null : (int?)Params[2].Value);
        }

        public static bool UpdateRegionVehicleRelation(int SettingID, ushort RegionProperty)
        {
            string sql = string.Format("UPDATE dbo.MapRegionSettings SET Property={1} WHERE ID={0}", SettingID, RegionProperty);
            return 1 == MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public static bool AddRegionVehicleRelation(string PlateNum, string TerminalCode, int RegionID, ushort RegionProperty)
        {
            string sql = string.Format(@"DECLARE @isExists BIT = 0;
                    SELECT @isExists=1
                    FROM dbo.MapRegionSettings AS [set]
                    INNER JOIN dbo.MapRegionsList AS list ON list.ID = [set].RegionID
                    WHERE [set].RegionID={0} AND [set].VehicleID=(SELECT ID FROM dbo.Vehicles WHERE PlateNum=@PlateNum)
                        AND [set].IsDeleted=0 AND [set].TerminalID=(SELECT ID FROM dbo.Terminals WHERE TerminalCode=@TerminalCode);

                    SELECT @Exists=@isExists;

                    IF (@isExists = 1)
                    BEGIN
                        INSERT INTO dbo.MapRegionSettings
                            (TerminalID,VehicleID,RegionID,Property,IsDeleted)
                        VALUES
                            (
                                (SELECT ID FROM dbo.Terminals WHERE TerminalCode=@TerminalCode),
                                (SELECT ID FROM dbo.Vehicles WHERE PlateNum=@PlateNum),
                                {0},
                                {1}
                                0
                            );
                    END", RegionID, RegionProperty);

            var Params = new SqlParameter[]
            {
                new SqlParameter("@TerminalCode", TerminalCode),
                new SqlParameter("@PlateNum", PlateNum),
                new SqlParameter("@Exists", SqlDbType.Bit)
            };
            Params[2].Direction = ParameterDirection.InputOutput;
            Params[2].Value = false;

            return (bool)Params[2].Value;
        }

        /// <summary>
        /// 找到数据库路线设置表dbo.MapLineSettings当中是否存在与条件匹配的记录并返回记录主键
        /// </summary>
        /// <param name="PlateNum">车牌号</param>
        /// <param name="TerminalCode">终端号</param>
        /// <param name="LineID">路线ID</param>
        /// <returns>Tuple(记录是否存在, 记录主键)</returns>
        public static Tuple<bool, int?> LineVehicleRelationExists(string PlateNum, string TerminalCode, int LineID)
        {
            string sql = string.Format(@"SELECT @Exists=1,
                    @SettingID=[set.ID]
                FROM dbo.MapLineSettings AS [set]
                INNER JOIN dbo.MapLinesList AS list ON list.ID = [set].LineID
                WHERE [set].LineID={0} AND [set].VehicleID=(SELECT ID FROM dbo.Vehicles WHERE PlateNum=@PlateNum)
                    AND [set].IsDeleted=0 AND [set].TerminalID=(SELECT ID FROM dbo.Terminals WHERE TerminalCode=@TerminalCode);", LineID);

            SqlParameter[] Params = new SqlParameter[]
            {
                new SqlParameter("@PlateNum", PlateNum),
                new SqlParameter("@TerminalCode", TerminalCode),
                new SqlParameter("@SettingID", SqlDbType.BigInt),
                new SqlParameter("@Exists", SqlDbType.Bit)
            };
            Params[2].Direction = ParameterDirection.Output;
            Params[3].Direction = ParameterDirection.InputOutput;
            Params[3].Value = false;

            MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, Params);
            return new Tuple<bool, int?>((bool)Params[3].Value, Convert.IsDBNull(Params[2].Value) ? null : (int?)Params[2].Value);
        }

        public static bool AddLineVehicleRelation(string PlateNum, string TerminalCode, int LineID, ushort LineProperty)
        {
            string sql = string.Format(@"DECLARE @isExists BIT = 0;
                    SELECT @isExists=1
                    FROM dbo.MapLineSettings AS [set]
                    INNER JOIN dbo.MapLinesList AS list ON list.ID = [set].LineID
                    WHERE [set].LineID={0} AND [set].VehicleID=(SELECT ID FROM dbo.Vehicles WHERE PlateNum=@PlateNum)
                        AND [set].IsDeleted=0 AND [set].TerminalID=(SELECT ID FROM dbo.Terminals WHERE TerminalCode=@TerminalCode);

                    SELECT @Exists=@isExists;

                    IF (@isExists = 1)
                    BEGIN
                        INSERT INTO dbo.MapLineSettings
                            (TerminalID,VehicleID,LineID,Property,IsDeleted)
                        VALUES
                            (
                                (SELECT ID FROM dbo.Terminals WHERE TerminalCode=@TerminalCode),
                                (SELECT ID FROM dbo.Vehicles WHERE PlateNum=@PlateNum),
                                {0},
                                {1}
                                0
                            );
                    END", LineID, LineProperty);

            var Params = new SqlParameter[]
            {
                new SqlParameter("@TerminalCode", TerminalCode),
                new SqlParameter("@PlateNum", PlateNum),
                new SqlParameter("@Exists", SqlDbType.Bit)
            };
            Params[2].Direction = ParameterDirection.InputOutput;
            Params[2].Value = false;

            return (bool)Params[2].Value;
        }


        #region 指定用户可以获取的车辆信息

        #region 自由模式
        /// <summary>
        /// 指定用户可以获取的车辆信息（自由模式）
        /// </summary>
        /// <param name="userid">当前登录用户</param>
        /// <param name="strucName">使用单位</param>
        /// <param name="plateNumOrTerminalCode">车牌号或终端号</param>
        /// <returns></returns>
        public static List<VehiclesTerminalModel> GetVehiclesList(int userid, string strucName, string plateNumOrTerminalCode)
        {
            string sqlTemlpate = @"SELECT  TerminalCode ,PlateNum ,b.StrucName   FROM  
                                                   dbo.Func_Get808VehiclesListByUserID_New (@UserID) AS a
                                                    INNER JOIN dbo.Structures AS b ON b.ID = a.StrucID WHERE 1=1 {0} ORDER BY b.StrucName;";

            StringBuilder sbConditions = new StringBuilder(250);
            if (!string.IsNullOrWhiteSpace(strucName))
            {
                sbConditions.Append(string.Format("AND b.StrucName='{0}' ", strucName));
            }
            if (!string.IsNullOrWhiteSpace(plateNumOrTerminalCode))
            {
                sbConditions.Append(string.Format("AND (PlateNum='{0}' OR TerminalCode='{0}') ", plateNumOrTerminalCode));
            }

            string sql = string.Format(sqlTemlpate, sbConditions.ToString());
            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, new SqlParameter("@UserID", userid.ToString()));
            List<VehiclesTerminalModel> list = ConvertToList<VehiclesTerminalModel>.Convert(dt);
            return list;
        }
        #endregion

        #region 默认模式
        /// <summary>
        /// 指定用户可以获取的车辆信息（默认模式）
        /// </summary>
        /// <param name="strucID">当前登录用户所属单位</param>
        /// <param name="strucName">使用单位</param>
        /// <param name="plateNumOrTerminalCode">车牌号或终端号</param>
        /// <returns></returns>
        public static List<VehiclesTerminalModel> GetDefaultVehiclesList(int strucID, string strucName, string plateNumOrTerminalCode)
        {
            string sqlTemlpate = @"SELECT  TerminalCode ,PlateNum ,b.StrucName   FROM  
                                                   dbo.Func_Get808AllTheSubsetOfVehiclesByStrucID (@StrucID) AS a
                                                    INNER JOIN dbo.Structures AS b ON b.ID = a.StrucID WHERE 1=1 {0} ORDER BY b.StrucName;";

            StringBuilder sbConditions = new StringBuilder(250);
            if (!string.IsNullOrWhiteSpace(strucName))
            {
                sbConditions.Append(string.Format("AND b.StrucName='{0}' ", strucName));
            }
            if (!string.IsNullOrWhiteSpace(plateNumOrTerminalCode))
            {
                sbConditions.Append(string.Format("AND (PlateNum='{0}' OR TerminalCode='{0}') ", plateNumOrTerminalCode));
            }

            string sql = string.Format(sqlTemlpate, sbConditions.ToString());
            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, new SqlParameter("@StrucID", strucID.ToString()));
            List<VehiclesTerminalModel> list = ConvertToList<VehiclesTerminalModel>.Convert(dt);
            return list;
        }
        #endregion

        #endregion

        #region 根据终端号获取终端所属服务器的wcf地址
        /// <summary>
        /// 根据终端号获取终端所属服务器的wcf地址
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<TerminalWCfModel> GetTerminalOfWCFAddress(IEnumerable<string> list)
        {
            string terminals =string.Empty;
            foreach (var item in list)
            {
                terminals += string.Format("'{0}',", item);
            }
            //            string sql = string.Format(@"SELECT a.WCFAddress,b.TerminalCode,v.PlateNum FROM (SELECT ID,ServerInfoID,TerminalCode,
            //                                                           LinkedVehicleID FROM Terminals WHERE TerminalCode IN({0})) AS b LEFT JOIN ServerInfo 
            //                                                           AS a ON a.ID = b.ServerInfoID  LEFT JOIN VehiclesTerminals AS vt ON b.ID = vt.TerminalID
            //                                                            LEFT JOIN Vehicles AS v ON v.ID = vt.VehicleID
            //                                                           ORDER BY a.WCFAddress;", terminals.TrimEnd(','));
            string sql = string.Format(@"SELECT  a.WCFAddress ,b.TerminalCode ,v.PlateNum FROM  ( SELECT  ServerInfoID ,TerminalCode ,
                                                           LinkedVehicleID FROM  Terminals WHERE  TerminalCode IN ({0})) AS b LEFT JOIN ServerInfo 
                                                          AS a ON a.ID = b.ServerInfoID LEFT JOIN Vehicles AS v ON v.ID = b.LinkedVehicleID;", terminals.TrimEnd(','));
            DataTable dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql);
            List<TerminalWCfModel> result = ConvertToList<TerminalWCfModel>.Convert(dt);
            return result;
        }

        /// <summary>
        /// 根据终端号获取WCF地址
        /// </summary>
        /// <param name="terminalCode"></param>
        /// <returns></returns>
        public static string GetTerminalOfWCFAddress(string terminalCode)
        {
            string sql = @"SELECT s.WCFAddress FROM dbo.Terminals t
INNER JOIN dbo.ServerInfo s ON t.ServerInfoID=s.ID
WHERE t.TerminalCode=@TerminalCode";
            SqlParameter para = new SqlParameter()
            {
                ParameterName = "@TerminalCode",
                SqlDbType = SqlDbType.VarChar,
                Size = 20,
                Value = terminalCode.Trim()
            };
            return Convert.ToString(MSSQLHelper.ExecuteScalar(CommandType.Text, sql, para));
        }
        #endregion


        #region 日志记录
        public static void BatchInsertTerminalOperationsLog(string values)
        {
            string sql = string.Format(@"INSERT INTO [dbo].[TerminalSetLogs] ([TerminalCode],[PlateNum],[SetType],[SetInfo],[Succeeded],
                                    [Result],[WanIP],[SetUserID],[SetDTime]) VALUES {0}", values);

            MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql);
        }
        #endregion

        #region   日志查询
        public static AsiatekPagedList<TerminalSettingLogListModel> GetPagedTerminalSettingLog(TerminalSettingLogSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","TerminalSetLogs AS a"),
                new SqlParameter("@joinStr",@" INNER JOIN Users AS b ON a.SetUserID = b.ID
                                                                      INNER JOIN dbo.Vehicles AS c ON c.PlateNum=a.PlateNum
                                                                      INNER JOIN dbo.Structures AS d ON d.ID = c.StrucID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","a.SetDTime DESC"),
                new SqlParameter("@showColumns",@"a.ID,a.[TerminalCode] ,a.[PlateNum],d.StrucName
                                                                              ,a.[SetType],a.[SetDTime] ,b.NickName AS SetUserName"),
            };

            string conditionStr = "1 = 1 ";
            if (!string.IsNullOrWhiteSpace(model.StrucName))
            {
                conditionStr += " AND d.StrucName LIKE '%" + model.StrucName + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.PlateNum))
            {
                conditionStr += " AND a.PlateNum LIKE '%" + model.PlateNum + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.TerminalCode))
            {
                conditionStr += " AND a.TerminalCode LIKE '%" + model.TerminalCode + "%'";
            }
            if (model.SettingType.HasValue)
            {
                conditionStr += " AND a.SetType = " + (byte)(TerminalSettingTypeEnum)model.SettingType;
            }
            if (model.StartTime.HasValue && model.EndTime.HasValue)
            {
                model.EndTime = Convert.ToDateTime(model.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                conditionStr += " AND SetDTime BETWEEN  '" + model.StartTime.Value + "' AND '" + model.EndTime.Value + "'";
            }
            else if (model.StartTime.HasValue && !model.EndTime.HasValue)
            {
                conditionStr += " AND SetDTime >= '" + model.StartTime.Value + "'";
            }
            else if (!model.StartTime.HasValue && model.EndTime.HasValue)
            {
                model.EndTime = Convert.ToDateTime(model.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                conditionStr += " AND SetDTime <= '" + model.EndTime.Value + "'";
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
            List<TerminalSettingLogListModel> list = ConvertToList<TerminalSettingLogListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region 日志详情
        public static SelectResult<TerminalSettingLogDetailModel> GetTerminalSettingLogDetailByID(long id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt)
            };
            paras[0].Value = id;

            string sql = @" SELECT TerminalCode,PlateNum,SetType,SetInfo,Succeeded,Result,WanIP,b.NickName AS SetUserName,SetDTime FROM  
                                     TerminalSetLogs AS a INNER JOIN dbo.Users AS b ON a.SetUserID = b.ID WHERE a.ID = @ID";
            List<TerminalSettingLogDetailModel> list = ConvertToList<TerminalSettingLogDetailModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            TerminalSettingLogDetailModel data = null;
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
            return new SelectResult<TerminalSettingLogDetailModel>()
            {
                DataResult = data,
                Message = msg
            };
        }
        #endregion

    }
}

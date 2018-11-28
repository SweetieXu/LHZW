using Asiatek.Components.GIS;
using Asiatek.DBUtility;
using Asiatek.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Asiatek.BLL.MSSQL
{
    public class SignalBLL
    {
        #region 根据用户ID获取用户分配的车辆的最新信号
        #region 自由模式
        /// <summary>
        /// 根据用户ID获取用户分配的车辆的最新信号 自由模式
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<RealTimeSignalTreeModel> GetRealTimeSignals(int userID)
        {
            #region 获取服务器名
            List<ServerInfoModel> linkName_list = ReportBLL.GetServerInfo(userID, 0);
            //            string linkName_sql = @" SELECT DISTINCT sv.LinkedServerName FROM dbo.Vehicles ve 
            //  INNER JOIN dbo.Terminals tt ON tt.LinkedVehicleID = ve.ID 
            //  INNER JOIN dbo.ServerInfo sv ON sv.ID = tt.ServerInfoID 
            //  WHERE ve.Status<>9 ";
            //            var linkName_list = ConvertToList<GetLinkedServerName>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, linkName_sql));
            if (linkName_list == null || linkName_list.Count == 0)
            {
                return null;
            }
            #endregion
            string sql = "";
            string linkedServerName = "";
            //SqlParameter[] paras = new SqlParameter[2*linkName_list.Count];
            for (int i = 0; i < linkName_list.Count; i++)
            {
                linkedServerName = linkName_list[i].LinkedServerName;
//                sql += string.Format(@"SELECT outerTB.ID AS VID,outerTB.VehicleName AS VN,outerTB.StrucID AS SID,outerTB.StrucName AS SN,outerTB.VIN,
//                        outerTB.ParentID AS SPID,CASE WHEN DATEDIFF(MINUTE,sg.SignalDateTime,GETDATE())>10 THEN 0
//                        ELSE 1 END AS IsOnline,CASE WHEN sg.Speed >0 THEN 1 ELSE 0 END AS IsRunning,
//                        sg.Latitude AS Latitude,sg.Longitude AS Longitude FROM {0}.GNSS.dbo.Signals  sg WITH(NOLOCK)
//             INNER JOIN
//             (
//                        SELECT innerTB.*,s.StrucName,s.ParentID,t.TerminalCode FROM dbo.Structures s WITH(NOLOCK)
//                        INNER JOIN
//                         (
//				            SELECT v.ID,v.VehicleName,v.StrucID,v.VIN FROM dbo.Vehicles v WITH(NOLOCK) INNER JOIN
//				            (
//				            SELECT StrucID FROM dbo.StructureDistributionInfo WITH(NOLOCK)
//				            WHERE UserID={1}) AS temp1 ON v.StrucID=temp1.StrucID
//				            WHERE v.Status=0 AND v.IsReceived=1
//				            UNION
//				            SELECT v.ID,v.VehicleName,v.StrucID,v.VIN FROM dbo.Vehicles v WITH(NOLOCK) INNER JOIN
//				            (
//				            SELECT VehicleID FROM dbo.VehicleDistributionInfo  WITH(NOLOCK)
//				            WHERE UserID={1}) AS temp1 ON v.ID=temp1.VehicleID
//				            WHERE v.Status=0 AND v.IsReceived=1
//                        ) AS innerTB ON s.ID=innerTB.StrucID
//                       INNER JOIN dbo.Terminals t  WITH(NOLOCK) ON innerTB.ID=t.LinkedVehicleID 
//                       INNER JOIN dbo.ServerInfo sv ON sv.ID = t.ServerInfoID 
//                       WHERE sv.LinkedServerName= '{0}'
//              )AS outerTB 
//              ON sg.VIN=outerTB.VIN ", linkedServerName, userID);
                sql += string.Format(@"SELECT outerTB.VID,outerTB.VehicleName AS VN,outerTB.StrucID AS SID,outerTB.StrucName AS SN,outerTB.VIN,
                                                        outerTB.ParentID AS SPID,CASE WHEN DATEDIFF(MINUTE,sg.SignalDateTime,GETDATE())>10 THEN 0
                                                        ELSE 1 END AS IsOnline,CASE WHEN sg.Speed >0 THEN 1 ELSE 0 END AS IsRunning,
                                                        sg.Latitude AS Latitude,sg.Longitude AS Longitude FROM {0}.GNSS.dbo.Signals  sg WITH(NOLOCK)
                                                        INNER JOIN 
                                                        (
                                                            SELECT innerTB.*,s.StrucName,s.ParentID,t.TerminalCode FROM dbo.Structures s WITH(NOLOCK)
                                                            INNER JOIN Func_GetVehiclesListByUserID_New({1}) AS innerTB ON s.ID=innerTB.StrucID
                                                           INNER JOIN dbo.Terminals t  WITH(NOLOCK) ON innerTB.VID=t.LinkedVehicleID 
                                                           INNER JOIN dbo.ServerInfo sv ON sv.ID = t.ServerInfoID 
                                                           WHERE sv.LinkedServerName= '{0}'
                                                      ) AS outerTB ON sg.VIN=outerTB.VIN ", linkedServerName, userID);
                if (i != linkName_list.Count - 1)
                {
                    sql += "  UNION  ";
                }
                //paras[2*i] = new SqlParameter()
                //{
                //    ParameterName = "@userID",
                //    Value = userID,
                //    SqlDbType = SqlDbType.Int
                //};
                //paras[2 * i + 1] = new SqlParameter()
                //{
                //    ParameterName = "@LinkedServerName",
                //    Value = linkedServerName,
                //    SqlDbType = SqlDbType.VarChar
                //};
            }
            return ConvertToList<RealTimeSignalTreeModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion

        #region 默认模式
        /// <summary>
        /// 根据用户ID获取用户分配的车辆的最新信号   默认模式
        /// </summary>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static List<RealTimeSignalTreeModel> GetDefaultRealTimeSignals(int strucID)
        {
            #region 获取服务器名
            List<ServerInfoModel> linkName_list = ReportBLL.GetDefaultServerInfo(strucID, 0);
            if (linkName_list == null || linkName_list.Count == 0)
            {
                return null;
            }
            #endregion
            string sql = "";
            string linkedServerName = "";
            for (int i = 0; i < linkName_list.Count; i++)
            {
                linkedServerName = linkName_list[i].LinkedServerName;
                sql += string.Format(@"SELECT outerTB.VID,outerTB.VehicleName AS VN,outerTB.StrucID AS SID,outerTB.StrucName AS SN,outerTB.VIN,
                                                        outerTB.ParentID AS SPID,CASE WHEN DATEDIFF(MINUTE,sg.SignalDateTime,GETDATE())>10 THEN 0
                                                        ELSE 1 END AS IsOnline,CASE WHEN sg.Speed >0 THEN 1 ELSE 0 END AS IsRunning,
                                                        sg.Latitude AS Latitude,sg.Longitude AS Longitude FROM {0}.GNSS.dbo.Signals  sg WITH(NOLOCK)
                                                        INNER JOIN
                                                         (
                                                                SELECT innerTB.*,s.StrucName,s.ParentID,t.TerminalCode FROM dbo.Structures s WITH(NOLOCK)
                                                                INNER JOIN Func_GetAllTheSubsetOfVehiclesByStrucID({1}) AS innerTB ON s.ID=innerTB.StrucID
                                                               INNER JOIN dbo.Terminals t  WITH(NOLOCK) ON innerTB.VID=t.LinkedVehicleID 
                                                               INNER JOIN dbo.ServerInfo sv ON sv.ID = t.ServerInfoID 
                                                               WHERE sv.LinkedServerName= '{0}'
                                                       ) AS outerTB  ON sg.VIN=outerTB.VIN ", linkedServerName, strucID);
                if (i != linkName_list.Count - 1)
                {
                    sql += "  UNION  ";
                }
            }
            return ConvertToList<RealTimeSignalTreeModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion
        #endregion



        public static RealTimeSignalModel GetRealTimeSignalByVheicleID(long vehicleID)
        {
            string sql = @"SELECT  vw.*,(select top 1 e.EmployeeName from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID where v.Type=1 and v.VehicleID=vw.VID) DriverName,(select top 1 e.ContactPhone from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID where v.Type=1 and v.VehicleID=vw.VID) DriverPhone,(select top 1 e.EmployeeName from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID where v.Type=2 and v.VehicleID=vw.VID) EscortName,(select top 1 e.ContactPhone from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID where v.Type=2 and v.VehicleID=vw.VID) EscortPhone,(SELECT top 1 e.EmployeeName from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID WHERE v.Type=3 and v.VehicleID=vw.VID) OwnersName,(SELECT top 1 e.ContactPhone from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID WHERE v.Type=3 and v.VehicleID=vw.VID) OwnersPhone  FROM  [dbo].[VW_GetRealTimeSignals] vw
WHERE  vw.VID = @vehicleID";
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@vehicleID",
                Value = vehicleID,
                SqlDbType = SqlDbType.BigInt,
            };
            var list = ConvertToList<RealTimeSignalModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
            if (list == null || list.Count == 0)
            {
                return null;
            }
            else
            {
                string points = string.Empty;
                for (int i = 0; i < list.Count; i++)
                {
                    //纠偏
                    PointModel point = NetDecryFixTrue(list[i].Longitude, list[i].Latitude);
                    list[i].Longitude = point.Longitude;
                    list[i].Latitude = point.Latitude;
                }
            }
            return list[0];
        }


        public static List<RealTimeSignalModel> GetRealTimeSignalsByVheicleIDs(IEnumerable<long> vehicleIDs)
        {
            string idstr = string.Empty;
            foreach (var item in vehicleIDs)
            {
                idstr += item + ",";
            }
            idstr = idstr.TrimEnd(',');
            string sql = @"SELECT  vw.*,(select top 1 e.EmployeeName from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID where v.Type=1 and v.VehicleID=vw.VID) DriverName,(select top 1 e.ContactPhone from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID where v.Type=1 and v.VehicleID=vw.VID) DriverPhone,(select top 1 e.EmployeeName from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID where v.Type=2 and v.VehicleID=vw.VID) EscortName,(select top 1 e.ContactPhone from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID where v.Type=2 and v.VehicleID=vw.VID) EscortPhone,(SELECT top 1 e.EmployeeName from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID WHERE v.Type=3 and v.VehicleID=vw.VID) OwnersName,(SELECT top 1 e.ContactPhone from [VehicleEmployeeInfo] v 
inner join [EmployeeInfo] e on v.EmployeeInfoID=e.ID WHERE v.Type=3 and v.VehicleID=vw.VID) OwnersPhone FROM   [dbo].[VW_GetRealTimeSignals] vw WHERE  vw.VID IN (" + idstr + ")";

            var list = ConvertToList<RealTimeSignalModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
            if (list != null && list.Count != 0)
            {
                string points = string.Empty;
                for (int i = 0; i < list.Count; i++)
                {
                    //纠偏
                    PointModel point = NetDecryFixTrue(list[i].Longitude, list[i].Latitude);
                    list[i].Longitude = point.Longitude;
                    list[i].Latitude = point.Latitude;
                }
            }
            return list;
        }

        #region 区域查车  矩形、多边形区域查车

        #region 自由模式
        /// <summary>
        /// 矩形、多边形区域查车（自由模式）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static List<RealTimeSignalModel> GetRealTimeSingalsByRectangle(GetRectangleRealTimeSingalsModel model, int currentUserID)
        {
            //纠偏
            PointModel pointMin = NetDecryFixFalse(model.LngMin, model.LatMin);
            model.LngMin = pointMin.Longitude;
            model.LatMin = pointMin.Latitude;
            PointModel pointMax = NetDecryFixFalse(model.LngMax, model.LatMax);
            model.LngMax = pointMax.Longitude;
            model.LatMax = pointMax.Latitude;
              string sql = @"SELECT sg.* FROM dbo.VW_GetRealTimeSignals sg
                                        INNER JOIN dbo.Func_New_GetVehicleIDByUserId(@userID) vids
                                        ON sg.VID=vids.VehicleID
                                        WHERE sg.Longitude BETWEEN @lngMin AND @lngMax AND sg.Latitude BETWEEN @latMin AND @latMax";

            SqlParameter[] paras = new SqlParameter[5];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@userID",
                Value = currentUserID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@lngMin",
                Value = model.LngMin,
                SqlDbType = SqlDbType.Float
            };
            paras[2] = new SqlParameter()
            {
                ParameterName = "@lngMax",
                Value = model.LngMax,
                SqlDbType = SqlDbType.Float
            };
            paras[3] = new SqlParameter()
            {
                ParameterName = "@latMin",
                Value = model.LatMin,
                SqlDbType = SqlDbType.Float
            };
            paras[4] = new SqlParameter()
            {
                ParameterName = "@latMax",
                Value = model.LatMax,
                SqlDbType = SqlDbType.Float
            };
            return ConvertToList<RealTimeSignalModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 默认模式
         /// <summary>
        /// 矩形、多边形区域查车（默认模式）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static List<RealTimeSignalModel> GetDefaultRealTimeSingalsByRectangle(GetRectangleRealTimeSingalsModel model, int strucID)
        {
            //纠偏
            PointModel pointMin = NetDecryFixFalse(model.LngMin, model.LatMin);
            model.LngMin = pointMin.Longitude;
            model.LatMin = pointMin.Latitude;
            PointModel pointMax = NetDecryFixFalse(model.LngMax, model.LatMax);
            model.LngMax = pointMax.Longitude;
            model.LatMax = pointMax.Latitude;
            string sql = @"SELECT sg.* FROM dbo.VW_GetRealTimeSignals sg
                                    INNER JOIN dbo.Func_GetVehicleIDByStrucID(@StrucID) vids
                                    ON sg.VID=vids.VehicleID
                                    WHERE sg.Longitude BETWEEN @lngMin AND @lngMax AND sg.Latitude BETWEEN @latMin AND @latMax";

            SqlParameter[] paras = new SqlParameter[5];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@lngMin",
                Value = model.LngMin,
                SqlDbType = SqlDbType.Float
            };
            paras[2] = new SqlParameter()
            {
                ParameterName = "@lngMax",
                Value = model.LngMax,
                SqlDbType = SqlDbType.Float
            };
            paras[3] = new SqlParameter()
            {
                ParameterName = "@latMin",
                Value = model.LatMin,
                SqlDbType = SqlDbType.Float
            };
            paras[4] = new SqlParameter()
            {
                ParameterName = "@latMax",
                Value = model.LatMax,
                SqlDbType = SqlDbType.Float
            };
            return ConvertToList<RealTimeSignalModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion
        #endregion

        #region 区域查车 圆形
        #region 默认模式
        /*求已知经纬度两点距离公式
 //EARTH_RADIUS*2*Math.asin(Math.sqrt(Math.pow(Math.sin(a/2),2) + Math.cos(radLat1)*Math.cos(radLat2)*Math.pow(Math.sin(b/2),2)));
 */
        /// <summary>
        /// 圆形区域查车 默认模式
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static List<RealTimeSignalModel> GetDefaultRealTimeSingalsByCircle(GetCircleRealTimeSingalsModel model, int strucID)
        {
            //纠偏
            PointModel point = NetDecryFixFalse(model.Lng, model.Lat);
            model.Lng = point.Longitude;
            model.Lat = point.Latitude;
            //目前用车代号代替
            string sql = @"SELECT sg.* FROM dbo.VW_GetRealTimeSignals sg
                                        INNER JOIN dbo.Func_GetVehicleIDByStrucID(@StrucID) vids
                                        ON sg.VID=vids.VehicleID
                                        WHERE  6378138.0*2*ASIN(SQRT(POWER(SIN((@lat*PI()/180-sg.Latitude*PI()/180)/2),2)+
                                        COS(@lat*PI()/180)*COS(sg.Latitude*PI()/180)*POWER(SIN((@lng*PI()/180-sg.Longitude*PI()/180)/2),2)))<@radius";

            SqlParameter[] paras = new SqlParameter[4];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@lat",
                Value = model.Lat,
                SqlDbType = SqlDbType.Float
            };
            paras[2] = new SqlParameter()
            {
                ParameterName = "@lng",
                Value = model.Lng,
                SqlDbType = SqlDbType.Float
            };
            paras[3] = new SqlParameter()
            {
                ParameterName = "@radius",
                Value = model.Radius,
                SqlDbType = SqlDbType.Float
            };

            return ConvertToList<RealTimeSignalModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 自由模式
        /*求已知经纬度两点距离公式
 //EARTH_RADIUS*2*Math.asin(Math.sqrt(Math.pow(Math.sin(a/2),2) + Math.cos(radLat1)*Math.cos(radLat2)*Math.pow(Math.sin(b/2),2)));
 */
        /// <summary>
        /// 圆形区域查车 自由模式
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static List<RealTimeSignalModel> GetRealTimeSingalsByCircle(GetCircleRealTimeSingalsModel model, int currentUserID)
        {
            //纠偏
            PointModel point = NetDecryFixFalse(model.Lng, model.Lat);
            model.Lng = point.Longitude;
            model.Lat = point.Latitude;
            //目前用车代号代替
            string sql = @"SELECT sg.* FROM dbo.VW_GetRealTimeSignals sg
                                    INNER JOIN dbo.Func_New_GetVehicleIDByUserId(@userID) vids
                                    ON sg.VID=vids.VehicleID
                                    WHERE  6378138.0*2*ASIN(SQRT(POWER(SIN((@lat*PI()/180-sg.Latitude*PI()/180)/2),2)+
                                    COS(@lat*PI()/180)*COS(sg.Latitude*PI()/180)*POWER(SIN((@lng*PI()/180-sg.Longitude*PI()/180)/2),2)))<@radius";

            SqlParameter[] paras = new SqlParameter[4];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@userID",
                Value = currentUserID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@lat",
                Value = model.Lat,
                SqlDbType = SqlDbType.Float
            };
            paras[2] = new SqlParameter()
            {
                ParameterName = "@lng",
                Value = model.Lng,
                SqlDbType = SqlDbType.Float
            };
            paras[3] = new SqlParameter()
            {
                ParameterName = "@radius",
                Value = model.Radius,
                SqlDbType = SqlDbType.Float
            };

            return ConvertToList<RealTimeSignalModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion
        #endregion


        /// <summary>
        /// 将地图坐标转换成与车机一致的
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static PointModel NetDecryFixFalse(double lng, double lat)
        {
            Rectify.Gcj02_To_Wgs84(ref lat, ref lng);
            //NetDecry.Fix(ref lng, ref lat, false);
            PointModel point = new PointModel() { Longitude = lng, Latitude = lat };
            return point;
        }

        /// <summary>
        /// 将车机坐标转换成与地图显示一致的
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static PointModel NetDecryFixTrue(double lng, double lat)
        {
            Rectify.Wgs84_To_Gcj02(ref lat, ref lng);
            //NetDecry.Fix(ref lng, ref lat, true);
            PointModel point = new PointModel() { Longitude = lng, Latitude = lat };
            return point;
        }
       
    }
}

using Asiatek.AjaxPager;
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
using System.Web;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Asiatek.Components.GIS;

namespace Asiatek.BLL.MSSQL
{
    public class HistoricalBLL
    {
        #region  查询  获取历史轨迹信号
        public static List<HistorySignalShowListModel> GetHistoricalInfo(HistorySignalAllInfoModels model)
        {
            //时间间隔
            int invTime = Convert.ToInt32(model.InvTime);
            //过滤零速
            bool isFilter = model.IsSpeed == "true" ? true : false;
            //根据车辆ID查询设备的车机  是否过滤异常信号点
            //bool isGPSFilter = GetVehicleGPSTypeFilter(model.VehicleID);
            //目前没有需要过滤异常信号点的设备，暂时先注释，都设置为false—不需要过滤
            bool isGPSFilter = false;

            //return DoGetSignals(model.VehicleID, model.StartTime, model.EndTime, invTime, isFilter, isGPSFilter);
            //return DoGetSignals(model.VehicleID, model.VIN, model.StartTime, model.EndTime, invTime, isFilter, isGPSFilter);
            return DoGetSignals(model.VehicleID, model.VIN, model.StartTime, model.EndTime, invTime, isFilter, isGPSFilter,model.showStopPoint);
        }
        #endregion


        //#region  根据车辆ID查询  终端类型是否过滤异常信号点
        //public static bool GetVehicleGPSTypeFilter(long vid)
        //{
        //    bool strFilter = false;
        //    try
        //    {
        //        //函数调用
        //        //string sql = @"select * from Func_GetTerminalTypesListByVehicleID('{0}')";
        //        string sql = @"select * from Func_New_GetTerminalTypesListByVehicleID('{0}')";
        //        sql = string.Format(sql, vid);
        //        List<TerminalTypeEditModel> list = ConvertToList<TerminalTypeEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        //        strFilter = Convert.ToBoolean(list[0].Filter);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.DoNormalLog(ex.ToString());
        //    }
        //    return strFilter;
        //}
        //#endregion

        #region  获取历史轨迹信号(按终端号查询)
//        /// <summary>
//        /// 获取历史轨迹信号
//        /// </summary>
//        /// <param name="vid">车辆ID</param>
//        /// <param name="fromtime">开始时间</param>
//        /// <param name="totime">结束时间</param>
//        /// <param name="invTime">时间间隔</param>
//        /// <param name="isFilterVelocity">是否过滤零速</param>
//        /// <param name="isGPSFilter">过滤异常信号点</param>
//        private static List<HistorySignalShowListModel> DoGetSignals(int vid, string fromtime, string totime, int invTime, bool isFilterVelocity, bool isGPSFilter)
//        {
//            List<HistorySignalShowListModel> resultList = new List<HistorySignalShowListModel>();

//            #region   获取历史记录信息
//            //查询历史数据
//            List<SqlParameter> paras = new List<SqlParameter>()
//            {
//                new SqlParameter("@VehicleID",SqlDbType.Int),
//                new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
//                new SqlParameter("@EndDateTime",SqlDbType.DateTime),
//            };
//            paras[0].Value = vid;
//            paras[1].Value = Convert.ToDateTime(fromtime);
//            paras[2].Value = Convert.ToDateTime(totime);
//            //var rs = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetVehicleHisSignal", paras.ToArray());
//            //List<HistorySignalsModel> hsList = ConvertToList<HistorySignalsModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetVehicleHisSignal", paras.ToArray()));
//            List<HistorySignalsModel> hsList = ConvertToList<HistorySignalsModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_New_GetVehicleHisSignal", paras.ToArray()));
////            string hsSQL = @" SELECT VehicleID
////                ,PlateNum
////                ,VehicleName
////                ,TerminalCode
////                ,SignalDateTime
////                ,Speed
////                ,Direction
////                ,Mileage
////                ,ACCState
////                ,IsBlind
////                ,OilHeight
////                ,RollerState
////                ,Temperature
////                ,Address 
////                ,Latitude
////                ,Longitude
////                ,PositioningState FROM dbo.HisSignals 
////                WHERE VehicleID='{0}' AND SignalDateTime BETWEEN '{1}' AND '{2}' 
////                ORDER BY SignalDateTime ASC";
//            //hsSQL = string.Format(hsSQL, vid, fromtime, totime);
//            //List<HistorySignalsModel> hsList = ConvertToList<HistorySignalsModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, hsSQL));
//            #endregion


//            #region  平均时速、最高时速
//            int Anum = 0;  //计数点
//            double AvgSpeed = 0.0;   //平均时速
//            double MaxSpeed = 0.0;   //最高时速
//            int count = hsList.Count;
//            for (int i = 0; i < count; i++)
//            {
//                #region  过滤零速
//                if (isFilterVelocity)
//                {
//                    if (Convert.ToDouble(hsList[i].Speed.ToString()) > 0)
//                    {
//                        AvgSpeed = AvgSpeed + Convert.ToDouble(hsList[i].Speed.ToString());
//                        Anum += 1;
//                    }
//                    if (i == count - 1)
//                    {
//                        AvgSpeed = Math.Round(AvgSpeed / Anum, 1);
//                    }
//                }
//                else
//                {
//                    if (Convert.ToDouble(hsList[i].Speed.ToString()) > 0)
//                    {
//                        AvgSpeed = AvgSpeed + Convert.ToDouble(hsList[i].Speed.ToString());
//                        Anum += 1;
//                    }
//                    else
//                    {
//                        Anum += 1;
//                    }
//                    if (i == count - 1)
//                    {
//                        AvgSpeed = Math.Round(AvgSpeed / Anum, 1);
//                    }
//                }
//                #endregion

//                if (!(MaxSpeed > Convert.ToDouble(hsList[i].Speed.ToString())))
//                {
//                    MaxSpeed = Convert.ToDouble(hsList[i].Speed.ToString());
//                }
//            }
           
//            #endregion

//            List<HistorySignalsModel> tempList = new List<HistorySignalsModel>();

//            double d = Convert.ToDouble("200"); //时速
//            double c = Convert.ToDouble("30"); //速度差
//            double dc = Convert.ToDouble("50"); //两点间距离，米
//            double slat = Convert.ToDouble("18.1");
//            double slng = Convert.ToDouble("73.38");
//            double elat = Convert.ToDouble("53.3");
//            double elng = Convert.ToDouble("135.05");

//            #region   时间间隔 单位s
//            if (invTime > 15)
//            {
//                bool blstart = false;
//                tempList.Clear();
//                string perZerospeedDate = "";
//                #region  过滤零速
//                if (isFilterVelocity)
//                {
//                    for (int i = 0; i < count; i++)
//                    {
//                        try
//                        {
//                            #region  过滤或不过滤 信号异常点
//                            if (isGPSFilter)
//                            {
//                                // 过滤200以上的点
//                                if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
//                                    continue;
//                                // 过滤比前一信号点快30码的点
//                                if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i-1].Speed)) > c)
//                                    continue;
//                                if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
//                                    continue;
//                                if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
//                                    continue;
//                            }
//                            #endregion

//                            #region  盲区补点
//                            if (!hsList[i].IsBlind)
//                            {
//                                if (blstart == false || i == count - 1)
//                                {
//                                    blstart = true;
//                                    tempList.Add(hsList[i]);
//                                    if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) < 5)
//                                    {
//                                        perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
//                                    }
//                                }
//                                else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) < 5 
//                                    && !string.IsNullOrEmpty(hsList[i-1].Speed.ToString()) && Convert.ToDouble(hsList[i-1].Speed) >= 5)
//                                {
//                                    perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
//                                }
//                                else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) >= 5
//                                    && !string.IsNullOrEmpty(hsList[i-1].Speed.ToString()) && Convert.ToDouble(hsList[i-1].Speed) < 5)
//                                {
//                                    DateTime d1 = Convert.ToDateTime(perZerospeedDate);
//                                    DateTime d2 = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString()));
//                                    double minutes = (d2 - d1).TotalMinutes;
//                                    if (minutes > 5)
//                                    {
//                                        tempList.Add(hsList[i - 1]);
//                                    }
//                                    tempList.Add(hsList[i]);
//                                }
//                                else if ((Convert.ToDateTime(hsList[i].SignalDateTime) >= Convert.ToDateTime(tempList[tempList.Count - 1].SignalDateTime).AddSeconds(invTime)) && (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed.ToString()) >= 5))
//                                {
//                                    tempList.Add(hsList[i]);
//                                }
//                            }
//                            #endregion
//                        }
//                        catch (Exception ex)
//                        {
//                            LogHelper.DoNormalLog(ex.ToString());
//                            continue;
//                        }
//                    }
//                }
//                #endregion
//                #region  不过滤零速
//                else
//                {
//                    for (int i = 0; i < count; i++)
//                    {
//                        try
//                        {
//                            #region 过滤或不过滤 信号异常点
//                            if (isGPSFilter)
//                            {
//                                // 过滤200以上的点
//                                if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
//                                    continue;
//                                // 过滤比前一信号点快30码的点
//                                if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
//                                    continue;
//                                if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
//                                    continue;
//                                if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
//                                    continue;
//                            }
//                            #endregion

//                            #region  盲区补点
//                            if (!hsList[i].IsBlind)
//                            {
//                                if (blstart == false || i == count - 1)
//                                {
//                                    blstart = true;
//                                    tempList.Add(hsList[i]);
//                                }
//                                else if (Convert.ToDateTime(hsList[i].SignalDateTime) >= Convert.ToDateTime(tempList[tempList.Count - 1].SignalDateTime).AddSeconds(invTime))
//                                {
//                                    tempList.Add(hsList[i]);
//                                }
//                            }
//                            #endregion
//                        }
//                        catch (Exception ex)
//                        {
//                            LogHelper.DoNormalLog(ex.ToString());
//                            continue;
//                        }
//                    }
//                }
//                #endregion
//            }
//            else
//            {
//                for (int i = 0; i < count; i++)
//                {
//                    try
//                    {
//                        #region  过滤或不过滤
//                        if (isGPSFilter)
//                        {
//                            // 过滤200以上的点
//                            if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
//                                continue;
//                            // 过滤比前一信号点快30码的点
//                            if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
//                                continue;
//                            if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
//                                continue;
//                            if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
//                                continue;
//                        }
//                        if (!hsList[i].IsBlind)
//                        {
//                            tempList.Add(hsList[i]);
//                        }
//                        else
//                        {
//                            Debug.WriteLine(hsList[i].ToString());
//                        }
//                        #endregion
//                    }
//                    catch (Exception ex)
//                    {
//                        LogHelper.DoNormalLog(ex.ToString());
//                        continue;
//                    }
//                }
//            }
//            #endregion


//            #region  经纬度  里程  地图里程
//            double dTotalMapDistance = 0.0;//地图里程 
//            decimal FirstDis = 0.0m;
//            int nn = 0;
//            decimal intDistance = 0.0m; //里程
//            decimal perDis = 0;//上一个点的值，如果为补点，那么里程就为0，所以就为上一个点的值-
            
//            for (int ii = 0; ii < tempList.Count; ii++)
//            {
//                try
//                {
//                    double dMapDistance = 0;
//                    if (ii > 0)
//                    {
//                        dMapDistance = GetDistance(Convert.ToDouble(tempList[ii].Latitude), Convert.ToDouble(tempList[ii].Longitude), Convert.ToDouble(tempList[ii - 1].Latitude), Convert.ToDouble(tempList[ii - 1].Longitude));
//                        dMapDistance = dMapDistance / 1000.000;
//                    }
//                    dTotalMapDistance += Convert.ToDouble(dMapDistance.ToString("f3"));
//                    //里程
//                    if (nn == 0)
//                    {
//                        intDistance = 0;
//                        FirstDis = Convert.ToDecimal(tempList[nn].Mileage);
//                        nn = 1;
//                    }
//                    else
//                    {
//                        if (FirstDis == 0)
//                        {
//                            FirstDis = Convert.ToDecimal(tempList[nn].Mileage);
//                        }
//                        if (intDistance > 0)
//                        {
//                            perDis = intDistance;
//                        }
//                        decimal Cur = string.IsNullOrEmpty(tempList[nn].Mileage.ToString()) ? 0m : Convert.ToDecimal(tempList[nn].Mileage.ToString());
//                        intDistance = Cur - FirstDis;
//                        if (Cur == 0m)//如果补点里程为0，那么置上一个点的值
//                        {
//                            intDistance = perDis;
//                        }
//                        else if (intDistance < -50000m)//否则判断溢出，如果溢出 那么需要加65535
//                        {
//                            intDistance += 65535;
//                        }
//                        nn++;

//                    }

//                    //经纬度
//                    string sAngle = tempList[ii].Direction.ToString();
//                    string sLatitude = tempList[ii].Latitude.ToString();
//                    double lat = double.Parse(sLatitude.Trim());
//                    string sLongitude = tempList[ii].Longitude.ToString();
//                    double lng = double.Parse(sLongitude.Trim());
//                    if (lat < 0.1 || lng < 0.1)
//                    {
//                        continue;
//                    }
//                    NetDecry.Fix(ref lng, ref lat, true);
//                    HistorySignalShowListModel hsmodel = new HistorySignalShowListModel();
//                    hsmodel.AvgSpeed = AvgSpeed.ToString();
//                    hsmodel.MaxSpeed = MaxSpeed.ToString();
//                    hsmodel.VehicleName = tempList[ii].VehicleName; 
//                    hsmodel.VehicleID = vid;
//                    hsmodel.Latitude = lat.ToString();
//                    hsmodel.Longitude = lng.ToString();
//                    hsmodel.Angle = tempList[ii].Direction.ToString();
//                    hsmodel.Mileage = intDistance.ToString();
//                    hsmodel.MapMileage = dTotalMapDistance.ToString();
//                    hsmodel.Speed = tempList[ii].Speed.ToString();
//                    hsmodel.OilMass = tempList[ii].OilHeight.ToString();
//                    hsmodel.Temperature = tempList[ii].Temperature;
//                    hsmodel.Time = tempList[ii].SignalDateTime;
//                    hsmodel.IsBlind = tempList[ii].IsBlind;
//                    hsmodel.Address = tempList[ii].Address;
//                    resultList.Add(hsmodel);
//                }
//                catch (Exception ex)
//                {
//                    LogHelper.DoNormalLog(ex.ToString());
//                    continue;
//                }
//            }
//            #endregion
//            return resultList;
//        }
        #endregion

        #region  获取历史轨迹信号(按车架号查询)
        /// <summary>
        /// 获取历史轨迹信号
        /// </summary>
        /// <param name="vid">车辆ID</param>
        /// <param name="vin">车架号</param>
        /// <param name="fromtime">开始时间</param>
        /// <param name="totime">结束时间</param>
        /// <param name="invTime">时间间隔</param>
        /// <param name="isFilterVelocity">是否过滤零速</param>
        /// <param name="isGPSFilter">过滤异常信号点</param>
        private static List<HistorySignalShowListModel> DoGetSignals(long vid, string vin, string fromtime, string totime, int invTime, bool isFilterVelocity, bool isGPSFilter)
        {
            List<HistorySignalShowListModel> resultList = new List<HistorySignalShowListModel>();

            #region 获取服务器名
            string linkName_sql = @"  SELECT LinkedServerName FROM dbo.ServerInfo WHERE ID in 
  ( SELECT tt.ServerInfoID FROM dbo.Terminals tt INNER JOIN dbo.Vehicles ve ON ve.ID=tt.LinkedVehicleID WHERE ve.VIN=@VIN ) ";
            List<SqlParameter> linkName_paras = new List<SqlParameter>()
            {
                new SqlParameter("@VIN",SqlDbType.Char,17)
            };
            linkName_paras[0].Value = vin;
            var linkName_list = ConvertToList<GetLinkedServerName>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, linkName_sql, linkName_paras.ToArray()));
            if (linkName_list == null || linkName_list.Count == 0)
            {
                return null;
            }
            string linkedServerName = linkName_list[0].LinkedServerName;
            #endregion

            #region   获取历史记录信息
            //查询历史数据
            List<SqlParameter> paras = new List<SqlParameter>()
                    {
                        new SqlParameter("@VIN",SqlDbType.Char,17),
                        new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                        new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                        new SqlParameter("@LinkedServerName",SqlDbType.NVarChar),
                        new SqlParameter("@IsFilter",SqlDbType.Bit),
                    };
            paras[0].Value = vin;
            paras[1].Value = Convert.ToDateTime(fromtime);
            paras[2].Value = Convert.ToDateTime(totime);
            paras[3].Value = linkedServerName;
            paras[4].Value = isFilterVelocity;
            List<HistorySignalsModel> hsList = ConvertToList<HistorySignalsModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetIntervalVINHisSignal", paras.ToArray()));
            #endregion


            #region  平均时速、最高时速
            int Anum = 0;  //计数点
            double AvgSpeed = 0.0;   //平均时速
            double MaxSpeed = 0.0;   //最高时速
            if (hsList == null)
            {
                return null;
            }
            int count = hsList.Count;
            for (int i = 0; i < count; i++)
            {
                #region  过滤零速
                if (isFilterVelocity)
                {
                    if (Convert.ToDouble(hsList[i].Speed.ToString()) > 0)
                    {
                        AvgSpeed = AvgSpeed + Convert.ToDouble(hsList[i].Speed.ToString());
                        Anum += 1;
                    }
                    if (i == count - 1)
                    {
                        AvgSpeed = Math.Round(AvgSpeed / Anum, 1);
                    }
                }
                else
                {
                    if (Convert.ToDouble(hsList[i].Speed.ToString()) > 0)
                    {
                        AvgSpeed = AvgSpeed + Convert.ToDouble(hsList[i].Speed.ToString());
                        Anum += 1;
                    }
                    else
                    {
                        Anum += 1;
                    }
                    if (i == count - 1)
                    {
                        AvgSpeed = Math.Round(AvgSpeed / Anum, 1);
                    }
                }
                #endregion

                if (!(MaxSpeed > Convert.ToDouble(hsList[i].Speed.ToString())))
                {
                    MaxSpeed = Convert.ToDouble(hsList[i].Speed.ToString());
                }
            }

            #endregion

            List<HistorySignalsModel> tempList = new List<HistorySignalsModel>();

            double d = Convert.ToDouble("200"); //时速
            double c = Convert.ToDouble("30"); //速度差
            double dc = Convert.ToDouble("50"); //两点间距离，米
            double slat = Convert.ToDouble("18.1");
            double slng = Convert.ToDouble("73.38");
            double elat = Convert.ToDouble("53.3");
            double elng = Convert.ToDouble("135.05");

            #region   时间间隔 单位s
            if (invTime > 15)
            {
                bool blstart = false;
                tempList.Clear();
                string perZerospeedDate = "";
                #region  过滤零速
                if (isFilterVelocity)
                {
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            #region  过滤或不过滤 信号异常点
                            if (isGPSFilter)
                            {
                                // 过滤200以上的点
                                if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
                                    continue;
                                // 过滤比前一信号点快30码的点
                                if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
                                    continue;
                            }
                            #endregion

                            #region  信号处理
                            if (blstart == false || i == count - 1)
                            {
                                blstart = true;
                                tempList.Add(hsList[i]);
                                if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) < 5)
                                {
                                    perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
                                }
                            }
                            else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) < 5
                                && !string.IsNullOrEmpty(hsList[i - 1].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) >= 5)
                            {
                                perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
                            }
                            else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) >= 5
                                && !string.IsNullOrEmpty(hsList[i - 1].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) < 5)
                            {
                                DateTime d1 = Convert.ToDateTime(perZerospeedDate);
                                DateTime d2 = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString()));
                                double minutes = (d2 - d1).TotalMinutes;
                                if (minutes > 5)
                                {
                                    tempList.Add(hsList[i - 1]);
                                }
                                tempList.Add(hsList[i]);
                            }
                            else if ((Convert.ToDateTime(hsList[i].SignalDateTime) >= Convert.ToDateTime(tempList[tempList.Count - 1].SignalDateTime).AddSeconds(invTime)) && (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed.ToString()) >= 5))
                            {
                                tempList.Add(hsList[i]);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            LogHelper.DoNormalLog(ex.ToString());
                            continue;
                        }
                    }
                }
                #endregion
                #region  不过滤零速
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            #region 过滤或不过滤 信号异常点
                            if (isGPSFilter)
                            {
                                // 过滤200以上的点
                                if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
                                    continue;
                                // 过滤比前一信号点快30码的点
                                if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
                                    continue;
                            }
                            #endregion

                            #region  信号处理
                            if (blstart == false || i == count - 1)
                            {
                                blstart = true;
                                tempList.Add(hsList[i]);
                            }
                            else if (Convert.ToDateTime(hsList[i].SignalDateTime) >= Convert.ToDateTime(tempList[tempList.Count - 1].SignalDateTime).AddSeconds(invTime))
                            {
                                tempList.Add(hsList[i]);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            LogHelper.DoNormalLog(ex.ToString());
                            continue;
                        }
                    }
                }
                #endregion
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        #region  过滤或不过滤
                        if (isGPSFilter)
                        {
                            // 过滤200以上的点
                            if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
                                continue;
                            // 过滤比前一信号点快30码的点
                            if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
                                continue;
                            if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
                                continue;
                            if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
                                continue;
                        }
                        tempList.Add(hsList[i]);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        LogHelper.DoNormalLog(ex.ToString());
                        continue;
                    }
                }
            }
            #endregion


            #region  经纬度  里程  地图里程
            double dTotalMapDistance = 0.0;//地图里程 
            decimal FirstDis = 0.0m;
            int nn = 0;
            decimal intDistance = 0.0m; //里程
            decimal perDis = 0;//上一个点的值，如果为补点，那么里程就为0，所以就为上一个点的值-

            for (int ii = 0; ii < tempList.Count; ii++)
            {
                try
                {
                    double dMapDistance = 0;
                    if (ii > 0)
                    {
                        dMapDistance = GetDistance(Convert.ToDouble(tempList[ii].Latitude), Convert.ToDouble(tempList[ii].Longitude), Convert.ToDouble(tempList[ii - 1].Latitude), Convert.ToDouble(tempList[ii - 1].Longitude));
                        dMapDistance = dMapDistance / 1000.000;
                    }
                    dTotalMapDistance += Convert.ToDouble(dMapDistance.ToString("f3"));
                    //里程
                    if (nn == 0)
                    {
                        intDistance = 0;
                        FirstDis = Convert.ToDecimal(tempList[nn].Mileage);
                        nn = 1;
                    }
                    else
                    {
                        if (FirstDis == 0)
                        {
                            FirstDis = Convert.ToDecimal(tempList[nn].Mileage);
                        }
                        if (intDistance > 0)
                        {
                            perDis = intDistance;
                        }
                        decimal Cur = string.IsNullOrEmpty(tempList[nn].Mileage.ToString()) ? 0m : Convert.ToDecimal(tempList[nn].Mileage.ToString());
                        intDistance = Cur - FirstDis;
                        if (Cur == 0m)//如果补点里程为0，那么置上一个点的值
                        {
                            intDistance = perDis;
                        }
                        else if (intDistance < -50000m)//否则判断溢出，如果溢出 那么需要加65535
                        {
                            intDistance += 65535;
                        }
                        nn++;

                    }

                    //经纬度
                    string sAngle = tempList[ii].Direction.ToString();
                    string sLatitude = tempList[ii].Latitude.ToString();
                    double lat = double.Parse(sLatitude.Trim());
                    string sLongitude = tempList[ii].Longitude.ToString();
                    double lng = double.Parse(sLongitude.Trim());
                    if (lat < 0.1 || lng < 0.1)
                    {
                        continue;
                    }
                    NetDecry.Fix(ref lng, ref lat, true);
                    HistorySignalShowListModel hsmodel = new HistorySignalShowListModel();
                    hsmodel.AvgSpeed = AvgSpeed.ToString();
                    hsmodel.MaxSpeed = MaxSpeed.ToString();
                    hsmodel.VehicleName = tempList[ii].VehicleName;
                    hsmodel.VehicleID = vid;
                    hsmodel.Latitude = lat.ToString();
                    hsmodel.Longitude = lng.ToString();
                    hsmodel.Angle = tempList[ii].Direction.ToString();
                    hsmodel.Mileage = intDistance.ToString();
                    hsmodel.MapMileage = dTotalMapDistance.ToString();
                    hsmodel.Speed = tempList[ii].Speed.ToString();
                    hsmodel.OilMass = tempList[ii].OilHeight.ToString();
                    hsmodel.Temperature = tempList[ii].Temperature;
                    hsmodel.Time = tempList[ii].SignalDateTime;
                    hsmodel.IsBlind = tempList[ii].IsBlind;
                    hsmodel.Address = tempList[ii].Address;
                    hsmodel.ExName = GetExNameByAlarmFlag(tempList[ii].AlarmFlag);
                    hsmodel.ACCState = tempList[ii].ACCState;
                    resultList.Add(hsmodel);
                }
                catch (Exception ex)
                {
                    LogHelper.DoNormalLog(ex.ToString());
                    continue;
                }
            }
            #endregion
            return resultList;
        }
        #endregion

        #region  获取历史轨迹信号(按车架号查询)+是否显示停车点  优化了停车点查询，去掉重复停车点数据
        /// <summary>
        /// 获取历史轨迹信号
        /// </summary>
        /// <param name="vid">车辆ID</param>
        /// <param name="vin">车架号</param>
        /// <param name="fromtime">开始时间</param>
        /// <param name="totime">结束时间</param>
        /// <param name="invTime">时间间隔</param>
        /// <param name="isFilterVelocity">是否过滤零速</param>
        /// <param name="isGPSFilter">过滤异常信号点</param>
        /// <param name="showStopPoint">显示停车点</param>
        private static List<HistorySignalShowListModel> DoGetSignals(long vid, string vin, string fromtime, string totime, int invTime, bool isFilterVelocity, bool isGPSFilter, bool showStopPoint)
        {
            List<HistorySignalShowListModel> resultList = new List<HistorySignalShowListModel>();

            #region 获取服务器名
            string linkName_sql = @"  SELECT LinkedServerName FROM dbo.ServerInfo WHERE ID in 
  ( SELECT tt.ServerInfoID FROM dbo.Terminals tt INNER JOIN dbo.Vehicles ve ON ve.ID=tt.LinkedVehicleID WHERE ve.VIN=@VIN ) ";
            List<SqlParameter> linkName_paras = new List<SqlParameter>()
            {
                new SqlParameter("@VIN",SqlDbType.Char,17)
            };
            linkName_paras[0].Value = vin;
            var linkName_list = ConvertToList<GetLinkedServerName>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, linkName_sql, linkName_paras.ToArray()));
            if (linkName_list == null || linkName_list.Count == 0)
            {
                return null;
            }
            string linkedServerName = linkName_list[0].LinkedServerName;
            #endregion

            #region   获取历史记录信息
            //查询历史数据，存储过程将是否过滤零速度作为参数带入，查询出来的结果集不需要再过滤零速度
            List<SqlParameter> paras = new List<SqlParameter>()
                    {
                        new SqlParameter("@VIN",SqlDbType.Char,17),
                        new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                        new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                        new SqlParameter("@LinkedServerName",SqlDbType.NVarChar),
                        new SqlParameter("@IsFilter",SqlDbType.Bit),
                    };
            paras[0].Value = vin;
            paras[1].Value = Convert.ToDateTime(fromtime);
            paras[2].Value = Convert.ToDateTime(totime);
            paras[3].Value = linkedServerName;
            paras[4].Value = isFilterVelocity;
            List<HistorySignalsModel> hsList = ConvertToList<HistorySignalsModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetIntervalVINHisSignal", paras.ToArray()));
            #endregion

            #region  平均时速、最高时速    目前界面用巡航器显示轨迹，平均时速、最高时速暂时不显示
            //int Anum = 0;  //计数点
            //double AvgSpeed = 0.0;   //平均时速
            //double MaxSpeed = 0.0;   //最高时速
            //if (hsList == null)
            //{
            //    return null;
            //}
            //int count = hsList.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    #region  过滤零速
            //    if (isFilterVelocity)
            //    {
            //        if (Convert.ToDouble(hsList[i].Speed.ToString()) > 0)
            //        {
            //            AvgSpeed = AvgSpeed + Convert.ToDouble(hsList[i].Speed.ToString());
            //            Anum += 1;
            //        }
            //        if (i == count - 1)
            //        {
            //            AvgSpeed = Math.Round(AvgSpeed / Anum, 1);
            //        }
            //    }
            //    else
            //    {
            //        if (Convert.ToDouble(hsList[i].Speed.ToString()) > 0)
            //        {
            //            AvgSpeed = AvgSpeed + Convert.ToDouble(hsList[i].Speed.ToString());
            //            Anum += 1;
            //        }
            //        else
            //        {
            //            Anum += 1;
            //        }
            //        if (i == count - 1)
            //        {
            //            AvgSpeed = Math.Round(AvgSpeed / Anum, 1);
            //        }
            //    }
            //    #endregion

            //    if (!(MaxSpeed > Convert.ToDouble(hsList[i].Speed.ToString())))
            //    {
            //        MaxSpeed = Convert.ToDouble(hsList[i].Speed.ToString());
            //    }
            //}
            #endregion

            int count = hsList.Count;
            List<HistorySignalsModel> tempList = new List<HistorySignalsModel>();
            //遇到0速度，开始添加记录，直到判断停车时长，若大于10minutes，清空；否则将所有记录按照原顺序添加到历史记录中再清空
            List<HistorySignalsModel> stopPointList = new List<HistorySignalsModel>();

            double d = Convert.ToDouble("200"); //时速
            double c = Convert.ToDouble("30"); //速度差
            double dc = Convert.ToDouble("50"); //两点间距离，米
            double slat = Convert.ToDouble("18.1");
            double slng = Convert.ToDouble("73.38");
            double elat = Convert.ToDouble("53.3");
            double elng = Convert.ToDouble("135.05");

            bool blstart = false;
            tempList.Clear();
            string perZerospeedDate = "";

            #region   时间间隔 单位s
            if (invTime > 15)
            {
                #region  不显示停车点
                if (showStopPoint == false)
                {
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            #region 过滤或不过滤 信号异常点
                            if (isGPSFilter)
                            {
                                // 过滤200以上的点
                                if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
                                    continue;
                                // 过滤比前一信号点快30码的点
                                if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
                                    continue;
                            }
                            #endregion

                            #region  信号处理
                            if (blstart == false || i == count - 1)
                            {
                                blstart = true;
                                tempList.Add(hsList[i]);
                            }
                            else if (Convert.ToDateTime(hsList[i].SignalDateTime) >= Convert.ToDateTime(tempList[tempList.Count - 1].SignalDateTime).AddSeconds(invTime))
                            {
                                tempList.Add(hsList[i]);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            LogHelper.DoNormalLog(ex.ToString());
                            continue;
                        }
                    }
                }
                #endregion
                #region  显示停车点
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            #region 过滤或不过滤 信号异常点
                            if (isGPSFilter)
                            {
                                // 过滤200以上的点
                                if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
                                    continue;
                                // 过滤比前一信号点快30码的点
                                if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
                                    continue;
                            }
                            #endregion

                            #region  信号处理
                            if (blstart == false) //第一个点
                            {
                                blstart = true;
                                tempList.Add(hsList[i]);
                                if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) == 0)
                                //速度为0，记录停车开始时间
                                {
                                    perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
                                    stopPointList.Add(hsList[i]);
                                }
                            }
                            else if (i == count - 1)  //最后一个点，只要前一个点速度为0，最后一个点速度是不是为0都算停车时长最后一个点
                            {
                                if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) == 0)
                                {
                                    DateTime d1 = Convert.ToDateTime(perZerospeedDate);
                                    DateTime d2 = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString()));
                                    TimeSpan ts = d2 - d1;
                                    //停车时长大于等于10minutes，记录停车点；否则不记录停车点，之前的0速度记录也要添加
                                    if (ts.TotalMinutes >= 10)
                                    {
                                        hsList[i].IsStopPoint = true;
                                        //前台StopContent要换行显示，所以在要换行的地方添加了"-"
                                        hsList[i].StopContent = @DisplayText.StartTime + ": " + d1 + "-" + @DisplayText.EndTime + ": " + d2 + "-" +
                                        @DisplayText.ActualDuration1 + ": " + ts.Days + @DisplayText.Day + ts.Hours +
                                        @DisplayText.Hour + ts.Minutes + @DisplayText.Minute + ts.Seconds + @DisplayText.Second;
                                        hsList[i].StartStopVehicleTime = d1.ToString();
                                        hsList[i].StopVehicleDuration = ts.Days + @DisplayText.Day + ts.Hours +
                                        @DisplayText.Hour + ts.Minutes + @DisplayText.Minute + ts.Seconds + @DisplayText.Second;
                                        stopPointList = new List<HistorySignalsModel>();
                                    }
                                    else
                                    {
                                        for (int k = 0; k < stopPointList.Count; k++)
                                        {
                                            tempList.Add(stopPointList[k]);   //停车时长若小于10分钟，则将添加到stopPointList的数据再添加回去，最后添加当前记录
                                        }
                                        stopPointList = new List<HistorySignalsModel>();
                                    }
                                }
                                tempList.Add(hsList[i]);
                            }
                            else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) == 0
                            && !string.IsNullOrEmpty(hsList[i - 1].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) > 0)
                            //遇到速度为0的点，记录停车开始时间（记录从0开始）
                            {
                                perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
                                stopPointList.Add(hsList[i]);
                            }
                            else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) > 0
                                && !string.IsNullOrEmpty(hsList[i - 1].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) == 0)
                            //判断停车结束，标记停车点，记录停车信息（记录到0后面一位结束）
                            {
                                DateTime d1 = Convert.ToDateTime(perZerospeedDate);
                                DateTime d2 = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString()));
                                TimeSpan ts = d2 - d1;
                                if (ts.TotalMinutes >= 10)
                                {
                                    hsList[i - 1].IsStopPoint = true;
                                    //前台StopContent要换行显示，所以在要换行的地方添加了"-"
                                    hsList[i - 1].StopContent = @DisplayText.StartTime + ": " + d1 + "-" + @DisplayText.EndTime + ": " + d2 + "-" +
                                        @DisplayText.ActualDuration1 + ": " + ts.Days + @DisplayText.Day + ts.Hours +
                                        @DisplayText.Hour + ts.Minutes + @DisplayText.Minute + ts.Seconds + @DisplayText.Second;
                                    hsList[i - 1].StartStopVehicleTime = d1.ToString();
                                    hsList[i - 1].StopVehicleDuration = ts.Days + @DisplayText.Day + ts.Hours +
                                    @DisplayText.Hour + ts.Minutes + @DisplayText.Minute + ts.Seconds + @DisplayText.Second;
                                    tempList.Add(hsList[i - 1]);
                                    stopPointList = new List<HistorySignalsModel>();
                                }
                                else
                                {
                                    for (int k = 0; k < stopPointList.Count; k++)
                                    {
                                        tempList.Add(stopPointList[k]);
                                    }
                                    stopPointList = new List<HistorySignalsModel>();
                                }
                                tempList.Add(hsList[i]);
                            }
                            else if (Convert.ToDateTime(hsList[i].SignalDateTime) >= Convert.ToDateTime(tempList[tempList.Count - 1].SignalDateTime).AddSeconds(invTime))
                            {
                                if (hsList[i].Speed == 0)
                                {
                                    stopPointList.Add(hsList[i]);
                                }
                                else
                                {
                                    tempList.Add(hsList[i]);
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            LogHelper.DoNormalLog(ex.ToString());
                            continue;
                        }
                    }
                }
                #endregion
            }
            else  //时间间隔15s，不过滤速度
            {
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        #region  过滤或不过滤
                        if (isGPSFilter)
                        {
                            // 过滤200以上的点
                            if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
                                continue;
                            // 过滤比前一信号点快30码的点
                            if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
                                continue;
                            if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
                                continue;
                            if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
                                continue;
                        }
                        #endregion
                        if (showStopPoint == false)   //不显示停车点
                        {
                            tempList.Add(hsList[i]);
                        }
                        else     //显示停车点
                        {
                            if (blstart == false) //第一个点
                            {
                                blstart = true;
                                tempList.Add(hsList[i]);
                                if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) == 0)
                                //速度为0，记录停车开始时间
                                {
                                    perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
                                    stopPointList.Add(hsList[i]);
                                }
                            }
                            else if (i == count - 1)  //最后一个点，只要前一个点速度为0，最后一个点速度是不是为0都算停车时长最后一个点
                            {
                                if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) == 0)
                                {
                                    hsList[i].IsStopPoint = true;
                                    DateTime d1 = Convert.ToDateTime(perZerospeedDate);
                                    DateTime d2 = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString()));
                                    TimeSpan ts = d2 - d1;
                                    //停车时长大于等于10minutes，记录停车点；否则不记录停车点，之前的0速度记录也要添加
                                    if (ts.TotalMinutes >= 10)
                                    {
                                        hsList[i].IsStopPoint = true;
                                        //前台StopContent要换行显示，所以在要换行的地方添加了"-"
                                        hsList[i].StopContent = @DisplayText.StartTime + ": " + d1 + "-" + @DisplayText.EndTime + ": " + d2 + "-" +
                                        @DisplayText.ActualDuration1 + ": " + ts.Days + @DisplayText.Day + ts.Hours +
                                        @DisplayText.Hour + ts.Minutes + @DisplayText.Minute + ts.Seconds + @DisplayText.Second;
                                        hsList[i].StartStopVehicleTime = d1.ToString();
                                        hsList[i].StopVehicleDuration = ts.Days + @DisplayText.Day + ts.Hours +
                                        @DisplayText.Hour + ts.Minutes + @DisplayText.Minute + ts.Seconds + @DisplayText.Second;
                                        stopPointList = new List<HistorySignalsModel>();
                                    }
                                    else
                                    {
                                        for (int k = 0; k < stopPointList.Count; k++)
                                        {
                                            tempList.Add(stopPointList[k]);
                                        }
                                        stopPointList = new List<HistorySignalsModel>();
                                    }
                                }
                                tempList.Add(hsList[i]);
                            }
                            else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) == 0
                            && !string.IsNullOrEmpty(hsList[i - 1].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) > 0)
                            //遇到速度为0的点，记录停车开始时间
                            {
                                perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
                                stopPointList.Add(hsList[i]);
                            }
                            else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) > 0
                                && !string.IsNullOrEmpty(hsList[i - 1].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) == 0)
                            //判断停车结束，标记停车点，记录停车信息
                            {
                                DateTime d1 = Convert.ToDateTime(perZerospeedDate);
                                DateTime d2 = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString()));
                                TimeSpan ts = d2 - d1;
                                if (ts.TotalMinutes >= 10)
                                {
                                    hsList[i - 1].IsStopPoint = true;
                                    //前台StopContent要换行显示，所以在要换行的地方添加了"-"
                                    hsList[i - 1].StopContent = @DisplayText.StartTime + ": " + d1 + "-" + @DisplayText.EndTime + ": " + d2 + "-" +
                                        @DisplayText.ActualDuration1 + ": " + ts.Days + @DisplayText.Day + ts.Hours +
                                        @DisplayText.Hour + ts.Minutes + @DisplayText.Minute + ts.Seconds + @DisplayText.Second;
                                    hsList[i - 1].StartStopVehicleTime = d1.ToString();
                                    hsList[i - 1].StopVehicleDuration = ts.Days + @DisplayText.Day + ts.Hours +
                                    @DisplayText.Hour + ts.Minutes + @DisplayText.Minute + ts.Seconds + @DisplayText.Second;
                                    tempList.Add(hsList[i - 1]);
                                    stopPointList = new List<HistorySignalsModel>();
                                }
                                else
                                {
                                    for (int k = 0; k < stopPointList.Count; k++)
                                    {
                                        tempList.Add(stopPointList[k]);
                                    }
                                    stopPointList = new List<HistorySignalsModel>();
                                }
                                tempList.Add(hsList[i]);
                            }
                            else if (Convert.ToDateTime(hsList[i].SignalDateTime) >= Convert.ToDateTime(tempList[tempList.Count - 1].SignalDateTime).AddSeconds(invTime))
                            {
                                if (hsList[i].Speed == 0)
                                {
                                    stopPointList.Add(hsList[i]);
                                }
                                else
                                {
                                    tempList.Add(hsList[i]);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.DoNormalLog(ex.ToString());
                        continue;
                    }
                }
            }
            #endregion

            #region  经纬度  里程  地图里程
            double dTotalMapDistance = 0.0;//地图里程 
            decimal FirstDis = 0.0m;
            int nn = 0;
            decimal intDistance = 0.0m; //里程
            decimal perDis = 0;//上一个点的值，如果为补点，那么里程就为0，所以就为上一个点的值-

            for (int ii = 0; ii < tempList.Count; ii++)
            {
                try
                {
                    double dMapDistance = 0;
                    if (ii > 0)
                    {
                        dMapDistance = GetDistance(Convert.ToDouble(tempList[ii].Latitude), Convert.ToDouble(tempList[ii].Longitude), Convert.ToDouble(tempList[ii - 1].Latitude), Convert.ToDouble(tempList[ii - 1].Longitude));
                        dMapDistance = dMapDistance / 1000.000;
                    }
                    dTotalMapDistance += Convert.ToDouble(dMapDistance.ToString("f3"));
                    //里程
                    if (nn == 0)
                    {
                        intDistance = 0;
                        FirstDis = Convert.ToDecimal(tempList[nn].Mileage);
                        nn = 1;
                    }
                    else
                    {
                        if (FirstDis == 0)
                        {
                            FirstDis = Convert.ToDecimal(tempList[nn].Mileage);
                        }
                        if (intDistance > 0)
                        {
                            perDis = intDistance;
                        }
                        decimal Cur = string.IsNullOrEmpty(tempList[nn].Mileage.ToString()) ? 0m : Convert.ToDecimal(tempList[nn].Mileage.ToString());
                        intDistance = Cur - FirstDis;
                        if (Cur == 0m)//如果补点里程为0，那么置上一个点的值
                        {
                            intDistance = perDis;
                        }
                        else if (intDistance < -50000m)//否则判断溢出，如果溢出 那么需要加65535
                        {
                            intDistance += 65535;
                        }
                        nn++;

                    }

                    //经纬度
                    string sAngle = tempList[ii].Direction.ToString();
                    string sLatitude = tempList[ii].Latitude.ToString();
                    double lat = double.Parse(sLatitude.Trim());
                    string sLongitude = tempList[ii].Longitude.ToString();
                    double lng = double.Parse(sLongitude.Trim());
                    if (lat < 0.1 || lng < 0.1)
                    {
                        continue;
                    }
                    Rectify.Wgs84_To_Gcj02(ref lat, ref lng);
                    //NetDecry.Fix(ref b, ref a, true);
                    HistorySignalShowListModel hsmodel = new HistorySignalShowListModel();
                    //hsmodel.AvgSpeed = AvgSpeed.ToString();
                    //hsmodel.MaxSpeed = MaxSpeed.ToString();
                    hsmodel.VehicleName = tempList[ii].VehicleName;
                    hsmodel.VehicleID = vid;
                    hsmodel.Latitude = lat.ToString();
                    hsmodel.Longitude = lng.ToString();
                    hsmodel.Angle = tempList[ii].Direction.ToString();
                    hsmodel.Mileage = intDistance.ToString();
                    hsmodel.MapMileage = dTotalMapDistance.ToString();
                    hsmodel.Speed = tempList[ii].Speed.ToString();
                    hsmodel.OilMass = tempList[ii].OilHeight.ToString();
                    hsmodel.Temperature = tempList[ii].Temperature;
                    hsmodel.Time = tempList[ii].SignalDateTime;
                    hsmodel.IsBlind = tempList[ii].IsBlind;
                    hsmodel.Address = tempList[ii].Address;
                    hsmodel.ExName = GetExNameByAlarmFlag(tempList[ii].AlarmFlag);
                    hsmodel.ACCState = tempList[ii].ACCState;
                    hsmodel.IsStopPoint = tempList[ii].IsStopPoint;
                    hsmodel.StopContent = tempList[ii].StopContent;
                    hsmodel.DoorSensorFlag = tempList[ii].DoorsensorFlag;
                    hsmodel.DoorSensor = tempList[ii].DoorsensorFlag == true ? "" + @DisplayText.DoorSensorState + ":" + @DisplayText.Open : "" + @DisplayText.DoorSensorState + ":" + @DisplayText.Close;
                    hsmodel.PressureFlag = tempList[ii].PressureFlag;
                    hsmodel.StartStopVehicleTime = tempList[ii].StartStopVehicleTime;
                    hsmodel.StopVehicleDuration = tempList[ii].StopVehicleDuration;
                    resultList.Add(hsmodel);
                }
                catch (Exception ex)
                {
                    LogHelper.DoNormalLog(ex.ToString());
                    continue;
                }
            }
            #endregion
            return resultList;
        }
        #endregion

        #region  异常类型
        public static string GetExNameByAlarmFlag(long alarmFlag)
        {
            string exName = "";
            if ((alarmFlag & 1) == 1)  //位0  紧急报警
                exName += @DisplayText.Ex_EmergencyAlarm + ',';
            if ((alarmFlag & 2) == 2)  //位1  超速报警
                exName += @DisplayText.Ex_OverSpeedAlarm + ',';
            if ((alarmFlag & 4) == 4)  //位2  疲劳驾驶
                exName += @DisplayText.Ex_FatigueDriving + ',';
            if ((alarmFlag & 16) == 16)  //位4  GNSS模块发生故障
                exName += @DisplayText.Ex_GNSSModuleFailed + ',';
            if ((alarmFlag & 32) == 32)  //位5  GNSS天线未接或被剪断
                exName += @DisplayText.Ex_GNSSAntennasNotConnectedOrCut + ',';
            if ((alarmFlag & 64) == 64)  //位6  GNSS天线短路
                exName += @DisplayText.Ex_GNSSAntennaShorted + ',';
            if ((alarmFlag & 128) == 128)  //位7  终端主电源欠压
                exName += @DisplayText.Ex_TerminalMainsPowerUndervoltage + ',';
            if ((alarmFlag & 256) == 256)  //位8  终端主电源掉电
                exName += @DisplayText.Ex_TerminalMainsPowerDown + ',';
            if ((alarmFlag & 512) == 512)  //位9  终端LCD或显示器故障
                exName += @DisplayText.Ex_TerminalLCDOrMonitorFailure + ',';
            if ((alarmFlag & 1024) == 1024)  //位10  TTS模块故障
                exName += @DisplayText.Ex_TTSModuleFailure + ',';
            if ((alarmFlag & 2048) == 2048)  //位11  摄像头故障
                exName += @DisplayText.Ex_CameraFailure + ',';
            if ((alarmFlag & 4096) == 4096)  //位12  道路运输证IC卡模块故障
                exName += @DisplayText.Ex_RoadTransportICCardModuleFailure + ',';
            if ((alarmFlag & 8192) == 8192)  //位13  超速预警
                exName += @DisplayText.Ex_OverSpeedWarning + ',';
            if ((alarmFlag & 16384) == 16384)  //位14  疲劳驾驶预警
                exName += @DisplayText.Ex_FatigueDrivingWarning + ',';
            if ((alarmFlag & 262144) == 262144)  //位18  当天累计驾驶超时
                exName += @DisplayText.Ex_SameDayDrivingOvertime + ',';
            if ((alarmFlag & 524288) == 524288)  //位19  超时停车
                exName += @DisplayText.Ex_OvertimeParking + ',';
            if (exName != "") {
                exName = exName.Substring(0,exName.Length-1);
            }
            return exName;
        }
        #endregion


        #region  计算两点间距离
        /// <summary>
        /// 计算两点间距离 
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns>以米为单位的长度</returns>
        public static double GetDistance(double latitude1, double longitude1,
                                   double latitude2, double longitude2)
        {
            if (System.Math.Abs(longitude1 - longitude2) > 0.000001 || System.Math.Abs(latitude1 - latitude2) > 0.000001)
            {
                double angLatA = latitude1 / 180 * Math.PI;
                double angLonA = longitude1 / 180 * Math.PI;
                double angLatB = latitude2 / 180 * Math.PI;
                double angLonB = longitude2 / 180 * Math.PI;

                double dDist = System.Math.Sin(angLatA) * System.Math.Sin(angLatB) +
                    System.Math.Cos(angLatA) * System.Math.Cos(angLatB) *
                    System.Math.Cos(angLonB - angLonA);
                //dDist = System.Math.Acos( dDist );
                //dDist *= 69.09 * ( 180 / System.Math.PI ) * 1609.344; //unit: Meter
                dDist = System.Math.Acos(dDist) * 6370693.4856530580439461631130889; //unit: Meter

                return dDist;
            }
            return 0;
        }
        #endregion


        //#region 查询异常轨迹
        //public static List<HistorySignalsModel> GetHistoryExceptionInfo(HistorySignalExportModel model)
        //{
        //    //查询异常数据参数
        //    List<SqlParameter> paras = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@VIN",SqlDbType.Char,17),
        //        new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
        //        new SqlParameter("@EndDateTime",SqlDbType.DateTime),
        //        new SqlParameter("@ExceptionTypeID",SqlDbType.Int),
        //    };
        //    paras[0].Value = model.VIN;
        //    paras[1].Value = model.StartTime;
        //    paras[2].Value = model.EndTime;
        //    paras[3].Value = 0;
        //    //查询历史轨迹参数
        //    List<SqlParameter> hsParas = new List<SqlParameter>()
        //    {
        //        new SqlParameter("@VIN",SqlDbType.Char,17),
        //        new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
        //        new SqlParameter("@EndDateTime",SqlDbType.DateTime),
        //    };
        //    hsParas[0].Value = model.VIN;
        //    hsParas[1].Value = model.StartTime;
        //    hsParas[2].Value = model.EndTime;
        //    List<ExceptionSingalModel> exList = ConvertToList<ExceptionSingalModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_New_GetExceptions", paras.ToArray()));
        //    List<HistorySignalsModel> sigList = ConvertToList<HistorySignalsModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_New_GetIntervalVINHisSignal", hsParas.ToArray()));
        //    List<HistorySignalsModel> list = new List<HistorySignalsModel>();

        //    //信号的时间跟异常表里的开始时间和结束时间匹配
        //    for (int i = 0; i < sigList.Count; i++)
        //    {
        //        for (int j = 0; j < exList.Count; j++)
        //        {
        //            DateTime GPSStartTime = exList[j].SignalStartTime;
        //            DateTime GPSEndTime = exList[j].SignalEndTime;
        //            DateTime SignalDateTime = sigList[i].SignalDateTime;
        //            TimeSpan ts1 = SignalDateTime - GPSStartTime;
        //            TimeSpan ts2 = GPSEndTime - SignalDateTime;
        //            if (ts1.TotalDays >= 0 && ts2.TotalDays >= 0)
        //            {
        //                sigList[i].ExceptionTypeID += exList[j].ExceptionTypeID + ",";
        //                sigList[i].ExName += exList[j].ExName + ",";
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(sigList[i].ExName) && !string.IsNullOrEmpty(sigList[i].ExceptionTypeID))
        //            list.Add(sigList[i]);
        //    }

        //    #region  经纬度  里程  地图里程
        //    List<HistorySignalsModel> resultList = new List<HistorySignalsModel>();
        //    double dTotalMapDistance = 0.0;//地图里程 
        //    decimal FirstDis = 0.0m;
        //    int nn = 0;
        //    decimal intDistance = 0.0m; //里程
        //    decimal perDis = 0;//上一个点的值，如果为补点，那么里程就为0，所以就为上一个点的值-
        //    for (int ii = 0; ii < list.Count; ii++)
        //    {
        //        try
        //        {
        //            double dMapDistance = 0;
        //            if (ii > 0)
        //            {
        //                dMapDistance = GetDistance(Convert.ToDouble(list[ii].Latitude), Convert.ToDouble(list[ii].Longitude), Convert.ToDouble(list[ii - 1].Latitude), Convert.ToDouble(list[ii - 1].Longitude));
        //                dMapDistance = dMapDistance / 1000.000;
        //            }
        //            dTotalMapDistance += Convert.ToDouble(dMapDistance.ToString("f3"));
        //            //里程
        //            if (nn == 0)
        //            {
        //                intDistance = 0;
        //                FirstDis = Convert.ToDecimal(list[nn].Mileage);
        //                nn = 1;
        //            }
        //            else
        //            {
        //                if (FirstDis == 0)
        //                {
        //                    FirstDis = Convert.ToDecimal(list[nn].Mileage);
        //                }
        //                if (intDistance > 0)
        //                {
        //                    perDis = intDistance;
        //                }
        //                decimal Cur = string.IsNullOrEmpty(list[nn].Mileage.ToString()) ? 0m : Convert.ToDecimal(list[nn].Mileage.ToString());
        //                intDistance = Cur - FirstDis;
        //                if (Cur == 0m)//如果补点里程为0，那么置上一个点的值
        //                {
        //                    intDistance = perDis;
        //                }
        //                else if (intDistance < -50000m)//否则判断溢出，如果溢出 那么需要加65535
        //                {
        //                    intDistance += 65535;
        //                }
        //                nn++;

        //            }

        //            //经纬度
        //            string sAngle = list[ii].Direction.ToString();
        //            string sLatitude = list[ii].Latitude.ToString();
        //            double lat = double.Parse(sLatitude.Trim());
        //            string sLongitude = list[ii].Longitude.ToString();
        //            double lng = double.Parse(sLongitude.Trim());
        //            if (lat < 0.1 || lng < 0.1)
        //            {
        //                continue;
        //            }
        //            NetDecry.Fix(ref lng, ref lat, true);
        //            HistorySignalsModel hsmodel = new HistorySignalsModel();
        //            hsmodel.VehicleName = list[ii].VehicleName;
        //            hsmodel.VehicleID = list[ii].VehicleID;
        //            hsmodel.ExName = list[ii].ExName;
        //            hsmodel.Latitude = (float)lat;
        //            hsmodel.Longitude = (float)lng;
        //            hsmodel.Direction = list[ii].Direction;
        //            hsmodel.Mileage = (float)intDistance;
        //            hsmodel.MapMileage = (float)dTotalMapDistance;
        //            hsmodel.Speed = list[ii].Speed;
        //            hsmodel.Time = list[ii].SignalDateTime;
        //            hsmodel.Address = list[ii].Address;
        //            resultList.Add(hsmodel);
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelper.DoNormalLog(ex.ToString());
        //            continue;
        //        }
        //    }
        //    #endregion

        //    string msg = string.Empty;

        //    if (resultList == null)
        //    {
        //        msg = PromptInformation.DBError;
        //    }
        //    else if (resultList.Count == 0)
        //    {
        //        msg = PromptInformation.NotExists;
        //    }
        //    return resultList;
        //}
        //#endregion


        #region 查询超速异常轨迹
        public static List<HistorySignalShowListModel> GetHistoryOverSpeedExceptionInfo(HistorySignalAllInfoModels model)
        {
            List<HistorySignalShowListModel> resultList = new List<HistorySignalShowListModel>();
            //时间间隔
            int invTime = Convert.ToInt32(model.InvTime);
            //过滤零速
            bool isFilterVelocity = model.IsSpeed == "true" ? true : false;
            //根据车辆ID查询设备的车机  是否过滤异常信号点
            //bool isGPSFilter = GetVehicleGPSTypeFilter(model.VehicleID);
            //目前没有需要过滤异常信号点的设备，暂时先注释，都设置为false—不需要过滤
            bool isGPSFilter = false;

            #region 获取服务器名
            string linkName_sql = @"  SELECT LinkedServerName FROM dbo.ServerInfo WHERE ID in 
  ( SELECT tt.ServerInfoID FROM dbo.Terminals tt INNER JOIN dbo.Vehicles ve ON ve.ID=tt.LinkedVehicleID WHERE ve.VIN=@VIN ) ";
            List<SqlParameter> linkName_paras = new List<SqlParameter>()
            {
                new SqlParameter("@VIN",SqlDbType.Char,17)
            };
            linkName_paras[0].Value = model.VIN;
            var linkName_list = ConvertToList<GetLinkedServerName>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, linkName_sql, linkName_paras.ToArray()));
            if (linkName_list == null || linkName_list.Count == 0)
            {
                return null;
            }
            string linkedServerName = linkName_list[0].LinkedServerName;
            #endregion

            #region   获取历史记录信息
            //查询历史数据
            List<SqlParameter> paras = new List<SqlParameter>()
                    {
                        new SqlParameter("@VIN",SqlDbType.Char,17),
                        new SqlParameter("@BeginDateTime",SqlDbType.DateTime),
                        new SqlParameter("@EndDateTime",SqlDbType.DateTime),
                        new SqlParameter("@LinkedServerName",SqlDbType.NVarChar),
                        new SqlParameter("@IsFilter",SqlDbType.Bit),
                    };
            paras[0].Value = model.VIN;
            paras[1].Value = Convert.ToDateTime(model.StartTime);
            paras[2].Value = Convert.ToDateTime(model.EndTime);
            paras[3].Value = linkedServerName;
            paras[4].Value = isFilterVelocity;
            List<HistorySignalsModel> hsList = ConvertToList<HistorySignalsModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetIntervalVINOverSpeedHisSignal", paras.ToArray()));
            #endregion


            #region  平均时速、最高时速
            int Anum = 0;  //计数点
            double AvgSpeed = 0.0;   //平均时速
            double MaxSpeed = 0.0;   //最高时速
            int count = hsList.Count;
            for (int i = 0; i < count; i++)
            {
                #region  过滤零速
                if (isFilterVelocity)
                {
                    if (Convert.ToDouble(hsList[i].Speed.ToString()) > 0)
                    {
                        AvgSpeed = AvgSpeed + Convert.ToDouble(hsList[i].Speed.ToString());
                        Anum += 1;
                    }
                    if (i == count - 1)
                    {
                        AvgSpeed = Math.Round(AvgSpeed / Anum, 1);
                    }
                }
                else
                {
                    if (Convert.ToDouble(hsList[i].Speed.ToString()) > 0)
                    {
                        AvgSpeed = AvgSpeed + Convert.ToDouble(hsList[i].Speed.ToString());
                        Anum += 1;
                    }
                    else
                    {
                        Anum += 1;
                    }
                    if (i == count - 1)
                    {
                        AvgSpeed = Math.Round(AvgSpeed / Anum, 1);
                    }
                }
                #endregion

                if (!(MaxSpeed > Convert.ToDouble(hsList[i].Speed.ToString())))
                {
                    MaxSpeed = Convert.ToDouble(hsList[i].Speed.ToString());
                }
            }

            #endregion

            List<HistorySignalsModel> tempList = new List<HistorySignalsModel>();
            double d = Convert.ToDouble("200"); //时速
            double c = Convert.ToDouble("30"); //速度差
            double dc = Convert.ToDouble("50"); //两点间距离，米
            double slat = Convert.ToDouble("18.1");
            double slng = Convert.ToDouble("73.38");
            double elat = Convert.ToDouble("53.3");
            double elng = Convert.ToDouble("135.05");

            #region   时间间隔 单位s
            if (invTime > 15)
            {
                bool blstart = false;
                tempList.Clear();
                string perZerospeedDate = "";
                #region  过滤零速
                if (isFilterVelocity)
                {
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            #region  过滤或不过滤 信号异常点
                            if (isGPSFilter)
                            {
                                // 过滤200以上的点
                                if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
                                    continue;
                                // 过滤比前一信号点快30码的点
                                if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
                                    continue;
                            }
                            #endregion

                            #region  信号处理
                            if (blstart == false || i == count - 1)
                            {
                                blstart = true;
                                tempList.Add(hsList[i]);
                                if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) < 5)
                                {
                                    perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
                                }
                            }
                            else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) < 5
                                && !string.IsNullOrEmpty(hsList[i - 1].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) >= 5)
                            {
                                perZerospeedDate = string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString());
                            }
                            else if (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed) >= 5
                                && !string.IsNullOrEmpty(hsList[i - 1].Speed.ToString()) && Convert.ToDouble(hsList[i - 1].Speed) < 5)
                            {
                                DateTime d1 = Convert.ToDateTime(perZerospeedDate);
                                DateTime d2 = Convert.ToDateTime(string.Format("{0:yyyy-MM-dd hh:mm:ss}", hsList[i].SignalDateTime.ToString()));
                                double minutes = (d2 - d1).TotalMinutes;
                                if (minutes > 5)
                                {
                                    tempList.Add(hsList[i - 1]);
                                }
                                tempList.Add(hsList[i]);
                            }
                            else if ((Convert.ToDateTime(hsList[i].SignalDateTime) >= Convert.ToDateTime(tempList[tempList.Count - 1].SignalDateTime).AddSeconds(invTime)) && (!string.IsNullOrEmpty(hsList[i].Speed.ToString()) && Convert.ToDouble(hsList[i].Speed.ToString()) >= 5))
                            {
                                tempList.Add(hsList[i]);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            LogHelper.DoNormalLog(ex.ToString());
                            continue;
                        }
                    }
                }
                #endregion
                #region  不过滤零速
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        try
                        {
                            #region 过滤或不过滤 信号异常点
                            if (isGPSFilter)
                            {
                                // 过滤200以上的点
                                if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
                                    continue;
                                // 过滤比前一信号点快30码的点
                                if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
                                    continue;
                                if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
                                    continue;
                            }
                            #endregion

                            #region  信号处理
                            if (blstart == false || i == count - 1)
                            {
                                blstart = true;
                                tempList.Add(hsList[i]);
                            }
                            else if (Convert.ToDateTime(hsList[i].SignalDateTime) >= Convert.ToDateTime(tempList[tempList.Count - 1].SignalDateTime).AddSeconds(invTime))
                            {
                                tempList.Add(hsList[i]);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            LogHelper.DoNormalLog(ex.ToString());
                            continue;
                        }
                    }
                }
                #endregion
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        #region  过滤或不过滤
                        if (isGPSFilter)
                        {
                            // 过滤200以上的点
                            if (i == 0 && Convert.ToDouble(hsList[i].Speed) > d)
                                continue;
                            // 过滤比前一信号点快30码的点
                            if (i > 0 && (Convert.ToDouble(hsList[i].Speed) - Convert.ToDouble(hsList[i - 1].Speed)) > c)
                                continue;
                            if (Convert.ToDouble(hsList[i].Latitude) < slat || Convert.ToDouble(hsList[i].Latitude) > elat)
                                continue;
                            if (Convert.ToDouble(hsList[i].Longitude) < slng || Convert.ToDouble(hsList[i].Longitude) > elng)
                                continue;
                        }
                        tempList.Add(hsList[i]);
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        LogHelper.DoNormalLog(ex.ToString());
                        continue;
                    }
                }
            }
            #endregion


            #region  经纬度  里程  地图里程
            double dTotalMapDistance = 0.0;//地图里程 
            decimal FirstDis = 0.0m;
            int nn = 0;
            decimal intDistance = 0.0m; //里程
            decimal perDis = 0;//上一个点的值，如果为补点，那么里程就为0，所以就为上一个点的值-

            for (int ii = 0; ii < tempList.Count; ii++)
            {
                try
                {
                    double dMapDistance = 0;
                    if (ii > 0)
                    {
                        dMapDistance = GetDistance(Convert.ToDouble(tempList[ii].Latitude), Convert.ToDouble(tempList[ii].Longitude), Convert.ToDouble(tempList[ii - 1].Latitude), Convert.ToDouble(tempList[ii - 1].Longitude));
                        dMapDistance = dMapDistance / 1000.000;
                    }
                    dTotalMapDistance += Convert.ToDouble(dMapDistance.ToString("f3"));
                    //里程
                    if (nn == 0)
                    {
                        intDistance = 0;
                        FirstDis = Convert.ToDecimal(tempList[nn].Mileage);
                        nn = 1;
                    }
                    else
                    {
                        if (FirstDis == 0)
                        {
                            FirstDis = Convert.ToDecimal(tempList[nn].Mileage);
                        }
                        if (intDistance > 0)
                        {
                            perDis = intDistance;
                        }
                        decimal Cur = string.IsNullOrEmpty(tempList[nn].Mileage.ToString()) ? 0m : Convert.ToDecimal(tempList[nn].Mileage.ToString());
                        intDistance = Cur - FirstDis;
                        if (Cur == 0m)//如果补点里程为0，那么置上一个点的值
                        {
                            intDistance = perDis;
                        }
                        else if (intDistance < -50000m)//否则判断溢出，如果溢出 那么需要加65535
                        {
                            intDistance += 65535;
                        }
                        nn++;

                    }

                    //经纬度
                    string sAngle = tempList[ii].Direction.ToString();
                    string sLatitude = tempList[ii].Latitude.ToString();
                    double lat = double.Parse(sLatitude.Trim());
                    string sLongitude = tempList[ii].Longitude.ToString();
                    double lng = double.Parse(sLongitude.Trim());
                    if (lat < 0.1 || lng < 0.1)
                    {
                        continue;
                    }
                    //NetDecry.Fix(ref lng, ref lat, true);
                    Rectify.Wgs84_To_Gcj02(ref lat, ref lng);
                    HistorySignalShowListModel hsmodel = new HistorySignalShowListModel();
                    hsmodel.AvgSpeed = AvgSpeed.ToString();
                    hsmodel.MaxSpeed = MaxSpeed.ToString();
                    hsmodel.VehicleName = tempList[ii].VehicleName;
                    hsmodel.VehicleID = model.VehicleID;
                    hsmodel.Latitude = lat.ToString();
                    hsmodel.Longitude = lng.ToString();
                    hsmodel.Angle = tempList[ii].Direction.ToString();
                    hsmodel.Mileage = intDistance.ToString();
                    hsmodel.MapMileage = dTotalMapDistance.ToString();
                    hsmodel.Speed = tempList[ii].Speed.ToString();
                    hsmodel.OilMass = tempList[ii].OilHeight.ToString();
                    hsmodel.Temperature = tempList[ii].Temperature;
                    hsmodel.Time = tempList[ii].SignalDateTime;
                    hsmodel.IsBlind = tempList[ii].IsBlind;
                    hsmodel.Address = tempList[ii].Address;
                    hsmodel.ExName = GetExNameByAlarmFlag(tempList[ii].AlarmFlag);
                    hsmodel.ACCState = tempList[ii].ACCState;
                    resultList.Add(hsmodel);
                }
                catch (Exception ex)
                {
                    LogHelper.DoNormalLog(ex.ToString());
                    continue;
                }
            }
            #endregion
            return resultList;

            
        }
        #endregion


        #region 显示区域
        public static List<MapRegionsEditModel> GetShowAreaDataWithUserID(int uid)
        {
            string sql = @"  SELECT MPL.ID,MPL.RegionsType,
                MPL.RegionsName,MPL.CenterLatitude,MPL.CenterLongitude,MPL.Radius,
                MPL.LeftUpperLatitude,MPL.LeftUpperLongitude,MPL.RightLowerLatitude,MPL.RightLowerLongitude,
                CAST(MPL.StartDate AS CHAR(10)) AS StartDate,
                CAST(MPL.StartTime AS CHAR(10)) AS StartTime,
                CAST(MPL.EndDate AS CHAR(10)) AS EndDate,
                CAST(MPL.EndTime AS CHAR(10)) AS EndTime,
                MPL.Periodic,
                MPL.SpeedLimit,
                MPL.OverSpeedDuration,MPD.Latitude,MPD.Longitude,MPD.OrderID
  FROM dbo.MapRegionsList MPL 
  LEFT JOIN dbo.MapRegionsDetails MPD 
  ON MPL.ID=MPD.RegionsID
  WHERE MPL.Status=0 AND MPL.StrucID IN (SELECT id FROM Func_GetStrucListByUserID('{0}'))";
            sql = string.Format(sql, uid);
            List<MapRegionsEditModel> list = ConvertToList<MapRegionsEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));

            if (list == null) {
                return null;
            }
            #region 纠偏
            for (int i = 0; i < list.Count; i++) {
                if (list[i].RegionsType == 1 || list[i].RegionsType == 2)
                {
                    double centerLng = double.Parse(list[i].CenterLongitude.ToString());
                    double centerLat = double.Parse(list[i].CenterLatitude.ToString());
                    //NetDecry.Fix(ref centerLng, ref centerLat, true);
                    Rectify.Wgs84_To_Gcj02(ref centerLat, ref centerLng);
                    double leftLng = double.Parse(list[i].LeftUpperLongitude.ToString());
                    double leftLat = double.Parse(list[i].LeftUpperLatitude.ToString());
                    double rightLng = double.Parse(list[i].RightLowerLongitude.ToString());
                    double rightLat = double.Parse(list[i].RightLowerLatitude.ToString());
                    Rectify.Wgs84_To_Gcj02(ref leftLat, ref leftLng);
                    Rectify.Wgs84_To_Gcj02(ref rightLat, ref rightLng);
                    //NetDecry.Fix(ref leftLng, ref leftLat, true);
                    //NetDecry.Fix(ref rightLng, ref rightLat, true);
                    list[i].CenterLongitude = (float)centerLng;
                    list[i].CenterLatitude = (float)centerLat;
                    list[i].LeftUpperLongitude = (float)leftLng;
                    list[i].LeftUpperLatitude = (float)leftLat;
                    list[i].RightLowerLongitude = (float)rightLng;
                    list[i].RightLowerLatitude = (float)rightLat;
                }
                else
                {
                    double lng = double.Parse(list[i].Longitude.ToString());
                    double lat = double.Parse(list[i].Latitude.ToString());
                    //NetDecry.Fix(ref lng, ref lat, true);
                    Rectify.Wgs84_To_Gcj02(ref lat, ref lng);
                    list[i].Longitude = (float)lng;
                    list[i].Latitude = (float)lat;
                }
            }
            #endregion

            return list;
        }
        #endregion
    }
}

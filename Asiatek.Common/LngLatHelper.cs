using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;

namespace Asiatek.Common
{
    /// <summary>
    ///LngLatHelper 的摘要说明
    /// </summary>
    public class LngLatHelper
    {
        public static int[] GetInt16Array4(int arg_int)
        {
            int[] intArray = new int[4];
            string c = Convert.ToString(arg_int, 2).PadLeft(4, (char)'0');
            intArray[0] = Convert.ToInt32(c.Substring(3, 1));
            intArray[1] = Convert.ToInt32(c.Substring(2, 1));
            intArray[2] = Convert.ToInt32(c.Substring(1, 1));
            intArray[3] = Convert.ToInt32(c.Substring(0, 1));
            return intArray;
        }

        public static void GetACCStatus(string strMPVCStatus, ref bool isAccOn, ref bool isMixing, ref bool isRemove)
        {
            if (string.IsNullOrEmpty(strMPVCStatus))
            {
                isAccOn = false;
                isMixing = false;
                isRemove = false;
                return;
            }
            if (strMPVCStatus.Length == 8)
            {
                int[] s = GetInt16Array4(Convert.ToInt32(strMPVCStatus.Substring(0, 1), 16));
                if ((s[0] & 1) == 1)
                {
                    isAccOn = true;
                }
                else
                {
                    isAccOn = false;
                }
                if ((s[2] & 1) == 1)
                {
                    isMixing = true;
                }
                else
                {
                    isMixing = false;
                }
                if ((s[3] & 1) == 1)
                {
                    isRemove = true;
                }
                else
                {
                    isRemove = false;
                }
            }
            else
            {
                if (strMPVCStatus == "0")
                {
                    isAccOn = false;
                    isMixing = false;
                    isRemove = false;
                }
                int s = Convert.ToInt32(strMPVCStatus);
                if ((s & 1) == 1)
                {
                    isAccOn = true;
                }
                else
                {
                    isAccOn = false;
                }
                if ((s & 4) == 4)
                {
                    isMixing = true;
                }
                else
                {
                    isMixing = false;
                }
                if ((s & 8) == 8)
                {
                    isRemove = true;
                }
                else
                {
                    isRemove = false;
                }
            }
        }

        public static void GetCompensateStatus(string platform, string strGpsid, string strMPVCStatus, ref string isCompensate)
        {
            try
            {
                if (platform == "VTS")
                {
                    isCompensate = "0";
                    if (strMPVCStatus.Substring(1, 1) == "1")
                    {
                        isCompensate = "1";
                    }
                }
                else
                {

                    if (strGpsid.Length == 15 && strGpsid.Substring(0, 2) == "35" && strMPVCStatus.Length == 8)
                    {
                        isCompensate = "0";
                        if (strMPVCStatus.Substring(1, 1) == "1")
                        {
                            isCompensate = "1";
                        }
                    }
                    else
                    {
                        isCompensate = "0";
                        if ((Convert.ToInt32(strMPVCStatus) & Convert.ToInt32(Math.Pow(2, 12))) == Math.Pow(2, 12))
                        {
                            isCompensate = "1";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isCompensate = "0";
            }
        }

        /// <summary>
        ///  经纬度列表
        /// </summary>
        private static List<string> LstLngLat = new List<string>{"117.248029,39.03915;117.25002,39.03926",
                                                            "117.25004,39.15534;117.252932,39.15519",
                                                            "117.63321,39.05854;117.639375,39.058755",
                                                            "117.21677,39.16995;117.218753,39.169346",
                                                            "117.15014,39.08653;117.156662,39.086234",
                                                            "117.20008,39.08306;117.206476,39.083111",
                                                            "117.23343,39.12308;117.231404,39.124006",
                                                            "117.26671,39.13729;117.257804,39.137306",
                                                            "117.2001,39.15848;117.202841,39.158062",
                                                            "117.45055,40.04554;117.441866,40.046001",
                                                            "117.2667,39.10114;117.259956,39.101642",
                                                            "117.1835,39.11524;117.182201,39.11553",
                                                            "117.2001,39.15235;117.205457,39.152594",
                                                            "117.23343,39.12695;117.269468,39.135022",
                                                            "117.1835,39.13064;117.176532,39.130563",
                                                            "117.2334,39.06148;117.236717,39.061514",
                                                            "117.15017,39.15943;117.154596,39.160979",
                                                            "117.38355,38.98033;117.386392,38.980406",
                                                            "117.13356,39.23553;117.132079,39.229918",
                                                            "117.01656,39.12665;117.015724,39.126348",
                                                            "117.33343,39.08122;117.329919,39.08066",
                                                            "117.20016,39.38804;117.194224,39.388288",
                                                            "117.3336,39.67557;117.3336,39.67557",
                                                            "116.98297,38.94746;116.977064,38.948044",
                                                            "117.81671,39.28805;117.814431,39.287948",
                                                            "117.41727,40.04397;117.414388,40.044426",
                                                            "117.63321,39.05819;117.640063,39.058471",
                                                            "117.48354,38.84118;117.472697,38.841614",
                                                            "117.81671,39.2626;117.783783,39.246584"};


        /// <summary>
        /// 取得经纬度与 listLngLat 中的经纬度的最近的点的偏移量
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns>以米为单位的长度</returns>
        public static string GetDiff(double lng, double lat)
        {
            double angLatA = lat / 180 * Math.PI;
            double angLonA = lng / 180 * Math.PI;
            double dMinDist = 0;
            string strDiff = "";
            for (int i = 0; i < LstLngLat.Count; i++)
            {
                double lng2 = Convert.ToDouble(LstLngLat[i].Split(';')[0].Split(',')[0]);
                double lat2 = Convert.ToDouble(LstLngLat[i].Split(';')[0].Split(',')[1]);
                GetLngLat_1000(ref lng2, ref lat2);

                double lng3 = Convert.ToDouble(LstLngLat[i].Split(';')[1].Split(',')[0]);
                double lat3 = Convert.ToDouble(LstLngLat[i].Split(';')[1].Split(',')[1]);

                double angLatB = lat2 / 180 * Math.PI;
                double angLonB = lng2 / 180 * Math.PI;
                if (System.Math.Abs(lng - lng2) != 0 || System.Math.Abs(lat - lat2) != 0)
                {
                    double dDist = System.Math.Sin(angLatA) * System.Math.Sin(angLatB) +
                        System.Math.Cos(angLatA) * System.Math.Cos(angLatB) *
                        System.Math.Cos(angLonB - angLonA);
                    //dDist = System.Math.Acos( dDist );
                    //dDist *= 69.09 * ( 180 / System.Math.PI ) * 1609.344; //unit: Meter
                    dDist = System.Math.Acos(dDist) * 6370693.4856530580439461631130889; //unit: Meter
                    if (i == 0)
                    {
                        dMinDist = dDist;
                        strDiff = Convert.ToString(lng2 - lng3) + "," + Convert.ToString(lat2 - lat3);
                    }
                    else if (dMinDist > dDist)
                    {
                        dMinDist = dDist;
                        strDiff = Convert.ToString(lng2 - lng3) + "," + Convert.ToString(lat2 - lat3);
                    }
                }
            }
            if (dMinDist < 500)
            {
                return strDiff;
            }
            else
                return "";
        }

        /// <summary>
        /// 经纬度西移1000米
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        public static void GetLngLat_1000(ref double lng, ref double lat)
        {
            double d2r = Math.PI / Convert.ToDouble(180);
            double r2d = Convert.ToDouble(180) / Math.PI;
            double Clat = 1.0 * 0.014483 / 1.609344;  // 把公里经纬度转换为英里经纬度
            double Clng = Clat / Math.Cos(lat * d2r);
            double theta = Math.PI * (Convert.ToDouble(180) / Convert.ToDouble(180));
            lat = lat + (Clat * Math.Sin(theta));
            lng = lng + (Clng * Math.Cos(theta));
        }

        /// <summary>
        /// 计算两点间距离
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static double DistanceFrom(double lat1, double lng1, double lat2, double lng2)
        {
            double R = 6378137;
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLng = (lng2 - lng1) * Math.PI / 180;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c;
            return Math.Round(d);
        }

        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="dateS"></param>
        /// <param name="dateE"></param>
        /// <returns></returns>
        public static string GetDateDiff(DateTime dateS, DateTime dateE)
        {
            TimeSpan tspan = dateE - dateS;
            double m = tspan.TotalMinutes;
            string dd = " ";
            if (m < 0) m = 0 - m;
            if (m > 60)
            {
                int h = Convert.ToInt32(m / 60);
                int mm = Convert.ToInt32(m % 60);
                if (h >= 24)
                {
                    int d = Convert.ToInt32(h / 24);
                    int hh = Convert.ToInt32(h % 24);
                    dd = d.ToString() + "天" + hh.ToString() + "时" + mm.ToString() + "分";
                }
                else { dd = h.ToString() + "时" + mm.ToString() + "分"; }
            }
            else
            {
                dd = Convert.ToInt32(m).ToString() + "分";
            }
            return dd;
        }
    }
}
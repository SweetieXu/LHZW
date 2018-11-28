﻿using Asiatek.Common;
using Asiatek.DBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Asiatek.WebGateDataService
{
//    /// <summary>
//    /// 每隔一段时间调用一次龙华那边接口，获取时间段内的门岗异常数据
//    /// </summary>
//    class Program
//    {
//        //执行任务的周期（毫秒）
//        static int timeIntervalFlag = Convert.ToInt32(ConfigurationManager.AppSettings["TimeIntervalFlag"]) * 60 * 1000;
//        //结束时间，用于作为下次获取数据的开始时间
//        static string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

//        static void Main(string[] args)
//        {
//            //刚运行程序的时候，查询门岗记录表中最近进出门岗的时间作为开始时间（如果是初次运行，表中没有数据则开始时间为当前时间），查询开始时间到目前时间的数据添加到门岗记录表中
//            //然后每隔10分钟执行调用程序，开始时间为上次结束时间+1毫秒，结束时间为上次的结束时间+10min
//            GetStartData();
//            //定义一个定时器
//            System.Timers.Timer Wtimer = new System.Timers.Timer(timeIntervalFlag);
//            //AutoReset 属性为 true 时，每隔指定时间循环一次；
//            //如果为 false，则只执行一次。
//            Wtimer.AutoReset = true;
//            Wtimer.Enabled = true;
//            Wtimer.Elapsed += new System.Timers.ElapsedEventHandler(Wtimer_Elapsed);
//            string readLine;
//            do
//            {
//                readLine = Console.ReadLine();
//            } while (readLine != null && readLine != "exit");
//        }

//        //定时调用
//        private static void Wtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
//        {
//            string msg = string.Format("调用龙华总务进出门岗系统数据接口：{0}", DateTime.Now);
//            LogHelper.DoGetGateRecordLog(msg);

//            //调用接口 
//            try
//            {
//                LongHua.Service1SoapClient service = new LongHua.Service1SoapClient();
//                //不传车牌号  返回的是这段时间范围内所有满足条件的车辆的进出岗的信息
//                //传递车牌号  返回的是查询指定车辆的信息
//                //开始时间必传
//                //不传结束时间   返回的是从开始时间开始到现在的所有满足条件的数据
//                //传递结束时间   返回的是从开始时间到结束时间所有满足条件的数据

//                //此时的开始时间是上次的结束时间+1毫秒，结束时间是上次的结束时间+任务周期
//                string startTime = Convert.ToDateTime(endTime).AddMilliseconds(1).ToString("yyyy-MM-dd HH:mm:ss");
//                endTime = Convert.ToDateTime(endTime).AddMilliseconds(timeIntervalFlag).ToString("yyyy-MM-dd HH:mm:ss");
//                string rd = service.GetVehicleRecord("", startTime, endTime);

//                ResultModel[] rs = JsonConvert.DeserializeObject<ResultModel[]>(rd);
//                if (rs.Length > 0)
//                {
//                    DataTable dt = ConvertToDataTable(rs);

//                    List<SqlBulkCopyColumnMapping> mapping = new List<SqlBulkCopyColumnMapping>();
//                    #region 数据
//                    mapping.Add(new SqlBulkCopyColumnMapping("CarID", "CarID"));
//                    mapping.Add(new SqlBulkCopyColumnMapping("CarNumber", "CarNumber"));
//                    mapping.Add(new SqlBulkCopyColumnMapping("PassGate", "PassGate"));
//                    mapping.Add(new SqlBulkCopyColumnMapping("InOrOut", "InOrOut"));
//                    mapping.Add(new SqlBulkCopyColumnMapping("InOrOutTime", "InOrOutTime"));
//                    #endregion
//                    MSSQLHelper.BulkCopyAsync(dt, "GateSentryRecord", mapping.ToArray());
//                }
//                else
//                {
//                    Console.WriteLine("暂无数据");
//                    string ms = string.Format("{0}：{1}  无返回数据", startTime, endTime);
//                    LogHelper.DoGetGateRecordLog(ms);
//                }

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                LogHelper.DoGetGateRecordLog(ex.Message);
//            }
//        }

//        //运行程序时候调用
//        private static void GetStartData()
//        {
//            //查询表中有无数据
//            string sql = string.Format(@"SELECT TOP 1 [InOrOutTime] 
//  FROM [dbo].[GateSentryRecord] ORDER BY InOrOutTime DESC ");
//            DataTable dtSource = MSSQLHelper.ExecuteDataTable(System.Data.CommandType.Text, sql);
//            //如果有异常，则直接返回
//            if (dtSource == null)
//            {
//                return;
//            }

//            string msg = string.Format("调用龙华总务进出门岗系统数据接口：{0}", DateTime.Now);
//            LogHelper.DoGetGateRecordLog(msg);

//            //调用接口 
//            try
//            {
//                LongHua.Service1SoapClient service = new LongHua.Service1SoapClient();
//                string rd, startTime;
//                //没有数据时，开始时间为当天00:00:00时间，当前时间为结束时间
//                if (dtSource.Rows.Count < 1)
//                {
//                    startTime = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
//                    rd = service.GetVehicleRecord("", startTime, endTime);
//                }
//                //有数据时，查询出来的时间作为开始时间，当前时间为结束时间
//                else
//                {
//                    startTime = Convert.ToDateTime(dtSource.Rows[0]["InOrOutTime"]).ToString("yyyy-MM-dd HH:mm:ss");
//                    rd = service.GetVehicleRecord("", startTime, endTime);
//                }

//                ResultModel[] rs = JsonConvert.DeserializeObject<ResultModel[]>(rd);
//                if (rs.Length > 0)
//                {
//                    DataTable dt = ConvertToDataTable(rs);

//                    List<SqlBulkCopyColumnMapping> mapping = new List<SqlBulkCopyColumnMapping>();
//                    #region 数据
//                    mapping.Add(new SqlBulkCopyColumnMapping("CarID", "CarID"));
//                    mapping.Add(new SqlBulkCopyColumnMapping("CarNumber", "CarNumber"));
//                    mapping.Add(new SqlBulkCopyColumnMapping("PassGate", "PassGate"));
//                    mapping.Add(new SqlBulkCopyColumnMapping("InOrOut", "InOrOut"));
//                    mapping.Add(new SqlBulkCopyColumnMapping("InOrOutTime", "InOrOutTime"));
//                    #endregion
//                    MSSQLHelper.BulkCopyAsync(dt, "GateSentryRecord", mapping.ToArray());
//                }
//                else
//                {
//                    Console.WriteLine("暂无数据");
//                    string ms = string.Format("{0}：{1}  无返回数据", startTime, endTime);
//                    LogHelper.DoGetGateRecordLog(ms);
//                }

//                #region MyRegion
//                //if (rs.Length > 0)  //插入数据到数据库  此方法限制一千条
//                //{
//                //    string insertSQL = "INSERT INTO [dbo].[GateSentryRecord] VALUES ";

//                //    foreach (var item in rs[0].ds)
//                //    {
//                //        insertSQL += "( '" + item.CarNumber + "','" + item.PassGate + "','" + item.InOrOut + "','" + item.InOrOutTime+"' ),";
//                //    }
//                //    //去掉最后的","
//                //    insertSQL = string.Format(insertSQL.Substring(0, insertSQL.Length - 1));
//                //    bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, insertSQL) > 0;
//                //} 
//                #endregion
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                LogHelper.DoGetGateRecordLog(ex.Message);
//            }
//        }


//        public static DataTable ConvertToDataTable(ResultModel[] rs)
//        {
//            DataTable dt = new DataTable();
//            dt.Columns.Add("CarID", typeof(int));
//            dt.Columns.Add("CarNumber", typeof(string));
//            dt.Columns.Add("PassGate", typeof(string));
//            dt.Columns.Add("InOrOut", typeof(string));
//            dt.Columns.Add("InOrOutTime", typeof(DateTime));

//            List<ReturnModel> list = rs[0].ds; //避免每个字段赋值时候都要ToList()
//            foreach (var item in list)
//            {
//                DataRow dr = dt.NewRow();
//                dr["CarID"] = item.CarID;
//                dr["CarNumber"] = item.CarNumber;
//                dr["PassGate"] = item.PassGate;
//                dr["InOrOut"] = item.InOrOut;
//                dr["InOrOutTime"] = item.InOrOutTime;
//                dt.Rows.Add(dr);
//            }

//            #region MyRegion
//            //for (int i = 0; i < rs[0].ds.Count; i++)
//            //{
//            //    DataRow dr = dt.NewRow();
//            //    dr["CarID"] = rs[0].ds[i].CarID;
//            //    dr["CarNumber"] = rs[0].ds[i].CarNumber;
//            //    dr["PassGate"] = rs[0].ds[i].PassGate;
//            //    dr["InOrOut"] = rs[0].ds[i].InOrOut;
//            //    dr["InOrOutTime"] = rs[0].ds[i].InOrOutTime;
//            //    dt.Rows.Add(dr);
//            //} 
//            #endregion

//            //去掉重复行（CarNumber、PassGate、InOrOut、InOrOutTime都重复的行数据）
//            for (int i = dt.Rows.Count - 2; i > 0; i--)
//            {
//                string form = string.Format("{0}='{1}' AND {2}='{3}' AND {4}='{5}' AND {6}='{7}'", "CarNumber", dt.Rows[i]["CarNumber"], "PassGate", dt.Rows[i]["PassGate"], "InOrOut", dt.Rows[i]["InOrOut"], "InOrOutTime", dt.Rows[i]["InOrOutTime"]);
//                DataRow[] rows = dt.Select(form);
//                if (rows.Length > 1)
//                {
//                    dt.Rows.RemoveAt(i);
//                }
//            }

//            //string[] filedName = { "CarNumber", "PassGate", "InOrOut", "InOrOutTime" };
//            //dt = GetDistinctSelf(dt, filedName);

//            return dt;
//        }

//    }

    
    class Program
    {
        static void Main(string[] args)
        {
            //带参启动运行服务
            if (args.Length > 0)
            {
                try
                {
                    ServiceBase[] serviceToRun = new ServiceBase[] { new WindowsService() };
                    ServiceBase.Run(serviceToRun);
                    System.IO.File.AppendAllText(@"D:\Log.txt", "\nService Start 带参启动运行服务：" + DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(@"D:\Log.txt", "\nService Start Error：" + DateTime.Now.ToString() + "\n" + ex.Message);
                }
            }
            //不带参启动配置程序
            else
            {
            StartLable:
                Console.WriteLine("请选择你要执行的操作——1：自动部署服务，2：安装服务，3：卸载服务，4：验证服务状态，5：退出");
                Console.WriteLine("————————————————————");
                ConsoleKey key = Console.ReadKey().Key;

                if (key == ConsoleKey.NumPad1 || key == ConsoleKey.D1)
                {
                    if (ServiceHelper.IsServiceExisted("GateDataService"))
                    {
                        ServiceHelper.ConfigService("GateDataService", false);
                    }
                    if (!ServiceHelper.IsServiceExisted("GateDataService"))
                    {
                        ServiceHelper.ConfigService("GateDataService", true);
                    }
                    ServiceHelper.StartService("GateDataService");
                    goto StartLable;
                }
                else if (key == ConsoleKey.NumPad2 || key == ConsoleKey.D2)
                {
                    if (!ServiceHelper.IsServiceExisted("GateDataService"))
                    {
                        ServiceHelper.ConfigService("GateDataService", true);
                    }
                    else
                    {
                        Console.WriteLine("\n服务已存在......");
                    }
                    goto StartLable;
                }
                else if (key == ConsoleKey.NumPad3 || key == ConsoleKey.D3)
                {
                    if (ServiceHelper.IsServiceExisted("GateDataService"))
                    {
                        ServiceHelper.ConfigService("GateDataService", false);
                    }
                    else
                    {
                        Console.WriteLine("\n服务不存在......");
                    }
                    goto StartLable;
                }
                else if (key == ConsoleKey.NumPad4 || key == ConsoleKey.D4)
                {
                    if (!ServiceHelper.IsServiceExisted("GateDataService"))
                    {
                        Console.WriteLine("\n服务不存在......");
                    }
                    else
                    {
                        Console.WriteLine("\n服务状态：" + ServiceHelper.GetServiceStatus("GateDataService").ToString());
                    }
                    goto StartLable;
                }
                else if (key == ConsoleKey.NumPad5 || key == ConsoleKey.D5) { }
                else
                {
                    Console.WriteLine("\n请输入一个有效键！");
                    Console.WriteLine("————————————————————");
                    goto StartLable;
                }
            }
        }

    }

    public class ResultModel
    {
        public List<ReturnModel> ds { get; set; }
    }

    public class ReturnModel
    {
        /// <summary>
        /// 返回结果集ID
        /// </summary>
        public string CarID { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string CarNumber { get; set; }
        /// <summary>
        /// 出入门岗
        /// </summary>
        public string PassGate { get; set; }
        /// <summary>
        /// 出入类型(出口/入口)
        /// </summary>
        public string InOrOut { get; set; }
        /// <summary>
        /// 出入时间
        /// </summary>
        public DateTime InOrOutTime { get; set; }
    }
}
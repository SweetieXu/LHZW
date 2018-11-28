﻿using Asiatek.Common;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.TimingItems.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Asiatek.TimingItems
{
//    /// <summary>
//    /// 定时项目，包括定时生成门岗异常报表以及定时发送邮件功能
//    /// </summary>
//    class Program
//    {
//        //执行任务的周期（毫秒）
//        static int timeIntervalFlag = Convert.ToInt32(ConfigurationManager.AppSettings["TimeIntervalFlag"]) * 60 * 1000;
//        static int inOrOutTimeFlag = Convert.ToInt32(ConfigurationManager.AppSettings["InOrOutTimeFlag"]);
//        // 获取设置时间； 在某个时候执行
//        static int iHour = Convert.ToInt32(ConfigurationManager.AppSettings["IHour"]);
//        static int iMinute = Convert.ToInt32(ConfigurationManager.AppSettings["IMinute"]);

//        static void Main(string[] args)
//        {
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


//        //定时调用  在每天的某个时间执行程序
//        private static void Wtimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
//        {
//            // 得到 hour minute 
//            int intHour = e.SignalTime.Hour;
//            int intMinute = e.SignalTime.Minute;
//            // 设置  每天的某个时候开始执行程序  
//            // 如果在iHour内重新运行程序，那么会重新导出excel、发送邮件
//            if (intHour == iHour && intMinute == iMinute)
//            {
//                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//                ExportExcel(time);
//            }
//        }


//        //导出门岗异常数据的excel表格
//        private static void ExportExcel(string time)
//        {
//            //标记邮件附件是否是空，没有异常数据的不添加附件
//            bool isEmailEmpty = false;
//            LogHelper.DoAnalysisGateExLog(time + "：分析门岗异常开始");

//            #region 查询门岗和电子围栏数据
//            //excel表格里面的数据是从昨天设定时间到今天设置时间的异常分析数据
//            var sTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd 00:00:00")).AddHours(iHour).AddMinutes(iMinute).ToString("yyyy-MM-dd HH:mm:ss");
//            var eTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")).AddHours(iHour).AddMinutes(iMinute).ToString("yyyy-MM-dd HH:mm:ss");
//            // 查询符合搜索条件的门岗数据
//            string sql_gate = string.Format(@" SELECT CarNumber AS PlateNum,PassGate,InOrOut,InOrOutTime FROM dbo.GateSentryRecord 
//                                     WHERE InOrOutTime BETWEEN '{0}' AND '{1}' ORDER BY InOrOutTime ASC  ", sTime, eTime);
//            DataTable gateDt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql_gate);

//            //查询符合搜索条件的电子围栏数据
//            string sql_ef = string.Format(@" SELECT PlateNum,ExceptionType,SignalStartTime,SignalEndTime  FROM [dbo].[VW_ElectricFenceException]  
//                                     WHERE ExceptionType=0 AND (SignalStartTime BETWEEN '{0}' AND '{1}' OR SignalEndTime BETWEEN '{0}' AND '{1}' ) 
//                                ORDER BY SignalStartTime ASC ", sTime, eTime);
//            DataTable efDt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql_ef); 
//            #endregion

//            #region 数据分析处理
//            //查询报错
//            if (gateDt == null || efDt == null)
//            {
//                LogHelper.DoAnalysisGateExLog("查询数据报错");
//                return;
//            }
//            //都没有数据
//            if (gateDt.Rows.Count == 0 && efDt.Rows.Count == 0)
//            {
//                LogHelper.DoAnalysisGateExLog("门岗和电子围栏都没有数据");
//                isEmailEmpty = true;
//            }

//            //数据对比的两种情况：
//            //一：其中一个结果集没有数据
//            //二：两个查询结果都有数据
//            //返回两种情况的数据：
//            //1、有进出电子围栏的信息，但是没有门岗系统的信息或者时间间隔相差10（可配置）分钟以上
//            //2、有进出门岗的信息，但是没有进出电子围栏的信息或者时间间隔相差10（可配置）分钟以上
//            List<GateExceptionModel> rs = new List<GateExceptionModel>();
//            DataTable tempDt = new DataTable();
//            tempDt.Columns.Add(StringConvert("车牌号", "1"), typeof(string));
//            tempDt.Columns.Add(StringConvert("异常类型", "1"), typeof(string));
//            tempDt.Columns.Add(StringConvert("门岗名称", "1"), typeof(string));
//            tempDt.Columns.Add(StringConvert("进出门岗", "1"), typeof(string));
//            tempDt.Columns.Add(StringConvert("进出门岗时间", "1"), typeof(string));//datetime类型不能为空，设置string类型
//            tempDt.Columns.Add(StringConvert("异常开始时间", "1"), typeof(string));
//            tempDt.Columns.Add(StringConvert("异常结束时间", "1"), typeof(string)); 
//            #endregion

//            #region 一、其中一个结果集没有数据
//            if (gateDt.Rows.Count == 0 && efDt.Rows.Count != 0) 
//            {
//                for (int i = 0; i < efDt.Rows.Count; i++)
//                {
//                    tempDt.Rows.Add(efDt.Rows[i]["PlateNum"], StringConvert("电子围栏异常", "1"), "", "", "", efDt.Rows[i]["SignalStartTime"], efDt.Rows[i]["SignalEndTime"]);
//                }
//            }
//            else if (gateDt.Rows.Count != 0 && efDt.Rows.Count == 0)
//            {
//                for (int i = 0; i < gateDt.Rows.Count; i++)
//                {
//                    tempDt.Rows.Add(gateDt.Rows[i]["PlateNum"], StringConvert("门岗异常", "1"), gateDt.Rows[i]["PassGate"], gateDt.Rows[i]["InOrOut"], gateDt.Rows[i]["InOrOutTime"], "", "");
//                }
//            }
//            #endregion

//            #region 二、两个结果集都有数据
//            else
//            {
//                //1   分析电子围栏信息时  门岗信息要同时有满足开始时间间隔10分钟内的进入门岗系统信息  和  满足结束时间间隔10分钟内的驶出门岗系统信息，否则记录异常
//                for (int i = 0; i < efDt.Rows.Count; i++)
//                {
//                    //查询电子围栏记录开始时间 正负10分钟内进入门岗系统的记录
//                    string formIn = string.Format(" {0}='{1}' AND {2}='{3}' AND {4} >= '{5}' AND  {6} <= '{7}' ",
//                        "PlateNum", efDt.Rows[i]["PlateNum"], "InOrOut", "入口",
//                        "InOrOutTime", Convert.ToDateTime(efDt.Rows[i]["SignalStartTime"]).AddMinutes(-inOrOutTimeFlag).ToString("yyyy-MM-dd HH:mm:ss"),
//                        "InOrOutTime", Convert.ToDateTime(efDt.Rows[i]["SignalStartTime"]).AddMinutes(inOrOutTimeFlag).ToString("yyyy-MM-dd HH:mm:ss"));
//                    var InCount = gateDt.Select(formIn).Count();

//                    //查询电子围栏记录结束时间 正负10分钟内出了门岗系统的记录
//                    string formOut = string.Format(" {0}='{1}' AND {2}='{3}' AND {4} >= '{5}' AND  {6} <= '{7}' ",
//                        "PlateNum", efDt.Rows[i]["PlateNum"], "InOrOut", "出口",
//                        "InOrOutTime", Convert.ToDateTime(efDt.Rows[i]["SignalEndTime"]).AddMinutes(-inOrOutTimeFlag).ToString("yyyy-MM-dd HH:mm:ss"),
//                        "InOrOutTime", Convert.ToDateTime(efDt.Rows[i]["SignalEndTime"]).AddMinutes(inOrOutTimeFlag).ToString("yyyy-MM-dd HH:mm:ss"));
//                    var OutCount = gateDt.Select(formOut).Count();

//                    //进入驶出都有记录时候不算异常，其余条件算异常
//                    if (!(InCount > 0 && OutCount > 0))
//                    {
//                        tempDt.Rows.Add(efDt.Rows[i]["PlateNum"], StringConvert("电子围栏异常", "1"), "", "", "", efDt.Rows[i]["SignalStartTime"], efDt.Rows[i]["SignalEndTime"]);
//                    }
//                }

//                //2 分析进出门岗信息，区别进入门岗和驶出门岗
//                for (int i = 0; i < gateDt.Rows.Count; i++)
//                {
//                    string form = string.Empty;
//                    //比较电子围栏开始时间
//                    if (gateDt.Rows[i]["InOrOut"].Equals("入口"))
//                    {
//                        form = string.Format(" {0}='{1}' AND {2} >= '{3}' AND {4} <= '{5}' ", "PlateNum", gateDt.Rows[i]["PlateNum"],
//                        "SignalStartTime", Convert.ToDateTime(gateDt.Rows[i]["InOrOutTime"]).AddMinutes(-inOrOutTimeFlag).ToString("yyyy-MM-dd HH:mm:ss"),
//                        "SignalStartTime", Convert.ToDateTime(gateDt.Rows[i]["InOrOutTime"]).AddMinutes(inOrOutTimeFlag).ToString("yyyy-MM-dd HH:mm:ss"));
//                    }
//                    //比较电子围栏结束时间
//                    if (gateDt.Rows[i]["InOrOut"].Equals("出口"))
//                    {
//                        form = string.Format(" {0}='{1}' AND {2} >= '{3}' AND {4} <= '{5}' ", "PlateNum", gateDt.Rows[i]["PlateNum"],
//                        "SignalEndTime", Convert.ToDateTime(gateDt.Rows[i]["InOrOutTime"]).AddMinutes(-inOrOutTimeFlag).ToString("yyyy-MM-dd HH:mm:ss"),
//                        "SignalEndTime", Convert.ToDateTime(gateDt.Rows[i]["InOrOutTime"]).AddMinutes(inOrOutTimeFlag).ToString("yyyy-MM-dd HH:mm:ss"));
//                    }
//                    //能查出记录时不算异常
//                    if (efDt.Select(form).Count() < 1)
//                    {
//                        tempDt.Rows.Add(gateDt.Rows[i]["PlateNum"], StringConvert("门岗异常", "1"), gateDt.Rows[i]["PassGate"], gateDt.Rows[i]["InOrOut"], gateDt.Rows[i]["InOrOutTime"], "", "");
//                    }
//                }  
//            }
//            #endregion

//            string fileName = String.Format("{0}_{1}.xls", StringConvert("门岗数据异常信息", "1"), Convert.ToDateTime(time).ToString("yyyyMMddHHssmm"));
//            string dirName = "导出excel";
//            //设置了导出的数据都显示繁体字
//            Asiatek.TimingItems.Helpers.NPOIHelper.DataTableToExcel(tempDt, fileName, dirName);
//            LogHelper.DoAnalysisGateExLog("导出excel成功！");

//            //导出成功后发送邮件
//            SendEmail(time, isEmailEmpty);
//        }


//        //设置发送邮件，将excel表格作为邮件附件发送给收件人
//        private static void SendEmail(string time, bool isEmailEmpty)
//        {
//            try
//            {
//                LogHelper.DoSendEmailLog(time+"：开始发送邮件");
//                MailMessage mailMessage = new MailMessage();
//                //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
//                mailMessage.From = new MailAddress("asiatek@cn.ennoconn.com");

//                #region 添加收件人邮箱
//                //收件人邮箱地址。获取数据表的数据
//                string sql = string.Format(@" SELECT DISTINCT Email FROM [dbo].[ReceiverMailInfo] WHERE Status=1 ");
//                List<EmailModel> emailList = ConvertToList<EmailModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
//                if (emailList == null)
//                {
//                    LogHelper.DoSendEmailLog("查询收件人邮箱地址失败！");
//                    return;
//                }
//                else if (emailList.Count == 0)
//                {
//                    LogHelper.DoSendEmailLog("没有收件人地址！");
//                    return;
//                }
//                foreach (var item in emailList)
//                {
//                    mailMessage.To.Add(new MailAddress(item.Email));
//                } 
//                #endregion

//                //邮件标题。
//                mailMessage.Subject = StringConvert("门岗异常数据", "1");
//                if (isEmailEmpty == true)  //没有异常数据，不用添加附件
//                {
//                    mailMessage.Body = StringConvert("没有相关异常数据，附件为空。", "1");
//                }
//                else 
//                {
//                    //邮件内容。
//                    mailMessage.Body = StringConvert("附件为昨日门岗相关异常数据。", "1");
//                    //获得附件在本地地址
//                    string dirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "导出\\导出excel");
//                    string fileName = String.Format("{0}_{1}.xls", StringConvert("门岗数据异常信息", "1"), Convert.ToDateTime(time).ToString("yyyyMMddHHssmm"));
//                    string SUpFile = Path.Combine(dirPath, fileName);
//                    //将文件进行转换成Attachments
//                    Attachment data = new Attachment(SUpFile, MediaTypeNames.Application.Octet);
//                    mailMessage.Attachments.Add(data);
//                }
//                //实例化一个SmtpClient类。
//                SmtpClient client = new SmtpClient();
//                //收件箱邮箱域名
//                client.Host = "10.134.99.130";
//                //使用安全加密连接。
//                client.EnableSsl = false;
//                //不和请求一块发送。
//                client.UseDefaultCredentials = false;
//                //发送
//                client.Send(mailMessage);
//                LogHelper.DoSendEmailLog(time+"：发送邮件成功！");

//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                LogHelper.DoSendEmailLog(ex.Message);
//            }

//        }


//        //简体和繁体相互转换
//        public static string StringConvert(string x, string type)
//        {
//            String value = String.Empty;
//            switch (type)
//            {
//                case "1"://转繁体
//                    value = Microsoft.VisualBasic.Strings.StrConv(x, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
//                    break;
//                case "2"://转简体
//                    value = Microsoft.VisualBasic.Strings.StrConv(x, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese, 0);
//                    break;
//                default:
//                    break;
//            }
//            return value;
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
                    if (ServiceHelper.IsServiceExisted("TimingItemsService"))
                    {
                        ServiceHelper.ConfigService("TimingItemsService", false);
                    }
                    if (!ServiceHelper.IsServiceExisted("TimingItemsService"))
                    {
                        ServiceHelper.ConfigService("TimingItemsService", true);
                    }
                    ServiceHelper.StartService("TimingItemsService");
                    goto StartLable;
                }
                else if (key == ConsoleKey.NumPad2 || key == ConsoleKey.D2)
                {
                    if (!ServiceHelper.IsServiceExisted("TimingItemsService"))
                    {
                        ServiceHelper.ConfigService("TimingItemsService", true);
                    }
                    else
                    {
                        Console.WriteLine("\n服务已存在......");
                    }
                    goto StartLable;
                }
                else if (key == ConsoleKey.NumPad3 || key == ConsoleKey.D3)
                {
                    if (ServiceHelper.IsServiceExisted("TimingItemsService"))
                    {
                        ServiceHelper.ConfigService("TimingItemsService", false);
                    }
                    else
                    {
                        Console.WriteLine("\n服务不存在......");
                    }
                    goto StartLable;
                }
                else if (key == ConsoleKey.NumPad4 || key == ConsoleKey.D4)
                {
                    if (!ServiceHelper.IsServiceExisted("TimingItemsService"))
                    {
                        Console.WriteLine("\n服务不存在......");
                    }
                    else
                    {
                        Console.WriteLine("\n服务状态：" + ServiceHelper.GetServiceStatus("TimingItemsService").ToString());
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


    public class EmailModel
    {
        public string Email { get; set; }
    }
}

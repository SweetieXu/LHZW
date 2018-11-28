using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Asiatek.Common
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 基础路径
        /// </summary>
        static readonly string BASEDIRECTORY = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "日志");

        #region 同步锁
        static object dbErrorLocker = new object();
        static object otherErrorLocker = new object();
        static object normalInfoLocker = new object();
        static object terminalSettingsLocker = new object();
        #endregion

        /// <summary>
        /// 记录日志
        /// GBK编码
        /// </summary>
        static void DoLog(string log, string dirName, object locker)
        {
            try
            {
                lock (locker)
                {
                    string dirPath = Path.Combine(BASEDIRECTORY, dirName);
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string filename = Path.Combine(dirPath, DateTime.Now.ToString("yyyy年MM月dd日") + ".log");
                    using (FileStream fs = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GBK")))
                        {
                            sw.WriteLine(string.Format("{0}：{1}{2}", DateTime.Now.ToString("HH:mm:ss"), Environment.NewLine, log));
                            sw.WriteLine("".PadLeft(100, '@'));
                            sw.Flush();
                        }
                    }
                }

            }
            catch { }
        }

        /// <summary>
        /// 记录数据库异常日志
        /// </summary>
        public static void DoDataBaseErrorLog(string errorMessage)
        {
            DoLog(errorMessage, "数据库异常", dbErrorLocker);
        }

        /// <summary>
        /// 记录其他异常日志
        /// </summary>
        public static void DoOtherErrorLog(string errorMessage)
        {
            DoLog(errorMessage, "其他异常", otherErrorLocker);
        }

        /// <summary>
        /// 记录普通信息日志
        /// </summary>
        public static void DoNormalLog(string message)
        {
            DoLog(message, "普通信息", normalInfoLocker);
        }

        /// <summary>
        /// 记录web服务调用异常
        /// </summary>
        public static void DoWebServiceErrorLog(string message)
        {
            DoLog(message, "Web服务异常", normalInfoLocker);
        }

        public static void TerminalSettingsErrorLog(string message)
        {
            DoLog(message, "终端设置异常", terminalSettingsLocker);
        }

        /// <summary>
        /// 调用龙华总务进出门岗系统数据接口
        /// </summary>
        public static void DoGetGateRecordLog(string message)
        {
            DoLog(message, "获取门岗数据异常", normalInfoLocker);
        }

        /// <summary>
        /// 分析门岗异常日志
        /// </summary>
        public static void DoAnalysisGateExLog(string message)
        {
            DoLog(message, "分析门岗异常日志", normalInfoLocker);
        }

        /// <summary>
        /// 发送电子邮件
        /// </summary>
        public static void DoSendEmailLog(string message)
        {
            DoLog(message, "发送电子邮件日志", normalInfoLocker);
        }

    }
}

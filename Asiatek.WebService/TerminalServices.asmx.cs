using Asiatek.BLL.MSSQL;
using RemoteClass;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Web;
using System.Web.Services;

namespace Asiatek.WebService
{
    /// <summary>
    /// TerminalServices 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class TerminalServices : System.Web.Services.WebService
    {

        #region 其他
        /// <summary>
        /// 远程对象
        /// </summary>
        TerminalRemote remoteObj;
        /// <summary>
        /// 共享的注册同步锁
        /// </summary>
        static object registerLocker = new object();
        /// <summary>
        /// 共享的注册标识
        /// true表示需要进行通道注册
        /// </summary>
        static bool needReg = true;

        /// <summary>
        /// 客户端注册信道
        /// </summary>
        private void Register()
        {
            //防止主机上machine.config文件中缺失相应通道信息导致无法自动注册
            //进行手动注册
            foreach (IChannel ic in ChannelServices.RegisteredChannels)
            {
                if (ic is TcpClientChannel)
                {
                    needReg = false;
                    break;
                }
            }
            if (needReg)
            {
                TcpClientChannel tcpChannel = new TcpClientChannel();
                ChannelServices.RegisterChannel(tcpChannel, false);
            }
        }

        /// <summary>
        /// 获取远程对象
        /// </summary>
        /// <returns></returns>
        private bool GetRemoteObj()
        {
            if (needReg)
            {
                lock (registerLocker)
                {
                    if (needReg)
                    {
                        Register();
                    }
                }
            }
            //激活远程对象
            remoteObj = (TerminalRemote)Activator.GetObject(typeof(TerminalRemote), string.Format("tcp://{0}/TerminalSet", ConfigurationManager.AppSettings["TerminalRemoting"].ToString()));
            if (remoteObj == null)
            {
                return false;
            }
            return true;
        }
        #endregion


        [WebMethod(Description = "处理紧急报警")]
        public int DealEmergencyAlarm(long ID, string terminalCode)
        {
            //TODO: 处理紧急报警 内部逻辑待定
            if (!GetRemoteObj())
            {
                return -2;
            }
            int result = 0;
            //808中要求，如果流水号为 0表示处理该报警类型所有消息
            var resultMsg = remoteObj.BeginSendHandleAlarm(terminalCode.Trim(), new byte[] { 0, 0 });
            switch (resultMsg)
            {
                case "成功":
                    //ExceptionBLL.UpdateEmergencyAlarm(ID);
                    result = 1;
                    break;
                case "暂无应答":
                    //ExceptionBLL.UpdateEmergencyAlarm(ID);
                    result = 2;
                    break;
                case "离线":
                    result = -1;
                    break;
                case "失败":
                case "消息有误":
                case "不支持":
                    result = 0;
                    break;
            }
            return result;
        }



    }
}

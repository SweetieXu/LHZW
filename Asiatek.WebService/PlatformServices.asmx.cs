using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Asiatek.BLL;
using Asiatek.Model;
using Asiatek.BLL.MSSQL;
using RemoteClass;
using System.Configuration;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;

namespace Asiatek.WebService
{
    /// <summary>
    /// PlatformServices 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PlatformServices : System.Web.Services.WebService
    {

        #region 其他
        /// <summary>
        /// 809平台远程对象
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
            remoteObj = (TerminalRemote)Activator.GetObject(typeof(TerminalRemote), string.Format("tcp://{0}/TerminalSet", ConfigurationManager.AppSettings["PlatformRemoting"].ToString()));
            if (remoteObj == null)
            {
                return false;
            }
            return true;
        }
        #endregion


        /// <summary>
        /// 回复查岗内容
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="replyContent"></param>
        /// <returns>-1：数据库异常或记录不存在；0：:通信异常；1：成功；2：已被回复</returns>
        [WebMethod(Description = "回复查岗信息")]
        public int ReplyInspectionMessage(UInt32 msgID, string replyContent)
        {
            //TODO: 回复查岗信息 内部逻辑待定
            //根据msgID获取查岗信息
            var result = InspectionBLL.GetInspectionInfoByID(msgID).DataResult;
            if (result == null)//发生错误或未找到记录
            {
                return -1;
            }
            //true代表已被其他人回复
            //这里其实不对，因为可能出现查询出以后被人回复的情况，所以实际上如果为了保证不会重复回复，需要做乐观并发
            //但是实际上发生概率实在太低，而且即使重复回复也不会有什么后果，所以不处理
            if (result.Flag)
            {
                return 2;
            }

            #region 由于老版本remoting设计问题，导致这里需要编写一次消息体

            if (!GetRemoteObj())
            {
                return -2;
            }

            //查岗应答内容
            byte[] byteText = Encoding.GetEncoding("GBK").GetBytes(replyContent.Trim());
            //查岗应答消息体
            //子业务类型标识2+后续数据长度4+查岗对象的类型1+查岗对象的ID12+信息ID4+数据长度4+应答内容
            byte[] clientSend = new byte[27 + byteText.Length];


            //子业务类型标识 开始0 结束1
            clientSend[0] = 0x13;
            clientSend[1] = 0x01;

            //后续数据长度 开始2 结束5
            byte[] checkObject = GetBigEndianBytes4(21 + byteText.Length);
            checkObject.CopyTo(clientSend, 2);

            //对象类型 开始6 结束6
            clientSend[6] = Convert.ToByte(result.ObjType);

            //查岗对象的ID 开始7 结束18
            byte[] B12 = new byte[12];
            B12 = Encoding.GetEncoding("GBK").GetBytes(result.ObjID);
            B12.CopyTo(clientSend, 7);

            // 信息ID 开始19 结束22
            byte[] byteSendId = GetBigEndianBytes4(msgID);
            byteSendId.CopyTo(clientSend, 19);

            // 数据长度 开始23 结束26
            byte[] sendTextLen = GetBigEndianBytes4(byteText.Length);
            sendTextLen.CopyTo(clientSend, 23);

            //应答内容 开始27
            byteText.CopyTo(clientSend, 27);
            #endregion



            string pn = result.PlatformName;
            string ac = result.AccessCode;
            string version = result.Version;
            uint M1 = (uint)result.M1;
            uint IA1 = (uint)result.IA1;
            uint IC1 = (uint)result.IC1;

            string opResult = string.Empty;
            //进行回复
            if (result.ObjType == "1")//针对平台的查岗
            {
                opResult = remoteObj.BeginSendPlatFormCheckMsg(clientSend, pn, ac, version, M1, IA1, IC1);
            }
            else//针对业户的查岗
            {
                opResult = remoteObj.BeginSendPlatFormCheckMsg(clientSend, pn, result.ObjID, ac, version, M1, IA1, IC1);
            }
            if (opResult != "成功")
            {
                return 0;
            }
            InspectionBLL.UpdateInspectionState(msgID, replyContent);
            return 1;
        }


        /// <summary>
        /// 获取4字节大端序字节数组
        /// </summary>
        private byte[] GetBigEndianBytes4(Int32 num)
        {
            byte[] bs = BitConverter.GetBytes(num);
            Array.Reverse(bs);
            return bs;
        }
        /// <summary>
        /// 获取4字节大端序字节数组
        /// </summary>
        private byte[] GetBigEndianBytes4(UInt32 num)
        {
            byte[] bs = BitConverter.GetBytes(num);
            Array.Reverse(bs);
            return bs;
        }
    }

}

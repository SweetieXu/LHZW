using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.TMS.Filters;
using Asiatek.Common;
using Asiatek.BLL.MSSQL;
using Asiatek.Model;
using System.Threading;

using Asiatek.TMS.PlatformServicesNS;
using Asiatek.TMS.TerminalServicesNS;
using Asiatek.Resource;
using Newtonsoft.Json;
using Asiatek.DBUtility;
using System.Threading.Tasks;
using System.Text;
using RemoteClass;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Configuration;
using Asiatek.TMS.TerminalOperation;
using Asiatek.TMS.Areas.TerminalSetting.Controllers;
using Asiatek.AjaxPager;


namespace Asiatek.TMS.Controllers
{
    public class HomeController : BaseController
    {

        #region 实时监控
        /// <summary>
        /// 实时监控
        /// </summary>
        public ActionResult Index()
        {
            ViewBag.Title = base.GetCurrentViewName();
            return View();
        }

        #region 查岗
        /// <summary>
        /// 获取查岗信息
        /// </summary>
        public ActionResult GetInspectionInfos()
        {
            List<InspectionListModel> result;
            if (base.IsUser)
            {
                result = InspectionBLL.GetUserInspectionInfos(base.CurrentStrucID);
            }
            else//管理员、超级管理员
            {
                result = InspectionBLL.GetPlatformInspectionInfos();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 旧版本通过web服务进行回复
        //[HttpPost, ValidateAntiForgeryToken]
        //[AsiatekSubordinateFunction("GetInspectionInfos")]
        //public async Task<ActionResult> ReplyInspection(UInt32 id, string replyContent)
        //{
        //    using (var client = new PlatformServicesSoapClient())
        //    {
        //        //调用web服务进行查岗回复
        //        var result = await client.ReplyInspectionMessageAsync(id, replyContent).ConfigureAwait(false);
        //        string msg = string.Empty;
        //        bool opResult = false;
        //        switch (result.Body.ReplyInspectionMessageResult)
        //        {
        //            case 1:
        //                msg = PromptInformation.OperationSuccess;
        //                opResult = true;
        //                break;
        //            case 0:
        //                msg = string.Format("{0}：{1}", PromptInformation.OperationFailure, PromptInformation.CommunicationError);
        //                break;
        //            case -1:
        //                msg = string.Format("{0}：{1}", PromptInformation.OperationFailure, PromptInformation.DBError);
        //                break;
        //            case -2:
        //                msg = string.Format("{0}：{1}", PromptInformation.OperationFailure, PromptInformation.RemotingError);
        //                break;
        //            case 2:
        //                msg = string.Format("{0}：{1}", PromptInformation.OperationFailure, PromptInformation.InspectionHadBeenRelied);
        //                break;
        //        }
        //        return Json(new OperationResult() { Success = opResult, Message = msg });
        //    }

        //} 
        #endregion


        /// <summary>
        /// 回复查岗信息
        /// 由于历史遗留问题造成查岗信息是存在物流的TMS库中
        /// 所以查询与更新均操作此库
        /// 以后后台程序全部更换后此功能需要重做
        /// </summary>
        /// <param name="id">查岗消息ID</param>
        /// <param name="replyContent">回复内容</param>
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("GetInspectionInfos")]
        public ActionResult ReplyInspection(UInt32 id, string replyContent)
        {
            string msg = PromptInformation.OperationFailure + ":{0}";


            #region 检查数据
            var obj = InspectionBLL.GetInspectionInfoByID(id);
            if (obj.DataResult == null)
            {
                return Json(new OperationResult() { Message = string.Format(msg, obj.Message) });
            }
            var data = obj.DataResult;
            //true代表已被其他人回复
            //这里其实有点问题，因为可能出现查询出以后被人回复的情况，所以实际上如果为了保证不会重复回复，需要做乐观并发
            //但是实际上发生概率实在太低，而且即使重复回复也不会有什么后果，所以不处理
            if (data.Flag)
            {
                return Json(new OperationResult() { Message = string.Format(msg, PromptInformation.InspectionHadBeenRelied) });
            }
            #endregion


            #region 回复查岗
            try
            {
                var remoteObj = GetRemoteObj();
                string pn = data.PlatformName;
                string ac = data.AccessCode;
                string version = data.Version;
                uint M1 = (uint)data.M1;
                uint IA1 = (uint)data.IA1;
                uint IC1 = (uint)data.IC1;
                string opResult = string.Empty;
                byte[] sendBytes = BuildReplyContentBytes(id, replyContent, data);
                //进行回复
                if (data.ObjType == "1")//针对平台的查岗
                {
                    opResult = remoteObj.BeginSendPlatFormCheckMsg(sendBytes, pn, ac, version, M1, IA1, IC1);
                }
                else//针对具体业户的查岗
                {
                    opResult = remoteObj.BeginSendPlatFormCheckMsg(sendBytes, pn, data.ObjID, ac, version, M1, IA1, IC1);
                }
                bool success = opResult == "成功";//remoting那边返回的结果是中文字
                string tempMsg = string.Empty;
                if (success)
                {
                    tempMsg = PromptInformation.OperationSuccess;
                    InspectionBLL.UpdateInspectionState(id, replyContent);//更新物流TMS库的查岗数据，注意这里无所谓更新失败还是成功，大不了重新再回复一次查岗
                }
                else
                {
                    tempMsg = string.Format(msg, PromptInformation.CommunicationError) + "[" + opResult + "]";
                }
                //不论是否成功，只要正常操作了就记录日志
                InspectionBLL.DoReplyLog(id, base.UserIDForLog, tempMsg, success);
                return Json(new OperationResult() { Success = success, Message = tempMsg });
            }
            catch
            {
                return Json(new OperationResult() { Message = string.Format(msg, PromptInformation.RemotingError) });
            }
            #endregion
        }

        #region 上级平台Remoting相关
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
        /// 获取上级平台远程操作对象
        /// </summary>
        /// <returns></returns>
        private TerminalRemote GetRemoteObj()
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
            string addr = string.Format("tcp://{0}/TerminalSet", ConfigurationManager.AppSettings["PlatformRemoting"].ToString());
            //激活远程对象
            return (TerminalRemote)Activator.GetObject(typeof(TerminalRemote), addr);
        }
        #endregion

        /// <summary>
        /// 由于上级平台对接程序设计的Remoting缺陷
        /// 因此需要重新编写一次查岗应答消息体
        /// </summary>
        /// <param name="msgID">查岗消息ID</param>
        /// <param name="replyContent">应答内容</param>
        /// <param name="data">查岗消息对象</param>
        /// <returns></returns>
        private byte[] BuildReplyContentBytes(UInt32 msgID, string replyContent, InspectionModel data)
        {
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
            clientSend[6] = Convert.ToByte(data.ObjType);

            //查岗对象的ID 开始7 结束18
            byte[] B12 = new byte[12];
            B12 = Encoding.GetEncoding("GBK").GetBytes(data.ObjID);
            B12.CopyTo(clientSend, 7);

            // 信息ID 开始19 结束22
            byte[] byteSendId = GetBigEndianBytes4(msgID);
            byteSendId.CopyTo(clientSend, 19);

            // 数据长度 开始23 结束26
            byte[] sendTextLen = GetBigEndianBytes4(byteText.Length);
            sendTextLen.CopyTo(clientSend, 23);

            //应答内容 开始27
            byteText.CopyTo(clientSend, 27);

            return clientSend;
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
        #endregion

        #region 信号
        [AsiatekSubordinateFunction("Index")]
        [AsiatekSubordinateFunction("Index", "HomeElectricFence")]
        public ActionResult GetUserVehiclesByVehicleName(string vehicleName)
        {
            List<UserVehicles> list = new List<UserVehicles>();
            // 默认模式
            if (base.VehicleViewMode)
            {
                list = VehicleBLL.GetDefaultVehiclesAndStrucName(base.CurrentStrucID, vehicleName);
            }
            else
            {
                list = VehicleBLL.GetVehiclesAndStrucName(base.CurrentUserID, vehicleName);
            }
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new { label = item.VehicleName + "[" + item.StrucName + "]", value = item.VehicleName, VID = item.VID });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }


        [AsiatekSubordinateFunction("Index")]
        public ActionResult GetRealTimeSignalByVehicleID(long vehicleID)
        {
            ////这里不对用户是否具有指定vehicleID做判断
            ////仅仅根据VID查询实时信号。。否则影响速度
            var result = SignalBLL.GetRealTimeSignalByVheicleID(vehicleID);
            if (result == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            var obj = new
            {
                VID = result.VID,
                VN = result.VehicleName,
                SN = result.StrucName,
                Speed = result.Speed,
                VIN = result.VIN,
                SignalTime = result.SignalDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Icon = result.Icon,
                Address = result.Address == null ? string.Empty : result.Address,
                Latitude = result.Latitude,
                Longitude = result.Longitude,
                Direction = GetDirection(result.Direction),
                Angle = result.Direction,
                IsOnline = result.IsOnline == 1,
                IsRunning = result.IsRunning == 1,
                Mileage = result.Mileage,
                OilHeight = result.OilHeight,
                Temperature = result.Temperature,
                ACCState = result.ACCState == true ? "ON" : "OFF",
                RollerState = result.RollerState,
                PlateColor = result.PlateColor,
                VehicleType = result.VehicleType,
                FlagState = (result.PressureFlag == true ? @DisplayText.PressureUnloading + "," : "") + (result.DoorsensorFlag == true ? @DisplayText.DoorOpen : ""),
                EscortName = result.EscortName,
                EscortPhone = result.EscortPhone,
                DriverName = result.DriverName,
                DriverPhone = result.DriverPhone,
                OwnersName = result.OwnersName,
                OwnersPhone = result.OwnersPhone
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        [AsiatekSubordinateFunction("Index"), HttpPost]
        public ActionResult GetRealTimeSignalsByVehicleIds(string ids)
        {
            List<long> vids = new List<long>();
            var temp = ids.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in temp)
            {
                vids.Add(long.Parse(item));
            }
            var result = SignalBLL.GetRealTimeSignalsByVheicleIDs(vids);

            if (result == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            List<dynamic> list = new List<dynamic>();
            foreach (var item in result)
            {
                list.Add(new
                {
                    VID = item.VID,
                    VN = item.VehicleName,
                    SN = item.StrucName,
                    Speed = item.Speed,
                    VIN = item.VIN,
                    SignalTime = item.SignalDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Icon = item.Icon,
                    Address = item.Address == null ? string.Empty : item.Address,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    Direction = GetDirection(item.Direction),
                    Angle = item.Direction,
                    IsOnline = item.IsOnline == 1,
                    IsRunning = item.IsRunning == 1,
                    Mileage = item.Mileage,
                    OilHeight = item.OilHeight,
                    Temperature = item.Temperature,
                    ACCState = item.ACCState == true ? "ON" : "OFF",
                    RollerState = item.RollerState,
                    PlateColor = item.PlateColor,
                    VehicleType = item.VehicleType,
                    FlagState = (item.PressureFlag == true ? @DisplayText.PressureUnloading + "," : "") + (item.DoorsensorFlag == true ? @DisplayText.DoorOpen : ""),
                    EscortName = item.EscortName,
                    EscortPhone = item.EscortPhone,
                    DriverName = item.DriverName,
                    DriverPhone = item.DriverPhone,
                    OwnersName = item.OwnersName,
                    OwnersPhone = item.OwnersPhone
                });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 根据车辆行驶角度获取方向
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private string GetDirection(double angle)
        {
            //TODO:待做国际化
            string direction = string.Empty;
            if (angle > 22 && angle <= 68)//东北
            {
                direction = UIText.NorthEast;
            }
            else if (angle > 68 && angle <= 112)//东
            {
                direction = UIText.East;
            }
            else if (angle > 112 && angle <= 158)//东南
            {
                direction = UIText.SouthEast;
            }
            else if (angle > 158 && angle <= 202)//南
            {
                direction = UIText.South;
            }
            else if (angle > 202 && angle <= 248)//西南
            {
                direction = UIText.SouthWest;
            }
            else if (angle > 248 && angle <= 292)//西
            {
                direction = UIText.West;
            }
            else if (angle > 292 && angle <= 338)//西北
            {
                direction = UIText.NorthWest;
            }
            else
            {
                direction = UIText.North;
            }
            return direction;
        }


        /// <summary>
        /// 根据用户ID获取用户分配的车辆的最新信号
        /// </summary>
        /// <returns></returns>
        [AsiatekSubordinateFunction("Index")]
        public ActionResult GetRealTimeSignals()
        {
            List<RealTimeSignalTreeModel> allItems;
            if (base.VehicleViewMode)
            {
                allItems = SignalBLL.GetDefaultRealTimeSignals(base.CurrentStrucID);
            }
            else
            {
                allItems = SignalBLL.GetRealTimeSignals(base.CurrentUserID);
            }



            if (allItems == null)
            {
                allItems = new List<RealTimeSignalTreeModel>();
            }
            allItems = allItems.OrderBy(item => item.SID).ToList();

            //提取单位
            var query = from item in allItems select new StructureTreeModel { ID = item.SID, StrucName = item.SN, ParentID = item.SPID };
            var structures = query.Distinct(new StrucTreeComparer()).ToList();

            List<BootstrapTreeViewNode> nodes = new List<BootstrapTreeViewNode>();
            for (int i = 0; i < structures.Count; i++)
            {
                var item = structures[i];
                var tempNode = new BootstrapTreeViewNode()
                {
                    text = item.StrucName,
                    selectable = false,
                    sid = item.ID,
                    state = new BootstrapTreeViewNodeState()
                    {
                        expanded = false
                    }
                };
                tempNode.nodes = new List<BootstrapTreeViewNode>();
                CreateSubStrucNode(tempNode, structures, allItems, item.ID);
                CreateVehicleNode(tempNode, allItems, item.ID);
                nodes.Add(tempNode);
                structures.RemoveAt(i);
                i--;
            }
            //展开第一个 单位节点
            if (nodes.Count > 0)
            {
                nodes.First().state.expanded = true;
            }
            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        private void CreateSubStrucNode(BootstrapTreeViewNode parentNode, List<StructureTreeModel> list, List<RealTimeSignalTreeModel> allItems, int pid)
        {
            var subStrucs = list.Where(s => s.ParentID == pid).ToList();
            if (subStrucs.Count == 0)
            {
                return;
            }
            foreach (var item in subStrucs)
            {
                var temp = new BootstrapTreeViewNode()
                {
                    text = item.StrucName,
                    selectable = false,
                    sid = item.ID
                };
                temp.nodes = new List<BootstrapTreeViewNode>();
                CreateVehicleNode(temp, allItems, item.ID);
                list.Remove(item);
                CreateSubStrucNode(temp, list, allItems, item.ID);
                parentNode.nodes.Add(temp);
            }
        }

        private void CreateVehicleNode(BootstrapTreeViewNode tempNode, List<RealTimeSignalTreeModel> allItems, int sid)
        {
            var query = allItems.Where(item => item.SID == sid);//找单位ID是当前单位节点的车辆
            List<RealTimeSignalTreeModel> list = new List<RealTimeSignalTreeModel>();
            foreach (var item in query)
            {
                string color = string.Empty;
                if (item.IsOnline == 0)//掉线 红色
                {
                    color = "#ff0202";
                }
                else if (item.Latitude == null)//信号表无数据
                {
                    color = "#B766AD";
                }
                else if (item.IsOnline == 1 && item.IsRunning == 1)//在线有速度
                {
                    color = "#00a1fe";
                }
                else//在线无速度 停车 灰色
                {
                    color = "#8c8c8c";
                }

                tempNode.nodes.Add(new BootstrapTreeViewNode()
                {
                    text = item.VN,
                    tag = new { VID = item.VID, SN = item.SN, SID = item.SID, IsOnline = item.IsOnline == 1, IsRunning = item.IsRunning == 1, NoSignal = item.Latitude == null },//只有车辆节点才有tag
                    color = color,
                });
                list.Add(item);
            }
            //移除添加过的数据
            list.ForEach(item =>
            {
                allItems.Remove(item);
            });
        }
        #endregion

        #region 紧急告警
        /// <summary>
        /// 获取紧急告警信息
        /// </summary>
        public ActionResult GetEmergencyAlarms()
        {
            List<EmergencyAlarmInfoModel> result;
            if (base.VehicleViewMode)
            {
                result = ExceptionBLL.GetDefaultEmergencyAlarms(base.CurrentStrucID);
            }
            else
            {
                result = ExceptionBLL.GetEmergencyAlarms(base.CurrentUserID);
            }
            //List<EmergencyAlarmInfoModel> list = new List<EmergencyAlarmInfoModel>()
            //{
            //    new EmergencyAlarmInfoModel(){
            //        ID=51,
            //        Address="异常地址",
            //        StartDateTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ms"),
            //        Status=0,
            //        StrucName="单位",
            //        TerminalCode="ABC0000",
            //        VehicleName="苏A12345"
            //    }
            //};
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 处理紧急报警
        /// </summary>
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("GetEmergencyAlarms")]
        public async Task<ActionResult> DealEmergencyAlarm(long id, string terminalCode, string dealInfo)
        {
            try
            {
                #region 调用WCF服务进行操作
                //查询终端对应的WCF地址
                var temp = ServerManagerBLL.GetTerminalServerInfo(terminalCode);
                if (temp.DataResult == null)
                {
                    return Json(new OperationResult() { Message = PromptInformation.DBError });
                }
                var serverInfo = temp.DataResult;
                //调用服务确认紧急报警
                OperationResultGeneralRep result = null;
                using (var client = base.GetTerminalOperationClient(serverInfo.WCFAddress))
                {
                    //使用终端号执行人工确认报警消息方法
                    result = await client.ConfirmAlarmAsync(terminalCode, new TerminalOperation.ManualConfirmAlarmData()
                    {
                        ManualConfirmAlarmType = TerminalOperation.ManualConfirmAlarmType.确认紧急报警,
                        SerialNumber = 0//在808协议中流水号0相当于确认所有相同类型报警
                    });
                }
                #endregion

                bool success = false;
                bool needUpdate = false;
                string msg = "{0}";

                //根据结果更新数据库
                //只有成功和超时需要更新处理记录
                if (result.State)//操作成功，看终端应答
                {
                    #region 操作成功
                    switch (result.ResultData.Value.Result)
                    {
                        case Asiatek.TMS.TerminalOperation.TerminalGeneralRepResult.成功或确认://需要更新
                            msg = string.Format(msg, PromptInformation.OperationSuccess);
                            needUpdate = success = true;
                            break;
                        case Asiatek.TMS.TerminalOperation.TerminalGeneralRepResult.失败:
                            msg = string.Format(msg, PromptInformation.OperationFailure);
                            break;
                        case Asiatek.TMS.TerminalOperation.TerminalGeneralRepResult.消息有误:
                            msg = string.Format(msg, PromptInformation.MessageWrong);
                            break;
                        case Asiatek.TMS.TerminalOperation.TerminalGeneralRepResult.不支持:
                            msg = string.Format(msg, PromptInformation.NotSupport);
                            break;
                    }
                    #endregion
                }
                else//操作失败
                {
                    #region 操作失败
                    switch (result.Code)
                    {
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.终端号格式有误:
                            msg = string.Format(msg, PromptInformation.TerminalCodeFormatError);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.终端号不存在:
                            msg = string.Format(msg, PromptInformation.TerminalCodeNotExist);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.未知错误:
                            msg = string.Format(msg, PromptInformation.UnknownError);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.终端离线:
                            msg = string.Format(msg, PromptInformation.TerminalIsOffline);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.终端通讯异常:
                            msg = string.Format(msg, PromptInformation.CommunicationError);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.等待应答超时://需要更新
                            msg = string.Format(msg, PromptInformation.Timeout);
                            needUpdate = true;
                            break;
                        default:
                            msg = string.Format(msg, PromptInformation.UnknownError);
                            break;
                    }
                    #endregion
                }

                #region 更新处理记录
                if (needUpdate)//更新异常记录的处理信息
                {
                    ExceptionBLL.UpdateEmergencyAlarm(new UpdateEmergencyAlarmModel()
                    {
                        ID = id,
                        DealUserID = base.UserIDForLog,
                        DealInfo = dealInfo,
                        LinkedServerName = serverInfo.LinkedServerName,
                        Timeout = !success
                    });
                }
                #endregion

                return Json(new OperationResult() { Success = success, Message = msg });
            }
            catch
            {
                return Json(new OperationResult() { Success = false, Message = PromptInformation.RemotingError });
            }
        }
        #endregion

        #region 实时异常
        /// <summary>
        /// 获取实时异常
        /// </summary>
        public ActionResult GetRealTimeExceptions()
        {
            List<RealTimeExceptionModel> result;
            if (base.VehicleViewMode)
            {
                result = ExceptionBLL.GetDefaultRealTimeExceptions(base.CurrentStrucID);
            }
            else
            {
                result = ExceptionBLL.GetRealTimeExceptions(base.CurrentUserID);
            }
            if (result == null)
            {
                result = new List<RealTimeExceptionModel>();
            }
            var query = from e in result select new { VN = e.VehicleName, SN = e.StrucName, TypeName = e.ExTypeName, StartDateTime = e.StartDateTime, VIN = e.VIN };
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 区域查车
        [AsiatekSubordinateFunction("Index")]
        public ActionResult GetRealTimeRectangleVehicles(GetRectangleRealTimeSingalsModel model)
        {
            List<RealTimeSignalModel> list;
            // 默认模式
            if (base.VehicleViewMode)
            {
                list = SignalBLL.GetDefaultRealTimeSingalsByRectangle(model, base.CurrentStrucID);
            }
            else
            {
                list = SignalBLL.GetRealTimeSingalsByRectangle(model, base.CurrentUserID);
            }
            if (list == null)
            {
                list = new List<RealTimeSignalModel>();
            }

            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new
                {
                    VID = item.VID,
                    VN = item.VehicleName,
                    SN = item.StrucName,
                    Speed = item.Speed,
                    VIN = item.VIN,
                    SignalTime = item.SignalDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Icon = item.Icon,
                    Address = item.Address == null ? string.Empty : item.Address,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    Direction = GetDirection(item.Direction),
                    Angle = item.Direction,
                    IsOnline = item.IsOnline == 1,
                    IsRunning = item.IsRunning == 1,
                    Mileage = item.Mileage,
                    OilHeight = item.OilHeight,
                    Temperature = item.Temperature,
                    ACCState = item.ACCState == true ? "ON" : "OFF",
                    RollerState = item.RollerState,
                    PlateColor = item.PlateColor,
                    VehicleType = item.VehicleType,
                    FlagState = (item.PressureFlag == true ? @DisplayText.PressureUnloading + "," : "") + (item.DoorsensorFlag == true ? @DisplayText.DoorOpen : ""),

                });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        [AsiatekSubordinateFunction("Index")]
        public ActionResult GetRealTimeCircleVehicles(GetCircleRealTimeSingalsModel model)
        {
            List<RealTimeSignalModel> list;
            if (base.VehicleViewMode)
            {
                list = SignalBLL.GetDefaultRealTimeSingalsByCircle(model, base.CurrentStrucID);
            }
            else
            {
                list = SignalBLL.GetRealTimeSingalsByCircle(model, base.CurrentUserID);
            }
            if (list == null)
            {
                list = new List<RealTimeSignalModel>();
            }
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new
                {
                    VID = item.VID,
                    VN = item.VehicleName,
                    SN = item.StrucName,
                    Speed = item.Speed,
                    VIN = item.VIN,
                    SignalTime = item.SignalDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Icon = item.Icon,
                    Address = item.Address == null ? string.Empty : item.Address,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    Direction = GetDirection(item.Direction),
                    Angle = item.Direction,
                    IsOnline = item.IsOnline == 1,
                    IsRunning = item.IsRunning == 1,
                    Mileage = item.Mileage,
                    OilHeight = item.OilHeight,
                    Temperature = item.Temperature,
                    ACCState = item.ACCState == true ? "ON" : "OFF",
                    RollerState = item.RollerState,
                    PlateColor = item.PlateColor,
                    VehicleType = item.VehicleType,
                    FlagState = (item.PressureFlag == true ? @DisplayText.PressureUnloading + "," : "") + (item.DoorsensorFlag == true ? @DisplayText.DoorOpen : ""),

                });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }

        [AsiatekSubordinateFunction("Index")]
        public ActionResult GetRealTimePolygonVehicles(string points)
        {
            List<RealTimePolygonVehiclesPoints> pointList = JsonConvert.DeserializeObject<List<RealTimePolygonVehiclesPoints>>(points);
            //过滤出包含多边形 最小正方形包含的数据
            int count = pointList.Count;
            double minLat = double.Parse(pointList[0].lat.ToString());
            double minLng = double.Parse(pointList[0].lng.ToString());
            double maxLat = minLat;
            double maxLng = minLng;
            for (int i = 0; i < count; i++)
            {
                double tmpLat = double.Parse(pointList[i].lat.ToString());
                double tmpLng = double.Parse(pointList[i].lng.ToString());
                if (tmpLat < minLat) { minLat = tmpLat; }
                if (tmpLng < minLng) { minLng = tmpLng; }
                if (tmpLat > maxLat) { maxLat = tmpLat; }
                if (tmpLng > maxLng) { maxLng = tmpLng; }
            }

            List<RealTimeSignalModel> list;
            // 默认模式
            if (base.VehicleViewMode)
            {
                list = SignalBLL.GetDefaultRealTimeSingalsByRectangle(new GetRectangleRealTimeSingalsModel()
                {
                    LatMax = maxLat,
                    LatMin = minLat,
                    LngMax = maxLng,
                    LngMin = minLng
                }, base.CurrentStrucID);
            }
            else
            {
                list = SignalBLL.GetRealTimeSingalsByRectangle(new GetRectangleRealTimeSingalsModel()
                {
                    LatMax = maxLat,
                    LatMin = minLat,
                    LngMax = maxLng,
                    LngMin = minLng
                }, base.CurrentUserID);
            }
            if (list == null)
            {
                list = new List<RealTimeSignalModel>();
            }
            List<dynamic> resultList = new List<dynamic>();
            foreach (var item in list)
            {
                resultList.Add(new
                {
                    VID = item.VID,
                    VN = item.VehicleName,
                    SN = item.StrucName,
                    Speed = item.Speed,
                    VIN = item.VIN,
                    SignalTime = item.SignalDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Icon = item.Icon,
                    Address = item.Address == null ? string.Empty : item.Address,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    Direction = GetDirection(item.Direction),
                    Angle = item.Direction,
                    IsOnline = item.IsOnline == 1,
                    IsRunning = item.IsRunning == 1,
                    Mileage = item.Mileage,
                    OilHeight = item.OilHeight,
                    Temperature = item.Temperature,
                    ACCState = item.ACCState == true ? "ON" : "OFF",
                    RollerState = item.RollerState,
                    PlateColor = item.PlateColor,
                    VehicleType = item.VehicleType,
                    FlagState = (item.PressureFlag == true ? @DisplayText.PressureUnloading + "," : "") + (item.DoorsensorFlag == true ? @DisplayText.DoorOpen : ""),

                });
            }
            return Json(resultList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 权限范围内的电子围栏
        [AsiatekSubordinateFunction("Index")]
        [AsiatekSubordinateFunction("Index", "Home", "HistoricalRoute")]
        public ActionResult GetEFMarkersInfo()
        {
            var list = ElectricFenceBLL.GetEFMarkersInfoBySID(base.CurrentStrucID);
            if (list == null)
            {
                list = new List<EFMarkerListModel>();
            }
            foreach (var item in list)
            {
                item.FenceTypeInfo = MGJH_TransportPointBLL.ChangeCoordinateSystem(item.FenceType, item.FenceTypeInfo, 2);  //将取出的车机坐标转成地图坐标显示
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 年检
        public ActionResult GetYearCheck()
        {
            List<YearCheckAlarmModel> result;
            if (base.VehicleViewMode)
            {
                result = ExceptionBLL.GetDefaultCheckAlarmModel(base.CurrentStrucID);
            }
            else
            {
                result = ExceptionBLL.GetYearCheckAlarm(base.CurrentUserID);
            }
            if (result == null)
            {
                result = new List<YearCheckAlarmModel>();
            }
            var query = from e in result
                        select new
                        {
                            StrucName = e.StrucName,
                            VehicleName = e.VehicleName,
                            CheckTime = (e.AnnualInspectionTime == DateTime.MinValue ? "" : (DateTime.Now.Year.ToString() + "-" + e.AnnualInspectionTime.Month.ToString() + "-" + e.AnnualInspectionTime.Day.ToString())),
                            CheckTime1 = (e.AnnualInspectionTime1 == DateTime.MinValue ? "" : (DateTime.Now.Year.ToString() + "-" + e.AnnualInspectionTime1.Month.ToString() + "-" + e.AnnualInspectionTime1.Day.ToString()))
                        };
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 车辆保养
        public ActionResult GetMaintenanceCheck()
        {
            List<MaintenanceCheckAlarmModel> result = new List<MaintenanceCheckAlarmModel>(); ;
            if (base.VehicleViewMode)
            {     //默认模式
                result = ExceptionBLL.GetDefaultMaintenanceCheckAlarmModel(base.CurrentStrucID);
            }
            else
            {     //自由模式
                result = ExceptionBLL.GetMaintenanceCheckAlarm(base.CurrentUserID);
            }
            if (result == null)
            {
                result = new List<MaintenanceCheckAlarmModel>();
            }
            var query = from e in result select new { StrucName = e.StrucName, VehicleName = e.VehicleName, ScheduleName = e.ScheduleName, RulesType = e.RulesType, AlarmInfo = e.AlarmInfo };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion


        #region 欢迎页
        /// <summary>
        /// 欢迎页(首页）
        /// </summary>
        public ActionResult Welcome()
        {
            ViewBag.Title = Asiatek.Resource.UIText.WelcomePage;
            return View();
        }
        #endregion

        #region 导航栏异常信息
        /// <summary>
        /// 获取需要处理的异常数量
        /// </summary>
        [AsiatekSubordinateFunction("NeedDealAlarmList")]
        public ActionResult GetNeedDealAlarmCount()
        {
            int? count = 0;
            if (base.VehicleViewMode)
            {
                count = ExceptionBLL.GetDefaultNeedDealAlarmCount(base.CurrentStrucID);
            }
            else
            {
                count = ExceptionBLL.GetNeedDealAlarmCount(base.CurrentUserID);
            }
            return Json(count == null ? 0 : count, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 需要处理的异常列表
        /// </summary>
        /// <returns></returns>
        public ActionResult NeedDealAlarmList()
        {
            var model = new SearchDataWithPagedDatas<NeedDealExceptionSearchModel, NeedDealExceptionListModel>();
            //默认的时间区间是一个月之前00：00：00到第二天0点
            var now = DateTime.Now;
            model.SearchModel = new NeedDealExceptionSearchModel();
            model.SearchModel.BeginDateTime = DateTime.Parse(now.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00"));
            model.SearchModel.EndDateTime = DateTime.Parse(now.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
            var endStates = new List<SelectListItem>(3)
            {
                new SelectListItem() { Text = UIText.All, Value = "-1",Selected=true},
                new SelectListItem() { Text = UIText.UnFinished, Value = "0"},
                new SelectListItem() { Text = UIText.Finished, Value = "1"},
            };
            model.SearchModel.EndStates = endStates;
            model.SearchModel.EndState = -1;
            if (base.VehicleViewMode)
            {
                model.PagedDatas = ExceptionBLL.GetDefaultNeedDealExceptionList(model.SearchModel, 1, this.PageSize, base.CurrentStrucID);
            }
            else
            {
                model.PagedDatas = ExceptionBLL.GetNeedDealExceptionList(model.SearchModel, 1, this.PageSize, base.CurrentUserID);
            }
            return PartialView("_NeedDealAlarmList", model);
        }


        [AsiatekSubordinateFunction("NeedDealAlarmList")]
        public ActionResult GetNeedDealAlarmList(NeedDealExceptionSearchModel model, int searchPage)
        {
            var result = new SearchDataWithPagedDatas<NeedDealExceptionSearchModel, NeedDealExceptionListModel>();
            var now = DateTime.Now;
            model.BeginDateTime = DateTime.Parse(now.AddMonths(-1).ToString("yyyy-MM-dd 00:00:00"));
            model.EndDateTime = DateTime.Parse(now.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
            result.SearchModel = model;
            if (base.VehicleViewMode)
            {
                result.PagedDatas = ExceptionBLL.GetDefaultNeedDealExceptionList(model, searchPage, this.PageSize, base.CurrentStrucID);
            }
            else
            {
                result.PagedDatas = ExceptionBLL.GetNeedDealExceptionList(model, searchPage, this.PageSize, base.CurrentUserID);
            }
            return PartialView("_NeedDealAlarmPagedGrid", result);
        }



        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("NeedDealAlarmList")]
        public ActionResult DealAlarm(long eid, string linkedServerName, string dealInfo)
        {
            var result = ExceptionBLL.DealAlarm(new DealAlarmModel()
            {
                EID = eid,
                DealInfo = dealInfo,
                DealUserID = base.UserIDForLog,
                LinkedServerName = linkedServerName
            });
            return Json(result);
        }



        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("NeedDealAlarmList")]
        public async Task<ActionResult> SendVolice(string terminalCode, string voliceMsg)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(terminalCode) || string.IsNullOrWhiteSpace(voliceMsg))
                {
                    return Json(new OperationResult() { Message = PromptInformation.UnknownError });
                }

                #region 调用WCF服务进行操作
                //查询终端对应的WCF地址
                var temp = ServerManagerBLL.GetTerminalServerInfo(terminalCode);
                if (temp.DataResult == null)
                {
                    return Json(new OperationResult() { Message = PromptInformation.DBError });
                }
                var serverInfo = temp.DataResult;
                if (string.IsNullOrWhiteSpace(serverInfo.WCFAddress))
                {
                    return Json(new OperationResult() { Message = PromptInformation.TerminalSetting_WCFError });
                }
                //调用服务下发声音
                OperationResultGeneralRep result = null;
                var realMessageText = System.Web.HttpUtility.HtmlDecode(voliceMsg.Trim());
                using (var client = base.GetTerminalOperationClient(serverInfo.WCFAddress))
                {
                    result = await client.SendTextInfoAsync(terminalCode, new TextInformationData()
                    {
                        Content = realMessageText,
                        TextInformationFlag = TextInformationFlag.紧急 | TextInformationFlag.终端TTS播读
                    });
                }
                #endregion

                bool success = false;
                string msg = "{0}";
                if (result.State)//操作成功，看终端应答
                {
                    #region 操作成功
                    switch (result.ResultData.Value.Result)
                    {
                        case Asiatek.TMS.TerminalOperation.TerminalGeneralRepResult.成功或确认://需要更新
                            msg = string.Format(msg, PromptInformation.OperationSuccess);
                            break;
                        case Asiatek.TMS.TerminalOperation.TerminalGeneralRepResult.失败:
                            msg = string.Format(msg, PromptInformation.OperationFailure);
                            break;
                        case Asiatek.TMS.TerminalOperation.TerminalGeneralRepResult.消息有误:
                            msg = string.Format(msg, PromptInformation.MessageWrong);
                            break;
                        case Asiatek.TMS.TerminalOperation.TerminalGeneralRepResult.不支持:
                            msg = string.Format(msg, PromptInformation.NotSupport);
                            break;
                    }
                    #endregion
                }
                else//操作失败
                {
                    #region 操作失败
                    switch (result.Code)
                    {
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.终端号格式有误:
                            msg = string.Format(msg, PromptInformation.TerminalCodeFormatError);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.终端号不存在:
                            msg = string.Format(msg, PromptInformation.TerminalCodeNotExist);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.未知错误:
                            msg = string.Format(msg, PromptInformation.UnknownError);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.终端离线:
                            msg = string.Format(msg, PromptInformation.TerminalIsOffline);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.终端通讯异常:
                            msg = string.Format(msg, PromptInformation.CommunicationError);
                            break;
                        case Asiatek.TMS.TerminalOperation.OperationResultCode.等待应答超时://需要更新
                            msg = string.Format(msg, PromptInformation.Timeout);
                            break;
                        default:
                            msg = string.Format(msg, PromptInformation.UnknownError);
                            break;
                    }
                    #endregion
                }
                return Json(new OperationResult() { Success = success, Message = msg });
            }
            catch
            {
                return Json(new OperationResult() { Success = false, Message = PromptInformation.RemotingError });
            }
        }

        #endregion

    }
}

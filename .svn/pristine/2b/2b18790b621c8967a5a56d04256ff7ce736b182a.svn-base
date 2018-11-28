using Asiatek.AjaxPager;
using Asiatek.BLL.MSSQL;
using Asiatek.Common;
using Asiatek.Model;
using Asiatek.Model.TerminalSetting;
using Asiatek.Resource;
using Asiatek.TMS.Controllers;
using Asiatek.TMS.Filters;
using Asiatek.TMS.TerminalOperation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    /// <summary>
    /// 终端设置控制器
    /// </summary>
    public class TerminalsSettingController : BaseController
    {
        ///// <summary>
        ///// 请求wcf地址后缀
        ///// </summary>
        //public const string WCFTPSAddress = "/ITerminalOperation";

        /// <summary>
        /// 进行终端操作
        /// </summary>
        /// <typeparam name="TSendModel">下发的模型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="listWCF">wcf地址集</param>
        /// <param name="model">下发类型的对象模型</param>
        /// <param name="func">实际执行的方法委托</param>
        /// <returns></returns>
        private async Task<IEnumerable<TResult>> DoOperation<TSendModel, TResult>
            (List<TerminalWCfModel> listWCF, TSendModel model, Func<TerminalOperationClient, string, TSendModel, Task<TResult>> func)
        {
            listWCF.OrderBy(obj => obj.WCfAddress);
            var tasks = new List<Task<TResult>>(listWCF.Count);
            var currentWCFAddr = listWCF.First().WCfAddress;
            var client = base.GetTerminalOperationClient(currentWCFAddr);
            foreach (var entity in listWCF)
            {
                // 如果集合中WCfAddress 和currentWCFAddr不一致 则重新实例client
                if (entity.WCfAddress != currentWCFAddr)
                {
                    currentWCFAddr = entity.WCfAddress;
                    client = base.GetTerminalOperationClient(currentWCFAddr);
                }
                tasks.Add(func(client, entity.TerminalCode, model));
            }
            // 所有任务完成时获取返回结果
            return await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 检查终端所属服务器WCF信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="result"></param>
        /// <param name="wcfList"></param>
        /// <returns></returns>
        private bool CheckWCF(List<VehiclesModel> list, out ActionResult result, out List<TerminalWCfModel> wcfList)
        {
            result = null;
            wcfList = GetWcfAddress(list);
            if (wcfList == null || wcfList.Count <= 0)
            {
                result = Json(new { Success = false, Message = PromptInformation.TerminalSetting_WCFError });
                return false;
            }
            return true;
        }

        #region 文本消息下发

        #region 下发文本消息视图
        /// <summary>
        /// 下发文本消息视图
        /// </summary>
        /// <returns></returns>
        public ActionResult TextMessagesIssued()
        {
            return InitPage("TextMessagesIssued");
        }
        #endregion



        #region 下发文本消息
        ///// <summary>
        ///// 将前台输入的消息下发给车机
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost, ValidateAntiForgeryToken]
        //[AsiatekSubordinateFunction("TextMessagesIssued")]
        //public async Task<ActionResult> SendTextMessage(SendTextMessageModel model)
        //{
        //    // 根据终端号获取终端所属服务器的wcf链接地址
        //    var listWCF = GetWcfAddress(model.ListVehicles);
        //    if (listWCF == null || listWCF.Count <= 0)
        //    {
        //        return Json(new { Success = false, Message = "终端所属WCF地址暂未设置，请先设置！" });
        //    }

        //    // 记录错误信息
        //    string errorMsg = string.Empty;
        //    // 调用wcf服务 如果终端属于同一个wcf地址 那么可以公用一个client实例
        //    try
        //    {
        //        //存储并行任务的集合
        //        var tasks = new List<Task<OperationResultGeneralRep>>(listWCF.Count);
        //        var firstWCFAddress = listWCF.First().WCfAddress;
        //        WSHttpBinding wshb = new WSHttpBinding();//使用协议与服务端相同
        //        wshb.Security.Mode = SecurityMode.None; //安全级别
        //        EndpointAddress addr = new EndpointAddress(firstWCFAddress + WCFTPSAddress);
        //        TerminalOperationClient client = new TerminalOperationClient(wshb, addr);

        //        foreach (var entity in listWCF)
        //        {
        //            // 如果集合中WCfAddress 和firstWCFAddress不一致 则重新实例client
        //            if (entity.WCfAddress != firstWCFAddress)
        //            {
        //                firstWCFAddress = entity.WCfAddress;
        //                addr = new EndpointAddress(firstWCFAddress + WCFTPSAddress);
        //                client = new TerminalOperationClient(wshb, addr);
        //            }
        //            tasks.Add(SyncSendTextInfo(client, entity.TerminalCode, model));
        //        }

        //        // 所有任务完成时获取返回结果
        //        var syncResult = await Task.WhenAll(tasks).ConfigureAwait(false);
        //        int index = 0;
        //        string valuesSql = string.Empty;
        //        string setInfo = "Flags:" + (TextInformationFlag)model.Flags + ",MessageText:" + model.MessageText;
        //        string wanIP = GetRemoteAddress();
        //        DateTime setDTime = DateTime.Now;

        //        foreach (var item in syncResult)
        //        {
        //            string plateNum = listWCF[index].PlateNum;
        //            string resultResponse = "Code:" + (int)(OperationResultCode)item.Code + ";Message:" + item.Message
        //                                           + ";ResultData:" + item.ResultData;

        //            // 记录日志 文本消息下发 这里拼接sql语句 批量插入   
        //            valuesSql += string.Format("('{0}','{1}',{2},'{3}',{4},'{5}','{6}',{7},'{8}'),",
        //                                                   listWCF[index].TerminalCode, plateNum, (byte)TerminalSettingTypeEnum.TextMessage,
        //                                                    setInfo, item.State ? 1 : 0, resultResponse, wanIP, base.CurrentUserID, setDTime);
        //            if (!item.State)
        //            {
        //                errorMsg += string.Format("<div style='font-weight:bolder;'>车牌号：{0}{1}，失败原因：{2}{3}</div>", plateNum, "<br/>",
        //                                                             item.Code.ToString(), "<hr/>");
        //            }
        //            index++;
        //        }
        //        // 批量插入日志
        //        TerminalSettingsBLL.BatchInsertTerminalOperationsLog(valuesSql.TrimEnd(','));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false, Message = ex.Message });
        //    }

        //    if (string.IsNullOrEmpty(errorMsg))
        //    {
        //        return Json(new { Success = true, Message = "文本消息下发成功" });
        //    }
        //    else
        //    {
        //        return Json(new { Success = false, Message = errorMsg });
        //    }

        //}

        /// <summary>
        /// 将前台输入的消息下发给车机
        /// </summary>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("TextMessagesIssued")]
        public async Task<ActionResult> SendTextMessage(SendTextMessageModel model)
        {
            ActionResult result = null;
            List<TerminalWCfModel> wcfList = null;
            if (!this.CheckWCF(model.ListVehicles, out result, out wcfList))
            {
                return result;
            }
            //升级指令包含<>,但是为了安全性是不允许提交<>这样的html字符的，虽然可以通过一下方式让MVC项目的请求允许通过html字符，但是不合适，所以
            //这里呢要求数据本身是html编码，到后台再转回成已解码的字符串
            //比如 升级指令 <EXT87DDE2B7AE0B45FX-GPS000000000000000000PP> ，在文本框里输入&lt;EXT87DDE2B7AE0B45FX-GPS000000000000000000PP&gt;
            //到后台再解码成<>就行了
            /**
             * web.config里面加上

                <system.web>
                    <httpRuntime requestValidationMode="2.0" />
                </system.web>
             */
            var realMessageText = System.Web.HttpUtility.HtmlDecode(model.MessageText);
            model.MessageText = realMessageText;

            // 记录错误信息
            string errorMsg = string.Empty;
            try
            {
                // 所有任务完成时获取返回结果
                var syncResult = await this.DoOperation<SendTextMessageModel, OperationResultGeneralRep>(wcfList, model, SyncSendTextInfo).ConfigureAwait(false);
                int index = 0;
                string valuesSql = string.Empty;
                string setInfo = "Flags:" + (TextInformationFlag)model.Flags + ",MessageText:" + model.MessageText;
                string wanIP = GetRemoteAddress();
                DateTime setDTime = DateTime.Now;

                foreach (var item in syncResult)
                {
                    string plateNum = wcfList[index].PlateNum;
                    string resultResponse = "Code:" + (int)(OperationResultCode)item.Code + ";Message:" + item.Message;

                    // 记录日志 文本消息下发 这里拼接sql语句 批量插入   
                    valuesSql += string.Format("('{0}','{1}',{2},'{3}',{4},'{5}','{6}',{7},'{8}'),",
                                                           wcfList[index].TerminalCode, plateNum, (byte)TerminalSettingTypeEnum.TextMessage,
                                                            setInfo, item.State ? 1 : 0, resultResponse, wanIP, base.CurrentUserID, setDTime);
                    if (!item.State)
                    {
                        //errorMsg += string.Format("<div style='font-weight:bolder;'>" + UIText.TerminalSetting_PlateNumber + "：{0}{1}，" + PromptInformation.FailReason+ "：{2}{3}</div>", plateNum, "<br/>",
                        //                                             item.Code.ToString(), "<hr/>");
                        errorMsg += ErrorMesage(plateNum, item.Code.ToString());
                    }
                    index++;
                }
                // 批量插入日志
                TerminalSettingsBLL.BatchInsertTerminalOperationsLog(valuesSql.TrimEnd(','));
            }
            catch (Exception ex)
            {
                LogHelper.TerminalSettingsErrorLog("下发文本消息异常：" + ex.Message);
                return Json(new { Success = false, Message = PromptInformation.RemotingError });
            }

            if (string.IsNullOrEmpty(errorMsg))
            {
                return Json(new { Success = true, Message = PromptInformation.TerminalSetting_SendTextMessageSuccess });
            }
            else
            {
                return Json(new { Success = false, Message = errorMsg });
            }

        }


        private Task<OperationResultGeneralRep> SyncSendTextInfo(TerminalOperationClient client, string terminalCode, SendTextMessageModel model)
        {
            return client.SendTextInfoAsync(terminalCode, new TextInformationData()
            {
                Content = model.MessageText.Trim(),
                TextInformationFlag = (TextInformationFlag)model.Flags
            });

            //return client.sendtextinfoasync(terminalcode, new textinformationdata()
            //{
            //    content = model.messagetext.trim(),
            //    textinformationflag = (textinformationflag)model.flags
            //});
            //return Task<OperationResultGeneralRep>.Run(() =>
            //{
            //    return client.SendTextInfo(terminalCode, new TextInformationData()
            //    {
            //        Content = model.MessageText.Trim(),
            //        TextInformationFlag = (TextInformationFlag)model.Flags
            //    });
            //});
        }
        #endregion

        #endregion

        #region 终端设置下发

        #region 终端设置下发 初始化视图
        /// <summary>
        /// 终端设置下发 初始化视图
        /// </summary>
        /// <returns></returns>
        public ActionResult TerminalSettingsIssued()
        {
            // 获取车牌颜色列表
            var listPlateColors = PlateColorBLL.GetPlateColors();
            ViewBag.SubViewData = listPlateColors;
            return InitPage("TerminalSettingsIssued");
        }
        #endregion

        #region 终端设置下发
        //[HttpPost, ValidateAntiForgeryToken]
        //[AsiatekSubordinateFunction("TerminalSettingsIssued")]
        //public async Task<ActionResult> UpdateTerminalSetting(TerminalSettingsIssuedModel model)
        //{
        //    // 根据终端号获取终端所属服务器的wcf链接地址
        //    var listWCF = GetWcfAddress(model.ListVehicles);
        //    if (listWCF == null || listWCF.Count <= 0)
        //    {
        //        return Json(new { Success = false, Message = "终端所属WCF地址暂未设置，请先设置！" });
        //    }
        //    // 记录错误信息
        //    string errorMsg = string.Empty;
        //    bool updateTerminal = true;
        //    // 调用wcf服务 如果终端属于同一个wcf地址 那么可以公用一个client实例
        //    try
        //    {
        //        //存储并行任务的集合
        //        var tasks = new List<Task<OperationResultGeneralRep>>(listWCF.Count);
        //        var firstWCFAddress = listWCF.First().WCfAddress;
        //        WSHttpBinding wshb = new WSHttpBinding();//使用协议与服务端相同
        //        wshb.Security.Mode = SecurityMode.None; //安全级别
        //        EndpointAddress addr = new EndpointAddress(firstWCFAddress + WCFTPSAddress);
        //        TerminalOperationClient client = new TerminalOperationClient(wshb, addr);

        //        foreach (var entity in listWCF)
        //        {
        //            // 如果集合中WCfAddress 和firstWCFAddress不一致 则重新实例client
        //            if (entity.WCfAddress != firstWCFAddress)
        //            {
        //                firstWCFAddress = entity.WCfAddress;
        //                addr = new EndpointAddress(firstWCFAddress + WCFTPSAddress);
        //                client = new TerminalOperationClient(wshb, addr);
        //            }
        //            tasks.Add(SyncSetTerminalParas(client, entity.TerminalCode, model));
        //        }
        //        // 所有任务完成时获取返回结果
        //        var syncResult = await Task.WhenAll(tasks).ConfigureAwait(false);
        //        int index = 0;
        //        string valuesSql = string.Empty;
        //        string setInfo = GetProperties<TerminalSettingsIssuedModel>(model);
        //        string wanIP = GetRemoteAddress();
        //        DateTime setDTime = DateTime.Now;
        //        List<string> listTerminalCode = new List<string>();
        //        foreach (var item in syncResult)
        //        {
        //            string plateNum = listWCF[index].PlateNum;
        //            string terminalCode = listWCF[index].TerminalCode;
        //            string resultResponse = "Code:" + (int)(OperationResultCode)item.Code + ";Message:" + item.Message
        //                                           + ";ResultData:" + item.ResultData;

        //            // 记录日志 文本消息下发 这里拼接sql语句 批量插入   
        //            valuesSql += string.Format("('{0}','{1}',{2},'{3}',{4},'{5}','{6}',{7},'{8}'),",
        //                                                   terminalCode, plateNum, (byte)TerminalSettingTypeEnum.TerminalSetup_Update,
        //                                                    setInfo, item.State ? 1 : 0, resultResponse, wanIP, base.CurrentUserID, setDTime);
        //            if (!item.State)
        //            {
        //                errorMsg += string.Format("<div style='font-weight:bolder;'>车牌号：{0}{1}，失败原因：{2}{3}</div>", plateNum, "<br/>",
        //                                                             item.Code.ToString(), "<hr/>");
        //            }
        //            else
        //            {
        //                //这里记录修改成功的终端值
        //                listTerminalCode.Add(terminalCode);
        //            }
        //            index++;
        //        }

        //        //如果下发终端成功了 则修改TMS表中对应终端的数据
        //        if (listTerminalCode.Count > 0)
        //        {
        //            TerminalSettingsSectionModel sectionModel = new TerminalSettingsSectionModel()
        //            {
        //                最高速度 = model.最高速度,
        //                超速持续时间 = model.最高速度,
        //                连续驾驶时间门限 = model.最高速度,
        //                最小休息时间 = model.最小休息时间,
        //                当天累计驾驶时间门限 = model.当天累计驾驶时间门限,
        //                最长停车时间 = model.最长停车时间
        //            };
        //            updateTerminal = TerminalBLL.UpdateTerminals(listTerminalCode, sectionModel);
        //        }
        //        // 批量插入日志
        //        TerminalSettingsBLL.BatchInsertTerminalOperationsLog(valuesSql.TrimEnd(','));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false, Message = ex.Message });
        //    }

        //    if (!updateTerminal)
        //    {
        //        return Json(new { Success = false, Message = "终端设置下发失败，请重新下发" });
        //    }
        //    if (string.IsNullOrEmpty(errorMsg))
        //    {
        //        return Json(new { Success = true, Message = "终端设置下发成功" });
        //    }
        //    else
        //    {
        //        return Json(new { Success = false, Message = errorMsg });
        //    }
        //}

        [HttpPost, ValidateAntiForgeryToken]
        [AsiatekSubordinateFunction("TerminalSettingsIssued")]
        public async Task<ActionResult> UpdateTerminalSetting(TerminalSettingsIssuedModel model)
        {
            ActionResult result = null;
            List<TerminalWCfModel> wcfList = null;
            if (!this.CheckWCF(model.ListVehicles, out result, out wcfList))
            {
                return result;
            }
            // 记录错误信息
            string errorMsg = string.Empty;
            bool updateTerminal = true;
            // 调用wcf服务 如果终端属于同一个wcf地址 那么可以公用一个client实例
            try
            {
                var syncResult = await this.DoOperation<TerminalSettingsIssuedModel, OperationResultGeneralRep>(wcfList, model, SyncSetTerminalParas).ConfigureAwait(false);
                int index = 0;
                string valuesSql = string.Empty;
                string setInfo = GetProperties<TerminalSettingsIssuedModel>(model);
                string wanIP = GetRemoteAddress();
                DateTime setDTime = DateTime.Now;
                List<string> listTerminalCode = new List<string>();
                foreach (var item in syncResult)
                {
                    string plateNum = wcfList[index].PlateNum;
                    string terminalCode = wcfList[index].TerminalCode;
                    string resultResponse = "Code:" + (int)(OperationResultCode)item.Code + ";Message:" + item.Message;

                    // 记录日志 文本消息下发 这里拼接sql语句 批量插入   
                    valuesSql += string.Format("('{0}','{1}',{2},'{3}',{4},'{5}','{6}',{7},'{8}'),",
                                                           terminalCode, plateNum, (byte)TerminalSettingTypeEnum.TerminalSetup_Update,
                                                            setInfo, item.State ? 1 : 0, resultResponse, wanIP, base.CurrentUserID, setDTime);
                    if (!item.State)
                    {
                        //errorMsg += string.Format("<div style='font-weight:bolder;'>" + UIText.TerminalSetting_PlateNumber + 
                        //                                              "：{0}{1}，"+PromptInformation.FailReason+"：{2}{3}</div>", plateNum, "<br/>",
                        //                                             item.Code.ToString(), "<hr/>");
                        errorMsg += ErrorMesage(plateNum, item.Code.ToString());
                    }
                    else
                    {
                        //这里记录修改成功的终端值
                        listTerminalCode.Add(terminalCode);
                    }
                    index++;
                }

                //如果下发终端成功了 则修改TMS表中对应终端的数据
                if (listTerminalCode.Count > 0)
                {
                    TerminalSettingsSectionModel sectionModel = new TerminalSettingsSectionModel()
                    {
                        最高速度 = model.最高速度,
                        超速持续时间 = model.最高速度,
                        连续驾驶时间门限 = model.最高速度,
                        最小休息时间 = model.最小休息时间,
                        当天累计驾驶时间门限 = model.当天累计驾驶时间门限,
                        最长停车时间 = model.最长停车时间
                    };
                    updateTerminal = TerminalBLL.UpdateTerminals(listTerminalCode, sectionModel);
                }
                // 批量插入日志
                TerminalSettingsBLL.BatchInsertTerminalOperationsLog(valuesSql.TrimEnd(','));
            }
            catch (Exception ex)
            {
                LogHelper.TerminalSettingsErrorLog("下发终端设置异常：" + ex.Message);
                return Json(new { Success = false, Message = PromptInformation.RemotingError });
            }

            if (!updateTerminal)
            {
                return Json(new { Success = false, Message = PromptInformation.TerminalSetting_UpdateTerminalSettingFail });
            }
            if (string.IsNullOrEmpty(errorMsg))
            {
                return Json(new { Success = true, Message = PromptInformation.TerminalSetting_UpdateTerminalSettingSuccess });
            }
            else
            {
                return Json(new { Success = false, Message = errorMsg });
            }
        }

        private Task<OperationResultGeneralRep> SyncSetTerminalParas(TerminalOperationClient client, string terminalCode, TerminalSettingsIssuedModel model)
        {
            TerminalParasData paraItems = new TerminalParasData();
            if (model.车牌颜色.HasValue)
            {
                paraItems.车牌颜色 = (PlateColor)model.车牌颜色.Value;
            }
            paraItems.终端心跳发送间隔 = model.终端心跳发送间隔;
            paraItems.主服务器IP地址或域名 = model.主服务器IP地址或域名;
            paraItems.备份服务器IP地址或域名 = model.备份服务器IP地址或域名;
            paraItems.服务器TCP端口 = model.服务器TCP端口;
            paraItems.休眠时汇报时间间隔 = model.休眠时汇报时间间隔;
            paraItems.紧急报警时汇报时间间隔 = model.紧急报警时汇报时间间隔;
            paraItems.缺省时间汇报间隔 = model.缺省时间汇报间隔;
            paraItems.最高速度 = model.最高速度;
            paraItems.超速持续时间 = model.超速持续时间;
            paraItems.连续驾驶时间门限 = model.连续驾驶时间门限;
            paraItems.当天累计驾驶时间门限 = model.当天累计驾驶时间门限;
            paraItems.最小休息时间 = model.最小休息时间;
            paraItems.最长停车时间 = model.最长停车时间;
            paraItems.超速报警预警差值 = model.超速报警预警差值;
            paraItems.疲劳驾驶预警差值 = model.疲劳驾驶预警差值;
            paraItems.车辆里程表读数 = model.车辆里程表读数;
            paraItems.公安交通管理部门颁发的机动车号牌 = model.公安交通管理部门颁发的机动车号牌;

            return client.SetTerminalParasAsync(terminalCode, new TerminalParasSettingData()
            {
                ParaItems = paraItems
            });

        }
        #endregion

        #endregion

        #region 终端设置读取

        #region 终端设置读取 初始化视图
        /// <summary>
        /// 终端设置下发 初始化视图
        /// </summary>
        /// <returns></returns>
        public ActionResult TerminalSettingsRead()
        {
            return InitPage("TerminalSettingsRead", false);
        }
        #endregion

        #region 终端设置读取
        //[HttpPost]
        //[AsiatekSubordinateFunction("TerminalSettingsRead")]
        //public async Task<ActionResult> QueryTerminalFlags(TerminalSettingsReadModel model)
        //{
        //    // 根据终端号获取终端所属服务器的wcf链接地址
        //    var listWCF = GetWcfAddress(model.ListVehicles);
        //    if (listWCF == null || listWCF.Count <= 0)
        //    {
        //        return Json(new { Success = false, Message = "终端所属WCF地址暂未设置，请先设置！" });
        //    }
        //    // 记录错误信息
        //    string errorMsg = string.Empty;
        //    OperationResultQueryTerminalParaRespData syncResult;
        //    try
        //    {
        //        var entity = listWCF[0];
        //        WSHttpBinding wshb = new WSHttpBinding();//使用协议与服务端相同
        //        wshb.Security.Mode = SecurityMode.None; //安全级别
        //        EndpointAddress addr = new EndpointAddress(entity.WCfAddress + WCFTPSAddress);
        //        TerminalOperationClient client = new TerminalOperationClient(wshb, addr);

        //        var checkedParamArrayLength = model.CheckedParamArray.Length;
        //        TerminalParaID[] paramIDs = new TerminalParaID[checkedParamArrayLength];
        //        var checkedParamArrayData = model.CheckedParamArray;
        //        string checkedTerminalPara = "查询内容:";
        //        for (int i = 0; i < checkedParamArrayLength; i++)
        //        {
        //            paramIDs[i] = (TerminalParaID)checkedParamArrayData[i];
        //            checkedTerminalPara += (TerminalParaID)checkedParamArrayData[i] + ",";
        //        }

        //        syncResult = await SyncQuerySpecifiedTerminalParas(client, entity.TerminalCode, paramIDs);
        //        string setInfo = checkedTerminalPara.TrimEnd(',');
        //        //string setInfo = GetProperties<TerminalSettingsReadModel>(model);
        //        string plateNum = entity.PlateNum;
        //        string resultResponse = "Code:" + (int)(OperationResultCode)syncResult.Code + ";Message:" + syncResult.Message
        //                                       + ";ResultData:" + syncResult.ResultData;

        //        // 记录日志 文本消息下发 这里拼接sql语句 批量插入   
        //        string valuesSql = string.Format("('{0}','{1}',{2},'{3}',{4},'{5}','{6}',{7},'{8}'),",
        //                                               entity.TerminalCode, plateNum, (byte)TerminalSettingTypeEnum.TerminalSetup_Query,
        //                                                setInfo, syncResult.State ? 1 : 0, resultResponse, GetRemoteAddress(), base.CurrentUserID, DateTime.Now);
        //        if (!syncResult.State)
        //        {
        //            errorMsg += string.Format("<div style='font-weight:bolder;'>车牌号：{0}{1}，失败原因：{2}{3}</div>", plateNum, "<br/>",
        //                                                         syncResult.Code.ToString(), "<hr/>");
        //        }
        //        // 插入日志
        //        TerminalSettingsBLL.BatchInsertTerminalOperationsLog(valuesSql.TrimEnd(','));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Success = false, Message = ex.Message });
        //    }

        //    if (string.IsNullOrEmpty(errorMsg))
        //    {
        //        #region 返回数据
        //        var responseData = syncResult.ResultData.ParaItems;
        //        string plateColor = null;
        //        if (responseData.车牌颜色.HasValue)
        //        {
        //            plateColor = responseData.车牌颜色.ToString();
        //        }

        //        dynamic result = new ExpandoObject();
        //        result.HeartbeatInterval = responseData.终端心跳发送间隔;
        //        result.PrimaryServerIP = responseData.主服务器IP地址或域名;
        //        result.BackupServerIP = responseData.备份服务器IP地址或域名;
        //        result.ServerTCPPort = responseData.服务器TCP端口;
        //        result.StateRepotrIntervalSleep = responseData.休眠时汇报时间间隔;
        //        result.StateRepotrIntervalUrgent = responseData.紧急报警时汇报时间间隔;
        //        result.StateRepotrIntervalGeneral = responseData.缺省时间汇报间隔;
        //        result.MaximumSpeed = responseData.最高速度;
        //        result.OverspeedCheckTime = responseData.超速持续时间;
        //        result.MaximumDrivingTime = responseData.连续驾驶时间门限;
        //        result.MaximumDrivingTimeToday = responseData.当天累计驾驶时间门限;
        //        result.MinimumBreakTime = responseData.最小休息时间;
        //        result.MaximumStoppingTime = responseData.最长停车时间;
        //        result.DifferenceOfOverspeedValues = responseData.超速报警预警差值;
        //        result.DifferenceOfFatigueDrivingTime = responseData.疲劳驾驶预警差值;
        //        result.TotalODO = responseData.车辆里程表读数;
        //        result.MotorNumberPlate = responseData.公安交通管理部门颁发的机动车号牌;
        //        result.LicensePlateColor = plateColor;
        //        #endregion
        //        return Json(new { Success = true, Message = JsonConvert.SerializeObject(result) });
        //    }
        //    else
        //    {
        //        return Json(new { Success = false, Message = errorMsg });
        //    }
        //}

        [HttpPost]
        [AsiatekSubordinateFunction("TerminalSettingsRead")]
        public async Task<ActionResult> QueryTerminalFlags(TerminalSettingsReadModel model)
        {
            // 根据终端号获取终端所属服务器的wcf链接地址
            ActionResult actionResult = null;
            List<TerminalWCfModel> wcfList = null;
            if (!this.CheckWCF(model.ListVehicles, out actionResult, out wcfList))
            {
                return actionResult;
            }
            // 记录错误信息
            string errorMsg = string.Empty;
            OperationResultQueryTerminalParaRespData syncResult;
            try
            {
                var entity = wcfList[0];
                var checkedParamArrayLength = model.CheckedParamArray.Length;
                TerminalParaID[] paramIDs = new TerminalParaID[checkedParamArrayLength];
                var checkedParamArrayData = model.CheckedParamArray;
                string checkedTerminalPara = PromptInformation.QueryContent + ":";
                for (int i = 0; i < checkedParamArrayLength; i++)
                {
                    paramIDs[i] = (TerminalParaID)checkedParamArrayData[i];
                    checkedTerminalPara += (TerminalParaID)checkedParamArrayData[i] + ",";
                }

                var tempResult = await this.DoOperation<TerminalParaID[], OperationResultQueryTerminalParaRespData>(wcfList, paramIDs, SyncQuerySpecifiedTerminalParas);
                syncResult = tempResult.Single();
                string setInfo = checkedTerminalPara.TrimEnd(',');
                string plateNum = entity.PlateNum;
                string resultResponse = "Code:" + (int)(OperationResultCode)syncResult.Code + ";Message:" + syncResult.Message;

                // 记录日志 文本消息下发 这里拼接sql语句 批量插入   
                string valuesSql = string.Format("('{0}','{1}',{2},'{3}',{4},'{5}','{6}',{7},'{8}'),",
                                                       entity.TerminalCode, plateNum, (byte)TerminalSettingTypeEnum.TerminalSetup_Query,
                                                        setInfo, syncResult.State ? 1 : 0, resultResponse, GetRemoteAddress(), base.CurrentUserID, DateTime.Now);
                if (!syncResult.State)
                {
                    //errorMsg += string.Format("<div style='font-weight:bolder;'>{0}：{1}{2}{3}：{4}{5}</div>",
                    //    UIText.TerminalSetting_PlateNumber, plateNum, "<br/>", PromptInformation.FailReason,syncResult.Code.ToString(), "<hr/>");
                    errorMsg += ErrorMesage(plateNum, syncResult.Code.ToString());
                }
                // 插入日志
                TerminalSettingsBLL.BatchInsertTerminalOperationsLog(valuesSql.TrimEnd(','));
            }
            catch (Exception ex)
            {
                LogHelper.TerminalSettingsErrorLog("终端设置读取异常：" + ex.Message);
                return Json(new { Success = false, Message = PromptInformation.RemotingError });
            }

            if (string.IsNullOrEmpty(errorMsg))
            {
                #region 返回数据
                var responseData = syncResult.ResultData.ParaItems;
                string plateColor = null;
                if (responseData.车牌颜色.HasValue)
                {
                    plateColor = responseData.车牌颜色.ToString();
                }

                dynamic result = new ExpandoObject();
                result.HeartbeatInterval = responseData.终端心跳发送间隔;
                result.PrimaryServerIP = responseData.主服务器IP地址或域名;
                result.BackupServerIP = responseData.备份服务器IP地址或域名;
                result.ServerTCPPort = responseData.服务器TCP端口;
                result.StateRepotrIntervalSleep = responseData.休眠时汇报时间间隔;
                result.StateRepotrIntervalUrgent = responseData.紧急报警时汇报时间间隔;
                result.StateRepotrIntervalGeneral = responseData.缺省时间汇报间隔;
                result.MaximumSpeed = responseData.最高速度;
                result.OverspeedCheckTime = responseData.超速持续时间;
                result.MaximumDrivingTime = responseData.连续驾驶时间门限;
                result.MaximumDrivingTimeToday = responseData.当天累计驾驶时间门限;
                result.MinimumBreakTime = responseData.最小休息时间;
                result.MaximumStoppingTime = responseData.最长停车时间;
                result.DifferenceOfOverspeedValues = responseData.超速报警预警差值;
                result.DifferenceOfFatigueDrivingTime = responseData.疲劳驾驶预警差值;
                result.TotalODO = responseData.车辆里程表读数;
                result.MotorNumberPlate = responseData.公安交通管理部门颁发的机动车号牌;
                result.LicensePlateColor = plateColor;
                #endregion
                return Json(new { Success = true, Message = JsonConvert.SerializeObject(result) });
            }
            else
            {
                return Json(new { Success = false, Message = errorMsg });
            }
        }

        private Task<OperationResultQueryTerminalParaRespData> SyncQuerySpecifiedTerminalParas(TerminalOperationClient client, string terminalCode, TerminalParaID[] terminalParaIDs)
        {
            return client.QuerySpecifiedTerminalParasAsync(terminalCode, new QuerySpecifiedTerminalParasData()
            {
                TerminalParaIDs = terminalParaIDs
            });
        }
        #endregion

        #endregion

        #region 终端日志下发查询

        #region 初始化查询页面
        public ActionResult LogsSetting()
        {
            SearchDataWithPagedDatas<TerminalSettingLogSearchModel, TerminalSettingLogListModel> model
                 = new SearchDataWithPagedDatas<TerminalSettingLogSearchModel, TerminalSettingLogListModel>();
            model.SearchModel = new TerminalSettingLogSearchModel();
            model.PagedDatas = new AsiatekPagedList<TerminalSettingLogListModel>(new List<TerminalSettingLogListModel>(), 1, this.PageSize, 1);
            // 首次进来时 不显示 “查无数据！”的提示 
            ViewBag.IsShow = 0;
            return PartialView("LogsSetting", model);
        }
        #endregion

        #region 查询操作
        [AsiatekSubordinateFunction("LogsSetting")]
        public ActionResult QueryLogs(TerminalSettingLogSearchModel model, int searchPage)
        {
            SearchDataWithPagedDatas<TerminalSettingLogSearchModel, TerminalSettingLogListModel> result
                       = new SearchDataWithPagedDatas<TerminalSettingLogSearchModel, TerminalSettingLogListModel>();
            // 根据查询条件查询数据 若没有符合条件的数据 则显示 “查无数据！”的提示 
            ViewBag.IsShow = 1;
            result.SearchModel = model;
            result.PagedDatas = TerminalSettingsBLL.GetPagedTerminalSettingLog(model, searchPage, this.PageSize);
            return PartialView("LogsPagedGrid", result);
        }
        #endregion

        #region 详情
        [AsiatekSubordinateFunction("LogsSetting")]
        public ActionResult LogsDetail(long id)
        {
            var result = TerminalSettingsBLL.GetTerminalSettingLogDetailByID(id);
            if (result.DataResult == null)
            {
                return Content(result.Message);
            }
            return PartialView("LogsDetail", result.DataResult);
        }
        #endregion

        #endregion

        #region 公共方法

        #region 根据相关参数获取用户所属车辆列表
        /// <summary>
        /// 根据使用单位，车牌号或终端号获取车辆信息
        /// </summary>
        /// <param name="strucName">使用单位</param>
        /// <param name="plateNumOrTerminalCode">车牌号或终端号</param>
        /// <returns></returns>
        [AsiatekSubordinateFunction("Index", "Home", "TerminalSetting")]
        public ActionResult GetVehiclesListByCurrentUserID(string strucName, string plateNumOrTerminalCode)
        {
            StrucVehiclesListModel[] list = InternalGetVehiclesListByCurrentUserID(strucName, plateNumOrTerminalCode);
            List<dynamic> jsonObject = new List<dynamic>();
            for (int i = 0; i != list.Length; i++)
            {
                dynamic struc = new ExpandoObject();
                struc.state = new ExpandoObject();
                struc.state.expanded = false;
                struc.text = list[i].CompanyName;
                var vehicles = new List<dynamic>();
                for (int j = 0; j != list[i].Vehicles.Length; j++)
                {
                    dynamic v = new ExpandoObject();
                    v.text = list[i].Vehicles[j].Key;
                    v.tags = list[i].Vehicles[j].Value;
                    vehicles.Add(v);
                }
                struc.nodes = vehicles;
                jsonObject.Add(struc);
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject));
        }

        /// <summary>
        /// 根据条件获取当前用户能查看的所有车辆，参数的两个条件由 AND 组合
        /// </summary>
        /// <param name="strucName">使用单位</param>
        /// <param name="plateNumOrTerminalCode">车牌号或终端号</param>
        private StrucVehiclesListModel[] InternalGetVehiclesListByCurrentUserID(string strucName, string plateNumOrTerminalCode)
        {
            var map = new Dictionary<string, List<KeyValuePair<string, string>>>();
            List<VehiclesTerminalModel> list;
            if (base.VehicleViewMode)
            {
                list = TerminalSettingsBLL.GetDefaultVehiclesList(base.CurrentStrucID, strucName, plateNumOrTerminalCode);
            }
            else
            {
                list = TerminalSettingsBLL.GetVehiclesList(base.CurrentUserID, strucName, plateNumOrTerminalCode);
            }

            if (list != null && list.Count != 0)
            {
                foreach (var item in list)
                {
                    var vehicle = new KeyValuePair<string, string>(item.PlateNum, item.TerminalCode);
                    string StrucName = item.StrucName;
                    List<KeyValuePair<string, string>> vehiclesList;
                    if (!map.ContainsKey(StrucName))
                    {
                        vehiclesList = new List<KeyValuePair<string, string>>();
                        map.Add(StrucName, vehiclesList);
                    }
                    else
                    {
                        vehiclesList = map[StrucName];
                    }
                    vehiclesList.Add(vehicle);
                }
            }
            var ret = new StrucVehiclesListModel[map.Values.Count];
            var Keys = map.Keys.ToArray();
            for (int i = 0; i != Keys.Length; i++)
            {
                ret[i] = new StrucVehiclesListModel();
                ret[i].CompanyName = Keys[i];
                ret[i].Vehicles = map[Keys[i]].ToArray();
            }
            return ret;
        }

        #endregion

        #region 获取WEB客户端IP地址
        /// <summary>
        /// 获取WEB客户端IP地址
        /// </summary>
        [AsiatekSubordinateFunction("Index", "Home", "TerminalSetting")]
        private string GetRemoteAddress()
        {
            string userIP = "未获取用户IP";
            var Context = this.HttpContext;
            try
            {
                if (Context == null || Context.Request == null || Context.Request.ServerVariables == null)
                {
                    return "";
                }

                string CustomerIP = "";

                //CDN加速后取到的IP
                CustomerIP = Context.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                CustomerIP = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }

                if (Context.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                    if (CustomerIP == null)
                    {
                        CustomerIP = Context.Request.ServerVariables["REMOTE_ADDR"];
                    }
                }
                else
                {
                    CustomerIP = Context.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.Compare(CustomerIP, "unknown", true) == 0 || String.IsNullOrEmpty(CustomerIP))
                {
                    return Context.Request.UserHostAddress;
                }
                return CustomerIP;
            }
            catch { }

            return userIP;
        }
        #endregion

        #region 初始化页面
        private ActionResult InitPage(string subView, bool treeViewShowCheckBox = true)
        {
            // 设置PublicPage页面链接的视图页面地址
            ViewBag.SubView = subView;
            // 设置treeview是否显示复选框
            ViewBag.TreeViewShowCheckBox = treeViewShowCheckBox;
            // 这里返回PublicPage 视图 是因为PublicPage 有Html.RenderPartial 重新链接到ViewBag.SubView设置的页面
            // 这样做是为了复用PublicPage页面
            return PartialView("PublicPage");
        }
        #endregion

        #region 获取WCF地址
        private List<TerminalWCfModel> GetWcfAddress(List<VehiclesModel> listVehicles)
        {
            List<string> listTerminal = new List<string>();
            foreach (var item in listVehicles)
            {
                listTerminal.Add(item.TerminalCode);
            }
            // 根据终端号获取终端所属服务器的wcf链接地址
            var listWCF = TerminalSettingsBLL.GetTerminalOfWCFAddress(listTerminal);
            return listWCF;
        }
        #endregion

        #region 错误提示封装
        private string ErrorMesage(string plateNum, string resultCode)
        {
            string errorMsg = string.Format("<div style='font-weight:bolder;'>{0}：{1}{2}{3}：{4}{5}</div>",
                         UIText.TerminalSetting_PlateNumber, plateNum, "<br/>", PromptInformation.FailReason, resultCode, "<hr/>");
            return errorMsg;
        }
        #endregion

        #endregion

        #region 实体类反射
        private string GetProperties<T>(T t)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            string setInfo = string.Empty;

            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return null;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    if (value != null)
                    {
                        ret.Add(name, value.ToString());
                    }

                }
            }

            if (ret.Count > 0)
            {
                foreach (var item in ret)
                {
                    setInfo += item.Key + ":" + item.Value + ",";
                }
            }
            return setInfo.TrimEnd(',');
        }


        #endregion
    }
}

using Asiatek.BLL.MSSQL;
using Asiatek.Model.TerminalSetting;
using Asiatek.TMS.TerminalOperation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asiatek.TMS.Areas.TerminalSetting.Controllers
{
    partial class TerminalSetupController
    {
        /// <summary>
        /// GET: /TerminalSetting/TerminalSetup/TextMessages/
        /// 下发文本消息页面
        /// </summary>
        public ActionResult TextMessages()
        {
            ViewBag.SubView = "TextMessages";
            ViewBag.SubViewData = null;
            ViewBag.TreeViewShowCheckBox = true;
            return PartialView("Main");
        }

        /// <summary>
        /// POST: /TerminalSetting/TerminalSetup/SendTextMessage/
        /// 将前台输入的消息下发给车机
        /// </summary>
        /// <param name="Vehicles">PlateNum#TerminalCode数组</param>
        /// <param name="Settings">文本消息</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendTextMessage(
            [ModelBinder(typeof(BasicArrayBinder<string>))] string[] Vehicles,
            [ModelBinder(typeof(ObjectBinder<TextMessageModel>))] TextMessageModel Settings)
        {
            int userid = GetUserSession().UserId;

            var client = new TerminalOperationClient();
            var objResult = new List<dynamic>();// 记录每次下发设置的返回结果
            foreach (var v in Vehicles)
            {
                dynamic result = new ExpandoObject();

                // Vehicles当中的记录为 PlateNum#TerminalCode
                string[] PlateNumAndTerminalCode = v.Split('#');
                string PlateNum = PlateNumAndTerminalCode[0];
                string TerminalCode = PlateNumAndTerminalCode[1];

                var response = client.SendTextInfo(TerminalCode, new TextInformationData()
                {
                    Content = Settings.MessageText,
                    TextInformationFlag = (TextInformationFlag)Settings.Flags
                });

                // 记录日志
                TerminalSettingsBLL.InsertTerminalOperationsLog(
                    userid, TerminalSettingType.TextMessage, PlateNum,
                    TerminalCode, response.State, JsonConvert.SerializeObject(Settings),
                    null, GetRemoteAddress()
                );

                //保存下发结果以便后面转换为json返回给页面显示
                result.state = response.State;
                result.message = response.Message;
                objResult.Add(result);
            }
            return Content(JsonConvert.SerializeObject(objResult));
        }

    }
}
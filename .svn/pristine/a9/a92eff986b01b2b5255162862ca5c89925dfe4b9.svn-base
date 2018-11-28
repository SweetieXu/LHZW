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
using System.Text.RegularExpressions;

namespace Asiatek.BLL.MSSQL
{
    public class SimCodeBLL
    {
        #region   查询
        public static AsiatekPagedList<SimCodeListModels> GetPagedSimCode(SimCodeSearchModels model, int searchPage, int pageSize)
        {
            string joinStr = string.Empty;
            joinStr = @"LEFT JOIN Structures T ON S.OwnerStrucID=T.ID
LEFT JOIN Structures T1 ON S.UseStrucID=T1.ID
LEFT JOIN ServiceProvider P ON S.ServiceProviderID=P.ID
LEFT JOIN CommunicationType C ON S.CommMode=C.ID";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","SimCodeList S"),
                new SqlParameter("@joinStr",joinStr),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","S.ID"),
                new SqlParameter("@showColumns",@"S.ID ,
        SimCode,
        CommMode,
        PurchaseDate,
        OpeningDate,
        ExpiryDate,
        S.Remark,
        S.Status,P.Name AS ServiceProvider,C.Name AS CName,T.StrucName AS OwnerStrucName,T1.StrucName AS UseStrucName"),
            };

            string conditionStr = " S.Status<>9 ";//不查询删除
            if (!string.IsNullOrWhiteSpace(model.SimCode))
            {
                conditionStr += " AND S.SimCode LIKE '%" + model.SimCode + "%'";
            }
            if (model.CommMode != -1)
            {
                conditionStr += " AND S.CommMode = " + model.CommMode + "";
            }
            if (model.OwnerStrucID != -1)
            {
                conditionStr += " AND S.OwnerStrucID =" + model.OwnerStrucID + "";
            }
            if (model.UseStrucID != -1)
            {
                conditionStr += " AND S.UseStrucID =" + model.UseStrucID + "";
            }
            if (model.ServiceProviderID != -1)
            {
                conditionStr += " AND S.ServiceProviderID =" + model.ServiceProviderID + "";
            }

            if (!string.IsNullOrWhiteSpace(conditionStr))
            {
                paras.Add(new SqlParameter("@conditionStr", conditionStr));
            }

            paras.Add(new SqlParameter()
            {
                ParameterName = "@totalItemCount",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Int
            });
            paras.Add(new SqlParameter()
            {
                ParameterName = "@newCurrentPage",
                Direction = ParameterDirection.Output,
                SqlDbType = SqlDbType.Int
            });
            List<SimCodeListModels> list = ConvertToList<SimCodeListModels>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region   新增

        /// <summary>
        /// 检查SIM卡号是否重复
        /// </summary>
        public static bool CheckSIMCodeExists(string SIMCode)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SIMCode",SqlDbType.VarChar,13),
            };
            paras[0].Value = SIMCode.Trim();


            string sql = "SELECT COUNT(0) FROM dbo.SimCodeList WHERE SimCode=@SIMCode";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        public static OperationResult AddSimCode(AddSimCodeModels model,int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
                new SqlParameter("@SimCode",SqlDbType.NVarChar,13),
                new SqlParameter("@CommMode",SqlDbType.TinyInt),
                new SqlParameter("@OwnerStrucID",SqlDbType.Int),
                new SqlParameter("@UseStrucID",SqlDbType.Int),
                new SqlParameter("@ServiceProviderID",SqlDbType.Int),
                new SqlParameter("@PurchaseDate",SqlDbType.NVarChar,50),
                new SqlParameter("@OpeningDate",SqlDbType.NVarChar,50),
                new SqlParameter("@ExpiryDate",SqlDbType.NVarChar,50),
                new SqlParameter("@Remark",SqlDbType.NVarChar,50),
                new SqlParameter("@CreateUserID",SqlDbType.Int),
            };

            paras[0].Value = model.ID;
            paras[1].Value = model.SimCode.Trim();
            paras[2].Value = model.CommMode;
            paras[3].Value = model.OwnerStrucID;
            paras[4].Value = model.UseStrucID;
            paras[5].Value = model.ServiceProviderID;
            paras[6].Value = model.PurchaseDate;
            paras[7].Value = model.OpeningDate;
            paras[8].Value = model.ExpiryDate;

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[9].Value = DBNull.Value;
            }
            else
            {
                paras[9].Value = model.Remark;
            }
            paras[10].Value = CreateUserID;
            #region  SQL
            string sql = @"INSERT INTO dbo.SimCodeList(SimCode,CommMode,OwnerStrucID,UseStrucID,ServiceProviderID,PurchaseDate,
                                   OpeningDate,ExpiryDate,Remark,Status,CreateUserID) VALUES (@SimCode,@CommMode,@OwnerStrucID,
                                   @UseStrucID,@ServiceProviderID,@PurchaseDate,@OpeningDate,@ExpiryDate,@Remark,'0',@CreateUserID)";
            #endregion

            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region   编辑
        /// <summary>
        /// 检查编辑时SIM卡号是否重复
        /// </summary>
        public static bool CheckEditSIMCodeExists(string SIMCode, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SIMCode",SqlDbType.NVarChar,200),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = SIMCode.Trim();
            paras[1].Value = id;


            string sql = "SELECT COUNT(0) FROM dbo.SimCodeList WHERE SimCode=@SIMCode AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        public static SelectResult<EditSimCodeModels> GetSimCodeID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@id",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"  SELECT a.[ID]
                                      ,a.[SimCode]
                                      ,a.[CommMode]
                                      ,OStr.StrucName AS OwnerStrucName
                                      ,a.[OwnerStrucID]
                                      ,UStr.StrucName AS UseStrucName
                                      ,a.[UseStrucID]
                                      ,a.[ServiceProviderID]
                                      ,a.[PurchaseDate]
                                      ,a.[OpeningDate]
                                      ,a.[ExpiryDate]
                                      ,a.[Remark]
                                      ,a.[Status] FROM dbo.SimCodeList AS a LEFT JOIN Structures AS OStr ON a.OwnerStrucID = OStr.ID
      LEFT JOIN Structures AS UStr ON a.UseStrucID = UStr.ID WHERE a.ID=@id";
            List<EditSimCodeModels> list = ConvertToList<EditSimCodeModels>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditSimCodeModels data = null;
            string msg = string.Empty;

            if (list == null)
            {
                msg = PromptInformation.DBError;
            }
            else if (list.Count == 0)
            {
                msg = PromptInformation.NotExists;
            }
            else
            {
                data = list[0];
            }
            return new SelectResult<EditSimCodeModels>()
            {
                DataResult = data,
                Message = msg
            };
        }


        public static OperationResult EditSimCode(EditSimCodeModels model,int EditUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.BigInt),
                new SqlParameter("@SimCode",SqlDbType.NVarChar,13),
                new SqlParameter("@CommMode",SqlDbType.TinyInt),
                new SqlParameter("@OwnerStrucID",SqlDbType.Int),
                new SqlParameter("@UseStrucID",SqlDbType.Int),
                new SqlParameter("@ServiceProviderID",SqlDbType.Int),
                new SqlParameter("@PurchaseDate",SqlDbType.NVarChar,50),
                new SqlParameter("@OpeningDate",SqlDbType.NVarChar,50),
                new SqlParameter("@ExpiryDate",SqlDbType.NVarChar,50),
                new SqlParameter("@Remark",SqlDbType.NVarChar,50),
                new SqlParameter("@EditUserID",SqlDbType.Int),
                new SqlParameter("@EditTime",SqlDbType.DateTime)
            };

            paras[0].Value = model.ID;
            paras[1].Value = model.SimCode.Trim();
            paras[2].Value = model.CommMode;
            paras[3].Value = model.OwnerStrucID;
            paras[4].Value = model.UseStrucID;
            paras[5].Value = model.ServiceProviderID;
            paras[6].Value = model.PurchaseDate;
            paras[7].Value = model.OpeningDate;
            paras[8].Value = model.ExpiryDate;

            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[9].Value = DBNull.Value;
            }
            else
            {
                paras[9].Value = model.Remark;
            }
            paras[10].Value = EditUserID;
            paras[11].Value = DateTime.Now;
            #region  SQL
            string sql;

            sql = @"UPDATE  dbo.SimCodeList
SET     SimCode=@SimCode,CommMode = @CommMode,OwnerStrucID=@OwnerStrucID,
        UseStrucID=@UseStrucID,ServiceProviderID = @ServiceProviderID,PurchaseDate = @PurchaseDate,
        OpeningDate = @OpeningDate,ExpiryDate = @ExpiryDate,Remark=@Remark,EditUserID=@EditUserID,EditTime=@EditTime
WHERE   ID = @ID";
            #endregion


            int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
            string msg = string.Empty;
            switch (result)
            {
                case 1:
                    msg = PromptInformation.OperationSuccess;
                    break;
                case 0:
                    msg = PromptInformation.NotExists;
                    break;
                case -1:
                    msg = PromptInformation.DBError;
                    break;
            }
            return new OperationResult()
            {
                Success = result > 0,
                Message = msg
            };
        }
        #endregion

        #region   删除
        /// <summary>
        /// 物理删除
        /// </summary>
        public static OperationResult DeleteSimCode(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {  
                // 已经被使用的sim不能被删除
                sqls[i] = @"DELETE FROM dbo.SimCodeList WHERE ID=@ID AND Status  <> 1";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i];
                paras[i][0] = temp;
            }
            bool result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion

        #region   通讯方式
        public static List<CommunicationTypeDDLModel> GetCommMode()
        {
            string sql = "SELECT ID,Name FROM dbo.CommunicationType where Status<>9 order by ID asc";
            return ConvertToList<CommunicationTypeDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion

        #region   服务商
        public static List<ServiceProviderDDLModel> GetServiceProvider()
        {
            string sql = "SELECT ID,Name FROM dbo.ServiceProvider where Status<>9 ORDER BY ID";
            return ConvertToList<ServiceProviderDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion

        #region 获取所有未使用未删除的SIM卡列表
        /// <summary>
        /// 获取单位下拉列表信息
        /// 包含单位ID、单位名称
        /// </summary>
        public static List<SimCodeDDLModel> GetNotUserdSimCodeList(string simCode, int? simCodeId)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SimCode",SqlDbType.VarChar)
            };
            paras[0].Value = "%" + simCode + "%";
            string sql = "SELECT  [ID],[SimCode] FROM  [dbo].[SimCodeList] WHERE [Status]<> 9 AND [Status] <> 1 AND SimCode LIKE @SimCode ";
            if (simCodeId.HasValue && simCodeId.Value > 0)
            {
                sql += " UNION SELECT  [ID],[SimCode] FROM  [dbo].[SimCodeList] WHERE ID = " + simCodeId;
            }

            return ConvertToList<SimCodeDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion

        #region 导入
        public static OperationResult ImportSimCodes(string excelFilePath, string sheetName, int createUserID)
        {
            bool success = false;
            string message = string.Empty;
            string brStr = "<br/>";
            var datas = ExcelHelper.ExcelToDataTable(excelFilePath, sheetName);
            if (datas == null)
            {
                message = PromptInformation.AccessExcelFailed;
            }
            else if (datas.Rows.Count == 0)
            {
                message = PromptInformation.NoSIMs;
            }
            else
            {
                //存储Sim卡号，用于判断DataTable中本身是否存在重复的Sim卡号
                List<string> SimCodes = new List<string>();

                //存储待新增的Sim卡号
                Dictionary<string, AddSimCodeModels> addDic = new Dictionary<string, AddSimCodeModels>();
                int i = 0;
                for (i = 0; i < datas.Rows.Count; i++)
                {
                    int rowNum = i + 2;
                    string rowMessage = string.Format(PromptInformation.RowIndex, rowNum);
                    AddSimCodeModels addModel = new AddSimCodeModels();
                    List<string> errorMessage = new List<string>();
                    var currentRow = datas.Rows[i];
                    if (CheckImportSimInfo(currentRow, errorMessage,SimCodes, addModel))
                    {
                        addDic.Add(rowMessage, addModel);
                    }
                    else//只要有一条失败，马上返回提示，不继续检查
                    {
                        message = rowMessage + brStr;
                        errorMessage.ForEach(msg =>
                        {
                            message += string.Format("{0}{1}", msg, brStr);
                        });
                        break;
                    }
                }

                if (i == datas.Rows.Count)//全部都检查无误才对数据进行新增
                {
                    int failedCount = 0;
                    addDic.ToList().ForEach(kv =>
                    {
                        if (!AddSimCode(kv.Value, createUserID).Success)
                        {
                            message += string.Format("{0}{1}", kv.Key, brStr);
                            failedCount++;
                        }
                    });
                    success = failedCount == 0;
                }
            }
            return new OperationResult()
            {
                Success = success,
                Message = success ? PromptInformation.ImportSuccess : PromptInformation.ImportFailed + brStr + message
            };
        }

        /// <summary>
        /// 检查导入的sim资料合法性
        /// </summary>
        private static bool CheckImportSimInfo(DataRow currentRow, List<string> errorMessages, List<string> SimCodes, AddSimCodeModels model)
        {
            string fieldName = string.Empty;

            #region 获取当前行数据
            string SimCodeStr = currentRow[0].ToString().Trim();//                    Sim卡号                           
            string CommModeStr = currentRow[1].ToString().Trim();//                 通信方式                     
            string OwnerStrucNameStr = currentRow[2].ToString().Trim();//                  归属单位                      
            string UseStrucNameStr = currentRow[3].ToString().Trim();//               使用单位
            string ServiceProviderStr = currentRow[4].ToString().Trim();//              服务商
            string PurchaseDateStr = currentRow[5].ToString().Trim();//               购买日期
            string OpeningDateStr = currentRow[6].ToString().Trim();//                 开通日期
            string ExpiryDateStr = currentRow[7].ToString().Trim();//               过期日期
            string RemarkStr = currentRow[8].ToString().Trim();//              备注
            #endregion

            #region 检查Sim卡号
            fieldName = DisplayText.SIMCode;
            Regex reg = new Regex(@"^([0-9]{11}|[0-9]{13})$");
           
            if (string.IsNullOrWhiteSpace(SimCodeStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查Sim卡格式
            else if ( !reg.IsMatch(SimCodeStr))
            {
                errorMessages.Add(DataAnnotations.SIMCodeError);
            }
            //检查Sim卡在excel中是否重复
            else if (SimCodes.Exists(sc => sc == SimCodeStr))
            {
                errorMessages.Add(PromptInformation.SameSIMInExcel + "：" + SimCodeStr);
            }
            //检查Sim卡在数据库中是否重复
            else if (CheckSIMCodeExists(SimCodeStr))
            {
                errorMessages.Add(string.Format(DataAnnotations.FieldExists, fieldName));
            }
            else
            {
                SimCodes.Add(SimCodeStr);
            }
            #endregion

            #region 通信方式
            fieldName = DisplayText.CommMode;
            int CommModeValue = -1;
            if (string.IsNullOrWhiteSpace(CommModeStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查通信方式合法性
            else if (!CommunicationTypeBLL.TryGetCodeByName(CommModeStr, out CommModeValue))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, CommModeStr, PromptInformation.NotExists));
            }
            #endregion


            #region 归属单位
            fieldName = DisplayText.OwnerStrucCode;
            int OwnerStrucID = -1;
            if (string.IsNullOrWhiteSpace(OwnerStrucNameStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查单位名是否存在，并且获取真正的单位ID
            else if (!StructureBLL.TryGetStructureIDByName(OwnerStrucNameStr, out OwnerStrucID))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, OwnerStrucNameStr, PromptInformation.NotExists));
            }
            #endregion

            #region 使用单位
            fieldName = DisplayText.UseStrucCode;
            int UseStrucID = -1;
            if (string.IsNullOrWhiteSpace(UseStrucNameStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查单位名是否存在，并且获取真正的单位ID
            else if (!StructureBLL.TryGetStructureIDByName(UseStrucNameStr, out UseStrucID))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, UseStrucNameStr, PromptInformation.NotExists));
            }
            #endregion

            #region 服务商
            fieldName = DisplayText.ServiceProvider;
            int ServiceProviderValue = -1;
            if (string.IsNullOrWhiteSpace(ServiceProviderStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            //检查服务商合法性
            else if (!ServiceProviderTypeBLL.TryGetCodeByName(ServiceProviderStr, out ServiceProviderValue))
            {
                errorMessages.Add(string.Format("{0}：{1}{2}", fieldName, ServiceProviderStr, PromptInformation.NotExists));
            }
            #endregion

            #region 购买日期
            fieldName = DisplayText.PurchaseDate;
            DateTime PurchaseDateValue = DateTime.Today;
            bool PurchaseDateIsVaild=true;
            if (string.IsNullOrWhiteSpace(PurchaseDateStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
                PurchaseDateIsVaild=false;
            }
            else if (!DateTime.TryParse(PurchaseDateStr, out PurchaseDateValue))
            {
                errorMessages.Add(fieldName + " " + DataAnnotations.DateError);
                PurchaseDateIsVaild=false;
            }
            #endregion

            #region 开通日期
            fieldName = DisplayText.OpeningDate;
            DateTime OpeningDateValue = DateTime.Today;
            bool OpeningDateIsVaild=true;
            if (string.IsNullOrWhiteSpace(OpeningDateStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
                OpeningDateIsVaild=false;
            }
            else if (!DateTime.TryParse(OpeningDateStr, out OpeningDateValue))
            {
                errorMessages.Add(fieldName + " " + DataAnnotations.DateError);
                OpeningDateIsVaild=false;
            }
            else if (PurchaseDateIsVaild  &&
                OpeningDateValue.Subtract(PurchaseDateValue).TotalMilliseconds < 0 )
            {
                errorMessages.Add(fieldName + DataAnnotations.NotLessThan + DisplayText.PurchaseDate);
            }
            #endregion

            #region 过期日期
            fieldName = DisplayText.ExpiryDate;
            DateTime ExpiryDateValue = DateTime.Today;
            if (string.IsNullOrWhiteSpace(ExpiryDateStr))
            {
                errorMessages.Add(PromptInformation.MissingField + fieldName);
            }
            else if (!DateTime.TryParse(ExpiryDateStr, out ExpiryDateValue))
            {
                errorMessages.Add(fieldName + " " + DataAnnotations.DateError);
            }
            else if (PurchaseDateIsVaild && OpeningDateIsVaild &&
                ExpiryDateValue.Subtract(PurchaseDateValue).TotalMilliseconds <0 ||
                ExpiryDateValue.Subtract(OpeningDateValue).TotalMilliseconds < 0)
            {
                errorMessages.Add(fieldName + DataAnnotations.NotLessThan + DisplayText.PurchaseDate + DataAnnotations.Or + DisplayText.OpeningDate);
            }
            #endregion

            model.SimCode = SimCodeStr;
            model.CommMode = CommModeValue;
            model.OwnerStrucID = OwnerStrucID;
            model.UseStrucID = UseStrucID;
            model.ServiceProviderID = ServiceProviderValue;
            model.PurchaseDate = PurchaseDateValue.ToString("yyyy-MM-dd");
            model.OpeningDate = OpeningDateValue.ToString("yyyy-MM-dd");
            model.ExpiryDate = ExpiryDateValue.ToString("yyyy-MM-dd");
            model.Remark = RemarkStr;
            model.Status = "0";

            return errorMessages.Count == 0;
        }
        #endregion

        #region 其他
        /// <summary>
        /// 根据sim卡号获取终端simID
        /// </summary>
        public static bool TryGetIDByCode(string Code, out int ID)
        {
            ID = -1;
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SimCode",SqlDbType.NVarChar,13),
            };
            paras[0].Value = Code;
            string sql = "SELECT ID FROM SimCodeList WHERE SimCode=@SimCode";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)
            {
                return false;
            }
            ID = Convert.ToInt32(result);
            return true;
        }
        #endregion
    }
}


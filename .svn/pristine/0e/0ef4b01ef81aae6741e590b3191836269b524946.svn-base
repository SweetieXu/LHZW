using Asiatek.AjaxPager;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Asiatek.BLL.MSSQL
{
    public class EmployeeInfoBLL
    {
        #region 查询
        /// <summary>
        /// 获取员工信息分页数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="searchPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static AsiatekPagedList<EmployeeInfoListModel> GetPagedEmployeeInfos(EmployeeInfoSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","EmployeeInfo E"),
                new SqlParameter("@joinStr",@" LEFT JOIN (SELECT * FROM dbo.DriveType WHERE Status=0) DT 
    ON E.DriveTypeID=DT.DriveTypeID  
    LEFT JOIN (SELECT * FROM dbo.DriveLicenseState WHERE Status=0) DLS 
    ON E.DriveLicenseStateID=DLS.DriveLicenseStateID "),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","E.ID DESC"),
                new SqlParameter("@showColumns",@"E.ID,
                E.EmployeeID,
                E.EmployeeName,
                E.EmployeeGender,
                E.CertificateCode,
                E.ContactPhone,
                E.EmergePhone,
                E.IsDriver,
                E.IsCarrier,
                DT.DriveTypeID,
                DT.DriveTypeName,
                DLS.DriveLicenseStateID,
                DLS.DriveLicenseStateName "),
            };

            #region 筛选条件
            string conditionStr = "E.Status=0";
            if (!string.IsNullOrWhiteSpace(model.EmployeeID))
            {
                conditionStr += " AND E.EmployeeID LIKE '%" + model.EmployeeID + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.EmployeeName))
            {
                conditionStr += " AND E.EmployeeName LIKE '%" + model.EmployeeName + "%'";
            }

            if (model.DriveLicenseStateID != -1)
            {
                conditionStr += " AND DLS.DriveLicenseStateID =" + model.DriveLicenseStateID + "";
            }
            if (!string.IsNullOrWhiteSpace(conditionStr))
            {
                paras.Add(new SqlParameter("@conditionStr", conditionStr));
            }
            #endregion

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
            List<EmployeeInfoListModel> list = ConvertToList<EmployeeInfoListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }


        /// <summary>
        /// 获取驾照状态信息
        /// </summary>
        /// <returns></returns>
        public static List<DriveLicenseStateSelectModel> GetDriveLicenseStates()
        {
            string sql = "SELECT * FROM dbo.DriveLicenseState WHERE Status=0";
            return ConvertToList<DriveLicenseStateSelectModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion

        #region 查询 新版
        /// <summary>
        /// 获取员工信息分页数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="searchPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static AsiatekPagedList<EmployeeInfoPageModel> GetPagedEmployeeInfo(EmployeeInfoFindModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","EmployeeInfo AS E"),
                  new SqlParameter("@joinStr",@" LEFT JOIN  Structures AS S ON E.StrucID = S.ID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","E.ID DESC"),
                new SqlParameter("@showColumns",@"E.ID,E.EmployeeName,E.EmployeeGender,E.IsDriver,E.IsCarrier,
                                                                                 E.CertificateCode,E.ContactPhone,E.EmergePhone,S.StrucName"),
            };

            #region 筛选条件
            string conditionStr = "E.Status=0";
            if (!string.IsNullOrWhiteSpace(model.EmployeeName))
            {
                conditionStr += " AND E.EmployeeName LIKE '%" + model.EmployeeName + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.CertificateCode))
            {
                conditionStr += " AND E.CertificateCode LIKE '%" + model.CertificateCode + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.ContactPhone))
            {
                conditionStr += " AND E.ContactPhone LIKE '%" + model.ContactPhone + "%'";
            }
            if (model.IsCarriers != -1)
            {
                conditionStr += " AND E.IsCarrier LIKE '%" + model.IsCarriers + "%'";
            }
            if (model.IsDrivers != -1)
            {
                conditionStr += " AND E.IsDriver LIKE '%" + model.IsDrivers + "%'";
            }
            if (model.SearchStrucID.HasValue)
            {
                conditionStr += " AND E.StrucID LIKE '%" + model.SearchStrucID + "%'";
            }

            paras.Add(new SqlParameter("@conditionStr", conditionStr));

            #endregion

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
            List<EmployeeInfoPageModel> list = ConvertToList<EmployeeInfoPageModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        #endregion

        #region 查询 新版 2017-10-23 根据用户所属单位与名称模糊匹配用户所属单位以及子单位信息
        /// <summary>
        /// 获取员工信息分页数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="searchPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static AsiatekPagedList<EmployeeInfoPageModel> GetPagedEmployeeInfo_New(EmployeeInfoFindModel model, int searchPage, int pageSize, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","EmployeeInfo AS E"),
                new SqlParameter("@joinStr",@" INNER JOIN  Func_GetStrucAndSubStrucByUserAffiliatedStrucID("+strucID+") AS S ON E.StrucID = S.ID"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","E.ID DESC"),
                new SqlParameter("@showColumns",@"E.ID,E.EmployeeName,E.EmployeeGender,E.IsDriver,E.IsCarrier,
                                                                                 E.CertificateCode,E.ContactPhone,E.EmergePhone,S.StrucName"),
            };

            #region 筛选条件
            string conditionStr = "E.Status=0";
            if (!string.IsNullOrWhiteSpace(model.EmployeeName))
            {
                conditionStr += " AND E.EmployeeName LIKE '%" + model.EmployeeName + "%'";
            }

            if (!string.IsNullOrWhiteSpace(model.CertificateCode))
            {
                conditionStr += " AND E.CertificateCode LIKE '%" + model.CertificateCode + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.ContactPhone))
            {
                conditionStr += " AND E.ContactPhone LIKE '%" + model.ContactPhone + "%'";
            }
            if (model.IsCarriers != -1)
            {
                conditionStr += " AND E.IsCarrier LIKE '%" + model.IsCarriers + "%'";
            }
            if (model.IsDrivers != -1)
            {
                conditionStr += " AND E.IsDriver LIKE '%" + model.IsDrivers + "%'";
            }
            if (model.SearchStrucID.HasValue)
            {
                conditionStr += " AND E.StrucID = " + model.SearchStrucID + "";
            }

            paras.Add(new SqlParameter("@conditionStr", conditionStr));

            #endregion

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
            List<EmployeeInfoPageModel> list = ConvertToList<EmployeeInfoPageModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        #endregion

        #region 新增
        public static OperationResult AddEmployeeInfo(EmployeeInfoAddModel model, int currentUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeID",SqlDbType.NVarChar,20),
                new SqlParameter("@EmployeeName",SqlDbType.NVarChar,50),
                new SqlParameter("@EmployeeGender",SqlDbType.Bit),
                new SqlParameter("@CertificateTypeID",SqlDbType.Int),
                new SqlParameter("@CertificateCode",SqlDbType.NVarChar,20),

                new SqlParameter("@BirthDate",SqlDbType.Date),
                new SqlParameter("@CertificateOffice",SqlDbType.NVarChar,50),
                new SqlParameter("@ValidStartTime",SqlDbType.Date),
                new SqlParameter("@ValidEndTime",SqlDbType.Date),
                new SqlParameter("@ContactPhone",SqlDbType.NVarChar,20),

                new SqlParameter("@ContactAddress",SqlDbType.NVarChar,200),
                new SqlParameter("@IsDriver",SqlDbType.Bit),
                new SqlParameter("@DriveCode",SqlDbType.NVarChar,20),
                new SqlParameter("@DriveTypeID",SqlDbType.NVarChar,20),
                new SqlParameter("@IsCarrier",SqlDbType.Bit),

                new SqlParameter("@EmergePhone",SqlDbType.NVarChar,20),
                new SqlParameter("@DriveLicenseStateID",SqlDbType.Int), 
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@Status",SqlDbType.Int),
                new SqlParameter("@CreateUser",SqlDbType.Int),

                new SqlParameter("@CreateTime",SqlDbType.DateTime),
                new SqlParameter("@UpdateUser",SqlDbType.Int),
                new SqlParameter("@UpdateTime",SqlDbType.DateTime),
                new SqlParameter("@DriveCodeValidTime",SqlDbType.Date),
                new SqlParameter("@CarrierCode",SqlDbType.NVarChar,20),

                new SqlParameter("@StrucID",SqlDbType.Int),
            };

            paras[0].Value = model.EmployeeID;
            paras[1].Value = model.EmployeeName.Trim();
            paras[2].Value = model.EmployeeGender;
            paras[3].Value = model.CertificateTypeID;
            paras[4].Value = model.CertificateCode;

            if (string.IsNullOrEmpty(model.BirthDate))
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.BirthDate;
            }
            if (string.IsNullOrEmpty(model.CertificateOffice))
            {
                paras[6].Value = DBNull.Value;
            }
            else
            {
                paras[6].Value = model.CertificateOffice.Trim();
            }
            if (string.IsNullOrEmpty(model.ValidStartTime))
            {
                paras[7].Value = DBNull.Value;
            }
            else
            {
                paras[7].Value = model.ValidStartTime;
            }
            if (string.IsNullOrEmpty(model.ValidEndTime))
            {
                paras[8].Value = DBNull.Value;
            }
            else
            {
                paras[8].Value = model.ValidEndTime;
            }
            paras[9].Value = model.ContactPhone.Trim();

            paras[10].Value = model.ContactAddress.Trim();
            paras[11].Value = model.IsDriver;
            if (string.IsNullOrEmpty(model.DriveCode))
            {
                paras[12].Value = DBNull.Value;
            }
            else
            {
                paras[12].Value = model.DriveCode.Trim();
            }
            paras[13].Value = model.DriveTypeID;
            paras[14].Value = model.IsCarrier;

            if (string.IsNullOrEmpty(model.EmergePhone))
            {
                paras[15].Value = DBNull.Value;
            }
            else
            {
                paras[15].Value = model.EmergePhone.Trim();
            }
            paras[16].Value = model.DriveLicenseStateID;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[17].Value = DBNull.Value;
            }
            else
            {
                paras[17].Value = model.Remark;
            }
            paras[18].Value = 0;
            paras[19].Value = currentUserID;

            paras[20].Value = DateTime.Now;
            paras[21].Value = DBNull.Value;
            paras[22].Value = DBNull.Value;
            if (string.IsNullOrEmpty(model.DriveCodeValidTime))
            {
                paras[23].Value = DBNull.Value;
            }
            else
            {
                paras[23].Value = model.DriveCodeValidTime;
            }
            if (string.IsNullOrEmpty(model.CarrierCode))
            {
                paras[24].Value = DBNull.Value;
            }
            else
            {
                paras[24].Value = model.CarrierCode.Trim();
            }

            paras[25].Value = model.StrucID;


            #region  SQL
            string sql = @"INSERT INTO dbo.EmployeeInfo
        ( EmployeeID ,
          EmployeeName ,
          EmployeeGender ,
          BirthDate ,
          ContactPhone ,
          ContactAddress ,
          CertificateTypeID ,
          CertificateCode ,
          CertificateOffice ,
          ValidStartTime ,
          ValidEndTime ,
          IsDriver ,
          DriveCode ,
          DriveTypeID ,
          DriveLicenseStateID ,
          IsCarrier ,
          EmergePhone ,
          Remark ,
          Status ,
          CreateUser ,
          CreateTime ,
          UpdateUser ,
          UpdateTime ,
          DriveCodeValidTime ,
          CarrierCode ,
          StrucID
        )
VALUES  ( @EmployeeID , -- EmployeeID - nvarchar(20)
          @EmployeeName , -- EmployeeName - nvarchar(50)
          @EmployeeGender , -- EmployeeGender - bit
          @BirthDate , -- BirthDate - date
          @ContactPhone , -- ContactPhone - nvarchar(20)
          @ContactAddress , -- ContactAddress - nvarchar(200)
          @CertificateTypeID , -- CertificateTypeID - int
          @CertificateCode , -- CertificateCode - nvarchar(20)
          @CertificateOffice, -- CertificateOffice - nvarchar(50)
          @ValidStartTime , -- ValidStartTime - date
          @ValidEndTime , -- ValidEndTime - date
          @IsDriver , -- IsDriver - bit
          @DriveCode , -- DriveCode - nvarchar(20)
          @DriveTypeID , -- DriveTypeID - int
          @DriveLicenseStateID , -- DriveLicenseStateID - int
          @IsCarrier , -- IsCarrier - bit
          @EmergePhone , -- EmergePhone - nvarchar(20)
          @Remark , -- Remark - nvarchar(500)
          @Status , -- Status - int
          @CreateUser , -- CreateUser - int
          @CreateTime , -- CreateTime - datetime
          @UpdateUser ,  -- UpdateUser - int
          @UpdateTime ,  -- UpdateTime - datetime
          @DriveCodeValidTime,  
          @CarrierCode ,  
          @StrucID
        )";
            #endregion


            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        /// <summary>
        /// 获取证件类型信息
        /// </summary>
        /// <returns></returns>
        public static List<CertificateTypeSelectModel> GetCertificateTypes()
        {
            string sql = "SELECT * FROM dbo.CertificateType WHERE Status=0";
            return ConvertToList<CertificateTypeSelectModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        /// <summary>
        /// 获取准驾车型信息
        /// </summary>
        /// <returns></returns>
        public static List<DriveTypeSelectModel> GetDriveTypes()
        {
            string sql = "SELECT * FROM dbo.DriveType WHERE Status=0";
            return ConvertToList<DriveTypeSelectModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }

        /// <summary>
        /// 检查工号是否重复
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static bool CheckAddEmployeeIDExists(string employeeID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@employeeID",SqlDbType.NVarChar,20),
            };
            paras[0].Value = employeeID;


            string sql = "SELECT COUNT(0) FROM dbo.EmployeeInfo WHERE EmployeeID=@employeeID AND Status=0";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 证件号是否重复
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static bool CheckAddIDCardIsExists(string idCard)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@CertificateCode",SqlDbType.NVarChar,20),
            };
            paras[0].Value = idCard;


            string sql = "SELECT COUNT(0) FROM dbo.EmployeeInfo WHERE CertificateCode=@CertificateCode AND Status=0";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        #endregion


        #region 修改

        public static SelectResult<EmployeeInfoEditModel> GetEmployeeInfoByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.NVarChar,20),
            };
            paras[0].Value = id;
            string sql = @"SELECT EmployeeID,EmployeeName
 ,EmployeeGender
      ,CAST(BirthDate AS CHAR(10)) AS BirthDate
      ,ContactPhone
      ,ContactAddress
      ,CertificateTypeID
      ,CertificateCode
      ,CertificateOffice
      ,CAST(ValidStartTime AS CHAR(10)) AS ValidStartTime
      ,CAST(ValidEndTime AS CHAR(10)) AS ValidEndTime
      ,IsDriver
      ,DriveCode
      ,DriveTypeID
      ,DriveLicenseStateID
      ,IsCarrier
      ,EmergePhone
      ,CarrierCode
      ,StrucID
      ,Remark
      ,CAST(DriveCodeValidTime AS CHAR(10)) AS DriveCodeValidTime
  FROM dbo.EmployeeInfo
  WHERE ID=@ID";
            List<EmployeeInfoEditModel> list = ConvertToList<EmployeeInfoEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EmployeeInfoEditModel data = null;
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
            return new SelectResult<EmployeeInfoEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditEmployeeInfo(EmployeeInfoEditModel model, int currentUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeName",SqlDbType.NVarChar,50),
                new SqlParameter("@EmployeeGender",SqlDbType.Bit),
                new SqlParameter("@CertificateTypeID",SqlDbType.Int),
                new SqlParameter("@CertificateCode",SqlDbType.NVarChar,20),
                new SqlParameter("@BirthDate",SqlDbType.Date),

                new SqlParameter("@CertificateOffice",SqlDbType.NVarChar,50),
                new SqlParameter("@ValidStartTime",SqlDbType.Date),
                new SqlParameter("@ValidEndTime",SqlDbType.Date),
                new SqlParameter("@ContactPhone",SqlDbType.NVarChar,20),
                new SqlParameter("@ContactAddress",SqlDbType.NVarChar,200),

                new SqlParameter("@IsDriver",SqlDbType.Bit),
                new SqlParameter("@DriveCode",SqlDbType.NVarChar,20),
                new SqlParameter("@DriveTypeID",SqlDbType.NVarChar,20),
                new SqlParameter("@IsCarrier",SqlDbType.Bit),
                new SqlParameter("@EmergePhone",SqlDbType.NVarChar,20),

                new SqlParameter("@DriveLicenseStateID",SqlDbType.Int), 
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@DriveCodeValidTime",SqlDbType.Date),
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@UpdateUser",SqlDbType.Int),

                new SqlParameter("@CarrierCode",SqlDbType.NVarChar,20),
                new SqlParameter("@StrucID",SqlDbType.Int),
            };

            paras[0].Value = model.EmployeeName.Trim();
            paras[1].Value = model.EmployeeGender;
            paras[2].Value = model.CertificateTypeID;
            paras[3].Value = model.CertificateCode.Trim();
            if (string.IsNullOrEmpty(model.BirthDate))
            {
                paras[4].Value = DBNull.Value;
            }
            else
            {
                paras[4].Value = model.BirthDate;
            }

            if (string.IsNullOrEmpty(model.CertificateOffice))
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.CertificateOffice.Trim();
            }
            if (string.IsNullOrEmpty(model.ValidStartTime))
            {
                paras[6].Value = DBNull.Value;
            }
            else
            {
                paras[6].Value = model.ValidStartTime;
            }
            if (string.IsNullOrEmpty(model.ValidEndTime))
            {
                paras[7].Value = DBNull.Value;
            }
            else
            {
                paras[7].Value = model.ValidEndTime;
            }
            paras[8].Value = model.ContactPhone.Trim();
            paras[9].Value = model.ContactAddress.Trim();

            paras[10].Value = model.IsDriver;
            if (string.IsNullOrEmpty(model.DriveCode))
            {
                paras[11].Value = DBNull.Value;
            }
            else
            {
                paras[11].Value = model.DriveCode.Trim();
            }
            paras[12].Value = model.DriveTypeID;
            paras[13].Value = model.IsCarrier;
            if (string.IsNullOrEmpty(model.EmergePhone))
            {
                paras[14].Value = DBNull.Value;
            }
            else
            {
                paras[14].Value = model.EmergePhone.Trim();
            }

            paras[15].Value = model.DriveLicenseStateID;
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[16].Value = DBNull.Value;
            }
            else
            {
                paras[16].Value = model.Remark;
            }
            if (string.IsNullOrWhiteSpace(model.DriveCodeValidTime))
            {
                paras[17].Value = DBNull.Value;
            }
            else
            {
                paras[17].Value = model.DriveCodeValidTime;
            }
            paras[18].Value = model.ID;
            paras[19].Value = currentUserID;

            if (string.IsNullOrWhiteSpace(model.CarrierCode))
            {
                paras[20].Value = DBNull.Value;
            }
            else
            {
                paras[20].Value = model.CarrierCode.Trim();
            }
            paras[21].Value = model.StrucID;

            #region  SQL
            string sql = @"UPDATE dbo.EmployeeInfo
      SET EmployeeName = @EmployeeName ,
          EmployeeGender = @EmployeeGender ,
          BirthDate = @BirthDate ,
          ContactPhone = @ContactPhone ,
          ContactAddress = @ContactAddress ,
          CertificateTypeID = @CertificateTypeID ,
          CertificateCode = @CertificateCode ,
          CertificateOffice = @CertificateOffice ,
          ValidStartTime = @ValidStartTime ,
          ValidEndTime = @ValidEndTime ,
          IsDriver = @IsDriver ,
          DriveCode = @DriveCode ,
          DriveTypeID = @DriveTypeID ,
          DriveLicenseStateID = @DriveLicenseStateID ,
          IsCarrier = @IsCarrier ,
          EmergePhone = @EmergePhone ,
          Remark = @Remark ,
          UpdateUser = @UpdateUser ,
          UpdateTime = GetDate() ,
          DriveCodeValidTime = @DriveCodeValidTime  ,
          CarrierCode = @CarrierCode ,
          StrucID = @StrucID 
        WHERE  ID = @ID ";
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

        public static bool CheckEditIDCardIsExists(string idCard, string employeeID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@CertificateCode",SqlDbType.NVarChar,20),
                new SqlParameter("@EmployeeID",SqlDbType.NVarChar,20),
            };
            paras[0].Value = idCard;
            paras[1].Value = employeeID;


            string sql = "SELECT COUNT(0) FROM dbo.EmployeeInfo WHERE CertificateCode=@CertificateCode AND Status=0 AND EmployeeID != @EmployeeID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion


        #region 删除
        /// <summary>
        /// 根据ID删除员工信息，非物理删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult DeleteEmployeeInfo(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.EmployeeInfo SET Status=9 WHERE ID=@ID";
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

        #region 删除
        /// <summary>
        /// 根据ID删除员工信息，物理删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult DeleteEmployeeInfoPhysical(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.EmployeeInfo WHERE ID=@ID";
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


        #region 员工信息新增修改新版本

        #region 新增
        public static OperationResult AddEmployeeInfoNew(EditEmployeeInfoModel model, int currentUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeName",SqlDbType.NVarChar,50),
                new SqlParameter("@EmployeeGender",SqlDbType.Bit),
                new SqlParameter("@CertificateTypeID",SqlDbType.Int),
                new SqlParameter("@CertificateCode",SqlDbType.NVarChar,20),
                new SqlParameter("@ContactPhone",SqlDbType.NVarChar,20),
                new SqlParameter("@ContactAddress",SqlDbType.NVarChar,200),
                new SqlParameter("@IsDriver",SqlDbType.Bit),
                new SqlParameter("@DriveCode",SqlDbType.NVarChar,20),
                new SqlParameter("@IsCarrier",SqlDbType.Bit),
                new SqlParameter("@EmergePhone",SqlDbType.NVarChar,20),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@Status",SqlDbType.Int),
                new SqlParameter("@CreateUser",SqlDbType.Int),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
                new SqlParameter("@DriveCodeValidTime",SqlDbType.Date),
                new SqlParameter("@CarrieCodeValidTime",SqlDbType.Date),
                new SqlParameter("@CarrierCode",SqlDbType.NVarChar,20),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@IsOwners",SqlDbType.Bit),
            };

            paras[0].Value = model.EmployeeName.Trim();
            paras[1].Value = model.EmployeeGender;
            paras[2].Value = model.CertificateTypeID;
            if (string.IsNullOrWhiteSpace(model.CertificateCode))
            {
                paras[3].Value = DBNull.Value;
            }
            else
            {
                paras[3].Value = model.CertificateCode.Trim();
            }
            paras[4].Value = model.ContactPhone;
            if (string.IsNullOrWhiteSpace(model.ContactAddress))
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.ContactAddress.Trim();
            }
            paras[6].Value = model.IsDriver;
            if (string.IsNullOrEmpty(model.DriveCode))
            {
                paras[7].Value = DBNull.Value;
            }
            else
            {
                paras[7].Value = model.DriveCode.Trim();
            }
            paras[8].Value = model.IsCarrier;

            if (string.IsNullOrEmpty(model.EmergePhone))
            {
                paras[9].Value = DBNull.Value;
            }
            else
            {
                paras[9].Value = model.EmergePhone.Trim();
            }
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[10].Value = DBNull.Value;
            }
            else
            {
                paras[10].Value = model.Remark;
            }
            paras[11].Value = 0;
            paras[12].Value = currentUserID;
            paras[13].Value = DateTime.Now;
            if (string.IsNullOrEmpty(model.DriveCodeValidTime))
            {
                paras[14].Value = DBNull.Value;
            }
            else
            {
                paras[14].Value = model.DriveCodeValidTime;
            }
            if (string.IsNullOrEmpty(model.CarrieCodeValidTime))
            {
                paras[15].Value = DBNull.Value;
            }
            else
            {
                paras[15].Value = model.CarrieCodeValidTime;
            }
            if (string.IsNullOrEmpty(model.CarrierCode))
            {
                paras[16].Value = DBNull.Value;
            }
            else
            {
                paras[16].Value = model.CarrierCode.Trim();
            }
            paras[17].Value = model.StrucID;
            paras[18].Value = model.IsOwners;

            #region  SQL
            string sql = @"INSERT INTO dbo.EmployeeInfo
        (EmployeeName ,EmployeeGender , ContactPhone ,ContactAddress ,CertificateTypeID ,CertificateCode ,
          IsDriver ,DriveCode ,IsCarrier ,EmergePhone ,Remark ,Status ,CreateUser ,CreateTime ,
          DriveCodeValidTime ,CarrieCodeValidTime,CarrierCode ,StrucID,IsOwners
        )
VALUES  (
          @EmployeeName , -- EmployeeName - nvarchar(50)
          @EmployeeGender , -- EmployeeGender - bit
          @ContactPhone , -- ContactPhone - nvarchar(20)
          @ContactAddress , -- ContactAddress - nvarchar(200)
          @CertificateTypeID , -- CertificateTypeID - int
          @CertificateCode , -- CertificateCode - nvarchar(20)
          @IsDriver , -- IsDriver - bit
          @DriveCode , -- DriveCode - nvarchar(20)
          @IsCarrier , -- IsCarrier - bit
          @EmergePhone , -- EmergePhone - nvarchar(20)
          @Remark , -- Remark - nvarchar(500)
          @Status , -- Status - int
          @CreateUser , -- CreateUser - int
          @CreateTime , -- CreateTime - datetime
          @DriveCodeValidTime,  
          @CarrieCodeValidTime,
          @CarrierCode ,  
          @StrucID,
          @IsOwners
        )";
            #endregion

            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion

        #region 修改
        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SelectResult<EditEmployeeInfoModel> GetNewEmployeeInfoByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT a.ID,EmployeeName,EmployeeGender,ContactPhone,ContactAddress ,CertificateTypeID,CertificateCode
                                  ,IsDriver,DriveCode,IsCarrier ,EmergePhone,CarrierCode ,StrucID ,b.StrucName ,a.Remark,
                                 CAST(DriveCodeValidTime AS CHAR(10)) AS DriveCodeValidTime,CAST(CarrieCodeValidTime AS CHAR(10)) AS
CarrieCodeValidTime,a.IsOwners FROM dbo.EmployeeInfo 
                             AS a LEFT JOIN dbo.Structures AS b ON a.StrucID = b.ID WHERE a.ID=@ID";
            List<EditEmployeeInfoModel> list = ConvertToList<EditEmployeeInfoModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditEmployeeInfoModel data = null;
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
            return new SelectResult<EditEmployeeInfoModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        /// <summary>
        /// 员工信息修改
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        public static OperationResult EditEmployeeInfoNew(EditEmployeeInfoModel model, int currentUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeName",SqlDbType.NVarChar,50),
                new SqlParameter("@EmployeeGender",SqlDbType.Bit),
                new SqlParameter("@CertificateTypeID",SqlDbType.Int),
                new SqlParameter("@CertificateCode",SqlDbType.NVarChar,20),
                new SqlParameter("@ContactPhone",SqlDbType.NVarChar,20),
                new SqlParameter("@ContactAddress",SqlDbType.NVarChar,200),
                new SqlParameter("@IsDriver",SqlDbType.Bit),
                new SqlParameter("@DriveCode",SqlDbType.NVarChar,20),
                new SqlParameter("@IsCarrier",SqlDbType.Bit),
                new SqlParameter("@EmergePhone",SqlDbType.NVarChar,20),
                new SqlParameter("@Remark",SqlDbType.NVarChar,500),
                new SqlParameter("@DriveCodeValidTime",SqlDbType.Date),
                new SqlParameter("@CarrieCodeValidTime",SqlDbType.Date),
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@UpdateUser",SqlDbType.Int),
                new SqlParameter("@CarrierCode",SqlDbType.NVarChar,20),
                new SqlParameter("@StrucID",SqlDbType.Int),
                new SqlParameter("@IsOwners",SqlDbType.Bit),
            };

            paras[0].Value = model.EmployeeName.Trim();
            paras[1].Value = model.EmployeeGender;
            paras[2].Value = model.CertificateTypeID;
            if (string.IsNullOrEmpty(model.CertificateCode))
            {
                paras[3].Value = DBNull.Value;
            }
            else
            {
                paras[3].Value = model.CertificateCode.Trim();
            }
            paras[4].Value = model.ContactPhone;
            if (string.IsNullOrEmpty(model.ContactAddress))
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.ContactAddress.Trim();
            }
            paras[6].Value = model.IsDriver;
            if (string.IsNullOrEmpty(model.DriveCode))
            {
                paras[7].Value = DBNull.Value;
            }
            else
            {
                paras[7].Value = model.DriveCode.Trim();
            }
            paras[8].Value = model.IsCarrier;
            if (string.IsNullOrEmpty(model.EmergePhone))
            {
                paras[9].Value = DBNull.Value;
            }
            else
            {
                paras[9].Value = model.EmergePhone.Trim();
            }
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[10].Value = DBNull.Value;
            }
            else
            {
                paras[10].Value = model.Remark;
            }
            if (string.IsNullOrWhiteSpace(model.DriveCodeValidTime))
            {
                paras[11].Value = DBNull.Value;
            }
            else
            {
                paras[11].Value = model.DriveCodeValidTime;
            }
            if (string.IsNullOrWhiteSpace(model.CarrieCodeValidTime))
            {
                paras[12].Value = DBNull.Value;
            }
            else
            {
                paras[12].Value = model.CarrieCodeValidTime;
            }
            paras[13].Value = model.ID;
            paras[14].Value = currentUserID;

            if (string.IsNullOrWhiteSpace(model.CarrierCode))
            {
                paras[15].Value = DBNull.Value;
            }
            else
            {
                paras[15].Value = model.CarrierCode.Trim();
            }
            paras[16].Value = model.StrucID;
            paras[17].Value = model.IsOwners;

            #region  SQL

            // 更新员工信息的时候需要注意 如果打开编辑页面的时候该员工还未被车辆使用，此时界面上是可以修改隶属单位的，但是当点击保存的时候
            // 用户在这之前刚好把这个员工关联了车辆  那么此时隶属单位就不应该被修改 为了防止类似事情发生，故where条件中在做限制
            // 上述的限制仅仅是在更换了隶属单位的时候才需要添加 如果隶属单位未做修改 无需添加此限制
            string sql = @"UPDATE dbo.EmployeeInfo SET EmployeeName = @EmployeeName ,EmployeeGender = @EmployeeGender ,
                                      ContactPhone = @ContactPhone ,ContactAddress = @ContactAddress ,CertificateTypeID = @CertificateTypeID ,
                                      CertificateCode = @CertificateCode ,IsDriver = @IsDriver ,DriveCode = @DriveCode ,IsCarrier = @IsCarrier ,
                                      EmergePhone = @EmergePhone ,Remark = @Remark ,UpdateUser = @UpdateUser ,UpdateTime = GetDate() ,
                                      DriveCodeValidTime = @DriveCodeValidTime  ,CarrieCodeValidTime = @CarrieCodeValidTime,
                                     CarrierCode = @CarrierCode ,StrucID = @StrucID,IsOwners = @IsOwners WHERE  ID = @ID ";

            if (model.OldStrucID != model.StrucID)
            {
                sql += "AND NOT EXISTS(SELECT EmployeeInfoID FROM VehicleEmployeeInfo WHERE EmployeeInfoID=@ID)";
            }
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

        #region 获取当前员是否已经被车辆使用
        /// <summary>
        /// 获取当前员是否已经被车辆使用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<VehicleEmployeeInfoDDLModel> GetEmployeeInfoUsedToVehicle(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT DISTINCT [Type] FROM VehicleEmployeeInfo WHERE EmployeeInfoID=@ID";
            return ConvertToList<VehicleEmployeeInfoDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion

        #region 检查重复性
        public static bool CheckEditCertificateCodeExists(string idCard, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@CertificateCode",SqlDbType.NVarChar,20),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = idCard.Trim();
            paras[1].Value = id;
            string sql = string.Empty;
            if (id > 0)
            {
                sql = "SELECT COUNT(0) FROM dbo.EmployeeInfo WHERE CertificateCode=@CertificateCode AND Status=0 AND ID <> @ID";
            }
            else
            {
                sql = "SELECT COUNT(0) FROM dbo.EmployeeInfo WHERE CertificateCode=@CertificateCode AND Status=0";
            }
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 检查工号是否重复
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public static bool CheckEditEmployeeIDExists(string employeeID, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@EmployeeID",SqlDbType.NVarChar,20),
                 new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = employeeID.Trim();
            paras[1].Value = id;

            string sql = string.Empty;
            if (id > 0)
            {
                sql = "SELECT COUNT(0) FROM dbo.EmployeeInfo WHERE EmployeeID=@EmployeeID AND ID <> @ID";
            }
            else
            {
                sql = "SELECT COUNT(0) FROM dbo.EmployeeInfo WHERE EmployeeID=@employeeID";
            }
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        #endregion

        #endregion

        #region 根据使用单位和驾驶员或者押运员名称模糊查询员工信息
        /// <summary>
        /// 根据使用单位和驾驶员或者押运员名称模糊查询员工信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="strucID"></param>
        /// <param name="type">类型 1 驾驶员 2押运员 3车主</param>
        /// <returns></returns>
        public static List<NewEmployeeInfoDDLModel> GetEmployeeInfoByStrucIDAndName(string name, int strucID, int type)
        {
            string sql = string.Empty;
            if (type == 1)
            {
                sql = @"SELECT [ID],EmployeeName,[IsDriver] FROM [EmployeeInfo] 
                                    WHERE Status = 0 AND IsDriver = 1 AND StrucID = @StrucID AND EmployeeName  LIKE @name";
            }
            else if (type == 2)
            {
                sql = @"SELECT [ID],EmployeeName,[IsDriver] FROM [EmployeeInfo] 
                                    WHERE Status = 0 AND IsCarrier = 1 AND StrucID = @StrucID AND EmployeeName  LIKE @name";
            }
            else if (type == 3)
            {
                sql = @"SELECT [ID],EmployeeName,[IsOwners] FROM [EmployeeInfo]   
   WHERE Status = 0 AND IsOwners = 1  AND StrucID = @StrucID AND EmployeeName  LIKE @name";
            }
           
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NVarChar),
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            paras[0].Value = "%" + name + "%";
            paras[1].Value = strucID;
            return ConvertToList<NewEmployeeInfoDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

//        /// <summary>
//        /// 根据车主名称模糊查询员工信息
//        /// </summary>
//        /// <param name="name"></param>
//        /// <param name="type">3</param>
//        /// <returns></returns>
//        public static List<NewEmployeeInfoDDLModel> GetEmployeeInfoByOwnersName(string name, int type, int currentStrucID)
//        {
//            string sql = string.Format(@"SELECT e.[ID],e.EmployeeName,e.[IsOwners] FROM [EmployeeInfo] e  
// INNER JOIN Func_GetStrucAndSubStrucByUserAffiliatedStrucID({0}) s ON e.StrucID = s.ID 
//   WHERE e.Status = 0 AND e.IsOwners = 1  AND EmployeeName  LIKE @name", currentStrucID);

//            List<SqlParameter> paras = new List<SqlParameter>()
//            {
//                new SqlParameter("@name",SqlDbType.NVarChar),
//            };
//            paras[0].Value = "%" + name + "%";
//            return ConvertToList<NewEmployeeInfoDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
//        }
        #endregion
    }
}

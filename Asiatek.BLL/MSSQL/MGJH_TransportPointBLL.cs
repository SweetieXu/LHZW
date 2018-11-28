using Asiatek.AjaxPager;
using Asiatek.Components.GIS;
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
    public class MGJH_TransportPointBLL
    {
        #region 提货点
        #region 查询
        public static AsiatekPagedList<MGJH_PickUpTransportPointListModel> GetPagedPickUpTransportPoints(MGJH_PickUpTransportPointSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.MGJH_TransportPointSetting tp "),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","tp.CreateTime DESC"),
                new SqlParameter("@showColumns",@"tp.ID
          ,tp.SettingType
          ,tp.AddressName
          ,tp.AddressCode
          ,tp.EFType
          ,tp.EFInfo "), 
            };

            #region 筛选条件
            string conditionStr = " tp.SettingType=1"; //查询提货点
            if (!string.IsNullOrWhiteSpace(model.AddressName))
            {
                conditionStr += " AND tp.AddressName LIKE '%" + model.AddressName + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.AddressCode))
            {
                conditionStr += " AND tp.AddressCode LIKE '%" + model.AddressCode + "%'";
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
            var rs = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray());
            List<MGJH_PickUpTransportPointListModel> list = ConvertToList<MGJH_PickUpTransportPointListModel>.Convert(rs);
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region 新增
        public static OperationResult AddPickUpTransportPoint(AddPickUpTransportPointModel model, int currentUserID)
        {
            string _EFInfo = ChangeCoordinateSystem(model.EFType, model.EFInfo, 1); //地图坐标转车机坐标,存入数据库
            #region 参数
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SettingType",SqlDbType.TinyInt),
                new SqlParameter("@AddressName",SqlDbType.NVarChar),
                new SqlParameter("@AddressCode",SqlDbType.NVarChar),
                new SqlParameter("@EFType",SqlDbType.TinyInt),

                new SqlParameter("@EFInfo",SqlDbType.NVarChar),
                new SqlParameter("@CreateUser",SqlDbType.Int),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
            };

            paras[0].Value = 1;  //1-提货点
            paras[1].Value = model.AddressName.Trim();
            paras[2].Value = model.AddressCode.Trim();
            paras[3].Value = model.EFType;

            paras[4].Value = _EFInfo;
            paras[5].Value = currentUserID;
            paras[6].Value = DateTime.Now;
            #endregion

            #region
            string sql = @"INSERT  INTO dbo.MGJH_TransportPointSetting
                ( [SettingType]
      ,[AddressName]
      ,[AddressCode]
      ,[EFType]
      ,[EFInfo]
      ,[CreateUser]
      ,[CreateTime]
                )
        VALUES  ( @SettingType , 
                    @AddressName, 
                    @AddressCode,
                    @EFType , 
                    @EFInfo, 
                    @CreateUser,
                    @CreateTime
                )";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;

            #endregion

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        #region 验证
        /// <summary>
        /// 提货点名称唯一性验证--添加
        /// </summary>
        /// <param name="addressName"></param>
        /// <returns></returns>
        public static bool CheckAddPickUpAddressNameExists(string addressName)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.MGJH_TransportPointSetting WHERE SettingType=1 AND AddressName=@AddressName";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressName",SqlDbType.NVarChar),
            };
            paras[0].Value = addressName.Trim();

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 提货点编码唯一性验证--添加
        /// </summary>
        /// <param name="addressCode"></param>
        /// <returns></returns>
        public static bool CheckAddPickUpAddressCodeExists(string addressCode)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.MGJH_TransportPointSetting WHERE SettingType=1 AND AddressCode=@AddressCode";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressCode",SqlDbType.NVarChar),
            };
            paras[0].Value = addressCode.Trim();

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion
        #endregion

        #region 物理删除
        public static OperationResult DeletePickUpTransportPoint(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.MGJH_TransportPointSetting WHERE ID=@ID";
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

        #region 修改
        public static SelectResult<EditPickUpTransportPointModel> GetPickUpTransportPointByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@ID",
                    SqlDbType=SqlDbType.Int,
                },
            };
            paras[0].Value = id;
            string sql = @"SELECT ID
      ,SettingType
      ,AddressName
      ,AddressCode
      ,EFType
      ,EFInfo
  FROM dbo.MGJH_TransportPointSetting
  WHERE ID=@ID";

            List<EditPickUpTransportPointModel> list = ConvertToList<EditPickUpTransportPointModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditPickUpTransportPointModel data = null;
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
                data.EFInfo = ChangeCoordinateSystem(data.EFType, data.EFInfo, 2); //将取出的车机坐标转成地图坐标，显示
            }
            return new SelectResult<EditPickUpTransportPointModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditPickUpTransportPoint(EditPickUpTransportPointModel model, int currentUserID)
        {
            string _EFInfo = ChangeCoordinateSystem(model.EFType, model.EFInfo, 1); //地图坐标转车机坐标,存入数据库
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressName",SqlDbType.NVarChar),
                new SqlParameter("@AddressCode",SqlDbType.NVarChar),
                new SqlParameter("@EFType",SqlDbType.TinyInt),
                new SqlParameter("@EFInfo",SqlDbType.NVarChar),
                new SqlParameter("@UpdateUser",SqlDbType.Int),
                new SqlParameter("@UpdateTime",SqlDbType.DateTime),
                new SqlParameter("@ID",SqlDbType.Int),
            };

            paras[0].Value = model.AddressName.Trim();
            paras[1].Value = model.AddressCode.Trim();
            paras[2].Value = model.EFType;
            paras[3].Value = _EFInfo;
            paras[4].Value = currentUserID;
            paras[5].Value = DateTime.Now;
            paras[6].Value = model.ID;

            #region  SQL
            string sql = @"UPDATE   dbo.MGJH_TransportPointSetting
       SET      AddressName = @AddressName ,
                AddressCode = @AddressCode ,
                EFType = @EFType ,
                EFInfo = @EFInfo ,
                UpdateUser = @UpdateUser,
                UpdateTime = @UpdateTime
       WHERE    ID = @ID";
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

        #region 验证
        /// <summary>
        /// 提货点名称唯一性验证--修改
        /// </summary>
        /// <param name="addressName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckEditPickUpAddressNameExists(string addressName, int id)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.MGJH_TransportPointSetting WHERE SettingType=1 AND AddressName=@AddressName AND ID !=@ID ";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressName",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = addressName.Trim();
            paras[1].Value = id;

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 提货点编码唯一性验证--修改
        /// </summary>
        /// <param name="addressCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckEditPickUpAddressCodeExists(string addressCode, int id)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.MGJH_TransportPointSetting WHERE SettingType=1 AND AddressCode=@AddressCode AND ID !=@ID ";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressCode",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = addressCode.Trim();
            paras[1].Value = id;

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion
        #endregion
        #endregion


        #region 收货点
        #region 查询
        public static AsiatekPagedList<MGJH_ReceiveTransportPointListModel> GetPagedReceiveTransportPoints(MGJH_ReceiveTransportPointSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.MGJH_TransportPointSetting tp "),
                new SqlParameter("@joinStr",@" LEFT JOIN dbo.MGJH_TransportPointSetting tp2 ON tp2.ID=tp.SuperiorAddressID "),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","tp.CreateTime DESC"),
                new SqlParameter("@showColumns",@"tp.ID
      ,tp.SettingType
      ,tp.CustomerName
      ,tp.AddressName
      ,tp.AddressCode
      ,tp.AddressArea
      ,tp.SuperiorAddressID
      ,tp.IsUnloadPoint
      ,tp.EFType
      ,tp.EFInfo
      ,tp.UnloadTime/60 AS UnloadTime
      ,tp.UnloadTimeError/60 AS UnloadTimeError
      ,tp2.AddressName AS SuperiorAddressName"), 
            };

            #region 筛选条件
            string conditionStr = " tp.SettingType=2";
            if (!string.IsNullOrWhiteSpace(model.AddressName))
            {
                conditionStr += " AND tp.AddressName LIKE '%" + model.AddressName + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.AddressCode))
            {
                conditionStr += " AND tp.AddressCode LIKE '%" + model.AddressCode + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.CustomerName))
            {
                conditionStr += " AND tp.CustomerName LIKE '%" + model.CustomerName + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.AddressArea))
            {
                conditionStr += " AND tp.AddressArea LIKE '%" + model.AddressArea + "%'";
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
            var rs = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray());
            List<MGJH_ReceiveTransportPointListModel> list = ConvertToList<MGJH_ReceiveTransportPointListModel>.Convert(rs);
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region 新增
        public static OperationResult AddReceiveTransportPoint(AddReceiveTransportPointModel model, int currentUserID)
        {
            #region 参数
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SettingType",SqlDbType.TinyInt),
                new SqlParameter("@AddressName",SqlDbType.NVarChar),
                new SqlParameter("@AddressCode",SqlDbType.NVarChar),
                new SqlParameter("@CustomerName",SqlDbType.NVarChar),
                new SqlParameter("@AddressArea",SqlDbType.NVarChar),

                new SqlParameter("@SuperiorAddressID",SqlDbType.Int),
                new SqlParameter("@IsUnloadPoint",SqlDbType.Bit),
                new SqlParameter("@EFType",SqlDbType.TinyInt),
                new SqlParameter("@EFInfo",SqlDbType.NVarChar),

                new SqlParameter("@UnloadTime",SqlDbType.Int),
                new SqlParameter("@UnloadTimeError",SqlDbType.Int),
                new SqlParameter("@CreateUser",SqlDbType.Int),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
                new SqlParameter("@SourceID",SqlDbType.Int),
            };

            paras[0].Value = 2;  //2-收货点
            paras[1].Value = model.AddressName.Trim();
            paras[2].Value = model.AddressCode.Trim();
            paras[3].Value = model.CustomerName.Trim();
            paras[4].Value = model.AddressArea.Trim();

            //无上级收货地址时，SuperiorAddressID赋空值
            if (model.SuperiorAddressID == -1)
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.SuperiorAddressID;
            }
            paras[6].Value = model.IsUnloadPoint;
            //不是卸货点，紧紧是收货点时，以下信息为空
            if (model.IsUnloadPoint == false)
            {
                paras[7].Value = DBNull.Value;
                paras[8].Value = DBNull.Value;
                paras[9].Value = DBNull.Value;
                paras[10].Value = DBNull.Value;
            }
            else  //是卸货点时
            {
                paras[7].Value = model.EFType;
                paras[8].Value = ChangeCoordinateSystem(model.EFType, model.EFInfo, 1); //地图坐标转车机坐标,存入数据库
                //暂时去掉预计卸货时长和误差 操作人：戴天辰
                paras[9].Value = DBNull.Value;
                paras[10].Value = DBNull.Value;
                //paras[9].Value = model.UnloadTime * 60; //数据库存的UnloadTime以秒为单位
                //if (model.UnloadTimeError == null) //误差前台没有做验证，这里做下处理
                //{
                //    paras[10].Value = DBNull.Value;
                //}
                //else
                //{
                //    paras[10].Value = model.UnloadTimeError * 60; //数据库存的UnloadTimeError以秒为单位
                //}
            }

            paras[11].Value = currentUserID;
            paras[12].Value = DateTime.Now;
            paras[13].Value = model.SourceID ?? (object)DBNull.Value;
            #endregion

            #region  SQL
            string sql = @"INSERT  INTO dbo.MGJH_TransportPointSetting
                ( [SettingType]
      ,[AddressName]
      ,[AddressCode]
      ,[CustomerName]
      ,[AddressArea]
      ,[SuperiorAddressID]
      ,[IsUnloadPoint]
      ,[EFType]
      ,[EFInfo]
      ,[UnloadTime]
      ,[UnloadTimeError]
      ,[CreateUser]
      ,[CreateTime] )
        VALUES  ( @SettingType , 
                    @AddressName, 
                    @AddressCode,
                    @CustomerName , 
                    @AddressArea, 
                    @SuperiorAddressID,
                    @IsUnloadPoint , 
                    @EFType, 
                    @EFInfo,
                    @UnloadTime , 
                    @UnloadTimeError, 
                    @CreateUser,
                    @CreateTime  )";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;

            #endregion

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        #region 上级收货点--查询所有收货地址
        public static List<SuperiorAddressModel> GetAddSuperiorAddress()
        {
            string sql = "SELECT ID,AddressName FROM dbo.MGJH_TransportPointSetting WHERE SettingType=2 ";
            return ConvertToList<SuperiorAddressModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion

        #region 验证
        /// <summary>
        /// 收货点名称唯一性验证--添加
        /// </summary>
        /// <param name="addressName"></param>
        /// <returns></returns>
        public static bool CheckAddReceiveAddressNameExists(string addressName)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.MGJH_TransportPointSetting WHERE SettingType=2 AND AddressName=@AddressName";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressName",SqlDbType.NVarChar),
            };
            paras[0].Value = addressName.Trim();

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 收货点编码唯一性验证--添加
        /// </summary>
        /// <param name="addressCode"></param>
        /// <returns></returns>
        public static bool CheckAddReceiveAddressCodeExists(string addressCode)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.MGJH_TransportPointSetting WHERE SettingType=2 AND AddressCode=@AddressCode";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressCode",SqlDbType.NVarChar),
            };
            paras[0].Value = addressCode.Trim();

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion
        #endregion

        #region 物理删除
        //public static OperationResult DeleteReceiveTransportPoint(string[] ids)
        //{
        //    //需判断是否有下级数据：有--删除失败并提示，无--直接删除
        //    var IsRelateData = false;
        //    for (int i = 0; i < ids.Length; i++)
        //    {
        //        string IsSqls = @"SELECT COUNT(0) FROM dbo.MGJH_TransportPointSetting WHERE SuperiorAddressID=@ID ";
        //        List<SqlParameter> IsParas = new List<SqlParameter>()
        //        {
        //            new SqlParameter("@ID",SqlDbType.Int),
        //        };
        //        IsParas[0].Value = ids[i];
        //        var count = MSSQLHelper.ExecuteScalar(CommandType.Text, IsSqls, IsParas.ToArray());
        //        if (count != null && (int)count > 0)
        //        {
        //            IsRelateData = true;
        //        }
        //    }
        //    bool result;
        //    if (IsRelateData == true) //有下级数据
        //    {
        //        result = false;
        //        return new OperationResult()
        //        {
        //            Success = result,
        //            Message = result ? PromptInformation.DeleteSuccess : PromptInformation.HaveRelateReceiveTransportPoint
        //        };
        //    }
        //    else //无下级数据，删除数据
        //    {
        //        string[] sqls = new string[ids.Length];
        //        SqlParameter[][] paras = new SqlParameter[ids.Length][];
        //        for (int i = 0; i < ids.Length; i++)
        //        {
        //            sqls[i] = @"DELETE FROM dbo.MGJH_TransportPointSetting WHERE ID=@ID";
        //            paras[i] = new SqlParameter[1];
        //            SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
        //            temp.Value = ids[i];
        //            paras[i][0] = temp;
        //        }
        //        result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
        //        return new OperationResult()
        //        {
        //            Success = result,
        //            Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
        //        };
        //    }

        //}

        /// <summary>
        /// 物理删除收货地
        /// 删除的收货地可能是有下级的，这里通过单表主外键控制
        /// 修改人：戴天辰
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static OperationResult DeleteReceiveTransportPoint(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"DELETE FROM dbo.MGJH_TransportPointSetting WHERE ID=@ID";
                paras[i] = new SqlParameter[1];
                SqlParameter temp = new SqlParameter("@ID", SqlDbType.Int);
                temp.Value = ids[i];
                paras[i][0] = temp;
            }
            var result = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, paras);
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.DeleteSuccess : PromptInformation.DeleteFailure
            };
        }
        #endregion

        #region 修改
        public static SelectResult<EditReceiveTransportPointModel> GetReceiveTransportPointByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@ID",
                    SqlDbType=SqlDbType.Int,
                },
            };
            paras[0].Value = id;
            string sql = @"SELECT tp.ID
      ,tp.SettingType
      ,tp.CustomerName
      ,tp.AddressName
      ,tp.AddressCode
      ,tp.AddressArea
      ,tp.SuperiorAddressID
      ,tp.IsUnloadPoint
      ,tp.EFType
      ,tp.EFInfo
      ,tp.UnloadTime/60 AS UnloadTime
      ,tp.UnloadTimeError/60 AS UnloadTimeError
      ,tp2.AddressName AS SuperiorAddressName 
      FROM dbo.MGJH_TransportPointSetting tp 
      LEFT JOIN dbo.MGJH_TransportPointSetting tp2 ON tp2.ID=tp.SuperiorAddressID 
  WHERE tp.ID=@ID";

            List<EditReceiveTransportPointModel> list = ConvertToList<EditReceiveTransportPointModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditReceiveTransportPointModel data = null;
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
                data.EFInfo = ChangeCoordinateSystem(data.EFType, data.EFInfo, 2); //将取出的车机坐标转成地图坐标，显示
            }
            return new SelectResult<EditReceiveTransportPointModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditReceiveTransportPoint(EditReceiveTransportPointModel model, int currentUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressName",SqlDbType.NVarChar),
                new SqlParameter("@AddressCode",SqlDbType.NVarChar),
                new SqlParameter("@CustomerName",SqlDbType.NVarChar),
                new SqlParameter("@AddressArea",SqlDbType.NVarChar),

                new SqlParameter("@SuperiorAddressID",SqlDbType.Int),
                new SqlParameter("@IsUnloadPoint",SqlDbType.Bit),
                new SqlParameter("@EFType",SqlDbType.TinyInt),
                new SqlParameter("@EFInfo",SqlDbType.NVarChar),

                new SqlParameter("@UnloadTime",SqlDbType.Int),
                new SqlParameter("@UnloadTimeError",SqlDbType.Int),
                new SqlParameter("@UpdateUser",SqlDbType.Int),
                new SqlParameter("@UpdateTime",SqlDbType.DateTime),
                new SqlParameter("@ID",SqlDbType.Int),
            };

            paras[0].Value = model.AddressName.Trim();
            paras[1].Value = model.AddressCode.Trim();
            paras[2].Value = model.CustomerName.Trim();
            paras[3].Value = model.AddressArea.Trim();

            //无上级收货地址时，SuperiorAddressID赋空值
            if (model.SuperiorAddressID == -1)
            {
                paras[4].Value = DBNull.Value;
            }
            else
            {
                paras[4].Value = model.SuperiorAddressID;
            }
            paras[5].Value = model.IsUnloadPoint;
            //不是卸货点，紧紧是收货点时，以下信息为空
            if (model.IsUnloadPoint == false)
            {
                paras[6].Value = DBNull.Value;
                paras[7].Value = DBNull.Value;
                paras[8].Value = DBNull.Value;
                paras[9].Value = DBNull.Value;
            }
            else  //是卸货点时
            {
                paras[6].Value = model.EFType;
                paras[7].Value = ChangeCoordinateSystem(model.EFType, model.EFInfo, 1); //地图坐标转车机坐标,存入数据库
                //暂时去掉预计卸货时长和误差 操作人：戴天辰
                paras[8].Value = DBNull.Value;
                paras[9].Value = DBNull.Value;
                //paras[8].Value = model.UnloadTime * 60; //数据库存的UnloadTime以秒为单位
                //if (model.UnloadTimeError == null) //误差前台没有做验证，这里做下处理
                //{
                //    paras[9].Value = DBNull.Value;
                //}
                //else
                //{
                //    paras[9].Value = model.UnloadTimeError * 60; //数据库存的UnloadTimeError以秒为单位
                //}
            }

            paras[10].Value = currentUserID;
            paras[11].Value = DateTime.Now;
            paras[12].Value = model.ID;

            #region  SQL
            string sql = @"UPDATE   dbo.MGJH_TransportPointSetting
       SET      AddressName = @AddressName ,
                AddressCode = @AddressCode ,
                CustomerName = @CustomerName ,
                AddressArea = @AddressArea ,
                SuperiorAddressID = @SuperiorAddressID ,
                IsUnloadPoint = @IsUnloadPoint ,
                EFType = @EFType ,
                EFInfo = @EFInfo ,
                UnloadTime = @UnloadTime ,
                UnloadTimeError = @UnloadTimeError ,
                UpdateUser = @UpdateUser,
                UpdateTime = @UpdateTime
       WHERE    ID = @ID";
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

        #region 上级收货点--查询除了自己外的所有收货地址
        public static List<SuperiorAddressModel> GetEditSuperiorAddress(int id)
        {
            string sql = "SELECT ID,AddressName FROM dbo.MGJH_TransportPointSetting WHERE SettingType=2 AND ID != @ID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            return ConvertToList<SuperiorAddressModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        #endregion

        #region 验证
        /// <summary>
        /// 收货点名称唯一性验证--修改
        /// </summary>
        /// <param name="addressName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckEditReceiveAddressNameExists(string addressName, int id)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.MGJH_TransportPointSetting WHERE SettingType=2 AND AddressName=@AddressName AND ID !=@ID ";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressName",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = addressName.Trim();
            paras[1].Value = id;

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 收货点编码唯一性验证--修改
        /// </summary>
        /// <param name="addressCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool CheckEditReceiveAddressCodeExists(string addressCode, int id)
        {
            string sql = @" SELECT COUNT(0) FROM dbo.MGJH_TransportPointSetting WHERE SettingType=2 AND AddressCode=@AddressCode AND ID !=@ID ";

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@AddressCode",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = addressCode.Trim();
            paras[1].Value = id;

            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion
        #endregion
        #endregion


        #region 同步收货点
        public static AsiatekPagedList<MGJH_SynchroReceivePointListModel> GetPagedSynchroReceivePoints(MGJH_SynchroReceivePointSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.MGJH_SynchroReceiveAddress sr  "),
                new SqlParameter("@joinStr",@" LEFT JOIN dbo.MGJH_TransportPointSetting tp ON tp.SourceID=sr.ID "),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","sr.ID DESC"),
                new SqlParameter("@showColumns",@"sr.ID
      ,sr.CustomerName
      ,sr.AddressName
      ,sr.AddressCode
      ,sr.AddressArea,tp.SourceID"), 
            };

            #region 筛选条件
            string conditionStr = " 1=1 ";
            if (!string.IsNullOrWhiteSpace(model.AddressName))
            {
                conditionStr += " AND sr.AddressName LIKE '%" + model.AddressName + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.AddressCode))
            {
                conditionStr += " AND sr.AddressCode LIKE '%" + model.AddressCode + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.CustomerName))
            {
                conditionStr += " AND sr.CustomerName LIKE '%" + model.CustomerName + "%'";
            }
            if (!string.IsNullOrWhiteSpace(model.AddressArea))
            {
                conditionStr += " AND sr.AddressArea LIKE '%" + model.AddressArea + "%'";
            }
            if (model.IsSynchro != -1) //全部
            {
                if (model.IsSynchro == 1) //已同步
                {
                    conditionStr += " AND tp.SourceID IS NOT NULL";
                }
                else if (model.IsSynchro == 2) //未同步
                {
                    conditionStr += " AND tp.SourceID IS NULL";
                }
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
            var rs = MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray());
            List<MGJH_SynchroReceivePointListModel> list = ConvertToList<MGJH_SynchroReceivePointListModel>.Convert(rs);
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        //获取指定需要同步的收货地址信息
        public static SelectResult<EditSynchroReceivePointModel> GetSynchroReceivePointByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@ID",
                    SqlDbType=SqlDbType.Int,
                },
            };
            paras[0].Value = id;
            string sql = @"SELECT sr.ID
      ,sr.CustomerName
      ,sr.AddressName
      ,sr.AddressCode
      ,sr.AddressArea 
      FROM dbo.MGJH_SynchroReceiveAddress sr 
  WHERE sr.ID=@ID";

            List<EditSynchroReceivePointModel> list = ConvertToList<EditSynchroReceivePointModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            EditSynchroReceivePointModel data = null;
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
            return new SelectResult<EditSynchroReceivePointModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        //将要同步的收货地址信息添加到收货点设定表中
        public static OperationResult EditSynchroReceivePoint(EditSynchroReceivePointModel model, int currentUserID)
        {
            #region 参数
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@SettingType",SqlDbType.TinyInt),
                new SqlParameter("@AddressName",SqlDbType.NVarChar),
                new SqlParameter("@AddressCode",SqlDbType.NVarChar),
                new SqlParameter("@CustomerName",SqlDbType.NVarChar),
                new SqlParameter("@AddressArea",SqlDbType.NVarChar),

                new SqlParameter("@SuperiorAddressID",SqlDbType.Int),
                new SqlParameter("@IsUnloadPoint",SqlDbType.Bit),
                new SqlParameter("@EFType",SqlDbType.TinyInt),
                new SqlParameter("@EFInfo",SqlDbType.NVarChar),

                new SqlParameter("@UnloadTime",SqlDbType.Int),
                new SqlParameter("@UnloadTimeError",SqlDbType.Int),
                new SqlParameter("@CreateUser",SqlDbType.Int),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
                new SqlParameter("@SourceID",SqlDbType.Int),
            };

            paras[0].Value = 2;  //2-收货点
            paras[1].Value = model.AddressName.Trim();
            paras[2].Value = model.AddressCode.Trim();
            paras[3].Value = model.CustomerName.Trim();
            paras[4].Value = model.AddressArea.Trim();

            //无上级收货地址时，SuperiorAddressID赋空值
            if (model.SuperiorAddressID == -1)
            {
                paras[5].Value = DBNull.Value;
            }
            else
            {
                paras[5].Value = model.SuperiorAddressID;
            }
            paras[6].Value = model.IsUnloadPoint;
            //不是卸货点，紧紧是收货点时，以下信息为空
            if (model.IsUnloadPoint == false)
            {
                paras[7].Value = DBNull.Value;
                paras[8].Value = DBNull.Value;
                paras[9].Value = DBNull.Value;
                paras[10].Value = DBNull.Value;
            }
            else  //是卸货点时
            {
                paras[7].Value = model.EFType;
                paras[8].Value = ChangeCoordinateSystem(model.EFType, model.EFInfo, 1); //地图坐标转车机坐标,存入数据库
                paras[9].Value = model.UnloadTime * 60; //数据库存的UnloadTime以秒为单位
                if (model.UnloadTimeError == null) //误差前台没有做验证，可以为空，这里做下处理
                {
                    paras[10].Value = DBNull.Value;
                }
                else
                {
                    paras[10].Value = model.UnloadTimeError * 60; //数据库存的UnloadTimeError以秒为单位
                }
            }

            paras[11].Value = currentUserID;
            paras[12].Value = DateTime.Now;
            paras[13].Value = model.ID;  //为同步收货地址表MGJH_SynchroReceiveAddress的ID，关联MGJH_TransportPointSetting表的SourceID字段
            #endregion

            #region  SQL
            string sql = @"INSERT  INTO dbo.MGJH_TransportPointSetting
                ( [SettingType]
      ,[AddressName]
      ,[AddressCode]
      ,[CustomerName]
      ,[AddressArea]
      ,[SuperiorAddressID]
      ,[IsUnloadPoint]
      ,[EFType]
      ,[EFInfo]
      ,[UnloadTime]
      ,[UnloadTimeError]
      ,[CreateUser]
      ,[CreateTime]
,SourceID)
        VALUES  ( @SettingType , 
                    @AddressName, 
                    @AddressCode,
                    @CustomerName , 
                    @AddressArea, 
                    @SuperiorAddressID,
                    @IsUnloadPoint , 
                    @EFType, 
                    @EFInfo,
                    @UnloadTime , 
                    @UnloadTimeError, 
                    @CreateUser,
                    @CreateTime ,
@SourceID )";
            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;

            #endregion

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }
        #endregion


        #region 坐标系转换
        /// <summary>
        /// 高德地图坐标和车机坐标互转
        /// 地图上获取的是地图坐标，存入数据库时需要转换成车机坐标
        /// 数据库查询出来的坐标是车机坐标，地图显示时候需要转换成地图坐标
        /// </summary>
        /// <param name="efType">坐标数据是圆形或者矩形、多边形，圆形和矩形、多边形的存储方式有别，需要分开处理</param>
        /// <param name="efInfo">坐标数据</param>
        /// <param name="changeType">1--地图坐标转车机坐标，2--车机坐标转地图坐标</param>
        /// <returns></returns>
        public static string ChangeCoordinateSystem(int? efType, string efInfo, int changeType)
        {
            string[] _tempEfInfo;
            string _rsEfInfo = "";
            if (efType == 1) //圆形
            {
                _tempEfInfo = efInfo.Split(';');
                for (int i = 0; i < _tempEfInfo.Length - 1; i++) //最后一个是圆形半径，去掉
                {
                    string[] _tempCoordinate = _tempEfInfo[i].Split(',');
                    double _lng = double.Parse(_tempCoordinate[0]);
                    double _lat = double.Parse(_tempCoordinate[1]);
                    if (changeType == 1)
                    {
                        //NetDecry.Fix(ref _lng, ref _lat, false);
                        Rectify.Gcj02_To_Wgs84(ref _lat, ref _lng);
                    } //将地图坐标置成车机坐标
                    else if (changeType == 2)
                    {
                        //NetDecry.Fix(ref _lng, ref _lat, true);
                        Rectify.Wgs84_To_Gcj02(ref _lat, ref _lng);
                    } //将车机坐标置成地图坐标
                    _rsEfInfo += _lng + "," + _lat + ";";
                }
                _rsEfInfo += _tempEfInfo[_tempEfInfo.Length - 1]; //加上半径信息
            }
            else if (efType == 2 || efType == 3) //矩形、多边形
            {
                _tempEfInfo = efInfo.Split(';');
                for (int i = 0; i < _tempEfInfo.Length; i++)
                {
                    string[] _tempCoordinate = _tempEfInfo[i].Split(',');
                    double _lng = double.Parse(_tempCoordinate[0]);
                    double _lat = double.Parse(_tempCoordinate[1]);
                    if (changeType == 1)
                    {
                        //NetDecry.Fix(ref _lng, ref _lat, false); 
                        Rectify.Gcj02_To_Wgs84(ref _lat, ref _lng);
                    } //将地图坐标置成车机坐标
                    else if (changeType == 2)
                    {
                        //NetDecry.Fix(ref _lng, ref _lat, true); 
                        Rectify.Wgs84_To_Gcj02(ref _lat, ref _lng);
                    } //将车机坐标置成地图坐标
                    _rsEfInfo += _lng + "," + _lat + ";";
                }
                _rsEfInfo = _rsEfInfo.Substring(0, _rsEfInfo.Length - 1); //去掉最后一个";"
            }
            return _rsEfInfo;
        }
        #endregion

    }
}

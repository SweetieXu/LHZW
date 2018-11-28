using Asiatek.AjaxPager;
using Asiatek.Components.GIS;
using Asiatek.DBUtility;
using Asiatek.Model;
using Asiatek.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Asiatek.BLL.MSSQL
{
    public class MapRegionsBLL
    {
        /// <summary>
        /// 获取地图区域列表基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="searchPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        #region 查询
        public static AsiatekPagedList<MapRegionsListModel> GetPagedMapRegions(MapRegionsSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","MapRegionsList"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","ID DESC"),
                new SqlParameter("@showColumns",@"ID,
                RegionsType,
                RegionsName,
                CAST(StartDate AS CHAR(10)) AS StartDate,
                CAST(StartTime AS CHAR(8)) AS StartTime,
                CAST(EndDate AS CHAR(10)) AS EndDate,
                CAST(EndTime AS CHAR(8)) AS EndTime,
                Periodic,
                SpeedLimit,
                OverSpeedDuration "),
            };

            #region 筛选条件
            string conditionStr = "Status=0";
            if (model.SearchRegionsType != 0)
            {
                conditionStr += " AND RegionsType =" + model.SearchRegionsType + "";
            }

            if (!string.IsNullOrWhiteSpace(model.SearchRegionsName))
            {
                conditionStr += " AND RegionsName LIKE '%" + model.SearchRegionsName + "%'";
            }

            if (model.SearchSpeedLimit != 0.0)
            {
                conditionStr += " AND SpeedLimit=" + model.SearchSpeedLimit + "";
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
            List<MapRegionsListModel> list = ConvertToList<MapRegionsListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 根据区域类别 分圆形、矩形、多边形三种情况添加
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentUserID"></param>
        /// <param name="currentStrucID"></param>
        /// <returns></returns>
        public static OperationResult AddMapRegions(MapRegionsAddModel model, int currentUserID, int currentStrucID)
        {
            #region 参数
            //纠偏
            double centerLng = double.Parse(model.CenterLongitude.ToString());
            double centerLat = double.Parse(model.CenterLatitude.ToString());
            Rectify.Gcj02_To_Wgs84(ref centerLat, ref centerLng);
            //NetDecry.Fix(ref centerLng, ref centerLat, false);

            double leftLng = double.Parse(model.LeftUpperLongitude.ToString());
            double leftLat = double.Parse(model.LeftUpperLatitude.ToString());
            double rightLng = double.Parse(model.RightLowerLongitude.ToString());
            double rightLat = double.Parse(model.RightLowerLatitude.ToString());
            Rectify.Gcj02_To_Wgs84(ref leftLat,ref leftLng);
            Rectify.Gcj02_To_Wgs84(ref rightLat,ref rightLng);
            //NetDecry.Fix(ref leftLng, ref leftLat, false);
            //NetDecry.Fix(ref rightLng, ref rightLat, false);

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@RegionsType",SqlDbType.Int),
                new SqlParameter("@RegionsName",SqlDbType.NVarChar,100),
                new SqlParameter("@CenterLatitude",SqlDbType.Float),
                new SqlParameter("@CenterLongitude",SqlDbType.Float),
                new SqlParameter("@LeftUpperLatitude",SqlDbType.Float),

                new SqlParameter("@LeftUpperLongitude",SqlDbType.Float),
                new SqlParameter("@RightLowerLatitude",SqlDbType.Float),
                new SqlParameter("@RightLowerLongitude",SqlDbType.Float),
                new SqlParameter("@Radius",SqlDbType.Float),
                new SqlParameter("@StartDate",SqlDbType.Date),

                new SqlParameter("@StartTime",SqlDbType.Time),
                new SqlParameter("@EndDate",SqlDbType.Date),
                new SqlParameter("@EndTime",SqlDbType.Time),
                new SqlParameter("@Periodic",SqlDbType.Bit),
                new SqlParameter("@SpeedLimit",SqlDbType.Float),

                new SqlParameter("@OverSpeedDuration",SqlDbType.Int),
                new SqlParameter("@Remark",SqlDbType.NVarChar,1000),
                new SqlParameter("@Status",SqlDbType.TinyInt),
                new SqlParameter("@CreateTime",SqlDbType.DateTime),
                new SqlParameter("@UpdateTime",SqlDbType.DateTime),

                new SqlParameter("@CreateUserID",SqlDbType.Int),
                new SqlParameter("@StrucID",SqlDbType.Int),
            };

            paras[0].Value = model.RegionsType;
            paras[1].Value = model.RegionsName;
            paras[2].Value = centerLng;
            paras[3].Value = centerLat;
            paras[4].Value = model.LeftUpperLatitude;

            paras[5].Value = model.LeftUpperLongitude;
            paras[6].Value = model.RightLowerLatitude;
            paras[7].Value = model.RightLowerLongitude;
            paras[8].Value = model.Radius;
            if (string.IsNullOrEmpty(model.StartDate) || model.Periodic == true)
            {
                paras[9].Value = DBNull.Value;
            }
            else
            {
                paras[9].Value = model.StartDate;
            }

            if (string.IsNullOrEmpty(model.StartTime))
            {
                paras[10].Value = DBNull.Value;
            }
            else
            {
                paras[10].Value = model.StartTime;
            }
            if (string.IsNullOrEmpty(model.EndDate) || model.Periodic == true)
            {
                paras[11].Value = DBNull.Value;
            }
            else
            {
                paras[11].Value = model.EndDate;
            }
            if (string.IsNullOrEmpty(model.EndTime))
            {
                paras[12].Value = DBNull.Value;
            }
            else
            {
                paras[12].Value = model.EndTime;
            }
            paras[13].Value = model.Periodic;
            paras[14].Value = model.SpeedLimit;

            paras[15].Value = model.OverSpeedDuration;
            if (string.IsNullOrEmpty(model.Remark))
            {
                paras[16].Value = DBNull.Value;
            }
            else
            {
                paras[16].Value = model.Remark;
            }
            paras[17].Value = 0;
            paras[18].Value = DateTime.Now;
            paras[19].Value = DateTime.Now;

            paras[20].Value = currentUserID;
            paras[21].Value = currentStrucID;
            #endregion

            string sql = string.Empty;
            bool result = false;

            #region  SQL 圆形区域 中心点经纬度和半径
            if (model.RegionsType == 1)
            {
                sql = @"INSERT  INTO dbo.MapRegionsList
                    ( RegionsType ,
                      RegionsName ,
                      StrucID,
                      CenterLatitude ,
                      CenterLongitude ,
                      Radius ,
                      StartDate ,
                      StartTime ,
                      EndDate ,
                      EndTime ,
                      Periodic ,
                      SpeedLimit ,
                      OverSpeedDuration ,
                      Remark ,
                      Status ,
                      CreateTime ,
                      UpdateTime ,
                      CreateUserID
                    )
            VALUES  ( @RegionsType , 
                      @RegionsName, 
                      @StrucID,
                      @CenterLatitude , 
                      @CenterLongitude, 
                      @Radius , 
                      @StartDate, 
                      @StartTime , 
                      @EndDate, 
                      @EndTime , 
                      @Periodic, 
                      @SpeedLimit , 
                      @OverSpeedDuration, 
                      @Remark , 
                      @Status, 
                      @CreateTime , 
                      @UpdateTime, 
                      @CreateUserID  
                    )";
                result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            }
            #endregion

            #region  SQL 矩形区域 左上点、右下点经纬度
            if (model.RegionsType == 2)
            {
                sql = @"INSERT  INTO dbo.MapRegionsList
                    ( RegionsType ,
                      RegionsName ,
                      StrucID,
                      LeftUpperLatitude ,
                      LeftUpperLongitude ,
                      RightLowerLatitude ,
                      RightLowerLongitude ,
                      StartDate ,
                      StartTime ,
                      EndDate ,
                      EndTime ,
                      Periodic ,
                      SpeedLimit ,
                      OverSpeedDuration ,
                      Remark ,
                      Status ,
                      CreateTime ,
                      UpdateTime ,
                      CreateUserID
                    )
            VALUES  ( @RegionsType , 
                      @RegionsName, 
                      @StrucID,
                      @LeftUpperLatitude , 
                      @LeftUpperLongitude, 
                      @RightLowerLatitude , 
                      @RightLowerLongitude ,
                      @StartDate, 
                      @StartTime , 
                      @EndDate, 
                      @EndTime , 
                      @Periodic, 
                      @SpeedLimit , 
                      @OverSpeedDuration, 
                      @Remark , 
                      @Status, 
                      @CreateTime , 
                      @UpdateTime, 
                      @CreateUserID  
                    )";
                result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            }
            #endregion

            #region  SQL 多边形区域 关联地图区域明细表 对应多个定位点、经纬度
            if (model.RegionsType == 3)
            {
                sql = @"INSERT  INTO dbo.MapRegionsList
                    ( RegionsType ,
                      RegionsName ,
                      StartDate ,
                      StartTime ,
                      EndDate ,
                      EndTime ,
                      Periodic ,
                      SpeedLimit ,
                      OverSpeedDuration ,
                      Remark ,
                      Status ,
                      CreateTime ,
                      UpdateTime ,
                      CreateUserID,
                      StrucID
                    )
            VALUES  ( @RegionsType , 
                      @RegionsName, 
                      @StartDate, 
                      @StartTime , 
                      @EndDate, 
                      @EndTime , 
                      @Periodic, 
                      @SpeedLimit , 
                      @OverSpeedDuration, 
                      @Remark , 
                      @Status, 
                      @CreateTime , 
                      @UpdateTime, 
                      @CreateUserID,
                      @StrucID 
                    );SELECT  SCOPE_IDENTITY();";

                //插数据到地图区域明细表（多边形点位）
                //string[] polygons = ((string[])(model.PolygonList))[0].Split(',');
                string[] polygons = model.PolygonList[0].Split(',');
                int len = polygons.Length / 2;

                int sqlLen = len + 1;
                string[] sqls = new string[sqlLen];
                SqlParameter[][] polygonsParas = new SqlParameter[sqlLen][];
                int num = 1;//sqls长度
                sqls[0] = sql;
                polygonsParas[0] = new SqlParameter[15];
                polygonsParas[0][0] = new SqlParameter()
                {
                    ParameterName = "@RegionsType",
                    SqlDbType = SqlDbType.Int,
                    Value = model.RegionsType
                };
                polygonsParas[0][1] = new SqlParameter()
                {
                    ParameterName = "@RegionsName",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Value = model.RegionsName
                };
                if (string.IsNullOrEmpty(model.StartDate) || model.Periodic == true)
                {
                    polygonsParas[0][2] = new SqlParameter()
                    {
                        ParameterName = "@StartDate",
                        SqlDbType = SqlDbType.Date,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    polygonsParas[0][2] = new SqlParameter()
                    {
                        ParameterName = "@StartDate",
                        SqlDbType = SqlDbType.Date,
                        Value = model.StartDate
                    };
                }
                if (string.IsNullOrEmpty(model.StartTime))
                {
                    polygonsParas[0][3] = new SqlParameter()
                    {
                        ParameterName = "@StartTime",
                        SqlDbType = SqlDbType.Time,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    polygonsParas[0][3] = new SqlParameter()
                    {
                        ParameterName = "@StartTime",
                        SqlDbType = SqlDbType.Time,
                        Value = model.StartTime
                    };
                }
                if (string.IsNullOrEmpty(model.EndDate) || model.Periodic == true)
                {
                    polygonsParas[0][4] = new SqlParameter()
                    {
                        ParameterName = "@EndDate",
                        SqlDbType = SqlDbType.Date,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    polygonsParas[0][4] = new SqlParameter()
                    {
                        ParameterName = "@EndDate",
                        SqlDbType = SqlDbType.Date,
                        Value = model.EndDate
                    };
                }
                if (string.IsNullOrEmpty(model.EndTime))
                {
                    polygonsParas[0][5] = new SqlParameter()
                    {
                        ParameterName = "@EndTime",
                        SqlDbType = SqlDbType.Time,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    polygonsParas[0][5] = new SqlParameter()
                    {
                        ParameterName = "@EndTime",
                        SqlDbType = SqlDbType.Time,
                        Value = model.EndTime
                    };
                }

                polygonsParas[0][6] = new SqlParameter()
                {
                    ParameterName = "@Periodic",
                    SqlDbType = SqlDbType.Bit,
                    Value = model.Periodic
                };
                polygonsParas[0][7] = new SqlParameter()
                {
                    ParameterName = "@SpeedLimit",
                    SqlDbType = SqlDbType.Float,
                    Value = model.SpeedLimit
                };
                polygonsParas[0][8] = new SqlParameter()
                {
                    ParameterName = "@OverSpeedDuration",
                    SqlDbType = SqlDbType.Int,
                    Value = model.OverSpeedDuration
                };
                if (string.IsNullOrEmpty(model.Remark))
                {
                    polygonsParas[0][9] = new SqlParameter()
                    {
                        ParameterName = "@Remark",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 500,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    polygonsParas[0][9] = new SqlParameter()
                    {
                        ParameterName = "@Remark",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 500,
                        Value = model.Remark
                    };
                }

                polygonsParas[0][10] = new SqlParameter()
                {
                    ParameterName = "@Status",
                    SqlDbType = SqlDbType.Int,
                    Value = 0
                };
                polygonsParas[0][11] = new SqlParameter()
                {
                    ParameterName = "@CreateTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DateTime.Now
                };
                polygonsParas[0][12] = new SqlParameter()
                {
                    ParameterName = "@UpdateTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DateTime.Now
                };
                polygonsParas[0][13] = new SqlParameter()
                {
                    ParameterName = "@CreateUserID",
                    SqlDbType = SqlDbType.Int,
                    Value = currentUserID
                };
                polygonsParas[0][14] = new SqlParameter()
                {
                    ParameterName = "@StrucID",
                    SqlDbType = SqlDbType.Int,
                    Value = currentStrucID
                };

                int orderID = 0;//定位点序号
                for (int i = 0; i < polygons.Length; i++)
                {
                    string tempSql = string.Empty;
                    tempSql = @"INSERT  INTO dbo.MapRegionsDetails( RegionsID, OrderID, Longitude, Latitude ) VALUES  ( @RegionsID,@OrderID,@Longitude,@Latitude)";
                    List<SqlParameter> tempParas = new List<SqlParameter>()
                    {
                        new SqlParameter("@RegionsID",SqlDbType.BigInt),
                        new SqlParameter("@OrderID",SqlDbType.TinyInt),
                        new SqlParameter("@Longitude",SqlDbType.Float),
                        new SqlParameter("@Latitude",SqlDbType.Float),

                    };
                    sqls[num] = tempSql;
                    polygonsParas[num] = new SqlParameter[4];
                    polygonsParas[num][0] = new SqlParameter { ParameterName = "@RegionsID", SqlDbType = SqlDbType.BigInt };
                    //纠偏
                    double lng = double.Parse(polygons[i].ToString());
                    double lat = double.Parse(polygons[i + 1].ToString());
                    //NetDecry.Fix(ref lng, ref lat, false);
                    Rectify.Gcj02_To_Wgs84(ref lat, ref lng);
                    polygonsParas[num][1] = new SqlParameter()
                    {
                        ParameterName = "@OrderID",
                        SqlDbType = SqlDbType.TinyInt,
                        Value = orderID
                    };
                    polygonsParas[num][2] = new SqlParameter()
                    {
                        ParameterName = "@Longitude",
                        SqlDbType = SqlDbType.Float,
                        Value = lng
                    };
                    polygonsParas[num][3] = new SqlParameter()
                    {
                        ParameterName = "@Latitude",
                        SqlDbType = SqlDbType.Float,
                        Value = lat
                    };
                    i++;
                    num++;
                    orderID++;

                }
                result = MSSQLHelper.ExecuteIdentityIncludeTransaction(CommandType.Text, sqls, polygonsParas) != 0;
            }
            #endregion

            //bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }


        /// <summary>
        /// 添加时 验证用户所属单位下是否有重复的区域名称
        /// </summary>
        /// <param name="regionsName"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static bool CheckAddRegionsNameExists(string regionsName, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@RegionsName",SqlDbType.NVarChar,50),
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            paras[0].Value = regionsName;
            paras[1].Value = strucID;

            string sql = "SELECT COUNT(0) FROM dbo.MapRegionsList WHERE RegionsName=@RegionsName AND StrucID=@StrucID AND Status=0";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }

        #endregion

        #region 修改
        public static SelectResult<MapRegionsEditModel> GetMapRegionsByID(int id, int regionsType)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName="@ID",
                    SqlDbType=SqlDbType.BigInt,
                },
            };
            paras[0].Value = id;
            string sql = string.Empty;
            if (regionsType == 1)
            {
                sql = @"SELECT ID,RegionsType,
                RegionsName,CenterLatitude,CenterLongitude,Radius,
                CAST(StartDate AS CHAR(10)) AS StartDate,
                CAST(StartTime AS CHAR(8)) AS StartTime,
                CAST(EndDate AS CHAR(10)) AS EndDate,
                CAST(EndTime AS CHAR(8)) AS EndTime,
                Periodic,
                SpeedLimit,
                OverSpeedDuration
  FROM dbo.MapRegionsList
  WHERE Status=0 AND ID=@ID";
            }
            if (regionsType == 2)
            {
                sql = @"SELECT ID,RegionsType,
                RegionsName,LeftUpperLatitude,LeftUpperLongitude,RightLowerLatitude,RightLowerLongitude,
                CAST(StartDate AS CHAR(10)) AS StartDate,
                CAST(StartTime AS CHAR(8)) AS StartTime,
                CAST(EndDate AS CHAR(10)) AS EndDate,
                CAST(EndTime AS CHAR(8)) AS EndTime,
                Periodic,
                SpeedLimit,
                OverSpeedDuration
  FROM dbo.MapRegionsList
  WHERE Status=0 AND ID=@ID";
            }
            if (regionsType == 3)
            {
                sql = @"SELECT MPL.ID,MPL.RegionsType,
                MPL.RegionsName,
                CAST(MPL.StartDate AS CHAR(10)) AS StartDate,
                CAST(MPL.StartTime AS CHAR(8)) AS StartTime,
                CAST(MPL.EndDate AS CHAR(10)) AS EndDate,
                CAST(MPL.EndTime AS CHAR(8)) AS EndTime,
                MPL.Periodic,
                MPL.SpeedLimit,
                MPL.OverSpeedDuration,MPD.Latitude,MPD.Longitude,MPD.OrderID
  FROM dbo.MapRegionsList MPL 
  LEFT JOIN dbo.MapRegionsDetails MPD 
  ON MPL.ID=MPD.RegionsID
  WHERE MPL.Status=0 AND MPL.ID=@ID";
            }

            List<MapRegionsEditModel> list = ConvertToList<MapRegionsEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            MapRegionsEditModel data = null;
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
                if (regionsType == 1 || regionsType == 2)
                {
                    //纠偏
                    double centerLng = double.Parse(list[0].CenterLongitude.ToString());
                    double centerLat = double.Parse(list[0].CenterLatitude.ToString());
                    //NetDecry.Fix(ref centerLng, ref centerLat, true);
                    Rectify.Wgs84_To_Gcj02(ref centerLat, ref centerLng);
                    double leftLng = double.Parse(list[0].LeftUpperLongitude.ToString());
                    double leftLat = double.Parse(list[0].LeftUpperLatitude.ToString());
                    double rightLng = double.Parse(list[0].RightLowerLongitude.ToString());
                    double rightLat = double.Parse(list[0].RightLowerLatitude.ToString());
                    Rectify.Wgs84_To_Gcj02(ref leftLat, ref leftLng);
                    Rectify.Wgs84_To_Gcj02(ref rightLat, ref rightLng);
                    //NetDecry.Fix(ref leftLng, ref leftLat, true);
                    //NetDecry.Fix(ref rightLng, ref rightLat, true);
                    list[0].CenterLongitude = (float)centerLng;
                    list[0].CenterLatitude = (float)centerLat;
                    list[0].LeftUpperLongitude = (float)leftLng;
                    list[0].LeftUpperLatitude = (float)leftLat;
                    list[0].RightLowerLongitude = (float)rightLng;
                    list[0].RightLowerLatitude = (float)rightLat;

                    data = list[0];
                }
                else
                {
                    int len = list.Count;
                    //List<string> points = new List<string>();
                    string points = string.Empty;
                    for (int i = 0; i < len; i++)
                    {
                        //纠偏
                        double lng = double.Parse(list[i].Longitude.ToString());
                        double lat = double.Parse(list[i].Latitude.ToString());
                        //NetDecry.Fix(ref lng, ref lat, true);
                        Rectify.Wgs84_To_Gcj02(ref lat, ref lng);
                        points = points + lng.ToString() + ",";
                        points = points + lat.ToString() + ",";
                        //points.Add(list[i].Longitude.ToString());
                        //points.Add(list[i].Latitude.ToString());
                    }
                    list[0].RePolygonList = points;
                    data = list[0];
                }

            }
            return new SelectResult<MapRegionsEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditMapRegions(MapRegionsEditModel model, int currentUserID, int currentStrucID)
        {
            #region 参数
            //纠偏
            double centerLng = double.Parse(model.CenterLongitude.ToString());
            double centerLat = double.Parse(model.CenterLatitude.ToString());
            //NetDecry.Fix(ref centerLng, ref centerLat, false);
            Rectify.Gcj02_To_Wgs84(ref centerLat, ref centerLng);
            double leftLng = double.Parse(model.LeftUpperLongitude.ToString());
            double leftLat = double.Parse(model.LeftUpperLatitude.ToString());
            double rightLng = double.Parse(model.RightLowerLongitude.ToString());
            double rightLat = double.Parse(model.RightLowerLatitude.ToString());
            //NetDecry.Fix(ref leftLng, ref leftLat, false);
            //NetDecry.Fix(ref rightLng, ref rightLat, false);
            Rectify.Gcj02_To_Wgs84(ref leftLat, ref leftLng);
            Rectify.Gcj02_To_Wgs84(ref rightLat, ref rightLng);
            
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@RegionsName",SqlDbType.NVarChar,100),
                new SqlParameter("@CenterLatitude",SqlDbType.Float),
                new SqlParameter("@CenterLongitude",SqlDbType.Float),
                new SqlParameter("@LeftUpperLatitude",SqlDbType.Float),
                new SqlParameter("@LeftUpperLongitude",SqlDbType.Float),

                new SqlParameter("@RightLowerLatitude",SqlDbType.Float),
                new SqlParameter("@RightLowerLongitude",SqlDbType.Float),
                new SqlParameter("@Radius",SqlDbType.Float),
                new SqlParameter("@StartDate",SqlDbType.Date),
                new SqlParameter("@StartTime",SqlDbType.Time),

                new SqlParameter("@EndDate",SqlDbType.Date),
                new SqlParameter("@EndTime",SqlDbType.Time),
                new SqlParameter("@Periodic",SqlDbType.Bit),
                new SqlParameter("@SpeedLimit",SqlDbType.Float),
                new SqlParameter("@OverSpeedDuration",SqlDbType.Int),

                new SqlParameter("@Remark",SqlDbType.NVarChar,1000),
                new SqlParameter("@UpdateUserID",SqlDbType.Int),
                new SqlParameter("@ID",SqlDbType.BigInt),
            };

            paras[0].Value = model.RegionsName;
            paras[1].Value = centerLat;
            paras[2].Value = centerLng;
            paras[3].Value = leftLat;
            paras[4].Value = leftLng;

            paras[5].Value = rightLat;
            paras[6].Value = rightLng;
            paras[7].Value = model.Radius;
            if (string.IsNullOrEmpty(model.StartDate) || model.Periodic == true)
            {
                paras[8].Value = DBNull.Value;
            }
            else
            {
                paras[8].Value = model.StartDate;
            }

            if (string.IsNullOrEmpty(model.StartTime))
            {
                paras[9].Value = DBNull.Value;
            }
            else
            {
                paras[9].Value = model.StartTime;
            }
            if (string.IsNullOrEmpty(model.EndDate) || model.Periodic == true)
            {
                paras[10].Value = DBNull.Value;
            }
            else
            {
                paras[10].Value = model.EndDate;
            }
            if (string.IsNullOrEmpty(model.EndTime))
            {
                paras[11].Value = DBNull.Value;
            }
            else
            {
                paras[11].Value = model.EndTime;
            }
            paras[12].Value = model.Periodic;
            paras[13].Value = model.SpeedLimit;

            paras[14].Value = model.OverSpeedDuration;
            if (string.IsNullOrEmpty(model.Remark))
            {
                paras[15].Value = DBNull.Value;
            }
            else
            {
                paras[15].Value = model.Remark;
            }
            paras[16].Value = currentUserID;
            paras[17].Value = model.ID;
            #endregion

            string sql = string.Empty;
            int result = 0;

            #region  SQL 圆形区域 中心点经纬度和半径
            if (model.RegionsType == 1)
            {
                sql = @"UPDATE dbo.MapRegionsList
                      SET RegionsName = @RegionsName ,
                      CenterLatitude = @CenterLatitude ,
                      CenterLongitude = @CenterLongitude ,
                      Radius = @Radius ,
                      StartDate = @StartDate ,
                      StartTime = @StartTime ,
                      EndDate = @EndDate ,
                      EndTime = @EndTime ,
                      Periodic = @Periodic ,
                      SpeedLimit = @SpeedLimit ,
                      OverSpeedDuration = @OverSpeedDuration ,
                      Remark = @Remark ,
                      UpdateTime = GetDate() ,
                      UpdateUserID = @UpdateUserID 
                     WHERE  ID = @ID ";
                result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
            }
            #endregion

            #region  SQL 矩形区域 左上点、右下点经纬度
            if (model.RegionsType == 2)
            {
                sql = @"UPDATE dbo.MapRegionsList
                      SET RegionsName = @RegionsName ,
                      LeftUpperLatitude = @LeftUpperLatitude ,
                      LeftUpperLongitude = @LeftUpperLongitude ,
                      RightLowerLatitude = @RightLowerLatitude ,
                      RightLowerLongitude = @RightLowerLongitude ,
                      StartDate = @StartDate ,
                      StartTime = @StartTime ,
                      EndDate = @EndDate ,
                      EndTime = @EndTime ,
                      Periodic = @Periodic ,
                      SpeedLimit = @SpeedLimit ,
                      OverSpeedDuration = @OverSpeedDuration ,
                      Remark = @Remark ,
                      UpdateTime = GetDate() ,
                      UpdateUserID = @UpdateUserID 
                     WHERE  ID = @ID ";
                result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
            }
            #endregion

            #region  SQL 多边形区域 关联地图区域明细表 对应多个定位点、经纬度
            if (model.RegionsType == 3)
            {
                #region 更新主表
                sql = @"UPDATE dbo.MapRegionsList
                      SET RegionsName = @RegionsName ,
                      StartDate = @StartDate ,
                      StartTime = @StartTime ,
                      EndDate = @EndDate ,
                      EndTime = @EndTime ,
                      Periodic = @Periodic ,
                      SpeedLimit = @SpeedLimit ,
                      OverSpeedDuration = @OverSpeedDuration ,
                      Remark = @Remark ,
                      UpdateTime = GetDate() ,
                      UpdateUserID = @UpdateUserID 
                     WHERE  ID = @ID ";
                var updateSqls = sql;
                SqlParameter[] updatePolygonsParas = new SqlParameter[11];
                updatePolygonsParas[0] = new SqlParameter()
                {
                    ParameterName = "@RegionsName",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Value = model.RegionsName
                };
                if (string.IsNullOrEmpty(model.StartDate) || model.Periodic == true)
                {
                    updatePolygonsParas[1] = new SqlParameter()
                    {
                        ParameterName = "@StartDate",
                        SqlDbType = SqlDbType.Date,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    updatePolygonsParas[1] = new SqlParameter()
                    {
                        ParameterName = "@StartDate",
                        SqlDbType = SqlDbType.Date,
                        Value = model.StartDate
                    };
                }
                if (string.IsNullOrEmpty(model.StartTime))
                {
                    updatePolygonsParas[2] = new SqlParameter()
                    {
                        ParameterName = "@StartTime",
                        SqlDbType = SqlDbType.Time,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    updatePolygonsParas[2] = new SqlParameter()
                    {
                        ParameterName = "@StartTime",
                        SqlDbType = SqlDbType.Time,
                        Value = model.StartTime
                    };
                }
                if (string.IsNullOrEmpty(model.EndDate) || model.Periodic == true)
                {
                    updatePolygonsParas[3] = new SqlParameter()
                    {
                        ParameterName = "@EndDate",
                        SqlDbType = SqlDbType.Date,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    updatePolygonsParas[3] = new SqlParameter()
                    {
                        ParameterName = "@EndDate",
                        SqlDbType = SqlDbType.Date,
                        Value = model.EndDate
                    };
                }
                if (string.IsNullOrEmpty(model.EndTime))
                {
                    updatePolygonsParas[4] = new SqlParameter()
                    {
                        ParameterName = "@EndTime",
                        SqlDbType = SqlDbType.Time,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    updatePolygonsParas[4] = new SqlParameter()
                    {
                        ParameterName = "@EndTime",
                        SqlDbType = SqlDbType.Time,
                        Value = model.EndTime
                    };
                }
                updatePolygonsParas[5] = new SqlParameter()
                {
                    ParameterName = "@Periodic",
                    SqlDbType = SqlDbType.Bit,
                    Value = model.Periodic
                };
                updatePolygonsParas[6] = new SqlParameter()
                {
                    ParameterName = "@SpeedLimit",
                    SqlDbType = SqlDbType.Float,
                    Value = model.SpeedLimit
                };
                updatePolygonsParas[7] = new SqlParameter()
                {
                    ParameterName = "@OverSpeedDuration",
                    SqlDbType = SqlDbType.Int,
                    Value = model.OverSpeedDuration
                };
                if (string.IsNullOrEmpty(model.Remark))
                {
                    updatePolygonsParas[8] = new SqlParameter()
                    {
                        ParameterName = "@Remark",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 500,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    updatePolygonsParas[8] = new SqlParameter()
                    {
                        ParameterName = "@Remark",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 500,
                        Value = model.Remark
                    };
                }
                updatePolygonsParas[9] = new SqlParameter()
                {
                    ParameterName = "@UpdateUserID",
                    SqlDbType = SqlDbType.Int,
                    Value = currentUserID
                };
                updatePolygonsParas[10] = new SqlParameter()
                {
                    ParameterName = "@ID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.ID
                };
                int updateResult = MSSQLHelper.ExecuteNonQuery(CommandType.Text, updateSqls, updatePolygonsParas);
                #endregion


                #region 删除
                var delSql = @"DELETE FROM dbo.MapRegionsDetails WHERE RegionsID=@RegionsID";
                SqlParameter[] delPolygonsParas = new SqlParameter[1];
                delPolygonsParas[0] = new SqlParameter()
                {
                    ParameterName = "@RegionsID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.ID
                };
                int delResult = MSSQLHelper.ExecuteNonQuery(CommandType.Text, delSql, delPolygonsParas);
                #endregion


                #region 添加
                //插数据到地图区域明细表（多边形点位）
                string[] polygons = model.PolygonList[0].Split(',');
                int len = polygons.Length / 2;
                string[] insertSqls = new string[len];
                SqlParameter[][] insertPolygonsParas = new SqlParameter[len][];
                int num = 0;//sqls长度

                int orderID = 0;//定位点序号
                for (int i = 0; i < polygons.Length; i++)
                {
                    string tempSql = string.Empty;
                    tempSql = @"INSERT  INTO dbo.MapRegionsDetails( RegionsID, OrderID, Longitude, Latitude ) VALUES  ( @RegionsID,@OrderID,@Longitude,@Latitude)";
                    insertSqls[num] = tempSql;
                    insertPolygonsParas[num] = new SqlParameter[4];
                    insertPolygonsParas[num][0] = new SqlParameter()
                    {
                        ParameterName = "@RegionsID",
                        SqlDbType = SqlDbType.BigInt,
                        Value = model.ID
                    };
                    //纠偏
                    double lng = double.Parse(polygons[i].ToString());
                    double lat = double.Parse(polygons[i + 1].ToString());
                    //NetDecry.Fix(ref lng, ref lat, false);
                    Rectify.Gcj02_To_Wgs84(ref lat, ref lng);
                    insertPolygonsParas[num][1] = new SqlParameter()
                    {
                        ParameterName = "@OrderID",
                        SqlDbType = SqlDbType.TinyInt,
                        Value = orderID
                    };
                    insertPolygonsParas[num][2] = new SqlParameter()
                    {
                        ParameterName = "@Longitude",
                        SqlDbType = SqlDbType.Float,
                        Value = lng
                    };
                    insertPolygonsParas[num][3] = new SqlParameter()
                    {
                        ParameterName = "@Latitude",
                        SqlDbType = SqlDbType.Float,
                        Value = lat
                    };
                    i++;
                    num++;
                    orderID++;
                }
                #endregion

                bool insertResult = MSSQLHelper.ExecuteTransaction(CommandType.Text, insertSqls, insertPolygonsParas);
                if (updateResult == 1 && delResult != 0 && insertResult == true)
                    result = 1;
            }
            #endregion

            //int result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray());
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

        /// <summary>
        /// 编辑时 验证用户所属单位下是否有重复的区域名称
        /// </summary>
        /// <param name="regionsName"></param>
        /// <param name="id"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static bool CheckEditRegionsNameExists(string regionsName, int id, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@RegionsName",SqlDbType.NVarChar,50),
                new SqlParameter("@ID",SqlDbType.BigInt),
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            paras[0].Value = regionsName;
            paras[1].Value = id;
            paras[2].Value = strucID;

            string sql = "SELECT COUNT(0) FROM dbo.MapRegionsList WHERE ID!=@ID AND RegionsName=@RegionsName AND StrucID=@StrucID AND Status=0";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion

        #region 删除
        public static OperationResult DeleteMapRegions(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.MapRegionsList SET Status=9 WHERE ID=@ID";
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




        #region 下拉列表
        /// <summary>
        /// 根据单位ID获取单位及子单位的区域信息
        /// </summary>
        public static List<StrucMapRegionsModel> GetStrucMapRegions(int strucID, string regionName)
        {
            string sql = "SELECT * FROM dbo.Func_GetRegionInfoByStrucID(@strucID) WHERE RegionsName LIKE @regionName";
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@strucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@regionName",
                Value = "%" + regionName + "%",
            };
            return ConvertToList<StrucMapRegionsModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion
    }
}

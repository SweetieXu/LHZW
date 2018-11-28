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
    public class MapLinesBLL
    {
        #region 查询
        public static AsiatekPagedList<MapLinesListModel> GetPagedMapLines(MapLinesSearchModel model, int searchPage, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","MapLinesList"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",searchPage),
                new SqlParameter("@orderBy","ID DESC"),
                new SqlParameter("@showColumns",@"ID,
                LinesType,
                LinesName,
                StartTime,
                EndTime "),
            };

            #region 筛选条件
            string conditionStr = "Status=0";

            if (!string.IsNullOrWhiteSpace(model.LinesName))
            {
                conditionStr += " AND LinesName LIKE '%" + model.LinesName + "%'";
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
            List<MapLinesListModel> list = ConvertToList<MapLinesListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        #endregion

        #region 新增
        public static OperationResult AddMapLines(MapLinesAddModel model, int currentUserID, int currentStrucID)
        {
            int len = model.MapLinesDetails.Count + 1;
            string[] sqls = new string[len];
            SqlParameter[][] linesParas = new SqlParameter[len][];
            int num = 1;

            #region 主表添加
            sqls[0] = @"INSERT  INTO dbo.MapLinesList
                    ( LinesType ,
                      LinesName ,
                      StartTime ,
                      EndTime ,
                      Status ,
                      CreateTime ,
                      UpdateTime ,
                      CreateUserID,
                      UpdateUserID,
                      StrucID
                    )
            VALUES  ( @LinesType , 
                      @LinesName, 
                      @StartTime , 
                      @EndTime , 
                      @Status, 
                      @CreateTime , 
                      @UpdateTime, 
                      @CreateUserID,
                      @UpdateUserID,
                      @StrucID 
                    );SELECT  SCOPE_IDENTITY();";

            linesParas[0] = new SqlParameter[10];
            linesParas[0][0] = new SqlParameter()
            {
                ParameterName = "@LinesType",
                SqlDbType = SqlDbType.Int,
                Value = 4
            };
            linesParas[0][1] = new SqlParameter()
            {
                ParameterName = "@LinesName",
                SqlDbType = SqlDbType.NVarChar,
                Size = 50,
                Value = model.LinesName
            };
            if (string.IsNullOrEmpty(model.StartTime))
            {
                linesParas[0][2] = new SqlParameter()
                {
                    ParameterName = "@StartTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DBNull.Value
                };
            }
            else
            {
                linesParas[0][2] = new SqlParameter()
                {
                    ParameterName = "@StartTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.StartTime
                };
            }
            if (string.IsNullOrEmpty(model.EndTime))
            {
                linesParas[0][3] = new SqlParameter()
                {
                    ParameterName = "@EndTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DBNull.Value
                };
            }
            else
            {
                linesParas[0][3] = new SqlParameter()
                {
                    ParameterName = "@EndTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.EndTime
                };
            }
            linesParas[0][4] = new SqlParameter()
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = 0
            };
            linesParas[0][5] = new SqlParameter()
            {
                ParameterName = "@CreateTime",
                SqlDbType = SqlDbType.DateTime,
                Value = DateTime.Now
            };
            linesParas[0][6] = new SqlParameter()
            {
                ParameterName = "@UpdateTime",
                SqlDbType = SqlDbType.DateTime,
                Value = DateTime.Now
            };
            linesParas[0][7] = new SqlParameter()
            {
                ParameterName = "@CreateUserID",
                SqlDbType = SqlDbType.Int,
                Value = currentUserID
            };
            linesParas[0][8] = new SqlParameter()
            {
                ParameterName = "@UpdateUserID",
                SqlDbType = SqlDbType.Int,
                Value = currentUserID
            };
            linesParas[0][9] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                SqlDbType = SqlDbType.Int,
                Value = currentStrucID
            };
            #endregion

            #region 明细表添加
            int orderID = 0;//定位点序号
            for (int i = 0; i < model.MapLinesDetails.Count; i++)
            {
                string tempSql = string.Empty;
                tempSql = @"INSERT  INTO dbo.MapLinesDetails( LinesID, OrderID, Latitude, Longitude, RoadWidth, IsCheckTime, MaxSecond, MinSecond, IsCheckSpeed, SpeedLimit, OverSpeedDuration ) VALUES  ( @LinesID,@OrderID,@Latitude,@Longitude,@RoadWidth,@IsCheckTime,@MaxSecond,@MinSecond,@IsCheckSpeed,@SpeedLimit,@OverSpeedDuration)";
                sqls[num] = tempSql;
                linesParas[num] = new SqlParameter[11];
                linesParas[num][0] = new SqlParameter { ParameterName = "@LinesID", SqlDbType = SqlDbType.BigInt };
                linesParas[num][1] = new SqlParameter()
                {
                    ParameterName = "@OrderID",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = orderID
                };
                //纠偏
                double lat = double.Parse(model.MapLinesDetails[i].Latitude.ToString());
                double lng = double.Parse(model.MapLinesDetails[i].Longitude.ToString());
                //NetDecry.Fix(ref lng, ref lat, false);
                Rectify.Gcj02_To_Wgs84(ref lat, ref lng);
                linesParas[num][2] = new SqlParameter()
                {
                    ParameterName = "@Latitude",
                    SqlDbType = SqlDbType.Float,
                    Value = lat
                };
                linesParas[num][3] = new SqlParameter()
                {
                    ParameterName = "@Longitude",
                    SqlDbType = SqlDbType.Float,
                    Value = lng
                };
                if (model.MapLinesDetails[i].RoadWidth == null)
                {
                    linesParas[num][4] = new SqlParameter()
                    {
                        ParameterName = "@RoadWidth",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    linesParas[num][4] = new SqlParameter()
                    {
                        ParameterName = "@RoadWidth",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].RoadWidth
                    };
                }
                linesParas[num][5] = new SqlParameter()
                {
                    ParameterName = "@IsCheckTime",
                    SqlDbType = SqlDbType.Bit,
                    Value = false
                };
                if (model.MapLinesDetails[i].MaxSecond == null)
                {
                    linesParas[num][6] = new SqlParameter()
                    {
                        ParameterName = "@MaxSecond",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                }
                else {
                    linesParas[num][6] = new SqlParameter()
                    {
                        ParameterName = "@MaxSecond",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MapLinesDetails[i].MaxSecond
                    };
                }
                if (model.MapLinesDetails[i].MinSecond == null)
                {
                    linesParas[num][7] = new SqlParameter()
                    {
                        ParameterName = "@MinSecond",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    linesParas[num][7] = new SqlParameter()
                    {
                        ParameterName = "@MinSecond",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].MinSecond
                    };
                }
                linesParas[num][8] = new SqlParameter()
                {
                    ParameterName = "@IsCheckSpeed",
                    SqlDbType = SqlDbType.Bit,
                    Value = false
                };
                if (model.MapLinesDetails[i].SpeedLimit == null)
                {
                    linesParas[num][9] = new SqlParameter()
                    {
                        ParameterName = "@SpeedLimit",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    linesParas[num][9] = new SqlParameter()
                    {
                        ParameterName = "@SpeedLimit",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].SpeedLimit
                    };
                }
                if (model.MapLinesDetails[i].OverSpeedDuration == null)
                {
                    linesParas[num][10] = new SqlParameter()
                    {
                        ParameterName = "@OverSpeedDuration",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    linesParas[num][10] = new SqlParameter()
                    {
                        ParameterName = "@OverSpeedDuration",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MapLinesDetails[i].OverSpeedDuration
                    };
                }
                //判断行驶时间选中，限速、最高速度、超速持续时间控件不可用，数据未传递过来
                if (model.MapLinesDetails[i].IsCheckTime == true)
                {
                    linesParas[num][5] = new SqlParameter()
                    {
                        ParameterName = "@IsCheckTime",
                        SqlDbType = SqlDbType.Bit,
                        Value = true
                    };
                    linesParas[num][6] = new SqlParameter()
                    {
                        ParameterName = "@MaxSecond",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MapLinesDetails[i].MaxSecond
                    };
                    linesParas[num][7] = new SqlParameter()
                    {
                        ParameterName = "@MinSecond",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].MinSecond
                    };
                    linesParas[num][8] = new SqlParameter()
                    {
                        ParameterName = "@IsCheckSpeed",
                        SqlDbType = SqlDbType.Bit,
                        Value = false
                    };
                    linesParas[num][9] = new SqlParameter()
                    {
                        ParameterName = "@SpeedLimit",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                    linesParas[num][10] = new SqlParameter()
                    {
                        ParameterName = "@OverSpeedDuration",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                }
                //限速选中，判断行驶时间、路段行驶过长、路段行驶不足控件不可用，数据未传递过来
                if (model.MapLinesDetails[i].IsCheckSpeed == true)
                {
                    linesParas[num][5] = new SqlParameter()
                    {
                        ParameterName = "@IsCheckTime",
                        SqlDbType = SqlDbType.Bit,
                        Value = false
                    };
                    linesParas[num][6] = new SqlParameter()
                    {
                        ParameterName = "@MaxSecond",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                    linesParas[num][7] = new SqlParameter()
                    {
                        ParameterName = "@MinSecond",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                    linesParas[num][8] = new SqlParameter()
                    {
                        ParameterName = "@IsCheckSpeed",
                        SqlDbType = SqlDbType.Bit,
                        Value = true
                    };
                    linesParas[num][9] = new SqlParameter()
                    {
                        ParameterName = "@SpeedLimit",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].SpeedLimit
                    };
                    linesParas[num][10] = new SqlParameter()
                    {
                        ParameterName = "@OverSpeedDuration",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MapLinesDetails[i].OverSpeedDuration
                    };
                }
                num++;
                orderID++;
            }

            #endregion

            bool result = MSSQLHelper.ExecuteIdentityIncludeTransaction(CommandType.Text, sqls,linesParas) != 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        /// <summary>
        /// 添加时 验证用户所属单位下是否有重复的路线名称
        /// </summary>
        /// <param name="linesName"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static bool CheckAddLinesNameExists(string linesName, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@LinesName",SqlDbType.NVarChar,50),
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            paras[0].Value = linesName;
            paras[1].Value = strucID;

            string sql = "SELECT COUNT(0) FROM dbo.MapLinesList WHERE LinesName=@LinesName AND StrucID=@StrucID AND Status=0";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        
        #endregion


        #region 修改
        public static SelectResult<MapLinesEditModel> GetMapLinesByID(int id)
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
            string sql = @"SELECT ID ,LinesType ,LinesName ,StartTime ,EndTime ,Remark
      FROM dbo.MapLinesList
      WHERE Status=0 AND ID=@ID";

            List<MapLinesEditModel> list = ConvertToList<MapLinesEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            MapLinesEditModel data = null;
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
                List<SqlParameter> detailParas = new List<SqlParameter>()
                {
                    new SqlParameter()
                    {
                        ParameterName="@ID",
                        SqlDbType=SqlDbType.BigInt,
                    },
                };
                detailParas[0].Value = id;
                string detailSql = @"SELECT OrderID ,Latitude ,Longitude ,RoadWidth ,IsCheckTime,
MaxSecond,MinSecond,IsCheckSpeed,SpeedLimit,OverSpeedDuration ,IsSouthLatitude,IsWestLongitude
FROM dbo.MapLinesDetails
WHERE LinesID=@ID";

                List<MapLinesDetailsEditModel> detailList = ConvertToList<MapLinesDetailsEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, detailSql, detailParas.ToArray()));

                if (detailList != null && detailList.Count != 0) {
                    string points = string.Empty;
                    for (int i = 0; i < detailList.Count; i++)
                    {
                        //纠偏
                        double lng = double.Parse(detailList[i].Longitude.ToString());
                        double lat = double.Parse(detailList[i].Latitude.ToString());
                        //NetDecry.Fix(ref lng, ref lat, true);
                        Rectify.Wgs84_To_Gcj02(ref lat, ref lng);
                        detailList[i].Longitude = (float)lng;
                        detailList[i].Latitude = (float)lat;

                        points = points + lng.ToString() + ",";
                        points = points + lat.ToString() + ",";
                    }
                    list[0].MapLinesPoints = points;
                    list[0].MapLinesDetails = detailList;
                }
                data = list[0];

            }
            return new SelectResult<MapLinesEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        public static OperationResult EditMapLines(MapLinesEditModel model, int currentUserID, int currentStrucID)
        {
            int result = 0;

            #region 更新主表
            var updateSql = @"UPDATE dbo.MapLinesList
                      SET LinesName = @LinesName ,
                      StartTime = @StartTime ,
                      EndTime = @EndTime ,
                      UpdateTime = GetDate() ,
                      UpdateUserID = @UpdateUserID 
                     WHERE  ID = @ID ";
            SqlParameter[] updateParas = new SqlParameter[5];
            updateParas[0] = new SqlParameter()
            {
                ParameterName = "@LinesName",
                SqlDbType = SqlDbType.NVarChar,
                Size = 50,
                Value = model.LinesName
            };
            if (string.IsNullOrEmpty(model.StartTime))
            {
                updateParas[1] = new SqlParameter()
                {
                    ParameterName = "@StartTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DBNull.Value
                };
            }
            else
            {
                updateParas[1] = new SqlParameter()
                {
                    ParameterName = "@StartTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.StartTime
                };
            }
            if (string.IsNullOrEmpty(model.EndTime))
            {
                updateParas[2] = new SqlParameter()
                {
                    ParameterName = "@EndTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = DBNull.Value
                };
            }
            else
            {
                updateParas[2] = new SqlParameter()
                {
                    ParameterName = "@EndTime",
                    SqlDbType = SqlDbType.DateTime,
                    Value = model.EndTime
                };
            }
            updateParas[3] = new SqlParameter()
            {
                ParameterName = "@UpdateUserID",
                SqlDbType = SqlDbType.Int,
                Value = currentUserID
            };
            updateParas[4] = new SqlParameter()
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.BigInt,
                Value = model.ID
            };
            int updateResult = MSSQLHelper.ExecuteNonQuery(CommandType.Text, updateSql, updateParas);
            #endregion


            #region 删除
            var delSql = @"DELETE FROM dbo.MapLinesDetails WHERE LinesID=@LinesID";
            SqlParameter[] delParas = new SqlParameter[1];
            delParas[0] = new SqlParameter()
            {
                ParameterName = "@LinesID",
                SqlDbType = SqlDbType.BigInt,
                Value = model.ID
            };
            int delResult = MSSQLHelper.ExecuteNonQuery(CommandType.Text, delSql, delParas);
            #endregion


            #region 添加明细表
            int orderID = 0;//定位点序号
            int len = model.MapLinesDetails.Count;
            string[] sqls = new string[len];
            SqlParameter[][] linesParas = new SqlParameter[len][];
            int num = 0;
            for (int i = 0; i < len; i++)
            {
                string tempSql = string.Empty;
                tempSql = @"INSERT  INTO dbo.MapLinesDetails( LinesID, OrderID, Latitude, Longitude, RoadWidth, IsCheckTime, MaxSecond, MinSecond, IsCheckSpeed, SpeedLimit, OverSpeedDuration ) VALUES  ( @LinesID,@OrderID,@Latitude,@Longitude,@RoadWidth,@IsCheckTime,@MaxSecond,@MinSecond,@IsCheckSpeed,@SpeedLimit,@OverSpeedDuration)";
                sqls[num] = tempSql;
                linesParas[num] = new SqlParameter[11];
                linesParas[num][0] = new SqlParameter()
                {
                    ParameterName = "@LinesID",
                    SqlDbType = SqlDbType.BigInt,
                    Value = model.ID
                };
                linesParas[num][1] = new SqlParameter()
                {
                    ParameterName = "@OrderID",
                    SqlDbType = SqlDbType.TinyInt,
                    Value = orderID
                };
                //纠偏
                double lat = double.Parse(model.MapLinesDetails[i].Latitude.ToString());
                double lng = double.Parse(model.MapLinesDetails[i].Longitude.ToString());
                //NetDecry.Fix(ref lng, ref lat, false);
                Rectify.Gcj02_To_Wgs84(ref lat, ref lng);
                linesParas[num][2] = new SqlParameter()
                {
                    ParameterName = "@Latitude",
                    SqlDbType = SqlDbType.Float,
                    Value = lat
                };
                linesParas[num][3] = new SqlParameter()
                {
                    ParameterName = "@Longitude",
                    SqlDbType = SqlDbType.Float,
                    Value = lng
                };
                if (model.MapLinesDetails[i].RoadWidth == null)
                {
                    linesParas[num][4] = new SqlParameter()
                    {
                        ParameterName = "@RoadWidth",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    linesParas[num][4] = new SqlParameter()
                    {
                        ParameterName = "@RoadWidth",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].RoadWidth
                    };
                }
                linesParas[num][5] = new SqlParameter()
                {
                    ParameterName = "@IsCheckTime",
                    SqlDbType = SqlDbType.Bit,
                    Value = false
                };
                if (model.MapLinesDetails[i].MaxSecond == null)
                {
                    linesParas[num][6] = new SqlParameter()
                    {
                        ParameterName = "@MaxSecond",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    linesParas[num][6] = new SqlParameter()
                    {
                        ParameterName = "@MaxSecond",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MapLinesDetails[i].MaxSecond
                    };
                }
                if (model.MapLinesDetails[i].MinSecond == null)
                {
                    linesParas[num][7] = new SqlParameter()
                    {
                        ParameterName = "@MinSecond",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    linesParas[num][7] = new SqlParameter()
                    {
                        ParameterName = "@MinSecond",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].MinSecond
                    };
                }
                linesParas[num][8] = new SqlParameter()
                {
                    ParameterName = "@IsCheckSpeed",
                    SqlDbType = SqlDbType.Bit,
                    Value = false
                };
                if (model.MapLinesDetails[i].SpeedLimit == null)
                {
                    linesParas[num][9] = new SqlParameter()
                    {
                        ParameterName = "@SpeedLimit",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    linesParas[num][9] = new SqlParameter()
                    {
                        ParameterName = "@SpeedLimit",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].SpeedLimit
                    };
                }
                if (model.MapLinesDetails[i].OverSpeedDuration == null)
                {
                    linesParas[num][10] = new SqlParameter()
                    {
                        ParameterName = "@OverSpeedDuration",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                }
                else
                {
                    linesParas[num][10] = new SqlParameter()
                    {
                        ParameterName = "@OverSpeedDuration",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MapLinesDetails[i].OverSpeedDuration
                    };
                }
                //判断行驶时间选中，限速、最高速度、超速持续时间控件不可用，数据未传递过来
                if (model.MapLinesDetails[i].IsCheckTime == true)
                {
                    linesParas[num][5] = new SqlParameter()
                    {
                        ParameterName = "@IsCheckTime",
                        SqlDbType = SqlDbType.Bit,
                        Value = true
                    };
                    linesParas[num][6] = new SqlParameter()
                    {
                        ParameterName = "@MaxSecond",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MapLinesDetails[i].MaxSecond
                    };
                    linesParas[num][7] = new SqlParameter()
                    {
                        ParameterName = "@MinSecond",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].MinSecond
                    };
                    linesParas[num][8] = new SqlParameter()
                    {
                        ParameterName = "@IsCheckSpeed",
                        SqlDbType = SqlDbType.Bit,
                        Value = false
                    };
                    linesParas[num][9] = new SqlParameter()
                    {
                        ParameterName = "@SpeedLimit",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                    linesParas[num][10] = new SqlParameter()
                    {
                        ParameterName = "@OverSpeedDuration",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                }
                //限速选中，判断行驶时间、路段行驶过长、路段行驶不足控件不可用，数据未传递过来
                if (model.MapLinesDetails[i].IsCheckSpeed == true)
                {
                    linesParas[num][5] = new SqlParameter()
                    {
                        ParameterName = "@IsCheckTime",
                        SqlDbType = SqlDbType.Bit,
                        Value = false
                    };
                    linesParas[num][6] = new SqlParameter()
                    {
                        ParameterName = "@MaxSecond",
                        SqlDbType = SqlDbType.Int,
                        Value = DBNull.Value
                    };
                    linesParas[num][7] = new SqlParameter()
                    {
                        ParameterName = "@MinSecond",
                        SqlDbType = SqlDbType.Float,
                        Value = DBNull.Value
                    };
                    linesParas[num][8] = new SqlParameter()
                    {
                        ParameterName = "@IsCheckSpeed",
                        SqlDbType = SqlDbType.Bit,
                        Value = true
                    };
                    linesParas[num][9] = new SqlParameter()
                    {
                        ParameterName = "@SpeedLimit",
                        SqlDbType = SqlDbType.Float,
                        Value = model.MapLinesDetails[i].SpeedLimit
                    };
                    linesParas[num][10] = new SqlParameter()
                    {
                        ParameterName = "@OverSpeedDuration",
                        SqlDbType = SqlDbType.Int,
                        Value = model.MapLinesDetails[i].OverSpeedDuration
                    };
                }
                num++;
                orderID++;
            }
            bool insertResult = MSSQLHelper.ExecuteTransaction(CommandType.Text, sqls, linesParas);
            if (updateResult > 0 && delResult != 0 && insertResult == true)
                result = 1;

            #endregion

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
        /// 编辑时 验证用户所属单位下是否有重复的路线名称
        /// </summary>
        /// <param name="linesName"></param>
        /// <param name="id"></param>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static bool CheckEditLinesNameExists(string linesName, int id, int strucID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@LinesName",SqlDbType.NVarChar,50),
                new SqlParameter("@ID",SqlDbType.BigInt),
                new SqlParameter("@StrucID",SqlDbType.Int),
            };
            paras[0].Value = linesName;
            paras[1].Value = id;
            paras[2].Value = strucID;

            string sql = "SELECT COUNT(0) FROM dbo.MapLinesList WHERE ID!=@ID AND LinesName=@LinesName AND StrucID=@StrucID AND Status=0";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        #endregion

        #region 删除
        public static OperationResult DeleteMapLines(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"UPDATE dbo.MapLinesList SET Status=9 WHERE ID=@ID";
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
        /// 根据单位ID获取单位及子单位的路线信息
        /// </summary>
        public static List<StrucMapLinesModel> GetStrucMapLines(int strucID, string lineName)
        {
            string sql = "SELECT * FROM dbo.Func_GetLineInfoByStrucID(@strucID) WHERE LinesName LIKE @lineName";
            SqlParameter[] paras = new SqlParameter[2];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@strucID",
                Value = strucID,
                SqlDbType = SqlDbType.Int
            };
            paras[1] = new SqlParameter()
            {
                ParameterName = "@lineName",
                Value = "%" + lineName + "%",
            };
            return ConvertToList<StrucMapLinesModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion
    }
}

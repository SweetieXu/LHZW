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
    public class StructureBLL
    {


        #region 查询
        public static AsiatekPagedList<StructureListModel> GetPagedStructures(StructureSettingModel model, int pageSize)
        {

            if (model.IsCascaded)
            {
                return GetPagedStructuresWithLevel(model, pageSize);
            }

            return GetPagedStructuresWithoutLevel(model, pageSize);
        }



        /// <summary>
        /// 查询单位信息分页数据（包含层次LevelID）
        /// 用于当查询要求 级联 时使用
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static AsiatekPagedList<StructureListModel> GetPagedStructuresWithLevel(StructureSettingModel model, int pageSize)
        {

            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.Func_GetStructuresWithLevelID('"+model.StrucAccountOrName+"','"+model.StrucAccountOrName+"')"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",model.SearchPage),
                new SqlParameter("@orderBy","LevelID"),
                new SqlParameter("@showColumns",@"*"),
            };




            #region 筛选条件
            //string conditionStr = string.Empty;
            //if (!model.InspectTypeAny)//如果选择了具体的查岗通知类型
            //{

            //    conditionStr += "InspectType1='" + model.InspectType1 + "'  AND InspectType2='" + model.InspectType2 + "' AND InspectType3='" + model.InspectType3 + "'";
            //}

            //if (!model.ExNoticeTypeAny)//如果选择了具体的异常通知类型
            //{
            //    if (string.IsNullOrWhiteSpace(conditionStr))
            //    {
            //        conditionStr += "ExNoticeType1='" + model.ExNoticeType1 + "'  AND ExNoticeType3='" + model.ExNoticeType3 + "'";
            //    }
            //    else
            //    {
            //        conditionStr += " AND ExNoticeType1='" + model.ExNoticeType1 + "'  AND ExNoticeType3='" + model.ExNoticeType3 + "'";
            //    }
            //}

            // 注释上面的代码 一开始加一个1=1 然后后面就不需要每次判断一下查询条件是否为空再来加AND
            string conditionStr = "1=1";
            if (!model.InspectTypeAny)//如果选择了具体的查岗通知类型
            {

                conditionStr += " AND InspectType1='" + model.InspectType1 + "'  AND InspectType2='" + model.InspectType2 + "' AND InspectType3='" + model.InspectType3 + "'";
            }

            if (!model.ExNoticeTypeAny)//如果选择了具体的异常通知类型
            {
                conditionStr += " AND ExNoticeType1='" + model.ExNoticeType1 + "'  AND ExNoticeType3='" + model.ExNoticeType3 + "'";
            }
            if (model.Nightban != -1)
            {
                conditionStr += " AND ISNULL(IsNightBan,0) = " + model.Nightban;
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

            List<StructureListModel> list = ConvertToList<StructureListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }

        /// <summary>
        /// 查询单位信息分页数据（不包含层次LevelID）
        /// 用于当查询不要求 级联 时使用
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        public static AsiatekPagedList<StructureListModel> GetPagedStructuresWithoutLevel(StructureSettingModel model, int pageSize)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@tableName","dbo.Structures"),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@currentPage",model.SearchPage),
                new SqlParameter("@orderBy","ID"),
                new SqlParameter("@showColumns",@"ID ,
                                                                                StrucName ,
                                                                                StrucAccount ,
                                                                                ParentID,
                                                                                InspectType1 ,
                                                                                InspectType2 ,
                                                                                InspectType3 ,
                                                                                ExNoticeType1 ,
                                                                                ExNoticeType3,
                                                                                IsNightBan"),
            };
            #region 筛选条件
            string conditionStr = "(StrucAccount LIKE '%" + model.StrucAccountOrName + "%' OR StrucName LIKE '%" + model.StrucAccountOrName + "%') AND [Status]<>9";
            if (!model.InspectTypeAny)//如果选择了具体的查岗通知类型
            {

                conditionStr += " AND InspectType1='" + model.InspectType1 + "'  AND InspectType2='" + model.InspectType2 + "' AND InspectType3='" + model.InspectType3 + "'";
            }

            if (!model.ExNoticeTypeAny)//如果选择了具体的异常通知类型
            {
                conditionStr += " AND ExNoticeType1='" + model.ExNoticeType1 + "'  AND ExNoticeType3='" + model.ExNoticeType3 + "'";
            }

            if (model.Nightban != -1)
            {
                conditionStr += " AND ISNULL(IsNightBan,0) = " + model.Nightban;
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
            List<StructureListModel> list = ConvertToList<StructureListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        }
        /// <summary>
        /// 获取单位下拉列表信息
        /// 包含单位ID、单位名称
        /// </summary>
        public static List<StructureDDLModel> GetStructures()
        {
            string sql = "SELECT ID,StrucName FROM dbo.Structures WHERE Status<>9";
            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        /// <summary>
        /// 获取单位下拉列表信息
        /// 包含单位ID、单位名称
        /// </summary>
        public static List<StructureDDLModel> GetStructuresByStructureName(string strucName)
        {
            string sql = "SELECT ID,StrucName FROM dbo.Structures WHERE Status<>9 and StrucName LIKE @clientName";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@clientName",SqlDbType.NVarChar)
            };
            paras[0].Value = "%" + strucName + "%";
            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        /// <summary>
        /// 获取单位下拉列表信息--一条数据
        /// 包含单位ID、单位名称
        /// </summary>
        public static List<StructureDDLModel> GetStructuresOneData()
        {
            string sql = "SELECT TOP 1 ID,StrucName FROM dbo.Structures WHERE Status<>9";
            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        /// <summary>
        /// 根据输入条件匹配名称搜索
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<StructureDDLModel> GetStructuresByName(string name)
        {
            string sql = "SELECT ID,StrucName FROM dbo.Structures WHERE Status<>9 AND StrucName LIKE @name";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NVarChar)
            };
            paras[0].Value = "%" + name + "%";
            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

        /// <summary>
        /// 根据输入条件匹配名称搜索  查询当前单位及其子单位
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<StructureDDLModel> GetStrucAndSubStrucByName(string name, int currentStrucID)
        {
            string sql = string.Format(@"SELECT ID,StrucName FROM Func_GetStrucAndSubStrucByUserAffiliatedStrucID({0}) WHERE StrucName LIKE @name", currentStrucID);
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@name",SqlDbType.NVarChar)
            };
            paras[0].Value = "%" + name + "%";
            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }
        /// <summary>
        /// 根据单位ID查单位名称
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public static List<StructureDDLModel> GetStructuresByID(int id)
        {
            string sql = "SELECT ID,StrucName FROM dbo.Structures WHERE Status<>9 AND ID=@id";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@id",SqlDbType.Int)
            };
            paras[0].Value = id;
            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }



        /// <summary>
        /// 获取不包含当前单位的单位下拉列表信息
        /// <para>包含单位ID、单位名称</para>
        /// <para>用于单位编辑</para>
        /// </summary>
        public static List<StructureDDLModel> GetStructures(int currentStrucID)
        {
            string sql = "SELECT ID,StrucName FROM dbo.Structures WHERE Status<>9 AND ID<>@currentStrucID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@currentStrucID",SqlDbType.Int)
            };
            paras[0].Value = currentStrucID;
            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

        /// <summary>
        /// 获取单位树结构信息
        /// </summary>
        /// <returns></returns>
        public static List<StructureTreeModel> GetStructuresForTree()
        {
            string sql = "SELECT ID,StrucName,ParentID FROM dbo.Structures WHERE [Status]<>9";
            return ConvertToList<StructureTreeModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }


        /// <summary>
        /// 获取用户分配的单位ID
        /// </summary>
        public static List<int?> GetDistributiveStrucIDByUserID(int userID)
        {
            string sql = @"  SELECT    StrucID
  FROM      dbo.StructureDistributionInfo sdi
            INNER JOIN dbo.Structures s ON sdi.StrucID = s.ID
  WHERE     s.[Status] <> 9
            AND sdi.UserID = @userID";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@userID",SqlDbType.Int),
            };
            paras[0].Value = userID;
            var dt = MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray());
            if (dt == null)
            {
                return null;
            }
            List<int?> ids = new List<int?>();
            foreach (DataRow dr in dt.Rows)
            {
                ids.Add(Convert.ToInt32(dr[0]));
            }
            return ids;
        }


        /// <summary>
        /// 获取单位下拉列表信息
        /// 包含单位ID、单位名称
        /// </summary>
        public static List<StrucByUIDModel> GetStructuresByUserID(int uid)
        {
            string sql = @"select * from Func_GetStrucListByUserID('{0}')";
            sql = string.Format(sql, uid.ToString());
            return ConvertToList<StrucByUIDModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));

        }
        /// <summary>
        /// 根据用户所属单位与名称模糊匹配用户所属单位以及子单位信息
        /// </summary>
        public static List<StructureDDLModel> GetUsersStructuresByName(string name, int strucID)
        {
            string sql = @"WITH myCte AS
(
SELECT StrucName,ID FROM Structures WHERE ID = @StrucID AND Status <> 9
UNION ALL
SELECT Structures.StrucName,Structures.ID FROM  Structures 
    INNER JOIN myCte ON  Structures.ParentID = myCte.ID WHERE Structures.Status <> 9
)
SELECT * FROM myCte
WHERE myCte.StrucName LIKE @Name";
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter{ParameterName="@Name",SqlDbType=SqlDbType.NVarChar,Value="%" + name + "%"},
                new SqlParameter{ParameterName="@StrucID",SqlDbType=SqlDbType.Int,Value=strucID},
            };
            return ConvertToList<StructureDDLModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));
        }

        //public static AsiatekPagedList<StructureListModel> GetPagedStructures(StructureSettingModel model, int pageSize, int currentPage = 1)
        //{

        //    if (model.IsCascaded)
        //    {
        //        return GetPagedStructuresWithLevel(model, pageSize, currentPage);
        //    }

        //    return GetPagedStructuresWithoutLevel(model, pageSize, currentPage);
        //}

        //        /// <summary>
        //        /// 查询单位信息分页数据（包含层次LevelID）
        //        /// 用于当查询要求 级联 时使用
        //        /// </summary>
        //        /// <param name="model"></param>
        //        /// <param name="pageSize"></param>
        //        /// <param name="currentPage"></param>
        //        /// <returns></returns>
        //        public static AsiatekPagedList<StructureListModel> GetPagedStructuresWithLevel(StructureSettingModel model, int pageSize, int currentPage = 1)
        //        {

        //            List<SqlParameter> paras = new List<SqlParameter>()
        //            {
        //                new SqlParameter("@tableName","dbo.Func_GetStructuresWithLevelID('"+model.StructureCodeOrName+"','"+model.StructureCodeOrName+"')"),
        //                new SqlParameter("@pageSize",pageSize),
        //                new SqlParameter("@currentPage",currentPage),
        //                new SqlParameter("@orderBy","LevelID"),
        //                new SqlParameter("@showColumns",@"*"),
        //            };




        //            #region 筛选条件
        //            string conditionStr = string.Empty;
        //            if (!model.InspectTypeAny)//如果选择了具体的查岗通知类型
        //            {

        //                conditionStr += "InspectType1='" + model.InspectType1 + "'  AND InspectType2='" + model.InspectType2 + "' AND InspectType3='" + model.InspectType3 + "'";
        //            }

        //            if (!model.ExNoticeTypeAny)//如果选择了具体的异常通知类型
        //            {
        //                if (string.IsNullOrWhiteSpace(conditionStr))
        //                {
        //                    conditionStr += "ExNoticeType1='" + model.ExNoticeType1 + "'  AND ExNoticeType3='" + model.ExNoticeType3 + "'";
        //                }
        //                else
        //                {
        //                    conditionStr += " AND ExNoticeType1='" + model.ExNoticeType1 + "'  AND ExNoticeType3='" + model.ExNoticeType3 + "'";
        //                }

        //            }
        //            if (!string.IsNullOrWhiteSpace(conditionStr))
        //            {
        //                paras.Add(new SqlParameter("@conditionStr", conditionStr));
        //            }

        //            #endregion




        //            paras.Add(new SqlParameter()
        //            {
        //                ParameterName = "@totalItemCount",
        //                Direction = ParameterDirection.Output,
        //                SqlDbType = SqlDbType.Int
        //            });
        //            paras.Add(new SqlParameter()
        //            {
        //                ParameterName = "@newCurrentPage",
        //                Direction = ParameterDirection.Output,
        //                SqlDbType = SqlDbType.Int
        //            });

        //            List<StructureListModel> list = ConvertToList<StructureListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
        //            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
        //            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
        //            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        //        }

        //        /// <summary>
        //        /// 查询单位信息分页数据（不包含层次LevelID）
        //        /// 用于当查询不要求 级联 时使用
        //        /// </summary>
        //        /// <param name="model"></param>
        //        /// <param name="pageSize"></param>
        //        /// <param name="currentPage"></param>
        //        /// <returns></returns>
        //        public static AsiatekPagedList<StructureListModel> GetPagedStructuresWithoutLevel(StructureSettingModel model, int pageSize, int currentPage = 1)
        //        {
        //            List<SqlParameter> paras = new List<SqlParameter>()
        //            {
        //                new SqlParameter("@tableName","dbo.Structures"),
        //                new SqlParameter("@pageSize",pageSize),
        //                new SqlParameter("@currentPage",currentPage),
        //                new SqlParameter("@orderBy","ID"),
        //                new SqlParameter("@showColumns",@"ID ,
        //                                                                                StrucName ,
        //                                                                                StrucCode ,
        //                                                                                ParentStrucCode ,
        //                                                                                InspectType1 ,
        //                                                                                InspectType2 ,
        //                                                                                InspectType3 ,
        //                                                                                ExNoticeType1 ,
        //                                                                                ExNoticeType3"),
        //            };




        //            #region 筛选条件
        //            string conditionStr = "(StrucCode LIKE '%" + model.StructureCodeOrName + "%' OR StrucName LIKE '%" + model.StructureCodeOrName + "%') AND StrucCode<>'0000' AND [Status]<>9";
        //            if (!model.InspectTypeAny)//如果选择了具体的查岗通知类型
        //            {

        //                conditionStr += " AND InspectType1='" + model.InspectType1 + "'  AND InspectType2='" + model.InspectType2 + "' AND InspectType3='" + model.InspectType3 + "'";
        //            }

        //            if (!model.ExNoticeTypeAny)//如果选择了具体的异常通知类型
        //            {


        //                conditionStr += " AND ExNoticeType1='" + model.ExNoticeType1 + "'  AND ExNoticeType3='" + model.ExNoticeType3 + "'";

        //            }


        //            if (!string.IsNullOrWhiteSpace(conditionStr))
        //            {
        //                paras.Add(new SqlParameter("@conditionStr", conditionStr));
        //            }

        //            #endregion





        //            paras.Add(new SqlParameter()
        //            {
        //                ParameterName = "@totalItemCount",
        //                Direction = ParameterDirection.Output,
        //                SqlDbType = SqlDbType.Int
        //            });
        //            paras.Add(new SqlParameter()
        //            {
        //                ParameterName = "@newCurrentPage",
        //                Direction = ParameterDirection.Output,
        //                SqlDbType = SqlDbType.Int
        //            });





        //            List<StructureListModel> list = ConvertToList<StructureListModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.StoredProcedure, "Proc_GetPagedDatas", paras.ToArray()));
        //            int totalItemCount = Convert.ToInt32(paras[paras.Count - 2].Value);
        //            int newCurrentPage = Convert.ToInt32(paras[paras.Count - 1].Value);
        //            return list.ToPagedList(newCurrentPage, pageSize, totalItemCount);
        //        }


        #endregion

        #region 删除
        /// <summary>
        /// 根据单位编号批量删除（逻辑删除）
        /// 将Status设置为9表示删除
        /// 更新当前单位以及所有子单位的状态
        /// 更新当前单位的用户、车辆以及所有子单位的用户、车辆状态
        /// 注意，删除单位并不删除车辆
        /// </summary>
        public static OperationResult DeleteStrus(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @"WITH CTE1
AS
(
	SELECT ID,StrucCode,ParentID FROM dbo.Structures WHERE ID=@ID
	UNION ALL
	SELECT  S.ID,s.StrucCode,s.ParentID  FROM dbo.Structures s 
	INNER JOIN CTE1 ON s.ParentID=CTE1.ID
)
UPDATE dbo.Structures SET [Status]=9 FROM CTE1 WHERE dbo.Structures.ID=CTE1.ID;
WITH CTE2
AS
(
	SELECT ID,StrucCode,ParentID FROM dbo.Structures WHERE ID=@ID
	UNION ALL
	SELECT  S.ID,s.StrucCode,s.ParentID  FROM dbo.Structures s 
	INNER JOIN CTE2 ON s.ParentID=CTE2.ID
)
UPDATE dbo.Users SET [Status]=9 FROM CTE2 WHERE dbo.Users.StrucID=CTE2.ID;
WITH CTE3
AS
(
	SELECT ID,StrucCode,ParentID FROM dbo.Structures WHERE ID=@ID
	UNION ALL
	SELECT  S.ID,s.StrucCode,s.ParentID  FROM dbo.Structures s 
	INNER JOIN CTE3 ON s.ParentID=CTE3.ID
)
UPDATE dbo.Vehicles SET [Status]=9 FROM CTE3 WHERE dbo.Vehicles.StrucID=CTE3.ID";
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
        /// 根据单位编号批量删除（物理删除）
        /// </summary>
        public static OperationResult DeleteStrusPhysical(string[] ids)
        {
            string[] sqls = new string[ids.Length];
            SqlParameter[][] paras = new SqlParameter[ids.Length][];
            for (int i = 0; i < ids.Length; i++)
            {
                sqls[i] = @" DELETE FROM dbo.Structures WHERE  ID= @ID";
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


        #region 新增


        /// <summary>
        /// 检查单位名称是否存在
        /// </summary>
        /// <param name="strucName"></param>
        /// <returns></returns>
        public static bool CheckStrucNameExists(string strucName)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@strucName",SqlDbType.NVarChar,200),
            };
            paras[0].Value = strucName.Trim();
            string sql = "SELECT COUNT(0) FROM Structures WHERE strucName=@strucName";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        public static bool CheckStrucAccountExists(string strucAccount)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@strucAccount",SqlDbType.NVarChar,20),
            };
            paras[0].Value = strucAccount.Trim();
            string sql = "SELECT COUNT(0) FROM Structures WHERE strucAccount=@strucAccount";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        /// <summary>
        /// 检查业户经营许可证号是否存在
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        public static bool CheckStrucLicenseNoExists(string licenseNo)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@licenseNo",SqlDbType.NVarChar),
            };
            paras[0].Value = licenseNo.Trim();
            string sql = "SELECT COUNT(0) FROM Structures WHERE licenseNo=@licenseNo";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        /// <summary>
        /// 检查添加时自定义编码是否存在
        /// </summary>
        /// <param name="customEncoding"></param>
        /// <returns></returns>
        public static bool CheckAddCustomEncodingExists(string customEncoding)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@customEncoding",SqlDbType.NVarChar),
            };
            paras[0].Value = customEncoding.Trim();
            string sql = "SELECT COUNT(0) FROM Structures WHERE CustomEncoding=@customEncoding";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }


        /// <summary>
        /// 新增功能信息
        /// </summary>
        public static OperationResult AddStructure(StructureAddSubModel model)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@StrucAccount",SqlDbType.NVarChar),
                new SqlParameter("@StrucName",SqlDbType.NVarChar),
                new SqlParameter("@ParentID",SqlDbType.Int),
                new SqlParameter("@InspectType1",SqlDbType.Bit),
                new SqlParameter("@InspectType2",SqlDbType.Bit),
                new SqlParameter("@InspectType3",SqlDbType.Bit),
                new SqlParameter("@ExNoticeType1",SqlDbType.Bit),
                new SqlParameter("@ExNoticeType3",SqlDbType.Bit),
                new SqlParameter("@MapType1",SqlDbType.Bit),
                new SqlParameter("@MapType2",SqlDbType.Bit),
                new SqlParameter("@MapType3",SqlDbType.Bit),
            };


            #region 非NULL 截止索引10
            paras[0].Value = model.StrucAccount;
            paras[1].Value = model.StrucName;
            if (model.ParentID == -1)
            {
                paras[2].Value = DBNull.Value;
            }
            else
            {
                paras[2].Value = model.ParentID;
            }


            paras[3].Value = model.InspectType1;
            paras[4].Value = model.InspectType2;
            paras[5].Value = model.InspectType3;

            paras[6].Value = model.ExNoticeType1;
            paras[7].Value = model.ExNoticeType3;

            paras[8].Value = model.MapType1;
            paras[9].Value = model.MapType2;
            paras[10].Value = model.MapType3;
            #endregion



            #region 可NULL
            paras.Add(new SqlParameter("@InspectMobiles", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.InspectMobiles))
            {
                paras[11].Value = DBNull.Value;
            }
            else
            {
                paras[11].Value = model.InspectMobiles;
            }

            paras.Add(new SqlParameter("@ExMobiles", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.ExMobiles))
            {
                paras[12].Value = DBNull.Value;
            }
            else
            {
                paras[12].Value = model.ExMobiles;
            }

            paras.Add(new SqlParameter("@LinkMan1", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMan1))
            {
                paras[13].Value = DBNull.Value;
            }
            else
            {
                paras[13].Value = model.LinkMan1;
            }

            paras.Add(new SqlParameter("@LinkMobile1", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMobile1))
            {
                paras[14].Value = DBNull.Value;
            }
            else
            {
                paras[14].Value = model.LinkMobile1;
            }

            paras.Add(new SqlParameter("@LinkMan2", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMan2))
            {
                paras[15].Value = DBNull.Value;
            }
            else
            {
                paras[15].Value = model.LinkMan2;
            }

            paras.Add(new SqlParameter("@LinkMobile2", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMobile2))
            {
                paras[16].Value = DBNull.Value;
            }
            else
            {
                paras[16].Value = model.LinkMobile2;
            }

            paras.Add(new SqlParameter("@LinkMan3", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMan3))
            {
                paras[17].Value = DBNull.Value;
            }
            else
            {
                paras[17].Value = model.LinkMan3;
            }

            paras.Add(new SqlParameter("@LinkMobile3", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMobile3))
            {
                paras[18].Value = DBNull.Value;
            }
            else
            {
                paras[18].Value = model.LinkMobile3;
            }









            paras.Add(new SqlParameter("@Logo", SqlDbType.Image));
            if (model.Logo == null)
            {
                paras[19].Value = DBNull.Value;
            }
            else
            {
                paras[19].Value = model.Logo;
            }


            paras.Add(new SqlParameter("@Remark", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[20].Value = DBNull.Value;
            }
            else
            {
                paras[20].Value = model.Remark;
            }

            //paras.Add(new SqlParameter("@LicenseNo", SqlDbType.BigInt));
            //if (string.IsNullOrWhiteSpace(model.LicenseNo))
            //{
            //    paras[21].Value = DBNull.Value;
            //}
            //else
            //{
            //    paras[21].Value = Convert.ToInt64(model.LicenseNo);
            //}
            paras.Add(new SqlParameter("@LicenseNo", SqlDbType.NVarChar));
            if (model.LicenseNo == null)
            {
                paras[21].Value = DBNull.Value;
            }
            else
            {
                paras[21].Value = model.LicenseNo;
            }
            paras.Add(new SqlParameter("@CustomEncoding", SqlDbType.NVarChar));
            if (model.CustomEncoding == null)
            {
                paras[22].Value = DBNull.Value;
            }
            else
            {
                paras[22].Value = model.CustomEncoding;
            }
            #endregion


            #region SQL
            string sql = @"INSERT INTO dbo.Structures
        ( StrucCode ,
          StrucName ,
          StrucAccount ,
          ParentID ,
          CreateTime ,
          InspectMobiles ,
          InspectType1 ,
          InspectType2 ,
          InspectType3 ,
          InspectType4 ,
          ExMobiles ,
          ExNoticeType1 ,
          ExNoticeType2 ,
          ExNoticeType3 ,
          ExNoticeType4 ,
          MapType1 ,
          MapType2 ,
          MapType3 ,
          MapType4 ,
          MapType5 ,
          LinkMan1 ,
          LinkMobile1 ,
          LinkMan2 ,
          LinkMobile2 ,
          LinkMan3 ,
          LinkMobile3 ,
          Logo ,
          Remark ,
          Status ,
          EditTime,
          LicenseNo,
CustomEncoding
        )
VALUES  ( @StrucAccount , 
          @StrucName , -- StrucName - nvarchar(50)
          @StrucAccount, 
          @ParentID,
          GETDATE() , -- CreateTime - datetime
          @InspectMobiles , -- InspectMobiles - nvarchar(50)
          @InspectType1 , -- InspectType1 - bit
          @InspectType2 , -- InspectType2 - bit
          @InspectType3 , -- InspectType3 - bit
          0 , -- InspectType4 - bit
          @ExMobiles , -- ExMobiles - nvarchar(50)
          @ExNoticeType1 , -- ExNoticeType1 - bit
          0 , -- ExNoticeType2 - bit
          @ExNoticeType3 , -- ExNoticeType3 - bit
          0 , -- ExNoticeType4 - bit
          @MapType1 , -- MapType1 - bit
          @MapType2 , -- MapType2 - bit
          @MapType3 , -- MapType3 - bit
          0 , -- MapType4 - bit
          0 , -- MapType5 - bit
          @LinkMan1, -- LinkMan1 - nvarchar(50)
          @LinkMobile1, -- LinkMobile1 - nvarchar(50)
          @LinkMan2, -- LinkMan2 - nvarchar(50)
          @LinkMobile2 , -- LinkMobile2 - nvarchar(50)
          @LinkMan3 , -- LinkMan3 - nvarchar(50)
          @LinkMobile3 , -- LinkMobile3 - nvarchar(50)
          @Logo , -- Logo - image
          @Remark , -- Remark - nvarchar(200)
          '0' , -- Status - nchar(1)
          GETDATE(),  -- EditTime - datetime
          @LicenseNo,
@CustomEncoding
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
        /// 新增功能信息
        /// </summary>
        public static OperationResult AddStructure(StructureAddModel model, int CreateUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@StrucAccount",SqlDbType.NVarChar),
                new SqlParameter("@StrucName",SqlDbType.NVarChar),
                new SqlParameter("@ParentID",SqlDbType.Int),
                new SqlParameter("@InspectType1",SqlDbType.Bit),
                new SqlParameter("@InspectType2",SqlDbType.Bit),
                new SqlParameter("@InspectType3",SqlDbType.Bit),
                new SqlParameter("@ExNoticeType1",SqlDbType.Bit),
                new SqlParameter("@ExNoticeType3",SqlDbType.Bit),
                new SqlParameter("@MapType1",SqlDbType.Bit),
                new SqlParameter("@MapType2",SqlDbType.Bit),
                new SqlParameter("@MapType3",SqlDbType.Bit),
            };

            #region 非NULL 截止索引10
            paras[0].Value = model.StrucAccount.Trim();
            paras[1].Value = model.StrucName.Trim();
            paras[2].Value = model.ParentID;



            paras[3].Value = model.InspectType1;
            paras[4].Value = model.InspectType2;
            paras[5].Value = model.InspectType3;

            paras[6].Value = model.ExNoticeType1;
            paras[7].Value = model.ExNoticeType3;

            paras[8].Value = model.MapType1;
            paras[9].Value = model.MapType2;
            paras[10].Value = model.MapType3;
            #endregion



            #region 可NULL
            paras.Add(new SqlParameter("@InspectMobiles", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.InspectMobiles))
            {
                paras[11].Value = DBNull.Value;
            }
            else
            {
                paras[11].Value = model.InspectMobiles.Trim();
            }

            paras.Add(new SqlParameter("@ExMobiles", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.ExMobiles))
            {
                paras[12].Value = DBNull.Value;
            }
            else
            {
                paras[12].Value = model.ExMobiles.Trim();
            }

            paras.Add(new SqlParameter("@LinkMan1", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMan1))
            {
                paras[13].Value = DBNull.Value;
            }
            else
            {
                paras[13].Value = model.LinkMan1.Trim();
            }

            paras.Add(new SqlParameter("@LinkMobile1", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMobile1))
            {
                paras[14].Value = DBNull.Value;
            }
            else
            {
                paras[14].Value = model.LinkMobile1.Trim();
            }

            paras.Add(new SqlParameter("@LinkMan2", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMan2))
            {
                paras[15].Value = DBNull.Value;
            }
            else
            {
                paras[15].Value = model.LinkMan2.Trim();
            }

            paras.Add(new SqlParameter("@LinkMobile2", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMobile2))
            {
                paras[16].Value = DBNull.Value;
            }
            else
            {
                paras[16].Value = model.LinkMobile2.Trim();
            }

            paras.Add(new SqlParameter("@LinkMan3", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMan3))
            {
                paras[17].Value = DBNull.Value;
            }
            else
            {
                paras[17].Value = model.LinkMan3.Trim();
            }

            paras.Add(new SqlParameter("@LinkMobile3", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMobile3))
            {
                paras[18].Value = DBNull.Value;
            }
            else
            {
                paras[18].Value = model.LinkMobile3.Trim();
            }


            paras.Add(new SqlParameter("@Logo", SqlDbType.Image));
            if (model.Logo == null)
            {
                paras[19].Value = DBNull.Value;
            }
            else
            {
                paras[19].Value = model.Logo;
            }


            paras.Add(new SqlParameter("@Remark", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[20].Value = DBNull.Value;
            }
            else
            {
                paras[20].Value = model.Remark;
            }

            paras.Add(new SqlParameter("@LicenseNo", SqlDbType.NVarChar));
            if (model.LicenseNo == null)
            {
                paras[21].Value = DBNull.Value;
            }
            else
            {
                paras[21].Value = model.LicenseNo.Trim();
            }
            #endregion

            paras.Add(new SqlParameter("@CreateUserID", SqlDbType.Int));
            paras[22].Value = CreateUserID;

            paras.Add(new SqlParameter("@CustomEncoding", SqlDbType.NVarChar));
            if (model.CustomEncoding == null)
            {
                paras[23].Value = DBNull.Value;
            }
            else
            {
                paras[23].Value = model.CustomEncoding.Trim();
            }
            #region SQL
            string sql = @"INSERT INTO dbo.Structures
        ( StrucCode ,
          StrucName ,
          StrucAccount ,
          ParentID ,
          CreateTime ,
          CreateUserID,
          InspectMobiles ,
          InspectType1 ,
          InspectType2 ,
          InspectType3 ,
          InspectType4 ,
          ExMobiles ,
          ExNoticeType1 ,
          ExNoticeType2 ,
          ExNoticeType3 ,
          ExNoticeType4 ,
          MapType1 ,
          MapType2 ,
          MapType3 ,
          MapType4 ,
          MapType5 ,
          LinkMan1 ,
          LinkMobile1 ,
          LinkMan2 ,
          LinkMobile2 ,
          LinkMan3 ,
          LinkMobile3 ,
          Logo ,
          Remark ,
          Status ,
          EditTime,
          LicenseNo,
CustomEncoding
        )
VALUES  ( @StrucAccount ,
          @StrucName , -- StrucName - nvarchar(50)
          @StrucAccount, 
          @ParentID,
          GETDATE() , -- CreateTime - datetime
          @CreateUserID,
          @InspectMobiles , -- InspectMobiles - nvarchar(50)
          @InspectType1 , -- InspectType1 - bit
          @InspectType2 , -- InspectType2 - bit
          @InspectType3 , -- InspectType3 - bit
          0 , -- InspectType4 - bit
          @ExMobiles , -- ExMobiles - nvarchar(50)
          @ExNoticeType1 , -- ExNoticeType1 - bit
          0 , -- ExNoticeType2 - bit
          @ExNoticeType3 , -- ExNoticeType3 - bit
          0 , -- ExNoticeType4 - bit
          @MapType1 , -- MapType1 - bit
          @MapType2 , -- MapType2 - bit
          @MapType3 , -- MapType3 - bit
          0 , -- MapType4 - bit
          0 , -- MapType5 - bit
          @LinkMan1, -- LinkMan1 - nvarchar(50)
          @LinkMobile1, -- LinkMobile1 - nvarchar(50)
          @LinkMan2, -- LinkMan2 - nvarchar(50)
          @LinkMobile2 , -- LinkMobile2 - nvarchar(50)
          @LinkMan3 , -- LinkMan3 - nvarchar(50)
          @LinkMobile3 , -- LinkMobile3 - nvarchar(50)
          @Logo , -- Logo - image
          @Remark , -- Remark - nvarchar(200)
          '0' , -- Status - nchar(1)
          GETDATE(),  -- EditTime - datetime
          @LicenseNo,
@CustomEncoding
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


        #region 编辑
        public static bool CheckStrucNameExists(string strucName, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@strucName",SqlDbType.NVarChar,200),
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = strucName.Trim();
            paras[1].Value = id;
            string sql = "SELECT COUNT(0) FROM Structures WHERE strucName=@strucName AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }
        /// <summary>
        /// 检查业户经营许可证号是否存在
        /// </summary>
        /// <param name="licenseNo"></param>
        /// <returns></returns>
        public static bool CheckStrucLicenseNoExists(string licenseNo, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@licenseNo",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int)
            };
            paras[0].Value = licenseNo.Trim();
            paras[1].Value = id;
            string sql = "SELECT COUNT(0) FROM Structures WHERE licenseNo=@licenseNo AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }



        public static bool CheckEditCustomEncodingExists(string customEncoding, int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@customEncoding",SqlDbType.NVarChar),
                new SqlParameter("@ID",SqlDbType.Int)
            };
            paras[0].Value = customEncoding.Trim();
            paras[1].Value = id;
            string sql = "SELECT COUNT(0) FROM Structures WHERE CustomEncoding=@customEncoding AND ID<>@ID";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)//如果发生错误，防止添加进非法数据，所以发生错误当作存在
            {
                return true;
            }
            return Convert.ToInt32(result) > 0;
        }



        /// <summary>
        /// 根据
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public static SelectResult<StructureEditModel> GetStructureByID(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT  ID ,
        StrucAccount ,
        StrucName ,
        ParentID ,
        InspectMobiles ,
        InspectType1 ,
        InspectType2 ,
        InspectType3,
       ExMobiles,
       ExNoticeType1,
       ExNoticeType3,
       MapType1,
       MapType2,
       MapType3,
       LinkMan1,
       LinkMobile1,
       LinkMan2,
       LinkMobile2,
       LinkMan3,
       LinkMobile3,
       CASE WHEN
       Logo IS NULL THEN 0
       ELSE 1 END
       AS HasLogo,
       Remark,
       LicenseNo,
CustomEncoding,
       (SELECT StrucName FROM dbo.Structures WHERE ID = a.ParentID ) AS ParentStructureName 
FROM    dbo.Structures AS a WHERE ID=@ID AND [Status]<>9";
            List<StructureEditModel> list = ConvertToList<StructureEditModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras.ToArray()));

            StructureEditModel data = null;
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
            return new SelectResult<StructureEditModel>()
            {
                DataResult = data,
                Message = msg
            };
        }

        /// <summary>
        /// 编辑功能信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static OperationResult EditStructure(StructureEditModel model, int EditUserID)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@StrucName",SqlDbType.NVarChar),
                new SqlParameter("@ParentID",SqlDbType.Int),
                new SqlParameter("@InspectType1",SqlDbType.Bit),
                new SqlParameter("@InspectType2",SqlDbType.Bit),
                new SqlParameter("@InspectType3",SqlDbType.Bit),
                new SqlParameter("@ExNoticeType1",SqlDbType.Bit),
                new SqlParameter("@ExNoticeType3",SqlDbType.Bit),
                new SqlParameter("@MapType1",SqlDbType.Bit),
                new SqlParameter("@MapType2",SqlDbType.Bit),
                new SqlParameter("@MapType3",SqlDbType.Bit),
            };


            #region 非NULL 截止索引10
            paras[0].Value = model.ID;
            paras[1].Value = model.StrucName.Trim();
            paras[2].Value = model.ParentID;



            paras[3].Value = model.InspectType1;
            paras[4].Value = model.InspectType2;
            paras[5].Value = model.InspectType3;

            paras[6].Value = model.ExNoticeType1;
            paras[7].Value = model.ExNoticeType3;

            paras[8].Value = model.MapType1;
            paras[9].Value = model.MapType2;
            paras[10].Value = model.MapType3;
            #endregion



            #region 可NULL
            paras.Add(new SqlParameter("@InspectMobiles", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.InspectMobiles))
            {
                paras[11].Value = DBNull.Value;
            }
            else
            {
                paras[11].Value = model.InspectMobiles.Trim();
            }

            paras.Add(new SqlParameter("@ExMobiles", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.ExMobiles))
            {
                paras[12].Value = DBNull.Value;
            }
            else
            {
                paras[12].Value = model.ExMobiles.Trim();
            }

            paras.Add(new SqlParameter("@LinkMan1", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMan1))
            {
                paras[13].Value = DBNull.Value;
            }
            else
            {
                paras[13].Value = model.LinkMan1.Trim();
            }

            paras.Add(new SqlParameter("@LinkMobile1", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMobile1))
            {
                paras[14].Value = DBNull.Value;
            }
            else
            {
                paras[14].Value = model.LinkMobile1.Trim();
            }

            paras.Add(new SqlParameter("@LinkMan2", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMan2))
            {
                paras[15].Value = DBNull.Value;
            }
            else
            {
                paras[15].Value = model.LinkMan2.Trim();
            }

            paras.Add(new SqlParameter("@LinkMobile2", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMobile2))
            {
                paras[16].Value = DBNull.Value;
            }
            else
            {
                paras[16].Value = model.LinkMobile2.Trim();
            }

            paras.Add(new SqlParameter("@LinkMan3", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMan3))
            {
                paras[17].Value = DBNull.Value;
            }
            else
            {
                paras[17].Value = model.LinkMan3.Trim();
            }

            paras.Add(new SqlParameter("@LinkMobile3", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.LinkMobile3))
            {
                paras[18].Value = DBNull.Value;
            }
            else
            {
                paras[18].Value = model.LinkMobile3.Trim();
            }

            paras.Add(new SqlParameter("@Remark", SqlDbType.NVarChar));
            if (string.IsNullOrWhiteSpace(model.Remark))
            {
                paras[19].Value = DBNull.Value;
            }
            else
            {
                paras[19].Value = model.Remark;
            }

            paras.Add(new SqlParameter("@LicenseNo", SqlDbType.NVarChar));
            if (model.LicenseNo == null)
            {
                paras[20].Value = DBNull.Value;
            }
            else
            {
                paras[20].Value = model.LicenseNo.Trim();
            }

            paras.Add(new SqlParameter("@EditUserID", SqlDbType.Int));
            paras[21].Value = EditUserID;
            if (model.ModifyLogo)
            {
                paras.Add(new SqlParameter("@Logo", SqlDbType.Image));
                if (model.Logo == null)
                {
                    paras[22].Value = DBNull.Value;
                }
                else
                {
                    paras[22].Value = model.Logo;
                }
            }

            paras.Add(new SqlParameter("@CustomEncoding", SqlDbType.NVarChar));
            if (model.CustomEncoding == null)
            {
                paras[22].Value = DBNull.Value;
            }
            else
            {
                paras[22].Value = model.CustomEncoding.Trim();
            }

            #endregion


            #region SQL
            string sql = string.Empty;
            if (model.ModifyLogo)
            {
                sql = @"UPDATE  dbo.Structures
SET     StrucName = @StrucName ,
        ParentID = @ParentID ,
        InspectMobiles = @InspectMobiles ,
        InspectType1 = @InspectType1 ,
        InspectType2 = @InspectType2 ,
        InspectType3 = @InspectType3 ,
        ExMobiles = @ExMobiles ,
        ExNoticeType1 = @ExNoticeType1 ,
        ExNoticeType3 = @ExNoticeType3 ,
        MapType1 = @MapType1 ,
        MapType2 = @MapType2 ,
        MapType3 = @MapType3 ,
        LinkMan1 = @LinkMan1 ,
        LinkMan2 = @LinkMan2 ,
        LinkMan3 = @LinkMan3 ,
        LinkMobile1 = @LinkMobile1 ,
        LinkMobile2 = @LinkMobile2 ,
        LinkMobile3 = @LinkMobile3 ,
        Logo = @Logo ,
        Remark = @Remark,
        EditTime = GETDATE(),
        LicenseNo=@LicenseNo,
        EditUserID=@EditUserID,
CustomEncoding =@CustomEncoding
WHERE   ID = @ID";
            }
            else
            {
                sql = @"UPDATE  dbo.Structures
SET     StrucName = @StrucName ,
        ParentID = @ParentID ,
        InspectMobiles = @InspectMobiles ,
        InspectType1 = @InspectType1 ,
        InspectType2 = @InspectType2 ,
        InspectType3 = @InspectType3 ,
        ExMobiles = @ExMobiles ,
        ExNoticeType1 = @ExNoticeType1 ,
        ExNoticeType3 = @ExNoticeType3 ,
        MapType1 = @MapType1 ,
        MapType2 = @MapType2 ,
        MapType3 = @MapType3 ,
        LinkMan1 = @LinkMan1 ,
        LinkMan2 = @LinkMan2 ,
        LinkMan3 = @LinkMan3 ,
        LinkMobile1 = @LinkMobile1 ,
        LinkMobile2 = @LinkMobile2 ,
        LinkMobile3 = @LinkMobile3 ,
        Remark = @Remark,
        EditTime = GETDATE(),
        LicenseNo=@LicenseNo,
        EditUserID=@EditUserID,
CustomEncoding=@CustomEncoding
WHERE   ID = @ID";
            }

            #endregion

            bool result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, paras.ToArray()) > 0;
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.DBError
            };
        }

        /// <summary>
        /// 根据单位ID获取单位的LOGO
        /// </summary>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static byte[] GetLogBytes(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"SELECT  Logo  FROM  dbo.Structures WHERE ID=@ID";
            var logoObj = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (logoObj is byte[])
            {
                return (byte[])logoObj;
            }
            return null;
        }
        #endregion

        #region 导入车辆资料
        /// <summary>
        /// 根据单位名称获取单位ID
        /// </summary>
        public static bool TryGetStructureIDByName(string strucName, out int id)
        {
            id = -1;
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@strucName",SqlDbType.NVarChar,200),
            };
            paras[0].Value = strucName.Trim();
            string sql = "SELECT ID FROM Structures WHERE strucName=@strucName";
            var result = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (result == null)
            {
                return false;
            }
            id = Convert.ToInt32(result);
            return true;
        }
        #endregion


        #region 其他
        /// <summary>
        /// 根据单位ID获取当前单位或上级单位的LOGO
        /// </summary>
        public static byte[] GetCurrentOrParentLogoBytes(int id)
        {
            List<SqlParameter> paras = new List<SqlParameter>()
            {
                new SqlParameter("@ID",SqlDbType.Int),
            };
            paras[0].Value = id;
            string sql = @"WITH cte(Logo,ParentID,Level)
AS (
SELECT Logo,ParentID,1 AS [Level] FROM dbo.Structures s
WHERE ID=@ID
UNION ALL
SELECT s.Logo,s.ParentID,cte.Level+1 FROM dbo.Structures s
INNER JOIN cte ON s.ID = cte.ParentID
)
SELECT TOP 1 Logo FROM cte WHERE Logo IS NOT NULL
ORDER BY  Level";
            var logoObj = MSSQLHelper.ExecuteScalar(CommandType.Text, sql, paras.ToArray());
            if (logoObj is byte[])
            {
                return (byte[])logoObj;
            }
            return null;
        }
        #endregion

        #region 获取经营范围列表
        /// <summary>
        /// 获取经营范围列表
        /// </summary>
        /// <returns></returns>
        public static List<BusinessScopeModel> GetBusinessScopeList()
        {
            string sql = "SELECT [Code],[NAME] FROM  [BusinessScope]";
            return ConvertToList<BusinessScopeModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion

        #region 获取运输行业列表
        /// <summary>
        /// 获取运输行业列表
        /// </summary>
        /// <returns></returns>
        public static List<TransportIndustryModel> GetTransportIndustryList()
        {
            string sql = "SELECT [Code],[Name] FROM [dbo].[TransportIndustry]";
            return ConvertToList<TransportIndustryModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql));
        }
        #endregion

        #region 获取单位已经分配的经营范围
        /// <summary>
        /// 获取单位已经分配的经营范围
        /// </summary>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static List<BusinessScopeModel> GetBusinessScopeListByStrucID(int strucID)
        {
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                SqlDbType = SqlDbType.Int,
                Value = strucID
            };
            string sql = @" SELECT a.BusinessScopeCode AS Code,b.NAME FROM StructureBussinessScope AS a
                                      INNER JOIN BusinessScope AS b ON a.BusinessScopeCode = b.Code WHERE a.StrucID = @StrucID";
            return ConvertToList<BusinessScopeModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 获取单位已经分配的运输行业
        /// <summary>
        /// 获取单位已经分配的运输行业
        /// </summary>
        /// <param name="strucID"></param>
        /// <returns></returns>
        public static List<TransportIndustryModel> GetTransportIndustryListByStrucID(int strucID)
        {
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter()
            {
                ParameterName = "@StrucID",
                SqlDbType = SqlDbType.Int,
                Value = strucID
            };
            string sql = @" SELECT a.TransportIndustryCode AS Code,b.Name FROM StructureTransportIndustry AS a
                                      INNER JOIN TransportIndustry AS b ON a.TransportIndustryCode = b.Code WHERE a.StrucID = @StrucID";
            return ConvertToList<TransportIndustryModel>.Convert(MSSQLHelper.ExecuteDataTable(CommandType.Text, sql, paras));
        }
        #endregion

        #region 分配经营范围
        /// <summary>
        /// 分配经营范围
        /// </summary>
        /// <param name="strucIDList">单位ID结合</param>
        /// <param name="codeList">经营范围Code集合</param>
        /// <returns></returns>
        public static OperationResult DistributionBusinessScope(string strucIDList, string codeList)
        {
            // 单位分配经营范围的时候 首先先删除该单位原来的经营范围 然后添加
            // 此处可以不需要判断该单位是否已经分配的经营范围 因为即使没有分配 delete语句也不会造成异常
            string deleteSql = string.Format("DELETE FROM StructureBussinessScope WHERE StrucID IN ({0});", strucIDList.TrimEnd(','));
            bool result = false;
            if (string.IsNullOrWhiteSpace(codeList))
            {
                result = MSSQLHelper.ExecuteTransaction(CommandType.Text, deleteSql, null);
            }
            else
            {
                string[] strucIDArray = strucIDList.TrimEnd(',').Split(',');
                string[] codeArray = codeList.TrimEnd(',').Split(',');
                string sql = "INSERT INTO  [StructureBussinessScope]([StrucID],[BusinessScopeCode])VALUES";
                foreach (var item in strucIDArray)
                {
                    foreach (var code in codeArray)
                    {
                        sql += string.Format("({0},'{1}'),", item, code);
                    }
                }
                sql = sql.TrimEnd(',');
                result = MSSQLHelper.ExecuteTransaction(CommandType.Text, deleteSql + sql, null);
            }
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.OperationFailure
            };

        }
        #endregion

        #region 分配运输行业
        /// <summary>
        /// 分配运输行业
        /// </summary>
        /// <param name="strucIDList">单位ID结合</param>
        /// <param name="codeList">运输行业Code集合</param>
        /// <returns></returns>
        public static OperationResult DistributionTransportIndustry(string strucIDList, string codeList)
        {
            // 单位分配运输行业的时候 首先先删除该单位原来的运输行业 然后添加
            // 此处可以不需要判断该单位是否已经分配的运输行业 因为即使没有分配 delete语句也不会造成异常
            string deleteSql = string.Format("DELETE FROM StructureTransportIndustry WHERE StrucID IN ({0});", strucIDList.TrimEnd(','));
            bool result = false;
            if (string.IsNullOrWhiteSpace(codeList))
            {
                result = MSSQLHelper.ExecuteTransaction(CommandType.Text, deleteSql, null);
            }
            else
            {
                string[] strucIDArray = strucIDList.TrimEnd(',').Split(',');
                string[] codeArray = codeList.TrimEnd(',').Split(',');
                string sql = "INSERT INTO  [StructureTransportIndustry]([StrucID],[TransportIndustryCode])VALUES";
                foreach (var item in strucIDArray)
                {
                    foreach (var code in codeArray)
                    {
                        sql += string.Format("({0},'{1}'),", item, code);
                    }
                }
                sql = sql.TrimEnd(',');
                result = MSSQLHelper.ExecuteTransaction(CommandType.Text, deleteSql + sql, null);
            }
            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.OperationFailure
            };

        }
        #endregion

        #region 夜间禁行启用/关闭设置
        /// <summary>
        /// 夜间禁行启用/关闭设置
        /// </summary>
        /// <param name="strucIDList"></param>
        /// <param name="isNightBan"></param>
        /// <returns></returns>
        public static OperationResult NightBan(string strucIDList, int isNightBan)
        {
            string sql = string.Format(@"UPDATE  [dbo].[Structures] SET [IsNightBan] = {0} WHERE ID IN ({1});", isNightBan, strucIDList.TrimEnd(','));

            var result = MSSQLHelper.ExecuteNonQuery(CommandType.Text, sql, null) > 0;

            return new OperationResult()
            {
                Success = result,
                Message = result ? PromptInformation.OperationSuccess : PromptInformation.OperationFailure
            };

        }
        #endregion
    }
}

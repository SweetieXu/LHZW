using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Asiatek.Common
{
    public class ExcelHelper
    {
        /// <summary>
        /// 转换Excel文件到DataTable
        /// </summary>
        /// <param name="filePath">Excel文件路径</param>
        /// <param name="sheetName">Excel文件中sheet名</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string filePath, string sheetName)
        {
            try
            {
                string strCon = @"Provider=Microsoft.Ace.OleDb.12.0;Data Source=@filePath;Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
                strCon = strCon.Replace("@filePath", filePath);
                using (OleDbConnection conn = new OleDbConnection(strCon))
                {
                    string sql = "SELECT * FROM [@sheetName$]";
                    sql = sql.Replace("@sheetName", sheetName);
                    DataTable dt = new DataTable();
                    using (OleDbDataAdapter da = new OleDbDataAdapter(sql, conn))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }

        }




    }
}

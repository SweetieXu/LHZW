
namespace Asiatek.Common
{
    /// <summary>
    /// String扩展方法类
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 转义模糊查询特殊字符
        /// </summary>
        /// <param name="queryConditions"></param>
        /// <returns></returns>
        public static string EscapeFuzzyQuerySpecialCharacters(this string queryConditions)
        {
            if (string.IsNullOrWhiteSpace(queryConditions))
            {
                return queryConditions;
            }
            // 必须先把 [ 替换成 [[] 写在最前面 否则后续的替换会把前面的 [ 替换成 [[]
            // 如 先替换 _ 变成 [_] 后续又有 [ 替换成 [[]  那么之前的[_] 会变成 [[]_]
            return queryConditions.Replace("[", "[[]").Replace("_", "[_]").Replace("%", "[%]");
        }
    }
}

namespace Asiatek.Common
{
    /// <summary>
    /// 模糊查询
    /// </summary>
    public class FuzzyQueryHelper
    {
        /// <summary>
        /// 模糊查询需要对于特殊字符的处理（下划线_，百分号%，方括号[） 
        /// 对于 ] 不需要 使用[]包含起来 使用了反而查不出来数据  不需要对*,^号进行处理
        /// </summary>
        /// <param name="queryConditions">查询条件</param>
        /// <returns></returns>
        public static string HandleSpecialCharacters(string queryConditions)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.AjaxPager
{
    /// <summary>
    /// 亚士德LINQ分页扩展
    /// </summary>
    public static class AsiatekPageLinqExtensions
    {
        /// <summary>
        /// 将枚举内容转换为分页集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="items">数据集合</param>
        /// <param name="currentPageIndex">当前页索引</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="totalItemCount">总记录数</param>
        /// <returns></returns>
        public static AsiatekPagedList<T> ToPagedList<T>(this IEnumerable<T> items, int currentPageIndex, int pageSize, int totalItemCount)
        {
            return new AsiatekPagedList<T>(items, currentPageIndex, pageSize, totalItemCount);
        }
    }
}

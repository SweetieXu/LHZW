using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.AjaxPager
{
    /// <summary>
    /// 亚士德分页集合
    /// </summary>
    public class AsiatekPagedList<T> : List<T>, IAsiatekPagedList
    {
        /// <summary>
        /// 分页集合
        /// </summary>
        /// <param name="items">分页数据</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalItemCount">总记录数</param>
        public AsiatekPagedList(IEnumerable<T> items, int currentPageIndex, int pageSize, int totalItemCount)
        {
            AddRange(items);
            TotalItemCount = totalItemCount;
            CurrentPageIndex = currentPageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 当前页索引（从1开始）
        /// </summary>
        public int CurrentPageIndex { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalItemCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                return (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            }
        }
    }
}

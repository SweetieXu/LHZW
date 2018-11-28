using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.AjaxPager
{
    /// <summary>
    /// 包含分页数据与搜索页索引
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ContainsPagedDatas<T>
    {
        /// <summary>
        /// 分页结果
        /// </summary>
        public AsiatekPagedList<T> PagedDatas { get; set; }
        /// <summary>
        /// 搜索页索引（从1开始）
        /// </summary>
        public int SearchPage { get; set; }
    }
}

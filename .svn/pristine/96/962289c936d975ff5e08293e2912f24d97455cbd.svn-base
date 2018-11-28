using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asiatek.AjaxPager
{
    /// <summary>
    /// 查询数据与分页结果
    /// </summary>
    /// <typeparam name="TSearchModel">包含查询条件字段的类型</typeparam>
    /// <typeparam name="TPagedModel">分页数据对应的类型</typeparam>
    public sealed class SearchDataWithPagedDatas<TSearchModel, TPagedModel>
    {
        /// <summary>
        /// 包含查询表单字段的模型
        /// </summary>
        public TSearchModel SearchModel { get; set; }
        /// <summary>
        /// 分页数据
        /// </summary>
        public AsiatekPagedList<TPagedModel> PagedDatas { get; set; }
        /// <summary>
        /// 搜索页索引（从1开始）
        /// </summary>
        public int SearchPage { get; set; }
    }
}

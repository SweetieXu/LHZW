using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Asiatek.Model
{
    #region 查询
    /// <summary>
    /// 车辆异常信息
    /// </summary>
    public class GetVehicleExceptionInfoModel
    {
        /// <summary>
        /// 异常类型编号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 异常名称
        /// </summary>
        public string ExName { get; set; }
        /// <summary>
        /// 异常备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public long VehicleID { get; set; }
    }

    /// <summary>
    /// 给车辆异常页面传递的参数
    /// </summary>
    public class VehicleExceptionInfoParaModel
    {
        public int ID { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNum { get; set; }
    }

    #endregion
}

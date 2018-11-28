using Asiatek.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Asiatek.AjaxPager;
using Asiatek.Resource;

namespace Asiatek.Model
{

    public class StructureTreeModel
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 单位名
        /// </summary>
        public string StrucName { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public int? ParentID { get; set; }
    }

    public class StrucTreeComparer : IEqualityComparer<StructureTreeModel>
    {

        public bool Equals(StructureTreeModel x, StructureTreeModel y)
        {
            if (x == y)
            {
                return true;
            }
            if (x == null && y != null)
            {
                return false;
            }
            if (x != null && y == null)
            {
                return false;
            }
            if (x.ID == y.ID && x.ParentID == y.ParentID && x.StrucName == y.StrucName)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(StructureTreeModel obj)
        {
            return obj.ID.GetHashCode() + obj.ParentID.GetHashCode() + obj.StrucName.GetHashCode();
        }
    }


    /// <summary>
    /// 单位下拉列表模型
    /// </summary>
    public class StructureDDLModel
    {
        /// <summary>
        /// 单位ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 单位名
        /// </summary>
        public string StrucName { get; set; }
    }



    public class StructureListModel
    {

        public int ID { get; set; }

        [Display(Name = "StrucName", ResourceType = typeof(DisplayText))]
        public string StrucName { get; set; }
        [Display(Name = "StrucAccount", ResourceType = typeof(DisplayText))]
        public string StrucAccount { get; set; }
        public int? ParentID { get; set; }

        /// <summary>
        /// 查岗通知类型1（平台）
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool InspectType1 { get; set; }

        /// <summary>
        /// 查岗通知类型2（短信）
        /// </summary>
        [Display(Name = "NotificationTypeMessage", ResourceType = typeof(DisplayText))]
        public bool InspectType2 { get; set; }

        /// <summary>
        /// 查岗通知类型3（管信）
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool InspectType3 { get; set; }


        /// <summary>
        /// 异常通知类型：平台
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType1 { get; set; }


        /// <summary>
        /// 异常通知类型：管信
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType3 { get; set; }

        public string LevelID { get; set; }

        public int IsNightBan { get; set; }
    }



    /// <summary>
    /// 单位设定
    /// </summary>
    public class StructureSettingModel : ContainsPagedDatas<StructureListModel>
    {

        /// <summary>
        /// 单位账号或名称
        /// </summary>
        [Display(Name = "StrucAccountOrName", ResourceType = typeof(DisplayText))]
        public string StrucAccountOrName { get; set; }
        /// <summary>
        /// 是否级联（查询出某个单位后，是否自动带出所有下级信息，包括直接与间接下级）
        /// </summary>
        [Display(Name = "IsCascaded", ResourceType = typeof(DisplayText))]
        public bool IsCascaded { get; set; }

        /// <summary>
        /// 查岗通知类型1（平台）
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool InspectType1 { get; set; }

        /// <summary>
        /// 查岗通知类型2（短信）
        /// </summary>
        [Display(Name = "NotificationTypeMessage", ResourceType = typeof(DisplayText))]
        public bool InspectType2 { get; set; }

        /// <summary>
        /// 查岗通知类型3（管信）
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool InspectType3 { get; set; }


        /// <summary>
        /// 异常通知类型：平台
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType1 { get; set; }


        /// <summary>
        /// 异常通知类型：管信
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType3 { get; set; }



        /// <summary>
        /// 查岗通知类型 ：不限制（选择不限制将查询所有查岗通知类型的数据）
        /// </summary>
        [Display(Name = "NotificationTypeAny", ResourceType = typeof(DisplayText))]
        public bool InspectTypeAny { get; set; }

        /// <summary>
        /// 异常通知类型 ：不限制（选择不限制将查询所有异常通知类型的数据）
        /// </summary>
        [Display(Name = "NotificationTypeAny", ResourceType = typeof(DisplayText))]
        public bool ExNoticeTypeAny { get; set; }
        /// <summary>
        /// 夜间禁行
        /// </summary>
        [Display(Name = "Nightban", ResourceType = typeof(DisplayText))]
        public int Nightban { get; set; }
    }

    /// <summary>
    /// 根据用户id查出来的Structure信息模型  对应sql函数Func_GetStrucListByUserID
    /// </summary>
    public class StrucByUIDModel
    {
        public int id { get; set; }

        /// <summary>
        /// 单位编号
        /// </summary>
        public string StrucCode { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string StrucName { get; set; }

        public int ParentID { get; set; }

        /// <summary>
        /// 业户经营许可证号
        /// </summary>
        public string LicenseNo { get; set; }
    }

    /// <summary>
    /// 新增单位模型
    /// </summary>
    public class StructureAddModel
    {

        /// <summary>
        /// 上级单位ID
        /// </summary>
        [Display(Name = "ParentStructure", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
          ErrorMessageResourceName = "MustSelect")]
        public int? ParentID { get; set; }

        public string ParentStructureName { get; set; }
        /// <summary>
        /// 上级单位下拉项
        /// </summary>

        //public IEnumerable<SelectListItem> ParentStructureSelectList { get; set; }

        /// <summary>
        /// 单位账号
        /// </summary>
        [Display(Name = "StrucAccount", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddStrucAccountExists", "Structure", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string StrucAccount { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Display(Name = "StrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MustInput")]
        [StringLength(200, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddStrucNameExists", "Structure", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string StrucName { get; set; }





        /// <summary>
        /// 查岗手机号 用半角;分割 最多4个
        /// </summary>
        [Display(Name = "InspectMobiles", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^(1[0-9]{10};){1,4}$",
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "StructureMobilesError")]
        public string InspectMobiles { get; set; }


        /// <summary>
        /// 异常提醒手机号  用半角;分割 最多4个
        /// </summary>
        [Display(Name = "ExMobiles", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^(1[0-9]{10};){1,4}$",
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "StructureMobilesError")]
        public string ExMobiles { get; set; }


        /// <summary>
        /// 联系人1
        /// </summary>
        [Display(Name = "LinkMan1", ResourceType = typeof(DisplayText))]
        public string LinkMan1 { get; set; }


        /// <summary>
        /// 联系人2
        /// </summary>
        [Display(Name = "LinkMan2", ResourceType = typeof(DisplayText))]
        public string LinkMan2 { get; set; }


        /// <summary>
        /// 联系人3
        /// </summary>
        [Display(Name = "LinkMan3", ResourceType = typeof(DisplayText))]
        public string LinkMan3 { get; set; }


        /// <summary>
        /// 联系电话1
        /// </summary>
        [Display(Name = "LinkMobile1", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string LinkMobile1 { get; set; }


        /// <summary>
        /// 联系电话2
        /// </summary>
        [Display(Name = "LinkMobile2", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
        ErrorMessageResourceType = typeof(DataAnnotations),
        ErrorMessageResourceName = "ChinesePhoneError")]
        public string LinkMobile2 { get; set; }


        /// <summary>
        /// 联系电话3
        /// </summary>
        [Display(Name = "LinkMobile3", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "ChinesePhoneError")]
        public string LinkMobile3 { get; set; }


        /// <summary>
        /// 地图类型谷歌
        /// </summary>
        [Display(Name = "MapTypeGoogle", ResourceType = typeof(DisplayText))]
        public bool MapType1 { get; set; }


        /// <summary>
        /// 地图类型离线
        /// </summary>
        [Display(Name = "MapTypeOffline", ResourceType = typeof(DisplayText))]
        public bool MapType2 { get; set; }


        /// <summary>
        /// 地图类型百度
        /// </summary>
        [Display(Name = "MapTypeBaiDu", ResourceType = typeof(DisplayText))]
        public bool MapType3 { get; set; }



        /// <summary>
        /// 查岗通知类型1（平台）
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool InspectType1 { get; set; }

        /// <summary>
        /// 查岗通知类型2（短信）
        /// </summary>
        [Display(Name = "NotificationTypeMessage", ResourceType = typeof(DisplayText))]
        public bool InspectType2 { get; set; }

        /// <summary>
        /// 查岗通知类型3（管信）
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool InspectType3 { get; set; }


        /// <summary>
        /// 异常通知类型：平台
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType1 { get; set; }


        /// <summary>
        /// 异常通知类型：管信
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType3 { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

        /// <summary>
        /// 单位LOGO
        /// </summary>
        [Display(Name = "Logo", ResourceType = typeof(DisplayText))]
        public HttpPostedFileBase LogoFile { get; set; }

        /// <summary>
        /// 单位logo图片文件字节数组
        /// </summary>
        public byte[] Logo { get; set; }

        /// <summary>
        /// 业户经营许可证号
        /// </summary>
        [Display(Name = "LicenseNo", ResourceType = typeof(DisplayText))]
        [AsiatekRemote("CheckAddStrucLicenseNoExists", "Structure", "Admin", HttpMethod = "POST",
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "FieldExists")]
        public string LicenseNo { get; set; }
        /// <summary>
        /// 用户自定义编码
        /// </summary>
        [Display(Name = "CustomEncoding", ResourceType = typeof(DisplayText))]
        [AsiatekRemote("CheckAddCustomEncodingExists", "Structure", "Admin", HttpMethod = "POST",
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "FieldExists")]
        public string CustomEncoding { get; set; }
    }





    /// <summary>
    /// 新增单位模型
    /// </summary>
    public class StructureEditModel
    {

        public int ID { get; set; }

        /// <summary>
        /// 单位账号
        /// </summary>
        [Display(Name = "StrucAccount", ResourceType = typeof(DisplayText))]
        public string StrucAccount { get; set; }


        /// <summary>
        /// 单位名称
        /// </summary>
        [Display(Name = "StrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MustInput")]
        [StringLength(200, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckEditStrucNameExists", "Structure", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string StrucName { get; set; }

        ///// <summary>
        ///// 上级单位下拉项
        ///// </summary>
        //public IEnumerable<SelectListItem> ParentStructureSelectList { get; set; }


        /// <summary>
        /// 上级单位编号
        /// </summary>
        [Display(Name = "ParentStructure", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
          ErrorMessageResourceName = "MustSelect")]
        public int? ParentID { get; set; }

        public string ParentStructureName { get; set; }


        /// <summary>
        /// 查岗手机号 用半角;分割 最多4个
        /// </summary>
        [Display(Name = "InspectMobiles", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^(1[0-9]{10};){1,4}$",
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "StructureMobilesError")]
        public string InspectMobiles { get; set; }


        /// <summary>
        /// 异常提醒手机号  用半角;分割 最多4个
        /// </summary>
        [Display(Name = "ExMobiles", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^(1[0-9]{10};){1,4}$",
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "StructureMobilesError")]
        public string ExMobiles { get; set; }


        /// <summary>
        /// 联系人1
        /// </summary>
        [Display(Name = "LinkMan1", ResourceType = typeof(DisplayText))]
        public string LinkMan1 { get; set; }


        /// <summary>
        /// 联系人2
        /// </summary>
        [Display(Name = "LinkMan2", ResourceType = typeof(DisplayText))]
        public string LinkMan2 { get; set; }


        /// <summary>
        /// 联系人3
        /// </summary>
        [Display(Name = "LinkMan3", ResourceType = typeof(DisplayText))]
        public string LinkMan3 { get; set; }


        /// <summary>
        /// 联系电话1
        /// </summary>
        [Display(Name = "LinkMobile1", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string LinkMobile1 { get; set; }


        /// <summary>
        /// 联系电话2
        /// </summary>
        [Display(Name = "LinkMobile2", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
        ErrorMessageResourceType = typeof(DataAnnotations),
        ErrorMessageResourceName = "ChinesePhoneError")]
        public string LinkMobile2 { get; set; }


        /// <summary>
        /// 联系电话3
        /// </summary>
        [Display(Name = "LinkMobile3", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "ChinesePhoneError")]
        public string LinkMobile3 { get; set; }


        /// <summary>
        /// 地图类型谷歌
        /// </summary>
        [Display(Name = "MapTypeGoogle", ResourceType = typeof(DisplayText))]
        public bool MapType1 { get; set; }


        /// <summary>
        /// 地图类型离线
        /// </summary>
        [Display(Name = "MapTypeOffline", ResourceType = typeof(DisplayText))]
        public bool MapType2 { get; set; }


        /// <summary>
        /// 地图类型百度
        /// </summary>
        [Display(Name = "MapTypeBaiDu", ResourceType = typeof(DisplayText))]
        public bool MapType3 { get; set; }



        /// <summary>
        /// 查岗通知类型1（平台）
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool InspectType1 { get; set; }

        /// <summary>
        /// 查岗通知类型2（短信）
        /// </summary>
        [Display(Name = "NotificationTypeMessage", ResourceType = typeof(DisplayText))]
        public bool InspectType2 { get; set; }

        /// <summary>
        /// 查岗通知类型3（管信）
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool InspectType3 { get; set; }


        /// <summary>
        /// 异常通知类型：平台
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType1 { get; set; }


        /// <summary>
        /// 异常通知类型：管信
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType3 { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

        /// <summary>
        /// 是否修改LOGO
        /// 当修改单位时，如果不想修改LOGO就保持为false，这样不会因为没有上传LOGO而覆盖掉之前的LOGO
        /// </summary>
        [Display(Name = "ModifyLogo", ResourceType = typeof(DisplayText))]
        public bool ModifyLogo { get; set; }

        /// <summary>
        /// 单位LOGO
        /// </summary>
        [Display(Name = "Logo", ResourceType = typeof(DisplayText))]
        public HttpPostedFileBase LogoFile { get; set; }

        /// <summary>
        /// 单位logo图片文件字节数组
        /// </summary>
        public byte[] Logo { get; set; }

        /// <summary>
        /// 业户经营许可证号
        /// </summary>
        [Display(Name = "LicenseNo", ResourceType = typeof(DisplayText))]
        [AsiatekRemote("CheckEditStrucLicenseNoExists", "Structure", "Admin", HttpMethod = "POST",
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string LicenseNo { get; set; }

        /// <summary>
        /// 是否具有LOGO
        /// </summary>
        public bool HasLogo { get; set; }

        /// <summary>
        /// 用户自定义编码
        /// </summary>
        [Display(Name = "CustomEncoding", ResourceType = typeof(DisplayText))]
        [AsiatekRemote("CheckEditCustomEncodingExists", "Structure", "Admin", HttpMethod = "POST",
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "FieldExists", AdditionalFields = "ID")]
        public string CustomEncoding { get; set; }
    }







    /// <summary>
    /// 新增子单位模型
    /// </summary>
    public class StructureAddSubModel
    {



        /// <summary>
        /// 单位编号
        /// </summary>
        [Display(Name = "StrucAccount", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MustInput")]
        [StringLength(20, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddStrucAccountExists", "Structure", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string StrucAccount { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [Display(Name = "StrucName", ResourceType = typeof(DisplayText))]
        [Required(ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MustInput")]
        [StringLength(200, ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "MaxLength")]
        [AsiatekRemote("CheckAddStrucNameExists", "Structure", "Admin", HttpMethod = "POST",
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "FieldExists")]
        public string StrucName { get; set; }


        /// <summary>
        /// 上级单位ID
        /// </summary>
        [Display(Name = "ParentStructure", ResourceType = typeof(DisplayText))]
        public int ParentID { get; set; }

        /// <summary>
        /// 上级单位名称
        /// </summary>
        public string ParentStrucName { get; set; }

        /// <summary>
        /// 查岗手机号 用半角;分割 最多4个
        /// </summary>
        [Display(Name = "InspectMobiles", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^(1[0-9]{10};){1,4}$",
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "StructureMobilesError")]
        public string InspectMobiles { get; set; }


        /// <summary>
        /// 异常提醒手机号  用半角;分割 最多4个
        /// </summary>
        [Display(Name = "ExMobiles", ResourceType = typeof(DisplayText))]
        [RegularExpression(@"^(1[0-9]{10};){1,4}$",
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "StructureMobilesError")]
        public string ExMobiles { get; set; }


        /// <summary>
        /// 联系人1
        /// </summary>
        [Display(Name = "LinkMan1", ResourceType = typeof(DisplayText))]
        public string LinkMan1 { get; set; }


        /// <summary>
        /// 联系人2
        /// </summary>
        [Display(Name = "LinkMan2", ResourceType = typeof(DisplayText))]
        public string LinkMan2 { get; set; }


        /// <summary>
        /// 联系人3
        /// </summary>
        [Display(Name = "LinkMan3", ResourceType = typeof(DisplayText))]
        public string LinkMan3 { get; set; }


        /// <summary>
        /// 联系电话1
        /// </summary>
        [Display(Name = "LinkMobile1", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "ChinesePhoneError")]
        public string LinkMobile1 { get; set; }


        /// <summary>
        /// 联系电话2
        /// </summary>
        [Display(Name = "LinkMobile2", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
        ErrorMessageResourceType = typeof(DataAnnotations),
        ErrorMessageResourceName = "ChinesePhoneError")]
        public string LinkMobile2 { get; set; }


        /// <summary>
        /// 联系电话3
        /// </summary>
        [Display(Name = "LinkMobile3", ResourceType = typeof(DisplayText))]
        [ChinesePhone(
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "ChinesePhoneError")]
        public string LinkMobile3 { get; set; }


        /// <summary>
        /// 地图类型谷歌
        /// </summary>
        [Display(Name = "MapTypeGoogle", ResourceType = typeof(DisplayText))]
        public bool MapType1 { get; set; }


        /// <summary>
        /// 地图类型离线
        /// </summary>
        [Display(Name = "MapTypeOffline", ResourceType = typeof(DisplayText))]
        public bool MapType2 { get; set; }


        /// <summary>
        /// 地图类型百度
        /// </summary>
        [Display(Name = "MapTypeBaiDu", ResourceType = typeof(DisplayText))]
        public bool MapType3 { get; set; }



        /// <summary>
        /// 查岗通知类型1（平台）
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool InspectType1 { get; set; }

        /// <summary>
        /// 查岗通知类型2（短信）
        /// </summary>
        [Display(Name = "NotificationTypeMessage", ResourceType = typeof(DisplayText))]
        public bool InspectType2 { get; set; }

        /// <summary>
        /// 查岗通知类型3（管信）
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool InspectType3 { get; set; }


        /// <summary>
        /// 异常通知类型：平台
        /// </summary>
        [Display(Name = "NotificationTypePlateform", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType1 { get; set; }


        /// <summary>
        /// 异常通知类型：管信
        /// </summary>
        [Display(Name = "NotificationTypeGuanXin", ResourceType = typeof(DisplayText))]
        public bool ExNoticeType3 { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark", ResourceType = typeof(DisplayText))]
        [StringLength(500,
            ErrorMessageResourceType = typeof(DataAnnotations),
            ErrorMessageResourceName = "MaxLength")]
        public string Remark { get; set; }

        /// <summary>
        /// 单位LOGO
        /// </summary>
        [Display(Name = "Logo", ResourceType = typeof(DisplayText))]
        public HttpPostedFileBase LogoFile { get; set; }

        /// <summary>
        /// 单位logo图片文件字节数组
        /// </summary>
        public byte[] Logo { get; set; }

        /// <summary>
        /// 业户经营许可证号
        /// </summary>
        [Display(Name = "LicenseNo", ResourceType = typeof(DisplayText))]
        [AsiatekRemote("CheckAddStrucLicenseNoExists", "Structure", "Admin", HttpMethod = "POST",
ErrorMessageResourceType = typeof(DataAnnotations),
ErrorMessageResourceName = "FieldExists")]
        public string LicenseNo { get; set; }

        /// <summary>
        /// 用户自定义编码
        /// </summary>
        [Display(Name = "CustomEncoding", ResourceType = typeof(DisplayText))]
        [AsiatekRemote("CheckAddCustomEncodingExists", "Structure", "Admin", HttpMethod = "POST",
    ErrorMessageResourceType = typeof(DataAnnotations),
    ErrorMessageResourceName = "FieldExists")]
        public string CustomEncoding { get; set; }
    }







}
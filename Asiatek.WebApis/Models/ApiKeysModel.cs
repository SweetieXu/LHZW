using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Asiatek.WebApis.Models
{
    public class ApiKeysModel
    {
        public IEnumerable<KeyModel> Keys { get; set; }
    }
    public class KeyModel
    {
        /// <summary>
        /// 接入Key
        /// </summary>
        public string AccessKey { get; set; }
        /// <summary>
        /// 加密Key
        /// </summary>
        public string SecretKey { get; set; }
    }
}
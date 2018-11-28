using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Asiatek.WebApis.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Health")]
    public class HealthController : BaseApiController
    {
        [Route("healthcheck")]
        [HttpGet]
        public IEnumerable<string> HealthCheck()
        {
            return new string[] { "success" };
        }
    }
}

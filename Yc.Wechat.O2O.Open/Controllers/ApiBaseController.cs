using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Yc.Wechat.O2O.Open.Controllers
{
    [Authorize]
    public class ApiBaseController : ApiController
    {
        protected int Wid
        {
            get
            {
                if (!User.Identity.IsAuthenticated) return 0;
                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity == null) return 0;
                var wid = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "as:wid");
                return wid != null ? Convert.ToInt32(wid.Value) : 0;
            }
        }
    }
}
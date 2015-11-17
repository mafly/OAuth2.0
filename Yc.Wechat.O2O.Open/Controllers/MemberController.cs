using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Yc.Wechat.O2O.Open.Controllers
{
    public class MemberController : ApiBaseController
    {
        // GET api/member
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/member/5
        public string Get(int id)
        {
            return "value";
        }
    }
}

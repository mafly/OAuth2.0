using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using Yc.WeChat.O2O.Model;

namespace Yc.Wechat.O2O.Open
{
    public class RefreshAuthenticationTokenProvider : AuthenticationTokenProvider
    {
        public override async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            if (string.IsNullOrEmpty(context.Ticket.Identity.Name)) return;

            var refreshToken = Guid.NewGuid().ToString("n");
            var clietId = context.OwinContext.Get<string>("as:client_id");
            if (string.IsNullOrEmpty(clietId)) return;

            var refreshTokenModel = new yc_developers_refresh_token
            {
                refresh_token = refreshToken,
                create_datetime = DateTime.Now,
                remark = context.SerializeTicket()
            };

            context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddHours(2);

            if (new WeChat.O2O.BLL.yc_developers_refresh_token().Add(refreshTokenModel))
                context.SetToken(refreshToken);
        }

        public override Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var refreshTokenBll = new WeChat.O2O.BLL.yc_developers_refresh_token();
            var refreshTokenEntity = refreshTokenBll.GetModel(context.Token);
            if (refreshTokenEntity != null)
            {
                context.DeserializeTicket(refreshTokenEntity.remark);
                refreshTokenBll.Delete(context.Token);
            }
            return base.ReceiveAsync(context);
        }
    }
}
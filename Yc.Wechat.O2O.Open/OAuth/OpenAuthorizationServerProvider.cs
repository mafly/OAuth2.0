using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Yc.WeChat.O2O.BLL;

namespace Yc.Wechat.O2O.Open
{
    public class OpenAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        ///     验证客户端
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            //context.TryGetFormCredentials(out clientId, out clientSecret);
            context.TryGetBasicCredentials(out clientId, out clientSecret);

            var developerEntity =
                new yc_developers_info().GetModelList(string.Format("client_id='{0}'", clientId)).FirstOrDefault();
            if (developerEntity == null || developerEntity.id <= 0)
            {
                context.SetError("invalid_client", "client is not valid");
                return;
            }
            if (developerEntity.client_secret != clientSecret)
            {
                context.SetError("invalid_client", "clientSecret is not valid");
                return;
            }
            if (developerEntity.is_open == 0)
            {
                context.SetError("invalid_client", "developer service is not open");
                return;
            }
            if (developerEntity.begain_datetime > DateTime.Now || developerEntity.end_datetime < DateTime.Now)
            {
                context.SetError("invalid_client", "developer license expired");
                return;
            }

            context.OwinContext.Set("as:client_id", clientId);
            context.OwinContext.Set("as:client_wid", developerEntity.wid.ToString());
            context.Validated(clientId);
            //return base.ValidateClientAuthentication(context);
        }

        /// <summary>
        ///     客户端授权[生成access token]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);

            //oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.OwinContext.Get<string>("as:client_id")));
            oAuthIdentity.AddClaim(new Claim("as:wid", context.OwinContext.Get<string>("as:client_wid")));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties() { AllowRefresh = true });

            context.Validated(ticket);
            return base.GrantClientCredentials(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //验证context.UserName与context.Password 
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            context.Validated(oAuthIdentity);
        }

        /// <summary>
        ///     刷新Token[刷新refresh_token]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            //enforce client binding of refresh token
            if (context.Ticket == null || context.Ticket.Identity == null || !context.Ticket.Identity.IsAuthenticated)
            {
                context.SetError("invalid_grant", "Refresh token is not valid");
            }
            else
            {
                //Additional claim is needed to separate access token updating from authentication 
                //requests in RefreshTokenProvider.CreateAsync() method

                var newId = new ClaimsIdentity(context.Ticket.Identity);
                newId.AddClaim(new Claim("newClaim", "refreshToken"));

                var newTicket = new AuthenticationTicket(newId, context.Ticket.Properties);
                context.Validated(newTicket);
            }
            return base.GrantRefreshToken(context);
        }
    }
}
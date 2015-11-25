using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Mafly.OAuth2._0.Demo.OAuth
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
            context.TryGetFormCredentials(out clientId, out clientSecret);
            //context.TryGetBasicCredentials(out clientId, out clientSecret); //Basic认证

            //TODO:读库，验证
            if (clientId != "malfy" && clientSecret != "111111")
            {
                context.SetError("invalid_client", "client is not valid");
                return;
            }
            context.OwinContext.Set("as:client_id", clientId);
            context.Validated(clientId);
        }

        /// <summary>
        ///     客户端授权[生成access token]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, context.OwinContext.Get<string>("as:client_id")));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties {AllowRefresh = true});
            context.Validated(ticket);
            return base.GrantClientCredentials(context);
        }
    }
}
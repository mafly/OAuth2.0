using System;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Yc.Wechat.O2O.Open;

[assembly: OwinStartup(typeof(Startup))]

namespace Yc.Wechat.O2O.Open
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            //开启了OAuth服务
            ConfigureOAuth(app);

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            // Token Generation
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,   //允许客户端使用http协议请求
                AuthenticationMode = AuthenticationMode.Active,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),   //token过期时间

                //提供具体的认证策略
                Provider = new OpenAuthorizationServerProvider(),
                RefreshTokenProvider = new RefreshAuthenticationTokenProvider()
            };
            app.UseOAuthBearerTokens(oAuthOptions);
        }

    }

}

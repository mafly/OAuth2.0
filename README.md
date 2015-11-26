# 我也想聊聊 OAuth 2.0 
[一、基本概念][9]
====

话说我也是今天看了霍金老师的《时间简史》后，我才知道霍金不姓「霍」，我在这里报以诚挚的歉意！对不起，霍老师，我不是人。

回到正题，最近公司的一个O2O项目，领导打算把用户数据共享给开发者，估计是疯了。当然，这也就涉及到开发者认证、API接口、Access Token、AppKey、OAuth这些你如果没接触过就会听的云里雾里的什么鬼。其实这些名词都包含在OAuth 2.0的概念中，从今天起，我要一步步掰开揉碎来理解它们，或许也能帮到你说不定。

## 什么是 OAuth 2.0？
为什么是2.0？肯定有1.0。
我打开了维基百科：OAuth（开放授权）是一个`开放标准`，允许用户让`第三方`应用访问该`用户`在某`一网站`上存储的私密的`资源`（如照片，视频，联系人列表），而无需将`用户名`和`密码`提供给第三方应用。看到这，也需你「哦」了一声，你是不是想起了QQ、微博等快捷登录、微信授权和TX的不要脸。OK，那接着看2.0。
OAuth 2.0是OAuth协议的`下一版本`，但不向下兼容OAuth 1.0。OAuth 2.0关注客户端开发者的`简易性`，同时为`Web应用`、`桌面应用`和`手机`，和`起居室设备`提供专门的认证流程。不就是升级版么，有什么了不起。对了，这就是 OAuth 2.0 。

## 为什么是 OAuth 2.0？
你今天刚刚发现了一个比较好玩的网站，它可以装逼，不是知乎，是[逼乎][6]。你是不是很好奇，点了进去，看到了这个「超目前主流扁平化设计的一种新型设计模式」的页面。![逼乎登录页][7]
是不是特别想进去看看，我不会告诉你里边有奥巴马、乔布斯和思聪的。当你去点击微博或QQ图标的时候，会跳转一个授权页面，当你输入你的账户及密码后，就可以去静静的装逼了。当然，逼乎是不会知道你的微博或QQ密码的，这就用到了 OAuth 2.0 协议。

## OAuth 2.0 协议有哪几种？
上面说到的「第三方快捷登录」只是 OAuth 2.0 协议的其中一种，其实它是有**四种**授权方式的，但无论如何客户端必须得到用户的授权（authorization grant），才能获得令牌（access token）。四种授权方式如下：

 **- 授权码模式（authorization code）
 - 简化模式（implicit）
 - 密码模式（resource owner password credentials）
 - 客户端模式（client credentials）**

我这就不一一赘述具体每个方式所适应的情况了，我下面几篇文章就针对我们打算开发的「开发者中心」来一行行写代码。
但请你记住：** OAuth 就是一个开放授权协议！ OAuth 就是一个开放授权协议！！ OAuth 就是一个开放授权协议！！！**
  
----------------------------------------------------------------------------------------------------
[二、Access Token][10]
=========
  
## 你在北方的寒夜里瑟瑟如狗，我在南方的艳阳里看你发抖
我的朋友圈反正今天早上是被「雪」爆了，而我依然穿着短袖。看到老三拍了两张带有老家标志建筑物的雪景照，发了条朋友圈说我有点想家了，老二回复说：回来呗。突然，鼻子一酸...回复他：老子在挤地铁！！！回屁家。然后，我喝了口凉水后心情大好！

回到正题，你肯定已经了解到什么是 OAuth 2.0 了，你说它不就是一个协议么，但是那些第三方接口都有什么`ClientId`、`ClientSecret`、`Access Token`、`Refresh Token`等等之类的，看着都头晕，这些到底是什么阿阿阿？。是这样的，我也如你一样苦恼，这些到底都是什么鬼、干什么的？这篇文章就讲清道明这些东西到底是什么鬼！！！

## 一个开发者中心的授权流程是怎样的
上文好像说到我们要开发一个「开发者中心」，所以，我想先讲一下这个所谓的「开发者中心」是怎样把数据开放给开发者的。
正如你所知道的数据是无价的，我们并不想`任何人`都能通过我们的开放平台来获取数据。举个栗子：你家有好多书，其实你并不看，但有好多朋友想看，你又不想任何人都能去你家拿走你心爱的《C#入门经典》，首先他要向你说明他想借你的书看看，你同意后说：你`请我吃饭`吧，然后我给你`我家钥匙`你自己去取吧，我忙。这个过程就叫做：**授权**！请你吃饭可以理解为`ClientId`、`ClientSecret`，你家的钥匙就是所谓的`Token`。
够了够了，你说的这些我都懂，那怎么用代码要体现呢？

## 利用 Web API 来实现 OAuth 授权
为什么是 Web API ?
因为 ASP.NET Web API 是针对接口而生的阿。况且它还是`REST风格`的哦，更轻量级一些，其实这些都不重要，重要的它够简单，十分钟即可上手。
怎么使用 OAuth 的方式实现授权？
Microsoft.Owin.Security.OAuth，就是它！你要知道这可是.Net的可爱之处，她把你需要的就放在了那里，你用不用她就在那。现在就使用它来实现 OAuth 2.0 中所说的四种授权模式之一的*客户端模式（client credentials）*。
1. 打开VS2013，新建一个 Web API 项目。[源码][5]
2. 在Project右键，选择“管理NuGet程序包”，搜索“Owin”。安装下面的包：

- Microsoft.Owin.Security.OAuth
- Microsoft.Owin.Security
- Microsoft.Owin
- Microsoft.Owin.Host.SystemWeb(我被它坑了好久)
- OWIN
- Microsoft ASP.Net Web API 2.2 OWIN
- Microsoft ASP.Net Identity OWIN

3. 修改 Startup.cs 文件如下，如没有，则新建。
```C#
using ...

[assembly: OwinStartup(typeof (Startup))]

namespace Mafly.OAuth2._0.Demo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            //开启OAuth服务
            ConfigureOAuth(app);

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            // Token 生成配置
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true, //允许客户端使用Http协议请求
                AuthenticationMode = AuthenticationMode.Active,
                TokenEndpointPath = new PathString("/token"), //请求地址
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(2), //token过期时间

                //提供认证策略
                Provider = new OpenAuthorizationServerProvider()
                //RefreshTokenProvider = new RefreshAuthenticationTokenProvider()
            };
            app.UseOAuthBearerTokens(oAuthOptions);
        }
    }
}
```

4. 新建`OpenAuthorizationServerProvider`
```C#
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
```
4. 没有第四步了。是不是很简单。[源码][5]


## 测试
我听说用[postman][1]测试API接口比较爽，然后我就用了它。

1. 当没有获取Token时，请求`/api/values`接口。
![无授权][2]

2. 那好，我们来获取Token。
![获取Token][3]

3. 得到了`access_token`，我们添加`Headers`，Header:Authorization Value:bearer [token]，token就是access_token。
![有Headers][4]

## 那行，总结一下
我们利用`Microsoft.Owin.Security.OAuth`和 Web API 实现了遵循 OAuth 2.0 协议的授权流程，这为我们开发「开发者中心」打下了坚实基础，让我们展望美好的未来吧。*ps:说的真官方，能不能说人话。*
最后一句：源码在Github上 [https://github.com/mafly/OAuth2.0][5]

-------------------------------
###### [Mafly][8]

  [1]: http://www.getpostman.com/
  [2]: http://images2015.cnblogs.com/blog/539095/201511/539095-20151126111851718-1207932973.png
  [3]: http://images2015.cnblogs.com/blog/539095/201511/539095-20151126112127093-882933124.png
  [4]: http://images2015.cnblogs.com/blog/539095/201511/539095-20151126112356640-1537119324.png
  [5]: https://github.com/mafly/OAuth2.0
  [6]: http://zhuangbi.me/
  [7]: http://images2015.cnblogs.com/blog/539095/201511/539095-20151120110250968-1824589423.png
  [8]: http://mayongfa.cn
  [9]: http://www.cnblogs.com/mafly/p/OAuth2_BasicConcept.html
  [10]: http://www.cnblogs.com/mafly/p/Access_Token.html

using System;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Whir.Framework.Extension;
using Whir.Framework.Helper;
using Whir.WeiXin.Enumeration;
using Whir.WeiXin.Models;

namespace Whir.WeiXin.Handler
{
    public class Login
    {
        private readonly string _redirectUri;
        private readonly LoginScope _loginScope;
        private readonly AppConfig _appConfig;

        public Login(string redirectUri, LoginScope loginScope, AppConfig appConfig)
        {
            _redirectUri = redirectUri;
            _loginScope = loginScope;
            _appConfig = appConfig;
            AppConfigLibrary.Create(appConfig.AppId, appConfig);
        }


        public RedirectResult InitLogin(ControllerContext context, string loginSuccessRedirectUrl, Action<WxUserInfo> loginSuccessAction)
        {
            string code = context.HttpContext.Request["code"];
            string url;
            if (string.IsNullOrEmpty(code))
            {
                url = string.Format(
                    "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect", _appConfig.AppId, _redirectUri, _loginScope.ToStr(), string.Empty);
                return new RedirectResult(url);
            }


            WxUserInfo wxUserInfo = new WxUserInfo();
            url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", _appConfig.AppId, _appConfig.AppSecret, code);
            string result = HttpHelper.Get(url);
            if (!string.IsNullOrEmpty(result))
            {
                var o = JObject.Parse(result);
                wxUserInfo.LoginToken = o.Property("access_token") == null ? "" : o["access_token"].ToStr();
                wxUserInfo.LoginRefreshToken = o.Property("refresh_token") == null ? "" : o["refresh_token"].ToStr();
                wxUserInfo.OpenId = o.Property("openid") == null ? "" : o["openid"].ToStr();
                int expires = o.Property("expires_in") == null ? 0 : o["expires_in"].ToInt();
                if (!string.IsNullOrEmpty(wxUserInfo.LoginToken) && expires > 0)
                {
                    wxUserInfo.LoginTokenOverTime = DateTime.Now.AddSeconds(expires);
                }
            }
            loginSuccessAction.Invoke(wxUserInfo);
            return new RedirectResult(loginSuccessRedirectUrl);

        }
    }
}
using System;

namespace Whir.WeiXin.Enumeration
{
    /// <summary> 网页授权的scope
    /// https://mp.weixin.qq.com/wiki/4/9ac2e7b1f1d22e9e57260f6553822520.html
    /// 1、以snsapi_base为scope发起的网页授权，是用来获取进入页面的用户的openid的，并且是静默授权并自动跳转到回调页的。用户感知的就是直接进入了回调页（往往是业务页面）
    /// 2、以snsapi_userinfo为scope发起的网页授权，是用来获取用户的基本信息的。但这种授权需要用户手动同意，并且由于用户同意过，所以无须关注，就可在授权后获取该用户的基本信息。
    /// 3、用户管理类接口中的“获取用户基本信息接口”，是在用户和公众号产生消息交互或关注后事件推送后，才能根据用户OpenID来获取用户基本信息。这个接口，包括其他微信接口，都是需要该用户（即openid）关注了公众号后，才能调用成功的。
    /// </summary>
    public enum LoginScope
    {
        SnsapiBase,
        SnsapiUserinfo
    }

    public static class LoginScopeExtension
    {
        public static string ToStr(this LoginScope loginScope)
        {
            switch (loginScope)
            {
                case LoginScope.SnsapiBase:
                    return "snsapi_base";
                case LoginScope.SnsapiUserinfo:
                    return "snsapi_userinfo";
                default:
                    throw new Exception("不是澄清的授权类型");
            }
        }
    }
}
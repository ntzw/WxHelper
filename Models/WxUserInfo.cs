using System;

namespace Whir.WeiXin.Models
{
    public class WxUserInfo
    {
        public string OpenId { get; set; }

        /// <summary>登录Token
        /// </summary>
        public string LoginToken { get; set; }

        /// <summary>
        /// 登录刷新Token
        /// </summary>
        public string LoginRefreshToken { get; set; }

        /// <summary>登录Token超时时间
        /// </summary>
        public DateTime LoginTokenOverTime { get; set; }
    }
}
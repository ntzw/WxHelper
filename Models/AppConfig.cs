using System;
using System.Collections.Generic;

namespace Whir.WeiXin.Models
{
    public class AppConfig
    {

        public string AppId { get; set; }
        public string AppSecret { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string Mchid { get; set; }

        /// <summary>
        /// 商户key
        /// </summary>
        public string Key { get; set; }

        private string _ip = "";

        public string NotifyUrl { get; set; }

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public string SslcertPath { get; set; }
        public string SslcertPassword { get; set; }

        public int ReportLevenl { get; set; }

        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
    }

    public class AppConfigLibrary
    {
        static readonly Dictionary<string, AppConfig> AppConfigs = new Dictionary<string, AppConfig>();

        public static void Create(string appid, AppConfig appConfig = null)
        {
            if (!AppConfigs.ContainsKey(appid))
            {
                AppConfigs.Add(appid, appConfig ?? new AppConfig());
            }
        }

        public static AppConfig Get(string appid)
        {
            if (AppConfigs.ContainsKey(appid)) return AppConfigs[appid];
            throw new Exception("AppId不存在");
        }
    }
}
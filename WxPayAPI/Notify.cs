﻿using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Whir.Framework.Helper;

namespace Whir.WeiXin.WxPayAPI
{
    class Notify
    {
        public HttpContextBase Context { get; set; }
        public Notify(HttpContextBase contenxt)
        {
            this.Context = contenxt;
        }

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetNotifyData(string appid)
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Context.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            LogManager.Log(this.GetType().ToString(), "Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData(appid);
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData(appid);
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                LogManager.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                Context.Response.Write(res.ToXml());
                Context.Response.End();
            }

            LogManager.Log(this.GetType().ToString(), "Check sign success");
            return data;
        }

        //派生类需要重写这个方法，进行不同的回调处理
        public virtual WxPayData ProcessNotify()
        {
            return null;
        }
    }
}
using System;
using System.Web;
using System.Web.Mvc;
using Whir.Framework.Extension;
using Whir.WeiXin.Models;
using Whir.WeiXin.WxPayAPI;

namespace Whir.WeiXin.Handler
{
    /// <summary>
    /// 微信支付辅助
    /// </summary>
    public class Payment
    {
        readonly AppConfig _appConfig = new AppConfig();
        private readonly HttpContextBase _context;

        public Payment(AppConfig appConfig, HttpContextBase context)
        {
            if (_appConfig != null)
            {
                _appConfig = appConfig;
                _context = context;
                AppConfigLibrary.Create(_appConfig.AppId, _appConfig);
            }
            throw new Exception("配置不能NULL");
        }

        /// <summary>获取公众号支付JSON字符串
        /// </summary>
        /// <param name="openId">微信用户OpenId</param>
        /// <param name="order"></param>
        /// <returns></returns>
        public string GetJsApiParameters(string openId, WxOrder order)
        {
            JsApiPay jsApiPay = new JsApiPay(_context, _appConfig);
            return jsApiPay.GetJsApiParameters(jsApiPay.GetUnifiedOrderResult(order));

        }

        /// <summary>微信扫码支付方式1
        /// </summary>
        /// <param name="producId"></param>
        /// <returns></returns>
        public string GetPrePayUrl(string producId)
        {
            NativePay pay = new NativePay(_appConfig);
            return pay.GetPrePayUrl(producId);
        }

        /// <summary>微信扫码支付方式2
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public string GetPayUrl(string productId, WxOrder order)
        {
            return new NativePay(_appConfig).GetPayUrl(productId, order);
        }

        /// <summary>微信支付异步回调
        /// </summary>
        /// <param name="successAction">支付成功委托</param>
        /// <returns></returns>
        public ContentResult NotifyResult(Action<PaymentSuccess> successAction)
        {
            WxPayData resultPayData = new ResultNotify(_context, _appConfig).ProcessNotify();
            if (resultPayData.GetValue("return_code").ToStr() == "SUCCESS")
            {
                PaymentSuccess success = new PaymentSuccess
                {
                    Attach = resultPayData.GetValue("attach").ToStr(),
                    OrderNum = resultPayData.GetValue("out_trade_no").ToStr(),
                    TransactionId = resultPayData.GetValue("transaction_id").ToStr(),
                    TotalFee = resultPayData.GetValue("total_fee").ToInt(),
                    IsSubscribe = resultPayData.GetValue("is_subscribe").ToBoolean(),
                    Openid = resultPayData.GetValue("openid").ToStr()
                };

                successAction.Invoke(success);
            }

            return new ContentResult
            {
                Content = resultPayData.ToXml()
            };
        }
    }
}
using System.Web;
using System.Web.Mvc;
using Whir.Framework.Helper;
using Whir.WeiXin.Models;

namespace Whir.WeiXin.WxPayAPI
{
    partial class ResultNotify : Notify
    {
        private readonly AppConfig _appConfig;
        public ResultNotify(HttpContextBase context, AppConfig appConfig)
            : base(context)
        {
            _appConfig = appConfig;
        }

        public override WxPayData ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData(_appConfig.AppId);

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData(_appConfig.AppId);
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                LogManager.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                return res;
            }

            string transactionId = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transactionId, _appConfig.AppId))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData(_appConfig.AppId);
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                LogManager.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                return res;
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData(_appConfig.AppId);
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                LogManager.Log(this.GetType().ToString(), "order query success : " + res.ToXml());
                return res;
            }
        }

        //查询订单
        private bool QueryOrder(string transactionId, string appid)
        {
            WxPayData req = new WxPayData(appid);
            req.SetValue("transaction_id", transactionId);
            WxPayData res = WxPayApi.OrderQuery(req, appid);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            return false;
        }
    }
}
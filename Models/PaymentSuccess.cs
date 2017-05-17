using System;

namespace Whir.WeiXin.Models
{
    public class PaymentSuccess
    {
        /// <summary>用户OpenId
        /// </summary>
        public string Openid { get; set; }

        /// <summary>是否关注公众账号
        /// </summary>
        public bool IsSubscribe { get; set; }

        /// <summary>订单金额
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>微信支付订单号
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>商户订单金额
        /// </summary>
        public string OrderNum { get; set; }

        /// <summary>商户附加订单信息
        /// </summary>
        public string Attach { get; set; }
    }
}
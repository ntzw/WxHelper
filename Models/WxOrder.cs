namespace Whir.WeiXin.Models
{
    public class WxOrder
    {
        private string _orderNum = "";
        private string _body = "";
        private string _attach = "";
        private string _goodsTag = "";

        /// <summary>支付金额
        /// </summary>
        public int TotalFee { get; set; }

        public string OrderNum
        {
            get { return _orderNum; }
            set { _orderNum = value; }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public string Attach
        {
            get { return _attach; }
            set { _attach = value; }
        }

        public string GoodsTag
        {
            get { return _goodsTag; }
            set { _goodsTag = value; }
        }
    }
}
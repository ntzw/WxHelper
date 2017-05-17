using System;

namespace Whir.WeiXin.WxPayAPI
{
    partial class WxPayException : Exception
    {
        public WxPayException(string msg)
            : base(msg)
        {

        }
    }
}
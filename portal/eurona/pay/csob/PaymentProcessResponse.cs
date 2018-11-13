using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Eurona.pay.csob {
    public class PaymentProcessResponse : SignedResponse{       
        public string payId{get;set;}
        public string dttm{get;set;}
        public int resultCode{get;set;}//Ok=0
        public string resultMessage{get;set;}
        public int paymentStatus{get;set;}//Ok=1
        public string authCode { get; set; }
        public string merchantData { get; set; }
        public string signature{get;set;}

        public override string getData2VerifyResponse() {
            string data2Verify = 
                getStringValue(this.payId) + "|" + getStringValue(this.dttm) + "|" + 
                getIntValue(this.resultCode) + "|" + getStringValue(this.resultMessage) + "|" +
                getIntValue(this.paymentStatus) + "|" + getStringValue(this.authCode) + "|" + getStringValue(this.merchantData);
            return data2Verify;
        }
    }
}
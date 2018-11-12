using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Eurona.pay.csob {
    public class PaymentInitResponse {       
        public string payId{get;set;}
        public string dttm{get;set;}
        public int resultCode{get;set;}//Ok=0
        public string resultMessage{get;set;}
        public int paymentStatus{get;set;}
        public string signature{get;set;}
    }
}
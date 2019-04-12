using Eurona.pay.csob.utils;
using Eurona.PAY.CSOB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.pay.csob {
    public partial class testVerify : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

            //Response from init payment
            //* {"dttm":"20190412083537","signature":"Ele1rnsrdA4Uf+8fdZscAX31I2FmXPNaL35uk9cv43r/GnOcaPbol9dKTibdh2YBCrJ2oNYWRLfG+XYLBA8QoJpV8OusHEGDUUmdWBBg1lSprcgTwOYf+lMd+RsuU8NnNjzBSjq2/5xEJxvBaPTXE+KImm5Ymh1aCnD17hDomQHVKIXa5nOZr0zV0L8NlfcxBu1GK23gLakBXjNlK7P5LYDsK6Q+6l9iliNqSz/VgIHzImw2IJA96IbCKpIUUmiF8P7nh0C1Jva91I49KR6JPQHvLqFNRKw8IVs7+P+MP2Wi4vjyhsxqzzydr1cZFIAJyQUhh3VZKrR5Bf9+SUO+eQ==",
            //"payId":"26892026c5391ED","resultCode":0,"resultMessage":"OK","paymentStatus":1}

            //payId|dttm|resultCode|resultMessage|paymentStatus|authCode|merchantData
            string data2Verify = "26892026c5391ED|20190412083537|0|OK|1";
           // string signature = HttpUtility.UrlDecode(urlSignature, Encoding.UTF8);
            string signature = "Ele1rnsrdA4Uf+8fdZscAX31I2FmXPNaL35uk9cv43r/GnOcaPbol9dKTibdh2YBCrJ2oNYWRLfG+XYLBA8QoJpV8OusHEGDUUmdWBBg1lSprcgTwOYf+lMd+RsuU8NnNjzBSjq2/5xEJxvBaPTXE+KImm5Ymh1aCnD17hDomQHVKIXa5nOZr0zV0L8NlfcxBu1GK23gLakBXjNlK7P5LYDsK6Q+6l9iliNqSz/VgIHzImw2IJA96IbCKpIUUmiF8P7nh0C1Jva91I49KR6JPQHvLqFNRKw8IVs7+P+MP2Wi4vjyhsxqzzydr1cZFIAJyQUhh3VZKrR5Bf9+SUO+eQ==";

            bool verification = Crypto.Verify(CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PublicKeyPath", this), data2Verify, signature);
            if (verification == false) {
                throw new InvalidOperationException("PaymentInitResponse verification failed!");
            }

        }
    }
}
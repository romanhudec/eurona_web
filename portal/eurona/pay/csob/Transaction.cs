using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using AccountEntity = CMS.Entities.Account;
using OrderEntity = Eurona.DAL.Entities.Order;
using ShpPaymentEntity = SHP.Entities.Classifiers.Payment;
using System.Web.Script.Serialization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Diagnostics;
using Eurona.pay.csob;
using Eurona.pay.csob.utils;
using Eurona.Common.DAL.Entities;

namespace Eurona.PAY.CSOB {
    public class Transaction {

        private PaymentInitRequest paymentInit = null;
        public Transaction(PaymentInitRequest paymentInit) {
            this.paymentInit = paymentInit;
        }

        public PaymentInitResponse InitPayment(System.Web.UI.Page page) {
            if (paymentInit == null) throw new InvalidOperationException("No payment initialize!!!");
            string orderId = Crypto.DecodeFromBase64String(paymentInit.merchantData);
            CMS.EvenLog.WritoToEventLog(string.Format("InitPayment, OrderId:{0}, orderNo:{1}", orderId, this.paymentInit.orderNo), EventLogEntryType.Information);

            string paymentMerchantID = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:MerchantID", page);
            string paymentGatewayUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PaymentGatewayUrl", page);
            string paymentResultUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PaymentResultUrl", page);
            string paymentPrivateKeyPath = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PrivateKeyPath", page);
            string paymentPublicKeyPath = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PublicKeyPath", page);

            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
             delegate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                     System.Security.Cryptography.X509Certificates.X509Chain chain,
                     System.Net.Security.SslPolicyErrors sslPolicyErrors) {
                 return true;
             };

            string apiUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PaymentGatewayUrl", page);
            string method = "/payment/init";
            string paymentInitUrl = apiUrl + method;

            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new JsonConverter() });
            var json = serializer.Serialize(paymentInit);
            string responseData = httpPostPaymentInit(paymentInitUrl, json);
            PaymentInitResponse paymentInitResponse = new JavaScriptSerializer().Deserialize<PaymentInitResponse>(responseData);
            string data2Verify = paymentInitResponse.getData2VerifyResponse();
            bool verification = Crypto.Verify(CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PublicKeyPath", page), data2Verify, paymentInitResponse.signature);
            if (verification == false) {
                throw new InvalidOperationException("PaymentInitResponse verification failed!");
            }
            return paymentInitResponse;
        }

        public void ProcessPayment(System.Web.UI.Page page, PaymentInitResponse paymentInitResponse) {
            //Požadavek obsahuje položky přímo v URL adrese https://api.platebnibrana.csob.cz/api/v1/payment/process/{merchantId}/{payId}/{dttm}/{signature}
            if (paymentInitResponse == null) throw new InvalidOperationException("No payment initialize response!!!");
            string orderId = Crypto.DecodeFromBase64String(paymentInit.merchantData);
            CMS.EvenLog.WritoToEventLog(string.Format("ProcessPayment, OrderId:{0}, orderNo:{1}", orderId, this.paymentInit.orderNo), EventLogEntryType.Information);

            string apiUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PaymentGatewayUrl", page);
            string method = "/payment/process";
            string paymentProcessUrl = apiUrl + method;

            string merchantId = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:MerchantID", page);
            string dttm = DateTime.Now.ToString("yyyyMMddHHmmss");
            string signature = signPaymentProcessData(paymentInitResponse, merchantId, dttm, page);
            paymentProcessUrl = paymentProcessUrl + "/" + merchantId + "/" + paymentInitResponse.payId + "/" + dttm + "/" + HttpUtility.UrlEncode(signature, Encoding.UTF8);//SafeBase64UrlEncoder.EncodeBase64Url(signature);
            page.Response.Redirect(paymentProcessUrl, true);
        }

        private static string httpPostPaymentInit(string url, string json) {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
            httpWebRequest.ProtocolVersion = HttpVersion.Version11;
            httpWebRequest.Method = "POST";
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            //If you are using .Net 4.0 then SecurityProtocolType.Tls11 and SecurityProtocolType.Tls2 are not defined so instead you can use the hard coded value below.
            SecurityProtocolType Tls11OrTsl12 = (SecurityProtocolType)3072;
            ServicePointManager.SecurityProtocol = Tls11OrTsl12;

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            String responseData = null;
            try {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    responseData = streamReader.ReadToEnd();
                }
            } catch (WebException webex) {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream()) {
                    StreamReader reader = new StreamReader(respStream);
                    responseData = reader.ReadToEnd();
                }
                if (string.IsNullOrEmpty(responseData)) {
                    CMS.EvenLog.WritoToEventLog(webex);
                    throw webex;
                }
            }
            return responseData;
        }
        /*
        private static string httpGetProcessPayment(string url) {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
            httpWebRequest.ProtocolVersion = HttpVersion.Version11;
            httpWebRequest.Method = "GET";
            httpWebRequest.AllowAutoRedirect = true;
            //If you are using .Net 4.0 then SecurityProtocolType.Tls11 and SecurityProtocolType.Tls2 are not defined so instead you can use the hard coded value below.
            SecurityProtocolType Tls11OrTsl12 = (SecurityProtocolType)3072;
            ServicePointManager.SecurityProtocol = Tls11OrTsl12;

            String responseData = null;
            try {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
                    responseData = streamReader.ReadToEnd();
                }
            } catch (WebException webex) {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream()) {
                    StreamReader reader = new StreamReader(respStream);
                    responseData = reader.ReadToEnd();
                }
                if (string.IsNullOrEmpty(responseData)) {
                    throw webex;
                }
            }
            return responseData;
        }
         * */

        private static string signPaymentInitData(PaymentInitRequest paymentInit, System.Web.UI.Page page) {
            string data2Sign = paymentInit.getData2Sign();
            string signedData = Crypto.Sign(data2Sign, CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PrivateKeyPath", page));
            return signedData;
        }

        private static string signPaymentProcessData(PaymentInitResponse paymentInitResponse, string merchantId, string dttm, System.Web.UI.Page page) {
            //Požadavek obsahuje položky přímo v URL adrese https://api.platebnibrana.csob.cz/api/v1/payment/process/{merchantId}/{payId}/{dttm}/{signature}
            string data2Sign = merchantId + "|" + paymentInitResponse.payId + "|" + dttm;
            string signedData = Crypto.Sign(data2Sign, CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PrivateKeyPath", page));
            return signedData;
        }

        /// <summary>
        /// Vytorenie objektu Pay transakcie.
        /// </summary>
        public static Transaction CreateTransaction(OrderEntity order, string vs, System.Web.UI.Page page) {
            CMS.EvenLog.WritoToEventLog(string.Format("CreatePaymentTransaction, ID:{0}, Order Number:{1}, VS:{2}", order.Id, order.OrderNumber, vs), EventLogEntryType.Information);
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = order.AccountId });
            decimal priceTotalWithWAT = order.PriceWVAT;

            //Zdruzene Objednavky
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
            if (list.Count != 0) {
                foreach (OrderEntity orderSdruzena in list) {
                    priceTotalWithWAT += orderSdruzena.PriceWVAT;
                }
            }

            string currencyCode = order.CurrencyCode;
            if (string.IsNullOrEmpty(currencyCode)) {
                currencyCode = "CZK";
            }

            PaymentInitRequest paymentInit = new PaymentInitRequest();
            paymentInit.merchantId = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:MerchantID", page);
            paymentInit.orderNo = vs;/*order.Id.ToString();*///Referenční číslo objednávky využívané pro párování plateb, které bude uvedeno také na výpisu z banky. Numerická hodnota, maximální délka 10 číslic.
            paymentInit.dttm = DateTime.Now.ToString("yyyyMMddHHmmss");//Datum a čas odeslání požadavku ve formátu YYYYMMDDHHMMSS
            paymentInit.payOperation = "payment";//Typ platební operace. Povolené hodnoty: payment, oneclickPayment 
            paymentInit.payMethod = "card";//Typ implicitní platební metody, která bude nabídnuta zákazníkovi. Povolené hodnoty: card.
            paymentInit.totalAmount = (int)(priceTotalWithWAT * 100);
            paymentInit.currency = currencyCode;//Kód měny. Povolené hodnoty: CZK, EUR, USD, GBP, HUF, PLN, HRK, RON, NOK, SEK 
            paymentInit.closePayment = true;
            paymentInit.returnUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PaymentResultUrl", page);
            paymentInit.returnMethod = "POST";
            //paymentInit.customerId = order.AccountId.ToString();//Jednoznačné ID zákazníka, který přiděluje e-shop. Maximální délka 50 znaků. Používá se při uložení karty a jejím následném použití při další návštěvě tohoto e-shopu
            //POZOR: Od verze v1 musí být v košíku nejméně jedna (například "dobití kreditu"), nejvýše dvě položky, například "nákup na mujobchod" a "poštovné"). 
            //Omezení je dáno grafickou úpravou platební brány a v další verzi bude limit položek výrazně posunut.
            paymentInit.cart = new List<PaymentCart>();
            paymentInit.cart.Add(new PaymentCart(setStringValue("Nákup: euronabycerny.com", 20), 1, (int)(priceTotalWithWAT * 100), setStringValue("", 40)));
            /*
            if ((int)(order.CartEntity.KatalogovaCenaCelkem * 100) + (int)(order.CartEntity.DopravneEurosap * 100) == (int)(priceTotalWithWAT * 100)) {
                paymentInit.cart.Add(new PaymentCart(setStringValue("Nákup: euronabycerny.com", 20), 1, (int)(order.CartEntity.KatalogovaCenaCelkem * 100), setStringValue("", 40)));
                paymentInit.cart.Add(new PaymentCart(setStringValue("Poštovné", 20), 1, (int)(order.CartEntity.DopravneEurosap * 100), setStringValue(order.ShipmentName, 40)));
            } else {
                paymentInit.cart.Add(new PaymentCart(setStringValue("Nákup: euronabycerny.com", 20), 1, (int)(priceTotalWithWAT * 100), setStringValue("", 40)));
            }
            */

            paymentInit.description = setStringValue("Nákup na euronabycerny.com", 255);
            paymentInit.merchantData = Crypto.EncodeToBase64String(order.Id.ToString());//Libovolná pomocná data, která budou vrácena ve zpětném redirectu z platební brány na stránku obchodníka. Mohou sloužit např pro udržení kontinuity procesu na e-shopu, musí být kódována v BASE64. Maximální délka po zakódování 255 znaků.
            paymentInit.language = "CZ";//Preferovaná jazyková mutace, která se zobrazí zákazníkovi na platební bráně. Od verze 1.6 povinný parametr. Povolené hodnoty: CZ, EN, DE, FR, HU, IT, JP, PL, PT, RO, RU, SK, ES, TR, VN, HR, SI. Stejnou jazykovou sadu lze použít i v eAPI v1, v1.5 a V1.6
            paymentInit.signature = signPaymentInitData(paymentInit, page);



            Transaction transaction = new Transaction(paymentInit);
            return transaction;
        }

        private static string setStringValue(string value, int maxLength) {
            if (value.Length > maxLength) {
                value = value.Substring(0, maxLength);
            }
            return value;
        }
    }
}
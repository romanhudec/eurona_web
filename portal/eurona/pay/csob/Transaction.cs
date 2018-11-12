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
            CMS.EvenLog.WritoToEventLog(string.Format("InitPayment, OrderId:{0}", this.paymentInit.orderNo), EventLogEntryType.Information);

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
            string responseData = postJson(paymentInitUrl, json);
            PaymentInitResponse paymentInitResponse = new JavaScriptSerializer().Deserialize<PaymentInitResponse>(responseData);
            return paymentInitResponse;
        }

        public string ProcessPayment(PaymentInitResponse paymentInitResponse) {
            return string.Empty;
        }

        private static string postJson(string url, string json) {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
            httpWebRequest.ProtocolVersion = HttpVersion.Version11;
            httpWebRequest.Method = "POST";
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
                    throw webex;
                }
            }
            return responseData;
        }

        private static string signPaymentInitData(PaymentInitRequest paymentInit, System.Web.UI.Page page) {
            string data2Sign = paymentInit.getData2Sign();
            string signedData = Digest.Sign(data2Sign, CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PrivateKeyPath", page));
            bool verification = Digest.Verify(CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PublicKeyPath", page), data2Sign, signedData);
            if (verification == false) {
                throw new InvalidOperationException("Signature verification failed!");
            }
            return signedData;
        }

        /// <summary>
        /// Vytorenie objektu Pay transakcie.
        /// </summary>
        public static Transaction CreateTransaction(OrderEntity order, System.Web.UI.Page page) {
            CMS.EvenLog.WritoToEventLog(string.Format("CreatePaymentTransaction, ID:{0}, Order Number:{1}", order.Id, order.OrderNumber), EventLogEntryType.Information);
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
            paymentInit.orderNo = order.Id.ToString();//Referenční číslo objednávky využívané pro párování plateb, které bude uvedeno také na výpisu z banky. Numerická hodnota, maximální délka 10 číslic.
            paymentInit.dttm = DateTime.Now.ToString("yyyyMMddHHmmss");//Datum a čas odeslání požadavku ve formátu YYYYMMDDHHMMSS
            paymentInit.payOperation = "payment";//Typ platební operace. Povolené hodnoty: payment, oneclickPayment 
            paymentInit.payMethod = "card";//Typ implicitní platební metody, která bude nabídnuta zákazníkovi. Povolené hodnoty: card.
            paymentInit.totalAmount = (int)(priceTotalWithWAT * 100);
            paymentInit.currency = currencyCode;//Kód měny. Povolené hodnoty: CZK, EUR, USD, GBP, HUF, PLN, HRK, RON, NOK, SEK 
            paymentInit.closePayment = true;
            paymentInit.returnUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PaymentResultUrl", page);
            paymentInit.returnMethod = "POST";
            paymentInit.customerId = order.AccountId.ToString();//Jednoznačné ID zákazníka, který přiděluje e-shop. Maximální délka 50 znaků. Používá se při uložení karty a jejím následném použití při další návštěvě tohoto e-shopu
            //POZOR: Od verze v1 musí být v košíku nejméně jedna (například "dobití kreditu"), nejvýše dvě položky, například "nákup na mujobchod" a "poštovné"). 
            //Omezení je dáno grafickou úpravou platební brány a v další verzi bude limit položek výrazně posunut.
            paymentInit.cart = new List<PaymentCart>();
            if ((int)(order.CartEntity.KatalogovaCenaCelkem * 100) + (int)(order.CartEntity.DopravneEurosap * 100) == (int)(priceTotalWithWAT * 100)) {
                paymentInit.cart.Add(new PaymentCart(setStringValue("Nákup: euronabycerny.com", 20), 1, (int)(order.CartEntity.KatalogovaCenaCelkem * 100), setStringValue("", 40)));
                paymentInit.cart.Add(new PaymentCart(setStringValue("Poštovné", 20), 1, (int)(order.CartEntity.DopravneEurosap * 100), setStringValue(order.ShipmentName, 40)));
            } else {
                paymentInit.cart.Add(new PaymentCart(setStringValue("Nákup: euronabycerny.com", 20), 1, (int)(priceTotalWithWAT * 100), setStringValue("", 40)));
            }

            paymentInit.description = setStringValue("Nákup na euronabycerny.com", 255);
            paymentInit.merchantData = Digest.EncodeToBase64String(order.Id.ToString());//Libovolná pomocná data, která budou vrácena ve zpětném redirectu z platební brány na stránku obchodníka. Mohou sloužit např pro udržení kontinuity procesu na e-shopu, musí být kódována v BASE64. Maximální délka po zakódování 255 znaků.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using AccountEntity = CMS.Entities.Account;
using OrderEntity = Eurona.DAL.Entities.Order;
using ShpPaymentEntity = SHP.Entities.Classifiers.Payment;
using System.IO;
using Eurona.pay.gp;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Eurona.PAY.GP {
    public class Transaction {

        //MERCHANTNUMBER pole zahrnuto v digest znakový 10 ano 
        //OPERATION pole zahrnuto v digest znakový 20 ano 
        //ORDERNUMBER pole zahrnuto v digest numerický 15 ano 
        //AMOUNT pole zahrnuto v digest numerický 15 ano 
        //CURRENCY pole zahrnuto v digest numerický 3 ano/ne pokud není uvedeno, použije se default z obchodníka nebo banky 
        //DEPOSITFLAG pole zahrnuto v digest numerický 1 ano 
        //MERORDERNUM pole zahrnuto v digest numerický 30 ne URL pole zahrnuto v digest znakový 300 ano 
        //DESCRIPTION pole zahrnuto v digest znakový 255 ne MD pole zahrnuto v digest znakový 255 ano/ne 
        //USERPARAM1 pole zahrnuto v digest znakový 255 ano/ne povinné pro registrační platbu pro funkci Opakovaná platba, jinak nepovinné 
        //FASTPAYID numerický 15 ano/ne pole zahrnuto v digest povinné, pokud je využita služba Fastpay 
        //PAYMETHOD pole zahrnuto v digest znakový 255 ne 
        //DISABLEPAYMETHOD pole zahrnuto v digest znakový 255 ne 
        //PAYMETHODS pole zahrnuto v digest znakový 255 ne 
        //EMAIL pole zahrnuto v digest znakový 255 ne 
        //REFERENCENUMBER pole zahrnuto v digest znakový 20 ne 
        //ADDINFO pole zahrnuto v digest XML schéma 24000 ne 
        //DIGEST znakový 2000 ano LANG pole NENÍ v digest znakový 2 ne
        //LANG pole NENÍ v digest

        public string MerchantNumber { get; set; }
        public string Operation { get; set; }
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string DepositFlag { get; set; }
        public string MerorderNum { get; set; }
        public string Description { get; set; }
        public string Userparam1 { get; set; }
        public string FastPayd { get; set; }
        public string PayMethod { get; set; }
        public string DisablePayMethod { get; set; }
        public string PayMethods { get; set; }
        public string Email { get; set; }
        public string ReferenceNumber { get; set; }
        public string AddInfo { get; set; }
        public string Digest { get; set; }
        public string Lang { get; set; }


        public string MakePayment(System.Web.UI.Page page) {

            string paymentGatewayUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:PaymentGatewayUrl", page);
            string paymentResultUrl = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:PaymentResultUrl", page);
            string paymentCertThumbprint = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:PaymentCertThumbprint", page);
            //string paymentCertPassword = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:PaymentCertPassword", page);

            //Method
            StringBuilder queryParams = new StringBuilder();
            queryParams.Append("?" + GetQueryParameter("MERCHANTNUMBER", this.MerchantNumber));
            queryParams.Append("&" + GetQueryParameter("OPERATION", this.Operation));
            queryParams.Append("&" + GetQueryParameter("ORDERNUMBER", this.TransactionId));
            queryParams.Append("&" + GetQueryParameter("AMOUNT", this.Amount));
            queryParams.Append("&" + GetQueryParameter("CURRENCY", this.Currency));
            queryParams.Append("&" + GetQueryParameter("DEPOSITFLAG", this.DepositFlag));
            queryParams.Append("&" + GetQueryParameter("MERORDERNUM", this.MerorderNum));
            queryParams.Append("&" + GetQueryParameter("URL", paymentResultUrl));

            /*
            queryParams.Append(GetInputHiddenString("DESCRIPTION", this.Description));
            queryParams.Append(GetInputHiddenString("USERPARAM1", this.Userparam1));
            queryParams.Append(GetInputHiddenString("FASTPAYID", this.FastPayd));
            queryParams.Append(GetInputHiddenString("PAYMETHOD", this.PayMethod));
            queryParams.Append(GetInputHiddenString("DISABLEPAYMETHOD", this.DisablePayMethod));
            queryParams.Append(GetInputHiddenString("PAYMETHODS", this.PayMethods));
            queryParams.Append(GetInputHiddenString("EMAIL", this.Email));
            queryParams.Append(GetInputHiddenString("REFERENCENUMBER", this.ReferenceNumber));
            */

            //Pokud obchodník posílá pouze povinné parametry, k výpočtu pole DIGEST slouží hodnota:
            //MERCHANTNUMBER + | + OPERATION + | + ORDERNUMBER + | + AMOUNT + | + CURRENCY + | + DEPOSITFLAG + | + URL

            string signParams = this.MerchantNumber + "|" + this.Operation + "|" + this.TransactionId + "|" +
                this.Amount + "|" + this.Currency + "|" + this.DepositFlag + "|" + this.MerorderNum + "|" + paymentResultUrl;

            //d:\sk\Eurona\eurona\eurona\pay\gp\keys\gpwebpay-pvk.key 
            //Kluc vygenerovany na stranke https://test.portal.gpwebpay.com

            X509Certificate2 cert = CertificateHelper.ToCertificate(paymentCertThumbprint);
            string signed = Eurona.pay.gp.Digest.SignData(signParams, cert);
            queryParams.Append("&" + GetQueryParameter("DIGEST", page.Server.UrlEncode(signed)));

            Uri uri = new Uri(paymentGatewayUrl + queryParams);
            page.Response.Redirect(uri.AbsoluteUri);

            return null;
        }

        private string GetQueryParameter(String name, string value) {
            if (string.IsNullOrEmpty(value)) return null;
            return String.Format("{0}={1}", name, value);
        }

        /// <summary>
        /// Vrati spravny format retazca pre Merchantref
        /// </summary>
        public static string FormatMerchantref(string str) {
            string result = string.Empty;
            foreach (char c in str) {
                if (!Char.IsLetterOrDigit(c)) continue;
                result += c;
            }

            return result;
        }

        /// <summary>
        /// Vytorenie objektu Pay transakcie.
        /// </summary>
        public static Transaction CreateTransaction(OrderEntity order, System.Web.UI.Page page) {
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

            string currency = "203";/*CZK*/
            if (order.CurrencyCode == "EUR") currency = "978";
            if (order.CurrencyCode == "PLN") currency = "616";

            Transaction tran = new Transaction();
            tran.TransactionId = generateTransactionID();
            tran.MerchantNumber = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:MerchantID", page);
            tran.Operation = "CREATE_ORDER";
            tran.Amount = string.Format("{0}", (int)(priceTotalWithWAT * 100));
            tran.Currency = currency;
            tran.MerorderNum = Transaction.FormatMerchantref(order.OrderNumber);
            tran.DepositFlag = "1";

            return tran;
        }

        public static string generateTransactionID() {
            var bytes = new byte[8];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            UInt64 random = BitConverter.ToUInt64(bytes, 0) % 1000000000000000;
            return String.Format("{0:D15}", random);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrderEntity = Eurona.DAL.Entities.Order;

namespace Eurona.pay.gp {
    public partial class PaymentResult : System.Web.UI.Page {
        private OrderEntity order;
        private string transactionId;
        protected void Page_Load(object sender, EventArgs e) {
            string paymentCertThumbprint = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:PaymentResultCertThumbprint", this);

            string merchantNumber = CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:MerchantID", this);
            transactionId = Request.QueryString["ORDERNUMBER"];
            string operation = Request.QueryString["OPERATION"];
            string orderNumber = Request.QueryString["MERORDERNUM"];
            string prCode = Request.QueryString["PRCODE"];
            string srCode = Request.QueryString["SRCODE"];
            string resultText = Request.QueryString["RESULTTEXT"];
            string digest = Request.QueryString["DIGEST"];
            string digest1 = Request.QueryString["DIGEST1"];

            CMS.EvenLog.WritoToEventLog(string.Format("PaymentResult Page, Order Number:{0}", orderNumber), EventLogEntryType.Information);

            try {
                order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadByFilter { OrderNumber = orderNumber });
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);
            }

            if (order == null) {
                CMS.EvenLog.WritoToEventLog(string.Format("PaymentResult Next Atempt to get order, Order Number:{0}", orderNumber), EventLogEntryType.Information);
                try {
                    order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadByFilter { OrderNumber = orderNumber });
                    CMS.EvenLog.WritoToEventLog("PaymentResult Next Atempt to get order - success", EventLogEntryType.Information);
                }catch(Exception ex){
                    CMS.EvenLog.WritoToEventLog("PaymentResult Next Atempt to get order - failed", EventLogEntryType.Error);
                }
            }


            if (order != null) {
                imSuccessCZ.HotSpots[0].NavigateUrl = "~/user/advisor/default.aspx";
                imSuccessSK.HotSpots[0].NavigateUrl = "~/user/advisor/default.aspx";
                imSuccessPL.HotSpots[0].NavigateUrl = "~/user/advisor/default.aspx";
                imErrorCZ.HotSpots[0].NavigateUrl = string.Format("~/user/advisor/newOrder.aspx?id={0}", order.Id);
                imErrorSK.HotSpots[0].NavigateUrl = string.Format("~/user/advisor/newOrder.aspx?id={0}", order.Id);
                imErrorPL.HotSpots[0].NavigateUrl = string.Format("~/user/advisor/newOrder.aspx?id={0}", order.Id);
            }
            imSuccessCZ.Visible = false;
            imSuccessSK.Visible = false;
            imSuccessPL.Visible = false;

            imErrorCZ.Visible = false;
            imErrorSK.Visible = false;
            imErrorPL.Visible = false;
            //?OPERATION=CREATE_ORDER&
            //ORDERNUMBER=20121201895
            //&PRCODE=14&
            //SRCODE=0&
            //RESULTTEXT=Duplicate%20order%20number&
            //DIGEST=rJqXHMbiG4K2YbTpoLTG%2Ba5MGWpEfEerwsXSzo%2BCW8%2BMth2tIuQD%2FtxYzFvngAhi4VVlR37oYruQIBFn1VM7vMTQXxF2I9ouVQjXIgsopGy%2FQ5af%2BsZvuyfVMzdgiC%2FYzwnDgudZlwq0iL4cA2sE0OgIS0R2RelXpLBwZr1WyGIZHRj7QD2NPGHUSX%2BTcR6nLWiier8BDvgwTDbWQemna81Jr5kHXsqhKpsNUrQbqbje0PH5tVn5lDXdiSfqDbO5ZtIYzrvv1KgRoF5g3E%2FZz%2BKnISf8znhh4QzcTnDW9aDZgaRWZdgsyYboTUsy52sO%2BhpSuQHVazN%2BsUwrSo20PA%3D%3D&DIGEST1=tPzN17ZXhqNjdnUpJg%2FRofP05eip7ZupJIYmlQuYP7KVbrJD2ZkMcdS%2B08NI0mc%2BR8O2%2BsiDe0d8vQO%2FtYmd6rVpgpX7FZ5uB9n19pk0wPdkZ7HlHGUUgooVBcLwGfbXGFy0BwlXILCtivvHIFa66g9Z4%2FgaAsFvMq684g6rKf7hpZp5XPgSk1UtLDVDPYjm1ZFfKnMQpNA26ln5lyELOyH985cuHyUOgAV%2BHzk7tB6jBcgrYY944J6%2BGdZrSByEEAs7vv8knsHE2FVEn8YI8jV8j8AwPaoOJKTdpx8QwsWM%2Ff7ACOY68yThdRcEnDmtXdRqNIeQMz09jmJQaM23dA%3D%3D


            X509Certificate2 cert = CertificateHelper.ToCertificate(paymentCertThumbprint, StoreName.Root, StoreLocation.LocalMachine);
            string messsage2Verify = getMessageToVerify(operation, transactionId, orderNumber, prCode, srCode, resultText) + "|" + merchantNumber;
            bool signValid = Eurona.pay.gp.Digest.ValidateDigest(digest1, messsage2Verify, cert);
            if (!signValid) {
                CMS.EvenLog.WritoToEventLog(string.Format("PaymentResult Page, Order Number:{0}, Signature not verified!!", orderNumber), EventLogEntryType.Error);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write("Signature not verified!!");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            } else {
                if (prCode != "0" || srCode != "0") {
                    CMS.EvenLog.WritoToEventLog(string.Format("PaymentResult Page, Order Number:{0}, OnPaymentNotSuccess, prCode:{1}, srCode:{2}", orderNumber, prCode, srCode), EventLogEntryType.Information);
                    OnPaymentNotSuccess(resultText, prCode, srCode);
                } else {
                    CMS.EvenLog.WritoToEventLog(string.Format("PaymentResult Page, Order Number:{0}, OnPaymentSuccess", orderNumber), EventLogEntryType.Information);
                    OnPaymentSuccess();
                }
            }
        }

        private string getMessageToVerify(string operation, string transactionId, string orderNumber, string prCode, string srCode, string resultText) {
            string message = "";
            if (!string.IsNullOrEmpty(operation)) message += operation;
            if (!string.IsNullOrEmpty(transactionId)) {
                if (message.Length != 0) message += "|";
                message += transactionId;
            }
            if (!string.IsNullOrEmpty(orderNumber)) {
                if (message.Length != 0) message += "|";
                message += orderNumber;
            }
            if (!string.IsNullOrEmpty(prCode)) {
                if (message.Length != 0) message += "|";
                message += prCode;
            }
            if (!string.IsNullOrEmpty(srCode)) {
                if (message.Length != 0) message += "|";
                message += srCode;
            }
            if (!string.IsNullOrEmpty(resultText)) {
                if (message.Length != 0) message += "|";
                message += resultText;
            }
            return message;
        }

        private string TVDPlatbaKartou(CMS.Pump.MSSQLStorage tvdStorage, SqlConnection connection, int orderId, decimal suma, string kodMeny, out bool bSuccess) {
            string msg = "";

#if __DEBUG_VERSION_WITHOUTTVD
            bSuccess = true;
            return string.Empty;
#endif
            //===============================================================================
            // ZAPIS PLATBy KARTOU
            //===============================================================================
            //@out_Probehlo OUTPUT    -- 0=chyba, 1=OK
            //,@out_Zprava OUTPUT    -- v případě chyby ev. zpráva k chybě, jinak 'OK'

            //@Cislo_objednavky int,
            //@Castka money,
            //@Kod_meny char(3)  

            SqlParameter probehlo = new SqlParameter("@out_Probehlo", false);
            probehlo.Direction = ParameterDirection.Output;

            SqlParameter zprava = new SqlParameter("@out_Zprava", string.Empty);
            zprava.Direction = ParameterDirection.Output;
            zprava.SqlDbType = SqlDbType.VarChar;
            zprava.Size = 255;

            /*
                @Cislo_objednavky int,
                @Castka money,
                @Kod_meny char(3)             
             */

            try {
                tvdStorage.ExecProc(connection, "esp_www_platbakartou",
                        new SqlParameter("Id_prepoctu", orderId),
                        new SqlParameter("Castka", suma),
                        new SqlParameter("Kod_meny", kodMeny),
                        probehlo, zprava);
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);
                bSuccess = false;
                if (zprava.Value != null) return zprava.Value.ToString();
                return "Eurosap odmítl platbu zpracovat!";
            }


            //===============================================================================
            //Vystupne parametra
            //===============================================================================
            //Probehlo	bit	1=úspěch, 0=chyba		
            //Zprava	varchar(255)	text chyby		
            //id_prepoctu	int	prim. klíč		
            bSuccess = Convert.ToBoolean(probehlo.Value);
            if (zprava != null && zprava.Value != null) {
                msg = string.Format("TVD Platba kartou -> esp_www_platbakartou(Cislo_objednavky:{0}, Suma:{1}, Kod meny:{2}) = {3}", orderId, suma, kodMeny, zprava.Value.ToString());
                CMS.EvenLog.WritoToEventLog(msg, EventLogEntryType.Information);
            } else {
                msg = string.Format("TVD Platba kartou -> esp_www_platbakartou(Cislo_objednavky:{0}, Suma:{1}, Kod meny:{2}) = SUCCESS!", orderId, suma, kodMeny);
                CMS.EvenLog.WritoToEventLog(msg, EventLogEntryType.Information);
            }
            return zprava.Value.ToString();
        }

        private bool MakeTVPlatbaKartou(OrderEntity order) {
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
//                string sql = @"SELECT Suma = ([celkem_k_uhrade]-[castka_dobropis])
//								FROM www_faktury WHERE id_prepoctu=@id_prepoctu";

                string sql = @"SELECT Suma = ([celkem_k_uhrade]-[castka_dobropis]), kod_meny
                FROM www_faktury WHERE id_prepoctu=@id_prepoctu
                GROUP BY celkem_k_uhrade, castka_dobropis, kod_meny";

                DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@id_prepoctu", order.Id));

                decimal suma = order.PriceWVAT;
                string kod_meny = order.CurrencyCode;
                if (dt.Rows.Count != 0) {
                    suma = Convert.ToDecimal(dt.Rows[0]["Suma"]);
                    kod_meny = dt.Rows[0]["kod_meny"].ToString();
                    string msg = string.Format("TVD Platba kartou -> Confirm(id_prepoctu:{0}, Suma:{1}, kod_meny:{2})", order.Id, suma, kod_meny);
                    CMS.EvenLog.WritoToEventLog(msg, EventLogEntryType.Information);
                } else {
                    string msg = string.Format("TVD Platba kartou -> Confirm(id_prepoctu:{0}, Suma:NULL!)", order.Id);
                    CMS.EvenLog.WritoToEventLog(msg, EventLogEntryType.Warning);
                }


                bool bSuccess = false;
                TVDPlatbaKartou(tvdStorage, connection, order.Id, suma, kod_meny, out bSuccess);
                return bSuccess;
            }
        }

        private void OnPaymentSuccess() {
            //Update Order Entity Pay.
            order.PaymentCode = transactionId;
            order.PaydDate = DateTime.Now;
            Storage<OrderEntity>.Update(order);

            //Zdruzene Objednavky
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            List<OrderEntity> listZdruzene = Storage<OrderEntity>.Read(filter);
            if (listZdruzene.Count != 0) {
                foreach (OrderEntity orderSdruzena in listZdruzene) {
                    orderSdruzena.PaymentCode = transactionId;
                    orderSdruzena.PaydDate = DateTime.Now;
                    Storage<OrderEntity>.Update(orderSdruzena);
                }
            }

            //Oznacenie platby v TVD
            MakeTVPlatbaKartou(order);
            //Zdruzene Objednavky
            filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            listZdruzene = Storage<OrderEntity>.Read(filter);
            if (listZdruzene.Count != 0) {
                foreach (OrderEntity orderSdruzena in listZdruzene) {
                    MakeTVPlatbaKartou(orderSdruzena);
                }
            }

            Session["payment_order"] = order.Id; ;

            string locale = Security.Account.Locale;//System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
           if (locale == "pl") {
               imSuccessPL.Visible = true;
           } else if (locale == "sk") {
               imSuccessSK.Visible = true;
           } else {
               imSuccessCZ.Visible = true;
           }

        }
        private void OnPaymentNotSuccess(string resultText, string prCode, string srCode) {
            string locale = Security.Account.Locale;//System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            if (locale == "pl") {
                imErrorPL.Visible = true;
            } else if (locale == "sk") {
                imErrorSK.Visible = true;
            } else {
                imErrorCZ.Visible = true;
            }
            /*
            //50 - Držitel karty zrušil platbu
            if (prCode == "50") { 
                lblTransactionResultMesage.Text = Resources.EShopStrings.Pay_TransactionWasNotSuccessfully;
                return;
            }
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(resultText + "<br/>" + "PR_CODE:" + prCode + "<br/>" + "SR_CODE:" + srCode);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();*/
        }

        protected void btnBackToOrder_Click(object sender, EventArgs e) {
            Response.Redirect(string.Format("~/user/advisor/newOrder.aspx?id={0}", order.Id));
        }

        protected void btnBackToMojeKancelar_Click(object sender, EventArgs e) {
            Response.Redirect(string.Format("~/user/advisor/default.aspx"));
        }

    }
}
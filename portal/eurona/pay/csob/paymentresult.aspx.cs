using Eurona.pay.csob.utils;
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

namespace Eurona.pay.csob {
    public partial class PaymentResult : System.Web.UI.Page {
        private OrderEntity order;
        private PaymentProcessResponse paymentResponse;
        protected void Page_Load(object sender, EventArgs e) {
            CMS.EvenLog.WritoToEventLog(string.Format("Payment Gateway redirect to Eurona PaymentResult"), EventLogEntryType.Information);

            paymentResponse = parseRequestData();
            string data2Verify = paymentResponse.getData2VerifyResponse();
            bool verification = Digest.Verify(CMS.Utilities.ConfigUtilities.ConfigValue("SHP:PAY:CSOB:PublicKeyPath", this), data2Verify, paymentResponse.signature);
            //if (verification == false) {
            //    throw new InvalidOperationException("PaymentResultResponse verification failed!");
            //}

            string merchantData = Digest.DefoceFromBase64String(paymentResponse.merchantData);
            CMS.EvenLog.WritoToEventLog(string.Format("PaymentResult OrderId={0}", merchantData), EventLogEntryType.Information);
            int orderId = Convert.ToInt32(merchantData);

            try {
                order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);
            }

            if (order == null) {
                CMS.EvenLog.WritoToEventLog(string.Format("PaymentResult Next Atempt to get order, Order Id:{0}", orderId), EventLogEntryType.Information);
                try {
                    order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
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

            if (paymentResponse.resultCode == 0 && (paymentResponse.paymentStatus == 4 || paymentResponse.paymentStatus == 7 || paymentResponse.paymentStatus == 8 )) {
                CMS.EvenLog.WritoToEventLog(string.Format("PaymentResult Page, Order Id:{0}, OnPaymentSuccess", orderId), EventLogEntryType.Information);
                OnPaymentSuccess(paymentResponse);
            } else {
                CMS.EvenLog.WritoToEventLog(string.Format("PaymentResult Page, Order id:{0}, OnPaymentNotSuccess, resultCode:{1}, paymentStatus:{2}", orderId, paymentResponse.resultCode, paymentResponse.paymentStatus), EventLogEntryType.Information);
                OnPaymentNotSuccess(paymentResponse.resultMessage, paymentResponse.resultCode, paymentResponse.paymentStatus);
            }
        }

        private PaymentProcessResponse parseRequestData() {
            /*
                payId=4db540c347414DK&dttm=20181113100229&resultCode=0&resultMessage=OK&paymentStatus=3&signature=G0g1zBCzckR394GFf%2Flps2TaK%2BcLR%2FyGwHzq201tls6MF3BJJFpa7i1ZX95uGXx3Nzn4nasRNjQajW5ScejGmiVyWctzboOGnuD2DxSTFFDKJdKIAWZKzKB5y6C9ht3rLSfRz31Pby8VIA76JQbneItPZAsDrs0Oz3H3IpgEWj1FyS2G2paq2vcEA2BbmKLXTc%2BxyvLRqtbhugtaeT4GNeRLtxcLCX8caIrSnd2s%2BvknK88LnKl90CFbyoSgJvDnHwCIGhfgM7T6LlUstl7lu6F%2BS06BaAko5DP5WviSffRq10jCttcFrxS%2Fffd8oK8sbQLZuhxbQLHkes2SnEkW6A%3D%3D&merchantData=NjMxNzYz	          
             */
            PaymentProcessResponse response = new PaymentProcessResponse();
            if (!string.IsNullOrEmpty(Request["payId"])) {
                //GET params
                response.payId = Request["payId"];
                response.dttm = Request["dttm"];
                response.resultCode = Convert.ToInt32(Request["resultCode"]);
                response.resultMessage = Request["resultMessage"];
                response.paymentStatus = Convert.ToInt32(Request["paymentStatus"]);
                response.merchantData = Request["merchantData"];
                response.signature = Request["signature"];
            } else if (!string.IsNullOrEmpty(Request.Form["payId"])) {
                //POST params
                response.payId = Request.Form["payId"];
                response.dttm = Request.Form["dttm"];
                response.resultCode = Convert.ToInt32(Request.Form["resultCode"]);
                response.resultMessage = Request.Form["resultMessage"];
                response.paymentStatus = Convert.ToInt32(Request.Form["paymentStatus"]);
                response.merchantData = Request.Form["merchantData"];
                response.signature = Request.Form["signature"];
            } else {
                throw new InvalidOperationException("Invalid paymnet response type from pay gateway!");
            }
            return response;
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

        private bool MakeTVDPlatbaKartou(OrderEntity order) {
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {

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

        private void OnPaymentSuccess(PaymentProcessResponse paymentResponse) {
            //Update Order Entity Pay.
            order.PaymentCode = paymentResponse.payId;
            order.PaydDate = DateTime.Now;
            Storage<OrderEntity>.Update(order);

            //Zdruzene Objednavky
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            List<OrderEntity> listZdruzene = Storage<OrderEntity>.Read(filter);
            if (listZdruzene.Count != 0) {
                foreach (OrderEntity orderSdruzena in listZdruzene) {
                    orderSdruzena.PaymentCode = paymentResponse.payId;
                    orderSdruzena.PaydDate = DateTime.Now;
                    Storage<OrderEntity>.Update(orderSdruzena);
                }
            }

            //Oznacenie platby v TVD
            MakeTVDPlatbaKartou(order);
            //Zdruzene Objednavky
            filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            listZdruzene = Storage<OrderEntity>.Read(filter);
            if (listZdruzene.Count != 0) {
                foreach (OrderEntity orderSdruzena in listZdruzene) {
                    MakeTVDPlatbaKartou(orderSdruzena);
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
        private void OnPaymentNotSuccess(string resultText, int resultCode, int paymentStatus) {
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
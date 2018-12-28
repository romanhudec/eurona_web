using System;
using OrderEntity = Eurona.DAL.Entities.Order;
using ShpCultureUtilities = SHP.Utilities.CultureUtilities;
using System.Data;
using Eurona.Controls;
using System.Web.UI;
using SettingsEntity = Eurona.Common.DAL.Entities.Settings;
using Eurona.Common.DAL.Entities;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Eurona.pay.csob;

namespace Eurona.EShop {
    public partial class PayOrderControl : Eurona.Common.Controls.UserControl {
        private OrderEntity order = null;
        private SettingsEntity settingsPlatbaKartouLimit = null;

        protected void Page_Load(object sender, EventArgs e) {
            btnPay.Attributes["onclick"] = "this.disabled=true;this.value='Please wait...';" + Page.ClientScript.GetPostBackEventReference(btnPay, null).ToString();
            this.settingsPlatbaKartouLimit = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "ESHOP_PLATBAKARTOU_LIMIT" });

            if (Eurona.Common.DAL.Entities.Settings.IsPlatbaKartouPovolena()) {
                if (this.settingsPlatbaKartouLimit != null) {
                    int limit = Eurona.Common.DAL.Entities.Settings.GetPlatbaKartouLimit();
                    if (limit > 0) {
                        if (Eurona.Common.DAL.Entities.Settings.IsPaymentAfterLimit(this.OrderEntity)) {
                            payLimit.Visible = false;
                            this.btnPay.Enabled = false;
                            this.btnPay.Text = "";
                            this.btnPay.CssClass = "button-uhrada-kartou-disabled";
                        } else {
                            payLimit.Visible = true;
                            DateTime dateFrom = OrderEntity.OrderDate;
                            DateTime dateTo = dateFrom.AddMinutes(limit);
                            int seconds = (int)(dateTo - DateTime.Now).TotalSeconds;
                            RegisterStartupCountDownScript("cnt_container", seconds);
                            this.btnPay.Enabled = true;
                            this.btnPay.CssClass = "button-uhrada-kartou";
                        }
                    }
                }
            }

            ////Zdruzene Objednavky
            //OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            //filter.ParentId = this.OrderEntity.Id;
            //List<OrderEntity> listZdruzene = Storage<OrderEntity>.Read(filter);
            //if (listZdruzene.Count != 0) {
            //    if (Settings.IsPlatbaKartou4ZdruzeneObjednavkyPovolena()) {
            //        this.lblPlatbaKarout4ZdruzeneObjednavkyMessage.Visible = false;
            //        this.lblCelkovaCenaPridruzeni.Visible = true;
            //        this.btnPay.Enabled = true;
            //        this.btnPay.CssClass = "button-uhrada-kartou";

            //        if (listZdruzene.Count != 0) {
            //            decimal celkovaSuma = 0.0m;
            //            foreach (OrderEntity order in listZdruzene) {
            //                celkovaSuma += order.PriceWVAT;
            //            }
            //            celkovaSuma = this.OrderEntity.PriceWVAT + celkovaSuma;
            //            this.lblCelkovaCenaPridruzeni.Text = string.Format(Resources.EShopStrings.OrderFinish_SdruzeneObjednavkyCenaCelkem_Format,
            //                ShpCultureUtilities.CurrencyInfo.ToString(celkovaSuma, this.Session));
            //        }
            //    } else {
            //        this.lblPlatbaKarout4ZdruzeneObjednavkyMessage.Visible = true;
            //        this.lblCelkovaCenaPridruzeni.Visible = false;
            //        this.btnPay.Enabled = false;
            //        this.btnPay.Text = "";
            //        this.btnPay.CssClass = "button-uhrada-kartou-disabled";
            //    }
            //}

        }

        public Literal HeaderControl {
            get { return this.lblHeader; }
        }

        //public Literal PlatbaKarout4ZdruzeneObjednavkyMessageControl {
        //    get { return this.lblPlatbaKarout4ZdruzeneObjednavkyMessage; }
        //}

        private void RegisterStartupCountDownScript(string containerId, int seconds) {
            if (!Eurona.Common.DAL.Entities.Settings.IsPlatbaKartouPovolena())
                return;

            ClientScriptManager cs = this.Page.ClientScript;
            Type cstype = this.Page.GetType();

            if (!cs.IsStartupScriptRegistered(cstype, "CountDownManager")) {
                string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
                string countDownConstructors = string.Format("cdm.add(new CountDown('{0}', {1}, '{2}', {3}));\n", containerId, seconds, locale, "onCountDownEndFunction");

                string js = @"<script type='text/javascript'> 
								var cdm = new CountDownManager();
								var loadBase = window.onload;
								window.onload = function() { " +
                                countDownConstructors
                                + @"cdm.start();
								    if (loadBase != null) loadBase();
								}</script>";
                cs.RegisterStartupScript(cstype, "CountDownManager", js);
            }
        }

        public int OrderId {
            get {
                object o = ViewState["OrderId"];
                return o != null ? (int)Convert.ToInt32(o) : 0;
            }
            set { ViewState["OrderId"] = value; }
        }

        public OrderEntity OrderEntity {
            get {
                if (this.order != null) return this.order;
                this.order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = this.OrderId });
                return this.order;
            }
        }

        protected void OnUhradaDobirkou(object sender, EventArgs e) {
            CartOrderHelper.UhradaDobirkou(this.OrderEntity);
            Response.Redirect(String.Format("~/user/advisor/orderFinish.aspx?id={0}", this.OrderId));
        }

        protected void OnPayNow(object sender, EventArgs e) {
            if (!Eurona.Common.DAL.Entities.Settings.IsPlatbaKartouPovolena()) {
                string jsAlert = string.Format("alert('{0}');", "Omlouváme se, ale platba kartou je dočasne nedostupná!");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "payTransactionResult", jsAlert, true);
                return;
            }

            int limit = Eurona.Common.DAL.Entities.Settings.GetPlatbaKartouLimit();
            if (limit > 0) {
                if (Eurona.Common.DAL.Entities.Settings.IsPaymentAfterLimit(this.OrderEntity)) {
                    string jsAlert = string.Format("alert('{0}');", "Omlouváme se, ale na platbu kartou vypršel časový limit!");
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "payTransactionResult", jsAlert, true);
                    return;
                }
            }
            string var_symbol_eurosap = "";
            DataTable dt = CartOrderHelper.GetTVDFaktura(this.OrderEntity);
            if (dt.Rows.Count == 0) {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "payTransactionResult", "alert('Pro danou objednávku se nenašla faktura!');", true);
                return;
            }
            if( dt.Rows[0]["var_symbol_eurosap"] == DBNull.Value ){
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "payTransactionResult", "alert('Daná objednávka nemá vyplnen variabilní symbol!');", true);
                return;
            }
            var_symbol_eurosap = Convert.ToInt32(dt.Rows[0]["var_symbol_eurosap"]).ToString();

            Eurona.PAY.CSOB.Transaction payTransaction = Eurona.PAY.CSOB.Transaction.CreateTransaction(order, var_symbol_eurosap, this.Page);
            PaymentInitResponse paymentInitResponse = payTransaction.InitPayment(this.Page);
            if (paymentInitResponse != null && paymentInitResponse.resultCode == 0) {
                payTransaction.ProcessPayment(this.Page, paymentInitResponse);
            } else {
                Response.Write(paymentInitResponse.resultMessage);
            }
            /*
            Eurona.PAY.GP.Transaction payTransaction = Eurona.PAY.GP.Transaction.CreateTransaction(order, this.Page);
            payTransaction.MakePayment(this.Page);
             **/
            /*#else
                                    this.OrderEntity.PriceWVAT = Convert.ToDecimal(dt.Rows[0]["celkem_k_uhrade"]);
                        this.OrderEntity.CurrencyCode = Convert.ToString(dt.Rows[0]["kod_meny"]);

                        Transaction tran = Transaction.CreateTransaction(this.OrderEntity, this.Page);
                        string result = tran.PostTransaction(this.Page);
             
                        this.payOrderInput.Visible = false;
                        this.payOrderResult.InnerHtml = result;

                        string js = string.Empty;
                        if (string.IsNullOrEmpty(this.ReturnUrl)) {
                            //Alert s informaciou o pridani do nakupneho kosika
                            js = string.Format("alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();", result, this.Request.RawUrl.Contains("?") ? "&" : "?");
                        } else {
                            //Alert s informaciou o pridani do nakupneho kosika
                            js = string.Format("alert('{0}');window.location.href='{1}';", result, this.ReturnUrl);
                        }
                        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "payTransactionResult", js, true);     
            #endif*/
        }
    }
}
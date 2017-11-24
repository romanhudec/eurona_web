using System;
using OrderEntity = Eurona.DAL.Entities.Order;
using System.Data;
using Eurona.Controls;

namespace Eurona.User.Anonymous {
    public partial class PayOrderControl : Eurona.Common.Controls.UserControl {
        private OrderEntity order = null;
        protected void Page_Load(object sender, EventArgs e) {
            btnPay.Attributes["onclick"] = "this.disabled=true;this.value='Please wait...';" + Page.ClientScript.GetPostBackEventReference(btnPay, null).ToString();
            if (Security.Account.Locale != "cs") {
                this.btnPay.Enabled = false;
                this.btnPay.Text = "";
                this.btnPay.CssClass = "button-uhrada-kartou-disabled";
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
            Response.Redirect(String.Format("~/user/anonymous/finish.aspx?id={0}", this.OrderId));
        }

        protected void OnPayNow(object sender, EventArgs e) {
            if (!Eurona.Common.DAL.Entities.Settings.IsPlatbaKartouPovolena()) {
                string jsAlert = string.Format("alert('{0}');", "Omlouváme se, ale platba kartou je dočasne nedostupná!");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "payTransactionResult", jsAlert, true);
                return;
            }

            DataTable dt = CartOrderHelper.GetTVDFaktura(this.OrderEntity);
            if (dt.Rows.Count == 0) {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "payTransactionResult", "alert('Pro danou objednávku se nenašla faktura!');", true);
                return;
            }

            this.OrderEntity.PriceWVAT = Convert.ToDecimal(dt.Rows[0]["celkem_k_uhrade"]);
            this.OrderEntity.CurrencyCode = Convert.ToString(dt.Rows[0]["kod_meny"]);
            Eurona.PAY.GP.Transaction payTransaction = Eurona.PAY.GP.Transaction.CreateTransaction(this.OrderEntity, this.Page);
            payTransaction.MakePayment(this.Page);
        }
    }
}
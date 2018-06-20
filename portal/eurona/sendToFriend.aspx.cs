using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using System.Text;
using Eurona.Controls;

namespace Eurona {
    public partial class sendToFriend : WebPage {
        private int productId;
        protected void Page_Load(object sender, EventArgs e) {
            return;
            if (string.IsNullOrEmpty(Request["productId"]))
                return;

            this.productId = Convert.ToInt32(Request["productId"]);

            #region Disable Close tournament button
            StringBuilder sb = new StringBuilder();
            //change button text and disable it
            sb.AppendFormat("this.value = '{0}';", "Odesílá se ...");
            sb.Append("this.disabled = true;");
            sb.Append(Page.ClientScript.GetPostBackEventReference(this.btnOdeslat, null) + ";");
            //sb.Append( "javascript:window.close();" );
            string button_onclick_js = sb.ToString();
            btnOdeslat.Attributes.Add("onclick", button_onclick_js);
            #endregion

            if (!Security.IsLogged(false))
                return;

            this.txtEmailFrom.Text = Security.Account.Email;
            Eurona.Common.DAL.Entities.Organization org = Storage<Eurona.Common.DAL.Entities.Organization>.ReadFirst(new Eurona.Common.DAL.Entities.Organization.ReadByAccountId { AccountId = Security.Account.Id });
            if (org != null) {
                this.txtFrom.Text = org.Name;
            }
        }

        private ProductEntity product = null;
        public ProductEntity Product {
            get {
                if (this.product == null)
                    this.product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = this.productId });
                return this.product;
            }
        }

        protected void OnSendEmail(object sender, EventArgs e) {
            return;
            if (string.IsNullOrEmpty(this.txtEmailFrom.Text)) return;
            if (string.IsNullOrEmpty(this.txtFrom.Text)) return;
            if (string.IsNullOrEmpty(this.txtEmailTo.Text)) return;

            //Zaevidovanie bonusovych kreditov
            if (Security.IsLogged(false)) {
                string kod = string.Format("{0};{1}", this.Product.Id, this.txtEmailTo.Text);
                BonusovyKreditUzivateleHelper.ZaevidujKredit(Security.Account.Id, DAL.Entities.Classifiers.BonusovyKreditTyp.ProduktEmailPoslatPriteli, null, kod);
            }

            string root = Utilities.Root(this.Request);
            if (root.EndsWith("/")) root = root.Remove(root.Length - 1, 1);

            CMS.EmailNotification email = new CMS.EmailNotification();
            email.To = this.txtEmailTo.Text;
            email.Subject = "Informace od Vašeho známého.";
            email.Message = string.Format("Ahoj,<br/>rád/a bych te upozornila na tento <a href='{0}'>produkt</a><br/><br/>S pozdravem<br/>{1}",
                    root + Page.ResolveUrl(this.Product.Alias), this.txtFrom.Text);
            email.Notify(true, this.txtEmailFrom.Text);

            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "<script type='text/JavaScript'>window.close();</script>");
        }
    }
}
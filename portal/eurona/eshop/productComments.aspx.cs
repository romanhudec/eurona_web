using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.IO;
using SettingsEntity = Eurona.Common.DAL.Entities.Settings;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using AccountEntity = Eurona.DAL.Entities.Account;

namespace Eurona.EShop {
    public partial class ProductCommentsPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            this.productCommentFormControl.OnCommentCreated += OnCommentCreated;
            if (!string.IsNullOrEmpty(this.Request["id"])) {
                this.productCommentsControl.ProductId = Convert.ToInt32(this.Request["id"]);
                this.productCommentsControl.CommentFormID = this.productCommentFormControl.ClientID;
                this.productCommentFormControl.ProductId = Convert.ToInt32(this.Request["id"]);
                this.productCommentFormControl.RedirectUrl = this.Request.RawUrl;

                //Nastavenie spravnej navratovej url adresy 
                // je to koli tomu, ze ak je galeria zobrazena z filtrovaneho zoznamu,
                // spet sa musi vratit na filtrovany zoznam, nie na vsetky galerie.
                if (!string.IsNullOrEmpty(this.productCommentFormControl.ReturnUrl))
                    this.returnUrl.HRef = this.productCommentFormControl.ReturnUrl;
            }
        }

        void OnCommentCreated(SHP.Entities.ProductComment comment) {
            //throw new NotImplementedException();
            //send email
            SendAdminTrackingEmail(comment, this.productCommentsControl.ProductId);
        }

        private void SendAdminTrackingEmail(SHP.Entities.ProductComment comment, int productId) {
            ProductEntity product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = productId });
            if (product == null) return;

            SettingsEntity emailSettings = Storage<SettingsEntity>.ReadFirst(new SettingsEntity.ReadByCode { Code = "EMAIL_KOMENTAR_PRODUKTU" });
            if (emailSettings == null) return;
            if (String.IsNullOrEmpty(emailSettings.Value)) return;

            AccountEntity senderAccount = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = comment.AccountId });
            string accountName = comment.AccountName == null ? senderAccount.Login : comment.AccountName;

            CMS.EmailNotification email = new CMS.EmailNotification();
            email.To = emailSettings.Value;
            email.Subject = string.Format("Nový vložený komentář k produktu : {0}", product.Name);
            email.Message = string.Format(@"Dobry den<br/>uživatel {0} vložil komentář k produktu '{1}':<br/><br/>{2}<br/>{3}<br/><br/>Automaticky genrovaný email",
                    accountName, product.Name, comment.Title, comment.Content);
            email.Notify(true);
        }
    }
}

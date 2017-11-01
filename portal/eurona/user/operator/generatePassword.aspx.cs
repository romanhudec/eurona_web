using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AccountEntity = Eurona.DAL.Entities.Account;
using CMS;
using CMS.Utilities;
using System.Text;

namespace Eurona.User.Operator {
    public partial class GeneratePasswordPage : WebPage {
        private CMS.Controls.CmsControl cmsCtrl;
        protected void Page_Load(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Request["id"])) return;
            this.account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = Convert.ToInt32(Request["id"]) });

            this.capcha.ErrorMessage = CMS.Resources.Controls.ForgotPasswordControl_Capcha_ErrorMessage;
            this.capcha.CaptchaTextBoxLabel = CMS.Resources.Controls.ForgotPasswordControl_Capcha_TextBoxLabel;
            this.capcha.Width = Unit.Percentage(100);
            this.capcha.EnableRefreshImage = true;
            this.capcha.CaptchaLinkButtonText = CMS.Resources.Controls.ForgotPasswordControl_Capcha_LinkButtonText;

            #region Disable Send button and js validation
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + this.btnGenerovat.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");

            //change button text and disable it
            sb.AppendFormat("this.value = '{0}...';", this.btnGenerovat.Text);
            sb.Append("this.disabled = true;");
            sb.Append(Page.ClientScript.GetPostBackEventReference(this.btnGenerovat, null) + ";");
            sb.Append("return true;");
            string submit_button_onclick_js = sb.ToString();
            this.btnGenerovat.Attributes.Add("onclick", submit_button_onclick_js);
            #endregion
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            this.cmsCtrl = new CMS.Controls.CmsControl();
            this.Controls.Add(this.cmsCtrl);

        }

        private AccountEntity account = null;
        public AccountEntity AccountEntity {
            get {
                return account;
            }
        }

        protected void OnCancel(object sender, EventArgs e) {
            Response.Redirect(this.cmsCtrl.ReturnUrl);
        }
        protected void OnGenerate(object sender, EventArgs e) {
            if (this.AccountEntity == null) return;

            this.capcha.Validate();
            if (!this.capcha.IsValid) return;

            string newPwd = capcha.CaptchaImage.Text;
            this.AccountEntity.Password = Cryptographer.MD5Hash(newPwd);
            /*    
            //PasswordPolicy
                this.AccountEntity.MustChangeAccountPassword = true;
             * */
            Storage<AccountEntity>.Update(this.AccountEntity);

            EmailNotification email = new EmailNotification();
            email.Subject = "Nové heslo pro EURONA.cz";
            email.Message = string.Format("Dobrý den<br/><br/>právě Vám bylo vygenerováno neové heslo pro vstup na portál EURONA.cz.<br/>Vaše přihlašovací jméno je : {0}<br/>Vaše nové heslo : {1}<br/><br/>S pozdravem<br/>{2}",
                    this.AccountEntity.Login, newPwd,
                    CMS.Utilities.ConfigUtilities.ConfigValue("SHP:SMTP:FromDisplay"));
            email.To = this.AccountEntity.Email;
            email.Notify(true);

            Response.Redirect(this.cmsCtrl.ReturnUrl);
        }
    }
}

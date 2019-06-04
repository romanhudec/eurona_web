using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using CMS.Utilities;
using CMS.Controls;
using AccountEntity = Eurona.DAL.Entities.Account;

namespace Eurona.Controls {
    public class ForgotPasswordControl : CmsControl {
        public Label lblValidatorText = null;
        public TextBox txtEmail = null;
        private Telerik.Web.UI.RadCaptcha capcha = null;
        public Button btnSend = null;
        private Button btnCancel = null;

        public string Salt { get; set; }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            this.lblValidatorText = new Label();
            this.lblValidatorText.ID = "lblValidatorText";
            this.lblValidatorText.Width = Unit.Percentage(100);

            this.txtEmail = new TextBox();
            this.txtEmail.ID = "txtEmail";
            this.txtEmail.Width = Unit.Percentage(100);

            if (!String.IsNullOrEmpty(Request["email"])) {
                this.txtEmail.Text = Request["email"];
            }

            this.capcha = new Telerik.Web.UI.RadCaptcha();
            this.capcha.ErrorMessage = global::CMS.Resources.Controls.ForgotPasswordControl_Capcha_ErrorMessage;
            this.capcha.CaptchaTextBoxLabel = global::CMS.Resources.Controls.ForgotPasswordControl_Capcha_TextBoxLabel;
            this.capcha.Width = Unit.Percentage(100);
            this.capcha.EnableRefreshImage = true;
            this.capcha.CaptchaLinkButtonText = global::CMS.Resources.Controls.ForgotPasswordControl_Capcha_LinkButtonText;
            //this.capcha.CaptchaTextBoxLabelCssClass 

            this.btnSend = new Button();
            this.btnSend.ID = "btnChange";
            this.btnSend.Text = global::CMS.Resources.Controls.ForgotPasswordControl_Send_ButtonText;
            this.btnSend.Click += (s, e) => {
                this.capcha.Validate();
                if (!this.capcha.IsValid) return;

                AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadByLogin { Login = this.txtEmail.Text });
                if (account == null)
                    account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadByEmail { Email = this.txtEmail.Text });
                if (account == null) {
                    string script = string.Format("alert('{0}');", global::CMS.Resources.Controls.ForgotPasswordControl_LoginNotExists);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ForgotPasswordControl_error", script, true);
                    return;
                }

                string code = string.Format("{0}|{1}|{2}", this.txtEmail.Text, account.Id, Utilities.GetUserIP(this.Page.Request));
                code = CMS.Utilities.Cryptographer.Encrypt(code);
                string url = Utilities.Root(this.Page.Request) + "user/changePassword.aspx?code=" + code;
                
                //string newPwd = capcha.CaptchaImage.Text;
                account.Password = Cryptographer.MD5Hash(capcha.CaptchaImage.Text, Salt);
                Storage<AccountEntity>.Update(account);

                CMS.EmailNotification email = new CMS.EmailNotification();
                email.Subject = global::CMS.Resources.Controls.ForgotPasswordControl_Email_ForgotPassword_Subject;
                email.Message = string.Format(Resources.Strings.ForgotPasswordControl_Email_ForgotPassword_Message, account.Login, url);
                email.To = account.Email;
                email.Notify(true);
                
                string rurl = this.ReturnUrl;
                /*
                if (string.IsNullOrEmpty(rurl)) return;
                Response.Redirect(Page.ResolveUrl(rurl));
                 * */

                this.Visible = false;

                string js2 = string.Format("blockUIAlert('Zapomenuté heslo', '{0}');", Resources.Strings.ForgotPasswordControl_Finish);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ForgotPassword2", js2, true);
            };

            this.btnCancel = new Button();
            this.btnCancel.ID = "btnCancel";
            this.btnCancel.CausesValidation = false;
            this.btnCancel.Text = global::CMS.Resources.Controls.CancelButton_Text;
            this.btnCancel.Click += (s, e) => {
                string url = this.ReturnUrl;
                if (string.IsNullOrEmpty(url)) return;
                Response.Redirect(Page.ResolveUrl(url));
            };

            Table table = new Table();
            table.Width = this.Width;
            table.Height = this.Height;
            table.Rows.Add(CreateValidationTextTableRow(this.lblValidatorText, false));
            table.Rows.Add(CreateTableRow(Resources.Strings.ForgotPasswordControl_Email_Label, this.txtEmail, true));
            TableRow row = new TableRow();
            row.Cells.Add(new TableCell());
            TableCell cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(this.capcha); row.Cells.Add(cell);
            table.Rows.Add(row);

            //Save Cancel Buttons
            row = new TableRow();
            cell = new TableCell();
            cell.ColumnSpan = 2;
            cell.Controls.Add(this.btnSend);
            cell.Controls.Add(this.btnCancel);
            row.Cells.Add(cell);
            table.Rows.Add(row);

            this.Controls.Add(table);

        }

        private TableRow CreateTableRow(string labelText, Control control, bool required) {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = required ? "form_label_required" : "form_label";
            cell.Text = labelText;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(control);
            if (required) cell.Controls.Add(base.CreateRequiredFieldValidatorControl(control.ID));
            row.Cells.Add(cell);

            return row;
        }

        private TableRow CreateValidationTextTableRow(Label control, bool visible) {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = "ms-formvalidation";
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.CssClass = "ms-formvalidation";
            cell.Controls.Add(control);           
            row.Cells.Add(cell);

            if (!visible) control.Style.Add("display", "none");
            else control.Style.Add("display", "block");

            return row;
        }
    }
}

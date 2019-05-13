using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccountEntity = Eurona.DAL.Entities.Account;
using RoleEntity = Eurona.DAL.Entities.Role;
using System.Web.UI.WebControls;
using CMS.Controls;
using Eurona.Common.DAL.Entities;
using CMS;
using CMS.Utilities;
using Eurona.Controls.UserManagement;


namespace Eurona.Controls {
    public class AdminAccountControl : CmsControl {
        private TextBox tbLogin;
        private TextBox tbEmail;
        private CheckBox cbEnabled;
        private CheckBox cbVerified;
        private Telerik.Web.UI.RadCaptcha capcha = null;
        //private TextBox tbVerifyCode;
        private CheckBoxList clbRoles;

        private CheckBox cbSingleUserCookieLinkEnabled;
        private CheckBox cbZrusitOvereniEmailem;
        private Button btnSave;
        private Button btnChangePassword;
        private Button btnCancel;

        private bool isNew = false;
        private AccountEntity accountEntity = null;

        public string ChangePasswordUrlFormat { get; set; }
        public string GeneratePasswordUrlFormat { get; set; }
        public string UserDetailUrlFormat { get; set; }
        public string RegisterUserUrlFormat { get; set; }

        public bool UseCapcha {
            get {
                return ViewState["UseCapcha"] != null ? Convert.ToBoolean(ViewState["UseCapcha"]) : false;
            }
            set { ViewState["UseCapcha"] = value; }
        }

        public AdminAccountControl() {
            this.UseCapcha = true;
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            isNew = Request["Id"] == null;
            if (isNew) accountEntity = new AccountEntity();
            else {
                int accountId = Convert.ToInt32(Request["id"]);
                accountEntity = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = accountId });
            }

            Table table = new Table();
            table.CssClass = this.CssClass;
            table.Width = this.Width;

            TableRow trLogin = new TableRow();
            trLogin.Cells.Add(new TableCell {
                Text = global::CMS.Resources.Controls.AdminAccountControl_LabelLogin,
                CssClass = "form_label_required"
            });
            trLogin.Cells.Add(CreateLoginInput());
            table.Rows.Add(trLogin);

            TableRow trEmail = new TableRow();
            trEmail.Cells.Add(new TableCell {
                Text = global::CMS.Resources.Controls.AdminAccountControl_LabelEmail,
                CssClass = "form_label_required"
            });
            trEmail.Cells.Add(CreateEmailInput());
            table.Rows.Add(trEmail);

            TableRow trEnabled = new TableRow();
            trEnabled.Cells.Add(new TableCell {
                Text = global::CMS.Resources.Controls.AdminAccountControl_LabelEnabled,
                CssClass = "form_label_required"
            });
            trEnabled.Cells.Add(CreateEnabledCheckBox());
            table.Rows.Add(trEnabled);

            TableRow trVerified = new TableRow();
            trVerified.Cells.Add(new TableCell {
                Text = global::CMS.Resources.Controls.AdminAccountControl_LabelVerified,
                CssClass = "form_label_required"
            });
            trVerified.Cells.Add(CreateVerifiedCheckBox());
            table.Rows.Add(trVerified);

            TableRow trSingleUserCookesEnabled = new TableRow();
            trSingleUserCookesEnabled.Cells.Add(new TableCell {
                Text = Resources.Strings.AdminAccountControl_LabelSingleUserCookesEnabled,
                CssClass = "form_label_required"
            });
            trSingleUserCookesEnabled.Cells.Add(CreateSingleUserCookieLinkEnabledCheckBox());
            table.Rows.Add(trSingleUserCookesEnabled);

            //TableRow trVerifyCode = new TableRow();
            //trVerifyCode.Cells.Add( new TableCell
            //{
            //    Text = global::CMS.Resources.Controls.AdminAccountControl_LabelVerifyCode,
            //    CssClass = "form_label_required"
            //} );
            //trVerifyCode.Cells.Add( CreateVerifyCodeInput() );
            //table.Rows.Add( trVerifyCode );

            TableRow trRoles = new TableRow();
            trRoles.Cells.Add(new TableCell {
                Text = global::CMS.Resources.Controls.AdminAccountControl_LabelRoles,
                CssClass = "form_label_required"
            });
            trRoles.Cells.Add(CreateRolesCheckListBox());
            table.Rows.Add(trRoles);

            if (accountEntity.EmailVerified.HasValue) {
                TableRow trZrusitOvereniEmailem = new TableRow();
                trZrusitOvereniEmailem.Cells.Add(new TableCell {
                    Text = Resources.Strings.AdminAccountControl_LabelZrusitOvereniEmailem,
                    CssClass = "form_label_required"
                });
                trZrusitOvereniEmailem.Cells.Add(CreateZrusitOvereniEmailemCheckBox());
                table.Rows.Add(trZrusitOvereniEmailem);
            }


            this.capcha = new Telerik.Web.UI.RadCaptcha();
            this.capcha.ErrorMessage = CMS.Resources.Controls.ForgotPasswordControl_Capcha_ErrorMessage;
            this.capcha.CaptchaTextBoxLabel = CMS.Resources.Controls.ForgotPasswordControl_Capcha_TextBoxLabel;
            this.capcha.Width = Unit.Percentage(100);
            this.capcha.EnableRefreshImage = true;
            this.capcha.CaptchaLinkButtonText = CMS.Resources.Controls.ForgotPasswordControl_Capcha_LinkButtonText;
            if (this.UseCapcha) {
                TableRow trCapcha = new TableRow();
                TableCell cellCapcha = new TableCell();
                cellCapcha.ColumnSpan = 2;
                cellCapcha.HorizontalAlign = HorizontalAlign.Center;
                cellCapcha.Controls.Add(this.capcha);
                trCapcha.Cells.Add(cellCapcha);
                table.Rows.Add(trCapcha);
            }

            CreateSaveButton();
            CreateChangePasswordButton();
            CreateCancelButton();

            TableRow trButtons = new TableRow();
            TableCell tdButtons = new TableCell();
            tdButtons.HorizontalAlign = HorizontalAlign.Center;
            tdButtons.ColumnSpan = 2;
            tdButtons.Controls.Add(btnSave);
            tdButtons.Controls.Add(btnChangePassword);
            tdButtons.Controls.Add(btnCancel);
            trButtons.Cells.Add(tdButtons);
            table.Rows.Add(trButtons);

            Controls.Add(table);
        }

        private TableCell CreateLoginInput() {
            TableCell cell = new TableCell();
            tbLogin = new TextBox {
                ID = "tbLogin",
                Text = accountEntity.Login,
                Width = Unit.Pixel(200)
            };
            cell.Controls.Add(tbLogin);
            cell.Controls.Add(CreateRequiredFieldValidatorControl(tbLogin.ID));
            if ((Security.IsInRole(Role.OPERATOR)) && !isNew) tbLogin.Enabled = false;
            return cell;
        }

        private TableCell CreateEmailInput() {
            TableCell cell = new TableCell();
            tbEmail = new TextBox {
                ID = "tbEmail",
                Text = accountEntity.Email,
                Width = Unit.Pixel(200)
            };
            cell.Controls.Add(tbEmail);
            cell.Controls.Add(CreateEmailValidatorControl(tbEmail.ID));

            if ((Security.IsInRole(Role.OPERATOR)) && !isNew) tbEmail.Enabled = false;
            return cell;
        }

        private TableCell CreateEnabledCheckBox() {
            TableCell cell = new TableCell();
            cbEnabled = new CheckBox {
                ID = "cbEnabled",
                Checked = accountEntity.Enabled
            };
            cell.Controls.Add(cbEnabled);
            return cell;
        }

        private TableCell CreateVerifiedCheckBox() {
            TableCell cell = new TableCell();
            cbVerified = new CheckBox {
                ID = "cbVerified",
                Checked = accountEntity.Verified
            };
            cell.Controls.Add(cbVerified);
            return cell;
        }

        private TableCell CreateZrusitOvereniEmailemCheckBox() {
            TableCell cell = new TableCell();
            cbZrusitOvereniEmailem = new CheckBox {
                ID = "cbZrusitOvereniEmailem",
                Checked = false
            };
            cell.Controls.Add(cbZrusitOvereniEmailem);
            return cell;
        }

        private TableCell CreateSingleUserCookieLinkEnabledCheckBox() {
            TableCell cell = new TableCell();
            cbSingleUserCookieLinkEnabled = new CheckBox {
                ID = "cbSingleUserCookieLinkEnabled",
                Checked = accountEntity.SingleUserCookieLinkEnabled
            };
            cell.Controls.Add(cbSingleUserCookieLinkEnabled);
            return cell;
        }

        //private TableCell CreateVerifyCodeInput()
        //{
        //    TableCell cell = new TableCell();
        //    tbVerifyCode = new TextBox
        //    {
        //        ID = "tbVerifyCode",
        //        Text = accountEntity.VerifyCode,
        //        Width = Unit.Percentage( 80 )
        //    };
        //    tbVerifyCode.Enabled = !accountEntity.Verified;
        //    cell.Controls.Add( tbVerifyCode );
        //    cell.Controls.Add( CreateRequiredFieldValidatorControl( tbVerifyCode.ID ) );
        //    return cell;
        //}

        private TableCell CreateRolesCheckListBox() {
            List<RoleEntity> list = Storage<RoleEntity>.Read().OrderBy(p => p.Name).ToList();
            List<RoleEntity> roles = new List<RoleEntity>();
            if (Security.IsInRole(Role.OPERATOR)) {
                foreach (RoleEntity role in list) {
                    if (role.Name != Role.ADVISOR && role.Name != Role.NEWSLETTER && role.Name != Role.REGISTEREDUSER)
                        continue;
                    roles.Add(role);
                }
            } else
                roles = list;

            TableCell cell = new TableCell();
            clbRoles = new CheckBoxList();
            clbRoles.ID = "clbRoles";
            clbRoles.DataSource = roles;
            clbRoles.DataTextField = "Name";
            clbRoles.DataValueField = "Id";
            clbRoles.DataBind();
            foreach (ListItem li in clbRoles.Items) {
                //Z role registrovaneho pouzivatela nie je mozne odobrat
                if (li.Text.ToUpper() == RoleEntity.REGISTEREDUSER.ToUpper()) {
                    li.Enabled = false;
                    //Ak sa jedna o nove konto, pouzivatel sa automaticky stava registrovanym pouzivatelom.
                    li.Selected = true;
                    continue;
                }

                li.Selected = accountEntity.RoleString.IndexOf(li.Text) > -1;
            }

            cell.Controls.Add(clbRoles);
            return cell;
        }

        private void CreateCancelButton() {
            btnCancel = new Button {
                Text = global::CMS.Resources.Controls.CancelButton_Text,
                CausesValidation = false
            };
            btnCancel.Click += (s1, e1) => Response.Redirect(this.ReturnUrl);
        }
        private void CreateChangePasswordButton() {
            btnChangePassword = new Button {
                Text = global::CMS.Resources.Controls.ChangePassword_Text,
                CausesValidation = false
            };
            string url = string.Empty;
            if (Security.IsInRole(Role.OPERATOR)) {
                btnChangePassword.Text = "Generovat nové heslo";
                url = string.Format(this.GeneratePasswordUrlFormat, accountEntity.Id) + "&" + this.BuildReturnUrlQueryParam();
                btnChangePassword.Click += (s1, e1) => Response.Redirect(Page.ResolveUrl(url));
                return;
            }

            url = string.Format(this.ChangePasswordUrlFormat, accountEntity.Id) + "&" + this.BuildReturnUrlQueryParam();
            btnChangePassword.Click += (s1, e1) => Response.Redirect(Page.ResolveUrl(url));
            btnChangePassword.Enabled = !isNew;
        }

        private void CreateSaveButton() {
            btnSave = new Button {
                Text = global::CMS.Resources.Controls.SaveButton_Text
            };
            btnSave.Click += (s1, e1) => {
                if (this.UseCapcha) {
                    this.capcha.Validate();
                    if (!this.capcha.IsValid) return;
                }

                StringBuilder roles = new StringBuilder();
                foreach (ListItem li in clbRoles.Items)
                    if (li.Selected) roles.AppendFormat("{0};", li.Text);

                accountEntity.Login = tbLogin.Text;
                accountEntity.Email = tbEmail.Text;
                accountEntity.Enabled = cbEnabled.Checked;
                accountEntity.Verified = cbVerified.Checked;
                //accountEntity.VerifyCode = tbVerifyCode.Text;
                accountEntity.SingleUserCookieLinkEnabled = cbSingleUserCookieLinkEnabled.Checked;
                accountEntity.RoleString = roles.ToString();

                bool overeniZruseno = false;
                if (accountEntity.EmailVerified.HasValue && this.cbZrusitOvereniEmailem.Checked) {
                    accountEntity.EmailVerified = null;
                    accountEntity.EmailVerifyCode = null;
                    accountEntity.EmailVerifyStatus = null;
                    accountEntity.EmailToVerify = null;
                    accountEntity.Login = accountEntity.LoginBeforeVerify;
                    overeniZruseno = true;
                }

                if (isNew) {
                    string newPwd = capcha.CaptchaImage.Text;
                    accountEntity.Password = Cryptographer.MD5Hash(newPwd);
                    /*
                        //PasswordPolicy
                        accountEntity.MustChangeAccountPassword = true;
                     * */
                    accountEntity = Storage<AccountEntity>.Create(accountEntity);

                    EmailNotification email = new EmailNotification();
                    email.Subject = "Registrace na EURONA";
                    email.Message = string.Format("Dobrý den<br/><br/>právě jste se stal registrovaným uživatelem.<br/>Vaše přihlašovací jméno je : {0}<br/>Vaše heslo : {1}<br/><br/>S pozdravem<br/>{2}",
                            accountEntity.Login,
                            newPwd,
                            ConfigValue("SHP:SMTP:FromDisplay"));
                    email.To = accountEntity.Email;
                    email.Notify(true);
                } else {
                    if (!accountEntity.IsInRole(Eurona.DAL.Entities.Role.REGISTEREDUSER))
                        accountEntity.RoleString = roles.ToString();
                    Storage<AccountEntity>.Update(accountEntity);
                }

                Organization org = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = accountEntity.Id });
                if (accountEntity.IsInRole(Eurona.DAL.Entities.Role.ADVISOR) || org != null) {
                    //Oprava roli, ak je neukoncena registracia
                    if (!accountEntity.IsInRole(Eurona.DAL.Entities.Role.REGISTEREDUSER)) {
                        accountEntity.RoleString = roles.ToString();
                        Storage<AccountEntity>.Update(accountEntity);
                    }

                    if (org == null) {
                        org = new Organization();
                        org.Name = accountEntity.Login;
                        org.AccountId = accountEntity.Id;
                        org.ContactEmail = accountEntity.Email;
                        Storage<Organization>.Create(org);

                        if (isNew) {
                            if (!string.IsNullOrEmpty(this.RegisterUserUrlFormat))
                                Response.Redirect(string.Format(this.RegisterUserUrlFormat, accountEntity.Id) + "&ReturnUrl=" + this.ReturnUrl);
                        }
                        if (!string.IsNullOrEmpty(this.UserDetailUrlFormat))
                            Response.Redirect(string.Format(this.UserDetailUrlFormat, accountEntity.Id) + "&ReturnUrl=" + this.ReturnUrl);

                    }
                }

                Response.Redirect(this.ReturnUrl);
            };
        }

    }
}

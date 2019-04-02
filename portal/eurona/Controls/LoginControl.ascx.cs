using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using System.Text;
using CMS.Utilities;
using CMS.Controls;
using Eurona.DAL.Entities;
using OrderEntity = Eurona.DAL.Entities.Order;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using System.Configuration;

namespace Eurona.Controls {
    public partial class LoginControl : CmsControl {
        public delegate void ContinueRegistrationHandler(Account account);
        public event ContinueRegistrationHandler OnContinueRegistration;

        protected void Page_Load(object sender, EventArgs e) {
            //Nastavenie default buttonu
            //this.Page.Form.DefaultButton = this.doLogin.UniqueID;
            bool logged = Request.IsAuthenticated;// && Security.IsLogged(false);
            loginForm.Visible = !logged;
            CMS.Utilities.AliasUtilities util = new AliasUtilities();
            string alias = util.Resolve("~/user/host/default.aspx", this.Page);// "~/user/advisor/register.aspx", this.Page );
        }

        private void DisplayInvalidLogin() {
            ClientScriptManager csm = Page.ClientScript;
            StringBuilder script = new StringBuilder();
            script.Append("alert('Nesprávne přihlašovací údaje!');");
            csm.RegisterStartupScript(GetType(), "invalidLogin", script.ToString(), true);
        }

        protected void OnLoginClick(object sender, EventArgs e) {
            int instanceId = 0;
            Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId);
            Account account = Storage<Account>.ReadFirst(new Account.ReadByLoginAndInstance { Login = login.Value, InstanceId = instanceId });
            if (account == null) {
                account = Storage<Account>.ReadFirst(new Account.ReadByLogin { Login = login.Value });
                if (account != null && !account.IsInRole(DAL.Entities.Role.ADMINISTRATOR))
                    account = null;
            }
            string pwd = Cryptographer.MD5Hash(password.Value);

            //Kontrola novych pouzivatelov na existenciu objednavky
            if (account != null && account.Enabled && account.Authenticate(pwd)) {
                //Ak je pouzivatel neovereny a bol vytvoreny skor ako pred 48 hodinami
                if (!account.Verified && account.Created <= (DateTime.Now.AddHours(-48))) {
                    OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadNot { AccountId = account.Id, OrderStatusCode = ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString() });

                    //Blokacia konta
                    if (order == null) {
                        account.Enabled = false;
                        Storage<Account>.Update(account);
                    }
                }
            }

            //Aktualicacia locale podla Registracneho cisla
            if (account != null) {
                OrganizationEntity organization = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = account.Id });
                if (organization != null) {
                    /*Nastaveni spravneho locale podle registracniho cisla*/
                    string registrationLocale = OrganizationEntity.GetLocaleByRegistrationCode(organization.Code);
                    if (account.Locale != registrationLocale) {
                        account.Locale = registrationLocale;
                        Storage<Account>.Update(account);
                    }
                }
            }

            if (account != null && account.Enabled && account.Authenticate(pwd)) {
                CMS.Entities.Role admininistrator = account.Roles.Find(r => r.Name == CMS.Entities.Role.ADMINISTRATOR);
                Security.Login(account, admininistrator == null);

                //Update Login time for logged user
                Security.UpdateLoginTime();

                //Redirect podla role
                if (admininistrator != null) Response.Redirect("~/admin");
                else if (Security.Account.IsInRole(Eurona.DAL.Entities.Role.ADVISOR)) Response.Redirect("~/user/advisor/");
                else if (Security.Account.IsInRole(Eurona.DAL.Entities.Role.OPERATOR)) Response.Redirect("~/user/operator/");
                else {
                    string url = this.ReturnUrl;
                    url = string.IsNullOrEmpty(url) ? "~/" : url;
                    Response.Redirect(url);
                }

                return;
            } else {
                if (account != null) {
                    if (string.IsNullOrEmpty(account.RoleString)) {
                        if (OnContinueRegistration != null) {
                            OnContinueRegistration(account);
                            return;
                        }
                    }

                    if (account.Enabled == false) {
                        //Account is after registration disables
                        Response.Redirect(string.Format("~/user/anonymous/requestEmailVerifycation.aspx?status=typeEmail&id={0}", account.Id));
                        /*
                        ClientScriptManager csm = Page.ClientScript;
                        StringBuilder script = new StringBuilder();
                        script.Append("alert('Váš účet ještě nebyl ověřen. Kliknutím na odkaz v emailu toto ověření provedete.');");
                        csm.RegisterStartupScript(GetType(), "invalidLogin", script.ToString(), true);
                        */
                        return;
                    }
                }
            }
            DisplayInvalidLogin();
        }

        protected void OnLogoutClick(object sender, EventArgs e) {
            Security.Logout();
            Response.Redirect("~/");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;
using CMS.Utilities;
using System.Text;
using Eurona.Common.DAL.Entities;
using System.IO;
using CMS;
using System.Configuration;
using AnonymniRegistraceEntity = Eurona.Common.DAL.Entities.AnonymniRegistrace;
using AccountEntity = Eurona.DAL.Entities.Account;
using SingleUserCookieLinkActivityEntity = Eurona.DAL.Entities.SingleUserCookieLinkActivity;
using System.Data;
using System.Net.Mail;

namespace Eurona.User.Anonymous {
    public partial class RegisterPage : WebPage {
        //private string mailAttachment = "~/userfiles/Registrační formulář.pdf";
        private AccountEntity singleUserCookieAccount = null;

        protected void Page_Load(object sender, EventArgs e) {

            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();  


            this.ddlStat.SelectedIndexChanged += ddlStat_SelectedIndexChanged;

            //Get account Id (Parent from cookie link)
            singleUserCookieAccount = CookiesUtils.GetSingleUserCookiesLink(this.Page);
            if (singleUserCookieAccount != null && !singleUserCookieAccount.SingleUserCookieLinkEnabled) {
                singleUserCookieAccount = null;
            }
            if (!IsPostBack) {
                this.rbHesloProHostaANO.Checked = true;
                if (singleUserCookieAccount != null) {
                    Organization parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByTVDId{TVD_Id = singleUserCookieAccount.TVD_Id.Value});
                    if (parentOrg != null) {
                        this.rbHesloProHostaANO.Enabled = false;
                        this.rbHesloProHostaNE.Enabled = false;
                        this.txtHesloProHosta.Text = parentOrg.Code;
                        this.txtHesloProHosta.Enabled = true;
                    }
                }
            }

            this.linkVasePrilezitosti.HRef = aliasUtilities.Resolve("~/eshop/kariera.aspx");


            string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            if (locale == "cs") locale = "CZ";
            this.hlSmluvniPodminky.NavigateUrl = string.Format("~/userfiles/Zasady_MLM_{0}.pdf", locale.ToUpper());
            this.hlObchodniPodminky.NavigateUrl = string.Format("/userfiles/podminky_obchodni_spoluprace_{0}.pdf", locale.ToUpper());            
            //if (locale == "pl") this.hlObchodniPodminky.NavigateUrl = "~/userfiles/WARUNKI UMOWY PL.pdf";

            #region Disable Send button and js validation
            StringBuilder sb = new StringBuilder();
            sb.Append("updateASPxValidators();");
            sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + btnContinue.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");

            //change button text and disable it
            sb.AppendFormat("this.value = '{0}...';", this.btnContinue.Text);
            sb.Append("this.disabled = true; if( !onSave() ){document.getElementById('" + this.cbAcceptTerms.ClientID + "').checked=false; this.disabled = false;this.value='" + this.btnContinue.Text + "';return false; }");
            sb.Append(Page.ClientScript.GetPostBackEventReference(this.btnContinue, null) + ";");
            sb.Append("return true;");
            string submit_button_onclick_js = sb.ToString();
            btnContinue.Attributes.Add("onclick", submit_button_onclick_js);
            #endregion

            //'CZ','SK','PL'
            if (!IsPostBack) {
                this.txtPsc.Attributes.Add("readonly", "readonly");
                this.ddlStat.Items.Clear();
                this.ddlStat.Items.Add(new ListItem { Value = "CZ", Text = "CZ" });
                this.ddlStat.Items.Add(new ListItem { Value = "SK", Text = "SK" });
                this.ddlStat.Items.Add(new ListItem { Value = "PL", Text = "PL" });
     

                this.ddlRegion.Items.Clear();
                this.ddlRegion.Items.AddRange(new Eurona.Common.Controls.UserManagement.OrganizationControl.Hepler().GetRegions(this.ddlStat.SelectedValue).ToArray());
                ListItem itemEmpty = new ListItem(string.Empty, string.Empty);
                this.ddlRegion.Items.Insert(0, itemEmpty);

                this.ddlPF.Items.Clear();
                this.ddlPF.Items.Add(new ListItem { Value = "F", Text = Eurona.Common.Resources.Controls.OrganizationControl_PF_F });
                this.ddlPF.Items.Add(new ListItem { Value = "P", Text = Eurona.Common.Resources.Controls.OrganizationControl_PF_P });
            }

            ReklamniZasilkyLoadData(!IsPostBack);
        }

        private void ReklamniZasilkyLoadData(bool bind) {
            List<DAL.Entities.ReklamniZasilky> reklamniZasilky = Storage<DAL.Entities.ReklamniZasilky>.Read();
            this.rpReklamniZasilky.DataSource = reklamniZasilky;
            if (bind) this.rpReklamniZasilky.DataBind();
        }

        protected void ReklamniZasilkyOnItemDataBound(object Sender, RepeaterItemEventArgs e) {
            /*
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
                CheckBox cbx = (CheckBox)e.Item.FindControl("cbReklamniZasilkySouhlas");
                if (cbx != null) {
                    if (string.IsNullOrEmpty(cbx.Attributes["CommandArgument"])) return;

                    int idZasilky = Convert.ToInt32(cbx.Attributes["CommandArgument"]);
                    DAL.Entities.ReklamniZasilky dataItem = (DAL.Entities.ReklamniZasilky)e.Item.DataItem;
                    cbx.Checked = dataItem.Default_souhlas;
                }
            }
             * */
        }

        void ddlStat_SelectedIndexChanged(object sender, EventArgs e) {
            this.ddlRegion.Items.Clear();
            this.ddlRegion.Items.AddRange(new Eurona.Common.Controls.UserManagement.OrganizationControl.Hepler().GetRegions(this.ddlStat.SelectedValue).ToArray());
            ListItem itemEmpty = new ListItem(string.Empty, string.Empty);
            this.ddlRegion.Items.Insert(0, itemEmpty);

            string prefix = GetPrefixByState(this.ddlStat.SelectedValue);
            this.lblMobilPrefix.Text = prefix;
        }

        protected void OnRadioButtonHesloProHostaChecked(object sender, EventArgs e) {
            this.spanHesloProHosta.Visible = this.rbHesloProHostaANO.Checked;
            //this.cbAcceptTerms.Checked = false;
        }

        protected void OnContinueClick(object sender, EventArgs e) {
            ViewState[this.txtPsc.ClientID] = this.txtPsc.Text;

            if (this.cbAcceptTerms.Checked == false) {
                string js = string.Format("alert('{0}');", Resources.EShopStrings.Anonymous_Register_AcceptTerms_Error);
                this.btnContinue.Page.ClientScript.RegisterStartupScript(this.btnContinue.Page.GetType(), "addValidateTerms", js, true);
                return;
            }
            /*
            if (AccountEmailExists(txtEmail.Text)) {
                string js = string.Format("alert('{0}');", Resources.EShopStrings.Anonymous_Register_EmailExists);
                this.btnContinue.Page.ClientScript.RegisterStartupScript(this.btnContinue.Page.GetType(), "addValidateOrganization", js, true);
                this.cbAcceptTerms.Checked = false;
                return;
            }
             * */
            /*
            if (AccountLoginExists(txtLogin.Text)) {
                string js = string.Format("alert('{0}');", Resources.EShopStrings.Anonymous_Register_LoginExists);
                this.btnContinue.Page.ClientScript.RegisterStartupScript(this.btnContinue.Page.GetType(), "addValidateOrganization", js, true);
                this.cbAcceptTerms.Checked = false;
                return;
            }*/


            string hesloProHosta = Organization.EURONA_CODE;
            if (this.rbHesloProHostaANO.Checked) hesloProHosta = this.txtHesloProHosta.Text;
            Organization parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByCode { Code = hesloProHosta });
            if (parentOrg == null) {
                string js = string.Format("alert('{0}');", "Nesprávne heslo pro hosta!");
                this.btnContinue.Page.ClientScript.RegisterStartupScript(this.btnContinue.Page.GetType(), "addValidateOrganization", js, true);
                this.cbAcceptTerms.Checked = false;
                return;
            }

            //Validate Region
            if (String.IsNullOrEmpty(this.ddlRegion.SelectedValue)) {
                string js = string.Format("alert('{0}');", "Prosím, vyplňte Region!");
                this.btnContinue.Page.ClientScript.RegisterStartupScript(this.btnContinue.Page.GetType(), "addValidateOrganization", js, true);
                this.cbAcceptTerms.Checked = false;
                return;
            }

            //Validate PSČ
            string message = Eurona.Common.PSCHelper.ValidatePSCByPSC(this.txtAdresaDodaci_PSC.Text, this.txtAdresaDodaci_Mesto.Text, this.ddlStat.SelectedValue);
            if (message != string.Empty) {
                string js = string.Format("alert('{0}');", message);
                this.btnContinue.Page.ClientScript.RegisterStartupScript(this.btnContinue.Page.GetType(), "addValidateOrganization", js, true);
                this.cbAcceptTerms.Checked = false;
                return;
            }

            this.btnContinue.Enabled = false;

            Account account = new Account();
            account.Locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            //account.Login = txtLogin.Text;
            account.Login = txtEmail.Text;
            account.Email = txtEmail.Text;
            account.Password = Cryptographer.MD5Hash(txtPassword.Text);
            account.SingleUserCookieLinkEnabled = true;
            account.Enabled = false;
            account = Storage<Account>.Create(account);

            Organization organization = new Organization();
            organization.AnonymousAssignToCode = hesloProHosta;
            organization.AnonymousCreatedAt = DateTime.Now;
            if (hesloProHosta != Organization.EURONA_CODE) {
                organization.AnonymousAssignBy = account.Id;
                organization.AnonymousAssignAt = DateTime.Now;
            }

            organization.AccountId = account.Id;
            organization.Id1 = this.txtICO.Text;
            organization.Id2 = this.txtDIC.Text;

            organization.Name = this.txtName.Text;

            organization.VATPayment = this.cbPlatceDPH.Checked;
            organization.TopManager = 0;
            organization.ParentId = parentOrg.TVD_Id;

            organization.ContactEmail = this.txtEmail.Text;
            organization.ContactPhone = this.txtTelefon.Text;

            string prefix = GetPrefixByState(this.ddlStat.SelectedValue);
            organization.ContactMobile = prefix + this.txtMobil.Text;

            organization.ContactBirthDay = (DateTime?)this.dtpDatumNarozeni.Value;
            organization.RegionCode = this.ddlRegion.Text;
            organization.PF = this.ddlPF.Text;
            organization.Statut = "NRZ";
            organization.PredmetCinnosti = this.txtPredmetCinnosti.Text;
            if (singleUserCookieAccount != null) {
                organization.RegistrationFromCookiesLinkAccountId = singleUserCookieAccount.Id;

                SingleUserCookieLinkActivityEntity activity = Storage<SingleUserCookieLinkActivityEntity>.ReadFirst(new SingleUserCookieLinkActivityEntity.ReadBy { IPAddress = Utilities.GetUserIP(this.Page.Request), CookieAccountId = singleUserCookieAccount.Id });
                if (activity != null) {
                    activity.RegistrationAccountId = account.Id;
                    activity.RegistrationTimestamp = DateTime.Now;
                    Storage<SingleUserCookieLinkActivityEntity>.Update(activity);
                }
            }

            organization = Storage<Organization>.Create(organization);

            //Korespondencni adresa
            organization.RegisteredAddress.City = this.txtMesto.Text;
            //organization.RegisteredAddress.Country = this.txtKraj.Text;
            organization.RegisteredAddress.Region = this.ddlRegion.SelectedValue;
            organization.RegisteredAddress.State = this.ddlStat.SelectedValue;
            organization.RegisteredAddress.Street = this.txtUlice.Text;
            organization.RegisteredAddress.Zip = this.txtPsc.Text;
            Storage<CMS.Entities.Address>.Update(organization.RegisteredAddress);

            //Adresa pro doruceni adresa
            organization.CorrespondenceAddress.City = this.txtAdresaDodaci_Mesto.Text;
            organization.CorrespondenceAddress.Region = this.ddlRegion.SelectedValue;
            organization.CorrespondenceAddress.State = this.ddlStat.SelectedValue;
            organization.CorrespondenceAddress.Street = this.txtAdresaDodaci_Ulice.Text;
            organization.CorrespondenceAddress.Zip = this.txtAdresaDodaci_PSC.Text;

            organization.AnonymousRegistration = true;
            Storage<CMS.Entities.Address>.Update(organization.CorrespondenceAddress);

            organization.BankContact.AccountNumber = this.txtBankovniUcet.Text;
            Storage<CMS.Entities.BankContact>.Update(organization.BankContact);


            if (!Eurona.Controls.UserManagement.OrganizationControl.SyncTVDUser(organization, this.btnContinue)) {
                Storage<Organization>.Delete(organization);
                Storage<Account>.Delete(account);
                this.cbAcceptTerms.Checked = false;
                return;
            }

            //Podla nastaveni spadne kazdy X-ty pouzivatel priamo pod euronu a ostatny sa zaradia do fronty cakajucich na priradenie.
            AnonymniRegistraceEntity are = Storage<AnonymniRegistraceEntity>.ReadFirst(new AnonymniRegistraceEntity.ReadById { AnonymniRegistraceId = (int)AnonymniRegistraceEntity.AnonymniRegistraceId.Eurona });
            if (are != null) {
                if (are.EuronaReistraceProcent.HasValue && are.EuronaReistracePocitadlo == (int)100 / are.EuronaReistraceProcent) {
                    are.EuronaReistracePocitadlo = 0;
                    Organization parentEurona = Storage<Organization>.ReadFirst(new Organization.ReadByCode { Code = Organization.EURONA_CODE });

                    organization.AnonymousAssignStatus = "";
                    organization.ParentId = parentEurona.TVD_Id.Value;
                    organization.AnonymousAssignAt = DateTime.Now;
                    organization.AnonymousAssignToCode = parentEurona.Code;
                    Storage<Organization>.Update(organization);
                } else
                    are.EuronaReistracePocitadlo++;

                Storage<AnonymniRegistraceEntity>.Update(are);
            }

            account.Enabled = false;
            account.Verified = false;
            account.AddToRoles(Eurona.Common.DAL.Entities.Role.REGISTEREDUSER, Eurona.Common.DAL.Entities.Role.ADVISOR, Eurona.Common.DAL.Entities.Role.ANONYMOUSADVISOR);
            Storage<Account>.Update(account);

            #region Reklamni zasilky
            //reload account
            account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = account.Id });
            List<DAL.Entities.ReklamniZasilky> reklamniZasilky = Storage<DAL.Entities.ReklamniZasilky>.Read();
            for (int i = 0; i < rpReklamniZasilky.Items.Count; i++) {
                CheckBox cbx = (CheckBox)rpReklamniZasilky.Items[i].FindControl("cbReklamniZasilkySouhlas");
                DAL.Entities.ReklamniZasilky reklamniZasilka = reklamniZasilky[i];
                DAL.Entities.ReklamniZasilkySouhlas reklamniZasilkaSouhlas = Storage<DAL.Entities.ReklamniZasilkySouhlas>.ReadFirst(new DAL.Entities.ReklamniZasilkySouhlas.ReadByOdberatel { Id_zasilky = reklamniZasilka.Id, Id_odberatele = account.TVD_Id.Value });
                if (reklamniZasilkaSouhlas == null) {
                    reklamniZasilkaSouhlas = new DAL.Entities.ReklamniZasilkySouhlas();
                    reklamniZasilkaSouhlas.Id_odberatele = account.TVD_Id.Value;
                    reklamniZasilkaSouhlas.Id_zasilky = reklamniZasilka.Id;
                    reklamniZasilkaSouhlas.Souhlas = cbx.Checked;
                    Storage<DAL.Entities.ReklamniZasilkySouhlas>.Create(reklamniZasilkaSouhlas);
                } else {
                    reklamniZasilkaSouhlas.Souhlas = cbx.Checked;
                    Storage<DAL.Entities.ReklamniZasilkySouhlas>.Update(reklamniZasilkaSouhlas);
                }
            }
            #endregion

            SendRegistrationEmail(account);

            account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = account.Id });
            if (account != null /*&& account.Enabled*/ && account.Authenticate(account.Password)) {
                Security.Login(account, false);
                Security.UpdateLoginTime();
            }

            //Email validation process
            if (Security.Account.IsInRole(Eurona.Common.DAL.Entities.Role.ADVISOR) || Security.Account.IsInRole(Eurona.Common.DAL.Entities.Role.ANONYMOUSADVISOR)) {
                if (Security.Account.EmailVerified.HasValue == false) {
                    Response.Redirect("~/user/anonymous/requestEmailVerifycation.aspx");
                    return;
                }
            }

            Response.Redirect(aliasUtilities.Resolve("~/user/anonymous/cart.aspx"));
        }

        private bool AccountLoginExists(string login) {
            List<Account> exists = Storage<Account>.Read(new Account.ReadByLogin { Login = login });
            return exists != null && exists.Count > 0;
        }

        private bool AccountEmailExists(string email) {
            List<Account> exists = Storage<Account>.Read(new Account.ReadByEmail { Email = email });
            return exists != null && exists.Count > 0;
        }

        /// <summary>
        /// Odoslanie informacneho mailu o registracii pouzivatela
        /// </summary>
        private bool SendRegistrationEmail(Account customerAccount) {
            Organization org = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = customerAccount.Id });
            if (org == null) return false;

            Organization parentOrg = null;
            if (org.ParentId.HasValue) parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByTVDId { TVD_Id = org.ParentId.Value });
            /*
			StringBuilder htmlResponse = new StringBuilder();
			TextWriter textWriter = new StringWriter(htmlResponse);
			Server.Execute(ResolveUrl(string.Format("~/user/advisor/registerDocument.aspx?id={0}", org.Id)), textWriter);
            */

            string root = Utilities.Root(Request);
            string urlUser = root + "user/advisor/";
            string urlParentUser = root + "advisor/newAdvisors.aspx";
            string urlCentral = String.Format("{0}admin/account.aspx?id={1}&ReturnUrl=/default.aspx", root, customerAccount.Id);
            EmailNotification email2User = new EmailNotification {
                To = customerAccount.Email,
                Subject = Resources.Strings.UserRegistrationPage_Email2User_Subject,
                Message = String.Format(Resources.Strings.UserRegistrationPage_Email2User_Message, customerAccount.Login, this.txtPassword.Text).Replace("\\n", Environment.NewLine) + "<br/><br/>"// + htmlResponse.ToString()
            };
            if (parentOrg != null) {
                string contact = string.Format("{0}, {1}, {2}, {3}, reg. číslo: {4}", org.Name, org.RegisteredAddressString, org.ContactMobile, org.Account.Email, org.Code);
                EmailNotification email2Parent = new EmailNotification {
                    To = parentOrg.Account.Email,
                    Subject = Resources.Strings.UserRegistrationPage_Email2Central_Subject,
                    Message = String.Format(Resources.Strings.UserRegistrationPage_Email2Sponsor_Message, contact.ToUpper()).Replace("\\n", Environment.NewLine) + "<br/><br/>"// + htmlResponse.ToString()
                };
                email2Parent.Notify(true);
            }

            EmailNotification email2Central = new EmailNotification {
                To = ConfigurationManager.AppSettings["SMTP:CentralInbox"],
                Subject = Resources.Strings.UserRegistrationPage_Email2Central_Subject,
                Message = String.Format(Resources.Strings.UserRegistrationPage_Email2Central_Message, urlCentral, customerAccount.Login).Replace("\\n", Environment.NewLine)
            };

            //14.11.2016 - Dále Vás žádám z e-mailů odstranit zobrazovaný registrační formulář a to jak ve znění e-mailu, tak v příloze. Tento registrační formulář nechci, aby se zasílal v jakékoli podobě.
            /*
            email2User.Attachments = new List<System.Net.Mail.Attachment>();
            Attachment attachment = new Attachment(Server.MapPath(mailAttachment));
            email2User.Attachments.Add(attachment);
            */

            bool okUser = email2User.Notify(true);
            bool okCentral = email2Central.Notify();

            return okUser && okCentral;
        }

        private string GetPrefixByState(string state) {
            string prefix = "+420";
            if (state.ToUpper() == "CZ") prefix = "+420";
            else if (state.ToUpper() == "SK") prefix = "+421";
            else if (state.ToUpper() == "PL") prefix = "+48";
            else prefix = "+420";
            return prefix;
        }
    }
}
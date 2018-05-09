using System;
using CMS.Utilities;
using System.Configuration;
using CMS;
using Eurona.DAL.Entities;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using System.Text;
using System.IO;
using Eurona.Common.DAL.Entities;
using System.Net.Mail;
using System.Collections.Generic;

namespace Eurona.User.Advisor {
    public partial class RegisterUser : WebPage {
        //private string mailAttachment = "~/userfiles/Registrační formulář.pdf";
        private int accountId = -2;
        private int AccountId {
            get {
                if (accountId != -2) return accountId;
                string id = Server.UrlDecode(Request["token"].Replace("+", "%2b"));
                id = Cryptographer.Decrypt(id);
                accountId = -1;
                Int32.TryParse(id, out accountId);
                return accountId;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            string locale = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
            //if (locale.ToLower() == "sk") mailAttachment = "~/userfiles/Registračný formulár.pdf";
            //if (locale.ToLower() == "pl") mailAttachment = "~/userfiles/Formularz rejestracyjny.pdf";


            //Ak uz osoba existuje, nebude ju mozne zmenit!!
            OrganizationEntity organization = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = this.AccountId });
            if (organization != null) {
                organizationControl.OrganizationId = organization.Id;
                //organizationControl.IsEditing = false;
                //return;
            }

            organizationControl.SaveCompleted += SaveCompeted;
            organizationControl.Canceled += Canceled;

            if (IsPostBack) return;

            organizationControl.AccountId = AccountId;
            organizationControl.Visible = AccountId > 0;

            #region Organization Settings
            //Address settings
            this.organizationControl.Settings = new Eurona.Common.Controls.UserManagement.OrganizationControl.ControlSettings();
            this.organizationControl.Settings.Visibility.Id3 = false;
            this.organizationControl.Settings.Visibility.ContactEmail = true;
            this.organizationControl.Settings.Visibility.ContactMobil = true;
            this.organizationControl.Settings.Visibility.ContactPhone = true;

            this.organizationControl.Settings.Require.ContactEmail = true;
            this.organizationControl.Settings.Require.ContactMobil = true;
            this.organizationControl.Settings.Require.PF = true;
            this.organizationControl.Settings.Require.Statut = true;
            this.organizationControl.Settings.Require.RegionCode = true;

            this.organizationControl.Settings.CorrespondenceAddressSettings.Visibility.Country = false;
            this.organizationControl.Settings.CorrespondenceAddressSettings.Visibility.State = true;
            this.organizationControl.Settings.CorrespondenceAddressSettings.Require.City = true;
            this.organizationControl.Settings.CorrespondenceAddressSettings.Require.Street = true;
            this.organizationControl.Settings.CorrespondenceAddressSettings.Require.Zip = true;
            this.organizationControl.Settings.CorrespondenceAddressSettings.Require.State = true;
            this.organizationControl.Settings.Visibility.InvoicingAddress = false;
            this.organizationControl.Settings.RegisteredAddressSettings = this.organizationControl.Settings.CorrespondenceAddressSettings;

            //Bank contact
            this.organizationControl.Settings.BankContactSettings.Require.BankCode = false;
            this.organizationControl.Settings.BankContactSettings.Require.AccountNumber = false;

            //Contact organization
            this.organizationControl.Settings.Visibility.ContactPerson = false;
            this.organizationControl.Settings.ContactPersonSettings.Require.FirstName = true;
            this.organizationControl.Settings.ContactPersonSettings.Require.LastName = true;
            this.organizationControl.Settings.ContactPersonSettings.Require.Email = true;
            this.organizationControl.Settings.ContactPersonSettings.Require.Mobile = true;
            this.organizationControl.Settings.ContactPersonSettings.Visibility.HomeAddress = false;
            this.organizationControl.Settings.ContactPersonSettings.Visibility.TempAddress = false;
            #endregion

            if (!IsPostBack) {
                if (Eurona.User.Host.HostSecurity.IsAutenticated(this)) {
                    EnsureChildControls();
                    object hostName = this.Page.Session[Eurona.User.Host.HostSecurity.HostNameSessionName];
                    object advisorCode = this.Page.Session[Eurona.User.Host.HostSecurity.HostAdvisorCodeSessionName];
                    if (hostName != null && hostName.ToString() != string.Empty)
                        organizationControl.PreddefinedName = hostName.ToString();

                    if (advisorCode != null && advisorCode.ToString() != string.Empty) {
                        organization = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = advisorCode.ToString() });
                        organizationControl.PreddefinedParentTVD_Id = organization.TVD_Id;
                    }

                }

                Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = AccountId });
                organizationControl.PreddefinedEmail = account.Email;

            }

        }

        protected void Canceled(object sender, EventArgs args) {
            Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = AccountId });
            if (account != null) Storage<Account>.Delete(account);
            Response.Redirect("~/default.aspx");
        }

        protected void SaveCompeted(object sender, EventArgs args) {
            Account account = Storage<Account>.ReadFirst(new Account.ReadById { AccountId = AccountId });
            account.Enabled = true;//false; zatial si to neobjednali//True nastavi operatorka po kontrole
            account.AddToRoles(Eurona.Common.DAL.Entities.Role.REGISTEREDUSER, Eurona.Common.DAL.Entities.Role.ADVISOR);
            Storage<Account>.Update(account);

            //Odoslanie Emailu registrovanemu zakaznikovi a administratorovi
            SendRegistrationEmail(account);

            Response.Redirect(String.Format("~/user/advisor/registerUserFinish.aspx?token={0}", Request["token"]));
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

            string pwd = string.Empty;
            if (Session[customerAccount.Login] != null)
                pwd = Session[customerAccount.Login].ToString();
            EmailNotification email2User = new EmailNotification {
                To = customerAccount.Email,
                Subject = Resources.Strings.UserRegistrationPage_Email2User_Subject,
                Message = String.Format(Resources.Strings.UserRegistrationPage_Email2User_Message, customerAccount.Login, pwd).Replace("\\n", Environment.NewLine) + "<br/><br/>"// + htmlResponse.ToString()
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

            Session[customerAccount.Login] = null;
            return okUser && okCentral;
        }

    }
}

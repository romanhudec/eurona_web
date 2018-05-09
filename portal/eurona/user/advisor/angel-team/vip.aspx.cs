using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using Eurona.Controls.UserManagement;
using System.Text;
using System.IO;
using CMS;
using System.Configuration;
using AngelTeamViewsEntity = Eurona.Common.DAL.Entities.AngelTeamViews;

namespace Eurona.User.Advisor.AngelTeam
{
    public partial class VIPPage : WebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Security.IsLogged(true);

            #region Check na maximalny pocet zobrazeni za 1 minutu
            AngelTeamSettings atpSettings = Storage<AngelTeamSettings>.ReadFirst();
            if (atpSettings == null)
            {
                atpSettings = new Common.DAL.Entities.AngelTeamSettings();
                atpSettings.DisableATP = false;
                atpSettings.BlockATPHours = 1;
                atpSettings.MaxViewPerMinute = 100;
                Storage<AngelTeamSettings>.Create(atpSettings);
            }

            AngelTeamViewsEntity atpViews = Storage<AngelTeamViewsEntity>.ReadFirst(new AngelTeamViews.ReadByAccount { AccountId = Security.Account.Id });
            if (atpViews != null && atpViews.ViewCount >= atpSettings.MaxViewPerMinute)
            {
                //Ak je pocet minut mensi ako je v nastaveni blokacie, pristup odmietnuty
                TimeSpan ts = DateTime.Now - atpViews.ViewDate;
                if (ts.TotalMinutes <= atpSettings.BlockATPHours * 60)
                {
                    Response.Redirect("~/right.aspx");
                    return;
                }
            }
            Storage<AngelTeamViewsEntity>.Update(new AngelTeamViews { AccountId = Security.Account.Id });
            #endregion

            LoadData(!IsPostBack);

            if (!Security.IsInRole(Role.ADMINISTRATOR))
            {
                if (this.LogedAdvisor == null || this.LogedAdvisor.AngelTeamClen == false && this.LogedAdvisor.AngelTeamManager == false)
                {
                    Response.Redirect("~/user/advisor/angel-team/");
                    return;
                }
            }
            List<LoggedAccount> listLoggedATPClen = Storage<LoggedAccount>.Read(new LoggedAccount.ReadLogged { AngelTeamClen = true, AngelTeamManager = false });
            List<LoggedAccount> listLoggedATPManager = Storage<LoggedAccount>.Read(new LoggedAccount.ReadLogged { AngelTeamClen = true, AngelTeamManager = true, AngelTeamManagerTyp = (int)ATPManagerTyp.ZlatyLector });
            this.rpPrihlaseniClenVIP.DataSource = listLoggedATPClen;
            this.rpPrihlaseniManagerVIP.DataSource = listLoggedATPManager;
            if (!IsPostBack)
            {
                this.rpPrihlaseniClenVIP.DataBind();
                this.rpPrihlaseniManagerVIP.DataBind();
            }

            //if (this.LogedAdvisor != null && this.LogedAdvisor.AngelTeamManager)
            //    this.angelTeamClen.Attributes.Add("class", "angel-man-yellow");
            //else
            //    this.angelTeamClen.Attributes.Add("class", "angel-man-blue");
        }

        private void LoadData(bool bind)
        {
            AnonymniRegistrace nastaveni = Storage<AnonymniRegistrace>.ReadFirst(new AnonymniRegistrace.ReadById { AnonymniRegistraceId = (int)AnonymniRegistrace.AnonymniRegistraceId.Eurona });
            List<Organization> listCekajici = new List<Organization>();
            if (nastaveni == null || nastaveni.ZobrazitVSeznamuNeomezene)
                listCekajici = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration = true, Assigned = false, RegionCode = this.LogedAdvisor.RegionCode });
            else
            {
                AnonymniRegistraceLimit anonymniRegistraceLimit = new AnonymniRegistraceLimit(nastaveni.ZobrazitVSeznamuLimit);
                if (anonymniRegistraceLimit.IsInLimitForATPClen(DateTime.Now) && this.LogedAdvisor.AngelTeamClen && this.LogedAdvisor.TopManager == 0)
                {
                    listCekajici = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration = true, Assigned = false, RegionCode = this.LogedAdvisor.RegionCode });
                }
                else if (anonymniRegistraceLimit.IsInLimitForATPManager(DateTime.Now) && this.LogedAdvisor.AngelTeamClen && (this.LogedAdvisor.TopManager == 1 || this.LogedAdvisor.AngelTeamManager))
                {
                    listCekajici = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration = true, Assigned = false, RegionCode = null });
                }
            }

            //Dnes prijati a cekajici na potvrzeni
            List<Organization> listDnesPrijati = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration = true, Assigned = true, AnonymousAssignAt = DateTime.Now });

            //Vsichni prijati a cekajici na potvrzeni
            DateTime date = DateTime.Now;
            List<Organization> listAllPrijatiCekajiciNaPotvrzeni = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration = true, Assigned = true, AssignedAndConfirmed = false, CreatedAtYear = date.Year, CreatedAtMonth = date.Month, CreatedAtDay = date.Day });

            this.rpCekajiciNovacci.DataSource = listCekajici;
            if (bind)
                this.rpCekajiciNovacci.DataBind();

            List<Organization> listAllPrijati = new List<Organization>();
            listAllPrijati.AddRange(listDnesPrijati);
            listAllPrijati.AddRange(listAllPrijatiCekajiciNaPotvrzeni);

            this.rpPrijatiNovacci.DataSource = listAllPrijati;
            if (bind)
                this.rpPrijatiNovacci.DataBind();
        }

        private OrganizationEntity logedAdvisor = null;
        public OrganizationEntity LogedAdvisor
        {
            get
            {
                if (logedAdvisor != null) return logedAdvisor;
                logedAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = Security.Account.Id });
                return logedAdvisor;
            }
        }


        protected void OnItemDataBound(object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Button btn = (Button)e.Item.FindControl("btnOk");
                if (btn != null)
                {
                    btn.OnClientClick = string.Format("return validatePrijemNovacka({0})", (e.Item.DataItem as Organization).Id);
                }
            }
        }

        protected void OnPrijmoutNovacka(object sender, EventArgs e)
        {
            if (!Security.Account.TVD_Id.HasValue) return;

            Button btn = (sender as Button);
            if (string.IsNullOrEmpty(btn.CommandArgument)) return;

            #region Kontrola na maximalny pocet dnes prijatych novacikov
            AnonymniRegistrace nastaveni = Storage<AnonymniRegistrace>.ReadFirst(new AnonymniRegistrace.ReadById { AnonymniRegistraceId = (int)AnonymniRegistrace.AnonymniRegistraceId.Eurona });
            if (nastaveni.MaxPocetPrijetychNovacku != 0)
            {
                //Ak som top manager a je v limite ATPManager (nasledujuca hodina po hornom limite), nebude sa pocet obmedzovat
                AnonymniRegistraceLimit anonymniRegistraceLimit = new AnonymniRegistraceLimit(nastaveni.ZobrazitVSeznamuLimit);
                if (anonymniRegistraceLimit.IsInLimitForATPManager(DateTime.Now) && this.LogedAdvisor.TopManager == 1)
                    nastaveni.MaxPocetPrijetychNovacku = 999999;

                int dnesPrijatiUzivatelem = 0;
                List<Organization> listDnesPrijati = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration = true, Assigned = true, AnonymousTempAssignAt = DateTime.Now });
                foreach (Organization organization in listDnesPrijati)
                {
                    if (organization.AnonymousAssignBy == Security.Account.Id)
                        dnesPrijatiUzivatelem++;
                }
                if (dnesPrijatiUzivatelem >= nastaveni.MaxPocetPrijetychNovacku)
                {
                    string js = string.Format("alert('{0}');", "Dosáhli jste maximálního povoleného počtu přijetých nováčků pro tento den!");
                    btn.Page.ClientScript.RegisterStartupScript(btn.Page.GetType(), "parentValidateOrganization", js, true);
                    return;
                }
            }
            #endregion

            int id = Convert.ToInt32(btn.CommandArgument);
            Organization org = Storage<Organization>.ReadFirst(new Organization.ReadById { OrganizationId = id });
            if (org == null) return;

            if (org.AnonymousAssignBy.HasValue)
            {
                string js = string.Format("alert('{0}');", "Nováček již byl přijat jiným uživatelem!");
                btn.Page.ClientScript.RegisterStartupScript(btn.Page.GetType(), "parentValidateOrganization", js, true);
                return;
            }

            //Get Parent
            string parentCode = this.hfRegistracniCislo.Value;
            Organization parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByCode { Code = parentCode });
            if (parentOrg == null || string.IsNullOrEmpty(parentCode))
            {
                string js = string.Format("alert('{0}');", "Nesprávne registrační číslo!");
                btn.Page.ClientScript.RegisterStartupScript(btn.Page.GetType(), "parentValidateOrganization", js, true);
                return;
            }

            Organization orgBy = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = Security.Account.Id });
            if (orgBy == null) return;

            org.ParentId = parentOrg.TVD_Id.Value;
            org.AnonymousAssignAt = null;
            org.AnonymousTempAssignAt = DateTime.Now;
            org.AnonymousAssignBy = Security.Account.Id;
            org.AnonymousAssignByCode = orgBy.Code;
            org.AnonymousAssignToCode = parentOrg.Code;
            Storage<Organization>.Update(org);

            //OrganizationControl.UpdateSponsorTVDUser(org, btn);
            SendAssignEmail(org.Account);
            LoadData(true);
        }

        /// <summary>
        /// Odoslanie informacneho mailu o registracii pouzivatela
        /// </summary>
        private bool SendAssignEmail(CMS.Entities.Account customerAccount)
        {
            Organization org = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = customerAccount.Id });
            if (org == null) return false;

            Organization parentOrg = null;
            if (org.ParentId.HasValue) parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByTVDId { TVD_Id = org.ParentId.Value });

            StringBuilder htmlResponse = new StringBuilder();
            TextWriter textWriter = new StringWriter(htmlResponse);
            Server.Execute(ResolveUrl(string.Format("~/user/advisor/registerDocument.aspx?id={0}", org.Id)), textWriter);

            string root = Utilities.Root(Request);
            string urlParentUser = root + "advisor/newAdvisors.aspx";
            if (parentOrg != null)
            {
                string contact = string.Format("{0}, {1}, {2}, {3}, reg. číslo: {4}", org.Name, org.RegisteredAddressString, org.ContactMobile, org.Account.Email, org.Code);
                EmailNotification email2Parent = new EmailNotification
                {
                    To = parentOrg.Account.Email,
                    Subject = Resources.Strings.UserRegistrationPage_Email2Central_Subject,
                    Message = String.Format(Resources.Strings.UserRegistrationPage_Email2Sponsor_Message, contact.ToUpper()).Replace("\\n", Environment.NewLine) + "<br/><br/>" + htmlResponse.ToString()
                };
                return email2Parent.Notify(true);
            }

            return false;
        }
    }
}
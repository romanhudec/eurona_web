using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.Common.DAL.Entities;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using Eurona.Controls.UserManagement;
using System.Web.UI.HtmlControls;

namespace Eurona.User.Advisor {
    public partial class AnonymousAssignPage : WebPage {
        protected void Page_Load(object sender, EventArgs e) {
            Security.IsLogged(true);
            LoadData(!IsPostBack);

            if (this.LogedAdvisor == null || this.LogedAdvisor.ManageAnonymousAssign == false) {
                Response.Redirect("~/user/advisor/");
                return;
            }
            Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "on_page_submit_script", "fnOnPageSubmit();");
        }

        private void LoadData(bool bind) {
            DateTime beforDate = DateTime.Now.AddMonths(-1);
            List<Organization> listNovacci = Storage<Organization>.Read(new Organization.ReadByAnonymous { AnonymousRegistration = true, Assigned = true, AssignedAndConfirmed = false, CreatedAtYear = beforDate.Year, CreatedAtMonth = beforDate.Month });
            int index = 1;
            foreach (Organization org in listNovacci) {
                org.BankContactId = index;
                index++;
            }
            this.btnPotvrditVybrane.Enabled = listNovacci.Count != 0;
            this.rpCekajiciNovacci.DataSource = listNovacci;
            if (bind) this.rpCekajiciNovacci.DataBind();
        }

        private OrganizationEntity logedAdvisor = null;
        public OrganizationEntity LogedAdvisor {
            get {
                if (logedAdvisor != null) return logedAdvisor;
                logedAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = Security.Account.Id });
                return logedAdvisor;
            }
        }

        protected void OnItemDataBound(object Sender, RepeaterItemEventArgs e) {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
            //    Button btn = (Button)e.Item.FindControl("btnPotvrdit");
            //    if (btn != null) {
            //        btn.OnClientClick = string.Format("return validatePrijemNovacka({0})", (e.Item.DataItem as Organization).Id);
            //    }
            //    CheckBox cbx = (CheckBox)e.Item.FindControl("cbxVybrat");
            //    if (cbx != null) {
            //        cbx.Attributes.Add("onchange", string.Format("return validatePrijemNovacka({0})", (e.Item.DataItem as Organization).Id));
            //    }
            //}
        }

        private bool PotvrditPrijetiNovacka(int organizationId, Control sender, string parentCode, string parentName) {
            Organization org = Storage<Organization>.ReadFirst(new Organization.ReadById { OrganizationId = organizationId });
            if (org == null) return false;

            if (!Security.Account.IsInRole(Role.ADMINISTRATOR)) {
                if (org.AnonymousAssignAt.HasValue) {
                    string js = string.Format("blockUIAlert('Chyba', '{0}');", "Nováček již byl potvrzen jiným uživatelem!");
                    sender.Page.ClientScript.RegisterStartupScript(sender.Page.GetType(), "parentValidateOrganization", js, true);
                    return false;
                }
            }

            //Get Parent
            Organization parentOrg = null;
            if (!string.IsNullOrEmpty(parentCode)) parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadByCode { Code = parentCode });
            if (parentOrg == null && !string.IsNullOrEmpty(parentName)) parentOrg = Storage<Organization>.ReadFirst(new Organization.ReadBy { Name = parentName });
            if (parentOrg == null || (string.IsNullOrEmpty(parentCode) && string.IsNullOrEmpty(parentName))) {
                return false;
                /*
                string js = string.Format("blockUIAlert('Chyba', '{0}');", "Nesprávne registrační číslo nebo jméno!");
                sender.Page.ClientScript.RegisterStartupScript(sender.Page.GetType(), "parentValidateOrganization", js, true);
                return false;
                 * */
            }

            string message = string.Empty;
            org.ParentId = parentOrg.TVD_Id.Value;
            if (OrganizationControl.UpdateSponsorTVDUser(org, sender, out message)) {
                org.AnonymousAssignStatus = message;
                org.ParentId = parentOrg.TVD_Id.Value;
                org.AnonymousAssignAt = DateTime.Now;
                org.AnonymousAssignToCode = parentOrg.Code;
                Storage<Organization>.Update(org);
            } else {
                org.AnonymousAssignStatus = message;
                Storage<Organization>.Update(org);
            }
            return true;
        }

        protected void OnPotvrditPrijeti(object sender, EventArgs e) {
            if (!Security.Account.TVD_Id.HasValue) return;

            Button btn = (sender as Button);
            if (string.IsNullOrEmpty(btn.CommandArgument)) return;

            int id = Convert.ToInt32(btn.CommandArgument);

            TextBox txtCode = (TextBox)btn.Parent.FindControl("txtCode");
            TextBox txtCodeJmeno = (TextBox)btn.Parent.FindControl("txtCodeJmeno");

            PotvrditPrijetiNovacka(id, btn, txtCode.Text, txtCodeJmeno.Text);
           
            LoadData(true);
        }

        protected void OnPotvrditPrijetiVybrane(object sender, EventArgs e) {
            foreach (RepeaterItem item in this.rpCekajiciNovacci.Items) {
                CheckBox cbxAnonymousOvereniSluzeb = (CheckBox)item.FindControl("cbAnonymousOvereniSluzeb");
                TextBox txtCode = (TextBox)item.FindControl("txtCode");
                TextBox txtCodeJmeno = (TextBox)item.FindControl("txtCodeJmeno");
               
                if (cbxAnonymousOvereniSluzeb.Checked) {
                    int id = Convert.ToInt32(cbxAnonymousOvereniSluzeb.Attributes["CommandArgument"]);
                    if (PotvrditPrijetiNovacka(id, cbxAnonymousOvereniSluzeb, txtCode.Text, txtCodeJmeno.Text) == false) return;
                }
            }
            LoadData(true);
        }

        protected string GetOrganizationNameByCode(string code) {
            Organization org = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByCode { Code = code });
            if (org == null) return string.Empty;

            return org.Name;
        }
    }
}
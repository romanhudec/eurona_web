using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AnonymniRegistraceEntity = Eurona.Common.DAL.Entities.AnonymniRegistrace;
using Eurona.Common.DAL.Entities;
using Telerik.Web.UI;
using System.Globalization;
using System.Threading;

namespace Eurona.Admin {
    public partial class EuronaNastaveniAnonymniRegistrace : WebPage {
        private AnonymniRegistraceEntity anonymniRegistrace = null;
        private AnonymniRegistraceLimit anonymniRegistraceLimit = null;
        protected void Page_Load(object sender, EventArgs e) {
            this.gridView.DataSource = Storage<Organization>.Read(new Organization.ReadAnonymousAssignManager { });
            if (!IsPostBack) this.gridView.DataBind();
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            this.anonymniRegistrace = Storage<AnonymniRegistraceEntity>.ReadFirst(new AnonymniRegistraceEntity.ReadById { AnonymniRegistraceId = (int)AnonymniRegistraceEntity.AnonymniRegistraceId.Eurona });
            if (anonymniRegistrace == null) return;

            this.txtEuronaReistraceProcent.Text = anonymniRegistrace.EuronaReistraceProcent.ToString();

            this.cbNeomezene.Checked = anonymniRegistrace.ZobrazitVSeznamuNeomezene;
            this.txtMaxPocetPrijetychNovacku.Text = anonymniRegistrace.MaxPocetPrijetychNovacku.ToString();

            this.anonymniRegistraceLimit = new AnonymniRegistraceLimit(anonymniRegistrace.ZobrazitVSeznamuLimit);

            this.gridViewZobrazitVSeznamuLimit.DataSource = this.anonymniRegistraceLimit.GetData();
            if (!IsPostBack) this.gridViewZobrazitVSeznamuLimit.DataBind();

            this.ddlZobrazitVSeznamuLimitFrom.Items.Clear();
            this.ddlZobrazitVSeznamuLimitTo.Items.Clear();
            List<String> daysInWeek = new List<string>();
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            for (int day = 0; day < 7; day++) {
                this.ddlZobrazitVSeznamuLimitFrom.Items.Add(new ListItem { Value = day.ToString(), Text = ci.DateTimeFormat.DayNames[day] });
                this.ddlZobrazitVSeznamuLimitTo.Items.Add(new ListItem { Value = day.ToString(), Text = ci.DateTimeFormat.DayNames[day] });
            }

            OnNeomezeneChecked(this, null);
        }

        protected void OnNeomezeneChecked(object sender, EventArgs e) {
            this.trObdobi.Visible = !this.cbNeomezene.Checked;
        }

        protected void OnZobrazitVSeznamuLimitRowCommand(object sender, GridCommandEventArgs e) {
            if (e.CommandName == "DELETE_ITEM") {
                GridDataItem dataItem = (GridDataItem)e.Item;
                string limitString = dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"].ToString();

                this.anonymniRegistraceLimit.Remove(limitString);
                this.anonymniRegistrace.ZobrazitVSeznamuLimit = this.anonymniRegistraceLimit.ToString();
                Storage<AnonymniRegistraceEntity>.Update(this.anonymniRegistrace);

                this.gridViewZobrazitVSeznamuLimit.DataSource = this.anonymniRegistraceLimit.GetData();
                this.gridViewZobrazitVSeznamuLimit.DataBind();
            }
        }

        protected void OnRowCommand(object sender, GridCommandEventArgs e) {
            if (e.CommandName == "DELETE_ITEM") {
                GridDataItem dataItem = (GridDataItem)e.Item;
                int id = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
                Organization organization = Storage<Organization>.ReadFirst(new Organization.ReadById { OrganizationId = id });
                if (organization == null) return;

                organization.ManageAnonymousAssign = false;
                Storage<Organization>.Update(organization);

                this.gridView.DataSource = Storage<Organization>.Read(new Organization.ReadAnonymousAssignManager { });
                this.gridView.DataBind();
            }
        }


        protected void OnSave(object sender, EventArgs e) {
            int procent = 0;
            Int32.TryParse(this.txtEuronaReistraceProcent.Text, out procent);
            if (procent != 0) this.anonymniRegistrace.EuronaReistraceProcent = procent;
            else this.anonymniRegistrace.EuronaReistraceProcent = null;

            int maxPocetPrijatychNovacku = 0;
            Int32.TryParse(this.txtMaxPocetPrijetychNovacku.Text, out maxPocetPrijatychNovacku);

            this.anonymniRegistrace.ZobrazitVSeznamuNeomezene = this.cbNeomezene.Checked;
            this.anonymniRegistrace.MaxPocetPrijetychNovacku = maxPocetPrijatychNovacku == 0 ? null : (int?)maxPocetPrijatychNovacku;
            this.anonymniRegistrace.ZobrazitVSeznamuLimit = this.anonymniRegistraceLimit.ToString();
            Storage<AnonymniRegistraceEntity>.Update(this.anonymniRegistrace);

            Response.Redirect(Page.ResolveUrl("~/admin/default.aspx"));
        }

        protected void OnPridatAnonymousManager(object sender, EventArgs e) {
            Organization org = Storage<Organization>.ReadFirst(new Organization.ReadByCode { Code = this.txtCode.Text });
            if (org == null) return;

            org.ManageAnonymousAssign = true;
            Storage<Organization>.Update(org);

            this.gridView.DataSource = Storage<Organization>.Read(new Organization.ReadAnonymousAssignManager { });
            this.gridView.DataBind();
        }

        protected void OnPridatZobrazitVSeznamuLimit(object sender, EventArgs e) {
            int dayFrom = Convert.ToInt32(this.ddlZobrazitVSeznamuLimitFrom.SelectedValue);
            int dayTo = Convert.ToInt32(this.ddlZobrazitVSeznamuLimitTo.SelectedValue);

            int hoursFrom = 0;
            int minutesFrom = 0;
            int hoursTo = 0;
            int minutesTo = 0;
            Int32.TryParse(this.txtZobrazitVSeznamuLimitFromHodin.Text, out hoursFrom);
            Int32.TryParse(this.txtZobrazitVSeznamuLimitFromMinut.Text, out minutesFrom);
            Int32.TryParse(this.txtZobrazitVSeznamuLimitToHodin.Text, out hoursTo);
            Int32.TryParse(this.txtZobrazitVSeznamuLimitToMinut.Text, out minutesTo);

            string limitString = AnonymniRegistraceLimit.Limit.GetStringLimit(dayFrom, hoursFrom, minutesFrom, dayTo, hoursTo, minutesTo);
            this.anonymniRegistraceLimit.Add(limitString);

            this.anonymniRegistrace.ZobrazitVSeznamuLimit = this.anonymniRegistraceLimit.ToString();
            Storage<AnonymniRegistraceEntity>.Update(this.anonymniRegistrace);

            this.gridViewZobrazitVSeznamuLimit.DataSource = this.anonymniRegistraceLimit.GetData();
            this.gridViewZobrazitVSeznamuLimit.DataBind();
        }

        protected void OnCancel(object sender, EventArgs e) {
            Response.Redirect(Page.ResolveUrl("~/admin/default.aspx"));
        }
    }
}
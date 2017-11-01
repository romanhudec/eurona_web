using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CMS.Controls;
using AccountEntity = Eurona.DAL.Entities.AdvisorAccount;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
namespace Eurona.Controls {
    public class AdminAdvisorAccountsControl : CmsControl {
        private const string DELETE_COMMAND = "DELETE_ITEM";
        private const string EDIT_COMMAND = "EDIT_ITEM";
        private const string CREDIT_COMMAND = "CREDIT_ITEM";
        //private const string ROLES_COMMAND = "ROLES";

        private RadGrid gridView;

        public AdminAdvisorAccountsControl() {
        }

        public string IdentificationUrlFromat { get; set; }
        public string NewUrl { get; set; }
        public string EditUrlFormat { get; set; }
        public string AddCreditUrlFormat { get; set; }
        public string RolesUrlFormat { get; set; }

        //public virtual IStorage<T> GetStorage<T>() where T: CMS.Entities.Entity, new()
        //{
        //    return Storage<T>.Instance;
        //}

        public SortDirection SortDirection {
            get { return GetSession<SortDirection>("AdminAdvisorAccountsControl-SortDirection", SortDirection.Ascending); }
            set { SetSession<SortDirection>("AdminAdvisorAccountsControl-SortDirection", value); }
        }

        public string SortExpression {
            get { return GetSession<string>("AdminAdvisorAccountsControl-SortExpression", "Login"); }
            set { SetSession<string>("AdminAdvisorAccountsControl-SortExpression", value); }
        }

        public bool HideCredit { get; set; }


        public DateTime? FilterRegistrationDate {
            get { return GetState<DateTime?>("FilterRegistrationDate"); }
            set { SetState<DateTime?>("FilterRegistrationDate", value); }
        }

        public string FilterAdvisorCode {
            get { return GetState<String>("FilterAdvisorCode"); }
            set { SetState<String>("FilterAdvisorCode", value); }
        }

        public string FilterLogin {
            get { return GetState<String>("FilterLogin"); }
            set { SetState<String>("FilterLogin", value); }
        }

        public string FilterEmail {
            get { return GetState<String>("FilterEmail"); }
            set { SetState<String>("FilterEmail", value); }
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            gridView = CreateGridView();
            this.Controls.Add(gridView);

            GridViewDataBind(IsPostBack);
        }

        public void AddAccount() {
            string url = Page.ResolveUrl(NewUrl + "&" + base.BuildReturnUrlQueryParam());
            Response.Redirect(url);
        }

        private AccountEntity.ReadByFilter GetFilterValue() {
            AccountEntity.ReadByFilter filter = new AccountEntity.ReadByFilter();

            if (this.FilterRegistrationDate.HasValue) filter.RegistrationDate = this.FilterRegistrationDate.Value;
            if (!String.IsNullOrEmpty(this.FilterAdvisorCode)) filter.AdvisorCode = this.FilterAdvisorCode;
            if (!String.IsNullOrEmpty(this.FilterLogin)) filter.Login = this.FilterLogin;
            if (!String.IsNullOrEmpty(this.FilterEmail)) filter.Email = this.FilterEmail;
            return filter;
        }

        public void GridViewDataBind(bool bind) {
            AccountEntity.ReadByFilter filter = GetFilterValue();
            if (!filter.isEmpty()) {
                List<AccountEntity> accounts = Storage<AccountEntity>.Read(filter);
                var ordered = accounts.AsQueryable().OrderBy(SortExpression + " " + SortDirection);
                gridView.DataSource = ordered.ToList();
            }
            if (bind) gridView.DataBind();
        }

        private RadGrid CreateGridView() {
            RadGrid grid = new RadGrid();
            CMS.Utilities.RadGridUtilities.Localize(grid);
            grid.MasterTableView.DataKeyNames = new string[] { "Id" };

            grid.AllowAutomaticInserts = true;
            grid.AllowFilteringByColumn = true;
            grid.ShowStatusBar = false;
            grid.ShowGroupPanel = true;
            grid.GroupingEnabled = true;
            grid.GroupingSettings.ShowUnGroupButton = true;
            grid.GroupingSettings.CaseSensitive = false;
            grid.ClientSettings.AllowDragToGroup = true;
            grid.ClientSettings.AllowColumnsReorder = true;

            grid.MasterTableView.ShowHeader = true;
            grid.MasterTableView.ShowFooter = false;
            grid.MasterTableView.AllowPaging = true;
            grid.MasterTableView.PageSize = 25;
            grid.MasterTableView.PagerStyle.AlwaysVisible = true;
            grid.MasterTableView.AllowSorting = true;
            grid.MasterTableView.GridLines = GridLines.None;
            grid.MasterTableView.AutoGenerateColumns = false;

            grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;
            grid.MasterTableView.CommandItemSettings.AddNewRecordText = global::CMS.Resources.Controls.AdminAccountsControl_NewAccountButton_Text;

            GridBoundColumn gbc = new GridBoundColumn();
            gbc.DataField = "Created";
            gbc.HeaderText = "Datum registrace";
            gbc.SortExpression = "Created";
            gbc.AutoPostBackOnFilter = true;
            gbc.DataFormatString = "{0:d}";
            gbc.CurrentFilterFunction = GridKnownFunction.Contains;
            gbc.HeaderStyle.Width = Unit.Pixel(90);
            grid.Columns.Add(gbc);

            grid.Columns.Add(new GridBoundColumn {
                DataField = "AdvisorCode",
                HeaderText = "Reg. číslo",
                SortExpression = "AdvisorCode",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });

            if (string.IsNullOrEmpty(this.IdentificationUrlFromat)) {
                grid.Columns.Add(new GridBoundColumn {
                    DataField = "Login",
                    HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnLogin,
                    SortExpression = "Login",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
            } else {
                grid.Columns.Add(new GridHyperLinkColumn {
                    DataTextField = "Login",
                    HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnLogin,
                    SortExpression = "Login",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains,
                    DataNavigateUrlFields = new string[] { "Id" },
                    DataNavigateUrlFormatString = Page.ResolveUrl(this.IdentificationUrlFromat + "&" + base.BuildReturnUrlQueryParam()),
                });
            }

            grid.Columns.Add(new GridHyperLinkColumn {
                DataTextField = "Email",
                HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnEmail,
                SortExpression = "Email",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains,
                DataTextFormatString = "<a href=mailto:{0:g}>{0:g}</a>",
            });

            grid.Columns.Add(new GridCheckBoxColumn {
                DataField = "Enabled",
                HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnEnabled,
                SortExpression = "Enabled",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });

            grid.Columns.Add(new GridCheckBoxColumn {
                DataField = "Verified",
                HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnVerified,
                SortExpression = "Verified",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            /*
            grid.Columns.Add( new GridBoundColumn
            {
                    DataField = "Name",
                    HeaderText = "Jméno",
                    SortExpression = "Name",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
            } );

            grid.Columns.Add( new GridBoundColumn
            {
                    DataField = "Phone",
                    HeaderText = "Telefon",
                    SortExpression = "Phone",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
            } );
            grid.Columns.Add( new GridBoundColumn
            {
                    DataField = "Mobile",
                    HeaderText = "Mobil",
                    SortExpression = "Mobile",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
            } );
            grid.Columns.Add( new GridBoundColumn
            {
                    DataField = "RegisteredAddress",
                    HeaderText = "Adresa sídla",
                    SortExpression = "RegisteredAddress",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
            } );
            grid.Columns.Add( new GridBoundColumn
            {
                    DataField = "CorrespondenceAddress",
                    HeaderText = "Kores. adresa",
                    SortExpression = "CorrespondenceAddress",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
            } );
             * */

            GridButtonColumn btnEdit = new GridButtonColumn();
            btnEdit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImage");
            btnEdit.Text = global::CMS.Resources.Controls.GridView_ToolTip_EditItem;
            btnEdit.ButtonType = GridButtonColumnType.ImageButton;
            btnEdit.CommandName = EDIT_COMMAND;
            grid.MasterTableView.Columns.Add(btnEdit);

            GridButtonColumn btnDelete = new GridButtonColumn();
            btnDelete.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImage");
            btnDelete.Text = global::CMS.Resources.Controls.GridView_ToolTip_DeleteItem;
            btnDelete.ButtonType = GridButtonColumnType.ImageButton;
            btnDelete.ConfirmTitle = global::CMS.Resources.Controls.DeleteItemQuestion;
            btnDelete.ConfirmText = global::CMS.Resources.Controls.DeleteItemQuestion;
            btnDelete.CommandName = DELETE_COMMAND;
            grid.MasterTableView.Columns.Add(btnDelete);

            grid.ItemCommand += OnRowCommand;

            return grid;
        }

        void OnRowCommand(object sender, GridCommandEventArgs e) {
            if (e.CommandName == RadGrid.InitInsertCommandName) OnNewCommand(sender, e);
            if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
            if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
            if (e.CommandName == CREDIT_COMMAND) OnAddCreditCommand(sender, e);
            //if ( e.CommandName == ROLES_COMMAND ) OnRolesCommand( sender, e );
        }
        private void OnNewCommand(object sender, GridCommandEventArgs e) {
            string url = Page.ResolveUrl(NewUrl + "&" + base.BuildReturnUrlQueryParam());
            Response.Redirect(url);
        }
        private void OnEditCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int accountId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            Response.Redirect(String.Format(EditUrlFormat, accountId) + "&" + base.BuildReturnUrlQueryParam());
        }

        private void OnDeleteCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int accountId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = accountId });
            Storage<AccountEntity>.Delete(account);
            if (account.TVD_Id.HasValue) {
                OrganizationEntity organization = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByTVDId { TVD_Id = account.TVD_Id.Value });
                if (organization != null) Storage<OrganizationEntity>.Delete(organization);
            }
            GridViewDataBind(true);
        }
        private void OnAddCreditCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int accountId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            Response.Redirect(String.Format(AddCreditUrlFormat, accountId) + "&" + base.BuildReturnUrlQueryParam());
        }
    }
}

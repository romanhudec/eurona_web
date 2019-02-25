using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CMS.Controls;
using AccountEntity = Eurona.DAL.Entities.Account;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;

namespace Eurona.Controls
{
	public class AdminAccountsControl : CmsControl
	{
        public delegate void DataLoadHandler(RadGrid gridView, bool bind);
        public event DataLoadHandler OnDataLoad;

        private RadGrid grid = null;

		private const string DELETE_COMMAND = "DELETE_ITEM";
		private const string EDIT_COMMAND = "EDIT_ITEM";
		private const string CREDIT_COMMAND = "CREDIT_ITEM";
        private const string LOGIN_COMMAND = "LOGIN_ITEM";
		//private const string ROLES_COMMAND = "ROLES";

		protected RadGrid gridView;

		public AdminAccountsControl()
		{
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

		public SortDirection SortDirection
		{
			get { return GetSession<SortDirection>("AdminAccountsControl-SortDirection", SortDirection.Ascending); }
			set { SetSession<SortDirection>("AdminAccountsControl-SortDirection", value); }
		}

		public string SortExpression
		{
			get { return GetSession<string>("AdminAccountsControl-SortExpression", "Login"); }
			set { SetSession<string>("AdminAccountsControl-SortExpression", value); }
		}

		public bool HideCredit { get; set; }

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			gridView = CreateGridView();
			this.Controls.Add(gridView);

			GridViewDataBind(!IsPostBack);
		}

		protected void GridViewDataBind(bool bind)
		{
            if (OnDataLoad != null) {
                OnDataLoad(gridView, bind);
                return;
            }
			List<AccountEntity> accounts = Storage<AccountEntity>.Read();
			var ordered = accounts.AsQueryable().OrderBy(SortExpression + " " + SortDirection);
			gridView.DataSource = ordered.ToList();
			if (bind) gridView.DataBind();
		}

        public void ExportToExcel(){
            if (grid == null) return;
            grid.ExportSettings.IgnorePaging = true;
            grid.MasterTableView.ExportToExcel();
        }
		private RadGrid CreateGridView()
		{
			grid = new RadGrid();
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

			grid.Columns.Add(new GridBoundColumn
			{
				DataField = "Created",
				HeaderText = "Datum registrace",
				SortExpression = "Created",
				AutoPostBackOnFilter = true,
				DataFormatString = "{0:d}",
				CurrentFilterFunction = GridKnownFunction.Contains
			});

			if (string.IsNullOrEmpty(this.IdentificationUrlFromat))
			{
				grid.Columns.Add(new GridBoundColumn
				{
					DataField = "Login",
					HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnLogin,
					SortExpression = "Login",
					AutoPostBackOnFilter = true,
					CurrentFilterFunction = GridKnownFunction.Contains,
				});
			}
			else
			{
				grid.Columns.Add(new GridHyperLinkColumn
				{
					DataTextField = "Login",
					HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnLogin,
					SortExpression = "Login",
					AutoPostBackOnFilter = true,
					CurrentFilterFunction = GridKnownFunction.Contains,
					DataNavigateUrlFields = new string[] { "Id" },
					DataNavigateUrlFormatString = Page.ResolveUrl(this.IdentificationUrlFromat + "&" + base.BuildReturnUrlQueryParam()),
				});
			}
			grid.Columns[0].HeaderStyle.Width = Unit.Pixel(80);

			grid.Columns.Add(new GridHyperLinkColumn
			{
				DataTextField = "Email",
				HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnEmail,
				SortExpression = "Email",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains,
				DataTextFormatString = "<a href=mailto:{0:g}>{0:g}</a>",
			});
			grid.Columns[1].HeaderStyle.Width = Unit.Pixel(120);

			grid.Columns.Add(new GridCheckBoxColumn
			{
				DataField = "Enabled",
				HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnEnabled,
				SortExpression = "Enabled",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});
			grid.Columns[2].HeaderStyle.Width = Unit.Pixel(60);

			grid.Columns.Add(new GridCheckBoxColumn
			{
				DataField = "Verified",
				HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnVerified,
				SortExpression = "Verified",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});
			grid.Columns[3].HeaderStyle.Width = Unit.Pixel(60);

            grid.Columns.Add(new GridBoundColumn {
                DataField = "EmailVerified",
                HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnEmailVerified,
                SortExpression = "EmailVerified",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns[4].HeaderStyle.Width = Unit.Pixel(60);

			grid.Columns.Add(new GridBoundColumn
			{
				DataField = "RoleStringDisplay",
				HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnRoles,
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});
			grid.Columns[5].ItemStyle.Wrap = true;

			if (!HideCredit)
			{
				grid.Columns.Add(new GridBoundColumn
				{
					DataField = "Credit",
					HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnCredit,
					AutoPostBackOnFilter = true,
					CurrentFilterFunction = GridKnownFunction.Contains
				});
			}

            if (Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR)) {
                GridButtonColumn btnLoginAs = new GridButtonColumn();
                btnLoginAs.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
                btnLoginAs.ImageUrl = ConfigValue("CMS:VerifyButtonImageD");
                btnLoginAs.Text = "Přihlásit jako uživatel";
                btnLoginAs.ButtonType = GridButtonColumnType.ImageButton;
                btnLoginAs.CommandName = LOGIN_COMMAND;
                grid.MasterTableView.Columns.Add(btnLoginAs);
            }


            if (!string.IsNullOrEmpty(this.AddCreditUrlFormat)) {
                GridButtonColumn btnAddCredit = new GridButtonColumn();
                btnAddCredit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
                btnAddCredit.ImageUrl = ConfigValue("CMS:EuroButtonImage");
                btnAddCredit.Text = "Ruční zadání bonusových kreditů uživateli";
                btnAddCredit.ButtonType = GridButtonColumnType.ImageButton;
                btnAddCredit.CommandName = CREDIT_COMMAND;
                grid.MasterTableView.Columns.Add(btnAddCredit);
            }

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

		void OnRowCommand(object sender, GridCommandEventArgs e)
		{
			if (e.CommandName == RadGrid.InitInsertCommandName) OnNewCommand(sender, e);
			if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
			if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
			if (e.CommandName == CREDIT_COMMAND) OnAddCreditCommand(sender, e);
            if (e.CommandName == LOGIN_COMMAND) OnLoginAsCommand(sender, e);
		}
        private void OnLoginAsCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int accountId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = accountId });
            Security.Login(account, true);
        }
		private void OnNewCommand(object sender, GridCommandEventArgs e)
		{
			string url = Page.ResolveUrl(NewUrl + "&" + base.BuildReturnUrlQueryParam());
			Response.Redirect(url);
		}
		private void OnEditCommand(object sender, GridCommandEventArgs e)
		{
			GridDataItem dataItem = (GridDataItem)e.Item;
			int accountId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
			Response.Redirect(String.Format(EditUrlFormat, accountId) + "&" + base.BuildReturnUrlQueryParam());
		}

		private void OnDeleteCommand(object sender, GridCommandEventArgs e)
		{
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
		private void OnAddCreditCommand(object sender, GridCommandEventArgs e)
		{
			GridDataItem dataItem = (GridDataItem)e.Item;
			int accountId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
			Response.Redirect(String.Format(AddCreditUrlFormat, accountId) + "&" + base.BuildReturnUrlQueryParam());
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CMS.Controls;
using AccountEntity = Eurona.DAL.Entities.AdvisorAccount;

namespace Eurona.Controls
{
	public class AdminNewsletterAccountsControl : CmsControl
	{
		private RadGrid gridView;

		public AdminNewsletterAccountsControl()
		{
		}

		public string IdentificationUrlFromat { get; set; }
		public string NewUrl { get; set; }
		public string EditUrlFormat { get; set; }
		public string AddCreditUrlFormat { get; set; }
		public string RolesUrlFormat { get; set; }

		public SortDirection SortDirection
		{
			get { return GetSession<SortDirection>("AdminNewsletterAccountsControl-SortDirection", SortDirection.Ascending); }
			set { SetSession<SortDirection>("AdminNewsletterAccountsControl-SortDirection", value); }
		}

		public string SortExpression
		{
			get { return GetSession<string>("AdminNewsletterAccountsControl-SortExpression", "Login"); }
			set { SetSession<string>("AdminNewsletterAccountsControl-SortExpression", value); }
		}

		public bool HideCredit { get; set; }

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			gridView = CreateGridView();
			this.Controls.Add(gridView);

			GridViewDataBind(!IsPostBack);
		}

		public RadGrid GetGridView()
		{
			return this.gridView;
		}

		private void GridViewDataBind(bool bind)
		{
			List<AccountEntity> accounts = Storage<AccountEntity>.Read(new AccountEntity.ReadNewsletter { });
			var ordered = accounts.AsQueryable().OrderBy(SortExpression + " " + SortDirection);
			gridView.DataSource = ordered.ToList();
			if (bind) gridView.DataBind();
		}

		private RadGrid CreateGridView()
		{
			RadGrid grid = new RadGrid();
			CMS.Utilities.RadGridUtilities.Localize(grid);
			grid.MasterTableView.DataKeyNames = new string[] { "Id" };

			grid.AllowAutomaticInserts = false;
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


			GridBoundColumn gbc = new GridBoundColumn();
			gbc.DataField = "Created";
			gbc.HeaderText = "Datum registrace";
			gbc.SortExpression = "Created";
			gbc.AutoPostBackOnFilter = true;
			gbc.DataFormatString = "{0:d}";
			gbc.CurrentFilterFunction = GridKnownFunction.Contains;
			gbc.HeaderStyle.Width = Unit.Pixel(90);
			grid.Columns.Add(gbc);

			grid.Columns.Add(new GridBoundColumn
			{
				DataField = "AdvisorCode",
				HeaderText = "Reg. číslo",
				SortExpression = "AdvisorCode",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});

			grid.Columns.Add(new GridBoundColumn
			{
				DataField = "Name",
				HeaderText = "Jméno",
				SortExpression = "Name",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});

			grid.Columns.Add(new GridBoundColumn
			{
				DataField = "Locale",
				HeaderText = "Jazyk",
				SortExpression = "Locale",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});

			grid.Columns.Add(new GridHyperLinkColumn
			{
				DataTextField = "Email",
				HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnEmail,
				SortExpression = "Email",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains,
				DataTextFormatString = "<a href=mailto:{0:g}>{0:g}</a>",
			});

			grid.Columns.Add(new GridCheckBoxColumn
			{
				DataField = "ZasilaniTiskovin",
				HeaderText = "Zasílání tiskovin",
				SortExpression = "ZasilaniTiskovin",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});

			grid.Columns.Add(new GridCheckBoxColumn
			{
				DataField = "ZasilaniNewsletter",
				HeaderText = "Zasílání el.materiálů",
				SortExpression = "ZasilaniNewsletter",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});

			grid.Columns.Add(new GridCheckBoxColumn
			{
				DataField = "ZasilaniKatalogu",
				HeaderText = "Zasílání katalogu",
				SortExpression = "ZasilaniKatalogu",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});
			return grid;
		}
	}
}

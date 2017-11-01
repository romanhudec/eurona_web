using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ForumThreadEntity = CMS.Entities.ForumThread;
using Telerik.Web.UI;

namespace CMS.Controls.Forum
{
	public class AdminForumThreadsControl : CmsControl
	{
		private const string DELETE_COMMAND = "DELETE_ITEM";
		private const string EDIT_COMMAND = "EDIT_ITEM";

		private RadGrid dataGrid = null;

		public SortDirection SortDirection
		{
			get { return GetState<SortDirection>("AdminForumThreadsControl-SortDirection", SortDirection.Descending); }
			set { SetState<SortDirection>("AdminForumThreadsControl-SortDirection", value); }
		}
		public string SortExpression
		{
			get { return GetState<string>("AdminForumThreadsControl-SortExpression"); }
			set { SetState<string>("AdminForumThreadsControl-SortExpression", value); }
		}

		#region Protected overrides
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.dataGrid = CreateGridControl();
			this.Controls.Add(this.dataGrid);

			//Binding
			this.dataGrid.DataSource = GetDataGridData();
			if (!IsPostBack)
				this.dataGrid.DataBind();

		}
		#endregion

		public string NewUrl { get; set; }
		public string EditUrlFormat { get; set; }

		/// <summary>
		/// Vytvori DataGrid control s pozadovanymi stlpcami a 
		/// pripravenym bindingom stlpcou.
		/// </summary>
		private RadGrid CreateGridControl()
		{
			RadGrid grid = new RadGrid();
			CMS.Utilities.RadGridUtilities.Localize(grid);
			grid.MasterTableView.DataKeyNames = new string[] { "Id" };

			grid.AllowAutomaticInserts = true;
			grid.AllowFilteringByColumn = true;
			grid.ShowStatusBar = false;
			grid.ShowGroupPanel = true;
			grid.GroupingEnabled = true;
			grid.GroupingSettings.ShowUnGroupButton = true;
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
			grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminForumThreadsControl_NewForumThread;

			GridBoundColumn bf = new GridBoundColumn();
			bf.DataField = "Name";
			bf.HeaderText = Resources.Controls.AdminForumThreadsControl_ColumnName;
			bf.SortExpression = "Name";
			bf.AutoPostBackOnFilter = true;
			bf.CurrentFilterFunction = GridKnownFunction.Contains;
			grid.Columns.Add(bf);

			GridIconColumn ic = new GridIconColumn();
			ic.DataImageUrlFields = new string[] { "Icon" };
			ic.ImageWidth = Unit.Pixel(16);
			ic.ImageHeight = Unit.Pixel(16);
			ic.HeaderText = Resources.Controls.AdminForumThreadsControl_ColumnIcon;
			ic.ShowFilterIcon = false;
			ic.AllowFiltering = false;
			grid.Columns.Add(ic);

			GridCheckBoxColumn cbc = new GridCheckBoxColumn();
			cbc.DataField = "Locked";
			cbc.HeaderText = Resources.Controls.AdminForumThreadsControl_ColumnLocked;
			cbc.SortExpression = "Locked";
			cbc.AutoPostBackOnFilter = true;
			cbc.CurrentFilterFunction = GridKnownFunction.Contains;
			grid.Columns.Add(cbc);

			/* Oxy: ked mam 10roli tak nevidim tlacidlo edit, pretoze mi vsetko zabera zoznam roli...
			bf = new GridBoundColumn();
			bf.DataField = "VisibleForRole";
			bf.HeaderText = Resources.Controls.AdminForumThreadsControl_ColumnVisibleForRole;
			bf.SortExpression = "VisibleForRole";
			bf.AutoPostBackOnFilter = true;
			bf.CurrentFilterFunction = GridKnownFunction.Contains;
			grid.Columns.Add( bf );

			bf = new GridBoundColumn();
			bf.DataField = "EditableForRole";
			bf.HeaderText = Resources.Controls.AdminForumThreadsControl_ColumnEditableForRole;
			bf.SortExpression = "EditableForRole";
			bf.AutoPostBackOnFilter = true;
			bf.CurrentFilterFunction = GridKnownFunction.Contains;
			grid.Columns.Add( bf );
			*/

			bf = new GridBoundColumn();
			bf.DataField = "ForumsCount";
			bf.HeaderText = Resources.Controls.AdminForumThreadsControl_ColumnForumsCount;
			bf.SortExpression = "ForumsCount";
			bf.AutoPostBackOnFilter = true;
			bf.CurrentFilterFunction = GridKnownFunction.Contains;
			grid.Columns.Add(bf);

			bf = new GridBoundColumn();
			bf.DataField = "ForumPostCount";
			bf.HeaderText = Resources.Controls.AdminForumThreadsControl_ColumnForumPostCount;
			bf.SortExpression = "ForumPostCount";
			bf.AutoPostBackOnFilter = true;
			bf.CurrentFilterFunction = GridKnownFunction.Contains;
			grid.Columns.Add(bf);

			GridButtonColumn btnEdit = new GridButtonColumn();
			btnEdit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
			btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImage");
			btnEdit.Text = Resources.Controls.GridView_ToolTip_EditItem;
			btnEdit.ButtonType = GridButtonColumnType.ImageButton;
			btnEdit.CommandName = EDIT_COMMAND;
			grid.MasterTableView.Columns.Add(btnEdit);

			GridButtonColumn btnDelete = new GridButtonColumn();
			btnDelete.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
			btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImage");
			btnDelete.Text = Resources.Controls.GridView_ToolTip_DeleteItem;
			btnDelete.ButtonType = GridButtonColumnType.ImageButton;
			btnDelete.ConfirmTitle = Resources.Controls.DeleteItemQuestion;
			btnDelete.ConfirmText = Resources.Controls.DeleteItemQuestion;
			btnDelete.CommandName = DELETE_COMMAND;
			grid.MasterTableView.Columns.Add(btnDelete);

			grid.ItemCommand += OnRowCommand;
			grid.ItemDataBound += OnRowDataBound;

			return grid;
		}

		private List<ForumThreadEntity> GetDataGridData()
		{
			List<ForumThreadEntity> list = Storage<ForumThreadEntity>.Read();
			SortDirection previous = SortDirection;
			string sortExpression = String.IsNullOrEmpty(SortExpression) ? "Name" : SortExpression;
			var ordered = list.AsQueryable().OrderBy(sortExpression + " " + (previous == SortDirection.Ascending ? "ascending" : "descending"));
			return ordered.ToList();
		}

		#region Event handlers
		void OnRowCommand(object sender, GridCommandEventArgs e)
		{
			if (e.CommandName == RadGrid.InitInsertCommandName) OnNewCommand(sender, e);
			if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
			if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
		}
		private void OnNewCommand(object sender, GridCommandEventArgs e)
		{
			string url = Page.ResolveUrl(string.Format("{0}?{1}", this.NewUrl, base.BuildReturnUrlQueryParam()));
			Response.Redirect(url);
		}
		private void OnEditCommand(object sender, GridCommandEventArgs e)
		{
			GridDataItem dataItem = (GridDataItem)e.Item;
			int id = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);

			string url = Page.ResolveUrl(string.Format(EditUrlFormat, id) + "&" + base.BuildReturnUrlQueryParam());
			Response.Redirect(url);
		}
		private void OnDeleteCommand(object sender, GridCommandEventArgs e)
		{
			GridDataItem dataItem = (GridDataItem)e.Item;
			int articleId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);

			ForumThreadEntity forumThread = Storage<ForumThreadEntity>.ReadFirst(new ForumThreadEntity.ReadById { ForumThreadId = articleId });
			Storage<ForumThreadEntity>.Delete(forumThread);

			this.dataGrid.DataSource = GetDataGridData();
			this.dataGrid.DataBind();
		}

		void OnRowDataBound(object sender, GridItemEventArgs e)
		{
			if (e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem)
				return;
		}
		#endregion
	}
}

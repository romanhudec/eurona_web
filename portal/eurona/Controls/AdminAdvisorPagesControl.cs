using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using AdvisorPageEntity = Eurona.DAL.Entities.AdvisorPage;
using Telerik.Web.UI;
using CMS.Controls;

namespace Eurona.Controls
{
    public class AdminAdvisorPagesControl : CmsControl
    {
        private const string DELETE_COMMAND = "DELETE_ITEM";
        private const string EDIT_COMMAND = "EDIT_ITEM";
        private const string EDIT_CONTENT_COMMAND = "EDIT_CONTENT";

        private RadGrid gridView;

        public string EditUrlFormat { get; set; }
        public string EditContentUrlFormat { get; set; }
        public string NewUrl { get; set; }

        public SortDirection SortDirection
        {
            get { return GetSession<SortDirection>("AdminAdvisorPagesControl-SortDirection", SortDirection.Ascending); }
            set { SetSession<SortDirection>("AdminAdvisorPagesControl-SortDirection", value); }
        }

        public string SortExpression
        {
            get { return GetSession<string>("AdminAdvisorPagesControl-SortExpression", "Title"); }
            set { SetSession<string>("AdminAdvisorPagesControl-SortExpression", value); }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            gridView = CreateGridView();
            this.Controls.Add(gridView);
            GridViewDataBind(!IsPostBack);
        }

        private void GridViewDataBind(bool bind)
        {
            List<AdvisorPageEntity> pages = Storage<AdvisorPageEntity>.Read(new AdvisorPageEntity.ReadContentPages());

            var ordered = pages.AsQueryable().OrderBy(SortExpression + " " + SortDirection);
            gridView.DataSource = ordered.ToList();

            if (bind) gridView.DataBind();
        }

        private RadGrid CreateGridView()
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
            grid.MasterTableView.CommandItemSettings.AddNewRecordText = CMS.Resources.Controls.AdminPagesControl_NewPageButton_Text;

            GridHyperLinkColumnEx ghc = new GridHyperLinkColumnEx(this.Page);
            ghc.DataTextField = "Title";
            ghc.DataNavigateUrlFields = new string[] { "Alias" };
            ghc.HeaderText = CMS.Resources.Controls.AdminPagesControl_ColumnTitle;
            ghc.SortExpression = "Title";
            ghc.AutoPostBackOnFilter = true;
            ghc.CurrentFilterFunction = GridKnownFunction.Contains;
            grid.Columns.Add(ghc);

            grid.Columns.Add(new GridBoundColumn
            {
                DataField = "Name",
                HeaderText = CMS.Resources.Controls.AdminPagesControl_ColumnName,
                SortExpression = "Name",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns.Add(new GridBoundColumn
            {
                DataField = "Email",
                HeaderText = Resources.Strings.AdminPagesControl_ColumnEmail,
                SortExpression = "Email",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns.Add(new GridBoundColumn
            {
                DataField = "OrganizationCode",
                HeaderText = Resources.Strings.AdminPagesControl_ColumnAdvisor,
                SortExpression = "OrganizationCode",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns.Add(new GridCheckBoxColumn
            {
                DataField = "Blocked",
                HeaderText = Resources.Strings.AdminPagesControl_ColumnBlocked,
                SortExpression = "Blocked",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            GridButtonColumn btnEdit = new GridButtonColumn();
            btnEdit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImage");
            btnEdit.Text = CMS.Resources.Controls.GridView_ToolTip_EditItem;
            btnEdit.ButtonType = GridButtonColumnType.ImageButton;
            btnEdit.CommandName = EDIT_COMMAND;
            grid.MasterTableView.Columns.Add(btnEdit);

            GridButtonColumn btnEditContent = new GridButtonColumn();
            btnEditContent.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnEditContent.ImageUrl = ConfigValue("CMS:EditContentButtonImage");
            btnEditContent.Text = CMS.Resources.Controls.GridView_ToolTip_EditContent;
            btnEditContent.ButtonType = GridButtonColumnType.ImageButton;
            btnEditContent.CommandName = EDIT_CONTENT_COMMAND;
            grid.MasterTableView.Columns.Add(btnEditContent);

            GridButtonColumn btnDelete = new GridButtonColumn();
            btnDelete.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImage");
            btnDelete.Text = CMS.Resources.Controls.GridView_ToolTip_DeleteItem;
            btnDelete.ButtonType = GridButtonColumnType.ImageButton;
            btnDelete.ConfirmTitle = CMS.Resources.Controls.DeleteItemQuestion;
            btnDelete.ConfirmText = CMS.Resources.Controls.DeleteItemQuestion;
            btnDelete.CommandName = DELETE_COMMAND;
            grid.MasterTableView.Columns.Add(btnDelete);

            grid.ItemCommand += OnRowCommand;
            grid.ItemDataBound += OnRowDataBound;

            return grid;
        }

        void OnRowDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem)
                return;

            AdvisorPageEntity page = e.Item.DataItem as AdvisorPageEntity;

            int nameIndex = 2;//Zacina vzdy indexom 2
            int deleteIndex = e.Item.Cells.Count - 1;
            ImageButton btnDelete = (e.Item.Cells[deleteIndex].Controls[0] as ImageButton);

            //Systemove pages
            if (page.Id < 0)
            {
                e.Item.Cells[nameIndex].Font.Bold = true;
                btnDelete.Enabled = false;
                btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImageD");
            }
        }

        void OnRowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.InitInsertCommandName) OnNewCommand(sender, e);
            if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
            if (e.CommandName == EDIT_CONTENT_COMMAND) OnEditContentCommand(sender, e);
            if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
        }
        private void OnNewCommand(object sender, GridCommandEventArgs e)
        {
            string url = Page.ResolveUrl(NewUrl + "&" + base.BuildReturnUrlQueryParam());
            Response.Redirect(url);
        }
        private void OnEditCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int advisorPageId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);

            Response.Redirect(String.Format(EditUrlFormat, advisorPageId) + "&" + base.BuildReturnUrlQueryParam());
        }

        private void OnEditContentCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int advisorPageId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);

            Response.Redirect(String.Format(EditContentUrlFormat, advisorPageId) + "&" + base.BuildReturnUrlQueryParam());
        }

        private void OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int advisorPageId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            if (advisorPageId < 0) return; //Systemove stranky sa nemazu.

            AdvisorPageEntity page = Storage<AdvisorPageEntity>.ReadFirst(new AdvisorPageEntity.ReadById { AdvisorPageId = advisorPageId });

            Storage<AdvisorPageEntity>.Delete(page);
            GridViewDataBind(true);
        }

        public class GridHyperLinkColumnEx : GridHyperLinkColumn
        {
            private System.Web.UI.Page page = null;
            public GridHyperLinkColumnEx(System.Web.UI.Page page)
            {
                this.page = page;
            }

            protected override string FormatDataNavigateUrlValue(object[] dataUrlValues)
            {
                for (int i = 0; i < dataUrlValues.Length; i++)
                {
                    if (dataUrlValues[i] is string) dataUrlValues[i] = page.ResolveUrl(dataUrlValues[i].ToString());
                }
                return base.FormatDataNavigateUrlValue(dataUrlValues);
            }

        }


    }
}

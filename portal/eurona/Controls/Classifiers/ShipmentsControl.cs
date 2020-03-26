﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Controls;
using SHP.Entities.Classifiers;
using System.Web.UI.HtmlControls;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using Telerik.Web.UI;

namespace Eurona.Controls.Classifiers {
    public class ShipmentsControl : CMS.Controls.CmsControl {
        protected const string DELETE_COMMAND = "DELETE_ITEM";
        protected const string EDIT_COMMAND = "EDIT_ITEM";

        protected RadGrid dataGrid = null;

        public SortDirection SortDirection {
            get { return GetSession<SortDirection>("ShipmentsControl-SortDirection", SortDirection.Ascending); }
            set { SetSession<SortDirection>("ShipmentsControl-SortDirection", value); }
        }
        public string SortExpression {
            get { return GetSession<string>("ShipmentsControl-SortExpression"); }
            set { SetSession<string>("ShipmentsControl-SortExpression", value); }
        }

        public ShipmentsControl() {
        }

        #region Protected overrides
        protected override void CreateChildControls() {
            base.CreateChildControls();

            Control clsControl = CreateClassifiersControl();
            if (clsControl != null) this.Controls.Add(clsControl);
            else return;

            //Binding
            this.dataGrid.DataSource = GetDataGridData();
            if (!IsPostBack) {
                this.dataGrid.MasterTableView.DataKeyNames = new string[] { "Id" };
                this.dataGrid.DataBind();
            }

        }
        #endregion

        protected ShipmentEntity ReadFirst(object criteria) {
            return Storage<ShipmentEntity>.Read(criteria)[0];
        }

        public string NewUrl { get; set; }
        public string EditUrlFormat { get; set; }

        /// <summary>
        /// Vytvori Control zoznamu poloziek ciselnika
        /// </summary>
        protected virtual Control CreateClassifiersControl() {
            HtmlGenericControl div = new HtmlGenericControl("div");

            //DataGrid
            this.dataGrid = CreateGridControl();
            div.Controls.Add(this.dataGrid);
            return div;
        }

        /// <summary>
        /// Vytvori DataGrid control s pozadovanymi stlpcami a 
        /// pripravenym bindingom stlpcou.
        /// </summary>
        protected virtual RadGrid CreateGridControl() {
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

            //grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;
            //grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.ClassifiersControl_AddItem;

            GridBoundColumn bf = new GridBoundColumn();
            bf.DataField = "Name";
            bf.HeaderText = SHP.Resources.Controls.ClassifiersControl_ColumnName;
            bf.SortExpression = "Name";
            bf.AutoPostBackOnFilter = true;
            bf.CurrentFilterFunction = GridKnownFunction.Contains;
            grid.Columns.Add(bf);


            bf = new GridBoundColumn();
            bf.DataField = "Order";
            bf.HeaderText = "Pořadí";
            bf.SortExpression = "Order";
            bf.AutoPostBackOnFilter = true;
            bf.CurrentFilterFunction = GridKnownFunction.Contains;
            grid.Columns.Add(bf);

            bf = new GridBoundColumn();
            bf.DataField = "Locale";
            bf.HeaderText = "Krajina";
            bf.SortExpression = "Locale";
            bf.AutoPostBackOnFilter = true;
            bf.CurrentFilterFunction = GridKnownFunction.Contains;
            grid.Columns.Add(bf);

            GridCheckBoxColumn cbf = new GridCheckBoxColumn();
            cbf.DataField = "PlatbaKartou";
            cbf.HeaderText = "Platba kartou";
            cbf.SortExpression = "PlatbaKartou";
            cbf.AutoPostBackOnFilter = true;
            cbf.CurrentFilterFunction = GridKnownFunction.Contains;
            grid.Columns.Add(cbf);


            cbf = new GridCheckBoxColumn();
            cbf.DataField = "PlatbaDobirkou";
            cbf.HeaderText = "Platba dobírkou";
            cbf.SortExpression = "PlatbaDobirkou";
            cbf.AutoPostBackOnFilter = true;
            cbf.CurrentFilterFunction = GridKnownFunction.Contains;
            grid.Columns.Add(cbf);

            cbf = new GridCheckBoxColumn();
            cbf.DataField = "Hide";
            cbf.HeaderText = "Skrytí";
            cbf.SortExpression = "Hide";
            cbf.AutoPostBackOnFilter = true;
            cbf.CurrentFilterFunction = GridKnownFunction.Contains;
            grid.Columns.Add(cbf);

            GridButtonColumn btnEdit = new GridButtonColumn();
            btnEdit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImage");
            btnEdit.Text = SHP.Resources.Controls.GridView_ToolTip_EditItem;
            btnEdit.ButtonType = GridButtonColumnType.ImageButton;
            btnEdit.CommandName = EDIT_COMMAND;
            grid.MasterTableView.Columns.Add(btnEdit);

            grid.ItemCommand += OnRowCommand;

            return grid;
        }

        protected virtual List<ShipmentEntity> GetDataGridData() {
            List<ShipmentEntity> list = Storage<ShipmentEntity>.Read(new ShipmentEntity.Read4AllLocales());

            SortDirection previous = SortDirection;
            string sortExpression = String.IsNullOrEmpty(SortExpression) ? "Order" : SortExpression;
            var ordered = list.AsQueryable().OrderBy(sortExpression + " " + (previous == SortDirection.Ascending ? "ascending" : "descending"));
            return ordered.ToList();
        }

        #region Event handlers
        protected void OnRowCommand(object sender, GridCommandEventArgs e) {
            if (e.CommandName == RadGrid.InitInsertCommandName) OnNewCommand(sender, e);
            if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
            if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
        }
        protected void OnNewCommand(object sender, GridCommandEventArgs e) {
            string url = Page.ResolveUrl(string.Format("{0}{1}{2}", this.NewUrl,
                    (this.NewUrl.Contains("?") ? "&" : "?"),
                    base.BuildReturnUrlQueryParam()));

            Response.Redirect(url);
        }
        protected void OnEditCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int id = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);

            string url = Page.ResolveUrl(string.Format(EditUrlFormat, id) + "&" + base.BuildReturnUrlQueryParam());
            Response.Redirect(url);
        }

        protected void OnDeleteCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int id = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);

            ShipmentEntity classifier = ReadFirst(new ShipmentEntity.ReadById { Id = id });
            Storage<ShipmentEntity>.Delete(classifier);

            this.dataGrid.DataSource = GetDataGridData();
            this.dataGrid.DataBind();
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using CMS.Controls;
using OrderEntity = Eurona.DAL.Entities.Order;
using CMS;
using System.Web.UI;
using Telerik.Web.UI;
using SHP.Controls;
using Eurona.DAL.Entities;

namespace Eurona.User.Advisor {
    public class AdminArchivedOrdersControl : CmsControl {
        private const string EDIT_COMMAND = "EDIT_ITEM";
        private const string PAY_COMMAND = "PAY_ITEM";

        //private TextBox txtOrderNumber = null;
        //private DropDownList ddlOrderStatus = null;
        //private DropDownList ddlShipment = null;

        private RadGrid gridView;
        public string EditUrlFormat { get; set; }
        public string PayUrlFormat { get; set; }
        public string UserUrlFormat { get; set; }

        public SortDirection SortDirection {
            get { return GetSession<SortDirection>("AdminArchivedOrdersControl-SortDirection", SortDirection.Descending); }
            set { SetSession<SortDirection>("AdminArchivedOrdersControl-SortDirection", value); }
        }

        public string SortExpression {
            get { return GetSession<string>("AdminArchivedOrdersControl-SortExpression", "OrderDate"); }
            set { SetSession<string>("AdminArchivedOrdersControl-SortExpression", value); }
        }


        protected override void CreateChildControls() {
            base.CreateChildControls();

            Control filter = CreateFilterControl();
            this.Controls.Add(filter); ;

            gridView = CreateGridView();
            this.Controls.Add(gridView);

            GridViewDataBind(!IsPostBack);
        }

        private Table CreateFilterControl() {
            Table table = new Table();
            table.Width = Unit.Percentage(100);
            TableRow row = new TableRow();

            ////--Cislo objednavky
            //TableCell cell = new TableCell();
            //this.txtOrderNumber = new TextBox();
            //cell.Controls.Add( new LiteralControl( SHP.Resources.Controls.AdminOrdersControl_OrderNumber ) );
            //cell.Controls.Add( this.txtOrderNumber );
            //row.Cells.Add( cell );

            ////--Stav objednavky
            //cell = new TableCell();
            //this.ddlOrderStatus = new DropDownList();
            //cell.Controls.Add( new LiteralControl( SHP.Resources.Controls.AdminOrdersControl_OrderStatus ) );
            //cell.Controls.Add( this.ddlOrderStatus );
            //row.Cells.Add( cell );

            ////--Preprava
            //cell = new TableCell();
            //this.ddlShipment = new DropDownList();
            //cell.Controls.Add( new LiteralControl( SHP.Resources.Controls.AdminOrdersControl_Shipment ) );
            //cell.Controls.Add( this.ddlShipment );
            //row.Cells.Add( cell );

            ////--Filtruj
            //cell = new TableCell();
            //Button btnFilter = new Button();
            //btnFilter.Text = SHP.Resources.Controls.AdminOrdersControl_Find;
            //cell.Controls.Add( btnFilter );
            //btnFilter.Click += ( s, e ) =>
            //{
            //    GridViewDataBind( true );
            //};
            //row.Cells.Add( cell );

            //table.Rows.Add( row );

            //{
            //    #region Fill drop down list
            //    List<OrderStatus> statuses = Storage<OrderStatus>.Read();
            //    OrderStatus status = new OrderStatus();
            //    statuses.Insert( 0, status );
            //    status.Name = SHP.Resources.Controls.AdminOrdersControl_OptionAll;
            //    status.Id = 0;
            //    status.Code = string.Empty;
            //    ddlOrderStatus.DataSource = statuses;
            //    ddlOrderStatus.DataTextField = "Name";
            //    ddlOrderStatus.DataValueField = "Code";

            //    List<Shipment> shipments = Storage<Shipment>.Read();
            //    Shipment shipment = new Shipment();
            //    shipments.Insert( 0, shipment );
            //    shipment.Name = Resources.Controls.AdminOrdersControl_OptionAll;
            //    shipment.Id = 0;
            //    ddlShipment.DataSource = shipments;
            //    ddlShipment.DataTextField = "Name";
            //    ddlShipment.DataValueField = "Code";
            //    #endregion

            //    ddlOrderStatus.DataBind();
            //    ddlShipment.DataBind();
            //}
            return table;
        }

        private OrderEntity.ReadNot GetFilterValue() {
            OrderEntity.ReadNot filter = new OrderEntity.ReadNot();
            filter.OrderStatusCode = ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString();
            filter.AccountId = Security.Account.Id;

            return filter;
        }

        private void GridViewDataBind(bool bind) {
            OrderEntity.ReadNot filter = GetFilterValue();
            List<OrderEntity> list = Storage<OrderEntity>.Read(filter);

            var ordered = list.AsQueryable().OrderBy(SortExpression + " " + SortDirection);
            gridView.DataSource = ordered.ToList();
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
            //grid.MasterTableView.NestedViewTemplate = new AssociatedOrdersDetailTemplate(this.EditUrlFormat, this, base.BuildReturnUrlQueryParam());

            grid.MasterTableView.Columns.Add(new GridBoundColumn {
                DataField = "OrderNumber",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderNumber,
                SortExpression = "OrderNumber",
            });
            grid.Columns.Add(new GridBoundColumn {
                DataField = "OrderDate",
                DataFormatString = "{0:d}",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderDate,
                SortExpression = "OrderDate",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns.Add(new GridIconColumn {
                ImageWidth = Unit.Pixel(16),
                ImageHeight = Unit.Pixel(16),
                DataImageUrlFields = new string[] { "OrderStatusIcon" },
                DataAlternateTextField = "OrderStatusName",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderStatus,
                SortExpression = "OrderStatusName",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns.Add(new GridIconColumn {
                ImageWidth = Unit.Pixel(16),
                ImageHeight = Unit.Pixel(16),
                DataImageUrlFields = new string[] { "ShipmentIcon" },
                DataAlternateTextField = "ShipmentName",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnShipment,
                SortExpression = "ShipmentName",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });

            grid.Columns.Add(new GridPriceColumn {
                //CurrencySymbolField = "CurrencySymbol",
                DataField = "PriceWVAT",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnPriceTotalWVAT,
                SortExpression = "PriceWVAT",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns.Add(new GridBoundColumn {
                DataField = "AssociationStatusText",
                HeaderText = Resources.EShopStrings.AdminOrdersControl_ColumnAssociated,
                SortExpression = "AssociationStatusText",
                AllowFiltering = false,
            });
            grid.Columns.Add(new GridBoundColumn {
                DataField = "PaydDate",
                DataFormatString = "{0:d}",
                HeaderText = "E platba",
                SortExpression = "PaydDate",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            GridButtonColumn btnEdit = new GridButtonColumn();
            btnEdit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImage");
            btnEdit.Text = SHP.Resources.Controls.GridView_ToolTip_EditItem;
            btnEdit.ButtonType = GridButtonColumnType.ImageButton;
            btnEdit.CommandName = EDIT_COMMAND;
            grid.MasterTableView.Columns.Add(btnEdit);

            grid.ItemCommand += OnRowCommand;
            grid.ItemDataBound += OnRowDataBound;

            return grid;
        }

        void OnRowDataBound(object sender, GridItemEventArgs e) {
            if (e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem)
                return;

            OrderEntity order = e.Item.DataItem as OrderEntity;

            bool isEditable = OrderEntity.GetOrderStatusFromCode(order.OrderStatusCode) == OrderEntity.OrderStatus.WaitingForProccess;

            int editIndex = e.Item.Cells.Count - 1;
            int payIndex = e.Item.Cells.Count - 2;

            ImageButton btnEdit = (e.Item.Cells[editIndex].Controls[0] as ImageButton);
            //ImageButton btnPay = (e.Item.Cells[payIndex].Controls[0] as ImageButton);

            if (!btnEdit.Enabled) {
                btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImageD");
                //btnPay.Enabled = false;
            }
        }

        void OnRowCommand(object sender, GridCommandEventArgs e) {
            if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
            if (e.CommandName == PAY_COMMAND) OnPayCommand(sender, e);
        }
        private void OnEditCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            Response.Redirect(String.Format(EditUrlFormat, orderId) + "&" + base.BuildReturnUrlQueryParam());
        }
        private void OnPayCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            Response.Redirect(String.Format(PayUrlFormat, orderId) + "&" + base.BuildReturnUrlQueryParam());
        }
        /*
        internal sealed class AssociatedOrdersDetailTemplate : ITemplate {
            private RadGrid gridView = null;
            public string EditUrlFormat { get; set; }
            private CmsControl Parent { get; set; }
            private string ReturnUrl { get; set; }
            public AssociatedOrdersDetailTemplate(string editUrlFormat, CmsControl parent, string returnUrl) {
                this.EditUrlFormat = editUrlFormat;
                this.Parent = parent;
                this.ReturnUrl = returnUrl;
            }

            void ITemplate.InstantiateIn(Control container) {
                gridView = CreateGridView();
                gridView.DataBinding += new EventHandler(detail_DataBinding);

                container.Controls.Add(gridView);
            }

            private RadGrid CreateGridView() {
                RadGrid grid = new RadGrid();
                CMS.Utilities.RadGridUtilities.Localize(grid);
                grid.MasterTableView.DataKeyNames = new string[] { "Id" };

                grid.AllowAutomaticInserts = false;
                grid.AllowFilteringByColumn = false;
                grid.ShowStatusBar = false;
                grid.ShowGroupPanel = false;
                grid.GroupingEnabled = false;
                grid.GroupingSettings.ShowUnGroupButton = false;
                grid.ClientSettings.AllowDragToGroup = false;
                grid.ClientSettings.AllowColumnsReorder = false;

                grid.MasterTableView.ShowHeader = true;
                grid.MasterTableView.ShowFooter = false;
                grid.MasterTableView.AllowPaging = false;
                grid.MasterTableView.AllowSorting = false;
                grid.MasterTableView.GridLines = GridLines.None;
                grid.MasterTableView.AutoGenerateColumns = false;

                grid.MasterTableView.Columns.Add(new GridBoundColumn {
                    DataField = "OrderNumber",
                    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderNumber,
                    SortExpression = "OrderNumber",
                });
                grid.Columns.Add(new GridBoundColumn {
                    DataField = "OrderDate",
                    DataFormatString = "{0:d}",
                    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderDate,
                    SortExpression = "OrderDate",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
                grid.Columns.Add(new GridIconColumn {
                    ImageWidth = Unit.Pixel(16),
                    ImageHeight = Unit.Pixel(16),
                    DataImageUrlFields = new string[] { "OrderStatusIcon" },
                    DataAlternateTextField = "OrderStatusName",
                    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderStatus,
                    SortExpression = "OrderStatusName",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
                grid.Columns.Add(new GridBoundColumn {
                    DataField = "OwnerName",
                    HeaderText = Resources.EShopStrings.AdminOrdersControl_ColumnOrderOwnerName,
                    SortExpression = "OwnerName",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
                grid.Columns.Add(new GridPriceColumn {
                    //CurrencySymbolField = "CurrencySymbol",
                    DataField = "Price",
                    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnPriceTotal,
                    SortExpression = "Price",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
                grid.Columns.Add(new GridPriceColumn {
                    //CurrencySymbolField = "CurrencySymbol",
                    DataField = "PriceWVAT",
                    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnPriceTotalWVAT,
                    SortExpression = "PriceWVAT",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
                GridButtonColumn btnEdit = new GridButtonColumn();
                btnEdit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
                btnEdit.ImageUrl = CMS.Utilities.ConfigUtilities.ConfigValue("CMS:EditButtonImage", this.Parent.Page);
                btnEdit.Text = SHP.Resources.Controls.GridView_ToolTip_EditItem;
                btnEdit.ButtonType = GridButtonColumnType.ImageButton;
                btnEdit.CommandName = EDIT_COMMAND;
                grid.MasterTableView.Columns.Add(btnEdit);

                grid.ItemCommand += OnRowCommand;

                return grid;
            }
            void OnRowCommand(object sender, GridCommandEventArgs e) {
                if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
            }
            private void OnEditCommand(object sender, GridCommandEventArgs e) {
                GridDataItem dataItem = (GridDataItem)e.Item;
                int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
                (sender as Control).Page.Response.Redirect(String.Format(EditUrlFormat, orderId) + "&" + this.ReturnUrl);
            }
            private void GridViewDataBind(bool bind, int parentId, GridNestedViewItem gni) {
                OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
                filter.ParentId = parentId;
                List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
                if (list.Count == 0) {
                    gridView.Visible = false;
                    Control c = gni.ParentItem.Cells[0];
                    c.Controls[0].Visible = false;
                    return;
                }
                gridView.DataSource = list;
                //gridView.DataBind();
            }

            void detail_DataBinding(object sender, EventArgs e) {
                RadGrid control = sender as RadGrid;
                GridNestedViewItem gni = (GridNestedViewItem)control.NamingContainer;
                OrderEntity order = (gni.DataItem as OrderEntity);

                GridViewDataBind(false, order.Id, gni);
            }
        }
         * */
    }
}
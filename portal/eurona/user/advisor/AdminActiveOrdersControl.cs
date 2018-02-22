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
using Eurona.Controls;

namespace Eurona.User.Advisor
{
    public class AdminActiveOrdersControl : CmsControl
    {
        private const string DELETE_COMMAND = "DELETE_ITEM";
        private const string EDIT_COMMAND = "EDIT_ITEM";

        private RadGrid gridView;

        public string EditUrlFormat { get; set; }
        public string UserUrlFormat { get; set; }

        public SortDirection SortDirection
        {
            get { return GetSession<SortDirection>("AdminActiveOrdersControl-SortDirection", SortDirection.Descending); }
            set { SetSession<SortDirection>("AdminActiveOrdersControl-SortDirection", value); }
        }

        public string SortExpression
        {
            get { return GetSession<string>("AdminActiveOrdersControl-SortExpression", "OrderDate"); }
            set { SetSession<string>("AdminActiveOrdersControl-SortExpression", value); }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            Control filter = CreateFilterControl();
            this.Controls.Add(filter); ;

            gridView = CreateGridView();
            this.Controls.Add(gridView);

            GridViewDataBind(!IsPostBack);
        }


        /// <summary>
        /// Vráti zoznam objednávok pre asociaciu (Nepridruzene a oznacene )
        /// </summary>
        /// <returns></returns>
        public List<OrderEntity> GetSelectedOrdersToAssociation()
        {
            List<OrderEntity> list = new List<OrderEntity>();
            foreach (GridDataItem dataItem in this.gridView.Items)
            {
                int checkIndex = 2;
                CheckBox cbAssociate = (dataItem.Cells[checkIndex].Controls[0] as CheckBox);
                if (cbAssociate.Checked)
                {
                    int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
                    OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
                    if (order.ParentId.HasValue) continue;
                    list.Add(order);
                }
            }
            return list;
        }

        /// <summary>
        /// Vráti zoznam objednávok ktore nie su pridruzene ani necakaju na akceptaciu pridruzenia
        /// </summary>
        /// <returns></returns>
        public List<OrderEntity> GetOrdersNotAssociated()
        {
            OrderEntity.ReadByFilter filter = GetFilterValue();
            List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
            List<OrderEntity> newList = new List<OrderEntity>();
            foreach (OrderEntity order in list)
            {
                if (order.ParentId.HasValue) continue;
                if (order.AssociationAccountId.HasValue) continue;

                newList.Add(order);
            }
            return newList;
        }

        private Table CreateFilterControl()
        {
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

        private OrderEntity.ReadByFilter GetFilterValue()
        {
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.OrderStatusCode = ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString();
            filter.AccountId = Security.Account.Id;

            return filter;
        }

        public void GridViewDataBind(bool bind)
        {
            OrderEntity.ReadByFilter filter = GetFilterValue();
            List<OrderEntity> list = Storage<OrderEntity>.Read(filter);

            var ordered = list.AsQueryable().OrderBy(SortExpression + " " + SortDirection);
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
            grid.MasterTableView.NestedViewTemplate = new AssociatedOrdersDetailTemplate(this.EditUrlFormat, this, base.BuildReturnUrlQueryParam());

            grid.MasterTableView.Columns.Add(new GridCheckBoxColumn { ShowFilterIcon = false, ShowSortIcon = false, AllowFiltering = false, AllowSorting = false });

            grid.MasterTableView.Columns.Add(new GridBoundColumn
            {
                DataField = "OrderNumber",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderNumber,
                SortExpression = "OrderNumber",
            });
            grid.Columns.Add(new GridBoundColumn
            {
                DataField = "OrderDate",
                DataFormatString = "{0:d}",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderDate,
                SortExpression = "OrderDate",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns.Add(new GridIconColumn
            {
                ImageWidth = Unit.Pixel(16),
                ImageHeight = Unit.Pixel(16),
                DataImageUrlFields = new string[] { "OrderStatusIcon" },
                DataAlternateTextField = "OrderStatusName",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderStatus,
                SortExpression = "OrderStatusName",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns.Add(new GridIconColumn
            {
                ImageWidth = Unit.Pixel(16),
                ImageHeight = Unit.Pixel(16),
                DataImageUrlFields = new string[] { "ShipmentIcon" },
                DataAlternateTextField = "ShipmentName",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnShipment,
                SortExpression = "ShipmentName",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            //grid.Columns.Add( new GridPriceColumn
            //{
            //    DataField = "Price",
            //    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnPriceTotal,
            //    SortExpression = "Price",
            //    AutoPostBackOnFilter = true,
            //    CurrentFilterFunction = GridKnownFunction.Contains
            //} );
            grid.Columns.Add(new GridPriceColumn
            {
                //CurrencySymbolField = "CurrencySymbol",
                DataField = "PriceWVAT",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnPriceTotalWVAT,
                SortExpression = "PriceWVAT",
                AutoPostBackOnFilter = true,
                CurrentFilterFunction = GridKnownFunction.Contains
            });
            grid.Columns.Add(new GridBoundColumn
            {
                DataField = "AssociationStatusText",
                HeaderText = Resources.EShopStrings.AdminOrdersControl_ColumnAssociated,
                SortExpression = "AssociationStatusText",
                AllowFiltering = false,
            });

            GridButtonColumn btnEdit = new GridButtonColumn();
            btnEdit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImage");
            btnEdit.Text = SHP.Resources.Controls.GridView_ToolTip_EditItem;
            btnEdit.ButtonType = GridButtonColumnType.ImageButton;
            btnEdit.CommandName = EDIT_COMMAND;
            grid.MasterTableView.Columns.Add(btnEdit);

            GridButtonColumn btnDelete = new GridButtonColumn();
            btnDelete.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImage");
            btnDelete.Text = SHP.Resources.Controls.GridView_ToolTip_DeleteItem;
            btnDelete.ButtonType = GridButtonColumnType.ImageButton;
            btnDelete.ConfirmTitle = SHP.Resources.Controls.DeleteItemQuestion;
            btnDelete.ConfirmText = SHP.Resources.Controls.DeleteItemQuestion;
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

            OrderEntity order = e.Item.DataItem as OrderEntity;

            bool isEditable = OrderEntity.GetOrderStatusFromCode(order.OrderStatusCode) == OrderEntity.OrderStatus.WaitingForProccess;

            int checkIndex = 2;
            int deleteIndex = e.Item.Cells.Count - 1;
            int editIndex = e.Item.Cells.Count - 2;

            CheckBox chk = (e.Item.Cells[checkIndex].Controls[0] as CheckBox);
            chk.Enabled = (!order.ParentId.HasValue && !order.AssociationAccountId.HasValue);

            ImageButton btnDelete = (e.Item.Cells[deleteIndex].Controls[0] as ImageButton);
            ImageButton btnEdit = (e.Item.Cells[editIndex].Controls[0] as ImageButton);

            btnDelete.Enabled = isEditable;

            if (!btnDelete.Enabled)
                btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImageD");

            if (!btnEdit.Enabled)
                btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImageD");
        }

        void OnRowCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
            if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
        }
        private void OnEditCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            Response.Redirect(String.Format(EditUrlFormat, orderId) + "&" + base.BuildReturnUrlQueryParam());
        }
        private void OnDeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            OrderEntity entity = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
            if (entity != null) {
                Storage<OrderEntity>.Delete(entity);
                CartOrderHelper.DeleteTVDOrderWithCart(entity);
            }
            GridViewDataBind(true);
        }

        internal sealed class AssociatedOrdersDetailTemplate : ITemplate
        {
            private RadGrid gridView = null;
            public string EditUrlFormat { get; set; }
            private CmsControl Parent { get; set; }
            private string ReturnUrl { get; set; }
            public AssociatedOrdersDetailTemplate(string editUrlFormat, CmsControl parent, string returnUrl)
            {
                this.EditUrlFormat = editUrlFormat;
                this.Parent = parent;
                this.ReturnUrl = returnUrl;
            }

            void ITemplate.InstantiateIn(Control container)
            {
                gridView = CreateGridView();
                gridView.DataBinding += new EventHandler(detail_DataBinding);

                container.Controls.Add(gridView);
            }

            private RadGrid CreateGridView()
            {
                RadGrid grid = new RadGrid();
                CMS.Utilities.RadGridUtilities.Localize(grid);
                grid.MasterTableView.DataKeyNames = new string[] { "Id" };
                grid.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(235, 10, 91);//eb0a5b

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

                grid.MasterTableView.Columns.Add(new GridBoundColumn
                {
                    DataField = "OrderNumber",
                    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderNumber,
                    SortExpression = "OrderNumber",
                });
                grid.Columns.Add(new GridBoundColumn
                {
                    DataField = "OrderDate",
                    DataFormatString = "{0:d}",
                    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderDate,
                    SortExpression = "OrderDate",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
                grid.Columns.Add(new GridIconColumn
                {
                    ImageWidth = Unit.Pixel(16),
                    ImageHeight = Unit.Pixel(16),
                    DataImageUrlFields = new string[] { "OrderStatusIcon" },
                    DataAlternateTextField = "OrderStatusName",
                    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnOrderStatus,
                    SortExpression = "OrderStatusName",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
                grid.Columns.Add(new GridBoundColumn
                {
                    DataField = "OwnerName",
                    HeaderText = Resources.EShopStrings.AdminOrdersControl_ColumnOrderOwnerName,
                    SortExpression = "OwnerName",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
                grid.Columns.Add(new GridPriceColumn
                {
                    //CurrencySymbolField = "CurrencySymbol",
                    DataField = "Price",
                    HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnPriceTotal,
                    SortExpression = "Price",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });
                grid.Columns.Add(new GridPriceColumn
                {
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
            void OnRowCommand(object sender, GridCommandEventArgs e)
            {
                if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
            }
            private void OnEditCommand(object sender, GridCommandEventArgs e)
            {

                GridDataItem dataItem = (GridDataItem)e.Item;
                int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
                (sender as Control).Page.Response.Redirect(String.Format(EditUrlFormat, orderId) + "&" + this.ReturnUrl);
            }
            private void GridViewDataBind(bool bind, int parentId, GridNestedViewItem gni)
            {
                OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
                filter.ParentId = parentId;
                //filter.AssociationAccountId = accountId;

                List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
                if (list.Count == 0)
                {
                    gridView.Visible = false;
                    Control c = gni.ParentItem.Cells[0];
                    c.Controls[0].Visible = false;
                    return;
                }

                gridView.DataSource = list;
                //gridView.DataBind();
            }

            void detail_DataBinding(object sender, EventArgs e)
            {
                RadGrid control = sender as RadGrid;
                GridNestedViewItem gni = (GridNestedViewItem)control.NamingContainer;
                OrderEntity order = (gni.DataItem as OrderEntity);

                GridViewDataBind(false, order.Id, gni);
            }
        }

    }
}

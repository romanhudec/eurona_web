using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using CMS.Controls;
using OrderFastViewEntity = Eurona.DAL.Entities.OrderFastView;
using OrderEntity = Eurona.DAL.Entities.Order;
using CMS;
using System.Web.UI;
using Telerik.Web.UI;
using SHP.Controls;
using Eurona.Common.DAL.Entities;
using SHP.Entities.Classifiers;

namespace Eurona.Controls.Order {
    public class AdminOrdersControl : CmsControl {

        public event EventHandler OnGridViewDataBinded;
        public event GridPageChangedEventHandler OnGridViewPageChanged;

        private const string DELETE_COMMAND = "DELETE_ITEM";
        private const string EDIT_COMMAND = "EDIT_ITEM";
        private const string STORNO_COMMAND = "STORNO_ITEM";

        private RadGrid gridView;
        public string EditUrlFormat { get; set; }
        public string UserUrlFormat { get; set; }

        private CheckBox cbShowAll = null;

        public SortDirection SortDirection {
            get { return GetSession<SortDirection>("AdminOrdersControl-SortDirection", SortDirection.Descending); }
            set { SetSession<SortDirection>("AdminOrdersControl-SortDirection", value); }
        }

        public string SortExpression {
            get { return GetSession<string>("AdminOrdersControl-SortExpression", "OrderDate"); }
            set { SetSession<string>("AdminOrdersControl-SortExpression", value); }
        }

        /// <summary>
        /// Filter na status objednavok
        /// </summary>
        public bool ShowFastFilterView {
            get { return GetState<bool>("ShowFastFilterView"); }
            set { SetState<bool>("ShowFastFilterView", value); }
        }

        /// <summary>
        /// Filter na status objednavok
        /// </summary>
        public string OrderStatusCode {
            get { return GetState<string>("OrderStatusCode"); }
            set { SetState<string>("OrderStatusCode", value); }
        }

        /// <summary>
        /// Filter na status objednavok
        /// </summary>
        public string FilterOrderStatusName {
            get { return GetState<string>("FilterOrderStatusName"); }
            set { SetState<string>("FilterOrderStatusName", value); }
        }
        public string FilterOrderNumber {
            get { return GetState<string>("FilterOrderNumber"); }
            set { SetState<string>("FilterOrderNumber", value); }
        }
        public string FilterOwnerName {
            get { return GetState<string>("FilterOwnerName"); }
            set { SetState<string>("FilterOwnerName", value); }
        }
        /// <summary>
        /// Filter na NOT status objednavok
        /// </summary>
        public string NotOrderStatusCode {
            get { return GetState<string>("NotOrderStatusCode"); }
            set { SetState<string>("NotOrderStatusCode", value); }
        }

        /// <summary>
        /// Filter posledne X mesiacov
        /// </summary>
        public int? OnlyLastMonths {
            get { return GetState<int?>("OnlyLastMonths"); }
            set { SetState<int?>("OnlyLastMonths", value); }
        }
        /// <summary>
        /// Len pridruzene objednavky objednávke ParentId
        /// </summary>
        public int? ParentId {
            get { return GetState<int?>("ParentId"); }
            set { SetState<int?>("ParentId", value); }
        }

        /// <summary>
        /// Len objednavky vytvorené daným používateťelom
        /// </summary>
        public int? CreatedByAccountId {
            get { return GetState<int?>("CreatedByAccountId"); }
            set { SetState<int?>("CreatedByAccountId", value); }
        }

        protected override void CreateChildControls() {
            base.CreateChildControls();

            Control filter = CreateFilterControl();
            this.Controls.Add(filter);

            gridView = CreateGridView();
            this.Controls.Add(gridView);
            
            GridViewDataBind(!IsPostBack);
        }

        private Table CreateFilterControl() {
            Table table = new Table();
            table.Width = Unit.Percentage(100);

            if (this.ShowFastFilterView ==false) {
                TableRow row = new TableRow();
                if (Security.IsInRole(Role.ADMINISTRATOR) || Security.IsInRole(Role.OPERATOR)) {
                    ////--Cislo objednavky
                    TableCell cell = new TableCell();
                    this.cbShowAll = new CheckBox();
                    this.cbShowAll.Text = "Zobrazit všechny objednávky ( Standardne se zobrazují jen nevyřízené)";
                    this.cbShowAll.AutoPostBack = true;
                    cell.Controls.Add(this.cbShowAll);
                    row.Cells.Add(cell);

                    //if (!IsPostBack) {
                    if (string.IsNullOrEmpty(this.NotOrderStatusCode))
                        this.NotOrderStatusCode = ((int)OrderEntity.OrderStatus.Proccessed).ToString();

                    if (this.OnlyLastMonths.HasValue == false)
                        this.OnlyLastMonths = 1;

                    this.cbShowAll.Text = string.Format("Zobrazit všechny objednávky ( Standardne se zobrazují jen nevyřízené a jen za poslední {0} měsíce )", this.OnlyLastMonths);
                    //}

                    if (!string.IsNullOrEmpty(this.NotOrderStatusCode)) {
                        if (OrderEntity.GetOrderStatusFromCode(this.NotOrderStatusCode) == SHP.Entities.Order.OrderStatus.Proccessed)
                            this.cbShowAll.Checked = false;
                        else this.cbShowAll.Checked = true;
                    }
                    this.cbShowAll.CheckedChanged += OnShowAll_CheckedChanged;

                }
            }
            return table;
        }

        void btnFilter_Click(object sender, EventArgs e) {
            GridViewDataBind(true);
        }

        void OnShowAll_CheckedChanged(object sender, EventArgs e) {
            if (this.cbShowAll.Checked) this.NotOrderStatusCode = null;
            else this.NotOrderStatusCode = ((int)OrderEntity.OrderStatus.Proccessed).ToString();
            GridViewDataBind(true);
        }
        private OrderFastViewEntity.ReadByFilter GetFilterValue() {
            OrderFastViewEntity.ReadByFilter filter = new OrderFastViewEntity.ReadByFilter();

            if (this.ShowFastFilterView == true) {
                if (!string.IsNullOrEmpty(FilterOwnerName)) filter.OwnerName = FilterOwnerName;
                if (!string.IsNullOrEmpty(FilterOrderNumber)) filter.OrderNumber = FilterOrderNumber;
                if (!string.IsNullOrEmpty(FilterOrderStatusName)) filter.OrderStatusName = FilterOrderStatusName;
                if (this.OnlyLastMonths.HasValue) filter.OnlyLastMonths = this.OnlyLastMonths;
                return filter;
            }

            if (this.ParentId.HasValue) filter.ParentId = this.ParentId.Value;
            if (this.CreatedByAccountId.HasValue) filter.CreatedByAccountId = this.CreatedByAccountId.Value;
            if (!string.IsNullOrEmpty(this.OrderStatusCode)) filter.OrderStatusCode = this.OrderStatusCode;
            if (!string.IsNullOrEmpty(this.NotOrderStatusCode)) filter.NotOrderStatusCode = this.NotOrderStatusCode;
            if (this.OnlyLastMonths.HasValue) {
                if (this.cbShowAll.Checked == false) filter.OnlyLastMonths = this.OnlyLastMonths;
            }
            return filter;
        }
       
        public void GridViewDataBind(bool bind) {
            OrderFastViewEntity.ReadByFilter filter = GetFilterValue();
            if (!Security.IsInRole(Role.ADMINISTRATOR) && !Security.IsInRole(Role.OPERATOR))
                filter.AccountId = Security.Account.Id;

            if (!filter.IsEmpty()) {
                List<OrderFastViewEntity> list = Storage<OrderFastViewEntity>.Read(filter);
                var ordered = list.AsQueryable().OrderBy(SortExpression + " " + SortDirection);
                gridView.DataSource = ordered.ToList();
            }
            if (bind) {
                gridView.DataBind();
                if (OnGridViewDataBinded != null) OnGridViewDataBinded(this, null);
            }
        }
        /// <summary>
        /// Vráti zoznam objednávok pre asociaciu (Nepridruzene a oznacene )
        /// </summary>
        /// <returns></returns>
        public List<OrderEntity> GetSelectedOrdersToAssociation() {
            List<OrderEntity> list = new List<OrderEntity>();
            foreach (GridDataItem dataItem in this.gridView.Items) {
                int checkIndex = 2;
                CheckBox cbAssociate = (dataItem.Cells[checkIndex].Controls[0] as CheckBox);
                if (cbAssociate.Checked) {
                    int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
                    OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
                    if (order.ParentId.HasValue) continue;
                    list.Add(order);
                }
            }
            return list;
        }

        public List<OrderEntity> GetSelectedOrders() {
            List<OrderEntity> list = new List<OrderEntity>();
            foreach (GridDataItem dataItem in this.gridView.Items) {
                int checkIndex = 2;
                CheckBox cbAssociate = (dataItem.Cells[checkIndex].Controls[0] as CheckBox);
                if (cbAssociate.Checked) {
                    int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
                    OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
                    if (order.ParentId.HasValue) continue;
                    list.Add(order);
                }
            }
            return list;
        }

        /// <summary>
        /// Metoda oznaci/odznaci vsetky povolene check boxy
        /// </summary>
        public void SelectUnselectAll(bool onlyEnabled = true) {
            foreach (GridDataItem dataItem in this.gridView.Items) {
                int checkIndex = 2;
                CheckBox cbAssociate = (dataItem.Cells[checkIndex].Controls[0] as CheckBox);
                if (onlyEnabled && !cbAssociate.Enabled) continue;

                cbAssociate.Checked = !cbAssociate.Checked;
            }
        }
        /// <summary>
        /// Metoda oznaci/odznaci vsetky povolene check boxy
        /// </summary>
        public void SelecAll(bool onlyEnabled = true) {
            foreach (GridDataItem dataItem in this.gridView.Items) {
                int checkIndex = 2;
                CheckBox cbAssociate = (dataItem.Cells[checkIndex].Controls[0] as CheckBox);
                if (onlyEnabled && !cbAssociate.Enabled) continue;

                cbAssociate.Checked = true;
            }
        }
        /// <summary>
        /// Metoda oznaci/odznaci vsetky povolene check boxy
        /// </summary>
        public void UnselecAll(bool onlyEnabled = true) {
            foreach (GridDataItem dataItem in this.gridView.Items) {
                int checkIndex = 2;
                CheckBox cbAssociate = (dataItem.Cells[checkIndex].Controls[0] as CheckBox);
                if (onlyEnabled && !cbAssociate.Enabled) continue;

                cbAssociate.Checked = false;
            }
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
            grid.MasterTableView.NestedViewTemplate = new AssociatedOrdersDetailTemplate();

            grid.MasterTableView.Columns.Add(new GridCheckBoxColumn { ShowFilterIcon = false, ShowSortIcon = false, AllowFiltering = false, AllowSorting = false });
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

            grid.Columns.Add(new Eurona.Common.Controls.GridPriceColumn {
                DataField = "PriceWVAT",
                CurrencySymbolDataField = "CurrencySymbol",
                HeaderText = SHP.Resources.Controls.AdminOrdersControl_ColumnPriceTotalWVAT,
                SortExpression = "PriceWVAT",
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
            grid.Columns.Add(new GridBoundColumn {
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

            GridButtonColumn btnStorno = new GridButtonColumn();
            btnStorno.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnStorno.ImageUrl = ConfigValue("CMS:CloseButtonImage");
            btnStorno.Text = Resources.EShopStrings.GridView_ToolTip_StornoItem;
            btnStorno.ButtonType = GridButtonColumnType.ImageButton;
            btnStorno.ConfirmTitle = Resources.Strings.StornoItemQuestion;
            btnStorno.ConfirmText = Resources.Strings.StornoItemQuestion;
            btnStorno.CommandName = STORNO_COMMAND;
            grid.MasterTableView.Columns.Add(btnStorno);

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
            grid.PageIndexChanged += grid_PageIndexChanged;

            return grid;
        }

        void grid_PageIndexChanged(object sender, GridPageChangedEventArgs e) {
            if (OnGridViewPageChanged != null) OnGridViewPageChanged(sender, e);
        }

        void OnRowDataBound(object sender, GridItemEventArgs e) {
            if (e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem)
                return;

            OrderFastViewEntity order = e.Item.DataItem as OrderFastViewEntity;

            bool isEditable = OrderEntity.GetOrderStatusFromCode(order.OrderStatusCode) == OrderEntity.OrderStatus.WaitingForProccess;
            bool canStrono = OrderEntity.GetOrderStatusFromCode(order.OrderStatusCode) == OrderEntity.OrderStatus.InProccess;

            int checkIndex = 2;
            int deleteIndex = e.Item.Cells.Count - 1;
            int stornoIndex = e.Item.Cells.Count - 2;
            int editIndex = e.Item.Cells.Count - 3;

            CheckBox chk = (e.Item.Cells[checkIndex].Controls[0] as CheckBox);
            chk.Enabled = (isEditable && !order.ParentId.HasValue && !order.AssociationAccountId.HasValue) && (!order.AssociationRequestStatus.HasValue || order.AssociationRequestStatus.Value != (int)OrderEntity.AssociationStatus.None);

            ImageButton btnDelete = (e.Item.Cells[deleteIndex].Controls[0] as ImageButton);
            ImageButton btnStorno = (e.Item.Cells[stornoIndex].Controls[0] as ImageButton);
            ImageButton btnEdit = (e.Item.Cells[editIndex].Controls[0] as ImageButton);

            btnDelete.Enabled = isEditable;
            btnStorno.Enabled = false;//canStrono;

            if (!btnDelete.Enabled)
                btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImageD");

            if (!btnEdit.Enabled)
                btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImageD");

            if (!btnStorno.Enabled)
                btnStorno.ImageUrl = ConfigValue("CMS:CloseButtonImageD");
        }

        void OnRowCommand(object sender, GridCommandEventArgs e) {
            if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
            if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
            if (e.CommandName == STORNO_COMMAND) OnStornoCommand(sender, e);
        }
        private void OnEditCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            Response.Redirect(String.Format(EditUrlFormat, orderId) + "&" + base.BuildReturnUrlQueryParam());
        }
        private void OnStornoCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
            order.OrderStatusCode = ((int)OrderEntity.OrderStatus.Storno).ToString();
            Storage<OrderEntity>.Update(order);
            GridViewDataBind(true);
        }
        private void OnDeleteCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });

            //Objednavky pre pridruzenie k tejto objednavke - zrusi sa pridruzenie
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
            foreach (OrderEntity childOrder in list) {
                childOrder.ParentId = null;
                childOrder.AssociationAccountId = null;
                childOrder.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.None;
                Storage<OrderEntity>.Update(childOrder);
            }
            // vymazanie danej objednavky
            Storage<OrderEntity>.Delete(order);


            GridViewDataBind(true);
        }

        internal sealed class AssociatedOrdersDetailTemplate : ITemplate {
            private RadGrid gridView = null;
            public AssociatedOrdersDetailTemplate() {
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
                grid.MasterTableView.ShowHeadersWhenNoRecords = false;
                grid.MasterTableView.NoDetailRecordsText = string.Empty;
                grid.MasterTableView.NoMasterRecordsText = string.Empty;

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

                return grid;
            }
            private void GridViewDataBind(bool bind, int parentId, GridNestedViewItem gni) {
                OrderFastViewEntity.ReadByFilter filter = new OrderFastViewEntity.ReadByFilter();
                filter.ParentId = parentId;

                List<OrderFastViewEntity> list = Storage<OrderFastViewEntity>.Read(filter);
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
                OrderFastViewEntity order = (gni.DataItem as OrderFastViewEntity);
                GridViewDataBind(false, order.Id, gni);
            }
        }

    }
}
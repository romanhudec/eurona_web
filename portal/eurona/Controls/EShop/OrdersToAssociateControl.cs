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
using CMS.Utilities;
using AccountEntity = CMS.Entities.Account;
using EshopEmailNotification = SHP.EmailNotification;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;


namespace Eurona.Controls.Order {
    public class OrdersToAssociateControl : CmsControl {
        private const string ACCEPT_COMMAND = "ACCEPT_ITEM";
        private const string REJECT_COMMAND = "REJECT_ITEM";
        private const string EDIT_COMMAND = "EDIT_ITEM";

        private RadGrid gridView;
        public string EditUrlFormat { get; set; }
        public string UserUrlFormat { get; set; }

        public SortDirection SortDirection {
            get { return GetSession<SortDirection>("OrdersToAssociateControl-SortDirection", SortDirection.Descending); }
            set { SetSession<SortDirection>("OrdersToAssociateControl-SortDirection", value); }
        }

        public string SortExpression {
            get { return GetSession<string>("OrdersToAssociateControl-SortExpression", "OrderDate"); }
            set { SetSession<string>("OrdersToAssociateControl-SortExpression", value); }
        }

        /// <summary>
        /// Filter na status objednavok
        /// </summary>
        public string OrderStatusCode {
            get { return GetState<string>("OrderStatusCode"); }
            set { SetState<string>("OrderStatusCode", value); }
        }

        public int? ParentId {
            get { return GetState<int?>("ParentId"); }
            set { SetState<int?>("ParentId", value); }
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

            return table;
        }

        private OrderEntity.ReadByFilter GetFilterValue() {
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();

            if (this.ParentId.HasValue)
                filter.ParentId = this.ParentId.Value;
            return filter;
        }

        private void GridViewDataBind(bool bind) {
            OrderEntity.ReadByFilter filter = GetFilterValue();
            filter.AssociationAccountId = Security.Account.Id;
            filter.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.WaitingToAccept;
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

            grid.Columns.Add(new GridPriceColumn {
                //CurrencySymbolField = "CurrencySymbol",
                DataField = "PriceWVAT",
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

            GridButtonColumn btnAccept = new GridButtonColumn();
            btnAccept.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnAccept.ImageUrl = ConfigValue("CMS:ApplyButtonImage");
            btnAccept.Text = "Akceptovat přidružení k mé objednávce";
            btnAccept.ButtonType = GridButtonColumnType.ImageButton;
            btnAccept.CommandName = ACCEPT_COMMAND;
            grid.MasterTableView.Columns.Add(btnAccept);

            GridButtonColumn btnReject = new GridButtonColumn();
            btnReject.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnReject.ImageUrl = ConfigValue("CMS:RejectButtonImage");
            btnReject.Text = "Zamítnout přidružení k mé objednávce";
            btnReject.ButtonType = GridButtonColumnType.ImageButton;
            btnReject.CommandName = REJECT_COMMAND;
            grid.MasterTableView.Columns.Add(btnReject);

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

        void OnRowCommand(object sender, GridCommandEventArgs e) {
            if (e.CommandName == EDIT_COMMAND) OnEditCommand(sender, e);
            if (e.CommandName == ACCEPT_COMMAND) OnAcceptCommand(sender, e);
            if (e.CommandName == REJECT_COMMAND) OnRejectCommand(sender, e);
        }
        private void OnEditCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            Response.Redirect(String.Format(EditUrlFormat, orderId) + "&" + base.BuildReturnUrlQueryParam());
        }
        private void OnAcceptCommand(object sender, GridCommandEventArgs e) {
            //Naviazanie objednavok na pridruzenie
            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.OrderStatusCode = ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString();
            filter.AccountId = Security.Account.Id;
            filter.HasChilds = false;//Iba tie objednavky, ktore uz niesu pridruzene k inym objednavkam
            OrderEntity myOrder = Storage<OrderEntity>.ReadFirst(filter);

            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
            order.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.Accepted;
            if (myOrder != null) order.ParentId = myOrder.Id;
            Storage<OrderEntity>.Update(order);

            GridViewDataBind(true);

            if (myOrder != null) this.Page.ClientScript.RegisterStartupScript(GetType(), "alert", string.Format("alert('Objednávka byla přidružena k Vaši objednávce č.{0}');", myOrder.OrderNumber), true);

            //Notifikacia ziadatela o pridruzenie
            SendEmail(order);

        }
        private void OnRejectCommand(object sender, GridCommandEventArgs e) {
            GridDataItem dataItem = (GridDataItem)e.Item;
            int orderId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
            OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = orderId });
            order.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.Rejected;
            Storage<OrderEntity>.Update(order);

            //Notifikacia ziadatela o pridruzenie
            SendEmail(order);

            GridViewDataBind(true);
        }

        private void SendEmail(OrderEntity order) {
            AccountEntity account = Security.Account;
            OrganizationEntity parentOrg = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = account.Id });

            EshopEmailNotification email = new EshopEmailNotification();
            email.To = account.Email;
            email.Subject = string.Format(Resources.EShopStrings.UserOrderFinishPage_Email2User_Subject, order.OrderNumber);
            if (order.AssociationRequestStatus == (int)OrderEntity.AssociationStatus.Accepted) {
                email.Message = String.Format("Přidružení Vaši objednávky č.{0} k objednávce poradce {1} bylo akceptováno!", order.OrderNumber, parentOrg.Name).Replace("\\n", Environment.NewLine) +
                    @"<br/><b>S pozdravem</b><br/>" + CMS.Utilities.ConfigUtilities.ConfigValue("SHP:SMTP:FromDisplay", this.Page);
            } else {
                email.Message = String.Format("Přidružení Vaši objednávky č.{0} k objednávce poradce {1} bylo zamítnuto!", order.OrderNumber, parentOrg.Name).Replace("\\n", Environment.NewLine) +
                    @"<br/><b>S pozdravem</b><br/>" + CMS.Utilities.ConfigUtilities.ConfigValue("SHP:SMTP:FromDisplay", this.Page);
            }
            email.Notify(true);
        }
    }
}
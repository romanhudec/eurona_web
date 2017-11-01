using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountEntity = CMS.Entities.Account;
using CartEntity = Eurona.Common.DAL.Entities.Cart;
using CartProductEntity = Eurona.Common.DAL.Entities.CartProduct;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using OrderEntity = Eurona.DAL.Entities.Order;
using OrderStatusEntity = SHP.Entities.Classifiers.OrderStatus;
using PaymentEntity = SHP.Entities.Classifiers.Payment;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using System.Security.Principal;
using System.Diagnostics;
using CMS.Controls;
using Eurona.EShop;
using Eurona.DAL.Entities;
using SHP;
using Eurona.Common.Controls.Cart;
using System.Text;
using SHP.Controls;
using Eurona.Common;
using Eurona.Controls;

namespace Eurona.User.Anonymous {
    public partial class OrderControl : CMS.Controls.CmsControl {
        private const string DELETE_COMMAND = "DELETE_ITEM";

        private GridView dataGrid = null;
        //private DropDownList ddlShipment = null;
        private Eurona.Controls.AddressControl addressDeliveryControl = null;

        private OrderEntity order = null;
        private LiteralControl lcBodyByEurosap = null;
        private LiteralControl lcKatalogovaCenaCelkemByEurosap = null;
        private LiteralControl lcDopravne = null;
        private Label lblFakturovanaCena = null;

        public OrderControl() {
            this.IsEditing = true;
        }

        public int OrderId {
            get {
                object o = ViewState["OrderId"];
                return o != null ? (int)Convert.ToInt32(o) : 0;
            }
            set { ViewState["OrderId"] = value; }
        }

        public OrderEntity OrderEntity {
            get {
                if (this.order != null) return this.order;
                this.order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadById { OrderId = this.OrderId });
                return this.order;
            }
        }

        public string CssGridView { get; set; }
        public string FinishUrlFormat { get; set; }

        #region Protected overrides
        protected override void CreateChildControls() {
            base.CreateChildControls();

            //Base kontroly
            if (!Security.IsLogged(true)) return;
            if (this.OrderEntity == null) return;

            //Kontrola na rolu pouizvatela
            if (!Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR) && !Security.Account.IsInRole(Eurona.DAL.Entities.Role.OPERATOR)) {
                if (this.OrderEntity.AccountId != Security.Account.Id)
                    this.IsEditing = false;
            }
            /*
            HtmlGenericControl div = new HtmlGenericControl( "div" );
            div.Attributes.Add( "class", this.CssClass + "_orderNumber" );
            div.Controls.Add( new LiteralControl( string.Format( SHP.Resources.Controls.OrderControl_OrderNumberFormatText, this.OrderEntity.OrderNumber ) ) );
            this.Controls.Add( div );
            */

            #region Create controls
            this.dataGrid = CreateGridControl();

            Table mainTable = new Table();
            mainTable.Width = Unit.Percentage(100);
            TableRow mainRow = new TableRow();
            TableCell leftCell = new TableCell(); leftCell.Width = Unit.Percentage(35); leftCell.VerticalAlign = VerticalAlign.Top;
            TableCell rightCell = new TableCell(); rightCell.Width = Unit.Percentage(65); rightCell.HorizontalAlign = HorizontalAlign.Center; rightCell.VerticalAlign = VerticalAlign.Top;
            mainRow.Cells.Add(leftCell); mainRow.Cells.Add(rightCell);
            mainTable.Rows.Add(mainRow);

            //Polozky objednavky
            RoundPanel rpOrderProducts = new RoundPanel();
            rpOrderProducts.CssClass = "roundPanel";
            rpOrderProducts.Controls.Add(this.dataGrid);
            rpOrderProducts.Text = string.Empty;//SHP.Resources.Controls.OrderControl_OrderProducts;

            #region Table UserInfo
            Table tableUserInfo = new Table();
            tableUserInfo.CellPadding = 3;
            tableUserInfo.CellSpacing = 5;
            TableRow rowUserInfo = new TableRow();
            TableCell cell = new TableCell();
            rowUserInfo.Cells.Add(cell); cell.CssClass = "form_label_required"; cell.HorizontalAlign = HorizontalAlign.Right; cell.Controls.Add(new LiteralControl(Resources.EShopStrings.OrderControl_OrderUser));
            cell = new TableCell(); rowUserInfo.Cells.Add(cell);
            cell.Controls.Add(new LiteralControl(this.OrderEntity.OwnerName));
            tableUserInfo.Rows.Add(rowUserInfo);

            rowUserInfo = new TableRow();
            cell = new TableCell();
            rowUserInfo.Cells.Add(cell); cell.CssClass = "form_label_required"; cell.HorizontalAlign = HorizontalAlign.Right; cell.Controls.Add(new LiteralControl(Resources.EShopStrings.OrderControl_OrderCreatedUser));
            cell = new TableCell(); rowUserInfo.Cells.Add(cell);
            cell.Controls.Add(new LiteralControl(this.OrderEntity.CreatedByName));
            tableUserInfo.Rows.Add(rowUserInfo);

            TableRow row = new TableRow();
            cell = new TableCell(); cell.CssClass = "form_label_required"; cell.HorizontalAlign = HorizontalAlign.Right; cell.Controls.Add(new LiteralControl(SHP.Resources.Controls.OrderControl_OrderStatus));
            row.Cells.Add(cell);
            cell = new TableCell(); cell.Controls.Add(new LiteralControl(this.OrderEntity.OrderStatusName));
            row.Cells.Add(cell);
            tableUserInfo.Rows.Add(row);

            /*
            //Preprava
            this.ddlShipment = new DropDownList();
            this.ddlShipment.ID = "ddlShipment";
            this.ddlShipment.DataSource = Storage<ShipmentEntity>.Read();
            this.ddlShipment.DataTextField = "Name";
            this.ddlShipment.DataValueField = "Code";
            this.ddlShipment.Enabled = this.IsEditing && ( !this.OrderEntity.Payed || Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) );
            this.ddlShipment.Width = Unit.Pixel( 150 );
            */
            tableUserInfo.Rows.Add(CreateTableRow(SHP.Resources.Controls.OrderControl_Shipment, new LiteralControl(this.OrderEntity.ShipmentName), false));
            /*
            tableUserInfo.Rows.Add(CreateTableRow(Resources.Strings.OrderControl_ShipmentFrom, new LiteralControl(this.OrderEntity.ShipmentFrom.ToString()), false));
            tableUserInfo.Rows.Add(CreateTableRow(Resources.Strings.OrderControl_ShipmentTo, new LiteralControl(this.OrderEntity.ShipmentTo.ToString()), false));
            tableUserInfo.Rows.Add(CreateTableRow(SHP.Resources.Controls.OrderControl_Shipment, new LiteralControl(this.OrderEntity.ShipmentName), false));
            */
            #endregion
            leftCell.Controls.Add(tableUserInfo);

            #region Informacie o cene
            Table priceInfoTable = new Table();
            rightCell.Controls.Add(priceInfoTable);
            TableRow rowPrice = null;

            //Body
            this.lcBodyByEurosap = new LiteralControl(OrderEntity.CartEntity.BodyEurosapTotal.ToString("F1"));
            rowPrice = new TableRow();
            TableCell cellPrice = new TableCell();
            cellPrice.Controls.Add(new LiteralControl(Resources.EShopStrings.AdminOrderControl_BodyCelkem));
            cellPrice.CssClass = "form_label";
            rowPrice.Cells.Add(cellPrice);
            cellPrice = new TableCell();
            cellPrice.Controls.Add(this.lcBodyByEurosap);
            cellPrice.CssClass = "form_control";
            rowPrice.Cells.Add(cellPrice);
            priceInfoTable.Rows.Add(rowPrice);

            //Katalogova Cena
            this.lcKatalogovaCenaCelkemByEurosap = new LiteralControl(SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.KatalogovaCenaCelkemByEurosap, this.Session));
            rowPrice = new TableRow();
            cellPrice = new TableCell();
            cellPrice.Controls.Add(new LiteralControl(Resources.EShopStrings.AdminOrderControl_KatalogovaCeneCelkem));
            cellPrice.CssClass = "form_label";
            rowPrice.Cells.Add(cellPrice);
            cellPrice = new TableCell();
            cellPrice.Controls.Add(this.lcKatalogovaCenaCelkemByEurosap);
            cellPrice.CssClass = "form_control";
            rowPrice.Cells.Add(cellPrice);
            priceInfoTable.Rows.Add(rowPrice);

            //Fakturovana Cena
            Label lblFakturovanaCena = new Label();
            lblFakturovanaCena.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(this.OrderEntity.PriceWVAT, this.OrderEntity.CurrencySymbol);
            lblFakturovanaCena.ForeColor = System.Drawing.Color.FromArgb(235, 10, 91);
            lblFakturovanaCena.Font.Bold = true;
            rowPrice = new TableRow();
            cellPrice = new TableCell();
            cellPrice.Controls.Add(new LiteralControl(Resources.EShopStrings.AdminOrderControl_FakturovanaSuma));
            cellPrice.CssClass = "form_label";
            rowPrice.Cells.Add(cellPrice);
            cellPrice = new TableCell();
            cellPrice.Controls.Add(lblFakturovanaCena);
            cellPrice.CssClass = "form_control";
            rowPrice.Cells.Add(cellPrice);
            priceInfoTable.Rows.Add(rowPrice);

            //Dopravne
            if (OrderEntity.CartEntity.DopravneEurosap.HasValue) {
                this.lcDopravne = new LiteralControl(SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.DopravneEurosap, this.Session));

                TableRow rowDopravne = new TableRow();
                TableCell cellDopravne = new TableCell();
                cellDopravne.Controls.Add(new LiteralControl(Resources.EShopStrings.AdminOrderControl_Dopravne));
                cellDopravne.CssClass = "form_label";
                rowDopravne.Cells.Add(cellDopravne);
                cellDopravne = new TableCell();
                cellDopravne.Controls.Add(this.lcDopravne);
                cellDopravne.CssClass = "form_control";
                rowDopravne.Cells.Add(cellDopravne);
                priceInfoTable.Rows.Add(rowDopravne);
            }

            HtmlGenericControl fieldSet = new HtmlGenericControl("fieldset");
            fieldSet.Attributes.Add("class", this.CssClass + "_fieldset");
            HtmlGenericControl legend = new HtmlGenericControl("legend");
            legend.Attributes.Add("class", this.CssClass + "_legend");
            legend.InnerText = Resources.EShopStrings.AdminOrderControl_InformaceOCene;
            fieldSet.Controls.Add(legend);
            fieldSet.Controls.Add(priceInfoTable);
            rightCell.Controls.Add(fieldSet);
            #endregion

            #region Body a marze//Poznamka
            fieldSet = new HtmlGenericControl("fieldset");
            fieldSet.Attributes.Add("class", this.CssClass + "_fieldset");
            legend = new HtmlGenericControl("legend");
            legend.Attributes.Add("class", this.CssClass + "_legend");
            HtmlGenericControl divOrderNotes = new HtmlGenericControl("div");
            divOrderNotes.Controls.Add(new LiteralControl(this.OrderEntity.Notes));
            divOrderNotes.Style.Add("text-align", "justify!important");
            divOrderNotes.Style.Add("font-size", "10px!important");
            fieldSet.Controls.Add(divOrderNotes);
            rightCell.Controls.Add(fieldSet);
            #endregion

            #region Table Association Info
            if (this.OrderEntity.AssociationAccountId.HasValue) {
                fieldSet = new HtmlGenericControl("fieldset");
                fieldSet.Attributes.Add("class", this.CssClass + "_fieldset");
                legend = new HtmlGenericControl("legend");
                legend.Attributes.Add("class", this.CssClass + "_legend");
                legend.InnerText = Resources.EShopStrings.AdminOrdersControl_ColumnAssociated;
                fieldSet.Controls.Add(legend);
                Table tableAssociationInfo = new Table();
                TableRow trAi = new TableRow(); tableAssociationInfo.Rows.Add(trAi);

                //Status zdruzenia
                TableCell cellAi = new TableCell(); cellAi.CssClass = "form_label_required";
                cellAi.Controls.Add(new LiteralControl(Resources.EShopStrings.OrderControl_AssociationStatus)); trAi.Cells.Add(cellAi);
                cellAi = new TableCell(); cellAi.CssClass = "form_control";
                cellAi.Controls.Add(new LiteralControl(this.OrderEntity.AssociationStatusText)); trAi.Cells.Add(cellAi);

                //Poradca, ktoremu bola pridruzena
                OrganizationEntity org = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = this.OrderEntity.AssociationAccountId.Value });
                trAi = new TableRow(); tableAssociationInfo.Rows.Add(trAi);
                cellAi = new TableCell(); cellAi.CssClass = "form_label_required";
                cellAi.Controls.Add(new LiteralControl(Resources.EShopStrings.OrderControl_AssociationAdvisor)); trAi.Cells.Add(cellAi);
                cellAi = new TableCell(); cellAi.CssClass = "form_control";
                cellAi.Controls.Add(new LiteralControl(org.Name)); trAi.Cells.Add(cellAi);

                fieldSet.Controls.Add(tableAssociationInfo);
                rightCell.Controls.Add(fieldSet);
            }
            #endregion

            rpOrderProducts.Controls.Add(mainTable);

            #region Footer
            #region Addresses
            this.addressDeliveryControl = new Eurona.Controls.AddressControl();
            this.addressDeliveryControl.Width = Unit.Percentage(100);
            this.addressDeliveryControl.IsEditing = this.IsEditing;
            this.addressDeliveryControl.AddressId = this.OrderEntity.DeliveryAddressId;

            //Delivery Address
            RoundPanel rpDlvAddress = new RoundPanel();
            rpDlvAddress.CssClass = "roundPanel";
            rpDlvAddress.Controls.Add(this.addressDeliveryControl);
            rpDlvAddress.Text = SHP.Resources.Controls.OrderControl_DeliveryAddress;
            rpOrderProducts.Controls.Add(rpDlvAddress);
            #endregion

            //PRVA DOCASNA UZAVIERKA

            //Check na uzavierku
            if (Security.Account.IsInRole(Role.ADVISOR) && Eurona.Common.Application.EuronaUzavierka.IsUzavierka4Advisor()) {
                buttonsDivControl.Visible = false;
                cell = new TableCell(); row.Cells.Add(cell);
                cell.Controls.Add(new LiteralControl(string.Format("<span style='color:red;font-weight:bold;'>Probíhá uzávěrka. Vytvářet objednývky bude možné : {0}</span>", Eurona.Common.Application.EuronaUzavierka.GeUzavierka4AdvisorTo())));
            } else
                buttonsDivControl.Visible = true;

            //Update Panel AJAX
            UpdateProgressControl upp = new UpdateProgressControl();
            upp.AssociatedUpdatePanelID = this.updatePanelCart.ID;
            upp.CssClass = "cart-update-progress";
            rpOrderProducts.Controls.Add(upp);
            #endregion
            this.Controls.Add(rpOrderProducts);

            #endregion

            //Binding
            GridViewDataBind(this.OrderEntity, !IsPostBack);

        }

        #endregion

        private TableRow CreateTableRow(string labelText, Control control, bool required) {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.CssClass = required ? "form_label_required" : "form_label";
            cell.Text = labelText;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.CssClass = "form_control";
            cell.Controls.Add(control);
            if (required) cell.Controls.Add(base.CreateRequiredFieldValidatorControl(control.ID));
            row.Cells.Add(cell);

            return row;
        }
        private TableRow CreateTableRow(string labelText, string css1, string controlText, string css2) {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = labelText;
            cell.CssClass = css1;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = controlText;
            cell.CssClass = css2;
            row.Cells.Add(cell);

            return row;
        }

        private void GridViewDataBind(OrderEntity order, bool bind) {
            List<CartProductEntity> list = Storage<CartProductEntity>.Read(new CartProductEntity.ReadByCart { CartId = order.CartId });

            this.dataGrid.PagerTemplate = null;
            dataGrid.DataSource = list;
            if (bind) {
                dataGrid.DataKeyNames = new string[] { "Id" };
                dataGrid.DataBind();
            }
        }

        private GridView CreateGridControl() {
            GridView grid = new GridView();
            grid.EnableViewState = true;
            grid.GridLines = GridLines.None;
            grid.Style.Add("margin-top", "5px");
            grid.RowDataBound += new GridViewRowEventHandler(grid_RowDataBound);

            grid.CssClass = CssGridView;
            grid.RowStyle.CssClass = CssGridView + "_rowStyle";
            grid.FooterStyle.CssClass = CssGridView + "_footerStyle";
            grid.PagerStyle.CssClass = CssGridView + "_pagerStyle";
            grid.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
            grid.HeaderStyle.CssClass = CssGridView + "_headerStyle";
            grid.EditRowStyle.CssClass = CssGridView + "_editRowStyle";
            grid.AlternatingRowStyle.CssClass = CssGridView + "_alternatingRowStyle";
            grid.ShowHeader = true;
            grid.ShowFooter = true;

            grid.AllowPaging = false;
            grid.AutoGenerateColumns = false;

            Eurona.Common.Controls.CartProductQuantityItemTemplate template = new Eurona.Common.Controls.CartProductQuantityItemTemplate();
            template.OnRefresh += (id, quantity) => {
                if (quantity != 0) {
                    CartProductEntity cartProduct = Storage<CartProductEntity>.ReadFirst(new CartProductEntity.ReadById { CartProductId = id });
                    ProductEntity p = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = cartProduct.ProductId });
                    bool isOperator = Security.IsLogged(false) && Security.Account.IsInRole(Role.OPERATOR);
                    if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(p.Code, p, quantity, false, this, isOperator))
                        return;
                    EuronaCartHelper.UpdateCartProduct(this.Page, cartProduct.CartId, cartProduct.ProductId, quantity);
                    //Prepocitanie kosiku a objednavky
                    this.RecalculateOrder();

                    this.lcBodyByEurosap.Text = OrderEntity.CartEntity.BodyEurosapTotal.ToString("F1");
                    this.lcKatalogovaCenaCelkemByEurosap.Text = SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.KatalogovaCenaCelkemByEurosap, this.Session);
                    this.lcDopravne.Text = SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.DopravneEurosap, this.Session);
                    this.lblFakturovanaCena.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(this.OrderEntity.PriceWVAT, this.OrderEntity.CurrencySymbol);

                }

                GridViewDataBind(this.OrderEntity, true);
            };
            grid.Columns.Add(new HyperLinkField {
                DataTextField = "ProductCode",
                HeaderText = "Kód",
                SortExpression = "ProductCode",
            });
            grid.Columns.Add(new HyperLinkField {
                DataTextField = "ProductName",
                HeaderText = SHP.Resources.Controls.CartControl_ColumnName,
                SortExpression = "ProductName",
            });

            bool isEditable = this.IsEditing && (OrderEntity.GetOrderStatusFromCode(this.OrderEntity.OrderStatusCode) == OrderEntity.OrderStatus.WaitingForProccess &&
                    (!this.OrderEntity.Payed || Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR)));
            if (isEditable) {
                grid.Columns.Add(new TemplateField {
                    ItemTemplate = template,
                    HeaderText = SHP.Resources.Controls.CartControl_ColumnQuantity,
                    SortExpression = "Quantity",
                });
            } else {
                grid.Columns.Add(new BoundField {
                    DataField = "Quantity",
                    HeaderText = SHP.Resources.Controls.CartControl_ColumnQuantity,
                    SortExpression = "Quantity",
                });
            }
            grid.Columns.Add(new BoundField {
                DataField = "BodyCelkem",
                HeaderText = "Body",
                SortExpression = "BodyCelkem"
            });

            grid.Columns.Add(new Eurona.Common.Controls.PriceField {
                DataField = "KatalogPriceWVATTotal",
                CurrencySymbolDataField = "CurrencySymbol",
                HeaderText = "Katalogová cena",
                SortExpression = "KatalogPriceWVATTotal",
            });
            grid.Columns.Add(new Eurona.Common.Controls.PriceField {
                DataField = "PriceTotalWVAT",
                CurrencySymbolDataField = "CurrencySymbol",
                HeaderText = "Zvýhodnená cena",
                SortExpression = "PriceTotalWVAT",
            });
            CMSButtonField btnDelete = new CMSButtonField();
            btnDelete.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
            btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImage");
            btnDelete.ToolTip = SHP.Resources.Controls.GridView_ToolTip_DeleteItem;
            btnDelete.ButtonType = ButtonType.Image;
            btnDelete.OnClientClick = string.Format("javascript:return confirm('{0}')", SHP.Resources.Controls.DeleteItemQuestion);
            btnDelete.CommandName = DELETE_COMMAND;
            grid.Columns.Add(btnDelete);

            grid.RowCommand += OnRowCommand;

            return grid;
        }

        void grid_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.Footer) {
            } else if (e.Row.RowType == DataControlRowType.DataRow) {
                int columnNameIndex = 0;
                HyperLink hl = (e.Row.Cells[columnNameIndex].Controls[0] as HyperLink);
                CartProductEntity cp = (e.Row.DataItem as CartProductEntity);
                if (!string.IsNullOrEmpty(cp.Alias))
                    hl.NavigateUrl = Page.ResolveUrl(cp.Alias + "?&" + base.BuildReturnUrlQueryParam());

                if (this.IsEditing && Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR))
                    return;

                bool isEditable = this.IsEditing && (OrderEntity.GetOrderStatusFromCode(this.OrderEntity.OrderStatusCode) == OrderEntity.OrderStatus.WaitingForProccess &&
                        (!this.OrderEntity.Payed || Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR)));
                int deleteIndex = e.Row.Cells.Count - 1;
                ImageButton btnDelete = (e.Row.Cells[deleteIndex].Controls[0] as ImageButton);
                btnDelete.Enabled = isEditable;

                if (!btnDelete.Enabled)
                    btnDelete.ImageUrl = ConfigValue("CMS:DeleteButtonImageD");
            }
        }

        void OnRowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
        }

        private void OnDeleteCommand(object sender, GridViewCommandEventArgs e) {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int cartProductId = Convert.ToInt32((sender as GridView).DataKeys[rowIndex].Value);

            EuronaCartHelper.RemoveProductFromCart(cartProductId);

            //Update cart from DB
            this.OrderEntity.CartEntity = null;

            //Prepocitanie kosiku a objednavky
            this.RecalculateOrder();

            GridViewDataBind(this.OrderEntity, true);
        }

        /// <summary>
        /// Metoda prepocita objednavku + kosik danej objednavky
        /// </summary>
        public void RecalculateOrder() {
            //Update cart from DB
            this.OrderEntity.CartEntity = null;

            //Recalculate Cart
            EuronaCartHelper.RecalculateCart(this.Page, this.OrderEntity.CartId);

            //Nastavenie dopravneho
            CartOrderHelper.RecalculateDopravne(this.OrderEntity.CartEntity, this.OrderEntity.ShipmentCode);

            //Update Order
            decimal? shipmentPrice = order.NoPostage ? 0m : (this.OrderEntity.CartEntity.Shipment != null ? this.OrderEntity.CartEntity.Shipment.Price : 0m);
            decimal? shipmentPriceWVAT = this.OrderEntity.CartEntity.DopravneEurosap;

            order.Price = this.OrderEntity.CartEntity.PriceTotal.Value + (shipmentPrice.HasValue ? shipmentPrice.Value : 0m);
            order.PriceWVAT = this.OrderEntity.CartEntity.PriceTotalWVAT.Value + (shipmentPriceWVAT.HasValue ? shipmentPriceWVAT.Value : 0m);
            order = Storage<OrderEntity>.Update(order);
            this.order = null;
        }

        public void OnOrder() {

            //Base kontroly
            if (!Security.IsLogged(true)) return;
            if (this.OrderEntity == null) return;


            this.addressDeliveryControl.UpdateAddress(this.OrderEntity.DeliveryAddress);
            this.OrderEntity.AssociationAccountId = null;
            this.OrderEntity.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.None;
            this.OrderEntity.ParentId = null;
            this.OrderEntity.OrderStatusCode = ((int)OrderEntity.OrderStatus.InProccess).ToString();

            //Validate PSČ
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = this.OrderEntity.AccountId });
            string message = Eurona.Common.PSCHelper.ValidatePSCByPSC(this.OrderEntity.DeliveryAddress.Zip, this.OrderEntity.DeliveryAddress.City, this.OrderEntity.DeliveryAddress.State);
            if (message != string.Empty) {
                string js = string.Format("alert('{0}');", message);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "addValidateOrganization", js, true);
                return;
            }

#if !__DEBUG_VERSION_WITHOUTTVD
            //Prepocitanie objednavky v TVD databazi
            if (CartOrderHelper.RecalculateTVDOrder(this.Page, this.updatePanelCart, this.OrderEntity, true)) {
                //Bonusove kredity sa pripocitavaju iba ak operaciu vykonal poradca
                if (Security.Account.IsInRole(Role.ADVISOR)) {
                    //Zaevidovanie bonusovych kreditov
                    BonusovyKreditUzivateleHelper.ZaevidujKredit(Security.Account.Id, DAL.Entities.Classifiers.BonusovyKreditTyp.OdeslanaObjednavka, this.OrderEntity.ProductsPriceWVAT, "", System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
                    //Zaevidovanie bonusovych kreditov nadriadenemu
                    OrganizationEntity advisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = Security.Account.Id });
                    if (advisor.ParentId.HasValue) {
                        OrganizationEntity parentAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByTVDId { TVD_Id = advisor.ParentId.Value });
                        if (parentAdvisor.AccountId.HasValue) {
                            //Iba za prvu objednavku
                            List<OrderEntity> userOrders = Storage<OrderEntity>.Read(new OrderEntity.ReadByFilter { AccountId = Security.Account.Id });
                            if (userOrders.Count == 1)
                                BonusovyKreditUzivateleHelper.ZaevidujKredit(parentAdvisor.AccountId.Value, DAL.Entities.Classifiers.BonusovyKreditTyp.RegistracePodrizenehoPrvniObjednavka, this.OrderEntity.ProductsPriceWVAT, "", System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
                        }
                    }
                }

                this.OrderEntity.CartEntity.Closed = DateTime.Now;
                this.OrderEntity.CartEntity.SessionId = null;
                Storage<CartEntity>.Update(this.OrderEntity.CartEntity);

                Storage<OrderEntity>.Update(this.OrderEntity);
                //Objednavky pre pridruzenie k tejto objednavke
                OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
                filter.ParentId = this.OrderEntity.Id;
                List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
                foreach (OrderEntity order in list) {
                    order.ParentId = this.OrderEntity.Id;
                    order.ShipmentCode = this.OrderEntity.ShipmentCode;
                    order.OrderStatusCode = this.OrderEntity.OrderStatusCode;
                    Storage<OrderEntity>.Update(order);

                    //Zaevidovanie bonusovych kreditov
                    BonusovyKreditUzivateleHelper.ZaevidujKredit(order.AccountId, DAL.Entities.Classifiers.BonusovyKreditTyp.OdeslanaObjednavka, order.ProductsPriceWVAT, "", System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
                }
            } else return;
#else

			this.OrderEntity.CartEntity.Closed = DateTime.Now;
			this.OrderEntity.CartEntity.SessionId = null;
			Storage<CartEntity>.Update(this.OrderEntity.CartEntity);
			Storage<OrderEntity>.Update(this.OrderEntity);
#endif
            if (string.IsNullOrEmpty(this.FinishUrlFormat))
                return;

            Response.Redirect(Page.ResolveUrl(string.Format(this.FinishUrlFormat, this.OrderEntity.Id)));
        }
    }
}
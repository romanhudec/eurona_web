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
using LastOrderAddressEntity = Eurona.Common.DAL.Entities.LastOrderAddress;
using OrderStatusEntity = SHP.Entities.Classifiers.OrderStatus;
using PaymentEntity = SHP.Entities.Classifiers.Payment;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using ZavozoveMistoEntity = Eurona.Common.DAL.Entities.ZavozoveMisto;
using AddressEntity = CMS.Entities.Address;
using ShpAddressEntity = SHP.Entities.Address;
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
using System.Data;
using CMS.Controls.Page;
using Eurona.user.advisor;

namespace Eurona.Controls {
    public class AdminOrderControl : CmsControl {
        private const string DELETE_COMMAND = "DELETE_ITEM";
        private List<ZavozoveMistoEntity> zavozoveMistaList;
        private List<ZavozoveMistoEntity> zavozoveMistaDatumyList;
        private TextBox txtKod = null;
        private TextBox txtMnozstvi = null;
        private Button btnAddProduct = null;

        private GridView dataGrid = null;
        private List<RadioButton> shipmentRadioButtons = null;
        private AddressControl addressDeliveryControl = null;
        private ASPxDatePicker dtpShipmentFrom = null;
        private ASPxDatePicker dtpShipmentTo = null;
        private CheckBox cbNoPostage = null;
        private LiteralControl lcBodyByEurosap = null;
        private LiteralControl lcKatalogovaCenaCelkemByEurosap = null;
        private LiteralControl lcDopravne = null;
        private Label lblFakturovanaCena = null;
        private bool hasBSRProduct = false;
        private bool pozadujObal = false;
        private bool hasObalProdukt = false;
        private bool hasZavozoveMistoPreStat = false;
        private DropDownList ddlZavozoveMisto_Mesto = null;
        private TableCell cellZavozoveMisto_DatumACas = null;
        private DropDownList ddlZavozoveMisto_DatumACas = null;
        private Table tableZavozoveMistoOsobniOdber;
        private ASPxDatePicker dtpZavozoveMistoOsobniOdberDatum;
        private TextBox txtZavozoveMistoOsobniOdberCas;
        private ObalyControl obalyControl;

        private Button btnSave = null;
        private Image imgSaveHelp = null;
        private Button btnOrder = null;
        private CheckBox cbSaveDeliveryAddressAsMainAddress = null;
        private PageControl orderBottomInfoContent;

        private OrderEntity order = null;
        private UpdatePanel updatePanel = null;

        public AdminOrderControl() {
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

        private ZavozoveMistoEntity findZavozoveMistoByOsobniOdber() {
            foreach (ZavozoveMistoEntity zm in this.zavozoveMistaList) {
                if (zm.OsobniOdberVSidleSpolecnosti) return zm;
            }
            return null;
        }
        private ZavozoveMistoEntity findZavozoveMistoByMesto(string mesto) {
            foreach (ZavozoveMistoEntity zm in this.zavozoveMistaList) {
                if (zm.Mesto == mesto) return zm;
            }
            return null;
        }
        private ZavozoveMistoEntity findZavozoveMistoByDatumACas(DateTime datum) {
            foreach (ZavozoveMistoEntity zm in this.zavozoveMistaDatumyList) {
                if (!zm.DatumACas.HasValue) continue;
                if (zm.DatumACas.Value == datum) return zm;
            }
            return null;
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

            //Nacitanie zavozovych miest pre dany stat
            string stat = ZavozoveMistoEntity.GetStatByLocale(Security.Account.Locale);
            zavozoveMistaList = Storage<ZavozoveMistoEntity>.Read(new ZavozoveMistoEntity.ReadOnlyMestoDistinctByStat { Stat = stat });
            hasZavozoveMistoPreStat = stat == "CZ" ? zavozoveMistaList.Count > 1 : zavozoveMistaList.Count > 0;//CZ  maju aj osobny odber

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.ID = "divOrderNumber";
            div.Attributes.Add("class", this.CssClass + "_orderNumber");
            div.Controls.Add(new LiteralControl(string.Format(SHP.Resources.Controls.OrderControl_OrderNumberFormatText, this.OrderEntity.OrderNumber)));
            this.Controls.Add(div);

            #region Create controls
            this.txtKod = new TextBox();
            this.txtKod.ID = "txtKod";
            this.txtKod.Width = Unit.Pixel(50);
            this.txtMnozstvi = new TextBox();
            this.txtMnozstvi.Width = Unit.Pixel(30);
            this.txtMnozstvi.ID = "txtMnozstvi";
            this.btnAddProduct = new Button();
            this.btnAddProduct.ID = "btnAddProduct";
            this.btnAddProduct.Text = Resources.EShopStrings.CartControl_Add;
            this.btnAddProduct.Click += new EventHandler(btnAddProduct_Click);

            this.dataGrid = CreateGridControl();

            Table mainTable = new Table();
            mainTable.Width = Unit.Percentage(100);
            TableRow mainRow = new TableRow();
            TableCell leftCell = new TableCell(); leftCell.Width = Unit.Percentage(45); leftCell.VerticalAlign = VerticalAlign.Top;
            TableCell rightCell = new TableCell(); rightCell.Width = Unit.Percentage(65); rightCell.HorizontalAlign = HorizontalAlign.Center; rightCell.VerticalAlign = VerticalAlign.Top;
            mainRow.Cells.Add(leftCell); mainRow.Cells.Add(rightCell);
            mainTable.Rows.Add(mainRow);

            this.cbNoPostage = new CheckBox();
            this.cbNoPostage.ID = "cbNoPostage";
            this.cbNoPostage.Enabled = false;
            if (Security.Account.IsInRole(Role.OPERATOR))
                this.cbNoPostage.Enabled = this.IsEditing && this.OrderEntity.OrderStatusCode == ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString();

            //Polozky objednavky
            RoundPanel rpOrderProducts = new RoundPanel();
            rpOrderProducts.ID = "rpOrderProducts";
            rpOrderProducts.CssClass = "_roundPanel";
            if (this.OrderEntity.OrderStatusCode == ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString() && this.IsEditing) {
                Table tableAddProduct = new Table();
                TableRow rowAddP = new TableRow(); tableAddProduct.Rows.Add(rowAddP);
                TableCell cellP = new TableCell(); rowAddP.Cells.Add(cellP); cellP.Controls.Add(new LiteralControl(Resources.EShopStrings.CartControl_ProductCode));
                cellP = new TableCell(); rowAddP.Cells.Add(cellP); cellP.Controls.Add(this.txtKod);
                cellP = new TableCell(); rowAddP.Cells.Add(cellP); cellP.Controls.Add(new LiteralControl(Resources.EShopStrings.CartControl_ProductQuantity));
                cellP = new TableCell(); rowAddP.Cells.Add(cellP); cellP.Controls.Add(this.txtMnozstvi);
                cellP = new TableCell(); rowAddP.Cells.Add(cellP); cellP.Controls.Add(this.btnAddProduct);
                rpOrderProducts.Controls.Add(tableAddProduct);

            }
            rpOrderProducts.Controls.Add(this.dataGrid);
            rpOrderProducts.Text = SHP.Resources.Controls.OrderControl_OrderProducts;

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

            //No postage
            row = new TableRow();
            cell = new TableCell(); cell.CssClass = "form_label"; cell.HorizontalAlign = HorizontalAlign.Right; cell.Controls.Add(new LiteralControl("Bez poštovného"));
            row.Cells.Add(cell);
            cell = new TableCell(); cell.Controls.Add(this.cbNoPostage);
            row.Cells.Add(cell);
            tableUserInfo.Rows.Add(row);

            #endregion
            leftCell.Controls.Add(tableUserInfo);

            #region Informacie o cene
            Table priceInfoTable = new Table();
            rightCell.Controls.Add(priceInfoTable);
            TableRow rowPrice = null;

            //Body
            this.lcBodyByEurosap = new LiteralControl(OrderEntity.CartEntity.BodyEurosapTotal.ToString("F1"));
            this.lcBodyByEurosap.ID = "lcBodyByEurosap";
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
            this.lcKatalogovaCenaCelkemByEurosap = new LiteralControl(Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.KatalogovaCenaCelkemByEurosap, OrderEntity.CurrencySymbol));
            this.lcKatalogovaCenaCelkemByEurosap.ID = "lcKatalogovaCenaCelkemByEurosap";

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

            //Dopravne
            if (OrderEntity.CartEntity.DopravneEurosap.HasValue) {
                this.lcDopravne = new LiteralControl(Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.DopravneEurosap, OrderEntity.CurrencySymbol));
                this.lcDopravne.ID = "lcDopravne";
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

            //Fakturovana Cena
            this.lblFakturovanaCena = new Label();
            this.lblFakturovanaCena.ID = "lblFakturovanaCena";
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
            this.cbSaveDeliveryAddressAsMainAddress = new CheckBox();
            this.cbSaveDeliveryAddressAsMainAddress.ID = "cbSaveDeliveryAddressAsMainAddress";
            this.cbSaveDeliveryAddressAsMainAddress.AutoPostBack = false;

            this.orderBottomInfoContent = new PageControl();
            this.orderBottomInfoContent.ID = "genericPage";
            this.orderBottomInfoContent.NewUrl = "~/admin/page.aspx";
            this.orderBottomInfoContent.ManageUrl = "~/admin/pages.aspx";
            this.orderBottomInfoContent.NotFoundUrlFormat = "~/notFound.aspx?page={0}";
            this.orderBottomInfoContent.PageName = "order-bottom-info-content";
            this.orderBottomInfoContent.PopUpEditorUrlFormat = "~/admin/contentEditor.aspx?id={0}";
            this.orderBottomInfoContent.CssEditorToolBar = "contentEditorToolbar";
            this.orderBottomInfoContent.CssEditorContent = "contentEditorContent";

            this.btnSave = new Button();
            this.btnSave.ID = "btnSave";
            this.btnSave.Attributes.Add("name", "btnSave");
            this.btnSave.CssClass = "button-save";
            this.btnSave.CausesValidation = true;
            this.btnSave.Text = SHP.Resources.Controls.SaveButton_Text.ToUpper();
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnSave.Visible = this.IsEditing;

            this.btnOrder = new Button();
            this.btnOrder.ID = "btnOrder";
            this.btnOrder.Attributes.Add("name", "btnOrder");
            this.btnOrder.CssClass = "button-order-disabled";
            this.btnOrder.CausesValidation = true;
            this.btnOrder.Text = Resources.EShopStrings.CartControl_OrderAndPayButton_Text.ToUpper();
            this.btnOrder.Click += new EventHandler(OnOrder);
            this.btnOrder.Visible = this.IsEditing && (!this.OrderEntity.ParentId.HasValue && this.OrderEntity.OrderStatusCode == ((int)OrderEntity.OrderStatus.WaitingForProccess).ToString());
            this.btnOrder.Enabled = false;

            #region Dopravne
            RoundPanel rpDopravne = new RoundPanel();
            rpDopravne.ID = "rpDopravne";
            rpDopravne.CssClass = "roundPanel";
            rpOrderProducts.Controls.Add(rpDopravne);

            Table tblDopravneTitle = new Table();
            tblDopravneTitle.CssClass = "order-warning-table";
            row = new TableRow(); tblDopravneTitle.Rows.Add(row);
            cell = new TableCell(); row.Cells.Add(cell);
            LiteralControl lcDopravneTitleText = new LiteralControl("<span class='order-warning-table-label' style='width:600px;'><b>" + SHP.Resources.Controls.OrderControl_Shipment + "</b></span>");
            cell.Controls.Add(lcDopravneTitleText);
            rpDopravne.Controls.Add(tblDopravneTitle);

            List<ShipmentEntity> shipments = Storage<ShipmentEntity>.Read();
            Table tableShipment = new Table();
            TableRow shipmentLabelRow = new TableRow();
            tableShipment.Rows.Add(shipmentLabelRow);

            TableRow shipmentRow = new TableRow();
            tableShipment.Rows.Add(shipmentRow);
            shipmentRadioButtons = new List<RadioButton>();
            foreach (ShipmentEntity shipment in shipments) {
                if (shipment.Hide == true && IsEditing) continue;
                RadioButton rbShipment = new RadioButton();
                rbShipment.GroupName = "rbShipment";
                rbShipment.ID = rbShipment.GroupName + "_" + shipment.Code;
                rbShipment.Text = shipment.Name;
                rbShipment.AutoPostBack = true;
                rbShipment.Enabled = IsEditing;
                shipmentRadioButtons.Add(rbShipment);
            }
            foreach (RadioButton rbShipment in shipmentRadioButtons) {
                TableCell shipmentCell = new TableCell();
                shipmentCell.CssClass = "form_label"; shipmentCell.HorizontalAlign = HorizontalAlign.Right;
                shipmentCell.Style.Add("padding-right", "20px");
                shipmentCell.Controls.Add(rbShipment);
                shipmentRow.Cells.Add(shipmentCell);

                rbShipment.CheckedChanged += rbShipment_CheckedChanged;
            }
            rpDopravne.Controls.Add(CreateTableRow(tableShipment));
            #endregion

            //#region Warning
            //RoundPanel rpWarningZavozoveMisto = new RoundPanel();
            //rpWarningZavozoveMisto.ID = "rpWarningZavozoveMisto";
            //rpWarningZavozoveMisto.CssClass = "roundPanel";
            //rpOrderProducts.Controls.Add(rpWarningZavozoveMisto);
            //LiteralControl lcZavozoveMistoWarning = new LiteralControl("<span style='width:600px;'><b style='color:#FF0000!important;font-size:20px!important;'>Pro daný stát není v tuto chvíli vypsáno žádné závozové místo pro vyzvednutí chlazených rybích produktů</b></span>");
            //rpWarningZavozoveMisto.Controls.Add(lcZavozoveMistoWarning);
            //rpWarningZavozoveMisto.Visible = false;
            //#endregion

            #region ZavozoveMisto
            List<CartProductEntity> cartProducts = this.order.CartEntity.CartProducts;
            foreach (CartProductEntity cartProduct in cartProducts) {
                if (cartProduct.BSRProdukt) {
                    this.hasBSRProduct = true;
                    break;
                }
            }
            if (this.hasBSRProduct) {
                RoundPanel rpZavozoveMisto = new RoundPanel();
                rpZavozoveMisto.ID = "rpZavozoveMisto";
                rpZavozoveMisto.CssClass = "roundPanel";
                rpOrderProducts.Controls.Add(rpZavozoveMisto);

                Table tblZavozoveMistoTitle = new Table();
                tblZavozoveMistoTitle.CssClass = "order-warning-table";
                row = new TableRow(); tblZavozoveMistoTitle.Rows.Add(row);
                cell = new TableCell(); row.Cells.Add(cell);
                LiteralControl lcZavozoveMistoTitle = new LiteralControl("<span class='order-warning-table-label' style='width:600px;'><b>ZÁVOZOVÉ MÍSTO ČERSTVÝCH RYB A CHLAZENÝCH PRODUKTÚ</b></span>");
                cell.Controls.Add(lcZavozoveMistoTitle);
                rpZavozoveMisto.Controls.Add(tblZavozoveMistoTitle);


                if (this.IsEditing) {
                    //rpWarningZavozoveMisto.Visible = false;
                    //if (!hasZavozoveMistoPreStat) {
                    //    rpWarningZavozoveMisto.Visible = true;
                    //}
                    ZavozoveMistoEntity emptyZavozoveMisto = new ZavozoveMistoEntity();
                    if (hasZavozoveMistoPreStat) {
                        if (this.OrderEntity.ZavozoveMisto_OsobniOdberVSidleSpolecnosti == false && !String.IsNullOrEmpty(this.OrderEntity.ZavozoveMisto_Mesto)) {
                            if (!checkIfBindedZavozoveMistoMestoIsInDatasource(zavozoveMistaList, this.OrderEntity.ZavozoveMisto_Mesto)) {
                                ZavozoveMistoEntity newZavozoveMisto = new ZavozoveMistoEntity();

                                newZavozoveMisto.Mesto = this.OrderEntity.ZavozoveMisto_Mesto;
                                newZavozoveMisto.DatumACas = this.OrderEntity.ZavozoveMisto_DatumACas.Value;
                                zavozoveMistaList.Add(newZavozoveMisto);
                            }
                        }
                    }

                    emptyZavozoveMisto.Id = 999999;
                    emptyZavozoveMisto.Mesto = "-- Vyberte město --";
                    zavozoveMistaList.Insert(0, emptyZavozoveMisto);

                    this.ddlZavozoveMisto_Mesto = new DropDownList();
                    this.ddlZavozoveMisto_Mesto.ID = "ddlZavozoveMisto_Mesto";
                    this.ddlZavozoveMisto_Mesto.Attributes.Add("name", "ddlZavozoveMisto_Mesto");
                    this.ddlZavozoveMisto_Mesto.AutoPostBack = true;
                    this.ddlZavozoveMisto_Mesto.DataSource = zavozoveMistaList;
                    this.ddlZavozoveMisto_Mesto.DataTextField = "DisplayMesto";
                    this.ddlZavozoveMisto_Mesto.DataValueField = "Kod";
                    this.ddlZavozoveMisto_Mesto.Enabled = this.IsEditing && (!this.OrderEntity.Payed || Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR));
                    this.ddlZavozoveMisto_Mesto.Width = Unit.Pixel(350);
                    this.ddlZavozoveMisto_Mesto.SelectedIndexChanged += new EventHandler(ddlZavozoveMisto_Mesto_SelectedIndexChanged);

                    this.ddlZavozoveMisto_DatumACas = new DropDownList();
                    this.ddlZavozoveMisto_DatumACas.ID = "ddlZavozoveMisto_DatumACas";
                    this.ddlZavozoveMisto_DatumACas.Attributes.Add("name", "ddlZavozoveMisto_DatumACas");
                    this.ddlZavozoveMisto_DatumACas.AutoPostBack = false;
                    this.ddlZavozoveMisto_DatumACas.DataTextField = "DatumACas";
                    this.ddlZavozoveMisto_DatumACas.DataValueField = "Id";
                    this.ddlZavozoveMisto_DatumACas.Enabled = this.IsEditing && (!this.OrderEntity.Payed || Security.Account.IsInRole(CMS.Entities.Role.ADMINISTRATOR));
                    this.ddlZavozoveMisto_DatumACas.Width = Unit.Pixel(150);

                    this.dtpZavozoveMistoOsobniOdberDatum = new ASPxDatePicker();
                    this.dtpZavozoveMistoOsobniOdberDatum.ID = "dtpZavozoveMistoOsobniOdberDatum";
                    this.dtpZavozoveMistoOsobniOdberDatum.Width = Unit.Pixel(100);
                    this.txtZavozoveMistoOsobniOdberCas = new TextBox();
                    this.txtZavozoveMistoOsobniOdberCas.ID = "txtZavozoveMistoOsobniOdberCas";
                    this.txtZavozoveMistoOsobniOdberCas.Width = Unit.Pixel(50);

                    Table tableZavozoveMisto = new Table();
                    tableZavozoveMisto.CellPadding = 0;
                    tableZavozoveMisto.CellSpacing = 0;
                    rpZavozoveMisto.Controls.Add(tableZavozoveMisto);
                    TableRow zavozoveMistoRow = new TableRow();
                    tableZavozoveMisto.Rows.Add(zavozoveMistoRow);
                    //Mesto
                    cell = new TableCell();
                    cell.VerticalAlign = VerticalAlign.Top;
                    cell.Controls.Add(new LiteralControl("<span style='padding:5px;'>Město:</span>&nbsp;"));
                    zavozoveMistoRow.Cells.Add(cell);
                    cell = new TableCell();
                    cell.VerticalAlign = VerticalAlign.Top;
                    cell.Controls.Add(ddlZavozoveMisto_Mesto);
                    zavozoveMistoRow.Cells.Add(cell);

                    //DatumACas
                    cellZavozoveMisto_DatumACas = new TableCell();
                    cellZavozoveMisto_DatumACas.VerticalAlign = VerticalAlign.Top;
                    cellZavozoveMisto_DatumACas.Controls.Add(new LiteralControl("<span style='padding:5px;'>Datum a čas:</span>&nbsp;"));
                    cellZavozoveMisto_DatumACas.Controls.Add(ddlZavozoveMisto_DatumACas);
                    zavozoveMistoRow.Cells.Add(cellZavozoveMisto_DatumACas);

                    //Zavozove misto Osobni odber
                    #region Zavozove misto Osobni odber
                    cell = new TableCell();
                    zavozoveMistoRow.Cells.Add(cell);
                    tableZavozoveMistoOsobniOdber = new Table();
                    tableZavozoveMistoOsobniOdber.CellPadding = 0;
                    tableZavozoveMistoOsobniOdber.CellSpacing = 0;
                    cell.Controls.Add(tableZavozoveMistoOsobniOdber);

                    TableRow zavozoveMistoOsobniOdberRow = new TableRow();
                    tableZavozoveMistoOsobniOdber.Rows.Add(zavozoveMistoOsobniOdberRow);
                    cell = new TableCell();
                    cell.VerticalAlign = VerticalAlign.Top;
                    cell.Controls.Add(new LiteralControl("<span style='padding:5px;'>Datum:</span>&nbsp;"));
                    zavozoveMistoOsobniOdberRow.Cells.Add(cell);
                    cell = new TableCell();
                    cell.VerticalAlign = VerticalAlign.Top;
                    cell.Controls.Add(this.dtpZavozoveMistoOsobniOdberDatum);
                    zavozoveMistoOsobniOdberRow.Cells.Add(cell);
                    cell = new TableCell();
                    cell.VerticalAlign = VerticalAlign.Top;
                    cell.Controls.Add(new LiteralControl("<span style='padding:5px;'>Čas:</span>&nbsp;"));
                    zavozoveMistoOsobniOdberRow.Cells.Add(cell);
                    cell = new TableCell();
                    cell.VerticalAlign = VerticalAlign.Top;
                    cell.Controls.Add(this.txtZavozoveMistoOsobniOdberCas);
                    cell.Controls.Add(new LiteralControl("<span style='padding:5px;'>hh:mm</span>"));
                    zavozoveMistoOsobniOdberRow.Cells.Add(cell);

                    ZavozoveMistoEntity zavozoveMisto = Storage<ZavozoveMistoEntity>.ReadFirst(new ZavozoveMistoEntity.ReadJenAktualiByKod { Kod = 1 });
                    if (zavozoveMisto != null) {
                        Eurona.Common.DAL.Entities.ZavozoveMistoLimit zavozoveMistoLimit = new Eurona.Common.DAL.Entities.ZavozoveMistoLimit(zavozoveMisto.OsobniOdberPovoleneCasy);
                        string limitsDisplayString = zavozoveMistoLimit.ToDisplayString();
                        TableRow zavozoveMistoOsobniOdberLimitRow = new TableRow();
                        tableZavozoveMistoOsobniOdber.Rows.Add(zavozoveMistoOsobniOdberLimitRow);
                        cell = new TableCell();
                        cell.ColumnSpan = 4;
                        zavozoveMistoOsobniOdberLimitRow.Cells.Add(cell);
                        cell.Controls.Add(new LiteralControl("<div style='margin-left:10px;margin-top:10px;color:#0077b6;text-align=center;'>" + limitsDisplayString + "</div>"));
                    }
                    tableZavozoveMistoOsobniOdber.Visible = false;
                    cellZavozoveMisto_DatumACas.Visible = true;
                    #endregion

                } else {
                    Table tableZavozoveMisto = new Table();
                    rpZavozoveMisto.Controls.Add(tableZavozoveMisto);
                    TableRow zavozoveMistoRow = new TableRow();
                    tableZavozoveMisto.Rows.Add(zavozoveMistoRow);
                    //Mesto
                    cell = new TableCell();
                    cell.Controls.Add(new LiteralControl("<span>Město:</span>"));
                    zavozoveMistoRow.Cells.Add(cell);
                    cell = new TableCell();
                    cell.Controls.Add(new LiteralControl("<span><b>" + this.OrderEntity.ZavozoveMisto_Mesto + "</b></span>"));
                    zavozoveMistoRow.Cells.Add(cell);

                    //DatumACas
                    cell = new TableCell();
                    cell.Controls.Add(new LiteralControl("<span>Datum a čas:</span>"));
                    zavozoveMistoRow.Cells.Add(cell);
                    cell = new TableCell();
                    cell.Controls.Add(new LiteralControl("<span><b>" + this.OrderEntity.ZavozoveMisto_DatumACas + "</b></span>"));
                    zavozoveMistoRow.Cells.Add(cell);
                }
            }
            #endregion

            #region Obaly
            foreach (CartProductEntity cartProduct in cartProducts) {
                if (cartProduct.PozadujObal) {
                    this.pozadujObal = true;
                }
                if (cartProduct.Obal) {
                    this.hasObalProdukt = true;
                }
            }

            if (this.pozadujObal && this.hasObalProdukt == false) {
                RoundPanel rpObaly = new RoundPanel();
                rpObaly.ID = "rpObaly";
                rpObaly.CssClass = "roundPanel";
                rpOrderProducts.Controls.Add(rpObaly);

                Table tblObalyTitle = new Table();
                tblObalyTitle.CssClass = "order-warning-table";
                row = new TableRow(); tblObalyTitle.Rows.Add(row);
                cell = new TableCell(); row.Cells.Add(cell);
                LiteralControl lcObalyTitle = new LiteralControl("<span class='order-warning-table-label' style='width:600px;'><b>ZVOLTE OBALY PRO CHLAZENÉ RYBÍ PRODUKTY</b></span>");
                cell.Controls.Add(lcObalyTitle);
                rpObaly.Controls.Add(tblObalyTitle);

                if (this.IsEditing) {
                    this.obalyControl = (ObalyControl)this.Page.LoadControl("~/user/advisor/ObalyControl.ascx");
                    this.obalyControl.ID = "obalyControl";
                    rpObaly.Controls.Add(this.obalyControl);
                    this.obalyControl.OnAddObalProduct += obalyControl_OnAddObalProduct;
                } else {
                }
            }
            #endregion

            #region Address
            RoundPanel rpAddress = new RoundPanel();
            rpAddress.ID = "rpAddress";
            rpAddress.CssClass = "roundPanel";
            rpOrderProducts.Controls.Add(rpAddress);

            #region Title
            Table tblWarning = new Table();
            tblWarning.CssClass = "order-warning-table";
            row = new TableRow(); tblWarning.Rows.Add(row);
            cell = new TableCell(); row.Cells.Add(cell);
            LiteralControl lcWarningText = new LiteralControl("<span class='order-warning-table-label' style='width:600px;'>" + Resources.EShopStrings.AdminOrderControl_warning + "</span>");
            cell.Controls.Add(lcWarningText);
            rpAddress.Controls.Add(tblWarning);
            #endregion

            #region Addresses
            bool isOperator = Security.IsLogged(false) && Security.Account.IsInRole(Role.OPERATOR);
            bool isAdmin = Security.IsLogged(false) && Security.Account.IsInRole(Role.ADMINISTRATOR);
            //this.addressDeliveryControl = new AddressControl("Kód voucheru:");
            this.addressDeliveryControl = new AddressControl();
            this.addressDeliveryControl.ID = "addressDeliveryControl";
            this.addressDeliveryControl.Width = Unit.Percentage(100);
            this.addressDeliveryControl.IsEditing = this.IsEditing;
            this.addressDeliveryControl.AddressId = this.OrderEntity.DeliveryAddressId;
            rpAddress.Controls.Add(this.addressDeliveryControl);
            this.addressDeliveryControl.EnableFirstName(false);
            this.addressDeliveryControl.EnableLastName(false);
            this.addressDeliveryControl.EnableState(isOperator || isAdmin);
            #endregion
            #endregion
            #region Buttons
            //Tlacidla
            Table buttonsTable = new Table();
            buttonsTable.CssClass = "order-buttons-table";
            buttonsTable.Width = Unit.Percentage(100);

            row = new TableRow(); buttonsTable.Rows.Add(row);
            this.cbSaveDeliveryAddressAsMainAddress.Text = Resources.EShopStrings.AdminOrderControl_save_delivery_addres_as_main;
            this.cbSaveDeliveryAddressAsMainAddress.CssClass = "checkbox";
            cell = new TableCell(); cell.ColumnSpan = 2; row.Cells.Add(cell); cell.Controls.Add(this.cbSaveDeliveryAddressAsMainAddress);

            //Add custom content page
            row = new TableRow(); buttonsTable.Rows.Add(row);
            cell = new TableCell(); cell.ColumnSpan = 2; row.Cells.Add(cell); cell.Controls.Add(this.orderBottomInfoContent);

            HtmlGenericControl divBtnSave = new HtmlGenericControl();
            divBtnSave.Controls.Add(btnSave);
            this.imgSaveHelp = new Image();
            this.imgSaveHelp.ToolTip = Resources.EShopStrings.AdminOrderControl_save_text;
            this.imgSaveHelp.CssClass = "button-save-help-image has-tooltip";
            this.imgSaveHelp.ImageUrl = Page.ResolveUrl("~/images/help.png");
            divBtnSave.Controls.Add(imgSaveHelp);

            row = new TableRow(); buttonsTable.Rows.Add(row);
            cell = new TableCell(); row.Cells.Add(cell); cell.HorizontalAlign = HorizontalAlign.Left; cell.Controls.Add(divBtnSave);
            //Check na uzavierku
            if (Security.Account.IsInRole(Role.ADVISOR) && Eurona.Common.Application.EuronaUzavierka.IsUzavierka4Advisor()) {
                cell = new TableCell(); row.Cells.Add(cell);
                cell.Controls.Add(new LiteralControl(string.Format("<span style='color:red;font-weight:bold;'>Probíhá uzávěrka. Vytvářet objednývky bude možné : {0}</span>", Eurona.Common.Application.EuronaUzavierka.GeUzavierka4AdvisorTo())));
            } else {
                cell = new TableCell(); row.Cells.Add(cell); //cell.Controls.Add(new LiteralControl(/*"<span style='color:#0077b6;'>" + Resources.EShopStrings.AdminOrderControl_order_text + "</span>"*/));
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.Controls.Add(btnOrder);
            }

            rpOrderProducts.Controls.Add(new LiteralControl("<br/>"));
            //rpOrderProducts.Controls.Add(buttonsTable);
            #endregion

            RoundPanel rpButtons = new RoundPanel();
            rpButtons.CssClass = "roundPanel";
            rpButtons.ID = "rpButtons";
            rpOrderProducts.Controls.Add(rpButtons);

            //Update Panel AJAX
            this.updatePanel = new UpdatePanel();
            this.updatePanel.ID = "updatePanelCart";
            this.updatePanel.ContentTemplateContainer.Controls.Add(buttonsTable);
            UpdateProgressControl upp = new UpdateProgressControl();
            upp.ID = "upp";
            upp.AssociatedUpdatePanelID = this.updatePanel.ID;
            upp.CssClass = "cart-update-progress";
            rpButtons.Controls.Add(updatePanel);
            rpButtons.Controls.Add(upp);
            rpButtons.Visible = this.IsEditing;
            #endregion

            this.Controls.Add(rpOrderProducts);

            #endregion

            #region Binding
            if (!IsPostBack) {
                if (!isOperator) this.cbSaveDeliveryAddressAsMainAddress.Checked = true;
                if (this.OrderEntity.AssociationAccountId.HasValue) {
                    string js = " return confirm('Objednávka je přidružená k objednávce jiného poradce! Objednánim (uzavřením objednávky) se toto přidružení zruší. Skutečne si přejete uzavřít objednávku?');";
                    this.btnOrder.Attributes.Add("onclick", js);
                }

                if (this.dtpShipmentFrom != null) this.dtpShipmentFrom.Value = this.OrderEntity.ShipmentFrom;
                if (this.dtpShipmentTo != null) this.dtpShipmentTo.Value = this.OrderEntity.ShipmentTo;
                this.cbNoPostage.Checked = this.OrderEntity.NoPostage;


                if (this.IsEditing) {
                    if (this.ddlZavozoveMisto_Mesto != null) {
                        if (!string.IsNullOrEmpty(this.OrderEntity.ZavozoveMisto_Mesto)) {
                            ZavozoveMistoEntity zavozoveMisto = null;
                            if (this.OrderEntity.ZavozoveMisto_OsobniOdberVSidleSpolecnosti == true) {
                                zavozoveMisto = findZavozoveMistoByOsobniOdber();
                            } else {
                                zavozoveMisto = findZavozoveMistoByMesto(this.OrderEntity.ZavozoveMisto_Mesto);
                            }
                            if (zavozoveMisto != null) this.ddlZavozoveMisto_Mesto.SelectedValue = zavozoveMisto.Kod.ToString();
                        }
                        this.ddlZavozoveMisto_Mesto.DataBind();
                    }
                    if (this.OrderEntity.ZavozoveMisto_DatumACas.HasValue) {
                        this.dtpZavozoveMistoOsobniOdberDatum.Value = this.OrderEntity.ZavozoveMisto_DatumACas.Value;
                        this.txtZavozoveMistoOsobniOdberCas.Text = string.Format("{0:00}:{1:00}", this.OrderEntity.ZavozoveMisto_DatumACas.Value.Hour, this.OrderEntity.ZavozoveMisto_DatumACas.Value.Minute);
                    }
                    if (this.ddlZavozoveMisto_DatumACas != null) {
                        if (this.OrderEntity.ZavozoveMisto_DatumACas.HasValue) {
                            ddlZavozoveMisto_Mesto_SelectedIndexChanged(this.ddlZavozoveMisto_Mesto, null);
                            ZavozoveMistoEntity zavozoveMisto = findZavozoveMistoByDatumACas(this.OrderEntity.ZavozoveMisto_DatumACas.Value);
                            if (zavozoveMisto != null) {
                                this.ddlZavozoveMisto_DatumACas.SelectedValue = zavozoveMisto.Id.ToString();
                                this.ddlZavozoveMisto_DatumACas.DataBind();
                            }

                        }
                    }

                }

                if (!string.IsNullOrEmpty(this.OrderEntity.ShipmentCode)) {
                    SetShipmentSelection(this.OrderEntity.ShipmentCode);
                }
            }
            #endregion

            string function_script = @"function fnOnUpdateValidators()
                {
                   var btnOrder = document.getElementById('" + this.btnOrder.ClientID + @"');
                   btnOrder.disabled = true;
                   btnOrder.className = 'button-order-disabled';
                   var isAllValid = true;
                   for (var i = 0; i < Page_Validators.length; i++)
                   {
                      var val = Page_Validators[i];
                      var ctrl = document.getElementById(val.controltovalidate);
                      if (ctrl != null && ctrl.style != null)
                      {
                         if (!val.isvalid){
                            ctrl.style.background = '#EFB6E6';
                            isAllValid = false;
                         }
                         else
                            ctrl.style.backgroundColor = '';
                      }
                   }                   
                   if( !isAllValid ){
                        btnOrder.disabled = false;
                        btnOrder.className = 'button-order';
                        $.unblockUI();
                        return false;
                   }  
                   return true;                 
                }";
            string submit_function_script = @"function fnOnPageSubmit()
                {
                   var btnOrder = document.getElementById('" + this.btnOrder.ClientID + @"');
                   btnOrder.disabled = true;
                   btnOrder.className = 'button-order-disabled';    
                    
                   blockUIProcessing('" + Resources.Strings.Please_Wait + @"');                   
                }";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "update_function_validator", function_script, true);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "function_onPageSubmit", submit_function_script, true);
            Page.ClientScript.RegisterOnSubmitStatement(this.GetType(), "on_order_submit_script", "fnOnPageSubmit();");

            //29.04.2014
            this.btnOrder.CssClass = "button-order";
            this.btnOrder.Enabled = true;

            string orderClickHandler = @"if(fnOnUpdateValidators()){
                this.value='Probíhá objednávka...'; 
                this.disabled=true; 
                this.className='button-order-disabled';" +
                Page.ClientScript.GetPostBackEventReference(this.btnOrder, string.Empty) +
                @";return false;}else{return false;}";
            this.btnOrder.Attributes.Add("onclick", orderClickHandler);

            string saveClickHandler = @"if(fnOnUpdateValidators()){" +
            Page.ClientScript.GetPostBackEventReference(this.btnSave, string.Empty) +
            @";return false;}else{return false;}";
            this.btnSave.Attributes.Add("onclick", saveClickHandler);

            string addProductClickHandler = @"if(fnOnUpdateValidators()){" +
            Page.ClientScript.GetPostBackEventReference(this.btnAddProduct, string.Empty) +
            @";return false;}else{return false;}";
            this.btnAddProduct.Attributes.Add("onclick", addProductClickHandler);

            //Binding
            GridViewDataBind(this.OrderEntity, !IsPostBack);

        }

        void obalyControl_OnAddObalProduct(int productId, int quantity) {
            ProductEntity p = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = productId });
            //bool isOperator = Security.IsLogged(false) && Security.Account.IsInRole(Role.OPERATOR);
            //if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod.Text, p, quantity, false, this, isOperator))
            //    return;

            bool updateResult = false;
            EuronaCartHelper.UpdateCartProduct(this.Page, this.OrderEntity.CartId, p.Id, quantity, out updateResult, false);
            if (updateResult == false)
                return;

            //Prepocitanie kosiku a objednavky           
            this.RecalculateOrder();
            UpdateDopravneUIbyOrder();

            OnSaveInternalToNextProcessing();
            Response.Redirect(this.Request.RawUrl);
        }

        void rbShipment_CheckedChanged(object sender, EventArgs e) {
            RadioButton rbShipment = (RadioButton)sender;
            string value = rbShipment.ID.Replace(rbShipment.GroupName + "_", "");
            if (value != null) this.OrderEntity.ShipmentCode = value;

            this.OrderEntity.NoPostage = this.cbNoPostage.Checked;
            this.OrderEntity.ShipmentCode = GetShipmentSelection();
            Storage<OrderEntity>.Update(this.OrderEntity);

            //Prepocitanie kosiku a objednavky           
            this.RecalculateOrder();
            UpdateDopravneUIbyOrder();
        }

        private string GetShipmentSelection() {
            foreach (RadioButton rbShipment in shipmentRadioButtons) {
                if (rbShipment.Checked) return rbShipment.ID.Replace(rbShipment.GroupName + "_", "");
            }
            return null;
        }

        private void SetShipmentSelection(string shipmentCode) {
            foreach (RadioButton rbShipment in shipmentRadioButtons) {
                string code = rbShipment.ID.Replace(rbShipment.GroupName + "_", "");
                if (code == shipmentCode) rbShipment.Checked = true;
                else rbShipment.Checked = false;
            }
        }


        void ddlZavozoveMisto_Mesto_SelectedIndexChanged(object sender, EventArgs e) {
            int zavozoveMistoKod = 0;
            if (!string.IsNullOrEmpty(this.ddlZavozoveMisto_Mesto.SelectedValue)) {
                zavozoveMistoKod = Convert.ToInt32(this.ddlZavozoveMisto_Mesto.SelectedValue);
            }

            if (zavozoveMistoKod == 1) {
                tableZavozoveMistoOsobniOdber.Visible = true;
                cellZavozoveMisto_DatumACas.Visible = false;
            } else {
                tableZavozoveMistoOsobniOdber.Visible = false;
                cellZavozoveMisto_DatumACas.Visible = true;
            }

            if (this.ddlZavozoveMisto_DatumACas == null) return;

            int kod = Convert.ToInt32(this.ddlZavozoveMisto_Mesto.SelectedValue);
            ZavozoveMistoEntity emptyDatum = new ZavozoveMistoEntity();
            emptyDatum.Id = 0;
            emptyDatum.Kod = 0;
            emptyDatum.DatumACas = null;
            zavozoveMistaDatumyList = Storage<ZavozoveMistoEntity>.Read(new ZavozoveMistoEntity.ReadJenAktualiByKod() { Kod = kod });
            string mesto = zavozoveMistaDatumyList.Count != 0 ? zavozoveMistaDatumyList[0].Mesto : "";
            zavozoveMistaDatumyList.Insert(0, emptyDatum);
            if (this.OrderEntity.ZavozoveMisto_DatumACas.HasValue) {
                if (!checkIfBindedZavozoveMistoDatumIsInDatasource(zavozoveMistaDatumyList, this.OrderEntity.ZavozoveMisto_DatumACas.Value) && this.OrderEntity.ZavozoveMisto_Mesto == mesto) {
                    ZavozoveMistoEntity newZavozoveMisto = new ZavozoveMistoEntity();
                    newZavozoveMisto.Mesto = mesto;
                    newZavozoveMisto.DatumACas = this.OrderEntity.ZavozoveMisto_DatumACas.Value;
                    zavozoveMistaDatumyList.Add(newZavozoveMisto);
                }
            }
            this.ddlZavozoveMisto_DatumACas.DataSource = zavozoveMistaDatumyList;
            this.ddlZavozoveMisto_DatumACas.DataBind();
        }

        private bool checkIfBindedZavozoveMistoMestoIsInDatasource(List<ZavozoveMistoEntity> zavozoveMistaDatumyList, string mesto) {
            foreach (ZavozoveMistoEntity zavozoveMisto in zavozoveMistaDatumyList) {
                if (zavozoveMisto.Mesto == mesto) return true;
            }
            return false;
        }
        private bool checkIfBindedZavozoveMistoDatumIsInDatasource(List<ZavozoveMistoEntity> zavozoveMistaDatumyList, DateTime datumACas) {
            foreach (ZavozoveMistoEntity zavozoveMisto in zavozoveMistaDatumyList) {
                if (zavozoveMisto.DatumACas == datumACas) return true;
            }
            return false;
        }

        void btnAddProduct_Click(object sender, EventArgs e) {
            int quantity = 1;
            if (!Int32.TryParse(this.txtMnozstvi.Text, out quantity)) quantity = 1;

            if (string.IsNullOrEmpty(this.txtMnozstvi.Text) && String.IsNullOrEmpty(this.txtKod.Text))
                return;

            ProductEntity p = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadByCode { Code = this.txtKod.Text });
            #region Validacia BSR produktu a existencia zavozoveho miesta
            if (!hasZavozoveMistoPreStat && p.BSR) {
                string message = "Pro daný stát není v tuto chvíli vypsáno žádné závozové místo pro vyzvednutí chlazených rybích produktů";
                string js = string.Format("blockUIAlert('', '{0}');", message);
                ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateZavozoveMistoProdukt", js, true);
                return;
            }
            #endregion

            bool isOperator = Security.IsLogged(false) && Security.Account.IsInRole(Role.OPERATOR);
            if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(this.txtKod.Text, p, quantity, false, this, isOperator))
                return;

            bool updateResult = false;
            EuronaCartHelper.UpdateCartProduct(this.Page, this.OrderEntity.CartId, p.Id, quantity, out updateResult, false);
            if (updateResult == false)
                return;

            //Prepocitanie kosiku a objednavky           
            this.RecalculateOrder();
            UpdateDopravneUIbyOrder();

            OnSaveInternalToNextProcessing();
            Response.Redirect(this.Request.RawUrl);

        }
        #endregion


        private TableRow CreateTableRow(Control control) {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.ColumnSpan = 2;
            cell.Controls.Add(control);
            row.Cells.Add(cell);
            return row;
        }

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

        private void UpdateDopravneUIbyOrder() {
            this.lcDopravne.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.DopravneEurosap, OrderEntity.CurrencySymbol);
            this.lblFakturovanaCena.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(this.OrderEntity.PriceWVAT, this.OrderEntity.CurrencySymbol);
        }

        private void GridViewDataBind(OrderEntity order, bool bind) {
            List<CartProductEntity> list = Storage<CartProductEntity>.Read(new CartProductEntity.ReadByCart { CartId = order.CartId, Locale = order.CartEntity.Locale });

            this.dataGrid.PagerTemplate = null;
            dataGrid.DataSource = list;
            if (bind) {
                dataGrid.DataKeyNames = new string[] { "Id" };
                dataGrid.DataBind();
            }
        }

        private GridView CreateGridControl() {
            GridView grid = new GridView();
            grid.ID = "dgProducts";
            grid.Attributes.Add("name", "dgProducts");
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
                    if (cartProduct.CerpatBonusoveKredity == true) {
                        string jsAlert = string.Format("alert('Nákup za bonusové kredity je možný pouze v sekcie \"Bonusové kredity\"');");
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "addProductToCart", jsAlert, true);

                        GridViewDataBind(this.OrderEntity, true);
                        return;
                    }
                    ProductEntity p = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = cartProduct.ProductId });
                    bool isOperator = Security.IsLogged(false) && Security.Account.IsInRole(Role.OPERATOR);
                    if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(p.Code, p, quantity, false, this, isOperator))
                        return;
                    bool updateResult = false;
                    EuronaCartHelper.UpdateCartProduct(this.Page, cartProduct.CartId, cartProduct.ProductId, quantity, out updateResult);

                    //Prepocitanie kosiku a objednavky
                    this.RecalculateOrder();
                    UpdateDopravneUIbyOrder();

                    this.lcBodyByEurosap.Text = OrderEntity.CartEntity.BodyEurosapTotal.ToString("F1");
                    this.lcKatalogovaCenaCelkemByEurosap.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.KatalogovaCenaCelkemByEurosap, OrderEntity.CurrencySymbol);

                    this.lcDopravne.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.DopravneEurosap, OrderEntity.CurrencySymbol);
                    this.lblFakturovanaCena.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(this.OrderEntity.PriceWVAT, this.OrderEntity.CurrencySymbol);

                }

                GridViewDataBind(this.OrderEntity, false);
                Response.Redirect(this.Request.RawUrl);
            };

            SlevyDetailTemplate slevyTemplate = new SlevyDetailTemplate(this.CssClass, this.OrderEntity.Id);
            grid.Columns.Add(new TemplateField {
                ItemTemplate = slevyTemplate,
                HeaderText = "",
            });
            grid.Columns.Add(new HyperLinkField {
                DataTextField = "ProductCode",
                HeaderText = "Kód",
                SortExpression = "ProductCode",
            });
            grid.Columns.Add(new HyperLinkField {
                DataTextField = "",
                HeaderText = "",
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
                HeaderText = SHP.Resources.Controls.CartControl_ColumnPriceTotalWithVAT,
                SortExpression = "KatalogPriceWVATTotal",
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

        private bool checkOrderBSRProduct() {
            List<CartProductEntity> cartProducts = this.order.CartEntity.CartProducts;
            foreach (CartProductEntity cartProduct in cartProducts) {
                if (cartProduct.BSRProdukt) {
                    this.hasBSRProduct = true;
                    break;
                }
            }
            return this.hasBSRProduct;
        }
        /// <summary>
        /// Metoda prepocita objednavku + kosik danej objednavky
        /// </summary>
        private bool RecalculateOrder() {
            //Update cart from DB
            this.OrderEntity.CartEntity = null;
            this.checkOrderBSRProduct();

            //Nastavenie postovneho podla celkovej ceny ...
            decimal sumaBezPostovneho = Common.DAL.Entities.OrderSettings.GetFreePostageSuma(this.OrderEntity.CartEntity.Locale);
            if (this.OrderEntity.CartEntity.KatalogovaCenaCelkemByEurosap >= sumaBezPostovneho) {
                order.NoPostage = true;
                this.OrderEntity.CartEntity.DopravneEurosap = 0m;
                this.OrderEntity.CartEntity = Storage<CartEntity>.Update(this.OrderEntity.CartEntity);

                this.cbNoPostage.Checked = true;
                this.OrderEntity.NoPostage = this.cbNoPostage.Checked;
            } else {
                this.cbNoPostage.Checked = false;
                this.OrderEntity.NoPostage = this.cbNoPostage.Checked;
            }

            //Update cart from DB
            this.OrderEntity.CartEntity = null;

            //Recalculate Cart
            EuronaCartHelper.RecalculateCart(this.Page, this.OrderEntity.CartId);

            //Vykonanie prepoctu v TVD
            bool bSuccess = false;
            int? currencyId = order.CurrencyId;
#if !__DEBUG_VERSION_WITHOUTTVD
            CartOrderHelper.RecalculateTVDCart(this.Page, this.updatePanel, order.OrderNumber, order.CartEntity, out currencyId, out bSuccess);
#else
            bSuccess = true;
#endif

            //Nastavenie dopravneho
            CartOrderHelper.RecalculateDopravne(this.OrderEntity.CartEntity, this.OrderEntity.ShipmentCode);

            //Update Order
            decimal? shipmentPrice = order.NoPostage ? 0m : (this.OrderEntity.CartEntity.Shipment != null ? this.OrderEntity.CartEntity.Shipment.Price : 0m);
            decimal? shipmentPriceWVAT = this.OrderEntity.CartEntity.DopravneEurosap;

            if (currencyId.HasValue) {
                order.CurrencyId = currencyId.Value;
            }
            order.Price = this.OrderEntity.CartEntity.PriceTotal.Value + (shipmentPrice.HasValue ? shipmentPrice.Value : 0m);
            order.PriceWVAT = this.OrderEntity.CartEntity.PriceTotalWVAT.Value + (shipmentPriceWVAT.HasValue ? shipmentPriceWVAT.Value : 0m);
            order = Storage<OrderEntity>.Update(order);

            order.CartEntity = null;
            this.order = null;
            return bSuccess;
        }

        void grid_RowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.Footer) {
            } else if (e.Row.RowType == DataControlRowType.DataRow) {
                #region Slevy
                Image collpaseExpand = (Image)e.Row.FindControl("CollpaseExpand");
                collpaseExpand.ImageUrl = Page.ResolveUrl("~/images/plus.png");
                collpaseExpand.Attributes.Add("onclick", "ExpandCollapseGridViewDetail(this," + e.Row.RowIndex + ", 7)");

                GridView gridViewSlevy = (GridView)e.Row.FindControl("gridViewSlevy");
                if (gridViewSlevy.Rows.Count == 0)
                    collpaseExpand.Visible = false;
                #endregion

                int columnNameIndex = 1;
                HyperLink hl = (e.Row.Cells[columnNameIndex].Controls[0] as HyperLink);
                CartProductEntity cp = (e.Row.DataItem as CartProductEntity);
                if (cp.CerpatBonusoveKredity == true) {
                    HyperLink hlBK = (e.Row.Cells[2].Controls[0] as HyperLink);
                    hlBK.NavigateUrl = Page.ResolveUrl("~/user/advisor/bonusoveKredity.aspx");
                    hlBK.ToolTip = "Čerpání bonusových kreditů";
                    Image img = new Image();
                    img.ID = "imgCerpatBK";
                    img.ImageUrl = Page.ResolveUrl("~/images/gift.png");
                    hlBK.Controls.Add(img);
                }
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
            UpdateDopravneUIbyOrder();

            this.lcBodyByEurosap.Text = OrderEntity.CartEntity.BodyEurosapTotal.ToString("F1");
            this.lcKatalogovaCenaCelkemByEurosap.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.KatalogovaCenaCelkemByEurosap, OrderEntity.CurrencySymbol);

            this.lcDopravne.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.DopravneEurosap, OrderEntity.CurrencySymbol);
            this.lblFakturovanaCena.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(this.OrderEntity.PriceWVAT, this.OrderEntity.CurrencySymbol);

            GridViewDataBind(this.OrderEntity, true);

            OnSaveInternalToNextProcessing();
            Response.Redirect(this.Request.RawUrl);
        }

        /// <summary>
        /// //Ulozi  Zavozove misto a dalsie veci ktore je potrebne ukladat priebezne
        /// </summary>
        public bool OnSaveInternalToNextProcessing() {
            this.OrderEntity.ZavozoveMisto_DatumACas = null;
            this.OrderEntity.ZavozoveMisto_Mesto = null;
            this.OrderEntity.ZavozoveMisto_OsobniOdberVSidleSpolecnosti = false;
            this.OrderEntity.ZavozoveMisto_Psc = null;
            //Osobni Odber
            if (this.ddlZavozoveMisto_Mesto != null && this.ddlZavozoveMisto_Mesto.SelectedValue != null && Convert.ToInt32(this.ddlZavozoveMisto_Mesto.SelectedValue) == 1) {
                int zavozoveMistoKod = Convert.ToInt32(this.ddlZavozoveMisto_Mesto.SelectedValue);
                ZavozoveMistoEntity zavozoveMisto = Storage<ZavozoveMistoEntity>.ReadFirst(new ZavozoveMistoEntity.ReadJenAktualiByKod { Kod = zavozoveMistoKod });
                if (zavozoveMisto != null) {
                    Eurona.Common.DAL.Entities.ZavozoveMistoLimit limit = new Eurona.Common.DAL.Entities.ZavozoveMistoLimit(zavozoveMisto.OsobniOdberPovoleneCasy);
                    this.OrderEntity.ZavozoveMisto_DatumACas = null;
                    this.OrderEntity.ZavozoveMisto_Mesto = zavozoveMisto.OsobniOdberAdresaSidlaSpolecnosti;
                    this.OrderEntity.ZavozoveMisto_OsobniOdberVSidleSpolecnosti = zavozoveMisto.OsobniOdberVSidleSpolecnosti;
                    this.OrderEntity.ZavozoveMisto_Psc = zavozoveMisto.Psc;
                    try {
                        DateTime datumOsobnihoOdberu = Convert.ToDateTime(this.dtpZavozoveMistoOsobniOdberDatum.Value);
                        DateTime cas = ZavozoveMistoEntity.GetTimeFromString(this.txtZavozoveMistoOsobniOdberCas.Text);
                        datumOsobnihoOdberu = datumOsobnihoOdberu.AddHours(cas.Hour);
                        datumOsobnihoOdberu = datumOsobnihoOdberu.AddMinutes(cas.Minute);
                        if (limit.IsInLimit(datumOsobnihoOdberu)) {
                            this.OrderEntity.ZavozoveMisto_DatumACas = datumOsobnihoOdberu;
                        } else {
                            string js = string.Format("blockUIAlert('', '{0}');", "Datum a čas osobního odběru není v povoleném období!");
                            ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateZvozoveMisto", js, true);
                            return false;
                        }
                    } catch {
                    }
                }
            } else if (this.ddlZavozoveMisto_Mesto != null && this.ddlZavozoveMisto_Mesto.SelectedItem != null && !String.IsNullOrEmpty(this.ddlZavozoveMisto_DatumACas.SelectedValue)) {
                int zavozoveMistoId = Convert.ToInt32(this.ddlZavozoveMisto_DatumACas.SelectedValue);
                ZavozoveMistoEntity zavozoveMisto = Storage<ZavozoveMistoEntity>.ReadFirst(new ZavozoveMistoEntity.ReadById { Id = zavozoveMistoId });
                if (zavozoveMisto != null) {
                    this.OrderEntity.ZavozoveMisto_Mesto = zavozoveMisto.Mesto;
                    this.OrderEntity.ZavozoveMisto_Psc = zavozoveMisto.Psc;
                    this.OrderEntity.ZavozoveMisto_DatumACas = zavozoveMisto.DatumACas.Value;
                    this.OrderEntity.ZavozoveMisto_OsobniOdberVSidleSpolecnosti = zavozoveMisto.OsobniOdberVSidleSpolecnosti;
                }
            }

            Storage<OrderEntity>.Update(this.OrderEntity);
            return true;
        }


        void OnSave(object sender, EventArgs e) {
            if (this.OrderEntity.DeliveryAddress == null) return;
            this.addressDeliveryControl.UpdateAddress(this.OrderEntity.DeliveryAddress);
            if (this.dtpShipmentFrom != null) this.OrderEntity.ShipmentFrom = this.dtpShipmentFrom.Value != null ? Convert.ToDateTime(this.dtpShipmentFrom.Value) : (DateTime?)null;
            if (this.dtpShipmentTo != null) this.OrderEntity.ShipmentTo = this.dtpShipmentTo.Value != null ? Convert.ToDateTime(this.dtpShipmentTo.Value) : (DateTime?)null;

            //Ulozi  Zavozove misto a dalsie veci ktore je potrebne ukladat priebezne
            if (!OnSaveInternalToNextProcessing()) {
                return;
            }

            this.OrderEntity.ShipmentCode = GetShipmentSelection();
            //Validate Shipment
            if (String.IsNullOrEmpty(this.OrderEntity.ShipmentCode)) {
                string js = string.Format("blockUIAlert('', '{0}');", "Je třeba zvolit dopravce");
                ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateShipment", js, true);
                return;
            }

            //Validate STATE
            string message = Eurona.Common.PSCHelper.ValidateState(this.OrderEntity.DeliveryAddress.State);
            if (message != string.Empty) {
                string js = string.Format("blockUIAlert('', '{0}');", message);
                ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateState", js, true);
                return;
            }

            //Validate PSČ                  
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = this.OrderEntity.AccountId });
            message = Eurona.Common.PSCHelper.ValidatePSCByPSC(this.OrderEntity.DeliveryAddress.Zip, this.OrderEntity.DeliveryAddress.City, this.OrderEntity.DeliveryAddress.State);
            if (message != string.Empty) {
                string js = string.Format("blockUIAlert('', '{0}');" +
                @"document.getElementById('" + this.addressDeliveryControl.GetCityClientID() + @"').value ='';" +
                @"document.getElementById('" + this.addressDeliveryControl.GetCityClientID() + @"').style.backgroundColor='#EFB6E6';" +
                @"document.getElementById('" + this.addressDeliveryControl.GetZipClientID() + @"').style.backgroundColor='#EFB6E6';" +
                @"document.getElementById('" + this.addressDeliveryControl.GetZipClientID() + @"').value = '';", message);
                ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidatePSC", js, true);
                return;
            }

            //Validate Zavozove misto
            if (this.hasBSRProduct == true) {
                //Validate Shipment
                if (String.IsNullOrEmpty(this.OrderEntity.ZavozoveMisto_Mesto)) {
                    string js = string.Format("blockUIAlert('', '{0}');", "Je třeba vyplnit závozové místo!");
                    ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateZvozoveMisto", js, true);
                    return;
                }
                if (this.OrderEntity.ZavozoveMisto_DatumACas == null) {
                    string js = string.Format("blockUIAlert('', '{0}');", "Je třeba vyplnit datum a čas závozového místa!");
                    ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateZvozoveMisto", js, true);
                    return;
                }
            }
            //Validate Obaly na ribi produkty
            if (this.pozadujObal && this.hasObalProdukt == false) {
                string js = string.Format("blockUIAlert('', '{0}');", "Pro dokončení Vaší objednávky je nutné zvolit odpovídající obal!");
                ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateObal", js, true);
                return;
            }


            this.OrderEntity.NoPostage = this.cbNoPostage.Checked;
            this.OrderEntity.ShipmentCode = GetShipmentSelection();
            Storage<OrderEntity>.Update(this.OrderEntity);

            //Prepocitanie kosiku a objednavky
            if (!this.RecalculateOrder()) {
                return;
            }

            Response.Redirect(this.ReturnUrl);
        }

        void OnCancel(object sender, EventArgs e) {
            Response.Redirect(this.ReturnUrl);
        }

        void OnOrder(object sender, EventArgs e) {

            this.btnOrder.Enabled = true;
            string locale = this.OrderEntity.CartEntity.Locale;

            if (this.dtpShipmentFrom != null) this.OrderEntity.ShipmentFrom = this.dtpShipmentFrom.Value != null ? Convert.ToDateTime(this.dtpShipmentFrom.Value) : (DateTime?)null;
            if (this.dtpShipmentTo != null) this.OrderEntity.ShipmentTo = this.dtpShipmentTo.Value != null ? Convert.ToDateTime(this.dtpShipmentTo.Value) : (DateTime?)null;
            this.addressDeliveryControl.UpdateAddress(this.OrderEntity.DeliveryAddress);
            this.OrderEntity.NoPostage = this.cbNoPostage.Checked;
            this.OrderEntity.AssociationAccountId = null;
            this.OrderEntity.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.None;
            this.OrderEntity.ParentId = null;
            this.OrderEntity.OrderDate = DateTime.Now;//Added 20.04.2017

            //Ulozi  Zavozove misto a dalsie veci ktore je potrebne ukladat priebezne
            if (!OnSaveInternalToNextProcessing()) {
                return;
            }

            //if (this.ddlShipment != null) this.OrderEntity.ShipmentCode = this.ddlShipment.SelectedValue;
            this.OrderEntity.ShipmentCode = GetShipmentSelection();
            this.OrderEntity.OrderStatusCode = ((int)OrderEntity.OrderStatus.InProccess).ToString();

            //Validate Shipment
            if (String.IsNullOrEmpty(this.OrderEntity.ShipmentCode)) {
                string js = string.Format("blockUIAlert('', '{0}');", "Je třeba zvolit dopravce");
                ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateShipment", js, true);
                return;
            }

            //Validate STATE
            string message = Eurona.Common.PSCHelper.ValidateState(this.OrderEntity.DeliveryAddress.State);
            if (message != string.Empty) {
                string js = string.Format("blockUIAlert('', '{0}');", message);
                ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateState", js, true);
                return;
            }

            //Validate PSČ
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = this.OrderEntity.AccountId });
            message = Eurona.Common.PSCHelper.ValidatePSCByPSC(this.OrderEntity.DeliveryAddress.Zip, this.OrderEntity.DeliveryAddress.City, this.OrderEntity.DeliveryAddress.State);
            if (message != string.Empty) {
                string js = string.Format("blockUIAlert('', '{0}');" +
                @"document.getElementById('" + this.addressDeliveryControl.GetCityClientID() + @"').value ='';" +
                @"document.getElementById('" + this.addressDeliveryControl.GetCityClientID() + @"').style.backgroundColor='#EFB6E6';" +
                @"document.getElementById('" + this.addressDeliveryControl.GetZipClientID() + @"').style.backgroundColor='#EFB6E6';" +
                @"document.getElementById('" + this.addressDeliveryControl.GetZipClientID() + @"').value = '';", message);
                ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateOrganization", js, true);
                return;
            }

            //Validate Zavozove misto
            if (this.hasBSRProduct == true) {
                if (String.IsNullOrEmpty(this.OrderEntity.ZavozoveMisto_Mesto)) {
                    string js = string.Format("blockUIAlert('', '{0}');", "Je třeba vyplnit závozové místo!");
                    ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateZvozoveMisto", js, true);
                    return;
                }
                if (this.OrderEntity.ZavozoveMisto_DatumACas == null) {
                    string js = string.Format("blockUIAlert('', '{0}');", "Je třeba vyplnit datum a čas závozového místa!");
                    ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateZvozoveMisto", js, true);
                    return;
                }
            }

            //Validate Obaly na ribi produkty
            if (this.pozadujObal && this.hasObalProdukt == false) {
                string js = string.Format("blockUIAlert('', '{0}');", "Pro dokončení Vaší objednávky je nutné zvolit odpovídající obal!");
                ScriptManager.RegisterStartupScript(this.updatePanel, this.updatePanel.GetType(), "addValidateObal", js, true);
                return;
            }

            //Prepocitanie objednavky v TVD databazi
            if (CartOrderHelper.RecalculateTVDOrder(this.Page, this.updatePanel, this.OrderEntity, true)) {
                //Bonusove kredity sa pripocitavaju iba ak operaciu vykonal poradca
                if (Security.Account.IsInRole(Role.ADVISOR)) {
                    //Zaevidovanie bonusovych kreditov
                    BonusovyKreditUzivateleHelper.ZaevidujKredit(Security.Account.Id, DAL.Entities.Classifiers.BonusovyKreditTyp.OdeslanaObjednavka, this.OrderEntity.ProductsPriceWVAT, "", locale);
                    //Zaevidovanie bonusovych kreditov nadriadenemu
                    OrganizationEntity advisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = Security.Account.Id });
                    if (advisor.ParentId.HasValue) {
                        OrganizationEntity parentAdvisor = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByTVDId { TVD_Id = advisor.ParentId.Value });
                        if (parentAdvisor.AccountId.HasValue) {
                            //Iba za prvu objednavku
                            List<OrderEntity> userOrders = Storage<OrderEntity>.Read(new OrderEntity.ReadByAccount { AccountId = Security.Account.Id });
                            if (userOrders.Count == 1)
                                BonusovyKreditUzivateleHelper.ZaevidujKredit(parentAdvisor.AccountId.Value, DAL.Entities.Classifiers.BonusovyKreditTyp.RegistracePodrizenehoPrvniObjednavka, this.OrderEntity.ProductsPriceWVAT, "", locale);
                        }
                    }
                }
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
                    BonusovyKreditUzivateleHelper.ZaevidujKredit(order.AccountId, DAL.Entities.Classifiers.BonusovyKreditTyp.OdeslanaObjednavka, order.ProductsPriceWVAT, "", locale);
                }

                //Ulozenie adresy na nasledovne predpisanie v objednavke
                if (this.cbSaveDeliveryAddressAsMainAddress.Checked) {
                    LastOrderAddressEntity lastOrderAddress = Storage<LastOrderAddressEntity>.ReadFirst(new LastOrderAddressEntity.ReadByAccountId { AccountId = order.AccountId });
                    if (lastOrderAddress == null) {
                        lastOrderAddress = new LastOrderAddressEntity();
                        lastOrderAddress.AccountId = order.AccountId;
                    }
                    lastOrderAddress.FirstName = order.DeliveryAddress.FirstName;
                    lastOrderAddress.LastName = order.DeliveryAddress.LastName;
                    lastOrderAddress.City = order.DeliveryAddress.City;
                    lastOrderAddress.Email = order.DeliveryAddress.Email;
                    lastOrderAddress.State = order.DeliveryAddress.State;
                    lastOrderAddress.Street = order.DeliveryAddress.Street;
                    lastOrderAddress.Zip = order.DeliveryAddress.Zip;
                    lastOrderAddress.Phone = order.DeliveryAddress.Phone;
                    lastOrderAddress.Organization = order.DeliveryAddress.Organization;
                    if (lastOrderAddress.Id == 0) Storage<LastOrderAddressEntity>.Create(lastOrderAddress);
                    else Storage<LastOrderAddressEntity>.Update(lastOrderAddress);

                }
            } else return;

            if (string.IsNullOrEmpty(this.FinishUrlFormat))
                return;

            Response.Redirect(Page.ResolveUrl(string.Format(this.FinishUrlFormat, this.OrderEntity.Id)));
        }

        internal sealed class SlevyDetailTemplate : ITemplate {
            private GridView gridView = null;
            private string cssClass = String.Empty;
            private int idePrepoctu = 0;
            public SlevyDetailTemplate(string cssClass, int idPrepoctu) {
                this.cssClass = cssClass;
                this.idePrepoctu = idPrepoctu;
            }

            void ITemplate.InstantiateIn(Control container) {
                gridView = CreateGridView();
                gridView.DataBinding += new EventHandler(detail_DataBinding);

                Image img = new Image();
                img.ID = "CollpaseExpand";
                container.Controls.Add(img);
                HtmlContainerControl div = new HtmlGenericControl("div");
                div.Attributes.Add("style", "display:none;");

                HtmlContainerControl div1 = new HtmlGenericControl("div");
                div1.Attributes.Add("style", "border: #EFEFEF 1px solid; background-color:#FFFAFA;padding:5px;");
                div1.Controls.Add(new LiteralControl("<span style='color:#ea008a;'>Rozpad slev</span>"));
                div1.Controls.Add(gridView);
                div.Controls.Add(div1);
                container.Controls.Add(div);
            }

            private GridView CreateGridView() {
                GridView grid = new GridView();

                grid.ID = "gridViewSlevy";
                grid.EnableViewState = true;
                grid.GridLines = GridLines.None;
                grid.CssClass = this.cssClass;
                grid.ShowHeader = false;
                grid.ShowFooter = false;

                grid.AllowPaging = false;
                grid.AutoGenerateColumns = false;

                grid.Columns.Add(new BoundField {
                    DataField = "Universal_nazev",
                    HeaderText = "Název",
                    SortExpression = "Universal_nazev",
                });

                grid.Columns.Add(new PriceField {
                    DataField = "Cena_mj_fakt_sdph",
                    HeaderText = "Sleva",
                    SortExpression = "Cena_mj_fakt_sdph",
                });

                return grid;
            }

            private void GridViewDataBind(bool bind, Object dataItem, GridViewRow row) {
#if __DEBUG_VERSION_WITHOUTTVD
                return;
#endif
                Eurona.Common.DAL.Entities.CartProduct cp = (Eurona.Common.DAL.Entities.CartProduct)dataItem;
                DataTable list = CartOrderHelper.GetRecalcResultSlevy(this.idePrepoctu, cp.ProductId);
                gridView.DataSource = list;
            }

            void detail_DataBinding(object sender, EventArgs e) {
                GridView control = sender as GridView;
                GridViewRow row = (GridViewRow)control.NamingContainer;
                Object dataItem = (Object)row.DataItem;
                GridViewDataBind(false, dataItem, row);
            }
        }
    }
}

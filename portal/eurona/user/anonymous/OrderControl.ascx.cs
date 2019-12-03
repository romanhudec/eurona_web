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
using ZavozoveMistoEntity = Eurona.Common.DAL.Entities.ZavozoveMisto;
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

        private List<RadioButton> shipmentRadioButtons = null;
        private List<ZavozoveMistoEntity> zavozoveMistaList;
        private List<ZavozoveMistoEntity> zavozoveMistaDatumyList;
        private bool hasBSRProduct = false;
        private bool pozadujObal = false;
        private bool hasObalProdukt = false;
        private DropDownList ddlZavozoveMisto_Mesto = null;
        private TableCell cellZavozoveMisto_DatumACas = null;
        private DropDownList ddlZavozoveMisto_DatumACas = null;
        private Table tableZavozoveMistoOsobniOdber;
        private ASPxDatePicker dtpZavozoveMistoOsobniOdberDatum;
        private TextBox txtZavozoveMistoOsobniOdberCas;
        private Eurona.user.advisor.ObalyControl obalyControl;

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
            //tableUserInfo.Rows.Add(CreateTableRow(SHP.Resources.Controls.OrderControl_Shipment, new LiteralControl(this.OrderEntity.ShipmentName), false));
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
            lblFakturovanaCena = new Label();
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
                RadioButton rbShipment = new RadioButton();
                rbShipment.GroupName = "rbShipment";
                rbShipment.ID = rbShipment.GroupName + "_" + shipment.Code;
                rbShipment.Text = shipment.Name;
                rbShipment.AutoPostBack = false;
                rbShipment.Enabled = false;
                shipmentRadioButtons.Add(rbShipment);
            }
            foreach (RadioButton rbShipment in shipmentRadioButtons) {
                TableCell shipmentCell = new TableCell();
                shipmentCell.CssClass = "form_label"; shipmentCell.HorizontalAlign = HorizontalAlign.Right;
                shipmentCell.Style.Add("padding-right", "20px");
                shipmentCell.Controls.Add(rbShipment);
                shipmentRow.Cells.Add(shipmentCell);

                //rbShipment.CheckedChanged += rbShipment_CheckedChanged;
            }
            rpDopravne.Controls.Add(CreateTableRow(tableShipment));
            #endregion

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
                    zavozoveMistaList = Storage<ZavozoveMistoEntity>.Read(new ZavozoveMistoEntity.ReadOnlyMestoDistinct());
                    ZavozoveMistoEntity emptyZavozoveMisto = new ZavozoveMistoEntity();
                    if (this.OrderEntity.ZavozoveMisto_OsobniOdberVSidleSpolecnosti == false && !String.IsNullOrEmpty(this.OrderEntity.ZavozoveMisto_Mesto)) {
                        if (!checkIfBindedZavozoveMistoMestoIsInDatasource(zavozoveMistaList, this.OrderEntity.ZavozoveMisto_Mesto)) {
                            ZavozoveMistoEntity newZavozoveMisto = new ZavozoveMistoEntity();

                            newZavozoveMisto.Mesto = this.OrderEntity.ZavozoveMisto_Mesto;
                            newZavozoveMisto.DatumACas = this.OrderEntity.ZavozoveMisto_DatumACas.Value;
                            zavozoveMistaList.Add(newZavozoveMisto);
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
                    this.ddlZavozoveMisto_DatumACas.AutoPostBack = true;
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
                    Eurona.Common.DAL.Entities.ZavozoveMistoLimit zavozoveMistoLimit = new Eurona.Common.DAL.Entities.ZavozoveMistoLimit(zavozoveMisto.OsobniOdberPovoleneCasy);
                    string limitsDisplayString = zavozoveMistoLimit.ToDisplayString();
                    TableRow zavozoveMistoOsobniOdberLimitRow = new TableRow();
                    tableZavozoveMistoOsobniOdber.Rows.Add(zavozoveMistoOsobniOdberLimitRow);
                    cell = new TableCell();
                    cell.ColumnSpan = 4;
                    zavozoveMistoOsobniOdberLimitRow.Cells.Add(cell);
                    cell.Controls.Add(new LiteralControl("<div style='margin-left:10px;margin-top:10px;color:#0077b6;text-align=center;'>" + limitsDisplayString + "</div>"));

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
                LiteralControl lcObalyTitle = new LiteralControl("<span class='order-warning-table-label' style='width:600px;'><b>ZVOLTE OBALY PRO RYBÍ CHLAZENÉ PRODUKTY</b></span>");
                cell.Controls.Add(lcObalyTitle);
                rpObaly.Controls.Add(tblObalyTitle);

                if (this.IsEditing) {
                    this.obalyControl = (Eurona.user.advisor.ObalyControl)this.Page.LoadControl("~/user/advisor/ObalyControl.ascx");
                    this.obalyControl.ID = "obalyControl";
                    rpObaly.Controls.Add(this.obalyControl);
                    this.obalyControl.OnAddObalProduct += obalyControl_OnAddObalProduct;
                } else {
                }
            }
            #endregion

            #region Addresses
            bool isOperator = Security.IsLogged(false) && Security.Account.IsInRole(Role.OPERATOR);
            bool isAdmin = Security.IsLogged(false) && Security.Account.IsInRole(Role.ADMINISTRATOR);
            this.addressDeliveryControl = new Eurona.Controls.AddressControl();
            this.addressDeliveryControl.Width = Unit.Percentage(100);
            this.addressDeliveryControl.IsEditing = this.IsEditing;
            this.addressDeliveryControl.AddressId = this.OrderEntity.DeliveryAddressId;
            this.addressDeliveryControl.EnableState(isOperator || isAdmin);

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

            #region Binding
            if (!IsPostBack) {
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

            //Binding
            GridViewDataBind(this.OrderEntity, !IsPostBack);

        }

        #endregion

        private void SetShipmentSelection(string shipmentCode) {
            foreach (RadioButton rbShipment in shipmentRadioButtons) {
                string code = rbShipment.ID.Replace(rbShipment.GroupName + "_", "");
                if (code == shipmentCode) rbShipment.Checked = true;
                else rbShipment.Checked = false;
            }
        }

        #region Zavozove Misto Methods
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
        #endregion

        #region Obaly Methods
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
            //Ulozi rozrobenu objednavku
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
            //this.lcDopravne.Text = SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.DopravneEurosap, this.Session);
            //TODO:20171205
            this.lcDopravne.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.DopravneEurosap, OrderEntity.CurrencySymbol);
            if (this.lblFakturovanaCena != null) {
                this.lblFakturovanaCena.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(this.OrderEntity.PriceWVAT, this.OrderEntity.CurrencySymbol);
            }
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

                    bool updateResult = false;
                    EuronaCartHelper.UpdateCartProduct(this.Page, cartProduct.CartId, cartProduct.ProductId, quantity, out updateResult);
                    //Prepocitanie kosiku a objednavky
                    this.RecalculateOrder();
                    UpdateDopravneUIbyOrder();

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
            UpdateDopravneUIbyOrder();

            this.lcBodyByEurosap.Text = OrderEntity.CartEntity.BodyEurosapTotal.ToString("F1");
            this.lcKatalogovaCenaCelkemByEurosap.Text = SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.KatalogovaCenaCelkemByEurosap, this.Session);

            this.lcDopravne.Text = SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(OrderEntity.CartEntity.DopravneEurosap, this.Session);
            this.lblFakturovanaCena.Text = Eurona.Common.Utilities.CultureUtilities.CurrencyInfo.ToString(this.OrderEntity.PriceWVAT, this.OrderEntity.CurrencySymbol);
            GridViewDataBind(this.OrderEntity, true);

            //Ulozi rozrobenu objednavku
            OnSaveInternalToNextProcessing();
            Response.Redirect(this.Request.RawUrl);
        }

        /// <summary>
        /// Metoda prepocita objednavku + kosik danej objednavky
        /// </summary>
        public void RecalculateOrder() {
            //Update cart from DB
            this.OrderEntity.CartEntity = null;

            //Nastavenie postovneho podla celkovej ceny ...
            decimal sumaBezPostovneho = Common.DAL.Entities.OrderSettings.GetFreePostageSuma(this.OrderEntity.CartEntity.Locale);
            if (this.OrderEntity.CartEntity.KatalogovaCenaCelkemByEurosap >= sumaBezPostovneho) {
                order.NoPostage = true;
                this.OrderEntity.CartEntity.DopravneEurosap = 0m;
                this.OrderEntity.CartEntity = Storage<CartEntity>.Update(this.OrderEntity.CartEntity);

                this.OrderEntity.NoPostage = true;
            } else {
                this.OrderEntity.NoPostage = false;
            }

            //Update cart from DB
            this.OrderEntity.CartEntity = null;

            //Recalculate Cart
            EuronaCartHelper.RecalculateCart(this.Page, this.OrderEntity.CartId);

            //Vykonanie prepoctu v TVD
            bool bSuccess = false;
            int? currencyId = order.CurrencyId;
#if !__DEBUG_VERSION_WITHOUTTVD
            CartOrderHelper.RecalculateTVDCart(this.Page, null, order.OrderNumber, order.CartEntity, out currencyId, out bSuccess);
#endif

            //Nastavenie dopravneho
            CartOrderHelper.RecalculateDopravne(this.OrderEntity.CartEntity, this.OrderEntity.ShipmentCode);

            if (currencyId.HasValue) {
                order.CurrencyId = currencyId.Value;
            }

            //Update Order
            decimal? shipmentPrice = order.NoPostage ? 0m : (this.OrderEntity.CartEntity.Shipment != null ? this.OrderEntity.CartEntity.Shipment.Price : 0m);
            decimal? shipmentPriceWVAT = this.OrderEntity.CartEntity.DopravneEurosap;

            order.Price = this.OrderEntity.CartEntity.PriceTotal.Value + (shipmentPrice.HasValue ? shipmentPrice.Value : 0m);
            order.PriceWVAT = this.OrderEntity.CartEntity.PriceTotalWVAT.Value + (shipmentPriceWVAT.HasValue ? shipmentPriceWVAT.Value : 0m);
            order = Storage<OrderEntity>.Update(order);
            this.order = null;
        }

        /// <summary>
        /// //Ulozi  Zavozove misto a dalsie veci ktore je potrebne ukladat priebezne
        /// </summary>
        public void OnSaveInternalToNextProcessing() {
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
                            string js = string.Format("alert('{0}');", "Datum a čas osobního odběru není v povoleném období!");
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "addValidateZvozoveMisto", js, true);
                            return;
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
        }

        public void OnOrder() {
            //Base kontroly
            if (!Security.IsLogged(true)) return;
            if (this.OrderEntity == null) return;

            if (this.addressDeliveryControl != null && this.OrderEntity.DeliveryAddress != null) {
                this.addressDeliveryControl.UpdateAddress(this.OrderEntity.DeliveryAddress);
            }

            this.OrderEntity.AssociationAccountId = null;
            this.OrderEntity.AssociationRequestStatus = (int)OrderEntity.AssociationStatus.None;
            this.OrderEntity.ParentId = null;
            this.OrderEntity.OrderStatusCode = ((int)OrderEntity.OrderStatus.InProccess).ToString();

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
                            string js = string.Format("alert('{0}');", "Datum a čas osobního odběru není v povoleném období!");
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "addValidateZvozoveMisto", js, true);
                            return;
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


            if (this.hasBSRProduct == true) {
                //Validate Shipment
                if (String.IsNullOrEmpty(this.OrderEntity.ZavozoveMisto_Mesto)) {
                    string js = string.Format("alert('{0}');", "Je třeba vyplnit závozové místo!");
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "addValidateZvozoveMisto", js, true);
                    return;
                }
                if (this.OrderEntity.ZavozoveMisto_DatumACas == null) {
                    string js = string.Format("alert('{0}');", "Je třeba vyplnit datum a čas závozového místa!");
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "addValidateZvozoveMisto", js, true);
                    return;
                }
            }

            //Obaly na ribi produkty
            if (this.pozadujObal && this.hasObalProdukt == false) {
                string js = string.Format("alert('{0}');", "Pro dokončení Vaší objednávky je nutné zvolit odpovídající obal!");
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "addValidateObal", js, true);
                return;
            }

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
                            List<OrderEntity> userOrders = Storage<OrderEntity>.Read(new OrderEntity.ReadByAccount { AccountId = Security.Account.Id });
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
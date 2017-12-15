using System;
using System.Collections.Generic;
using CMS.Controls;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using CartEntity = Eurona.Common.DAL.Entities.Cart;
using CartProductEntity = Eurona.Common.DAL.Entities.CartProduct;
using OrderEntity = Eurona.DAL.Entities.Order;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using PaymentEntity = SHP.Entities.Classifiers.Payment;
using AccountEntity = Eurona.DAL.Entities.Account;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using ShpAddressEntity = SHP.Entities.Address;
using System.ComponentModel;
using CMS.Utilities;
using SHP.Controls;
using Eurona.Common.Controls.Cart;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Eurona.Common.DAL.Entities;
using System.Text;
using Eurona.Common;

namespace Eurona.Controls {
    public class CartControl : CmsControl {
        private const string DELETE_COMMAND = "DELETE_ITEM";

        public delegate bool OnProccessOrderBeforeSaveHandler(out string result);
        public delegate bool OnProccessOrderAfterSaveHandler(int orderId, out string invoiceUrl, out string result);

        public event OnProccessOrderBeforeSaveHandler OnProccessOrderBeforeSave;
        public event OnProccessOrderAfterSaveHandler OnProccessOrderAfterSave;

        public event EventHandler OnCartItemsChanged;

        private GridViewEx dataGrid = null;
        private LiteralControl lblIntensaMarzeInfo = null;
        private Button btnRecalculate = null;
        private Button btnCreateOrder = null;
        private Button btnBackToCart = null;

        public CartControl() {
        }

        private CartEntity cartEntity = null;
        public CartEntity CartEntity {
            get {
                if (this.cartEntity != null) return this.cartEntity;
                this.cartEntity = EuronaCartHelper.GetAccountCart(this.Page);
                return this.cartEntity;
            }
        }

        /// <summary>
        /// Aktualny krok precesu nakupu, Poslednym krokom je vytvorenie Objednavky
        /// </summary>
        public int Step { get; set; }
        public string FinishUrlFormat { get; set; }

        #region Protected overrides
        protected override void CreateChildControls() {
            base.CreateChildControls();

            if (!Security.IsLogged(false)) {
                this.dataGrid = CreateGridControl();
                this.Controls.Add(this.dataGrid);

                //Binding
                GridViewDataBind(!IsPostBack);

                Button btnLogin = new Button();
                btnLogin.Text = SHP.Resources.Controls.CartControl_LoginButton_Text;
                btnLogin.Click += (s, e) => { Security.IsLogged(true); };
                this.Controls.Add(btnLogin);

                return;
            }


            if (string.IsNullOrEmpty(Request["step"])) this.Step = 1;
            else this.Step = Convert.ToInt32(Request["step"]);

            if (this.CartEntity == null) return;

            //Buttons Spat, Pokracovat
            btnRecalculate = new Button();
            btnRecalculate.Text = Resources.EShopStrings.CartControl_RecalculateCartButton_Text;

            btnCreateOrder = new Button();
            btnCreateOrder.Text = Resources.EShopStrings.CartControl_CreateOrderButton_Text;

            btnBackToCart = new Button();
            btnBackToCart.CausesValidation = false;
            btnBackToCart.Text = SHP.Resources.Controls.BackLink;
            btnBackToCart.Click += (s, e) => {
                if (string.IsNullOrEmpty(this.ReturnUrl))
                    return;
                Response.Redirect(Page.ResolveUrl(this.ReturnUrl));
            };

            //UI podla aktualneho kroku
            this.dataGrid = CreateGridControl();
            this.Controls.Add(this.dataGrid);

            //btnRecalculate.Style.Add( "float", "left" );
            HtmlGenericControl divButtons = new HtmlGenericControl("div");
            divButtons.Style.Add("margin-top", "10px");
            divButtons.Style.Add("width", "100%");

            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Style.Add("margin-top", "10px");
            div.Style.Add("padding-left", "10px");
            div.Style.Add("padding-right", "10px");
            div.Style.Add("width", "90%");

            if (Security.IsInRole(Role.ADVISOR)) {
                /*
				if (this.CartEntity.HasCernyForLifeProducts)
				{
					this.lblIntensaMarzeInfo = new LiteralControl();
					Eurona.Common.EuronaUserMarzeInfo umi = new Common.EuronaUserMarzeInfo(this.Page, Security.Account.TVD_Id.Value);
					string errorMessage;
					umi.GetMarzeInfoFromEurosap(this.CartEntity.Id, out errorMessage);
					UpdateUntensaMarzeInfo(umi);
					divButtons.Controls.Add(this.lblIntensaMarzeInfo);
				}
                 * */

                AccountEntity cartAccount = null;
                int accountOrdersCount = 0;
                cartAccount = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = this.CartEntity.AccountId.Value });

                //Objednat moze iba overeny pouzivatel alebo neovreny ale ten moze vytvorit len jednu objednvku.
                List<OrderEntity> list = Storage<OrderEntity>.Read(new OrderEntity.ReadByAccount { AccountId = cartAccount.Id });
                accountOrdersCount = list.Count;
                if (cartAccount.Verified || (!cartAccount.Verified && accountOrdersCount == 0)) {
                    Organization org = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = Security.Account.Id });

                    //Check na uzavierku
                    if (Security.Account.IsInRole(Role.ADVISOR) && Eurona.Common.Application.EuronaUzavierka.IsUzavierka4Advisor()) {
                        divButtons.Controls.Add(new LiteralControl(string.Format("<span style='color:red;font-weight:bold;'>Probíhá uzávěrka. Vytvářet objednávky bude možné : {0}</span>", Eurona.Common.Application.EuronaUzavierka.GeUzavierka4AdvisorTo())));
                    } else {
                        divButtons.Controls.Add(btnRecalculate);
                        divButtons.Controls.Add(btnCreateOrder);
                    }
                } else
                    divButtons.Controls.Add(new LiteralControl(string.Format("<b style='color:red;'>{0}</b>", Resources.EShopStrings.CartControl_NotVeriedUserCanOnly1OrderCreate)));
            }

            HtmlGenericControl divUp = new HtmlGenericControl("div");
            divUp.Attributes.Add("id", "divUp");
            divUp.Attributes.Add("class", "cart-update-progress");
            divUp.Style.Add("display", "none");
            Image progressImage = new Image();
            progressImage.ImageUrl = this.Page.ResolveUrl("~/images/ajax-indicator.gif");
            divUp.Controls.Add(progressImage);

            this.Controls.Add(divButtons);
            this.Controls.Add(divUp);
            this.Controls.Add(div);

            //Binding
            GridViewDataBind(!IsPostBack);
            #region js on click prepocitavam
            StringBuilder sb = new StringBuilder();
            sb.Append("document.getElementById('divUp').style.display='block';");
            sb.Append("this.value = 'Přepočítavam ...';");
            sb.Append("this.disabled = true;");
            sb.Append(Page.ClientScript.GetPostBackEventReference(btnRecalculate, null) + ";");

            string submit_button_onclick_js = sb.ToString();
            btnRecalculate.Attributes.Add("onclick", submit_button_onclick_js);
            #endregion

            #region js on click vytvaram objednavku
            sb = new StringBuilder();
            sb.Append("document.getElementById('divUp').style.display='block';");
            sb.AppendFormat("this.value = '{0}';", Resources.EShopStrings.CartControl_CreateOrderButton_Text);
            sb.Append("this.disabled = true;");
            sb.Append(Page.ClientScript.GetPostBackEventReference(btnCreateOrder, null) + ";");

            submit_button_onclick_js = sb.ToString();
            btnCreateOrder.Attributes.Add("onclick", submit_button_onclick_js);
            #endregion

            btnRecalculate.Click += (s, e) => {
                //Prepocitanie kosika a nastavenie spravnej marze na kosiku.
                EuronaCartHelper.RecalculateCart(this.Page, this.CartEntity.Id);
                this.cartEntity = null;

                //Vykonanie prepoctu v TVD
                bool bSuccess = false;
#if !__DEBUG_VERSION_WITHOUTTVD
                string message = CartOrderHelper.RecalculateTVDCart(this.Page, /*this.updatePanel*/null, this.CartEntity, out bSuccess);
#else
                string message = "Přepočteno!";
#endif
                if (!bSuccess) return;
                this.CartEntity.Notes = message;
                Storage<CartEntity>.Update(this.CartEntity);

                //div.Style.Add("color", "#000!important");
                div.Controls.Add(new LiteralControl(message));
                GridViewDataBind(true);
            };

            //Nasledujuci krok je 2
            btnCreateOrder.Click += (s, e) => {
                CreateOrder(Security.Account.Id, true);
            };

        }

        /// <summary>
        /// Metoda nastavi spravnu informaciu o aktualnej marzi/zlave na intensa vyrobky
        /// </summary>
        /// <param name="umi"></param>
        private void UpdateUntensaMarzeInfo(Eurona.Common.EuronaUserMarzeInfo umi) {
            if (this.lblIntensaMarzeInfo == null) return;
            if (umi == null) {
                this.lblIntensaMarzeInfo.Visible = false;
                return;
            }

            if (umi.MarzeEurona > 0)//Uplatnuje sa mnozstevna zlava CL
                this.lblIntensaMarzeInfo.Text = string.Format("<div style='color:red;'>Na CernyForLife výrobky se Vám uplatňuje marže ve výši : <span style='color:red;font-weight:bold;'>{0}%</span></div><br/>", umi.MarzeIntensa.ToString());
        }

        /// <summary>
        /// Vytvorenie objednávky pre daného pouzivatela
        /// </summary>
        /// <param name="orderAccount"></param>
        public void CreateOrder(int orderAccountId, bool redirect) {
            //Prepocitanie kosika a nastavenie spravnej marze na kosiku.
            EuronaCartHelper.RecalculateCart(this.Page, this.CartEntity.Id);

            //Prepocet pre spravneho pouzivatela
            EuronaCartHelper.UpdateIntensaProductInCart(this.Page, this.CartEntity.Id, orderAccountId);

            //Nanovo potiahnutie kosika
            this.cartEntity = Storage<Cart>.ReadFirst(new Cart.ReadById { CartId = this.CartEntity.Id });
            //Nastavenie spravneho majitela
            this.cartEntity.AccountId = orderAccountId;

            //Vykonanie prepoctu v TVD
            //Ak sa z eurosapu vrati chyba -> objednavku nemozno vytvorit.
            bool bSuccess = false;
#if !__DEBUG_VERSION_WITHOUTTVD
            string message = CartOrderHelper.RecalculateTVDCart(this.Page, /*this.updatePanel*/null, this.CartEntity, out bSuccess);
#else
            bSuccess = true;
#endif

            if (!bSuccess) return;

            OrderEntity order = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadByCart { CartId = this.CartEntity.Id });
            if (order != null) {
                if (string.IsNullOrEmpty(this.FinishUrlFormat))
                    return;

                if (!this.cartEntity.Closed.HasValue) {
                    //Close Cart
                    this.cartEntity.Closed = DateTime.Now;
                    this.cartEntity.AccountId = orderAccountId;
                    this.cartEntity.SessionId = null;
                    this.cartEntity = Storage<CartEntity>.Update(this.cartEntity);
                }

                Response.Redirect(Page.ResolveUrl(string.Format(this.FinishUrlFormat, order.Id)));
                return;
            }
            try {
                #region Before Proccesing Event
                if (OnProccessOrderBeforeSave != null) {
                    string result = string.Empty;
                    if (!OnProccessOrderBeforeSave(out result)) {
                        this.Controls.Add(new LiteralControl(result));
                        return;
                    }
                }
                #endregion

                if (String.IsNullOrEmpty(this.cartEntity.ShipmentCode))
                    this.cartEntity.ShipmentCode = "2"/*DPD*/;

                //Nastavenie dopravneho
                CartOrderHelper.RecalculateDopravne(this.cartEntity, this.cartEntity.ShipmentCode);

                //Create Order
                order = new OrderEntity();
                order.OrderDate = Eurona.Common.Application.CurrentOrderDate;
                order.CartId = this.cartEntity.Id;
                order.AccountId = orderAccountId;
                order.ShipmentCode = this.cartEntity.ShipmentCode;
                order.PaymentCode = this.cartEntity.PaymentCode;
                order.DeliveryAddressId = this.cartEntity.DeliveryAddressId;
                order.InvoiceAddressId = this.cartEntity.InvoiceAddressId;
                order.Price = this.cartEntity.PriceTotal.Value;
                order.PriceWVAT = this.cartEntity.PriceTotalWVAT.Value + this.cartEntity.DopravneEurosap.Value;
                order.CreatedByAccountId = Security.Account.Id;
                decimal sumaBezPostovneho = Common.DAL.Entities.OrderSettings.GetFreePostageSuma(Security.Account.Locale);
                if (this.cartEntity.KatalogovaCenaCelkemByEurosap >= sumaBezPostovneho) {
                    order.NoPostage = true;
                    this.cartEntity.DopravneEurosap = 0m;
                    this.cartEntity = Storage<CartEntity>.Update(this.cartEntity);
                }
                order = Storage<OrderEntity>.Create(order);

                Organization org = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = order.AccountId });
                order.DeliveryAddress.City = org.CorrespondenceAddress.City;
                order.DeliveryAddress.Email = org.ContactEmail;
                order.DeliveryAddress.State = org.CorrespondenceAddress.State;
                order.DeliveryAddress.Street = org.CorrespondenceAddress.Street;
                order.DeliveryAddress.Zip = org.CorrespondenceAddress.Zip;
                order.DeliveryAddress.Phone = string.IsNullOrEmpty(org.ContactPhone) ? org.ContactMobile : org.ContactPhone;
                order.DeliveryAddress.Organization = org.Name;

                string[] name = org.Name.Split(' ');
                if (name.Length >= 2) {
                    order.DeliveryAddress.FirstName = name[1];
                    order.DeliveryAddress.LastName = name[0];
                }

                bool isOperator = Security.IsLogged(false) && Security.Account.IsInRole(Role.OPERATOR);
                if (!isOperator) {
                    OrderEntity lastOrder = Storage<OrderEntity>.ReadFirst(new OrderEntity.ReadLastByAccount { AccountId = order.AccountId, GreaterAtOrderNumber = order.OrderNumber });
                    if (lastOrder != null) {
                        if (lastOrder.DeliveryAddress.City != org.CorrespondenceAddress.City || lastOrder.DeliveryAddress.Zip != org.CorrespondenceAddress.Zip) {
                            string result1 = PSCHelper.ValidatePSCByPSC(lastOrder.DeliveryAddress.Zip, lastOrder.DeliveryAddress.City, lastOrder.DeliveryAddress.State);
                            if (result1 == string.Empty) {
                                order.DeliveryAddress.City = lastOrder.DeliveryAddress.City;
                                order.DeliveryAddress.Email = org.ContactEmail;
                                order.DeliveryAddress.State = lastOrder.DeliveryAddress.State;
                                order.DeliveryAddress.Street = lastOrder.DeliveryAddress.Street;
                                order.DeliveryAddress.Zip = lastOrder.DeliveryAddress.Zip;
                                order.DeliveryAddress.Phone = string.IsNullOrEmpty(org.ContactPhone) ? org.ContactMobile : org.ContactPhone;
                                order.DeliveryAddress.Organization = org.Name;

                            }
                        }
                    }
                }
                Storage<SHP.Entities.Address>.Update(order.DeliveryAddress);

                //Naviazanie objednavok na pridruzenie
                int associationCount = 0;
                List<OrderEntity> ordersToAssociate = Storage<OrderEntity>.Read(new OrderEntity.ReadByFilter { AssociationAccountId = order.AccountId, AssociationRequestStatus = (int)OrderEntity.AssociationStatus.Accepted });
                foreach (OrderEntity orderToAssociate in ordersToAssociate) {
                    if (orderToAssociate.ParentId.HasValue) continue;
                    orderToAssociate.ParentId = order.Id;
                    Storage<OrderEntity>.Update(orderToAssociate);
                    associationCount++;
                }

                //Close Cart
                this.cartEntity.Closed = DateTime.Now;
                this.cartEntity.AccountId = orderAccountId;
                this.cartEntity.SessionId = null;
                this.cartEntity = Storage<CartEntity>.Update(this.cartEntity);

                #region After Proccesing Event
                if (OnProccessOrderAfterSave != null) {
                    string result = string.Empty;
                    string invoiceUrl = string.Empty;
                    if (!OnProccessOrderAfterSave(order.Id, out invoiceUrl, out result)) {
                        //Unodo close cart !!
                        Storage<OrderEntity>.Delete(order);
                        this.cartEntity.Closed = null;
                        Storage<CartEntity>.Update(this.cartEntity);

                        this.Controls.Add(new LiteralControl(result));
                        return;
                    } else if (!string.IsNullOrEmpty(invoiceUrl)) {
                        order.InvoiceUrl = invoiceUrl;
                        order = Storage<OrderEntity>.Update(order);
                    }
                }
                #endregion

                if (string.IsNullOrEmpty(this.FinishUrlFormat))
                    return;

                if (associationCount != 0) {
                    string js = string.Format("alert('K Vaši objednávce jsou přidruženy {0} objednávky jiného poradce!');window.location.href='{1}';", associationCount, Page.ResolveUrl(string.Format(this.FinishUrlFormat, order.Id)));
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "alert", js, true);
                } else {
                    if (redirect) Response.Redirect(Page.ResolveUrl(string.Format(this.FinishUrlFormat, order.Id)));
                }
            } finally { }
        }
        #endregion
        private void GridViewDataBind(bool bind) {
            List<CartProductEntity> list = new List<CartProductEntity>();
            if (this.CartEntity != null)
                list = Storage<CartProductEntity>.Read(new CartProductEntity.ReadByCart { CartId = this.CartEntity.Id });

            this.dataGrid.PagerTemplate = null;
            dataGrid.DataSource = list;
            if (bind) {
                dataGrid.DataKeyNames = new string[] { "Id" };
                dataGrid.DataBind();
            }

            bool bEnabled = false;
            if (list.Count == 0) bEnabled = false;
            else bEnabled = true;

            if (this.btnCreateOrder != null) this.btnCreateOrder.Enabled = bEnabled;
            if (this.btnRecalculate != null) this.btnRecalculate.Enabled = bEnabled;
        }
        private GridViewEx CreateGridControl() {
            GridViewEx grid = new GridViewEx();
            grid.ShowWhenEmpty = true;
            grid.EnableViewState = true;
            grid.GridLines = GridLines.None;
            grid.RowDataBound += new GridViewRowEventHandler(grid_RowDataBound);

            grid.CssClass = CssClass;
            grid.RowStyle.CssClass = CssClass + "_rowStyle";
            grid.FooterStyle.CssClass = CssClass + "_footerStyle";
            grid.PagerStyle.CssClass = CssClass + "_pagerStyle";
            grid.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
            grid.HeaderStyle.CssClass = CssClass + "_headerStyle";
            grid.EditRowStyle.CssClass = CssClass + "_editRowStyle";
            grid.AlternatingRowStyle.CssClass = CssClass + "_alternatingRowStyle";
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
                        GridViewDataBind(true);
                        return;
                    }
                    ProductEntity p = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = cartProduct.ProductId });
                    bool isOperator = Security.IsLogged(false) && Security.Account.IsInRole(Role.OPERATOR);
                    if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(p.Code, p, quantity, false, this, isOperator))
                        return;
                    Eurona.Common.EuronaUserMarzeInfo umi = EuronaCartHelper.UpdateCartProduct(this.Page, cartProduct.CartId, cartProduct.ProductId, quantity);
                    this.cartEntity = null;
                    UpdateUntensaMarzeInfo(umi);
                    if (OnCartItemsChanged != null) OnCartItemsChanged(this, null);
                }

                GridViewDataBind(true);
            };

            SlevyDetailTemplate slevyTemplate = new SlevyDetailTemplate(this.CssClass, this.CartEntity.Id);
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
            grid.Columns.Add(new TemplateField {
                ItemTemplate = template,
                HeaderText = SHP.Resources.Controls.CartControl_ColumnQuantity,
                SortExpression = "Quantity",
            });
            grid.Columns.Add(new BoundField {
                DataField = "BodyCelkem",
                HeaderText = "Body",
                SortExpression = "BodyCelkem",
            });
            grid.Columns.Add(new PriceField {
                DataField = "KatalogPriceWVATTotal",
                HeaderText = "Katalogová cena celkem",
                SortExpression = "KatalogPriceWVATTotal",
            });
            grid.Columns.Add(new PriceField {
                DataField = "PriceTotalWVAT",
                HeaderText = SHP.Resources.Controls.CartControl_ColumnPriceTotalWithVAT,
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
            if (e.Row.RowType == DataControlRowType.DataRow) {
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
                    img.ImageUrl = Page.ResolveUrl("~/images/gift.png");
                    hlBK.Controls.Add(img);
                }

                if (!string.IsNullOrEmpty(cp.Alias))
                    hl.NavigateUrl = Page.ResolveUrl(cp.Alias + "?&" + base.BuildReturnUrlQueryParam());

                return;
            }

            if (e.Row.RowType == DataControlRowType.Footer) {
                //Cena celkom/Cena celkom s DPH
                string price = string.Empty;
                price = string.Format("Body celkem : {0}&nbsp;&nbsp;&nbsp;&nbsp;Cena celkem : {1}", this.CartEntity.BodyKatalogTotal,
                SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(this.CartEntity.PriceTotalWVAT, this.Session));

                e.Row.Cells[0].Text = SHP.Resources.Controls.CartControl_ColumnPriceTotal;
                e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
                e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
                e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
                int lastCellIndex = e.Row.Cells.Count - 1;
                e.Row.Cells[lastCellIndex].Text = price;
                e.Row.Cells[lastCellIndex].ColumnSpan = 4;
                e.Row.Cells[lastCellIndex].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;

                return;
            }
        }

        void OnRowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == DELETE_COMMAND) OnDeleteCommand(sender, e);
        }
        private void OnDeleteCommand(object sender, GridViewCommandEventArgs e) {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            int cartProductId = Convert.ToInt32((sender as GridView).DataKeys[rowIndex].Value);

            EuronaCartHelper.RemoveProductFromCart(cartProductId);

            //Recalculate Cart
            Eurona.Common.EuronaUserMarzeInfo umi = EuronaCartHelper.RecalculateCart(this.Page, this.CartEntity.Id);
            this.cartEntity = null;
            UpdateUntensaMarzeInfo(umi);

            GridViewDataBind(true);
            if (OnCartItemsChanged != null) OnCartItemsChanged(this, null);
        }

        protected class GridViewEx : GridView {
            protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding) {
                int numRows = base.CreateChildControls(dataSource, dataBinding);

                //no data rows created, create empty table if enabled
                if (numRows == 0 && ShowWhenEmpty) {
                    //create table
                    Table table = new Table();
                    table.ID = this.ID;

                    //convert the exisiting columns into an array and initialize
                    DataControlField[] fields = new DataControlField[this.Columns.Count];
                    this.Columns.CopyTo(fields, 0);

                    if (this.ShowHeader) {
                        //create a new header row
                        GridViewRow headerRow = base.CreateRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal);

                        this.InitializeRow(headerRow, fields);
                        table.Rows.Add(headerRow);
                    }

                    //create the empty row
                    GridViewRow emptyRow = new GridViewRow(-1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);

                    TableCell cell = new TableCell();
                    cell.ColumnSpan = this.Columns.Count;
                    cell.Width = Unit.Percentage(100);
                    if (!String.IsNullOrEmpty(EmptyDataText))
                        cell.Controls.Add(new LiteralControl(EmptyDataText));

                    if (this.EmptyDataTemplate != null)
                        EmptyDataTemplate.InstantiateIn(cell);

                    emptyRow.Cells.Add(cell);
                    table.Rows.Add(emptyRow);

                    if (this.ShowFooter) {
                        //create footer row
                        GridViewRow footerRow = base.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal);

                        this.InitializeRow(footerRow, fields);
                        table.Rows.Add(footerRow);
                    }

                    this.Controls.Clear();
                    this.Controls.Add(table);
                }

                return numRows;
            }

            [Bindable(BindableSupport.No)]
            public bool ShowWhenEmpty {
                get {
                    if (ViewState["ShowWhenEmpty"] == null)
                        ViewState["ShowWhenEmpty"] = false;

                    return (bool)ViewState["ShowWhenEmpty"];
                }
                set { ViewState["ShowWhenEmpty"] = value; }
            }
        }
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
            CartProduct cp = (CartProduct)dataItem;
            DataTable list = CartOrderHelper.GetRecalcResultSlevy(-1 * this.idePrepoctu, cp.ProductId);
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

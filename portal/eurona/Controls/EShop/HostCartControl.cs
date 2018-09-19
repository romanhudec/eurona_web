using System;
using System.Collections.Generic;
using CMS.Controls;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using CartEntity = Eurona.Common.DAL.Entities.Cart;
using CartProductEntity = Eurona.Common.DAL.Entities.CartProduct;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using OrderEntity = Eurona.DAL.Entities.Order;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using PaymentEntity = SHP.Entities.Classifiers.Payment;
using AccountEntity = Eurona.DAL.Entities.Account;
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
using Eurona.User.Host;

namespace Eurona.Controls {
    public class HostCartControl : CmsControl {
        private const string DELETE_COMMAND = "DELETE_ITEM";

        public delegate bool OnProccessOrderBeforeSaveHandler(out string result);
        public delegate bool OnProccessOrderAfterSaveHandler(int orderId, out string invoiceUrl, out string result);

        public event EventHandler OnCartItemsChanged;

        private GridViewEx dataGrid = null;

        public HostCartControl() {
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

            if (!HostSecurity.IsAutenticated(this.Page))
                return;

            this.dataGrid = CreateGridControl();
            this.Controls.Add(this.dataGrid);

            //Binding
            GridViewDataBind(!IsPostBack);

            Button btnRegister = new Button();
            btnRegister.Text = Resources.EShopStrings.CartControl_RegisterButton_Text;
            btnRegister.Click += (s, e) => { Response.Redirect(Page.ResolveUrl(this.FinishUrlFormat)); };
            this.Controls.Add(btnRegister);

            return;


            //if ( string.IsNullOrEmpty( Request["step"] ) ) this.Step = 1;
            //else this.Step = Convert.ToInt32( Request["step"] );

            //if ( this.CartEntity == null ) return;

            ////UI podla aktualneho kroku
            //this.dataGrid = CreateGridControl();
            //this.Controls.Add( this.dataGrid );

            ////btnRecalculate.Style.Add( "float", "left" );
            //HtmlGenericControl divButtons = new HtmlGenericControl( "div" );
            //divButtons.Style.Add( "margin-top", "10px" );
            //divButtons.Style.Add( "width", "100%" );

            //HtmlGenericControl div = new HtmlGenericControl( "div" );
            //div.Style.Add( "margin-top", "10px" );
            //div.Style.Add( "padding-left", "10px" );
            //div.Style.Add( "padding-right", "10px" );
            //div.Style.Add( "width", "90%" );

            //divButtons.Controls.Add( new LiteralControl( string.Format( "<b style='color:red;'>{0}</b>", Resources.EShopStrings.CartControl_NotVeriedUserCanOnly1OrderCreate ) ) );

            //HtmlGenericControl divUp = new HtmlGenericControl( "div" );
            //divUp.Attributes.Add( "id", "divUp" );
            //divUp.Attributes.Add( "class", "cart-update-progress" );
            //divUp.Style.Add( "display", "none" );
            //Image progressImage = new Image();
            //progressImage.ImageUrl = this.Page.ResolveUrl( "~/images/ajax-indicator.gif" );
            //divUp.Controls.Add( progressImage );

            ////this.Controls.Add( this.updatePanel );
            //this.Controls.Add( divButtons );
            //this.Controls.Add( divUp );
            //this.Controls.Add( div );

            ////Binding
            //GridViewDataBind( !IsPostBack );
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
                    ProductEntity p = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = cartProduct.ProductId });
                    if (!EuronaCartHelper.ValidateProductBeforeAddingToChart(p.Code, p, quantity, this))
                        return;
                    bool updateResult = false;
                    EuronaCartHelper.UpdateCartProduct(this.Page, cartProduct.CartId, cartProduct.ProductId, quantity, out updateResult);
                    this.cartEntity = null;
                    if (OnCartItemsChanged != null) OnCartItemsChanged(this, null);
                }

                GridViewDataBind(true);
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
            grid.Columns.Add(new TemplateField {
                ItemTemplate = template,
                HeaderText = SHP.Resources.Controls.CartControl_ColumnQuantity,
                SortExpression = "Quantity",
            });
            grid.Columns.Add(new PriceField {
                DataField = "PriceWithDiscount",
                HeaderText = SHP.Resources.Controls.CartControl_ColumnPrice,
                SortExpression = "PriceWithDiscount",
            });
            //grid.Columns.Add( new PriceField
            //{
            //    DataField = "PriceTotal",
            //    HeaderText = SHP.Resources.Controls.CartControl_ColumnPriceTotal,
            //    SortExpression = "PriceTotal",
            //} );
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
                int columnNameIndex = 0;
                HyperLink hl = (e.Row.Cells[columnNameIndex].Controls[0] as HyperLink);
                CartProductEntity cp = (e.Row.DataItem as CartProductEntity);
                if (!string.IsNullOrEmpty(cp.Alias))
                    hl.NavigateUrl = Page.ResolveUrl(cp.Alias + "?&" + base.BuildReturnUrlQueryParam());
                return;
            }

            if (e.Row.RowType == DataControlRowType.Footer) {

                //Cena celkom/Cena celkom s DPH
                string price = string.Empty;
                if (this.CartEntity.PriceTotalWVAT == 0)
                    price = SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(this.CartEntity.PriceTotal, this.Session);
                else
                    price = SHP.Utilities.CultureUtilities.CurrencyInfo.ToString(this.CartEntity.PriceTotalWVAT, this.Session);

                e.Row.Cells[0].Text = SHP.Resources.Controls.CartControl_ColumnPriceTotal;
                e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
                e.Row.Cells.RemoveAt(e.Row.Cells.Count - 1);
                //e.Row.Cells.RemoveAt( e.Row.Cells.Count - 1 );
                int lastCellIndex = e.Row.Cells.Count - 1;
                e.Row.Cells[lastCellIndex].Text = price;
                e.Row.Cells[lastCellIndex].ColumnSpan = 4;
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
}

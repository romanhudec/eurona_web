using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS;
using CMS.Controls;
using AccountEntity = CMS.Entities.Account;
using CartEntity = SHP.Entities.Cart;
using CartProductEntity = SHP.Entities.CartProduct;
using OrderEntity = SHP.Entities.Order;
using OrderStatusEntity = SHP.Entities.Classifiers.OrderStatus;
using PaymentEntity = SHP.Entities.Classifiers.Payment;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using System.Security.Principal;
using System.Diagnostics;
using SHP.Controls.Cart;

namespace SHP.Controls.Order
{
		public class AdminOrderControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";

				private GridView dataGrid = null;
				private AddressControl addressDeliveryControl = null;
				private AddressControl addressInvoiceControl = null;
				private DropDownList ddlShipment = null;
				private DropDownList ddlPayment = null;
				private DropDownList ddlOrderStatus = null;
				private ASPxDatePicker dtpPaydDate = null;
				private OrderEntity order = null;

				private Label lblProductPrice = null;
				private Label lblShipmentPrice = null;
				private Label lblPriceTotal = null;
				private Label lblPriceTotalWVAT = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				public AdminOrderControl()
				{
						this.IsEditing = true;
				}

				public int OrderId
				{
						get
						{
								object o = ViewState["OrderId"];
								return o != null ? (int)Convert.ToInt32( o ) : 0;
						}
						set { ViewState["OrderId"] = value; }
				}

				public OrderEntity OrderEntity
				{
						get
						{
								if ( this.order != null ) return this.order;
								this.order = Storage<OrderEntity>.ReadFirst( new OrderEntity.ReadById { OrderId = this.OrderId } );
								return this.order;
						}
				}

				public string CssGridView { get; set; }

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						//Base kontroly
						if ( !Security.IsLogged( true ) ) return;
						if ( this.OrderEntity == null ) return;
						if ( !Security.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) &&
						this.OrderEntity.AccountId != Security.Account.Id )
								return;

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass + "_orderNumber" );
						div.Controls.Add( new LiteralControl( string.Format( Resources.Controls.OrderControl_OrderNumberFormatText, this.OrderEntity.OrderNumber ) ) );
						this.Controls.Add( div );

						#region Create controls
						this.dataGrid = CreateGridControl();
						//Polozky objednavky
						RoundPanel rpOrderProducts = new RoundPanel();
						rpOrderProducts.CssClass = "roundPanel";
						rpOrderProducts.Controls.Add( this.dataGrid );
						rpOrderProducts.Text = Resources.Controls.OrderControl_OrderProducts;

						this.ddlShipment = new DropDownList();
						this.ddlShipment.ID = "ddlShipment";
						this.ddlShipment.DataSource = Storage<ShipmentEntity>.Read();
						this.ddlShipment.DataTextField = "Display";
						this.ddlShipment.DataValueField = "Code";
						this.ddlShipment.Enabled = this.IsEditing && ( !this.OrderEntity.Payed || Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) );
						this.ddlShipment.Width = Unit.Pixel( 150 );

						this.ddlPayment = new DropDownList();
						this.ddlPayment.ID = "ddlPayment";
						this.ddlPayment.DataSource = Storage<PaymentEntity>.Read();
						this.ddlPayment.DataTextField = "Name";
						this.ddlPayment.DataValueField = "Code";
						this.ddlPayment.Enabled = this.IsEditing && ( !this.OrderEntity.Payed || Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) );
						this.ddlPayment.Width = Unit.Pixel( 150 );

						this.ddlOrderStatus = new DropDownList();
						this.ddlOrderStatus.ID = "ddlOrderStatus";
						this.ddlOrderStatus.DataSource = Storage<OrderStatusEntity>.Read();
						this.ddlOrderStatus.DataTextField = "Name";
						this.ddlOrderStatus.DataValueField = "Code";
						this.ddlOrderStatus.Enabled = this.IsEditing && Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR );
						this.ddlOrderStatus.Width = Unit.Pixel( 150 );

						this.dtpPaydDate = new ASPxDatePicker();
						this.dtpPaydDate.ID = "dtpPaydDate";
						this.dtpPaydDate.Enabled = this.IsEditing && Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR );

						#region Addresses
						this.addressDeliveryControl = new AddressControl();
						this.addressDeliveryControl.Width = Unit.Percentage( 100 );
						this.addressDeliveryControl.IsEditing = this.IsEditing;
						this.addressDeliveryControl.AddressId = this.OrderEntity.DeliveryAddressId;

						this.addressInvoiceControl = new AddressControl();
						this.addressInvoiceControl.Width = Unit.Percentage( 100 );
						this.addressInvoiceControl.IsEditing = this.IsEditing;
						this.addressInvoiceControl.AddressId = this.OrderEntity.InvoiceAddressId;

						//Delivery Address
						RoundPanel rpDlvAddress = new RoundPanel();
						rpDlvAddress.CssClass = "roundPanel";
						rpDlvAddress.Controls.Add( this.addressDeliveryControl );
						rpDlvAddress.Text = Resources.Controls.OrderControl_DeliveryAddress;

						//Invoice Address
						RoundPanel rpInvAddress = new RoundPanel();
						rpInvAddress.CssClass = "roundPanel";
						rpInvAddress.Controls.Add( this.addressInvoiceControl );
						rpInvAddress.Text = Resources.Controls.OrderControl_InvoiceAddress;
						#endregion

						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = Resources.Controls.SaveButton_Text;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnSave.Visible = this.IsEditing;
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = this.IsEditing ? Resources.Controls.CancelButton_Text : Resources.Controls.BackLink;
						this.btnCancel.Click += new EventHandler( OnCancel );
						#endregion

						#region Create Layout
						//Grid
						this.Controls.Add( rpOrderProducts );

						//Adresa pre dorucenia a fakturacna adresa
						this.Controls.Add( rpDlvAddress );
						this.Controls.Add( rpInvAddress );

						//Main controls
						Table mainTable = new Table();
						TableRow row = new TableRow();
						mainTable.Rows.Add( row );
						TableCell leftCell = new TableCell(); row.Cells.Add( leftCell ); leftCell.VerticalAlign = VerticalAlign.Top;
						TableCell rightCell = new TableCell(); row.Cells.Add( rightCell ); rightCell.VerticalAlign = VerticalAlign.Top;

						Table table = new Table();
						table.Width = Unit.Percentage( 100 );
						row = new TableRow();
						table.Rows.Add( CreateTableRow( Resources.Controls.OrderControl_Shipment, this.ddlShipment, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.OrderControl_Payment, this.ddlPayment, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.OrderControl_OrderStatus, this.ddlOrderStatus, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.OrderControl_PaydDate, this.dtpPaydDate, false ) );
						leftCell.Controls.Add( table );

						#region Celkom k uhrade
						Table tablePt = new Table();
						tablePt.CssClass = this.CssClass + "_priceSumary";
						string cssLabel = tablePt.CssClass + "_label";
						string cssLabelB = tablePt.CssClass + "_labelBold";
						this.lblProductPrice = new Label();this.lblShipmentPrice = new Label();
						this.lblPriceTotal = new Label();this.lblPriceTotalWVAT = new Label();
						//Update UI order Total
						UpdateOrderTotalPriceUI();
						tablePt.Rows.Add( CreateTableRow( Resources.Controls.OrderControl_PriceForProducts, cssLabelB, this.lblProductPrice, cssLabel ) );
						tablePt.Rows.Add( CreateTableRow( Resources.Controls.OrderControl_PriceForShipment, cssLabelB, this.lblShipmentPrice, cssLabel ) );
						tablePt.Rows.Add( CreateTableRow( Resources.Controls.OrderControl_PriceTotal, cssLabelB, this.lblPriceTotal, cssLabel ) );
						tablePt.Rows.Add( CreateTableRow( Resources.Controls.OrderControl_PriceTotalWVAT, cssLabelB, this.lblPriceTotalWVAT, tablePt.CssClass + "_priceTotal" ) );

						RoundPanel rpPriceTotal = new RoundPanel();
						rpPriceTotal.CssClass = "roundPanel";
						rpPriceTotal.Controls.Add( tablePt );
						rpPriceTotal.Text = Resources.Controls.OrderControl_Price;
						rpPriceTotal.Height = Unit.Percentage( 100 );
						rpPriceTotal.Width = Unit.Percentage( 100 );
						rightCell.Controls.Add( rpPriceTotal );
						#endregion

						this.Controls.Add( mainTable );

						//Tlacidla
						this.Controls.Add( btnSave );
						this.Controls.Add( btnCancel );

						#endregion

						#region Binding
						if ( !IsPostBack )
						{
								if ( !string.IsNullOrEmpty( this.OrderEntity.ShipmentCode ) )
										this.ddlShipment.SelectedValue = this.OrderEntity.ShipmentCode;
								this.ddlShipment.DataBind();

								if ( !string.IsNullOrEmpty( this.OrderEntity.PaymentCode ) )
										this.ddlPayment.SelectedValue = this.OrderEntity.PaymentCode;
								this.ddlPayment.DataBind();

								if ( !string.IsNullOrEmpty( this.OrderEntity.OrderStatusCode ) )
										this.ddlOrderStatus.SelectedValue = this.OrderEntity.OrderStatusCode;
								this.ddlOrderStatus.DataBind();

								this.dtpPaydDate.Value = this.OrderEntity.PaydDate;
						}
						#endregion

						//Binding
						GridViewDataBind( this.OrderEntity, !IsPostBack );

				}
				#endregion

				/// <summary>
				/// Update celkovu sumu objednavky
				/// </summary>
				private void UpdateOrderTotalPriceUI()
				{
						this.lblProductPrice.Text = Utilities.CultureUtilities.CurrencyInfo.ToString( this.OrderEntity.ProductsPrice, this.Session );
						this.lblShipmentPrice.Text = Utilities.CultureUtilities.CurrencyInfo.ToString( this.OrderEntity.ShipmentPrice, this.Session );
						this.lblPriceTotal.Text = Utilities.CultureUtilities.CurrencyInfo.ToString( this.OrderEntity.Price, this.Session );
						this.lblPriceTotalWVAT.Text = Utilities.CultureUtilities.CurrencyInfo.ToString( this.OrderEntity.PriceWVAT, this.Session );
				}
				private TableRow CreateTableRow( string labelText, Control control, bool required )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						if ( required ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						row.Cells.Add( cell );

						return row;
				}
				private TableRow CreateTableRow( string labelText, string css1, Control control, string css2 )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.Text = labelText;
						cell.CssClass = css1;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.Controls.Add( control );
						cell.CssClass = css2;
						row.Cells.Add( cell );

						return row;
				}

				private void GridViewDataBind( OrderEntity order, bool bind )
				{
						List<CartProductEntity> list = Storage<CartProductEntity>.Read( new CartProductEntity.ReadByCart { CartId = order.CartId } );

						this.dataGrid.PagerTemplate = null;
						dataGrid.DataSource = list;
						if ( bind )
						{
								dataGrid.DataKeyNames = new string[] { "Id" };
								dataGrid.DataBind();
						}
				}

				private GridView CreateGridControl()
				{
						GridView grid = new GridView();
						grid.EnableViewState = true;
						grid.GridLines = GridLines.None;
						grid.Style.Add( "margin-top", "5px" );
						grid.RowDataBound += new GridViewRowEventHandler( grid_RowDataBound );

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

						CartProductQuantityItemTemplate template = new CartProductQuantityItemTemplate();
						template.OnRefresh += ( id, quantity ) =>
						{
								if ( quantity != 0 )
								{
										CartProductEntity cartProduct = Storage<CartProductEntity>.ReadFirst( new CartProductEntity.ReadById { CartProductId = id } );
										CartHelper.UpdateCartProduct( cartProduct.CartId, cartProduct.ProductId, quantity );

										//Update Order
										CartEntity cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadById { CartId = this.OrderEntity.CartId } );
										decimal? shipmentPrice = cart.Shipment != null ? cart.Shipment.Price : 0m;
										decimal? shipmentPriceWVAT = cart.Shipment != null ? cart.Shipment.PriceWVAT : 0m;

										order.Price = cart.PriceTotal.Value + ( shipmentPrice.HasValue ? shipmentPrice.Value : 0m );
										order.PriceWVAT = cart.PriceTotalWVAT.Value + ( shipmentPriceWVAT.HasValue ? shipmentPriceWVAT.Value : 0m );
										order = Storage<OrderEntity>.Update( order );
										this.order = null;

										//Update UI order total
										UpdateOrderTotalPriceUI();
								}

								GridViewDataBind( this.OrderEntity, true );
						};

						grid.Columns.Add( new HyperLinkField
						{
								DataTextField = "ProductName",
								HeaderText = Resources.Controls.CartControl_ColumnName,
								SortExpression = "ProductName",
						} );

						bool isEditable = this.IsEditing && ( OrderEntity.GetOrderStatusFromCode( this.OrderEntity.OrderStatusCode ) == OrderEntity.OrderStatus.WaitingForProccess &&
								( !this.OrderEntity.Payed || Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) ) );
						if ( isEditable )
						{
								grid.Columns.Add( new TemplateField
								{
										ItemTemplate = template,
										HeaderText = Resources.Controls.CartControl_ColumnQuantity,
										SortExpression = "Quantity",
								} );
						}
						else
						{
								grid.Columns.Add( new BoundField
								{
										DataField = "Quantity",
										HeaderText = Resources.Controls.CartControl_ColumnQuantity,
										SortExpression = "Quantity",
								} );
						}
						grid.Columns.Add( new PriceField
						{
								DataField = "PriceWithDiscount",
								HeaderText = Resources.Controls.CartControl_ColumnPrice,
								SortExpression = "PriceWithDiscount",
						} );
						grid.Columns.Add( new PriceField
						{
								DataField = "Price",
								HeaderText = Resources.Controls.CartControl_ColumnPriceTotal,
								SortExpression = "Price",
						} );
						grid.Columns.Add( new PriceField
						{
								DataField = "PriceWVAT",
								HeaderText = Resources.Controls.CartControl_ColumnPriceTotalWithVAT,
								SortExpression = "PriceWVAT",
						} );
						CMSButtonField btnDelete = new CMSButtonField();
						btnDelete.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImage" );
						btnDelete.ToolTip = Resources.Controls.GridView_ToolTip_DeleteItem;
						btnDelete.ButtonType = ButtonType.Image;
						btnDelete.OnClientClick = string.Format( "javascript:return confirm('{0}')", Resources.Controls.DeleteItemQuestion );
						btnDelete.CommandName = DELETE_COMMAND;
						grid.Columns.Add( btnDelete );

						grid.RowCommand += OnRowCommand;

						return grid;
				}

				void grid_RowDataBound( object sender, GridViewRowEventArgs e )
				{
						if ( e.Row.RowType == DataControlRowType.Footer )
						{
								CartEntity cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadById { CartId = this.OrderEntity.CartId } );

								//Cena celkom/Cena celkom s DPH
								string price = string.Empty;
								if ( cart.PriceTotalWVAT == 0 )
										price = Utilities.CultureUtilities.CurrencyInfo.ToString( cart.PriceTotal, this.Session );
								else
										price = string.Format( "{0}/{1} {2}",
										Utilities.CultureUtilities.CurrencyInfo.ToString( cart.PriceTotal, this.Session ),
										Resources.Controls.WithVAT,
										Utilities.CultureUtilities.CurrencyInfo.ToString( cart.PriceTotalWVAT, this.Session ) );

								e.Row.Cells[0].Text = Resources.Controls.CartControl_ColumnPriceTotal;
								e.Row.Cells.RemoveAt( e.Row.Cells.Count - 1 );
								e.Row.Cells.RemoveAt( e.Row.Cells.Count - 1 );
								e.Row.Cells.RemoveAt( e.Row.Cells.Count - 1 );
								int lastCellIndex = e.Row.Cells.Count - 1;
								e.Row.Cells[lastCellIndex].Text = price;
								e.Row.Cells[lastCellIndex].ColumnSpan = 4;
								e.Row.Font.Bold = true;
						}
						else if ( e.Row.RowType == DataControlRowType.DataRow )
						{
								int columnNameIndex = 0;
								HyperLink hl = ( e.Row.Cells[columnNameIndex].Controls[0] as HyperLink );
								CartProductEntity cp = ( e.Row.DataItem as CartProductEntity );
								hl.NavigateUrl = Page.ResolveUrl( cp.Alias + "?&" + base.BuildReturnUrlQueryParam() );

								if ( this.IsEditing && Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) )
										return;

								bool isEditable = this.IsEditing && ( OrderEntity.GetOrderStatusFromCode( this.OrderEntity.OrderStatusCode ) == OrderEntity.OrderStatus.WaitingForProccess &&
										( !this.OrderEntity.Payed || Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) ) );
								int deleteIndex = e.Row.Cells.Count - 1;
								ImageButton btnDelete = ( e.Row.Cells[deleteIndex].Controls[0] as ImageButton );
								btnDelete.Enabled = isEditable;

								if ( !btnDelete.Enabled )
										btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImageD" );
						}
				}

				void OnRowCommand( object sender, GridViewCommandEventArgs e )
				{
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
				}

				private void OnDeleteCommand( object sender, GridViewCommandEventArgs e )
				{
						int rowIndex = Convert.ToInt32( e.CommandArgument );
						int cartProductId = Convert.ToInt32( ( sender as GridView ).DataKeys[rowIndex].Value );
						CartProductEntity cartProduct = Storage<CartProductEntity>.ReadFirst( new CartProductEntity.ReadById { CartProductId = cartProductId } );
						Storage<CartProductEntity>.Delete( cartProduct );

						//Kontrola kosiku
						CartEntity cartEntity = Storage<CartEntity>.ReadFirst( new CartEntity.ReadById { CartId = cartProduct.CartId } );
						if ( cartEntity != null )
						{
								if ( cartEntity.CartProductsCount == 0 )
								{
										cartEntity.PriceTotal = 0m;
										cartEntity.PriceTotalWVAT = 0m;
										Storage<CartEntity>.Update( cartEntity );
								}
						}
						GridViewDataBind( this.OrderEntity, true );
				}

				private void NotifyCustomer( OrderEntity order )
				{
						OrderStatusEntity ose = Storage<OrderStatusEntity>.ReadFirst( new OrderStatusEntity.ReadByCode { Code = order.OrderStatusCode } );
						AccountEntity account = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadById { AccountId = order.AccountId } );

						EmailNotification email = new EmailNotification();
						email.To = account.Email;
						email.Subject = string.Format( Resources.Controls.OrderControl_OrderNumberFormatText, order.OrderNumber );
						email.Message = string.Format( Resources.Controls.OrderControl_OrderStatusChanged_Email_Message,
										order.OrderNumber, ose.Name, ConfigValue( "SHP:SMTP:FromDisplay" ) );
						email.Notify( true );
				}

				void OnSave( object sender, EventArgs e )
				{
						SHP.Entities.Classifiers.Shipment shipment = Storage<SHP.Entities.Classifiers.Shipment>.ReadFirst( new SHP.Entities.Classifiers.Shipment.ReadByCode { Code = this.ddlShipment.SelectedValue } );
						SHP.Entities.Cart cart = Storage<SHP.Entities.Cart>.ReadFirst( new SHP.Entities.Cart.ReadById { CartId = this.OrderEntity.CartId } );

						string oldOrderStatusCode = this.OrderEntity.OrderStatusCode;

						this.addressDeliveryControl.UpdateAddress( this.OrderEntity.DeliveryAddress );
						this.addressInvoiceControl.UpdateAddress( this.OrderEntity.InvoiceAddress );

						this.OrderEntity.ShipmentCode = this.ddlShipment.SelectedValue;
						this.OrderEntity.PaymentCode = this.ddlPayment.SelectedValue;
						this.OrderEntity.OrderStatusCode = this.ddlOrderStatus.SelectedValue;
						this.OrderEntity.PaydDate = (DateTime?)this.dtpPaydDate.Value;

						//Vypocet ceny s DPH a Bez DPH
						decimal? shipmentPrice = shipment.Price != null ? shipment.Price : 0m;
						decimal? shipmentPriceWVAT = shipment.PriceWVAT != null ? shipment.PriceWVAT : 0m;

						this.OrderEntity.ShipmentPrice = shipmentPrice;
						this.OrderEntity.ShipmentPriceWVAT = shipmentPriceWVAT;

						this.OrderEntity.Price = cart.PriceTotal.Value + ( shipmentPrice.HasValue ? shipmentPrice.Value : 0m );
						this.OrderEntity.PriceWVAT = cart.PriceTotalWVAT.Value + ( shipmentPriceWVAT.HasValue ? shipmentPriceWVAT.Value : 0m );
						Storage<OrderEntity>.Update( this.OrderEntity );

						if ( oldOrderStatusCode != this.OrderEntity.OrderStatusCode )
								NotifyCustomer( this.OrderEntity );

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}

using System;
using System.Collections.Generic;
using CMS.Controls;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using CartEntity = SHP.Entities.Cart;
using CartProductEntity = SHP.Entities.CartProduct;
using OrderEntity = SHP.Entities.Order;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using PaymentEntity = SHP.Entities.Classifiers.Payment;
using PersonEntity = CMS.Entities.Person;
using OrganizationEntity = CMS.Entities.Organization;
using System.ComponentModel;
using CMS.Utilities;

namespace SHP.Controls.Cart
{
		public class CartControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";

				public delegate bool OnProccessOrderBeforeSaveHandler( out string result );
				public delegate bool OnProccessOrderAfterSaveHandler( int orderId, out string invoiceUrl, out string result );

				public event OnProccessOrderBeforeSaveHandler OnProccessOrderBeforeSave;
				public event OnProccessOrderAfterSaveHandler OnProccessOrderAfterSave;

				public event EventHandler OnCartItemsChanged;

				private GridViewEx dataGrid = null;
				private DropDownList ddlShipment = null;
				private DropDownList ddlPayment = null;
				private Button btnNextStep = null;
				private Button btnBackToCart = null;
				private AddressControl addressDeliveryControl = null;
				private AddressControl addressInvoiceControl = null;

				public CartControl()
				{
				}

				private CartEntity cartEntity = null;
				public CartEntity CartEntity
				{
						get
						{
								if ( this.cartEntity != null ) return this.cartEntity;
								this.cartEntity = CartHelper.GetAccountCart( this.Page );
								return this.cartEntity;
						}
				}

				/// <summary>
				/// Aktualny krok precesu nakupu, Poslednym krokom je vytvorenie Objednavky
				/// </summary>
				public int Step { get; set; }
				public string Step2Url { get; set; }
				public string Step3Url { get; set; }
				public string FinishUrlFormat { get; set; }

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						if ( !Security.IsLogged( false ) )
						{
								this.dataGrid = CreateGridControl();
								this.Controls.Add( this.dataGrid );

								//Binding
								GridViewDataBind( !IsPostBack );

								Button btnLogin = new Button();
								btnLogin.Text = Resources.Controls.CartControl_LoginButton_Text;
								btnLogin.Click += ( s, e ) => { Security.IsLogged( true ); };
								this.Controls.Add( btnLogin );

								return;
						}

						if ( string.IsNullOrEmpty( Request["step"] ) ) this.Step = 1;
						else this.Step = Convert.ToInt32( Request["step"] );

						if ( this.CartEntity == null ) return;

						//Buttons Spat, Pokracovat
						btnNextStep = new Button();
						btnNextStep.Text = Resources.Controls.CartControl_NextStepButton_Text;

						btnBackToCart = new Button();
						btnBackToCart.CausesValidation = false;
						btnBackToCart.Text = Resources.Controls.BackLink;
						btnBackToCart.Click += ( s, e ) =>
						{
								if ( string.IsNullOrEmpty( this.ReturnUrl ) )
										return;
								Response.Redirect( Page.ResolveUrl( this.ReturnUrl ) );
						};

						//UI podla aktualneho kroku
						if ( this.Step == 1 )
						{
								this.dataGrid = CreateGridControl();
								this.Controls.Add( this.dataGrid );

								btnNextStep.Style.Add( "margin-top", "30px" );
								btnNextStep.Style.Add( "float", "right" );
								this.Controls.Add( btnNextStep );

								//Binding
								GridViewDataBind( !IsPostBack );

								//Nasledujuci krok je 2
								btnNextStep.Click += ( s, e ) =>
								{
										AliasUtilities aliasUtils = new AliasUtilities();
										string alias = aliasUtils.Resolve( this.Step2Url, this.Page );
										Response.Redirect( alias + ( alias.Contains( "?" ) ? "&" : "?" ) + this.BuildReturnUrlQueryParam() );
								};
						}
						else if ( this.Step == 2 )
						{
								this.ddlShipment = new DropDownList();
								this.ddlShipment.ID = "ddlShipment";
								this.ddlShipment.DataSource = Storage<ShipmentEntity>.Read();
								this.ddlShipment.DataTextField = "Display";
								this.ddlShipment.DataValueField = "Code";
								this.ddlShipment.Width = Unit.Pixel( 200 );

								this.ddlPayment = new DropDownList();
								this.ddlPayment.ID = "ddlPayment";
								this.ddlPayment.DataSource = Storage<PaymentEntity>.Read();
								this.ddlPayment.DataTextField = "Name";
								this.ddlPayment.DataValueField = "Code";
								this.ddlPayment.Width = Unit.Pixel( 200 );

								Table table = new Table();
								table.Width = Unit.Percentage( 100 );
								//table.Attributes.Add( "border", "1" );

								//Shipmemnt
								TableRow row = new TableRow();
								TableCell cell = new TableCell();
								cell.CssClass = "form_label_required";
								cell.Controls.Add( new LiteralControl( Resources.Controls.CartControl_Shipment ) );
								row.Cells.Add( cell );
								cell = new TableCell();
								cell.CssClass = "form_control";
								cell.Controls.Add( this.ddlShipment );
								cell.Controls.Add( CreateRequiredFieldValidatorControl( this.ddlShipment.ID ) );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Payment
								row = new TableRow();
								cell = new TableCell();
								cell.CssClass = "form_label_required";
								cell.Controls.Add( new LiteralControl( Resources.Controls.CartControl_Payment ) );
								row.Cells.Add( cell );
								cell = new TableCell();
								cell.CssClass = "form_control";
								cell.Controls.Add( this.ddlPayment );
								cell.Controls.Add( CreateRequiredFieldValidatorControl( this.ddlPayment.ID ) );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.ColumnSpan = 2;
								cell.Style.Add( "padding-top", "20px" );
								cell.Controls.Add( btnBackToCart );
								cell.Controls.Add( btnNextStep );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								this.Controls.Add( table );

								if ( !IsPostBack )
								{
										if ( !string.IsNullOrEmpty( this.cartEntity.ShipmentCode ) )
												this.ddlShipment.SelectedValue = this.cartEntity.ShipmentCode;
										if ( !string.IsNullOrEmpty( this.cartEntity.PaymentCode ) )
												this.ddlPayment.SelectedValue = this.cartEntity.PaymentCode;
										this.ddlShipment.DataBind();
										this.ddlPayment.DataBind();
								}

								//Nasledujuci krok je 3
								btnNextStep.Click += ( s, e ) =>
								{
										this.cartEntity.ShipmentCode = this.ddlShipment.SelectedValue;
										this.cartEntity.PaymentCode = this.ddlPayment.SelectedValue;
										Storage<CartEntity>.Update( this.cartEntity );

										AliasUtilities aliasUtils = new AliasUtilities();
										string alias = aliasUtils.Resolve( this.Step3Url, this.Page );
										Response.Redirect( alias + ( alias.Contains( "?" ) ? "&" : "?" ) + this.BuildReturnUrlQueryParam() );
								};
						}
						else if ( this.Step == 3 )
						{
								this.addressDeliveryControl = new AddressControl();
								this.addressDeliveryControl.Width = Unit.Percentage( 100 );
								this.addressDeliveryControl.IsEditing = true;
								this.addressDeliveryControl.AddressId = this.cartEntity.DeliveryAddressId;

								this.addressInvoiceControl = new AddressControl();
								this.addressInvoiceControl.Width = Unit.Percentage( 100 );
								this.addressInvoiceControl.IsEditing = true;
								this.addressInvoiceControl.AddressId = this.cartEntity.InvoiceAddressId;

								Table tableAddress = new Table();
								tableAddress.Width = Unit.Percentage( 100 );
								//tableAddress.Attributes.Add( "border", "1" );
								TableRow row = new TableRow();
								TableCell cellAddress = new TableCell(); cellAddress.VerticalAlign = VerticalAlign.Top;
								row.Cells.Add( cellAddress );
								tableAddress.Rows.Add( row );

								row = new TableRow();
								TableCell cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.ColumnSpan = 2;
								cell.Style.Add( "padding-top", "20px" );
								cell.Controls.Add( btnBackToCart );
								cell.Controls.Add( btnNextStep );
								row.Cells.Add( cell );
								tableAddress.Rows.Add( row );

								//Delivery Address
								RoundPanel rpDlvAddress = new RoundPanel();
								rpDlvAddress.CssClass = "roundPanel";
								rpDlvAddress.Controls.Add( this.addressDeliveryControl );
								rpDlvAddress.Text = Resources.Controls.CartControl_DeliveryAddress;
								cellAddress.Controls.Add( rpDlvAddress );

								//Invoice Address
								RoundPanel rpInvAddress = new RoundPanel();
								rpInvAddress.CssClass = "roundPanel";
								rpInvAddress.Controls.Add( this.addressInvoiceControl );
								rpInvAddress.Text = Resources.Controls.CartControl_InvoiceAddress;
								cellAddress.Controls.Add( rpInvAddress );

								//Nasledujuci krok je vytvorenie objednavky
								btnNextStep.Text = Resources.Controls.CartControl_OrderButton_Text;
								btnNextStep.Click += ( s, e ) =>
								{
										OrderEntity order = Storage<OrderEntity>.ReadFirst( new OrderEntity.ReadByCart { CartId = this.CartEntity.Id } );
										if ( order != null )
										{
												if ( string.IsNullOrEmpty( this.FinishUrlFormat ) )
														return;

												Response.Redirect( Page.ResolveUrl( string.Format( this.FinishUrlFormat, order.Id ) ) );
												return;
										}
										try
										{
												//Save addresses
												this.addressDeliveryControl.UpdateAddress( this.cartEntity.DeliveryAddress );
												this.addressInvoiceControl.UpdateAddress( this.cartEntity.InvoiceAddress );

												#region Before Proccesing Event
												if ( OnProccessOrderBeforeSave != null )
												{
														string result = string.Empty;
														if ( !OnProccessOrderBeforeSave( out result ) )
														{
																this.Controls.Add( new LiteralControl( result ) );
																return;
														}
												}
												#endregion

												decimal? shipmentPrice = this.cartEntity.Shipment != null ? this.cartEntity.Shipment.Price : 0m;
												decimal? shipmentPriceWVAT = this.cartEntity.Shipment != null ? this.cartEntity.Shipment.PriceWVAT : 0m;

												//Create Order
												order = new OrderEntity();
												order.CartId = this.cartEntity.Id;
												order.AccountId = Security.Account.Id;
												order.ShipmentCode = this.cartEntity.ShipmentCode;
												order.PaymentCode = this.cartEntity.PaymentCode;
												order.DeliveryAddressId = this.cartEntity.DeliveryAddressId;
												order.InvoiceAddressId = this.cartEntity.InvoiceAddressId;
												order.Price = this.cartEntity.PriceTotal.Value + ( shipmentPrice.HasValue ? shipmentPrice.Value : 0m );
												order.PriceWVAT = this.cartEntity.PriceTotalWVAT.Value + ( shipmentPriceWVAT.HasValue ? shipmentPriceWVAT.Value : 0m );
												order = Storage<OrderEntity>.Create( order );

												//Close Cart
												this.cartEntity.Closed = DateTime.Now;
												this.cartEntity.AccountId = Security.Account.Id;
												this.cartEntity.SessionId = null;
												this.cartEntity = Storage<CartEntity>.Update( this.cartEntity );

												#region After Proccesing Event
												if ( OnProccessOrderAfterSave != null )
												{
														string result = string.Empty;
														string invoiceUrl = string.Empty;
														if ( !OnProccessOrderAfterSave( order.Id, out invoiceUrl, out result ) )
														{
																//Unodo close cart !!
																Storage<OrderEntity>.Delete( order );
																this.cartEntity.Closed = null;
																Storage<CartEntity>.Update( this.cartEntity );

																this.Controls.Add( new LiteralControl( result ) );
																return;
														}
														else if ( !string.IsNullOrEmpty( invoiceUrl ) )
														{
																order.InvoiceUrl = invoiceUrl;
																order = Storage<OrderEntity>.Update( order );
														}
												}
												#endregion

												if ( string.IsNullOrEmpty( this.FinishUrlFormat ) )
														return;

												Response.Redirect( Page.ResolveUrl( string.Format( this.FinishUrlFormat, order.Id ) ) );
										}
										finally { }

								};
								this.Controls.Add( tableAddress );

								#region Binding Addresses
								if ( !IsPostBack )
								{
										if ( string.IsNullOrEmpty( this.cartEntity.DeliveryAddress.LastName ) || string.IsNullOrEmpty( this.cartEntity.InvoiceAddress.LastName ) )
										{
												PersonEntity p = Storage<PersonEntity>.ReadFirst( new PersonEntity.ReadByAccountId { AccountId = Security.Account.Id } );
												OrganizationEntity o = Storage<OrganizationEntity>.ReadFirst( new OrganizationEntity.ReadByAccountId { AccountId = Security.Account.Id } );
												if ( p != null )
												{
														this.cartEntity.DeliveryAddress.FirstName = p.FirstName;
														this.cartEntity.DeliveryAddress.LastName = p.LastName;
														this.cartEntity.DeliveryAddress.Phone = p.Mobile;
														this.cartEntity.DeliveryAddress.Street = p.AddressHome.Street;
														this.cartEntity.DeliveryAddress.City = p.AddressHome.City;
														this.cartEntity.DeliveryAddress.Zip = p.AddressHome.Zip;
														this.cartEntity.DeliveryAddress.Email = p.Email;

														this.cartEntity.InvoiceAddress.FirstName = p.FirstName;
														this.cartEntity.InvoiceAddress.LastName = p.LastName;
														this.cartEntity.InvoiceAddress.Phone = p.Mobile;
														this.cartEntity.InvoiceAddress.Street = p.AddressHome.Street;
														this.cartEntity.InvoiceAddress.City = p.AddressHome.City;
														this.cartEntity.InvoiceAddress.Zip = p.AddressHome.Zip;
														this.cartEntity.InvoiceAddress.Email = p.Email;

														this.addressDeliveryControl.UpdateUIFromEntity( this.cartEntity.DeliveryAddress );
														this.addressInvoiceControl.UpdateUIFromEntity( this.cartEntity.InvoiceAddress );
												}
												else if ( o != null )
												{
														this.cartEntity.DeliveryAddress.FirstName = o.ContactPerson.FirstName;
														this.cartEntity.DeliveryAddress.LastName = o.ContactPerson.LastName;
														this.cartEntity.DeliveryAddress.Phone = o.ContactPerson.Mobile;
														this.cartEntity.DeliveryAddress.Street = o.CorrespondenceAddress.Street;
														this.cartEntity.DeliveryAddress.City = o.CorrespondenceAddress.City;
														this.cartEntity.DeliveryAddress.Zip = o.CorrespondenceAddress.Zip;
														this.cartEntity.DeliveryAddress.Email = o.ContactPerson.Email;

														this.cartEntity.InvoiceAddress.FirstName = o.ContactPerson.FirstName;
														this.cartEntity.InvoiceAddress.LastName = o.ContactPerson.LastName;
														this.cartEntity.InvoiceAddress.Phone = o.ContactPerson.Mobile;
														this.cartEntity.InvoiceAddress.Street = o.CorrespondenceAddress.Street;
														this.cartEntity.InvoiceAddress.City = o.CorrespondenceAddress.City;
														this.cartEntity.InvoiceAddress.Zip = o.CorrespondenceAddress.Zip;
														this.cartEntity.InvoiceAddress.Email = o.ContactPerson.Email;

														this.addressDeliveryControl.UpdateUIFromEntity( this.cartEntity.DeliveryAddress );
														this.addressInvoiceControl.UpdateUIFromEntity( this.cartEntity.InvoiceAddress );
												}
										}
								}
								#endregion

						}
				}
				#endregion
				private void GridViewDataBind( bool bind )
				{
						List<CartProductEntity> list = new List<CartProductEntity>();
						if ( this.CartEntity != null )
								list = Storage<CartProductEntity>.Read( new CartProductEntity.ReadByCart { CartId = this.CartEntity.Id } );

						this.dataGrid.PagerTemplate = null;
						dataGrid.DataSource = list;
						if ( bind )
						{
								dataGrid.DataKeyNames = new string[] { "Id" };
								dataGrid.DataBind();
						}
				}
				private GridViewEx CreateGridControl()
				{
						GridViewEx grid = new GridViewEx();
						grid.ShowWhenEmpty = true;
						grid.EnableViewState = true;
						grid.GridLines = GridLines.None;
						grid.RowDataBound += new GridViewRowEventHandler( grid_RowDataBound );

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

						CartProductQuantityItemTemplate template = new CartProductQuantityItemTemplate();
						template.OnRefresh += ( id, quantity ) =>
						{
								if ( quantity != 0 )
								{
										CartProductEntity cartProduct = Storage<CartProductEntity>.ReadFirst( new CartProductEntity.ReadById { CartProductId = id } );
										CartHelper.UpdateCartProduct( cartProduct.CartId, cartProduct.ProductId, quantity );
										this.cartEntity = null;
										if ( OnCartItemsChanged != null ) OnCartItemsChanged( this, null );										
								}

								GridViewDataBind( true );
						};

						grid.Columns.Add( new HyperLinkField
						{
								DataTextField = "ProductName",
								HeaderText = Resources.Controls.CartControl_ColumnName,
								SortExpression = "ProductName",
						} );
						grid.Columns.Add( new TemplateField
						{
								ItemTemplate = template,
								HeaderText = Resources.Controls.CartControl_ColumnQuantity,
								SortExpression = "Quantity",
						} );
						grid.Columns.Add( new PriceField
						{
								DataField = "PriceWithDiscount",
								HeaderText = Resources.Controls.CartControl_ColumnPrice,
								SortExpression = "PriceWithDiscount",
						} );
						grid.Columns.Add( new PriceField
						{
								DataField = "PriceTotal",
								HeaderText = Resources.Controls.CartControl_ColumnPriceTotal,
								SortExpression = "PriceTotal",
						} );
						grid.Columns.Add( new PriceField
						{
								DataField = "PriceTotalWVAT",
								HeaderText = Resources.Controls.CartControl_ColumnPriceTotalWithVAT,
								SortExpression = "PriceTotalWVAT",
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
						if ( e.Row.RowType == DataControlRowType.DataRow )
						{
								int columnNameIndex = 0;
								HyperLink hl = ( e.Row.Cells[columnNameIndex].Controls[0] as HyperLink );
								CartProductEntity cp = ( e.Row.DataItem as CartProductEntity );
								hl.NavigateUrl = Page.ResolveUrl( cp.Alias + "?&" + base.BuildReturnUrlQueryParam() );
								return;
						}

						if ( e.Row.RowType == DataControlRowType.Footer )
						{

								//Cena celkom/Cena celkom s DPH
								string price = string.Empty;
								if ( this.CartEntity.PriceTotalWVAT == 0 )
										price = Utilities.CultureUtilities.CurrencyInfo.ToString( this.CartEntity.PriceTotal, this.Session );
								else
										price = string.Format( "{0}/{1} {2}",
										Utilities.CultureUtilities.CurrencyInfo.ToString( this.CartEntity.PriceTotal, this.Session ),
										Resources.Controls.WithVAT,
										Utilities.CultureUtilities.CurrencyInfo.ToString( this.CartEntity.PriceTotalWVAT, this.Session ) );

								e.Row.Cells[0].Text = Resources.Controls.CartControl_ColumnPriceTotal;
								e.Row.Cells.RemoveAt( e.Row.Cells.Count - 1 );
								e.Row.Cells.RemoveAt( e.Row.Cells.Count - 1 );
								e.Row.Cells.RemoveAt( e.Row.Cells.Count - 1 );
								int lastCellIndex = e.Row.Cells.Count - 1;
								e.Row.Cells[lastCellIndex].Text = price;
								e.Row.Cells[lastCellIndex].ColumnSpan = 4;
								e.Row.Font.Bold = true;

								return;
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
						if( cartProduct != null) Storage<CartProductEntity>.Delete( cartProduct );
						
						//Kontrola kosiku
						this.cartEntity = null;
						if ( this.CartEntity != null )
						{
								if ( this.CartEntity.CartProductsCount == 0 )
								{
										this.CartEntity.PriceTotal = 0m;
										this.CartEntity.PriceTotalWVAT = 0m;
										Storage<CartEntity>.Update( this.CartEntity );
								}
						}

						GridViewDataBind( true );
						if ( OnCartItemsChanged != null ) OnCartItemsChanged( this, null );
				}

				protected class GridViewEx: GridView
				{
						protected override int CreateChildControls( System.Collections.IEnumerable dataSource, bool dataBinding )
						{
								int numRows = base.CreateChildControls( dataSource, dataBinding );

								//no data rows created, create empty table if enabled
								if ( numRows == 0 && ShowWhenEmpty )
								{
										//create table
										Table table = new Table();
										table.ID = this.ID;

										//convert the exisiting columns into an array and initialize
										DataControlField[] fields = new DataControlField[this.Columns.Count];
										this.Columns.CopyTo( fields, 0 );

										if ( this.ShowHeader )
										{
												//create a new header row
												GridViewRow headerRow = base.CreateRow( -1, -1, DataControlRowType.Header, DataControlRowState.Normal );

												this.InitializeRow( headerRow, fields );
												table.Rows.Add( headerRow );
										}

										//create the empty row
										GridViewRow emptyRow = new GridViewRow( -1, -1, DataControlRowType.EmptyDataRow, DataControlRowState.Normal );

										TableCell cell = new TableCell();
										cell.ColumnSpan = this.Columns.Count;
										cell.Width = Unit.Percentage( 100 );
										if ( !String.IsNullOrEmpty( EmptyDataText ) )
												cell.Controls.Add( new LiteralControl( EmptyDataText ) );

										if ( this.EmptyDataTemplate != null )
												EmptyDataTemplate.InstantiateIn( cell );

										emptyRow.Cells.Add( cell );
										table.Rows.Add( emptyRow );

										if ( this.ShowFooter )
										{
												//create footer row
												GridViewRow footerRow = base.CreateRow( -1, -1, DataControlRowType.Footer, DataControlRowState.Normal );

												this.InitializeRow( footerRow, fields );
												table.Rows.Add( footerRow );
										}

										this.Controls.Clear();
										this.Controls.Add( table );
								}

								return numRows;
						}

						[Bindable( BindableSupport.No )]
						public bool ShowWhenEmpty
						{
								get
								{
										if ( ViewState["ShowWhenEmpty"] == null )
												ViewState["ShowWhenEmpty"] = false;

										return (bool)ViewState["ShowWhenEmpty"];
								}
								set { ViewState["ShowWhenEmpty"] = value; }
						}
				}
		}
}

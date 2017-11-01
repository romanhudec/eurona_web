using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using CMS.Controls;
using OrderEntity = SHP.Entities.Order;
using CMS;
using System.Web.UI;
using SHP.Entities.Classifiers;
using Telerik.Web.UI;

namespace SHP.Controls.Order
{
		public class AdminOrdersControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";

				private TextBox txtOrderNumber = null;
				private DropDownList ddlOrderStatus = null;
				private DropDownList ddlShipment = null;

				private RadGrid gridView;
				public string EditUrlFormat { get; set; }
				public string UserUrlFormat { get; set; }

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminOrdersControl-SortDirection", SortDirection.Descending ); }
						set { SetSession<SortDirection>( "AdminOrdersControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminOrdersControl-SortExpression", "OrderDate" ); }
						set { SetSession<string>( "AdminOrdersControl-SortExpression", value ); }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control filter = CreateFilterControl();
						this.Controls.Add( filter ); ;

						gridView = CreateGridView();
						this.Controls.Add( gridView );

						GridViewDataBind( !IsPostBack );
				}

				private Table CreateFilterControl()
				{
						Table table = new Table();
						table.Width = Unit.Percentage( 100 );
						TableRow row = new TableRow();

						//--Cislo objednavky
						TableCell cell = new TableCell();
						this.txtOrderNumber = new TextBox();
						cell.Controls.Add( new LiteralControl( Resources.Controls.AdminOrdersControl_OrderNumber ) );
						cell.Controls.Add( this.txtOrderNumber );
						row.Cells.Add( cell );

						//--Stav objednavky
						cell = new TableCell();
						this.ddlOrderStatus = new DropDownList();
						cell.Controls.Add( new LiteralControl( Resources.Controls.AdminOrdersControl_OrderStatus ) );
						cell.Controls.Add( this.ddlOrderStatus );
						row.Cells.Add( cell );

						//--Preprava
						cell = new TableCell();
						this.ddlShipment = new DropDownList();
						cell.Controls.Add( new LiteralControl( Resources.Controls.AdminOrdersControl_Shipment ) );
						cell.Controls.Add( this.ddlShipment );
						row.Cells.Add( cell );

						//--Filtruj
						cell = new TableCell();
						Button btnFilter = new Button();
						btnFilter.Text = Resources.Controls.AdminOrdersControl_Find;
						cell.Controls.Add( btnFilter );
						btnFilter.Click += ( s, e ) =>
						{
								GridViewDataBind( true );
						};
						row.Cells.Add( cell );

						table.Rows.Add( row );

						{
								#region Fill drop down list
								List<OrderStatus> statuses = Storage<OrderStatus>.Read();
								OrderStatus status = new OrderStatus();
								statuses.Insert( 0, status );
								status.Name = Resources.Controls.AdminOrdersControl_OptionAll;
								status.Id = 0;
								status.Code = string.Empty;
								ddlOrderStatus.DataSource = statuses;
								ddlOrderStatus.DataTextField = "Name";
								ddlOrderStatus.DataValueField = "Code";

								List<Shipment> shipments = Storage<Shipment>.Read();
								Shipment shipment = new Shipment();
								shipments.Insert( 0, shipment );
								shipment.Name = Resources.Controls.AdminOrdersControl_OptionAll;
								shipment.Id = 0;
								ddlShipment.DataSource = shipments;
								ddlShipment.DataTextField = "Name";
								ddlShipment.DataValueField = "Code";
								#endregion

								ddlOrderStatus.DataBind();
								ddlShipment.DataBind();
						}
						return table;
				}

				private OrderEntity.ReadByFilter GetFilterValue()
				{
						OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
						if ( !string.IsNullOrEmpty( this.txtOrderNumber.Text ) )
								filter.OrderNumber = this.txtOrderNumber.Text;

						if ( !string.IsNullOrEmpty( this.ddlOrderStatus.SelectedValue ) ) filter.OrderStatusCode = this.ddlOrderStatus.SelectedValue;
						if ( !string.IsNullOrEmpty( this.ddlShipment.SelectedValue ) ) filter.ShipmentCode = this.ddlShipment.SelectedValue;

						return filter;
				}

				private void GridViewDataBind( bool bind )
				{
						OrderEntity.ReadByFilter filter = GetFilterValue();
						if ( !Security.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) )
								filter.AccountId = Security.Account.Id;

						List<OrderEntity> list = Storage<OrderEntity>.Read( filter );

						var ordered = list.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
						gridView.DataSource = ordered.ToList();
						if ( bind ) gridView.DataBind();
				}

				private RadGrid CreateGridView()
				{
						RadGrid grid = new RadGrid();
						CMS.Utilities.RadGridUtilities.Localize( grid );
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

						grid.MasterTableView.Columns.Add( new GridBoundColumn
						{
								DataField = "OrderNumber",
								HeaderText = Resources.Controls.AdminOrdersControl_ColumnOrderNumber,
								SortExpression = "OrderNumber",
						} );
						//if ( Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) )
						//{
						//    grid.Columns.Add( new GridHyperLinkColumn
						//    {
						//        DataTextField = "AccountName",
						//        DataNavigateUrlFields = new string[] { "AccountId" },
						//        DataNavigateUrlFormatString = Page.ResolveUrl(this.UserUrlFormat) + "&" + base.BuildReturnUrlQueryParam(),
						//        HeaderText = Resources.Controls.AdminOrdersControl_ColumnUser,
						//        SortExpression = "AccountName",
						//    } );
						//}
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "OrderDate",
								DataFormatString = "{0:d}",
								HeaderText = Resources.Controls.AdminOrdersControl_ColumnOrderDate,
								SortExpression = "OrderDate",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridIconColumn
						{
								ImageWidth = Unit.Pixel( 16 ),
								ImageHeight = Unit.Pixel( 16 ),
								DataImageUrlFields = new string[] { "OrderStatusIcon" },
								DataAlternateTextField = "OrderStatusName",
								HeaderText = Resources.Controls.AdminOrdersControl_ColumnOrderStatus,
								SortExpression = "OrderStatusName",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridIconColumn
						{
								ImageWidth = Unit.Pixel( 16 ),
								ImageHeight = Unit.Pixel( 16 ),
								DataImageUrlFields = new string[] { "ShipmentIcon" },
								DataAlternateTextField = "ShipmentName",
								HeaderText = Resources.Controls.AdminOrdersControl_ColumnShipment,
								SortExpression = "ShipmentName",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridIconColumn
						{
								ImageWidth = Unit.Pixel( 16 ),
								ImageHeight = Unit.Pixel( 16 ),
								DataImageUrlFields = new string[] { "PaymentIcon" },
								DataAlternateTextField = "PaymentName",
								HeaderText = Resources.Controls.AdminOrdersControl_ColumnPayment,
								SortExpression = "PaymentName",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridPriceColumn
						{
                            //CurrencySymbolField = "CurrencySymbol",
								DataField = "Price",
								HeaderText = Resources.Controls.AdminOrdersControl_ColumnPriceTotal,
								SortExpression = "Price",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridPriceColumn
						{
                            //CurrencySymbolField = "CurrencySymbol",
								DataField = "PriceWVAT",
								HeaderText = Resources.Controls.AdminOrdersControl_ColumnPriceTotalWVAT,
								SortExpression = "PriceWVAT",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridCheckBoxColumn
						{
								DataField = "Payed",
								HeaderText = Resources.Controls.AdminOrdersControl_ColumnPayed,
								SortExpression = "Payed",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );

						GridButtonColumn btnEdit = new GridButtonColumn();
						btnEdit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
						btnEdit.Text = Resources.Controls.GridView_ToolTip_EditItem;
						btnEdit.ButtonType = GridButtonColumnType.ImageButton;
						btnEdit.CommandName = EDIT_COMMAND;
						grid.MasterTableView.Columns.Add( btnEdit );

						GridButtonColumn btnDelete = new GridButtonColumn();
						btnDelete.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImage" );
						btnDelete.Text = Resources.Controls.GridView_ToolTip_DeleteItem;
						btnDelete.ButtonType = GridButtonColumnType.ImageButton;
						btnDelete.ConfirmTitle = Resources.Controls.DeleteItemQuestion;
						btnDelete.ConfirmText = Resources.Controls.DeleteItemQuestion;
						btnDelete.CommandName = DELETE_COMMAND;
						grid.MasterTableView.Columns.Add( btnDelete );

						grid.ItemCommand += OnRowCommand;
						grid.ItemDataBound += OnRowDataBound;

						return grid;
				}

				void OnRowDataBound( object sender, GridItemEventArgs e )
				{
						if ( e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem )
								return;

						if ( Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) )
								return;

						OrderEntity order = e.Item.DataItem as OrderEntity;

						bool isEditable = OrderEntity.GetOrderStatusFromCode( order.OrderStatusCode ) == OrderEntity.OrderStatus.WaitingForProccess &&
								( !order.Payed || Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) );

						int deleteIndex = e.Item.Cells.Count - 1;
						int editIndex = e.Item.Cells.Count - 2;

						ImageButton btnDelete = ( e.Item.Cells[deleteIndex].Controls[0] as ImageButton );
						ImageButton btnEdit = ( e.Item.Cells[editIndex].Controls[0] as ImageButton );

						btnDelete.Enabled = isEditable;

						if ( !btnDelete.Enabled )
								btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImageD" );

						if ( !btnEdit.Enabled )
								btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImageD" );
				}

				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int orderId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( EditUrlFormat, orderId ) + "&" + base.BuildReturnUrlQueryParam() );
				}
				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int orderId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						OrderEntity entity = Storage<OrderEntity>.ReadFirst( new OrderEntity.ReadById { OrderId = orderId } );
						Storage<OrderEntity>.Delete( entity );
						GridViewDataBind( true );
				}
		}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ProductEntity = SHP.Entities.Product;
using CMS.Controls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace SHP.Controls.Product
{
		public class AdminProductsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";

				private RadGrid gridView;

				public string EditUrlFormat { get; set; }
				public string NewUrl { get; set; }

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminProductsControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminProductsControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminProductsControl-SortExpression", "Name" ); }
						set { SetSession<string>( "AdminProductsControl-SortExpression", value ); }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						gridView = CreateGridView();
						this.Controls.Add( gridView );
						GridViewDataBind( !IsPostBack );
				}

				private void GridViewDataBind( bool bind )
				{
						List<ProductEntity> products = Storage<ProductEntity>.Read();

						var ordered = products.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
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
						
						grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminProductsControl_NewProductButton_Text;

						grid.MasterTableView.Columns.Add( new GridBoundColumn 
						{
								DataField = "Name",
								HeaderText = Resources.Controls.AdminProductsControl_ColumnName,
								SortExpression = "Name",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.MasterTableView.Columns.Add( new GridBoundColumn
						{
								DataField = "Code",
								HeaderText = Resources.Controls.AdminProductsControl_ColumnCode,
								SortExpression = "Code",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.MasterTableView.Columns.Add( new GridBoundColumn
						{
								DataField = "Manufacturer",
								HeaderText = Resources.Controls.AdminProductsControl_ColumnManufacturer,
								SortExpression = "Manufacturer",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						//grid.MasterTableView.Columns.Add( new GridBoundColumn
						//{
						//    DataField = "Description",
						//    HeaderText = Resources.Controls.AdminProductsControl_ColumnDescription,
						//    SortExpression = "Description",
						//} );
						grid.MasterTableView.Columns.Add( new GridPriceColumn
						{
								DataField = "Price",
								HeaderText = Resources.Controls.AdminProductsControl_ColumnPrice,
								SortExpression = "Price",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.MasterTableView.Columns.Add( new GridBoundColumn
						{
								DataField = "DiscountTypeText",
								HeaderText = Resources.Controls.AdminProductsControl_ColumnDiscountType,
								SortExpression = "DiscountTypeText",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.MasterTableView.Columns.Add( new GridBoundColumn
						{
								DataField = "Discount",
								DataFormatString = "{0:n}",
								HeaderText = Resources.Controls.AdminProductsControl_ColumnDiscount,
								SortExpression = "Discount",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.MasterTableView.Columns.Add( new GridBoundColumn
						{
								DataField = "Alias",
								HeaderText = Resources.Controls.AdminProductsControl_ColumnAlias,
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

						return grid;
				}

				void OnRowCommand( object source, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( source, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( source, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( source, e );
				}
				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						e.Canceled = true;
						Response.Redirect( Page.ResolveUrl( NewUrl + "&" + base.BuildReturnUrlQueryParam() ) );
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int productId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( EditUrlFormat, productId ) + "&" + base.BuildReturnUrlQueryParam() );
				}
				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int productId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						ProductEntity product = Storage<ProductEntity>.ReadFirst( new ProductEntity.ReadById { ProductId = productId } );
						Storage<ProductEntity>.Delete( product );
						GridViewDataBind( true );
				}
		}
}

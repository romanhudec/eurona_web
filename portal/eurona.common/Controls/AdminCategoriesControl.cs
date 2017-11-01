using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Controls;
using Telerik.Web.UI;
using CategoryEntity = SHP.Entities.Category;
using SHP.Controls.Category;

namespace Eurona.Common.Controls.Category
{
		public class AdminCategoriesControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";
				private const string CHILD_ITEMS_COMMAND = "CHILD_ITEMS";
				private const string ATTRIBUTES_ITEMS_COMMAND = "ATTRIBUTES_ITEMS";

				private RadGrid gridView;
				private CategoryPathControl categoryPathControl;

				public string EditUrlFormat { get; set; }
				public string NewUrl
				{
						get
						{
								object o = ViewState["NewUrl"];
								return o != null ? o.ToString() : string.Empty;
						}
						set { ViewState["NewUrl"] = value; }
				}
				/// <summary>
				/// Url pre navigacie po jednotlivych kategoriach/podkategoriach
				/// </summary>
				public string CategoriesUrlFormat { get; set; }
				/// <summary>
				/// Url pre management attributov kategorie
				/// </summary>
				public string AttributesUrlFormat { get; set; }
				
				/// <summary>
				/// ID rodičovskej kategórie.
				/// </summary>
				public int? ParentId
				{
						get
						{
								object o = ViewState["ParentId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ParentId"] = value; }
				}

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminCategoriesControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminCategoriesControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminCategoriesControl-SortExpression", "Order" ); }
						set { SetSession<string>( "AdminCategoriesControl-SortExpression", value ); }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.categoryPathControl = new CategoryPathControl();
						this.categoryPathControl.CategoryId = this.ParentId;
						this.categoryPathControl.NavigateUrlFormat = this.CategoriesUrlFormat;
						this.Controls.Add( this.categoryPathControl );
						this.Controls.Add( new LiteralControl( "<br />" ) );

						gridView = CreateGridView();
						this.Controls.Add( gridView );
						GridViewDataBind( !IsPostBack );
				}

				private void GridViewDataBind( bool bind )
				{
						List<CategoryEntity> categories = Storage<CategoryEntity>.Read( new CategoryEntity.ReadByParentId { ParentId = this.ParentId } );

						var ordered = categories.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
						gridView.DataSource = ordered.ToList();
						if ( bind ) gridView.DataBind();
				}

				private RadGrid CreateGridView()
				{
						RadGrid grid = new RadGrid();
						CMS.Utilities.RadGridUtilities.Localize( grid );
						grid.MasterTableView.DataKeyNames = new string[] { "Id" };

						grid.AllowAutomaticInserts = false;
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

						grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = SHP.Resources.Controls.AdminCategoriesControl_NewCategoryButton_Text;

						grid.MasterTableView.Columns.Add( new GridBoundColumn
						{
								DataField = "Order",
								HeaderText = SHP.Resources.Controls.AdminCategoriesControl_ColumnOrder,
								SortExpression = "Order",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.MasterTableView.Columns.Add( new GridBoundColumn 
						{
								DataField = "Name",
								HeaderText = SHP.Resources.Controls.AdminCategoriesControl_ColumnName,
								SortExpression = "Name",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Alias",
								HeaderText = SHP.Resources.Controls.AdminCategoriesControl_ColumnAlias,
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );

						GridButtonColumn btnEdit = new GridButtonColumn();
						btnEdit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
						btnEdit.Text = SHP.Resources.Controls.GridView_ToolTip_EditItem;
						btnEdit.ButtonType = GridButtonColumnType.ImageButton;
						btnEdit.CommandName = EDIT_COMMAND;
						grid.MasterTableView.Columns.Add( btnEdit );


						GridButtonColumn btnSubItems = new GridButtonColumn();
						btnSubItems.HeaderStyle.Width = Unit.Pixel( 16 );
						btnSubItems.ImageUrl = ConfigValue( "SHP:CategoryItemsImage" );
						btnSubItems.Text = SHP.Resources.Controls.AdminCategoriesControl_SubCategoriesToolTip;
						btnSubItems.ButtonType = GridButtonColumnType.ImageButton;
						btnSubItems.CommandName = CHILD_ITEMS_COMMAND;
						grid.Columns.Add( btnSubItems );

						//GridButtonColumn btnAttributes = new GridButtonColumn();
						//btnAttributes.HeaderStyle.Width = Unit.Pixel( 16 );
						//btnAttributes.ImageUrl = ConfigValue( "SHP:AttributeItemsImage" );
						//btnAttributes.Text = SHP.Resources.Controls.AdminCategoriesControl_AttributesToolTip;
						//btnAttributes.ButtonType = GridButtonColumnType.ImageButton;
						//btnAttributes.CommandName = ATTRIBUTES_ITEMS_COMMAND;
						//grid.Columns.Add( btnAttributes );

						//GridButtonColumn btnDelete = new GridButtonColumn();
						//btnDelete.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						//btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImage" );
						//btnDelete.Text = SHP.Resources.Controls.GridView_ToolTip_DeleteItem;
						//btnDelete.ButtonType = GridButtonColumnType.ImageButton;
						//btnDelete.ConfirmTitle = SHP.Resources.Controls.DeleteItemQuestion;
						//btnDelete.ConfirmText = SHP.Resources.Controls.DeleteItemQuestion;
						//btnDelete.CommandName = DELETE_COMMAND;
						//grid.MasterTableView.Columns.Add( btnDelete );

						grid.ItemCommand += OnRowCommand;

						return grid;
				}

				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
						if ( e.CommandName == CHILD_ITEMS_COMMAND ) OnSubCategoriesCommand( sender, e );
						if ( e.CommandName == ATTRIBUTES_ITEMS_COMMAND ) OnAttributesCommand( sender, e );
				}
				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						e.Canceled = true;
						Response.Redirect( Page.ResolveUrl( NewUrl + "&" + base.BuildReturnUrlQueryParam() ) );
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int categoryId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( EditUrlFormat, categoryId ) + "&" + base.BuildReturnUrlQueryParam() );
				}
				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int categoryId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						CategoryEntity category = Storage<CategoryEntity>.ReadFirst( new CategoryEntity.ReadById { CategoryId = categoryId } );
						Storage<CategoryEntity>.Delete( category );
						GridViewDataBind( true );
				}

				private void OnSubCategoriesCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int categoryId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( CategoriesUrlFormat, categoryId ) + "&" + base.BuildReturnUrlQueryParam(), false );
				}

				private void OnAttributesCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int categoryId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( this.AttributesUrlFormat, categoryId ) + "&" + base.BuildReturnUrlQueryParam(), false );
				}
		}
}

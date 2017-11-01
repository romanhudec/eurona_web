using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using MenuEntity = CMS.Entities.NavigationMenuItem;
using Telerik.Web.UI;

namespace CMS.Controls.Menu
{
		public class AdminNavigationMenuItemsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";
				private const string MOVE_UP_COMMAND = "UP_ITEM";
				private const string MOVE_DOWN_COMMAND = "DOWN_ITEM";

				private RadGrid gridView;

				public string EditUrlFormat { get; set; }
				public string NewUrl { get; set; }


				/// <summary>
				/// Parent menu Id
				/// </summary>
				public int NavigationMenuId{get; set;}

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminNavigationMenuItemsControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminNavigationMenuItemsControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminNavigationMenuItemsControl-SortExpression", "Order" ); }
						set { SetSession<string>( "AdminNavigationMenuItemsControl-SortExpression", value ); }
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
						List<MenuEntity> menus = Storage<MenuEntity>.Read( new MenuEntity.ReadByNavigationMenuId { NavigationMenuId = this.NavigationMenuId } );
						var ordered = menus.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
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
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminMenusControl_NewMenuButton_Text;

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Order",
								HeaderText = Resources.Controls.AdminMenusControl_ColumnOrder,
								SortExpression = "Order",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridIconColumn
						{
								DataImageUrlFields = new string[] { "Icon" },
								ImageWidth = Unit.Pixel( 16 ),
								ImageHeight = Unit.Pixel( 16 ),
								HeaderText = Resources.Controls.AdminMenusControl_ColumnIcon,
								ShowFilterIcon = false
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Name",
								HeaderText = Resources.Controls.AdminMenusControl_ColumnName,
								SortExpression = "Name",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Alias",
								HeaderText = Resources.Controls.AdminMenusControl_ColumnAlias,
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "RoleName",
								HeaderText = Resources.Controls.AdminAccountsControl_ColumnRole,
								SortExpression = "RoleName",
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

				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
				}
				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						string url = Page.ResolveUrl( string.Format( NewUrl, this.NavigationMenuId ) + "&" + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int menuId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( EditUrlFormat, menuId ) + "&" + base.BuildReturnUrlQueryParam() );
				}

				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int menuId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						MenuEntity menu = Storage<MenuEntity>.ReadFirst( new MenuEntity.ReadById { NavigationMenuItemId = menuId } );
						Storage<MenuEntity>.Delete( menu );
						GridViewDataBind( true );
				}

		}
}

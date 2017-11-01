using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using BlogEntity = CMS.Entities.Blog;
using Telerik.Web.UI;

namespace CMS.Controls.Blog
{
		public class AdminBlogsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";
				private const string APPROVE_COMMAND = "APPROVE_ITEM";
				private const string SHOW_COMMAND = "SHOW_ITEM";
				private const string HIDE_COMMAND = "HIDE_ITEM";

				private RadGrid dataGrid = null;

				public SortDirection SortDirection
				{
						get { return GetState<SortDirection>( "AdminBlogsControl-SortDirection", SortDirection.Descending ); }
						set { SetState<SortDirection>( "AdminBlogsControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetState<string>( "AdminBlogsControl-SortExpression" ); }
						set { SetState<string>( "AdminBlogsControl-SortExpression", value ); }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						//Povolene len pre prihlaseneho pouzivatela
						if ( !Security.IsLogged( true ) ) return;

						base.CreateChildControls();

						this.dataGrid = CreateGridControl();
						this.Controls.Add( this.dataGrid );

						//Binding
						this.dataGrid.DataSource = GetDataGridData();
						if ( !IsPostBack )
								this.dataGrid.DataBind();

				}
				#endregion

				public string NewUrl { get; set; }
				public string EditUrlFormat { get; set; }

				/// <summary>
				/// Vytvori DataGrid control s pozadovanymi stlpcami a 
				/// pripravenym bindingom stlpcou.
				/// </summary>
				private RadGrid CreateGridControl()
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

						if ( !Security.IsInRole( Entities.Role.ADMINISTRATOR ) )
						{
								grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;
								grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminBlogsControl_NewBlog;
						}

						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Title";
						bf.HeaderText = Resources.Controls.AdminBlogsControl_ColumnTitle;
						bf.SortExpression = "Title";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "ReleaseDate";
						bf.HeaderText = Resources.Controls.AdminBlogsControl_ColumnReleaseDate;
						bf.SortExpression = "ReleaseDate";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						bf.DataFormatString = "{0:d}";
						grid.Columns.Add( bf );

						GridButtonColumn btnEdit = new GridButtonColumn();
						btnEdit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
						btnEdit.Text = Resources.Controls.GridView_ToolTip_EditItem;
						btnEdit.ButtonType = GridButtonColumnType.ImageButton;
						btnEdit.CommandName = EDIT_COMMAND;
						grid.MasterTableView.Columns.Add( btnEdit );

						GridButtonColumn btnApprove = new GridButtonColumn();
						btnApprove.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnApprove.ImageUrl = ConfigValue( "CMS:ApproveButtonImage" );
						btnApprove.Text = Resources.Controls.GridView_ToolTip_Approve;
						btnApprove.ButtonType = GridButtonColumnType.ImageButton;
						btnApprove.CommandName = APPROVE_COMMAND;
						grid.MasterTableView.Columns.Add( btnApprove );

						GridButtonColumn btnShowInCatalog = new GridButtonColumn();
						btnShowInCatalog.HeaderStyle.Width = Unit.Pixel( 16 );
						btnShowInCatalog.ImageUrl = ConfigValue( "CMS:ShowButtonImage" );
						btnShowInCatalog.Text = Resources.Controls.GridView_ToolTip_Show;
						btnShowInCatalog.ButtonType = GridButtonColumnType.ImageButton;
						btnShowInCatalog.CommandName = SHOW_COMMAND;
						grid.MasterTableView.Columns.Add( btnShowInCatalog );

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

				private List<BlogEntity> GetDataGridData()
				{
						List<BlogEntity> list = null;
						if( Security.IsInRole( Entities.Role.ADMINISTRATOR ) )
								list = Storage<BlogEntity>.Read();
						else
								list = Storage<BlogEntity>.Read( new BlogEntity.ReadByAccountId { AccountId = Security.Account.Id } );

						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "ReleaseDate" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();
				}

				#region Event handlers
				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
						if ( e.CommandName == APPROVE_COMMAND ) OnApprovetCommand( sender, e );
						if ( e.CommandName == SHOW_COMMAND ) OnShowCommand( sender, e );
						if ( e.CommandName == HIDE_COMMAND ) OnHideCommand( sender, e );
				}
				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						string url = Page.ResolveUrl( string.Format( "{0}?{1}", this.NewUrl, base.BuildReturnUrlQueryParam() ) );
						Response.Redirect( url );
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						string url = Page.ResolveUrl( string.Format( EditUrlFormat, id ) + "&" + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
				}
				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						BlogEntity blog = Storage<BlogEntity>.ReadFirst( new BlogEntity.ReadById { BlogId = id } );
						Storage<BlogEntity>.Delete( blog );

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}
				private void OnApprovetCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						BlogEntity blog = Storage<BlogEntity>.ReadFirst( new BlogEntity.ReadById { BlogId = id } );
						blog.Approved = true;
						Storage<BlogEntity>.Update( blog );

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}
				private void OnShowCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						BlogEntity blog = Storage<BlogEntity>.ReadFirst( new BlogEntity.ReadById { BlogId = id } );
						blog.Visible = true;
						Storage<BlogEntity>.Update( blog );

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}
				private void OnHideCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						BlogEntity blog = Storage<BlogEntity>.ReadFirst( new BlogEntity.ReadById { BlogId = id } );
						blog.Visible = false;
						Storage<BlogEntity>.Update( blog );

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}

				void OnRowDataBound( object sender, GridItemEventArgs e )
				{
						if ( e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem )
								return;

						BlogEntity blog = e.Item.DataItem as BlogEntity;

						int deleteIndex = e.Item.Cells.Count - 1;
						int showHideIndex = e.Item.Cells.Count - 2;
						int approveIndex = e.Item.Cells.Count - 3;

						ImageButton btnDelete = ( e.Item.Cells[deleteIndex].Controls[0] as ImageButton );
						ImageButton btnShowInCatalogHide = ( e.Item.Cells[showHideIndex].Controls[0] as ImageButton );
						ImageButton btnApprove = ( e.Item.Cells[approveIndex].Controls[0] as ImageButton );

						if ( blog.Approved )
						{
								btnApprove.ImageUrl = ConfigValue( "CMS:OKButtonImageD" );
								btnApprove.Enabled = false;
						}

						//Ak je blog schvaleny
						if ( blog.Approved )
						{
								//Zobrazenie v zozname
								if ( blog.Visible )
								{
										btnShowInCatalogHide.ImageUrl = ConfigValue( "CMS:ShowButtonImage" );
										btnShowInCatalogHide.CommandName = HIDE_COMMAND;
										btnShowInCatalogHide.ToolTip = Resources.Controls.GridView_ToolTip_Hide;
								}
								else
								{
										btnShowInCatalogHide.ImageUrl = ConfigValue( "CMS:HideButtonImage" );
										btnShowInCatalogHide.CommandName = SHOW_COMMAND;
										btnShowInCatalogHide.ToolTip = Resources.Controls.GridView_ToolTip_Show;
								}
						}
						else
						{
								btnShowInCatalogHide.ImageUrl = ConfigValue( "CMS:ShowButtonImageD" );
								btnShowInCatalogHide.CommandName = SHOW_COMMAND;
								btnShowInCatalogHide.ToolTip = Resources.Controls.GridView_ToolTip_Show;
								btnShowInCatalogHide.Enabled = false;
						}
				}
				#endregion
		}
}

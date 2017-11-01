using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using TagEntity = CMS.Entities.Tag;
using System.Web.UI;
using Telerik.Web.UI;

namespace CMS.Controls.Tag
{
		public class AdminTagsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";

				private RadGrid gridView;
				public AdminTagsControl()
				{
				}

				public string EditUrlFormat { get; set; }
				public string NewUrl { get; set; }

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminTagsControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminTagsControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminTagsControl-SortExpression", "Name" ); }
						set { SetSession<string>( "AdminTagsControl-SortExpression", value ); }
				}

				public bool HideCredit { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						gridView = CreateGridView();
						this.Controls.Add( gridView );

						GridViewDataBind( !IsPostBack );
				}

				private void GridViewDataBind( bool bind )
				{
						List<TagEntity> roles = Storage<TagEntity>.Read();
						var ordered = roles.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
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
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminTagsControl_NewTagButton_Text;

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Name",
								HeaderText = Resources.Controls.AdminTagsControl_ColumnName,
								SortExpression = "Name",
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

						TagEntity tag = e.Item.DataItem as TagEntity;

						int nameIndex = 2;
						int deleteIndex = e.Item.Cells.Count - 1;
						ImageButton btnDelete = ( e.Item.Cells[deleteIndex].Controls[0] as ImageButton );

						//Systemove tag
						if ( tag.Id < 0 )
						{
								e.Item.Cells[nameIndex].Font.Bold = true;
								btnDelete.Enabled = false;
								btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImageD" );
						}
				}

				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
				}
				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						string url = Page.ResolveUrl( NewUrl + "?" + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
				}
				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int tagId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						TagEntity tag = Storage<TagEntity>.ReadFirst( new TagEntity.ReadById { TagId = tagId } );
						Storage<TagEntity>.Delete( tag );
						GridViewDataBind( true );
				}

				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int tagId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						Response.Redirect( String.Format( EditUrlFormat, tagId ) + "&" + base.BuildReturnUrlQueryParam() );
				}
		}
}

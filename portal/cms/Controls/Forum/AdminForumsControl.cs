using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ForumEntity = CMS.Entities.Forum;
using ForumThreadEntity = CMS.Entities.ForumThread;
using Telerik.Web.UI;
using System.Web.UI;

namespace CMS.Controls.Forum
{
		public class AdminForumsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";
				private const string FORUM_POSTS_COMMAND = "FORUM_POSTS_ITEM";

				private DropDownList ddlForumThread = null;
				private RadGrid dataGrid = null;

				public int? ForumThreadId { get; set; }

				public SortDirection SortDirection
				{
						get { return GetState<SortDirection>( "AdminForumsControl-SortDirection", SortDirection.Descending ); }
						set { SetState<SortDirection>( "AdminForumsControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetState<string>( "AdminForumsControl-SortExpression" ); }
						set { SetState<string>( "AdminForumsControl-SortExpression", value ); }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.ddlForumThread = new DropDownList();
						this.ddlForumThread.ID = "ddlForumThread";
						this.ddlForumThread.AutoPostBack = true;
						this.ddlForumThread.DataSource = Storage<ForumThreadEntity>.Read();
						this.ddlForumThread.DataTextField = "Name";
						this.ddlForumThread.DataValueField = "Id";
						this.ddlForumThread.SelectedIndexChanged += new EventHandler( OnForumThreadChanged );

						this.Controls.Add( new LiteralControl( Resources.Controls.AdminForumsControl_ForumThread ) );
						this.Controls.Add( this.ddlForumThread );
						this.Controls.Add( new LiteralControl( "<div><br/></div>" ) );

						this.dataGrid = CreateGridControl();
						this.Controls.Add( this.dataGrid );

						//Binding
						if ( !IsPostBack )
						{
								if ( this.ddlForumThread.Items.Count != 0 )
								{
										if ( this.ForumThreadId.HasValue )
												this.ddlForumThread.SelectedValue = this.ForumThreadId.Value.ToString();
										else
												this.ddlForumThread.SelectedIndex = 0;
								}
								this.ddlForumThread.DataBind();

								//this.dataGrid.DataBind();
								this.OnForumThreadChanged( this, null );
						}

				}

				#endregion

				public string NewUrl { get; set; }
				public string ForumPostsUrlFormat { get; set; }
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

						grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminForumsControl_NewForum;


						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Name";
						bf.HeaderText = Resources.Controls.AdminForumsControl_ColumnName;
						bf.SortExpression = "Name";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						GridIconColumn ic = new GridIconColumn();
						ic.DataImageUrlFields = new string[] { "Icon" };
						ic.ImageWidth = Unit.Pixel( 16 );
						ic.ImageHeight = Unit.Pixel( 16 );
						ic.HeaderText = Resources.Controls.AdminForumsControl_ColumnIcon;
						ic.ShowFilterIcon = false;
						ic.AllowFiltering = false;
						grid.Columns.Add( ic );

						GridCheckBoxColumn cbc = new GridCheckBoxColumn();
						cbc.DataField = "Locked";
						cbc.HeaderText = Resources.Controls.AdminForumsControl_ColumnLocked;
						cbc.SortExpression = "Locked";
						cbc.AutoPostBackOnFilter = true;
						cbc.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( cbc );

						cbc = new GridCheckBoxColumn();
						cbc.DataField = "Pinned";
						cbc.HeaderText = Resources.Controls.AdminForumsControl_ColumnPinned;
						cbc.SortExpression = "Pinned";
						cbc.AutoPostBackOnFilter = true;
						cbc.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( cbc );

						bf = new GridBoundColumn();
						bf.DataField = "ForumPostCount";
						bf.HeaderText = Resources.Controls.AdminForumsControl_ColumnPostCount;
						bf.SortExpression = "ForumPostCount";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "ViewCount";
						bf.HeaderText = Resources.Controls.AdminForumsControl_ColumnViewCount;
						bf.SortExpression = "ViewCount";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						GridButtonColumn btnPosts = new GridButtonColumn();
						btnPosts.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnPosts.ImageUrl = ConfigValue( "CMS:DisplayButtonImage" );
						btnPosts.Text = Resources.Controls.GridView_ToolTip_EditPosts;
						btnPosts.ButtonType = GridButtonColumnType.ImageButton;
						btnPosts.CommandName = FORUM_POSTS_COMMAND;
						grid.MasterTableView.Columns.Add( btnPosts );

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

				void OnForumThreadChanged( object sender, EventArgs e )
				{
						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}

				private List<ForumEntity> GetDataGridData()
				{
						if ( string.IsNullOrEmpty( this.ddlForumThread.SelectedValue ) ) return new List<ForumEntity>();
						int forumThreadId = Convert.ToInt32( this.ddlForumThread.SelectedValue );

						List<ForumEntity> list = Storage<ForumEntity>.Read( new ForumEntity.ReadByForumThreadId { ForumThreadId = forumThreadId } );
						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "Pinned" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();
				}

				#region Event handlers
				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == FORUM_POSTS_COMMAND ) OnForumPostsCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
				}

				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						if ( string.IsNullOrEmpty( this.ddlForumThread.SelectedValue ) ) return;
						int threadId = Convert.ToInt32( this.ddlForumThread.SelectedValue );
						string url = Page.ResolveUrl( string.Format( "{0}&{1}", string.Format( this.NewUrl, threadId ), base.BuildReturnUrlQueryParam() ) );
						Response.Redirect( url );
				}
				private void OnForumPostsCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						string url = Page.ResolveUrl( string.Format( ForumPostsUrlFormat, id ) + "&" + base.BuildReturnUrlQueryParam() );
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
						int forumId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						ForumEntity forum = Storage<ForumEntity>.ReadFirst( new ForumEntity.ReadById { ForumId = forumId } );
						Storage<ForumEntity>.Delete( forum );

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}

				void OnRowDataBound( object sender, GridItemEventArgs e )
				{
						if ( e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem )
								return;
				}
				#endregion
		}
}

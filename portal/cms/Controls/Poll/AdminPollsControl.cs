using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS.Entities;
using PollEntity = CMS.Entities.Poll;
using Telerik.Web.UI;

namespace CMS.Controls.Poll
{
		public class AdminPollsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string DISPLAY_COMMAND = "DISPLAY_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";
				private const string OPTIONS_COMMAND = "OPTIONS";
				private const string CLOSEPOLL_COMMAND = "CLOSE_POLL";

				private RadGrid dataGrid = null;

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminPollsControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminPollsControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetSession<string>( "AdminPollsControl-SortExpression" ); }
						set { SetSession<string>( "AdminPollsControl-SortExpression", value ); }
				}

				public AdminPollsControl()
				{
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.dataGrid = CreateGridControl();
						this.Controls.Add( this.dataGrid );

						//Binding
						this.dataGrid.DataSource = GetDataGridData();
						if ( !IsPostBack )
								this.dataGrid.DataBind();

				}
				#endregion

				public string DisplayUrlFormat { get; set; }
				public string NewUrl { get; set; }
				public string EditUrlFormat { get; set; }
				public string OptionsUrlFormat { get; set; }

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
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminPollsControl_NewPoll;

						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Question";
						bf.HeaderText = Resources.Controls.AdminPollsControl_ColumnQuestion;
						bf.SortExpression = "Question";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "Closed";
						bf.HeaderText = Resources.Controls.AdminPollsControl_ColumnClosed;
						bf.SortExpression = "Closed";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "DateFrom";
						bf.HeaderText = Resources.Controls.AdminPollsControl_ColumnDateFrom;
						bf.SortExpression = "DateFrom";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						bf.DataFormatString = "{0:d}";
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "DateTo";
						bf.HeaderText = Resources.Controls.AdminPollsControl_ColumnDateTo;
						bf.SortExpression = "DateTo";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						bf.DataFormatString = "{0:d}";
						grid.Columns.Add( bf );

						GridButtonColumn btnDisplay = new GridButtonColumn();
						btnDisplay.HeaderStyle.Width = Unit.Pixel( 16 );
						btnDisplay.ImageUrl = ConfigValue( "CMS:DisplayButtonImage" );
						btnDisplay.Text = Resources.Controls.AdminPollsControl_ColumnShowPollResult;
						btnDisplay.ButtonType = GridButtonColumnType.ImageButton;
						btnDisplay.CommandName = DISPLAY_COMMAND;
						grid.Columns.Add( btnDisplay );

						GridButtonColumn btnPollItems = new GridButtonColumn();
						btnPollItems.HeaderStyle.Width = Unit.Pixel( 16 );
						btnPollItems.ImageUrl = ConfigValue( "CMS:PollItemsButtonImage" );
						btnPollItems.ButtonType = GridButtonColumnType.ImageButton;
						btnPollItems.Text = Resources.Controls.AdminPollsControl_ColumnPollOptions;
						btnPollItems.CommandName = OPTIONS_COMMAND;
						grid.Columns.Add( btnPollItems );

						GridButtonColumn btnEdit = new GridButtonColumn();
						btnEdit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
						btnEdit.Text = Resources.Controls.GridView_ToolTip_EditItem;
						btnEdit.ButtonType = GridButtonColumnType.ImageButton;
						btnEdit.CommandName = EDIT_COMMAND;
						grid.MasterTableView.Columns.Add( btnEdit );

						GridButtonColumn btnClose = new GridButtonColumn();
						btnClose.HeaderStyle.Width = Unit.Pixel( 16 );
						btnClose.ImageUrl = ConfigValue( "CMS:CloseButtonImage" );
						btnClose.Text = Resources.Controls.AdminPollsControl_ColumnClosePoll;
						btnClose.ButtonType = GridButtonColumnType.ImageButton;
						btnClose.CommandName = CLOSEPOLL_COMMAND;
						grid.Columns.Add( btnClose );

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

				private List<PollEntity> GetDataGridData()
				{
						List<PollEntity> list = Storage<PollEntity>.Read();
						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "DateFrom" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();
				}

				#region Event handlers
				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						//Ak to je neznamy command, nebu se spracovavat tu.
						if ( e.CommandName != DISPLAY_COMMAND &&
								e.CommandName != EDIT_COMMAND &&
								e.CommandName != DELETE_COMMAND &&
								e.CommandName != OPTIONS_COMMAND &&
								e.CommandName != CLOSEPOLL_COMMAND &&
								e.CommandName != RadGrid.InitInsertCommandName )
								return;

						if ( e.CommandName == RadGrid.InitInsertCommandName )
						{
								string url = Page.ResolveUrl( string.Format( "{0}?{1}", this.NewUrl, base.BuildReturnUrlQueryParam() ) );
								Response.Redirect( url );
						}

						GridDataItem dataItem = (GridDataItem)e.Item;
						int pollId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						if ( e.CommandName == DISPLAY_COMMAND )
								Response.Redirect( string.Format( DisplayUrlFormat, pollId ) + "&" + base.BuildReturnUrlQueryParam() );

						if ( e.CommandName == EDIT_COMMAND )
								Response.Redirect( string.Format( EditUrlFormat, pollId ) + "&" + base.BuildReturnUrlQueryParam() );

						if ( e.CommandName == DELETE_COMMAND )
						{
								PollEntity poll = Storage<PollEntity>.ReadFirst( new PollEntity.ReadById { PollId = pollId } );
								Storage<PollEntity>.Delete( poll );

								this.dataGrid.DataSource = GetDataGridData();
								this.dataGrid.DataBind();
						}

						if ( e.CommandName == OPTIONS_COMMAND )
						{
								string url = Page.ResolveUrl( string.Format( OptionsUrlFormat, pollId ) + "&" + base.BuildReturnUrlQueryParam() );
								Response.Redirect( url );
						}

						if ( e.CommandName == CLOSEPOLL_COMMAND )
						{
								PollEntity poll = Storage<PollEntity>.ReadFirst( new PollEntity.ReadById { PollId = pollId } );
								poll.Closed = true;
								poll.DateTo = DateTime.Now.Date;
								Storage<PollEntity>.Update( poll );

								this.dataGrid.DataSource = GetDataGridData();
								this.dataGrid.DataBind();
						}
				}

				#endregion
		}
}

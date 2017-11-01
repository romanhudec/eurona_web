using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using CMS.Entities;
using PollEntity = CMS.Entities.Poll;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace CMS.Controls.Poll
{
		public class AdminOptionsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";

				private RadGrid dataGrid = null;

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminOptionsControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminOptionsControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetSession<string>( "AdminOptionsControl-SortExpression" ); }
						set { SetSession<string>( "AdminOptionsControl-SortExpression", value ); }
				}

				public AdminOptionsControl()
				{
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control pollControl = CreateOptionListControl();
						if ( pollControl != null ) this.Controls.Add( pollControl );
						else return;

						//Binding
						this.dataGrid.DataSource = GetDataGridData();

						if ( !IsPostBack )
								this.dataGrid.DataBind();

				}
				#endregion

				public string NewUrlFormat { get; set; }
				public string EditUrlFormat { get; set; }

				/// <summary>
				/// Vytvori Control zoznamu poloziek Ankity
				/// </summary>
				private Control CreateOptionListControl()
				{
						if ( string.IsNullOrEmpty( this.Request["pollId"] ) )
								return null;

						int pollId = Convert.ToInt32( this.Request["pollId"] );
						PollEntity poll = Storage<PollEntity>.ReadFirst( new PollEntity.ReadById() { PollId = pollId } );

						HtmlGenericControl div = new HtmlGenericControl( "div" );

						div.Controls.Add( new LiteralControl(
								string.Format( "{0} <b>{1}</b>", Resources.Controls.AdminOptionsControl_Poll, poll.Question ) ) );

						//DataGrid
						this.dataGrid = CreateGridControl();
						div.Controls.Add( this.dataGrid );

						return div;
				}

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
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminOptionsControl_NewOption;

						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Order";
						bf.HeaderText = Resources.Controls.AdminOptionsControl_ColumnOrder;
						bf.SortExpression = "Order";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "Name";
						bf.HeaderText = Resources.Controls.AdminOptionsControl_ColumnName;
						bf.SortExpression = "Name";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

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

				private List<PollOption> GetDataGridData()
				{
						if ( string.IsNullOrEmpty( this.Request["pollId"] ) )
								return new List<PollOption>();

						int pollId = Convert.ToInt32( this.Request["pollId"] );
						List<PollOption> list = Storage<PollOption>.Read( new PollOption.ReadByPollId() { PollId = pollId } );

						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "Order" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();

				}

				#region Event handlers
				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						//Ak to je neznamy command, nebu se spracovavat tu.
						if ( e.CommandName != EDIT_COMMAND &&
								e.CommandName != DELETE_COMMAND &&
								e.CommandName != RadGrid.InitInsertCommandName )
								return;

						if ( e.CommandName == RadGrid.InitInsertCommandName )
						{
								if ( string.IsNullOrEmpty( this.Request["pollId"] ) ) return;
								int pollId = Convert.ToInt32( this.Request["pollId"] );

								string newUrl = string.Format( this.NewUrlFormat, pollId );
								Response.Redirect( Page.ResolveUrl( string.Format( "{0}&{1}", newUrl, base.BuildReturnUrlQueryParam() ) ) );
								return;
						}

						GridDataItem dataItem = (GridDataItem)e.Item;
						int pollOptionId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						if ( e.CommandName == EDIT_COMMAND )
						{
								string url = Page.ResolveUrl( string.Format( EditUrlFormat, pollOptionId ) + "&" + base.BuildReturnUrlQueryParam() );
								Response.Redirect( url );
								return;
						}

						if ( e.CommandName == DELETE_COMMAND )
						{
								PollOption pollOption = Storage<PollOption>.ReadFirst( new PollOption.ReadById { PollOptionId = pollOptionId } );
								Storage<PollOption>.Delete( pollOption );

								this.dataGrid.DataSource = GetDataGridData();
								this.dataGrid.DataBind();
						}
				}
				#endregion
		}
}

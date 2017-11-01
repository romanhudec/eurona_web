using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForumPostAttachmentEntity = CMS.Entities.ForumPostAttachment;
using ForumPostEntity = CMS.Entities.ForumPost;
using ForumEntity = CMS.Entities.Forum;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;
using CMS.Controls.RadEditor;
using System.Text;
using CMS.Utilities;
using System.IO;
using Telerik.Web.UI;

namespace CMS.Controls.Forum
{
		public class AdminForumPostControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";

				private TextBox txtTitle = null;
				private RadEditor.RadEditor txtContent = null;
				private RadGrid dataGrid = null;
				private ASPxMultipleFileUpload mfuFiles = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private ForumPostEntity ForumPost = null;

				public AdminForumPostControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? ForumPostId
				{
						get
						{
								object o = ViewState["ForumPostId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ForumPostId"] = value; }
				}

				/// <summary>
				///
				/// </summary>
				public int? ForumId
				{
						get
						{
								object o = ViewState["ForumId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["ForumId"] = value; }
				}

				public SortDirection SortDirection
				{
						get { return GetState<SortDirection>( "AdminForumPostControl-SortDirection", SortDirection.Ascending ); }
						set { SetState<SortDirection>( "AdminForumPostControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetState<string>( "AdminForumPostControl-SortExpression" ); }
						set { SetState<string>( "AdminForumPostControl-SortExpression", value ); }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						//Get data
						if ( !this.ForumPostId.HasValue )
						{
								if ( !this.ForumId.HasValue ) throw new InvalidOperationException( "If create new Forum, Property ForumThreadId must be set!!" );
								this.ForumPost = new ForumPostEntity();
								this.ForumPost.ForumId = this.ForumId.Value;
								this.ForumPost.Date = DateTime.Now;
						}
						else this.ForumPost = Storage<ForumPostEntity>.ReadFirst( new ForumPostEntity.ReadById { ForumPostId = this.ForumPostId.Value } );
						ForumEntity forumThread = Storage<ForumEntity>.ReadFirst( new ForumEntity.ReadById { ForumId = this.ForumPost.ForumId } );

						Label lblForumThreadName = new Label();
						lblForumThreadName.Font.Bold = true;
						lblForumThreadName.Text = forumThread.Name;
						this.Controls.Add( lblForumThreadName );

						this.Controls.Add( new LiteralControl( "<div><br/></div>" ) );

						Control forumControl = CreateDetailControl();
						if ( forumControl != null )
								this.Controls.Add( forumControl );

						this.dataGrid.DataSource = GetDataGridData();
						//Binding
						if ( !IsPostBack )
						{
								this.dataGrid.DataBind();
								this.txtTitle.Text = this.ForumPost.Title;
								this.txtContent.Content = this.ForumPost.Content;
						}
				}
				#endregion

				/// <summary>
				/// Vytvori Control Clanku
				/// </summary>
				private Control CreateDetailControl()
				{
						this.txtTitle = new TextBox();
						this.txtTitle.ID = "txtTitle";
						this.txtTitle.Width = Unit.Percentage( 100 );

						this.txtContent = new CMSEditor();
						this.txtContent.ID = "txtContent";
						this.txtContent.Width = Unit.Percentage( 100 );
						this.txtContent.Height = Unit.Pixel( 400 );

						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = Resources.Controls.SaveButton_Text;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = Resources.Controls.CancelButton_Text;
						this.btnCancel.Click += new EventHandler( OnCancel );

						Table table = new Table();
						table.Width = this.Width;
						table.Height = this.Height;

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminForumPostControl_Title, this.txtTitle, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminForumPostControl_Content, this.txtContent, false ) );

						//Grid
						this.dataGrid = CreateGridControl();
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminForumPostControl_Attachments, this.dataGrid, false ) );

						this.mfuFiles = new ASPxMultipleFileUpload();
						this.mfuFiles.ID = "mfuFiles";
						this.mfuFiles.MaxfilesToUpload = 20;
						this.mfuFiles.EnableDescription = true;
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminForumPostControl_AddAttachments, this.mfuFiles, false ) );

						//Save Cancel Buttons
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnSave );
						cell.Controls.Add( this.btnCancel );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						return table;
				}


				private TableRow CreateTableRow( string labelText, Control control, bool required )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						if ( required ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						row.Cells.Add( cell );

						return row;
				}

				#region Grid attachments methods
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

						grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;

						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Order";
						bf.HeaderText = Resources.Controls.AdminForumPostControl_ColumnOrder;
						bf.SortExpression = "Order";
						bf.ItemStyle.Wrap = false;
						bf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
						bf.AutoPostBackOnFilter = false;
						bf.AllowFiltering = false;
						bf.CurrentFilterFunction = GridKnownFunction.NoFilter;
						bf.ShowFilterIcon = false;
						grid.Columns.Add( bf );

						GridHyperLinkColumn bhf = new GridHyperLinkColumn();
						bhf.DataTextField = "Name";
						bhf.DataNavigateUrlFields = new string[] { "Url" };
						bhf.Target = "_blank";
						bhf.HeaderText = Resources.Controls.AdminForumPostControl_ColumnName;
						bhf.SortExpression = "Name";
						bhf.AutoPostBackOnFilter = true;
						bhf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bhf );

						bf = new GridBoundColumn();
						bf.DataField = "Size";
						bf.HeaderText = Resources.Controls.AdminForumPostControl_ColumnSize;
						bf.SortExpression = "Size";
						bf.ItemStyle.Wrap = false;
						bf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "Description";
						bf.HeaderText = Resources.Controls.AdminForumPostControl_ColumnDescription;
						bf.SortExpression = "Description";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

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

				private List<ForumPostAttachmentEntity> GetDataGridData()
				{
						if ( !this.ForumPostId.HasValue ) return new List<ForumPostAttachmentEntity>();

						List<ForumPostAttachmentEntity> list = Storage<ForumPostAttachmentEntity>.Read( new ForumPostAttachmentEntity.ReadBy { ForumPostId = this.ForumPostId.Value } );
						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "Order" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();
				}

				#region Event handlers
				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
				}
				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int attachmentId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						ForumPostAttachmentEntity attachment = Storage<ForumPostAttachmentEntity>.ReadFirst( new ForumPostAttachmentEntity.ReadById { ForumPostAttachmentId = attachmentId } );
						Storage<ForumPostAttachmentEntity>.Delete( attachment );

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}

				void OnRowDataBound( object sender, GridItemEventArgs e )
				{
						if ( e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem )
								return;
				}
				#endregion
				#endregion

				void OnSave( object sender, EventArgs e )
				{
						this.ForumPost.Title = this.txtTitle.Text;
						this.ForumPost.Content = this.txtContent.Content;

						if ( !this.ForumPostId.HasValue ) this.ForumPost = Storage<ForumPostEntity>.Create( this.ForumPost );
						else Storage<ForumPostEntity>.Update( this.ForumPost );

						ForumPostAttachmentHelper.UpdatePostAttachments( this.Page, this.ForumPost, this.mfuFiles.GetUploadEventArgs() );

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using AttributeEntity = SHP.Entities.Attribute;
using CMS.Controls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace SHP.Controls.Attribute
{
		public class AdminAttributesControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";

				private Category.CategoryPathControl attributePathControl;
				private Telerik.Web.UI.RadGrid gridView;

				public string EditUrlFormat { get; set; }
				public string NewUrlFormat { get; set; }
				/// <summary>
				/// Url pre navigacie po jednotlivych kategoriach
				/// </summary>
				public string CategoriesUrlFormat { get; set; }

				public int CategoryId
				{
						get
						{
								object o = ViewState["CategoryId"];
								return o != null ? (int)Convert.ToInt32( o ) : 0;
						}
						set { ViewState["CategoryId"] = value; }
				}


				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminAttributesControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminAttributesControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminAttributesControl-SortExpression", "Name" ); }
						set { SetSession<string>( "AdminAttributesControl-SortExpression", value ); }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						//Category path control
						this.attributePathControl = new Category.CategoryPathControl();
						this.attributePathControl.CategoryId = this.CategoryId;
						this.attributePathControl.NavigateUrlFormat = this.CategoriesUrlFormat;
						this.Controls.Add( this.attributePathControl );
						this.Controls.Add( new LiteralControl( "<br />" ) );

						//Create Telerik.Web.UI.RadGrid control
						gridView = CreateGridView();
						this.Controls.Add( gridView );

						GridViewDataBind( !IsPostBack );
				}

				private void GridViewDataBind( bool bind )
				{
						List<AttributeEntity> attributes = Storage<AttributeEntity>.Read( new AttributeEntity.ReadByCategoryId { CategoryId = this.CategoryId } );

						var ordered = attributes.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
						gridView.DataSource = ordered.ToList();
						if ( bind ) gridView.DataBind();
				}

				private Telerik.Web.UI.RadGrid CreateGridView()
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

						grid.MasterTableView.CommandItemDisplay = Telerik.Web.UI.GridCommandItemDisplay.TopAndBottom;
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminAttributesControl_NewAttributeButton_Text;

						grid.MasterTableView.Columns.Add( new GridBoundColumn
						{
								DataField = "Name",
								HeaderText = Resources.Controls.AdminAttributesControl_ColumnName,
								SortExpression = "Name",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "TypeName",
								HeaderText = Resources.Controls.AdminAttributesControl_ColumnType,
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "DefaultValue",
								HeaderText = Resources.Controls.AdminAttributesControl_ColumnDefaultValue,
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
						string url = Page.ResolveUrl( string.Format( NewUrlFormat, this.CategoryId ) + "&" + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int attributeId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( EditUrlFormat, attributeId ) + "&" + base.BuildReturnUrlQueryParam() );
				}
				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int attributeId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						AttributeEntity attribute = Storage<AttributeEntity>.ReadFirst( new AttributeEntity.ReadById { AttributeId = attributeId } );
						Storage<AttributeEntity>.Delete( attribute );
						GridViewDataBind( true );
				}
		}
}

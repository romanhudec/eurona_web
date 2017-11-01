using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;
using Telerik.Web.UI;
using Eurona.Common.DAL.Entities;

namespace Eurona.Controls
{
		public partial class FindAdvisorControl: CMS.Controls.CmsControl
		{
				private RadGrid dataGrid = null;
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						if ( !IsPostBack )
						{
								this.ddlRegion.Items.Clear();
								List<ListItem> items = new Eurona.Common.Controls.UserManagement.OrganizationControl.Hepler().GetRegions();
								foreach ( ListItem item in items )
										this.ddlRegion.Items.Add( new RadComboBoxItem( item.Text, item.Value ) );

								RadComboBoxItem itemEmpty = new RadComboBoxItem( string.Empty, string.Empty );
								this.ddlRegion.Items.Insert( 0, itemEmpty );
						}

						dataGrid = CreateGridView();
						this.gridContainer.Controls.Add( dataGrid );

						GetGridViewData( !IsPostBack );
				}
				/// <summary>
				/// Url kontaktneho formulara.
				/// </summary>
				public string ContactUrlFromat { get; set; }

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "FindAdvisorControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "FindAdvisorControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "FindAdvisorControl-SortExpression", "Name" ); }
						set { SetSession<string>( "FindAdvisorControl-SortExpression", value ); }
				}
				private RadGrid CreateGridView()
				{
						RadGrid grid = new RadGrid();
						CMS.Utilities.RadGridUtilities.Localize( grid );
						grid.MasterTableView.DataKeyNames = new string[] { "Id" };

						grid.AllowAutomaticInserts = false;
						grid.AllowFilteringByColumn = true;
						grid.ShowStatusBar = false;
						grid.ShowGroupPanel = false;
						grid.GroupingEnabled = false;
						grid.GroupingSettings.ShowUnGroupButton = false;
						grid.ClientSettings.AllowDragToGroup = false;
						grid.ClientSettings.AllowColumnsReorder = true;

						grid.MasterTableView.ShowHeader = true;
						grid.MasterTableView.ShowFooter = false;
						grid.MasterTableView.AllowPaging = true;
						grid.MasterTableView.PageSize = 50;
						grid.MasterTableView.PagerStyle.AlwaysVisible = true;
						grid.MasterTableView.AllowSorting = true;
						grid.MasterTableView.GridLines = GridLines.None;
						grid.MasterTableView.AutoGenerateColumns = false;
						grid.MasterTableView.ShowHeadersWhenNoRecords = false;

						grid.Columns.Add( new GridHyperLinkColumn
						{
								DataTextField = "ContactEmail",
								HeaderText = CMS.Resources.Controls.AdminAccountsControl_ColumnEmail,
								SortExpression = "ContactEmail",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains,
								//DataTextFormatString = "<a href=mailto:{0:g}>{0:g}</a>",
								DataNavigateUrlFields = new string[] { "AccountId" },
								DataNavigateUrlFormatString = Page.ResolveUrl( this.ContactUrlFromat + "&" + base.BuildReturnUrlQueryParam() ),
						} );

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Name",
								HeaderText = Resources.Strings.FindAdvisorControl_ColumnName,
								SortExpression = "Name",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );

						//grid.Columns.Add( new GridBoundColumn
						//{
						//    DataField = "RegisteredAddressString",
						//    HeaderText = Resources.Strings.FindAdvisorControl_ColumnAddress,
						//    SortExpression = "RegisteredAddressString",
						//    AutoPostBackOnFilter = true,
						//    CurrentFilterFunction = GridKnownFunction.Contains
						//} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "ContactMobile",
								HeaderText = Resources.Strings.FindAdvisorControl_ColumnMobile,
								SortExpression = "ContactMobile",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "ContactPhone",
								HeaderText = Resources.Strings.FindAdvisorControl_ColumnPhone,
								SortExpression = "ContactPhone",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						return grid;
				}

				private void GetGridViewData( bool bind )
				{
						List<Organization> list = new List<Organization>();
						if ( !string.IsNullOrEmpty( this.txtAdvisorName.Text ) || !string.IsNullOrEmpty( this.txtCity.Text ) || !string.IsNullOrEmpty( this.ddlRegion.SelectedValue ) )
						{
								string city = string.IsNullOrEmpty( txtCity.Text ) ? null : txtCity.Text;
								string advisorName = string.IsNullOrEmpty( txtAdvisorName.Text ) ? null : txtAdvisorName.Text;
								string regionCode = string.IsNullOrEmpty( ddlRegion.SelectedValue ) ? null : ddlRegion.SelectedValue;

								list = Storage<Organization>.Read( new Organization.ReadTOPForHost { City = city, Name = advisorName, RegionCode = regionCode } );
								//list = Storage<Organization>.Read( new Organization.ReadBy { Top=true, City = city, Name = advisorName, RegionCode = regionCode } );
						}
						
						var ordered = list.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
						this.dataGrid.DataSource = ordered.ToList();

						if( bind ) this.dataGrid.DataBind();
				}
				protected void OnFindAdvisor( object sender, EventArgs e )
				{
						GetGridViewData( true );
				}
		}
}
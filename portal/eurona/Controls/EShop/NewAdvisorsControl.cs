using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using CMS.Controls;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using CMS;
using System.Web.UI;
using Telerik.Web.UI;
using SHP.Controls;

namespace Eurona.Controls
{
		public class NewAdvisorsControl: CmsControl
		{

				private RadGrid gridView;
				public string EditUrlFormat { get; set; }
				public string UserUrlFormat { get; set; }

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "NewAdvisorsControl-SortDirection", SortDirection.Descending ); }
						set { SetSession<SortDirection>( "NewAdvisorsControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "NewAdvisorsControl-SortExpression", "Name" ); }
						set { SetSession<string>( "NewAdvisorsControl-SortExpression", value ); }
				}

				public int? ParentId
				{
						get { return GetState<int?>( "ParentId" ); }
						set { SetState<int?>( "ParentId", value ); }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						gridView = CreateGridView();
						this.Controls.Add( gridView );

						GridViewDataBind( !IsPostBack );
				}


				private OrganizationEntity.ReadBy GetFilterValue()
				{
						OrganizationEntity.ReadBy filter = new OrganizationEntity.ReadBy();

						if ( this.ParentId.HasValue )
								filter.ParentId = this.ParentId.Value;
						return filter;
				}

				private void GridViewDataBind( bool bind )
				{
						OrganizationEntity.ReadBy filter = GetFilterValue();
						List<OrganizationEntity> list = Storage<OrganizationEntity>.Read( filter );

						var ordered = list.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
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

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Name",
								HeaderText = Resources.Strings.NewAdvisorsControl_ColumnName,
								SortExpression = "Name",
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "RegisteredAddressString",
								HeaderText = Resources.Strings.NewAdvisorsControl_ColumnAddress,
								SortExpression = "RegisteredAddressString",
						} );
						grid.Columns.Add( new GridHyperLinkColumn
						{
								DataTextField = "ContactEmail",
								HeaderText = Resources.Strings.NewAdvisorsControl_ColumnEmail,
								SortExpression = "ContactEmail",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains,
								DataTextFormatString = "<a href=mailto:{0:g}>{0:g}</a>",
						} );
						grid.Columns[1].HeaderStyle.Width = Unit.Pixel( 120 );

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "ContactMobile",
								HeaderText = Resources.Strings.NewAdvisorsControl_ColumnMobile,
								SortExpression = "ContactMobile",
						} );

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "LeftToVerification",
								HeaderText = Resources.Strings.NewAdvisorsControl_ColumnLeftToVerification,
								SortExpression = "LeftToVerification",
						} );


						return grid;
				}
		}
}
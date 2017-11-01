using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using VocabularyEntity = CMS.Entities.Vocabulary;

namespace CMS.Controls.Translations
{
		public class AdminVocabulariesControl: CmsControl
		{
				private const string EDIT_TRANSLATIONS_COMMAND = "EDIT_TRANSLATIONS";
				private GridView gridView;
				public string EditTranslationsUrlFormat { get; set; }

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminVocabulariesControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminVocabulariesControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminVocabulariesControl-SortExpression", "Name" ); }
						set { SetSession<string>( "AdminVocabulariesControl-SortExpression", value ); }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						gridView = CreateGridView();
						gridView.PagerTemplate = null;

						this.Controls.Add( gridView );

						GridViewDataBind( !IsPostBack );
				}

				private void GridViewDataBind( bool bind )
				{
						List<VocabularyEntity> vocs = Storage<VocabularyEntity>.Read();

						var ordered = vocs.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
						gridView.DataSource = ordered.ToList();

						if ( bind ) gridView.DataBind();
				}

				private GridView CreateGridView()
				{
						GridView grid = new GridView();
						grid.DataKeyNames = new string[] { "Id" };

						grid.AllowPaging = true;
						grid.PageSize = 25;
						grid.PagerSettings.Mode = PagerButtons.NumericFirstLast;
						grid.PagerSettings.PageButtonCount = 10;

						grid.AllowSorting = true;
						grid.Sorting += OnSorting;

						grid.GridLines = GridLines.None;

						grid.CssClass = CssClass;
						grid.RowStyle.CssClass = CssClass + "_rowStyle";
						grid.FooterStyle.CssClass = CssClass + "_footerStyle";
						grid.PagerStyle.CssClass = CssClass + "_pagerStyle";
						grid.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
						grid.HeaderStyle.CssClass = CssClass + "_headerStyle";
						grid.EditRowStyle.CssClass = CssClass + "_editRowStyle";
						grid.AlternatingRowStyle.CssClass = CssClass + "_alternatingRowStyle";

						grid.AutoGenerateColumns = false;
						grid.Columns.Add( new BoundField
						{
								DataField = "Name",
								HeaderText = Resources.Controls.AdminVocabulariesControl_ColumnName,
								SortExpression = "Name",
						} );

						CMSButtonField btnEditTrans = new CMSButtonField();
						btnEditTrans.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEditTrans.ImageUrl = ConfigValue( "CMS:EditContentButtonImage" );
						btnEditTrans.ToolTip = Resources.Controls.GridView_ToolTip_EditItem;
						btnEditTrans.ButtonType = ButtonType.Image;
						btnEditTrans.CommandName = EDIT_TRANSLATIONS_COMMAND;
						grid.Columns.Add( btnEditTrans );

						grid.RowCommand += OnRowCommand;
						grid.RowDataBound += OnRowDataBound;

						return grid;
				}

				void OnRowDataBound( object sender, GridViewRowEventArgs e )
				{
				}

				void OnSorting( object sender, GridViewSortEventArgs e )
				{
						SortDirection = SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
						SortExpression = e.SortExpression;
						GridViewDataBind( true );
				}

				void OnRowCommand( object sender, GridViewCommandEventArgs e )
				{
						if ( e.CommandName == EDIT_TRANSLATIONS_COMMAND ) OnEditTranslationsCommand( sender, e );
				}

				private void OnEditTranslationsCommand( object sender, GridViewCommandEventArgs e )
				{
						int rowIndex = Convert.ToInt32( e.CommandArgument );
						int vocId = Convert.ToInt32( ( sender as GridView ).DataKeys[rowIndex].Value );
						Response.Redirect( String.Format( EditTranslationsUrlFormat, vocId ) + "&" + base.BuildReturnUrlQueryParam() );
				}
		}
}

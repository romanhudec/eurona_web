using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using TranslationEntity = CMS.Entities.Translation;

namespace CMS.Controls.Translations
{
		public class AdminTranslationsControl: CmsControl
		{
				private static class DataSource
				{
						public static List<TranslationEntity> GetTranslations( int vocabularyId )
						{
								return Storage<TranslationEntity>.Read( new TranslationEntity.ReadByVocabulary { VocabularyId = vocabularyId } );
						}
						public static void SetTranslation( int id, string trans, string notes )
						{
								TranslationEntity t = Storage<TranslationEntity>.ReadFirst( new TranslationEntity.ReadById { TranslationId = id } );
								if ( t == null ) return;
								t.Trans = trans;
								t.Notes = notes;
								Storage<TranslationEntity>.Update( t );
						}
				}

				private const string EDIT_COMMAND = "EDIT";
				private GridView gridView;

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminTranslationsControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminTranslationsControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminTranslationsControl-SortExpression", "Term" ); }
						set { SetSession<string>( "AdmintranslationsControl-SortExpression", value ); }
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
						int vocId = -1;
						if ( !Int32.TryParse( Request.QueryString["id"], out vocId ) ) return;

						/*
						List<TranslationEntity> vocs = Storage<TranslationEntity>.Read(new TranslationEntity.ReadByVocabulary { VocabularyId = vocId });
						var ordered = vocs.AsQueryable().OrderBy(SortExpression + " " + SortDirection);
						gridView.DataSource = ordered.ToList();
						*/

						ObjectDataSource ods = new ObjectDataSource()
						{
								ID = "ods1",
								TypeName = typeof( DataSource ).ToString(),
								SelectMethod = "GetTranslations",
								UpdateMethod = "SetTranslation"
						};
						ods.SelectParameters.Add( "vocabularyId", vocId.ToString() );
						Controls.Add( ods );
						gridView.DataSourceID = "ods1";

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
								DataField = "Term",
								HeaderText = Resources.Controls.AdminTranslationsControl_ColumnTerm,
								SortExpression = "Term",
								ReadOnly = true
						} );
						grid.Columns.Add( new BoundField
						{
								DataField = "Trans",
								HeaderText = Resources.Controls.AdminVocabulariesControl_ColumnTranslation,
								SortExpression = "Trans",
								ReadOnly = false
						} );
						grid.Columns.Add( new BoundField
						{
								DataField = "Notes",
								HeaderText = Resources.Controls.AdminTranslationsControl_ColumnNotes,
								SortExpression = "Notes"
						} );
						grid.Columns[0].HeaderStyle.Width = Unit.Pixel( 150 );
						grid.Columns[1].HeaderStyle.Width = Unit.Percentage( 60 );
						//grid.AutoGenerateEditButton = true;
						/*
						CMSButtonField btnEditTrans = new CMSButtonField();
						btnEditTrans.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
						btnEditTrans.ImageUrl = ConfigValue("CMS:EditContentButtonImage");
						btnEditTrans.ToolTip = Resources.Controls.GridView_ToolTip_EditItem;
						btnEditTrans.ButtonType = ButtonType.Image;
						btnEditTrans.CommandName = EDIT_COMMAND;
						grid.Columns.Add(btnEditTrans);
						 */
						CommandField editField = new CommandField()
						{
								EditImageUrl = ConfigValue( "CMS:EditButtonImage" ),
								CancelImageUrl = ConfigValue( "CMS:CloseButtonImage" ),
								UpdateImageUrl = ConfigValue( "CMS:OkButtonImage" ),
								ShowEditButton = true,
								ButtonType = ButtonType.Image
						};
						editField.HeaderStyle.Width = new Unit( 36, UnitType.Pixel );
						grid.Columns.Add( editField );

						grid.RowEditing += OnRowEditing;
						//grid.RowCommand += OnRowCommand;
						//grid.RowDataBound += OnRowDataBound;

						return grid;
				}

				void OnRowEditing( object sender, GridViewEditEventArgs e )
				{
						gridView.EditIndex = e.NewEditIndex;
						e.Cancel = false;
				}

				/*
				void OnRowDataBound(object sender, GridViewRowEventArgs e)
				{
				}
				*/

				void OnSorting( object sender, GridViewSortEventArgs e )
				{
						SortDirection = SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
						SortExpression = e.SortExpression;
						GridViewDataBind( true );
				}

				/*
				void OnRowCommand(object sender, GridViewCommandEventArgs e)
				{
					//if (e.CommandName == EDIT_TRANSLATIONS_COMMAND) OnEditTranslationsCommand(sender, e);
				}
				*/

				/*
				private void OnEditTranslationsCommand(object sender, GridViewCommandEventArgs e)
				{
					int rowIndex = Convert.ToInt32(e.CommandArgument);
					int vocId = Convert.ToInt32((sender as GridView).DataKeys[rowIndex].Value);
					Response.Redirect(String.Format(EditTranslationsUrlFormat, vocId) + "&" + base.BuildReturnUrlQueryParam());
				}
				*/
		}
}

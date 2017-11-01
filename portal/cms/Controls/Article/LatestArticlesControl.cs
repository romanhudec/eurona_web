using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ArticleEntity = CMS.Entities.Article;
using System.Web.UI;
using System.Data;
using System.ComponentModel;

namespace CMS.Controls.Article
{
		// do not show the StatsData class in the Toolbox...
		[ToolboxItem( false )]
		public class LatestArticlesData: WebControl, INamingContainer
		{
				private ArticleEntity article;

				internal LatestArticlesData(string tag, LatestArticlesControl owner, ArticleEntity article )
						: base( tag )
				{
						this.article = article;
						this.CssClass = owner.CssClass + "_container";
				}

				[Bindable( true )]
				public int Id
				{
						get { return this.article.Id; }
				}

				[Bindable( true )]
				public DateTime? ReleaseDate
				{
						get { return this.article.ReleaseDate; }
				}

				[Bindable( true )]
				public string Title
				{
						get { return this.article.Title; }
				}

				[Bindable( true )]
				public string Teaser
				{
						get { return this.article.Teaser; }
				}

				[Bindable( true )]
				public string Alias
				{
						get { return this.article.Alias; }
				}
		}

		public class LatestArticlesControl: CmsControl, INamingContainer
		{
				public int MaxItemsCount { get; set; }
				public string DisplayUrlFormat { get; set; }
				private string tag = "div";
				public string Tag { get { return tag; } set { tag = value; } }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "The top-article template." )]
				[TemplateContainer( typeof( LatestArticlesData ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate LatestArticlesTemplate{ get; set;}

				// For composite controls, the Controls collection needs to be overriden to
				// ensure that the child controls have been created before the Controls
				// property can be modified by the page developer...
				public override ControlCollection Controls
				{
						get
						{
								this.EnsureChildControls();
								return base.Controls;
						}
				}

				protected override void CreateChildControls()
				{
						Controls.Clear();		// clear out the control hierarchy

						ITemplate template = this.LatestArticlesTemplate;
						if ( template == null ) template = new DefaultLatestArticlesTemplate( this );

						List<ArticleEntity> list = Storage<ArticleEntity>.Read( new ArticleEntity.ReadLatest { Count = MaxItemsCount <= 0 ? 3 : MaxItemsCount } );
						if ( list.Count == 0 )
						{
								this.Visible = false;
								return;
						}

						foreach ( ArticleEntity article in list )
						{
								LatestArticlesData data = new LatestArticlesData( tag, this, article );
								template.InstantiateIn( data );
								Controls.Add( data );
						}

						this.DataBind();
				}

				public override void DataBind()
				{
						this.ChildControlsCreated = true;
						base.DataBind();
				}

				internal sealed class DefaultLatestArticlesTemplate: ITemplate
				{
						public string CssClass { get; set; }
						private string DisplayAliasUrlFormat { get; set; }
						public string DisplayUrlFormat { get; set; }

						public DefaultLatestArticlesTemplate( LatestArticlesControl owner )
						{
								this.CssClass = owner.CssClass;
								this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
								this.DisplayUrlFormat = owner.DisplayUrlFormat;
						}

						void ITemplate.InstantiateIn( Control container )
						{

								Table table = new Table();
								table.CssClass = this.CssClass + "_article";

								TableRow row = new TableRow();
								TableCell cell = new TableCell();
								cell.Text = "» ";
								row.Cells.Add( cell );

								cell = new TableCell();
								HyperLink hlHead = new HyperLink();
								hlHead.DataBinding += ( sender, e ) =>
								{
										LatestArticlesData data = (LatestArticlesData)hlHead.NamingContainer;
										HyperLink control = sender as HyperLink;
										control.Text = data.Title;

										if ( string.IsNullOrEmpty( data.Alias ) && !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
												control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayUrlFormat, data.Id ) );
										else control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayAliasUrlFormat, data.Alias ) );

								};
								hlHead.CssClass = this.CssClass + "_head";
								cell.Controls.Add( hlHead );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								container.Controls.Add( table );
						}

				}

		}
}

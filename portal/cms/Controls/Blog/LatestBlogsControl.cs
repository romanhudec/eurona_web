using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using BlogEntity = CMS.Entities.Blog;
using System.Web.UI;
using System.Data;
using System.ComponentModel;

namespace CMS.Controls.Blog
{
		// do not show the StatsData class in the Toolbox...
		[ToolboxItem( false )]
		public class LatestBlogsData: WebControl, INamingContainer
		{
				private BlogEntity blog;

				internal LatestBlogsData( string tag, LatestBlogsControl owner, BlogEntity blog )
						: base( tag )
				{
						this.blog = blog;
						this.CssClass = owner.CssClass + "_container";
				}

				[Bindable( true )]
				public int Id
				{
						get { return this.blog.Id; }
				}

				[Bindable( true )]
				public DateTime? ReleaseDate
				{
						get { return this.blog.ReleaseDate; }
				}

				[Bindable( true )]
				public string Title
				{
						get { return this.blog.Title; }
				}

				[Bindable( true )]
				public string Teaser
				{
						get { return this.blog.Teaser; }
				}

				[Bindable( true )]
				public string Alias
				{
						get { return this.blog.Alias; }
				}
		}

		public class LatestBlogsControl: CmsControl, INamingContainer
		{
				public int MaxItemsCount { get; set; }
				public string DisplayUrlFormat { get; set; }
				private string tag = "div";
				public string Tag { get { return tag; } set { tag = value; } }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "The top-blog template." )]
				[TemplateContainer( typeof( LatestBlogsData ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate LatestBlogsTemplate{ get; set;}

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

						ITemplate template = this.LatestBlogsTemplate;
						if ( template == null ) template = new DefaultLatestBlogsTemplate( this );

						List<BlogEntity> list = Storage<BlogEntity>.Read( new BlogEntity.ReadLatest { Count = MaxItemsCount <= 0 ? 3 : MaxItemsCount } );
						if ( list.Count == 0 )
						{
								this.Visible = false;
								return;
						}

						foreach ( BlogEntity blog in list )
						{
								LatestBlogsData data = new LatestBlogsData( tag, this, blog );
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

				internal sealed class DefaultLatestBlogsTemplate: ITemplate
				{
						public string CssClass { get; set; }
						private string DisplayAliasUrlFormat { get; set; }
						public string DisplayUrlFormat { get; set; }

						public DefaultLatestBlogsTemplate( LatestBlogsControl owner )
						{
								this.CssClass = owner.CssClass;
								this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
								this.DisplayUrlFormat = owner.DisplayUrlFormat;
						}

						void ITemplate.InstantiateIn( Control container )
						{
								Table table = new Table();
								table.CssClass = this.CssClass + "_blog";

								TableRow row = new TableRow();
								TableCell cell = new TableCell();
								cell.Text = "» ";
								row.Cells.Add( cell );

								cell = new TableCell();
								HyperLink hlHead = new HyperLink();
								hlHead.DataBinding += ( sender, e ) =>
								{
										LatestBlogsData data = (LatestBlogsData)hlHead.NamingContainer;
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

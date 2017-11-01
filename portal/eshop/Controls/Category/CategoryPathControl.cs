using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using CategoryEntity = SHP.Entities.Category;
using System.Web.UI.WebControls;

namespace SHP.Controls.Category
{
		public class CategoryPathControl: WebControl
		{
				public CategoryPathControl() :
						base( "div" )
				{
				}

				public int? CategoryId { get; set; }
				public string NavigateUrlFormat { get; set; }

				public string Path
				{
						get
						{
								string path = string.Empty;
								if ( this.CategoryId.HasValue )
										path = GetCategoriesPath( this.CategoryId.Value, path );

								if ( path.StartsWith( "/" ) ) path = path.Remove( 0, 1 );

								return path;
						}
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						if ( this.CategoryId.HasValue )
								FillParentCategory( this.CategoryId.Value );

						if ( string.IsNullOrEmpty( this.NavigateUrlFormat ) )
						{
								LiteralControl lc = new LiteralControl( "Kategórie" );
								this.Controls.AddAt( 0, lc );
						}
						else
						{
								string url = Page.ResolveUrl( string.Format( this.NavigateUrlFormat, string.Empty ) );
								LiteralControl link = new LiteralControl( string.Format( "<a href='{0}'>{1}</a>", url, "Kategórie" ) );
								this.Controls.AddAt( 0, link );
						}
				}
				private void FillParentCategory( int rootId )
				{
						CategoryEntity parent = Storage<CategoryEntity>.ReadFirst( new CategoryEntity.ReadById { CategoryId = rootId } );
						if ( parent != null )
						{
								if ( string.IsNullOrEmpty( this.NavigateUrlFormat ) )
								{
										LiteralControl lc = new LiteralControl( parent.Name );
										this.Controls.AddAt( 0, lc );
										this.Controls.AddAt( 0, new LiteralControl( "/" ) );
								}
								else
								{
										string url = this.Page.ResolveUrl( String.Format( this.NavigateUrlFormat, parent.Id ) );
										LiteralControl link = new LiteralControl( string.Format( "<a href='{0}'>{1}</a>", url, parent.Name ) );
										this.Controls.AddAt( 0, link );
										this.Controls.AddAt( 0, new LiteralControl( "/" ) );
								}

								if ( !parent.ParentId.HasValue )
										return;

								FillParentCategory( parent.ParentId.Value );
						}
				}

				private string GetCategoriesPath( int rootId, string path )
				{
						CategoryEntity parent = Storage<CategoryEntity>.ReadFirst( new CategoryEntity.ReadById { CategoryId = rootId } );
						if ( parent != null )
						{
								LiteralControl lc = new LiteralControl( parent.Name );
								path = string.Format( "/{0}{1}", parent.Name, path );
								if ( !parent.ParentId.HasValue )
										return path;

								return GetCategoriesPath( parent.ParentId.Value, path );
						}

						return path;
				}
		}
}

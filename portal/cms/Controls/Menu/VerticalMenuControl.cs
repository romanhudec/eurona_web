using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MenuEntity = CMS.Entities.NavigationMenu;
using PageEntity = CMS.Entities.Page;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace CMS.Controls.Menu
{
		public class VerticalMenuControl: CmsControl
		{
				private HtmlGenericControl div;
				private List<Control> customMenuItems = null;

				public VerticalMenuControl()
				{
						this.div = new HtmlGenericControl( "div" );
						this.customMenuItems = new List<Control>();
				}

				/// <summary>
				/// Kod menu, podla ktoreho sa nacita prislusne navigacne menu.
				/// </summary>
				public string Code { get; set; }

				public string CssClassSelected { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						List<MenuEntity> menu = Storage<MenuEntity>.Read( new MenuEntity.ReadForCurrentAccount { Code = Code } );

						if ( !String.IsNullOrEmpty( CssClass ) ) div.Attributes.Add( "class", CssClass );
						menu.ForEach( m => CreateMenuItem( this.div.Controls, m ) );
						this.customMenuItems.ForEach( m => this.div.Controls.Add( m ) );
						this.Controls.Add( this.div );
				}

				private void CreateMenuItem( ControlCollection controls, MenuEntity m )
				{
						HyperLink hl = new HyperLink();
						hl.Text = m.Name;
						hl.NavigateUrl = this.Page.ResolveUrl( m.Alias );
						if ( !string.IsNullOrEmpty( m.Icon ) )
						{
								Image img = new Image();
								img.ImageUrl = this.Page.ResolveUrl(m.Icon);
								hl.Controls.Add( img );
						}

						controls.Add( hl );
				}

				public HyperLink AddMenuItem( string itemName, string navigateUrl )
				{
						HyperLink hl = new HyperLink();
						hl.Text = itemName;
						hl.NavigateUrl = this.Page.ResolveUrl( navigateUrl );

						this.customMenuItems.Add( hl );
						return hl;
				}

				public LinkButton AddMenuItem( string itemName )
				{
						LinkButton lb = new LinkButton();
						lb.Text = itemName;

						this.customMenuItems.Add( lb );
						return lb;
				}

				public virtual void SelectMenuItem( string url )
				{
						EnsureChildControls();
						if( string.IsNullOrEmpty( this.CssClassSelected ) ) return;
						foreach ( Control ctrl in this.div.Controls )
						{
								if ( !( ctrl is HyperLink ) ) continue;
								if ( ( ctrl as HyperLink ).NavigateUrl == url )
								{
										( ctrl as HyperLink ).CssClass = this.CssClassSelected;
								}
						}
				}
		}
}

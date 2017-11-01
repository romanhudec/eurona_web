using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CMS.Controls
{
		public class ContentEditorToolbarButton: UserControl
		{
				public event EventHandler Click;
				public ContentEditorToolbarButton( string text, string imageUrl, string navigateUrl )
				{
						this.Text = text;
						this.ImageUrl = imageUrl;
						this.NavigateUrl = navigateUrl;
				}

				public string Text { get; set; }
				public string ImageUrl { get; set; }
				public string NavigateUrl { get; set; }
				public string OnClientClick { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						if ( string.IsNullOrEmpty( this.NavigateUrl ) && string.IsNullOrEmpty( this.ImageUrl ) )
						{
								Button button = new Button();
								button.CausesValidation = false;
								button.Text = this.Text;
								button.Click += ( s, e ) => { if ( Click != null ) Click( s, e ); };
								button.OnClientClick = this.OnClientClick;
								Controls.Add( button );

								return;
						}

						if ( !string.IsNullOrEmpty( this.ImageUrl ) )
						{
								WebControl ctrl = null;
								if ( string.IsNullOrEmpty( this.NavigateUrl ) )
								{
										LinkButton lb = new LinkButton();
										lb.CausesValidation = false;
										if ( string.IsNullOrEmpty( this.OnClientClick ) )
												lb.Click += ( s, e ) => { if ( Click != null ) Click( s, e ); };
										lb.OnClientClick = this.OnClientClick;
										ctrl = lb;
								}
								else
								{
										HyperLink hl = new HyperLink();
										hl.NavigateUrl = this.NavigateUrl;
										ctrl = hl;
								}

								if ( ctrl == null ) return;

								ctrl.Style.Add( "display", "block" );
								ctrl.Style.Add( "text-decoration", "none" );
								ctrl.Style.Add( "color", "inherit" );

								Image img = new Image();
								img.Style.Add( "display", "block" );
								img.ImageUrl = this.ImageUrl;

								ctrl.Controls.Add( img );
								ctrl.Controls.Add( new LiteralControl( this.Text ) );
								this.Controls.Add( ctrl );
						}

				}
		}
}

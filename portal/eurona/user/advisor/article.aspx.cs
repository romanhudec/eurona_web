using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User.Advisor
{
		public partial class ArticlePage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
								this.articleControl.ArticleId = Convert.ToInt32( this.Request["id"] );

						//Nastavenie spravnej navratovej url adresy 
						// je to koli tomu, ze ak je galeria zobrazena z filtrovaneho zoznamu,
						// spet sa musi vratit na filtrovany zoznam, nie na vsetky galerie.
						if ( !string.IsNullOrEmpty( this.articleControl.ReturnUrl ) )
								this.returnUrl.HRef = this.articleControl.ReturnUrl;
				}
		}
}

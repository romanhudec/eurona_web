using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona.User.Advisor
{
		public partial class ArticleCommentsPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
						{
								this.articleCommentsControl.ArticleId = Convert.ToInt32( this.Request["id"] );
								this.articleCommentsControl.CommentFormID = this.articleCommentFormControl.ClientID;
								this.articleCommentFormControl.ArticleId = Convert.ToInt32( this.Request["id"] );
								this.articleCommentFormControl.RedirectUrl = this.Request.RawUrl;

								//Nastavenie spravnej navratovej url adresy 
								// je to koli tomu, ze ak je galeria zobrazena z filtrovaneho zoznamu,
								// spet sa musi vratit na filtrovany zoznam, nie na vsetky galerie.
								if ( !string.IsNullOrEmpty( this.articleCommentFormControl.ReturnUrl ) )
										this.returnUrl.HRef = this.articleCommentFormControl.ReturnUrl;
						}
				}
		}
}

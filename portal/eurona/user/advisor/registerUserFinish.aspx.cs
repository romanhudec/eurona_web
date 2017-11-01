using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CMS.Utilities;
using Eurona.DAL.Entities;
using Eurona.Common.Controls.Cart;
using Eurona.Common.DAL.Entities;

namespace Eurona.User.Advisor
{
		public partial class RegisterUserFinish: WebPage
		{
				private int accountId = -2;
				private int AccountId
				{
						get
						{
								if ( accountId != -2 ) return accountId;
								string id = Server.UrlDecode( Request["token"] );
								id = Cryptographer.Decrypt( id );
								accountId = -1;
								Int32.TryParse( id, out accountId );
								return accountId;
						}
				}

				protected void Page_Load( object sender, EventArgs e )
				{
						this.accountId = -2;
				}

				protected void OnGoToMyOffice( object sender, EventArgs e )
				{
						AliasUtilities aliasUtils = new AliasUtilities();
						string aliasOffice = aliasUtils.Resolve( "~/user/advisor/default.aspx", this.Page );
						string aliasOfficeCart = aliasUtils.Resolve( "~/user/advisor/cart.aspx", this.Page );

						Account account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = this.AccountId } );
						if ( account != null && account.Enabled && account.Authenticate( account.Password ) )
								Security.Login( account, false );

						Cart cart = EuronaCartHelper.GetAccountCart( this );
						if ( cart != null &&  cart.CartProductsCount != 0 ) Response.Redirect( aliasOfficeCart );
						else Response.Redirect( aliasOffice );
				}
		}
}

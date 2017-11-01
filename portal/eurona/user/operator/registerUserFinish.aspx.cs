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

namespace Eurona.User.Operator
{
		public partial class RegisterUserFinish: WebPage
		{
				private int accountId = -2;
				protected void Page_Load( object sender, EventArgs e )
				{
						this.accountId = Convert.ToInt32( Request["id"]);
						Account account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = this.accountId } );

						this.lblRegisterInfo.Text = string.Format( "Gratulujeme, registrace poradce {0} je úspěšne ukončena! ", account.Login );
				}
		}
}

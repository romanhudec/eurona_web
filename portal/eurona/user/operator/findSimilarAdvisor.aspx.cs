using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AccountEntity = Eurona.DAL.Entities.AdvisorAccount;

namespace Eurona.Operator
{
		public partial class FindSimilarAdvisor: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if( string.IsNullOrEmpty(Request["Id"]) ) return;
						int accountId = Convert.ToInt32( Request["Id"]);

						AccountEntity account = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadById { AccountId = accountId } );
						if ( account == null ) return;

						this.lblRegisterDate.Text = account.Created.ToShortDateString();
						this.lblCode.Text = account.AdvisorCode;
						this.lblCorrespondenceAddress.Text = account.CorrespondenceAddress;
						this.lblRegisteredAddress.Text = account.RegisteredAddress;

						this.lblEmail.Text = account.Email;
						this.lblName.Text = account.Name;
						this.lblPhone.Text = account.Phone;
						this.lblMobile.Text = account.Mobile;
				}
		}
}

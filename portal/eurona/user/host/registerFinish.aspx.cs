using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CMS.Utilities;
using CMS.Entities;
using CMS;

namespace Eurona.User.Host
{
		public partial class RegisterFinish: System.Web.UI.Page
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

				}

				//protected void OnRequestClick( object sender, EventArgs e )
				//{
				//    Account account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = AccountId } );
				//    if ( account == null )
				//    {
				//        Response.Redirect( "~/right.aspx" );
				//        return;
				//    }

				//    string root = Utilities.Root( Request );
				//    string urlUser = root + "user/verify.aspx";
				//    string urlCentral = String.Format( "{0}admin/account.aspx?id={1}&ReturnUrl=/default.aspx", root, account.Id );
				//    EmailNotification email2User = new EmailNotification
				//    {
				//        To = account.Email,
				//        Subject = Resources.Strings.UserVerifyRegistrationPage_Email2User_Subject,
				//        Message = String.Format( Resources.Strings.UserVerifyRegistrationPage_Email2User_Message, urlUser ).Replace( "\\n", Environment.NewLine )
				//    };
				//    EmailNotification email2Central = new EmailNotification
				//    {
				//        To = ConfigurationManager.AppSettings["SMTP:CentralInbox"],
				//        Subject = Resources.Strings.UserVerifyRegistrationPage_Email2Central_Subject,
				//        Message = String.Format( Resources.Strings.UserVerifyRegistrationPage_Email2Central_Message, urlCentral, account.Login ).Replace( "\\n", Environment.NewLine )
				//    };
				//    bool okUser = email2User.Notify();
				//    bool okCentral = email2Central.Notify();
				//    if ( okUser && okCentral )
				//        Response.Redirect( "~/user/verify.aspx?requested=1" );
				//}
		}
}

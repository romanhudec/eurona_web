using System;
using System.Configuration;
using CMS.Utilities;
using Eurona.DAL.Entities;
using CMS;
using OrganizationEntity = CMS.Entities.Organization;

namespace Eurona.User.Host
{
		public partial class RegisterOrganization: System.Web.UI.Page
		{
				private int accountId = -2;
				private int AccountId
				{
						get
						{
								if ( accountId != -2 ) return accountId;
								string id = Server.UrlDecode( Request["token"].Replace( "+", "%2b" ) );
								id = Cryptographer.Decrypt( id );
								accountId = -1;
								Int32.TryParse( id, out accountId );
								return accountId;
						}
				}

				protected void Page_Load( object sender, EventArgs e )
				{
						//Ak uz osoba existuje, nebude ju mozne zmenit!!
						OrganizationEntity organization = Storage<OrganizationEntity>.ReadFirst( new OrganizationEntity.ReadByAccountId { AccountId = this.AccountId } );
						if ( organization != null )
						{
								organizationControl.IsEditing = false;
								return;
						}

						organizationControl.SaveCompleted += SaveCompeted;
						organizationControl.Canceled += Canceled;

						if ( IsPostBack ) return;

						organizationControl.AccountId = AccountId;
						organizationControl.Visible = AccountId > 0;

						#region Organization Settings
						this.organizationControl.Settings = new CMS.Controls.UserManagement.OrganizationControl.ControlSettings();
						this.organizationControl.Settings.Visibility.Id3 = false;

						//Address settings
						this.organizationControl.Settings.CorrespondenceAddressSettings.Visibility.Country = false;
						this.organizationControl.Settings.CorrespondenceAddressSettings.Require.City = true;
						this.organizationControl.Settings.CorrespondenceAddressSettings.Require.Street = true;
						this.organizationControl.Settings.CorrespondenceAddressSettings.Require.Zip = true;
						this.organizationControl.Settings.InvoicingAddressSettings = this.organizationControl.Settings.CorrespondenceAddressSettings;
						this.organizationControl.Settings.RegisteredAddressSettings = this.organizationControl.Settings.CorrespondenceAddressSettings;

						//Bank contact
						this.organizationControl.Settings.BankContactSettings.Require.BankCode = true;
						this.organizationControl.Settings.BankContactSettings.Require.AccountNumber = true;

						//Contact person
						this.organizationControl.Settings.ContactPersonSettings.Require.FirstName = true;
						this.organizationControl.Settings.ContactPersonSettings.Require.LastName = true;
						this.organizationControl.Settings.ContactPersonSettings.Require.Email = true;
						this.organizationControl.Settings.ContactPersonSettings.Require.Mobile = true;
						this.organizationControl.Settings.ContactPersonSettings.Visibility.HomeAddress = false;
						this.organizationControl.Settings.ContactPersonSettings.Visibility.TempAddress = false;
						#endregion


				}

				protected void Canceled( object sender, EventArgs args )
				{
						Account account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = AccountId } );
						if ( account != null ) Storage<Account>.Delete( account );
						Response.Redirect( "~/default.aspx" );
				}

				protected void SaveCompeted( object sender, EventArgs args )
				{
						Account account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = AccountId } );
						account.Enabled = false;
						account.AddToRoles( Role.REGISTEREDUSER, Role.HOST );
						Storage<Account>.Update( account );

						//Odoslanie Emailu registrovanemu zakaznikovi a administratorovi
						SendRegostrationEmail( account );

						Response.Redirect( String.Format( "~/user/host/registerFinish.aspx?token={0}", Request["token"] ) );
				}

				/// <summary>
				/// Odoslanie informacneho mailu o registracii pouzivatela
				/// </summary>
				private bool SendRegostrationEmail( Account customerAccount )
				{
						string root = Utilities.Root( Request );
						string urlUser = root + "login.aspx";
						string urlCentral = String.Format( "{0}admin/account.aspx?id={1}&ReturnUrl=/default.aspx", root, customerAccount.Id );
						EmailNotification email2User = new EmailNotification
						{
								To = customerAccount.Email,
								Subject = Resources.Strings.UserRegistrationPage_Email2User_Subject,
								Message = String.Format( Resources.Strings.UserRegistrationPage_Email2User_Message, urlUser, customerAccount.Login ).Replace( "\\n", Environment.NewLine )
						};
						EmailNotification email2Central = new EmailNotification
						{
								To = ConfigurationManager.AppSettings["SMTP:CentralInbox"],
								Subject = Resources.Strings.UserRegistrationPage_Email2Central_Subject,
								Message = String.Format( Resources.Strings.UserRegistrationPage_Email2Central_Message, urlCentral, customerAccount.Login ).Replace( "\\n", Environment.NewLine )
						};
						bool okUser = email2User.Notify(true);
						bool okCentral = email2Central.Notify();

						return okUser && okCentral;
				}

		}
}

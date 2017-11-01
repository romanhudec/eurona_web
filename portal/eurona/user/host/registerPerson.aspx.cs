using System;
using System.Configuration;
using CMS.Utilities;
using Eurona.DAL.Entities;
using CMS;
using PersonEntity = CMS.Entities.Person;

namespace Eurona.User.Host
{
		public partial class RegisterPerson: System.Web.UI.Page
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
						PersonEntity person = Storage<PersonEntity>.ReadFirst( new PersonEntity.ReadByAccountId { AccountId = this.AccountId } );
						if ( person != null )
						{
								personControl.IsEditing = false;
								return;
						}

						personControl.SaveCompleted += SaveCompeted;
						personControl.Canceled += Canceled;

						if ( IsPostBack ) return;

						personControl.AccountId = AccountId;
						personControl.Visible = AccountId > 0;

						#region Person Settings
						this.personControl.Settings = new CMS.Controls.UserManagement.PersonControl.ControlSettings();
						this.personControl.Settings.Require.FirstName = true;
						this.personControl.Settings.Require.LastName = true;
						this.personControl.Settings.Require.Email = true;
						this.personControl.Settings.Require.Mobile = true;
						//Address settings
						this.personControl.Settings.HomeAddressSettings.Require.City = true;
						this.personControl.Settings.HomeAddressSettings.Require.Street = true;
						this.personControl.Settings.HomeAddressSettings.Require.Zip = true;
						this.personControl.Settings.HomeAddressSettings.Require.District = true;
						this.personControl.Settings.HomeAddressSettings.Require.Region = true;
						this.personControl.Settings.HomeAddressSettings.Visibility.State = false;
						this.personControl.Settings.HomeAddressSettings.Visibility.Country = false;
						this.personControl.Settings.HomeAddressSettings.Visibility.District = true;
						this.personControl.Settings.HomeAddressSettings.Visibility.Region = true;

						//this.personControl.Settings.TempAddressSettings = this.personControl.Settings.HomeAddressSettings;
						this.personControl.Settings.Visibility.TempAddress = false;
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

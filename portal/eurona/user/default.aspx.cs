using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;
using CMS;

namespace Eurona.User
{
		public partial class DefaultPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						this.personControl.Visible = false;
						this.organizationControl.Visible = false;

						Person person = Storage<Person>.ReadFirst( new Person.ReadByAccountId { AccountId = Security.Account.Id } );
						if ( person != null )
						{
								this.Title = Resources.Strings.UserPage_PersonDetail;
								this.personControl.Visible = true;
								this.personControl.PersonId = person.Id;

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

						Organization organization = Storage<Organization>.ReadFirst( new Organization.ReadByAccountId { AccountId = Security.Account.Id } );
						if ( organization != null )
						{
								this.Title = Resources.Strings.UserPage_OrganizationDetail;
								int dealerType = ( organization.Id1 == "-1" && organization.Id2 == "-1" && organization.Id3 == "-1" && organization.Name == string.Empty ) ? 2 : 1;

								this.organizationControl.Visible = true;
								this.organizationControl.OrganizationId = organization.Id;

								#region Organization Settings
								//Address settings
								this.organizationControl.Settings = new CMS.Controls.UserManagement.OrganizationControl.ControlSettings();
								this.organizationControl.Settings.Visibility.Id3 = false;

								if ( dealerType == 2 )/*Sukromna osoba*/
								{
										this.organizationControl.Settings.Visibility.Id1 = false;
										this.organizationControl.Settings.Visibility.Id2 = false;
										this.organizationControl.Settings.Visibility.Id3 = false;
										this.organizationControl.Settings.Visibility.Name = false;
										this.organizationControl.Settings.Visibility.Notes = false;
										this.organizationControl.Settings.Visibility.Web = false;
										this.organizationControl.Settings.Visibility.ContactEmail = false;
										this.organizationControl.Settings.Visibility.ContactPhone = false;
										this.organizationControl.Settings.Visibility.ContactMobil = false;
										this.organizationControl.Settings.Visibility.RegisteredAddress = false;
								}
								this.organizationControl.Settings.CorrespondenceAddressSettings.Visibility.Country = false;
								this.organizationControl.Settings.CorrespondenceAddressSettings.Require.City = true;
								this.organizationControl.Settings.CorrespondenceAddressSettings.Require.Street = true;
								this.organizationControl.Settings.CorrespondenceAddressSettings.Require.Zip = true;
								this.organizationControl.Settings.CorrespondenceAddressSettings.Require.Region = true;
								this.organizationControl.Settings.CorrespondenceAddressSettings.Require.District = true;
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

						this.rpPerson.Visible = this.personControl.Visible;
						this.rpOrganization.Visible = this.organizationControl.Visible;
				}
		}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CMS.Entities;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace CMS.Controls.UserManagement
{
		public class OrganizationControl: CmsControl
		{
				#region Private members
				private TextBox txtId1 = null;
				private TextBox txtId2 = null;
				private TextBox txtId3 = null;

				private TextBox txtName = null;
				private TextBox txtWeb = null;
				private TextBox txtNotes = null;

				private TextBox txtContactEmail = null;
				private TextBox txtContactPhone = null;
				private TextBox txtContactMobil = null;

				private PersonControl contactPerson = null;

				private AddressControl registeredAddress = null;
				private CheckBox cbCorrespondenceAddressAsRegistered = null;
				private AddressControl correspondenceAddress = null;
				private CheckBox cbInvoicingAddressAsRegistered = null;
				private AddressControl invoicingAddress = null;
				private BankContactControl bankContact = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private Organization organization = null;
				private bool isNew = false;

				#endregion

				public string CancelButtonText { get; set; }
				public string SaveButtonText { get; set; }

				public EventHandler SaveCompleted = null;
				public EventHandler Canceled = null;

				public OrganizationControl()
				{
				}

				public ControlSettings Settings
				{
						get
						{
								object o = ViewState["Settings"];
								return o != null ? (ControlSettings)o : null;
						}
						set { ViewState["Settings"] = value; }
				}

				#region Public properties
				/// <summary>
				/// Property je nutne naplnat pri zobrazovani a editacii organizacie.
				/// </summary>
				public int? OrganizationId
				{
						get
						{
								object o = ViewState["OrganizationId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["OrganizationId"] = value; }
				}

				/// <summary>
				/// Property je nutne naplnat pri vytvarani novej organizacie
				/// ak su to osobne udaje registrovaneho pouzivatela.
				/// </summary>
				public int? AccountId
				{
						get
						{
								object o = ViewState["AccountId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["AccountId"] = value; }
				}


				/// <summary>
				/// Extended mod pre zadavanie telefonneho cisla podla locale daneho pouzivatela.
				/// </summary>
				public bool ExtendedPhoneMode
				{
						get
						{
								object o = ViewState["ExtendedPhoneMode"];
								return o != null ? (bool)Convert.ToBoolean( o ) : false;
						}
						set { ViewState["ExtendedPhoneMode"] = value; }
				}
				#endregion

				#region Protected overrides
				protected override void CreateChildControls()
				{
						//View State je povoleny iba v editacnom rezime
						this.EnableViewState = this.IsEditing;

						//Vytvorenie settings ak neexistuje
						if ( this.Settings == null )
								this.Settings = new ControlSettings();

						base.CreateChildControls();

						Control detailControl = CreateDetailControl();
						if ( detailControl != null )
								if ( detailControl != null )
								{
										this.Controls.Add( detailControl );
								}

						//Binding
						this.isNew = this.OrganizationId == null;
						if ( !this.OrganizationId.HasValue )
								this.organization = new Organization();
						else
								this.organization = Storage<Organization>.ReadFirst( new Organization.ReadById { OrganizationId = this.OrganizationId.Value } );

						if ( !IsPostBack )
						{
								this.txtId1.Text = this.organization.Id1;
								this.txtId2.Text = this.organization.Id2;
								this.txtId3.Text = this.organization.Id3;

								this.txtName.Text = this.organization.Name;
								this.txtWeb.Text = this.organization.Web;
								this.txtNotes.Text = this.organization.Notes;

								this.txtContactEmail.Text = this.organization.ContactEmail;
								this.txtContactMobil.Text = this.organization.ContactMobile;
								this.txtContactPhone.Text = this.organization.ContactPhone;
								
								this.registeredAddress.AddressId = this.organization.RegisteredAddressId;
								this.correspondenceAddress.AddressId = this.organization.CorrespondenceAddressId;
								this.invoicingAddress.AddressId = this.organization.InvoicingAddressId;
								this.bankContact.BankContactId = this.organization.BankContactId;

								this.contactPerson.PersonId = this.organization.ContactPersonId;

								#region Disable Send button and js validation
								StringBuilder sb = new StringBuilder();
								sb.Append( "if (typeof(Page_ClientValidate) == 'function') { " );
								sb.Append( "var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;" );
								sb.Append( "if (Page_ClientValidate('" + btnSave.ValidationGroup + "') == false) {" );
								sb.Append( " Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} " );
								//change button text and disable it
								sb.AppendFormat( "this.value = '{0}...';", this.btnSave.Text );
								sb.Append( "this.disabled = true;" );
								sb.Append( Page.ClientScript.GetPostBackEventReference( this.btnSave, null ) + ";" );
								sb.Append( "return true;" );

								string submit_button_onclick_js = sb.ToString();
								btnSave.Attributes.Add( "onclick", submit_button_onclick_js );
								#endregion

								this.DataBind();
						}

				}
				#endregion

				#region Public Styles Properties
				public string CssRoundPanel { get; set; }
				#endregion

				#region Private methods & handlers
				/// <summary>
				/// Vytvori Control Adresy
				/// </summary>
				private Control CreateDetailControl()
				{
						this.txtId1 = new TextBox();
						this.txtId1.ID = "txtId1";
						this.txtId1.Width = Unit.Pixel( 100 );

						this.txtId2 = new TextBox();
						this.txtId2.ID = "txtId2";
						this.txtId2.Width = Unit.Pixel( 100 );

						this.txtId3 = new TextBox();
						this.txtId3.ID = "txtId3";
						this.txtId3.Width = Unit.Pixel( 100 );

						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						this.txtName.Width = Unit.Pixel( 200 );

						this.txtWeb = new TextBox();
						this.txtWeb.ID = "txtWeb";
						this.txtWeb.Width = Unit.Pixel( 200 );

						this.txtNotes = new TextBox();
						this.txtNotes.ID = "txtNotes";
						this.txtNotes.Width = Unit.Percentage( 100 );
						
						this.txtContactEmail = new TextBox();
						this.txtContactEmail.ID = "txtContactEmail";
						this.txtContactEmail.Width = Unit.Pixel( 100 );

						this.txtContactPhone = new TextBox();
						this.txtContactPhone.ID = "txtNotes";
						this.txtContactPhone.Width = Unit.Percentage( 100 );

						this.txtContactMobil = new TextBox();
						this.txtContactMobil.ID = "txtContactMobil";
						this.txtContactMobil.Width = Unit.Pixel( 100 );
						
						this.registeredAddress = new AddressControl();
						this.registeredAddress.ID = "registeredAddress";
						this.registeredAddress.IsEditing = this.IsEditing;
						this.registeredAddress.Settings = this.Settings.RegisteredAddressSettings;

						this.cbCorrespondenceAddressAsRegistered = new CheckBox();
						this.cbCorrespondenceAddressAsRegistered.ID = "cbCorrespondenceAddressAsRegistered";
						this.cbCorrespondenceAddressAsRegistered.Text = Resources.Controls.OrganizationControl_EqualWithRegisteredAdress;
						this.cbCorrespondenceAddressAsRegistered.AutoPostBack = true;
						this.cbCorrespondenceAddressAsRegistered.Font.Bold = true;
						this.cbCorrespondenceAddressAsRegistered.CheckedChanged += new EventHandler( cbCorrespondenceAddressAsRegistered_CheckedChanged );
						this.cbCorrespondenceAddressAsRegistered.CssClass = "checkBox";
						this.cbCorrespondenceAddressAsRegistered.CausesValidation = false;

						this.correspondenceAddress = new AddressControl();
						this.correspondenceAddress.ID = "correspondenceAddress";
						this.correspondenceAddress.IsEditing = this.IsEditing;
						this.correspondenceAddress.Settings = this.Settings.CorrespondenceAddressSettings;

						this.cbInvoicingAddressAsRegistered = new CheckBox();
						this.cbInvoicingAddressAsRegistered.ID = "cbInvoicingAddressAsRegistered";
						this.cbInvoicingAddressAsRegistered.Text = Resources.Controls.OrganizationControl_EqualWithRegisteredAdress;
						this.cbInvoicingAddressAsRegistered.AutoPostBack = true;
						this.cbInvoicingAddressAsRegistered.Font.Bold = true;
						this.cbInvoicingAddressAsRegistered.CheckedChanged += new EventHandler( cbInvoicingAddressAsRegistered_CheckedChanged );
						this.cbInvoicingAddressAsRegistered.CssClass = "checkBox";
						this.cbInvoicingAddressAsRegistered.CausesValidation = false;

						this.invoicingAddress = new AddressControl();
						this.invoicingAddress.ID = "invoicingAddress";
						this.invoicingAddress.IsEditing = this.IsEditing;
						this.invoicingAddress.Settings = this.Settings.InvoicingAddressSettings;

						this.bankContact = new BankContactControl();
						this.bankContact.ID = "bankContact";
						this.bankContact.IsEditing = this.IsEditing;
						this.bankContact.Settings = this.Settings.BankContactSettings;

						this.contactPerson = new PersonControl();
						this.contactPerson.ID = "contactPerson";
						this.contactPerson.CssRoundPanel = this.CssRoundPanel;
						this.contactPerson.IsEditing = this.IsEditing;
						this.contactPerson.HideSaveCancel = true;
						this.contactPerson.Settings = this.Settings.ContactPersonSettings;
						this.contactPerson.ExtendedPhoneMode = this.ExtendedPhoneMode;


						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = String.IsNullOrEmpty( SaveButtonText ) ? Resources.Controls.SaveButton_Text : SaveButtonText;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = String.IsNullOrEmpty( CancelButtonText ) ? Resources.Controls.CancelButton_Text : CancelButtonText;
						this.btnCancel.Click += new EventHandler( OnCancel );

						Table mainTable = new Table();
						mainTable.Width = this.Width;
						mainTable.Height = this.Height;

						#region Row 1
						Table table = new Table();
						table.Width = Unit.Percentage( 100 );
						//Id1
						TableRow row = null;
						if ( this.Settings.Visibility.Id1 )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.OrganizationControl_Id1, this.txtId1, 0, this.Settings.Require.Id1 );
								table.Rows.Add( row );
						}

						//Id2
						if ( this.Settings.Visibility.Id2 )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.OrganizationControl_Id2, this.txtId2, 0, this.Settings.Require.Id2 );
								table.Rows.Add( row );
						}

						//Id3
						if ( this.Settings.Visibility.Id3 )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.OrganizationControl_Id3, this.txtId3, 0, this.Settings.Require.Id3 );
								table.Rows.Add( row );
						}

						//Name
						if ( this.Settings.Visibility.Name )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.OrganizationControl_Name, this.txtName, 0, this.Settings.Require.Name );
								table.Rows.Add( row );
						}

						//Web
						if ( this.Settings.Visibility.Web )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.OrganizationControl_Web, this.txtWeb, 0, this.Settings.Require.Web );
								table.Rows.Add( row );
						}

						//Notes
						if ( this.Settings.Visibility.Notes )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.OrganizationControl_Notes, this.txtNotes, 0, this.Settings.Require.Notes );
								table.Rows.Add( row );
						}


						//Registered Address
						if ( this.Settings.Visibility.RegisteredAddress )
						{
								row = new TableRow();
								TableCell cell = new TableCell();
								cell.Width = Unit.Percentage( 50 );
								cell.Controls.Add( table );
								row.Cells.Add( cell );

								//Registered Address
								cell = new TableCell();
								cell.Width = Unit.Percentage( 50 );
								cell.Controls.Add( CreateRoundPanel( Resources.Controls.OrganizationControl_RegisteredAddressTitle,
										new Control[] { this.registeredAddress }, false ) );
								row.Cells.Add( cell );

								mainTable.Rows.Add( row );
						}
						#endregion

						#region Row 2
						row = new TableRow();

						//Contact person
						if ( this.Settings.Visibility.ContactPerson )
						{
								TableCell cell = new TableCell();
								cell.VerticalAlign = VerticalAlign.Top;
								cell.Controls.Add( CreateRoundPanel( Resources.Controls.OrganizationControl_ContactPersonTitle,
										new Control[] { this.contactPerson }, true ) );
								row.Cells.Add( cell );
						}
						//Correspondence Address
						if ( this.Settings.Visibility.CorrespondenceAddress )
						{
								TableCell cell = new TableCell();
								cell.VerticalAlign = VerticalAlign.Top;
								cell.Controls.Add( CreateRoundPanel( Resources.Controls.OrganizationControl_CorrespondenceAddressTitle,
										new Control[] { this.IsEditing && this.Settings.Visibility.RegisteredAddress ? this.cbCorrespondenceAddressAsRegistered : null, this.correspondenceAddress },
										false ) );
								row.Cells.Add( cell );
						}

						mainTable.Rows.Add( row );
						#endregion

						#region Row 3
						row = new TableRow();
						//Invoicing Address
						if ( this.Settings.Visibility.InvoicingAddress )
						{
								TableCell cell = new TableCell();
								cell.Controls.Add( CreateRoundPanel( Resources.Controls.OrganizationControl_InvoicingAddressTitle,
										new Control[] { this.IsEditing && this.Settings.Visibility.RegisteredAddress ? this.cbInvoicingAddressAsRegistered : null, this.invoicingAddress },
										false ) );
								row.Cells.Add( cell );
						}

						//Bank contact
						if ( this.Settings.Visibility.BankContact )
						{
								TableCell cell = new TableCell();
								cell.VerticalAlign = VerticalAlign.Top;
								cell.Controls.Add( CreateRoundPanel( Resources.Controls.OrganizationControl_BankContactTitle,
										new Control[] { this.bankContact }, true ) );
								row.Cells.Add( cell );
						}
						if ( row.Cells.Count != 0 ) mainTable.Rows.Add( row );
						#endregion
						
						if ( this.IsEditing )
						{
								//Save Cancel Buttons
								row = new TableRow();
								TableCell cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.Controls.Add( this.btnSave );
								cell.Controls.Add( this.btnCancel );
								row.Cells.Add( cell );
								mainTable.Rows.Add( row );
						}

						return mainTable;
				}

				private void AddControlToRow( TableRow row, string labelText, Control control, int controlColspan, bool required )
				{
						( control as WebControl ).Enabled = this.IsEditing;

						TableCell cell = new TableCell();
						cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						if ( controlColspan != 0 )
								cell.ColumnSpan = controlColspan;
						if ( required && this.IsEditing ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						row.Cells.Add( cell );
				}

				private RoundPanel CreateRoundPanel( string title, Control[] controls, bool fillHeader )
				{
						RoundPanel rp = new RoundPanel();
						rp.CssClass = this.CssRoundPanel;
						rp.Text = title;
						rp.FillHeader = fillHeader;

						foreach ( Control control in controls )
						{
								if ( control == null ) continue;
								rp.Controls.Add( control );
						}
						return rp;
				}

				void cbCorrespondenceAddressAsRegistered_CheckedChanged( object sender, EventArgs e )
				{
						if ( this.cbCorrespondenceAddressAsRegistered.Checked )
						{
								Address current = new Address();
								current = this.registeredAddress.UpdateEntityFromUI( current );
								this.correspondenceAddress.UpdateUIFromEntity( current );
						}
				}

				void cbInvoicingAddressAsRegistered_CheckedChanged( object sender, EventArgs e )
				{
						if ( this.cbInvoicingAddressAsRegistered.Checked )
						{
								Address current = new Address();
								current = this.registeredAddress.UpdateEntityFromUI( current );
								this.invoicingAddress.UpdateUIFromEntity( current );
						}
				}
				#endregion

				#region Public methods
				private void UpdateEntityFromUI( Organization organization )
				{
						organization.Id1 = this.txtId1.Text;
						organization.Id2 = this.txtId2.Text;
						organization.Id3 = this.txtId3.Text;

						organization.Name = this.txtName.Text;
						organization.Web = this.txtWeb.Text;
						organization.Notes = this.txtNotes.Text;

						organization.ContactEmail = this.txtContactEmail.Text;
						organization.ContactPhone = this.txtContactPhone.Text;
						organization.ContactMobile = this.txtContactMobil.Text;
				}

				public Organization UpdateOrganization( Organization organization )
				{
						UpdateEntityFromUI( organization );
						Storage<Organization>.Update( organization );

						if ( this.Settings.Visibility.RegisteredAddress )
								this.registeredAddress.UpdateAddress( organization.RegisteredAddress );

						if ( this.Settings.Visibility.CorrespondenceAddress )
								this.correspondenceAddress.UpdateAddress( organization.CorrespondenceAddress );

						if ( this.Settings.Visibility.InvoicingAddress )
								this.invoicingAddress.UpdateAddress( organization.InvoicingAddress );

						if ( this.Settings.Visibility.BankContact )
								this.bankContact.UpdateBankContact( organization.BankContact );

						if ( this.Settings.Visibility.ContactPerson )
								this.contactPerson.UpdatePerson( organization.ContactPerson );

						return organization;
				}

				public Organization CreateOrganization( Organization organization )
				{
						if ( this.AccountId.HasValue )
								organization.AccountId = this.AccountId.Value;

						UpdateEntityFromUI( organization );
						Storage<Organization>.Create( organization );

						if ( this.Settings.Visibility.RegisteredAddress )
								this.registeredAddress.UpdateAddress( organization.RegisteredAddress );

						if ( this.Settings.Visibility.CorrespondenceAddress )
								this.correspondenceAddress.UpdateAddress( organization.CorrespondenceAddress );

						if ( this.Settings.Visibility.InvoicingAddress )
								this.invoicingAddress.UpdateAddress( organization.InvoicingAddress );

						if ( this.Settings.Visibility.BankContact )
								this.bankContact.UpdateBankContact( organization.BankContact );

						if ( this.Settings.Visibility.ContactPerson )
								this.contactPerson.UpdatePerson( organization.ContactPerson );

						return organization;
				}
				#endregion

				#region Event handlers
				void OnSave( object sender, EventArgs e )
				{
						if ( this.isNew ) CreateOrganization( this.organization );
						else UpdateOrganization( this.organization );

						if ( SaveCompleted != null )
								SaveCompleted( this, EventArgs.Empty );

						if ( !string.IsNullOrEmpty( this.ReturnUrl ) )
								Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						if ( Canceled != null )
								Canceled( this, EventArgs.Empty );

						if ( !string.IsNullOrEmpty( this.ReturnUrl ) )
								Response.Redirect( this.ReturnUrl );
				}
				#endregion

				#region Settings classes
				[Serializable()]
				public class ControlSettings
				{
						public ControlSettings()
						{
								this.Visibility = new Visibility();
								this.Require = new Require();

								this.RegisteredAddressSettings = new AddressControl.ControlSettings();
								this.CorrespondenceAddressSettings = new AddressControl.ControlSettings();
								this.InvoicingAddressSettings = new AddressControl.ControlSettings();
								this.BankContactSettings = new BankContactControl.ControlSettings();
								this.ContactPersonSettings = new PersonControl.ControlSettings();
						}

						public Visibility Visibility { get; set; }
						public Require Require { get; set; }
						public AddressControl.ControlSettings RegisteredAddressSettings { get; set; }
						public AddressControl.ControlSettings CorrespondenceAddressSettings { get; set; }
						public AddressControl.ControlSettings InvoicingAddressSettings { get; set; }
						public BankContactControl.ControlSettings BankContactSettings { get; set; }
						public PersonControl.ControlSettings ContactPersonSettings { get; set; }

				}

				[Serializable()]
				public class Visibility
				{
						public Visibility()
						{
								bool defaultValue = true;
								this.Id1 = defaultValue;
								this.Id2 = defaultValue;
								this.Id3 = defaultValue;
								this.Name = defaultValue;
								this.Web = defaultValue;
								this.Notes = defaultValue;
								this.ContactEmail = defaultValue;
								this.ContactPhone = defaultValue;
								this.ContactMobil = defaultValue;

								this.RegisteredAddress = defaultValue;
								this.CorrespondenceAddress = defaultValue;
								this.InvoicingAddress = defaultValue;
								this.BankContact = defaultValue;
								this.ContactPerson = defaultValue;
						}

						public bool Id1 { get; set; }
						public bool Id2 { get; set; }
						public bool Id3 { get; set; }
						public bool Name { get; set; }
						public bool Web { get; set; }
						public bool Notes { get; set; }
						public bool ContactEmail { get; set; }
						public bool ContactPhone { get; set; }
						public bool ContactMobil { get; set; }

						public bool RegisteredAddress { get; set; }
						public bool CorrespondenceAddress { get; set; }
						public bool InvoicingAddress { get; set; }
						public bool BankContact { get; set; }
						public bool ContactPerson { get; set; }
				}

				[Serializable()]
				public class Require
				{
						public Require()
						{
								bool defaultValue = false;
								this.Id1 = defaultValue;
								this.Id2 = defaultValue;
								this.Id3 = defaultValue;
								this.Web = defaultValue;
								this.Name = true;
								this.Notes = defaultValue;
								this.ContactEmail = defaultValue;
								this.ContactPhone = defaultValue;
								this.ContactMobil = defaultValue;
						}

						public bool Id1 { get; set; }
						public bool Id2 { get; set; }
						public bool Id3 { get; set; }
						public bool Name { get; set; }
						public bool Web { get; set; }
						public bool Notes { get; set; }
						public bool ContactEmail { get; set; }
						public bool ContactPhone { get; set; }
						public bool ContactMobil { get; set; }
				}

				#endregion
		}
}

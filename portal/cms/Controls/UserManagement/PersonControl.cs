using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CMS.Entities;
using System.Web.UI.WebControls;
using System.Threading;

namespace CMS.Controls.UserManagement
{
		public class PersonControl: CmsControl
		{
				#region Private members
				private TextBox txtTitle = null;
				private TextBox txtFirstName = null;
				private TextBox txtLastName = null;
				private TextBox txtEmail = null;
				private TextBox txtNotes = null;
				private DropDownList ddlIPNFPhone = null;
				private TextBox txtPhone = null;
				private DropDownList ddlIPNFMobile = null;
				private TextBox txtMobile = null;
				private AddressControl homeAddress = null;
				private CheckBox cbTempAddressAsHomeAdress = null;
				private AddressControl tempAddress = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private Person person = null;
				private bool isNew = false;

				public string CancelButtonText { get; set; }
				public string SaveButtonText { get; set; }

				public EventHandler SaveCompleted = null;
				public EventHandler Canceled = null;
				#endregion

				public PersonControl()
				{
						HideSaveCancel = false;
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
				/// Property je nutne naplnat pri zobrazovani a editacii osoby.
				/// </summary>
				public int? PersonId
				{
						get
						{
								object o = ViewState["PersonId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["PersonId"] = value; }
				}

				/// <summary>
				/// Property je nutne naplnat pri vytvarani novej osoby
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

				public bool HideSaveCancel { get; set; }
				#endregion

				#region Public Styles Properties
				public string CssRoundPanel { get; set; }
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

						//Get data
						this.isNew = this.PersonId == null;
						if ( !this.PersonId.HasValue )
						{
								this.person = new Person();
								if ( this.AccountId.HasValue ) this.person.AccountId = this.AccountId.Value;
						}
						else this.person = Storage<Person>.ReadFirst( new Person.ReadById { PersonId = this.PersonId.Value } );

						Control detailControl = CreateDetailControl();
						if ( detailControl != null )
						{
								this.Controls.Add( detailControl );
						}

						//Binding
						if ( !IsPostBack )
						{
								this.txtTitle.Text = this.person.Title;
								this.txtFirstName.Text = this.person.FirstName;
								this.txtLastName.Text = this.person.LastName;
								this.txtEmail.Text = this.isNew && this.person.AccountId.HasValue ? this.person.Account.Email : this.person.Email;
								this.txtNotes.Text = this.person.Notes;

								this.txtPhone.Text = this.person.Phone;
								this.txtMobile.Text = this.person.Mobile;

								if ( this.ExtendedPhoneMode && this.IsEditing )
								{
										string locale = ( this.isNew || !this.person.AccountId.HasValue ) ? Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName : this.person.Account.Locale;

										//Telefonne cisla su vo romate
										//+421 (905) XXX XXX ...

										List<IPNF> phones = Storage<IPNF>.Read( new IPNF.ReadByType { Type = IPNF.IPNFType.Phone, Locale = locale } );
										this.ddlIPNFPhone.DataSource = phones;
										this.ddlIPNFPhone.DataValueField = "Id";
										this.ddlIPNFPhone.DataTextField = "IPF";
										this.ddlIPNFPhone.DataBind();

										List<IPNF> mobiles = Storage<IPNF>.Read( new IPNF.ReadByType { Type = IPNF.IPNFType.Mobile, Locale = locale } );
										this.ddlIPNFMobile.DataSource = mobiles;
										this.ddlIPNFMobile.DataValueField = "Id";
										this.ddlIPNFMobile.DataTextField = "IPF";
										this.ddlIPNFMobile.DataBind();

										if ( !string.IsNullOrEmpty( this.person.Phone ) )
										{
												IPNF ipnf = phones.Find( x => this.person.Phone.IndexOf( x.IPF ) != -1 );
												if ( ipnf != null )
												{
														this.ddlIPNFPhone.SelectedValue = ipnf.Id.ToString();
														this.txtPhone.Text = this.person.Phone.Remove( 0, ipnf.IPF.Length );
												}
												else
														this.txtPhone.Text = this.person.Phone;
										}

										if ( !string.IsNullOrEmpty( this.person.Mobile ) )
										{
												IPNF ipnf = mobiles.Find( x => this.person.Mobile.IndexOf( x.IPF ) != -1 );
												if ( ipnf != null )
												{
														this.ddlIPNFMobile.SelectedValue = ipnf.Id.ToString();
														this.txtMobile.Text = this.person.Mobile.Remove( 0, ipnf.IPF.Length );
												}
												else
														this.txtMobile.Text = this.person.Mobile;
										}

								}

								this.homeAddress.AddressId = this.person.AddressHomeId;
								this.tempAddress.AddressId = this.person.AddressTempId;

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

				#region Private methods & handlers
				/// <summary>
				/// Vytvori Control Adresy
				/// </summary>
				private Control CreateDetailControl()
				{
						this.txtTitle = new TextBox();
						this.txtTitle.ID = "txtTitle";
						this.txtTitle.Width = Unit.Pixel( 100 );

						this.txtFirstName = new TextBox();
						this.txtFirstName.ID = "txtFirstName";
						this.txtFirstName.Width = Unit.Pixel( 200 );

						this.txtLastName = new TextBox();
						this.txtLastName.ID = "txtLastName";
						this.txtLastName.Width = Unit.Pixel( 200 );

						this.txtEmail = new TextBox();
						this.txtEmail.ID = "txtEmail";
						this.txtEmail.Width = Unit.Pixel( 200 );

						this.ddlIPNFPhone = new DropDownList();
						this.ddlIPNFPhone.ID = "ddlIPNFPhone";
						this.ddlIPNFPhone.Width = Unit.Pixel( 100 );

						this.txtPhone = new TextBox();
						this.txtPhone.ID = "txtPhone";

						this.ddlIPNFMobile = new DropDownList();
						this.ddlIPNFMobile.ID = "ddlIPNFMobile";
						this.ddlIPNFMobile.Width = Unit.Pixel( 100 );

						this.txtMobile = new TextBox();
						this.txtMobile.ID = "txtMobile";

						this.txtNotes = new TextBox();
						this.txtNotes.ID = "txtNotes";
						this.txtNotes.Width = Unit.Pixel( 200 );

						this.homeAddress = new AddressControl();
						this.homeAddress.ID = "homeAddress";
						this.homeAddress.IsEditing = this.IsEditing;
						this.homeAddress.Settings = this.Settings.HomeAddressSettings;

						this.cbTempAddressAsHomeAdress = new CheckBox();
						this.cbTempAddressAsHomeAdress.ID = "cbTempAddressAsHomeAdress";
						this.cbTempAddressAsHomeAdress.Text = Resources.Controls.PersonControl_EqualWithHomeAdress;
						this.cbTempAddressAsHomeAdress.AutoPostBack = true;
						this.cbTempAddressAsHomeAdress.Font.Bold = true;
						this.cbTempAddressAsHomeAdress.CheckedChanged += new EventHandler( cbTempAddressAsHomeAdress_CheckedChanged );
						this.cbTempAddressAsHomeAdress.CssClass = "checkBox";

						this.tempAddress = new AddressControl();
						this.tempAddress.ID = "tempAddress";
						this.tempAddress.IsEditing = this.IsEditing;
						this.tempAddress.Settings = this.Settings.TempAddressSettings;

						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = String.IsNullOrEmpty( SaveButtonText ) ? Resources.Controls.SaveButton_Text : SaveButtonText;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = String.IsNullOrEmpty( CancelButtonText ) ? Resources.Controls.CancelButton_Text : CancelButtonText;
						this.btnCancel.Click += new EventHandler( OnCancel );

						Table table = new Table();
						table.Width = this.Width;
						table.Height = this.Height;

						TableRow row = null;

						//Title
						if ( this.Settings.Visibility.Title )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.PersonControl_Title, this.txtTitle, 0, this.Settings.Require.Title, null );
								table.Rows.Add( row );
						}

						//FirstName
						if ( this.Settings.Visibility.FirstName )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.PersonControl_FirstName, this.txtFirstName, 0, this.Settings.Require.FirstName, null );
								table.Rows.Add( row );
						}

						//LastName
						if ( this.Settings.Visibility.LastName )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.PersonControl_LastName, this.txtLastName, 0, this.Settings.Require.LastName, null );
								table.Rows.Add( row );
						}

						//Mobile
						if ( this.Settings.Visibility.Mobile )
						{
								row = new TableRow();
								if ( this.ExtendedPhoneMode )
										AddPhoneControlsToRow( row, Resources.Controls.PersonControl_Mobile, new Control[] { this.ddlIPNFMobile, this.txtMobile }, 0, this.Settings.Require.Mobile, this.Settings.Validation.Mobile );
								else
										AddControlToRow( row, Resources.Controls.PersonControl_Mobile, this.txtMobile, 0, this.Settings.Require.Mobile, this.Settings.Validation.Mobile );
								table.Rows.Add( row );
						}

						//Phone
						if ( this.Settings.Visibility.Phone )
						{
								row = new TableRow();
								if ( this.ExtendedPhoneMode )
										AddPhoneControlsToRow( row, Resources.Controls.PersonControl_Phone, new Control[] { this.ddlIPNFPhone, this.txtPhone }, 0, this.Settings.Require.Phone, this.Settings.Validation.Phone );
								else
										AddControlToRow( row, Resources.Controls.PersonControl_Phone, this.txtPhone, 0, this.Settings.Require.Phone, this.Settings.Validation.Phone );
								table.Rows.Add( row );
						}

						//Notes
						if ( this.Settings.Visibility.Notes )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.PersonControl_Notes, this.txtNotes, 0, this.Settings.Require.Notes, null );
								table.Rows.Add( row );
						}

						//Email
						if ( this.Settings.Visibility.Email )
						{
								row = new TableRow();
								if ( IsEditing ) AddControlToRow( row, Resources.Controls.PersonControl_Email, this.txtEmail, 0, this.Settings.Require.Email, null );
								else AddControlToRow( row, Resources.Controls.PersonControl_Email, new LiteralControl( GetEmail( this.person ) ), 0, this.Settings.Require.Email, null );
								table.Rows.Add( row );
								//Ak je person registrovany pouzivatel Email nemoze menit.
								if ( this.person.AccountId.HasValue )
										this.txtEmail.Enabled = false;
						}

						RoundPanel rp = null;
						//Home Address
						if ( this.Settings.Visibility.HomeAddress )
						{
								rp = new RoundPanel();
								rp.CssClass = this.CssRoundPanel;
								rp.FillHeader = false;
								if ( string.IsNullOrEmpty( this.Settings.HomeAddressSettings.Title ) )
										rp.Text = Resources.Controls.PersonControl_HomeAddress;
								else
										rp.Text = this.Settings.HomeAddressSettings.Title;
								rp.Controls.Add( this.homeAddress );

								row = new TableRow();
								TableCell cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.Controls.Add( rp );
								row.Cells.Add( cell );
								table.Rows.Add( row );
						}

						if ( this.Settings.Visibility.TempAddress )
						{
								//Temp Address
								rp = new RoundPanel();
								rp.CssClass = this.CssRoundPanel;
								rp.FillHeader = false;
								if ( string.IsNullOrEmpty( this.Settings.TempAddressSettings.Title ) )
										rp.Text = Resources.Controls.PersonControl_TempAddress;
								else
										rp.Text = this.Settings.TempAddressSettings.Title;

								if ( this.IsEditing ) rp.Controls.Add( this.cbTempAddressAsHomeAdress );
								rp.Controls.Add( this.tempAddress );

								row = new TableRow();
								TableCell cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.Controls.Add( rp );
								row.Cells.Add( cell );
								table.Rows.Add( row );
						}

						if ( this.IsEditing && !this.HideSaveCancel )
						{
								//Save Cancel Buttons
								row = new TableRow();
								TableCell cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.Controls.Add( this.btnSave );
								cell.Controls.Add( this.btnCancel );
								row.Cells.Add( cell );
								table.Rows.Add( row );
						}

						return table;
				}

				void cbTempAddressAsHomeAdress_CheckedChanged( object sender, EventArgs e )
				{
						if ( this.cbTempAddressAsHomeAdress.Checked )
						{
								Address currentHomeAddress = new Address();
								currentHomeAddress = this.homeAddress.UpdateEntityFromUI( currentHomeAddress );
								this.tempAddress.UpdateUIFromEntity( currentHomeAddress );
						}
				}

				private void AddControlToRow( TableRow row, string labelText, Control control, int controlColspan, bool required, RegExpValidator validator )
				{
						if ( control is WebControl )
								( control as WebControl ).Enabled = this.IsEditing;

						TableCell cell = new TableCell();
						cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						cell.ColumnSpan = controlColspan;
						if ( required && this.IsEditing ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						if ( validator != null && this.IsEditing )
						{
								validator.ControlToValidate = control.ID;
								cell.Controls.Add(  base.CreateRegularExpressionValidatorControl(validator) );
						}
						row.Cells.Add( cell );
				}

				private void AddPhoneControlsToRow( TableRow row, string labelText, Control[] controls, int controlColspan, bool required, RegExpValidator validator )
				{

						TableCell cell = new TableCell();
						cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						foreach ( Control control in controls )
						{
								if ( control is WebControl )
										( control as WebControl ).Enabled = this.IsEditing;

								cell.Controls.Add( control );
								cell.ColumnSpan = controlColspan;

								if ( control != controls[controls.Length - 1] )
										cell.Controls.Add( new LiteralControl( "-" ) );

						}
						if ( required && this.IsEditing ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( controls[controls.Length - 1].ID ) );
						if ( validator == null && this.IsEditing )
								cell.Controls.Add( base.CreatePhoneValidatorControl( controls[controls.Length - 1].ID ) );
						else if ( validator != null && this.IsEditing )
						{
								validator.ControlToValidate = controls[controls.Length - 1].ID;
								cell.Controls.Add( base.CreateRegularExpressionValidatorControl(validator) );
						}
						row.Cells.Add( cell );

				}

				private string GetEmail( Person person )
				{
						if ( this.isNew && person.AccountId.HasValue )
								return string.Format( "<a href=mailto:{0:g}>{0:g}</a>", person.Account.Email );
						else
								return string.Format( "<a href=mailto:{0:g}>{0:g}</a>", person.Email );
				}
				#endregion

				#region Public methods
				private void UpdateEntityFromUI( Person person )
				{
						person.Title = this.txtTitle.Text;
						person.Notes = this.txtNotes.Text;
						person.FirstName = this.txtFirstName.Text;
						person.LastName = this.txtLastName.Text;
						person.Email = this.txtEmail.Text;

						person.Phone = this.txtPhone.Text;
						person.Mobile = this.txtMobile.Text;

						if ( this.ExtendedPhoneMode )
						{
								if ( this.ddlIPNFMobile.SelectedIndex != -1 )
										person.Mobile = string.Format( "{0} {1}", this.ddlIPNFMobile.SelectedItem.Text, this.txtMobile.Text.Trim() );

								if ( this.ddlIPNFPhone.SelectedIndex != -1 )
										person.Phone = string.Format( "{0} {1}", this.ddlIPNFPhone.SelectedItem.Text, this.txtPhone.Text.Trim() );
						}

				}

				public Person UpdatePerson( Person person )
				{
						UpdateEntityFromUI( person );
						Storage<Person>.Update( person );

						if ( this.Settings.Visibility.HomeAddress )
								this.homeAddress.UpdateAddress( person.AddressHome );

						if ( this.Settings.Visibility.TempAddress )
								this.tempAddress.UpdateAddress( person.AddressTemp );

						return person;
				}

				public Person CreatePerson( Person person )
				{
						if ( this.AccountId.HasValue )
								person.AccountId = this.AccountId.Value;

						UpdateEntityFromUI( person );
						Storage<Person>.Create( person );

						if ( this.Settings.Visibility.HomeAddress )
								this.homeAddress.UpdateAddress( person.AddressHome );

						if ( this.Settings.Visibility.TempAddress )
								this.tempAddress.UpdateAddress( person.AddressTemp );

						return person;
				}
				#endregion

				#region Event handlers
				void OnSave( object sender, EventArgs e )
				{
						if ( this.isNew ) CreatePerson( this.person );
						else UpdatePerson( this.person );

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
								this.Validation = new Validation();
								this.HomeAddressSettings = new AddressControl.ControlSettings();
								this.TempAddressSettings = new AddressControl.ControlSettings();
						}

						public Visibility Visibility { get; set; }
						public Require Require { get; set; }
						public Validation Validation { get; set; }
						public AddressControl.ControlSettings HomeAddressSettings { get; set; }
						public AddressControl.ControlSettings TempAddressSettings { get; set; }
				}

				[Serializable()]
				public class Visibility
				{
						public Visibility()
						{
								bool defaultValue = true;
								this.Title = defaultValue;
								this.FirstName = defaultValue;
								this.LastName = defaultValue;
								this.Email = defaultValue;
								this.Notes = defaultValue;
								this.Phone = defaultValue;
								this.Mobile = defaultValue;
								this.HomeAddress = defaultValue;
								this.TempAddress = defaultValue;
						}

						public bool Title { get; set; }
						public bool FirstName { get; set; }
						public bool LastName { get; set; }
						public bool Email { get; set; }
						public bool Notes { get; set; }
						public bool Phone { get; set; }
						public bool Mobile { get; set; }
						public bool HomeAddress { get; set; }
						public bool TempAddress { get; set; }
				}

				[Serializable()]
				public class Require
				{
						public Require()
						{
								bool defaultValue = false;
								this.Title = defaultValue;
								this.FirstName = defaultValue;
								this.LastName = defaultValue;
								this.Email = defaultValue;
								this.Notes = defaultValue;
								this.Phone = defaultValue;
								this.Mobile = defaultValue;
						}

						public bool Title { get; set; }
						public bool FirstName { get; set; }
						public bool LastName { get; set; }
						public bool Email { get; set; }
						public bool Notes { get; set; }
						public bool Phone { get; set; }
						public bool Mobile { get; set; }
				}

				[Serializable()]
				public class Validation
				{
						public Validation()
						{
								RegExpValidator defaultValue = null;
								this.Phone = defaultValue;
								this.Mobile = defaultValue;
						}

						public RegExpValidator Phone { get; set; }
						public RegExpValidator Mobile { get; set; }
				}
				#endregion
		}
}

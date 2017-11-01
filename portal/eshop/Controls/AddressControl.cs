using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShpAddress = SHP.Entities.Address;
using CMS.Controls;

namespace SHP.Controls
{
		public class AddressControl: CmsControl
		{
				#region Private mebers
				private TextBox txtFirstName = null;
				private TextBox txtLastName = null;
				private TextBox txtOrganization = null;
				private TextBox txtPhone = null;
				private TextBox txtEmail = null;
				private TextBox txtId1 = null;
				private TextBox txtId2 = null;
				private TextBox txtId3 = null;

				private TextBox txtCity = null;
				private TextBox txtState = null;
				private TextBox txtZip = null;
				private TextBox txtStreet = null;
				private TextBox txtNotes = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private ShpAddress address = null;
				private bool isNew = false;
				#endregion

				public AddressControl()
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

				public int? AddressId
				{
						get
						{
								object o = ViewState["AddressId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["AddressId"] = value; }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						//View State je povoleny iba v editacnom rezime
						this.EnableViewState = this.IsEditing;

						//Vytvorenie settings ak neexistuje
						if ( this.Settings == null  )
								this.Settings = new ControlSettings();
						else if( this.Settings.Validation.Email == null )
						{
								this.Settings.Validation.Email = new RegExpValidator();
								this.Settings.Validation.Email.CssClass = "ms-formvalidation";
								this.Settings.Validation.Email.EnableClientScript = true;
								this.Settings.Validation.Email.ErrorMessage = "!";
								this.Settings.Validation.Email.SetFocusOnError = true;
								this.Settings.Validation.Email.ValidationExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
						}

						base.CreateChildControls();

						Control detailControl = CreateDetailControl();
						if ( detailControl != null )
								this.Controls.Add( detailControl );

						//Binding
						this.isNew = this.AddressId == null;
						if ( !this.AddressId.HasValue ) this.address = new ShpAddress();
						else this.address = Storage<ShpAddress>.ReadFirst( new ShpAddress.ReadById { AddressId = this.AddressId.Value } );

						if ( !IsPostBack )
						{
								UpdateUIFromEntity( this.address );
								this.DataBind();
						}

				}
				#endregion

				/// <summary>
				/// Vytvori Control Adresy
				/// </summary>
				private Control CreateDetailControl()
				{
						this.txtFirstName = new TextBox();
						this.txtFirstName.ID = "txtFirstName";
						this.txtFirstName.Width = Unit.Percentage( 100 );

						this.txtLastName = new TextBox();
						this.txtLastName.ID = "txtLastName";
						this.txtLastName.Width = Unit.Percentage( 100 );

						this.txtOrganization = new TextBox();
						this.txtOrganization.ID = "txtOrganization";
						this.txtOrganization.Width = Unit.Percentage( 100 );

						this.txtId1 = new TextBox();
						this.txtId1.ID = "txtId1";
						this.txtId1.Width = Unit.Percentage( 100 );

						this.txtId2 = new TextBox();
						this.txtId2.ID = "txtId2";
						this.txtId2.Width = Unit.Percentage( 100 );

						this.txtId3 = new TextBox();
						this.txtId3.ID = "txtId3";
						this.txtId3.Width = Unit.Percentage( 100 );

						this.txtPhone = new TextBox();
						this.txtPhone.ID = "txtPhone";
						this.txtPhone.Width = Unit.Percentage( 100 );

						this.txtEmail = new TextBox();
						this.txtEmail.ID = "txtEmail";
						this.txtEmail.Width = Unit.Percentage( 100 );

						this.txtStreet = new TextBox();
						this.txtStreet.ID = "txtStreet";
						this.txtStreet.Width = Unit.Percentage( 100 );

						this.txtZip = new TextBox();
						this.txtZip.ID = "txtZip";
						this.txtZip.Width = Unit.Pixel( 50 );

						this.txtCity = new TextBox();
						this.txtCity.ID = "txtCity";
						this.txtCity.Width = Unit.Percentage( 100 );

						this.txtState = new TextBox();
						this.txtState.ID = "txtState";
						this.txtState.Width = Unit.Percentage( 100 );

						this.txtNotes = new TextBox();
						this.txtNotes.ID = "txtNotes";
						this.txtNotes.Width = Unit.Percentage( 100 );

						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = Resources.Controls.SaveButton_Text;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = Resources.Controls.CancelButton_Text;
						this.btnCancel.Click += new EventHandler( OnCancel );

						Table table = new Table();
						table.Width = this.Width;
						table.Height = this.Height;
						//table.Attributes.Add( "border", "1" );

						//FirstName|Phone
						TableRow row = null;
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.AddressControl_FirstName, this.txtFirstName, 0, true, null );
						AddControlToRow( row, Resources.Controls.AddressControl_Phone, this.txtPhone, 0, true, Settings.Validation.Phone  );
						table.Rows.Add( row );

						//LastName|Email
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.AddressControl_LastName, this.txtLastName, 0, true, null );
						AddControlToRow( row, Resources.Controls.AddressControl_Email, this.txtEmail, 0, true, Settings.Validation.Email );
						table.Rows.Add( row );

						//Organization|Id1
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.AddressControl_Organization, this.txtOrganization, 0, false, null );
						AddControlToRow( row, Resources.Controls.AddressControl_Id1, this.txtId1, 0, false, null );
						table.Rows.Add( row );

						//Street|Id2
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.AddressControl_Street, this.txtStreet, 0, true, null );
						AddControlToRow( row, Resources.Controls.AddressControl_Id2, this.txtId2, 0, false, null );
						table.Rows.Add( row );

						//Zip|Id3
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.AddressControl_Zip, this.txtZip, 0, true, Settings.Validation.Zip );
						AddControlToRow( row, Resources.Controls.AddressControl_Id3, this.txtId3, 0, false, null );
						table.Rows.Add( row );

						//City|Notes
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.AddressControl_City, this.txtCity, 0, true, null );
						AddControlToRow( row, Resources.Controls.AddressControl_Notes, this.txtNotes, 0, false, null );
						table.Rows.Add( row );

						//State
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.AddressControl_State, this.txtState, 0, false, null );
						table.Rows.Add( row );

						return table;
				}

				private void AddControlToRow( TableRow row, string labelText, Control control, int controlColspan, bool required, RegExpValidator validator )
				{
						( control as WebControl ).Enabled = this.IsEditing;

						TableCell cell = new TableCell();
						cell.CssClass = required && this.IsEditing ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						//cell.CssClass = "form_control";
						cell.Style.Add( "width", "50%" );
						cell.Style.Add( "padding-right", "5px" );//margin: 2px;white-space: nowrap;text-align: left;
						cell.Style.Add( "white-space", "nowrap" );
						cell.Style.Add( "text-align", "left" );
						cell.Controls.Add( control );
						cell.ColumnSpan = controlColspan;
						if ( required && this.IsEditing ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						if ( validator != null && this.IsEditing )
						{
								validator.ControlToValidate = control.ID;
								cell.Controls.Add( base.CreateRegularExpressionValidatorControl( validator ) );
						}
						row.Cells.Add( cell );
				}

				#region Public methods

				public ShpAddress UpdateEntityFromUI( ShpAddress address )
				{
						address.FirstName = this.txtFirstName.Text;
						address.LastName = this.txtLastName.Text;
						address.Organization = this.txtOrganization.Text;
						address.Id1 = this.txtId1.Text;
						address.Id2 = this.txtId2.Text;
						address.Id3 = this.txtId3.Text;
						address.Phone = this.txtPhone.Text;
						address.Email = this.txtEmail.Text;

						address.City = this.txtCity.Text;
						address.Notes = this.txtNotes.Text;
						address.State = this.txtState.Text;
						address.Street = this.txtStreet.Text;
						address.Zip = this.txtZip.Text;

						return address;
				}

				public void UpdateUIFromEntity( ShpAddress address )
				{
						EnsureChildControls();

						this.txtFirstName.Text = address.FirstName;
						this.txtLastName.Text = address.LastName;
						this.txtOrganization.Text = address.Organization;
						this.txtId1.Text = address.Id1;
						this.txtId2.Text = address.Id2;
						this.txtId3.Text = address.Id3;
						this.txtPhone.Text = address.Phone;
						this.txtEmail.Text = address.Email;

						this.txtCity.Text = address.City;
						this.txtNotes.Text = address.Notes;
						this.txtState.Text = address.State;
						this.txtStreet.Text = address.Street;
						this.txtZip.Text = address.Zip;
				}

				public ShpAddress UpdateAddress( ShpAddress address )
				{
						address = UpdateEntityFromUI( address );
						Storage<ShpAddress>.Update( address );
						return address;
				}

				public ShpAddress CreateAddress( ShpAddress address )
				{
						address = UpdateEntityFromUI( address );
						Storage<ShpAddress>.Create( address );
						return address;
				}
				#endregion

				#region Evant handlers
				void OnSave( object sender, EventArgs e )
				{
						if ( this.isNew ) CreateAddress( this.address );
						else UpdateAddress( this.address );

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
				#endregion

				#region Settings classes
				[Serializable()]
				public class ControlSettings
				{
						public ControlSettings()
						{
								this.Validation = new Validation();
						}
				
						public Validation Validation { get; set; }
				}

				[Serializable()]
				public class Validation
				{
						public Validation()
						{
								this.Email = null;
								this.Phone = null;
								this.Zip = null;
						}

						public RegExpValidator Email { get; set; }
						public RegExpValidator Phone { get; set; }
						public RegExpValidator Zip { get; set; }
				}

				#endregion

		}
}

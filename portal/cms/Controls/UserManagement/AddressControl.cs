﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;

namespace CMS.Controls.UserManagement
{
		public class AddressControl: CmsControl
		{
				#region Private mebers
				private TextBox txtCity = null;
				private TextBox txtDistrict = null;
				private TextBox txtRegion = null;
				private TextBox txtCountry = null;
				private TextBox txtState = null;
				private TextBox txtZip = null;
				private TextBox txtStreet = null;
				private TextBox txtNotes = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private Address address = null;
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
						if ( this.Settings == null )
								this.Settings = new ControlSettings();

						base.CreateChildControls();

						Control detailControl = CreateDetailControl();
						if ( detailControl != null )
								this.Controls.Add( detailControl );

						//Binding
						this.isNew = this.AddressId == null;
						if ( !this.AddressId.HasValue ) this.address = new Address();
						else this.address = Storage<Address>.ReadFirst( new Address.ReadById { AddressId = this.AddressId.Value } );

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
						this.txtStreet = new TextBox();
						this.txtStreet.ID = "txtStreet";
						this.txtStreet.Width = Unit.Percentage( 100 );

						this.txtZip = new TextBox();
						this.txtZip.ID = "txtZip";
						this.txtZip.Width = Unit.Pixel( 50 );

						this.txtCity = new TextBox();
						this.txtCity.ID = "txtCity";
						this.txtCity.Width = Unit.Percentage( 100 );

						this.txtDistrict = new TextBox();
						this.txtDistrict.ID = "txtDistrict";
						this.txtDistrict.Width = Unit.Percentage( 100 );

						this.txtRegion = new TextBox();
						this.txtRegion.ID = "txtRegion";
						this.txtRegion.Width = Unit.Percentage( 100 );

						this.txtCountry = new TextBox();
						this.txtCountry.ID = "txtCountry";
						this.txtCountry.Width = Unit.Percentage( 100 );

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

						//Street
						TableRow row = null;
						if ( this.Settings.Visibility.Street )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.AddressControl_Street, this.txtStreet, 3, this.Settings.Require.Street, null );
								table.Rows.Add( row );
						}

						//Zip
						if ( this.Settings.Visibility.Zip )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.AddressControl_Zip, this.txtZip, 3, this.Settings.Require.Zip, this.Settings.Validation.Zip );
								table.Rows.Add( row );
						}

						//City
						if ( this.Settings.Visibility.City )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.AddressControl_City, this.txtCity, 3, this.Settings.Require.City, null );
								table.Rows.Add( row );
						}

						//District
						if ( this.Settings.Visibility.District )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.AddressControl_District, this.txtDistrict, 3, this.Settings.Require.District, null );
								table.Rows.Add( row );
						}

						//Region
						if ( this.Settings.Visibility.Region )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.AddressControl_Region, this.txtRegion, 3, this.Settings.Require.Region, null );
								table.Rows.Add( row );
						}

						//Country
						if ( this.Settings.Visibility.Country )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.AddressControl_Country, this.txtCountry, 3, this.Settings.Require.Country, null );
								table.Rows.Add( row );
						}

						//State
						if ( this.Settings.Visibility.State )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.AddressControl_State, this.txtState, 3, this.Settings.Require.State, null );
								table.Rows.Add( row );
						}

						//Notes
						if ( this.Settings.Visibility.Notes )
						{
								row = new TableRow();
								AddControlToRow( row, Resources.Controls.AddressControl_Notes, this.txtNotes, 3, this.Settings.Require.Notes, null );
								table.Rows.Add( row );
						}
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
						cell.CssClass = "form_control";
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

				public Address UpdateEntityFromUI( Address address )
				{
						address.City = this.txtCity.Text;
						address.Country = this.txtCountry.Text;
						address.District = this.txtDistrict.Text;
						address.Notes = this.txtNotes.Text;
						address.Region = this.txtRegion.Text;
						address.State = this.txtState.Text;
						address.Street = this.txtStreet.Text;
						address.Zip = this.txtZip.Text;

						return address;
				}

				public void UpdateUIFromEntity( Address address )
				{
						this.txtCity.Text = address.City;
						this.txtCountry.Text = address.Country;
						this.txtDistrict.Text = address.District;
						this.txtNotes.Text = address.Notes;
						this.txtRegion.Text = address.Region;
						this.txtState.Text = address.State;
						this.txtStreet.Text = address.Street;
						this.txtZip.Text = address.Zip;
				}

				public Address UpdateAddress( Address address )
				{
						address = UpdateEntityFromUI( address );
						Storage<Address>.Update( address );
						return address;
				}

				public Address CreateAddress( Address address )
				{
						address = UpdateEntityFromUI( address );
						Storage<Address>.Create( address );
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
								this.Title = string.Empty;
								this.Visibility = new Visibility();
								this.Require = new Require();
								this.Validation = new Validation();
						}
						public ControlSettings Clone()
						{
								ControlSettings cs = new ControlSettings();
								cs.Title = this.Title;
								cs.Visibility = this.Visibility;
								cs.Require = this.Require;
								cs.Validation = this.Validation;
								return cs;
						}
						public string Title { get; set; }

						public Visibility Visibility { get; set; }
						public Require Require { get; set; }
						public Validation Validation { get; set; }
				}
				[Serializable()]
				public class Visibility
				{
						public Visibility()
						{
								this.City = true;
								this.District = true;
								this.Region = true;
								this.Country = true;
								this.State = true;
								this.Zip = true;
								this.Street = true;
								this.Notes = true;
						}

						public bool City { get; set; }
						public bool District { get; set; }
						public bool Region { get; set; }
						public bool Country { get; set; }
						public bool State { get; set; }
						public bool Zip { get; set; }
						public bool Street { get; set; }
						public bool Notes { get; set; }
				}

				[Serializable()]
				public class Require
				{
						public Require()
						{
								this.City = false;
								this.District = false;
								this.Region = false;
								this.Country = false;
								this.State = false;
								this.Zip = false;
								this.Street = false;
								this.Notes = false;
						}

						public bool City { get; set; }
						public bool District { get; set; }
						public bool Region { get; set; }
						public bool Country { get; set; }
						public bool State { get; set; }
						public bool Zip { get; set; }
						public bool Street { get; set; }
						public bool Notes { get; set; }
				}

				[Serializable()]
				public class Validation
				{
						public Validation()
						{
								this.Zip = null;
						}

						public RegExpValidator Zip { get; set; }
				}

				#endregion
		}
}

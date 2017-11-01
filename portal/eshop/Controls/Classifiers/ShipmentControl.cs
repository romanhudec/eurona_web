using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;
using CMS.Entities.Classifiers;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using VATEntity = SHP.Entities.Classifiers.VAT;
using CMS.Controls;
using SupportedLocaleEntity = CMS.Entities.Classifiers.SupportedLocale;
using System.Collections.Generic;

namespace SHP.Controls.Classifiers
{
		public class ShipmentControl: ClassifierControl<ShipmentEntity>
		{
				protected TextBox txtPrice = null;
				protected DropDownList ddlVAT = null;

				public ShipmentControl()
				{
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						Control pollControl = CreateDetailControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

						//Binding
						if ( !this.Id.HasValue ) this.classifier = new ShipmentEntity();
						else this.classifier = Storage<ShipmentEntity>.Read( new ClassifierBase.ReadById { Id = this.Id.Value } )[0];

						//Nastavenie zobrazenia icony
						if ( string.IsNullOrEmpty( this.classifier.Icon ) ) { this.iconRemove.Visible = false; this.icon.Visible = false; }
						else this.iconUpload.Visible = false;

						//Datasource for DropDownList
						this.ddlVAT.DataSource = Storage<VATEntity>.Read( null );
						this.ddlVAT.DataValueField = "Id";
						this.ddlVAT.DataTextField = "Display";
						if ( !IsPostBack )
						{
								this.ddlVAT.DataBind();
								this.txtName.Text = this.classifier.Name;
								this.txtPrice.Text = this.classifier.Price.ToString();
								if ( this.classifier.VATId.HasValue ) this.ddlVAT.SelectedValue = this.classifier.VATId.ToString();
								this.icon.ImageUrl = this.classifier.Icon != null ? Page.ResolveUrl( this.classifier.Icon ) : string.Empty;
								this.icon.Style.Add( "max-width", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );
								this.icon.Style.Add( "max-height", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );
								this.txtNotes.Text = this.classifier.Notes;
								this.DataBind();
						}
				}
				#endregion


				/// <summary>
				/// Vytvori Control
				/// </summary>
				protected override Control CreateDetailControl()
				{
						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						this.txtName.Width = Unit.Percentage( 100 );

						this.txtPrice = new TextBox();
						this.txtPrice.ID = "txtPrice";
						this.txtPrice.Width = Unit.Percentage( 100 );

						this.ddlVAT = new DropDownList();
						this.ddlVAT.ID = "ddlVAT";
						this.ddlVAT.Width = Unit.Pixel( 100 );

						this.icon = new Image();
						this.icon.ID = "icon";
						this.iconUpload = new FileUpload();
						this.iconUpload.ID = "iconUpload";
						this.iconRemove = new Button();
						this.iconRemove.Text = Resources.Controls.ClassifierControl_RemoveIcon;
						this.iconRemove.ID = "iconRemove";
						this.iconRemove.Click += ( s, e ) =>
						{
								if ( this.classifier == null ) return;
								this.RemoveIcon( this.classifier );
								this.icon.ImageUrl = string.Empty;

								//Nastavenie viditelnosti
								this.iconRemove.Visible = false;
								this.icon.Visible = false;
								this.iconUpload.Visible = true;
						};

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

						//Name
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = "form_label_required";
						cell.Text = Resources.Controls.ClassifierControl_Name;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtName );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtName.ID ) );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Price
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.ShipmentControl_Price;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtPrice );
						cell.Controls.Add( CreateDoubleValidatorControl( this.txtPrice.ID ) );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtPrice.ID ) );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//VAT
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.ShipmentControl_VAT;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.ddlVAT );
						cell.Controls.Add( CreateDoubleValidatorControl( this.ddlVAT.ID ) );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.ddlVAT.ID ) );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Icon
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.ClassifierControl_Icon;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.VerticalAlign = VerticalAlign.Middle;
						cell.Controls.Add( this.icon );
						cell.Controls.Add( this.iconRemove );
						cell.Controls.Add( this.iconUpload );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Notes
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.ClassifierControl_Notes;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtNotes );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Save Cancel Buttons
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnSave );
						cell.Controls.Add( this.btnCancel );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						return table;
				}

				public override void Save()
				{
						this.classifier.Name = this.txtName.Text;
						this.classifier.Price = Convert.ToDecimal( this.txtPrice.Text );
						this.classifier.VATId = Convert.ToInt32( this.ddlVAT.Text );
						this.classifier.Notes = this.txtNotes.Text;
						this.classifier.Icon = this.UploadIcon( this.classifier );
						this.classifier.Locale = Security.Account.Locale;

						bool isNew = !this.Id.HasValue;
						if ( isNew )
						{
								this.classifier.Code = CMS.Utilities.StringUtilities.RemoveDiacritics( this.classifier.Name );

								//Supported languages - Vytvorenie vo vsetkych podporovanych jazykoch
								List<SupportedLocaleEntity> list = Storage<SupportedLocaleEntity>.Read();
								if ( list.Count == 0 )
										this.ClassifierStorage.Create( this.classifier );
								else
								{
										Entities.Classifiers.VAT vat = Storage<Entities.Classifiers.VAT>.ReadFirst( new Entities.Classifiers.VAT.ReadById{ Id = this.classifier.VATId.Value});
										foreach ( SupportedLocaleEntity locale in list )
										{
												List<Entities.Classifiers.VAT> vats = Storage<Entities.Classifiers.VAT>.Read( new Entities.Classifiers.VAT.ReadByCode { Code = vat.Code } );
												Entities.Classifiers.VAT localeVat = vats.Find( x=>x.Locale == locale.Code );
												this.classifier.VATId = localeVat != null ? localeVat.Id : (int?)null ;
												this.classifier.Locale = locale.Code;
												Storage<ShipmentEntity>.Create( this.classifier );
										}
								}
						}
						else Storage<ShipmentEntity>.Update( this.classifier );
				}
		}
}

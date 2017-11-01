using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CMS;
using System.Web.UI.WebControls;
using CMS.MSSQL;
using CMS.Entities.Classifiers;
using System.IO;
using CMS.Controls;

namespace Eurona.Admin.Controls
{
		public class IconClassifierControl<T>: CMS.Controls.CmsControl where T: ClassifierBase, new()
		{
				protected TextBox txtName = null;
				protected TextBox txtCode = null;
				protected TextBox txtNotes = null;
				protected FileUpload iconUpload = null;
				protected Image icon = null;
				protected Button iconRemove = null;

				protected Button btnSave = null;
				protected Button btnCancel = null;

				protected T classifier = null;

				public IconClassifierControl()
				{
				}

				/// <summary>
				///Property musi byt vyplnena ak sa nejedna o novy ciselnik.
				/// </summary>
				public int? Id
				{
						get
						{
								object o = ViewState["Id"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["Id"] = value; }
				}

				/// <summary>
				/// Storage ciselnika
				/// </summary>

				public IStorage<T> ClassifierStorage
				{
						get { return new WebStorage<T>().Access(); }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control classifierControl = CreateDetailControl();
						if ( classifierControl != null )
								this.Controls.Add( classifierControl );

						//Binding
						if ( !this.Id.HasValue ) this.classifier = new T();
						else this.classifier = this.ClassifierStorage.Read( new ClassifierBase.ReadById { Id = this.Id.Value } )[0];

						//Nastavenie zobrazenia icony
						if ( string.IsNullOrEmpty( this.classifier.Icon ) ) { this.iconRemove.Visible = false; this.icon.Visible = false; }
						else this.iconUpload.Visible = false;

						if ( !IsPostBack )
						{
								this.txtName.Text = this.classifier.Name;
								this.txtCode.Text = this.classifier.Code;
								this.txtNotes.Text = this.classifier.Notes;
								this.icon.ImageUrl = this.classifier.Icon != null ? Page.ResolveUrl( this.classifier.Icon ) : string.Empty;
								this.icon.Style.Add( "max-width", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );
								this.icon.Style.Add( "max-height", ImageGalleryControl.IMAGE_WIDTH.ToString() + "px" );
								this.DataBind();
						}

				}
				#endregion

				#region Protected, Private Methods
				/// <summary>
				/// Vytvori Control
				/// </summary>
				protected virtual Control CreateDetailControl()
				{
						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						this.txtName.Width = Unit.Percentage( 100 );

						this.txtCode = new TextBox();
						this.txtCode.ID = "txtCode";
						this.txtCode.Width = Unit.Percentage( 100 );

						this.txtNotes = new TextBox();
						this.txtNotes.ID = "txtNotes";
						this.txtNotes.Width = Unit.Percentage( 100 );

						this.icon = new Image();
						this.icon.ID = "icon";
						this.iconUpload = new FileUpload();
						this.iconUpload.ID = "iconUpload";
						this.iconRemove = new Button();
						this.iconRemove.Text = Resources.Strings.ClassifierControl_RemoveIcon;
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

						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = Resources.Strings.SaveButton_Text;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = Resources.Strings.CancelButton_Text;
						this.btnCancel.Click += new EventHandler( OnCancel );

						Table table = new Table();
						table.Width = this.Width;
						table.Height = this.Height;

						//Name
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = "form_label_required";
						cell.Text = Resources.Strings.ClassifierControl_Name;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtName );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtName.ID ) );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Code
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Strings.ClassifierControl_Code;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtCode );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Icon
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Strings.ClassifierControl_Icon;
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
						cell.Text = Resources.Strings.ClassifierControl_Notes;
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

				protected void RemoveIcon( T classifier )
				{
						string filePath = Server.MapPath( classifier.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						classifier.Icon = null;
						this.ClassifierStorage.Update( classifier );
				}

				protected string UploadIcon( T classifier )
				{
						if ( !this.iconUpload.HasFile ) return classifier.Icon;

						string storagePath = CMS.Utilities.ConfigUtilities.GetEntityIconStoragePath( classifier.GetType() );
						string filePath = Server.MapPath( classifier.Icon );
						if ( File.Exists( filePath ) ) File.Delete( filePath );

						string storageDirectoty = Server.MapPath( storagePath );
						if ( !Directory.Exists( storageDirectoty ) ) Directory.CreateDirectory( storageDirectoty );
						string dstFileName = string.Format( "{0}.png", CMS.Utilities.StringUtilities.RemoveDiacritics( classifier.Name ) );
						dstFileName = dstFileName.Replace( ":", "-" ); dstFileName = dstFileName.Replace( "&", "" );
						string dstFilePath = Path.Combine( storageDirectoty, dstFileName );

						//Zapis suboru
						Stream stream = this.iconUpload.PostedFile.InputStream;
						int len = (int)stream.Length;
						if ( len == 0 ) return null;
						byte[] data = new byte[len];
						stream.Read( data, 0, len );
						stream.Close();
						using ( FileStream fs = new FileStream( dstFilePath, FileMode.Create, FileAccess.Write ) )
						{
								fs.Write( data, 0, len );
						}

						return string.Format( "{0}{1}", storagePath, dstFileName );
				}

				public virtual void Save()
				{
						this.classifier.Name = this.txtName.Text;
						this.classifier.Code = this.txtCode.Text;
						this.classifier.Notes = this.txtNotes.Text;
						this.classifier.Locale = Security.Account.Locale;
						this.classifier.Icon = this.UploadIcon( this.classifier );

						bool isNew = !this.Id.HasValue;
						if ( isNew ) this.ClassifierStorage.Create( this.classifier );
						else this.ClassifierStorage.Update( this.classifier );
				}

				protected void OnSave( object sender, EventArgs e )
				{
						this.Save();
						Response.Redirect( this.ReturnUrl );
				}

				protected void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
				#endregion
		}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using CMS;
using System.Web.UI.WebControls;
using CMS.MSSQL;
using CMS.Entities.Classifiers;

namespace Eurona.Admin.Controls
{
		public class ClassifierControl<T>: CMS.Controls.CmsControl where T: ClassifierBase, new()
		{
				protected TextBox txtName = null;
				protected TextBox txtNotes = null;

				protected Button btnSave = null;
				protected Button btnCancel = null;

				protected T classifier = null;

				public ClassifierControl()
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

						Control pollControl = CreateDetailControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

						//Binding
						if ( !this.Id.HasValue ) this.classifier = new T();
						else
								this.classifier = this.ClassifierStorage.Read( new ClassifierBase.ReadById { Id = this.Id.Value } )[0];


						if ( !IsPostBack )
						{
								this.txtName.Text = this.classifier.Name;
								this.txtNotes.Text = this.classifier.Notes;
								this.DataBind();
						}

				}
				#endregion


				/// <summary>
				/// Vytvori Control Ankety
				/// </summary>
				protected virtual Control CreateDetailControl()
				{
						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						this.txtName.Width = Unit.Pixel( 400 );

						this.txtNotes = new TextBox();
						this.txtNotes.ID = "txtNotes";
						this.txtNotes.Width = Unit.Percentage( 100 );

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

						//Order
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

				public virtual void Save()
				{
						this.classifier.Name = this.txtName.Text;
						this.classifier.Notes = this.txtNotes.Text;
						this.classifier.Locale = Security.Account.Locale;

						bool isNew = !this.Id.HasValue;
						if ( isNew ) this.ClassifierStorage.Create( this.classifier );
						else this.ClassifierStorage.Update( this.classifier );
				}

				void OnSave( object sender, EventArgs e )
				{
						this.Save();
						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}

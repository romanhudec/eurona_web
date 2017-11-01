using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoleEntity = CMS.Entities.Role;
using System.Web.UI.WebControls;


namespace CMS.Controls.Role
{
		public class AdminRoleControl: CmsControl
		{
				private TextBox txtName;
				private TextBox txtNotes;
				private Button btnSave;
				private Button btnCancel;

				private bool isNew = false;
				private RoleEntity roleEntity = null;

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? RoleId
				{
						get
						{
								object o = ViewState["RoleId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["RoleId"] = value; }
				}
				
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						isNew = !this.RoleId.HasValue;
						if ( isNew ) roleEntity = new RoleEntity();
						else roleEntity = Storage<RoleEntity>.ReadFirst( new RoleEntity.ReadById { RoleId = this.RoleId.Value } );

						Table table = new Table();
						table.CssClass = this.CssClass;
						table.Width = this.Width;

						TableRow trName = new TableRow();
						trName.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminRoleControl_LabelName,
								CssClass = "form_label_required"
						} );
						trName.Cells.Add( CreateNameInput() );
						table.Rows.Add( trName );

						TableRow trNotes = new TableRow();
						trNotes.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminRoleControl_LabelNotes,
								CssClass = "form_label"
						} );
						trNotes.Cells.Add( CreateNotesInput() );
						table.Rows.Add( trNotes );

						
						CreateSaveButton();
						CreateCancelButton();

						TableRow trButtons = new TableRow();
						TableCell tdButtons = new TableCell();
						tdButtons.ColumnSpan = 2;
						tdButtons.Controls.Add( btnSave );
						tdButtons.Controls.Add( btnCancel );
						trButtons.Cells.Add( tdButtons );
						table.Rows.Add( trButtons );

						Controls.Add( table );
				}

				private TableCell CreateNameInput()
				{
						TableCell cell = new TableCell();
						this.txtName = new TextBox()
						{
								ID = "txtName",
								Text = roleEntity.Name,
								Width = Unit.Percentage( 80 )
						};
						cell.Controls.Add( this.txtName );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtName.ID ) );
						return cell;
				}

				private TableCell CreateNotesInput()
				{
						TableCell cell = new TableCell();
						this.txtNotes = new TextBox()
						{
								ID = "txtNotes",
								Text = roleEntity.Notes,
								Width = Unit.Percentage( 80 ),
								TextMode = TextBoxMode.MultiLine,
								Rows = 10
						};
						cell.Controls.Add( this.txtNotes );
						return cell;
				}

				private void CreateCancelButton()
				{
						btnCancel = new Button
						{
								Text = Resources.Controls.CancelButton_Text,
								CausesValidation = false
						};
						btnCancel.Click += ( s1, e1 ) => Response.Redirect( this.ReturnUrl );
				}

				private void CreateSaveButton()
				{
						btnSave = new Button
						{
								Text = Resources.Controls.SaveButton_Text
						};

						btnSave.Click += ( s1, e1 ) =>
						{

								roleEntity.Name = txtName.Text;
								roleEntity.Notes = txtNotes.Text;

								if ( isNew ) Storage<RoleEntity>.Create( roleEntity );
								else Storage<RoleEntity>.Update( roleEntity );

								Response.Redirect( this.ReturnUrl );
						};
				}

		}
}

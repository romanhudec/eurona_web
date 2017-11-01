using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagEntity = CMS.Entities.Tag;
using System.Web.UI.WebControls;


namespace CMS.Controls.Tag
{
		public class AdminTagControl: CmsControl
		{
				private TextBox txtName;
				private Button btnSave;
				private Button btnCancel;

				private bool isNew = false;
				private TagEntity tag = null;

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? TagId
				{
						get
						{
								object o = ViewState["TagId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["TagId"] = value; }
				}
				
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						isNew = !this.TagId.HasValue;
						if ( isNew ) tag = new TagEntity();
						else tag = Storage<TagEntity>.ReadFirst( new TagEntity.ReadById { TagId = this.TagId.Value } );

						Table table = new Table();
						table.CssClass = this.CssClass;
						table.Width = this.Width;

						TableRow trName = new TableRow();
						trName.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminTagControl_LabelName,
								CssClass = "form_label_required"
						} );
						trName.Cells.Add( CreateNameInput() );
						table.Rows.Add( trName );
						
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
								Text = tag.Name,
								Width = Unit.Percentage( 80 ),
								CssClass = "form_control"
						};
						cell.Controls.Add( this.txtName );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtName.ID ) );
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

								tag.Name = txtName.Text;
								if ( isNew ) Storage<TagEntity>.Create( tag );
								else Storage<TagEntity>.Update( tag );

								Response.Redirect( this.ReturnUrl );
						};
				}

		}
}

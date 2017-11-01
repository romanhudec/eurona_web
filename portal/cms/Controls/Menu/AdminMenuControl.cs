using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MenuEntity = CMS.Entities.Menu;
using UrlAliasEntity = CMS.Entities.UrlAlias;
using RoleEntity = CMS.Entities.Role;

namespace CMS.Controls.Menu
{
		public class AdminMenuControl: CmsControl
		{
				private TextBox tbCode;
				private TextBox tbName;
				private DropDownList ddlRole;
				private Button btnSave;
				private Button btnCancel;

				private bool isNew = false;
				private MenuEntity menuEntity = null;

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						isNew = Request["Id"] == null;
						if ( isNew ) menuEntity = new MenuEntity();
						else
						{
								int menuId = Convert.ToInt32( Request["id"] );
								menuEntity = Storage<MenuEntity>.ReadFirst( new MenuEntity.ReadById { MenuId = menuId } );
						}

						Table table = new Table();
						table.Width = this.Width;

						TableRow trOrder = new TableRow();
						trOrder.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminMenuControl_LabelCode,
								CssClass = "form_label_required"
						} );
						trOrder.Cells.Add( CreateCodeInput() );
						table.Rows.Add( trOrder );

						TableRow trName = new TableRow();
						trName.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminMenuControl_LabelName,
								CssClass = "form_label_required"
						} );
						trName.Cells.Add( CreateNameInput() );
						table.Rows.Add( trName );

						TableRow trRole = new TableRow();
						trRole.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminMenuControl_LabelRole,
								CssClass = "form_label_required"
						} );
						trRole.Cells.Add( CreateRoleList() );
						table.Rows.Add( trRole );

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

				private TableCell CreateCodeInput()
				{
						TableCell cell = new TableCell();
						cell.CssClass = "form_control";
						tbCode = new TextBox
						{
								ID = "tbCode",
								Text = menuEntity.Code,
								Width = Unit.Percentage( 80 ),
								Enabled = this.menuEntity.Id >= 0 
						};
						cell.Controls.Add( tbCode );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( tbCode.ID ) );
						return cell;
				}

				private TableCell CreateNameInput()
				{
						TableCell cell = new TableCell();
						cell.CssClass = "form_control";
						tbName = new TextBox
						{
								ID = "tbName",
								Text = menuEntity.Name,
								Width = Unit.Percentage( 80 )

						};
						cell.Controls.Add( tbName );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( tbName.ID ) );
						return cell;
				}

				private TableCell CreateRoleList()
				{
						List<RoleEntity> roles = Storage<RoleEntity>.Read();
						roles.Add( new RoleEntity { Id = 0, Name = String.Empty } );
						roles = roles.OrderBy( p => p.Name ).ToList();
						TableCell cell = new TableCell();
						cell.CssClass = "form_control";
						ddlRole = new DropDownList();
						ddlRole.ID = "ddlRole";
						ddlRole.DataSource = roles;
						ddlRole.DataTextField = "Name";
						ddlRole.DataValueField = "Id";
						ddlRole.Width = Unit.Percentage( 80 );
						ddlRole.DataBind();
						//Systemove role su podfarbene
						foreach ( ListItem li in ddlRole.Items )
						{
								RoleEntity role = roles.FirstOrDefault( p => p.Id.ToString() == li.Value );
								if ( role == default( RoleEntity ) ) continue;
								if ( role.Id >= 0 ) continue;
								li.Attributes.Add( "style", "font-weight: bold; background-color: silver;" );
						}

						cell.Controls.Add( ddlRole );
						ddlRole.SelectedValue = menuEntity.RoleId.HasValue ? menuEntity.RoleId.ToString() : "0";
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
								menuEntity.Code = tbCode.Text;
								menuEntity.Name = tbName.Text;
								menuEntity.RoleId = Convert.ToInt32( ddlRole.SelectedValue );
								if ( menuEntity.RoleId == 0 ) menuEntity.RoleId = null;

								if ( isNew )
										Storage<MenuEntity>.Create( menuEntity );
								else
										Storage<MenuEntity>.Update( menuEntity );

								Response.Redirect( this.ReturnUrl );
						};
				}
		}
}

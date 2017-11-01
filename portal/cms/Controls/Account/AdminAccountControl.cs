using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccountEntity = CMS.Entities.Account;
using RoleEntity = CMS.Entities.Role;
using System.Web.UI.WebControls;


namespace CMS.Controls.Account
{
		public class AdminAccountControl: CmsControl
		{
				private TextBox tbLogin;
				private TextBox tbEmail;
				private CheckBox cbEnabled;
				private CheckBox cbVerified;
				//private TextBox tbVerifyCode;
				private CheckBoxList clbRoles;
				private Button btnSave;
				private Button btnChangePassword;
				private Button btnCancel;

				private bool isNew = false;
				private AccountEntity accountEntity = null;

				public string ChangePasswordUrlFormat { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						isNew = Request["Id"] == null;
						if ( isNew ) accountEntity = new AccountEntity();
						else
						{
								int accountId = Convert.ToInt32( Request["id"] );
								accountEntity = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadById { AccountId = accountId } );
						}

						Table table = new Table();
						table.CssClass = this.CssClass;
						table.Width = this.Width;

						TableRow trLogin = new TableRow();
						trLogin.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminAccountControl_LabelLogin,
								CssClass = "form_label_required"
						} );
						trLogin.Cells.Add( CreateLoginInput() );
						table.Rows.Add( trLogin );

						TableRow trEmail = new TableRow();
						trEmail.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminAccountControl_LabelEmail,
								CssClass = "form_label_required"
						} );
						trEmail.Cells.Add( CreateEmailInput() );
						table.Rows.Add( trEmail );

						TableRow trEnabled = new TableRow();
						trEnabled.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminAccountControl_LabelEnabled,
								CssClass = "form_label_required"
						} );
						trEnabled.Cells.Add( CreateEnabledCheckBox() );
						table.Rows.Add( trEnabled );

						TableRow trVerified = new TableRow();
						trVerified.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminAccountControl_LabelVerified,
								CssClass = "form_label_required"
						} );
						trVerified.Cells.Add( CreateVerifiedCheckBox() );
						table.Rows.Add( trVerified );

						/*
						TableRow trVerifyCode = new TableRow();
						trVerifyCode.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminAccountControl_LabelVerifyCode,
								CssClass = "form_label_required"
						} );
						trVerifyCode.Cells.Add( CreateVerifyCodeInput() );
						table.Rows.Add( trVerifyCode );
						 * */

						TableRow trRoles = new TableRow();
						trRoles.Cells.Add( new TableCell
						{
								Text = Resources.Controls.AdminAccountControl_LabelRoles,
								CssClass = "form_label_required"
						} );
						trRoles.Cells.Add( CreateRolesCheckListBox() );
						table.Rows.Add( trRoles );

						CreateSaveButton();
						CreateChangePasswordButton();
						CreateCancelButton();

						TableRow trButtons = new TableRow();
						TableCell tdButtons = new TableCell();
						tdButtons.ColumnSpan = 2;
						tdButtons.Controls.Add( btnSave );
						tdButtons.Controls.Add( btnChangePassword );
						tdButtons.Controls.Add( btnCancel );
						trButtons.Cells.Add( tdButtons );
						table.Rows.Add( trButtons );

						Controls.Add( table );
				}

				private TableCell CreateLoginInput()
				{
						TableCell cell = new TableCell();
						tbLogin = new TextBox
						{
								ID = "tbLogin",
								Text = accountEntity.Login,
								Width = Unit.Percentage( 80 )
						};
						cell.Controls.Add( tbLogin );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( tbLogin.ID ) );
						return cell;
				}

				private TableCell CreateEmailInput()
				{
						TableCell cell = new TableCell();
						tbEmail = new TextBox
						{
								ID = "tbEmail",
								Text = accountEntity.Email,
								Width = Unit.Percentage( 80 )
						};
						cell.Controls.Add( tbEmail );
						cell.Controls.Add( CreateEmailValidatorControl( tbEmail.ID ) );
						return cell;
				}

				private TableCell CreateEnabledCheckBox()
				{
						TableCell cell = new TableCell();
						cbEnabled = new CheckBox
						{
								ID = "cbEnabled",
								Checked = accountEntity.Enabled
						};
						cell.Controls.Add( cbEnabled );
						return cell;
				}

				private TableCell CreateVerifiedCheckBox()
				{
						TableCell cell = new TableCell();
						cbVerified = new CheckBox
						{
								ID = "cbVerified",
								Checked = accountEntity.Verified
						};
						cell.Controls.Add( cbVerified );
						return cell;
				}

				//private TableCell CreateVerifyCodeInput()
				//{
				//    TableCell cell = new TableCell();
				//    tbVerifyCode = new TextBox
				//    {
				//        ID = "tbVerifyCode",
				//        Text = accountEntity.VerifyCode,
				//        Width = Unit.Percentage( 80 )
				//    };
				//    tbVerifyCode.Enabled = !accountEntity.Verified;
				//    cell.Controls.Add( tbVerifyCode );
				//    cell.Controls.Add( CreateRequiredFieldValidatorControl( tbVerifyCode.ID ) );
				//    return cell;
				//}

				private TableCell CreateRolesCheckListBox()
				{
						List<RoleEntity> roles = Storage<RoleEntity>.Read().OrderBy( p => p.Name ).ToList();
						TableCell cell = new TableCell();
						clbRoles = new CheckBoxList();
						clbRoles.ID = "clbRoles";
						clbRoles.DataSource = roles;
						clbRoles.DataTextField = "Name";
						clbRoles.DataValueField = "Id";
						clbRoles.DataBind();
						foreach ( ListItem li in clbRoles.Items )
						{
								//Z role registrovaneho pouzivatela nie je mozne odobrat
								if ( li.Text.ToUpper() == RoleEntity.REGISTEREDUSER.ToUpper() )
								{
										li.Enabled = false;
										//Ak sa jedna o nove konto, pouzivatel sa automaticky stava registrovanym pouzivatelom.
										if ( isNew )
										{
												li.Selected = true;
												continue;
										}
								}

								li.Selected = accountEntity.RoleString.IndexOf( li.Text ) > -1;
						}
						
						cell.Controls.Add( clbRoles );
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
				private void CreateChangePasswordButton()
				{
						btnChangePassword = new Button
						{
								Text = Resources.Controls.ChangePassword_Text,
								CausesValidation = false
						};
						string url = string.Format( this.ChangePasswordUrlFormat, accountEntity.Id ) + "&" + this.BuildReturnUrlQueryParam();
						btnChangePassword.Click += ( s1, e1 ) => Response.Redirect( Page.ResolveUrl( url ) );
				}
				private void CreateSaveButton()
				{
						btnSave = new Button
						{
								Text = Resources.Controls.SaveButton_Text
						};
						btnSave.Click += ( s1, e1 ) =>
						{
								StringBuilder roles = new StringBuilder();
								foreach ( ListItem li in clbRoles.Items )
										if ( li.Selected ) roles.AppendFormat( "{0};", li.Text );

								accountEntity.Login = tbLogin.Text;
								accountEntity.Email = tbEmail.Text;
								accountEntity.Enabled = cbEnabled.Checked;
								accountEntity.Verified = cbVerified.Checked;
								//accountEntity.VerifyCode = tbVerifyCode.Text;
								accountEntity.RoleString = roles.ToString();
								if ( !accountEntity.IsInRole( RoleEntity.REGISTEREDUSER ) )
										accountEntity.AddToRoles( RoleEntity.REGISTEREDUSER );

								if ( isNew )
										Storage<AccountEntity>.Create( accountEntity );
								else
										Storage<AccountEntity>.Update( accountEntity );

								Response.Redirect( this.ReturnUrl );
						};
				}

		}
}

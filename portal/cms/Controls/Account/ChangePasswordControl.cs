using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccountEntity = CMS.Entities.Account;
using System.Web.UI.WebControls;
using System.Web.UI;
using CMS.Utilities;

namespace CMS.Controls.Account
{
		public class ChangePasswordControl: CmsControl
		{
				private Telerik.Web.UI.RadCaptcha capcha = null;
				private Label lblLogin = null;
				private TextBox txtOldPassword = null;
				private TextBox txtNewPassword = null;
				private TextBox txtConfirmNewPassword = null;
				private Button btnChange = null;
				private Button btnCancel = null;

				private AccountEntity account = null;

				#region Public properties
				public int AccountId { get; set; }
                public string Salt { get; set; }
				#endregion

				#region Protected Overrides
				protected override void CreateChildControls()
				{
						//Povolene len pre prihlaseneho pouzivatela
						if ( !Security.IsLogged( true ) ) return;

						base.CreateChildControls();

						this.AccountId = Convert.ToInt32( Request["id"] );
						this.account = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadById { AccountId = this.AccountId } );
						if ( this.account == null ) return;

						#region Create controls
						this.lblLogin = new Label();
						this.lblLogin.Text = this.account.Login;

						this.txtOldPassword = new TextBox();
						this.txtOldPassword.ID = "oldPwd";
						this.txtOldPassword.TextMode = TextBoxMode.Password;

						this.txtNewPassword = new TextBox();
						this.txtNewPassword.ID = "newPwd";
						this.txtNewPassword.TextMode = TextBoxMode.Password;

						this.txtConfirmNewPassword = new TextBox();
						this.txtConfirmNewPassword.ID = "cnewPwd";
						this.txtConfirmNewPassword.TextMode = TextBoxMode.Password;

						this.capcha = new Telerik.Web.UI.RadCaptcha();
						this.capcha.ErrorMessage = Resources.Controls.ForgotPasswordControl_Capcha_ErrorMessage;
						this.capcha.CaptchaTextBoxLabel = Resources.Controls.ForgotPasswordControl_Capcha_TextBoxLabel;
						this.capcha.Width = Unit.Percentage( 100 );
						this.capcha.EnableRefreshImage = true;
						this.capcha.CaptchaLinkButtonText = Resources.Controls.ForgotPasswordControl_Capcha_LinkButtonText;

						this.btnChange = new Button();
						this.btnChange.ID = "btnChange";
						this.btnChange.Text = Resources.Controls.ChangePasswordControl_Change_ButtonText;
						this.btnChange.Click += ( s, e ) =>
						{
								this.capcha.Validate();
								if ( !this.capcha.IsValid ) return;

								if ( Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) == false )
								{
										if ( this.account.Password != Cryptographer.MD5Hash( this.txtOldPassword.Text, Salt ) )
										{
												string script = string.Format( "alert('{0}');", Resources.Controls.ChangePasswordControl_InvalidOldPasswordMessage );
												Page.ClientScript.RegisterStartupScript( this.GetType(), "ChangePasswordControl_error", script, true );
												return;
										}
								}

								this.account.Password = Cryptographer.MD5Hash( this.txtNewPassword.Text, Salt );
								this.account = Storage<AccountEntity>.Update( account );

								string url = this.ReturnUrl;
								if ( string.IsNullOrEmpty( url ) ) return;
								Response.Redirect( Page.ResolveUrl( url ) );
						};

						this.btnCancel = new Button();
						this.btnCancel.ID = "btnCancel";
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = Resources.Controls.CancelButton_Text;
						this.btnCancel.Click += ( s, e ) =>
								{
										string url = this.ReturnUrl;
										if ( string.IsNullOrEmpty( url ) ) return;
										Response.Redirect( Page.ResolveUrl( url ) );
								};
						#endregion

						Table table = new Table();
						table.Width = this.Width;
						table.Height = this.Height;
						table.Rows.Add( CreateTableRow( Resources.Controls.ChangePasswordControl_Login_Label, this.lblLogin, false ) );
						if ( Security.Account.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) == false )
								table.Rows.Add( CreateTableRow( Resources.Controls.ChangePasswordControl_OldPassword_Label, this.txtOldPassword, false ) );
						
						table.Rows.Add( CreateTableRow( Resources.Controls.ChangePasswordControl_NewPassword_Label, this.txtNewPassword, true ) );
						TableRow row = CreateTableRow( Resources.Controls.ChangePasswordControl_ConfirmNewPassword_Label, this.txtConfirmNewPassword, true );
						CompareValidator cv = new CompareValidator();
						cv.ID = "cv_pwd";
						cv.ErrorMessage = "!";
						cv.CssClass = "ms-formvalidation";
						cv.ControlToValidate = this.txtNewPassword.ID;
						cv.ControlToCompare = this.txtConfirmNewPassword.ID;
						cv.SetFocusOnError = true;
						cv.EnableClientScript = true;
						TableCell cell = new TableCell();
						cell.Controls.Add( cv );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Capcha
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.HorizontalAlign = HorizontalAlign.Center;
						cell.Controls.Add( this.capcha );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Save Cancel Buttons
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnChange );
						cell.Controls.Add( this.btnCancel );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						this.Controls.Add( table );
				}
				#endregion

				private TableRow CreateTableRow( string labelText, Control control, bool required )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						if ( required ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						row.Cells.Add( cell );

						return row;
				}
		}
}

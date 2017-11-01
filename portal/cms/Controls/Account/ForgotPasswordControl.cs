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
		public class ForgotPasswordControl : CmsControl
		{
				private TextBox txtInput = null;
				private Telerik.Web.UI.RadCaptcha capcha = null;
				private Button btnSend = null;
				private Button btnCancel = null;

                public string Salt { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.txtInput = new TextBox();
						this.txtInput.ID = "txtInput";
						this.txtInput.Width = Unit.Percentage( 100 );

						this.capcha = new Telerik.Web.UI.RadCaptcha();
						this.capcha.ErrorMessage = Resources.Controls.ForgotPasswordControl_Capcha_ErrorMessage;
						this.capcha.CaptchaTextBoxLabel = Resources.Controls.ForgotPasswordControl_Capcha_TextBoxLabel;
						this.capcha.Width = Unit.Percentage( 100 );
						this.capcha.EnableRefreshImage = true;
						this.capcha.CaptchaLinkButtonText = Resources.Controls.ForgotPasswordControl_Capcha_LinkButtonText;
						//this.capcha.CaptchaTextBoxLabelCssClass 

						this.btnSend = new Button();
						this.btnSend.ID = "btnChange";
						this.btnSend.Text = Resources.Controls.ForgotPasswordControl_Send_ButtonText;
						this.btnSend.Click += ( s, e ) =>
						{
								this.capcha.Validate();
								if ( !this.capcha.IsValid ) return;

								AccountEntity account = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadByLogin { Login = this.txtInput.Text } );
								if ( account == null )
										account = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadByEmail { Email = this.txtInput.Text } );
								if ( account == null )
								{
										string script = string.Format( "alert('{0}');", Resources.Controls.ForgotPasswordControl_LoginNotExists );
										Page.ClientScript.RegisterStartupScript( this.GetType(), "ForgotPasswordControl_error", script, true );
										return;
								}

								string newPwd = capcha.CaptchaImage.Text;
								account.Password = Cryptographer.MD5Hash( capcha.CaptchaImage.Text, Salt );
								Storage<AccountEntity>.Update( account );

								EmailNotification email = new EmailNotification();
								email.Subject = Resources.Controls.ForgotPasswordControl_Email_ForgotPassword_Subject;
								email.Message = string.Format( Resources.Controls.ForgotPasswordControl_Email_ForgotPassword_Message, 
										account.Login, 
										newPwd, 
										ConfigValue( "SMTP:FromDisplay" ) );
								email.To = account.Email;
								email.Notify( true );

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

						Table table = new Table();
						table.Width = this.Width;
						table.Height = this.Height;
						table.Rows.Add( CreateTableRow( Resources.Controls.ForgotPasswordControl_LoginOrEmail_Label, this.txtInput, true ) );
						TableRow row = new TableRow();
						row.Cells.Add( new TableCell() );
						TableCell cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.capcha ); row.Cells.Add( cell );
						table.Rows.Add( row );

						//Save Cancel Buttons
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnSend );
						cell.Controls.Add( this.btnCancel );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						this.Controls.Add( table );

				}

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

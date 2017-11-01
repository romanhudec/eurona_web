using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using CMS.Controls;
using System.Web.UI;
using CMS.Entities;
using System.Drawing;
using CMS;

namespace Eurona.Controls
{
		public class AccountControlEx: CmsControl
		{
				private TextBox txtEmail;
				private CheckBox cbVerified;
				private TextBox txtVerifyCode;
				private CheckBoxList clbRoles;

				private Label lblVerifyErrorMessage;
				private CheckBox cbSendInformationEmail;

				private Button btnSave;
				private Button btnCancel;

				private Account account = null;

				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						if ( !this.AccountId.HasValue )
								return;

						this.account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = this.AccountId.Value } );

						Control control = CreateDetailControl( account );
						if ( control != null )
						{
								this.Controls.Add( control );
								DataBindControl( account );
						}
				}

				public int? AccountId
				{
						get
						{
								object o = ViewState["AccountId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["AccountId"] = value; }
				}

				private Control CreateDetailControl( Account account )
				{
						this.lblVerifyErrorMessage = new Label();
						this.lblVerifyErrorMessage.ForeColor = Color.Red;
						this.lblVerifyErrorMessage.Visible = false;
						this.lblVerifyErrorMessage.Text = Resources.Strings.AccountControl_InvalidVerifyCodeMessage;

						this.txtEmail = new TextBox();
						this.txtEmail.ID = "txtEmail";
						this.txtEmail.Width = Unit.Percentage( 80 );

						this.cbVerified = new CheckBox();
						this.cbVerified.ID = "cbVerified";
						this.cbVerified.Width = Unit.Percentage( 80 );
						this.cbVerified.Enabled = !account.Verified;
						this.cbVerified.AutoPostBack = true;
						this.cbVerified.CheckedChanged += ( s, e ) =>
								{
										if ( account.Verified )
												return;

										this.txtVerifyCode.Enabled = this.cbVerified.Checked;
								};

						this.txtVerifyCode = new TextBox();
						this.txtVerifyCode.ID = "txtVerifyCode";
						this.txtVerifyCode.Width = Unit.Percentage( 80 );
						this.txtVerifyCode.Enabled = false;

						this.clbRoles = new CheckBoxList();
						this.clbRoles.ID = "clbRoles";
						this.clbRoles.Width = Unit.Percentage( 80 );

						this.cbSendInformationEmail = new CheckBox();
						this.cbSendInformationEmail.ID = "cbSendInformationEmail";
						this.cbSendInformationEmail.Width = Unit.Percentage( 80 );

						Table table = new Table();
						table.CssClass = this.CssClass;
						table.Width = this.Width;

						//Login
						TableRow row = new TableRow();
						AddControlToRow( row, Resources.Strings.AccountControl_LabelLogin, account.Login, 0 );
						table.Rows.Add( row );

						//Email
						row = new TableRow();
						AddControlToRow( row, Resources.Strings.AccountControl_LabelEmail, this.txtEmail, 0, true, null );
						row.Cells[row.Cells.Count - 1].Controls.Add( CreateEmailValidatorControl( this.txtEmail.ID ) );
						table.Rows.Add( row );

						{
								//Verified
								row = new TableRow();
								AddControlToRow( row, Resources.Strings.AccountControl_LabelVerified, this.cbVerified, 0, false, null );
								table.Rows.Add( row );

								//Verify code
								row = new TableRow();
								AddControlToRow( row, Resources.Strings.AccountControl_LabelVerifyCode, this.txtVerifyCode, 0, false, null );
								table.Rows.Add( row );
						}

						//Roles
						row = new TableRow();
						AddControlToRow( row, Resources.Strings.AccountControl_LabelRoles, this.account.RoleString, 0 );
						table.Rows.Add( row );

						#region Dodatocne vlastnosti uctu.
						TableCell cell = null;
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( new LiteralControl( "</hr>" ) );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						row = new TableRow();
						AddControlToRow( row, Resources.Strings.AccountControl_SendInformationEmails, this.cbSendInformationEmail, 0, false, null );
						table.Rows.Add( row );
						#endregion

						//ErrorMessage
						row = new TableRow();
						cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.lblVerifyErrorMessage );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						CreateSaveButton();
						CreateCancelButton();

						TableRow trButtons = new TableRow();
						TableCell tdButtons = new TableCell();
						tdButtons.ColumnSpan = 2;
						tdButtons.Controls.Add( btnSave );
						tdButtons.Controls.Add( btnCancel );
						trButtons.Cells.Add( tdButtons );
						table.Rows.Add( trButtons );

						return table;
				}

				private void DataBindControl( Account account )
				{
						if ( this.IsPostBack )
								return;

						this.txtEmail.Text = account.Email;
						this.cbVerified.Checked = account.Verified;
						if ( account.Verified ) this.txtVerifyCode.Text = account.VerifyCode;

						this.cbSendInformationEmail.Checked = account.IsInRole( Role.NEWSLETTER );
				}

				#region Private helper methods
				private void AddControlToRow( TableRow row, string labelText, Control control, int controlColspan, bool required, ValidationDataType? valDataType )
				{
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						cell.ColumnSpan = controlColspan;
						if ( required ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );

						switch ( valDataType )
						{
								case ValidationDataType.Integer:
										cell.Controls.Add( base.CreateNumberValidatorControl( control.ID ) );
										break;
								case ValidationDataType.Double:
										cell.Controls.Add( base.CreateDoubleValidatorControl( control.ID ) );
										break;
						}

						row.Cells.Add( cell );
				}
				private void AddControlToRow( TableRow row, string labelText, string controlText, int controlColspan )
				{
						TableCell cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( new LiteralControl( controlText ) );
						cell.ColumnSpan = controlColspan;

						row.Cells.Add( cell );
				}
				#endregion

				private void CreateCancelButton()
				{
						btnCancel = new Button
						{
								Text = Resources.Strings.CancelButton_Text,
								CausesValidation = false
						};
						btnCancel.Click += ( s1, e1 ) => Response.Redirect( this.ReturnUrl );
				}
				private void CreateSaveButton()
				{
						btnSave = new Button
						{
								Text = Resources.Strings.SaveButton_Text
						};
						btnSave.Click += ( s1, e1 ) =>
						{
								if ( this.cbSendInformationEmail.Checked && !account.IsInRole( Role.NEWSLETTER ) )
										account.AddToRoles( Role.NEWSLETTER );

								if ( !this.cbSendInformationEmail.Checked && account.IsInRole( Role.NEWSLETTER ) )
										account.RemoveFromRole( Role.NEWSLETTER );

								account.Email = this.txtEmail.Text;
								Storage<Account>.Update( account );

								#region Verify account
								if ( !account.Verified && this.cbVerified.Checked )
								{
										Account.Verify verify = Storage<Account>.Execute<Account.Verify>( new Account.Verify
										{
												Account = account,
												VerifyCode = txtVerifyCode.Text
										} );
										if ( !verify.Result )
										{
												this.lblVerifyErrorMessage.Visible = true;
												return;
										}
								}
								#endregion

								Response.Redirect( this.ReturnUrl );
						};
				}

		}
}

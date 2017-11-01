using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CMS.Entities;
using PollEntity = CMS.Entities.Poll;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Configuration;

namespace CMS.Controls.ContactForm
{
		public class ContactFormControl: CmsControl
		{
				private TextBox txtName = null;
				private TextBox txtEmail = null;
				private TextBox txtPhone = null;
				private TextBox txtSubject = null;
				private TextBox txtMessage = null;

				private Button btnSend = null;
				private Label lblResult = null;

				public ContactFormControl()
				{
				}

				/// <summary>
				/// E-mail, ktory reprezentuje prijmatela spravy
				/// </summary>
				public string DestinationEmail
				{
						get
						{
								object o = ViewState["DestinationEmail"];
								return o != null ? o.ToString() : string.Empty;
						}
						set { ViewState["DestinationEmail"] = value; }
				}

				/// <summary>
				/// E-mail, ktory reprezentuje skryteho prijmatela spravy
				/// </summary>
				public string BccEmail
				{
						get
						{
								object o = ViewState["BccEmail"];
								return o != null ? o.ToString() : string.Empty;
						}
						set { ViewState["BccEmail"] = value; }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control ctrl = CreateDetailControl();
						if ( ctrl != null )
								this.Controls.Add( ctrl );

				}
				#endregion

				/// <summary>
				/// Vytvori Control Kontaktneho formulara
				/// </summary>
				private Control CreateDetailControl()
				{
						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						//this.txtName.Width = Unit.Percentage( 100 );

						this.txtEmail = new TextBox();
						this.txtEmail.ID = "txtEmail";
						//this.txtEmail.Width = Unit.Percentage( 100 );

						this.txtPhone = new TextBox();
						this.txtPhone.ID = "txtPhone";
						//this.txtPhone.Width = Unit.Percentage( 100 );

						this.txtSubject = new TextBox();
						this.txtSubject.ID = "txtSubject";
						//this.txtSubject.Width = Unit.Percentage( 100 );

						this.txtMessage = new TextBox();
						this.txtMessage.ID = "txtMessage";
						this.txtMessage.TextMode = TextBoxMode.MultiLine;
						//this.txtMessage.Width = Unit.Percentage( 100 );
						//this.txtMessage.Height = Unit.Percentage( 100 );

						this.btnSend = new Button();
						this.btnSend.CssClass = this.CssClass + "_sendButton";
						this.btnSend.CausesValidation = true;
						this.btnSend.Text = Resources.Controls.SendButton_Text;
						this.btnSend.Click += new EventHandler( OnSend );

						this.lblResult = new Label();
						this.lblResult.ID = "lblResult";

						Table table = new Table();
						table.CssClass = this.CssClass;
						//table.Attributes.Add( "border", "1" );
						table.Width = this.Width;
						table.Height = this.Height;

						//Name
						TableRow row = new TableRow();
						AddControlToRow( row, Resources.Controls.ContactFormControl_Name, this.txtName, 0, true, null );
						table.Rows.Add( row );

						//Email
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.ContactFormControl_Email, this.txtEmail, 0, true, null );
						table.Rows.Add( row );

						//Email
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.ContactFormControl_Phone, this.txtPhone, 0, false, null );
						table.Rows.Add( row );

						//Subject
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.ContactFormControl_Subject, this.txtSubject, 0, true, null );
						table.Rows.Add( row );

						//Message
						row = new TableRow();
						AddControlToRow( row, Resources.Controls.ContactFormControl_Message, this.txtMessage, 0, true, null );
						row.Cells[row.Cells.Count - 1].Height = Unit.Percentage( 100 );
						row.Cells[row.Cells.Count - 2].Height = Unit.Percentage( 100 );
						table.Rows.Add( row );

						//Save Cancel Buttons
						row = new TableRow();
						TableCell cell = new TableCell();
						cell.HorizontalAlign = HorizontalAlign.Right;
						//cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnSend );
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.HorizontalAlign = HorizontalAlign.Left;
						//cell.ColumnSpan = 2;
						cell.Controls.Add( this.lblResult );
						row.Cells.Add( cell );

						table.Rows.Add( row );

						return table;
				}

				private void AddControlToRow( TableRow row, string labelText, Control control, int controlColspan, bool required, ValidationDataType? valDataType )
				{
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						if ( control == null )
								return;

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

				void OnSend( object sender, EventArgs e )
				{
						try
						{
								using ( MailMessage msg = new MailMessage() )
								{
										bool useSSL = false;
										string strUseSSL = ConfigurationManager.AppSettings["SMTP:UseSSL"];
										if ( !string.IsNullOrEmpty( strUseSSL ) )
												Boolean.TryParse( strUseSSL, out useSSL );

										string displayFrom = this.txtPhone.Text != string.Empty ? string.Format( "{0} - Tel.:{1}", this.txtEmail.Text, this.txtPhone.Text ) : this.txtEmail.Text;
										msg.From = new MailAddress( this.txtEmail.Text, displayFrom );
										msg.To.Add( new MailAddress( this.DestinationEmail, this.DestinationEmail ) );
										if ( !string.IsNullOrEmpty( this.BccEmail ) ) msg.Bcc.Add( new MailAddress( this.BccEmail, this.BccEmail ) );
										msg.Subject = this.txtSubject.Text;
										msg.IsBodyHtml = false;
										msg.Body = this.txtMessage.Text;
										SmtpClient smtp = new SmtpClient();
										smtp.EnableSsl = useSSL;
										smtp.Send( msg );
								}


								this.txtName.Text = string.Empty;
								this.txtEmail.Text = string.Empty;
								this.txtPhone.Text = string.Empty;
								this.txtSubject.Text = string.Empty;
								this.txtMessage.Text = string.Empty;
								this.lblResult.Text = Resources.Controls.ContactFormControl_SendSuccessMessage;
						}
						catch ( Exception ex )
						{
								CMS.EvenLog.WritoToEventLog( ex );
								this.lblResult.Text = Resources.Controls.ContactFormControl_SendErrorMessage;
						}
				}
		}
}

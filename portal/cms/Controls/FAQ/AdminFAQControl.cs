using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using FAQEntinty = CMS.Entities.FAQ;
using CMSEditor = CMS.Controls.RadEditor.RadEditor;

namespace CMS.Controls.FAQ
{
		public class AdminFAQControl: CmsControl
		{
				private CMSEditor txtAnswer;
				private TextBox txtQuestion = null;
				private TextBox txtOrder = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private FAQEntinty faq = null;

				public AdminFAQControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? FAQId
				{
						get
						{
								object o = ViewState["FAQId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["FAQId"] = value; }
				}


				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control pollControl = CreateDetailControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

						//Binding
						if ( !this.FAQId.HasValue ) this.faq = new CMS.Entities.FAQ();
						else
								this.faq = Storage<FAQEntinty>.ReadFirst( new FAQEntinty.ReadById { FAQId = this.FAQId.Value } );

						if ( !IsPostBack )
						{
								this.txtQuestion.Text = this.faq.Question;
								this.txtAnswer.Content = this.faq.Answer;
								this.txtOrder.Text = this.faq.Order.ToString();
								this.DataBind();
						}

				}
				#endregion

				/// <summary>
				/// Vytvori Control FAQ
				/// </summary>
				private Control CreateDetailControl()
				{
						this.txtOrder = new TextBox();
						this.txtOrder.ID = "txtOrder";

						this.txtQuestion = new TextBox();
						this.txtQuestion.ID = "txtQuestion";
						this.txtQuestion.TextMode = TextBoxMode.MultiLine;
						this.txtQuestion.Width = Unit.Percentage( 100 );
						this.txtQuestion.Height = Unit.Pixel( 100 );

						this.txtAnswer = new CMSEditor();
						this.txtAnswer.ID = "txtAnswer";
						this.txtAnswer.Width = Unit.Percentage( 100 );
						this.txtAnswer.Height = Unit.Pixel( 400 );
						
						this.btnSave = new Button();
						this.btnSave.CausesValidation = true;
						this.btnSave.Text = Resources.Controls.SaveButton_Text;
						this.btnSave.Click += new EventHandler( OnSave );
						this.btnCancel = new Button();
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = Resources.Controls.CancelButton_Text;
						this.btnCancel.Click += new EventHandler( OnCancel );
						
						Table table = new Table();
						table.Width = this.Width;
						table.Height = this.Height;

						table.Rows.Add( CreateTableRow( Resources.Controls.AdminFAQControl_Order, this.txtOrder, true, true ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminFAQControl_Question, this.txtQuestion, true, false ) );
						table.Rows.Add( CreateTableRow( Resources.Controls.AdminFAQControl_Answer, this.txtAnswer, false, false ) );


						//Save Cancel Buttons
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.ColumnSpan = 2;
						cell.Controls.Add( this.btnSave );
						cell.Controls.Add( this.btnCancel );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						return table;
				}


				private TableRow CreateTableRow( string labelText, Control control, bool required, bool isNumber )
				{
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = required ? "form_label_required" : "form_label";
						cell.Text = labelText;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( control );
						if ( required && !(control is ASPxDatePicker)  ) cell.Controls.Add( base.CreateRequiredFieldValidatorControl( control.ID ) );
						if ( isNumber ) cell.Controls.Add( base.CreateNumberValidatorControl( control.ID ) );
						row.Cells.Add( cell );

						return row;
				}


				void OnSave( object sender, EventArgs e )
				{
						this.faq.Question = this.txtQuestion.Text;
						this.faq.Answer = this.txtAnswer.Content;
						this.faq.Order = Convert.ToInt32(this.txtOrder.Text);
						this.faq.Locale = Security.Account.Locale;

						if ( !this.FAQId.HasValue ) Storage<FAQEntinty>.Create( this.faq );
						else Storage<FAQEntinty>.Update( this.faq );

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}

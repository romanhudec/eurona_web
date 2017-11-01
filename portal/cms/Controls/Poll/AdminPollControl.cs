using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CMS.Entities;
using PollEntity = CMS.Entities.Poll;
using System.Web.UI.WebControls;

namespace CMS.Controls.Poll
{
		public class AdminPollControl: CmsControl
		{
				private TextBox txtQuestion = null;
				private ASPxDatePicker dtpDateFrom = null;
				private ASPxDatePicker dtpDateTo = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private PollEntity poll = null;

				public AdminPollControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? PollId
				{
						get
						{
								object o = ViewState["PollId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["PollId"] = value; }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control pollControl = CreateDetailControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

						//Binding
						if ( !this.PollId.HasValue ) this.poll = new CMS.Entities.Poll();
						else
								this.poll = Storage<PollEntity>.ReadFirst( new PollEntity.ReadById { PollId = this.PollId.Value } );

						if ( !IsPostBack )
						{
								this.txtQuestion.Text = this.poll.Question;
								this.dtpDateFrom.Value = this.poll.DateFrom;
								this.dtpDateTo.Value = this.poll.DateTo;
								this.DataBind();
						}

				}
				#endregion

				/// <summary>
				/// Vytvori Control Ankety
				/// </summary>
				private Control CreateDetailControl()
				{
						this.txtQuestion = new TextBox();
						this.txtQuestion.ID = "txtQuestion";
						this.txtQuestion.Width = Unit.Percentage( 100 );

						this.dtpDateFrom = new ASPxDatePicker();
						this.dtpDateFrom.ID = "dtpDateFrom";

						this.dtpDateTo = new ASPxDatePicker();
						this.dtpDateTo.ID = "dtpDateTo";

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

						//Question
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = "form_label_required";
						cell.Text = Resources.Controls.AdminPollControl_Question;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtQuestion );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtQuestion.ID ) );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Date From
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label_required";
						cell.Text = Resources.Controls.AdminPollControl_DateFrom;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.dtpDateFrom );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.dtpDateFrom.ID ) );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Date To
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.AdminPollControl_DateTo;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.dtpDateTo );
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

				void OnSave( object sender, EventArgs e )
				{
						this.poll.Question = this.txtQuestion.Text;
						this.poll.DateFrom = Convert.ToDateTime( this.dtpDateFrom.Value );
						this.poll.DateTo = (DateTime?)this.dtpDateTo.Value;
						this.poll.Locale = Security.Account.Locale;
						
						if( !this.PollId.HasValue ) Storage<PollEntity>.Create( this.poll );
						else Storage<PollEntity>.Update( this.poll );

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}

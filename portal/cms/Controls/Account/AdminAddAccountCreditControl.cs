using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using CMS.Entities;

namespace CMS.Controls.Account
{
		public class AdminAddAccountCreditControl: CmsControl
		{
				private bool isNew = false;
				private Label lblUser = null;
				private Label lblCurrentCredit = null;
				private TextBox txtCreditToAdd = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private AccountCredit accountCredit = null;

				public AdminAddAccountCreditControl()
				{
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

				#region Protected overrides
				protected override void CreateChildControls()
				{
						if ( !this.AccountId.HasValue )
								throw new InvalidOperationException( "'AccountId' property must be suplied!" );

						base.CreateChildControls();

						Control pollControl = CreateDetailControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

						//Binding
						this.accountCredit = Storage<AccountCredit>.ReadFirst( new AccountCredit.ReadByAccountId { AccountId = this.AccountId.Value } );
						if( this.accountCredit == null ){
								this.accountCredit = new AccountCredit();
								this.accountCredit.AccountId = this.AccountId.Value;
								this.isNew = true;
						}

						if ( !IsPostBack )
						{
								this.lblUser.Text = this.accountCredit.Account.Display;
								this.lblCurrentCredit.Text = this.accountCredit.Credit.ToString();
								this.txtCreditToAdd.Text = string.Empty;
								this.DataBind();
						}

				}
				#endregion


				/// <summary>
				/// Vytvori Control Ankety
				/// </summary>
				private Control CreateDetailControl()
				{
						this.lblUser = new Label();
						this.lblUser.ID = "lblUser";
						this.lblUser.Width = Unit.Pixel( 200 );
						this.lblUser.Enabled = false;

						this.lblCurrentCredit = new Label();
						this.lblCurrentCredit.ID = "lblCurrentCredit";
						this.lblCurrentCredit.Width = Unit.Pixel( 200 );
						this.lblCurrentCredit.Enabled = false;

						this.txtCreditToAdd = new TextBox();
						this.txtCreditToAdd.ID = "txtCreditToAdd";

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

						//Login
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.AdminAddAccountCreditControl_User;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.lblUser );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Current credit
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.AdminAddAccountCreditControl_CurrentCredit;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.lblCurrentCredit );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//txtCreditToAdd
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label_required";
						cell.Text = Resources.Controls.AdminAddAccountCreditControl_CreditToAdd;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtCreditToAdd );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtCreditToAdd.ID ) );
						cell.Controls.Add( CreateDoubleValidatorControl( this.txtCreditToAdd.ID ) );
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
						this.accountCredit.Credit += Convert.ToDecimal( this.txtCreditToAdd.Text );

						if ( this.isNew )Storage<AccountCredit>.Create( this.accountCredit );
						else Storage<AccountCredit>.Update( this.accountCredit );

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}

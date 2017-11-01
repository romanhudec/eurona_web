using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using CMS.Entities.Classifiers;

namespace CMS.Controls.Services
{
		public class AdminPaidServiceControl: CmsControl
		{
				private TextBox txtName = null;
				private TextBox txtCreditCost = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private PaidService paidService = null;

				public AdminPaidServiceControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? PaidServiceId
				{
						get
						{
								object o = ViewState["PaidServiceId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["PaidServiceId"] = value; }
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						if ( !this.PaidServiceId.HasValue )
								throw new InvalidOperationException( "'PaidServiceId' property must be suplied!" );

						base.CreateChildControls();

						Control pollControl = CreateDetailControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

						//Binding
						if ( !this.PaidServiceId.HasValue ) this.paidService = new PaidService();
						else
								this.paidService = Storage<PaidService>.ReadFirst( new PaidService.ReadById { PaidServiceId = this.PaidServiceId.Value } );

						if ( !IsPostBack )
						{
								this.txtName.Text = GetLocalizedSeviceName( this.paidService );
								this.txtCreditCost.Text = this.paidService.CreditCost.ToString();
								this.DataBind();
						}

				}

				protected virtual string GetLocalizedSeviceName( PaidService paidService )
				{
						return paidService.Name;
				}
				#endregion

				#region Public Styles Properties
				#endregion

				/// <summary>
				/// Vytvori Control Ankety
				/// </summary>
				private Control CreateDetailControl()
				{
						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						this.txtName.Width = Unit.Pixel( 200 );
						this.txtName.Enabled = false;

						this.txtCreditCost = new TextBox();
						this.txtCreditCost.ID = "txtCreditCost";

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

						//Name
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.CssClass = "form_label";
						cell.Text = Resources.Controls.AdminPaidServiceControl_Name;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtName );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//txtCreditCost
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label_required";
						cell.Text = Resources.Controls.AdminPaidServiceControl_CreditCost;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtCreditCost );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtCreditCost.ID ) );
						cell.Controls.Add( CreateDoubleValidatorControl( this.txtCreditCost.ID ) );
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
						this.paidService.CreditCost = Convert.ToDecimal( this.txtCreditCost.Text );
						Storage<PaidService>.Update( this.paidService );
						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}

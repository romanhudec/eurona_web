using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using CMS.Entities;

namespace CMS.Controls.Poll
{
		public class AdminOptionControl: CmsControl
		{
				private TextBox txtName = null;
				private TextBox txtOrder = null;

				private Button btnSave = null;
				private Button btnCancel = null;

				private PollOption pollOption = null;

				public AdminOptionControl()
				{
				}

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int? PollOptionId
				{
						get
						{
								object o = ViewState["PollOptionId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["PollOptionId"] = value; }
				}

				/// <summary>
				///Property musi byt vyplnena ak sa jedna o novy poll.
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
						if ( !this.PollOptionId.HasValue && !this.PollId.HasValue )
								throw new InvalidOperationException( "'PollOptionId' OR 'PollId' property must be suplied!" );

						base.CreateChildControls();

						Control pollControl = CreateDetailControl();
						if ( pollControl != null )
								this.Controls.Add( pollControl );

						//Binding
						if ( !this.PollOptionId.HasValue ) this.pollOption = new CMS.Entities.PollOption();
						else this.pollOption = Storage<PollOption>.ReadFirst( new PollOption.ReadById { PollOptionId = this.PollOptionId.Value } );

						if ( !IsPostBack )
						{
								this.txtName.Text = this.pollOption.Name;
								this.txtOrder.Text = this.pollOption.Order.HasValue ? this.pollOption.Order.ToString() : string.Empty;
								
								//Prednastavenie poradia
								if ( string.IsNullOrEmpty( this.txtOrder.Text ) )
								{
										int count = 1;
										if ( this.PollId.HasValue )
												count = 1 + Storage<PollOption>.Count( new PollOption.ReadByPollId { PollId = this.PollId.Value } );

										this.txtOrder.Text = count.ToString();
								}
								this.DataBind();
						}

				}
				#endregion


				/// <summary>
				/// Vytvori Control Ankety
				/// </summary>
				private Control CreateDetailControl()
				{
						this.txtName = new TextBox();
						this.txtName.ID = "txtName";
						this.txtName.Width = Unit.Pixel( 200 );

						this.txtOrder = new TextBox();
						this.txtOrder.ID = "txtOrder";

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

						TableRow row = null;
						TableCell cell = null;
						//Order
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label_required";
						cell.Text = Resources.Controls.AdminOptionControl_Order;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtOrder );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtOrder.ID ) );
						cell.Controls.Add( CreateNumberValidatorControl( this.txtOrder.ID ) );
						row.Cells.Add( cell );
						table.Rows.Add( row );

						//Name
						row = new TableRow();
						cell = new TableCell();
						cell.CssClass = "form_label_required";
						cell.Text = Resources.Controls.AdminOptionControl_Name;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = "form_control";
						cell.Controls.Add( this.txtName );
						cell.Controls.Add( CreateRequiredFieldValidatorControl( this.txtName.ID ) );
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
						this.pollOption.Name = this.txtName.Text;
						this.pollOption.Order = Convert.ToInt32( this.txtOrder.Text );

						bool isNew = !this.PollOptionId.HasValue;
						if ( isNew )
								this.pollOption.PollId = this.PollId.Value;

						if ( isNew ) Storage<PollOption>.Create( this.pollOption );
						else Storage<PollOption>.Update( this.pollOption );

						Response.Redirect( this.ReturnUrl );
				}

				void OnCancel( object sender, EventArgs e )
				{
						Response.Redirect( this.ReturnUrl );
				}
		}
}
